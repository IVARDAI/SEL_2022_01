<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FP_Map
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
    <Obsolete>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FP_Map))
        Me.WV_Map = New Microsoft.Toolkit.Forms.UI.Controls.WebView()
        CType(Me.WV_Map, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'WV_Map
        '
        Me.WV_Map.Location = New System.Drawing.Point(40, 34)
        Me.WV_Map.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WV_Map.Name = "WV_Map"
        Me.WV_Map.Size = New System.Drawing.Size(1279, 525)
        Me.WV_Map.Source = New System.Uri("https://selester.hu", System.UriKind.Absolute)
        Me.WV_Map.TabIndex = 0
        '
        'FP_Map
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1354, 595)
        Me.Controls.Add(Me.WV_Map)
        Me.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "FP_Map"
        Me.Text = "FP_Map"
        CType(Me.WV_Map, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents WV_Map As Microsoft.Toolkit.Forms.UI.Controls.WebView
End Class
