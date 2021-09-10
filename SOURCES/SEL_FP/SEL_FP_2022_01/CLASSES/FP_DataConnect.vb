Imports System.Data
Imports System.Data.SqlClient

Public Class FP_DataConnect
    Public Event CNN_INITIALISED(sender As FP_DataConnect)

    Protected Friend FPApp As FP_App
    ReadOnly Form_SQLConnect As FP_DB_Place
    Private Disposed As Boolean = False
    Private Initialised As Boolean = False
    Private DBVersionCheckOK As Boolean = False

    Public LocalDB_SEL As FP_LocalDB_SEL

    Public Sub New(ByVal MyFPApp As FP_App)
        FPApp = MyFPApp
        Form_SQLConnect = New FP_DB_Place(Me, , , False)
    End Sub

    Public ReadOnly Property P_Initialised As Boolean
        Get
            Return Initialised
        End Get
    End Property

    Public ReadOnly Property P_DBVersion_Is_OK As Boolean
        Get
            Return DBVersionCheckOK
        End Get
    End Property

    Public Sub Dispose()
        Disposed = True
    End Sub

    Public ReadOnly Property CNN() As SqlClient.SqlConnection
        Get
            CNN = Form_SQLConnect.P_Connection
        End Get
    End Property

    Public ReadOnly Property CNN_IsConnected() As Boolean
        Get
            CNN_IsConnected = Form_SQLConnect.P_IsConnected
        End Get
    End Property

    Public ReadOnly Property P_USE_LocalDB As Boolean
        Get
            Dim LOCALDB_Params As String = ""

            FPApp.PFDlesen("LOCALDB", LOCALDB_Params)
            Return LOCALDB_Params <> "OFF" And FPApp.Is_DEBUG_MODE = False
        End Get
    End Property

    Public Function CNN_INIT(Optional ByVal AllwaysOpenFormWhenShiftPressed As Boolean = False, Optional VersionCheck As Boolean = True) As Boolean
        Dim OUT As Boolean = False

        If Initialised Then
            OUT = True
        Else
            Dim ShiftPressed As Boolean = My.Computer.Keyboard.ShiftKeyDown

            VERS_LOCAL_DB = Val(FPApp.Text_getTextFile(FPApp.EXE_Dir + "\instance.ini"))

            FPApp.PFDlesen("TERMINAL", Terminal)
            If ShiftPressed = False Then
                If Terminal = Nothing Then
                    OUT = False

                    OUT = Form_SQLConnect.M_setConnectionFromINI(False)
                    If OUT = True Then
                        Dim sqlComm As SqlCommand = gl_FPApp.DC.CNN.CreateCommand

                        With gl_FPApp.DC
                            .Qdf_set_SP(sqlComm, "SEL_SYS_INSTALLED_TERMINALS_GET_TERMINALNAME")
                            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, ParameterDirection.Output, 10)

                            Try
                                If .Qdf_Execute("", sqlComm) Then
                                    Terminal = sqlComm.Parameters("@Terminal").Value

                                    If Terminal IsNot Nothing Then
                                        FPApp.PFDinsertOrUpdate("TERMINAL", Terminal)

                                        OUT = True
                                    End If
                                End If

                            Catch ex As Exception
                                OUT = False
                                FPApp.DoErrorMsgBox("FP_DataConnect.CNN_INIT", Err.Number, Err.Description)
                            End Try
                        End With

                        If OUT = True Then
                            OUT = False

                            sqlComm = FPApp.DC.CNN.CreateCommand

                            With FPApp.DC
                                .Qdf_set_SP(sqlComm, "SEL_SYS_INSTALLED_TERMINALS_CHECK")
                                .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                                .Qdf_AddParameter(sqlComm, "@ComputerName", SqlDbType.NVarChar, , 25, My.Computer.Name)
                                .Qdf_AddParameter(sqlComm, "@StartupPath", SqlDbType.NVarChar, , 255, Application.StartupPath)

                                Try
                                    If .Qdf_Execute("", sqlComm) Then
                                        OUT = True
                                    End If

                                Catch ex As Exception
                                    OUT = False
                                    FPApp.DoErrorMsgBox("FP_DataConnect.CNN_INIT", Err.Number, Err.Description)
                                End Try
                            End With
                        End If
                    End If
                End If
            End If

            If (AllwaysOpenFormWhenShiftPressed = False Or ShiftPressed = False) Then
                OUT = Form_SQLConnect.M_setConnectionFromINI(False, , VersionCheck)
                If OUT = True Then
                    If P_DBVersion_Is_OK = True Then
                        Dim MySQL As String = String.Format("SELECT ID FROM SEL_SYS_INSTALLED_TERMINALS WHERE Terminal='{0}' AND ComputerName='{1}' AND StartupPath='{2}'", Terminal, My.Computer.Name, Application.StartupPath)

                        Dim DRow As DataRow = Qdf_get_DataRow(MySQL)

                        If DRow Is Nothing Then
                            'If DLookup("ID", "SEL_SYS_INSTALLED_TERMINALS", String.Format("Terminal='{0}' AND ComputerName='{1}' AND StartupPath='{2}'", Terminal, My.Computer.Name, Application.StartupPath), , 0) = 0 Then
                            OUT = False
                        End If
                    End If
                End If
            End If

            If OUT = False Then
                OUT = CNN_SET_WITH_FORM()
            End If

            If OUT Then
                Initialised = True

                FPApp.PFD_LOAD_FROM_DB()

                If FPApp.DC.P_USE_LocalDB Then
                    LocalDB_SEL = New FP_LocalDB_SEL(Me)
                End If

                If P_DBVersion_Is_OK Then
                    RaiseEvent CNN_INITIALISED(Me)
                End If
            End If
        End If

        CNN_INIT = OUT
    End Function

    Public Function CNN_RE_INIT() As Boolean
        Dim OUT As Boolean

        If Not (LocalDB_SEL Is Nothing) Then
            LocalDB_SEL.Dispose()
            LocalDB_SEL = Nothing
        End If

        Initialised = False

        OUT = CNN_INIT()

        Return OUT
    End Function

    Public Function CNN_SET_WITH_FORM() As Boolean
        Return (Form_SQLConnect.ShowDialog = DialogResult.OK)
    End Function
    Public Function CNN_OPEN(Optional WithDialog As Boolean = True) As Boolean
        Return Form_SQLConnect.M_OpenConnection(WithDialog)
    End Function
