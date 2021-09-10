<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SEL_TASKMAN_TASK
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Title_Label = New System.Windows.Forms.Label()
        Me.CHAT_List_Label = New System.Windows.Forms.Label()
        Me.TaskNum_Label = New System.Windows.Forms.Label()
        Me.TaskNum = New System.Windows.Forms.TextBox()
        Me.Added_Users_Name_Label = New System.Windows.Forms.Label()
        Me.RESP_Panel_Label = New System.Windows.Forms.Label()
        Me.Descr = New System.Windows.Forms.TextBox()
        Me.Descr_Label = New System.Windows.Forms.Label()
        Me.TASK_TYPES_ID_Label = New System.Windows.Forms.Label()
        Me.Added_Date = New System.Windows.Forms.TextBox()
        Me.Added_Date_Label = New System.Windows.Forms.Label()
        Me.DueDate = New System.Windows.Forms.TextBox()
        Me.DueDate_Label = New System.Windows.Forms.Label()
        Me.StatusID_Label = New System.Windows.Forms.Label()
        Me.CHAT_List = New System.Windows.Forms.ListView()
        Me.CHAT_Text_Label = New System.Windows.Forms.Label()
        Me.CHAT_Text = New System.Windows.Forms.TextBox()
        Me.Connected_Records_Panel_Label = New System.Windows.Forms.Label()
        Me.ATTACHED_DOCS_Panel = New System.Windows.Forms.Panel()
        Me.BTN_REFRESH = New System.Windows.Forms.PictureBox()
        Me.BTN_OK = New System.Windows.Forms.PictureBox()
        Me.BTN_HLP = New System.Windows.Forms.PictureBox()
        Me.Added_Users_Name = New System.Windows.Forms.TextBox()
        Me.TASK_TYPES_ID = New System.Windows.Forms.ComboBox()
        Me.StatusID = New System.Windows.Forms.ComboBox()
        Me.TM_RESP_GRID = New System.Windows.Forms.DataGridView()
        Me.TM_CON_REC_GRID = New System.Windows.Forms.DataGridView()
        Me.BTN_PUBLISH = New System.Windows.Forms.Button()
        Me.RESP_Panel = New System.Windows.Forms.Panel()
        Me.TM_RESP_SavePoint = New System.Windows.Forms.TextBox()
        Me.Connected_Records_Panel = New System.Windows.Forms.Panel()
        Me.TM_CON_REC_SaveRecord = New System.Windows.Forms.TextBox()
        Me.Schedule_INFO_Label = New System.Windows.Forms.Label()
        Me.Schedule_INFO = New System.Windows.Forms.RichTextBox()
        Me.BTN_Schedule = New System.Windows.Forms.PictureBox()
        Me.Answer_YN_Label = New System.Windows.Forms.Label()
        Me.Answer_Panel = New System.Windows.Forms.Panel()
        Me.Answer_NO_Label = New System.Windows.Forms.Label()
        Me.Answer_YES_Label = New System.Windows.Forms.Label()
        Me.Answer_NO = New System.Windows.Forms.RadioButton()
        Me.Answer_YES = New System.Windows.Forms.RadioButton()
        Me.BTN_CANCEL = New System.Windows.Forms.Button()
        CType(Me.BTN_REFRESH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_OK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_HLP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TM_RESP_GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TM_CON_REC_GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RESP_Panel.SuspendLayout()
        Me.Connected_Records_Panel.SuspendLayout()
        CType(Me.BTN_Schedule, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Answer_Panel.SuspendLayout()
        Me.SuspendLayout()
        '
        'Title_Label
        '
        Me.Title_Label.BackColor = System.Drawing.Color.MidnightBlue
        Me.Title_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Title_Label.ForeColor = System.Drawing.Color.White
        Me.Title_Label.Location = New System.Drawing.Point(15, 21)
        Me.Title_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Title_Label.Name = "Title_Label"
        Me.Title_Label.Size = New System.Drawing.Size(827, 41)
        Me.Title_Label.TabIndex = 9999
        Me.Title_Label.Text = "FELADAT RÉSZLETES ADATAI"
        Me.Title_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CHAT_List_Label
        '
        Me.CHAT_List_Label.BackColor = System.Drawing.Color.SteelBlue
        Me.CHAT_List_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.CHAT_List_Label.ForeColor = System.Drawing.Color.White
        Me.CHAT_List_Label.Location = New System.Drawing.Point(15, 432)
        Me.CHAT_List_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.CHAT_List_Label.Name = "CHAT_List_Label"
        Me.CHAT_List_Label.Size = New System.Drawing.Size(862, 41)
        Me.CHAT_List_Label.TabIndex = 9999
        Me.CHAT_List_Label.Text = "CHAT"
        Me.CHAT_List_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TaskNum_Label
        '
        Me.TaskNum_Label.BackColor = System.Drawing.Color.DimGray
        Me.TaskNum_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.TaskNum_Label.ForeColor = System.Drawing.Color.White
        Me.TaskNum_Label.Location = New System.Drawing.Point(15, 82)
        Me.TaskNum_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.TaskNum_Label.Name = "TaskNum_Label"
        Me.TaskNum_Label.Size = New System.Drawing.Size(257, 41)
        Me.TaskNum_Label.TabIndex = 9999
        Me.TaskNum_Label.Text = "Feladat száma:"
        '
        'TaskNum
        '
        Me.TaskNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TaskNum.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.TaskNum.Location = New System.Drawing.Point(284, 80)
        Me.TaskNum.Margin = New System.Windows.Forms.Padding(6)
        Me.TaskNum.Name = "TaskNum"
        Me.TaskNum.Size = New System.Drawing.Size(603, 33)
        Me.TaskNum.TabIndex = 1
        '
        'Added_Users_Name_Label
        '
        Me.Added_Users_Name_Label.BackColor = System.Drawing.Color.DimGray
        Me.Added_Users_Name_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Added_Users_Name_Label.ForeColor = System.Drawing.Color.White
        Me.Added_Users_Name_Label.Location = New System.Drawing.Point(15, 134)
        Me.Added_Users_Name_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Added_Users_Name_Label.Name = "Added_Users_Name_Label"
        Me.Added_Users_Name_Label.Size = New System.Drawing.Size(257, 41)
        Me.Added_Users_Name_Label.TabIndex = 9999
        Me.Added_Users_Name_Label.Text = "Létrehozta:"
        '
        'RESP_Panel_Label
        '
        Me.RESP_Panel_Label.BackColor = System.Drawing.Color.DimGray
        Me.RESP_Panel_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.RESP_Panel_Label.ForeColor = System.Drawing.Color.White
        Me.RESP_Panel_Label.Location = New System.Drawing.Point(19, 293)
        Me.RESP_Panel_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.RESP_Panel_Label.Name = "RESP_Panel_Label"
        Me.RESP_Panel_Label.Size = New System.Drawing.Size(257, 41)
        Me.RESP_Panel_Label.TabIndex = 9999
        Me.RESP_Panel_Label.Text = "Felelősök:"
        '
        'Descr
        '
        Me.Descr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Descr.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Descr.Location = New System.Drawing.Point(284, 232)
        Me.Descr.Margin = New System.Windows.Forms.Padding(6)
        Me.Descr.Multiline = True
        Me.Descr.Name = "Descr"
        Me.Descr.Size = New System.Drawing.Size(603, 53)
        Me.Descr.TabIndex = 4
        '
        'Descr_Label
        '
        Me.Descr_Label.BackColor = System.Drawing.Color.DimGray
        Me.Descr_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Descr_Label.ForeColor = System.Drawing.Color.White
        Me.Descr_Label.Location = New System.Drawing.Point(15, 234)
        Me.Descr_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Descr_Label.Name = "Descr_Label"
        Me.Descr_Label.Size = New System.Drawing.Size(257, 41)
        Me.Descr_Label.TabIndex = 9999
        Me.Descr_Label.Text = "Feladat rövid leírása:"
        '
        'TASK_TYPES_ID_Label
        '
        Me.TASK_TYPES_ID_Label.BackColor = System.Drawing.Color.DimGray
        Me.TASK_TYPES_ID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.TASK_TYPES_ID_Label.ForeColor = System.Drawing.Color.White
        Me.TASK_TYPES_ID_Label.Location = New System.Drawing.Point(15, 181)
        Me.TASK_TYPES_ID_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.TASK_TYPES_ID_Label.Name = "TASK_TYPES_ID_Label"
        Me.TASK_TYPES_ID_Label.Size = New System.Drawing.Size(257, 41)
        Me.TASK_TYPES_ID_Label.TabIndex = 9999
        Me.TASK_TYPES_ID_Label.Text = "Feladat típusa:"
        '
        'Added_Date
        '
        Me.Added_Date.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Added_Date.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Added_Date.Location = New System.Drawing.Point(1181, 78)
        Me.Added_Date.Margin = New System.Windows.Forms.Padding(6)
        Me.Added_Date.Name = "Added_Date"
        Me.Added_Date.Size = New System.Drawing.Size(309, 33)
        Me.Added_Date.TabIndex = 201
        '
        'Added_Date_Label
        '
        Me.Added_Date_Label.BackColor = System.Drawing.Color.DimGray
        Me.Added_Date_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Added_Date_Label.ForeColor = System.Drawing.Color.White
        Me.Added_Date_Label.Location = New System.Drawing.Point(912, 80)
        Me.Added_Date_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Added_Date_Label.Name = "Added_Date_Label"
        Me.Added_Date_Label.Size = New System.Drawing.Size(257, 41)
        Me.Added_Date_Label.TabIndex = 9999
        Me.Added_Date_Label.Text = "Létrehozás dátuma:"
        '
        'DueDate
        '
        Me.DueDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DueDate.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.DueDate.Location = New System.Drawing.Point(1181, 135)
        Me.DueDate.Margin = New System.Windows.Forms.Padding(6)
        Me.DueDate.Name = "DueDate"
        Me.DueDate.Size = New System.Drawing.Size(309, 33)
        Me.DueDate.TabIndex = 202
        '
        'DueDate_Label
        '
        Me.DueDate_Label.BackColor = System.Drawing.Color.DimGray
        Me.DueDate_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.DueDate_Label.ForeColor = System.Drawing.Color.White
        Me.DueDate_Label.Location = New System.Drawing.Point(912, 137)
        Me.DueDate_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.DueDate_Label.Name = "DueDate_Label"
        Me.DueDate_Label.Size = New System.Drawing.Size(257, 41)
        Me.DueDate_Label.TabIndex = 9999
        Me.DueDate_Label.Text = "Lejárat dátuma:"
        '
        'StatusID_Label
        '
        Me.StatusID_Label.BackColor = System.Drawing.Color.DimGray
        Me.StatusID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.StatusID_Label.ForeColor = System.Drawing.Color.White
        Me.StatusID_Label.Location = New System.Drawing.Point(912, 192)
        Me.StatusID_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.StatusID_Label.Name = "StatusID_Label"
        Me.StatusID_Label.Size = New System.Drawing.Size(257, 41)
        Me.StatusID_Label.TabIndex = 9999
        Me.StatusID_Label.Text = "Státusz:"
        '
        'CHAT_List
        '
        Me.CHAT_List.Location = New System.Drawing.Point(20, 479)
        Me.CHAT_List.Margin = New System.Windows.Forms.Padding(6)
        Me.CHAT_List.Name = "CHAT_List"
        Me.CHAT_List.Size = New System.Drawing.Size(857, 102)
        Me.CHAT_List.TabIndex = 300
        Me.CHAT_List.TabStop = False
        Me.CHAT_List.UseCompatibleStateImageBehavior = False
        '
        'CHAT_Text_Label
        '
        Me.CHAT_Text_Label.BackColor = System.Drawing.Color.DimGray
        Me.CHAT_Text_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.CHAT_Text_Label.ForeColor = System.Drawing.Color.White
        Me.CHAT_Text_Label.Location = New System.Drawing.Point(15, 598)
        Me.CHAT_Text_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.CHAT_Text_Label.Name = "CHAT_Text_Label"
        Me.CHAT_Text_Label.Size = New System.Drawing.Size(257, 41)
        Me.CHAT_Text_Label.TabIndex = 9999
        Me.CHAT_Text_Label.Text = "Elküldendő szöveg:"
        '
        'CHAT_Text
        '
        Me.CHAT_Text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CHAT_Text.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.CHAT_Text.Location = New System.Drawing.Point(284, 593)
        Me.CHAT_Text.Margin = New System.Windows.Forms.Padding(6)
        Me.CHAT_Text.Multiline = True
        Me.CHAT_Text.Name = "CHAT_Text"
        Me.CHAT_Text.Size = New System.Drawing.Size(576, 46)
        Me.CHAT_Text.TabIndex = 401
        '
        'Connected_Records_Panel_Label
        '
        Me.Connected_Records_Panel_Label.BackColor = System.Drawing.Color.DimGray
        Me.Connected_Records_Panel_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Connected_Records_Panel_Label.ForeColor = System.Drawing.Color.White
        Me.Connected_Records_Panel_Label.Location = New System.Drawing.Point(912, 309)
        Me.Connected_Records_Panel_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Connected_Records_Panel_Label.Name = "Connected_Records_Panel_Label"
        Me.Connected_Records_Panel_Label.Size = New System.Drawing.Size(257, 41)
        Me.Connected_Records_Panel_Label.TabIndex = 9999
        Me.Connected_Records_Panel_Label.Text = "Kapcsolódó adatok:"
        '
        'ATTACHED_DOCS_Panel
        '
        Me.ATTACHED_DOCS_Panel.Location = New System.Drawing.Point(24, 812)
        Me.ATTACHED_DOCS_Panel.Name = "ATTACHED_DOCS_Panel"
        Me.ATTACHED_DOCS_Panel.Size = New System.Drawing.Size(1485, 342)
        Me.ATTACHED_DOCS_Panel.TabIndex = 400
        '
        'BTN_REFRESH
        '
        Me.BTN_REFRESH.Location = New System.Drawing.Point(58, 1223)
        Me.BTN_REFRESH.Name = "BTN_REFRESH"
        Me.BTN_REFRESH.Size = New System.Drawing.Size(55, 58)
        Me.BTN_REFRESH.TabIndex = 10020
        Me.BTN_REFRESH.TabStop = False
        '
        'BTN_OK
        '
        Me.BTN_OK.Location = New System.Drawing.Point(1370, 1232)
        Me.BTN_OK.Name = "BTN_OK"
        Me.BTN_OK.Size = New System.Drawing.Size(55, 58)
        Me.BTN_OK.TabIndex = 10021
        Me.BTN_OK.TabStop = False
        '
        'BTN_HLP
        '
        Me.BTN_HLP.Location = New System.Drawing.Point(1454, 1232)
        Me.BTN_HLP.Name = "BTN_HLP"
        Me.BTN_HLP.Size = New System.Drawing.Size(55, 58)
        Me.BTN_HLP.TabIndex = 10022
        Me.BTN_HLP.TabStop = False
        '
        'Added_Users_Name
        '
        Me.Added_Users_Name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Added_Users_Name.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Added_Users_Name.Location = New System.Drawing.Point(283, 132)
        Me.Added_Users_Name.Margin = New System.Windows.Forms.Padding(6)
        Me.Added_Users_Name.Name = "Added_Users_Name"
        Me.Added_Users_Name.Size = New System.Drawing.Size(603, 33)
        Me.Added_Users_Name.TabIndex = 2
        '
        'TASK_TYPES_ID
        '
        Me.TASK_TYPES_ID.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TASK_TYPES_ID.FormattingEnabled = True
        Me.TASK_TYPES_ID.Location = New System.Drawing.Point(290, 180)
        Me.TASK_TYPES_ID.Name = "TASK_TYPES_ID"
        Me.TASK_TYPES_ID.Size = New System.Drawing.Size(586, 33)
        Me.TASK_TYPES_ID.TabIndex = 3
        '
        'StatusID
        '
        Me.StatusID.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.StatusID.FormattingEnabled = True
        Me.StatusID.Location = New System.Drawing.Point(1178, 189)
        Me.StatusID.Name = "StatusID"
        Me.StatusID.Size = New System.Drawing.Size(219, 33)
        Me.StatusID.TabIndex = 203
        '
        'TM_RESP_GRID
        '
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TM_RESP_GRID.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TM_RESP_GRID.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.TM_RESP_GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TM_RESP_GRID.Location = New System.Drawing.Point(39, 15)
        Me.TM_RESP_GRID.Name = "TM_RESP_GRID"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TM_RESP_GRID.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.TM_RESP_GRID.Size = New System.Drawing.Size(504, 89)
        Me.TM_RESP_GRID.TabIndex = 100
        '
        'TM_CON_REC_GRID
        '
        Me.TM_CON_REC_GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TM_CON_REC_GRID.Location = New System.Drawing.Point(19, 14)
        Me.TM_CON_REC_GRID.Name = "TM_CON_REC_GRID"
        Me.TM_CON_REC_GRID.Size = New System.Drawing.Size(261, 75)
        Me.TM_CON_REC_GRID.TabIndex = 200
        '
        'BTN_PUBLISH
        '
        Me.BTN_PUBLISH.AutoSize = True
        Me.BTN_PUBLISH.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.142858!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.BTN_PUBLISH.ForeColor = System.Drawing.Color.Red
        Me.BTN_PUBLISH.Location = New System.Drawing.Point(917, 21)
        Me.BTN_PUBLISH.Name = "BTN_PUBLISH"
        Me.BTN_PUBLISH.Size = New System.Drawing.Size(466, 41)
        Me.BTN_PUBLISH.TabIndex = 9999
        Me.BTN_PUBLISH.TabStop = False
        Me.BTN_PUBLISH.Text = "Feladat kiadása (katt. ide)"
        Me.BTN_PUBLISH.UseVisualStyleBackColor = False
        '
        'RESP_Panel
        '
        Me.RESP_Panel.Controls.Add(Me.TM_RESP_GRID)
        Me.RESP_Panel.Controls.Add(Me.TM_RESP_SavePoint)
        Me.RESP_Panel.Location = New System.Drawing.Point(284, 294)
        Me.RESP_Panel.Name = "RESP_Panel"
        Me.RESP_Panel.Size = New System.Drawing.Size(602, 117)
        Me.RESP_Panel.TabIndex = 10023
        '
        'TM_RESP_SavePoint
        '
        Me.TM_RESP_SavePoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TM_RESP_SavePoint.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.TM_RESP_SavePoint.Location = New System.Drawing.Point(525, 59)
        Me.TM_RESP_SavePoint.Margin = New System.Windows.Forms.Padding(6)
        Me.TM_RESP_SavePoint.Name = "TM_RESP_SavePoint"
        Me.TM_RESP_SavePoint.Size = New System.Drawing.Size(33, 33)
        Me.TM_RESP_SavePoint.TabIndex = 199
        '
        'Connected_Records_Panel
        '
        Me.Connected_Records_Panel.Controls.Add(Me.TM_CON_REC_GRID)
        Me.Connected_Records_Panel.Controls.Add(Me.TM_CON_REC_SaveRecord)
        Me.Connected_Records_Panel.Location = New System.Drawing.Point(1191, 309)
        Me.Connected_Records_Panel.Name = "Connected_Records_Panel"
        Me.Connected_Records_Panel.Size = New System.Drawing.Size(318, 102)
        Me.Connected_Records_Panel.TabIndex = 10024
        '
        'TM_CON_REC_SaveRecord
        '
        Me.TM_CON_REC_SaveRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TM_CON_REC_SaveRecord.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.TM_CON_REC_SaveRecord.Location = New System.Drawing.Point(263, 30)
        Me.TM_CON_REC_SaveRecord.Margin = New System.Windows.Forms.Padding(6)
        Me.TM_CON_REC_SaveRecord.Name = "TM_CON_REC_SaveRecord"
        Me.TM_CON_REC_SaveRecord.Size = New System.Drawing.Size(33, 33)
        Me.TM_CON_REC_SaveRecord.TabIndex = 299
        '
        'Schedule_INFO_Label
        '
        Me.Schedule_INFO_Label.BackColor = System.Drawing.Color.DimGray
        Me.Schedule_INFO_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Schedule_INFO_Label.ForeColor = System.Drawing.Color.White
        Me.Schedule_INFO_Label.Location = New System.Drawing.Point(912, 244)
        Me.Schedule_INFO_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Schedule_INFO_Label.Name = "Schedule_INFO_Label"
        Me.Schedule_INFO_Label.Size = New System.Drawing.Size(257, 41)
        Me.Schedule_INFO_Label.TabIndex = 9999
        Me.Schedule_INFO_Label.Text = "Emlékeztető:"
        '
        'Schedule_INFO
        '
        Me.Schedule_INFO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Schedule_INFO.Location = New System.Drawing.Point(1190, 245)
        Me.Schedule_INFO.Name = "Schedule_INFO"
        Me.Schedule_INFO.Size = New System.Drawing.Size(235, 39)
        Me.Schedule_INFO.TabIndex = 5
        Me.Schedule_INFO.Text = ""
        '
        'BTN_Schedule
        '
        Me.BTN_Schedule.Location = New System.Drawing.Point(1454, 248)
        Me.BTN_Schedule.Name = "BTN_Schedule"
        Me.BTN_Schedule.Size = New System.Drawing.Size(22, 22)
        Me.BTN_Schedule.TabIndex = 10025
        Me.BTN_Schedule.TabStop = False
        '
        'Answer_YN_Label
        '
        Me.Answer_YN_Label.BackColor = System.Drawing.Color.DimGray
        Me.Answer_YN_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Answer_YN_Label.ForeColor = System.Drawing.Color.White
        Me.Answer_YN_Label.Location = New System.Drawing.Point(7, 15)
        Me.Answer_YN_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Answer_YN_Label.Name = "Answer_YN_Label"
        Me.Answer_YN_Label.Size = New System.Drawing.Size(257, 41)
        Me.Answer_YN_Label.TabIndex = 10026
        Me.Answer_YN_Label.Text = "Engedélyezés:"
        '
        'Answer_Panel
        '
        Me.Answer_Panel.Controls.Add(Me.Answer_NO_Label)
        Me.Answer_Panel.Controls.Add(Me.Answer_YES_Label)
        Me.Answer_Panel.Controls.Add(Me.Answer_NO)
        Me.Answer_Panel.Controls.Add(Me.Answer_YES)
        Me.Answer_Panel.Controls.Add(Me.Answer_YN_Label)
        Me.Answer_Panel.Location = New System.Drawing.Point(905, 597)
        Me.Answer_Panel.Name = "Answer_Panel"
        Me.Answer_Panel.Size = New System.Drawing.Size(603, 68)
        Me.Answer_Panel.TabIndex = 200
        '
        'Answer_NO_Label
        '
        Me.Answer_NO_Label.BackColor = System.Drawing.SystemColors.Control
        Me.Answer_NO_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Answer_NO_Label.ForeColor = System.Drawing.Color.Black
        Me.Answer_NO_Label.Location = New System.Drawing.Point(480, 15)
        Me.Answer_NO_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Answer_NO_Label.Name = "Answer_NO_Label"
        Me.Answer_NO_Label.Size = New System.Drawing.Size(91, 41)
        Me.Answer_NO_Label.TabIndex = 9999
        Me.Answer_NO_Label.Text = "NEM"
        Me.Answer_NO_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Answer_YES_Label
        '
        Me.Answer_YES_Label.BackColor = System.Drawing.SystemColors.Control
        Me.Answer_YES_Label.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.Answer_YES_Label.ForeColor = System.Drawing.Color.Black
        Me.Answer_YES_Label.Location = New System.Drawing.Point(318, 17)
        Me.Answer_YES_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Answer_YES_Label.Name = "Answer_YES_Label"
        Me.Answer_YES_Label.Size = New System.Drawing.Size(91, 41)
        Me.Answer_YES_Label.TabIndex = 9999
        Me.Answer_YES_Label.Text = "IGEN"
        Me.Answer_YES_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Answer_NO
        '
        Me.Answer_NO.AutoCheck = False
        Me.Answer_NO.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Answer_NO.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Answer_NO.Location = New System.Drawing.Point(443, 25)
        Me.Answer_NO.Name = "Answer_NO"
        Me.Answer_NO.Size = New System.Drawing.Size(21, 22)
        Me.Answer_NO.TabIndex = 202
        Me.Answer_NO.UseVisualStyleBackColor = True
        '
        'Answer_YES
        '
        Me.Answer_YES.AutoCheck = False
        Me.Answer_YES.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Answer_YES.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Answer_YES.Location = New System.Drawing.Point(286, 15)
        Me.Answer_YES.Name = "Answer_YES"
        Me.Answer_YES.Size = New System.Drawing.Size(47, 43)
        Me.Answer_YES.TabIndex = 201
        Me.Answer_YES.UseVisualStyleBackColor = True
        '
        'BTN_CANCEL
        '
        Me.BTN_CANCEL.AutoSize = True
        Me.BTN_CANCEL.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.142858!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.BTN_CANCEL.ForeColor = System.Drawing.Color.Red
        Me.BTN_CANCEL.Location = New System.Drawing.Point(1408, 21)
        Me.BTN_CANCEL.Name = "BTN_CANCEL"
        Me.BTN_CANCEL.Size = New System.Drawing.Size(41, 41)
        Me.BTN_CANCEL.TabIndex = 9999
        Me.BTN_CANCEL.TabStop = False
        Me.BTN_CANCEL.UseVisualStyleBackColor = False
        '
        'SEL_TASKMAN_TASK
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1532, 1302)
        Me.Controls.Add(Me.BTN_CANCEL)
        Me.Controls.Add(Me.Answer_Panel)
        Me.Controls.Add(Me.BTN_Schedule)
        Me.Controls.Add(Me.Schedule_INFO)
        Me.Controls.Add(Me.Schedule_INFO_Label)
        Me.Controls.Add(Me.Connected_Records_Panel_Label)
        Me.Controls.Add(Me.RESP_Panel_Label)
        Me.Controls.Add(Me.Connected_Records_Panel)
        Me.Controls.Add(Me.RESP_Panel)
        Me.Controls.Add(Me.BTN_PUBLISH)
        Me.Controls.Add(Me.StatusID)
        Me.Controls.Add(Me.TASK_TYPES_ID)
        Me.Controls.Add(Me.Added_Users_Name)
        Me.Controls.Add(Me.BTN_HLP)
        Me.Controls.Add(Me.BTN_OK)
        Me.Controls.Add(Me.BTN_REFRESH)
        Me.Controls.Add(Me.ATTACHED_DOCS_Panel)
        Me.Controls.Add(Me.CHAT_Text)
        Me.Controls.Add(Me.CHAT_Text_Label)
        Me.Controls.Add(Me.CHAT_List)
        Me.Controls.Add(Me.StatusID_Label)
        Me.Controls.Add(Me.DueDate)
        Me.Controls.Add(Me.DueDate_Label)
        Me.Controls.Add(Me.Added_Date)
        Me.Controls.Add(Me.Added_Date_Label)
        Me.Controls.Add(Me.TASK_TYPES_ID_Label)
        Me.Controls.Add(Me.Descr)
        Me.Controls.Add(Me.Descr_Label)
        Me.Controls.Add(Me.Added_Users_Name_Label)
        Me.Controls.Add(Me.TaskNum)
        Me.Controls.Add(Me.TaskNum_Label)
        Me.Controls.Add(Me.CHAT_List_Label)
        Me.Controls.Add(Me.Title_Label)
        Me.Name = "SEL_TASKMAN_TASK"
        Me.Text = "SEL_TASKMAN_TASK"
        CType(Me.BTN_REFRESH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_OK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_HLP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TM_RESP_GRID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TM_CON_REC_GRID, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RESP_Panel.ResumeLayout(False)
        Me.RESP_Panel.PerformLayout()
        Me.Connected_Records_Panel.ResumeLayout(False)
        Me.Connected_Records_Panel.PerformLayout()
        CType(Me.BTN_Schedule, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Answer_Panel.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Title_Label As System.Windows.Forms.Label
    Friend WithEvents CHAT_List_Label As System.Windows.Forms.Label
    Friend WithEvents TaskNum_Label As System.Windows.Forms.Label
    Friend WithEvents TaskNum As System.Windows.Forms.TextBox
    Friend WithEvents Added_Users_Name_Label As System.Windows.Forms.Label
    Friend WithEvents RESP_Panel_Label As System.Windows.Forms.Label
    Friend WithEvents Descr As System.Windows.Forms.TextBox
    Friend WithEvents Descr_Label As System.Windows.Forms.Label
    Friend WithEvents TASK_TYPES_ID_Label As System.Windows.Forms.Label
    Friend WithEvents Added_Date As System.Windows.Forms.TextBox
    Friend WithEvents Added_Date_Label As System.Windows.Forms.Label
    Friend WithEvents DueDate As System.Windows.Forms.TextBox
    Friend WithEvents DueDate_Label As System.Windows.Forms.Label
    Friend WithEvents StatusID_Label As System.Windows.Forms.Label
    Friend WithEvents CHAT_List As System.Windows.Forms.ListView
    Friend WithEvents CHAT_Text_Label As System.Windows.Forms.Label
    Friend WithEvents CHAT_Text As System.Windows.Forms.TextBox
    Friend WithEvents Connected_Records_Panel_Label As System.Windows.Forms.Label
    Friend WithEvents ATTACHED_DOCS_Panel As System.Windows.Forms.Panel
    Friend WithEvents BTN_REFRESH As System.Windows.Forms.PictureBox
    Friend WithEvents BTN_OK As System.Windows.Forms.PictureBox
    Friend WithEvents BTN_HLP As System.Windows.Forms.PictureBox
    Friend WithEvents Added_Users_Name As System.Windows.Forms.TextBox
    Friend WithEvents TASK_TYPES_ID As System.Windows.Forms.ComboBox
    Friend WithEvents StatusID As System.Windows.Forms.ComboBox
    Friend WithEvents TM_RESP_GRID As System.Windows.Forms.DataGridView
    Friend WithEvents TM_CON_REC_GRID As System.Windows.Forms.DataGridView
    Friend WithEvents BTN_PUBLISH As System.Windows.Forms.Button
    Friend WithEvents RESP_Panel As System.Windows.Forms.Panel
    Friend WithEvents Connected_Records_Panel As System.Windows.Forms.Panel
    Friend WithEvents TM_RESP_SavePoint As System.Windows.Forms.TextBox
    Friend WithEvents TM_CON_REC_SaveRecord As System.Windows.Forms.TextBox
    Friend WithEvents Schedule_INFO_Label As System.Windows.Forms.Label
    Friend WithEvents Schedule_INFO As System.Windows.Forms.RichTextBox
    Friend WithEvents BTN_Schedule As System.Windows.Forms.PictureBox
    Friend WithEvents Answer_YN_Label As System.Windows.Forms.Label
    Friend WithEvents Answer_Panel As System.Windows.Forms.Panel
    Friend WithEvents Answer_NO_Label As System.Windows.Forms.Label
    Friend WithEvents Answer_YES_Label As System.Windows.Forms.Label
    Friend WithEvents Answer_NO As System.Windows.Forms.RadioButton
    Friend WithEvents Answer_YES As System.Windows.Forms.RadioButton
    Friend WithEvents BTN_CANCEL As System.Windows.Forms.Button
End Class
