Imports System.Data
Imports System.Data.SqlClient

Public Class SEL_TASKMAN_TASK
    Public WithEvents FPf As FP_Form
    Public WithEvents FP_TASKS As FP
    Public WithEvents FP_RESP As FP
    Public WithEvents FP_CONNECTED_RECORDS As FP

    Public TM_TASKS_ID As Long = 0

    Public WithEvents FPp_BTN_OK As FP_PictureBox
    Public FPp_BTN_Schedule As FP_PictureBox

    Public FPc_TaskNum As FP_Control
    Public FPc_Added_Users_Name As FP_Control
    Public FPc_Added_Users_ID As FP_Control
    Public FPc_Added_Date As FP_Control
    Public WithEvents FPc_TASK_TYPES_ID As FP_Control
    Public FPc_Descr As FP_Control
    Public FPc_DueDate As FP_Control
    Public WithEvents FPc_StatusID As FP_Control
    Public FPc_Connected_Records_GRID As FP_Control
    Public WithEvents FPc_Responsibles_GRID As FP_Control
    Public WithEvents FPp_BTN_REFRESH As FP_PictureBox
    Public FPc_TM_CONNECTED_RECORDS_TYPES_ID As FP_Control
    Private WithEvents FPc_Answer_YES As FP_Control
    Private WithEvents FPc_Answer_NO As FP_Control
    Public WithEvents FPc_PARAM_TEXT As FP_Control

    Public WithEvents FPc_CHAT_List As FP_Control
    Public WithEvents FPc_CHAT_Text As FP_Control

    Private DT_CONNECTED_RECORDS_TYPES As DataTable

    Public TM_DOCMAN As DOCMAN_Doc_Panel

    Private WithEvents FP_L_SCHEDULE_INFO As FP_L_Rtf_InfoField_With_PlusButton
    Private FPc_Schedule_INFO As FP_Control

    Private List_Of_Predefined_Connected_Records As New List(Of STRUCT_TASKMAN_PREDEFINED_CONNECTED_RECORDS)

    Public Sub New()
        InitializeComponent()

        INIT_Form()

        TM_TASKS_ID = 0
    End Sub

    Public Sub New(MyTM_TASKS_ID As Long)
        InitializeComponent()

        INIT_Form()

        TM_TASKS_ID = MyTM_TASKS_ID
    End Sub

    Public Sub New(To_Record As STRUCT_TASKMAN_PREDEFINED_CONNECTED_RECORDS)
        InitializeComponent()

        List_Of_Predefined_Connected_Records.Add(To_Record)

        INIT_Form()
    End Sub

    Private Sub INIT_Form()
        FPf = New FP_Form("TM_TASKS_BASE", gl_FPApp, Me, False)
        FP_TASKS = New FP(FPf, "TM_TASKS")
        FP_RESP = New FP(FPf, "TM_RESPONSIBLES", "", FP_TASKS, "TM_TASKS_ID")
        FP_CONNECTED_RECORDS = New FP(FPf, "TM_CONNECTED_RECORDS", "", FP_TASKS, "TM_TASKS_ID")

        Dim MySQL As String = "SELECT ID, Code, Marker_Code, SIMPLE_SELECT_CODE FROM TM_CONNECTED_RECORDS_TYPES"
        gl_FPApp.DC.Qdf_Fill_DT(MySQL, DT_CONNECTED_RECORDS_TYPES)
    End Sub

    Private Sub SEL_TASKMAN_TASK_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '---------------------------------------------------------------------------
        'FPf
        '---------------------------------------------------------------------------
        Dim FPf_CONTROLS As New Struct_FP_FORM_CONTROLS_COLLECTION
        With FPf_CONTROLS
            .Btn_HELP = BTN_HLP
        End With
        FPf.INIT_CONTROLS(FPf_CONTROLS)

        '---------------------------------------------------------------------------
        'FP_TASKS
        '---------------------------------------------------------------------------
        FP_TASKS.INIT_CONTROLS(Nothing)

        '---------------------------------------------------------------------------
        'FP_RESP
        '---------------------------------------------------------------------------
        Dim FP_RESP_CONTROLS As New Struct_FP_CONTROLS_COLLECTION
        With FP_RESP_CONTROLS
            .FieldPrefix = "TM_RESP_"
            With .GRID
                .GRID = TM_RESP_GRID
            End With
        End With
        FP_RESP.INIT_CONTROLS(FP_RESP_CONTROLS)

        '---------------------------------------------------------------------------
        'FP_CONNECTED_RECORDS
        '---------------------------------------------------------------------------
        Dim FP_CONNECTED_RECORDS_CONTROLS As New Struct_FP_CONTROLS_COLLECTION
        With FP_CONNECTED_RECORDS_CONTROLS
            .FieldPrefix = "TM_CON_REC_"
            With .GRID
                .GRID = TM_CON_REC_GRID
            End With
        End With
        FP_CONNECTED_RECORDS.INIT_CONTROLS(FP_CONNECTED_RECORDS_CONTROLS)

        '---------------------------------------------------------------------------
        'TM_DOCMAN
        '---------------------------------------------------------------------------
        Dim P_TM_DOCMAN As New DOCMAN_Doc_Panel.Struct_DOCMAN_Docs_Panel_CONTROL_COLLECTION

        With P_TM_DOCMAN
            .DOCMAN_Panel = ATTACHED_DOCS_Panel
            .FP_Parent = FP_TASKS
            .Subprefix = ""
            .Fieldprefix = ""
            .Parent_TableName = "TM_TASKS"
        End With

        TM_DOCMAN = New DOCMAN_Doc_Panel(P_TM_DOCMAN)


        '---------------------------------------------------------------------------
        'LOAD RECORDS
        '---------------------------------------------------------------------------
        If TM_TASKS_ID = 0 Then
            FP_TASKS.FORM_RECORDS_LOAD(, True)
        Else
            Dim Crit As String = String.Format("ID = {0}", TM_TASKS_ID)

            If Not FP_TASKS.FORM_RECORDS_LOAD(Crit) Then
                Me.Close()
            End If
        End If
    End Sub

    Private Sub FP_TASKS_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_TASKS.CONTROLS_INITIALIZED
        With FP_TASKS
            FPp_BTN_Schedule = .PICTUREBOXES("BTN_Schedule")

            FPc_TaskNum = .CONTROLS("TaskNum")
            FPc_Added_Users_Name = .CONTROLS("Added_Users_Name")
            FPc_TASK_TYPES_ID = .CONTROLS("TASK_TYPES_ID")
            FPc_Descr = .CONTROLS("Descr")
            FPc_Added_Date = .CONTROLS("Added_Date")
            FPc_DueDate = .CONTROLS("DueDate")
            FPc_StatusID = .CONTROLS("StatusID")
            FPc_CHAT_List = .CONTROLS("CHAT_List")
            FPc_CHAT_Text = .CONTROLS("CHAT_Text")
            FPc_Schedule_INFO = .CONTROLS("Schedule_INFO")
            FPc_Answer_YES = .CONTROLS("Answer_YES")
            FPc_Answer_NO = .CONTROLS("Answer_NO")
        End With

        FP_L_SCHEDULE_INFO = New FP_L_Rtf_InfoField_With_PlusButton(FP_TASKS, FPc_Schedule_INFO, FPp_BTN_Schedule)
    End Sub

    Public Sub FPc_CHAT_List_REFRESH()
        Dim MyWHERE2 As String = String.Format("TM_TASKS_ID = {0}", FP_TASKS.P_DATA_Current_ID)

        FPc_CHAT_List.P.DT_WHERE2 = MyWHERE2
        FPc_CHAT_List.DT_REFRESH()
    End Sub

    Private Sub REFRESH_TASKS_FRM()
        gl_FPApp.RAISEEVENT_Message("TM_REFRESH", FPf, Nothing, False)
    End Sub

    Private Sub ADD_Predefined_CONNECTED_RECORDS(Optional Without_DoResync As Boolean = False)
        'Amikor egy modul/form/egyeb valami letrehoz egy task-ot, akkor az a dilemma
        'alakul ki, hogy meg nincsen task, ugyanakkor ehhez a nemletezo task-hoz mar meg szeretnem adni a kapcsolodo rekordokat.
        'Ezert amikor letrehozol egy uj TASK-ot, akkor a New eljarasnak van egy To_Reocords nevezetu parametere,
        'ahol megadhatod, hogy melyik rekordokhoz fogod hozzafuzni a letrejovo TASK-ot.

        If List_Of_Predefined_Connected_Records.Count > 0 Then
            For i As Integer = 0 To List_Of_Predefined_Connected_Records.Count - 1
                Dim Current_Conn As New STRUCT_TASKMAN_PREDEFINED_CONNECTED_RECORDS

                With Current_Conn
                    .TM_TASKS_ID = FP_TASKS.P_DATA_Current_ID
                    .TM_CONNECTED_RECORDS_TYPES_CODE = List_Of_Predefined_Connected_Records(i).TM_CONNECTED_RECORDS_TYPES_CODE
                    .PARAM_INT = List_Of_Predefined_Connected_Records(i).PARAM_INT
                    .PARAM_TEXT = List_Of_Predefined_Connected_Records(i).PARAM_TEXT
                End With

                TM_CONNECTED_RECORDS_ADD(Current_Conn)
            Next i

            List_Of_Predefined_Connected_Records.Clear()

            If Without_DoResync = False Then
                FP_CONNECTED_RECORDS.FORM_DORESYNC(True)
                FP_CONNECTED_RECORDS.FORM_GOTO_NEWRECORD()
            End If
        End If
    End Sub

    Private Sub FP_TASKS_Form_AfterDelete(sender_FP As FP) Handles FP_TASKS.Form_AfterDelete
        REFRESH_TASKS_FRM()
    End Sub

    Private Sub FP_TASKS_Form_AfterUpdate(sender_FP As FP) Handles FP_TASKS.Form_AfterUpdate
        ADD_Predefined_CONNECTED_RECORDS(True)
        REFRESH_TASKS_FRM()
    End Sub

    Private Sub FP_TASKS_Form_Current(sender_FP As FP) Handles FP_TASKS.Form_Current
        If FP_TASKS.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
            FP_TASKS.P_FORM_AllowAdditions = False
        End If

        FPc_CHAT_List_REFRESH()
        FP_TASKS_SET_LAYOUT()
    End Sub

    Private Sub FP_TASKS_Form_Field_AfterUpdate(FPc As FP_Control) Handles FP_TASKS.Form_Field_AfterUpdate
        If FP_TASKS.P_DATA_RecordStatus = ENUM_RecordStatus.NEWRECORD Then
            If FPc_TASK_TYPES_ID.P_VALUE <> 0 And FPc_DueDate.P_VALUE <> NULLDATE Then
                FP_TASKS.FORM_RECORDS_SAVE_CURRENT()
                FPf.FOCUS_ON_AT_THE_END(Descr)
            End If
        Else
            'Nothing to do
        End If

        FP_TASKS_SET_LAYOUT()
    End Sub

    Private Sub FP_TASKS_Form_Field_Enter(FPc As FP_Control, ByRef Handled As Boolean) Handles FP_TASKS.Form_Field_Enter
        With FP_TASKS
            .Form_Field_Enter_Chk(FPc, FPc_TaskNum, Handled)
            .Form_Field_Enter_Chk(FPc, FPc_Added_Users_Name, Handled)
            .Form_Field_Enter_Chk(FPc, FPc_TASK_TYPES_ID, Handled)
            .Form_Field_Enter_Chk(FPc, FPc_Descr, Handled)
            .Form_Field_Enter_Chk(FPc, FPc_Added_Date, Handled)
            .Form_Field_Enter_Chk(FPc, FPc_DueDate, Handled)
            .Form_Field_Enter_Chk(FPc, FPc_StatusID, Handled)
        End With

        FP_TASKS_SET_LAYOUT()
    End Sub

    Private Sub FP_CONNECTED_RECORDS_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_CONNECTED_RECORDS.CONTROLS_INITIALIZED
        With FP_CONNECTED_RECORDS
            FPc_TM_CONNECTED_RECORDS_TYPES_ID = .CONTROLS("TM_CONNECTED_RECORDS_TYPES_ID")
            FPc_PARAM_TEXT = .CONTROLS("PARAM_TEXT")

            FPc_PARAM_TEXT.P_Marker = FP_Control.ENUM_Markertypes.Right_Arrow
        End With
    End Sub

    Public Sub FPc_PARAM_TEXT_RAISE_Marker_Click()
        If FP_CONNECTED_RECORDS.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            Dim Current_CONN_Type_ID As Long = FPc_TM_CONNECTED_RECORDS_TYPES_ID.P_VALUE

            If Current_CONN_Type_ID <> 0 Then
                Dim MyCrit As String = String.Format("ID = {0}", Current_CONN_Type_ID)
                Dim DRow As DataRow = DT_CONNECTED_RECORDS_TYPES.Select(MyCrit).First

                If DRow Is Nothing Then
                    FPf.DoErrorMsgBox("SEL_TASKMAN_TASK.FPc_PARAM_TEXT_RAISE_Marker_Click", 0, String.Format("Unknown TM_CONNECTED_RECORDS_TYPES.ID ({0})", Current_CONN_Type_ID))
                Else
                    Dim Curr_Marker As String = DRow!Marker_Code

                    If Curr_Marker > "" Then
                        gl_FPApp.RAISEEVENT_Marker_Clicked(FPf, FPc_PARAM_TEXT.Selected_ID, Curr_Marker, Nothing, False)
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub FPc_PARAM_TEXT_SET_SIMPLE_SELECT_PARAMS()
        If FP_CONNECTED_RECORDS.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            Dim Current_CONN_Type_ID As Long = FPc_TM_CONNECTED_RECORDS_TYPES_ID.P_VALUE

            If Current_CONN_Type_ID = 0 Then
                FPc_PARAM_TEXT.P.DT_FixText_Key = ""
            Else
                Dim MyCrit As String = String.Format("ID = {0}", Current_CONN_Type_ID)
                Dim DRow As DataRow = DT_CONNECTED_RECORDS_TYPES.Select(MyCrit).First

                If DRow Is Nothing Then
                    FPf.DoErrorMsgBox("SEL_TASKMAN_TASK.FPc_PARAM_TEXT_SET_SIMPLE_SELECT_PARAMS", 0, String.Format("Unknown TM_CONNECTED_RECORDS_TYPES.ID ({0})", Current_CONN_Type_ID))
                    FPc_PARAM_TEXT.P.DT_FixText_Key = ""
                Else
                    FPc_PARAM_TEXT.P.DT_FixText_Key = DRow!SIMPLE_SELECT_CODE
                End If
            End If
        End If
    End Sub

    Public ReadOnly Property P_Is_Task_Closed As Boolean
        Get
            Dim OUT As Boolean = False

            If FP_TASKS.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                OUT = (FPc_StatusID.P_VALUE = SEL_TASKMAN_M.ENUM_TASKMAN_STATUSES.CLOSED)
            End If

            Return OUT
        End Get
    End Property

    Public ReadOnly Property P_Has_Right(RightCode As ENUM_TASKMAN_RIGHTS) As Boolean
        Get
            Dim OUT As Boolean = False

            Select Case FP_TASKS.P_DATA_RecordStatus
                Case ENUM_RecordStatus.NORECORD
                    'Nothing to do

                Case ENUM_RecordStatus.NEWRECORD
                    Dim Rights_STR As String = "|TASK_TYPE_EDIT|TASK_DELETE|DEADLINE_EDIT|"

                    OUT = TASKMAN_GET_RIGHT_FROM_STR(RightCode, Rights_STR)

				Case ENUM_RecordStatus.EXISTS
                    Dim Rights_STR As String = FP_TASKS.DATA_Field_getSavedValue("Rights")

                    OUT = TASKMAN_GET_RIGHT_FROM_STR(RightCode, Rights_STR)
            End Select

            Return OUT
        End Get
    End Property

    Public ReadOnly Property P_Is_Task_Published As Boolean
        Get
            Dim OUT As Boolean = False

            If FP_TASKS.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                OUT = (FPc_StatusID.P_VALUE <> SEL_TASKMAN_M.ENUM_TASKMAN_STATUSES.NOT_PUBLISHED)
            End If

            Return OUT
        End Get
    End Property

    Public ReadOnly Property P_Type_of_Answer As CL_TASK_TYPES.ENUM_TASKMAN_TYPE_OF_ANSWER
        Get
            Dim OUT As CL_TASK_TYPES.ENUM_TASKMAN_TYPE_OF_ANSWER = CL_TASK_TYPES.ENUM_TASKMAN_TYPE_OF_ANSWER.OPEN_CLOSE
            Dim Current_TASK_TYPES_ID As Long = FPc_TASK_TYPES_ID.P_VALUE

            OUT = gl_FPApp.CL_TASK_TYPES.GET_Type_of_Answer(Current_TASK_TYPES_ID)

            Return OUT
        End Get
    End Property

    Private Sub FP_TASKS_SET_LAYOUT_Answer_Type()
        Dim BTN_PUBLISH_Visible As Boolean = False
        Dim BTN_CANCEL_Visible As Boolean = False
        Dim FPc_StatusID_Visible As Boolean = False
        Dim Answer_OPEN_CLOSE_Visible As Boolean = False
        Dim Answer_YN_Visible As Boolean = False
        Dim Answer_Locked As Boolean = True

        Select Case FP_TASKS.P_DATA_RecordStatus
            Case ENUM_RecordStatus.NORECORD
                'Nothing to do

            Case ENUM_RecordStatus.NEWRECORD
                BTN_PUBLISH_Visible = FP_TASKS.P_FORM_Dirty
                BTN_CANCEL_Visible = FP_TASKS.P_FORM_Dirty

            Case ENUM_RecordStatus.EXISTS
                Dim Is_Task_Published As Boolean = P_Is_Task_Published

                If Is_Task_Published = False Then
                    BTN_PUBLISH_Visible = True
                    BTN_CANCEL_Visible = True
                Else
                    Dim Current_TASK_TYPES_ID As Long = FPc_TASK_TYPES_ID.P_VALUE

                    If Current_TASK_TYPES_ID <> 0 Then
                        Dim Current_StatusID As SEL_TASKMAN_M.ENUM_TASKMAN_STATUSES = FPc_StatusID.P_VALUE
                        Dim Current_Answer_Type As CL_TASK_TYPES.ENUM_TASKMAN_TYPE_OF_ANSWER = P_Type_of_Answer
                        Dim Is_Task_Closed As Boolean = P_Is_Task_Closed

                        Select Case Current_Answer_Type
                            Case CL_TASK_TYPES.ENUM_TASKMAN_TYPE_OF_ANSWER.OPEN_CLOSE
                                Answer_OPEN_CLOSE_Visible = True
                                Answer_Locked = False

                            Case CL_TASK_TYPES.ENUM_TASKMAN_TYPE_OF_ANSWER.YES_NO
                                Answer_YN_Visible = True
                                Answer_Locked = Is_Task_Closed

                            Case Else
                                gl_FPApp.DoErrorMsgBox("SEL_TASKMAN_TASK.FP_TASKS_SET_LAYOUT_Answer_Type", 0, "Unknown type of answer")
                        End Select
                    End If
                End If
        End Select

        BTN_PUBLISH.Visible = BTN_PUBLISH_Visible
        BTN_CANCEL.Visible = BTN_CANCEL_Visible
        FPc_StatusID.P_VISIBLE = Answer_OPEN_CLOSE_Visible
        Answer_Panel.Visible = Answer_YN_Visible
        Answer_Locked = Answer_Locked
    End Sub

    Public Sub FP_TASKS_SET_LAYOUT()
        Select Case FP_TASKS.P_DATA_RecordStatus
            Case ENUM_RecordStatus.NORECORD
                FPc_StatusID.P_VISIBLE = False
                FPc_CHAT_Text.P.Locked = True

            Case ENUM_RecordStatus.NEWRECORD
                FPc_CHAT_Text.P.Locked = True

            Case ENUM_RecordStatus.EXISTS
                If CHAT_List.Equals(FPf.P_ActiveControl) Then
                    CHAT_Text.ForeColor = Drawing.Color.DarkGray
                    FPc_CHAT_Text.P.Locked = True
                Else
                    CHAT_Text.ForeColor = Drawing.Color.Black
                    FPc_CHAT_Text.P.Locked = False
                End If
        End Select

        FP_TASKS_SET_LAYOUT_Answer_Type()

        SET_LAYOUT_SET_RIGHT_BASED_PROPS()

        FP_TASKS.COLORING_ALL()
    End Sub

    Public Sub SET_LAYOUT_SET_RIGHT_BASED_PROPS()
        'RIGHT TASK_TYPE_EDIT
        Dim RIGHT_TASK_TYPE_EDIT = P_Has_Right(ENUM_TASKMAN_RIGHTS.TASK_TYPE_EDIT)
        FPc_TASK_TYPES_ID.P.Locked = (RIGHT_TASK_TYPE_EDIT = False)

        'RIGHT DESCR_EDIT
        Dim RIGHT_DESCR_EDIT As Boolean = P_Has_Right(ENUM_TASKMAN_RIGHTS.DESCR_EDIT)
        FPc_Descr.P.Locked = (RIGHT_DESCR_EDIT = False)

        'RIGHT RESPONSIBLES_EDIT
        Dim RIGHT_RESPONSIBLES_EDIT = P_Has_Right(ENUM_TASKMAN_RIGHTS.RESPONSIBLES_EDIT)
        With FP_RESP
            .P_FORM_AllowAdditions = RIGHT_RESPONSIBLES_EDIT
            .P_FORM_AllowDeletions = RIGHT_RESPONSIBLES_EDIT
            .P_FORM_AllowEdits = RIGHT_RESPONSIBLES_EDIT
        End With

        'RIGHT CONNECTED_RECORDS_EDIT
        Dim RIGHT_CONNECTED_RECORDS_EDIT = P_Has_Right(ENUM_TASKMAN_RIGHTS.CONNECTED_RECORDS_EDIT)
        With FP_CONNECTED_RECORDS
            .P_FORM_AllowAdditions = RIGHT_CONNECTED_RECORDS_EDIT
            .P_FORM_AllowDeletions = RIGHT_CONNECTED_RECORDS_EDIT
            .P_FORM_AllowEdits = RIGHT_CONNECTED_RECORDS_EDIT
        End With

        'RIGHT CHAT
        Dim RIGHT_CHAT = P_Has_Right(ENUM_TASKMAN_RIGHTS.CHAT)
        FPc_CHAT_Text.P.Locked = (RIGHT_CHAT = False)

        'RIGHT TASK_CLOSE
        If P_Is_Task_Published = False Then
            Dim RIGHT_TASK_PUBLISH = P_Has_Right(ENUM_TASKMAN_RIGHTS.PUBLISH)
            FPc_StatusID.P.Locked = (RIGHT_TASK_PUBLISH = False)
            FPc_Answer_YES.P.Locked = True
            FPc_Answer_NO.P.Locked = True
        Else
            Dim RIGHT_TASK_CLOSE = P_Has_Right(ENUM_TASKMAN_RIGHTS.TASK_CLOSE)
            FPc_StatusID.P.Locked = (RIGHT_TASK_CLOSE = False)
            FPc_Answer_YES.P.Locked = (RIGHT_TASK_CLOSE = False)
            FPc_Answer_NO.P.Locked = (RIGHT_TASK_CLOSE = False)
        End If

        'RIGHT DEADLINE_EDIT
        Dim RIGHT_DEADLINE_EDIT = P_Has_Right(ENUM_TASKMAN_RIGHTS.DEADLINE_EDIT)
        FPc_DueDate.P.Locked = (RIGHT_DEADLINE_EDIT = False)

        'RIGHT DOCMAN_ADD
        'RIGHT DOCMAN_DELETE
        'RIGHT DOCMAN_EDIT
        Dim RIGHT_DOCMAN_ADD = P_Has_Right(ENUM_TASKMAN_RIGHTS.DOCMAN_ADD)
        Dim RIGHT_DOCMAN_DELETE = P_Has_Right(ENUM_TASKMAN_RIGHTS.DOCMAN_DELETE)
        Dim RIGHT_DOCMAN_EDIT = P_Has_Right(ENUM_TASKMAN_RIGHTS.DOCMAN_EDIT)

        With TM_DOCMAN.FP_DOCMAN_Docs
            .P_FORM_AllowAdditions = RIGHT_DOCMAN_ADD
            .P_FORM_AllowDeletions = RIGHT_DOCMAN_DELETE
            .P_FORM_AllowEdits = RIGHT_DOCMAN_EDIT
        End With

    End Sub

    Public Sub FP_CONNECTED_RECORDS_SET_LAYOUT()
        FPc_PARAM_TEXT_SET_SIMPLE_SELECT_PARAMS()
    End Sub

    Private Sub FP_CONNECTED_RECORDS_Form_Current(sender_FP As FP) Handles FP_CONNECTED_RECORDS.Form_Current
        FP_CONNECTED_RECORDS_SET_LAYOUT()
    End Sub

    Private Sub FP_CONNECTED_RECORDS_Form_Field_AfterUpdate(FPc As FP_Control) Handles FP_CONNECTED_RECORDS.Form_Field_AfterUpdate
        FP_CONNECTED_RECORDS_SET_LAYOUT()
    End Sub

    Private Sub FP_CONNECTED_RECORDS_Form_NoRecord(sender_FP As FP) Handles FP_CONNECTED_RECORDS.Form_NoRecord
        FP_CONNECTED_RECORDS_SET_LAYOUT()
    End Sub

    Private Sub FPc_PARAM_TEXT_Field_Marker_Click(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Handled As Boolean) Handles FPc_PARAM_TEXT.Field_Marker_Click, FPc_PARAM_TEXT.Field_Doubleclick
        FPc_PARAM_TEXT_RAISE_Marker_Click()
    End Sub

    Public Sub CHAT_ADD(MessageText As String)
        If FP_TASKS.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            If FPf.SAVE_ALL Then
                If FP_TASKS.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                    If MessageText > "" Then
                        Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()
                        Dim Result As Boolean = False

                        With FPf.FPApp.DC
                            .Qdf_set_SP(sqlComm, "TM_CHATS_SAVE")
                            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                            .Qdf_AddParameter(sqlComm, "@ID", SqlDbType.Int, ParameterDirection.InputOutput, , , , 0)
                            .Qdf_AddParameter(sqlComm, "@OldTransactID", SqlDbType.Int, , , , , 0)

                            .Qdf_AddParameter(sqlComm, "@TM_TASKS_ID", SqlDbType.Int, , , , , FP_TASKS.P_DATA_Current_ID)
                            .Qdf_AddParameter(sqlComm, "@MessageText", SqlDbType.NVarChar, , -1, MessageText)

                            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
                            .Qdf_AddParameter(sqlComm, "@ErrField", SqlDbType.NVarChar, ParameterDirection.Output, 255)
                        End With

                        CURSOR_SHOW_WAIT()
                        Try
                            Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
                        Catch ex As Exception
                            FPf.FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
                        End Try

                        FPc_CHAT_List_REFRESH()
                        FPc_CHAT_Text.P_VALUE = ""

                        CURSOR_SHOW_DEFAULT()

                        REFRESH_TASKS_FRM()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub FPc_CHAT_Text_Field_KeyPreview_KeyDown(sender_FPc As FP_Control, sender As Object, ByRef e As Windows.Forms.KeyEventArgs) Handles FPc_CHAT_Text.Field_KeyPreview_KeyDown
        If e.KeyCode = Windows.Forms.Keys.Enter Then
            Dim ShiftPressed = My.Computer.Keyboard.ShiftKeyDown
            Dim CtrlPressed = My.Computer.Keyboard.CtrlKeyDown

            If ShiftPressed = False And CtrlPressed = False Then
                CHAT_ADD(FPc_CHAT_Text.P_VALUE)
            End If
        End If
    End Sub

    Private Sub FPf_CONTROLS_INITIALIZED(sender_FPf As FP_Form) Handles FPf.CONTROLS_INITIALIZED
        With FPf
            FPp_BTN_REFRESH = .PICTUREBOXES("BTN_REFRESH")
            FPp_BTN_OK = .PICTUREBOXES("BTN_OK")
        End With
    End Sub

    Private Sub FPp_BTN_REFRESH_CLICK(sender_FPc As FP_PictureBox, e As Windows.Forms.MouseEventArgs) Handles FPp_BTN_REFRESH.CLICK
        If FPf.SAVE_ALL Then
            FP_TASKS.FORM_DORESYNC()
        End If
    End Sub

    Private Sub FPp_BTN_OK_CLICK(sender_FPc As FP_PictureBox, e As Windows.Forms.MouseEventArgs) Handles FPp_BTN_OK.CLICK
        If FPc_StatusID.P_VALUE = ENUM_TASKMAN_STATUSES.NOT_PUBLISHED Or FPc_StatusID.P_VALUE = ENUM_TASKMAN_STATUSES.NOT_DEFINED Then
            FPc_StatusID.P_VALUE = ENUM_TASKMAN_STATUSES.OPEN
        End If

        If FPf.SAVE_ALL Then
            Me.Close()
        End If
    End Sub

    Private Sub BTN_PUBLISH_Click(sender As Object, e As EventArgs) Handles BTN_PUBLISH.Click
        If FPf.SAVE_ALL Then
            If FP_TASKS.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                If FP_TASKS.FORM_DIRTY_SET Then
                    FPc_StatusID.P_VALUE = SEL_TASKMAN_M.ENUM_TASKMAN_STATUSES.OPEN
                    FPf.SAVE_ALL()
                End If
            End If
        End If
    End Sub

    Private Sub FP_TASKS_Form_NoRecord(sender_FP As FP) Handles FP_TASKS.Form_NoRecord
        FP_TASKS_SET_LAYOUT()
    End Sub

    Public ReadOnly Property P_RESPONSIBLES_EXISTS As Boolean
        Get
            Dim OUT As Boolean = False
            Dim MySQL As String = String.Format("SELECT TOP 1 ID FROM TM_RESPONSIBLES WITH (READUNCOMMITTED) WHERE TM_TASKS_ID = {0}", FP_TASKS.P_DATA_Current_ID)
            Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)

            If Not (DRow Is Nothing) Then
                OUT = True
            End If

            Return OUT
        End Get
    End Property

    Public Function FPc_StatusID_SET_TO_OPEN() As Boolean
        Dim OUT As Boolean = False

        If FP_TASKS.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            Select Case FPc_StatusID.P_VALUE
                Case ENUM_TASKMAN_STATUSES.OPEN
                    OUT = True

                Case ENUM_TASKMAN_STATUSES.NOT_DEFINED, ENUM_TASKMAN_STATUSES.CLOSED
                    FPc_StatusID.P_VALUE = ENUM_TASKMAN_STATUSES.OPEN
                    OUT = True

                Case ENUM_TASKMAN_STATUSES.NOT_PUBLISHED
                    If P_RESPONSIBLES_EXISTS = False Then
                        If FPf.DoMyMsgBox(96003, , "SEQ,NO", "SEQ,YES") = 2 Then
                            FPc_StatusID.P_VALUE = ENUM_TASKMAN_STATUSES.OPEN
                            OUT = True
                        End If
                    End If

                Case Else
                    FPf.DoErrorMsgBox("SEL_TASKMAN_TASK.FPc_StatusID_SET_TO_OPEN", 0, String.Format("Unknown current status {0}", FPc_StatusID.P_VALUE))
            End Select
        End If

        Return OUT
    End Function

    Private Sub FPf_FORM_CLOSING(sender As Object, ByRef e As Windows.Forms.FormClosingEventArgs) Handles FPf.FORM_CLOSING
        If FPf.SAVE_ALL Then
            If FP_TASKS.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                If FPc_StatusID.P_VALUE = ENUM_TASKMAN_STATUSES.NOT_PUBLISHED Or FPc_StatusID.P_VALUE = ENUM_TASKMAN_STATUSES.NOT_DEFINED Then
                    Select Case FPf.DoMyMsgBox(96002, , "SEQ,YES", "SEQ,NO", "SEQ,CANCEL")
                        Case 1 'YES
                            If FPc_StatusID_SET_TO_OPEN() = False Then
                                e.Cancel = True
                            End If

                        Case 2 'NO
                            'Nothing to do

                        Case 3 'CANCEL
                            e.Cancel = True
                    End Select
                End If
            End If
        End If
    End Sub

    Private Sub FP_RESP_Form_AfterUpdate(sender_FP As FP) Handles FP_RESP.Form_AfterUpdate
        REFRESH_TASKS_FRM()
    End Sub

    Private Sub FPc_CHAT_List_Field_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc_CHAT_List.Field_TextChanged
        Dim CurrentText As String = ""
        Dim Crit As String = String.Format("ID = {0}", FPc_CHAT_List.P_VALUE)

        With FPc_CHAT_List.DT
            If .Select(Crit).Count > 0 Then
                CurrentText = .Select(Crit).First!MessageText_ORIG
            End If
        End With

        FPc_CHAT_Text.P_VALUE = CurrentText
    End Sub

    Private Sub CHAT_List_LostFocus(sender As Object, e As EventArgs) Handles CHAT_List.LostFocus
        FPc_CHAT_Text.P_VALUE = ""
    End Sub

    Private Sub FPc_StatusID_Field_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc_StatusID.Field_TextChanged
        If FPc_StatusID.P_VALUE = ENUM_TASKMAN_STATUSES.NOT_PUBLISHED Then
            If FP_TASKS.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                If FPc_StatusID.P_VALUE_Saved <> ENUM_TASKMAN_STATUSES.NOT_PUBLISHED Then
                    Cancel = True
                    FPf.DoMyMsgBox_AT_THE_END(96001) 'Nem allithatja vissza a feladatot NOT_PUBLISHED-re.
                End If
            End If
        End If
    End Sub

    Private Sub FP_L_SCHEDULE_INFO_EVENT_NAVIGATION_FORWARD(sender_FP_L As FP_L_Rtf_InfoField_With_PlusButton, ByRef Cancel As Integer) Handles FP_L_SCHEDULE_INFO.EVENT_NAVIGATION_FORWARD
        If FPf.SAVE_ALL Then
            If FP_TASKS.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                Dim Frm_Reminder As New SEL_TASKMAN_REMINDER_HISTORY(FP_TASKS.P_DATA_Current_ID)

                gl_FPApp.ShowDialogForm(Frm_Reminder, FPf)
                FP_TASKS.FORM_DORESYNC()
            End If
        End If
    End Sub

    Private Sub FPc_Answer_YES_Field_Coloring(sender_FPc As FP_Control, ByRef Handled As Boolean) Handles FPc_Answer_YES.Field_Coloring
        Handled = True

        Dim New_BackColor As Color = SystemColors.Control
        Dim New_ForeColor As Color = SystemColors.WindowText
        Dim IsYES As Boolean = (FPc_Answer_YES.P_VALUE = True)
        Dim IsNO As Boolean = (FPc_Answer_NO.P_VALUE = True)

        If IsYES = False And IsNO = False Then
            New_BackColor = Color.FromArgb(255, 228, 0)     'Color.Gold-hoz hasonlo, figyelemfelhivo szin
            New_ForeColor = Color.Black
        Else
            If P_Type_of_Answer = CL_TASK_TYPES.ENUM_TASKMAN_TYPE_OF_ANSWER.YES_NO Then
                If FPc_Answer_YES.P_VALUE = True Then
                    New_BackColor = Color.Green
                    New_ForeColor = Color.White
                End If
            End If
        End If

        With sender_FPc.c_Label
            .BackColor = New_BackColor
            .ForeColor = New_ForeColor
        End With
    End Sub

    Private Sub FPc_Answer_NO_Field_Coloring(sender_FPc As FP_Control, ByRef Handled As Boolean) Handles FPc_Answer_NO.Field_Coloring
        Handled = True

        Dim New_BackColor As Color = SystemColors.Control
        Dim New_ForeColor As Color = SystemColors.WindowText
        Dim IsYES As Boolean = (FPc_Answer_YES.P_VALUE = True)
        Dim IsNO As Boolean = (FPc_Answer_NO.P_VALUE = True)

        If IsYES = False And IsNO = False Then
            New_BackColor = Color.FromArgb(255, 228, 0)     'Color.Gold-hoz hasonlo, figyelemfelhivo szin
            New_ForeColor = Color.Black
        Else
            If P_Type_of_Answer = CL_TASK_TYPES.ENUM_TASKMAN_TYPE_OF_ANSWER.YES_NO Then
                If FPc_Answer_NO.P_VALUE = True Then
                    New_BackColor = Color.Red
                    New_ForeColor = Color.White
                End If
            End If
        End If

        With sender_FPc.c_Label
            .BackColor = New_BackColor
            .ForeColor = New_ForeColor
        End With
    End Sub

    Private Sub FPc_Answer_YES_Field_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc_Answer_YES.Field_TextChanged
        Answer_NO.Checked = (Not Answer_YES.Checked)
        FP_TASKS_SET_LAYOUT()

        If FPf.DoMyMsgBox(96014, , "SEQ,CANCEL", "SEQ,TASK_CLOSE") <> 2 Then
            Cancel = True

            FPc_Answer_YES.P_VALUE = False
            FP_TASKS_SET_LAYOUT()
        End If
    End Sub

    Private Sub FPc_Answer_NO_Field_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc_Answer_NO.Field_TextChanged
        Answer_YES.Checked = (Not Answer_NO.Checked)
        FP_TASKS_SET_LAYOUT()

        If FPf.DoMyMsgBox(96015, , "SEQ,CANCEL", "SEQ,TASK_CLOSE") <> 2 Then
            Cancel = True

            FPc_Answer_NO.P_VALUE = False
            FP_TASKS_SET_LAYOUT()
        End If
    End Sub

    Private Sub FPc_TASK_TYPES_ID_Field_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc_TASK_TYPES_ID.Field_TextChanged
        FP_TASKS_SET_LAYOUT()
    End Sub

    Private Sub BTN_CANCEL_Click(sender As Object, e As EventArgs) Handles BTN_CANCEL.Click
        FP_TASKS.FORM_RECORDS_DELETE_CURRENT(, True)
    End Sub
End Class