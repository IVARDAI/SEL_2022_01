Imports System.Data
Imports System.Data.SqlClient

Public Class FP_GRIDVIEW
    Public FP As FP
    Public WithEvents GRID As DataGridView
    Public GRID_OrigHeight As Integer
    Public Footer_Panel As Panel
    Public WithEvents GRID_Label As Label
    Private RecordSelector As Panel
    Public WithEvents Btn_FooterVisible As FP_PictureBox
    Private WithEvents EditingIcon As PictureBox
    Public COLUMNS_Frozen As Integer
    Public WithEvents FPc_FilterIcon As FP_PictureBox

    Protected Friend Filter_Panel As Panel
    Private Filter_Selector As Panel
    Private FilterIcon As PictureBox
    Private Disposed As Boolean = False

    Public DT_ALL_FIELDS As New DataTable
    Public DT As New DataTable

    Private WithEvents GRID_Panel As Panel = Nothing

    Private HeaderHeight As Integer = 22
    Private RowHeight As Integer = 22

    Private Const FILTERFIELD_IDENTIFIER As String = "_GRID_FILTER_"
    Private DIC_FILTERFIELDS As New Dictionary(Of String, FP_Control)
    Private FPc_EditIcon As FP_PictureBox

    Sub New(ByVal MyFP As FP, ByVal MyControls As Struct_FP_GRID_CONTROL_COLLECTION)
        Dim col_St As New DataGridViewColumn

        FP = MyFP

        HeaderHeight = 22 * FP.FPf.P_Layout_DPI_Factor_Y 'FP.FPf.FPApp.P_Layout_TextBox_NormalHeight
        RowHeight = FP.FPf.P_Layout_TextBox_NormalHeight

        If FP.SQL_BIND_Params.NameOf_GRID = "" Then
            With FP.SQL_BIND_Params
                .NameOf_GRID = FP.ServerObject_Prefix + "_GRID"
                FP.FPf.FPApp.RS_GET_FIELDPROPERTIES_FROM_SERVER_OBJECT(.NameOf_GRID, FP.DATA_DT_FORM_SQLFIELDS_FOR_GRID, FP.SQL_BIND_Params.TypeOf_GRID)
            End With
        End If

        With MyControls
            GRID = .GRID
            If Not (GRID Is Nothing) Then
                GRID.AllowUserToResizeRows = False
                GRID.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
                GRID.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
                GRID.ColumnHeadersHeight = HeaderHeight
                GRID_AllowAdditions_SET(FP.P_FORM_AllowAdditions)
            End If

            Dim CellStyle As New DataGridViewCellStyle
            CellStyle.Font = New System.Drawing.Font("Tahoma", 9)
            GRID.DefaultCellStyle = CellStyle

            GRID_OrigHeight = GRID.Height

            Footer_Panel = .Footer_Panel

            If Not (.Btn_FooterVisible Is Nothing) Then
                Dim FPc As FP_PictureBox = FP.PICTUREBOXES_GET(.Btn_FooterVisible.Name)

                If FPc Is Nothing Then
                    FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.New", 0, "A Btn_FooterVisible PICTUREBOX-nak a megadott FP-hez kell tartoznia, maskepp nem fog mukodni.")
                ElseIf (Footer_Panel Is Nothing) Then
                    FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.New", 0, "Nincs megadva a Footer_Panel.")
                Else
                    If Not FPc.ToggleButton Then
                        FPc.ToggleButton = True
                    End If

                    Btn_FooterVisible = FPc
                End If
            End If

            GRID_Label = .Label
        End With

        GRID_PANEL_INIT()
    End Sub

    Protected Friend Sub GRID_OrigHeight_INIT()
        If GRID Is Nothing Then
            GRID_OrigHeight = 0
        Else
            If Footer_Panel Is Nothing Then
                GRID_OrigHeight = GRID.Height
            Else
                GRID_OrigHeight = Footer_Panel.Top - 1 - GRID.Top
            End If
        End If
    End Sub

    Public Sub SAVE_Field_Length()
        For Each Key As String In FP.CONTROLS.Keys
            Dim FPc As FP_Control = FP.CONTROLS(Key)

            If FPc.P.ShowInGRID Then
                If Not FPc.c Is Nothing Then
                    Dim Pfd_Key As String = FP.FPf.FPApp.PFD_Key_SIZE_FIX_SAVE(FP.ServerObject_Prefix, FP.SubPrefix, FPc.c.Name)

                    FP.FPf.FPApp.PFDinsertOrUpdate(Pfd_Key, FPc.c.Width.ToString)
                End If
            End If
        Next
    End Sub


    Public Sub Dispose()
        If Not (FPc_EditIcon Is Nothing) Then
            FPc_EditIcon.Dispose()
            FPc_EditIcon = Nothing
        End If

        If Not (EditingIcon Is Nothing) Then
            CONTROL_DISPOSE(EditingIcon)
            EditingIcon = Nothing
        End If

        If Not (FPc_FilterIcon Is Nothing) Then
            FPc_FilterIcon.Dispose()
            FPc_FilterIcon = Nothing
        End If

        If Not (FilterIcon Is Nothing) Then
            CONTROL_DISPOSE(FilterIcon)
            FilterIcon = Nothing
        End If

        If Not (RecordSelector Is Nothing) Then
            CONTROL_DISPOSE(RecordSelector)
            RecordSelector = Nothing
        End If

        If Not (Filter_Selector Is Nothing) Then
            CONTROL_DISPOSE(Filter_Selector)
            Filter_Selector = Nothing
        End If

        If Not (Filter_Panel Is Nothing) Then
            CONTROL_DISPOSE(Filter_Panel)
            Filter_Panel = Nothing
        End If

        If Not (GRID_Panel Is Nothing) Then
            CONTROL_DISPOSE(GRID_Panel)
            GRID_Panel = Nothing
        End If

        GRID = Nothing
        Footer_Panel = Nothing
        GRID_Label = Nothing
        Btn_FooterVisible = Nothing
        EditingIcon = Nothing

        Disposed = True
    End Sub

    Protected Friend Sub FILTER_FIELDS_SET_FILTER_TEXT(ByVal NameOfField As String, ByVal FilterText As String)
        Dim FilterFieldName As String = CONTROLS_FILTERFIELDS_GET_NAME(NameOfField)

        If FP.FPf.CONTROLS.Keys.Contains(FilterFieldName) Then
            Dim c As Control = FP.FPf.CONTROLS(FilterFieldName)

            c.Text = FilterText
        End If
    End Sub

    Protected Friend Sub COLUMNS_Frozen_ACTIVATE()
        If Not (GRID Is Nothing) Then
            If Not (GRID.Parent Is Nothing) Then
                If COLUMNS_Frozen > 0 Then
                    Dim DoIt As Boolean = True
                    Dim MyTabControl As TabControl = Nothing
                    Dim MyTabPage As TabPage = Nothing

                    If CONTROLS_Is_c_On_TabControl(GRID, MyTabControl, MyTabPage) Then
                        If MyTabControl Is Nothing Then
                            DoIt = False
                        Else
                            If Not MyTabControl.SelectedTab.Equals(MyTabPage) Then
                                COLUMNS_Frozen_DEACTIVATE()
                                DoIt = False
                            End If
                        End If
                    End If

                    If DoIt Then
                        Dim i As Integer = 0

                        Do While i < GRID.Columns.Count - 1 And i <= COLUMNS_Frozen
                            With GRID.Columns(i)
                                If .Visible Then
                                    If Not .Frozen Then
                                        .Frozen = True
                                    End If
                                End If
                            End With

                            i += 1
                        Loop
                    End If
                End If
            End If
        End If
    End Sub

    Protected Friend Sub COLUMNS_Frozen_DEACTIVATE()
        For Each col In GRID.Columns
            If col.Frozen Then
                col.Frozen = False
            End If
        Next
    End Sub

    Public Property P_FilterActive() As Boolean
        Get
            P_FilterActive = FPc_FilterIcon.P_Pressed
        End Get
        Set(ByVal value As Boolean)
            If value <> FPc_FilterIcon.P_Pressed Then
                FPc_FilterIcon.P_Pressed = value
                FPc_FilterIcon_CLICK()
            End If
        End Set
    End Property

    Public WriteOnly Property P_VISIBLE() As Boolean
        Set(ByVal value As Boolean)
            FIELD_VISIBLE(GRID, value)
            FIELD_VISIBLE(GRID_Label, value)
            FIELD_VISIBLE(GRID_Panel, value)
            If Not (Btn_FooterVisible Is Nothing) Then
                FIELD_VISIBLE(Btn_FooterVisible.c, value)
            End If
            FIELD_VISIBLE(Filter_Selector, value)

            If value = False Then
                FIELD_VISIBLE(Filter_Panel, False)
                FIELD_VISIBLE(Footer_Panel, False)

            Else
                If Btn_FooterVisible Is Nothing Then
                    FIELD_VISIBLE(Footer_Panel, True)
                Else
                    FIELD_VISIBLE(Footer_Panel, Btn_FooterVisible.P_Pressed)
                End If
                FIELD_VISIBLE(Filter_Panel, FPc_FilterIcon.P_Pressed)
                MOVE_GRID_PANEL_ON_ROW()
            End If
        End Set
    End Property

    Public Property P_Width() As Integer
        Get
            Dim OUT As Integer = 0

            If Not (GRID Is Nothing) Then
                OUT = GRID.Width
            End If

            P_Width = OUT
        End Get
        Set(ByVal value As Integer)
            If Not (GRID Is Nothing) Then
                Dim Btn_FooterVisible_Width = 0

                If Not (Btn_FooterVisible Is Nothing) Then
                    Btn_FooterVisible_Width = Btn_FooterVisible.c.Width
                End If

                Dim MinWidht As Integer = Btn_FooterVisible_Width

                If value >= MinWidht Then
                    GRID.Width = value
                    If Not (Footer_Panel Is Nothing) Then
                        Footer_Panel.Width = GRID.Width
                    End If

                    If Not (GRID_Label Is Nothing) Then
                        GRID_Label.Width = GRID.Width - Btn_FooterVisible_Width
                    End If

                    If Not (Btn_FooterVisible Is Nothing) Then
                        Btn_FooterVisible.c.Left = GRID.Width - Btn_FooterVisible_Width
                    End If
                End If
            End If
        End Set
    End Property

    Public Property P_Height() As Integer
        Get
            Dim OUT As Integer = 0

            If Not (GRID Is Nothing) Then
                Dim GRID_Top As Integer = P_Top
                Dim GRID_Bottom As Integer = 0

                If Footer_Panel Is Nothing Then
                    GRID_Bottom = GRID.Top + GRID.Height
                Else
                    GRID_Bottom = Footer_Panel.Top + Footer_Panel.Height
                End If

                OUT = GRID_Bottom - GRID_Top
            End If

            P_Height = OUT
        End Get

        Set(ByVal value As Integer)
            If Not (GRID Is Nothing) Then
                Dim GRID_Label_Height As Integer = 0
                Dim GRID_Footer_Height As Integer = 0

                If GRID_Label Is Nothing Then
                    If Btn_FooterVisible Is Nothing Then
                        GRID_Label_Height = 0
                    Else
                        GRID_Label_Height = Btn_FooterVisible.c.Height
                    End If
                Else
                    GRID_Label_Height = GRID_Label.Height
                End If

                If Footer_Panel Is Nothing Then
                    GRID_Footer_Height = 0
                Else
                    GRID_Footer_Height = Footer_Panel.Height
                End If

                Dim MinValue As Integer = GRID_Label_Height + GRID_Footer_Height

                If value > MinValue Then
                    GRID_OrigHeight = value - GRID_Label_Height - GRID_Footer_Height
                    If Footer_Panel Is Nothing Then
                        GRID.Height = GRID_OrigHeight
                    Else
                        Footer_Panel.Top = GRID.Top + GRID_OrigHeight
                        If Footer_Panel.Visible Then
                            FOOTER_SHOW()
                        Else
                            FOOTER_HIDE()
                        End If
                    End If
                End If
            End If
        End Set
    End Property

    Public Property P_Left() As Integer
        Get
            Dim OUT As Integer = 0

            If Not (GRID Is Nothing) Then
                OUT = GRID.Left
            End If

            P_Left = OUT
        End Get
        Set(ByVal value As Integer)
            If Not GRID Is Nothing Then
                GRID.Left = value
                If Not (GRID_Label Is Nothing) Then
                    GRID_Label.Left = value
                End If
                If Not (Btn_FooterVisible Is Nothing) Then
                    Btn_FooterVisible.c.Left = GRID.Left + GRID.Width - Btn_FooterVisible.c.Width
                End If
                If Not (Footer_Panel Is Nothing) Then
                    Footer_Panel.Left = value
                End If
            End If
        End Set
    End Property

    Public Property P_Top() As Integer
        Get
            Dim OUT As Integer = 0

            If Not (GRID Is Nothing) Then
                If GRID_Label Is Nothing Then
                    If Btn_FooterVisible Is Nothing Then
                        OUT = GRID.Top
                    Else
                        OUT = Btn_FooterVisible.c.Top
                    End If
                Else
                    OUT = GRID_Label.Top
                End If
            End If

            P_Top = OUT
        End Get
        Set(ByVal value As Integer)

        End Set
    End Property

    Private Function get_CELL_Rect(ByVal Cell As DataGridViewCell, Optional ByVal cutOverflow As Boolean = False) As Rectangle
        get_CELL_Rect = GRID.GetCellDisplayRectangle(Cell.ColumnIndex, Cell.RowIndex, cutOverflow)
    End Function

    Protected Friend Sub GRID_AllowAdditions_SET(ByVal AllowAdditions As Boolean)
        If (Not GRID Is Nothing) Then
            If GRID.AllowUserToAddRows <> AllowAdditions Then
                GRID.AllowUserToAddRows = AllowAdditions
            End If
        End If
    End Sub

    Public Function ROW_VISIBLE(ByVal RowIndex As Integer) As Boolean
        Dim OUT As Boolean = False

        Dim RowsVisibleFROM As Integer = GRID.FirstDisplayedScrollingRowIndex
        Dim RowsVisibleTO As Integer = RowsVisibleFROM + GRID.DisplayedRowCount(False) - 1

        If RowIndex >= RowsVisibleFROM And RowIndex <= RowsVisibleTO Then
            OUT = True
        End If

        ROW_VISIBLE = OUT
    End Function

    Public Sub ROW_ENSURE_VISIBLE(ByVal RowIndex As Integer, Optional ByVal AsLastRow_InWindow As Boolean = False)
        If RowIndex >= 0 And RowIndex <= GRID.RowCount Then
            If Not ROW_VISIBLE(RowIndex) Then
                If Not AsLastRow_InWindow Then
                    GRID.FirstDisplayedScrollingRowIndex = RowIndex
                Else
                    Dim RowsVisibleFROM As Integer = GRID.FirstDisplayedScrollingRowIndex
                    Dim RowsVisibleTO As Integer = RowsVisibleFROM + GRID.DisplayedRowCount(False) - 1

                    If RowsVisibleFROM >= 0 And RowsVisibleTO >= 0 Then
                        If RowsVisibleTO > GRID.RowCount Then
                            GRID.FirstDisplayedScrollingRowIndex = 1
                        Else
                            Dim FirstRow As Integer = RowIndex - (RowsVisibleTO - RowsVisibleFROM)

                            If FirstRow < 0 Then
                                FirstRow = 0
                            End If
                            GRID.FirstDisplayedScrollingRowIndex = FirstRow
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Protected Friend Function SQL_GET(ByVal RS_ID As Long, ByVal SubWHERE As String, Only_Fields_In_GRID As Boolean) As String
        Dim OUT As String = ""
        Dim Field_List As String = SQL_FIELD_LIST(False, Only_Fields_In_GRID)
        Dim Comma As String = IIf(Field_List > "", ",", "")

        Select Case FP.SQL_BIND_Params.TypeOf_GRID
            Case ENUM_ServerObject_Type.V
                If SubWHERE > "" Then
                    OUT = String.Format("SELECT RecordID, SeqNum{0} {1} FROM {2} WHERE RS_ID = {3} And ({4}) ORDER BY SeqNum", Comma, Field_List, FP.SQL_BIND_Params.NameOf_GRID, FP.RS_ID, SubWHERE)
                Else
                    OUT = String.Format("SELECT RecordID, SeqNum{0} {1} FROM {2} WHERE RS_ID = {3} ORDER BY SeqNum", Comma, Field_List, FP.SQL_BIND_Params.NameOf_GRID, FP.RS_ID)
                End If

            Case ENUM_ServerObject_Type.TF
                If SubWHERE > "" Then
                    OUT = String.Format("SELECT RecordID, SeqNum{0} {1} FROM dbo.{2}({3}) WHERE {4} ORDER BY SeqNum", Comma, Field_List, FP.SQL_BIND_Params.NameOf_GRID, FP.RS_ID, SubWHERE)
                Else
                    OUT = String.Format("SELECT RecordID, SeqNum{0} {1} FROM dbo.{2}({3}) ORDER BY SeqNum", Comma, Field_List, FP.SQL_BIND_Params.NameOf_GRID, FP.RS_ID)
                End If

            Case ENUM_ServerObject_Type.P
                '+++
                FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.SQL_GET", 0, "Ezt a funkciot meg ki kell dolgozni!!!")

            Case Else
                FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.GET", 0, "Unknown SQL ObjectType for GRID")
        End Select

        Return OUT
    End Function

    Protected Friend Sub GRID_PANEL_HIDE()
        If Not (GRID_Panel Is Nothing) Then
            If GRID_Panel.Height <> 0 Then
                GRID_Panel.Height = 0
                GRID_Panel.Top = GRID.Top
            End If
        End If
    End Sub

    Protected Friend Sub GRID_PANEL_SHOW()
        If Not (GRID Is Nothing) Then
            If GRID.Visible Then
                If Not (GRID_Panel Is Nothing) Then
                    If GRID_Panel.Height <> RowHeight Then
                        GRID_Panel.Height = RowHeight
                    End If
                End If
            End If
        End If
    End Sub

    Protected Friend Sub MOVE_GRID_PANEL_ON_ROW()
        Dim i As Integer

        If Not ROW_CURRENT_ROWINDEX(i) Then
            GRID_PANEL_HIDE()
        Else
            If Not ROW_VISIBLE(i) Then
                GRID_PANEL_HIDE()
            Else
                'Dim Y As Integer = HeaderHeight + (i - GRID.FirstDisplayedScrollingRowIndex) * RowHeight + 1
                Dim Y As Integer = HeaderHeight + (i - GRID.FirstDisplayedScrollingRowIndex) * GRID.Rows(0).Height + 1

                GRID_PANEL_SHOW()
                If GRID_Panel.Top <> GRID.Top + Y Then
                    GRID_Panel.Top = GRID.Top + Y
                End If
            End If
        End If
    End Sub

    Public Function COLUMNS_ALL_WITH(Optional ByVal WithScrollBar_With As Boolean = False) As Integer
        Dim OUT As Integer = 0

        COLUMNS_WITH_SET()

        If Not (GRID Is Nothing) Then
            OUT = GRID.RowHeadersWidth
            Dim w As Integer

            For w = 0 To GRID.Columns.Count - 1
                With GRID.Columns(w)
                    If .Visible Then
                        OUT += .Width
                    End If
                End With
            Next
        End If

        If WithScrollBar_With Then
            'If SCROLLBAR_V_Visible() Then
            OUT += FP.FPf.P_Layout_Form_HorizontalScrollBar_With   'Vertical Scrollbar Width
            'End If
        End If

        COLUMNS_ALL_WITH = OUT
    End Function

    Public Function SCROLLBAR_V_Visible() As Boolean
        Dim OUT As Boolean = False

        If GRID.RowCount > 0 Then
            'SCROLLBAR_V_Visible = (GRID.RowCount * RowHeight > GRID.DisplayRectangle.Height)
            SCROLLBAR_V_Visible = (GRID.RowCount * GRID.Rows(0).Height > GRID.DisplayRectangle.Height)
        End If

        SCROLLBAR_V_Visible = OUT
    End Function

    Public Function SET_WITH(ByVal MyWith As Integer) As Boolean
        Dim OUT As Boolean = False

        If Not GRID Is Nothing Then
            GRID.Width = MyWith
            If Not (Footer_Panel Is Nothing) Then
                Footer_Panel.Width = MyWith
            End If

            Dim Btn_FooterVisible_Width As Integer = 0

            If Not (Btn_FooterVisible Is Nothing) Then
                If Not Btn_FooterVisible.c Is Nothing Then
                    Btn_FooterVisible_Width = Btn_FooterVisible.c.Width
                    Btn_FooterVisible.c.Left = GRID.Left + MyWith - Btn_FooterVisible_Width
                End If
            End If

            If Not (GRID_Label Is Nothing) Then
                GRID_Label.Width = GRID.Width - Btn_FooterVisible_Width
            End If

            OUT = True
        End If

        SET_WITH = OUT
    End Function

