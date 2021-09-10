Public Class FP_SELECT_Language
    Private FPApp As FP_App

    Sub New(ByVal MyFPApp As FP_App)
        InitializeComponent()

        FPApp = MyFPApp
    End Sub

    Sub NAVIGATION_FORWARD()
        Dim OUT As Boolean = False

        If Languages.SelectedIndex >= 0 Then
            Select Case Languages.SelectedIndex
                Case 0
                    FPApp.setLanddialog("H")
                    OUT = True

                Case 1
                    FPApp.setLanddialog("GB")
                    OUT = True

                Case 2
                    FPApp.setLanddialog("D")
                    OUT = True

                Case Else
                    MsgBox("FP_SELECT_Language.NAVIGATION_FORWARD: Unknown language: " + Languages.SelectedItem)
            End Select

            If OUT = True Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Private Sub ButtonOK_Click() Handles ButtonOK.Click
        NAVIGATION_FORWARD()
    End Sub

    Private Sub ListSprache_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Languages.DoubleClick
        NAVIGATION_FORWARD()
    End Sub

    Private Sub FP_SELECT_Language_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        FPApp.SKIN_DRAW_BACKGROUND(e.Graphics, Me.ClientRectangle)
    End Sub

    Private Sub FP_SELECT_Language_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Languages.SelectedIndex = 0
        Title_Label.Text = "SELEXPED " + VERS + " SRV" + SRV
    End Sub

    Private Sub ButtonOK_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles ButtonOK.Paint
        FPApp.SKIN_DRAW_BACKGROUND(e.Graphics, ButtonOK.ClientRectangle, "but_ok_44x44.png")
    End Sub

    Private Sub ButtonCancel_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles ButtonCancel.Paint
        FPApp.SKIN_DRAW_BACKGROUND(e.Graphics, ButtonOK.ClientRectangle, "but_cancel_44x44.png")
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Me.Close()
    End Sub
End Class
