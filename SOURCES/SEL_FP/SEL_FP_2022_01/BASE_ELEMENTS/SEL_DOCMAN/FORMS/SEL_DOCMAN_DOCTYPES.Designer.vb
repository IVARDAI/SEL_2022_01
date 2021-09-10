<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SEL_DOCMAN_DOCTYPES
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SEL_DOCMAN_DOCTYPES))
        Me.DG = New System.Windows.Forms.DataGridView()
        Me.DG_Label = New System.Windows.Forms.Label()
        Me.Btn_OK = New System.Windows.Forms.PictureBox()
        Me.btn_CANCEL = New System.Windows.Forms.PictureBox()
        Me.btn_HLP = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btn_ExcelExport = New System.Windows.Forms.PictureBox()
        Me.DG_SavePoint = New System.Windows.Forms.TextBox()
        CType(Me.DG, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btn_CANCEL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btn_HLP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btn_ExcelExport, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DG
        '
        Me.DG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DG.Location = New System.Drawing.Point(14, 42)
        Me.DG.Name = "DG"
        Me.DG.Size = New System.Drawing.Size(489, 221)
        Me.DG.TabIndex = 100
        '
        'DG_Label
        '
        Me.DG_Label.BackColor = System.Drawing.Color.DimGray
        Me.DG_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.DG_Label.ForeColor = System.Drawing.Color.White
        Me.DG_Label.Location = New System.Drawing.Point(14, 10)
        Me.DG_Label.Name = "DG_Label"
        Me.DG_Label.Size = New System.Drawing.Size(489, 22)
        Me.DG_Label.TabIndex = 9999
        Me.DG_Label.Text = "DG_Label"
        Me.DG_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Btn_OK
        '
        Me.Btn_OK.Location = New System.Drawing.Point(723, 274)
        Me.Btn_OK.Name = "Btn_OK"
        Me.Btn_OK.Size = New System.Drawing.Size(44, 44)
        Me.Btn_OK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Btn_OK.TabIndex = 100036
        Me.Btn_OK.TabStop = False
        '
        'btn_CANCEL
        '
        Me.btn_CANCEL.Location = New System.Drawing.Point(668, 274)
        Me.btn_CANCEL.Name = "btn_CANCEL"
        Me.btn_CANCEL.Size = New System.Drawing.Size(44, 44)
        Me.btn_CANCEL.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.btn_CANCEL.TabIndex = 100037
        Me.btn_CANCEL.TabStop = False
        '
        'btn_HLP
        '
        Me.btn_HLP.Location = New System.Drawing.Point(516, 274)
        Me.btn_HLP.Name = "btn_HLP"
        Me.btn_HLP.Size = New System.Drawing.Size(44, 44)
        Me.btn_HLP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.btn_HLP.TabIndex = 100038
        Me.btn_HLP.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(516, 13)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(247, 250)
        Me.PictureBox1.TabIndex = 100039
        Me.PictureBox1.TabStop = False
        '
        'btn_ExcelExport
        '
        Me.btn_ExcelExport.Location = New System.Drawing.Point(574, 274)
        Me.btn_ExcelExport.Name = "btn_ExcelExport"
        Me.btn_ExcelExport.Size = New System.Drawing.Size(44, 44)
        Me.btn_ExcelExport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.btn_ExcelExport.TabIndex = 100040
        Me.btn_ExcelExport.TabStop = False
        '
        'DG_SavePoint
        '
        Me.DG_SavePoint.Location = New System.Drawing.Point(454, 204)
        Me.DG_SavePoint.Name = "DG_SavePoint"
        Me.DG_SavePoint.Size = New System.Drawing.Size(14, 22)
        Me.DG_SavePoint.TabIndex = 199
        '
        'SEL_DOCMAN_DOCTYPES
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(777, 337)
        Me.ControlBox = False
        Me.Controls.Add(Me.btn_ExcelExport)
        Me.Controls.Add(Me.btn_HLP)
        Me.Controls.Add(Me.btn_CANCEL)
        Me.Controls.Add(Me.Btn_OK)
        Me.Controls.Add(Me.DG_Label)
        Me.Controls.Add(Me.DG)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.DG_SavePoint)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "SEL_DOCMAN_DOCTYPES"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " "
        CType(Me.DG, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btn_CANCEL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btn_HLP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btn_ExcelExport, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DG As System.Windows.Forms.DataGridView
    Friend WithEvents DG_Label As System.Windows.Forms.Label
    Friend WithEvents Btn_OK As System.Windows.Forms.PictureBox
    Friend WithEvents btn_CANCEL As System.Windows.Forms.PictureBox
    Friend WithEvents btn_HLP As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btn_ExcelExport As System.Windows.Forms.PictureBox
    Friend WithEvents DG_SavePoint As System.Windows.Forms.TextBox
End Class
