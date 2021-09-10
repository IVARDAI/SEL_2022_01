Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Wordprocessing
Imports System.Data.SqlClient
Imports MSWORD = Microsoft.Office.Interop.Word

Public Class FP_WORD_X
    Public Structure Struct_Word_Report
        Dim Report_Template_Name As String                  'A printoption definicoban megadott <filename>.docx file neve
        Dim Report_Template_Path As String                  'A word sablon teljes eleresi utvonala a file nevvel egyutt ..\Reports\<filename>.docx
        Dim Report_Target_Name As String                    'a cel dokmentum neve:  <TEMPLATENAME>yymmddHHmmss.docx
        Dim Report_Target_Name_Without_Extension As String
        Dim Report_Target_Path As String                    'A temp konyvtarban a cel dokmentum teljes eleresi neve:  ..\SEL_TEMP\<filename>yymmddHHmmss.docx
        Dim Report_Identifier As String                     'A printoption definicionban a report atózonositoja
        Dim Report_Head_ID As Long
        Dim Doc_Extension As String                     'A printoption definicioban megadott sablon file kiterjesztese (csak .DOCX lehet. Ha nem az, akor hibauzenettel a folyamat elszall)
        Dim Doc_Type As String                       'SP neve, amelyik feltolti a WORD_REPORT_FIELDS tablat

        'Az alabbi parametereket a program a dokumentum definiciobol olvassa ki, nem kell megadni.
        Dim Save_To_DOCMAN As Boolean
        Dim Saved_FileName As String
        Dim DOCMAN_Types_ID As Long
        Dim DOCMAN_RefNum As String
        Dim DOCMAN_Description As String
        Dim DOCMAN_Doc_Date As DateTime
        Dim DOCMAN_Scan_Date As DateTime
        Dim DOCMAN_ParentRecord_ID As Long
        Dim DOCMAN_Parent_TableName As String
        Dim DOCMAN_SAVE_PDF As Boolean
        Dim DOCMAN_SAVE_DOCX As Boolean
        Dim DOCMAN_Language As String
        Dim DOCMAN_Type As String
        Dim DOCMAN_Def_Parent_Table As String
        Dim DOCMAN_Doc_Filename_Format As String
        Dim DOCMAN_NoShowInternal As Boolean

        Dim Doc_Key As String
        Dim Doc As Microsoft.Office.Interop.Word.Document
    End Structure
    Private Structure Stru_P
        Dim BlockName As String
        Dim Par As DocumentFormat.OpenXml.Wordprocessing.Paragraph
        Dim TB As DocumentFormat.OpenXml.Wordprocessing.Table
    End Structure
    Private Structure Stru_IDX_NEWIDX
        Dim IDX As Integer
        Dim NEW_IDX As Integer
    End Structure

    Public DOC_OTHER_PARAMS As String = ""
    Public FPf As FP_Form
    Public WordApp As MSWORD.Application = Nothing
    Public DIC_DocX As New Dictionary(Of String, Struct_Word_Report)
    Private PArr() As Stru_P

    Public ReadOnly Property P_Is_WordApp_Open() As Boolean
        Get
            Dim OUT As Boolean = True

            If WordApp Is Nothing Then
                OUT = False
            Else
                Try
                    Dim Word_Name As String = WordApp.Name

                Catch ex As Exception
                    WordApp = Nothing
                    OUT = False
                End Try
            End If

            Return OUT
        End Get
    End Property
    Public ReadOnly Property P_Doc_Get_From_Doc_Key(Doc_Key As String) As MSWORD.Document
        Get
            Dim OUT As MSWORD.Document = Nothing

            If DIC_DocX.ContainsKey(nz(Doc_Key, "")) Then
                Dim Doc As MSWORD.Document = DIC_DocX(Doc_Key).Doc
                If Not (Doc Is Nothing) Then
                    Dim Doc_Name As String
                    Try
                        Doc_Name = Doc.Name
                        OUT = P_Doc_Get_From_Doc_Name(Doc_Name)
                    Catch ex As Exception
                        'Nothing to do
                    End Try
                End If
            End If

            Return OUT
        End Get
    End Property
    Public ReadOnly Property P_Doc_Get_From_Doc_Name(Doc_Name As String) As MSWORD.Document
        Get
            Dim OUT As MSWORD.Document = Nothing

            If P_Is_WordApp_Open Then
                For i As Integer = 1 To WordApp.Documents.Count
                    With WordApp.Documents(i)
                        If .Name = Doc_Name Then
                            OUT = WordApp.Documents(Doc_Name)
                            Exit For
                        End If
                    End With
                Next
            End If

            Return OUT
        End Get
    End Property
    Public ReadOnly Property P_Doc_Is_Open_In_Word(Doc_Key As String) As Boolean
        Get
            Return (Not (P_Doc_Get_From_Doc_Key(Doc_Key) Is Nothing))
        End Get
    End Property
    Sub New(ByVal MyFPf As FP_Form)
        FPf = MyFPf
    End Sub
    Public Sub Dispose()
        If Not (WordApp Is Nothing) Then
            WordApp.Visible = True
            WordApp.Quit()
            WordApp = Nothing
            KillUnusedWordProcess()
        End If
    End Sub
    Public Sub KillUnusedWordProcess()
        Dim oXlProcess As Process() = Process.GetProcessesByName("Word")

        For Each oXLP As Process In oXlProcess
            If Len(oXLP.MainWindowTitle) = 0 Then
                oXLP.Kill()
            End If
        Next
    End Sub
    'Sub CLOSE()
    '    If P_Is_WordApp_Open Then
    '        Try
    '            WordApp.Visible = True
    '            Do While WordApp.Documents.Count > 0
    '                WordApp.Documents(1).Close(False)
    '            Loop
    '            WordApp.Quit()

    '        Catch ex As Exception
    '            'Nothing to do
    '        End Try
    '        ReleaseObject(WordApp)
    '        WordApp = Nothing

    '        KillUnusedWordProcess()
    '    End If
    'End Sub
    Private Function WordApp_CREATE() As Boolean
        Dim OUT As Boolean = True

        If P_Is_WordApp_Open = False Then
            Try
                WordApp = New MSWORD.Application

            Catch ex As Exception
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word.WordApp_CREATE", Err.Number, Err.Description)
            End Try
        End If

        Return OUT
    End Function
    Sub WordApp_SHOW_MAX()
        If P_Is_WordApp_Open Then
            If WordApp.Visible = False Then
                WordApp.Visible = True
            End If

            If WordApp.WindowState <> Microsoft.Office.Interop.Word.WdWindowState.wdWindowStateMaximize Then
                WordApp.WindowState = Microsoft.Office.Interop.Word.WdWindowState.wdWindowStateMaximize
            End If
        End If
    End Sub
    Function OPEN_Doc(Doc_FullName As String, Optional ByRef OUT_WordDoc As Microsoft.Office.Interop.Word.Document = Nothing) As Boolean
        Dim OUT As Boolean = True
        Dim Doc_Name As String = getFileNameFromFullName(Doc_FullName)

        If Doc_FullName = "" Then
            OUT = False
            FPf.FPApp.DoErrorMsgBox("FP_Word.OPEN", 0, "MyWordFileName = ''")
        End If

        If OUT = True Then
            OUT = WordApp_CREATE()
        End If

        If OUT Then
            WordApp_SHOW_MAX()
            Try
                OUT_WordDoc = WordApp.Documents.Open(Doc_FullName)

            Catch ex As Exception
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word.OPEN_Doc", Err.Number, Err.Description)
            End Try
        End If

        Return OUT
    End Function
    Private Function Get_Block_Name(S As String) As String
        Dim OUT As String
        Dim I As Integer
        Dim J As Integer
        I = S.IndexOf("#_")
        J = S.IndexOf("_#")
        If I < 0 Or J < 0 Then
            OUT = ""
        Else
            If J < I Then
                OUT = ""
            Else
                OUT = S.Substring(I, (J - I) + 2)
            End If
        End If
        Return OUT
    End Function
    Private Sub Check_Table_Cells(tbl As Table)
        Dim TCount As Integer = tbl.Elements(Of TableRow).Count
        Dim PCount As Integer
        Dim TcCount As Integer
        Dim TRow As TableRow

        Dim I As Integer
        Dim J As Integer
        For I = 0 To TCount - 1
            TRow = tbl.Elements(Of TableRow)().ElementAt(I)
            TcCount = TRow.Elements(Of TableCell).Count
            For J = 0 To TcCount - 1
                Dim TC As TableCell = TRow.Elements(Of TableCell)().ElementAt(J)
                PCount = TC.Elements(Of Paragraph).Count
                If PCount = 0 Then
                    Dim para As Paragraph = TC.AppendChild(New Paragraph)
                End If
            Next
        Next
    End Sub
    Private Sub Add_Para_To_Body(P As Paragraph, B As Body, Tbl As Table)
        If Not P Is Nothing Then
            Dim NP As Paragraph = P.CloneNode(True)
            B.AppendChild(NP)
        End If
        If Not Tbl Is Nothing Then
            Dim NTbl As Table = Tbl.CloneNode(True)
            Check_Table_Cells(NTbl)
            B.AppendChild(NTbl)
        End If
    End Sub
    Private Sub AddRun_To_P(ByRef P As Paragraph, Tx As String, R As Run)
        Dim NewRun As Run = P.AppendChild(New Run)
        Dim rps As RunProperties = Nothing

        If R.Descendants(Of RunProperties)().Count > 0 Then
            rps = R.Descendants(Of RunProperties)().First()
        End If

        If Not rps Is Nothing Then
            Dim NewRunProps As OpenXmlElement
            NewRunProps = rps.CloneNode(True)
            NewRun.AppendChild(Of RunProperties)(NewRunProps)
        End If

        If Tx.ToUpper = "#TAB#" Then
            Dim TC As New TabChar
            NewRun.Append(TC)
        ElseIf Tx.ToUpper = "#BR#" Then
            Dim BR As New Break
            NewRun.Append(BR)
        Else
            Dim newText As New Text(Tx) With {
                .Space = SpaceProcessingModeValues.Preserve
            }
            Dim new_TextElement As OpenXmlElement = NewRun.AppendChild(Of Text)(newText)
        End If
    End Sub

    Private Function AddNew_Empty_Run_With_Property(r As Run) As Run
        Dim OUT As Run = New Run
        Dim rps As RunProperties = Nothing
        If r.Descendants(Of RunProperties)().Count > 0 Then
            rps = r.Descendants(Of RunProperties)().First()
        End If

        If Not rps Is Nothing Then
            Dim NewRunProps As OpenXmlElement
            NewRunProps = rps.CloneNode(True)
            OUT.AppendChild(Of RunProperties)(NewRunProps)
        End If

        Return OUT
    End Function
    Private Function SearchAndReplaceInTable(TBL As Table, Search As String, Replace As String) As Table
        Dim OUT As Table = TBL
        Dim T As String = TBL.InnerText
        Dim paras As Object

        If T.ToUpper.Contains(Search.ToUpper) Then
            paras = TBL.Descendants(Of Paragraph)
            For Each para As OpenXmlElement In paras
                If para.InnerText.ToUpper.Contains(Search.ToUpper) Then
                    para = SearchAndReplaceInParagraph(para, Search, Replace)
                End If
            Next
        End If
        Return OUT
    End Function

    Private Function SearchAndReplaceInFullDoc(ByVal DCM As Document, ByVal Search As String, ByVal Replace As String) As Document
        Dim OUT As Document = DCM
        Dim P As Paragraph
        Dim TBL As Table
        For Each Child1 In OUT.ChildElements
            Debug.Print(Child1.ToString)
            For Each Child2 In Child1.ChildElements
                Debug.Print(Child2.ToString)
                Select Case Child2.LocalName.ToUpper
                    Case "P"
                        P = Child2
                        P = SearchAndReplaceInParagraph(P, Search, Replace)
                    Case "TBL"
                        TBL = Child2
                        TBL = SearchAndReplaceInTable(TBL, Search, Replace)
                    Case Else
                        Debug.Print(Child2.LocalName & ": nem kezelt localname")
                End Select
            Next
        Next
        Return OUT
    End Function
    Private Function Handling_Keep_In_Table(TBL As Table) As Table
        Dim OUT As Table = TBL
        Dim T As String = TBL.InnerText
        Dim KEEP As String = "#_KEEP_#"
        Dim Rid As Integer = -1
        If T.ToUpper.Contains(KEEP) Then
            For Each tr As OpenXmlElement In OUT.Elements(Of TableRow)
                Rid += 1
                If tr.InnerText.ToUpper.Contains(KEEP) Then
                    For Each tc As OpenXmlElement In tr.Elements(Of TableCell)
                        If tc.InnerText.ToUpper.Contains(KEEP) Then
                            For Each pr As OpenXmlElement In tc.Elements(Of Paragraph)
                                If pr.InnerText.ToUpper.Contains(KEEP) Then
                                    pr = SearchAndReplaceInParagraph(pr, KEEP, "")
                                    If Rid > 0 Then
                                        For Each ru As OpenXmlElement In pr.Elements(Of Run)
                                            For Each tx As Text In ru.Elements(Of Text)
                                                tx.Text = ""
                                            Next
                                        Next
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            Next
        End If
        Return OUT
    End Function

    Structure Stru_Run_Text
        Dim R As Run
        Dim T As Text
        Dim RI As Integer
        Dim TI As Integer
        Dim Is_TAB As Boolean
        Dim Is_BR As Boolean
        Dim Is_Drawing As Boolean
        Dim Is_AlternateContent As Boolean
        Dim Is_Pict As Boolean
    End Structure

    Structure Stru_Run_Chars
        Dim Run_IDX As Integer
        Dim CHR As String
        Dim CHR_IDX As Integer
        Dim New_Run_IDX As Integer
    End Structure

    Private Function Merge_Runs_In_Table(TBL As Table) As Table
        Dim OUT As Table = TBL
        Dim T As String = TBL.InnerText
        Dim paras As Object

        paras = TBL.Descendants(Of Paragraph)
        For Each para As OpenXmlElement In paras
            para = Merge_Runs_In_Paragraph(para)
        Next
        Return OUT
    End Function
    Private Function Merge_Runs_In_Paragraph(PARA As Paragraph) As Paragraph
        Dim runs As Object
        Dim Arr_Run() As Run = Nothing
        Dim A As Integer = 0
        Dim AktRun As Run = Nothing
        Dim OUT As Paragraph = PARA
        Dim I As Integer
        Dim LocalName As String
        Dim AktText As Text
        Dim IS_TAB As Boolean
        Dim IS_BR As Boolean
        Dim IS_Drawing As Boolean
        Dim Is_AlterCont As Boolean
        Dim Is_Pict As Boolean
        Dim Arr_RT() As Stru_Run_Text = Nothing
        Dim In_F As Boolean
        Dim Arr_CH() As Stru_Run_Chars = Nothing
        Dim Tx As String
        Dim CH_I As Integer = 0
        Dim Arr_IDX_NEWIDX() As Stru_IDX_NEWIDX = Nothing
        Dim J As Integer = 0
        runs = PARA.Descendants(Of Run)
        For Each run As OpenXmlElement In runs
            ReDim Preserve Arr_Run(A)
            Arr_Run(A) = run
            A += 1
        Next
        If Not Arr_Run Is Nothing Then

            PARA.RemoveAllChildren(Of Run)()
            PARA.RemoveAllChildren(Of Hyperlink)()

            For RI As Integer = 0 To Arr_Run.Length - 1
                AktRun = Arr_Run(RI)
                I = 0

                For Each TT In AktRun.ChildElements
                    LocalName = TT.LocalName.ToUpper
                    AktText = Nothing
                    Tx = ""

                    Select Case LocalName
                        Case "RPR"
                           'Nothing to do
                        Case "T"
                            AktText = TT
                            Tx = AktText.InnerText
                        Case "TAB"
                            AktText = Nothing
                            Tx = "#TAB#"
                        Case "BR"
                            AktText = Nothing
                            Tx = "#BR#"
                        Case "DRAWING"
                            AktText = Nothing
                            Tx = "#DRAW#"
                        Case "ALTERNATECONTENT"
                            AktText = Nothing
                            Tx = "#ALT#"
                        Case "LASTRENDEREDPAGEBREAK"
                            Tx = ""
                        Case "PICT"
                            AktText = Nothing
                            Tx = "#PICT#"
                        Case Else
                            MsgBox("Not treated LocalName: " & TT.LocalName.ToUpper)
                    End Select
                    For I = 0 To Tx.Length - 1
                        ReDim Preserve Arr_CH(CH_I)
                        With Arr_CH(CH_I)
                            .CHR = Tx(I)
                            .CHR_IDX = I
                            .Run_IDX = RI
                            .New_Run_IDX = 0
                        End With
                        CH_I += 1
                    Next
                Next
            Next

            If Not Arr_CH Is Nothing Then
                Dim CH0 As String
                Dim CH1 As String
                Dim RX0 As Integer
                Dim RX1 As Integer
                Dim New_Run_IDX As Integer = 0
                Dim L As Integer = Arr_CH.Length - 1
                For I = 0 To L
                    CH1 = Arr_CH(I).CHR
                    RX1 = Arr_CH(I).Run_IDX
                    If I = 0 Then
                        CH0 = "!!!@@@!!!"
                        RX0 = -1
                    Else
                        CH0 = Arr_CH(I - 1).CHR
                        RX0 = Arr_CH(I - 1).Run_IDX
                    End If

                    If RX0 <> RX1 Then
                        If In_F Then
                            If CH0 = "_" And CH1 = "#" Then
                                In_F = False
                            End If
                            Arr_CH(I).New_Run_IDX = New_Run_IDX
                        Else
                            If CH0 = "#" And CH1 = "_" Then
                                In_F = True
                            Else
                                New_Run_IDX += 1
                            End If
                            Arr_CH(I).New_Run_IDX = New_Run_IDX
                        End If
                    Else
                        If In_F Then
                            Arr_CH(I).New_Run_IDX = New_Run_IDX
                            If CH0 = "_" And CH1 = "#" Then
                                In_F = False
                            End If
                        Else
                            Arr_CH(I).New_Run_IDX = New_Run_IDX
                            If CH0 = "#" And CH1 = "_" Then
                                In_F = True
                            End If
                        End If
                    End If
                Next
            End If

            If Not Arr_CH Is Nothing Then
                Dim RX0 As Integer
                Dim RX1 As Integer
                Dim NewIDX0 As Integer
                Dim NewIDX1 As Integer
                Dim L As Integer = Arr_CH.Length - 1
                For I = 0 To L
                    RX1 = Arr_CH(I).Run_IDX
                    NewIDX1 = Arr_CH(I).New_Run_IDX
                    If I = 0 Then
                        RX0 = -1
                        NewIDX0 = -1
                        ReDim Preserve Arr_IDX_NEWIDX(J)
                        With Arr_IDX_NEWIDX(J)
                            .IDX = RX1
                            .NEW_IDX = Arr_CH(I).New_Run_IDX
                        End With
                        J += 1
                    Else
                        RX0 = Arr_CH(I - 1).Run_IDX
                        NewIDX0 = Arr_CH(I - 1).New_Run_IDX
                        If RX0 <> RX1 Then
                            ReDim Preserve Arr_IDX_NEWIDX(J)
                            With Arr_IDX_NEWIDX(J)
                                .IDX = RX1
                                .NEW_IDX = Arr_CH(I).New_Run_IDX
                            End With
                            J += 1
                        Else
                            'nothing to do
                        End If
                    End If
                Next
            End If

            If Not Arr_IDX_NEWIDX Is Nothing Then
                Dim RRR As Integer = 0
                Dim Add As Boolean = True
                Dim RI As Integer
                Dim TI As Integer
                For I = 0 To Arr_IDX_NEWIDX.Length - 1

                    IS_TAB = False
                    IS_BR = False
                    IS_Drawing = False
                    Is_AlterCont = False
                    Is_Pict = False
                    AktText = Nothing
                    Tx = ""

                    AktRun = Arr_Run(I)
                    TI = Arr_IDX_NEWIDX(I).NEW_IDX
                    RI = Arr_IDX_NEWIDX(I).IDX

                    For Each TT In AktRun.ChildElements
                        LocalName = TT.LocalName.ToUpper
                        Add = True
                        Select Case LocalName
                            Case "RPR"
                                Add = False
                            Case "T"
                                AktText = TT
                                Tx = AktText.InnerText
                            Case "TAB"
                                AktText = Nothing
                                Tx = "#TAB#"
                                IS_TAB = True
                            Case "BR"
                                AktText = Nothing
                                Tx = "#BR#"
                                IS_BR = True
                            Case "DRAWING"
                                AktText = Nothing
                                Tx = "#DR#"
                                IS_Drawing = True
                            Case "ALTERNATECONTENT"
                                AktText = Nothing
                                Tx = "#AC#"
                                Is_AlterCont = True
                            Case "LASTRENDEREDPAGEBREAK"
                                Debug.Print(AktRun.InnerXml)
                            Case "PICT"
                                AktText = Nothing
                                Tx = "#PICT#"
                                Is_Pict = True
                            Case Else
                                MsgBox("Localname: " + TT.LocalName.ToUpper)
                                Add = False
                        End Select
                        If Add Then
                            ReDim Preserve Arr_RT(RRR)
                            With Arr_RT(RRR)
                                .R = AktRun
                                .T = AktText
                                .RI = RI
                                .TI = TI
                                .Is_TAB = IS_TAB
                                .Is_BR = IS_BR
                                .Is_Drawing = IS_Drawing
                                .Is_AlternateContent = Is_AlterCont
                                .Is_Pict = Is_Pict
                            End With
                            RRR += 1
                        End If
                    Next
                Next
            End If

            If Not Arr_RT Is Nothing Then
                Dim MyRun As Run = Nothing
                Dim Curr_TI As Integer
                Dim Old_TI As Integer = -1
                For I = 0 To Arr_RT.Length - 1
                    With Arr_RT(I)
                        Curr_TI = .TI
                        If MyRun Is Nothing Then
                            If .Is_Drawing Then
                                MyRun = .R.CloneNode(True)
                            ElseIf .Is_AlternateContent Then
                                MyRun = .R.CloneNode(True)
                            ElseIf .Is_Pict Then
                                MyRun = .R.CloneNode(True)
                            Else
                                MyRun = AddNew_Empty_Run_With_Property(.R)
                                If Not .T Is Nothing Then
                                    Dim new_TextElement As OpenXmlElement = MyRun.AppendChild(Of Text)(.T.CloneNode(True))
                                End If
                                If .Is_TAB Then
                                    Dim TC As New TabChar
                                    MyRun.Append(TC)
                                End If
                                If .Is_BR Then
                                    Dim BR As New Break
                                    MyRun.Append(BR)
                                End If
                            End If
                            Old_TI = .TI
                        Else
                            If Curr_TI = Old_TI Then
                                If MyRun.Elements(Of Text).Count = 0 Then
                                    If Not .T Is Nothing Then
                                        MyRun.AppendChild(Of Text)(.T.CloneNode(True))
                                    Else
                                        'nothing to do
                                    End If
                                Else
                                    Dim TT As String
                                    If .T Is Nothing Then
                                        TT = ""
                                    Else
                                        TT = .T.Text
                                    End If
                                    MyRun.Elements(Of Text).First.Text += TT
                                End If
                            Else
                                PARA.AppendChild(Of Run)(MyRun)
                                If .Is_Drawing Then
                                    MyRun = .R.CloneNode(True)
                                ElseIf .Is_AlternateContent Then
                                    MyRun = .R.CloneNode(True)
                                ElseIf .Is_Pict Then
                                    MyRun = .R.CloneNode(True)
                                Else
                                    MyRun = AddNew_Empty_Run_With_Property(.R)
                                End If
                                If Not .T Is Nothing Then
                                    Dim new_TextElement As OpenXmlElement = MyRun.AppendChild(Of Text)(.T.CloneNode(True))
                                End If
                                If .Is_TAB Then
                                    Dim TC As New TabChar
                                    MyRun.Append(TC)
                                End If
                                If .Is_BR Then
                                    Dim BR As New Break
                                    MyRun.Append(BR)
                                End If
                                Old_TI = Curr_TI
                            End If
                        End If
                    End With
                Next
                If Not MyRun Is Nothing Then
                    PARA.AppendChild(Of Run)(MyRun)
                End If
            End If
        End If
        OUT = PARA
        Return OUT
    End Function
    Private Function Merge_Runs_In_Full_Doc(ByVal DCM As Document) As Document
        Dim OUT As Document = DCM
        Dim P As Paragraph
        Dim TBL As Table
        For Each Child1 In OUT.ChildElements
            Debug.Print(Child1.ToString)
            For Each Child2 In Child1.ChildElements
                Debug.Print(Child2.ToString)
                Select Case Child2.LocalName.ToUpper
                    Case "P"
                        P = Child2
                        P = Merge_Runs_In_Paragraph(P)
                    Case "TBL"
                        TBL = Child2
                        TBL = Merge_Runs_In_Table(TBL)
                    Case Else
                        Debug.Print(Child2.LocalName & ": nem kezelt localname")
                End Select
            Next
        Next
        Return OUT
    End Function
    Private Function SearchAndReplaceInParagraph(ByVal PARA As Paragraph, ByVal Search As String, ByVal Replace As String) As Paragraph
        Dim OUT As Paragraph
        Dim RI As Integer = 0
        Dim runs As Object
        Dim AktRun As Run
        Dim LocalName As String
        Dim MyRun As Run = Nothing
        Dim Arr_Run() As Run = Nothing

        If PARA.InnerText.ToUpper.Contains(Search.ToUpper) Then
            runs = PARA.Descendants(Of Run)
            For Each run As OpenXmlElement In runs
                ReDim Preserve Arr_Run(RI)
                Arr_Run(RI) = run
                RI += 1
            Next
            If Not Arr_Run Is Nothing Then
                PARA.RemoveAllChildren(Of Run)()
                For RI = 0 To Arr_Run.Length - 1
                    AktRun = Arr_Run(RI)
                    With AktRun
                        MyRun = AddNew_Empty_Run_With_Property(AktRun)
                        For Each TT In AktRun.ChildElements
                            LocalName = TT.LocalName.ToUpper
                            Select Case LocalName
                                Case "RPR"
                                    'Nothing to do
                                Case "T"
                                    Dim new_TextElement As OpenXmlElement = MyRun.AppendChild(Of Text)(TT.CloneNode(True))
                                    If new_TextElement.InnerText.ToUpper.Contains(Search.ToUpper) Then
                                        CType(new_TextElement, Text).Text = CType(new_TextElement, Text).Text.Replace(Search, Replace)
                                    End If
                                Case "TAB"
                                    Dim TC As New TabChar
                                    MyRun.Append(TC)
                                Case "BR"
                                    Dim BR As New Break
                                    MyRun.Append(BR)
                                Case "DRAWING"
                                    MyRun.AppendChild(Of Drawing)(TT.CloneNode(True))
                                Case "ALTERNATECONTENT"
                                    MyRun.AppendChild(Of AlternateContent)(TT.CloneNode(True))
                                Case Else
                                    'MsgBox("Localname: " + TT.LocalName.ToUpper)
                                    '!!! ide még kell valami !!!
                            End Select
                        Next
                        PARA.AppendChild(Of Run)(MyRun)
                    End With
                Next
            End If
        End If
        OUT = PARA
        Return OUT
    End Function
    Public Function Doc_Table_Cell_Text_Is_Empty(MyCell_Text As String) As Boolean
        Dim OUT As Boolean
        If Trim(nz(MyCell_Text, "")) = Chr(13) + Chr(7) Then
            OUT = True
        ElseIf InStr(MyCell_Text, "#_KEEP_#") > 0 Then
            OUT = True
        Else
            OUT = False
        End If
        Return OUT
    End Function
    Private Function RowIsEmpty(R As TableRow) As Boolean
        Dim OUT As Boolean = True
        For Each TC In R.Elements(Of TableCell)
            For Each P As Paragraph In TC.Elements(Of Paragraph)
                For Each Rn As Run In P.Elements(Of Run)
                    For Each T As Text In Rn.Elements(Of Text)
                        If Not T.Text Is String.Empty Then
                            OUT = False
                        End If
                    Next
                Next
            Next
        Next
        Return OUT
    End Function
    Private Function Clear_Table_Row(TR As TableRow) As TableRow
        Dim OUT As TableRow = TR
        Dim KEEP As String = "#_KEEP_#"
        If TR.Elements(Of TableCell).Count > 0 Then
            For Each tc As OpenXmlElement In TR.Elements(Of TableCell)
                If tc.InnerText.ToUpper.Contains(KEEP) Then
                    'nothing to do
                Else
                    For Each pr As OpenXmlElement In tc.Elements(Of Paragraph)
                        For Each ru As OpenXmlElement In pr.Elements(Of Run)
                            For Each tx As Text In ru.Elements(Of Text)
                                tx.Text = ""
                            Next
                        Next
                    Next
                End If
            Next
        End If
        Return OUT
    End Function
    Private Function Insert_Row_To_Table(RowText As String, Tbl As DocumentFormat.OpenXml.Wordprocessing.Table, Is_First_Table_Row As Boolean) As Table
        Dim InsertRow As Integer
        Dim rVal As String() = Split(RowText, "#_#")
        Dim OUT As DocumentFormat.OpenXml.Wordprocessing.Table = Tbl.CloneNode(True)
        Dim pPr As ParagraphMarkRunProperties = Nothing
        Dim pp As ParagraphProperties
        Dim EditedRow As TableRow

        If Not OUT Is Nothing Then
            Dim RCount As Integer = OUT.Elements(Of TableRow).Count

            If Is_First_Table_Row Then
                InsertRow = 0
                EditedRow = OUT.Elements(Of TableRow)().ElementAt(RCount - 1)
            Else
                InsertRow = RCount
                EditedRow = OUT.Elements(Of TableRow)().ElementAt(RCount - 1).CloneNode(True)
            End If

            EditedRow = Clear_Table_Row(EditedRow)

            Dim I As Integer
            Dim CellCount As Integer = EditedRow.Elements(Of TableCell).Count
            Dim N As Integer = rVal.Length
            If CellCount < N Then N = CellCount
            For I = 0 To N - 1
                Dim TC As TableCell = EditedRow.Elements(Of TableCell)().ElementAt(I)
                Dim P As Paragraph
                If TC.Elements(Of Paragraph).Count = 0 Then
                    TC.Append(New Paragraph(New Run(New Text(rVal(I)))))
                Else
                    P = TC.Elements(Of Paragraph)().First
                    Try
                        pp = P.Descendants(Of ParagraphProperties).First
                        pPr = pp.Descendants(Of ParagraphMarkRunProperties).First
                    Catch ex As Exception

                    End Try

                    If P.Elements(Of Run).Count = 0 Then
                        Dim newr As Run = P.AppendChild(New Run)
                        If Not pPr Is Nothing Then
                            Dim NewRunProps As OpenXmlElement
                            NewRunProps = pPr.CloneNode(True)
                            newr.AppendChild(Of ParagraphMarkRunProperties)(NewRunProps)
                        End If
                        newr.AppendChild(New Text(rVal(I)))
                    Else
                        Dim r As Run = P.Elements(Of Run)().First
                        If r.Elements(Of Text).Count = 0 Then
                            r.AppendChild(New Text(rVal(I)))
                        Else
                            Dim t As Text = r.Elements(Of Text)().First
                            t.Text &= rVal(I)
                            t.Text = t.Text.Trim
                        End If
                    End If
                End If
            Next

            If InsertRow = 0 Then
                'Nothing to do
            Else
                OUT.Append(EditedRow)
            End If
        End If
        Return OUT
    End Function
    Private Function Block_Defs_LOAD(ByRef SWD As Struct_Word_Report, From_ZD As Boolean) As Boolean
        Dim OUT As Boolean = True
        Dim Params_Tbl As New DataTable
        Dim MySQL As String = ""
        Dim REPORT_Identifier As String = SWD.Report_Identifier
        Dim Param_DOCMAN_SAVE_YN_Exists As Boolean = False

        Using WD_Target As WordprocessingDocument = WordprocessingDocument.Open(SWD.Report_Target_Path, True)
            Dim Report_Def_Table As DocumentFormat.OpenXml.Wordprocessing.Table = Nothing
            Dim TRow As DocumentFormat.OpenXml.Wordprocessing.TableRow = Nothing
            Dim TCell As DocumentFormat.OpenXml.Wordprocessing.TableCell = Nothing
            Dim TColCount As Integer
            Dim TRowCount As Integer

            If WD_Target.MainDocumentPart.Document.Body.Elements(Of Table).Count = 0 Then
                OUT = False
            Else
                Report_Def_Table = WD_Target.MainDocumentPart.Document.Body.Elements(Of Table)().First()
            End If

            If OUT Then
                TRowCount = Report_Def_Table.Elements(Of TableRow).Count
                TRow = Report_Def_Table.Elements(Of TableRow)().ElementAt(0)
                If TRow Is Nothing Then
                    OUT = False
                Else
                    TColCount = TRow.Elements(Of TableCell).Count
                End If
                If TColCount < 3 Then OUT = False
            End If
            If OUT Then
                TCell = TRow.Elements(Of TableCell)().ElementAt(0)
                If Not TCell Is Nothing Then
                    If TCell.InnerText.ToUpper <> "#_DOC_TYPE_#" Then
                        OUT = False
                    End If
                Else
                    OUT = False
                End If
            End If

            If OUT Then
                With Params_Tbl
                    .Columns.Add("BLOCKNAME", System.Type.GetType("System.String"))
                    .Columns.Add("FIELDNAME", System.Type.GetType("System.String"))
                    .Columns.Add("PARAMS", System.Type.GetType("System.String"))
                End With

                If FPf.FPApp.DC.P_USE_LocalDB Then
                    If REPORT_Identifier > "" Then
                        With FPf.FPApp.DC.LocalDB_SEL
                            .RunSQL(String.Format("DELETE WORD_Docs_Block_Defs WHERE REPORT_Identifier = '{0}'", REPORT_Identifier))
                            .RunSQL(String.Format("DELETE WORD_Docs_Block_Defs_L WHERE REPORT_Identifier = '{0}'", REPORT_Identifier))
                            .RunSQL(String.Format("DELETE WORD_Docs_Block_Ranges WHERE REPORT_Identifier = '{0}'", REPORT_Identifier))
                        End With
                    End If
                End If

                'Default values for Docman Save
                SWD.DOCMAN_SAVE_PDF = True
                SWD.DOCMAN_SAVE_DOCX = False

                For i As Integer = 0 To TRowCount - 1
                    TRow = Report_Def_Table.Elements(Of TableRow)().ElementAt(i)
                    Dim C_BLOCKNAME As String = MSWORD_CELL_REMOVE_SYS_CHARS(Trim(nz(TRow.Elements(Of TableCell)().ElementAt(0).InnerText, "")))
                    Dim C_FIELDNAME As String = MSWORD_CELL_REMOVE_SYS_CHARS(Trim(nz(TRow.Elements(Of TableCell)().ElementAt(1).InnerText, "")))
                    Dim C_PARAMS As String = MSWORD_CELL_REMOVE_SYS_CHARS(Trim(nz(TRow.Elements(Of TableCell)().ElementAt(2).InnerText, "")))

                    If C_BLOCKNAME.ToUpper = "#_DOC_TYPE_#" Then
                        SWD.Doc_Type = C_FIELDNAME
                    ElseIf C_BLOCKNAME.ToUpper = "#_DOC_LANGUAGE_#" Then
                        SWD.DOCMAN_Language = C_FIELDNAME
                    ElseIf C_BLOCKNAME.ToUpper = "#_FILENAME_#" Then
                        SWD.DOCMAN_Doc_Filename_Format = C_FIELDNAME
                    ElseIf C_BLOCKNAME.ToUpper = "#_NO_SHOW_INTERNAL_#" Then
                        SWD.DOCMAN_NoShowInternal = (C_FIELDNAME = "1")
                    ElseIf C_BLOCKNAME.ToUpper = "#_PARENT_TABLE_#" Then
                        SWD.DOCMAN_Def_Parent_Table = C_FIELDNAME
                    ElseIf C_BLOCKNAME.ToUpper = "#_DOCMAN_SAVE_YN_#" Then
                        SWD.Save_To_DOCMAN = TEXT_Is_YES(C_FIELDNAME)
                        Param_DOCMAN_SAVE_YN_Exists = True
                    ElseIf C_BLOCKNAME.ToUpper = "#_DOCMAN_SAVE_PDF_#" Then
                        SWD.DOCMAN_SAVE_PDF = TEXT_Is_YES(C_FIELDNAME)
                    ElseIf C_BLOCKNAME.ToUpper = "#_DOCMAN_SAVE_DOCX_#" Then
                        SWD.DOCMAN_SAVE_DOCX = TEXT_Is_YES(C_FIELDNAME)
                    ElseIf C_BLOCKNAME.ToUpper = "#_DOCMAN_TYPE_#" Then
                        SWD.DOCMAN_Type = C_FIELDNAME
                    ElseIf C_BLOCKNAME.ToUpper = "#_DOCMAN_REFNUM_FIELD_#" Then
                        SWD.DOCMAN_RefNum = C_FIELDNAME
                    ElseIf C_BLOCKNAME.ToUpper = "#_DOCMAN_DESCRIPTION_#" Then
                        SWD.DOCMAN_Description = C_FIELDNAME
                    Else
                        If C_BLOCKNAME > "" Or C_FIELDNAME > "" Then
                            Dim DRow As DataRow = Params_Tbl.NewRow
                            With DRow
                                !BLOCKNAME = C_BLOCKNAME
                                !FIELDNAME = C_FIELDNAME
                                !PARAMS = C_PARAMS
                            End With

                            Params_Tbl.Rows.Add(DRow)

                            If FPf.FPApp.DC.P_USE_LocalDB Then
                                If REPORT_Identifier > "" Then
                                    MySQL = String.Format("INSERT INTO WORD_Docs_Block_Defs_L (REPORT_Identifier, BlockName, FieldName, Params) VALUES ('{0}', '{1}', '{2}', '{3}')", REPORT_Identifier, C_BLOCKNAME, C_FIELDNAME, C_PARAMS)
                                    FPf.FPApp.DC.LocalDB_SEL.RunSQL(MySQL)
                                End If
                            End If
                        End If
                    End If
                Next
            End If
        End Using

        If OUT = True Then
            If SWD.Doc_Type = "" Then
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOC_TYPE_#' in document definition table not found.")
            ElseIf SWD.DOCMAN_Language = "" Then
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOC_LANGUAGE_#' in document definition table not found.")
            ElseIf SWD.DOCMAN_Doc_Filename_Format = "" Then
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOC_FILENAME_#' in document definition table not found.")
            ElseIf SWD.DOCMAN_Def_Parent_Table = "" Then
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_PARENT_TABLE_#' in document definition table not found.")
            ElseIf Param_DOCMAN_SAVE_YN_Exists = False Then
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOCMAN_SAVE_YN_#' in document definition table not found.")
            ElseIf SWD.DOCMAN_Type = "" Then
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOCMAN_TYPE_#' in document definition table not found.")
            ElseIf SWD.DOCMAN_RefNum = "" Then
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOCMAN_REFNUM_FIELD_#' in document definition table not found.")
            ElseIf SWD.DOCMAN_Description = "" Then
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOCMAN_DESCRIPTION_#' in document definition table not found.")
            End If

            Dim Doc_Parent_FP As FP = Nothing

            If Not From_ZD Then
                If OUT = True Then
                    Doc_Parent_FP = FPf.GET_FP_BY_ALIAS(SWD.DOCMAN_Def_Parent_Table)

                    If Doc_Parent_FP Is Nothing Then
                        OUT = False
                        FPf.FPApp.DoErrorMsgBox("FP_Word.Word_Blocks.Block_Defs_LOAD", 0, String.Format("Unknown FP Alias ({0})", SWD.DOCMAN_Def_Parent_Table))
                    End If
                End If

                If OUT = True Then
                    If SWD.Report_Head_ID = 0 Then
                        SWD.Report_Head_ID = Doc_Parent_FP.P_DATA_Current_ID
                    End If
                    If SWD.Report_Head_ID = 0 Then
                        OUT = False
                        MsgBox("+++ Alljon ra a rekordra, amit ki szeretne nyomtatni.")
                    End If
                End If

                If OUT = True Then
                    If SWD.Save_To_DOCMAN Then
                        SWD.DOCMAN_Parent_TableName = ""
                        SWD.DOCMAN_ParentRecord_ID = 0
                        SWD.DOCMAN_Types_ID = 0

                        If Doc_Parent_FP.FP_DOCMAN Is Nothing Then
                            OUT = False
                            FPf.FPApp.DoErrorMsgBox("FP_Word.Block_Defs_LOAD", 0, String.Format("Define FP_DOCMAN for FP '{0}'" + vbCrLf + "(Command: {0}.FP_DOCMAN = <Your DOCMAN_Doc_Panel>)", Doc_Parent_FP.ServerObject_Prefix))
                        End If
                        If OUT = True Then
                            With Doc_Parent_FP.FP_DOCMAN
                                SWD.DOCMAN_Parent_TableName = .P_Parent_TableName
                                SWD.DOCMAN_ParentRecord_ID = .FP_Parent.P_DATA_Current_ID
                                OUT = .Doc_Types_GET_FROM_STR(SWD.DOCMAN_Type, SWD.DOCMAN_Types_ID)
                            End With
                        End If
                    End If
                End If
            End If

            If OUT = True Then
                Dim Params_STR As String = ""

                For i As Integer = 0 To Params_Tbl.Rows.Count - 1
                    With Params_Tbl.Rows(i)
                        Params_STR += !BLOCKNAME + "#_#" + !FIELDNAME + "#_#" + !PARAMS + "|_ROWEND_|"
                    End With
                Next

                Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

                Dim ProcName As String = "WORD_REPORT_" + SWD.Doc_Type + "_PREPARE"

                If Strings.Left(SWD.Doc_Type, 2) = "U_" Then
                    ProcName = SWD.Doc_Type
                End If

                With FPf.FPApp.DC
                    .Qdf_set_SP(sqlComm, ProcName)
                    .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                    .Qdf_AddParameter(sqlComm, "@Report_Name", SqlDbType.NVarChar, , 128, REPORT_Identifier)
                    .Qdf_AddParameter(sqlComm, "@Doc_Language", SqlDbType.NVarChar, , 3, SWD.DOCMAN_Language)
                    .Qdf_AddParameter(sqlComm, "@Head_ID", SqlDbType.Int, , , , , SWD.Report_Head_ID)
                    .Qdf_AddParameter(sqlComm, "@DOC_PARAMS", SqlDbType.NVarChar, , -1, Params_STR)
                    .Qdf_AddParameter(sqlComm, "@DOC_OTHER_PARAMS", SqlDbType.NVarChar, , -1, DOC_OTHER_PARAMS)
                    .Qdf_AddParameter(sqlComm, "@DOC_FILENAME_FORMAT", SqlDbType.NVarChar, , -1, SWD.DOCMAN_Doc_Filename_Format)

                    .Qdf_AddParameter(sqlComm, "@DOC_FILENAME", SqlDbType.NVarChar, ParameterDirection.Output, 240)
                    .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                    .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
                End With

                CURSOR_SHOW_WAIT()
                Try
                    OUT = FPf.FPApp.DC.Qdf_Execute("FP_Word..Word_Blocks.Block_Defs_LOAD", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
                Catch ex As Exception
                    OUT = False
                    FPf.FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
                End Try

                If OUT = True Then
                    SWD.Saved_FileName = gl_FPApp.Text_Remove_IllegalCharacters_From_FileName(Trim(nz(sqlComm.Parameters("@DOC_FILENAME").Value, "")))
                    If SWD.Saved_FileName = "" Then
                        OUT = False
                        FPf.FPApp.DoErrorMsgBox("FP_WORD_X.Block_Defs_LOAD", 0, "Param '#_DOC_FILENAME_#' invalid or procedure '{0}' returns with empty value. " & ProcName, True)
                    End If
                End If
                If OUT Then
                    Try
                        Dim New_File_Path As String = FPf.FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP) + SWD.Saved_FileName + ".docx"
                        System.IO.File.Copy(SWD.Report_Template_Path, New_File_Path)
                        SWD.Report_Target_Path = New_File_Path
                    Catch ex As Exception
                        OUT = False
                        FPf.FPApp.DoErrorMsgBox("FP_Word.OPEN_Word_REPORT", Err.Number, Err.Description)
                    End Try
                End If

                CURSOR_SHOW_DEFAULT()
            End If
        End If

        Return OUT
    End Function
    Public Function Load_Word_Doc(ByRef Stru_WordRep As Struct_Word_Report, From_ZD As Boolean) As Boolean
        Dim OUT As Boolean = True
        Block_Defs_LOAD(Stru_WordRep, From_ZD)

        Dim pi As Integer = 0
        Dim SectPr As DocumentFormat.OpenXml.Wordprocessing.SectionProperties = Nothing
        Dim BlockStart As Boolean = False
        Dim Block_Name As String = "--"
        Dim DT_Commands As DataTable = Nothing
        Dim MySQL As String = String.Format("SELECT Command, Field_Name, Field_Value FROM WORD_REPORT_FIELDS WHERE Terminals_ID = {0} And Report_Name = '{1}' ORDER BY ID", Terminals_ID, Stru_WordRep.Report_Identifier)

        Using WD_Target As WordprocessingDocument = WordprocessingDocument.Open(Stru_WordRep.Report_Target_Path, True)
            Dim my_doc_body As Body = WD_Target.MainDocumentPart.Document.Body
            Dim MainPart As MainDocumentPart = WD_Target.MainDocumentPart
            Dim D As Document = MainPart.Document
            Dim tb As Table = D.Descendants(Of Table)().First()
            tb.Remove()
            D = Merge_Runs_In_Full_Doc(D)

            For Each Child1 In D.ChildElements
                Debug.Print(Child1.ToString)
                For Each Child2 In Child1.ChildElements
                    Debug.Print(Child2.ToString)
                    If Child2.InnerText.ToUpper.Contains("#_BLOCK") Then
                        Block_Name = Get_Block_Name(Child2.InnerText.ToUpper)
                        BlockStart = True
                    Else
                        BlockStart = False
                    End If
                    If Child2.InnerText.ToUpper.Contains("#_END_#") Then
                        Block_Name = "--"
                    End If
                    If (Block_Name <> "--") And (Not BlockStart) Then
                        Dim P As Paragraph = Nothing
                        Dim TBL As Table = Nothing

                        Select Case Child2.LocalName.ToUpper
                            Case "P"
                                P = Child2.CloneNode(True)
                                TBL = Nothing
                            Case "TBL"
                                TBL = Child2.CloneNode(True)
                                P = Nothing
                            Case "BOOKMARKEND"
                                P = Nothing
                                TBL = Nothing
                            Case "SECTPR"
                                P = Nothing
                                TBL = Nothing
                            Case Else
                                MsgBox(Child2.LocalName & ": nem kezelt localname")
                        End Select

                        ReDim Preserve PArr(pi)
                        With PArr(pi)
                            .BlockName = Block_Name
                            .Par = P
                            .TB = TBL
                            pi += 1
                        End With
                    End If
                Next
            Next

            For Each P In D.Descendants(Of Paragraph).ToList
                P.Remove()
            Next
            For Each P In D.Descendants(Of Table).ToList
                P.Remove()
            Next

            For Each Child1 In D.ChildElements
                Debug.Print(Child1.ToString)
                For Each Child2 In Child1.ChildElements
                    If Child2.LocalName.ToUpper = "SECTPR" Then
                        SectPr = Child2.CloneNode(True)
                        Child2.Remove()
                    End If
                    Debug.Print(Child2.ToString)
                Next
            Next

            Dim DR As DataRow
            Dim Command As String
            Dim Current_Block_Name As String = ""
            Dim BlockArr() As Stru_P = Nothing
            Dim BI As Integer = 0
            Dim Is_First_Table_Row As Boolean = False
            Dim Search As String
            Dim Replace As String

            FPf.FPApp.DC.Qdf_Fill_DT(MySQL, DT_Commands)

            Dim DRow_RefNum() As DataRow = DT_Commands.Select(String.Format("Field_Name = '{0}'", Stru_WordRep.DOCMAN_RefNum))
            If DRow_RefNum.Length > 0 Then
                Stru_WordRep.DOCMAN_RefNum = DRow_RefNum(0).Item("Field_Value")
            End If
            For Each DR In DT_Commands.Rows
                Command = DR.Item("Command")
                Select Case Command.ToUpper
                    Case "BLOCK_INSERT"
                        Is_First_Table_Row = True
                        If Not BlockArr Is Nothing Then
                            For BI = 0 To BlockArr.Length - 1
                                Add_Para_To_Body(BlockArr(BI).Par, my_doc_body, BlockArr(BI).TB)
                            Next
                        End If
                        Current_Block_Name = DR.Item("Field_Name")
                        BI = 0
                        BlockArr = Nothing
                        For pi = 0 To PArr.Length - 1
                            If PArr(pi).BlockName.ToUpper = Current_Block_Name.ToUpper Then
                                ReDim Preserve BlockArr(BI)
                                With BlockArr(BI)
                                    .BlockName = PArr(pi).BlockName
                                    If Not PArr(pi).Par Is Nothing Then
                                        .Par = PArr(pi).Par.CloneNode(True)
                                    End If
                                    If Not PArr(pi).TB Is Nothing Then
                                        .TB = PArr(pi).TB.CloneNode(True)
                                    End If
                                End With
                                BI += 1
                            End If
                        Next
                    Case "REPLACE"
                        If Not BlockArr Is Nothing Then
                            Search = DR.Item("Field_Name")
                            Replace = DR.Item("Field_Value")
                            For BI = 0 To BlockArr.Length - 1
                                Dim _Prg As Paragraph = BlockArr(BI).Par
                                If Not _Prg Is Nothing Then
                                    If _Prg.InnerText.ToUpper.Contains(Search.ToUpper) Then
                                        BlockArr(BI).Par = SearchAndReplaceInParagraph(_Prg, Search, Replace)
                                    End If
                                End If
                                Dim _tb As Table = BlockArr(BI).TB
                                If Not _tb Is Nothing Then
                                    If _tb.InnerText.ToUpper.Contains(Search.ToUpper) Then
                                        BlockArr(BI).TB = SearchAndReplaceInTable(_tb, Search, Replace)
                                    End If
                                End If

                            Next
                        End If

                    Case "INSERT_ROW"
                        Dim RowText As String = DR.Item("Field_Value")
                        Dim Tbl As DocumentFormat.OpenXml.Wordprocessing.Table = Nothing
                        If Not BlockArr Is Nothing Then
                            For BI = 0 To BlockArr.Length - 1
                                Tbl = BlockArr(BI).TB
                                If Not Tbl Is Nothing Then
                                    BlockArr(BI).TB = Insert_Row_To_Table(RowText, Tbl, Is_First_Table_Row)
                                    Is_First_Table_Row = False
                                End If
                            Next
                        End If

                    Case "REPLACE_IN_ENTIRE_DOC"
                        Debug.Print("")
                End Select
            Next

            If Not BlockArr Is Nothing Then
                For BI = 0 To BlockArr.Length - 1
                    Add_Para_To_Body(BlockArr(BI).Par, my_doc_body, BlockArr(BI).TB)
                Next
            End If
            If Not SectPr Is Nothing Then
                my_doc_body.AppendChild(SectPr)
            End If

            'REPLACE_IN_ENTIRE_DOC
            Dim Replace_Rows() As DataRow
            Replace_Rows = DT_Commands.Select("Command IN ('REPLACE_IN_ENTIRE_DOC', 'REPLACE')")
            For Each DR In Replace_Rows
                Search = DR.Item("Field_Name")
                Replace = DR.Item("Field_Value")
                If D.InnerText.ToUpper.Contains(Search.ToUpper) Then
                    D = SearchAndReplaceInFullDoc(D, Search, Replace)
                End If
            Next

            'Delete Paragraph when empty
            BlockArr = Nothing
            BI = -1
            For Each Child1 In D.ChildElements

                For Each Child2 In Child1.ChildElements

                    If Child2.InnerText.ToUpper.Contains("#_DELETE_PARAGRAPH_WHEN_EMPTY_#") Then

                        If Child2.InnerText.ToUpper.Trim = "#_DELETE_PARAGRAPH_WHEN_EMPTY_#" Then
                            BI += 1
                            ReDim Preserve BlockArr(BI)
                            Select Case Child2.LocalName.ToUpper
                                Case "P"
                                    BlockArr(BI).Par = Child2
                                Case "TBL"
                                    BlockArr(BI).TB = Child2
                                Case Else
                                    MsgBox(Child2.LocalName & ": nem kezelt localname")
                            End Select
                        Else
                            Select Case Child2.LocalName.ToUpper
                                Case "P"
                                    SearchAndReplaceInParagraph(Child2, "#_DELETE_PARAGRAPH_WHEN_EMPTY_#", "")
                                Case "TBL"
                                    SearchAndReplaceInTable(Child2, "#_DELETE_PARAGRAPH_WHEN_EMPTY_#", "")
                                Case Else
                                    MsgBox(Child2.LocalName & ": nem kezelt localname")
                            End Select
                        End If
                    End If
                    If Child2.InnerText.ToUpper.Contains("#_DELETE_PARAGRAPH_WHEN_EMPTY_2X_#") Then
                        If Child2.InnerText.ToUpper.Trim = "#_DELETE_PARAGRAPH_WHEN_EMPTY_2X_#" Then
                            BI += 1
                            ReDim Preserve BlockArr(BI)
                            Select Case Child2.LocalName.ToUpper
                                Case "P"
                                    BlockArr(BI).Par = Child2
                                Case "TBL"
                                    BlockArr(BI).TB = Child2
                                Case Else
                                    MsgBox(Child2.LocalName & ": nem kezelt localname")
                            End Select
                        Else
                            Select Case Child2.LocalName.ToUpper
                                Case "P"
                                    SearchAndReplaceInParagraph(Child2, "#_DELETE_PARAGRAPH_WHEN_EMPTY_2X_#", "")
                                Case "TBL"
                                    SearchAndReplaceInTable(Child2, "#_DELETE_PARAGRAPH_WHEN_EMPTY_2X_#", "")
                                Case Else
                                    MsgBox(Child2.LocalName & ": nem kezelt localname")
                            End Select
                        End If
                    End If
                Next
            Next

            If BI >= 0 Then
                For i = 0 To BlockArr.Length - 1
                    If Not IsNothing(BlockArr(i).Par) Then
                        BlockArr(i).Par.Remove()
                    End If
                Next
            End If

            'Handling KEEP
            BlockArr = Nothing
            BI = -1
            For Each Child1 In D.ChildElements
                Debug.Print(Child1.ToString)
                For Each Child2 In Child1.ChildElements
                    Debug.Print(Child2.ToString)
                    If Child2.InnerText.ToUpper.Contains("#_KEEP_#") Then
                        BI += 1
                        ReDim Preserve BlockArr(BI)
                        Select Case Child2.LocalName.ToUpper
                            Case "P"
                                'ezt egyelore nem kezelem, talan nincs is ilyen. #_KEEP_# tablan belul fordul elo
                            Case "TBL"
                                Child2 = Handling_Keep_In_Table(Child2)
                            Case Else
                                Debug.Print(Child2.LocalName & ": nem kezelt localname")
                        End Select
                    End If
                Next
            Next

        End Using
        Return OUT
    End Function
    Public Function Report_Prepare(ByRef P_Doc As Struct_Word_Report) As Boolean
        Dim OUT As Boolean = True
        P_Doc.Report_Template_Path = FPf.FPApp.P.Report_Params.ReportPath + P_Doc.Report_Template_Name
        Dim Now_Time As DateTime = DateTime.Now
        Dim Dformat As String = "yyMMdd_HHmmss"

        Dim Doc_Extension As String = getFileExtension(P_Doc.Report_Template_Name)
        If Doc_Extension.ToUpper <> "DOCX" Then
            OUT = False
            MsgBox("A fájl kiterjesztése legyen .docx !!!")
        Else
            With P_Doc
                .Doc_Extension = Doc_Extension
                .Report_Target_Name_Without_Extension = getFileName_without_Extension(P_Doc.Report_Template_Name) + "_" + Now_Time.ToString(Dformat)
                .Report_Target_Name = .Report_Target_Name_Without_Extension + "." + .Doc_Extension
                .Report_Target_Path = FPf.FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP) + .Report_Target_Name
                .Doc_Key = .Report_Target_Name
            End With
        End If

        If OUT Then
            If Not System.IO.File.Exists(P_Doc.Report_Template_Path) Then
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word.OPEN_Word_REPORT", 0, String.Format("File {0} does not exists.", P_Doc.Report_Template_Path))
            End If
        End If

        If OUT Then
            FPf.FPApp.TEMP_Folders_EMPTY()
        End If

        If OUT Then
            Try
                System.IO.File.Copy(P_Doc.Report_Template_Path, P_Doc.Report_Target_Path)
            Catch ex As Exception
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word.OPEN_Word_REPORT", Err.Number, Err.Description)
            End Try
        End If
        Return OUT
    End Function
    Public Function Open_DocX(ByRef P_Doc As Struct_Word_Report) As Boolean
        Dim OUT As Boolean = True
        Dim NewDoc As New Struct_Word_Report
        Dim CurrDate As DateTime = FPf.FPApp.GET_SERVER_CURRENT_DATE(True)
        Dim OUT_WordDoc As Microsoft.Office.Interop.Word.Document = Nothing

        If Not System.IO.File.Exists(P_Doc.Report_Target_Path) Then
            OUT = False
            FPf.FPApp.DoErrorMsgBox("FP_Word_X.OPEN_DocX", 0, String.Format("File {0} does not exists.", P_Doc.Report_Target_Path))
        End If

        If OUT Then
            NewDoc = P_Doc
            OUT = OPEN_Doc(P_Doc.Report_Target_Path, OUT_WordDoc)
        End If

        If OUT Then
            With NewDoc
                .Doc = OUT_WordDoc
                .Doc_Key = P_Doc.Doc_Key
            End With

            DIC_DocX.Add(NewDoc.Doc_Key, NewDoc)
        End If
        Return OUT
    End Function
    Public Sub REPORT_REMOVE_FROM_OPENED_DOCS(Report_Key As String)
        If Not (DIC_DocX Is Nothing) Then
            If DIC_DocX.ContainsKey(nz(Report_Key, "")) Then
                DIC_DocX.Remove(Report_Key)
            End If
        End If
    End Sub
    Private Function REPORT_SAVE_TO_DOCMAN_SAVE_IMAGE(Doc_Images_ID As Long, Origin As String, DocData() As Byte) As Boolean
        Dim OUT As Boolean = False

        Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand

        With FPf.FPApp.DC
            .Qdf_set_SP(sqlComm, "FP_DOCMAN_Docs_Panel_Manage_Doc")
            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)

            .Qdf_AddParameter(sqlComm, "@Doc_Images_ID", SqlDbType.Int, , , , , Doc_Images_ID)
            .Qdf_AddParameter(sqlComm, "@Doc_Pages_ID", SqlDbType.Int, , , , , 0)
            .Qdf_AddParameter(sqlComm, "@Manage_Type", SqlDbType.Int, , , , , 1)
            .Qdf_AddParameter(sqlComm, "@Origin", SqlDbType.NVarChar, , 255, Origin)
            .Qdf_AddParameter(sqlComm, "@ImageData", SqlDbType.VarBinary, , -1, , , , , , DocData)
            .Qdf_AddParameter(sqlComm, "@Barcode01", SqlDbType.NVarChar, , 50, "")

            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
            .Qdf_AddParameter(sqlComm, "@ErrField", SqlDbType.NVarChar, ParameterDirection.Output, 255)

        End With

        CURSOR_SHOW_WAIT()
        Try
            OUT = FPf.FPApp.DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")

        Catch ex As Exception
            OUT = False
            FPf.FPApp.DoErrorMsgBox("FP_Word.Doc_SAVE_TO_DOCMAN", Err.Number, Err.Description)
        End Try

        Return OUT
    End Function
    Public Function REPORT_SAVE_TO_DOCMAN(Doc_key As String, Optional WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = DIC_DocX.ContainsKey(Doc_key)
        Dim CurrDoc As New Struct_Word_Report
        Dim DocData() As Byte = Nothing
        Dim PDF_DocData() As Byte = Nothing
        Dim Doc_Images_ID As Long = 0

        If OUT Then
            OUT = DIC_DocX.ContainsKey(Doc_key)
            If OUT Then
                CurrDoc = DIC_DocX(Doc_key)
            End If
        End If

        If OUT Then
            If Not P_Doc_Is_Open_In_Word(Doc_key) Then
                OUT = False
                FPf.FPApp.DoMyMsgBox(83006) 'Sikertelen archivalas - a dokumentum ido kozben be lett csukva.
            Else
                Try
                    CurrDoc.Doc.Save()
                    Dim Now_Time As DateTime = DateTime.Now
                    Dim TimeStamp As String = Now_Time.ToString("yyMMdd_HHmmss")

                    Dim TempFileName As String = FPf.FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP) + "TEMP_" + TimeStamp + ".docx"
                    Dim TempFileName2 As String = FPf.FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP) + "TEMP_" + TimeStamp + "_2" + ".docx"
                    Dim TempFileName_PDF As String = FPf.FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP) + "TEMP_" + TimeStamp + ".pdf"

                    If CurrDoc.DOCMAN_Doc_Date = NULLDATE Then
                        CurrDoc.DOCMAN_Doc_Date = Now_Time
                    End If

                    CurrDoc.Doc.SaveAs2(TempFileName, MSWORD.WdSaveFormat.wdFormatDocumentDefault)
                    System.IO.File.Copy(TempFileName, TempFileName2)

                    OUT = FPf.FPApp.ByteArray_getFile(TempFileName2, DocData)

                    If OUT Then
                        If CurrDoc.DOCMAN_SAVE_PDF Then
                            CurrDoc.Doc.SaveAs2(TempFileName_PDF, MSWORD.WdSaveFormat.wdFormatPDF)
                            OUT = FPf.FPApp.ByteArray_getFile(TempFileName_PDF, PDF_DocData)
                        End If
                    End If

                Catch ex As Exception
                    OUT = False
                End Try

                If OUT = False Then
                    If WithDialog Then
                        FPf.FPApp.DoMyMsgBox(83004) 'A dokumentumot ido kozben toroltek vagy atmozgattak masik helyre.
                    End If
                End If
            End If
        End If

        'Dim Doc_Ref_Num As String
        Dim Ref_Num_SQL As String = String.Format("SELECT * FROM WORD_REPORT_FIELDS WHERE TER")

        If OUT Then
            Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

            With FPf.FPApp.DC
                .Qdf_set_SP(sqlComm, "FP_DOCMAN_Docs_Panel_SAVE")
                .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                .Qdf_AddParameter(sqlComm, "@ID", SqlDbType.Int, ParameterDirection.Output, , , , 0)
                .Qdf_AddParameter(sqlComm, "@OldTransactID", SqlDbType.Int, , , , , 0)

                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Images_ID", SqlDbType.Int, , , , , 0)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Types_ID", SqlDbType.Int, , , , , CurrDoc.DOCMAN_Types_ID)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_RefNum", SqlDbType.NVarChar, , 50, CurrDoc.DOCMAN_RefNum)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_CUST_ID", SqlDbType.Int, , , , , 0)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_CUST_Name1", SqlDbType.NVarChar, , 50, "")
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Date", SqlDbType.DateTime, , , , CurrDoc.DOCMAN_Doc_Date)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Scan_Date", SqlDbType.DateTime, , , , CurrDoc.DOCMAN_Scan_Date)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Descr", SqlDbType.NVarChar, , 255, CurrDoc.DOCMAN_Description)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Status_ID", SqlDbType.Int, , , , , 0)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Security_Level", SqlDbType.Int, , , , , 0)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Parent_TableName", SqlDbType.NVarChar, , 50, CurrDoc.DOCMAN_Parent_TableName)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Parent_Record_ID", SqlDbType.Int, , , , , CurrDoc.DOCMAN_ParentRecord_ID)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Sent_date", SqlDbType.DateTime, , , , CurrDoc.DOCMAN_Doc_Date)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Message", SqlDbType.NVarChar, , -1, "")
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Origin", SqlDbType.NVarChar, , 255, "")

                .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
                .Qdf_AddParameter(sqlComm, "@ErrField", SqlDbType.NVarChar, ParameterDirection.Output, 255)
            End With

            CURSOR_SHOW_WAIT()
            Try
                OUT = FPf.FPApp.DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")

                Dim MySQL As String = String.Format("SELECT Doc_Images_ID FROM Doc_Parents WHERE ID = {0}", sqlComm.Parameters("@ID").Value)
                Dim DRow As DataRow = FPf.FPApp.DC.Qdf_get_DataRow(MySQL)

                Doc_Images_ID = DRow!Doc_Images_ID

            Catch ex As Exception
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word.Doc_SAVE_TO_DOCMAN", Err.Number, Err.Description)
            End Try
        End If

        Dim Origin As String = getFileName_without_Extension(Trim(nz(CurrDoc.Saved_FileName, "")))

        If Origin = "" Then
            Origin = getFileName_without_Extension(CurrDoc.Doc_Key)
        End If

        If OUT Then
            If CurrDoc.DOCMAN_SAVE_DOCX Then
                OUT = REPORT_SAVE_TO_DOCMAN_SAVE_IMAGE(Doc_Images_ID, Origin + ".docx", DocData)
            End If
        End If
        If OUT Then
            If CurrDoc.DOCMAN_SAVE_PDF Then
                OUT = REPORT_SAVE_TO_DOCMAN_SAVE_IMAGE(Doc_Images_ID, Origin + ".pdf", PDF_DocData)
            End If
        End If

        CURSOR_SHOW_DEFAULT()
        Return OUT
    End Function
    Public Function Doc_REOPEN(MyDoc As Struct_Word_Report, Optional SaveDoc As Boolean = True) As Boolean
        Dim OUT As Boolean = True

        Try
            If SaveDoc Then
                MyDoc.Doc.Save()
            End If

            MyDoc.Doc.Close()
            MyDoc.Doc = WordApp.Documents.Open(MyDoc.Report_Target_Path)
            DIC_DocX(MyDoc.Doc_Key) = MyDoc

        Catch ex As Exception

        End Try

        Return OUT
    End Function
End Class
