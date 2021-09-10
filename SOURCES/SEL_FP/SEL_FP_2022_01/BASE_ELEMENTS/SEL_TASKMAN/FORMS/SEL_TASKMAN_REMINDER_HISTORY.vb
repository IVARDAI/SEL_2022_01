Imports System.Data
Imports System.Data.SqlClient

Public Class SEL_TASKMAN_REMINDER_HISTORY
    Public WithEvents FPf As FP_Form
    Public WithEvents FP_REMINDERS As FP
    Public WithEvents FP_REMINDER_HISTORY As FP
    Public TM_TASKS_ID As Long
    Public WithEvents FPc_Selected As FP_Control

    Public Select_Checkbox As FP_L_GRID_RS_Select_Checkbox

    Public FPc_Added_User As FP_Control
    Public WithEvents FPc_Scheduled_Date_Relativ As FP_Control
    Public WithEvents FPc_Scheduled_Date_Relativ_Unit As FP_Control
    Public WithEvents FPc_Scheduled_Date As FP_Control
    Public FPc_Reminder_Type As FP_Control
    Public WithEvents FPc_Reminder_for_Users_Type_ID As FP_Control
    Public FPc_Remider_Function_By_Task_Close_ID As FP_Control

    Public Sub New(My_TM_TASKS_ID As Long)
        InitializeComponent()

        TM_TASKS_ID = My_TM_TASKS_ID

        FPf = New FP_Form("TM_REMINDERS_BASE", gl_FPApp, Me, False)
        FP_REMINDERS = New FP(FPf, "TM_REMINDERS")
        With FP_REMINDERS
            .FORM_SubWHERE_FIX = String.Format("TM_TASKS_ID={0}", TM_TASKS_ID)
        End With

        FP_REMINDER_HISTORY = New FP(FPf, "TM_SCHEDULES_USERS", "", FP_REMINDERS, "TM_SCHEDULES_ID")
        With FP_REMINDER_HISTORY
            With .SQL_BIND_Params
                .NameOf_DEL = ""
                .NameOf_SAVE = ""
            End With
            .P_FORM_AllowAdditions = False
            .P_FORM_AllowDeletions = False
            .P_FORM_AllowEdits = False
        End With
    End Sub

    Private Sub SEL_TASKMAN_REMINDER_HISTORY_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '----------------------------------------------------------------------------
        ' FPf
        '----------------------------------------------------------------------------
        Dim FPf_CONTROL_COLLECTION As New Struct_FP_FORM_CONTROLS_COLLECTION

        With FPf_CONTROL_COLLECTION
        End With

        FPf.INIT_CONTROLS(FPf_CONTROL_COLLECTION)

        '----------------------------------------------------------------------------
        ' FP_REMINDERS
        '----------------------------------------------------------------------------
        Dim FP_REMINDERS_CONTROL_COLLECTION As New Struct_FP_CONTROLS_COLLECTION

        With FP_REMINDERS_CONTROL_COLLECTION
            With .GRID
                .Label = SCHEDULE_GRID_Label
                .GRID = SCHEDULE_GRID
            End With
        End With

        FP_REMINDERS.INIT_CONTROLS(FP_REMINDERS_CONTROL_COLLECTION)

        '----------------------------------------------------------------------------
        ' FP_REMINDERS_HISTORY
        '----------------------------------------------------------------------------
        Dim FP_REMINDERS_HISTORY_CONTROL_COLLECTION As New Struct_FP_CONTROLS_COLLECTION

        With FP_REMINDERS_HISTORY_CONTROL_COLLECTION
            With .GRID
                .Label = SCHEDULED_USERS_GRID_Label
                .GRID = SCHEDULED_USERS_GRID
            End With
        End With
        FP_REMINDER_HISTORY.INIT_CONTROLS(FP_REMINDERS_HISTORY_CONTROL_COLLECTION)

        Dim Crit As String = String.Format("TM_TASKS_ID = {0}", TM_TASKS_ID)
        FP_REMINDERS.FORM_RECORDS_LOAD(Crit, False, True)
    End Sub

    Private Sub FP_REMINDERS_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_REMINDERS.CONTROLS_INITIALIZED
        With FP_REMINDERS
            FPc_Scheduled_Date_Relativ = .CONTROLS("Scheduled_Date_Relativ")
            FPc_Scheduled_Date_Relativ_Unit = .CONTROLS("Scheduled_Date_Relativ_Unit")
            FPc_Scheduled_Date = .CONTROLS("Scheduled_Date")
            FPc_Reminder_Type = .CONTROLS("Reminder_Type")
            FPc_Reminder_for_Users_Type_ID = .CONTROLS("Reminder_for_Users_Type_ID")
            FPc_Remider_Function_By_Task_Close_ID = .CONTROLS("Remider_Function_By_Task_Close_ID")
        End With
    End Sub

    Private Sub FP_REMINDERS_Form_BeforeUpdate(sender_FP As FP, ByRef Cancel As Integer) Handles FP_REMINDERS.Form_BeforeUpdate
        If FP_REMINDERS.P_DATA_RecordStatus = ENUM_RecordStatus.NEWRECORD Then
            FP_REMINDERS.DATA_Field_setValue("TM_TASKS_ID", TM_TASKS_ID.ToString)
        End If
    End Sub

    Private Sub FP_REMINDER_HISTORY_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_REMINDER_HISTORY.CONTROLS_INITIALIZED
        FPc_Selected = FP_REMINDER_HISTORY.CONTROLS("Selected")
        If Not (Select_Checkbox Is Nothing) Then
            Select_Checkbox.Dispose()
            Select_Checkbox = Nothing
        End If

        Select_Checkbox = New FP_L_GRID_RS_Select_Checkbox(FPc_Selected)
    End Sub

    Private Sub FP_REMINDER_HISTORY_Data_RS_SET_After(sender_FP As FP) Handles FP_REMINDER_HISTORY.Data_RS_SET_After
        Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

        FPf.FPApp.DC.Qdf_set_SP(sqlComm, "TM_SCHEDULES_USERS_RS_SET_After")

        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , FP_REMINDER_HISTORY.RS_ID)

        CURSOR_SHOW_WAIT()
        Try
            FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE)
        Catch ex As Exception
            FPf.FPApp.DoErrorMsgBox("FP_REMINDER_HISTORY.FP_REMINDER_HISTORY_Data_Records_Loading", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()
    End Sub

    Private Sub FP_REMINDER_HISTORY_Data_RS_SET_Before(sender_FP As FP) Handles FP_REMINDER_HISTORY.Data_RS_SET_Before
        Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

        FPf.FPApp.DC.Qdf_set_SP(sqlComm, "TM_SCHEDULES_USERS_PREPARE")

        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@TM_SCHEDULES_ID", SqlDbType.Int, , , , , FP_REMINDERS.P_DATA_Current_ID)

        CURSOR_SHOW_WAIT()
        Try
            FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
        Catch ex As Exception
            FPf.FPApp.DoErrorMsgBox("FP_REMINDER_HISTORY.FP_REMINDER_HISTORY_Data_Records_Loading", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()
    End Sub

    Public Sub FP_REMINDERS_SET_LAYOUT()
        If FPc_Scheduled_Date_Relativ_Unit.P_VALUE = ENUM_TASKMAN_DEADLINE_TYPES.FIX_DATE_MANUALY_DEFINED Then
            FPc_Scheduled_Date_Relativ.P.Mandatory = False
            FPc_Scheduled_Date_Relativ.P_TABSTOP = False
            FPc_Scheduled_Date.P_TABSTOP = True
        Else
            FPc_Scheduled_Date_Relativ.P.Mandatory = True
            FPc_Scheduled_Date_Relativ.P_TABSTOP = True
            FPc_Scheduled_Date.P_TABSTOP = False
        End If

        FP_REMINDERS.COLORING_ALL()
    End Sub

    Private Sub FP_REMINDERS_Form_Current(sender_FP As FP) Handles FP_REMINDERS.Form_Current
        If FP_REMINDERS.P_DATA_RecordStatus = ENUM_RecordStatus.NEWRECORD Then
            FPf.FOCUS_ON_AT_THE_END(FPc_Scheduled_Date_Relativ.c)
        End If
        FP_REMINDERS_SET_LAYOUT()
    End Sub

    Private ReadOnly Property P_TM_TASKS_Deadline As DateTime
        Get
            Dim OUT As DateTime = NULLDATE
            Static For_TM_TASKS_ID As Long = 0
            Static TM_TASKS_Deadline As DateTime = NULLDATE

            If For_TM_TASKS_ID = TM_TASKS_ID Then
                OUT = TM_TASKS_Deadline
            Else
                For_TM_TASKS_ID = TM_TASKS_ID
                TM_TASKS_Deadline = NULLDATE

                Dim MySQL As String = String.Format("SELECT DueDate FROM TM_TASKS WHERE ID = {0}", TM_TASKS_ID)
                Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)

                If DRow Is Nothing Then
                    FPf.DoErrorMsgBox("SEL_TASKMAN_REMINDER_HISTORY.P_TM_TASKS_Deadline", 0, String.Format("Task not found (TM_TASKS_ID = {0})", For_TM_TASKS_ID))
                Else
                    TM_TASKS_Deadline = nz(DRow!DueDate, NULLDATE)
                End If

                OUT = TM_TASKS_Deadline
            End If

            Return OUT
        End Get
    End Property

    Private Sub FPc_Scheduled_Date_SET()
        Dim NewDate As DateTime = REMINDER_GET_DATE_FROM_DEADLINE_TYPE(FPc_Scheduled_Date_Relativ_Unit.P_VALUE, FPc_Scheduled_Date_Relativ.P_VALUE, P_TM_TASKS_Deadline)

        FPc_Scheduled_Date.P_VALUE = NewDate
    End Sub

    Private Sub FP_REMINDERS_Form_Field_AfterUpdate(FPc As FP_Control) Handles FP_REMINDERS.Form_Field_AfterUpdate
        Dim From_Tabindex As Integer = FPc.c.TabIndex

        If FPc.c.Equals(FPc_Scheduled_Date_Relativ.c) Then
            If FPc_Scheduled_Date_Relativ.P_VALUE > 0 Then
                If FPc_Scheduled_Date_Relativ_Unit.P_VALUE = ENUM_TASKMAN_DEADLINE_TYPES.FIX_DATE_MANUALY_DEFINED Then
                    FPc_Scheduled_Date_Relativ_Unit.P_VALUE = ENUM_TASKMAN_DEADLINE_TYPES.DAYS_BEFORE
                End If
            End If

            FPc_Scheduled_Date_SET()
        End If

        If FPc.c.Equals(FPc_Scheduled_Date_Relativ_Unit.c) Then
            If FPc_Scheduled_Date_Relativ_Unit.P_VALUE = ENUM_TASKMAN_DEADLINE_TYPES.FIX_DATE_MANUALY_DEFINED Then
                FPc_Scheduled_Date_Relativ.P_VALUE = 0
            End If

            FPc_Scheduled_Date_SET()
        End If

        FP_REMINDERS_SET_LAYOUT()
    End Sub

    Private Sub FP_REMINDERS_Form_Field_Enter(FPc As FP_Control, ByRef Handled As Boolean) Handles FP_REMINDERS.Form_Field_Enter
        With FP_REMINDERS
            If Not (FPf.P_ActiveControl Is Nothing) Then
                If Not FPf.P_ActiveControl.c.Equals(FPc_Scheduled_Date_Relativ_Unit.c) Then
                    .Form_Field_Enter_Chk(FPc, FPc_Scheduled_Date_Relativ, Handled)
                End If
            End If
            .Form_Field_Enter_Chk(FPc, FPc_Scheduled_Date_Relativ_Unit, Handled)
            .Form_Field_Enter_Chk(FPc, FPc_Scheduled_Date, Handled)
            .Form_Field_Enter_Chk(FPc, FPc_Reminder_Type, Handled)
            .Form_Field_Enter_Chk(FPc, FPc_Reminder_for_Users_Type_ID, Handled)
            .Form_Field_Enter_Chk(FPc, FPc_Remider_Function_By_Task_Close_ID, Handled)
        End With
    End Sub

    Private Sub FP_REMINDERS_Form_NoRecord(sender_FP As FP) Handles FP_REMINDERS.Form_NoRecord
        FP_REMINDERS_SET_LAYOUT()
    End Sub

    Private Sub FPc_Scheduled_Date_Relativ_Field_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc_Scheduled_Date_Relativ.Field_TextChanged
        FP_REMINDERS_SET_LAYOUT()
    End Sub

    Private Sub FPc_Scheduled_Date_Relativ_Unit_Field_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc_Scheduled_Date_Relativ_Unit.Field_TextChanged
        FP_REMINDERS_SET_LAYOUT()
    End Sub

    Private Sub FPc_Selected_Field_BeforeUpdate(sender_FPc As FP_Control, ByRef Cancel As Integer) Handles FPc_Selected.Field_BeforeUpdate
        If FPc_Reminder_for_Users_Type_ID.P_VALUE <> ENUM_TASKMAN_REMINDER_FOR_USERS_TYPES.ONLY_FOR_SELECTED Then
            MsgBox("BU BA HUHA HUUUU")
        End If
    End Sub
End Class
