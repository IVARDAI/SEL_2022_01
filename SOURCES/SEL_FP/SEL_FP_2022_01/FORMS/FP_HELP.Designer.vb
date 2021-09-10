<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_HELP
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
        Me.HELP_Short = New System.Windows.Forms.Label
        Me.HELP_Link = New System.Windows.Forms.Label
        Me.HELP_NoShow = New System.Windows.Forms.Label
        Me.HELP_CheckBox = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'HELP_Short
        '
        Me.HELP_Short.AutoSize = True
        Me.HELP_Short.Location = New System.Drawing.Point(5, 5)
        Me.HELP_Short.Name = "HELP_Short"
        Me.HELP_Short.Size = New System.Drawing.Size(0, 13)
        Me.HELP_Short.TabIndex = 9999
        '
        'HELP_Link
        '
        Me.HELP_Link.Cursor = System.Windows.Forms.Cursors.Hand
        Me.HELP_Link.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.HELP_Link.ForeColor = System.Drawing.Color.Blue
        Me.HELP_Link.Location = New System.Drawing.Point(5, 58)
        Me.HELP_Link.Name = "HELP_Link"
        Me.HELP_Link.Size = New System.Drawing.Size(120, 13)
        Me.HELP_Link.TabIndex = 9999
        Me.HELP_Link.Text = "More Help..."
        Me.HELP_Link.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'HELP_NoShow
        '
        Me.HELP_NoShow.Cursor = System.Windows.Forms.Cursors.Hand
        Me.HELP_NoShow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.HELP_NoShow.ForeColor = System.Drawing.Color.Black
        Me.HELP_NoShow.Location = New System.Drawing.Point(140, 58)
        Me.HELP_NoShow.Name = "HELP_NoShow"
        Me.HELP_NoShow.Size = New System.Drawing.Size(160, 18)
        Me.HELP_NoShow.TabIndex = 9999
        Me.HELP_NoShow.Text = "Do not show"
        Me.HELP_NoShow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'HELP_CheckBox
        '
        Me.HELP_CheckBox.CheckAlign = System.Drawing.ContentAlignment.TopRight
        Me.HELP_CheckBox.Cursor = System.Windows.Forms.Cursors.Hand
        Me.HELP_CheckBox.Location = New System.Drawing.Point(300, 58)
        Me.HELP_CheckBox.Name = "HELP_CheckBox"
        Me.HELP_CheckBox.Size = New System.Drawing.Size(18, 18)
        Me.HELP_CheckBox.TabIndex = 9999
        Me.HELP_CheckBox.TabStop = False
        Me.HELP_CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.HELP_CheckBox.UseVisualStyleBackColor = False
        '
        'FP_HELP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(330, 75)
        Me.Controls.Add(Me.HELP_CheckBox)
        Me.Controls.Add(Me.HELP_NoShow)
        Me.Controls.Add(Me.HELP_Link)
        Me.Controls.Add(Me.HELP_Short)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FP_HELP"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents HELP_Short As System.Windows.Forms.Label
    Friend WithEvents HELP_Link As System.Windows.Forms.Label
    Friend WithEvents HELP_NoShow As System.Windows.Forms.Label
    Friend WithEvents HELP_CheckBox As System.Windows.Forms.CheckBox
End Class
