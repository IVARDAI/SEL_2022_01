Public Class FP_ProgressForm
    Public FPApp As FP_App
    Public WithEvents FPf As FP_Form
    Public WithEvents FP As FP

    Public CancelPressed As Boolean = False

    Private WithEvents FPp_Btn_Cancel As FP_PictureBox

    Private MinValue As Double
    Private MaxValue As Double

    Private LastUpdated As DateTime = DateAdd(DateInterval.Second, -1, Now)

    Sub New(ByVal MyFPApp As FP_App, ByVal MySubPrefix As String, ByVal MinVal As Double, ByVal MaxVal As Double)
        InitializeComponent()

        FPApp = MyFPApp
        FPf = New FP_Form("FP_PROGRESSFORM_BASE", FPApp, Me, True)
        FPf.Location_Save_On_Close = False

        FP = New FP(FPf, "FP_PROGRESSFORM", MySubPrefix, True)

        MinValue = MinVal
        MaxValue = MaxVal
    End Sub

    Private Sub FP_ProgressForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        FPf.INIT_CONTROLS(Nothing)
        FPf.PICTUREBOXES("Animation").P_Locked = True
        FP.INIT_CONTROLS(Nothing)
        PBar.Minimum = MinValue
        PBar.Maximum = MaxValue
        Refresh()
    End Sub

    Private Sub FP_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP.CONTROLS_INITIALIZED
        FPp_Btn_Cancel = FPf.PICTUREBOXES("Btn_Cancel")
    End Sub

    Private Sub FPp_Btn_Cancel_CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FPp_Btn_Cancel.CLICK
        CancelPressed = True
        CURSOR_SHOW_WAIT()
        Close()
    End Sub

    Public Sub SET_VALUES(ByVal NewValue As Integer, ByVal NewText As String)
        NewValue = 1.02 * NewValue 'A progressbar mindig "lemarad", vagyis kesessel rajzolja az allapotot, igy 100%-nal egy folyamat vegen kb 95%-on all, ha 100 lepesben megyunk 0-tol 100-ig.
        If PBar.Minimum > NewValue Then
            NewValue = PBar.Minimum
        ElseIf PBar.Maximum < NewValue Then
            NewValue = PBar.Maximum
        End If

        InfoText.Text = NewText

        If 0.0 + (Math.Abs(PBar.Value - NewValue) / (PBar.Maximum - PBar.Minimum)) > 0.02 Then
            PBar.Value = NewValue
            Application.DoEvents()
            PBar.Refresh()
            System.Threading.Thread.Sleep(100)
            Application.DoEvents()
            PBar.Refresh()
        End If

        Application.DoEvents()

        Me.Refresh()
    End Sub

    Private Sub FPf_FORM_CLOSED(sender As Object) Handles FPf.FORM_CLOSED
        'Erre azert van szukseg, mert ez a form DoEvent-el mukodik, aminek hatasara egyeb szalak is futhatnak.
        'A felhasznalo akar be is zarhatja a SELEXPED-et a menu-n keresztul. Ekkor az esetlegesen futo process - ha mashonnan nem is - legalabb innen vegye eszre,
        'hogy idoszeru lenne leallnia.
        CancelPressed = True
    End Sub
End Class
