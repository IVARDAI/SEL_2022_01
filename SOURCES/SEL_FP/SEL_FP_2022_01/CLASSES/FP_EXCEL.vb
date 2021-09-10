Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Office.Interop.Excel

Public Class FP_XLS
    Public FPApp As FP_App

    Public XLApp As Application = Nothing
    Public ExcelFileName As String = ""
    Public SheetName As String = ""
    Public WorkBookName As String = ""
    Private Disposed As Boolean = False

    Public WriteOnly Property P_ExcelVisible As Boolean
        Set(value As Boolean)
            XLApp.Visible = value
        End Set
    End Property

    Public ReadOnly Property P_IsExcelOpen() As Boolean
        Get
            Dim OUT As Boolean = True

            If XLApp Is Nothing Then
                OUT = False
            Else
                Try
                    Dim wchar As String = XLApp.Name

                Catch ex As Exception
                    XLApp = Nothing
                    OUT = False
                End Try
            End If

            P_IsExcelOpen = OUT
        End Get
    End Property

    Sub New(ByVal MyFPApp As FP_App)
        FPApp = MyFPApp
    End Sub

    Public Sub Dispose()
        If Not (XLApp Is Nothing) Then
            XLApp.Visible = True
            XLApp.Quit()
            XLApp = Nothing
            KillUnusedExcelProcess()
        End If

        Disposed = True
    End Sub

    Private Sub KillUnusedExcelProcess()
        Dim oXlProcess As Process() = Process.GetProcessesByName("Excel")

        For Each oXLP As Process In oXlProcess
            If Len(oXLP.MainWindowTitle) = 0 Then
                oXLP.Kill()
            End If
        Next
    End Sub

    Sub CLOSE()
        If P_IsExcelOpen Then
            Try
                XLApp.Visible = True
                Do While XLApp.Workbooks.Count > 0
                    XLApp.Workbooks(1).Close(False)
                Loop
                XLApp.Quit()

            Catch ex As Exception
                'Nothing to do
            End Try
            ReleaseObject(XLApp)
            XLApp = Nothing

            KillUnusedExcelProcess()
        End If
    End Sub

    Function OPEN(ByVal MyExcelFileName As String, ByVal MySheet As String) As Boolean
        Dim OUT As Boolean = True

        SheetName = ""

        If MyExcelFileName = "" Then
            OUT = False
            FPApp.DoErrorMsgBox("FP_EXCEL.OPEN", 0, "MyExcelFileName = ''")
        Else
            WorkBookName = getFileNameFromFullName(MyExcelFileName)

            If P_IsExcelOpen = False Then
                Try
                    XLApp = New Application

                Catch ex As Exception
                    OUT = False
                    FPApp.DoErrorMsgBox("FP_EXCEL.OPEN", Err.Number, Err.Description)
                End Try
            End If

            If OUT Then
                Dim WorkbookActivated As Boolean = False

                If XLApp.Workbooks.Count > 0 Then
                    For i As Integer = 1 To XLApp.Workbooks.Count
                        With XLApp.Workbooks(i)
                            If .Name = WorkBookName Then
                                XLApp.Workbooks(WorkBookName).Activate()
                                WorkbookActivated = True
                            End If
                        End With
                    Next
                End If

                If Not WorkbookActivated Then
                    Try
                        XLApp.Workbooks.Open(MyExcelFileName)
                        XLApp.Workbooks(WorkBookName).Activate()
                        WorkbookActivated = True

                    Catch ex As Exception
                        OUT = False
                        FPApp.DoMyMsgBox(805, MyExcelFileName)
                        FPApp.DoErrorMsgBox("FP_EXCEL.OPEN", Err.Number, Err.Description)
                    End Try
                End If

                If WorkbookActivated Then
                    If Trim(MySheet) = "" Then
                        If XLApp.Workbooks(WorkBookName).Sheets.Count > 0 Then
                            MySheet = XLApp.Workbooks(WorkBookName).Sheets(1).Name
                        End If
                    End If

                    If MySheet = "" Then
                        OUT = False
                        Call FPApp.DoMyMsgBox(879, WorkBookName)
                    End If

                    If OUT = True Then
                        Try
                            XLApp.Workbooks(WorkBookName).Sheets(MySheet).Activate()
                            SheetName = MySheet

                        Catch ex As Exception
                            OUT = False
                            Call FPApp.DoMyMsgBox(807, MySheet)
                        End Try
                    End If
                End If
            End If
        End If

        OPEN = OUT

    End Function
