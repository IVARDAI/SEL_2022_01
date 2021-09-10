<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_SELECT_PICTURE
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FP_SELECT_PICTURE))
        Me.Picture_Panel = New System.Windows.Forms.Panel()
        Me.Title_Label = New System.Windows.Forms.Label()
        Me.Btn_OK = New System.Windows.Forms.PictureBox()
        Me.Descr_Label = New System.Windows.Forms.Label()
        Me.Category = New System.Windows.Forms.ComboBox()
        Me.Category_Label = New System.Windows.Forms.Label()
        Me.Btn_Cancel = New System.Windows.Forms.PictureBox()
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Picture_Panel
        '
        Me.Picture_Panel.Location = New System.Drawing.Point(0, 75)
        Me.Picture_Panel.Name = "Picture_Panel"
        Me.Picture_Panel.Size = New System.Drawing.Size(680, 100)
        Me.Picture_Panel.TabIndex = 0
        '
        'Title_Label
        '
        Me.Title_Label.BackColor = System.Drawing.Color.DimGray
        Me.Title_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Title_Label.ForeColor = System.Drawing.Color.White
        Me.Title_Label.Location = New System.Drawing.Point(0, -1)
        Me.Title_Label.Name = "Title_Label"
        Me.Title_Label.Size = New System.Drawing.Size(680, 22)
        Me.Title_Label.TabIndex = 9999
        Me.Title_Label.Text = "Title_Label"
        Me.Title_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Btn_OK
        '
        Me.Btn_OK.Location = New System.Drawing.Point(636, 181)
        Me.Btn_OK.Name = "Btn_OK"
        Me.Btn_OK.Size = New System.Drawing.Size(44, 44)
        Me.Btn_OK.TabIndex = 10000
        Me.Btn_OK.TabStop = False
        '
        'Descr_Label
        '
        Me.Descr_Label.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Descr_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Descr_Label.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Descr_Label.Location = New System.Drawing.Point(0, 21)
        Me.Descr_Label.Name = "Descr_Label"
        Me.Descr_Label.Size = New System.Drawing.Size(680, 22)
        Me.Descr_Label.TabIndex = 9999
        Me.Descr_Label.Text = "Select an icon from the list. This icon will apear on your function button in the" & _
    " menü"
        Me.Descr_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Category
        '
        Me.Category.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Category.FormattingEnabled = True
        Me.Category.Location = New System.Drawing.Point(132, 46)
        Me.Category.Name = "Category"
        Me.Category.Size = New System.Drawing.Size(548, 22)
        Me.Category.TabIndex = 10001
        '
        'Category_Label
        '
        Me.Category_Label.BackColor = System.Drawing.Color.DimGray
        Me.Category_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Category_Label.ForeColor = System.Drawing.Color.White
        Me.Category_Label.Location = New System.Drawing.Point(9, 46)
        Me.Category_Label.Margin = New System.Windows.Forms.Padding(0)
        Me.Category_Label.Name = "Category_Label"
        Me.Category_Label.Size = New System.Drawing.Size(120, 22)
        Me.Category_Label.TabIndex = 9999
        Me.Category_Label.Text = "Category_Label"
        Me.Category_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Btn_Cancel
        '
        Me.Btn_Cancel.Location = New System.Drawing.Point(586, 181)
        Me.Btn_Cancel.Name = "Btn_Cancel"
        Me.Btn_Cancel.Size = New System.Drawing.Size(44, 44)
        Me.Btn_Cancel.TabIndex = 10002
        Me.Btn_Cancel.TabStop = False
        '
        'FP_SELECT_PICTURE
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(680, 230)
        Me.ControlBox = False
        Me.Controls.Add(Me.Btn_Cancel)
        Me.Controls.Add(Me.Category_Label)
        Me.Controls.Add(Me.Category)
        Me.Controls.Add(Me.Descr_Label)
        Me.Controls.Add(Me.Btn_OK)
        Me.Controls.Add(Me.Title_Label)
        Me.Controls.Add(Me.Picture_Panel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FP_SELECT_PICTURE"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " "
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Picture_Panel As System.Windows.Forms.Panel
    Friend WithEvents Title_Label As System.Windows.Forms.Label
    Friend WithEvents Btn_OK As System.Windows.Forms.PictureBox
    Friend WithEvents Descr_Label As System.Windows.Forms.Label
    Friend WithEvents Category As System.Windows.Forms.ComboBox
    Friend WithEvents Category_Label As System.Windows.Forms.Label
    Friend WithEvents Btn_Cancel As System.Windows.Forms.PictureBox
End Class
