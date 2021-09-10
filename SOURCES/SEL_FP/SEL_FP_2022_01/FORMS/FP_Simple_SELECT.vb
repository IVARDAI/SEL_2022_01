Imports System.Data
Imports System.Data.SqlClient

Public Class FP_Simple_SELECT
    Private Enum ENUM_ARRANGE_TYPE As Integer
        TEXTBOX_SEARCH_ON_TOP = 0
        TEXTBOX_SEARCH_ON_BOTTOM = 1
    End Enum

    Public Enum ENUM_LIST_ACTIVATED_BY As Integer
        IMMEDIATELY = 0
        STAR = 1
        KEYPRESS_THEN_STAR = 2
    End Enum

    Public List_Activated_By As ENUM_LIST_ACTIVATED_BY = ENUM_LIST_ACTIVATED_BY.IMMEDIATELY

    Public FPApp As FP_App

    Public WithEvents FPf As FP_Form
    Public WithEvents FP As FP

    Private FPc_Selected As FP_Control
    Private WithEvents FP_L_Selected As FP_L_GRID_RS_Select_Checkbox

    Public P As New Struct_Simple_SELECT_Params
    Private FormDef_SQL_WHERE As String = ""
    Private FormDef_Mandatory As Boolean = False
    Private FormDef_LimitToList As Boolean = False
    Private RS_Loaded As Boolean = False
    Private RS_Loaded_Text As String = ""
    Private MAXRECORDS As Long = -1
    Private MaxRecords_Label_TEXT As String = "> {0}"

    Public LimitToList As Boolean = True

    Public Out_Params As New Struct_Simple_SELECT_OutputParams


    Public SQL_ORDERBY As String

    Public QUERY_PREFIX As String
    Public FIELD_SELECTED_ID As String
    Public FIELD_TEXT As String
    Public FIELD_SELECTED_STR As String
    Public FIELD_SELECTED_LONG As String
    Public WINDOWFORMAT As String
    Public MultiSelect As Boolean = False
    Private ArrangeType As ENUM_ARRANGE_TYPE = ENUM_ARRANGE_TYPE.TEXTBOX_SEARCH_ON_TOP

    Public Const SIMPLE_SELECT_Prefix As String = "@@VB_SIMPLE_SELECT_"
    Private OrigHeight As Integer = 0

    Private TextBox_Search_OldText As String = ""

    Dim CurrentScreen As Screen
    Dim ScreenShot As Bitmap

    Sub New(ByVal MyFPApp As FP_App)
        InitializeComponent()

        FPApp = MyFPApp
        FPf = New FP_Form("SIMPLESELECT_HEAD", FPApp, Me, True)

        gl_SIMPLE_SELECT_OutputParams = New Struct_Simple_SELECT_OutputParams
    End Sub

    Private Sub FPf_FORM_CASH_REOPEN(sender_FPf As FP_Form) Handles FPf.FORM_CASH_REOPEN

    End Sub

    Private ReadOnly Property P_UCASE As Boolean
        Get
            Dim OUT As Boolean = False

            If Not (P.FPc Is Nothing) Then
                OUT = P.FPc.F_Format_UCASE
            End If

            Return OUT
        End Get
    End Property

    Private ReadOnly Property P_NOSPACE As Boolean
        Get
            Dim OUT As Boolean = False

            If Not (P.FPc Is Nothing) Then
                OUT = P.FPc.F_Format_NOSPACE
            End If

            Return OUT
        End Get
    End Property

    Public Sub INIT_CASHED_FORM(ByVal Params As Struct_Simple_SELECT_Params)
        Dim OUT As Boolean = True
        Dim Orig_MAXRECORDS As Integer = P.MAXRECORDS

        P = Params
        P.MAXRECORDS = Orig_MAXRECORDS

        Out_Params = New Struct_Simple_SELECT_OutputParams

        P.SQL_WHERE = FPApp.Text_Replace_Standard_Params(nz(P.SQL_WHERE, ""))

        TextBox_Search.Text = ""

        If Not (P.FPc Is Nothing) Then
            CurrentScreen = Screen.FromPoint(P.FPc.c.PointToScreen(New Point(0, 0)))
        Else
            CurrentScreen = Screen.FromPoint(Me.PointToScreen(New Point(0, 0)))
        End If

        If WINDOWFORMAT = "MULTITEXTBOX" Then
            If P.FPc Is Nothing Then
                WINDOWFORMAT = ""
            End If
        End If

        If WINDOWFORMAT = "MULTITEXTBOX" Then
            Try
                ScreenShot = New Bitmap(CurrentScreen.WorkingArea.Width, FPf.P_Layout_TextBox_NormalHeight)
                Dim gBitmapFromScreen As Graphics = Graphics.FromImage(ScreenShot)

                gBitmapFromScreen.CopyFromScreen(CurrentScreen.Bounds.X, P.FPc.c.PointToScreen(New Point(0, 0)).Y - 2, 0, 0, ScreenShot.Size)
                gBitmapFromScreen.Dispose()

            Catch ex As Exception
                'Nothing to do - elofordulhat olyan gep, amelyik nem tudja eloallitani a ScreenShot-ot (talan a felbontas tul nagy vagy nincs eleg memoria)
            End Try
        End If

        P.SQL_WHERE = TEXT_AND(P.SQL_WHERE, FormDef_SQL_WHERE)

        CURSOR_SHOW_DEFAULT()
    End Sub

    Public Function INIT(ByVal Params As Struct_Simple_SELECT_Params) As Boolean
        Dim OUT As Boolean = True
        Dim MyFixText As String = ""

        P = Params
        Out_Params = New Struct_Simple_SELECT_OutputParams

        P.SQL_WHERE = nz(FPApp.Text_Replace_Standard_Params(nz(P.SQL_WHERE, "")), "")

        TextBox_Search.Text = ""

        Dim DIC_Properties As New Dictionary(Of String, String)

        If Mid(P.FixText_Key, 1, Len(SIMPLE_SELECT_Prefix)).ToUpper <> SIMPLE_SELECT_Prefix Then
            P.FixText_Key = SIMPLE_SELECT_Prefix + P.FixText_Key
        End If
        MyFixText = FPApp.Text_Replace_Standard_Params(FPApp.getFixText(P.FixText_Key))
        If MyFixText = "" Then
            OUT = False
            MsgBox(String.Format("FP_Simple_SELECT.INIT: {0} in FixText not found", P.FixText_Key))
        Else
            FPApp.FIXTEXT_SPLIT_PARAMS(MyFixText, DIC_Properties)
            OUT = (OUT And FPApp.FIXTEXT_CHK_PARAM(DIC_Properties, P.FixText_Key, "FIELD_SELECTED_ID"))
            OUT = (OUT And FPApp.FIXTEXT_CHK_PARAM(DIC_Properties, P.FixText_Key, "FIELD_TEXT"))

            If OUT Then
                SQL_ORDERBY = DIC_GET("SQL_ORDERBY", DIC_Properties)
                FIELD_SELECTED_ID = DIC_GET("FIELD_SELECTED_ID", DIC_Properties)
                FIELD_TEXT = DIC_GET("FIELD_TEXT", DIC_Properties)
                FIELD_SELECTED_STR = DIC_GET("FIELD_SELECTED_STR", DIC_Properties)
                FIELD_SELECTED_LONG = DIC_GET("FIELD_SELECTED_LONG", DIC_Properties)
                QUERY_PREFIX = DIC_GET("QUERY_PREFIX", DIC_Properties)
                If QUERY_PREFIX = "" Then
                    QUERY_PREFIX = Replace(P.FixText_Key, "@", "")
                End If

                If DIC_GET("NO_LIMIT_TO_LIST", DIC_Properties) = "1" Then
                    LimitToList = False
                End If

                If Not (P.FPc Is Nothing) Then
                    CurrentScreen = Screen.FromPoint(P.FPc.c.PointToScreen(New Point(0, 0)))
                Else
                    CurrentScreen = Screen.FromPoint(Me.PointToScreen(New Point(0, 0)))
                End If

                If Val(DIC_GET("MAXRECORDS", DIC_Properties)) > 0 Then
                    P.MAXRECORDS = Val(DIC_GET("MAXRECORDS", DIC_Properties))
                End If

                WINDOWFORMAT = DIC_GET("WINDOWFORMAT", DIC_Properties)
                If WINDOWFORMAT = "MULTITEXTBOX" Then
                    If P.FPc Is Nothing Then
                        WINDOWFORMAT = ""
                    End If
                End If

                If WINDOWFORMAT = "MULTITEXTBOX" Then
                    Try
                        ScreenShot = New Bitmap(CurrentScreen.WorkingArea.Width, FPf.P_Layout_TextBox_NormalHeight)
                        Dim gBitmapFromScreen As Graphics = Graphics.FromImage(ScreenShot)

                        gBitmapFromScreen.CopyFromScreen(CurrentScreen.Bounds.X, P.FPc.c.PointToScreen(New Point(0, 0)).Y - 2, 0, 0, ScreenShot.Size)
                        gBitmapFromScreen.Dispose()

                    Catch ex As Exception
                        'Nothing to do - elofordulhat olyan gep, amelyik nem tudja eloallitani a ScreenShot-ot (talan a felbontas tul nagy vagy nincs eleg memoria)
                    End Try
                End If

                FormDef_SQL_WHERE = DIC_GET("SQL_WHERE", DIC_Properties)
                If FormDef_SQL_WHERE > "" Then
                    P.SQL_WHERE = TEXT_AND(P.SQL_WHERE, FormDef_SQL_WHERE)
                End If

                Select Case DIC_GET("Mandatory", DIC_Properties)
                    Case "", "0"
                        FormDef_Mandatory = False

                    Case "1"
                        FormDef_Mandatory = True
                        P.Field_Mandatory = True

                    Case Else
                        FPApp.DoErrorMsgBox("FP_Simple_SELECT.INIT", 0, String.Format("Value of parameter 'MANDATORY' in '{0}' is invalid ('{1}')", P.FixText_Key, DIC_GET("MANDATORY", DIC_Properties)))
                End Select

                Select Case DIC_GET("LIMIT_TO_LIST", DIC_Properties)
                    Case "", "0"
                        FormDef_LimitToList = False

                    Case "1"
                        FormDef_LimitToList = True

                    Case Else
                        FPApp.DoErrorMsgBox("FP_Simple_SELECT.INIT", 0, String.Format("Value of parameter 'LIMIT_TO_LIST' in '{0}' is invalid ('{1}')", P.FixText_Key, DIC_GET("LIMIT_TO_LIST", DIC_Properties)))
                End Select

                Select Case DIC_GET("LIST_ACTIVATED_BY", DIC_Properties)
                    Case "", "IMMEDIATELY"
                        List_Activated_By = ENUM_LIST_ACTIVATED_BY.IMMEDIATELY

                    Case "*"
                        List_Activated_By = ENUM_LIST_ACTIVATED_BY.STAR
                        Selected_Text_Remove_Star()

                    Case "KEYPRESS_THEN_*"
                        List_Activated_By = ENUM_LIST_ACTIVATED_BY.KEYPRESS_THEN_STAR
                        Selected_Text_Remove_Star()

                    Case Else
                        FPApp.DoErrorMsgBox("FP_Simple_SELECT.INIT", 0, String.Format("Unknown parameter setting for parameter 'LIST_ACTIVATED_BY' (FixText: '{0}', unknown parameter setting: '{1}'", P.FixText_Key, DIC_GET("LIST_ACTIVATED_BY", DIC_Properties)))
                End Select
            End If
        End If

        With FPf
            .Location_Save_On_Close = False
            .P_ShowCentralMenuOnClose = False
            '+++ML .Cashable = True
        End With

        FP = New FP(FPf, "SIMPLESELECT", P.FixText_Key) 'Ez csak itt lehet, mert a P.FixText_Key itt kerul kiolvasasra
        With FP
            .P_FORM_AllowAdditions = False
            .P_FORM_AllowDeletions = False
            .P_FORM_AllowEdits = False

            If MAXRECORDS > 0 Then
                .MAXRECORDS = P.MAXRECORDS
            End If

            With .SQL_BIND_Params
                .NameOf_WhereQuery = QUERY_PREFIX + "_WhereQuery"
                .NameOf_GRID = QUERY_PREFIX + "_GRID"
                .NameOf_READ = QUERY_PREFIX + "_GRID"
                .NameOf_SAVE = ""
                .NameOf_DEL = ""
            End With
        End With

        CURSOR_SHOW_DEFAULT()

        INIT = OUT
    End Function

    Private Sub Selected_Text_Remove_Star()
        If Len(P.Selected_Text) > 0 Then
            If Mid(P.Selected_Text, Len(P.Selected_Text), 1) = "*" Then
                P.Selected_Text = Mid(P.Selected_Text, 1, Len(P.Selected_Text) - 1)
            End If
        End If
    End Sub

    Private Sub OUT_PARAMS_CLEAR()
        With Out_Params
            .RS_ID = 0
            .Selected_ID = 0
            .Selected_Long = 0
            .Selected_String = ""
            .Selected_Text = ""
            .NO_LIMIT_TO_LIST = (Not LimitToList)
            .GotoNextField = False
        End With
    End Sub

    Private Sub OUT_PARAMS_SET()
        OUT_PARAMS_CLEAR()

        If MultiSelect Then
            FPf.FPApp.DC.Qdf_RunSQL(String.Format("DELETE RS_L WHERE RS_ID = {0} And Selected = 0", FP.RS_ID), 0)
            Out_Params.RS_ID = FP.RS_ID
        Else
            With Out_Params
                .RS_ID = FP.RS_ID

                If Not FP.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                    FPf.FPApp.DC.Qdf_RunSQL(String.Format("DELETE RS_L WHERE RS_ID = {0}", FP.RS_ID), 0)
                    If Not LimitToList Then
                        .Selected_Text = TextBox_Search.Text
                    End If
                Else
                    .Selected_ID = Val(FP.DATA_Field_getSavedValue(FIELD_SELECTED_ID))
                    .Selected_Text = TextBox_Search.Text

                    FPf.FPApp.DC.Qdf_RunSQL(String.Format("DELETE RS_L WHERE RS_ID = {0} And RecordID <> {1}", FP.RS_ID, .Selected_ID), 0)

                    If FIELD_SELECTED_LONG = "" Then
                        .Selected_Long = 0
                    Else
                        .Selected_Long = Val(FP.DATA_Field_getSavedValue(FIELD_SELECTED_LONG))
                    End If

                    If FIELD_SELECTED_STR = "" Then
                        .Selected_String = ""
                    Else
                        .Selected_String = FP.DATA_Field_getSavedValue(FIELD_SELECTED_STR)
                    End If
                End If
            End With

            gl_SIMPLE_SELECT_OutputParams = Out_Params
        End If
    End Sub

    Private Sub NAVIGATION_EXIT()
        OUT_PARAMS_CLEAR()
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Function NAVIGATION_FORWARD() As Boolean
        Dim OUT As Boolean = True

        If P.NoMessageForNoRecord = False Then
            If MultiSelect Then
                Dim MySQL As String = String.Format("SELECT TOP 1 RecordID FROM RS_L WITH (READUNCOMMITTED) WHERE RS_ID = {0} AND Selected = 1", FP.RS_ID)
                Dim DRow As DataRow = FPApp.DC.Qdf_get_DataRow(MySQL)

                If DRow Is Nothing Then 'Korabban: If FP.GRID.DT.Select("Selected = 1").Count < 1 Then
                    OUT = False
                    FPf.FPApp.DoMyMsgBox(57) 'Egyetlen adatsor sem lett kivalasztva.
                End If
            Else
                If Not RS_Loaded Then
                    If TextBox_Search.Text = "" Then
                        If P.Field_Mandatory Then
                            OUT = False
                            FP.FPf.FPApp.DoMyMsgBox(7)      'Adatmegadas kotelezo
                        End If
                    End If

                    If OUT = True Then
                        If LimitToList Then
                            If TextBox_Search.Text > "" Then
                                Dim Criteria As String = String.Format("{0} = '{1}'", FIELD_TEXT, TextBox_Search.Text)

                                If Not FP.GRID.DT.Select(Criteria).Count > 0 Then
                                    OUT = False
                                    FP.FPf.FPApp.DoMyMsgBox(60)      'Valasszon egy adatsort a listabol
                                Else
                                    Dim Row As DataRow = FP.GRID.DT.Select(Criteria).First

                                    FP.FORM_GOTO_RECORD_BY_ID(Row!RecordID, True)
                                End If
                            End If
                        End If
                    End If
                Else
                    If P.Field_Mandatory And TextBox_Search.Text = "" Then
                        OUT = False
                        FP.FPf.FPApp.DoMyMsgBox(7)      'Adatmegadas kotelezo
                    Else
                        If LimitToList Then
                            If FP.P_DATA_RecordStatus <> ENUM_RecordStatus.EXISTS Then
                                OUT = False
                                FP.FPf.FPApp.DoMyMsgBox(19) 'Ervenytelen adat
                            End If
                        End If
                    End If
                End If
            End If
        End If

        If OUT Then
            OUT_PARAMS_SET()
            Out_Params.GotoNextField = True
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If

        NAVIGATION_FORWARD = OUT
    End Function

    Private Sub TextBox_Search_SET_TEXT_AND_SELECTION(ByVal MySearchText As String, ByVal MyText As String)
        MySearchText = Nz(MySearchText, "")
        MyText = Nz(MyText, "")
        With TextBox_Search
            If .Text <> MyText Then
                .Text = MyText
            End If
            If InStr(MyText.ToUpper, MySearchText.ToUpper) = 1 Then
                If MyText.Length >= MySearchText.Length Then
                    .SelectionStart = MySearchText.Length
                    .SelectionLength = MyText.Length - MySearchText.Length + 1
                Else
                    .SelectionStart = MyText.Length
                    .SelectionLength = 0
                End If
            Else
                .SelectionStart = MyText.Length
                .SelectionLength = 0
            End If
        End With
    End Sub

    Private Function GRID_FIND(ByVal FindText As String) As Boolean
        Dim OUT As Boolean = False
        Dim FindTextU As String = ""
        Dim FindTextU2 As String = ""

        If FindText Is Nothing Then
            FindText = ""
        End If

        FindTextU = FindText.ToUpper()
        FindTextU2 = Replace(FindTextU, "'", "''")

        If (Not RS_Loaded) Or RS_Loaded_Text > "" Then
            If (Not RS_Loaded) Or Mid(FindTextU, 1, Len(RS_Loaded_Text)) <> RS_Loaded_Text Then
                LOAD_RECORDS(FindTextU, True)
            End If
        End If

        If Not RS_Loaded Then
            If FP.RS_RecCount >= FP.MAXRECORDS Then
                MaxRecords_Label.Text = String.Format(MaxRecords_Label_TEXT, FP.MAXRECORDS.ToString)
                MaxRecords_Label.Visible = True
                OUT = True
            End If
            FP.GRID.P_VISIBLE = False
        Else
            MaxRecords_Label.Visible = False
            FP.GRID.P_VISIBLE = True

            If FP.GRID.DT Is Nothing Then
                FPApp.DoErrorMsgBox("FP_Simple_SELECT.GRID_FIND", 0, "FP.GRID.DT is nothing")
            Else
                Dim CritText As String = FIELD_TEXT

                If CritText > "" Then
                    Dim LastChar As String = Mid(CritText, Len(CritText), 1)
                End If

                'Dim Criteria As String = String.Format("{0} '{1}%'", FIELD_TEXT, FindTextU)
                Dim Criteria = String.Format("SUBSTRING({0}, 1, {2}) + 'X' = '{1}' + 'X'", FIELD_TEXT, FindTextU2, Len(FindTextU))

                Try
                    If FP.GRID.DT.Select(Criteria).Count > 0 Then
                        Dim Row As DataRow = FP.GRID.DT.Select(Criteria).First

                        FP.FORM_GOTO_RECORD_BY_ID(Row!RecordID, True)

                        OUT = True
                    Else
                        If Not LimitToList Then
                            FP.FORM_GOTO_NORECORD()
                            OUT = True
                        End If
                    End If

                Catch ex As Exception
                    FPApp.DoErrorMsgBox("FP_Simple_SELECT.GRID_FIND", Err.Number, Err.Description)
                End Try
            End If
        End If

        GRID_FIND = OUT
    End Function

    Private Sub TextBox_Search_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox_Search.KeyDown
        If MultiSelect Then
            Select Case e.KeyCode
                Case Keys.Enter
                    e.Handled = True
                    NAVIGATION_FORWARD()

                Case Keys.Escape
                    e.Handled = True
                    NAVIGATION_EXIT()

                Case Keys.Up
                    If FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                        If Not FP.FORM_GOTO_RECORD_GRID_LASTROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    Else
                        If Not FP.FORM_GOTO_RECORD_GRID_PREVIOUSROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    End If
                    e.Handled = True

                Case Keys.Down
                    If FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                        If Not FP.FORM_GOTO_RECORD_GRID_FIRSTROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    Else
                        If Not FP.FORM_GOTO_RECORD_GRID_NEXTROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    End If
                    e.Handled = True

                Case Keys.PageUp
                    If FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                        If Not FP.FORM_GOTO_RECORD_GRID_LASTROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    Else
                        If Not FP.FORM_GOTO_RECORD_GRID_PREVIOUSPAGE() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    End If
                    e.Handled = True

                Case Keys.PageDown
                    If FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                        If Not FP.FORM_GOTO_RECORD_GRID_FIRSTROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    Else
                        If Not FP.FORM_GOTO_RECORD_GRID_NEXTPAGE() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    End If
                    e.Handled = True

                Case Keys.Home
                    If Not FP.FORM_GOTO_RECORD_GRID_FIRSTROW() Then
                        FP.RAISEEVENT_Form_Current()
                    End If
                    e.Handled = True

                Case Keys.End
                    If Not FP.FORM_GOTO_RECORD_GRID_LASTROW() Then
                        FP.RAISEEVENT_Form_Current()
                    End If
                    e.Handled = True

                Case Else
                    e.Handled = True
            End Select
        Else
            Select Case e.KeyCode
                Case Keys.Enter
                    e.Handled = True
                    NAVIGATION_FORWARD()

                Case Keys.Escape
                    e.Handled = True
                    NAVIGATION_EXIT()

                Case Keys.F2
                    TextBox_Search.SelectionStart = Len(TextBox_Search.Text)
                    TextBox_Search.SelectionLength = 0
                    e.Handled = True

                Case Keys.Up
                    If FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                        If Not FP.FORM_GOTO_RECORD_GRID_LASTROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    Else
                        If Not FP.FORM_GOTO_RECORD_GRID_PREVIOUSROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    End If
                    e.Handled = True

                Case Keys.Down
                    If FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                        If Not FP.FORM_GOTO_RECORD_GRID_FIRSTROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    Else
                        If Not FP.FORM_GOTO_RECORD_GRID_NEXTROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    End If
                    e.Handled = True

                Case Keys.PageUp
                    If FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                        If Not FP.FORM_GOTO_RECORD_GRID_LASTROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    Else
                        If Not FP.FORM_GOTO_RECORD_GRID_PREVIOUSPAGE() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    End If
                    e.Handled = True

                Case Keys.PageDown
                    If FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                        If Not FP.FORM_GOTO_RECORD_GRID_FIRSTROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    Else
                        If Not FP.FORM_GOTO_RECORD_GRID_NEXTPAGE() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                    End If
                    e.Handled = True

                Case Keys.Home
                    If TextBox_Search.SelectionStart = 0 And TextBox_Search.SelectionLength = Len(TextBox_Search.Text) Then
                        If Not FP.FORM_GOTO_RECORD_GRID_FIRSTROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                        e.Handled = True
                    Else
                        TextBox_Search.SelectionStart = 0
                        TextBox_Search.SelectionLength = 0
                        e.Handled = True
                    End If

                Case Keys.End
                    If TextBox_Search.SelectionStart = 0 And TextBox_Search.SelectionLength = Len(TextBox_Search.Text) Then
                        If Not FP.FORM_GOTO_RECORD_GRID_LASTROW() Then
                            FP.RAISEEVENT_Form_Current()
                        End If
                        e.Handled = True
                    Else
                        TextBox_Search.SelectionStart = Len(TextBox_Search.Text)
                        TextBox_Search.SelectionLength = 0
                        e.Handled = True
                    End If

                Case Keys.Delete, Keys.Back
                    If LimitToList Then
                        TextBox_Search_SET_TEXT_AND_SELECTION("", "")
                    End If

                Case Else
                    'Nothing to do
            End Select
        End If
    End Sub

    Private Sub TextBox_Search_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_Search.KeyPress
        If MultiSelect Then
            If FP.P_DATA_RecordStatus <> ENUM_RecordStatus.EXISTS Then
                e.Handled = True
            Else
                Select Case e.KeyChar
                    Case " "
                        FPc_Selected.P_VALUE = Not (FPc_Selected.P_VALUE)

                    Case "+"
                        Dim Current_RowIndex As Integer = 0

                        FP.GRID.ROW_CURRENT_ROWINDEX(Current_RowIndex)
                        FPf.FPApp.DC.Qdf_RunSQL(String.Format("UPDATE RS_L SET Selected = 1 FROM RS_L WITH (READUNCOMMITTED) WHERE RS_ID = {0}", FP.RS_ID), 0)
                        FP.FORM_RECORDS_LOAD_FROM_EXISTING_RS(True)
                        FP.FORM_GOTO_RECORD_BY_GRID_ROWINDEX(Current_RowIndex)
                        FP.RAISEEVENT_Form_AfterUpdate()

                    Case "-"
                        Dim Current_RowIndex As Integer = 0

                        FP.GRID.ROW_CURRENT_ROWINDEX(Current_RowIndex)
                        FPf.FPApp.DC.Qdf_RunSQL(String.Format("UPDATE RS_L SET Selected = 0 FROM RS_L WITH (READUNCOMMITTED) WHERE RS_ID = {0}", FP.RS_ID), 0)
                        FP.FORM_RECORDS_LOAD_FROM_EXISTING_RS(True)
                        FP.FORM_GOTO_RECORD_BY_GRID_ROWINDEX(Current_RowIndex)
                        FP.RAISEEVENT_Form_AfterUpdate()

                    Case "*"
                        Dim Current_RowIndex As Integer = 0

                        FP.GRID.ROW_CURRENT_ROWINDEX(Current_RowIndex)
                        FPf.FPApp.DC.Qdf_RunSQL(String.Format("UPDATE RS_L SET Selected = (1 - Selected) FROM RS_L WITH (READUNCOMMITTED) WHERE RS_ID = {0}", FP.RS_ID), 0)
                        FP.FORM_RECORDS_LOAD_FROM_EXISTING_RS(True)
                        FP.FORM_GOTO_RECORD_BY_GRID_ROWINDEX(Current_RowIndex)
                        FP.RAISEEVENT_Form_AfterUpdate()

                    Case Else
                        e.Handled = True
                End Select
            End If
        Else
            If Asc(e.KeyChar) <> 3 Then 'When not Ctrl-C
                If Asc(e.KeyChar) = 22 Then 'Ctrl-V
                    e.Handled = True
                    Dim FindText As String = nz(My.Computer.Clipboard.GetText, "")

                    If GRID_FIND(FindText) Then
                        Dim SELECTED_TEXT As String = FP.CONTROLS(FIELD_TEXT).c.Text

                        TextBox_Search_SET_TEXT_AND_SELECTION(FindText, SELECTED_TEXT)
                    End If
                Else
                    If e.KeyChar = "*" And List_Activated_By = ENUM_LIST_ACTIVATED_BY.KEYPRESS_THEN_STAR Then
                        Me.Height = OrigHeight
                        List_Activated_By = ENUM_LIST_ACTIVATED_BY.IMMEDIATELY
                        e.Handled = True
                    Else
                        If Asc(e.KeyChar) > 31 Then
                            If e.KeyChar = " " Then
                                If P_NOSPACE Then
                                    e.Handled = True
                                End If
                            End If

                            If Not e.Handled Then
                                Dim FindText As String = ""
                                Dim MyChar As String = e.KeyChar

                                If P_UCASE Then
                                    MyChar = MyChar.ToUpper
                                End If

                                FindText = Mid(TextBox_Search.Text, 1, TextBox_Search.SelectionStart) + MyChar

                                If GRID_FIND(FindText) Then
                                    e.Handled = True

                                    If FP.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS And RS_Loaded Then
                                        Dim SELECTED_TEXT As String = FP.CONTROLS(FIELD_TEXT).c.Text

                                        TextBox_Search_SET_TEXT_AND_SELECTION(FindText, SELECTED_TEXT)
                                    Else
                                        Dim SELECTED_TEXT = FindText

                                        TextBox_Search_SET_TEXT_AND_SELECTION(FindText, FindText)
                                    End If
                                Else
                                    If LimitToList Then
                                        e.Handled = True
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Public Function LOAD_RECORDS(ByVal SelectedText As String, ByVal NoRecordOK As Boolean) As Boolean
        Dim OUT As Boolean = True
        Dim MyWHERE As String = P.SQL_WHERE

        If P.RS_ID <> 0 Then
            FPApp.DoErrorMsgBox("FP_Simple_SELECT.LOAD_RECORDS", 0, "If Simple_SELECT works from existing RS, then calling function LOAD_RECORDS not needed.")
            FP.FORM_RECORDS_LOAD_FROM_EXISTING_RS()
        Else
            RS_Loaded = False
            RS_Loaded_Text = ""
            SelectedText = SelectedText.ToUpper

            If SelectedText > "" Then
                'MyWHERE = TEXT_AND(P.SQL_WHERE, String.Format("{0} like '{1}%'", FIELD_TEXT, SelectedText))
                MyWHERE = TEXT_AND(P.SQL_WHERE, String.Format("LEFT({0}, {2}) = '{1}'", FIELD_TEXT, SelectedText, Len(SelectedText)))
            End If
            If Not FP.FORM_RECORDS_LOAD(MyWHERE) Then
                FP.FORM_RECORDS_LOAD_FROM_EXISTING_RS()
            Else
                RS_Loaded_Text = SelectedText
                RS_Loaded = True
                If P.Selected = True Then
                    FPApp.RS_SELECT_ALL(FP.RS_ID, True)
                    FP.FORM_RECORDS_LOAD_FROM_EXISTING_RS()
                End If
            End If
            If FP.RS_RecCount < 1 Then
                If Not NoRecordOK Then
                    If RS_Loaded_Text = "" Then
                        OUT = False
                    End If
                End If
            Else
                If P.Selected = True Then
                    FPApp.RS_SELECT_ALL(FP.RS_ID, True)
                    FP.FORM_RECORDS_LOAD_FROM_EXISTING_RS()
                End If
            End If
        End If

        LOAD_RECORDS = OUT
    End Function

    Private Sub FP_Simple_SELECT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                e.Handled = True
                NAVIGATION_FORWARD()

            Case Else
                'Nothing to do
        End Select
    End Sub

    Private Sub FP_Simple_SELECT_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        FPf.P_GRIDS_SAVE_ALL_Field_Length_ENABLED = False

        If FPApp.Cashed_Forms.IsCashed(FPf) = False Then
            Dim FPF_CONTROLS As New Struct_FP_FORM_CONTROLS_COLLECTION
            Dim FP_CONTROLS As New Struct_FP_CONTROLS_COLLECTION

            With FPF_CONTROLS
                .Btn_HELP = Btn_Hlp
                .Dlg_Btn_CANCEL = Btn_Cancel
            End With
            FPf.INIT_CONTROLS(FPF_CONTROLS)

            With FP_CONTROLS
                With .GRID
                    .Label = GRID_Label
                    .GRID = GRID
                    .Btn_FooterVisible = GRID_Btn_FooterVisible
                    .Footer_Panel = GRID_Panel
                End With
                .Btn_ExportToExcel = Btn_ExcelExport
            End With

            FP.INIT_CONTROLS(FP_CONTROLS)
        End If

        Select Case WINDOWFORMAT
            Case "", "FULLSCREEN"
                Me.Top = Me.CurrentScreen.Bounds.Top
                OrigHeight = CurrentScreen.WorkingArea.Height
                Me.Height = OrigHeight

            Case "MANUAL"
                OrigHeight = Me.Height

            Case "MULTITEXTBOX"
                If P.FPc Is Nothing Then
                    WINDOWFORMAT = ""
                    FPApp.DoErrorMsgBox("FP_Simple_SELECT.Shown", 0, "If WINDOWFORMAT = 'MULTITEXTBOX' THEN P.c is mandatory.")
                Else
                    TextBox_Search.BackColor = COLORS_FIELD_CURRENT_BG

                    Me.StartPosition = FormStartPosition.Manual
                    Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None

                    Dim c_Rect As New Rectangle(P.FPc.c.PointToScreen(New Point(0, 0)), P.FPc.c.Size)
                    Dim MyScreen As Screen = Screen.FromControl(P.FPc.c)

                    If c_Rect.Top > MyScreen.WorkingArea.Height / 2 Then
                        ArrangeType = ENUM_ARRANGE_TYPE.TEXTBOX_SEARCH_ON_BOTTOM
                    End If

                    If ArrangeType = ENUM_ARRANGE_TYPE.TEXTBOX_SEARCH_ON_TOP Then
                        Me.Top = c_Rect.Top - TextBox_Search_Panel.Top + 1
                        OrigHeight = MyScreen.WorkingArea.Height - Me.Top
                        Me.Height = OrigHeight
                    Else
                        Me.Top = 0
                        OrigHeight = c_Rect.Top + c_Rect.Height - 2
                        Me.Height = OrigHeight
                    End If
                End If

            Case Else
                FPApp.DoErrorMsgBox("FP_Simple_SELECT.Shown", 0, String.Format("Unknown format '{0}'", WINDOWFORMAT))
        End Select


        If WINDOWFORMAT = "MULTITEXTBOX" Then
            Btn_ExcelExport.Visible = False
        End If

        If MaxRecords_Label.Text > "" Then
            MaxRecords_Label_TEXT = MaxRecords_Label.Text
        End If

        Dim DoIt As Boolean = True

        If P.RS_ID <> 0 Then
            FP.RS_ID = P.RS_ID
            If P.Selected = True Then
                FPApp.RS_SELECT_ALL(FP.RS_ID, True)
            End If
            RS_Loaded = FP.FORM_RECORDS_LOAD_FROM_EXISTING_RS
            If Not RS_Loaded Then
                If LimitToList Then
                    DoIt = False
                    Me.Close()
                    If P.NoMessageForNoRecord = False Then
                        FPApp.DoMyMsgBox(8)  'Nincs megjelenitendo adat
                    End If
                End If
            End If
        Else
            If P.MAXRECORDS > 0 Then
                FP.MAXRECORDS = P.MAXRECORDS
            End If

            If Not LOAD_RECORDS("", False) Then
                If LimitToList Then
                    gl_Doit = False
                    DoIt = False
                    FPApp.DoMyMsgBox(8) 'Nincs megjelenitendo adat
                    Me.Close()
                End If
            End If
        End If

        If DoIt Then
            '---------------------
            'Fuggoleges elrendezes
            '---------------------
            Select Case ArrangeType
                Case ENUM_ARRANGE_TYPE.TEXTBOX_SEARCH_ON_TOP
                    If WINDOWFORMAT = "MULTITEXTBOX" Then
                        FIELD_VISIBLE(Btn_Hlp, False)
                        FIELD_VISIBLE(Btn_Cancel, False)
                        FIELD_VISIBLE(Btn_OK, False)

                        FPf.ARRANGE_BOTTOMS(GRID_Panel, Me, 1)

                        FPf.ARRANGE_TOPS(TextBox_Search_Panel, Header_Panel)
                        FPf.ARRANGE_ON_BOTTOM_LEFT(GRID_Btn_FooterVisible, TextBox_Search, 1)
                        FPf.ARRANGE_TOPS(GRID_Label, GRID_Btn_FooterVisible)

                        FPf.SIZE_HEIGHT_BETWEEN(GRID, GRID_Label, GRID_Panel, 1, 1)
                        FP.GRID.GRID_OrigHeight = GRID.Height
                        FPf.ARRANGE_ON_BOTTOM_LEFT(GRID, GRID_Label)
                    Else
                        FPf.ARRANGE_BOTTOMS(Btn_Hlp, FPf.Frm, 1)
                        FPf.ARRANGE_BOTTOMS(Btn_Cancel, Btn_Hlp)
                        FPf.ARRANGE_BOTTOMS(Btn_OK, Btn_Hlp)
                        FPf.ARRANGE_ON_TOP_LEFT(GRID_Panel, Btn_Hlp, 1)

                        If MultiSelect Then
                            TextBox_Search_Panel.Height = 0

                            FPf.ARRANGE_TOPS(GRID_Btn_FooterVisible, Header_Panel, 1)
                            FPf.ARRANGE_TOPS(GRID_Label, GRID_Btn_FooterVisible)
                        Else
                            FPf.ARRANGE_TOPS(TextBox_Search_Panel, Header_Panel, 1)
                            FPf.ARRANGE_ON_BOTTOM_LEFT(GRID_Btn_FooterVisible, TextBox_Search, 1)
                            FPf.ARRANGE_TOPS(GRID_Label, GRID_Btn_FooterVisible)
                        End If

                        FPf.SIZE_HEIGHT_BETWEEN(GRID, GRID_Label, GRID_Panel, 1, 1)
                        FP.GRID.GRID_OrigHeight = GRID.Height
                        FPf.ARRANGE_ON_BOTTOM_LEFT(GRID, GRID_Label)
                    End If

                Case ENUM_ARRANGE_TYPE.TEXTBOX_SEARCH_ON_BOTTOM
                    If WINDOWFORMAT = "MULTITEXTBOX" Then
                        FIELD_VISIBLE(Btn_Hlp, False)
                        FIELD_VISIBLE(Btn_Cancel, False)
                        FIELD_VISIBLE(Btn_OK, False)

                        FPf.ARRANGE_BOTTOMS(TextBox_Search_Panel, Me)
                        FPf.ARRANGE_ON_TOP_LEFT(GRID_Panel, TextBox_Search_Panel, 0)

                        FPf.ARRANGE_TOPS(Header_Panel, Me, 1)
                        FPf.ARRANGE_ON_BOTTOM_LEFT(GRID_Btn_FooterVisible, Header_Panel, 1)
                        FPf.ARRANGE_TOPS(GRID_Label, GRID_Btn_FooterVisible)

                        FPf.SIZE_HEIGHT_BETWEEN(GRID, GRID_Label, GRID_Panel, 1, 1)
                        FP.GRID.GRID_OrigHeight = GRID.Height
                        FPf.ARRANGE_ON_BOTTOM_LEFT(GRID, GRID_Label)
                    Else
                        FPf.ARRANGE_TOPS(Btn_Hlp, FPf.Frm, 1)
                        FPf.ARRANGE_TOPS(Btn_Cancel, Btn_Hlp)
                        FPf.ARRANGE_TOPS(Btn_OK, Btn_Hlp)
                        FPf.ARRANGE_BOTTOMS(GRID_Panel, Me, 1)

                        If MultiSelect Then
                            TextBox_Search_Panel.Height = 0

                            FPf.ARRANGE_TOPS(GRID_Btn_FooterVisible, Header_Panel, 1)
                            FPf.ARRANGE_TOPS(GRID_Label, GRID_Btn_FooterVisible)
                        Else
                            FPf.ARRANGE_BOTTOMS(TextBox_Search_Panel, Me, 1)
                            FPf.ARRANGE_ON_BOTTOM_LEFT(GRID_Btn_FooterVisible, Header_Panel, 1)
                            FPf.ARRANGE_TOPS(GRID_Label, GRID_Btn_FooterVisible)
                        End If

                        FPf.SIZE_HEIGHT_BETWEEN(GRID, GRID_Label, GRID_Panel, 1, 1)
                        FP.GRID.GRID_OrigHeight = GRID.Height
                        FPf.ARRANGE_ON_BOTTOM_LEFT(GRID, GRID_Label)
                    End If

                Case Else
                    FPApp.DoErrorMsgBox("FP_Simple_SELECT.Shown", 0, "Unknown ArrangeType")
            End Select

            '---------------------
            'Vizszintes elrendezes
            '---------------------

            Dim Form_BorderWidth = (Me.Width - Me.ClientRectangle.Width) / 2
            Dim Form_TitlebarHeight As Integer = Me.Height - Me.ClientSize.Height - 2 * Form_BorderWidth
            Dim MaxWidth As Integer = CurrentScreen.WorkingArea.Width - 2 * Form_BorderWidth

            Dim GRID_With = FP.GRID.COLUMNS_ALL_WITH(True)

            If GRID_With > MaxWidth Then
                GRID_With = MaxWidth
            End If
            FP.GRID.SET_WITH(GRID_With)


            If WINDOWFORMAT = "MULTITEXTBOX" Then
                Dim wWidth As Integer = GRID_With + Form_BorderWidth * 2

                If wWidth < P.FPc.c.Width Then
                    wWidth = P.FPc.c.Width
                End If

                Me.Width = wWidth
                TextBox_Search_Panel.Width = Me.Width
                TextBox_Search.Width = P.FPc.c.Width

                Dim DeltaLeft As Integer = -(FP.GRID.GRID.RowHeadersWidth + 1)
                Dim wLeft As Integer = P.FPc.c.PointToScreen(New Point(0, 0)).X

                If wLeft + DeltaLeft < CurrentScreen.Bounds.Left Then
                    DeltaLeft = 0
                End If

                Dim DeltaRight As Integer = (CurrentScreen.Bounds.Left + CurrentScreen.Bounds.Width) - (wLeft + DeltaLeft + Me.Width)

                If DeltaRight > 0 Then
                    DeltaRight = 0
                End If
                Me.Left = wLeft + DeltaLeft + DeltaRight
                TextBox_Search.Left = -(DeltaLeft + DeltaRight)

                FPf.ARRANGE_RIGHTS(GRID_Btn_FooterVisible, FPf.Frm, 1)

                FPf.SIZE_WIDTH_TO(GRID_Panel, GRID)

                FPf.SIZE_WIDTH_BETWEEN(GRID_Label, FPf.Frm, GRID_Btn_FooterVisible, 1, 1)
                FPf.ARRANGE_TOPS(GRID_Label, GRID_Btn_FooterVisible)
                FPf.ARRANGE_LEFTS(GRID_Label, FPf.Frm, 1)


                '------------- ScreenShot as Background ----------------------
                Try
                    Dim BitmapFromScreen As New Bitmap(Me.Width, FPf.P_Layout_TextBox_NormalHeight)
                    Dim gBitmapFromScreenShot As Graphics = Graphics.FromImage(ScreenShot)
                    TextBox_Search_Panel.BackgroundImage = ScreenShot.Clone(New Rectangle(New Point((Me.Left - CurrentScreen.Bounds.Left), 0), New Size(Me.Width, FPf.P_Layout_TextBox_NormalHeight)), Imaging.PixelFormat.DontCare)
                Catch ex As Exception
                    'Nothing to do
                End Try
                '------------- ScreenShot as Bacground - END -----------------
            Else
                Me.Width = GRID_With + Form_BorderWidth * 2
                Me.Left = CurrentScreen.Bounds.Left + (CurrentScreen.WorkingArea.Width - Me.Width) / 2

                If MultiSelect Then
                    FPf.ARRANGE_RIGHTS(GRID_Btn_FooterVisible, FPf.Frm, 1)
                Else
                    FPf.ARRANGE_LEFTS(TextBox_Search_Panel, FPf.Frm, 1)
                    If TextBox_Search_Panel.Width > Me.ClientRectangle.Width - 2 Then
                        FPf.SIZE_WIDTH_TO(TextBox_Search_Panel, FPf.Frm, 1, 1)
                        TextBox_Search.Width = TextBox_Search_Panel.Width
                    End If
                    FPf.ARRANGE_RIGHTS(GRID_Btn_FooterVisible, FPf.Frm, 1)
                End If

                FPf.ARRANGE_LEFTS(Btn_Hlp, FPf.Frm, 1)
                FPf.ARRANGE_ON_RIGHT_TOP(Btn_ExcelExport, Btn_Hlp, 1)
                FPf.ARRANGE_RIGHTS(Btn_OK, FPf.Frm, 1)
                FPf.ARRANGE_ON_LEFT_TOP(Btn_Cancel, Btn_OK, 1)
                FPf.SIZE_WIDTH_TO(GRID_Panel, GRID)

                FPf.SIZE_WIDTH_BETWEEN(GRID_Label, FPf.Frm, GRID_Btn_FooterVisible, 1, 1)
                FPf.ARRANGE_TOPS(GRID_Label, GRID_Btn_FooterVisible)
                FPf.ARRANGE_LEFTS(GRID_Label, FPf.Frm, 1)
            End If

            If GRID_Panel.Controls.Count < 1 Then
                FP.GRID.FOOTER_HIDE()
            End If

            If Not FP.CONTROLS.ContainsKey(FIELD_TEXT) Then
                FPApp.DoErrorMsgBox("FP_Simple_SELECT.Shown", 0, String.Format("FP.CONTROLS does not contains the field given in 'FIELD_TEXT' property. (FIELD_TEXT='{0}', FixText key = '{1}')", FIELD_TEXT, P.FixText_Key))
                TextBox_Search_SET_TEXT_AND_SELECTION("", "")
            Else
                If LimitToList Then
                    If GRID_FIND(P.Selected_Text) Then
                        If Not RS_Loaded Then
                            TextBox_Search_SET_TEXT_AND_SELECTION(P.Selected_Text, P.Selected_Text)
                        Else
                            TextBox_Search_SET_TEXT_AND_SELECTION(P.Selected_Text, FP.CONTROLS(FIELD_TEXT).c.Text)
                        End If
                    Else
                        TextBox_Search_SET_TEXT_AND_SELECTION("", "")
                    End If
                Else
                    GRID_FIND(P.Selected_Text)
                    If FP.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                        TextBox_Search_SET_TEXT_AND_SELECTION(P.Selected_Text, FP.CONTROLS(FIELD_TEXT).c.Text)
                    Else
                        TextBox_Search_SET_TEXT_AND_SELECTION(P.Selected_Text, P.Selected_Text)
                    End If
                End If
            End If
            MaxRecords_Label.Left = 0
            MaxRecords_Label.Width = Me.ClientRectangle.Width

            FPf.ARRANGE_LEFTS(SavePoint, TextBox_Search)
            FPf.ARRANGE_TOPS(SavePoint, TextBox_Search)

            If List_Activated_By = ENUM_LIST_ACTIVATED_BY.KEYPRESS_THEN_STAR Then
                Me.Height = Form_TitlebarHeight + TextBox_Search_Panel.Top + TextBox_Search.Height + Form_BorderWidth
            End If

            If MultiSelect Then
                FPf.FOCUS_ON_AT_THE_END(FPc_Selected.c)
            End If
        End If

        FPf.P_GRIDS_SAVE_ALL_Field_Length_ENABLED = True

        FP.GRID.MOVE_GRID_PANEL_ON_ROW()

        If P.PressOK Then
            NAVIGATION_FORWARD()
        End If
    End Sub

    Private Sub FP_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP.CONTROLS_INITIALIZED
        If Not (FPc_Selected Is Nothing) Then
            FPc_Selected.Dispose()
            FPc_Selected = Nothing
        End If

        If FP.CONTROLS.ContainsKey("Selected") Then
            If TypeOf (FP.CONTROLS("Selected").c) Is CheckBox Then
                FPc_Selected = FP.CONTROLS("Selected")
                FP_L_Selected = New FP_L_GRID_RS_Select_Checkbox(FPc_Selected)
                MultiSelect = True
            End If
        End If

        If GRID_Panel.Controls.Count > 0 Then
            FP.GRID.FOOTER_SHOW()
        End If
    End Sub

    Private Sub FP_Form_Current() Handles FP.Form_Current
        'FPf.FOCUS_ON_AT_THE_END(TextBox_Search)

        If FPf.ActiveControl Is Nothing Then
            If MultiSelect Then
                FPc_Selected.c.Focus()
            Else
                TextBox_Search.Focus()
            End If
        ElseIf FPf.ActiveControl.CreatedBy <> ENUM_FP_CONTROL_Created_by.GRID And FPf.ActiveControl.c.Name <> "Selected" Then
            If MultiSelect Then
                FPc_Selected.c.Focus()
            Else
                TextBox_Search.Focus()
            End If
        Else
            'Nothing to do
        End If

        If Not FP.CONTROLS.ContainsKey(FIELD_TEXT) Then
            FPApp.DoErrorMsgBox("FP_Simple_SELECT.FP_Form_Current", 0, String.Format("A FIELD_TEXT-kent megadott '{0}' mezohoz nincs FP_CONTROL megadva!", FIELD_TEXT))
        Else
            Dim SELECTED_TEXT As String = FP.CONTROLS(FIELD_TEXT).c.Text
            TextBox_Search_SET_TEXT_AND_SELECTION("", SELECTED_TEXT)
        End If

        FP_SET_LAYOUT()
    End Sub

    Private Sub FP_SET_LAYOUT()
        FP.COLORING_ALL()
    End Sub

    Private Sub FP_Form_Field_Coloring(sender_FPc As FP_Control, ByRef Handled As Boolean) Handles FP.Form_Field_Coloring
        If Not Handled Then
            With sender_FPc
                If .P.ShowInGRID Then
                    Handled = True
                    .c.BackColor = COLORS_GRID_SELECTEDROW_BG
                End If
            End With
        End If
    End Sub

    Private Sub FP_Form_Field_Enter(ByVal FPc As FP_Control, ByRef Handled As Boolean) Handles FP.Form_Field_Enter
        If Not MultiSelect Then
            FP.FPf.FOCUS_ON_AT_THE_END(TextBox_Search)
            Handled = True
        Else
            If FPc.c.Name <> "Selected" Then
                FP.FPf.FOCUS_ON_AT_THE_END(FPc_Selected.c)
                Handled = True
            End If
        End If
    End Sub

    Private Sub FP_GRID_Filter_Changed() Handles FP.GRID_Filter_Changed
        If FP.P_DATA_RecordStatus <> ENUM_RecordStatus.EXISTS Then
            If RS_Loaded Then
                Dim First_RecID As Long = 0

                If FP.GRID.ROW_GET_FIRSTROW_RECORDID(First_RecID) Then
                    FP.FORM_GOTO_RECORD_BY_ID(First_RecID)
                Else
                    TextBox_Search.Text = ""
                End If
            End If
        End If
    End Sub

    Private Sub FP_GRID_Row_DoubleClick() Handles FP.GRID_Row_DoubleClick
        If Not MultiSelect Then
            If FP.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                TextBox_Search.Text = FP.CONTROLS(FIELD_TEXT).c.Text
                NAVIGATION_FORWARD()
            End If
        End If
    End Sub

    Private Sub Btn_OK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_OK.Click
        NAVIGATION_FORWARD()
    End Sub

    Private Sub TextBox_Search_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TextBox_Search.MouseWheel
        Dim i As Integer
        Dim WheelCount = e.Delta / 120

        If WheelCount > 0 Then
            For i = 1 To WheelCount
                FP.FORM_GOTO_PREVIOUSRECORD()
            Next
        Else
            For i = -1 To WheelCount Step -1
                If Not FP.FORM_GOTO_NEXTRECORD() Then
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub SavePoint_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SavePoint.GotFocus
        FOCUS_ON_IMMEDIATELY(TextBox_Search)

        If WINDOWFORMAT = "MULTITEXTBOX" Then
            NAVIGATION_FORWARD()
        End If
    End Sub

    Private Sub TextBox_Search_TextChanged(sender As Object, e As EventArgs) Handles TextBox_Search.TextChanged
        If P.XLength > 0 Then
            If Len(nz(TextBox_Search.Text, "")) > P.XLength Then
                TextBox_Search.Text = TextBox_Search_OldText
            Else
                TextBox_Search_OldText = TextBox_Search.Text
            End If
        End If
    End Sub

    Private Sub FP_L_Selected_Selection_Changed(sender As FP_L_GRID_RS_Select_Checkbox, DTofChangedIDs As DataTable) Handles FP_L_Selected.Selection_Changed
        FP.RAISEEVENT_Form_AfterUpdate()
    End Sub
End Class
