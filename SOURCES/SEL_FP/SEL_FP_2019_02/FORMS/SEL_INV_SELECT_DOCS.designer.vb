<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SEL_INV_SELECT_DOCS
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
        Me.ListView_Doc_Types = New System.Windows.Forms.ListView()
        Me.Label_Title = New System.Windows.Forms.Label()
        Me.BTN_HLP = New System.Windows.Forms.PictureBox()
        Me.BTN_OK = New System.Windows.Forms.PictureBox()
        Me.BTN_CANCEL = New System.Windows.Forms.PictureBox()
        Me.BTN_SELECT_ALL = New System.Windows.Forms.PictureBox()
        Me.BTN_UNSELECT_ALL = New System.Windows.Forms.PictureBox()
        Me.ListView_Included = New System.Windows.Forms.ListView()
        Me.ListView_Excluded = New System.Windows.Forms.ListView()
        Me.Label_Included = New System.Windows.Forms.Label()
        Me.Label_Excluded = New System.Windows.Forms.Label()
        Me.BTN_SELECT_ALL_EX = New System.Windows.Forms.PictureBox()
        Me.BTN_SELECT_ALL_IN = New System.Windows.Forms.PictureBox()
        CType(Me.BTN_HLP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_OK, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_CANCEL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_SELECT_ALL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_UNSELECT_ALL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_SELECT_ALL_EX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BTN_SELECT_ALL_IN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ListView_Doc_Types
        '
        Me.ListView_Doc_Types.CheckBoxes = True
        Me.ListView_Doc_Types.HideSelection = False
        Me.ListView_Doc_Types.Location = New System.Drawing.Point(12, 35)
        Me.ListView_Doc_Types.Name = "ListView_Doc_Types"
        Me.ListView_Doc_Types.Size = New System.Drawing.Size(220, 356)
        Me.ListView_Doc_Types.TabIndex = 0
        Me.ListView_Doc_Types.UseCompatibleStateImageBehavior = False
        '
        'Label_Title
        '
        Me.Label_Title.Location = New System.Drawing.Point(12, 9)
        Me.Label_Title.Name = "Label_Title"
        Me.Label_Title.Size = New System.Drawing.Size(776, 23)
        Me.Label_Title.TabIndex = 2
        Me.Label_Title.Text = "Label_Title"
        '
        'BTN_HLP
        '
        Me.BTN_HLP.Location = New System.Drawing.Point(644, 397)
        Me.BTN_HLP.Name = "BTN_HLP"
        Me.BTN_HLP.Size = New System.Drawing.Size(44, 44)
        Me.BTN_HLP.TabIndex = 3
        Me.BTN_HLP.TabStop = False
        '
        'BTN_OK
        '
        Me.BTN_OK.Location = New System.Drawing.Point(744, 397)
        Me.BTN_OK.Name = "BTN_OK"
        Me.BTN_OK.Size = New System.Drawing.Size(44, 44)
        Me.BTN_OK.TabIndex = 4
        Me.BTN_OK.TabStop = False
        '
        'BTN_CANCEL
        '
        Me.BTN_CANCEL.Location = New System.Drawing.Point(694, 397)
        Me.BTN_CANCEL.Name = "BTN_CANCEL"
        Me.BTN_CANCEL.Size = New System.Drawing.Size(44, 44)
        Me.BTN_CANCEL.TabIndex = 5
        Me.BTN_CANCEL.TabStop = False
        '
        'BTN_SELECT_ALL
        '
        Me.BTN_SELECT_ALL.Location = New System.Drawing.Point(15, 397)
        Me.BTN_SELECT_ALL.Name = "BTN_SELECT_ALL"
        Me.BTN_SELECT_ALL.Size = New System.Drawing.Size(22, 22)
        Me.BTN_SELECT_ALL.TabIndex = 6
        Me.BTN_SELECT_ALL.TabStop = False
        '
        'BTN_UNSELECT_ALL
        '
        Me.BTN_UNSELECT_ALL.Location = New System.Drawing.Point(43, 397)
        Me.BTN_UNSELECT_ALL.Name = "BTN_UNSELECT_ALL"
        Me.BTN_UNSELECT_ALL.Size = New System.Drawing.Size(22, 22)
        Me.BTN_UNSELECT_ALL.TabIndex = 7
        Me.BTN_UNSELECT_ALL.TabStop = False
        '
        'ListView_Included
        '
        Me.ListView_Included.CheckBoxes = True
        Me.ListView_Included.HideSelection = False
        Me.ListView_Included.Location = New System.Drawing.Point(238, 66)
        Me.ListView_Included.Name = "ListView_Included"
        Me.ListView_Included.Size = New System.Drawing.Size(550, 159)
        Me.ListView_Included.TabIndex = 8
        Me.ListView_Included.UseCompatibleStateImageBehavior = False
        '
        'ListView_Excluded
        '
        Me.ListView_Excluded.CheckBoxes = True
        Me.ListView_Excluded.HideSelection = False
        Me.ListView_Excluded.Location = New System.Drawing.Point(238, 256)
        Me.ListView_Excluded.Name = "ListView_Excluded"
        Me.ListView_Excluded.Size = New System.Drawing.Size(550, 135)
        Me.ListView_Excluded.TabIndex = 9
        Me.ListView_Excluded.UseCompatibleStateImageBehavior = False
        '
        'Label_Included
        '
        Me.Label_Included.Location = New System.Drawing.Point(238, 32)
        Me.Label_Included.Name = "Label_Included"
        Me.Label_Included.Size = New System.Drawing.Size(524, 23)
        Me.Label_Included.TabIndex = 10
        Me.Label_Included.Text = "Label_Included"
        '
        'Label_Excluded
        '
        Me.Label_Excluded.Location = New System.Drawing.Point(235, 230)
        Me.Label_Excluded.Name = "Label_Excluded"
        Me.Label_Excluded.Size = New System.Drawing.Size(524, 23)
        Me.Label_Excluded.TabIndex = 11
        Me.Label_Excluded.Text = "Label_Excluded"
        '
        'BTN_SELECT_ALL_EX
        '
        Me.BTN_SELECT_ALL_EX.Location = New System.Drawing.Point(269, 397)
        Me.BTN_SELECT_ALL_EX.Name = "BTN_SELECT_ALL_EX"
        Me.BTN_SELECT_ALL_EX.Size = New System.Drawing.Size(22, 22)
        Me.BTN_SELECT_ALL_EX.TabIndex = 13
        Me.BTN_SELECT_ALL_EX.TabStop = False
        '
        'BTN_SELECT_ALL_IN
        '
        Me.BTN_SELECT_ALL_IN.Location = New System.Drawing.Point(241, 397)
        Me.BTN_SELECT_ALL_IN.Name = "BTN_SELECT_ALL_IN"
        Me.BTN_SELECT_ALL_IN.Size = New System.Drawing.Size(22, 22)
        Me.BTN_SELECT_ALL_IN.TabIndex = 12
        Me.BTN_SELECT_ALL_IN.TabStop = False
        '
        'SEL_INV_SELECT_DOCS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.BTN_SELECT_ALL_EX)
        Me.Controls.Add(Me.BTN_SELECT_ALL_IN)
        Me.Controls.Add(Me.Label_Excluded)
        Me.Controls.Add(Me.Label_Included)
        Me.Controls.Add(Me.ListView_Excluded)
        Me.Controls.Add(Me.ListView_Included)
        Me.Controls.Add(Me.BTN_UNSELECT_ALL)
        Me.Controls.Add(Me.BTN_SELECT_ALL)
        Me.Controls.Add(Me.BTN_CANCEL)
        Me.Controls.Add(Me.BTN_OK)
        Me.Controls.Add(Me.BTN_HLP)
        Me.Controls.Add(Me.Label_Title)
        Me.Controls.Add(Me.ListView_Doc_Types)
        Me.Name = "SEL_INV_SELECT_DOCS"
        Me.Text = "SEL_INV_SELECT_DOCS"
        CType(Me.BTN_HLP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_OK, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_CANCEL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_SELECT_ALL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_UNSELECT_ALL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_SELECT_ALL_EX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BTN_SELECT_ALL_IN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ListView_Doc_Types As ListView
    Friend WithEvents Label_Title As Label
    Friend WithEvents BTN_HLP As PictureBox
    Friend WithEvents BTN_OK As PictureBox
    Friend WithEvents BTN_CANCEL As PictureBox
    Friend WithEvents BTN_SELECT_ALL As PictureBox
    Friend WithEvents BTN_UNSELECT_ALL As PictureBox
    Friend WithEvents ListView_Included As ListView
    Friend WithEvents ListView_Excluded As ListView
    Friend WithEvents Label_Included As Label
    Friend WithEvents Label_Excluded As Label
    Friend WithEvents BTN_SELECT_ALL_EX As PictureBox
    Friend WithEvents BTN_SELECT_ALL_IN As PictureBox
End Class
