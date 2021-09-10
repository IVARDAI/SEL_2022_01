Public Class SEL_BASE_DATA_ACCESS_RIGHT_PANEL
    Public Structure STRUCT_SEL_BASE_DATA_ACCESS_RIGHTS_PANEL
        Dim BASE_DATA_ACCESS_RIGHTS_Panel As Panel
        Dim Fieldprefix As String
        Dim Subprefix As String
        Dim Parent_FP As FP
        Dim Parent_Table_ID As Integer
    End Structure

    Private BASE_DATA_ACCESS_RIGHTS_Panel As Panel
    ReadOnly Control_Prefix = "BASE_DATA_ACCESS_RIGHTS_"
    ReadOnly FieldPrefix As String
    ReadOnly SubPrefix As String
    Private WithEvents Parent_FP As FP
    ReadOnly Parent_Table_ID As Integer

    Private FPf As FP_Form
    Private WithEvents FP_BASE_DATA_ACCESS_RIGHTS_PANEL As FP
    Private Disposed As Boolean = False

    Private Control_Btn_SELECT_ALL As PictureBox
    Private Control_List_of_MODULE_IDENTIFIERS As ListView
    Private Control_List_of_MODULE_IDENTIFIERS_Label As Label

    Private WithEvents FPc_List_of_MODULE_IDENTIFIERS As FP_Control
    Private WithEvents FPp_Btn_SELECT_ALL As FP_PictureBox

    Public Sub New(P As STRUCT_SEL_BASE_DATA_ACCESS_RIGHTS_PANEL)
        With P
            BASE_DATA_ACCESS_RIGHTS_Panel = .BASE_DATA_ACCESS_RIGHTS_Panel
            Parent_FP = .Parent_FP
            FieldPrefix = nz(P.Fieldprefix, "")
            SubPrefix = nz(P.Subprefix, "")
            Parent_Table_ID = .Parent_Table_ID
        End With

        FPf = Parent_FP.FPf

        FP_BASE_DATA_ACCESS_RIGHTS_PANEL = New FP(FPf, "BASE_DATA_ACCESS_RIGHTS_PANEL", SubPrefix, True)

        CREATE_CONTROLS()

        Dim FP_BASE_DATA_ACCESS_RIGHTS_P As New Struct_FP_CONTROLS_COLLECTION
        With FP_BASE_DATA_ACCESS_RIGHTS_P
            .FieldPrefix = Control_FieldPrefix_And_Prefix()
        End With
        FP_BASE_DATA_ACCESS_RIGHTS_PANEL.INIT_CONTROLS(FP_BASE_DATA_ACCESS_RIGHTS_P)
    End Sub

    Public Function LISTBOX_SAVE() As Boolean
        Dim OUT As Boolean = False
        Dim ChecedIDS As String = FPc_List_of_MODULE_IDENTIFIERS.LISTVIEW_GET_CHECKED_IDs_WITH_SEPARATOR("|")
        Dim SqlComm As SqlClient.SqlCommand = Nothing
        Using SqlComm
            gl_FPApp.DC.Qdf_set_SP(SqlComm, "BASE_DATA_ACCESS_RIGHTS_PANEL_SAVE")
            gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Parent_Record_ID", SqlDbType.Int, , , , , Parent_FP.P_DATA_Current_ID)
            gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Parent_Table_ID", SqlDbType.Int, , , , , Parent_Table_ID)
            gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@CheckList", SqlDbType.NVarChar, , -1, ChecedIDS)
            gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
            gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@ErrField", SqlDbType.NVarChar, ParameterDirection.Output, 255)

            If Not gl_FPApp.DC.Qdf_Execute(FPf, SqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE) Then
                Call gl_FPApp.DoErrorMsgBox("SEL_BASE_DATA_ACCESS_RIGHT_PANEL.LISTBOX_SAVE", Err.Number, Err.Description)
                Exit Function
            End If
        End Using

        Return OUT
    End Function

    Public Sub LISTBOX_SET_FROM_DB()
        Select Case Parent_FP.P_DATA_RecordStatus
            Case ENUM_RecordStatus.NORECORD
                FPc_List_of_MODULE_IDENTIFIERS.LISTVIEW_UNCHECK_ALL()

            Case ENUM_RecordStatus.NEWRECORD
                FPc_List_of_MODULE_IDENTIFIERS.LISTVIEW_CHECK_ALL()

            Case ENUM_RecordStatus.EXISTS
                Dim Checked_Modules_On_DB As String = Get_Checked_Modules_From_DB()
                FPc_List_of_MODULE_IDENTIFIERS.LISTVIEW_SET_CHECKED_IDs_FROM_DELIMITED_STRING(Checked_Modules_On_DB, "|")
                Dim ListOf_Checked_boxes As List(Of String) = Split(Checked_Modules_On_DB, "|").ToList
                For Each i As ListViewItem In Control_List_of_MODULE_IDENTIFIERS.Items
                    i.Checked = (ListOf_Checked_boxes.Contains(i.Name))
                Next
        End Select
    End Sub

    Public Sub Dispose_Me()
        If Disposed = False Then
            FP_BASE_DATA_ACCESS_RIGHTS_PANEL.Dispose()
            FP_BASE_DATA_ACCESS_RIGHTS_PANEL = Nothing

            FPf = Nothing

            BASE_DATA_ACCESS_RIGHTS_Panel = Nothing
            Parent_FP = Nothing

            If Not (Control_Btn_SELECT_ALL Is Nothing) Then
                Control_Btn_SELECT_ALL.Dispose()
                Control_Btn_SELECT_ALL = Nothing
            End If

            If Not (Control_List_of_MODULE_IDENTIFIERS Is Nothing) Then
                Control_List_of_MODULE_IDENTIFIERS.Dispose()
                Control_List_of_MODULE_IDENTIFIERS = Nothing
            End If

            If Not (Control_List_of_MODULE_IDENTIFIERS_Label Is Nothing) Then
                Control_List_of_MODULE_IDENTIFIERS_Label.Dispose()
                Control_List_of_MODULE_IDENTIFIERS_Label = Nothing
            End If
            Disposed = True
        End If
    End Sub

    Public Function Control_FieldPrefix_And_Prefix() As String
        Dim OUT As String = Control_Prefix

        If Fieldprefix > "" Then
            OUT = Fieldprefix + OUT
        End If

        Return OUT
    End Function

    Private Sub FP_BASE_DATA_ACCESS_RIGHTS_PANEL_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_BASE_DATA_ACCESS_RIGHTS_PANEL.CONTROLS_INITIALIZED
        With FP_BASE_DATA_ACCESS_RIGHTS_PANEL
            FPp_Btn_SELECT_ALL = .PICTUREBOXES("Btn_SELECT_ALL")

            FPc_List_of_MODULE_IDENTIFIERS = .CONTROLS("List_of_MODULE_IDENTIFIERS")
            With FPc_List_of_MODULE_IDENTIFIERS
                If .P.DT_FixText_Key = "" Then
                    .P.DT_FixText_Key = "@@VB_LISTVIEW_BASE_DATA_ACCESS_RIGHTS_MODULE_IDS"
                End If

                .DT_REFRESH()
            End With
        End With
    End Sub

    Protected Overrides Sub Finalize()
        Dispose_Me()
        MyBase.Finalize()
    End Sub

    Private Sub FPp_Btn_SELECT_ALL_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPp_Btn_SELECT_ALL.CLICK
        With FPc_List_of_MODULE_IDENTIFIERS
            If .LISTVIEW_IS_ALL_CHECKED Then
                .LISTVIEW_UNCHECK_ALL()
            Else
                .LISTVIEW_CHECK_ALL()
            End If
        End With
    End Sub

    Private Sub CREATE_CONTROLS()
        If Not Disposed Then
            Control_Btn_SELECT_ALL = New PictureBox
            With Control_Btn_SELECT_ALL
                .Name = Control_FieldPrefix_And_Prefix() + "Btn_SELECT_ALL"
                .Visible = True
                .Parent = BASE_DATA_ACCESS_RIGHTS_Panel
            End With
            BASE_DATA_ACCESS_RIGHTS_Panel.Controls.Add(Control_Btn_SELECT_ALL)
            FPf.CONTROLS_ADD(Control_Btn_SELECT_ALL)

            Control_List_of_MODULE_IDENTIFIERS = New ListView
            With Control_List_of_MODULE_IDENTIFIERS
                .Name = Control_FieldPrefix_And_Prefix() + "List_of_MODULE_IDENTIFIERS"
                .Visible = True
                .Parent = BASE_DATA_ACCESS_RIGHTS_Panel
            End With

            Control_List_of_MODULE_IDENTIFIERS_Label = New Label
            With Control_List_of_MODULE_IDENTIFIERS_Label
                .Name = Control_FieldPrefix_And_Prefix() + "List_of_MODULE_IDENTIFIERS_Label"
                .Visible = True
                .Parent = BASE_DATA_ACCESS_RIGHTS_Panel
            End With

            BASE_DATA_ACCESS_RIGHTS_Panel.Controls.Add(Control_List_of_MODULE_IDENTIFIERS)
            BASE_DATA_ACCESS_RIGHTS_Panel.Controls.Add(Control_List_of_MODULE_IDENTIFIERS_Label)

            FPf.CONTROLS_ADD(Control_List_of_MODULE_IDENTIFIERS)
            FPf.CONTROLS_ADD(Control_List_of_MODULE_IDENTIFIERS_Label)

        End If
    End Sub

    Private Sub FPc_List_of_MODULE_IDENTIFIERS_Field_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc_List_of_MODULE_IDENTIFIERS.Field_TextChanged
        If Not Parent_FP.FORM_DIRTY_SET Then
            Cancel = True
        End If
    End Sub

    Private Function Get_Checked_Modules_From_DB() As String
        Dim OUT As String = ""
        Dim SelectSQL As String = String.Format("SELECT dbo.Modul_IDS_To_BASE_Data({0},{1}) SelectedModules", Parent_FP.P_DATA_Current_ID, Parent_Table_ID)
        Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(SelectSQL)
        If Not DRow Is Nothing Then
            OUT = DRow.Item("SelectedModules")
        End If
        Return OUT
    End Function

    Private Sub Parent_FP_Form_BeforeUpdate(sender_FP As FP, ByRef Cancel As Integer) Handles Parent_FP.Form_BeforeUpdate
        If Parent_FP.P_FORM_Dirty Then
            LISTBOX_SAVE()
        End If
    End Sub

    Private Sub Parent_FP_Form_Current(sender_FP As FP) Handles Parent_FP.Form_Current
        LISTBOX_SET_FROM_DB()
    End Sub

    Private Sub Parent_FP_Form_NoRecord(sender_FP As FP) Handles Parent_FP.Form_NoRecord
        LISTBOX_SET_FROM_DB()
    End Sub
End Class
