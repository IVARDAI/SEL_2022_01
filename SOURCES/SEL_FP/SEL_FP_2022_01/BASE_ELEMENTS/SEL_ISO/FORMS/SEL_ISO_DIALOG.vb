Public Class SEL_ISO_DIALOG
    Public Structure STRUCT_SEL_ISO_DIALOG
        Dim ISO_Parent_FP As FP
        Dim ISO_Code As String
        Dim Open_In_DialogMode As Boolean
    End Structure

    Public P As STRUCT_SEL_ISO_DIALOG
    Public ISO_PANEL As SEL_ISO_PANEL
    Public WithEvents FPf As FP_Form
    Public WithEvents FPp_BTN_OK As FP_PictureBox
    Public WithEvents FP_MAIN As FP
    Public WithEvents Parent_FPf As FP_Form
    Protected Frm_disposed As Boolean = False

    Sub New(MyP As STRUCT_SEL_ISO_DIALOG)
        InitializeComponent()

        P = MyP

        FPf = New FP_Form("SEL_ISO_DIALOG_MAIN", gl_FPApp, Me, P.Open_In_DialogMode)
        FP_MAIN = New FP(FPf, "SEL_ISO_DIALOG", , True)
        Parent_FPf = P.ISO_Parent_FP.FPf
    End Sub

    Overridable Sub Dispose_Me()
        If Not Frm_disposed Then
            If Not (ISO_PANEL Is Nothing) Then
                ISO_PANEL.Dispose_Me()
                ISO_PANEL = Nothing

            End If

            If Not (FP_MAIN Is Nothing) Then
                FP_MAIN.Dispose()
                FP_MAIN = Nothing
            End If

            If Not (FPf Is Nothing) Then
                FPf.Dispose()
                FPf = Nothing
            End If

            Frm_disposed = True
        End If
    End Sub

    Private Sub SEL_ISO_DIALOG_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Dispose_Me()
    End Sub

    Protected Overridable Sub SEL_ISO_DIALOG_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim FPf_Params As New Struct_FP_FORM_CONTROLS_COLLECTION

        With FPf_Params
            If FPf.DialogMode Then
                .Btn_Default = BTN_OK
            End If
            .Btn_HELP = BTN_HLP
        End With
        FPf.INIT_CONTROLS(FPf_Params)

        Dim FP_MAIN_Params As New Struct_FP_CONTROLS_COLLECTION

        FP_MAIN.INIT_CONTROLS(FP_MAIN_Params)
    End Sub

    Protected Overridable Sub FP_MAIN_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_MAIN.CONTROLS_INITIALIZED
        If Not (ISO_PANEL Is Nothing) Then
            ISO_PANEL.Dispose_Me()
            ISO_PANEL = Nothing
        End If

        Dim ISO_PANEL_PARAMS As New SEL_ISO_PANEL.STRUCT_SEL_ISO_PANEL

        With ISO_PANEL_PARAMS
            .ISO_Panel = Panel_ISO
            .ISO_Panel_Parent_FP = P.ISO_Parent_FP
            .ISO_Code = P.ISO_Code
        End With
        ISO_PANEL = New SEL_ISO_PANEL(ISO_PANEL_PARAMS)
    End Sub

    Private Sub FP_MAIN_CONTROLS_INITIALIZING(sender_FP As FP) Handles FP_MAIN.CONTROLS_INITIALIZING
        If Not (ISO_PANEL Is Nothing) Then
            ISO_PANEL.Dispose_Me()
            ISO_PANEL = Nothing
        End If
    End Sub

    Private Sub Parent_FPf_FORM_CLOSING(sender As Object, ByRef e As FormClosingEventArgs) Handles Parent_FPf.FORM_CLOSING
        If Frm_disposed = False Then
            If Not ISO_PANEL.SAVE Then
                e.Cancel = True
            End If
        End If
    End Sub

    Protected Overridable Sub FPf_CONTROLS_INITIALIZED(sender_FPf As FP_Form) Handles FPf.CONTROLS_INITIALIZED
        FPp_BTN_OK = FPf.PICTUREBOXES("BTN_OK")
    End Sub

    Public Overridable Sub FPp_BTN_OK_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPp_BTN_OK.CLICK
        If ISO_PANEL.SAVE Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class