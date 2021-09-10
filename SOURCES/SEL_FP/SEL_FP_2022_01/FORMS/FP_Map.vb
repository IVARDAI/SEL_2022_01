Public Class FP_Map
    Public WithEvents FPf As FP_Form = Nothing
    Public WithEvents FP_MAP As FP = Nothing

    Private URL As String

    Public Sub New(ByVal P_URL As String)
        InitializeComponent()

        URL = P_URL
    End Sub

    Private Sub SEL_MAP_Load(sender As Object, e As EventArgs) Handles Me.Load
        FPf = New FP_Form("FP_MAP_BASE", gl_FPApp, Me, False)
        FPf.Location_Save_On_Close = False
        FP_MAP = New FP(FPf, "FP_MAP", , True)

        If URL <> "" Then
            Dim Uri As New Uri(URL)
            WV_Map.Navigate(Uri)
        End If
    End Sub

    Private Sub FP_Map_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        FPf.INIT_CONTROLS(Nothing)
        FP_MAP.INIT_CONTROLS(Nothing)
    End Sub
End Class