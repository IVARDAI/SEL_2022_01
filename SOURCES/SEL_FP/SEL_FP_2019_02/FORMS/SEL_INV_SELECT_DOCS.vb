Imports System.Data.SqlClient
Imports System.IO
Public Class SEL_INV_SELECT_DOCS

    Public WithEvents FPf As FP_Form
    Private _INV_ID As Integer
    Public WithEvents FP_INV_SELECT_DOCS As FP
    Dim DT_ALL As DataTable
    ReadOnly PFD_KEY As String = "ECOMM_DOC_TYPES"
    Public WithEvents FPP_BTN_SELECT_ALL As FP_PictureBox
    Public WithEvents FPP_BTN_UNSELECT_ALL As FP_PictureBox
    Public WithEvents FPP_BTN_SELECT_ALL_IN As FP_PictureBox
    Public WithEvents FPP_BTN_SELECT_ALL_EX As FP_PictureBox
    Public WithEvents FPP_BTN_OK As FP_PictureBox
    Public WithEvents FPP_BTN_CANCEL As FP_PictureBox

    Private AutoRefresh As Boolean = False
    Private Out_File_Paths() As String = Nothing
    Dim Arr_L_IN() As ListViewItem = Nothing
    Dim Arr_L_EX() As ListViewItem = Nothing
    Dim I_IN As Integer = 0
    Dim I_EX As Integer = 0

    Public Sub New(INV_ID As Integer)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _INV_ID = INV_ID
    End Sub
    Public ReadOnly Property FilePaths() As String()
        Get
            Return Out_File_Paths
        End Get
    End Property
    Private Function Prepare_Data() As Boolean
        Dim SqlComm As New SqlClient.SqlCommand
        Dim OUT As Boolean = True

        gl_FPApp.DC.Qdf_set_SP(SqlComm, "EComm_INV_Prepare_Docman_Docs")
        gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@INV_ID", SqlDbType.Int, , , , , _INV_ID)
        gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)

        If Not gl_FPApp.DC.Qdf_Execute(FPf, SqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE) Then
            Call gl_FPApp.DoErrorMsgBox("SEL_INV_SELECT_DOCS.Prepare_Data", Err.Number, Err.Description)
            OUT = False
        End If

        Return OUT
    End Function

    Private Sub SEL_INV_SELECT_DOCS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AutoRefresh = False
        If Prepare_Data() Then
            Dim fp_Controls As New Struct_FP_FORM_CONTROLS_COLLECTION
            Dim fp_INV_SELECT_DOCS_Controls As New Struct_FP_CONTROLS_COLLECTION
            FPf = New FP_Form("INV_SELECT_DOCSfrm", gl_FPApp, Me, True)
            With fp_Controls
                .Btn_HELP = BTN_HLP
            End With
            FPf.INIT_CONTROLS(fp_Controls)
            FP_INV_SELECT_DOCS = New FP(FPf, "INV_SELECT_DOCS", "", True)
            With FP_INV_SELECT_DOCS.SQL_BIND_Params
                .NameOf_READ = ""
                .NameOf_DEL = ""
                .NameOf_SAVE = ""
                .NameOf_WhereQuery = ""
            End With
            FP_INV_SELECT_DOCS.INIT_CONTROLS(fp_INV_SELECT_DOCS_Controls)
        End If
        Load_Doc_Types_Chk()
        AutoRefresh = True
        Load_Listviews()
    End Sub
    Private Sub Load_Doc_Types_Chk()
        Dim Str_Selected_Doc_Types As String = ""
        Dim Type_Arr() As String
        Dim S_OK As Boolean
        gl_FPApp.PFDlesen(PFD_KEY, Str_Selected_Doc_Types)
        If Not Str_Selected_Doc_Types = String.Empty Then
            Type_Arr = Str_Selected_Doc_Types.Split("|")
            For Each S In Type_Arr
                Dim TypeID As String = ""
                Dim Checked As Boolean
                S_OK = True
                If S.Contains(";") Then
                    Dim T() As String = S.Split(";")
                    If T.Length = 2 Then
                        If IsNumeric(T(0)) Then
                            TypeID = T(0)
                        Else
                            S_OK = False
                            TypeID = ""
                        End If
                        Select Case T(1)
                            Case "0"
                                Checked = False
                            Case "1"
                                Checked = True
                            Case Else
                                Checked = True
                        End Select
                    Else
                        S_OK = False
                    End If
                Else
                    S_OK = False
                End If
                If S_OK Then
                    For Each LI As ListViewItem In ListView_Doc_Types.Items
                        Debug.Print(LI.Text)
                        If LI.SubItems(1).Text.ToUpper = TypeID.ToUpper Then
                            LI.Checked = Checked
                        End If
                    Next
                End If
            Next
        End If
    End Sub
    Private Sub Save_Doc_Types_Chk()
        Dim L_ID As String
        Dim L_Chk As String
        Dim P As String = ""
        Dim Str_Selected_Doc_Types As String = ""
        For Each LI As ListViewItem In ListView_Doc_Types.Items
            L_ID = LI.SubItems(1).Text
            If LI.Checked Then
                L_Chk = "1"
            Else
                L_Chk = "0"
            End If
            Str_Selected_Doc_Types += P + L_ID + ";" + L_Chk
            P = "|"
        Next
        gl_FPApp.PFDinsertOrUpdate(PFD_KEY, Str_Selected_Doc_Types)
    End Sub
    Private Function Doc_Type_Checked(Doc_Types_ID As Integer) As Boolean
        Dim OUT As Boolean = False
        Dim L_ID As String

        For Each LI As ListViewItem In ListView_Doc_Types.Items
            L_ID = LI.SubItems(1).Text
            If L_ID = Doc_Types_ID.ToString Then
                OUT = LI.Checked
            End If
        Next
        Return OUT
    End Function

    Private Sub Refresh_Listviews_By_Checked_DocTypes()
        If AutoRefresh Then
            If Not DT_ALL Is Nothing Then
                Me.ListView_Included.Items.Clear()
                Me.ListView_Excluded.Items.Clear()
                Arr_L_IN = Nothing
                Arr_L_EX = Nothing
                I_IN = 0
                I_EX = 0
                Dim DRow As DataRow
                Dim Doc_Types_ID As Integer
                For Each DRow In DT_ALL.Rows
                    Doc_Types_ID = DRow.Item("Doc_Types_ID")
                    If Doc_Type_Checked(Doc_Types_ID) Then
                        ReDim Preserve Arr_L_IN(I_IN)
                        Arr_L_IN(I_IN) = New ListViewItem
                        Arr_L_IN(I_IN).SubItems.Add(DRow.Item("PosNr"))
                        If Not IsDBNull(DRow.Item("Doc_Descr")) Then
                            Arr_L_IN(I_IN).SubItems.Add(DRow.Item("Doc_Descr"))
                        Else
                            Arr_L_IN(I_IN).SubItems.Add(DRow.Item("Doc_Type_Descr"))
                        End If
                        Arr_L_IN(I_IN).SubItems.Add(DRow.Item("Doc_Type_Descr"))
                        Arr_L_IN(I_IN).SubItems.Add(DRow.Item("RecordID"))
                        Arr_L_IN(I_IN).SubItems.Add(DRow.Item("Doc_Types_ID"))
                        Arr_L_IN(I_IN).Checked = True
                        I_IN += 1
                    Else
                        ReDim Preserve Arr_L_EX(I_EX)
                        Arr_L_EX(I_EX) = New ListViewItem
                        Arr_L_EX(I_EX).SubItems.Add(DRow.Item("PosNr"))
                        If Not IsDBNull(DRow.Item("Doc_Descr")) Then
                            Arr_L_EX(I_EX).SubItems.Add(DRow.Item("Doc_Descr"))
                        Else
                            Arr_L_EX(I_EX).SubItems.Add(DRow.Item("Doc_Type_Descr"))
                        End If
                        Arr_L_EX(I_EX).SubItems.Add(DRow.Item("Doc_Type_Descr"))
                        Arr_L_EX(I_EX).SubItems.Add(DRow.Item("RecordID"))
                            Arr_L_EX(I_EX).SubItems.Add(DRow.Item("Doc_Types_ID"))
                            Arr_L_EX(I_EX).Checked = False
                            I_EX += 1
                        End If
                Next
                AutoRefresh = False
                If Not Arr_L_IN Is Nothing Then
                    ListView_Included.Items.AddRange(Arr_L_IN)
                End If

                If Not Arr_L_EX Is Nothing Then
                    ListView_Excluded.Items.AddRange(Arr_L_EX)
                End If
                AutoRefresh = True
            End If
        End If
    End Sub
    Private Sub Refresh_Listviews_Simple()
        If AutoRefresh Then
            Arr_L_IN = Nothing
            Arr_L_EX = Nothing
            I_IN = 0
            I_EX = 0
            For Each LI As ListViewItem In ListView_Included.Items
                If LI.Checked Then
                    ReDim Preserve Arr_L_IN(I_IN)
                    Arr_L_IN(I_IN) = LI
                    I_IN += 1
                Else
                    ReDim Preserve Arr_L_EX(I_EX)
                    Arr_L_EX(I_EX) = LI
                    I_EX += 1
                End If
            Next

            For Each LI As ListViewItem In ListView_Excluded.Items
                If LI.Checked Then
                    ReDim Preserve Arr_L_IN(I_IN)
                    Arr_L_IN(I_IN) = LI
                    I_IN += 1
                Else
                    ReDim Preserve Arr_L_EX(I_EX)
                    Arr_L_EX(I_EX) = LI
                    I_EX += 1
                End If
            Next

            ListView_Included.Items.Clear()
            ListView_Excluded.Items.Clear()

            AutoRefresh = False
            If Not Arr_L_IN Is Nothing Then
                ListView_Included.Items.AddRange(Arr_L_IN)
            End If

            If Not Arr_L_EX Is Nothing Then
                ListView_Excluded.Items.AddRange(Arr_L_EX)
            End If
            AutoRefresh = True
        End If
    End Sub
    Private Sub Load_Listviews()
        Dim DT As DataTable = Nothing
        Dim SQL As String = String.Format("SELECT CAST(0 as BIT) Selected, * FROM EComm_INV_Prepared_Docs WHERE Terminal = '{0}'", Terminal)
        gl_FPApp.DC.Qdf_Fill_DT(SQL, DT_ALL)

        With Me.ListView_Included
            .Columns.Add("#", 50, HorizontalAlignment.Left)
            .Columns.Add("PosNr", 150, HorizontalAlignment.Left)
            .Columns.Add("Doc_Descr", 300, HorizontalAlignment.Left)
            .Columns.Add("Doc_Type_Descr", 150, HorizontalAlignment.Left)
            .Columns.Add("RecordID", 0, HorizontalAlignment.Left)
            .Columns.Add("Doc_Types_ID", 0, HorizontalAlignment.Left)
            .View = View.Details
            .LabelEdit = False
            .CheckBoxes = True
            .FullRowSelect = True
            .GridLines = True
            .Columns(0).Text = "#"
            gl_FPApp.Text_TextVonSEQ("SEQ,VBSEQ_LISTVIEW_INV_SELECT_DOCS,1", "PosNr", .Columns(1).Text)
            gl_FPApp.Text_TextVonSEQ("SEQ,VBSEQ_LISTVIEW_INV_SELECT_DOCS,2", "Doc_Descr", .Columns(2).Text)
            gl_FPApp.Text_TextVonSEQ("SEQ,VBSEQ_LISTVIEW_INV_SELECT_DOCS,3", "Doc_Type_Descr", .Columns(3).Text)
        End With
        With Me.ListView_Excluded
            .Columns.Add("#", 50, HorizontalAlignment.Left)
            .Columns.Add("PosNr", 150, HorizontalAlignment.Left)
            .Columns.Add("Doc_Descr", 300, HorizontalAlignment.Left)
            .Columns.Add("Doc_Type_Descr", 150, HorizontalAlignment.Left)
            .Columns.Add("RecordID", 0, HorizontalAlignment.Left)
            .Columns.Add("Doc_Types_ID", 0, HorizontalAlignment.Left)
            .View = View.Details
            .LabelEdit = False
            .CheckBoxes = True
            .FullRowSelect = True
            .GridLines = True
            .Columns(0).Text = "#"
            gl_FPApp.Text_TextVonSEQ("SEQ,VBSEQ_LISTVIEW_INV_SELECT_DOCS,1", "PosNr", .Columns(1).Text)
            gl_FPApp.Text_TextVonSEQ("SEQ,VBSEQ_LISTVIEW_INV_SELECT_DOCS,2", "Doc_Descr", .Columns(2).Text)
            gl_FPApp.Text_TextVonSEQ("SEQ,VBSEQ_LISTVIEW_INV_SELECT_DOCS,3", "Doc_Type_Descr", .Columns(3).Text)
        End With

        Refresh_Listviews_By_Checked_DocTypes()

    End Sub

    Private Sub ListView_Doc_Types_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ListView_Doc_Types.ItemChecked
        Refresh_Listviews_By_Checked_DocTypes()
    End Sub
    Private Sub SEL_INV_SELECT_DOCS_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Save_Doc_Types_Chk()
    End Sub
    Private Sub FP_INV_SELECT_DOCS_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_INV_SELECT_DOCS.CONTROLS_INITIALIZED
        With FP_INV_SELECT_DOCS
            FPP_BTN_SELECT_ALL = .PICTUREBOXES_GET("BTN_SELECT_ALL")
            FPP_BTN_UNSELECT_ALL = .PICTUREBOXES_GET("BTN_UNSELECT_ALL")
            FPP_BTN_SELECT_ALL_IN = .PICTUREBOXES_GET("BTN_SELECT_ALL_IN")
            FPP_BTN_SELECT_ALL_EX = .PICTUREBOXES_GET("BTN_SELECT_ALL_EX")
            FPP_BTN_OK = .PICTUREBOXES_GET("BTN_OK")
            FPP_BTN_CANCEL = .PICTUREBOXES_GET("BTN_CANCEL")
        End With
    End Sub

    Enum Group_Select_Type As Integer
        NONE = 0
        ALL = 1
        CLEAR = 2
    End Enum
    Private Sub Select_Simple_Group(T As Group_Select_Type)
        AutoRefresh = False
        Dim B As Boolean
        Select Case T
            Case Group_Select_Type.ALL
                B = True
            Case Group_Select_Type.CLEAR
                B = False
            Case Else
                B = True
        End Select
        For Each LI As ListViewItem In ListView_Included.Items
            LI.Checked = B
        Next
        For Each LI As ListViewItem In ListView_Excluded.Items
            LI.Checked = B
        Next
        AutoRefresh = True
        Refresh_Listviews_Simple()
    End Sub
    Private Sub Select_By_DocTypes_Group(T As Group_Select_Type)
        AutoRefresh = False
        Dim B As Boolean
        Select Case T
            Case Group_Select_Type.ALL
                B = True
            Case Group_Select_Type.CLEAR
                B = False
            Case Else
                B = True
        End Select
        For Each LI As ListViewItem In ListView_Doc_Types.Items
            LI.Checked = B
        Next
        AutoRefresh = True
        Refresh_Listviews_By_Checked_DocTypes()
    End Sub
    Private Function Attachements_LOAD_DOC(Doc_Pages_ID As Long, ByRef OUT_Bytes() As Byte) As Boolean
        Dim OUT As Boolean = False
        Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

        ReDim OUT_Bytes(0)

        With gl_FPApp.DC
            .Qdf_set_SP(sqlComm, "FP_DOCMAN_Page_READ")
            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            .Qdf_AddParameter(sqlComm, "@ID", SqlDbType.Int, , , , , Doc_Pages_ID)
            .Qdf_AddParameter(sqlComm, "@OldTransactID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@Doc_Images_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@PageNum", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@Origin", SqlDbType.NVarChar, ParameterDirection.Output, 255)
            .Qdf_AddParameter(sqlComm, "@ImageData", SqlDbType.VarBinary, ParameterDirection.Output, -1)

            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
        End With

        CURSOR_SHOW_WAIT()
        Try
            OUT = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
        Catch ex As Exception
            OUT = False
            FPf.FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        If OUT Then
            OUT_Bytes = sqlComm.Parameters("@ImageData").Value
        End If

        Return OUT
    End Function

    Private Function Get_File_Paths() As Boolean
        Dim OUT As Boolean = True
        Dim OUT_ToFolder As String = gl_FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP) + "\DOCMAN_TEMP"
        Dim L_IDS As New List(Of Integer)
        Dim L_Origins As New List(Of String)
        Dim Bytes() As Byte = Nothing
        Dim FileNames() As String = Nothing
        Dim I As Integer = 0
        Try
            If Directory.Exists(OUT_ToFolder) Then
                Try
                    Directory.Delete(OUT_ToFolder, True)
                Catch ex As Exception
                    'Nothing to do
                End Try
            End If
            If Not Directory.Exists(OUT_ToFolder) Then
                Directory.CreateDirectory(OUT_ToFolder)
            End If
        Catch ex As Exception
            OUT = False
            gl_FPApp.DoErrorMsgBox("DOCMAN_Doc_Panel.Attachements_ItemDrag", Err.Number, Err.Description)
        End Try

        If OUT Then
            For Each LI As ListViewItem In ListView_Included.Items
                Dim Doc_Pages_ID As String
                Dim Origin As String
                Dim _SQL As String
                Doc_Pages_ID = LI.SubItems(4).Text
                If IsNumeric(Doc_Pages_ID) Then
                    _SQL = String.Format("SELECT ID, Origin FROM Doc_Pages WHERE ID = {0}", Doc_Pages_ID)
                    Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(_SQL)
                    If Not DRow Is Nothing Then
                        Doc_Pages_ID = DRow.Item("ID")
                        Origin = DRow.Item("Origin")
                        If L_IDS.Contains(Doc_Pages_ID) Then
                            'nothing to do
                        Else
                            L_IDS.Add(Doc_Pages_ID)
                            While L_Origins.Contains(Origin.ToUpper)
                                Origin = "1" + Origin
                            End While
                            L_Origins.Add(Origin)
                            If Attachements_LOAD_DOC(Doc_Pages_ID, Bytes) Then
                                Dim FileName_With_Path As String = OUT_ToFolder + "\" + Origin
                                gl_FPApp.ByteArray_SaveFile(FileName_With_Path, Bytes)
                                ReDim Preserve FileNames(I)
                                FileNames(I) = FileName_With_Path
                                I += 1
                            End If
                        End If
                    End If
                End If
            Next
        End If
        Out_File_Paths = FileNames
        Return OUT
    End Function
    Private Sub FPP_BTN_SELECT_ALL_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPP_BTN_SELECT_ALL.CLICK
        Select_By_DocTypes_Group(Group_Select_Type.ALL)
    End Sub
    Private Sub FPP_BTN_UNSELECT_ALL_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPP_BTN_UNSELECT_ALL.CLICK
        Select_By_DocTypes_Group(Group_Select_Type.CLEAR)
    End Sub
    Private Sub FPP_BTN_SELECT_ALL_IN_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPP_BTN_SELECT_ALL_IN.CLICK
        Select_Simple_Group(Group_Select_Type.ALL)
    End Sub
    Private Sub FPP_BTN_SELECT_ALL_EX_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPP_BTN_SELECT_ALL_EX.CLICK
        Select_Simple_Group(Group_Select_Type.CLEAR)
    End Sub
    Private Sub ListView_Included_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ListView_Included.ItemChecked
        Refresh_Listviews_Simple()
    End Sub
    Private Sub ListView_Excluded_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ListView_Excluded.ItemChecked
        Refresh_Listviews_Simple()
    End Sub
    Private Sub FPP_BTN_OK_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPP_BTN_OK.CLICK
        If Get_File_Paths() Then
            Me.DialogResult = DialogResult.OK
        Else
            Me.DialogResult = DialogResult.Abort
        End If
        Me.Close()
    End Sub
    Private Sub FPP_BTN_CANCEL_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPP_BTN_CANCEL.CLICK
        DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class