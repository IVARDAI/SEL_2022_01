Imports System.Data
Imports System.Data.SqlClient

Public Module NATIONAL_SPEC_M
    Private Function CHECK_Address(ZIP As String, City As String, Addr As String, Addr_ps_type As String) As Boolean
        Dim OUT As Boolean = False

        If Len(Trim(nz(ZIP, ""))) >= 4 _
           And Trim(nz(City, "")) > "" _
           And Trim(nz(Addr, "")) > "" _
           And Trim(nz(Addr_ps_type, "")) > "" Then
            OUT = True
        End If

        Return OUT
    End Function

    Public Function CHECK_Address(Addr As FP_CUST_Address) As Boolean
        'Ellenorzi, hogy az atadott cim megfelel-e az eloirasoknak.
        Return CHECK_Address(Addr.ZIP, Addr.City, Addr.Addr, Addr.Addr_ps_type)
    End Function

    Public Function CHECK_Address(FZ_ID As Long) As Boolean
        'Ellenorzi, hogy az atadott cim megfelel-e az eloirasoknak.
        Dim OUT As Boolean = False
        Dim MySQL As String = String.Format("SELECT * FROM Cegek_FZID WHERE Cegek_ID = {0}", FZ_ID)
        Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)

        If Not (DRow Is Nothing) Then
            OUT = CHECK_Address(DRow!FZISZ, DRow!FZVaros, DRow!FZUtca, DRow!FZAddr_ps_type)
        End If

        Return OUT
    End Function

    Private Sub INV_UPDATE_WITH_NEW_CUST_DATA(INV_ID As Long)
        'Lefrissiti az osszes meg frissitheto szamlat a javitott parameterekkel az ID=@INV_ID szamlaban.
        'A meghivott eljaras azonban nem csak ebben, hanem a tobbi meg javithato szamlaban is javithatja a parametereket.
        'Konkretan ebben a verzioban a szamla szallitojanak es vevoinek adatait javitja.
        Dim sqlComm As SqlCommand = gl_FPApp.DC.CNN.CreateCommand()
        Dim Result As Boolean = False

        With gl_FPApp.DC
            gl_FPApp.DC.Qdf_set_SP(sqlComm, "NATIONAL_SPEC_UPDATE_NEW_INV_WITH_CUST_DATA")
            gl_FPApp.DC.Qdf_AddParameter(sqlComm, "@INV_ID", SqlDbType.Int, , , , , INV_ID)
        End With

        CURSOR_SHOW_WAIT()
        Try
            Result = gl_FPApp.DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
        Catch ex As Exception
            Result = False
            gl_FPApp.DoErrorMsgBox("NATIONAL_SPEC..NATIONAL_SPEC_M.INV_UPDATE_WITH_NEW_CUST_DATA", Err.Number, Err.Description)
        End Try
    End Sub

    Public Function CHECK_INV_PreparePrint(INV_ID As Long) As Boolean
        'Ellenorzi, hogy az ID = INV_ID szamla adatai alapjan nyomtathato-e
        Dim OUT As Boolean = False
        Dim MySQL As String = String.Format("SELECT SzallitoKod Vendor_ID, VevoKod Customer_ID, dbo.NATIONAL_SPEC_CHECK_INV({0}) Result FROM Szamlak WHERE ID = {0}", INV_ID)

        Dim DoIt As Boolean = True

        Do
            Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)

            If DRow Is Nothing Then
                DoIt = False
                gl_FPApp.DoErrorMsgBox("NATIONAL_SPEC_M.CHECK_INV_PreparePrint", 0, String.Format("Invoice not found (INV_ID = {0})", INV_ID))

            Else
                Dim Result = DRow!Result

                Select Case Result
                    Case -1
                        DoIt = False
                        OUT = True

                    Case 92001 'A szallito adatai hibasak
                        If gl_FPApp.DoMyMsgBox(92001, , "SEQ,EDIT_CUST", "SEQ,CANCEL") = 1 Then
                            gl_FPApp.CUST_DATA_EDIT_DIALOG(DRow!Vendor_ID)
                            INV_UPDATE_WITH_NEW_CUST_DATA(INV_ID)
                        Else
                            DoIt = False
                        End If

                    Case 92002 'A vevo adatai hibasak
                        If gl_FPApp.DoMyMsgBox(92002, , "SEQ,EDIT_CUST", "SEQ,CANCEL") = 1 Then
                            gl_FPApp.CUST_DATA_EDIT_DIALOG(DRow!Customer_ID)
                            INV_UPDATE_WITH_NEW_CUST_DATA(INV_ID)
                        Else
                            DoIt = False
                        End If

                    Case Else
                        DoIt = False
                        gl_FPApp.DoErrorMsgBox("NATIONAL_SPEC_M.CHECK_INV_PreparePrint", 0, String.Format("Unknown errorcode ({0})", Result))
                End Select
            End If
        Loop While DoIt = True

        Return OUT
    End Function

    Public Sub FP_ADDRESS_SET_LAYOUT(Inv_Type_ID As ENUM_INV_Types, FP_ADDRESS As FP)
        'Ezt az eljarast a SEL_Address form hivja meg az FP_ADDRESS_SET_LAYOUT eljarasbol.
        Dim FPc_ZIP As FP_Control = FP_ADDRESS.CONTROLS("ZIP")
        Dim FPc_State As FP_Control = FP_ADDRESS.CONTROLS("State")
        Dim FPc_City As FP_Control = FP_ADDRESS.CONTROLS("City")
        Dim FPc_Addr As FP_Control = FP_ADDRESS.CONTROLS("Addr")
        Dim FPc_Addr_ps_type As FP_Control = FP_ADDRESS.CONTROLS("Addr_ps_type")

        Dim IsMandatory As Boolean = (INV_Types_IsOutgoing(Inv_Type_ID))

        FPc_ZIP.P.Mandatory = IsMandatory
        FPc_City.P.Mandatory = IsMandatory
        FPc_Addr.P.Mandatory = IsMandatory
        FPc_Addr_ps_type.P.Mandatory = IsMandatory
    End Sub

    Public Sub FP_CUST_SET_LAYOUT(FP_CUST As FP)
        'Ezt az eljarast a SEL_CUST form hivja meg az FP_CUST_SET_LAYOUT eljarasbol.
        If FP_CUST.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            With FP_CUST
                Dim FPc_Customer_YN = .CONTROLS("Customer_YN")

                Dim FPc_ZIP = .CONTROLS("ZIP")
                Dim FPc_State = .CONTROLS("State")
                Dim FPc_City = .CONTROLS("City")
                Dim FPc_Addr = .CONTROLS("Addr")
                Dim FPc_Addr_ps_type = .CONTROLS("Addr_ps_type")
                Dim FPc_Addr_Type = .CONTROLS("Addr_Type")

                Dim FPc_Mail_ZIP = .CONTROLS("Mail_ZIP")
                Dim FPc_Mail_Addr_State = .CONTROLS("Mail_Addr_State")
                Dim FPc_Mail_City = .CONTROLS("Mail_City")
                Dim FPc_Mail_Addr = .CONTROLS("Mail_Addr")
                Dim FPc_Mail_Addr_ps_type = .CONTROLS("Mail_Addr_ps_type")
                Dim FPc_Mail_Addr_Type = .CONTROLS("Mail_Addr_Type")

                Dim FPc_Site_ZIP = .CONTROLS("Site_ZIP")
                Dim FPc_Site_Addr_State = .CONTROLS("Site_Addr_State")
                Dim FPc_Site_City = .CONTROLS("Site_City")
                Dim FPc_Site_Addr = .CONTROLS("Site_Addr")
                Dim FPc_Site_Addr_ps_type = .CONTROLS("Site_Addr_ps_type")
                Dim FPc_Site_Addr_Type = .CONTROLS("Site_Addr_Type")

                Dim FPc_InvoiceAddress_Type = .CONTROLS("InvoiceAddress_Type")
                Dim FPc_Inv_ZIP = .CONTROLS("Inv_ZIP")
                Dim FPc_Inv_Addr_State = .CONTROLS("Inv_Addr_State")
                Dim FPc_Inv_City = .CONTROLS("Inv_City")
                Dim FPc_Inv_Addr = .CONTROLS("Inv_Addr")
                Dim FPc_Inv_Addr_ps_type = .CONTROLS("Inv_Addr_ps_type")
                Dim FPc_Inv_Addr_Type = .CONTROLS("Inv_Addr_Type")

                Dim IsCustomer As Boolean = (FPc_Customer_YN.P_VALUE = True)
                Dim Inv_Addr_Type As ENUM_Cust_AddressTypes = FPc_InvoiceAddress_Type.P_VALUE

                Dim Addr_Mandatory = (IsCustomer And Inv_Addr_Type = ENUM_Cust_AddressTypes.NORMAL)
                Dim Mail_Addr_Mandatory = (IsCustomer And Inv_Addr_Type = ENUM_Cust_AddressTypes.MAIL)
                Dim Site_Addr_Mandatory = (IsCustomer And Inv_Addr_Type = ENUM_Cust_AddressTypes.SITE)
                Dim Inv_Addr_Mandatory = (IsCustomer And Inv_Addr_Type = ENUM_Cust_AddressTypes.INV)

                FPc_ZIP.P.Mandatory = Addr_Mandatory
                FPc_City.P.Mandatory = Addr_Mandatory
                FPc_Addr.P.Mandatory = Addr_Mandatory
                FPc_Addr_ps_type.P.Mandatory = Addr_Mandatory

                FPc_Mail_ZIP.P.Mandatory = Mail_Addr_Mandatory
                FPc_Mail_City.P.Mandatory = Mail_Addr_Mandatory
                FPc_Mail_Addr.P.Mandatory = Mail_Addr_Mandatory
                FPc_Mail_Addr_ps_type.P.Mandatory = Mail_Addr_Mandatory

                FPc_Site_ZIP.P.Mandatory = Site_Addr_Mandatory
                FPc_Site_City.P.Mandatory = Site_Addr_Mandatory
                FPc_Site_Addr.P.Mandatory = Site_Addr_Mandatory
                FPc_Site_Addr_ps_type.P.Mandatory = Site_Addr_Mandatory

                FPc_Inv_ZIP.P.Mandatory = Inv_Addr_Mandatory
                FPc_Inv_City.P.Mandatory = Inv_Addr_Mandatory
                FPc_Inv_Addr.P.Mandatory = Inv_Addr_Mandatory
                FPc_Inv_Addr_ps_type.P.Mandatory = Inv_Addr_Mandatory
            End With
        End If
    End Sub

    Public Function CHECK_Address_Owner_with_loop() As Boolean
        Dim OUT As Boolean = False

        Dim DoIt As Boolean = True

        Do
            OUT = CHECK_Address(gl_FPApp.P.Owner_Params.CUST_OWNER_ID)
            If OUT = True Then
                DoIt = False
            Else
                If gl_FPApp.DoMyMsgBox(92003, , "SEQ,EDIT_CUST", "SEQ,CANCEL") = 1 Then 'A sajat ceg adatai hibasak.
                    gl_FPApp.CUST_DATA_EDIT_DIALOG(gl_FPApp.P.Owner_Params.CUST_OWNER_ID)
                Else
                    DoIt = False
                End If
            End If
        Loop While (DoIt = True)

        Return OUT
    End Function

    Public Function CHECK_Address_with_loop(Inv_Type_ID As ENUM_INV_Types, FZID As Long) As Boolean
        Dim OUT As Boolean = False

        If FZID = 0 Then
            OUT = False
        Else
            Dim DoIt As Boolean = True

            If INV_Types_IsOutgoing(Inv_Type_ID) = False Then
                OUT = True
            Else
                Do
                    OUT = CHECK_Address(FZID)
                    If OUT = True Then
                        DoIt = False
                    Else
                        If gl_FPApp.DoMyMsgBox(92002, , "SEQ,EDIT_CUST", "SEQ,CANCEL") = 1 Then 'A vevo adatai hibasak.
                            gl_FPApp.CUST_DATA_EDIT_DIALOG(FZID)
                        Else
                            DoIt = False
                        End If
                    End If
                Loop While (DoIt = True)
            End If
        End If

        Return OUT
    End Function
    Public Structure Struct_Addresses_Elements
        Dim Addr As String
        Dim Spec_Addr As String
        Dim District As String
        Dim Addr_ps_type As String
        Dim Addr_housenr As String
        Dim Addr_building As String
        Dim Addr_stairway As String
        Dim Addr_floor As String
        Dim Addr_door As String
        Dim Lang As String
    End Structure

    Public Function Addresses_get_Full_Addr_from_Elements(Addr_params As Struct_Addresses_Elements) As String
        Dim MySQL As String = String.Format("SELECT dbo.FN_Address_get_Full_Addr_from_Elements('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}') Addr", Addr_params.Addr, Addr_params.Spec_Addr, Addr_params.District, Addr_params.Addr_ps_type, Addr_params.Addr_housenr, Addr_params.Addr_building, Addr_params.Addr_stairway, Addr_params.Addr_floor, Addr_params.Addr_door, Addr_params.Lang)
        Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)

        Addresses_get_Full_Addr_from_Elements = DRow!Addr
    End Function

End Module