#Region "FOOTER"
    Public Sub FOOTER_SHOW()
        If Not (GRID Is Nothing) Then
            If Not (Footer_Panel Is Nothing) Then
                If Not (Btn_FooterVisible Is Nothing) Then
                    Btn_FooterVisible.P_Pressed = True
                End If

                GRID.Height = GRID_OrigHeight
                FIELD_VISIBLE(Footer_Panel, True)
                If Footer_Panel.Visible Then
                    FP.FPf.CONTROLS_ARRANGE_ALL()
                End If
                MOVE_GRID_PANEL_ON_ROW()

                Dim AktRowIndex As Integer
                If ROW_CURRENT_ROWINDEX(AktRowIndex) Then

                    ROW_ENSURE_VISIBLE(AktRowIndex, True)

                    GRID_PANEL_ARRANGE(GRID(0, AktRowIndex))
                Else
                    If FPc_FilterIcon.P_Pressed Then
                        FILTER_PANEL_ARRANGE()
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub FOOTER_HIDE()
        If Not (GRID Is Nothing) Then
            If Not (Footer_Panel Is Nothing) Then
                If Not (Btn_FooterVisible Is Nothing) Then
                    Btn_FooterVisible.P_Pressed = False
                End If

                FIELD_VISIBLE(Footer_Panel, False)
                GRID.Height = Footer_Panel.Top + Footer_Panel.Height - GRID.Top
                MOVE_GRID_PANEL_ON_ROW()
            End If
        End If
    End Sub
    Private Sub EVENT_BTN_FOOTERVISIBLE_MOUSEUP() Handles Btn_FooterVisible.CLICK
        If Btn_FooterVisible.P_Pressed Then
            FOOTER_SHOW()
        Else
            FOOTER_HIDE()
        End If
    End Sub
