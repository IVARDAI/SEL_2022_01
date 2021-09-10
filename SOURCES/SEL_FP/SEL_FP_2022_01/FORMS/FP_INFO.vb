Public Class FP_Info
    Public Event FP_Info_Details_Click(sender As FP_Info)

    Public Structure Struct_SEL_INFO_Params
        Dim ModuleIdentifier As String
        Dim SubPrefix As String
        Dim MsgNumber As Integer
        Dim MsgParams As String
        Dim TitleText_Params As String
    End Structure

    Public Enum ENUM_SEL_INFO_Timer_Status
        SLEEPING = 0
        WAITING_FOR_SHOW = 1
        SHOWED = 2
        HIDING = 3
        CLOSED = 4
    End Enum

    Public FPApp As FP_App = Nothing
    Public WithEvents FPf As FP_Form = Nothing
    Public WithEvents FP As FP = Nothing
    Public FPc_MessageText As FP_Control
    Public SubPrefix As String = ""
    Public P As New Struct_SEL_INFO_Params
    Public WithEvents T As Timer = Nothing
    Public T_Status As ENUM_SEL_INFO_Timer_Status = ENUM_SEL_INFO_Timer_Status.SLEEPING
    Public T_Hiding_State As Integer = 100
    Private Form_Closed As Boolean = False

    Sub New(MyFPApp As FP_App, MyP As Struct_SEL_INFO_Params)
        InitializeComponent()

        FPApp = MyFPApp
        P = MyP
        FPf = New FP_Form("FP_INFO_BASE", FPApp, Me, False)
        FPf.Location_Save_On_Close = False
        FPf.P_ShowCentralMenuOnClose = False

        FP = New FP(FPf, "FP_INFO", P.SubPrefix, True)

        FPf.INIT_CONTROLS(Nothing)
        FP.INIT_CONTROLS(Nothing)

        FPc_MessageText.P_VALUE = FPApp.Text_getDialogText(P.ModuleIdentifier, P.MsgNumber, P.MsgParams)

        If MyP.TitleText_Params > "" Then
            Me.Capture = MyP.TitleText_Params
        End If

        T_WAITING_FOR_SHOW_START()
    End Sub

    Private Sub T_STOP()
        T_Status = ENUM_SEL_INFO_Timer_Status.SLEEPING
        If Not (T Is Nothing) Then
            T.Stop()
            T.Enabled = False
            T.Dispose()
            T = Nothing
        End If
    End Sub

    Private Sub T_WAITING_FOR_SHOW_START()
        T_STOP()
        T = New Timer

        T_Status = ENUM_SEL_INFO_Timer_Status.WAITING_FOR_SHOW
        T.Interval = 2000
        T.Enabled = True
    End Sub

    Private Sub T_SHOW_START()
        T_STOP()
        T = New Timer

        T_Status = ENUM_SEL_INFO_Timer_Status.SHOWED
        T.Interval = 6000
        T.Enabled = True
    End Sub

    Private Sub T_HIDING_RESET()
        T_Hiding_State = 100
        Me.Opacity = T_Hiding_State / 100
        T_Status = ENUM_SEL_INFO_Timer_Status.SHOWED
    End Sub

    Private Sub T_HIDING_START()
        If T Is Nothing Then
            FPApp.DoErrorMsgBox(FPf, "FP_Info.T_HIDING_START", 0, "Timer T is nothing")
        Else
            T_Status = ENUM_SEL_INFO_Timer_Status.HIDING
            T_Hiding_State = 100

            Try
                Me.Opacity = T_Hiding_State / 100
                T.Interval = 70
                T.Enabled = True

            Catch ex As Exception
                T_STOP()
            End Try
        End If
    End Sub

    Private Sub FP_Info_MouseEnter(sender As Object, e As EventArgs) Handles Me.MouseEnter, Me.Click, Me.Activated
        T_STOP()
        T_HIDING_RESET()
        T_Status = ENUM_SEL_INFO_Timer_Status.SHOWED
    End Sub

    Private Sub FP_Info_MouseLeave(sender As Object, e As EventArgs) Handles Me.MouseLeave, Me.Deactivate
        If Me.Focused = False Then
            T_SHOW_START()
        End If
    End Sub

    Private Sub SEL_Info_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim ScrRect As New FP_L_Rect(Screen.PrimaryScreen.WorkingArea.Location, Screen.PrimaryScreen.WorkingArea.Size)

        Me.Location = ScrRect.RightBottom - Me.Size

        T_SHOW_START()
    End Sub

    Private Sub T_Tick(sender As Object, e As EventArgs) Handles T.Tick
        T.Enabled = False

        Select Case T_Status
            Case ENUM_SEL_INFO_Timer_Status.WAITING_FOR_SHOW
                If FPApp.FORMS_IS_DIALOG_OPEN Then
                    T.Interval = 2000
                    T.Enabled = True
                Else
                    Dim ActiveFPf As FP_Form = FPApp.FORMS_GET_ACTIVE_FPf()

                    If Not (ActiveFPf Is Nothing) Then
                        If ActiveFPf.Frm Is Nothing Then
                            ActiveFPf = Nothing
                        End If
                    End If

                    Show()

                    If Not (ActiveFPf Is Nothing) Then
                        ActiveFPf.Frm.Activate()
                    End If
                End If

            Case ENUM_SEL_INFO_Timer_Status.SLEEPING
                T_STOP()

            Case ENUM_SEL_INFO_Timer_Status.SHOWED
                T_HIDING_START()

            Case ENUM_SEL_INFO_Timer_Status.HIDING
                T_Hiding_State -= 2
                If T_Hiding_State <= 0 Then
                    If FPApp.FORMS_IS_DIALOG_OPEN Then
                        FPf.P_VISIBLE = False
                        T.Interval = 2000
                        T.Enabled = True
                    Else
                        T_STOP()
                        T_Status = ENUM_SEL_INFO_Timer_Status.CLOSED
                        Me.Close()
                    End If
                Else
                    Me.Opacity = T_Hiding_State / 100
                    T.Enabled = True
                End If

            Case Else
                FPApp.DoErrorMsgBox(FPf, "SEL_INFO.T_Tick", 0, String.Format("Unknown T_Status {0}", T_Status))
        End Select
    End Sub

    Private Sub More_Info_Label_Click(sender As Object, e As EventArgs) Handles More_Info_Label.Click
        RaiseEvent FP_Info_Details_Click(Me)
    End Sub

    Private Sub FPf_FORM_CLOSING(sender As Object, ByRef e As FormClosingEventArgs) Handles FPf.FORM_CLOSING
        T_STOP()
    End Sub

    Private Sub FP_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP.CONTROLS_INITIALIZED
        FPc_MessageText = FP.CONTROLS("MessageText")
        FPc_MessageText.P.xlength = 1024
    End Sub
End Class
