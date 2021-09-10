Imports System.Data
Imports System.Data.SqlClient

Public Class FP_KeyGen
    Public WithEvents FPf As FP_Form = Nothing
    Public WithEvents FP_KG As FP = Nothing

    Public WithEvents FPp_Btn_OK As FP_PictureBox
    Public WithEvents FPp_Btn_Cancel As FP_PictureBox

    Public FPc_Product_ID As FP_Control = Nothing
    Public FPc_VersionNo As FP_Control = Nothing
    Public FPc_SerialNo As FP_Control = Nothing
    Public WithEvents FPc_Valid As FP_Control = Nothing
    Public WithEvents FPc_CountOfUsers As FP_Control = Nothing
    Public FPc_Start_Code As FP_Control = Nothing

    Private Sub KeyGen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FPf = New FP_Form("KEYGEN_BASE", gl_FPApp, Me, False)
        FPf.Location_Save_On_Close = False
        FP_KG = New FP(FPf, "KEYGEN", , True)
    End Sub

    Private Sub KeyGen_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        FPf.INIT_CONTROLS(Nothing)
        FP_KG.INIT_CONTROLS(Nothing)

        SET_FIELD_VALUES()
    End Sub

    Private Sub SET_FIELD_VALUES()
        Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()
        Dim Result As Boolean = False

        With gl_FPApp.DC
            .Qdf_set_SP(sqlComm, "KEYGEN_READ")
            .Qdf_AddParameter(sqlComm, "@Product_ID", SqlDbType.NVarChar, ParameterDirection.Output, -1)
            .Qdf_AddParameter(sqlComm, "@VersionNo", SqlDbType.NVarChar, ParameterDirection.Output, -1)
            .Qdf_AddParameter(sqlComm, "@Valid", SqlDbType.DateTime, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@CountOfUsers", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@SerialNo", SqlDbType.NVarChar, ParameterDirection.Output, -1)

            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
        End With

        CURSOR_SHOW_WAIT()
        Try
            Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
        Catch ex As Exception
            Result = False
            FPf.FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        If Result Then
            With sqlComm
                FPc_Product_ID.P_VALUE = .Parameters("@Product_ID").Value
                FPc_VersionNo.P_VALUE = .Parameters("@VersionNo").Value
                FPc_SerialNo.P_VALUE = .Parameters("@SerialNo").Value
                FPc_Valid.P_VALUE = .Parameters("@Valid").Value
                FPc_CountOfUsers.P_VALUE = .Parameters("@CountOfUsers").Value
                FPc_Start_Code.P_VALUE = ""
            End With

            FP_KG.COLORING_ALL()
        End If
    End Sub

    Private Function SET_NEW_CODE() As Boolean
        Dim OUT As Boolean = False
        Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

        With gl_FPApp.DC
            .Qdf_set_SP(sqlComm, "KEYGEN_SAVE")
            .Qdf_AddParameter(sqlComm, "@Valid", SqlDbType.DateTime, , , , FPc_Valid.P_VALUE)
            .Qdf_AddParameter(sqlComm, "@CountOfUsers", SqlDbType.Int, , , , , FPc_CountOfUsers.P_VALUE)
            .Qdf_AddParameter(sqlComm, "@SerialNo", SqlDbType.NVarChar, , -1, FPc_SerialNo.P_VALUE)
            .Qdf_AddParameter(sqlComm, "@Starter_Code", SqlDbType.NVarChar, , -1, FPc_Start_Code.P_VALUE)

            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
        End With

        CURSOR_SHOW_WAIT()
        Try
            OUT = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
        Catch ex As Exception
            OUT = False
            FPf.FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        Return OUT
    End Function

    Private Sub FPf_CONTROLS_INITIALIZED(ByVal sender_FPf As FP_Form) Handles FPf.CONTROLS_INITIALIZED
        With FPf
            FPp_Btn_OK = .PICTUREBOXES("Btn_OK")
            FPp_Btn_Cancel = .PICTUREBOXES("Btn_Cancel")
        End With
    End Sub

    Private Sub FPp_Btn_Cancel_CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FPp_Btn_Cancel.CLICK
        Dim DoIt As Boolean = True

        If gl_FPApp.License_Valid = False Then
            DoIt = (FPf.DoMyMsgBox(1401, , "SEQ,CANCEL", "SEQ,YES") = 2)
        End If

        If DoIt Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub FPp_Btn_OK_CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FPp_Btn_OK.CLICK
        Dim Cancel As Boolean = (FP_KG.FORM_CHK_FIELDS = False)

        If Cancel = False Then
            FPc_Valid_Field_BeforeUpdate(FPc_Valid, Cancel)
        End If

        If Cancel = False Then
            FPc_CountOfUsers_Field_BeforeUpdate(FPc_CountOfUsers, Cancel)
        End If

        If Cancel = False Then
            If SET_NEW_CODE() Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                FPf.DoMyMsgBox(1405) 'A regisztracio RENDBEN befejezodott
                Me.Close()
            End If
        End If
    End Sub

    Private Sub FP_KG_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_KG.CONTROLS_INITIALIZED
        With FP_KG
            FPc_Product_ID = .CONTROLS("Product_ID")
            FPc_VersionNo = .CONTROLS("VersionNo")
            FPc_SerialNo = .CONTROLS("SerialNo")
            FPc_Valid = .CONTROLS("Valid")
            FPc_CountOfUsers = .CONTROLS("CountOfUsers")
            FPc_Start_Code = .CONTROLS("Start_Code")
        End With
    End Sub

    Private Sub FPc_Valid_Field_BeforeUpdate(sender_FPc As FP_Control, ByRef Cancel As Integer) Handles FPc_Valid.Field_BeforeUpdate
        If FPc_Valid.P_VALUE < gl_FPApp.GET_SERVER_CURRENT_DATE Then
            Cancel = True
            FPf.FOCUS_ON_AT_THE_END(Valid, 1402) 'A kod lejarati datuma kisebb, mint a napi datum.
        End If
    End Sub

    Private Sub FPc_CountOfUsers_Field_BeforeUpdate(sender_FPc As FP_Control, ByRef Cancel As Integer) Handles FPc_CountOfUsers.Field_BeforeUpdate
        If FPc_CountOfUsers.P_VALUE > 500 Then
            Cancel = True
            FPf.FOCUS_ON_AT_THE_END(CountOfUsers, 1403) 'A felhasznalok szama nem lehet tobb, mint 500
        End If
    End Sub
End Class