End Class

Public Class FP_XLS_Import
    Public Structure Struct_FP_XLS_Import
        Dim FPf As FP_Form
        Dim DATA_FP As FP 'Az az FP, ami ala be kell olvasni a rekordokat. Lehet nothing
        Dim ExcelImportCode As String
        Dim FileName As String      'Optional
    End Structure

    Public Event Data_Records_Prepared(ByVal sender As FP_XLS_Import, ByRef Cancel As Boolean)
    Public Event Check_Data(ByVal sender As FP_XLS_Import)
    Public Event Import_Data(ByVal sender As FP_XLS_Import, ByRef Cancel As Boolean)

    Public ReadOnly ExcelImportCode As String
    Public FPf As FP_Form = Nothing
    Public DATA_FP As FP 'Az az FP, ami ala be kell olvasni a rekordokat. Lehet nothing
    Public XLS As FP_XLS = Nothing
    Public FileName As String = ""
    Public DIC_ImportDef As New Dictionary(Of String, String)
    Public ExcelFirstRow As Long = 0
    Public ExcelLastRow As Long = 0
    Public WithEvents Form_DataImport As FP_DataImport = Nothing
    Public FileType As String = ""
    Public STEP_1_ZDISPO_Identifier As String = ""
    Public STEP_2_StoredProcedure_CHK As String = ""
    Public STEP_3_StoredProcedure_LoadData As String = ""
    Public STEP_4_ZDISPO_Identifier As String = ""

    Private WorksheetName As String = ""
    Private ExcelFileName As String = ""
    Private ImportDef As String = ""
    Private Disposed As Boolean = False

    Function PFD_Key() As String
        PFD_Key = "EXCELIMPORT_" & UCase(ExcelImportCode) & "_PATH"
    End Function

    Sub New(P As Struct_FP_XLS_Import)
        Dim ImportDef_Key As String = "ExcelImportDef_" & P.ExcelImportCode

        With P
            FPf = .FPf
            DATA_FP = .DATA_FP
            ExcelImportCode = .ExcelImportCode
            FileName = .FileName
        End With

        ImportDef = gl_FPApp.getFixText(ImportDef_Key)

        If ImportDef = "" Then
            gl_FPApp.DoErrorMsgBox("FP_EXCEL.FP_XLS_Import.New", 0, String.Format("Excel import definition missing. (FixText key: '{0}')", ImportDef_Key))
        Else
            If gl_FPApp.FIXTEXT_SPLIT_PARAMS(ImportDef, DIC_ImportDef) Then
                Dim ParamsOK As Boolean = True

                ParamsOK = (ParamsOK And gl_FPApp.FIXTEXT_CHK_PARAM(DIC_ImportDef, ImportDef_Key, "FILETYPE"))

                If ParamsOK Then
                    FileType = gl_FPApp.FIXTEXT_getParam("FILETYPE", DIC_ImportDef)

                    Select Case FileType
                        Case "XLS", "CSV"
                            XLS = New FP_XLS(gl_FPApp)

                            WorksheetName = gl_FPApp.FIXTEXT_getParam("SHEETNAME", DIC_ImportDef)
                            STEP_1_ZDISPO_Identifier = gl_FPApp.FIXTEXT_getParam("STEP_1_ZDISPO_IDENTIFIER", DIC_ImportDef)
                            STEP_2_StoredProcedure_CHK = gl_FPApp.FIXTEXT_getParam("STEP_2_STOREDPROCEDURE_CHK", DIC_ImportDef)
                            STEP_3_StoredProcedure_LoadData = gl_FPApp.FIXTEXT_getParam("STEP_3_STOREDPROCEDURE_LOADDATA", DIC_ImportDef)
                            STEP_4_ZDISPO_Identifier = gl_FPApp.FIXTEXT_getParam("STEP_4_ZDISPO_IDENTIFIER", DIC_ImportDef)

                        Case Else
                            gl_FPApp.DoErrorMsgBox("FP_XLS_Import", 0, String.Format("Unknown filetype '{0}'", FileType))
                    End Select
                End If
            End If
        End If
    End Sub

    Sub Dispose()
        DIC_ImportDef.Clear()
        If Not (XLS Is Nothing) Then
            XLS.Dispose()

            XLS = Nothing
        End If

        If Not (Form_DataImport Is Nothing) Then
            Form_DataImport.Dispose()
            Form_DataImport = Nothing
        End If

        Disposed = True
    End Sub

    Public ReadOnly Property P_ExcelFilename() As String
        Get
            Return ExcelFileName
        End Get
    End Property

    Function CELL_GET(ByVal RowNum As Integer, ByVal ColumnNum As Integer) As String
        Dim OUT As String = ""

        If XLS Is Nothing Then
            gl_FPApp.DoErrorMsgBox("FP_XLS_Import.CELL_GET", 0, "XLS is nothing.")
        Else
            Try
                With XLS.XLApp.Workbooks(XLS.WorkBookName).Sheets(XLS.SheetName)
                    If Not (.Cells(RowNum, ColumnNum).Value Is Nothing) Then
                        OUT = .Cells(RowNum, ColumnNum).Value
                    End If
                End With

            Catch ex As Exception
                gl_FPApp.DoErrorMsgBox("FP_XLS_Import.CELL_GET", Err.Number, Err.Description)
            End Try
        End If

        CELL_GET = OUT
    End Function

    Function File_Open() As Boolean
        Dim OUT As Boolean = False
        Dim DefaultPath As String = ""

        Dim FilterString As String = gl_FPApp.FIXTEXT_getParam("FILTERSTRING", DIC_ImportDef)

        Select Case FileType

            Case "XLS"
                If Not (XLS Is Nothing) Then
                    XLS.CLOSE()

                    If ExcelFileName = "" Then
                        gl_FPApp.PFDlesen(PFD_Key, DefaultPath)

                        If FilterString = "" Then
                            FilterString = "Excel (*.xlsx)|*.xlsx|Excel 97-2003 (*.xls)|*.xls"
                        End If

                        ExcelFileName = gl_FPApp.Windows_FileOpenDialogBox(FilterString, DefaultPath, "SEQ_EXCELFILES_IMPORT")

                        If ExcelFileName > "" Then
                            gl_FPApp.PFDinsertOrUpdate(PFD_Key, getDirectoryFromFullName(ExcelFileName))
                        End If
                    End If

                    If ExcelFileName > "" Then
                        OUT = XLS.OPEN(ExcelFileName, WorksheetName)
                    End If
                End If

            Case "CSV"
                If FilterString = "" Then
                    FilterString = "csv (*.csv)|*.csv"
                End If
                ExcelFileName = gl_FPApp.Windows_FileOpenDialogBox(FilterString, DefaultPath, "SEQ_EXCELFILES_IMPORT")

                If ExcelFileName > "" Then
                    gl_FPApp.PFDinsertOrUpdate(PFD_Key, getDirectoryFromFullName(ExcelFileName))
                    OUT = True
                End If

            Case Else
                gl_FPApp.DoErrorMsgBox("FP_EXCEL.FP_XLS_Import", 0, String.Format("Unknown file format {0}", FileType))
        End Select

        File_Open = OUT
    End Function

    Public Function ImportData() As Boolean
        Dim OUT As Boolean = True
        Dim FPf_Enabled_Old As Boolean = True

        If Not (FPf Is Nothing) Then
            FPf_Enabled_Old = FPf.P_ENABLED
        End If

        KillUnusedExcelProcess()

        OUT = File_Open()

        If OUT Then
            Dim UsedRange As Range = XLS.XLApp.Worksheets(XLS.SheetName).UsedRange

            ExcelFirstRow = Val(gl_FPApp.FIXTEXT_getParam("FirstRowInExcel", DIC_ImportDef))
            If ExcelFirstRow = 0 Then
                ExcelFirstRow = 1
            End If

            Try

                ExcelLastRow = UsedRange.Rows.Count

            Catch ex As Exception
                OUT = False
                gl_FPApp.DoErrorMsgBox("FP_EXCEL.FP_XLS_Import.ImportDataWithForm", Err.Number, Err.Description)
            End Try

            If ExcelLastRow < ExcelFirstRow Then
                OUT = False
                gl_FPApp.DoMyMsgBox(1301) 'A megadott file ures
            Else
                Dim ProgressForm As New FP_ProgressForm(FPf.FPApp, "", 0, ExcelLastRow)

                If Not (FPf Is Nothing) Then
                    FPf.P_ENABLED = False
                End If

                ProgressForm.Show()

                'CURSOR_SHOW_WAIT()

                Dim ExcelData As String = ""

                For i As Integer = 1 To UsedRange.Rows.Count
                    Dim CurrentRow As String = ""

                    Dim ProgressForm_Text As String = String.Format("{0} / {1}", i, ExcelLastRow)
                    ProgressForm.SET_VALUES(i, ProgressForm_Text)

                    If ProgressForm.CancelPressed Then
                        ProgressForm.Close()
                        ProgressForm.Dispose()
                        ProgressForm = Nothing

                        If Not (FPf Is Nothing) Then
                            FPf.P_ENABLED = FPf_Enabled_Old
                        End If

                        Return False
                    End If

                    With XLS.XLApp.Workbooks(XLS.WorkBookName).Sheets(XLS.SheetName)
                        For k = 1 To UsedRange.Columns.Count
                            Dim CurrentValue As String = .Cells(i, k).Value

                            CurrentValue = Replace(CurrentValue, vbCrLf, " ")
                            CurrentValue = Replace(CurrentValue, vbCr, " ")
                            CurrentValue = Replace(CurrentValue, vbLf, " ")

                            CurrentRow += CurrentValue + "|#_#|"
                        Next

                        ExcelData += CurrentRow + "|#_CR_#|"
                    End With
                Next

                ProgressForm.Close()
                ProgressForm.Dispose()
                ProgressForm = Nothing

                Dim SplashForm As New FP_L_SplashForm(FPf.Frm, "Working...")

                Dim sqlComm As SqlCommand = gl_FPApp.DC.CNN.CreateCommand()

                With gl_FPApp.DC
                    .Qdf_set_SP(sqlComm, "Excel_Import_DataFill", 0)
                    .Qdf_AddParameter(sqlComm, "@VERS", SqlDbType.NVarChar, , 5, "2.0")
                    .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                    .Qdf_AddParameter(sqlComm, "@Data", SqlDbType.NVarChar, , -1, ExcelData)
                    .Qdf_AddParameter(sqlComm, "@FirstRowInExcel", SqlDbType.Int, , , , , ExcelFirstRow)
                End With

                CURSOR_SHOW_WAIT()
                Try
                    OUT = gl_FPApp.DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE)

                Catch ex As Exception
                    OUT = False
                    gl_FPApp.DoErrorMsgBox("FP_EXCEL.FP_XLS_Import.ImportDataWithForm", Err.Number, Err.Description)
                End Try

                SplashForm.CloseSplashForm()

                If Not (FPf Is Nothing) Then
                    FPf.P_ENABLED = FPf_Enabled_Old
                End If

                'CURSOR_SHOW_DEFAULT()

                If STEP_1_ZDISPO_Identifier > "" Then
                    Dim STEP_1_ZDISPO_P As New Struct_ZDISPO_Params

                    With STEP_1_ZDISPO_P
                        .Identifier = STEP_1_ZDISPO_Identifier
                    End With

                    OUT = gl_FPApp.ZDISPO(STEP_1_ZDISPO_P, Nothing)
                End If
            End If
        End If

        Return OUT
    End Function


    Public Function ImportDataWithForm() As Boolean
        Dim OUT As Boolean = False

        If FileType = "XLS" Then
            OUT = ImportData()
        End If

        If FileType = "CSV" Then
            Dim Separator As String = gl_FPApp.FIXTEXT_getParam("SEPARATOR", DIC_ImportDef)

            OUT = Import_CSV_Data(Separator)
        End If

        If OUT Then
            Dim DataImport_Params As New FP_DataImport.Struct_FP_DataImport_Params

            With DataImport_Params
                .DataImport_ServerObject_Prefix = "EXCEL_IMPORT"
                .DataImport_SubPrefix = ExcelImportCode
                .DATA_FP = DATA_FP
                .STEP_1_ZDISPO_Identifier = STEP_1_ZDISPO_Identifier
                .STEP_2_StoredProcedure_CHK = STEP_2_StoredProcedure_CHK
                .STEP_3_StoredProcedure_LoadData = STEP_3_StoredProcedure_LoadData
                .STEP_4_ZDISPO_Identifier = STEP_4_ZDISPO_Identifier
            End With

            Form_DataImport = New FP_DataImport(DataImport_Params)
            With Form_DataImport.FP_L.SQL_BIND_Params
                .NameOf_DEL = "EXCEL_IMPORT_DEL"
                .NameOf_GRID = "EXCEL_IMPORT_GRID"
                .NameOf_READ = "EXCEL_IMPORT_READ"
                .NameOf_SAVE = "EXCEL_IMPORT_SAVE"
                .NameOf_WhereQuery = "EXCEL_IMPORT_WhereQuery"
            End With

            OUT = (gl_FPApp.ShowDialogForm(Form_DataImport) = DialogResult.OK)
        End If

        Return OUT
    End Function

    Private Sub Form_DataImport_Check_Data(ByVal sender As FP_DataImport) Handles Form_DataImport.Check_Data
        RaiseEvent Check_Data(Me)
    End Sub

    Private Sub MyForm_DataImport_Data_Records_Prepared(ByVal sender As FP_DataImport, ByRef Cancel As Boolean) Handles Form_DataImport.Data_Records_Prepared
        RaiseEvent Data_Records_Prepared(Me, Cancel)
        XLS.CLOSE()
    End Sub

    Private Sub Form_DataImport_Import_Data(ByVal sender As FP_DataImport, ByRef Cancel As Boolean) Handles Form_DataImport.Import_Data
        RaiseEvent Import_Data(Me, Cancel)
    End Sub

    Private Sub Kill_Excel_Process(Optional ExcelTitle As String = "")
        Dim oXlProcess As Process() = Process.GetProcessesByName("Excel")

        For Each oXLP As Process In oXlProcess
            If Len(oXLP.MainWindowTitle) = 0 Then
                oXLP.Kill()
            End If

            If oXLP.MainWindowTitle.ToUpper = ExcelTitle.ToUpper Then
                oXLP.Kill()
            End If
        Next

    End Sub

    Private Function CheckFile() As Boolean
        Dim ExcelTitle As String = ""
        Dim sPath() As String
        Dim sLen As Integer

        sPath = ExcelFileName.Split("\")
        sLen = sPath.Length - 1
        ExcelTitle = sPath(sLen) & " - Excel"

        Dim RET As Boolean = True

        If Not System.IO.File.Exists(ExcelFileName) Then
            RET = False
        End If

        If RET Then
            Dim fs As System.IO.FileStream

            Try
                fs = System.IO.File.OpenRead(ExcelFileName)
                fs.Close()

            Catch ex As System.IO.IOException
                'A fájl nyitva van, megpróbálja bezárni
                Kill_Excel_Process(ExcelTitle)

            Catch ex As Exception
                RET = False
            End Try
        End If

        Return RET
    End Function

    Private Sub KillUnusedExcelProcess()
        Dim oXlProcess As Process() = Process.GetProcessesByName("Excel")

        For Each oXLP As Process In oXlProcess
            If Len(oXLP.MainWindowTitle) = 0 Then
                oXLP.Kill()
            End If
        Next
    End Sub

    Public Function Import_CSV_Data(Optional ByVal Separator As String = ";") As Boolean
        Dim OUT As Boolean = True
        Dim Your_DT As New System.Data.DataTable
        Dim MyRow As String
        Dim MyCode As System.Text.Encoding = System.Text.Encoding.GetEncodings()(41).GetEncoding
        Dim ItemColumnCount As Integer

        KillUnusedExcelProcess()

        OUT = File_Open()

        If OUT Then
            OUT = CheckFile()
        End If

        If OUT Then
            Dim items = (From line In IO.File.ReadAllLines(ExcelFileName, MyCode) _
                                     Select Array.ConvertAll(line.Split(Separator), _
                                    Function(v) v.ToString.TrimStart(""" ".ToCharArray).TrimEnd(""" ".ToCharArray))).ToArray

            ItemColumnCount = items(0).GetUpperBound(0) + 1

            For x As Integer = 0 To items(0).GetUpperBound(0)
                Your_DT.Columns.Add()
            Next

            For Each a In items
                If a.Length = ItemColumnCount Then
                    Dim dr As DataRow = Your_DT.NewRow

                    dr.ItemArray = a
                    Your_DT.Rows.Add(dr)
                End If
            Next

            CURSOR_SHOW_WAIT()

            Dim ExcelData As String = ""
            Dim DRow As System.Data.DataRow

            For Each DRow In Your_DT.Rows
                MyRow = ""
                For k = 0 To Your_DT.Columns.Count - 1
                    Dim CurrentValue As String = DRow.Item(k)

                    CurrentValue = Replace(CurrentValue, vbCrLf, " ")
                    CurrentValue = Replace(CurrentValue, vbCr, " ")
                    CurrentValue = Replace(CurrentValue, vbLf, " ")

                    MyRow += CurrentValue + "|#_#|"
                Next

                ExcelData += MyRow + "|#_CR_#|"
            Next

            Dim sqlComm As SqlCommand = gl_FPApp.DC.CNN.CreateCommand()

            With gl_FPApp.DC
                .Qdf_set_SP(sqlComm, "Excel_Import_DataFill")
                .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                .Qdf_AddParameter(sqlComm, "@Data", SqlDbType.NVarChar, , -1, ExcelData)
                .Qdf_AddParameter(sqlComm, "@FirstRowInExcel", SqlDbType.Int, , , , , ExcelFirstRow)
            End With

            CURSOR_SHOW_WAIT()

            Try
                OUT = gl_FPApp.DC.Qdf_Execute(FPf, sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE)

            Catch ex As Exception
                OUT = False
                gl_FPApp.DoErrorMsgBox("FP_EXCEL.FP_XLS_Import.ImportDataWithForm", Err.Number, Err.Description)
            End Try

            CURSOR_SHOW_DEFAULT()
        End If

        Return OUT
    End Function
End Class

Public Class FP_XLS_Export
    Public FPApp As FP_App
    Public Parent_FPf As FP_Form
    Public DT As New System.Data.DataTable
    Public FileName As String = ""
    Public Identifier As String
    Public PFD_KEY_FILENAME As String = ""
    Public Excel_Export_Def As String = ""
    Public DIC_ExportDef As New Dictionary(Of String, String)
    Private Field_Names_To_Header As Boolean = True
    Private Dict_Excel_Col As Dictionary(Of Integer, String)


    Public Sub New(ByVal MyFPApp As FP_App)
        FPApp = MyFPApp
    End Sub

    Public Sub New(ByVal MyFPApp As FP_App, ByVal MyDT As System.Data.DataTable, Optional ByVal MyIdentifier As String = "", Optional ByVal MyFileName As String = "")
        FPApp = MyFPApp
        Identifier = MyIdentifier
        If Identifier > "" Then
            PFD_KEY_FILENAME = "EXCELEXPORT_" + MyIdentifier + "_FILENAME"
        End If
        DT = MyDT
        FileName = MyFileName

        If Fill_Excel_Export_Def() Then
            Check_Excel_Export_Def()
        End If
    End Sub

    Public Sub New(ByVal MyFPApp As FP_App, ByVal MyParent_FPf As FP_Form)
        FPApp = MyFPApp
        Parent_FPf = MyParent_FPf
    End Sub

    Public Sub New(ByVal MyFPApp As FP_App, ByVal MyParent_FPf As FP_Form, ByVal MyDT As System.Data.DataTable, Optional ByVal MyIdentifier As String = "", Optional ByVal MyFileName As String = "")
        FPApp = MyFPApp
        Parent_FPf = MyParent_FPf
        Identifier = MyIdentifier
        If Identifier > "" Then
            PFD_KEY_FILENAME = "EXCELEXPORT_" + MyIdentifier + "_FILENAME"
        End If
        DT = MyDT
        FileName = MyFileName

        If Fill_Excel_Export_Def() Then
            Check_Excel_Export_Def()
        End If
    End Sub
    Private Sub Fill_Excel_Col_Dict()
        Dim Cols As String = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,AA,AB,AC,AD,AE,AF,AG,AH,AI,AJ,AK,AL,AM,AN,AO,AP,AQ,AR,AS,AT,AU,AV,AW,AX,AY,AZ,BA,BB,BC,BD,BE,BF,BG,BH,BI,BJ,BK,BL,BM,BN,BO,BP,BQ,BR,BS,BT,BU,BV,BW,BX,BY,BZ"
        Dim ARR() As String
        Dim C As String
        Dim i As Integer = 0
        ARR = Cols.Split(",")
        Dict_Excel_Col = New Dictionary(Of Integer, String)
        For Each C In ARR
            Dict_Excel_Col.Add(i, C)
            i += 1
        Next
    End Sub

    Private Function Fill_Excel_Export_Def() As Boolean
        Fill_Excel_Col_Dict()
        Dim OUT As Boolean = False
        Dim ExportDef_Key As String = "EXCELEXPORTDEF_" & Identifier
        Excel_Export_Def = gl_FPApp.getFixText(ExportDef_Key)

        If gl_FPApp.FIXTEXT_SPLIT_PARAMS(Excel_Export_Def, DIC_ExportDef) Then
            OUT = True
        End If
        Return OUT
    End Function
    Private Sub Check_Excel_Export_Def()
        Dim s_Field_Names_To_Header As String = gl_FPApp.FIXTEXT_getParam("FELD_NAMES_TO_HEADER", DIC_ExportDef)
        If s_Field_Names_To_Header.ToUpper = "NO" Then
            Field_Names_To_Header = False
        End If
    End Sub

    Public Sub ExportData(Optional ByVal ShowFileSelectDialog As Boolean = False, Optional FieldNames_To_Header As Boolean = True)
        If DT Is Nothing Then
            FPApp.DoErrorMsgBox("FP_XLS_Export.ExportData", 0, "DT is nothing.")
        Else
            If DT.Rows.Count < 1 Then
                FPApp.DoMyMsgBox(876) 'Nincsen exportalhato adat
            Else
                Dim DoIt As Boolean = True

                If FileName = "" And ShowFileSelectDialog Then
                    If FileName = "" Then
                        If PFD_KEY_FILENAME > "" Then
                            FPApp.PFDlesen(PFD_KEY_FILENAME, FileName)
                        End If
                    End If
                    FileName = FPApp.Windows_FileOpenDialogBox("SEQ##SEQ_EXCEL_FILES,1##END|*.xls", , "SEQ_EXCELFILES_EXPORT", FileName)
                    If FileName = "" Then
                        DoIt = False
                    End If
                End If

                If DoIt Then
                    Dim Excel As New Microsoft.Office.Interop.Excel.Application

                    Dim wBook = Excel.Workbooks.Add()
                    Dim wSheet = wBook.ActiveSheet()
                    Dim w As Integer = 0

                    CURSOR_SHOW_WAIT()

                    Dim DataArray(DT.Rows.Count, DT.Columns.Count - 1) As Object
                    If Field_Names_To_Header Then
                        w = 1
                        For colindex As Integer = 0 To DT.Columns.Count - 1
                            DataArray(0, colindex) = DT.Columns(colindex).ColumnName
                        Next
                    End If

                    For rowindex As Integer = 0 To DT.Rows.Count - 1
                        For colindex As Integer = 0 To DT.Columns.Count - 1
                            DataArray(rowindex + w, colindex) = DT.Rows(rowindex).Item(colindex)
                        Next
                    Next

                    'UPS-nel hibat dobott, az elem nem talalhato a szotarban, ezert kivettem - VI 20210712
                    'SET Column number format
                    'For Colindex As Integer = 0 To DT.Columns.Count - 1
                    '    Dim NF As String = ""
                    '    Debug.Print(DT.Columns(Colindex).ColumnName)
                    '    Select Case DT.Columns(Colindex).DataType.FullName.ToUpper
                    '        Case "SYSTEM.INT16", "SYSTEM.INT32", "SYSTEM.INT64"
                    '            NF = "0"
                    '        Case "SYSTEM.STRING"
                    '            NF = "@"
                    '        Case "SYSTEM.DOUBLE", "SYSTEM.SINGLE"
                    '            NF = "#.##0"
                    '        Case "SYSTEM.DATETIME"
                    '            'nothing to do          -- egyelore az Excel-re bizom a datum formatum megallapitasat es formazasat
                    '        Case Else
                    '            NF = "@"
                    '    End Select
                    '    NF = ""     '!!!SSS!!! Apollo excel export eseteben minden mezo nvarchar az eredmeny tablaban, ezert a numerikus ertekek is szovegkent mennek at Excel-be
                    '    wSheet.Columns(Dict_Excel_Col(Colindex)).NumberFormat = NF
                    'Next

                    wSheet.Range("A1").resize(DT.Rows.Count + 1, DT.Columns.Count).Value = DataArray
                    wSheet.Columns.AutoFit()

                    CURSOR_HIDE_DEFAULT()


                    If FileName > "" Then
                        Excel.SaveWorkspace(FileName)
                    Else
                        Excel.Visible = True
                        Excel.WindowState = XlWindowState.xlMaximized
                    End If
                End If
            End If
        End If
    End Sub
End Class