#Region "ConnectString"
    Private Function ConnectString_getParam(ByVal MyConnectString As String, ByVal MyParam As String) As String
        Dim OUT As String = String.Empty
        Dim AktConnectStringParam As String
        Dim AktConnectStringParam_Name As String
        Dim AktConnectStringParam_Value As String
        Dim w As Long = 0
        Dim ACTION_VALID As Boolean = True
        Do
            w += 1
            AktConnectStringParam = FPApp.Text_getParamFromLine(MyConnectString, w, , , ";")
            AktConnectStringParam_Name = FPApp.Text_getParamFromLine(AktConnectStringParam$, 1, , , "=")
            AktConnectStringParam_Value = FPApp.Text_getParamFromLine(AktConnectStringParam$, 2, , , "=")
            If AktConnectStringParam_Name = MyParam Then
                OUT = AktConnectStringParam_Value
                ACTION_VALID = False
            End If
        Loop While AktConnectStringParam <> String.Empty And ACTION_VALID
        If ACTION_VALID Then
            OUT = String.Empty
        End If
        Return OUT
    End Function
    Public Function ConnectString_getSQLServerNameFromConnectString(ByVal MyConnectString As String) As String
        Return ConnectString_getParam(MyConnectString, "data source")
    End Function
    Public Function ConnectString_getDBNameFromConnectString(ByVal MyConnectString As String) As String
        Return ConnectString_getParam(MyConnectString, "initial catalog")
    End Function
    Public Function ConnectString_getUserIdFromConnectString(ByVal MyConnectString As String) As String
        Return ConnectString_getParam(MyConnectString, "user id")
    End Function
    Public Function ConnectString_getPasswordFromConnectString(ByVal MyConnectString As String) As String
        Return ConnectString_getParam(MyConnectString, "password")
    End Function
    Public Function ConnectString_getIntegratedSecurity(ByVal MyConnectString As String) As Boolean
        Return (ConnectString_getParam(MyConnectString, "Integrated Security") = "True")
    End Function
    Public Function ConnectString_getConnectStringFromElements(ByVal Cnn_SQLServer_Name As String, ByVal Cnn_DBName As String, Optional ByVal Cnn_IntegratedSecurity As Boolean = True, Optional ByVal Cnn_User As String = "", Optional ByVal Cnn_Password As String = "") As String
        Dim OUT As String

        If Cnn_IntegratedSecurity Then
            'OUT = String.Format("data source={0};initial catalog={1};Integrated Security=True;Min Pool Size=1;Max Pool Size=100; Connect Timeout = 3", Cnn_SQLServer_Name, Cnn_DBName)
            OUT = String.Format("data source={0};initial catalog={1};Integrated Security=True;", Cnn_SQLServer_Name, Cnn_DBName)
        Else
            'OUT = String.Format("data source={0};initial catalog={1};persist security info=True;user id={2};password={3};Min Pool Size=1;Max Pool Size=100; Connect Timeout = 3", Cnn_SQLServer_Name, Cnn_DBName, Cnn_User, Cnn_Password)
            OUT = String.Format("data source={0};initial catalog={1};persist security info=True;user id={2};password={3};", Cnn_SQLServer_Name, Cnn_DBName, Cnn_User, Cnn_Password)
        End If

        ConnectString_getConnectStringFromElements = OUT
    End Function
    Public Function ConnectString_Check(ByVal MyConnectString As String, Optional ByVal mitDialog As Boolean = True, Optional ByVal DialNum As Long = 0, Optional ByVal OnlySelexpedDB As Boolean = True, Optional VersionCheck As Boolean = True) As Boolean
        Dim OUT As Boolean = False
        Dim sqlCnn As SqlClient.SqlConnection
        Dim sqlComm As SqlClient.SqlCommand
        Dim sqlreader As SqlClient.SqlDataReader
        Dim DBVersion As String
        Dim Old_UseWaitCursor As Boolean
        Try
            If Trim(MyConnectString) <> String.Empty Then
                Old_UseWaitCursor = Application.UseWaitCursor
                Application.UseWaitCursor = True
                Try
                    sqlCnn = New SqlClient.SqlConnection(MyConnectString)
                    Using sqlCnn
                        sqlCnn.Open()
                        If OnlySelexpedDB Then
                            Try
                                sqlComm = New SqlClient.SqlCommand("SELECT AlfaNumErtek FROM Parameterek WITH (READUNCOMMITTED) WHERE Kulcs='VERSION'", sqlCnn)
                                Using sqlComm
                                    sqlreader = sqlComm.ExecuteReader()
                                    If sqlreader.Read() Then
                                        DBVersion = sqlreader.Item("AlfaNumErtek").ToString()
                                        If VersionCheck And DBVersion <> FP_Globals.VERS Then
                                            Application.UseWaitCursor = Old_UseWaitCursor
                                            If mitDialog Then
                                                Dim Params(2) As String

                                                Params(0) = DBVersion
                                                Params(1) = FP_Globals.VERS

                                                FPApp.DoMyMsgBox_From_Resources(1021, Params) 'Programs Versionsnumber <> Database Versionsnumber
                                            End If
                                        Else
                                            Application.UseWaitCursor = Old_UseWaitCursor
                                            If DBVersion = FP_Globals.VERS Then
                                                DBVersionCheckOK = True
                                            End If
                                            OUT = True
                                        End If
                                    Else
                                        Application.UseWaitCursor = Old_UseWaitCursor
                                        If mitDialog Then
                                            FPApp.DoMyMsgBox_From_Resources(1020) 'A megadott adatbazis nem Selexped adatbazis.
                                        End If
                                    End If
                                End Using
                            Catch ex As Exception
                                Application.UseWaitCursor = Old_UseWaitCursor
                                If mitDialog Then
                                    FPApp.DoMyMsgBox_From_Resources(1020) 'A megadott adatbazis nem Selexped adatbazis.
                                End If
                            End Try
                        Else
                            Application.UseWaitCursor = Old_UseWaitCursor
                            OUT = True
                        End If
                    End Using
                Catch ex As Exception
                    Application.UseWaitCursor = Old_UseWaitCursor
                    If mitDialog Then
                        FPApp.DoMyMsgBox_From_Resources(1019) 'A megadott kapcsolatot nem tudtam megnyitni.
                    End If
                End Try
            Else
                OUT = False
                If mitDialog Then
                    FPApp.DoMyMsgBox_From_Resources(1018) 'Nincsenek megadva a kapcsolat parameterei.
                End If
            End If
        Catch ex As Exception
            Application.UseWaitCursor = Old_UseWaitCursor
            FPApp.DoErrorMsgBox("FP_DC.ConnectString_Check", Err.Number, Err.Description)
        End Try
        Return OUT
    End Function
