<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_SELECT_Language
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
        Me.ButtonOK = New System.Windows.Forms.PictureBox
        Me.Languages = New System.Windows.Forms.ListBox
        Me.ButtonCancel = New System.Windows.Forms.PictureBox
        Me.Title_Label = New System.Windows.Forms.Label
        CType(Me.ButtonOK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ButtonCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonOK
        '
        Me.ButtonOK.Location = New System.Drawing.Point(264, 165)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(44, 44)
        Me.ButtonOK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ButtonOK.TabIndex = 100079
        Me.ButtonOK.TabStop = False
        '
        'Languages
        '
        Me.Languages.BackColor = System.Drawing.Color.DarkGray
        Me.Languages.Font = New System.Drawing.Font("Perpetua", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Languages.ForeColor = System.Drawing.Color.Black
        Me.Languages.FormattingEnabled = True
        Me.Languages.ItemHeight = 24
        Me.Languages.Items.AddRange(New Object() {"Magyar", "English", "Deutsch"})
        Me.Languages.Location = New System.Drawing.Point(83, 46)
        Me.Languages.Name = "Languages"
        Me.Languages.Size = New System.Drawing.Size(144, 100)
        Me.Languages.TabIndex = 1
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Location = New System.Drawing.Point(214, 165)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(44, 44)
        Me.ButtonCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ButtonCancel.TabIndex = 100080
        Me.ButtonCancel.TabStop = False
        '
        'Title_Label
        '
        Me.Title_Label.BackColor = System.Drawing.Color.Maroon
        Me.Title_Label.Dock = System.Windows.Forms.DockStyle.Top
        Me.Title_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Title_Label.ForeColor = System.Drawing.Color.Gold
        Me.Title_Label.Location = New System.Drawing.Point(0, 0)
        Me.Title_Label.Name = "Title_Label"
        Me.Title_Label.Size = New System.Drawing.Size(311, 22)
        Me.Title_Label.TabIndex = 100081
        Me.Title_Label.Text = "Label1"
        Me.Title_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FP_SELECT_Language
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(311, 213)
        Me.Controls.Add(Me.Title_Label)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.Languages)
        Me.Controls.Add(Me.ButtonOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FP_SELECT_Language"
        Me.Text = "FP_SELECT_Language"
        CType(Me.ButtonOK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ButtonCancel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonOK As System.Windows.Forms.PictureBox
    Friend WithEvents Languages As System.Windows.Forms.ListBox
    Friend WithEvents ButtonCancel As System.Windows.Forms.PictureBox
    Friend WithEvents Title_Label As System.Windows.Forms.Label
End Class
