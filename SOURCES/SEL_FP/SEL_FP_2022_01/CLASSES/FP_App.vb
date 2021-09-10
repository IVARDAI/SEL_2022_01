Imports System.Data
Imports System.Data.SqlClient
Imports System.IO


'ZIP-hez (avagy 1 het kiborito kuzdelem rovid tortenete)
'-------------------------------------------------------
'Imports interop.Shell32                 - A leglogikusabb megoldas lenne a Windows-ban eleve meglevo ZIP funkcionalitast hivni de...
'                                                              a gepek tobbsegen nem mukodik az interop hivas (Win7? 32 bit? nem tudom miert)
'Imports System.IO.Packaging             - Szokasos Microsoft: mukodik, ha elvegzel egy mini-egyetemet es 3 hetig csak ezzel foglalkozol,
'                                                              raadasul nem is teljesen kompatibilis ZIP-et hoz letre.
'Imports System.IO.Compression           - Megint Microsoft:   Minek DotNet alatt 2 fele megoldas is ugyanarra a problemara?
'                                                              Raadasul ez is ugyanolyan horror, mint a System.IO.Packaging
'                                                              Raadasul elobb-utobb rajosz, hogy nem is ZIP file-t hoz letre,
'                                                              hanem GZ file-t, ami szerintuk nagyon ismert, bar en meg soha nem hallottam rola.
'Imports Ionic                           - (3th party, ismertebb neven DotNetZip) Nagyon kezreallo, oda vagy tole meg vissza,
'                                                              csak eppen nem hozza letre a ZIP-et. (elszall)
Imports ICSharpCode.SharpZipLib.Core    '- (3th party) Nem annyira szep a kod, de egybol mukodott, igy ezt valasztottam.
Imports ICSharpCode.SharpZipLib.Zip
Imports Newtonsoft.Json