#End Region

    Public Function SQLObjectExists(ByVal ObjectName As String, ByVal ObjectType As ENUM_SQLOBJTYPES) As Boolean
        Dim OUT As Boolean = False
        Dim DRow As DataRow
        Dim SelSQL As String
        Dim oType As String

        If CNN_OPEN() Then
            Try
                Select Case ObjectType
                    Case ENUM_SQLOBJTYPES.FN
                        oType = "FN"
                    Case ENUM_SQLOBJTYPES.P
                        oType = "P"
                    Case ENUM_SQLOBJTYPES.TR
                        oType = "TR"
                    Case ENUM_SQLOBJTYPES.U
                        oType = "U"
                    Case ENUM_SQLOBJTYPES.V
                        oType = "V"
                    Case Else
                        OUT = False
                End Select

                SelSQL = String.Format("SELECT * FROM sysobjects WHERE name = '{0}' AND xtype = '{1}'", ObjectName, ObjectType)
                DRow = FPApp.DC.Qdf_get_DataRow(SelSQL)
                If DRow Is Nothing Then
                    OUT = False
                Else
                    OUT = True
                End If

            Catch ex As Exception
                FPApp.DoErrorMsgBox("FP_DataConnect.SQLObjectExists", Err.Number, Err.Description)
                OUT = False
            End Try
        End If

        Return OUT
    End Function

