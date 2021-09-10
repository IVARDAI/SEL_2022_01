<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SEL_TASKMAN_REMINDER_HISTORY
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.SCHEDULE_GRID = New System.Windows.Forms.DataGridView()
        Me.SCHEDULE_GRID_Label = New System.Windows.Forms.Label()
        Me.SCHEDULED_USERS_GRID = New System.Windows.Forms.DataGridView()
        Me.SCHEDULED_USERS_GRID_Label = New System.Windows.Forms.Label()
        Me.SC_MAIN = New System.Windows.Forms.SplitContainer()
        CType(Me.SCHEDULE_GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SCHEDULED_USERS_GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SC_MAIN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SC_MAIN.Panel1.SuspendLayout()
        Me.SC_MAIN.Panel2.SuspendLayout()
        Me.SC_MAIN.SuspendLayout()
        Me.SuspendLayout()
        '
        'SCHEDULE_GRID
        '
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.SCHEDULE_GRID.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.SCHEDULE_GRID.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.SCHEDULE_GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.SCHEDULE_GRID.Location = New System.Drawing.Point(25, 76)
        Me.SCHEDULE_GRID.Name = "SCHEDULE_GRID"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.SCHEDULE_GRID.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.SCHEDULE_GRID.Size = New System.Drawing.Size(944, 89)
        Me.SCHEDULE_GRID.TabIndex = 101
        '
        'SCHEDULE_GRID_Label
        '
        Me.SCHEDULE_GRID_Label.BackColor = System.Drawing.Color.MidnightBlue
        Me.SCHEDULE_GRID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.SCHEDULE_GRID_Label.ForeColor = System.Drawing.Color.White
        Me.SCHEDULE_GRID_Label.Location = New System.Drawing.Point(20, 18)
        Me.SCHEDULE_GRID_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.SCHEDULE_GRID_Label.Name = "SCHEDULE_GRID_Label"
        Me.SCHEDULE_GRID_Label.Size = New System.Drawing.Size(945, 41)
        Me.SCHEDULE_GRID_Label.TabIndex = 9999
        Me.SCHEDULE_GRID_Label.Text = "EMLÉKEZTETŐK RÉSZLETES ADATAI"
        Me.SCHEDULE_GRID_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SCHEDULED_USERS_GRID
        '
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.SCHEDULED_USERS_GRID.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle4
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.SCHEDULED_USERS_GRID.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.SCHEDULED_USERS_GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.SCHEDULED_USERS_GRID.Location = New System.Drawing.Point(54, 90)
        Me.SCHEDULED_USERS_GRID.Name = "SCHEDULED_USERS_GRID"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.SCHEDULED_USERS_GRID.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.SCHEDULED_USERS_GRID.Size = New System.Drawing.Size(915, 104)
        Me.SCHEDULED_USERS_GRID.TabIndex = 10003
        '
        'SCHEDULED_USERS_GRID_Label
        '
        Me.SCHEDULED_USERS_GRID_Label.BackColor = System.Drawing.Color.SteelBlue
        Me.SCHEDULED_USERS_GRID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.SCHEDULED_USERS_GRID_Label.ForeColor = System.Drawing.Color.White
        Me.SCHEDULED_USERS_GRID_Label.Location = New System.Drawing.Point(45, 32)
        Me.SCHEDULED_USERS_GRID_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.SCHEDULED_USERS_GRID_Label.Name = "SCHEDULED_USERS_GRID_Label"
        Me.SCHEDULED_USERS_GRID_Label.Size = New System.Drawing.Size(920, 41)
        Me.SCHEDULED_USERS_GRID_Label.TabIndex = 9999
        Me.SCHEDULED_USERS_GRID_Label.Text = "KIVÁLASZTOTT EMLÉKEZTETŐ OLVASÁSI ÁLLAPOTA FELHASZNÁLÓNKÉNT"
        Me.SCHEDULED_USERS_GRID_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SC_MAIN
        '
        Me.SC_MAIN.Location = New System.Drawing.Point(27, 25)
        Me.SC_MAIN.Name = "SC_MAIN"
        Me.SC_MAIN.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SC_MAIN.Panel1
        '
        Me.SC_MAIN.Panel1.Controls.Add(Me.SCHEDULE_GRID_Label)
        Me.SC_MAIN.Panel1.Controls.Add(Me.SCHEDULE_GRID)
        '
        'SC_MAIN.Panel2
        '
        Me.SC_MAIN.Panel2.Controls.Add(Me.SCHEDULED_USERS_GRID)
        Me.SC_MAIN.Panel2.Controls.Add(Me.SCHEDULED_USERS_GRID_Label)
        Me.SC_MAIN.Size = New System.Drawing.Size(1006, 558)
        Me.SC_MAIN.SplitterDistance = 272
        Me.SC_MAIN.TabIndex = 10002
        '
        'SEL_TASKMAN_REMINDER_HISTORY
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1045, 595)
        Me.Controls.Add(Me.SC_MAIN)
        Me.Name = "SEL_TASKMAN_REMINDER_HISTORY"
        Me.Text = "Form1"
        CType(Me.SCHEDULE_GRID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SCHEDULED_USERS_GRID, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SC_MAIN.Panel1.ResumeLayout(False)
        Me.SC_MAIN.Panel2.ResumeLayout(False)
        CType(Me.SC_MAIN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SC_MAIN.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SCHEDULE_GRID As System.Windows.Forms.DataGridView
    Friend WithEvents SCHEDULE_GRID_Label As System.Windows.Forms.Label
    Friend WithEvents SCHEDULED_USERS_GRID As System.Windows.Forms.DataGridView
    Friend WithEvents SCHEDULED_USERS_GRID_Label As System.Windows.Forms.Label
    Friend WithEvents SC_MAIN As System.Windows.Forms.SplitContainer
End Class
