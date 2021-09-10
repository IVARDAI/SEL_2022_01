Imports System.Data
Imports System.Data.SqlClient

Public Class FP_Login
    Public WithEvents FPf As FP_Form
    Public WithEvents FP_L As FP

    Public WithEvents FPp_Btn_OK As FP_PictureBox
    Public WithEvents FPp_Btn_LoggedUsers As FP_PictureBox
    Public WithEvents FPp_License_Button As FP_Control

    Public WithEvents FPc_UserGroups As FP_Control
    Public WithEvents FPc_Users_ID As FP_Control
    Public WithEvents FPc_Password As FP_Control

    Public FPc_Title_Label As FP_Control

    Public User_ID_Old As Integer = 0
    Public UserGroups_ID_Old As Integer = 0

    Private Sub FP_Login_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim MySQL As String = String.Format("SELECT TOP 1 UserName, Users_ID, UserGroups_ID FROM LoggedUsers WHERE TerminalName = '{0}'", Terminal)
        Dim DT As New DataTable
        Dim DoIt As Boolean = True

        If Not gl_FPApp.DC.Qdf_Fill_DT(MySQL, DT) Then
            DoIt = False
        Else
            If DT.Rows.Count > 0 Then
                If nz(DT.Rows(0)!Users_ID, 0) = 0 Then
                    gl_FPApp.DC.Qdf_RunSQL(String.Format("DELETE Terminals WHERE TerminalName = '{0}'", Terminal))
                Else
                    DialogResult = Windows.Forms.DialogResult.OK
                    DoIt = False
                    gl_FPApp.DoMyMsgBox(28009, String.Format("{0}|{1}", Terminal, nz(DT.Rows(0)!UserName, ""))) 'Terminal allready logged in
                End If
            End If
        End If

        If DoIt = False Then
            Close()
        Else
            FPf = New FP_Form("FP_LOGIN_BASE", gl_FPApp, Me, True) With {
                .Location_Save_On_Close = False
            }

            FP_L = New FP(FPf, "FP_LOGIN", "", True)
        End If
    End Sub

    Private Sub FP_Login_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Dim FPf_CONTROLS_COLLECTION As New Struct_FP_FORM_CONTROLS_COLLECTION
        With FPf_CONTROLS_COLLECTION
            .Btn_HELP = Btn_HLP
            .Dlg_Btn_CANCEL = Btn_Cancel
        End With
        FPf.INIT_CONTROLS(FPf_CONTROLS_COLLECTION)

        Dim FP_LOGIN_CONTROLS_COLLECTION As New Struct_FP_CONTROLS_COLLECTION
        FP_L.INIT_CONTROLS(FP_LOGIN_CONTROLS_COLLECTION)
        WB.Url = New System.Uri(String.Format(FPc_Title_Label.P.Tag, gl_FPApp.LandDialog))

        Dim User_ID_Old_STR As String = ""
        Call gl_FPApp.PFDlesen("USER", User_ID_Old_STR)
        User_ID_Old = Val(User_ID_Old_STR)

        Dim DT As DataTable = Nothing
        Dim MySQL As String = String.Format("SELECT UserGruppe UserGroups_ID FROM Users WHERE ID = {0}", User_ID_Old)

        gl_FPApp.DC.Qdf_Fill_DT(MySQL, DT)
        If DT.Rows.Count < 1 Then
            User_ID_Old = 0
            FPf.FOCUS_ON_AT_THE_END(UserGroups)
        Else
            UserGroups_ID_Old = DT.Rows(0)!UserGroups_ID
            FPc_UserGroups.P_VALUE = UserGroups_ID_Old
            FP_L_Form_Field_AfterUpdate(FPc_UserGroups)
            FPc_Users_ID.P_VALUE = User_ID_Old
            FPf.FOCUS_ON_AT_THE_END(Password)
        End If
    End Sub

    Private Sub FPf_CONTROLS_INITIALIZED(ByVal sender_FPf As FP_Form) Handles FPf.CONTROLS_INITIALIZED
        FPp_Btn_OK = FPf.PICTUREBOXES("Btn_OK")
        FPp_Btn_LoggedUsers = FPf.PICTUREBOXES("Btn_LoggedUsers")
    End Sub

    Private Function getAktLoginName() As String
        Dim OUT As String = ""

        If FPc_Users_ID.P_VALUE <> 0 Then
            OUT = Users_ID.FocusedItem.SubItems(0).Text
        End If

        Return OUT
    End Function

    Private Sub FPp_Btn_OK_CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FPp_Btn_OK.CLICK
        If FP_L.FORM_CHK_FIELDS Then
            If Not gl_FPApp.Login(getAktLoginName, Password.Text) Then
                FPf.FOCUS_ON_AT_THE_END(Password)
            Else
                gl_FPApp.PFDinsertOrUpdate("OLDUSERID", FPc_Users_ID.P_VALUE)

                DialogResult = Windows.Forms.DialogResult.OK
                gl_FPApp.Users_setUserGlobals()
                Close()
            End If
        End If
    End Sub

    Private Sub FPc_Users_ID_SET_LISTVIEW()
        Dim Criteria As String = String.Format("UserGruppe = {0}", FPc_UserGroups.P_VALUE)

        With FPc_Users_ID
            If Criteria <> .P.DT_WHERE2 Then
                .P_VALUE = 0
                .P.DT_WHERE2 = Criteria

                .DT_REFRESH()
                If .DT.Rows.Count > 0 Then
                    .P_VALUE = .DT.Rows(0)!ID
                End If
            End If
        End With
    End Sub

    Private LICENSE_EXPIRATION_TEXT As String = ""

    Private Sub LICENSE_EXPIRATION_GET()
        Dim LicenseExpDate_STR As String = ""

        gl_FPApp.ParmLesen("LZ", "GULTIG", 0, LicenseExpDate_STR)

        Dim LicenseExpDate As DateTime = DBFORMAT_get_Date_From_DbStr(LicenseExpDate_STR)

        If LICENSE_EXPIRATION_TEXT = "" Then
            LICENSE_EXPIRATION_TEXT = License_Button.Text
        End If

        License_Button.Text = String.Format(LICENSE_EXPIRATION_TEXT, getStrDate(LicenseExpDate))

        Dim NearToExpire As Boolean = False

        If LicenseExpDate = NULLDATE Then
            NearToExpire = True
        Else
            If DateDiff(DateInterval.Day, Now, LicenseExpDate) < 20 Then
                NearToExpire = True
            End If
        End If

        If NearToExpire Then
            With FPp_License_Button
                .P_BACKGROUNDIMAGE = "RndBtn_Long_06.png"
                .c.ForeColor = Color.FromArgb(255, 255, 255)
                .c.TabStop = True
            End With
        Else
            With FPp_License_Button
                .P_BACKGROUNDIMAGE = "RndBtn_Long_01.png"
                .c.ForeColor = Color.FromArgb(106, 107, 107)
                .c.TabStop = False
            End With
        End If
    End Sub

    Private Sub FP_L_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP_L.CONTROLS_INITIALIZED
        With FP_L
            FPc_Title_Label = .CONTROLS("Title_Label")
            FPc_UserGroups = .CONTROLS("UserGroups")
            FPc_Users_ID = .CONTROLS("Users_ID")
            FPc_Password = .CONTROLS("Password")
            FPp_License_Button = .CONTROLS("License_Button")
        End With

        LICENSE_EXPIRATION_GET()
    End Sub

    Private Sub FP_L_Form_Field_AfterUpdate(ByVal FPc As FP_Control) Handles FP_L.Form_Field_AfterUpdate
        If FPc.Equals(FPc_UserGroups) Then
            FPc_Users_ID_SET_LISTVIEW()
        End If

        FP_L.COLORING_ALL()
    End Sub

    '-------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------

    '    Sub WerteErmitteln_AfterUpdate(ByVal AbWert As Integer)
    '        On Error GoTo WerteErmitteln_AfterUpdate_err

    '        If AbWert <= AB_ComboUserGruppen Then
    '            Me.UserID.RowSource = "SELECT Users.ID, Users.KurzName, Alkalmazottak.Nev " & _
    '                                  " FROM Users LEFT JOIN Alkalmazottak ON Users.Alkalmazottak_ID = Alkalmazottak.ID " & _
    '                                  " WHERE UserGruppe=" & Nz(Me.ComboUserGruppen, 0) & " And IsNull(Alkalmazottak.KilepettIN, 0) = 0"

    '            Call Field_Repaint(Me, Me.UserID)

    '            If Nz(Me.ComboUserGruppen.Value, 0) = 0 Then
    '                Me.UserID.Value = 0
    '            End If
    '            If Nz(Me.UserID.Value, 0) <> 0 Then
    '                If Nz(DLookup("UserGruppe", "Users", "ID=" & Me.UserID.Value), 0) <> Nz(Me.ComboUserGruppen, 0) Then
    '                    Me.UserID.Value = 0
    '                End If
    '            End If
    '        End If

    '        If AbWert <= AB_UserID Then
    '            'Muss nichts tun
    '        End If

    '        If AbWert <= AB_Password Then
    '            'Muss nichts tun
    '        End If

    '        Call Form_FeldEigenschafteneinstellen(Me)

    '        Exit Sub

    'WerteErmitteln_AfterUpdate_err:
    '        Call DoErrorMsgBox("Users.WerteErmitteln_AfterUpdate", Err.Number, Err.Description)
    '        Exit Sub
    '    End Sub

    Private Sub FP_L_Form_Field_Enter(ByVal FPc As FP_Control, ByRef Handled As Boolean) Handles FP_L.Form_Field_Enter
        Dim From_Tabindex As Integer = FPc.c.TabIndex

        If (Not Handled) And From_Tabindex > UserGroups.TabIndex Then
            If FPc_UserGroups.P_VALUE = 0 Then
                FPf.FOCUS_ON_AT_THE_END(UserGroups)
                Handled = True
            End If
        End If
    End Sub

    Private Sub Password_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Password.KeyDown, Users_ID.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                e.Handled = True
                Dim ee As New System.Windows.Forms.MouseEventArgs(Windows.Forms.MouseButtons.Left, 1, 0, 0, 0)
                FPp_Btn_OK_CLICK(FPp_Btn_OK, ee)

            Case Else
                'Nothing to do
        End Select
    End Sub

    Private Sub FPc_Users_ID_Field_Click(sender As Object, e As EventArgs) Handles FPc_Users_ID.Field_Click
        FPf.FOCUS_ON_AT_THE_END(FPc_Password.c)
    End Sub

    Private Sub FPc_Users_ID_Field_Doubleclick(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByVal e As System.EventArgs, ByRef Handled As Boolean) Handles FPc_Users_ID.Field_Doubleclick
        Dim ee As New System.Windows.Forms.MouseEventArgs(Windows.Forms.MouseButtons.Left, 1, 0, 0, 0)

        FPp_Btn_OK_CLICK(FPp_Btn_OK, ee)
    End Sub

    Private Sub FPp_Btn_LoggedUsers_CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FPp_Btn_LoggedUsers.CLICK
        Dim CountOfMaxUsers As Integer = 0
        Dim CountOfLoggedUsers As Integer = 0
        Dim wstr As String = ""
        Dim DT As New DataTable
        Dim MySQL As String = "SELECT COUNT(*) CountOfLoggedUsers FROM Terminals"

        gl_FPApp.DC.Qdf_Fill_DT(MySQL, DT)
        CountOfLoggedUsers = DT.Rows(0)!CountOfLoggedUsers

        Call gl_FPApp.ParmLesen("LZ", "ANZUSER", 0, wstr)
        CountOfMaxUsers = Val(wstr)

        Dim ListOfLoggedUsers As New FP_Simple_Edit(gl_FPApp, "LOGGED_USERS")

        ListOfLoggedUsers.DATAFIELD_ADD("CountOfFreePlaces", (CountOfMaxUsers - CountOfLoggedUsers).ToString)

        gl_FPApp.ShowDialogForm(ListOfLoggedUsers)
    End Sub

    Private Sub FPc_License_Button_Field_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FPp_License_Button.Field_Click
        Dim F As New FP_KeyGen

        gl_FPApp.ShowDialogForm(F)

        LICENSE_EXPIRATION_GET()

        FPf.FOCUS_ON_AT_THE_END(Password)
    End Sub

    Private Sub FPc_UserGroups_Field_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc_UserGroups.Field_TextChanged
        FP_L_Form_Field_AfterUpdate(FPc_UserGroups)
    End Sub

    Private Sub UserGroups_DropDownClosed(sender As Object, e As EventArgs) Handles UserGroups.DropDownClosed
        FPf.FOCUS_ON_AT_THE_END(FPc_Password.c)
    End Sub

    Private Sub FPc_Password_Field_KeyPreview_KeyDown(sender_FPc As FP_Control, sender As Object, ByRef e As KeyEventArgs) Handles FPc_Password.Field_KeyPreview_KeyDown
        Select Case e.KeyCode
            Case Keys.Up
                e.Handled = True
                FPc_Users_ID.LISTVIEW_GOTO_PREVIOUS_ITEM()

            Case Keys.Down
                e.Handled = True
                FPc_Users_ID.LISTVIEW_GOTO_NEXT_ITEM()
        End Select
    End Sub

    Private Sub FPf_FORM_KeyPreview_KeyPress(sender_FPf As FP_Form, ByRef sender As Object, ByRef e As KeyPressEventArgs) Handles FPf.FORM_KeyPreview_KeyPress
        Dim CtrlPressed = My.Computer.Keyboard.CtrlKeyDown

        If CtrlPressed Then
            Select Case Asc(e.KeyChar)
                Case 21 'Ctrl-U 'Kenyszeritett update
                    If FPf.DoMyMsgBox(1517, , "SEQ,YES", "SEQ,NO") = 1 Then
                        Dim Selexped_Updater_Location As String = ""

                        If gl_FPApp.UPDATE_PREPARE(Selexped_Updater_Location, False) Then
                            Process.Start(Selexped_Updater_Location)
                            Me.DialogResult = Windows.Forms.DialogResult.Cancel
                            Me.Close()
                        End If
                    End If

            End Select
        End If
    End Sub

    Private Sub WB_Navigating(sender As Object, e As WebBrowserNavigatingEventArgs) Handles WB.Navigating
        If FPc_Title_Label IsNot Nothing Then
            If e.Url.ToString <> String.Format(FPc_Title_Label.P.Tag, gl_FPApp.LandDialog) Then
                Process.Start(e.Url.ToString)
                e.Cancel = True
            End If
        End If
    End Sub
End Class
