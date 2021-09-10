Public Class SEL_MODULE_NOT_INSTALLED_PANEL
    Public Structure Struct_SEL_MODUL_NOT_INSTALLED_PANEL_PARAMS
        Dim BASE_Panel As Panel
        Dim FPf As FP_Form
        Dim Fieldprefix As String
        Dim Subprefix As String
        Dim Info_URL As String
    End Structure

    Public BASE_Panel As Panel
    Public FPf As FP_Form
    Public Fieldprefix As String
    Public Subprefix As String
    Public Control_Prefix As String = "NOT_INST_"
    Public Info_URL As String
    Public FP_MAIN As FP

    Public CONTROLS_Info_WB As WebBrowser

    Private Disposed As Boolean

    Sub New(P As Struct_SEL_MODUL_NOT_INSTALLED_PANEL_PARAMS)
        With P
            BASE_Panel = .BASE_Panel
            FPf = .FPf
            Fieldprefix = .Fieldprefix
            Subprefix = .Subprefix
            Info_URL = gl_FPApp.Text_Replace_Standard_Params(.Info_URL)
        End With

        CREATE_CONTROLS()

        FP_MAIN = New FP(FPf, "SEL_MODUL_NOT_INST", Subprefix, True)

        FP_MAIN.INIT_CONTROLS(Nothing)
    End Sub

    Private Sub CREATE_CONTROLS()
        CONTROLS_Info_WB = New WebBrowser
        With CONTROLS_Info_WB
            .Name = Control_FieldPrefix_And_Prefix() + "WB"
            .TabIndex = 9999
            .TabStop = False
            .Url = New System.Uri("", System.UriKind.Relative)
            .Visible = True
            .Parent = BASE_Panel
        End With
        FPf.CONTROLS_ADD(CONTROLS_Info_WB)
    End Sub

    Public Sub DisposeMe()
        If Disposed = False Then
            Disposed = True

            If Not (CONTROLS_Info_WB Is Nothing) Then
                FPf.CONTROLS_REMOVE(CONTROLS_Info_WB.Name)
                CONTROLS_Info_WB.Dispose()
                CONTROLS_Info_WB = Nothing
            End If

            FPf = Nothing
            BASE_Panel = Nothing
        End If
    End Sub

    Public Function Control_FieldPrefix_And_Prefix() As String
        Dim OUT As String = Control_Prefix

        If Fieldprefix > "" Then
            OUT = Fieldprefix + OUT
        End If

        Return OUT
    End Function
End Class
