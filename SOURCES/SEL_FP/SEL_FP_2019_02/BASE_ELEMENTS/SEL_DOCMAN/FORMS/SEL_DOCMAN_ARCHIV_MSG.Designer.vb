<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SEL_DOCMAN_ARCHIV_MSG
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
        Me.Btn_SAVE = New System.Windows.Forms.PictureBox()
        CType(Me.Btn_SAVE, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Btn_SAVE
        '
        Me.Btn_SAVE.Location = New System.Drawing.Point(12, 12)
        Me.Btn_SAVE.Name = "Btn_SAVE"
        Me.Btn_SAVE.Size = New System.Drawing.Size(44, 44)
        Me.Btn_SAVE.TabIndex = 0
        Me.Btn_SAVE.TabStop = False
        '
        'SEL_DOCMAN_ARCHIV_MSG
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(71, 73)
        Me.Controls.Add(Me.Btn_SAVE)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MinimizeBox = False
        Me.Name = "SEL_DOCMAN_ARCHIV_MSG"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.TopMost = True
        CType(Me.Btn_SAVE, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Btn_SAVE As System.Windows.Forms.PictureBox
End Class
