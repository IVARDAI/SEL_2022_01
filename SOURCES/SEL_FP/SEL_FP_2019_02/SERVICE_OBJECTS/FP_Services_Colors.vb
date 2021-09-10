Public Class FP_Services_Colors

#Region "DECLARE"

    Public FPApp As FP_App

    Public OUT_Selected_Color As Color = Color.Transparent

    Private WebColorsList As New Dictionary(Of String, Color)
    Private SystemColorsList As New Dictionary(Of String, Color)
    Private Sorted_WebColorsList As New Dictionary(Of String, Integer)
    Private Sorted_SystemColorsList As New Dictionary(Of String, Integer)

    Private pos_x As Integer = 5
    Private pos_y As Integer = 5
    Private w As Integer

    Private PreviewLabel_Start As String = "Pick the color from the screen. (stop - F7)"
    Private PreviewLabel_Stop As String = "Start - F6"

    Private Form_Orig_Height As Integer
    Private Form_Orig_Width As Integer
    Private col_Preview_Orig_Top As Integer
    Private col_Preview_Orig_Left As Integer

    Private Drag As Boolean
    Private Mouse_X As Integer
    Private Mouse_Y As Integer

    Private ColorPickerMode As Boolean
    Private LastPickedRGB As String

#End Region

#Region "FORM CONSTRUCTOR"

    Public Sub New(ByVal MyFPApp As FP_App)
        InitializeComponent()

        FPApp = MyFPApp
    End Sub

#End Region

#Region "FORM PROPERTY"

    Public ReadOnly Property p_RGB() As String
        Get
            p_RGB = RGB.Text
        End Get
    End Property

    Public ReadOnly Property p_HEX() As String
        Get
            p_HEX = HEX.Text
        End Get
    End Property

#End Region

#Region "FORM EVENTS"

    Private Sub Form_Colors_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Drag = True
            Mouse_X = Cursor.Position.X - Me.Left
            Mouse_Y = Cursor.Position.Y - Me.Top
        End If
    End Sub

    Private Sub Form_Colors_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If Drag Then
            Me.Top = Cursor.Position.Y - Mouse_Y
            Me.Left = Cursor.Position.X - Mouse_X
        End If
    End Sub

    Private Sub Form_Colors_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        Drag = False
    End Sub

#End Region

