Imports System.Data
Imports System.Data.SqlClient

Public Module SEL_DOCMAN_M
    Public Sub FPApp_MenuItem_Activated(ByVal sender As FP_MenuItem.Struct_FP_MenuItem_Params, ByRef Handled As Boolean)
        'Handles: "SEL_DOCMAN_DOCTYPES"

        If Not Handled Then
            Select Case sender.Action
                Case "SEL_DOCMAN_DOCTYPES"
                    Handled = True
                    If Not gl_FPApp.FORMS_BringToFront("SEL_DOCMAN_DOCTYPES") Then
                        Dim Frm As New SEL_DOCMAN_DOCTYPES
                        Frm.Show()
                    End If
            End Select
        End If
    End Sub

    Public Sub FPApp_Marker_Clicked(Clicked_FPc As FP_Control, Action_Code As String, ByRef Individual_Params As Object, ByRef Handled As Boolean)
        If Handled = False Then
            Select Case Action_Code
                Case "SEL_DOCMAN_DOCTYPES_DIALOG"
                    Handled = True
                    Marker_SEL_DOCMAN_DOCTYPES_Dialog(Clicked_FPc, Handled)
            End Select
        End If
    End Sub

    Public Sub Marker_SEL_DOCMAN_DOCTYPES_Dialog(Clicked_FPc As FP_Control, ByRef Handled As Boolean)
        Handled = True

        Dim frm As New SEL_DOCMAN_DOCTYPES
        Dim GOTO_RECORD As FP_L_FORM_GOTO_RECORD_ON_OPEN = Nothing

        If Clicked_FPc.P_VALUE <> 0 Then
            GOTO_RECORD = New FP_L_FORM_GOTO_RECORD_ON_OPEN(frm.FP_DocTypes, Clicked_FPc.P_VALUE, True)
        End If

        gl_FPApp.ShowDialogForm(frm, Clicked_FPc.FP.FPf)

        Clicked_FPc.DT_REFRESH()

        If Clicked_FPc.FP.GRID_EXISTS Then
            Clicked_FPc.FP.GRID.REFRESH()
        End If
    End Sub

    Public Function SEL_DOCMAN_DOC_SAVE_TO_DOCMAN(DOC_P As DOCMAN_Doc_Panel.Struct_DOCMAN_Doc_Params, Optional WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = True
        Dim DocData() As Byte = Nothing
        Dim PDF_DocData() As Byte = Nothing
        Dim Doc_Images_ID As Long = 0

        Dim FileName_without_Extension As String = getFileName_without_Extension(DOC_P.FileName_with_Path)

        If OUT Then
            If WithDialog Then
                OUT = (gl_FPApp.DoMyMsgBox(83005, FileName_without_Extension, "SEQ,YES", "SEQ,NO") = 1)
            End If
        End If

        If OUT Then
            Try
                Dim TimeStamp As String = Format(Now, "yyMMdd_HHmmss")

                OUT = gl_FPApp.ByteArray_getFile(DOC_P.FileName_with_Path, DocData)

            Catch ex As Exception
                OUT = False
            End Try

            If OUT = False Then
                If WithDialog Then
                    gl_FPApp.DoMyMsgBox(83004) 'A dokumentumot ido kozben toroltek vagy atmozgattak masik helyre.
                End If
            End If
        End If

        If OUT Then
            Dim sqlComm As SqlCommand = gl_FPApp.DC.CNN.CreateCommand()

            With gl_FPApp.DC
                .Qdf_set_SP(sqlComm, "FP_DOCMAN_Docs_Panel_SAVE")
                .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                .Qdf_AddParameter(sqlComm, "@ID", SqlDbType.Int, ParameterDirection.Output, , , , 0)
                .Qdf_AddParameter(sqlComm, "@OldTransactID", SqlDbType.Int, , , , , 0)

                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Images_ID", SqlDbType.Int, , , , , 0)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Types_ID", SqlDbType.Int, , , , , DOC_P.DOCMAN_Doc_Types_ID)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_RefNum", SqlDbType.NVarChar, , 50, DOC_P.DOCMAN_RefNum)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_CUST_ID", SqlDbType.Int, , , , , DOC_P.DOCMAN_CUST_ID)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_CUST_Name1", SqlDbType.NVarChar, , 50, DOC_P.DOCMAN_CUST_Name1)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Date", SqlDbType.DateTime, , , , DOC_P.DOCMAN_Doc_Date)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Scan_Date", SqlDbType.DateTime, , , , DOC_P.DOCMAN_Scan_Date)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Descr", SqlDbType.NVarChar, , 255, DOC_P.DOCMAN_Descr)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Status_ID", SqlDbType.Int, , , , , DOC_P.DOCMAN_Doc_Status_ID)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Security_Level", SqlDbType.Int, , , , , DOC_P.DOCMAN_Doc_Security_Level)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Parent_TableName", SqlDbType.NVarChar, , 50, DOC_P.DOCMAN_Parent_TableName)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Parent_Record_ID", SqlDbType.Int, , , , , DOC_P.DOCMAN_Parent_Record_ID)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Sent_date", SqlDbType.DateTime, , , , DOC_P.DOCMAN_Doc_Date)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Message", SqlDbType.NVarChar, , -1, DOC_P.DOCMAN_Message)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Origin", SqlDbType.NVarChar, , 255, DOC_P.DOCMAN_Origin)

                .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
                .Qdf_AddParameter(sqlComm, "@ErrField", SqlDbType.NVarChar, ParameterDirection.Output, 255)
            End With

            CURSOR_SHOW_WAIT()
            Try
                OUT = gl_FPApp.DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")

                Dim MySQL As String = String.Format("SELECT Doc_Images_ID FROM Doc_Parents WHERE ID = {0}", sqlComm.Parameters("@ID").Value)
                Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)

                Doc_Images_ID = DRow!Doc_Images_ID

            Catch ex As Exception
                OUT = False
                gl_FPApp.DoErrorMsgBox("DOCMAN_Doc_Panel.REPORT_SAVE_TO_DOCMAN", Err.Number, Err.Description)
            End Try
        End If

        If OUT Then
            OUT = SEL_DOCMAN_DOC_SAVE_TO_DOCMAN_SAVE_IMAGE(Doc_Images_ID, DOC_P.DOCMAN_Origin, DocData)
        End If

        CURSOR_SHOW_DEFAULT()

        Return OUT
    End Function

    Private Function SEL_DOCMAN_DOC_SAVE_TO_DOCMAN_SAVE_IMAGE(Doc_Images_ID As Long, Origin As String, DocData() As Byte) As Boolean
        Dim OUT As Boolean = False

        Dim sqlComm As SqlCommand = gl_FPApp.DC.CNN.CreateCommand

        With gl_FPApp.DC
            .Qdf_set_SP(sqlComm, "FP_DOCMAN_Docs_Panel_Manage_Doc")
            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)

            .Qdf_AddParameter(sqlComm, "@Doc_Images_ID", SqlDbType.Int, , , , , Doc_Images_ID)
            .Qdf_AddParameter(sqlComm, "@Doc_Pages_ID", SqlDbType.Int, , , , , 0)
            .Qdf_AddParameter(sqlComm, "@Manage_Type", SqlDbType.Int, , , , , 1)
            .Qdf_AddParameter(sqlComm, "@Origin", SqlDbType.NVarChar, , 255, Origin)
            .Qdf_AddParameter(sqlComm, "@ImageData", SqlDbType.VarBinary, , -1, , , , , , DocData)
            .Qdf_AddParameter(sqlComm, "@Barcode01", SqlDbType.NVarChar, , 50, "")

            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
            .Qdf_AddParameter(sqlComm, "@ErrField", SqlDbType.NVarChar, ParameterDirection.Output, 255)

        End With

        CURSOR_SHOW_WAIT()
        Try
            OUT = gl_FPApp.DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")

        Catch ex As Exception
            OUT = False
            gl_FPApp.DoErrorMsgBox("FP_Word.Doc_SAVE_TO_DOCMAN", Err.Number, Err.Description)
        End Try

        Return OUT
    End Function

End Module