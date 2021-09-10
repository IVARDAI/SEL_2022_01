Imports System.Data
Imports System.Data.SqlClient

Public Class FP_Form
    Public Enum ENUM_Cash_State As Integer
        ACTIVE = 0
        CASHED = 1
    End Enum

    Public Event CONTROLS_INITIALIZED(ByVal sender_FPf As FP_Form)

    Public Event FORM_SAVE_ALL(ByVal sender_FPf As FP_Form, ByRef Handled As Boolean, ByRef Cancel As Integer)
    Public Event FORM_UNDO_ALL(ByVal sender_FPf As FP_Form, ByRef Handled As Boolean)
    Public Event FORM_ENABLE_CHANGED(ByVal sender_FPf As FP_Form)
    Public Event FORM_CLOSING(ByVal sender As Object, ByRef e As System.Windows.Forms.FormClosingEventArgs)
    Public Event FORM_CLOSED(ByVal sender As Object)
    Public Event FORM_KeyPreview_KeyPress(ByVal sender_FPf As FP_Form, ByRef sender As Object, ByRef e As System.Windows.Forms.KeyPressEventArgs)
    Public Event FORM_CASH_BEFORE_REOPEN(sender_FPf As FP_Form, ByRef Cancel As Boolean)
    Public Event FORM_CASH_REOPEN(sender_FPf As FP_Form)

    Public WithEvents Dlg_Btn_OK As PictureBox
    Public WithEvents Dlg_Btn_CANCEL As PictureBox
    Public WithEvents Btn_SAVE As PictureBox
    Public WithEvents Btn_HELP As FP_PictureBox
    Public Btn_Default As Control 'Nem kezelt enter lenyomasa eseten ez a control kap egy Click esemenyt.

    Public Cashable As Boolean = False
    Protected Friend Cash_Identifier As String = ""

    Private Keyboard_Enabled As Boolean = True

    Public CONTROLS As Dictionary(Of String, Control)
    Public PICTUREBOXES As New Dictionary(Of String, FP_PictureBox)
    Public SPECCONTROLS As New Dictionary(Of String, FP_SpecControl)

    Public ServerObject_Prefix As String
    Public FPs As New Dictionary(Of Integer, FP)

    Public FPApp As FP_App
    Public WithEvents Frm As Form

    Public DialogMode As Boolean = False
    Public ModuleIdentifier As String

    Public ContextMenu_DEBUG As ContextMenuStrip

    Public InfoPanel As FP_Services_DebugInfoPanel = Nothing

    Public Location_Save_On_Close As Boolean = True

    Protected Friend P_ActiveControl_Last_FP As FP = Nothing

    Private ShowCentralMenuOnClose As Boolean = True

    Private Cash_State As ENUM_Cash_State = ENUM_Cash_State.ACTIVE

    Protected Friend WithEvents P_Enabled_Form_Opacity As Form = Nothing
    Protected Friend P_Enabled_Form_Child As Form = Nothing
    Private P_Enabled_value As Boolean = True

    Private Current_BackGroundImageName As String = ""

    Public FrmMoveControl As FPf_Move

    Public ReadOnly Property P_Disposed As Boolean
        Get
            Dim OUT As Boolean = Disposed

            If OUT = False Then
                If FPApp Is Nothing Then
                    OUT = True
                Else
                    OUT = FPApp.P_Disposed
                End If
            End If

            Return OUT
        End Get
    End Property

    Public Property P_Keyboard_Enabled As Boolean
        Get
            Return Keyboard_Enabled
        End Get
        Set(value As Boolean)
            If value <> Keyboard_Enabled Then
                If Keyboard_Enabled = False Then
                    SendKeys.Flush()
                End If
                Keyboard_Enabled = value
            End If
        End Set
    End Property

    Public Sub RAISEEVENT_FORM_CASH_REOPEN()
        RaiseEvent FORM_CASH_REOPEN(Me)
    End Sub

    Public Sub RAISEEVENT_FORM_CASH_BEFORE_REOPEN(ByRef OUT_Cancelled As Boolean)
        OUT_Cancelled = False

        RaiseEvent FORM_CASH_BEFORE_REOPEN(Me, OUT_Cancelled)
    End Sub

    Private GRIDS_SAVE_ALL_Field_Length_ENABLED As Boolean = True
    Public Property P_GRIDS_SAVE_ALL_Field_Length_ENABLED
        Get
            Return GRIDS_SAVE_ALL_Field_Length_ENABLED
        End Get
        Set(value)
            GRIDS_SAVE_ALL_Field_Length_ENABLED = value
        End Set
    End Property

    Public Property P_Cash_State As ENUM_Cash_State
        Get
            Return Cash_State
        End Get

        Set(value As ENUM_Cash_State)
            If value <> Cash_State Then
                Cash_State = value

                Select Case value
                    Case ENUM_Cash_State.ACTIVE
                        FPApp.FORM_ADD(Me)
                        Cash_State = ENUM_Cash_State.ACTIVE
                        Frm.Visible = True

                    Case ENUM_Cash_State.CASHED
                        FPApp.FORM_REMOVE(Me)
                        Cash_State = ENUM_Cash_State.CASHED
                        FPApp.Cashed_Forms.Add_to_Cash(Me)
                        Frm.Visible = False
                        Frm.Parent = Nothing

                    Case Else
                        DoErrorMsgBox("FP_Form.P_Cash_State", 0, String.Format("Unknown Cash_State ({0})", value))
                End Select
            End If
        End Set
    End Property

    Public ReadOnly Property P_Layout_TextBox_NormalHeight() As Integer
        Get
            Return Math.Round(0.5 + (P_Layout_DPI_Factor_Y * 22))

            'Dim OUT As Integer = 22

            'If Frm.CurrentAutoScaleDimensions.Height > 0 Then
            'OUT = Math.Round(0.5 + OUT * (Frm.CurrentAutoScaleDimensions.Height / Frm.AutoScaleDimensions.Height), 0)
            'End If

            'Return OUT

            'P_Layout_TextBox_NormalHeight = (1.0 + Frm.CurrentAutoScaleDimensions.Height) * 22
            'P_Layout_TextBox_NormalHeight = Layout_TextBox_NormalHeight
        End Get
    End Property

    Public ReadOnly Property P_Layout_Form_HorizontalScrollBar_With As Integer
        Get
            Return Math.Round(0.5 + (P_Layout_DPI_Factor_Y * 17))

            'Return (1.0 + Frm.CurrentAutoScaleDimensions.Width) * 17
            'Return Layout_Form_ScrollBar_With
        End Get
    End Property

    Public ReadOnly Property P_Layout_DPI_Factor_X() As Double
        Get
            Dim OUT As Double = 1.0

            If Not (Frm Is Nothing) Then
                If Frm.CurrentAutoScaleDimensions.Width > 0.00005 And Frm.AutoScaleDimensions.Width > 0.00005 Then
                    OUT = (Frm.CurrentAutoScaleDimensions.Width / Frm.AutoScaleDimensions.Width)
                End If

                'Return Math.Round((0.0 + Layout_TextBox_NormalHeight) / 22, 4)
                'Return (1.0 + Frm.CurrentAutoScaleDimensions.Width)
            End If

            Return OUT
        End Get
    End Property

    Public ReadOnly Property P_Layout_DPI_Factor_Y() As Double
        Get
            Dim OUT As Double = 1.0

            If Not (Frm Is Nothing) Then
                If Frm.CurrentAutoScaleDimensions.Height > 0.00005 And Frm.AutoScaleDimensions.Height > 0.00005 Then
                    OUT = (Frm.CurrentAutoScaleDimensions.Height / Frm.AutoScaleDimensions.Height)
                End If

                Return OUT

                'Return Math.Round((0.0 + Layout_TextBox_NormalHeight) / 22, 4)
                'Return (1.0 + Frm.CurrentAutoScaleDimensions.Height)
            End If
        End Get
    End Property

    Public Property P_Cash_Identifier As String
        Get
            Dim OUT As String = Cash_Identifier

            If Cash_Identifier = "" Then
                OUT = ServerObject_Prefix
                Dim MyRoot_FP As FP = ROOT_FP()

                If Not (MyRoot_FP Is Nothing) Then
                    OUT += "|" + MyRoot_FP.SubPrefix
                End If
            End If

            Return OUT
        End Get
        Set(value As String)
            Cash_Identifier = value
        End Set
    End Property

    Protected Friend Function FP_CONTROL_Is_In_Other_FP(MyFPc As FP_Control) As Boolean
        Dim OUT As Boolean = False

        If Not (P_ActiveControl_Last_FP Is Nothing) Then
            If Not (MyFPc Is Nothing) Then
                OUT = Not (MyFPc.FP.Equals(P_ActiveControl_Last_FP))
            End If
        End If

        Return OUT
    End Function

    Public Property P_ActiveControl() As FP_Control
        Get
            P_ActiveControl = ActiveControl
        End Get

        Set(ByVal value As FP_Control)
            Dim DoIt As Boolean = True

            If FP_CONTROL_Is_In_Other_FP(value) Then
                If Not P_ActiveControl_Last_FP.FORM_RECORDS_SAVE_CURRENT() Then
                    DoIt = False
                    If Not (ActiveControl Is Nothing) Then
                        FOCUS_ON_AT_THE_END(ActiveControl.c, , , True)
                    End If
                End If
            End If

            If Not (value Is Nothing) Then
                P_ActiveControl_Last_FP = value.FP
            End If

            If DoIt Then
                If Not value Is Nothing Then
                    If value.FP.FORM_IsSubForm Then
                        If Not value.FP.Parent_FP.FORM_RECORDS_SAVE_CURRENT Then
                            DoIt = False
                            If Not ActiveControl Is Nothing Then
                                If Not ActiveControl.c Is Nothing Then
                                    FOCUS_ON_AT_THE_END(ActiveControl.c, , , True)
                                End If
                            End If
                        Else
                            If Not value.FP.Parent_FP.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                                DoIt = False
                                If Not ActiveControl Is Nothing Then
                                    If Not ActiveControl.c Is Nothing Then
                                        FOCUS_ON_AT_THE_END(ActiveControl.c, , , True)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            If DoIt Then
                ActiveControl = value
            End If
        End Set
    End Property

    Public ReadOnly Property P_ParentFPf As FP_Form
        Get
            Dim OUT As FP_Form = Nothing

            For Each Form_ID As Long In FPApp.Forms.Keys
                Dim Current_FPf As FP_Form = FPApp.Forms(Form_ID)

                If Not (Current_FPf.P_Enabled_Form_Child Is Nothing) Then
                    If Current_FPf.P_Enabled_Form_Child.Equals(Frm) Then
                        OUT = Current_FPf
                        Exit For
                    End If
                End If
            Next

            Return OUT
        End Get
    End Property

    Public Property P_VISIBLE As Boolean
        Get
            P_VISIBLE = Frm.Visible
        End Get
        Set(value As Boolean)
            If Not (Frm Is Nothing) Then
                Frm.Visible = value
                If Not (P_Enabled_Form_Opacity Is Nothing) Then
                    P_Enabled_Form_Opacity.Visible = value
                End If
            End If
        End Set
    End Property

    Public Property P_ENABLED() As Boolean
        Get
            P_ENABLED = P_Enabled_value
        End Get
        Set(ByVal value As Boolean)
            If value <> P_ENABLED Then
                If value = True Then
                    If P_Enabled_value = False Then
                        P_Enabled_value = True

                        RaiseEvent FORM_ENABLE_CHANGED(Me)

                        If Not (P_Enabled_Form_Opacity Is Nothing) Then
                            P_Enabled_Form_Opacity.Close()
                            P_Enabled_Form_Opacity.Dispose()
                            P_Enabled_Form_Opacity = Nothing
                        End If

                        If Not (P_Enabled_Form_Child Is Nothing) Then
                            P_Enabled_Form_Child = Nothing
                        End If
                    End If
                Else
                    If P_Enabled_value = True Then
                        P_Enabled_value = False

                        RaiseEvent FORM_ENABLE_CHANGED(Me)

                        P_Enabled_Form_Opacity = FPApp.ShowDialogForm_getOpacityForm(Frm)
                    End If
                End If
            End If
        End Set
    End Property

    Public Property P_ShowCentralMenuOnClose() As Boolean
        Get
            Return ShowCentralMenuOnClose
        End Get
        Set(value As Boolean)
            ShowCentralMenuOnClose = value
        End Set
    End Property

    Sub New(ByVal MyServerObject_Prefix As String, ByVal MyFPApp As FP_App, ByVal MyFrm As System.Windows.Forms.Form, ByVal MyDialogMode As Boolean, Optional ByVal MyBackGroundImageName As String = "")
        New_INIT("", MyServerObject_Prefix, MyFPApp, MyFrm, MyDialogMode, MyBackGroundImageName)
    End Sub

    Sub New(ByVal MyModuleIdentifier As String, ByVal MyServerObject_Prefix As String, ByVal MyFPApp As FP_App, ByVal MyFrm As System.Windows.Forms.Form, ByVal MyDialogMode As Boolean, Optional ByVal MyBackGroundImageName As String = "")
        New_INIT(MyModuleIdentifier, MyServerObject_Prefix, MyFPApp, MyFrm, MyDialogMode, MyBackGroundImageName)
    End Sub

    Private Sub New_INIT(ByVal MyModuleIdentifier As String, ByVal MyServerObject_Prefix As String, ByVal MyFPApp As FP_App, ByVal MyFrm As System.Windows.Forms.Form, ByVal MyDialogMode As Boolean, Optional ByVal MyBackGroundImageName As String = "")
        ModuleIdentifier = MyModuleIdentifier
        ServerObject_Prefix = MyServerObject_Prefix

        FPApp = MyFPApp

        Frm = MyFrm

        If Not (Frm Is Nothing) Then
            Frm_Handle = Form_Handle(MyFrm)
            MyFrm.KeyPreview = True
            FrmMoveControl = New FPf_Move(FPApp, Frm)

            LOCATION_MOVE_TO_SAVED_POS()
        End If

        FPApp.FORM_ADD(Me)

        FORM_DATETIME_MonthCalendar = New MonthCalendar
        FIELD_VISIBLE(FORM_DATETIME_MonthCalendar, False)

        Marker_sign = New PictureBox
        With Marker_sign
            .Parent = Frm
            .Visible = False
            .Size = New Size(10, 10)
            .Image = FPApp.SKIN_GET_IMAGE("but_Marker_10x10.png")
        End With

        DialogMode = MyDialogMode

        Frm.Controls.Add(FORM_DATETIME_MonthCalendar)

        If MyBackGroundImageName > "" Then
            BACKGROUND_SET(MyBackGroundImageName)
        End If

        If FPApp.Is_DEBUG_MODE() Then
            CONTEXTMENU_DEBUG_SETUP()
            Frm.ContextMenuStrip = ContextMenu_DEBUG
        End If
    End Sub

    Public Sub Dispose()
        If Not Disposed Then
            FPApp.FORM_REMOVE(Me)

            Dlg_Btn_OK = Nothing
            Dlg_Btn_CANCEL = Nothing
            Btn_SAVE = Nothing

            If Not Btn_HELP Is Nothing Then
                Btn_HELP.Dispose()
                Btn_HELP = Nothing
            End If

            If Not (HELP_Frm Is Nothing) Then
                HELP_Frm.Close()
                HELP_Frm.Dispose()
                HELP_Frm = Nothing
            End If

            If Not (FrmMoveControl Is Nothing) Then
                FrmMoveControl.Dispose()
                FrmMoveControl = Nothing
            End If

            Frm = Nothing

            Menu_Properties = Nothing
            Menu_SAVE = Nothing
            FOCUS_ON_AT_THE_END_Timer = Nothing

            If Not (P_Enabled_Form_Opacity Is Nothing) Then
                P_Enabled_Form_Opacity.Close()
                P_Enabled_Form_Opacity.Dispose()
                P_Enabled_Form_Opacity = Nothing
            End If

            If Not (P_Enabled_Form_Child Is Nothing) Then
                P_Enabled_Form_Child = Nothing
            End If

            Disposed = True
        End If
    End Sub


    Public Function GET_Rect_Next_to_me(ByVal MyRectSize As Size, ByVal Orientation As ENUM_Direction, Optional ByVal Distance As Integer = 0, Optional ByRef OUT_Next_Orientation As ENUM_Direction = ENUM_Direction.None) As FP_L_Rect
        Dim Frm_rect As New FP_L_Rect(Frm.Location, Frm.Size)
        Dim Screen_rect As FP_L_Rect = FPApp.SCREEN_GET_WorkingArea(Frm)

        Return Frm_rect.GET_Rect_Next_to_me(Screen_rect, MyRectSize, Orientation, Distance, OUT_Next_Orientation)
    End Function

    Public Sub DoErrorMsgBox(ByVal ErrPlace As String, ByVal ErrNr As Long, ByVal ErrDescription As String, Optional ByVal WriteToTransactErrors As Boolean = True)
        FPApp.DoErrorMsgBox(ErrPlace, ErrNr, ErrDescription, WriteToTransactErrors, ModuleIdentifier)
    End Sub

    Public Function DoMyMsgBox(ByVal DialNr As Long, Optional ByVal Ersetz As String = "", Optional ByVal Button1 As String = "SEQ,OK", Optional ByVal Button2 As String = "", Optional ByVal Button3 As String = "") As Long
        Dim OUT As Long = 0
        Try
            Dim MyMsgBox As New FP_MsgBox(FPApp, ModuleIdentifier, DialNr, Ersetz, Button1, Button2, Button3)

            If MyMsgBox.P_IsOK Then
                CURSOR_SHOW_DEFAULT()
                gl_Data_Binded = False
                MyMsgBox.Select()
                OUT = FPApp.ShowDialogForm(MyMsgBox)
                gl_Data_Binded = True
            End If

        Catch ex As Exception
            FPApp.DoMyMsgBox_From_Resources(1000) 'Error by showing Dialog
        End Try
        Return OUT
    End Function

    Protected Friend Sub FILTER_TEXT_SET(ByVal NameOfField As String, ByVal FilterText As String)
        For Each Current_FP_ID As Integer In FPs.Keys
            With FPs(Current_FP_ID)
                If .GRID_EXISTS Then
                    .GRID.FILTER_FIELDS_SET_FILTER_TEXT(NameOfField, FilterText)
                End If
            End With
        Next
    End Sub

    Public Sub HELP_SHOW(ByVal c As Control)
        If Not (HELP_Frm Is Nothing) Then
            HELP_Frm.HELP_SHOW(c)
        End If
    End Sub

    Public Function FORM_GET_HEAD_FPs() As Dictionary(Of Integer, FP)
        Dim OUT As New Dictionary(Of Integer, FP)

        For Each Current_FP_ID As Integer In FPs.Keys
            With FPs(Current_FP_ID)
                If Not .FORM_IsSubForm Then
                    OUT.Add(Current_FP_ID, FPs(Current_FP_ID))
                End If
            End With
        Next

        Return OUT
    End Function

    Public Function BACKGROUND_SET(ByVal BackgroundImageName_with_ASM As String) As Boolean
        'a hatterkepet kiterjesztessel egyutt kell megadni! (Peldaul: 'kep.png'
        'A 'kep.png' formatum azt jelenti, hogy a kep.png a SEL_FP.dll-ben van benne.
        'A SEL_FP.dll-ben valaszthato kepek a SEL_FP/FP_SKIN konyvtarban talalhatok.
        'Ha sajat projectunkben hoztunk letre kepeket, akkor azt a kovetkezo formaban kell megadni:
        '            "<My.Application.Info.AssemblyName>..kep.png"
        'A kod felismeri es kicsereli az "<APP_NAME>" elotagot a project sajat elotagjara. Vagyis helyes a kovetkezo kifejezes: "<APP_NAME>..kep.png"
        'A project-ek APP_NAME elotagjat a Project/Properties menupont alatt az "Application" fulon lehet beallitani illetve kiolvasni.
        'Ne felejtsd el, hogy a kepeket, amiket hozzaadsz a project-hez, a property ablakban allitsd "Embended resource"-ra, maskeppen NEM fog mukodni!!!
        Dim OUT As Boolean = True

        If Current_BackGroundImageName <> BackgroundImageName_with_ASM Then
            If BackgroundImageName_with_ASM = "" Then
                Frm.BackgroundImage = Nothing
                Current_BackGroundImageName = ""
                OUT = True
            Else
                Dim asm As Reflection.Assembly = Nothing
                Dim BackgroundImage_ImageName As String = ""
                Dim backGroundImage As Bitmap = Nothing

                If Not FPApp.Bitmaps_LOAD(BackgroundImageName_with_ASM, backGroundImage) Then
                    OUT = FPApp.SKIN_getASM_And_OBJECTNAME(BackgroundImageName_with_ASM, asm, BackgroundImage_ImageName)
                    If OUT = True Then
                        Try
                            backGroundImage = New Bitmap(asm.GetManifestResourceStream(BackgroundImage_ImageName))

                        Catch ex As Exception
                            OUT = False
                            FPApp.DoErrorMsgBox("FP_Form.BACKGROUND_SET", 0, String.Format("Set form background to {0} was not successfull.", BackgroundImageName_with_ASM))
                        End Try
                    End If

                    If OUT = True Then
                        FPApp.Bitmaps_SAVE(BackgroundImageName_with_ASM, backGroundImage)
                    End If
                End If

                If OUT Then
                    Frm.BackgroundImage = backGroundImage
                    Current_BackGroundImageName = BackgroundImageName_with_ASM
                    Frm.BackgroundImageLayout = ImageLayout.Stretch
                End If
            End If
        End If

        BACKGROUND_SET = OUT

    End Function

    Public Function INIT_CONTROLS(ByVal Control_Collection As Struct_FP_FORM_CONTROLS_COLLECTION) As Boolean
        Dim OUT As Boolean = True

        CONTROLS_WRITE_ALL_CONTROLS_TO_DIC(Frm, CONTROLS)

        Dim AktKey As String

        For Each AktKey In CONTROLS.Keys
            If TypeOf (CONTROLS(AktKey)) Is TabControl Then
                If SPECCONTROLS.ContainsKey(AktKey) Then
                    FPApp.DoErrorMsgBox("FP_Form.INIT_CONTROLS", 0, String.Format("DIC SPECCONTROLS already contains key {0}", AktKey))
                Else
                    SPECCONTROLS.Add(AktKey, New FP_SpecControl(Me, CONTROLS(AktKey)))
                End If
            ElseIf TypeOf (CONTROLS(AktKey)) Is SplitContainer Then
                If SPECCONTROLS.ContainsKey(AktKey) Then
                    FPApp.DoErrorMsgBox("FP_Form.INIT_CONTROLS", 0, String.Format("DIC SPECCONTROLS already contains key {0}", AktKey))
                Else
                    SPECCONTROLS.Add(AktKey, New FP_SpecControl(Me, CONTROLS(AktKey)))
                End If
            End If
        Next

        CONTROLS_FROM_RS_SET()

        With Control_Collection
            If Not (.Dlg_Btn_OK Is Nothing) Then
                If Not DialogMode Then
                    FPApp.DoErrorMsgBox("FP_Form.INIT_CONTROLS", 0, "Dialog_ButtonOK megadasa csak DialogMode=True eseten lehetseges.")
                Else
                    Dlg_Btn_OK = .Dlg_Btn_OK
                End If
            End If

            If Not (.Dlg_Btn_CANCEL Is Nothing) Then
                If Not DialogMode Then
                    FPApp.DoErrorMsgBox("FP_Form.INIT_CONTROLS", 0, "Dialog_ButtonCANCEL megadasa csak DialogMode=True eseten lehetseges.")
                Else
                    If Frm.ControlBox = True Then
                        FPApp.DoErrorMsgBox("FP_Form.INIT_CONTROLS", 0, "Dialog_ButtonCANCEL megadasa eseten a Form ControlBox tulajdonsaga legyen False! (Ellentmondas lehet, hogy a Form-on kint van egy Cancel gomb, es a jobb felso sarokban levo gombbal is be lehet csukni!)")
                    End If
                    Dlg_Btn_CANCEL = .Dlg_Btn_CANCEL
                End If
            End If

            Btn_SAVE = .Btn_SAVE
            Btn_Default = .Btn_Default

            If Not (.Btn_HELP Is Nothing) Then
                If Not PICTUREBOXES.ContainsKey(.Btn_HELP.Name) Then
                    FPApp.DoErrorMsgBox("FP_Form.INIT_CONTROLS", 0, String.Format("A megadott '{0}' button nem lett megadva az RS_Fields tablaban.", .Btn_HELP.Name))
                Else
                    Btn_HELP = PICTUREBOXES(.Btn_HELP.Name)
                    HELP_Frm = New FP_HELP(Me)
                    HELP_Frm.ADD_HELP_DICTIONARY(ServerObject_Prefix, "")
                End If
            End If
        End With

        CONTROLS_ARRANGE(ServerObject_Prefix, "", ARRANGE_DT, "")

        SET_LABELTEXTS_FROM_SEQ(ServerObject_Prefix, "", "")

        RaiseEvent CONTROLS_INITIALIZED(Me)

        INIT_CONTROLS = OUT
    End Function

    Public Function SET_LABELTEXTS_FROM_SEQ(ByVal VB_SEQ_Key As String, ByVal SubPrefix As String, FieldPrefix As String) As Boolean
        Dim OUT As Boolean = True
        Dim Real_VB_SEQ_Key As String = "VBSEQ_" + VB_SEQ_Key
        Dim MySEQ As New FP_SEQ(FPApp, Real_VB_SEQ_Key, String.Format("Text2='{0}'", SubPrefix))
        Dim Row As DataRow

        For Each Row In MySEQ.DT_SEQ.Rows
            Dim FieldName = Row!Text1
            Dim c As Control

            If FieldName = "#FORM#" Then
                Frm.Text = FPApp.Text_Replace_Standard_Params(Row!Text3)
            Else
                Dim RealFieldName As String = FieldPrefix + FieldName
                If Not CONTROLS.ContainsKey(RealFieldName) Then
                    'Nothing to do - ML: van olyan testreszabott DoFilter, amiben nincsen meg minden szabvanyos szuro. Ezek elnevezesenel is pl. hibat dob, amit nem kene.
                    'Regen hibajelzes volt: FPApp.DoErrorMsgBox("FP_Form.SET_LABELTEXTS_FROM_SEQ", 0, String.Format("Field '{0}' not found. (SEQ_Key: '{1}')", RealFieldName, Real_VB_SEQ_Key))
                Else
                    c = CONTROLS(RealFieldName)

                    Dim gl_DataBinded_OLD = gl_Data_Binded

                    gl_Data_Binded = False
                    c.Text = FPApp.Text_Replace_Standard_Params(Row!Text3)
                    gl_Data_Binded = gl_DataBinded_OLD
                End If
            End If
        Next

        Return OUT
    End Function

    Private Function CONTROLS_GET_FPp_FROM_CONTROL(ByVal p As PictureBox, ByRef OUT_FPp As FP_PictureBox) As Boolean
        Dim OUT As Boolean = False

        OUT_FPp = Nothing

        If PICTUREBOXES.ContainsKey(p.Name) Then
            If Not (PICTUREBOXES(p.Name).c Is Nothing) Then
                OUT_FPp = PICTUREBOXES(p.Name)
                OUT = True
            End If
        End If

        If Not OUT Then
            For Each Current_FP_ID As Integer In FPs.Keys
                With FPs(Current_FP_ID)
                    Dim ControlName_To_Find As String = .CONTROLS_GET_FieldName_Without_FieldPrefix(p.Name)

                    If .PICTUREBOXES.ContainsKey(ControlName_To_Find) Then
                        If Not (.PICTUREBOXES(ControlName_To_Find).c Is Nothing) Then
                            OUT_FPp = .PICTUREBOXES(ControlName_To_Find)
                            OUT = True

                            Exit For
                        End If
                    End If
                End With
            Next
        End If

        CONTROLS_GET_FPp_FROM_CONTROL = OUT
    End Function

    Private Function CONTROLS_GET_FPc_FROM_CONTROL(ByVal c As Control, ByRef OUT_FPc As FP_Control) As Boolean
        Dim OUT As Boolean = False

        OUT_FPc = Nothing

        If Not (c Is Nothing) Then
            For Each Current_FP_ID As Integer In FPs.Keys
                With FPs(Current_FP_ID)
                    Dim ControlName_To_Find As String = .CONTROLS_GET_FieldName_Without_FieldPrefix(c.Name)

                    If .CONTROLS.ContainsKey(ControlName_To_Find) Then
                        If Not (.CONTROLS(ControlName_To_Find).c Is Nothing) Then
                            OUT_FPc = .CONTROLS(ControlName_To_Find)
                            OUT = True

                            Exit For
                        End If
                    End If
                End With
            Next
        End If

        Return OUT
    End Function

    Private Function CONTROLS_GET_FPc_FROM_CONTROL_LABEL(ByVal L As Label, ByRef OUT_FPc As FP_Control) As Boolean
        Dim OUT As Boolean = False

        OUT_FPc = Nothing

        For Each Current_FP_ID As Integer In FPs.Keys
            With FPs(Current_FP_ID)
                For Each AktKey As String In .CONTROLS.Keys
                    If L.Equals(.CONTROLS(AktKey).c_Label) Then
                        OUT_FPc = .CONTROLS(AktKey)
                        OUT = True

                        Exit For
                    End If
                Next
            End With
        Next

        CONTROLS_GET_FPc_FROM_CONTROL_LABEL = OUT
    End Function

    Public Function CONTROLS_GET_FPo_FROM_CONTROL(ByVal c As Control, ByRef OUT_FPo As FP_ControlObject) As Boolean
        Dim OUT As Boolean = False

        OUT_FPo = Nothing

        If Not c Is Nothing Then
            If TypeOf (c) Is PictureBox Then
                OUT = CONTROLS_GET_FPp_FROM_CONTROL(c, OUT_FPo)
            Else
                If CONTROLS_GET_FPc_FROM_CONTROL(c, OUT_FPo) Then
                    OUT = True
                Else
                    If TypeOf c Is Label Then
                        OUT = CONTROLS_GET_FPc_FROM_CONTROL_LABEL(c, OUT_FPo)
                    End If
                End If
            End If
        End If

        CONTROLS_GET_FPo_FROM_CONTROL = OUT
    End Function

    Public Function CONTROLS_GET_FROM_NAME(ByVal c As Control, ByVal PrevControl As Control, ByVal PrevLabel As Label, ByVal ControlName As String, ByRef OUT_Control As Control) As Boolean
        Dim OUT As Boolean = False

        If ControlName = "#FORM#" Then
            OUT_Control = Frm
            OUT = True
        ElseIf ControlName = "#PARENT#" Then
            If Not (c Is Nothing) Then
                OUT_Control = ARRANGE_getParent(c)
                OUT = True
            End If
        ElseIf ControlName = "#PREV#" Then
            If PrevControl Is Nothing Then
                FPApp.DoErrorMsgBox("FP_Form.CONTROLS_GET_FROM_NAME", 0, "You have a declaration with '#PREV#', but PrevControl is nothing.")
            Else
                OUT_Control = PrevControl
                OUT = True
            End If
        ElseIf ControlName = "#PREV_LABEL#" Then
            If PrevLabel Is Nothing Then
                FPApp.DoErrorMsgBox("FP_Form.CONTROLS_GET_FROM_NAME", 0, "You have a declaration with '#PREV_LABEL#', but PrevLabel is nothing.")
            Else
                OUT_Control = PrevLabel
                OUT = True
            End If
        Else
            If Trim(ControlName) = "" Then
                OUT_Control = Nothing
                OUT = True
            Else
                If Not CONTROLS.ContainsKey(ControlName) Then
                    FPApp.DoErrorMsgBox("FP_Form.CONTROLS_GET_FROM_NAME", 0, String.Format("Control '{0}' not found.", ControlName))
                Else
                    OUT_Control = CONTROLS(ControlName)
                    OUT = True
                End If
            End If
        End If

        CONTROLS_GET_FROM_NAME = OUT
    End Function

    Public Sub CONTROLS_ARRANGE_ALL()
        CONTROLS_ARRANGE(ServerObject_Prefix, "", ARRANGE_DT, "")

        For Each Current_FP_ID As Integer In FPs.Keys
            FPs(Current_FP_ID).CONTROLS_ARRANGE()
        Next
    End Sub

    Public Function PICTUREBOXES_ADD(ByVal MyFPp As FP_PictureBox) As Boolean
        Dim OUT As Boolean = False

        If PICTUREBOXES.ContainsKey(MyFPp.c.Name) Then
            FPApp.DoErrorMsgBox("FP_Form.PICTUREBOXES_ADD", 0, String.Format("Field '{0}' is already exists.", MyFPp.c.Name))
        Else
            PICTUREBOXES.Add(MyFPp.c.Name, MyFPp)
            OUT = True
        End If

        PICTUREBOXES_ADD = OUT
    End Function
    Public Function PICTUREBOXES_GET(ByVal Name As String) As FP_PictureBox
        Dim OUT As FP_PictureBox = Nothing

        If Not PICTUREBOXES.ContainsKey(Name) Then
            FPApp.DoErrorMsgBox("FP_Form.PICTUREBOX_GET", 0, String.Format("Control '{0}' does not exists.", Name))
        Else
            OUT = PICTUREBOXES(Name)
        End If

        PICTUREBOXES_GET = OUT
    End Function

    Public Function PICTUREBOXES_GET_FPp_FROM_CONTROL(ByVal c As Control) As FP_PictureBox
        Dim OUT As FP_PictureBox = Nothing

        If Not (c Is Nothing) Then
            If TypeOf (c) Is PictureBox Then
                If PICTUREBOXES.ContainsKey(c.Name) Then
                    OUT = PICTUREBOXES(c.Name)
                Else
                    For Each Current_FP_ID As Integer In FPs.Keys
                        With FPs(Current_FP_ID)
                            If .PICTUREBOXES.ContainsKey(c.Name) Then
                                If Not (.PICTUREBOXES(c.Name).c Is Nothing) Then
                                    OUT = .PICTUREBOXES(c.Name)
                                    Exit For
                                End If
                            End If
                        End With
                    Next
                End If
            End If
        End If

        Return OUT
    End Function

    Public Sub PICTUREBOXES_REMOVE(ByVal Name As String)
        If Not PICTUREBOXES.ContainsKey(Name) Then
            FPApp.DoErrorMsgBox("FP_Form.PICTUREBOXES_REMOVE", 0, String.Format("Picturebox '{0}' not found.", Name))
        Else
            With PICTUREBOXES(Name)
                If Not (.c Is Nothing) Then
                    CONTROLS_REMOVE(.c.Name)
                    If .CreatedAtRuntime Then
                        .c.Dispose()
                    End If
                    PICTUREBOXES.Remove(Name)
                End If
            End With
        End If
    End Sub

    Public Function GET_FP(ByVal MyServerObject_Prefix As String, ByVal MySubPrefix As String, MyFieldPrefix As String) As FP
        Dim OUT As FP = Nothing
        Dim Current_FP_ID As Integer

        For Each Current_FP_ID In FPs.Keys
            With FPs(Current_FP_ID)
                If .ServerObject_Prefix = MyServerObject_Prefix And .SubPrefix = MySubPrefix And .P_FieldPrefix = MyFieldPrefix Then
                    OUT = FPs(Current_FP_ID)
                    Exit For
                End If
            End With
        Next

        GET_FP = OUT
    End Function

    Public Function GET_FP_BY_ALIAS(My_FP_Alias As String) As FP
        Dim OUT As FP = Nothing

        For Each Current_FP_ID In FPs.Keys
            With FPs(Current_FP_ID)
                If .FP_ALIAS = My_FP_Alias Then
                    OUT = FPs(Current_FP_ID)
                    Exit For
                End If
            End With
        Next

        Return OUT
    End Function

    Public Function Has_UnboundFP() As Boolean
        Dim OUT As Boolean = False

        For Each Current_FP_ID In FPs.Keys
            If FPs(Current_FP_ID).UnboundForm Then
                OUT = True
                Exit For
            End If
        Next

        Has_UnboundFP = OUT
    End Function

    Public Sub ActiveControl_OLDVALUE_SET_FROM_CURRENT_VALUE()
        ActiveControl.OLDVALUE_SET_FROM_CURRENT_VALUE()
    End Sub

    Public Function ActiveControl_VALIDATE() As Boolean
        Dim OUT As Boolean = True

        If FPc_HAS_FIELD(ActiveControl) Then
            If ActiveControl.Value_Validated = False Then
                Dim ee1 As New System.ComponentModel.CancelEventArgs
                Dim MyControl As FP_Control = ActiveControl

                MyControl.EVENT_VALIDATING(ActiveControl.c, ee1)
                OUT = (Not ee1.Cancel)
                If OUT = True Then
                    Dim ee2 As New System.EventArgs
                    MyControl.EVENT_VALIDATED(ActiveControl.c, ee2)
                End If
            End If
        End If

        ActiveControl_VALIDATE = OUT
    End Function

    Public Sub COMBOBOX_REFRESH_ALL()
        For Each Current_FP_ID In FPs.Keys
            FPs(Current_FP_ID).CONTROLS_REFRESH_DT_ALL()
        Next
    End Sub

    Public Sub COMBOBOX_REFRESH_DT_FixText_Key(MyDT_FixText_Key As String)
        For Each Current_FP_ID In FPs.Keys
            FPs(Current_FP_ID).CONTROLS_REFRESH_DT_FixText_Key(MyDT_FixText_Key)
        Next
    End Sub

    Public Sub FOCUS_ON_FIRST_CONTROL()
        Dim MinTabIndex As Integer = Integer.MaxValue
        Dim c As Control = Nothing

        For Each AktKey As String In CONTROLS.Keys
            If TypeOf (CONTROLS(AktKey)) Is TextBox _
                   Or TypeOf (CONTROLS(AktKey)) Is ComboBox _
                   Or TypeOf (CONTROLS(AktKey)) Is RichTextBox _
                   Or TypeOf (CONTROLS(AktKey)) Is CheckBox Then

                If CONTROLS(AktKey).Visible And CONTROLS(AktKey).TabStop Then
                    If MinTabIndex > CONTROLS(AktKey).TabIndex Then
                        c = CONTROLS(AktKey)
                        MinTabIndex = c.TabIndex
                    End If
                End If
            End If
        Next

        If Not (c Is Nothing) Then
            FOCUS_ON_AT_THE_END(c)
        End If
    End Sub

    Public Sub DoMyMsgBox_AT_THE_END(ByVal MsgNum As Long, Optional ByVal MsgParams As String = "", Optional ByVal OnlyWhenNotSet As Boolean = False)
        Dim DoIt As Boolean = True

        If FOCUS_ON_AT_THE_END_Timer.Enabled Then
            If FOCUS_ON_AT_THE_END_MsgNum <> 0 Then
                If OnlyWhenNotSet = False Then
                    DoIt = False
                End If
            End If
        End If

        If DoIt Then
            If FOCUS_ON_AT_THE_END_Timer.Enabled = False Then
                FOCUS_ON_AT_THE_END_c = Nothing
            End If

            FOCUS_ON_AT_THE_END_MsgNum = MsgNum
            FOCUS_ON_AT_THE_END_MsgParams = MsgParams

            With FOCUS_ON_AT_THE_END_Timer
                .Interval = 20
                .Enabled = True
            End With
        End If
    End Sub

    Public Sub FOCUS_ON_AT_THE_END(ByVal c As Control, Optional ByVal MsgNum As Long = 0, Optional ByVal MsgParams As String = "", Optional ByVal OnlyWhenNotSet As Boolean = False)
        If Not Disposed Then
            If FOCUS_ON_AT_THE_END_Timer.Enabled = False Or Not OnlyWhenNotSet Then
                FOCUS_ON_AT_THE_END_c = c
                FOCUS_ON_AT_THE_END_MsgNum = MsgNum
                FOCUS_ON_AT_THE_END_MsgParams = MsgParams

                With FOCUS_ON_AT_THE_END_Timer
                    .Interval = 20
                    .Enabled = True
                End With
            End If
        End If
    End Sub
    Public Function FOCUS_ON_AT_THE_END_is_on() As Boolean
        Dim OUT As Boolean = False

        If Not (FOCUS_ON_AT_THE_END_Timer Is Nothing) Then
            OUT = FOCUS_ON_AT_THE_END_Timer.Enabled
        End If

        Return OUT
    End Function

    Protected Friend Sub FOCUS_ON_AT_THE_END_CLEAR()
        If Not (FOCUS_ON_AT_THE_END_Timer Is Nothing) Then
            FOCUS_ON_AT_THE_END_Timer.Enabled = False
        End If

        FOCUS_ON_AT_THE_END_c = Nothing
        FOCUS_ON_AT_THE_END_MsgNum = 0
        FOCUS_ON_AT_THE_END_MsgParams = ""
    End Sub

    Public Function DLG_NAVIGATION_FORWARD() As Boolean
        Dim OUT As Boolean = True

        If OUT = True Then
            If Not DialogMode Then
                OUT = False
                FPApp.DoErrorMsgBox("DLG_NAVIGATION_FORWARD", 0, "Ha a FORM nem Dialog modban van (az FP_Form definiciojaban lehet megadni), akkor ne add meg az OK button-t, hanem kezeld te magad a Click esemenyet.")
            End If
        End If

        If OUT = True Then
            If Has_UnboundFP() Then
                OUT = False
                FPApp.DoErrorMsgBox("DLG_NAVIGATION_FORWARD", 0, "Ha a FORM-on van olyan FP, ami nem kapcsolodik adatbazishoz (a letrehozasnal a ThisIsAnUnboundForm=1), akkor ne add meg az OK button-t, hanem kezeld te magad a Click esemenyet.")
            End If
        End If

        If OUT = True Then
            If Not (Dlg_Btn_OK Is Nothing) Then
                FOCUS_ON_IMMEDIATELY(Dlg_Btn_OK)
                OUT = Dlg_Btn_OK.Focused
            End If
        End If

        'If OUT = True Then
        '    OUT = CHK_FIELDS_ALL()
        'End If

        If OUT = True Then
            OUT = SAVE_ALL()
        End If

        If OUT = True Then
            gl_Doit = True
            Frm.DialogResult = Windows.Forms.DialogResult.OK
            Frm.Close()
        End If

        DLG_NAVIGATION_FORWARD = OUT
    End Function
    Public Sub DLG_NAVIGATION_EXIT()
        If Not (Frm Is Nothing) Then
            If DialogMode Then
                gl_Doit = False
                Frm.DialogResult = Windows.Forms.DialogResult.Cancel
                Frm.Close()
            End If
        End If
    End Sub
    Public Function DLG_NAVIGATION_ON_KEYDOWN(ByVal c As Control, ByRef e As System.Windows.Forms.KeyEventArgs) As Boolean
        Dim OUT As Boolean = False
        Dim AltPressed = My.Computer.Keyboard.AltKeyDown

        Select Case e.KeyCode
            Case Keys.Enter
                If Not AltPressed Then
                    If Not (Dlg_Btn_OK Is Nothing) Then
                        e.Handled = DLG_NAVIGATION_FORWARD()
                        OUT = e.Handled
                    End If
                End If

            Case Keys.Escape
                OUT = True
                e.Handled = True
                DLG_NAVIGATION_EXIT()
        End Select

        DLG_NAVIGATION_ON_KEYDOWN = OUT
    End Function

    Public Function CONTROLS_GET_NEXTCONTROL(ByVal MyControl As Control) As Control
        Dim AktNextControl As Control

        AktNextControl = Frm.GetNextControl(MyControl, True)
        If Not (AktNextControl Is Nothing) Then
            While AktNextControl.Visible = False Or AktNextControl.TabStop = False Or (TypeOf (AktNextControl) Is Label) Or Mid(AktNextControl.Name, 1, 1) = "#" Or (TypeOf (AktNextControl) Is DataGridView) Or (TypeOf (AktNextControl) Is DataGrid) Or (TypeOf (AktNextControl) Is TabControl) Or (TypeOf (AktNextControl) Is TabPage) Or (TypeOf (AktNextControl) Is Panel) Or (TypeOf (AktNextControl) Is SplitContainer)
                AktNextControl = Frm.GetNextControl(AktNextControl, True)
                If AktNextControl Is Nothing Then
                    Exit While
                End If
            End While
        End If

        CONTROLS_GET_NEXTCONTROL = AktNextControl
    End Function

    Public Sub VISIBLE_ALL(ByVal MyVisible As Boolean)
        Dim AktKey As String

        For Each AktKey In PICTUREBOXES.Keys
            With PICTUREBOXES(AktKey)
                If Not .c Is Nothing Then
                    '.P_Visible = MyVisible
                    .c.Visible = MyVisible
                End If
            End With
        Next
    End Sub

    Public Function CHK_FIELDS_ALL() As Boolean
        Dim OUT As Boolean = ActiveControl_VALIDATE()
        'Dim OUT As Boolean = True

        If OUT Then
            For Each Current_FP_ID In FPs.Keys
                If Not FPs(Current_FP_ID).FORM_CHK_FIELDS Then
                    OUT = False
                    Exit For
                End If
            Next
        End If

        CHK_FIELDS_ALL = OUT
    End Function
    Public Sub DORESYNC_ALL()
        For Each Current_FP_ID In FPs.Keys
            FPs(Current_FP_ID).FORM_DORESYNC()
        Next
    End Sub

    Public Function SAVE_ALL() As Boolean
        Dim OUT As Boolean = True
        Dim Handled As Boolean = False
        Dim Cancel As Integer = 0

        OUT = ActiveControl_VALIDATE()

        If OUT = True Then
            Cancel = 0
            RaiseEvent FORM_SAVE_ALL(Me, Handled, Cancel)

            If Cancel <> 0 Then
                OUT = False
            Else
                If Not Handled Then
                    For Each Current_FP_ID In FPs.Keys
                        If Not FPs(Current_FP_ID).FORM_RECORDS_SAVE_CURRENT Then
                            OUT = False
                            Exit For
                        End If
                    Next
                End If
            End If
        End If

        SAVE_ALL = OUT
    End Function
    Public Sub UNDO_ALL()
        Dim Handled As Boolean = False

        RaiseEvent FORM_UNDO_ALL(Me, Handled)

        If Not Handled Then
            For Each Current_FP_ID In FPs.Keys
                FPs(Current_FP_ID).UNDO()
            Next
        End If
    End Sub
    Public ReadOnly Property P_FORM_Dirty() As Boolean
        Get
            Dim OUT As Boolean = False

            For Each Current_FP_ID In FPs.Keys
                If FPs(Current_FP_ID).P_FORM_Dirty = True Then
                    OUT = True
                    Exit For
                End If
            Next

            Return OUT
        End Get
    End Property
    Public Sub HELP_SHOW(ByVal MyControlObject As Object, Optional ByVal SubControl_Name As String = "")
        If Not (HELP_Frm Is Nothing) Then
            HELP_Frm.HELP_SHOW(MyControlObject, SubControl_Name)
        End If
    End Sub
    Public Sub HELP_HIDE()
        If Not (HELP_Frm Is Nothing) Then
            HELP_Frm.HELP_HIDE()
        End If
    End Sub

