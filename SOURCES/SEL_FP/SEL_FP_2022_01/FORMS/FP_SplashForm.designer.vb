<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_SplashForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FP_SplashForm))
        Me.MessageText_Label = New System.Windows.Forms.Label()
        Me.Animation = New System.Windows.Forms.PictureBox()
        CType(Me.Animation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MessageText_Label
        '
        Me.MessageText_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.MessageText_Label.ForeColor = System.Drawing.Color.White
        Me.MessageText_Label.Location = New System.Drawing.Point(52, 9)
        Me.MessageText_Label.Name = "MessageText_Label"
        Me.MessageText_Label.Size = New System.Drawing.Size(447, 40)
        Me.MessageText_Label.TabIndex = 3
        Me.MessageText_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Animation
        '
        Me.Animation.Image = CType(resources.GetObject("Animation.Image"), System.Drawing.Image)
        Me.Animation.Location = New System.Drawing.Point(7, 9)
        Me.Animation.Name = "Animation"
        Me.Animation.Size = New System.Drawing.Size(40, 40)
        Me.Animation.TabIndex = 2
        Me.Animation.TabStop = False
        '
        'Form_SplashForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(503, 55)
        Me.ControlBox = False
        Me.Controls.Add(Me.MessageText_Label)
        Me.Controls.Add(Me.Animation)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "Form_SplashForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.Animation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MessageText_Label As System.Windows.Forms.Label
    Friend WithEvents Animation As System.Windows.Forms.PictureBox
End Class
