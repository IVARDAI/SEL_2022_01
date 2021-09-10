<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FP_ErrorList
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
        Me.MainGrid_Label = New System.Windows.Forms.Label()
        Me.MainGrid = New System.Windows.Forms.DataGridView()
        CType(Me.MainGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainGrid_Label
        '
        Me.MainGrid_Label.BackColor = System.Drawing.Color.SteelBlue
        Me.MainGrid_Label.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.MainGrid_Label.ForeColor = System.Drawing.Color.White
        Me.MainGrid_Label.Location = New System.Drawing.Point(55, 49)
        Me.MainGrid_Label.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.MainGrid_Label.Name = "MainGrid_Label"
        Me.MainGrid_Label.Size = New System.Drawing.Size(772, 41)
        Me.MainGrid_Label.TabIndex = 9999
        Me.MainGrid_Label.Text = "Csomagolások"
        Me.MainGrid_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MainGrid
        '
        Me.MainGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.MainGrid.Location = New System.Drawing.Point(55, 95)
        Me.MainGrid.Margin = New System.Windows.Forms.Padding(6)
        Me.MainGrid.Name = "MainGrid"
        Me.MainGrid.Size = New System.Drawing.Size(772, 661)
        Me.MainGrid.TabIndex = 10000
        '
        'FP_ErrorList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(870, 857)
        Me.Controls.Add(Me.MainGrid_Label)
        Me.Controls.Add(Me.MainGrid)
        Me.Name = "FP_ErrorList"
        Me.Text = "FP_ErrorList"
        CType(Me.MainGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MainGrid_Label As System.Windows.Forms.Label
    Friend WithEvents MainGrid As System.Windows.Forms.DataGridView
End Class
