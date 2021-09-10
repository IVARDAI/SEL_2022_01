Public Class FP_Simple_Edit
    Public SimpleEdit_Key As String
    Public FPApp As FP_App
    Public FPf As FP_Form
    Public WithEvents FP_SIMPLEEDIT As FP
    Public PFD_Params As FP_L_Form_with_PFD_Params = Nothing

    Public Sub New(ByVal MyFPApp As FP_App, ByVal MySimpleEdit_Key As String, Optional ByVal Pfd_Key_for_Save_Params As String = "")
        InitializeComponent()

        FPApp = MyFPApp

        FPf = New FP_Form("SIMPLEEDIT_HEAD", FPApp, Me, True)
        FPf.Location_Save_On_Close = False

        SimpleEdit_Key = MySimpleEdit_Key
        FP_SIMPLEEDIT = New FP(FPf, "SIMPLEEDIT", SimpleEdit_Key, True)

        Dim PFD_Params_P As New FP_L_Form_with_PFD_Params.Struct_FP_L_Form_with_PFD_Params

        With PFD_Params_P
            .FPf = FPf
            .Pfd_Key = Pfd_Key_for_Save_Params
        End With

        PFD_Params = New FP_L_Form_with_PFD_Params(PFD_Params_P)
    End Sub

    Public Sub DATAFIELD_ADD(ByVal FieldName As String, ByVal DB_Formatted_Value As String, Optional ByVal Selected_ID_Value As Long = 0, Optional FieldLength As Integer = 0)
        PFD_Params.DATAFIELD_ADD(FieldName, DB_Formatted_Value, Selected_ID_Value, FieldLength)
    End Sub

    Public Sub DATAFIELD_ADD(ByVal FieldName As String)
        PFD_Params.DATAFIELD_ADD(FieldName)
    End Sub

    Public Function DATAFIELD_GET(ByVal FieldName As String, Optional ByRef OUT_Selected_ID As Long = 0) As String
        Return PFD_Params.DATAFIELD_GET(FieldName, OUT_Selected_ID)
    End Function

    Private Sub FP_Simple_Edit_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                NAVIGATION_FORWARD()
                e.Handled = True

            Case Else
                'Nothing to do
        End Select
    End Sub

    Private Sub FP_Simple_Edit_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Dim FPf_CONTROLS As New Struct_FP_FORM_CONTROLS_COLLECTION
        With FPf_CONTROLS
            .Btn_HELP = Btn_Help
            .Dlg_Btn_CANCEL = Btn_Cancel
        End With

        FPf.INIT_CONTROLS(FPf_CONTROLS)
        FP_SIMPLEEDIT.INIT_CONTROLS(Nothing)

        PFD_Params.DATAFIELD_SET_ALL_FIELDS_FROM_DT()

        FPf.FOCUS_ON_FIRST_CONTROL()
    End Sub

    Private Sub NAVIGATION_FORWARD()
        If FP_SIMPLEEDIT.FORM_CHK_FIELDS Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            If PFD_Params.Pfd_Key > "" Then
                PFD_Params.SAVE_TO_PFD()
            End If
            Me.Close()
        End If
    End Sub

    Private Sub Btn_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_OK.Click
        NAVIGATION_FORWARD()
    End Sub
End Class
