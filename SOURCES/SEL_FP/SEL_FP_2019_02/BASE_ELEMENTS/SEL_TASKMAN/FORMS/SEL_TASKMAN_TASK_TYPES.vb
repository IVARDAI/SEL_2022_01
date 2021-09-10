Public Class SEL_TASKMAN_TASK_TYPES
    Public WithEvents FPf As FP_Form
    Public WithEvents FP_TASK_TYPES As FP

    Public WithEvents FPc_Reminder_Type As FP_Control
    Public FPc_Reminder As FP_Control
    Public FPc_Reminder_Unit As FP_Control

    Public Sub New()
        InitializeComponent()

        FPf = New FP_Form("TM_TASK_TYPES_BASE", gl_FPApp, Me, False)
        FP_TASK_TYPES = New FP(FPf, "TM_TASK_TYPES")

    End Sub
    Private Sub SEL_TASKMAN_TASK_TYPES_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim FPf_CONTROLS As New Struct_FP_FORM_CONTROLS_COLLECTION

        With FPf_CONTROLS
            .Btn_HELP = BTN_Help
        End With
        FPf.INIT_CONTROLS(FPf_CONTROLS)

        Dim TM_TASK_TYPES_CONTROLS As New Struct_FP_CONTROLS_COLLECTION

        With TM_TASK_TYPES_CONTROLS
            With .GRID
                .GRID = TASK_TYPES_GRID
                .Label = TASK_TYPES_GRID_Label
            End With
        End With
        FP_TASK_TYPES.INIT_CONTROLS(TM_TASK_TYPES_CONTROLS)

        FP_TASK_TYPES.FORM_RECORDS_LOAD()
    End Sub

    Private Sub FP_TASK_TYPES_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_TASK_TYPES.CONTROLS_INITIALIZED
        With FP_TASK_TYPES
            FPc_Reminder_Type = .CONTROLS("Reminder_Type")
            FPc_Reminder = .CONTROLS("Reminder")
            FPc_Reminder_Unit = .CONTROLS("Reminder_Unit")
        End With
    End Sub

    Public Sub SET_LAYOUT()
        Dim Current_Reminder_Type As ENUM_TASKMAN_TASKTYPES_REMINDER_TYPES = FPc_Reminder_Type.P_VALUE

        Select Case Current_Reminder_Type
            Case ENUM_TASKMAN_TASKTYPES_REMINDER_TYPES.NONE
                FPc_Reminder.P_VISIBLE = False
                FPc_Reminder.P.Mandatory = False
                FPc_Reminder_Unit.P_VISIBLE = False

            Case ENUM_TASKMAN_TASKTYPES_REMINDER_TYPES.EMAIL,
                 ENUM_TASKMAN_TASKTYPES_REMINDER_TYPES.SCREEN_MESSAGE,
                 ENUM_TASKMAN_TASKTYPES_REMINDER_TYPES.SCREEN_MESSAGE_AND_EMAIL
                FPc_Reminder.P_VISIBLE = True
                FPc_Reminder.P.Mandatory = True
                FPc_Reminder_Unit.P_VISIBLE = True

            Case Else
                FPf.DoErrorMsgBox("SEL_TASKMAN_TASK_TYPES.SET_LAYOUT", 0, String.Format("Unknown REMINDER_TYPE ({0})", Current_Reminder_Type))
        End Select

        FP_TASK_TYPES.COLORING_ALL()
    End Sub

    Private Sub FP_TASK_TYPES_Form_Current(sender_FP As FP) Handles FP_TASK_TYPES.Form_Current
        SET_LAYOUT()
    End Sub

    Private Sub FP_TASK_TYPES_Form_Field_AfterUpdate(FPc As FP_Control) Handles FP_TASK_TYPES.Form_Field_AfterUpdate
        SET_LAYOUT()
    End Sub

    Private Sub FP_TASK_TYPES_Form_NoRecord(sender_FP As FP) Handles FP_TASK_TYPES.Form_NoRecord
        SET_LAYOUT()
    End Sub

    Private Sub FPc_Reminder_Type_Field_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc_Reminder_Type.Field_TextChanged
        SET_LAYOUT()
    End Sub
End Class