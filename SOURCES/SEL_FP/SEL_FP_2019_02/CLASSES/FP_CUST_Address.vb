Imports System.Data
Imports System.Data.SqlClient

Public Class FP_CUST_Address
    Private Disposed As Boolean = False
    Private FPApp As FP_App
    Public Cust_ID As Long
    Public Cust_Address_Type As ENUM_Cust_AddressTypes
    Public Name1 As String
    Public Name2 As String
    Public Country As String
    Public ZIP As String
    Public District As String
    Public State As String
    Public City As String
    Public Addr As String
    Public Addr_ps_type As String
    Public Addr_housenr As String
    Public Addr_building As String
    Public Addr_stairway As String
    Public Addr_floor As String
    Public Addr_door As String
    Public Addr_Data_Format As ENUM_Address_Data_Format
    Public Spec_Addr As String
    Public Contact As String
    Public Tel As String
    Public Fax As String
    Public Email As String
    Public Remarks As String

    Sub New(ByVal MyFPApp As FP_App)
        FPApp = MyFPApp
    End Sub

    Public Sub Dispose()
        Disposed = True
    End Sub

    Public Sub CLEAR()
        Cust_ID = 0
        Cust_Address_Type = ENUM_Cust_AddressTypes.Normal
        Name1 = ""
        Name2 = ""
        Country = ""
        ZIP = ""
        District = ""
        State = ""
        City = ""
        Addr = ""
        Addr_ps_type = ""
        Addr_housenr = ""
        Addr_building = ""
        Addr_stairway = ""
        Addr_floor = ""
        Addr_door = ""
        Addr_Data_Format = ENUM_Address_Data_Format.NORMAL
        Spec_Addr = ""
        Contact = ""
        Tel = ""
        Fax = ""
        Email = ""
        Remarks = ""
    End Sub

    Public Function LOAD_Detailed(ByVal MyCust_ID As Long, Optional ByVal MyCust_Address_Type As ENUM_Cust_AddressTypes = ENUM_Cust_AddressTypes.NORMAL, Optional Get_Address_If_Empty As Boolean = False) As Boolean
        Dim OUT As Boolean = False
        Dim sqlComm As SqlCommand = FPApp.DC.CNN.CreateCommand()
        Dim Result As Boolean

        CLEAR()

        With FPApp.DC
            .Qdf_set_SP(sqlComm, "get_CUST_Address_SEL_ADDRESS")
            .Qdf_AddParameter(sqlComm, "@CUST_ID", SqlDbType.Int, , , , , MyCust_ID)
            .Qdf_AddParameter(sqlComm, "@AddressType", SqlDbType.Int, , , , , MyCust_Address_Type)
            .Qdf_AddParameter(sqlComm, "@Get_Address_If_Empty", SqlDbType.Bit, , , , , Math.Abs(CInt(Get_Address_If_Empty)))

            .Qdf_AddParameter(sqlComm, "@OUTPUT_Found", SqlDbType.Bit, ParameterDirection.Output)

            .Qdf_AddParameter(sqlComm, "@OUTPUT_CUST_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_AddressType", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Name1", SqlDbType.NVarChar, ParameterDirection.Output, 100)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Name2", SqlDbType.NVarChar, ParameterDirection.Output, 50)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Country", SqlDbType.NVarChar, ParameterDirection.Output, 3)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_State", SqlDbType.NVarChar, ParameterDirection.Output, 10)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_ZIP", SqlDbType.NVarChar, ParameterDirection.Output, 12)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_District", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_City", SqlDbType.NVarChar, ParameterDirection.Output, 50)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr", SqlDbType.NVarChar, ParameterDirection.Output, 100)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_ps_type", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_housenr", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_building", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_stairway", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_floor", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_door", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_Data_Type", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Spec_Addr", SqlDbType.NVarChar, ParameterDirection.Output, -1)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Contact", SqlDbType.NVarChar, ParameterDirection.Output, 50)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Tel", SqlDbType.NVarChar, ParameterDirection.Output, 50)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Fax", SqlDbType.NVarChar, ParameterDirection.Output, 50)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Email", SqlDbType.NVarChar, ParameterDirection.Output, 50)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Remarks", SqlDbType.NVarChar, ParameterDirection.Output, -1)
        End With

        CURSOR_SHOW_WAIT()

        Try
            Result = FPApp.DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE)
        Catch ex As Exception
            Result = False
            FPApp.DoErrorMsgBox("OrderEntry.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        If Result Then
            If sqlComm.Parameters("@OUTPUT_Found").Value = True Then
                Cust_ID = sqlComm.Parameters("@OUTPUT_Cust_ID").Value
                Cust_Address_Type = MyCust_Address_Type
                Name1 = sqlComm.Parameters("@OUTPUT_Name1").Value
                Name2 = sqlComm.Parameters("@OUTPUT_Name2").Value
                Country = sqlComm.Parameters("@OUTPUT_Country").Value
                ZIP = sqlComm.Parameters("@OUTPUT_ZIP").Value
                District = sqlComm.Parameters("@OUTPUT_District").Value
                State = sqlComm.Parameters("@OUTPUT_State").Value
                City = sqlComm.Parameters("@OUTPUT_City").Value
                Addr = sqlComm.Parameters("@OUTPUT_Addr").Value
                Addr_ps_type = sqlComm.Parameters("@OUTPUT_Addr_ps_type").Value
                Addr_housenr = sqlComm.Parameters("@OUTPUT_Addr_housenr").Value
                Addr_building = sqlComm.Parameters("@OUTPUT_Addr_building").Value
                Addr_stairway = sqlComm.Parameters("@OUTPUT_Addr_stairway").Value
                Addr_floor = sqlComm.Parameters("@OUTPUT_Addr_floor").Value
                Addr_door = sqlComm.Parameters("@OUTPUT_Addr_door").Value
                Addr_Data_Format = sqlComm.Parameters("@OUTPUT_Addr_Data_Type").Value
                Spec_Addr = sqlComm.Parameters("@OUTPUT_Spec_Addr").Value
                Contact = sqlComm.Parameters("@OUTPUT_Contact").Value
                Tel = sqlComm.Parameters("@OUTPUT_Tel").Value
                Fax = sqlComm.Parameters("@OUTPUT_Fax").Value
                Email = sqlComm.Parameters("@OUTPUT_Email").Value
                Remarks = sqlComm.Parameters("@OUTPUT_Remarks").Value

                OUT = True
            End If
        End If

        Return OUT
    End Function

    Public Function LOAD(ByVal MyCust_ID As Long, Optional ByVal MyCust_Address_Type As ENUM_Cust_AddressTypes = ENUM_Cust_AddressTypes.Normal, Optional Get_Address_If_Empty As Boolean = False) As Boolean
        Dim OUT As Boolean = False
        Dim sqlComm As SqlCommand = FPApp.DC.CNN.CreateCommand()
        Dim Result As Boolean

        CLEAR()

        With FPApp.DC
            'ddd  .Qdf_set_SP(sqlComm, "get_CUST_Address_SEL_ADDRESS")
            .Qdf_set_SP(sqlComm, "get_CUST_Address")
            .Qdf_AddParameter(sqlComm, "@CUST_ID", SqlDbType.Int, , , , , MyCust_ID)
            .Qdf_AddParameter(sqlComm, "@AddressType", SqlDbType.Int, , , , , MyCust_Address_Type)
            .Qdf_AddParameter(sqlComm, "@Get_Address_If_Empty", SqlDbType.Bit, , , , , Math.Abs(CInt(Get_Address_If_Empty)))

            .Qdf_AddParameter(sqlComm, "@OUTPUT_Found", SqlDbType.Bit, ParameterDirection.Output)

            .Qdf_AddParameter(sqlComm, "@OUTPUT_CUST_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_AddressType", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Name1", SqlDbType.NVarChar, ParameterDirection.Output, 100)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Name2", SqlDbType.NVarChar, ParameterDirection.Output, 50)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Country", SqlDbType.NVarChar, ParameterDirection.Output, 3)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_State", SqlDbType.NVarChar, ParameterDirection.Output, 10)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_ZIP", SqlDbType.NVarChar, ParameterDirection.Output, 12)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_District", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_City", SqlDbType.NVarChar, ParameterDirection.Output, 50)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr", SqlDbType.NVarChar, ParameterDirection.Output, 100)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_ps_type", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_housenr", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_building", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_stairway", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_floor", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_door", SqlDbType.NVarChar, ParameterDirection.Output, 24)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Addr_Data_Type", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Spec_Addr", SqlDbType.NVarChar, ParameterDirection.Output, -1)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Contact", SqlDbType.NVarChar, ParameterDirection.Output, 50)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Tel", SqlDbType.NVarChar, ParameterDirection.Output, 50)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Fax", SqlDbType.NVarChar, ParameterDirection.Output, 50)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Email", SqlDbType.NVarChar, ParameterDirection.Output, 50)
            .Qdf_AddParameter(sqlComm, "@OUTPUT_Remarks", SqlDbType.NVarChar, ParameterDirection.Output, -1)
        End With

        CURSOR_SHOW_WAIT()

        Try
            Result = FPApp.DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE)
        Catch ex As Exception
            Result = False
            FPApp.DoErrorMsgBox("OrderEntry.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        If Result Then
            If sqlComm.Parameters("@OUTPUT_Found").Value = True Then
                Cust_ID = sqlComm.Parameters("@OUTPUT_Cust_ID").Value
                Cust_Address_Type = MyCust_Address_Type
                Name1 = sqlComm.Parameters("@OUTPUT_Name1").Value
                Name2 = sqlComm.Parameters("@OUTPUT_Name2").Value
                Country = sqlComm.Parameters("@OUTPUT_Country").Value
                ZIP = sqlComm.Parameters("@OUTPUT_ZIP").Value
                District = sqlComm.Parameters("@OUTPUT_District").Value
                State = sqlComm.Parameters("@OUTPUT_State").Value
                City = sqlComm.Parameters("@OUTPUT_City").Value
                Addr = sqlComm.Parameters("@OUTPUT_Addr").Value
                Addr_ps_type = sqlComm.Parameters("@OUTPUT_Addr_ps_type").Value
                Addr_housenr = sqlComm.Parameters("@OUTPUT_Addr_housenr").Value
                Addr_building = sqlComm.Parameters("@OUTPUT_Addr_building").Value
                Addr_stairway = sqlComm.Parameters("@OUTPUT_Addr_stairway").Value
                Addr_floor = sqlComm.Parameters("@OUTPUT_Addr_floor").Value
                Addr_door = sqlComm.Parameters("@OUTPUT_Addr_door").Value
                Addr_Data_Format = sqlComm.Parameters("@OUTPUT_Addr_Data_Type").Value
                Spec_Addr = sqlComm.Parameters("@OUTPUT_Spec_Addr").Value
                Contact = sqlComm.Parameters("@OUTPUT_Contact").Value
                Tel = sqlComm.Parameters("@OUTPUT_Tel").Value
                Fax = sqlComm.Parameters("@OUTPUT_Fax").Value
                Email = sqlComm.Parameters("@OUTPUT_Email").Value
                Remarks = sqlComm.Parameters("@OUTPUT_Remarks").Value

                OUT = True
            End If
        End If

        Return OUT
    End Function

    Function SET_ADDRESS_FIELDS(Optional ByVal c_Cust_ID As Control = Nothing, Optional ByVal c_Cust_Address_Type As Control = Nothing, Optional ByVal c_Name1 As Control = Nothing, Optional ByVal c_Name2 As Control = Nothing, Optional ByVal c_Country As Control = Nothing, Optional ByVal c_ZIP As Control = Nothing, Optional ByVal c_State As Control = Nothing, Optional ByVal c_City As Control = Nothing, Optional ByVal c_Addr As Control = Nothing, Optional ByVal c_Spec_Addr As Control = Nothing, Optional ByVal c_Contact As Control = Nothing, Optional ByVal c_Tel As Control = Nothing, Optional ByVal c_Fax As Control = Nothing, Optional ByVal c_Email As Control = Nothing, Optional ByVal c_Remarks As Control = Nothing) As Boolean

        Dim OUT As Boolean = False

        Try
            CONTROL_SET_VALUE(c_Cust_ID, Cust_ID, True)
            CONTROL_SET_VALUE(c_Cust_Address_Type, Cust_Address_Type, True)
            CONTROL_SET_VALUE(c_Name1, Name1, True)
            CONTROL_SET_VALUE(c_Name2, Name2, True)
            CONTROL_SET_VALUE(c_Country, Country, True)
            CONTROL_SET_VALUE(c_ZIP, ZIP, True)
            CONTROL_SET_VALUE(c_State, State, True)
            CONTROL_SET_VALUE(c_City, City, True)
            CONTROL_SET_VALUE(c_Addr, Addr, True)
            CONTROL_SET_VALUE(c_Spec_Addr, Spec_Addr, True)
            CONTROL_SET_VALUE(c_Contact, Contact, True)
            CONTROL_SET_VALUE(c_Tel, Tel, True)
            CONTROL_SET_VALUE(c_Fax, Fax, True)
            CONTROL_SET_VALUE(c_Email, Email, True)
            CONTROL_SET_VALUE(c_Remarks, Remarks, True)

            OUT = True

        Catch ex As Exception
            FPApp.DoErrorMsgBox("CUST_Address.SET_ADDRESS_FIELDS", Err.Number, Err.Description)
        End Try

        SET_ADDRESS_FIELDS = OUT
    End Function

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub
End Class
