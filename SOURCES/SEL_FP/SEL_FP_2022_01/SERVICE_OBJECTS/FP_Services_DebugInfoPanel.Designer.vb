<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_Services_DebugInfoPanel
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
        Me.ControlName_Label = New System.Windows.Forms.Label
        Me.ParentControlName_Label = New System.Windows.Forms.Label
        Me.ControlType_Label = New System.Windows.Forms.Label
        Me.ControlName = New System.Windows.Forms.TextBox
        Me.FP_Name_Label = New System.Windows.Forms.Label
        Me.ParentControlName = New System.Windows.Forms.TextBox
        Me.ControlType = New System.Windows.Forms.TextBox
        Me.FP_Name = New System.Windows.Forms.TextBox
        Me.RecordID = New System.Windows.Forms.TextBox
        Me.RecordID_Label = New System.Windows.Forms.Label
        Me.RS_ID = New System.Windows.Forms.TextBox
        Me.RS_ID_Label = New System.Windows.Forms.Label
        Me.ServerObjectPrefix = New System.Windows.Forms.TextBox
        Me.ServerObjectPrefix_Label = New System.Windows.Forms.Label
        Me.Subprefix = New System.Windows.Forms.TextBox
        Me.Subprefix_Label = New System.Windows.Forms.Label
        Me.ConnectionString = New System.Windows.Forms.TextBox
        Me.ConnectionString_Label = New System.Windows.Forms.Label
        Me.TerminalString = New System.Windows.Forms.TextBox
        Me.Terminal_Label = New System.Windows.Forms.Label
        Me.Titel_Label = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'ControlName_Label
        '
        Me.ControlName_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ControlName_Label.ForeColor = System.Drawing.Color.DimGray
        Me.ControlName_Label.Location = New System.Drawing.Point(6, 24)
        Me.ControlName_Label.Name = "ControlName_Label"
        Me.ControlName_Label.Size = New System.Drawing.Size(161, 22)
        Me.ControlName_Label.TabIndex = 2
        Me.ControlName_Label.Text = "Control name:"
        '
        'ParentControlName_Label
        '
        Me.ParentControlName_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ParentControlName_Label.ForeColor = System.Drawing.Color.DimGray
        Me.ParentControlName_Label.Location = New System.Drawing.Point(6, 46)
        Me.ParentControlName_Label.Name = "ParentControlName_Label"
        Me.ParentControlName_Label.Size = New System.Drawing.Size(161, 22)
        Me.ParentControlName_Label.TabIndex = 4
        Me.ParentControlName_Label.Text = "Parent control name:"
        '
        'ControlType_Label
        '
        Me.ControlType_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ControlType_Label.ForeColor = System.Drawing.Color.DimGray
        Me.ControlType_Label.Location = New System.Drawing.Point(6, 68)
        Me.ControlType_Label.Name = "ControlType_Label"
        Me.ControlType_Label.Size = New System.Drawing.Size(161, 22)
        Me.ControlType_Label.TabIndex = 6
        Me.ControlType_Label.Text = "Control type:"
        '
        'ControlName
        '
        Me.ControlName.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ControlName.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ControlName.ForeColor = System.Drawing.Color.Black
        Me.ControlName.Location = New System.Drawing.Point(170, 24)
        Me.ControlName.Name = "ControlName"
        Me.ControlName.ReadOnly = True
        Me.ControlName.Size = New System.Drawing.Size(418, 22)
        Me.ControlName.TabIndex = 3
        '
        'FP_Name_Label
        '
        Me.FP_Name_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FP_Name_Label.ForeColor = System.Drawing.Color.DimGray
        Me.FP_Name_Label.Location = New System.Drawing.Point(6, 90)
        Me.FP_Name_Label.Name = "FP_Name_Label"
        Me.FP_Name_Label.Size = New System.Drawing.Size(161, 22)
        Me.FP_Name_Label.TabIndex = 8
        Me.FP_Name_Label.Text = "FP:"
        '
        'ParentControlName
        '
        Me.ParentControlName.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ParentControlName.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ParentControlName.ForeColor = System.Drawing.Color.Black
        Me.ParentControlName.Location = New System.Drawing.Point(170, 46)
        Me.ParentControlName.Name = "ParentControlName"
        Me.ParentControlName.ReadOnly = True
        Me.ParentControlName.Size = New System.Drawing.Size(418, 22)
        Me.ParentControlName.TabIndex = 5
        '
        'ControlType
        '
        Me.ControlType.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ControlType.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ControlType.ForeColor = System.Drawing.Color.Black
        Me.ControlType.Location = New System.Drawing.Point(170, 68)
        Me.ControlType.Name = "ControlType"
        Me.ControlType.ReadOnly = True
        Me.ControlType.Size = New System.Drawing.Size(418, 22)
        Me.ControlType.TabIndex = 7
        '
        'FP_Name
        '
        Me.FP_Name.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.FP_Name.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FP_Name.ForeColor = System.Drawing.Color.Black
        Me.FP_Name.Location = New System.Drawing.Point(170, 90)
        Me.FP_Name.Name = "FP_Name"
        Me.FP_Name.ReadOnly = True
        Me.FP_Name.Size = New System.Drawing.Size(418, 22)
        Me.FP_Name.TabIndex = 9
        '
        'RecordID
        '
        Me.RecordID.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.RecordID.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.RecordID.ForeColor = System.Drawing.Color.Black
        Me.RecordID.Location = New System.Drawing.Point(170, 112)
        Me.RecordID.Name = "RecordID"
        Me.RecordID.ReadOnly = True
        Me.RecordID.Size = New System.Drawing.Size(418, 22)
        Me.RecordID.TabIndex = 11
        '
        'RecordID_Label
        '
        Me.RecordID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.RecordID_Label.ForeColor = System.Drawing.Color.DimGray
        Me.RecordID_Label.Location = New System.Drawing.Point(6, 112)
        Me.RecordID_Label.Name = "RecordID_Label"
        Me.RecordID_Label.Size = New System.Drawing.Size(161, 22)
        Me.RecordID_Label.TabIndex = 10
        Me.RecordID_Label.Text = "Record ID:"
        '
        'RS_ID
        '
        Me.RS_ID.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.RS_ID.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.RS_ID.ForeColor = System.Drawing.Color.Black
        Me.RS_ID.Location = New System.Drawing.Point(170, 134)
        Me.RS_ID.Name = "RS_ID"
        Me.RS_ID.ReadOnly = True
        Me.RS_ID.Size = New System.Drawing.Size(418, 22)
        Me.RS_ID.TabIndex = 13
        '
        'RS_ID_Label
        '
        Me.RS_ID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.RS_ID_Label.ForeColor = System.Drawing.Color.DimGray
        Me.RS_ID_Label.Location = New System.Drawing.Point(6, 134)
        Me.RS_ID_Label.Name = "RS_ID_Label"
        Me.RS_ID_Label.Size = New System.Drawing.Size(161, 22)
        Me.RS_ID_Label.TabIndex = 12
        Me.RS_ID_Label.Text = "RS ID:"
        '
        'ServerObjectPrefix
        '
        Me.ServerObjectPrefix.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ServerObjectPrefix.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ServerObjectPrefix.ForeColor = System.Drawing.Color.Black
        Me.ServerObjectPrefix.Location = New System.Drawing.Point(170, 156)
        Me.ServerObjectPrefix.Name = "ServerObjectPrefix"
        Me.ServerObjectPrefix.ReadOnly = True
        Me.ServerObjectPrefix.Size = New System.Drawing.Size(418, 22)
        Me.ServerObjectPrefix.TabIndex = 15
        '
        'ServerObjectPrefix_Label
        '
        Me.ServerObjectPrefix_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ServerObjectPrefix_Label.ForeColor = System.Drawing.Color.DimGray
        Me.ServerObjectPrefix_Label.Location = New System.Drawing.Point(6, 156)
        Me.ServerObjectPrefix_Label.Name = "ServerObjectPrefix_Label"
        Me.ServerObjectPrefix_Label.Size = New System.Drawing.Size(161, 22)
        Me.ServerObjectPrefix_Label.TabIndex = 14
        Me.ServerObjectPrefix_Label.Text = "Server object prefix:"
        '
        'Subprefix
        '
        Me.Subprefix.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.Subprefix.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Subprefix.ForeColor = System.Drawing.Color.Black
        Me.Subprefix.Location = New System.Drawing.Point(170, 178)
        Me.Subprefix.Name = "Subprefix"
        Me.Subprefix.ReadOnly = True
        Me.Subprefix.Size = New System.Drawing.Size(418, 22)
        Me.Subprefix.TabIndex = 17
        '
        'Subprefix_Label
        '
        Me.Subprefix_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Subprefix_Label.ForeColor = System.Drawing.Color.DimGray
        Me.Subprefix_Label.Location = New System.Drawing.Point(6, 178)
        Me.Subprefix_Label.Name = "Subprefix_Label"
        Me.Subprefix_Label.Size = New System.Drawing.Size(161, 22)
        Me.Subprefix_Label.TabIndex = 16
        Me.Subprefix_Label.Text = "Subprefix:"
        '
        'ConnectionString
        '
        Me.ConnectionString.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ConnectionString.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ConnectionString.ForeColor = System.Drawing.Color.Black
        Me.ConnectionString.Location = New System.Drawing.Point(170, 200)
        Me.ConnectionString.Multiline = True
        Me.ConnectionString.Name = "ConnectionString"
        Me.ConnectionString.ReadOnly = True
        Me.ConnectionString.Size = New System.Drawing.Size(418, 46)
        Me.ConnectionString.TabIndex = 19
        '
        'ConnectionString_Label
        '
        Me.ConnectionString_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ConnectionString_Label.ForeColor = System.Drawing.Color.DimGray
        Me.ConnectionString_Label.Location = New System.Drawing.Point(6, 200)
        Me.ConnectionString_Label.Name = "ConnectionString_Label"
        Me.ConnectionString_Label.Size = New System.Drawing.Size(161, 22)
        Me.ConnectionString_Label.TabIndex = 18
        Me.ConnectionString_Label.Text = "Connection string:"
        '
        'TerminalString
        '
        Me.TerminalString.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TerminalString.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.TerminalString.ForeColor = System.Drawing.Color.Black
        Me.TerminalString.Location = New System.Drawing.Point(170, 246)
        Me.TerminalString.Name = "TerminalString"
        Me.TerminalString.ReadOnly = True
        Me.TerminalString.Size = New System.Drawing.Size(418, 22)
        Me.TerminalString.TabIndex = 21
        '
        'Terminal_Label
        '
        Me.Terminal_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Terminal_Label.ForeColor = System.Drawing.Color.DimGray
        Me.Terminal_Label.Location = New System.Drawing.Point(6, 246)
        Me.Terminal_Label.Name = "Terminal_Label"
        Me.Terminal_Label.Size = New System.Drawing.Size(161, 22)
        Me.Terminal_Label.TabIndex = 20
        Me.Terminal_Label.Text = "Terminal:"
        '
        'Titel_Label
        '
        Me.Titel_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Titel_Label.ForeColor = System.Drawing.Color.DimGray
        Me.Titel_Label.Location = New System.Drawing.Point(9, 3)
        Me.Titel_Label.Name = "Titel_Label"
        Me.Titel_Label.Size = New System.Drawing.Size(579, 16)
        Me.Titel_Label.TabIndex = 22
        Me.Titel_Label.Text = "[#FORM] debug info panel"
        Me.Titel_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FP_DebugInfoPanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.ClientSize = New System.Drawing.Size(593, 274)
        Me.Controls.Add(Me.Titel_Label)
        Me.Controls.Add(Me.TerminalString)
        Me.Controls.Add(Me.Terminal_Label)
        Me.Controls.Add(Me.ConnectionString)
        Me.Controls.Add(Me.ConnectionString_Label)
        Me.Controls.Add(Me.Subprefix)
        Me.Controls.Add(Me.Subprefix_Label)
        Me.Controls.Add(Me.ServerObjectPrefix)
        Me.Controls.Add(Me.ServerObjectPrefix_Label)
        Me.Controls.Add(Me.RS_ID)
        Me.Controls.Add(Me.RS_ID_Label)
        Me.Controls.Add(Me.RecordID)
        Me.Controls.Add(Me.RecordID_Label)
        Me.Controls.Add(Me.FP_Name)
        Me.Controls.Add(Me.ControlType)
        Me.Controls.Add(Me.ParentControlName)
        Me.Controls.Add(Me.FP_Name_Label)
        Me.Controls.Add(Me.ControlName)
        Me.Controls.Add(Me.ControlType_Label)
        Me.Controls.Add(Me.ParentControlName_Label)
        Me.Controls.Add(Me.ControlName_Label)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FP_DebugInfoPanel"
        Me.Text = "Debug info panel"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ControlName_Label As System.Windows.Forms.Label
    Friend WithEvents ParentControlName_Label As System.Windows.Forms.Label
    Friend WithEvents ControlType_Label As System.Windows.Forms.Label
    Friend WithEvents ControlName As System.Windows.Forms.TextBox
    Friend WithEvents FP_Name_Label As System.Windows.Forms.Label
    Friend WithEvents ParentControlName As System.Windows.Forms.TextBox
    Friend WithEvents ControlType As System.Windows.Forms.TextBox
    Friend WithEvents FP_Name As System.Windows.Forms.TextBox
    Friend WithEvents RecordID As System.Windows.Forms.TextBox
    Friend WithEvents RecordID_Label As System.Windows.Forms.Label
    Friend WithEvents RS_ID As System.Windows.Forms.TextBox
    Friend WithEvents RS_ID_Label As System.Windows.Forms.Label
    Friend WithEvents ServerObjectPrefix As System.Windows.Forms.TextBox
    Friend WithEvents ServerObjectPrefix_Label As System.Windows.Forms.Label
    Friend WithEvents Subprefix As System.Windows.Forms.TextBox
    Friend WithEvents Subprefix_Label As System.Windows.Forms.Label
    Friend WithEvents ConnectionString As System.Windows.Forms.TextBox
    Friend WithEvents ConnectionString_Label As System.Windows.Forms.Label
    Friend WithEvents TerminalString As System.Windows.Forms.TextBox
    Friend WithEvents Terminal_Label As System.Windows.Forms.Label
    Friend WithEvents Titel_Label As System.Windows.Forms.Label
End Class
