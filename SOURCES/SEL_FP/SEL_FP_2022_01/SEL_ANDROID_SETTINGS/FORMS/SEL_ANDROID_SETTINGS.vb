
Imports MessagingToolkit.QRCode.Codec
Imports System.IO
Public Class SEL_ANDROID_SETTINGS
    Public WithEvents FPf As FP_Form
    Public WithEvents FP_WEBSERVICES_PARAMS As FP
    Public URL_for_DOWNLOAD As String = URL_DOWNLAD_GET("WRHS_ANDROID")
    Public WithEvents FPC_meessage_01 As FP_Control
    Public ANDROID_WEBSERVICES_PARAMS_DIC As Dictionary(Of String, String)

    Public Sub New()
        InitializeComponent()

        FPf = New FP_Form("Webservices_frm", gl_FPApp, Me, False)
        FP_WEBSERVICES_PARAMS = New FP(FPf, "WEBSERVICES_PARAMS", , True)
    End Sub

    Public Function URL_DOWNLAD_GET(ProductName As String) As String
        Dim OUT As String = ""
        Dim ANDROID_WEBSERVICES_PARAMS_STR As String = gl_FPApp.getFixText("SEL_WEBSERVICES_PARAMS", False)
        gl_FPApp.FIXTEXT_SPLIT_PARAMS(ANDROID_WEBSERVICES_PARAMS_STR, ANDROID_WEBSERVICES_PARAMS_DIC)

        OUT = String.Format(gl_FPApp.FIXTEXT_getParam("DOWNLOAD_URL", ANDROID_WEBSERVICES_PARAMS_DIC) + "/{0}/{1}", Replace(VERS, ".", "_"), "WRHS_ANDROID.apk")
        'OUT = String.Format(gl_FPApp.FIXTEXT_getParam("WEBSERVICES_URL", ANDROID_WEBSERVICES_PARAMS_DIC) + "/{0}", "V01.apk")
        Return OUT
    End Function

    Public Function SVC_GET() As String
        Dim OUT As String = ""
        Dim ANDROID_WEBSERVICES_PARAMS_STR As String = gl_FPApp.getFixText("SEL_WEBSERVICES_PARAMS", False)
        gl_FPApp.FIXTEXT_SPLIT_PARAMS(ANDROID_WEBSERVICES_PARAMS_STR, ANDROID_WEBSERVICES_PARAMS_DIC)

        OUT = gl_FPApp.FIXTEXT_getParam("WEBSERVICES_URL", ANDROID_WEBSERVICES_PARAMS_DIC)

        Return OUT
    End Function


    Private Sub Form_Wrhs_Android_Load(sender As Object, e As EventArgs) Handles MyBase.Load




        PBox_download.Image = ByteToImage(CREATE_QR_Bite(RemoveWhitespace(URL_for_DOWNLOAD)))
        '  PBox_download.Image = ByteToImage(CREATE_QR_Bite(RemoveWhitespace("http://webrestapiservices02.azurewebsites.net/service1.svc")))

        PBox_SVC.Image = ByteToImage(CREATE_QR_Bite(RemoveWhitespace(SVC_GET)))
        WB.Url = New System.Uri(String.Format("http://www.selexped.selester.hu/SELEXPED/Android_screen_1_H/Android_screen_1_H.htm", gl_FPApp.LandDialog))
        'Stinging_box01()
        'Stinging_box02()
        Me.Width = My.Computer.Screen.WorkingArea.Width - 50


    End Sub

    Function RemoveWhitespace(fullString As String) As String
        Return New String(fullString.Where(Function(x) Not Char.IsWhiteSpace(x)).ToArray())
    End Function


    Private Function CREATE_QR_Bite(Data As String) As Byte()
        '  Dim OUT As Boolean = False
        Dim QR As New QRCodeEncoder
        Dim Bitmap As Bitmap = QR.Encode(Data)
        Dim Converter As New ImageConverter

        Dim QRCode As Byte() = Converter.ConvertTo(Bitmap, GetType(Byte()))

        Return QRCode
    End Function
    Public Shared Function ByteToImage(ByVal blob As Byte()) As Bitmap
        Dim mStream As MemoryStream = New MemoryStream()
        Dim pData As Byte() = blob
        mStream.Write(pData, 0, Convert.ToInt32(pData.Length))
        Dim bm As Bitmap = New Bitmap(mStream, False)
        mStream.Dispose()
        Return bm
    End Function
    'Private Sub Stinging_box01()

    '    Dim Target As String = ""
    '    Infobox1.Text = "1.ELSŐ LÉPÉS" & vbCrLf _
    '                         & "Olvassa be a letöltéshez tartozó QR kódot."




    '    Target = "1.ELSŐ LÉPÉS"

    '    Infobox1.SelectionStart = Infobox1.Text.IndexOf(Target)
    '    Infobox1.SelectionLength = Target.Length
    '    Infobox1.SelectionFont = New Font("Arial", 30, FontStyle.Bold Or FontStyle.Underline)
    '    Infobox1.SelectionColor = Color.Red



    '    Target = "Olvassa be a letöltéshez tartozó QR kódot."

    '    Infobox1.SelectionStart = Infobox1.Text.IndexOf(Target)
    '    Infobox1.SelectionLength = Target.Length
    '    Infobox1.SelectionFont = New Font("Arial", 14, FontStyle.Bold Or FontStyle.Regular)



    '    Infobox1.SelectionStart = 0
    '    Infobox1.SelectionLength = 0
    'End Sub

    'Private Sub Stinging_box02()

    '    Dim Target As String = ""
    '    infobox2.Text = "2.MÁSODIK LÉPÉS" & vbCrLf _
    '                         & "Olvassa be az alkalmazás beállításhoz tartozó QR kódot"




    '    Target = "2.MÁSODIK LÉPÉS"

    '    infobox2.SelectionStart = infobox2.Text.IndexOf(Target)
    '    infobox2.SelectionLength = Target.Length
    '    infobox2.SelectionFont = New Font("Arial", 30, FontStyle.Bold Or FontStyle.Underline)
    '    infobox2.SelectionColor = Color.Red



    '    Target = "Olvassa be az alkalmazás beállításhoz tartozó QR kódot"

    '    infobox2.SelectionStart = infobox2.Text.IndexOf(Target)
    '    infobox2.SelectionLength = Target.Length
    '    infobox2.SelectionFont = New Font("Arial", 14, FontStyle.Bold Or FontStyle.Regular)



    '    infobox2.SelectionStart = 0
    '    infobox2.SelectionLength = 0
    'End Sub

    Private Sub Tovább_Click(sender As Object, e As EventArgs)
1:
        PBox_download.Visible = False
        PBox_SVC.Visible = True
        WB.Url = New System.Uri(String.Format("http://Startlap.com", ""))
    End Sub

    Private Sub WB_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WB.DocumentCompleted

    End Sub
End Class