#End Region

    Public Sub DIRTY_SET()
        RecordSelector.Controls(EditingIcon.Name).Visible = True
        RecordSelector.Controls(EditingIcon.Name).BringToFront()
    End Sub

    Public Sub DIRTY_CLEAR()
        Try
            '+++ Ghibli-ben a Task management orokke itt szallt el. Ki kell vizsgalni, miert.
            RecordSelector.Controls(EditingIcon.Name).Visible = False

        Catch ex As Exception
            'Nothing to do
        End Try
    End Sub
    Protected Friend Function SQL_FIELD_LIST(ByVal HeaderNames_As_Columns As Boolean, Only_Fields_In_GRIDVIEW As Boolean) As String
        Dim OUT As String = ""

        If GRID_Panel Is Nothing Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.SQL_FIELD_LIST", 0, "GRID_Panel Is Nothing")
        Else
            Dim DIC_Columns As New Dictionary(Of Integer, String)
            Dim ListOfColumns As New List(Of String)

            For i As Integer = 0 To GRID_Panel.Controls.Count - 1
                With GRID_Panel.Controls(i)
                    If Trim(.Name) > "" Then

                        If Left(.Name, 1) <> "#" Then
                            If Not DIC_Columns.ContainsKey(.TabIndex) Then
                                DIC_Columns.Add(.TabIndex, .Name)
                                ListOfColumns.Add(.Name)
                            End If
                        End If
                    End If
                End With
            Next

            Dim L_TabOrder As List(Of Integer) = DIC_Columns.Keys.ToList

            L_TabOrder.Sort()

            Dim CommaStr = ""

            For Each AktTabOrder In L_TabOrder
                Dim AktColumnName As String = DIC_Columns(AktTabOrder)
                Dim AktFieldName As String = FP.FieldName_Get_From_ControlName(AktColumnName)

                If HeaderNames_As_Columns Then
                    OUT += CommaStr + String.Format("[{0}] '{1}'", FP.FieldName_Get_From_ControlName(AktColumnName), GRID.Columns(AktColumnName).HeaderText)
                    'OUT += CommaStr + String.Format("[{0}] '{1}'", AktFieldName, AktFieldName)
                Else
                    Dim FieldName As String = FP.FieldName_Get_From_ControlName(AktColumnName)

                    If Strings.Left(FieldName, 1) <> "[" Then
                        FieldName = "[" + FieldName + "]"
                    End If

                    OUT += CommaStr + String.Format("{0}", FieldName)
                End If

                CommaStr = ", "
            Next

            If Only_Fields_In_GRIDVIEW = False Then
                For i As Integer = 0 To FP.DATA_DT_FORM_SQLFIELDS_FOR_GRID.Rows.Count - 1
                    With FP.DATA_DT_FORM_SQLFIELDS_FOR_GRID.Rows(i)
                        Dim FieldName As String = !FieldName

                        Select Case FieldName
                            Case "RecordID", "SeqNum"
                                'Nothing to do

                            Case Else
                                If Not ListOfColumns.Contains(FieldName) Then
                                    If Strings.Left(FieldName, 1) <> "[" Then
                                        FieldName = "[" + FieldName + "]"
                                    End If

                                    OUT += CommaStr + FieldName
                                    CommaStr = ", "
                                End If
                        End Select
                    End With
                Next
            End If
        End If

        SQL_FIELD_LIST = OUT
    End Function

    Function CONTROLS_FILTERFIELDS_GET_NAME(ByVal FieldName As String) As String
        CONTROLS_FILTERFIELDS_GET_NAME = Control_Prefix() + FILTERFIELD_IDENTIFIER + FieldName
    End Function

    Function CONTROLS_FILTERFIELDS_GET_FILTERED_DATAFIELD_NAME(ByVal FilterField_Name As String) As String
        Dim OUT As String = Nothing

        Dim p As Integer = InStr(FilterField_Name, FILTERFIELD_IDENTIFIER)

        If p > 0 Then
            OUT = Mid(FilterField_Name, p + Len(FILTERFIELD_IDENTIFIER))
        End If

        CONTROLS_FILTERFIELDS_GET_FILTERED_DATAFIELD_NAME = OUT
    End Function

    Public Function DT_Clone_AllRows() As DataTable
        'Az esetleg torolt sorokat is visszaadja, nem csak a szurteket..
        Dim OUT As DataTable = Nothing

        If Not (DT Is Nothing) Then
            OUT = DT.Copy
            OUT.RejectChanges()
        End If

        DT_Clone_AllRows = OUT
    End Function

    Private Sub FILTER_PANEL_REMOVE_ALL_CONTROLS()
        If Filter_Panel Is Nothing Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.FILTER_PANEL_REMOVE_ALL_CONTROLS", 0, "Filter_Panel is nothing.")
        Else
            Dim L As List(Of String)
            Dim AktKey As String

            L = DIC_FILTERFIELDS.Keys.ToList
            For Each AktKey In L
                FP.CONTROLS_REMOVE(AktKey)
            Next
            DIC_FILTERFIELDS.Clear()

            If Filter_Panel.Controls.Count > 0 Then
                Dim DIC As Dictionary(Of String, Control) = Nothing

                CONTROLS_WRITE_ALL_CONTROLS_TO_DIC(Filter_Panel, DIC)
                For Each AktKey In DIC.Keys
                    If Left(AktKey, 10) = "#FP_SETUP_" Then
                        DIC(AktKey).Parent = Filter_Panel.Parent
                    End If
                Next

                If Filter_Panel.Controls.Count > 0 Then
                    FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.FILTER_PANEL_REMOVE_ALL_CONTROLS", 0, "Nem sikerult eltavolitani az osszes controlt a Filter_Panel-rol.")
                End If
            End If

            FP.FPf.CONTROLS_REMOVE(Filter_Panel.Name)
            Filter_Panel.Dispose()
        End If
    End Sub

    Protected Friend Function FILTER_PANEL_INIT() As Boolean
        Dim OUT As Boolean = True

        If Filter_Panel.Controls.Count = 0 Then
            Dim i As Integer
            Dim FPc As FP_Control

            For i = 0 To GRID.ColumnCount - 1
                With GRID.Columns(i)
                    If .Visible Then
                        Dim c As Control

                        If GRID.Columns(i).ValueType.ToString = "System.Boolean" Then
                            c = New CheckBox

                            With CType(c, CheckBox)
                                .ThreeState = True
                                .CheckState = CheckState.Indeterminate
                                .CheckAlign = ContentAlignment.MiddleCenter
                            End With
                        Else
                            c = New TextBox

                            c.ForeColor = COLORS_GRID_FILTER_FORE
                            c.Font = Font_NORMAL
                        End If

                        c.Name = CONTROLS_FILTERFIELDS_GET_NAME(GRID.Columns(i).Name)
                        c.Parent = Filter_Panel
                        c.TabStop = False
                        c.TabIndex = Filter_Panel.TabIndex
                        c.Visible = True
                        FP.FPf.CONTROLS_ADD(c)

                        FPc = New FP_Control(FP, c, Nothing, True, CONTROLS_FILTERFIELDS_GET_NAME(GRID.Columns(i).Name), "")
                        FPc.CreatedBy = Enum_FP_CONTROL_Created_by.GRID

                        FPc.P.COLOR_NORMAL_BG = COLORS_GRID_FILTER_BG

                        AddHandler FPc.c.TextChanged, AddressOf FILTER_FIELDS_TEXTCHANGED
                        AddHandler FPc.c.MouseClick, AddressOf FILTER_FIELDS_MOUSECLICK
                        AddHandler FPc.c.KeyPress, AddressOf FILTER_FIELDS_KEYPRESS
                        If Not (FPc.c_ChkBox Is Nothing) Then
                            AddHandler FPc.c_ChkBox.CheckStateChanged, AddressOf FILTER_FIELDS_CHECKSTATECHANGED
                        End If
                        DIC_FILTERFIELDS.Add(FPc.FieldName, FPc)
                    End If
                End With
            Next

            P_FilterActive = FP.FPf.FPApp.P.Layout_Params.GRID_FILTER_Button_Pressed
        End If

        FILTER_PANEL_INIT = OUT
    End Function

    Sub FILTER_FIELDS_KEYPRESS(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = "%" Then
            e.Handled = True
        End If
    End Sub

    Sub FILTER_FIELDS_TEXTCHANGED(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim FPc As FP_Control = FP.CONTROLS(CType(sender, Control).Name)
        Dim OldValue As String = FPc.OldValue
        Dim AktValue As String = FPc.c.Text

        If Mid(AktValue, 1, Len(OldValue)) <> OldValue Or Len(AktValue) < Len(OldValue) Then
            FILTER_FIELDS_FILTER_DEACTIVATE()
        End If

        FILTER_FIELDS_FILTER_ACTIVATE()

        FPc.EVENT_GOTFOCUS()
    End Sub

    Sub FILTER_FIELDS_CHECKSTATECHANGED(ByVal sender As Object, ByVal e As System.EventArgs)
        FILTER_FIELDS_FILTER_DEACTIVATE()
        FILTER_FIELDS_FILTER_ACTIVATE()
    End Sub

    Sub FILTER_FIELDS_MOUSECLICK(ByVal sender As Object, ByVal e As System.EventArgs)
        FP.FORM_RECORDS_SAVE_CURRENT()
        'FP.FORM_GOTO_NORECORD()
    End Sub

    Protected Friend Function FILTER_FIELDS_IS_FILTERFIELD(ByVal FPc As FP_Control) As Boolean
        Dim OUT As Boolean = False

        If FPc_HAS_FIELD(FPc) Then
            If CONTROLS_FILTERFIELDS_GET_FILTERED_DATAFIELD_NAME(FPc.FieldName) > "" Then
                OUT = True
            End If
        End If

        FILTER_FIELDS_IS_FILTERFIELD = OUT
    End Function

    Private Function Filter_Text_get_Filter_Text(FieldName As String, Filter_OrigText As String) As String
        'Mert azok a bizonyos magyar kettosbetuk... ba.. ku.. @!%+"'
        Dim OUT As String = ""

        If Len(Filter_OrigText) > 0 Then
            Filter_OrigText = Replace(Filter_OrigText, "'", "''")
            Dim WHERE_Str As String = "isnull(CONVERT({0}, 'System.String'), '') NOT like '%{1}{2}%'"
            Dim WHERE_Str_2 As String = "isnull(CONVERT({0}, 'System.String'), '') NOT like '%{2}{1}%'"
            OUT = String.Format(WHERE_Str, FieldName, Filter_OrigText, "")

            Dim RightChar As String = Mid(Filter_OrigText, Len(Filter_OrigText), 1)

            Select Case RightChar
                Case "c", "C"
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "s")
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "cs")

                Case "d", "D"
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "z")
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "dz")
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "zs")

                Case "g", "G"
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "y")
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "gy")

                Case "l", "L"
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "y")
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "ly")

                Case "n", "N"
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "y")
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "ny")

                Case "s", "S"
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "z")
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "sz")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "z")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "zz")
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "dz")
                    If Filter_OrigText.ToUpper = "ZS" Then
                        OUT += " And " + String.Format("isnull(CONVERT({0}, 'System.String'), '') NOT like '%dzs%'", FieldName)
                    End If

                Case "t", "T"
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "y")
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "ty")

                Case "z", "Z"
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "s")
                    OUT += " And " + String.Format(WHERE_Str, FieldName, Filter_OrigText, "zs")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "s")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "ss")

                    OUT += " And " + String.Format("isnull(CONVERT({0}, 'System.String'), '') NOT like '%dzs%'", FieldName)

                Case "y", "Y"
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "g")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "gg")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "l")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "ll")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "n")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "nn")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "t")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "tt")

                Case "s", "S"
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "z")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "zz")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "dz")

                Case "z", "Z"
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "s")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "ss")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "d")
                    OUT += " And " + String.Format(WHERE_Str_2, FieldName, Filter_OrigText, "dd")
            End Select
        End If

        Return Replace(OUT, "%%", "%")
    End Function

    Protected Friend Function FILTER_FIELDS_FILTER_ACTIVATE() As Boolean
        Dim OUT As Boolean = True

        If FPc_FilterIcon.P_PRESSED Then
            Dim Let_Goto_NewRecord As Boolean = False

            If FP.P_DATA_RecordStatus = ENUM_RecordStatus.NEWRECORD Then
                Let_Goto_NewRecord = True
                FP.FORM_GOTO_NORECORD()
            End If

            If Not DT Is Nothing Then
                Dim i As Integer

                For i = 0 To Filter_Panel.Controls.Count - 1
                    Dim FilterField_Name As String = Filter_Panel.Controls(i).Name
                    Dim FieldName As String = CONTROLS_FILTERFIELDS_GET_FILTERED_DATAFIELD_NAME(FilterField_Name)
                    If FilterField_Name > "" And FieldName > "" Then
                        Dim Criteria As String = ""

                        If TypeOf (Filter_Panel.Controls(i)) Is CheckBox Then
                            With CType(Filter_Panel.Controls(i), CheckBox)
                                If Not .CheckState = CheckState.Indeterminate Then
                                    If .Checked Then
                                        Criteria = String.Format("{0} = False", FieldName)
                                    Else
                                        Criteria = String.Format("{0} = True", FieldName)
                                    End If
                                End If
                            End With
                        Else
                            Dim Filter_Text As String = Filter_Panel.Controls(i).Text
                            If Filter_Text > "" Then

                                'Filter_Text = String.Format("%{0}%", Filter_Text)
                                'Criteria = Replace(String.Format("isnull(CONVERT({0}, 'System.String'), '') NOT like '{1}%'", FieldName, Filter_Text), "%%", "%")
                                Criteria = Filter_Text_get_Filter_Text(FieldName, Filter_Text)
                            End If
                        End If

                        If Criteria > "" Then
                            Dim Let_Goto_NoRecord As Boolean = False

                            Dim A_Rows() As DataRow = DT.Select(Criteria).ToArray
                            Dim A_Rows_ALL() As DataRow = DT_ALL_FIELDS.Select(Criteria).ToArray

                            Dim iR As Integer = A_Rows.Count
                            Do While iR > 0
                                iR -= 1
                                If Not (A_Rows(iR) Is Nothing) Then
                                    If FP.P_DATA_Current_ID = nz(A_Rows(iR)!RecordID, 0) Then
                                        Let_Goto_NoRecord = True
                                    End If

                                    A_Rows(iR).Delete()
                                    A_Rows_ALL(iR).Delete()
                                End If
                            Loop

                            If Let_Goto_NoRecord Then
                                FP.FORM_GOTO_NORECORD()
                            End If
                        End If
                    End If
                Next

                MOVE_GRID_PANEL_ON_ROW()
            End If

            If Let_Goto_NewRecord Then
                FP.FORM_GOTO_NEWRECORD()
            End If
        End If

        If OUT Then
            FP.RAISEEVENT_GRID_Filter_Changed()
        End If

        FILTER_FIELDS_FILTER_ACTIVATE = OUT
    End Function

    Private Function FILTER_FIELDS_FILTER_DEACTIVATE() As Boolean
        Dim OUT As Boolean = True
        Dim Let_Goto_NewRecord As Boolean = False
        Dim LastSortedColumn As DataGridViewColumn = GRID.SortedColumn
        Dim LastSortedColumn_Name As String = ""
        Dim LastSortDirection As Windows.Forms.SortOrder = GRID.SortOrder

        If Not (LastSortedColumn Is Nothing) Then
            LastSortedColumn_Name = LastSortedColumn.Name
        End If

        If FP.P_DATA_RecordStatus = ENUM_RecordStatus.NEWRECORD Then
            Let_Goto_NewRecord = True
            FP.FORM_GOTO_NORECORD()
        End If

        If Not (DT Is Nothing) Then
            DT.RejectChanges()
        End If

        If Not (DT_ALL_FIELDS Is Nothing) Then
            DT_ALL_FIELDS.RejectChanges()
        End If

        FP.RAISEEVENT_GRID_Filter_Deactivated()

        If LastSortedColumn_Name > "" Then
            If LastSortDirection = Windows.Forms.SortOrder.Ascending Then
                GRID.Sort(GRID.Columns(LastSortedColumn_Name), System.ComponentModel.ListSortDirection.Ascending)
            Else
                GRID.Sort(GRID.Columns(LastSortedColumn_Name), System.ComponentModel.ListSortDirection.Descending)
            End If
        End If

        If Let_Goto_NewRecord Then
            FP.FORM_GOTO_NEWRECORD()
        End If

        If OUT Then
            FP.RAISEEVENT_GRID_Filter_Changed()
        End If

        FILTER_FIELDS_FILTER_DEACTIVATE = OUT
    End Function

    Public Sub REFRESH()
        Dim Current_RecordID As Long = FP.P_DATA_Current_ID
        Dim Current_FirstDisplayedColumnIndex As Integer = GRID.FirstDisplayedScrollingColumnIndex
        Dim Current_FirstDisplayedRowIndex As Integer = GRID.FirstDisplayedScrollingRowIndex
        Dim Current_ColumnIndex As Integer = -1

        If Not (GRID.CurrentCell Is Nothing) Then
            Current_ColumnIndex = GRID.CurrentCell.ColumnIndex
        End If

        FILL()
        FP.DATA_GOTO_RECORD_BY_ID(Current_RecordID)
        FP.DATA_RECORDS_LOAD_CURRENT()

        If Current_ColumnIndex > -1 Then
            If Not (GRID.CurrentCell Is Nothing) Then
                GRID.CurrentCell = GRID(Current_ColumnIndex, GRID.CurrentCell.RowIndex)
            End If
        End If
        GOTO_ROW_BY_RECORDID(Current_RecordID, Current_FirstDisplayedColumnIndex, , Current_FirstDisplayedRowIndex)
    End Sub

    Public Function FILL() As Boolean
        Dim OUT As Boolean = True
        Dim SQL As String = SQL_GET(FP.RS_ID, "", True)
        Dim SQL_ALL_FIELDS As String = SQL_GET(FP.RS_ID, "", False)
        Dim GRID_FieldList As String = " " + SQL_FIELD_LIST(False, True) + ","

        GRID_FieldList = Replace(GRID_FieldList, "[", "")
        GRID_FieldList = Replace(GRID_FieldList, "]", "")

        Dim col As DataGridViewColumn

        COLUMNS_Frozen_DEACTIVATE()

        Try
            '+++FP.FPf.FPApp.SQL_CLOSE()

            DT_ALL_FIELDS = New DataTable

            FP.FPf.FPApp.DC.Qdf_Fill_DT(SQL_ALL_FIELDS, DT_ALL_FIELDS)

            DT = DT_ALL_FIELDS.Copy

            For Each DT_col As DataColumn In DT_ALL_FIELDS.Columns
                Dim CurrColName As String = DT_col.ColumnName

                If CurrColName <> "RecordID" And CurrColName <> "SeqNum" Then
                    If InStr(GRID_FieldList, " " + CurrColName + ",") = 0 Then
                        DT.Columns.Remove(CurrColName)
                    End If
                End If
            Next

            GRID.DataSource = DT

            If DT.Rows.Count > 0 Or FP.P_FORM_AllowAdditions Then
                'GRID.CurrentCell = CELL_get_First_Cell(0)
                If GRID.Visible Then
                    FIELD_VISIBLE(GRID_Panel, True)
                End If
            Else
                FIELD_VISIBLE(GRID_Panel, False)
            End If

            If GRID.Columns.Contains("RecordID") Then
                GRID.Columns("RecordID").Visible = False
            End If

            If GRID.Columns.Contains("SeqNum") Then
                GRID.Columns("SeqNum").Visible = False
            End If

            For Each col In GRID.Columns
                Dim FPc As FP_Control = Nothing

                If FPc_FROM_Column(col.Index, FPc) Then
                    col.ReadOnly = True

                    If Not (FPc.c_Label Is Nothing) Then
                        col.HeaderText = FPc.c_Label.Text
                    End If

                    'Set Alignment
                    Dim Do_Alignment_setting As Boolean = True

                    If FPc.F_Format_ALIGN > "" And Not (TypeOf (FPc.c) Is CheckBox) Then
                        Select Case FPc.F_Format_ALIGN
                            Case "L" : GRID.Columns(col.Name).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                            Case "R" : GRID.Columns(col.Name).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                            Case "C" : GRID.Columns(col.Name).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                            Case Else
                                FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.FILL", 0, String.Format("Unknown alignment format '{0}'", FPc.F_Format_ALIGN))
                        End Select
                        Do_Alignment_setting = False
                    End If

                    If Do_Alignment_setting Then
                        Select Case DT.Columns(col.Name).DataType.FullName
                            Case "System.Int32", "System.Decimal", "System.Double"
                                GRID.Columns(col.Name).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                            Case "System.String", "System.Boolean", "System.DateTime"
                                'Nothing to do

                            Case Else
                                FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.FILL", 0, String.Format("Unknown Datatype '{0}' for field '{1}'", DT.Columns(col.Name).DataType.FullName, col.Name))
                        End Select
                    End If

                    'Set header text alignment
                    Dim Do_Header_Alignment_setting As Boolean = True

                    If FPc.F_Format_LABEL_ALIGN > "" And Not (TypeOf (FPc.c) Is CheckBox) Then
                        Select Case FPc.F_Format_LABEL_ALIGN
                            Case "L" : GRID.Columns(col.Name).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                            Case "R" : GRID.Columns(col.Name).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                            Case "C" : GRID.Columns(col.Name).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

                            Case Else
                                FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.FILL", 0, String.Format("Unknown alignment format '{0}'", FPc.F_Format_ALIGN))
                        End Select
                        Do_Header_Alignment_setting = False
                    End If

                    If Do_Header_Alignment_setting Then
                        Select Case DT.Columns(col.Name).DataType.FullName
                            Case "System.Int32", "System.Decimal", "System.Double"
                                GRID.Columns(col.Name).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight

                            Case "System.Boolean"
                                If TypeOf (FPc.c) Is CheckBox Then
                                    GRID.Columns(col.Name).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                                End If

                            Case "System.String", "System.DateTime"
                                'Nothing to do

                            Case Else
                                FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.FILL", 0, String.Format("Unknown Datatype '{0}' for field '{1}'", DT.Columns(col.Name).DataType.FullName, col.Name))
                        End Select
                    End If

                    If FPc.F_Format_FORMAT > "" Then
                        GRID.Columns(col.Name).DefaultCellStyle.Format = FPc.F_Format_FORMAT
                    Else
                        Select Case DT.Columns(col.Name).DataType.FullName
                            Case "System.Int32"
                                GRID.Columns(col.Name).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                            Case "System.String", "System.Boolean"
                                'Nothing to do

                            Case "System.DateTime"
                                'Nothing to do
                                'GRID.Columns(Col.Name).DefaultCellStyle.Format = "d"

                            Case "System.Decimal", "System.Double"
                                GRID.Columns(col.Name).DefaultCellStyle.Format = "N2"

                            Case Else
                                FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.FILL", 0, String.Format("Unknown Datatype '{0}' for field '{1}'", DT.Columns(col.Name).DataType.FullName, col.Name))
                        End Select
                    End If

                    If Not FPc.P.Visible Then
                        col.Visible = False
                    End If
                End If
            Next

            COLUMNS_WITH_SET()
            FILTER_PANEL_INIT()
            FILTER_FIELDS_FILTER_ACTIVATE()

        Catch ex As Exception
            OUT = False
            FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.FILL", Err.Number, Err.Description)
        End Try

        FILL = OUT
    End Function

    Public Function ROW_CURRENT_REFRESH(Optional ByVal WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = True

        If FP.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
            If ROW_GET_CURRENT(False) Is Nothing Then
                OUT = False
                If WithDialog Then
                    FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.ROW_CURRENT_Refresh", 0, "Currentrow in GRID not found")
                End If
            Else
                Dim Crit_NewRow As String = String.Format("RecordID = {0}", FP.P_DATA_Current_ID)
                Dim SQL_NewRow As String = SQL_GET(FP.RS_ID, Crit_NewRow, False)

                Dim DA As SqlDataAdapter = New SqlDataAdapter(SQL_NewRow, FP.FPf.FPApp.DC.CNN)
                Dim DT_NewRow As DataTable = New DataTable
                Dim DoIt As Boolean = True

                Try
                    '+++FP.FPf.FPApp.SQL_CLOSE()
                    DA.Fill(DT_NewRow)

                Catch ex As Exception
                    DoIt = False
                    FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.ROW_CURRENT_Refresh", Err.Number, Err.Description)
                End Try

                If DoIt Then
                    If DT_NewRow.Rows.Count <> 1 Then
                        OUT = False
                        If WithDialog Then
                            FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.ROW_CURRENT_Refresh", 0, "Current row not found.")
                        End If
                    Else
                        Dim NewRow As DataRow = DT_NewRow.Rows(0)
                        Dim DT_ALL_FIELDS_CurrentRow As DataRow = ROW_GET_CURRENT(False)
                        Dim DT_CurrentRow As DataRow = ROW_GET_CURRENT(True)

                        DT_ALL_FIELDS_CurrentRow.BeginEdit()
                        DT_CurrentRow.BeginEdit()
                        For Each col As System.Data.DataColumn In DT_NewRow.Columns
                            Select Case col.ColumnName
                                Case "RecordID"
                                    'Nothing to do

                                Case "SeqNum"
                                    If DT_ALL_FIELDS_CurrentRow!SeqNum <> NewRow!SeqNum Then
                                        FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.ROW_CURRENT_REFRESH", 0, "A SeqNum erteke megvaltozott. Ilyen esetben a ROW_CURRENT_REFRESH eljaras nem alkalmazhato.")
                                    End If

                                Case Else
                                    DT_ALL_FIELDS_CurrentRow.Item(col.ColumnName) = NewRow.Item(col.ColumnName)
                                    If DT.Columns.Contains(col.ColumnName) Then
                                        DT_CurrentRow.Item(col.ColumnName) = NewRow.Item(col.ColumnName)
                                    End If
                            End Select
                        Next
                        DT_ALL_FIELDS_CurrentRow.EndEdit()
                        DT_CurrentRow.EndEdit()
                    End If
                End If
            End If
        End If
    End Function

    Public Function ROW_CURRENT_ROWINDEX(ByRef OUT_ROWINDEX As Integer) As Boolean
        Dim OUT As Boolean = False

        OUT_ROWINDEX = -1

        If FP.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            If Not (GRID Is Nothing) Then
                If GRID.CurrentRow Is Nothing Then
                    GOTO_ROW_BY_RECORDID(FP.P_DATA_Current_ID)
                End If
                If Not GRID.CurrentRow Is Nothing Then
                    OUT_ROWINDEX = GRID.CurrentRow.Index
                    OUT = True
                End If
            End If
        End If

        ROW_CURRENT_ROWINDEX = OUT
    End Function

    Public Function ROW_GET_CURRENT(Only_Fields_In_GRIDVIEW As Boolean) As DataRow
        Dim OUT As DataRow = Nothing

        Select Case FP.P_DATA_RecordStatus
            Case ENUM_RecordStatus.NORECORD
                OUT = Nothing

            Case ENUM_RecordStatus.NEWRECORD
                OUT = DT.NewRow

            Case ENUM_RecordStatus.EXISTS
                Dim Criteria As String = String.Format("RecordID = {0}", FP.P_DATA_Current_ID)
                If Only_Fields_In_GRIDVIEW Then
                    OUT = DT.Select(Criteria).First
                Else
                    OUT = DT_ALL_FIELDS.Select(Criteria).First
                End If
        End Select

        ROW_GET_CURRENT = OUT
    End Function

    Function ROW_GET_NEXTROW_RECORDID(ByRef OUT_RECORDID As Long) As Boolean
        Dim OUT As Boolean = False
        Dim CurrentRowIndex As Integer = 0

        OUT_RECORDID = 0
        If ROW_CURRENT_ROWINDEX(CurrentRowIndex) Then
            If Not GRID.CurrentRow.Index = GRID.NewRowIndex Then
                OUT_RECORDID = ROW_GET_RecordID(CurrentRowIndex + 1)
                If OUT_RECORDID = 0 Then
                    OUT = FP.P_FORM_AllowAdditions
                Else
                    OUT = True
                End If
            End If
        End If

        ROW_GET_NEXTROW_RECORDID = OUT
    End Function

    Function ROW_GET_PREVIOUSROW_RECORDID(ByRef OUT_RECORDID As Long) As Boolean
        Dim OUT As Boolean = False
        Dim CurrentRowIndex As Integer = 0

        OUT_RECORDID = 0
        If ROW_CURRENT_ROWINDEX(CurrentRowIndex) Then
            If CurrentRowIndex > 0 Then
                OUT_RECORDID = ROW_GET_RecordID(CurrentRowIndex - 1)
                OUT = True
            End If
        End If

        ROW_GET_PREVIOUSROW_RECORDID = OUT
    End Function

    Function ROW_GET_LASTROW_RECORDID(ByRef OUT_RECORDID As Long) As Boolean
        Dim OUT As Boolean = False
        Dim CurrentRowIndex As Integer = 0

        OUT_RECORDID = 0
        If GRID.RowCount > 0 Then
            OUT_RECORDID = ROW_GET_RecordID(GRID.RowCount - 1)
            OUT = True
        End If

        ROW_GET_LASTROW_RECORDID = OUT
    End Function

    Function ROW_GET_FIRSTROW_RECORDID(ByRef OUT_RECORDID As Long) As Boolean
        Dim OUT As Boolean = False

        OUT_RECORDID = 0
        If GRID.RowCount > 0 Then
            OUT_RECORDID = ROW_GET_RecordID(0)
            OUT = True
        End If

        ROW_GET_FIRSTROW_RECORDID = OUT
    End Function

    Protected Friend Function GRID_PANEL_INIT_DATAFIELDS() As Boolean
        Dim OUT As Boolean = True
        Dim AktKey As String

        Dim DIC_Columns As New Dictionary(Of Integer, FP_Control)

        For Each AktKey In FP.CONTROLS.Keys
            With FP.CONTROLS(AktKey)
                If .P.ShowInGRID Then
                    If DIC_Columns.ContainsKey(.c.TabIndex) Then
                        FP.FPf.FPApp.DoErrorMsgBox("", 0, String.Format("Duplicated TabIndex (ServerObject_Prefix: '{0}', SubPrefix: '{1}', FieldName: '{2}', TabIndex: {3}). The TabIndex property must be unique for the GRID items.", FP.ServerObject_Prefix, FP.SubPrefix, .FieldName, .c.TabIndex.ToString))
                    Else
                        DIC_Columns.Add(.c.TabIndex, FP.CONTROLS(AktKey))
                    End If
                End If
            End With
        Next

        Dim L_TabOrder As List(Of Integer) = DIC_Columns.Keys.ToList

        Dim AktTabOrder As Integer

        L_TabOrder.Sort()
        L_TabOrder.Reverse()

        For Each AktTabOrder In L_TabOrder
            With DIC_Columns(AktTabOrder)
                .c.Top = 0
                .c.Left = 0
                If .c_Label Is Nothing Then
                    FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.GRID_PANEL_INIT_DATAFIELDS", 0, String.Format("Field {0} has no label. Controls in GRID must have a label.", .FieldName))
                Else
                    .c_Label.Visible = False
                End If

                .c.Visible = True

                .c.Parent = GRID_Panel

                .c.BringToFront()
            End With
        Next

        GRID_PANEL_INIT_DATAFIELDS = OUT
    End Function

    Protected Friend Sub GRID_REMOVE_ALL_CONTROLS()
        If Not (GRID_Panel Is Nothing) Then
            FP.PICTUREBOXES_REMOVE(EditingIcon.Name)
            FP.FPf.CONTROLS_REMOVE(RecordSelector.Name)
            RecordSelector.Dispose()
            FILTER_PANEL_REMOVE_ALL_CONTROLS()
            FP.PICTUREBOXES_REMOVE(FPc_FilterIcon.FieldName)
            FPc_FilterIcon = Nothing
            FP.FPf.CONTROLS_REMOVE(Filter_Selector.Name)
            Filter_Selector.Dispose()
            Filter_Selector = Nothing

            Dim DIC As Dictionary(Of String, Control) = Nothing
            Dim AktKey As String

            CONTROLS_WRITE_ALL_CONTROLS_TO_DIC(GRID_Panel, DIC)

            For Each AktKey In DIC.Keys
                If Left(AktKey, 10) = "#FP_SETUP_" Then
                    GRID_Panel.Controls(AktKey).Parent = GRID_Panel.Parent
                Else
                    Dim FPc As FP_Control = FP.CONTROLS_GET_FPc(AktKey)

                    If FPc Is Nothing Then
                        FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.GRID_REMOVE_ALL_CONTROLS", 0, String.Format("Control '{0}' not found.", AktKey))
                    Else
                        If Not (FPc.c Is Nothing) Then
                            FPc.c.Parent = GRID_Panel.Parent
                        End If
                        If Not (FPc.c_Label Is Nothing) Then
                            FPc.c_Label.Parent = GRID_Panel.Parent
                        End If

                        Dim FieldName_To_Remove As String = FP.CONTROLS_GET_FieldName_Without_FieldPrefix(AktKey)
                        FP.CONTROLS_REMOVE(FieldName_To_Remove)
                    End If
                End If
            Next AktKey

            FP.FPf.CONTROLS_REMOVE(GRID_Panel.Name)
            GRID_Panel.Dispose()

            If Not Footer_Panel Is Nothing Then
                CONTROLS_WRITE_ALL_CONTROLS_TO_DIC(Footer_Panel, DIC)

                For Each AktKey In DIC.Keys
                    If Left(AktKey, 10) = "#FP_SETUP_" Then
                        Footer_Panel.Controls(AktKey).Parent = Footer_Panel.Parent
                    Else
                        Dim FPc As FP_Control = FP.CONTROLS_GET_FPc(AktKey)

                        If Not FPc Is Nothing Then
                            FP.CONTROLS_REMOVE(AktKey)
                        End If
                    End If
                Next AktKey
            End If
        End If
    End Sub

    Private Function Control_Prefix() As String
        Dim OUT As String = "#" + FP.ServerObject_Prefix

        If FP.P_FieldPrefix > "" Then
            OUT += "_" + FP.P_FieldPrefix
        End If

        Return OUT
    End Function

    Protected Friend Function GRID_PANEL_INIT() As Boolean
        Dim OUT As Boolean = True

        If Not (GRID_Panel Is Nothing) Then
            OUT = False
            FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.GRID_Panel_INIT", 0, "GRID_Panel already created.")
        Else
            GRID.TabStop = False

            GRID_Panel = New Panel
            With GRID_Panel
                .Name = Control_Prefix() + "_GRID_PANEL"
                .Visible = True

                GRID.Parent.Controls.Add(GRID_Panel)
                FP.FPf.CONTROLS_ADD(GRID_Panel)
                .BackColor = Color.Black
                .Left = GRID.Left
                .Top = GRID.Top + GRID.Height - 30
                .Width = GRID.Width
                .Height = RowHeight

                .TabIndex = GRID.TabIndex
                GRID.TabStop = False

                .BringToFront()
            End With

            RecordSelector = New Panel
            With RecordSelector
                .Name = Control_Prefix() + "_RecordSelector"
                GRID_Panel.Controls.Add(RecordSelector)
                FP.FPf.CONTROLS_ADD(RecordSelector)
                .BackColor = Color.DimGray
                .Left = 0
                .Top = 0
                .Width = 22
                .Height = FP.FPf.P_Layout_TextBox_NormalHeight
                .Visible = True
                .TabIndex = 99999
                .TabStop = False
                .BringToFront()
            End With

            GRID_PANEL_INIT_DATAFIELDS()

            EditingIcon = New PictureBox
            With EditingIcon
                .Parent = RecordSelector
                .Name = Control_Prefix() + "_EditingIcon"
                FP.FPf.CONTROLS_ADD(EditingIcon)
                .Left = 0
                .Top = 0
                .Width = 22
                .Height = 22
                .Visible = False
                .BringToFront()
            End With
            FPc_EditIcon = New FP_PictureBox(FP.FPf, FP, EditingIcon, , "FP_GRID_edit_22x22.png")
            FPc_EditIcon.CreatedAtRuntime = True
            FPc_EditIcon.CreatedBy = ENUM_FP_CONTROL_Created_by.GRID
            FP.PICTUREBOXES_ADD(FPc_EditIcon)

            '------------- FILTER -------------
            Filter_Panel = New Panel
            With Filter_Panel
                .Name = Control_Prefix() + "_FILTER_PANEL"
                .Visible = False

                GRID.Parent.Controls.Add(Filter_Panel)
                FP.FPf.CONTROLS_ADD(Filter_Panel)
                .BackColor = Color.DarkGray
                .Top = GRID.Top - RowHeight
                .Height = RowHeight
                'Width and Left set is in FILTER_PANEL_ARRANGE
                .TabIndex = GRID.TabIndex
                .TabStop = False

                .BringToFront()
            End With

            Filter_Selector = New Panel
            With Filter_Selector
                .Name = Control_Prefix() + "_FilterSelector"
                GRID.Parent.Controls.Add(Filter_Selector)
                FP.FPf.CONTROLS_ADD(Filter_Selector)
                .BackColor = Color.DarkGray
                .Left = GRID.Left
                .Top = GRID.Top
                .Width = 22
                .Height = HeaderHeight
                .Visible = True
                .TabIndex = 99999
                .TabStop = False
                .BringToFront()
            End With

            FilterIcon = New PictureBox
            With FilterIcon
                .Parent = Filter_Selector
                .Name = Control_Prefix() + "_FilterIcon"
                .Left = 0
                .Top = 0
                .Width = 22
                .Height = 22
                .Visible = True
                .BringToFront()
            End With
            FP.FPf.CONTROLS_ADD(FilterIcon)
            FPc_FilterIcon = New FP_PictureBox(FP.FPf, FP, FilterIcon, True, "FP_GRID_filter_22x22.png")
            FPc_FilterIcon.CreatedAtRuntime = True
            FPc_FilterIcon.CreatedBy = ENUM_FP_CONTROL_Created_by.GRID
            FP.PICTUREBOXES_ADD(FPc_FilterIcon)

            '-------------- HELP -------------
            If Not (FP.FPf.HELP_Frm Is Nothing) Then
                With FP.FPf.HELP_Frm
                    .ADD_HELP_STANDARD_ITEM("###GRID_EditingIcon###", EditingIcon.Name)
                    .ADD_HELP_STANDARD_ITEM("###GRID_FilterIcon###", FPc_FilterIcon.c.Name)
                End With
                FP.FPf.HELP_Frm.ADD_HELP_DICTIONARY("FP_GRID", "")
            End If
        End If

        GRID_PANEL_INIT = OUT
    End Function

    Public Sub COLUMNS_HIDE(ByVal ColumnName As String)
        If GRID.Columns.Contains(ColumnName) Then
            With GRID.Columns(ColumnName)
                If .Visible Then
                    GRID.CurrentCell = Nothing
                    GRID.Columns(ColumnName).Visible = False
                    GRID_PANEL_ARRANGE(Nothing)
                End If
            End With
        End If
    End Sub

    Public Sub COLUMNS_SHOW(ByVal ColumnName As String)
        If GRID.Columns.Contains(ColumnName) Then
            With GRID.Columns(ColumnName)
                If Not .Visible Then
                    GRID.CurrentCell = Nothing
                    GRID.Columns(ColumnName).Visible = True
                    GRID_PANEL_ARRANGE(Nothing)
                End If
            End With
        End If
    End Sub

    Private Sub COLUMNS_WITH_SET()
        Dim ColumnIndex As Integer

        For ColumnIndex = 0 To GRID.Columns.Count - 1
            If FP.CONTROLS.ContainsKey(GRID.Columns(ColumnIndex).Name) Then
                Dim FPc As FP_Control = FP.CONTROLS(GRID.Columns(ColumnIndex).Name)

                If GRID.Columns(ColumnIndex).Width <> FPc.c.Width Then
                    GRID.Columns(ColumnIndex).Width = FPc.c.Width
                End If
            End If
        Next
    End Sub

    Protected Friend Sub FILTER_PANEL_ARRANGE()
        ARRNAGE_Selectors()

        If Filter_Panel.Visible Then
            Dim Filter_Panel_Left As Integer = GRID_Panel.Left + RecordSelector.Width
            Dim Filter_Panel_Width As Integer

            If Btn_FooterVisible Is Nothing Then
                Filter_Panel_Width = GRID.Width - RecordSelector.Width
            Else
                Filter_Panel_Width = GRID.Width - RecordSelector.Width - Btn_FooterVisible.c.Width
            End If

            With Filter_Panel
                If .Left <> Filter_Panel_Left Then
                    .Left = Filter_Panel_Left
                End If
                If .Width <> Filter_Panel_Width Then
                    .Width = Filter_Panel_Width
                End If
            End With

            Dim i As Integer

            For i = 0 To GRID.Columns.Count - 1
                Dim Col_Rect As Rectangle = GRID.GetColumnDisplayRectangle(i, False)
                Dim FilterField_Name As String = CONTROLS_FILTERFIELDS_GET_NAME(GRID.Columns(i).Name)

                If Filter_Panel.Controls.ContainsKey(FilterField_Name) Then
                    Dim FilterField As Control = Filter_Panel.Controls(FilterField_Name)

                    With FilterField
                        If Col_Rect.Width = 0 Then
                            .Left = -.Width - 1
                        Else
                            If .Width <> Col_Rect.Width Then
                                .Width = Col_Rect.Width
                            End If

                            Dim Col_Left = Col_Rect.Left - RecordSelector.Width

                            If .Left <> Col_Left Then
                                .Left = Col_Left
                            End If
                        End If
                    End With
                End If
            Next
        End If
    End Sub

    Private Sub GRID_PANEL_ARRANGE(ByVal ReferenceCell As DataGridViewCell)
        ARRNAGE_Selectors()

        If GRID.Columns.Count > 0 Then
            Dim GridRow_Left = GRID.Left + GRID.DisplayRectangle.Left

            If GRID_Panel.Left <> GRID.Left Then
                GRID_Panel.Left = GRID.Left
            End If

            If GRID_Panel.Width <> GRID.DisplayRectangle.Width - 1 Then
                GRID_Panel.Width = GRID.DisplayRectangle.Width - 1
            End If

            COLUMNS_WITH_SET()

            For ColumnIndex As Integer = 0 To GRID.Columns.Count - 1
                If FP.CONTROLS.ContainsKey(GRID.Columns(ColumnIndex).Name) Then
                    Dim FPc As FP_Control = FP.CONTROLS(GRID.Columns(ColumnIndex).Name)
                    Dim CellRect As New Rectangle(New Point(0, 0), New Size(0, 22))

                    If Not (ReferenceCell Is Nothing) Then
                        Dim ColumnCell As DataGridViewCell = GRID(ColumnIndex, ReferenceCell.RowIndex)

                        CellRect = get_CELL_Rect(ColumnCell)
                    End If

                    'If GRID.Columns(ColumnIndex).Width <> FPc.c.Width Then
                    '    GRID.Columns(ColumnIndex).Width = FPc.c.Width
                    'End If

                    If CellRect.Width = 0 Then
                        FPc.c.Left = -FPc.c.Width
                    Else
                        'If FPc.c.Height <> CellRect.Height Then
                        '    FPc.c.Height = CellRect.Height
                        'End If

                        If FPc.c.Height <> 22 Then
                            FPc.c.Height = 22
                        End If

                        If FPc.c.Top <> 0 Then
                            FPc.c.Top = 0
                        End If

                        If FPc.c.Width = CellRect.Width Then
                            If FPc.c.Left <> CellRect.Left Then
                                FPc.c.Left = CellRect.Left
                            End If
                        Else
                            If FPc.c.Left <> CellRect.Left + CellRect.Width - FPc.c.Width Then
                                FPc.c.Left = CellRect.Left + CellRect.Width - FPc.c.Width
                            End If
                        End If
                    End If


                    If FPc.c.Left > 0 Then
                        FPc.c.Height = GRID_Panel.Height
                    End If
                End If
            Next

            FILTER_PANEL_ARRANGE()
            Marker_Move()
        End If
    End Sub

    Private Sub Marker_Move()
        With FP.FPf
            If Not (.Marker_sign Is Nothing) Then
                If .Marker_sign.Visible Then
                    If Not (.FPc_Marker_Sign Is Nothing) Then
                        .Marker_SHOW(.FPc_Marker_Sign)
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub ARRNAGE_Selectors()
        If RecordSelector.Width <> GRID.RowHeadersWidth + 1 Then
            RecordSelector.Width = GRID.RowHeadersWidth + 1

            Dim EditingIcon_X As Integer = (RecordSelector.Width - EditingIcon.Width) / 2

            If EditingIcon.Left <> EditingIcon_X Then
                EditingIcon.Left = EditingIcon_X
            End If
        End If

        RecordSelector.BringToFront()

        'Filter_Selector
        If Filter_Selector.Width <> GRID.RowHeadersWidth + 1 Then
            Filter_Selector.Width = GRID.RowHeadersWidth + 1
        End If

        Dim FilterIcon_X As Integer = (Filter_Selector.Width - FPc_FilterIcon.c.Width) / 2

        If FPc_FilterIcon.c.Left <> FilterIcon_X Then
            FPc_FilterIcon.c.Left = FilterIcon_X
        End If

        RecordSelector.BringToFront()

        If Filter_Panel.Top < 0 Then
            Filter_Panel.Top = 0
        End If

    End Sub

    Public Function CELL_get_From_FPc(ByVal FPc As FP_Control, ByRef OUT_Cell As DataGridViewCell) As Boolean
        Dim OUT As Boolean = False

        OUT_Cell = Nothing

        If Not (FPc Is Nothing) Then
            If Not (GRID Is Nothing) Then
                If FPc.P.ShowInGRID Then
                    If FPc.FP.Equals(FP) Then
                        Dim i As Integer

                        If ROW_GET_ROWINDEX_OF_RECORDID(FP.P_DATA_Current_ID, i) Then
                            OUT_Cell = GRID(GRID.Columns(FPc.FieldName).Index, i)
                            OUT = True
                        End If
                    End If
                End If
            End If
        End If

        CELL_get_From_FPc = OUT
    End Function

    Public Function get_first_FPc() As FP_Control
        Dim OUT As FP_Control = Nothing

        If Not (FP Is Nothing) Then
            If Not (GRID Is Nothing) Then
                Dim ColumnIndex = 0

                Do While (OUT Is Nothing) And ColumnIndex < GRID.ColumnCount
                    With GRID.Columns(ColumnIndex)
                        If .Name <> "RecordID" Then
                            If FP.CONTROLS.ContainsKey(.Name) Then
                                OUT = FP.CONTROLS(.Name)
                            End If
                        End If
                    End With

                    ColumnIndex += 1
                Loop
            End If
        End If

        get_first_FPc = OUT
    End Function

    Public Function get_last_FPc() As FP_Control
        Dim OUT As FP_Control = Nothing

        If Not (FP Is Nothing) Then
            If Not (GRID Is Nothing) Then
                Dim ColumnIndex = GRID.ColumnCount - 1

                Do While (OUT Is Nothing) And ColumnIndex > 0
                    With GRID.Columns(ColumnIndex)
                        If .Name <> "RecordID" Then
                            If FP.CONTROLS.ContainsKey(.Name) Then
                                OUT = FP.CONTROLS(.Name)
                            End If
                        End If
                    End With

                    ColumnIndex -= 1
                Loop
            End If
        End If

        get_last_FPc = OUT
    End Function

    Public Function CELL_get_First_Cell(ByVal RowIndex As Integer) As DataGridViewCell
        Dim OUT As DataGridViewCell = Nothing
        Dim First_FPc As FP_Control = get_first_FPc()

        If Not (First_FPc Is Nothing) Then
            OUT = GRID(GRID.Columns(First_FPc.FieldName).Index, RowIndex)
        End If

        CELL_get_First_Cell = OUT
    End Function

    Public Function COLUMN_get_Index_Of_First_Column() As Integer
        Dim OUT As Integer = 0

        Dim First_FPc As FP_Control = get_first_FPc()

        If Not (First_FPc Is Nothing) Then
            OUT = GRID.Columns(First_FPc.FieldName).Index
        End If

        COLUMN_get_Index_Of_First_Column = OUT
    End Function

    Public Function COLUMN_ENSURE_VISIBLE(ByVal ColumnName As String) As Boolean
        Dim OUT As Boolean = False
        Dim ri As Integer

        If ROW_CURRENT_ROWINDEX(ri) Then
            If ColumnName > "" Then
                If GRID.Columns.Contains(ColumnName) Then
                    If GRID.DisplayedColumnCount(False) > 0 Then
                        With GRID.Columns(ColumnName)
                            If .Frozen = True Then
                                OUT = True
                            Else
                                Dim i As Integer = GRID.FirstDisplayedScrollingColumnIndex
                                Dim k As Integer

                                If i = .Index Then
                                    If get_CELL_Rect(GRID(i, ri), True).Width = GRID.Columns(i).Width Then
                                        OUT = True
                                    Else
                                        'Ahhoz, hogy a GRID tenyleg a jelenleg is (de csak reszlegesen) lathato, mezo bal
                                        'szelere alljon, szukseges, hogy eloszor egy masik, lathato mezore allitsuk a
                                        'FirsDisplayedScrollingColumnIndex-et. Errol szol ez a for k ciklus
                                        For k = 0 To GRID.Columns.Count - 1
                                            If GRID.Columns(k).Visible And i <> k And GRID.Columns(k).Frozen = False Then
                                                GRID.FirstDisplayedScrollingColumnIndex = k
                                                Exit For
                                            End If
                                        Next

                                        GRID.FirstDisplayedScrollingColumnIndex = k
                                        GRID.FirstDisplayedScrollingColumnIndex = .Index
                                        GRID_PANEL_ARRANGE(GRID(i, ri))

                                        OUT = True
                                    End If
                                ElseIf i > .Index Then
                                    GRID.FirstDisplayedScrollingColumnIndex = .Index
                                    GRID_PANEL_ARRANGE(GRID(i, ri))

                                    OUT = True
                                Else
                                    Dim ColumnCurrentX As Integer = 0
                                    Dim DisplayWidth As Integer = 0

                                    For k = i To .Index
                                        ColumnCurrentX += GRID.Columns(k).Width + GRID.Columns(k).DividerWidth
                                        DisplayWidth += get_CELL_Rect(GRID(k, ri), True).Width
                                    Next

                                    Dim dx As Integer = ColumnCurrentX - DisplayWidth

                                    While i < .Index And dx > 0
                                        dx -= GRID.Columns(i).Width + GRID.Columns(i).DividerWidth
                                        i += 1
                                    End While

                                    While (i < .Index) And (GRID.Columns(i).Visible = False)
                                        i += 1
                                    End While
                                    If i <> GRID.FirstDisplayedScrollingColumnIndex Then
                                        GRID.FirstDisplayedScrollingColumnIndex = i
                                        GRID_PANEL_ARRANGE(GRID(i, ri))
                                    End If
                                End If

                                OUT = True
                            End If
                        End With
                    End If
                End If
            End If
        End If

        COLUMN_ENSURE_VISIBLE = OUT
    End Function

    Public Function ROW_GET_ROWINDEX_OF_RECORDID(ByVal RecordID As Long, ByRef OUT_RowIndex As Integer) As Boolean
        Dim OUT As Boolean = False

        OUT_RowIndex = -1

        If RecordID = 0 Then
            If GRID.NewRowIndex >= 0 Then
                OUT_RowIndex = GRID.NewRowIndex
                OUT = True
            End If
        Else
            If DT.Columns.Contains("RecordID") Then
                If DT.Select(String.Format("RecordID = {0}", RecordID.ToString)).Count > 0 Then
                    For i = 0 To GRID.RowCount - 1
                        If ROW_GET_RecordID(i) = RecordID Then
                            OUT_RowIndex = i
                            OUT = True

                            Exit For
                        End If
                    Next
                End If
            End If
        End If

        ROW_GET_ROWINDEX_OF_RECORDID = OUT
    End Function

    Public Sub GOTO_NORECORD()
        GRID.CurrentCell = Nothing
        GRID_PANEL_HIDE()
    End Sub

    Public Function GOTO_ROW_BY_RECORDID(ByVal RecordID As Long, Optional ByVal FirstDisplayedColumnIndex As Integer = -1, Optional ByVal SetAsFirstRowInGRID As Boolean = False, Optional ByVal FirstDisplayedRowIndex As Integer = -1) As Boolean
        Dim OUT As Boolean = False
        Dim i As Integer

        If ROW_GET_ROWINDEX_OF_RECORDID(RecordID, i) Then

            Dim CurrentColumnIndex As Integer = COLUMN_get_Index_Of_First_Column()

            If Not (GRID.CurrentCell Is Nothing) Then
                CurrentColumnIndex = GRID.CurrentCell.ColumnIndex
            Else
                If Not (FP.FPf.ActiveControl Is Nothing) Then
                    If FPc_HAS_FIELD(FP.FPf.ActiveControl) Then
                        With FP.FPf.ActiveControl
                            If .c.Parent.Equals(GRID_Panel) Then
                                CurrentColumnIndex = GRID.Columns(.FieldName).Index
                            End If
                        End With
                    End If
                End If
            End If

            If SetAsFirstRowInGRID Then
                GRID.FirstDisplayedScrollingRowIndex = i
            End If

            If GRID(CurrentColumnIndex, i).Visible Then
                GRID.CurrentCell = GRID(CurrentColumnIndex, i)

                If FirstDisplayedRowIndex > -1 Then
                    Try
                        GRID.FirstDisplayedScrollingRowIndex = FirstDisplayedRowIndex
                    Catch ex As Exception
                        Dim ww As Integer = 1

                    End Try
                    'GRID.FirstDisplayedScrollingRowIndex = FirstDisplayedRowIndex
                End If

                If FirstDisplayedColumnIndex > -1 Then
                    GRID.FirstDisplayedScrollingColumnIndex = FirstDisplayedColumnIndex
                End If

                MOVE_GRID_PANEL_ON_ROW()

                OUT = True
            End If
        End If

        GOTO_ROW_BY_RECORDID = OUT
    End Function

    Public Sub PREPARE_EDIT(ByVal FPc As FP_Control)
        Dim CurrentCell As DataGridViewCell = Nothing

        If FP.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            GOTO_ROW_BY_RECORDID(FP.P_DATA_Current_ID)

            If CELL_get_From_FPc(FPc, CurrentCell) Then
                GRID.CurrentCell = CurrentCell
                COLUMN_ENSURE_VISIBLE(FPc.FieldName)
                GRID_PANEL_ARRANGE(CurrentCell)
            End If
        End If
    End Sub

    Public Function FPc_FROM_Column(ByVal ColumnIndex As Integer, ByRef OUT_FPc As FP_Control) As Boolean
        Dim OUT As Boolean = False

        OUT_FPc = Nothing

        If ColumnIndex >= 0 Then
            If FP.CONTROLS.ContainsKey(GRID.Columns(ColumnIndex).Name) Then
                OUT_FPc = FP.CONTROLS(GRID.Columns(ColumnIndex).Name)
                OUT = True
            End If
        End If

        FPc_FROM_Column = OUT
    End Function

    Public Function ROW_CURRENT_RecordID() As Long
        Dim OUT As Long = 0

        If FP.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            If GRID.CurrentCell Is Nothing Then
                FP.FPf.FPApp.DoErrorMsgBox("FP_GRIDVIEW.ROW_CURRENT_RecordID", 0, "CurrentCell Is Nothing.")
            Else
                OUT = ROW_GET_RecordID(GRID.CurrentCell.RowIndex)
            End If
        End If

        ROW_CURRENT_RecordID = OUT
    End Function

    Public Function ROW_GET_RecordID(ByVal RowIndex As Integer) As Long
        Dim OUT As Long = 0

        If Not (GRID Is Nothing) Then
            If GRID.NewRowIndex = RowIndex Then
                OUT = 0
            Else
                If RowIndex < GRID.RowCount Then
                    OUT = nz(GRID(GRID.Columns("RecordID").Index, RowIndex).Value, 0)
                End If
            End If
        End If

        Return OUT
    End Function

    Private Sub EVENT_DG_CELLCLICK(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles GRID.CellClick
        If Not Disposed Then
            If e.RowIndex >= 0 Then
                Dim i As Integer

                ROW_GET_ROWINDEX_OF_RECORDID(FP.P_DATA_Current_ID, i)
                If Not ROW_VISIBLE(e.RowIndex) Then
                    Try
                        GRID.FirstDisplayedScrollingRowIndex += 1

                    Catch ex As Exception
                        'Nothing to do - lehet, hogy olyan kicsi az ablak, hogy 1 sor sem fer ki, akkor hiba jonne
                    End Try
                End If

                Dim DoIt As Boolean = True

                If e.RowIndex <> i Then
                    DoIt = FP.FORM_RECORDS_SAVE_CURRENT
                End If

                If Not DoIt Then
                    GRID.CurrentCell = Nothing
                Else
                    Dim FPc As FP_Control = Nothing
                    If FPc_FROM_Column(e.ColumnIndex, FPc) Then
                        FP.FPf.P_ActiveControl = FPc
                    End If

                    If Not FP.FORM_GOTO_RECORD_BY_ID(ROW_GET_RecordID(e.RowIndex)) Then
                        FP.FORM_GOTO_NORECORD()
                    Else
                        If FPc_FROM_Column(e.ColumnIndex, FPc) Then
                            Dim Handled As Boolean = False

                            FP.FPf.P_ActiveControl = FPc
                            'FP.FPf.P_ActiveControl.EVENT_GOTFOCUS()

                            FP.RAISEEVENT_GRID_CellClick(FPc, Handled)
                            If Not Handled Then
                                FP.RAISEEVENT_Form_Field_Enter(FPc, Handled)
                                If Not Handled Then
                                    FP.FPf.FOCUS_ON_AT_THE_END(FPc.c)
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub EVENT_GRID_EDITICON_MOUSEUP(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles EditingIcon.MouseUp
        FP.FORM_RECORDS_SAVE_CURRENT()
    End Sub

    Private Sub EVENT_GRID_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles GRID.Scroll
        If e.ScrollOrientation = ScrollOrientation.VerticalScroll Then
            MOVE_GRID_PANEL_ON_ROW()
        Else
            Dim AktRowIndex As Integer

            If ROW_CURRENT_ROWINDEX(AktRowIndex) Then
                GRID_PANEL_ARRANGE(GRID(0, AktRowIndex))
            End If
        End If
    End Sub

    Private Sub EVENT_GRID_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GRID.VisibleChanged
        If FP.P_DATA_Binded Then
            MOVE_GRID_PANEL_ON_ROW()
        End If
    End Sub

    Public Function ROW_GET_ROWINDEX_FROM_POINT(ByVal p As Point, ByRef OUT_ROW As Integer) As Boolean
        Dim OUT As Boolean = False

        Dim hit As DataGridView.HitTestInfo = GRID.HitTest(p.X, p.Y)
        If hit.RowIndex <> -1 Then
            Dim Selected_RecordID As Long = ROW_GET_RecordID(hit.RowIndex)

            If Selected_RecordID <> 0 Then
                OUT_ROW = hit.RowIndex
                OUT = True
            End If
        End If

        Return OUT
    End Function

    Public Function ROW_GET_RECORDID_FROM_POINT(ByVal p As Point, ByRef OUT_Record_ID As Long) As Boolean
        Dim OUT As Boolean = False
        Dim Selected_RowIndex As Integer

        If ROW_GET_ROWINDEX_FROM_POINT(p, Selected_RowIndex) Then
            OUT_Record_ID = ROW_GET_RecordID(Selected_RowIndex)
            OUT = True
        End If

        Return OUT
    End Function

    Protected Friend Sub EVENT_GRID_MOUSEDOWN(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GRID.MouseDown
        'DragRow
        If FP.P_DragRow_Allowed Then
            If e.Button = MouseButtons.Right Then
                Dim Cancel As Integer = 0
                Dim Selected_RecordID As Long = 0

                If ROW_GET_RECORDID_FROM_POINT(New Point(e.X, e.Y), Selected_RecordID) Then
                    Dim DATA As String = ""

                    FP.RAISEEVENT_GRID_Row_Drag_Begin(DATA, e, Cancel)
                    If Not Cancel Then
                        GRID.DoDragDrop(DATA, DragDropEffects.Move)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub GRID_RowPostPaint(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles GRID.RowPostPaint
        If Not Disposed Then
            Dim AktRowIndex As Integer

            If ROW_CURRENT_ROWINDEX(AktRowIndex) Then
                If e.RowIndex = AktRowIndex Then
                    GRID_PANEL_ARRANGE(GRID(0, AktRowIndex))
                End If
            Else
                FILTER_PANEL_ARRANGE()
            End If
        End If
    End Sub

    Private Sub EVENT_GRID_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles GRID.Sorted
        GOTO_ROW_BY_RECORDID(FP.P_DATA_Current_ID)

        If Not (GRID.SortedColumn Is Nothing) Then
            Dim FPc As FP_Control = FP.CONTROLS_GET_FPc(GRID.SortedColumn.Name)
            If FPc_HAS_FIELD(FPc) Then
                FP.FPf.FOCUS_ON_AT_THE_END(FPc.c)
            End If
        End If
        FP.RAISEEVENT_GRID_Sorted(e)
    End Sub

    Private Sub EVENT_GRID_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles GRID.ColumnWidthChanged
        If Not Disposed Then
            Dim FPc As FP_Control = Nothing
            Dim CurrentColumnIndex As Integer = e.Column.Index
            Dim CurrentRowIndex As Integer
            Dim NewColumnWidth As Integer = e.Column.Width

            If Not ROW_CURRENT_ROWINDEX(CurrentRowIndex) Then
                FILTER_PANEL_ARRANGE()
            Else
                If FPc_FROM_Column(CurrentColumnIndex, FPc) Then
                    If FPc.c.Width <> NewColumnWidth Then
                        FPc.c.Width = NewColumnWidth
                        GRID_PANEL_ARRANGE(GRID(CurrentColumnIndex, CurrentRowIndex))
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub EVENT_GRID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles GRID.KeyPress
        FIELD_LOCKED(e)
    End Sub

    Private Sub EVENT_GRID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GRID.KeyDown
        e.Handled = True
    End Sub

    Private Sub FILTER_REMOVE()
        FILTER_FIELDS_FILTER_DEACTIVATE()

        Filter_Panel.Visible = False

        MOVE_GRID_PANEL_ON_ROW()
    End Sub

    Private Sub FPc_FilterIcon_CLICK() Handles FPc_FilterIcon.CLICK
        If FPc_FilterIcon.P_PRESSED Then
            Filter_Panel.Visible = True
            FILTER_PANEL_ARRANGE()
            FILTER_FIELDS_FILTER_ACTIVATE()
        Else
            FILTER_REMOVE()
        End If
    End Sub

    Private Sub GRID_Label_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GRID_Label.DoubleClick
        FPc_FilterIcon.P_PRESSED = True
        FPc_FilterIcon_CLICK()
    End Sub

    Private Sub GRID_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles GRID.Move
        If Not (Filter_Selector Is Nothing) Then
            With Filter_Selector
                .Left = GRID.Left
                .Top = GRID.Top
            End With
        End If

        If Not (Filter_Panel Is Nothing) Then
            Filter_Panel.Top = GRID.Top - RowHeight
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Private Sub GRID_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles GRID.Paint
        FP.RAISEEVENT_GRID_Paint(sender, e)
    End Sub

    Private Sub GRID_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles GRID.Resize
        MOVE_GRID_PANEL_ON_ROW()
    End Sub
End Class


