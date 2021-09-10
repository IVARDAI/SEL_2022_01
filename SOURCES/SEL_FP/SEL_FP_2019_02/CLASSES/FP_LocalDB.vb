Imports System.Data.Sqlserverce
Imports System.Data
Imports System.Data.SqlClient

Public Class FP_LocalDB
    Public Event Event_CheckDB_Version(ByVal sender As FP_LocalDB, ByRef NotOK As Boolean)
    Public Event Event_CreateDB(ByVal sender As FP_LocalDB, ByRef Cancel As Boolean)

    Private CNN As SqlCeConnection

    Protected Friend DC As FP_DataConnect
    Private Disposed As Boolean = False
    Private DBFileName As String = ""
    Private CNN_Str As String = ""
    Private DB As SqlCeEngine
    Private DBExists As Boolean = False

    Sub New(MyDC As FP_DataConnect, Sdf_Name As String, Password As String)
        DC = MyDC
        DBFileName = String.Format("{0}\{1}.sdf", Application.StartupPath, Sdf_Name)
        CNN_Str = String.Format("Data Source={0}; Password={1};", DBFileName, Password)
        DB = New SqlCeEngine(CNN_Str)
        CNN = New SqlCeConnection(CNN_Str)
    End Sub

    Public Function CreateDB() As Boolean
        Dim OUT As Boolean = True

        If System.IO.File.Exists(DBFileName) Then
            DBExists = True

            Dim NotOK As Boolean = False

            RaiseEvent Event_CheckDB_Version(Me, NotOK)
            If NotOK Then
                System.IO.File.Delete(DBFileName)
                DBExists = False
            End If
        End If

        If DBExists = False Then
            DB.CreateDatabase()

            Dim Cancel As Boolean = False

            RaiseEvent Event_CreateDB(Me, Cancel)

            If Cancel Then
                System.IO.File.Delete(DBFileName)
            Else
                DBExists = True
            End If
        End If

        Return OUT
    End Function

    Public Sub Dispose()
        If Disposed = False Then
            If Not (DB Is Nothing) Then
                DB.Dispose()
                DB = Nothing
            End If

            Disposed = True
        End If
    End Sub

    Public Function TableExists(ByVal TableName As String) As Boolean
        Dim OUT As Boolean = False

        If DBExists Then
            Dim DRow As DataRow
            Dim SelSQL As String
            Dim oType As String = ""
            Try
                SelSQL = String.Format("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'", TableName)
                DRow = get_DataRow(SelSQL)
                If DRow Is Nothing Then
                    OUT = False
                Else
                    OUT = True
                End If

            Catch ex As Exception
                DC.FPApp.DoErrorMsgBox("FP_LocalDB.SQLObjectExists", Err.Number, Err.Description)
                OUT = False
            End Try
        End If

        Return OUT
    End Function

    Public Function Fill_DT(ByVal MySQL As String, ByRef DT As DataTable) As Boolean
        Dim OUT As Boolean = True

        If DBExists = False Then
            OUT = False
        End If

        If OUT Then
            Dim DA As New SqlCeDataAdapter()
            Dim sqlComm As SqlCeCommand = CNN.CreateCommand

            sqlComm.CommandText = MySQL
            sqlComm.CommandTimeout = 0

            DA.SelectCommand = sqlComm

            DT = New DataTable

            Try
                DA.Fill(DT)

            Catch ex As Exception
                OUT = False
                DC.FPApp.DoErrorMsgBox("FP_LocalDB.Fill_DT", Err.Number, Err.Description)
            End Try
        End If

        Return OUT
    End Function
    Public Function get_DataRow(MySQL As String) As DataRow
        Dim OUT As DataRow = Nothing

        If DBExists Then
            Dim DT As DataTable = Nothing

            If Fill_DT(MySQL, DT) Then
                If DT.Rows.Count > 0 Then
                    OUT = DT.Rows(0)
                End If
            End If
        End If

        Return OUT
    End Function
    Public Function RunSQL(ByVal mySQL As String) As Boolean
        Dim OUT As Boolean = False

        If DBExists Then
            Dim sqlComm As SqlCeCommand = CNN.CreateCommand

            CNN_OPEN()

            Try
                sqlComm.CommandText = mySQL
                'sqlComm.CommandTimeout not supported in SqlServer CE
                sqlComm.CommandType = CommandType.Text
                sqlComm.ExecuteNonQuery()
                OUT = True

            Catch ex As Exception
                DC.FPApp.DoErrorMsgBox("FP_LocalDB.RunSQL", Err.Number, Err.Description)
            End Try

        End If

        Return OUT
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

    Private Function CNN_OPEN() As Boolean
        Dim OUT As Boolean = True

        If CNN.State <> ConnectionState.Open Then
            Try
                CNN.Open()

            Catch ex As Exception
                OUT = False
                DC.FPApp.DoErrorMsgBox("FP_LocalDB.CNN_OPEN", Err.Number, Err.Description)
            End Try
        End If

        Return OUT
    End Function

    Public Sub CNN_CLOSE()
        If CNN.State <> ConnectionState.Closed Then
            Try
                CNN.Close()

            Catch ex As Exception
                'Nothing to do
            End Try
        End If
    End Sub

    Public Function Execute_Script(SQL_Script As String) As Boolean
        Dim OUT As Boolean = True

        Dim sqlComm As New SqlCeCommand(SQL_Script, CNN)

        Try
            CNN_OPEN()
            sqlComm.ExecuteNonQuery()

        Catch ex As Exception
            OUT = False
            DC.FPApp.DoErrorMsgBox("FP_LocalDB.Qdf_Execute_Script", Err.Number, Err.Description)
        End Try

        Return OUT
    End Function

    Public Function Execute_Script_with_GO(SQL_Script As String) As Boolean
        SQL_Script += vbCrLf

        Dim OUT As Boolean = True
        Dim Lines() As String = Split(SQL_Script, vbCrLf)
        Dim CurrentScript As String = ""
        Dim i As Integer = 0
        Dim InRemark As Boolean = False

        Do While i < UBound(Lines) And OUT = True
            If Trim(Lines(i)).ToUpper = "#END_BAT#" And InRemark = False Then
                If InRemark Then
                    OUT = False
                    DC.FPApp.DoErrorMsgBox("FP_LocalDB.Execute_Script_with_GO", 0, String.Format("line {0}: GO statement in REMARK!!!", i))
                Else
                    OUT = Execute_Script(CurrentScript)
                    CurrentScript = ""
                End If
            Else
                If Mid(Trim(Lines(i)), 1, 2) <> "--" Then
                    If InStr(Lines(i), "/*") > 0 Then
                        InRemark = True
                    End If
                    If InStr(Lines(i), "*/") > 0 Then
                        InRemark = False
                    End If

                    CurrentScript += Lines(i) + vbCrLf
                End If
            End If

            i += 1
        Loop

        Return OUT
    End Function

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub
End Class