#Region "CONTROL EVENTS"

    Private Sub ButtonSystem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSystem.CheckedChanged
        If ButtonSystem.Checked = True Then
            ColorName.Text = ""
            ColorName.BackColor = SystemColors.Control
            HEX.Text = ""
            RGB.Text = ""
            OUT_Selected_Color = Color.Transparent

            For Each p In Sorted_SystemColorsList
                ADD_CONTROL(p)
            Next
        Else
            For Each p In Sorted_SystemColorsList
                DELETE_CONTROL("col_" + p.Key)
            Next
        End If
        pos_x = 0
        pos_y = 5
    End Sub

    Private Sub ButtonWeb_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonWeb.CheckedChanged
        If ButtonWeb.Checked = True Then
            ColorName.Text = ""
            ColorName.BackColor = SystemColors.Control
            HEX.Text = ""
            RGB.Text = ""
            OUT_Selected_Color = Color.Transparent

            For Each p In Sorted_WebColorsList
                ADD_CONTROL(p)
            Next
        Else
            For Each p In Sorted_WebColorsList
                DELETE_CONTROL("col_" + p.Key)
            Next
        End If
        pos_x = 0
        pos_y = 5
    End Sub

    Private Sub ButtonColorPicker_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonColorPicker.CheckedChanged
        Dim l As Label

        If ButtonColorPicker.Checked = True Then
            ColorName.Text = ""
            ColorName.BackColor = SystemColors.Control
            HEX.Text = ""
            RGB.Text = ""
            OUT_Selected_Color = Color.Transparent

            l = New Label
            l.Visible = True
            l.Name = "col_Preview"
            l.Text = PreviewLabel_Stop
            l.Size = New Size(Panel.Width * 0.35, Panel.Height * 0.35)
            l.Location = New Point((Panel.Width / 2) - (l.Width / 2), (Panel.Height / 2) - (l.Height / 2))
            l.BorderStyle = BorderStyle.FixedSingle
            l.TextAlign = ContentAlignment.MiddleCenter
            l.ForeColor = Color.Wheat
            l.BackColor = Color.Black
            l.Font = New Font("Tahoma", 16, FontStyle.Regular)

            Panel.Controls.Add(l)
        Else
            DELETE_CONTROL("col_Preview")
        End If
    End Sub

    Private Sub Button_SELECT_COLOR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_SELECT_COLOR.Click
        If OUT_Selected_Color <> Color.Transparent Then
            DialogResult = Windows.Forms.DialogResult.OK

            Me.Close()
        End If
    End Sub

    Private Sub FP_Colors_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Dim col_Preview As Label = Panel.Controls.Item("col_Preview")

        If ButtonColorPicker.Checked = True Then
            'Color picker start
            If e.KeyCode = Keys.F6 Then
                ColorPickerMode = True

                col_Preview_Orig_Left = col_Preview.Left
                col_Preview_Orig_Top = col_Preview.Top
                Form_Orig_Height = Height
                Form_Orig_Width = Width

                FormBorderStyle = Windows.Forms.FormBorderStyle.None
                TopMost = True
                Height = col_Preview.Height + 1
                Width = col_Preview.Width + 1

                Button_SELECT_COLOR.Enabled = False
                ButtonWeb.Enabled = False
                ButtonSystem.Enabled = False
                ButtonFP.Enabled = False

                col_Preview.Text = PreviewLabel_Start
                col_Preview.Top = 0
                col_Preview.Left = 0

                AddHandler col_Preview.MouseDown, AddressOf Form_Colors_MouseDown
                AddHandler col_Preview.MouseMove, AddressOf Form_Colors_MouseMove
                AddHandler col_Preview.MouseUp, AddressOf Form_Colors_MouseUp

                T_Color.Start()
            End If

            'Color picker stop
            If e.KeyCode = Keys.F7 Then
                ColorPickerMode = False

                FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
                TopMost = False

                Height = Form_Orig_Height
                Width = Form_Orig_Width
                Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
                Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2

                Button_SELECT_COLOR.Enabled = True
                ButtonWeb.Enabled = True
                ButtonSystem.Enabled = True
                ButtonFP.Enabled = True

                col_Preview.Text = PreviewLabel_Stop
                col_Preview.Left = col_Preview_Orig_Left
                col_Preview.Top = col_Preview_Orig_Top

                RemoveHandler col_Preview.MouseDown, AddressOf Form_Colors_MouseDown
                RemoveHandler col_Preview.MouseMove, AddressOf Form_Colors_MouseMove
                RemoveHandler col_Preview.MouseUp, AddressOf Form_Colors_MouseUp

                T_Color.Stop()
            End If
        End If
    End Sub

    Private Sub FP_Colors_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Color As Color

        For Each col In [Enum].GetValues(GetType(KnownColor))
            Color = Color.FromKnownColor(col)
            If Color.Name <> "Transparent" Then
                If Color.IsSystemColor = False Then
                    WebColorsList.Add(Color.Name, Color)
                End If
                If Color.IsSystemColor = True Then
                    SystemColorsList.Add(Color.Name, Color)
                End If
            End If
        Next

        Sorted_WebColorsList = SET_HSB_SORT(WebColorsList)
        Sorted_SystemColorsList = SET_HSB_SORT(SystemColorsList)

        ButtonWeb.Checked = True
    End Sub

    Private Sub FP_Colors_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseHover
        Dim c As Control = DirectCast(sender, Control)

        If TypeOf c Is Label Then
            ColorName.Text = c.BackColor.Name
            ColorName.BackColor = c.BackColor
            ColorName.ForeColor = ContrastColor(c.BackColor)
            HEX.Text = Microsoft.VisualBasic.Conversion.Hex(Convert.ToInt32(c.BackColor.ToArgb))
            RGB.Text = c.BackColor.R.ToString + "; " + c.BackColor.G.ToString + "; " + c.BackColor.B.ToString
            OUT_Selected_Color = c.BackColor
        End If
    End Sub

    Private Sub Timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles T_Color.Tick
        Dim b As New Bitmap(1, 1)
        Dim g As Graphics = Graphics.FromImage(b)

        g.CopyFromScreen(New Point(MousePosition.X, MousePosition.Y), New Point(0, 0), b.Size)
        Dim ColorOfPixel As Color = b.GetPixel(0, 0)
        Panel.Controls.Item("col_Preview").BackColor = ColorOfPixel
        Panel.Controls.Item("col_Preview").ForeColor = ContrastColor(ColorOfPixel)
        ColorName.BackColor = ColorOfPixel
        ColorName.ForeColor = ContrastColor(ColorOfPixel)
        HEX.Text = Microsoft.VisualBasic.Conversion.Hex(Convert.ToInt32(ColorOfPixel.ToArgb))
        RGB.Text = ColorOfPixel.R.ToString + "; " + ColorOfPixel.G.ToString + "; " + ColorOfPixel.B.ToString
        OUT_Selected_Color = ColorOfPixel

        If RGB.Text <> LastPickedRGB Then
            BringToFront()
            Activate()

            LastPickedRGB = RGB.Text
        End If
    End Sub

