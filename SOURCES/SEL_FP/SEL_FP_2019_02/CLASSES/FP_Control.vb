Public Class FP_Control
    Inherits FP_ControlObject

    Public Enum ENUM_Markertypes
        None = 0
        Right_Arrow = 1
        Padlock_Unlock = 2
    End Enum

    Public Event Field_KeyPreview_KeyDown(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByRef e As System.Windows.Forms.KeyEventArgs)
    Public Event Field_KeyPreview_KeyPress(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByRef e As System.Windows.Forms.KeyPressEventArgs)
    Public Event Field_BeforeUpdate(ByVal sender_FPc As FP_Control, ByRef Cancel As Integer)
    Public Event Field_MouseWheel(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs, ByRef Handled As Boolean)
    Public Event Field_MouseEnter(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByVal e As System.EventArgs, ByRef Handled As Boolean)
    Public Event Field_MouseLive(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByVal e As System.EventArgs, ByRef Handled As Boolean)
    Public Event Field_Click(ByVal sender_FPc As FP_Control, ByVal e As System.EventArgs)
    Public Event Field_Doubleclick(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByVal e As System.EventArgs, ByRef Handled As Boolean)
    Public Event Field_Marker_Click(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByVal e As System.EventArgs, ByRef Handled As Boolean)
    Public Event Field_TextChanged(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByVal e As System.EventArgs, ByRef Cancel As Boolean)
    Public Event Field_Coloring(ByVal sender_FPc As FP_Control, ByRef Handled As Boolean)

    Public FieldName As String
    Public WithEvents c As Control
    Public WithEvents c_ChkBox As CheckBox
    Public WithEvents c_ComboBox As ComboBox
    Public WithEvents Select_Panel As Panel = Nothing
    Private c_ComboBox_Value As Long = 0 'Mivel a ComboBox mar olyan erteket is tud tarolni, ami nincs benne a listaban

    Public WithEvents c_Button As Button
    Public WithEvents c_HTML_editor As MSDN.Html.Editor.HtmlEditorControl

    Public WithEvents c_ListView As ListView
    Private c_ListView_LastSortOrder As ListViewItemComparer.Enum_SortOrder = ListViewItemComparer.Enum_SortOrder.Ascending
    Private c_ListView_LastSelected_Item_IDX As Integer = -1

    Public LabelName As String
    Public c_Label As System.Windows.Forms.Label
    Public DT As DataTable

    Public P As Struct_FP_CONTROL_PROPS = FIELD_UNKNOWN_PROPS()

    Public F_Format_TRIM As Boolean = False
    Public F_Format_UCASE As Boolean = False
    Public F_Format_NOSPACE As Boolean = False
    Public F_Format_FORMAT As String = ""
    Public F_Format_ALIGN As String = ""
    Public F_Format_LABEL_ALIGN As String = ""
    Public F_Format_MinusAllowed = False
    Public F_Format_NoShow0 As Boolean = False

    Protected Friend LastRecord_Value As String
    Protected Friend OldValue As String = ""
    Protected Friend OldValue_Selected_ID As Long
    Protected Friend Refresh_Type As ENUM_FP_CONTROL_REFRESH_TYPE = ENUM_FP_CONTROL_REFRESH_TYPE.Normal
    Public Selected_ID As Long = 0

    Public Tag_DIC As Dictionary(Of String, String)

    Public CreatedBy As ENUM_FP_CONTROL_Created_by

    Protected Friend CreatedAtRuntime As Boolean
    Protected Friend Value_Validated As Boolean = True

    Private Value_Before_TextChanged As String = ""
    Private Disposed As Boolean = False

    Private Marker As ENUM_Markertypes
    Private c_ComboBox_prevClick As DateTime = DateTime.Now

    Sub New(ByVal MyCreatedAtRuntime As Boolean)
        CreatedAtRuntime = MyCreatedAtRuntime
    End Sub
    Sub New(ByVal MyFP As FP, ByVal MyC As Control, ByVal MyC_Label As Label, ByVal MyCreatedAtRuntime As Boolean, FieldName As String, LabelName As String)
        CreatedAtRuntime = MyCreatedAtRuntime
        FP = MyFP
        If Not FP Is Nothing Then
            FPf = FP.FPf
        End If

        SET_CONTROLS(MyC, MyC_Label, FieldName, LabelName)

        If FP.FPf.FPApp.Is_DEBUG_MODE() Then
            c.ContextMenuStrip = FP.FPf.ContextMenu_DEBUG
            If Not (c_Label Is Nothing) Then
                c_Label.ContextMenuStrip = FP.FPf.ContextMenu_DEBUG
            End If
        End If

        MyFP.CONTROLS_ADD(Me)
    End Sub

    Public ReadOnly Property P_Disposed As Boolean
        Get
            Dim OUT As Boolean = Disposed

            If OUT = False Then
                If FPf Is Nothing Then
                    OUT = True
                Else
                    OUT = FPf.P_Disposed
                End If
            End If

            Return OUT
        End Get
    End Property
    Protected Friend Sub RAISEEVENT_Field_Before_Update(ByVal sender_FPc As FP_Control, ByRef Cancel As Integer)
        Dim ee As New System.EventArgs
        RaiseEvent Field_BeforeUpdate(sender_FPc, Cancel)
    End Sub

    Protected Friend Sub RAISEEVENT_Field_Marker_Click(Optional ByRef Handled As Boolean = False)
        Dim ee As New System.EventArgs

        RaiseEvent Field_Marker_Click(Me, c, ee, Handled)
    End Sub

    Protected Friend Sub RAISEEVENT_Field_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        c_Click(sender, e)
    End Sub

    Public Overloads Sub Dispose()
        If Disposed = False Then
			If Not (c Is Nothing) Then
				If (TypeOf (c) Is TabPage) Then
					If Not (c.Parent Is Nothing) Then
						FP.FPf.SPECCONTROLS(c.Parent.Name).Dispose()
					End If

				ElseIf (TypeOf (c) Is TabControl) Then
					FP.FPf.SPECCONTROLS(FieldName).Dispose()

				ElseIf (TypeOf (c) Is SplitContainer) Then
					FP.FPf.SPECCONTROLS(FieldName).Dispose()

				Else
					'Nothing to do
				End If
			End If

			c = Nothing
			c_ChkBox = Nothing
			c_ComboBox = Nothing
			c_Button = Nothing
			c_ListView = Nothing
			c_Label = Nothing
			c_HTML_editor = Nothing

			MyBase.Dispose()

			Disposed = True
        End If
    End Sub

    Public Function FIELD_CLEAR() As Boolean
        Dim OUT As Boolean = False
        Dim DataBinded_Old As Boolean = FP.DATA_Binded

        FP.DATA_Binded = False
        If Not (c Is Nothing) Then
            If TypeOf (c) Is TextBox Then
                c.Text = ""
                Selected_ID = 0
                OUT = True

            ElseIf TypeOf (c) Is ComboBox Then
                OUT = FP.FPf.FPApp.COMBOBOX_FIND(c, "")
            ElseIf TypeOf (c) Is RichTextBox Then
                CType(c, RichTextBox).Text = ""
                OUT = True
            ElseIf TypeOf (c) Is CheckBox Then
                CType(c, CheckBox).Checked = False
                OUT = True
            ElseIf TypeOf (c) Is Label Then
                'Nothing to do
                OUT = True
            ElseIf TypeOf (c) Is TabControl Then
                'Nothing to do
                OUT = True
            ElseIf TypeOf (c) Is TabPage Then
                'Nothing to do
                OUT = True
            ElseIf TypeOf (c) Is ListView Then
                LISTVIEW_UNCHECK_ALL()
                OUT = True
            ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
                CType(c, MSDN.Html.Editor.HtmlEditorControl).InnerHtml = ""
                OUT = True
            Else
                FP.FPf.FPApp.DoErrorMsgBox("FP_Control.FIELD_CLEAR", 0, String.Format("Unknown fieldtype of field {0}.", FieldName))
            End If
        End If

        FP.DATA_Binded = DataBinded_Old
        Value_Validated = True

        FIELD_CLEAR = OUT
    End Function
    Public Function FIELD_IsDirty() As Boolean
        Dim OUT As Boolean = False
        Dim FormattedValue As String = ""

        If Can_Dirty() Then
            If Value_Validated = False Then
                OUT = True
            Else
                If Not GET_DBFORMAT_from_CONTROL(FormattedValue) Then
                    OUT = True
                Else
                    OUT = (FormattedValue <> OldValue Or Selected_ID <> OldValue_Selected_ID)

                    'Ha NoShow0 es megis van a mezoben valami, akkor IsDirty = True
                    If OUT = False Then
                        If (P.xType_VB = "INT" Or P.xType_VB = "FLOAT") And F_Format_NoShow0 Then
                            If FormattedValue = "0" Then
                                If c.Text > "" Then
                                    OUT = True
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        FIELD_IsDirty = OUT
    End Function
    Public Sub SET_CONTROLS(ByVal MyC As Control, ByVal MyC_Label As Label, MyFieldName As String, MyLabelName As String)
        c = MyC
        FieldName = MyFieldName
        c_Label = MyC_Label
        LabelName = MyLabelName

        If TypeOf (c) Is CheckBox Then
            c_ChkBox = c
        Else
            c_ChkBox = Nothing
        End If

        If TypeOf (c) Is ComboBox Then
            c_ComboBox = c
        Else
            c_ComboBox = Nothing
        End If

        If TypeOf (c) Is Button Then
            c_Button = c
        Else
            c_Button = Nothing
        End If

        If TypeOf (c) Is ListView Then
            c_ListView = c
            c_ListView.OwnerDraw = True
        Else
            c_ListView = Nothing
        End If

        If TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
            c_HTML_editor = c
        Else
            c_HTML_editor = Nothing
        End If
    End Sub

    Public Sub DT_REFRESH(Optional REFRESH_GRID_COLUMN_ALSO As Boolean = True)
        If Not (c Is Nothing) Then
            Dim Old_P_Value As String = P_VALUE
            Dim DATA_Binded_OldValue As Boolean = FP.DATA_Binded

            FP.DATA_Binded = False

            If TypeOf (c) Is ComboBox Then
                FP.FPf.FPApp.COMBOBOX_INIT_FROM_FIXTEXT(Me, P.DT_FixText_Key, DT, P.DT_WHERE2)
                If REFRESH_GRID_COLUMN_ALSO Then
                    If FP.GRID_EXISTS Then
                        FP.GRID.REFRESH()
                    End If
                End If
                P_VALUE = Old_P_Value

            ElseIf TypeOf (c) Is ListView Then
                Dim LV_Params As New Struct_LISTVIEW_Params
                With LV_Params
                    .ListViewControl = c
                    .FixText_Key = P.DT_FixText_Key
                    .WHERE2 = P.DT_WHERE2
                End With

                FP.LISTVIEW_INIT_FROM_FIXTEXT(LV_Params)

                Refresh_Type = LV_Params.REFRESH_Type
                Dim MySQL As String = FP.LISTVIEW_SQL_get(LV_Params, Me)
                FP.FPf.FPApp.DC.Qdf_Fill_DT(MySQL, DT)
                FP.LISTVIEW_FILL(LV_Params, DT)

                If c_ListView.CheckBoxes = False Then
                    P_VALUE = Old_P_Value
                End If
			Else
                FP.FPf.FPApp.DoErrorMsgBox("FP_Control.DT_REFRESH", 0, String.Format("This function works only on comboboxes and listboxes. (FieldName = '{0}'", FieldName))
            End If

            FP.DATA_Binded = DATA_Binded_OldValue
        End If
    End Sub

    Public Function Tag(ByVal Key As String) As String
        Tag = FP.FPf.FPApp.FIXTEXT_getParam(Key, Tag_DIC)
    End Function
    Protected Friend Function SET_SELECT_Control(ByVal Field_Props As Struct_FP_CONTROL_PROPS) As Boolean
        If Field_Props.DT_FixText_Key.ToUpper.Trim.IndexOf("@@VB_SELECT_CONTROL") = 0 Then
            Dim FpSC As New FP_SELECT_CONTROL(Me)
        End If
    End Function

    Protected Friend Function SET_PARAMS(ByVal Field_Props As Struct_FP_CONTROL_PROPS) As Boolean
        Dim Old_Data_Binded As Boolean = FP.DATA_Binded

        FP.DATA_Binded = False

        With Field_Props
            P.Visible = .Visible
            If Not (c Is Nothing) Then
                c.Visible = P.Visible
            End If
            If Not (c_Label Is Nothing) Then
                c_Label.Visible = P.Visible
            End If

            P.Mandatory = .Mandatory
            P.Locked = .Locked
            P.xType_VB = .xType_VB

            If P.xType_VB = "" Then
                If .xlength > 0 Then
                    P.xlength = .xlength
                Else
                    If Not (c Is Nothing) Then
                        Dim wProps As Struct_DATA_Field_Props = FP.DATA_Field_getProps(FieldName)
                        If wProps.xLength <> 0 Then
                            P.xlength = wProps.xLength
                        End If
                    End If
                End If
            End If

            P.F_Format = .F_Format
            P.COLOR_NORMAL_BG = .COLOR_NORMAL_BG
            P.COLOR_NORMAL_FORE = .COLOR_NORMAL_FORE
            P.COLOR_SELECTED_FORE = .COLOR_SELECTED_FORE
            P.COLOR_LABEL_BG = .COLOR_LABEL_BG
            P.COLOR_LABEL_FORE = .COLOR_LABEL_FORE

            P.BG_Image_Name = .BG_Image_Name
            If P.BG_Image_Name > "" Then
                FP.FPf.FPApp.BACKGROUND_SET(c, P.BG_Image_Name)
                If TypeOf (c) Is Button Then
                    c_Button_RoundShape_SET()
                End If
            End If

            P.Label_Text = .Label_Text
            P.ShowInGRID = .ShowInGRID

            P.SavePoint = .SavePoint
            If P.SavePoint = ENUM_SavePoint_Type.ON_GOTFOCUS And CreatedAtRuntime Then
                c.SendToBack()
            End If
            P.Forced_NextField = .Forced_NextField
            P.Tag = .Tag

            FP.FPf.FPApp.FIXTEXT_SPLIT_PARAMS(P.Tag, Tag_DIC)

            If P.DT_FixText_Key <> .DT_FixText_Key Or P.DT_WHERE2 <> .DT_WHERE2 Or P.DT_ID_Field <> .DT_ID_Field Then
                P.DT_FixText_Key = .DT_FixText_Key
                P.DT_WHERE2 = .DT_WHERE2
                P.DT_ID_Field = .DT_ID_Field

                If TypeOf (c) Is ComboBox Then
                    FP.FPf.FPApp.COMBOBOX_INIT_FROM_FIXTEXT(Me, P.DT_FixText_Key, DT, P.DT_WHERE2)
                ElseIf TypeOf (c) Is ListView Then
                    Dim ListView_Params As New Struct_LISTVIEW_Params

                    With ListView_Params
                        .ListViewControl = c
                        .FixText_Key = P.DT_FixText_Key
                        .WHERE2 = P.DT_WHERE2
                    End With

                    P.DT_ID_Field = ""

                    If FP.LISTVIEW_INIT_FROM_FIXTEXT(ListView_Params) Then
                        Refresh_Type = ListView_Params.REFRESH_Type
                        If c_ListView.CheckBoxes <> ListView_Params.CheckBoxes Then
                            c_ListView.CheckBoxes = ListView_Params.CheckBoxes
                        End If

                        If P.xType_VB <> "INT" Then
                            FPf.FPApp.DoErrorMsgBox("FP_Control.SET_PARAMS", 0, String.Format("ListView mezo tipusa kizarolag INT lehet. (mezo neve: '{0}')", FieldName, ListView_Params.ValueMember))
                        End If
                        If P.xType_VB <> "INT" Then
                            FPf.FPApp.DoErrorMsgBox("FP_Control.SET_PARAMS", 0, String.Format("A(z) '{0}' ListView VALUEMEMBER-kent megadott '{1}' mezoje nem INT tipusu.", FieldName, ListView_Params.ValueMember))
                        Else
                            P.DT_ID_Field = ListView_Params.ValueMember

                            Dim MySQL As String = FP.LISTVIEW_SQL_get(ListView_Params, Me)

                            MySQL = FP.Text_Replace_Standard_Params(MySQL)

                            FP.FPf.FPApp.DC.Qdf_Fill_DT(MySQL, DT)
                            FP.LISTVIEW_FILL(ListView_Params, DT)
                        End If
                    End If
                End If
            End If
        End With

        F_Format_TRIM = (InStr(P.F_Format.ToUpper, "|TRIM|") > 0)
        F_Format_UCASE = (InStr(P.F_Format.ToUpper, "|UCASE|") > 0)
        F_Format_NOSPACE = (InStr(P.F_Format.ToUpper, "|NOSPACE|") > 0)
        F_Format_MinusAllowed = (InStr(P.F_Format, "|+/-|") > 0)
        F_Format_NoShow0 = (InStr(P.F_Format, "|NOSHOW0|") > 0)
        If InStr(P.F_Format, "|FORMAT('") > 0 Then
            Dim p0 As Integer = InStr(P.F_Format.ToUpper, "|FORMAT('") + 9
            Dim p1 As Integer = InStr(p0 + 1, P.F_Format, "')|")

            If p1 > p0 Then
                F_Format_FORMAT = Mid(P.F_Format, p0, p1 - p0)
            Else
                FP.FPf.FPApp.DoErrorMsgBox("FP_Control.SET_PARAMS", 0, String.Format("Parameter 'F_Format' for Field '{0} is invalid.'", FieldName))
                F_Format_FORMAT = ""
            End If
        End If

        If F_Format_FORMAT = "" Then
            If TypeOf (c) Is TextBox Then
                Select Case P.xType_VB
                    Case "FLOAT"
                        F_Format_FORMAT = "N2"

                    Case "INT"
                        F_Format_FORMAT = "N0"

                    Case Else
                        'Nothing to do
                End Select
            End If
        End If

        If InStr(P.F_Format, "|ALIGN(") > 0 Then
            Dim p0 As Integer = InStr(P.F_Format.ToUpper, "|ALIGN(") + 7
            Dim p1 As Integer = InStr(p0 + 1, P.F_Format, ")|")

            If p1 > p0 Then
                F_Format_ALIGN = Mid(P.F_Format, p0, p1 - p0)
            Else
                FP.FPf.FPApp.DoErrorMsgBox("FP_Control.SET_PARAMS", 0, String.Format("Parameter 'F_Format' for Field '{0} is invalid.'", FieldName))
                F_Format_ALIGN = ""
            End If
        End If

        If InStr(P.F_Format, "|LABEL_ALIGN(") > 0 Then
            Dim p0 As Integer = InStr(P.F_Format.ToUpper, "|LABEL_ALIGN(") + 13
            Dim p1 As Integer = InStr(p0 + 1, P.F_Format, ")|")

            If p1 > p0 Then
                F_Format_LABEL_ALIGN = Mid(P.F_Format, p0, p1 - p0)
            Else
                FP.FPf.FPApp.DoErrorMsgBox("FP_Control.SET_PARAMS", 0, String.Format("Parameter 'F_Format' for Field '{0} is invalid.'", FieldName))
                F_Format_LABEL_ALIGN = ""
            End If
        End If

        If Not (c_Label Is Nothing) Then
            c_Label.Text = P.Label_Text
            c_Label.TabIndex = 9999

            If F_Format_LABEL_ALIGN > "" Then
                Select Case F_Format_LABEL_ALIGN
                    Case "L" : c_Label.TextAlign = ContentAlignment.MiddleLeft
                    Case "R" : c_Label.TextAlign = ContentAlignment.MiddleRight
                    Case "C" : c_Label.TextAlign = ContentAlignment.MiddleCenter
                    Case Else
                        FP.FPf.FPApp.DoErrorMsgBox("FP_Control.SET_PARAMS", Err.Number, String.Format("Unknown ALIGN Setting for label '{0}'.", c_Label.Name))
                End Select
            End If

            c_Label.BackColor = P.COLOR_LABEL_BG
            c_Label.ForeColor = P.COLOR_LABEL_FORE
        End If

        If Not (c Is Nothing) Then
            If F_Format_ALIGN > "" Then
                If TypeOf (c) Is TextBox Then
                    Select Case F_Format_ALIGN
                        Case "L" : CType(c, TextBox).TextAlign = HorizontalAlignment.Left
                        Case "R" : CType(c, TextBox).TextAlign = HorizontalAlignment.Right
                        Case "C" : CType(c, TextBox).TextAlign = HorizontalAlignment.Center
                        Case Else
                            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.SET_PARAMS", Err.Number, String.Format("Unknown ALIGN Setting for field '{0}'.", FieldName))
                    End Select
                ElseIf TypeOf (c) Is Label Then
                    Select Case F_Format_ALIGN
                        Case "L" : CType(c, Label).TextAlign = ContentAlignment.MiddleLeft
                        Case "R" : CType(c, Label).TextAlign = ContentAlignment.MiddleRight
                        Case "C" : CType(c, Label).TextAlign = ContentAlignment.MiddleCenter
                        Case Else
                            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.SET_PARAMS", Err.Number, String.Format("Unknown ALIGN Setting for field '{0}'.", FieldName))
                    End Select
                Else
                    FP.FPf.FPApp.DoErrorMsgBox("FP_Control.SET_PARAMS", 0, String.Format("TextAlign of field '{0}' could not set. (Align text is only for type textbox and label possible)", FieldName))
                End If
            Else
                If P.xType_VB = "INT" Or P.xType_VB = "FLOAT" Then
                    If TypeOf c Is TextBox Then
                        CType(c, TextBox).TextAlign = HorizontalAlignment.Right
                    End If
                End If
            End If
        End If

        OLDVALUE_SET_FROM_CURRENT_VALUE()

        FP.DATA_Binded = Old_Data_Binded

        SET_PARAMS = True
    End Function

    Public Function GET_DBFORMAT_from_CONTROL(ByRef OUT_FormattedValue As String) As Boolean
        Dim OUT As Boolean = True
        OUT_FormattedValue = ""

        If TypeOf (c) Is System.Windows.Forms.ComboBox Then
            With (CType(c, System.Windows.Forms.ComboBox))
                If .SelectedValue Is Nothing Then
                    OUT_FormattedValue = c_ComboBox_Value.ToString
                    'OUT_FormattedValue = "0"
                Else
                    OUT_FormattedValue = .SelectedValue.ToString
                End If
            End With

        ElseIf TypeOf (c) Is ListView Then
            If c_ListView.CheckBoxes = True Then
                OUT_FormattedValue = "0"
            Else
                If P.DT_ID_Field = "" Then
                    OUT_FormattedValue = "0"
                Else
                    With (CType(c, ListView))
                        If (.FocusedItem Is Nothing) Then
                            OUT_FormattedValue = "0"
                        Else
                            OUT_FormattedValue = .FocusedItem.Name
                        End If
                    End With
                End If
            End If

        ElseIf TypeOf (c) Is System.Windows.Forms.CheckBox Then
            OUT_FormattedValue = IIf(CType(c, System.Windows.Forms.CheckBox).Checked, "1", "0")

		ElseIf TypeOf (c) Is RadioButton Then
            OUT_FormattedValue = IIf(CType(c, RadioButton).Checked, "1", "0")

		ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
            OUT_FormattedValue = nz(CType(c, MSDN.Html.Editor.HtmlEditorControl).InnerHtml, "")

		Else
            Select Case P.xType_VB
                Case "" : OUT_FormattedValue = c.Text
                Case "DATETIME"
                    Dim wDate As DateTime

                    If Not getDateFromStr(c.Text, wDate) Then
                        OUT = False
                        OUT_FormattedValue = ""
                    Else
                        OUT_FormattedValue = DBFORMAT_get_DbStr_From_Date(wDate)
                    End If

                Case "INT"
                    If TypeOf (c) Is System.Windows.Forms.ComboBox Then
                        With CType(c, System.Windows.Forms.ComboBox)
                            If (.SelectedValue Is Nothing) Then
                                OUT_FormattedValue = c_ComboBox_Value.ToString
                            Else
                                OUT_FormattedValue = (CType(c, System.Windows.Forms.ComboBox).SelectedValue).ToString
                            End If
                        End With
                    ElseIf TypeOf (c) Is System.Windows.Forms.CheckBox Then
                        OUT_FormattedValue = IIf(CType(c, System.Windows.Forms.CheckBox).Checked, "1", "0")
                    Else
                        OUT_FormattedValue = (ValLong(c.Text)).ToString
                    End If

                Case "FLOAT"
                    'Dim CurrentText As String = c.Text
                    'Dim pPoint = InStrRev(CurrentText, ".")
                    'Dim pComma = InStrRev(CurrentText, ",")

                    'If pPoint > 0 And pComma > 0 Then
                    '    If pPoint > pComma Then
                    '        CurrentText = Replace(CurrentText, ",", "")
                    '    Else
                    '        CurrentText = Replace(CurrentText, ".", "")
                    '        CurrentText = Replace(CurrentText, ",", ".")
                    '    End If
                    'ElseIf pPoint = 0 And pComma > 0 Then
                    '    CurrentText = Replace(CurrentText, ",", ".")
                    'End If

                    'OUT_FormattedValue = Trim(Replace(Str(ValDbl(CurrentText)), ",", "."))
                    OUT_FormattedValue = Trim(Replace(Str(ValDbl(c.Text)), ",", "."))

                Case "BIT"
                    If TypeOf (c) Is System.Windows.Forms.CheckBox Then
                        OUT_FormattedValue = IIf(CType(c, System.Windows.Forms.CheckBox).Checked, "1", "0")
                    Else
                        OUT_FormattedValue = IIf(Val(c.Text) <> 0, "1", "0")
                    End If

                Case Else
                    FP.FPf.FPApp.DoErrorMsgBox("FP.DBFORMAT_from_CONTROL", 0, String.Format("Field {0} has an unknown datatype ({1})", FieldName, P.xType_VB))
            End Select
        End If

        GET_DBFORMAT_from_CONTROL = OUT
    End Function

    Public Function SET_VALUE_from_DBFORMAT(ByVal DbFormattedValue As String, Optional ByVal SetOldValue As Boolean = True) As Boolean
        Dim OUT As Boolean = True
        Dim Data_Binded_OLD As Boolean = FP.DATA_Binded

        FP.DATA_Binded = False

        If Not (c Is Nothing) Then
            Select Case P.xType_VB
                Case ""
                    If TypeOf (c) Is TextBox Then
                        c.Text = DbFormattedValue
                    ElseIf TypeOf (c) Is RichTextBox Then
                        With CType(c, System.Windows.Forms.RichTextBox)
                            .Rtf = DbFormattedValue
                        End With
                    ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
                        With CType(c, MSDN.Html.Editor.HtmlEditorControl)
                            If TEXT_Is_HTML(DbFormattedValue) Then
                                .InnerHtml = DbFormattedValue
                            Else
                                .InnerText = DbFormattedValue
                            End If
                        End With
                    ElseIf TypeOf (c) Is ComboBox Then
                        If Not DT Is Nothing Then
                            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.SET_VALUE_from_DBFORMAT", 0, String.Format("XType_VB of field '{0}' is invalid", FieldName))
                        Else
                            'nothing to do
                        End If
                    Else
                        FP.FPf.FPApp.DoErrorMsgBox("FP_Control.SET_VALUE_from_DBFORMAT", 0, String.Format("Field '{0}' has an unknown controltype", FieldName))
                    End If

                Case "DATETIME"
                    If Not (TypeOf (c) Is System.Windows.Forms.TextBox) Then
                        FP.FPf.FPApp.DoErrorMsgBox("FP_Control.SET_VALUE_from_DBFORMAT", 0, String.Format("XType_VB=DATETIME is only for controls available. (Wrong field is: '{0}')", FieldName))
                    Else
                        c.Text = getStrDate(DBFORMAT_get_Date_From_DbStr(DbFormattedValue))
                    End If
                Case "INT"
                    If TypeOf (c) Is System.Windows.Forms.ComboBox Then
                        c_ComboBox_Value = Val(DbFormattedValue)
                        FP.FPf.FPApp.COMBOBOX_FIND(c, DT, CType(c, System.Windows.Forms.ComboBox).ValueMember, c_ComboBox_Value)
                    ElseIf TypeOf (c) Is ListView Then
                        If c_ListView.CheckBoxes = True Then
                            FPf.FPApp.DoErrorMsgBox("FP_Control.SET_VALUE_from_DBFORMAT", 0, String.Format("Checkbox-os listview-nak nem adhato ertek. (controlname: {0})", FieldName))
                        Else
                            FP.LISTVIEW_FIND(c_ListView, DbFormattedValue)

                            Dim ee As New System.EventArgs

                            c_ListView_SelectedIndexChanged(c_ListView, ee)
                        End If

                    ElseIf TypeOf (c) Is System.Windows.Forms.CheckBox Then
                        CType(c, System.Windows.Forms.CheckBox).Checked = (Val(DbFormattedValue) <> 0)
                    Else
                        If c.Focused Then
                            If ValLong(DbFormattedValue) = 0 Then
                                If F_Format_NoShow0 Then
                                    c.Text = ""
                                Else
                                    c.Text = getStrInt(Val(DbFormattedValue))
                                End If
                            Else
                                c.Text = getStrInt(Val(DbFormattedValue))
                            End If
                        Else
                            If ValLong(DbFormattedValue) = 0 Then
                                If F_Format_NoShow0 Then
                                    c.Text = ""
                                Else
                                    c.Text = Format(ValLong(DbFormattedValue), F_Format_FORMAT)
                                End If
                            Else
                                c.Text = Format(ValLong(DbFormattedValue), F_Format_FORMAT)
                            End If
                        End If
                    End If
                Case "FLOAT"
                    If Not (TypeOf (c) Is System.Windows.Forms.TextBox) Then
                        FP.FPf.FPApp.DoErrorMsgBox("FP_Control.SET_VALUE_from_DBFORMAT", 0, String.Format(": xType_VB of field '{0}' is invalid", FieldName))
                    Else
                        If c.Focused Then
                            If Val(DbFormattedValue) = 0 Then
                                If F_Format_NoShow0 Then
                                    c.Text = ""
                                Else
                                    c.Text = Format(Val(DbFormattedValue), F_Format_FORMAT)
                                End If
                            Else
                                c.Text = getStrFloat(Val(DbFormattedValue))
                            End If
                        Else
                            If Val(DbFormattedValue) = 0 Then
                                If F_Format_NoShow0 Then
                                    c.Text = ""
                                Else
                                    c.Text = Format(Val(DbFormattedValue), F_Format_FORMAT)
                                    'c.Text = getStrFloat(Val(DbFormattedValue))
                                End If
                            Else
                                c.Text = Format(Val(DbFormattedValue), F_Format_FORMAT)
                                'c.Text = getStrFloat(Val(DbFormattedValue))
                            End If
                        End If
                    End If

                Case "BIT"
                    If TypeOf (c) Is System.Windows.Forms.CheckBox Then
                        CType(c, System.Windows.Forms.CheckBox).Checked = IIf(DbFormattedValue = "1", True, False)
                    ElseIf TypeOf (c) Is RadioButton Then
                        CType(c, RadioButton).Checked = IIf(DbFormattedValue = "1", True, False)
                    ElseIf TypeOf (c) Is System.Windows.Forms.ComboBox Then
                        FP.FPf.FPApp.COMBOBOX_FIND(c, DT, CType(c, System.Windows.Forms.ComboBox).ValueMember, DbFormattedValue)
                    Else
                        c.Text = getStrInt(Val(DbFormattedValue))
                    End If

                Case Else
                    OUT = False
                    FP.FPf.FPApp.DoErrorMsgBox("FP_Control.SET_VALUE_from_DBFORMAT", 0, String.Format("Field {0} has an unknown datatype ({1})", FieldName, P.xType_VB))
            End Select

            If OUT Then
                If SetOldValue Then
                    OLDVALUE_SET_FROM_CURRENT_VALUE()
                End If
            End If
        End If

        FP.DATA_Binded = Data_Binded_OLD
        SET_VALUE_from_DBFORMAT = OUT
    End Function
    Public Function ISEMPTY() As Boolean
        Dim OUT As Boolean = True

        If Not (c_ComboBox Is Nothing) Then
            If (c_ComboBox.SelectedValue Is Nothing) Then
                If c_ComboBox_Value <> 0 Then
                    OUT = False
                End If
            Else
                If TypeOf (c_ComboBox.SelectedValue) Is Integer Then
                    If c_ComboBox.SelectedValue <> 0 Then
                        OUT = False
                    End If
                End If
            End If
        ElseIf Not (c_ChkBox Is Nothing) Then
            If Not c_ChkBox.CheckState = CheckState.Indeterminate Then
                OUT = False
            End If
        ElseIf TypeOf (c) Is ListView Then
            If Not (c_ListView.FocusedItem Is Nothing) Then
                If c_ListView_LastSelected_Item_IDX > -1 Then
                    OUT = False
                End If
            End If
        ElseIf TypeOf (c) Is Button Then
            OUT = False
        ElseIf Trim(c.Text) > "" Then
            OUT = False
        End If

        ISEMPTY = OUT
    End Function
    Public Sub COLORING()
        Dim Handled As Boolean = False

        If Not (c Is Nothing) Then
            FP.RAISEEVENT_Form_Field_Coloring(Me, Handled)
            If Not Handled Then
                RaiseEvent Field_Coloring(Me, Handled)
            End If

            If Not Handled Then
                If Not (TypeOf (c) Is Button) And Not (TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl) Then
                    If c.Focused Then
                        c.BackColor = COLORS_FIELD_CURRENT_BG
                    Else
                        If P.Mandatory And ISEMPTY() Then
                            c.BackColor = COLORS_FIELD_INVALID_BG
                        Else
                            If P.COLOR_NORMAL_BG <> COLORS_FIELD_NORMAL_BG Then
                                c.BackColor = P.COLOR_NORMAL_BG
                            Else
                                If P.ShowInGRID Then
                                    If P_Locked Then
                                        c.BackColor = COLORS_FIELD_LOCKED_BG
                                    Else
                                        c.BackColor = COLORS_GRID_SELECTEDROW_BG
                                    End If
                                ElseIf P_Locked Then
                                    c.BackColor = COLORS_FIELD_LOCKED_BG
                                Else
                                    c.BackColor = P.COLOR_NORMAL_BG
                                End If
                            End If
                        End If
                    End If

                    If c.Focused Then
                        c.ForeColor = P.COLOR_SELECTED_FORE
                    Else
                        If P.Mandatory And ISEMPTY() Then
                            c.ForeColor = COLORS_FIELD_NORMAL_FORE
                        Else
                            If P.COLOR_NORMAL_FORE <> COLORS_FIELD_NORMAL_FORE Then
                                c.ForeColor = P.COLOR_NORMAL_FORE
                            Else
                                If P.ShowInGRID Then
                                    'Nothing to do
                                ElseIf P_Locked Then
                                    'Nothing to do
                                Else
                                    'Nothing to do
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Public Property P_Locked As Boolean
        Get
            Dim OUT As Boolean = P.Locked

            If OUT = False Then
                Select Case FP.P_DATA_RecordStatus
                    Case ENUM_RecordStatus.NEWRECORD
                        OUT = (FP.P_FORM_AllowAdditions = False)

                    Case ENUM_RecordStatus.EXISTS
                        OUT = (FP.P_FORM_AllowEdits = False)

                    Case Else
                        OUT = P.Locked
                End Select
            End If

            Return OUT
            Return (P.Locked Or (FP.P_FORM_AllowEdits = False))

        End Get
        Set(value As Boolean)
            P.Locked = value
        End Set
    End Property

    Public Property P_Marker() As ENUM_Markertypes
        Get
            Return Marker
        End Get
        Set(value As ENUM_Markertypes)
            Select Case value
                Case ENUM_Markertypes.None
                    Marker = ENUM_Markertypes.None
                    If Not (FP.FPf.ActiveControl Is Nothing) Then
                        If FP.FPf.ActiveControl.c.Equals(c) Then
                            FPf.Marker_HIDE()
                        End If
                    End If

                Case ENUM_Markertypes.Right_Arrow
                    Marker = ENUM_Markertypes.Right_Arrow
                    If Not (FP.FPf.ActiveControl Is Nothing) Then
                        If FP.FPf.ActiveControl.c.Equals(c) Then
                            FPf.Marker_SHOW(Me)
                        End If
                    End If

                Case ENUM_Markertypes.Padlock_Unlock
                    Marker = ENUM_Markertypes.Padlock_Unlock
                    If Not (FP.FPf.ActiveControl Is Nothing) Then
                        If FP.FPf.ActiveControl.c.Equals(c) Then
                            FPf.Marker_SHOW(Me)
                        End If
                    End If

                Case Else
                    FPf.FPApp.DoErrorMsgBox("FP_Control.P_Marker", 0, String.Format("Unknown Markertype ({0})", value))
            End Select
        End Set
    End Property

    Protected Friend ReadOnly Property P_VALUE_VALIDATED() As Boolean
        Get
            P_VALUE_VALIDATED = Value_Validated
        End Get
    End Property

    Public Property P_ID() As Long
        Get
            Dim OUT As Long = 0

            If Not (c Is Nothing) Then
                If TypeOf (c) Is ComboBox Then
                    OUT = P_VALUE
                ElseIf TypeOf (c) Is ListView Then
                    OUT = P_VALUE
                ElseIf TypeOf (c) Is TextBox Then
                    OUT = Selected_ID
                Else
                    FPf.FPApp.DoErrorMsgBox("FP_Control..P_ID", 0, String.Format("Unknown type of control ('{0}')", c.Name))
                End If
            End If

            Return OUT

        End Get
        Set(value As Long)
            If Not (c Is Nothing) Then
                If TypeOf (c) Is ComboBox Then
                    P_VALUE = value
                ElseIf TypeOf (c) Is ListView Then
                    P_VALUE = value
                ElseIf TypeOf (c) Is TextBox Then
                    Selected_ID = value
                Else
                    FPf.FPApp.DoErrorMsgBox("FP_Control..P_ID", 0, String.Format("Unknown type of control ('{0}')", c.Name))
                End If
            End If
        End Set
    End Property

    Public Property P_ID_SAVED() As Long
        Get
            Dim OUT As Long = 0

            If Not (c Is Nothing) Then
                If TypeOf (c) Is ComboBox Then
                    OUT = P_VALUE_Saved
                ElseIf TypeOf (c) Is ListView Then
                    OUT = P_VALUE_Saved
                ElseIf TypeOf (c) Is TextBox Then
                    If P.DT_ID_Field = "" Then
                        OUT = 0
                    Else
                        If FP.DATA_Field_Exists(P.DT_ID_Field) = False Then
                            OUT = 0
                        Else
                            OUT = Val(FP.DATA_Field_getSavedValue(P.DT_ID_Field))
                        End If
                    End If
                Else
                    FPf.FPApp.DoErrorMsgBox("FP_Control..P_ID_SAVED", 0, String.Format("Unknown type of control ('{0}')", c.Name))
                End If
            End If

            Return OUT

        End Get
        Set(value As Long)
            If Not (c Is Nothing) Then
                If TypeOf (c) Is ComboBox Then
                    P_VALUE = value
                ElseIf TypeOf (c) Is ListView Then
                    P_VALUE = value
                ElseIf TypeOf (c) Is TextBox Then
                    Selected_ID = value
                Else
                    FPf.FPApp.DoErrorMsgBox("FP_Control..P_ID_SAVED", 0, String.Format("Unknown type of control ('{0}')", c.Name))
                End If
            End If
        End Set
    End Property


    Public Property P_VALUE() As Object
        Get
            Dim OUT As Object

            Select Case P.xType_VB
                Case "" : OUT = ""
                Case "INT" : OUT = 0
                Case "FLOAT" : OUT = 0.0
                Case "DATETIME" : OUT = NULLDATE
                Case "BIT" : OUT = False
                Case Else
                    OUT = ""
                    FP.FPf.FPApp.DoErrorMsgBox("FP_Control.P_VALUE", 0, String.Format("Unknown xType '{0}'", P.xType_VB))
            End Select

            If FPc_HAS_FIELD(Me) Then
                Dim DB_val As String = ""
                If GET_DBFORMAT_from_CONTROL(DB_val) Then
                    OUT = DBFORMAT_TO_Object(DB_val, P.xType_VB)
                End If
            End If
            P_VALUE = OUT
        End Get
        Set(ByVal value As Object)
            If FPc_HAS_FIELD(Me) Then
                Dim DB_val As String = DBFORMAT_from_OBJECT(value, FieldName, P.xType_VB)
                SET_VALUE_from_DBFORMAT(DB_val)
                Selected_ID = 0
            End If
        End Set
    End Property

    Public ReadOnly Property P_VALUE_Saved() As Object
        Get
            Dim OUT As Object = CStr("")
            If FPc_HAS_FIELD(Me) Then
                If Not FP.DATA_IsDataField(FieldName) Then
                    OUT = DBFORMAT_TO_Object(FP.DATA_Field_getValue_FromREAD(FieldName), P.xType_VB)

                    'OUT = DBFORMAT_TO_Object("", P.xType_VB)
                    'FP.FPf.FPApp.DoErrorMsgBox("FP_Control.P_VALUE_Saved", 0, String.Format("Field '{0} is no DataField. (The fields of _WRITE SQL Object are only DataFields!)'", FieldName))
                Else
                    OUT = DBFORMAT_TO_Object(FP.DATA_Field_getSavedValue(FieldName), P.xType_VB)
                End If
            End If

            P_VALUE_Saved = OUT
        End Get
    End Property

    Public Property P_BACKGROUNDIMAGE() As String
        Get
            Return P.BG_Image_Name
        End Get
        Set(ByVal value As String)
            Dim asm As Reflection.Assembly = Nothing
            Dim ResourceName As String = ""

            If FPf.FPApp.SKIN_getASM_And_OBJECTNAME(value, asm, ResourceName) Then
                Try
                    P.BG_Image_Name = value
                    FP.FPf.FPApp.BACKGROUND_SET(c, P.BG_Image_Name)
                    If TypeOf (c) Is Button Then
                        c_Button_RoundShape_SET()
                    End If

                Catch ex As Exception
                    FPf.FPApp.DoErrorMsgBox("FP_Control.P_BackgroundImage", 0, String.Format("Image not found '{0}'", ResourceName))
                End Try
            End If
        End Set
    End Property

    Public ReadOnly Property P_Has_SIMPLE_SELECT As Boolean
        Get
            Return (P.DT_FixText_Key > "" And (TypeOf c Is TextBox))
        End Get
    End Property

    Public ReadOnly Property P_IsDataField As Boolean
        Get
            Return FP.DATA_IsDataField(FieldName)
        End Get
    End Property

    Public Property P_TABSTOP() As Boolean
        'Ez a property azert lett bevezetve, mert combobox eseten ha a TabStop property valtozik, akkor kijelolodik a combo-ban a szoveg
        'es ez zavaro lehet. Ezt a mellekhatast kuszoboli ki ez a property
        Get
            If c Is Nothing Then
                P_TABSTOP = False
            Else
                P_TABSTOP = c.TabStop
            End If
        End Get
        Set(ByVal value As Boolean)
            If Not (c Is Nothing) Then
                If TypeOf (c) Is ComboBox Then
                    c.TabStop = value
                    If Not c.Focused Then
                        c_ComboBox.SelectionStart = 0
                        c_ComboBox.SelectionLength = 0
                    End If
                Else
                    c.TabStop = value
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property P_Focused() As Boolean
        Get
            Dim OUT As Boolean = False

            If Not (c Is Nothing) Then
                OUT = c.Focused
            End If

            Return OUT
        End Get
    End Property

    Public WriteOnly Property P_VISIBLE() As Boolean
        Set(ByVal value As Boolean)
            If Not (c Is Nothing) Then
                FIELD_VISIBLE(c, value)
                If P.ShowInGRID Then
                    If FP.GRID_EXISTS Then
                        If value = True Then
                            FP.GRID.COLUMNS_SHOW(FieldName)
                        Else
                            FP.GRID.COLUMNS_HIDE(FieldName)
                        End If
                    End If
                End If
            End If
            If Not (c_Label Is Nothing) Then
                If P.ShowInGRID Then
                    FIELD_VISIBLE(c_Label, False)
                Else
                    FIELD_VISIBLE(c_Label, value)
                End If
            End If

            If P_Focused() Then
                If P.xType_VB = "DATETIME" Then
                    FP.FPf.FORM_DATETIME_MonthCalendar_SET(Me)
                End If
            Else
                UnSelectFieldContent()
            End If
            If value Then
                COLORING()
            End If
        End Set
    End Property

    Public Sub UnSelectFieldContent()
        If FPc_HAS_FIELD(Me) Then
            If TypeOf (c) Is TextBox Then
                With CType(c, TextBox)
                    .SelectionStart = 0
                    .SelectionLength = 0
                End With

            ElseIf TypeOf (c) Is ComboBox Then
                With CType(c, ComboBox)
                    If .DrawMode = DrawMode.Normal Then
                        .SelectionStart = 0
                        .SelectionLength = 0
                    End If
                End With

            ElseIf TypeOf (c) Is CheckBox Then
                'Nothing to do

            ElseIf TypeOf (c) Is RadioButton Then
                'Nothing to do

            ElseIf TypeOf (c) Is Button Then
                'Nothing to do

            ElseIf TypeOf (c) Is Panel Then
                'Nothing to do

            ElseIf TypeOf (c) Is TabControl Then
                'Nothing to do

            ElseIf TypeOf (c) Is TabPage Then
                'Nothing to do

            ElseIf TypeOf (c) Is Label Then
                'Nothing to do

            ElseIf TypeOf (c) Is TreeView Then
                'Nothing to do

            ElseIf TypeOf (c) Is RichTextBox Then
                With CType(c, RichTextBox)
                    .SelectionStart = 0
                    .SelectionLength = 0
                End With

            ElseIf TypeOf (c) Is ListView Then
                'Nothing to do

            ElseIf TypeOf (c) Is NumericUpDown Then
                'Nothing to do

            ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
                'Nothing to do

            Else
                FP.FPf.FPApp.DoErrorMsgBox("FP_Control.UnSelectFieldContent", 0, String.Format("Control '{0}' has an unknown controltype", FieldName))
            End If
        End If
    End Sub

    Public Sub SelectEntireField()
        If FPc_HAS_FIELD(Me) Then
            If TypeOf (c) Is TextBox Then
                With CType(c, TextBox)
                    .SelectionStart = 0
                    .SelectionLength = Len(nz(.Text, ""))
                End With

            ElseIf TypeOf (c) Is Button Then
                'Nothing to do

            ElseIf TypeOf (c) Is ComboBox Then
                With CType(c, ComboBox)
                    If .DrawMode = DrawMode.Normal Then
                        .SelectionStart = 0
                        .SelectionLength = Len(nz(.Text, ""))
                    End If
                End With

            ElseIf TypeOf (c) Is CheckBox Then
                'Nothing to do

            ElseIf TypeOf (c) Is TabControl Then
                'Nothing to do

            ElseIf TypeOf (c) Is TabPage Then
                'Nothing to do

            ElseIf TypeOf (c) Is RichTextBox Then
                With CType(c, RichTextBox)
                    .SelectionStart = 0
                    .SelectionLength = Len(nz(.Text, ""))
                End With

            ElseIf TypeOf (c) Is ListView Then
                'Nothing to do

            ElseIf TypeOf (c) Is NumericUpDown Then
                'Nothing to do

            ElseIf TypeOf (c) Is TreeView Then
                'Nothing to do

            ElseIf TypeOf (c) Is RadioButton Then
                'Nothing to do

            ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
                'nothing to do

            Else
                FP.FPf.FPApp.DoErrorMsgBox("FP_Control.SelectEntireField", 0, String.Format("Control '{0}' has an unknown controltype", FieldName))
            End If
        End If
    End Sub
    Public Function IsEntireFieldSelected() As Boolean
        Dim OUT As Boolean = False

        If FPc_HAS_FIELD(Me) Then
            If TypeOf (c) Is TextBox Then
                With CType(c, TextBox)
                    If .SelectionStart <= 0 And .SelectionLength = Len(nz(.Text, "")) Then
                        OUT = True
                    End If
                End With

            ElseIf TypeOf (c) Is ComboBox Then
                With CType(c, ComboBox)
                    If .DrawMode <> DrawMode.Normal Then
                        OUT = True
                    Else
                        If .SelectionStart <= 0 And .SelectionLength = Len(nz(.Text, "")) Then
                            OUT = True
                        End If
                    End If
                End With

            ElseIf TypeOf (c) Is CheckBox Then
                OUT = True

            ElseIf TypeOf (c) Is RichTextBox Then
                With CType(c, RichTextBox)
                    If .SelectionStart <= 0 And .SelectionLength = Len(nz(.Text, "")) Then
                        OUT = True
                    End If
                End With

            ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
                With CType(c, MSDN.Html.Editor.HtmlEditorControl)
                    .TextSelectAll()
                End With

            Else
                FP.FPf.FPApp.DoErrorMsgBox("FP_Control.IsEntireFieldSelected", 0, String.Format("Control '{0}' has an unknown controltype", FieldName))
            End If
        End If

        IsEntireFieldSelected = OUT
    End Function
    Public Sub UNDO()
        If FIELD_IsDirty() Then
            Selected_ID = OldValue_Selected_ID
            SET_VALUE_from_DBFORMAT(OldValue)
        End If
    End Sub

    Private Function setFormattedValueFromEntered() As Boolean
        Dim OUT As Boolean = True
        Dim Converted_Value As String = ""

        If TypeOf (c) Is Windows.Forms.TextBox Then
            Select Case P.xType_VB
                Case ""
                    Converted_Value = c.Text

                Case "DATETIME"
                    If c.Text = "" Then
                        Converted_Value = Nothing
                    Else
                        Dim EnteredDateTime As DateTime

                        If Not getDateFromStr(c.Text, EnteredDateTime) Then
                            OUT = False
                        Else
                            Converted_Value = getStrDate(EnteredDateTime)
                        End If
                    End If

                Case "FLOAT"
                    If c.Text = "" Then
                        Converted_Value = Nothing
                    Else
                        Dim MyFloat As Double

                        If Not getFloatFromStr(c.Text, MyFloat) Then
                            OUT = False
                        Else
                            Converted_Value = getStrFloat(MyFloat)
                        End If
                    End If

                Case "INT"
                    If c.Text = "" Then
                        Converted_Value = Nothing
                    Else
                        Dim MyInt As Long

                        If Not getIntFromStr(c.Text, MyInt) Then
                            OUT = False
                        Else
                            Converted_Value = getStrInt(MyInt)
                        End If
                    End If
            End Select

            If OUT Then
                Dim DATA_Binded_OldValue As Boolean = FP.DATA_Binded

                FP.DATA_Binded = False
                c.Text = Converted_Value
                FP.DATA_Binded = DATA_Binded_OldValue
            End If
        End If

        setFormattedValueFromEntered = OUT
    End Function
    Private Sub UNDO_To_Before_TextChanged()
        Dim Old_P_DATA_Binded As Boolean = FP.P_DATA_Binded

        FP.DATA_Binded = False
        If TypeOf (c) Is RichTextBox Then
            CType(c, RichTextBox).Rtf = Value_Before_TextChanged
        ElseIf TypeOf (c) Is ListView Then
            If c_ListView.CheckBoxes = False Then
                If c_ListView_LastSelected_Item_IDX = -1 Then
                    c_ListView.FocusedItem = Nothing
                Else
                    c_ListView.FocusedItem = c_ListView.Items(c_ListView_LastSelected_Item_IDX)
                End If
            End If
        ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
            With CType(c, MSDN.Html.Editor.HtmlEditorControl)
                .InnerHtml = Value_Before_TextChanged
            End With
        ElseIf TypeOf (c) Is RadioButton Then
            SET_VALUE_from_DBFORMAT(Value_Before_TextChanged, True)
        Else
            c.Text = Value_Before_TextChanged
        End If
        FP.DATA_Binded = Old_P_DATA_Binded
    End Sub
    Private Sub UNDO_To_Saved_Value()
        If FP.DATA_IsDataField(FieldName) Then
            If Not FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                If FIELD_IsDirty() Then
                    Dim Old_P_DATA_Binded As Boolean = FP.P_DATA_Binded

                    FP.DATA_Binded = False
                    If TypeOf (c) Is TextBox Then
                        SET_VALUE_from_DBFORMAT(FP.DATA_Field_getSavedValue(FieldName))
                    ElseIf TypeOf (c) Is ComboBox Then
                        If DT Is Nothing Then
                            FP.FPf.FPApp.COMBOBOX_FIND(c, FP.DATA_Field_getSavedValue(FieldName))
                        Else
                            FP.FPf.FPApp.COMBOBOX_FIND(c, DT, c_ComboBox.ValueMember, FP.DATA_Field_getSavedValue(FieldName))
                        End If
                    ElseIf TypeOf (c) Is ListView Then
                        FP.LISTVIEW_FIND(c_ListView, Val(FP.DATA_Field_getSavedValue(c_ListView.Name)))
                    ElseIf TypeOf (c) Is CheckBox Then
                        c_ChkBox.Checked = (FP.DATA_Field_getSavedValue(FieldName) = "1")
                    ElseIf TypeOf (c) Is RichTextBox Then
                        CType(c, RichTextBox).Rtf = FP.DATA_Field_getSavedValue(FieldName)
                    ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
                        CType(c, MSDN.Html.Editor.HtmlEditorControl).InnerHtml = FP.DATA_Field_getSavedValue(FieldName)
                    Else
                        FP.FPf.FPApp.DoErrorMsgBox("FP_Control.UNDO_To_Saved_Value", 0, String.Format("Unknown type of Control '{0}'", FieldName))
                    End If

                    FP.DATA_Binded = Old_P_DATA_Binded

                End If
            End If
        Else
            UNDO_To_Before_TextChanged()
        End If
    End Sub

