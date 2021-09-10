<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_DoFilter
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FP_DoFilter))
        Me.ButtonCancel = New System.Windows.Forms.PictureBox()
        Me.ButtonOK = New System.Windows.Forms.PictureBox()
        Me.Title_Label = New System.Windows.Forms.Label()
        Me.ButtonClear = New System.Windows.Forms.PictureBox()
        Me.NewRecord_YN = New System.Windows.Forms.CheckBox()
        Me.NewRecord_YN_Label = New System.Windows.Forms.Label()
        Me.DoFilter_Panel = New System.Windows.Forms.Panel()
        Me.ButtonHELP = New System.Windows.Forms.PictureBox()
        Me.FoundRecords_Label = New System.Windows.Forms.Label()
        CType(Me.ButtonCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ButtonOK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ButtonClear, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ButtonHELP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Location = New System.Drawing.Point(411, 416)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(44, 44)
        Me.ButtonCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ButtonCancel.TabIndex = 100082
        Me.ButtonCancel.TabStop = False
        '
        'ButtonOK
        '
        Me.ButtonOK.Location = New System.Drawing.Point(456, 416)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(44, 44)
        Me.ButtonOK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ButtonOK.TabIndex = 100081
        Me.ButtonOK.TabStop = False
        '
        'Title_Label
        '
        Me.Title_Label.BackColor = System.Drawing.Color.DimGray
        Me.Title_Label.Dock = System.Windows.Forms.DockStyle.Top
        Me.Title_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Title_Label.ForeColor = System.Drawing.Color.White
        Me.Title_Label.Location = New System.Drawing.Point(0, 0)
        Me.Title_Label.Name = "Title_Label"
        Me.Title_Label.Size = New System.Drawing.Size(500, 22)
        Me.Title_Label.TabIndex = 9999
        Me.Title_Label.Text = "Title_Label"
        Me.Title_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonClear
        '
        Me.ButtonClear.Location = New System.Drawing.Point(456, 23)
        Me.ButtonClear.Name = "ButtonClear"
        Me.ButtonClear.Size = New System.Drawing.Size(44, 44)
        Me.ButtonClear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ButtonClear.TabIndex = 100084
        Me.ButtonClear.TabStop = False
        '
        'NewRecord_YN
        '
        Me.NewRecord_YN.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.NewRecord_YN.BackColor = System.Drawing.Color.Silver
        Me.NewRecord_YN.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.NewRecord_YN.Location = New System.Drawing.Point(161, 30)
        Me.NewRecord_YN.Name = "NewRecord_YN"
        Me.NewRecord_YN.Size = New System.Drawing.Size(22, 22)
        Me.NewRecord_YN.TabIndex = 1
        Me.NewRecord_YN.UseVisualStyleBackColor = False
        '
        'NewRecord_YN_Label
        '
        Me.NewRecord_YN_Label.BackColor = System.Drawing.Color.DimGray
        Me.NewRecord_YN_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.NewRecord_YN_Label.ForeColor = System.Drawing.Color.White
        Me.NewRecord_YN_Label.Location = New System.Drawing.Point(0, 30)
        Me.NewRecord_YN_Label.Name = "NewRecord_YN_Label"
        Me.NewRecord_YN_Label.Size = New System.Drawing.Size(160, 22)
        Me.NewRecord_YN_Label.TabIndex = 9999
        Me.NewRecord_YN_Label.Text = "NewRecord_YN_Label"
        Me.NewRecord_YN_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DoFilter_Panel
        '
        Me.DoFilter_Panel.BackColor = System.Drawing.Color.Silver
        Me.DoFilter_Panel.Location = New System.Drawing.Point(0, 73)
        Me.DoFilter_Panel.Name = "DoFilter_Panel"
        Me.DoFilter_Panel.Size = New System.Drawing.Size(500, 337)
        Me.DoFilter_Panel.TabIndex = 1000
        '
        'ButtonHELP
        '
        Me.ButtonHELP.Location = New System.Drawing.Point(0, 416)
        Me.ButtonHELP.Name = "ButtonHELP"
        Me.ButtonHELP.Size = New System.Drawing.Size(44, 44)
        Me.ButtonHELP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ButtonHELP.TabIndex = 100085
        Me.ButtonHELP.TabStop = False
        '
        'FoundRecords_Label
        '
        Me.FoundRecords_Label.BackColor = System.Drawing.Color.DimGray
        Me.FoundRecords_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FoundRecords_Label.ForeColor = System.Drawing.Color.White
        Me.FoundRecords_Label.Location = New System.Drawing.Point(158, 429)
        Me.FoundRecords_Label.Name = "FoundRecords_Label"
        Me.FoundRecords_Label.Size = New System.Drawing.Size(160, 22)
        Me.FoundRecords_Label.TabIndex = 9999
        Me.FoundRecords_Label.Text = "FoundRecords_Label"
        Me.FoundRecords_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FP_DoFilter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(500, 460)
        Me.ControlBox = False
        Me.Controls.Add(Me.FoundRecords_Label)
        Me.Controls.Add(Me.ButtonHELP)
        Me.Controls.Add(Me.DoFilter_Panel)
        Me.Controls.Add(Me.NewRecord_YN_Label)
        Me.Controls.Add(Me.NewRecord_YN)
        Me.Controls.Add(Me.ButtonClear)
        Me.Controls.Add(Me.Title_Label)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonOK)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FP_DoFilter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = " "
        CType(Me.ButtonCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ButtonOK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ButtonClear, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ButtonHELP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonCancel As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonOK As System.Windows.Forms.PictureBox
    Friend WithEvents Title_Label As System.Windows.Forms.Label
    Friend WithEvents ButtonClear As System.Windows.Forms.PictureBox
    Friend WithEvents NewRecord_YN As System.Windows.Forms.CheckBox
    Friend WithEvents NewRecord_YN_Label As System.Windows.Forms.Label
    'Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    'Friend WithEvents Head_Line As Microsoft.VisualBasic.PowerPacks.LineShape
    Friend WithEvents DoFilter_Panel As System.Windows.Forms.Panel
    Friend WithEvents ButtonHELP As System.Windows.Forms.PictureBox
    Friend WithEvents FoundRecords_Label As System.Windows.Forms.Label
End Class
