Public Class SEL_DOCMAN_ARCHIV_MSG
    Public Enum ENUM_SEL_DOCMAN_ARCHIV_MSG_TYPE As Integer
        BTN_SAVE = 0
        BTN_MAIL = 1
        BTN_SAVE_AND_MAIL = 2
    End Enum

    Public Structure STRUCT_SEL_DOCMAN_ARCHIV_MSG_P
        Dim Show_Type As ENUM_SEL_DOCMAN_ARCHIV_MSG_TYPE
    End Structure

    Public Show_Type As ENUM_SEL_DOCMAN_ARCHIV_MSG_TYPE
    Public FPf As FP_Form
    Public WithEvents FP As FP
    Public WithEvents FPp_Btn_SAVE As FP_PictureBox

    Sub New()
        InitializeComponent()
    End Sub

    Sub New(P As STRUCT_SEL_DOCMAN_ARCHIV_MSG_P)
        InitializeComponent()

        With P
            Show_Type = .Show_Type
        End With
    End Sub

    Sub New(Form_Show_Type As ENUM_SEL_DOCMAN_ARCHIV_MSG_TYPE)
        InitializeComponent()

        Show_Type = Form_Show_Type
    End Sub

    Private Sub SEL_DOCMAN_ARCHIV_MSG_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FPf = New FP_Form("DOCMAN_ARCHIV_DIALOG_BASE", gl_FPApp, Me, True)
        FP = New FP(FPf, "DOCMAN_ARCHIVE_DIALOG", , True)

        FPf.INIT_CONTROLS(Nothing)
        FP.INIT_CONTROLS(Nothing)

        Me.BringToFront()
    End Sub

    Private Sub FP_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP.CONTROLS_INITIALIZED
        FPp_Btn_SAVE = FP.PICTUREBOXES("Btn_SAVE")

        Select Case Show_Type
            Case ENUM_SEL_DOCMAN_ARCHIV_MSG_TYPE.BTN_SAVE
                FPp_Btn_SAVE.PICTURES_SET("but_save_44x44.png")

            Case ENUM_SEL_DOCMAN_ARCHIV_MSG_TYPE.BTN_MAIL
                FPp_Btn_SAVE.PICTURES_SET("but_mail_44x44.png")

            Case Else
                gl_FPApp.DoErrorMsgBox("SEL_DOCMAN_ARCHIV_MSG.FP_CONTROLS_INITIALIZED", 0, "Unknown Show_Type")
        End Select

        FPp_Btn_SAVE.SHOW()
    End Sub

    Private Sub FPp_Btn_SAVE_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPp_Btn_SAVE.CLICK
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class