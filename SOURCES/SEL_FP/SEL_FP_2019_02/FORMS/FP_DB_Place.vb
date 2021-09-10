Imports System.Data
Imports System.Data.SqlClient

Public Class FP_DB_Place
    Private DC As FP_DataConnect
    Private FpApp As FP_App

    Private Cnn As New SqlClient.SqlConnection
    Private ConnectString$
    Private Connect_SQLServer$
    Private Connect_IntegratedSecurity As Boolean
    Private Connect_DBName$
    Private Connect_UserId$
    Private Connect_Password$
    Private OnlySelexpedDB As Boolean = True
    Private SelexpedVersion$ = FP_Globals.VERS
    Private pOpenArgs As String
    Private Link_Install_Guide As String = ""

    Public Property OpenArgs() As String
        Get
            Return pOpenArgs
        End Get
        Set(ByVal value As String)
            pOpenArgs = value
        End Set
    End Property
    Public Sub New(ByVal MyDC As FP_DataConnect, Optional ByVal Let_OnlySelexpedDB As Boolean = True, Optional ByVal Let_SelexpedVersion As String = VERS, Optional ByVal LoadFromINI As Boolean = True)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        DC = MyDC
        FpApp = DC.FPApp

        Try
            ' Add any initialization after the InitializeComponent() call.
            P_OnlySelexpedDB = Let_OnlySelexpedDB
            SelexpedVersion = Let_SelexpedVersion
            If LoadFromINI Then
                Call M_setConnectionFromINI(False)
            End If

        Catch ex As Exception
            FpApp.DoErrorMsgBox("DB_Place.New", Err.Number, Err.Description)
            Exit Sub
        End Try
    End Sub

    Public ReadOnly Property P_Connection() As SqlClient.SqlConnection
        Get
            P_Connection = Cnn
        End Get
    End Property

    Public ReadOnly Property P_IsConnected() As Boolean
        Get
            P_IsConnected = (Cnn.ConnectionString > "")
        End Get
    End Property

    Public Property P_OnlySelexpedDB() As Boolean
        Get
            P_OnlySelexpedDB = OnlySelexpedDB
        End Get
        Set(ByVal value As Boolean)
            OnlySelexpedDB = value
        End Set
    End Property

    Public ReadOnly Property P_SelexpedVersion As String
        Get
            P_SelexpedVersion = SelexpedVersion
        End Get
    End Property

    Public Function M_CloseConnection() As Boolean
        Dim OUT As Boolean = (Cnn.State = ConnectionState.Closed)

        If OUT = False Then
            OUT = (Cnn.ConnectionString = "")
        End If

        If OUT = False Then
            Try
                Cnn.Close()
                OUT = True

            Catch ex As Exception
                OUT = False
            End Try
        End If

        Return OUT
    End Function

    Public Function M_setConnectionFromString(ByVal MyConnectString As String, Optional ByVal mitDialog As Boolean = True, Optional ByVal DialNum As Long = 0, Optional ByVal OnlySelexpedDB As Boolean = True, Optional VersionCheck As Boolean = True) As Boolean
        Try
            If Not M_CloseConnection() Then
                M_setConnectionFromString = False
                Exit Function
            End If

            If Not FpApp.DC.ConnectString_Check(MyConnectString$, mitDialog, DialNum, OnlySelexpedDB, VersionCheck) Then
                M_setConnectionFromString = False
                Exit Function
            End If

            Cnn = New SqlClient.SqlConnection()

            Cnn.ConnectionString = MyConnectString

            Me.SQLServer.Text = FpApp.DC.ConnectString_getSQLServerNameFromConnectString(MyConnectString$)
            Me.DbName.Text = FpApp.DC.ConnectString_getDBNameFromConnectString(MyConnectString$)
            Me.IntegratedSecurity.Checked = FpApp.DC.ConnectString_getIntegratedSecurity(MyConnectString$)
            Me.UserId.Text = FpApp.DC.ConnectString_getUserIdFromConnectString(MyConnectString$)
            Me.Password.Text = FpApp.DC.ConnectString_getPasswordFromConnectString(MyConnectString$)

            M_setConnectionFromString = True
            Exit Function

        Catch ex As Exception
            M_setConnectionFromString = False
            FpApp.DoErrorMsgBox("Sel_DB_Place.M_SetConnectionFromString", Err.Number, Err.Description)
            Exit Function
        End Try
    End Function

    Public Function M_setConnectionFromINI(Optional ByVal mitDialog As Boolean = True, Optional ByVal DialNum As Long = 0, Optional VersionCheck As Boolean = True) As Boolean
        Dim INIConnectstring$ = ""

        Try
            FpApp.PFDlesen("CONNECT_VB", INIConnectstring$)
            M_setConnectionFromINI = M_setConnectionFromString(INIConnectstring$, mitDialog, DialNum, OnlySelexpedDB, VersionCheck)
        Catch ex As Exception
            M_setConnectionFromINI = False
            FpApp.DoErrorMsgBox("Sel_DB_Place.M_setConnectionFromINI", Err.Number, Err.Description)
            Exit Function
        End Try
    End Function

    Public Function M_OpenConnection(Optional WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = (Cnn.State = ConnectionState.Open)

        If OUT = False Then
            If Cnn.ConnectionString > "" Then
                Try
                    Cnn.Open()
                    While Cnn.State <> ConnectionState.Open
                        System.Threading.Thread.Sleep(200)
                    End While
                    OUT = True

                Catch ex As Exception
                    OUT = False
                    If WithDialog Then
                        DoErrorMsgBox_Without_SQL_Connection("Sel_Db_Place.M_OpenConnection", Err.Number, Err.Description)
                    End If
                End Try
            End If
        End If

        Return OUT
    End Function

    Private Sub SET_LAYOUT()
        Me.UserPassword_Panel.Enabled = (Not Me.IntegratedSecurity.Checked)
    End Sub

    Private Sub IntegratedSecurity_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IntegratedSecurity.CheckedChanged
        Call SET_LAYOUT()
    End Sub

    Private Sub DB_Place_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Trim$(nz(FpApp.LandDialog$, "")) = "" Then
            FpApp.PFDlesen("LANDDIALOG", FpApp.LandDialog$)
        End If
        ComputerName.Text = My.Computer.Name
        StartupPath.Text = Application.StartupPath

        Select Case getOpenTyp()
            Case 0 'Megvizsgalja, hogy jok-e a kapcsolatok. Ha igen, akkor nem csinal semmit.
                If Me.ChkDB_Params(False) Then
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Close()
                    Exit Sub
                End If

            Case 1 'A kapcsolatokat mindenkeppen fel kell venni
                'Muss nichts tun

            Case 2 'Automatikusan inditja a tovabb gombot. (A megadott parameterekkel probalja felvenni a kapcsolatokat)
                If Me.ChkDB_Params(False) = True Then
                    If AlleKontakteErstellen() Then
                        Me.DialogResult = Windows.Forms.DialogResult.OK
                        Exit Sub
                    End If
                End If

            Case Else
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                MsgBox("Form_DB_Place.DB_Place_Load error: the starter parameter are wrong.", , ProgramName$)
                Close()
                Exit Sub
        End Select

        WB.Url = New System.Uri(String.Format("http://www.webandtrace.com/SELEXPED/Connect_to_DB//Connect_to_DB_{0}.htm", FpApp.LandDialog))
    End Sub

    Public Function Public_BeforeUpdate(Optional WithTerminalChk As Boolean = True) As Boolean
        Dim MyConnectString$
        Try
            If Trim$(Me.SQLServer.Text) = "" Then
                Public_BeforeUpdate = False
                Me.SQLServer.Focus()
                FpApp.DoMyMsgBox_From_Resources(7) 'Adatmegadas kotelezo
                Exit Function
            End If

            If Trim$(Me.DbName.Text) = "" Then
                Public_BeforeUpdate = False
                Me.DbName.Focus()
                FpApp.DoMyMsgBox_From_Resources(7)   'Adatmegadas kotelezo
                Exit Function
            End If

            If Me.IntegratedSecurity.Checked = False Then
                If Trim$(Me.UserId.Text) = "" Then
                    Public_BeforeUpdate = False
                    Me.UserId.Focus()
                    FpApp.DoMyMsgBox_From_Resources(7)   'Adatmegadas kotelezo
                    Exit Function
                End If
            End If

            MyConnectString$ = FpApp.DC.ConnectString_getConnectStringFromElements(Me.SQLServer.Text, Me.DbName.Text, Me.IntegratedSecurity.Checked, Me.UserId.Text, Me.Password.Text)

            CURSOR_SHOW_WAIT()
            If Not FpApp.DC.ConnectString_Check(MyConnectString$, , , , False) Then
                CURSOR_SHOW_DEFAULT()
                Public_BeforeUpdate = False
                Exit Function
            End If

            If WithTerminalChk = True Then
                If Trim(Pfd_Terminal.Text) = "" Then
                    CURSOR_SHOW_DEFAULT()
                    Me.Pfd_Terminal.Focus()
                    FpApp.DoMyMsgBox_From_Resources(7) 'Adatmegadas kotelezo
                    Public_BeforeUpdate = False
                    Exit Function
                End If
            End If

            If Len(Pfd_Terminal.Text) > 10 Then
                CURSOR_SHOW_DEFAULT()
                Me.Pfd_Terminal.Focus()
                FpApp.DoMyMsgBox_From_Resources(8) 'A Terminal max 10 karakter-bol allhat.
                Public_BeforeUpdate = False
                Exit Function
            End If

            Public_BeforeUpdate = True

        Catch ex As Exception
            Public_BeforeUpdate = False
            FpApp.DoErrorMsgBox("DB_Place.Form_BeforeUpdate", Err.Number, Err.Description)
        End Try
    End Function

    Function ChkDB_Params(Optional ByVal MitSetFocus As Boolean = True) As Boolean
        Dim OUT As Boolean = True

        If OUT Then
            If FpApp.DC.CNN_IsConnected = False Then
                OUT = False
            End If
        End If

        If OUT Then
            If Terminal = "" Then
                OUT = False
            ElseIf Terminal <> Trim(Terminal) Then
                OUT = False
            ElseIf Len(Terminal) > 10 Then
                OUT = False
            ElseIf FpApp.DC.DLookup("ID", "SEL_SYS_INSTALLED_TERMINALS", String.Format("Terminal='{0}' AND ComputerName='{1}' AND StartupPath='{2}'", Terminal, My.Computer.Name, Application.StartupPath), , 0) = 0 Then
                OUT = False
            End If
        End If

        Return OUT
    End Function
    Function getOpenTyp() As Integer
        getOpenTyp = Val(FPApp.Text_getParamFromLine(Nz(Me.OpenArgs, ""), 1, , , "|"))
    End Function

    Private Sub Sel_DB_Place_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        FpApp.Text_get_From_Resource("FP_DB_PLACE", 1, SQLServer_Label.Text)
        FPApp.Text_get_From_Resource("FP_DB_PLACE", 2, DbName_Label.Text)
        FPApp.Text_get_From_Resource("FP_DB_PLACE", 3, IntegratedSecurity.Text)
        FPApp.Text_get_From_Resource("FP_DB_PLACE", 4, UserId_Label.Text)
        FPApp.Text_get_From_Resource("FP_DB_PLACE", 5, Password_Label.Text)
        'FPApp.Text_get_From_Resource("FP_DB_PLACE", 6, btnTest.Text)
        'FPApp.Text_get_From_Resource("FP_DB_PLACE", 7, btnOK.Text)
        'FpApp.Text_get_From_Resource("FP_DB_PLACE", 8, HELP_Label.Text)
        FpApp.Text_get_From_Resource("FP_DB_PLACE", 9, Pfd_Terminal_Label.Text)
        FpApp.Text_get_From_Resource("FP_DB_PLACE", 10, ComputerName_Label.Text)
        FpApp.Text_get_From_Resource("FP_DB_PLACE", 11, StartupPath_Label.Text)
        FpApp.Text_get_From_Resource("FP_DB_PLACE", 12, Title_Label.Text)
        FpApp.Text_get_From_Resource("FP_DB_PLACE", 13, Title_SELEXPED_Label.Text)
        FpApp.Text_get_From_Resource("FP_DB_PLACE", 14, Me.Text)
        FpApp.Text_get_From_Resource("LINK_INSTALL_GUIDE", 1, Link_Install_Guide)

        Dim INIConnectstring As String = ""

        FPApp.PFDlesen("CONNECT_VB", INIConnectstring)
        If INIConnectstring > "" Then
            With DC
                SQLServer.Text = .ConnectString_getSQLServerNameFromConnectString(INIConnectstring)
                DbName.Text = .ConnectString_getDBNameFromConnectString(INIConnectstring)
                IntegratedSecurity.Checked = .ConnectString_getIntegratedSecurity(INIConnectstring)
                UserId.Text = .ConnectString_getUserIdFromConnectString(INIConnectstring)
                Password.Text = .ConnectString_getPasswordFromConnectString(INIConnectstring)
            End With
        End If

        FpApp.PFDlesen("TERMINAL", Terminal)
        Pfd_Terminal.Text = UCase(Trim(Terminal))

        'ARRANGE
        Dim FPF_for_Arrange As New FP_Form("CONNECT_TO_DB", FpApp, Me, False)
        FPF_for_Arrange.Location_Save_On_Close = False

        With FPF_for_Arrange
            .SIZE_WIDTH_TO(WB, Me, , , 50)
            .SIZE_HEIGHT_TO(WB, Me)
            .ARRANGE_TOPS(WB, Me)
            .ARRANGE_RIGHTS(WB, Me)
            .ARRANGE_LEFTS(ButtonTest, Me)
            .ARRANGE_BOTTOMS(ButtonTest, Me)
            .ARRANGE_ON_RIGHT_TOP(ButtonInstalledTerminals, ButtonTest, 1)
            .ARRANGE_ON_LEFT(ButtonOK, WB)
            .ARRANGE_BOTTOMS(ButtonOK, Me)
            .ARRANGE_ON_LEFT_TOP(ButtonCancel, ButtonOK)
            Title_Label.Location = New Point(0, 0)
            .SIZE_WIDTH_BETWEEN(Title_Label, Me, WB)
            .ARRANGE_ON_BOTTOM(SQLServer_Label, Title_Label, 1)
            .SIZE_WIDTH_TO(SQLServer_Label, Title_Label, , , 35)
            .ARRANGE_ON_RIGHT(SQLServer, SQLServer_Label)
            .SIZE_WIDTH_BETWEEN(SQLServer, SQLServer_Label, Title_Label, 0, 1)
            .ARRANGE_AS_NEXT_ROW(DbName_Label, SQLServer_Label, , , 1)
            .ARRANGE_AS_NEXT_ROW(DbName, SQLServer, , , 1)
            .ARRANGE_AS_NEXT_ROW(IntegratedSecurity, DbName_Label, DbName, , 1)
            .ARRANGE_AS_NEXT_ROW(UserPassword_Panel, IntegratedSecurity, , , 1)
            UserId_Label.Location = New Point(0, 0)
            .SIZE_SAME(UserId_Label, SQLServer_Label)
            .ARRANGE_ON_RIGHT_TOP(UserId, UserId_Label)
            .SIZE_SAME(UserId, SQLServer)
            .ARRANGE_AS_NEXT_ROW(Password_Label, UserId_Label, , , 1)
            .ARRANGE_AS_NEXT_ROW(Password, UserId, , , 1)
            .SIZE_HEIGHT_TO_MAX(UserPassword_Panel)
            .ARRANGE_AS_NEXT_ROW(Title_SELEXPED_Label, UserPassword_Panel, , , 1)
            .ARRANGE_ON_BOTTOM(Pfd_Terminal_Label, Title_SELEXPED_Label, 1)
            .SIZE_SAME(Pfd_Terminal_Label, SQLServer_Label)
            .ARRANGE_ON_RIGHT_TOP(Pfd_Terminal, Pfd_Terminal_Label)
            .SIZE_SAME(Pfd_Terminal, SQLServer)
            .ARRANGE_TOPS(ButtonGetTerminal, Pfd_Terminal)
            .ARRANGE_RIGHTS(ButtonGetTerminal, Pfd_Terminal)
            .ARRANGE_AS_NEXT_ROW(ComputerName_Label, Pfd_Terminal_Label, , , 1)
            .ARRANGE_AS_NEXT_ROW(ComputerName, Pfd_Terminal, , , 1)
            .ARRANGE_AS_NEXT_ROW(StartupPath_Label, ComputerName_Label, , , 1)
            .ARRANGE_AS_NEXT_ROW(StartupPath, ComputerName, , , 1)
        End With

        FPF_for_Arrange.Dispose()

        'END OF ARRANGE

        SET_LAYOUT()

    End Sub

    Function AlleKontakteErstellen(Optional ByVal mitDialog As Boolean = True) As Boolean
        On Error GoTo AlleKontakteErstellen_err

        Dim rst As DataSet
        Dim MyConnectADE$ = ""
        Dim w As Integer

        If (Not FpApp.DC.CNN_IsConnected) Then
            GoTo AlleKontakteErstellen_errEnd
        End If

        If Not IsCatalogSelExped(MyConnectADE$) Then
            GoTo AlleKontakteErstellen_errEnd
        End If

        AlleKontakteErstellen = True
        Exit Function

AlleKontakteErstellen_errEnd:
        AlleKontakteErstellen = False
        Exit Function

AlleKontakteErstellen_err:
        FpApp.DoErrorMsgBox("Form_DB_Place.AlleKontakteErstellen", Err.Number, Err.Description)
        GoTo AlleKontakteErstellen_errEnd
        Resume
    End Function
    Function IsCatalogSelExped(ByVal MyConnect$, Optional ByVal OnlyTheSameVersion As Boolean = True, Optional ByVal mitDialog As Boolean = True) As Boolean
        On Error GoTo IsCatalogSelExped_err

        Dim wOK As Boolean
        Dim DA As SqlDataAdapter
        Dim DT As New DataTable
        Dim MyTable$
        Dim MyID$
        Dim MySQL$ = ""
        Dim TbName As String = "TbName"

        wOK = True
        MyTable$ = "Alkalmazottak" : MyID$ = "ID" : Call IsCatalogSelExped_Chk(wOK, MyID$, MyTable$)
        MyTable$ = "Parameterek" : MyID$ = "ID" : Call IsCatalogSelExped_Chk(wOK, MyID$, MyTable$)

        If Not wOK Then
            If mitDialog Then
                MsgBox("This database art not a " & ProgramName$ & " database.", , ProgramName$)
                IsCatalogSelExped = False
                Exit Function
            End If
        End If

        '+++FPApp.SQL_CLOSE()
        MySQL = "SELECT * FROM Parameterek WHERE KulcsElotag='SEL' And Kulcs='VERSION'"
        DA = New SqlDataAdapter(MySQL, FpApp.DC.CNN)
        DA.Fill(DT)
        If DT.Rows.Count <> 1 Then
            If mitDialog Then
                MsgBox("I couldn't find out the database version number. I can't connect the database now.", , ProgramName$)
            End If
            GoTo IsCatalogSelExped_errEnd
        End If

        If Trim$(Nz(DT.Rows(0)!AlfaNumertek, "")) <> VERS Then
            If mitDialog Then
                MsgBox("I couldn't connect the database, because the database version number (" & Trim$(Nz(DT.Rows(0)!AlfaNumertek, "")) & ") and the program version number (" & VERS & ") are not the same", , ProgramName$)
            End If
            GoTo IsCatalogSelExped_errEnd
        End If

        IsCatalogSelExped = True

        Exit Function

IsCatalogSelExped_errEnd:
        IsCatalogSelExped = False
        Exit Function

IsCatalogSelExped_err:
        MsgBox("Form_DB_Place.IsCatalogSelExped error. Error Nr.: " & Str$(Err.Number) & vbCrLf & "Description: " & Err.Description, , ProgramName$)
        GoTo IsCatalogSelExped_errEnd
    End Function
    Public Sub IsCatalogSelExped_Chk(Optional ByRef wOK As Boolean = False, Optional ByRef MyID$ = "", Optional ByRef MyTable$ = "")
        Dim DT As New DataTable
        Dim MySQL As String = "SELECT TOP 1 " & MyID$ & " FROM " & MyTable$
        Dim DA As SqlDataAdapter
        Dim TbName As String = "TbName"

        If wOK Then
            On Error Resume Next

            '+++FPApp.SQL_CLOSE()
            DA = New SqlDataAdapter(MySQL, FpApp.DC.CNN)
            DA.Fill(DT)
            If Err.Number <> 0 Then
                wOK = False
            End If
        End If
    End Sub

    Private Sub HELP_Label_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Process.Start(Link_Install_Guide)
    End Sub

    Private Sub ButtonCancel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ButtonCancel.MouseUp
        Close()
    End Sub

    Private Sub ButtonOK_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ButtonOK.MouseUp
        Dim MyConnectString As String
        Dim CtrlPressed = My.Computer.Keyboard.CtrlKeyDown

        Try
            If Not Public_BeforeUpdate() Then
                Exit Sub
            End If

            MyConnectString = FpApp.DC.ConnectString_getConnectStringFromElements(Me.SQLServer.Text, Me.DbName.Text, Me.IntegratedSecurity.Checked, Me.UserId.Text, Me.Password.Text)

            If Not M_setConnectionFromString(MyConnectString, , , P_OnlySelexpedDB, False) Then
                Exit Sub
            End If

            If CtrlPressed = False Then
                Dim sqlComm As SqlCommand = FpApp.DC.CNN.CreateCommand

                With FpApp.DC
                    .Qdf_set_SP(sqlComm, "SEL_SYS_INSTALLED_TERMINALS_CHECK")
                    .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Pfd_Terminal.Text)
                    .Qdf_AddParameter(sqlComm, "@ComputerName", SqlDbType.NVarChar, , 25, ComputerName.Text)
                    .Qdf_AddParameter(sqlComm, "@StartupPath", SqlDbType.NVarChar, , 255, StartupPath.Text)

                    Try
                        If Not .Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG) Then
                            If sqlComm.Parameters("@RetValue").Value = 1 Then
                                Pfd_Terminal.Focus()
                                FpApp.DoMyMsgBox_From_Resources(9) 'Ez a terminál már foglalt, kérem adjon meg egy másik értéket.
                            End If
                            Exit Sub
                        End If

                    Catch ex As Exception
                        FpApp.DoErrorMsgBox("Form_FP_DB_Place.ButtonOK_MouseUp", Err.Number, Err.Description)
                        Exit Sub
                    End Try
                End With
            End If

            FpApp.PFDinsertOrUpdate("CONNECT_VB", MyConnectString)

            Terminal = Pfd_Terminal.Text
            FpApp.PFDinsertOrUpdate("TERMINAL", Terminal)
            FpApp.PFD_SAVE()

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            gl_Doit = True
            Me.Close()

        Catch ex As Exception
            FpApp.DoErrorMsgBox("DB_Place.btnOK_Click", Err.Number, Err.Description)
            Exit Sub
        End Try
    End Sub

    Private Sub ButtonTest_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ButtonTest.MouseUp
        Try
            If Public_BeforeUpdate() Then
                FpApp.DoMyMsgBox_From_Resources(2) 'A tesztek SIKERESEN befejezodtek
            End If
        Catch
            FpApp.DoErrorMsgBox("DB_Place.btnTest_Click", Err.Number, Err.Description)
        End Try
    End Sub

    Private Sub Pfd_Terminal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Pfd_Terminal.KeyPress
        If e.KeyChar = " " Then
            e.Handled = True
        Else
            FIELD_UCASE(Pfd_Terminal, e)
        End If
    End Sub

    Private Sub ButtonInstalledTerminals_MouseUp(sender As Object, e As MouseEventArgs) Handles ButtonInstalledTerminals.MouseUp
        If Public_BeforeUpdate() Then
            Dim MyConnectString As String = FpApp.DC.ConnectString_getConnectStringFromElements(Me.SQLServer.Text, Me.DbName.Text, Me.IntegratedSecurity.Checked, Me.UserId.Text, Me.Password.Text)

            If Not M_setConnectionFromString(MyConnectString, , , P_OnlySelexpedDB) Then
                Exit Sub
            End If

            TopMost = False
            FpApp.PFDinsertOrUpdate("LOCALDB", "OFF")

            Dim ListOfInstalledTerminals As New FP_Simple_Edit(FpApp, "INSTALLED_TERMINALS")
            FpApp.ShowDialogForm(ListOfInstalledTerminals)

            TopMost = True
            FpApp.PFDinsertOrUpdate("LOCALDB", "")
        End If
    End Sub

    Private Sub ButtonGetTerminal_MouseUp(sender As Object, e As MouseEventArgs) Handles ButtonGetTerminal.MouseUp
        Dim MyConnectString As String

        If Not Public_BeforeUpdate(False) Then
            Exit Sub
        End If

        MyConnectString = FpApp.DC.ConnectString_getConnectStringFromElements(Me.SQLServer.Text, Me.DbName.Text, Me.IntegratedSecurity.Checked, Me.UserId.Text, Me.Password.Text)

        If Not M_setConnectionFromString(MyConnectString, , , P_OnlySelexpedDB) Then
            Exit Sub
        End If

        Dim sqlComm As SqlCommand = FpApp.DC.CNN.CreateCommand

        With FpApp.DC
            .Qdf_set_SP(sqlComm, "SEL_SYS_INSTALLED_TERMINALS_GET_TERMINALNAME")
            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, ParameterDirection.Output, 10)

            Try
                If .Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_STANDARD) Then
                    Pfd_Terminal.Text = sqlComm.Parameters("@Terminal").Value
                End If

            Catch ex As Exception
                FpApp.DoErrorMsgBox("Form_FP_DB_Place.ButtonGetTerminal_MouseUp", Err.Number, Err.Description)
                Exit Sub
            End Try
        End With
    End Sub

End Class
