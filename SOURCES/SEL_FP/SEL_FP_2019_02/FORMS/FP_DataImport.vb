Imports System.Data
Imports System.Data.SqlClient

Public Class FP_DataImport
    Public Structure Struct_FP_DataImport_Params
        Dim DataImport_ServerObject_Prefix As String
        Dim DataImport_SubPrefix As String
        Dim DATA_FP As FP 'Az az FP, ami ala be kell kerulnie a rekordoknak. Lehet Nothing
        Dim STEP_1_ZDISPO_Identifier As String
        Dim STEP_2_StoredProcedure_CHK As String
        Dim STEP_3_StoredProcedure_LoadData As String
        Dim STEP_4_ZDISPO_Identifier As String
    End Structure

    Private Structure Struct_Excel_Field
        Dim RowNum As Integer
        Dim Column As String
        Dim Value As String
    End Structure

    Public Event Data_Records_Prepared(ByVal sender As FP_DataImport, ByRef Cancel As Boolean)
    Public Event Check_Data(ByVal sender As FP_DataImport)
    Public Event Import_Data(ByVal sender As FP_DataImport, ByRef Cancel As Boolean)

    Public FPf As FP_Form
    Public WithEvents FP_HEAD As FP 'Azok az adatok kerulnek bele, amelyek a fejreszben tarolodnak
    Public WithEvents FP_L As FP 'Az excel sorok kerulnek bele
    Public WithEvents FP_ERR As FP

    Public STEP_1_ZDISPO_Identifier As String
    Public STEP_2_StoredProcedure_CHK As String
    Public STEP_3_StoredProcedure_LoadData As String
    Public STEP_4_ZDISPO_Identifier As String
    Public DATA_FP As FP = Nothing

    Private WithEvents FPp_Btn_REFRESH As FP_PictureBox
    Public SIGN As FP_L_Field_Sign

    Private FPc_ERR_RowNum As FP_Control
    Private FPc_ERR_ColumnName As FP_Control
    Private FPc_ERR_HeaderFieldName As FP_Control

    Sub New(P As Struct_FP_DataImport_Params)
        InitializeComponent()

        With P
            DATA_FP = .DATA_FP
            STEP_1_ZDISPO_Identifier = .STEP_1_ZDISPO_Identifier
            STEP_2_StoredProcedure_CHK = .STEP_2_StoredProcedure_CHK
            STEP_3_StoredProcedure_LoadData = .STEP_3_StoredProcedure_LoadData
            STEP_4_ZDISPO_Identifier = .STEP_4_ZDISPO_Identifier
        End With

        FPf = New FP_Form("FP_DATAIMPORT_BASE", gl_FPApp, Me, False)
        FPf.Location_Save_On_Close = False
        FP_HEAD = New FP(FPf, P.DataImport_ServerObject_Prefix + "HEAD", P.DataImport_SubPrefix, True)
        FP_L = New FP(FPf, P.DataImport_ServerObject_Prefix, P.DataImport_SubPrefix)
        FP_ERR = New FP(FPf, "DATAIMPORT_ERRORS")
    End Sub

    Private Sub FP_DataImport_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        SIGN = New FP_L_Field_Sign(FPf, 1, Color.Red)

        Dim FPf_CONTROLS As New Struct_FP_FORM_CONTROLS_COLLECTION

        With FPf_CONTROLS
            .Btn_HELP = Btn_HLP

            FPf.INIT_CONTROLS(FPf_CONTROLS)
        End With

        With SplitContainer_MAIN
            .TabIndex = 100
            .TabStop = False
            With .Panel1
                .TabIndex = 1000
                .TabStop = False
            End With

            With .Panel2
                .TabIndex = 2000
                .TabStop = False
            End With
        End With
        Header_Panel.TabStop = False

        FP_HEAD.INIT_CONTROLS(Nothing)

        Dim FP_CONTROLS As New Struct_FP_CONTROLS_COLLECTION
        With FP_CONTROLS
            With .GRID
                .Label = GRID_Label
                .Btn_FooterVisible = GRID_Btn_FooterVisible
                .GRID = GRID
                .Footer_Panel = GRID_Panel
            End With
            FP_L.INIT_CONTROLS(FP_CONTROLS)
        End With

        Dim FP_ERR_CONTROLS As New Struct_FP_CONTROLS_COLLECTION
        With FP_ERR_CONTROLS
            .Btn_ExportToExcel = Btn_ExcelExport

            With .GRID
                .Label = GRID_ERR_Label
                .GRID = GRID_ERR
            End With
            With FP_ERR.SQL_BIND_Params
                .NameOf_DEL = ""
                .NameOf_SAVE = ""
            End With

            FP_ERR.INIT_CONTROLS(FP_ERR_CONTROLS)
        End With

        If Not FP_L.FORM_RECORDS_LOAD(String.Format("Terminal = '{0}'", Terminal)) Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Close()
        Else
            Header_Panel_SET_VALUES()

            Dim Cancel As Boolean = False
            RaiseEvent Data_Records_Prepared(Me, Cancel)

            If Cancel Then
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Close()
            Else
                CHECK()
                CURRENT_ERR_FOCUS()
            End If
        End If
    End Sub

    Private Function EXCEL_FIELD_Row_And_Column_Name_from_String(MyKoo As String, ByRef Excel_Field As Struct_Excel_Field) As Boolean
        Dim OUT As Boolean = True
        Dim Row_STR As String = ""
        Dim Column_STR As String = ""

        MyKoo = Trim(MyKoo)

        If MyKoo = "" Then
            Return False
        End If

        For p As Integer = 1 To Len(MyKoo)
            Dim Current_Char As String = Mid(MyKoo, p, 1)

            If InStr("0123456789", Current_Char) > 0 Then
                If Len(Column_STR) = 0 Then
                    Return False
                End If

                Row_STR += Current_Char
            ElseIf InStr("ABCDEFGHIJKLMNOPQRSTUVWXYZ", Current_Char) Then
                If Len(Row_STR) > 0 Then
                    Return False
                End If
                Column_STR += Current_Char
            Else
                Return False
            End If
        Next

        If OUT = True Then
            With Excel_Field
                .RowNum = Val(Row_STR)
                .Column = Column_STR
            End With
        End If

        Return OUT
    End Function

    Private Function EXCEL_FIELD_GET_VALUE(MyField As Struct_Excel_Field) As String
        Dim OUT As String = ""

        Dim MySQL As String = String.Format("SELECT dbo.Excel_Import_Data_GET_VALUE('{0}', {1}, '{2}') Field_Value", Terminal, MyField.RowNum, MyField.Column)
        Dim DataRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)

        If Not (DataRow Is Nothing) Then
            OUT = nz(DataRow!Field_Value, "")
        End If

        Return OUT
    End Function

    Private Function EXCEL_FIELD_SET_VALUE(MyField As Struct_Excel_Field) As Boolean
        Dim OUT As Boolean = True
        Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

        FPf.FPApp.DC.Qdf_set_SP(sqlComm, "Excel_Import_Data_SET_VALUE")
        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@RowNum", SqlDbType.Int, , , , , MyField.RowNum)
        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Column", SqlDbType.NVarChar, , 2, MyField.Column)
        FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@VALUE", SqlDbType.NVarChar, , -1, nz(MyField.Value, ""))

        CURSOR_SHOW_WAIT()
        Try
            OUT = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE)
        Catch ex As Exception
            OUT = False
            FPf.FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        Return OUT
    End Function

    Private Sub Header_Panel_SET_VALUES()
        For Each Key As String In FP_HEAD.CONTROLS.Keys
            Dim Current_FPc As FP_Control = FP_HEAD.CONTROLS(Key)
            Dim Current_Excel_Field As New Struct_Excel_Field

            With Current_FPc
                If EXCEL_FIELD_Row_And_Column_Name_from_String(.P.Tag, Current_Excel_Field) Then
                    Current_FPc.P_VALUE = EXCEL_FIELD_GET_VALUE(Current_Excel_Field)
                End If
            End With
        Next
    End Sub

    Private Sub Header_Panel_SAVE_VALUES()
        For Each Key As String In FP_HEAD.CONTROLS.Keys
            Dim Current_FPc As FP_Control = FP_HEAD.CONTROLS(Key)
            Dim Current_Excel_Field As New Struct_Excel_Field

            With Current_FPc
                If EXCEL_FIELD_Row_And_Column_Name_from_String(.P.Tag, Current_Excel_Field) Then
                    Current_Excel_Field.Value = Current_FPc.P_VALUE
                    EXCEL_FIELD_SET_VALUE(Current_Excel_Field)
                End If
            End With
        Next
    End Sub

    Private Sub FP_L_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP_L.CONTROLS_INITIALIZED
        FP_L.P_FORM_AllowAdditions = False

        FP_L.GRID.COLUMNS_Frozen = 2

        FPp_Btn_REFRESH = FPf.PICTUREBOXES("Btn_REFRESH")
    End Sub

    Sub CHECK()
        If FPf.SAVE_ALL Then
            Header_Panel_SAVE_VALUES()

            If STEP_2_StoredProcedure_CHK > "" Then
                Dim Result As Boolean = False
                Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

                FPf.FPApp.DC.Qdf_set_SP(sqlComm, STEP_2_StoredProcedure_CHK)
                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)

                If (DATA_FP Is Nothing) Then
                    FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Head_ID", SqlDbType.Int, , , , , 0)
                Else
                    FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Head_ID", SqlDbType.Int, , , , , DATA_FP.P_DATA_Current_ID)
                End If

                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)

                CURSOR_SHOW_WAIT()
                Try
                    Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
                Catch ex As Exception
                    Result = False
                    FPf.FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
                End Try

                CURSOR_SHOW_DEFAULT()
            Else
                RaiseEvent Check_Data(Me)
            End If

            Dim SubWHERE As String = String.Format("Terminal = '{0}'", Terminal)
            FP_ERR.FORM_RECORDS_LOAD(SubWHERE, , True)
            FP_L.FORM_DORESYNC(True)
        End If
    End Sub

    Private Sub FPp_Btn_REFRESH_CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FPp_Btn_REFRESH.CLICK
        CHECK()
        CURRENT_ERR_FOCUS()
    End Sub

    Private Sub FP_ERR_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP_ERR.CONTROLS_INITIALIZED
        With FP_ERR
            .P_FORM_AllowAdditions = False
            .P_FORM_AllowDeletions = False
            .P_FORM_AllowEdits = False

            FPc_ERR_RowNum = .CONTROLS("ERR_RowNum")
            FPc_ERR_ColumnName = .CONTROLS("ERR_ColumnName")
            FPc_ERR_HeaderFieldName = .CONTROLS("ERR_HeaderFieldName")
        End With
    End Sub

    Private Function CURRENT_ERR_GET_FPc() As FP_Control
        Dim OUT As FP_Control = Nothing

        If FP_ERR.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
            Dim GOTO_ID As Long = Val(FP_ERR.DATA_Field_getSavedValue("ERR_RowID"))
            If GOTO_ID > 0 Then
                FP_L.FORM_GOTO_RECORD_BY_ID(GOTO_ID)
            End If

            If FPc_ERR_HeaderFieldName.P_VALUE > "" Then
                OUT = FP_HEAD.CONTROLS_GET_FPc(FPc_ERR_HeaderFieldName.P_VALUE)
            Else
                If FPc_ERR_ColumnName.P_VALUE > "" Then
                    OUT = FP_L.CONTROLS_GET_FPc(FPc_ERR_ColumnName.P_VALUE)
                End If
            End If
        End If

        CURRENT_ERR_GET_FPc = OUT
    End Function

    Private Sub CURRENT_ERR_FOCUS()
        Dim FPc As FP_Control = CURRENT_ERR_GET_FPc()

        If Not (FPc Is Nothing) Then
            If FP_L.FORM_RECORDS_SAVE_CURRENT Then
                SIGN.HIDE()
                FPf.FOCUS_ON_AT_THE_END(FPc.c)
            End If
        End If
    End Sub

    Private Sub CURRENT_ERR_SIGN()
        Dim FPc As FP_Control = CURRENT_ERR_GET_FPc()

        If Not (FPc Is Nothing) Then
            If FP_L.FORM_RECORDS_SAVE_CURRENT Then
                SIGN.SHOW(FPc)
            End If
        End If
    End Sub

    Private Sub FP_ERR_Form_Current(ByVal sender_FP As FP) Handles FP_ERR.Form_Current
        CURRENT_ERR_SIGN()
    End Sub

    Private Sub FP_L_Form_AfterDelete(sender_FP As FP) Handles FP_L.Form_AfterDelete
        CHECK()
    End Sub

    Private Sub FP_L_Form_BeforeDelete(sender_FP As FP, ByRef Cancel As Integer) Handles FP_L.Form_BeforeDelete
        If FP_L.P_DATA_RecordCount = 1 Then
            Cancel = True
            FPf.DoMyMsgBox_AT_THE_END(1300) 'Nem torolheti ki az utolso adatsort is az adatimportbol
        End If
    End Sub

    Private Sub FP_L_Form_Field_Enter(ByVal FPc As FP_Control, ByRef Handled As Boolean) Handles FP_L.Form_Field_Enter
        SIGN.HIDE()
    End Sub

    Private Sub FP_ERR_Form_Field_Enter(ByVal FPc As FP_Control, ByRef Handled As Boolean) Handles FP_ERR.Form_Field_Enter
        CURRENT_ERR_SIGN()
    End Sub

    Private Sub FP_ERR_GRID_Row_DoubleClick(ByVal sender_FP As FP, ByRef Handled As Boolean) Handles FP_ERR.GRID_Row_DoubleClick
        CURRENT_ERR_FOCUS()
    End Sub

    Private Sub Btn_OK_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Btn_OK.MouseUp
        CHECK()
        If FP_ERR.RS_RecCount > 0 Then
            gl_FPApp.DoMyMsgBox(1302) 'Az adatimportot nem lehet vegrehajtani, mert meg vannak hibas tetelek.
        Else
            Dim Cancel As Boolean = False

            If STEP_3_StoredProcedure_LoadData > "" Then
                Dim Result As Boolean = False
                Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

                FPf.FPApp.DC.Qdf_set_SP(sqlComm, STEP_3_StoredProcedure_LoadData)
                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)

                If (DATA_FP Is Nothing) Then
                    FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Head_ID", SqlDbType.Int, , , , , 0)
                Else
                    FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Head_ID", SqlDbType.Int, , , , , DATA_FP.P_DATA_Current_ID)
                End If

                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@OUT_NewRecords_SubWHERE", SqlDbType.NVarChar, ParameterDirection.Output, -1) 'Ha nem uresen ter vissza, akkor az FPf-re beolvassa a rekordokat.

                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                FPf.FPApp.DC.Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)

                CURSOR_SHOW_WAIT()
                Try
                    Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
                Catch ex As Exception
                    Result = False
                    FPf.FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
                End Try

                CURSOR_SHOW_DEFAULT()

                If Result Then
                    If Not (DATA_FP Is Nothing) Then
                        Dim NewRecords_SubWHERE As String = nz(sqlComm.Parameters("@OUT_NewRecords_SubWHERE").Value, "")

                        If NewRecords_SubWHERE > "" Then
                            FPf.FPApp.DoFilter_Params_CLEAR(DATA_FP.DOFILTER_ReturnedParams, False)
                            DATA_FP.FORM_RECORDS_LOAD(NewRecords_SubWHERE, False, True, True)
                        End If
                    End If
                End If
            Else
                RaiseEvent Import_Data(Me, Cancel)
            End If

            If Not Cancel Then
                If STEP_4_ZDISPO_Identifier > "" Then
                    Dim ZDISPO_P As New Struct_ZDISPO_Params

                    gl_FPApp.ZDISPO(ZDISPO_P, Nothing)
                End If

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Sub NAVIGATION_EXIT()
        gl_Doit = False
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Btn_Cancel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Btn_Cancel.MouseUp
        If e.Button = MouseButtons.Left Then
            If gl_FPApp.DoMyMsgBox(1303, , "SEQ,NO", "SEQ,YES") = 2 Then
                NAVIGATION_EXIT()
            End If
        End If
    End Sub
End Class
