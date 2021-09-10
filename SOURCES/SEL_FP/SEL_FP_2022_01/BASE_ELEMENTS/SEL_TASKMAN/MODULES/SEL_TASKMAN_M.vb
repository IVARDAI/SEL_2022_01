Imports System.Data
Imports System.Data.SqlClient

Public Module SEL_TASKMAN_M
    Public Enum ENUM_TASKMAN_STATUSES As Integer
        NOT_DEFINED = 0
        NOT_PUBLISHED = -3000
        OPEN = -3001
        CLOSED = -3002
    End Enum

    Public Enum ENUM_TASKMAN_TASKTYPES_REMINDER_TYPES As Integer
        NONE = 0
        SCREEN_MESSAGE = 1
        EMAIL = 2
        SCREEN_MESSAGE_AND_EMAIL = 3
    End Enum

    Public Enum ENUM_TASKMAN_DEADLINE_TYPES As Integer
        DAYS_BEFORE = 0
        HOURS_BEFORE = 1
        MINUTES_BEFORE = 2
        FIX_DATE_MANUALY_DEFINED = 99
    End Enum

    Public Enum ENUM_TASKMAN_REMINDER_FOR_USERS_TYPES As Integer
        ONLY_ME = 0
        ALL_RESPONSIBLES = 1
        ONLY_FOR_SELECTED = 2
        EVERYBODY = 99
    End Enum

    Public Enum ENUM_TASKMAN_REMINDER_STATUS As Integer
        DELAYED_TOMORROW_MORNING = -101
        DELAYED = 99
        OK = 999
    End Enum

    Public Enum ENUM_TASKMAN_RIGHTS As Integer
        PUBLISH = 0
        TASK_TYPE_EDIT = 1
        DESCR_EDIT = 2
        DEADLINE_EDIT = 3
        RESPONSIBLES_EDIT = 4
        CONNECTED_RECORDS_EDIT = 5
        DOCMAN_ADD = 6
        DOCMAN_DELETE = 7
        DOCMAN_EDIT = 8
        TASK_CLOSE = 9
        CHAT = 10
        TASK_REOPEN = 11
    End Enum

    Public Structure STRUCT_TASKMAN_INFO
        Dim TASKS_Count As Integer
        Dim TASKS_Count_of_NEW As Integer
        Dim TASKS_Count_of_YELLOW As Integer
        Dim TASKS_Count_of_RED As Integer

        Dim REMINDERS_Count As Integer
        Dim REMINDERS_Count_of_NEW As Integer
        Dim REMINDERS_Count_of_SHOW_REMINDERS As Integer

        Dim HISTORY_Count_Of_NEW As Integer
    End Structure

    Public Structure STRUCT_TASKMAN_PREDEFINED_CONNECTED_RECORDS
        Dim TM_TASKS_ID As Long
        Dim TM_CONNECTED_RECORDS_TYPES_CODE As String
        Dim PARAM_TEXT As String
        Dim PARAM_INT As Long
    End Structure

    Public Function REMINDER_GET_DATE_FROM_DEADLINE_TYPE(DEADLINE_TYPE As ENUM_TASKMAN_DEADLINE_TYPES, Number_Of_Units As Integer, Date_of_Deadline As DateTime) As DateTime
        Dim OUT As DateTime = NULLDATE

        If Not (Date_of_Deadline = NULLDATE) Then
            Select Case DEADLINE_TYPE
                Case ENUM_TASKMAN_DEADLINE_TYPES.DAYS_BEFORE
                    OUT = DateAdd(DateInterval.Day, -Number_Of_Units, Date_of_Deadline)

                Case ENUM_TASKMAN_DEADLINE_TYPES.HOURS_BEFORE
                    OUT = DateAdd(DateInterval.Hour, -Number_Of_Units, Date_of_Deadline)

                Case ENUM_TASKMAN_DEADLINE_TYPES.MINUTES_BEFORE
                    OUT = DateAdd(DateInterval.Minute, -Number_Of_Units, Date_of_Deadline)

                Case ENUM_TASKMAN_DEADLINE_TYPES.FIX_DATE_MANUALY_DEFINED
                    'Nothing to do

                Case Else
                    gl_FPApp.DoErrorMsgBox("SEL_TASKMAN_M.REMINDER_GET_DATE_FROM_DEADLINE_TYPE", 0, String.Format("Unknown DEADLINE_TYPE ({0})", DEADLINE_TYPE))
            End Select
        End If

        Return OUT
    End Function

    Public Sub FPApp_MenuItem_Activated(ByVal sender As FP_MenuItem.Struct_FP_MenuItem_Params, ByRef Handled As Boolean)
        'Handles: "SEL_TASKMAN"
        '         "SEL_TASKMAN_GROUPS"
        '         "SEL_TASKMAN_TASK_TYPES"

        If Handled = False Then
            Select Case sender.Action
                Case "SEL_TASKMAN"
                    Handled = True
                    If gl_FPApp.FORMS_BringToFront("SEL_TASKMAN_FRM") Then
                        Dim FPf = gl_FPApp.FORMS_GET_FROM_NAME("SEL_TASKMAN_FRM")

                        FPf.LOCATION_MOVE_TO_SAVED_POS()
                    Else
                        Dim Frm As New SEL_TASKMAN_FRM
                        Frm.Show()
                    End If

                    If sender.OpenArgs > "" Then
                        Dim TM_FPf As FP_Form = gl_FPApp.FORMS_GET_FROM_NAME("SEL_TASKMAN_FRM")

                        If Not (TM_FPf Is Nothing) Then
                            Dim MyTaskMan_Frm As SEL_TASKMAN_FRM = TM_FPf.Frm
                            Select Case sender.OpenArgs
                                Case "TASKS"
                                    MyTaskMan_Frm.P_LAYOUT_TYPE = SEL_TASKMAN_FRM.ENUM_TASKMAN_FRM_LAYOUT_TYPE.TASKS

                                Case "REMINDERS"
                                    MyTaskMan_Frm.P_LAYOUT_TYPE = SEL_TASKMAN_FRM.ENUM_TASKMAN_FRM_LAYOUT_TYPE.REMINDERS

                                Case "HISTORY"
                                    MyTaskMan_Frm.P_LAYOUT_TYPE = SEL_TASKMAN_FRM.ENUM_TASKMAN_FRM_LAYOUT_TYPE.CHAT

                                Case Else
                                    'Nothing to do
                            End Select
                        End If
                    End If

                Case "SEL_TASKMAN_GROUPS"
                    Handled = True
                    If Not gl_FPApp.FORMS_BringToFront("SEL_TASKMAN_GROUPS") Then
                        Dim Frm As New SEL_TASKMAN_GROUPS
                        Frm.Show()
                    End If

                Case "SEL_TASKMAN_TASK_TYPES"
                    Handled = True
                    If Not gl_FPApp.FORMS_BringToFront("SEL_TASKMAN_TASK_TYPES") Then
                        Dim Frm As New SEL_TASKMAN_TASK_TYPES
                        Frm.Show()
                    End If

                Case Else
                    'Nothing to do
            End Select
        End If
    End Sub

    Public Sub FPApp_Marker_Clicked(Clicked_FPc As FP_Control, Action_Code As String, ByRef Individual_Params As Object, ByRef Handled As Boolean)
        If Handled = False Then
            Select Case Action_Code
                Case Else
                    'Nothing to do
            End Select
        End If
    End Sub

    Public Sub TASKS_SHOW(TASKS_ID As Long)
        If TASKS_ID <> 0 Then
            Dim TASK_frm As New SEL_TASKMAN_TASK(TASKS_ID)

            TASK_frm.Show()
        End If
    End Sub

    Public Function TM_INFO_GET(FPf As FP_Form, RESPOND_WHERE As String, Last_Tasks_ID As Long, Last_History_ID As Long, ByRef OUT_Info As STRUCT_TASKMAN_INFO) As Boolean
        Dim OUT As Boolean = False

        OUT_Info = New STRUCT_TASKMAN_INFO

        Dim sqlComm As SqlCommand = gl_FPApp.DC.CNN.CreateCommand()
        Dim Result As Boolean = False

        With gl_FPApp.DC
            .Qdf_set_SP(sqlComm, "TM_GET_INFO")
            .Qdf_AddParameter(sqlComm, "@Users_ID", SqlDbType.Int, , , , , SelUser)
            .Qdf_AddParameter(sqlComm, "@RESPOND_WHERE", SqlDbType.NVarChar, , -1, RESPOND_WHERE)
            .Qdf_AddParameter(sqlComm, "@Last_Tasks_ID", SqlDbType.Int, , , , , Last_Tasks_ID)
            .Qdf_AddParameter(sqlComm, "@Last_History_ID", SqlDbType.Int, , , , , Last_History_ID)
            .Qdf_AddParameter(sqlComm, "@OUT_TASKS_Count", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_TASKS_Count_of_NEW", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_TASKS_Count_of_YELLOW", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_TASKS_Count_of_RED", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_REMINDERS_Count", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_REMINDERS_Count_of_NEW", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_REMINDERS_Count_of_SHOW_REMINDERS", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_HISTORY_Count_Of_NEW", SqlDbType.Int, ParameterDirection.Output)
        End With

        CURSOR_SHOW_WAIT()
        Try
            Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
        Catch ex As Exception
            Result = False
            gl_FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        If Result Then
            With OUT_Info
                .TASKS_Count = sqlComm.Parameters("@OUT_TASKS_Count").Value
                .TASKS_Count_of_NEW = sqlComm.Parameters("@OUT_TASKS_Count_of_NEW").Value
                .TASKS_Count_of_YELLOW = sqlComm.Parameters("@OUT_TASKS_Count_of_YELLOW").Value
                .TASKS_Count_of_RED = sqlComm.Parameters("@OUT_TASKS_Count_of_RED").Value
                .REMINDERS_Count = sqlComm.Parameters("@OUT_REMINDERS_Count").Value
                .REMINDERS_Count_of_NEW = sqlComm.Parameters("@OUT_REMINDERS_Count_of_NEW").Value
                .HISTORY_Count_Of_NEW = sqlComm.Parameters("@OUT_HISTORY_Count_Of_NEW").Value
                .REMINDERS_Count_of_SHOW_REMINDERS = sqlComm.Parameters("@OUT_REMINDERS_Count_of_SHOW_REMINDERS").Value
            End With

            OUT = True
        End If

        Return OUT
    End Function

    Public Sub SEND_MESSAGE_TM_INFO(FPf As FP_Form, RESPOND_WHERE As String, Last_Tasks_ID As Long, Last_History_ID As Long)
        Dim I As New STRUCT_TASKMAN_INFO

        If TM_INFO_GET(FPf, RESPOND_WHERE, Last_Tasks_ID, Last_History_ID, I) Then
            gl_FPApp.RAISEEVENT_Message("TM_INFO", FPf, I, False)
        End If
    End Sub

    Public Function TM_CONNECTED_RECORDS_ADD(RECORD As STRUCT_TASKMAN_PREDEFINED_CONNECTED_RECORDS) As Boolean
        Dim OUT As Boolean = False
        Dim sqlComm As SqlCommand = gl_FPApp.DC.CNN.CreateCommand()

        With gl_FPApp.DC
            .Qdf_set_SP(sqlComm, "TM_CONNECTED_RECORDS_ADD")
            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            .Qdf_AddParameter(sqlComm, "@TM_TASKS_ID", SqlDbType.Int, , , , , RECORD.TM_TASKS_ID)
            .Qdf_AddParameter(sqlComm, "@TM_CONNECTED_RECORDS_TYPES_CODE", SqlDbType.NVarChar, , 128, RECORD.TM_CONNECTED_RECORDS_TYPES_CODE)
            .Qdf_AddParameter(sqlComm, "@PARAM_TEXT", SqlDbType.NVarChar, , 128, nz(RECORD.PARAM_TEXT, ""))
            .Qdf_AddParameter(sqlComm, "@PARAM_INT", SqlDbType.Int, , , , , RECORD.PARAM_INT)

            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
        End With

        CURSOR_SHOW_WAIT()
        Try
            OUT = gl_FPApp.DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
        Catch ex As Exception
            OUT = False
            gl_FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        Return OUT
    End Function

    Public Function TASKMAN_GET_RIGHT_FROM_STR(RightCode As ENUM_TASKMAN_RIGHTS, From_Right_Str As String) As Boolean
        Dim OUT As Boolean = False
        Dim RightCode_STR As String = String.Format("|{0}|", RightCode.ToString)

        If InStr(From_Right_Str, RightCode_STR) > 0 Then
            OUT = True
        End If

        Return OUT
    End Function

    Public Function TASKMAN_INSTALLED() As Boolean
        Return gl_FPApp.Installed_Products_Exists("TASKMAN")
    End Function
End Module
