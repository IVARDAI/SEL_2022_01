Public Class FP_VerticalButton
    Inherits Button
    Public Property VerticalText As String
    Private Fmt As New StringFormat
    Public Sub New()
        Fmt.Alignment = StringAlignment.Center
        Fmt.LineAlignment = StringAlignment.Center
    End Sub
    Protected Overrides Sub OnPaint(ByVal pevent As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(pevent)
        pevent.Graphics.TranslateTransform(Width, 0)
        pevent.Graphics.RotateTransform(90)
        pevent.Graphics.DrawString(_VerticalText, Font, Brushes.Black, New Rectangle(0, 0, Height, Width), Fmt)
    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        Me.ResumeLayout(False)

    End Sub
End Class
