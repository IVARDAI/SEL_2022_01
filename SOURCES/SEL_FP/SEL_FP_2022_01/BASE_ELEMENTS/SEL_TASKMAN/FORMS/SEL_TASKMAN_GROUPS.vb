Public Class SEL_TASKMAN_GROUPS
    Public WithEvents FPf As FP_Form
    Public WithEvents FP_GROUPS As FP
    Public WithEvents FP_GROUPS_L As FP

    Public FPc_Rights_INV_Allowed_Max As FP_Control
    Public FPc_Rights_INV_Allowed_Max_Curr_ID As FP_Control
    Public FPc_Rights_INV_Allowed_Max_Task As FP_Control
    Public FPc_Rights_CreditNote_Allowed_Max As FP_Control
    Public FPc_Rights_CreditNote_Allowed_Max_Curr_ID As FP_Control
    Public FPc_Rights_CreditNote_Allowed_Max_Task As FP_Control

    Public Sub New()
        InitializeComponent()

        FPf = New FP_Form("TM_GROUPS_BASE", gl_FPApp, Me, False)
        FP_GROUPS = New FP(FPf, "TM_GROUPS")
        FP_GROUPS_L = New FP(FPf, "TM_GROUPS_L", "", FP_GROUPS, "TM_GROUPS_ID")
    End Sub

    Private Sub SEL_TASKMAN_GROUPS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim FPf_CONTROLS As New Struct_FP_FORM_CONTROLS_COLLECTION

        With FPf_CONTROLS
            .Btn_HELP = BTN_Help
        End With
        FPf.INIT_CONTROLS(FPf_CONTROLS)

        Dim FP_GROUPS_CONTROLS As New Struct_FP_CONTROLS_COLLECTION

        With FP_GROUPS_CONTROLS
            With .GRID
                .GRID = GROUPS_GRID
                .Label = GROUPS_GRID_Label
            End With
        End With
        FP_GROUPS.INIT_CONTROLS(FP_GROUPS_CONTROLS)

        Dim FP_GROUPS_L_CONTROLS As New Struct_FP_CONTROLS_COLLECTION

        With FP_GROUPS_L_CONTROLS
            With .GRID
                .GRID = GROUPS_L_GRID
                .Label = GROUPS_L_GRID_Label
            End With
        End With
        FP_GROUPS_L.INIT_CONTROLS(FP_GROUPS_L_CONTROLS)

        FP_GROUPS.FORM_RECORDS_LOAD()
    End Sub

    Public Sub SET_LAYOUT()
        FPc_Rights_INV_Allowed_Max_Curr_ID.P.Mandatory = (Math.Abs(FPc_Rights_INV_Allowed_Max.P_VALUE) > 0.0005)
        FPc_Rights_CreditNote_Allowed_Max_Curr_ID.P.Mandatory = (Math.Abs(FPc_Rights_CreditNote_Allowed_Max.P_VALUE) > 0.0005)

        FP_GROUPS.COLORING_ALL()
    End Sub

    Private Sub FP_GROUPS_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_GROUPS.CONTROLS_INITIALIZED
        With FP_GROUPS
            FPc_Rights_INV_Allowed_Max = .CONTROLS("Rights_INV_Allowed_Max")
            FPc_Rights_INV_Allowed_Max_Curr_ID = .CONTROLS("Rights_INV_Allowed_Max_Curr_ID")
            FPc_Rights_INV_Allowed_Max_Task = .CONTROLS("Rights_INV_Allowed_Max_Task")
            FPc_Rights_CreditNote_Allowed_Max = .CONTROLS("Rights_CreditNote_Allowed_Max")
            FPc_Rights_CreditNote_Allowed_Max_Curr_ID = .CONTROLS("Rights_CreditNote_Allowed_Max_Curr_ID")
            FPc_Rights_CreditNote_Allowed_Max_Task = .CONTROLS("Rights_CreditNote_Allowed_Max_Task")
        End With
    End Sub

    Private Sub FP_GROUPS_Form_Current(sender_FP As FP) Handles FP_GROUPS.Form_Current
        SET_LAYOUT()
    End Sub

    Private Sub FP_GROUPS_Form_Field_AfterUpdate(FPc As FP_Control) Handles FP_GROUPS.Form_Field_AfterUpdate
        SET_LAYOUT()
    End Sub

    Private Sub FP_GROUPS_Form_NoRecord(sender_FP As FP) Handles FP_GROUPS.Form_NoRecord
        SET_LAYOUT()
    End Sub
End Class