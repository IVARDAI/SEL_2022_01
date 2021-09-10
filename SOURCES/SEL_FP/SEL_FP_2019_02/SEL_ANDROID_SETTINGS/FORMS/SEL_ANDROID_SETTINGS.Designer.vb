<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SEL_ANDROID_SETTINGS
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DG_Header_Label = New System.Windows.Forms.Label()
        Me.PBox_SVC = New System.Windows.Forms.PictureBox()
        Me.WB = New System.Windows.Forms.WebBrowser()
        Me.PBox_download = New System.Windows.Forms.PictureBox()
        CType(Me.PBox_SVC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PBox_download, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.SteelBlue
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(1191, 358)
        Me.Label1.Margin = New System.Windows.Forms.Padding(0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(357, 34)
        Me.Label1.TabIndex = 10475
        Me.Label1.Text = "QR Az alkalmazás beállításhoz"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DG_Header_Label
        '
        Me.DG_Header_Label.BackColor = System.Drawing.Color.SteelBlue
        Me.DG_Header_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DG_Header_Label.ForeColor = System.Drawing.Color.White
        Me.DG_Header_Label.Location = New System.Drawing.Point(1201, -1)
        Me.DG_Header_Label.Margin = New System.Windows.Forms.Padding(0)
        Me.DG_Header_Label.Name = "DG_Header_Label"
        Me.DG_Header_Label.Size = New System.Drawing.Size(357, 34)
        Me.DG_Header_Label.TabIndex = 10474
        Me.DG_Header_Label.Text = "QR Az alkalmazás letöltéséhez"
        Me.DG_Header_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PBox_SVC
        '
        Me.PBox_SVC.Location = New System.Drawing.Point(1195, 408)
        Me.PBox_SVC.Name = "PBox_SVC"
        Me.PBox_SVC.Size = New System.Drawing.Size(348, 290)
        Me.PBox_SVC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PBox_SVC.TabIndex = 10471
        Me.PBox_SVC.TabStop = False
        '
        'WB
        '
        Me.WB.Location = New System.Drawing.Point(-114, -28)
        Me.WB.MinimumSize = New System.Drawing.Size(23, 22)
        Me.WB.Name = "WB"
        Me.WB.Size = New System.Drawing.Size(1270, 819)
        Me.WB.TabIndex = 10472
        Me.WB.TabStop = False
        Me.WB.Url = New System.Uri("", System.UriKind.Relative)
        '
        'PBox_download
        '
        Me.PBox_download.Location = New System.Drawing.Point(1195, 51)
        Me.PBox_download.Name = "PBox_download"
        Me.PBox_download.Size = New System.Drawing.Size(363, 278)
        Me.PBox_download.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PBox_download.TabIndex = 10473
        Me.PBox_download.TabStop = False
        '
        'SEL_ANDROID_SETTINGS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1611, 809)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DG_Header_Label)
        Me.Controls.Add(Me.PBox_SVC)
        Me.Controls.Add(Me.WB)
        Me.Controls.Add(Me.PBox_download)
        Me.Name = "SEL_ANDROID_SETTINGS"
        Me.Text = "SEL_ANDROID_SETTINGS"
        CType(Me.PBox_SVC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PBox_download, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DG_Header_Label As System.Windows.Forms.Label
    Friend WithEvents PBox_SVC As System.Windows.Forms.PictureBox
    Friend WithEvents WB As System.Windows.Forms.WebBrowser
    Friend WithEvents PBox_download As System.Windows.Forms.PictureBox
End Class
