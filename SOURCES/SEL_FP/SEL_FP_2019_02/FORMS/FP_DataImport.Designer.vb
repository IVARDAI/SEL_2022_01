<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_DataImport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FP_DataImport))
        Me.Header_Panel = New System.Windows.Forms.Panel()
        Me.GRID_Btn_FooterVisible = New System.Windows.Forms.PictureBox()
        Me.GRID_Label = New System.Windows.Forms.Label()
        Me.GRID_Panel = New System.Windows.Forms.Panel()
        Me.GRID = New System.Windows.Forms.DataGridView()
        Me.Header_Label = New System.Windows.Forms.Label()
        Me.Btn_REFRESH = New System.Windows.Forms.PictureBox()
        Me.Btn_OK = New System.Windows.Forms.PictureBox()
        Me.Btn_Cancel = New System.Windows.Forms.PictureBox()
        Me.GRID_ERR_Label = New System.Windows.Forms.Label()
        Me.GRID_ERR = New System.Windows.Forms.DataGridView()
        Me.SplitContainer_MAIN = New System.Windows.Forms.SplitContainer()
        Me.GRID_SavePoint = New System.Windows.Forms.TextBox()
        Me.GRID_ERR_SavePoint = New System.Windows.Forms.TextBox()
        Me.Btn_HLP = New System.Windows.Forms.PictureBox()
        Me.Btn_ExcelExport = New System.Windows.Forms.PictureBox()
        CType(Me.GRID_Btn_FooterVisible, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_REFRESH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GRID_ERR, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer_MAIN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer_MAIN.Panel1.SuspendLayout()
        Me.SplitContainer_MAIN.Panel2.SuspendLayout()
        Me.SplitContainer_MAIN.SuspendLayout()
        CType(Me.Btn_HLP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_ExcelExport, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Header_Panel
        '
        Me.Header_Panel.Location = New System.Drawing.Point(7, 65)
        Me.Header_Panel.Name = "Header_Panel"
        Me.Header_Panel.Size = New System.Drawing.Size(902, 29)
        Me.Header_Panel.TabIndex = 1100
        '
        'GRID_Btn_FooterVisible
        '
        Me.GRID_Btn_FooterVisible.Location = New System.Drawing.Point(883, 34)
        Me.GRID_Btn_FooterVisible.Name = "GRID_Btn_FooterVisible"
        Me.GRID_Btn_FooterVisible.Size = New System.Drawing.Size(22, 22)
        Me.GRID_Btn_FooterVisible.TabIndex = 100022
        Me.GRID_Btn_FooterVisible.TabStop = False
        '
        'GRID_Label
        '
        Me.GRID_Label.BackColor = System.Drawing.Color.MidnightBlue
        Me.GRID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.GRID_Label.ForeColor = System.Drawing.Color.White
        Me.GRID_Label.Location = New System.Drawing.Point(3, 31)
        Me.GRID_Label.Name = "GRID_Label"
        Me.GRID_Label.Size = New System.Drawing.Size(873, 24)
        Me.GRID_Label.TabIndex = 9999
        '
        'GRID_Panel
        '
        Me.GRID_Panel.BackColor = System.Drawing.Color.DimGray
        Me.GRID_Panel.Location = New System.Drawing.Point(7, 138)
        Me.GRID_Panel.Name = "GRID_Panel"
        Me.GRID_Panel.Size = New System.Drawing.Size(902, 33)
        Me.GRID_Panel.TabIndex = 1300
        '
        'GRID
        '
        Me.GRID.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GRID.Location = New System.Drawing.Point(7, 100)
        Me.GRID.Name = "GRID"
        Me.GRID.Size = New System.Drawing.Size(902, 31)
        Me.GRID.TabIndex = 1200
        '
        'Header_Label
        '
        Me.Header_Label.BackColor = System.Drawing.Color.Maroon
        Me.Header_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Header_Label.ForeColor = System.Drawing.Color.Gold
        Me.Header_Label.Location = New System.Drawing.Point(3, 8)
        Me.Header_Label.Name = "Header_Label"
        Me.Header_Label.Size = New System.Drawing.Size(905, 24)
        Me.Header_Label.TabIndex = 9999
        '
        'Btn_REFRESH
        '
        Me.Btn_REFRESH.Location = New System.Drawing.Point(476, 418)
        Me.Btn_REFRESH.Name = "Btn_REFRESH"
        Me.Btn_REFRESH.Size = New System.Drawing.Size(44, 44)
        Me.Btn_REFRESH.TabIndex = 100036
        Me.Btn_REFRESH.TabStop = False
        '
        'Btn_OK
        '
        Me.Btn_OK.Location = New System.Drawing.Point(894, 418)
        Me.Btn_OK.Name = "Btn_OK"
        Me.Btn_OK.Size = New System.Drawing.Size(44, 44)
        Me.Btn_OK.TabIndex = 100035
        Me.Btn_OK.TabStop = False
        '
        'Btn_Cancel
        '
        Me.Btn_Cancel.Location = New System.Drawing.Point(844, 418)
        Me.Btn_Cancel.Name = "Btn_Cancel"
        Me.Btn_Cancel.Size = New System.Drawing.Size(44, 44)
        Me.Btn_Cancel.TabIndex = 100037
        Me.Btn_Cancel.TabStop = False
        '
        'GRID_ERR_Label
        '
        Me.GRID_ERR_Label.BackColor = System.Drawing.Color.MidnightBlue
        Me.GRID_ERR_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.GRID_ERR_Label.ForeColor = System.Drawing.Color.White
        Me.GRID_ERR_Label.Location = New System.Drawing.Point(3, 13)
        Me.GRID_ERR_Label.Name = "GRID_ERR_Label"
        Me.GRID_ERR_Label.Size = New System.Drawing.Size(905, 24)
        Me.GRID_ERR_Label.TabIndex = 9999
        Me.GRID_ERR_Label.Text = "ERROR LIST"
        Me.GRID_ERR_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GRID_ERR
        '
        Me.GRID_ERR.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.GRID_ERR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GRID_ERR.Location = New System.Drawing.Point(3, 40)
        Me.GRID_ERR.Name = "GRID_ERR"
        Me.GRID_ERR.Size = New System.Drawing.Size(902, 48)
        Me.GRID_ERR.TabIndex = 100038
        '
        'SplitContainer_MAIN
        '
        Me.SplitContainer_MAIN.Location = New System.Drawing.Point(14, 16)
        Me.SplitContainer_MAIN.Name = "SplitContainer_MAIN"
        Me.SplitContainer_MAIN.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer_MAIN.Panel1
        '
        Me.SplitContainer_MAIN.Panel1.Controls.Add(Me.GRID_Panel)
        Me.SplitContainer_MAIN.Panel1.Controls.Add(Me.Header_Label)
        Me.SplitContainer_MAIN.Panel1.Controls.Add(Me.GRID_Label)
        Me.SplitContainer_MAIN.Panel1.Controls.Add(Me.GRID_Btn_FooterVisible)
        Me.SplitContainer_MAIN.Panel1.Controls.Add(Me.Header_Panel)
        Me.SplitContainer_MAIN.Panel1.Controls.Add(Me.GRID)
        Me.SplitContainer_MAIN.Panel1.Controls.Add(Me.GRID_SavePoint)
        '
        'SplitContainer_MAIN.Panel2
        '
        Me.SplitContainer_MAIN.Panel2.Controls.Add(Me.GRID_ERR)
        Me.SplitContainer_MAIN.Panel2.Controls.Add(Me.GRID_ERR_Label)
        Me.SplitContainer_MAIN.Panel2.Controls.Add(Me.GRID_ERR_SavePoint)
        Me.SplitContainer_MAIN.Size = New System.Drawing.Size(930, 380)
        Me.SplitContainer_MAIN.SplitterDistance = 181
        Me.SplitContainer_MAIN.TabIndex = 100040
        '
        'GRID_SavePoint
        '
        Me.GRID_SavePoint.Location = New System.Drawing.Point(821, 127)
        Me.GRID_SavePoint.Name = "GRID_SavePoint"
        Me.GRID_SavePoint.Size = New System.Drawing.Size(18, 22)
        Me.GRID_SavePoint.TabIndex = 1999
        '
        'GRID_ERR_SavePoint
        '
        Me.GRID_ERR_SavePoint.Location = New System.Drawing.Point(887, 83)
        Me.GRID_ERR_SavePoint.Name = "GRID_ERR_SavePoint"
        Me.GRID_ERR_SavePoint.Size = New System.Drawing.Size(18, 22)
        Me.GRID_ERR_SavePoint.TabIndex = 2999
        '
        'Btn_HLP
        '
        Me.Btn_HLP.Location = New System.Drawing.Point(17, 418)
        Me.Btn_HLP.Name = "Btn_HLP"
        Me.Btn_HLP.Size = New System.Drawing.Size(44, 44)
        Me.Btn_HLP.TabIndex = 100041
        Me.Btn_HLP.TabStop = False
        '
        'Btn_ExcelExport
        '
        Me.Btn_ExcelExport.Location = New System.Drawing.Point(67, 418)
        Me.Btn_ExcelExport.Name = "Btn_ExcelExport"
        Me.Btn_ExcelExport.Size = New System.Drawing.Size(44, 44)
        Me.Btn_ExcelExport.TabIndex = 100041
        Me.Btn_ExcelExport.TabStop = False
        '
        'FP_DataImport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(959, 492)
        Me.ControlBox = False
        Me.Controls.Add(Me.Btn_ExcelExport)
        Me.Controls.Add(Me.Btn_HLP)
        Me.Controls.Add(Me.SplitContainer_MAIN)
        Me.Controls.Add(Me.Btn_Cancel)
        Me.Controls.Add(Me.Btn_REFRESH)
        Me.Controls.Add(Me.Btn_OK)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FP_DataImport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DataImport"
        CType(Me.GRID_Btn_FooterVisible, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GRID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_REFRESH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GRID_ERR, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer_MAIN.Panel1.ResumeLayout(False)
        Me.SplitContainer_MAIN.Panel1.PerformLayout()
        Me.SplitContainer_MAIN.Panel2.ResumeLayout(False)
        Me.SplitContainer_MAIN.Panel2.PerformLayout()
        CType(Me.SplitContainer_MAIN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer_MAIN.ResumeLayout(False)
        CType(Me.Btn_HLP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_ExcelExport, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Header_Panel As System.Windows.Forms.Panel
    Friend WithEvents GRID_Btn_FooterVisible As System.Windows.Forms.PictureBox
    Friend WithEvents GRID_Label As System.Windows.Forms.Label
    Friend WithEvents GRID_Panel As System.Windows.Forms.Panel
    Friend WithEvents GRID As System.Windows.Forms.DataGridView
    Friend WithEvents Header_Label As System.Windows.Forms.Label
    Friend WithEvents Btn_REFRESH As System.Windows.Forms.PictureBox
    Friend WithEvents Btn_OK As System.Windows.Forms.PictureBox
    Friend WithEvents Btn_Cancel As System.Windows.Forms.PictureBox
    Friend WithEvents GRID_ERR_Label As System.Windows.Forms.Label
    Friend WithEvents GRID_ERR As System.Windows.Forms.DataGridView
    Friend WithEvents SplitContainer_MAIN As System.Windows.Forms.SplitContainer
    Friend WithEvents GRID_SavePoint As System.Windows.Forms.TextBox
    Friend WithEvents GRID_ERR_SavePoint As System.Windows.Forms.TextBox
    Friend WithEvents Btn_HLP As System.Windows.Forms.PictureBox
    Friend WithEvents Btn_ExcelExport As System.Windows.Forms.PictureBox
End Class
