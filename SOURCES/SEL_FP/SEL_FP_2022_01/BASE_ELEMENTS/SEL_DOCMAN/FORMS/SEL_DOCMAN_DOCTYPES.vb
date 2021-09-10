Public Class SEL_DOCMAN_DOCTYPES
    Public FPf As FP_Form = Nothing
    Public WithEvents FP_DocTypes As FP = Nothing
    Private WithEvents FPc_Descr As FP_Control = Nothing

    Public Sub New()
        InitializeComponent()

        FPf = New FP_Form("FP_DOCMAN_DocTypes_BASE", gl_FPApp, Me, True)
        FP_DocTypes = New FP(FPf, "FP_DOCMAN_DocTypes")
    End Sub

    Private Sub SEL_DOCMAN_DOCTYPES_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim FPF_CONTROLS As New Struct_FP_FORM_CONTROLS_COLLECTION

        With FPF_CONTROLS
            .Btn_HELP = btn_HLP
            .Dlg_Btn_CANCEL = btn_CANCEL
            .Dlg_Btn_OK = Btn_OK
        End With

        FPf.INIT_CONTROLS(FPF_CONTROLS)

        Dim FP_DocTypes_CONTROLS As New Struct_FP_CONTROLS_COLLECTION

        With FP_DocTypes_CONTROLS
            With .GRID
                .GRID = DG
                .Label = DG_Label
            End With
            .Btn_ExportToExcel = btn_ExcelExport
        End With

        FP_DocTypes.INIT_CONTROLS(FP_DocTypes_CONTROLS)
    End Sub

    Private Sub SEL_DOCMAN_DocTypes_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If Not FP_DocTypes.FORM_RECORDS_LOAD Then
            If Not FP_DocTypes.FORM_RECORDS_LOAD(, True) Then
                Close()
            End If
        End If
    End Sub

    Private Sub FP_DocTypes_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_DocTypes.CONTROLS_INITIALIZED
        FPc_Descr = FP_DocTypes.CONTROLS("Descr")
    End Sub

    Private Sub FPc_Descr_Field_BeforeUpdate(sender_FPc As FP_Control, ByRef Cancel As Integer) Handles FPc_Descr.Field_BeforeUpdate
        Dim Current_Descr As String = FPc_Descr.P_VALUE

        If Current_Descr > "" Then
            Dim Criteria As String = String.Format("RecordID <> {0} And Descr = '{1}'", FP_DocTypes.P_DATA_Current_ID, FPc_Descr.P_VALUE)

            If FP_DocTypes.GRID.DT.Select(Criteria).Count > 0 Then
                Cancel = True
                FPf.DoMyMsgBox_AT_THE_END(52003) 'Ez a tipus mar letezik
            End If
        End If
    End Sub

    Private Sub FP_DocTypes_Form_AfterDelete(sender_FP As FP) Handles FP_DocTypes.Form_AfterDelete
        COMBOBOX_REFRESH_ALL()
    End Sub

    Private Sub FP_DocTypes_Form_AfterUpdate(sender_FP As FP) Handles FP_DocTypes.Form_AfterUpdate
        COMBOBOX_REFRESH_ALL()
    End Sub

    Private Sub COMBOBOX_REFRESH_ALL()
        gl_FPApp.COMBOBOX_REFRESH_DT_FixText_Key("@@VB_COMBO_DOCTYPES")
    End Sub
End Class