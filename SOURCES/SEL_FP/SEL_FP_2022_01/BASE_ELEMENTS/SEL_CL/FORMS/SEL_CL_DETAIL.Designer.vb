<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SEL_CL_DETAIL
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SEL_CL_DETAIL))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Calc_GRID = New System.Windows.Forms.DataGridView()
        Me.BTN_View = New System.Windows.Forms.PictureBox()
        Me.Calc_GRID_Label = New System.Windows.Forms.Label()
        Me.Inv_GRID = New System.Windows.Forms.DataGridView()
        Me.Inv_GRID_Label = New System.Windows.Forms.Label()
        Me.BTN_Help = New System.Windows.Forms.PictureBox()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.Calc_GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_View, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Inv_GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_Help, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.Location = New System.Drawing.Point(44, 12)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Calc_GRID)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Calc_GRID_Label)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.Inv_GRID)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Inv_GRID_Label)
        Me.SplitContainer1.Size = New System.Drawing.Size(1070, 542)
        Me.SplitContainer1.SplitterDistance = 237
        Me.SplitContainer1.TabIndex = 0
        '
        'Calc_GRID
        '
        Me.Calc_GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Calc_GRID.Location = New System.Drawing.Point(33, 40)
        Me.Calc_GRID.Name = "Calc_GRID"
        Me.Calc_GRID.Size = New System.Drawing.Size(1006, 192)
        Me.Calc_GRID.TabIndex = 100195
        '
        'BTN_View
        '
        Me.BTN_View.Location = New System.Drawing.Point(1070, 583)
        Me.BTN_View.Name = "BTN_View"
        Me.BTN_View.Size = New System.Drawing.Size(44, 44)
        Me.BTN_View.TabIndex = 100193
        Me.BTN_View.TabStop = False
        '
        'Calc_GRID_Label
        '
        Me.Calc_GRID_Label.BackColor = System.Drawing.Color.DimGray
        Me.Calc_GRID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Calc_GRID_Label.ForeColor = System.Drawing.Color.White
        Me.Calc_GRID_Label.Location = New System.Drawing.Point(30, 8)
        Me.Calc_GRID_Label.Name = "Calc_GRID_Label"
        Me.Calc_GRID_Label.Size = New System.Drawing.Size(1009, 22)
        Me.Calc_GRID_Label.TabIndex = 100194
        Me.Calc_GRID_Label.Text = "Calc_GRID_Label"
        Me.Calc_GRID_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Inv_GRID
        '
        Me.Inv_GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Inv_GRID.Location = New System.Drawing.Point(33, 36)
        Me.Inv_GRID.Name = "Inv_GRID"
        Me.Inv_GRID.Size = New System.Drawing.Size(1006, 260)
        Me.Inv_GRID.TabIndex = 100196
        '
        'Inv_GRID_Label
        '
        Me.Inv_GRID_Label.BackColor = System.Drawing.Color.DimGray
        Me.Inv_GRID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Inv_GRID_Label.ForeColor = System.Drawing.Color.White
        Me.Inv_GRID_Label.Location = New System.Drawing.Point(30, 10)
        Me.Inv_GRID_Label.Name = "Inv_GRID_Label"
        Me.Inv_GRID_Label.Size = New System.Drawing.Size(1009, 22)
        Me.Inv_GRID_Label.TabIndex = 100195
        Me.Inv_GRID_Label.Text = "Inv_GRID_Label"
        Me.Inv_GRID_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BTN_Help
        '
        Me.BTN_Help.Location = New System.Drawing.Point(44, 583)
        Me.BTN_Help.Name = "BTN_Help"
        Me.BTN_Help.Size = New System.Drawing.Size(44, 44)
        Me.BTN_Help.TabIndex = 100185
        Me.BTN_Help.TabStop = False
        '
        'SEL_CL_DETAIL
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(1149, 648)
        Me.Controls.Add(Me.BTN_Help)
        Me.Controls.Add(Me.BTN_View)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "SEL_CL_DETAIL"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SEL_CUST_CL_DETAIL"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.Calc_GRID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_View, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Inv_GRID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_Help, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Calc_GRID As System.Windows.Forms.DataGridView
    Friend WithEvents BTN_View As System.Windows.Forms.PictureBox
    Friend WithEvents Calc_GRID_Label As System.Windows.Forms.Label
    Friend WithEvents Inv_GRID As System.Windows.Forms.DataGridView
    Friend WithEvents Inv_GRID_Label As System.Windows.Forms.Label
    Friend WithEvents BTN_Help As System.Windows.Forms.PictureBox
End Class