Public Class FP_App
    Public Enum ENUM_FP_APP_RUNNING_STATE As Integer
        RUNNING = 0
        ON_CLOSING = 99
        CLOSED = 999
    End Enum

    Protected Friend FP_APP_RUNNING_STATE As ENUM_FP_APP_RUNNING_STATE

    Public Event MenuItem_Activated(ByVal sender As FP_MenuItem.Struct_FP_MenuItem_Params)
    Public Event APPLICATION_CLOSED(ByVal sender As FP_App)
    Public Event Central_Menu_Show()
    Public Event Marker_Clicked(Clicked_FPc As FP_Control, ActionCode As String, ByRef Individual_Params As Object, ByRef Handled As Boolean)
    Public Event Message(sender As FP_App, From_FPf As FP_Form, MessageCode As String, ByRef Individual_Params As Object, ByRef Handled As Boolean)

    Private Disposed As Boolean = False
    Public Parent As FP_App = Nothing
    Public UserGruppe As Long
    Public UserStufe As Long
    Public UserRights As New Struct_Rights
    Public Layout_Style As ENUM_LAYOUT_STYLE = ENUM_LAYOUT_STYLE.NORMAL

    Public SKIN_DLLs As New Dictionary(Of String, System.Reflection.Assembly)
    Public DIC_Docks As New Dictionary(Of String, FP_L_Dock_Forms)
    Public DIC_PFD As New Dictionary(Of String, String)
    Private MENUITEM_PROPS_LEVELS As DataTable
    Private MENUITEM_PROPS_GROUPS As DataTable
    Private MENUITEM_PROPS_Loaded As Boolean = False

    Private DebugMode As Boolean = False

    Public INIFileName As String = "SelExped.INI"

    Public Cashed_Forms As New FP_CASH_FORMS(Me)

    Public WithEvents DC As FP_DataConnect

    Public P As Struct_PA

    Public Forms As New Dictionary(Of Long, FP_Form)
    Public StartForm As Form
    Public Initialized As Boolean = False

    Public LandDialog As String = ""

    Public MyMsgTitel As String
    Public MyMsgText As String

    Public gl_FilterParams As New Struct_DoFilter_gl_Params

    Protected Friend Forms_LastActivated As FP_Form = Nothing
    Protected Friend DT_Countries As DataTable = Nothing
    Protected Friend DT_Currencies As DataTable = Nothing
    Protected Friend DT_TRN_Params As DataTable = Nothing

    Public DT_Installed_Products As DataTable
    Public CL_Statuscodes As CL_Statuscodes
    Public CL_AddressTypes As CL_AddressTypes
    Public CL_TASK_TYPES As CL_TASK_TYPES
    Protected Friend ShowDialogForm_InActivate As Boolean

    ReadOnly DIC_Bitmaps As New Dictionary(Of String, Bitmap)

    Public ReadOnly Property P_Disposed As Boolean
        Get
            Return Disposed
        End Get
    End Property

    Public ReadOnly Property P_RUNNING_STATE As ENUM_FP_APP_RUNNING_STATE
        Get
            Return FP_APP_RUNNING_STATE
        End Get
    End Property

    Public ReadOnly Property P_DebugMode() As Boolean
        Get
            P_DebugMode = DebugMode
        End Get
    End Property

    Public Sub Bitmaps_SAVE(Name As String, MyBitmap As Bitmap)
        If Not DIC_Bitmaps.Keys.Contains(Name) Then
            DIC_Bitmaps.Add(Name, MyBitmap)
        End If
    End Sub

    Public Function Installed_Products_Exists(ProductCode As String, Optional DialNum As Integer = 0, Optional Param As String = "") As Boolean
        Dim OUT As Boolean = False
        Dim Crit As String = String.Format("Code = '{0}'", ProductCode)

        If Param <> "" Then
            Crit = String.Format("{0} AND Params = '{1}'", Crit, Param)
        End If

        If DT_Installed_Products.Select(Crit).Count = 0 Then
            If DialNum > 0 Then
                DoMyMsgBox(DialNum)
            End If
        Else
            OUT = True
        End If

        Return OUT
    End Function

    Public Function Bitmaps_LOAD(Name As String, ByRef OUT_Bitmap As Bitmap) As Boolean
        Dim OUT As Boolean = False

        OUT_Bitmap = Nothing

        If DIC_Bitmaps.Keys.Contains(Name) Then
            OUT_Bitmap = DIC_Bitmaps(Name)
            OUT = True
        End If

        Return OUT
    End Function

    Public Function FORMS_GET_ACTIVE_FPf() As FP_Form
        Dim OUT As FP_Form = Nothing

        If Not (Form.ActiveForm Is Nothing) Then
            OUT = FORMS_GET_FPf(Form.ActiveForm)
        End If

        If OUT Is Nothing Then
            OUT = Forms_LastActivated
        End If

        FORMS_GET_ACTIVE_FPf = OUT
    End Function

    Public Function FORMS_GET_FROM_NAME(ByVal Name As String, Optional Allways_nothing_when_shift_pressed As Boolean = True) As FP_Form
        Dim OUT As FP_Form = Nothing

        If Allways_nothing_when_shift_pressed Then
            Dim CtrlPressed = My.Computer.Keyboard.ShiftKeyDown

            If CtrlPressed = True Then
                Return Nothing
            End If
        End If

        For Each AktKey As Long In Forms.Keys
            With Forms(AktKey)
                If Not (.Frm Is Nothing) Then
                    If .Frm.Name = Name Then
                        OUT = Forms(AktKey)
                        Exit For
                    End If
                End If
            End With
        Next

        Return OUT
    End Function

    Public Sub RAISEEVENT_MenuItem_Activated(ByVal P As FP_MenuItem.Struct_FP_MenuItem_Params)
        Dim Do_RaiseEvent As Boolean = True

        Select Case Trim(P.Action)
            Case "HTTP"
                Do_RaiseEvent = False
                Process.Start(P.OpenArgs)

            Case "FOLDER"
                Do_RaiseEvent = False

                Dim MyPath As String = Trim(P.OpenArgs)

                If Strings.Left(MyPath, 1) <> "\" Then
                    MyPath = "\" + MyPath
                End If

                MyPath = EXE_Dir() + MyPath

                If System.IO.Directory.Exists(MyPath) Then
                    Process.Start(EXE_Dir() + "\" + Trim(P.OpenArgs))
                Else
                    MsgBox(String.Format("Directory not found: {0} A művelet megszakadt.", MyPath))
                End If

            Case "EXE"
                Do_RaiseEvent = False

                Dim MyPath As String = Trim(Trim(P.OpenArgs))

                If Strings.Left(MyPath, 1) <> "\" Then
                    MyPath = "\" + MyPath
                End If

                MyPath = EXE_Dir() + MyPath

                If System.IO.File.Exists(MyPath) Then
                    Process.Start(EXE_Dir() + "\" + Trim(P.OpenArgs))
                Else
                    MsgBox(String.Format("File not found: {0} A művelet megszakadt.", MyPath))
                End If

            Case Else
                'Nothing to do
        End Select

        If Do_RaiseEvent Then
            RaiseEvent MenuItem_Activated(P)
        End If
    End Sub

    Public Sub RAISEEVENT_APPLICATION_CLOSED()
        If (StartForm IsNot Nothing) Then
            RaiseEvent APPLICATION_CLOSED(Me)
            StartForm = Nothing
        End If
    End Sub

    Public Sub RAISEEVENT_Central_Menu_Show()
        RaiseEvent Central_Menu_Show()
    End Sub

    Public Sub RAISEEVENT_Message(MessageCode As String, From_FPf As FP_Form, ByRef Individual_Params As Object, ByRef Handled As Boolean)
        RaiseEvent Message(Me, From_FPf, MessageCode, Individual_Params, Handled)
    End Sub

    Public Sub RAISEEVENT_Marker_Clicked(Clicked_FPc As FP_Control, ActionCode As String, ByRef Individual_Params As Object, ByRef Handled As Boolean)
        RaiseEvent Marker_Clicked(Clicked_FPc, ActionCode, Individual_Params, Handled)
    End Sub

    Public Sub RAISEEVENT_Marker_Clicked(FPf As FP_Form, ID As Long, ActionCode As String, ByRef Individual_Params As Object, ByRef Handled As Boolean)
        Dim c As New TextBox
        Dim FP As New FP(FPf, "NOTHING", , True)

        Dim Clicked_FPc As New FP_Control(True)

        With Clicked_FPc
            .FPf = FP.FPf
            .FP = FP
            .c = c
            .Selected_ID = ID
        End With

        RaiseEvent Marker_Clicked(Clicked_FPc, ActionCode, Individual_Params, Handled)
    End Sub

    Public Function BACKGROUND_SET(ByVal c As Control, ByVal BackgroundImage As Bitmap) As Boolean
        Dim OUT As Boolean = True

        If Not (BackgroundImage Is Nothing) Then
            If TypeOf c Is TabPage Then
                CType(c, TabPage).BackgroundImage = BackgroundImage
                CType(c, TabPage).BackgroundImageLayout = ImageLayout.Stretch

            ElseIf TypeOf c Is Panel Then
                CType(c, Panel).BackgroundImage = BackgroundImage
                CType(c, Panel).BackgroundImageLayout = ImageLayout.Stretch

            ElseIf TypeOf c Is Button Then
                CType(c, Button).BackgroundImage = BackgroundImage
                CType(c, Button).BackgroundImageLayout = ImageLayout.Stretch

            ElseIf TypeOf c Is PictureBox Then
                CType(c, PictureBox).BackgroundImage = BackgroundImage
                CType(c, PictureBox).BackgroundImageLayout = ImageLayout.Stretch

            Else
                OUT = False
                DoErrorMsgBox("FP_App.BACKGROUND_SET", 0, String.Format("Type of Control '{0}' is unknown.", c.Name))
            End If
        End If

        BACKGROUND_SET = OUT
    End Function

    Public Function BACKGROUND_SET(ByVal c As Control, ByVal BackgroundImageName As String) As Boolean
        'a hatterkepet kiterjesztessel egyutt kell megadni! (Peldaul: 'kep.png'
        'A 'kep.png' formatum azt jelenti, hogy a kep.png a SEL_FP.dll-ben van benne.
        'A SEL_FP.dll-ben valaszthato kepek a SEL_FP/FP_SKIN konyvtarban talalhatok.
        'Ha sajat projectunkben hoztunk letre kepeket, akkor azt a kovetkezo formaban kell megadni:
        '            "<My.Application.Info>..kep.png"
        'A kod felismeri es kicsereli a "<APP_NAME>" elotagot a project sajat elotagjara. Vagyis helyes a kovetkezo kifejezes: "<APP_NAME>..kep.png"
        'A project-ek Application Name elotagjat a Project/Properties menupont alatt az "Application" fulon lehet beallitani illetve kiolvasni.
        'Ne felejtsd el, hogy a kepeket, amiket hozzaadsz a project-hez, a property ablakban allitsd "Embended resource"-ra, maskeppen NEM fog mukodni!!!
        Dim OUT As Boolean = False

        If BackgroundImageName > "" Then
            If DIC_Bitmaps.Keys.Contains(BackgroundImageName) Then
                OUT = BACKGROUND_SET(c, DIC_Bitmaps(BackgroundImageName))
            Else
                Dim asm As Reflection.Assembly = Nothing

                If SKIN_getASM_And_OBJECTNAME(BackgroundImageName, asm, BackgroundImageName) Then
                    Try
                        Dim backGroundImage As New Bitmap(asm.GetManifestResourceStream(BackgroundImageName))
                        OUT = BACKGROUND_SET(c, backGroundImage)
                    Catch ex As Exception
                        DoErrorMsgBox("FP_App.BACKGROUND_SET", 0, String.Format("Set form background to {0} was not successfull.", BackgroundImageName))
                    End Try
                End If
            End If
        End If

        BACKGROUND_SET = OUT
    End Function

    Public Sub DoFilter_Params_CLEAR(ByRef FilterParams As Struct_DoFilter_gl_Params, ByVal LetNewRecord As Boolean)
        With FilterParams
            .DoIt = False
            .ProcessID = 0
            .LetNewRecord = LetNewRecord
            .FilterText = ""
            .FilterWHERE = ""

            ReDim .ParamInt(30)
            ReDim .ParamDbl(30)
            ReDim .ParamStr(30)
            ReDim .ParamDate(30)
            ReDim .FilterFields(0)
            ReDim .FilterTexts(0)
        End With
    End Sub

    Public Sub Files_EMF_Remove()
        For Each myFile In Directory.GetFiles(Application.StartupPath, "*.EMF")
            Try
                File.Delete(myFile)

            Catch ex As Exception
                'Nothing to do
            End Try
        Next
    End Sub


    Public Function ShowDialogForm_getOpacityForm(ByVal MyForm As Form) As Form
        Dim OUT As Form = Nothing

        If Not (MyForm Is Nothing) Then
            If MyForm.Visible Then
                Dim Old_ActiveForm As FP_Form = FORMS_GET_ACTIVE_FPf()

                OUT = New Form
                With OUT
                    .BackColor = Color.WhiteSmoke
                    .Location = MyForm.Location

                    .TopLevel = True
                    .Owner = MyForm
                    .AllowTransparency = True

                    .Size = MyForm.Size
                    .Opacity = 0.7
                    .StartPosition = FormStartPosition.Manual
                    .ShowInTaskbar = False
                    .Region = MyForm.Region

                    .FormBorderStyle = FormBorderStyle.None

                    .Show()
                End With

                If Not (Old_ActiveForm Is Nothing) Then
                    If Not MyForm.Equals(Old_ActiveForm.Frm) Then
                        If Not (Old_ActiveForm Is Nothing) Then
                            If Not (Old_ActiveForm.Frm Is Nothing) Then
                                With Old_ActiveForm.Frm
                                    .BringToFront()
                                    .Activate()
                                End With
                            End If
                        End If
                    End If
                End If
            End If
        End If

        ShowDialogForm_getOpacityForm = OUT
    End Function

    Public Function FPf_ModuleIdentifier_GET(ByVal FPf As FP_Form) As String
        Dim OUT As String = ""

        If Not (FPf Is Nothing) Then
            OUT = FPf.ModuleIdentifier
        End If

        Return OUT
    End Function

    Public Function DoMyMsgBox(ByVal DialNr As Long, Optional ByVal Ersetz As String = "", Optional ByVal Button1 As String = "SEQ,OK", Optional ByVal Button2 As String = "", Optional ByVal Button3 As String = "", Optional ByVal ModuleIdentifier As String = "") As Long
        Dim OUT As Long = 0
        Try
            Dim MyMsgBox As New FP_MsgBox(Me, ModuleIdentifier, DialNr, Ersetz, Button1, Button2, Button3)
            Using MyMsgBox
                If MyMsgBox.P_IsOK Then
                    CURSOR_SHOW_DEFAULT()
                    Dim gl_Data_Binded_OLD As Boolean = gl_Data_Binded

                    gl_Data_Binded = False
                    MyMsgBox.Select()
                    OUT = ShowDialogForm(MyMsgBox)

                    gl_Data_Binded = gl_Data_Binded_OLD
                End If
            End Using

        Catch ex As Exception
            DoMyMsgBox_From_Resources(1000) 'Error by showing Dialog
        End Try
        Return OUT
    End Function

    Public Function DoMyMsgBox(ByVal FPf As FP_Form, ByVal DialNr As Long, Optional ByVal Ersetz As String = "", Optional ByVal Button1 As String = "SEQ,OK", Optional ByVal Button2 As String = "", Optional ByVal Button3 As String = "") As Long
        Dim ModuleIdentifier As String = FPf_ModuleIdentifier_GET(FPf)

        DoMyMsgBox(DialNr, Ersetz, Button1, Button2, Button3, ModuleIdentifier)
    End Function


    Protected Friend ShowDialogForm_RootFrm As Form = Nothing
    Protected Friend ShowDialogForm_SET_ZORDER_ACTIVE As Boolean = False

    Protected Friend Sub ShowDialogForm_SET_ZORDER(ByVal From_FPf As FP_Form, Optional ByRef OUT_FPf_Topmost As FP_Form = Nothing)
        ShowDialogForm_SET_ZORDER_ACTIVE = True

        OUT_FPf_Topmost = From_FPf
        Do While Not (From_FPf Is Nothing)
            With From_FPf
                If Not (.Frm Is Nothing) Then
                    From_FPf.Frm.BringToFront()
                    From_FPf.Frm.Refresh()
                End If

                If Not (.P_Enabled_Form_Opacity Is Nothing) Then
                    .P_Enabled_Form_Opacity.BringToFront()
                End If

                From_FPf = FORMS_GET_FPf(.P_Enabled_Form_Child)
                If Not (From_FPf Is Nothing) Then
                    OUT_FPf_Topmost = From_FPf
                End If
            End With
        Loop

        ShowDialogForm_SET_ZORDER_ACTIVE = False
    End Sub

    Public Function ShowDialogForm(ByVal MyDialogForm As Form, Optional ByVal ParentFPf As FP_Form = Nothing) As DialogResult
        Dim OUT As DialogResult = DialogResult.None

        If Not (MyDialogForm Is Nothing) Then
            If ParentFPf Is Nothing Then
                ParentFPf = FORMS_GET_ACTIVE_FPf()
                If Not (ParentFPf Is Nothing) Then
                    If ParentFPf.Equals(MyDialogForm) Then
                        ParentFPf = Nothing
                    End If
                End If
            End If

            If Not (ParentFPf Is Nothing) Then
                If ParentFPf.Frm Is Nothing Then
                    ParentFPf = Nothing
                End If
            End If

            If ParentFPf Is Nothing Then
                OUT = MyDialogForm.ShowDialog
            Else
                Dim RootFrm_setted As Boolean = False
                'Dim P_ENABLED_Old As Boolean = ParentFPf.P_ENABLED

                'ParentFPf.P_ENABLED = False
                ParentFPf.P_Enabled_Form_Child = MyDialogForm

                If ShowDialogForm_RootFrm Is Nothing Then
                    ShowDialogForm_RootFrm = ParentFPf.Frm
                    RootFrm_setted = True
                End If

                Dim Disabled_Forms_LST As New List(Of Long)
                Dim MyDialogForm_Handle As Long = Form_Handle(MyDialogForm)

                For Each Key As Integer In Forms.Keys
                    If Key <> MyDialogForm_Handle Then
                        If Forms(Key).P_ENABLED Then
                            Disabled_Forms_LST.Add(Key)
                            Forms(Key).P_ENABLED = False
                        End If
                    End If
                Next

                OUT = MyDialogForm.ShowDialog()

                If RootFrm_setted Then
                    ShowDialogForm_RootFrm = Nothing
                End If

                ParentFPf.P_Enabled_Form_Child = Nothing
                'ParentFPf.P_ENABLED = P_ENABLED_Old

                For Each Key As String In Disabled_Forms_LST
                    If Forms.ContainsKey(Key) Then
                        If Forms(Key).P_ENABLED = False Then
                            Forms(Key).P_ENABLED = True
                        End If
                    End If
                Next

                If Not (ParentFPf.Frm Is Nothing) Then
                    ParentFPf.Frm.Activate()
                End If
            End If
        End If

        Return OUT
    End Function

    Public Function SIMPLE_SELECT_QUERY_PREFIX_GET(Identifier As String) As String
        Dim OUT As String
        Dim FixText_Key As String = FP_Simple_SELECT.SIMPLE_SELECT_Prefix + Identifier.ToUpper
        Dim FixText As String = getFixText(FixText_Key)
        Dim FixText_DIC As New Dictionary(Of String, String)

        FIXTEXT_SPLIT_PARAMS(FixText, FixText_DIC)

        OUT = FIXTEXT_getParam("QUERY_PREFIX", FixText_DIC)

        Return OUT
    End Function

    Public Function SIMPLE_SELECT(ByVal Params As Struct_Simple_SELECT_Params, ByRef Out_Params As Struct_Simple_SELECT_OutputParams) As Boolean
        Dim OUT As Boolean = False
        Dim Form_Simple_SELECT As FP_Simple_SELECT = Nothing
        Dim Cash_Identifier As String = "SIMPLESELECT_HEAD|" + Params.FixText_Key
        Dim Cashed_FPf As FP_Form = Nothing
        Dim DoIt As Boolean = True

        If Cashed_Forms.Get_FPf_from_Cash(Cash_Identifier, FP_CASH_FORMS.ENUM_CASH_FORMS_SEARCH_TYPE.ONLY_CASHED_STATE, Cashed_FPf) = True Then
            Form_Simple_SELECT = Cashed_FPf.Frm
            Form_Simple_SELECT.INIT_CASHED_FORM(Params)
        Else
            Form_Simple_SELECT = New FP_Simple_SELECT(Me)
            Form_Simple_SELECT.FPf.P_Cash_Identifier = Cash_Identifier
            DoIt = Form_Simple_SELECT.INIT(Params)
        End If

        Out_Params = New Struct_Simple_SELECT_OutputParams

        With Form_Simple_SELECT
            If DoIt Then
                If ShowDialogForm(Form_Simple_SELECT) = DialogResult.OK Then
                    Out_Params = .Out_Params

                    OUT = True
                End If
            End If
        End With

        SIMPLE_SELECT = OUT
    End Function
    Public Function SIMPLE_SELECT_FIELD_KEYPRESS_DISPO(ByVal c As TextBox, ByRef ValueForSelectedID As Long, ByVal e As System.Windows.Forms.KeyPressEventArgs, ByVal Params As Struct_Simple_SELECT_Params, ByRef OUT_Params As Struct_Simple_SELECT_OutputParams) As Boolean
        Dim OUT As Boolean = False
        Dim MyText As String = ""
        Dim MySelectionStart As Long = 0
        Dim K As Integer = AscW(e.KeyChar)

        OUT_Params = New Struct_Simple_SELECT_OutputParams With {
            .Selected_ID = ValueForSelectedID
        }

        If c.Focused Then
            If K <> 27 Then 'Not ESC
                If Not e.Handled Then
                    Select Case K
                        Case 24 'Ctrl X
                            If c.SelectionStart = 0 And c.SelectionLength = Len(c.Text) Then
                                e.Handled = True
                                ValueForSelectedID = 0

                                Clipboard_SET_TEXT(c.Text)
                                If Params.NoSetText = False Then
                                    c.Text = ""
                                End If
                                OUT = True
                            Else
                                If c.SelectionLength > 0 Then
                                    Clipboard_SET_TEXT(Mid(c.Text, c.SelectionStart + 1, c.SelectionLength))
                                    If c.SelectionStart = 0 Then
                                        Params.Selected_Text = Mid(c.Text, c.SelectionLength)
                                    Else
                                        Params.Selected_Text = Mid(c.Text, 1, c.SelectionStart - 1) '+ Mid(c.Text, c.SelectionStart + c.SelectionLength + 1)
                                    End If
                                Else
                                    e.Handled = True
                                    OUT = True
                                End If
                            End If

                        Case 8 'Backspace
                            If c.SelectionStart = 0 And c.SelectionLength = Len(c.Text) Then
                                e.Handled = True
                                ValueForSelectedID = 0
                                If Params.NoSetText = False Then
                                    c.Text = ""
                                End If
                                OUT = True
                            Else
                                If c.SelectionLength > 0 Then
                                    If c.SelectionStart = 0 Then
                                        Params.Selected_Text = Mid(c.Text, c.SelectionLength)
                                    Else
                                        Params.Selected_Text = Mid(c.Text, 1, c.SelectionStart) '+ Mid(c.Text, c.SelectionStart + c.SelectionLength + 1)
                                    End If
                                Else
                                    If c.SelectionStart = 0 Then
                                        e.Handled = True
                                        OUT = False
                                    Else
                                        Params.Selected_Text = Mid(c.Text, 1, c.SelectionStart - 1) '+ Mid(c.Text, c.SelectionStart + c.SelectionLength + 1)
                                    End If
                                End If
                            End If
                    End Select
                End If

                If Not e.Handled Then
                    If K <> 3 Then 'Not Ctrl-C
                        If K = 22 Then 'Ctrl-V
                            Params.Selected_Text = nz(My.Computer.Clipboard.GetText, "")
                            If Params.Field_MULTILINE = False Then
                                Params.Selected_Text = Replace(Params.Selected_Text, Chr(13), "")
                                Params.Selected_Text = Replace(Params.Selected_Text, Chr(10), "")
                            End If
                            If Params.Field_TRIM = True Then
                                Params.Selected_Text = Trim(Params.Selected_Text)
                            End If
                        Else
                            If K > 31 Or K = 13 Then
                                If AscW(e.KeyChar) = 13 Then
                                    Params.Selected_Text = c.Text
                                Else
                                    MySelectionStart = CType(c, TextBox).SelectionStart
                                    If MySelectionStart > 0 Then
                                        Params.Selected_Text = Left(c.Text, MySelectionStart) + e.KeyChar
                                    Else
                                        Params.Selected_Text = e.KeyChar
                                    End If
                                End If
                            End If
                        End If

                        If K = 8 Or K = 22 Or K = 24 Or K = 13 Or K > 31 Then   'Backspace, Ctrl-V, Ctrl-X, Enter
                            Dim Simple_SELECT_Form As FP_Simple_SELECT = Nothing
                            Dim Cash_Identifier As String = "SIMPLESELECT_HEAD|" + Params.FixText_Key
                            Dim Cashed_FPf As FP_Form = Nothing
                            Dim Simple_SELECT_OpenFrom_Cash As Boolean = Cashed_Forms.Get_FPf_from_Cash(Cash_Identifier, FP_CASH_FORMS.ENUM_CASH_FORMS_SEARCH_TYPE.ONLY_CASHED_STATE, Cashed_FPf)

                            If Simple_SELECT_OpenFrom_Cash Then
                                Simple_SELECT_Form = Cashed_FPf.Frm
                            Else
                                Simple_SELECT_Form = New FP_Simple_SELECT(Me)
                            End If

                            ValueForSelectedID = 0
                            With Simple_SELECT_Form
                                If Simple_SELECT_OpenFrom_Cash Then
                                    .INIT_CASHED_FORM(Params)
                                Else
                                    .INIT(Params)
                                End If
                                If .List_Activated_By = FP_Simple_SELECT.ENUM_LIST_ACTIVATED_BY.STAR And e.KeyChar <> "*" Then
                                    Cashed_Forms.Add_to_Cash(Simple_SELECT_Form.FPf)
                                    Simple_SELECT_Form.FPf.P_Cash_State = FP_Form.ENUM_Cash_State.CASHED
                                Else
                                    If K = 22 And c.SelectionStart = 0 And c.SelectionLength = Len(c.Text) Then 'Ctrl-V
                                        Dim MyDT As New DataTable
                                        Dim MyWHERE As String = TEXT_AND(Simple_SELECT_Form.FP.DATA_RS_WHERE(Params.SQL_WHERE), String.Format("{0} = '{1}'", Simple_SELECT_Form.FIELD_TEXT, SQLStr(Params.Selected_Text)))
                                        Dim MySQL As String = Simple_SELECT_Form.FP.DATA_RS_SQL(MyWHERE, 2)

                                        If DC.Qdf_Fill_DT(MySQL, MyDT) Then
                                            If MyDT.Rows.Count = 1 Then
                                                OUT_Params = New Struct_Simple_SELECT_OutputParams With {
                                                    .GotoNextField = False,
                                                    .NO_LIMIT_TO_LIST = Simple_SELECT_Form.LimitToList,
                                                    .RS_ID = 0,
                                                    .Selected_ID = MyDT.Rows(0)!ID,
                                                    .Selected_Long = 0,
                                                    .Selected_String = "",
                                                    .Selected_Text = Params.Selected_Text
                                                }
                                                OUT = True
                                                e.Handled = True
                                            End If
                                        End If
                                    End If

                                    If e.Handled = False Then
                                        e.Handled = True

                                        If Simple_SELECT_Form.ShowDialog = DialogResult.OK Then
                                            If Params.NoSetText = False Then
                                                c.Text = .Out_Params.Selected_Text
                                            End If

                                            OUT_Params = .Out_Params
                                            ValueForSelectedID = .Out_Params.Selected_ID

                                            OUT = True
                                        End If
                                    End If
                                End If
                            End With
                        End If
                    End If
                End If
            End If
        End If

        SIMPLE_SELECT_FIELD_KEYPRESS_DISPO = OUT
    End Function

    Public Function ZDISPO(Identifier As String) As Boolean
        Dim OUT As Boolean = False

        Dim P_IN As New Struct_ZDISPO_Params
        Dim P_OUT As New Struct_ZDISPO_OutputParams

        With P_IN
            .Identifier = Identifier
        End With

        OUT = ZDISPO(P_IN, P_OUT)
    End Function

    Private Function ZDISPO_Spec_Params_Replace(MySTR As String, P_ZDISPO_IN As Struct_ZDISPO_Params)
        Dim OUT As String = MySTR

        OUT = Replace(OUT, "#_CURRENT_ID_#", P_ZDISPO_IN.Current_ID.ToString)
        OUT = Replace(OUT, "#_CURRENT_RS_ID_#", P_ZDISPO_IN.Current_RS_ID)

        Return OUT
    End Function

    Public Function ZDISPO(ByVal P_ZDISPO As Struct_ZDISPO_Params, ByRef P_ZDISPO_OUT As Struct_ZDISPO_OutputParams) As Boolean
        Dim OUT As Boolean = True
        Dim DOFILTER_Opened As Boolean = False

        P_ZDISPO_OUT = New Struct_ZDISPO_OutputParams

        If P_ZDISPO.Identifier = "" Then
            OUT = False
            DoMyMsgBox(P_ZDISPO.Parent_FPf, 41) 'ZDISPO kann ohne Parameters nicht geoffnet werden.
        Else
            '+++If Installed_Products_Exists(P_ZDISPO.Identifier, 132) Then
            Dim DispoDEF As New Struct_ZDISPO_DEF
            Dim Curr_DoFilter_Params As New Struct_DoFilter_gl_Params

            If Not DispoDEF_GET(P_ZDISPO.Identifier, DispoDEF) Then
                OUT = False
            Else
                With DispoDEF
                    .Fix_WHERE = ZDISPO_Spec_Params_Replace(.Fix_WHERE, P_ZDISPO)
                    .Report_SQL = ZDISPO_Spec_Params_Replace(.Report_SQL, P_ZDISPO)
                End With

                Dim SQL_WHERE As String = TEXT_AND(DispoDEF.Fix_WHERE, P_ZDISPO.SQL_WHERE)
                Dim RS_ID As Long = P_ZDISPO.RS_ID
                Dim Simple_Select_Params As New Struct_Simple_SELECT_Params
                Dim Simple_Select_Frm As FP_Simple_SELECT = Nothing

                'If RS_ID <> 0 Then
                SQL_WHERE = Replace(SQL_WHERE, "@RS_ID", RS_ID)
                P_ZDISPO.RS_Type = Replace(P_ZDISPO.RS_Type, "@RS_ID", RS_ID)
                'End If
                SQL_WHERE = Text_ParametersErsetzen(SQL_WHERE, "")
                P_ZDISPO.RS_Type = Text_ParametersErsetzen(P_ZDISPO.RS_Type, "")

                If OUT Then
                    If DispoDEF.Simple_Select > "" And (RS_ID = 0 Or P_ZDISPO.Show_SimpleSelect = True) Then
                        Simple_Select_Frm = New FP_Simple_SELECT(Me)

                        With Simple_Select_Params
                            .FixText_Key = DispoDEF.Simple_Select
                            .NoSetText = True
                            .Selected = P_ZDISPO.Selected
                            .SQL_WHERE = SQL_WHERE
                            .NoMessageForNoRecord = P_ZDISPO.NoMessageForNoRecord
                            .PressOK = P_ZDISPO.PressOK
                            .RS_ID = RS_ID
                        End With

                        If Not Simple_Select_Frm.INIT(Simple_Select_Params) Then
                            OUT = False
                        Else
                            DispoDEF.DoFilter_WhereQuery = Simple_Select_Frm.QUERY_PREFIX + "_WhereQuery"
                        End If
                    End If
                End If

                'DoFilter
                If OUT Then
                    'If DispoDEF.DoFilter > "" And RS_ID = 0 Then
                    If DispoDEF.DoFilter > "" Then
                        DOFILTER_Opened = True
                        OUT = DOFILTER(P_ZDISPO.Parent_FPf, DispoDEF.DoFilter, DispoDEF.DoFilter_WhereQuery, , False, SQL_WHERE)
                        If OUT Then
                            Curr_DoFilter_Params = gl_FilterParams
                            SQL_WHERE = TEXT_AND(SQL_WHERE, Curr_DoFilter_Params.FilterWHERE)
                        End If
                    End If
                End If

                If OUT Then
                    If Trim(DispoDEF.StoredProc_1) > "" Then
                        Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

                        DC.Qdf_set_SP(sqlComm, DispoDEF.StoredProc_1, 0)
                        DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal) 'Azert kell, mert ha se simple_select, se dofilter nincs megadva, akkor csak ebbol lehet megtudni a terminal-t
                        DC.Qdf_AddParameter(sqlComm, "@ProcessID", SqlDbType.Int, , , , , Curr_DoFilter_Params.ProcessID)
                        If RS_ID = 0 Then
                            DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, ParameterDirection.Output)
                        Else
                            DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , RS_ID)
                        End If
                        DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                        DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.VarChar, ParameterDirection.Output, 255)
                        DC.Qdf_AddParameter(sqlComm, "@ErrParams", SqlDbType.VarChar, ParameterDirection.Output, 255)

                        CURSOR_SHOW_WAIT()
                        Try
                            OUT = DC.Qdf_Execute(P_ZDISPO.ModuleIdentifier, sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrParams")
                            If RS_ID = 0 Then
                                RS_ID = nz(sqlComm.Parameters("@RS_ID").Value, 0)
                            End If
                        Catch ex As Exception
                            OUT = False
                            DoErrorMsgBox(P_ZDISPO.Parent_FPf, "FP_App.ZDISPO", Err.Number, Err.Description)
                        End Try

                        CURSOR_SHOW_DEFAULT()
                    End If
                End If

                If OUT Then
                    If DispoDEF.Simple_Select > "" And (RS_ID = 0 Or P_ZDISPO.Show_SimpleSelect = True) Then
                        Simple_Select_Frm.P.SQL_WHERE = SQL_WHERE

                        OUT = ShowDialogForm(Simple_Select_Frm) = DialogResult.OK

                        If OUT Then
                            RS_ID = Simple_Select_Frm.Out_Params.RS_ID
                            P_ZDISPO_OUT.RS = Simple_Select_Frm.Out_Params
                            SQL_WHERE = Replace(SQL_WHERE, "@RS_ID", RS_ID)
                            P_ZDISPO.RS_Type = Replace(P_ZDISPO.RS_Type, "@RS_ID", RS_ID)
                        End If
                    End If

                    If OUT Then
                        If Trim(DispoDEF.StoredProc_2) > "" Then
                            Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

                            DC.Qdf_set_SP(sqlComm, DispoDEF.StoredProc_2, 0)
                            DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal) 'Azert kell, mert ha se simple_select, se dofilter nincs megadva, akkor csak ebbol lehet megtudni a terminal-t
                            DC.Qdf_AddParameter(sqlComm, "@ProcessID", SqlDbType.Int, , , , , Curr_DoFilter_Params.ProcessID)
                            If RS_ID = 0 Then
                                DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, ParameterDirection.Output)
                            Else
                                DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , RS_ID)
                            End If
                            DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                            DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.VarChar, ParameterDirection.Output, 255)
                            DC.Qdf_AddParameter(sqlComm, "@ErrParams", SqlDbType.VarChar, ParameterDirection.Output, 255)

                            CURSOR_SHOW_WAIT()
                            Try
                                OUT = DC.Qdf_Execute(P_ZDISPO.ModuleIdentifier, sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrParams")

                                If OUT = True Then
                                    If RS_ID = 0 Then
                                        RS_ID = nz(sqlComm.Parameters("@RS_ID").Value, 0)
                                    End If
                                End If

                            Catch ex As Exception
                                OUT = False
                                DoErrorMsgBox(P_ZDISPO.Parent_FPf, "FP_App.ZDISPO", Err.Number, Err.Description)
                            End Try

                            CURSOR_SHOW_DEFAULT()
                        End If
                    End If

                    If OUT Then
                        If DispoDEF.Txt_export > "" Then
                            Dim p As String() = Split(DispoDEF.Txt_export, "#ENC#")

                            If UBound(p) < 1 Then
                                ReDim Preserve p(1)
                            End If

                            Dim Txt_Export As String = Text_Replace_Standard_Params(p(0))
                            Dim Txt_Encoding As String = ""
                            Dim Encoding As System.Text.Encoding = Nothing

                            If p.Length = 2 Then
                                Txt_Encoding = Trim(p(1))
                            End If

                            Txt_Export = Replace(Txt_Export, "@RS_ID", RS_ID)   'Mert a Text_Replace_Standard_Params ezt a cseret nem teszi meg ebben az esetben.

                            If Tbl_IsRecordsetNotEmpty(Txt_Export, False) Then
                                If DispoDEF.FilePath = "" Then
                                    DispoDEF.FilePath = Windows_FolderOpenDialogBox(, , "DISPO_TXT_EXPORT_" + DispoDEF.Identifier)
                                End If

                                If Directory.Exists(DispoDEF.FilePath) Then
                                    Select Case Txt_Encoding
                                        Case ""
                                            Encoding = System.Text.Encoding.Default

                                        Case "ANSI"
                                            Encoding = System.Text.Encoding.Default

                                        Case "UTF-7"
                                            Encoding = System.Text.Encoding.UTF7

                                        Case "UTF-8"
                                            Encoding = System.Text.Encoding.UTF8

                                        Case "UTF-16"
                                            Encoding = System.Text.Encoding.Unicode

                                        Case "ASCII"
                                            Encoding = System.Text.Encoding.ASCII

                                        Case "UNICODE"
                                            Encoding = System.Text.Encoding.Default

                                        Case Else
                                            OUT = False
                                            MsgBox("Invalid encoding type!!!")
                                    End Select

                                    If OUT = True Then
                                        If Not SendDatalineToTxtFormat(Txt_Export, DispoDEF.FilePath, Encoding) Then
                                            OUT = False
                                        End If
                                    End If
                                Else
                                    OUT = False
                                End If
                            End If
                        End If
                    End If

                    If OUT Then
                        If Trim(DispoDEF.StoredProc_3) > "" Then
                            Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

                            DC.Qdf_set_SP(sqlComm, DispoDEF.StoredProc_3, 0)
                            DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal) 'Azert kell, mert ha se simple_select, se dofilter nincs megadva, akkor csak ebbol lehet megtudni a terminal-t
                            DC.Qdf_AddParameter(sqlComm, "@ProcessID", SqlDbType.Int, , , , , Curr_DoFilter_Params.ProcessID)
                            If RS_ID = 0 Then
                                DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, ParameterDirection.Output)
                            Else
                                DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , RS_ID)
                            End If
                            DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                            DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.VarChar, ParameterDirection.Output, 255)
                            DC.Qdf_AddParameter(sqlComm, "@ErrParams", SqlDbType.VarChar, ParameterDirection.Output, 255)

                            CURSOR_SHOW_WAIT()
                            Try
                                OUT = DC.Qdf_Execute(P_ZDISPO.ModuleIdentifier, sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrParams")
                                If RS_ID = 0 Then
                                    RS_ID = nz(sqlComm.Parameters("@RS_ID").Value, 0)
                                End If
                            Catch ex As Exception
                                OUT = False
                                DoErrorMsgBox(P_ZDISPO.Parent_FPf, "FP_App.ZDISPO", Err.Number, Err.Description)
                            End Try

                            CURSOR_SHOW_DEFAULT()
                        End If
                    End If

                    If OUT Then
                        If DispoDEF.Report_Name > "" Then
                            If DispoDEF.Report_Name = "PROCESS_START" Then
                                Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

                                DC.Qdf_set_SP(sqlComm, DispoDEF.Report_SQL, 0)
                                DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal) 'Azert kell, mert ha se simple_select, se dofilter nincs megadva, akkor csak ebbol lehet megtudni a terminal-t
                                DC.Qdf_AddParameter(sqlComm, "@ProcessID", SqlDbType.Int, , , , , Curr_DoFilter_Params.ProcessID)
                                If RS_ID = 0 Then
                                    DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, ParameterDirection.Output)
                                Else
                                    DC.Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , RS_ID)
                                End If

                                DC.Qdf_AddParameter(sqlComm, "@OUT_PARAM_1", SqlDbType.NVarChar, ParameterDirection.Output, -1)
                                DC.Qdf_AddParameter(sqlComm, "@OUT_PARAM_2", SqlDbType.NVarChar, ParameterDirection.Output, -1)
                                DC.Qdf_AddParameter(sqlComm, "@OUT_PARAM_3", SqlDbType.NVarChar, ParameterDirection.Output, -1)

                                DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                                DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.VarChar, ParameterDirection.Output, 255)
                                DC.Qdf_AddParameter(sqlComm, "@ErrParams", SqlDbType.VarChar, ParameterDirection.Output, 255)

                                CURSOR_SHOW_WAIT()
                                Try
                                    OUT = DC.Qdf_Execute(P_ZDISPO.ModuleIdentifier, sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrParams")
                                    If OUT = True Then
                                        If RS_ID = 0 Then
                                            RS_ID = nz(sqlComm.Parameters("@RS_ID").Value, 0)
                                        End If

                                        Process.Start(sqlComm.Parameters("@OUT_PARAM_1").Value)
                                    End If

                                Catch ex As Exception
                                    OUT = False
                                    DoErrorMsgBox(P_ZDISPO.Parent_FPf, "FP_App.ZDISPO", Err.Number, Err.Description)
                                End Try

                                CURSOR_SHOW_DEFAULT()

                            ElseIf (DispoDEF.Report_Name.ToUpper.IndexOf(".DOC") > 0) Or (DispoDEF.Report_Name.ToUpper.IndexOf(".DOCX") > 0) Then
                                Dim Prepared As Boolean = False
                                Simple_Select_Frm.FP.PRINT_BY_IDENTIFIER(P_ZDISPO.Identifier, Prepared, False, Simple_Select_Frm.Out_Params.Selected_ID, DispoDEF.Report_Name, ENUM_ReportType.WORD)
                            Else
                                Dim MyPrintForm As New FP_Print(Me, DispoDEF.Report_Name)

                                DispoDEF.Report_SQL = Replace(DispoDEF.Report_SQL, "@RS_ID", RS_ID)

                                If DOFILTER_Opened Then
                                    DispoDEF.Report_SQL = Replace(DispoDEF.Report_SQL, "@FILTERTEXT", SQLStr(Curr_DoFilter_Params.FilterText))
                                    Curr_DoFilter_Params = DOFILTER_get_Params_from_SERVER(Curr_DoFilter_Params.ProcessID)
                                    DispoDEF.Report_SQL = DOFILTER_Replace_Params_In_String(DispoDEF.Report_SQL, Curr_DoFilter_Params)
                                End If
                                DispoDEF.Report_SQL = Text_ParametersErsetzen(DispoDEF.Report_SQL, "")


                                Dim Report_RS() As String = Split(DispoDEF.Report_SQL, vbCrLf)
                                Dim DT() As DataTable
                                Dim SubreportName As String = ""

                                For i As Integer = 0 To UBound(Report_RS)
                                    If Trim(Report_RS(i)) = "" Then
                                        SubreportName = ""
                                    Else
                                        If Trim(Report_RS(i)).IndexOf("SUBREPORT=") <> -1 Then
                                            SubreportName = Trim(Report_RS(i)).Replace("SUBREPORT=", "")
                                        Else
                                            ReDim Preserve DT(i)
                                            DC.Qdf_Fill_DT(Report_RS(i), DT(i))

                                            If SubreportName <> "" Then
                                                MyPrintForm.Add_Subreport(SubreportName, DT(i))
                                            Else
                                                MyPrintForm.DATASOURCES_ADD(DT(i))
                                            End If
                                        End If
                                    End If
                                Next

                                Select Case DispoDEF.Report_OpenType
                                    Case 0
                                        MyPrintForm.Show()
                                    Case 2
                                        MyPrintForm.Show()
                                    Case Else
                                        DoErrorMsgBox("FP_App.ZDISPO", 0, String.Format("Unknown Report_OpenType '{0}'", DispoDEF.Report_OpenType))
                                End Select
                            End If
                        End If
                    End If

                    If OUT Then
                        If DispoDEF.Excel_export > "" Then
                            Dim MySQL As String = Trim(Text_ParametersErsetzen(DispoDEF.Excel_export.Replace("@RS_ID", RS_ID), ""))
                            MySQL = Replace(MySQL, "#_PROCESS_ID_#", Curr_DoFilter_Params.ProcessID.ToString)
                            Dim MySQL_Is_ViewName As Boolean = True
                            Dim MySQL_ObjectType As Integer
                            Dim DoExcelExport As Boolean = True

                            If InStr(MySQL, " ") > 0 Then
                                MySQL_Is_ViewName = False
                            End If

                            If InStr(MySQL, vbTab) > 0 Then
                                MySQL_Is_ViewName = False
                            End If

                            If MySQL_Is_ViewName = True Then
                                Dim DT_Of_Fields As DataTable = Nothing

                                MySQL_ObjectType = ENUM_ServerObject_Type.None
                                If Not RS_GET_FIELDPROPERTIES_FROM_SERVER_OBJECT(MySQL, DT_Of_Fields, MySQL_ObjectType) Then
                                    DoExcelExport = False
                                End If
                                If DoExcelExport Then
                                    Dim ObjectWithRS_ID As Boolean = False

                                    If DT_Of_Fields.Select("FieldName = 'RS_ID'").Count = 1 Then
                                        ObjectWithRS_ID = True
                                    End If

                                    Dim SELECT_List As String = ""
                                    Dim MyComma As String = ""

                                    For i As Integer = 0 To DT_Of_Fields.Rows.Count - 1
                                        Dim CurrFieldName As String = DT_Of_Fields.Rows(i)!FieldName
                                        If CurrFieldName.ToUpper <> "TERMINAL" And CurrFieldName.ToUpper <> "RS_ID" Then
                                            SELECT_List += MyComma + CurrFieldName
                                            MyComma = ", "
                                        End If
                                    Next

                                    Select Case MySQL_ObjectType
                                        Case ENUM_ServerObject_Type.V
                                            If ObjectWithRS_ID Then
                                                MySQL = String.Format("SELECT {0} FROM {1} WHERE RS_ID = {2}", SELECT_List, MySQL, RS_ID)
                                            Else
                                                MySQL = String.Format("SELECT {0} FROM {1} WHERE Terminal = '{2}'", SELECT_List, MySQL, Terminal)
                                            End If

                                        Case ENUM_ServerObject_Type.TF
                                            'If InStr(MySQL, ".") = 0 Then
                                            '    MySQL = "dbo." + MySQL
                                            'End If

                                            If ObjectWithRS_ID Then
                                                MySQL = String.Format("SELECT * FROM dbo.{0}({1})", MySQL, RS_ID)
                                            Else
                                                MySQL = String.Format("SELECT * FROM dbo.{0}('{1}')", MySQL, Terminal)
                                            End If

                                        Case Else
                                            DoExcelExport = False
                                            DoErrorMsgBox("FP_App.ZDISPO", 0, String.Format("Unknown type of Excel_export object '{0}'", MySQL))
                                    End Select
                                End If
                            End If

                            If DoExcelExport Then
                                Dim DT_ExcelData As DataTable = Nothing

                                CURSOR_SHOW_WAIT()
                                If DC.Qdf_Fill_DT(MySQL, DT_ExcelData, 0) Then
                                    Dim XLS_Export As New FP_XLS_Export(Me, P_ZDISPO.Parent_FPf, DT_ExcelData, P_ZDISPO.Identifier)

                                    XLS_Export.ExportData()
                                End If
                                CURSOR_SHOW_DEFAULT()
                            End If
                        End If
                    End If

                    If OUT Then
                        If DispoDEF.Next_Identifier > "" Then
                            Dim P_NEXT_ZDISPO As Struct_ZDISPO_Params

                            With P_NEXT_ZDISPO
                                .Identifier = DispoDEF.Next_Identifier
                                .Current_ID = P_ZDISPO.Current_ID
                                .Current_RS_ID = P_ZDISPO.RS_ID
                                .ModuleIdentifier = P_ZDISPO.ModuleIdentifier
                                .NoMessageForNoRecord = P_ZDISPO.NoMessageForNoRecord
                                .Parent_FPf = P_ZDISPO.Parent_FPf
                                .PressOK = P_ZDISPO.PressOK
                                .RS_ID = RS_ID
                                .RS_Type = P_ZDISPO.RS_Type
                                .Selected = P_ZDISPO.Selected
                                .Show_SimpleSelect = P_ZDISPO.Show_SimpleSelect
                                .SQL_WHERE = P_ZDISPO.SQL_WHERE
                            End With

                            OUT = ZDISPO(P_NEXT_ZDISPO, P_ZDISPO_OUT)
                        End If
                    End If
                End If
            End If
            '+++End If
        End If

        ZDISPO = OUT
    End Function

    Public Sub ZDISPO_FPApp_MenuItem_Activated(ByVal sender As FP_MenuItem.Struct_FP_MenuItem_Params, ByRef Handled As Boolean)
        'Handles: "DISPODEF|<Identifier>"

        If Handled = False Then
            If sender.Action = "DISPODEF" Then
                Handled = True

                Dim P As New Struct_ZDISPO_Params

                With P
                    .Identifier = sender.OpenArgs

                End With

                ZDISPO(P, Nothing)
            End If
        End If
    End Sub

    Public Function DOFILTER(ByVal FPf As FP_Form, ByVal FilterName As String, Optional ByVal MyWhereQuery As String = "", Optional ByVal FilterText_Label As Label = Nothing, Optional ByVal NewRecordSHOW As Boolean = True, Optional ByVal SubWhere As String = "") As Boolean
        Dim Filter As New FP_DoFilter(Me, FilterName, MyWhereQuery, NewRecordSHOW)
        Using Filter
            Filter.SubFilter = SubWhere

            ShowDialogForm(Filter, FPf)

            If gl_Doit Then
                gl_FilterParams = Filter.P

                If Not (FilterText_Label Is Nothing) Then
                    FilterText_Label.Text = Filter.P.FilterText
                End If
            End If
        End Using

        DOFILTER = gl_Doit
    End Function

    Function DOFILTER_get_Params_from_SERVER(Process_ID As Long) As Struct_DoFilter_gl_Params
        Dim OUT As New Struct_DoFilter_gl_Params

        DoFilter_Params_CLEAR(OUT, False)

        Dim MySQL As String = String.Format("SELECT	FilterWHERE, FilterTEXT, ParamInt00, ParamInt01, ParamInt02, ParamInt03, ParamInt04, ParamInt05, ParamInt06, ParamInt07, ParamInt08, ParamInt09, ParamInt10, ParamDbl00, ParamDbl01, ParamDbl02, ParamDbl03, ParamDbl04, ParamDbl05, ParamDbl06, ParamDbl07, ParamDbl08, ParamDbl09, ParamDbl10, ParamStr00, ParamStr01, ParamStr02, ParamStr03, ParamStr04, ParamStr05, ParamStr06, ParamStr07, ParamStr08, ParamStr09, ParamStr10, ParamDate00, ParamDate01, ParamDate02, ParamDate03, ParamDate04, ParamDate05, ParamDate06, ParamDate07, ParamDate08, ParamDate09, ParamDate10 FROM DoFilter_Params WHERE ProcessID = {0}", Process_ID)
        Dim DRow As DataRow = DC.Qdf_get_DataRow(MySQL)

        If Not (DRow Is Nothing) Then
            With OUT
                .ProcessID = Process_ID
                .FilterWHERE = nz(DRow!FilterWHERE, "")
                .FilterText = nz(DRow!FilterText, "")
                .ParamInt(0) = nz(DRow!ParamInt00, 0)
                .ParamInt(1) = nz(DRow!ParamInt01, 0)
                .ParamInt(2) = nz(DRow!ParamInt02, 0)
                .ParamInt(3) = nz(DRow!ParamInt03, 0)
                .ParamInt(4) = nz(DRow!ParamInt04, 0)
                .ParamInt(5) = nz(DRow!ParamInt05, 0)
                .ParamInt(6) = nz(DRow!ParamInt06, 0)
                .ParamInt(7) = nz(DRow!ParamInt07, 0)
                .ParamInt(8) = nz(DRow!ParamInt08, 0)
                .ParamInt(9) = nz(DRow!ParamInt09, 0)
                .ParamInt(10) = nz(DRow!ParamInt10, 0)
                .ParamDbl(0) = nz(DRow!ParamDbl00, 0)
                .ParamDbl(1) = nz(DRow!ParamDbl01, 0)
                .ParamDbl(2) = nz(DRow!ParamDbl02, 0)
                .ParamDbl(3) = nz(DRow!ParamDbl03, 0)
                .ParamDbl(4) = nz(DRow!ParamDbl04, 0)
                .ParamDbl(5) = nz(DRow!ParamDbl05, 0)
                .ParamDbl(6) = nz(DRow!ParamDbl06, 0)
                .ParamDbl(7) = nz(DRow!ParamDbl07, 0)
                .ParamDbl(8) = nz(DRow!ParamDbl08, 0)
                .ParamDbl(9) = nz(DRow!ParamDbl09, 0)
                .ParamDbl(10) = nz(DRow!ParamDbl10, 0)
                .ParamStr(0) = nz(DRow!ParamStr00, "")
                .ParamStr(1) = nz(DRow!ParamStr01, "")
                .ParamStr(2) = nz(DRow!ParamStr02, "")
                .ParamStr(3) = nz(DRow!ParamStr03, "")
                .ParamStr(4) = nz(DRow!ParamStr04, "")
                .ParamStr(5) = nz(DRow!ParamStr05, "")
                .ParamStr(6) = nz(DRow!ParamStr06, "")
                .ParamStr(7) = nz(DRow!ParamStr07, "")
                .ParamStr(8) = nz(DRow!ParamStr08, "")
                .ParamStr(9) = nz(DRow!ParamStr09, "")
                .ParamStr(10) = nz(DRow!ParamStr10, "")
                .ParamDate(0) = nz(DRow!ParamDate00, NULLDATE)
                .ParamDate(1) = nz(DRow!ParamDate01, NULLDATE)
                .ParamDate(2) = nz(DRow!ParamDate02, NULLDATE)
                .ParamDate(3) = nz(DRow!ParamDate03, NULLDATE)
                .ParamDate(4) = nz(DRow!ParamDate04, NULLDATE)
                .ParamDate(5) = nz(DRow!ParamDate05, NULLDATE)
                .ParamDate(6) = nz(DRow!ParamDate06, NULLDATE)
                .ParamDate(7) = nz(DRow!ParamDate07, NULLDATE)
                .ParamDate(8) = nz(DRow!ParamDate08, NULLDATE)
                .ParamDate(9) = nz(DRow!ParamDate09, NULLDATE)
                .ParamDate(10) = nz(DRow!ParamDate10, NULLDATE)
            End With
        End If

        Return OUT
    End Function

    Function DOFILTER_getProcessID(ByRef P As Struct_DoFilter_gl_Params) As Boolean
        ' Beir egy Process-t a DoFilter_Params tablaba es visszaadja annak ProzessID-jet.

        Dim OUT As Boolean = True

        P.ProcessID = 0

        If InitGlobals() Then
            Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()
            Using sqlComm
                DC.Qdf_set_SP(sqlComm, "DoFilter_Process_Prepare")
                DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)

                DC.Qdf_AddParameter(sqlComm, "@FilterWHERE", SqlDbType.NVarChar, , 4000, P.FilterWHERE)
                DC.Qdf_AddParameter(sqlComm, "@FilterTEXT", SqlDbType.NVarChar, , 4000, P.FilterText)

                If P.ParamInt Is Nothing Then
                    ReDim P.ParamInt(10)
                End If
                If P.ParamDbl Is Nothing Then
                    ReDim P.ParamDbl(10)
                End If
                If P.ParamStr Is Nothing Then
                    ReDim P.ParamStr(10)
                End If
                If P.ParamDate Is Nothing Then
                    ReDim P.ParamDate(10)
                End If

                If UBound(P.ParamInt) < 10 Then
                    ReDim Preserve P.ParamInt(10)
                End If
                If UBound(P.ParamDbl) < 10 Then
                    ReDim Preserve P.ParamDbl(10)
                End If
                If UBound(P.ParamStr) < 10 Then
                    ReDim Preserve P.ParamStr(10)
                End If
                If UBound(P.ParamDate) < 10 Then
                    ReDim Preserve P.ParamDate(10)
                End If

                For i As Integer = 0 To 10
                    DC.Qdf_AddParameter(sqlComm, "@ParamInt" + Format(i, "00"), SqlDbType.Int, , , , , P.ParamInt(i))
                Next i
                For i As Integer = 0 To 10
                    DC.Qdf_AddParameter(sqlComm, "@ParamDbl" + Format(i, "00"), SqlDbType.Float, , , , , , P.ParamDbl(i))
                Next i
                For i As Integer = 0 To 10
                    DC.Qdf_AddParameter(sqlComm, "@ParamStr" + Format(i, "00"), SqlDbType.NVarChar, , 4000, P.ParamStr(i))
                Next i
                For i As Integer = 0 To 10
                    If P.ParamDate(i) = NULLDATE Then
                        DC.Qdf_AddParameter(sqlComm, "@ParamDate" + Format(i, "00"), SqlDbType.DateTime, , , "NULL", NULLDATE)
                    Else
                        DC.Qdf_AddParameter(sqlComm, "@ParamDate" + Format(i, "00"), SqlDbType.DateTime, , , , P.ParamDate(i))
                    End If
                Next i

                CURSOR_SHOW_WAIT()
                Try
                    DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG)
                    P.ProcessID = sqlComm.Parameters("@RetValue").Value
                Catch ex As Exception
                    OUT = False
                    DoErrorMsgBox("FP_App.DOFILTER_getProcessID", Err.Number, Err.Description)
                End Try
                CURSOR_SHOW_DEFAULT()
            End Using
            sqlComm.Dispose()
        End If

        DOFILTER_getProcessID = OUT

    End Function

    Public Function DOFILTER_Replace_Params_In_String(MyText As String, FilterParams As Struct_DoFilter_gl_Params) As String
        Dim OUT As String = MyText

        With FilterParams
            OUT = Replace(OUT, "#_PROCESS_ID_#", FilterParams.ProcessID)
            OUT = Replace(OUT, "#_FILTERTEXT_#", FilterParams.FilterText)
            OUT = Replace(OUT, "#_FILTERWHERE_#", FilterParams.FilterWHERE)

            Dim FilterWhere_with_WHERE As String = Trim(FilterParams.FilterWHERE)
            If FilterWhere_with_WHERE > "" Then
                FilterWhere_with_WHERE = " WHERE " + FilterWhere_with_WHERE
            End If

            OUT = Replace(OUT, "#_FILTERWHERE_WITH_WHERE_#", FilterWhere_with_WHERE)

            For i As Integer = 0 To UBound(FilterParams.ParamInt)
                Dim ParamNum_STR As String = Right("0" + i.ToString, 2)

                OUT = Replace(OUT, "#_PARAMINT" + ParamNum_STR + "_#", .ParamInt(i))
                OUT = Replace(OUT, "#_PARAMDBL" + ParamNum_STR + "_#", DBFORMAT_from_OBJECT(.ParamDbl(i), "", "FLOAT"))
                OUT = Replace(OUT, "#_PARAMSTR" + ParamNum_STR + "_#", .ParamStr(i))
                OUT = Replace(OUT, "#_PARAMSTR" + ParamNum_STR + "_#", SQLDate(.ParamDate(i)))
            Next
        End With

        Return OUT
    End Function

    Sub New(ByVal MyStartForm As Form, Optional ByVal MyParent As FP_App = Nothing)
        StartForm = MyStartForm
        Parent = MyParent
        INIT()
    End Sub

    Sub New()
        INIT()
    End Sub

    Sub New(Different_INIFileName As String)
        INIFileName = Different_INIFileName
        INIT()
    End Sub

    Private Sub INIT()
        PFD_LOAD_FROM_INI()
        Files_EMF_Remove()
        SKIN_ADD("SEL_SKIN")
        InitLanguage()
        DC = New FP_DataConnect(Me)
    End Sub

    Public Function UPDATE_INSTANCE_GET(AktInstance As Integer, ByRef OUT_UPDATE_Method As ENUM_UPDATE_METHOD, ByRef OUT_UPDATE_Just_Finished As Boolean) As Integer
        Dim OUT As Integer
        Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

        OUT_UPDATE_Method = ENUM_UPDATE_METHOD.MANUALY
        OUT_UPDATE_Just_Finished = False

        With DC
            .Qdf_set_SP(sqlComm, "SEL_SYS_UPDATES_GET_INSTANCE")
            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            .Qdf_AddParameter(sqlComm, "@Current_Instance", SqlDbType.Int, , , , , AktInstance)

            .Qdf_AddParameter(sqlComm, "@OUT_Instance", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_Update_Method", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_Update_Status", SqlDbType.Int, ParameterDirection.Output)
        End With

        Dim Result As Boolean = False

        CURSOR_SHOW_WAIT()
        Try
            Result = DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
        Catch ex As Exception
            Result = False
            DoErrorMsgBox("UPDATE_INSTANCE_GET", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        If Result Then
            OUT = sqlComm.Parameters("@OUT_Instance").Value
            OUT_UPDATE_Method = sqlComm.Parameters("@OUT_Update_Method").Value
            OUT_UPDATE_Just_Finished = (sqlComm.Parameters("@OUT_Update_Status").Value = 2)
        End If

        Return OUT
    End Function

    Function GET_SERVER_CURRENT_DATE(Optional ByVal With_Time As Boolean = False) As DateTime
        Dim OUT As DateTime = NULLDATE

        Static Last_Terminal_Date As DateTime = NULLDATE
        Static Last_OUT As DateTime = NULLDATE
        Dim DoIt As Boolean = True

        If With_Time = False Then
            If Last_Terminal_Date <> NULLDATE Then
                If DateDiff(DateInterval.Minute, Now, Last_Terminal_Date) < 60 And DateDiff(DateInterval.Minute, Now, Last_Terminal_Date) >= 0 Then
                    OUT = Last_OUT
                    DoIt = False
                End If
            End If
        End If

        If DoIt Then
            Dim DRow As DataRow
            Dim SQL As String = "SELECT GETDATE() CurrentDate"

            DRow = DC.Qdf_get_DataRow(SQL)

            If Not (DRow Is Nothing) Then
                OUT = DRow!CurrentDate

                If With_Time = False Then
                    OUT = DateSerial(OUT.Year, OUT.Month, OUT.Day)
                End If
            End If

            Last_Terminal_Date = Now
            Last_OUT = DateSerial(OUT.Year, OUT.Month, OUT.Day)
        End If

        GET_SERVER_CURRENT_DATE = OUT
    End Function

    Function GET_SERVER_TIMESTAMP() As String
        Dim OUT As String = Format(GET_SERVER_CURRENT_DATE(True), "yyMMdd_HHmmss")

        Return OUT
    End Function

    Public Function GetFZIDfromResztvevoID(ByVal MyResztvevoID As Integer, ByVal ParametersFrom_ID As Integer) As Integer
        Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

        With DC
            .Qdf_set_SP(sqlComm, "gl_getFZIDfromResztvevoID")
            .Qdf_AddParameter(sqlComm, "@PartID", SqlDbType.Int, , , , , MyResztvevoID)
            .Qdf_AddParameter(sqlComm, "@ParametersFrom_ID", SqlDbType.Int, , , , , ParametersFrom_ID)
            .Qdf_AddParameter(sqlComm, "@ErgFZID", SqlDbType.Int, ParameterDirection.Output)
        End With

        CURSOR_SHOW_WAIT()
        Try
            DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG)
        Catch ex As Exception
            GetFZIDfromResztvevoID = 0
            DoErrorMsgBox("FP_App.getFZIDfromResztvevoID", Err.Number, Err.Description)
            Exit Function
        End Try

        GetFZIDfromResztvevoID = sqlComm.Parameters("@ErgFZID").Value

        CURSOR_SHOW_DEFAULT()
    End Function
    Public Function COMBOBOX_FIND(ByRef c As ComboBox, ByRef dt As DataTable, ByVal ColumnName As String, ByVal FindValue As String, Optional ByVal SelectNothingWhenNotFound As Boolean = True) As Boolean
        Dim OUT As Boolean = False
        Dim gl_Data_Binded_OLD As Boolean = gl_Data_Binded

        gl_Data_Binded = False

        FindValue = UCase(FindValue)

        If dt Is Nothing Then
            c.SelectedIndex = -1
        Else
            If Not dt.Columns.Contains(ColumnName) Then
                DoErrorMsgBox("FP_App.COMBOBOX_FIND", 0, String.Format("dt does not contains Column '{0}'", ColumnName))
            Else
                Dim Criteria As String = ""

                Select Case dt.Columns(ColumnName).DataType.ToString
                    Case "System.String"
                        Criteria = String.Format("{0} = '{1}'", ColumnName, SQLStr(FindValue))

                    Case "System.Int32", "System.Int16"
                        Criteria = String.Format("{0} = {1}", ColumnName, Val(FindValue))

                    Case Else
                        DoErrorMsgBox("FP_App.COMBOBOX_FIND", 0, String.Format("Column '{0}' has an unknown datatype", ColumnName))
                End Select

                If Criteria > "" Then
                    If dt.Select(Criteria).Count < 1 Then
                        If SelectNothingWhenNotFound Then
                            c.SelectedIndex = -1
                        End If
                    Else
                        Dim Row As DataRow = dt.Select(Criteria).First

                        c.SelectedValue = Row.Item(c.ValueMember)
                        If nz(c.DisplayMember, "") = "" Then
                            c.Text = ""
                        Else
                            c.Text = Row.Item(c.DisplayMember)
                        End If
                        OUT = True
                    End If
                End If
            End If
        End If

        gl_Data_Binded = gl_Data_Binded_OLD

        COMBOBOX_FIND = OUT
    End Function
    Public Function COMBOBOX_FIND(ByVal c As ComboBox, ByVal FindValue As String) As Boolean
        Dim OUT As Boolean = False

        Dim iFoundIndex As Integer

        If c.SelectedIndex = -1 And FindValue = "" Then
            OUT = True
        Else
            iFoundIndex = c.FindStringExact(FindValue)
            If iFoundIndex >= 0 Then
                If iFoundIndex <> c.SelectedIndex Then
                    c.SelectedIndex = iFoundIndex
                End If

                If iFoundIndex = c.SelectedIndex Then
                    OUT = True
                End If
            End If
        End If

        COMBOBOX_FIND = OUT
    End Function
    Public Sub COMBOBOX_FIND(ByVal c As ComboBox, ByRef e As System.Windows.Forms.KeyPressEventArgs)
        'Ezt a rutint a ComboBox KeyPress esemenyebe tedd bele.

        Dim TextToFind As String = Mid(c.Text, 1, c.SelectionStart) + e.KeyChar
        Dim FoundIndex As Integer = c.FindString(TextToFind)

        If FoundIndex >= 0 Then
            c.SelectedIndex = FoundIndex
            c.SelectionStart = Len(TextToFind)
            c.SelectionLength = Len(c.Text) - c.SelectionStart
            e.Handled = True
        End If
    End Sub

    Public Function COMBOBOX_INIT_FROM_DT(c As ComboBox, ByVal ValueMember As String, ByVal DisplayMember As String, DT As DataTable, Optional SQL_WHERE_FOR_LIST As String = "") As Boolean
        Dim OUT As Boolean = False
        Dim DT_FOR_LIST As DataTable = Nothing

        Try
            If Trim(SQL_WHERE_FOR_LIST) = "" Then
                DT_FOR_LIST = DT.Copy
            Else
                DT_FOR_LIST = DT.Select(SQL_WHERE_FOR_LIST).CopyToDataTable
            End If
            With c
                .DataSource = DT_FOR_LIST
                .ValueMember = ValueMember
                .DisplayMember = DisplayMember
                'If .Items.Count > 0 Then
                If DT_FOR_LIST.Rows.Count > 0 Then
                    .SelectedIndex = 0
                End If
            End With

            OUT = True

        Catch ex As Exception
            DoErrorMsgBox("FP_App.COMBOBOX_INIT_FROM_DT", Err.Number, Err.Description)
        End Try

        Return OUT
    End Function

    Public Function COMBOBOX_INIT(ByRef COMBO_P As STRUCT_COMBOBOX_PARAMS) As Boolean
        Dim OUT As Boolean = False
        Dim SQL As String = SQL_getFROM_ELEMENTS(COMBO_P.SQL_SELECT_0, COMBO_P.SQL_SELECT, COMBO_P.SQL_FROM, COMBO_P.SQL_WHERE, COMBO_P.SQL_GROUPBY, COMBO_P.SQL_ORDERBY)
        Dim DT_FOR_LIST As DataTable = Nothing

        OUT = DC.Qdf_Fill_DT(SQL, COMBO_P.DT)

        If OUT = False Then
            If COMBO_P.c Is Nothing Then
                DoErrorMsgBox("FP_App.COMBOBOX_INIT (MyComboBox = Nothing", Err.Number, Err.Description)
            Else
                DoErrorMsgBox(String.Format("FP_App.COMBOBOX_INIT (MyComboBox = '{0}')", COMBO_P.c.Name), Err.Number, Err.Description)
            End If
        Else

            'OUT = COMBOBOX_INIT(COMBO_P.c, COMBO_P.DT, COMBO_P.ValueMember, COMBO_P.DisplayMember)

            Try
                If Trim(nz(COMBO_P.SQL_WHERE_FOR_LIST, "")) = "" Then
                    DT_FOR_LIST = COMBO_P.DT
                Else
                    DT_FOR_LIST = COMBO_P.DT.Select(COMBO_P.SQL_WHERE_FOR_LIST).CopyToDataTable
                End If
                With COMBO_P.c
                    .DataSource = DT_FOR_LIST
                    '.DataSource = COMBO_P.DT
                    .ValueMember = COMBO_P.ValueMember
                    .DisplayMember = COMBO_P.DisplayMember
                    'If DT.Rows.Count > 0 Then
                    'If .Items.Count > 0 Then
                    If DT_FOR_LIST.Rows.Count > 0 Then
                        .SelectedIndex = 0
                    End If
                End With

                OUT = True

            Catch ex As Exception
                DoErrorMsgBox("FP_App.COMBOBOX_INIT", Err.Number, Err.Description)
            End Try
        End If

        Return OUT
    End Function

    Public Function COMBOBOX_INIT_FROM_FIXTEXT(ByVal FPc As FP_Control, ByVal FixText_Key As String, ByRef DT As DataTable, Optional ByVal WHERE2 As String = "") As Boolean
        Dim OUT As Boolean = False
        Dim FixText As String = getFixText(FixText_Key)
        Dim Params_DIC As New Dictionary(Of String, String)

        If FixText > "" Then
            If FIXTEXT_SPLIT_PARAMS(FixText, Params_DIC) Then
                Dim SQL_SELECT_0 As String = FIXTEXT_getParam("SELECT_0", Params_DIC)
                Dim SQL_SELECT As String = FIXTEXT_getParam("SELECT", Params_DIC)
                Dim SQL_FROM As String = FIXTEXT_getParam("FROM", Params_DIC)
                Dim SQL_WHERE As String = FIXTEXT_getParam("WHERE", Params_DIC)
                Dim SQL_WHERE_FOR_LIST As String = FIXTEXT_getParam("WHERE_FOR_LIST", Params_DIC)
                Dim SQL_GROUPBY As String = FIXTEXT_getParam("GROUPBY", Params_DIC)
                Dim SQL_ORDERBY As String = FIXTEXT_getParam("ORDERBY", Params_DIC)
                Dim SQL_VALUEMEMBER As String = FIXTEXT_getParam("VALUEMEMBER", Params_DIC)
                Dim SQL_DISPLAYMEMBER As String = FIXTEXT_getParam("DISPLAYMEMBER", Params_DIC)
                Dim REFRESH_TYPE_STR As String = FIXTEXT_getParam("REFRESH", Params_DIC)

                If WHERE2 > "" Then
                    SQL_WHERE = IIf(SQL_WHERE > "", String.Format("({0}) And ({1})", SQL_WHERE, WHERE2), WHERE2)
                End If

                If Not (FPc Is Nothing) Then
                    Select Case REFRESH_TYPE_STR
                        Case "" : FPc.Refresh_Type = ENUM_FP_CONTROL_REFRESH_TYPE.Normal
                        Case "NORMAL" : FPc.Refresh_Type = ENUM_FP_CONTROL_REFRESH_TYPE.Normal
                        Case "FORM_CURRENT" : FPc.Refresh_Type = ENUM_FP_CONTROL_REFRESH_TYPE.On_Form_Current
                        Case "FORM_AFTERUPDATE" : FPc.Refresh_Type = ENUM_FP_CONTROL_REFRESH_TYPE.On_Form_AfterUpdate
                        Case Else
                            FPc.Refresh_Type = ENUM_FP_CONTROL_REFRESH_TYPE.Normal
                            DoErrorMsgBox("FP_App.COMBOBOX_INIT_FROM_FIXTEXT", 0, String.Format("Unknown REFRESH_TYPE parameter ('{0}')", REFRESH_TYPE_STR))
                    End Select
                End If

                Dim COMBO_PARAMS As New STRUCT_COMBOBOX_PARAMS

                With COMBO_PARAMS
                    .c = FPc.c
                    .SQL_SELECT_0 = SQL_SELECT_0
                    .SQL_SELECT = SQL_SELECT
                    .SQL_FROM = SQL_FROM
                    .SQL_WHERE = SQL_WHERE
                    .SQL_WHERE_FOR_LIST = SQL_WHERE_FOR_LIST
                    .SQL_GROUPBY = SQL_GROUPBY
                    .SQL_ORDERBY = SQL_ORDERBY
                    .DT = DT
                    .ValueMember = SQL_VALUEMEMBER
                    .DisplayMember = SQL_DISPLAYMEMBER
                End With

                OUT = COMBOBOX_INIT(COMBO_PARAMS)

                DT = COMBO_PARAMS.DT
            End If
        End If

        Return OUT
    End Function

    Public Function CONTROLS_GET_Frm_from_c(c As Control) As Form
        Dim OUT As Form = Nothing

        If Not (c Is Nothing) Then
            While Not (TypeOf (c) Is Form)
                c = c.Parent
            End While

            OUT = c
        End If

        Return OUT
    End Function

    Public Sub COMBOBOX_REFRESH_ALL()
        For Each AktKey In Forms.Keys
            Forms(AktKey).COMBOBOX_REFRESH_ALL()
        Next
    End Sub
    Public Sub COMBOBOX_REFRESH_DT_FixText_Key(MyDT_FixText_Key As String)
        For Each AktKey In Forms.Keys
            Forms(AktKey).COMBOBOX_REFRESH_DT_FixText_Key(MyDT_FixText_Key)
        Next
    End Sub

    Public Sub CONTROLS_SPLITCONTAINER_SPLITTER_DISTANCE_LOAD(MySplitContainer As SplitContainer, Frm_Name As String, frm_WindowState As Windows.Forms.FormWindowState)
        If Not (MySplitContainer Is Nothing) Then
            Dim CtrlPressed = My.Computer.Keyboard.CtrlKeyDown

            If CtrlPressed = False Then
                If frm_WindowState = FormWindowState.Maximized Or frm_WindowState = FormWindowState.Normal Then
                    Dim Key As String = String.Format("SP_DIST_{0}_{1}_{2}", Frm_Name, MySplitContainer.Name, CInt(frm_WindowState))
                    Dim Splitter_Proportion_STR As String = ""

                    If PFDlesen(Key, Splitter_Proportion_STR) Then
                        Dim Splitter_Proportion As Integer = Val(Splitter_Proportion_STR)

                        If Splitter_Proportion > 0 Then
                            Dim MySplitter_Distance As Integer = 0

                            Select Case MySplitContainer.Orientation
                                Case Orientation.Horizontal
                                    MySplitContainer.SplitterDistance = (0.0 + MySplitContainer.Height) * (0.0 + Splitter_Proportion) / 10000

                                Case Orientation.Vertical
                                    MySplitContainer.SplitterDistance = (0.0 + MySplitContainer.Width) * (0.0 + Splitter_Proportion) / 10000

                                Case Else
                                    'Nothing to do
                            End Select
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub CONTROLS_SPLITCONTAINER_SPLITTER_DISTANCE_SAVE(MySplitContainer As SplitContainer, Frm_Name As String, frm_WindowState As Windows.Forms.FormWindowState)
        If Not (MySplitContainer Is Nothing) Then
            Dim Splitter_Proportion As Integer = 0

            If frm_WindowState = FormWindowState.Maximized Or frm_WindowState = FormWindowState.Normal Then
                Select Case MySplitContainer.Orientation
                    Case Orientation.Horizontal
                        Splitter_Proportion = (0.0 + MySplitContainer.SplitterDistance) / (0.0 + MySplitContainer.Height) * 10000

                    Case Orientation.Vertical
                        Splitter_Proportion = (0.0 + MySplitContainer.SplitterDistance) / (0.0 + MySplitContainer.Width) * 10000

                    Case Else
                        'Nothing to do
                End Select

                Dim Key As String = String.Format("SP_DIST_{0}_{1}_{2}", Frm_Name, MySplitContainer.Name, CInt(frm_WindowState))

                PFDinsertOrUpdate(Key, Splitter_Proportion.ToString)
            End If
        End If
    End Sub

    Public Function License_Valid() As Boolean
        Dim OUT As Boolean = False

        Dim QuestionsCode As String = ""
        Dim AnswerCode As String = ""
        Dim CountOfUsers As Integer
        Dim CountOfUsers_STR As String = ""
        Dim ValidationsDate As DateTime
        Dim ValidationsDate_STR As String = ""

        ParmLesen("LZ", "FRAGE", 0, QuestionsCode)
        ParmLesen("LZ", "ANTWORT", 0, AnswerCode)
        ParmLesen("LZ", "ANZUSER", 0, CountOfUsers_STR)
        ParmLesen("LZ", "GULTIG", 0, ValidationsDate_STR)

        CountOfUsers = Val(CountOfUsers_STR)
        ValidationsDate = DateSerial(Val(Mid(ValidationsDate_STR, 1, 4)), Val(Mid(ValidationsDate_STR, 5, 2)), Val(Mid(ValidationsDate_STR, 7, 2)))

        If ValidationsDate > DATE2359(GET_SERVER_CURRENT_DATE()) Then
            Dim MySQL As String = String.Format("SELECT SUBSTRING(dbo.FN_Code_IsValid('{0}', '{1}', {2}, {3}, 'X1RTT85VUI'), 4, 1) OUT", QuestionsCode, AnswerCode, CountOfUsers, SQLDate(ValidationsDate))

            Dim DRow As DataRow = DC.Qdf_get_DataRow(MySQL)

            If Not (DRow Is Nothing) Then
                OUT = IIf(DRow!OUT = "1", True, False)
            End If
        End If

        Return OUT
    End Function

    Public Function SCREEN_GET_WorkingArea(Frm As Form) As FP_L_Rect
        Dim OUT As FP_L_Rect = Nothing

        Dim MyScreen As Screen = Screen.FromPoint(New Point(Frm.Left + Frm.Width / 2, Frm.Top + Frm.Height / 2))

        If MyScreen Is Nothing Then
            MyScreen = Screen.FromPoint(New Point(Frm.Left + Frm.Width / 2, Frm.Top))
        End If

        If MyScreen Is Nothing Then
            MyScreen = Screen.FromPoint(New Point(Frm.Left + Frm.Width / 2, Frm.Bottom))
        End If

        If MyScreen Is Nothing Then
            MyScreen = Screen.FromPoint(New Point(Frm.Left, Frm.Bottom + Frm.Height / 2))
        End If

        If MyScreen Is Nothing Then
            MyScreen = Screen.FromPoint(New Point(Frm.Right, Frm.Bottom + Frm.Height / 2))
        End If

        If MyScreen Is Nothing Then
            MyScreen = Screen.FromPoint(New Point(Frm.Left, Frm.Top))
        End If

        If MyScreen Is Nothing Then
            MyScreen = Screen.FromPoint(New Point(Frm.Left, Frm.Bottom))
        End If

        If MyScreen Is Nothing Then
            MyScreen = Screen.FromPoint(New Point(Frm.Right, Frm.Top))
        End If

        If MyScreen Is Nothing Then
            MyScreen = Screen.FromPoint(New Point(Frm.Right, Frm.Bottom))
        End If

        If Not (MyScreen Is Nothing) Then
            OUT = New FP_L_Rect(MyScreen.WorkingArea)
        End If

        Return OUT
    End Function

    Public Function SELECT_PRINTER(Optional ByVal PFDKey_for_Printer As String = "PRINTER_LAST_SELECTED", Optional ByRef OUT_SelectedPrinter As String = "") As Boolean
        OUT_SelectedPrinter = ""

        Dim SelectedPrinter As String = ""
        Dim PDoc As New Printing.PrintDocument
        Dim OUT As Boolean = False
        Dim Show_PrinterDialog As Boolean = True
        Dim Save_To_PFD As Boolean = True

        Using PDoc
            If PFDKey_for_Printer > "" Then
                PFDlesen(PFDKey_for_Printer, SelectedPrinter)
                If SelectedPrinter.ToUpper = "#_QUESTION_#" Then Save_To_PFD = False
                PDoc.PrinterSettings.PrinterName = SelectedPrinter
                If Not PDoc.PrinterSettings.IsValid Then
                    SelectedPrinter = ""
                    Show_PrinterDialog = True
                Else
                    Show_PrinterDialog = False
                    OUT_SelectedPrinter = SelectedPrinter
                    OUT = True
                End If
            End If
        End Using

        If Show_PrinterDialog Then
            Dim PDialog As New PrintDialog
            Using PDialog
                PDialog.PrinterSettings.PrinterName = SelectedPrinter
                Dim F As Form = ShowDialogForm_getOpacityForm(Form.ActiveForm)
                OUT = (PDialog.ShowDialog() = DialogResult.OK)
                If Not (F Is Nothing) Then
                    F.Close()
                End If

                If OUT Then
                    OUT_SelectedPrinter = PDialog.PrinterSettings.PrinterName
                    If PFDKey_for_Printer > "" Then
                        If Save_To_PFD Then
                            PFDinsertOrUpdate(PFDKey_for_Printer, OUT_SelectedPrinter)
                        End If
                    End If
                End If
            End Using
        End If

        Return OUT
    End Function

    Public Sub DoErrorMsgBox(ByVal FPf As FP_Form, ByVal ErrPlace As String, ByVal ErrNr As Long, ByVal ErrDescription As String, Optional ByVal WriteToTransactErrors As Boolean = True)
        Dim ModuleIdentifier As String = ""

        If Not (FPf Is Nothing) Then
            ModuleIdentifier = FPf.ModuleIdentifier
        End If

        DoErrorMsgBox(ErrPlace, ErrNr, ErrDescription, WriteToTransactErrors, ModuleIdentifier)
    End Sub

    Public Sub DoErrorMsgBox(ByVal ErrPlace As String, ByVal ErrNr As Long, ByVal ErrDescription As String, Optional ByVal WriteToTransactErrors As Boolean = True, Optional ByVal ModuleIdentifier As String = "")
        Try
            If DC.CNN_IsConnected Then
                If WriteToTransactErrors Then
                    Tr_WriteError_To_Transact_Errors(-1, Left(ErrPlace, 128), 0, ErrNr, ErrDescription)
                End If
                If InitMinimalGlobals() Then
                    DoMyMsgBox(31, Trim$(ErrPlace) & "|" & Trim(Str(ErrNr)) + " " + ErrDescription, , , , ModuleIdentifier)    '!!!!!!! +Err.description-t én tettem hozzá. A tesztelés során hasznos
                Else
                    MsgBox(ErrPlace + " Error Code: " + ErrNr.ToString + vbCrLf + vbCrLf + ErrDescription)
                End If
            Else
                If InitMinimalGlobals() Then
                    Dim Params(2) As String
                    Params(0) = ErrNr.ToString
                    Params(1) = ErrDescription
                    DoMyMsgBox_From_Resources(31, Params)
                Else
                    CURSOR_SHOW_DEFAULT()
                    MsgBox(ErrPlace + " Error Code: " + ErrNr.ToString + vbCrLf + vbCrLf + ErrDescription)
                End If
            End If

        Catch ex As Exception
            CURSOR_SHOW_DEFAULT()
            MsgBox("FP_App.DoErrorMsgBox: Error by showing error dialog.")
        End Try
    End Sub

    Public Sub DoMyMsgBox_From_Resources(ByVal MsgNum As Long, Optional ByVal Params() As String = Nothing)
        Dim MsgBox_Text As String = ""
        Dim MsgBox_Titel As String = "Simple_MsgBox Number: " + MsgNum.ToString

        Text_get_From_Resource("DIALOG", MsgNum, MsgBox_Text, , , Params)

        CURSOR_SHOW_DEFAULT()
        MsgBox(MsgBox_Text, , MsgBox_Titel)
    End Sub

    Public Function Directory_Delete(Directory_Name As String, Optional WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = False

        If System.IO.Directory.Exists(Directory_Name) Then
            Try
                System.IO.Directory.Delete(Directory_Name, True)
                OUT = True

            Catch ex As Exception
                If WithDialog Then
                    DoMyMsgBox(1508, Directory_Name)
                End If
            End Try
        End If

        Return OUT
    End Function

    Public Function Directory_Create(Directory_Name As String, Optional WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = True

        If System.IO.Directory.Exists(Directory_Name) = False Then
            Try
                System.IO.Directory.CreateDirectory(Directory_Name)

            Catch ex As Exception
                OUT = False
                If WithDialog Then
                    DoMyMsgBox(1509, Directory_Name)
                End If
            End Try
        End If

        Return OUT
    End Function

    Public Function Directory_IsEmpty(Directory_Name As String) As Boolean
        Dim OUT As Boolean

        Dim DirInfo As New IO.DirectoryInfo(Directory_Name)
        Dim DirectoryList() As IO.DirectoryInfo = DirInfo.GetDirectories

        OUT = (DirectoryList.Count = 0)

        If OUT Then
            Dim FileList() As IO.FileInfo = DirInfo.GetFiles()

            OUT = (FileList.Count = 0)
        End If

        Return OUT
    End Function

    Public Function Directory_Empty(Directory_Name As String, Optional WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = Directory_Delete(Directory_Name, WithDialog)

        If OUT = True Then
            OUT = Directory_Create(Directory_Name, WithDialog)
        End If

        Return OUT
    End Function

    Public Function Directory_Copy(Directory_Name As String, To_Directory_Name As String) As Boolean
        Dim OUT As Boolean = False

        If System.IO.Directory.Exists(Directory_Name) = False Then
            OUT = True
        Else
            If Directory_Create(To_Directory_Name) Then

                Try
                    My.Computer.FileSystem.CopyDirectory(Directory_Name, To_Directory_Name)
                    OUT = True
                Catch ex As Exception
                    DoMyMsgBox(1510, Directory_Name + "|" + To_Directory_Name)
                End Try
            End If
        End If

        Return OUT
    End Function

    Public Function Directory_Move(Directory_Name As String, To_Directory_Name As String) As Boolean
        Dim OUT As Boolean = Directory_Copy(Directory_Name, To_Directory_Name)

        If OUT Then
            OUT = Directory_Delete(Directory_Name)
        End If

        Return OUT
    End Function


    Public Function File_Delete(File_Name As String, Optional WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = False

        If System.IO.File.Exists(File_Name) Then
            Try
                System.IO.File.Delete(File_Name)
                OUT = True

            Catch ex As Exception
                If WithDialog Then
                    DoMyMsgBox(1511, File_Name)
                End If
            End Try
        End If

        Return OUT
    End Function

    Public Function File_Copy(File_Name As String, To_File_Name As String, Optional WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = False

        If System.IO.File.Exists(File_Name) = False Then
            OUT = False
            If WithDialog Then
                DoMyMsgBox(1512, File_Name)
            End If
        Else
            Try
                System.IO.File.Copy(File_Name, To_File_Name)

                File_Wait_Until_Locked(To_File_Name)

                OUT = True
            Catch ex As Exception
                If WithDialog Then
                    DoMyMsgBox(1513, File_Name + "|" + To_File_Name)
                End If
            End Try
        End If

        Return OUT
    End Function

    Public Function File_Move(File_Name As String, To_File_Name As String, Optional WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = File_Copy(File_Name, To_File_Name, WithDialog)

        If OUT Then
            OUT = File_Delete(File_Name)
        End If

        Return OUT
    End Function

    Public Sub File_Wait_Until_Locked(FileName As String)
        Do While File_Is_Locked(FileName)
            Application.DoEvents()
            System.Threading.Thread.Sleep(1000)
            Application.DoEvents()
            System.Threading.Thread.Sleep(1000)
            Application.DoEvents()
        Loop
    End Sub

    Public Function File_Is_Locked(FileName As String) As Boolean
        Dim OUT As Boolean = False

        If File.Exists(FileName) Then
            Dim FStream As FileStream

            Try
                FStream = File.Open(FileName, FileMode.Open)
                FStream.Close()
                FStream.Dispose()
                FStream = Nothing

            Catch ex As Exception
                OUT = True
            End Try
        End If

        Return OUT
    End Function

    Private Sub ZIP_CompressFolder(path As String, zipStream As ZipOutputStream, folderOffset As Integer)

        Dim files As String() = Directory.GetFiles(path)

        For Each filename As String In files

            Dim fi As New FileInfo(filename)

            Dim entryName As String = filename.Substring(folderOffset)  ' Makes the name in zip based on the folder
            entryName = ZipEntry.CleanName(entryName)       ' Removes drive from name and fixes slash direction
            Dim newEntry As New ZipEntry(entryName) With {
                .DateTime = fi.LastWriteTime            ' Note the zip format stores 2 second granularity
            }

            ' Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
            '   newEntry.AESKeySize = 256;

            ' To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
            ' you need to do one of the following: Specify UseZip64.Off, or set the Size.
            ' If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
            ' but the zip will be in Zip64 format which not all utilities can understand.
            '   zipStream.UseZip64 = UseZip64.Off;
            newEntry.Size = fi.Length

            zipStream.PutNextEntry(newEntry)

            ' Zip the file in buffered chunks
            ' the "using" will close the stream even if an exception occurs
            Dim buffer As Byte() = New Byte(4095) {}
            Using streamReader As FileStream = File.OpenRead(filename)
                StreamUtils.Copy(streamReader, zipStream, buffer)
            End Using
            zipStream.CloseEntry()
        Next
        Dim folders As String() = Directory.GetDirectories(path)
        For Each folder As String In folders
            ZIP_CompressFolder(folder, zipStream, folderOffset)
        Next
    End Sub
    Public Function ZIP(DirectoryName As String, To_ZIP_DirectoryName As String) As Boolean
        '------------------------------------------------------------------------------------------------------------------
        'icsharpcode
        '------------------------------------------------------------------------------------------------------------------

        Dim OUT As Boolean = False

        Try
            Dim fsOut As FileStream = File.Create(To_ZIP_DirectoryName)
            Dim zipStream As New ZipOutputStream(fsOut)

            zipStream.SetLevel(3)          '0-9, 9 being the highest level of compression
            zipStream.Password = Nothing   ' optional. Null is the same as not setting.

            ' This setting will strip the leading part of the folder path in the entries, to
            ' make the entries relative to the starting folder.
            ' To include the full path for each entry up to the drive root, assign folderOffset = 0.
            Dim folderOffset As Integer = DirectoryName.Length + (If(DirectoryName.EndsWith("\"), 0, 1))

            ZIP_CompressFolder(DirectoryName, zipStream, folderOffset)

            zipStream.IsStreamOwner = True
            ' Makes the Close also Close the underlying stream
            zipStream.Close()

            OUT = True

        Catch ex As Exception
            DoErrorMsgBox("FP_App.File_ZIP", Err.Number, Err.Description)
        End Try

        Return OUT



        '------------------------------------------------------------------------------------------------------------------
        'DotNetZip
        '------------------------------------------------------------------------------------------------------------------

        'Dim OUT As Boolean = False

        'Try
        '    Using ZIPFile As Ionic.Zip.ZipFile = New Ionic.Zip.ZipFile(To_ZIP_FileName)

        '        ZIPFile.AddDirectory(DirectoryName, "")

        '        ZIPFile.ZipErrorAction = Ionic.Zip.ZipErrorAction.Skip

        '        ZIPFile.Save()

        '        OUT = True

        '    End Using


        'Catch ex As Exception
        '    DoErrorMsgBox("FP_App.File_ZIP", Err.Number, Err.Description)
        'End Try

        'Return OUT

    End Function

    Public Function UNZIP(ZIP_FileName As String, To_Directory As String) As Boolean
        '------------------------------------------------------------------------------------------------------------------
        'icsharpcode
        '------------------------------------------------------------------------------------------------------------------

        Dim OUT As Boolean = False

        To_Directory = Trim(To_Directory)

        Try
            Dim zf As ZipFile = Nothing
            Try
                Dim fs As FileStream = File.OpenRead(ZIP_FileName)

                zf = New ZipFile(fs)
                'zf.Password = ""

                For Each zipEntry As ZipEntry In zf
                    If Not zipEntry.IsFile Then     ' Ignore directories
                        Continue For
                    End If
                    Dim entryFileName As [String] = zipEntry.Name
                    ' to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    ' Optionally match entrynames against a selection list here to skip as desired.
                    ' The unpacked length is available in the zipEntry.Size property.

                    Dim buffer As Byte() = New Byte(4095) {}    ' 4K is optimum
                    Dim zipStream As Stream = zf.GetInputStream(zipEntry)

                    ' Manipulate the output filename here as desired.
                    Dim fullZipToPath As [String] = Path.Combine(To_Directory, entryFileName)
                    Dim directoryName As String = Path.GetDirectoryName(fullZipToPath)
                    If directoryName.Length > 0 Then
                        Directory.CreateDirectory(directoryName)
                    End If

                    ' Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    ' of the file, but does not waste memory.
                    ' The "Using" will close the stream even if an exception occurs.
                    Using streamWriter As FileStream = File.Create(fullZipToPath)
                        StreamUtils.Copy(zipStream, streamWriter, buffer)
                    End Using
                Next
            Finally
                If zf IsNot Nothing Then
                    zf.IsStreamOwner = True     ' Makes close also shut the underlying stream
                    ' Ensure we release resources
                    zf.Close()

                    OUT = True
                End If
            End Try

        Catch ex As Exception
            DoErrorMsgBox("FP_App.UNZIP", Err.Number, Err.Description)
            MsgBox(ex.Message)
        End Try

        Return OUT

        '-----------------------------------------------------------------------------
        'DotNetZip
        '-----------------------------------------------------------------------------
        'Dim OUT As Boolean = False

        'To_Directory = Trim(To_Directory)

        'Try
        '    Using Z As Ionic.Zip.ZipFile = Ionic.Zip.ZipFile.Read(ZIP_FileName)
        '        Z.ExtractAll(To_Directory)
        '    End Using

        'Catch ex As Exception
        '    DoErrorMsgBox("FP_App.UNZIP", Err.Number, Err.Description)
        '    MsgBox(ex.Message)
        'End Try

        'Return OUT
    End Function


    Public Function Archive_SELEXPED_ProgramFiles(ToDirectory As String) As Boolean
        ToDirectory = Trim(ToDirectory)
        If Strings.Right(ToDirectory, 1) <> "\" Then ToDirectory += "\"

        Dim OUT As Boolean = Directory_Create(ToDirectory)
        Dim App_StartUp_Path As String = ""

        If OUT Then
            App_StartUp_Path = Application.StartupPath + "\"
            OUT = Directory_Empty(ToDirectory)
        End If

        If OUT Then
            Dim DirInfo As New IO.DirectoryInfo(App_StartUp_Path)

            If OUT Then
                Dim DirectoryList() As IO.DirectoryInfo = DirInfo.GetDirectories

                For Each item In DirectoryList
                    If item.Name.ToString <> "_OLD" Then
                        OUT = Directory_Copy(App_StartUp_Path + item.Name, ToDirectory + item.Name)

                        If OUT = False Then
                            Exit For
                        End If
                    End If
                Next
            End If

            If OUT Then
                Dim FileList() As IO.FileInfo = DirInfo.GetFiles()

                For Each item In FileList
                    OUT = File_Copy(item.Name, ToDirectory + item.Name)
                    If OUT = False Then
                        Exit For
                    End If
                Next
            End If
        End If

        Return OUT
    End Function

    Public Function UPDATE_PREPARE(ByRef OUT_Selexped_Updater_Location As String, NoTestUpdate As Boolean) As Boolean
        Dim OUT As Boolean = SELEXPED_Folders_CREATE()
        Dim Dir_UPDATE As String = SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP) + "UPDATER\"
        Dim Dir_OLD As String = Application.StartupPath + "\_OLD\"
        Dim Dir_Files As String = Dir_UPDATE + "Files"
        Dim ZIP_FileName As String = Dir_UPDATE + "SELEXPED_UPDATE.zip"
        Dim Updater_INI_File As String = ""
        Dim UPDATE_ID As Long = 0
        Dim DRow As DataRow = Nothing
        Dim TestUpdate As Boolean = False
        Dim ProgressForm As New FP_ProgressForm(Me, "", 0, 100)

        If NoTestUpdate = False Then
            DRow = DC.Qdf_get_DataRow("SELECT ID FROM SEL_SYS_UPDATES WHERE StatusID = 1")
            If DRow IsNot Nothing Then
                UPDATE_ID = DRow!ID
                TestUpdate = True
                DoMyMsgBox(1519) 'Tesztupdate
            End If
        End If

        If UPDATE_ID = 0 Then
            DRow = DC.Qdf_get_DataRow("SELECT ID FROM SEL_SYS_UPDATES WHERE StatusID = 2")
            If DRow Is Nothing Then
                OUT = False
                DoMyMsgBox(1520) 'Nincsenek update-ek feltoltve.
            Else
                UPDATE_ID = DRow!ID
            End If
        End If

        If OUT Then
            ProgressForm.Show()
            ProgressForm.SET_VALUES(2, "")
        End If

        OUT_Selexped_Updater_Location = ""

        '--------------------------------------------------------------------------
        'Create directories
        '--------------------------------------------------------------------------
        If OUT Then OUT = Directory_Create(Dir_UPDATE)
        If OUT Then OUT = Directory_Create(Dir_Files)
        If OUT Then OUT = Directory_Create(Dir_OLD)

        If OUT Then
            ProgressForm.SET_VALUES(10, "")
        End If

        '--------------------------------------------------------------------------
        'Archive current version
        '--------------------------------------------------------------------------
        If OUT Then OUT = Archive_SELEXPED_ProgramFiles(Dir_OLD)

        '--------------------------------------------------------------------------
        'SELEXPED files for update to SEL_TEMP\UPDATER\Files
        '--------------------------------------------------------------------------
        If OUT Then
            Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

            With DC
                .Qdf_set_SP(sqlComm, "SEL_SYS_UPDATES_LOAD_FILE", 0)
                .Qdf_AddParameter(sqlComm, "@SEL_SYS_UPDATES_ID", SqlDbType.Int, , , , , UPDATE_ID)
                .Qdf_AddParameter(sqlComm, "@ImageData", SqlDbType.VarBinary, ParameterDirection.Output, -1)

                .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
            End With

            CURSOR_SHOW_WAIT()
            Try
                OUT = DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")

            Catch ex As Exception
                CURSOR_SHOW_DEFAULT()
                OUT = False
                DoErrorMsgBox("FP_App.UPDATE_PREPARE", Err.Number, Err.Description)
            End Try

            CURSOR_SHOW_DEFAULT()

            If OUT Then
                OUT = ByteArray_SaveFile(ZIP_FileName, sqlComm.Parameters("@ImageData").Value)
                If OUT Then
                    CURSOR_SHOW_WAIT()
                    OUT = UNZIP(ZIP_FileName, Dir_Files)
                    CURSOR_SHOW_DEFAULT()
                End If
            End If
        End If

        If OUT Then
            ProgressForm.SET_VALUES(75, "")

            '--------------------------------------------------------------------------
            'SELEXPED_INSTALLER.exe
            '--------------------------------------------------------------------------
            Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

            With DC
                .Qdf_set_SP(sqlComm, "SEL_SYS_UPDATES_LOAD_UPDATER", 720)
                .Qdf_AddParameter(sqlComm, "@ImageData", SqlDbType.VarBinary, ParameterDirection.Output, -1)
                .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
            End With

            CURSOR_SHOW_WAIT()
            Try
                OUT = DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")

            Catch ex As Exception
                CURSOR_SHOW_DEFAULT()
                OUT = False
                DoErrorMsgBox("FP_App.UPDATE_PREPARE", Err.Number, Err.Description)
            End Try

            CURSOR_SHOW_DEFAULT()

            If OUT Then
                OUT_Selexped_Updater_Location = Dir_UPDATE + "SELEXPED_UPDATER.exe"
                OUT = ByteArray_SaveFile(OUT_Selexped_Updater_Location, sqlComm.Parameters("@ImageData").Value)
            End If
        End If

        '--------------------------------------------------------------------------
        'SELEXPED_INSTALLER.ini
        '--------------------------------------------------------------------------
        If OUT Then
            ProgressForm.SET_VALUES(95, "")

            Updater_INI_File += String.Format("HOME_DIR={0}", Application.StartupPath + "\") + vbCrLf
            Updater_INI_File += String.Format("SELEXPED_STARTUP_FILE={0}", Application.ExecutablePath) + vbCrLf
            Updater_INI_File += String.Format("TEST_UPDATE={0}", IIf(TestUpdate, "1", "0")) + vbCrLf
            OUT = Text_WriteToTextFile(Updater_INI_File, Dir_UPDATE + "SELEXPED_UPDATER.ini")
        End If

        '--------------------------------------------------------------------------
        'Register update in SEL_SYS_UPDATES_TERMINAL_STATUS table
        '--------------------------------------------------------------------------
        If OUT Then
            Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

            With DC
                .Qdf_set_SP(sqlComm, "SEL_SYS_UPDATES_SET_INPROCESS")
                .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            End With

            CURSOR_SHOW_WAIT()
            Try
                OUT = DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE)

            Catch ex As Exception
                CURSOR_SHOW_DEFAULT()
                OUT = False
                DoErrorMsgBox("FP_App.UPDATE_PREPARE", Err.Number, Err.Description)
            End Try

            CURSOR_SHOW_DEFAULT()
        End If

        ProgressForm.SET_VALUES(100, "")
        System.Threading.Thread.Sleep(1000)
        ProgressForm.Close()
        ProgressForm.Dispose()
        ProgressForm = Nothing

        Return OUT
    End Function

    Public Function EXE_FullPath() As String
        Dim OUT As String = System.Reflection.Assembly.GetExecutingAssembly.CodeBase

        OUT = Replace(OUT, "file:///", "")
        OUT = Replace(OUT, "/", "\")

        Return OUT
    End Function

    Public Function EXE_Dir() As String
        Dim w As Integer

        w = EXE_FullPath.LastIndexOf("\")
        EXE_Dir = EXE_FullPath.Substring(0, w)
    End Function
    Public Function EXE_GetName() As String
        Dim w As Integer

        w = EXE_FullPath.LastIndexOf("/")
        EXE_GetName = EXE_FullPath.Substring(w + 1, EXE_FullPath.Length - (w + 1))
    End Function

    Public Function FIXTEXT_SAVE_SIMPLE(ByVal FixText_Key As String, ByVal FixText As String) As Boolean
        Dim OUT As Boolean = False

        Dim sqlComm As SqlCommand = Nothing

        DC.Qdf_set_SP(sqlComm, "FixText_SAVE_Simple")
        DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
        DC.Qdf_AddParameter(sqlComm, "@FixText_Key", SqlDbType.NVarChar, , 50, FixText_Key)
        DC.Qdf_AddParameter(sqlComm, "@FixText", SqlDbType.NVarChar, , -1, FixText)

        CURSOR_SHOW_WAIT()
        Try
            OUT = DC.Qdf_Execute("", sqlComm)
        Catch ex As Exception
            DoErrorMsgBox("FP_App.FIXTEXT_SAVE_SIMPLE", Err.Number, Err.Description)
        End Try
        CURSOR_SHOW_DEFAULT()

        FIXTEXT_SAVE_SIMPLE = OUT
    End Function
    Public Function FIXTEXT_CHK_PARAM(ByRef FIXTEXT_DIC As Dictionary(Of String, String), ByVal FixTextKey As String, ByVal ParamName As String, Optional ByVal MustHaveValue As Boolean = True, Optional ByVal DoDialog As Boolean = True, Optional ByVal DialNr As Long = 877, Optional ByVal DialParams As String = "") As Boolean
        Dim OUT As Boolean = True

        ParamName = ParamName.ToUpper

        If Not FIXTEXT_DIC.ContainsKey(ParamName) Then
            OUT = False
        Else
            If MustHaveValue Then
                If Trim(FIXTEXT_DIC(ParamName)) = "" Then
                    OUT = False
                End If
            End If
        End If

        If Not OUT Then
            If DoDialog Then
                If Trim(DialParams) = "" Then
                    DialParams = FixTextKey + "|" + ParamName
                End If
                DoMyMsgBox(DialNr, DialParams)
            End If
        End If

        FIXTEXT_CHK_PARAM = OUT
    End Function
    Public Function FIXTEXT_SPLIT_PARAMS(ByVal FixText_Text As String, ByRef ToDictionary As Dictionary(Of String, String)) As Boolean
        Dim OUT As Boolean = True

        Dim FixText_Rows As String()

        If FixText_Text > "" Then
            If ToDictionary Is Nothing Then
                ToDictionary = New Dictionary(Of String, String)
            End If

            Dim RowNum As Long
            Dim AktLine As String
            Dim Key As String
            Dim Value As String
            Dim p As Long

            FixText_Rows = Split(FixText_Text, vbCrLf)
            For RowNum = 0 To UBound(FixText_Rows)
                AktLine = FixText_Rows(RowNum)
                If Trim(AktLine) > "" Then
                    If Left(Trim(AktLine), 1) <> "'" Then
                        p = InStr(AktLine, "=")
                        If p > 0 Then
                            Key = Trim(Replace((Mid(AktLine, 1, p - 1)).ToUpper, Chr(9), ""))
                            Value = Trim(Mid(AktLine, p + 1))

                            If ToDictionary.ContainsKey(Key) = False Then 'Mert ha "_+"-al ki lett egeszitve, akkor igy valosul meg az "orokles"
                                ToDictionary.Add(Key, Value)
                            End If
                        End If
                    End If
                End If
            Next
        End If

        Return OUT
    End Function
    Public Function FIXTEXT_getParam(ByVal ParamName As String, ByRef FixText_DIC As Dictionary(Of String, String)) As String
        Dim OUT As String = ""

        If Not (FixText_DIC Is Nothing) Then
            ParamName = ParamName.ToUpper
            If FixText_DIC.ContainsKey(ParamName) Then
                OUT = FixText_DIC(ParamName)
            End If
        End If

        Return OUT
    End Function

    Public Function FORMS_GET_HANDLE(ByVal FormName As String) As Long
        Dim OUT As Long = 0
        Dim AktHandle As Long

        For Each AktHandle In Forms.Keys
            If Forms(AktHandle).Frm.Name = FormName Then
                OUT = AktHandle
                Exit For
            End If
        Next

        Return OUT
    End Function

    Public Function FORMS_IS_DIALOG_OPEN() As Boolean
        Dim OUT As Boolean = False
        Dim AktHandle As Long

        For Each AktHandle In Forms.Keys
            If Forms(AktHandle).Frm.Modal Then
                OUT = True
                Exit For
            End If
        Next

        Return OUT
    End Function

    Public Function FORMS_GET_FPf(ByVal Frm As Form) As FP_Form
        Dim OUT As FP_Form = Nothing
        Dim Frm_Handle As Long = Form_Handle(Frm)

        If Frm_Handle <> 0 Then
            If Forms.ContainsKey(Frm_Handle) Then
                OUT = Forms(Frm_Handle)
            End If
        End If

        FORMS_GET_FPf = OUT
    End Function

    Public Function getFixText(ByVal MyCode As String, Optional LoadFromLocalDB As Boolean = True) As String
        Dim OUT As String = ""

        If DC.P_USE_LocalDB And LoadFromLocalDB = True Then
            Dim SQL As String = String.Format("SELECT [Text] FROM FixText WHERE Code = '{0}'", MyCode)
            Dim DRow As DataRow = DC.LocalDB_SEL.get_DataRow(SQL)

            If Not (DRow Is Nothing) Then
                OUT = DRow!Text
            End If
        Else
            Dim SQL As String = String.Format("SELECT dbo.FN_get_FixText('{0}') Text", MyCode)
            Dim DRow As DataRow = DC.Qdf_get_DataRow(SQL)

            If Not (DRow Is Nothing) Then
                OUT = DRow!Text
            End If
        End If

        Return OUT
    End Function

    Public Function getINIFileName(Optional ByVal NameOhnePath As Boolean = False) As String
        Dim OUT As String = String.Empty
        Dim p_BackSlash As Long
        Try
            If NameOhnePath Then
                OUT = INIFileName
            Else
                p_BackSlash = InStrRev(Application.ExecutablePath, "\")
                If p_BackSlash > 1 Then
                    OUT = String.Format("{0}{1}", Left(Application.ExecutablePath, p_BackSlash), INIFileName)
                Else
                    DoMyMsgBox(1016) 'nem tudtam meghatarozni a nevet
                End If
            End If
        Catch ex As Exception
            DoErrorMsgBox("FP_App.getINIFileName", Err.Number, Err.Description)
        End Try
        Return OUT
    End Function

    Public Function getINI_Backup_FileName() As String
        Dim OUT As String = getFileName_without_Extension(getINIFileName()) + ".INI_BACKUP"
        Return OUT
    End Function

    Public Sub SET_DATA_FORMATS_RESET()
        Format_Date_Splitter = "/"
        Format_Date_LengthOfYear = 4
        Format_Date_Order = "DMY"
        Format_Time_Splitter = ":"
        Format_Float_DecimalPoint = ","
    End Sub

    Public Sub InitLanguage()
        If Trim(LandDialog) = String.Empty Then
            PFDlesen("LANDDIALOG", LandDialog)
            setLanddialog(LandDialog)

            Dim Saved_Data_Formats As String = ""

            If Not PFDlesen("DATA_FORMATS", Saved_Data_Formats) Then
                SET_DATA_FORMATS_RESET()
            Else
                Format_Date_Splitter = Mid(Saved_Data_Formats, 1, 1)
                Format_Date_Order = Mid(Saved_Data_Formats, 2, 3)
                Format_Date_LengthOfYear = Val(Mid(Saved_Data_Formats, 5, 1))
                Format_Time_Splitter = Mid(Saved_Data_Formats, 6, 1)
                Format_Float_DecimalPoint = Mid(Saved_Data_Formats, 7, 1)

                If Format_Date_Splitter = "" Or Len(Format_Date_Order) <> 3 Or (Format_Date_LengthOfYear <> 2 And Format_Date_LengthOfYear <> 4) Or Format_Time_Splitter = "" Or Format_Float_DecimalPoint = "" Or Format_Date_Splitter = Format_Time_Splitter Then
                    SET_DATA_FORMATS_RESET()
                End If
            End If
        End If
    End Sub

    Public Function InitMinimalGlobals() As Boolean
        Dim OUT As Boolean = True

        Randomize()
        InitLanguage()

        Return OUT
    End Function

    Public Function HELP_Link_Execute(ByVal Link As String) As Boolean
        Dim OUT As Boolean = True

        Link = Trim(Link)

        If Link > "" Then
            Dim MyDir As String = ""

            ParmLesen("HLP", "DIR", 0, MyDir)
            If MyDir > "" Then
                If Microsoft.VisualBasic.Right(MyDir, 1) <> "\" Then
                    MyDir += "\"
                End If
            End If

            OUT = True
            Try
                Process.Start(MyDir + Link)
            Catch ex As Exception
                OUT = False
            End Try

            If Not OUT Then
                OUT = True
                Try
                    Process.Start(Link)
                Catch ex As Exception
                    OUT = False
                End Try
            End If

            If Not OUT Then
                DoMyMsgBox(1205) 'Link does not exists
            End If
        End If

        HELP_Link_Execute = OUT
    End Function

    Public Function Login(ByVal LoginName As String, ByVal Password As String) As Boolean
        Dim OUT As Boolean = True
        Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

        With DC
            DC.Qdf_set_SP(sqlComm, "Terminals_Login")
            DC.Qdf_AddParameter(sqlComm, "@Language", SqlDbType.NVarChar, , 3, LandDialog)
            DC.Qdf_AddParameter(sqlComm, "@TerminalName", SqlDbType.NVarChar, , 24, Terminal)
            DC.Qdf_AddParameter(sqlComm, "@LoginName", SqlDbType.NVarChar, , 10, LoginName)
            DC.Qdf_AddParameter(sqlComm, "@Password", SqlDbType.NVarChar, , 24, Password)
            DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
        End With

        CURSOR_SHOW_WAIT()

        Try
            OUT = DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")

        Catch ex As Exception
            CURSOR_SHOW_DEFAULT()
            OUT = False
            DoErrorMsgBox("FP_App.Login", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        Return OUT
    End Function


    Public Sub Logout()
        Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

        With DC
            DC.Qdf_set_SP(sqlComm, "gl_UnlockTerminal")
            DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
        End With

        CURSOR_SHOW_WAIT()

        Try
            DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")

        Catch ex As Exception
            CURSOR_SHOW_DEFAULT()
            DoErrorMsgBox("FP_App.Login", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()
    End Sub

    Public Function InitGlobals_with_Reconnect()
        Dim OUT As Boolean

        Initialized = False
        DC = New FP_DataConnect(Me)
        OUT = InitGlobals()

        Return OUT
    End Function

    Public Function InitGlobals(Optional ByVal mitUserCheck As Boolean = True) As Boolean
        Dim OUT As Boolean = True
        Static InitGlobals_Running As Boolean = False

        If Not InitGlobals_Running Then
            InitGlobals_Running = True

            If Not Initialized Then
                If Not InitMinimalGlobals() Then
                    OUT = False
                End If

                If OUT = True Then
                    Dim MyInitPhase As String = String.Empty

                    PFDlesen("INITPHASE", MyInitPhase)
                    If MyInitPhase = "DOCONNECT" Or My.Computer.Keyboard.ShiftKeyDown Then
                        OUT = DC.CNN_SET_WITH_FORM
                        If OUT Then
                            PFDinsertOrUpdate("INITPHASE", "DONE")
                        End If
                    End If
                End If

                If OUT Then
                    OUT = DC.CNN_INIT(True)
                End If

                Dim Layout_Style_STR As String = "NORMAL"
                PFDlesen("LAYOUT", Layout_Style_STR)
                Select Case Trim(Layout_Style_STR).ToUpper
                    Case "", "NORMAL"
                        Layout_Style = ENUM_LAYOUT_STYLE.NORMAL

                    Case "TABLET"
                        Layout_Style = ENUM_LAYOUT_STYLE.TABLET

                    Case Else
                        Layout_Style = ENUM_LAYOUT_STYLE.NORMAL
                End Select

                If OUT Then
                    If mitUserCheck Then
                        If Terminal <> "TSERVER" Then
                            If Not Users_setUserGlobals() Then
                                OUT = False
                                DoMyMsgBox(844) 'A Terminals tablaban a User parameter erteke hibas. A program igy nem indithato el.
                            End If
                        End If
                    End If
                End If

                If OUT Then
                    OUT = InitGlobals_ReadGeneralParams()
                End If

                TEMP_Folders_EMPTY()
            End If

            If OUT Then
                If mitUserCheck Then
                    Initialized = True
                End If

                If Is_DEBUG_MODE() Then
                    'Nothing to do
                End If
            End If

            InitGlobals_Running = False
        End If

        Return OUT
    End Function

    Public Function ParmInsertOrUpdate(ByVal MyKulcsElotag As String, ByVal MyKulcs As String, ByVal MyNumertek As Long, Optional ByVal MyAlfaNum As String = "", Optional ByVal ParentTransactID As Long = 0) As Boolean
        Dim OUT As Boolean = False

        If InitMinimalGlobals() Then
            Try
                Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

                DC.Qdf_set_SP(sqlComm, "gl_ParmInsertOrUpdate")
                DC.Qdf_AddParameter(sqlComm, "@MyKulcsElotag", SqlDbType.NVarChar, , 3, MyKulcsElotag)
                DC.Qdf_AddParameter(sqlComm, "@MyKulcs", SqlDbType.NVarChar, , 20, MyKulcs)
                DC.Qdf_AddParameter(sqlComm, "@MyNumertek", SqlDbType.Int, , , , , MyNumertek)
                DC.Qdf_AddParameter(sqlComm, "@MyAlfaNum", SqlDbType.NVarChar, , 132, MyAlfaNum)
                DC.Qdf_AddParameter(sqlComm, "@ParentTransactID", SqlDbType.Int, , , , , ParentTransactID)
                DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)

                OUT = DC.Qdf_Execute("", sqlComm)
                sqlComm.Dispose()

            Catch ex As Exception
                DoErrorMsgBox("FP_App.ParmInsertOrUpdate", Err.Number, Err.Description)
            End Try
        End If

        Return OUT

    End Function

    Public Function ParmLesen(ByVal MyKulcsElotag As String, ByVal MyKulcs As String, ByRef MyNumertek As Long, Optional ByRef MyAlfaNum As String = "") As Boolean
        Dim OUT As Boolean = True

        MyNumertek = 0
        MyAlfaNum = ""
        Dim MySQL As String = String.Format("SELECT NumErtek, AlfaNumertek FROM Parameterek WHERE KulcsElotag='{0}' AND Kulcs='{1}'", MyKulcsElotag, MyKulcs)

        Dim DRow As DataRow = DC.Qdf_get_DataRow(MySQL)

        If (DRow Is Nothing) Then
            OUT = False
        Else
            MyNumertek = nz(DRow!NumErtek, 0)
            MyAlfaNum = nz(DRow!AlfaNumertek, "")
        End If

        Return OUT
    End Function

    Private Function PFD_CHK_Params(DIC_Temp As Dictionary(Of String, String)) As Boolean
        Dim OUT As Boolean = True
        If Not DIC_Temp.ContainsKey("LANDDIALOG") Then
            OUT = False
        ElseIf Not DIC_Temp.ContainsKey("TERMINAL") Then
            OUT = False
        ElseIf Not DIC_Temp.ContainsKey("CONNECT_VB") Then
            OUT = False
        End If

        Return OUT
    End Function

    Private Sub PFD_LOAD_FROM_INI()
        Dim Heap As String = Text_getTextFile(getINIFileName())
        Dim DIC_Temp As New Dictionary(Of String, String)

        FIXTEXT_SPLIT_PARAMS(Heap, DIC_Temp)

        For Each Key As String In DIC_Temp.Keys
            If Not DIC_PFD.Keys.Contains(Key) Then
                DIC_PFD.Add(Key, DIC_Temp(Key))
            End If
        Next

        If Not PFD_CHK_Params(DIC_PFD) Then
            Dim Heap_Backup As String = Text_getTextFile(getINI_Backup_FileName())
            Dim DIC_Backup As New Dictionary(Of String, String)

            FIXTEXT_SPLIT_PARAMS(Heap_Backup, DIC_Backup)
            For Each Key As String In DIC_Backup.Keys
                If Not DIC_PFD.Keys.Contains(Key) Then
                    PFDinsertOrUpdate(Key, DIC_Backup(Key))
                End If
            Next
        End If
    End Sub

    Protected Friend Sub PFD_LOAD_FROM_DB()
        Dim Heap As String = getFixText("PFD_" + Terminal, False)

        If Heap = "" Then
            Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

            DC.Qdf_set_SP(sqlComm, "PFD_INIT")
            DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)

            CURSOR_SHOW_WAIT()
            Try
                DC.Qdf_Execute("", sqlComm)
                Heap = getFixText("PFD_" + Terminal, False)

            Catch ex As Exception
                'Nothing to do
            End Try
            sqlComm.Dispose()
        End If

        Dim DIC_Temp As New Dictionary(Of String, String)

        FIXTEXT_SPLIT_PARAMS(Heap, DIC_Temp)

        For Each Key As String In DIC_Temp.Keys
            If Not DIC_PFD.Keys.Contains(Key) Then
                DIC_PFD.Add(Key, DIC_Temp(Key))
            End If
        Next
    End Sub

    Public Function PFD_Key_SIZE_FIX_SAVE(ServerObject_Prefix As String, SubPrefix As String, FieldName As String) As String
        Return String.Format("SIZE_FIX_{0}_{1}_{2}", ServerObject_Prefix, SubPrefix, FieldName)
    End Function

    Public Sub PFD_SAVE(Optional SaveToINI As Boolean = True, Optional SaveToDB As Boolean = True)
        Dim ToINI As String = ""
        Dim ToDB As String = ""
        Dim INIFileName = getINIFileName()

        For Each Key As String In DIC_PFD.Keys
            If PFD_IsKeyInFile(Key) Then
                ToINI += Key + "=" + DIC_PFD(Key) + vbCrLf
            Else
                ToDB += Key + "=" + DIC_PFD(Key) + vbCrLf
            End If
        Next

        If SaveToINI Then
            Text_WriteToTextFile(ToINI, INIFileName)
            Text_WriteToTextFile(ToINI, getINI_Backup_FileName)
        End If

        If Not (DC Is Nothing) Then
            If DC.P_Initialised Then
                If SaveToDB Then
                    Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

                    DC.Qdf_set_SP(sqlComm, "PFD_SAVE")
                    DC.Qdf_AddParameter(sqlComm, "Terminal", SqlDbType.NVarChar, , 10, Terminal)
                    DC.Qdf_AddParameter(sqlComm, "Heap", SqlDbType.NVarChar, , -1, ToDB)

                    DC.Qdf_Execute("", sqlComm)
                    sqlComm.Dispose()
                End If
            End If
        End If
    End Sub

    Public Sub PFDinsertOrUpdate(ByVal Key As String, ByVal Param As String)
        Key = Key.ToUpper

        If DIC_PFD.ContainsKey(Key) Then
            DIC_PFD(Key) = Param
        Else
            DIC_PFD.Add(Key, Param)
        End If
    End Sub

    Public Function PFDlesen(ByVal Key As String, ByRef OUT_Param As String) As Boolean
        Dim OUT As Boolean = False

        OUT_Param = ""

        Key = Key.ToUpper

        If DIC_PFD.ContainsKey(Key) Then
            OUT_Param = DIC_PFD(Key)
            OUT = True
        End If

        Return OUT
    End Function

    Public Sub Dispose()
        Disposed = True
    End Sub

    Public Function RS_GET_FIELDPROPERTIES_FROM_SERVER_OBJECT(ByVal Object_Name As String, ByRef OUT_DT_Fields As DataTable, ByRef OUT_Object_Type As Integer) As Boolean
        Dim OUT As Boolean = False
        Dim Found_In_LocalDB As Boolean = False

        If DC.P_USE_LocalDB = True Then
            Dim MySQL As String

            MySQL = String.Format("SELECT ObjectType FROM SERVER_OBJECTS_FIELDPROPERTIES_HEAD WHERE ObjectName = '{0}'", Object_Name)
            Dim DRow As DataRow = DC.LocalDB_SEL.get_DataRow(MySQL)

            If Not (DRow Is Nothing) Then
                Found_In_LocalDB = True
                MySQL = String.Format("SELECT SeqNum, FieldName, xtype, xtype_VB, xlength, '' Value FROM SERVER_OBJECTS_FIELDPROPERTIES WHERE ObjectName = '{0}' ORDER BY SeqNum", Object_Name)
                DC.LocalDB_SEL.Fill_DT(MySQL, OUT_DT_Fields)

                OUT_Object_Type = DRow!ObjectType
                OUT = True
            End If
        End If

        If Found_In_LocalDB = False Then
            Dim SqlComm As SqlCommand = DC.CNN.CreateCommand

            DC.Qdf_set_SP(SqlComm, "RS_GET_FIELDPROPERTIES_FROM_SERVER_OBJECT")
            DC.Qdf_AddParameter(SqlComm, "@NameOfObject", SqlDbType.NVarChar, , 255, Object_Name)

            If gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_IsInstalled("SEL_SYS_DB_FIELDPROPERTIES") Then
                DC.Qdf_AddParameter(SqlComm, "@Use_SEL_SYS_DB_FIELDPROPERTIES", SqlDbType.Bit, ,,,, IIf(Is_DEBUG_MODE, 0, 1))
            End If

            DC.Qdf_AddParameter(SqlComm, "@OUT_Object_Type", SqlDbType.Int, ParameterDirection.Output)

            DC.Qdf_AddParameter(SqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            DC.Qdf_AddParameter(SqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)

            Dim DA As New SqlDataAdapter(SqlComm)
            Using DA

                OUT_DT_Fields = New DataTable

                OUT_Object_Type = 0

                Try
                    DA.Fill(OUT_DT_Fields)

                    If SqlComm.Parameters("@Result").Value <> -1 Then
                        Dim Result As Long
                        Dim ErrText As String

                        Result = SqlComm.Parameters("@Result").Value
                        ErrText = SqlComm.Parameters("@ErrText").Value

                        OUT = False
                        DoErrorMsgBox("FP_App.RS_GET_FIELDPROPERTIES_FROM_SERVER_OBJECT", 0, String.Format("@Result = {0}. {1}", Result.ToString, ErrText))
                    Else
                        OUT = True
                        OUT_Object_Type = SqlComm.Parameters("@OUT_Object_Type").Value
                    End If

                Catch ex As Exception
                    OUT = False
                    DoErrorMsgBox("FP_App.FORM_SET_FIELDPROPERTIES_FROM_SERVER_OBJECT", Err.Number, Err.Description)
                End Try

                If OUT = True Then
                    If DC.P_USE_LocalDB Then
                        Dim MySQL_Temp As String = ""

                        MySQL_Temp = String.Format("DELETE SERVER_OBJECTS_FIELDPROPERTIES WHERE ObjectName = '{0}'", Object_Name)
                        DC.LocalDB_SEL.RunSQL(MySQL_Temp)

                        MySQL_Temp = "INSERT INTO SERVER_OBJECTS_FIELDPROPERTIES (ObjectName, SeqNum, FieldName, xtype, xtype_VB, xlength) VALUES ('{0}', {1}, '{2}', {3}, '{4}', {5})"
                        For Each R As DataRow In OUT_DT_Fields.Rows
                            DC.LocalDB_SEL.RunSQL(String.Format(MySQL_Temp, Object_Name, R!SeqNum, R!FieldName, R!xtype, R!xtype_VB, R!xlength))
                        Next

                        MySQL_Temp = String.Format("INSERT INTO SERVER_OBJECTS_FIELDPROPERTIES_HEAD (ObjectName, ObjectType) VALUES ('{0}', '{1}')", Object_Name, OUT_Object_Type)
                        DC.LocalDB_SEL.RunSQL(MySQL_Temp)
                    End If
                End If
            End Using
        End If

        RS_GET_FIELDPROPERTIES_FROM_SERVER_OBJECT = OUT
    End Function
    Public Sub RS_SELECT_ALL(ByVal RS_ID As Long, ByVal Selected As Boolean)
        DC.Qdf_RunSQL(String.Format("UPDATE RS_L SET Selected = {1} FROM RS_L WITH (READUNCOMMITTED) WHERE RS_ID = {0}", RS_ID, IIf(Selected, "1", "0")), 0)
    End Sub
    Public Function RS_COPY(ByVal NameOfNewRS As String, ByVal From_RS_ID As Long) As Long
        Dim OUT As Long = 0

        If InitGlobals() Then
            Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

            DC.Qdf_set_SP(sqlComm, "RS_COPY")
            DC.Qdf_AddParameter(sqlComm, "@From_RS_ID", SqlDbType.Int, , , , , From_RS_ID)
            DC.Qdf_AddParameter(sqlComm, "@NameOfNewRS", SqlDbType.NVarChar, , 255, NameOfNewRS)
            DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)

            CURSOR_SHOW_WAIT()
            Try
                DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG)
            Catch ex As Exception
                DoErrorMsgBox("FP_App.RS_COPY", Err.Number, Err.Description)
            End Try
            CURSOR_SHOW_DEFAULT()
            OUT = sqlComm.Parameters.Item("@RetValue").Value
            If OUT = 0 Then
                DoErrorMsgBox("FP_App.RS_COPY", 0, sqlComm.Parameters("@ErrText").Value)
            End If
            sqlComm.Dispose()
        End If

        RS_COPY = OUT
    End Function

    Public Function RS_SET(P As Struct_RS, ByRef P_OUT As Struct_RS_OUT) As Boolean
        Dim OUT As Boolean = False

        P_OUT = New Struct_RS_OUT

        If InitGlobals() Then
            P.RS_WHERE = Text_Replace_Standard_Params(P.RS_WHERE)

            Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

            DC.Qdf_set_SP(sqlComm, "RS_New")
            DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            DC.Qdf_AddParameter(sqlComm, "@Obj_Name", SqlDbType.NVarChar, , 255, P.RS_Obj_Name)
            DC.Qdf_AddParameter(sqlComm, "@HWND", SqlDbType.Int, , , , , P.HWND)
            DC.Qdf_AddParameter(sqlComm, "@ID_FieldName", SqlDbType.NVarChar, , 255, P.RS_ID_FieldName)
            DC.Qdf_AddParameter(sqlComm, "@FROM", SqlDbType.NVarChar, , -1, P.RS_FROM)
            DC.Qdf_AddParameter(sqlComm, "@WHERE", SqlDbType.NVarChar, , -1, P.RS_WHERE)
            DC.Qdf_AddParameter(sqlComm, "@GROUPBY", SqlDbType.NVarChar, , -1, P.RS_GROUPBY)
            DC.Qdf_AddParameter(sqlComm, "@ORDERBY", SqlDbType.NVarChar, , -1, P.RS_ORDERBY)
            DC.Qdf_AddParameter(sqlComm, "@Selected", SqlDbType.Bit, , , , , P.Selected)
            DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, -1)
            DC.Qdf_AddParameter(sqlComm, "@MaxRecords", SqlDbType.Int, ParameterDirection.InputOutput, , , , P.MaxRecords)

            CURSOR_SHOW_WAIT()
            Try
                DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG)
            Catch ex As Exception
                DoErrorMsgBox("FP_App.RS_SET", Err.Number, Err.Description)
            End Try
            CURSOR_SHOW_DEFAULT()

            With P_OUT
                .RS_ID = sqlComm.Parameters.Item("@RetValue").Value
                If .RS_ID = 0 Then
                    DoErrorMsgBox("FP_App.RS_SET", 0, sqlComm.Parameters("@ErrText").Value)
                Else
                    .RECORDCOUNT = sqlComm.Parameters.Item("@MaxRecords").Value
                    OUT = True
                End If
            End With
        End If

        RS_SET = OUT
    End Function

    Public Sub SET_DATA_FORMATS(Lang As String)
        Dim MySQL As String = String.Format("SELECT Date_Separator, Date_Format_STR, Year_Format, Time_Separator, Decimal_Point_Character FROM Lang_Formats WHERE Code = '{0}'", Lang)
        Dim DRow As DataRow = DC.Qdf_get_DataRow(MySQL)

        If Not (DRow Is Nothing) Then
            SET_DATA_FORMATS(DRow!Date_Separator, DRow!Date_Format_STR, DRow!Year_Format, DRow!Time_Separator, DRow!Decimal_Point_Character)
        End If
    End Sub

    Private Sub SET_DATA_FORMATS(ByVal MyFormat_Date_Splitter As String, ByVal MyFormat_Date_Order As String, ByVal MyFormat_Date_LengthOfYear As Long, ByVal MyFormat_Time_Splitter As String, ByVal MyFormat_Float_DecimalPoint As String)
        Format_Date_Splitter = MyFormat_Date_Splitter
        Format_Date_Order = MyFormat_Date_Order
        Format_Date_LengthOfYear = MyFormat_Date_LengthOfYear
        Format_Time_Splitter = MyFormat_Time_Splitter
        Format_Float_DecimalPoint = MyFormat_Float_DecimalPoint

        Dim Curr_Data_Formats As String = Mid(Format_Date_Splitter + Space(1), 1, 1) +
                                          Mid(Format_Date_Order + Space(3), 1, 3) +
                                          Format_Date_LengthOfYear.ToString +
                                          Mid(Format_Time_Splitter + Space(1), 1, 1) +
                                          Mid(Format_Float_DecimalPoint + Space(1), 1, 1)

        PFDinsertOrUpdate("DATA_FORMATS", Curr_Data_Formats)
    End Sub
    Public Function setLanddialog(ByVal MySprache As String) As Boolean
        Dim OUT As Boolean = False
        Try
            LandDialog = MySprache
            If (LandDialog = String.Empty) Or (LandDialog = "##???##") Then
                Dim Languages As New FP_SELECT_Language(Me)
                Languages.ShowDialog()
                Languages.Dispose()
            End If
            If (LandDialog = String.Empty) Or (LandDialog = "##???##") Then
                OUT = False
            Else
                PFDinsertOrUpdate("LANDDIALOG", LandDialog)
                OUT = True
            End If
        Catch
            '
        End Try
        Return OUT
    End Function

    Public Function setFunctionLock(FunctionID As ENUM_FUNCTIONLOCK_IDs, ByRef InOut_For_Seconds As Integer) As Boolean
        Dim OUT As Boolean = False

        Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

        With DC
            .Qdf_set_SP(sqlComm, "setFunctionLock")
            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            .Qdf_AddParameter(sqlComm, "@FunctionID", SqlDbType.Int, , , , , FunctionID)
            .Qdf_AddParameter(sqlComm, "@For_Seconds", SqlDbType.Int, , , , , InOut_For_Seconds)
        End With

        CURSOR_SHOW_WAIT()
        Try
            DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG)
            OUT = nz(sqlComm.Parameters("@RetValue").Value, False)
            If OUT = False Then
                InOut_For_Seconds = nz(sqlComm.Parameters("@For_Seconds").Value, 0)
            End If

        Catch ex As Exception
            CURSOR_SHOW_DEFAULT()
            DoErrorMsgBox("FP_App.setFunctionLock", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        Return OUT
    End Function

    Public Sub clearFunctionLock(FunctionID As ENUM_FUNCTIONLOCK_IDs)
        Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

        With DC
            .Qdf_set_SP(sqlComm, "clearFunctionLock")
            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            .Qdf_AddParameter(sqlComm, "@FunctionID", SqlDbType.Int, , , , , FunctionID)
        End With

        CURSOR_SHOW_WAIT()
        Try
            DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG)

        Catch ex As Exception
            CURSOR_SHOW_DEFAULT()
            DoErrorMsgBox("FP_App.clearFunctionLock", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()
    End Sub

    Public Function setRecordLock(ByVal MyRecordID As Long, Optional ByVal mitDialog As Boolean = True, Optional ByVal myDialogNr As Long = 0, Optional ByVal MyFormName$ = "", Optional ByVal MyTableName As String = "") As Boolean
        Dim OUT As Boolean = False

        If InitGlobals() Then
            If myDialogNr = 0 Then
                myDialogNr = 1000
            End If

            Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()
            Dim Result As Boolean

            DC.Qdf_set_SP(sqlComm, "setRecordLock")
            DC.Qdf_AddParameter(sqlComm, "@MyTerminal", SqlDbType.NVarChar, , 10, Terminal$)
            DC.Qdf_AddParameter(sqlComm, "@MyRecordID", SqlDbType.Int, , , , , MyRecordID)
            DC.Qdf_AddParameter(sqlComm, "@MyFormName", SqlDbType.NVarChar, , 30, MyFormName$)
            DC.Qdf_AddParameter(sqlComm, "@MyTableName", SqlDbType.NVarChar, , 128, MyTableName$)

            CURSOR_SHOW_WAIT()

            Try
                Result = DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG)
            Catch ex As Exception
                Result = False
                DoErrorMsgBox("FP_App.setRecordLock", Err.Number, Err.Description)
            End Try

            CURSOR_SHOW_DEFAULT()

            If Result Then
                OUT = IIf(nz(sqlComm.Parameters("@RetValue").Value, 0) = -1, True, False)
                If OUT = False Then
                    If mitDialog Then
                        DoMyMsgBox(myDialogNr)
                    End If
                End If
            End If
            sqlComm.Dispose()
        End If

        setRecordLock = OUT
    End Function

    Private Function DispoDEF_GET(ByVal Identifier As String, ByRef Dispo_DEF As Struct_ZDISPO_DEF) As Boolean
        Dim OUT As Boolean = False
        Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

        Dispo_DEF = New Struct_ZDISPO_DEF

        With DC
            .Qdf_set_SP(sqlComm, "DispoDEF_GET")
            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 100, Terminal)
            .Qdf_AddParameter(sqlComm, "@Identifier", SqlDbType.NVarChar, , 100, Identifier)
            .Qdf_AddParameter(sqlComm, "@StoredProc_1", SqlDbType.NVarChar, ParameterDirection.Output, 128)
            .Qdf_AddParameter(sqlComm, "@Fix_WHERE", SqlDbType.NVarChar, ParameterDirection.Output, -1)
            .Qdf_AddParameter(sqlComm, "@DoFilter", SqlDbType.NVarChar, ParameterDirection.Output, 128)
            .Qdf_AddParameter(sqlComm, "@DoFilter_WhereQuery", SqlDbType.NVarChar, ParameterDirection.Output, 128)
            .Qdf_AddParameter(sqlComm, "@Simple_Select", SqlDbType.NVarChar, ParameterDirection.Output, 128)
            .Qdf_AddParameter(sqlComm, "@StoredProc_2", SqlDbType.NVarChar, ParameterDirection.Output, 128)
            .Qdf_AddParameter(sqlComm, "@Report_Name", SqlDbType.NVarChar, ParameterDirection.Output, 128)
            .Qdf_AddParameter(sqlComm, "@Report_SQL", SqlDbType.NVarChar, ParameterDirection.Output, -1)
            .Qdf_AddParameter(sqlComm, "@Report_OpenType", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@Excel_export", SqlDbType.NVarChar, ParameterDirection.Output, -1)
            .Qdf_AddParameter(sqlComm, "@Txt_export", SqlDbType.NVarChar, ParameterDirection.Output, -1)
            .Qdf_AddParameter(sqlComm, "@FilePath", SqlDbType.NVarChar, ParameterDirection.Output, -1)
            .Qdf_AddParameter(sqlComm, "@StoredProc_3", SqlDbType.NVarChar, ParameterDirection.Output, 128)
            .Qdf_AddParameter(sqlComm, "Email", SqlDbType.NVarChar, ParameterDirection.Output, 128)
            .Qdf_AddParameter(sqlComm, "@Next_Identifier", SqlDbType.NVarChar, ParameterDirection.Output, 100)

            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.VarChar, ParameterDirection.Output, 255)
            .Qdf_AddParameter(sqlComm, "@ErrParams", SqlDbType.VarChar, ParameterDirection.Output, 255)
        End With

        CURSOR_SHOW_WAIT()
        Try
            OUT = DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrParams")
        Catch ex As Exception
            OUT = False
            DoErrorMsgBox("FPApp.DispoDEF_GET", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        If OUT Then
            With Dispo_DEF
                .Identifier = Identifier

                .StoredProc_1 = sqlComm.Parameters("@StoredProc_1").Value
                .Fix_WHERE = sqlComm.Parameters("@Fix_WHERE").Value
                .DoFilter = sqlComm.Parameters("@DoFilter").Value
                .DoFilter_WhereQuery = sqlComm.Parameters("@DoFilter_WhereQuery").Value
                .Simple_Select = sqlComm.Parameters("@Simple_Select").Value
                .StoredProc_2 = sqlComm.Parameters("@StoredProc_2").Value
                .Report_Name = sqlComm.Parameters("@Report_Name").Value
                .Report_SQL = sqlComm.Parameters("@Report_SQL").Value
                .Report_OpenType = sqlComm.Parameters("@Report_OpenType").Value
                .Excel_export = sqlComm.Parameters("@Excel_export").Value
                .Txt_export = sqlComm.Parameters("@Txt_export").Value
                .FilePath = sqlComm.Parameters("@FilePath").Value
                .StoredProc_3 = sqlComm.Parameters("@StoredProc_3").Value
                .Next_Identifier = sqlComm.Parameters("@Next_Identifier").Value
            End With
        End If

        DispoDEF_GET = OUT
    End Function

    Public Function SKIN_GET_IMAGE(ImageName_With_SKIN As String) As Bitmap
        Dim OUT As Bitmap = Nothing
        Dim asm As Reflection.Assembly = Nothing
        Dim ImageName As String = ""

        If SKIN_getASM_And_OBJECTNAME(ImageName_With_SKIN, asm, ImageName) Then
            Try
                OUT = New Bitmap(asm.GetManifestResourceStream(ImageName))

            Catch ex As Exception
                DoErrorMsgBox("FP_App.SKIN_GET_IMAGE", 0, String.Format("Image '{0}' not found.", ImageName_With_SKIN))
            End Try
        End If

        Return OUT
    End Function

    Public Sub SKIN_DRAW_BACKGROUND(ByRef graphics As System.Drawing.Graphics, ByRef clientRectangle As System.Drawing.Rectangle, Optional ByVal BackgroundImageName As String = "Background_SelexpedWaves.png")
        'a hatterkepet kiterjesztessel egyutt kell megadni! (Peldaul: 'kep.png'
        'A 'kep.png' formatum azt jelenti, hogy a kep.png a SEL_FP.dll-ben van benne.
        'A SEL_FP.dll-ben valaszthato kepek a SEL_FP/FP_SKIN konyvtarban talalhatok.
        'Ha sajat projectunkben hoztunk letre kepeket, akkor azt a kovetkezo formaban kell megadni:
        '            "<My.Application.Info.AssemblyName>..kep.png"
        'A kod felismeri es kicsereli az "<APP_NAME>" elotagot a project sajat elotagjara. Vagyis helyes a kovetkezo kifejezes: "<APP_NAME>..kep.png"
        'A project-ek APP_NAME (=Application name) elotagjat a Project/Properties menupont alatt az "Application" fulon lehet beallitani illetve kiolvasni.
        'Ne felejtsd el, hogy a kepeket, amiket hozzaadsz a project-hez, a property ablakban allitsd "Embended resource"-ra, maskeppen NEM fog mukodni!!!

        Dim asm As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly()

        If SKIN_getASM_And_OBJECTNAME(BackgroundImageName, asm, BackgroundImageName) Then
            Dim backGroundImage As New Bitmap(asm.GetManifestResourceStream(BackgroundImageName))

            graphics.DrawImage(backGroundImage, clientRectangle, New Rectangle(0, 0, backGroundImage.Width, backGroundImage.Height), GraphicsUnit.Pixel)
        End If
    End Sub

    Public Function SQL_getFROM_ELEMENTS(ByVal SQL_SELECT_0 As String, ByVal SQL_SELECT As String, ByVal SQL_FROM As String, ByVal SQL_WHERE As String, ByVal SQL_GROUPBY As String, ByVal SQL_ORDERBY As String, Optional FPc As FP_Control = Nothing) As String
        Dim OUT As String

        SQL_SELECT_0 = IIf(SQL_SELECT_0 > "", " SELECT " + SQL_SELECT_0 + " UNION ALL ", "")
        SQL_WHERE = IIf(SQL_WHERE > "", " WHERE " + SQL_WHERE, "")
        SQL_GROUPBY = IIf(SQL_GROUPBY > "", " GROUP BY " + SQL_GROUPBY, "")
        SQL_ORDERBY = IIf(SQL_ORDERBY > "", " ORDER BY " + SQL_ORDERBY, "")

        OUT = String.Format("{0}SELECT {1} FROM {2}{3}{4}{5}", SQL_SELECT_0, SQL_SELECT, SQL_FROM, SQL_WHERE, SQL_GROUPBY, SQL_ORDERBY)

        If Not (FPc Is Nothing) Then
            OUT = Text_Replace_Standard_Params(OUT, FPc.FP)
        Else
            OUT = Text_Replace_Standard_Params(OUT)
        End If

        SQL_getFROM_ELEMENTS = OUT
    End Function
    Public Function SQL_OPEN()
        Return DC.CNN_OPEN()
    End Function

    Public Function Text_Remove_IllegalCharacters_From_FileName(ByVal MyFileName As String) As String
        MyFileName = Replace(MyFileName, "\", "_")
        MyFileName = Replace(MyFileName, "/", "_")
        MyFileName = Replace(MyFileName, "?", "_")
        MyFileName = Replace(MyFileName, ":", "_")
        MyFileName = Replace(MyFileName, ";", "_")
        MyFileName = Replace(MyFileName, "*", "_")
        MyFileName = Replace(MyFileName, "?", "_")
        MyFileName = Replace(MyFileName, "<", "_")
        MyFileName = Replace(MyFileName, ">", "_")
        MyFileName = Replace(MyFileName, Chr(34), "_")      'Idezojel (")
        MyFileName = Replace(MyFileName, Chr(13), "")
        MyFileName = Replace(MyFileName, Chr(10), "")
        MyFileName = Replace(MyFileName, "|", "_")
        MyFileName = Replace(MyFileName, " ", "_")
        MyFileName = Replace(MyFileName, Chr(9), "_")
        MyFileName = Replace(MyFileName, ",", "_")

        Text_Remove_IllegalCharacters_From_FileName = MyFileName
    End Function

    Function Tbl_IsRecordsetEmpty(ByVal SQL As String, Optional ByVal mitDialog As Boolean = True, Optional ByVal MyDialNum As Long = 10208) As Boolean
        Dim OUT As Boolean = True

        If SQL.Trim > "" Then
            Dim DT As DataTable = Nothing

            OUT = DC.Qdf_Fill_DT(SQL, DT)

            If OUT = True Then
                OUT = (DT.Rows.Count = 0)
            End If

            If OUT = False Then
                If mitDialog Then
                    DoMyMsgBox(MyDialNum)
                End If
            End If
        End If

        Tbl_IsRecordsetEmpty = OUT
    End Function

    Function Tbl_IsRecordsetNotEmpty(ByVal SQL As String, Optional ByVal mitDialog As Boolean = True, Optional ByVal MyDialNum As Long = 10212, Optional ByVal MyDialParam As String = "") As Boolean
        Dim OUT As Boolean = False

        If SQL.Trim > "" Then
            Dim DT As DataTable = Nothing

            OUT = DC.Qdf_Fill_DT(SQL, DT)

            If OUT = True Then
                OUT = (DT.Rows.Count > 0)
            End If

            If OUT = False Then
                If mitDialog Then
                    DoMyMsgBox(MyDialNum, MyDialParam)
                End If
            End If
        End If

        Tbl_IsRecordsetNotEmpty = OUT

    End Function

    Public Function Text_getDialogText(ModuleIdentifier As String, DialNum As Integer, MsgParams As String) As String
        Dim OUT As String
        Dim Texts(4) As String
        Dim MySQL As String
        Dim DT As DataTable = Nothing

        If ModuleIdentifier = "" Then
            MySQL = String.Format("SELECT Text1 FROM VB_SEQ WHERE [Language] = '{0}' And SEQ_Key = {1}", LandDialog, "'DIALOG" + Format(DialNum, "000000") + "'")

            If DC.P_USE_LocalDB Then
                DC.LocalDB_SEL.Fill_DT(MySQL, DT)
            Else
                DC.Qdf_Fill_DT(MySQL, DT)
            End If

            If DT.Rows.Count > 0 Then
                For i = 0 To DT.Rows.Count - 1
                    Texts(i) = Text_ParametersErsetzen(DT.Rows(i)!Text1, MsgParams)
                Next
            End If
        Else
            MySQL = String.Format("SELECT VALUE FROM RS_Texts{0}_View WHERE LANG = '{1}' And MODULE = '{2}' And ServerObject_Prefix = '' AND SubPrefix = '' AND IDX = {3} And GROUP_Code = 'DIALOG' And CTRL_Code = '' And SUB_Code = ''", IIf(Is_DEBUG_MODE, "_DEBUG", ""), LandDialog, ModuleIdentifier, DialNum)

            If DC.P_USE_LocalDB Then
                DC.LocalDB_SEL.Fill_DT(MySQL, DT)
            Else
                DC.Qdf_Fill_DT(MySQL, DT)
            End If

            If DT.Rows.Count = 1 Then
                Texts = Split(Text_ParametersErsetzen(DT.Rows(0)!VALUE, MsgParams), "|")
            End If

            ReDim Preserve Texts(4)
        End If

        OUT = IIf(Texts(0) > "", vbCrLf + Texts(0), "") +
              IIf(Texts(1) > "", vbCrLf + Texts(1), "") +
              IIf(Texts(2) > "", vbCrLf + Texts(2), "") +
              IIf(Texts(3) > "", vbCrLf + Texts(3), "")

        Return OUT
    End Function

    Public Function Text_get_Encrypted_Text(MyText As String) As String
        Randomize()

        Dim OUT As String = "#MD5#"
        Dim RndNum As Integer = CInt(Rnd() * 26)
        Dim MaxASC As Integer = 0
        Dim MinASC As Integer = Integer.MaxValue

        If MyText = "" Then
            MaxASC = 90
            MinASC = 65
        Else
            For p As Integer = 1 To Len(MyText)
                Dim CurrAsc = AscW(Mid(MyText, p, 1))
                If MaxASC < CurrAsc Then
                    MaxASC = CurrAsc
                End If
                If MinASC > CurrAsc Then
                    MinASC = CurrAsc
                End If
            Next

            For i As Integer = 1 To RndNum \ 5
                OUT += Chr(CInt(Rnd() * 26) + 65)
            Next

            Dim TXT_Array(2 * Len(MyText)) As String

            For i As Integer = 1 To Len(TXT_Array)
                TXT_Array(i) = Asc(0)
            Next

            Dim FirstPos = RndNum \ Len(MyText)

            Dim CurrPos = FirstPos

            For i As Integer = 1 To Len(MyText)
                Dim Jumping As Integer = CInt(Rnd() * (MaxASC - MinASC)) + 1
                Dim NewPos As Integer = (CurrPos + Jumping) \ Len(MyText)

                If NewPos = 0 Then
                    NewPos = 1
                    Jumping += 1
                End If

                Do While AscW(TXT_Array(NewPos * 2)) <> 0
                    Jumping += 1
                    NewPos = (NewPos + 1) \ Len(MyText)
                    If NewPos = 0 Then
                        NewPos = 1
                        Jumping += 1
                    End If
                Loop

                TXT_Array(CurrPos) = ChrW(MinASC + NewPos)
            Next

            CurrPos = FirstPos
            For p As Integer = 1 To Len(OUT)
                Dim CurrAscII As Integer = AscW(Mid(MyText, p, 1)) + RndNum

                If CurrAscII > MaxASC Then
                    CurrAscII -= (MaxASC - MinASC - 1)
                End If

                TXT_Array(CurrPos + 1) = ChrW(CurrAscII)
                CurrPos = (CurrPos + AscW(TXT_Array(CurrPos)) - MinASC) \ Len(MyText)
            Next p
        End If

        OUT += Chr(MaxASC + RndNum) + Chr(MinASC + RndNum) + Chr(RndNum) + Chr(CInt(Rnd() * 26) + 65) + Chr(CInt(Rnd() * 26) + 65)

        Return OUT
    End Function

    Public Function Text_get_From_Resource(ByVal Res_Code_Prefix As String, ByVal Res_Code_Num As Long, ByRef OUT_Text As String, Optional ByVal ProjectName As String = "SEL_FP", Optional ByVal ResourceBaseName As String = "SEL_FP", Optional ByVal Params() As String = Nothing) As Boolean
        Dim OUT As Boolean = False

        OUT_Text = ""

        If Trim(LandDialog) > "" Then
            Dim Code = Res_Code_Prefix + "_" + Res_Code_Num.ToString + "_" + LandDialog
            Try

                OUT_Text = My.Resources.ResourceManager.GetString(Code)
                If Not (Params Is Nothing) Then
                    Dim i As Integer

                    For i = 0 To UBound(Params)
                        Dim ParamName As String = "@" + i.ToString

                        OUT_Text = Replace(OUT_Text, ParamName, Params(i))
                    Next
                End If

                OUT = True

            Catch ex As Exception
                'Nothing to do
            End Try
        End If

        Return OUT
    End Function
    Public Function Text_getParamFromLine(ByVal MyStr As String, ByVal ParamNr As Long,
                               Optional ByVal BisZumStrEndelesen As Boolean = False,
                               Optional ByVal OhneTrim As Boolean = False,
                               Optional ByVal FixTrennzeichen As String = "") As String
        Dim OUT As String = String.Empty
        Dim w, p0, wPosStart, wPosEnd, wLen As Integer
        Dim Ergebnis As String
        Try
            wLen = Len(MyStr)
            w = 0
            wPosStart = 0
            wPosEnd = 0
            Do
                wPosStart = wPosEnd + 1
                wPosEnd = wLen + 1
                If FixTrennzeichen <> String.Empty Then
                    p0 = InStr(wPosStart, MyStr, FixTrennzeichen) : If p0 > 0 And wPosEnd > p0 Then wPosEnd = p0
                Else
                    p0 = InStr(wPosStart, MyStr, ";") : If p0 > 0 And wPosEnd > p0 Then wPosEnd = p0
                    p0 = InStr(wPosStart, MyStr, "|") : If p0 > 0 And wPosEnd > p0 Then wPosEnd = p0
                End If
                w += 1
            Loop While wPosEnd < wLen And w < ParamNr
            If w < ParamNr Then
                Ergebnis = String.Empty
            Else
                If BisZumStrEndelesen Then
                    Ergebnis = Mid(MyStr, wPosStart)
                Else
                    Ergebnis = Mid(MyStr, wPosStart, wPosEnd - wPosStart)
                End If
            End If
            If Not OhneTrim Then Ergebnis = Trim(Ergebnis)
            OUT = Ergebnis
        Catch ex As Exception
            DoErrorMsgBox("FP_App.Text_GetParamFromLine", Err.Number, Err.Description)
        End Try
        Return OUT
    End Function
    Public Function Text_getAnzahlReihen(ByVal MyText As String) As Integer
        Dim OUT As Integer = 0
        Dim AktPos As Integer
        Dim AktReihe As Long
        Try
            AktPos = -1
            AktReihe = 0
            Do
                AktPos += 2
                AktReihe += 1
                AktPos = InStr(AktPos, MyText, vbCrLf)
            Loop While AktPos > 0
            OUT = AktReihe
        Catch ex As Exception
            DoErrorMsgBox("FP_App.getAnzahlReihen", Err.Number, Err.Description)
        End Try
        Return OUT
    End Function

    Public Function Text_getTextFile(ByVal FileName As String) As String
        Dim oRead As System.IO.StreamReader
        Dim OUT As String = ""
        Try
            If Not Vorhanden(FileName) Then
                'nothing to do
            Else
                oRead = File.OpenText(FileName)
                OUT = oRead.ReadToEnd
                oRead.Close()
            End If

        Catch ex As Exception
            OUT = ""
            DoErrorMsgBox("FP_App.Text_getTextFile", Err.Number, Err.Description)
        End Try
        Return OUT
    End Function

    Public Function MENUITEM_IsHidden(MenuItem_Tag As String) As Boolean
        MENUITEM_PROPS_LOAD()

        Dim Hidden_for_Current_Level As Boolean = (MENUITEM_PROPS_LEVELS.Select(String.Format("Tag = '{0}'", MenuItem_Tag)).Count > 0)
        Dim Hidden_for_Current_Group As Boolean = (MENUITEM_PROPS_GROUPS.Select(String.Format("Tag = '{0}'", MenuItem_Tag)).Count > 0)

        MENUITEM_IsHidden = (Hidden_for_Current_Level And Hidden_for_Current_Group)
    End Function

    Public Sub MENUITEM_PROPS_LOAD(Optional MustRefresh As Boolean = False)
        If MustRefresh = True Or MENUITEM_PROPS_Loaded = False Then
            Dim SQL_LEVELS As String = String.Format("SELECT * FROM MENU_SETUP_UserLevels WHERE UserLevels_ID = {0}", UserStufe)
            Dim SQL_GROUPS As String = String.Format("SELECT * FROM MENU_SETUP_UserGroups WHERE UserGroups_ID = {0}", UserGruppe)

            DC.Qdf_Fill_DT(SQL_LEVELS, MENUITEM_PROPS_LEVELS)
            DC.Qdf_Fill_DT(SQL_GROUPS, MENUITEM_PROPS_GROUPS)

            MENUITEM_PROPS_Loaded = True
        End If
    End Sub

    Public Function NachsteNummerVergeben() As Long
        Dim sqlComm As SqlClient.SqlCommand = Nothing
        Dim SpName As String = "NachsteNummerVergeben"
        Dim RetValue As String
        Try
            DC.Qdf_set_SP(sqlComm, SpName)
            DC.Qdf_AddParameter(sqlComm, "ReturnValue", SqlDbType.Int, 6)
            sqlComm.ExecuteNonQuery()
            RetValue = sqlComm.Parameters.Item("@RetValue").Value
            Return RetValue
        Catch ex As Exception
            DoErrorMsgBox("FP_App.NachsteNummerVergeben", Err.Number, Err.Description)
            Return 0
        End Try
    End Function
    Public Function Tr_getNewTransactID(ByVal ProcedureName As String, ByVal RecordID As Long, Optional ByVal ParentTransactID As Long = 0, Optional ByVal ServerAsTerminal As Boolean = False) As Long
        Dim OUT As Long = 0
        Dim Qdf As New SqlClient.SqlCommand
        Using Qdf
            Try
                If InitMinimalGlobals() Then
                    DC.Qdf_set_SP(Qdf, "Tr_getNewTransactID")
                    DC.Qdf_AddParameter(Qdf, "Terminal", SqlDbType.VarChar, , 10, IIf(ServerAsTerminal, "SERVER", Terminal))
                    DC.Qdf_AddParameter(Qdf, "ProcedureName", SqlDbType.VarChar, , 128, ProcedureName)
                    DC.Qdf_AddParameter(Qdf, "RecordID", SqlDbType.Int, , , , , RecordID)
                    DC.Qdf_AddParameter(Qdf, "ParentTransactID", SqlDbType.Int, , , , , ParentTransactID)
                    DC.Qdf_Execute("", Qdf, , ENUM_ERRDIAL_TYPE.NODIALOG)
                    OUT = Qdf.Parameters("@RetValue").Value
                End If
            Catch ex As Exception
                DoErrorMsgBox("Mod_Class.Tr_getNewTransactID", Err.Number, Err.Description)
            End Try
        End Using
        Qdf.Dispose()
        Return OUT
    End Function
    Public Function Curr_get_Round_Digit(ByVal Curr_ID As Long) As Integer
        Dim OUT As Integer = 2
        Dim Criteria As String = String.Format("ID = {0}", Curr_ID)

        If DT_Currencies.Select(Criteria).Count > 0 Then
            OUT = DT_Currencies.Select(Criteria).First!Digits
        End If

        Return OUT
    End Function

    Public Sub CUST_DATA_EDIT_DIALOG(Cust_ID As Long)
        Dim P_MENU As New FP_MenuItem.Struct_FP_MenuItem_Params

        With P_MENU
            .Action = "SEL_CUST_DIALOG_GOTO_ID"
            .OpenArgs = Cust_ID
        End With

        RAISEEVENT_MenuItem_Activated(P_MENU)
    End Sub

    Public Function Curr_get_Code(ByVal Curr_ID As Long) As String
        Dim OUT As String = ""
        Dim Criteria As String = String.Format("ID = {0}", Curr_ID)

        If DT_Currencies.Select(Criteria).Count > 0 Then
            OUT = DT_Currencies.Select(Criteria).First!Code
        End If

        Return OUT
    End Function
    Public Function Curr_get_Sign(ByVal Curr_ID As Long) As String
        Dim OUT As String = ""
        Dim Criteria As String = String.Format("ID = {0}", Curr_ID)

        If DT_Currencies.Select(Criteria).Count > 0 Then
            OUT = DT_Currencies.Select(Criteria).First!Sign
        End If

        Return OUT
    End Function
    Public Function TRN_Params(Code As Integer) As Struct_TRN_Params
        Dim OUT As New Struct_TRN_Params With {
            .Code = Code
        }

        Dim Criteria As String = String.Format("Code = {0}", Code)

        If DT_TRN_Params.Select(Criteria).Count > 0 Then
            With DT_TRN_Params.Select(Criteria).First
                OUT.LNrCode = !LNrCode
                OUT.LNrSchlussel = !LNrSchlussel
                OUT.Param1 = !Param1
                OUT.OldArtikel_Mussfeld = (!OldArtikel_Mussfeld = 1)
                OUT.Artikel_Mussfeld = (!Artikel_Mussfeld = 1)
                OUT.Price_Code = !Price_Code
                OUT.Production = (!Production = 1)
                OUT.Multiclient = (!Multiclient = 1)
                OUT.Vorzeichen = !Vorzeichen
                OUT.EinAus = !EinAus
                OUT.Kreditor = !Kreditor
                OUT.Debitor = !Debitor
                OUT.GrundKreditorID = !GrundKreditorID
                OUT.GrundDebitorID = !GrundDebitorID
                OUT.Remark = !Bemerkung
            End With
        End If

        Return OUT
    End Function
    Public Function Form_setArfolyamMezok(ByVal ControlCode As ENUM_Arf_Codes,
                                ByVal OrigDatum As Date,
                                ByVal NichtGenau As Boolean,
                                ByVal MyVonWahrungen_ID As Long,
                                ByVal MyInWahrungen_ID As Long,
                                ByRef MyFremdBetragFeld As FP_Control,
                                ByRef MyKursDatumFeld As FP_Control,
                                ByRef MyKursEinheitFeld As FP_Control,
                                ByRef MyKursFeld As FP_Control,
                                ByRef MyBetragFeld As FP_Control) As Boolean

        'Beallitja az atadott arfolyammezok erteket.
        'Bemeneti ertekek:  ControlCode:    az eljaras tipusat adja meg.
        '                                   0: Nem kell csinalni semmit
        '                                   ARF_UJRA (1):         Az arfolyam datumat es az arfolyamot az OrigDatum parameter
        '                                                         alapjan meg kell hatarozni.
        '                                   ARF_CSAK_SZORZAS (2): A MyKursEinheitFeld es MyKursFeld mezokben
        '                                                         megadott arfolyamokat kell alapul venni.

        Dim ErgKursEinheit As Long
        Dim ErgKurs As Double
        Dim ErgKursDatum As Date
        Dim InWahrungRundung As Integer
        'Ellenorzesek:
        '-------------
        Try
            If ControlCode < 0 Or ControlCode > 2 Then
                DoMyMsgBox(827)
                Return False
            End If

            If ControlCode = 0 Then
                Return True
            End If

            If MyVonWahrungen_ID = 0 Then
                DoMyMsgBox(828)
                Return False
            End If

            If MyInWahrungen_ID = 0 Then
                DoMyMsgBox(829)
                Return False
            End If

            InWahrungRundung = Curr_get_Round_Digit(MyInWahrungen_ID)

            If IsNull(MyFremdBetragFeld.P_VALUE) Then
                DoMyMsgBox(830)
                Return False
            End If

            If ControlCode = ENUM_Arf_Codes.ARF_CSAK_SZORZAS Then
                If IsNull(MyKursDatumFeld.P_VALUE) Then
                    DoMyMsgBox(931)
                    Return False
                End If

                If MyKursEinheitFeld.P_VALUE < 1 Then
                    DoMyMsgBox(932)
                    Return False
                End If

                If MyKursFeld.P_VALUE < 0 Then
                    DoMyMsgBox(833)
                    Return False
                End If
            End If

            'Vegrehajtas
            '-----------
            If ControlCode = ENUM_Arf_Codes.ARF_UJRA Then
                KursErmitteln(1, MyVonWahrungen_ID, MyInWahrungen_ID, OrigDatum, 0, ErgKurs, ErgKursEinheit, ErgKursDatum, NichtGenau)
                MyKursDatumFeld.P_VALUE = ErgKursDatum
                MyKursEinheitFeld.P_VALUE = ErgKursEinheit
                MyKursFeld.P_VALUE = ErgKurs
            End If

            MyBetragFeld.P_VALUE = SelRound(MyFremdBetragFeld.P_VALUE / MyKursEinheitFeld.P_VALUE * MyKursFeld.P_VALUE, InWahrungRundung)
            Return True

        Catch ex As Exception
            DoErrorMsgBox("FP_App.Form_setArfolyamMezok", Err.Number, Err.Description)
            Return False
        End Try
    End Function

    Public Function KursErmitteln(ByVal Betrag As Double,
                        ByVal VonWahrungen_ID As Long,
                        ByVal InWahrungen_ID As Long,
                        ByVal Datum As Date,
                        ByRef ErgBetrag As Double,
                        ByRef ErgKurs As Double,
                        ByRef ErgKursEinheit As Long,
                        ByRef ErgKursDatum As Date,
                        Optional ByVal NichtGenau As Boolean = False) As Boolean
        Dim Qdf As New SqlClient.SqlCommand
        Try
            DC.Qdf_set_SP(Qdf, "KursErmitteln", 0)
            DC.Qdf_AddParameter(Qdf, "@Betrag", SqlDbType.Float, , , , , , Betrag)
            DC.Qdf_AddParameter(Qdf, "@VonWahrungen_ID", SqlDbType.Int, , , , , VonWahrungen_ID)
            DC.Qdf_AddParameter(Qdf, "@InWahrungen_ID", SqlDbType.Int, , , , , InWahrungen_ID)
            DC.Qdf_AddParameter(Qdf, "@Datum", SqlDbType.DateTime, , , , Datum)
            DC.Qdf_AddParameter(Qdf, "@ErgBetrag", SqlDbType.Float, ParameterDirection.Output)
            DC.Qdf_AddParameter(Qdf, "@ErgKurs", SqlDbType.Float, ParameterDirection.Output)
            DC.Qdf_AddParameter(Qdf, "@ErgKursEinheit", SqlDbType.Int, ParameterDirection.Output)
            DC.Qdf_AddParameter(Qdf, "@ErgKursDatum", SqlDbType.DateTime, ParameterDirection.Output)
            DC.Qdf_AddParameter(Qdf, "@NichtGenau", SqlDbType.SmallInt, , , , , NichtGenau)

            KursErmitteln = DC.Qdf_Execute("", Qdf, , ENUM_ERRDIAL_TYPE.NODIALOG)

            ErgBetrag = Qdf.Parameters("@ErgBetrag").Value
            ErgKurs = Qdf.Parameters("@ErgKurs").Value
            ErgKursEinheit = Qdf.Parameters("@ErgKursEinheit").Value
            ErgKursDatum = Qdf.Parameters("@ErgKursDatum").Value
        Catch ex As Exception
            DoErrorMsgBox("Sel_Globals.KursErmitteln", Err.Number, Err.Description)
            KursErmitteln = False
        End Try
        Qdf.Dispose()
    End Function
    Public Function SelRound(ByVal MyWert As Double, ByVal NumDigits As Long) As Double
        Dim OUT As Double = 0
        If NumDigits < 0 Or NumDigits > 5 Then
            MsgBox("SEL_gLOBALS.SelRound: Invalid argument.")
            OUT = MyWert
        Else
            MyWert = (Math.Abs(MyWert) + 0.000005) * Math.Sign(MyWert)

            Select Case NumDigits
                Case 0 : OUT = Int(MyWert + 0.5)
                Case 1 : OUT = Int(MyWert * 10 + 0.5) / 10
                Case 2 : OUT = Int(MyWert * 100 + 0.5) / 100
                Case 3 : OUT = Int(MyWert * 1000 + 0.5) / 1000
                Case 4 : OUT = Int(MyWert * 10000 + 0.5) / 10000
                Case 5 : OUT = Int(MyWert * 100000 + 0.5) / 100000
            End Select
        End If
        Return OUT
    End Function

    Function SendDatalineToTxtFormat(ByVal SQL As String, ByVal TxtFilePath As String, Optional TxtEncoding As System.Text.Encoding = Nothing) As Boolean
        Dim OUT As Boolean = True
        Dim AktFileName As String
        Dim OldFileName As String = ""
        Dim DT As DataTable = Nothing

        Const TXTLineName As String = "TXT_OUTPUT_LINE"
        Const TXTFileName As String = "TXT_OUTPUT_LINE_FILENAME"

        If Not Directory.Exists(TxtFilePath) Then
            SendDatalineToTxtFormat = False
            Exit Function
        End If

        If Not DC.Qdf_Fill_DT(SQL, DT) Then
            SendDatalineToTxtFormat = False
            Exit Function
        End If

        If Not DT.Columns.Contains(TXTLineName) Then
            SendDatalineToTxtFormat = False
            Exit Function
        End If

        If Not DT.Columns.Contains(TXTFileName) Then
            SendDatalineToTxtFormat = False
            Exit Function
        End If

        Dim i As Integer = 0
        Dim OutputText As String = ""

        For Each rows As DataRow In DT.Rows
            AktFileName = rows(TXTFileName)
            If AktFileName = "" Then
                SendDatalineToTxtFormat = False
                Exit Function
            End If

            If OldFileName <> "" And AktFileName <> OldFileName Then
                If Not Text_WriteToTextFile(OutputText, String.Format("{0}\{1}", TxtFilePath, OldFileName), TxtEncoding) Then
                    SendDatalineToTxtFormat = False
                    Exit Function
                End If

                OutputText = ""
            End If

            i += 1
            OutputText += rows(TXTLineName)

            If DT.Rows.Count = i Then
                If Not Text_WriteToTextFile(OutputText, String.Format("{0}\{1}", TxtFilePath, AktFileName), TxtEncoding) Then
                    SendDatalineToTxtFormat = False
                    Exit Function
                End If
            End If

            OldFileName = AktFileName
        Next

        SendDatalineToTxtFormat = OUT
    End Function

    Public Function Text_ParametersErsetzen(ByVal MyText As String, ByVal MyErsetz As String, Optional ByVal FixTrennzeichen As String = "|", Optional ByVal OhneTrim As Boolean = False) As String
        Dim OUT As String = ""
        Dim wPos As Integer
        Dim AktReihe As String
        Dim AktErsetz As String
        Dim AktReiheStart As Integer
        Dim AktReiheEnd As Integer
        Dim wParamNr As Long
        Dim wSEQ_List As String()
        Dim i As Long

        InitMinimalGlobals() 'nem lehet initglobals-ra cserelni, mert initrutinok hasznaljak ezt a funkciot.

        If Trim(MyText) <> String.Empty Then
            'Belso valtozok csereje
            MyText = Text_Replace_Standard_Params(MyText)
            wSEQ_List = Split(MyText, FixTrennzeichen)
            For i = 0 To UBound(wSEQ_List) - 1
                Dim SEQ_Start As Integer = InStr(1, wSEQ_List(i), "SEQ##")
                Dim SEQ_End As Integer = InStr(1, wSEQ_List(i), "##END")

                If SEQ_Start > 0 And SEQ_End > 0 Then
                    Dim SEQ_Key As String = Mid$(wSEQ_List(i), InStr(1, wSEQ_List(i), "SEQ##") + 5, InStr(1, wSEQ_List(i), "##END") - InStr(1, wSEQ_List(i), "SEQ##") - 5)
                    Dim SEQ_Key_P1 As String = Trim(Text_getParamFromLine(SEQ_Key, 1, , , ","))
                    Dim SEQ_Key_P2 As String = Trim(Text_getParamFromLine(SEQ_Key, 2, , , ","))

                    Dim MySQL As String = String.Format("SELECT Text1 FROM VB_SEQ_BY_Number WHERE SEQ_Key = '{0}' And Number = {1}", SEQ_Key_P1, SEQ_Key_P2)
                    Dim DRow As DataRow = DC.Qdf_get_DataRow(MySQL)

                    If Not (DRow Is Nothing) Then
                        MyText = Replace(MyText, Mid$(wSEQ_List(i), SEQ_Start, Len(wSEQ_List(i)) - SEQ_Start - (Len(wSEQ_List(i)) - SEQ_End - 5)), DRow!Text1)
                    End If
                End If
            Next
            AktReiheEnd = -1
            Do
                AktReiheStart = AktReiheEnd + 2
                AktReiheEnd = InStr(AktReiheStart, MyText, vbCrLf)
                If AktReiheEnd > 0 Then
                    AktReihe = Mid(MyText, AktReiheStart, AktReiheEnd - AktReiheStart)
                Else
                    AktReihe = Mid(MyText, AktReiheStart)
                End If
                wPos = InStr(1, AktReihe, "@")
                Do While wPos > 0
                    Dim Curr_Param As String = Mid(AktReihe, wPos + 1, 1)
                    If Curr_Param = "@" Then
                        wPos = InStr(wPos + 2, AktReihe, "@")
                    Else
                        wParamNr = Val(Curr_Param)
                        If wParamNr = 0 Then
                            wParamNr = Asc(UCase(Curr_Param)) - 55
                        End If
                        AktErsetz = Text_getParamFromLine(MyErsetz$, wParamNr, , OhneTrim, FixTrennzeichen$)
                        AktReihe = Left(AktReihe, wPos - 1) + AktErsetz + Mid(AktReihe, wPos + 2)
                        wPos = InStr(wPos + Len(AktErsetz), AktReihe, "@")
                    End If
                Loop
                OUT += AktReihe
                If AktReiheEnd > 0 Then OUT += vbCrLf
            Loop While AktReiheEnd > 0
        Else
            OUT = MyText
        End If

        Return OUT
    End Function

    Public Function TEXT_REPLACE_PLACEHOLDER_FROM_ORD_L(MySTR As String, ORD_L_ID As Long, Lang As String) As String
        Dim OUT As String = MySTR

        Dim MySQL As String = String.Format("SELECT dbo.FN_RTF_ORD_L('{0}', {1}, '{2}') Replaced_Text", SQLStr(MySTR), ORD_L_ID, Lang)
        Dim DRow As DataRow = DC.Qdf_get_DataRow(MySQL)

        Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

        DC.Qdf_set_SP(sqlComm, "TEXT_REPLACE_FROM_ORD_L")
        DC.Qdf_AddParameter(sqlComm, "@ORD_L_ID", SqlDbType.Int, , , , , ORD_L_ID)
        DC.Qdf_AddParameter(sqlComm, "@Text_with_placeholders", SqlDbType.NVarChar, , -1, MySTR)
        DC.Qdf_AddParameter(sqlComm, "@Lang", SqlDbType.NVarChar, , 3, LandDialog)
        DC.Qdf_AddParameter(sqlComm, "@OUT_Text", SqlDbType.NVarChar, ParameterDirection.Output, -1)

        CURSOR_SHOW_WAIT()
        Try
            If DC.Qdf_Execute("", sqlComm) Then
                OUT = nz(sqlComm.Parameters("@OUT_Text").Value, "")
            End If
        Catch ex As Exception
            CURSOR_SHOW_DEFAULT()
            DoErrorMsgBox("FP_App.TEXT_REPLACE_PLACEHOLDER_FROM_ORD_L", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        sqlComm.Dispose()
        Return OUT
    End Function

    Public Function Text_Replace_Standard_Params(ByVal Text As String, Optional MyFP As FP = Nothing) As String
        Dim OUT As String = Text

        OUT = Replace(OUT, "@LANDDIALOG", LandDialog)
        OUT = Replace(OUT, "@LANG", LandDialog)
        OUT = Replace(OUT, "@USERID", Trim(Str(SelUser)))
        OUT = Replace(OUT, "@USERNAME", UserName)
        OUT = Replace(OUT, "@USERNAME_FOREIGN_LANG_1", UserName_Foreign_Lang_1)
        OUT = Replace(OUT, "@USERNAME_FOREIGN_LANG_2", UserName_Foreign_Lang_2)
        OUT = Replace(OUT, "@EMAIL", UserEmail)
        OUT = Replace(OUT, "@USERPHONE1", UserPhone1)
        OUT = Replace(OUT, "@USERPHONE2", UserPhone2)
        OUT = Replace(OUT, "@TERMINAL", Terminal)
        OUT = Replace(OUT, "@TERMINAL_ID", Terminals_ID)
        OUT = Replace(OUT, "@LANDDIALOG", LandDialog)
        OUT = Replace(OUT, "@EUR", P.Currencies_Params.Curr_FC_Code)
        OUT = Replace(OUT, "@EIGENEWAHRUNG", P.Currencies_Params.Curr_LC_Code)
        OUT = Replace(OUT, "@CURR_LC", P.Currencies_Params.Curr_LC_Sign)
        OUT = Replace(OUT, "@CURR_FC", P.Currencies_Params.Curr_FC_Sign)
        OUT = Replace(OUT, "#VBCRLF#", vbCrLf)

        OUT = Replace(OUT, "@SELESTERNAME", SelesterName)
        OUT = Replace(OUT, "@SELESTERLAND", SelesterCity)
        OUT = Replace(OUT, "@SELESTERPLZ", SelesterZIP)
        OUT = Replace(OUT, "@SELESTERORT", SelesterCity)
        OUT = Replace(OUT, "@SELESTERSTRASSE", SelesterAddress)
        OUT = Replace(OUT, "@SELESTERTEL", SelesterTel)
        OUT = Replace(OUT, "@SELESTERFAX", SelesterFax)
        OUT = Replace(OUT, "@SELESTERHOTLINE", SelesterHotline)
        OUT = Replace(OUT, "@SELESTERHOMEPAGE", SelesterHomepage)
        OUT = Replace(OUT, "@SELESTEREMAIL", SelesterEMail)

        OUT = Replace(OUT, "@SELESTER_NAME", SelesterName)
        OUT = Replace(OUT, "@SELESTER_ADDRESS_CITY", SelesterCity)
        OUT = Replace(OUT, "@SELESTER_ADDRESS_ZIP", SelesterZIP)
        OUT = Replace(OUT, "@SELESTER_ADDRESS_CITY", SelesterCity)
        OUT = Replace(OUT, "@SELESTER_ADDRESS_ADDRESS", SelesterAddress)
        OUT = Replace(OUT, "@SELESTER_TEL", SelesterTel)
        OUT = Replace(OUT, "@SELESTER_HOTLINE", SelesterHotline)
        OUT = Replace(OUT, "@SELESTER_HOMEPAGE", SelesterHomepage)
        OUT = Replace(OUT, "@SELESTER_EMAIL", SelesterEMail)

        OUT = Replace(OUT, "@PROGRAMNAME", ProgramName)
        OUT = Replace(OUT, "@VERS", VERS)
        OUT = Replace(OUT, "@SRV", SRV)
        OUT = Replace(OUT, "@PRODUCT_NAME", P.PRODUCT_NAME)
        If Not (MyFP Is Nothing) Then
            OUT = Replace(OUT, "@RS_ID", MyFP.RS_ID)
            OUT = Replace(OUT, "@CURRENT_ID", MyFP.P_DATA_Current_ID)
        End If

        Text_Replace_Standard_Params = OUT
    End Function
    Public Function Text_TextVonSEQ(ByVal Text As String, Optional ByRef ControlTipText As String = "", Optional ByRef StatusText As String = "") As String
        Dim OUT As String = Text
        Dim MyArt As String = ""
        Dim MyNummer As String = ""

        Text = Trim(Text)

        ControlTipText = String.Empty
        StatusText = String.Empty

        If UCase(Left(Text, 3)) = "SEQ" Then
            MyArt = Text_getParamFromLine(Text, 2, , , ",")
            MyNummer = Text_getParamFromLine(Text, 3, , , ",")
            If Trim(MyNummer) = String.Empty Then MyNummer = "1"

            Dim MySQL As String = String.Format("SELECT * FROM VB_SEQ_BY_Number WHERE SEQ_Key = '{0}' And Number = {1} And Language = '{2}'", MyArt, MyNummer, LandDialog)
            Dim DRow As DataRow = DC.Qdf_get_DataRow(MySQL)
            If Not (DRow Is Nothing) Then
                With DRow
                    ControlTipText = nz(!Text4, "")
                    StatusText = nz(!Text3, "")
                    OUT = nz(!Text1, "")
                End With
            End If
        End If

        Return OUT
    End Function
    Public Function Text_Without_Enter(ByVal tb As TextBox, ByVal lastKeycode As Integer) As String
        Dim OUT As String
        Dim s As String = tb.Text
        OUT = IIf(lastKeycode = 13, s.Substring(0, s.Length - 2), s)
        Return OUT
    End Function

    Public Function Text_WriteToTextFile(ByVal MyText As String, ByVal MyFile As String, Optional MyEncoding As System.Text.Encoding = Nothing) As Boolean
        Dim OUT As Boolean = True
        Try
            If MyEncoding Is Nothing Then
                MyEncoding = System.Text.Encoding.UTF8
            End If

            Dim fs As New FileStream(MyFile, FileMode.Create, FileAccess.Write, FileShare.None)
            fs.Flush()
            fs.Close()

            Dim sw As New StreamWriter(MyFile, True, MyEncoding)

            With sw
                .Write(MyText)
                .Flush()
                .Close()
            End With

        Catch ex As Exception
            OUT = False
            DoErrorMsgBox("FP_App.Text_WriteToTextFile", Err.Number, Err.Description)
        End Try
        Return OUT
    End Function

    Public Sub Tr_WriteError_To_Transact_Errors(ByVal TransactID As Long, ByVal ProcedureName$, ByVal RecordID As Long, ByVal ErrorCode As Long, ByVal ErrorText$)
        'Elozetesen lefoglalt TransactID-k:
        '
        '-1:    A hibat a DoErrorMsgBox generalja a kovetkezo parameterekkel:
        '       ProcedureName   = DoErrorMsgBox.ErrPlace
        '       RecordID        = 0
        '       ErrorCode       = ErrNr
        '       ErrorText$      = Error$(ErrNr)
        Dim sqlComm As SqlClient.SqlCommand = Nothing
        Try
            DC.Qdf_set_SP(sqlComm, "Tr_WriteError_To_Transact_Errors")
            DC.Qdf_AddParameter(sqlComm, "TransactID", SqlDbType.Int, , , , , TransactID)
            DC.Qdf_AddParameter(sqlComm, "ProcedureName", SqlDbType.VarChar, , 128, ProcedureName$)
            DC.Qdf_AddParameter(sqlComm, "RecordID", SqlDbType.Int, , , , , RecordID)
            DC.Qdf_AddParameter(sqlComm, "ErrorCode", SqlDbType.Int, , , , , ErrorCode)
            DC.Qdf_AddParameter(sqlComm, "ErrorText", SqlDbType.VarChar, , -1, ErrorText$)
            DC.Qdf_AddParameter(sqlComm, "Terminal", SqlDbType.VarChar, , 10, Terminal)
            DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE)
        Catch ex As Exception
            'Nothing to do
        End Try
    End Sub

    Public Function SingleInstance(Optional ByVal WithActivate As Boolean = False) As Boolean
        Dim OUT As Boolean = True
        Dim cProcess As Process = Process.GetCurrentProcess()
        Dim aProcesses() As Process = Process.GetProcessesByName(cProcess.ProcessName)

        For Each process As Process In aProcesses
            If process.Id <> cProcess.Id Then
                If process.MainModule.FileName = cProcess.MainModule.FileName Then
                    OUT = False
                    If WithActivate Then
                        AppActivate(process.Id)
                    End If
                    Exit For
                End If
            End If
        Next

        SingleInstance = OUT
    End Function

    Public Function Countries_Get_Country_Type(ByVal Country_Code As String) As ENUM_COUNTRY_TYPE
        Dim CountryType As Integer = 0
        Dim Select_Crit As String = String.Format("SELECT Country_Type FROM Country WHERE Country_Code = '{0}'", Country_Code)
        Dim DRow As DataRow = DC.Qdf_get_DataRow(Select_Crit)
        If DRow IsNot Nothing Then
            CountryType = DRow.Item("Country_Type")
        End If
        Return CountryType
    End Function

    Public Sub DT_Countries_Refresh()
        DC.Qdf_Fill_DT("SELECT * FROM Country WITH (READUNCOMMITTED)", DT_Countries)
    End Sub

    Public Sub DT_Currencies_Refresh()
        DC.Qdf_Fill_DT("SELECT ID, KurzName Code, KurzZeichen Sign, IntRundung Digits FROM Wahrungen", DT_Currencies)
    End Sub

    Private Sub DT_TRN_Params_Refresh()
        DC.Qdf_Fill_DT("SELECT * FROM TRN_Params", DT_TRN_Params)
    End Sub

    Public Function DeliveryDate_get_Enum_from_Code(Code As String, Optional Enum_Code_If_Empty As ENUM_DELIVERYDATE_CODE = ENUM_DELIVERYDATE_CODE.NOT_DEFINED) As ENUM_DELIVERYDATE_CODE
        Dim OUT As ENUM_DELIVERYDATE_CODE_L = ENUM_DELIVERYDATE_CODE_L.NOT_DEFINED

        If Trim(Code) = "" Then
            If Enum_Code_If_Empty <> ENUM_DELIVERYDATE_CODE_L.NOT_DEFINED Then
                OUT = Enum_Code_If_Empty
            End If
        End If

        If OUT = ENUM_DELIVERYDATE_CODE_L.NOT_DEFINED Then
            Select Case Code
                Case "", " ", "0" : OUT = ENUM_DELIVERYDATE_CODE_L.EXPIMP
                Case "1" : OUT = ENUM_DELIVERYDATE_CODE_L.UNLOADING
                Case "2" : OUT = ENUM_DELIVERYDATE_CODE_L.LOADING
                Case "3" : OUT = ENUM_DELIVERYDATE_CODE_L.INV_DATE
                Case "4" : OUT = ENUM_DELIVERYDATE_CODE_L.CURRENT_DATE
                Case "5" : OUT = ENUM_DELIVERYDATE_CODE_L.EXPIMP
                Case "6" : OUT = ENUM_DELIVERYDATE_CODE_L.UNLOADING
                Case "7" : OUT = ENUM_DELIVERYDATE_CODE_L.LOADING
                Case "8" : OUT = ENUM_DELIVERYDATE_CODE_L.INV_DATE
                Case "A" : OUT = ENUM_DELIVERYDATE_CODE_L.EXPIMP
                Case "B" : OUT = ENUM_DELIVERYDATE_CODE_L.UNLOADING
                Case "C" : OUT = ENUM_DELIVERYDATE_CODE_L.LOADING
                Case "D" : OUT = ENUM_DELIVERYDATE_CODE_L.EXPIMP
                Case "E" : OUT = ENUM_DELIVERYDATE_CODE_L.UNLOADING
                Case "F" : OUT = ENUM_DELIVERYDATE_CODE_L.LOADING
                Case Else
                    DoErrorMsgBox("FP_App.DeliveryDate_get_Enum_From_Code", 0, String.Format("Unknown delivery date code: '{0}'", Code))
            End Select
        End If

        Return OUT
    End Function

    Public Function InitGlobals_ReadGeneralParams(Optional Allways As Boolean = False) As Boolean
        Static GeneralParams_Readed As Boolean = False

        If GeneralParams_Readed = False Or Allways Then
            GeneralParams_Readed = False

        End If

        Dim OUT As Boolean = True
        Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()
        Dim Result As Boolean = False
        Dim ErrMessage As String = ""

        With DC
            .Qdf_set_SP(sqlComm, "Params_Get_General_Params")
            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.VarChar, , 10, Terminal)
            .Qdf_AddParameter(sqlComm, "@Terminals_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@INV_Params", SqlDbType.VarChar, ParameterDirection.Output, 132)
            .Qdf_AddParameter(sqlComm, "@INV_Format", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@DailyFee_Rates_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@RouteStart_Rates_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@CashMove_Rates_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@Routes_Cards_Cash_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@DailyFee_Curr_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@Routes_Cost_Fuel_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ROUTES_DEFAULT_STATUS_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ReportsPath", SqlDbType.NVarChar, ParameterDirection.Output, 255)
            .Qdf_AddParameter(sqlComm, "@Curr_LC_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@Curr_LC_Code", SqlDbType.NVarChar, ParameterDirection.Output, 3)
            .Qdf_AddParameter(sqlComm, "@Curr_LC_Sign", SqlDbType.NVarChar, ParameterDirection.Output, 3)
            .Qdf_AddParameter(sqlComm, "@Curr_FC_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@Curr_FC_Code", SqlDbType.NVarChar, ParameterDirection.Output, 3)
            .Qdf_AddParameter(sqlComm, "@Curr_FC_Sign", SqlDbType.NVarChar, ParameterDirection.Output, 3)
            .Qdf_AddParameter(sqlComm, "@ACC_AC_DEB_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ACC_AC_VENDOR_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@CUST_OWNER_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@CUST_OWNER_Name1", SqlDbType.NVarChar, ParameterDirection.Output, 50)
            .Qdf_AddParameter(sqlComm, "@COUNTRY", SqlDbType.NVarChar, ParameterDirection.Output, 3)
            .Qdf_AddParameter(sqlComm, "@LANGUAGE", SqlDbType.NVarChar, ParameterDirection.Output, 3)
            .Qdf_AddParameter(sqlComm, "@JOBS_TIMERINTERVAL", SqlDbType.Int, ParameterDirection.Output)
            '.Qdf_AddParameter(sqlComm, "@ORD_DEFAULT_STATUS_ID", SqlDbType.Int, ParameterDirection.Output)
            '.Qdf_AddParameter(sqlComm, "@ORD_L_DEFAULT_STATUS_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@PROCESS_HANDLING_METHOD", SqlDbType.NVarChar, ParameterDirection.Output, 20)
            .Qdf_AddParameter(sqlComm, "@WEB_CURR_MANAGE", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@DOCMAN", SqlDbType.NVarChar, ParameterDirection.Output, 20)
            .Qdf_AddParameter(sqlComm, "@Cust_Base_Groups_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@CUST_TaxNum_Control_Method", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@PRODUCT_NAME", SqlDbType.NVarChar, ParameterDirection.Output, 128)
            .Qdf_AddParameter(sqlComm, "@CREDIT_LIMIT_PARAMS", SqlDbType.NVarChar, ParameterDirection.Output, 128)
            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.VarChar, ParameterDirection.Output, 255)
        End With

        Try
            Result = DC.Qdf_Execute("", sqlComm, , , "@ErrText")

        Catch ex As Exception
            DoErrorMsgBox("ReadGeneralParams", Err.Number, Err.Description)
        End Try

        If Not Result Then
            OUT = False
        Else
            Terminals_ID = sqlComm.Parameters("@Terminals_ID").Value

            Dim INV_PARAMS_STR As String = Mid(nz(sqlComm.Parameters("@INV_Params").Value, "") + Space(132), 1, 132)
            Dim INV_Params_Arr() As Char = INV_PARAMS_STR.ToCharArray

            If UBound(INV_Params_Arr) < 132 Then
                ReDim Preserve INV_Params_Arr(132)
            End If

            With P.INV_PARAMS
                .INV_Format_ID = sqlComm.Parameters("@INV_Format").Value

                'INV_PARAMS char(2): Copies -----------------------------------------------------------------------------------------------------------------------------------------------
                Select Case INV_Params_Arr(1)
                    Case "", " ", "0"
                        .Copies = 3

                    Case "1", "2", "3", "4", "5", "6", "7", "8", "9"
                        .Copies = Val(INV_Params_Arr(1))

                    Case Else
                        OUT = False
                        DoErrorMsgBox("FP_App.InitGlobals_ReadGeneralParams", 0, String.Format("Parameter 'ALT/SZAMLAZAS' position 2 (=Copies) has an unknown setting ('{0}')", INV_Params_Arr(1)))
                End Select

                'INV_PARAMS char(3): Leporello -----------------------------------------------------------------------------------------------------------------------------------------------
                Select Case INV_Params_Arr(2)
                    Case "", " ", "0"
                        .Leporello = False

                    Case "1"
                        .Leporello = True

                    Case Else
                        OUT = False
                        DoErrorMsgBox("FP_App.InitGlobals_ReadGeneralParams", 0, String.Format("Parameter 'ALT/SZAMLAZAS' position 3 (=Leporello) has an unknown setting ('{0}')", INV_Params_Arr(2)))
                End Select

                'INV_PARAMS char(4): Barmely ugyfelhez felveheto szla -------------------------------------------------------------------------------------------------------------------------
                Select Case INV_Params_Arr(3)
                    Case "", " ", "0"
                        .InvFor = ENUM_INV_PARAMS_INV_FOR.ALL

                    Case "1"
                        .InvFor = ENUM_INV_PARAMS_INV_FOR.HAVE_ACCT_Cust_ID

                    Case "2"
                        .InvFor = ENUM_INV_PARAMS_INV_FOR.HAVE_LINKCODE_1

                    Case "3"
                        .InvFor = ENUM_INV_PARAMS_INV_FOR.HAVE_LINKCODE_2

                    Case "4"
                        .InvFor = ENUM_INV_PARAMS_INV_FOR.HAVE_LINKCODE_1_FOR_OUT_AND_HAVE_INKCODE_2_FOR_IN

                    Case Else
                        OUT = False
                        DoErrorMsgBox("FP_App.InitGlobals_ReadGeneralParams", 0, String.Format("Parameter 'ALT/SZAMLAZAS' position 4 (=InvForClients) has an unknown setting ('{0}')", INV_Params_Arr(3)))
                End Select

                'INV_PARAMS char(5): Rounding -------------------------------------------------------------------------------------------------------------------------------------------------
                Select Case INV_Params_Arr(4)
                    Case "", " ", "0" : .Rounding = ENUM_INV_PARAMS_ROUNDING.PER_INV_LINE
                    Case "1" : .Rounding = ENUM_INV_PARAMS_ROUNDING.ONLY_AT_THE_END
                    Case "2" : .Rounding = ENUM_INV_PARAMS_ROUNDING.PER_TAXCODE

                    Case Else
                        OUT = False
                        DoErrorMsgBox("FP_App.InitGlobals_ReadGeneralParams", 0, String.Format("Parameter 'ALT/SZAMLAZAS' position 5 (=Rounding) has an unknown setting ('{0}')", INV_Params_Arr(4)))
                End Select

                'INV_PARAMS char(17): Kezi iktatas -------------------------------------------------------------------------------------------------------------------------------------------------
                Select Case INV_Params_Arr(16)
                    Case "", " ", "0" : .ManualFiling = False
                    Case "1" : .ManualFiling = True

                    Case Else
                        OUT = False
                        DoErrorMsgBox("FP_App.InitGlobals_ReadGeneralParams", 0, String.Format("Parameter 'ALT/SZAMLAZAS' position 17 (=Manual Filing) has an unknown setting ('{0}')", INV_Params_Arr(16)))
                End Select

                'INV_PARAMS char(23): Credit_Payment_Method ----------------------------------------------------------------------------------------------------------------------------------------------
                Select Case INV_Params_Arr(22)
                    Case "", " ", "0", "2" : .Credit_Payment_Method = ENUM_INV_PAYMENT_METHOD.BANK_TRANSFER
                    Case "1" : .Credit_Payment_Method = ENUM_INV_PAYMENT_METHOD.CASH
                    Case "3" : .Credit_Payment_Method = ENUM_INV_PAYMENT_METHOD.CHECK
                    Case Else : .Credit_Payment_Method = ENUM_INV_PAYMENT_METHOD.NOT_DEFINED
                End Select

                'INV_PARAMS char(24): Debit_Payment_Method ----------------------------------------------------------------------------------------------------------------------------------------------
                Select Case INV_Params_Arr(23)
                    Case "", " ", "0", "2" : .Debit_Payment_Method = ENUM_INV_PAYMENT_METHOD.BANK_TRANSFER
                    Case "1" : .Debit_Payment_Method = ENUM_INV_PAYMENT_METHOD.CASH
                    Case "3" : .Debit_Payment_Method = ENUM_INV_PAYMENT_METHOD.CHECK
                    Case Else : .Debit_Payment_Method = ENUM_INV_PAYMENT_METHOD.NOT_DEFINED
                End Select

                .Credit_DueDays = Val(Mid(INV_PARAMS_STR, 26, 3))
                .Debit_DueDays = Val(Mid(INV_PARAMS_STR, 29, 3))

                Select Case INV_Params_Arr(31)
                    Case "", " ", "0" : .Subcontracted_Services_Method = ENUM_Subcontracted_Services_Method.Allways_TRUE
                    Case "1" : .Subcontracted_Services_Method = ENUM_Subcontracted_Services_Method.Allways_FALSE
                    Case "2" : .Subcontracted_Services_Method = ENUM_Subcontracted_Services_Method.True_When_Subcontracter_Exists
                    Case Else : .Subcontracted_Services_Method = ENUM_Subcontracted_Services_Method.Allways_TRUE
                End Select

                Select Case INV_Params_Arr(32)
                    Case "", " ", "0" : .Debit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.NOT_DEFINED
                    Case "1" : .Debit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.UNLOADING
                    Case "2" : .Debit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.LOADING
                    Case "3" : .Debit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.INV_DATE
                    Case "4" : .Debit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.CURRENT_DATE
                    Case "5" : .Debit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.INV_DEADLINE
                    Case "6" : .Debit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.EXPIMP
                    Case "7" : .Debit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.INV_DELIVERYDATE
                    Case Else : .Debit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.NOT_DEFINED
                End Select

                Select Case INV_Params_Arr(33)
                    Case "", " ", "0" : .Debit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.NOT_DEFINED
                    Case "1" : .Debit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.UNLOADING
                    Case "2" : .Debit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.LOADING
                    Case "3" : .Debit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.INV_DATE
                    Case "4" : .Debit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.CURRENT_DATE
                    Case "5" : .Debit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.INV_DEADLINE
                    Case "6" : .Debit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.EXPIMP
                    Case "7" : .Debit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.INV_DELIVERYDATE
                    Case Else : .Debit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.NOT_DEFINED
                End Select

                Select Case INV_Params_Arr(34)
                    Case "", " ", "0" : .Credit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.NOT_DEFINED
                    Case "1" : .Credit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.UNLOADING
                    Case "2" : .Credit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.LOADING
                    Case "3" : .Credit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.INV_DATE
                    Case "4" : .Credit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.CURRENT_DATE
                    Case "5" : .Credit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.INV_DEADLINE
                    Case "6" : .Credit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.EXPIMP
                    Case "7" : .Credit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.INV_DELIVERYDATE
                    Case Else : .Credit_CurrDate_Code = ENUM_DELIVERYDATE_CODE_L.NOT_DEFINED
                End Select

                Select Case INV_Params_Arr(35)
                    Case "", " ", "0" : .Credit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.NOT_DEFINED
                    Case "1" : .Credit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.UNLOADING
                    Case "2" : .Credit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.LOADING
                    Case "3" : .Credit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.INV_DATE
                    Case "4" : .Credit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.CURRENT_DATE
                    Case "5" : .Credit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.INV_DEADLINE
                    Case "6" : .Credit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.EXPIMP
                    Case "7" : .Credit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.INV_DELIVERYDATE
                    Case Else : .Credit_DeliveryDate_Code_L = ENUM_DELIVERYDATE_CODE_L.NOT_DEFINED
                End Select

                Select Case INV_Params_Arr(36)
                    Case "", " ", "0" : .Periodic_DeliveryDate_Code = ENUM_PERIODIC_DELIVERYDATE.AS_BY_CASUAL_INV
                    Case "1" : .Periodic_DeliveryDate_Code = ENUM_PERIODIC_DELIVERYDATE.PERIOD_END
                    Case "2" : .Periodic_DeliveryDate_Code = ENUM_PERIODIC_DELIVERYDATE.PERIOD_BEGIN
                    Case "3" : .Periodic_DeliveryDate_Code = ENUM_PERIODIC_DELIVERYDATE.INV_DATE
                    Case "4" : .Periodic_DeliveryDate_Code = ENUM_PERIODIC_DELIVERYDATE.CURRENT_DATE
                    Case "5" : .Periodic_DeliveryDate_Code = ENUM_PERIODIC_DELIVERYDATE.INV_DEADLINE
                    Case Else : .Periodic_DeliveryDate_Code = ENUM_PERIODIC_DELIVERYDATE.AS_BY_CASUAL_INV
                End Select

                Select Case INV_Params_Arr(37)
                    Case "", " ", "0" : .Subcontracted_Services_ShowOnInvoice = ENUM_INV_SUBCONTRACTED_SERVICES_SHOW.In_HEAD
                    Case "1" : .Subcontracted_Services_ShowOnInvoice = ENUM_INV_SUBCONTRACTED_SERVICES_SHOW.In_LINE
                    Case Else : .Subcontracted_Services_ShowOnInvoice = ENUM_INV_SUBCONTRACTED_SERVICES_SHOW.In_HEAD
                End Select

                Select Case INV_Params_Arr(38)
                    Case "", " ", "0" : .DeliveryDate_Code_Head = ENUM_INV_DELIVERYDATE_HEAD_METHOD.NOT_DEFINED
                    Case "1" : .DeliveryDate_Code_Head = ENUM_INV_DELIVERYDATE_HEAD_METHOD.LineDelivery_Max
                    Case "2" : .DeliveryDate_Code_Head = ENUM_INV_DELIVERYDATE_HEAD_METHOD.LineDelivery_Min
                    Case "3" : .DeliveryDate_Code_Head = ENUM_INV_DELIVERYDATE_HEAD_METHOD.Inv_Date
                    Case "5" : .DeliveryDate_Code_Head = ENUM_INV_DELIVERYDATE_HEAD_METHOD.Inv_DueDate
                    Case Else : .DeliveryDate_Code_Head = ENUM_INV_DELIVERYDATE_HEAD_METHOD.NOT_DEFINED
                End Select

                Select Case INV_Params_Arr(39)
                    Case "", " ", "0" : .INV_AccDate_From = ENUM_INV_PLANED_PAYMENT_METHOD.FROM_DUEDATE
                    Case "1" : .INV_AccDate_From = ENUM_INV_PLANED_PAYMENT_METHOD.FROM_POSTINDATE
                    Case "2" : .INV_AccDate_From = ENUM_INV_PLANED_PAYMENT_METHOD.FROM_CUST_CREDIT_DUEDAYS
                    Case Else
                        MsgBox("+++ '" + nz(INV_Params_Arr(39), "NULL") + "'")
                        .INV_AccDate_From = ENUM_INV_PLANED_PAYMENT_METHOD.FROM_DUEDATE
                End Select

                Select Case INV_Params_Arr(40)
                    Case "", " ", "0" : .INV_And_Calc_Manage = ENUM_INV_Inv_And_Calc_Manage_Type.NORMAL
                    Case "1" : .INV_And_Calc_Manage = ENUM_INV_Inv_And_Calc_Manage_Type.INV_AND_CALC_ARE_SAME
                    Case Else
                        MsgBox("+++ '" + nz(INV_Params_Arr(40), "NULL") + "'")
                        .INV_And_Calc_Manage = ENUM_INV_Inv_And_Calc_Manage_Type.NORMAL
                End Select

                Select Case INV_Params_Arr(41)
                    Case "", " ", "0" : .INV_Line_Handling = ENUM_INV_Line_Handling_Type.NORMAL
                    Case "1" : .INV_Line_Handling = ENUM_INV_Line_Handling_Type.INV_LINE_ONLY_FROM_CALC
                    Case Else
                        MsgBox("+++ '" + nz(INV_Params_Arr(41), "NULL") + "'")
                        .INV_And_Calc_Manage = ENUM_INV_Line_Handling_Type.NORMAL
                End Select

                Select Case INV_Params_Arr(42)
                    Case "", " ", "0" : .CreditNote_Sign_Manage = ENUM_INV_CreditNote_Sign_Manage.PCS_is_Negative
                    Case "1" : .CreditNote_Sign_Manage = ENUM_INV_CreditNote_Sign_Manage.UNITPRICE_is_Negative
                    Case Else
                        MsgBox("+++ '" + nz(INV_Params_Arr(42), "NULL") + "'")
                        .INV_And_Calc_Manage = ENUM_INV_CreditNote_Sign_Manage.PCS_is_Negative
                End Select

                Select Case INV_Params_Arr(43)
                    Case "", " ", "0" : .Calc_Sign_Manage = ENUM_INV_Calc_Sign_Manage.NORMAL
                    Case "1" : .Calc_Sign_Manage = ENUM_INV_Calc_Sign_Manage.ONLY_WITH_SAME_SIGN
                    Case Else
                        MsgBox("+++ '" + nz(INV_Params_Arr(43), "NULL") + "'")
                        .INV_And_Calc_Manage = ENUM_INV_Calc_Sign_Manage.NORMAL
                End Select

                Select Case INV_Params_Arr(46)
                    Case "", " ", "0" : .INV_Save_Origin_To_PDF = ENUM_INV_Save_Origin_To_PDF.Default_Save_With_Question
                    Case "1" : .INV_Save_Origin_To_PDF = ENUM_INV_Save_Origin_To_PDF.No_Save
                    Case "2" : .INV_Save_Origin_To_PDF = ENUM_INV_Save_Origin_To_PDF.Save_With_Question
                    Case "3" : .INV_Save_Origin_To_PDF = ENUM_INV_Save_Origin_To_PDF.Save_Auto_To_Map_Monthly
                    Case "4" : .INV_Save_Origin_To_PDF = ENUM_INV_Save_Origin_To_PDF.Save_Auto_To_Map_Yearly
                    Case Else
                        MsgBox("+++ '" + nz(INV_Params_Arr(47), "NULL") + "'")
                        .INV_Save_Origin_To_PDF = ENUM_INV_Save_Origin_To_PDF.Default_Save_With_Question
                End Select

                ParmLesen("INV", "SAVE_PDF_LOCATION", 0, .INV_Save_Origin_To_PDF_Location)

                '-----------------------------------------------------------------------------
                ' INV/SAVE_PDF_FILENAME parameter
                '-----------------------------------------------------------------------------
                Dim INV_PDF_FielName_Nr As Integer = 0
                Dim INV_PDF_Filename_Str As String = ""

                .INV_Save_Origin_To_PDF_FileName = "<CUST>_<NR>_Invoice.pdf"

                ParmLesen("INV", "SAVE_PDF_FILENAME", INV_PDF_FielName_Nr, INV_PDF_Filename_Str)
                If INV_PDF_Filename_Str = "" Then
                    INV_PDF_Filename_Str = "<CUST>_<NR>_Invoice.pdf"
                    INV_PDF_FielName_Nr = 0
                    gl_FPApp.ParmInsertOrUpdate("INV", "SAVE_PDF_FILENAME", INV_PDF_FielName_Nr, INV_PDF_Filename_Str)
                End If

                .INV_Save_Origin_To_PDF_FileName = INV_PDF_Filename_Str

                '-----------------------------------------------------------------------------
                ' INV/DEPOS_PRINT_MODE parameter
                '-----------------------------------------------------------------------------
                Dim Depos_Print_Mode As Integer
                ParmLesen("INV", "DEPOS_PRINT_MODE", Depos_Print_Mode)
                If Depos_Print_Mode = 0 Then Depos_Print_Mode = 1
                .Deposit_Invoice_Print_Mode = Depos_Print_Mode
            End With

            With P.INV_DIRECT_PARAMS
                Select Case INV_Params_Arr(44)
                    Case "", " ", "0" : .METHOD = ENUM_INV_DIRECT_METHOD.INV
                    Case "1" : .METHOD = ENUM_INV_DIRECT_METHOD.INV_AND_CREDITNOTE
                    Case Else
                        .METHOD = ENUM_INV_DIRECT_METHOD.INV
                        MsgBox("+++ '" + nz(INV_Params_Arr(44), "NULL") + "'")
                End Select

                Select Case INV_Params_Arr(45)
                    Case "", " ", "0" : .APPEND_TO_EXISTING = False
                    Case "1" : .APPEND_TO_EXISTING = True
                    Case Else
                        .APPEND_TO_EXISTING = False
                        MsgBox("+++ '" + nz(INV_Params_Arr(45), "NULL") + "'")
                End Select

            End With
            With P.ROUTES_PARAMS
                .DailyFee_Rates_ID = sqlComm.Parameters("@DailyFee_Rates_ID").Value
                .RouteStart_Rates_ID = sqlComm.Parameters("@RouteStart_Rates_ID").Value
                .CashMove_Rates_ID = sqlComm.Parameters("@CashMove_Rates_ID").Value
                .Routes_Cards_Cash_ID = sqlComm.Parameters("@Routes_Cards_Cash_ID").Value
                .DailyFee_Curr_ID = sqlComm.Parameters("@DailyFee_Curr_ID").Value
                .Routes_Cost_Fuel_ID = sqlComm.Parameters("@Routes_Cost_Fuel_ID").Value
                .ROUTE_STATUS_ID_BASE = sqlComm.Parameters("@ROUTES_DEFAULT_STATUS_ID").Value
            End With

            With P.Report_Params
                .ReportPath = FolderName_With_LogicalNames(sqlComm.Parameters("@ReportsPath").Value)
            End With
            With P.Accounting_Params
                .ACC_AC_DEB_ID = sqlComm.Parameters("@ACC_AC_DEB_ID").Value
                .ACC_AC_VENDOR_ID = sqlComm.Parameters("@ACC_AC_VENDOR_ID").Value
            End With

            With P.Currencies_Params
                .Curr_FC_ID = sqlComm.Parameters("@Curr_FC_ID").Value
                .Curr_FC_Code = sqlComm.Parameters("@Curr_FC_Code").Value
                .Curr_FC_Sign = sqlComm.Parameters("@Curr_FC_Sign").Value
                .Curr_LC_ID = sqlComm.Parameters("@Curr_LC_ID").Value
                .Curr_LC_Code = sqlComm.Parameters("@Curr_LC_Code").Value
                .Curr_LC_Sign = sqlComm.Parameters("@Curr_LC_Sign").Value
            End With

            With P.Owner_Params
                .COUNTRY = sqlComm.Parameters("@COUNTRY").Value
                .CUST_OWNER_ID = sqlComm.Parameters("@CUST_OWNER_ID").Value
                .CUST_OWNER_Name1 = sqlComm.Parameters("@CUST_OWNER_Name1").Value
                .LANGUAGE = sqlComm.Parameters("@LANGUAGE").Value
            End With

            With P.ORD_PARAMS
                '.ORD_STATUS_ID_BASE = sqlComm.Parameters("@ORD_DEFAULT_STATUS_ID").Value
                '.ORD_L_STATUS_ID_BASE = sqlComm.Parameters("@ORD_L_DEFAULT_STATUS_ID").Value
                .PROCESS_HANDLING = sqlComm.Parameters("@PROCESS_HANDLING_METHOD").Value
            End With

            With P.System_Params
                .JOBS_TIMERINTERVAL = sqlComm.Parameters("@JOBS_TIMERINTERVAL").Value
            End With

            With P.WEB
                .WEB_Curr_Manage = sqlComm.Parameters("@WEB_CURR_MANAGE").Value
            End With

            With P.DOCMAN
                Select Case sqlComm.Parameters("@DOCMAN").Value
                    Case "", " ", "0" : .DeleteAfterSave = ENUM_DOCMAN.DELETE_WITH_CONFIRM
                    Case "1" : .DeleteAfterSave = ENUM_DOCMAN.DELETE_WITHOUT_CONFIRM
                    Case "2" : .DeleteAfterSave = ENUM_DOCMAN.NO_DELETE
                    Case Else
                        DoErrorMsgBox("FP_App.InitGlobals_ReadGeneralParams", 0, String.Format("Value of Parameter '@DOCMAN' is invalid. '{0}'", sqlComm.Parameters("@DOCMAN").Value))
                End Select
            End With
        End If

        'Cust_Base_Groups_ID ----------------------------------------------------------------------------------------------------------------------------------------------
        P.CUST.Base_Groups_ID = sqlComm.Parameters("@Cust_Base_Groups_ID").Value

        'CUST_TaxNum_Control_Method ---------------------------------------------------------------------------------------------------------------------------------------
        With P.CUST
            Select Case nz(sqlComm.Parameters("@CUST_TaxNum_Control_Method").Value, ENUM_Cust_TaxNum_Control_Method.NORMAL)
                Case ENUM_Cust_TaxNum_Control_Method.NORMAL, ENUM_Cust_TaxNum_Control_Method.NO_CHECK
                    .TaxNum_Control_Method = sqlComm.Parameters("@CUST_TaxNum_Control_Method").Value

                Case Else
                    DoErrorMsgBox("FP_App.InitGlobals_ReadGeneralParams", 0, String.Format("Value of Parameter '@CUST_TaxNum_Control_Method' is invalid. (Value: {0})", nz(sqlComm.Parameters("@CUST_TaxNum_Control_Method").Value, 0)))
            End Select
        End With

        'PRODUCT_NAME -----------------------------------------------------------------------------------------------------------------------------------------------------
        P.PRODUCT_NAME = nz(sqlComm.Parameters("@PRODUCT_NAME").Value, "")

        '@CREDIT_LIMIT_PARAMS --------------------------------------------------------------------------------------------------------------------------------------------------------
        Dim CREDIT_LIMIT_PARAMS As String = nz(sqlComm.Parameters("@CREDIT_LIMIT_PARAMS").Value, "")
        Dim CREDIT_LIMIT_CHECK_METHOD_STR As String = Mid(CREDIT_LIMIT_PARAMS, 1, 1)

        Select Case CREDIT_LIMIT_CHECK_METHOD_STR
            Case "", "0" : P.CREDIT_LIMIT_Params.CHK_MODE = ENUM_CREDIT_LIMIT_CHK_MODE.NO_CHECK
            Case "1" : P.CREDIT_LIMIT_Params.CHK_MODE = ENUM_CREDIT_LIMIT_CHK_MODE.INCOME_ONLY
            Case "2" : P.CREDIT_LIMIT_Params.CHK_MODE = ENUM_CREDIT_LIMIT_CHK_MODE.CHECK_ALL
            Case Else
                DoErrorMsgBox("FP_App.InitGlobals_ReadGeneralParams", 0, String.Format("Value of Parameter '@CREDIT_LIMIT_PARAMS' is invalid. (Value: {0})", CREDIT_LIMIT_PARAMS))
        End Select

        'NEW_DEVELOPMENT_PARAMS_JSON
        'Ez a parameter azt az ellentmondast szeretne feloldani, hogy egy konkret uj fejlesztest egy ceg adatbazisan fejlesztunk ki, backend es modositasokkal
        'es ettol orokke elmaszik egy mar kiadott verzio kompatibilitasa.
        'Megoldas menete:
        '
        '1. A FixText-be vegyed fel a 'NEW_DEVELOPMENT_PARAMS_JSON' parametert es tedd bele az uj funkcio megletere vonatkozo parametereket. (JSON formatumban!!!!!!)
        '   Pl.: 
        '        {
        '          "TOMTOM_DEVELOPMENT": {
        '                                  "INSTALLED": true,
        '                                  "Param_1"  : "valami"
        '                                }
        '        }
        '
        '2. Ahol a frontend-ben valtoztatni kell pl. egy eljaras hivast, oda ird be:
        '   Dim serializer As New JavaScriptSerializer()
        '   Dim serializer_Values As Response = serializer.Deserialize(Of Response)(jsonResponse)
        '   If nz(serialiser.TOMTOM_DEVELOPMENT.INSTALLED, false) = True Then
        '      (...)

        P.NEW_DEVELOPMENT_PARAMS_JSON = getFixText("NEW_DEVELOPMENT_PARAMS_JSON")

        InitGlobals_ReadGeneralParams = OUT
    End Function

    Function Windows_FolderOpenDialogBox(Optional ByVal MyDialogTitle As String = "SEQ##SEQ_FOLDER_OPEN,1##END|", Optional ByVal SelectedPath As String = "", Optional ByVal INI_Key_for_SelectedPath As String = "") As String
        Dim OUT As String = ""

        Dim FODialog As New FolderBrowserDialog
        With FODialog
            If SelectedPath = "" Then
                If INI_Key_for_SelectedPath > "" Then
                    PFDlesen(INI_Key_for_SelectedPath, SelectedPath)
                End If
            End If

            If SelectedPath > "" Then
                .SelectedPath = SelectedPath
            End If

            .ShowNewFolderButton = True
            .Description = Text_getParamFromLine(Text_ParametersErsetzen(MyDialogTitle, "", "|"), 1, , , "|")

            Dim ActiveFPf As FP_Form = FORMS_GET_ACTIVE_FPf()
            Dim F As Form = Nothing

            If Not (ActiveFPf Is Nothing) Then
                F = ShowDialogForm_getOpacityForm(ActiveFPf.Frm)
            End If

            If FODialog.ShowDialog = DialogResult.OK Then
                OUT = .SelectedPath
                If INI_Key_for_SelectedPath > "" Then
                    PFDinsertOrUpdate(INI_Key_for_SelectedPath, OUT)
                End If
            End If
            If Not (F Is Nothing) Then
                F.Close()
            End If
        End With

        Windows_FolderOpenDialogBox = OUT

    End Function

    Function Windows_FileSaveDialogBox(Extension_Filter As String, ByRef OUT_FullName As String) As Boolean
        Dim OUT As Boolean = False
        Dim SaveDialog As New SaveFileDialog
        Dim DirName As String = getDirectoryFromFullName(OUT_FullName)
        Dim FileName As String = getFileNameFromFullName(OUT_FullName)

        OUT_FullName = ""

        With SaveDialog
            .Filter = Extension_Filter
            .FileName = FileName
            .InitialDirectory = DirName
            .ShowDialog()

            If .FileName > "" Then
                OUT_FullName = .FileName
                OUT = True
            End If
        End With

        Return OUT
    End Function

    Function Windows_FileOpenDialogBox(Optional ByVal MyFilterString As String = "*.*", Optional ByVal MyDir As String = "", Optional ByVal DialogTitle As String = "", Optional ByVal MyFileName As String = "") As String
        Dim OUT As String = ""
        Dim FileTitle As String = ""
        Dim DialogTitle_Text As String = ""

        MyFilterString = Text_ParametersErsetzen(MyFilterString, "", "|")
        If DialogTitle > "" Then
            Dim MySEQ As New FP_SEQ(Me, DialogTitle)

            DialogTitle_Text = DialogTitle
        End If

        Dim OFDialog As New OpenFileDialog

        With OFDialog
            .Filter = MyFilterString
            .InitialDirectory = MyDir
            .FileName = MyFileName

            Dim FPf_ActiveForm As FP_Form = FORMS_GET_ACTIVE_FPf()
            If Not (FPf_ActiveForm Is Nothing) Then
                FPf_ActiveForm.P_ENABLED = False
            End If

            If .ShowDialog = DialogResult.OK Then
                OUT = .FileName
            End If

            If Not (FPf_ActiveForm Is Nothing) Then
                FPf_ActiveForm.P_ENABLED = True
            End If
        End With

        Windows_FileOpenDialogBox = OUT
    End Function

    Function ACCT_Periods_Get_From_Year_And_Month(Year_STR As String, Month_STR As String) As Long
        Dim OUT As Long
        Dim MySQL As String = String.Format("SELECT dbo.ACCT_Periods_GET_FROM_YEAR_AND_MONTH('{0}', '{1}') ACCT_Periods_ID", Year_STR, Month_STR)

        OUT = DC.Qdf_get_DataRow(MySQL)!ACCT_Periods_ID

        Return OUT
    End Function

    Function ACCT_Periods_Get_From_Year_And_WeekNum(Year_STR As String, WeekNum_STR As String) As Long
        Dim OUT As Long
        Dim MySQL As String = String.Format("SELECT dbo.ACCT_Periods_GET_FROM_YEAR_AND_WEEKNUM('{0}', '{1}') ACCT_Periods_ID", Year_STR, WeekNum_STR)

        OUT = DC.Qdf_get_DataRow(MySQL)!ACCT_Periods_ID

        Return OUT
    End Function

    Function ACCT_Periods_Get_Info(ACCT_Periods_ID As Long, ByRef OUT_P As Struct_ACCT_Periods_Info) As Boolean
        Dim OUT As Boolean = True

        OUT_P = New Struct_ACCT_Periods_Info

        Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

        With DC
            .Qdf_set_SP(sqlComm, "ACCT_Periods_GET_PERIOD_INFO")
            .Qdf_AddParameter(sqlComm, "@ACCT_Periods_ID", SqlDbType.Int, , , , , ACCT_Periods_ID)
            .Qdf_AddParameter(sqlComm, "@Lang", SqlDbType.NVarChar, , 3, LandDialog)

            .Qdf_AddParameter(sqlComm, "@OUT_ACCT_Periods_Name", SqlDbType.NVarChar, ParameterDirection.Output, 20)
            .Qdf_AddParameter(sqlComm, "@OUT_Parent_ACCT_Periods_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_Parent_ACCT_Periods_Name", SqlDbType.NVarChar, ParameterDirection.Output, 20)
            .Qdf_AddParameter(sqlComm, "@OUT_Date_FROM", SqlDbType.DateTime, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_Date_TO", SqlDbType.DateTime, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_No_New_Order", SqlDbType.Bit, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_CSHBK_Closed", SqlDbType.Bit, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_INV_Outgoing_Closed", SqlDbType.Bit, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_INV_Incoming_Closed", SqlDbType.Bit, ParameterDirection.Output)

            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
        End With

        CURSOR_SHOW_WAIT()
        Try
            OUT = DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
        Catch ex As Exception
            OUT = False
            DoErrorMsgBox("FP_App.Perioden_Get_From_Date", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        If OUT Then
            With OUT_P
                .ACCT_Periods_Name = nz(sqlComm.Parameters("@OUT_ACCT_Periods_Name").Value, "")
                .Parent_ACCT_Periods_ID = nz(sqlComm.Parameters("@OUT_Parent_ACCT_Periods_ID").Value, 0)
                .Parent_ACCT_Periods_Name = nz(sqlComm.Parameters("@OUT_Parent_ACCT_Periods_Name").Value, "")
                .Date_FROM = nz(sqlComm.Parameters("@OUT_Date_FROM").Value, NULLDATE)
                .Date_TO = nz(sqlComm.Parameters("@OUT_Date_TO").Value, NULLDATE)
                .No_New_Order = (nz(sqlComm.Parameters("@OUT_No_New_Order").Value, False))
                .CSHBK_Closed = (nz(sqlComm.Parameters("@OUT_CSHBK_Closed").Value, False))
                .INV_Outgoing_Closed = (nz(sqlComm.Parameters("@OUT_INV_Outgoing_Closed").Value, 0) = 1)
                .INV_Incoming_Closed = (nz(sqlComm.Parameters("@OUT_INV_Incoming_Closed").Value, 0) = 1)
            End With
        End If

        Return OUT
    End Function

    Function ACCT_Periods_Get_From_Date(MyDate As DateTime, ByRef OUT_P As Struct_ACCT_Periods_Info) As Boolean
        Dim OUT As Boolean = True

        OUT_P = New Struct_ACCT_Periods_Info

        Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

        With DC
            .Qdf_set_SP(sqlComm, "ACCT_Periods_GET_FROM_DATE")
            .Qdf_AddParameter(sqlComm, "@Date", SqlDbType.DateTime, , , , MyDate)
            .Qdf_AddParameter(sqlComm, "@Lang", SqlDbType.NVarChar, , 3, LandDialog)

            .Qdf_AddParameter(sqlComm, "@OUT_ACCT_Periods_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_ACCT_Periods_Name", SqlDbType.NVarChar, ParameterDirection.Output, 20)
            .Qdf_AddParameter(sqlComm, "@OUT_Parent_ACCT_Periods_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_Parent_ACCT_Periods_Name", SqlDbType.NVarChar, ParameterDirection.Output, 20)
            .Qdf_AddParameter(sqlComm, "@OUT_Date_FROM", SqlDbType.DateTime, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_Date_TO", SqlDbType.DateTime, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_No_New_Order", SqlDbType.Bit, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_CSHBK_Closed", SqlDbType.Bit, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_INV_Outgoing_Closed", SqlDbType.Bit, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUT_INV_Incoming_Closed", SqlDbType.Bit, ParameterDirection.Output)

            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
        End With

        CURSOR_SHOW_WAIT()
        Try
            OUT = DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
        Catch ex As Exception
            OUT = False
            DoErrorMsgBox("FP_App.Perioden_Get_From_Date", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        If OUT Then
            With OUT_P
                .ACCT_Periods_ID = nz(sqlComm.Parameters("@OUT_ACCT_Periods_ID").Value, 0)
                .ACCT_Periods_Name = nz(sqlComm.Parameters("@OUT_ACCT_Periods_Name").Value, "")
                .Parent_ACCT_Periods_ID = nz(sqlComm.Parameters("@OUT_Parent_ACCT_Periods_ID").Value, 0)
                .Parent_ACCT_Periods_Name = nz(sqlComm.Parameters("@OUT_Parent_ACCT_Periods_Name").Value, "")
                .Date_FROM = nz(sqlComm.Parameters("@OUT_Date_FROM").Value, NULLDATE)
                .Date_TO = nz(sqlComm.Parameters("@OUT_Date_TO").Value, NULLDATE)
                .No_New_Order = nz(sqlComm.Parameters("@OUT_No_New_Order").Value, False)
                .CSHBK_Closed = nz(sqlComm.Parameters("@OUT_CSHBK_Closed").Value, False)
                .INV_Outgoing_Closed = (nz(sqlComm.Parameters("@OUT_INV_Outgoing_Closed").Value, 0) = 1)
                .INV_Incoming_Closed = (nz(sqlComm.Parameters("@OUT_INV_Incoming_Closed").Value, 0) = 1)
            End With
        End If

        Return OUT
    End Function

    Function ByteArray_getFile(ByVal FileName As String, ByRef MyByteArray As Byte()) As Boolean
        Dim fInfo As FileInfo = New FileInfo(FileName)
        Dim numBytes As Long
        Dim fStream As FileStream = Nothing
        Dim fStream_Opened As Boolean = False
        Dim br As BinaryReader

        Try
            If Not fInfo.Exists Then
                ReDim MyByteArray(0)
                ByteArray_getFile = False
                Exit Function
            End If
            numBytes = fInfo.Length
            fStream = New FileStream(FileName, FileMode.Open, FileAccess.Read)
            fStream_Opened = True

            br = New BinaryReader(fStream)
            ReDim MyByteArray(numBytes)
            MyByteArray = br.ReadBytes(CInt(numBytes))

            fStream.Close()
            br.Close()

            ByteArray_getFile = True
            Exit Function

        Catch ex As Exception
            ByteArray_getFile = False
            If fStream_Opened Then
                fStream.Close()
            End If
            DoErrorMsgBox("FP_App.ByteArray_getFile", Err.Number, Err.Description)
            Exit Function
        End Try
    End Function

    Function ByteArray_SaveFile(ByVal FullFileName As String, ByVal MyByteArray As Byte()) As Boolean
        Dim OUT As Boolean = False

        If MyByteArray IsNot Nothing Then
            Try
                Dim numBytes As Long
                Dim fStream As FileStream

                numBytes = UBound(MyByteArray)
                If numBytes > 0 Then

                    fStream = New FileStream(FullFileName, FileMode.Create, FileAccess.Write)
                    fStream.Write(MyByteArray, 0, numBytes + 1)
                    fStream.Close()

                    OUT = True
                End If

            Catch ex As Exception
                DoErrorMsgBox("FP_App.ByteArray_SaveFile", Err.Number, Err.Description)
            End Try
        End If

        Return OUT
    End Function

    Private Sub Create_CL_Chk_DataTable()
        CL_Chk_DataTable = New DataTable("CL_DT")
        Dim FieldDef As String = "HWND.INT|CUST_ID.INT|DT.DATE"
        Dim DFields() As String = FieldDef.Split("|")
        Dim DField() As String
        Dim F As String
        Dim DC As DataColumn
        For Each F In DFields
            DField = F.Split(".")
            DC = New DataColumn With {
                .ColumnName = DField(0)
            }
            Select Case DField(1).ToUpper
                Case "INT"
                    DC.DataType = System.Type.GetType("System.Int32")
                Case "DATE"
                    DC.DataType = System.Type.GetType("System.DateTime")
                Case Else
                    DC.DataType = System.Type.GetType("System.String")
            End Select
            CL_Chk_DataTable.Columns.Add(DC)
        Next
    End Sub

    Private Function CL_Required_For_This_Cust(Cust_ID As Long, Hwnd As Long, Cust_Groups_ID As Integer) As Boolean
        Dim OUT As Boolean = True
        If Cust_Groups_ID = -1001 Then  'Belso osztaly eseten nem kell hitelkeret vizsgalat
            Return False
        End If

        If CL_Chk_DataTable Is Nothing Then
            Create_CL_Chk_DataTable()
        End If

        'Delete old records (older than 10 minutes)
        Dim I As Integer = 0
        Do While I <= CL_Chk_DataTable.Rows.Count - 1
            Dim TimeDiff As Integer = DateDiff(DateInterval.Minute, CL_Chk_DataTable.Rows(I).Item("DT"), Now)
            If TimeDiff >= 10 Then
                CL_Chk_DataTable.Rows.Remove(CL_Chk_DataTable.Rows(I))
            Else
                I += 1
            End If
        Loop

        'Check for existing record
        Dim MySelect As String = String.Format("HWND = {0} AND Cust_ID = {1}", Hwnd, Cust_ID)
        Dim dRows() As DataRow
        dRows = CL_Chk_DataTable.Select(MySelect)

        If dRows.Length > 0 Then
            OUT = False
        Else
            Dim NewDRow As DataRow = CL_Chk_DataTable.NewRow
            NewDRow.Item("HWND") = Hwnd
            NewDRow.Item("CUST_ID") = Cust_ID
            NewDRow.Item("DT") = Now
            CL_Chk_DataTable.Rows.Add(NewDRow)
        End If

        Return OUT
    End Function

    Private Sub CL_DataTable_Del_Cust_Rows(Cust_ID As Long)
        Dim I As Integer = 0
        Do While I <= CL_Chk_DataTable.Rows.Count - 1
            Dim C_ID As Integer = CL_Chk_DataTable.Rows(I).Item("CUST_ID")
            If C_ID = Cust_ID Then
                CL_Chk_DataTable.Rows.Remove(CL_Chk_DataTable.Rows(I))
            Else
                I += 1
            End If
        Loop
    End Sub

    Public Function CL_CHECK(Cust_ID As Long _
                               , With_Dialog As Boolean _
                               , Hwnd As Long _
                               , Optional Current_Chrg_ID As Long = 0 _
                               , Optional Current_Netto As Double = 0 _
                               , Optional Current_Curr_ID As Long = 0 _
                               , Optional Current_TAX_ID As Long = 0 _
                               , Optional Current_CRDR As Long = 0 _
                               , Optional Current_Date As DateTime = Nothing _
                               , Optional Cust_Groups_ID As Long = 0
                               ) As Boolean

        Dim OUT As Boolean
        Dim DT As String = SQLDate(Current_Date)

        If CL_Required_For_This_Cust(Cust_ID, Hwnd, Cust_Groups_ID) Then
            Dim Current_Netto_STR As String = DBFORMAT_from_OBJECT(Current_Netto, "", "FLOAT")
            Dim SQL As String = String.Format("SELECT dbo.CL_CHECK({0}, {1}, {2}, {3}, {4}, {5}, {6}) CHOK FROM Cegek WHERE ID = {0}", Cust_ID, Current_Chrg_ID, Current_Netto_STR, Current_Curr_ID, Current_TAX_ID, Current_CRDR, DT)
            Dim DR As DataRow
            Dim UserRight_OK As Integer = UserRights.CUS_ACC_Handling

            DR = DC.Qdf_get_DataRow(SQL)
            If DR IsNot Nothing Then
                OUT = DR.Item("CHOK")
            Else
                OUT = False
            End If

            If With_Dialog Then
                If Not OUT Then
                    If UserRight_OK Then
                        If DoMyMsgBox(22041, "", "SEQ,NO", "SEQ,YES") = 2 Then
                            OUT = True
                        End If
                    Else
                        DoMyMsgBox(22040) 'Az ügyfél tartozása meghaladta a megengedett hitelkeretet!
                    End If
                Else
                    CL_DataTable_Del_Cust_Rows(Cust_ID)
                End If
            End If
        Else
            OUT = True
        End If
        Return OUT
    End Function

    Public Sub TEMP_Folders_EMPTY()
        Try
            System.IO.Directory.Delete(SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP), True)

        Catch ex As Exception
            'Nothing to do
        End Try

        Try
            System.IO.Directory.Delete(SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.REPORT_TEMP), True)

        Catch ex As Exception
            'Nothing to do
        End Try

        SELEXPED_Folders_CREATE()
    End Sub

    Public Function FolderName_With_LogicalNames(FolderName) As String
        Dim OUT As String = FolderName

        OUT = Replace(OUT, "<STARTUPPATH>", Application.StartupPath)
        OUT = Replace(OUT, "<SELEXPED_TEMP>", SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP))
        OUT = Replace(OUT, "<SELEXPED_REPORTTEMP>", SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.REPORT_TEMP))
        OUT = Replace(OUT, "<SELEXPED_PREPARED_TEMPLATES>", SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.PREPARED_TEMPLATES))

        Return OUT
    End Function

    Public ReadOnly Property SELEXPED_FolderName_GET(FolderType As ENUM_SELEXPED_Folder_Types) As String
        Get
            Select Case FolderType
                Case ENUM_SELEXPED_Folder_Types.REPORT_TEMP
                    Return Application.StartupPath + "\SEL_REPORTS_TEMP\"

                Case ENUM_SELEXPED_Folder_Types.PREPARED_TEMPLATES
                    Return Application.StartupPath + "\SEL_WORD_PREPARED_TEMPLATES\"

                Case ENUM_SELEXPED_Folder_Types.TEMP
                    Return Application.StartupPath + "\SEL_TEMP\"

                Case Else
                    DoErrorMsgBox("FP_App.SELEXPED_FolderName_GET", 0, String.Format("Unknown foldertype ({0})", FolderType))
                    Return ""
            End Select
        End Get
    End Property

#Region "PROTECTED"
    Public Function APPLICATION_PREPARE_QUIT() As Boolean
        Dim OUT As Boolean

        FP_APP_RUNNING_STATE = ENUM_FP_APP_RUNNING_STATE.ON_CLOSING

        If Not FORMS_CLOSE_ALL_FORM() Then
            OUT = False
            FP_APP_RUNNING_STATE = ENUM_FP_APP_RUNNING_STATE.RUNNING
        Else
            FP_APP_RUNNING_STATE = ENUM_FP_APP_RUNNING_STATE.CLOSED
            OUT = True
            PFD_SAVE()
            RAISEEVENT_APPLICATION_CLOSED()
        End If

        APPLICATION_PREPARE_QUIT = OUT
    End Function
    Protected Friend Function DEBUG_MODE_SAVE() As Boolean
        Dim OUT As Boolean = False

        If MsgBox("Save changes?" + vbCrLf +
                  "After saving, all the user will see your changes on the mask. Continue?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()
            Dim Result As Boolean

            DC.Qdf_set_SP(sqlComm, "DEBUG_MODE_SAVE_CHANGES")
            Try
                Result = DC.Qdf_Execute("", sqlComm)
                MsgBox("Changes SUCCESSFULLY saved.")
            Catch ex As Exception
                DoErrorMsgBox("FP_App.DEBUG_MODE_SAVE", Err.Number, Err.Description)
            End Try
            sqlComm.Dispose()
        End If

        DEBUG_MODE_SAVE = OUT
    End Function
    Public Function Is_DEBUG_MODE() As Boolean
        Dim OUT As Boolean = False
        Dim wchar As String = ""
        Static DebugMode_Checked As Boolean = False

        If DebugMode_Checked Then
            OUT = DebugMode
        Else
            If Not (Parent Is Nothing) Then
                DebugMode = Parent.P_DebugMode
                OUT = DebugMode
            Else
                If DC.P_DBVersion_Is_OK = False Then
                    DebugMode = True
                    OUT = DebugMode
                Else
                    DebugMode_Checked = True

                    PFDlesen("DEBUG", wchar)

                    If Trim(wchar) = "1" Then
                        If MsgBox("DEBUG MODE is ACTIVE!!!" + vbCrLf + "Do you want to turn it off?", MsgBoxStyle.Critical + MsgBoxStyle.YesNo, "DEBUG MODE ACTIVE") = MsgBoxResult.Yes Then
                            PFDinsertOrUpdate("DEBUG", "0")
                            PFDinsertOrUpdate("DEBUG_PREPARED", "0")
                            PFD_SAVE(True, False) 'Azert, mert ezutan jon csak a bejelentkezes. Ha nem jelentkezik be, akkor nem menti el a valtozasokat.
                            DebugMode = False
                            DebugMode_Checked = True
                            OUT = DebugMode
                        Else
                            PFDlesen("DEBUG_PREPARED", wchar)
                            If wchar = "1" Then
                                DebugMode_Checked = True
                                DebugMode = True
                                OUT = DebugMode
                            Else
                                DebugMode = True
                                DebugMode_Checked = True
                                OUT = DebugMode
                                If MsgBox("Do you want to INIT Debug Mode? " + vbCrLf +
                                          "(If you choose YES, then the current settings of the fields will be written into the Table 'RS_Fields_DEBUG'." + vbCrLf +
                                          " If you didn't saved your earlier settings in DEBUG mode, then they will be lost.)", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                                    Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()
                                    Dim Result As Boolean

                                    DC.Qdf_set_SP(sqlComm, "DEBUG_MODE_INIT")
                                    Try
                                        Result = DC.Qdf_Execute("", sqlComm)
                                        MsgBox("DEBUG MODE is Initialized")
                                    Catch ex As Exception
                                        DoErrorMsgBox("FP_App.Is_DEBUG_MODE", Err.Number, Err.Description)
                                    End Try
                                    sqlComm.Dispose()
                                End If
                                PFDinsertOrUpdate("DEBUG_PREPARED", "1")
                                PFD_SAVE(True, False)
                            End If
                        End If
                    End If
                End If
            End If
        End If

        Is_DEBUG_MODE = OUT
    End Function
    Function IsVersionOK() As Boolean
        Dim OUT As Boolean = True

        Dim FP_Version
        Dim DB_VERS As String = ""
        Dim DB_SRV As String = ""
        Dim DB_Version As String

        FP_Version = String.Format("{0} {1}", VERS, SRV)

        ParmLesen("SEL", "VERSION", 0, DB_VERS)
        ParmLesen("SEL", "SRV", 0, DB_SRV)

        DB_Version = String.Format("{0} {1}", DB_VERS, DB_SRV)

        If DB_Version <> FP_Version Then
            OUT = False
            DoMyMsgBox(120, DB_Version + "|" + FP_Version)
        End If

        Return OUT
    End Function

    Protected Friend Sub FORM_ADD(ByVal FPf As FP_Form)
        If FPf Is Nothing Then
            MsgBox("FP_App.ADD: FPf is nothing.")
        Else
            If FPf.Frm Is Nothing Then
                MsgBox("FP_App.ADD: FPf.Frm is nothing.")
            Else
                Dim Frm_Handle As Long = Form_Handle(FPf.Frm)

                If Forms.ContainsKey(Frm_Handle) Then
                    MsgBox(String.Format("FP_App.ADD: FPf '{0}' already added to FP_App.", FPf.Frm.Name))
                Else
                    Forms.Add(Frm_Handle, FPf)
                End If
            End If
        End If
    End Sub
    Protected Friend Sub FORM_REMOVE(ByVal FPf As FP_Form)
        If FPf Is Nothing Then
            MsgBox("FP_App.REMOVE: FPf is nothing.")
        Else
            If FPf.Frm Is Nothing Then
                MsgBox("FP_App.REMOVE: FPf.Frm is nothing.")
            Else
                If Not Forms.ContainsKey(FPf.Frm_Handle) Then
                    MsgBox("FP_App.REMOVE: FPf.Frm in Forms not found.")
                Else
                    Forms.Remove(FPf.Frm_Handle)
                    If Forms_LastActivated IsNot Nothing Then
                        If FPf.Equals(Forms_LastActivated.Frm) Then
                            Forms_LastActivated = Nothing
                        End If
                    End If

                    If FPf.P_ShowCentralMenuOnClose Then
                        RAISEEVENT_Central_Menu_Show()
                    End If
                End If
            End If
        End If
    End Sub

    Public Function FORMS_BringToFront(ByVal Frm As Form) As Boolean
        'Eloterbe hozza a formot, ha mar meg van nyitva
        Dim OUT As Boolean = False

        If Not (Frm Is Nothing) Then
            With Frm
                If Frm.WindowState = FormWindowState.Minimized Then
                    Frm.WindowState = FormWindowState.Normal
                Else
                    .Show()
                End If

                .BringToFront()
                .Focus()
                OUT = True
            End With
        End If

        Return OUT
    End Function

    Public Function FORMS_BringToFront(ByVal FPf As FP_Form) As Boolean
        'Eloterbe hozza a formot, ha mar meg van nyitva
        Dim OUT As Boolean = False

        If Not (FPf Is Nothing) Then
            If Not (FPf.FrmMoveControl Is Nothing) Then
                Dim MoveControl_Enabled As Boolean = FPf.FrmMoveControl.Enabled

                FPf.FrmMoveControl.Enabled = False
                OUT = FORMS_BringToFront(FPf.Frm)
                FPf.FrmMoveControl.Enabled = MoveControl_Enabled
            End If
        End If

        Return OUT
    End Function

    Public Function FORMS_BringToFront(ByVal Name As String, Optional ByRef OUT_FPf As FP_Form = Nothing) As Boolean
        'Eloterbe hozza a formot, ha mar meg van nyitva. Ha a Shift le van nyomva, akkor hiaba van mar megnyitva, false-al ter vissza (igy mindenkeppen uj ablak fog megnyilni)
        Dim OUT As Boolean = False
        Dim ShiftPressed = My.Computer.Keyboard.ShiftKeyDown

        OUT_FPf = Nothing

        If ShiftPressed = False Then
            OUT_FPf = FORMS_GET_FROM_NAME(Name)
            OUT = FORMS_BringToFront(OUT_FPf)
        End If

        Return OUT
    End Function

    Public Function FORMS_BringToFront(ByVal Handle As Long) As Boolean
        'Eloterbe hozza a formot, ha mar meg van nyitva
        Dim OUT As Boolean = False

        If Forms.ContainsKey(Handle) Then
            OUT = FORMS_BringToFront(Forms(Handle).Frm)
        End If

        Return OUT
    End Function

    Protected Friend Function FORMS_CLOSE_ALL_FORM() As Boolean
        'A StartForm-ot es a FromForm-ot NEM zarja be! Ezt zard be a Me.Close eljarassal.
        Dim OUT As Boolean = True
        Dim ListOfForms As List(Of Long)

        ListOfForms = Forms.Keys.ToList
        Dim AktHandle As Long
        Dim StartForm_Handle As Long = 0

        If Not (StartForm Is Nothing) Then
            StartForm_Handle = Form_Handle(StartForm)
        End If

        For Each AktHandle In ListOfForms
            Dim DoIt As Boolean = True

            If StartForm_Handle <> 0 Then
                DoIt = (AktHandle <> StartForm_Handle)
            End If

            If DoIt Then
                If Forms.ContainsKey(AktHandle) Then
                    Dim Check_AktHandle As Long = Form_Handle(Forms(AktHandle).Frm)

                    If Check_AktHandle = 0 Then
                        FORM_REMOVE(Forms(AktHandle))
                    Else
                        If Not Forms(AktHandle).Frm.InvokeRequired Then
                            Forms(AktHandle).Frm.Close()
                            If Forms.ContainsKey(AktHandle) Then
                                OUT = False
                                Exit For
                            End If
                        End If
                    End If
                End If
            End If
        Next

        FORMS_CLOSE_ALL_FORM = OUT
    End Function
    Protected Friend Function RS_FILL_FIELD_PARAMETER_TABLE(ByVal ServerObject_Prefix As String, ByVal SubPrefix As String, ByRef DT As DataTable) As Boolean
        Dim OUT As Boolean

        If DC.P_USE_LocalDB Then
            Dim MySQL As String = String.Format("SELECT SeqNum, SubPrefix, FieldName, LabelName, Parent, CreateAtRuntime, CreateAtRuntime_FieldType, Visible, Mandatory, Locked, xType_VB, F_Format, DT_FixText_Key, DT_WHERE2, DT_ID_Field, COLORS, BG_Image, BG_Toggle, Label_Text, ShowInGrid, SavePoint, Forced_NextField, Tag, TabIndex, TabStop FROM RS_Fields WHERE ServerObject_Prefix = '{0}' And (SubPrefix = '' Or SubPrefix = '{1}') ORDER BY SubPrefix, FromTable, SeqNum", ServerObject_Prefix, SubPrefix)

            OUT = DC.LocalDB_SEL.Fill_DT(MySQL, DT)
        Else
            Dim MySQL As String = String.Format("SELECT SeqNum, SubPrefix, FieldName, LabelName, Parent, CreateAtRuntime, CreateAtRuntime_FieldType, Visible, Mandatory, Locked, xType_VB, F_Format, DT_FixText_Key, DT_WHERE2, DT_ID_Field, COLORS, BG_Image, BG_Toggle, Label_Text, ShowInGrid, SavePoint, Forced_NextField, Tag, TabIndex, TabStop FROM RS_Fields_view{2} WHERE ServerObject_Prefix = '{0}' And (SubPrefix = '' Or SubPrefix = '{1}') ORDER BY SubPrefix, FromTable, SeqNum", ServerObject_Prefix, SubPrefix, IIf(Is_DEBUG_MODE, "_DEBUG", ""))

            OUT = DC.Qdf_Fill_DT(MySQL, DT)
        End If

        Return OUT
    End Function

    Protected Friend Function RS_FILL_FIELD_ARRANGE_TABLE(ByVal ServerObject_Prefix As String, ByVal SubPrefix As String, ByRef DT As DataTable) As Boolean
        Dim OUT As Boolean

        If DC.P_USE_LocalDB Then
            Dim MySQL As String = String.Format("SELECT SeqNum, ControlName, ArrangeType, P1_Control,P1_Koo, P1_SpaceInPixel, P2_Control, P2_Koo, P2_SpaceInPixel, Percentage FROM {0} WHERE ServerObject_Prefix = '{1}' And (SubPrefix = '' Or SubPrefix = '{2}') ORDER BY SubPrefix, FromTable, SeqNum", "RS_ARRANGE", ServerObject_Prefix, SubPrefix)

            DC.LocalDB_SEL.Fill_DT(MySQL, DT)
        Else
            Dim ViewName As String = String.Format("dbo.RS_ARRANGE_view{0}", IIf(Is_DEBUG_MODE, "_DEBUG", ""))
            Dim MySQL As String = String.Format("SELECT SeqNum, ControlName, ArrangeType, P1_Control,P1_Koo, P1_SpaceInPixel, P2_Control, P2_Koo, P2_SpaceInPixel, Percentage FROM {0} WHERE ServerObject_Prefix = '{1}' And (SubPrefix = '' Or SubPrefix = '{2}') ORDER BY SubPrefix, FromTable, SeqNum", ViewName, ServerObject_Prefix, SubPrefix)

            DC.Qdf_Fill_DT(MySQL, DT)
        End If
        OUT = True

        RS_FILL_FIELD_ARRANGE_TABLE = OUT
    End Function

    Public Function SKIN_ADD(Name As String) As Boolean
        Dim OUT As Boolean = False
        Dim DLL_FileName As String = Name + ".dll"

        If SKIN_DLLs.ContainsKey(Name) Then
            OUT = True
        Else
            Dim SKIN_Path As String = Application.StartupPath + "\SKIN\"

            If Not Vorhanden(SKIN_Path + DLL_FileName) Then
                DoErrorMsgBox_Without_SQL_Connection("FP_App.SKIN_ADD", 0, String.Format("{0} in SKIN directory not found.", DLL_FileName))
            Else
                Dim NewASM As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(SKIN_Path + DLL_FileName)
                SKIN_DLLs.Add(Name, NewASM)
                OUT = True
            End If
        End If

        Return OUT
    End Function

    Public Function SKIN_getASM_And_OBJECTNAME(ByVal AsmAndObjectName As String, ByRef OUT_ASM As Reflection.Assembly, ByRef OUT_ObjectName As String) As Boolean
        Dim OUT As Boolean = False
        If AsmAndObjectName > "" Then
            Dim p As Integer = 0
            Dim ASM_Name As String = ""

            AsmAndObjectName = Replace(AsmAndObjectName, "<APP_NAME>", My.Application.Info.AssemblyName)
            p = InStr(AsmAndObjectName, "..")
            If p < 1 Then
                ASM_Name = "SEL_SKIN"
                OUT_ObjectName = ASM_Name + "." + AsmAndObjectName
            Else
                'Eredeti kod:
                '                ASM_Name = Mid(AsmAndObjectName, 1, p - 1)
                '                OUT_ObjectName = Replace(AsmAndObjectName, "..", ".")

                'Uj kod:
                'Ha a SKIN file nem vegzodik "_SKIN"-re, akkor kiegesziti azzal
                'Igy az uj, SKIN-es FP kompatibilis marad a regi, nem SKIN file-os FP-vel (Yusen)
                'A jovoben ez a logika felesleges.

                ASM_Name = Mid(AsmAndObjectName, 1, p - 1)

                Dim ObjectName As String = Mid(AsmAndObjectName, p + 2)

                If Strings.Right(ASM_Name, 5) <> "_SKIN" Then
                    ASM_Name += "_SKIN"
                End If

                OUT_ObjectName = ASM_Name + "." + ObjectName

            End If

            If Not SKIN_DLLs.ContainsKey(ASM_Name) Then
                DoErrorMsgBox_Without_SQL_Connection("FP_App.SKIN_getASM_And_OBJECTNAME", 0, String.Format("SKIN dll '{0}' not loaded. Use FP_App.SKIN_ADD method to add external SKIN-s!", ASM_Name))
            Else
                OUT_ASM = SKIN_DLLs(ASM_Name)
                OUT = True
            End If
        End If

        SKIN_getASM_And_OBJECTNAME = OUT
    End Function
#End Region
#Region "PRIVATE"

    Public Function SELEXPED_Folders_CREATE() As Boolean
        Dim OUT As Boolean = True

        Try
            If Not System.IO.Directory.Exists(SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.PREPARED_TEMPLATES)) Then
                System.IO.Directory.CreateDirectory(SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.PREPARED_TEMPLATES))
            End If

            If Not System.IO.Directory.Exists(SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.REPORT_TEMP)) Then
                System.IO.Directory.CreateDirectory(SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.REPORT_TEMP))
            End If

            If Not System.IO.Directory.Exists(SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP)) Then
                System.IO.Directory.CreateDirectory(SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP))
            End If

        Catch ex As Exception
            OUT = False
            DoErrorMsgBox("FP_App.SELEXPED_Folders_CREATE", Err.Number, Err.Description)
        End Try

        Return OUT
    End Function

    Public Function SELEXPED_Folders_Is_SELEXPED_Folder(MyFolder As String) As Boolean
        Dim OUT As Boolean = False

        MyFolder = Trim(MyFolder).ToUpper

        If Right(MyFolder, 1) <> "\" Then
            MyFolder += "\"
        End If

        If OUT = False Then OUT = (MyFolder = SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.PREPARED_TEMPLATES).ToUpper)
        If OUT = False Then OUT = (MyFolder = SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.REPORT_TEMP).ToUpper)
        If OUT = False Then OUT = (MyFolder = SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP).ToUpper)
        If OUT = False Then OUT = (MyFolder = Application.StartupPath.ToUpper)

        Return OUT
    End Function

    Private Function PFD_IsKeyInFile(ByVal MyKey As String) As Boolean
        PFD_IsKeyInFile = (MyKey = "HELPSTATUS" Or
                           MyKey = "TERMINAL" Or
                           MyKey = "LANDDIALOG" Or
                           MyKey = "USER" Or
                           MyKey = "INITPHASE" Or
                           MyKey = "DEBUG" Or
                           MyKey = "DEBUG_PREPARED" Or
                           Left(MyKey, 8) = "CONNECT_" Or
                           Left(MyKey, 8) = "LASTLOC_" Or
                           MyKey = "PATH_RATESERVER_LOG" Or
                           MyKey = "WEB_CURR_MANAGE" Or
                           MyKey = "DATA_FORMATS" Or
                           MyKey = "LOCALDB" Or
                           MyKey = "LAYOUT" Or
                           MyKey = "SEL_TASKMAN_FRM_PARAMS_0" Or
                           MyKey = "SEL_TASKMAN_FRM_PARAMS_1" Or
                           MyKey = "SEL_TASKMAN_FRM_PARAMS_2"
                          )
    End Function

    Function UnlockTerminal() As Boolean
        Dim OUT As Boolean

        If Not InitMinimalGlobals() Then
            Return False
        End If

        Dim sqlComm As SqlCommand = DC.CNN.CreateCommand()

        DC.Qdf_set_SP(sqlComm, "gl_UnlockTerminal")
        DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)

        CURSOR_SHOW_WAIT()
        Try
            OUT = DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE)
        Catch ex As Exception
            OUT = False
            DoErrorMsgBox("FP_App.UnlockTerminal", Err.Number, Err.Description)
        End Try
        sqlComm.Dispose()
        CURSOR_SHOW_DEFAULT()

        Return OUT
    End Function

    Public Function Users_setUserGlobals() As Boolean
        Dim OUT As Boolean = False

        If InitMinimalGlobals() Then
            SelUser = 0
            UserKurzName$ = String.Empty
            UserGruppe = 0
            UserStufe = 0
            UserRights = New Struct_Rights

            DC.CNN_OPEN()
            Dim MySQL As String
            If Organisation_Handling Then
                MySQL = String.Format("SELECT UserID, Stufe, UserGruppe, KurzName, Sprache, dbo.FN_USER_Name(UserID) User_Name, dbo.FN_User_Name_Foreign_Lang(UserID, 1) User_Name_Foreign_Lang_1, dbo.FN_User_Name_Foreign_Lang(UserID, 2) User_Name_Foreign_Lang_2, dbo.FN_USER_PHONE1(UserID) Phone1, dbo.FN_USER_PHONE2(UserID) Phone2, dbo.FN_USER_EMAIL(UserID)	Email, dbo.Rights_To_Organisation_IDS(UserID) Organisation_Rights_IDS FROM Terminals WITH (READUNCOMMITTED) INNER JOIN Users WITH (READUNCOMMITTED) ON Terminals.UserID=Users.ID WHERE TerminalName='{0}'", Terminal)
            Else
                MySQL = String.Format("SELECT UserID, Stufe, UserGruppe, KurzName, Sprache, dbo.FN_USER_Name(UserID) User_Name, dbo.FN_User_Name_Foreign_Lang(UserID, 1) User_Name_Foreign_Lang_1, dbo.FN_User_Name_Foreign_Lang(UserID, 2) User_Name_Foreign_Lang_2, dbo.FN_USER_PHONE1(UserID) Phone1, dbo.FN_USER_PHONE2(UserID) Phone2, dbo.FN_USER_EMAIL(UserID)	Email, '' Organisation_Rights_IDS FROM Terminals WITH (READUNCOMMITTED) INNER JOIN Users WITH (READUNCOMMITTED) ON Terminals.UserID=Users.ID WHERE TerminalName='{0}'", Terminal)
            End If

            Dim DRow_Users As DataRow = DC.Qdf_get_DataRow(MySQL)

            If Not (DRow_Users Is Nothing) Then
                With DRow_Users
                    SelUser = !UserID
                    PFDinsertOrUpdate("USER", Str(SelUser))

                    UserKurzName = !KurzName
                    UserGruppe = !UserGruppe
                    UserStufe = !Stufe
                    LandDialog = !Sprache
                    UserName = !User_Name
                    UserName_Foreign_Lang_1 = !User_Name_Foreign_Lang_1
                    UserName_Foreign_Lang_2 = !User_Name_Foreign_Lang_2
                    UserEmail = !Email
                    UserPhone1 = !Phone1
                    UserPhone2 = !Phone2
                    Organisation_Rights_IDS = !Organisation_Rights_IDS
                End With

                PFDinsertOrUpdate("LANDDIALOG", LandDialog$)

                MySQL = String.Format("SELECT * FROM UserStufen WITH (READUNCOMMITTED) WHERE ID = {0}", UserStufe)
                Dim DRow_User_Level As DataRow = DC.Qdf_get_DataRow(MySQL)

                If Not (DRow_User_Level Is Nothing) Then
                    With UserRights
                        .ORD_Handling = DRow_User_Level!SendungenModifizieren
                        .ORD_Handling_AfterClosing = DRow_User_Level!SendungenModifizierenNachAbschluss
                        .ORD_Temp_Handling = DRow_User_Level!SablonModifizieren
                        .ORD_Handling_All = DRow_User_Level!AlleSendungenMod
                        .AR_INV_Print = (DRow_User_Level!ReDrucken = 1 Or DRow_User_Level!ReDrucken = 2)
                        .AR_INV_Handling = DRow_User_Level!ReModifizieren
                        .Report_Print = DRow_User_Level!SpedibuchDrucken
                        .AP_INV_Handling = DRow_User_Level!EingangsReModifizieren
                        .AP_INV_Filing = (DRow_User_Level!ReDrucken = 1 Or DRow_User_Level!ReDrucken = 3)
                        .CUS_Add = DRow_User_Level!KundenstammErfassen
                        .CUS_Handling = DRow_User_Level!KundenstammModifizieren
                        .CUS_Del = DRow_User_Level!KundenstammLoschen
                        .CUS_Report = DRow_User_Level!KundenStammDrucken
                        .CUS_ACC_Handling = DRow_User_Level!KundenStammKreditLimitMod
                        .GOD_Handling = DRow_User_Level!ArtikelstammBearbeiten
                        .VCL_Handling = DRow_User_Level!LKWStammBearbeiten
                        .EMP_Handling = DRow_User_Level!PersonalStammBearbeiten
                        .EMP_Handling_All = DRow_User_Level!PersonalStammAlleDaten
                        .PAR_Handling = DRow_User_Level!ParaStammBearbeiten
                        .Distances_Handling = DRow_User_Level!EntfernungenBearbeiten
                        .RAT_Handling = DRow_User_Level!TarifenBearbeiten
                        .CUR_RAT_Handling = DRow_User_Level!WahrungStammBearbeiten
                        .CUR_Handling = DRow_User_Level!WahrungGrunddatenAndern
                        .BNK_AC_Handling = DRow_User_Level!BankkontoNrBearbeiten
                        .PA_Defs_Handling = DRow_User_Level!FixTexteBearbeiten
                        .USR_Handling = DRow_User_Level!KonfigAendern
                        .GLNr_Handling = DRow_User_Level!FIBUKontenEditieren
                        .BNK_Handling = DRow_User_Level!BankBearbeiten
                        .CBK_Handling = DRow_User_Level!KasseBearbeiten
                        .INV_Handling_Level = DRow_User_Level!RechnungDokuStandBis
                        .CBK_ACC_Handling_Level = DRow_User_Level!KasseDokuStandBis
                        .BNK_ACC_Handling_Level = DRow_User_Level!BankDokuStandBis
                        .RTS_MAN = DRow_User_Level!TurenBearbeiten
                        .RTS_Handling_AfterClosing = DRow_User_Level!TurenBearbeitenNachAbschluss
                        .WHS_BasData_MAN = DRow_User_Level!LgrGrundDatenBearbeiten
                        .WHS_Tran_MAN = DRow_User_Level!LgrBewegungenBearbeiten
                        .ExcelExport = DRow_User_Level!ExcelExport
                        .WorkInClosedPeriod = DRow_User_Level!WorkInClosedPeriod
                        .TranBetweenPeriods = DRow_User_Level!TransactionsBetweenPeriods
                        .CalcModifyAfterBilling = DRow_User_Level!CalcModifyAfterBilling
                        .DOCMAN_SecurityLevel = DRow_User_Level!DOCMAN_SecurityLevel
                        .UserSpec01 = DRow_User_Level!UserSpec01
                        .UserSpec02 = DRow_User_Level!UserSpec02
                        .UserSpec03 = DRow_User_Level!UserSpec03
                        .UserSpec04 = DRow_User_Level!UserSpec04
                        .UserSpec05 = DRow_User_Level!UserSpec05
                        .UserSpec06 = DRow_User_Level!UserSpec06
                    End With

                    SET_DATA_FORMATS(LandDialog)

                    OUT = True
                End If
            End If

        End If

        Return OUT
    End Function
