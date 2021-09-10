Imports System.Data
Imports System.Data.SqlClient

Public Class SEL_TASKMAN_FRM
    Public Enum ENUM_TASKMAN_FRM_LAYOUT_TYPE
        NORMAL = 0
        TASKS = 1
        CHAT = 2
        REMINDERS = 3
    End Enum

    Private WithEvents FPApp_for_Messages As FP_App
    Public WithEvents FPf As FP_Form = Nothing
    Public WithEvents FP_TASKMAN As FP = Nothing

    Public WithEvents FP_TASKS_OVERVIEW As FP
    Public WithEvents FP_HISTORY As FP
    Public WithEvents FP_REMINDERS As FP

    Public WithEvents FPp_BTN_TASKS As FP_PictureBox = Nothing
    Public WithEvents FPp_BTN_CHAT As FP_PictureBox = Nothing
    Public WithEvents FPp_BTN_REMINDERS As FP_PictureBox = Nothing

    Public WithEvents FPp_BTN_REFRESH As FP_PictureBox = Nothing
    Public WithEvents FPp_BTN_ADD_NEW As FP_PictureBox = Nothing
    Public WithEvents FPp_BTN_EDIT_TASK As FP_PictureBox = Nothing

    Private WithEvents FPc_LocalJobs_Status_ID As FP_Control
    Private WithEvents FPc_Scheduled_Date As FP_Control

    Private SEQ_TASKMAN_FRM As New FP_SEQ(gl_FPApp, "VBSEQ_SEL_TASKMAN_FRM_LOCAL")

    Private LAYOUT_TYPE As ENUM_TASKMAN_FRM_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.NORMAL

    Private RESPOND_WHERE As String = ""

    Dim FP_ImageList_for_Tasks As New FP_L_ImageList("")      'Tartalmazza a TASK-ok allapotahoz hasznalt kepek listajat. Nem kotelezo megadni.
    Private FPc_Status_Picture As FP_Control
    Private FPc_Status_Picture_PictureTextbox As FP_L_PictureTextbox

    Public WithEvents Frm_INFO As FP_Info

    Public WithEvents T As New Windows.Forms.Timer

    Private TASKMAN_INFO_HISTORY_CheckNum As Long = -1

    Private Last_Tasks_ID As Long = 0
    Private Last_History_ID As Long = 0

    Private FP_TASKS_OVERVIEW_Spec_Filter_ON As Boolean = False
    Private FP_HISTORY_Spec_Filter_ON As Boolean = False

    Public Sub T_START(Optional MyInterval_In_Milliseconds As Long = 300000)
        T.Enabled = False
        If MyInterval_In_Milliseconds < 5000 Then
            MyInterval_In_Milliseconds = 5000
        End If
        T.Interval = MyInterval_In_Milliseconds
        T.Enabled = True
    End Sub


    Public Sub T_START(Next_Scheduled_Date As DateTime)
        Dim Current_DateTime As DateTime = Now
        Dim Count_Of_Milliseconds As Long = DateDiff(DateInterval.Second, Now, Next_Scheduled_Date) * 1000

        T_START(Count_Of_Milliseconds)
    End Sub

    Public Sub T_STOP()
        T.Enabled = False
    End Sub

    Public Sub New()
        InitializeComponent()

        INIT_Form()
    End Sub

    Private Sub INIT_Form()
        FPApp_for_Messages = gl_FPApp
        FPf = New FP_Form("FP_TASKMAN_BASE", gl_FPApp, Me, False)
        FP_TASKMAN = New FP(FPf, "FP_TASKMAN", , True)
        FP_TASKS_OVERVIEW = New FP(FPf, "TM_TASKS_OVERVIEW")
        With FP_TASKS_OVERVIEW
            With .SQL_BIND_Params
                .NameOf_DEL = ""
                .NameOf_SAVE = ""
            End With

            .P_FORM_AllowDeletions = False
            .P_FORM_AllowEdits = False
            .P_FORM_AllowAdditions = False
        End With

        Dim MySQL As String = String.Format("SELECT dbo.TM_GET_WHERE_FOR_USER_RESPONSIBLES({0}) SQL_WHERE", SelUser)
        Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)
        RESPOND_WHERE = DRow!SQL_WHERE

        FP_HISTORY = New FP(FPf, "TM_HISTORY")
        With FP_HISTORY
            With .SQL_BIND_Params
                .NameOf_DEL = ""
                .NameOf_SAVE = ""
            End With

            .P_FORM_AllowDeletions = False
            .P_FORM_AllowEdits = False
            .P_FORM_AllowAdditions = False
        End With

        FP_REMINDERS = New FP(FPf, "TM_TASKS_REMINDERS")
        With FP_REMINDERS
            With .SQL_BIND_Params
                .NameOf_DEL = ""
            End With

            .P_FORM_AllowDeletions = False
            .P_FORM_AllowAdditions = False
        End With
    End Sub

    Public Property P_LAYOUT_TYPE As ENUM_TASKMAN_FRM_LAYOUT_TYPE
        Get
            Return LAYOUT_TYPE
        End Get
        Set(value As ENUM_TASKMAN_FRM_LAYOUT_TYPE)
            Select Case value
                Case ENUM_TASKMAN_FRM_LAYOUT_TYPE.NORMAL
                    If P_LAYOUT_TYPE <> ENUM_TASKMAN_FRM_LAYOUT_TYPE.NORMAL Then
                        FRM_SIZE_SAVE_TO_PFD()
                        LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.NORMAL
                        FPp_BTN_TASKS.P_PRESSED = False
                        FPp_BTN_CHAT.P_PRESSED = False
                        FPp_BTN_REMINDERS.P_PRESSED = False
                        FRM_SIZE_SET_FROM_PFD()
                        SET_LAYOUT()
                    End If

                Case ENUM_TASKMAN_FRM_LAYOUT_TYPE.TASKS
                    If P_LAYOUT_TYPE <> ENUM_TASKMAN_FRM_LAYOUT_TYPE.TASKS Then
                        FRM_SIZE_SAVE_TO_PFD()
                        LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.TASKS
                        FPp_BTN_TASKS.P_PRESSED = True
                        FPp_BTN_CHAT.P_PRESSED = False
                        FPp_BTN_REMINDERS.P_PRESSED = False
                        FRM_SIZE_SET_FROM_PFD()
                        SET_LAYOUT()
                    End If

                Case ENUM_TASKMAN_FRM_LAYOUT_TYPE.CHAT
                    If P_LAYOUT_TYPE <> ENUM_TASKMAN_FRM_LAYOUT_TYPE.CHAT Then
                        FRM_SIZE_SAVE_TO_PFD()
                        LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.CHAT
                        FPp_BTN_TASKS.P_PRESSED = False
                        FPp_BTN_CHAT.P_PRESSED = True
                        FPp_BTN_REMINDERS.P_PRESSED = False
                        FRM_SIZE_SET_FROM_PFD()
                        SET_LAYOUT()
                    End If

                Case ENUM_TASKMAN_FRM_LAYOUT_TYPE.REMINDERS
                    If P_LAYOUT_TYPE <> ENUM_TASKMAN_FRM_LAYOUT_TYPE.REMINDERS Then
                        FRM_SIZE_SAVE_TO_PFD()
                        LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.REMINDERS
                        FPp_BTN_TASKS.P_PRESSED = False
                        FPp_BTN_CHAT.P_PRESSED = False
                        FPp_BTN_REMINDERS.P_PRESSED = True
                        FRM_SIZE_SET_FROM_PFD()
                        SET_LAYOUT()
                    End If

                Case Else
                    FPf.DoErrorMsgBox("SEL_TASKMAN_FRM.P_LAYOUT_TYPE", 0, String.Format("Unknown Layout Type ({0})", value))
            End Select
        End Set
    End Property

    Private ReadOnly Property P_PARAMS_PFD_KEY As String
        Get
            Return String.Format("SEL_TASKMAN_FRM_PARAMS_{0}", CInt(P_LAYOUT_TYPE))
        End Get
    End Property

    Private Sub Last_Tasks_ID_SET()
        If FP_TASKS_OVERVIEW_Spec_Filter_ON = False Then
            If FP_TASKS_OVERVIEW.DATA_DT_FORM.Rows.Count = 0 Then
                Last_Tasks_ID = 0
            Else
                Last_Tasks_ID = FP_TASKS_OVERVIEW.DATA_DT_FORM.Compute("MAX(RecordID)", "")
            End If
        End If
    End Sub

    Private Sub Last_History_ID_SET()
        If FP_HISTORY_Spec_Filter_ON = False Then
            If FP_HISTORY.DATA_DT_FORM.Rows.Count = 0 Then
                Last_History_ID = gl_FPApp.NachsteNummerVergeben
            Else
                Last_History_ID = FP_HISTORY.DATA_DT_FORM.Compute("MAX(RecordID)", "")
            End If
        End If
    End Sub

    Private Sub SEL_TASKMAN_FRM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '--------------------------------------------------------------------------------
        'FPf
        '--------------------------------------------------------------------------------
        Dim FPf_CONTROLS_COLLECTION As New Struct_FP_FORM_CONTROLS_COLLECTION

        With FPf_CONTROLS_COLLECTION
            .Btn_HELP = BTN_HLP
        End With

        FPf.INIT_CONTROLS(FPf_CONTROLS_COLLECTION)

        FP_TASKMAN.INIT_CONTROLS(Nothing)

        '--------------------------------------------------------------------------------
        'FP_TASKS_OVERVIEW
        '--------------------------------------------------------------------------------
        Dim FP_TASKS_OVERVIEW_CONTROLS As New Struct_FP_CONTROLS_COLLECTION
        With FP_TASKS_OVERVIEW_CONTROLS
            .FieldPrefix = "OVERVIEW_"
            With .GRID
                .Label = TASKS_Title_Label
                .GRID = TASKS_GRID
            End With
            .Btn_ExportToExcel = TASKS_Excel_Export
        End With
        FP_TASKS_OVERVIEW.INIT_CONTROLS(FP_TASKS_OVERVIEW_CONTROLS)

        '--------------------------------------------------------------------------------
        'FP_HISTORY
        '--------------------------------------------------------------------------------
        Dim FP_HISTORY_CONTROLS As New Struct_FP_CONTROLS_COLLECTION
        With FP_HISTORY_CONTROLS
            .FieldPrefix = "HISTORY_"
            With .GRID
                .Label = CHATS_Title_Label
                .GRID = CHAT_GRID
            End With
            .Btn_ExportToExcel = HISTORY_Excel_Export
        End With
        FP_HISTORY.INIT_CONTROLS(FP_HISTORY_CONTROLS)

        '--------------------------------------------------------------------------------
        'FP_REMINDERS
        '--------------------------------------------------------------------------------
        Dim FP_REMINDERS_CONTROLS As New Struct_FP_CONTROLS_COLLECTION
        With FP_REMINDERS_CONTROLS
            .FieldPrefix = "REMINDERS_"
            With .GRID
                .Label = REMINDERS_Title_Label
                .GRID = REMINDERS_GRID
            End With
            .Btn_ExportToExcel = REMINDERS_Excel_Export
        End With
        FP_REMINDERS.INIT_CONTROLS(FP_REMINDERS_CONTROLS)

        '--------------------------------------------------------------------------------
        'Load records
        '--------------------------------------------------------------------------------
        FRM_SIZE_SET_FROM_PFD()

        Dim FP_TASKS_OVERVIEW_Crit As String = String.Format("TASK_Closed = 0 AND {0}", RESPOND_WHERE)
        FP_TASKS_OVERVIEW.FORM_RECORDS_LOAD(FP_TASKS_OVERVIEW_Crit, , True, True)
        Last_Tasks_ID_SET()

        Dim FP_HISTORY_Crit As String = String.Format("Date_Of_Event > DATEADD(DAY, -1, GETDATE()) AND {0}", RESPOND_WHERE)
        FP_HISTORY.FORM_RECORDS_LOAD(FP_HISTORY_Crit, , True, True)
        Last_History_ID_SET()

        Dim FP_REMINDERS_Crit As String = String.Format("Reminder_Status_ID < 999 AND Users_ID = {0} AND Scheduled_Date < GETDATE()", SelUser)
        FP_REMINDERS.FORM_RECORDS_LOAD(FP_REMINDERS_Crit, , True, True)

        '--------------------------------------------------------------------------------
        'Timer elinditasa
        '--------------------------------------------------------------------------------
        T_START(5000)
    End Sub

    Public Sub SET_LAYOUT()
        Panel_TASKS.Visible = (P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.TASKS)
        Panel_CHAT.Visible = (P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.CHAT)
        Panel_REMINDERS.Visible = (P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.REMINDERS)

        FPp_BTN_ADD_NEW.P_VISIBLE = (P_LAYOUT_TYPE <> ENUM_TASKMAN_FRM_LAYOUT_TYPE.NORMAL)
        FPp_BTN_EDIT_TASK.P_VISIBLE = (P_LAYOUT_TYPE <> ENUM_TASKMAN_FRM_LAYOUT_TYPE.NORMAL)

        If FPp_BTN_TASKS.P_PRESSED Then
        ElseIf FPp_BTN_CHAT.P_PRESSED Then
        ElseIf FPp_BTN_REMINDERS.P_PRESSED Then

        Else
        End If
    End Sub

    Private Sub FPf_CONTROLS_INITIALIZED(sender_FPf As FP_Form) Handles FPf.CONTROLS_INITIALIZED
        With FPf
            FPp_BTN_TASKS = .PICTUREBOXES("BTN_TASKS")
            FPp_BTN_CHAT = .PICTUREBOXES("BTN_CHAT")
            FPp_BTN_REFRESH = .PICTUREBOXES("BTN_REFRESH")
            FPp_BTN_REMINDERS = .PICTUREBOXES("BTN_REMINDERS")
        End With
    End Sub

    Private Sub FPp_BTN_TASKS_CLICK(sender_FPc As FP_PictureBox, e As Windows.Forms.MouseEventArgs) Handles FPp_BTN_TASKS.CLICK
        If P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.TASKS Then
            P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.NORMAL
        Else
            P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.TASKS
            REFRESH_TM_INFO()
        End If
    End Sub

    Private Sub FPp_BTN_CHAT_CLICK(sender_FPc As FP_PictureBox, e As Windows.Forms.MouseEventArgs) Handles FPp_BTN_CHAT.CLICK
        If P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.CHAT Then
            P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.NORMAL
        Else
            P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.CHAT
            FP_HISTORY_REFRESH(True)
            REFRESH_TM_INFO()
        End If
    End Sub

    Public Sub REFRESH_TM_INFO()
        SEND_MESSAGE_TM_INFO(FPf, RESPOND_WHERE, Last_Tasks_ID, Last_History_ID)
    End Sub

    Private Sub FPp_BTN_REMINDERS_CLICK(sender_FPc As FP_PictureBox, e As Windows.Forms.MouseEventArgs) Handles FPp_BTN_REMINDERS.CLICK
        If P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.REMINDERS Then
            P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.NORMAL
        Else
            P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.REMINDERS
            REFRESH_TM_INFO()
        End If
    End Sub

    Public Sub FRM_SIZE_SAVE_TO_PFD()
        If Me.WindowState = Windows.Forms.FormWindowState.Normal Then
            Dim Params_STR As String = String.Format("{0}|{1}", Me.Width, Me.Height)

            gl_FPApp.PFDinsertOrUpdate(P_PARAMS_PFD_KEY, Params_STR)
        End If
    End Sub

    Public Sub FRM_SIZE_SET_FROM_PFD()
        Dim Size_STR As String = ""

        If gl_FPApp.PFDlesen(P_PARAMS_PFD_KEY, Size_STR) Then
            Dim Size_Koo() As String = Split(Size_STR, "|")

            If UBound(Size_Koo) >= 1 Then
                Dim Size_Width As Integer = Val(Size_Koo(0))
                Dim Size_Height As Integer = Val(Size_Koo(1))

                If Size_Width > 0 And Size_Height > 0 Then
                    Me.Size = New Drawing.Size(Size_Width, Size_Height)
                    FPf.CONTROLS_ARRANGE_ALL()
                End If
            End If
        End If
    End Sub

    Private Sub FPf_FORM_CLOSING(sender As Object, ByRef e As Windows.Forms.FormClosingEventArgs) Handles FPf.FORM_CLOSING
        If gl_FPApp.P_RUNNING_STATE = FP_App.ENUM_FP_APP_RUNNING_STATE.ON_CLOSING Then
            FRM_SIZE_SAVE_TO_PFD()
            Dispose_WithEvents_Elements()
        Else
            FRM_SIZE_SAVE_TO_PFD()
            FPf.LOCATION_SAVE()
            e.Cancel = True
            Me.WindowState = FormWindowState.Minimized
        End If
    End Sub

    Private Sub Dispose_WithEvents_Elements()
        FPApp_for_Messages = Nothing
    End Sub

    Private Sub FP_TASKMAN_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_TASKMAN.CONTROLS_INITIALIZED
        With FP_TASKMAN
            FPp_BTN_ADD_NEW = .PICTUREBOXES("BTN_ADD_NEW")
            FPp_BTN_EDIT_TASK = .PICTUREBOXES("BTN_EDIT_TASK")
        End With
    End Sub

    Private Sub FPp_BTN_ADD_NEW_CLICK(sender_FPc As FP_PictureBox, e As Windows.Forms.MouseEventArgs) Handles FPp_BTN_ADD_NEW.CLICK
        Dim Frm_SEL_TASKMAN_TASK As New SEL_TASKMAN_TASK

        Frm_SEL_TASKMAN_TASK.Show()
    End Sub

    Private Sub FP_TASKS_OVERVIEW_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_TASKS_OVERVIEW.CONTROLS_INITIALIZED
        With FP_TASKS_OVERVIEW
            FPc_Status_Picture = .CONTROLS("Status_Picture")
        End With
        FPc_Status_Picture_PictureTextbox = New FP_L_PictureTextbox(FPc_Status_Picture)
        FPc_Status_Picture_PictureTextbox.BITMAPS_ADD(FP_ImageList_for_Tasks)
    End Sub

    Private Sub FP_TASKS_OVERVIEW_Form_Current(sender_FP As FP) Handles FP_TASKS_OVERVIEW.Form_Current
        SET_LAYOUT()
    End Sub

    Private Sub FP_TASKS_OVERVIEW_Form_NoRecord(sender_FP As FP) Handles FP_TASKS_OVERVIEW.Form_NoRecord
        SET_LAYOUT()
    End Sub

    Private Sub FP_TASKS_OVERVIEW_GRID_Row_DoubleClick(sender_FP As FP, ByRef Handled As Boolean) Handles FP_TASKS_OVERVIEW.GRID_Row_DoubleClick
        FP_TASKS_OVERVIEW_SHOW_CURRENT_TASK()
    End Sub

    Private Sub FP_REMINDERS_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_REMINDERS.CONTROLS_INITIALIZED
        With FP_REMINDERS
            FPc_LocalJobs_Status_ID = .CONTROLS("LocalJobs_Status_ID")
            FPc_Scheduled_Date = .CONTROLS("Scheduled_Date")
        End With
    End Sub

    Private Sub FP_REMINDERS_Form_AfterUpdate(sender_FP As FP) Handles FP_REMINDERS.Form_AfterUpdate
        REFRESH_TM_INFO()
    End Sub

    Private Sub FP_REMINDERS_Form_Current(sender_FP As FP) Handles FP_REMINDERS.Form_Current
        SET_LAYOUT()
    End Sub

    Private Function FPc_LocalJobs_Status_ID_GET_MINUTES_TO_ADD() As Integer
        Dim OUT As Integer = 0

        If FP_REMINDERS.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            Select Case FPc_LocalJobs_Status_ID.P_VALUE
                Case ENUM_TASKMAN_REMINDER_STATUS.OK
                    'Nothing to do

                Case ENUM_TASKMAN_REMINDER_STATUS.DELAYED
                    'Nothing to do

                Case ENUM_TASKMAN_REMINDER_STATUS.DELAYED_TOMORROW_MORNING
                    'Nothing to do

                Case Else 'Calculates Scheduled_Date based on Minutes
                    Dim Crit As String
                    Dim DRow As DataRow

                    With FPc_LocalJobs_Status_ID
                        Crit = String.Format("ID = {0}", .P_VALUE)
                        DRow = .DT.Select(Crit).First
                        OUT = DRow!Minutes
                    End With
            End Select
        End If

        Return OUT
    End Function

    Private Sub FPc_Scheduled_Date_SET()
        If FP_REMINDERS.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            Dim New_Scheduled_Date As DateTime = NULLDATE

            Select Case FPc_LocalJobs_Status_ID.P_VALUE
                Case ENUM_TASKMAN_REMINDER_STATUS.OK
                    'Nothing to do

                Case ENUM_TASKMAN_REMINDER_STATUS.DELAYED
                    'Nothing to do

                Case ENUM_TASKMAN_REMINDER_STATUS.DELAYED_TOMORROW_MORNING
                    New_Scheduled_Date = DateAdd(DateInterval.Day, 1, gl_FPApp.GET_SERVER_CURRENT_DATE)

                Case Else 'Calculates Scheduled_Date based on Minutes
                    Dim Minutes_Add As Integer = FPc_LocalJobs_Status_ID.P_VALUE
                    Dim Server_DateTime As DateTime = gl_FPApp.GET_SERVER_CURRENT_DATE(True)

                    If Minutes_Add > 0 Then
                        New_Scheduled_Date = DateAdd(DateInterval.Minute, Minutes_Add, Server_DateTime)
                    End If
            End Select

            If New_Scheduled_Date <> NULLDATE Then
                FPc_Scheduled_Date.P_VALUE = New_Scheduled_Date
            End If
        End If
    End Sub
    Private Sub FP_REMINDERS_Form_Field_AfterUpdate(FPc As FP_Control) Handles FP_REMINDERS.Form_Field_AfterUpdate
        Dim From_Tabindex As Integer = FPc.c.TabIndex

        If FPc.c.Equals(FPc_LocalJobs_Status_ID.c) Then
            FPc_Scheduled_Date_SET()
        End If
    End Sub

    Private Sub FP_REMINDERS_Form_NoRecord(sender_FP As FP) Handles FP_REMINDERS.Form_NoRecord
        SET_LAYOUT()
    End Sub

    Private Sub FP_REMINDERS_GRID_Row_DoubleClick(sender_FP As FP, ByRef Handled As Boolean) Handles FP_REMINDERS.GRID_Row_DoubleClick
        FP_REMINDERS_SHOW_CURRENT_TASK()
    End Sub

    Private Sub FPp_BTN_EDIT_TASK_CLICK(sender_FPc As FP_PictureBox, e As Windows.Forms.MouseEventArgs) Handles FPp_BTN_EDIT_TASK.CLICK
        Select Case LAYOUT_TYPE
            Case ENUM_TASKMAN_FRM_LAYOUT_TYPE.NORMAL
                FPf.DoErrorMsgBox("SEL_TASKMAN_FRM..FPp_BTN_EDIT_TASK_CLICK", 0, "Normal nezetben nem szabadna latszania ennek a gombnak.")

            Case ENUM_TASKMAN_FRM_LAYOUT_TYPE.CHAT
                FP_HISTORY_SHOW_CURRENT_TASK()

            Case ENUM_TASKMAN_FRM_LAYOUT_TYPE.TASKS
                FP_TASKS_OVERVIEW_SHOW_CURRENT_TASK()

            Case Else
                FPf.DoErrorMsgBox("SEL_TASKMAN_FRM..FPp_BTN_EDIT_TASK_CLICK", 0, "Unknown LAYOUT_TYPE")
        End Select
    End Sub

    Public Sub FP_TASKS_OVERVIEW_SHOW_CURRENT_TASK()
        If FP_TASKS_OVERVIEW.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then

            CURSOR_SHOW_WAIT()

            Dim TASKS_ID As Long = FP_TASKS_OVERVIEW.P_DATA_Current_ID
            TASKS_SHOW(TASKS_ID)

            CURSOR_SHOW_DEFAULT()

        End If
    End Sub

    Public Sub FP_HISTORY_SHOW_CURRENT_TASK()
        If FP_HISTORY.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then

            CURSOR_SHOW_WAIT()

            Dim TASKS_ID As Long = Val(FP_HISTORY.DATA_Field_getValue_FromREAD("TM_TASKS_ID"))
            TASKS_SHOW(TASKS_ID)

            CURSOR_SHOW_DEFAULT()

        End If
    End Sub

    Private Sub FPp_BTN_REFRESH_CLICK(sender_FPc As FP_PictureBox, e As Windows.Forms.MouseEventArgs) Handles FPp_BTN_REFRESH.CLICK
        FP_TASKS_OVERVIEW_REFRESH(True)
        FP_HISTORY_REFRESH(True)
        FP_REMINDERS_REFRESH(True)

        REFRESH_TM_INFO()
    End Sub

    Public Sub FP_TASKS_OVERVIEW_REFRESH(Always As Boolean)
        If Always Or P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.TASKS Then
            Dim FP_TASKS_OVERVIEW_Current_ID As Long = FP_TASKS_OVERVIEW.P_DATA_Current_ID

            FP_TASKS_OVERVIEW.FORM_DORESYNC(True)

            If FP_TASKS_OVERVIEW_Current_ID <> 0 Then
                FP_TASKS_OVERVIEW.FORM_GOTO_RECORD_BY_ID(FP_TASKS_OVERVIEW_Current_ID)
            End If

            Last_Tasks_ID_SET()
        End If
    End Sub

    Public Sub FP_HISTORY_REFRESH(Always As Boolean)
        If Always Or P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.CHAT Then
            Dim FP_HISTORY_Current_ID As Long = FP_HISTORY.P_DATA_Current_ID

            FP_HISTORY.FORM_DORESYNC(True)

            If FP_HISTORY_Current_ID <> 0 Then
                FP_HISTORY.FORM_GOTO_RECORD_BY_ID(FP_HISTORY_Current_ID)
            End If

            Last_History_ID_SET()
        End If
    End Sub

    Public Sub SHOW_REMINDERS()
        Dim Crit As String = "Reminder_status = 0"

        If FP_REMINDERS.GRID.DT_ALL_FIELDS.Select(Crit).Count > 0 Then
            Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()
            Dim Result As Boolean = False

            FPf.FPApp.DC.Qdf_set_SP(sqlComm, "TM_TASKS_REMINDERS_SET_REMINDER_T_READED")
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , FP_REMINDERS.RS_ID)

            CURSOR_SHOW_WAIT()
            Try
                Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
            Catch ex As Exception
                Result = False
                FPf.FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
            End Try

            CURSOR_SHOW_DEFAULT()

            Dim wINFO_P As New FP_Info.Struct_SEL_INFO_Params

            With wINFO_P
                .MsgNumber = 96005 'Uj emlekeztetok erkeztek
                .MsgParams = ""
                .SubPrefix = ""
            End With

            Frm_INFO = New FP_Info(gl_FPApp, wINFO_P)
        End If
    End Sub

    Public Sub FP_REMINDERS_REFRESH(Always As Boolean)
        If Always Or P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.REMINDERS Then
            Dim FP_REMINDERS_Current_ID As Long = FP_REMINDERS.P_DATA_Current_ID

            FP_REMINDERS.FORM_DORESYNC(True)

            If FP_REMINDERS_Current_ID <> 0 Then
                FP_REMINDERS.FORM_GOTO_RECORD_BY_ID(FP_REMINDERS_Current_ID)
            End If

            Last_History_ID_SET()

            SHOW_REMINDERS()
        End If
    End Sub

    Public Sub TASKMAN_INFO_SET(TASKS_Count As Integer, TASKS_NEW_Count As Integer)
        TASKS_INFO.Text = TASKS_Count.ToString

        Dim INFO_NEW_SEQ As String = SEQ_TASKMAN_FRM.GET_SEQ_BY_TEXT1("TASKS_INFO_NEW").Text3

        INFO_NEW_SEQ = Replace(INFO_NEW_SEQ, "@1", TASKS_NEW_Count.ToString)
        TASKS_INFO_NEW.Text = INFO_NEW_SEQ

        TASKS_INFO.Visible = True
        TASKS_INFO_NEW.Visible = (TASKS_NEW_Count <> 0)
    End Sub

    Public Sub REMINDER_INFO_SET(REMINDERS_Count As Integer, REMINDERS_NEW_Count As Integer)
        REMINDERS_INFO.Text = REMINDERS_Count.ToString

        Dim INFO_NEW_SEQ As String = SEQ_TASKMAN_FRM.GET_SEQ_BY_TEXT1("REMINDERS_INFO_NEW").Text3

        INFO_NEW_SEQ = Replace(INFO_NEW_SEQ, "@1", REMINDERS_NEW_Count.ToString)
        REMINDERS_INFO_NEW.Text = INFO_NEW_SEQ

        REMINDERS_INFO.Visible = True
        REMINDERS_INFO_NEW.Visible = (REMINDERS_NEW_Count <> 0)
    End Sub

    Public Sub HISTORY_INFO_SET(HISTORY_NEW_Count As Integer)
        HISTORY_INFO.Text = HISTORY_NEW_Count.ToString

        HISTORY_INFO.Visible = (HISTORY_NEW_Count <> 0)
    End Sub

    Private Sub FPApp_for_Messages_Message(sender As FP_App, From_FPf As FP_Form, MessageCode As String, ByRef Individual_Params As Object, ByRef Handled As Boolean) Handles FPApp_for_Messages.Message
        Select Case MessageCode
            Case "TM_INFO"
                'Handled-et nem kell True-ra allitani, mert tobb objektum is feldolgozhatja ezt az uzenetet

                Dim I As STRUCT_TASKMAN_INFO = Individual_Params

                TASKMAN_INFO_SET(I.TASKS_Count, I.TASKS_Count_of_NEW)

                If I.REMINDERS_Count_of_SHOW_REMINDERS > 0 Then
                    FP_REMINDERS_REFRESH(True)
                End If
                REMINDER_INFO_SET(I.REMINDERS_Count, I.REMINDERS_Count_of_NEW)
                HISTORY_INFO_SET(I.HISTORY_Count_Of_NEW)

            Case "TM_REFRESH"
                FP_TASKS_OVERVIEW_REFRESH(True)
                FP_HISTORY_REFRESH(False)
                FP_REMINDERS_REFRESH(False)
                SEL_TASKMAN_M.SEND_MESSAGE_TM_INFO(FPf, RESPOND_WHERE, Last_Tasks_ID, Last_History_ID)

            Case Else
                'Nothing to do
        End Select
    End Sub

    Private Sub FP_HISTORY_GRID_Row_DoubleClick(sender_FP As FP, ByRef Handled As Boolean) Handles FP_HISTORY.GRID_Row_DoubleClick
        FP_HISTORY_SHOW_CURRENT_TASK()
    End Sub

    Public Sub FP_REMINDERS_SHOW_CURRENT_TASK()
        If FP_REMINDERS.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then

            CURSOR_SHOW_WAIT()

            Dim TASKS_ID As Long = Val(FP_REMINDERS.DATA_Field_getValue_FromREAD("TM_TASKS_ID"))
            TASKS_SHOW(TASKS_ID)

            CURSOR_SHOW_DEFAULT()

        End If
    End Sub

    Private Sub Frm_INFO_FormClosed(sender As Object, e As Windows.Forms.FormClosedEventArgs) Handles Frm_INFO.FormClosed
        Frm_INFO.Dispose()
        Frm_INFO = Nothing
    End Sub

    Private Sub Frm_INFO_FP_Info_Details_Click(sender As FP_Info) Handles Frm_INFO.FP_Info_Details_Click
        P_LAYOUT_TYPE = ENUM_TASKMAN_FRM_LAYOUT_TYPE.REMINDERS
        If Me.WindowState <> FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Normal
        End If
    End Sub

    Private Sub T_Tick(sender As Object, e As EventArgs) Handles T.Tick
        Static w1 As Integer = 0

        T_STOP()

        REFRESH_TM_INFO()

        T_START()
    End Sub
End Class

