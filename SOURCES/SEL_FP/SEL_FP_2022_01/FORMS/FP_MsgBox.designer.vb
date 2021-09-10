<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_MsgBox
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Message_Text = New System.Windows.Forms.Label()
        Me.Taste1 = New System.Windows.Forms.Button()
        Me.Taste2 = New System.Windows.Forms.Button()
        Me.Taste3 = New System.Windows.Forms.Button()
        Me.Titel = New System.Windows.Forms.Label()
        Me.LinkLabel = New System.Windows.Forms.LinkLabel()
        Me.SuspendLayout()
        '
        'Message_Text
        '
        Me.Message_Text.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Message_Text.Location = New System.Drawing.Point(15, 76)
        Me.Message_Text.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Message_Text.Name = "Message_Text"
        Me.Message_Text.Size = New System.Drawing.Size(1062, 264)
        Me.Message_Text.TabIndex = 3
        Me.Message_Text.Text = "Message text"
        Me.Message_Text.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Taste1
        '
        Me.Taste1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Taste1.ForeColor = System.Drawing.Color.Navy
        Me.Taste1.Location = New System.Drawing.Point(15, 354)
        Me.Taste1.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Taste1.Name = "Taste1"
        Me.Taste1.Size = New System.Drawing.Size(345, 68)
        Me.Taste1.TabIndex = 0
        Me.Taste1.Text = "Taste1"
        Me.Taste1.UseVisualStyleBackColor = True
        '
        'Taste2
        '
        Me.Taste2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Taste2.ForeColor = System.Drawing.Color.Navy
        Me.Taste2.Location = New System.Drawing.Point(374, 354)
        Me.Taste2.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Taste2.Name = "Taste2"
        Me.Taste2.Size = New System.Drawing.Size(345, 68)
        Me.Taste2.TabIndex = 1
        Me.Taste2.Text = "Taste2"
        Me.Taste2.UseVisualStyleBackColor = True
        '
        'Taste3
        '
        Me.Taste3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Taste3.ForeColor = System.Drawing.Color.Navy
        Me.Taste3.Location = New System.Drawing.Point(732, 354)
        Me.Taste3.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Taste3.Name = "Taste3"
        Me.Taste3.Size = New System.Drawing.Size(345, 68)
        Me.Taste3.TabIndex = 2
        Me.Taste3.Text = "Taste3"
        Me.Taste3.UseVisualStyleBackColor = True
        '
        'Titel
        '
        Me.Titel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Titel.Location = New System.Drawing.Point(15, 15)
        Me.Titel.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Titel.Name = "Titel"
        Me.Titel.Size = New System.Drawing.Size(1063, 85)
        Me.Titel.TabIndex = 4
        Me.Titel.Text = "Titel"
        Me.Titel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'LinkLabel
        '
        Me.LinkLabel.AutoSize = True
        Me.LinkLabel.Location = New System.Drawing.Point(15, 437)
        Me.LinkLabel.Name = "LinkLabel"
        Me.LinkLabel.Size = New System.Drawing.Size(96, 25)
        Me.LinkLabel.TabIndex = 5
        Me.LinkLabel.TabStop = True
        Me.LinkLabel.Text = "LinkLabel"
        '
        'FP_MsgBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1093, 482)
        Me.ControlBox = False
        Me.Controls.Add(Me.LinkLabel)
        Me.Controls.Add(Me.Message_Text)
        Me.Controls.Add(Me.Taste1)
        Me.Controls.Add(Me.Taste2)
        Me.Controls.Add(Me.Taste3)
        Me.Controls.Add(Me.Titel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FP_MsgBox"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = ""
        Me.Text = "SEQ,MESSAGETITEL"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Message_Text As System.Windows.Forms.Label
    Friend WithEvents Taste1 As System.Windows.Forms.Button
    Friend WithEvents Taste2 As System.Windows.Forms.Button
    Friend WithEvents Taste3 As System.Windows.Forms.Button
    Friend WithEvents Titel As System.Windows.Forms.Label
    Friend WithEvents LinkLabel As System.Windows.Forms.LinkLabel
End Class