#End Region

    Protected Overrides Sub Finalize()
        Files_EMF_Remove()
        Dispose()
        MyBase.Finalize()
    End Sub

    Private Sub DC_CNN_INITIALISED(sender As FP_DataConnect) Handles DC.CNN_INITIALISED
        DC.Qdf_Fill_DT("SELECT * FROM SEL_SYS_INSTALLED_PRODUCTS", DT_Installed_Products)
        CL_Statuscodes = New CL_Statuscodes()
        CL_AddressTypes = New CL_AddressTypes
        CL_TASK_TYPES = New CL_TASK_TYPES
        DT_Currencies_Refresh()
        DT_TRN_Params_Refresh()
    End Sub

    Public Function NEW_DEVELOPMENT_PARAMS_JSON_IsInstalled(NameOfDevelopment As String, Optional Tag_INSTALLED As String = "INSTALLED") As Boolean
        Dim OUT As Boolean = False

        Dim JSON As String = Trim(gl_FPApp.P.NEW_DEVELOPMENT_PARAMS_JSON)

        If JSON > "" Then
            Dim doc As System.Xml.XmlDocument = JsonConvert.DeserializeXmlNode(JSON)
            Dim nodelist As System.Xml.XmlNodeList = doc.GetElementsByTagName(NameOfDevelopment)

            If nodelist.Count > 0 Then
                Dim Node_INSTALLED As Xml.XmlNode = nodelist(0).SelectSingleNode(Tag_INSTALLED)

                If Node_INSTALLED IsNot Nothing Then
                    If TEXT_Is_YES(Node_INSTALLED.InnerText) Then
                        OUT = True
                    End If
                End If
            End If
        End If

        Return OUT
    End Function

    Public Function NEW_DEVELOPMENT_PARAMS_JSON_GET_PARAM(NameOfDevelopment As String, Tag As String) As String
        Dim OUT As String = ""

        Dim JSON As String = Trim(gl_FPApp.P.NEW_DEVELOPMENT_PARAMS_JSON)

        If JSON > "" Then
            Dim doc As System.Xml.XmlDocument = JsonConvert.DeserializeXmlNode(JSON)
            Dim nodelist As System.Xml.XmlNodeList = doc.GetElementsByTagName(NameOfDevelopment)

            If nodelist.Count > 0 Then
                Dim Node_Tag As Xml.XmlNode = nodelist(0).SelectSingleNode(Tag)

                If Node_Tag IsNot Nothing Then
                    OUT = nz(Node_Tag.InnerText, "")
                End If
            End If
        End If

        Return OUT
    End Function

End Class
