Public Class FP_HELP
    Private Enum Enum_Help_Orientation As Integer
        OnTop = 0
        OnBottom = 1
    End Enum

    Public Parent_FPf As FP_Form
    Public WithEvents Parent_Frm As Form
    Protected Friend DIC_HELP As New Dictionary(Of String, Struct_HELPText)
    Private HELP_Button As FP_PictureBox
    Private CurrentLink As String = ""
    Private CurrentControl As Control = Nothing
    Private Help_Size As Size = Nothing
    Private Help_Orientation As Enum_Help_Orientation
    Private WithEvents HELP_Button_PictureBox As PictureBox

    Sub New(ByVal MyFPf As FP_Form)
        InitializeComponent()

        Parent_FPf = MyFPf
        Parent_Frm = Parent_FPf.Frm

        HELP_Button = Parent_FPf.Btn_HELP
        If Not (HELP_Button Is Nothing) Then
            HELP_Button_PictureBox = HELP_Button.c
            HELP_Button.ToggleButton = True
        End If

        SET_LABELTEXTS_FROM_SEQ()
        CLEAR_HELP_DICTIONARY()
        INIT()
        Me.Show()
        Help_Size = Me.Size
        HELP_Short.MaximumSize = New Size(Help_Size.Width, 320)
        HELP_HIDE()
    End Sub


    Public Function SET_LABELTEXTS_FROM_SEQ() As Boolean
        Dim OUT As Boolean = False
        Dim MySEQ As New FP_SEQ(Parent_FPf.FPApp, "VBSEQ_HELP")
        Dim Row As DataRow

        For Each Row In MySEQ.DT_SEQ.Rows
            Dim FieldName = Row!Text1
            Dim c As Control

            If Not Controls.ContainsKey(FieldName) Then
                Parent_FPf.FPApp.DoErrorMsgBox("FP_HELP.SET_LABELTEXTS_FROM_SEQ", 0, String.Format("Field '{0}' not found.", FieldName))
            Else
                c = Controls(FieldName)
                c.Text = Row!Text3
            End If
        Next

        SET_LABELTEXTS_FROM_SEQ = OUT
    End Function

    Sub INIT()
        Dim wchar As String = String.Empty

        Parent_FPf.FPApp.PFDlesen("HELPSTATUS", wchar)
        HELP_CheckBox.Checked = (wchar = "1")
        HELP_Button.P_Pressed = (Not HELP_CheckBox.Checked)
    End Sub

    Public Sub CLEAR_HELP_DICTIONARY()
        DIC_HELP.Clear()
        If Not (HELP_Button Is Nothing) Then
            Dim HELP_Button_SEQ As New FP_SEQ(Parent_FPf.FPApp, "VBSEQ_HELPBUTTON")
            Dim HELP_Button_HelpText As Struct_HELPText

            With HELP_Button_HelpText
                .ShortText = HELP_Button_SEQ.GET_SEQ_BY_NUMBER(1).Text3
                .Link = HELP_Button_SEQ.GET_SEQ_BY_NUMBER(1).Text4
            End With

            DIC_HELP.Add(HELP_Button.c.Name, HELP_Button_HelpText)

            ADD_HELP_DICTIONARY("STANDARD", "")
        End If
    End Sub

    Public Function ADD_HELP_DICTIONARY(ByVal HELP_Key As String, ByVal SubPrefix As String) As Boolean
        Dim OUT As Boolean = False

        Dim HELP_SEQ As New FP_SEQ(Parent_FPf.FPApp, "HELP_" + HELP_Key, String.Format("Text2='{0}'", Trim(SubPrefix)))

        Dim Row As DataRow
        For Each Row In HELP_SEQ.DT_SEQ.Rows
            Dim AktHELPText As Struct_HELPText
            Dim AktKey As String = ""

            AktKey = Row!Text1
            With AktHELPText
                .ShortText = Row!Text3
                .Link = Row!Text4
            End With

            If DIC_HELP.ContainsKey(AktKey) Then
                DIC_HELP(AktKey) = AktHELPText
            Else
                DIC_HELP.Add(AktKey, AktHELPText)
            End If
        Next

        OUT = True

        ADD_HELP_DICTIONARY = OUT
    End Function

    Public Sub ADD_HELP_STANDARD_ITEM(ByVal StandardItem_Key As String, ByVal StandardItem_Name As String)
        If Not DIC_HELP.ContainsKey(StandardItem_Key) Then
            Parent_FPf.FPApp.DoErrorMsgBox("FP_HELP.ADD_HELP_STANDARD_ITEM", 0, String.Format("Standard Item '{0}' not found.", StandardItem_Key))
        Else
            If Not DIC_HELP.ContainsKey(StandardItem_Name) Then
                Dim AktHELPText As Struct_HELPText

                With AktHELPText
                    .ShortText = DIC_HELP(StandardItem_Key).ShortText
                    .Link = DIC_HELP(StandardItem_Key).Link
                End With

                DIC_HELP.Add(StandardItem_Name, AktHELPText)
            End If
        End If
    End Sub

    Public Sub REPLACE_HELPKEY_STANDARD_TO_EXISTING(ByVal StandardKey As String, ByVal c As Control)
        If Not (c Is Nothing) Then
            If Not DIC_HELP.ContainsKey(c.Name) Then
                If Not DIC_HELP.ContainsKey(StandardKey) Then
                    Parent_FPf.FPApp.DoErrorMsgBox("FP_HELP.REPLACE_HELPKEY_STANDARD_TO_EXISTING", 0, String.Format("Key '{0}' not found.", StandardKey))
                Else
                    Dim HELPText As Struct_HELPText

                    HELPText = DIC_HELP(StandardKey)
                    DIC_HELP.Add(c.Name, HELPText)
                End If
            End If
        End If
    End Sub

    Public Sub HELP_SHOW(ByVal c As Control, Optional ByVal SubControl_Name As String = "")
        'Ezt az eljarast a control MouseEnter esemenyebe tedd bele, ha nem FP_Controls-t hasznalsz.
        If Not (c Is Nothing) Then
            CurrentControl = Nothing

            Dim HELP_Key As String = IIf(SubControl_Name > "", SubControl_Name, c.Name)

            If HELP_Key = HELP_Button.c.Name Or HELP_Button.P_Pressed Then
                If DIC_HELP.ContainsKey(HELP_Key) Then

                    With DIC_HELP(HELP_Key)
                        HELP_Short.Text = .ShortText
                        CurrentLink = .Link
                        CurrentControl = c
                        HELP_Link.Visible = (CurrentLink > "")
                    End With

                    Help_Size.Height = 5 + HELP_Short.Height + 15 + IIf(HELP_CheckBox.Height > HELP_Link.Height, HELP_CheckBox.Height, HELP_Link.Height)

                    Me.Size = New Size(0, 0)

                    Dim c_Loc As Point = c.PointToScreen(New Point(0, 0))
                    Dim CurrentScreen As Screen = Screen.FromPoint(New Point(c_Loc.X + c.Width / 2, c_Loc.Y + c.Height / 2))

                    Dim Screen_RECT As New FP_L_Rect(CurrentScreen.Bounds)
                    Dim HELP_RECT_Right_Bottom As New FP_L_Rect(New Point(c_Loc.X + c.Width / 2, c_Loc.Y + c.Height - 3), Help_Size)
                    Dim HELP_RECT_Left_Bottom As New FP_L_Rect(New Point(c_Loc.X - c.Width / 2, c_Loc.Y + c.Height - 3), Help_Size)
                    Dim HELP_RECT_Right_Top As New FP_L_Rect(New Point(c_Loc.X + c.Width / 2, c_Loc.Y - Help_Size.Height + 3), Help_Size)
                    Dim HELP_RECT_Left_Top As New FP_L_Rect(New Point(c_Loc.X - c.Width / 2, c_Loc.Y - Help_Size.Height + 3), Help_Size)
                    Dim HELP_RECT_Left_1 As New FP_L_Rect(New Point(c_Loc.X - Help_Size.Width + 3, c_Loc.Y), Help_Size)
                    Dim HELP_RECT_Right_1 As New FP_L_Rect(New Point(c_Loc.X + c.Width - 3, c_Loc.Y), Help_Size)
                    Dim HELP_RECT_Left_2 As New FP_L_Rect(New Point(c_Loc.X - Help_Size.Width + 3, c_Loc.Y + c.Height - Help_Size.Height), Help_Size)
                    Dim HELP_RECT_Right_2 As New FP_L_Rect(New Point(c_Loc.X + c.Width - 3, c_Loc.Y + c.Height - Help_Size.Height), Help_Size)

                    Dim HELP_Koo As New Point(0, 0)

                    If Screen_RECT.Contains(HELP_RECT_Right_Bottom) Then
                        Help_Orientation = Enum_Help_Orientation.OnBottom
                        HELP_Koo = HELP_RECT_Right_Bottom.LeftTop
                    ElseIf Screen_RECT.Contains(HELP_RECT_Left_Bottom) Then
                        Help_Orientation = Enum_Help_Orientation.OnBottom
                        HELP_Koo = HELP_RECT_Left_Bottom.LeftTop
                    ElseIf Screen_RECT.Contains(HELP_RECT_Left_Top) Then
                        Help_Orientation = Enum_Help_Orientation.OnTop
                        HELP_Koo = HELP_RECT_Left_Top.LeftTop
                    ElseIf Screen_RECT.Contains(HELP_RECT_Right_Top) Then
                        Help_Orientation = Enum_Help_Orientation.OnTop
                        HELP_Koo = HELP_RECT_Right_Top.LeftTop
                    ElseIf Screen_RECT.Contains(HELP_RECT_Left_1) Then
                        Help_Orientation = Enum_Help_Orientation.OnTop
                        HELP_Koo = HELP_RECT_Left_1.LeftTop
                    ElseIf Screen_RECT.Contains(HELP_RECT_Left_2) Then
                        Help_Orientation = Enum_Help_Orientation.OnTop
                        HELP_Koo = HELP_RECT_Left_2.LeftTop
                    ElseIf Screen_RECT.Contains(HELP_RECT_Right_1) Then
                        Help_Orientation = Enum_Help_Orientation.OnTop
                        HELP_Koo = HELP_RECT_Right_1.LeftTop
                    ElseIf Screen_RECT.Contains(HELP_RECT_Right_2) Then
                        Help_Orientation = Enum_Help_Orientation.OnTop
                        HELP_Koo = HELP_RECT_Right_2.LeftTop
                    Else
                        Help_Orientation = Enum_Help_Orientation.OnTop
                        HELP_Koo = Screen_RECT.LeftTop
                    End If

                    Me.Left = HELP_Koo.X
                    Me.Top = HELP_Koo.Y
                    If Help_Orientation = Enum_Help_Orientation.OnTop Then
                        HELP_Short.Top = 5
                        HELP_Link.Top = HELP_Short.Top + HELP_Short.Height + 15
                    Else
                        HELP_Link.Top = 5
                        HELP_Short.Top = HELP_Link.Top + HELP_Link.Height + 15
                    End If
                    HELP_NoShow.Top = HELP_Link.Top
                    HELP_CheckBox.Top = HELP_Link.Top

                    Me.Region = Nothing
                    Me.Size = Help_Size

                End If
            End If
        End If
    End Sub

    Public Sub HELP_HIDE()
        'Ezt az eljarast a control MouseLeave esemenyebe tedd bele
        If Me.Size.Width > 0 Then
            Dim DoIt As Boolean = True

            If DoIt Then
                If Me.Bounds.Contains(Control.MousePosition) Then
                    Dim HELP_NoShow_Y As Integer = Me.PointToScreen(New Point(0, HELP_NoShow.Top)).Y

                    If Help_Orientation = Enum_Help_Orientation.OnTop Then
                        If Control.MousePosition.Y >= HELP_NoShow_Y Then
                            DoIt = False
                        End If
                    Else
                        If Control.MousePosition.Y <= HELP_NoShow_Y + HELP_NoShow.Height Then
                            DoIt = False
                        End If
                    End If
                End If
            End If

            If DoIt Then
                CurrentControl = Nothing

                Dim gr As New System.Drawing.Drawing2D.GraphicsPath

                gr.AddRectangle(New Rectangle(2, 2, Me.Width, Me.Height))

                Me.Region = New System.Drawing.Region(gr)

                Me.Size = New Size(0, 0)
                If Not (Parent_Frm Is Nothing) Then
                    Me.Location = Parent_Frm.PointToScreen(New Point(0, 0))
                End If
            End If
        End If
    End Sub

    Private Sub HELP_CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HELP_CheckBox.CheckedChanged
        If HELP_CheckBox.Checked Then
            HELP_OFF()
        Else
            HELP_ON()
        End If
    End Sub
    Sub HELP_ON()
        If HELP_CheckBox.Checked Then
            HELP_CheckBox.Checked = False
        End If
        If HELP_Button.P_Pressed <> True Then
            HELP_Button.P_Pressed = True
        End If
        Parent_FPf.FPApp.PFDinsertOrUpdate("HELPSTATUS", "0")
    End Sub
    Sub HELP_OFF()
        If Not HELP_CheckBox.Checked Then
            HELP_CheckBox.Checked = True
        End If
        If HELP_Button.P_Pressed <> False Then
            HELP_Button.P_Pressed = False
        End If
        If Parent_FPf.FPApp.Initialized Then
            Parent_FPf.FPApp.PFDinsertOrUpdate("HELPSTATUS", "1")
        End If
    End Sub

    Private Sub HELP_CheckBox_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles HELP_CheckBox.MouseLeave
        HELP_HIDE()
    End Sub

    Private Sub HELP_Link_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles HELP_Link.MouseLeave
        HELP_HIDE()
    End Sub

    Private Sub HELP_NoShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HELP_NoShow.Click
        HELP_CheckBox.Checked = (Not HELP_CheckBox.Checked)
    End Sub

    Private Sub HELP_NoShow_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles HELP_NoShow.MouseLeave
        HELP_HIDE()
    End Sub

    Private Sub HELP_Short_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles HELP_Short.MouseLeave
        HELP_HIDE()
    End Sub

    Private Sub HELP_Button_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles HELP_Button_PictureBox.MouseUp
        If e.Button = MouseButtons.Left Then
            HELP_CheckBox.Checked = (Not HELP_Button.P_Pressed)
        End If
    End Sub

    Private Sub HELP_Link_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HELP_Link.Click
        Parent_FPf.FPApp.HELP_Link_Execute(CurrentLink)
    End Sub

    Private Sub FP_HELP_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter
        If CurrentLink = "" Then
            HELP_HIDE()
        End If
    End Sub

    Private Sub FP_HELP_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        Dim DoIt As Boolean = True

        If Not (CurrentControl Is Nothing) Then
            If CurrentControl.Bounds.Contains(CurrentControl.PointToClient(Control.MousePosition)) Then
                DoIt = False
            End If
        End If

        If DoIt Then
            HELP_HIDE()
        End If
    End Sub

    Private Sub FP_HELP_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        HELP_HIDE()
    End Sub

    Private Sub FP_HELP_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        'If Me.Width > 0 And Me.Height > 0 Then
        '    Help_Size = Me.Size
        'Else
        '    If Help_Size.Width = 0 Then
        '        Parent_FPf.FPApp.DoErrorMsgBox("FP_HELP.FP_HELP_Shown", 0, "Nem tudtam meghatarozni a HELP eredeti meretet!")
        '    End If
        'End If
        'HELP_Short.MaximumSize = New Size(Help_Size.Width, 320)
        'HELP_HIDE()
    End Sub
End Class