Public Class FP_LocalDB_SEL
    Public WithEvents LocalDB As FP_LocalDB
    Private Disposed As Boolean = False

    Sub New(MyDC As FP_DataConnect)
        LocalDB = New FP_LocalDB(MyDC, "LocalDB", "9241")
        LocalDB.CreateDB()
    End Sub

    Public Sub Dispose()
        If Disposed = False Then
            LocalDB.Dispose()
            LocalDB = Nothing

            Disposed = True
        End If
    End Sub

    Public Sub CNN_CLOSE()
        LocalDB.CNN_CLOSE()
    End Sub

    Public Function get_Param(Code As String, ByRef OUT_StrParam As String, Optional ByRef OUT_IntParam As Long = 0) As Boolean
        Dim OUT As Boolean = False
        Dim SQL As String = String.Format("SELECT IntParam, StrParam FROM LDB_Params WHERE Code = '{0}'", Code)
        Dim DRow As DataRow = LocalDB.get_DataRow(SQL)

        If Not (DRow Is Nothing) Then
            With DRow
                OUT_StrParam = !StrParam
                OUT_IntParam = !IntParam
            End With
        End If

        Return OUT
    End Function

    Private Sub DB_Version_Chk(ByVal sender As FP_LocalDB, ByRef NotOK As Boolean) Handles LocalDB.Event_CheckDB_Version
        NotOK = False
        If VERS_LOCAL_DB <> -1 Then
            If Not LocalDB.TableExists("LDB_Params") Then
                NotOK = True
            Else
                Dim DBVers As Integer = -1

                get_Param("LOCAL_DB_VERS", "", DBVers)

                If DBVers <> VERS_LOCAL_DB Then
                    NotOK = True
                End If
            End If
        End If
    End Sub

    Function Fill_DT(ByVal MySQL As String, ByRef DT As DataTable) As Boolean
        Return LocalDB.Fill_DT(MySQL, DT)
    End Function

    Function get_DataRow(MySQL As String) As DataRow
        Return LocalDB.get_DataRow(MySQL)
    End Function

    Function RunSQL(ByVal mySQL As String) As Boolean
        Return LocalDB.RunSQL(mySQL)
    End Function

    Function DLookup(DT As DataTable, Criteria As String, FieldName As String, ByRef OUT_Value As Object) As Boolean
        Return LocalDB.DLookup(DT, Criteria, FieldName, OUT_Value)
    End Function

    Function Execute_Script(SQL_Script As String) As Boolean
        Return LocalDB.Execute_Script(SQL_Script)
    End Function

    Function Execute_Script_with_GO(SQL_Script As String) As Boolean
        Return LocalDB.Execute_Script_with_GO(SQL_Script)
    End Function

    Private Sub LocalDB_Event_CreateDB(sender As FP_LocalDB, ByRef Cancel As Boolean) Handles LocalDB.Event_CreateDB
        Execute_Script_with_GO(My.Resources.LocalDB_SQL_CREATEDB_SEL)

        Dim LocalDB_Data As DataTable = Nothing

        'NEW_DEVELOPMENT_PARAMS_JSON - LOCALDB_FILL_SEL_SYS_DB_FIELDPROPERTIES - nem tudtam berakni a feltetelt, mert itt meg nincs betoltve a get_general_params

        Dim sqlComm As SqlCommand = LocalDB.DC.CNN.CreateCommand()

        LocalDB.DC.Qdf_set_SP(sqlComm, "LOCALDB_FILL_SEL_SYS_DB_FIELDPROPERTIES", 0)

        CURSOR_SHOW_WAIT()
        If Not LocalDB.DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG) Then
            MsgBox("Nincs telepitve a LOCALDB_FILL_SEL_SYS_DB_FIELDPROPERTIES tarolt eljaras! Telepitsed!")
        End If
        CURSOR_SHOW_DEFAULT()

        LocalDB.DC.Qdf_Fill_DT("SELECT * FROM LocalDB_SEL_SETDATA", LocalDB_Data)

        For Each DRow As DataRow In LocalDB_Data.Rows
            Execute_Script(DRow!Script_Rows)
        Next

        Execute_Script(String.Format("UPDATE LDB_Params	SET IntParam = {0} WHERE Code = 'LOCAL_DB_VERS'", VERS_LOCAL_DB))
    End Sub
End Class

