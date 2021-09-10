
Imports System.IO
'Imports System.Data
'Imports System.Data.SqlClient
Imports Microsoft.Reporting.WinForms
Imports System.Drawing.Printing
Imports System.Drawing.Imaging

Public Class FP_Print
    Public FP_ReportViewer As FP_L_ReportViewer

    Public OtherParameters As String = ""
    Public SUBREPORTS As New Dictionary(Of String, DataTable)
    Public DATASOURCES As New List(Of DataTable)

    ReadOnly FPApp As FP_App = Nothing
    ReadOnly RdlcReportName As String = ""
    ReadOnly pWindowState As Windows.Forms.FormWindowState
    Private m_currentPageIndex As Integer
    Private m_streams As IList(Of Stream)

    Public Sub New(ByVal MyFPApp As FP_App, ByVal MyRdlcReportName As String, Optional ByVal MyOtherParameters As String = "", Optional ByVal MyWindowState As Windows.Forms.FormWindowState = FormWindowState.Normal)
        InitializeComponent()

        FPApp = MyFPApp
        RdlcReportName = MyRdlcReportName
        OtherParameters = MyOtherParameters
        pWindowState = MyWindowState

        FPApp.Files_EMF_Remove()
    End Sub
    Public Sub DATASOURCES_ADD(ByVal MyDT As DataTable)
        DATASOURCES.Add(MyDT)
    End Sub

    Public Sub Add_Subreport(ByVal MySubreportName As String, ByVal MyDT As DataTable)
        SUBREPORTS.Add(MySubreportName.Replace(".rdlc", ""), MyDT)
    End Sub

    Private Sub Form_Print_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FP_ReportViewer = New FP_L_ReportViewer(FPApp) With {
            .SUBREPORTS = SUBREPORTS,
            .DATASOURCES = DATASOURCES
        }
        FP_ReportViewer.INIT(ReportViewerControl, RdlcReportName, OtherParameters)
        FP_ReportViewer.ReportViewerControl.SetDisplayMode(DisplayMode.PrintLayout)
        FP_ReportViewer.ReportViewerControl.ZoomMode = ZoomMode.PageWidth

        Me.WindowState = FormWindowState.Maximized
    End Sub

    Public Function SaveToFile(ByVal FileFormat As String, Optional ByVal FileName As String = "") As Boolean
        Dim OUT As Boolean = False

        If FileFormat <> "PDF" And FileFormat <> "Excel" Then
            FPApp.DoErrorMsgBox("FP_Print.vb..SaveToFile", 0, String.Format("Unknown format '{0}'", FileFormat))
        Else
            If FileName = "" Then
                Dim FileExtension As String = "*.*"

                Select Case FileFormat
                    Case "PDF"
                        FileExtension = "*.PDF"

                    Case "Excel"
                        FileExtension = "*.XLS"
                End Select

                FileName = FPApp.Windows_FileOpenDialogBox(FileExtension)
            End If

            If FileName > "" Then
                Dim warnings As Warning() = Nothing
                Dim streamids As String() = Nothing
                Dim mimeType As String = Nothing
                Dim encoding As String = Nothing
                Dim extension As String = Nothing
                Dim bytes As Byte()

                FP_ReportViewer = New FP_L_ReportViewer(FPApp) With {
                    .SUBREPORTS = SUBREPORTS,
                    .DATASOURCES = DATASOURCES
                }
                FP_ReportViewer.INIT(ReportViewerControl, RdlcReportName, OtherParameters)

                bytes = ReportViewerControl.LocalReport.Render(FileFormat, Nothing, mimeType, encoding, extension, streamids, warnings)

                Dim fs As New FileStream(FileName, FileMode.Create)
                Try
                    fs.Write(bytes, 0, bytes.Length)
                    fs.Close()
                    OUT = True

                Catch ex As Exception
                    FPApp.DoErrorMsgBox("FP_FieldLogics.vb..FP_L_ReportViewer.SaveToFile", Err.Number, Err.Description)
                End Try
            End If
        End If

        SaveToFile = OUT
    End Function

    Public Function SaveToPDF(ByVal FileName As String, Optional ShowIt As Boolean = False) As Boolean
        SaveToPDF = FP_ReportViewer.SaveToPDF(FileName, ShowIt)
    End Function

    Public Function SaveToEXCEL(ByVal FileName As String, Optional ShowIt As Boolean = False) As Boolean
        SaveToEXCEL = FP_ReportViewer.SaveToEXCEL(FileName, ShowIt)
    End Function

    Public Function Print(Optional ByVal PFDKey_for_Printer As String = "PRINTER_LAST_SELECTED", Optional ByVal DocumentName As String = "", Optional ByVal WithPrintDialog As Boolean = False, Optional ByVal TryAgainDialog As Boolean = False, Optional ByVal OtherParameters As String = "", Optional Selected_Printer_Name As String = "", Optional ByRef out_bytes As Byte() = Nothing)
        Dim OUT As Boolean = False
        Dim warnings As Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim bytes As Byte()
        Dim DoIt As Boolean = True
        Dim PDoc As New Printing.PrintDocument
        Dim Save_Printer_To_PFD As Boolean = True
        Dim PFD_Printer_Name As String = ""
        Dim Printer_OK As Boolean = False

        CURSOR_SHOW_WAIT()

        FP_ReportViewer = New FP_L_ReportViewer(FPApp) With {
            .SUBREPORTS = SUBREPORTS,
            .DATASOURCES = DATASOURCES
        }
        FP_ReportViewer.INIT(ReportViewerControl, RdlcReportName, OtherParameters)

        bytes = ReportViewerControl.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
        out_bytes = bytes
        ExportReport("Image")
        CURSOR_SHOW_DEFAULT()

        Dim oPS As New System.Drawing.Printing.PrinterSettings
        Dim SelectedPrinter As String = ""

        FPApp.PFDlesen(PFDKey_for_Printer, PFD_Printer_Name)

        If PFD_Printer_Name.ToUpper = "#_QUESTION_#" Then
            Save_Printer_To_PFD = False
        End If

        If Selected_Printer_Name <> String.Empty Then
            PDoc.PrinterSettings.PrinterName = Selected_Printer_Name
            If PDoc.PrinterSettings.IsValid Then
                SelectedPrinter = Selected_Printer_Name

                Printer_OK = True
            End If
        End If

        If Not Printer_OK Then
            PDoc.PrinterSettings.PrinterName = PFD_Printer_Name
            If PDoc.PrinterSettings.IsValid Then
                SelectedPrinter = PFD_Printer_Name
                Printer_OK = True
            End If
        End If

        If Not Printer_OK Then
            SelectedPrinter = ""
            WithPrintDialog = True
        End If

        If WithPrintDialog Then
            Dim PDialog As New PrintDialog

            PDialog.PrinterSettings.PrinterName = SelectedPrinter

            Dim F As Form = FPApp.ShowDialogForm_getOpacityForm(Form.ActiveForm)
            DoIt = (PDialog.ShowDialog() = DialogResult.OK)
            If Not (F Is Nothing) Then
                Try
                    F.Close()

                Catch ex As Exception
                    'Nothing to do
                End Try
            End If
            If DoIt Then
                SelectedPrinter = PDialog.PrinterSettings.PrinterName
            End If
        End If

        If DoIt Then
            If SelectedPrinter = "" Then
                Try
                    SelectedPrinter = oPS.PrinterName

                Catch ex As Exception
                    DoIt = False
                End Try
            End If
        End If

        If DoIt Then
            If PFDKey_for_Printer > "" Then
                If Save_Printer_To_PFD Then
                    FPApp.PFDinsertOrUpdate(PFDKey_for_Printer, SelectedPrinter)
                End If
            End If

            PDoc.PrinterSettings.PrinterName = SelectedPrinter

            If DocumentName.Trim = "" Then
                DocumentName = "Document"
            End If
            PDoc.DocumentName = DocumentName

            AddHandler PDoc.PrintPage, New PrintPageEventHandler(AddressOf PrintPage)
            PDoc.Print()
            OUT = True
        End If

        Print = OUT
    End Function

    Private Sub ExportReport(ByVal ExportType As String)
        Dim deviceInfo As String = _
