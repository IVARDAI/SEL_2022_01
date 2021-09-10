Public Class FP_SELECT_PICTURE
    Public Structure STRUCT_FP_SELECT_PICTURE_PARAMS
        Dim SubPrefix As String
        Dim SKIN_File_Name As String    'Annak a SKIN file-nak a neve, amiben a kepek talalhatok Pl.: "SEL_MENU_SKIN"
        Dim PictureName_Prefix As String    'Azokat a kepfile-okat fogja felsorolni a SKIN_File_Name SKIN-bol, amelyeknek neve az itt megadottal kezdodik. Pl.: "FUNC_"
    End Structure

    Public WithEvents FPf As FP_Form = Nothing
    Public WithEvents FP As FP = Nothing
    Public WithEvents FPp_Btn_OK As FP_PictureBox
    Public WithEvents FPp_Category As FP_Control
    Public FPpPanel As FP_L_PictureList

    Public SubPrefix As String = ""
    Public SKIN_File_Name As String = ""
    Public PictureName_Prefix As String = ""

    Public OUT_SELECTED_Picture As String

    Public Sub New(P As STRUCT_FP_SELECT_PICTURE_PARAMS)
        InitializeComponent()

        SubPrefix = P.SubPrefix
        SKIN_File_Name = P.SKIN_File_Name
        PictureName_Prefix = P.PictureName_Prefix

        FPf = New FP_Form("FP_SELECT_PICTURE_BASE", gl_FPApp, Me, True)
        FPf.Location_Save_On_Close = False

        FP = New FP(FPf, "FP_SELECT_PICTURE", SubPrefix, True)
    End Sub


    Private Sub SELECT_F_PICTURE_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Dim FPf_CONTROLS As New Struct_FP_FORM_CONTROLS_COLLECTION

        With FPf_CONTROLS
            .Dlg_Btn_CANCEL = Btn_Cancel
        End With
        FPf.INIT_CONTROLS(FPf_CONTROLS)

        FP.INIT_CONTROLS(Nothing)

        FPpPanel = New FP_L_PictureList(FPf, Picture_Panel)
    End Sub

    Private Sub FPf_CONTROLS_INITIALIZED(ByVal sender_FPf As FP_Form) Handles FPf.CONTROLS_INITIALIZED
        With FPf
            FPp_Btn_OK = .PICTUREBOXES("Btn_OK")
        End With
    End Sub

    Private Sub FPp_Btn_OK_CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FPp_Btn_OK.CLICK
        If FPpPanel.Last_SELECTED_Picture = "" Then
            FPf.DoMyMsgBox(131) 'Valasszon egy kepet!
        Else
            OUT_SELECTED_Picture = FPpPanel.Last_SELECTED_Picture
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Close()
        End If
    End Sub

    Private Sub FP_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP.CONTROLS_INITIALIZED
        FPp_Category = FP.CONTROLS("Category")
    End Sub

    Private Sub FPp_Category_Field_TextChanged(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByVal e As System.EventArgs, ByRef Cancel As Boolean) Handles FPp_Category.Field_TextChanged
        FPpPanel.PICTURES_REMOVE_ALL()

        If FPp_Category.P_VALUE > 0 Then
            'Dim ASM_Name As String = "SELEXPED_Menu"
            'Dim CurrentASM As Reflection.Assembly = Reflection.Assembly.Load(ASM_Name)
            Dim CurrentASM As Reflection.Assembly = Nothing
            Dim ASM_Name As String = nz(SKIN_File_Name, "")

            If ASM_Name > "" Then
                ASM_Name += ".."
            End If

            If gl_FPApp.SKIN_getASM_And_OBJECTNAME(ASM_Name + PictureName_Prefix + "_Empty.png", CurrentASM, "") Then

                Dim Criteria As String = String.Format("Number = {0}", FPp_Category.P_VALUE)
                Dim DRow As DataRow = FPp_Category.DT.Select(Criteria).First
                Dim PictureName_Prefix_With_Category As String = String.Format("{0}_{1}", PictureName_Prefix, DRow!Text3)
                Dim PictureSize_Width As Integer = 44
                Dim PictureSize_Height As Integer = 44

                If DRow!Text4 <> "" Then
                    PictureSize_Width = gl_FPApp.Text_getParamFromLine(UCase(DRow!Text4), 1, , , "X")
                    PictureSize_Height = gl_FPApp.Text_getParamFromLine(UCase(DRow!Text4), 2, , , "X")
                End If

                For Each ResName As String In CurrentASM.GetManifestResourceNames
                    Dim Extension As String = ResName.Substring(ResName.LastIndexOf("."))
                    Dim Extension_Pressed As Boolean = (ResName.LastIndexOf("_.") > 0)

                    Select Case Extension
                        Case ".bmp", ".jpg", ".png", ".gif"
                            If Not Extension_Pressed Then
                                Dim Add_Picture_As_Name As String = ""

                                If ASM_Name = "" Then
                                    Add_Picture_As_Name = Mid(ResName, Len("SEL_SKIN") + 2)
                                Else
                                    Add_Picture_As_Name = Mid(ResName, Len(ASM_Name))
                                End If

                                If Mid(Add_Picture_As_Name, 1, Len(PictureName_Prefix_With_Category) + 1) = PictureName_Prefix_With_Category + "_" Then
                                    Dim Prepared_RES_Name As String = ""

                                    If ASM_Name = "" Then
                                        Prepared_RES_Name = Mid(ResName, Len("SEL_SKIN") + 2)
                                    Else
                                        Prepared_RES_Name = ASM_Name + Mid(ResName, Len(ASM_Name))
                                    End If

                                    Add_Picture_As_Name = Mid(Add_Picture_As_Name, 1, Len(Add_Picture_As_Name) - Len(Extension))
                                    FPpPanel.PICTURES_ADD(Prepared_RES_Name, Add_Picture_As_Name, New Size(PictureSize_Width, PictureSize_Height), "")
                                End If
                            End If

                        Case Else
                            'Nothing to do
                    End Select
                Next
            End If
        End If
    End Sub
End Class