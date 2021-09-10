<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_ProgressForm
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
        Me.PBar = New System.Windows.Forms.ProgressBar
        Me.InfoText = New System.Windows.Forms.Label
        Me.Btn_Cancel = New System.Windows.Forms.PictureBox
        Me.Animation = New System.Windows.Forms.PictureBox
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Animation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PBar
        '
        Me.PBar.Location = New System.Drawing.Point(0, 116)
        Me.PBar.Name = "PBar"
        Me.PBar.Size = New System.Drawing.Size(706, 28)
        Me.PBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.PBar.TabIndex = 1
        '
        'InfoText
        '
        Me.InfoText.BackColor = System.Drawing.Color.DarkSlateGray
        Me.InfoText.ForeColor = System.Drawing.Color.Silver
        Me.InfoText.Location = New System.Drawing.Point(0, 144)
        Me.InfoText.Name = "InfoText"
        Me.InfoText.Size = New System.Drawing.Size(706, 16)
        Me.InfoText.TabIndex = 4
        Me.InfoText.Text = "InfoText"
        Me.InfoText.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Btn_Cancel
        '
        Me.Btn_Cancel.Location = New System.Drawing.Point(706, 116)
        Me.Btn_Cancel.Name = "Btn_Cancel"
        Me.Btn_Cancel.Size = New System.Drawing.Size(44, 44)
        Me.Btn_Cancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Btn_Cancel.TabIndex = 5
        Me.Btn_Cancel.TabStop = False
        '
        'Animation
        '
        Me.Animation.Location = New System.Drawing.Point(0, 0)
        Me.Animation.Name = "Animation"
        Me.Animation.Size = New System.Drawing.Size(750, 115)
        Me.Animation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.Animation.TabIndex = 6
        Me.Animation.TabStop = False
        '
        'FP_ProgressForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(750, 161)
        Me.ControlBox = False
        Me.Controls.Add(Me.Animation)
        Me.Controls.Add(Me.Btn_Cancel)
        Me.Controls.Add(Me.InfoText)
        Me.Controls.Add(Me.PBar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "FP_ProgressForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " "
        Me.TopMost = True
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Animation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PBar As System.Windows.Forms.ProgressBar
    Friend WithEvents InfoText As System.Windows.Forms.Label
    Friend WithEvents Btn_Cancel As System.Windows.Forms.PictureBox
    Friend WithEvents Animation As System.Windows.Forms.PictureBox

End Class
