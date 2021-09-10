<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SEL_ISO_DIALOG
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
        Me.Panel_ISO = New System.Windows.Forms.Panel()
        Me.BTN_HLP = New System.Windows.Forms.PictureBox()
        Me.BTN_OK = New System.Windows.Forms.PictureBox()
        CType(Me.BTN_HLP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_OK, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel_ISO
        '
        Me.Panel_ISO.Location = New System.Drawing.Point(53, 35)
        Me.Panel_ISO.Name = "Panel_ISO"
        Me.Panel_ISO.Size = New System.Drawing.Size(798, 424)
        Me.Panel_ISO.TabIndex = 0
        '
        'BTN_HLP
        '
        Me.BTN_HLP.Location = New System.Drawing.Point(53, 490)
        Me.BTN_HLP.Name = "BTN_HLP"
        Me.BTN_HLP.Size = New System.Drawing.Size(44, 44)
        Me.BTN_HLP.TabIndex = 1
        Me.BTN_HLP.TabStop = False
        '
        'BTN_OK
        '
        Me.BTN_OK.Location = New System.Drawing.Point(807, 500)
        Me.BTN_OK.Name = "BTN_OK"
        Me.BTN_OK.Size = New System.Drawing.Size(44, 44)
        Me.BTN_OK.TabIndex = 2
        Me.BTN_OK.TabStop = False
        '
        'SEL_ISO_DIALOG
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(915, 606)
        Me.Controls.Add(Me.BTN_OK)
        Me.Controls.Add(Me.BTN_HLP)
        Me.Controls.Add(Me.Panel_ISO)
        Me.Name = "SEL_ISO_DIALOG"
        Me.Text = "SEL_ISO_DIALOG"
        CType(Me.BTN_HLP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_OK, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel_ISO As System.Windows.Forms.Panel
    Friend WithEvents BTN_HLP As System.Windows.Forms.PictureBox
    Friend WithEvents BTN_OK As System.Windows.Forms.PictureBox
End Class