#Region "CONTROL_EVENTS"
    Private Sub c_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles c.MouseDown
        If e.Button = MouseButtons.Right Then
            If P.ShowInGRID Then
                If FP.P_DragRow_Allowed Then
                    Dim p As Point = FP.GRID.GRID.PointToClient(c.PointToScreen(New Point(e.X, e.Y)))

                    Dim ee As New System.Windows.Forms.MouseEventArgs(MouseButtons.Right, e.Clicks, p.X, p.Y, e.Delta)

                    FP.GRID.EVENT_GRID_MOUSEDOWN(FP.GRID.GRID, ee)
                End If
            End If
        End If
    End Sub

    Public Sub EVENT_MOUSEWHEEL(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)     'Az FP_Form valtja ki es nem a control maga mostantol  Handles c.MouseWheel
        If Disposed Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.EVENT_MOUSEWHEEL", 0, "Control recieves events but it is disposed!")
        Else
            Dim Handled As Boolean = False

            RaiseEvent Field_MouseWheel(Me, sender, e, Handled)
            If Not Handled Then
                If P.ShowInGRID Then
                    FP.RAISEEVENT_GRID_Row_MouseWheel(sender, e, Handled)
                End If
            End If
            If Not Handled Then
                If P.ShowInGRID = True Then
                    Dim CtrlPressed As Boolean = My.Computer.Keyboard.CtrlKeyDown
                    Dim MoveRow As Boolean = CtrlPressed And Not (FP.Button_Down Is Nothing) And Not (FP.Button_Up Is Nothing)
                    Dim IsComboBox As Boolean = (TypeOf (c) Is ComboBox)
                    Dim DoIt As Boolean = True

                    If IsComboBox Then
                        DoIt = (c_ComboBox.DroppedDown = False)
                    End If

                    If DoIt Then
                        If FP.FORM_RECORDS_SAVE_CURRENT Then
                            Dim i As Integer
                            Dim WheelCount = e.Delta / 120

                            If WheelCount > 0 Then
                                For i = 1 To WheelCount
                                    If MoveRow Then
                                        If Not FP.FORM_RECORD_UPDOWN(ENUM_UpDown.UP) Then
                                            Exit For
                                        End If
                                    Else
                                        If Not FP.FORM_GOTO_PREVIOUSRECORD() Then
                                            Exit For
                                        End If
                                    End If
                                Next
                            Else
                                For i = -1 To WheelCount Step -1
                                    If MoveRow Then
                                        If Not FP.FORM_RECORD_UPDOWN(ENUM_UpDown.DOWN) Then
                                            Exit For
                                        End If
                                    Else
                                        If Not FP.FORM_GOTO_NEXTRECORD() Then
                                            Exit For
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub EVENT_TEXTCHANGED(ByVal sender As Object, ByVal e As System.EventArgs) Handles c.TextChanged
        If Disposed Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.EVENT_TEXTCHANGED", 0, "Control recieves events but it is disposed!")
        Else
            If FP.UnboundForm Then
                If FP.P_DATA_Binded Then
                    Dim Was_Undo As Boolean = False

                    'Locked ellenorzese
                    If Not Was_Undo Then
                        If c.Focused Then
                            If P.Locked Then
                                UNDO_To_Before_TextChanged()
                                Was_Undo = True
                            End If
                        End If
                    End If

                    'Field length ellenorzese
                    If Not Was_Undo Then
                        If P.xType_VB = "" Then
                            If Not (TypeOf (c) Is ComboBox) Then
                                If P.xlength > 0 Then
                                    Dim Current_Length As Integer = 0
                                    If TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
                                        Current_Length = CType(c, MSDN.Html.Editor.HtmlEditorControl).InnerHtml.Length
                                    Else
                                        Current_Length = c.Text.Length
                                    End If

                                    If Current_Length > P.xlength Then
                                        UNDO_To_Before_TextChanged()

                                        If TypeOf (c) Is RichTextBox Then
                                            CType(c, RichTextBox).SelectionStart = Len(Value_Before_TextChanged)
                                            CType(c, RichTextBox).SelectionLength = 0
                                        ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
                                            'Nothing to do
                                        ElseIf TypeOf (c) Is Label Then
                                            'Nothing to do
                                        ElseIf TypeOf (c) Is ComboBox Then
                                            'Nothing to do
                                        Else
                                            CType(c, TextBox).SelectionStart = Len(Value_Before_TextChanged)
                                            CType(c, TextBox).SelectionLength = 0
                                        End If

                                        Was_Undo = True
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If Not Was_Undo Then
                        Dim Cancel As Boolean = False

                        If Not (TypeOf (c) Is Button Or TypeOf (c) Is ComboBox Or TypeOf (c) Is CheckBox Or TypeOf (c) Is Label) Then
                            RaiseEvent Field_TextChanged(Me, sender, e, Cancel)
                        End If

                        If Cancel Then
                            UNDO_To_Before_TextChanged()
                            Was_Undo = True
                        Else
                            If Can_Dirty() Then
                                If CreatedBy <> ENUM_FP_CONTROL_Created_by.GRID Then
                                    Value_Validated = False
                                End If
                            End If

                            Value_Before_TextChanged_SET()
                        End If
                    End If
                End If
            Else
                Dim Was_Undo As Boolean = False

                'Field length ellenorzese
                If Not Was_Undo Then
                    If P.xType_VB = "" Then
                        Dim Current_Length As Integer = 0
                        If TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
                            Current_Length = CType(c, MSDN.Html.Editor.HtmlEditorControl).InnerHtml.Length
                        Else
                            Current_Length = c.Text.Length
                        End If

                        If P.xlength > 0 Then
                            If Current_Length > P.xlength Then
                                UNDO_To_Before_TextChanged()

                                If TypeOf (c) Is RichTextBox Then
                                    CType(c, RichTextBox).SelectionStart = Len(Value_Before_TextChanged)
                                    CType(c, RichTextBox).SelectionLength = 0
                                ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
                                    'Nothing to do
                                ElseIf TypeOf (c) Is Label Then
                                    'Nothing to do
                                Else
                                    CType(c, TextBox).SelectionStart = Len(Value_Before_TextChanged)
                                    CType(c, TextBox).SelectionLength = 0
                                End If

                                Was_Undo = True
                            End If
                        End If
                    End If
                End If

                If FP.P_DATA_Binded Then
                    'Locked ellenorzese
                    If Not Was_Undo Then
                        If c.Focused Then
                            If P.Locked Then
                                UNDO_To_Before_TextChanged()
                                Was_Undo = True
                            End If
                        End If
                    End If

                    'Form_Dirty ellenorzese
                    If Not FP.P_FORM_Dirty Then
                        If FP.DATA_IsDataField(FieldName) Then
                            If FP.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
                                If FIELD_IsDirty() Then
                                    If Not FP.FORM_DIRTY_SET(FieldName) Then
                                        UNDO_To_Saved_Value()
                                        Was_Undo = True
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If Not Was_Undo Then
                        If Can_Dirty() Then
                            If CreatedBy <> ENUM_FP_CONTROL_Created_by.GRID Then
                                Value_Validated = False
                            End If
                        End If
                    End If

                    If Not Was_Undo Then
                        Dim Cancel As Boolean = False
                        RaiseEvent Field_TextChanged(Me, sender, e, Cancel)

                        If Cancel Then
                            UNDO_To_Before_TextChanged()
                            Was_Undo = True
                        Else
                            If FIELD_IsDirty() Then
                                Selected_ID = 0
                            End If

                            Value_Before_TextChanged_SET()
                        End If
                    End If
                End If

                If Not Was_Undo Then
                    If P.DT_ID_Field = "" Then
                        Selected_ID = 0
                    End If
                End If
            End If
        End If
    End Sub

    Public Function GET_TEXT() As String
        Dim OUT As String = ""

        If TypeOf (c) Is RichTextBox Then
            OUT = CType(c, RichTextBox).Rtf
        ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
            OUT = CType(c, MSDN.Html.Editor.HtmlEditorControl).InnerHtml
        Else
            OUT = c.Text
        End If

        Return OUT
    End Function

    Private Sub Value_Before_TextChanged_SET()
        If gl_Data_Binded Then
            If TypeOf (c) Is RichTextBox Then
                Value_Before_TextChanged = CType(c, RichTextBox).Rtf
            ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
                Value_Before_TextChanged = CType(c, MSDN.Html.Editor.HtmlEditorControl).InnerHtml
            ElseIf TypeOf (c) Is ComboBox Then
                Try
                    Value_Before_TextChanged = c.Text
                Catch ex As Exception
                    Value_Before_TextChanged = ""
                End Try
            Else
                Value_Before_TextChanged = c.Text
            End If
        End If
    End Sub

    Private Sub EVENT_CHECKEDCHANGED() Handles c_ChkBox.CheckedChanged
        Dim Was_Undo As Boolean = False

        If FP.P_DATA_Binded Then
            If P.Locked Then
                FP.DATA_Binded = False
                c_ChkBox.Checked = (Not c_ChkBox.Checked)
                FP.DATA_Binded = True
                Was_Undo = True
            Else
                If FP.DATA_IsDataField(FieldName) Then
                    If Not FP.P_FORM_Dirty Then
                        If Not FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                            If c_ChkBox.Checked <> (FP.DATA_Field_getSavedValue(FieldName) = "1") Then
                                If Not FP.FORM_DIRTY_SET(FieldName) Then
                                    FP.DATA_Binded = False
                                    c_ChkBox.Checked = (FP.DATA_Field_getSavedValue(FieldName) = "1")
                                    FP.DATA_Binded = True
                                    Was_Undo = True
                                End If
                            End If
                        End If
                    End If

                    If Was_Undo = False Then
                        Dim ee As New System.EventArgs
                        Dim Cancel As Boolean

                        RaiseEvent Field_TextChanged(Me, c_ChkBox, ee, Cancel)

                        If Cancel Then
                            UNDO()
                            Was_Undo = True
                        End If
                    End If
                Else
                    Dim ee As New System.EventArgs
                    Dim Cancel As Boolean

                    RaiseEvent Field_TextChanged(Me, c_ChkBox, ee, Cancel)

                    If Cancel Then
                        FP.DATA_Binded = False
                        c_ChkBox.Checked = (Not c_ChkBox.Checked)
                        FP.DATA_Binded = True
                        Was_Undo = True
                    End If
                End If
            End If
        End If

        If Not Was_Undo Then
            If FP.P_DATA_Binded Then
                Value_Validated = False
            End If
        End If
    End Sub
    Private Sub EVENT_DOUBLECLICK(ByVal sender As Object, ByVal e As System.EventArgs) Handles c.DoubleClick
        If Disposed Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.DoubleClick", 0, "Control recieves events but it is disposed!")
        Else
            Dim Handled As Boolean = False

            RaiseEvent Field_Doubleclick(Me, sender, e, Handled)

            If Not Handled Then
                If P.ShowInGRID Then
                    FP.RAISEEVENT_GRID_Row_DoubleClick(Handled)
                End If
            End If
        End If
    End Sub

    Protected Friend Sub OLDVALUE_SET_FROM_CURRENT_VALUE()
        If gl_Data_Binded Then
            If FP.UnboundForm Then
                GET_DBFORMAT_from_CONTROL(OldValue)
            Else
                If FP.DATA_IsDataField(FieldName) Then
                    If FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                        OldValue = DBFORMAT_from_OBJECT("", FieldName, P.xType_VB)
                    Else
                        GET_DBFORMAT_from_CONTROL(OldValue)
                    End If
                Else
                    GET_DBFORMAT_from_CONTROL(OldValue)
                End If
            End If

            OldValue_Selected_ID = Selected_ID

            Value_Validated = True
        End If
    End Sub

    Public Sub EVENT_GOTFOCUS() Handles c.GotFocus
        'Tipp 1 debug-ra:
        '----------------
        'Debug.Print(c.Name)

        'Tipp 2 debug-ra:
        '----------------
        'If c.Name = "BTN_ACC_DIM" Then
        'Dim x As Long = 1 'Ide tegyel egy breakpoint-ot!
        'End If

        If P_Disposed = True Then
            Exit Sub
        End If

        If TypeOf (c) Is Button Then
            'Nothing to do
        ElseIf FP.FPf.FORM_DATETIME_MonthCalendar_Manipulated Then
            'Nothing to do
        Else
            If gl_Data_Binded = False Then
                FP.FPf.FORM_DATETIME_MonthCalendar_SET(Me)
                Exit Sub
            End If

            Dim Handled As Boolean = False

            If CreatedBy = ENUM_FP_CONTROL_Created_by.GRID Or FP.FPf.FP_CONTROL_Is_In_Other_FP(Me) Then
                Dim Last_FP As FP = FP.FPf.P_ActiveControl_Last_FP
                If Not (Last_FP Is Nothing) Then
                    If Not FP.FPf.P_ActiveControl_Last_FP.FORM_RECORDS_SAVE_CURRENT() Then
                        If Not (FP.FPf.ActiveControl Is Nothing) Then
                            FP.FPf.FOCUS_ON_AT_THE_END(FP.FPf.ActiveControl.c, , , True)
                        End If

                        Exit Sub 'Mert ha nem sikerul a regi fp mentese, akkor oda vissza fog kerulni a cursor es innentol itt semmit nem kell csinalni.
                    End If
                End If
            End If

            FP.FPf.P_ActiveControl = Me
            If Marker <> ENUM_Markertypes.None Then
                FP.FPf.Marker_SHOW(Me)
            End If

            If Me.Equals(FP.FPf.P_ActiveControl) Then
                If Not FP.FPf.ProcessSavePoint_Active Then
                    FP.RAISEEVENT_Form_Field_Enter(Me, Handled)
                End If

                If Not Handled Then
                    If P.SavePoint <> ENUM_SavePoint_Type.NONE Then
                        If Not FP.FPf.ProcessSavePoint_Active Then
                            If FP.P_DATA_Binded Then
                                'Ha a FOCUS_ON timer aktiv es a SavePoint-ra akarja allitani vegul a focus-t, akkor azt ne tegye, mert a focus mar most itt van
                                If FPf.FOCUS_ON_AT_THE_END_is_on And Not (FPf.FOCUS_ON_AT_THE_END_c Is Nothing) Then
                                    If c.Equals(FPf.FOCUS_ON_AT_THE_END_c) Then
                                        FPf.FOCUS_ON_AT_THE_END_CLEAR()
                                    End If
                                End If
                                If FPf.FOCUS_ON_AT_THE_END_is_on And Not (FPf.FOCUS_ON_AT_THE_END_c Is Nothing) Then
                                    FP.FPf.ProcessSavePoint_Active = True 'Hogy biztos ne fusson le a SavePoint, hiszen el akarjuk venni rola a focus-t (peldaul egy AfterUpdate esemenyben)
                                Else
                                    Select Case P.SavePoint
                                        Case ENUM_SavePoint_Type.ON_GOTFOCUS
                                            FP.FPf.ProcessSavePoint_Active = True

                                            Dim Keyboard_Enabled_OLD As Boolean = FP.FPf.P_Keyboard_Enabled
                                            FP.FPf.P_Keyboard_Enabled = False
                                            Dim SaveOK As Boolean = FP.FPf.SAVE_ALL
                                            FP.FPf.P_Keyboard_Enabled = Keyboard_Enabled_OLD

                                            If SaveOK = False Then
                                                If Not FP.FPf.FOCUS_ON_AT_THE_END_is_on Then
                                                    FP.FPf.ProcessSavePoint_Active = False

                                                    Dim FPc_PrevControl As FP_Control = FPf.P_ActiveControl

                                                    If Not (FPc_PrevControl Is Nothing) Then
                                                        FPf.FOCUS_ON_AT_THE_END(FPc_PrevControl.c, , , True)
                                                    End If
                                                End If
                                            Else
                                                If P.Forced_NextField = "" Then
                                                    Dim FPc_First As FP_Control = FP.CONTROLS_GET_FIRST_FPc

                                                    If FPc_First Is Nothing Then
                                                        FPf.FPApp.DoErrorMsgBox("FP_Control.GotFocus", 0, "SavePoint utan nem sikerult meghatarozni, hogy a kovetkezo rekordon melyik mezore alljak.")
                                                    Else
                                                        FP.FPf.FOCUS_ON_AT_THE_END(FPc_First.c, , , True)
                                                    End If
                                                Else
                                                    If Not FP.FPf.CONTROLS.ContainsKey(FP.P_FieldPrefix + P.Forced_NextField) Then
                                                        FP.FPf.FPApp.DoErrorMsgBox("FP_Control.EVENT_GOTFOCUS", 0, String.Format("Forced_NextField '{0}' for Field '{1}' not found.", FP.P_FieldPrefix + P.Forced_NextField, FieldName))
                                                    Else
                                                        FP.FPf.FOCUS_ON_AT_THE_END(FP.FPf.CONTROLS(FP.P_FieldPrefix + P.Forced_NextField), , , True)
                                                    End If
                                                End If

                                                FP.FORM_GOTO_NEXTRECORD()
                                            End If

                                        Case ENUM_SavePoint_Type.TRANSACT_MODE
                                            If FP.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                                                FP.FPf.ProcessSavePoint_Active = True
                                                FP.FPf.SAVE_ALL()
                                                FP.FPf.ProcessSavePoint_Active = False
                                            End If

                                        Case Else
                                            FP.FPf.FPApp.DoErrorMsgBox("FP_Controls.EVENT_GOTFOCUS", 0, String.Format("SavePoint Type '{0}' for Field '{1}' is unknown.", P.SavePoint.ToString, FieldName))
                                    End Select
                                End If
                            End If
                        End If
                    End If

                    If Not (FP.GRID Is Nothing) Then
                        FP.GRID.PREPARE_EDIT(Me)
                    End If

                    If Value_Validated Then
                        OLDVALUE_SET_FROM_CURRENT_VALUE()
                        Value_Before_TextChanged_SET()
                    End If

                    If TypeOf (c) Is TextBox Then
                        Dim Old_P_DATA_Binded As Boolean = FP.DATA_Binded
                        Dim AktVal As String = ""

                        FP.DATA_Binded = False

                        If GET_DBFORMAT_from_CONTROL(AktVal) Then
                            SET_VALUE_from_DBFORMAT(AktVal, Value_Validated)
                        End If
                        FP.DATA_Binded = Old_P_DATA_Binded
                    ElseIf TypeOf (c) Is RichTextBox Then
                        SelectEntireField()
                    End If

                    If Not (FP.FPf.FPc_DATETIME Is Nothing) Then
                        FP.FPf.FPc_DATETIME = Nothing
                    End If

                    FP.FPf.FORM_DATETIME_MonthCalendar_SET(Me)
                End If

                COLORING()

            End If
        End If
    End Sub

    Public Sub EVENT_LOSTFOCUS() Handles c.LostFocus
        'Tipp 1 debug-ra:
        '----------------
        'Debug.Print(c.Name)

        'Tipp 2 debug-ra:
        '----------------
        'If c.Name = "Rate_Date_DC" Then
        '    Dim x As Long = 1 'Ide tegyel egy breakpoint-ot!
        'End If

        If Marker <> ENUM_Markertypes.None Then
            FP.FPf.Marker_HIDE()
        End If

        UnSelectFieldContent()
        If Not (FP.FPf.ActiveControl Is Nothing) Then
            If FP.FPf.ActiveControl.Equals(Me) Then
                If Not FP.FPf.FORM_DATETIME_MonthCalendar.Focused Then
                    FP.FPf.P_ActiveControl = Nothing
                End If
            End If
        End If

        Select Case P.xType_VB
            Case "DATETIME"
                If Not FP.FPf.FORM_DATETIME_MonthCalendar.Focused Then
                    FP.FPf.FORM_DATETIME_MonthCalendar.Visible = False
                    COLORING()
                End If

            Case "FLOAT", "INT"
                If TypeOf (c) Is TextBox Then
                    Dim Old_DATA_Binded As Boolean = FP.DATA_Binded

                    FP.DATA_Binded = False
                    Dim AktVal As String = ""
                    If GET_DBFORMAT_from_CONTROL(AktVal) Then
                        SET_VALUE_from_DBFORMAT(AktVal, False)
                    End If
                    FP.DATA_Binded = Old_DATA_Binded

                ElseIf TypeOf (c) Is ComboBox Then
                    Dim ComboBox_Text As String = ""

                    Try
                        ComboBox_Text = c_ComboBox.Text
                    Catch ex As Exception
                        'nothing to do
                    End Try

                    If P_VALUE = 0 And ComboBox_Text = "" Then
                        If Not (DT Is Nothing) Then
                            Dim Criteria As String = String.Format("{0} = 0", c_ComboBox.ValueMember)

                            If DT.Select(Criteria).Count = 1 Then
                                Dim OkDisplayText = ""

                                If nz(c_ComboBox.DisplayMember, "") > "" Then
                                    OkDisplayText = DT.Select(Criteria).First.Item(c_ComboBox.DisplayMember)
                                End If

                                If c_ComboBox.Text <> OkDisplayText Then
                                    Dim Old_DATA_Binded As Boolean = FP.DATA_Binded

                                    FP.DATA_Binded = False
                                    c_ComboBox.Text = OkDisplayText
                                    FP.DATA_Binded = Old_DATA_Binded
                                End If
                            End If
                        End If
                    End If
                End If

                COLORING()

            Case Else
                COLORING()
        End Select
    End Sub
    Public Sub EVENT_KEYDOWN(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles c.KeyDown
        If Disposed Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.KeyDown", 0, "Control recieves events but it is disposed!")
        Else
            If Not FP.UnboundForm Then
                If FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                    If Not CreatedBy = ENUM_FP_CONTROL_Created_by.GRID Then
                        e.Handled = True
                    End If
                End If
            End If

            If Not e.Handled Then
                Dim CtrlPressed = My.Computer.Keyboard.CtrlKeyDown

                Dim MoveRow As Boolean = CtrlPressed And Not (FP.Button_Down Is Nothing) And Not (FP.Button_Up Is Nothing)
                Dim ComboBox_DroppedDown As Boolean = False

                If TypeOf (c) Is ComboBox Then
                    ComboBox_DroppedDown = c_ComboBox.DroppedDown
                End If

                RaiseEvent Field_KeyPreview_KeyDown(Me, sender, e)
                If Not e.Handled Then
                    FP.RAISEEVENT_Form_KeyPreview_KeyDown(sender, e)
                End If

                If Not e.Handled Then
                    If TypeOf (c) Is ComboBox Then
                        If ComboBox_DroppedDown Then
                            Select Case e.KeyCode
                                Case Keys.Escape
                                    c_ComboBox.DroppedDown = False
                                    ComboBox_DroppedDown = False
                                    e.Handled = True

                                Case Keys.Enter
                                    c_ComboBox.DroppedDown = False
                                    ComboBox_DroppedDown = False
                                    e.Handled = True

                                Case Else
                                    'Nothing to do
                            End Select

                        Else
                            Select Case e.KeyCode
                                Case Keys.Down
                                    If P.ShowInGRID = False Or e.Alt Then
                                        c_ComboBox.DroppedDown = True
                                        ComboBox_DroppedDown = True
                                        e.Handled = True
                                    End If
                            End Select
                        End If
                    End If
                End If

                If Not e.Handled Then
                    If e.KeyCode = Keys.Escape Then
                        If FIELD_IsDirty() Then
                            UNDO()
                            e.Handled = True
                        ElseIf FP.P_FORM_Dirty Then
                            FP.FPf.UNDO_ALL()
                            OLDVALUE_SET_FROM_CURRENT_VALUE()
                            e.Handled = True
                        End If
                    End If
                End If

                If Not e.Handled Then
                    If e.KeyCode = Keys.Enter Then
                        If P.xType_VB = "DATETIME" Then
                            If P_VALUE = NULLDATE Then
                                e.Handled = True
                                P_VALUE = FPf.FORM_DATETIME_MonthCalendar.SelectionStart
                                Value_Validated = False
                                FPf.FOCUS_ON_AT_THE_END(FPf.CONTROLS_GET_NEXTCONTROL(c))
                            End If
                        End If
                    End If
                End If

                If Not e.Handled Then
                    If FP.FPf.DialogMode Then
                        FP.FPf.DLG_NAVIGATION_ON_KEYDOWN(c, e)
                    End If
                End If

                If Not e.Handled Then
                    If P.ShowInGRID Then
                        If FP.GRID_EXISTS Then
                            If Not ComboBox_DroppedDown Then
                                Select Case e.KeyCode
                                    Case Keys.Up
                                        e.Handled = True
                                        If FP.FORM_RECORDS_SAVE_CURRENT Then
                                            If FPf.FOCUS_ON_AT_THE_END_is_on And Not (FPf.FOCUS_ON_AT_THE_END_c Is Nothing) Then
                                                'nothing to do
                                            Else
                                                If MoveRow Then
                                                    FP.FORM_RECORD_UPDOWN(ENUM_UpDown.UP)
                                                Else
                                                    If Not CtrlPressed Then
                                                        FP.FORM_GOTO_RECORD_GRID_PREVIOUSROW()
                                                    End If
                                                End If
                                            End If
                                        End If

                                    Case Keys.Down
                                        e.Handled = True
                                        If FP.FORM_RECORDS_SAVE_CURRENT Then
                                            If FPf.FOCUS_ON_AT_THE_END_is_on And Not (FPf.FOCUS_ON_AT_THE_END_c Is Nothing) Then
                                                'nothing to do
                                            Else
                                                If MoveRow Then
                                                    FP.FORM_RECORD_UPDOWN(ENUM_UpDown.DOWN)
                                                    e.Handled = True
                                                Else
                                                    If Not CtrlPressed Then
                                                        FP.FORM_GOTO_RECORD_GRID_NEXTROW()
                                                    End If
                                                End If
                                            End If
                                        End If

                                    Case Keys.PageUp
                                        e.Handled = True
                                        If FP.FORM_RECORDS_SAVE_CURRENT Then
                                            If FPf.FOCUS_ON_AT_THE_END_is_on And Not (FPf.FOCUS_ON_AT_THE_END_c Is Nothing) Then
                                                'nothing to do
                                            Else
                                                FP.FORM_GOTO_RECORD_GRID_PREVIOUSPAGE()
                                            End If
                                        End If

                                    Case Keys.PageDown
                                        e.Handled = True
                                        If FP.FORM_RECORDS_SAVE_CURRENT Then
                                            If FPf.FOCUS_ON_AT_THE_END_is_on And Not (FPf.FOCUS_ON_AT_THE_END_c Is Nothing) Then
                                                'nothing to do
                                            Else
                                                FP.FORM_GOTO_RECORD_GRID_NEXTPAGE()
                                            End If
                                        End If

                                    Case Keys.Home
                                        If Not FIELD_IsDirty() And IsEntireFieldSelected() Then
                                            Dim FPc As FP_Control = FP.GRID.get_first_FPc

                                            If Not (FPc Is Nothing) Then
                                                e.Handled = True
                                                FP.GRID.COLUMN_ENSURE_VISIBLE(FPc.FieldName)
                                                FP.FPf.FOCUS_ON_AT_THE_END(FPc.c)
                                            End If
                                        End If

                                    Case Keys.End
                                        If Not FIELD_IsDirty() And IsEntireFieldSelected() Then
                                            Dim FPc As FP_Control = FP.GRID.get_last_FPc

                                            If Not (FPc Is Nothing) Then
                                                e.Handled = True
                                                FP.GRID.COLUMN_ENSURE_VISIBLE(FPc.FieldName)
                                                FP.FPf.FOCUS_ON_AT_THE_END(FPc.c)
                                            End If
                                        End If

                                    Case Keys.Enter
                                        FP.RAISEEVENT_GRID_Row_DoubleClick(e.Handled)
                                End Select
                            End If
                        End If
                    End If
                End If

                If Not e.Handled Then
                    If e.KeyCode = Keys.Right And CtrlPressed Then 'Shift right arrow == insert value from last record
                        e.Handled = True

                        Dim DoThisFunc As Boolean = True

                        If P_IsDataField Then
                            DoThisFunc = FP.FORM_DIRTY_SET
                        End If

                        If DoThisFunc Then
                            If TypeOf (c) Is ComboBox Then
                                FP.FPf.FPApp.COMBOBOX_FIND(c, DT, c_ComboBox.ValueMember, Val(LastRecord_Value), False)
                            ElseIf TypeOf (c) Is ListView Then
                                FP.LISTVIEW_FIND(c, Val(LastRecord_Value), False)
                            ElseIf TypeOf (c) Is TextBox And P.DT_ID_Field > "" Then
                                'Simulation Ctrl-V
                                Dim ee As New Windows.Forms.KeyPressEventArgs(Chr(22))

                                Clipboard_SET_TEXT(LastRecord_Value)

                                EVENT_KEYPRESS(sender, ee)
                            Else
                                SET_VALUE_from_DBFORMAT(LastRecord_Value)
                                Value_Validated = False
                            End If
                        End If
                    End If
                End If

                If Not e.Handled Then
                    If TypeOf (c) Is ComboBox Then
                        With CType(c, ComboBox)
                            Select Case e.KeyCode
                                Case Keys.Delete, Keys.Back
                                    If .SelectionStart > 0 Or .SelectionLength <> Len(nz(.Text, "")) Then
                                        e.Handled = True
                                    Else
                                        'mert ilyenkor nem lep fel a selecteditemchanged esemeny
                                        c.Text = ""
                                    End If

                                Case Keys.Left
                                    If .SelectionStart > 0 Then
                                        .SelectionStart -= 1
                                    End If
                                    .SelectionLength = 0
                                    e.Handled = True

                                Case Keys.Right
                                    If .SelectionStart < c.Text.Length Then
                                        .SelectionStart += 1
                                    End If
                                    .SelectionLength = 0
                                    e.Handled = True

                                Case Keys.F2
                                    .SelectionStart = c.Text.Length
                                    .SelectionLength = 0

                                Case Else
                                    If P.Locked Then
                                        e.Handled = True
                                    End If
                            End Select
                        End With

                    ElseIf TypeOf (c) Is TextBox Then
                        With CType(c, TextBox)
                            Select Case e.KeyCode
                                Case Keys.Delete
                                    If P.Locked Then
                                        e.Handled = True
                                    End If

                                    If e.Handled = False Then
                                        If P.DT_FixText_Key > "" Then
                                            Dim CursorOrigPos As Integer = -1

                                            If .SelectionLength = 0 Then
                                                If .SelectionStart < Len(.Text) Then
                                                    CursorOrigPos = .SelectionStart
                                                    .SelectionStart = .SelectionStart + 1
                                                End If
                                            End If
                                            Dim ee As New System.Windows.Forms.KeyPressEventArgs(Chr(8))

                                            EVENT_KEYPRESS(sender, ee)
                                            If ee.Handled Then
                                                e.Handled = True
                                                If CursorOrigPos <> -1 Then
                                                    If Len(.Text) >= CursorOrigPos Then
                                                        .SelectionStart = CursorOrigPos
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If

                                Case Keys.Back
                                    If P.Locked Then
                                        e.Handled = True
                                    End If

                                Case Keys.F2
                                    If .SelectionStart = 0 And .SelectionLength = Len(.Text) Then
                                        .SelectionStart = Len(.Text)
                                        .SelectionLength = 0
                                    End If
                                    e.Handled = True

                                Case Keys.Enter
                                    Dim AltPressed = My.Computer.Keyboard.AltKeyDown

                                    If AltPressed Then
                                        If CType(c, TextBox).Multiline Then
                                            With CType(c, TextBox)
                                                Dim CurPos As Integer = .SelectionStart

                                                .Text = Mid(.Text, 1, CurPos) + vbCrLf + Mid(.Text, CurPos + 1)
                                                .SelectionStart = CurPos + 2
                                                .SelectionLength = 0

                                                e.Handled = True
                                            End With
                                        End If
                                    End If
                            End Select
                        End With

                    ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
                        With CType(c, MSDN.Html.Editor.HtmlEditorControl)
                            Select Case e.KeyCode
                                Case Keys.Delete
                                    If P.Locked Then
                                        e.Handled = True
                                    End If

                                Case Keys.Back
                                    If P.Locked Then
                                        e.Handled = True
                                    End If

                                Case Else
                                    'Nothing to do
                            End Select
                        End With
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub EVENT_KEYPRESS(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles c.KeyPress
        If Disposed Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.KeyPress", 0, "Control recieves events but it is disposed!")
        Else
            If Not FP.UnboundForm Then
                If FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                    If Not CreatedBy = ENUM_FP_CONTROL_Created_by.GRID Then
                        e.Handled = True
                    End If
                End If
            End If

            If Not e.Handled Then
                RaiseEvent Field_KeyPreview_KeyPress(Me, sender, e)
                If Not e.Handled Then
                    FP.RAISEEVENT_Form_KeyPreview_KeyPress(sender, e)
                End If
                If Not e.Handled Then
                    Dim ShiftPressed = My.Computer.Keyboard.ShiftKeyDown
                    Dim CtrlPressed = My.Computer.Keyboard.CtrlKeyDown

                    If Not (CtrlPressed And Asc(e.KeyChar) = 3) Then 'Az ellenorzesek csak akkor futnak le, ha nem Ctrl-C
                        If CtrlPressed Then
                            Select Case Asc(e.KeyChar)
                                Case 1      'Ctrl-a, Ctrl-A
                                    If ShiftPressed Then
                                        FP.FPf.FPApp.DoErrorMsgBox("FP_Control.KeyPress", 0, "Ctrl-Shift-A Pressed")
                                        e.Handled = True
                                    End If

                                Case 4      'Ctrl-d, Ctrl-D
                                    e.Handled = True
                                    FP.FORM_RECORDS_DELETE_CURRENT()

                                Case 18 'Ctrl-r, Ctrl-R
                                    Call FP.FORM_DORESYNC(, True)
                            End Select
                        End If

                        If Not e.Handled Then
                            If TypeOf (c) Is ComboBox Then
                                Select Case Asc(e.KeyChar)
                                    Case 8 'Backspace
                                        e.Handled = True

                                    Case 22 'Ctrl-V
                                        If Not P.Locked Then
                                            Dim iData As IDataObject = Clipboard.GetDataObject

                                            If iData.GetDataPresent(DataFormats.Text) Then
                                                Dim TextToFind As String = CType(iData.GetData(DataFormats.Text), String)
                                                FP.FPf.FPApp.COMBOBOX_FIND(c, DT, c_ComboBox.DisplayMember, TextToFind, False)
                                            End If
                                        End If
                                        e.Handled = True

                                    Case Else
                                        If Not P.Locked Then
                                            FP.FPf.FPApp.COMBOBOX_FIND(c, e)
                                        End If
                                        e.Handled = True
                                End Select
                            End If
                        End If

                        If Not e.Handled Then
                            If TypeOf (c) Is ListView Then
                                Select Case Asc(e.KeyChar)
                                    Case 22 'Ctrl-V
                                        e.Handled = True    'Nothing else to do

                                End Select
                            End If
                        End If

                        If Not e.Handled Then
                            If P.Locked Then
                                FIELD_LOCKED(e)
                            Else
                                If Not (CtrlPressed And Asc(e.KeyChar) = 22) Then 'Az ellenorzesek csak akkor futnak le, ha nem Ctrl-V. (Ctrl-V eseten rossz ertek is beirodhat, de a Validate nem engedi majd tovabb.)
                                    Select Case P.xType_VB
                                        Case "INT"
                                            FIELD_NUMERIC(e, , F_Format_MinusAllowed)

                                        Case "FLOAT"
                                            FIELD_NUMERIC(e, True, F_Format_MinusAllowed)

                                        Case "DATETIME"
                                            If Asc(e.KeyChar) <> 8 Then 'If not BackSpace
                                                Dim EnabledChars As String = " 0123456789" + Format_Date_Splitter + Format_Date_Order + Format_Time_Splitter
                                                e.Handled = (InStr(EnabledChars, e.KeyChar) = 0)
                                            End If

                                        Case "BIT"
                                            'Nothing to do

                                        Case ""
                                            If e.KeyChar = " " Then
                                                If F_Format_NOSPACE Then
                                                    FIELD_LOCKED(e)
                                                End If
                                            End If
                                            If Not e.Handled Then
                                                If F_Format_UCASE Then
                                                    If Char.IsLower(e.KeyChar) Then
                                                        e.KeyChar = Char.ToUpper(e.KeyChar)
                                                    End If
                                                End If
                                            End If

                                        Case Else
                                            FIELD_LOCKED(e)
                                            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.EVENT_KEYPRESS", 0, String.Format("Unknown xType_VB '{1}' for field '{0}'", FieldName, P.xType_VB))
                                    End Select
                                End If
                            End If
                        End If

                        If Not e.Handled Then
                            If P.Locked = False And FP.P_FORM_AllowEdits Then
                                Select Case P.xType_VB
                                    Case ""
                                        If P.DT_FixText_Key > "" Then
                                            If (e.KeyChar = Chr(8) Or e.KeyChar = Chr(3) Or e.KeyChar = Chr(22) Or e.KeyChar = Chr(24) Or Char.IsControl(e.KeyChar) = False) Then 'Backspace, Ctrl-C, Ctrl-V, Ctrl-X
                                                Dim ee_VALIDATED As System.EventArgs
                                                Dim Cancel As Boolean = False

                                                ee_VALIDATED = New System.EventArgs
                                                RaiseEvent Field_TextChanged(Me, c, ee_VALIDATED, Cancel)

                                                If Cancel Then
                                                    e.Handled = True
                                                Else
                                                    Dim DoIt As Boolean = True
                                                    Dim DT_P As New Struct_Simple_SELECT_Params
                                                    Dim DT_P_OUT As New Struct_Simple_SELECT_OutputParams

                                                    With DT_P
                                                        .FixText_Key = P.DT_FixText_Key
                                                        .SQL_WHERE = P.DT_WHERE2
                                                        .Field_Mandatory = P.Mandatory
                                                        .FPc = Me
                                                        .XLength = P.xlength
                                                        .NoSetText = True

                                                        .Field_TRIM = F_Format_TRIM
                                                        .Field_UCASE = F_Format_UCASE
                                                        .Field_NOSPACE = F_Format_NOSPACE

                                                        .Field_MULTILINE = False
                                                        If TypeOf (c) Is TextBox Then
                                                            .Field_MULTILINE = CType(c, TextBox).Multiline
                                                        End If
                                                    End With

                                                    DoIt = FP.FPf.FPApp.SIMPLE_SELECT_FIELD_KEYPRESS_DISPO(c, 0, e, DT_P, DT_P_OUT)

                                                    If DoIt Then
                                                        FP.FORM_DIRTY_SET(FieldName)
                                                        If c.Text = DT_P_OUT.Selected_Text Then
                                                            Dim ee As New System.EventArgs

                                                            EVENT_TEXTCHANGED(Me, ee)
                                                        Else
                                                            c.Text = DT_P_OUT.Selected_Text
                                                        End If

                                                        Selected_ID = DT_P_OUT.Selected_ID

                                                        Dim ee_VALIDATING As New System.ComponentModel.CancelEventArgs
                                                        EVENT_VALIDATING(Me, ee_VALIDATING)

                                                        If ee_VALIDATING.Cancel Then
                                                            If DT_P_OUT.NO_LIMIT_TO_LIST = False Then
                                                                UNDO()
                                                            End If
                                                        Else
                                                            ee_VALIDATED = New System.EventArgs
                                                            EVENT_VALIDATED(Me, ee_VALIDATED)
                                                            If DT_P_OUT.GotoNextField Then
                                                                FP.FPf.FOCUS_ON_AT_THE_END(FP.FPf.CONTROLS_GET_NEXTCONTROL(c), , , True)
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        Else
                                            If e.KeyChar = Chr(22) Then 'Ctrl-V
                                                If F_Format_UCASE Then
                                                    If My.Computer.Clipboard.ContainsText Then
                                                        Dim MyStr As String = nz(My.Computer.Clipboard.GetText, "")
                                                        If MyStr > "" Then
                                                            Clipboard_SET_TEXT(MyStr.ToUpper)
                                                        End If
                                                    End If
                                                End If
                                                If F_Format_TRIM Then
                                                    If My.Computer.Clipboard.ContainsText Then
                                                        Clipboard_SET_TEXT(nz(My.Computer.Clipboard.GetText, "").Trim)
                                                    End If
                                                End If
                                                If F_Format_NOSPACE Then
                                                    If My.Computer.Clipboard.ContainsText Then
                                                        Dim MyStr As String = Replace(nz(My.Computer.Clipboard.GetText, ""), " ", "")

                                                        Clipboard_SET_TEXT(MyStr)
                                                    End If
                                                End If
                                                If TypeOf (c) Is TextBox Then
                                                    With CType(c, TextBox)
                                                        If .Multiline = False Then
                                                            Dim MyStr As String = Replace(nz(My.Computer.Clipboard.GetText, ""), vbCrLf, "")
                                                            MyStr = Replace(MyStr, vbCr, "")
                                                            MyStr = Replace(MyStr, vbLf, "")

                                                            Clipboard_SET_TEXT(MyStr)
                                                        End If
                                                    End With
                                                End If
                                            End If
                                        End If

                                    Case Else
                                        'Nothing to do
                                End Select
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub EVENT_MOUSEENTER(ByVal sender As Object, ByVal e As System.EventArgs) Handles c.MouseEnter
        If Disposed Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.MouseEnter", 0, "Control recieves events but it is disposed!")
        Else
            Dim Handled As Boolean = False

            RaiseEvent Field_MouseEnter(Me, sender, e, Handled)
            If Not Handled Then
                FP.FPf.HELP_SHOW(c)
            End If
        End If
    End Sub
    Private Sub EVENT_MOUSELEAVE(ByVal sender As Object, ByVal e As System.EventArgs) Handles c.MouseLeave
        If Disposed Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.MouseLeave", 0, "Control recieves events but it is disposed!")
        Else
            Dim Handled As Boolean = False

            FP.FPf.HELP_HIDE()

            RaiseEvent Field_MouseLive(Me, sender, e, Handled)
        End If
    End Sub

    Public Sub EVENT_VALIDATING(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles c.Validating
        If Disposed Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.EVENT_VALIDATING", 0, "Control recieves events but it is disposed!")
        Else
            If FIELD_IsDirty() Then
                Value_Validated = (Value_Validated And P_Has_SIMPLE_SELECT)

                If Value_Validated = False Then
                    If P.xType_VB = "DATETIME" And FP.FPf.FORM_DATETIME_MonthCalendar.Focused Then
                        'Nothing to do
                    Else
                        e.Cancel = Not setFormattedValueFromEntered()

                        If Not e.Cancel Then
                            If F_Format_TRIM Then
                                c.Text = Trim(c.Text)
                            End If

                            Select Case P.xType_VB
                                Case ""
                                    Select Case F_Format_FORMAT
                                        Case "00 00"
                                            If c.Text <> "" Then
                                                Dim Pattern As System.Text.RegularExpressions.Regex
                                                Dim IsFormatted As Boolean = False
                                                Dim EnteredValue As String = c.Text

                                                Pattern = New System.Text.RegularExpressions.Regex("^([0-1][0-9]|[2][0-3]):([0-5][0-9])$") 'formátum: 00:00
                                                If Pattern.IsMatch(c.Text) = True Then
                                                    IsFormatted = True
                                                End If

                                                If IsFormatted = False Then
                                                    Pattern = New System.Text.RegularExpressions.Regex("^([0-1][0-9]|[2][0-3])([0-5][0-9])$") 'formátum: 0000
                                                    If Pattern.IsMatch(c.Text) = True Then
                                                        EnteredValue = String.Format("{0}:{1}", c.Text.Substring(0, 2), c.Text.Substring(2, 2))
                                                        IsFormatted = True
                                                    End If
                                                End If

                                                If IsFormatted = False Then
                                                    Pattern = New System.Text.RegularExpressions.Regex("^([0-1][0-9]|[2][0-3]) ([0-5][0-9])$") 'formátum: 00 00
                                                    If Pattern.IsMatch(c.Text) = True Then
                                                        EnteredValue = c.Text.Replace(" ", ":")
                                                        IsFormatted = True
                                                    End If
                                                End If

                                                If IsFormatted = False Then
                                                    Pattern = New System.Text.RegularExpressions.Regex("^([0-9]|[2][0-3]):([0-5][0-9])$") 'formátum: 0:00
                                                    If Pattern.IsMatch(c.Text) = True Then
                                                        c.Text = c.Text.Replace(":", "")
                                                        EnteredValue = String.Format("0{0}:{1}", c.Text.Substring(0, 1), c.Text.Substring(1, 2))
                                                        IsFormatted = True
                                                    End If
                                                End If

                                                If IsFormatted = False Then
                                                    Pattern = New System.Text.RegularExpressions.Regex("^([0-9]|[2][0-3])([0-5][0-9])$") 'formátum: 000
                                                    If Pattern.IsMatch(c.Text) = True Then
                                                        EnteredValue = String.Format("0{0}:{1}", c.Text.Substring(0, 1), c.Text.Substring(1, 2))
                                                        IsFormatted = True
                                                    End If
                                                End If

                                                If IsFormatted = False Then
                                                    Pattern = New System.Text.RegularExpressions.Regex("^([0-9]|[2][0-3]) ([0-5][0-9])$") 'formátum: 0 00
                                                    If Pattern.IsMatch(c.Text) = True Then
                                                        EnteredValue = String.Format("0{0}", c.Text).Replace(" ", ":")
                                                        IsFormatted = True
                                                    End If
                                                End If

                                                If IsFormatted = True Then
                                                    c.Text = EnteredValue
                                                Else
                                                    e.Cancel = True
                                                    FP.FPf.FOCUS_ON_AT_THE_END(c, 1204, F_Format_FORMAT) 'Wrong format
                                                End If
                                            End If
                                        Case Else
                                            If F_Format_FORMAT > "" Then
                                                If c.Text > "" Then
                                                    If F_Format_FORMAT = "00:00" Then
                                                        'Regex -- megnezni

                                                        Dim p As Integer = InStr(c.Text, ":")

                                                        If p > 0 Then
                                                            If p = 2 Then
                                                                c.Text = "0" + c.Text
                                                                p = 3
                                                            End If

                                                            If p = 3 Then
                                                                If Strings.Len(c.Text) = 3 Then
                                                                    c.Text = Strings.Left(c.Text, 3) + "00"
                                                                ElseIf Strings.Len(c.Text) = 4 Then
                                                                    c.Text = Strings.Left(c.Text, 3) + "0" + Strings.Mid(c.Text, 4)
                                                                End If
                                                            End If
                                                        End If
                                                    End If

                                                    Dim i As Integer
                                                    Dim AktFormat As String
                                                    Dim AktValue As String

                                                    If Len(c.Text) <> Len(F_Format_FORMAT) Then
                                                        e.Cancel = True
                                                        FP.FPf.FOCUS_ON_AT_THE_END(c, 1204, F_Format_FORMAT) 'Wrong format
                                                    Else
                                                        For i = 1 To Len(c.Text)
                                                            AktFormat = Mid(F_Format_FORMAT, i, 1)
                                                            AktValue = Mid(c.Text, i, 1)

                                                            Select Case AktFormat
                                                                Case "A"
                                                                    If InStr("ABCDEFGHIJKLMNOPQRSTUVWXYZ", AktValue) = 0 Then
                                                                        e.Cancel = True
                                                                        FP.FPf.FOCUS_ON_AT_THE_END(c, 1204, F_Format_FORMAT) 'Wrong format
                                                                        Exit For
                                                                    End If

                                                                Case "0"
                                                                    If InStr("0123456789", AktValue) = 0 Then
                                                                        e.Cancel = True
                                                                        FP.FPf.FOCUS_ON_AT_THE_END(c, 1204, F_Format_FORMAT) 'Wrong format
                                                                        Exit For
                                                                    End If

                                                                Case "_"
                                                                    'Nothing to do

                                                                Case Else
                                                                    If AktFormat <> AktValue Then
                                                                        e.Cancel = True
                                                                        FP.FPf.FOCUS_ON_AT_THE_END(c, 1204, F_Format_FORMAT) 'Wrong format
                                                                        Exit For
                                                                    End If
                                                            End Select
                                                        Next
                                                    End If
                                                End If
                                            End If
                                    End Select

                                Case Else
                                    'Nothing to do
                            End Select

                            If Not e.Cancel Then
                                If TypeOf (c) Is ComboBox Then
                                    If Not FP.FPf.FPApp.COMBOBOX_FIND(c, c.Text) Then
                                        e.Cancel = True
                                    End If
                                End If
                            End If

                            If Not e.Cancel Then
                                If FIELD_IsDirty() Then
                                    Dim Data_Binded_OLD As Boolean = FP.DATA_Binded
                                    FP.DATA_Binded = False
                                    RaiseEvent Field_BeforeUpdate(Me, e.Cancel)
                                    FP.DATA_Binded = Data_Binded_OLD
                                End If
                            End If

                            If e.Cancel Then
                                'UNDO()
                                If Not FP.FPf.FOCUS_ON_AT_THE_END_is_on Then
                                    FP.FPf.FOCUS_ON_AT_THE_END(c)
                                Else
                                    If Not (FP.FPf.FOCUS_ON_AT_THE_END_c Is Nothing) Then
                                        If Not c.Equals(FP.FPf.FOCUS_ON_AT_THE_END_c) Then
                                            FP.FPf.FOCUS_ON_AT_THE_END(c)
                                        End If
                                    End If
                                End If
                            Else
                                'Value_Validated = True
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Private Function Can_Dirty() As Boolean
        Dim OUT = False

        If c Is Nothing Then
            OUT = False
        ElseIf TypeOf c Is TextBox Then
            OUT = True
        ElseIf TypeOf c Is ComboBox Then
            OUT = True
        ElseIf TypeOf c Is CheckBox Then
            OUT = True
        ElseIf TypeOf c Is RadioButton Then
            OUT = True
        ElseIf TypeOf c Is Button Then
            OUT = False
        ElseIf TypeOf c Is RichTextBox Then
            OUT = True
        ElseIf TypeOf c Is Panel Then
            OUT = False
        ElseIf TypeOf c Is TabControl Then
            OUT = False
        ElseIf TypeOf c Is TabPage Then
            OUT = False
        ElseIf TypeOf c Is Label Then
            OUT = False
        ElseIf TypeOf c Is ListView Then
            If CType(c, ListView).CheckBoxes = False Then
                OUT = True
            End If
        ElseIf TypeOf c Is Button Then
            OUT = False
        ElseIf TypeOf c Is TreeView Then
            OUT = True
        ElseIf TypeOf c Is NumericUpDown Then
            OUT = True
        ElseIf TypeOf c Is MSDN.Html.Editor.HtmlEditorControl Then
            OUT = True
        Else
            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.Can_Dirty", 0, String.Format("FieldType of control '{0}' is unknown.", FieldName))
            OUT = True
        End If
        Can_Dirty = OUT
    End Function

    Private Function sender_is_MyCheckBox_In_GRID(ByVal sender As Object) As Boolean
        Dim OUT As Boolean = False

        If TypeOf (sender) Is CheckBox Then
            If CType(sender, CheckBox).Equals(c_ChkBox) Then
                OUT = (TypeOf (c) Is CheckBox And P.ShowInGRID)
            End If
        End If

        Return OUT
    End Function

    Protected Friend Sub EVENT_VALIDATED(ByVal sender As Object, ByVal e As System.EventArgs) Handles c.Validated
        If Disposed Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.EVENT_VALIDATED", 0, "Control recieves events but it is disposed!")
        Else
            If P.xType_VB = "DATETIME" And FP.FPf.FORM_DATETIME_MonthCalendar.Focused Then
                'Nothing to do
            Else
                If FP.P_DATA_Binded Then
                    'If FIELD_IsDirty() Then
                    If Value_Validated = False Then
                        FP.DATA_Binded = False
                        FP.RAISEEVENT_Form_Field_AfterUpdate(Me)
                        FP.DATA_Binded = True
                    End If
                End If

                Value_Validated = True

                If FP.P_DATA_Binded Then
                    If P.SavePoint <> ENUM_SavePoint_Type.NONE Then
                        If Not FP.FPf.ProcessSavePoint_Active Then
                            If P.SavePoint = ENUM_SavePoint_Type.TRANSACT_MODE Then
                                If FP.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                                    FP.FPf.ProcessSavePoint_Active = True
                                    If Not FP.FPf.SAVE_ALL() Then
                                        c.Focus()
                                        If FP.FPf.FOCUS_ON_AT_THE_END_is_on Then
                                            If c.Equals(FP.FPf.FOCUS_ON_AT_THE_END_c) Then
                                                FP.FPf.FOCUS_ON_AT_THE_END_CLEAR()
                                            End If
                                        End If
                                        Application.DoEvents()
                                    End If
                                    FP.FPf.ProcessSavePoint_Active = False
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub
#End Region

    Protected Friend Function IS_GRID_FILTERFIELD() As Boolean
        Dim OUT As Boolean = False

        If Not (c Is Nothing) Then
            If FP.GRID_EXISTS() Then
                OUT = FP.GRID.FILTER_FIELDS_IS_FILTERFIELD(Me)
            End If
        End If

        Return OUT
    End Function

    Private Sub c_ComboBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles c_ComboBox.MouseClick
        'Ez a szar azert kell, mert ComboBox-nal nem lep fel a doubleclick esemeny. Regenyek szolnak rola a Google-ban...
        Dim OldPrevClick As DateTime = c_ComboBox_prevClick

        c_ComboBox_prevClick = DateTime.Now
        If DateTime.Now.AddMilliseconds(-400) < OldPrevClick Then
            EVENT_DOUBLECLICK(c, e)
        End If
    End Sub

    Private Sub c_ComboBox_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles c_ComboBox.SelectedValueChanged
        Dim Was_Undo As Boolean = False

        If FP.P_DATA_Binded Then
            If nz(c_ComboBox.ValueMember, "") = "" Then
                c_ComboBox_Value = 0
            Else
                c_ComboBox_Value = nz(c_ComboBox.SelectedValue, 0)
            End If
        End If

        If P.Locked Then
            If FP.P_DATA_Binded Then
                UNDO_To_Saved_Value()
                Was_Undo = True
            End If
        End If

        If Not Was_Undo Then
            If FP.P_DATA_Binded Then
                'Form_Dirty ellenorzese
                If Not FP.P_FORM_Dirty Then
                    If FP.DATA_IsDataField(FieldName) Then
                        If Not FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                            Dim DBFormattedValue As String = ""

                            If FIELD_IsDirty() Then
                                If Not FP.FORM_DIRTY_SET(FieldName) Then
                                    UNDO_To_Saved_Value()
                                    Was_Undo = True
                                End If
                            End If
                        End If
                    End If
                End If

                If Not Was_Undo Then
                    Dim Cancel As Boolean = False
                    RaiseEvent Field_TextChanged(Me, sender, e, Cancel)
                    If Cancel Then
                        UNDO_To_Saved_Value()
                        Was_Undo = True
                    End If
                End If

                If Not Was_Undo Then
                    Value_Validated = False
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Private Sub c_ChkBox_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles c_ChkBox.PreviewKeyDown
        If P.ShowInGRID Then
            Select Case e.KeyCode
                Case Keys.Down, Keys.Up
                    Dim ee As New System.Windows.Forms.KeyEventArgs(e.KeyData)

                    EVENT_KEYDOWN(sender, ee)

                    FPf.FOCUS_ON_AT_THE_END(c_ChkBox, , , True)

                Case Else
                    'Nothing to do
            End Select
        End If
    End Sub

    Private Sub c_ListView_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles c_ListView.ColumnClick
        If c_ListView_LastSortOrder = ListViewItemComparer.Enum_SortOrder.Ascending Then
            c_ListView_LastSortOrder = ListViewItemComparer.Enum_SortOrder.Descending
        Else
            c_ListView_LastSortOrder = ListViewItemComparer.Enum_SortOrder.Ascending
        End If

        c_ListView.ListViewItemSorter = New ListViewItemComparer(e.Column, c_ListView_LastSortOrder)

        Dim ee As New System.EventArgs

        c_ListView_SelectedIndexChanged(c_ListView, ee)
    End Sub

    Private Sub c_ListView_DrawColumnHeader(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewColumnHeaderEventArgs) Handles c_ListView.DrawColumnHeader
        e.DrawDefault = True
    End Sub

    Private Sub c_ListView_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewItemEventArgs) Handles c_ListView.DrawItem
        If c_ListView.CheckBoxes Then
            e.DrawDefault = True
        Else
            'e.DrawBackground()
        End If
    End Sub

    Private c_ListView_Old_Checked_Values() As Boolean
    Private c_ListView_In_CheckChange As Boolean = False

    Private Sub c_ListView_UNDO()
        Dim c_ListView_In_CheckChange_OldValue As Boolean = c_ListView_In_CheckChange

        If UBound(c_ListView_Old_Checked_Values) <> c_ListView.Items.Count - 1 Then
            FPf.FPApp.DoErrorMsgBox("", 0, "c_ListView_UNDO: c_ListView.Items.Count - 1 <> UBOUND(c_ListView_InCheckChange)")
        Else
            c_ListView_In_CheckChange = True
            For i As Integer = 0 To c_ListView.Items.Count

            Next
        End If

        c_ListView_In_CheckChange = c_ListView_In_CheckChange_OldValue
    End Sub

    Private Sub c_ListView_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles c_ListView.ItemCheck
        If Disposed Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.c_ListView_ItemCheck", 0, "Control recieves events but it is disposed!")
        Else
            If c_ListView_In_CheckChange = False Then
                c_ListView_In_CheckChange = True
                ReDim c_ListView_Old_Checked_Values(c_ListView.Items.Count - 1)
                For i As Integer = 0 To c_ListView.Items.Count - 1
                    If (c_ListView.Items(i) Is Nothing) Then
                        'Sajnos elofordul, hogy az Item(i) Null, vagyis az Items.Count nagyobb, mint a tenyleges elemek szama
                        ReDim Preserve c_ListView_Old_Checked_Values(i - 1)
                        Exit For
                    Else
                        c_ListView_Old_Checked_Values(i) = c_ListView.Items(i).Checked
                    End If
                Next
                c_ListView_In_CheckChange = False
            End If
        End If
    End Sub

    Private Sub c_ListView_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles c_ListView.ItemChecked
        If Disposed Then
            Exit Sub
        End If

        Dim ActiveControl As FP_Control = FPf.P_ActiveControl

        If ActiveControl Is Nothing Then
            Exit Sub
        End If

        If Not (c_ListView.Equals(ActiveControl.c)) Then
            Exit Sub
        End If

        If c_ListView_In_CheckChange Then
            Exit Sub
        End If

        c_ListView_In_CheckChange = True

        Dim Was_Undo As Boolean = False

        'Locked ellenorzese
        If Not Was_Undo Then
            If c.Focused Then
                If P.Locked Then
                    c_ListView_UNDO()
                    Was_Undo = True
                End If
            End If
        End If

        If FP.DATA_IsDataField(FieldName) Then
            FPf.FPApp.DoErrorMsgBox("FP_Control.c_ListView_ItemCheck", 0, String.Format("Checkbox-os listview-nak nincs egyertelmu, mentheto erteke igy nem szerepelhet a SAVE eljarasban. (controlname: {0})", FieldName))
            c_ListView_UNDO()
            Was_Undo = True

            If Was_Undo = False Then
                If FP.P_DATA_Binded Then
                    If Not FP.FORM_DIRTY_SET Then
                        c_ListView_UNDO()
                        Was_Undo = True
                    End If
                End If
            End If
        End If

        If Was_Undo = False Then
            If FP.P_DATA_Binded Then
                RaiseEvent Field_TextChanged(Me, sender, e, Was_Undo)
            End If
            If Was_Undo Then
                c_ListView_UNDO()
            End If
        End If

        If Not Was_Undo Then
            Value_Validated = False
        End If

        c_ListView_In_CheckChange = False
    End Sub

    Public Sub LISTVIEW_GOTO_NEXT_ITEM()
        If Not (c_ListView Is Nothing) Then
            With c_ListView
                If Not (.FocusedItem Is Nothing) Then
                    If .FocusedItem.Index < .Items.Count - 1 Then
                        .FocusedItem = .Items(.FocusedItem.Index + 1)
                        c_ListView_SelectedIndexChanged()
                    End If
                End If
            End With
        End If
    End Sub

    Public Function LISTVIEW_IS_ALL_CHECKED() As Boolean
        Dim OUT As Boolean = False

        If Not (c_ListView Is Nothing) Then
            With c_ListView
                If .CheckBoxes Then
                    OUT = True

                    For Each I As ListViewItem In .Items
                        If I.Checked = False Then
                            OUT = False
                            Exit For
                        End If
                    Next
                End If
            End With
        End If

        Return OUT
    End Function

    Public Function LISTVIEW_IS_ALL_UNCHECKED() As Boolean
        Dim OUT As Boolean = False

        If Not (c_ListView Is Nothing) Then
            With c_ListView
                If .CheckBoxes Then
                    OUT = True

                    For Each I As ListViewItem In .Items
                        If I.Checked = True Then
                            OUT = False
                            Exit For
                        End If
                    Next
                End If
            End With
        End If

        Return OUT
    End Function

    Public Sub LISTVIEW_GOTO_PREVIOUS_ITEM()
        If Not (c_ListView Is Nothing) Then
            With c_ListView
                If Not (.FocusedItem Is Nothing) Then
                    If .FocusedItem.Index > 0 Then
                        .FocusedItem = .Items(.FocusedItem.Index - 1)
                        c_ListView_SelectedIndexChanged()
                    End If
                End If
            End With
        End If
    End Sub

    Public Function LISTVIEW_GET_CHECKED_TEXTS_WITH_SEPARATOR(Optional SeparatorChar As String = ", ", Optional Quotation_mark As String = "") As String
        Dim OUT As String = ""

        If Not (c_ListView Is Nothing) Then
            Dim CurrentSeparator As String = ""

            For Each I As ListViewItem In c_ListView.Items
                If I.Checked Then
                    OUT += CurrentSeparator + Quotation_mark + I.Text + Quotation_mark
                    CurrentSeparator = SeparatorChar
                End If
            Next
        End If

        Return OUT
    End Function

    Public Function LISTVIEW_GET_CHECKED_IDs_WITH_SEPARATOR(Optional SeparatorChar As String = ", ") As String
        Dim OUT As String = ""

        If Not (c_ListView Is Nothing) Then
            Dim CurrentSeparator As String = ""

            For Each I As ListViewItem In c_ListView.Items
                If I.Checked Then
                    OUT += CurrentSeparator + (I.Name).ToString
                    CurrentSeparator = SeparatorChar
                End If
            Next
        End If

        Return OUT
    End Function

    Public Sub LISTVIEW_SET_CHECKED_IDs_FROM_DELIMITED_STRING(Delimited_String As String, Optional SeparatorChar As String = ",")
        If Not (c_ListView Is Nothing) Then
            If c_ListView.CheckBoxes = True Then
                Dim Array_of_IDs() As String = Split(Delimited_String, SeparatorChar)

                For i As Integer = 0 To UBound(Array_of_IDs)
                    If Val(Array_of_IDs(i)) <> 0 Then
                        Dim CurrentKey As String = Trim(Array_of_IDs(i))

                        If c_ListView.Items.ContainsKey(CurrentKey) Then
                            c_ListView.Items(CurrentKey).Checked = True
                        End If
                    End If
                Next
            End If
        End If
    End Sub

    Public Sub LISTVIEW_CHECK_ALL()
        If Not (c_ListView Is Nothing) Then
            If c_ListView.CheckBoxes = True Then
                For Each I As ListViewItem In c_ListView.Items
                    I.Checked = True
                Next
            End If
        End If
    End Sub

    Public Sub LISTVIEW_UNCHECK_ALL()
        If Not (c_ListView Is Nothing) Then
            If c_ListView.CheckBoxes = True Then
                For Each I As ListViewItem In c_ListView.Items
                    I.Checked = False
                Next
            End If
        End If
    End Sub

    Public Sub LISTVIEW_CHECK_CHANGE_ALL()
        If Not (c_ListView Is Nothing) Then
            If c_ListView.CheckBoxes = True Then
                For Each I As ListViewItem In c_ListView.Items
                    I.Checked = False
                Next
            End If
        End If
    End Sub

    Private Sub c_ListView_SelectedIndexChanged()

        Dim ee As New System.EventArgs

        c_ListView_SelectedIndexChanged(c_ListView, ee)
    End Sub

    Private Sub c_ListView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles c_ListView.SelectedIndexChanged
        If c_ListView.CheckBoxes = False Then
            If c_ListView_LastSelected_Item_IDX > -1 Then
                If c_ListView.Items.Count > c_ListView_LastSelected_Item_IDX Then
                    For Each wSubItem As ListViewItem.ListViewSubItem In c_ListView.Items(c_ListView_LastSelected_Item_IDX).SubItems
                        wSubItem.BackColor = Nothing
                    Next

                    c_ListView.Items(c_ListView_LastSelected_Item_IDX).BackColor = Nothing
                End If
            End If

            c_ListView_LastSelected_Item_IDX = -1

            If Not (c_ListView.FocusedItem Is Nothing) Then
                Dim SubItem_BackColor As Color = IIf(P.COLOR_SELECTED_FORE.Equals(Color.FromArgb(0, 0, 0)), Color.SkyBlue, P.COLOR_SELECTED_FORE)

                For Each wSubItem As ListViewItem.ListViewSubItem In c_ListView.FocusedItem.SubItems
                    wSubItem.BackColor = SubItem_BackColor
                Next
                c_ListView_LastSelected_Item_IDX = c_ListView.FocusedItem.Index
            End If
        End If
    End Sub

    Private Sub c_ListView_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles c_ListView.ItemSelectionChanged
        Dim Was_Undo As Boolean = False

        If P.Locked Then
            If FP.P_DATA_Binded Then
                UNDO_To_Saved_Value()
                Was_Undo = True
            End If
        End If

        If Not Was_Undo Then
            If c_ListView.CheckBoxes = False Then
                If FP.P_DATA_Binded Then
                    'Form_Dirty ellenorzese
                    If Not FP.P_FORM_Dirty Then
                        If FP.DATA_IsDataField(FieldName) Then
                            If Not FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                                If FIELD_IsDirty() Then
                                    If Not FP.FORM_DIRTY_SET(FieldName) Then
                                        UNDO_To_Saved_Value()
                                        Was_Undo = True
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If Not Was_Undo Then
                        Dim Cancel As Boolean = False

                        RaiseEvent Field_TextChanged(Me, sender, e, Cancel)

                        If Cancel Then
                            UNDO_To_Before_TextChanged()
                            Was_Undo = True
                        End If
                    End If

                    If Not Was_Undo Then
                        Value_Validated = False
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub c_ListView_DrawSubItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewSubItemEventArgs) Handles c_ListView.DrawSubItem
        e.DrawBackground()
        e.DrawText()
    End Sub

#Region "c_Button"
    Private Const RoundShape_Key As String = "RndBtn_"

    Private Sub c_Button_SHOW(ByVal Pressed As Boolean)
        Dim ImageName As String = P.BG_Image_Name

        If InStr(ImageName, RoundShape_Key) > 0 Then
            Dim StatusLetter As String = ""

            If Pressed Then
                StatusLetter = "_"
            Else
                If c_Button.Focused Then
                    StatusLetter = "S"
                End If
            End If

            If StatusLetter > "" Then
                Dim p As Integer

                p = InStrRev(ImageName, ".")
                If p > 0 Then
                    ImageName = Mid(ImageName, 1, p - 1) + StatusLetter + Mid(ImageName, p)
                End If
            End If

            Dim asm As Reflection.Assembly = Nothing
            Dim ResourceName As String = ""

            If FPf.FPApp.SKIN_getASM_And_OBJECTNAME(ImageName, asm, ResourceName) Then
                Try
                    c_Button.BackgroundImage = New Bitmap(asm.GetManifestResourceStream(ResourceName))

                Catch ex As Exception
                    FPf.FPApp.DoErrorMsgBox("FP_Control.c_Button_SHOW", 0, String.Format("Image not found '{0}'", ResourceName))
                End Try
            End If
        End If
    End Sub

    Private Sub c_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles c.Click
        If P.Locked = False Then
            If Windows.Forms.MouseButtons.Left Then
                If TypeOf (c) Is RadioButton Then
                    With CType(c, RadioButton)
                        If .AutoCheck = True Then
                            If P_IsDataField Then
                                MsgBox("A Radiobutton 'AutoCheck' tulajdonsagat vedd ki, mert nem kezeli rendesen az UNDO funkciot!!!")
                            End If
                        Else
                            .Checked = (Not .Checked)
                        End If
                    End With

                    Dim ee As New System.EventArgs

                    EVENT_TEXTCHANGED(c, ee)

                ElseIf TypeOf (c) Is Button Then
                    c_Button_SHOW(True)
                    RaiseEvent Field_Click(Me, e)
                    c_Button_SHOW(False)
                Else
                    RaiseEvent Field_Click(Me, e)
                End If
            End If
        End If
    End Sub

    Private Sub c_Button_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles c_Button.GotFocus
        c_Button_SHOW(False)
    End Sub

    Private Sub c_Button_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles c_Button.LostFocus
        c_Button_SHOW(False)
    End Sub

    Private Sub c_Button_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles c_Button.MouseDown
        c_Button_SHOW(True)
    End Sub

    Private Sub c_Button_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles c_Button.MouseUp
        c_Button_SHOW(False)
    End Sub

    Public Sub c_Button_RoundShape_SET()
        If P.BG_Image_Name > "" Then
            If InStr(P.BG_Image_Name, RoundShape_Key) > 0 Then
                Dim gr As New System.Drawing.Drawing2D.GraphicsPath
                Dim BorderWidth As Integer = 4

                gr.FillMode = Drawing2D.FillMode.Winding

                gr.AddRectangle(New Rectangle(c_Button.Height / 2 - BorderWidth, BorderWidth, c_Button.Width - (c_Button.Height - 2 * BorderWidth), c_Button.Height - 2 * BorderWidth))
                gr.AddPie(New Rectangle(BorderWidth, BorderWidth, c_Button.Height - 2 * BorderWidth, c_Button.Height - 2 * BorderWidth), 90, 180)
                gr.AddPie(New Rectangle(c_Button.Width - c_Button.Height + BorderWidth, BorderWidth, c_Button.Height - 2 * BorderWidth, c_Button.Height - 2 * BorderWidth), -90, 180)

                c_Button.Region = New System.Drawing.Region(gr)
            Else
                c_Button.Region = Nothing
            End If
        End If
    End Sub
#End Region

    Private Sub c_Button_Resize(sender As Object, e As EventArgs) Handles c_Button.Resize
        c_Button_RoundShape_SET()
    End Sub

    Private Sub c_ListView_KeyPress(sender As Object, e As KeyPressEventArgs) Handles c_ListView.KeyPress
        Select Case e.KeyChar
            Case "+"
                LISTVIEW_CHECK_ALL()

            Case "-"
                LISTVIEW_UNCHECK_ALL()

            Case "*"
                LISTVIEW_CHECK_CHANGE_ALL()

            Case Else
                'Nothing to do
        End Select
    End Sub

    Private Sub c_HTML_editor_Enter(sender As Object, e As EventArgs) Handles c_HTML_editor.Enter

    End Sub

    Private Sub c_HTML_editor_KeyDown(sender As Object, ByRef KeyCode As Integer) Handles c_HTML_editor.HtmlEditor_KeyDown
        If Disposed Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_Control.c_HTML_editor_KeyPress", 0, "Control recieves events but it is disposed!")
        Else
            Dim Was_Undo As Boolean = False

            If FP.UnboundForm Then
                If FP.P_DATA_Binded Then

                    'Locked ellenorzese
                    If Not Was_Undo Then
                        If c.Focused Then
                            If P.Locked Then
                                UNDO_To_Before_TextChanged()
                                Was_Undo = True
                            End If
                        End If
                    End If

                    If Not Was_Undo Then
                        Dim Cancel As Boolean = False

                        Dim ee As New System.EventArgs

                        RaiseEvent Field_TextChanged(Me, sender, ee, Cancel)

                        If Cancel Then
                            UNDO_To_Before_TextChanged()
                            Was_Undo = True
                        Else
                            Value_Validated = False
                            Value_Before_TextChanged_SET()
                        End If
                    End If
                End If
            Else
                If FP.P_DATA_Binded Then
                    'Locked ellenorzese
                    If Not Was_Undo Then
                        If c.Focused Then
							If P.Locked Then
                                UNDO_To_Before_TextChanged()
                                Was_Undo = True
                            End If
                        End If
                    End If

                    'Form_Dirty ellenorzese
                    If Not FP.P_FORM_Dirty Then
                        If FP.DATA_IsDataField(FieldName) Then
                            If FP.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
                                If CHAR_Causes_Dirty(KeyCode) Then
                                    If Not FP.FORM_DIRTY_SET(FieldName) Then
                                        UNDO_To_Saved_Value()
                                        Was_Undo = True
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If Not Was_Undo Then
                        Dim Cancel As Boolean = False
                        Dim ee As New System.EventArgs

                        RaiseEvent Field_TextChanged(Me, sender, ee, Cancel)

                        If Cancel Then
                            UNDO_To_Before_TextChanged()
                            Was_Undo = True
                        Else
                            Value_Before_TextChanged_SET()
                        End If
                    End If
                End If
            End If

            If Was_Undo = True Then
                KeyCode = 0
            Else
                Value_Validated = False
            End If
        End If
    End Sub
End Class

Class ListViewItemComparer
    Implements IComparer

    Public Enum Enum_SortOrder As Integer
        Ascending = 0
        Descending = 1
    End Enum

    Private SortColumn As Integer
    Private SortOrder As Enum_SortOrder

    Public Sub New(ByVal SortColumn As Integer, ByVal SortOrder As Enum_SortOrder)
        Me.SortColumn = SortColumn
        Me.SortOrder = SortOrder
    End Sub
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim xString As String
        Dim YString As String

        Dim l1 As ListViewItem
        Dim l2 As ListViewItem

        l1 = CType(x, ListViewItem)
        l2 = CType(y, ListViewItem)

        xString = l1.SubItems(SortColumn).ToString
        YString = l2.SubItems(SortColumn).ToString

        If xString = YString Then
            Return 0
        ElseIf xString > YString Then
            If SortOrder = Enum_SortOrder.Ascending Then
                Return 1
            Else
                Return -1
            End If
        ElseIf xString < YString Then
            If SortOrder = Enum_SortOrder.Ascending Then
                Return -1
            Else
                Return 1
            End If

        Else
            Return 0
        End If
    End Function
End Class