Imports System.Data
Imports System.Data.SqlClient

Public Class FP_DoFilter
    Public WithEvents FPApp As FP_App
    Public WithEvents FPf As FP_Form
    Public WithEvents DOFILTER As FP
    Public WhereQuery As String = ""
    Public SubFilter As String = ""
    Public Do_FoundRecords As Boolean = False
    Public CountOfFoundRecords As Integer = 0

    Public NewRecords_SHOW As Boolean = True
    Public Identifier As String
    Public P As New Struct_DoFilter_gl_Params

    Public DefaultValues As New Dictionary(Of String, String)

    Private SEQ_0 As FP_SEQ
    Private SEQ_SubPrefix As FP_SEQ

    Private FoundRecords_Label_TEXT As String = "{0}"

    Private WithEvents FPc_CURRENT As FP_Control

    Sub New(ByVal MyFPApp As FP_App, ByVal MyIdentifier As String, Optional ByVal MyWhereQuery As String = "", Optional ByVal MyNewRecordsSHOW As Boolean = True)
        InitializeComponent()

        FPApp = MyFPApp
        WhereQuery = MyWhereQuery

        NewRecords_SHOW = MyNewRecordsSHOW

        FPApp.DoFilter_Params_CLEAR(P, NewRecords_SHOW)

        Identifier = MyIdentifier

        FPf = New FP_Form("DOFILTER_FPF", FPApp, Me, True)
        FPf.Location_Save_On_Close = False

        DOFILTER = New FP(FPf, "DOFILTER", Identifier, True)
    End Sub

    Private Function GET_FilterText_Segment_From_SEQ(ByVal FieldName As String) As String
        Dim OUT As String = "?"
        Dim LabelName As String = ""

        With DOFILTER.CONTROLS(FieldName)
            If Not (.c_Label Is Nothing) Then
                LabelName = .c_Label.Name
            End If
        End With

        Dim Criteria As String = String.Format("Text1 = '{0}'", LabelName)

        If SEQ_SubPrefix.DT_SEQ.Select(Criteria).Count() > 0 Then
            OUT = SEQ_SubPrefix.DT_SEQ.Select(Criteria).First!Text4
        End If

        GET_FilterText_Segment_From_SEQ = OUT
    End Function

    Private Function GET_FILTER_REPLACE_QUESTIONMARK(MyText As String, Q_TEXT As String, Q_ID As String, Q As String) As String
        'Ez a fuggveny azert jott letre, mert nem jo vegyiteni a ?(TEXT) es a ? parametereket.
        'Miert? Mert ha a szoveg: "???", akkor eloszor kicserelodik a ?(TEXT) "???"-re, majd ez kicserelodik haromszor "???"-ra.
        '       Tehat kizarolagosan vagy az egyik helyettesites jon letre, vagy a masik.
        Dim OUT As String = MyText

        If InStr(OUT, "?(TEXT)") > 0 Then
            OUT = Replace(OUT, "?(TEXT)", nz(Q_TEXT, ""))
        ElseIf InStr(OUT, "?(ID)") > 0 Then
            OUT = Replace(OUT, "?(ID)", Q_ID)
        Else
            OUT = Replace(OUT, "?", nz(Q, ""))
        End If

        Return OUT
    End Function

    Private Function GET_FILTER_REPLACE_QUESTIONMARK(MyText As String, Q_TEXT As String, Q_ID As Long, Q As String) As String
        'Ez a fuggveny azert jott letre, mert nem jo vegyiteni a ?(TEXT) es a ? parametereket.
        'Miert? Mert ha a szoveg: "???", akkor eloszor kicserelodik a ?(TEXT) "???"-re, majd ez kicserelodik haromszor "???"-ra.
        '       Tehat kizarolagosan vagy az egyik helyettesites jon letre, vagy a masik.
        Dim OUT As String = MyText

        If InStr(OUT, "?(TEXT)") > 0 Then
            OUT = Replace(OUT, "?(TEXT)", nz(Q_TEXT, ""))
        ElseIf InStr(OUT, "?(ID)") > 0 Then
            OUT = Replace(OUT, "?(ID)", Q_ID)
        Else
            OUT = Replace(OUT, "?", nz(Q, ""))
        End If

        Return OUT
    End Function

    Private Function GET_FILTER() As Boolean
        Dim OUT As Boolean = True
        Dim ListOfFields As New List(Of String)
        Dim AktKey As String = ""

        FPApp.DoFilter_Params_CLEAR(P, NewRecord_YN.Checked)

        If Not NewRecord_YN.Checked Then
            For Each AktKey In DOFILTER.CONTROLS.Keys
                If FIELD_IS_PARENT_OF(DoFilter_Panel, DOFILTER.CONTROLS(AktKey).c) Then
                    With DOFILTER.CONTROLS(AktKey)
                        If .P.Tag > "" Then
                            If TypeOf (.c) Is TextBox Then
                                If .c.Text > "" Then
                                    ListOfFields.Add(Format(.c.TabIndex, "000000") + AktKey)
                                End If
                            ElseIf TypeOf (.c) Is ComboBox Then
                                If .c_ComboBox.Text > "" Then
                                    ListOfFields.Add(Format(.c.TabIndex, "000000") + AktKey)
                                End If
                            ElseIf TypeOf (.c) Is CheckBox Then
                                If Not .c_ChkBox.ThreeState Then
                                    ListOfFields.Add(Format(.c.TabIndex, "000000") + AktKey)
                                Else
                                    If Not .c_ChkBox.CheckState = CheckState.Indeterminate Then
                                        ListOfFields.Add(Format(.c.TabIndex, "000000") + AktKey)
                                    End If
                                End If
                            ElseIf TypeOf (.c) Is ListView Then
                                With CType(.c, ListView)
                                    If .CheckBoxes = False Then
                                        If DOFILTER.CONTROLS(.Name).P_VALUE <> 0 Then
                                            ListOfFields.Add(Format(.TabIndex, "000000") + AktKey)
                                        End If
                                    Else
                                        Dim FPc As FP_Control = DOFILTER.CONTROLS(.Name)

                                        If FPc.LISTVIEW_IS_ALL_UNCHECKED = False Then
                                            ListOfFields.Add(Format(.TabIndex, "000000") + AktKey)
                                        End If
                                    End If
                                End With
                            Else
                                OUT = False
                                FPApp.DoErrorMsgBox("FP_DoFilter.GET_FILTER", 0, String.Format("Unknown Type of control '{0}'", .c.Name))
                            End If

                            Dim Params() As String = Split(.P.Tag, "|")

                            If UBound(Params) < 2 Then
                                ReDim Preserve Params(2)
                            End If

                            If Params(2) > "" Then
                                Dim U As Integer = UBound(P.FilterTexts)

                                P.FilterTexts(U) = Replace(.c.Text, "%", "")
                                P.FilterFields(U) = Params(2)
                                U += 1
                                ReDim Preserve P.FilterTexts(U)
                                ReDim Preserve P.FilterFields(U)
                            End If
                        End If
                    End With
                End If
            Next

            ListOfFields.Sort()

            Dim MyAND As String = ""

            If SubFilter > "" Then
                P.FilterWHERE = "(" + SubFilter + ")"
                MyAND = " AND "
            End If

            For Each AktKey In ListOfFields
                Dim FieldName As String = Mid(AktKey, 7, 255)

                With DOFILTER.CONTROLS(FieldName)
                    Dim Params() As String = Split(.P.Tag, "|")
                    Dim WHERE_Segment As String = ""
                    Dim Text_Segment As String = GET_FilterText_Segment_From_SEQ(FieldName)
                    Dim ParamNUM As Integer = -1

                    If UBound(Params) < 2 Then
                        ReDim Preserve Params(2)
                    End If

                    If Params(1) > "" Then
                        If Not IsNumeric(Params(1)) Then
                            FPApp.DoErrorMsgBox("FP_DoFilter.GET_FILTER", 0, String.Format("Invalid parameter for field '{0}', '{1}'. This parameter defines the ParamNum for DoFilter parameters and it must be a number between 0 and 30.", FieldName, Params(1)))
                        Else
                            If Int(Val(Params(1))) < 0 Or Int(Val(Params(1))) > UBound(P.ParamInt) Then
                                FPApp.DoErrorMsgBox("FP_DoFilter.GET_FILTER", 0, String.Format("Invalid parameter for field '{0}', '{1}'. This parameter defines the ParamNum for DoFilter parameters and it must be a number between 0 and 30.", FieldName, Params(1)))
                            Else
                                ParamNUM = Int(Val(Params(1)))
                            End If
                        End If
                    End If

                    Select Case .P.xType_VB
                        Case ""
                            WHERE_Segment = GET_FILTER_REPLACE_QUESTIONMARK(Params(0), SQLStr(.P_VALUE), .Selected_ID, SQLStr(.P_VALUE))
                            If ParamNUM > -1 Then
                                If .Selected_ID <> 0 Then
                                    P.ParamInt(ParamNUM) = Val(DBFORMAT_from_OBJECT(.Selected_ID, .c.Name, "INT"))
                                End If

                                P.ParamStr(ParamNUM) = .P_VALUE

                            End If
                            If TypeOf (.c) Is ComboBox Then
                                Text_Segment = Replace(Text_Segment, "?", .c.Text)
                            Else
                                Text_Segment = Replace(Text_Segment, "?", .P_VALUE)
                            End If

                        Case "INT", "BIT"
                            If TypeOf (.c) Is ListView Then
                                With DOFILTER.CONTROLS(.c.Name)
                                    If .c_ListView.CheckBoxes Then
                                        Dim SelectedIDs As String = .LISTVIEW_GET_CHECKED_IDs_WITH_SEPARATOR
                                        Dim SelectedTexts As String = .LISTVIEW_GET_CHECKED_TEXTS_WITH_SEPARATOR
                                        Dim Selected_QTexts As String = .LISTVIEW_GET_CHECKED_TEXTS_WITH_SEPARATOR(", ", "'")

                                        WHERE_Segment = GET_FILTER_REPLACE_QUESTIONMARK(Params(0), Selected_QTexts, SelectedIDs, SelectedIDs)
                                        Text_Segment = GET_FILTER_REPLACE_QUESTIONMARK(Text_Segment, SelectedTexts, SelectedIDs, SelectedTexts)
                                    Else
                                        WHERE_Segment = GET_FILTER_REPLACE_QUESTIONMARK(Params(0), nz(.c.Text, ""), DBFORMAT_from_OBJECT(.P_VALUE, .c.Name, .P.xType_VB), DBFORMAT_from_OBJECT(.P_VALUE, .c.Name, .P.xType_VB))
                                        If ParamNUM > -1 Then
                                            P.ParamInt(ParamNUM) = .P_VALUE
                                        End If
                                    End If
                                End With
                            Else
                                WHERE_Segment = GET_FILTER_REPLACE_QUESTIONMARK(Params(0), nz(.c.Text, ""), DBFORMAT_from_OBJECT(.P_VALUE, .c.Name, .P.xType_VB), DBFORMAT_from_OBJECT(.P_VALUE, .c.Name, .P.xType_VB))
                                If ParamNUM > -1 Then
                                    P.ParamInt(ParamNUM) = .P_VALUE
                                End If
                            End If

                            If TypeOf (.c) Is ComboBox Then
                                Text_Segment = Replace(Text_Segment, "?", nz(.c.Text, ""))
                            ElseIf TypeOf (.c) Is ListView Then

                            Else
                                Text_Segment = Replace(Text_Segment, "?", getStrInt(.P_VALUE))
                            End If

                        Case "FLOAT"
                            WHERE_Segment = Replace(Params(0), "?(TEXT)", .P_VALUE.ToString)
                            WHERE_Segment = Replace(WHERE_Segment, "?", .P_VALUE.ToString)
                            If ParamNUM > -1 Then
                                P.ParamDbl(ParamNUM) = .P_VALUE
                            End If
                            Text_Segment = Replace(Text_Segment, "?(TEXT)", getStrFloat(.P_VALUE))
                            Text_Segment = Replace(Text_Segment, "?", getStrFloat(.P_VALUE))

                        Case "DATETIME"
                            Dim MyDate As DateTime = .P_VALUE
                            If InStr(Params(0), "?(2359)") > 0 Then
                                Params(0) = Replace(Params(0), "?(2359)", "?")
                                MyDate = New DateTime(MyDate.Year, MyDate.Month, MyDate.Day, 23, 59, 59)
                            End If
                            WHERE_Segment = Replace(Params(0), "?", SQLDate(MyDate))
                            If ParamNUM > -1 Then
                                P.ParamDate(ParamNUM) = MyDate
                            End If
                            Text_Segment = Replace(Text_Segment, "?", getStrDate(MyDate))

                        Case Else
                            OUT = False
                            WHERE_Segment = ""
                            Text_Segment = ""
                            FPApp.DoErrorMsgBox("FP_DoFilter.GET_FILTER", 0, String.Format("Unknown xType_VB '{1}' for control '{0}'", .c.Name, .P.xType_VB))
                    End Select

                    If WHERE_Segment > "" Then
                        P.FilterWHERE += String.Format("{0}({1})", MyAND, WHERE_Segment)
                        MyAND = " AND "
                    End If

                    If Text_Segment > "" Then
                        P.FilterText += " " + Text_Segment
                    End If

                End With
            Next
        End If

        GET_FILTER = OUT
    End Function

    Private Function NAVIGATION_FORWARD() As Boolean
        Dim OUT As Boolean = True

        gl_Doit = False

        If NewRecord_YN.Checked = False Then
            If Do_FoundRecords Then
                If CountOfFoundRecords = 0 Then
                    OUT = False
                    FPApp.DoMyMsgBox(130) 'Nincs olyan adat, ami megfelelne a kriteriumoknak.
                End If
            End If
        End If

        If OUT Then
            OUT = GET_FILTER()
        End If

        If OUT Then
            If P.LetNewRecord = False Then
                OUT = FPf.CHK_FIELDS_ALL()
            End If
        End If

        If OUT Then
            OUT = SAVE_FilterField_Values()
        End If

        If OUT Then
            gl_Doit = True
            FPApp.gl_FilterParams = P
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If

        Return OUT
    End Function

    Private Sub FP_DoFilter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If Not (FPf.ActiveControl Is Nothing) Then
            FPf.ActiveControl.EVENT_KEYDOWN(sender, e)
        End If

        If e.Handled Then
            e.Handled = False
        Else
            Select Case e.KeyCode
                Case Keys.Enter
                    e.Handled = True
                    NAVIGATION_FORWARD()

                Case Else
                    'Nothing to do
            End Select
        End If
    End Sub

    Private Sub FP_DoFilter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Select Case Asc(e.KeyChar)
            Case 24 'Ctrl-X
                Dim ee As New System.EventArgs

                ButtonClear_Click(Me, ee)
        End Select
    End Sub

    Private Function WhereQuery_Contains_Field(WhereQuery As String, FieldName As String) As Boolean
        Dim OUT As Boolean = False
        Dim SQL As String = String.Format("SELECT * FROM sys.objects o inner join sys.columns c on o.object_id = c.object_id WHERE o.name = '{0}' and c.name = '{1}'", WhereQuery, FieldName)
        Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(SQL)
        If DRow IsNot Nothing Then
            OUT = True
        End If
        Return OUT
    End Function

    Public Sub FoundRecords_REFRESH()
        If Do_FoundRecords Then
            If NewRecord_YN.Checked Then
                FoundRecords_Label.Visible = False
            Else
                FoundRecords_Label.Visible = True

                If GET_FILTER() Then
                    Dim SQL As String = String.Format("SELECT TOP 301 ID FROM {0}", WhereQuery)
                    Dim DT As DataTable = Nothing

                    If P.FilterWHERE > "" Then
                        SQL += " WHERE " + P.FilterWHERE
                    End If

                    If Organisation_Handling Then
                        If WhereQuery_Contains_Field(WhereQuery, "Organisation_ID") Then
                            If Organisation_Rights_IDS <> String.Empty Then
                                Dim Org_Where As String = String.Format("(Organisation_ID IN({0}))", Organisation_Rights_IDS)
                                If SQL.IndexOf(" WHERE ") > 0 Then
                                    SQL += " AND " + Org_Where
                                Else
                                    SQL += " WHERE " + Org_Where
                                End If
                            End If
                        End If
                    End If

                    SQL += " GROUP BY ID"

                    FPApp.DC.Qdf_Fill_DT(SQL, DT)

                    Dim MyText As String

                    CountOfFoundRecords = DT.Rows.Count

                    If CountOfFoundRecords > 300 Then
                        MyText = ">300"
                    Else
                        MyText = DT.Rows.Count.ToString
                    End If

                    FoundRecords_Label.Text = String.Format(FoundRecords_Label_TEXT, MyText)
                End If
            End If
        End If
    End Sub

    Private Sub NewRecord_YN_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NewRecord_YN.CheckedChanged
        If NewRecord_YN.Visible Then
            DoFilter_Panel.Visible = (Not NewRecord_YN.Checked)
        End If
        FoundRecords_REFRESH()
    End Sub

    Private Function get_PFD_Key() As String
        get_PFD_Key = "F_" + Identifier
    End Function

    Private Function SAVE_FilterField_Values() As Boolean
        Dim OUT As Boolean = True

        If Not (FPc_CURRENT Is Nothing) Then
            Dim ee As New System.ComponentModel.CancelEventArgs

            FPc_CURRENT.EVENT_VALIDATING(FPc_CURRENT.c, ee)
            OUT = (Not ee.Cancel)
        End If

        If OUT Then
            Dim Values As String = ""

            If NewRecord_YN.Visible And NewRecord_YN.Checked Then
                Values = "NewRecord_YN|1|"
            Else
                Values = "NewRecord_YN|0|"
            End If

            For Each AktKey As String In DOFILTER.CONTROLS.Keys
                Dim SaveIt As Boolean = False

                With DOFILTER.CONTROLS(AktKey)
                    Dim Params() As String = Split(.P.Tag, "|")

                    If UBound(Params) < 2 Then
                        ReDim Preserve Params(2)
                    End If

                    If FIELD_IS_PARENT_OF(DoFilter_Panel, .c) Then
                        If .P.Tag > "" Then
                            If TypeOf (.c) Is TextBox Then
                                If .c.Text > "" Then
                                    SaveIt = True
                                End If

                            ElseIf TypeOf (.c) Is ComboBox Then
                                If .c_ComboBox.Text > "" Then
                                    SaveIt = True
                                End If

                            ElseIf TypeOf (.c) Is CheckBox Then
                                If Not .c_ChkBox.ThreeState Then
                                    SaveIt = True
                                Else
                                    If Not .c_ChkBox.CheckState = CheckState.Indeterminate Then
                                        SaveIt = True
                                    End If
                                End If

                            ElseIf TypeOf (.c) Is Label Then
                                'nothing to do

                            ElseIf TypeOf (.c) Is ListView Then
                                SaveIt = True

                            Else
                                OUT = False
                                FPApp.DoErrorMsgBox("FP_DoFilter.SAVE_FilterField_Values", 0, String.Format("Unknown Type of control '{0}'", .c.Name))
                            End If
                        End If

                        If SaveIt Then
                            If TypeOf (.c) Is ListView Then
                                Values += String.Format("{0}|{1}|", .c.Name, .LISTVIEW_GET_CHECKED_IDs_WITH_SEPARATOR)
                            Else
                                Values += String.Format("{0}|{1}|", .c.Name, DBFORMAT_from_OBJECT(.P_VALUE, .c.Name, .P.xType_VB))
                            End If
                            If .Selected_ID <> 0 Then
                                Values += String.Format("#ID|{0}|", .Selected_ID)
                            End If
                        End If
                    End If
                End With
            Next

            If OUT Then
                FPApp.PFDinsertOrUpdate(get_PFD_Key, Values)
                OUT = FPApp.DOFILTER_getProcessID(P)
            End If

            SAVE_FilterField_Values = OUT
        End If
    End Function

    Function LOAD_FilterField_Values() As Boolean
        Dim OUT As Boolean = False
        Dim Values As String = ""
        Dim LastFieldName As String = ""

        FPf.FPApp.DoFilter_Params_CLEAR(P, True)

        FPApp.PFDlesen(get_PFD_Key, Values)

        If Values > "" Then
            Dim A() As String = Split(Values, "|")

            For i As Integer = 0 To UBound(A) - 2 Step 2
                Dim FieldName As String = A(i)
                Dim DBValue As String = A(i + 1)

                If FieldName = NewRecord_YN.Name Then
                    NewRecord_YN.Checked = (DBValue = "1")
                    LastFieldName = ""
                ElseIf FieldName = "#ID" Then
                    DOFILTER.CONTROLS(LastFieldName).Selected_ID = Val(DBValue)
                Else
                    If DOFILTER.CONTROLS.ContainsKey(FieldName) Then
                        With DOFILTER.CONTROLS(FieldName)
                            If TypeOf (.c) Is ListView Then
                                .LISTVIEW_SET_CHECKED_IDs_FROM_DELIMITED_STRING(DBValue)
                            Else
                                .P_VALUE = OBJECT_from_DBFORMAT(DBValue, FieldName, .P.xType_VB)
                            End If
                            LastFieldName = FieldName
                        End With
                    Else
                        LastFieldName = ""
                    End If
                End If
            Next

            DOFILTER.COLORING_ALL()

            OUT = True
        End If

        LOAD_FilterField_Values = OUT
    End Function

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        NAVIGATION_FORWARD()
    End Sub

    Private Sub ButtonClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Dim CtrlPressed = My.Computer.Keyboard.CtrlKeyDown
        Dim TC As TabControl = Nothing
        If CtrlPressed Then
            TC = Get_Tab_Control(Me)
            If TC Is Nothing Then
                DOFILTER.CONTROLS_CLEAR_ALL()
            Else
                Dim TP As TabPage = TC.SelectedTab
                DOFILTER.CONTROLS_CLEAR_ALL(TP)
            End If
        Else
            DOFILTER.CONTROLS_CLEAR_ALL()
        End If
        FoundRecords_REFRESH()
    End Sub

    Private Function Get_Tab_Control(O As Object) As TabControl
        Dim OUT As TabControl = Nothing
        Dim Ctrl As Object
        Dim Type_Name As String = ""
        Type_Name = TypeName(O).ToString.ToUpper
        If Type_Name = "TABCONTROL" Then
            OUT = O
        Else
            For Each Ctrl In O.Controls
                OUT = Get_Tab_Control(Ctrl)
                If Not OUT Is Nothing Then Exit For
            Next
        End If

        Return OUT
    End Function
    Private Sub DOFILTER_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles DOFILTER.CONTROLS_INITIALIZED
        SEQ_0 = New FP_SEQ(FPApp, "VBSEQ_DOFILTER", "Text2 = ''")
        SEQ_SubPrefix = New FP_SEQ(FPApp, "VBSEQ_DOFILTER", String.Format("Text2 = '{0}'", Identifier))

        Dim gl_DataBinded_OLD As Boolean = gl_Data_Binded

        gl_Data_Binded = False

        If WhereQuery > "" Then
            Do_FoundRecords = True
        End If

        FoundRecords_Label_TEXT = FoundRecords_Label.Text

        If Not LOAD_FilterField_Values() Then
            If NewRecord_YN.Visible Then
                NewRecord_YN.Checked = True
            End If
        End If

        If Not NewRecord_YN.Visible Then
            NewRecord_YN.Checked = False
        End If

        gl_Data_Binded = gl_DataBinded_OLD
    End Sub

    Private Sub DOFILTER_Form_Field_AfterUpdate(ByVal FPc As FP_Control) Handles DOFILTER.Form_Field_AfterUpdate
        FoundRecords_REFRESH()
    End Sub

    Private Sub DOFILTER_Form_Field_Enter(ByVal FPc As FP_Control, ByRef Handled As Boolean) Handles DOFILTER.Form_Field_Enter
        FPc_CURRENT = FPc
    End Sub

    Private Sub FPc_CURRENT_Field_TextChanged(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByVal e As System.EventArgs, ByRef Cancel As Boolean) Handles FPc_CURRENT.Field_TextChanged
        If FPc_CURRENT.P.xType_VB = "DATETIME" Then
            If Not FPc_CURRENT.c.Focused Then
                FoundRecords_REFRESH()
            Else
                If FPc_CURRENT.c.Text = "" Then
                    FoundRecords_REFRESH()
                End If
            End If
        Else
            FoundRecords_REFRESH()
        End If
    End Sub

    Private Sub FP_DoFilter_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Elorehozott LOAD esemeny

        gl_Doit = False

        Dim FPF_CONTROLS As New Struct_FP_FORM_CONTROLS_COLLECTION

        With FPF_CONTROLS
            .Dlg_Btn_CANCEL = ButtonCancel
            .Btn_HELP = ButtonHELP
        End With

        FPf.INIT_CONTROLS(FPF_CONTROLS)
        DOFILTER.INIT_CONTROLS(Nothing)

        FPApp.DoFilter_Params_CLEAR(FPApp.gl_FilterParams, NewRecord_YN.Checked)

        If DefaultValues.Count > 0 Then
            For Each AktKey As String In DefaultValues.Keys
                If Not DOFILTER.CONTROLS.ContainsKey(AktKey) Then
                    FPApp.DoErrorMsgBox("FP_DoFilter.FP_DoFilter_Shown", 0, String.Format("Field '{0}' not found.", AktKey))
                Else
                    DOFILTER.CONTROLS(AktKey).SET_VALUE_from_DBFORMAT(DefaultValues(AktKey))
                End If
            Next
        End If

        If NewRecords_SHOW Then
            FPf.FOCUS_ON_AT_THE_END(NewRecord_YN)
        Else
            NewRecord_YN.Checked = False
            NewRecord_YN.Visible = False
            NewRecord_YN_Label.Visible = False
        End If

        If Trim(WhereQuery) = "" Then
            FoundRecords_Label.Visible = False
        End If

        FitToScreen()
    End Sub

    Public Sub FitToScreen()
        Dim Screen_WorkingArea As New FP_L_Rect(FPApp.SCREEN_GET_WorkingArea(Me).Rect)
        Dim Form_Rect As New FP_L_Rect(Me.Location, Me.Size)
        Dim Form_Location_OK As Boolean = False
        Dim Check_Rect As FP_L_Rect = Form_Rect
        Dim MyParentFPf As FP_Form = FPf.P_ParentFPf

        Form_Location_OK = (Screen_WorkingArea.Contains(Check_Rect))

        If Form_Location_OK = False Then
            If Not (MyParentFPf Is Nothing) Then
                Check_Rect = New FP_L_Rect(MyParentFPf.Frm.Left + MyParentFPf.Frm.Width / 2 - Me.Width / 2, MyParentFPf.Frm.Top, Me.Width, Me.Height)

                Form_Location_OK = (Screen_WorkingArea.Contains(Check_Rect))
                If Form_Location_OK Then
                    Me.Location = Check_Rect.Location
                End If
            End If
        End If

        If Form_Location_OK = False Then
            If Not (MyParentFPf Is Nothing) Then
                Check_Rect = New FP_L_Rect(MyParentFPf.Frm.Left + MyParentFPf.Frm.Width / 2 - Me.Width / 2, Screen_WorkingArea.Top + Screen_WorkingArea.Height / 2 - Me.Height / 2, Me.Width, Me.Height)

                Form_Location_OK = (Screen_WorkingArea.Contains(Check_Rect))
                If Form_Location_OK Then
                    Me.Location = Check_Rect.Location
                End If
            End If
        End If

        If Form_Location_OK = False Then
            Me.Location = New Point(Screen_WorkingArea.Left + Screen_WorkingArea.Width / 2 - Me.Width / 2, Screen_WorkingArea.Top)
        End If
    End Sub

    Private Sub FP_DoFilter_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If SubFilter > "" Or WhereQuery > "" Then
            FoundRecords_REFRESH()
        End If
    End Sub
End Class