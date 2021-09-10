<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_Simple_Edit
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FP_Simple_Edit))
        Me.Btn_Cancel = New System.Windows.Forms.PictureBox()
        Me.Btn_OK = New System.Windows.Forms.PictureBox()
        Me.Btn_Help = New System.Windows.Forms.PictureBox()
        Me.SimpleEdit_Panel = New System.Windows.Forms.Panel()
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_Help, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Btn_Cancel
        '
        Me.Btn_Cancel.Location = New System.Drawing.Point(597, 406)
        Me.Btn_Cancel.Name = "Btn_Cancel"
        Me.Btn_Cancel.Size = New System.Drawing.Size(44, 44)
        Me.Btn_Cancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Btn_Cancel.TabIndex = 10005
        Me.Btn_Cancel.TabStop = False
        '
        'Btn_OK
        '
        Me.Btn_OK.Location = New System.Drawing.Point(656, 406)
        Me.Btn_OK.Name = "Btn_OK"
        Me.Btn_OK.Size = New System.Drawing.Size(44, 44)
        Me.Btn_OK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Btn_OK.TabIndex = 10004
        Me.Btn_OK.TabStop = False
        '
        'Btn_Help
        '
        Me.Btn_Help.Location = New System.Drawing.Point(318, 406)
        Me.Btn_Help.Name = "Btn_Help"
        Me.Btn_Help.Size = New System.Drawing.Size(44, 44)
        Me.Btn_Help.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Btn_Help.TabIndex = 10008
        Me.Btn_Help.TabStop = False
        '
        'SimpleEdit_Panel
        '
        Me.SimpleEdit_Panel.BackColor = System.Drawing.Color.DimGray
        Me.SimpleEdit_Panel.Location = New System.Drawing.Point(3, 3)
        Me.SimpleEdit_Panel.Name = "SimpleEdit_Panel"
        Me.SimpleEdit_Panel.Size = New System.Drawing.Size(703, 396)
        Me.SimpleEdit_Panel.TabIndex = 1000
        '
        'FP_Simple_Edit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(709, 454)
        Me.ControlBox = False
        Me.Controls.Add(Me.SimpleEdit_Panel)
        Me.Controls.Add(Me.Btn_Help)
        Me.Controls.Add(Me.Btn_Cancel)
        Me.Controls.Add(Me.Btn_OK)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "FP_Simple_Edit"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " "
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_Help, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Btn_Cancel As System.Windows.Forms.PictureBox
    Friend WithEvents Btn_OK As System.Windows.Forms.PictureBox
    Friend WithEvents Btn_Help As System.Windows.Forms.PictureBox
    Friend WithEvents SimpleEdit_Panel As System.Windows.Forms.Panel
End Class
