<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_Simple_SELECT
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FP_Simple_SELECT))
        Me.TextBox_Search = New System.Windows.Forms.TextBox()
        Me.GRID = New System.Windows.Forms.DataGridView()
        Me.GRID_Panel = New System.Windows.Forms.Panel()
        Me.GRID_Label = New System.Windows.Forms.Label()
        Me.GRID_Btn_FooterVisible = New System.Windows.Forms.PictureBox()
        Me.Btn_Hlp = New System.Windows.Forms.PictureBox()
        Me.Btn_Cancel = New System.Windows.Forms.PictureBox()
        Me.Btn_OK = New System.Windows.Forms.PictureBox()
        Me.Header_Panel = New System.Windows.Forms.Panel()
        Me.TextBox_Search_Panel = New System.Windows.Forms.Panel()
        Me.SavePoint = New System.Windows.Forms.TextBox()
        Me.MaxRecords_Label = New System.Windows.Forms.Label()
        Me.Btn_ExcelExport = New System.Windows.Forms.PictureBox()
        CType(Me.GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GRID_Btn_FooterVisible, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_Hlp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TextBox_Search_Panel.SuspendLayout()
        CType(Me.Btn_ExcelExport, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextBox_Search
        '
        Me.TextBox_Search.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TextBox_Search.ForeColor = System.Drawing.Color.Black
        Me.TextBox_Search.Location = New System.Drawing.Point(0, 0)
        Me.TextBox_Search.Name = "TextBox_Search"
        Me.TextBox_Search.Size = New System.Drawing.Size(450, 22)
        Me.TextBox_Search.TabIndex = 201
        '
        'GRID
        '
        Me.GRID.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GRID.Location = New System.Drawing.Point(0, 49)
        Me.GRID.Name = "GRID"
        Me.GRID.Size = New System.Drawing.Size(450, 232)
        Me.GRID.TabIndex = 300
        '
        'GRID_Panel
        '
        Me.GRID_Panel.BackColor = System.Drawing.Color.DimGray
        Me.GRID_Panel.Location = New System.Drawing.Point(0, 282)
        Me.GRID_Panel.Name = "GRID_Panel"
        Me.GRID_Panel.Size = New System.Drawing.Size(450, 115)
        Me.GRID_Panel.TabIndex = 400
        '
        'GRID_Label
        '
        Me.GRID_Label.BackColor = System.Drawing.Color.MidnightBlue
        Me.GRID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.GRID_Label.ForeColor = System.Drawing.Color.White
        Me.GRID_Label.Location = New System.Drawing.Point(0, 26)
        Me.GRID_Label.Name = "GRID_Label"
        Me.GRID_Label.Size = New System.Drawing.Size(428, 22)
        Me.GRID_Label.TabIndex = 9999
        '
        'GRID_Btn_FooterVisible
        '
        Me.GRID_Btn_FooterVisible.Location = New System.Drawing.Point(428, 26)
        Me.GRID_Btn_FooterVisible.Name = "GRID_Btn_FooterVisible"
        Me.GRID_Btn_FooterVisible.Size = New System.Drawing.Size(22, 22)
        Me.GRID_Btn_FooterVisible.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.GRID_Btn_FooterVisible.TabIndex = 100018
        Me.GRID_Btn_FooterVisible.TabStop = False
        '
        'Btn_Hlp
        '
        Me.Btn_Hlp.Location = New System.Drawing.Point(0, 400)
        Me.Btn_Hlp.Name = "Btn_Hlp"
        Me.Btn_Hlp.Size = New System.Drawing.Size(44, 44)
        Me.Btn_Hlp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Btn_Hlp.TabIndex = 100021
        Me.Btn_Hlp.TabStop = False
        '
        'Btn_Cancel
        '
        Me.Btn_Cancel.Location = New System.Drawing.Point(361, 400)
        Me.Btn_Cancel.Name = "Btn_Cancel"
        Me.Btn_Cancel.Size = New System.Drawing.Size(44, 44)
        Me.Btn_Cancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Btn_Cancel.TabIndex = 100020
        Me.Btn_Cancel.TabStop = False
        '
        'Btn_OK
        '
        Me.Btn_OK.Location = New System.Drawing.Point(406, 400)
        Me.Btn_OK.Name = "Btn_OK"
        Me.Btn_OK.Size = New System.Drawing.Size(44, 44)
        Me.Btn_OK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Btn_OK.TabIndex = 100019
        Me.Btn_OK.TabStop = False
        '
        'Header_Panel
        '
        Me.Header_Panel.Dock = System.Windows.Forms.DockStyle.Top
        Me.Header_Panel.Location = New System.Drawing.Point(0, 0)
        Me.Header_Panel.Name = "Header_Panel"
        Me.Header_Panel.Size = New System.Drawing.Size(450, 0)
        Me.Header_Panel.TabIndex = 100
        '
        'TextBox_Search_Panel
        '
        Me.TextBox_Search_Panel.Controls.Add(Me.TextBox_Search)
        Me.TextBox_Search_Panel.Controls.Add(Me.SavePoint)
        Me.TextBox_Search_Panel.Location = New System.Drawing.Point(0, 3)
        Me.TextBox_Search_Panel.Name = "TextBox_Search_Panel"
        Me.TextBox_Search_Panel.Size = New System.Drawing.Size(450, 22)
        Me.TextBox_Search_Panel.TabIndex = 200
        '
        'SavePoint
        '
        Me.SavePoint.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.SavePoint.ForeColor = System.Drawing.Color.Black
        Me.SavePoint.Location = New System.Drawing.Point(3, 0)
        Me.SavePoint.Name = "SavePoint"
        Me.SavePoint.Size = New System.Drawing.Size(25, 22)
        Me.SavePoint.TabIndex = 202
        '
        'MaxRecords_Label
        '
        Me.MaxRecords_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.MaxRecords_Label.Location = New System.Drawing.Point(0, 109)
        Me.MaxRecords_Label.Name = "MaxRecords_Label"
        Me.MaxRecords_Label.Size = New System.Drawing.Size(450, 22)
        Me.MaxRecords_Label.TabIndex = 9999
        Me.MaxRecords_Label.Text = "> {0}"
        Me.MaxRecords_Label.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.MaxRecords_Label.Visible = False
        '
        'Btn_ExcelExport
        '
        Me.Btn_ExcelExport.Location = New System.Drawing.Point(50, 400)
        Me.Btn_ExcelExport.Name = "Btn_ExcelExport"
        Me.Btn_ExcelExport.Size = New System.Drawing.Size(44, 44)
        Me.Btn_ExcelExport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Btn_ExcelExport.TabIndex = 100022
        Me.Btn_ExcelExport.TabStop = False
        '
        'FP_Simple_SELECT
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(450, 445)
        Me.ControlBox = False
        Me.Controls.Add(Me.Btn_ExcelExport)
        Me.Controls.Add(Me.MaxRecords_Label)
        Me.Controls.Add(Me.TextBox_Search_Panel)
        Me.Controls.Add(Me.Header_Panel)
        Me.Controls.Add(Me.Btn_Hlp)
        Me.Controls.Add(Me.Btn_Cancel)
        Me.Controls.Add(Me.Btn_OK)
        Me.Controls.Add(Me.GRID_Btn_FooterVisible)
        Me.Controls.Add(Me.GRID_Label)
        Me.Controls.Add(Me.GRID_Panel)
        Me.Controls.Add(Me.GRID)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FP_Simple_SELECT"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " "
        CType(Me.GRID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GRID_Btn_FooterVisible, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_Hlp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TextBox_Search_Panel.ResumeLayout(False)
        Me.TextBox_Search_Panel.PerformLayout()
        CType(Me.Btn_ExcelExport, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TextBox_Search As System.Windows.Forms.TextBox
    Friend WithEvents GRID As System.Windows.Forms.DataGridView
    Friend WithEvents GRID_Panel As System.Windows.Forms.Panel
    Friend WithEvents GRID_Label As System.Windows.Forms.Label
    Friend WithEvents GRID_Btn_FooterVisible As System.Windows.Forms.PictureBox
    Friend WithEvents Btn_Hlp As System.Windows.Forms.PictureBox
    Friend WithEvents Btn_Cancel As System.Windows.Forms.PictureBox
    Friend WithEvents Btn_OK As System.Windows.Forms.PictureBox
    Friend WithEvents Header_Panel As System.Windows.Forms.Panel
    Friend WithEvents TextBox_Search_Panel As System.Windows.Forms.Panel
    Friend WithEvents MaxRecords_Label As System.Windows.Forms.Label
    Friend WithEvents SavePoint As System.Windows.Forms.TextBox
    Friend WithEvents Btn_ExcelExport As System.Windows.Forms.PictureBox
End Class
