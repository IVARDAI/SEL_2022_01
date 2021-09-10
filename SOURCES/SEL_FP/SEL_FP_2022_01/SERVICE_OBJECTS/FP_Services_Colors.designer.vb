<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_Services_Colors
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FP_Services_Colors))
        Me.Panel = New System.Windows.Forms.Panel()
        Me.RGB = New System.Windows.Forms.TextBox()
        Me.GroupBox = New System.Windows.Forms.GroupBox()
        Me.ButtonColorPicker = New System.Windows.Forms.RadioButton()
        Me.ButtonFP = New System.Windows.Forms.RadioButton()
        Me.ButtonSystem = New System.Windows.Forms.RadioButton()
        Me.ButtonWeb = New System.Windows.Forms.RadioButton()
        Me.RGB_Label = New System.Windows.Forms.Label()
        Me.HEX_Label = New System.Windows.Forms.Label()
        Me.HEX = New System.Windows.Forms.TextBox()
        Me.T_Color = New System.Windows.Forms.Timer(Me.components)
        Me.ColorName_Label = New System.Windows.Forms.Label()
        Me.ColorName = New System.Windows.Forms.TextBox()
        Me.Button_SELECT_COLOR = New System.Windows.Forms.Button()
        Me.GroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel
        '
        Me.Panel.AutoScroll = True
        Me.Panel.BackColor = System.Drawing.Color.White
        Me.Panel.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel.Location = New System.Drawing.Point(0, 0)
        Me.Panel.Name = "Panel"
        Me.Panel.Size = New System.Drawing.Size(1055, 359)
        Me.Panel.TabIndex = 0
        '
        'RGB
        '
        Me.RGB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.RGB.Location = New System.Drawing.Point(115, 415)
        Me.RGB.Name = "RGB"
        Me.RGB.ReadOnly = True
        Me.RGB.Size = New System.Drawing.Size(234, 22)
        Me.RGB.TabIndex = 1
        Me.RGB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox
        '
        Me.GroupBox.Controls.Add(Me.ButtonColorPicker)
        Me.GroupBox.Controls.Add(Me.ButtonFP)
        Me.GroupBox.Controls.Add(Me.ButtonSystem)
        Me.GroupBox.Controls.Add(Me.ButtonWeb)
        Me.GroupBox.Location = New System.Drawing.Point(355, 355)
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.Size = New System.Drawing.Size(481, 81)
        Me.GroupBox.TabIndex = 4
        Me.GroupBox.TabStop = False
        '
        'ButtonColorPicker
        '
        Me.ButtonColorPicker.Appearance = System.Windows.Forms.Appearance.Button
        Me.ButtonColorPicker.BackColor = System.Drawing.Color.Silver
        Me.ButtonColorPicker.FlatAppearance.CheckedBackColor = System.Drawing.Color.ForestGreen
        Me.ButtonColorPicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonColorPicker.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ButtonColorPicker.ForeColor = System.Drawing.Color.White
        Me.ButtonColorPicker.Location = New System.Drawing.Point(335, 13)
        Me.ButtonColorPicker.Name = "ButtonColorPicker"
        Me.ButtonColorPicker.Size = New System.Drawing.Size(139, 62)
        Me.ButtonColorPicker.TabIndex = 3
        Me.ButtonColorPicker.TabStop = True
        Me.ButtonColorPicker.Text = "Pick color"
        Me.ButtonColorPicker.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ButtonColorPicker.UseVisualStyleBackColor = False
        '
        'ButtonFP
        '
        Me.ButtonFP.Appearance = System.Windows.Forms.Appearance.Button
        Me.ButtonFP.BackColor = System.Drawing.Color.Silver
        Me.ButtonFP.FlatAppearance.CheckedBackColor = System.Drawing.Color.ForestGreen
        Me.ButtonFP.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonFP.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ButtonFP.ForeColor = System.Drawing.Color.White
        Me.ButtonFP.Location = New System.Drawing.Point(225, 13)
        Me.ButtonFP.Name = "ButtonFP"
        Me.ButtonFP.Size = New System.Drawing.Size(103, 62)
        Me.ButtonFP.TabIndex = 2
        Me.ButtonFP.TabStop = True
        Me.ButtonFP.Text = "FP"
        Me.ButtonFP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ButtonFP.UseVisualStyleBackColor = False
        '
        'ButtonSystem
        '
        Me.ButtonSystem.Appearance = System.Windows.Forms.Appearance.Button
        Me.ButtonSystem.BackColor = System.Drawing.Color.Silver
        Me.ButtonSystem.FlatAppearance.CheckedBackColor = System.Drawing.Color.ForestGreen
        Me.ButtonSystem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonSystem.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ButtonSystem.ForeColor = System.Drawing.Color.White
        Me.ButtonSystem.Location = New System.Drawing.Point(116, 13)
        Me.ButtonSystem.Name = "ButtonSystem"
        Me.ButtonSystem.Size = New System.Drawing.Size(103, 62)
        Me.ButtonSystem.TabIndex = 1
        Me.ButtonSystem.TabStop = True
        Me.ButtonSystem.Text = "System"
        Me.ButtonSystem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ButtonSystem.UseVisualStyleBackColor = False
        '
        'ButtonWeb
        '
        Me.ButtonWeb.Appearance = System.Windows.Forms.Appearance.Button
        Me.ButtonWeb.BackColor = System.Drawing.Color.Silver
        Me.ButtonWeb.FlatAppearance.CheckedBackColor = System.Drawing.Color.ForestGreen
        Me.ButtonWeb.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonWeb.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ButtonWeb.ForeColor = System.Drawing.Color.White
        Me.ButtonWeb.Location = New System.Drawing.Point(7, 13)
        Me.ButtonWeb.Name = "ButtonWeb"
        Me.ButtonWeb.Size = New System.Drawing.Size(103, 62)
        Me.ButtonWeb.TabIndex = 0
        Me.ButtonWeb.TabStop = True
        Me.ButtonWeb.Text = "Web"
        Me.ButtonWeb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ButtonWeb.UseVisualStyleBackColor = False
        '
        'RGB_Label
        '
        Me.RGB_Label.BackColor = System.Drawing.Color.DimGray
        Me.RGB_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.RGB_Label.ForeColor = System.Drawing.Color.White
        Me.RGB_Label.Location = New System.Drawing.Point(0, 415)
        Me.RGB_Label.Name = "RGB_Label"
        Me.RGB_Label.Size = New System.Drawing.Size(117, 22)
        Me.RGB_Label.TabIndex = 2
        Me.RGB_Label.Text = "RGB:"
        Me.RGB_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'HEX_Label
        '
        Me.HEX_Label.BackColor = System.Drawing.Color.DimGray
        Me.HEX_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.HEX_Label.ForeColor = System.Drawing.Color.White
        Me.HEX_Label.Location = New System.Drawing.Point(0, 388)
        Me.HEX_Label.Name = "HEX_Label"
        Me.HEX_Label.Size = New System.Drawing.Size(117, 22)
        Me.HEX_Label.TabIndex = 0
        Me.HEX_Label.Text = "HEX:"
        Me.HEX_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'HEX
        '
        Me.HEX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.HEX.Location = New System.Drawing.Point(115, 388)
        Me.HEX.Name = "HEX"
        Me.HEX.ReadOnly = True
        Me.HEX.Size = New System.Drawing.Size(234, 22)
        Me.HEX.TabIndex = 2
        Me.HEX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'T_Color
        '
        Me.T_Color.Interval = 1
        '
        'ColorName_Label
        '
        Me.ColorName_Label.BackColor = System.Drawing.Color.DimGray
        Me.ColorName_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ColorName_Label.ForeColor = System.Drawing.Color.White
        Me.ColorName_Label.Location = New System.Drawing.Point(0, 361)
        Me.ColorName_Label.Name = "ColorName_Label"
        Me.ColorName_Label.Size = New System.Drawing.Size(117, 22)
        Me.ColorName_Label.TabIndex = 5
        Me.ColorName_Label.Text = "Color name:"
        Me.ColorName_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ColorName
        '
        Me.ColorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ColorName.Location = New System.Drawing.Point(115, 361)
        Me.ColorName.Name = "ColorName"
        Me.ColorName.ReadOnly = True
        Me.ColorName.Size = New System.Drawing.Size(234, 22)
        Me.ColorName.TabIndex = 6
        Me.ColorName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button_SELECT_COLOR
        '
        Me.Button_SELECT_COLOR.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.Button_SELECT_COLOR.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button_SELECT_COLOR.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Button_SELECT_COLOR.ForeColor = System.Drawing.Color.White
        Me.Button_SELECT_COLOR.Location = New System.Drawing.Point(858, 365)
        Me.Button_SELECT_COLOR.Name = "Button_SELECT_COLOR"
        Me.Button_SELECT_COLOR.Size = New System.Drawing.Size(197, 71)
        Me.Button_SELECT_COLOR.TabIndex = 7
        Me.Button_SELECT_COLOR.Text = "SELECT COLOR"
        Me.Button_SELECT_COLOR.UseVisualStyleBackColor = True
        '
        'FP_Services_Colors
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(1055, 444)
        Me.Controls.Add(Me.Button_SELECT_COLOR)
        Me.Controls.Add(Me.ColorName_Label)
        Me.Controls.Add(Me.ColorName)
        Me.Controls.Add(Me.Panel)
        Me.Controls.Add(Me.HEX_Label)
        Me.Controls.Add(Me.HEX)
        Me.Controls.Add(Me.RGB_Label)
        Me.Controls.Add(Me.GroupBox)
        Me.Controls.Add(Me.RGB)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FP_Services_Colors"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Színválasztó"
        Me.GroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel As System.Windows.Forms.Panel
    Friend WithEvents RGB As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonSystem As System.Windows.Forms.RadioButton
    Friend WithEvents ButtonWeb As System.Windows.Forms.RadioButton
    Friend WithEvents RGB_Label As System.Windows.Forms.Label
    Friend WithEvents HEX_Label As System.Windows.Forms.Label
    Friend WithEvents HEX As System.Windows.Forms.TextBox
    Friend WithEvents ButtonFP As System.Windows.Forms.RadioButton
    Friend WithEvents ButtonColorPicker As System.Windows.Forms.RadioButton
    Friend WithEvents T_Color As System.Windows.Forms.Timer
    Friend WithEvents ColorName_Label As System.Windows.Forms.Label
    Friend WithEvents ColorName As System.Windows.Forms.TextBox
    Friend WithEvents Button_SELECT_COLOR As System.Windows.Forms.Button
End Class