#Region "ARRANGE"
    Private Function ARRANGE_getBaseMesaure(ByVal Base_OrigMesaure As Integer, ByVal Percentage As Integer) As Integer
        Dim OUT As Integer = 0

        If Percentage = 0 Then
            OUT = Base_OrigMesaure
        Else
            OUT = Int(((1 / 100 * Percentage) * Base_OrigMesaure + 0.5))
        End If

        ARRANGE_getBaseMesaure = OUT
    End Function

    Public Sub ARRANGE_FROM_TO(c As Control, c1 As Control, c2 As Control, SpaceInPixel As Integer, Percentage As Integer)
        Dim FPc As FP_Control = Nothing
        Dim FPc1 As FP_Control = Nothing
        Dim FPc2 As FP_Control = Nothing

        If CONTROLS_GET_FPc_FROM_CONTROL(c, FPc) Then
            If CONTROLS_GET_FPc_FROM_CONTROL(c1, FPc1) Then
                If CONTROLS_GET_FPc_FROM_CONTROL(c2, FPc2) Then
                    If Not (FPc.c_Label Is Nothing) And Not (FPc1.c_Label Is Nothing) Then
                        SIZE_WIDTH_TO(FPc1.c_Label, FPc.c_Label)
                        ARRANGE_ON_BOTTOM_LEFT(FPc1.c_Label, FPc.c_Label, SpaceInPixel)
                    End If
                    If FPc2.c_Label Is Nothing Then
                        SIZE_WIDTH_TO(FPc1.c, FPc.c, , , 50)
                        ARRANGE_ON_BOTTOM_LEFT(FPc1.c, FPc.c, SpaceInPixel)
                        SIZE_WIDTH_BETWEEN(FPc2.c, FPc1.c, FPc.c)
                        SIZE_HEIGHT_TO(FPc2.c, FPc1.c)
                        ARRANGE_ON_BOTTOM_RIGHT(FPc2.c, FPc.c, SpaceInPixel)
                    Else
                        Dim Perc As Integer = Percentage
                        If Perc = 0 Then
                            Perc = 47
                        End If
                        SIZE_WIDTH_TO(FPc1.c, FPc.c, , , Perc)
                        ARRANGE_ON_BOTTOM_LEFT(FPc1.c, FPc.c, SpaceInPixel)
                        SIZE_SAME(FPc2.c, FPc1.c)
                        ARRANGE_ON_BOTTOM_RIGHT(FPc2.c, FPc.c, SpaceInPixel)
                        ARRANGE_ON_RIGHT_TOP(FPc2.c_Label, FPc1.c, 0)
                        SIZE_WIDTH_BETWEEN(FPc2.c_Label, FPc1.c, FPc2.c)
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub ARRANGE_ON_LEFT(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        If c.Parent.Equals(BaseControl.Parent) Then
            c.Left = BaseControl.Left - SpaceInPixel - c.Width
        End If
    End Sub
    Public Sub ARRANGE_ON_LEFT_TOP(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        If c.Parent.Equals(BaseControl.Parent) Then
            c.Left = BaseControl.Left - SpaceInPixel - c.Width
            c.Top = BaseControl.Top
        End If
    End Sub
    Public Sub ARRANGE_ON_LEFT_BOTTOM(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        If c.Parent.Equals(BaseControl.Parent) Then
            c.Left = BaseControl.Left - SpaceInPixel - c.Width
            c.Top = BaseControl.Bottom - c.Height
        End If
    End Sub
    Public Sub ARRANGE_ON_RIGHT(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        If c.Parent.Equals(BaseControl.Parent) Then
            c.Left = BaseControl.Left + BaseControl.Width + SpaceInPixel
        End If
    End Sub
    Public Sub ARRANGE_ON_RIGHT_TOP(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        If c.Parent.Equals(BaseControl.Parent) Then
            c.Left = BaseControl.Left + BaseControl.Width + SpaceInPixel
            c.Top = BaseControl.Top
        End If
    End Sub
    Public Sub ARRANGE_ON_RIGHT_BOTTOM(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        If c.Parent.Equals(BaseControl.Parent) Then
            c.Left = BaseControl.Left + BaseControl.Width + SpaceInPixel
            c.Top = BaseControl.Bottom - c.Height
        End If
    End Sub
    Public Sub ARRANGE_ON_CENTER_X(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        Dim c_Parent As Control = ARRANGE_getParent(c)

        If c_Parent.Equals(BaseControl) Then
            c.Left = BaseControl.Width / 2 - c.Width / 2 + SpaceInPixel
        ElseIf c.Parent.Equals(BaseControl.Parent) Then
            c.Left = BaseControl.Left + BaseControl.Width / 2 - c.Width / 2 + SpaceInPixel
        Else
            FPApp.DoErrorMsgBox("FP_Form", 0, String.Format("Parent of '{0}' <> parent of {1}'", c.Parent.Name, BaseControl.Name))
        End If
    End Sub
    Public Sub ARRANGE_ON_CENTER_Y(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        Dim c_Parent As Control = ARRANGE_getParent(c)

        If c_Parent.Equals(BaseControl) Then
            c.Top = BaseControl.Height / 2 - c.Height / 2 + SpaceInPixel
        ElseIf c.Parent.Equals(BaseControl.Parent) Then
            c.Top = BaseControl.Top + BaseControl.Height / 2 - c.Height / 2 + SpaceInPixel
        Else
            FPApp.DoErrorMsgBox("FP_Form", 0, String.Format("Parent of '{0}' <> parent of {1}'", c.Parent.Name, BaseControl.Name))
        End If
    End Sub
    Public Sub ARRANGE_ON_TOP(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        If c.Parent.Equals(BaseControl.Parent) Then
            c.Top = BaseControl.Top - SpaceInPixel - c.Height
        End If
    End Sub
    Public Sub ARRANGE_ON_TOP_LEFT(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        If c.Parent.Equals(BaseControl.Parent) Then
            c.Top = BaseControl.Top - SpaceInPixel - c.Height
            c.Left = BaseControl.Left
        End If
    End Sub
    Public Sub ARRANGE_ON_TOP_RIGHT(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        If c.Parent.Equals(BaseControl.Parent) Then
            c.Top = BaseControl.Top - SpaceInPixel - c.Height
            c.Left = BaseControl.Right - c.Width
        End If
    End Sub
    Public Sub ARRANGE_ON_BOTTOM(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        If c.Parent.Equals(BaseControl.Parent) Then
            c.Top = BaseControl.Top + BaseControl.Height + SpaceInPixel
        End If
    End Sub
    Public Sub ARRANGE_ON_BOTTOM_LEFT(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        If c.Parent.Equals(BaseControl.Parent) Then
            c.Top = BaseControl.Top + BaseControl.Height + SpaceInPixel
            c.Left = BaseControl.Left
        End If
    End Sub
    Public Sub ARRANGE_ON_BOTTOM_RIGHT(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 1)
        If c.Parent.Equals(BaseControl.Parent) Then
            c.Top = BaseControl.Top + BaseControl.Height + SpaceInPixel
            c.Left = BaseControl.Right - c.Width
        End If
    End Sub
    Public Sub ARRANGE_TOPS(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 0, Optional ByVal Percentage As Integer = 0)
        Dim c_Parent As Control = ARRANGE_getParent(c)

        If c_Parent.Equals(BaseControl) Then
            c.Top = BaseControl.ClientRectangle.Top + BaseControl.ClientRectangle.Height - ARRANGE_getBaseMesaure(BaseControl.ClientRectangle.Height, Percentage) + SpaceInPixel
        ElseIf c.Parent.Equals(BaseControl.Parent) Then
            c.Top = BaseControl.Top + BaseControl.Height - ARRANGE_getBaseMesaure(BaseControl.Height, Percentage) + SpaceInPixel
        Else
            FPApp.DoErrorMsgBox("FP_Form", 0, String.Format("Parent of '{0}' <> parent of {1}'", c.Parent.Name, BaseControl.Name))
        End If
    End Sub
    Public Sub ARRANGE_BOTTOMS(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 0, Optional ByVal Percentage As Integer = 0)
        Dim c_Parent As Control = ARRANGE_getParent(c)

        If c_Parent.Equals(BaseControl) Then
            c.Top = ARRANGE_getBaseMesaure(BaseControl.ClientRectangle.Height, Percentage) - c.Height - SpaceInPixel
        ElseIf c.Parent.Equals(BaseControl.Parent) Then
            c.Top = BaseControl.Top + ARRANGE_getBaseMesaure(BaseControl.Height, Percentage) - c.Height - SpaceInPixel
        Else
            FPApp.DoErrorMsgBox("FP_Form", 0, String.Format("Parent of '{0}' <> parent of {1}'", c.Parent.Name, BaseControl.Name))
        End If
    End Sub
    Public Sub ARRANGE_LEFTS(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 0, Optional ByVal Percentage As Integer = 0)
        Dim c_Parent As Control = ARRANGE_getParent(c)

        If c_Parent.Equals(BaseControl) Then
            c.Left = BaseControl.ClientRectangle.Left + BaseControl.ClientRectangle.Width - ARRANGE_getBaseMesaure(BaseControl.ClientRectangle.Width, Percentage) + SpaceInPixel
        ElseIf c.Parent.Equals(BaseControl.Parent) Then
            c.Left = BaseControl.Left + BaseControl.Width - ARRANGE_getBaseMesaure(BaseControl.Width, Percentage) + SpaceInPixel
        Else
            FPApp.DoErrorMsgBox("FP_Form", 0, String.Format("Parent of '{0}' <> parent of {1}'", c.Parent.Name, BaseControl.Name))
        End If
    End Sub
    Public Sub ARRANGE_RIGHTS(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixel As Integer = 0, Optional ByVal Percentage As Integer = 0)
        Dim c_Parent As Control = ARRANGE_getParent(c)

        If c_Parent.Equals(BaseControl) Then
            c.Left = ARRANGE_getBaseMesaure(BaseControl.ClientRectangle.Width, Percentage) - c.Width - SpaceInPixel
        ElseIf c.Parent.Equals(BaseControl.Parent) Then
            c.Left = BaseControl.Left + ARRANGE_getBaseMesaure(BaseControl.Width, Percentage) - c.Width - SpaceInPixel
        Else
            FPApp.DoErrorMsgBox("FP_Form", 0, String.Format("Parent of '{0}' <> parent of {1}'", c.Parent.Name, BaseControl.Name))
        End If
    End Sub

    Public Sub ARRANGE_AS_NEXT_ROW(c As Control, c1 As Control, Optional c2 As Control = Nothing, Optional Percentage As Integer = 0, Optional SpaceInPixel As Integer = 0)
        If c2 Is Nothing Then
            SIZE_WIDTH_TO(c, c1, 0, 0, Percentage)
        Else
            SIZE_WIDTH_TO(c, c1, c2, 0, 0, Percentage)
        End If

        'A magassagot nem allitja az "AS_NEXT_ROW_TO" mert a ComboBox magassaga 150%-os betumeretnel 30, mig a textbox-e es a Label-e 29.
        'Vagyis ha egy DoFilter-ben Combobox utan textbox jon, akkor annak magassaga is 30 lesz, mig a label-e 29 es elcsusznak egymastol a sorok
        'SIZE_HEIGHT_TO(c, c1)
        ARRANGE_ON_BOTTOM_LEFT(c, c1, SpaceInPixel)
    End Sub

    Public Sub SIZE_SAME(ByVal c As Control, ByVal BaseControl As Control)
        Dim c_Parent As Control = ARRANGE_getParent(c)

        If c_Parent.Equals(BaseControl) Then
            c.Width = BaseControl.ClientRectangle.Width
            SIZE_HEIGHT_SET(c, BaseControl.ClientRectangle.Height)
        Else
            c.Width = BaseControl.Width
            SIZE_HEIGHT_SET(c, BaseControl.Height)
        End If
        SIZE_FINISH_RESIZE(c)
    End Sub

    Private Function LOCATION_PfdKey(Optional ByVal SubPrefix As String = "") As String
        Dim OUT As String = String.Format("LASTLOC_{0}", ServerObject_Prefix).ToUpper

        If SubPrefix > "" Then
            OUT += "_" + SubPrefix.ToUpper
        End If

        Return OUT
    End Function

    Private Function LOCATION_SAVE_NO() As Boolean
        Dim OUT As Boolean = (Not Location_Save_On_Close)

        If Frm Is Nothing Then
            OUT = True
        End If

        If OUT = False Then
            If Frm_Minimized Then
                OUT = True
            End If
        End If

        If OUT = False Then
            If Frm.WindowState = FormWindowState.Minimized Then
                OUT = True
            End If
        End If
        If OUT = False Then
            If Frm.WindowState = FormWindowState.Normal Then
                Dim Scr As Screen = Screen.FromPoint(New Point(Frm.Left + Frm.Width / 2, Frm.Top + Frm.Height / 2))

                If Scr Is Nothing Then
                    OUT = True
                Else
                    Dim WA As New FP_L_Rect(Scr.WorkingArea)

                    If Not WA.Contains(New Rectangle(Frm.Location, Frm.Size)) Then
                        OUT = True
                    End If
                End If
            End If
        End If

        If OUT = False Then
            Dim wName As String = Frm.Name.ToUpper

            'If Left(wName, 3) = "FP_" Then
            '    OUT = True
            'End If
        End If

        Return OUT
    End Function

    Public Sub LOCATION_SAVE(Optional ByVal SubPrefix As String = "")
        Dim Location_STR As String = ""

        If LOCATION_SAVE_NO() = False Then
            If Frm.WindowState = FormWindowState.Maximized Then
                Location_STR = String.Format("{0}|{1}|{2}|{3}|{4}|{5}", Frm.RestoreBounds.Left, Frm.RestoreBounds.Top, CInt(Frm.WindowState), Frm.RestoreBounds.Width, Frm.RestoreBounds.Height, P_Layout_TextBox_NormalHeight)
            Else
                Location_STR = String.Format("{0}|{1}|{2}|{3}|{4}|{5}", Frm.Left, Frm.Top, CInt(Frm.WindowState), Frm.Width, Frm.Height, P_Layout_TextBox_NormalHeight)
            End If

            FPApp.PFDinsertOrUpdate(LOCATION_PfdKey(SubPrefix), Location_STR)
        End If
    End Sub

    Public Sub LOCATION_MOVE_TO_SAVED_POS(Optional ByVal SubPrefix As String = "")
        Dim Last_Loc_STR As String = ""
        Dim Last_Loc() As String = Nothing

        FPApp.PFDlesen(LOCATION_PfdKey(SubPrefix), Last_Loc_STR)
        Last_Loc = Last_Loc_STR.Split("|")
        If UBound(Last_Loc) <> 5 Then
            Dim MyScreen As Screen = Screen.FromPoint(Frm.Location)

            If MyScreen Is Nothing Then
                Frm.Location = New Point(0, 0)
            End If
        Else
            Dim Last_Loc_p As New Point(Val(Last_Loc(0)), Val(Last_Loc(1)))
            Dim Last_FormState As Integer = Val(Last_Loc(2))
            Dim Last_Size As New Size(Val(Last_Loc(3)), Val(Last_Loc(4)))
            Dim Last_Screen As Screen = Screen.FromPoint(New Point(Last_Loc_p.X + 40, Last_Loc_p.Y + 40))
            Dim Last_Layout_TextBox_NormalHeight As Integer = Val(Last_Loc(5))

            Dim DoIt As Boolean = True

            If (Last_Screen Is Nothing) Then
                DoIt = False
            ElseIf Last_Layout_TextBox_NormalHeight <> P_Layout_TextBox_NormalHeight Then
                DoIt = False
            Else
                Dim CheckPoint As New Point(Last_Loc_p.X + Last_Size.Width / 2, Last_Loc_p.Y)

                If CheckPoint.X < Last_Screen.WorkingArea.Left Or
                   CheckPoint.X > Last_Screen.WorkingArea.Left + Last_Screen.WorkingArea.Width Or
                   CheckPoint.Y < Last_Screen.WorkingArea.Top Or
                   CheckPoint.Y > Last_Screen.WorkingArea.Top + Last_Screen.WorkingArea.Height Then
                    DoIt = False
                End If
            End If

            If DoIt = False Then
                Frm.Location = New Point(0, 0)
                Frm.WindowState = FormWindowState.Normal
            Else
                Frm.Location = Last_Loc_p
                Frm.WindowState = Last_FormState
                If Frm.WindowState = FormWindowState.Normal Then
                    Frm.Location = Last_Loc_p
                    Frm.Size = Last_Size
                End If
            End If
        End If

        If LOCATION_SAVE_NO() = True Then
            If Frm.WindowState = FormWindowState.Normal Then
                Dim Scr As Screen = Screen.FromPoint(New Point(Frm.Left + Frm.Width / 2, Frm.Top + Frm.Height / 2))

                If Scr Is Nothing Then
                    Frm.Location = New Point(0, 0)
                Else
                    Dim WA As New FP_L_Rect(Scr.WorkingArea)

                    If Not WA.Contains(New Rectangle(Frm.Location, Frm.Size)) Then
                        Frm.Location = New Point(0, 0)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Checkboxes_Location_SAVE(c As Control, ByRef OUT_ARRAY_of_checkboxes() As Control, ByRef OUT_ARRAY_of_Location() As Point)
        ReDim OUT_ARRAY_of_checkboxes(0)
        ReDim OUT_ARRAY_of_Location(0)
        Dim CountOfElements As Integer = 0

        For Each wc As Control In c.Controls
            If TypeOf (wc) Is CheckBox Then
                OUT_ARRAY_of_checkboxes(CountOfElements) = wc
                OUT_ARRAY_of_Location(CountOfElements) = wc.Location

                CountOfElements += 1

                ReDim Preserve OUT_ARRAY_of_checkboxes(CountOfElements)
                ReDim Preserve OUT_ARRAY_of_Location(CountOfElements)
            End If
        Next
    End Sub

    Private Sub Checkboxes_Location_RESET(ByVal ARRAY_of_checkboxes() As Control, ByVal ARRAY_of_Location() As Point)
        For i As Integer = 0 To UBound(ARRAY_of_checkboxes) - 1
            If Not ARRAY_of_checkboxes(i) Is Nothing Then
                ARRAY_of_checkboxes(i).Location = ARRAY_of_Location(i)
            End If
        Next
    End Sub

    Public Sub SIZE_HEIGHT_TO_MAX(ByVal c As Control, Optional ByVal SpaceInPixels As Integer = 0)
        Dim MaxHeight As Integer = 0
        Dim ChkBoxes(0) As Control
        Dim ChkBoxes_Locs(0) As Point

        Checkboxes_Location_SAVE(c, ChkBoxes, ChkBoxes_Locs)

        If TypeOf (c) Is TabControl Then
            Dim DIC_of_CONTROLS As Dictionary(Of String, Control) = Nothing

            CONTROLS_WRITE_ALL_CONTROLS_TO_DIC(c, DIC_of_CONTROLS)

            For Each AktKey As String In DIC_of_CONTROLS.Keys
                Dim wc As Control = DIC_of_CONTROLS(AktKey)

                If Mid(wc.Name, 1, 1) <> "#" And wc.Name > "" Then
                    Dim CurrHeight As Integer = wc.Top + wc.Height + SpaceInPixels

                    If CurrHeight > MaxHeight Then
                        MaxHeight = CurrHeight
                    End If
                End If
            Next
        Else
            For Each wc As Control In c.Controls
                If Mid(wc.Name, 1, 1) <> "#" And wc.Name > "" Then
                    Dim CurrHeight As Integer = wc.Top + wc.Height + SpaceInPixels

                    If CurrHeight > MaxHeight Then
                        MaxHeight = CurrHeight
                    End If
                End If
            Next
        End If

        If TypeOf (c) Is Form Then
            Dim CurrentScreen As Screen = Nothing

            CurrentScreen = Screen.FromPoint(New Point(c.Left, c.Top))

            MaxHeight += Form_Title_Height(c) + Form_Border_Width(c)

            If MaxHeight > CurrentScreen.WorkingArea.Height Then
                MaxHeight = CurrentScreen.WorkingArea.Height
            End If

            If c.Height <> MaxHeight Then
                c.Height = MaxHeight

                If CType(c, Form).StartPosition = FormStartPosition.CenterScreen Then
                    Form_Move_To_CenterScreen(c)
                End If
            End If
        Else
            'If MaxHeight > c.Parent.Height Then
            '    MaxHeight = c.Parent.Height
            'End If

            Dim DoNormal As Boolean = True

            If TypeOf (c) Is Panel Then
                Dim c_Parent As Control = c.Parent

                If Not (c_Parent Is Nothing) Then
                    If TypeOf (c_Parent) Is SplitContainer Then
                        DoNormal = False

                        With (CType(c_Parent, System.Windows.Forms.SplitContainer))
                            Dim BorderWidth As Integer = 4

                            Select Case .BorderStyle
                                Case BorderStyle.Fixed3D : BorderWidth = 4
                                Case BorderStyle.FixedSingle : BorderWidth = 4
                                Case BorderStyle.FixedSingle : BorderWidth = 0
                                Case Else : BorderWidth = 4
                            End Select

                            Dim MaxSplitterDistance As Integer = .Height - .SplitterWidth
                            Dim NewSplitterDistance As Integer = Math.Min(MaxSplitterDistance, MaxHeight + BorderWidth)

                            If c.Equals(.Panel1) Then
                                .SplitterDistance = NewSplitterDistance
                            ElseIf c.Equals(.Panel2) Then
                                .SplitterDistance = MaxSplitterDistance - NewSplitterDistance
                            Else
                                'Nothing to do
                            End If
                        End With
                    End If
                End If
            End If

            If DoNormal = True Then
                c.Height = MaxHeight
            End If
        End If

        Checkboxes_Location_RESET(ChkBoxes, ChkBoxes_Locs)
    End Sub

    Public Sub SIZE_WIDTH_TO_MAX(ByVal c As Control, Optional ByVal SpaceInPixels As Integer = 0)
        Dim MaxWidth As Integer = c.Width
        Dim ChkBoxes(0) As Control
        Dim ChkBoxes_Locs(0) As Point

        Checkboxes_Location_SAVE(c, ChkBoxes, ChkBoxes_Locs)

        For Each wc As Control In c.Controls
            If Mid(wc.Name, 1, 1) <> "#" Then
                Dim CurrWidth = wc.Left + wc.Width + SpaceInPixels

                If CurrWidth > MaxWidth Then
                    MaxWidth = CurrWidth
                End If
            End If
        Next

        If TypeOf (c) Is Form Then
            Dim CurrentScreen As Screen = Screen.FromPoint(c.PointToScreen(New Point(0, 0)))

            If MaxWidth > CurrentScreen.WorkingArea.Width Then
                MaxWidth = CurrentScreen.WorkingArea.Width
            End If

            If c.Width <> MaxWidth Then
                c.Width = MaxWidth

                If CType(c, Form).StartPosition = FormStartPosition.CenterScreen Then
                    Form_Move_To_CenterScreen(c)
                End If
            End If
        Else
            If c.Parent.Width <> MaxWidth Then
                MaxWidth = c.Parent.Width
                c.Width = MaxWidth
            End If
        End If

        Checkboxes_Location_RESET(ChkBoxes, ChkBoxes_Locs)
    End Sub

    Public Function Text_getDialogText(DialNum As Integer, MsgParams As String) As String
        Return FPApp.Text_getDialogText(ModuleIdentifier, DialNum, MsgParams)
    End Function

    Public Sub SIZE_WIDTH_TO(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixels_Left As Integer = 0, Optional ByVal SpaceInPixels_Right As Integer = 0, Optional ByVal Percentage As Integer = 0)
        Dim c_Parent As Control = ARRANGE_getParent(c)

        If c_Parent.Equals(BaseControl) Then
            c.Width = ARRANGE_getBaseMesaure(BaseControl.ClientRectangle.Width, Percentage) - (SpaceInPixels_Left + SpaceInPixels_Right)
        Else
            c.Width = ARRANGE_getBaseMesaure(BaseControl.Width, Percentage) - (SpaceInPixels_Left + SpaceInPixels_Right)
        End If
        SIZE_FINISH_RESIZE(c)
    End Sub
    Public Sub SIZE_WIDTH_TO(ByVal c As Control, ByVal BaseControl1 As Control, BaseControl2 As Control, Optional ByVal SpaceInPixels_Left As Integer = 0, Optional ByVal SpaceInPixels_Right As Integer = 0, Optional ByVal Percentage As Integer = 0)
        c.Width = ARRANGE_getBaseMesaure(BaseControl2.Right - BaseControl1.Left, Percentage) - (SpaceInPixels_Left + SpaceInPixels_Right)
        SIZE_FINISH_RESIZE(c)
    End Sub

    Public Sub SIZE_HEIGHT_TO(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixels_Top As Integer = 0, Optional ByVal SpaceInPixels_Bottom As Integer = 0, Optional ByVal Percentage As Integer = 0)
        Dim c_Parent As Control = ARRANGE_getParent(c)
        Dim NewHeight As Integer = 0

        If c_Parent.Equals(BaseControl) Then
            NewHeight = ARRANGE_getBaseMesaure(BaseControl.ClientRectangle.Height, Percentage) - (SpaceInPixels_Top + SpaceInPixels_Bottom)
        Else
            NewHeight = ARRANGE_getBaseMesaure(BaseControl.Height, Percentage) - (SpaceInPixels_Top + SpaceInPixels_Bottom)
        End If

        SIZE_HEIGHT_SET(c, NewHeight)
        SIZE_FINISH_RESIZE(c)
    End Sub

    Public Sub SIZE_HEIGHT_TO(ByVal c As Control, ByVal BaseControl1 As Control, BaseControl2 As Control, Optional ByVal SpaceInPixels_Top As Integer = 0, Optional ByVal SpaceInPixels_Bottom As Integer = 0, Optional ByVal Percentage As Integer = 0)
        Dim NewHeight As Integer = 0

        NewHeight = ARRANGE_getBaseMesaure(BaseControl2.Bottom - BaseControl1.Top, Percentage) - (SpaceInPixels_Top + SpaceInPixels_Bottom)

        SIZE_HEIGHT_SET(c, NewHeight)
        SIZE_FINISH_RESIZE(c)
    End Sub
    Public Sub SIZE_WIDTH_BETWEEN(ByVal c As Control, ByVal c_Left As Control, ByVal c_Right As Control, Optional ByVal SpaceInPixel_Left As Integer = 0, Optional ByVal SpaceInPixel_Right As Integer = 0, Optional ByVal Percentage As Integer = 0)
        Dim c_Left_Parent As Control = ARRANGE_getParent(c_Left)
        Dim c_Right_Parent As Control = ARRANGE_getParent(c_Right)

        If c_Left_Parent.Equals(c_Right) Then
            c.Width = ARRANGE_getBaseMesaure(c_Right.ClientRectangle.Width - (c_Left.Left + c_Left.Width), Percentage) - SpaceInPixel_Right - SpaceInPixel_Left
        ElseIf c_Right_Parent.Equals(c_Left) Then
            c.Width = ARRANGE_getBaseMesaure(c_Right.Left, Percentage) - SpaceInPixel_Right - SpaceInPixel_Left
        Else
            'c.Width = ARRANGE_getBaseMesaure(c_Right.Left - (c_Left.Left + c_Left.Width), Percentage) - SpaceInPixel_Right - SpaceInPixel_Left
            Dim LeftKoo As Integer
            Dim RightKoo As Integer

            If c_Left.Right < c_Right.Left Then
                'Normal helyzet
                LeftKoo = c_Left.Right
                RightKoo = c_Right.Left
            Else
                If c_Left.Right < c_Right.Right Then
                    LeftKoo = c_Left.Right
                    RightKoo = c_Right.Right
                Else
                    LeftKoo = c_Left.Left
                    RightKoo = c_Right.Left
                End If
            End If

            c.Width = ARRANGE_getBaseMesaure(RightKoo - LeftKoo, Percentage) - SpaceInPixel_Right - SpaceInPixel_Left
        End If
        SIZE_FINISH_RESIZE(c)
    End Sub

    Public Sub SIZE_WIDTH_SET(ByVal c As Control, ByVal Width As Integer)
        If Not (c Is Nothing) Then
            If TypeOf (c) Is ComboBox Then
                With CType(c, ComboBox)
                    Try
                        Dim Sel_Start As Integer = .SelectionStart
                        Dim Sel_Length As Integer = .SelectionLength

                        .Width = Width

                        .SelectionStart = Sel_Start
                        .SelectionLength = Sel_Length

                    Catch ex As Exception
                        'Nothing to do (elofordult, hogy a .SelectionLength negativ szam volt!!!???
                    End Try
                End With
            Else
                c.Width = Width
            End If
        End If
    End Sub

    Public Sub SIZE_HEIGHT_SET(ByVal c As Control, ByVal Height As Integer)
        If Not (c Is Nothing) Then
            If TypeOf (c) Is TextBox Then
                With (CType(c, TextBox))
                    If Height > P_Layout_TextBox_NormalHeight Then
                        .Multiline = True
                    End If

                    If .Multiline Then
                        .Height = Height
                    End If
                End With
            ElseIf TypeOf (c) Is ComboBox Then
                With CType(c, ComboBox)
                    Dim Sel_Start As Integer = .SelectionStart
                    Dim Sel_Length As Integer = .SelectionLength

                    .Height = Height

                    .SelectionStart = Sel_Start
                    .SelectionLength = Sel_Length
                End With
            Else
                c.Height = Height
            End If
        End If
    End Sub

    Public Sub SIZE_HEIGHT_TO_WIDTH(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixels_Top As Integer = 0, Optional ByVal SpaceInPixels_Bottom As Integer = 0, Optional ByVal Percentage As Integer = 0)
        Dim c_Parent As Control = ARRANGE_getParent(c)
        Dim NewHeight As Integer = 0

        If c_Parent.Equals(BaseControl) Then
            NewHeight = ARRANGE_getBaseMesaure(BaseControl.ClientRectangle.Width, Percentage) - (SpaceInPixels_Top + SpaceInPixels_Bottom)
        Else
            NewHeight = ARRANGE_getBaseMesaure(BaseControl.Width, Percentage) - (SpaceInPixels_Top + SpaceInPixels_Bottom)
        End If

        SIZE_HEIGHT_SET(c, NewHeight)
        SIZE_FINISH_RESIZE(c)
    End Sub

    Private Sub SIZE_FINISH_RESIZE(ByVal c As Control)
        If TypeOf (c) Is ComboBox Then
            If Not c.Focused Then
                With CType(c, ComboBox)
                    .SelectionStart = 0
                    .SelectionLength = 0
                End With
            End If
        ElseIf TypeOf (c) Is PictureBox Then
            Dim FPp As FP_PictureBox = PICTUREBOXES_GET_FPp_FROM_CONTROL(c)

            If Not (FPp Is Nothing) Then
                FPp.SHOW()
            End If
        ElseIf TypeOf (c) Is MSDN.Html.Editor.HtmlEditorControl Then
            'A control csak akkor allitja be helyesen az editor resz mereteit, ha a control meretet megvaltatjuk
            Dim Orig_Width As Integer = c.Width
            Dim Orig_Height As Integer = c.Height

            CType(c, MSDN.Html.Editor.HtmlEditorControl).SetBounds(c.Left, c.Top, c.Width + 1, c.Height + 1)
            CType(c, MSDN.Html.Editor.HtmlEditorControl).SetBounds(c.Left, c.Top, c.Width, c.Height)
        End If
    End Sub
    Public Sub SIZE_WIDTH_TO_HEIGHT(ByVal c As Control, ByVal BaseControl As Control, Optional ByVal SpaceInPixels_Left As Integer = 0, Optional ByVal SpaceInPixels_Right As Integer = 0, Optional ByVal Percentage As Integer = 0)
        Dim c_Parent As Control = ARRANGE_getParent(c)

        If c_Parent.Equals(BaseControl) Then
            c.Width = ARRANGE_getBaseMesaure(BaseControl.ClientRectangle.Height, Percentage) - (SpaceInPixels_Left + SpaceInPixels_Right)
        Else
            c.Width = ARRANGE_getBaseMesaure(BaseControl.Height, Percentage) - (SpaceInPixels_Left + SpaceInPixels_Right)
        End If

        SIZE_FINISH_RESIZE(c)
    End Sub

    Public Sub SIZE_HEIGHT_BETWEEN(ByVal c As Control, ByVal c_Top As Control, ByVal c_Bottom As Control, Optional ByVal SpaceInPixel_Top As Integer = 0, Optional ByVal SpaceInPixel_Bottom As Integer = 0, Optional ByVal Percentage As Integer = 0)
        Dim c_Top_Parent As Control = ARRANGE_getParent(c_Top)
        Dim c_Bottom_Parent As Control = ARRANGE_getParent(c_Bottom)
        Dim NewHeight As Integer = 0

        If c_Top_Parent.Equals(c_Bottom) Then
            NewHeight = ARRANGE_getBaseMesaure(c_Bottom.ClientRectangle.Height - (c_Top.Top + c_Top.Height), Percentage) - SpaceInPixel_Top - SpaceInPixel_Bottom
        ElseIf c_Bottom_Parent.Equals(c_Top) Then
            NewHeight = ARRANGE_getBaseMesaure(c_Bottom.Top, Percentage) - SpaceInPixel_Bottom - SpaceInPixel_Top
        Else
            'NewHeight = ARRANGE_getBaseMesaure(c_Bottom.Top - (c_Top.Top + c_Top.Height), Percentage) - SpaceInPixel_Bottom - SpaceInPixel_Top
            Dim TopKoo As Integer = c_Top.Top + c_Top.Height
            Dim BottomKoo As Integer = c_Bottom.Top

            'If BottomKoo < TopKoo Then
            '    BottomKoo = c_Bottom.Top + c_Bottom.Height
            'End If

            If c_Top.Bottom < c_Bottom.Top Then
                'Normal helyzet
                TopKoo = c_Top.Bottom
                BottomKoo = c_Bottom.Top
            Else
                If c_Top.Bottom < c_Bottom.Bottom Then
                    TopKoo = c_Top.Bottom
                    BottomKoo = c_Bottom.Bottom
                Else
                    TopKoo = c_Top.Top
                    BottomKoo = c_Bottom.Top
                End If
            End If

            NewHeight = ARRANGE_getBaseMesaure(BottomKoo - TopKoo, Percentage) - SpaceInPixel_Bottom - SpaceInPixel_Top
        End If

        SIZE_HEIGHT_SET(c, NewHeight)
    End Sub

    Private Function ARRANGE_CHK_CONTROL(ByVal ArrangedField_Name As String, ByVal ArrangedField As Control, ByVal ReferredControl_Name As String, ByVal ReferredControl As Control, ByVal ArrangeType As String) As Boolean
        Dim OUT As Boolean = False

        If Trim(ArrangedField_Name) = "" Then
            FPApp.DoErrorMsgBox("FP_Form.ARRANGE_CHK_CONTROL", 0, String.Format("Name of Arranged Field missing (Arrange Type: {0}).", ArrangeType))
        ElseIf ArrangedField Is Nothing Then
            FPApp.DoErrorMsgBox("FP_Form.ARRANGE_CHK_CONTROL", 0, String.Format("Arranged Field '{0}' not found.", ArrangedField_Name))
        ElseIf Trim(ReferredControl_Name) = "" Then
            FPApp.DoErrorMsgBox("FP_Form.ARRANGE_CHK_CONTROL", 0, String.Format("Not all parameter given for Arrange Type '{0}' by field '{1}'", ArrangeType, ArrangedField_Name))
        ElseIf ReferredControl Is Nothing Then
            FPApp.DoErrorMsgBox("FP_Form.ARRANGE_CHK_CONTROL", 0, String.Format("Field '{0}' not found. (Arranged Field: '{1}', Arrange Type: '{2}')", ReferredControl_Name, ArrangedField_Name, ArrangeType))
        Else
            OUT = True
        End If

        ARRANGE_CHK_CONTROL = OUT
    End Function

    Private Function ARRANGE_CHK_CONTROL(ByVal ArrangedField_Name As String, ByVal ArrangedField As Control, ByVal ArrangeType As String) As Boolean
        Dim OUT As Boolean = False

        If Trim(ArrangedField_Name) = "" Then
            FPApp.DoErrorMsgBox("FP_Form.ARRANGE_CHK_CONTROL", 0, String.Format("Name of Arranged Field missing (Arrange Type: {0}).", ArrangeType))
        ElseIf ArrangedField Is Nothing Then
            FPApp.DoErrorMsgBox("FP_Form.ARRANGE_CHK_CONTROL", 0, String.Format("Arranged Field '{0}' not found.", ArrangedField_Name))
        Else
            OUT = True
        End If

        ARRANGE_CHK_CONTROL = OUT
    End Function

    Private CONTROLS_ARRANGE_Runs As Boolean = False

    Private Function CONTROLS_ARRANGE_SIZE_FIX_X_GET_FROM_PFD(ServerObject_Prefix As String, SubPrefix As String, FieldName As String) As Integer
        Dim OUT As Integer = 0
        Dim ControlPressed As Boolean = My.Computer.Keyboard.CtrlKeyDown

        If ControlPressed = False Then
            Dim OUT_STR As String = ""
            Dim PFD_Key As String = FPApp.PFD_Key_SIZE_FIX_SAVE(ServerObject_Prefix, SubPrefix, FieldName)

            If FPApp.PFDlesen(PFD_Key, OUT_STR) Then
                OUT = Val(OUT_STR)
            End If
        End If

        Return OUT
    End Function


    Public Sub CONTROLS_ARRANGE(ByVal ServerObject_Prefix As String, ByVal SubPrefix As String, ByRef DT As DataTable, FieldPrefix As String)
        If CONTROLS_ARRANGE_Runs = False Then
            If Not (CONTROLS Is Nothing) Then
                CONTROLS_ARRANGE_Runs = True
                'Frm.SuspendLayout()

                If DT Is Nothing Then
                    FPApp.RS_FILL_FIELD_ARRANGE_TABLE(ServerObject_Prefix, SubPrefix, DT)
                End If

                If Not DT Is Nothing Then
                    Dim Row As DataRow = Nothing
                    Dim PrevControl As Control = Nothing
                    Dim PrevLabel As Label = Nothing
                    Dim PrevArrangeType As String = ""

                    For Each Row In DT.Rows
                        If Row!ControlName = "#FORM#" Then
                            Select Case Row!ArrangeType
                                Case "SIZE_FIX"
                                    Dim ChkBoxes(0) As Control
                                    Dim ChkBoxes_Locs(0) As Point

                                    Checkboxes_Location_SAVE(Frm, ChkBoxes, ChkBoxes_Locs)

                                    If Row!P1_Koo > 0 Then
                                        Frm.Width = Row!P1_Koo
                                    End If
                                    If Row!P2_Koo > 0 Then
                                        Frm.Height = Row!P2_Koo
                                    End If

                                    Checkboxes_Location_RESET(ChkBoxes, ChkBoxes_Locs)

                                Case "SIZE_HEIGHT_TO_MAX"
                                    SIZE_HEIGHT_TO_MAX(Frm, Row!P1_SpaceInPixel + Row!P2_SpaceInPixel)

                                Case "SIZE_WIDTH_TO_MAX"
                                    SIZE_WIDTH_TO_MAX(Frm, Row!P1_SpaceInPixel + Row!P2_SpaceInPixel)

                                Case Else
                                    FPApp.DoErrorMsgBox("FP_Form.CONTROLS_ARRANGE", 0, "You can use ControlName '#FORM#' only with Arrange Type 'SIZE_FIX', 'SIZE_HEIGHT_TO_MAX' Or 'SIZE_WIDTH_TO_MAX'")
                            End Select

                            If Frm.StartPosition = FormStartPosition.CenterScreen Then

                                Dim Scr As FP_L_Rect = FPApp.SCREEN_GET_WorkingArea(Frm)
                                Dim NewLoc As New Point(Scr.Left + (Scr.Width - Frm.Width) / 2, Scr.Top + (Scr.Height - Frm.Height) / 2)

                                If NewLoc.X < Scr.Left Then NewLoc.X = Scr.Left
                                If NewLoc.Y < Scr.Top Then NewLoc.Y = Scr.Top

                                Frm.Location = NewLoc
                            End If
                        Else
                            Dim RealFieldName As String = FieldPrefix + Row!ControlName

                            If Not CONTROLS.ContainsKey(RealFieldName) Then
                                FPApp.DoErrorMsgBox("FP_Form.CONTROLS_ARRANGE", 0, String.Format("Control '{0}' not found. (ServerObject_Prefix: '{1}', Subprefix = '{2}', SeqNum = {3})", RealFieldName, ServerObject_Prefix, SubPrefix, Row!SeqNum))
                            Else
                                Dim c As Control = CONTROLS(RealFieldName)
                                Dim c1 As Control = Nothing
                                Dim c2 As Control = Nothing
                                Dim P1_Control_Name As String = nz(Row!P1_Control, "")
                                Dim P2_Control_Name As String = nz(Row!P2_Control, "")

                                If P1_Control_Name > "" And Mid(P1_Control_Name, 1, 1) <> "#" Then
                                    P1_Control_Name = FieldPrefix + P1_Control_Name
                                End If

                                If P2_Control_Name > "" And Mid(P2_Control_Name, 1, 1) <> "#" Then
                                    P2_Control_Name = FieldPrefix + P2_Control_Name
                                End If

                                If CONTROLS_GET_FROM_NAME(c, PrevControl, PrevLabel, P1_Control_Name, c1) = False Then
                                    FPApp.DoErrorMsgBox(Me, "FP_Form.CONTROLS_ARRANGE", 0, String.Format("ServerObject_Prefix: '{0}', SubPrefix: '{1}', Row: {2}", ServerObject_Prefix, SubPrefix, Row!SeqNum))
                                Else
                                    If CONTROLS_GET_FROM_NAME(c, PrevControl, PrevLabel, P2_Control_Name, c2) Then
                                        Select Case Row!ArrangeType
                                            Case "LOCATION_FIX"
                                                c.Left = Row!P1_Koo * P_Layout_DPI_Factor_X
                                                c.Top = Row!P2_Koo * P_Layout_DPI_Factor_Y

                                            Case "ARRANGE_ON_LEFT"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_LEFT(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_ON_LEFT_TOP"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_LEFT_TOP(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_ON_LEFT_BOTTOM"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_LEFT_BOTTOM(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_ON_RIGHT"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_RIGHT(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_ON_RIGHT_TOP"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_RIGHT_TOP(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_ON_RIGHT_BOTTOM"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_RIGHT_BOTTOM(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_ON_TOP"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_TOP(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_ON_TOP_LEFT"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_TOP_LEFT(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_ON_TOP_RIGHT"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_TOP_RIGHT(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_ON_BOTTOM"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_BOTTOM(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_ON_BOTTOM_LEFT"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_BOTTOM_LEFT(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_ON_BOTTOM_RIGHT"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_BOTTOM_RIGHT(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_ON_CENTER_X"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_CENTER_X(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_ON_CENTER_Y"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_ON_CENTER_Y(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_TOPS"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_TOPS(c, c1, Row!P1_SpaceInPixel, Row!Percentage)
                                                End If

                                            Case "ARRANGE_BOTTOMS"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_BOTTOMS(c, c1, Row!P1_SpaceInPixel, Row!Percentage)
                                                End If

                                            Case "ARRANGE_LEFTS"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_LEFTS(c, c1, Row!P1_SpaceInPixel, Row!Percentage)
                                                End If

                                            Case "ARRANGE_RIGHTS"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_RIGHTS(c, c1, Row!P1_SpaceInPixel, Row!Percentage)
                                                End If

                                            Case "AS_NEXT_ROW_TO"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    ARRANGE_AS_NEXT_ROW(c, c1, c2, Row!Percentage, Row!P1_SpaceInPixel)
                                                    'If c2 Is Nothing Then
                                                    '    SIZE_WIDTH_TO(c, c1, 0, 0, Row!Percentage)
                                                    'Else
                                                    '    SIZE_WIDTH_TO(c, c1, c2, 0, 0, Row!Percentage)
                                                    'End If

                                                    ''A magassagot nem allitja az "AS_NEXT_ROW_TO" mert a ComboBox magassaga 150%-os betumeretnel 30, mig a textbox-e es a Label-e 29.
                                                    ''Vagyis ha egy DoFilter-ben Combobox utan textbox jon, akkor annak magassaga is 30 lesz, mig a label-e 29 es elcsusznak egymastol a sorok
                                                    ''SIZE_HEIGHT_TO(c, c1)
                                                    'ARRANGE_ON_BOTTOM(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "AS_PREV_ROW_TO"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    If c2 Is Nothing Then
                                                        SIZE_WIDTH_TO(c, c1, 0, 0, Row!Percentage)
                                                    Else
                                                        SIZE_WIDTH_TO(c, c1, c2, 0, 0, Row!Percentage)
                                                    End If

                                                    'A magassagot nem allitja az "AS_NEXT_ROW_TO" mert a ComboBox magassaga 150%-os betumeretnel 30, mig a textbox-e es a Label-e 29.
                                                    'Vagyis ha egy DoFilter-ben Combobox utan textbox jon, akkor annak magassaga is 30 lesz, mig a label-e 29 es elcsusznak egymastol a sorok
                                                    'SIZE_HEIGHT_TO(c, c1)
                                                    ARRANGE_ON_TOP_LEFT(c, c1, Row!P1_SpaceInPixel)
                                                End If

                                            Case "ARRANGE_FROM_TO"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    If ARRANGE_CHK_CONTROL(c.Name, c, P2_Control_Name, c2, Row!ArrangeType) Then
                                                        ARRANGE_FROM_TO(c, c1, c2, Row!P1_SpaceInPixel, Row!Percentage)
                                                    End If
                                                End If

                                            Case "SIZE_FIX"
                                                If Row!P1_Koo > 0 Then
                                                    Dim Saved_Koo_X As Integer = CONTROLS_ARRANGE_SIZE_FIX_X_GET_FROM_PFD(ServerObject_Prefix, SubPrefix, c.Name)

                                                    If Saved_Koo_X > 0 Then
                                                        SIZE_WIDTH_SET(c, Saved_Koo_X)
                                                    Else
                                                        SIZE_WIDTH_SET(c, Row!P1_Koo * P_Layout_DPI_Factor_X)
                                                    End If
                                                End If
                                                If Row!P2_Koo > 0 Then
                                                    SIZE_HEIGHT_SET(c, Row!P2_Koo * P_Layout_DPI_Factor_Y)
                                                End If

                                            Case "SIZE_SAME"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    SIZE_SAME(c, c1)
                                                End If

                                            Case "SIZE_HEIGHT_TO_MAX"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, Row!ArrangeType) Then
                                                    SIZE_HEIGHT_TO_MAX(c, Row!P1_SpaceInPixel + Row!P2_SpaceInPixel)
                                                End If

                                            Case "SIZE_WIDTH_TO_MAX"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, Row!ArrangeType) Then
                                                    SIZE_WIDTH_TO_MAX(c, Row!P1_SpaceInPixel + Row!P2_SpaceInPixel)
                                                End If

                                            Case "SIZE_WIDTH_TO"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    If c2 Is Nothing Then
                                                        SIZE_WIDTH_TO(c, c1, Row!P1_SpaceInPixel, Row!P2_SpaceInPixel, Row!Percentage)
                                                    Else
                                                        SIZE_WIDTH_TO(c, c1, c2, Row!P1_SpaceInPixel, Row!P2_SpaceInPixel, Row!Percentage)
                                                    End If
                                                End If

                                            Case "SIZE_HEIGHT_TO"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    If c2 Is Nothing Then
                                                        SIZE_HEIGHT_TO(c, c1, Row!P1_SpaceInPixel, Row!P2_SpaceInPixel, Row!Percentage)
                                                    Else
                                                        SIZE_HEIGHT_TO(c, c1, c2, Row!P1_SpaceInPixel, Row!P2_SpaceInPixel, Row!Percentage)
                                                    End If
                                                End If

                                            Case "SIZE_WIDTH_BETWEEN"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    If ARRANGE_CHK_CONTROL(c.Name, c, P2_Control_Name, c2, Row!ArrangeType) Then
                                                        SIZE_WIDTH_BETWEEN(c, c1, c2, Row!P1_SpaceInPixel, Row!P2_SpaceInPixel, Row!Percentage)
                                                    End If
                                                End If

                                            Case "SIZE_HEIGHT_BETWEEN"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    If ARRANGE_CHK_CONTROL(c.Name, c, P2_Control_Name, c2, Row!ArrangeType) Then
                                                        SIZE_HEIGHT_BETWEEN(c, c1, c2, Row!P1_SpaceInPixel, Row!P2_SpaceInPixel, Row!Percentage)
                                                    End If
                                                End If

                                            Case "SIZE_HEIGHT_TO_WIDTH"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    SIZE_HEIGHT_TO_WIDTH(c, c1, Row!P1_SpaceInPixel, Row!P2_SpaceInPixel, Row!Percentage)
                                                End If

                                            Case "SIZE_WIDTH_TO_HEIGHT"
                                                If ARRANGE_CHK_CONTROL(c.Name, c, P1_Control_Name, c1, Row!ArrangeType) Then
                                                    SIZE_WIDTH_TO_HEIGHT(c, c1, Row!P1_SpaceInPixel, Row!P2_SpaceInPixel, Row!Percentage)
                                                End If

                                            Case Else
                                                FPApp.DoErrorMsgBox("FP_Form.CONTROLS_ARRANGE", 0, String.Format("Unknown ArrangeType '{0}' for control '{1}'.", Row!ArrangeType, Row!ControlName))
                                        End Select
                                    End If
                                End If

                                If TypeOf (c) Is Label Then
                                    PrevLabel = c
                                Else
                                    PrevControl = c
                                End If
                                PrevArrangeType = Row!ArrangeType
                            End If
                        End If
                    Next
                End If
                CONTROLS_ARRANGE_Runs = False
                'Frm.ResumeLayout()
            End If
        End If
    End Sub
#End Region

#Region "PROTECTED"
    Protected Friend Frm_Handle As Long
    Protected Friend WithEvents FORM_DATETIME_MonthCalendar As MonthCalendar
    Protected Friend WithEvents Marker_sign As PictureBox
    Protected Friend FPc_DATETIME As New FP_Control(False)
    Protected Friend FPc_Marker_Sign As New FP_Control(False)

    Protected Friend HELP_Frm As FP_HELP
    Protected Friend ActiveControl As FP_Control = Nothing
    Protected Friend ARRANGE_DT As DataTable
    Protected Friend ProcessSavePoint_Active As Boolean = False

    Private FP_ID_Last As Integer = 0

    Protected Friend Function FP_Add(ByRef MyFP As FP, ByRef OUT_FP_ID As Integer) As Boolean
        Dim OUT As Boolean = False

        Dim Existing_FP As FP = GET_FP(MyFP.ServerObject_Prefix, MyFP.SubPrefix, MyFP.P_FieldPrefix)

        If Not (Existing_FP Is Nothing) Then
            OUT_FP_ID = Existing_FP.P_FP_ID
            OUT = True
        Else
            FP_ID_Last += 1
            OUT_FP_ID = FP_ID_Last

            FPs.Add(FP_ID_Last, MyFP)
            OUT = True
        End If

        FP_Add = OUT
    End Function
    Public Function CONTROLS_ADD(ByVal c As Control) As Boolean
        Dim OUT As Boolean = False

        If c Is Nothing Then
            FPApp.DoErrorMsgBox("FP_Form.CONTROLS_ADD", 0, "c is nothing.")
        Else
            If CONTROLS.ContainsKey(c.Name) Then
                FPApp.DoErrorMsgBox("FP_Form.CONTROLS_ADD", 0, String.Format("Control {0} already exists.", c.Name))
            Else
                If c.Parent Is Nothing Then
                    Frm.Controls.Add(c)
                End If

                Try
                    CONTROLS.Add(c.Name, c)

                    If TypeOf (c) Is TabControl Then
                        With SPECCONTROLS
                            If .ContainsKey(c.Name) Then
                                FPApp.DoErrorMsgBox("FP_Form.CONTROLS_ADD", 0, String.Format("Control {0} already exists in Dictionary 'FP_Form.SPECCONTROLS'.", c.Name))
                            Else
                                .Add(c.Name, New FP_SpecControl(Me, c))
                            End If
                        End With
                    ElseIf TypeOf (c) Is SplitContainer Then
                        With SPECCONTROLS
                            If .ContainsKey(c.Name) Then
                                FPApp.DoErrorMsgBox("FP_Form.CONTROLS_ADD", 0, String.Format("Control {0} already exists in Dictionary 'FP_Form.SPECCONTROLS'.", c.Name))
                            Else
                                .Add(c.Name, New FP_SpecControl(Me, c))
                            End If
                        End With
                    End If

                    OUT = True

                Catch ex As Exception
                    FPApp.DoErrorMsgBox("FP_Form.CONTROLS_ADD", Err.Number, Err.Description)
                End Try
            End If
        End If

        CONTROLS_ADD = OUT
    End Function

    Protected Friend Sub CONTROLS_REMOVE(ByVal ControlName As String)
        If CONTROLS.ContainsKey(ControlName) Then
            CONTROLS_REMOVE_CHILDCONTROLS_FROM_DIC(CONTROLS(ControlName), CONTROLS)
            If TypeOf (CONTROLS(ControlName)) Is TabControl Then
                If Not SPECCONTROLS.ContainsKey(ControlName) Then
                    FPApp.DoErrorMsgBox("CONTROLS_REMOVE", 0, String.Format("Control {0} is a TabControl and it does not exists in Dictionary SPECCONTROLS.", ControlName))
                Else
                    SPECCONTROLS(ControlName).Dispose()
                    SPECCONTROLS.Remove(ControlName)
                End If
            ElseIf TypeOf (CONTROLS(ControlName)) Is SplitContainer Then
                If Not SPECCONTROLS.ContainsKey(ControlName) Then
                    FPApp.DoErrorMsgBox("CONTROLS_REMOVE", 0, String.Format("Control {0} is a TabControl and it does not exists in Dictionary SPECCONTROLS.", ControlName))
                Else
                    SPECCONTROLS(ControlName).Dispose()
                    SPECCONTROLS.Remove(ControlName)
                End If
            End If
            CONTROLS.Remove(ControlName)
        End If
    End Sub

    Protected Friend Function CONTROLS_REFRESH_FROM_RS() As Boolean
        Dim OUT As Boolean = True

        If Not SAVE_ALL() Then
            OUT = False
        Else
            Dim Form_Control_Collection As New Struct_FP_FORM_CONTROLS_COLLECTION

            With Form_Control_Collection
                .Dlg_Btn_OK = Dlg_Btn_OK
                .Dlg_Btn_CANCEL = Dlg_Btn_CANCEL
                .Btn_SAVE = Btn_SAVE
                If Not (Btn_HELP Is Nothing) Then
                    If Not (Btn_HELP.c Is Nothing) Then
                        .Btn_HELP = Btn_HELP.c
                    End If
                End If
            End With

            If PICTUREBOXES.Keys.Count > 0 Then
                Dim Fields As String()
                Dim i As Integer

                Fields = PICTUREBOXES.Keys.ToArray
                For i = 0 To UBound(Fields)
                    With PICTUREBOXES(Fields(i))
                        If .CreatedBy = ENUM_FP_CONTROL_Created_by.RS Or .CreatedBy = ENUM_FP_CONTROL_Created_by.GRID Then
                            PICTUREBOXES_REMOVE(Fields(i))
                        End If
                    End With
                Next
            End If

            If Not (HELP_Frm Is Nothing) Then
                HELP_Frm.Dispose()
                HELP_Frm = Nothing
            End If

            CONTROLS.Clear()
            SPECCONTROLS.Clear()
            ARRANGE_DT = Nothing

            INIT_CONTROLS(Form_Control_Collection)

            DORESYNC_ALL()
        End If

        CONTROLS_REFRESH_FROM_RS = OUT
    End Function

    Protected Friend Sub CONTROLS_FROM_RS_SET_for_FORM(RS_Row As DataRow)
        Dim Color_Defs() As String = Split(RS_Row!COLORS, "|")
        Dim BG_COLOR As Color = Nothing

        If UBound(Color_Defs) < 4 Then
            ReDim Preserve Color_Defs(4)
        End If

        COLOR_GET_FROM_STR(Color_Defs(0), COLORS_FIELD_NORMAL_BG, "#FORM#", BG_COLOR)
        If Color_Defs(0) > "" Then
            Frm.BackColor = BG_COLOR
        End If
        With RS_Row
            If !BG_Image > "" Then
                BACKGROUND_SET(!BG_Image)
            End If
        End With
    End Sub

    Private Function CONTROLS_SET_PARENT(ByVal c As Control, ByVal Parent As String) As Boolean
        Dim OUT As Boolean = True

        If Parent > "" Then
            If Not CONTROLS.ContainsKey(Parent) Then
                FPApp.DoErrorMsgBox("FP_Form.CONTROLS_SET_PARENT", 0, String.Format("Parentcontrol '{0} does not exists'", Parent))
                OUT = False
            Else
                Try
                    c.Parent = CONTROLS(Parent)

                Catch ex As Exception
                    OUT = False
                    FPApp.DoErrorMsgBox("FP_Form.CONTROLS_SET_PARENT", Err.Number, Err.Description)
                End Try
            End If
        End If

        CONTROLS_SET_PARENT = OUT
    End Function

    Private Function CONTROLS_CREATE(ByVal Control_Props As Struct_CONTROL_PROPS, ByRef OUT_c As PictureBox) As Boolean
        Dim OUT As Boolean = True

        With Control_Props
            Select Case .Type.ToUpper
                Case "PICTUREBOX"
                    OUT_c = New PictureBox
                    OUT_c.BackColor = Color.Transparent
                    OUT_c.Size = New Size(44, 44)

                Case Else
                    OUT = False
                    FPApp.DoErrorMsgBox("FP_Form.CONTROLS_CREATE", 0, String.Format("Unknown Controltype {0} for field {1}", .Type, .Name))
            End Select

            If OUT = True Then
                OUT_c.Name = .Name
                OUT_c.Location = .ClientRectangle.Location
                OUT_c.Font = Font_NORMAL
                OUT_c.Visible = True
                OUT = CONTROLS_SET_PARENT(OUT_c, .Parent)

                OUT = CONTROLS_ADD(OUT_c)
            End If
        End With

        CONTROLS_CREATE = OUT
    End Function

    Protected Friend Function CONTROLS_FROM_RS_SET() As Boolean
        Dim OUT As Boolean = False
        Dim DT As New DataTable

        If FPApp.RS_FILL_FIELD_PARAMETER_TABLE(ServerObject_Prefix, "", DT) Then
            Dim Row As DataRow

            For Each Row In DT.Rows
                If Row!FieldName = "#FORM#" Then
                    CONTROLS_FROM_RS_SET_for_FORM(Row)
                Else
                    Dim c As Control = Nothing

                    If Row!CreateAtRuntime = True Then
                        If Row!CreateAtRuntime_FieldType <> "PICTUREBOX" Then
                            MsgBox(String.Format("FP_Form.CONTROLS_FROM_RS_SET: Field '{0}' is not a PictureBox.", Row!FieldName))
                        Else
                            Dim c_props As New Struct_CONTROL_PROPS

                            With c_props
                                .Name = Row!FieldName
                                .Parent = Row!Parent
                                .Type = Row!CreateAtRuntime_FieldType
                            End With
                            CONTROLS_CREATE(c_props, Nothing)
                        End If
                    End If

                    If Not CONTROLS.ContainsKey(Row!FieldName) Then
                        MsgBox(String.Format("FP_Form.CONTROLS_FROM_RS_SET: Field '{0}' not found.", Row!FieldName))
                    Else
                        c = CONTROLS(Row!FieldName)
                        If Not (TypeOf (c) Is PictureBox) Then
                            MsgBox(String.Format("FP_Form.CONTROLS_FROM_RS_SET: Field '{0}' is not a PictureBox.", Row!FieldName))
                        Else
                            Dim FPc As New FP_PictureBox(Me, Nothing, c, Row!BG_Toggle, Row!BG_Image)
                            FPc.CreatedBy = ENUM_FP_CONTROL_Created_by.RS
                            FPc.P_LOCKED = Row!Locked
                            FPc.P_VISIBLE = Row!Visible
                            FPc.CreatedAtRuntime = Row!CreateAtRuntime
                            PICTUREBOXES_ADD(FPc)
                        End If
                    End If
                End If
            Next
        End If

        CONTROLS_FROM_RS_SET = OUT
    End Function

    Protected Friend Sub Marker_SHOW(ByVal MyFPc As FP_Control)
        If Not (Frm Is Nothing) Then
            Dim MaxKoo As Point

            Dim FPc_Koo As New Point(Frm.PointToClient(MyFPc.c.PointToScreen(New Point(0, 0))))
            FPc_Marker_Sign = MyFPc

            With Marker_sign
                MaxKoo.X = Frm.ClientRectangle.Width - .Width
                MaxKoo.Y = Frm.ClientRectangle.Height - .Height

                .Left = FPc_Koo.X + MyFPc.c.Width - 2 * Marker_sign.Width
                If FPc_Koo.Y - Marker_sign.Height > 0 Then
                    .Top = FPc_Koo.Y - Marker_sign.Height + 1
                Else
                    .Top = FPc_Koo.Y + MyFPc.c.Height - 1
                End If

                If MyFPc.c.Visible Then
                    FIELD_VISIBLE(Marker_sign, True)
                    .BringToFront()
                Else
                    FIELD_VISIBLE(Marker_sign, False)
                End If
            End With
        End If
    End Sub

    Protected Friend Sub FORM_DATETIME_MonthCalendar_SET(ByVal MyFPc As FP_Control)
        Dim OUT As Boolean = True

        If MyFPc.P.xType_VB <> "DATETIME" Then
            FORM_DATETIME_MonthCalendar.Visible = False
        Else
            If Not (Frm Is Nothing) Then
                Dim MaxKoo As Point

                Dim FPc_Koo As New Point(Frm.PointToClient(MyFPc.c.PointToScreen(New Point(0, 0))))

                FPc_DATETIME = MyFPc

                With FORM_DATETIME_MonthCalendar
                    MaxKoo.X = Frm.ClientRectangle.Width - .Width
                    MaxKoo.Y = Frm.ClientRectangle.Height - .Height

                    .Left = IIf(FPc_Koo.X < MaxKoo.X, FPc_Koo.X, FPc_Koo.X + MyFPc.c.Width - .Width)
                    .Top = IIf(FPc_Koo.Y + MyFPc.c.Height < MaxKoo.Y, FPc_Koo.Y + MyFPc.c.Height, FPc_Koo.Y - .Height)

                    .TabIndex = MyFPc.c.TabIndex
                    .TabStop = False
                    If MyFPc.c.Text = "" Then
                        .SetDate(DateSerial(Now.Year, Now.Month, Now.Day))
                    Else
                        Dim MyDate As DateTime

                        If getDateFromStr(MyFPc.c.Text, MyDate) Then
                            .SetDate(MyDate)
                        End If
                    End If

                    Dim CalendarVisible As Boolean = (MyFPc.c.Visible And MyFPc.c.Focused And MyFPc.P.Locked = False)

                    If CalendarVisible Then
                        If MyFPc.FP.UnboundForm Then
                            CalendarVisible = MyFPc.FP.P_FORM_AllowEdits
                        Else
                            Select Case MyFPc.FP.P_DATA_RecordStatus
                                Case ENUM_RecordStatus.NORECORD
                                    CalendarVisible = False

                                Case ENUM_RecordStatus.EXISTS
                                    CalendarVisible = MyFPc.FP.P_FORM_AllowEdits

                                Case ENUM_RecordStatus.NEWRECORD
                                    CalendarVisible = MyFPc.FP.P_FORM_AllowAdditions
                            End Select
                        End If
                    End If

                    If CalendarVisible Then
                        FIELD_VISIBLE(FORM_DATETIME_MonthCalendar, True)
                        .BringToFront()
                    Else
                        FIELD_VISIBLE(FORM_DATETIME_MonthCalendar, False)
                    End If
                End With
            End If
        End If
    End Sub
#End Region

#Region "PRIVATE"
    Private Disposed As Boolean = False
    Private WithEvents Menu_Properties As ToolStripMenuItem
    Private WithEvents Menu_SAVE As ToolStripMenuItem
    Private WithEvents FOCUS_ON_AT_THE_END_Timer As New Timer

    Protected Friend FOCUS_ON_AT_THE_END_c As Control = Nothing
    Protected Friend FOCUS_ON_AT_THE_END_MsgNum As Long = 0
    Protected Friend FOCUS_ON_AT_THE_END_MsgParams As String

    Private asm As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly()

    Private Sub EVENT_Dlg_Btn_CANCEL_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Dlg_Btn_CANCEL.MouseUp
        If e.Button = MouseButtons.Left Then
            DLG_NAVIGATION_EXIT()
        End If
    End Sub
    Private Sub EVENT_Dlg_Btn_OK_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Dlg_Btn_OK.MouseUp
        If e.Button = MouseButtons.Left Then
            DLG_NAVIGATION_FORWARD()
        End If
    End Sub

    Private Sub Frm_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Frm.Activated
        If P_ENABLED Then
            FPApp.Forms_LastActivated = Me
        Else
            Dim FPf_Topmost As FP_Form = Nothing

            FPApp.ShowDialogForm_SET_ZORDER(FPApp.FORMS_GET_FPf(FPApp.ShowDialogForm_RootFrm), FPf_Topmost)
            If Not (FPf_Topmost Is Nothing) Then
                If Not FPf_Topmost.Equals(Me) Then
                    FPf_Topmost.Frm.Activate()
                Else
                    If P_ENABLED = False Then
                        If Not (P_Enabled_Form_Opacity Is Nothing) Then
                            P_Enabled_Form_Opacity.Activate()
                        End If
                    Else
                        FPApp.Forms_LastActivated = Me
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Frm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Frm.FormClosed
        RaiseEvent FORM_CLOSED(Me)
    End Sub

    Public Sub GRIDS_SAVE_ALL_Field_Length()
        If CONTROLS_ARRANGE_Runs = False Then
            If P_GRIDS_SAVE_ALL_Field_Length_ENABLED Then
                For Each Current_FP_ID In FPs.Keys
                    Dim Current_FP As FP = FPs(Current_FP_ID)

                    If Current_FP.GRID_EXISTS Then

                        Current_FP.GRID.SAVE_Field_Length()
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub EVENT_Frm_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Frm.FormClosing
        If FOCUS_ON_AT_THE_END_is_on() Then
            e.Cancel = True
        End If

        If Not e.Cancel Then
            RaiseEvent FORM_CLOSING(sender, e)
        End If

        If Not e.Cancel Then
            Marker_HIDE()

            For Each wHandle As Long In FPApp.Forms.Keys
                Dim wFPf As FP_Form = FPApp.Forms(wHandle)

                If Not (wFPf.P_Enabled_Form_Child Is Nothing) Then
                    If wFPf.P_Enabled_Form_Child.Equals(Me) Then
                        wFPf.P_Enabled_Form_Child = Nothing
                    End If
                End If
            Next

            If Dlg_Btn_CANCEL Is Nothing Then
                If Not SAVE_ALL() Then
                    e.Cancel = True
                End If
            End If

            If Not e.Cancel Then
                GRIDS_SAVE_ALL_Field_Length()

                LOCATION_SAVE()

                If Not (FPApp.StartForm Is Nothing) Then
                    If FPApp.StartForm.Equals(Me.Frm) Then
                        If Not FPApp.APPLICATION_PREPARE_QUIT Then
                            e.Cancel = True
                        End If
                    End If
                End If
            End If

            If Not e.Cancel Then
                RaiseEvent FORM_CLOSED(Me)
            End If

            '------------------------------------------------------------------------------------------------
            '-- Ezutan mar nem lehet elkaszalni a Form bezarasat! (Mert mar elment a Form_Closed esemeny
            '------------------------------------------------------------------------------------------------

            If Not e.Cancel Then
                If Cashable Then
                    '------------------------------------------------------------------------
                    '-- Cash the form
                    '------------------------------------------------------------------------
                    If P_Cash_State <> ENUM_Cash_State.CASHED Then
                        Dim Current_Cash_Identifier As String = P_Cash_Identifier

                        If Current_Cash_Identifier > "" Then
                            P_Cash_State = ENUM_Cash_State.CASHED
                            e.Cancel = True
                        End If
                    End If
                End If

                If Frm.Modal = False Then
                    Dim MySQL As String = String.Format("DELETE RS WHERE Terminals_ID = {0} And HWND = {1}", Terminals_ID, Frm_Handle)
                    FPApp.DC.Qdf_RunSQL(MySQL)
                End If

                If Not (InfoPanel Is Nothing) Then
                    InfoPanel.Dispose_ME()
                End If

                Dim MyRoot_FP As FP = ROOT_FP()

                If MyRoot_FP IsNot Nothing Then
                    ROOT_FP.DATA_LOGS_ACTIVITY_CLEAR()
                End If

                If P_Cash_State = ENUM_Cash_State.ACTIVE Then
                    Dispose()
                End If
            End If
        End If
    End Sub

    Private Sub CONTEXTMENU_DEBUG_SETUP()
        Menu_Properties = New ToolStripMenuItem("Properties")
        Menu_SAVE = New ToolStripMenuItem("Save Changes")
        ContextMenu_DEBUG = New ContextMenuStrip
        With ContextMenu_DEBUG.Items
            .Add(Menu_Properties)
            .Add("-")
            .Add(Menu_SAVE)
        End With
    End Sub
    Private Sub EVENT_MENU_Properties(ByVal Sender As Object, ByVal e As System.EventArgs) Handles Menu_Properties.Click
        If SAVE_ALL() Then
            Dim Source_FPo As FP_ControlObject = Nothing

            CONTROLS_GET_FPo_FROM_CONTROL(CType(CType(Sender, ToolStripMenuItem).Owner, ContextMenuStrip).SourceControl, Source_FPo)

            Dim DEBUG_MODE_FP_Setup As New FP_Services_Setup(Me, Source_FPo)
            DEBUG_MODE_FP_Setup.ShowDialog()
        End If
    End Sub

    Private Sub EVENT_MENU_SAVE() Handles Menu_SAVE.Click
        FPApp.DEBUG_MODE_SAVE()
    End Sub

    Private Sub FOCUS_ON_AT_THE_END_TICK(ByVal sender As Object, ByVal e As System.EventArgs) Handles FOCUS_ON_AT_THE_END_Timer.Tick
        FOCUS_ON_AT_THE_END_Timer.Enabled = False

        If Not Disposed Then
            Dim Current_c As Control = FOCUS_ON_AT_THE_END_c
            Dim Current_MsgNum As Long = FOCUS_ON_AT_THE_END_MsgNum
            Dim Current_MsgParams As String = FOCUS_ON_AT_THE_END_MsgParams

            FOCUS_ON_AT_THE_END_c = Nothing
            FOCUS_ON_AT_THE_END_MsgNum = 0
            FOCUS_ON_AT_THE_END_MsgParams = ""

            If Not (Current_c Is Nothing) Then
                If Not (Current_c Is Nothing) Then
                    Dim FPc As FP_Control = Nothing
                    CONTROLS_GET_FPc_FROM_CONTROL(Current_c, FPc)

                    If Not (FPc Is Nothing) Then
                        If FPc.FP.GRID_EXISTS Then
                            Dim GRID_Footer As Panel = FPc.FP.GRID.Footer_Panel

                            If Not (GRID_Footer Is Nothing) Then
                                Dim wc As Control = Current_c.Parent

                                Do While Not (wc Is Nothing)
                                    If wc.Equals(GRID_Footer) Then
                                        FPc.FP.GRID.FOOTER_SHOW()
                                        Exit Do
                                    End If

                                    If wc.Equals(Frm) Then
                                        Exit Do
                                    End If

                                    wc = wc.Parent
                                Loop
                            End If
                        End If
                    End If
                End If

                FOCUS_ON_IMMEDIATELY(Current_c)

                ProcessSavePoint_Active = False
            End If
            If Current_MsgNum <> 0 Then
                FPApp.DoMyMsgBox(Current_MsgNum, Current_MsgParams)
            End If
        End If

        ProcessSavePoint_Active = False
    End Sub

    Private Function ARRANGE_getParent(ByVal c As Control) As Control
        Dim OUT As Control = Nothing

        If c.Parent Is Nothing Then
            OUT = Frm
        Else
            OUT = c.Parent
        End If

        ARRANGE_getParent = OUT
    End Function

    Protected Friend Sub Marker_HIDE()
        Marker_sign.Visible = False
        FPc_Marker_Sign = Nothing
    End Sub


    Private Sub FORM_DATETIME_MonthCalendar_LostFocus() Handles FORM_DATETIME_MonthCalendar.LostFocus
        If Not (FPc_DATETIME Is Nothing) Then
            If Not (FPc_DATETIME.c Is Nothing) Then
                If Not FPc_DATETIME.c.Focused Then
                    FPc_DATETIME.COLORING()
                End If
            End If
        End If
    End Sub

    Private Frm_Minimized As Boolean = False

    Private Sub Frm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Frm.KeyDown
        If Keyboard_Enabled = False Then
            e.Handled = True
            Exit Sub
        End If

        Static Frm_Height_Orig As Integer = 0
        Static Frm_Opacity_Orig As Integer = 1

        If e.Handled = False Then
            Dim AltPressed As Boolean = My.Computer.Keyboard.AltKeyDown

            If AltPressed Then
                If Frm.WindowState = FormWindowState.Normal Then
                    If Frm_Minimized Then
                        If e.KeyCode = Keys.Down Then
                            Frm.Height = Frm_Height_Orig
                            Frm.Opacity = Frm_Opacity_Orig
                            Frm_Minimized = False
                            e.Handled = True
                        End If
                    Else
                        If e.KeyCode = Keys.Up Then
                            Dim DoIt As Boolean = True

                            If Not (ActiveControl Is Nothing) Then
                                If Not (ActiveControl.c_ComboBox Is Nothing) Then
                                    If ActiveControl.c_ComboBox.DroppedDown Then
                                        DoIt = False
                                    End If
                                End If
                            End If

                            If DoIt Then
                                Frm.AutoSize = False
                                Frm_Height_Orig = Frm.Height
                                Frm.Height = Form_Header_Height(Frm)
                                Frm.Opacity = 0.35
                                Frm_Minimized = True
                                e.Handled = True
                            End If
                        End If
                    End If
                End If
            End If

            If e.Handled = False Then
                If e.Control And e.Shift And e.KeyCode = Keys.S Then
                    e.Handled = True

                    If Not (InfoPanel Is Nothing) Then
                        FPApp.FORMS_BringToFront(InfoPanel)
                    Else
                        InfoPanel = New FP_Services_DebugInfoPanel(Me)
                    End If
                End If
            End If
        End If
    End Sub

    Public Function ROOT_FP() As FP
        Dim OUT As FP = Nothing

        If FPs.Keys.Count > 0 Then
            For Each Current_FP_ID In FPs.Keys
                If (FPs(Current_FP_ID).Parent_FP Is Nothing) Then
                    OUT = FPs(Current_FP_ID)
                    Exit For
                End If
            Next

            If (OUT Is Nothing) Then
                Dim Current_FP As FP = FPs(1).Parent_FP

                Do While Not (Current_FP Is Nothing)
                    OUT = Current_FP
                    Current_FP = Current_FP.Parent_FP
                Loop
            End If
        End If

        Return OUT
    End Function

    Private Sub Frm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Frm.KeyPress
        Dim ShiftPressed = My.Computer.Keyboard.ShiftKeyDown
        Dim CtrlPressed = My.Computer.Keyboard.CtrlKeyDown

        RaiseEvent FORM_KeyPreview_KeyPress(Me, sender, e)

        If e.Handled = False Then
            If CtrlPressed Then
                Dim RootFP As FP = ROOT_FP()

                If Not (RootFP Is Nothing) Then
                    ROOT_FP.RAISEEVENT_Form_KeyPreview_KeyPress(ROOT_FP, e)
                End If

                If e.Handled = False Then
                    If Not (RootFP Is Nothing) Then
                        Select Case Asc(e.KeyChar)
                            Case 6      'Ctrl-f, Ctrl-F
                                e.Handled = True

                                If RootFP.FORMDEF("FILTERNAME") > "" Then
                                    RootFP.FORM_RECORDS_LOAD()
                                End If

                            Case 12     'Ctrl-l, Ctrl-L
                                e.Handled = True

                                If Not (RootFP.ButtonFind Is Nothing) Then
                                    Dim ee As New System.EventArgs
                                    RootFP.ButtonFind_Click(RootFP.ButtonFind, ee)
                                End If

                            Case 13     'Ctrl-m, Ctrl-M
                                e.Handled = True
                                FPApp.RAISEEVENT_Central_Menu_Show()

                            Case 14     'Ctrl-n, Ctrl-N
                                e.Handled = True
                                If RootFP.UnboundForm = False Then
                                    If SAVE_ALL() Then
                                        RootFP.FORM_RECORDS_LOAD(RootFP.FORM_SubWHERE, True)
                                    End If
                                End If

                            Case 19     'Ctrl-s, Ctrl-S
                                e.Handled = True
                                SAVE_ALL()
                        End Select
                    End If
                End If
            End If
        End If

        If e.Handled = False Then
            If DialogMode Then
                If Asc(e.KeyChar) = 27 Then
                    Dim DoEsc As Boolean = True
                    If Not (ActiveControl Is Nothing) Then
                        If ActiveControl.c.Focused Then
                            DoEsc = False
                        End If
                    End If

                    If DoEsc Then
                        e.Handled = True
                        If P_FORM_Dirty Then
                            UNDO_ALL()
                        Else
                            Frm.Close()
                        End If
                    End If
                End If
            End If
        End If

        If Not e.Handled Then
            If AscW(e.KeyChar) = 13 Then 'Enter
                If ActiveControl Is Nothing Then
                    Btn_Default_RAISEEVENT_CLICK(e.Handled)
                Else
                    If Not ActiveControl.FIELD_IsDirty Then
                        Btn_Default_RAISEEVENT_CLICK(e.Handled)
                    End If
                End If

                If Not e.Handled Then
                    Dim DoIt As Boolean = True

                    If Not (ActiveControl Is Nothing) Then
                        If Not (ActiveControl.c Is Nothing) Then
                            If TypeOf (ActiveControl.c) Is TextBox Then
                                With CType(ActiveControl.c, TextBox)
                                    If .Multiline Then
                                        DoIt = ActiveControl.IsEntireFieldSelected
                                    End If

                                End With
                            End If
                        End If
                    End If

                    If DoIt Then
                        e.Handled = True
                        SendKeys.Send("{TAB}")
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Btn_Default_RAISEEVENT_CLICK(ByRef Handled As Boolean)
        Handled = False

        If Not (Btn_Default Is Nothing) Then
            If TypeOf (Btn_Default) Is PictureBox Then
                Dim FPp_Btn_Default As FP_PictureBox = Nothing

                If CONTROLS_GET_FPp_FROM_CONTROL(Btn_Default, FPp_Btn_Default) Then
                    Handled = True
                    FPp_Btn_Default.RAISEEVENT_CLICK()
                End If
            Else
                Dim FPc_Btn_Default As FP_Control = Nothing

                If CONTROLS_GET_FPc_FROM_CONTROL(Btn_Default, FPc_Btn_Default) Then
                    Dim ee As New MouseEventArgs(Windows.Forms.MouseButtons.Left, 1, Btn_Default.Left, Btn_Default.Top, 0)

                    FPc_Btn_Default.RAISEEVENT_Field_Click(Btn_Default, ee)
                End If
            End If
        End If
    End Sub

    Private Sub Frm_MouseWheel(sender As Object, e As MouseEventArgs) Handles Frm.MouseWheel
        If Not (ActiveControl Is Nothing) Then
            ActiveControl.EVENT_MOUSEWHEEL(sender, e)
        End If
    End Sub


    Private Sub EVENT_Frm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Frm.Resize
        If CONTROLS_ARRANGE_Runs = False Then
            GRIDS_SAVE_ALL_Field_Length()
            CONTROLS_ARRANGE_ALL()
        End If
    End Sub
#End Region

    Protected Friend FORM_DATETIME_MonthCalendar_Manipulated As Boolean = False

    Private Sub Marker_sign_Click(sender As Object, e As System.EventArgs) Handles Marker_sign.Click
        If Not (FPc_Marker_Sign Is Nothing) Then
            FPc_Marker_Sign.RAISEEVENT_Field_Marker_Click()
        End If
    End Sub

    Protected Friend Sub FORM_DATETIME_MonthCalendar_DateSelected(ByVal sender As Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles FORM_DATETIME_MonthCalendar.DateSelected
        If Not (FPc_DATETIME Is Nothing) Then
            With FPc_DATETIME
                FORM_DATETIME_MonthCalendar_Manipulated = True
                .c.Text = getStrDate(DATE_WITHOUT_TIME(FORM_DATETIME_MonthCalendar.SelectionStart))
                FOCUS_ON_IMMEDIATELY(.c)
                FORM_DATETIME_MonthCalendar_Manipulated = False
            End With
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Private Sub P_Enabled_Form_Opacity_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles P_Enabled_Form_Opacity.Activated
        If FPApp.ShowDialogForm_InActivate Then
            If (P_Enabled_Form_Child Is Nothing) Then
                FPApp.ShowDialogForm_InActivate = False
            Else
                P_Enabled_Form_Child.Activate()
            End If
        Else
            FPApp.ShowDialogForm_InActivate = True
            Frm.Activate()
        End If
    End Sub

    Private Sub Frm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Frm.Shown
        FPApp.Forms_LastActivated = Me
    End Sub

    '------------------------------------------------------------------------------------------------------------------------------------------------

#Region "Qdf_"
    Public Function Qdf_set_SP(ByRef sqlComm As SqlClient.SqlCommand, ByVal ProcName As String, Optional ByVal MyCommandTimeOut As Long = 30) As Boolean
        Return FPApp.DC.Qdf_set_SP(sqlComm, ProcName, MyCommandTimeOut)
    End Function

    Public Sub CNN_OPEN()
        FPApp.DC.CNN_OPEN()
    End Sub

    Public Function Qdf_Execute(ByRef sqlComm As SqlClient.SqlCommand, Optional ByVal OKValue As Long = -1, Optional ByVal DialNum As ENUM_ERRDIAL_TYPE = ENUM_ERRDIAL_TYPE.DIALNUM_STANDARD, Optional ByVal ErrParamName As String = "") As Boolean
        Return FPApp.DC.Qdf_Execute(ModuleIdentifier, sqlComm, OKValue, DialNum, ErrParamName)
    End Function
    Public Function Qdf_AddParameter(ByRef SqlComm As SqlClient.SqlCommand, ByVal ParamName As String, ByVal ParamType As Long, Optional ByVal ParamDirection As ParameterDirection = ParameterDirection.Input, Optional ByVal ParamSize As Long = 0, Optional ByVal StrValue As String = "", Optional ByVal DatetimeValue As Date = NULLDATE, Optional ByVal IntValue As Long = 0, Optional ByVal FloatValue As Double = 0, Optional ByVal XmlValue As Xml.XmlDocument = Nothing) As Boolean
        Return FPApp.DC.Qdf_AddParameter(SqlComm, ParamName, ParamType, ParamDirection, ParamSize, StrValue, DatetimeValue, IntValue, FloatValue, XmlValue)
    End Function
    Public Function Qdf_Fill_DT(ByVal MySQL As String, ByRef DT As DataTable) As Boolean
        Return FPApp.DC.Qdf_Fill_DT(MySQL, DT)
    End Function
    Public Function Qdf_RunSQL(ByVal mySQL As String, Optional ByVal MyCommandTimeOut As Long = 30) As Boolean
        Return FPApp.DC.Qdf_RunSQL(mySQL, MyCommandTimeOut)
    End Function
#End Region

    Public Function ZDISPO(ByVal P_ZDISPO As Struct_ZDISPO_Params, ByVal P_ZDISPO_OUT As Struct_ZDISPO_OutputParams) As Boolean
        P_ZDISPO.Parent_FPf = Me
        Return FPApp.ZDISPO(P_ZDISPO, P_ZDISPO_OUT)
    End Function

    Class FPf_Move

        Public FPApp As FP_App
        Public WithEvents Frm As Form

        Public Enabled As Boolean = True

        Private Disposed As Boolean = False

        Private Do_MOVE As Point = New Point(-99999, -99999)
        Private Do_RESIZE As Size

        Private In_Move As Boolean = False
        Private In_Resize As Boolean = False

        Private Frm_Stick_Distance As Integer = 20 'Megadja, hogy hany pontra kell lennie a form-oknak ahhoz, hogy "osszetapadjanak"

        Sub New(MyFPApp As FP_App, MyFrm As Form)
            FPApp = MyFPApp
            Frm = MyFrm

            If Frm.FormBorderStyle = FormBorderStyle.None Then
                Frm_Stick_Distance = 4
            End If
        End Sub

        Public Sub Dispose()
            If Disposed = False Then
                Frm = Nothing
                Disposed = True
                Enabled = False
            End If
        End Sub

        Protected Overrides Sub Finalize()
            Dispose()
            MyBase.Finalize()
        End Sub

        Private Sub Frm_Move(sender As Object, e As EventArgs) Handles Frm.Move
            '+++ El lett veve a kovetkezo feltetel, mert ha a Windows ugy van beallitva, hogy a form-nak csak a kerete mozogjon, akkor az Frm.Move
            'esemeny csak akkor lep fel, amikor az egergombot felengedik
            '
            'If Control.MouseButtons <> MouseButtons.Left Then
            '    Exit Sub
            'End If

            If Enabled = False Then
                Exit Sub
            End If

            If In_Move Or In_Resize Then
                Exit Sub
            End If

            If Is_Resize() Then
                Exit Sub
            End If

            In_Move = True
            Dim Moved_X As Boolean = False
            Dim Moved_Y As Boolean = False

            'Ablak illesztese a kepernyo szelehez vagy masik megnyitott ablakhoz
            Dim D As Integer = 9999
            Dim Frm_Handle As Long = Form_Handle(Frm)

            If Frm_Handle = 0 Then
                Exit Sub
            End If

            For Each h As Long In FPApp.Forms.Keys
                Dim CurrFrm As Form = FPApp.Forms(h).Frm
                Dim CurrFrm_Handle As Long = Form_Handle(CurrFrm)

                If CurrFrm_Handle <> Frm_Handle Then
                    If CurrFrm.WindowState <> FormWindowState.Minimized And CurrFrm.Visible = True Then

                        If Moved_X = False Then
                            D = Math.Abs(CurrFrm.Left - Frm.Left)

                            If D < Frm_Stick_Distance And D > 0 Then
                                Do_MOVE.X = CurrFrm.Left
                                'Do_MOVE.Y = Frm.Top

                                Moved_X = True
                            End If
                        End If

                        If Moved_X = False Then
                            D = Math.Abs(CurrFrm.Left - Frm.Right)

                            If D < Frm_Stick_Distance And D > 0 Then
                                Do_MOVE.X = CurrFrm.Left - Frm.Width
                                'Do_MOVE.Y = Frm.Top

                                Moved_X = True
                            End If
                        End If

                        If Moved_X = False Then
                            D = Math.Abs(CurrFrm.Right - Frm.Left)

                            If D < Frm_Stick_Distance And D > 0 Then
                                Do_MOVE.X = CurrFrm.Right
                                'Do_MOVE.Y = Frm.Top

                                Moved_X = True
                            End If
                        End If

                        If Moved_X = False Then
                            D = Math.Abs(CurrFrm.Right - Frm.Right)

                            If D < Frm_Stick_Distance And D > 0 Then
                                Do_MOVE.X = CurrFrm.Right - Frm.Width
                                'Do_MOVE.Y = Frm.Top

                                Moved_X = True
                            End If
                        End If

                        If Moved_Y = False Then
                            D = Math.Abs(CurrFrm.Top - Frm.Top)

                            If D < Frm_Stick_Distance And D > 0 Then
                                'Do_MOVE.X = Frm.Left
                                Do_MOVE.Y = CurrFrm.Top
                                Moved_Y = True
                            End If
                        End If

                        If Moved_Y = False Then
                            D = Math.Abs(CurrFrm.Top - Frm.Bottom)

                            If D < Frm_Stick_Distance And D > 0 Then
                                'Do_MOVE.X = Frm.Left
                                Do_MOVE.Y = CurrFrm.Top - Frm.Height

                                Moved_Y = True
                            End If
                        End If

                        If Moved_Y = False Then
                            D = Math.Abs(CurrFrm.Bottom - Frm.Top)

                            If D < Frm_Stick_Distance And D > 0 Then
                                'Do_MOVE.X = Frm.Left
                                Do_MOVE.Y = CurrFrm.Bottom

                                Moved_Y = True
                            End If
                        End If

                        If Moved_Y = False Then
                            D = Math.Abs(CurrFrm.Bottom - Frm.Bottom)

                            If D < Frm_Stick_Distance And D > 0 Then
                                'Do_MOVE.X = Frm.Left
                                Do_MOVE.Y = CurrFrm.Bottom - Frm.Size.Height

                                Moved_Y = True
                            End If
                        End If
                    End If

                    If Moved_X Or Moved_Y Then Exit For
                End If
            Next

            'kepernyo szelehez igazitas

            Dim Scr As FP_L_Rect = FPApp.SCREEN_GET_WorkingArea(Frm)

            If Not (Scr Is Nothing) Then
                If Moved_X = False Then
                    D = Math.Abs(Scr.Left - Frm.Left)

                    If D < Frm_Stick_Distance And D > 0 Then
                        Do_MOVE.X = Scr.Left
                        Moved_X = True
                    End If
                End If

                If Moved_Y = False Then
                    D = Math.Abs(Scr.Top - Frm.Top)

                    If D < Frm_Stick_Distance And D > 0 Then
                        Do_MOVE.Y = Scr.Top
                        Moved_Y = True
                    End If
                End If

                If Moved_X = False Then
                    D = Math.Abs(Scr.Right - Frm.Right)

                    If D < Frm_Stick_Distance And D > 0 Then
                        Do_MOVE.X = Scr.Right - Frm.Width
                        'Do_MOVE = Frm.Location
                        Moved_X = True
                    End If
                End If

                If Moved_Y = False Then
                    D = Math.Abs(Scr.Bottom - Frm.Bottom)

                    If D < Frm_Stick_Distance And D > 0 Then
                        Do_MOVE.Y = Scr.Bottom - Frm.Height
                        'Do_MOVE = Frm.Location
                        Moved_Y = True
                    End If
                End If

                If Moved_X = True And Moved_Y = True Then
                    Frm.Location = Do_MOVE
                ElseIf Moved_X = True Then
                    Frm.Left = Do_MOVE.X
                    Do_MOVE = Frm.Location
                ElseIf Moved_Y = True Then
                    Frm.Top = Do_MOVE.Y
                    Do_MOVE = Frm.Location
                Else
                    Do_MOVE = Frm.Location
                End If

                In_Move = False
            End If
        End Sub

        Private Function Is_Resize() As Boolean
            Dim OUT As Boolean = False

            If Do_MOVE.X = -99999 Then
                Frm_ResizeBegin(Nothing, Nothing)
            End If

            If Frm.Size <> Do_RESIZE Then
                OUT = True
            End If

            Return OUT
        End Function

        Private Sub Frm_Resize(sender As Object, e As EventArgs) Handles Frm.Resize
            If Enabled = True Then

                If In_Resize = False And In_Move = False Then
                    In_Resize = True

                    'Ablak illesztese a kepernyo szelehez vagy masik megnyitott ablakhoz
                    '+++ El lett veve a kovetkezo feltetel, mert ha a Windows ugy van beallitva, hogy a form-nak csak a kerete mozogjon, akkor az Frm.Move
                    'esemeny csak akkor lep fel, amikor az egergombot felengedik
                    '
                    'If Control.MouseButtons = MouseButtons.Left Then
                    Dim Sized_By_Left As Boolean = (Do_MOVE.X <> Frm.Left And Do_RESIZE.Width <> Frm.Width)
                    Dim Sized_By_Right As Boolean = (Do_MOVE.X = Frm.Left And Do_RESIZE.Width <> Frm.Width)
                    Dim Sized_By_Top As Boolean = (Do_MOVE.Y <> Frm.Top And Do_RESIZE.Height <> Frm.Height)
                    Dim Sized_By_Bottom As Boolean = (Do_MOVE.Y = Frm.Top And Do_RESIZE.Height <> Frm.Height)

                    Dim Resized_X As Boolean = False
                    Dim Resized_Y As Boolean = False

                    Dim D As Integer = 9999
                    Dim Frm_Handle As Long = Form_Handle(Frm)

                    For Each h As Long In FPApp.Forms.Keys
                        Dim CurrFrm As Form = FPApp.Forms(h).Frm
                        Dim CurrFrm_Handle As Long = Form_Handle(CurrFrm)

                        If CurrFrm_Handle <> Frm_Handle Then
                            If CurrFrm.WindowState <> FormWindowState.Minimized And CurrFrm.Visible = True Then

                                If Sized_By_Left Then
                                    If Math.Abs(Frm.Left - CurrFrm.Left) < Frm_Stick_Distance Then
                                        Do_RESIZE.Width = Do_MOVE.X + Do_RESIZE.Width - CurrFrm.Left
                                        Do_MOVE.X = CurrFrm.Left
                                        Resized_X = True
                                    ElseIf Math.Abs(Frm.Left - CurrFrm.Right) < Frm_Stick_Distance Then
                                        Do_RESIZE.Width = Do_MOVE.X + Do_RESIZE.Width - Frm.Left
                                        Do_MOVE.X = CurrFrm.Right
                                        Resized_X = True
                                    End If
                                End If

                                If Sized_By_Right Then
                                    If Math.Abs(Frm.Right - CurrFrm.Left) < Frm_Stick_Distance Then
                                        Do_RESIZE.Width = Do_RESIZE.Width + CurrFrm.Left - (Do_MOVE.X + Do_RESIZE.Width)
                                        Resized_X = True
                                    ElseIf Math.Abs(Frm.Right - CurrFrm.Right) < Frm_Stick_Distance Then
                                        Do_RESIZE.Width = Do_RESIZE.Width + CurrFrm.Right - (Do_MOVE.X + Do_RESIZE.Width)
                                        Resized_X = True
                                    End If
                                End If
                            End If

                            If Sized_By_Top Then
                                If Math.Abs(Frm.Top - CurrFrm.Top) < Frm_Stick_Distance Then
                                    Dim NewHeight As Integer = Frm.Top - CurrFrm.Top + Frm.Height

                                    Frm.Top = CurrFrm.Top
                                    Do_RESIZE.Height = NewHeight
                                    Resized_Y = True
                                ElseIf Math.Abs(Frm.Top - CurrFrm.Bottom) < Frm_Stick_Distance Then
                                    Dim NewHeight As Integer = Frm.Top - CurrFrm.Bottom + Frm.Height

                                    Frm.Top = CurrFrm.Bottom
                                    Do_RESIZE.Height = NewHeight
                                    Resized_Y = True
                                End If
                            End If

                            If Sized_By_Bottom Then
                                If Math.Abs(Frm.Bottom - CurrFrm.Top) < Frm_Stick_Distance Then
                                    Do_RESIZE.Height = CurrFrm.Top - Do_MOVE.Y
                                    Resized_Y = True

                                ElseIf Math.Abs(Frm.Bottom - CurrFrm.Bottom) < Frm_Stick_Distance Then
                                    Do_RESIZE.Height = CurrFrm.Bottom - Frm.Top
                                    Resized_Y = True
                                End If
                            End If

                            If Resized_X Or Resized_Y Then Exit For
                        End If
                    Next

                    'kepernyo szelehez igazitas

                    Dim Scr As FP_L_Rect = FPApp.SCREEN_GET_WorkingArea(Frm)

                    If Not (Scr Is Nothing) Then
                        If Resized_X = False Then
                            If Sized_By_Left Then
                                If Math.Abs(Frm.Left - Scr.Left) < Frm_Stick_Distance Then
                                    Do_MOVE.X = Scr.Left
                                    Do_RESIZE.Width = Do_MOVE.X + Do_RESIZE.Width - Scr.Left
                                    Resized_X = True
                                End If
                            End If

                            If Sized_By_Right Then
                                If Math.Abs(Frm.Right - Scr.Right) < Frm_Stick_Distance Then
                                    Do_RESIZE.Width = Do_RESIZE.Width + Scr.Right - (Do_MOVE.X + Do_RESIZE.Width)
                                    Resized_X = True
                                End If
                            End If
                        End If

                        If Resized_Y = False Then
                            If Sized_By_Top Then
                                If Math.Abs(Frm.Top - Scr.Top) < Frm_Stick_Distance Then
                                    'A Windows 10 maga intezi, ezert nem kell

                                    'Dim NewHeight As Integer = Resize_StartLoc.Y + Resize_StartSize.Height - Scr.Top
                                    'Frm.Top = Scr.Top
                                    'Frm.Height = NewHeight
                                    'Resized_Y = True
                                End If
                            End If

                            If Sized_By_Bottom Then
                                If Math.Abs(Frm.Bottom - Scr.Top) < Frm_Stick_Distance Then
                                    'A Windows 10 maga intezi, ezert nem kell

                                    'Frm.Height = Scr.Top - Resize_StartLoc.Y

                                    'Resized_Y = True
                                End If
                            End If
                        End If
                    End If

                    If Resized_X = False And Resized_Y = False Then
                        Do_RESIZE = Frm.Size
                        Do_MOVE = Frm.Location
                    Else
                        If Resized_X = True Then
                            Frm.Width = Frm.Size.Width
                        End If
                        If Resized_Y = True Then
                            Frm.Height = Frm.Size.Height
                        End If
                        If Do_MOVE.X > -99999 And Do_MOVE.Y > -99999 Then
                            Frm.Location = Do_MOVE
                        Else
                            If Do_MOVE.X > -99999 Then
                                Frm.Left = Do_MOVE.X
                            End If

                            If Do_MOVE.Y > -99999 Then
                                Frm.Top = Do_MOVE.Y
                            End If
                        End If
                    End If
                End If

                In_Resize = False
            End If
        End Sub

        Private Sub Frm_ResizeBegin(sender As Object, e As EventArgs) Handles Frm.ResizeBegin
            Do_MOVE = Frm.Location
            Do_RESIZE = Frm.Size
        End Sub

        Private Sub Frm_ResizeEnd(sender As Object, e As EventArgs) Handles Frm.ResizeEnd
            If Enabled = True Then
                If Frm.Location <> Do_MOVE Then
                    In_Move = True
                    Frm.Location = Do_MOVE
                    In_Move = False
                End If

                If Frm.Size <> Do_RESIZE Then
                    In_Resize = True
                    Frm.Size = Do_RESIZE
                    In_Resize = False
                End If
            End If

            In_Move = False
            In_Resize = False
        End Sub
    End Class

    Private Sub Marker_sign_MouseLeave(sender As Object, e As EventArgs) Handles Marker_sign.MouseLeave
        Dim Hide_Sign As Boolean = True

        If Not (Marker_sign Is Nothing) Then
            If Marker_sign.Visible Then
                If Not (FPc_Marker_Sign Is Nothing) Then
                    If Not (FPc_Marker_Sign.c Is Nothing) Then
                        If Not (ActiveControl Is Nothing) Then
                            If Not (ActiveControl.c Is Nothing) Then
                                If FPc_Marker_Sign.c.Equals(ActiveControl.c) Then
                                    Hide_Sign = False
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        If Hide_Sign Then
            Marker_sign.Visible = False
        End If
    End Sub
End Class

