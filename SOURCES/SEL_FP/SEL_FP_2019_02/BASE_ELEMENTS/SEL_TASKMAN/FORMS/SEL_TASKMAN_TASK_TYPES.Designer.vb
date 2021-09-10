<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SEL_TASKMAN_TASK_TYPES
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
        Me.TASK_TYPES_GRID_Panel = New System.Windows.Forms.Panel()
        Me.TASK_TYPES_GRID_FOOTER_Label = New System.Windows.Forms.Label()
        Me.TASK_TYPES_GRID_Label = New System.Windows.Forms.Label()
        Me.TASK_TYPES_GRID = New System.Windows.Forms.DataGridView()
        Me.BTN_ExcelExport = New System.Windows.Forms.PictureBox()
        Me.BTN_Help = New System.Windows.Forms.PictureBox()
        Me.SP_MAIN = New System.Windows.Forms.SplitContainer()
        Me.TASK_TYPES_GRID_Panel.SuspendLayout()
        CType(Me.TASK_TYPES_GRID, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_ExcelExport, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_Help, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SP_MAIN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SP_MAIN.Panel1.SuspendLayout()
        Me.SP_MAIN.Panel2.SuspendLayout()
        Me.SP_MAIN.SuspendLayout()
        Me.SuspendLayout()
        '
        'TASK_TYPES_GRID_Panel
        '
        Me.TASK_TYPES_GRID_Panel.Controls.Add(Me.TASK_TYPES_GRID_FOOTER_Label)
        Me.TASK_TYPES_GRID_Panel.Location = New System.Drawing.Point(51, 46)
        Me.TASK_TYPES_GRID_Panel.Margin = New System.Windows.Forms.Padding(6)
        Me.TASK_TYPES_GRID_Panel.Name = "TASK_TYPES_GRID_Panel"
        Me.TASK_TYPES_GRID_Panel.Size = New System.Drawing.Size(565, 247)
        Me.TASK_TYPES_GRID_Panel.TabIndex = 10308
        '
        'TASK_TYPES_GRID_FOOTER_Label
        '
        Me.TASK_TYPES_GRID_FOOTER_Label.BackColor = System.Drawing.Color.SteelBlue
        Me.TASK_TYPES_GRID_FOOTER_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TASK_TYPES_GRID_FOOTER_Label.ForeColor = System.Drawing.Color.White
        Me.TASK_TYPES_GRID_FOOTER_Label.Location = New System.Drawing.Point(6, 14)
        Me.TASK_TYPES_GRID_FOOTER_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.TASK_TYPES_GRID_FOOTER_Label.Name = "TASK_TYPES_GRID_FOOTER_Label"
        Me.TASK_TYPES_GRID_FOOTER_Label.Size = New System.Drawing.Size(503, 41)
        Me.TASK_TYPES_GRID_FOOTER_Label.TabIndex = 10310
        Me.TASK_TYPES_GRID_FOOTER_Label.Text = "KIVÁLASZTOTT FELADAT TÍPUS RÉSZLETES ADATAI"
        Me.TASK_TYPES_GRID_FOOTER_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TASK_TYPES_GRID_Label
        '
        Me.TASK_TYPES_GRID_Label.BackColor = System.Drawing.Color.SteelBlue
        Me.TASK_TYPES_GRID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TASK_TYPES_GRID_Label.ForeColor = System.Drawing.Color.White
        Me.TASK_TYPES_GRID_Label.Location = New System.Drawing.Point(26, 118)
        Me.TASK_TYPES_GRID_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.TASK_TYPES_GRID_Label.Name = "TASK_TYPES_GRID_Label"
        Me.TASK_TYPES_GRID_Label.Size = New System.Drawing.Size(659, 41)
        Me.TASK_TYPES_GRID_Label.TabIndex = 10309
        Me.TASK_TYPES_GRID_Label.Text = "FELADAT TÍPUSOK"
        Me.TASK_TYPES_GRID_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TASK_TYPES_GRID
        '
        Me.TASK_TYPES_GRID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TASK_TYPES_GRID.Location = New System.Drawing.Point(26, 164)
        Me.TASK_TYPES_GRID.Margin = New System.Windows.Forms.Padding(6)
        Me.TASK_TYPES_GRID.Name = "TASK_TYPES_GRID"
        Me.TASK_TYPES_GRID.Size = New System.Drawing.Size(660, 249)
        Me.TASK_TYPES_GRID.TabIndex = 10307
        '
        'BTN_ExcelExport
        '
        Me.BTN_ExcelExport.Location = New System.Drawing.Point(535, 15)
        Me.BTN_ExcelExport.Margin = New System.Windows.Forms.Padding(6)
        Me.BTN_ExcelExport.Name = "BTN_ExcelExport"
        Me.BTN_ExcelExport.Size = New System.Drawing.Size(81, 81)
        Me.BTN_ExcelExport.TabIndex = 10311
        Me.BTN_ExcelExport.TabStop = False
        '
        'BTN_Help
        '
        Me.BTN_Help.Location = New System.Drawing.Point(616, 15)
        Me.BTN_Help.Margin = New System.Windows.Forms.Padding(6)
        Me.BTN_Help.Name = "BTN_Help"
        Me.BTN_Help.Size = New System.Drawing.Size(81, 81)
        Me.BTN_Help.TabIndex = 10310
        Me.BTN_Help.TabStop = False
        '
        'SP_MAIN
        '
        Me.SP_MAIN.Location = New System.Drawing.Point(25, 10)
        Me.SP_MAIN.Name = "SP_MAIN"
        Me.SP_MAIN.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SP_MAIN.Panel1
        '
        Me.SP_MAIN.Panel1.Controls.Add(Me.BTN_ExcelExport)
        Me.SP_MAIN.Panel1.Controls.Add(Me.BTN_Help)
        Me.SP_MAIN.Panel1.Controls.Add(Me.TASK_TYPES_GRID_Label)
        Me.SP_MAIN.Panel1.Controls.Add(Me.TASK_TYPES_GRID)
        '
        'SP_MAIN.Panel2
        '
        Me.SP_MAIN.Panel2.Controls.Add(Me.TASK_TYPES_GRID_Panel)
        Me.SP_MAIN.Size = New System.Drawing.Size(721, 792)
        Me.SP_MAIN.SplitterDistance = 477
        Me.SP_MAIN.TabIndex = 10312
        '
        'SEL_TASKMAN_TASK_TYPES
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(758, 841)
        Me.Controls.Add(Me.SP_MAIN)
        Me.Name = "SEL_TASKMAN_TASK_TYPES"
        Me.Text = "SEL_TASKMAN_TASK_TYPES"
        Me.TASK_TYPES_GRID_Panel.ResumeLayout(False)
        CType(Me.TASK_TYPES_GRID, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_ExcelExport, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_Help, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SP_MAIN.Panel1.ResumeLayout(False)
        Me.SP_MAIN.Panel2.ResumeLayout(False)
        CType(Me.SP_MAIN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SP_MAIN.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TASK_TYPES_GRID_Panel As System.Windows.Forms.Panel
    Friend WithEvents TASK_TYPES_GRID_Label As System.Windows.Forms.Label
    Friend WithEvents TASK_TYPES_GRID As System.Windows.Forms.DataGridView
    Friend WithEvents BTN_ExcelExport As System.Windows.Forms.PictureBox
    Friend WithEvents BTN_Help As System.Windows.Forms.PictureBox
    Friend WithEvents TASK_TYPES_GRID_FOOTER_Label As System.Windows.Forms.Label
    Friend WithEvents SP_MAIN As System.Windows.Forms.SplitContainer
End Class
