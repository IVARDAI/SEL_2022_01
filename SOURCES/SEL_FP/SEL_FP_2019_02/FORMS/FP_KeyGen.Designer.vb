<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_KeyGen
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FP_KeyGen))
        Me.SC_MAIN = New System.Windows.Forms.SplitContainer()
        Me.Start_Code = New System.Windows.Forms.TextBox()
        Me.Start_Code_Label = New System.Windows.Forms.Label()
        Me.Key_Panel = New System.Windows.Forms.Panel()
        Me.SerialNo = New System.Windows.Forms.TextBox()
        Me.SerialNo_Label = New System.Windows.Forms.Label()
        Me.Valid = New System.Windows.Forms.TextBox()
        Me.Valid_Label = New System.Windows.Forms.Label()
        Me.CountOfUsers = New System.Windows.Forms.TextBox()
        Me.CountOfUsers_Label = New System.Windows.Forms.Label()
        Me.Product_ID = New System.Windows.Forms.TextBox()
        Me.Title_Label = New System.Windows.Forms.Label()
        Me.Product_ID_Label = New System.Windows.Forms.Label()
        Me.Btn_OK = New System.Windows.Forms.PictureBox()
        Me.Btn_Cancel = New System.Windows.Forms.PictureBox()
        Me.WB = New System.Windows.Forms.WebBrowser()
        Me.VersionNo = New System.Windows.Forms.TextBox()
        Me.VersionNo_Label = New System.Windows.Forms.Label()
        CType(Me.SC_MAIN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SC_MAIN.Panel1.SuspendLayout()
        Me.SC_MAIN.Panel2.SuspendLayout()
        Me.SC_MAIN.SuspendLayout()
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SC_MAIN
        '
        Me.SC_MAIN.Location = New System.Drawing.Point(0, 0)
        Me.SC_MAIN.Name = "SC_MAIN"
        '
        'SC_MAIN.Panel1
        '
        Me.SC_MAIN.Panel1.Controls.Add(Me.VersionNo)
        Me.SC_MAIN.Panel1.Controls.Add(Me.VersionNo_Label)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Start_Code)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Start_Code_Label)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Key_Panel)
        Me.SC_MAIN.Panel1.Controls.Add(Me.SerialNo)
        Me.SC_MAIN.Panel1.Controls.Add(Me.SerialNo_Label)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Valid)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Valid_Label)
        Me.SC_MAIN.Panel1.Controls.Add(Me.CountOfUsers)
        Me.SC_MAIN.Panel1.Controls.Add(Me.CountOfUsers_Label)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Product_ID)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Title_Label)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Product_ID_Label)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Btn_OK)
        Me.SC_MAIN.Panel1.Controls.Add(Me.Btn_Cancel)
        '
        'SC_MAIN.Panel2
        '
        Me.SC_MAIN.Panel2.Controls.Add(Me.WB)
        Me.SC_MAIN.Size = New System.Drawing.Size(1027, 270)
        Me.SC_MAIN.SplitterDistance = 583
        Me.SC_MAIN.SplitterWidth = 5
        Me.SC_MAIN.TabIndex = 1
        '
        'Start_Code
        '
        Me.Start_Code.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Start_Code.Location = New System.Drawing.Point(269, 183)
        Me.Start_Code.Name = "Start_Code"
        Me.Start_Code.Size = New System.Drawing.Size(188, 22)
        Me.Start_Code.TabIndex = 105
        '
        'Start_Code_Label
        '
        Me.Start_Code_Label.BackColor = System.Drawing.Color.DimGray
        Me.Start_Code_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Start_Code_Label.ForeColor = System.Drawing.Color.White
        Me.Start_Code_Label.Location = New System.Drawing.Point(133, 183)
        Me.Start_Code_Label.Margin = New System.Windows.Forms.Padding(3)
        Me.Start_Code_Label.Name = "Start_Code_Label"
        Me.Start_Code_Label.Size = New System.Drawing.Size(136, 22)
        Me.Start_Code_Label.TabIndex = 9999
        Me.Start_Code_Label.Text = "Start Code:"
        '
        'Key_Panel
        '
        Me.Key_Panel.Location = New System.Drawing.Point(14, 17)
        Me.Key_Panel.Name = "Key_Panel"
        Me.Key_Panel.Size = New System.Drawing.Size(112, 183)
        Me.Key_Panel.TabIndex = 9999
        '
        'SerialNo
        '
        Me.SerialNo.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.SerialNo.Location = New System.Drawing.Point(269, 92)
        Me.SerialNo.Name = "SerialNo"
        Me.SerialNo.Size = New System.Drawing.Size(188, 22)
        Me.SerialNo.TabIndex = 102
        Me.SerialNo.TabStop = False
        '
        'SerialNo_Label
        '
        Me.SerialNo_Label.BackColor = System.Drawing.Color.DimGray
        Me.SerialNo_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.SerialNo_Label.ForeColor = System.Drawing.Color.White
        Me.SerialNo_Label.Location = New System.Drawing.Point(133, 92)
        Me.SerialNo_Label.Margin = New System.Windows.Forms.Padding(3)
        Me.SerialNo_Label.Name = "SerialNo_Label"
        Me.SerialNo_Label.Size = New System.Drawing.Size(136, 22)
        Me.SerialNo_Label.TabIndex = 9999
        Me.SerialNo_Label.Text = "Serial number:"
        '
        'Valid
        '
        Me.Valid.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Valid.Location = New System.Drawing.Point(269, 122)
        Me.Valid.Name = "Valid"
        Me.Valid.Size = New System.Drawing.Size(188, 22)
        Me.Valid.TabIndex = 103
        Me.Valid.TabStop = False
        '
        'Valid_Label
        '
        Me.Valid_Label.BackColor = System.Drawing.Color.DimGray
        Me.Valid_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Valid_Label.ForeColor = System.Drawing.Color.White
        Me.Valid_Label.Location = New System.Drawing.Point(133, 122)
        Me.Valid_Label.Margin = New System.Windows.Forms.Padding(3)
        Me.Valid_Label.Name = "Valid_Label"
        Me.Valid_Label.Size = New System.Drawing.Size(136, 22)
        Me.Valid_Label.TabIndex = 9999
        Me.Valid_Label.Text = "Valid until:"
        '
        'CountOfUsers
        '
        Me.CountOfUsers.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.CountOfUsers.Location = New System.Drawing.Point(269, 152)
        Me.CountOfUsers.Name = "CountOfUsers"
        Me.CountOfUsers.Size = New System.Drawing.Size(188, 22)
        Me.CountOfUsers.TabIndex = 104
        Me.CountOfUsers.TabStop = False
        '
        'CountOfUsers_Label
        '
        Me.CountOfUsers_Label.BackColor = System.Drawing.Color.DimGray
        Me.CountOfUsers_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.CountOfUsers_Label.ForeColor = System.Drawing.Color.White
        Me.CountOfUsers_Label.Location = New System.Drawing.Point(133, 152)
        Me.CountOfUsers_Label.Margin = New System.Windows.Forms.Padding(3)
        Me.CountOfUsers_Label.Name = "CountOfUsers_Label"
        Me.CountOfUsers_Label.Size = New System.Drawing.Size(136, 22)
        Me.CountOfUsers_Label.TabIndex = 9999
        Me.CountOfUsers_Label.Text = "Count of users:"
        '
        'Product_ID
        '
        Me.Product_ID.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Product_ID.Location = New System.Drawing.Point(269, 27)
        Me.Product_ID.Name = "Product_ID"
        Me.Product_ID.Size = New System.Drawing.Size(188, 22)
        Me.Product_ID.TabIndex = 101
        Me.Product_ID.TabStop = False
        '
        'Title_Label
        '
        Me.Title_Label.BackColor = System.Drawing.Color.DimGray
        Me.Title_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Title_Label.ForeColor = System.Drawing.Color.White
        Me.Title_Label.Location = New System.Drawing.Point(141, 0)
        Me.Title_Label.Name = "Title_Label"
        Me.Title_Label.Size = New System.Drawing.Size(322, 24)
        Me.Title_Label.TabIndex = 9999
        Me.Title_Label.Text = "KEY GEN"
        Me.Title_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Product_ID_Label
        '
        Me.Product_ID_Label.BackColor = System.Drawing.Color.DimGray
        Me.Product_ID_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Product_ID_Label.ForeColor = System.Drawing.Color.White
        Me.Product_ID_Label.Location = New System.Drawing.Point(133, 27)
        Me.Product_ID_Label.Margin = New System.Windows.Forms.Padding(3)
        Me.Product_ID_Label.Name = "Product_ID_Label"
        Me.Product_ID_Label.Size = New System.Drawing.Size(136, 22)
        Me.Product_ID_Label.TabIndex = 9999
        Me.Product_ID_Label.Text = "Product:"
        '
        'Btn_OK
        '
        Me.Btn_OK.Location = New System.Drawing.Point(524, 194)
        Me.Btn_OK.Name = "Btn_OK"
        Me.Btn_OK.Size = New System.Drawing.Size(44, 44)
        Me.Btn_OK.TabIndex = 100010
        Me.Btn_OK.TabStop = False
        '
        'Btn_Cancel
        '
        Me.Btn_Cancel.Location = New System.Drawing.Point(465, 194)
        Me.Btn_Cancel.Name = "Btn_Cancel"
        Me.Btn_Cancel.Size = New System.Drawing.Size(44, 44)
        Me.Btn_Cancel.TabIndex = 100011
        Me.Btn_Cancel.TabStop = False
        '
        'WB
        '
        Me.WB.Location = New System.Drawing.Point(16, 17)
        Me.WB.MinimumSize = New System.Drawing.Size(23, 22)
        Me.WB.Name = "WB"
        Me.WB.Size = New System.Drawing.Size(254, 186)
        Me.WB.TabIndex = 9999
        Me.WB.TabStop = False
        Me.WB.Url = New System.Uri("", System.UriKind.Relative)
        '
        'VersionNo
        '
        Me.VersionNo.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.VersionNo.Location = New System.Drawing.Point(269, 55)
        Me.VersionNo.Name = "VersionNo"
        Me.VersionNo.Size = New System.Drawing.Size(188, 22)
        Me.VersionNo.TabIndex = 100012
        Me.VersionNo.TabStop = False
        '
        'VersionNo_Label
        '
        Me.VersionNo_Label.BackColor = System.Drawing.Color.DimGray
        Me.VersionNo_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.VersionNo_Label.ForeColor = System.Drawing.Color.White
        Me.VersionNo_Label.Location = New System.Drawing.Point(133, 55)
        Me.VersionNo_Label.Margin = New System.Windows.Forms.Padding(3)
        Me.VersionNo_Label.Name = "VersionNo_Label"
        Me.VersionNo_Label.Size = New System.Drawing.Size(136, 22)
        Me.VersionNo_Label.TabIndex = 100013
        Me.VersionNo_Label.Text = "Version:"
        '
        'FP_KeyGen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1062, 283)
        Me.ControlBox = False
        Me.Controls.Add(Me.SC_MAIN)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FP_KeyGen"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FP_KeyGen"
        Me.SC_MAIN.Panel1.ResumeLayout(False)
        Me.SC_MAIN.Panel1.PerformLayout()
        Me.SC_MAIN.Panel2.ResumeLayout(False)
        CType(Me.SC_MAIN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SC_MAIN.ResumeLayout(False)
        CType(Me.Btn_OK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btn_Cancel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SC_MAIN As System.Windows.Forms.SplitContainer
    Friend WithEvents Title_Label As System.Windows.Forms.Label
    Friend WithEvents Product_ID_Label As System.Windows.Forms.Label
    Friend WithEvents Btn_OK As System.Windows.Forms.PictureBox
    Friend WithEvents Btn_Cancel As System.Windows.Forms.PictureBox
    Friend WithEvents WB As System.Windows.Forms.WebBrowser
    Friend WithEvents Product_ID As System.Windows.Forms.TextBox
    Friend WithEvents Valid As System.Windows.Forms.TextBox
    Friend WithEvents Valid_Label As System.Windows.Forms.Label
    Friend WithEvents CountOfUsers As System.Windows.Forms.TextBox
    Friend WithEvents CountOfUsers_Label As System.Windows.Forms.Label
    Friend WithEvents SerialNo As System.Windows.Forms.TextBox
    Friend WithEvents SerialNo_Label As System.Windows.Forms.Label
    Friend WithEvents Key_Panel As System.Windows.Forms.Panel
    Friend WithEvents Start_Code As System.Windows.Forms.TextBox
    Friend WithEvents Start_Code_Label As System.Windows.Forms.Label
    Friend WithEvents VersionNo As System.Windows.Forms.TextBox
    Friend WithEvents VersionNo_Label As System.Windows.Forms.Label
End Class
