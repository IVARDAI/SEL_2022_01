Imports System.Data
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class FP_Services_Setup
    Public WithEvents FPApp As FP_App
    Public WithEvents FPf As FP_Form
    Public WithEvents FP_Head As FP
    Public WithEvents FP_Lines As FP
    Public WithEvents FP_ARRANGE As FP
    Public WithEvents FP_HELP As FP
    Public WithEvents FP_TEXTS As FP

    Public Controlled_FPf As FP_Form
    Public Controlled_FPo As FP_ControlObject

    Public DT_DataFields As DataTable

    Public DT_ServerObject_Prefix As DataTable
    Private DT_Controls As DataTable
    Private DT_Resources As DataTable

    Private WithEvents FPc_P_Parent_STR As FP_Control
    Private WithEvents FPc_Forced_NextField_STR As FP_Control
    Private WithEvents FPc_ControlName_STR As FP_Control
    Private WithEvents FPc_P1_Control As FP_Control
    Private WithEvents FPc_P2_Control As FP_Control
    Private WithEvents FPc_Btn_REFRESH As FP_PictureBox
    Private WithEvents FPc_ArrangeType As FP_Control
    Private WithEvents FPc_BG_Image_STR As FP_Control
    Private WithEvents FPc_H_ShortText As FP_Control
    Private WithEvents FPp_H_Btn_Link As FP_PictureBox
    Private WithEvents FPc_T_ShortText As FP_Control

    Private WithEvents FPc_DataField As FP_Control
    Private WithEvents FPc_FieldName As FP_Control
    Private WithEvents FPc_Colors_ForeColor As FP_Control
    Private WithEvents FPc_Colors_BackColor As FP_Control
    Private WithEvents FPc_Colors_SELECTED_FORE As FP_Control
    Private FPc_DT_ID_Field As FP_Control
    Private WithEvents FPc_LabelName As FP_Control
    Private WithEvents FPc_Label_Text As FP_Control
    Private WithEvents FPc_Colors_Label_ForeColor As FP_Control
    Private WithEvents FPc_Colors_Label_BackColor As FP_Control

    Private Sign_CONTROL As Button
    Private Sign_CONTROL_Label As Button
    Private Sign_CONTROL_P1 As Button
    Private Sign_CONTROL_P2 As Button

    Private INITPHASE As Boolean = False

    Private Refresh_FP_ARRANGE_GRID_IN_AfterUpdate As Boolean = False

    Enum Enum_Level As Integer
        FP_FORM = 1
        FP = 2
    End Enum

    Sub New(ByVal MyControlled_FPf As FP_Form, ByVal MyControlled_FPo As FP_ControlObject)
        InitializeComponent()

        FPApp = MyControlled_FPf.FPApp
        Controlled_FPf = MyControlled_FPf
        Controlled_FPo = MyControlled_FPo
    End Sub

    Function ServerObject_Prefix_INIT() As Boolean
        Dim OUT As Boolean = False

        If Controlled_FPf.FPs.Count < 1 Then
            MsgBox("This form hasn't got any FP object.")
        Else
            DT_ServerObject_Prefix = New DataTable

            With DT_ServerObject_Prefix
                .Columns.Add("ID", System.Type.GetType("System.Int32"))
                .Columns.Add("Text", System.Type.GetType("System.String"))
                .Columns.Add("ServerObject_Prefix", System.Type.GetType("System.String"))
                .Columns.Add("SubPrefix", System.Type.GetType("System.String"))
                .Columns.Add("CreatedFromSubPrefix", System.Type.GetType("System.String"))
                .Columns.Add("FieldPrefix", System.Type.GetType("System.String"))
                .Columns.Add("LEVEL", System.Type.GetType("System.Int32"))
            End With

            Dim CurrentID As Long = 1
            Dim Row As DataRow

            Row = DT_ServerObject_Prefix.NewRow
            With Controlled_FPf
                Row("ID") = 0
                Row("ServerObject_Prefix") = .ServerObject_Prefix
                Row("SubPrefix") = ""
                Row("CreatedFromSubPrefix") = ""
                Row("FieldPrefix") = ""
                Row("Text") = .ServerObject_Prefix + " (FORM LEVEL)"
                Row("LEVEL") = Enum_Level.FP_FORM

                DT_ServerObject_Prefix.Rows.Add(Row)
            End With

            For Each Current_FP_ID In Controlled_FPf.FPs.Keys
                Row = DT_ServerObject_Prefix.NewRow

                With Controlled_FPf.FPs(Current_FP_ID)
                    Dim MyText As String = .ServerObject_Prefix

                    If .P_FieldPrefix > "" Then
                        MyText += "; FieldPrefix = " + .P_FieldPrefix
                    End If

                    If nz(.SubPrefix, "") > "" Then
                        'SubPrefix nelkuli objektum hozzaadasa
                        Row("ID") = CurrentID
                        Row("ServerObject_Prefix") = .ServerObject_Prefix
                        Row("SubPrefix") = ""
                        Row("CreatedFromSubPrefix") = .SubPrefix
                        Row("FieldPrefix") = .P_FieldPrefix
                        If .P_FieldPrefix > "" Then
                            Row("Text") = MyText + String.Format(" (Automatic created, SubPrefix = '', FieldPrefix = '{0}')", .P_FieldPrefix)
                        Else
                            Row("Text") = MyText + " (Automatic created, SubPrefix = '')"
                        End If
                        Row("LEVEL") = Enum_Level.FP

                        DT_ServerObject_Prefix.Rows.Add(Row)

                        Row = DT_ServerObject_Prefix.NewRow

                        MyText += " (" + .SubPrefix + ")"

                        CurrentID += 1
                    End If

                    Row("ID") = CurrentID
                    Row("ServerObject_Prefix") = .ServerObject_Prefix
                    Row("SubPrefix") = nz(.SubPrefix, "")
                    Row("CreatedFromSubPrefix") = ""
                    Row("FieldPrefix") = .P_FieldPrefix
                    Row("Text") = MyText
                    Row("LEVEL") = Enum_Level.FP

                    DT_ServerObject_Prefix.Rows.Add(Row)

                End With

                CurrentID += 1
            Next

            Dim INITPHASE_Old As Boolean = INITPHASE
            INITPHASE = True

            FPApp.COMBOBOX_INIT_FROM_DT(ServerObject_Prefix, "ID", "Text", DT_ServerObject_Prefix)

            INITPHASE = INITPHASE_Old

            OUT = True
        End If

        ServerObject_Prefix_INIT = OUT
    End Function

    Sub DT_Resources_SET_Add_All_Bitmaps_From_ASM(ByVal CurrentASM As Reflection.Assembly, ByVal L As List(Of String))
        'Dim CurrentASM As Reflection.Assembly = Reflection.Assembly.Load(ASM_Name)

        Dim ASM_Name As String = CurrentASM.GetName.Name

        For Each ResName As String In CurrentASM.GetManifestResourceNames
            Dim Extension As String = ResName.Substring(ResName.LastIndexOf("."))
            Dim Extension_Pressed As Boolean = (ResName.LastIndexOf("_.") > 0)

            Select Case Extension
                Case ".bmp", ".jpg", ".png", ".gif"
                    If Not Extension_Pressed Then
                        Select Case ASM_Name
                            Case "SEL_SKIN"
                                ResName = Mid(ResName, Len("SEL_SKIN") + 2)
                                L.Add(ResName)

                            Case "SEL_MENU_SKIN"
                                Dim ResName_without_ASM_Name = Mid(ResName, Len("SEL_MENU_SKIN") + 2)
                                If Mid(ResName_without_ASM_Name, 1, 5) <> "FUNC_" Then
                                    ResName = ASM_Name + ".." + Mid(ResName, Len(ASM_Name) + 2)
                                    L.Add(ResName)
                                End If

                            Case Else
                                ResName = ASM_Name + ".." + Mid(ResName, Len(ASM_Name) + 2)
                                L.Add(ResName)
                        End Select
                    End If

                Case Else
                    'Nothing to do
            End Select
        Next
    End Sub

    Sub DT_Resources_SET()
        Dim Row As DataRow

        DT_Resources = New DataTable

        With DT_Resources
            .Columns.Add("ID", System.Type.GetType("System.Int32"))
            .Columns.Add("ResName", System.Type.GetType("System.String"))
        End With

        Dim CurrentID As Integer = 0

        Row = DT_Resources.NewRow
        With Row
            !ID = CurrentID
            !ResName = ""
        End With
        DT_Resources.Rows.Add(Row)

        Dim L As New List(Of String)

        'For pCurrentASM = 0 To Reflection.Assembly.GetEntryAssembly.GetReferencedAssemblies.Count - 1
        For Each AktKey As String In FPApp.SKIN_DLLs.Keys
            'Dim CurrentASM_Name As String = Reflection.Assembly.GetEntryAssembly.GetReferencedAssemblies(pCurrentASM).Name
            Dim CurrentASM As Reflection.Assembly = FPApp.SKIN_DLLs(AktKey)

            DT_Resources_SET_Add_All_Bitmaps_From_ASM(CurrentASM, L)

        Next

        DT_Resources_SET_Add_All_Bitmaps_From_ASM(Reflection.Assembly.GetEntryAssembly, L)

        L.Sort()

        Dim AktRes As String

        For Each AktRes In L
            Row = DT_Resources.NewRow
            CurrentID += 1
            With Row
                !ID = CurrentID
                !ResName = AktRes
            End With

            DT_Resources.Rows.Add(Row)
        Next AktRes


        With FP_Lines.CONTROLS("BG_Image_STR")
            Dim Data_Binded_Old As Boolean = FP_Lines.DATA_Binded

            FP_Lines.DATA_Binded = False
            .DT = DT_Resources.Copy
            FPApp.COMBOBOX_INIT_FROM_DT(.c, "ID", "ResName", .DT)
            FP_Lines.DATA_Binded = Data_Binded_Old
        End With
    End Sub

    Sub DT_CONTROLS_SET()
        Dim Row As DataRow
        Dim Current_FieldPrefix As String = GET_CURRENT_FieldPrefix()
        Dim Length_of_Current_FieldPrefix = Strings.Len(Current_FieldPrefix)

        DT_Controls = New DataTable

        With DT_Controls
            .Columns.Add("ID", System.Type.GetType("System.Int32"))
            .Columns.Add("ControlName", System.Type.GetType("System.String"))
        End With

        Dim CurrentID As Integer = 0

        Row = DT_Controls.NewRow
        With Row
            !ID = CurrentID
            !ControlName = ""
        End With
        DT_Controls.Rows.Add(Row)

        Row = DT_Controls.NewRow
        CurrentID += 1
        With Row
            !ID = CurrentID
            !ControlName = "#PARENT#"
        End With
        DT_Controls.Rows.Add(Row)

        Row = DT_Controls.NewRow
        CurrentID += 1
        With Row
            !ID = CurrentID
            !ControlName = "#PREV#"
        End With
        DT_Controls.Rows.Add(Row)

        Row = DT_Controls.NewRow
        CurrentID += 1
        With Row
            !ID = CurrentID
            !ControlName = "#PREV_LABEL#"
        End With
        DT_Controls.Rows.Add(Row)

        Row = DT_Controls.NewRow
        CurrentID += 1
        With Row
            !ID = CurrentID
            !ControlName = "#FORM#"
        End With
        DT_Controls.Rows.Add(Row)

        Dim KeyList As List(Of String) = Controlled_FPf.CONTROLS.Keys.ToList

        KeyList.Sort()

        Dim AktKey As String

        For Each AktKey In KeyList
            If Mid(AktKey, 1, 1) <> "#" Then
                Dim DoIt As Boolean = True
                Dim AktKey_for_add As String = AktKey

                If Current_FieldPrefix > "" Then
                    AktKey_for_add = Strings.Mid(AktKey, Length_of_Current_FieldPrefix + 1)
                    If Strings.Left(AktKey, Length_of_Current_FieldPrefix) <> Current_FieldPrefix Then
                        DoIt = False
                    End If
                End If

                If DoIt Then
                    Row = DT_Controls.NewRow
                    CurrentID += 1
                    With Row
                        !ID = CurrentID
                        !ControlName = AktKey_for_add
                    End With
                    DT_Controls.Rows.Add(Row)
                End If
            End If
        Next AktKey

        Unbound_ControlCombo_SETROWSOURCE(FPc_ControlName_STR)
        Unbound_ControlCombo_SETROWSOURCE(FPc_P1_Control)
        Unbound_ControlCombo_SETROWSOURCE(FPc_P2_Control)
        Unbound_ControlCombo_SETROWSOURCE(FPc_P_Parent_STR)
        Unbound_ControlCombo_SETROWSOURCE(FPc_Forced_NextField_STR)
    End Sub

    Sub Unbound_ControlCombo_SETROWSOURCE(ByVal FPcombo As FP_Control)
        Dim Old_DataBinded_ByUser = FPcombo.FP.P_DATA_Binded_ByUser
        Dim P_Value_Old As Long = FPcombo.P_VALUE

        FPcombo.FP.P_DATA_Binded_ByUser = False

        FPcombo.DT = DT_Controls.Copy
        FPApp.COMBOBOX_INIT_FROM_DT(FPcombo.c, "ID", "ControlName", FPcombo.DT)

        If P_Value_Old <> 0 Then
            FPcombo.P_VALUE = P_Value_Old
        End If

        FPcombo.FP.P_DATA_Binded_ByUser = Old_DataBinded_ByUser
    End Sub

    Private Sub FP_Setup_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Sign_REMOVE_ALL()
        Controlled_FPf.HELP_HIDE()
    End Sub

    Private Sub FP_Setup_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim FPf_CONTROLS As New Struct_FP_FORM_CONTROLS_COLLECTION
        Dim FP_Head_CONTROLS As New Struct_FP_CONTROLS_COLLECTION
        Dim FP_Lines_CONTROLS As New Struct_FP_CONTROLS_COLLECTION
        Dim FP_Lines_GRID As New Struct_FP_GRID_CONTROL_COLLECTION
        Dim FP_A_CONTROLS As New Struct_FP_CONTROLS_COLLECTION
        Dim FP_H_CONTROLS As New Struct_FP_CONTROLS_COLLECTION
        Dim FP_T_CONTROLS As New Struct_FP_CONTROLS_COLLECTION

        FPf = New FP_Form("RS_SETUP", FPApp, Me, True, "")
        FP_Head = New FP(FPf, "RS_SETUP_HEAD", "", True)
        FP_Lines = New FP(FPf, "RS_Fields_DEBUG", "")
        FP_ARRANGE = New FP(FPf, "RS_ARRANGE_DEBUG", "")
        FP_HELP = New FP(FPf, "RS_SETUP_HELP")
        FP_TEXTS = New FP(FPf, "RS_SETUP_TEXTS")

        With FPf_CONTROLS
            .Btn_HELP = Btn_Help
        End With
        FPf.INIT_CONTROLS(FPf_CONTROLS)

        With FP_Head_CONTROLS
            'No specified controls
        End With
        FP_Head.INIT_CONTROLS(FP_Head_CONTROLS)

        With FP_Lines_GRID
            .GRID = GRID_Fields
        End With

        With FP_Lines_CONTROLS
            .GRID = FP_Lines_GRID
            .Btn_Del = Btn_Delete
        End With
        FP_Lines.INIT_CONTROLS(FP_Lines_CONTROLS)

        With FP_A_CONTROLS
            With .GRID
                .GRID = A_GRID
                .Label = A_GRID_Label
                .Btn_FooterVisible = A_GRID_BtnFooter
                .Footer_Panel = A_GRID_Panel
            End With
        End With
        FP_ARRANGE.INIT_CONTROLS(FP_A_CONTROLS)

        With FP_H_CONTROLS
            With .GRID
                .GRID = H_GRID
                .Label = H_GRID_Label
                .Btn_FooterVisible = H_GRID_BtnFooter
                .Footer_Panel = H_GRID_Panel
            End With
        End With
        FP_HELP.INIT_CONTROLS(FP_H_CONTROLS)

        With FP_T_CONTROLS
            With .GRID
                .GRID = T_GRID
                .Label = T_GRID_Label
                .Btn_FooterVisible = T_GRID_BtnFooter
                .Footer_Panel = T_GRID_Panel
            End With
        End With
        FP_TEXTS.INIT_CONTROLS(FP_T_CONTROLS)

        FPf.SIZE_HEIGHT_TO_MAX(Me)

        FP_Lines.GRID.FOOTER_SHOW()
        FP_ARRANGE.GRID.FOOTER_SHOW()
        FP_TEXTS.GRID.FOOTER_SHOW()
        FP_HELP.GRID.FOOTER_SHOW()

        DT_Resources_SET()
    End Sub

    Private Sub Sign_SET_ALL()
        If FPf.ActiveControl Is Nothing Then
            Sign_HIDE_ALL()
        Else
            If FPf.ActiveControl.FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
                Sign_HIDE_ALL()
            Else
                If FPf.ActiveControl.FP.Equals(FP_ARRANGE) Then
                    Dim Current_FieldPrefix As String = GET_CURRENT_Controlled_FP_FieldPrefix()

                    Sign_SET(Sign_CONTROL, Current_FieldPrefix + ControlName_STR.Text)
                    Sign_HIDE(Sign_CONTROL_Label)
                    Sign_SET(Sign_CONTROL_P1, Current_FieldPrefix + P1_Control_STR.Text)
                    Sign_SET(Sign_CONTROL_P2, Current_FieldPrefix + P2_Control_STR.Text)

                    If Not (GET_CURRENT_Controlled_FP() Is Nothing) Then
                        With GET_CURRENT_Controlled_FP()
                            If .GRID_EXISTS Then
                                If .CONTROLS.ContainsKey(Current_FieldPrefix + ControlName_STR.Text) Then
                                    Dim FPc As FP_Control = .CONTROLS(Current_FieldPrefix + ControlName_STR.Text)

                                    If FPc_HAS_FIELD(FPc) Then
                                        If FPc.P.ShowInGRID Then
                                            .GRID.COLUMN_ENSURE_VISIBLE(FPc.c.Name)
                                        End If
                                    End If
                                End If
                            End If
                        End With
                    End If

                ElseIf FPf.ActiveControl.FP.Equals(FP_Lines) Then
                    If FieldName.Text = "" Then
                        Sign_HIDE_ALL()
                    Else
                        Dim Current_FieldPrefix As String = GET_CURRENT_Controlled_FP_FieldPrefix()

                        If ShowInGRID.Checked Then
                            If Not (GET_CURRENT_Controlled_FP() Is Nothing) Then
                                With GET_CURRENT_Controlled_FP()
                                    If .GRID_EXISTS Then

                                        .GRID.COLUMN_ENSURE_VISIBLE(Current_FieldPrefix + FieldName.Text)
                                    End If
                                End With
                            End If
                            Sign_SET(Sign_CONTROL, Current_FieldPrefix + FieldName.Text)
                            Sign_HIDE(Sign_CONTROL_Label)
                        Else
                            Sign_SET(Sign_CONTROL, Current_FieldPrefix + FieldName.Text)
                            Sign_SET(Sign_CONTROL_Label, Current_FieldPrefix + LabelName.Text)
                        End If

                        Sign_HIDE(Sign_CONTROL_P1)
                        Sign_HIDE(Sign_CONTROL_P2)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Sign_CREATE(ByRef Sign_c As Button, ByVal Name As String, ByVal BG_Color As Color)
        If Sign_c Is Nothing Then
            Sign_c = New Button
            With Sign_c
                .Name = Name
                .BackColor = BG_Color
                .Text = ""
                .Width = 15
                .Height = 15
            End With
        End If
    End Sub

    Private Sub Sign_HIDE(ByVal Sign_c As Control)
        Sign_c.Parent = Controlled_FPf.Frm
        Sign_c.Visible = False
    End Sub

    Private Sub Sign_CREATE_ALL()
        Sign_CREATE(Sign_CONTROL, "#FP_SETUP_SIGN_CONTROL#", System.Drawing.Color.Red)
        Sign_CREATE(Sign_CONTROL_Label, "#FP_SETUP_SIGN_CONTROL_LABEL#", System.Drawing.Color.Blue)
        Sign_CREATE(Sign_CONTROL_P1, "#FP_SETUP_SIGN_CONTROL_P1#", System.Drawing.Color.Blue)
        Sign_CREATE(Sign_CONTROL_P2, "#FP_SETUP_SIGN_CONTROL_P2#", System.Drawing.Color.Blue)
    End Sub

    Private Sub Sign_HIDE_ALL()
        Sign_HIDE(Sign_CONTROL)
        Sign_HIDE(Sign_CONTROL_Label)
        Sign_HIDE(Sign_CONTROL_P1)
        Sign_HIDE(Sign_CONTROL_P2)
    End Sub

    Private Sub Sign_REMOVE_ALL()
        Sign_REMOVE(Sign_CONTROL)
        Sign_REMOVE(Sign_CONTROL_Label)
        Sign_REMOVE(Sign_CONTROL_P1)
        Sign_REMOVE(Sign_CONTROL_P2)
    End Sub

    Private Sub Sign_REMOVE(ByVal Sign_c As Control)
        If Not (Sign_c Is Nothing) Then
            Sign_c.Dispose()
        End If
    End Sub

    Private Sub getPrevControlsAtRowSeqNum(ByVal SeqNum As Integer, ByRef Prev_Name As String, ByRef Prev_Label_Name As String)
        Prev_Name = ""
        Prev_Label_Name = ""

        If Not (Controlled_FPf Is Nothing) Then
            If Not (Controlled_FPf.CONTROLS Is Nothing) Then
                If Not (FP_ARRANGE.GRID.DT Is Nothing) Then
                    With FP_ARRANGE.GRID.DT
                        If SeqNum > .Rows.Count Or SeqNum < 1 Then
                            SeqNum = .Rows.Count - 1
                        End If
                    End With

                    If SeqNum > 0 Then
                        Dim wDT As DataTable = FP_ARRANGE.GRID.DT_Clone_AllRows

                        For i As Integer = 1 To SeqNum - 1

                            'Dim Row As DataRow = FP_ARRANGE.GRID.DT.Select(String.Format("SeqNum = {0}", i)).First
                            Dim Row As DataRow = wDT.Select(String.Format("SeqNum = {0}", i)).First

                            If Controlled_FPf.CONTROLS.ContainsKey(Row!ControlName_STR) Then
                                If TypeOf Controlled_FPf.CONTROLS(Row!ControlName_STR) Is Label Then
                                    Prev_Label_Name = Row!ControlName_STR
                                Else
                                    Prev_Name = Row!ControlName_STR
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Sign_SET(ByVal Sign_c As Button, ByVal for_ControlName As String)
        Sign_HIDE(Sign_c)
        If for_ControlName > "" Then
            Dim Prev_Name As String = ""
            Dim Prev_Label_Name As String = ""

            If for_ControlName = "#PREV#" Or for_ControlName = "#PREV_LABEL#" Then
                getPrevControlsAtRowSeqNum(FP_ARRANGE.P_DATA_Current_SeqNum, Prev_Name, Prev_Label_Name)
                Select Case for_ControlName
                    Case "#PREV#"
                        for_ControlName = Prev_Name

                    Case "#PREV_LABEL#"
                        for_ControlName = Prev_Label_Name

                    Case Else
                        FPApp.DoErrorMsgBox("FP_Setup.Sign_SET", 0, String.Format("Unknown Virtual Controlname '{0}'", for_ControlName))
                End Select
            End If

            If Controlled_FPf.CONTROLS.ContainsKey(for_ControlName) Then
                Dim for_Control As Control = Controlled_FPf.CONTROLS(for_ControlName)
                'If Not for_Control.Visible Then
                FIELD_VISIBLE_WITH_ENSURE(for_Control)
                'End If
                With Sign_c
                    If for_Control.Parent Is Nothing Then
                        .Parent = Controlled_FPf.Frm
                        .Left = for_Control.Right - Sign_c.Width
                        '.Left = for_Control.Left
                        .Top = for_Control.Top
                    ElseIf TypeOf (for_Control) Is TabPage Then
                        .Parent = for_Control
                        .Left = 0
                        .Top = 0
                    Else
                        If Not (TypeOf (for_Control.Parent) Is TabControl Or TypeOf (for_Control.Parent) Is SplitContainer) Then
                            .Parent = for_Control.Parent
                        End If
                        .Left = for_Control.Right - Sign_c.Width
                        '.Left = for_Control.Left
                        .Top = for_Control.Top
                    End If

                    .Visible = True
                    .BringToFront()
                End With
            End If
        End If
    End Sub

    Public Sub GOTO_FPo(ByVal FPo As FP_ControlObject)
        If FPo Is Nothing Then
            ServerObject_Prefix.SelectedValue = 0
            ServerObject_Prefix_SET_FORM_TO_SELECTED()
            FP_Lines.FORM_GOTO_NORECORD()
        Else
            If FPo.FP Is Nothing Then
                'FP_Form Level

                Dim Criteria As String = String.Format("ServerObject_Prefix = '{0}' And SubPrefix = '' And LEVEL={1}", FPo.FPf.ServerObject_Prefix, Val(Enum_Level.FP_FORM).ToString)

                If DT_ServerObject_Prefix.Select(Criteria).Count > 0 Then
                    Dim Row As DataRow = DT_ServerObject_Prefix.Select(Criteria).First

                    ServerObject_Prefix.SelectedValue = Row.Item(ServerObject_Prefix.ValueMember)
                    ServerObject_Prefix_SET_FORM_TO_SELECTED()

                    Criteria = String.Format("FieldName = '{0}'", FPo.P_FieldName)

                    With FP_Lines
                        If .GRID.DT.Select(Criteria).Count > 0 Then
                            .FORM_GOTO_RECORD_BY_ID(.GRID.DT.Select(Criteria).First!RecordID)
                        End If
                    End With
                End If
            Else
                'FP Level

                Dim Criteria As String = String.Format("ServerObject_Prefix = '{0}' And ISNULL(SubPrefix, '') = '{1}' And LEVEL={2}", FPo.FP.ServerObject_Prefix, FPo.FP.SubPrefix, Val(Enum_Level.FP).ToString)

                If DT_ServerObject_Prefix.Select(Criteria).Count > 0 Then
                    Dim Row As DataRow = DT_ServerObject_Prefix.Select(Criteria).First

                    ServerObject_Prefix.SelectedValue = Row.Item(ServerObject_Prefix.ValueMember)

                    Criteria = String.Format("FieldName = '{0}'", FPo.P_FieldName)

                    With FP_Lines
                        If .GRID.DT.Select(Criteria).Count > 0 Then
                            .FORM_GOTO_RECORD_BY_ID(.GRID.DT.Select(Criteria).First!RecordID)
                        Else
                            If FPo.FP.SubPrefix > "" Then
                                Criteria = String.Format("ServerObject_Prefix = '{0}' And SubPrefix = ''", FPo.FP.ServerObject_Prefix)

                                If DT_ServerObject_Prefix.Select(Criteria).Count > 0 Then
                                    Row = DT_ServerObject_Prefix.Select(Criteria).First

                                    ServerObject_Prefix.SelectedValue = Row.Item(ServerObject_Prefix.ValueMember)

                                    Criteria = String.Format("FieldName = '{0}'", FPo.P_FieldName)

                                    With FP_Lines
                                        If .GRID.DT.Select(Criteria).Count > 0 Then
                                            .FORM_GOTO_RECORD_BY_ID(.GRID.DT.Select(Criteria).First!RecordID)
                                        End If
                                    End With
                                End If
                            End If
                        End If
                    End With
                End If
            End If
        End If
    End Sub

    Private Sub FP_Setup_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If Not ServerObject_Prefix_INIT() Then
            Me.Close()
        Else
            DT_CONTROLS_SET()
            Unbound_ResCombo_SETVALUE(FPc_BG_Image_STR, "BG_Image")

            GOTO_FPo(Controlled_FPo)
        End If
    End Sub

    Sub DT_DataFields_CreateEmptyTable()
        DT_DataFields = New DataTable

        With DT_DataFields
            .Columns.Add("ID", System.Type.GetType("System.Int32"))
            .Columns.Add("FieldName", System.Type.GetType("System.String"))
            .Columns.Add("xtype_VB", System.Type.GetType("System.String"))
        End With

        Dim Row As DataRow = DT_DataFields.NewRow

        With Row
            !ID = 0
            !FieldName = ""
            !xtype_VB = ""
        End With
        DT_DataFields.Rows.Add(Row)
    End Sub

    Function DT_DataFields_INIT() As Boolean
        Dim OUT As Boolean = True

        Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

        Select Case GET_CURRENT_Level()
            Case Enum_Level.FP_FORM
                DT_DataFields_CreateEmptyTable()

            Case Enum_Level.FP
                Dim Current_FP As FP = GET_CURRENT_Controlled_FP()

                If Current_FP.UnboundForm Then
                    DT_DataFields_CreateEmptyTable()
                Else
                    With FPf
                        .Qdf_set_SP(sqlComm, "RS_SETUP_DataField_SET_ROWSOURCE")
                        .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                        .Qdf_AddParameter(sqlComm, "@Name_Of_Proc_READ", SqlDbType.NVarChar, , 255, Current_FP.SQL_BIND_Params.NameOf_READ)
                        .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                        .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)

                        CURSOR_SHOW_WAIT()

                        Try
                            If .Qdf_Execute(sqlComm) Then
                                If sqlComm.Parameters("@Result").Value <> -1 Then
                                    Dim Result As Long
                                    Dim ErrText As String

                                    Result = sqlComm.Parameters("@Result").Value
                                    ErrText = sqlComm.Parameters("@ErrText").Value

                                    OUT = False
                                    MsgBox(String.Format("FP_Setup.DataField_INIT: @Result = {0}. {1}", Result.ToString, ErrText))
                                Else
                                    '+++FPApp.SQL_CLOSE()
                                    Dim MySQL As String = String.Format("SELECT ID ID, Dispo1Varchar FieldName, Dispo2Varchar xtype_VB FROM Dispo1 WITH (READUNCOMMITTED) WHERE Terminal = '{0}' And Art = 'FP_SETUP_DATAFIELDS'", Terminal)
                                    Dim DA As New SqlDataAdapter(MySQL, FPApp.DC.CNN)

                                    DT_DataFields = New DataTable

                                    DA.Fill(DT_DataFields)
                                End If
                            End If

                        Catch ex As Exception
                            OUT = False
                            FPApp.DoErrorMsgBox("FP_Setup.DataField_INIT", Err.Number, Err.Description)
                        End Try

                        CURSOR_SHOW_DEFAULT()
                    End With
                End If

            Case Else
                FPApp.DoErrorMsgBox("FP_Setup.DataField_INIT", 0, "Unknown Level")
        End Select

        If OUT Then
            Dim DATA_Binded_OLD = FP_Lines.DATA_Binded

            FP_Lines.DATA_Binded = False

            FPc_DataField.DT = DT_DataFields.Copy
            FPApp.COMBOBOX_INIT_FROM_DT(DataField, "ID", "FieldName", DT_DataFields)
            FPc_DT_ID_Field.DT = DT_DataFields.Copy
            FPApp.COMBOBOX_INIT_FROM_DT(DT_ID_Field, "ID", "FieldName", DT_DataFields)
            FP_Lines.DATA_Binded = DATA_Binded_OLD
        End If

        DT_DataFields_INIT = OUT
    End Function

    Private Sub ServerObject_Prefix_SET_FORM_TO_SELECTED()
        If ServerObject_Prefix.ValueMember > "" And ServerObject_Prefix.DisplayMember > "" Then
            DT_DataFields_INIT()
            DT_CONTROLS_SET()

            With DT_ServerObject_Prefix.Select(String.Format("ID={0}", ServerObject_Prefix.SelectedValue)).First
                Dim SubWhere As String = String.Format("ServerObject_Prefix = '{0}' And SubPrefix = '{1}'", !ServerObject_Prefix, !SubPrefix)
                If Not FP_Lines.FORM_RECORDS_LOAD(SubWhere) Then
                    FP_Lines.FORM_RECORDS_LOAD(SubWhere, True)
                End If

                If Not FP_ARRANGE.FORM_RECORDS_LOAD(SubWhere) Then
                    FP_ARRANGE.FORM_RECORDS_LOAD(SubWhere, True)
                End If
            End With

        End If
    End Sub

    Private Sub ServerObject_Prefix_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ServerObject_Prefix.SelectedValueChanged
        If Not INITPHASE Then
            ServerObject_Prefix_SET_FORM_TO_SELECTED()
        End If
    End Sub

    Private Function RECORD_UPDOWN(ByVal RS_ID As Long, ByVal UniqueTable As String, ByVal Current_ID As Long, ByVal SeqNum_Field As String, ByVal UpDown As Enum_UpDown) As Boolean
        Dim OUT As Boolean = False

        If FPf.FPApp.InitGlobals() Then
            Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()
            Dim Result As Boolean

            FPf.FPApp.DC.Qdf_set_SP(sqlComm, "Form_ButtonUpDown_Click_Standard")
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , RS_ID)
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@UniqueTable", SqlDbType.NVarChar, , 255, UniqueTable)
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@CurrentRecordID", SqlDbType.Int, , , , , Current_ID)
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@SeqNum_Field", SqlDbType.NVarChar, , 255, SeqNum_Field)
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@UpDown", SqlDbType.Int, , , , , UpDown)
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)

            CURSOR_SHOW_WAIT()

            Try
                Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
            Catch ex As Exception
                Result = False
                FPf.FPApp.DoErrorMsgBox("OrderEntry.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
            End Try

            CURSOR_SHOW_DEFAULT()

            If Result Then
                OUT = True
            End If
        End If

        RECORD_UPDOWN = OUT
    End Function

    Private Sub REFRESH_CONTROLS()
        Dim FPc_LastActive As FP_Control = FPf.ActiveControl

        Sign_HIDE_ALL()

        If Not FPc_Btn_REFRESH.P_Pressed Then
            Select Case GET_CURRENT_Level()
                Case Enum_Level.FP_FORM
                    Controlled_FPf.CONTROLS_REFRESH_FROM_RS()
                    DT_CONTROLS_SET()

                Case Enum_Level.FP
                    Dim Current_Controlled_FP As FP = GET_CURRENT_Controlled_FP()

                    Current_Controlled_FP.CONTROLS_REFRESH_FROM_RS()
                    DT_CONTROLS_SET()

                Case Else
                    FPApp.DoErrorMsgBox("FP_Setup.REFRESH_CONTROLS", 0, "Unknown Level.")
            End Select
        End If
        Unbound_Combos_FP_Lines_SET_ALL_VALUES()
        Unbound_Combos_FP_ARRANGE_SET_ALL_VALUES()

        If Not (FPc_LastActive Is Nothing) Then
            FPf.FOCUS_ON_AT_THE_END(FPc_LastActive.c, , , True)
        End If

        Sign_SET_ALL()
    End Sub

    Private Function GET_CURRENT_ServerObject_Prefix() As String
        Dim AktServerObject_Prefix As DataRow = DT_ServerObject_Prefix.Select(String.Format("ID = {0}", ServerObject_Prefix.SelectedValue)).First

        GET_CURRENT_ServerObject_Prefix = AktServerObject_Prefix!ServerObject_Prefix
    End Function

    Private Function GET_CURRENT_Level() As Integer
        Dim AktServerObject_Prefix As DataRow = DT_ServerObject_Prefix.Select(String.Format("ID = {0}", ServerObject_Prefix.SelectedValue)).First

        GET_CURRENT_Level = AktServerObject_Prefix!LEVEL
    End Function

    Private Function GET_CURRENT_SubPrefix() As String
        Dim AktServerObject_Prefix As DataRow = DT_ServerObject_Prefix.Select(String.Format("ID = {0}", ServerObject_Prefix.SelectedValue)).First

        GET_CURRENT_SubPrefix = nz(AktServerObject_Prefix!SubPrefix, "")
    End Function

    Private Function GET_CURRENT_FieldPrefix() As String
        Dim AktServerObject_Prefix As DataRow = DT_ServerObject_Prefix.Select(String.Format("ID = {0}", ServerObject_Prefix.SelectedValue)).First

        Return nz(AktServerObject_Prefix!FieldPrefix, "")
    End Function

    Private Function GET_CURRENT_CreatedFromSubPrefix() As String
        Dim AktServerObject_Prefix As DataRow = DT_ServerObject_Prefix.Select(String.Format("ID = {0}", ServerObject_Prefix.SelectedValue)).First

        GET_CURRENT_CreatedFromSubPrefix = AktServerObject_Prefix!CreatedFromSubPrefix
    End Function

    Private Function GET_CURRENT_Controlled_FP_FieldPrefix() As String
        Dim OUT As String = ""

        Dim CurrFP As FP = GET_CURRENT_Controlled_FP()

        If Not (CurrFP Is Nothing) Then
            OUT = CurrFP.P_FieldPrefix
        End If

        Return OUT
    End Function

    Private Function GET_CURRENT_Controlled_FP() As FP
        Dim MySubPrefix As String = GET_CURRENT_CreatedFromSubPrefix()
        Dim MyFieldPrefix As String = GET_CURRENT_FieldPrefix()

        If MySubPrefix = "" Then
            MySubPrefix = GET_CURRENT_SubPrefix()
        End If

        GET_CURRENT_Controlled_FP = Controlled_FPf.GET_FP(GET_CURRENT_ServerObject_Prefix, MySubPrefix, MyFieldPrefix)
    End Function

    Private Sub FP_Lines_Form_AfterDelete(ByVal sender_FP As FP) Handles FP_Lines.Form_AfterDelete
        REFRESH_CONTROLS()
        FP_ARRANGE.FORM_DORESYNC(True)
    End Sub

    Private Sub FP_Lines_Form_AfterUpdate(ByVal sender_FP As FP) Handles FP_Lines.Form_AfterUpdate
        REFRESH_CONTROLS()
        Unbound_Combos_FP_Lines_SET_ALL_VALUES()
        If FieldName.Text > "" Then
            If Controlled_FPf.CONTROLS.ContainsKey(FieldName.Text) Then
                Controlled_FPf.CONTROLS(FieldName.Text).Invalidate()
            End If
        End If

        If Refresh_FP_ARRANGE_GRID_IN_AfterUpdate Then
            Refresh_FP_ARRANGE_GRID_IN_AfterUpdate = False

            Dim CurrentID As Long = FP_ARRANGE.P_DATA_Current_ID

            FP_ARRANGE.FORM_DORESYNC(True)
            FP_ARRANGE.FORM_GOTO_RECORD_BY_ID(CurrentID)
        End If

        Dim GotoFirstField As Boolean = True
        If Not (FPf.ActiveControl Is Nothing) Then
            If FPf.ActiveControl.FP.Equals(FP_Lines) Then
                If FPf.ActiveControl.P.ShowInGRID Then
                    GotoFirstField = False
                End If
            End If
        End If
        If GotoFirstField Then
            FPf.FOCUS_ON_AT_THE_END(DataField)
        End If
    End Sub

    Private Sub FP_Lines_Form_BeforeUpdate(ByVal sender_FP As FP, ByRef Cancel As Integer) Handles FP_Lines.Form_BeforeUpdate
        With FP_Lines
            .DATA_Field_setValue("ServerObject_Prefix", GET_CURRENT_ServerObject_Prefix)
            .DATA_Field_setValue("SubPrefix", GET_CURRENT_SubPrefix)
            .DATA_Field_setValue("P_Parent", P_Parent_STR.Text)
            .DATA_Field_setValue("Forced_NextField", Forced_NextField_STR.Text)
            .DATA_Field_setValue("BG_Image", BG_Image_STR.Text)
        End With

        If FPc_FieldName.P_VALUE <> FPc_FieldName.P_VALUE_Saved Then
            Refresh_FP_ARRANGE_GRID_IN_AfterUpdate = True
        End If
    End Sub

    Private Sub Call_Select_Control_Setup(FixText_Key As String)
        Dim Set_Form As New FP_Services_Select_Control_Setup(FixText_Key)

        Set_Form.ShowDialog()
        If Set_Form.DialogResult = DialogResult.OK Then
            Dim FixText As String = Set_Form.FixText
            FPApp.FIXTEXT_SAVE_SIMPLE(FixText_Key, FixText)
            FP_Lines.CONTROLS("DT_FixText_Key").DT_REFRESH()
            FPf.FPApp.COMBOBOX_FIND(DT_FixText_Key, FixText_Key)
            REFRESH_CONTROLS()
        End If
    End Sub
    Private Sub Btn_DTFixtTextKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_DTFixtTextKey.Click
        If FP_Lines.FORM_RECORDS_SAVE_CURRENT Then
            If FP_Lines.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                Dim FixText_Key As String = DT_FixText_Key.Text
                Dim FixText As String = ""
                If FixText_Key > "" Then
                    FixText = FPf.FPApp.getFixText(DT_FixText_Key.Text)
                End If
                Dim FieldType As Integer = 0

                If CreateAtRuntime.Checked Then
                    FieldType = FP_Lines.CONTROLS("CreateAtRuntime_FieldType").P_VALUE
                Else
                    If Not Controlled_FPf.CONTROLS.ContainsKey(FieldName.Text) Then
                        FieldType = -1
                        FPApp.DoErrorMsgBox("FP_SETUP.Btn_DTFixTextKey_Click", 0, String.Format("Field '{0}' in CONTROLS not found.", FieldName.Text))
                    Else
                        Dim c As Control = Controlled_FPf.CONTROLS(FieldName.Text)

                        If TypeOf (c) Is TextBox Then
                            FieldType = 0
                        ElseIf TypeOf (c) Is ComboBox Then
                            FieldType = 1
                        ElseIf TypeOf (c) Is CheckBox Then
                            FieldType = 2
                        ElseIf TypeOf (c) Is Label Then
                            FieldType = 3
                        ElseIf TypeOf (c) Is ListView Then
                            FieldType = 4
                        ElseIf TypeOf (c) Is RichTextBox Then
                            FieldType = 5
                        Else
                            FieldType = -1
                            FPApp.DoErrorMsgBox("FP_SETUP.Btn_DTFixTextKey_Click", 0, String.Format("Field '{0}' has an unknown fieldtype.", FieldName.Text))
                        End If
                    End If
                End If

                If FixText_Key.IndexOf("@@VB_SELECT_CONTROL") = 0 Then
                    If FieldType = 0 Then
                        Call_Select_Control_Setup(FixText_Key)
                    Else
                        FieldType = -1
                        FPApp.DoErrorMsgBox("FP_SETUP.Btn_DTFixTextKey_Click", 0, String.Format("Field '{0}' has an unknown fieldtype.", FieldName.Text))
                    End If
                Else
                    Dim FixText_Edit As FP_Simple_Edit

                    Dim Params As New Dictionary(Of String, String)

                    If FPf.FPApp.FIXTEXT_SPLIT_PARAMS(FixText, Params) Then

                        Select Case FieldType
                            Case 1 'Combobox
                                FixText_Edit = New FP_Simple_Edit(FPApp, "@@VB_COMBO")

                                Dim REFRESH_INT As Integer = 0

                                With FixText_Edit
                                    .DATAFIELD_ADD("FIXTEXT_Key", FixText_Key)
                                    FixText_Key = ""

                                    Select Case DIC_GET("REFRESH", Params)
                                        Case "" : REFRESH_INT = 0
                                        Case "FORM_CURRENT" : REFRESH_INT = 1
                                        Case "FORM_AFTERUPDATE" : REFRESH_INT = 2
                                        Case Else
                                            REFRESH_INT = 0
                                            FPApp.DoErrorMsgBox(FPf, "FP_Services_Setup.Btn_DTFixTextKey_Click", 0, String.Format("Unknown value for parameter 'REFRESH' ('{0}')", DIC_GET("REFRESH", Params)))
                                    End Select

                                    .DATAFIELD_ADD("SELECT_0", DIC_GET("SELECT_0", Params), , 1024)
                                    .DATAFIELD_ADD("SELECT", DIC_GET("SELECT", Params), , 1024)
                                    .DATAFIELD_ADD("FROM", DIC_GET("FROM", Params), , 1024)
                                    .DATAFIELD_ADD("WHERE", DIC_GET("WHERE", Params), , 1024)
                                    .DATAFIELD_ADD("ORDERBY", DIC_GET("ORDERBY", Params), , 1024)
                                    .DATAFIELD_ADD("VALUEMEMBER", DIC_GET("VALUEMEMBER", Params), , 1024)
                                    .DATAFIELD_ADD("DISPLAYMEMBER", DIC_GET("DISPLAYMEMBER", Params), , 1024)
                                    .DATAFIELD_ADD("REFRESH", REFRESH_INT)
                                    .DATAFIELD_ADD("WHERE_FOR_LIST", DIC_GET("WHERE_FOR_LIST", Params), , 1024)
                                End With
                                If FPApp.ShowDialogForm(FixText_Edit) = Windows.Forms.DialogResult.OK Then
                                    Dim REFRESH_STR As String = ""

                                    With FixText_Edit
                                        FixText_Key = Trim(.DATAFIELD_GET("FIXTEXT_Key"))

                                        Select Case Val(.DATAFIELD_GET("REFRESH"))
                                            Case 0 : REFRESH_STR = ""
                                            Case 1 : REFRESH_STR = "FORM_CURRENT"
                                            Case 2 : REFRESH_STR = "FORM_AFTERUPDATE"
                                            Case Else : FixText = ""
                                        End Select

                                        FixText = "SELECT_0        = " + .DATAFIELD_GET("SELECT_0") + vbCrLf +
                                                  "SELECT          = " + .DATAFIELD_GET("SELECT") + vbCrLf +
                                                  "FROM            = " + .DATAFIELD_GET("FROM") + vbCrLf +
                                                  "WHERE           = " + .DATAFIELD_GET("WHERE") + vbCrLf +
                                                  "ORDERBY         = " + .DATAFIELD_GET("ORDERBY") + vbCrLf +
                                                  "VALUEMEMBER     = " + .DATAFIELD_GET("VALUEMEMBER") + vbCrLf +
                                                  "DISPLAYMEMBER   = " + .DATAFIELD_GET("DISPLAYMEMBER") + vbCrLf +
                                                  "REFRESH         = " + REFRESH_STR + vbCrLf +
                                                  "WHERE_FOR_LIST  = " + .DATAFIELD_GET("WHERE_FOR_LIST")
                                    End With
                                End If

                            Case 4 'ListView
                                FixText_Edit = New FP_Simple_Edit(FPApp, "@@VB_LISTVIEW")

                                Dim REFRESH_INT As Integer = 0

                                With FixText_Edit
                                    .DATAFIELD_ADD("FIXTEXT_Key", FixText_Key)
                                    FixText_Key = ""

                                    Select Case DIC_GET("REFRESH", Params)
                                        Case "" : REFRESH_INT = 0
                                        Case "FORM_CURRENT" : REFRESH_INT = 1
                                        Case "FORM_AFTERUPDATE" : REFRESH_INT = 2
                                        Case Else
                                            REFRESH_INT = 0
                                            FPApp.DoErrorMsgBox(FPf, "FP_Services_Setup.Btn_DTFixTextKey_Click", 0, String.Format("Unknown value for parameter 'REFRESH' ('{0}')", DIC_GET("REFRESH", Params)))
                                    End Select

                                    .DATAFIELD_ADD("COUNTOFCOLUMNS", DIC_GET("COUNTOFCOLUMNS", Params), , 1024)
                                    .DATAFIELD_ADD("SELECT", DIC_GET("SELECT", Params), , 1024)
                                    .DATAFIELD_ADD("FROM", DIC_GET("FROM", Params), , 1024)
                                    .DATAFIELD_ADD("WHERE", DIC_GET("WHERE", Params), , 1024)
                                    .DATAFIELD_ADD("ORDERBY", DIC_GET("ORDERBY", Params), , 1024)
                                    .DATAFIELD_ADD("VALUEMEMBER", DIC_GET("VALUEMEMBER", Params), , 1024)
                                    .DATAFIELD_ADD("SEQ_KEY_HEADERS", DIC_GET("SEQ_KEY_HEADERS", Params), , 1024)
                                    .DATAFIELD_ADD("COLUMNWIDTHS", DIC_GET("COLUMNWIDTHS", Params), , 1024)
                                    .DATAFIELD_ADD("ALIGNS", DIC_GET("ALIGNS", Params), , 1024)
                                    .DATAFIELD_ADD("FORMATS", DIC_GET("FORMATS", Params), , 1024)
                                    .DATAFIELD_ADD("CHECKBOXES", DIC_GET("CHECKBOXES", Params), , 1024)
                                    .DATAFIELD_ADD("REFRESH", REFRESH_INT)
                                End With
                                If FPApp.ShowDialogForm(FixText_Edit) = Windows.Forms.DialogResult.OK Then
                                    Dim REFRESH_STR As String = ""

                                    With FixText_Edit
                                        FixText_Key = Trim(.DATAFIELD_GET("FIXTEXT_Key"))

                                        Select Case Val(.DATAFIELD_GET("REFRESH"))
                                            Case 0 : REFRESH_STR = ""
                                            Case 1 : REFRESH_STR = "FORM_CURRENT"
                                            Case 2 : REFRESH_STR = "FORM_AFTERUPDATE"
                                            Case Else : FixText = ""
                                        End Select

                                        FixText = "COUNTOFCOLUMNS  = " + .DATAFIELD_GET("COUNTOFCOLUMNS") + vbCrLf +
                                                  "SELECT          = " + .DATAFIELD_GET("SELECT") + vbCrLf +
                                                  "FROM            = " + .DATAFIELD_GET("FROM") + vbCrLf +
                                                  "WHERE           = " + .DATAFIELD_GET("WHERE") + vbCrLf +
                                                  "ORDERBY         = " + .DATAFIELD_GET("ORDERBY") + vbCrLf +
                                                  "VALUEMEMBER     = " + .DATAFIELD_GET("VALUEMEMBER") + vbCrLf +
                                                  "SEQ_KEY_HEADERS = " + .DATAFIELD_GET("SEQ_KEY_HEADERS") + vbCrLf +
                                                  "COLUMNWIDTHS    = " + .DATAFIELD_GET("COLUMNWIDTHS") + vbCrLf +
                                                  "ALIGNS          = " + .DATAFIELD_GET("ALIGNS") + vbCrLf +
                                                  "FORMATS         = " + .DATAFIELD_GET("FORMATS") + vbCrLf +
                                                  "CHECKBOXES      = " + .DATAFIELD_GET("CHECKBOXES") + vbCrLf +
                                                  "REFRESH         = " + REFRESH_STR
                                    End With
                                End If

                            Case Else
                                FixText_Edit = New FP_Simple_Edit(FPApp, "@@VB_SIMPLE_SELECT")

                                With FixText_Edit
                                    .DATAFIELD_ADD("FIXTEXT_Key", FixText_Key)
                                    FixText_Key = ""

                                    .DATAFIELD_ADD("QUERY_PREFIX", DIC_GET("QUERY_PREFIX", Params), , 1024)
                                    .DATAFIELD_ADD("FIELD_SELECTED_ID", DIC_GET("FIELD_SELECTED_ID", Params), , 1024)
                                    .DATAFIELD_ADD("FIELD_TEXT", DIC_GET("FIELD_TEXT", Params), , 1024)
                                    .DATAFIELD_ADD("FIELD_SELECTED_STR", DIC_GET("FIELD_SELECTED_STR", Params), , 1024)
                                    .DATAFIELD_ADD("FIELD_SELECTED_LONG", DIC_GET("FIELD_SELECTED_LONG", Params), , 1024)
                                    Select Case DIC_GET("WINDOWFORMAT", Params).ToUpper
                                        Case "", "FULLSCREEN" : .DATAFIELD_ADD("FIELD_WINDOWFORMAT", "0")
                                        Case "MANUAL" : .DATAFIELD_ADD("FIELD_WINDOWFORMAT", "1")
                                        Case "MULTITEXTBOX" : .DATAFIELD_ADD("FIELD_WINDOWFORMAT", "2")
                                        Case Else
                                            FPApp.DoErrorMsgBox("FP_Setup.Btn_DTFixTextKey_Click", 0, String.Format("Unknown parameter for 'WINDOWSFORMAT' ('{0}')", DIC_GET("WINDOWSFORMAT", Params).ToUpper))
                                    End Select
                                    Select Case DIC_GET("LIST_ACTIVATED_BY", Params).ToUpper
                                        Case "" : .DATAFIELD_ADD("FIELD_LIST_ACTIVATED_BY", "0")
                                        Case "IMMEDIATELY" : .DATAFIELD_ADD("FIELD_LIST_ACTIVATED_BY", "1")
                                        Case "KEYPRESS_THEN_*" : .DATAFIELD_ADD("FIELD_LIST_ACTIVATED_BY", "2")
                                        Case "*" : .DATAFIELD_ADD("FIELD_LIST_ACTIVATED_BY", "3")
                                        Case Else
                                            FPApp.DoErrorMsgBox("FP_Setup.Btn_DTFixTextKey_Click", 0, String.Format("Unknown parameter for 'LIST_ACTIVATED_BY' ('{0}')", DIC_GET("LIST_ACTIVATED_BY", Params).ToUpper))
                                    End Select
                                    .DATAFIELD_ADD("FIELD_MAXRECORDS", DIC_GET("MAXRECORDS", Params))
                                    .DATAFIELD_ADD("FIELD_NO_LIMIT_TO_LIST", DIC_GET("NO_LIMIT_TO_LIST", Params))
                                End With
                                If FPApp.ShowDialogForm(FixText_Edit) = Windows.Forms.DialogResult.OK Then
                                    With FixText_Edit
                                        FixText_Key = Trim(.DATAFIELD_GET("FIXTEXT_Key"))
                                        FixText = "QUERY_PREFIX        = " + .DATAFIELD_GET("QUERY_PREFIX") + vbCrLf +
                                                  "FIELD_SELECTED_ID   = " + .DATAFIELD_GET("FIELD_SELECTED_ID") + vbCrLf +
                                                  "FIELD_TEXT          = " + .DATAFIELD_GET("FIELD_TEXT") + vbCrLf +
                                                  "FIELD_SELECTED_STR  = " + .DATAFIELD_GET("FIELD_SELECTED_STR") + vbCrLf +
                                                  "FIELD_SELECTED_LONG = " + .DATAFIELD_GET("FIELD_SELECTED_LONG") + vbCrLf

                                        Select Case Val(.DATAFIELD_GET("FIELD_WINDOWFORMAT"))
                                            Case "0" 'Nothing to do
                                            Case "1" : FixText += "WINDOWFORMAT        = MANUAL" + vbCrLf
                                            Case "2" : FixText += "WINDOWFORMAT        = MULTITEXTBOX" + vbCrLf
                                        End Select

                                        Select Case Val(.DATAFIELD_GET("FIELD_LIST_ACTIVATED_BY"))
                                            Case "0" 'Nothing to do
                                            Case "1" : FixText += "LIST_ACTIVATED_BY   = IMMEDIATELY" + vbCrLf
                                            Case "2" : FixText += "LIST_ACTIVATED_BY   = KEYPRESS_THEN_*" + vbCrLf
                                            Case "3" : FixText += "LIST_ACTIVATED_BY   = *" + vbCrLf
                                        End Select

                                        If Val(.DATAFIELD_GET("FIELD_MAXRECORDS")) > 0 Then
                                            FixText += vbCrLf + "MAXRECORDS          = " + .DATAFIELD_GET("FIELD_MAXRECORDS") + vbCrLf
                                        End If

                                        If .DATAFIELD_GET("FIELD_NO_LIMIT_TO_LIST") = "1" Then
                                            FixText += vbCrLf + "NO_LIMIT_TO_LIST    = 1" + vbCrLf
                                        End If
                                    End With
                                End If
                        End Select
                    End If

                    If FixText_Key > "" Then
                        FPApp.FIXTEXT_SAVE_SIMPLE(FixText_Key, FixText)
                        FP_Lines.CONTROLS("DT_FixText_Key").DT_REFRESH()
                        FPf.FPApp.COMBOBOX_FIND(DT_FixText_Key, FixText_Key)
                        REFRESH_CONTROLS()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Tag_Params_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tag_Params.Click
        Dim Params_Edit As FP_Simple_Edit = Nothing
        Dim AktTag As String = P_Tag.Text
        Dim Params As String()
        Dim NewTag As String = ""

        Params = Split(AktTag, "|")
        ReDim Preserve Params(12)

        If Controlled_FPf.Frm.Name = "FP_DoFilter" Then
            Params_Edit = New FP_Simple_Edit(FPApp, "TAG_PARAMS_DOFILTER")

            With Params_Edit
                If Trim(Params(1)) = "" Then
                    Params(1) = "-1"
                End If
                .DATAFIELD_ADD("Param01", Params(0))
                .DATAFIELD_ADD("Param02", Params(1))
                .DATAFIELD_ADD("Param03", Params(2))

                If FPApp.ShowDialogForm(Params_Edit) = Windows.Forms.DialogResult.OK Then
                    Params(0) = .DATAFIELD_GET("Param01")
                    Params(1) = .DATAFIELD_GET("Param02")
                    Params(2) = .DATAFIELD_GET("Param03")
                    If Params(1) = "-1" Then
                        Params(1) = ""
                    End If

                    NewTag = Params(0)
                    If Params(1) > "" Or Params(2) > "" Then
                        NewTag += "|" + Params(1)
                    End If

                    If Params(2) > "" Then
                        NewTag += "|" + Params(2)
                    End If
                    P_Tag.Text = NewTag
                End If
            End With
        Else
            Params_Edit = New FP_Simple_Edit(FPApp, "TAG_PARAMS")

            With Params_Edit
                .DATAFIELD_ADD("Param01", Params(0))
                .DATAFIELD_ADD("Param02", Params(1))
                .DATAFIELD_ADD("Param03", Params(2))
                .DATAFIELD_ADD("Param04", Params(3))
                .DATAFIELD_ADD("Param05", Params(4))
                .DATAFIELD_ADD("Param06", Params(5))
                .DATAFIELD_ADD("Param07", Params(6))
                .DATAFIELD_ADD("Param08", Params(7))
                .DATAFIELD_ADD("Param09", Params(8))
                .DATAFIELD_ADD("Param10", Params(9))
                .DATAFIELD_ADD("Param11", Params(10))
                .DATAFIELD_ADD("Param12", Params(11))
            End With

            If FPApp.ShowDialogForm(Params_Edit) = Windows.Forms.DialogResult.OK Then
                With Params_Edit
                    NewTag = .DATAFIELD_GET("Param01") + "|" + _
                    .DATAFIELD_GET("Param02") + "|" + _
                    .DATAFIELD_GET("Param03") + "|" + _
                    .DATAFIELD_GET("Param04") + "|" + _
                    .DATAFIELD_GET("Param05") + "|" + _
                    .DATAFIELD_GET("Param06") + "|" + _
                    .DATAFIELD_GET("Param07") + "|" + _
                    .DATAFIELD_GET("Param08") + "|" + _
                    .DATAFIELD_GET("Param09") + "|" + _
                    .DATAFIELD_GET("Param10") + "|" + _
                    .DATAFIELD_GET("Param11") + "|" + _
                    .DATAFIELD_GET("Param12")
                End With

                P_Tag.Text = RemovePipesOnEnd(NewTag)
            End If
        End If
    End Sub

    Private Function RemovePipesOnEnd(ByVal MyStr As String) As String
        Dim OUT As String = MyStr

        While Microsoft.VisualBasic.Right(OUT, 1) = "|"
            OUT = Mid(OUT, 1, Len(OUT) - 1)
        End While

        RemovePipesOnEnd = OUT
    End Function

    Private Sub FP_Lines_Form_BeginEdit() Handles FP_Lines.Form_BeginEdit
        If FP_Lines.P_DATA_RecordStatus = ENUM_RecordStatus.NEWRECORD Then
            P_Visible.Checked = True
        End If
    End Sub

    Private Sub FP_Lines_Form_Field_AfterUpdate(ByVal FPc As FP_Control) Handles FP_Lines.Form_Field_AfterUpdate
        Dim From_TabIndex As Integer = FPc.c.TabIndex
        Dim D_ID As Long = DataField.SelectedValue
        Dim D_DT As DataTable = FP_Lines.CONTROLS("DataField").DT
        Dim D_Row As DataRow = Nothing
        Dim DName As String = ""
        Dim Curr_FieldPrefix = GET_CURRENT_Controlled_FP_FieldPrefix()

        If From_TabIndex <= RS_FIELDS_DEBUG_SeqNum.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex = DataField.TabIndex Then
            If D_ID <> 0 Then
                D_Row = D_DT.Select(String.Format("ID={0}", D_ID.ToString)).First
                DName = D_Row!FieldName

                FieldName.Text = DName
                xType_VB.Text = D_Row!xtype_VB
            End If
        End If

        If From_TabIndex <= FieldName.TabIndex Then
            If FieldName.Text > "" Then
                Dim IsPictureBox As Boolean = False

                If Controlled_FPf.CONTROLS.ContainsKey(FieldName.Text) Then
                    If TypeOf (Controlled_FPf.CONTROLS(FieldName.Text)) Is PictureBox Then
                        IsPictureBox = True
                    End If
                End If

                If FP_Lines.P_DATA_RecordStatus = ENUM_RecordStatus.NEWRECORD Then
                    If FieldName.Text <> "#FORM#" Then
                        CreateAtRuntime.Checked = (Not Controlled_FPf.CONTROLS.ContainsKey(Curr_FieldPrefix + FieldName.Text))
                    End If
                End If

                If IsPictureBox Then
                    LabelName.Text = ""
                    Label_Text.Text = ""
                ElseIf FieldName.Text = "#FORM#" Then
                    LabelName.Text = ""
                    Label_Text.Text = ""
                Else
                    If FP_Lines.CONTROLS("FieldName").OldValue <> nz(FieldName.Text, "") Then
                        LabelName.Text = FieldName.Text + "_Label"
                        Label_Text.Text = FieldName.Text + ":"
                    End If
                End If
            End If
        End If

        If From_TabIndex <= CreateAtRuntime.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= xType_VB.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= ShowInGRID.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= P_TabIndex.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= P_TabStop.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= P_Visible.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= Mandatory.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= P_Locked.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= F_Format_TRIM.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= F_Format_UCASE.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= F_Format_NOSPACE.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= F_Format_MinusAllowed.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= BG_Toggle.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= P_Parent_STR.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= CreateAtRuntime_FieldType.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= Colors_ForeColor.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= Colors_BackColor.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= Colors_SELECTED_FORE.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= SavePoint.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= Forced_NextField_STR.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= F_Format_Format.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= BG_Image_STR.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= P_Tag.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= DT_FixText_Key.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= DT_WHERE2.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= LabelName.TabIndex Then
            If From_TabIndex = LabelName.TabIndex Then
                If Trim(FPc_LabelName.P_VALUE) = "" Then
                    FPc_Label_Text.P_VALUE = ""
                End If
            End If
        End If

        If From_TabIndex <= Colors_Label_ForeColor.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= Colors_Label_BackColor_Label.TabIndex Then
            'Nothing to do
        End If

        If From_TabIndex <= Label_Text.TabIndex Then
            'Nothing to do
        End If

        FP_Lines_SET_LAYOUT()
    End Sub

    Sub Unbound_ControlCombo_SETVALUE(ByVal FPcombo As FP_Control, ByVal FieldName As String)
        If FPcombo.FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            FPApp.COMBOBOX_FIND(FPcombo.c, "")
        Else
            FPApp.COMBOBOX_FIND(FPcombo.c, FPcombo.FP.DATA_Field_getSavedValue(FieldName))
        End If
    End Sub

    Sub Unbound_Combos_FP_ARRANGE_SET_ALL_VALUES()
        FP_ARRANGE.P_DATA_Binded_ByUser = False

        Unbound_ControlCombo_SETVALUE(FPc_ControlName_STR, "ControlName")
        Unbound_ControlCombo_SETVALUE(FPc_P1_Control, "P1_Control")
        Unbound_ControlCombo_SETVALUE(FPc_P2_Control, "P2_Control")

        FP_ARRANGE.P_DATA_Binded_ByUser = True
    End Sub

    Sub Unbound_Combos_FP_Lines_SET_ALL_VALUES()
        FP_Lines.P_DATA_Binded_ByUser = False

        Unbound_ControlCombo_SETVALUE(FPc_P_Parent_STR, "P_Parent")
        Unbound_ControlCombo_SETVALUE(FPc_Forced_NextField_STR, "Forced_NextField")

        Unbound_ResCombo_SETVALUE(FPc_BG_Image_STR, "BG_Image")

        FP_Lines.P_DATA_Binded_ByUser = True
    End Sub

    Sub Unbound_ResCombo_SETVALUE(ByVal FPcombo As FP_Control, ByVal FieldName As String)
        If FPcombo.FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            FPApp.COMBOBOX_FIND(FPcombo.c, "")
        Else
            FPApp.COMBOBOX_FIND(FPcombo.c, FPcombo.FP.DATA_Field_getSavedValue(FieldName))
        End If
    End Sub

    Private Sub FP_Head_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP_Head.CONTROLS_INITIALIZED
        FPc_Btn_REFRESH = FPf.PICTUREBOXES("Btn_REFRESH")
    End Sub

    Private Sub FP_ARRANGE_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP_ARRANGE.CONTROLS_INITIALIZED
        FPc_ControlName_STR = FP_ARRANGE.CONTROLS("ControlName_STR")
        FPc_P1_Control = FP_ARRANGE.CONTROLS("P1_Control_STR")
        FPc_P2_Control = FP_ARRANGE.CONTROLS("P2_Control_STR")
        FPc_ArrangeType = FP_ARRANGE.CONTROLS("ArrangeType")

        Sign_CREATE_ALL()
    End Sub

    Private Sub FP_Lines_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP_Lines.CONTROLS_INITIALIZED
        FPc_DataField = FP_Lines.CONTROLS("DataField")
        FPc_FieldName = FP_Lines.CONTROLS("FieldName")

        FPc_Colors_ForeColor = FP_Lines.CONTROLS("Colors_ForeColor")
        FPc_Colors_BackColor = FP_Lines.CONTROLS("Colors_BackColor")
        FPc_Colors_SELECTED_FORE = FP_Lines.CONTROLS("Colors_SELECTED_FORE")
        FPc_P_Parent_STR = FP_Lines.CONTROLS("P_Parent_STR")
        FPc_Forced_NextField_STR = FP_Lines.CONTROLS("Forced_NextField_STR")
        FPc_BG_Image_STR = FP_Lines.CONTROLS("BG_Image_STR")
        FPc_DT_ID_Field = FP_Lines.CONTROLS("DT_ID_Field")
        FPc_Colors_Label_ForeColor = FP_Lines.CONTROLS("Colors_Label_ForeColor")
        FPc_Colors_Label_BackColor = FP_Lines.CONTROLS("Colors_Label_BackColor")
        FPc_LabelName = FP_Lines.CONTROLS("LabelName")
        FPc_Label_Text = FP_Lines.CONTROLS("Label_Text")

        Sign_CREATE_ALL()
    End Sub

    Private Function FieldName_Check(ByVal MyFieldName As String, ByVal MySeqNum As Long) As Boolean
        Dim OUT As Boolean = True

        If FP_Lines.GRID.DT.Select(String.Format("FieldName = '{0}' And SeqNum <> {1}", MyFieldName, MySeqNum)).Count > 0 Then
            OUT = False
            FPApp.DoMyMsgBox(59) 'Ezt a mezot mar felvette
        End If

        FieldName_Check = OUT
    End Function

    Private Sub FP_ARRANGE_Form_AfterUpdate() Handles FP_ARRANGE.Form_AfterUpdate
        FPf.FOCUS_ON_AT_THE_END(FP_ARRANGE.CONTROLS("ControlName_STR").c, , , True)
        REFRESH_CONTROLS()
    End Sub

    Private Sub FP_ARRANGE_Form_BeforeUpdate(ByVal sender_FP As FP, ByRef Cancel As Integer) Handles FP_ARRANGE.Form_BeforeUpdate
        Dim Curr_FP_FieldPrefix As String = GET_CURRENT_Controlled_FP_FieldPrefix()

        With FP_ARRANGE
            .DATA_Field_setValue("ServerObject_Prefix", GET_CURRENT_ServerObject_Prefix())
            .DATA_Field_setValue("SubPrefix", GET_CURRENT_SubPrefix())

            .DATA_Field_setValue("ControlName", ControlName_STR.Text)

            .DATA_Field_setValue("P1_Control", P1_Control_STR.Text)
            .DATA_Field_setValue("P2_Control", P2_Control_STR.Text)
        End With
    End Sub

    Private Sub FP_ARRANGE_Form_Current() Handles FP_ARRANGE.Form_Current
        Unbound_Combos_FP_ARRANGE_SET_ALL_VALUES()
        FP_ARRANGE_SET_LAYOUT()
    End Sub

    Private Sub FP_ARRANGE_Form_NoRecord() Handles FP_ARRANGE.Form_NoRecord
        Unbound_Combos_FP_ARRANGE_SET_ALL_VALUES()
        FP_ARRANGE_SET_LAYOUT()
    End Sub

    Private Sub Unbound_Controls_Field_TextChanged(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByVal e As System.EventArgs, ByRef Cancel As Boolean) Handles FPc_ControlName_STR.Field_TextChanged, FPc_P1_Control.Field_TextChanged, FPc_P2_Control.Field_TextChanged, FPc_P_Parent_STR.Field_TextChanged, FPc_Forced_NextField_STR.Field_TextChanged, FPc_BG_Image_STR.Field_TextChanged
        If sender_FPc.FP.P_DATA_Binded Then
            If sender_FPc.FIELD_IsDirty Then
                sender_FPc.FP.FORM_DIRTY_SET()
                Sign_SET_ALL()
            End If
        End If
    End Sub

    Private Sub RECORD_UPDOWN_MouseUp(ByVal MyFP As FP, ByVal UpDown As Enum_UpDown)
        With MyFP
            Dim Current_ID = .P_DATA_Current_ID

            If Current_ID <> 0 Then
                If RECORD_UPDOWN(.RS_ID, MyFP.FORMDEF("UNIQUETABLE"), Current_ID, MyFP.FORMDEF("UNIQUETABLE_SEQNUM"), UpDown) Then
                    .FORM_RECORDS_LOAD(.FORM_SubWHERE)
                    .FORM_GOTO_RECORD_BY_ID(Current_ID)
                    REFRESH_CONTROLS()
                    If MyFP.Equals(FP_Lines) Then
                        Unbound_Combos_FP_Lines_SET_ALL_VALUES()
                    ElseIf MyFP.Equals(FP_ARRANGE) Then
                        Unbound_Combos_FP_ARRANGE_SET_ALL_VALUES()
                    Else
                        'Nothing to do
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub RECORD_UPDOWN_GRID_Row_MouseWheel(ByVal MyFP As FP, ByVal sender As Object, ByRef e As System.Windows.Forms.MouseEventArgs, ByRef Handled As Boolean)
        Dim CtrlPressed As Boolean = My.Computer.Keyboard.CtrlKeyDown

        If CtrlPressed Then
            With MyFP
                If .FORM_RECORDS_SAVE_CURRENT Then
                    Dim i As Integer
                    Dim WheelCount = e.Delta / 120
                    Dim Current_ID = .P_DATA_Current_ID

                    If Current_ID <> 0 Then
                        If WheelCount > 0 Then
                            For i = 1 To WheelCount
                                If Not RECORD_UPDOWN(.RS_ID, MyFP.FORMDEF("UNIQUETABLE"), Current_ID, MyFP.FORMDEF("UNIQUETABLE_SEQNUM"), Enum_UpDown.UP) Then
                                    Exit For
                                End If
                            Next
                        Else
                            For i = -1 To WheelCount Step -1
                                If Not RECORD_UPDOWN(.RS_ID, MyFP.FORMDEF("UNIQUETABLE"), Current_ID, MyFP.FORMDEF("UNIQUETABLE_SEQNUM"), Enum_UpDown.DOWN) Then
                                    Exit For
                                End If
                            Next
                        End If
                        .FORM_RECORDS_LOAD(.FORM_SubWHERE)
                        .FORM_GOTO_RECORD_BY_ID(Current_ID)
                        REFRESH_CONTROLS()
                        Handled = True
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub Btn_A_Up_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Btn_A_Up.MouseUp
        RECORD_UPDOWN_MouseUp(FP_ARRANGE, Enum_UpDown.UP)
    End Sub

    Private Sub Btn_A_Down_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Btn_A_Down.MouseUp
        RECORD_UPDOWN_MouseUp(FP_ARRANGE, Enum_UpDown.DOWN)
    End Sub

    Private Sub FP_ARRANGE_GRID_Row_MouseWheel(ByVal sender_FP As FP, ByVal sender As Object, ByRef e As System.Windows.Forms.MouseEventArgs, ByRef Handled As Boolean) Handles FP_ARRANGE.GRID_Row_MouseWheel, FP_Lines.GRID_Row_MouseWheel
        RECORD_UPDOWN_GRID_Row_MouseWheel(sender_FP, sender, e, Handled)
    End Sub

    Private Sub FP_KeyPreview_KeyPress(ByVal sender_FP As FP, ByRef sender As Object, ByRef e As System.Windows.Forms.KeyEventArgs) Handles FP_ARRANGE.Form_KeyPreview_KeyDown, FP_Lines.Form_KeyPreview_KeyDown
        Dim CtrlPressed As Boolean = My.Computer.Keyboard.CtrlKeyDown

        If CtrlPressed Then
            If sender_FP.CONTROLS(CType(sender, Control).Name).P.ShowInGRID Then
                Select Case e.KeyCode
                    Case Keys.Up
                        RECORD_UPDOWN_MouseUp(sender_FP, Enum_UpDown.UP)
                        e.Handled = True

                    Case Keys.Down
                        RECORD_UPDOWN_MouseUp(sender_FP, Enum_UpDown.DOWN)
                        e.Handled = True
                End Select
            End If
        End If
    End Sub

    Private Sub Btn_Down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Down.Click
        RECORD_UPDOWN_MouseUp(FP_Lines, Enum_UpDown.DOWN)
    End Sub

    Private Sub Btn_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Up.Click
        RECORD_UPDOWN_MouseUp(FP_Lines, Enum_UpDown.UP)
    End Sub

    Private Sub Btn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_OK.Click
        If FPf.SAVE_ALL Then
            Me.Close()
        End If
    End Sub

    Private Sub FP_ARRANGE_Form_Field_Enter(ByVal FPc As FP_Control, ByRef Handled As Boolean) Handles FP_ARRANGE.Form_Field_Enter
        Sign_SET_ALL()
    End Sub


    Private Sub FP_Head_Form_Field_Enter(ByVal FPc As FP_Control, ByRef Handled As Boolean) Handles FP_Head.Form_Field_Enter
        Sign_SET_ALL()
    End Sub

    Private Sub FP_Lines_Form_Field_Enter(ByVal FPc As FP_Control, ByRef Handled As Boolean) Handles FP_Lines.Form_Field_Enter
        Sign_SET_ALL()
    End Sub

    Private Sub FP_Btn_REFRESH_CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FPc_Btn_REFRESH.CLICK
        REFRESH_CONTROLS()
    End Sub

    Private Sub Btn_BackColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_BackColor.Click
        COLOR_BTN_PRESSED(Colors_BackColor, COLORS_FIELD_NORMAL_BG)
    End Sub

    Private Sub COLOR_BTN_PRESSED(ByVal Textbox_for_ColorDef As TextBox, ByVal Color_Default As Color)
        Dim CurrentColor As Color = Nothing

        If COLOR_GET_FROM_STR(Textbox_for_ColorDef.Text, Color_Default, Textbox_for_ColorDef.Name, CurrentColor) Then
            Dim Frm_Colors As New FP_Services_Colors(FPApp)
            Dim Frm_Colors_Result As System.Windows.Forms.DialogResult = Windows.Forms.DialogResult.Cancel

            'Me.WindowState = FormWindowState.Minimized
            Frm_Colors_Result = Frm_Colors.ShowDialog
            'Me.WindowState = FormWindowState.Normal

            If Frm_Colors_Result = Windows.Forms.DialogResult.OK Then
                Textbox_for_ColorDef.Text = COLOR_GET_STR_FROM_COLOR(Frm_Colors.OUT_Selected_Color)
                FP_Lines_SET_LAYOUT()
            End If
        End If

        'OLD VERSION:
        'Dim CurrentColor As Color = Nothing

        'If COLOR_GET_FROM_STR(Textbox_for_ColorDef.Text, Color_Default, Textbox_for_ColorDef.Name, CurrentColor) Then
        '    With Dlg_EditColor
        '        .Color = CurrentColor

        '        Dim F As Form = FPApp.ShowDialogForm_getOpacityForm(FPf.Frm)
        '        If .ShowDialog = Windows.Forms.DialogResult.OK Then
        '            Textbox_for_ColorDef.Text = COLOR_GET_STR_FROM_COLOR(.Color)
        '            FP_Lines_SET_LAYOUT()
        '        End If
        '        If Not (F Is Nothing) Then
        '            F.Close()
        '        End If
        '    End With
        'End If
    End Sub

    Sub FP_Lines_SET_LAYOUT()
        Dim wColor As Color

        If COLOR_GET_FROM_STR(Colors_ForeColor.Text, COLORS_FIELD_NORMAL_FORE, Colors_ForeColor.Name, wColor) Then
            Btn_ForeColor.BackColor = wColor
        End If

        If COLOR_GET_FROM_STR(Colors_BackColor.Text, COLORS_FIELD_NORMAL_BG, Colors_BackColor.Name, wColor) Then
            Btn_BackColor.BackColor = wColor
        End If

        If COLOR_GET_FROM_STR(Colors_SELECTED_FORE.Text, COLORS_FIELD_SELECTED_FORE, Colors_SELECTED_FORE.Name, wColor) Then
            Btn_Colors_SELECTED_FORE.BackColor = wColor
        End If

        If COLOR_GET_FROM_STR(Colors_Label_ForeColor.Text, COLORS_LABEL_FORE, Colors_Label_ForeColor.Name, wColor) Then
            Btn_Label_ForeColor.BackColor = wColor
        End If

        If COLOR_GET_FROM_STR(Colors_Label_BackColor.Text, COLORS_LABEL_BG, Colors_Label_BackColor.Name, wColor) Then
            Btn_Label_BackColor.BackColor = wColor
        End If

        Sign_SET_ALL()
    End Sub

    Sub FP_ARRANGE_SET_NEXTFIELD()
        Dim NextField As Control = Nothing

        If FP_ARRANGE.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            'Nothing to do
        Else
            Select Case ArrangeType.Text
                Case "", "LOCATION_FIX", "SIZE_FIX"
                    NextField = P1_Koo

                Case "ARRANGE_ON_LEFT", "ARRANGE_ON_RIGHT", "ARRANGE_ON_TOP", "ARRANGE_ON_BOTTOM", _
                     "ARRANGE_ON_LEFT_TOP", "ARRANGE_ON_RIGHT_TOP", "ARRANGE_ON_TOP_LEFT", "ARRANGE_ON_BOTTOM_LEFT", _
                     "ARRANGE_ON_LEFT_BOTTOM", "ARRANGE_ON_RIGHT_BOTTOM", "ARRANGE_ON_TOP_RIGHT", "ARRANGE_ON_BOTTOM_RIGHT", _
                     "ARRANGE_TOPS", "ARRANGE_BOTTOMS", "ARRANGE_LEFTS", "ARRANGE_RIGHTS", _
                     "SIZE_WIDTH_TO", "SIZE_HEIGHT_TO", "SIZE_WIDTH_BETWEEN", "SIZE_HEIGHT_BETWEEN", _
                      "AS_NEXT_ROW_TO", "AS_PREV_ROW_TO", "ARRANGE_FROM_TO", "SIZE_SAME", "SIZE_HEIGHT_TO_WIDTH", "SIZE_WIDTH_TO_HEIGHT", _
                      "ARRANGE_ON_CENTER_X", "ARRANGE_ON_CENTER_Y"
                    NextField = P1_Control_STR

                Case "SIZE_HEIGHT_TO_MAX", "SIZE_WIDTH_TO_MAX"
                    NextField = P1_SpaceInPixel

                Case Else
                    FPApp.DoErrorMsgBox("FP_Setup.FP_ARRANGE_SET_NEXTFIELD", 0, String.Format("Unknown Arrange Type '{0}'", ArrangeType.Text))
            End Select
        End If

        If Not (NextField Is Nothing) Then
            FPf.FOCUS_ON_AT_THE_END(NextField)
        End If
    End Sub

    Sub FP_ARRANGE_SET_LAYOUT()
        Dim A_c1_M As Boolean = False
        Dim A_c2_M As Boolean = False
        Dim A_c1_Text As String = "Width:"
        Dim A_c2_Text As String = "Height:"
        Dim TabStop_P1_Koo As Boolean = False
        Dim TabStop_P2_Koo As Boolean = False
        Dim TabStop_P1_Control_STR As Boolean = False
        Dim TabStop_P2_Control_STR As Boolean = False
        Dim TabStop_P1_SpaceInPixel As Boolean = False
        Dim TabStop_P2_SpaceInPixel As Boolean = False
        Dim TabStop_Percentage As Boolean = False

        If FP_ARRANGE.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            'Nothing to do
        Else
            Select Case ArrangeType.Text
                Case "", "LOCATION_FIX"
                    A_c1_Text = "Left:"
                    A_c2_Text = "Top:"
                    TabStop_P1_Koo = True
                    TabStop_P2_Koo = True

                Case "SIZE_FIX"
                    TabStop_P1_Koo = True
                    TabStop_P2_Koo = True

                Case "SIZE_SAME"
                    TabStop_P1_Control_STR = True
                    A_c1_M = True

                Case "AS_NEXT_ROW_TO", "AS_PREV_ROW_TO"
                    TabStop_P1_Control_STR = True
                    TabStop_P1_SpaceInPixel = True
                    TabStop_P2_Control_STR = True
                    TabStop_Percentage = True
                    A_c1_M = True

                Case "ARRANGE_FROM_TO"
                    TabStop_P1_Control_STR = True
                    TabStop_P1_SpaceInPixel = True
                    TabStop_P2_Control_STR = True
                    A_c1_M = True
                    A_c2_M = True

                Case "ARRANGE_ON_LEFT", "ARRANGE_ON_RIGHT", "ARRANGE_ON_TOP", "ARRANGE_ON_BOTTOM", _
                     "ARRANGE_ON_LEFT_TOP", "ARRANGE_ON_RIGHT_TOP", "ARRANGE_ON_TOP_LEFT", "ARRANGE_ON_BOTTOM_LEFT", _
                     "ARRANGE_ON_LEFT_BOTTOM", "ARRANGE_ON_RIGHT_BOTTOM", "ARRANGE_ON_TOP_RIGHT", "ARRANGE_ON_BOTTOM_RIGHT"
                    A_c1_M = True
                    TabStop_P1_Control_STR = True
                    TabStop_P1_SpaceInPixel = True
                    TabStop_Percentage = True

                Case "ARRANGE_TOPS", "ARRANGE_BOTTOMS", "ARRANGE_LEFTS", "ARRANGE_RIGHTS", "SIZE_HEIGHT_TO_WIDTH", "SIZE_WIDTH_TO_HEIGHT"
                    A_c1_M = True
                    TabStop_P1_Control_STR = True
                    TabStop_P1_SpaceInPixel = True
                    TabStop_Percentage = True

                Case "SIZE_WIDTH_TO", "SIZE_HEIGHT_TO"
                    A_c1_M = True
                    TabStop_P1_Control_STR = True
                    TabStop_P2_Control_STR = True
                    TabStop_P1_SpaceInPixel = True
                    TabStop_P2_SpaceInPixel = True
                    TabStop_Percentage = True

                Case "ARRANGE_ON_CENTER_X", "ARRANGE_ON_CENTER_Y"
                    A_c1_M = True
                    TabStop_P1_Control_STR = True
                    TabStop_P1_SpaceInPixel = True

                Case "SIZE_WIDTH_BETWEEN", "SIZE_HEIGHT_BETWEEN"
                    A_c1_M = True
                    A_c2_M = True
                    TabStop_P1_Control_STR = True
                    TabStop_P2_Control_STR = True
                    TabStop_P1_SpaceInPixel = True
                    TabStop_P2_SpaceInPixel = True
                    TabStop_Percentage = True

                Case "SIZE_HEIGHT_TO_MAX", "SIZE_WIDTH_TO_MAX"
                    TabStop_P1_SpaceInPixel = True
                    TabStop_P2_SpaceInPixel = True

                Case Else
                    FPApp.DoErrorMsgBox("FP_Setup.FP_ARRANGE_SET_LAYOUT", 0, String.Format("Unknown Arrange Type '{0}'", ArrangeType.Text))
            End Select
        End If

        P1_Koo_Label.Text = A_c1_Text
        P2_Koo_Label.Text = A_c2_Text
        FP_ARRANGE.CONTROLS("P1_Control_STR").P.Mandatory = A_c1_M
        FP_ARRANGE.CONTROLS("P2_Control_STR").P.Mandatory = A_c2_M

        P1_Koo.TabStop = TabStop_P1_Koo
        P2_Koo.TabStop = TabStop_P2_Koo
        FP_ARRANGE.CONTROLS("P1_Control_STR").P_TABSTOP = TabStop_P1_Control_STR
        FP_ARRANGE.CONTROLS("P2_Control_STR").P_TABSTOP = TabStop_P2_Control_STR
        P1_SpaceInPixel.TabStop = TabStop_P1_SpaceInPixel
        P2_SpaceInPixel.TabStop = TabStop_P2_SpaceInPixel
        Percentage.TabStop = TabStop_Percentage

        FP_ARRANGE.COLORING_ALL()

        Sign_SET_ALL()
    End Sub

    Private Function FieldName_Get_phisicalName() As String
        Dim OUT As String = ""

        If FP_Lines.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            OUT = GET_CURRENT_FieldPrefix() + nz(FieldName.Text, "")
        End If

        Return OUT
    End Function

    Private Function CurrentField_IsLabel() As Boolean
        Dim OUT As Boolean = False

        If FP_Lines.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            FPApp.DoErrorMsgBox("FP_Setup.CurrentField_IsLabel", 0, "FP_Lines.P_DATA_RecordStatus = NORECORD")
        Else
            If FieldName.Text > "" Then
                If CreateAtRuntime.Checked Then
                    If CreateAtRuntime_FieldType.Text = "Label" Then
                        OUT = True
                    End If
                Else
                    If Controlled_FPf.CONTROLS.ContainsKey(FieldName_Get_phisicalName) Then
                        If TypeOf Controlled_FPf.CONTROLS(FieldName_Get_phisicalName) Is Label Then
                            OUT = True
                        End If
                    End If
                End If
            End If
        End If

        Return OUT
    End Function

    Private Function CurrentField_IsTabPage() As Boolean
        Dim OUT As Boolean = False

        If FP_Lines.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            FPApp.DoErrorMsgBox("FP_Setup.CurrentField_IsTabPage", 0, "FP_Lines.P_DATA_RecordStatus = NORECORD")
        Else
            If FieldName.Text > "" Then
                If CreateAtRuntime.Checked Then
                    If CreateAtRuntime_FieldType.Text = "TabPage" Then
                        OUT = True
                    End If
                Else
                    If Controlled_FPf.CONTROLS.ContainsKey(FieldName.Text) Then
                        If TypeOf Controlled_FPf.CONTROLS(FieldName.Text) Is TabPage Then
                            OUT = True
                        End If
                    End If
                End If
            End If
        End If

        Return OUT
    End Function

    Private Function CurrentField_IsButton() As Boolean
        Dim OUT As Boolean = False

        If FP_Lines.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            FPApp.DoErrorMsgBox("FP_Setup.CurrentField_IsButton", 0, "FP_Lines.P_DATA_RecordStatus = NORECORD")
        Else
            If FieldName.Text > "" Then
                If CreateAtRuntime.Checked Then
                    If CreateAtRuntime_FieldType.Text = "Button" Then
                        OUT = True
                    End If
                Else
                    If Controlled_FPf.CONTROLS.ContainsKey(FieldName.Text) Then
                        If TypeOf Controlled_FPf.CONTROLS(FieldName.Text) Is Button Then
                            OUT = True
                        End If
                    End If
                End If
            End If
        End If

        Return OUT
    End Function

    Private Sub FP_TEXTS_SET_RECORDSOURCE()
        Dim Set_Empty_Recordset As Boolean = True
        Dim SubWhere As String = ""

        If FP_Lines.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
            If Trim(FieldName.Text) > "" Then
                If Trim(LabelName.Text) > "" Or (FieldName.Text = "#FORM#") Or CurrentField_IsLabel() Or CurrentField_IsTabPage() Or CurrentField_IsButton() Then
                    Dim Current_ServerObject_Prefix As String = ""
                    Dim Current_SubPrefix As String = ""

                    Select Case GET_CURRENT_Level()
                        Case Enum_Level.FP_FORM
                            Current_ServerObject_Prefix = Controlled_FPf.ServerObject_Prefix
                            Current_SubPrefix = ""

                        Case Enum_Level.FP
                            Current_ServerObject_Prefix = GET_CURRENT_Controlled_FP.ServerObject_Prefix
                            Current_SubPrefix = GET_CURRENT_SubPrefix()
                    End Select

                    If FieldName.Text = "#FORM#" Or CurrentField_IsLabel() Or CurrentField_IsTabPage() Or CurrentField_IsButton() Then
                        SubWhere = String.Format("T_ServerObject_Prefix = '{0}' And SubPrefix = '{1}' And T_LabelName = '{2}'", Current_ServerObject_Prefix, Current_SubPrefix, FieldName.Text)
                        Set_Empty_Recordset = False
                    Else
                        If Trim(LabelName.Text) > "" Then
                            SubWhere = String.Format("T_ServerObject_Prefix = '{0}' And SubPrefix = '{1}' And T_LabelName = '{2}'", Current_ServerObject_Prefix, Current_SubPrefix, LabelName.Text)
                            Set_Empty_Recordset = False
                        End If
                    End If
                End If
            End If
        End If

        If Set_Empty_Recordset Then
            FP_TEXTS.P_FORM_AllowAdditions = False

            SubWhere = "T_ServerObject_Prefix = '' And T_LabelName = ''"
            FP_TEXTS.FORM_RECORDS_LOAD(SubWhere, , True)
        Else
            FP_TEXTS.P_FORM_AllowAdditions = True

            If Not FP_TEXTS.FORM_RECORDS_LOAD(SubWhere) Then
                FP_TEXTS.FORM_RECORDS_LOAD(SubWhere, True)
            End If
        End If
    End Sub

    Private Sub FP_HELP_SET_RECORDSOURCE()
        Dim Set_Empty_Recordset As Boolean = True
        Dim SubWhere As String = ""

        If FP_Lines.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
            If Trim(FieldName.Text) > "" Then
                Dim Current_ServerObject_Prefix As String = ""
                Dim Current_SubPrefix As String = ""

                Select Case GET_CURRENT_Level()
                    Case Enum_Level.FP_FORM
                        Current_ServerObject_Prefix = Controlled_FPf.ServerObject_Prefix
                        Current_SubPrefix = ""

                    Case Enum_Level.FP
                        Current_ServerObject_Prefix = GET_CURRENT_Controlled_FP.ServerObject_Prefix
                        Current_SubPrefix = GET_CURRENT_SubPrefix()
                End Select

                SubWhere = String.Format("H_ServerObject_Prefix = '{0}' And SubPrefix = '{1}' And H_FieldName = '{2}'", Current_ServerObject_Prefix, Current_SubPrefix, FieldName.Text)

                Set_Empty_Recordset = False
            End If
        End If

        If Set_Empty_Recordset Then
            FP_HELP.P_FORM_AllowAdditions = False

            SubWhere = "H_ServerObject_Prefix = '' And H_FieldName = ''"
            FP_HELP.FORM_RECORDS_LOAD(SubWhere, , True)
        Else
            If FieldName.Text = "#FORM#" Then
                FP_HELP.P_FORM_AllowAdditions = False
            Else
                FP_HELP.P_FORM_AllowAdditions = True
            End If

            If Not FP_HELP.FORM_RECORDS_LOAD(SubWhere) Then
                FP_HELP.FORM_RECORDS_LOAD(SubWhere, True)
            End If
        End If
    End Sub

    Private Sub FP_Lines_Form_Current(ByVal sender_FP As FP) Handles FP_Lines.Form_Current
        Unbound_Combos_FP_Lines_SET_ALL_VALUES()
        FP_HELP_SET_RECORDSOURCE()
        FP_TEXTS_SET_RECORDSOURCE()
        FP_Lines_SET_LAYOUT()
    End Sub

    Private Sub Btn_ForeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_ForeColor.Click
        COLOR_BTN_PRESSED(Colors_ForeColor, COLORS_FIELD_NORMAL_FORE)
    End Sub

    Private Sub Btn_Colors_SELECTED_FORE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Colors_SELECTED_FORE.Click
        COLOR_BTN_PRESSED(Colors_SELECTED_FORE, COLORS_FIELD_SELECTED_FORE)
    End Sub

    Private Sub Btn_Label_ForeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Label_ForeColor.Click
        COLOR_BTN_PRESSED(Colors_Label_ForeColor, COLORS_LABEL_FORE)
    End Sub

    Private Sub Btn_Label_BackColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Label_BackColor.Click
        COLOR_BTN_PRESSED(Colors_Label_BackColor, COLORS_LABEL_BG)
    End Sub

    Private Sub COLORFields_BeforeUpdate(ByVal sender_FPc As FP_Control, ByRef Cancel As Integer) Handles FPc_Colors_BackColor.Field_BeforeUpdate, FPc_Colors_ForeColor.Field_BeforeUpdate, FPc_Colors_SELECTED_FORE.Field_BeforeUpdate, FPc_Colors_Label_BackColor.Field_BeforeUpdate, FPc_Colors_Label_ForeColor.Field_BeforeUpdate
        Dim OUT_Color As Color

        Cancel = Not COLOR_GET_FROM_STR(sender_FPc.c.Text, Nothing, sender_FPc.c.Name, OUT_Color)
    End Sub

    Private Sub FP_Lines_Form_NoRecord(ByVal sender_FP As FP) Handles FP_Lines.Form_NoRecord
        Unbound_Combos_FP_Lines_SET_ALL_VALUES()
        FP_HELP_SET_RECORDSOURCE()
        FP_TEXTS_SET_RECORDSOURCE()
        FP_Lines_SET_LAYOUT()
    End Sub

    Private Sub FP_ARRANGE_Form_AfterDelete(ByVal sender_FP As FP) Handles FP_ARRANGE.Form_AfterDelete
        REFRESH_CONTROLS()
    End Sub

    Private Sub FP_HELP_Show_Current()
        Dim ForControl_Name As String = ""

        If TabControl.SelectedTab.Equals(Page_Language_Settings) Then
            If FP_Lines.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                If FieldName.Text > "" Then
                    If Controlled_FPf.CONTROLS.ContainsKey(FieldName.Text) Then
                        If FP_HELP.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
                            If FPc_H_ShortText.c.Text > "" Or FPf.CONTROLS("H_Link").Text > "" Then
                                ForControl_Name = FieldName.Text

                                If Not (Controlled_FPf.HELP_Frm Is Nothing) Then
                                    If Controlled_FPf.HELP_Frm.DIC_HELP.ContainsKey(ForControl_Name) Then
                                        Dim Hlp_Text As New Struct_HELPText

                                        With Hlp_Text
                                            .ShortText = FPc_H_ShortText.c.Text
                                            .Link = FPf.CONTROLS("H_Link").Text
                                        End With

                                        Controlled_FPf.HELP_Frm.DIC_HELP(ForControl_Name) = Hlp_Text
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        If ForControl_Name = "" Then
            Controlled_FPf.HELP_HIDE()
        Else
            Controlled_FPf.HELP_SHOW(Controlled_FPf.CONTROLS(ForControl_Name))
        End If
    End Sub

    Private Sub FP_TEXTS_Show_Current()
        Dim ForLabel_Name As String = ""

        If TabControl.SelectedTab.Equals(Page_Language_Settings) Then
            If FP_Lines.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                If FieldName.Text = "#FORM#" Then
                    ForLabel_Name = "#FORM#"
                ElseIf CurrentField_IsLabel() Or CurrentField_IsTabPage() Or CurrentField_IsButton() Then
                    ForLabel_Name = FieldName.Text
                Else
                    ForLabel_Name = LabelName.Text
                End If
                If ForLabel_Name > "" Then
                    If ForLabel_Name = "#FORM#" Then
                        Controlled_FPf.Frm.Text = nz(FPc_T_ShortText.c.Text, "")
                    Else
                        If Controlled_FPf.CONTROLS.ContainsKey(ForLabel_Name) Then
                            If FP_TEXTS.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
                                If FPc_T_ShortText.c.Text > "" Then
                                    Try
                                        Controlled_FPf.CONTROLS(ForLabel_Name).Text = FPc_T_ShortText.c.Text
                                    Catch ex As Exception
                                        'Nothing to do
                                    End Try
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub FP_HELP_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP_HELP.CONTROLS_INITIALIZED
        FPp_H_Btn_Link = FP_HELP.PICTUREBOXES_GET("H_Btn_Link")
        FPc_H_ShortText = FP_HELP.CONTROLS("H_ShortText")
    End Sub

    Private Sub FP_HELP_Form_AfterDelete(ByVal sender_FP As FP) Handles FP_HELP.Form_AfterDelete
        REFRESH_CONTROLS()
    End Sub

    Private Sub FP_HELP_Form_AfterUpdate(ByVal sender_FP As FP) Handles FP_HELP.Form_AfterUpdate
        REFRESH_CONTROLS()
        FP_HELP_Show_Current()
    End Sub

    Private Sub FP_TEXTS_Form_AfterDelete(ByVal sender_FP As FP) Handles FP_TEXTS.Form_AfterDelete
        REFRESH_CONTROLS()
    End Sub

    Private Sub FP_TEXTS_Form_AfterUpdate(ByVal sender_FP As FP) Handles FP_TEXTS.Form_AfterUpdate
        REFRESH_CONTROLS()
        FP_TEXTS_Show_Current()
    End Sub

    Private Sub FP_HELP_Form_BeforeUpdate(ByVal sender_FP As FP, ByRef Cancel As Integer) Handles FP_HELP.Form_BeforeUpdate
        With FP_HELP
            .DATA_Field_setValue("H_ServerObject_Prefix", GET_CURRENT_ServerObject_Prefix)
            .DATA_Field_setValue("H_SubPrefix", GET_CURRENT_SubPrefix)
            .DATA_Field_setValue("H_FieldName", FieldName.Text)
        End With
    End Sub

    Private Sub FP_TEXTS_Form_BeforeUpdate(ByVal sender_FP As FP, ByRef Cancel As Integer) Handles FP_TEXTS.Form_BeforeUpdate
        With FP_TEXTS
            .DATA_Field_setValue("T_ServerObject_Prefix", GET_CURRENT_ServerObject_Prefix)
            If FieldName.Text = "#FORM#" Or CurrentField_IsLabel() Or CurrentField_IsTabPage() Or CurrentField_IsButton() Then
                .DATA_Field_setValue("T_LabelName", FieldName.Text)
            Else
                .DATA_Field_setValue("T_LabelName", LabelName.Text)
            End If

            .DATA_Field_setValue("T_SubPrefix", GET_CURRENT_SubPrefix)
        End With
    End Sub

    Private Sub FP_ARRANGE_Form_Field_AfterUpdate(ByVal FPc As FP_Control) Handles FP_ARRANGE.Form_Field_AfterUpdate
        FP_ARRANGE_SET_LAYOUT()
    End Sub

    Private Sub FPc_ArrangeType_Field_BeforeUpdate(ByVal sender_FPc As FP_Control, ByRef Cancel As Integer) Handles FPc_ArrangeType.Field_BeforeUpdate
        FP_ARRANGE_SET_NEXTFIELD()
    End Sub

    Private Sub FP_HELP_Form_Current(ByVal sender_FP As FP) Handles FP_HELP.Form_Current
        FP_HELP_Show_Current()
    End Sub

    Private Sub FP_TEXTS_Form_Current(ByVal sender_FP As FP) Handles FP_TEXTS.Form_Current
        FP_TEXTS_Show_Current()
    End Sub

    Private Sub FP_HELP_Form_NoRecord(ByVal sender_FP As FP) Handles FP_HELP.Form_NoRecord
        FP_HELP_Show_Current()
    End Sub

    Private Sub FP_TEXTS_Form_NoRecord(ByVal sender_FP As FP) Handles FP_TEXTS.Form_NoRecord
        FP_TEXTS_Show_Current()
    End Sub

    Private Sub FPp_H_Btn_Link_CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FPp_H_Btn_Link.CLICK
        If FP_HELP.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            FPf.FPApp.HELP_Link_Execute(H_Link.Text)
        End If
    End Sub

    Private Sub TabControl_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl.SelectedIndexChanged
        FP_HELP_Show_Current()
        FP_TEXTS_Show_Current()
    End Sub

    Private Sub FPc_H_ShortText_Field_TextChanged(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByVal e As System.EventArgs, ByRef Cancel As Boolean) Handles FPc_H_ShortText.Field_TextChanged
        FP_HELP_Show_Current()
    End Sub

    Private Sub FPc_T_ShortText_Field_TextChanged(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByVal e As System.EventArgs, ByRef Cancel As Boolean) Handles FPc_T_ShortText.Field_TextChanged
        FP_TEXTS_Show_Current()
    End Sub

    Private Sub FP_TEXTS_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP_TEXTS.CONTROLS_INITIALIZED
        FPc_T_ShortText = FP_TEXTS.CONTROLS("T_ShortText")
    End Sub

    Private Sub FP_HELP_GRID_Filter_Changed(ByVal sender_FP As FP) Handles FP_HELP.GRID_Filter_Changed
        If FP_HELP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            Dim FirstRow_RecordID As Long = FP_HELP.GRID.ROW_GET_RecordID(0)

            If FirstRow_RecordID <> 0 Then
                FP_HELP.FORM_GOTO_RECORD_BY_ID(FirstRow_RecordID)
            End If
        End If
    End Sub

    Private Sub FP_TEXTS_GRID_Filter_Changed(ByVal sender_FP As FP) Handles FP_TEXTS.GRID_Filter_Changed
        If FP_TEXTS.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            Dim FirstRow_RecordID As Long = FP_TEXTS.GRID.ROW_GET_RecordID(0)

            If FirstRow_RecordID <> 0 Then
                FP_TEXTS.FORM_GOTO_RECORD_BY_ID(FirstRow_RecordID)
            End If
        End If
    End Sub

    Private Sub ToolStrip_RS_Fields0_To_DEBUG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStrip_RS_Fields0_To_DEBUG.Click
        Dim P As New Struct_Simple_SELECT_Params
        Dim P_OUT As New Struct_Simple_SELECT_OutputParams

        P.FixText_Key = "@@VB_SIMPLE_SELECT_RS_Fields0"

        If FPf.FPApp.SIMPLE_SELECT(P, P_OUT) Then
            If FPf.FPApp.DoMyMsgBox(1207, , "SEQ,NO", "SEQ,YES") = 2 Then 'Continue?
                Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()
                Dim Result As Boolean

                FPf.FPApp.DC.Qdf_set_SP(sqlComm, "RS_SETUP_Fields0_To_DEBUG")
                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , P_OUT.RS_ID)
                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)

                CURSOR_SHOW_WAIT()
                Try
                    Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
                Catch ex As Exception
                    Result = False
                    FPf.FPApp.DoErrorMsgBox("FP_Setup.ToolStrip_RS_Fields0_To_DEBUG_Click", Err.Number, Err.Description)
                End Try

                CURSOR_SHOW_DEFAULT()

                If Result Then
                    Dim ee As New System.EventArgs

                    ServerObject_Prefix_SelectedValueChanged(ServerObject_Prefix, ee)

                    FPf.FPApp.DoMyMsgBox(1208) 'A folyamat rendben befejezodott.
                End If
            End If
        End If
    End Sub

    Private Sub ToolStrip_DEBUG_To_RS_Fields0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStrip_DEBUG_To_RS_Fields0.Click
        Dim P As New Struct_Simple_SELECT_Params
        Dim P_OUT As New Struct_Simple_SELECT_OutputParams

        P.FixText_Key = "@@VB_SIMPLE_SELECT_RS_Fields_DEBUG"

        If FPf.FPApp.SIMPLE_SELECT(P, P_OUT) Then
            If FPf.FPApp.DoMyMsgBox(1209, , "SEQ,NO", "SEQ,YES") = 2 Then   'Continue?
                Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()
                Dim Result As Boolean

                FPf.FPApp.DC.Qdf_set_SP(sqlComm, "RS_SETUP_DEBUG_To_Fields0")
                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , P_OUT.RS_ID)
                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)

                CURSOR_SHOW_WAIT()
                Try
                    Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
                Catch ex As Exception
                    Result = False
                    FPf.FPApp.DoErrorMsgBox("FP_Setup.ToolStrip_DEBUG_To_RS_Fields0_Click", Err.Number, Err.Description)
                End Try

                CURSOR_SHOW_DEFAULT()

                If Result Then
                    Dim ee As New System.EventArgs

                    ServerObject_Prefix_SelectedValueChanged(ServerObject_Prefix, ee)

                    FPf.FPApp.DoMyMsgBox(1208) 'A folyamat rendben befejezodott.
                End If
            End If
        End If
    End Sub

    Private Sub ToolStrip_Generate_SQL_Script_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStrip_Generate_SQL_Script.Click
        Dim TargetDatabaseName As String = ""
        Dim WithDelete As String = ""
        Dim WithDepends As String = ""
        Dim AppendFile As String = ""
        Dim OnlyOpenedForm As String = ""
        Dim StandardSettings As String = ""
        Dim CustomizedSettings As String = ""
        Dim SELECT_Params As New Struct_Simple_SELECT_Params
        Dim SELECT_Params_OUT As New Struct_Simple_SELECT_OutputParams
        Dim ParamsList As String = ""
        Dim sqlComm As SqlCommand = FPApp.DC.CNN.CreateCommand()
        Dim OUT As Integer
        Dim SavedFolderPath As String = ""
        Dim DialogResult As DialogResult

        Dim SE As New FP_Simple_Edit(FPApp, "Script_Form_TargetDatabase")

        FPApp.PFDlesen("SCRIPT_FORM_TARGETDATABASE", TargetDatabaseName)
        FPApp.PFDlesen("SCRIPT_FORM_WITHDELETE", WithDelete)
        FPApp.PFDlesen("SCRIPT_FORM_WITHDEPENDS", WithDepends)
        FPApp.PFDlesen("SCRIPT_FORM_APPENDFILE", AppendFile)
        FPApp.PFDlesen("SCRIPT_FORM_ONLYOPENEDFORM", OnlyOpenedForm)
        FPApp.PFDlesen("SCRIPT_FORM_STANDARDSETTINGS", StandardSettings)
        FPApp.PFDlesen("SCRIPT_FORM_CUSTOMIZEDSETTINGS", CustomizedSettings)

        With SE
            .DATAFIELD_ADD("TargetDatabaseName", TargetDatabaseName)
            .DATAFIELD_ADD("WithDelete", IIf(WithDelete = "False", 0, 1))
            .DATAFIELD_ADD("WithDepends", IIf(WithDepends = "False", 0, 1))
            .DATAFIELD_ADD("AppendFile", IIf(AppendFile = "False", 0, 1))
            .DATAFIELD_ADD("OnlyOpenedForm", IIf(OnlyOpenedForm = "False", 0, 1))
            .DATAFIELD_ADD("StandardSettings", IIf(StandardSettings = "False", 0, 1))
            .DATAFIELD_ADD("CustomizedSettings", IIf(CustomizedSettings = "False", 0, 1))

            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                TargetDatabaseName = .FP_SIMPLEEDIT.CONTROLS("TargetDatabaseName").P_VALUE
                WithDelete = .FP_SIMPLEEDIT.CONTROLS("WithDelete").P_VALUE
                WithDepends = .FP_SIMPLEEDIT.CONTROLS("WithDepends").P_VALUE
                AppendFile = .FP_SIMPLEEDIT.CONTROLS("AppendFile").P_VALUE
                OnlyOpenedForm = .FP_SIMPLEEDIT.CONTROLS("OnlyOpenedForm").P_VALUE
                StandardSettings = .FP_SIMPLEEDIT.CONTROLS("StandardSettings").P_VALUE
                CustomizedSettings = .FP_SIMPLEEDIT.CONTROLS("CustomizedSettings").P_VALUE

                FPApp.PFDinsertOrUpdate("SCRIPT_FORM_TARGETDATABASE", TargetDatabaseName)
                FPApp.PFDinsertOrUpdate("SCRIPT_FORM_WITHDELETE", WithDelete)
                FPApp.PFDinsertOrUpdate("SCRIPT_FORM_WITHDEPENDS", WithDepends)
                FPApp.PFDinsertOrUpdate("SCRIPT_FORM_APPENDFILE", AppendFile)
                FPApp.PFDinsertOrUpdate("SCRIPT_FORM_ONLYOPENEDFORM", OnlyOpenedForm)
                FPApp.PFDinsertOrUpdate("SCRIPT_FORM_STANDARDSETTINGS", StandardSettings)
                FPApp.PFDinsertOrUpdate("SCRIPT_FORM_CUSTOMIZEDSETTINGS", CustomizedSettings)
            Else
                Exit Sub
            End If
        End With

        FPApp.DC.Qdf_RunSQL("DELETE Dispo1 WHERE (Terminal = '" + Terminal + "') AND (Art = 'SCRIPT_FORM_FP')")

        FPApp.DC.Qdf_RunSQL("INSERT INTO Dispo1 (Art, Terminal, Dispo1Varchar, Dispo2Varchar, Dispo1Text) SELECT 'SCRIPT_FORM_FP', '" + Terminal + "', '" + Controlled_FPf.ServerObject_Prefix + "', 'FORM LEVEL', ''")

        For Each p In Controlled_FPf.FPs
            ParamsList += p.Value.SQL_BIND_Params.NameOf_READ + "|"
            ParamsList += p.Value.SQL_BIND_Params.NameOf_SAVE + "|"
            ParamsList += p.Value.SQL_BIND_Params.NameOf_DEL + "|"
            ParamsList += p.Value.SQL_BIND_Params.NameOf_GRID + "|"
            ParamsList += p.Value.SQL_BIND_Params.NameOf_FormDef + "|"
            ParamsList += p.Value.SQL_BIND_Params.NameOf_WhereQuery + "|"
            ParamsList += p.Value.FORMDEF("FilterName") + "|"
            ParamsList += p.Value.FORMDEF("ZDISPO_OpenArgs") + "|"
            ParamsList += p.Value.FORMDEF("Right_StoredProc") + "|"

            FPApp.DC.Qdf_RunSQL("INSERT INTO Dispo1 (Art, Terminal, Dispo1Varchar, Dispo2Varchar, Dispo1Text) SELECT 'SCRIPT_FORM_FP', '" + Terminal + "', '" + p.Value.ServerObject_Prefix + "', '" + p.Value.SubPrefix + "', '" + ParamsList + "'")

            ParamsList = ""
        Next

        With SELECT_Params
            .FixText_Key = "SCRIPT_FORM_FP"
            .SQL_WHERE = "Terminal='" + Terminal + "'"
        End With

        If Not FPf.FPApp.SIMPLE_SELECT(SELECT_Params, SELECT_Params_OUT) Then
            Exit Sub
        End If

        With FPApp.DC
            .Qdf_set_SP(sqlComm, "RS_SETUP_SCRIPT_FORM_GET_OBJECTS")
            .Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , SELECT_Params_OUT.RS_ID)
            .Qdf_AddParameter(sqlComm, "@StandardSettings", SqlDbType.Bit, , , , , IIf(StandardSettings = "False", 0, 1))
            .Qdf_AddParameter(sqlComm, "@CustomizedSettings", SqlDbType.Bit, , , , , IIf(CustomizedSettings = "False", 0, 1))

            Try
                OUT = .Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE)
                If OUT <> -1 Then
                    Exit Sub
                End If

            Catch ex As Exception
                FPApp.DoErrorMsgBox("Form_FP_Setup.ToolStrip_Generate_SQL_Script", Err.Number, Err.Description)
                Exit Sub
            End Try
        End With

        With SELECT_Params
            .FixText_Key = "SCRIPT_FORM_OBJECTS"
            .SQL_WHERE = "Terminal='" + Terminal + "'"
        End With

        If Not FPf.FPApp.SIMPLE_SELECT(SELECT_Params, SELECT_Params_OUT) Then
            Exit Sub
        End If

        With FPApp.DC
            .Qdf_RunSQL(String.Format("DELETE Dispo1 WHERE (Terminal='{0}') AND (Art='SERVICES')", Terminal))

            .Qdf_set_SP(sqlComm, "RS_SETUP_SCRIPT_FORM_GET_SQL_SCRIPTS")
            .Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , SELECT_Params_OUT.RS_ID)
            .Qdf_AddParameter(sqlComm, "@WithDelete", SqlDbType.Bit, , , , , IIf(WithDelete = "False", 0, 1))
            .Qdf_AddParameter(sqlComm, "@TargetDatabaseName", SqlDbType.NVarChar, , 255, TargetDatabaseName)

            Try
                Dim SplashForm As New FP_L_SplashForm(FPf.Frm, "Script készítése folyamatban...")

                OUT = .Qdf_Execute(FPf, sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE)

                SplashForm.CloseSplashForm()

                If OUT <> -1 Then
                    Exit Sub
                End If

            Catch ex As Exception
                FPApp.DoErrorMsgBox("Form_FP_Setup.ToolStrip_Generate_SQL_Script", Err.Number, Err.Description)
                Exit Sub
            End Try
        End With

        FPApp.PFDlesen("SCRIPT_FORM_PATH", SavedFolderPath)

        If AppendFile = True Then
            Dim OpenFileDialog As New OpenFileDialog

            OpenFileDialog.InitialDirectory = SavedFolderPath
            OpenFileDialog.Filter = "SQL Files | *.sql"

            DialogResult = OpenFileDialog.ShowDialog
            SavedFolderPath = OpenFileDialog.FileName
        Else
            Dim SaveFileDialog As New SaveFileDialog

            SaveFileDialog.InitialDirectory = SavedFolderPath
            SaveFileDialog.Filter = "SQL Files | *.sql"

            DialogResult = SaveFileDialog.ShowDialog
            SavedFolderPath = SaveFileDialog.FileName
        End If

        Cursor = Cursors.WaitCursor

        If DialogResult = Windows.Forms.DialogResult.OK Then
            FPApp.PFDinsertOrUpdate("SCRIPT_FORM_PATH", SavedFolderPath)

            Dim txtWriter As New System.IO.StreamWriter(SavedFolderPath, AppendFile)
            Dim DT As DataTable = Nothing

            If Not gl_FPApp.DC.Qdf_Fill_DT(String.Format("SELECT Dispo1Text FROM Dispo1 WITH (READUNCOMMITTED) WHERE (Terminal='{0}') AND (Art='SERVICES') ORDER BY ID", Terminal), DT) Then
                Exit Sub
            End If

            For Each row As DataRow In DT.Rows
                txtWriter.Write(row.Item(0))
            Next

            txtWriter.Close()

            MsgBox("Script készítése sikeresen befejezve!")
        End If

        Cursor = Cursors.Default
    End Sub

    Private Function FieldNameField_KeyPress(ByVal sender As FP_Control, ByVal e As System.Windows.Forms.KeyPressEventArgs) As Boolean
        Dim OUT As Boolean = False

        If Not Char.IsControl(e.KeyChar) Then
            Dim ControlsOnForm As String = "#FORM#;"

            For Each AktKey As String In Controlled_FPf.CONTROLS.Keys
                If Mid(AktKey, 1, 1) <> "#" Then
                    ControlsOnForm += AktKey + ";"
                End If
            Next

            Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()
            Dim Result As Boolean = False

            With FPf.FPApp.DC
                .Qdf_set_SP(sqlComm, "RS_SETUP_SET_FieldName_RowSource")
                .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                .Qdf_AddParameter(sqlComm, "@Controls", SqlDbType.NVarChar, , -1, ControlsOnForm)
                .Qdf_AddParameter(sqlComm, "@FieldPrefix", SqlDbType.NVarChar, , 128, GET_CURRENT_FieldPrefix)
            End With

            CURSOR_SHOW_WAIT()
            Try
                Result = FPf.Qdf_Execute(sqlComm)
            Catch ex As Exception
                FPf.FPApp.DoErrorMsgBox("FP_Setup.FieldName_KeyPress", Err.Number, Err.Description)
            End Try
            CURSOR_SHOW_DEFAULT()

            If Result Then
                Dim P As New Struct_Simple_SELECT_Params
                Dim P_OUT As New Struct_Simple_SELECT_OutputParams

                With P
                    .FixText_Key = "@@VB_SIMPLE_SELECT_RS_SETUP_CONTROLS"
                    .SQL_WHERE = "Terminal = '@TERMINAL'"
                    .FPc = sender
                    .Field_Mandatory = False
                End With

                OUT = FP_Lines.FPf.FPApp.SIMPLE_SELECT_FIELD_KEYPRESS_DISPO(sender.c, 0, e, P, P_OUT)
            End If
        End If

        FieldNameField_KeyPress = OUT
    End Function

    Private Sub FieldName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles FieldName.KeyPress
        FieldNameField_KeyPress(FPc_FieldName, e)
    End Sub

    Private Sub LabelName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles LabelName.KeyPress
        If Not FP_Lines.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            If CreateAtRuntime.Checked = False Then
                FieldNameField_KeyPress(FPc_LabelName, e)
            End If
        End If
    End Sub

    Private Sub FPc_DataField_Field_BeforeUpdate(ByVal sender_FPc As FP_Control, ByRef Cancel As Integer) Handles FPc_DataField.Field_BeforeUpdate, FPc_FieldName.Field_BeforeUpdate
        If Not FieldName_Check(sender_FPc.c.Text, FP_Lines.CONTROLS("RS_FIELDS_DEBUG_SeqNum").P_VALUE) Then
            Cancel = True
        End If
    End Sub

    Private Sub FPc_DataField_Field_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc_DataField.Field_TextChanged
        Unbound_Controls_Field_TextChanged(sender_FPc, sender, e, Cancel)
    End Sub
End Class

