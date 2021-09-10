<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SEL_TASKMAN_GROUPS
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
        Me.GROUPS_GRID_Panel = New System.Windows.Forms.Panel()
        Me.GROUPS_L_GRID_Label = New System.Windows.Forms.Label()
        Me.GROUPS_L_GRID = New System.Windows.Forms.DataGridView()
        Me.GROUPS_L_SavePoint = New System.Windows.Forms.TextBox()
        Me.GROUPS_GRID_Label = New System.Windows.Forms.Label()
        Me.GROUPS_GRID = New System.Windows.Forms.DataGridView()
        Me.BTN_ExcelExport = New System.Windows.Forms.PictureBox()
        Me.BTN_Help = New System.Windows.Forms.PictureBox()
        Me.GROUPS_TabControl = New System.Windows.Forms.TabControl()
        Me.Tab_Members = New System.Windows.Forms.TabPage()
        Me.Tab_Rights = New System.Windows.Forms.TabPage()
        Me.Rights_INV_Allowed_Max = New System.Windows.Forms.TextBox()
        Me.Rights_INV_Allowed_Max_Label = New System.Windows.Forms.Label()
        Me.Rights_INV_Allowed_Max_Curr_ID = New System.Windows.Forms.ComboBox()
        Me.Rights_Title_Label = New System.Windows.Forms.Label()
        Me.Rights_CreditNote_Allowed_Max_Curr_ID = New System.Windows.Forms.ComboBox()
        Me.Rights_CreditNote_Allowed_Max = New System.Windows.Forms.TextBox()
        Me.Rights_CreditNote_Allowed_Max_Label = New System.Windows.Forms.Label()
        Me.Rights_INV_Allowed_Max_Task = New System.Windows.Forms.ComboBox()
        Me.Rights_INV_Allowed_Max_Task_Label = New System.Windows.Forms.Label()
        Me.Rights_CreditNote_Allowed_Max_Task = New System.Windows.Forms.ComboBox()
        Me.Rights_CreditNote_Allowed_Max_Task_Label = New System.Windows.Forms.Label()
        Me.GROUPS_GRID_Panel.SuspendLayout()
        CType(Me.GROUPS_L_GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GROUPS_GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_ExcelExport, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_Help, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GROUPS_TabControl.SuspendLayout()
        Me.Tab_Members.SuspendLayout()
        Me.Tab_Rights.SuspendLayout()
        Me.SuspendLayout()
        '
        'GROUPS_GRID_Panel
        '
        Me.GROUPS_GRID_Panel.Controls.Add(Me.GROUPS_L_GRID_Label)
        Me.GROUPS_GRID_Panel.Controls.Add(Me.GROUPS_L_GRID)
        Me.GROUPS_GRID_Panel.Controls.Add(Me.GROUPS_L_SavePoint)
        Me.GROUPS_GRID_Panel.Location = New System.Drawing.Point(9, 9)
        Me.GROUPS_GRID_Panel.Margin = New System.Windows.Forms.Padding(6)
        Me.GROUPS_GRID_Panel.Name = "GROUPS_GRID_Panel"
        Me.GROUPS_GRID_Panel.Size = New System.Drawing.Size(672, 339)
        Me.GROUPS_GRID_Panel.TabIndex = 1000
        '
        'GROUPS_L_GRID_Label
        '
        Me.GROUPS_L_GRID_Label.BackColor = System.Drawing.Color.SteelBlue
        Me.GROUPS_L_GRID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.GROUPS_L_GRID_Label.ForeColor = System.Drawing.Color.White
        Me.GROUPS_L_GRID_Label.Location = New System.Drawing.Point(0, 0)
        Me.GROUPS_L_GRID_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.GROUPS_L_GRID_Label.Name = "GROUPS_L_GRID_Label"
        Me.GROUPS_L_GRID_Label.Size = New System.Drawing.Size(660, 41)
        Me.GROUPS_L_GRID_Label.TabIndex = 9999
        Me.GROUPS_L_GRID_Label.Text = "CSOPORT TAGJAI"
        Me.GROUPS_L_GRID_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GROUPS_L_GRID
        '
        Me.GROUPS_L_GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GROUPS_L_GRID.Location = New System.Drawing.Point(5, 47)
        Me.GROUPS_L_GRID.Margin = New System.Windows.Forms.Padding(6)
        Me.GROUPS_L_GRID.Name = "GROUPS_L_GRID"
        Me.GROUPS_L_GRID.Size = New System.Drawing.Size(660, 297)
        Me.GROUPS_L_GRID.TabIndex = 1100
        '
        'GROUPS_L_SavePoint
        '
        Me.GROUPS_L_SavePoint.Location = New System.Drawing.Point(578, 284)
        Me.GROUPS_L_SavePoint.Name = "GROUPS_L_SavePoint"
        Me.GROUPS_L_SavePoint.Size = New System.Drawing.Size(100, 29)
        Me.GROUPS_L_SavePoint.TabIndex = 10307
        '
        'GROUPS_GRID_Label
        '
        Me.GROUPS_GRID_Label.BackColor = System.Drawing.Color.SteelBlue
        Me.GROUPS_GRID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.GROUPS_GRID_Label.ForeColor = System.Drawing.Color.White
        Me.GROUPS_GRID_Label.Location = New System.Drawing.Point(7, 82)
        Me.GROUPS_GRID_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.GROUPS_GRID_Label.Name = "GROUPS_GRID_Label"
        Me.GROUPS_GRID_Label.Size = New System.Drawing.Size(659, 41)
        Me.GROUPS_GRID_Label.TabIndex = 9999
        Me.GROUPS_GRID_Label.Text = "TASK MANAGEMENT CSOPORTOK"
        Me.GROUPS_GRID_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GROUPS_GRID
        '
        Me.GROUPS_GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GROUPS_GRID.Location = New System.Drawing.Point(7, 128)
        Me.GROUPS_GRID.Margin = New System.Windows.Forms.Padding(6)
        Me.GROUPS_GRID.Name = "GROUPS_GRID"
        Me.GROUPS_GRID.Size = New System.Drawing.Size(660, 249)
        Me.GROUPS_GRID.TabIndex = 100
        '
        'BTN_ExcelExport
        '
        Me.BTN_ExcelExport.Location = New System.Drawing.Point(504, -5)
        Me.BTN_ExcelExport.Margin = New System.Windows.Forms.Padding(6)
        Me.BTN_ExcelExport.Name = "BTN_ExcelExport"
        Me.BTN_ExcelExport.Size = New System.Drawing.Size(81, 81)
        Me.BTN_ExcelExport.TabIndex = 10306
        Me.BTN_ExcelExport.TabStop = False
        '
        'BTN_Help
        '
        Me.BTN_Help.Location = New System.Drawing.Point(585, -5)
        Me.BTN_Help.Margin = New System.Windows.Forms.Padding(6)
        Me.BTN_Help.Name = "BTN_Help"
        Me.BTN_Help.Size = New System.Drawing.Size(81, 81)
        Me.BTN_Help.TabIndex = 10305
        Me.BTN_Help.TabStop = False
        '
        'GROUPS_TabControl
        '
        Me.GROUPS_TabControl.Controls.Add(Me.Tab_Members)
        Me.GROUPS_TabControl.Controls.Add(Me.Tab_Rights)
        Me.GROUPS_TabControl.Location = New System.Drawing.Point(7, 423)
        Me.GROUPS_TabControl.Name = "GROUPS_TabControl"
        Me.GROUPS_TabControl.SelectedIndex = 0
        Me.GROUPS_TabControl.Size = New System.Drawing.Size(698, 391)
        Me.GROUPS_TabControl.TabIndex = 10307
        '
        'Tab_Members
        '
        Me.Tab_Members.Controls.Add(Me.GROUPS_GRID_Panel)
        Me.Tab_Members.Location = New System.Drawing.Point(4, 33)
        Me.Tab_Members.Name = "Tab_Members"
        Me.Tab_Members.Padding = New System.Windows.Forms.Padding(3)
        Me.Tab_Members.Size = New System.Drawing.Size(690, 354)
        Me.Tab_Members.TabIndex = 0
        Me.Tab_Members.Text = "Tab_Members"
        Me.Tab_Members.UseVisualStyleBackColor = True
        '
        'Tab_Rights
        '
        Me.Tab_Rights.Controls.Add(Me.Rights_CreditNote_Allowed_Max_Task)
        Me.Tab_Rights.Controls.Add(Me.Rights_CreditNote_Allowed_Max_Task_Label)
        Me.Tab_Rights.Controls.Add(Me.Rights_INV_Allowed_Max_Task)
        Me.Tab_Rights.Controls.Add(Me.Rights_INV_Allowed_Max_Task_Label)
        Me.Tab_Rights.Controls.Add(Me.Rights_CreditNote_Allowed_Max_Curr_ID)
        Me.Tab_Rights.Controls.Add(Me.Rights_CreditNote_Allowed_Max)
        Me.Tab_Rights.Controls.Add(Me.Rights_CreditNote_Allowed_Max_Label)
        Me.Tab_Rights.Controls.Add(Me.Rights_Title_Label)
        Me.Tab_Rights.Controls.Add(Me.Rights_INV_Allowed_Max_Curr_ID)
        Me.Tab_Rights.Controls.Add(Me.Rights_INV_Allowed_Max)
        Me.Tab_Rights.Controls.Add(Me.Rights_INV_Allowed_Max_Label)
        Me.Tab_Rights.Location = New System.Drawing.Point(4, 33)
        Me.Tab_Rights.Name = "Tab_Rights"
        Me.Tab_Rights.Padding = New System.Windows.Forms.Padding(3)
        Me.Tab_Rights.Size = New System.Drawing.Size(690, 354)
        Me.Tab_Rights.TabIndex = 1
        Me.Tab_Rights.Text = "Tab_Rights"
        Me.Tab_Rights.UseVisualStyleBackColor = True
        '
        'Rights_INV_Allowed_Max
        '
        Me.Rights_INV_Allowed_Max.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Rights_INV_Allowed_Max.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Rights_INV_Allowed_Max.Location = New System.Drawing.Point(244, 71)
        Me.Rights_INV_Allowed_Max.Margin = New System.Windows.Forms.Padding(6)
        Me.Rights_INV_Allowed_Max.Name = "Rights_INV_Allowed_Max"
        Me.Rights_INV_Allowed_Max.Size = New System.Drawing.Size(261, 33)
        Me.Rights_INV_Allowed_Max.TabIndex = 201
        '
        'Rights_INV_Allowed_Max_Label
        '
        Me.Rights_INV_Allowed_Max_Label.BackColor = System.Drawing.Color.DimGray
        Me.Rights_INV_Allowed_Max_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Rights_INV_Allowed_Max_Label.ForeColor = System.Drawing.Color.White
        Me.Rights_INV_Allowed_Max_Label.Location = New System.Drawing.Point(23, 71)
        Me.Rights_INV_Allowed_Max_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Rights_INV_Allowed_Max_Label.Name = "Rights_INV_Allowed_Max_Label"
        Me.Rights_INV_Allowed_Max_Label.Size = New System.Drawing.Size(220, 41)
        Me.Rights_INV_Allowed_Max_Label.TabIndex = 9999
        Me.Rights_INV_Allowed_Max_Label.Text = "Kiállítható számla maximális összege:"
        Me.Rights_INV_Allowed_Max_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Rights_INV_Allowed_Max_Curr_ID
        '
        Me.Rights_INV_Allowed_Max_Curr_ID.BackColor = System.Drawing.Color.White
        Me.Rights_INV_Allowed_Max_Curr_ID.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Rights_INV_Allowed_Max_Curr_ID.ForeColor = System.Drawing.Color.Black
        Me.Rights_INV_Allowed_Max_Curr_ID.FormattingEnabled = True
        Me.Rights_INV_Allowed_Max_Curr_ID.Location = New System.Drawing.Point(517, 70)
        Me.Rights_INV_Allowed_Max_Curr_ID.Margin = New System.Windows.Forms.Padding(6)
        Me.Rights_INV_Allowed_Max_Curr_ID.Name = "Rights_INV_Allowed_Max_Curr_ID"
        Me.Rights_INV_Allowed_Max_Curr_ID.Size = New System.Drawing.Size(101, 33)
        Me.Rights_INV_Allowed_Max_Curr_ID.TabIndex = 10000
        '
        'Rights_Title_Label
        '
        Me.Rights_Title_Label.BackColor = System.Drawing.Color.SteelBlue
        Me.Rights_Title_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Rights_Title_Label.ForeColor = System.Drawing.Color.White
        Me.Rights_Title_Label.Location = New System.Drawing.Point(15, 12)
        Me.Rights_Title_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Rights_Title_Label.Name = "Rights_Title_Label"
        Me.Rights_Title_Label.Size = New System.Drawing.Size(660, 41)
        Me.Rights_Title_Label.TabIndex = 10001
        Me.Rights_Title_Label.Text = "SZÁMLÁZÁSI JOGOSULTSÁGOK"
        Me.Rights_Title_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Rights_CreditNote_Allowed_Max_Curr_ID
        '
        Me.Rights_CreditNote_Allowed_Max_Curr_ID.BackColor = System.Drawing.Color.White
        Me.Rights_CreditNote_Allowed_Max_Curr_ID.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Rights_CreditNote_Allowed_Max_Curr_ID.ForeColor = System.Drawing.Color.Black
        Me.Rights_CreditNote_Allowed_Max_Curr_ID.FormattingEnabled = True
        Me.Rights_CreditNote_Allowed_Max_Curr_ID.Location = New System.Drawing.Point(508, 172)
        Me.Rights_CreditNote_Allowed_Max_Curr_ID.Margin = New System.Windows.Forms.Padding(6)
        Me.Rights_CreditNote_Allowed_Max_Curr_ID.Name = "Rights_CreditNote_Allowed_Max_Curr_ID"
        Me.Rights_CreditNote_Allowed_Max_Curr_ID.Size = New System.Drawing.Size(101, 33)
        Me.Rights_CreditNote_Allowed_Max_Curr_ID.TabIndex = 10004
        '
        'Rights_CreditNote_Allowed_Max
        '
        Me.Rights_CreditNote_Allowed_Max.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Rights_CreditNote_Allowed_Max.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Rights_CreditNote_Allowed_Max.Location = New System.Drawing.Point(235, 173)
        Me.Rights_CreditNote_Allowed_Max.Margin = New System.Windows.Forms.Padding(6)
        Me.Rights_CreditNote_Allowed_Max.Name = "Rights_CreditNote_Allowed_Max"
        Me.Rights_CreditNote_Allowed_Max.Size = New System.Drawing.Size(261, 33)
        Me.Rights_CreditNote_Allowed_Max.TabIndex = 10002
        '
        'Rights_CreditNote_Allowed_Max_Label
        '
        Me.Rights_CreditNote_Allowed_Max_Label.BackColor = System.Drawing.Color.DimGray
        Me.Rights_CreditNote_Allowed_Max_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Rights_CreditNote_Allowed_Max_Label.ForeColor = System.Drawing.Color.White
        Me.Rights_CreditNote_Allowed_Max_Label.Location = New System.Drawing.Point(14, 173)
        Me.Rights_CreditNote_Allowed_Max_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Rights_CreditNote_Allowed_Max_Label.Name = "Rights_CreditNote_Allowed_Max_Label"
        Me.Rights_CreditNote_Allowed_Max_Label.Size = New System.Drawing.Size(220, 41)
        Me.Rights_CreditNote_Allowed_Max_Label.TabIndex = 10003
        Me.Rights_CreditNote_Allowed_Max_Label.Text = "Kiállítható jóváírás maximális összege:"
        Me.Rights_CreditNote_Allowed_Max_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Rights_INV_Allowed_Max_Task
        '
        Me.Rights_INV_Allowed_Max_Task.BackColor = System.Drawing.Color.White
        Me.Rights_INV_Allowed_Max_Task.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Rights_INV_Allowed_Max_Task.ForeColor = System.Drawing.Color.Black
        Me.Rights_INV_Allowed_Max_Task.FormattingEnabled = True
        Me.Rights_INV_Allowed_Max_Task.Location = New System.Drawing.Point(544, 115)
        Me.Rights_INV_Allowed_Max_Task.Margin = New System.Windows.Forms.Padding(6)
        Me.Rights_INV_Allowed_Max_Task.Name = "Rights_INV_Allowed_Max_Task"
        Me.Rights_INV_Allowed_Max_Task.Size = New System.Drawing.Size(101, 33)
        Me.Rights_INV_Allowed_Max_Task.TabIndex = 10006
        '
        'Rights_INV_Allowed_Max_Task_Label
        '
        Me.Rights_INV_Allowed_Max_Task_Label.BackColor = System.Drawing.Color.DimGray
        Me.Rights_INV_Allowed_Max_Task_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Rights_INV_Allowed_Max_Task_Label.ForeColor = System.Drawing.Color.White
        Me.Rights_INV_Allowed_Max_Task_Label.Location = New System.Drawing.Point(50, 116)
        Me.Rights_INV_Allowed_Max_Task_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Rights_INV_Allowed_Max_Task_Label.Name = "Rights_INV_Allowed_Max_Task_Label"
        Me.Rights_INV_Allowed_Max_Task_Label.Size = New System.Drawing.Size(220, 41)
        Me.Rights_INV_Allowed_Max_Task_Label.TabIndex = 10005
        Me.Rights_INV_Allowed_Max_Task_Label.Text = "Engedélykérő TASK:"
        Me.Rights_INV_Allowed_Max_Task_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Rights_CreditNote_Allowed_Max_Task
        '
        Me.Rights_CreditNote_Allowed_Max_Task.BackColor = System.Drawing.Color.White
        Me.Rights_CreditNote_Allowed_Max_Task.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Rights_CreditNote_Allowed_Max_Task.ForeColor = System.Drawing.Color.Black
        Me.Rights_CreditNote_Allowed_Max_Task.FormattingEnabled = True
        Me.Rights_CreditNote_Allowed_Max_Task.Location = New System.Drawing.Point(517, 235)
        Me.Rights_CreditNote_Allowed_Max_Task.Margin = New System.Windows.Forms.Padding(6)
        Me.Rights_CreditNote_Allowed_Max_Task.Name = "Rights_CreditNote_Allowed_Max_Task"
        Me.Rights_CreditNote_Allowed_Max_Task.Size = New System.Drawing.Size(101, 33)
        Me.Rights_CreditNote_Allowed_Max_Task.TabIndex = 10008
        '
        'Rights_CreditNote_Allowed_Max_Task_Label
        '
        Me.Rights_CreditNote_Allowed_Max_Task_Label.BackColor = System.Drawing.Color.DimGray
        Me.Rights_CreditNote_Allowed_Max_Task_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Rights_CreditNote_Allowed_Max_Task_Label.ForeColor = System.Drawing.Color.White
        Me.Rights_CreditNote_Allowed_Max_Task_Label.Location = New System.Drawing.Point(23, 236)
        Me.Rights_CreditNote_Allowed_Max_Task_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Rights_CreditNote_Allowed_Max_Task_Label.Name = "Rights_CreditNote_Allowed_Max_Task_Label"
        Me.Rights_CreditNote_Allowed_Max_Task_Label.Size = New System.Drawing.Size(220, 41)
        Me.Rights_CreditNote_Allowed_Max_Task_Label.TabIndex = 10007
        Me.Rights_CreditNote_Allowed_Max_Task_Label.Text = "Kiállítható jóváírás maximális összege:"
        Me.Rights_CreditNote_Allowed_Max_Task_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SEL_TASKMAN_GROUPS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(717, 826)
        Me.Controls.Add(Me.GROUPS_TabControl)
        Me.Controls.Add(Me.GROUPS_GRID_Label)
        Me.Controls.Add(Me.GROUPS_GRID)
        Me.Controls.Add(Me.BTN_ExcelExport)
        Me.Controls.Add(Me.BTN_Help)
        Me.Name = "SEL_TASKMAN_GROUPS"
        Me.Text = "SEL_TASKMAN_GROUPS"
        Me.GROUPS_GRID_Panel.ResumeLayout(False)
        Me.GROUPS_GRID_Panel.PerformLayout()
        CType(Me.GROUPS_L_GRID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GROUPS_GRID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_ExcelExport, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_Help, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GROUPS_TabControl.ResumeLayout(False)
        Me.Tab_Members.ResumeLayout(False)
        Me.Tab_Rights.ResumeLayout(False)
        Me.Tab_Rights.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GROUPS_GRID_Panel As System.Windows.Forms.Panel
    Friend WithEvents GROUPS_L_GRID_Label As System.Windows.Forms.Label
    Friend WithEvents GROUPS_L_GRID As System.Windows.Forms.DataGridView
    Friend WithEvents GROUPS_GRID_Label As System.Windows.Forms.Label
    Friend WithEvents GROUPS_GRID As System.Windows.Forms.DataGridView
    Friend WithEvents BTN_ExcelExport As System.Windows.Forms.PictureBox
    Friend WithEvents BTN_Help As System.Windows.Forms.PictureBox
    Friend WithEvents GROUPS_L_SavePoint As System.Windows.Forms.TextBox
    Friend WithEvents GROUPS_TabControl As System.Windows.Forms.TabControl
    Friend WithEvents Tab_Members As System.Windows.Forms.TabPage
    Friend WithEvents Tab_Rights As System.Windows.Forms.TabPage
    Friend WithEvents Rights_INV_Allowed_Max As System.Windows.Forms.TextBox
    Friend WithEvents Rights_INV_Allowed_Max_Label As System.Windows.Forms.Label
    Friend WithEvents Rights_CreditNote_Allowed_Max_Task As System.Windows.Forms.ComboBox
    Friend WithEvents Rights_CreditNote_Allowed_Max_Task_Label As System.Windows.Forms.Label
    Friend WithEvents Rights_INV_Allowed_Max_Task As System.Windows.Forms.ComboBox
    Friend WithEvents Rights_INV_Allowed_Max_Task_Label As System.Windows.Forms.Label
    Friend WithEvents Rights_CreditNote_Allowed_Max_Curr_ID As System.Windows.Forms.ComboBox
    Friend WithEvents Rights_CreditNote_Allowed_Max As System.Windows.Forms.TextBox
    Friend WithEvents Rights_CreditNote_Allowed_Max_Label As System.Windows.Forms.Label
    Friend WithEvents Rights_Title_Label As System.Windows.Forms.Label
    Friend WithEvents Rights_INV_Allowed_Max_Curr_ID As System.Windows.Forms.ComboBox
End Class
