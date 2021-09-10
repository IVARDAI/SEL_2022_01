Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class FP_SELECT_CONTROL
    Public Event Fpc_BeforeUpdate(ByVal sender_FPc As FP_Control, ByRef Cancel As Boolean)
    Public WithEvents FpC As FP_Control
    ReadOnly C As Control
    ReadOnly FP As FP
    ReadOnly FPf As FP_Form
    Public P As Struct_FP_CONTROL_PROPS = FIELD_UNKNOWN_PROPS()
    Private SSQL As Stru_SQL
    Private SField As Stru_Field_Def

    Private bExitHandled As Boolean = False
    Private EditedText As String
    ReadOnly OrigBackColor As Drawing.Color
    Private Control_Definition As String
    Private sPanelHeight As String
    Private PanelHeight As Integer
    Private sMaxLength As String
    Private MAxLength As Integer
    Private No_Limit_To_List As Boolean = False
    Private Next_Control_Name As String
    Private GridWidth As Integer
    Private WithEvents Select_Panel As Panel
    Private WithEvents Select_Grid As DataGridView
    Private Grid_Prepared As Boolean = False
    Private CtrlPressed As Boolean
    Dim MyCancel As Boolean
    ReadOnly OrigFont As Drawing.Font
    ReadOnly SelFont As Drawing.Font

    Public WithEvents TextBox_END As TextBox
    Public WithEvents Select_TextBox As System.Windows.Forms.TextBox = Nothing
    Public Enum EnumStepDirection As Integer
        None = 0
        Up = 1
        Down = 2
        Home = 3
        [End] = 4
        Current = 5
    End Enum
    Private Structure Stru_SQL
        Dim SQL_From1 As String
        Dim SQL_From2 As String
        Dim OrderBy1 As String
        Dim OrderBy2 As String
        Dim DT_WHERE2 As String
    End Structure
    Private Structure Stru_Field_Def
        Dim Field_Text1 As String
        Dim Field_Text2 As String
        Dim Selected_ID As String
    End Structure

    Dim Dic_Fields As Dictionary(Of String, Stru_Select_Control_Field_Prop)
    ReadOnly DIC_Params As New Dictionary(Of String, String)
    Private Current_Field_Text As String
    Private Current_SQL_From As String
    Private Current_SQL_OrderBy As String

    Public Sub New(My_FpC As FP_Control)
        FpC = My_FpC
        With FpC
            C = .c
            P = .P
            FP = .FP
            FPf = .FPf
        End With
        Add_Select_Panel()
        OrigFont = CType(C, System.Windows.Forms.TextBox).Font
        SelFont = New Drawing.Font(OrigFont.Name, OrigFont.Size, Drawing.FontStyle.Underline)
        OrigBackColor = FpC.P.COLOR_NORMAL_BG
        CtrlPressed = False
        Load_Control_Definition()
    End Sub
    Private Function GetFieldLen(ByVal WhereQueryName As String, ByVal FieldName As String) As Integer
        Dim OUT As Integer = 0
        Dim DRow As DataRow
        Dim MySQL As String = My.Resources.ColLenSelect

        MySQL = MySQL.Replace("###TBNAME###", WhereQueryName)
        MySQL = MySQL.Replace("###FIELDNAME###", FieldName)
        DRow = FP.FPf.FPApp.DC.Qdf_get_DataRow(MySQL)
        If Not DRow Is Nothing Then
            OUT = DRow.Item("c_len")
        End If
        Return OUT
    End Function
    Private Sub Load_Control_Definition()
        If TypeOf (C) Is TextBox Then
            If P.DT_FixText_Key <> String.Empty Then
                Control_Definition = gl_FPApp.getFixText(P.DT_FixText_Key)
                If JSon_Select_Control_Load_Params(Control_Definition, DIC_Params, Dic_Fields, GridWidth) Then
                    SField.Selected_ID = DIC_Params("IdFieldName")
                    SField.Field_Text1 = DIC_Params("Field_Text1")
                    SField.Field_Text2 = DIC_Params("Field_Text2")
                    If SField.Field_Text2 = String.Empty Then SField.Field_Text2 = SField.Field_Text1

                    SSQL.DT_WHERE2 = FpC.P.DT_WHERE2
                    SSQL.SQL_From1 = DIC_Params("SQL_From1")
                    SSQL.SQL_From2 = DIC_Params("SQL_From2")
                    If SSQL.SQL_From2 = String.Empty Then SSQL.SQL_From2 = SSQL.SQL_From1
                    SSQL.OrderBy1 = DIC_Params("OrderBy1")
                    SSQL.OrderBy2 = DIC_Params("OrderBy2")

                    Next_Control_Name = FpC.P.Forced_NextField

                    sPanelHeight = DIC_Params("PanelHeight")
                    PanelHeight = 0
                    If IsNumeric(sPanelHeight) Then PanelHeight = CType(sPanelHeight, Integer)

                    sMaxLength = DIC_Params("MaxLength")
                    MAxLength = 0
                    If IsNumeric(sMaxLength) Then MAxLength = CType(sMaxLength, Integer)
                    If MAxLength = 0 Then
                        MAxLength = GetFieldLen(SSQL.SQL_From1, SField.Field_Text1)
                    Else
                        If MAxLength > GetFieldLen(SSQL.SQL_From1, SField.Field_Text1) Then
                            MAxLength = GetFieldLen(SSQL.SQL_From1, SField.Field_Text1)
                        End If
                    End If

                    No_Limit_To_List = DIC_Params("No_Limit_To_List")

                    Current_Field_Text = SField.Field_Text1
                    Current_SQL_From = String.Format("SELECT * FROM {0} ", SSQL.SQL_From1)
                    If SSQL.OrderBy1.Trim <> String.Empty Then
                        Current_SQL_OrderBy = String.Format(" ORDER BY {0}", SSQL.OrderBy1)
                    End If
                Else
                    SField.Selected_ID = ""
                    SField.Field_Text1 = ""
                    SField.Field_Text2 = ""
                    SSQL.OrderBy1 = ""
                    SSQL.OrderBy2 = ""
                End If
            End If
        End If
    End Sub
    Public Function GetControlWindowPosition(ByVal MyForm As FP,
                                ByVal ForContinuous As Boolean,
                              Optional ByVal MyDataGridView As DataGridView = Nothing) As Point
        Dim tmpx As Integer
        Dim tmpy As Integer
        Dim Point1 As Point         'ContinuousGrid balfelső sarkától a szerkesztett mezőig
        Dim Point2 As Point         'Képernyő sarkától a Continuous grid sarkáig
        Dim Point3 As Point         'Képernyő sarkától a Subform sarkáig
        Dim RetPoint As Point
        Dim MyCell As DataGridViewCell
        Dim NullPoint As Point
        Dim rowh As Integer
        Dim i As Integer
        Dim ACTION_VALID As Boolean = True
        Dim MyCellDisplayIndex As Integer
        Try
            NullPoint.X = 0
            NullPoint.Y = 0
            RetPoint = NullPoint

            Point1.X = 0
            Point1.Y = 0
            Point2.X = 0
            Point2.Y = 0
            Point3 = MyForm.FPf.Frm.PointToScreen(NullPoint)

            If ForContinuous Then
                Try
                    MyCell = MyForm.GRID.GRID.CurrentCell
                Catch ex As Exception
                    MyCell = Nothing
                End Try

                If MyCell Is Nothing Then
                    Try
                        MyCell = MyDataGridView.CurrentCell
                    Catch ex As Exception
                        MyCell = Nothing
                        ACTION_VALID = False
                    End Try
                End If

                If ACTION_VALID Then
                    rowh = MyDataGridView.Rows(0).Height
                    tmpy = rowh * (MyCell.RowIndex + 2)
                    MyCellDisplayIndex = MyDataGridView.Columns(MyCell.ColumnIndex).DisplayIndex
                    For i = 0 To MyDataGridView.Columns.Count - 1
                        If (MyDataGridView.Columns(i).Visible) And (MyDataGridView.Columns(i).DisplayIndex < MyCellDisplayIndex) Then
                            tmpx += MyDataGridView.Columns(i).Width
                        End If
                    Next
                    Point1.X = tmpx + MyDataGridView.RowHeadersWidth
                    Point1.Y = tmpy
                    Point2 = MyDataGridView.PointToScreen(NullPoint)

                    RetPoint.X = Point2.X - Point3.X + Point1.X
                    RetPoint.Y = Point2.Y - Point3.Y + Point1.Y
                End If
            Else    'If ForContinuous
                If Not IsNothing(CType(C, TextBox)) Then
                    tmpy = 0
                    tmpx = 0
                    Point1.X = tmpx
                    Point1.Y = tmpy
                    Point2 = CType(C, TextBox).PointToScreen(NullPoint)
                    RetPoint.X = Point2.X - Point3.X + Point1.X
                    RetPoint.Y = Point2.Y - Point3.Y + Point1.Y
                    If (MyForm.FPf.Frm.Height - 50) < (RetPoint.Y + Select_Panel.Height) Then
                        RetPoint.Y = MyForm.FPf.Frm.Height - Select_Panel.Height - 50
                    End If
                    If (MyForm.FPf.Frm.Width - 50) < (RetPoint.X + Select_Panel.Width) Then
                        RetPoint.X = MyForm.FPf.Frm.Width - Select_Panel.Width - 50
                    End If
                End If
            End If

        Catch ex As Exception
            MsgBox("ControlAssist.GetControlWindowPosition" & Err.Description)
            RetPoint = NullPoint
        End Try
        Return RetPoint
    End Function
    Private Sub Add_Select_Panel()
        If Select_Panel Is Nothing Then
            Select_Panel = New Panel With {
                .Visible = False,
                .Location = New System.Drawing.Point(0, 0),
                .Name = "Select_Control",
                .Size = New System.Drawing.Size(106, 18),
                .TabIndex = 99999,
                .TabStop = False
            }
            FPf.Frm.Controls.Add(Select_Panel)

            Select_Grid = New DataGridView
        End If
    End Sub
    Private Sub FillGrid()
        Dim MySQL As String
        Dim SelectTB As DataTable = Nothing
        Dim My_SQL_Where As String

        My_SQL_Where = "WHERE (1=1)"
        If Select_TextBox.Text.Trim = "*" Then Select_TextBox.Text = ""
        If Select_TextBox.Text.Trim <> String.Empty Then
            My_SQL_Where = String.Format("{0} AND ({1} Like '%{2}%')", My_SQL_Where, Current_Field_Text, Select_TextBox.Text.Trim)
        End If
        If SSQL.DT_WHERE2 <> String.Empty Then
            My_SQL_Where = String.Format("{0} AND ({1})", My_SQL_Where, SSQL.DT_WHERE2)
        End If
        If FpC.P.DT_WHERE2 <> String.Empty Then
            My_SQL_Where = String.Format("{0} AND ({1})", My_SQL_Where, FpC.P.DT_WHERE2)
        End If

        MySQL = String.Format("{0}{1}{2}", Current_SQL_From, My_SQL_Where, Current_SQL_OrderBy)
        gl_FPApp.DC.Qdf_Fill_DT(MySQL, SelectTB)

        Select_Grid.DataSource = SelectTB
        If Not Grid_Prepared Then SetGridColumns(SelectTB)
        Select_Grid.Refresh()
    End Sub
    Private Sub SetGridColumns(ByVal GridTable As DataTable)
        Dim Col As DataColumn
        Dim ColName As String
        Dim HeaderText As String
        For Each Col In GridTable.Columns
            ColName = Col.ColumnName.ToUpper
            If Dic_Fields.ContainsKey(ColName) Then
                Select_Grid.Columns(ColName).Visible = Dic_Fields(ColName).Visible
                Select_Grid.Columns(ColName).Width = Dic_Fields(ColName).FieldLen
                HeaderText = Dic_Fields(ColName).HeaderTexts(gl_FPApp.LandDialog).HeaderText   'Dic_Fields(ColName).HeaderTexts.HeaderText
                Select_Grid.Columns(ColName).HeaderText = HeaderText
            Else
                Select_Grid.Columns(ColName).Visible = False
            End If
        Next
        Grid_Prepared = True
    End Sub
    Private Sub ExitWithSave()
        If Not bExitHandled Then
            Dim SelectedID As Integer
            Dim SelectedText As String = ""
            Dim AV As Boolean = True
            Dim Focused_Control As Control
            bExitHandled = True

            If Select_Grid.RowCount <= 0 Then
                SelectedID = 0
                If Not No_Limit_To_List Then
                    FpC.Selected_ID = 0
                    FpC.P_VALUE = ""
                    AV = False
                Else
                    SelectedText = Select_TextBox.Text
                End If
            Else
                If Select_TextBox.Text = "" Then
                    SelectedID = 0
                    SelectedText = ""
                Else
                    If Select_Grid.CurrentCell Is Nothing Then
                        Select_Grid.CurrentCell = Select_Grid(SField.Selected_ID, 0)
                    End If
                    SelectedID = Select_Grid(SField.Selected_ID, Select_Grid.CurrentCell.RowIndex).Value
                    SelectedText = Select_Grid(SField.Field_Text1, Select_Grid.CurrentCell.RowIndex).Value
                End If
            End If
            If AV Then
                FpC.P_VALUE = SelectedText
                FpC.Selected_ID = SelectedID
            End If
            If Not MyCancel Then
                FpC.RAISEEVENT_Field_Before_Update(FpC, MyCancel)
            End If
            If Not MyCancel Then
                FP.RAISEEVENT_Form_Field_AfterUpdate(FpC)
            End If
            If MyCancel Then
                FpC.UNDO()
            End If
            Select_Panel.Visible = False
            FpC.c.Focus()
            If Next_Control_Name <> String.Empty Then
                If FP.FPf.CONTROLS.ContainsKey(Next_Control_Name) Then
                    Focused_Control = FP.FPf.CONTROLS(Next_Control_Name)
                    Focused_Control.Focus()
                End If
            End If
        End If
    End Sub
    Private Sub ExitWithSave(ByVal Focused_Control As FP_Control)
        If Not bExitHandled Then
            bExitHandled = True
            Dim SelectedID As Integer
            Dim SelectedText As String = ""
            Dim AV As Boolean = True

            If Select_Grid.RowCount <= 0 Then
                SelectedID = 0
                If Not No_Limit_To_List Then
                    FpC.Selected_ID = 0
                    FpC.P_VALUE = ""
                    AV = False
                Else
                    SelectedText = Select_TextBox.Text
                End If
            Else
                If Select_TextBox.Text = "" Then
                    SelectedID = 0
                    SelectedText = ""
                Else
                    If Select_Grid.CurrentCell Is Nothing Then
                        Select_Grid.CurrentCell = Select_Grid(SField.Selected_ID, 0)
                    End If
                    SelectedID = Select_Grid(SField.Selected_ID, Select_Grid.CurrentCell.RowIndex).Value
                    SelectedText = Select_Grid(SField.Field_Text1, Select_Grid.CurrentCell.RowIndex).Value
                End If
            End If
            If AV Then
                FpC.P_VALUE = SelectedText
                FpC.Selected_ID = SelectedID
            End If
            If Not MyCancel Then
                'RaiseEvent Fpc_BeforeUpdate(FpC, MyCancel)
                FpC.RAISEEVENT_Field_Before_Update(FpC, MyCancel)
            End If
            If MyCancel Then
                FpC.UNDO()
            End If
            Select_Panel.Visible = False
            If Not Focused_Control Is Nothing Then Focused_Control.c.Focus()
        End If
    End Sub
    Private Sub ExitWithoutSave()
        Select_Panel.Visible = False
        Exit Sub
    End Sub
    Private Sub Set_Grid_Current_Row(ByVal stepdir As EnumStepDirection, Optional ByVal FixID As Integer = 0)
        Dim CurrRowInd As Integer
        Dim MaxROwInd As Integer
        If Select_Grid.RowCount > 0 Then
            If Select_Grid.CurrentCell Is Nothing Then
                CurrRowInd = 0
            Else
                CurrRowInd = Select_Grid.CurrentCell.RowIndex
            End If
            MaxROwInd = Select_Grid.RowCount - 1

            Select Case stepdir
                Case EnumStepDirection.Down
                    If CurrRowInd < MaxROwInd Then
                        CurrRowInd += 1
                    End If
                Case EnumStepDirection.End
                    CurrRowInd = MaxROwInd
                Case EnumStepDirection.Home
                    CurrRowInd = 0
                Case EnumStepDirection.Up
                    If CurrRowInd > 0 Then
                        CurrRowInd -= 1
                    End If
                Case EnumStepDirection.Current
                    CurrRowInd = FixID
                Case Else
            End Select
        End If
        If CurrRowInd < 0 Then
            Select_Grid.CurrentCell = Nothing
        Else
            Try
                Select_Grid.CurrentCell = Select_Grid(SField.Field_Text1, CurrRowInd)
            Catch ex As Exception
                Debug.Print(Err.Description)
            End Try
        End If
        If Select_Grid.CurrentCell Is Nothing Then

        Else
            Select_TextBox.Text = Select_Grid.CurrentCell.Value
            Select_TextBox.Focus()
            Select_TextBox.SelectionStart = Select_TextBox.Text.Length
        End If
    End Sub
    Private Sub Set_TextBox()
        If CtrlPressed Then
            Select_TextBox.BackColor = Drawing.Color.AntiqueWhite
            Select_TextBox.Font = SelFont

            Current_Field_Text = SField.Field_Text2
            Current_SQL_From = String.Format("SELECT * FROM {0} ", SSQL.SQL_From2)
            If SSQL.OrderBy2.Trim <> String.Empty Then
                Current_SQL_OrderBy = String.Format(" ORDER BY {0}", SSQL.OrderBy2)
            End If
        Else
            Select_TextBox.BackColor = OrigBackColor
            Select_TextBox.Font = OrigFont

            Current_Field_Text = SField.Field_Text1
            Current_SQL_From = String.Format("SELECT * FROM {0} ", SSQL.SQL_From1)
            If SSQL.OrderBy1.Trim <> String.Empty Then
                Current_SQL_OrderBy = String.Format(" ORDER BY {0}", SSQL.OrderBy1)
            End If
        End If
    End Sub
    Private Sub FPC_Field_KeyPreview_KeyDown(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByRef e As System.Windows.Forms.KeyEventArgs) Handles FpC.Field_KeyPreview_KeyDown
        If e.Control Then
            If e.KeyCode = Keys.S Then Exit Sub
            If e.KeyCode = Keys.N Then Exit Sub
            If e.Shift Then
                CtrlPressed = Not CtrlPressed
            End If
        End If
        If e.KeyCode = Keys.Escape Then Exit Sub
        If Not Select_Panel Is Nothing Then
            If Select_Panel.Visible Then
                e.Handled = True
            End If
        End If
        If CtrlPressed Then
            FpC.P.COLOR_NORMAL_BG = Drawing.Color.AntiqueWhite
            CType(FpC.c, System.Windows.Forms.TextBox).Font = SelFont
        Else
            FpC.P.COLOR_NORMAL_BG = OrigBackColor
            CType(FpC.c, System.Windows.Forms.TextBox).Font = OrigFont
        End If
    End Sub
    Private Sub FPC_Field_KeyPreview_KeyPress(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByRef e As System.Windows.Forms.KeyPressEventArgs) Handles FpC.Field_KeyPreview_KeyPress
        Dim MyChar As Char
        Dim P As System.Drawing.Point
        MyCancel = False
        If (Asc(e.KeyChar) <= 32) And (Asc(e.KeyChar) <> 8) Then
            Exit Sub
        End If
        Reset_Where()
        If FpC.c.GetType.ToString.ToUpper = "SYSTEM.WINDOWS.FORMS.TEXTBOX" Then
            Dim txtLeft As String
            Dim txtRight As String
            Dim asc_char As Integer
            Dim MyTxtBox As TextBox = CType(FpC.c, TextBox)
            Dim index As Integer = MyTxtBox.SelectionStart
            Dim SelLen As Integer = MyTxtBox.SelectionLength

            If SelLen > 0 Then
                txtLeft = MyTxtBox.Text.Substring(0, index)
                txtRight = MyTxtBox.Text.Substring(index + SelLen, MyTxtBox.Text.Length - (index + SelLen))
                MyTxtBox.Text = String.Format("{0}{1}", txtLeft, txtRight)
            End If

            Dim CurrentLine As Integer = MyTxtBox.GetLineFromCharIndex(index)
            Dim currentColumn As Integer = index - MyTxtBox.GetFirstCharIndexFromLine(CurrentLine)

            txtLeft = FpC.c.Text.Substring(0, currentColumn)
            txtRight = FpC.c.Text.Substring(currentColumn, FpC.c.Text.Length - currentColumn)
            MyChar = e.KeyChar
            asc_char = Asc(MyChar)
            If asc_char = 8 Then
                If txtLeft.Length > 0 Then
                    txtLeft = txtLeft.Substring(0, txtLeft.Length - 1)
                End If
                MyChar = ""
            End If
            EditedText = txtLeft & MyChar & txtRight
            Select_Panel.Width = GridWidth
            If PanelHeight = 0 Then PanelHeight = 160
            Select_Panel.Height = PanelHeight
            P = GetControlWindowPosition(FP, False, Select_Grid)
            Select_Panel.Location = P

            Select_Panel.Controls.Clear()
            Select_Grid = New DataGridView
            Select_TextBox = New TextBox
            TextBox_END = New TextBox
            Grid_Prepared = False

            Select_TextBox.TabStop = True
            Select_TextBox.TabIndex = 0
            Select_TextBox.Left = 0
            Select_TextBox.Top = 0
            Select_TextBox.Width = Select_Panel.Width
            Select_TextBox.Text = EditedText

            TextBox_END.TabStop = True
            TextBox_END.TabIndex = 1
            TextBox_END.SendToBack()

            With Select_Grid
                .TabStop = False
                .TabIndex = 1000
                .Left = 0
                .Top = Select_TextBox.Height
                .Width = Select_Panel.Width
                .Height = Select_Panel.Height - Select_TextBox.Height
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .AllowUserToOrderColumns = False
                .AllowUserToResizeRows = False
                .RowHeadersVisible = False
            End With
            Select_Panel.Controls.Add(Select_TextBox)
            Select_Panel.Controls.Add(TextBox_END)
            Select_Panel.Controls.Add(Select_Grid)
            Select_Panel.Visible = True

            Select_TextBox.Focus()
            Select_TextBox.Text = EditedText
            Select_TextBox.SelectionStart = Select_TextBox.Text.Length

            If CtrlPressed Then
                Current_Field_Text = SField.Field_Text2
            Else
                Current_Field_Text = SField.Field_Text1
            End If
            Set_TextBox()
            FillGrid()
            If FpC.FP.DATA_IsDataField(FpC.FieldName) Then
                FpC.FP.FORM_DIRTY_SET()
            End If
            Select_Panel.BringToFront()
        End If
    End Sub
    Private Sub MTB_End_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox_END.GotFocus
        ExitWithSave()
    End Sub
    Private Sub MTB_Textbox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Select_TextBox.KeyDown
        If e.Shift Then
            If e.Control Then
                CtrlPressed = Not CtrlPressed
            End If
        End If
        e.Handled = True
        Set_TextBox()
    End Sub
    Private Sub MTB_Textbox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Select_TextBox.KeyUp
        If e.Control Then
            Exit Sub
        End If
        If e.KeyCode = Keys.Tab Then
            ExitWithSave()
            Exit Sub
        End If
        If e.KeyCode = Keys.Enter Then
            ExitWithSave()
            Exit Sub
        End If
        If e.KeyCode = Keys.Escape Then
            ExitWithoutSave()
            FpC.c.Focus()
            FpC.UNDO()
            Exit Sub
        End If
        If e.KeyCode = Keys.Down Then
            Set_Grid_Current_Row(EnumStepDirection.Down)
            Select_Panel.BringToFront()
            Exit Sub
        End If
        If e.KeyCode = Keys.Up Then
            Set_Grid_Current_Row(EnumStepDirection.Up)
            Select_Panel.BringToFront()
            Exit Sub
        End If
        If e.KeyCode = Keys.Home Then
            Set_Grid_Current_Row(EnumStepDirection.Home)
            Select_Panel.BringToFront()
            Exit Sub
        End If
        If e.KeyCode = Keys.End Then
            Set_Grid_Current_Row(EnumStepDirection.End)
            Select_Panel.BringToFront()
            Exit Sub
        End If

        If MAxLength > 0 Then
            If Select_TextBox.Text.Length > MAxLength Then
                Select_TextBox.Text = Select_TextBox.Text.Substring(0, MAxLength)
            End If
        End If
        FillGrid()
        e.Handled = True
        Select_Panel.BringToFront()
    End Sub
    Protected Friend Sub MTB_Panel_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Select_Panel.Leave
        If Not bExitHandled Then
            If Not Select_Grid Is Nothing Then
                ExitWithSave(FpC)
            End If
        End If
    End Sub
    Private Sub MTB_Panel_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Select_Panel.VisibleChanged
        If Select_Panel.Visible Then
            If bExitHandled Then bExitHandled = False
            Select_Panel.BringToFront()
        Else
            If Not bExitHandled Then
                bExitHandled = True
                Select_Panel.Controls.Clear()
                Select_Grid = Nothing
                Select_TextBox = Nothing
                TextBox_END = Nothing
            End If
        End If
    End Sub
    Private Sub MTB_Grid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles Select_Grid.CellDoubleClick
        Dim SelectedText As String
        If Select_Grid.CurrentCell Is Nothing Then
            Select_Grid.CurrentCell = Select_Grid(SField.Selected_ID, 0)
        End If
        SelectedText = Select_Grid(SField.Field_Text1, Select_Grid.CurrentCell.RowIndex).Value
        Select_TextBox.Text = SelectedText

        ExitWithSave(FpC)
    End Sub
    Private Sub MTB_Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Select_Grid.CellEnter
        If CType(sender, DataGridView).Focused Then
            Select_Panel.BringToFront()
        End If
    End Sub
    Private Sub MTB_Grid_KeyUp(sender As Object, e As KeyEventArgs) Handles Select_Grid.KeyUp
        If e.KeyCode = Keys.Escape Then
            ExitWithoutSave()
            FpC.c.Focus()
            FpC.UNDO()
            Exit Sub
        End If
    End Sub
    Private Sub Reset_Where()
        With SSQL
            Current_Field_Text = SField.Field_Text1
            Current_SQL_From = SSQL.SQL_From1
            Current_SQL_OrderBy = SSQL.OrderBy1
        End With
    End Sub
    'Private Sub FpC_Field_BeforeUpdate(sender_FPc As FP_Control, ByRef Cancel As Integer) Handles FpC.Field_BeforeUpdate

    'End Sub
End Class