#End Region

#Region "SUBS"

    Private Sub ADD_CONTROL(ByVal p As KeyValuePair(Of String, Integer))
        Dim t As Label

        If pos_y + 25 > Panel.Height Then
            pos_y = 5
            pos_x += w + 5
        End If

        t = New Label
        t.Visible = True
        t.Name = "col_" + p.Key
        t.Text = p.Key
        t.Location = New Point(pos_x, pos_y)
        t.BorderStyle = BorderStyle.FixedSingle
        t.TextAlign = ContentAlignment.MiddleLeft
        t.ForeColor = ContrastColor(Color.FromName(p.Key))
        t.BackColor = Color.FromName(p.Key)

        Panel.Controls.Add(t)

        AddHandler t.MouseHover, AddressOf FP_Colors_MouseHover

        w = t.Width
        pos_y += 25
    End Sub

    Private Sub DELETE_CONTROL(ByVal cName As String)
        If Panel.Controls.Find(cName, False).Length > 0 Then
            Panel.Controls.Item(cName).Dispose()
        End If
    End Sub

#End Region

#Region "FUNCTIONS"

    Private Function ContrastColor(ByVal col As Color) As Color
        Dim d As Integer = 0

        ' Counting the perceptive luminance - human eye favors green color... 
        Dim a As Double = 1 - (0.299 * col.R + 0.587 * col.G + 0.114 * col.B) / 255

        If a < 0.5 Then
            d = 0
        Else
            ' bright colors - black font
            d = 255
        End If
        ' dark colors - white font
        Return Color.FromArgb(d, d, d)
    End Function

    Private Function SET_HSB_SORT(ByVal DIC As Dictionary(Of String, Color)) As Dictionary(Of String, Integer)
        Dim Sorted_DIC As New Dictionary(Of String, Integer)
        Dim DT As New DataTable
        Dim i As Integer

        DT.Columns.Add("ColorName", GetType(String))
        DT.Columns.Add("H", GetType(Double))
        DT.Columns.Add("S", GetType(Double))
        DT.Columns.Add("B", GetType(Double))

        For Each p In DIC
            DT.Rows.Add(p.Key, p.Value.GetHue, p.Value.GetSaturation, p.Value.GetBrightness)
        Next

        Dim DTView As New DataView(DT)

        DTView.Sort = "H ASC, S ASC, B ASC"

        For Each r As DataRowView In DTView
            i += 1
            Sorted_DIC.Add(r.Row("ColorName"), i)
        Next

        Return Sorted_DIC
    End Function

#End Region

End Class