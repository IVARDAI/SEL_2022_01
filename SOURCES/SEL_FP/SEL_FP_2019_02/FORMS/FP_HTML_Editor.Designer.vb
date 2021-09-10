<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_HTML_Editor
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
        Me.HtmlEdit = New MSDN.Html.Editor.HtmlEditorControl()
        Me.SuspendLayout()
        '
        'HtmlEdit
        '
        Me.HtmlEdit.InnerText = Nothing
        Me.HtmlEdit.Location = New System.Drawing.Point(93, 60)
        Me.HtmlEdit.Name = "HtmlEdit"
        Me.HtmlEdit.Size = New System.Drawing.Size(675, 562)
        Me.HtmlEdit.TabIndex = 0
        Me.HtmlEdit.ToolbarDock = System.Windows.Forms.DockStyle.Right
        '
        'FP_HTML_Editor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(868, 671)
        Me.Controls.Add(Me.HtmlEdit)
        Me.Name = "FP_HTML_Editor"
        Me.Text = "FP_HTML_Editor"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents HtmlEdit As MSDN.Html.Editor.HtmlEditorControl
End Class
