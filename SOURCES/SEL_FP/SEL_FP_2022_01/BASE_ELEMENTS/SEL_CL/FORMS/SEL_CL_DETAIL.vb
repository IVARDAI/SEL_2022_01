Public Class SEL_CL_DETAIL

#Region "DECLARE"

    Private _Cust_ID As Integer
    Private _Mon_Calc As Boolean
    Private _Mon_Inv As Boolean
    Private _Mon_Inv_Exp As Boolean
    Private _Mon_Cost As Boolean

    Private WithEvents FPf As FP_Form
    Private WithEvents FP_CALC As FP
    Private WithEvents FP_INV As FP

    Private FPp_BTN_View As FP_PictureBox

    Private SplitContainer As FP_L_SplitContainer_Resize

    Private WithEvents FPc_CALC_OrderNum As FP_Control
    Private WithEvents FPc_INV_InvoiceNo As FP_Control

#End Region

#Region "CLASS CONSTRUCTOR"

    Public Sub New(ByVal Cust_ID As Integer)
        InitializeComponent()

        _Cust_ID = Cust_ID

        Dim DT As DataTable = Nothing

        If gl_FPApp.DC.Qdf_Fill_DT(String.Format("SELECT CL_Mon_Calc, CL_Mon_Inv, CL_Mon_Inv_Exp, CL_Mon_Cost FROM Cegek WHERE ID={0}", _Cust_ID), DT) Then
            _Mon_Calc = DT.Rows(0).Item(0)
            _Mon_Inv = DT.Rows(0).Item(1)
            _Mon_Inv_Exp = DT.Rows(0).Item(2)
            _Mon_Cost = DT.Rows(0).Item(3)
        End If

        FPf = New FP_Form("CL_DETAIL_FORM", gl_FPApp, Me, False)
        FP_CALC = New FP(FPf, "CL_DETAIL_CALC")
        FP_INV = New FP(FPf, "CL_DETAIL_INV")
    End Sub

#End Region

#Region "FORM EVENTS"

    Private Sub SEL_CL_DETAIL_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not gl_FPApp.InitGlobals Then
            Me.Close()
        Else
            Dim FPF_CONTROLS As New Struct_FP_FORM_CONTROLS_COLLECTION

            With FPF_CONTROLS
                .Btn_HELP = BTN_Help
                FPf.INIT_CONTROLS(FPF_CONTROLS)
            End With

            Dim FP_CALC_CONTROLS As New Struct_FP_CONTROLS_COLLECTION

            With FP_CALC_CONTROLS
                With .GRID
                    .GRID = Calc_GRID
                    .Label = Calc_GRID_Label
                End With

                With FP_CALC.SQL_BIND_Params
                    .NameOf_READ = "CL_DETAIL_CALC_GRID"
                    .NameOf_SAVE = ""
                    .NameOf_DEL = ""
                    .NameOf_GRID = "CL_DETAIL_CALC_GRID"
                End With

                FP_CALC.INIT_CONTROLS(FP_CALC_CONTROLS)
            End With

            Dim FP_INV_CONTROLS As New Struct_FP_CONTROLS_COLLECTION

            With FP_INV_CONTROLS
                With .GRID
                    .GRID = Inv_GRID
                    .Label = Inv_GRID_Label
                End With

                With FP_INV.SQL_BIND_Params
                    .NameOf_READ = "CL_DETAIL_INV_GRID"
                    .NameOf_SAVE = ""
                    .NameOf_DEL = ""
                    .NameOf_GRID = "CL_DETAIL_INV_GRID"
                End With

                FP_INV.INIT_CONTROLS(FP_INV_CONTROLS)
            End With

            'SPLIT CONTAINER RESIZE
            SplitContainer = New FP_L_SplitContainer_Resize(SplitContainer1, FPp_BTN_View)
        End If
    End Sub

    Private Sub SEL_CL_DETAIL_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim WHERE As String = String.Format("Cust_ID={0}", _Cust_ID)

        With FP_CALC
            If Not .FORM_RECORDS_LOAD(WHERE, , True) Then
                Me.Close()
            End If

            .P_FORM_AllowAdditions = False
            .P_FORM_AllowDeletions = False
            .P_FORM_AllowEdits = False
        End With

        With FP_INV
            If Not .FORM_RECORDS_LOAD(WHERE, , True) Then
                Me.Close()
            End If

            .P_FORM_AllowAdditions = False
            .P_FORM_AllowDeletions = False
            .P_FORM_AllowEdits = False
        End With
    End Sub

#End Region

#Region "FPF EVENTS"

    Private Sub FPf_CONTROLS_INITIALIZED(sender_FPf As FP_Form) Handles FPf.CONTROLS_INITIALIZED
        FPp_BTN_View = FPf.PICTUREBOXES("BTN_View")
    End Sub

#End Region

#Region "FP_INV EVENTS"

    Private Sub FP_INV_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_INV.CONTROLS_INITIALIZED
        FPc_INV_InvoiceNo = FP_INV.CONTROLS("INV_InvoiceNo")

        FPc_INV_InvoiceNo.P_Marker = FP_Control.ENUM_Markertypes.Right_Arrow
    End Sub

#End Region

#Region "FP_CALC EVENTS"

    Private Sub FP_CALC_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_CALC.CONTROLS_INITIALIZED
        FPc_CALC_OrderNum = FP_CALC.CONTROLS("CALC_OrderNum")

        FPc_CALC_OrderNum.P_Marker = FP_Control.ENUM_Markertypes.Right_Arrow
    End Sub

#End Region

#Region "FPC EVENTS"

    Private Sub FPc_CALC_OrderNum_Field_Marker_Click(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Handled As Boolean) Handles FPc_CALC_OrderNum.Field_Marker_Click, FPc_CALC_OrderNum.Field_Doubleclick
        gl_FPApp.RAISEEVENT_Marker_Clicked(sender_FPc, "CL_ORD_SHOW", Nothing, False)
    End Sub

    Private Sub FPc_INV_InvoiceNo_Field_Marker_Click(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Handled As Boolean) Handles FPc_INV_InvoiceNo.Field_Marker_Click, FPc_INV_InvoiceNo.Field_Doubleclick
        gl_FPApp.RAISEEVENT_Marker_Clicked(sender_FPc, "SEL_INV_DIALOG", Nothing, False)
    End Sub

#End Region

End Class