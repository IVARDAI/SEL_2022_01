Imports System.Data
Imports System.Data.SqlClient

Public Class FP
    Public Event CONTROLS_INITIALIZING(ByVal sender_FP As FP)
    Public Event CONTROLS_INITIALIZED(ByVal sender_FP As FP)
    Public Event CONTROLS_INITIALIZED_AND_ARRANGED(sender_FP As FP)
    Public Event Form_BeforeInsert(ByVal sender_FP As FP, ByRef Cancel As Integer, ByRef ID_of_Newrecord As Long)
    Public Event Data_RS_SET_Before(ByVal sender_FP As FP)
    Public Event Data_RS_SET_After(ByVal sender_FP As FP)
    Public Event Data_Records_Loading(ByVal sender_FP As FP)
    Public Event Data_Records_Loaded(ByVal sender_FP As FP)
    Public Event Form_Records_Loading(ByVal sender_FP As FP, ByVal SubWHERE As String, ByVal NoRecord_OK As Boolean, ByRef Result As Boolean, ByRef Handled As Boolean)
    Public Event Form_BeforeUpdate(ByVal sender_FP As FP, ByRef Cancel As Integer)
    Public Event Form_BeforeDelete(ByVal sender_FP As FP, ByRef Cancel As Integer)
    Public Event Form_AfterUpdate(ByVal sender_FP As FP)
    Public Event Form_AfterDelete(ByVal sender_FP As FP)
    Public Event Form_Current(ByVal sender_FP As FP)
    Public Event Form_Current_AfterChildren(ByVal sender_FP As FP)
    Public Event Form_NoRecord(ByVal sender_FP As FP)
    Public Event Form_Dirty(ByVal sender_FP As FP, ByRef Cancel As Integer)
    Public Event Form_BeginEdit(ByVal sender_FP As FP)
    Public Event Form_Field_Enter(ByVal FPc As FP_Control, ByRef Handled As Boolean)
    Public Event Form_Field_AfterUpdate(ByVal FPc As FP_Control)
    Public Event Form_Field_Coloring(ByVal sender_FPc As FP_Control, ByRef Handled As Boolean)
    Public Event Form_KeyPreview_KeyDown(ByVal sender_FP As FP, ByRef sender As Object, ByRef e As System.Windows.Forms.KeyEventArgs)
    Public Event Form_KeyPreview_KeyPress(ByVal sender_FP As FP, ByRef sender As Object, ByRef e As System.Windows.Forms.KeyPressEventArgs)
    Public Event Form_Print_Begin(ByVal sender_FP As FP, ByRef Cancel As Integer)
    Public Event Form_Print_Prepare(ByVal sender_FP As FP, ByVal Identifier As String, ByRef Prepared As Boolean, ByRef CancelOpenReport As Boolean)
    Public Event Form_Print_End(ByVal sender_FP As FP)
    Public Event Form_Record_Duplicated(ByVal sender_FP As FP)
    Public Event Form_Controls_Arrange_Begin(sender_FP As FP)
    Public Event Form_Controls_Arrange_End(sender_FP As FP)
    Public Event GRID_Paint(ByVal sender_FP As FP, ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
    Public Event GRID_Row_Drag_Begin(ByVal sender_FP As FP, ByRef DATA As String, ByVal e As System.Windows.Forms.MouseEventArgs, ByRef Cancel As Boolean)
    Public Event GRID_Row_DoubleClick(ByVal sender_FP As FP, ByRef Handled As Boolean)
    Public Event GRID_Row_MouseWheel(ByVal sender_FP As FP, ByVal sender As Object, ByRef e As System.Windows.Forms.MouseEventArgs, ByRef Handled As Boolean)
    Public Event GRID_Filter_Changed(ByVal sender_FP As FP)
    Public Event GRID_Filter_Deactivated(sender_FP As FP)
    Public Event GRID_Sorted(ByVal sender_FP As FP, ByVal e As System.EventArgs)
    Public Event GRID_CellClick(ByVal sender_FP As FP, ByVal FPc As FP_Control, ByRef Handled As Boolean)
    Public Event EXCEL_IMPORT_Data_Records_Prepared(ByVal sender_FP As FP, ByVal sender As FP_XLS_Import, ByRef Cancel As Boolean)
    Public Event EXCEL_IMPORT_Check_Data(ByVal sender_FP As FP, ByVal sender As FP_XLS_Import)
    Public Event EXCEL_IMPORT_Import_Data(ByVal sender_FP As FP, ByVal sender As FP_XLS_Import, ByRef Cancel As Boolean)

    Public WithEvents BINDINGNAVIGATOR_FORM As System.Windows.Forms.BindingNavigator
    Public WithEvents BINDINGNAVIGATOR_FORM_MoveFirstItem As System.Windows.Forms.ToolStripButton
    Public WithEvents BINDINGNAVIGATOR_FORM_MovePreviousItem As System.Windows.Forms.ToolStripButton
    Public WithEvents BINDINGNAVIGATOR_FORM_PositionItem As System.Windows.Forms.ToolStripTextBox
    Public WithEvents BINDINGNAVIGATOR_FORM_CountItem As System.Windows.Forms.ToolStripLabel
    Public WithEvents BINDINGNAVIGATOR_FORM_MoveNextItem As System.Windows.Forms.ToolStripButton
    Public WithEvents BINDINGNAVIGATOR_FORM_MoveLastItem As System.Windows.Forms.ToolStripButton
    Public WithEvents BINDINGNAVIGATOR_FORM_AddNewItem As System.Windows.Forms.ToolStripButton
    Public WithEvents BINDINGNAVIGATOR_FORM_DeleteItem As System.Windows.Forms.ToolStripButton
    Public WithEvents GRID As FP_GRIDVIEW
    Public WithEvents FilterText As Label
    Public WithEvents Button_Add_New As PictureBox
    Public WithEvents Button_Up As PictureBox
    Public WithEvents Button_Down As PictureBox
    Public WithEvents Button_Delete As PictureBox
    Public WithEvents ButtonFilter As PictureBox
    Public WithEvents ButtonFind As PictureBox
    Public WithEvents ButtonExportToExcel As PictureBox
    Public WithEvents ButtonImportFromExcel As PictureBox
    Public WithEvents ButtonPrint As PictureBox
    Public WithEvents Button_Duplicate_Record As PictureBox
    Public WithEvents ButtonFilterLine As PictureBox

    Public DATA_DT_FORM As New DataTable

    Public DOFILTER_ReturnedParams As New Struct_DoFilter_gl_Params

    Public CONTROLS As New Dictionary(Of String, FP_Control)
    Public PICTUREBOXES As New Dictionary(Of String, FP_PictureBox)

    Public ServerObject_Prefix As String = ""

    'Automatikus nyomtatashoz hasznalatos parameterek
    Public FP_ALIAS As String = "" 'A WORD dokumentumokban a #_PARENT_TABLE_# parameterben hivatkozott ertek (javasolt: "ORD" vagy "ORD_L")
    Public FP_DOCMAN As DOCMAN_Doc_Panel 'Az automatikus nyomtatas soran a dokumentum ebbe a dokumentum panelba mentodik el.

    Public SubPrefix As String = ""
    Private FieldPrefix As String = ""
    Public SQL_BIND_Params As New Struct_FP_SQL_BIND_PARAMS
    Public FORM_SubWHERE As String = ""
    Public FORM_SubWHERE_FIX As String = ""
    Private FORM_SubWHERE_for_NewRecords As String = ""
    Public UnboundForm As Boolean = False
    Public RS_ID As Long = 0
    Public RS_RecCount As Long = 0

    Public FPf As FP_Form
    Public Parent_FP As FP
    Public Parent_FP_JOINED_DATA_FIELD As String = ""

    Public WithEvents XLS_IMPORT As FP_XLS_Import

    Public MAXRECORDS As Long = 15000

    Private FP_ID As Integer
    Private Logs_Activity_Active As Boolean = False
    Private Logs_Activity_NewRecord As Boolean = False
    Private Logs_Activity_ID As Long = 0
    Private Logs_Activity_StartTime As DateTime = NULLDATE

    Private WithEvents FP_Active_Only_When_This_Field_Visible As Control
    Private FP_Active_Only_When_This_Field_Visible_Current_Parent_ID As Long = 0

    Sub New(ByRef MyFPf As FP_Form, ByVal MyServerObject_Prefix As String, Optional ByVal MySubPrefix As String = "", Optional ByVal This_Is_An_Unbound_Form As Boolean = False)
        FPf = MyFPf
        If FPf.ServerObject_Prefix = MyServerObject_Prefix Then
            FPf.FPApp.DoErrorMsgBox("FP.New", 0, String.Format("FP_FORM.ServerObject_Prefix = FP.ServerObject_Prefix ('{0}'). Ez nem jo igy, mert egy csomo utkozeshez fog vezetni!!!", MyServerObject_Prefix))
        End If
        ServerObject_Prefix = MyServerObject_Prefix

        SubPrefix = MySubPrefix
        UnboundForm = This_Is_An_Unbound_Form

        INIT_DEFAULT_PARAMS()
    End Sub
    Sub New(ByRef MyFPf As FP_Form, ByVal MyServerObject_Prefix As String, ByVal MySubPrefix As String, ByRef MyParent_FP As FP, ByVal MyParent_FP_JOINED_DATA_FIELD As String)
        FPf = MyFPf
        If FPf.ServerObject_Prefix = MyServerObject_Prefix Then
            FPf.FPApp.DoErrorMsgBox("FP.New", 0, String.Format("FP_FORM.ServerObject_Prefix = FP.ServerObject_Prefix ('{0}'). Ez nem jo igy, mert egy csomo utkozeshez fog vezetni!!!", MyServerObject_Prefix))
        End If
        ServerObject_Prefix = MyServerObject_Prefix
        SubPrefix = MySubPrefix

        SET_PARENT(MyParent_FP, MyParent_FP_JOINED_DATA_FIELD)

        INIT_DEFAULT_PARAMS()
    End Sub
    Sub New(MyFieldPrefix As String, ByRef MyFPf As FP_Form, ByVal MyServerObject_Prefix As String, ByVal MySubPrefix As String, ByRef MyParent_FP As FP, ByVal MyParent_FP_JOINED_DATA_FIELD As String)
        FieldPrefix = MyFieldPrefix
        FPf = MyFPf
        If FPf.ServerObject_Prefix = MyServerObject_Prefix Then
            FPf.FPApp.DoErrorMsgBox("FP.New", 0, String.Format("FP_FORM.ServerObject_Prefix = FP.ServerObject_Prefix ('{0}'). Ez nem jo igy, mert egy csomo utkozeshez fog vezetni!!!", MyServerObject_Prefix))
        End If
        ServerObject_Prefix = MyServerObject_Prefix
        SubPrefix = MySubPrefix

        SET_PARENT(MyParent_FP, MyParent_FP_JOINED_DATA_FIELD)

        INIT_DEFAULT_PARAMS()
    End Sub

    Public ReadOnly Property P_FP_ID As Integer
        Get
            Return FP_ID
        End Get
    End Property

    Public Property P_FP_Refresh_Only_When_This_Field_Visible As Control
        Get
            Return FP_Active_Only_When_This_Field_Visible
        End Get
        Set(value As Control)
            FP_Active_Only_When_This_Field_Visible = value
        End Set
    End Property

    Protected Friend ReadOnly Property P_FP_Refresh_Field_Visible As Boolean
        Get
            Dim OUT As Boolean = True
            Dim Refresh_c As Control = P_FP_Refresh_Only_When_This_Field_Visible

            If Not (Refresh_c Is Nothing) Then
                OUT = Refresh_c.Visible
            End If

            Return OUT
        End Get
    End Property

    Public Property P_DATA_Binded_ByUser() As Boolean
        Get
            P_DATA_Binded_ByUser = DATA_Binded_ByUser
        End Get
        Set(ByVal value As Boolean)
            DATA_Binded_ByUser = value
        End Set
    End Property
    Public ReadOnly Property Sub_IDS(My_FP As FP) As String
        Get
            Dim IDS As String = ""
            Dim SqlComm As New SqlClient.SqlCommand
            Dim Parent_Record_ID As Integer = My_FP.ROOT_FP.P_DATA_Current_ID
            Dim Parent_RS_FROM As String = My_FP.ROOT_FP.RS_FROM
            Dim Parent_RS_Where As String = My_FP.ROOT_FP.RS_WHERE
            gl_FPApp.DC.Qdf_set_SP(SqlComm, "ORD_Line_Sub_Filter_IDS")
            gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@FP_Alias", SqlDbType.NVarChar, , 50, My_FP.FP_ALIAS)
            gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Parent_RS_FROM", SqlDbType.NVarChar, , 100, Parent_RS_FROM)
            gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Parent_Record_ID", SqlDbType.Int, , , , , Parent_Record_ID)
            gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Parent_RS_Where", SqlDbType.NVarChar, , -1, Parent_RS_Where)
            gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@OUT", SqlDbType.NVarChar, ParameterDirection.Output, -1)

            If Not gl_FPApp.DC.Qdf_Execute(FPf, SqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE) Then
                Call gl_FPApp.DoErrorMsgBox("FP.SubFilter_IDS", Err.Number, Err.Description)
            Else
                IDS = SqlComm.Parameters("@OUT").Value
            End If
            Return IDS
        End Get
    End Property

    Private Sub BINDINGNAVIGATOR_EVENT_MOUSEENTER(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles BINDINGNAVIGATOR_FORM_MoveFirstItem.MouseEnter,
    BINDINGNAVIGATOR_FORM_MovePreviousItem.MouseEnter,
    BINDINGNAVIGATOR_FORM_PositionItem.MouseEnter,
    BINDINGNAVIGATOR_FORM_CountItem.MouseEnter,
    BINDINGNAVIGATOR_FORM_MoveNextItem.MouseEnter,
    BINDINGNAVIGATOR_FORM_MoveLastItem.MouseEnter,
    BINDINGNAVIGATOR_FORM_AddNewItem.MouseEnter,
    BINDINGNAVIGATOR_FORM_DeleteItem.MouseEnter
        FPf.HELP_SHOW(BINDINGNAVIGATOR_FORM, sender.name)
    End Sub
    Private Sub BINDINGNAVIGATOR_EVENT_MOUSELEAVE(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles BINDINGNAVIGATOR_FORM_MoveFirstItem.MouseLeave,
    BINDINGNAVIGATOR_FORM_MovePreviousItem.MouseLeave,
    BINDINGNAVIGATOR_FORM_PositionItem.MouseLeave,
    BINDINGNAVIGATOR_FORM_CountItem.MouseLeave,
    BINDINGNAVIGATOR_FORM_MoveNextItem.MouseLeave,
    BINDINGNAVIGATOR_FORM_MoveLastItem.MouseLeave,
    BINDINGNAVIGATOR_FORM_AddNewItem.MouseLeave,
    BINDINGNAVIGATOR_FORM_DeleteItem.MouseLeave
        FPf.HELP_HIDE()
    End Sub

    Public Function SET_PARENT(ByVal MyParent_FP As FP, ByVal MyJOINED_DATA_FIELD As String, Optional ByVal MyFORM_SubWHERE_FIX As String = "") As Boolean
        Dim OUT As Boolean = True

        If (MyParent_FP IsNot Nothing And Trim(MyJOINED_DATA_FIELD) = "") _
           Or (MyParent_FP Is Nothing And MyJOINED_DATA_FIELD <> "") Then
            FPf.FPApp.DoErrorMsgBox("FP.SET_PARENT", 0, "Invalid parameters")
        Else
            Parent_FP = MyParent_FP
            Parent_FP_JOINED_DATA_FIELD = MyJOINED_DATA_FIELD
            FORM_SubWHERE_FIX = MyFORM_SubWHERE_FIX
        End If

        SET_PARENT = OUT
    End Function
    Public Function FORM_IsSubForm() As Boolean
        Dim OUT As Boolean = False

        If Not (Parent_FP Is Nothing) Then
            If Trim(Parent_FP_JOINED_DATA_FIELD) = "" Then
                FPf.FPApp.DoErrorMsgBox("FP.FORM_IsSubForm", 0, "Nincs megadva az FP_Parent-hez tartozo Parent_FP_JOINED_DATA_FIELD tulajdonsag. Az FP igy nem kezelheto CHILD-kent.")
            Else
                OUT = True
            End If
        End If

        FORM_IsSubForm = OUT
    End Function
    Public Function ROOT_FP() As FP
        Dim OUT As FP = Me

        Do While Not (OUT.Parent_FP Is Nothing)
            OUT = OUT.Parent_FP
        Loop

        ROOT_FP = OUT
    End Function

    Public Function INIT_CONTROLS(ByVal Control_Collection As Struct_FP_CONTROLS_COLLECTION) As Boolean
        Dim OUT As Boolean = True

        INIT_SQL_BIND()

        FieldPrefix = nz(Control_Collection.FieldPrefix, "")

        CONTROLS_FROM_RS_SET()

        With Control_Collection
            If .GRID.GRID Is Nothing Then
                GRID = Nothing
            Else
                GRID = New FP_GRIDVIEW(Me, .GRID)
            End If

            BINDINGNAVIGATOR_FORM = .BindingNavigator
            If BINDINGNAVIGATOR_FORM IsNot Nothing Then
                With BINDINGNAVIGATOR_FORM
                    BINDINGNAVIGATOR_FORM_MoveFirstItem = .MoveFirstItem
                    BINDINGNAVIGATOR_FORM_MovePreviousItem = .MovePreviousItem
                    BINDINGNAVIGATOR_FORM_PositionItem = .PositionItem
                    BINDINGNAVIGATOR_FORM_CountItem = .CountItem
                    BINDINGNAVIGATOR_FORM_MoveNextItem = .MoveNextItem
                    BINDINGNAVIGATOR_FORM_MoveLastItem = .MoveLastItem
                    BINDINGNAVIGATOR_FORM_AddNewItem = .AddNewItem
                    BINDINGNAVIGATOR_FORM_DeleteItem = .DeleteItem

                    If Not (BINDINGNAVIGATOR_FORM_MoveFirstItem Is Nothing) Then
                        BINDINGNAVIGATOR_FORM_MoveFirstItem.ToolTipText = ""
                        BINDINGNAVIGATOR_FORM_MoveFirstItem.Text = ""
                    End If
                    If Not (BINDINGNAVIGATOR_FORM_MovePreviousItem Is Nothing) Then
                        BINDINGNAVIGATOR_FORM_MovePreviousItem.ToolTipText = ""
                        BINDINGNAVIGATOR_FORM_MovePreviousItem.Text = ""
                    End If
                    If Not (BINDINGNAVIGATOR_FORM_PositionItem Is Nothing) Then
                        BINDINGNAVIGATOR_FORM_PositionItem.ToolTipText = ""
                        BINDINGNAVIGATOR_FORM_PositionItem.Text = ""
                    End If
                    If Not (BINDINGNAVIGATOR_FORM_CountItem Is Nothing) Then
                        BINDINGNAVIGATOR_FORM_CountItem.ToolTipText = ""
                        BINDINGNAVIGATOR_FORM_CountItem.Text = "/{0}"
                    End If
                    If Not (BINDINGNAVIGATOR_FORM_MoveNextItem Is Nothing) Then
                        BINDINGNAVIGATOR_FORM_MoveNextItem.ToolTipText = ""
                        BINDINGNAVIGATOR_FORM_MoveNextItem.Text = ""
                    End If
                    If Not (BINDINGNAVIGATOR_FORM_MoveLastItem Is Nothing) Then
                        BINDINGNAVIGATOR_FORM_MoveLastItem.ToolTipText = ""
                        BINDINGNAVIGATOR_FORM_MoveLastItem.Text = ""
                    End If
                    If Not (BINDINGNAVIGATOR_FORM_AddNewItem Is Nothing) Then
                        BINDINGNAVIGATOR_FORM_AddNewItem.ToolTipText = ""
                        BINDINGNAVIGATOR_FORM_AddNewItem.Text = ""
                    End If
                    If Not (BINDINGNAVIGATOR_FORM_DeleteItem Is Nothing) Then
                        BINDINGNAVIGATOR_FORM_DeleteItem.ToolTipText = ""
                        BINDINGNAVIGATOR_FORM_DeleteItem.Text = ""
                    End If
                End With
            End If

            FilterText = .FilterText
            ButtonFilter = .Btn_Filter
            ButtonFilterLine = .Btn_Filter_Line
            ButtonFind = .Btn_Find
            ButtonExportToExcel = .Btn_ExportToExcel
            ButtonImportFromExcel = .Btn_ImportFromExcel
            ButtonPrint = .Btn_Print
            Button_Add_New = .Btn_Add_New
            Button_Delete = .Btn_Del
            Button_Up = .Btn_Up
            Button_Down = .Btn_Down
            Button_Duplicate_Record = .Btn_DuplicateRecord
        End With

        If Not (FPf.HELP_Frm Is Nothing) Then
            With FPf.HELP_Frm
                .ADD_HELP_DICTIONARY(ServerObject_Prefix, "")
                .ADD_HELP_DICTIONARY(ServerObject_Prefix, SubPrefix)
                If SubPrefix > "" Then
                    .ADD_HELP_DICTIONARY(ServerObject_Prefix, SubPrefix)
                End If
                If Not (ButtonFilter Is Nothing) Then
                    .ADD_HELP_STANDARD_ITEM("###ButtonFilter###", ButtonFilter.Name)
                End If
                If Not (ButtonFilterLine Is Nothing) Then
                    .ADD_HELP_STANDARD_ITEM("###ButtonFilterLine###", ButtonFilterLine.Name)
                End If
                If Not (ButtonFind Is Nothing) Then
                    .ADD_HELP_STANDARD_ITEM("###ButtonFind###", ButtonFind.Name)
                End If
                If Not (ButtonExportToExcel Is Nothing) Then
                    .ADD_HELP_STANDARD_ITEM("###ButtonExportToExcel###", ButtonExportToExcel.Name)
                End If
                If Not (ButtonImportFromExcel Is Nothing) Then
                    .ADD_HELP_STANDARD_ITEM("###ButtonImportFromExcel###", ButtonImportFromExcel.Name)
                End If
                If Not (ButtonPrint Is Nothing) Then
                    .ADD_HELP_STANDARD_ITEM("###ButtonPrint###", ButtonPrint.Name)
                End If
                If Not (Button_Add_New Is Nothing) Then
                    .ADD_HELP_STANDARD_ITEM("###Button_Add_New###", Button_Add_New.Name)
                End If
                If Not (Button_Delete Is Nothing) Then
                    .ADD_HELP_STANDARD_ITEM("###Button_Delete###", Button_Delete.Name)
                End If
                If Not (Button_Up Is Nothing) Then
                    .ADD_HELP_STANDARD_ITEM("###Button_Up###", Button_Up.Name)
                End If
                If Not (Button_Down Is Nothing) Then
                    .ADD_HELP_STANDARD_ITEM("###Button_Down###", Button_Down.Name)
                End If

                If Not (BINDINGNAVIGATOR_FORM Is Nothing) Then
                    If Not (BINDINGNAVIGATOR_FORM.MoveFirstItem Is Nothing) Then
                        .ADD_HELP_STANDARD_ITEM("###BINDINGNAV_MoveFirst###", BINDINGNAVIGATOR_FORM.MoveFirstItem.Name)
                    End If
                    If Not (BINDINGNAVIGATOR_FORM.MovePreviousItem Is Nothing) Then
                        .ADD_HELP_STANDARD_ITEM("###BINDINGNAV_MovePrevious###", BINDINGNAVIGATOR_FORM.MovePreviousItem.Name)
                    End If
                    If Not (BINDINGNAVIGATOR_FORM.PositionItem Is Nothing) Then
                        .ADD_HELP_STANDARD_ITEM("###BINDINGNAV_PositionItem###", BINDINGNAVIGATOR_FORM.PositionItem.Name)
                    End If
                    If Not (BINDINGNAVIGATOR_FORM.CountItem Is Nothing) Then
                        .ADD_HELP_STANDARD_ITEM("###BINDINGNAV_CountItem###", BINDINGNAVIGATOR_FORM.CountItem.Name)
                    End If
                    If Not (BINDINGNAVIGATOR_FORM.MoveFirstItem Is Nothing) Then
                        .ADD_HELP_STANDARD_ITEM("###BINDINGNAV_MoveNext###", BINDINGNAVIGATOR_FORM.MoveNextItem.Name)
                    End If
                    If Not (BINDINGNAVIGATOR_FORM.MovePreviousItem Is Nothing) Then
                        .ADD_HELP_STANDARD_ITEM("###BINDINGNAV_MoveLast###", BINDINGNAVIGATOR_FORM.MoveLastItem.Name)
                    End If
                    If Not (BINDINGNAVIGATOR_FORM.AddNewItem Is Nothing) Then
                        .ADD_HELP_STANDARD_ITEM("###BINDINGNAV_AddNew###", BINDINGNAVIGATOR_FORM.AddNewItem.Name)
                    End If
                    If Not (BINDINGNAVIGATOR_FORM.DeleteItem Is Nothing) Then
                        .ADD_HELP_STANDARD_ITEM("###BINDINGNAV_Delete###", BINDINGNAVIGATOR_FORM.DeleteItem.Name)
                    End If
                End If
            End With
        End If

        FPf.SET_LABELTEXTS_FROM_SEQ(ServerObject_Prefix, "", FieldPrefix)
        If SubPrefix > "" Then
            FPf.SET_LABELTEXTS_FROM_SEQ(ServerObject_Prefix, SubPrefix, FieldPrefix)
        End If

        RAISEEVENT_CONTROLS_INITIALIZED()

        CONTROLS_ARRANGE()

        RAISEVENT_CONTROLS_INITIALIZED_AND_ARRANGED()

        INIT_CONTROLS = OUT
    End Function

    Public Function CONTROLS_ADD(ByVal FPc As FP_Control) As Boolean
        Dim OUT As Boolean = True

        With CONTROLS
            If .ContainsKey(FPc.FieldName) Then
                FPf.FPApp.DoErrorMsgBox("FP.CONTROLS_ADD", 0, String.Format("Control {0} already exists.", FPc.FieldName))
                'CONTROLS(FPc.FieldName) = FPc
            Else
                CONTROLS.Add(FPc.FieldName, FPc)
            End If
        End With

        'Az FPf.CONTROLS-hoz nem kell hozzaadni, mert az sima control-ok gyujtemenye, igy az FPc.c mar vagy eleve ott van, vagy a krealaskor kellett azt hozzaadni.
        'If FPc.CreatedAtRuntime Then
        '    FPf.CONTROLS_ADD(FPc.c)
        '    If Not (FPc.c_Label Is Nothing) Then
        '        FPf.CONTROLS_ADD(FPc.c)
        '    End If
        'End If

        CONTROLS_ADD = OUT
    End Function

    Public Function CONTROLS_REFRESH_FROM_RS(Optional ByVal Goto_Current_Record_After_Refresh As Boolean = True) As Boolean
        Dim OUT As Boolean = True

        If Not FPf.SAVE_ALL Then
            OUT = False
        Else
            Dim FP_Grid_Control_Collection As New Struct_FP_GRID_CONTROL_COLLECTION
            Dim FP_Control_Collection As New Struct_FP_CONTROLS_COLLECTION
            Dim CurrentRecordID As Long = P_DATA_Current_ID

            FORM_GOTO_NORECORD()

            If GRID Is Nothing Then
                With FP_Grid_Control_Collection
                    .GRID = Nothing
                    .Label = Nothing
                    .Btn_FooterVisible = Nothing
                    .Footer_Panel = Nothing
                End With
            Else
                With FP_Grid_Control_Collection
                    GRID.DT = Nothing
                    .GRID = GRID.GRID

                    Dim i As Integer = 0
                    While i < .GRID.Columns.Count
                        If .GRID.Columns(i).IsDataBound Then
                            .GRID.Columns.Remove(.GRID.Columns(i))
                        Else
                            i += 1
                        End If
                    End While

                    .Label = GRID.GRID_Label
                    If GRID.Btn_FooterVisible Is Nothing Then
                        .Btn_FooterVisible = Nothing
                    Else
                        .Btn_FooterVisible = GRID.Btn_FooterVisible.c
                    End If
                    .Footer_Panel = GRID.Footer_Panel
                End With

                GRID.GRID_REMOVE_ALL_CONTROLS()

                GRID.Dispose()
                GRID.GRID = Nothing
                GRID = Nothing
            End If

            With FP_Control_Collection
                .GRID = FP_Grid_Control_Collection
                .BindingNavigator = BINDINGNAVIGATOR_FORM
                .Btn_Filter = ButtonFilter
                .Btn_Filter_Line = ButtonFilterLine
                .FilterText = FilterText
                .Btn_Find = ButtonFind
                .Btn_ExportToExcel = ButtonExportToExcel
                .Btn_ImportFromExcel = ButtonImportFromExcel
                .Btn_Print = ButtonPrint
                .Btn_Add_New = Button_Add_New
                .Btn_Del = Button_Delete
                .Btn_Up = Button_Up
                .Btn_Down = Button_Down
                .Btn_DuplicateRecord = Button_Duplicate_Record
                .FieldPrefix = FieldPrefix
            End With

            If CONTROLS.Keys.Count > 0 Then
                Dim Fields As String()
                Dim i As Integer

                Fields = CONTROLS.Keys.ToArray()
                For i = UBound(Fields) To 0 Step -1
                    With CONTROLS(Fields(i))
                        If Left(.FieldName, 1) <> "#" Then
                            CONTROLS_REMOVE(Fields(i))
                        End If
                    End With
                Next
            End If

            If PICTUREBOXES.Count > 0 Then
                Dim Fields As String()
                Dim i As Integer

                Fields = PICTUREBOXES.Keys.ToArray()
                For i = 0 To UBound(Fields)
                    With PICTUREBOXES(Fields(i))
                        If Left(.FieldName, 1) <> "#" Then
                            PICTUREBOXES_REMOVE(Fields(i))
                        End If
                    End With
                Next
            End If

            ARRANGE_DT = Nothing

            INIT_CONTROLS(FP_Control_Collection)

            If Goto_Current_Record_After_Refresh Then
                If Not (GRID Is Nothing) Then
                    GRID.FILL()
                End If

                FORM_GOTO_RECORD_BY_ID(CurrentRecordID)
            End If
        End If

        CONTROLS_REFRESH_FROM_RS = OUT
    End Function

    Public Sub CONTROLS_REMOVE(ByVal ControlName As String)
        If Not CONTROLS.Keys.Contains(ControlName) Then
            'Nothing to do
            ' FPf.FPApp.DoErrorMsgBox("FP.CONTROLS_REMOVE", 0, String.Format("Control '{0}' not found", ControlName))
        Else
            Dim c As Control = CONTROLS(ControlName).c
            Dim c_Label As Label = CONTROLS(ControlName).c_Label
            Dim CreatedAtRuntime As Boolean = CONTROLS(ControlName).CreatedAtRuntime

            With CONTROLS(ControlName)
                If .Equals(FPf.ActiveControl) Then
                    FPf.ActiveControl = Nothing
                End If
                .Dispose()
            End With

            If Not (c Is Nothing) Then
                If CreatedAtRuntime Then
                    FPf.CONTROLS_REMOVE(c.Name)
                    c.Dispose()
                    c = Nothing
                End If
            End If

            If Not (c_Label Is Nothing) Then
                If CreatedAtRuntime Then
                    FPf.CONTROLS_REMOVE(c_Label.Name)
                    c_Label.Dispose()
                    c_Label = Nothing
                End If
            End If

            CONTROLS.Remove(ControlName)
        End If
    End Sub

    Public Function CONTROLS_GET_FieldName_Without_FieldPrefix(MyFieldName As String) As String
        Dim OUT As String = MyFieldName

        If FieldPrefix > "" Then
            Dim Length_of_FieldPrefix As Integer = Strings.Len(FieldPrefix)

            If Strings.Left(MyFieldName, Length_of_FieldPrefix) = FieldPrefix Then
                OUT = Mid(MyFieldName, Length_of_FieldPrefix + 1)
            End If
        End If

        Return OUT
    End Function

    Public Function CONTROLS_GET_FPc(ByVal ControlName As String, Optional ControlName_Includes_FieldPrefix As Boolean = True) As FP_Control
        Dim OUT As FP_Control = Nothing
        Dim MyFieldName As String = ControlName

        If ControlName_Includes_FieldPrefix Then
            MyFieldName = FieldName_Get_From_ControlName(ControlName)
        End If

        If CONTROLS.ContainsKey(MyFieldName) Then
            OUT = CONTROLS(MyFieldName)
        End If

        CONTROLS_GET_FPc = OUT
    End Function

    Private CONTROLS_ARRANGE_Runs As Boolean = False

    Public Function CONTROLS_GET_FIRST_FPc() As FP_Control
        Dim OUT As FP_Control = Nothing
        Dim MinTabIndex As Integer = 9999

        For Each AktKey As String In CONTROLS.Keys
            Dim FPc As FP_Control = CONTROLS(AktKey)

            If MinTabIndex > FPc.c.TabIndex Then
                If Not (FPc.c Is Nothing) Then
                    If FPc.c.Visible Then
                        OUT = CONTROLS(AktKey)
                        MinTabIndex = FPc.c.TabIndex
                    End If
                End If
            End If
        Next

        CONTROLS_GET_FIRST_FPc = OUT
    End Function

    Public Sub CONTROLS_ARRANGE()
        If Not CONTROLS_ARRANGE_Runs Then
            CONTROLS_ARRANGE_Runs = True
            RaiseEvent Form_Controls_Arrange_Begin(Me)

            Dim Footer_Showed As Boolean = False

            If GRID_EXISTS() Then
                If GRID.GRID.Visible Then
                    If Not (GRID.Footer_Panel Is Nothing) Then
                        If Not (GRID.Btn_FooterVisible Is Nothing) Then
                            Footer_Showed = GRID.Btn_FooterVisible.P_PRESSED
                        Else
                            Footer_Showed = GRID.Footer_Panel.Visible
                            GRID.FOOTER_SHOW()
                        End If
                    End If
                End If

                FPf.CONTROLS_ARRANGE(ServerObject_Prefix, SubPrefix, ARRANGE_DT, FieldPrefix)

                If GRID.GRID.Visible Then
                    GRID.GRID_OrigHeight_INIT()
                    If Not (GRID.Footer_Panel Is Nothing) Then
                        If Footer_Showed Then
                            GRID.FOOTER_SHOW()
                        Else
                            GRID.FOOTER_HIDE()
                        End If
                    Else
                        GRID.FILTER_PANEL_ARRANGE()
                    End If
                End If
            Else
                FPf.CONTROLS_ARRANGE(ServerObject_Prefix, SubPrefix, ARRANGE_DT, FieldPrefix)
            End If

            RaiseEvent Form_Controls_Arrange_End(Me)
            CONTROLS_ARRANGE_Runs = False
        End If
    End Sub
    Public Sub CONTROLS_CLEAR_ALL()
        Dim AktKey As String = ""

        For Each AktKey In CONTROLS.Keys
            CONTROLS(AktKey).FIELD_CLEAR()
        Next

        COLORING_ALL()
    End Sub

    Public Sub CONTROLS_CLEAR_ALL(TP As TabPage)
        Dim AktKey As String
        For Each AktKey In CONTROLS.Keys
            If CONTROLS(AktKey).c.Parent.Name.ToUpper = TP.Name.ToUpper Then
                CONTROLS(AktKey).FIELD_CLEAR()
            End If
        Next
        COLORING_ALL()
    End Sub

    Public Function PICTUREBOXES_ADD(ByVal MyFPp As FP_PictureBox) As Boolean
        Dim OUT As Boolean = False

        'If nz(MyFPp.FieldName, "") = "" Then
        MyFPp.FieldName = FieldName_Get_From_ControlName(MyFPp.c.Name)
        'End If
        If PICTUREBOXES.ContainsKey(MyFPp.FieldName) Then
            FPf.FPApp.DoErrorMsgBox("FP.PICTUREBOXES_ADD", 0, String.Format("Field '{0}' is already exists.", MyFPp.FieldName))
        Else
            PICTUREBOXES.Add(MyFPp.FieldName, MyFPp)
            OUT = True
        End If

        PICTUREBOXES_ADD = OUT
    End Function
    Public Sub Dispose()
        If Disposed = False Then
            Disposed = True

            BINDINGNAVIGATOR_FORM = Nothing
            BINDINGNAVIGATOR_FORM_MoveFirstItem = Nothing
            BINDINGNAVIGATOR_FORM_MovePreviousItem = Nothing
            BINDINGNAVIGATOR_FORM_PositionItem = Nothing
            BINDINGNAVIGATOR_FORM_CountItem = Nothing
            BINDINGNAVIGATOR_FORM_MoveNextItem = Nothing
            BINDINGNAVIGATOR_FORM_MoveLastItem = Nothing
            BINDINGNAVIGATOR_FORM_AddNewItem = Nothing
            BINDINGNAVIGATOR_FORM_DeleteItem = Nothing
            GRID = Nothing
            FilterText = Nothing
            Button_Add_New = Nothing
            Button_Up = Nothing
            Button_Down = Nothing
            Button_Delete = Nothing
            ButtonFilter = Nothing
            ButtonFilterLine = Nothing
            ButtonFind = Nothing
            ButtonExportToExcel = Nothing
            ButtonImportFromExcel = Nothing
            ButtonPrint = Nothing

            BINDINGNAVIGATOR_FORM_BS = Nothing
            'BINDINGNAVIGATOR_FORM_RS = Nothing

            If Not (XLS_IMPORT Is Nothing) Then
                XLS_IMPORT.Dispose()
                XLS_IMPORT = Nothing
            End If

            FP_DOCMAN = Nothing

            FP_Active_Only_When_This_Field_Visible = Nothing

            Dim List_Of_Controls As List(Of String) = CONTROLS.Keys.ToList
            For i As Integer = 0 To List_Of_Controls.Count - 1
                If CONTROLS.Keys.Contains(List_Of_Controls(i)) Then
                    Dim Current_FPc As FP_Control = CONTROLS(List_Of_Controls(i))

                    If Not (Current_FPc.c Is Nothing) Then
                        CONTROLS_REMOVE(Current_FPc.c.Name)
                    End If
                End If
            Next

            Dim List_Of_Pictureboxes As List(Of String) = PICTUREBOXES.Keys.ToList
            For i As Integer = 0 To List_Of_Pictureboxes.Count - 1
                If CONTROLS.Keys.Contains(List_Of_Pictureboxes(i)) Then
                    Dim Current_FPp As FP_PictureBox = PICTUREBOXES(List_Of_Pictureboxes(i))

                    If Not (Current_FPp.c Is Nothing) Then
                        PICTUREBOXES_REMOVE(Current_FPp.c.Name)
                    End If
                End If
            Next

            If GRID_EXISTS() Then
                GRID.Dispose()
            End If
        End If
    End Sub

    Public Function FieldName_Get_From_ControlName(MyControlName As String) As String
        Dim OUT As String

        If FieldPrefix = "" Then
            OUT = MyControlName
        Else
            If Strings.Left(MyControlName, 1) = "#" Then
                OUT = MyControlName
            Else
                If FieldPrefix = "" Then
                    OUT = MyControlName
                Else
                    If Strings.Left(MyControlName, Len(FieldPrefix)) = FieldPrefix Then
                        OUT = Mid(MyControlName, Len(FieldPrefix) + 1)
                    Else
                        OUT = MyControlName
                    End If
                End If
            End If
        End If

        Return OUT
    End Function

    Public Function PICTUREBOXES_GET(ByVal Control_Name As String, Optional ControlName_Includes_FieldPrefix As Boolean = True) As FP_PictureBox
        Dim OUT As FP_PictureBox = Nothing
        Dim MyFieldName As String = Control_Name

        If ControlName_Includes_FieldPrefix Then
            MyFieldName = FieldName_Get_From_ControlName(Control_Name)
        End If

        If PICTUREBOXES.ContainsKey(MyFieldName) Then
            OUT = PICTUREBOXES(MyFieldName)
        End If

        Return OUT
    End Function
    Public Sub PICTUREBOXES_REMOVE(ByVal Name As String)
        If Not PICTUREBOXES.ContainsKey(Name) Then
            FPf.FPApp.DoErrorMsgBox("FP.PICTUREBOXES_REMOVE", 0, String.Format("Picturebox '{0}' not found.", Name))
        Else
            With PICTUREBOXES(Name)
                If Not (.c Is Nothing) Then
                    If .CreatedAtRuntime Then
                        FPf.CONTROLS_REMOVE(.c.Name)
                        .c.Dispose()
                    End If
                    PICTUREBOXES.Remove(Name)
                End If
            End With
        End If
    End Sub

    Public ReadOnly Property P_DATA_Current_SeqNum() As Long
        Get
            P_DATA_Current_SeqNum = DATA_GET_CURRENT_SEQNUM()
        End Get
    End Property

    Public ReadOnly Property P_DATA_Current_ID() As Long
        Get
            If P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD And DATA_DT_FORM_Current_ID <> 0 Then
                FPf.FPApp.DoErrorMsgBox("FP.P_DATA_Current_ID", 0, "Internal error.")
                P_DATA_Current_ID = 0
            Else
                P_DATA_Current_ID = DATA_DT_FORM_Current_ID
            End If
        End Get
    End Property

    Public ReadOnly Property P_DATA_RecordCount() As Integer
        Get
            P_DATA_RecordCount = DATA_DT_FORM.Rows.Count
        End Get
    End Property

    Public ReadOnly Property P_DATA_RecordStatus() As ENUM_RecordStatus
        Get
            Dim OUT As ENUM_RecordStatus

            If DATA_NoRecord Then
                OUT = ENUM_RecordStatus.NORECORD
            Else
                If P_DATA_NewRecord Then
                    OUT = ENUM_RecordStatus.NEWRECORD
                Else
                    OUT = ENUM_RecordStatus.EXISTS
                End If
            End If

            P_DATA_RecordStatus = OUT
        End Get
    End Property

    Public ReadOnly Property P_FieldPrefix As String
        Get
            Return FieldPrefix
        End Get
    End Property
    Public ReadOnly Property P_DATA_NewRecord() As Boolean
        Get
            If DATA_NoRecord Then
                P_DATA_NewRecord = False
            Else
                P_DATA_NewRecord = (DATA_DT_FORM_Current_ID = 0)
            End If
        End Get
    End Property

    Public Property P_FORM_AllowAdditions() As Boolean
        Get
            P_FORM_AllowAdditions = Data_AllowAdditions
        End Get
        Set(ByVal value As Boolean)
            If value <> Data_AllowAdditions Then
                If value = True Then
                    Data_AllowAdditions = value
                    If GRID_EXISTS() Then
                        GRID.GRID_AllowAdditions_SET(value)
                    End If
                Else
                    If P_FORM_Dirty Then
                        FPf.FPApp.DoErrorMsgBox("FP.P_FORM_AllowAdditions", 0, "You can't set the P_FORM_AllowAdditions property to false until the forms data is Dirty.")
                    Else
                        Data_AllowAdditions = value
                        If GRID_EXISTS() Then
                            GRID.GRID_AllowAdditions_SET(value)
                        End If

                        If P_DATA_RecordStatus = ENUM_RecordStatus.NEWRECORD Then
                            FORM_GOTO_NORECORD()
                        End If
                    End If
                End If
            End If
        End Set
    End Property
    Public Property P_FORM_AllowDeletions() As Boolean
        Get
            P_FORM_AllowDeletions = Data_AllowDeletions
        End Get
        Set(ByVal value As Boolean)
            Data_AllowDeletions = value
        End Set
    End Property
    Public Property P_FORM_AllowEdits() As Boolean
        Get
            P_FORM_AllowEdits = Data_AllowEdits
        End Get
        Set(ByVal value As Boolean)
            If value <> Data_AllowEdits Then
                If value = True Then
                    Data_AllowEdits = value
                Else
                    If P_FORM_Dirty Then
                        FPf.FPApp.DoErrorMsgBox("FP.P_FORM_AllowEdits", 0, "You can't set the P_FORM_AllowEdits property to false until the forms data is Dirty.")
                    Else
                        Data_AllowEdits = value
                    End If
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property P_FORM_Dirty() As Boolean
        Get
            P_FORM_Dirty = Dirty
        End Get
    End Property

    Public Property P_DragRow_Allowed() As Boolean
        Get
            Dim OUT As Boolean = False

            If GRID_EXISTS() Then
                OUT = GRID.GRID.AllowDrop
            End If

            P_DragRow_Allowed = OUT
        End Get
        Set(ByVal value As Boolean)
            If GRID_EXISTS() Then
                If GRID.GRID.AllowDrop <> value Then
                    GRID.GRID.AllowDrop = value
                End If
            End If
        End Set
    End Property

    Public Function FORMDEF(ByVal Key As String, Optional Value_If_Null As String = "") As String
        Dim OUT As String

        OUT = FPf.FPApp.FIXTEXT_getParam(Key, FORMDEF_DIC)

        If OUT = "" Then
            OUT = Value_If_Null
        End If

        Return OUT
    End Function

    Public Function FORMDEF_PRINTOPTIONS(ByVal Identifier As String, Optional Value_If_Null As String = "") As String
        Dim OUT As String

        OUT = FPf.FPApp.FIXTEXT_getParam(Identifier, FORMDEF_PrintOption_DIC)

        If OUT = "" Then
            OUT = Value_If_Null
        End If

        Return OUT
    End Function

    Public Function FORMDEF_PRINTOPTIONS_PARAMS(Identifier As String) As Struct_PrintDocs_Options
        Dim OUT As New Struct_PrintDocs_Options

        If FORMDEF_PrintOption_DIC.Keys.Contains(Identifier) Then
            OUT = ButtonPrint_getPrintDocOptionsFromLine(Identifier, FORMDEF_PrintOption_DIC(Identifier))
        End If

        Return OUT
    End Function

    Public Function FORMDEF_SETTINGS(ByVal Key As String, Optional Value_If_Null As String = "") As String
        Dim OUT As String

        OUT = FPf.FPApp.FIXTEXT_getParam(Key, FORMDEF_SETTINGS_DIC)

        If OUT = "" Then
            OUT = Value_If_Null
        End If

        Return OUT
    End Function

    Private Function SET_FP_Sub_Where_ON(My_FP As FP) As String
        Dim OUT As String
        Dim IDS As String

        Dim SubWHERE = SET_FP_Sub_Where_OFF(My_FP)

        IDS = Sub_IDS(My_FP)
        If IDS <> String.Empty Then
            OUT = String.Format("ID IN({0})", IDS)
        Else
            OUT = SubWHERE
        End If

        Return OUT
    End Function
    Private Function SET_FP_Sub_Where_OFF(My_FP As FP) As String
        Dim OUT As String
        Dim SqlComm As New SqlClient.SqlCommand
        Dim Parent_Record_ID As Integer = My_FP.Parent_FP.P_DATA_Current_ID
        Dim Parent_FP_Joined_Data_Fields As String = My_FP.Parent_FP_JOINED_DATA_FIELD
        OUT = String.Format("({0} = {1})", Parent_FP_Joined_Data_Fields, Parent_Record_ID)
        Return OUT
    End Function
    Public Function FORM_RECORDS_LOAD(Optional ByVal SubWHERE As String = "", Optional ByVal LET_NEWRECORD As Boolean = False, Optional ByVal NoRecord_OK As Boolean = False, Optional ByVal WithoutDoFilter As Boolean = False) As Boolean
        Dim OUT As Boolean = False

        Select Case Me.FP_ALIAS
            Case "ORD_L", "CONT"
                If Me.ROOT_FP.ButtonFilterLine IsNot Nothing Then
                    If Me.ROOT_FP.PICTUREBOXES.ContainsKey("Btn_Filter_Line") Then
                        Dim Btn As FP_PictureBox = Me.ROOT_FP.PICTUREBOXES("Btn_Filter_Line")
                        If Btn.P_PRESSED Then
                            SubWHERE = SET_FP_Sub_Where_ON(Me)
                        Else
                            SubWHERE = SET_FP_Sub_Where_OFF(Me)
                        End If
                    End If
                End If
            Case Else
                'nothing to do
        End Select

        Dim Handled As Boolean = False
        RaiseEvent Form_Records_Loading(Me, SubWHERE, NoRecord_OK, OUT, Handled)

        If Handled = False Then
            If WithoutDoFilter = False Or DOFILTER_ReturnedParams.LetNewRecord <> LET_NEWRECORD Then
                FPf.FPApp.DoFilter_Params_CLEAR(DOFILTER_ReturnedParams, LET_NEWRECORD)
            End If

            If UnboundForm Then
                FPf.FPApp.DoErrorMsgBox("FP.FORM_RECORDS_LOAD", 0, String.Format("FP '{0}' is unbound. Unbound FPs can't load records.", ServerObject_Prefix))
            Else
                If (Parent_FP Is Nothing) Then
                    If FPf.FPApp.InitGlobals = False Then
                        Exit Function
                    End If
                End If

                Dim RightsOK As Boolean = True

                If LET_NEWRECORD Then
                    If Not P_FORM_AllowAdditions Then
                        RightsOK = False
                    Else
                        If Not FORM_GETRIGHT("FORM_INSERT") Then
                            RightsOK = False
                        End If
                    End If
                End If
                If RightsOK Then
                    Dim CloseDoFilter As Boolean = False
                    Dim setRecordSource As Boolean = False
                    Dim FilterName As String = FORMDEF("FILTERNAME")
                    Dim ID_of_NewRecord As Long = 0

                    'Fent ezt mar egyszer megvizsgalta (SSS)
                    'If WithoutDoFilter = False Or DOFILTER_ReturnedParams.LetNewRecord <> LET_NEWRECORD Then
                    '    FPf.FPApp.DoFilter_Params_CLEAR(DOFILTER_ReturnedParams, LET_NEWRECORD)
                    'End If
                    While Not CloseDoFilter
                        setRecordSource = False
                        ID_of_NewRecord = 0

                        If FilterName = "" Or LET_NEWRECORD Or WithoutDoFilter Then
                            gl_Doit = True
                            CloseDoFilter = True
                            If WithoutDoFilter = False Or DOFILTER_ReturnedParams.LetNewRecord <> LET_NEWRECORD Then
                                FPf.FPApp.DoFilter_Params_CLEAR(DOFILTER_ReturnedParams, LET_NEWRECORD)
                                If WithoutDoFilter = False Then
                                    FORM_SubWHERE_for_NewRecords = ""
                                End If
                            End If
                        Else
                            SubWHERE = FORM_SubWHERE_FIX 'Mert a SubWHERE tulajdonkeppen arra lett kitalalva, hogy lehessen szurni DOFILTER nelkul. Ha van DOFILTER, akkor ertelmetlen.
                            FPf.FPApp.DOFILTER(FPf, FilterName, SQL_BIND_Params.NameOf_WhereQuery, FilterText, P_FORM_AllowAdditions)
                            DOFILTER_ReturnedParams = FPf.FPApp.gl_FilterParams
                        End If
                        If Not gl_Doit Then
                            CloseDoFilter = True
                            OUT = False
                        Else
                            If Not DOFILTER_ReturnedParams.LetNewRecord Then
                                setRecordSource = True
                                FORM_SubWHERE_for_NewRecords = ""
                            Else
                                If Not FORM_GETRIGHT("FORM_INSERT") Then
                                    FPf.FPApp.DoMyMsgBox(878) 'Add new record is not allowed.
                                    DOFILTER_ReturnedParams.LetNewRecord = False
                                Else
                                    Dim Cancel As Integer = False

                                    RaiseEvent Form_BeforeInsert(Me, Cancel, ID_of_NewRecord)

                                    CURSOR_SHOW_DEFAULT()

                                    If Cancel = 0 Then
                                        CURSOR_SHOW_WAIT()
                                        FORM_CLEAR_SQLFIELDS()
                                        If ID_of_NewRecord <> 0 Then
                                            FORM_SubWHERE_for_NewRecords = ""
                                            With DOFILTER_ReturnedParams
                                                If Not FORM_IsSubForm() Then
                                                    .FilterWHERE = String.Format("ID = {0}", ID_of_NewRecord)
                                                End If
                                                .LetNewRecord = False
                                            End With
                                            setRecordSource = True
                                        Else
                                            FPf.FPApp.DoFilter_Params_CLEAR(DOFILTER_ReturnedParams, True)
                                            If FORM_SubWHERE_for_NewRecords = "" Then
                                                FORM_SubWHERE_for_NewRecords = String.Format("ID > {0}", FPf.FPApp.NachsteNummerVergeben)
                                            End If
                                            setRecordSource = True
                                        End If
                                    End If
                                End If
                            End If

                            If P_FP_Refresh_Field_Visible = False Then
                                OUT = True
                            Else
                                If setRecordSource Then
                                    RS_WHERE = DATA_RS_WHERE(SubWHERE)

                                    If DATA_RS_SET(RS_WHERE) Then
                                        If RS_RecCount >= MAXRECORDS Then
                                            If FilterName > "" Then
                                                FPf.FPApp.DoMyMsgBox(58) 'Tul sok rekord felel meg a kriteriumoknak. Adjon meg tovabbi szurofelteteleket.
                                            End If
                                        Else
                                            If NoRecord_OK = False Then
                                                If GRID_EXISTS() Then
                                                    If P_FORM_AllowAdditions Then
                                                        NoRecord_OK = True
                                                    End If
                                                End If
                                            End If

                                            If FORM_RECORDS_LOAD_FROM_EXISTING_RS(NoRecord_OK Or DOFILTER_ReturnedParams.LetNewRecord) Then
                                                Dim U As Integer = 0

                                                If Not (DOFILTER_ReturnedParams.FilterTexts Is Nothing) Then
                                                    UBound(DOFILTER_ReturnedParams.FilterTexts)
                                                End If
                                                If U > 0 Then
                                                    For Ui As Integer = 0 To U - 1
                                                        Dim FieldNames() As String

                                                        FieldNames = Split(DOFILTER_ReturnedParams.FilterFields(Ui), ",")
                                                        For Uu As Integer = 0 To UBound(FieldNames)
                                                            FPf.FILTER_TEXT_SET(Trim(FieldNames(Uu)), DOFILTER_ReturnedParams.FilterTexts(Ui))
                                                        Next Uu
                                                    Next Ui
                                                End If

                                                CloseDoFilter = True
                                                FORM_SubWHERE = SubWHERE

                                                If Not (FilterText Is Nothing) Then
                                                    If DOFILTER_ReturnedParams.LetNewRecord Then
                                                        FilterText.Text = "" '+++ 'New Record' szoveg kene...
                                                    Else
                                                        FilterText.Text = DOFILTER_ReturnedParams.FilterText
                                                    End If
                                                End If

                                                If ID_of_NewRecord <> 0 Then
                                                    OUT = FORM_GOTO_RECORD_BY_ID(ID_of_NewRecord)

                                                ElseIf DOFILTER_ReturnedParams.LetNewRecord Then
                                                    OUT = FORM_GOTO_NEWRECORD()
                                                Else
                                                    OUT = True
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End While
                End If
                'End If
            End If
        End If

        FORM_RECORDS_LOAD = OUT
    End Function
    Public Function FORM_RECORDS_LOAD_FROM_EXISTING_RS(Optional ByVal NoRecord_OK = False) As Boolean
        Dim OUT As Boolean = False

        If DATA_RECORDS_LOAD_FROM_RS(NoRecord_OK) Then
            If GRID_EXISTS() Then
                GRID.FILL()
                If DATA_DT_FORM.Rows.Count <> GRID.DT.Rows.Count Then
                    MsgBox(String.Format("FP.FORM_RECORDS_LOAD_FROM_EXISTING_RS: {0}.Rowcount <> {1}.Rowcount (RS_ID = {2})", SQL_BIND_Params.NameOf_WhereQuery, SQL_BIND_Params.NameOf_GRID, RS_ID))
                End If
                'If DATA_NoRecord Then
                If P_DATA_RecordCount > 0 Then
                    FORM_GOTO_RECORD_GRID_FIRSTROW()
                Else
                    If Data_AllowAdditions Then
                        DATA_DT_FORM_Current_ID = 0
                        DATA_NoRecord = False
                    End If
                End If
                'End If
            End If

            If DATA_NoRecord Then
                FORM_RECORDS_LOAD_CURRENT() 'Kitorli a mezoket
                OUT = ((P_DATA_RecordCount > 0) Or NoRecord_OK)
            Else
                If FORM_RECORDS_LOAD_CURRENT() Then
                    OUT = True
                End If
            End If
        End If

        FORM_RECORDS_LOAD_FROM_EXISTING_RS = OUT
    End Function
    Public Sub FORM_CLEAR_SQLFIELDS()
        DATA_CLEAR_SQLFIELDS()
        If Not (FilterText Is Nothing) Then
            FilterText.Text = ""
        End If

        FORM_BIND_DATA()
    End Sub

    Public Function FORM_ADD_NEW() As Boolean
        If FPf.SAVE_ALL Then
            FORM_ADD_NEW = FORM_RECORDS_LOAD(FORM_SubWHERE, True)
        End If
    End Function
    Public Function FORM_RECORDS_DELETE_CURRENT(Optional ByVal mitDialogYesNo As Boolean = True, Optional Close_Form_when_NoRecord As Boolean = True) As Boolean
        Dim OUT As Boolean = False

        If P_FORM_AllowDeletions Then
            If P_DATA_Dirty Then
                UNDO()
            End If

            If P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                If Not FORM_GETRIGHT("FORM_DELETE") Then
                    FPf.FPApp.DoMyMsgBox(61) 'Sajnalom, de Onnek nincs joga torolni.
                Else
                    Dim Cancel As Integer = 0
                    Dim Current_SeqNum As Long = DATA_GET_CURRENT_SEQNUM()

                    RaiseEvent Form_BeforeDelete(Me, Cancel)
                    If Not Cancel Then
                        Dim DialResult As Long = 2

                        If mitDialogYesNo Then
                            DialResult = FPf.FPApp.DoMyMsgBox(62, , "SEQ,NEIN", "SEQ,JA") 'Valoban toroljem az adatsort? Nem|Igen
                        End If

                        If DialResult = 2 Then
                            'A FORM_SETLOCK hasznalatbol valo kivetelenek indoklasat lasd a FORM_SETLOCK
                            'eljaras elejen talalhato megjegyzesben.
                            'If FORM_SETLOCK() Then
                            If Not DATA_RECORDS_DELETE_CURRENT() Then
                                FORM_DORESYNC() 'Mert valoszinuleg a DEL eljaras uj TransactID-vel lefrissitette a rekordot, mielott ki probalta volna torolni.
                                'Igy a Form-on levo betoltott rekord TransactID-je elter a szereveretol.
                            Else
                                OUT = True
                                FORM_CLEAR_SQLFIELDS()
                                RAISEEVENT_Form_AfterDelete()
                                DATA_RS_SET(RS_WHERE)
                                DATA_RECORDS_LOAD_FROM_RS(True)
                                FORM_GOTO_NORECORD()
                                If GRID_EXISTS() Then
                                    GRID.FILL()
                                End If

                                If Current_SeqNum > RS_RecCount Then
                                    Current_SeqNum = RS_RecCount
                                End If

                                If Current_SeqNum > 0 Then
                                    FORM_GOTO_RECORD_BY_SeqNum(Current_SeqNum)
                                Else
                                    If FORM_IsSubForm() Then
                                        If P_FORM_AllowAdditions Then
                                            FORM_GOTO_NEWRECORD()
                                        Else
                                            FORM_GOTO_NORECORD()
                                        End If
                                    Else
                                        If GRID_EXISTS() Then
                                            If P_FORM_AllowAdditions Then
                                                FORM_GOTO_NEWRECORD()
                                            Else
                                                FORM_GOTO_NORECORD()
                                            End If
                                        Else
                                            If FORMDEF("FILTERNAME") > "" Then
                                                If Not FORM_RECORDS_LOAD() Then
                                                    FPf.Frm.Close()
                                                End If
                                            Else
                                                If Not FORM_GOTO_NEWRECORD() Then
                                                    If Close_Form_when_NoRecord Then
                                                        FPf.Frm.Close()
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        FORM_RECORDS_DELETE_CURRENT = OUT
    End Function

    Public Function FORM_GETRIGHT(ByVal RightCode As String, Optional ByVal FieldName As String = "", Optional ByVal IntParam As Long = 0, Optional ByVal StrParam As String = "") As Boolean
        Dim OUT As Boolean = False

        If FPf.FPApp.InitGlobals() Then
            If RightCode$ = "FORM_INSERT" And FORM_IsSubForm() Then
                OUT = Parent_FP.FORM_GETRIGHT("FORM_UPDATE", "SUBFORM_FORM_INSERT", IntParam, ServerObject_Prefix)
            Else
                Dim Right_StoredProc As String = FORMDEF("Right_StoredProc")

                If Right_StoredProc = "" Then
                    If Not FORM_IsSubForm() Then
                        OUT = True
                    Else
                        OUT = Parent_FP.FORM_GETRIGHT(RightCode, "SUBFORM_" & RightCode, IntParam, ServerObject_Prefix)
                    End If
                Else
                    Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()
                    Using sqlComm
                        Dim Result As Boolean
                        FPf.FPApp.DC.Qdf_set_SP(sqlComm, Right_StoredProc)
                        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ID", SqlDbType.Int, , , , , P_DATA_Current_ID)
                        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@RightCode", SqlDbType.NVarChar, , 30, RightCode)
                        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@FieldName", SqlDbType.NVarChar, , 50, FieldName)
                        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@IntParam", SqlDbType.Int, , , , , IntParam)
                        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@StrParam", SqlDbType.NVarChar, , 50, StrParam)

                        CURSOR_SHOW_WAIT()
                        Try
                            Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG)
                        Catch ex As Exception
                            Result = False
                            FPf.FPApp.DoErrorMsgBox("FP.FORM_GETRIGHT", Err.Number, Err.Description)
                        End Try

                        CURSOR_SHOW_DEFAULT()

                        OUT = (nz(sqlComm.Parameters.Item("@RetValue").Value, 0) = 1)
                    End Using
                End If
            End If
        End If

        FORM_GETRIGHT = OUT

    End Function

    Public Function FORM_SETLOCK() As Boolean
        '2012-06-16 Hasznalatbol kivett, de meg nem megszuntetett eljaras. Indoklas:
        '---------------------------------------------------------------------------
        'Megfontolasom szerint NEM KELL a FORM_SETLOCK.
        'Egyreszt a _SAVE SQL objektum mentes elott amugy is leellenorzi, hogy volt-e a rekordon idokozben
        'valtozas. Ezzel gyakorlatilag opimistic lock-ot valosit meg.
        'Masreszt a RecordLock onmagaban nem menti meg a
        'folyamatokat attol, hogy a record ido kozben ne valtozzon.
        'Szamos folyamat van most is a SELEXPED-ben, ami a
        'hatterben mas rekordokat is valtoztat anelkul, hogy hasznalna vagy figyelne a setlock-ot.
        '(peldaul szamlazas, jovairas, statuszvaltas, stb)
        '
        'A FORM_Recordlock eredeti celjanak megfelelo eset elenyeszoen ritkan jon letre.
        'Az eljaras eredeti cel az volt, hogy ne lehessen a szerkesztest megkezdeni mas helyen
        'mar megnyitott rekordon. Ez azonban a gyakorlat alapjan rendkivul ritka eset. Ha nagy ritkan
        'elofordul, akkor az optimistic lock kivedi a parhuzamos valtoztatast.
        'Tehat az alapindok a FORM_SETLOCK kivetelere kettos:
        '  - Egyreszt egy rekord szerkesztese egesz elettartalmat tekintve merhetetlenul
        '    rovid ido. Igy nagyon kicsi a valoszinusege annak, hogy a problema egyaltalan fellep.
        '  - A recordlock funkcio torlese, a veletlen bennragadasok elkerulesere beepitendo
        '    triggerek ugyanakkor fajdalmasan lassithatjak a rendszert. Trigger kene
        '    tobbek kozott az RS es/vagy az RS_L tablakra is, ami sulyos belassulasokat okozhatna.
        '
        ' E Z E K   A L A P J A N:
        '-------------------------
        '
        'Ha feltetlenul el akarjuk kerulni a szerkesztes megkezdeset egy megvaltoztatott recordon, sokkal
        'celszerubb kozvetlenul a rekord szerkesztesre valo megnyitasa elott (Dirty esemeny) azt
        'ellenorizni, hogy a memoriaban levo record az aktualisan mentett adatokat tartalmazza-e 
        '(vagyis vizsgaljuk meg, hogy a TransactID egyenlo-e a tarolt record TransactID-jevel).

        Dim OUT As Boolean = True

        If FORMDEF("UniqueTable") = "" Then
            OUT = False
            FPf.DoErrorMsgBox("FP.FORM_SETLOCK", 0, "UniqueTable = ''")
        Else
            If P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                If FORM_IsSubForm() Then
                    OUT = Parent_FP.FORM_SETLOCK()
                Else
                    If FORMDEF("SETLOCK") = "1" Then
                        If Not FPf.FPApp.setRecordLock(P_DATA_Current_ID, , , FORM_GETFORM_LOCKNAME(), FORMDEF("UniqueTable")) Then
                            OUT = False
                        End If
                    End If
                End If
            End If
        End If

        FORM_SETLOCK = OUT
    End Function
    Public Function FORM_GETFORM_LOCKNAME() As String
        FORM_GETFORM_LOCKNAME = FPf.Frm.Name
    End Function

    Public Function FORM_GOTO_RECORD_BY_SeqNum(ByVal SeqNum As Long) As Boolean
        Dim GotoID As Long = DATA_GET_ID_FROM_SEQNUM(SeqNum)

        FORM_GOTO_RECORD_BY_SeqNum = FORM_GOTO_RECORD_BY_ID(GotoID)
    End Function
    Public Function FORM_GOTO_RECORD_BY_ID(ByVal ID As Long, Optional ByVal SetAsFirstRowInGRID As Boolean = False) As Boolean
        Dim OUT As Boolean = False

        If P_DATA_Current_ID = ID And P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            OUT = True
        Else
            If DATA_GOTO_RECORD_BY_ID(ID) Then
                BINDINGNAVIGATOR_SETPOSITION_BY_ID(DATA_DT_FORM_Current_ID)
                If GRID_EXISTS() Then
                    If Not GRID.GOTO_ROW_BY_RECORDID(DATA_DT_FORM_Current_ID, , SetAsFirstRowInGRID) Then
                        GRID.GOTO_NORECORD()
                    End If
                End If
                OUT = FORM_RECORDS_LOAD_CURRENT()
            End If
        End If
        FORM_GOTO_RECORD_BY_ID = OUT
    End Function
    Public Function FORM_GOTO_RECORD_BY_GRID_ROWINDEX(ByVal RowIndex As Integer) As Boolean
        Dim OUT As Boolean = False

        If GRID_EXISTS() Then
            If RowIndex >= 0 And RowIndex < GRID.GRID.RowCount Then
                Dim CurrentRowIndex As Integer = -1

                If Not (GRID.GRID.CurrentRow Is Nothing) Then
                    CurrentRowIndex = GRID.GRID.CurrentRow.Index
                End If
                If RowIndex = CurrentRowIndex Then
                    OUT = True
                Else
                    Dim RecordID As Long = GRID.GRID(GRID.GRID.Columns("RecordID").Index, RowIndex).Value

                    OUT = FORM_GOTO_RECORD_BY_ID(RecordID, False)
                End If
            End If
        End If

        FORM_GOTO_RECORD_BY_GRID_ROWINDEX = OUT
    End Function
    Public Function FORM_GOTO_RECORD_GRID_NEXTPAGE() As Boolean
        Dim OUT As Boolean = False

        If GRID_EXISTS() Then
            If Not (GRID.DT Is Nothing) Then
                If GRID.GRID.CurrentRow IsNot Nothing Then
                    Dim RowIndex As Integer = GRID.GRID.CurrentRow.Index + GRID.GRID.DisplayedRowCount(False)

                    If RowIndex >= GRID.DT.Rows.Count Then
                        RowIndex = GRID.DT.Rows.Count - 1
                    End If
                    FORM_GOTO_RECORD_BY_GRID_ROWINDEX(RowIndex)
                End If
            End If
        End If

        FORM_GOTO_RECORD_GRID_NEXTPAGE = OUT
    End Function
    Public Function FORM_GOTO_RECORD_GRID_NEXTROW() As Boolean
        Dim OUT As Boolean = False

        If GRID_EXISTS() Then
            If GRID.GRID.CurrentRow IsNot Nothing Then
                OUT = FORM_GOTO_RECORD_BY_GRID_ROWINDEX(GRID.GRID.CurrentRow.Index + 1)
            End If
        End If

        FORM_GOTO_RECORD_GRID_NEXTROW = OUT
    End Function

    Public Function FORM_GOTO_RECORD_GRID_PREVIOUSROW() As Boolean
        Dim OUT As Boolean = False

        If GRID_EXISTS() Then
            If GRID.GRID.CurrentRow IsNot Nothing Then
                OUT = FORM_GOTO_RECORD_BY_GRID_ROWINDEX(GRID.GRID.CurrentRow.Index - 1)
            End If
        End If

        FORM_GOTO_RECORD_GRID_PREVIOUSROW = OUT
    End Function

    Public Function FORM_GOTO_RECORD_GRID_PREVIOUSPAGE() As Boolean
        Dim OUT As Boolean = False

        If GRID_EXISTS() Then
            If Not (GRID.DT Is Nothing) Then
                If GRID.GRID.CurrentRow IsNot Nothing Then
                    Dim RowIndex As Integer = GRID.GRID.CurrentRow.Index - GRID.GRID.DisplayedRowCount(False)

                    If RowIndex < 0 Then
                        RowIndex = 0
                    End If
                    FORM_GOTO_RECORD_BY_GRID_ROWINDEX(RowIndex)
                End If
            End If
        End If

        FORM_GOTO_RECORD_GRID_PREVIOUSPAGE = OUT
    End Function
    Public Function FORM_GOTO_RECORD_GRID_FIRSTROW() As Boolean
        FORM_GOTO_RECORD_GRID_FIRSTROW = FORM_GOTO_RECORD_BY_GRID_ROWINDEX(0)
    End Function
    Public Function FORM_GOTO_RECORD_GRID_LASTROW() As Boolean
        Dim OUT As Boolean

        If GRID_EXISTS() Then
            OUT = FORM_GOTO_RECORD_BY_GRID_ROWINDEX(GRID.GRID.RowCount - 1)
        End If

        OUT = FORM_GOTO_RECORD_GRID_LASTROW
    End Function
    Public Function FORM_GOTO_NORECORD() As Boolean
        Dim OUT As Boolean

        'If FORM_RECORDS_SAVE_CURRENT() Then
        DATA_GOTO_NORECORD()
        FORM_RECORDS_LOAD_CURRENT()
        FPf.FORM_DATETIME_MonthCalendar.Visible = False
        OUT = True
        'End If

        FORM_GOTO_NORECORD = OUT
    End Function
    Public Function FORM_GOTO_NEWRECORD() As Boolean
        Dim OUT As Boolean = False

        If Data_AllowAdditions Then
            OUT = FORM_GOTO_RECORD_BY_ID(0)
        End If

        FORM_GOTO_NEWRECORD = OUT
    End Function
    Public Function FORM_GOTO_NEXTRECORD() As Boolean
        Dim OUT As Boolean = False

        If Not P_DATA_NewRecord Then
            If GRID_EXISTS() Then
                Dim NewRecordID As Long = 0

                If GRID.ROW_GET_NEXTROW_RECORDID(NewRecordID) Then
                    OUT = FORM_GOTO_RECORD_BY_ID(NewRecordID)
                End If
            Else
                If Not P_DATA_NewRecord Then
                    OUT = FORM_GOTO_RECORD_BY_SeqNum(DATA_GET_CURRENT_SEQNUM() + 1)
                End If
            End If
        End If

        FORM_GOTO_NEXTRECORD = OUT
    End Function
    Public Function FORM_GOTO_PREVIOUSRECORD() As Boolean
        Dim OUT As Boolean = False

        If GRID_EXISTS() Then
            Dim NewRecordID As Long = 0

            If GRID.ROW_GET_PREVIOUSROW_RECORDID(NewRecordID) Then
                OUT = FORM_GOTO_RECORD_BY_ID(NewRecordID)
            End If
        Else
            If P_DATA_NewRecord Then
                OUT = FORM_GOTO_RECORD_BY_SeqNum(DATA_RECORDS_GET_MAX_SEQNUM)
            Else
                OUT = FORM_GOTO_RECORD_BY_SeqNum(DATA_GET_CURRENT_SEQNUM() - 1)
            End If
        End If

        FORM_GOTO_PREVIOUSRECORD = OUT
    End Function

    Public Sub FORM_DORESYNC_CURRENT_AND_GRID()
        'Lefrissiti a current record-ot es a mogotte levo grid-et.

        Dim Current_ORD_L_ID = P_DATA_Current_ID

        If GRID_EXISTS() Then
            GRID.REFRESH()
            FORM_GOTO_RECORD_BY_ID(Current_ORD_L_ID)
        End If
        FORM_DORESYNC(, , True)
    End Sub

    Public Sub FORM_DORESYNC(Optional ByVal AllRecords As Boolean = False, Optional ByVal mitDialogYesNo As Boolean = False, Optional ByVal NoUpdateChildren As Boolean = False)
        If Disposed = False Then
            Dim w As Integer

            If AllRecords And NoUpdateChildren Then
                FPf.FPApp.DoErrorMsgBox("FP.FORM_DORESYNC", 0, "Wrong parameters. AllRecords=True and NoUpdateChildren=True are NOT ALLOWED.")
            Else
                If P_DATA_Current_ID <> 0 Or AllRecords Then
                    If AllRecords Then
                        If P_DATA_Dirty Then
                            FPf.UNDO_ALL()
                        End If

                        If DATA_RS_SET(RS_WHERE) Then
                            If Not FORM_RECORDS_LOAD_FROM_EXISTING_RS(True) Then
                                FPf.FPApp.DoErrorMsgBox("FP.FORM_DORESYNC", 0, "RESYNC was NOT successfull.")
                            End If
                        End If
                    Else
                        If mitDialogYesNo Then
                            w = FPf.FPApp.DoMyMsgBox(27, , "SEQ,NEIN", "SEQ,JA") 'Valoban frissitsem az adatsort? Nem|Igen
                            If w <> 2 Then
                                Exit Sub
                            End If
                        End If

                        If P_DATA_Dirty Then
                            UNDO()
                        End If

                        If GRID_EXISTS() Then
                            GRID.ROW_CURRENT_REFRESH(False)
                        End If
                        FORM_RECORDS_LOAD_CURRENT(NoUpdateChildren)
                    End If
                End If
            End If
        End If
    End Sub

    Public Function DATA_IsDataField(ByVal FieldName As String) As Boolean
        Dim OUT As Boolean = False

        If Not UnboundForm Then

            If Not (DATA_DT_FORM_SQLFIELDS_FOR_WRITE Is Nothing) Then
                With DATA_DT_FORM_SQLFIELDS_FOR_WRITE
                    If .Rows.Count > 0 Then
                        Dim Criteria = String.Format("FieldName = '{0}'", FieldName)
                        OUT = .Select(Criteria).Count = 1
                    End If
                End With
            End If
        End If

        DATA_IsDataField = OUT
    End Function
    Public Function DATA_Field_setValue(ByVal FieldName As String, ByVal Value As Object) As Boolean
        Dim OUT As Boolean = False

        If P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_setValue", 0, "There is no record on the form!")
        Else
            If DATA_LoadedRecord_ID <> P_DATA_Current_ID Then
                FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_setValue", 0, "Data not loaded.")
            Else
                If DATA_DT_FORM_SQLFIELDS_FOR_WRITE Is Nothing Then
                    FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_setValue", 0, "DATA_DT_FORM_SQLFIELDS_FOR_WRITE is nothing.")
                Else
                    Dim Criteria As String = String.Format("FieldName = '{0}'", FieldName)

                    If DATA_DT_FORM_SQLFIELDS_FOR_WRITE.Select(Criteria).Count < 1 Then
                        FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_setValue", 0, String.Format("FP.DATA_Field_setValue: Field {0} not found", FieldName))
                    Else
                        Dim AktRow As DataRow = DATA_DT_FORM_SQLFIELDS_FOR_WRITE.Select(Criteria).First

                        With AktRow
                            .BeginEdit()
                            !Value = DBFORMAT_from_OBJECT(Value, !FieldName, !xtype_VB)
                            .EndEdit()
                        End With
                        OUT = True
                    End If
                End If
            End If
        End If

        DATA_Field_setValue = OUT
    End Function
    Public Function DATA_Field_getProps(ByVal FieldName As String) As Struct_DATA_Field_Props
        Dim OUT As Struct_DATA_Field_Props = Nothing
        Dim Criteria = String.Format("FieldName = '{0}'", FieldName)
        Dim Found As Boolean = False

        If Not Found Then
            If Not (DATA_DT_FORM_SQLFIELDS_FOR_WRITE Is Nothing) Then
                If DATA_DT_FORM_SQLFIELDS_FOR_WRITE.Rows.Count > 0 Then
                    If DATA_DT_FORM_SQLFIELDS_FOR_WRITE.Select(Criteria).Count = 1 Then
                        With DATA_DT_FORM_SQLFIELDS_FOR_WRITE.Select(Criteria)
                            OUT.SeqNum = .First!SeqNum
                            OUT.xtype = .First!xtype
                            OUT.xtype_VB = .First!xtype_VB
                            OUT.xLength = .First!xLength
                            OUT.Value = .First!Value
                        End With
                        Found = True
                    End If
                End If
            End If
        End If

        If Not Found Then
            If Not (DATA_DT_FORM_SQLFIELDS_FOR_READ Is Nothing) Then
                If DATA_DT_FORM_SQLFIELDS_FOR_READ.Rows.Count > 0 Then
                    If DATA_DT_FORM_SQLFIELDS_FOR_READ.Select(Criteria).Count = 1 Then
                        With DATA_DT_FORM_SQLFIELDS_FOR_READ.Select(Criteria)
                            OUT.SeqNum = .First!SeqNum
                            OUT.xtype = .First!xtype
                            OUT.xtype_VB = .First!xtype_VB
                            OUT.xLength = .First!xLength
                            OUT.Value = .First!Value
                        End With
                        Found = True
                    End If
                End If
            End If
        End If

        DATA_Field_getProps = OUT
    End Function
    Public Function DATA_Field_getValue(ByVal FieldName As String) As Object
        Dim OUT As Object = CType("", String)

        If DATA_DT_FORM_SQLFIELDS_FOR_WRITE Is Nothing Then
            FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getValue", 0, "DATA_DT_FORM_SQLFIELDS_FOR_WRITE is nothing.")
        Else
            If P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getValue", 0, "RecordStatus is NORECORD.")
            Else
                If (Not P_DATA_NewRecord) And DATA_LoadedRecord_ID = 0 Then
                    FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getValue", 0, "Data not loaded.")
                Else
                    If DATA_LoadedRecord_ID <> P_DATA_Current_ID Then
                        FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getValue", 0, "Internal error.")
                    Else
                        FieldName = Trim(FieldName)
                        If Mid(FieldName, 1, 1) = "@" Then
                            FieldName = Mid(FieldName, 2)
                        End If

                        Dim Criteria = String.Format("FieldName = '{0}'", FieldName)
                        With DATA_DT_FORM_SQLFIELDS_FOR_WRITE
                            If .Select(Criteria).Count < 1 Then
                                FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getValue", 0, String.Format("Field  '{0}'  not found", FieldName))
                            Else
                                Dim AktRow As DataRow = .Select(String.Format("FieldName = '{0}'", FieldName)).First

                                With AktRow
                                    'OUT = OBJECT_from_DBFORMAT(AktRow!Value, AktRow!FieldName, AktRow!xtype_VB)
                                    OUT = AktRow!Value
                                End With
                            End If
                        End With
                    End If
                End If
            End If
        End If

        DATA_Field_getValue = OUT
    End Function

    Public Function DATA_Field_getValue_FromREAD(ByVal FieldName As String) As Object
        Dim OUT As Object = CType("", String)

        If DATA_DT_FORM_SQLFIELDS_FOR_READ Is Nothing Then
            FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getValue_FromREAD", 0, "DATA_DT_FORM_SQLFIELDS_FOR_READ is nothing.")
        Else
            If P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getValue_FromREAD", 0, "RecordStatus is NORECORD.")
            Else
                If (Not P_DATA_NewRecord) And DATA_LoadedRecord_ID = 0 Then
                    FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getValue_FromREAD", 0, "Data not loaded.")
                Else
                    If DATA_LoadedRecord_ID <> P_DATA_Current_ID Then
                        FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getValue_FromREAD", 0, "Internal error.")
                    Else
                        FieldName = Trim(FieldName)
                        If Mid(FieldName, 1, 1) = "@" Then
                            FieldName = Mid(FieldName, 2)
                        End If

                        Dim Criteria = String.Format("FieldName = '{0}'", FieldName)
                        With DATA_DT_FORM_SQLFIELDS_FOR_READ
                            If .Select(Criteria).Count < 1 Then
                                FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getValue_FromREAD", 0, String.Format("Field  '{0}'  not found", FieldName))
                            Else
                                Dim AktRow As DataRow = .Select(String.Format("FieldName = '{0}'", FieldName)).First

                                With AktRow
                                    OUT = AktRow!Value
                                End With
                            End If
                        End With
                    End If
                End If
            End If
        End If

        DATA_Field_getValue_FromREAD = OUT
    End Function
    Public Function DATA_Field_Exists(FieldName As String) As Boolean
        Dim OUT As Boolean = False

        If Not (DATA_DT_FORM_SQLFIELDS_FOR_READ Is Nothing) Then
            FieldName = Trim(FieldName)
            If Mid(FieldName, 1, 1) = "@" Then
                FieldName = Mid(FieldName, 2)
            End If

            Dim Criteria = String.Format("FieldName = '{0}'", FieldName)
            OUT = (DATA_DT_FORM_SQLFIELDS_FOR_READ.Select(Criteria).Count = 1)
        End If

        Return OUT
    End Function
    Public Function DATA_Field_getSavedValue(ByVal FieldName As String) As String
        Dim OUT As Object = CType("", String)

        If DATA_DT_FORM_SQLFIELDS_FOR_READ Is Nothing Then
            FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getSavedValue", 0, "DATA_DT_FORM_SQLFIELDS_FOR_READ is nothing.")
        Else
            If DATA_DT_FORM_SQLFIELDS_FOR_WRITE Is Nothing Then
                FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getSavedValue", 0, "DATA_DT_FORM_SQLFIELDS_FOR_WRITE is nothing.")
            Else
                If P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                    FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getSavedValue", 0, "RecordStatus is NORECORD.")
                Else
                    If Not P_DATA_NewRecord And DATA_LoadedRecord_ID = 0 Then
                        FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getSavedValue", 0, "Data not loaded.")
                    Else
                        If DATA_LoadedRecord_ID <> P_DATA_Current_ID Then
                            FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getSavedValue", 0, "Internal error.")
                        Else
                            FieldName = Trim(FieldName)
                            If Mid(FieldName, 1, 1) = "@" Then
                                FieldName = Mid(FieldName, 2)
                            End If

                            Dim Criteria = String.Format("FieldName = '{0}'", FieldName)
                            With DATA_DT_FORM_SQLFIELDS_FOR_READ
                                If .Select(Criteria).Count < 1 Then
                                    FPf.FPApp.DoErrorMsgBox("FP.DATA_Field_getSavedValue", 0, String.Format("Field {0} not found", FieldName))
                                Else
                                    Dim AktRow As DataRow = .Select(String.Format("FieldName = '{0}'", FieldName)).First

                                    OUT = AktRow!Value
                                End If
                            End With
                        End If
                    End If
                End If
            End If
        End If

        Return OUT
    End Function

    Public Function FORM_CHK_FIELDS() As Boolean
        Dim OUT As Boolean = True
        Dim AktKey As String
        Dim Wrong_c As Control = Nothing

        If UnboundForm Or P_FORM_Dirty Then
            If FPc_HAS_FIELD(FPf.ActiveControl) Then
                If FPf.ActiveControl.FP.Equals(Me) Then
                    If Not FPf.ActiveControl_VALIDATE Then
                        OUT = False
                        FPf.FOCUS_ON_AT_THE_END(FPf.ActiveControl.c, 10, , True)
                    End If
                End If
            End If

            If OUT Then
                For Each AktKey In CONTROLS.Keys
                    With CONTROLS(AktKey)
                        If .P.Mandatory Then
                            If TypeOf (.c) Is System.Windows.Forms.TextBox Then
                                If Trim(.c.Text) = "" Then
                                    Wrong_c = .c
                                    OUT = False
                                    Exit For
                                ElseIf .F_Format_NoShow0 And Trim(.c.Text) = "0" Then
                                    Wrong_c = .c
                                    OUT = False
                                    Exit For

                                ElseIf TypeOf (.c) Is System.Windows.Forms.NumericUpDown Then   'ML_20160519
                                    If CType(.c, System.Windows.Forms.NumericUpDown).Value = 0 Then
                                        Wrong_c = .c
                                        OUT = False
                                        Exit For
                                    End If
                                End If

                            ElseIf TypeOf (.c) Is System.Windows.Forms.ComboBox Then
                                'If CType(.c, System.Windows.Forms.ComboBox).SelectedValue = 0 Then
                                If .P_VALUE = 0 Then
                                    Wrong_c = .c
                                    OUT = False
                                    Exit For
                                End If

                            ElseIf TypeOf (.c) Is System.Windows.Forms.CheckBox Then
                                'Nothing to do

                            ElseIf TypeOf (.c) Is ListView Then
                                If (CType(.c, ListView).FocusedItem Is Nothing) Then
                                    Wrong_c = .c
                                    OUT = False
                                    Exit For
                                End If
                            ElseIf TypeOf (.c) Is MSDN.Html.Editor.HtmlEditorControl Then
                                If CType(.c, MSDN.Html.Editor.HtmlEditorControl).InnerText = "" Then
                                    Wrong_c = .c
                                    OUT = False
                                    Exit For
                                End If
                            Else
                                FPf.FPApp.DoErrorMsgBox("FP.FORM_CHK_FIELDS", 0, String.Format("Field '{0}' Unknown Controltype", .FieldName))
                                Wrong_c = .c
                                OUT = False
                                Exit For
                            End If
                        End If

                        If Not .P_VALUE_VALIDATED Then
                            Dim ee As New System.ComponentModel.CancelEventArgs

                            .EVENT_VALIDATING(CONTROLS(AktKey).c, ee)

                            If ee.Cancel Then
                                Wrong_c = .c
                                OUT = False
                                Exit For
                            End If
                        End If
                    End With
                Next

                If Not OUT Then
                    FPf.FOCUS_ON_AT_THE_END(Wrong_c, 7)
                End If
            End If
        End If

        FORM_CHK_FIELDS = OUT
    End Function

    Public Function FORM_DIRTY_SET(Optional FieldName As String = "") As Boolean
        Dim OUT As Boolean = True
        Dim HasRight As Boolean

        If Not Dirty Then
            OUT = FPf.SAVE_ALL

            If OUT = True Then
                If FORM_IsSubForm() Then
                    If Not (Parent_FP Is Nothing) Then
                        If Parent_FP.P_DATA_RecordStatus <> ENUM_RecordStatus.EXISTS Then
                            OUT = False
                        End If
                    End If
                End If
            End If

            If OUT = True Then
                If P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                    OUT = False
                Else
                    If Not P_FORM_AllowEdits Then
                        OUT = False
                    Else
                        If P_DATA_RecordStatus = ENUM_RecordStatus.NEWRECORD And P_FORM_AllowAdditions = False Then
                            OUT = False
                        Else
                            If Not UnboundForm Then
                                If P_DATA_NewRecord Then
                                    HasRight = FORM_GETRIGHT("FORM_INSERT", FieldName)
                                Else
                                    HasRight = FORM_GETRIGHT("FORM_UPDATE", FieldName)
                                End If

                                If Not HasRight Then
                                    OUT = False
                                Else
                                    Dim Cancel As Integer = False

                                    RaiseEvent Form_Dirty(Me, Cancel)
                                    If Cancel Then
                                        Select Case P_DATA_RecordStatus
                                            Case ENUM_RecordStatus.NORECORD
                                                FORM_CLEAR_SQLFIELDS()

                                            Case ENUM_RecordStatus.EXISTS
                                                FORM_DORESYNC(, , True)

                                            Case ENUM_RecordStatus.NEWRECORD
                                                FORM_CLEAR_SQLFIELDS()
                                                FORM_GOTO_NEWRECORD()
                                        End Select

                                        OUT = False
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            If OUT = True Then
                Dirty = True

                If GRID_EXISTS() Then
                    GRID.DIRTY_SET()
                End If

                Dim Data_Binded_OLD As Boolean = DATA_Binded
                DATA_Binded = False
                RaiseEvent Form_BeginEdit(Me)
                DATA_Binded = Data_Binded_OLD
            End If
        End If

        FORM_DIRTY_SET = OUT
    End Function
    Public Function FORM_RECORDS_SAVE_CURRENT() As Boolean
        Dim OUT As Boolean = False
        Dim ErrField As String = String.Empty

        If P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            OUT = True
        Else
            'If P_DATA_Dirty = False Then
            If Dirty = False Then
                OUT = True
            Else
                Dim Cancel As Integer = False

                If Not (FPf.P_ActiveControl Is Nothing) Then
                    If FPf.P_ActiveControl.c.Focused Then
                        If FPf.P_ActiveControl.FP.Equals(Me) Then
                            If FPf.P_ActiveControl.FIELD_IsDirty Then
                                Cancel = (Not FPf.ActiveControl_VALIDATE)
                                'If Not Cancel Then
                                '    RAISEEVENT_Form_Field_AfterUpdate(FPf.P_ActiveControl)
                                'End If
                            End If
                        End If
                    End If
                End If

                If Not Cancel Then
                    Dim Data_Binded_OLD = DATA_Binded
                    DATA_Binded = False
                    RaiseEvent Form_BeforeUpdate(Me, Cancel)
                    DATA_Binded = Data_Binded_OLD
                    If Cancel = 0 Then
                        FORM_WRITE_FIELDS_TO_DATA()
                        If FORM_CHK_FIELDS() Then
                            If DATA_RECORDS_SAVE_CURRENT(ErrField) Then
                                Dim Record_Data_Allready_loaded As Boolean = False

                                If GRID_EXISTS() Then
                                    If SQL_BIND_Params.NameOf_READ <> SQL_BIND_Params.NameOf_GRID Then
                                        Dim Current_RecordID As Long = P_DATA_Current_ID
                                        Dim Current_FirstDisplayedColumnIndex As Integer = GRID.GRID.FirstDisplayedScrollingColumnIndex
                                        Dim Current_FirstDisplayedRowIndex As Integer = GRID.GRID.FirstDisplayedScrollingRowIndex
                                        Dim Current_ColumnIndex As Integer = -1

                                        If Not (GRID.GRID.CurrentCell Is Nothing) Then
                                            Current_ColumnIndex = GRID.GRID.CurrentCell.ColumnIndex
                                        End If

                                        DATA_RS_SET(RS_WHERE)
                                        DATA_RECORDS_LOAD_FROM_RS(True)
                                        GRID.FILL()
                                        DATA_GOTO_RECORD_BY_ID(Current_RecordID)
                                        Record_Data_Allready_loaded = DATA_RECORDS_LOAD_CURRENT()

                                        DATA_NoRecord = False

                                        If Current_ColumnIndex > -1 Then
                                            If Not (GRID.GRID.CurrentCell Is Nothing) Then
                                                GRID.GRID.CurrentCell = GRID.GRID(Current_ColumnIndex, GRID.GRID.CurrentCell.RowIndex)
                                            End If
                                        End If
                                        GRID.GOTO_ROW_BY_RECORDID(Current_RecordID, Current_FirstDisplayedColumnIndex, , Current_FirstDisplayedRowIndex)
                                    End If
                                End If

                                FORM_RECORDS_LOAD_CURRENT(, Record_Data_Allready_loaded)
                                RAISEEVENT_Form_AfterUpdate()

                                OUT = True
                            Else
                                If ErrField = "" Then
                                    Dim First_FPc As FP_Control = CONTROLS_GET_FIRST_FPc()
                                    If Not (First_FPc Is Nothing) Then
                                        FPf.FOCUS_ON_AT_THE_END(First_FPc.c)
                                    End If
                                Else
                                    FORM_FOCUS_ON_AT_THE_END(ErrField)
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        Return OUT
    End Function
    Public Sub UNDO()
        If P_DATA_Dirty Or Dirty Then
            FORM_BIND_DATA()
            FORM_DIRTY_CLEAR()
            FORM_RECORDS_LOAD_CURRENT()
        End If
    End Sub

    Public Function FORM_FOCUS_ON_AT_THE_END(ByVal MyFieldName As String) As Boolean
        Dim OUT As Boolean = False

        If CONTROLS.ContainsKey(MyFieldName) Then
            FPf.FOCUS_ON_AT_THE_END(CONTROLS(MyFieldName).c)
        End If

        FORM_FOCUS_ON_AT_THE_END = OUT
    End Function

    Public Sub COLORING_ALL()
        Dim AktKey As String

        For Each AktKey In CONTROLS.Keys
            CONTROLS(AktKey).COLORING()
        Next
    End Sub
    Public Sub CONTROLS_REFRESH_DT_ALL()
        For Each AktKey In CONTROLS.Keys
            With CONTROLS(AktKey)
                If Not (.c Is Nothing) Then
                    If TypeOf (.c) Is ComboBox Or TypeOf (.c) Is ListView Then
                        CONTROLS(AktKey).DT_REFRESH()
                    End If
                End If
            End With
        Next
    End Sub
    Public Sub CONTROLS_REFRESH_DT_ALL(For_REFRESH_TYPE As ENUM_FP_CONTROL_REFRESH_TYPE)
        For Each AktKey In CONTROLS.Keys
            With CONTROLS(AktKey)
                If Not (.c Is Nothing) Then
                    If TypeOf (.c) Is ComboBox Or TypeOf (.c) Is ListView Then
                        If .Refresh_Type = For_REFRESH_TYPE Then
                            CONTROLS(AktKey).DT_REFRESH()
                        End If
                    End If
                End If
            End With
        Next
    End Sub
    Public Sub CONTROLS_REFRESH_DT_FixText_Key(MyDT_FixText_Key As String)
        For Each AktKey In CONTROLS.Keys
            With CONTROLS(AktKey)
                If Not (.c Is Nothing) Then
                    If TypeOf (.c) Is ComboBox Or TypeOf (.c) Is ListView Then
                        If .P.DT_FixText_Key = MyDT_FixText_Key Then
                            CONTROLS(AktKey).DT_REFRESH()
                        End If
                    End If
                End If
            End With
        Next
    End Sub
    Public Sub VISIBLE_ALL(ByVal MyVisible As Boolean)
        Dim AktKey As String

        For Each AktKey In CONTROLS.Keys
            With CONTROLS(AktKey)
                If .CreatedBy <> ENUM_FP_CONTROL_Created_by.GRID Then
                    .P_VISIBLE = MyVisible
                End If
            End With
        Next

        For Each AktKey In PICTUREBOXES.Keys
            With PICTUREBOXES(AktKey)
                If .c IsNot Nothing Then
                    If .CreatedBy <> ENUM_FP_CONTROL_Created_by.GRID Then
                        .c.Visible = MyVisible
                    End If
                End If
            End With
        Next

        If GRID_EXISTS() Then
            GRID.P_VISIBLE = MyVisible
        End If
    End Sub

    Public Function FORM_RECORD_UPDOWN(ByVal MyUPDOWN As ENUM_UpDown) As Boolean
        Dim OUT As Boolean = False

        If FORMDEF_CHK_PARAM("UNIQUETABLE_SEQNUM") And FORMDEF_CHK_PARAM("UNIQUETABLE") Then
            If FORM_RECORDS_SAVE_CURRENT() Then
                If FORM_GETRIGHT("FORM_UPDATE") Then
                    Dim Current_ID = P_DATA_Current_ID

                    If Current_ID <> 0 Then
                        If DATA_RECORD_UPDOWN(MyUPDOWN) Then
                            FORM_RECORDS_LOAD(FORM_SubWHERE)
                            FORM_GOTO_RECORD_BY_ID(Current_ID)

                            OUT = True
                        End If
                    End If
                End If
            End If
        End If

        FORM_RECORD_UPDOWN = OUT
    End Function

#Region "PROTECTED"
    Protected Friend DATA_DT_FORM_SQLFIELDS_FOR_GRID As New DataTable

    Protected Friend DATA_Binded As Boolean = True

    Protected Friend Function CONTROLS_FROM_RS_SET() As Boolean
        Dim OUT As Boolean = False
        Dim DT As New DataTable
        Dim Row As DataRow

        RAISEEVENT_CONTROLS_INITIALIZING()

        If FPf.FPApp.RS_FILL_FIELD_PARAMETER_TABLE(ServerObject_Prefix, SubPrefix, DT) Then
            For Each Row In DT.Rows

                Dim c As Control = Nothing
                Dim c_Label As Control = Nothing
                Dim c_OK As Boolean = False
                Dim c_Label_OK As Boolean = False

                If Row!FieldName = "#FORM#" Then
                    FPf.CONTROLS_FROM_RS_SET_for_FORM(Row)
                Else
                    If Row!CreateAtRuntime Then
                        Dim Props As Struct_CONTROL_PROPS = Nothing

                        With Props
                            .Type = Row!CreateAtRuntime_FieldType
                            .Name = Row!FieldName
                            .ClientRectangle = New Rectangle(0, 0, 100, FPf.P_Layout_TextBox_NormalHeight)
                            .Parent = Row!Parent
                            .Label_Name = Row!LabelName
                            .Label_Clientrechtangle = New Rectangle(0, 0, 100, FPf.P_Layout_TextBox_NormalHeight)
                        End With
                        c_OK = CONTROLS_CREATE(Props, c, c_Label)
                        c_Label_OK = c_OK
                    Else
                        If Not FPf.CONTROLS.ContainsKey(FieldPrefix + Row!FieldName) Then
                            FPf.FPApp.DoErrorMsgBox("FP.CONTROLS_FROM_RS_SET", 0, String.Format("Unknown Field '{0}' (ServerObject_Prefix: '{1}', SubPrefix = '{2}', SeqNum = '{3}')", FieldPrefix + Row!FieldName, ServerObject_Prefix, SubPrefix, Row!SeqNum))
                        Else
                            c = FPf.CONTROLS(FieldPrefix + Row!FieldName)
                            c_OK = True
                            If Trim(Row!LabelName) = "" Then
                                c_Label_OK = True
                            Else
                                If Not FPf.CONTROLS.ContainsKey(FieldPrefix + Row!LabelName) Then
                                    FPf.FPApp.DoErrorMsgBox("FP.CONTROLS_FROM_RS_SET", 0, String.Format("Unknown Label '{0}'", FieldPrefix + Row!LabelName))
                                Else
                                    c_Label = FPf.CONTROLS(FieldPrefix + Row!LabelName)
                                    c_Label_OK = True
                                End If
                            End If
                        End If
                    End If

                    If c_OK And c_Label_OK Then
                        If TypeOf (c) Is PictureBox Then
                            Dim DoIt As Boolean = True

                            If PICTUREBOXES.ContainsKey(Row!FieldName) Then
                                With PICTUREBOXES(Row!FieldName)
                                    If .ParametersFrom_SubPrefix = Row!SubPrefix Then
                                        DoIt = False
                                        FPf.FPApp.DoErrorMsgBox("FP.CONTROLS_FROM_RS_SET", 0, String.Format("Picturebox '{0}' is already exists.", Row!FieldName))
                                    Else
                                        PICTUREBOXES_REMOVE(Row!FieldName)
                                    End If
                                End With
                            End If

                            If DoIt Then
                                Dim FPp As New FP_PictureBox(FPf, Me, c, Row!BG_Toggle, Row!BG_Image)
                                With FPp
                                    .FieldName = FieldPrefix + Row!FieldName
                                    .CreatedBy = ENUM_FP_CONTROL_Created_by.RS
                                    .P_LOCKED = Row!Locked
                                    .ParametersFrom_SubPrefix = Row!SubPrefix
                                    .P_VISIBLE = Row!Visible
                                    .CreatedAtRuntime = Row!CreateAtRuntime
                                End With
                                PICTUREBOXES_ADD(FPp)
                            End If
                        Else
                            Dim DoIt As Boolean = True

                            If CONTROLS.ContainsKey(FieldPrefix + Row!FieldName) Then
                                With CONTROLS(FieldPrefix + Row!FieldName)
                                    If .ParametersFrom_SubPrefix = Row!SubPrefix Then
                                        DoIt = False
                                        FPf.FPApp.DoErrorMsgBox("FP.CONTROLS_FROM_RS_SET", 0, String.Format("Field '{0}' is already exists.", FieldPrefix + Row!FieldName))
                                    Else
                                        CONTROLS_REMOVE(FieldPrefix + Row!FieldName)
                                    End If
                                End With
                            End If

                            If DoIt Then
                                Dim F As New FP_Control(Me, c, c_Label, Row!CreateAtRuntime, Row!FieldName, Row!LabelName) With {
                                    .ParametersFrom_SubPrefix = Row!SubPrefix,
                                    .CreatedBy = ENUM_FP_CONTROL_Created_by.RS
                                }

                                F.CreatedBy = ENUM_FP_CONTROL_Created_by.RS

                                Dim Color_Defs() As String = Split(Row!COLORS, "|")
                                Dim BG_COLOR As Color = Nothing
                                Dim FORE_COLOR As Color = Nothing
                                Dim COLOR_SELECTED_FORE As Color = Nothing
                                Dim LABEL_FORECOLOR As Color = Nothing
                                Dim LABEL_BGCOLOR As Color = Nothing

                                Dim Props As New Struct_FP_CONTROL_PROPS

                                If UBound(Color_Defs) < 4 Then
                                    ReDim Preserve Color_Defs(4)
                                End If

                                If TypeOf (F.c) Is Label Then
                                    COLOR_GET_FROM_STR(Color_Defs(0), COLORS_LABEL_BG, c.Name, BG_COLOR)
                                    COLOR_GET_FROM_STR(Color_Defs(1), COLORS_LABEL_FORE, c.Name, FORE_COLOR)
                                Else
                                    COLOR_GET_FROM_STR(Color_Defs(0), COLORS_FIELD_NORMAL_BG, c.Name, BG_COLOR)
                                    COLOR_GET_FROM_STR(Color_Defs(1), COLORS_FIELD_NORMAL_FORE, c.Name, FORE_COLOR)
                                    If TypeOf (F.c) Is Button Then
                                        If Color_Defs(0) > "" Then
                                            F.c.BackColor = BG_COLOR
                                        End If
                                        If Color_Defs(1) > "" Then
                                            F.c.ForeColor = FORE_COLOR
                                        End If
                                    End If
                                End If
                                COLOR_GET_FROM_STR(Color_Defs(2), COLORS_FIELD_SELECTED_FORE, c.Name, COLOR_SELECTED_FORE)
                                COLOR_GET_FROM_STR(Color_Defs(3), COLORS_LABEL_FORE, "Label for " + c.Name, LABEL_FORECOLOR)
                                COLOR_GET_FROM_STR(Color_Defs(4), COLORS_LABEL_BG, "Label for " + c.Name, LABEL_BGCOLOR)
                                With Row
                                    Props.Visible = !Visible
                                    Props.Mandatory = !Mandatory
                                    Props.Locked = !Locked
                                    Props.xType_VB = !xType_VB
                                    Props.DT_FixText_Key = !DT_FixText_Key
                                    Props.DT_WHERE2 = !DT_WHERE2
                                    Props.DT_ID_Field = !DT_ID_Field
                                    Props.F_Format = !F_Format
                                    Props.COLOR_NORMAL_BG = BG_COLOR
                                    Props.COLOR_NORMAL_FORE = FORE_COLOR
                                    Props.COLOR_SELECTED_FORE = COLOR_SELECTED_FORE
                                    Props.COLOR_LABEL_FORE = LABEL_FORECOLOR
                                    Props.COLOR_LABEL_BG = LABEL_BGCOLOR
                                    Props.BG_Image_Name = !BG_Image
                                    Props.Label_Text = !Label_Text
                                    Props.ShowInGRID = !ShowInGRID
                                    Props.SavePoint = !SavePoint
                                    Props.Forced_NextField = !Forced_NextField
                                    Props.Tag = !Tag

                                    F.SET_PARAMS(Props)
                                    F.SET_SELECT_Control(Props)
                                    If !TabIndex > 0 Then
                                        F.P_TABSTOP = !TabStop
                                        c.TabIndex = !TabIndex
                                    End If

                                    F.P_VISIBLE = !Visible
                                End With
                            End If
                        End If
                    End If
                End If
            Next
        End If

        CONTROLS_FROM_RS_SET = OUT
    End Function

    'Protected Friend Sub RAISEEVENT_Form_ButtonFind_RECORD_SELECTED(ByVal Out_Params As Struct_Simple_SELECT_OutputParams)
    '    RaiseEvent Form_ButtonFind_RECORD_SELECTED(Me, Out_Params)
    'End Sub

    Protected Friend Sub RAISEEVENT_Form_NoRecord()
        DATA_LOGS_ACTIVITY_CLEAR()
        RaiseEvent Form_NoRecord(Me)
    End Sub

    Protected Friend Sub RAISEEVENT_Form_Current()
        CONTROLS_REFRESH_DT_ALL(ENUM_FP_CONTROL_REFRESH_TYPE.On_Form_Current)

        If P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            DATA_LOGS_ACTIVITY_SET()
            FP_LAYOUT_SET()
            RaiseEvent Form_Current(Me)
        End If
    End Sub

    Protected Friend Sub RAISEEVENT_Form_AfterUpdate()
        CONTROLS_REFRESH_DT_ALL(ENUM_FP_CONTROL_REFRESH_TYPE.On_Form_AfterUpdate)
        If P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            RaiseEvent Form_AfterUpdate(Me)
        End If
    End Sub

    Protected Friend Sub RAISEEVENT_Form_AfterDelete()
        CONTROLS_REFRESH_DT_ALL(ENUM_FP_CONTROL_REFRESH_TYPE.On_Form_AfterUpdate)
        RaiseEvent Form_AfterDelete(Me)
    End Sub

    Protected Friend Sub RAISEEVENT_Form_Field_Coloring(ByVal sender_FPc As FP_Control, ByRef Handled As Boolean)
        RaiseEvent Form_Field_Coloring(sender_FPc, Handled)
    End Sub

    Private Sub FP_LAYOUT_SET()
        If FPf.FPApp.UserRights.ExcelExport = False Then
            If Not (ButtonExportToExcel Is Nothing) Then
                If ButtonExportToExcel.Visible Then
                    ButtonExportToExcel.Visible = False
                End If
            End If

            If Not (ButtonImportFromExcel Is Nothing) Then
                If ButtonImportFromExcel.Visible Then
                    ButtonImportFromExcel.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Friend Sub RAISEEVENT_CONTROLS_INITIALIZING()
        RaiseEvent CONTROLS_INITIALIZING(Me)
    End Sub

    Protected Friend Sub RAISEEVENT_CONTROLS_INITIALIZED()
        RaiseEvent CONTROLS_INITIALIZED(Me)
    End Sub

    Protected Friend Sub RAISEVENT_CONTROLS_INITIALIZED_AND_ARRANGED()
        RaiseEvent CONTROLS_INITIALIZED_AND_ARRANGED(Me)
    End Sub

    Protected Friend Sub RAISEEVENT_Form_Field_Enter(ByVal FPc As FP_Control, ByRef Handled As Boolean)
        Dim DoIt As Boolean = True

        If GRID_EXISTS() Then
            DoIt = Not FPc.IS_GRID_FILTERFIELD
        End If

        If DoIt Then
            If P_DATA_Binded_ByUser Then
                RaiseEvent Form_Field_Enter(FPc, Handled)
            End If
        End If
    End Sub
    Protected Friend Sub RAISEEVENT_Form_Field_AfterUpdate(ByVal FPc As FP_Control)
        Dim DoIt As Boolean = True

        If GRID_EXISTS() Then
            DoIt = (Not FPc.IS_GRID_FILTERFIELD)
        End If

        If DoIt Then
            If P_DATA_Binded_ByUser Then
                RaiseEvent Form_Field_AfterUpdate(FPc)
            End If
        End If
    End Sub
    Protected Friend Sub RAISEEVENT_Form_KeyPreview_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        RaiseEvent Form_KeyPreview_KeyDown(Me, sender, e)
    End Sub
    Protected Friend Sub RAISEEVENT_Form_KeyPreview_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        RaiseEvent Form_KeyPreview_KeyPress(Me, sender, e)
    End Sub
    Protected Friend Sub RAISEEVENT_GRID_CellClick(ByVal FPc As FP_Control, ByRef Handled As Boolean)
        RaiseEvent GRID_CellClick(Me, FPc, Handled)
    End Sub
    Protected Friend Sub RAISEEVENT_GRID_Row_Drag_Begin(ByRef DATA As String, ByVal e As System.Windows.Forms.MouseEventArgs, ByRef Cancel As Boolean)
        RaiseEvent GRID_Row_Drag_Begin(Me, DATA, e, Cancel)
    End Sub
    Protected Friend Sub RAISEEVENT_GRID_Row_DoubleClick(ByRef Handled As Boolean)
        RaiseEvent GRID_Row_DoubleClick(Me, Handled)
    End Sub
    Protected Friend Sub RAISEEVENT_GRID_Filter_Changed()
        RaiseEvent GRID_Filter_Changed(Me)
    End Sub
    Protected Friend Sub RAISEEVENT_GRID_Filter_Deactivated()
        RaiseEvent GRID_Filter_Deactivated(Me)
    End Sub
    Protected Friend Sub RAISEEVENT_GRID_Row_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs, ByRef Handled As Boolean)
        RaiseEvent GRID_Row_MouseWheel(Me, sender, e, Handled)
    End Sub
    Protected Friend Sub RAISEEVENT_GRID_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        RaiseEvent GRID_Paint(Me, sender, e)
    End Sub
    Protected Friend Sub RAISEEVENT_GRID_Sorted(ByVal e As System.EventArgs)
        RaiseEvent GRID_Sorted(Me, e)
    End Sub
    Protected Friend Sub RAISEEVENT_EXCEL_IMPORT_Data_Records_Prepared(ByRef Cancel As Boolean)
        RaiseEvent EXCEL_IMPORT_Data_Records_Prepared(Me, XLS_IMPORT, Cancel)
    End Sub
    Protected Friend Sub RAISEEVENT_EXCEL_IMPORT_Check_Data()
        RaiseEvent EXCEL_IMPORT_Check_Data(Me, XLS_IMPORT)
    End Sub
    Protected Friend Sub RAISEEVENT_EXCEL_IMPORT_Import_Data(ByRef Cancel As Boolean)
        RaiseEvent EXCEL_IMPORT_Import_Data(Me, XLS_IMPORT, Cancel)
    End Sub
    Protected Friend Sub FORM_DIRTY_CLEAR()
        Dirty = False

        If GRID_EXISTS() Then
            GRID.DIRTY_CLEAR()
        End If
    End Sub
    Public Function GRID_EXISTS() As Boolean
        Dim OUT As Boolean = False

        If Not (GRID Is Nothing) Then
            If GRID.GRID IsNot Nothing Then
                OUT = True
            End If
        End If
        GRID_EXISTS = OUT
    End Function

    Private Sub FORM_SAVE_AS_LastRecord()
        Dim DB_val As String = ""

        For Each AktKey As String In CONTROLS.Keys
            With CONTROLS(AktKey)

                If .GET_DBFORMAT_from_CONTROL(DB_val) Then
                    .LastRecord_Value = DB_val
                End If
            End With
        Next
    End Sub

    Protected Friend Function FORM_RECORDS_LOAD_CURRENT(Optional ByVal NoUpdateChildren As Boolean = False, Optional Record_Data_Allready_loaded As Boolean = False) As Boolean
        Dim OUT As Boolean = False

        If Not Record_Data_Allready_loaded Then
            Record_Data_Allready_loaded = DATA_RECORDS_LOAD_CURRENT()
        End If

        If Record_Data_Allready_loaded Then
            FORM_SAVE_AS_LastRecord()
            If FORM_BIND_DATA(NoUpdateChildren) Then
                OUT = True
            End If
        Else
            FORM_BIND_DATA(NoUpdateChildren)
        End If

        FORM_RECORDS_LOAD_CURRENT = OUT
    End Function
#End Region
#Region "PRIVATE"
    Private Disposed As Boolean = False
    Private DATA_Binded_ByUser As Boolean = True
    Private ARRANGE_DT As DataTable

    Private FORMDEF_DIC As New Dictionary(Of String, String)
    Private FORMDEF_PrintOption_DIC As New Dictionary(Of String, String)
    Private FORMDEF_SETTINGS_DIC As New Dictionary(Of String, String)

    Private Data_AllowAdditions As Boolean = True
    Private Data_AllowDeletions As Boolean = True
    Private Data_AllowEdits As Boolean = True
    Private RS_Obj_Name As String = ""
    Private RS_FROM As String = ""
    Private RS_GROUPBY As String = ""
    Private RS_ORDERBY As String = ""
    Private RS_WHERE As String = ""

    Private DATA_DT_FORM_SQLFIELDS_FOR_READ As New DataTable
    Private DATA_DT_FORM_SQLFIELDS_FOR_WRITE As New DataTable
    Private DATA_DT_FORM_SQLFIELDS_FOR_DEL As New DataTable
    Private DATA_DT_FORM_SQLFIELDS_FOR_WHEREQUERY As New DataTable

    Private DATA_DT_FORM_Current_ID As Long = 0
    Private Dirty As Boolean = False
    Private DATA_NoRecord As Boolean = True
    Private DATA_LoadedRecord_ID As Long = 0

    Private WithEvents BINDINGNAVIGATOR_FORM_BS As New System.Windows.Forms.BindingSource
    'Private WithEvents BINDINGNAVIGATOR_FORM_RS As New DataSet

    Public ReadOnly Property P_DATA_Binded() As Boolean
        Get
            P_DATA_Binded = (DATA_Binded And DATA_Binded_ByUser And gl_Data_Binded)
        End Get
    End Property

    Private ReadOnly Property P_Disposed As Boolean
        Get
            Dim OUT As Boolean = Disposed

            If Disposed = False Then
                If Not (FPf Is Nothing) Then
                    OUT = FPf.P_Disposed
                End If
            End If

            Return OUT
        End Get
    End Property

    Private ReadOnly Property P_DATA_Dirty() As Boolean
        Get
            Dim OUT As Boolean = False

            If P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
                If P_DATA_Binded Then
                    OUT = Dirty
                End If
            End If

            P_DATA_Dirty = OUT
        End Get
    End Property

    Private Sub INIT_DEFAULT_PARAMS()
        INIT_SQL_BIND_DEFAULTS()
    End Sub
    Private Sub INIT_SQL_BIND_DEFAULTS()
        With SQL_BIND_Params
            .RS_ServerObject_Prefix = ServerObject_Prefix
            .NameOf_FormDef = String.Format("FORMDEF_{0}", ServerObject_Prefix)
            .NameOf_WhereQuery = ServerObject_Prefix + "_WhereQuery"
            .NameOf_GRID = ""
            .NameOf_READ = ServerObject_Prefix + "_READ"
            .NameOf_SAVE = ServerObject_Prefix + "_SAVE"
            .NameOf_DEL = ServerObject_Prefix + "_DEL"
        End With
    End Sub

    Private Sub FORMDEF_SETTINGS_LOAD()
        Dim FixText_Key As String = SQL_BIND_Params.NameOf_FormDef

        Dim FixText_SETTINGS_Key As String = FixText_Key + "_SETTINGS"
        Dim FixText_SETTINGS As String = FPf.FPApp.getFixText(FixText_SETTINGS_Key)

        FPf.FPApp.FIXTEXT_SPLIT_PARAMS(FixText_SETTINGS, FORMDEF_SETTINGS_DIC)
    End Sub

    Private Sub FORMDEF_PRINTOPTION_LOAD()
        Dim FixText_Key As String = SQL_BIND_Params.NameOf_FormDef

        Dim FixText_PrintOption_Key As String = FixText_Key + "_PrintOptions"
        Dim FixText_PrintOption As String = FPf.FPApp.getFixText(FixText_PrintOption_Key)

        FPf.FPApp.FIXTEXT_SPLIT_PARAMS(FixText_PrintOption, FORMDEF_PrintOption_DIC)
    End Sub

    Private Function FORMDEF_LOAD() As Boolean
        Dim OUT As Boolean = True
        Dim FixText_Key As String = SQL_BIND_Params.NameOf_FormDef
        Dim FixText As String = FPf.FPApp.getFixText(FixText_Key)

        FORMDEF_SETTINGS_LOAD()
        FORMDEF_PRINTOPTION_LOAD()

        If FixText = "" Then
            OUT = False
            FPf.FPApp.DoErrorMsgBox("FP.FORMDEF_LOAD", 0, String.Format("FORMDEF not definied ({0})", FixText_Key))
        Else
            If FPf.FPApp.FIXTEXT_SPLIT_PARAMS(FixText, FORMDEF_DIC) Then
                OUT = (OUT And FORMDEF_CHK_PARAM("FILTERNAME", False))
                OUT = (OUT And FORMDEF_CHK_PARAM("ORDERBY", False))
                OUT = (OUT And FORMDEF_CHK_PARAM("RECORDSOURCE_WHERE", False))
                OUT = (OUT And FORMDEF_CHK_PARAM("RIGHT_STOREDPROC", False))
                OUT = (OUT And FORMDEF_CHK_PARAM("UNIQUETABLE", False))
                OUT = (OUT And FORMDEF_CHK_PARAM("UNIQUETABLE_SEQNUM", False))
            End If
        End If

        If OUT Then
            If Val(FORMDEF("MAXRECORDS")) > 0 Then
                MAXRECORDS = Val(FORMDEF("MAXRECORDS"))
            End If

            If FORMDEF("SQL_DEL") > "" Then
                SQL_BIND_Params.NameOf_DEL = FORMDEF("SQL_DEL")
            End If
            If FORMDEF("SQL_GRID") > "" Then
                SQL_BIND_Params.NameOf_GRID = FORMDEF("SQL_GRID")
            End If
            If FORMDEF("SQL_READ") > "" Then
                SQL_BIND_Params.NameOf_READ = FORMDEF("SQL_READ")
            End If
            If FORMDEF("SQL_SAVE") > "" Then
                SQL_BIND_Params.NameOf_SAVE = FORMDEF("SQL_SAVE")
            End If
            If FORMDEF("SQL_WHEREQUERY") > "" Then
                SQL_BIND_Params.NameOf_WhereQuery = FORMDEF("SQL_WHEREQUERY")
            End If
        End If

        FORMDEF_LOAD = OUT
    End Function

    Private Function FORMDEF_CHK_PARAM(ByVal ParamName As String, Optional ByVal MustHaveValue As Boolean = True) As Boolean
        FORMDEF_CHK_PARAM = FPf.FPApp.FIXTEXT_CHK_PARAM(FORMDEF_DIC, SQL_BIND_Params.NameOf_FormDef, ParamName, MustHaveValue, , , String.Format("{0}|{1}", SQL_BIND_Params.NameOf_FormDef, ParamName))
    End Function

    Protected Friend Sub ButtonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        If FPf.SAVE_ALL Then
            Dim ZDISPO_OpenArgs = FORMDEF("ZDISPO_OpenArgs")

            If Trim(ZDISPO_OpenArgs) > "" Then
                ZDISPO_OpenArgs = ZDISPO_OpenArgs

                Dim UniqueField As String = FORMDEF("UNIQUE_FIELD")
                Dim UniqueValue As String = ""
                Dim Params As New Struct_Simple_SELECT_Params
                Dim Out_Params As New Struct_Simple_SELECT_OutputParams

                If UniqueField > "" Then
                    If Not CONTROLS.ContainsKey(UniqueField) Then
                        FPf.FPApp.DoErrorMsgBox("FP.ButtonFind_Click", 0, String.Format("UNIQUE_FIELD '{0}' in FixText_Key '{1}' not found.", UniqueField, ZDISPO_OpenArgs))
                    Else
                        UniqueValue = CONTROLS(UniqueField).c.Text
                    End If
                End If

                With Params
                    .FixText_Key = ZDISPO_OpenArgs

                    .Selected_Text = UniqueValue
                    .Field_Mandatory = True
                    .SQL_WHERE = String.Format("RS_ID = {0}", RS_ID.ToString)
                End With

                If FPf.FPApp.SIMPLE_SELECT(Params, Out_Params) Then
                    FORM_GOTO_RECORD_BY_ID(Out_Params.Selected_ID)
                    'RAISEEVENT_Form_ButtonFind_RECORD_SELECTED(Out_Params)
                End If
            End If
        End If
    End Sub

    Private Sub ButtonExportToExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportToExcel.Click
        If FPf.SAVE_ALL Then
            Dim MyObjectName As String = "FormToExcel"

            If FPf.FPApp.DC.Qdf_RunSQL(String.Format("DELETE Dispo1 WHERE Terminal='{0}' And Art='EXCELEXPORT_SELECT'", Terminal)) Then
                If FPf.FPApp.DC.CreateSQLObject_DeleteAllDynamicObjects Then
                    Dim FixTextKey_Excel_Export As String = SQL_BIND_Params.NameOf_FormDef + "_ExcelExportDef"
                    Dim ExcelExportDefs As String = FPf.FPApp.getFixText(FixTextKey_Excel_Export)
                    Dim ExcelExportCode As String = ""
                    Dim DoIt As Boolean = True
                    Dim SourceName As String = ""
                    Dim SourceType As String = ""
                    Dim MyRecordSource As String = ""
                    Dim OutputSelect As String = ""
                    Dim OrderBy As String = ""
                    Dim MyWHERE As String = String.Format("RS_ID = {0}", RS_ID)
                    Dim DoFilter_ProcessID As Long = 0
                    Dim ExcelFileName As String = ""

                    If ExcelExportDefs = "" Then
                        If Not GRID_EXISTS() Then
                            DoIt = False
                            FPf.FPApp.DoErrorMsgBox("FP.ButtonExportToExcel_Click", 0, String.Format("FixText key '{0}' not definied.", FixTextKey_Excel_Export))
                        Else
                            ExcelExportCode = "EXCELEXPORT_" + ServerObject_Prefix + IIf(SubPrefix > "", "_" + SubPrefix, "") + "_GRID"
                            SourceName = SQL_BIND_Params.NameOf_GRID
                            OutputSelect = GRID.SQL_FIELD_LIST(True, True)

                            Select Case SQL_BIND_Params.TypeOf_GRID
                                Case ENUM_ServerObject_Type.V
                                    MyRecordSource = String.Format("SELECT {0} FROM {1} WHERE {2}", OutputSelect, SourceName, MyWHERE)

                                Case ENUM_ServerObject_Type.TF
                                    MyRecordSource = String.Format("SELECT {0} FROM dbo.{1}({2})", OutputSelect, SourceName, RS_ID)

                                Case Else
                                    FPf.FPApp.DoErrorMsgBox("FP.ButtonExportToExcel_Click", 0, "Unknown type of GRID SQL")
                                    DoIt = False
                            End Select
                        End If
                    Else
                        Dim DIC_ExcelExportDefs As New Dictionary(Of String, String)

                        If FPf.FPApp.FIXTEXT_SPLIT_PARAMS(ExcelExportDefs, DIC_ExcelExportDefs) Then
                            If DIC_ExcelExportDefs.Count > 0 Then
                                If DIC_ExcelExportDefs.Count = 1 Then
                                    ExcelExportCode = "EXCELEXPORT_" + DIC_ExcelExportDefs.Keys(0)
                                Else
                                    For Each AktKey As String In DIC_ExcelExportDefs.Keys
                                        If DIC_ExcelExportDefs(AktKey) > "" Then
                                            Dim Params As String() = Split(DIC_ExcelExportDefs(AktKey), "|")

                                            If UBound(Params) < 2 Then
                                                ReDim Preserve Params(2)
                                            End If

                                            'Dispo1Varchar = Kod
                                            'Dispo2Varchar = Leiras
                                            FPf.FPApp.DC.Qdf_RunSQL(String.Format("INSERT INTO Dispo1 (Terminal, Art, Dispo1Varchar, Dispo2Varchar) SELECT '{0}', 'EXCELEXPORT_SELECT', '{1}', '{2}'", Terminal, AktKey, Params(0)), 0)
                                        End If
                                    Next

                                    Dim P As New Struct_Simple_SELECT_Params
                                    Dim P_OUT As New Struct_Simple_SELECT_OutputParams

                                    With P
                                        .FixText_Key = "EXCELEXPORT"
                                        .Field_Mandatory = True
                                        .SQL_WHERE = String.Format("Terminal = '{0}'", Terminal)
                                    End With

                                    If Not FPf.FPApp.SIMPLE_SELECT(P, P_OUT) Then
                                        DoIt = False
                                    Else
                                        ExcelExportCode = "EXCELEXPORT_" + P_OUT.Selected_String
                                    End If
                                End If
                            End If
                        End If

                        If DoIt Then
                            Dim FixTextKey_Excel_Export_Code As String = "ExcelExportDef_" & ExcelExportCode
                            Dim ExportDef As String = FPf.FPApp.getFixText(FixTextKey_Excel_Export_Code)
                            If ExportDef = "" Then
                                DoIt = False
                                FPf.FPApp.DoErrorMsgBox("FP.ButtonExportToExcel_Click", 0, String.Format("FixText key '{0}' not definied.", FixTextKey_Excel_Export_Code))
                            Else
                                Dim DIC_Excel_Export_Code As New Dictionary(Of String, String)

                                If FPf.FPApp.FIXTEXT_SPLIT_PARAMS(ExportDef, DIC_Excel_Export_Code) Then
                                    DoIt = (DoIt And FPf.FPApp.FIXTEXT_CHK_PARAM(DIC_Excel_Export_Code, FixTextKey_Excel_Export_Code, "SOURCENAME"))

                                    If DoIt Then
                                        SourceName = FPf.FPApp.FIXTEXT_getParam("SOURCENAME", DIC_Excel_Export_Code)
                                        SourceType = FPf.FPApp.FIXTEXT_getParam("SOURCETYPE", DIC_Excel_Export_Code)
                                        Dim DoFilterName As String = FPf.FPApp.FIXTEXT_getParam("DOFILTERNAME", DIC_Excel_Export_Code)
                                        Dim SQLFilterWHERE As String = FPf.FPApp.FIXTEXT_getParam("SQLFILTERWHERE", DIC_Excel_Export_Code)
                                        If SQLFilterWHERE > "" Then
                                            MyWHERE = FPf.FPApp.Text_Replace_Standard_Params(FPf.FPApp.FIXTEXT_getParam("SQLFILTERWHERE", DIC_Excel_Export_Code), Me)
                                        End If

                                        Dim ProcedureName As String = FPf.FPApp.FIXTEXT_getParam("PROCEDURENAME", DIC_Excel_Export_Code)
                                        OutputSelect = FPf.FPApp.FIXTEXT_getParam("OUTPUTSELECT", DIC_Excel_Export_Code)
                                        OrderBy = FPf.FPApp.FIXTEXT_getParam("ORDERBY", DIC_Excel_Export_Code)
                                        Dim SelectFields As Boolean = (FPf.FPApp.FIXTEXT_getParam("SELECTFIELDSFROMLIST", DIC_Excel_Export_Code) = "1")

                                        'Ha van DoFilter definialva...
                                        If DoFilterName > "" Then
                                            Dim DoFilter_WhereQuery As String = FPf.FPApp.FIXTEXT_getParam("DOFILTER_WHEREQUERY", DIC_Excel_Export_Code)
                                            If Not FPf.FPApp.DOFILTER(FPf, DoFilterName, DoFilter_WhereQuery, , False) Then
                                                DoIt = False
                                            Else
                                                With FPf.FPApp.gl_FilterParams
                                                    DoFilter_ProcessID = .ProcessID
                                                    MyWHERE = .FilterWHERE
                                                End With
                                            End If
                                        End If
                                        If DoIt Then
                                            'Ha van tarolt eljaras definialva...
                                            If ProcedureName > "" Then
                                                Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

                                                With FPf.FPApp.DC
                                                    .Qdf_set_SP(sqlComm, ProcedureName)
                                                    .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                                                    .Qdf_AddParameter(sqlComm, "@ProcessID", SqlDbType.Int, , , , , DoFilter_ProcessID)
                                                    .Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , RS_ID)
                                                    .Qdf_AddParameter(sqlComm, "@MyWHERE", SqlDbType.NVarChar, , -1, MyWHERE)
                                                    .Qdf_AddParameter(sqlComm, "@ExcelFileName", SqlDbType.NVarChar, ParameterDirection.Output, 128)

                                                    .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                                                    .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
                                                    .Qdf_AddParameter(sqlComm, "@ErrParams", SqlDbType.NVarChar, ParameterDirection.Output, 255)
                                                End With

                                                CURSOR_SHOW_WAIT()
                                                Try
                                                    DoIt = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
                                                    ExcelFileName = nz(sqlComm.Parameters("@ExcelFileName").Value, "")
                                                    MyWHERE = String.Format("Terminal = '{0}'", Terminal)

                                                Catch ex As Exception
                                                    DoIt = False
                                                    FPf.FPApp.DoErrorMsgBox("FP.ButtonExportToExcel", Err.Number, Err.Description)
                                                End Try
                                                CURSOR_SHOW_DEFAULT()
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If DoIt Then
                        If MyRecordSource = "" Then
                            MyWHERE = IIf(MyWHERE <> "", "WHERE " & MyWHERE, "")
                            If OutputSelect = "" Then
                                OutputSelect = "*"
                            End If

                            If SourceType = "" Or SourceType = ENUM_ServerObject_Type.V.ToString Then
                                MyRecordSource = String.Format("SELECT {0} FROM {1} {2}", OutputSelect, SourceName, MyWHERE)
                            End If
                            If SourceType = ENUM_ServerObject_Type.TF.ToString Then
                                MyRecordSource = String.Format("SELECT {0} FROM {1}({2})", OutputSelect, SourceName, RS_ID)
                            End If
                            If OrderBy <> "" Then
                                MyRecordSource = String.Format("{0} ORDER BY {1}", MyRecordSource, OrderBy)
                            End If
                        End If

                        Dim DT As New DataTable

                        CURSOR_SHOW_WAIT()
                        FPf.FPApp.DC.Qdf_Fill_DT(MyRecordSource, DT)
                        CURSOR_HIDE_DEFAULT()

                        Dim Identifier = IIf(ExcelFileName > "", ExcelExportCode, "")
                        Dim ExcelExport As New FP_XLS_Export(FPf.FPApp, DT, Identifier, ExcelFileName)

                        FPf.P_ENABLED = False
                        ExcelExport.ExportData()
                        FPf.P_ENABLED = True
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ButtonImportFromExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportFromExcel.Click
        If FPf.SAVE_ALL Then
            If FPf.FPApp.InitGlobals Then
                If FPf.FPApp.DC.Qdf_RunSQL(String.Format("DELETE Dispo1 WHERE Terminal='{0}'", Terminal)) Then
                    Dim FixTextKey_Excel_Import As String = SQL_BIND_Params.NameOf_FormDef + "_ExcelImportDef"
                    Dim ExcelImportDefs As String = FPf.FPApp.getFixText(FixTextKey_Excel_Import)

                    If ExcelImportDefs = "" Then
                        FPf.FPApp.DoErrorMsgBox("FP.ButtonImportFromExcel_Click", 0, String.Format("FixText key '{0}' not definied.", FixTextKey_Excel_Import))
                    Else
                        Dim DIC_ExcelImportDefs As New Dictionary(Of String, String)
                        Dim DoIt As Boolean = True
                        Dim ExcelImportCode As String = ""

                        If FPf.FPApp.FIXTEXT_SPLIT_PARAMS(ExcelImportDefs, DIC_ExcelImportDefs) Then
                            If DIC_ExcelImportDefs.Count < 1 Then
                                DoIt = False
                                FPf.FPApp.DoErrorMsgBox("FP.ButtonImportFromExcel_Click", 0, String.Format("Az excel import definíció nem tartalmazza egyetlen excel importnak sem a leírását. (FixText kulcs: '{0}')", FixTextKey_Excel_Import))
                            Else
                                If DIC_ExcelImportDefs.Count = 1 Then
                                    ExcelImportCode = DIC_ExcelImportDefs.Keys(0)
                                Else
                                    For Each AktKey As String In DIC_ExcelImportDefs.Keys
                                        If DIC_ExcelImportDefs(AktKey) > "" Then
                                            Dim Params As String() = Split(DIC_ExcelImportDefs(AktKey), "|")

                                            If UBound(Params) < 2 Then
                                                ReDim Preserve Params(2)
                                            End If

                                            'Dispo1Varchar = Kod
                                            'Dispo2Varchar = Leiras
                                            FPf.FPApp.DC.Qdf_RunSQL(String.Format("INSERT INTO Dispo1 (Terminal, Art, Dispo1Varchar, Dispo2Varchar) SELECT '{0}', 'EXCELIMPORT_SELECT', '{1}', '{2}'", Terminal, AktKey, Params(0)), 0)
                                        End If
                                    Next

                                    Dim P As New Struct_Simple_SELECT_Params
                                    Dim P_OUT As New Struct_Simple_SELECT_OutputParams

                                    With P
                                        .FixText_Key = "EXCELIMPORT"
                                        .Field_Mandatory = True
                                        .SQL_WHERE = String.Format("Terminal = '{0}'", Terminal)
                                    End With

                                    If Not FPf.FPApp.SIMPLE_SELECT(P, P_OUT) Then
                                        DoIt = False
                                    Else
                                        ExcelImportCode = P_OUT.Selected_String
                                    End If
                                End If
                            End If
                        End If

                        If DoIt Then
                            If Not (XLS_IMPORT Is Nothing) Then
                                XLS_IMPORT.Dispose()
                                XLS_IMPORT = Nothing
                            End If

                            Dim XLS_IMPORT_Params As New FP_XLS_Import.Struct_FP_XLS_Import

                            With XLS_IMPORT_Params
                                .FPf = FPf
                                .DATA_FP = Me
                                .ExcelImportCode = ExcelImportCode
                                .FileName = ""
                            End With

                            XLS_IMPORT = New FP_XLS_Import(XLS_IMPORT_Params)

                            With XLS_IMPORT
                                .ImportDataWithForm()
                                .Dispose()
                            End With
                            XLS_IMPORT = Nothing

                            Dim Current_ID As Long = P_DATA_Current_ID
                            FORM_DORESYNC(True)
                            If Current_ID <> 0 Then
                                FORM_GOTO_RECORD_BY_ID(Current_ID)
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Public Function ButtonPrint_getPrintDocOptionsFromLine(ByVal Identifier As String, ByVal AktLine As String) As Struct_PrintDocs_Options
        Dim OUT As New Struct_PrintDocs_Options

        Dim Params() As String = Split(AktLine, "|")

        If UBound(Params) < 5 Then
            ReDim Preserve Params(5)
        End If

        With OUT
            .NameInList = Trim(Params(0))
            If .NameInList = "" Then
                FPf.FPApp.DoErrorMsgBox("FP.ButtonPrint_getPrintDocOptionsFromLine", 0, String.Format("NameInList is empty at document '{0}'", Identifier))
                .NameInList = Identifier
            End If

            If Trim(Params(1)).ToUpper = "WORD" Then
                .ReportType = ENUM_ReportType.WORD
                .ReportFileName = Trim(Params(2))
            ElseIf Trim(Params(1)).ToUpper = "WORDX" Then
                .ReportType = ENUM_ReportType.WORDX
                .ReportFileName = Trim(Params(2))
            ElseIf Trim(Params(1)).ToUpper = "HTML" Then
                .ReportType = ENUM_ReportType.HTML
                .ReportFileName = Trim(Params(2))
            Else
                .ReportType = ENUM_ReportType.RDLC
                .ReportFileName = Trim(Params(1))
                If .NameInList = "" Then
                    FPf.FPApp.DoErrorMsgBox("FP.ButtonPrint_getPrintDocOptionsFromLine", 0, String.Format("ReportName is empty at document '{0}'", Identifier))
                    .NameInList = Identifier
                End If

                Select Case Trim(Params(2))
                    Case "", "0"
                        .ReportOpenType = ENUM_ReportOpenType.ToPrinter
                    Case "2"
                        .ReportOpenType = ENUM_ReportOpenType.ToScreen
                    Case Else
                        FPf.FPApp.DoErrorMsgBox("FP.ButtonPrint_getPrintDocOptionsFromLine", 0, String.Format("Unknown parameter {0} for ReportOpenType at document '{1}'", Params(2), Identifier))
                End Select

                Select Case Trim(Params(3))
                    Case "", "0"
                        .AskAfterPrint = False
                    Case "1"
                        .AskAfterPrint = True
                    Case Else
                        FPf.FPApp.DoErrorMsgBox("FP.ButtonPrint_getPrintDocOptionsFromLine", 0, String.Format("Unknown parameter {0} for AskAfterPrint at document '{1}'", Params(3), Identifier))
                End Select

                If Params(4) = "" Then
                    .NumberOfCopies = 1
                Else
                    If Not IsNumeric(Params(4)) Then
                        .NumberOfCopies = 1
                        FPf.FPApp.DoErrorMsgBox("FP.ButtonPrint_getPrintDocOptionsFromLine", 0, String.Format("Unknown parameter {0} for NumberOfCopies at document '{1}'", Params(4), Identifier))
                    Else
                        If Val(Params(4)) < 1 Then
                            FPf.FPApp.DoErrorMsgBox("FP.ButtonPrint_getPrintDocOptionsFromLine", 0, String.Format("Unknown parameter {0} for NumberOfCopies at document '{1}'", Params(4), Identifier))
                        Else
                            .NumberOfCopies = Val(Params(4))
                        End If
                    End If
                End If

                Select Case Trim(Params(5))
                    Case "", "0"
                        .Source = ENUM_ReportSource.CurrentRecord

                    Case "1"
                        .Source = ENUM_ReportSource.RS

                    Case "2"
                        .Source = ENUM_ReportSource.RS_with_Select
                End Select
            End If
        End With

        ButtonPrint_getPrintDocOptionsFromLine = OUT
    End Function

    Public Sub PrintReport(ByVal RdlReportFileName As String, ByVal Report_DT As DataTable, Optional ByVal OtherParameters As String = "")

        Dim MyPrintForm As New FP_Print(FPf.FPApp, RdlReportFileName, OtherParameters)
        MyPrintForm.DATASOURCES_ADD(Report_DT)
        MyPrintForm.Show()
    End Sub

    Public Sub PRINT_BY_IDENTIFIER(Identifier As String, ByRef Prepared As Boolean, ByRef CancelOpenReport As Boolean, ByVal ZD_Report_Head_ID As Integer, ReportFileName As String, ReportType As ENUM_ReportType)
        Prepared = False

        If CancelOpenReport = False Then
            Dim DoIt As Boolean = True

            If Not Prepared Then

                If ReportType = ENUM_ReportType.WORD Then
                    Dim MSWORD_FOR_REPORTS As FP_Word = Nothing

                    Try
                        MSWORD_FOR_REPORTS = New FP_Word(FPf)

                    Catch ex As Exception
                        DoIt = False
                    End Try

                    Prepared = True

                    If DoIt Then
                        Dim DOC_P As New FP_Word.Struct_Word_Report
                        Dim DOC_P_OUT As New FP_Word.Struct_WordDoc_OUT_Params

                        With DOC_P
                            .Report_Identifier = Identifier
                            .Report_FileName = ReportFileName
                            .Report_Head_ID = ZD_Report_Head_ID
                        End With

                        If DoIt Then
                            DoIt = MSWORD_FOR_REPORTS.REPORT_OPEN(DOC_P, DOC_P_OUT)
                        End If

                        If DoIt Then
                            DoIt = MSWORD_FOR_REPORTS.REPORT_FILL(DOC_P_OUT.Doc_Key, True)
                        End If
                    End If

                ElseIf ReportType = ENUM_ReportType.HTML Then
                    Prepared = True
                    Process.Start(ReportFileName)
                End If
            End If

            If Not Prepared Then
                Dim RS_P_IN As New Struct_RS
                Dim RS_P_OUT As Struct_RS_OUT

                With RS_P_IN
                    .RS_Obj_Name = "REPORT_CURRENT_ID"
                    .HWND = 0
                    .RS_ID_FieldName = "CURRENT_RECORD.RecordID"
                    .RS_FROM = String.Format("(SELECT {0} RecordID) CURRENT_RECORD", P_DATA_Current_ID)
                    .MaxRecords = 1
                End With

                FPf.FPApp.RS_SET(RS_P_IN, RS_P_OUT)

                Dim ZDISPO_P_IN As New Struct_ZDISPO_Params

                With ZDISPO_P_IN
                    .Identifier = Identifier
                    .Current_ID = P_DATA_Current_ID
                    .Current_RS_ID = RS_P_OUT.RS_ID
                    .RS_ID = RS_ID
                    .Show_SimpleSelect = True
                End With

                FPf.FPApp.ZDISPO(ZDISPO_P_IN, Nothing)
            End If
        End If
    End Sub

    Private Function Can_be_Print(Identifier As String) As Boolean
        Dim OUT As Boolean = True
        Dim RetValue As Integer
        Dim Fn_Name As String = String.Format("FN_DOC_CHK_{0}", ServerObject_Prefix)
        If gl_FPApp.DC.SQLObjectExists(Fn_Name, ENUM_SQLOBJTYPES.FN) Then
            Dim SQL As String = String.Format("SELECT dbo.{0}({1},'{2}') CanBePrint", Fn_Name, P_DATA_Current_ID, Identifier)
            Dim DR As DataRow
            DR = gl_FPApp.DC.Qdf_get_DataRow(SQL)
            If DR IsNot Nothing Then
                RetValue = DR.Item("CanBePrint")
                If RetValue <> -1 Then
                    OUT = False
                    gl_FPApp.DoMyMsgBox(RetValue)
                End If
            End If
        End If
        Return OUT
    End Function
    Public Sub PRINT_BY_IDENTIFIER(Identifier As String, ByRef Prepared As Boolean, ByRef CancelOpenReport As Boolean)
        Prepared = False
        RaiseEvent Form_Print_Prepare(Me, Identifier, Prepared, CancelOpenReport)

        If gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_IsInstalled("DOCMAN_DEV_2021_03") Then
            CancelOpenReport = Not Can_be_Print(Identifier)
        End If

        If CancelOpenReport = False Then
            Dim DoIt As Boolean = True

            If Not Prepared Then
                Dim P As Struct_PrintDocs_Options = ButtonPrint_getPrintDocOptionsFromLine(Identifier, FORMDEF_PrintOption_DIC(Identifier))

                If P.ReportType = ENUM_ReportType.WORD Then
                    Dim MSWORD_FOR_REPORTS As FP_Word = Nothing

                    Try
                        MSWORD_FOR_REPORTS = New FP_Word(FPf)

                    Catch ex As Exception
                        DoIt = False
                    End Try

                    Prepared = True

                    If DoIt Then
                        Dim DOC_P As New FP_Word.Struct_Word_Report
                        Dim DOC_P_OUT As New FP_Word.Struct_WordDoc_OUT_Params

                        With DOC_P
                            .Report_Identifier = Identifier
                            .Report_FileName = P.ReportFileName
                        End With

                        If DoIt Then
                            DoIt = MSWORD_FOR_REPORTS.REPORT_OPEN(DOC_P, DOC_P_OUT)
                        End If

                        If DoIt Then
                            DoIt = MSWORD_FOR_REPORTS.REPORT_FILL(DOC_P_OUT.Doc_Key, False)
                        End If

                        If DoIt Then
                            Dim CurrDoc As FP_Word.Struct_OpenedDocs = MSWORD_FOR_REPORTS.DIC_Opened_Docs(DOC_P_OUT.Doc_Key)
                            If CurrDoc.Def_DOCMAN_SAVE Then
                                Dim DOCMAN As DOCMAN_Doc_Panel = FPf.GET_FP_BY_ALIAS(CurrDoc.Def_ParentTable).FP_DOCMAN

                                With DOCMAN
                                    If .SHOW_ARCHIV_DIALOG Then
                                        If MSWORD_FOR_REPORTS.REPORT_SAVE_TO_DOCMAN(CurrDoc.Doc_Key, False) Then
                                            DOCMAN.DORESYNC()
                                        End If
                                    End If
                                End With

                                MSWORD_FOR_REPORTS.REPORT_REMOVE_FROM_OPENED_DOCS(DOC_P_OUT.Doc_Key)
                            End If
                        End If
                    End If

                ElseIf P.ReportType = ENUM_ReportType.WORDX Then
                    Dim MSWORD_FOR_REPORTS As FP_WORD_X = Nothing

                    Try
                        MSWORD_FOR_REPORTS = New FP_WORD_X(FPf)
                    Catch ex As Exception
                        DoIt = False
                    End Try

                    Prepared = True

                    If DoIt Then
                        Dim DOC_P As New FP_WORD_X.Struct_Word_Report

                        With DOC_P
                            .Report_Identifier = Identifier
                            .Report_Template_Name = P.ReportFileName
                        End With

                        If DoIt Then
                            DoIt = MSWORD_FOR_REPORTS.Report_Prepare(DOC_P)
                        End If
                        If DoIt Then
                            DoIt = MSWORD_FOR_REPORTS.Load_Word_Doc(DOC_P, False)
                        End If

                        'Open DOC
                        If DoIt Then
                            DoIt = MSWORD_FOR_REPORTS.Open_DocX(DOC_P)
                        End If

                        'Save To Docman
                        If DoIt Then
                            Dim CurrDoc As FP_WORD_X.Struct_Word_Report = MSWORD_FOR_REPORTS.DIC_DocX(DOC_P.Doc_Key)
                            If CurrDoc.Save_To_DOCMAN Then
                                Dim DOCMAN As DOCMAN_Doc_Panel = FPf.GET_FP_BY_ALIAS(CurrDoc.DOCMAN_Def_Parent_Table).FP_DOCMAN
                                With DOCMAN
                                    If .SHOW_ARCHIV_DIALOG Then
                                        If MSWORD_FOR_REPORTS.REPORT_SAVE_TO_DOCMAN(CurrDoc.Doc_Key, False) Then
                                            DOCMAN.DORESYNC()
                                        End If
                                    End If
                                End With

                                MSWORD_FOR_REPORTS.REPORT_REMOVE_FROM_OPENED_DOCS(CurrDoc.Doc_Key)
                            End If
                        End If
                    End If

                ElseIf P.ReportType = ENUM_ReportType.HTML Then
                    Prepared = True
                    Process.Start(P.ReportFileName)
                End If
            End If

            If Not Prepared Then
                Dim RS_P_IN As New Struct_RS
                Dim RS_P_OUT As Struct_RS_OUT

                With RS_P_IN
                    .RS_Obj_Name = "REPORT_CURRENT_ID"
                    .HWND = 0
                    .RS_ID_FieldName = "CURRENT_RECORD.RecordID"
                    .RS_FROM = String.Format("(SELECT {0} RecordID) CURRENT_RECORD", P_DATA_Current_ID)
                    .MaxRecords = 1
                End With

                FPf.FPApp.RS_SET(RS_P_IN, RS_P_OUT)

                Dim ZDISPO_P_IN As New Struct_ZDISPO_Params

                With ZDISPO_P_IN
                    .Identifier = Identifier
                    .Current_ID = P_DATA_Current_ID
                    .Current_RS_ID = RS_P_OUT.RS_ID
                    .RS_ID = RS_ID
                    .Show_SimpleSelect = True
                End With

                FPf.FPApp.ZDISPO(ZDISPO_P_IN, Nothing)
            End If
        End If
    End Sub

    Private ButtonPrint_InProcess As Boolean = False
    Private Sub ButtonPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPrint.Click
        If ButtonPrint_InProcess = False Then
            ButtonPrint_InProcess = True
            If FPf.SAVE_ALL Then
                If FORMDEF_PrintOption_DIC.Count < 1 Then
                    FPf.FPApp.DoErrorMsgBox("FP.ButtonPrint_Click", 0, "No Reports defined")
                Else
                    If Not FPf.FPApp.DC.Qdf_RunSQL(String.Format("DELETE Dispo1 WHERE Terminal='{0}' And Art='REPORT_SELECT'", Terminal)) Then
                        ButtonPrint_InProcess = False
                        Exit Sub
                    End If

                    For Each AktKey In FORMDEF_PrintOption_DIC.Keys
                        Dim P As Struct_PrintDocs_Options = ButtonPrint_getPrintDocOptionsFromLine(AktKey, FORMDEF_PrintOption_DIC(AktKey))

                        FPf.FPApp.DC.Qdf_RunSQL("INSERT INTO Dispo1 (Terminal,     Art,             Dispo2Varchar,          Dispo1Text) " _
                                                 & " SELECT         '" & Terminal & "', 'REPORT_SELECT', '" & AktKey & "',  '" & P.NameInList & "'", 0)
                    Next AktKey

                    Dim DoIt As Boolean = True
                    Dim P_SELECT As New Struct_Simple_SELECT_Params
                    Dim P_SELECT_OUT As New Struct_Simple_SELECT_OutputParams
                    Dim SQL As String = ""
                    Dim DT As DataTable = Nothing

                    If FORMDEF_PrintOption_DIC.Count = 1 Then
                        SQL = String.Format("SELECT Dispo1.Dispo2Varchar Identifier FROM Dispo1 WITH (READUNCOMMITTED) WHERE Terminal = '{0}' And Art = 'REPORT_SELECT'", Terminal)
                    Else
                        With P_SELECT
                            .FixText_Key = "REPORT_SELECT"
                            .SQL_WHERE = String.Format("Terminal = '{0}'", Terminal)
                        End With
                        If Not FPf.FPApp.SIMPLE_SELECT(P_SELECT, P_SELECT_OUT) Then
                            DoIt = False
                        Else
                            SQL = String.Format("SELECT Dispo1.Dispo2Varchar Identifier FROM RS_L WITH (READUNCOMMITTED) INNER JOIN Dispo1 WITH (READUNCOMMITTED) ON RS_L.RecordID = Dispo1.ID WHERE RS_L.RS_ID = {0} ORDER BY RS_L.SeqNum", P_SELECT_OUT.RS_ID)
                        End If
                    End If

                    If DoIt Then
                        DoIt = FPf.FPApp.DC.Qdf_Fill_DT(SQL, DT)
                    End If

                    If DoIt Then
                        Dim CancelOpenReport As Boolean = False

                        RaiseEvent Form_Print_Begin(Me, CancelOpenReport)

                        If Not CancelOpenReport Then
                            For Each Row As DataRow In DT.Rows
                                PRINT_BY_IDENTIFIER(Row!Identifier, False, CancelOpenReport)
                                If CancelOpenReport Then
                                    Exit For
                                End If
                            Next

                            RaiseEvent Form_Print_End(Me)

                        End If
                    End If
                End If
            End If

            ButtonPrint_InProcess = False
        End If
    End Sub
    Private Sub Form_Button_Add_New_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add_New.Click
        FORM_ADD_NEW()
    End Sub
    Private Sub Form_Button_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Up.Click
        FORM_RECORD_UPDOWN(ENUM_UpDown.UP)
    End Sub
    Private Sub Form_Button_Down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Down.Click
        FORM_RECORD_UPDOWN(ENUM_UpDown.DOWN)
    End Sub
    Private Sub Form_Button_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Delete.Click
        FORM_RECORDS_DELETE_CURRENT()
    End Sub

    Private Sub DATA_CLEAR()
        DATA_GOTO_NORECORD()

        DATA_DT_FORM.Clear()

        'RS_ID = 0

    End Sub
    Private Function DATA_CLEAR_SQLFIELDS() As Boolean
        Dim OUT As Boolean = True

        Dim AktRow As DataRow

        For Each AktRow In DATA_DT_FORM_SQLFIELDS_FOR_READ.Rows
            With AktRow
                Dim EmptyValue As String = DBFORMAT_get_EmptyValue(!xtype_VB)
                If !Value > "" Then
                    .BeginEdit()
                    !value = EmptyValue
                    .EndEdit()
                End If
            End With
        Next

        For Each AktRow In DATA_DT_FORM_SQLFIELDS_FOR_WRITE.Rows
            With AktRow
                If !Value > "" Then
                    .BeginEdit()
                    !value = DBFORMAT_from_OBJECT("", !FieldName, !xtype_VB)
                    .EndEdit()
                End If
            End With
        Next

        DATA_DT_FORM_Current_ID = 0
        DATA_LoadedRecord_ID = 0
        DATA_NoRecord = True

        DATA_CLEAR_SQLFIELDS = OUT
    End Function
    Protected Friend Sub DATA_GOTO_NORECORD()
        DATA_CLEAR_SQLFIELDS()
        DATA_DT_FORM_Current_ID = 0

        DATA_NoRecord = True
        DATA_LoadedRecord_ID = 0
    End Sub
    Private Function DATA_RECORDS_LOAD_FROM_RS(Optional ByVal NoRecord_OK As Boolean = False) As Boolean
        Dim OUT As Boolean = False

        Try
            CURSOR_SHOW_WAIT()

            Dim MySQL As String = String.Format("SELECT RecordID, SeqNum FROM RS_L WITH (READUNCOMMITTED) WHERE RS_ID = {0} ORDER BY SeqNum", RS_ID)

            RaiseEvent Data_Records_Loading(Me)

            FPf.FPApp.DC.Qdf_Fill_DT(MySQL, DATA_DT_FORM)
            'Dim da As New SqlDataAdapter(, FPf.FPApp.DC.CNN)

            'DATA_CLEAR()
            'da.Fill(DATA_DT_FORM)

            If Not (BINDINGNAVIGATOR_FORM Is Nothing) Then
                'BINDINGNAVIGATOR_FORM_RS.Tables.Clear()
                'BINDINGNAVIGATOR_FORM_RS.Tables.Add(DATA_DT_FORM.Clone)


                'BINDINGNAVIGATOR_FORM_RS.Tables.Add(DATA_DT_FORM)
                'BINDINGNAVIGATOR_FORM_RS.Tables(0).TableName = "FORM"
                'BINDINGNAVIGATOR_FORM_BS.DataSource = BINDINGNAVIGATOR_FORM_RS
                'BINDINGNAVIGATOR_FORM_BS.DataMember = "FORM"

                BINDINGNAVIGATOR_FORM_BS.DataSource = DATA_DT_FORM
                'BINDINGNAVIGATOR_FORM_BS.DataMember = "FORM"

                BINDINGNAVIGATOR_FORM.BindingSource = BINDINGNAVIGATOR_FORM_BS
            End If

            If DATA_DT_FORM.Rows.Count < 1 Then
                DATA_NoRecord = True
                DATA_DT_FORM_Current_ID = 0

                If NoRecord_OK Then
                    OUT = True
                Else
                    Dim FilterName As String = FORMDEF("FILTERNAME")

                    If FilterName > "" Then
                        FPf.FPApp.DoMyMsgBox(52) ' Record not found
                    End If
                End If
            Else
                'Dim Criteria As String = "SeqNum>0"
                Dim Criteria As String = "SeqNum = 1"

                DATA_DT_FORM_Current_ID = (DATA_DT_FORM.Select(Criteria).First)!RecordID
                BINDINGNAVIGATOR_SETPOSITION_BY_SEQNUM(1)
                DATA_NoRecord = False

                OUT = True
            End If

            RaiseEvent Data_Records_Loaded(Me)

            DATA_RECORDS_LOAD_FROM_RS = OUT

        Catch ex As Exception
            CURSOR_SHOW_DEFAULT()
            FPf.FPApp.DoErrorMsgBox("FP.DATA_RECORDS_LOAD_FROM_RS", Err.Number, Err.Description)
        End Try
    End Function
    Protected Friend Function DATA_LOGS_ACTIVITY_SET() As Boolean
        Dim OUT As Boolean = False

        If ServerObject_Prefix <> "SIMPLESELECT" Then
            If FORMDEF("NO_ACTIVITY_SET") <> "1" Then
                If Not FORM_IsSubForm() Then
                    If Not UnboundForm Then

                        DATA_LOGS_ACTIVITY_CLEAR()

                        If P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
                            Logs_Activity_NewRecord = P_DATA_RecordStatus
                            Logs_Activity_ID = P_DATA_Current_ID
                            Logs_Activity_StartTime = Now
                            Logs_Activity_Active = True
                        End If

                        'Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

                        'FPf.FPApp.DC.Qdf_set_SP(sqlComm, "Logs_Activity_Set")
                        'FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                        'FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@NewRecord", SqlDbType.Int, , , , , P_DATA_NewRecord)
                        'FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@RecordID", SqlDbType.Int, , , , , P_DATA_Current_ID)
                        'FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@TableName", SqlDbType.NVarChar, , 128, FORMDEF("UniqueTable"))
                        'FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@FormName", SqlDbType.NVarChar, , 128, FPf.Frm.Name)

                        'CURSOR_SHOW_WAIT()
                        'Try
                        '    OUT = FPf.Qdf_Execute(sqlComm)
                        'Catch ex As Exception
                        '    FPf.FPApp.DoErrorMsgBox("FP.DATA_LOGS_ACTIVITY_SET", Err.Number, Err.Description)
                        'End Try
                        'CURSOR_SHOW_DEFAULT()
                    End If
                End If
            End If
        End If

        DATA_LOGS_ACTIVITY_SET = OUT
    End Function
    Protected Friend Sub DATA_LOGS_ACTIVITY_CLEAR()
        'If FORMDEF("LOGS_ACTIVITY") = "1" Then
        If FORMDEF("UniqueTable") > "" Then
            If ServerObject_Prefix <> "SIMPLESELECT" Then
                If Not FORM_IsSubForm() Then
                    If FORMDEF("NO_ACTIVITY_SET") <> "1" Then
                        If Not UnboundForm Then
                            If Logs_Activity_Active Then
                                Dim Logs_Activity_Time As Single = DateDiff(DateInterval.Second, Logs_Activity_StartTime, Now)

                                If Logs_Activity_Time >= 5 Then
                                    Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

                                    With FPf.FPApp.DC
                                        .Qdf_set_SP(sqlComm, "Logs_Activity_Add")
                                        .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                                        .Qdf_AddParameter(sqlComm, "@RecordID", SqlDbType.Int, , , , , Logs_Activity_ID)
                                        .Qdf_AddParameter(sqlComm, "@Logs_Activity_Second", SqlDbType.Float, , , , , , Logs_Activity_Time)
                                        .Qdf_AddParameter(sqlComm, "@TableName", SqlDbType.NVarChar, , 128, FORMDEF("UniqueTable"))
                                        .Qdf_AddParameter(sqlComm, "@FormName", SqlDbType.NVarChar, , 128, FPf.Frm.Name)
                                        .Qdf_AddParameter(sqlComm, "@Logged_Users_ID", SqlDbType.Int, , , , , SelUser)
                                    End With

                                    CURSOR_SHOW_WAIT()
                                    Try
                                        FPf.Qdf_Execute(sqlComm)
                                    Catch ex As Exception
                                        FPf.FPApp.DoErrorMsgBox("FP.DATA_LOGS_ACTIVITY_CLEAR", Err.Number, Err.Description)
                                    End Try

                                    CURSOR_SHOW_DEFAULT()

                                    Logs_Activity_NewRecord = False
                                    Logs_Activity_ID = 0
                                    Logs_Activity_StartTime = NULLDATE
                                    Logs_Activity_Active = False
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
        'End If
    End Sub
    Private Function DATA_GOTO_RECORD_BY_SeqNum(ByVal SeqNum As Long) As Boolean
        Dim OUT As Boolean = False
        Dim Criteria As String = String.Format("SeqNum={0}", SeqNum.ToString)

        If Not (DATA_DT_FORM Is Nothing) Then
            If DATA_DT_FORM.Select(Criteria).Count > 0 Then
                DATA_DT_FORM_Current_ID = (DATA_DT_FORM.Select(Criteria).First())!RecordID
                OUT = True
            End If
        End If

        DATA_GOTO_RECORD_BY_SeqNum = OUT
    End Function
    Protected Friend Function DATA_GOTO_RECORD_BY_ID(ByVal ID As Long) As Boolean
        Dim OUT As Boolean = False
        Dim Criteria As String = String.Format("RecordID={0}", ID.ToString)

        If ID = 0 Then
            If Data_AllowAdditions Then
                DATA_DT_FORM_Current_ID = 0
                DATA_NoRecord = False
                OUT = True
            End If
        Else
            If Not (DATA_DT_FORM Is Nothing) Then
                If DATA_DT_FORM.Select(Criteria).Count > 0 Then
                    Dim DoIt As Boolean = True

                    If GRID_EXISTS() Then
                        Dim RowIndex As Integer = -1

                        If Not GRID.ROW_GET_ROWINDEX_OF_RECORDID(ID, RowIndex) Then
                            DoIt = False
                        End If
                    End If

                    If DoIt Then
                        DATA_NoRecord = False
                        DATA_DT_FORM_Current_ID = (DATA_DT_FORM.Select(Criteria).First())!RecordID
                        OUT = True
                    End If
                End If
            End If
        End If

        DATA_GOTO_RECORD_BY_ID = OUT
    End Function
    Private Function DATA_RECORDS_GET_MAX_SEQNUM() As Long
        Dim i As Integer = 0
        Dim maxSeqNum As Long = 0

        If DATA_DT_FORM.Rows.Count > 0 Then

            For i = DATA_DT_FORM.Rows.Count - 1 To 0 Step -1
                If maxSeqNum < DATA_DT_FORM.Rows(i)!SeqNum Then
                    maxSeqNum = DATA_DT_FORM.Rows(i)!SeqNum
                End If
            Next
        End If

        Return maxSeqNum
    End Function
    Private Function DATA_GET_ID_FROM_SEQNUM(ByVal SeqNum As Long) As Long
        Dim OUT As Long = 0
        Dim Criteria As String = String.Format("SeqNum={0}", SeqNum.ToString)

        If Not (DATA_DT_FORM Is Nothing) Then
            If DATA_DT_FORM.Select(Criteria).Count > 0 Then
                OUT = (DATA_DT_FORM.Select(Criteria).First())!RecordID
            End If
        End If

        DATA_GET_ID_FROM_SEQNUM = OUT
    End Function
    Private Function DATA_GET_CURRENT_SEQNUM() As Long
        Dim OUT As Long = 0

        If P_DATA_Current_ID <> 0 Then
            Dim Criteria As String = String.Format("RecordID={0}", P_DATA_Current_ID.ToString)

            If Not (DATA_DT_FORM Is Nothing) Then
                If DATA_DT_FORM.Select(Criteria).Count > 0 Then
                    OUT = (DATA_DT_FORM.Select(Criteria).First())!SeqNum
                End If
            End If
        End If

        DATA_GET_CURRENT_SEQNUM = OUT
    End Function
    Protected Friend Function DATA_RECORDS_LOAD_CURRENT() As Boolean
        Dim OUT As Boolean = False
        If FPf.FPApp.InitGlobals() Then
            Dim AktRow_READ As DataRow
            Dim AktRow_WRITE As DataRow
            Dim Result As Boolean

            If DATA_NoRecord Then
                DATA_CLEAR_SQLFIELDS()
            ElseIf DATA_DT_FORM_Current_ID = 0 Then
                DATA_CLEAR_SQLFIELDS()
                DATA_NoRecord = False
                OUT = True
            Else
                Dim CurrentID As Long = DATA_DT_FORM_Current_ID
                If DATA_CLEAR_SQLFIELDS() Then
                    If GRID_EXISTS() And SQL_BIND_Params.NameOf_GRID.ToUpper = SQL_BIND_Params.NameOf_READ.ToUpper Then
                        Dim CurrentRow As DataRow = GRID.DT_ALL_FIELDS.Select("RecordID=" & CurrentID).First

                        For Each AktRow_READ In DATA_DT_FORM_SQLFIELDS_FOR_READ.Rows
                            With AktRow_READ
                                .BeginEdit()
                                !Value = DBFORMAT_from_OBJECT(CurrentRow.Item(!FieldName), !FieldName, !xtype_VB)
                                .EndEdit()
                            End With
                        Next
                        Result = True
                    Else
                        Select Case SQL_BIND_Params.TypeOf_READ
                            Case ENUM_ServerObject_Type.P   'Stored Procedure
                                Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

                                FPf.FPApp.DC.Qdf_set_SP(sqlComm, SQL_BIND_Params.NameOf_READ)
                                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ID", SqlDbType.Int, , , , , CurrentID)

                                For Each AktRow_READ In DATA_DT_FORM_SQLFIELDS_FOR_READ.Rows
                                    With AktRow_READ
                                        Select Case !xtype
                                            Case SqlDbType.Int, SqlDbType.SmallInt, SqlDbType.Bit, SqlDbType.Float, SqlDbType.DateTime
                                                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@" + !FieldName, !xtype, ParameterDirection.Output)

                                            Case SqlDbType.VarChar, SqlDbType.NVarChar
                                                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@" + !FieldName, !xtype, ParameterDirection.Output, !xLength)

                                            Case Else
                                                FPf.FPApp.DoErrorMsgBox("FP.DATA_RECORDS_LOAD_CURRENT", 0, String.Format("Field {0} has an unknown datatype ({1}).", (!FieldName).ToString, (!xtype).ToString))
                                        End Select
                                    End With
                                Next

                                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)

                                CURSOR_SHOW_WAIT()
                                Try
                                    Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
                                Catch ex As Exception
                                    Result = False
                                    FPf.FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
                                End Try

                                CURSOR_SHOW_DEFAULT()

                                If Result Then
                                    For Each AktRow_READ In DATA_DT_FORM_SQLFIELDS_FOR_READ.Rows
                                        With AktRow_READ
                                            .BeginEdit()
                                            !Value = DBFORMAT_from_SqlCommParameter(sqlComm.Parameters("@" + !FieldName), !xtype_VB)
                                            .EndEdit()
                                        End With
                                    Next
                                End If

                            Case ENUM_ServerObject_Type.V   'View
                                Dim SQL As String = String.Format(String.Format("SELECT * FROM {0} WHERE RS_ID = {1} And RecordID = {2}", SQL_BIND_Params.NameOf_READ, RS_ID.ToString, CurrentID.ToString))
                                Dim DA As New SqlDataAdapter(SQL, FPf.FPApp.DC.CNN)
                                Dim DT As New DataTable
                                Dim Row As DataRow = Nothing
                                Dim RowFound As Boolean = False

                                CURSOR_SHOW_WAIT()

                                Dim Try_Load_Data As Boolean = True

                                Do While Try_Load_Data = True
                                    Try
                                        DA.Fill(DT)
                                        Try_Load_Data = False

                                    Catch ex As Exception
                                        CURSOR_SHOW_DEFAULT()
                                        FPf.FPApp.DoMyMsgBox_From_Resources(4)
                                    End Try
                                Loop

                                CURSOR_SHOW_DEFAULT()

                                If DT.Rows.Count > 1 Then
                                    FPf.FPApp.DoErrorMsgBox("FP.DATA_RECORDS_LOAD_CURRENT", 0, String.Format("'{0}' returned more than 1 record.", SQL))
                                    Result = False
                                Else
                                    If DT.Rows.Count = 1 Then
                                        Row = DT.Rows(0)
                                        RowFound = True
                                    End If

                                    For Each AktRow_READ In DATA_DT_FORM_SQLFIELDS_FOR_READ.Rows
                                        With AktRow_READ
                                            .BeginEdit()
                                            If RowFound Then
                                                !Value = DBFORMAT_from_OBJECT(Row(!FieldName), !FieldName, !xtype_VB)
                                            Else
                                                !Value = ""
                                            End If
                                            .EndEdit()
                                        End With
                                    Next
                                End If
                                Result = True

                            Case ENUM_ServerObject_Type.TF   'Table-valued function
                                Dim SQL As String = String.Format(String.Format("SELECT * FROM dbo.{0}({1}) WHERE RecordID = {2}", SQL_BIND_Params.NameOf_READ, RS_ID.ToString, CurrentID.ToString))
                                Dim DT As DataTable = Nothing
                                Dim Row As DataRow = Nothing
                                Dim RowFound As Boolean = False

                                CURSOR_SHOW_WAIT()

                                FPf.FPApp.DC.Qdf_Fill_DT(SQL, DT)

                                CURSOR_SHOW_DEFAULT()

                                If DT.Rows.Count > 1 Then
                                    FPf.FPApp.DoErrorMsgBox("FP.DATA_RECORDS_LOAD_CURRENT", 0, String.Format("'{0}' returned more than 1 record.", SQL))
                                    Result = False
                                Else
                                    If DT.Rows.Count = 1 Then
                                        Row = DT.Rows(0)
                                        RowFound = True
                                    End If

                                    For Each AktRow_READ In DATA_DT_FORM_SQLFIELDS_FOR_READ.Rows
                                        With AktRow_READ
                                            .BeginEdit()
                                            If RowFound Then
                                                !Value = DBFORMAT_from_OBJECT(Row(!FieldName), !FieldName, !xtype_VB)
                                            Else
                                                !Value = ""
                                            End If
                                            .EndEdit()
                                        End With
                                    Next
                                End If
                                Result = True

                            Case Else
                                FPf.FPApp.DoErrorMsgBox("FP.DATA_RECORDS_LOAD_CURRENT", 0, "Unknown Object Type")
                        End Select
                    End If

                    If Result Then
                        For Each AktRow_WRITE In DATA_DT_FORM_SQLFIELDS_FOR_WRITE.Rows
                            Dim Criteria As String = String.Format("FieldName = '{0}'", AktRow_WRITE!FieldName)
                            If DATA_DT_FORM_SQLFIELDS_FOR_READ.Select(Criteria).Count > 0 Then
                                AktRow_READ = DATA_DT_FORM_SQLFIELDS_FOR_READ.Select(Criteria).First
                                If AktRow_WRITE!Value <> AktRow_READ!Value Then
                                    AktRow_WRITE.BeginEdit()
                                    AktRow_WRITE!Value = AktRow_READ!Value
                                    AktRow_WRITE.EndEdit()
                                End If
                            Else
                                If AktRow_WRITE!Value <> "" Then
                                    AktRow_WRITE.BeginEdit()
                                    AktRow_WRITE!Value = ""
                                    AktRow_WRITE.EndEdit()
                                End If
                            End If
                        Next
                    End If

                    If Result Then
                        DATA_DT_FORM_Current_ID = CurrentID
                        DATA_NoRecord = False
                        DATA_LoadedRecord_ID = DATA_DT_FORM_Current_ID
                        FORM_DIRTY_CLEAR()

                        OUT = True
                    Else
                        DATA_GOTO_NORECORD()
                        FORM_DIRTY_CLEAR()
                    End If
                End If
            End If
        End If

        DATA_RECORDS_LOAD_CURRENT = OUT
    End Function
    Private Function DATA_RECORDS_SAVE_CURRENT(ByRef OUT_ERRFIELD As String) As Boolean
        Dim OUT As Boolean = False

        If UnboundForm Then
            OUT = True
        Else
            Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()
            Dim Result As Boolean = False

            OUT_ERRFIELD = ""

            If FPf.FPApp.InitGlobals() Then
                If P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                    OUT = True
                Else
                    'If Not P_DATA_Dirty Then
                    If Not Dirty Then
                        OUT = True
                    Else
                        Dim AktRow As DataRow
                        Dim MyDate As DateTime = Nothing
                        Dim NullIfNull As String = ""

                        FPf.FPApp.DC.Qdf_set_SP(sqlComm, SQL_BIND_Params.NameOf_SAVE)
                        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ID", SqlDbType.Int, ParameterDirection.InputOutput, , , , DATA_DT_FORM_Current_ID)

                        For Each AktRow In DATA_DT_FORM_SQLFIELDS_FOR_WRITE.Rows
                            With AktRow
                                Select Case !xtype
                                    Case SqlDbType.Int : FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@" + !FieldName, SqlDbType.Int, , , , , Val(!Value))
                                    Case SqlDbType.NVarChar : FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@" + !FieldName, SqlDbType.NVarChar, , !xlength, !Value)
                                    Case SqlDbType.Bit : FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@" + !FieldName, SqlDbType.Bit, , , , , Val(!Value))
                                    Case SqlDbType.DateTime
                                        MyDate = DBFORMAT_get_Date_From_DbStr(!Value, NullIfNull)
                                        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@" + !FieldName, SqlDbType.DateTime, , , NullIfNull, MyDate)
                                    Case SqlDbType.Float : FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@" + !FieldName, SqlDbType.Float, , , , , , Val(!Value))
                                    Case SqlDbType.NText : FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@" + !FieldName, SqlDbType.NText, , , !Value)
                                    Case Else
                                        FPf.FPApp.DoErrorMsgBox("FP.DATA_SAVE_CURRENTRECORD", 0, String.Format("Datafield {0} has an unknown datatype (1)", !FieldName, !xtype))
                                End Select
                            End With
                        Next

                        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
                        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ErrField", SqlDbType.NVarChar, ParameterDirection.Output, 255)

                        CURSOR_SHOW_WAIT()
                        Try
                            Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
                        Catch ex As Exception
                            FPf.FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
                        End Try
                        CURSOR_SHOW_DEFAULT()
                        If Not Result Then
                            OUT_ERRFIELD = nz(sqlComm.Parameters("@ErrField").Value, "")
                        Else
                            If P_DATA_RecordStatus = ENUM_RecordStatus.NEWRECORD Then
                                DATA_DT_FORM_Current_ID = sqlComm.Parameters("@ID").Value
                                If Logs_Activity_Active Then
                                    Logs_Activity_ID = DATA_DT_FORM_Current_ID
                                    Logs_Activity_NewRecord = False
                                End If
                            End If

                            If GRID_EXISTS() Then
                                If SQL_BIND_Params.NameOf_READ = SQL_BIND_Params.NameOf_GRID Then
                                    Dim Current_RecordID As Long = P_DATA_Current_ID
                                    Dim Current_FirstDisplayedColumnIndex As Integer = GRID.GRID.FirstDisplayedScrollingColumnIndex
                                    Dim Current_FirstDisplayedRowIndex As Integer = GRID.GRID.FirstDisplayedScrollingRowIndex
                                    Dim Current_ColumnIndex As Integer = -1

                                    If Not (GRID.GRID.CurrentCell Is Nothing) Then
                                        Current_ColumnIndex = GRID.GRID.CurrentCell.ColumnIndex
                                    End If

                                    DATA_RS_SET(RS_WHERE)
                                    DATA_RECORDS_LOAD_FROM_RS(True)
                                    GRID.FILL()
                                    DATA_GOTO_RECORD_BY_ID(Current_RecordID)
                                    DATA_RECORDS_LOAD_CURRENT()

                                    DATA_NoRecord = False

                                    If Current_ColumnIndex > -1 Then
                                        If Not (GRID.GRID.CurrentCell Is Nothing) Then
                                            GRID.GRID.CurrentCell = GRID.GRID(Current_ColumnIndex, GRID.GRID.CurrentCell.RowIndex)
                                        End If
                                    End If
                                    GRID.GOTO_ROW_BY_RECORDID(Current_RecordID, Current_FirstDisplayedColumnIndex, , Current_FirstDisplayedRowIndex)
                                End If
                            End If

                            OUT = DATA_RECORDS_LOAD_CURRENT()
                        End If
                    End If
                End If
            End If
        End If

        DATA_RECORDS_SAVE_CURRENT = OUT
    End Function
    Private Function FORM_WRITE_FIELDS_TO_DATA() As Boolean
        Dim OUT As Boolean = False

        If P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            If Not (Parent_FP Is Nothing) And Parent_FP_JOINED_DATA_FIELD > "" Then
                DATA_Field_setValue(Parent_FP_JOINED_DATA_FIELD, Parent_FP.P_DATA_Current_ID)
            End If

            Dim AktRow_Write As DataRow
            For Each AktRow_Write In DATA_DT_FORM_SQLFIELDS_FOR_WRITE.Rows
                If CONTROLS.ContainsKey(AktRow_Write!FieldName) Then
                    AktRow_Write.BeginEdit()
                    If P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                        AktRow_Write!Value = ""
                    Else
                        Dim FormattedValue As String = ""

                        CONTROLS(AktRow_Write!FieldName).GET_DBFORMAT_from_CONTROL(FormattedValue)
                        AktRow_Write!Value = FormattedValue
                    End If
                    AktRow_Write.EndEdit()
                End If
            Next

            'Selected_ID-k beirasa
            For Each AktKey As String In CONTROLS.Keys
                With CONTROLS(AktKey)
                    If .P.DT_ID_Field > "" Then
                        Dim Criteria As String = String.Format("FieldName = '{0}'", .P.DT_ID_Field)

                        Select Case DATA_DT_FORM_SQLFIELDS_FOR_WRITE.Select(Criteria).Count
                            Case 0
                                'Nothing to do

                            Case 1
                                AktRow_Write = DATA_DT_FORM_SQLFIELDS_FOR_WRITE.Select(Criteria).First
                                AktRow_Write.BeginEdit()
                                AktRow_Write!Value = DBFORMAT_from_OBJECT(.Selected_ID, .FieldName + ".Selected_ID", "INT")
                                AktRow_Write.EndEdit()

                            Case Else
                                FPf.FPApp.DoErrorMsgBox("FP.FORM_WRITE_FIELDS_TO_DATA", 0, String.Format("Field '{0}' exists in DATA_DT_FORM_SQLFIELDS_FOR_WRITE more than ones! (ServerObject_Prefix: '{1}', SubPrefix: '{2}')", .P.DT_ID_Field, ServerObject_Prefix, SubPrefix))
                        End Select
                    End If
                End With
            Next

            OUT = True
        End If

        FORM_WRITE_FIELDS_TO_DATA = OUT
    End Function
    Private Function FORM_RECORDS_LOAD_FOR_PARENT_FP_CURRENT_RECORD() As Boolean
        Dim OUT As Boolean = False

        If Not (Parent_FP Is Nothing) And Parent_FP_JOINED_DATA_FIELD > "" Then
            'Dim SubWHERE As String = TEXT_AND(String.Format("{0} = {1}", .Parent_FP_JOINED_DATA_FIELD, P_DATA_Current_ID), FORM_SubWHERE_FIX)
            Dim SubWHERE As String = String.Format("{0} = {1}", Parent_FP_JOINED_DATA_FIELD, Parent_FP.P_DATA_Current_ID)
            OUT = FORM_RECORDS_LOAD(SubWHERE, , True)

            If OUT = True Then
                If P_FP_Refresh_Field_Visible Then
                    FP_Active_Only_When_This_Field_Visible_Current_Parent_ID = Parent_FP.P_DATA_Current_ID
                End If
            End If
        End If

        Return OUT
    End Function
    Private Function FORM_SET_RECORDSOURCE_FOR_CHILDREN() As Boolean
        Dim OUT As Boolean = True

        For Each Current_FP_ID As Integer In FPf.FPs.Keys
            With FPf.FPs(Current_FP_ID)
                If Not (.Parent_FP Is Nothing) And .Parent_FP_JOINED_DATA_FIELD > "" Then
                    If .Parent_FP.Equals(Me) Then
                        OUT = .FORM_RECORDS_LOAD_FOR_PARENT_FP_CURRENT_RECORD()
                    End If
                End If
            End With
        Next
        If OUT = True Then
            RaiseEvent Form_Current_AfterChildren(Me)
        End If
        FORM_SET_RECORDSOURCE_FOR_CHILDREN = OUT
    End Function

    Private Sub ActiveControl_OLDVALUE_SET_FROM_CURRENT_VALUE()
        If Not (FPf.ActiveControl Is Nothing) Then
            With FPf.ActiveControl
                '!!! Itt a kulonbseg az FP_Form.ActiveControl_SET_OLDVALUE_FROM_CURRENT_VALUE-hoz kepest:
                '    Ez a rutin csak akkor allitja be az OldValue-t, ha az ActiveControl az FP-hez tartozik.
                If .FP.Equals(Me) Then
                    .OLDVALUE_SET_FROM_CURRENT_VALUE()
                End If
            End With
        End If
    End Sub

    Public Sub Form_Field_Enter_Chk(ByVal FPc As FP_Control, ByVal FPc_for_Check As FP_Control, ByRef Handled As Boolean)
        If Not Handled Then
            If Not P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                If Not (FPc_for_Check Is Nothing) Then
                    If FPc.c.TabIndex > FPc_for_Check.c.TabIndex Then
                        If FPc_for_Check.P.Mandatory Then
                            If FPc_for_Check.ISEMPTY Then
                                FPf.FOCUS_ON_AT_THE_END(FPc_for_Check.c)
                                Handled = True
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Function FORM_BIND_DATA(Optional ByVal NoUpdateChildren As Boolean = False) As Boolean
        Dim OUT As Boolean = False

        Try
            DATA_Binded = False

            If GRID_EXISTS() And SQL_BIND_Params.NameOf_READ.ToUpper = SQL_BIND_Params.NameOf_GRID.ToUpper Then
                For Each AktRow In DATA_DT_FORM_SQLFIELDS_FOR_READ.Rows
                    Dim Is_COMBO_VALUEMEMBER As Boolean = False
                    Dim Is_DATETIME_VALUEMEMBER As Boolean = False
                    Dim Is_Formatted_Data As Boolean = False
                    Dim FPc_Name As String = AktRow!FieldName
                    Dim FieldName_COMBO_VALUEMEMBER As String = FPc_Name + "_COMBO_VALUEMEMBER"
                    Dim FieldName_DATETIME_VALUEMEMBER As String = FPc_Name + "_DATETIME_VALUEMEMBER"
                    Dim Crit_has_COMBO_VALUEMEMBER As String = String.Format("FieldName = '{0}'", FieldName_COMBO_VALUEMEMBER)
                    Dim Crit_has_DATETIME_VALUEMEMBER As String = String.Format("FieldName = '{0}'", FieldName_DATETIME_VALUEMEMBER)

                    If Strings.Right(FPc_Name.ToUpper, 18) = "_COMBO_VALUEMEMBER" Then
                        FPc_Name = Strings.Left(FPc_Name, Strings.Len(FPc_Name) - 18)
                        Is_COMBO_VALUEMEMBER = True
                    ElseIf Strings.Right(FPc_Name.ToUpper, 21) = "_DATETIME_VALUEMEMBER" Then
                        FPc_Name = Strings.Left(FPc_Name, Strings.Len(FPc_Name) - 21)
                        Is_DATETIME_VALUEMEMBER = True
                    Else
                        With DATA_DT_FORM_SQLFIELDS_FOR_READ
                            If .Select(Crit_has_COMBO_VALUEMEMBER).Count > 0 Then
                                Is_Formatted_Data = True
                            ElseIf .Select(Crit_has_DATETIME_VALUEMEMBER).Count > 0 Then
                                Is_Formatted_Data = True
                            End If
                        End With
                    End If

                    If Is_Formatted_Data = False Then
                        If CONTROLS.ContainsKey(FPc_Name) Then
                            With CONTROLS(FPc_Name)
                                .SET_VALUE_from_DBFORMAT(AktRow!Value)
                                If .P.DT_ID_Field = "" Then
                                    .Selected_ID = 0 'UNDO-nal ez fontos
                                Else
                                    If DATA_DT_FORM_SQLFIELDS_FOR_READ.Select(String.Format("FieldName = '{0}'", .P.DT_ID_Field)).Count < 1 Then
                                        FPf.FPApp.DoErrorMsgBox("FP.FORM_BIND_DATA", 0, String.Format("ID_Field '{0}' for field '{1}' in DATA_DT_FORM_SQLFIELDS_FOR_READ not found.", .P.DT_ID_Field, AktRow!FieldName))
                                    Else
                                        .Selected_ID = Val(DATA_DT_FORM_SQLFIELDS_FOR_READ.Select(String.Format("FieldName = '{0}'", .P.DT_ID_Field)).First!Value)
                                    End If
                                End If
                            End With
                        End If
                    End If
                Next
            Else
                For Each AktRow In DATA_DT_FORM_SQLFIELDS_FOR_READ.Rows
                    If CONTROLS.ContainsKey(AktRow!FieldName) Then
                        With CONTROLS(AktRow!FieldName)
                            .SET_VALUE_from_DBFORMAT(AktRow!Value)
                            If .P.DT_ID_Field = "" Then
                                .Selected_ID = 0 'UNDO-nal ez fontos
                            Else
                                If DATA_DT_FORM_SQLFIELDS_FOR_READ.Select(String.Format("FieldName = '{0}'", .P.DT_ID_Field)).Count < 1 Then
                                    FPf.FPApp.DoErrorMsgBox("FP.FORM_BIND_DATA", 0, String.Format("ID_Field '{0}' for field '{1}' in DATA_DT_FORM_SQLFIELDS_FOR_READ not found.", .P.DT_ID_Field, AktRow!FieldName))
                                Else
                                    .Selected_ID = Val(DATA_DT_FORM_SQLFIELDS_FOR_READ.Select(String.Format("FieldName = '{0}'", .P.DT_ID_Field)).First!Value)
                                End If
                            End If
                        End With
                    End If
                Next
            End If

            For Each CurrentKey As String In CONTROLS.Keys
                CONTROLS(CurrentKey).Value_Validated = True
            Next

            If DATA_NoRecord Then
                If GRID_EXISTS() Then
                    GRID.GOTO_NORECORD()
                End If

                RAISEEVENT_Form_NoRecord()
            Else
                If GRID_EXISTS() Then
                    GRID.COLUMNS_Frozen_ACTIVATE()
                    GRID.MOVE_GRID_PANEL_ON_ROW()
                End If

                RAISEEVENT_Form_Current()
            End If
            If FPf.P_ActiveControl IsNot Nothing Then
                If FPf.P_ActiveControl.FP.Equals(Me) Then
                    Dim Handled As Boolean = False

                    FPf.P_ActiveControl.EVENT_GOTFOCUS()
                End If
            End If
            If NoUpdateChildren = False Then
                FORM_SET_RECORDSOURCE_FOR_CHILDREN()
            End If

            COLORING_ALL()

            DATA_Binded = True 'Nagyon fontos, hogy ez ne legyen a Form_Current es a FORM_SET_RECORDSOURCE_FOR_CHILDREN elott.
            'Ha ugyanis ott valamilyen adatmanipulacio, majd mentes zajlik le, akkor mindenfele bonyolult szoveveny johet letre.
            'Igy viszont a P_Data_Dirty False-t fog visszaadni, lenyegesen leroviditve a dolog folyasat.

            If Not (FPf.ActiveControl Is Nothing) Then
                With FPf.ActiveControl
                    If .FP.Equals(Me) Then
                        If Not GRID_EXISTS() Then
                            .SelectEntireField()
                        Else
                            If Not GRID.FILTER_FIELDS_IS_FILTERFIELD(FPf.ActiveControl) Then
                                .SelectEntireField()
                            End If
                        End If
                    End If

                    .OLDVALUE_SET_FROM_CURRENT_VALUE()
                End With
            End If

            OUT = True

        Catch ex As Exception
            DATA_Binded = True
            FPf.FPApp.DoErrorMsgBox("FP.FORM_BIND_DATA", Err.Number, Err.Description + " ServerObject_Prefix: '" + ServerObject_Prefix + "' Subprefix: '" + SubPrefix + "'")
        End Try

        DATA_Binded = True

        FORM_BIND_DATA = OUT
    End Function

    Private Function FORM_SET_CURRENT_FROM_NAVIGATOR_POSITION() As Boolean
        Dim OUT As Boolean = False

        If Not (BINDINGNAVIGATOR_FORM Is Nothing) Then
            If FPf.SAVE_ALL() Then
                If Not (DATA_DT_FORM Is Nothing) Then
                    Dim SelectedSeqNum As Long = BINDINGNAVIGATOR_FORM.BindingSource.Position + 1

                    If DATA_GOTO_RECORD_BY_SeqNum(SelectedSeqNum) Then
                        OUT = FORM_RECORDS_LOAD_CURRENT()
                    End If
                End If
            End If
        End If

        FORM_SET_CURRENT_FROM_NAVIGATOR_POSITION = OUT
    End Function

    Private Function BINDINGNAVIGATOR_FORM_GOTO_OLDPOSITION() As Boolean
        Dim OUT As Boolean = True

        If (BINDINGNAVIGATOR_FORM_PositionItem Is Nothing) Then
            OUT = True
        Else
            If DATA_DT_FORM_Current_ID >= 0 Then
                Dim Criteria As String = String.Format("RecordID={0}", DATA_DT_FORM_Current_ID)

                If DATA_DT_FORM.Select(Criteria).Count < 1 Then
                    OUT = False
                Else
                    BINDINGNAVIGATOR_FORM.BindingSource.Position = (DATA_DT_FORM.Select(Criteria).First)!SeqNum - 1
                End If
            End If
        End If

        BINDINGNAVIGATOR_FORM_GOTO_OLDPOSITION = OUT
    End Function
    Private Sub BINDINGNAVIGATOR_FORM_Move_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BINDINGNAVIGATOR_FORM_MoveFirstItem.MouseUp, BINDINGNAVIGATOR_FORM_MovePreviousItem.MouseUp, BINDINGNAVIGATOR_FORM_MoveNextItem.MouseUp, BINDINGNAVIGATOR_FORM_MoveLastItem.MouseUp
        If Not FPf.SAVE_ALL Then
            BINDINGNAVIGATOR_FORM_GOTO_OLDPOSITION()
        Else
            FORM_SET_CURRENT_FROM_NAVIGATOR_POSITION()
        End If
    End Sub
    Private Sub BINDINGNAVIGATOR_FORM_AddNewItem_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BINDINGNAVIGATOR_FORM_AddNewItem.MouseUp
        Dim CurrentID As Long = P_DATA_Current_ID

        If Not FORM_ADD_NEW() Then
            FORM_DORESYNC(True)
            If CurrentID <> 0 Then
                FORM_GOTO_RECORD_BY_ID(CurrentID)
            End If
        End If
    End Sub

    Private Sub BINDINGNAVIGATOR_FORM_DeleteItem_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BINDINGNAVIGATOR_FORM_DeleteItem.MouseUp
        Dim CurrentID As Long = P_DATA_Current_ID

        If Not FORM_RECORDS_DELETE_CURRENT() Then
            FORM_DORESYNC(True)
            If CurrentID <> 0 Then
                FORM_GOTO_RECORD_BY_ID(CurrentID)
            End If
        End If
    End Sub

    Private Sub BINDINGNAVIGATOR_FORM_PositionItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles BINDINGNAVIGATOR_FORM_PositionItem.GotFocus
        FPf.SAVE_ALL()
    End Sub
    Private Function BINDINGNAVIGATOR_SETPOSITION_BY_SEQNUM(ByVal SeqNum As Long) As Boolean
        Dim OUT As Boolean = False

        If (BINDINGNAVIGATOR_FORM Is Nothing) Then
            OUT = True
        Else
            If Not (DATA_DT_FORM Is Nothing) Then
                Dim Criteria As String = String.Format("SeqNum={0}", SeqNum)

                If DATA_DT_FORM.Select(Criteria).Count > 0 Then
                    BINDINGNAVIGATOR_FORM.BindingSource.Position = SeqNum - 1
                    OUT = True
                End If
            End If

            If OUT = False Then
                BINDINGNAVIGATOR_FORM.BindingSource.Position = -1
            End If
        End If

        BINDINGNAVIGATOR_SETPOSITION_BY_SEQNUM = OUT
    End Function
    Private Function BINDINGNAVIGATOR_SETPOSITION_BY_ID(ByVal ID As Long) As Boolean
        Dim OUT As Boolean = False

        If BINDINGNAVIGATOR_FORM Is Nothing Then
            OUT = True
        Else
            If Not (DATA_DT_FORM Is Nothing) Then
                Dim Criteria As String = String.Format("RecordID={0}", ID)

                If DATA_DT_FORM.Select(Criteria).Count > 0 Then
                    BINDINGNAVIGATOR_FORM.BindingSource.Position = (DATA_DT_FORM.Select(Criteria).First)!SeqNum - 1
                    OUT = True
                End If
            End If

            If OUT = False Then
                BINDINGNAVIGATOR_FORM.BindingSource.Position = -1
            End If
        End If

        BINDINGNAVIGATOR_SETPOSITION_BY_ID = OUT
    End Function

    Private Function DATA_RECORDS_DELETE_CURRENT() As Boolean
        Dim OUT As Boolean = False

        If FPf.FPApp.InitGlobals() Then
            If P_DATA_Current_ID <> 0 Then
                Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()
                Using sqlComm
                    FPf.FPApp.DC.Qdf_set_SP(sqlComm, SQL_BIND_Params.NameOf_DEL)
                    FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                    FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ID", SqlDbType.Int, , , , , P_DATA_Current_ID)
                    FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                    FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)

                    CURSOR_SHOW_WAIT()
                    Try
                        OUT = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
                    Catch ex As Exception
                        FPf.FPApp.DoErrorMsgBox("FP.DATA_DELETE", Err.Number, Err.Description)
                    End Try
                End Using

                CURSOR_SHOW_DEFAULT()
            End If
        End If

        DATA_RECORDS_DELETE_CURRENT = OUT
    End Function
    Private Function DATA_RECORD_UPDOWN(ByVal UpDown As ENUM_UpDown) As Boolean
        Dim OUT As Boolean = False

        If FPf.FPApp.InitGlobals() Then
            Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()
            Dim Result As Boolean

            FPf.FPApp.DC.Qdf_set_SP(sqlComm, "Form_ButtonUpDown_Click_Standard")
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , RS_ID)
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@UniqueTable", SqlDbType.NVarChar, , 255, FORMDEF("UNIQUETABLE"))
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@CurrentRecordID", SqlDbType.Int, , , , , P_DATA_Current_ID)
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@SeqNum_Field", SqlDbType.NVarChar, , 255, FORMDEF("UNIQUETABLE_SEQNUM"))
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@UpDown", SqlDbType.Int, , , , , UpDown)
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)

            CURSOR_SHOW_WAIT()

            Try
                Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
            Catch ex As Exception
                Result = False
                FPf.FPApp.DoErrorMsgBox("OrderEntry.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
            End Try

            CURSOR_SHOW_DEFAULT()

            If Result Then
                OUT = True
            End If
        End If

        DATA_RECORD_UPDOWN = OUT
    End Function
    Protected Friend Function Text_Replace_Standard_Params(MyText As String) As String
        Return FPf.FPApp.Text_Replace_Standard_Params(MyText, Me)
    End Function
    Private Function CONTROLS_SET_PARENT(ByVal c As Control, ByVal c_Label As Label, ByVal Parent As String) As Boolean
        Dim OUT As Boolean = True

        If Parent > "" Then
            If Not FPf.CONTROLS.ContainsKey(Parent) Then
                FPf.FPApp.DoErrorMsgBox("FP.CONTROLS_SET_PARENT", 0, String.Format("Parentcontrol '{0} does not exists'", Parent))
                OUT = False
            Else
                Try
                    c.Parent = FPf.CONTROLS(Parent)
                    If Not (c_Label Is Nothing) Then
                        c_Label.Parent = FPf.CONTROLS(Parent)
                    End If

                Catch ex As Exception
                    OUT = False
                    FPf.FPApp.DoErrorMsgBox("FP.CONTROLS_SET_PARENT", Err.Number, Err.Description)
                End Try
            End If
        End If

        CONTROLS_SET_PARENT = OUT
    End Function
    Private Function CONTROLS_CREATE(ByVal Control_Props As Struct_CONTROL_PROPS, ByRef OUT_c As Control, ByRef OUT_Label As Label) As Boolean
        Dim OUT As Boolean = True

        With Control_Props
            Select Case .Type.ToUpper
                Case ""
                    OUT_c = New TextBox

                Case "COMBOBOX"
                    OUT_c = New ComboBox
                    OUT_c.Size = .ClientRectangle.Size
                    OUT_c.BackColor = COLORS_FIELD_NORMAL_BG

                Case "CHECKBOX"
                    OUT_c = New CheckBox
                    OUT_c.Size = .ClientRectangle.Size
                    OUT_c.BackColor = COLORS_FIELD_NORMAL_BG
                    With CType(OUT_c, CheckBox)
                        .CheckAlign = ContentAlignment.MiddleCenter
                    End With

                Case "BUTTON"
                    OUT_c = New Button
                    OUT_c.Size = .ClientRectangle.Size
                    'OUT_c.BackColor = COLORS_FIELD_NORMAL_BG
                    OUT_c.BackColor = System.Drawing.SystemColors.Control

                Case "LABEL"
                    OUT_c = New Label
                    OUT_c.Size = .ClientRectangle.Size
                    OUT_c.BackColor = COLORS_FIELD_NORMAL_BG

                Case "LISTVIEW"
                    OUT_c = New ListView
                    OUT_c.Size = .ClientRectangle.Size
                    OUT_c.BackColor = COLORS_FIELD_NORMAL_BG

                Case "RICHTEXTBOX"
                    OUT_c = New RichTextBox
                    OUT_c.Size = .ClientRectangle.Size
                    OUT_c.BackColor = COLORS_FIELD_NORMAL_BG

                Case "TABCONTROL"
                    OUT_c = New TabControl
                    OUT_c.BackColor = COLORS_FIELD_NORMAL_BG

                Case "TABPAGE"
                    OUT_c = New TabPage
                    OUT_c.BackColor = COLORS_FIELD_NORMAL_BG

                Case "PICTUREBOX"
                    OUT_c = New PictureBox
                    OUT_c.BackColor = Color.Transparent
                    OUT_c.Size = New Size(44, 44)

                Case "GRID"
                    OUT_c = New DataGrid

                Case "PANEL"
                    OUT_c = New Panel

                Case "HTML EDITOR"
                    OUT_c = New MSDN.Html.Editor.HtmlEditorControl
                    With CType(OUT_c, MSDN.Html.Editor.HtmlEditorControl)
                        .ToolbarDock = DockStyle.Top
                        .ToolbarVisible = True
                    End With

                Case Else
                    OUT = False
                    FPf.FPApp.DoErrorMsgBox("FP.CONTROLS_CREATE", 0, String.Format("Unknown Controltype {0} for field {1}", .Type, .Name))
            End Select

            If OUT = True Then
                OUT_c.Name = FieldPrefix + .Name
                OUT_c.Location = .ClientRectangle.Location

                If TypeOf (OUT_c) IsNot MSDN.Html.Editor.HtmlEditorControl Then
                    OUT_c.Font = Font_NORMAL
                End If
                OUT_c.Visible = True
                If nz(FieldPrefix, "") = "" Then
                    OUT = CONTROLS_SET_PARENT(OUT_c, Nothing, .Parent)
                Else
                    If nz(.Parent, "") > "" Then
                        OUT = CONTROLS_SET_PARENT(OUT_c, Nothing, FieldPrefix + .Parent)
                    End If
                End If

                If OUT Then
                    OUT = FPf.CONTROLS_ADD(OUT_c)
                End If
            End If

            If OUT = True Then
                If .Label_Name > "" Then
                    OUT_Label = New Label
                    OUT_Label.Name = FieldPrefix + .Label_Name
                    OUT_Label.Location = .Label_Clientrechtangle.Location
                    OUT_Label.Font = Font_NORMAL
                    OUT_Label.Size = .Label_Clientrechtangle.Size
                    If .ClientRectangle.Height = .Label_Clientrechtangle.Height Then
                        OUT_Label.Height = OUT_c.Height
                    End If
                    If .ClientRectangle.Width = .Label_Clientrechtangle.Width Then
                        OUT_Label.Width = OUT_c.Width
                    End If
                    OUT_Label.BackColor = COLORS_LABEL_BG
                    OUT_Label.ForeColor = COLORS_LABEL_FORE
                    OUT_Label.Visible = True

                    OUT = FPf.CONTROLS_ADD(OUT_Label)
                End If
            End If

            If OUT = True Then
                If nz(FieldPrefix, "") = "" Then
                    OUT = CONTROLS_SET_PARENT(OUT_c, OUT_Label, .Parent)
                Else
                    If .Parent > "" Then
                        OUT = CONTROLS_SET_PARENT(OUT_c, OUT_Label, FieldPrefix + .Parent)
                    End If
                End If
            End If
        End With

        If OUT = True Then
            If Not (OUT_c Is Nothing) Then
                OUT_c.BringToFront()
            End If

            If Not (OUT_Label Is Nothing) Then
                OUT_Label.BringToFront()
            End If
        End If

        CONTROLS_CREATE = OUT
    End Function
    Public Function DATA_RS_SQL(ByVal SubWHERE As String, Optional ByVal Spec_MaxRecords As Integer = -1) As String
        Dim OUT As String = ""

        Dim SQL_SELECT As String = String.Format("SELECT TOP {0} ID", IIf(Spec_MaxRecords <> -1, Spec_MaxRecords, MAXRECORDS).ToString)
        Dim SQL_FROM As String = String.Format(" FROM {0} ", SQL_BIND_Params.NameOf_WhereQuery)
        Dim SQL_WHERE As String = DATA_RS_WHERE(SubWHERE)
        Dim SQL_GROUPBY As String = " GROUP BY ID "
        Dim SQL_ORDERBY As String = FPf.FPApp.FIXTEXT_getParam("OrderBy", FORMDEF_DIC)

        If SQL_WHERE > "" Then
            SQL_WHERE = " WHERE " + SQL_WHERE
        End If

        If SQL_ORDERBY > "" Then
            SQL_ORDERBY = " ORDER BY " + SQL_ORDERBY
        End If

        OUT = SQL_SELECT + SQL_FROM + SQL_WHERE + SQL_GROUPBY + SQL_ORDERBY

        Return OUT
    End Function
    Public Function DATA_RS_WHERE(ByVal SubWHERE As String) As String
        Dim OUT As String = ""

        OUT = FORM_SubWHERE_FIX
        OUT = TEXT_AND(OUT, DOFILTER_ReturnedParams.FilterWHERE)
        OUT = TEXT_AND(OUT, SubWHERE)
        OUT = TEXT_AND(OUT, FORMDEF("Recordsource_WHERE"))

        If DOFILTER_ReturnedParams.LetNewRecord Then
            OUT = TEXT_AND(OUT, FORM_SubWHERE_for_NewRecords)
        End If

        If Organisation_Handling Then
            Dim Org_Where As String
            Dim R() As DataRow = DATA_DT_FORM_SQLFIELDS_FOR_WHEREQUERY.Select("FieldName = 'Organisation_ID'")
            If R.Length = 1 Then
                If Organisation_Rights_IDS <> String.Empty Then
                    Org_Where = String.Format("Organisation_ID IN({0})", Organisation_Rights_IDS)
                    OUT = TEXT_AND(OUT, Org_Where)
                End If
            End If
        End If

        Return OUT
    End Function
    Private Function DATA_RS_SET(ByVal MyWHERE As String, Optional ByVal Selected As Boolean = False) As Boolean
        Dim OUT As Boolean = False

        RaiseEvent Data_RS_SET_Before(Me)

        RS_Obj_Name = String.Format("{0}", SQL_BIND_Params.RS_ServerObject_Prefix)
        RS_FROM = SQL_BIND_Params.NameOf_WhereQuery
        RS_GROUPBY = "ID"
        RS_ORDERBY = FPf.FPApp.FIXTEXT_getParam("OrderBy", FORMDEF_DIC)
        RS_ID = 0
        RS_RecCount = 0

        Dim RS_P As New Struct_RS
        Dim RS_P_OUT As New Struct_RS_OUT
        Dim Frm_Modal As Boolean = False

        If Not (FPf Is Nothing) Then
            If Not (FPf.Frm Is Nothing) Then
                Frm_Modal = FPf.Frm.Modal
            End If
        End If

        With RS_P
            If Frm_Modal = True Then
                .HWND = 0
            Else
                .HWND = FPf.Frm_Handle
            End If
            .RS_Obj_Name = FieldPrefix + RS_Obj_Name
            .RS_ID_FieldName = "ID"
            .RS_FROM = RS_FROM
            .RS_WHERE = MyWHERE
            .RS_GROUPBY = RS_GROUPBY
            .RS_ORDERBY = RS_ORDERBY
            .Selected = Selected
            .MaxRecords = MAXRECORDS
        End With

        OUT = FPf.FPApp.RS_SET(RS_P, RS_P_OUT)

        If OUT Then
            With RS_P_OUT
                RS_ID = .RS_ID
                RS_RecCount = .RECORDCOUNT
            End With

            RaiseEvent Data_RS_SET_After(Me)
        End If

        DATA_RS_SET = OUT
    End Function
    Private Function INIT_SQL_BIND() As Boolean
        Dim OUT As Boolean = True

        FPf.FP_Add(Me, FP_ID)

        If UnboundForm Then
            FORMDEF_SETTINGS_LOAD()
            FORMDEF_PRINTOPTION_LOAD()
        Else
            If ServerObject_Prefix = "" Then
                OUT = False
                FPf.FPApp.DoErrorMsgBox("FP.INIT_SQL_BIND", Err.Number, "ServerObject_Prefix = '' Not Allowed for Binded Forms.")
            End If

            If OUT Then
                OUT = FORMDEF_LOAD()
            End If


            If OUT Then
                OUT = FPf.FPApp.RS_GET_FIELDPROPERTIES_FROM_SERVER_OBJECT(SQL_BIND_Params.NameOf_READ, DATA_DT_FORM_SQLFIELDS_FOR_READ, SQL_BIND_Params.TypeOf_READ)
            End If

            If OUT Then
                If SQL_BIND_Params.NameOf_SAVE > "" Then
                    OUT = FPf.FPApp.RS_GET_FIELDPROPERTIES_FROM_SERVER_OBJECT(SQL_BIND_Params.NameOf_SAVE, DATA_DT_FORM_SQLFIELDS_FOR_WRITE, SQL_BIND_Params.TypeOf_SAVE)
                End If
            End If

            If OUT Then
                If SQL_BIND_Params.NameOf_DEL > "" Then
                    OUT = FPf.FPApp.RS_GET_FIELDPROPERTIES_FROM_SERVER_OBJECT(SQL_BIND_Params.NameOf_DEL, DATA_DT_FORM_SQLFIELDS_FOR_DEL, SQL_BIND_Params.TypeOf_DEL)
                End If
            End If

            If OUT Then
                If SQL_BIND_Params.NameOf_GRID > "" Then
                    OUT = FPf.FPApp.RS_GET_FIELDPROPERTIES_FROM_SERVER_OBJECT(SQL_BIND_Params.NameOf_GRID, DATA_DT_FORM_SQLFIELDS_FOR_GRID, SQL_BIND_Params.TypeOf_GRID)
                End If
            End If

            If OUT Then
                If SQL_BIND_Params.NameOf_WhereQuery > "" Then
                    OUT = FPf.FPApp.RS_GET_FIELDPROPERTIES_FROM_SERVER_OBJECT(SQL_BIND_Params.NameOf_WhereQuery, DATA_DT_FORM_SQLFIELDS_FOR_WHEREQUERY, SQL_BIND_Params.TypeOf_WhereQuery)
                End If
            End If
        End If

        INIT_SQL_BIND = OUT
    End Function
    Private Sub ButtonFilter_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ButtonFilter.MouseClick
        If FPf.SAVE_ALL Then
            FORM_RECORDS_LOAD()
        End If
    End Sub

    Private Function FP_ORD_L() As FP
        Dim OUT As FP = Nothing
        Dim MyFP As FP

        For Each MyFP In FPf.FPs.Values
            If MyFP.FP_ALIAS = "ORD_L" Then
                OUT = MyFP
            End If
        Next
        Return OUT
    End Function
    Private Function FP_CONT() As FP
        Dim OUT As FP = Nothing
        Dim MyFP As FP

        For Each MyFP In FPf.FPs.Values
            If MyFP.FP_ALIAS = "CONT" Then
                OUT = MyFP
            End If
        Next
        Return OUT
    End Function

    Private Sub ButtonFilterLine_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ButtonFilterLine.MouseClick
        Dim Btn As FP_PictureBox = Me.PICTUREBOXES("Btn_Filter_Line")
        If Btn.ToggleButton Then
            If Not FP_CONT() Is Nothing Then
                FP_CONT.FORM_SubWHERE = SET_FP_Sub_Where_ON(FP_CONT)
                FP_CONT.FORM_RECORDS_LOAD(FP_CONT.FORM_SubWHERE, False, False, True)
            End If

            FP_ORD_L.FORM_SubWHERE = SET_FP_Sub_Where_ON(FP_ORD_L)
            FP_ORD_L.FORM_RECORDS_LOAD(FP_ORD_L.FORM_SubWHERE, False, False, True)
        End If
    End Sub

    Private Function FORM_LOGS_ACTIVITY_SET() As Boolean
        Dim OUT As Boolean = True

        If Not FORM_IsSubForm() Then
            OUT = DATA_LOGS_ACTIVITY_SET()
        End If

        FORM_LOGS_ACTIVITY_SET = OUT
    End Function
#End Region

    Private Sub BINDINGNAVIGATOR_FORM_PositionItem_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles BINDINGNAVIGATOR_FORM_PositionItem.TextChanged
        With BINDINGNAVIGATOR_FORM_PositionItem
            If .Focused Then
                If Val(.Text) > 0 Then
                    If DATA_GET_ID_FROM_SEQNUM(Val(.Text)) Then
                        FORM_GOTO_RECORD_BY_SeqNum(Val(.Text))
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub FilterText_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles FilterText.DoubleClick
        Dim ee As New System.Windows.Forms.MouseEventArgs(Nothing, 1, 0, 0, 0)

        ButtonFilter_MouseClick(ButtonFilter, ee)
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Private Sub XLS_IMPORT_Check_Data(ByVal sender As FP_XLS_Import) Handles XLS_IMPORT.Check_Data
        RAISEEVENT_EXCEL_IMPORT_Check_Data()
    End Sub

    Private Sub XLS_IMPORT_Data_Records_Prepared(ByVal sender As FP_XLS_Import, ByRef Cancel As Boolean) Handles XLS_IMPORT.Data_Records_Prepared
        RAISEEVENT_EXCEL_IMPORT_Data_Records_Prepared(Cancel)
    End Sub

    Private Sub XLS_IMPORT_Import_Data(ByVal sender As FP_XLS_Import, ByRef Cancel As Boolean) Handles XLS_IMPORT.Import_Data
        RAISEEVENT_EXCEL_IMPORT_Import_Data(Cancel)
    End Sub

    Public Function FORM_RECORDS_DUPLICATE_CURRENT() As Boolean
        Dim OUT As Boolean = False

        If FORM_RECORDS_SAVE_CURRENT() Then
            If P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                Dim DT_Clone As DataTable = DATA_DT_FORM_SQLFIELDS_FOR_READ.Copy

                If FORM_GOTO_NEWRECORD() Then
                    If FORM_DIRTY_SET() Then

                        DATA_DT_FORM_SQLFIELDS_FOR_READ = DT_Clone

                        FORM_BIND_DATA()
                        Dim DataBinded_old As Boolean = DATA_Binded

                        DATA_Binded = False
                        RaiseEvent Form_Record_Duplicated(Me)
                        DATA_Binded = DataBinded_old
                    End If
                End If
            End If
        End If

        Return OUT
    End Function

    Private Sub Button_Duplicate_Record_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button_Duplicate_Record.MouseUp
        FORM_RECORDS_DUPLICATE_CURRENT()
    End Sub

    Public Function LISTVIEW_INIT_FROM_FIXTEXT(ByRef MyListView_Params As Struct_LISTVIEW_Params) As Boolean
        Dim OUT As Boolean = True
        Dim FixText As String = FPf.FPApp.getFixText(MyListView_Params.FixText_Key)

        If FixText = "" Then
            FPf.FPApp.DoErrorMsgBox("FP_App.LISTVIEW_INIT_FROM_FIXTEXT", 0, String.Format("Fixtext_Key '{0}' not exists.", MyListView_Params.FixText_Key))
            OUT = False
        Else
            Dim Params_DIC As New Dictionary(Of String, String)

            FPf.FPApp.FIXTEXT_SPLIT_PARAMS(FixText, Params_DIC)

            OUT = (OUT And FPf.FPApp.FIXTEXT_CHK_PARAM(Params_DIC, MyListView_Params.FixText_Key, "CountOfColumns"))
            OUT = (OUT And FPf.FPApp.FIXTEXT_CHK_PARAM(Params_DIC, MyListView_Params.FixText_Key, "SELECT"))
            OUT = (OUT And FPf.FPApp.FIXTEXT_CHK_PARAM(Params_DIC, MyListView_Params.FixText_Key, "FROM"))
            OUT = (OUT And FPf.FPApp.FIXTEXT_CHK_PARAM(Params_DIC, MyListView_Params.FixText_Key, "VALUEMEMBER"))

            If OUT Then
                With MyListView_Params
                    If .ListViewControl Is Nothing Then
                        OUT = False
                        FPf.FPApp.DoErrorMsgBox("FP_App.LISTVIEW_INIT_FROM_FIXTEXT", 0, "MyListViewControl.ListViewControl is nothing.")
                    Else
                        With .ListViewControl
                            .View = View.Details
                            .Items.Clear()
                            .Columns.Clear()
                            '.CheckBoxes = .CheckBoxes
                            .HeaderStyle = ColumnHeaderStyle.Clickable
                            .Scrollable = True
                            .GridLines = True
                        End With

                        .CountOfColumns = Val(FPf.FPApp.FIXTEXT_getParam("COUNTOFCOLUMNS", Params_DIC))
                        If .CountOfColumns < 1 Then
                            OUT = False
                            FPf.FPApp.DoErrorMsgBox("FP_App.LISTVIEW_INIT_FROM_FIXTEXT", 0, String.Format("Fixtext_Key '{0}': Parameter CountOfColumns = 0", MyListView_Params.FixText_Key))
                        End If

                        If OUT = True Then
                            .ValueMember = FPf.FPApp.FIXTEXT_getParam("VALUEMEMBER", Params_DIC)
                            .SQL_SELECT = Replace(FPf.FPApp.FIXTEXT_getParam("SELECT", Params_DIC), "|", ",")
                            .SQL_FROM = FPf.FPApp.FIXTEXT_getParam("FROM", Params_DIC)
                            .SQL_WHERE = FPf.FPApp.FIXTEXT_getParam("WHERE", Params_DIC)
                            .SQL_ORDER_BY = FPf.FPApp.FIXTEXT_getParam("ORDERBY", Params_DIC)
                            .SEQ_KEY_HEADERS = FPf.FPApp.FIXTEXT_getParam("SEQ_KEY_HEADERS", Params_DIC)

                            .CheckBoxes = TEXT_Is_YES(FPf.FPApp.FIXTEXT_getParam("CHECKBOXES", Params_DIC))
                            .ListViewControl.CheckBoxes = .CheckBoxes

                            Select Case FPf.FPApp.FIXTEXT_getParam("REFRESH", Params_DIC)
                                Case "" : .REFRESH_Type = ENUM_FP_CONTROL_REFRESH_TYPE.Normal
                                Case "NORMAL" : .REFRESH_Type = ENUM_FP_CONTROL_REFRESH_TYPE.Normal
                                Case "FORM_CURRENT" : .REFRESH_Type = ENUM_FP_CONTROL_REFRESH_TYPE.On_Form_Current
                                Case "FORM_AFTERUPDATE" : .REFRESH_Type = ENUM_FP_CONTROL_REFRESH_TYPE.On_Form_AfterUpdate
                                Case Else
                                    .REFRESH_Type = ENUM_FP_CONTROL_REFRESH_TYPE.Normal
                                    FPf.FPApp.DoErrorMsgBox("FP_App.LISTVIEW_INIT_FROM_FIXTEXT", 0, String.Format("Unknown refresh type '{1}' (FixText Code: '{0}')", MyListView_Params.FixText_Key, FPf.FPApp.FIXTEXT_getParam("REFRESH", Params_DIC)))
                            End Select

                            With MyListView_Params.ListViewControl
                                .FullRowSelect = True
                                .MultiSelect = (Not .CheckBoxes)
                            End With

                            Dim DT As New DataTable
                            Dim MySQL As String = FPf.FPApp.SQL_getFROM_ELEMENTS("", "TOP 1 " + MyListView_Params.SQL_SELECT, MyListView_Params.SQL_FROM, MyListView_Params.SQL_WHERE, "", "")

                            MySQL = Replace(MySQL, "@RS_ID", RS_ID)
                            MySQL = Replace(MySQL, "@ID", P_DATA_Current_ID)
                            OUT = FPf.FPApp.DC.Qdf_Fill_DT(MySQL, DT)

                            If OUT Then
                                ReDim .ARRAY_SQL_Column_Names(.CountOfColumns)
                                ReDim .ARRAY_SQL_Column_XTypes(.CountOfColumns)

                                For i As Integer = 0 To .CountOfColumns - 1
                                    .ARRAY_SQL_Column_Names(i) = DT.Columns(i).ColumnName

                                    Select Case DT.Columns(i).DataType.ToString
                                        Case "System.String" : .ARRAY_SQL_Column_XTypes(i) = ""
                                        Case "System.Int32" : .ARRAY_SQL_Column_XTypes(i) = "INT"
                                        Case "System.Double" : .ARRAY_SQL_Column_XTypes(i) = "FLOAT"
                                        Case "System.Boolean" : .ARRAY_SQL_Column_XTypes(i) = "BIT"
                                        Case "System.DateTime" : .ARRAY_SQL_Column_XTypes(i) = "DATETIME"
                                        Case Else
                                            OUT = False
                                            FPf.FPApp.DoErrorMsgBox("FPApp.LISTVIEW_INIT_FROM_FIXTEXT", 0, String.Format("Column '{0}' has an unknown datatype", DT.Columns(i).ColumnName))
                                    End Select
                                Next

                                If Not DT.Columns.Contains(.ValueMember) Then
                                    OUT = False
                                    FPf.FPApp.DoErrorMsgBox("FP_App.LISTVIEW_INIT_FROM_FIXTEXT", 0, String.Format("Parameter 'VALUEMEMBER' does not exists in 'SELECT' list. (Fixtext_Key '{0}')", MyListView_Params.FixText_Key))
                                Else
                                    If DT.Columns(.ValueMember).DataType.Name <> "Int32" Then
                                        OUT = False
                                        FPf.FPApp.DoErrorMsgBox("FP_App.LISTVIEW_INIT_FROM_FIXTEXT", 0, String.Format("Type of column '{0}' (the VALUEMEMBER of the listview) is not INT. (Fixtext_Key '{1}')", .ValueMember, MyListView_Params.FixText_Key))
                                    End If
                                End If

                                Dim ARRAY_ColumnWidths_STR As String()

                                ReDim .ARRAY_ColumnWidths(.CountOfColumns)
                                ARRAY_ColumnWidths_STR = FPf.FPApp.FIXTEXT_getParam("COLUMNWIDTHS", Params_DIC).Split("|")
                                ReDim Preserve ARRAY_ColumnWidths_STR(.CountOfColumns)
                                .ARRAY_Aligns = FPf.FPApp.FIXTEXT_getParam("ALIGNS", Params_DIC).Split("|")
                                ReDim Preserve .ARRAY_Aligns(.CountOfColumns)
                                ReDim .ARRAY_HeaderTexts(.CountOfColumns)
                                .ARRAY_Formats = FPf.FPApp.FIXTEXT_getParam("FORMATS", Params_DIC).Split("|")
                                ReDim Preserve .ARRAY_Formats(.CountOfColumns)

                                Dim Header_SEQ As FP_SEQ = Nothing

                                If .SEQ_KEY_HEADERS > "" Then
                                    Header_SEQ = New FP_SEQ(FPf.FPApp, .SEQ_KEY_HEADERS)
                                End If

                                For i As Integer = 0 To .CountOfColumns - 1
                                    Dim NewColumn As New System.Windows.Forms.ColumnHeader

                                    If Trim(ARRAY_ColumnWidths_STR(i)) = "" Then
                                        .ARRAY_ColumnWidths(i) = 100
                                    Else
                                        .ARRAY_ColumnWidths(i) = Val(ARRAY_ColumnWidths_STR(i))
                                    End If
                                    NewColumn.Width = .ARRAY_ColumnWidths(i)

                                    If .SEQ_KEY_HEADERS = "" Then
                                        .ARRAY_HeaderTexts(i) = .ARRAY_SQL_Column_Names(i)
                                    Else
                                        .ARRAY_HeaderTexts(i) = Header_SEQ.GET_SEQ_BY_TEXT1(.ARRAY_SQL_Column_Names(i)).Text3
                                        If Trim(.ARRAY_HeaderTexts(i)) = "" Then
                                            .ARRAY_HeaderTexts(i) = .ARRAY_SQL_Column_Names(i)
                                        End If
                                    End If
                                    NewColumn.Text = .ARRAY_HeaderTexts(i)

                                    .ARRAY_Aligns(i) = Replace(.ARRAY_Aligns(i), Chr(34), "")
                                    If Trim(.ARRAY_Aligns(i)) = "" Then
                                        Select Case .ARRAY_SQL_Column_XTypes(i)
                                            Case "" : .ARRAY_Aligns(i) = "L"
                                            Case "INT" : .ARRAY_Aligns(i) = "R"
                                            Case "FLOAT" : .ARRAY_Aligns(i) = "R"
                                            Case "BIT" : .ARRAY_Aligns(i) = "M"
                                            Case "DATETIME" : .ARRAY_Aligns(i) = "L"
                                            Case Else
                                                FPf.FPApp.DoErrorMsgBox("FPApp.LISTVIEW_INIT_FROM_FIXTEXT", 0, String.Format("Column '{0}' has an unknown xtype {1}", DT.Columns(i).ColumnName, .ARRAY_SQL_Column_XTypes(i)))
                                        End Select
                                    End If

                                    Select Case Trim(.ARRAY_Aligns(i))
                                        Case "L" : NewColumn.TextAlign = HorizontalAlignment.Left
                                        Case "M" : NewColumn.TextAlign = HorizontalAlignment.Center
                                        Case "R" : NewColumn.TextAlign = HorizontalAlignment.Right
                                        Case Else
                                            FPf.FPApp.DoErrorMsgBox("FPApp.LISTVIEW_INIT_FROM_FIXTEXT", 0, String.Format("Column '{0}' has an unknown Align type {1}", DT.Columns(i).ColumnName, .ARRAY_Aligns(i)))
                                    End Select

                                    .ARRAY_Formats(i) = Replace(.ARRAY_Formats(i), Chr(34), "")

                                    .ListViewControl.Columns.Add(NewColumn)
                                Next
                            End If
                        End If
                    End If
                End With
            End If
        End If

        Return OUT
    End Function

    Protected Friend Function LISTVIEW_SQL_get(ByVal MyListView_Params As Struct_LISTVIEW_Params, FPc As FP_Control) As String
        Dim OUT As String = FPf.FPApp.SQL_getFROM_ELEMENTS("", MyListView_Params.SQL_SELECT, MyListView_Params.SQL_FROM, TEXT_AND(MyListView_Params.SQL_WHERE, MyListView_Params.WHERE2), "", MyListView_Params.SQL_ORDER_BY, FPc)

        OUT = Replace(OUT, "@ID", P_DATA_Current_ID)
        OUT = Replace(OUT, "@RS_ID", RS_ID)
        Return OUT
    End Function

    Public Function LISTVIEW_FILL(ByVal MyListView_Params As Struct_LISTVIEW_Params, FPc As FP_Control) As Boolean
        Dim OUT As Boolean = True
        Dim DT As New DataTable
        Dim MySQL As String = LISTVIEW_SQL_get(MyListView_Params, FPc)

        OUT = FPf.FPApp.DC.Qdf_Fill_DT(MySQL, DT)

        If OUT Then
            LISTVIEW_FILL(MyListView_Params, DT)
        End If

        Return OUT
    End Function

    Public Function LISTVIEW_FILL(ByVal MyListView_Params As Struct_LISTVIEW_Params, ByVal DT As DataTable) As Boolean
        Dim OUT As Boolean = True

        With MyListView_Params.ListViewControl
            .Items.Clear()

            If Not (DT Is Nothing) Then
                Dim IDX As Integer = 0

                For Each AktRow As DataRow In DT.Rows
                    IDX += 1

                    Dim Row_STR(MyListView_Params.CountOfColumns) As String

                    For i As Integer = 0 To MyListView_Params.CountOfColumns - 1
                        Dim CurrentFormat As String = MyListView_Params.ARRAY_Formats(i)

                        Select Case DT.Columns(i).DataType.ToString
                            Case "System.String"
                                Row_STR(i) = Format(nz(AktRow.Item(i), "").ToString, CurrentFormat)

                            Case "System.Int32"
                                Dim CurrVal As Integer = nz(AktRow.Item(i), 0)

                                If CurrVal = 0 Then
                                    Row_STR(i) = ""
                                Else
                                    If CurrentFormat > "" Then
                                        Row_STR(i) = Format(CurrVal, CurrentFormat)
                                    Else
                                        Row_STR(i) = getStrInt(CurrVal)
                                    End If
                                End If

                            Case "System.Double"
                                Dim CurrVal As Double = nz(AktRow.Item(i), 0)

                                If Math.Abs(CurrVal) < 0.0001 Then
                                    Row_STR(i) = ""
                                Else
                                    If CurrentFormat > "" Then
                                        Row_STR(i) = Format(CurrVal, CurrentFormat)
                                    Else
                                        Row_STR(i) = getStrFloat(CurrVal)
                                    End If
                                End If
                            Case "System.Boolean"
                                Dim CurrVal As Boolean = nz(AktRow.Item(i), 0)

                                If CurrVal = True Then
                                    Row_STR(i) = "+"
                                Else
                                    Row_STR(i) = ""
                                End If

                            Case "System.DateTime"
                                Dim CurrVal As DateTime = nz(AktRow.Item(i), NULLDATE)

                                If CurrVal = NULLDATE Then
                                    Row_STR(i) = ""
                                Else
                                    If CurrentFormat > "" Then
                                        Row_STR(i) = Format(CurrVal, CurrentFormat)
                                    Else
                                        Row_STR(i) = getStrDate(CurrVal)
                                    End If
                                End If

                            Case Else
                                FPf.FPApp.DoErrorMsgBox("FPApp.LISTVIEW_FILL", 0, String.Format("Column '{0}' has an unknown datatype", DT.Columns(i).ColumnName))
                                OUT = False
                        End Select
                    Next

                    Dim itm As New ListViewItem(Row_STR)
                    If MyListView_Params.ValueMember > "" Then
                        itm.Name = AktRow.Item(MyListView_Params.ValueMember)
                    Else
                        itm.Name = IDX.ToString
                    End If

                    .Items.Add(itm)
                Next
            End If
        End With

        Return OUT
    End Function

    Public Function LISTVIEW_FIND(ByRef c As ListView, ByVal FindValue As Long, Optional ByVal SelectNothingWhenNotFound As Boolean = True) As Boolean
        Dim OUT As Boolean = False

        If Not c.Items.ContainsKey(FindValue.ToString) Then
            If SelectNothingWhenNotFound Then
                c.FocusedItem = Nothing
            End If
        Else
            c.FocusedItem = c.Items(FindValue.ToString)
            OUT = True
        End If

        Return OUT
    End Function

    Private Sub FP_Active_Only_When_This_Field_Visible_VisibleChanged(sender As Object, e As EventArgs) Handles FP_Active_Only_When_This_Field_Visible.VisibleChanged
        If P_FP_Refresh_Field_Visible Then
            If Not (Parent_FP Is Nothing) Then
                If FP_Active_Only_When_This_Field_Visible_Current_Parent_ID <> Parent_FP.P_DATA_Current_ID Then
                    FORM_RECORDS_LOAD_FOR_PARENT_FP_CURRENT_RECORD()
                End If
            End If
        End If
    End Sub
End Class
