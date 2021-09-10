<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SEL_TASKMAN_FRM
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
        Me.BTN_TASKS = New System.Windows.Forms.PictureBox()
        Me.BTN_CHAT = New System.Windows.Forms.PictureBox()
        Me.BTN_HLP = New System.Windows.Forms.PictureBox()
        Me.Panel_TASKS = New System.Windows.Forms.Panel()
        Me.TASKS_Excel_Export = New System.Windows.Forms.PictureBox()
        Me.TASKS_Spec_Filter = New System.Windows.Forms.PictureBox()
        Me.TASKS_GRID = New System.Windows.Forms.DataGridView()
        Me.TASKS_Title_Label = New System.Windows.Forms.Label()
        Me.Panel_CHAT = New System.Windows.Forms.Panel()
        Me.CHAT_GRID = New System.Windows.Forms.DataGridView()
        Me.HISTORY_Spec_Filter = New System.Windows.Forms.PictureBox()
        Me.HISTORY_Excel_Export = New System.Windows.Forms.PictureBox()
        Me.CHATS_Title_Label = New System.Windows.Forms.Label()
        Me.BTN_ADD_NEW = New System.Windows.Forms.PictureBox()
        Me.BTN_EDIT_TASK = New System.Windows.Forms.PictureBox()
        Me.BTN_REFRESH = New System.Windows.Forms.PictureBox()
        Me.Panel_REMINDERS = New System.Windows.Forms.Panel()
        Me.REMINDERS_Excel_Export = New System.Windows.Forms.PictureBox()
        Me.REMINDERS_Spec_Filter = New System.Windows.Forms.PictureBox()
        Me.REMINDERS_GRID = New System.Windows.Forms.DataGridView()
        Me.REMINDERS_Title_Label = New System.Windows.Forms.Label()
        Me.BTN_REMINDERS = New System.Windows.Forms.PictureBox()
        Me.TASKS_INFO = New System.Windows.Forms.Label()
        Me.REMINDERS_INFO = New System.Windows.Forms.Label()
        Me.HISTORY_INFO = New System.Windows.Forms.Label()
        Me.TASKS_INFO_NEW = New System.Windows.Forms.Label()
        Me.REMINDERS_INFO_NEW = New System.Windows.Forms.Label()
        Me.HISTORY_INFO_NEW = New System.Windows.Forms.Label()
        CType(Me.BTN_TASKS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_CHAT, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_HLP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel_TASKS.SuspendLayout()
        CType(Me.TASKS_Excel_Export, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TASKS_Spec_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TASKS_GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel_CHAT.SuspendLayout()
        CType(Me.CHAT_GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HISTORY_Spec_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HISTORY_Excel_Export, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_ADD_NEW, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_EDIT_TASK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_REFRESH, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel_REMINDERS.SuspendLayout()
        CType(Me.REMINDERS_Excel_Export, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.REMINDERS_Spec_Filter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.REMINDERS_GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_REMINDERS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BTN_TASKS
        '
        Me.BTN_TASKS.BackColor = System.Drawing.Color.Transparent
        Me.BTN_TASKS.Location = New System.Drawing.Point(28, 24)
        Me.BTN_TASKS.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.BTN_TASKS.Name = "BTN_TASKS"
        Me.BTN_TASKS.Size = New System.Drawing.Size(79, 81)
        Me.BTN_TASKS.TabIndex = 10274
        Me.BTN_TASKS.TabStop = False
        '
        'BTN_CHAT
        '
        Me.BTN_CHAT.BackColor = System.Drawing.Color.Transparent
        Me.BTN_CHAT.Location = New System.Drawing.Point(450, 24)
        Me.BTN_CHAT.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.BTN_CHAT.Name = "BTN_CHAT"
        Me.BTN_CHAT.Size = New System.Drawing.Size(79, 81)
        Me.BTN_CHAT.TabIndex = 10275
        Me.BTN_CHAT.TabStop = False
        '
        'BTN_HLP
        '
        Me.BTN_HLP.BackColor = System.Drawing.Color.Transparent
        Me.BTN_HLP.Location = New System.Drawing.Point(1155, 707)
        Me.BTN_HLP.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.BTN_HLP.Name = "BTN_HLP"
        Me.BTN_HLP.Size = New System.Drawing.Size(42, 50)
        Me.BTN_HLP.TabIndex = 10278
        Me.BTN_HLP.TabStop = False
        '
        'Panel_TASKS
        '
        Me.Panel_TASKS.Controls.Add(Me.TASKS_Excel_Export)
        Me.Panel_TASKS.Controls.Add(Me.TASKS_Spec_Filter)
        Me.Panel_TASKS.Controls.Add(Me.TASKS_GRID)
        Me.Panel_TASKS.Controls.Add(Me.TASKS_Title_Label)
        Me.Panel_TASKS.Location = New System.Drawing.Point(45, 200)
        Me.Panel_TASKS.Name = "Panel_TASKS"
        Me.Panel_TASKS.Size = New System.Drawing.Size(301, 418)
        Me.Panel_TASKS.TabIndex = 2000
        '
        'TASKS_Excel_Export
        '
        Me.TASKS_Excel_Export.BackColor = System.Drawing.Color.Transparent
        Me.TASKS_Excel_Export.Location = New System.Drawing.Point(3, 21)
        Me.TASKS_Excel_Export.Name = "TASKS_Excel_Export"
        Me.TASKS_Excel_Export.Size = New System.Drawing.Size(41, 41)
        Me.TASKS_Excel_Export.TabIndex = 10002
        Me.TASKS_Excel_Export.TabStop = False
        '
        'TASKS_Spec_Filter
        '
        Me.TASKS_Spec_Filter.BackColor = System.Drawing.Color.Transparent
        Me.TASKS_Spec_Filter.Location = New System.Drawing.Point(240, 21)
        Me.TASKS_Spec_Filter.Name = "TASKS_Spec_Filter"
        Me.TASKS_Spec_Filter.Size = New System.Drawing.Size(41, 41)
        Me.TASKS_Spec_Filter.TabIndex = 10001
        Me.TASKS_Spec_Filter.TabStop = False
        '
        'TASKS_GRID
        '
        Me.TASKS_GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TASKS_GRID.Location = New System.Drawing.Point(39, 100)
        Me.TASKS_GRID.Name = "TASKS_GRID"
        Me.TASKS_GRID.Size = New System.Drawing.Size(242, 224)
        Me.TASKS_GRID.TabIndex = 10000
        '
        'TASKS_Title_Label
        '
        Me.TASKS_Title_Label.BackColor = System.Drawing.Color.MidnightBlue
        Me.TASKS_Title_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TASKS_Title_Label.ForeColor = System.Drawing.Color.White
        Me.TASKS_Title_Label.Location = New System.Drawing.Point(16, 21)
        Me.TASKS_Title_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.TASKS_Title_Label.Name = "TASKS_Title_Label"
        Me.TASKS_Title_Label.Size = New System.Drawing.Size(242, 41)
        Me.TASKS_Title_Label.TabIndex = 9999
        Me.TASKS_Title_Label.Text = "TASKS"
        Me.TASKS_Title_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel_CHAT
        '
        Me.Panel_CHAT.Controls.Add(Me.CHAT_GRID)
        Me.Panel_CHAT.Controls.Add(Me.HISTORY_Spec_Filter)
        Me.Panel_CHAT.Controls.Add(Me.HISTORY_Excel_Export)
        Me.Panel_CHAT.Controls.Add(Me.CHATS_Title_Label)
        Me.Panel_CHAT.Location = New System.Drawing.Point(906, 200)
        Me.Panel_CHAT.Name = "Panel_CHAT"
        Me.Panel_CHAT.Size = New System.Drawing.Size(301, 418)
        Me.Panel_CHAT.TabIndex = 3000
        '
        'CHAT_GRID
        '
        Me.CHAT_GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.CHAT_GRID.Location = New System.Drawing.Point(29, 100)
        Me.CHAT_GRID.Name = "CHAT_GRID"
        Me.CHAT_GRID.Size = New System.Drawing.Size(242, 224)
        Me.CHAT_GRID.TabIndex = 10001
        '
        'HISTORY_Spec_Filter
        '
        Me.HISTORY_Spec_Filter.BackColor = System.Drawing.Color.Transparent
        Me.HISTORY_Spec_Filter.Location = New System.Drawing.Point(246, 21)
        Me.HISTORY_Spec_Filter.Name = "HISTORY_Spec_Filter"
        Me.HISTORY_Spec_Filter.Size = New System.Drawing.Size(41, 41)
        Me.HISTORY_Spec_Filter.TabIndex = 10003
        Me.HISTORY_Spec_Filter.TabStop = False
        '
        'HISTORY_Excel_Export
        '
        Me.HISTORY_Excel_Export.BackColor = System.Drawing.Color.Transparent
        Me.HISTORY_Excel_Export.Location = New System.Drawing.Point(9, 21)
        Me.HISTORY_Excel_Export.Name = "HISTORY_Excel_Export"
        Me.HISTORY_Excel_Export.Size = New System.Drawing.Size(41, 41)
        Me.HISTORY_Excel_Export.TabIndex = 10004
        Me.HISTORY_Excel_Export.TabStop = False
        '
        'CHATS_Title_Label
        '
        Me.CHATS_Title_Label.BackColor = System.Drawing.Color.MidnightBlue
        Me.CHATS_Title_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.CHATS_Title_Label.ForeColor = System.Drawing.Color.White
        Me.CHATS_Title_Label.Location = New System.Drawing.Point(24, 21)
        Me.CHATS_Title_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.CHATS_Title_Label.Name = "CHATS_Title_Label"
        Me.CHATS_Title_Label.Size = New System.Drawing.Size(242, 41)
        Me.CHATS_Title_Label.TabIndex = 9999
        Me.CHATS_Title_Label.Text = "LAST ACTIVITIES"
        Me.CHATS_Title_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BTN_ADD_NEW
        '
        Me.BTN_ADD_NEW.BackColor = System.Drawing.Color.Transparent
        Me.BTN_ADD_NEW.Location = New System.Drawing.Point(45, 699)
        Me.BTN_ADD_NEW.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.BTN_ADD_NEW.Name = "BTN_ADD_NEW"
        Me.BTN_ADD_NEW.Size = New System.Drawing.Size(79, 81)
        Me.BTN_ADD_NEW.TabIndex = 10282
        Me.BTN_ADD_NEW.TabStop = False
        '
        'BTN_EDIT_TASK
        '
        Me.BTN_EDIT_TASK.BackColor = System.Drawing.Color.Transparent
        Me.BTN_EDIT_TASK.Location = New System.Drawing.Point(171, 699)
        Me.BTN_EDIT_TASK.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.BTN_EDIT_TASK.Name = "BTN_EDIT_TASK"
        Me.BTN_EDIT_TASK.Size = New System.Drawing.Size(79, 81)
        Me.BTN_EDIT_TASK.TabIndex = 10283
        Me.BTN_EDIT_TASK.TabStop = False
        '
        'BTN_REFRESH
        '
        Me.BTN_REFRESH.BackColor = System.Drawing.Color.Transparent
        Me.BTN_REFRESH.Location = New System.Drawing.Point(1040, 687)
        Me.BTN_REFRESH.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.BTN_REFRESH.Name = "BTN_REFRESH"
        Me.BTN_REFRESH.Size = New System.Drawing.Size(79, 81)
        Me.BTN_REFRESH.TabIndex = 10285
        Me.BTN_REFRESH.TabStop = False
        '
        'Panel_REMINDERS
        '
        Me.Panel_REMINDERS.Controls.Add(Me.REMINDERS_Excel_Export)
        Me.Panel_REMINDERS.Controls.Add(Me.REMINDERS_Spec_Filter)
        Me.Panel_REMINDERS.Controls.Add(Me.REMINDERS_GRID)
        Me.Panel_REMINDERS.Controls.Add(Me.REMINDERS_Title_Label)
        Me.Panel_REMINDERS.Location = New System.Drawing.Point(450, 200)
        Me.Panel_REMINDERS.Name = "Panel_REMINDERS"
        Me.Panel_REMINDERS.Size = New System.Drawing.Size(301, 418)
        Me.Panel_REMINDERS.TabIndex = 10002
        '
        'REMINDERS_Excel_Export
        '
        Me.REMINDERS_Excel_Export.BackColor = System.Drawing.Color.Transparent
        Me.REMINDERS_Excel_Export.Location = New System.Drawing.Point(13, 21)
        Me.REMINDERS_Excel_Export.Name = "REMINDERS_Excel_Export"
        Me.REMINDERS_Excel_Export.Size = New System.Drawing.Size(41, 41)
        Me.REMINDERS_Excel_Export.TabIndex = 10006
        Me.REMINDERS_Excel_Export.TabStop = False
        '
        'REMINDERS_Spec_Filter
        '
        Me.REMINDERS_Spec_Filter.BackColor = System.Drawing.Color.Transparent
        Me.REMINDERS_Spec_Filter.Location = New System.Drawing.Point(253, 21)
        Me.REMINDERS_Spec_Filter.Name = "REMINDERS_Spec_Filter"
        Me.REMINDERS_Spec_Filter.Size = New System.Drawing.Size(41, 41)
        Me.REMINDERS_Spec_Filter.TabIndex = 10005
        Me.REMINDERS_Spec_Filter.TabStop = False
        '
        'REMINDERS_GRID
        '
        Me.REMINDERS_GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.REMINDERS_GRID.Location = New System.Drawing.Point(31, 100)
        Me.REMINDERS_GRID.Name = "REMINDERS_GRID"
        Me.REMINDERS_GRID.Size = New System.Drawing.Size(242, 224)
        Me.REMINDERS_GRID.TabIndex = 10001
        '
        'REMINDERS_Title_Label
        '
        Me.REMINDERS_Title_Label.BackColor = System.Drawing.Color.MidnightBlue
        Me.REMINDERS_Title_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.REMINDERS_Title_Label.ForeColor = System.Drawing.Color.White
        Me.REMINDERS_Title_Label.Location = New System.Drawing.Point(26, 21)
        Me.REMINDERS_Title_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.REMINDERS_Title_Label.Name = "REMINDERS_Title_Label"
        Me.REMINDERS_Title_Label.Size = New System.Drawing.Size(242, 41)
        Me.REMINDERS_Title_Label.TabIndex = 9999
        Me.REMINDERS_Title_Label.Text = "REMINDERS OPEN"
        Me.REMINDERS_Title_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BTN_REMINDERS
        '
        Me.BTN_REMINDERS.BackColor = System.Drawing.Color.Transparent
        Me.BTN_REMINDERS.Location = New System.Drawing.Point(906, 24)
        Me.BTN_REMINDERS.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.BTN_REMINDERS.Name = "BTN_REMINDERS"
        Me.BTN_REMINDERS.Size = New System.Drawing.Size(79, 81)
        Me.BTN_REMINDERS.TabIndex = 10286
        Me.BTN_REMINDERS.TabStop = False
        '
        'TASKS_INFO
        '
        Me.TASKS_INFO.BackColor = System.Drawing.Color.Transparent
        Me.TASKS_INFO.Font = New System.Drawing.Font("Tahoma", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TASKS_INFO.ForeColor = System.Drawing.Color.DimGray
        Me.TASKS_INFO.Location = New System.Drawing.Point(21, 122)
        Me.TASKS_INFO.Name = "TASKS_INFO"
        Me.TASKS_INFO.Size = New System.Drawing.Size(97, 44)
        Me.TASKS_INFO.TabIndex = 9999
        Me.TASKS_INFO.Text = "TASKS_INFO"
        Me.TASKS_INFO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.TASKS_INFO.Visible = False
        '
        'REMINDERS_INFO
        '
        Me.REMINDERS_INFO.BackColor = System.Drawing.Color.Transparent
        Me.REMINDERS_INFO.Font = New System.Drawing.Font("Tahoma", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.REMINDERS_INFO.ForeColor = System.Drawing.Color.DimGray
        Me.REMINDERS_INFO.Location = New System.Drawing.Point(443, 122)
        Me.REMINDERS_INFO.Name = "REMINDERS_INFO"
        Me.REMINDERS_INFO.Size = New System.Drawing.Size(97, 44)
        Me.REMINDERS_INFO.TabIndex = 9999
        Me.REMINDERS_INFO.Text = "REMINDERS_INFO"
        Me.REMINDERS_INFO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.REMINDERS_INFO.Visible = False
        '
        'HISTORY_INFO
        '
        Me.HISTORY_INFO.BackColor = System.Drawing.Color.Transparent
        Me.HISTORY_INFO.Font = New System.Drawing.Font("Tahoma", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.HISTORY_INFO.ForeColor = System.Drawing.Color.DimGray
        Me.HISTORY_INFO.Location = New System.Drawing.Point(899, 122)
        Me.HISTORY_INFO.Name = "HISTORY_INFO"
        Me.HISTORY_INFO.Size = New System.Drawing.Size(97, 44)
        Me.HISTORY_INFO.TabIndex = 9999
        Me.HISTORY_INFO.Text = "HISTORY_INFO"
        Me.HISTORY_INFO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.HISTORY_INFO.Visible = False
        '
        'TASKS_INFO_NEW
        '
        Me.TASKS_INFO_NEW.BackColor = System.Drawing.Color.Transparent
        Me.TASKS_INFO_NEW.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TASKS_INFO_NEW.ForeColor = System.Drawing.Color.Red
        Me.TASKS_INFO_NEW.Location = New System.Drawing.Point(27, 166)
        Me.TASKS_INFO_NEW.Name = "TASKS_INFO_NEW"
        Me.TASKS_INFO_NEW.Size = New System.Drawing.Size(97, 30)
        Me.TASKS_INFO_NEW.TabIndex = 9999
        Me.TASKS_INFO_NEW.Text = "TASKS_INFO_NEW"
        Me.TASKS_INFO_NEW.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.TASKS_INFO_NEW.Visible = False
        '
        'REMINDERS_INFO_NEW
        '
        Me.REMINDERS_INFO_NEW.BackColor = System.Drawing.Color.Transparent
        Me.REMINDERS_INFO_NEW.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.REMINDERS_INFO_NEW.ForeColor = System.Drawing.Color.Red
        Me.REMINDERS_INFO_NEW.Location = New System.Drawing.Point(445, 166)
        Me.REMINDERS_INFO_NEW.Name = "REMINDERS_INFO_NEW"
        Me.REMINDERS_INFO_NEW.Size = New System.Drawing.Size(97, 30)
        Me.REMINDERS_INFO_NEW.TabIndex = 9999
        Me.REMINDERS_INFO_NEW.Text = "REMINDERS_INFO_NEW"
        Me.REMINDERS_INFO_NEW.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.REMINDERS_INFO_NEW.Visible = False
        '
        'HISTORY_INFO_NEW
        '
        Me.HISTORY_INFO_NEW.BackColor = System.Drawing.Color.Transparent
        Me.HISTORY_INFO_NEW.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.HISTORY_INFO_NEW.ForeColor = System.Drawing.Color.Red
        Me.HISTORY_INFO_NEW.Location = New System.Drawing.Point(901, 166)
        Me.HISTORY_INFO_NEW.Name = "HISTORY_INFO_NEW"
        Me.HISTORY_INFO_NEW.Size = New System.Drawing.Size(97, 30)
        Me.HISTORY_INFO_NEW.TabIndex = 9999
        Me.HISTORY_INFO_NEW.Text = "HISTORY_INFO_NEW"
        Me.HISTORY_INFO_NEW.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.HISTORY_INFO_NEW.Visible = False
        '
        'SEL_TASKMAN_FRM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Beige
        Me.ClientSize = New System.Drawing.Size(1243, 777)
        Me.Controls.Add(Me.HISTORY_INFO_NEW)
        Me.Controls.Add(Me.REMINDERS_INFO_NEW)
        Me.Controls.Add(Me.TASKS_INFO_NEW)
        Me.Controls.Add(Me.HISTORY_INFO)
        Me.Controls.Add(Me.REMINDERS_INFO)
        Me.Controls.Add(Me.TASKS_INFO)
        Me.Controls.Add(Me.BTN_REMINDERS)
        Me.Controls.Add(Me.Panel_REMINDERS)
        Me.Controls.Add(Me.BTN_REFRESH)
        Me.Controls.Add(Me.BTN_EDIT_TASK)
        Me.Controls.Add(Me.BTN_ADD_NEW)
        Me.Controls.Add(Me.Panel_CHAT)
        Me.Controls.Add(Me.Panel_TASKS)
        Me.Controls.Add(Me.BTN_HLP)
        Me.Controls.Add(Me.BTN_CHAT)
        Me.Controls.Add(Me.BTN_TASKS)
        Me.DoubleBuffered = True
        Me.Name = "SEL_TASKMAN_FRM"
        Me.ShowInTaskbar = False
        Me.Text = "e5xrtsdyw4"
        CType(Me.BTN_TASKS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_CHAT, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_HLP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel_TASKS.ResumeLayout(False)
        CType(Me.TASKS_Excel_Export, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TASKS_Spec_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TASKS_GRID, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel_CHAT.ResumeLayout(False)
        CType(Me.CHAT_GRID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HISTORY_Spec_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HISTORY_Excel_Export, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_ADD_NEW, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_EDIT_TASK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_REFRESH, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel_REMINDERS.ResumeLayout(False)
        CType(Me.REMINDERS_Excel_Export, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.REMINDERS_Spec_Filter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.REMINDERS_GRID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_REMINDERS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BTN_TASKS As System.Windows.Forms.PictureBox
    Friend WithEvents BTN_CHAT As System.Windows.Forms.PictureBox
    Friend WithEvents BTN_HLP As System.Windows.Forms.PictureBox
    Friend WithEvents Panel_TASKS As System.Windows.Forms.Panel
    Friend WithEvents Panel_CHAT As System.Windows.Forms.Panel
    Friend WithEvents TASKS_Title_Label As System.Windows.Forms.Label
    Friend WithEvents CHATS_Title_Label As System.Windows.Forms.Label
    Friend WithEvents BTN_ADD_NEW As System.Windows.Forms.PictureBox
    Friend WithEvents BTN_EDIT_TASK As System.Windows.Forms.PictureBox
    Friend WithEvents TASKS_GRID As System.Windows.Forms.DataGridView
    Friend WithEvents BTN_REFRESH As System.Windows.Forms.PictureBox
    Friend WithEvents CHAT_GRID As System.Windows.Forms.DataGridView
    Friend WithEvents Panel_REMINDERS As System.Windows.Forms.Panel
    Friend WithEvents REMINDERS_GRID As System.Windows.Forms.DataGridView
    Friend WithEvents REMINDERS_Title_Label As System.Windows.Forms.Label
    Friend WithEvents BTN_REMINDERS As System.Windows.Forms.PictureBox
    Friend WithEvents TASKS_INFO As System.Windows.Forms.Label
    Friend WithEvents REMINDERS_INFO As System.Windows.Forms.Label
    Friend WithEvents HISTORY_INFO As System.Windows.Forms.Label
    Friend WithEvents TASKS_INFO_NEW As System.Windows.Forms.Label
    Friend WithEvents REMINDERS_INFO_NEW As System.Windows.Forms.Label
    Friend WithEvents HISTORY_INFO_NEW As System.Windows.Forms.Label
    Friend WithEvents TASKS_Excel_Export As System.Windows.Forms.PictureBox
    Friend WithEvents TASKS_Spec_Filter As System.Windows.Forms.PictureBox
    Friend WithEvents HISTORY_Excel_Export As System.Windows.Forms.PictureBox
    Friend WithEvents HISTORY_Spec_Filter As System.Windows.Forms.PictureBox
    Friend WithEvents REMINDERS_Excel_Export As System.Windows.Forms.PictureBox
    Friend WithEvents REMINDERS_Spec_Filter As System.Windows.Forms.PictureBox
End Class