#Region "Qdf_"
    Public Function Qdf_set_SP(ByRef sqlComm As SqlClient.SqlCommand, ByVal ProcName As String, Optional ByVal MyCommandTimeOut As Long = 30) As Boolean
        Dim OUT As Boolean = False
        Try
            sqlComm = New SqlClient.SqlCommand(ProcName, CNN) With {
                .CommandType = CommandType.StoredProcedure
                }
            If CNN_OPEN() Then
                sqlComm.CommandTimeout = MyCommandTimeOut
                If Qdf_AddParameter(sqlComm, "@RetValue", SqlDbType.Int, ParameterDirection.ReturnValue) Then
                    OUT = True
                Else
                    sqlComm = Nothing
                End If
            End If
        Catch ex As Exception
            FPApp.DoErrorMsgBox("FP_DataConnect.Qdf_set_SP", Err.Number, Err.Description)
        End Try
        Return OUT
    End Function
    Public Function Qdf_Execute(ByVal FPf As FP_Form, ByRef sqlComm As SqlClient.SqlCommand, Optional ByVal OKValue As Long = -1, Optional ByVal DialNum As ENUM_ERRDIAL_TYPE = ENUM_ERRDIAL_TYPE.DIALNUM_STANDARD, Optional ByVal ErrParamName As String = "") As Boolean
        Dim OUT As Boolean

        If Not (FPf Is Nothing) Then
            OUT = FPf.Qdf_Execute(sqlComm, OKValue, DialNum, ErrParamName)
        Else
            OUT = Qdf_Execute("", sqlComm, OKValue, DialNum, ErrParamName)
        End If

        Return OUT
    End Function

    Public Sub CHECK_EXECUTION_TIME(Transact_START As DateTime, ObjName As String)
        If ObjName <> "SEL_SYS_Error_LongTransacts_INSERT" Then
            Dim Transact_END = Now

            If DateDiff(DateInterval.Second, Transact_START, Transact_END) > 2 Then
                Dim sqlComm As SqlCommand = CNN.CreateCommand()
                Qdf_set_SP(sqlComm, "SEL_SYS_Error_LongTransacts_INSERT")
                Using sqlComm
                    Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                    Qdf_AddParameter(sqlComm, "@ObjName", SqlDbType.NVarChar, , -1, ObjName)
                    Qdf_AddParameter(sqlComm, "@CommandText", SqlDbType.NVarChar, , -1, "")
                    Qdf_AddParameter(sqlComm, "@Date_START", SqlDbType.DateTime, , , , Transact_START)
                    Qdf_AddParameter(sqlComm, "@Date_END", SqlDbType.DateTime, , , , Transact_END)

                    Try
                        Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG)
                    Catch ex As Exception
                        'Nothing to do
                    End Try
                End Using

                CURSOR_SHOW_DEFAULT()
            End If
        End If
    End Sub

    Public Sub CHECK_EXECUTION_TIME(Transact_START As DateTime, SqlComm_SP As SqlCommand)
        Dim Transact_END = Now

        If DateDiff(DateInterval.Second, Transact_START, Transact_END) > 2 Then
            Dim ObjName As String = SqlComm_SP.CommandText
            If ObjName <> "SEL_SYS_Error_LongTransacts_INSERT" Then
                Dim sqlComm As SqlCommand = CNN.CreateCommand()
                Dim CommandText As String = Services.Qdf_Print_SP(SqlComm_SP)

                Qdf_set_SP(sqlComm, "SEL_SYS_Error_LongTransacts_INSERT")
                Using sqlComm
                    Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                    Qdf_AddParameter(sqlComm, "@ObjName", SqlDbType.NVarChar, , -1, ObjName)
                    Qdf_AddParameter(sqlComm, "@CommandText", SqlDbType.NVarChar, , -1, CommandText)
                    Qdf_AddParameter(sqlComm, "@Date_START", SqlDbType.DateTime, , , , Transact_START)
                    Qdf_AddParameter(sqlComm, "@Date_END", SqlDbType.DateTime, , , , Transact_END)
                    Try
                        Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG)
                    Catch ex As Exception
                        'Nothing to do
                    End Try
                End Using

                CURSOR_SHOW_DEFAULT()
            End If
        End If
    End Sub

    Public Function Qdf_Execute(ByVal ModuleIdentifier As String, ByRef sqlComm As SqlClient.SqlCommand, Optional ByVal OKValue As Long = -1, Optional ByVal DialNum As ENUM_ERRDIAL_TYPE = ENUM_ERRDIAL_TYPE.DIALNUM_STANDARD, Optional ByVal ErrParamName As String = "") As Boolean
        'Ha DialNr=0  (ERRDIAL_DIALNR_STANDARD) hiba eseten megjelenik: DoMyMsgBox(14, Qdf.CommandText"|"@RetValue"|"ErrText)
        'Ha DialNr=-1 (ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE), hiba eseten a dialogusablak szama a visszateresi ertek:
        '                                                               DoMyMsgBox(RetValue, Qdf.CommandText"|"ErrText)
        'Ha DialNr=-2, (ERRDIAL_NODIALOG) akkor nem jelenik meg dialogusablak.
        Dim OUT As Boolean = False
        Dim RetValue As Long
        Dim ErrText As String = String.Empty
        Dim CountOfTry As Integer = 2
        Dim ExecutedSuccessfully As Boolean = False
        Dim Transact_START As DateTime = Now

        Dim ExecLock As New Object

        CURSOR_SHOW_WAIT()

        SyncLock ExecLock
            Do While CountOfTry > 0 And ExecutedSuccessfully = False
                CountOfTry -= 1
                Try
                    CNN_OPEN()
                    sqlComm.ExecuteNonQuery()
                    ExecutedSuccessfully = True

                Catch ex As Exception
                    If CountOfTry = 0 Then
                        CURSOR_SHOW_DEFAULT()
                        FPApp.DoErrorMsgBox("FP_DataConnect.Qdf_Execute", Err.Number, Err.Description)
                    Else
                        If CNN.State = ConnectionState.Open Then
                            CNN.Close()
                            System.Threading.Thread.Sleep(200)
                            CNN_OPEN()
                            System.Threading.Thread.Sleep(200)
                        End If
                    End If
                End Try
            Loop
        End SyncLock

        CURSOR_SHOW_DEFAULT()

        If ExecutedSuccessfully Then
            CHECK_EXECUTION_TIME(Transact_START, sqlComm)

            RetValue = nz(sqlComm.Parameters("@RetValue").Value, 0)

            If RetValue = OKValue Then
                OUT = True
            Else
                If ErrParamName <> String.Empty Then
                    ErrText = nz(sqlComm.Parameters.Item(ErrParamName).Value, "")
                End If
                Select Case DialNum
                    Case ENUM_ERRDIAL_TYPE.DIALNUM_STANDARD : FPApp.DoMyMsgBox(14, String.Format("{0}|{1}|{2}", sqlComm.CommandText, RetValue, ErrText)) 'tarolt eljaras hibaval ter vissza
                    Case ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE : FPApp.DoMyMsgBox(RetValue, ErrText, , , , ModuleIdentifier)
                    Case ENUM_ERRDIAL_TYPE.NODIALOG 'not handled!
                    Case Else : FPApp.DoMyMsgBox(DialNum, ErrText, , , , ModuleIdentifier) 'A tarolt eljaras hibaval tert vissza
                End Select
            End If
        End If

        Return OUT
    End Function

    Public Function Qdf_AddParameter(ByRef SqlComm As SqlClient.SqlCommand, ByVal ParamName As String, ByVal ParamType As SqlDbType, Optional ByVal ParamDirection As ParameterDirection = ParameterDirection.Input, Optional ByVal ParamSize As Long = 0, Optional ByVal StrValue As String = "", Optional ByVal DatetimeValue As Date = NULLDATE, Optional ByVal IntValue As Long = 0, Optional ByVal FloatValue As Double = 0, Optional ByVal XmlValue As Xml.XmlDocument = Nothing, Optional ByVal BinaryValue As Byte() = Nothing) As Boolean
        Dim OUT As Boolean = False
        Dim p As New SqlClient.SqlParameter
        Dim LetNull As Boolean

        If StrValue Is Nothing Then
            StrValue = ""
        End If
        LetNull = (StrValue.ToUpper = "NULL" Or DatetimeValue = NULLDATE)

        Try
            p.Direction = ParamDirection
            p.ParameterName = ParamName
            p.SqlDbType = ParamType
            Select Case ParamType
                Case SqlDbType.Int, SqlDbType.SmallInt : p.Value = IntValue
                Case SqlDbType.Bit : p.Value = Math.Abs(IntValue)
                Case SqlDbType.Float : p.Value = FloatValue
                Case SqlDbType.DateTime : p.Value = IIf(LetNull, DBNull.Value, DatetimeValue)
                Case SqlDbType.VarChar : p.Value = StrValue : p.Size = ParamSize
                Case SqlDbType.NVarChar : p.Value = StrValue : p.Size = ParamSize
                Case SqlDbType.Xml : p.Value = StrValue : p.Size = ParamSize
                Case SqlDbType.Binary : p.Value = BinaryValue : p.Size = ParamSize
                Case SqlDbType.VarBinary : p.Value = BinaryValue : p.Size = ParamSize
            End Select
            SqlComm.Parameters.Add(p)
            OUT = True
        Catch ex As Exception
            FPApp.DoErrorMsgBox("FP_DataConnect.DC.Qdf_AddParameter", Err.Number, Err.Description)
        End Try
        Return OUT
    End Function
    Public Function Qdf_Fill_DT(ByVal MySQL As String, ByRef DT As DataTable, Optional ByVal Timeout As Integer = 0) As Boolean
        Dim OUT As Boolean = True

        Dim DA As New SqlDataAdapter
        Dim sqlComm As SqlCommand = CNN.CreateCommand
        Dim CountOfTry As Integer = 2
        Dim ExecutedSuccessfully As Boolean = False
        Dim Transact_START As DateTime = Now
        Dim ExecLock As New Object
        Using DA
            sqlComm.CommandText = MySQL
            sqlComm.CommandTimeout = Timeout

            DA.SelectCommand = sqlComm

            DT = New DataTable

            SyncLock ExecLock
                Do While CountOfTry > 0 And ExecutedSuccessfully = False
                    CountOfTry -= 1
                    Try
                        CNN_OPEN()
                        DA.Fill(DT)
                        ExecutedSuccessfully = True
                        OUT = True

                    Catch ex As Exception
                        If CountOfTry = 0 Then
                            OUT = False
                            FPApp.DoErrorMsgBox("FP_DataConnect.Qdf_Fill_DT", Err.Number, Err.Description)
                            MsgBox(MySQL)
                        Else
                            If CNN.State = ConnectionState.Open Then
                                CNN.Close()
                                System.Threading.Thread.Sleep(200)
                                CNN_OPEN()
                                System.Threading.Thread.Sleep(200)
                            End If
                        End If
                    End Try
                Loop
            End SyncLock
        End Using

        If OUT Then
            CHECK_EXECUTION_TIME(Transact_START, MySQL)
        End If

        Return OUT
    End Function
    Public Function Qdf_get_DataRow(MySQL As String, Optional TimeOut As Long = 30) As DataRow
        Dim OUT As DataRow = Nothing
        Dim DT As DataTable = Nothing

        Dim ExecLock As New Object

        SyncLock ExecLock
            CNN_OPEN()
            If Qdf_Fill_DT(MySQL, DT, TimeOut) Then
                If DT.Rows.Count > 0 Then
                    OUT = DT.Rows(0)
                End If
            End If
        End SyncLock

        Return OUT
    End Function
    Public Function Qdf_RunSQL(ByVal mySQL As String, Optional ByVal MyCommandTimeOut As Long = 30) As Boolean
        Dim OUT As Boolean = False
        Dim sqlComm As SqlClient.SqlCommand
        Dim CountOfTry As Integer = 2
        Dim ExecutedSuccessfully As Boolean = False
        Dim Transact_START As DateTime = Now

        Dim ExecLock As New Object

        SyncLock ExecLock
            If Trim(CNN.ConnectionString) > "" Then
                Do While CountOfTry > 0 And ExecutedSuccessfully = False
                    CountOfTry -= 1
                    Try
                        CNN_OPEN()
                        sqlComm = New SqlClient.SqlCommand(mySQL, CNN) With {
                            .CommandTimeout = MyCommandTimeOut,
                            .CommandType = CommandType.Text
                            }
                        Using sqlComm
                            sqlComm.ExecuteNonQuery()
                            ExecutedSuccessfully = True
                            OUT = True
                        End Using

                    Catch ex As Exception
                        If CountOfTry = 0 Then
                            FPApp.DoErrorMsgBox("FP_DataConnect.Qdf_RunSQL", Err.Number, Err.Description)
                        Else
                            If CNN.State = ConnectionState.Open Then
                                CNN.Close()
                                System.Threading.Thread.Sleep(200)
                                CNN_OPEN()
                                System.Threading.Thread.Sleep(200)
                            End If
                        End If
                    End Try
                Loop
            Else
                MsgBox("There is not valid connection with the database.")
            End If
        End SyncLock

        If OUT Then
            CHECK_EXECUTION_TIME(Transact_START, mySQL)
        End If

        Return OUT
    End Function
