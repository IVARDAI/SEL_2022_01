Public Class FP_ErrorList
    Public Structure FP_ERRORLIST_PARAMS
        Dim Parent_FPf As FP_Form
        Dim TransactID As Long
    End Structure

    Public WithEvents FPf As FP_Form
    Public WithEvents FP_ErrorList As FP
    Public WithEvents Parent_FPf As FP_Form
    Public TransactID As Long
    Public Field_Sign As FP_L_Field_Sign
    Private Frm_Disposed As Boolean = False

    Sub New(P As FP_ERRORLIST_PARAMS)
        InitializeComponent()

        With P
            Parent_FPf = .Parent_FPf
            TransactID = .TransactID
        End With

        FPf = New FP_Form("FP_ERRORLIST_BASE", gl_FPApp, Me, False)
        FP_ErrorList = New FP(FPf, "FP_ERRORLIST")
        With FP_ErrorList
            With .SQL_BIND_Params
                .NameOf_DEL = ""
                .NameOf_SAVE = ""
            End With

            .P_FORM_AllowAdditions = False
            .P_FORM_AllowDeletions = False
            .P_FORM_AllowEdits = False
        End With
    End Sub

    Private Sub FP_ErrorList_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        Sign_Goto_Current()
    End Sub

    Private Sub FP_ErrorList_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
        Field_Sign.HIDE()
    End Sub

    Private Sub FP_ErrorList_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        '---------------------------------------------------------------------------
        'Form
        '---------------------------------------------------------------------------
        Dim FPf_Controls As New Struct_FP_FORM_CONTROLS_COLLECTION

        With FPf_Controls
            '.Btn_HELP = Button_Help
        End With
        FPf.INIT_CONTROLS(FPf_Controls)

        '---------------------------------------------------------------------------
        'FP_ErrorList
        '---------------------------------------------------------------------------
        Dim FP_ErrorList_Controls As New Struct_FP_CONTROLS_COLLECTION

        With FP_ErrorList_Controls
            '.Btn_ExportToExcel = Button_ExcelExport
            With .GRID
                .GRID = MainGrid
                .Label = MainGrid_Label
            End With
        End With

        FP_ErrorList.INIT_CONTROLS(FP_ErrorList_Controls)

        Field_Sign = New FP_L_Field_Sign(Parent_FPf, 1, Color.Red)
    End Sub

    Private Sub FP_ErrorList_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim Crit As String = String.Format("TransactID = {0} AND Terminals_ID = {1}", TransactID, Terminals_ID)

        If Not FP_ErrorList.FORM_RECORDS_LOAD(Crit, False, True) Then
            Me.Close()
        End If
    End Sub

    Private Sub Sign_Goto_Current()
        Field_Sign.HIDE()

        If FP_ErrorList.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
            Dim GOTO_ALIASES As String = FP_ErrorList.DATA_Field_getValue_FromREAD("FP_ALIASES")
            Dim GOTO_IDs As String = FP_ErrorList.DATA_Field_getValue_FromREAD("FP_IDs")
            Dim FieldName As String = FP_ErrorList.DATA_Field_getValue_FromREAD("FieldName")
            Dim DoIt As Boolean = True

            If GOTO_ALIASES > "" And GOTO_IDs > "" Then
                Dim GOTO_Handler As New FP_L_FORM_GOTO_RECORDS(Parent_FPf)

                With GOTO_Handler
                    .GOTO_RECORDS_ADD(GOTO_ALIASES, GOTO_IDs)
                    DoIt = .GOTO_RECORDS()
                End With

                If DoIt And FieldName > "" Then
                    If Parent_FPf.CONTROLS.Keys.Contains(FieldName) Then
                        Dim c As Control = Parent_FPf.CONTROLS(FieldName)
                        Dim FPc As FP_Control = Nothing

                        If Parent_FPf.CONTROLS_GET_FPo_FROM_CONTROL(c, FPc) Then
                            Field_Sign.SHOW(FPc)
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub FP_ErrorList_Form_Current(sender_FP As FP) Handles FP_ErrorList.Form_Current
        Sign_Goto_Current()
    End Sub

    Public Sub DisposeMe()
        If Frm_Disposed = False Then
            Field_Sign.Dispose()
            Field_Sign = Nothing

            Parent_FPf = Nothing

            Frm_Disposed = True
        End If
    End Sub

    Private Sub Parent_FPf_FORM_CLOSING(sender As Object, ByRef e As FormClosingEventArgs) Handles Parent_FPf.FORM_CLOSING
        DisposeMe()
        Me.Close()
    End Sub
End Class