"  <DeviceInfo>" + _
"  <OutputFormat>EMF</OutputFormat>" + _
"  <PageWidth>#_PAGE_WIDTH_#</PageWidth>" + _
"  <PageHeight>#_PAGE_HEIGHT_#</PageHeight>" + _
"  <MarginTop>#_MARGIN_TOP_#</MarginTop>" + _
"  <MarginLeft>#_MARGIN_LEFT_#</MarginLeft>" + _
"  <MarginRight>#_MARGIN_RIGHT_#</MarginRight>" + _
"  <MarginBottom>#_MARGIN_BOTTOM_#</MarginBottom>" + _
"</DeviceInfo>"

        With ReportViewerControl.LocalReport.GetDefaultPageSettings
            deviceInfo = Strings.Replace(deviceInfo, "#_PAGE_WIDTH_#", .PaperSize.Width)
            deviceInfo = Strings.Replace(deviceInfo, "#_PAGE_HEIGHT_#", .PaperSize.Height)

            deviceInfo = Strings.Replace(deviceInfo, "#_MARGIN_TOP_#", .Margins.Top)
            deviceInfo = Strings.Replace(deviceInfo, "#_MARGIN_LEFT_#", .Margins.Left)
            deviceInfo = Strings.Replace(deviceInfo, "#_MARGIN_RIGHT_#", .Margins.Right)
            deviceInfo = Strings.Replace(deviceInfo, "#_MARGIN_BOTTOM_#", .Margins.Bottom)
        End With

        Dim warnings As Warning() = Nothing
        Try
            m_streams = New List(Of Stream)()
            ReportViewerControl.LocalReport.Render(ExportType, deviceInfo, AddressOf CreateStream, warnings)
            For Each stream As Stream In m_streams
                stream.Position = 0
            Next
        Catch ex As Exception
            Call FPApp.DoErrorMsgBox("FP_FieldLogics.vb..ExportReport", Err.Number, Err.Description)
        End Try
    End Sub
    Private Function CreateStream(ByVal name As String, ByVal fileNameExtension As String, ByVal encoding As System.Text.Encoding, ByVal mimeType As String, ByVal willSeek As Boolean) As Stream
        Dim FileNum As Integer = FPApp.NachsteNummerVergeben
        Dim stream As Stream = New FileStream(name & FileNum & "." & fileNameExtension, FileMode.Create)
        m_streams.Add(stream)
        Return stream
    End Function

    Private Sub PrintPage(ByVal sender As Object, ByVal ev As PrintPageEventArgs)
        Dim pageImage As New Metafile(m_streams(m_currentPageIndex))
        ev.Graphics.DrawImage(pageImage, ev.PageBounds)
        m_currentPageIndex += 1
        ev.HasMorePages = (m_currentPageIndex < m_streams.Count)
    End Sub
End Class

