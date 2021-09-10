<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_DB_Place
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FP_DB_Place))
        Me.SQLServer_Label = New System.Windows.Forms.Label()
        Me.UserPassword_Panel = New System.Windows.Forms.Panel()
        Me.Password = New System.Windows.Forms.TextBox()
        Me.UserId = New System.Windows.Forms.TextBox()
        Me.Password_Label = New System.Windows.Forms.Label()
        Me.UserId_Label = New System.Windows.Forms.Label()
        Me.SQLServer = New System.Windows.Forms.TextBox()
        Me.DbName_Label = New System.Windows.Forms.Label()
        Me.DbName = New System.Windows.Forms.TextBox()
        Me.IntegratedSecurity = New System.Windows.Forms.CheckBox()
        Me.Title_Label = New System.Windows.Forms.Label()
        Me.ButtonCancel = New System.Windows.Forms.PictureBox()
        Me.ButtonOK = New System.Windows.Forms.PictureBox()
        Me.ButtonTest = New System.Windows.Forms.PictureBox()
        Me.Title_SELEXPED_Label = New System.Windows.Forms.Label()
        Me.Pfd_Terminal = New System.Windows.Forms.TextBox()
        Me.Pfd_Terminal_Label = New System.Windows.Forms.Label()
        Me.WB = New System.Windows.Forms.WebBrowser()
        Me.ComputerName = New System.Windows.Forms.TextBox()
        Me.ComputerName_Label = New System.Windows.Forms.Label()
        Me.StartupPath = New System.Windows.Forms.TextBox()
        Me.StartupPath_Label = New System.Windows.Forms.Label()
        Me.ButtonInstalledTerminals = New System.Windows.Forms.PictureBox()
        Me.ButtonGetTerminal = New System.Windows.Forms.PictureBox()
        Me.UserPassword_Panel.SuspendLayout()
        CType(Me.ButtonCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ButtonOK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ButtonTest, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ButtonInstalledTerminals, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ButtonGetTerminal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SQLServer_Label
        '
        Me.SQLServer_Label.BackColor = System.Drawing.Color.DimGray
        Me.SQLServer_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.SQLServer_Label.ForeColor = System.Drawing.Color.White
        Me.SQLServer_Label.Location = New System.Drawing.Point(0, 25)
        Me.SQLServer_Label.Name = "SQLServer_Label"
        Me.SQLServer_Label.Size = New System.Drawing.Size(190, 22)
        Me.SQLServer_Label.TabIndex = 9999
        Me.SQLServer_Label.Text = "SQL-Server:"
        '
        'UserPassword_Panel
        '
        Me.UserPassword_Panel.BackColor = System.Drawing.Color.Transparent
        Me.UserPassword_Panel.Controls.Add(Me.Password)
        Me.UserPassword_Panel.Controls.Add(Me.UserId)
        Me.UserPassword_Panel.Controls.Add(Me.Password_Label)
        Me.UserPassword_Panel.Controls.Add(Me.UserId_Label)
        Me.UserPassword_Panel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.UserPassword_Panel.Location = New System.Drawing.Point(0, 95)
        Me.UserPassword_Panel.Name = "UserPassword_Panel"
        Me.UserPassword_Panel.Size = New System.Drawing.Size(482, 48)
        Me.UserPassword_Panel.TabIndex = 10
        '
        'Password
        '
        Me.Password.Location = New System.Drawing.Point(191, 26)
        Me.Password.Name = "Password"
        Me.Password.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.Password.Size = New System.Drawing.Size(290, 22)
        Me.Password.TabIndex = 12
        '
        'UserId
        '
        Me.UserId.Location = New System.Drawing.Point(191, 3)
        Me.UserId.Name = "UserId"
        Me.UserId.Size = New System.Drawing.Size(290, 22)
        Me.UserId.TabIndex = 11
        '
        'Password_Label
        '
        Me.Password_Label.BackColor = System.Drawing.Color.DimGray
        Me.Password_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Password_Label.ForeColor = System.Drawing.Color.White
        Me.Password_Label.Location = New System.Drawing.Point(0, 26)
        Me.Password_Label.Name = "Password_Label"
        Me.Password_Label.Size = New System.Drawing.Size(190, 22)
        Me.Password_Label.TabIndex = 9999
        Me.Password_Label.Text = "Password:"
        '
        'UserId_Label
        '
        Me.UserId_Label.BackColor = System.Drawing.Color.DimGray
        Me.UserId_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.UserId_Label.ForeColor = System.Drawing.Color.White
        Me.UserId_Label.Location = New System.Drawing.Point(0, 3)
        Me.UserId_Label.Name = "UserId_Label"
        Me.UserId_Label.Size = New System.Drawing.Size(190, 22)
        Me.UserId_Label.TabIndex = 9999
        Me.UserId_Label.Text = "User:"
        '
        'SQLServer
        '
        Me.SQLServer.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.SQLServer.Location = New System.Drawing.Point(191, 25)
        Me.SQLServer.Name = "SQLServer"
        Me.SQLServer.Size = New System.Drawing.Size(290, 22)
        Me.SQLServer.TabIndex = 0
        '
        'DbName_Label
        '
        Me.DbName_Label.BackColor = System.Drawing.Color.DimGray
        Me.DbName_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.DbName_Label.ForeColor = System.Drawing.Color.White
        Me.DbName_Label.Location = New System.Drawing.Point(0, 48)
        Me.DbName_Label.Name = "DbName_Label"
        Me.DbName_Label.Size = New System.Drawing.Size(190, 22)
        Me.DbName_Label.TabIndex = 9999
        Me.DbName_Label.Text = "Database:"
        '
        'DbName
        '
        Me.DbName.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.DbName.Location = New System.Drawing.Point(191, 48)
        Me.DbName.Name = "DbName"
        Me.DbName.Size = New System.Drawing.Size(290, 22)
        Me.DbName.TabIndex = 1
        '
        'IntegratedSecurity
        '
        Me.IntegratedSecurity.BackColor = System.Drawing.Color.DimGray
        Me.IntegratedSecurity.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.IntegratedSecurity.ForeColor = System.Drawing.Color.White
        Me.IntegratedSecurity.Location = New System.Drawing.Point(0, 71)
        Me.IntegratedSecurity.Name = "IntegratedSecurity"
        Me.IntegratedSecurity.Padding = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.IntegratedSecurity.Size = New System.Drawing.Size(482, 22)
        Me.IntegratedSecurity.TabIndex = 2
        Me.IntegratedSecurity.Text = "Windows NT authentication"
        Me.IntegratedSecurity.UseVisualStyleBackColor = False
        '
        'Title_Label
        '
        Me.Title_Label.BackColor = System.Drawing.Color.Maroon
        Me.Title_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Title_Label.ForeColor = System.Drawing.Color.Gold
        Me.Title_Label.Location = New System.Drawing.Point(0, 0)
        Me.Title_Label.Name = "Title_Label"
        Me.Title_Label.Size = New System.Drawing.Size(482, 24)
        Me.Title_Label.TabIndex = 9999
        Me.Title_Label.Text = "Connect to Microsoft SQL-Server"
        Me.Title_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Image = CType(resources.GetObject("ButtonCancel.Image"), System.Drawing.Image)
        Me.ButtonCancel.Location = New System.Drawing.Point(389, 242)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(44, 44)
        Me.ButtonCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ButtonCancel.TabIndex = 100084
        Me.ButtonCancel.TabStop = False
        '
        'ButtonOK
        '
        Me.ButtonOK.Image = CType(resources.GetObject("ButtonOK.Image"), System.Drawing.Image)
        Me.ButtonOK.Location = New System.Drawing.Point(439, 242)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(44, 44)
        Me.ButtonOK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ButtonOK.TabIndex = 100083
        Me.ButtonOK.TabStop = False
        '
        'ButtonTest
        '
        Me.ButtonTest.Image = CType(resources.GetObject("ButtonTest.Image"), System.Drawing.Image)
        Me.ButtonTest.Location = New System.Drawing.Point(3, 242)
        Me.ButtonTest.Name = "ButtonTest"
        Me.ButtonTest.Size = New System.Drawing.Size(44, 44)
        Me.ButtonTest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ButtonTest.TabIndex = 100085
        Me.ButtonTest.TabStop = False
        '
        'Title_SELEXPED_Label
        '
        Me.Title_SELEXPED_Label.BackColor = System.Drawing.Color.Maroon
        Me.Title_SELEXPED_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Title_SELEXPED_Label.ForeColor = System.Drawing.Color.Gold
        Me.Title_SELEXPED_Label.Location = New System.Drawing.Point(0, 146)
        Me.Title_SELEXPED_Label.Name = "Title_SELEXPED_Label"
        Me.Title_SELEXPED_Label.Size = New System.Drawing.Size(482, 24)
        Me.Title_SELEXPED_Label.TabIndex = 9999
        Me.Title_SELEXPED_Label.Text = "Connect to SELEXPED"
        Me.Title_SELEXPED_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Pfd_Terminal
        '
        Me.Pfd_Terminal.Location = New System.Drawing.Point(191, 171)
        Me.Pfd_Terminal.Name = "Pfd_Terminal"
        Me.Pfd_Terminal.Size = New System.Drawing.Size(290, 22)
        Me.Pfd_Terminal.TabIndex = 21
        '
        'Pfd_Terminal_Label
        '
        Me.Pfd_Terminal_Label.BackColor = System.Drawing.Color.DimGray
        Me.Pfd_Terminal_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Pfd_Terminal_Label.ForeColor = System.Drawing.Color.White
        Me.Pfd_Terminal_Label.Location = New System.Drawing.Point(0, 171)
        Me.Pfd_Terminal_Label.Name = "Pfd_Terminal_Label"
        Me.Pfd_Terminal_Label.Size = New System.Drawing.Size(190, 22)
        Me.Pfd_Terminal_Label.TabIndex = 9999
        Me.Pfd_Terminal_Label.Text = "Terminal:"
        '
        'WB
        '
        Me.WB.Location = New System.Drawing.Point(489, 0)
        Me.WB.MinimumSize = New System.Drawing.Size(23, 22)
        Me.WB.Name = "WB"
        Me.WB.Size = New System.Drawing.Size(657, 286)
        Me.WB.TabIndex = 100086
        '
        'ComputerName
        '
        Me.ComputerName.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ComputerName.Location = New System.Drawing.Point(191, 194)
        Me.ComputerName.Name = "ComputerName"
        Me.ComputerName.Size = New System.Drawing.Size(290, 22)
        Me.ComputerName.TabIndex = 100087
        '
        'ComputerName_Label
        '
        Me.ComputerName_Label.BackColor = System.Drawing.Color.DimGray
        Me.ComputerName_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ComputerName_Label.ForeColor = System.Drawing.Color.White
        Me.ComputerName_Label.Location = New System.Drawing.Point(0, 194)
        Me.ComputerName_Label.Name = "ComputerName_Label"
        Me.ComputerName_Label.Size = New System.Drawing.Size(190, 22)
        Me.ComputerName_Label.TabIndex = 100088
        Me.ComputerName_Label.Text = "Computer name:"
        '
        'StartupPath
        '
        Me.StartupPath.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.StartupPath.Location = New System.Drawing.Point(191, 217)
        Me.StartupPath.Name = "StartupPath"
        Me.StartupPath.Size = New System.Drawing.Size(290, 22)
        Me.StartupPath.TabIndex = 100089
        '
        'StartupPath_Label
        '
        Me.StartupPath_Label.BackColor = System.Drawing.Color.DimGray
        Me.StartupPath_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.StartupPath_Label.ForeColor = System.Drawing.Color.White
        Me.StartupPath_Label.Location = New System.Drawing.Point(0, 217)
        Me.StartupPath_Label.Name = "StartupPath_Label"
        Me.StartupPath_Label.Size = New System.Drawing.Size(190, 22)
        Me.StartupPath_Label.TabIndex = 100090
        Me.StartupPath_Label.Text = "Startup path:"
        '
        'ButtonInstalledTerminals
        '
        Me.ButtonInstalledTerminals.Image = CType(resources.GetObject("ButtonInstalledTerminals.Image"), System.Drawing.Image)
        Me.ButtonInstalledTerminals.Location = New System.Drawing.Point(53, 242)
        Me.ButtonInstalledTerminals.Name = "ButtonInstalledTerminals"
        Me.ButtonInstalledTerminals.Size = New System.Drawing.Size(44, 44)
        Me.ButtonInstalledTerminals.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ButtonInstalledTerminals.TabIndex = 100091
        Me.ButtonInstalledTerminals.TabStop = False
        '
        'ButtonGetTerminal
        '
        Me.ButtonGetTerminal.Image = CType(resources.GetObject("ButtonGetTerminal.Image"), System.Drawing.Image)
        Me.ButtonGetTerminal.Location = New System.Drawing.Point(459, 173)
        Me.ButtonGetTerminal.Name = "ButtonGetTerminal"
        Me.ButtonGetTerminal.Size = New System.Drawing.Size(22, 22)
        Me.ButtonGetTerminal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ButtonGetTerminal.TabIndex = 100092
        Me.ButtonGetTerminal.TabStop = False
        '
        'FP_DB_Place
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(1144, 293)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonGetTerminal)
        Me.Controls.Add(Me.ButtonInstalledTerminals)
        Me.Controls.Add(Me.StartupPath)
        Me.Controls.Add(Me.StartupPath_Label)
        Me.Controls.Add(Me.ComputerName)
        Me.Controls.Add(Me.ComputerName_Label)
        Me.Controls.Add(Me.WB)
        Me.Controls.Add(Me.Pfd_Terminal)
        Me.Controls.Add(Me.Pfd_Terminal_Label)
        Me.Controls.Add(Me.Title_SELEXPED_Label)
        Me.Controls.Add(Me.ButtonTest)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Me.Title_Label)
        Me.Controls.Add(Me.IntegratedSecurity)
        Me.Controls.Add(Me.DbName)
        Me.Controls.Add(Me.DbName_Label)
        Me.Controls.Add(Me.SQLServer)
        Me.Controls.Add(Me.UserPassword_Panel)
        Me.Controls.Add(Me.SQLServer_Label)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FP_DB_Place"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FP_DB_Place"
        Me.TopMost = True
        Me.UserPassword_Panel.ResumeLayout(False)
        Me.UserPassword_Panel.PerformLayout()
        CType(Me.ButtonCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ButtonOK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ButtonTest, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ButtonInstalledTerminals, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ButtonGetTerminal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SQLServer_Label As System.Windows.Forms.Label
    Friend WithEvents UserPassword_Panel As System.Windows.Forms.Panel
    Friend WithEvents UserId_Label As System.Windows.Forms.Label
    Friend WithEvents Password_Label As System.Windows.Forms.Label
    Friend WithEvents SQLServer As System.Windows.Forms.TextBox
    Friend WithEvents Password As System.Windows.Forms.TextBox
    Friend WithEvents UserId As System.Windows.Forms.TextBox
    Friend WithEvents DbName_Label As System.Windows.Forms.Label
    Friend WithEvents DbName As System.Windows.Forms.TextBox
    Friend WithEvents IntegratedSecurity As System.Windows.Forms.CheckBox
    Friend WithEvents Title_Label As System.Windows.Forms.Label
    Friend WithEvents ButtonCancel As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonOK As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonTest As System.Windows.Forms.PictureBox
    Friend WithEvents Title_SELEXPED_Label As System.Windows.Forms.Label
    Friend WithEvents Pfd_Terminal As System.Windows.Forms.TextBox
    Friend WithEvents Pfd_Terminal_Label As System.Windows.Forms.Label
    Friend WithEvents WB As System.Windows.Forms.WebBrowser
    Friend WithEvents ComputerName As System.Windows.Forms.TextBox
    Friend WithEvents ComputerName_Label As System.Windows.Forms.Label
    Friend WithEvents StartupPath As System.Windows.Forms.TextBox
    Friend WithEvents StartupPath_Label As System.Windows.Forms.Label
    Friend WithEvents ButtonInstalledTerminals As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonGetTerminal As System.Windows.Forms.PictureBox
End Class
