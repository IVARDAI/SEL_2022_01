Public Class SEL_ISO_PANEL
    Public Structure STRUCT_SEL_ISO_PANEL
        Dim ISO_Panel As Panel
        Dim Fieldprefix As String
        Dim Subprefix As String
        Dim ISO_Panel_Parent_FP As FP
        Dim ISO_Code As String
    End Structure

    Public FPf As FP_Form
    Public WithEvents FP_ISO_PANEL As FP
    Private ISO_Panel As Panel
    Private Control_Prefix = "ISO_"
    Private FieldPrefix As String
    Private SubPrefix As String
    Private ISO_Panel_Parent_FP As FP
    Private ISO_Code As String
    Private Disposed As Boolean = False

    Private Control_Btn_SELECT_ALL As PictureBox
    Private Control_List_of_QUALIFICATIONS As ListView
    Private Control_Percentage_SUM As TextBox
    Private Control_Percentage_SUM_Label As Label
    Private Control_Remarks As TextBox
    Private Control_Remarks_Label As Label

    Private FPc_Percentage_SUM As FP_Control
    Private WithEvents FPc_List_of_QUALIFICATIONS As FP_Control
    Private WithEvents FPp_Btn_SELECT_ALL As FP_PictureBox

    Public Sub New(P As STRUCT_SEL_ISO_PANEL)
        ISO_Panel = P.ISO_Panel
        ISO_Panel_Parent_FP = P.ISO_Panel_Parent_FP
        ISO_Code = P.ISO_Code

        FieldPrefix = nz(P.Fieldprefix, "")
        SubPrefix = nz(P.Subprefix, "")

        FPf = gl_FPApp.FORMS_GET_FPf(gl_FPApp.CONTROLS_GET_Frm_from_c(ISO_Panel))

        FP_ISO_PANEL = New FP(FPf, "ISO_PANEL", SubPrefix, ISO_Panel_Parent_FP, "Parent_Record_ID")
        With FP_ISO_PANEL
            .P_FORM_AllowDeletions = False

            With .SQL_BIND_Params
                .NameOf_DEL = ""
            End With
        End With

        CREATE_CONTROLS()

        Dim FP_ISO_P As New Struct_FP_CONTROLS_COLLECTION
        With FP_ISO_P
            .FieldPrefix = Control_FieldPrefix_And_Prefix()
        End With
        FP_ISO_PANEL.INIT_CONTROLS(FP_ISO_P)

        Dim FPf_WHERE As String = String.Format("Parent_Record_ID = {0}", ISO_Panel_Parent_FP.P_DATA_Current_ID)
        If Not FP_ISO_PANEL.FORM_RECORDS_LOAD(FPf_WHERE) Then
            FP_ISO_PANEL.FORM_RECORDS_LOAD(FPf_WHERE, True)
        End If
    End Sub

    Public Function SAVE() As Boolean
        Dim OUT As Boolean = False

        OUT = FP_ISO_PANEL.FORM_RECORDS_SAVE_CURRENT

        Return OUT
    End Function

    Public Sub Dispose_Me()
        If Disposed = False Then
            If Not (FP_ISO_PANEL Is Nothing) Then
                FP_ISO_PANEL.Dispose()
                FP_ISO_PANEL = Nothing
            End If

            If Not (FPf Is Nothing) Then
                FPf = Nothing
            End If

            ISO_Panel = Nothing
            ISO_Panel_Parent_FP = Nothing

            If Not (Control_Btn_SELECT_ALL Is Nothing) Then
                Control_Btn_SELECT_ALL.Dispose()
                Control_Btn_SELECT_ALL = Nothing
            End If

            If Not (Control_List_of_QUALIFICATIONS Is Nothing) Then
                Control_List_of_QUALIFICATIONS.Dispose()
                Control_List_of_QUALIFICATIONS = Nothing
            End If

            If Not (Control_Percentage_SUM Is Nothing) Then
                Control_Percentage_SUM.Dispose()
                Control_Percentage_SUM = Nothing
            End If

            If Not (Control_Percentage_SUM_Label Is Nothing) Then
                Control_Percentage_SUM_Label.Dispose()
                Control_Percentage_SUM_Label = Nothing
            End If

            If Not (Control_Remarks Is Nothing) Then
                Control_Remarks.Dispose()
                Control_Remarks = Nothing
            End If

            If Not (Control_Remarks_Label Is Nothing) Then
                Control_Remarks_Label.Dispose()
                Control_Remarks_Label = Nothing
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

    Private Sub FP_ISO_PANEL_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_ISO_PANEL.CONTROLS_INITIALIZED
        With FP_ISO_PANEL
            FPp_Btn_SELECT_ALL = .PICTUREBOXES("Btn_SELECT_ALL")
            FPc_Percentage_SUM = .CONTROLS("Percentage_SUM")

            FPc_List_of_QUALIFICATIONS = .CONTROLS("List_of_QUALIFICATIONS")
            With FPc_List_of_QUALIFICATIONS
                If .P.DT_FixText_Key = "" Then
                    .P.DT_FixText_Key = "@@VB_LISTVIEW_ISO_QUALIFICATIONS"
                End If

                If .P.DT_WHERE2 = "" Then
                    .P.DT_WHERE2 = String.Format("Code = '{0}'", ISO_Code)
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
        If FP_ISO_PANEL.FORM_DIRTY_SET Then
            With FPc_List_of_QUALIFICATIONS
                If .LISTVIEW_IS_ALL_CHECKED Then
                    .LISTVIEW_UNCHECK_ALL()
                Else
                    .LISTVIEW_CHECK_ALL()
                End If
            End With
        End If
    End Sub

    Private Sub FPc_Percentage_SUM_CALCULATE()
        Dim Sum_Of_Selected As Single = 0

        If Not (FPc_List_of_QUALIFICATIONS.DT Is Nothing) Then
            For Each i As ListViewItem In FPc_List_of_QUALIFICATIONS.c_ListView.Items
                If i.Checked Then
                    Sum_Of_Selected += FPc_List_of_QUALIFICATIONS.DT.Select(String.Format("ID = {0}", i.Name)).First!Percentage
                End If
            Next
        End If

        FPc_Percentage_SUM.P_VALUE = Format(Sum_Of_Selected / 100, "P0")
    End Sub

    Private Sub CREATE_CONTROLS()
        If Not Disposed Then
            Control_Btn_SELECT_ALL = New PictureBox
            With Control_Btn_SELECT_ALL
                .Name = Control_FieldPrefix_And_Prefix() + "Btn_SELECT_ALL"
                .Visible = True
                .Parent = ISO_Panel
            End With
            ISO_Panel.Controls.Add(Control_Btn_SELECT_ALL)
            FPf.CONTROLS_ADD(Control_Btn_SELECT_ALL)

            Control_List_of_QUALIFICATIONS = New ListView
            With Control_List_of_QUALIFICATIONS
                .Name = Control_FieldPrefix_And_Prefix() + "List_of_QUALIFICATIONS"
                .Visible = True
                .Parent = ISO_Panel
            End With
            ISO_Panel.Controls.Add(Control_List_of_QUALIFICATIONS)
            FPf.CONTROLS_ADD(Control_List_of_QUALIFICATIONS)

            Control_Percentage_SUM = New TextBox
            With Control_Percentage_SUM
                .Name = Control_FieldPrefix_And_Prefix() + "Percentage_SUM"
                .Font = Font_NORMAL
                .Visible = True
                .Parent = ISO_Panel
            End With
            ISO_Panel.Controls.Add(Control_Percentage_SUM)
            FPf.CONTROLS_ADD(Control_Percentage_SUM)

            Control_Percentage_SUM_Label = New Label
            With Control_Percentage_SUM_Label
                .Name = Control_FieldPrefix_And_Prefix() + "Percentage_SUM_Label"
                .Font = Font_NORMAL
                .Parent = ISO_Panel
                .Visible = True
            End With
            ISO_Panel.Controls.Add(Control_Percentage_SUM_Label)
            FPf.CONTROLS_ADD(Control_Percentage_SUM_Label)

            Control_Remarks = New TextBox
            With Control_Remarks
                .Name = Control_FieldPrefix_And_Prefix() + "Remarks"
                .Font = Font_NORMAL
                .Visible = True
                .Parent = ISO_Panel
            End With
            ISO_Panel.Controls.Add(Control_Remarks)
            FPf.CONTROLS_ADD(Control_Remarks)

            Control_Remarks_Label = New Label
            With Control_Remarks_Label
                .Name = Control_FieldPrefix_And_Prefix() + "Remarks_Label"
                .Font = Font_NORMAL
                .Parent = ISO_Panel
                .Visible = True
            End With
            ISO_Panel.Controls.Add(Control_Remarks_Label)
            FPf.CONTROLS_ADD(Control_Remarks_Label)

        End If
    End Sub

    Private Sub FPc_List_of_QUALIFICATIONS_Field_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc_List_of_QUALIFICATIONS.Field_TextChanged
        If Not FP_ISO_PANEL.FORM_DIRTY_SET Then
            Cancel = True
        Else
            FPc_Percentage_SUM_CALCULATE()
        End If
    End Sub

    Private Sub FPc_List_of_QUALIFICATIONS_SET_Checkboxes()
        Dim Data_Binded_OLD As Boolean = FP_ISO_PANEL.DATA_Binded

        FP_ISO_PANEL.DATA_Binded = False

        Dim Checked_boxes As String = ""

        If FP_ISO_PANEL.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
            Checked_boxes = FP_ISO_PANEL.DATA_Field_getSavedValue("CheckList")
        End If

        Dim ListOf_Checked_boxes As List(Of String) = Split(Checked_boxes, "|").ToList

        For Each i As ListViewItem In Control_List_of_QUALIFICATIONS.Items
            i.Checked = (ListOf_Checked_boxes.Contains(i.Name))
        Next

        FPc_Percentage_SUM_CALCULATE()

        FP_ISO_PANEL.DATA_Binded = Data_Binded_OLD
    End Sub

    Public Overridable Sub FP_ISO_PANEL_Form_AfterUpdate(sender_FP As FP) Handles FP_ISO_PANEL.Form_AfterUpdate
        'Nothing to do
    End Sub

    Public Overridable Sub FP_ISO_PANEL_Form_BeforeUpdate(sender_FP As FP, ByRef Cancel As Integer) Handles FP_ISO_PANEL.Form_BeforeUpdate
        FP_ISO_PANEL.DATA_Field_setValue("CheckList", FPc_List_of_QUALIFICATIONS.LISTVIEW_GET_CHECKED_IDs_WITH_SEPARATOR("|"))
        FP_ISO_PANEL.DATA_Field_setValue("Code", ISO_Code)
    End Sub

    Public Overridable Sub FP_ISO_PANEL_Form_Current(sender_FP As FP) Handles FP_ISO_PANEL.Form_Current
        FPc_List_of_QUALIFICATIONS_SET_Checkboxes()
    End Sub
End Class
