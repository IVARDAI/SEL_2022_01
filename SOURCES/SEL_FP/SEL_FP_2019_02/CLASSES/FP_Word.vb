Imports System.Data.SqlClient
Imports MSWORD = Microsoft.Office.Interop.Word

Public Class FP_Word
    Public Structure Struct_Word_Report
        Dim Report_FileName As String
        Dim Report_Identifier As String
        Dim Report_Head_ID As Long

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
    End Structure

    Public Structure Struct_WordDoc_OUT_Params
        Dim CopiedFileNameWithPath As String
        Dim Doc_Key As String
        Dim Doc As Microsoft.Office.Interop.Word.Document
    End Structure

    Public Structure Struct_OpenedDocs
        Dim Doc_FileName_without_Extension As String
        Dim Doc_Extension As String
        Dim Doc_FullName As String
        Dim Doc_Key As String
        Dim Doc As Microsoft.Office.Interop.Word.Document

        Dim Report_Head_ID As Long

        Dim DOCMAN_Types_ID As Long
        Dim DOCMAN_Parent_TableName As String
        Dim DOCMAN_ParentRecord_ID As Long
        Dim DOCMAN_RefNum As String
        Dim DOCMAN_Description As String
        Dim DOCMAN_Doc_Date As DateTime
        Dim DOCMAN_Scan_Date As DateTime

        Dim Report_Identifier As String
        Dim Report_FileName As String
        Dim Prepared_Template_Exists As Boolean

        Dim Def_ParentTable As String
        Dim Def_Doc_Type As String
        Dim Def_Doc_Language As String
        Dim Def_Doc_FileName_Format As String
        Dim Def_Doc_NoShowInternal As Boolean
        Dim Def_Doc_FileName As String
        Dim Def_DOCMAN_SAVE As Boolean
        Dim Def_DOCMAN_SAVE_PDF As Boolean
        Dim Def_DOCMAN_SAVE_DOCX As Boolean
        Dim Def_DOCMAN_Type As String
        Dim Def_DOCMAN_RefNum As String
        Dim Def_DOCMAN_Descr As String
        Dim Def_Doc_Other_Params As String
    End Structure

    Public FPf As FP_Form

    Public WordApp As MSWORD.Application = Nothing
    Public DIC_Opened_Docs As New Dictionary(Of String, Struct_OpenedDocs)

    Private Disposed As Boolean = False

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

            If DIC_Opened_Docs.ContainsKey(nz(Doc_Key, "")) Then
                Dim Doc As MSWORD.Document = DIC_Opened_Docs(Doc_Key).Doc

                If Not (Doc Is Nothing) Then
                    Dim Doc_Name As String = ""

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

        Disposed = True
    End Sub

    Public Sub KillUnusedWordProcess()
        Dim oXlProcess As Process() = Process.GetProcessesByName("Word")

        For Each oXLP As Process In oXlProcess
            If Len(oXLP.MainWindowTitle) = 0 Then
                oXLP.Kill()
            End If
        Next
    End Sub

    Sub CLOSE()
        If P_Is_WordApp_Open Then
            Try
                WordApp.Visible = True
                Do While WordApp.Documents.Count > 0
                    WordApp.Documents(1).Close(False)
                Loop
                WordApp.Quit()

            Catch ex As Exception
                'Nothing to do
            End Try
            ReleaseObject(WordApp)
            WordApp = Nothing

            KillUnusedWordProcess()
        End If
    End Sub

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

    Public Function Doc_Activate(Doc_Key As String) As Boolean
        Dim OUT As Boolean = False

        Dim CurrDoc As MSWORD.Document = P_Doc_Get_From_Doc_Key(Doc_Key)

        If Not CurrDoc Is Nothing Then
            CurrDoc.Activate()
            OUT = True
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

    Private Function Prepared_Temp_FileName_GET(REPORT_Identifier As String) As String
        Return FPf.FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.PREPARED_TEMPLATES) + REPORT_Identifier + "_Prepared_Temp.docx"
    End Function

    Private Function Blocks_FileName_GET(REPORT_Identifier As String) As String
        Return FPf.FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.PREPARED_TEMPLATES) + REPORT_Identifier + "_Blocks.docx"
    End Function

    Public Function SAVE_TO_PDF(Doc_Key As String, FileName_with_folder As String) As Boolean
        Dim OUT As Boolean = True

        If Not P_Doc_Is_Open_In_Word(Doc_Key) Then
            FPf.FPApp.DoMyMsgBox(67) 'A dokumentumot nem lehet elmenteni. - a dokumentum ido kozben be lett csukva.
            FPf.P_ENABLED = True
        Else
            Dim MyDOC As FP_Word.Struct_OpenedDocs = DIC_Opened_Docs(Doc_Key)

            Try
                MyDOC.Doc.SaveAs2(FileName_with_folder, MSWORD.WdSaveFormat.wdFormatPDF)
                OUT = True

            Catch ex As Exception
                OUT = False
                gl_FPApp.DoErrorMsgBox("FP_Word.SAVE_TO_PDF", 0, Err.Description)
            End Try
        End If

        Return OUT
    End Function

    Function REPORT_FILL(Doc_Key As String, From_ZD As Boolean) As Boolean
        Dim OUT As Boolean = True
        Dim CurrDoc As New Struct_OpenedDocs

        OUT = Doc_Activate(Doc_Key)

        If OUT Then
            CurrDoc = DIC_Opened_Docs(Doc_Key)
            OUT = (CurrDoc.Report_Identifier > "")
        End If

        '------------------------------------------------------------------------------------------------------------------------------
        'SET REPORT FIELDS
        '------------------------------------------------------------------------------------------------------------------------------
        If OUT Then
            Dim i As Integer = 0
            Dim Blocks As New Word_Blocks(FPf, CurrDoc.Doc, WordApp, CurrDoc.Def_Doc_Other_Params, CurrDoc.Report_Head_ID, CurrDoc.Report_Identifier, CurrDoc.Prepared_Template_Exists)

            OUT = Blocks.Block_Defs_LOAD(From_ZD)

            If OUT Then
                With CurrDoc
                    .Def_ParentTable = Blocks.Def_ParentTable
                    .Def_Doc_Type = Blocks.Def_Doc_Type
                    .Def_Doc_Language = Blocks.Def_Doc_Language
                    .Def_Doc_FileName_Format = Blocks.Def_Doc_FileName_Format
                    .Def_Doc_FileName = Blocks.Doc_Blocks_Doc_FileName
                    .Def_Doc_NoShowInternal = Blocks.Def_Doc_NoShowInternal

                    .Def_DOCMAN_SAVE = Blocks.Def_DOCMAN_SAVE
                    .Def_DOCMAN_SAVE_PDF = Blocks.Def_DOCMAN_SAVE_PDF
                    .Def_DOCMAN_SAVE_DOCX = Blocks.Def_DOCMAN_SAVE_DOCX
                    .Def_DOCMAN_Type = Blocks.Def_DOCMAN_Type
                    .Def_DOCMAN_RefNum = Blocks.Def_DOCMAN_RefNum
                    .Def_DOCMAN_Descr = Blocks.Def_DOCMAN_Descr

                    .DOCMAN_Parent_TableName = Blocks.Def_DOCMAN_TableName
                    .DOCMAN_Types_ID = Blocks.Def_DOCMAN_Types_ID
                    .DOCMAN_ParentRecord_ID = Blocks.Def_DOCMAN_ParentRecord_ID
                    .DOCMAN_Description = Blocks.Def_DOCMAN_Descr
                End With
            End If

            If OUT Then
                Dim Doc_FullName_NEW As String = FPf.FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP) + CurrDoc.Def_Doc_FileName + CurrDoc.Doc_Extension

                CurrDoc.Doc_FullName = Doc_FullName_NEW
                CurrDoc.Doc.SaveAs2(Doc_FullName_NEW)

                Dim DT_Commands As DataTable = Nothing
                Dim MySQL As String = String.Format("SELECT Command, Field_Name, Field_Value FROM WORD_REPORT_FIELDS WHERE Terminals_ID = {0} And Report_Name = '{1}' ORDER BY ID", Terminals_ID, CurrDoc.Report_Identifier)
                Dim Current_Block As Word_Blocks.Struct_Block_Data
                Dim Block_Inserted As Boolean = False

                Current_Block.Skip_Block = False 'Mert ha a nyomtatvany nem block alapu, akkor nem toltene semmit.

                FPf.FPApp.DC.Qdf_Fill_DT(MySQL, DT_Commands)

                For Each DRow As DataRow In DT_Commands.Rows
                    Dim Current_Command As String = (DRow!Command).ToString.ToUpper

                    Select Case Current_Command
                        Case "REPLACE", "REPLACE_IN_ENTIRE_DOC"
                            If Current_Block.Skip_Block = False Or Current_Command = "REPLACE_IN_ENTIRE_DOC" Then
                                If Len(DRow!Field_Value) < 255 Then
                                    Try
                                        CurrDoc.Doc.Content.Find.Execute(FindText:=DRow!Field_Name, ReplaceWith:=DRow!Field_Value, Replace:=MSWORD.WdReplace.wdReplaceAll)
                                    Catch ex As Exception
                                        'Nothing to do
                                    End Try
                                Else
                                    Dim FullString As String = DRow!Field_Value
                                    Dim LenFullString As Integer = FullString.Length
                                    Dim Steps As Integer = 255 - Len(DRow!Field_Name)
                                    Dim FromPos As Integer = 1

                                    While FromPos <= FullString.Length
                                        Dim CurrReplace As String = Mid(FullString, FromPos, Steps) + DRow!Field_Name
                                        FromPos += Steps
                                        CurrDoc.Doc.Content.Find.Execute(FindText:=DRow!Field_Name, ReplaceWith:=CurrReplace, Replace:=MSWORD.WdReplace.wdReplaceAll)
                                    End While

                                    'Dim StrForReplace As String = DRow!Field_Value

                                    'Do
                                    '    Dim CurrReplace As String = Mid(StrForReplace, 1, Steps) + DRow!Field_Name
                                    '    StrForReplace = Mid(StrForReplace, Steps + 1, Steps) '!!!SSS!!!
                                    '    CurrDoc.Doc.Content.Find.Execute(FindText:=DRow!Field_Name, ReplaceWith:=CurrReplace, Replace:=MSWORD.WdReplace.wdReplaceAll)
                                    'Loop While StrForReplace > ""

                                    Try
                                        CurrDoc.Doc.Content.Find.Execute(FindText:=DRow!Field_Name, ReplaceWith:="", Replace:=MSWORD.WdReplace.wdReplaceAll)
                                    Catch ex As Exception
                                        'Nothing to do
                                    End Try
                                End If
                            End If

                        Case "INSERT_ROW"
                            If Current_Block.Skip_Block = False Then
                                Try
                                    Dim TableNum As Integer

                                    Select Case Trim(DRow!Field_Name)
                                        Case "", "0"
                                            TableNum = CurrDoc.Doc.Tables.Count

                                        Case Else
                                            TableNum = CInt(DRow!Field_Name)
                                    End Select

                                    Dim MyTable As MSWORD.Table = CurrDoc.Doc.Tables.Item(TableNum)
                                    Dim RowValues() As String = Split(DRow!Field_Value, "#_#")
                                    Dim LastRow_Index As Integer = Doc_Table_Create_Empty_Row_At_The_End(MyTable)
                                    Dim MaxColumn_Index = Math.Min(UBound(RowValues) + 1, MyTable.Columns.Count)

                                    For i = 1 To MaxColumn_Index
                                        Dim CellText As String = MSWORD_CELL_REMOVE_SYS_CHARS(nz(MyTable.Cell(LastRow_Index, i).Range.Text, ""))

                                        If InStr(CellText, "#_KEEP_#") > 0 Then
                                            MyTable.Cell(LastRow_Index, i).Range.Text = Replace(CellText, "#_KEEP_#", "")
                                        Else
                                            MyTable.Cell(LastRow_Index, i).Range.Text = RowValues(i - 1)
                                        End If
                                    Next

                                Catch ex As Exception
                                    'Nothing to do
                                End Try
                            End If

                        Case "BLOCK_LOAD"
                            If Block_Inserted Then
                                FPf.FPApp.DoErrorMsgBox("FP_Word.REPORT_FILL", 0, "A BLOCK_LOAD utasitasoknak meg kell elozniuk a BLOCK_INSERT-eket!")
                            End If
                            Blocks.BLOCK_LOAD(DRow!Field_Name)

                        Case "BLOCK_INSERT"
                            If CurrDoc.Prepared_Template_Exists = False Then
                                Dim Prepared_Temp_FileName As String = Prepared_Temp_FileName_GET(CurrDoc.Report_Identifier)
                                Dim Blocks_FileName As String = Blocks_FileName_GET(CurrDoc.Report_Identifier)

                                Try
                                    If IO.File.Exists(Prepared_Temp_FileName) Then
                                        IO.File.Delete(Prepared_Temp_FileName)
                                    End If
                                    If IO.File.Exists(Blocks_FileName) Then
                                        IO.File.Delete(Blocks_FileName)
                                    End If
                                    CurrDoc.Doc.SaveAs2(Prepared_Temp_FileName, MSWORD.WdSaveFormat.wdFormatDocumentDefault)
                                    CurrDoc.Doc.SaveAs2(CurrDoc.Doc_FullName)
                                    Blocks.Doc_Blocks_Temp.SaveAs2(Blocks_FileName, MSWORD.WdSaveFormat.wdFormatDocumentDefault)

                                    If IO.File.Exists(Prepared_Temp_FileName) And IO.File.Exists(Blocks_FileName) Then
                                        Dim Report_File_Date As DateTime = IO.File.GetLastWriteTime(CurrDoc.Report_FileName)

                                        If FPf.FPApp.DC.P_USE_LocalDB Then
                                            If CurrDoc.Report_Identifier > "" Then
                                                With FPf.FPApp.DC.LocalDB_SEL
                                                    .RunSQL(String.Format("INSERT INTO WORD_Docs_Block_Defs (REPORT_Identifier, REPORT_Date, ParentTable, Doc_Type, Doc_Language, Doc_FileName_Format, Doc_NoShowInternal, DOCMAN_SAVE, DOCMAN_SAVE_PDF, DOCMAN_SAVE_DOCX, DOCMAN_Type, DOCMAN_RefNum, DOCMAN_Descr) VALUES ('{0}', {1}, '{2}', '{3}', '{4}', '{5}', {6}, {7}, '{8}', '{9}', '{10}', '{11}', '{12}')", CurrDoc.Report_Identifier, SQLDate(Report_File_Date), CurrDoc.Def_ParentTable, CurrDoc.Def_Doc_Type, CurrDoc.Def_Doc_Language, CurrDoc.Def_Doc_FileName_Format, Math.Abs(CInt(CurrDoc.Def_Doc_NoShowInternal)), Math.Abs(CInt(CurrDoc.Def_DOCMAN_SAVE)), Math.Abs(CInt(CurrDoc.Def_DOCMAN_SAVE_PDF)), Math.Abs(CInt(CurrDoc.Def_DOCMAN_SAVE_DOCX)), CurrDoc.Def_DOCMAN_Type, CurrDoc.Def_DOCMAN_RefNum, CurrDoc.Def_DOCMAN_Descr))
                                                End With
                                            End If
                                        End If
                                    End If

                                    CurrDoc.Prepared_Template_Exists = True

                                Catch ex As Exception
                                    'Nothing to do
                                End Try
                            End If
                            Current_Block = Blocks.Block_INSERT(DRow!Field_Name)
                            Block_Inserted = True

                        Case Else
                            FPf.FPApp.DoErrorMsgBox("FP_Word.REPORT_FILL", 0, String.Format("Unknown Command {0}", Current_Command))
                    End Select
                Next

                i = 1

                Do While i <= CurrDoc.Doc.Paragraphs.Count
                    Dim ParText As String = Trim(MSWORD_CELL_REMOVE_SYS_CHARS(nz(CurrDoc.Doc.Paragraphs(i).Range.Text, "")))

                    If InStr(ParText, "#_NEW_PARAGRAPH_WHEN_NOT_EMPTY_#") > 0 Then
                        If TEXT_TRIM(ParText) <> "#_NEW_PARAGRAPH_WHEN_NOT_EMPTY_#" Then
                            CurrDoc.Doc.Paragraphs(i).Range.Text += Chr(13)
                            i += 2
                        End If
                    ElseIf InStr(ParText, "#_DELETE_PARAGRAPH_WHEN_EMPTY_#") > 0 Then
                        If TEXT_TRIM(ParText) = "#_DELETE_PARAGRAPH_WHEN_EMPTY_#" Then
                            Dim RangeForDel As Microsoft.Office.Interop.Word.Range = CurrDoc.Doc.Paragraphs(i).Range

                            RangeForDel.Delete()
                        Else
                            i += 1
                        End If
                    ElseIf InStr(ParText, "#_DELETE_PARAGRAPH_WHEN_EMPTY_2X_#") > 0 Then
                        If TEXT_TRIM(ParText) = "#_DELETE_PARAGRAPH_WHEN_EMPTY_2X_#" Then
                            Dim RangeForDel As Microsoft.Office.Interop.Word.Range = CurrDoc.Doc.Paragraphs(i).Range

                            RangeForDel.Delete()
                            RangeForDel.Delete()
                        End If
                    Else
                        i += 1
                    End If
                Loop
                CurrDoc.Doc.Content.Find.Execute(FindText:="#_DELETE_PARAGRAPH_WHEN_EMPTY_#", ReplaceWith:="", Replace:=MSWORD.WdReplace.wdReplaceAll)
                CurrDoc.Doc.Content.Find.Execute(FindText:="#_DELETE_PARAGRAPH_WHEN_EMPTY_2X_#", ReplaceWith:="", Replace:=MSWORD.WdReplace.wdReplaceAll)

                CurrDoc.Doc.Content.Find.Execute(FindText:="#_KEEP_#", ReplaceWith:="", Replace:=MSWORD.WdReplace.wdReplaceAll)

                Blocks.Dispose()
                Blocks = Nothing
            End If

            If OUT Then
                Doc_REOPEN(CurrDoc)
            End If
        End If

        Return OUT
    End Function

    Public Function Doc_REOPEN(MyDoc As Struct_OpenedDocs, Optional SaveDoc As Boolean = True) As Boolean
        Dim OUT As Boolean = True

        Try
            If SaveDoc Then
                MyDoc.Doc.Save()
            End If

            MyDoc.Doc.Close()
            MyDoc.Doc = WordApp.Documents.Open(MyDoc.Doc_FullName)
            DIC_Opened_Docs(MyDoc.Doc_Key) = MyDoc

        Catch ex As Exception

        End Try

        Return OUT
    End Function

    Public Function Doc_Table_Cell_Text_Is_Empty(MyCell_Text As String) As Boolean
        Dim OUT As Boolean = True

        If Trim(nz(MyCell_Text, "")) = Chr(13) + Chr(7) Then
            OUT = True
        ElseIf InStr(MyCell_Text, "#_KEEP_#") > 0 Then
            OUT = True
        Else
            OUT = False
        End If

        Return OUT
    End Function

    Public Function Doc_Table_Create_Empty_Row_At_The_End(MyTable As MSWORD.Table) As Integer
        Dim OUT As Integer = -1
        Dim LastColumn_Index As Integer = MyTable.Columns.Count
        Dim LastRow_Index = MyTable.Rows.Count
        Dim CreateNewRow As Boolean = False

        For i As Integer = 0 To LastColumn_Index
            If Doc_Table_Cell_Text_Is_Empty(MyTable.Cell(LastRow_Index, i).Range.Text) = False Then
                CreateNewRow = True
                Exit For
            End If
        Next

        If CreateNewRow Then
            MyTable.Rows.Add()
        End If

        OUT = MyTable.Rows.Count

        Return OUT
    End Function

    Private Function Template_Exists(REPORT_Identifier As String, Report_FileName As String) As Boolean
        Dim OUT As Boolean = False

        If FPf.FPApp.DC.P_USE_LocalDB Then
            If IO.File.Exists(Prepared_Temp_FileName_GET(REPORT_Identifier)) And IO.File.Exists(Blocks_FileName_GET(REPORT_Identifier)) And IO.File.Exists(Report_FileName) Then
                Dim MySQL As String = String.Format("SELECT REPORT_Date FROM WORD_Docs_Block_Defs WHERE REPORT_Identifier = '{0}'", REPORT_Identifier)
                Dim DRow As DataRow = FPf.FPApp.DC.LocalDB_SEL.get_DataRow(MySQL)

                If Not (DRow Is Nothing) Then
                    If Math.Abs(DateDiff(DateInterval.Second, DRow!REPORT_Date, IO.File.GetLastWriteTime(Report_FileName))) < 5 Then
                        OUT = True
                    End If
                End If
            End If
        End If

        Return OUT
    End Function

    Function REPORT_OPEN(P_Doc As Struct_Word_Report, ByRef OUT_Params As Struct_WordDoc_OUT_Params) As Boolean
        Dim OUT As Boolean = True
        Dim Report_FileName As String = FPf.FPApp.P.Report_Params.ReportPath + P_Doc.Report_FileName
        Dim NewDoc As New Struct_OpenedDocs
        Dim CurrDate As DateTime = FPf.FPApp.GET_SERVER_CURRENT_DATE(True)
        Dim Temp_Exists As Boolean = Template_Exists(P_Doc.Report_Identifier, Report_FileName)

        If P_Doc.Saved_FileName = "" Then
            P_Doc.Saved_FileName = P_Doc.Report_FileName
        End If

        OUT_Params = New Struct_WordDoc_OUT_Params
        With OUT_Params
            .CopiedFileNameWithPath = FPf.FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP) + getFileName_without_Extension(P_Doc.Saved_FileName) + "_" + Format(Now, "yyMMdd_HHmmss") + getFileExtension(P_Doc.Report_FileName, True)
        End With

        If OUT Then
            If Not System.IO.File.Exists(Report_FileName) Then
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word.OPEN_Word_REPORT", 0, String.Format("File {0} does not exists.", Report_FileName))
            End If
        End If

        If OUT Then
            FPf.FPApp.TEMP_Folders_EMPTY()
        End If

        If OUT Then
            Try
                If Temp_Exists Then
                    System.IO.File.Copy(Prepared_Temp_FileName_GET(P_Doc.Report_Identifier), OUT_Params.CopiedFileNameWithPath)
                Else
                    System.IO.File.Copy(Report_FileName, OUT_Params.CopiedFileNameWithPath)
                End If
            Catch ex As Exception
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP_Word.OPEN_Word_REPORT", Err.Number, Err.Description)
            End Try
        End If

        If OUT Then
            With NewDoc
                .Doc_FullName = OUT_Params.CopiedFileNameWithPath
                .Doc_FileName_without_Extension = getFileName_without_Extension(OUT_Params.CopiedFileNameWithPath)
                .Doc_Extension = getFileExtension(OUT_Params.CopiedFileNameWithPath, True)
                .Doc_Key = .Doc_FileName_without_Extension + .Doc_Extension

                .Report_Head_ID = P_Doc.Report_Head_ID

                .DOCMAN_Types_ID = P_Doc.DOCMAN_Types_ID
                .DOCMAN_RefNum = IIf(P_Doc.DOCMAN_RefNum > "", P_Doc.DOCMAN_RefNum, "-")
                .DOCMAN_Description = IIf(P_Doc.DOCMAN_Description > "", P_Doc.DOCMAN_Description, .Doc_FileName_without_Extension)
                .DOCMAN_Doc_Date = IIf(P_Doc.DOCMAN_Doc_Date = NULLDATE, CurrDate, P_Doc.DOCMAN_Doc_Date)
                .DOCMAN_Scan_Date = IIf(P_Doc.DOCMAN_Scan_Date = NULLDATE, CurrDate, P_Doc.DOCMAN_Scan_Date)
                .DOCMAN_ParentRecord_ID = P_Doc.DOCMAN_ParentRecord_ID
                .DOCMAN_Parent_TableName = P_Doc.DOCMAN_Parent_TableName

                .Report_Identifier = P_Doc.Report_Identifier
                .Report_FileName = Report_FileName
                .Prepared_Template_Exists = Temp_Exists
            End With
        End If

        If OUT Then
            OUT = OPEN_Doc(NewDoc.Doc_FullName, NewDoc.Doc)
        End If

        If OUT Then
            With OUT_Params
                .Doc = NewDoc.Doc
                .Doc_Key = NewDoc.Doc_Key
            End With

            'If P_Doc.Save_To_DOCMAN Then
            DIC_Opened_Docs.Add(NewDoc.Doc_Key, NewDoc)
            'End If
        End If

        Return OUT
    End Function

    Public Sub Doc_Close(Doc_Name As String)
        If DIC_Opened_Docs.ContainsKey(Doc_Name) Then
            Dim CurrDoc As Struct_OpenedDocs = DIC_Opened_Docs(Doc_Name)

            If Not (CurrDoc.Doc Is Nothing) Then
                Try
                    CurrDoc.Doc.Save()
                    CurrDoc.Doc.Close()

                Catch ex As Exception
                    'Nothing to do
                End Try
            End If

            DIC_Opened_Docs.Remove(Doc_Name)
        End If
    End Sub

    Public Sub REPORT_REMOVE_FROM_OPENED_DOCS(Report_Key As String)
        If Not (DIC_Opened_Docs Is Nothing) Then
            If DIC_Opened_Docs.ContainsKey(nz(Report_Key, "")) Then
                DIC_Opened_Docs.Remove(Report_Key)
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
        Dim OUT As Boolean = DIC_Opened_Docs.ContainsKey(Doc_key)
        Dim CurrDoc As New Struct_OpenedDocs
        Dim DocData() As Byte = Nothing
        Dim PDF_DocData() As Byte = Nothing
        Dim Doc_Images_ID As Long = 0

        'If OUT Then
        '    If WithDialog Then
        '        OUT = (FPApp.DoMyMsgBox(83005, CurrDoc.Doc_FileName_without_Extension, "SEQ,YES", "SEQ,NO") = 1)
        '    End If
        'End If

        If OUT Then
            OUT = DIC_Opened_Docs.ContainsKey(Doc_key)
            If OUT Then
                CurrDoc = DIC_Opened_Docs(Doc_key)
            End If
        End If

        If OUT Then
            If Not P_Doc_Is_Open_In_Word(Doc_key) Then
                OUT = False
                FPf.FPApp.DoMyMsgBox(83006) 'Sikertelen archivalas - a dokumentum ido kozben be lett csukva.
                'OUT = System.IO.File.Exists(CurrDoc.Doc_FullName)
                'If OUT Then
                '    OUT = FPApp.ByteArray_getFile(CurrDoc.Doc_FullName, DocData)
                'End If
            Else
                Try
                    CurrDoc.Doc.Save()

                    Dim TimeStamp As String = Format(Now, "yyMMdd_HHmmss")

                    Dim TempFileName As String = FPf.FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP) + "TEMP_" + TimeStamp + ".docx"
                    Dim TempFileName2 As String = FPf.FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP) + "TEMP_" + TimeStamp + "_2" + ".docx"
                    Dim TempFileName_PDF As String = FPf.FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP) + "TEMP_" + TimeStamp + ".pdf"

                    CurrDoc.Doc.SaveAs2(TempFileName, MSWORD.WdSaveFormat.wdFormatDocumentDefault)
                    System.IO.File.Copy(TempFileName, TempFileName2)

                    OUT = FPf.FPApp.ByteArray_getFile(TempFileName2, DocData)

                    If OUT Then
                        If CurrDoc.Def_DOCMAN_SAVE_PDF Then
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

        Dim Origin As String = getFileName_without_Extension(Trim(nz(CurrDoc.Def_Doc_FileName, "")))

        If Origin = "" Then
            Origin = getFileName_without_Extension(CurrDoc.Doc_Key)
        End If

        If OUT Then
            If CurrDoc.Def_DOCMAN_SAVE_DOCX Then
                OUT = REPORT_SAVE_TO_DOCMAN_SAVE_IMAGE(Doc_Images_ID, Origin + ".docx", DocData)
            End If
        End If
        If OUT Then
            If CurrDoc.Def_DOCMAN_SAVE_PDF Then
                OUT = REPORT_SAVE_TO_DOCMAN_SAVE_IMAGE(Doc_Images_ID, Origin + ".pdf", PDF_DocData)
            End If
        End If

        If OUT Then
            Doc_REOPEN(CurrDoc) 'Maskulonben TEMP lesz a dokumentum neve
        End If

        CURSOR_SHOW_DEFAULT()
        Return OUT
    End Function

    Private Class Word_Blocks
        Public Structure Struct_Block_Data
            Dim Block_Name As String
            Dim Range As MSWORD.Range
            Dim Skip_Block As Boolean
        End Structure

        Public Def_ParentTable As String = ""
        Public Def_Doc_Type As String = ""
        Public Def_Doc_Language As String = ""
        Public Def_Doc_FileName_Format As String = ""
        Public Def_Doc_NoShowInternal As Boolean = False
        Public Def_DOCMAN_SAVE As Boolean = False
        Public Def_DOCMAN_SAVE_PDF As Boolean = True
        Public Def_DOCMAN_SAVE_DOCX As Boolean = True
        Public Def_DOCMAN_TableName As String = ""
        Public Def_DOCMAN_Type As String = ""
        Public Def_DOCMAN_Types_ID As Long = 0
        Public Def_DOCMAN_ParentRecord_ID As Long = 0
        Public Def_DOCMAN_RefNum As String = ""
        Public Def_DOCMAN_Descr As String = ""

        Public FPf As FP_Form
        Public REPORT_Identifier As String
        Public Prepared_Template_Exists As Boolean
        Public CurrDoc As MSWORD.Document
        Public WordApp As MSWORD.Application = Nothing
        Public DOC_OTHER_PARAMS As String = ""
        Public Head_ID As Long = 0
        Public Doc_Blocks_Temp As MSWORD.Document = Nothing

        Public Doc_Blocks_Doc_FileName As String

        Private DIC_BLOCKS As New Dictionary(Of String, Struct_Block_Data)
        Private Disposed As Boolean = False

        Sub New(MyFPf As FP_Form, MyCurrDoc As MSWORD.Document, MyWordApp As MSWORD.Application, MyDOC_OTHER_PARAMS As String, MyHead_ID As Long, MyReport_Identifier As String, MyPrepared_Template_Exists As Boolean)
            FPf = MyFPf
            CurrDoc = MyCurrDoc
            WordApp = MyWordApp
            DOC_OTHER_PARAMS = MyDOC_OTHER_PARAMS
            Head_ID = MyHead_ID
            REPORT_Identifier = MyReport_Identifier
            Prepared_Template_Exists = MyPrepared_Template_Exists
        End Sub

        Public Sub Dispose()
            If Not Disposed Then
                Doc_Blocks_Temp_CLOSE()
                CurrDoc = Nothing
                DIC_BLOCKS = Nothing

                Disposed = True
            End If
        End Sub

        Private Sub Doc_Blocks_Temp_CLOSE()
            If Not (Doc_Blocks_Temp Is Nothing) Then
                DIC_BLOCKS.Clear()
                Try
                    Doc_Blocks_Temp.Close(0) 'Close and NOT save document

                Catch ex As Exception
                    'Nothing to do
                End Try
                Doc_Blocks_Temp = Nothing
            End If
        End Sub

        Private Function Blocks_FileName_GET(REPORT_Identifier As String) As String
            Return FPf.FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.PREPARED_TEMPLATES) + REPORT_Identifier + "_Blocks.docx"
        End Function

        Private Function Doc_Blocks_Temp_OPEN(CurrDoc As MSWORD.Document) As Boolean
            Dim OUT As Boolean = False

            If Not (Doc_Blocks_Temp Is Nothing) Then
                OUT = True
            Else
                Try
                    If Prepared_Template_Exists Then
                        Doc_Blocks_Temp = WordApp.Documents.Open(Blocks_FileName_GET(REPORT_Identifier))
                    Else
                        Doc_Blocks_Temp = WordApp.Documents.Add
                    End If
                    CurrDoc.Activate()
                    OUT = True

                Catch ex As Exception
                    FPf.FPApp.DoErrorMsgBox("FP_Word.Doc_Blocks_Temp_OPEN", Err.Number, Err.Description)
                End Try
            End If

            Return OUT
        End Function

        Public Function BLOCK_LOAD(Block_Name As String, Optional WithDialog As Boolean = True) As Boolean
            Dim OUT As Boolean = False

            If DIC_BLOCKS.ContainsKey(Block_Name) Then
                OUT = True
            Else
                If Prepared_Template_Exists Then
                    Dim MySQL As String = String.Format("SELECT Range_Start, Range_End, Skip_Block FROM WORD_Docs_Block_Ranges WHERE REPORT_Identifier = '{0}' And BlockName = '{1}'", REPORT_Identifier, Block_Name)
                    Dim DRow As DataRow = FPf.FPApp.DC.LocalDB_SEL.get_DataRow(MySQL)

                    If Not (DRow Is Nothing) Then
                        Dim New_Block As New Struct_Block_Data

                        With New_Block
                            .Block_Name = Block_Name

                            .Range = Doc_Blocks_Temp.Content
                            With .Range
                                .End = DRow!Range_End
                                .Start = DRow!Range_Start
                            End With

                            .Skip_Block = DRow!Skip_Block
                        End With

                        DIC_BLOCKS.Add(Block_Name, New_Block)

                        OUT = True
                    End If
                Else
                    Block_Name = Trim(Block_Name)
                    If Trim(Block_Name) = "" Then
                        FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.BLOCK_LOAD", 0, "Command 'BLOCK_LOAD' needs the name of the block in column WORD_REPORT_FIELDS.Field_Name ('{0}')")
                    ElseIf DIC_BLOCKS.ContainsKey(Block_Name) Then
                        FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.BLOCK_LOAD", 0, String.Format("Block already exists ('{0}')", Block_Name))
                    Else
                        Try
                            Doc_Blocks_Temp_OPEN(CurrDoc)

                            Dim Block_Range As MSWORD.Range = CurrDoc.Content
                            Dim Block_Range_Content As MSWORD.Range = CurrDoc.Content
                            Dim Start_Found As Boolean = False
                            Dim End_Found As Boolean = False
                            Dim i As Integer = 1

                            While i <= CurrDoc.Paragraphs.Count
                                Dim CurrRange As MSWORD.Range = CurrDoc.Paragraphs(i).Range

                                If Start_Found = False Then
                                    If CurrRange.Text = Block_Name + Chr(13) Then
                                        Block_Range.Start = CurrRange.Start
                                        If i + 1 <= CurrDoc.Paragraphs.Count Then
                                            Block_Range_Content.Start = CurrDoc.Paragraphs(i + 1).Range.Start
                                        End If
                                        Start_Found = True
                                    End If
                                Else
                                    If CurrRange.Text = "#_END_#" + Chr(13) Then
                                        Block_Range.End = CurrRange.End
                                        Block_Range_Content.End = CurrDoc.Paragraphs(i - 1).Range.End
                                        End_Found = True
                                        Exit While
                                    End If
                                End If
                                i += 1
                            End While

                            Dim New_Block As New Struct_Block_Data

                            New_Block.Block_Name = Block_Name

                            If Start_Found = False Then
                                New_Block.Skip_Block = True
                                DIC_BLOCKS.Add(Block_Name, New_Block)
                            ElseIf End_Found = False Then
                                FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.BLOCK_LOAD", 0, String.Format("End of block not found ('{0}'). Use the '#_END_#' keyword in separated paragraph of the document.", Block_Name))
                                New_Block.Skip_Block = True
                                DIC_BLOCKS.Add(Block_Name, New_Block)
                            Else
                                Try
                                    Block_Range_Content.Copy()

                                    New_Block.Range = (Doc_Blocks_Temp.Paragraphs.Add).Range

                                    New_Block.Range.Paste()

                                    Doc_Blocks_Temp.Paragraphs.Add()

                                Catch ex As Exception
                                    'Nothing to do
                                End Try

                                DIC_BLOCKS.Add(Block_Name, New_Block) 'Azert itt es nem a Try-Catch-en belul, mert az ures block hibat eredmenyez (a try-catch ezert lett beteve 'Nothing to do-val)
                                '                                      Az ures block letezik, ezert hozzaadjuk a DIC_BLOCKS-hoz.

                                If FPf.FPApp.DC.P_USE_LocalDB Then
                                    If REPORT_Identifier > "" Then
                                        Dim Range_Start As Integer = -1
                                        Dim Range_End As Integer = -1

                                        If Not (New_Block.Range Is Nothing) Then
                                            With New_Block.Range
                                                Range_Start = .Start
                                                Range_End = .End
                                            End With
                                        End If
                                        Dim MySQL As String = String.Format("INSERT INTO WORD_Docs_Block_Ranges (REPORT_Identifier, BlockName, Range_Start, Range_End, Skip_Block) VALUES ('{0}', '{1}', {2}, {3}, {4})", REPORT_Identifier, Block_Name, Range_Start, Range_End, Val(New_Block.Skip_Block))

                                        FPf.FPApp.DC.LocalDB_SEL.RunSQL(MySQL)
                                    End If
                                End If

                                Block_Range.Delete()

                                OUT = True
                            End If

                        Catch ex As Exception
                            'Nothing to do
                        End Try
                    End If
                End If
            End If

            Return OUT
        End Function

        Public Function Block_INSERT(Block_Name As String) As Struct_Block_Data
            Dim OUT As New Struct_Block_Data

            OUT.Skip_Block = True

            If Trim(Block_Name) = "" Then
                FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_INSERT", 0, "Command 'BLOCK_INSERT' needs the name of the block in column WORD_REPORT_FIELDS.Field_Name ('{0}')")
            Else
                If DIC_BLOCKS.ContainsKey(Block_Name) Then
                    OUT = DIC_BLOCKS(Block_Name)
                    If OUT.Skip_Block = False Then
                        Try
                            If OUT.Range.Start < OUT.Range.End Then
                                OUT.Range.Copy()

                                Dim NewParagraph As MSWORD.Paragraph = CurrDoc.Paragraphs.Add

                                NewParagraph.Range.Paste()

                            End If

                        Catch ex As Exception
                            'Nothing to do
                        End Try
                    End If
                End If
            End If

            Return OUT
        End Function

        Private Sub DIC_Blocks_FILL_FROM_LocalDB()
            Dim DT As DataTable = Nothing
            Dim MySQL As String = String.Format("SELECT BlockName, Range_Start, Range_End, Skip_Block FROM WORD_Docs_Block_Ranges WHERE REPORT_Identifier = '{0}'", REPORT_Identifier)

            If FPf.FPApp.DC.LocalDB_SEL.Fill_DT(MySQL, DT) Then
                For Each Drow As DataRow In DT.Rows
                    Dim Block_Data As New Struct_Block_Data

                    With Block_Data
                        .Block_Name = Drow!BlockName
                        If Drow!Range_Start <> -1 Then
                            .Range = Doc_Blocks_Temp.Range(Drow!Range_Start, Drow!Range_End)
                        End If
                        .Skip_Block = Drow!Skip_Block
                    End With
                    DIC_BLOCKS.Add(Drow!BlockName, Block_Data)
                Next
            End If
        End Sub

        Public Function Block_Defs_LOAD(From_ZD As Boolean) As Boolean
            Dim OUT As Boolean = True
            Dim Params_Tbl As New DataTable
            Dim MySQL As String = ""

            Dim Param_DOCMAN_SAVE_YN_Exists As Boolean = False
            Def_ParentTable = ""
            Def_Doc_Type = ""
            Def_Doc_Language = ""
            Def_Doc_FileName_Format = ""
            Def_DOCMAN_SAVE = False
            Def_DOCMAN_SAVE_PDF = True
            Def_DOCMAN_SAVE_DOCX = True
            Def_DOCMAN_Type = ""
            Def_DOCMAN_Types_ID = 0
            Def_DOCMAN_TableName = ""
            Def_DOCMAN_ParentRecord_ID = 0
            Def_DOCMAN_RefNum = ""
            Def_DOCMAN_Descr = ""

            Doc_Blocks_Doc_FileName = ""

            If Prepared_Template_Exists Then
                Doc_Blocks_Temp_OPEN(CurrDoc)

                MySQL = String.Format("SELECT ParentTable, Doc_Type, Doc_Language, Doc_FileName_Format, Doc_NoShowInternal, DOCMAN_SAVE, DOCMAN_SAVE_PDF, DOCMAN_SAVE_DOCX, DOCMAN_Type, DOCMAN_RefNum, DOCMAN_Descr FROM WORD_Docs_Block_Defs WHERE REPORT_Identifier = '{0}'", REPORT_Identifier)

                Dim DRow As DataRow = FPf.FPApp.DC.LocalDB_SEL.get_DataRow(MySQL)

                If DRow Is Nothing Then
                    OUT = False
                Else
                    Def_ParentTable = DRow!ParentTable
                    Def_Doc_Type = DRow!Doc_Type
                    Def_Doc_Language = DRow!Doc_Language
                    Def_Doc_FileName_Format = DRow!Doc_FileName_Format
                    Def_Doc_NoShowInternal = DRow!Doc_NoShowInternal

                    Def_DOCMAN_SAVE = DRow!DOCMAN_SAVE
                    Def_DOCMAN_SAVE_PDF = DRow!DOCMAN_SAVE_PDF
                    Def_DOCMAN_SAVE_DOCX = DRow!DOCMAN_SAVE_DOCX
                    Param_DOCMAN_SAVE_YN_Exists = True

                    Def_DOCMAN_Type = DRow!DOCMAN_Type
                    Def_DOCMAN_RefNum = DRow!DOCMAN_RefNum
                    Def_DOCMAN_Descr = DRow!DOCMAN_Descr

                    MySQL = String.Format("SELECT BLOCKNAME, FIELDNAME, PARAMS FROM WORD_Docs_Block_Defs_L WHERE REPORT_Identifier = '{0}'", REPORT_Identifier)
                    FPf.FPApp.DC.LocalDB_SEL.Fill_DT(MySQL, Params_Tbl)
                End If
            Else
                If CurrDoc.Tables.Count = 0 Then
                    OUT = False
                ElseIf CurrDoc.Tables(1).Rows.Count < 1 Then
                    OUT = False
                ElseIf CurrDoc.Tables(1).Columns.Count < 1 Then
                    OUT = False
                ElseIf MSWORD_CELL_REMOVE_SYS_CHARS(CurrDoc.Tables(1).Cell(1, 1).Range.Text) <> "#_DOC_TYPE_#" Then
                    OUT = False
                End If

                If OUT = True Then
                    OUT = Doc_Blocks_Temp_OPEN(CurrDoc)
                End If

                If OUT = True Then
                    Try
                        Dim Block_Range As MSWORD.Range = CurrDoc.Tables(1).Range

                        Block_Range.Copy()
                        Dim New_Block_Range As MSWORD.Range = (Doc_Blocks_Temp.Paragraphs.Add).Range
                        New_Block_Range.Paste()
                        CurrDoc.Tables(1).Delete()

                    Catch ex As Exception
                        OUT = False
                        FPf.FPApp.DoErrorMsgBox("Word_Blocks..Block_Defs_LOAD", 0, Err.Description)
                    End Try
                End If

                If OUT = True Then
                    With Params_Tbl
                        .Columns.Add("BLOCKNAME", System.Type.GetType("System.String"))
                        .Columns.Add("FIELDNAME", System.Type.GetType("System.String"))
                        .Columns.Add("PARAMS", System.Type.GetType("System.String"))
                    End With

                    If Doc_Blocks_Temp.Tables.Count < 1 Then
                        FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Table of definitions not found. Please insert into '#_DEFS_#' block a table with 3 columns.")
                    Else
                        Dim Doc_Def_Table = Doc_Blocks_Temp.Tables(1)

                        If Doc_Def_Table.Columns.Count < 3 Then
                            FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Table in Block '#_DEFS_#' must have 3 columns.")
                        Else
                            If FPf.FPApp.DC.P_USE_LocalDB Then
                                If REPORT_Identifier > "" Then
                                    With FPf.FPApp.DC.LocalDB_SEL
                                        .RunSQL(String.Format("DELETE WORD_Docs_Block_Defs WHERE REPORT_Identifier = '{0}'", REPORT_Identifier))
                                        .RunSQL(String.Format("DELETE WORD_Docs_Block_Defs_L WHERE REPORT_Identifier = '{0}'", REPORT_Identifier))
                                        .RunSQL(String.Format("DELETE WORD_Docs_Block_Ranges WHERE REPORT_Identifier = '{0}'", REPORT_Identifier))
                                        '+++.RunSQL(String.Format("INSERT INTO WORD_Docs_Block_Defs (REPORT_Identifier, Schema_Date, BlockName, FieldName, Params) VALUES ('{0}', {1})", REPORT_Identifier, xxx))
                                    End With
                                End If
                            End If

                            For i As Integer = 1 To Doc_Def_Table.Rows.Count
                                Dim C_BLOCKNAME As String = MSWORD_CELL_REMOVE_SYS_CHARS(Trim(nz(Doc_Def_Table.Cell(i, 1).Range.Text, "")))
                                Dim C_FIELDNAME As String = MSWORD_CELL_REMOVE_SYS_CHARS(Trim(nz(Doc_Def_Table.Cell(i, 2).Range.Text, "")))
                                Dim C_PARAMS As String = MSWORD_CELL_REMOVE_SYS_CHARS(nz(Doc_Def_Table.Cell(i, 3).Range.Text, ""))

                                If C_BLOCKNAME = "#_DOC_TYPE_#" Then
                                    Def_Doc_Type = C_FIELDNAME
                                ElseIf C_BLOCKNAME = "#_DOC_LANGUAGE_#" Then
                                    Def_Doc_Language = C_FIELDNAME
                                ElseIf C_BLOCKNAME = "#_FILENAME_#" Then
                                    Def_Doc_FileName_Format = C_FIELDNAME
                                ElseIf C_BLOCKNAME = "#_NO_SHOW_INTERNAL_#" Then
                                    Def_Doc_NoShowInternal = (C_FIELDNAME = "1")

                                ElseIf C_BLOCKNAME = "#_PARENT_TABLE_#" Then
                                    Def_ParentTable = C_FIELDNAME
                                ElseIf C_BLOCKNAME = "#_DOCMAN_SAVE_YN_#" Then
                                    Def_DOCMAN_SAVE = TEXT_Is_YES(C_FIELDNAME)
                                    Param_DOCMAN_SAVE_YN_Exists = True
                                ElseIf C_BLOCKNAME = "#_DOCMAN_SAVE_PDF_#" Then
                                    Def_DOCMAN_SAVE_PDF = TEXT_Is_YES(C_FIELDNAME)
                                ElseIf C_BLOCKNAME = "#_DOCMAN_SAVE_DOCX_#" Then
                                    Def_DOCMAN_SAVE_DOCX = TEXT_Is_YES(C_FIELDNAME)
                                ElseIf C_BLOCKNAME = "#_DOCMAN_TYPE_#" Then
                                    Def_DOCMAN_Type = C_FIELDNAME
                                ElseIf C_BLOCKNAME = "#_DOCMAN_REFNUM_FIELD_#" Then
                                    Def_DOCMAN_RefNum = C_FIELDNAME
                                ElseIf C_BLOCKNAME = "#_DOCMAN_DESCRIPTION_#" Then
                                    Def_DOCMAN_Descr = C_FIELDNAME
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
                    End If
                End If
            End If

            If OUT = True Then
                If Def_Doc_Type = "" Then
                    OUT = False
                    FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOC_TYPE_#' in document definition table not found.")
                ElseIf Def_Doc_Language = "" Then
                    OUT = False
                    FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOC_LANGUAGE_#' in document definition table not found.")
                ElseIf Def_Doc_FileName_Format = "" Then
                    OUT = False
                    FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOC_FILENAME_#' in document definition table not found.")
                ElseIf Def_ParentTable = "" Then
                    OUT = False
                    FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_PARENT_TABLE_#' in document definition table not found.")
                ElseIf Param_DOCMAN_SAVE_YN_Exists = False Then
                    OUT = False
                    FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOCMAN_SAVE_YN_#' in document definition table not found.")
                ElseIf Def_DOCMAN_Type = "" Then
                    OUT = False
                    FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOCMAN_TYPE_#' in document definition table not found.")
                ElseIf Def_DOCMAN_RefNum = "" Then
                    OUT = False
                    FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOCMAN_REFNUM_FIELD_#' in document definition table not found.")
                ElseIf Def_DOCMAN_Descr = "" Then
                    OUT = False
                    FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOCMAN_DESCRIPTION_#' in document definition table not found.")
                End If

                Dim Doc_Parent_FP As FP = Nothing

                If Not From_ZD Then
                    If OUT = True Then
                        Doc_Parent_FP = FPf.GET_FP_BY_ALIAS(Def_ParentTable)

                        If Doc_Parent_FP Is Nothing Then
                            OUT = False
                            FPf.FPApp.DoErrorMsgBox("FP_Word.Word_Blocks.Block_Defs_LOAD", 0, String.Format("Unknown FP Alias ({0})", Def_ParentTable))
                        End If
                    End If

                    If OUT = True Then
                        If Head_ID = 0 Then
                            Head_ID = Doc_Parent_FP.P_DATA_Current_ID
                        End If
                        If Head_ID = 0 Then
                            OUT = False
                            MsgBox("+++ Alljon ra a rekordra, amit ki szeretne nyomtatni.")
                        End If
                    End If

                    If OUT = True Then
                        If Def_DOCMAN_SAVE = True Then
                            Def_DOCMAN_Types_ID = 0
                            Def_DOCMAN_TableName = ""
                            Def_DOCMAN_ParentRecord_ID = 0

                            If Doc_Parent_FP.FP_DOCMAN Is Nothing Then
                                OUT = False
                                FPf.FPApp.DoErrorMsgBox("FP_Word.Block_Defs_LOAD", 0, String.Format("Define FP_DOCMAN for FP '{0}'" + vbCrLf + "(Command: {0}.FP_DOCMAN = <Your DOCMAN_Doc_Panel>)", Doc_Parent_FP.ServerObject_Prefix))
                            End If
                            If OUT = True Then
                                With Doc_Parent_FP.FP_DOCMAN
                                    Def_DOCMAN_TableName = .P_Parent_TableName
                                    Def_DOCMAN_ParentRecord_ID = .FP_Parent.P_DATA_Current_ID
                                    OUT = .Doc_Types_GET_FROM_STR(Def_DOCMAN_Type, Def_DOCMAN_Types_ID)
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

                    Dim ProcName As String = "WORD_REPORT_" + Def_Doc_Type + "_PREPARE"

                    If Strings.Left(Def_Doc_Type, 2) = "U_" Then
                        ProcName = Def_Doc_Type
                    End If

                    With FPf.FPApp.DC
                        .Qdf_set_SP(sqlComm, ProcName)
                        .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                        .Qdf_AddParameter(sqlComm, "@Report_Name", SqlDbType.NVarChar, , 128, REPORT_Identifier)
                        .Qdf_AddParameter(sqlComm, "@Doc_Language", SqlDbType.NVarChar, , 3, Def_Doc_Language)
                        .Qdf_AddParameter(sqlComm, "@Head_ID", SqlDbType.Int, , , , , Head_ID)
                        .Qdf_AddParameter(sqlComm, "@DOC_PARAMS", SqlDbType.NVarChar, , -1, Params_STR)
                        .Qdf_AddParameter(sqlComm, "@DOC_OTHER_PARAMS", SqlDbType.NVarChar, , -1, DOC_OTHER_PARAMS)
                        .Qdf_AddParameter(sqlComm, "@DOC_FILENAME_FORMAT", SqlDbType.NVarChar, , -1, Def_Doc_FileName_Format)

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
                        Doc_Blocks_Doc_FileName = gl_FPApp.Text_Remove_IllegalCharacters_From_FileName(Trim(nz(sqlComm.Parameters("@DOC_FILENAME").Value, "")))

                        If Doc_Blocks_Doc_FileName = "" Then
                            OUT = False
                            FPf.FPApp.DoErrorMsgBox("FP_Word..Word_Blocks.Block_Defs_LOAD", 0, "Param '#_DOC_FILENAME_#' invalid or procedure '{0}' returns with empty value.", ProcName)
                        End If
                    End If

                    CURSOR_SHOW_DEFAULT()
                End If
            End If

            Return OUT
        End Function
    End Class
End Class

