<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_Login
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FP_Login))
        Me.Title_Label = New System.Windows.Forms.Label()
        Me.UserGroups = New System.Windows.Forms.ComboBox()
        Me.UserGroups_Label = New System.Windows.Forms.Label()
        Me.Btn_HLP = New System.Windows.Forms.PictureBox()
        Me.Btn_Cancel = New System.Windows.Forms.PictureBox()
        Me.Btn_OK = New System.Windows.Forms.PictureBox()
        Me.Users_ID_Label = New System.Windows.Forms.Label()
        Me.Password = New System.Windows.Forms.TextBox()
        Me.Password_Label = New System.Windows.Forms.Label()
        Me.Btn_LoggedUsers = New System.Windows.Forms.PictureBox()
        Me.Users_ID = New System.Windows.Forms.ListView()
        Me.WB = New System.Windows.Forms.WebBrowser()
        Me.SC_MAIN = New System.Windows.Forms.SplitContainer()
        Me.License_Button = New System.Windows.Forms.Button()
        CType(Me.Btn_HLP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_LoggedUsers, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SC_MAIN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SC_MAIN.Panel1.SuspendLayout()
        Me.SC_MAIN.Panel2.SuspendLayout()
        Me.SC_MAIN.SuspendLayout()
        Me.SuspendLayout()
        '
        'Title_Label
        '
        Me.Title_Label.BackColor = System.Drawing.Color.DimGray
        Me.Title_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Title_Label.ForeColor = System.Drawing.Color.White
        Me.Title_Label.Location = New System.Drawing.Point(17, 6)
        Me.Title_Label.Name = "Title_Label"
        Me.Title_Label.Size = New System.Drawing.Size(412, 22)
        Me.Title_Label.TabIndex = 9999
        Me.Title_Label.Text = "Terminals"
        Me.Title_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'UserGroups
        '
        Me.UserGroups.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.UserGroups.FormattingEnabled = True
        Me.UserGroups.Location = New System.Drawing.Point(154, 31)
        Me.UserGroups.Name = "UserGroups"
        Me.UserGroups.Size = New System.Drawing.Size(275, 22)
        Me.UserGroups.TabIndex = 1
        '
        'UserGroups_Label
        '
        Me.UserGroups_Label.BackColor = System.Drawing.Color.DimGray
        Me.UserGroups_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.UserGroups_Label.ForeColor = System.Drawing.Color.White
        Me.UserGroups_Label.Location = New System.Drawing.Point(17, 31)
        Me.UserGroups_Label.Margin = New System.Windows.Forms.Padding(3)
        Me.UserGroups_Label.Name = "UserGroups_Label"
        Me.UserGroups_Label.Size = New System.Drawing.Size(136, 22)
        Me.UserGroups_Label.TabIndex = 9999
        Me.UserGroups_Label.Text = "Group:"
        '
        'Btn_HLP
        '
        Me.Btn_HLP.Location = New System.Drawing.Point(13, 258)
        Me.Btn_HLP.Name = "Btn_HLP"
        Me.Btn_HLP.Size = New System.Drawing.Size(44, 44)
        Me.Btn_HLP.TabIndex = 100012
        Me.Btn_HLP.TabStop = False
        '
        'Btn_Cancel
        '
        Me.Btn_Cancel.Location = New System.Drawing.Point(321, 258)
        Me.Btn_Cancel.Name = "Btn_Cancel"
        Me.Btn_Cancel.Size = New System.Drawing.Size(44, 44)
        Me.Btn_Cancel.TabIndex = 100011
        Me.Btn_Cancel.TabStop = False
        '
        'Btn_OK
        '
        Me.Btn_OK.Location = New System.Drawing.Point(373, 258)
        Me.Btn_OK.Name = "Btn_OK"
        Me.Btn_OK.Size = New System.Drawing.Size(44, 44)
        Me.Btn_OK.TabIndex = 100010
        Me.Btn_OK.TabStop = False
        '
        'Users_ID_Label
        '
        Me.Users_ID_Label.BackColor = System.Drawing.Color.DimGray
        Me.Users_ID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Users_ID_Label.ForeColor = System.Drawing.Color.White
        Me.Users_ID_Label.Location = New System.Drawing.Point(17, 56)
        Me.Users_ID_Label.Margin = New System.Windows.Forms.Padding(3)
        Me.Users_ID_Label.Name = "Users_ID_Label"
        Me.Users_ID_Label.Size = New System.Drawing.Size(136, 22)
        Me.Users_ID_Label.TabIndex = 9999
        Me.Users_ID_Label.Text = "User:"
        '
        'Password
        '
        Me.Password.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Password.Location = New System.Drawing.Point(154, 228)
        Me.Password.Name = "Password"
        Me.Password.PasswordChar = Global.Microsoft.VisualBasic.ChrW(8226)
        Me.Password.Size = New System.Drawing.Size(275, 22)
        Me.Password.TabIndex = 3
        '
        'Password_Label
        '
        Me.Password_Label.BackColor = System.Drawing.Color.DimGray
        Me.Password_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Password_Label.ForeColor = System.Drawing.Color.White
        Me.Password_Label.Location = New System.Drawing.Point(17, 228)
        Me.Password_Label.Margin = New System.Windows.Forms.Padding(3)
        Me.Password_Label.Name = "Password_Label"
        Me.Password_Label.Size = New System.Drawing.Size(136, 22)
        Me.Password_Label.TabIndex = 9999
        Me.Password_Label.Text = "Password:"
        '
        'Btn_LoggedUsers
        '
        Me.Btn_LoggedUsers.Location = New System.Drawing.Point(192, 258)
        Me.Btn_LoggedUsers.Name = "Btn_LoggedUsers"
        Me.Btn_LoggedUsers.Size = New System.Drawing.Size(44, 44)
        Me.Btn_LoggedUsers.TabIndex = 100016
        Me.Btn_LoggedUsers.TabStop = False
        '
        'Users_ID
        '
        Me.Users_ID.Location = New System.Drawing.Point(154, 56)
        Me.Users_ID.Name = "Users_ID"
        Me.Users_ID.Size = New System.Drawing.Size(275, 166)
        Me.Users_ID.TabIndex = 2
        Me.Users_ID.UseCompatibleStateImageBehavior = False
        '
        'WB
        '
        Me.WB.Location = New System.Drawing.Point(16, 17)
        Me.WB.MinimumSize = New System.Drawing.Size(23, 22)
        Me.WB.Name = "WB"
        Me.WB.Size = New System.Drawing.Size(254, 303)
        Me.WB.TabIndex = 9999
        Me.WB.TabStop = False
        Me.WB.Url = New System.Uri("", System.UriKind.Relative)
        '
        'SC_MAIN
        '
        Me.SC_MAIN.Location = New System.Drawing.Point(45, 13)
        Me.SC_MAIN.Name = "SC_MAIN"
        '
        'SC_MAIN.Panel1
        '
        Me.SC_MAIN.Panel1.Controls.Add(Me.License_Button)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Title_Label)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Users_ID)
        Me.SC_MAIN.Panel1.Controls.Add(Me.UserGroups_Label)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Btn_LoggedUsers)
        Me.SC_MAIN.Panel1.Controls.Add(Me.UserGroups)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Password_Label)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Btn_OK)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Password)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Btn_Cancel)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Users_ID_Label)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Btn_HLP)
        '
        'SC_MAIN.Panel2
        '
        Me.SC_MAIN.Panel2.Controls.Add(Me.WB)
        Me.SC_MAIN.Size = New System.Drawing.Size(1027, 351)
        Me.SC_MAIN.SplitterDistance = 553
        Me.SC_MAIN.SplitterWidth = 5
        Me.SC_MAIN.TabIndex = 1
        Me.SC_MAIN.TabStop = False
        '
        'License_Button
        '
        Me.License_Button.Location = New System.Drawing.Point(43, 310)
        Me.License_Button.Name = "License_Button"
        Me.License_Button.Size = New System.Drawing.Size(329, 38)
        Me.License_Button.TabIndex = 4
        Me.License_Button.Text = "License_Btn"
        Me.License_Button.UseVisualStyleBackColor = True
        '
        'FP_Login
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1160, 368)
        Me.ControlBox = False
        Me.Controls.Add(Me.SC_MAIN)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FP_Login"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " "
        CType(Me.Btn_HLP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_LoggedUsers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SC_MAIN.Panel1.ResumeLayout(False)
        Me.SC_MAIN.Panel1.PerformLayout()
        Me.SC_MAIN.Panel2.ResumeLayout(False)
        CType(Me.SC_MAIN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SC_MAIN.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Title_Label As System.Windows.Forms.Label
    Friend WithEvents UserGroups As System.Windows.Forms.ComboBox
    Friend WithEvents UserGroups_Label As System.Windows.Forms.Label
    Friend WithEvents Btn_HLP As System.Windows.Forms.PictureBox
    Friend WithEvents Btn_Cancel As System.Windows.Forms.PictureBox
    Friend WithEvents Btn_OK As System.Windows.Forms.PictureBox
    Friend WithEvents Users_ID_Label As System.Windows.Forms.Label
    Friend WithEvents Password As System.Windows.Forms.TextBox
    Friend WithEvents Password_Label As System.Windows.Forms.Label
    Friend WithEvents Btn_LoggedUsers As System.Windows.Forms.PictureBox
    Friend WithEvents Users_ID As System.Windows.Forms.ListView
    Friend WithEvents WB As System.Windows.Forms.WebBrowser
    Friend WithEvents SC_MAIN As System.Windows.Forms.SplitContainer
    Friend WithEvents License_Button As System.Windows.Forms.Button
End Class
