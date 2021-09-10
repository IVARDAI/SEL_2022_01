<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FP_Services_Select_Control_Setup
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.SQL_From1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.FixText_Identifier = New System.Windows.Forms.TextBox()
        Me.Label_Head = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.SQL_From2 = New System.Windows.Forms.TextBox()
        Me.IdFieldName = New System.Windows.Forms.TextBox()
        Me.Field_Text1 = New System.Windows.Forms.TextBox()
        Me.Field_Text2 = New System.Windows.Forms.TextBox()
        Me.OrderBy1 = New System.Windows.Forms.TextBox()
        Me.OrderBy2 = New System.Windows.Forms.TextBox()
        Me.MaxLength = New System.Windows.Forms.TextBox()
        Me.PanelHeight = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.DGV_Field_Def = New System.Windows.Forms.DataGridView()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Button_OK = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.No_Limit_To_List = New System.Windows.Forms.CheckBox()
        Me.DGV_Lang = New System.Windows.Forms.DataGridView()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.DGV_Field_Def, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.DGV_Lang, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SQL_From1
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.SQL_From1, 2)
        Me.SQL_From1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SQL_From1.Location = New System.Drawing.Point(160, 22)
        Me.SQL_From1.Margin = New System.Windows.Forms.Padding(0)
        Me.SQL_From1.Name = "SQL_From1"
        Me.SQL_From1.Size = New System.Drawing.Size(640, 20)
        Me.SQL_From1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(0, 22)
        Me.Label1.Margin = New System.Windows.Forms.Padding(0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(160, 22)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "SQL from 1:"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.FixText_Identifier, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label_Head, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 0, 10)
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Label6, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.Label7, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.Label8, 0, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.Label9, 0, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.Label10, 0, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.Label11, 0, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.SQL_From1, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.SQL_From2, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.IdFieldName, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Field_Text1, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.Field_Text2, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.OrderBy1, 1, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.OrderBy2, 1, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.MaxLength, 1, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.PanelHeight, 1, 10)
        Me.TableLayoutPanel1.Controls.Add(Me.Label12, 0, 11)
        Me.TableLayoutPanel1.Controls.Add(Me.DGV_Field_Def, 0, 12)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 1, 13)
        Me.TableLayoutPanel1.Controls.Add(Me.No_Limit_To_List, 1, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.DGV_Lang, 2, 12)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 14
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(800, 450)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'FixText_Identifier
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.FixText_Identifier, 2)
        Me.FixText_Identifier.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FixText_Identifier.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FixText_Identifier.Location = New System.Drawing.Point(160, 0)
        Me.FixText_Identifier.Margin = New System.Windows.Forms.Padding(0)
        Me.FixText_Identifier.Name = "FixText_Identifier"
        Me.FixText_Identifier.Size = New System.Drawing.Size(640, 21)
        Me.FixText_Identifier.TabIndex = 25
        '
        'Label_Head
        '
        Me.Label_Head.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_Head.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label_Head.Location = New System.Drawing.Point(0, 0)
        Me.Label_Head.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_Head.Name = "Label_Head"
        Me.Label_Head.Size = New System.Drawing.Size(160, 22)
        Me.Label_Head.TabIndex = 2
        Me.Label_Head.Text = "Fixtext key:"
        '
        'Label3
        '
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label3.Location = New System.Drawing.Point(0, 44)
        Me.Label3.Margin = New System.Windows.Forms.Padding(0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(160, 22)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "SQL from 2:"
        '
        'Label5
        '
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label5.Location = New System.Drawing.Point(0, 220)
        Me.Label5.Margin = New System.Windows.Forms.Padding(0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(160, 22)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Select panel height:"
        '
        'Label4
        '
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label4.Location = New System.Drawing.Point(0, 66)
        Me.Label4.Margin = New System.Windows.Forms.Padding(0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(160, 22)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "ID field name:"
        '
        'Label6
        '
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label6.Location = New System.Drawing.Point(0, 88)
        Me.Label6.Margin = New System.Windows.Forms.Padding(0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(160, 22)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Text field 1 name"
        '
        'Label7
        '
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label7.Location = New System.Drawing.Point(0, 110)
        Me.Label7.Margin = New System.Windows.Forms.Padding(0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(160, 22)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Text field 2 name:"
        '
        'Label8
        '
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label8.Location = New System.Drawing.Point(0, 132)
        Me.Label8.Margin = New System.Windows.Forms.Padding(0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(160, 22)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Order by field name 1:"
        '
        'Label9
        '
        Me.Label9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label9.Location = New System.Drawing.Point(0, 154)
        Me.Label9.Margin = New System.Windows.Forms.Padding(0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(160, 22)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "Order by field name 2:"
        '
        'Label10
        '
        Me.Label10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label10.Location = New System.Drawing.Point(0, 176)
        Me.Label10.Margin = New System.Windows.Forms.Padding(0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(160, 22)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "No limit to list:"
        '
        'Label11
        '
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label11.Location = New System.Drawing.Point(0, 198)
        Me.Label11.Margin = New System.Windows.Forms.Padding(0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(160, 22)
        Me.Label11.TabIndex = 11
        Me.Label11.Text = "Max. field display length:"
        '
        'SQL_From2
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.SQL_From2, 2)
        Me.SQL_From2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SQL_From2.Location = New System.Drawing.Point(160, 44)
        Me.SQL_From2.Margin = New System.Windows.Forms.Padding(0)
        Me.SQL_From2.Name = "SQL_From2"
        Me.SQL_From2.Size = New System.Drawing.Size(640, 20)
        Me.SQL_From2.TabIndex = 1
        '
        'IdFieldName
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.IdFieldName, 2)
        Me.IdFieldName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.IdFieldName.Location = New System.Drawing.Point(160, 66)
        Me.IdFieldName.Margin = New System.Windows.Forms.Padding(0)
        Me.IdFieldName.Name = "IdFieldName"
        Me.IdFieldName.Size = New System.Drawing.Size(640, 20)
        Me.IdFieldName.TabIndex = 2
        '
        'Field_Text1
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Field_Text1, 2)
        Me.Field_Text1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Field_Text1.Location = New System.Drawing.Point(160, 88)
        Me.Field_Text1.Margin = New System.Windows.Forms.Padding(0)
        Me.Field_Text1.Name = "Field_Text1"
        Me.Field_Text1.Size = New System.Drawing.Size(640, 20)
        Me.Field_Text1.TabIndex = 3
        '
        'Field_Text2
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Field_Text2, 2)
        Me.Field_Text2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Field_Text2.Location = New System.Drawing.Point(160, 110)
        Me.Field_Text2.Margin = New System.Windows.Forms.Padding(0)
        Me.Field_Text2.Name = "Field_Text2"
        Me.Field_Text2.Size = New System.Drawing.Size(640, 20)
        Me.Field_Text2.TabIndex = 4
        '
        'OrderBy1
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.OrderBy1, 2)
        Me.OrderBy1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OrderBy1.Location = New System.Drawing.Point(160, 132)
        Me.OrderBy1.Margin = New System.Windows.Forms.Padding(0)
        Me.OrderBy1.Name = "OrderBy1"
        Me.OrderBy1.Size = New System.Drawing.Size(640, 20)
        Me.OrderBy1.TabIndex = 5
        '
        'OrderBy2
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.OrderBy2, 2)
        Me.OrderBy2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OrderBy2.Location = New System.Drawing.Point(160, 154)
        Me.OrderBy2.Margin = New System.Windows.Forms.Padding(0)
        Me.OrderBy2.Name = "OrderBy2"
        Me.OrderBy2.Size = New System.Drawing.Size(640, 20)
        Me.OrderBy2.TabIndex = 6
        '
        'MaxLength
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.MaxLength, 2)
        Me.MaxLength.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MaxLength.Location = New System.Drawing.Point(160, 198)
        Me.MaxLength.Margin = New System.Windows.Forms.Padding(0)
        Me.MaxLength.Name = "MaxLength"
        Me.MaxLength.Size = New System.Drawing.Size(640, 20)
        Me.MaxLength.TabIndex = 8
        '
        'PanelHeight
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.PanelHeight, 2)
        Me.PanelHeight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelHeight.Location = New System.Drawing.Point(160, 220)
        Me.PanelHeight.Margin = New System.Windows.Forms.Padding(0)
        Me.PanelHeight.Name = "PanelHeight"
        Me.PanelHeight.Size = New System.Drawing.Size(640, 20)
        Me.PanelHeight.TabIndex = 9
        '
        'Label12
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Label12, 3)
        Me.Label12.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label12.Location = New System.Drawing.Point(0, 242)
        Me.Label12.Margin = New System.Windows.Forms.Padding(0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(800, 22)
        Me.Label12.TabIndex = 22
        Me.Label12.Text = "Data field definitions"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DGV_Field_Def
        '
        Me.DGV_Field_Def.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TableLayoutPanel1.SetColumnSpan(Me.DGV_Field_Def, 2)
        Me.DGV_Field_Def.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DGV_Field_Def.Location = New System.Drawing.Point(3, 267)
        Me.DGV_Field_Def.Name = "DGV_Field_Def"
        Me.DGV_Field_Def.Size = New System.Drawing.Size(474, 136)
        Me.DGV_Field_Def.TabIndex = 23
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 4
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.Button_OK, 2, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Button_Cancel, 3, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(163, 409)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(314, 38)
        Me.TableLayoutPanel2.TabIndex = 24
        '
        'Button_OK
        '
        Me.Button_OK.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button_OK.Location = New System.Drawing.Point(159, 3)
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.Size = New System.Drawing.Size(72, 32)
        Me.Button_OK.TabIndex = 0
        Me.Button_OK.Text = "OK"
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Cancel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button_Cancel.Location = New System.Drawing.Point(237, 3)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(74, 32)
        Me.Button_Cancel.TabIndex = 1
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'No_Limit_To_List
        '
        Me.No_Limit_To_List.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.No_Limit_To_List, 2)
        Me.No_Limit_To_List.Dock = System.Windows.Forms.DockStyle.Fill
        Me.No_Limit_To_List.Location = New System.Drawing.Point(163, 179)
        Me.No_Limit_To_List.Name = "No_Limit_To_List"
        Me.No_Limit_To_List.Size = New System.Drawing.Size(634, 16)
        Me.No_Limit_To_List.TabIndex = 7
        Me.No_Limit_To_List.UseVisualStyleBackColor = True
        '
        'DGV_Lang
        '
        Me.DGV_Lang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_Lang.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DGV_Lang.Location = New System.Drawing.Point(483, 267)
        Me.DGV_Lang.Name = "DGV_Lang"
        Me.DGV_Lang.Size = New System.Drawing.Size(314, 136)
        Me.DGV_Lang.TabIndex = 26
        '
        'FP_Services_Select_Control_Setup
        '
        Me.AcceptButton = Me.Button_OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button_Cancel
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "FP_Services_Select_Control_Setup"
        Me.Text = "FP_Services_Select_Control_Setup"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.DGV_Field_Def, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel2.ResumeLayout(False)
        CType(Me.DGV_Lang, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SQL_From1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Label_Head As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents PanelHeight As TextBox
    Friend WithEvents MaxLength As TextBox
    Friend WithEvents OrderBy1 As TextBox
    Friend WithEvents OrderBy2 As TextBox
    Friend WithEvents Field_Text2 As TextBox
    Friend WithEvents IdFieldName As TextBox
    Friend WithEvents SQL_From2 As TextBox
    Friend WithEvents Field_Text1 As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents DGV_Field_Def As DataGridView
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents Button_OK As Button
    Friend WithEvents Button_Cancel As Button
    Friend WithEvents No_Limit_To_List As CheckBox
    Friend WithEvents FixText_Identifier As TextBox
    Friend WithEvents DGV_Lang As DataGridView
End Class
