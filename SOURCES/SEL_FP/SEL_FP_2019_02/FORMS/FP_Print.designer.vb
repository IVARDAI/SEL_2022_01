<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_Print
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ReportViewerControl = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.SuspendLayout()
        '
        'ReportViewerControl
        '
        Me.ReportViewerControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ReportViewerControl.Location = New System.Drawing.Point(0, 0)
        Me.ReportViewerControl.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.ReportViewerControl.Name = "ReportViewerControl"
        Me.ReportViewerControl.Size = New System.Drawing.Size(1909, 993)
        Me.ReportViewerControl.TabIndex = 0
        Me.ReportViewerControl.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth
        '
        'FP_Print
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1909, 993)
        Me.Controls.Add(Me.ReportViewerControl)
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Name = "FP_Print"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form_Print"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ReportViewerControl As Microsoft.Reporting.WinForms.ReportViewer
End Class