#End Region

    Function CreateSQLObject_DeleteAllDynamicObjects() As Boolean
        Dim OUT As Boolean = True

        If Not FPApp.InitGlobals Then
            OUT = False
        Else
            Dim ExecLock As New Object
            Dim DT As New DataTable
            Dim SQL As String = String.Format("SELECT name FROM sysobjects WHERE (name LIKE 'U_D_{0}%') AND (xtype = 'V')", Terminal)
            Dim DA As New SqlDataAdapter(SQL, CNN)
            Using DA
                SyncLock ExecLock
                    Try
                        CNN_OPEN()
                        DA.Fill(DT)

                        'Korabbi view torlese
                        For Each Row As DataRow In DT.Rows
                            Call Qdf_RunSQL("DROP VIEW " & Row!Name)
                        Next

                    Catch ex As Exception
                        OUT = False
                        FPApp.DoErrorMsgBox("FP_DataConnect.CreateSQLObject_DeleteAllDynamicObjects", Err.Number, Err.Description)
                    End Try
                End SyncLock
            End Using

            CreateSQLObject_DeleteAllDynamicObjects = OUT

            Exit Function
        End If

        CreateSQLObject_DeleteAllDynamicObjects = OUT
    End Function

    Public Function DLookup(DT As DataTable, Criteria As String, FieldName As String, ByRef OUT_Value As Object) As Boolean
        Dim OUT As Boolean = False

        If Not (DT Is Nothing) Then
            If DT.Select(Criteria).Count > 0 Then
                OUT_Value = DT.Select(Criteria).First.Item(FieldName)
            End If
        End If

        Return OUT
    End Function

    Public Function DLookup(ByVal Expr As String, ByVal MyDomain As String, ByVal Criteria As String, Optional ByVal AliasName As String = "", Optional ByVal NullValue As Object = Nothing) As Object
        Dim MySQL As String = ""
        Dim Transact_START As DateTime = Now

        Try

            If InStr(Trim(MyDomain), " ") > 0 Then
                MySQL = "SELECT " + Expr + " FROM " + AliasName + " WHERE " + Criteria + " AND EXISTS (" + MyDomain + ")"
            Else
                MySQL = "SELECT TOP 1 " + Expr + " FROM " + MyDomain + IIf(Trim(Criteria) = "", "", " WHERE " + Criteria)
            End If
            If Not CNN_OPEN() Then
                DLookup = ""
                Exit Function
            End If

            Dim DRow As DataRow = Qdf_get_DataRow(MySQL)

            If DRow Is Nothing Then
                If Not IsNothing(NullValue) Then
                    DLookup = NullValue
                Else
                    DLookup = ""
                End If

                Exit Function
            End If

            If AliasName <> "" Then
                Try
                    DLookup = DRow.Item(AliasName).ToString
                Catch ex As Exception
                    DLookup = DRow.Item(Expr).ToString
                End Try

            Else
                DLookup = DRow.Item(Expr).ToString
            End If
            Exit Function

        Catch ex As Exception
            DLookup = ""
            FPApp.DoErrorMsgBox("FP_DataConnect.DLookup", Err.Number, Err.Description)
        End Try

        CHECK_EXECUTION_TIME(Transact_START, MySQL)
    End Function

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub
End Class
