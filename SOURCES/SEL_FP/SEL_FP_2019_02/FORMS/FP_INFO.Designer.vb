<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_Info
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
        Me.Logo_Background_Panel = New System.Windows.Forms.Panel()
        Me.Btn_Logo = New System.Windows.Forms.PictureBox()
        Me.MessageText = New System.Windows.Forms.TextBox()
        Me.More_Info_Label = New System.Windows.Forms.Label()
        Me.Logo_Background_Panel.SuspendLayout()
        CType(Me.Btn_Logo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Logo_Background_Panel
        '
        Me.Logo_Background_Panel.BackColor = System.Drawing.Color.FromArgb(CType(CType(3, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Logo_Background_Panel.Controls.Add(Me.Btn_Logo)
        Me.Logo_Background_Panel.Location = New System.Drawing.Point(13, 6)
        Me.Logo_Background_Panel.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Logo_Background_Panel.Name = "Logo_Background_Panel"
        Me.Logo_Background_Panel.Size = New System.Drawing.Size(207, 386)
        Me.Logo_Background_Panel.TabIndex = 0
        '
        'Btn_Logo
        '
        Me.Btn_Logo.Location = New System.Drawing.Point(28, 17)
        Me.Btn_Logo.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Btn_Logo.Name = "Btn_Logo"
        Me.Btn_Logo.Size = New System.Drawing.Size(154, 98)
        Me.Btn_Logo.TabIndex = 1
        Me.Btn_Logo.TabStop = False
        '
        'MessageText
        '
        Me.MessageText.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.MessageText.Location = New System.Drawing.Point(242, 17)
        Me.MessageText.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MessageText.Multiline = True
        Me.MessageText.Name = "MessageText"
        Me.MessageText.Size = New System.Drawing.Size(523, 323)
        Me.MessageText.TabIndex = 1
        '
        'More_Info_Label
        '
        Me.More_Info_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.More_Info_Label.ForeColor = System.Drawing.Color.Blue
        Me.More_Info_Label.Location = New System.Drawing.Point(270, 360)
        Me.More_Info_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.More_Info_Label.Name = "More_Info_Label"
        Me.More_Info_Label.Size = New System.Drawing.Size(257, 30)
        Me.More_Info_Label.TabIndex = 2
        Me.More_Info_Label.Text = "More_Info_Label"
        '
        'FP_Info
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(787, 397)
        Me.Controls.Add(Me.More_Info_Label)
        Me.Controls.Add(Me.MessageText)
        Me.Controls.Add(Me.Logo_Background_Panel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Location = New System.Drawing.Point(-1000, -1000)
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Name = "FP_Info"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "SELEXPED"
        Me.Logo_Background_Panel.ResumeLayout(False)
        CType(Me.Btn_Logo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Logo_Background_Panel As System.Windows.Forms.Panel
    Friend WithEvents Btn_Logo As System.Windows.Forms.PictureBox
    Friend WithEvents MessageText As System.Windows.Forms.TextBox
    Friend WithEvents More_Info_Label As System.Windows.Forms.Label
End Class
