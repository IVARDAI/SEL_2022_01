Imports Outlook = Microsoft.Office.Interop.Outlook
Imports System.Net.Mail

Public Class FP_EMAIL
    'Egyszeru e-mail kuldes
    'Az e-mail kuldes alapparameterei megadhatok kozvetlenul vagy fixtext bejegyzeseken keresztul.
    '
    '-----------------------------------------------------------------------------------------------
    'Legegyszerubb e-mail kuldes (MAILTO) szabvany
    '------------------------------------------------------------------------------------------------
    ' Ha egy bongeszobe beirod:
    '
    '            mailto: molnar.laszlo@selester.hu
    '
    ' akkor megnyilik az alapertelmezett e-mail kuldo program, benne az e-mail cimmel (probald ki)
    ' Ezen a modon kuldott e-mail-hez nem csatolhato melleklet! Az elonye a vegtelen egyszerusege.
    '
    ' Pelda a MAILTO alapu e-mail kuldes hasznalatara:
    ' ------------------------------------------------
    '
    ' Dim Email As New FP_EMAIL('MyFixText_Key', Email_Params)
    ' Email.EMAIL_SEND_via_MAILTO()
    '
    ' A FixText-be a kovetkezo bejegyzeseket kell betenni:
    ' MyFixText_Key_PARAMS
    ' --------------------
    ' TO = <e-mail cimek>
    ' CC = 
    ' BCC = 
    ' SUBJECT = Megrendeles @1
    '
    ' A SUBJECT-be @1, @2, stb.-t irva parametereket jelolhetunk ki, amelyeket az Email_Params-ban adhatunk at "|" jelekkel elvalasztva.
    '
    ' MyFixText_Key_TEXT
    ' ------------------
    ' Ez itt az e-mail szovege. @1, @2 parametereket tartalmazhat.
    '
    '------------------------------------------------------------------------------------------------
    ' Pelda OUTLOOK-al valo kuldesre (A gepen OUTLOOK-nak lennie kell!!!)
    '-----------------------------------------------------------------------------------------------
    ' Dim Email As New FP_EMAIL('MyFixText_Key', Email_Params)
    ' Email.EMAIL_SEND_via_OUTLOOK()
    '
    ' FixText bejegyzesek mint MAILTO tipusu kuldesnel.
    '
    '
    '------------------------------------------------------------------------------------------------
    ' Pelda SMTP alapu kuldesre
    '-----------------------------------------------------------------------------------------------
    ' Dim Email As New FP_EMAIL('MyFixText_Key', Email_Params)
    ' Email.EMAIL_SEND_via_SMTP()
    '
    ' FixText bejegyzesek:
    ' --------------------
    '
    ' SMTP_PARAMS
    ' -----------
    ' IP=192.168.1.253
    ' FROM=noreply-belfold@ghibli.hu
    ' CREDENTIAL_USER=noreply-belfold@ghibli.hu
    ' CREDENTIAL_PASSWORD=Ghibli1
    ' PORT=25
    '
    ' MyFixText_Key_PARAMS
    ' --------------------
    ' lasd: MAILTO-nal leirtak
    '
    ' MyFixText_Key_TEXT
    ' ------------------
    ' Lasd MAILTO-nal leirtak

    Public Structure STRUCT_FP_EMAIL_SMTP_PARAMS
        Dim Credential_UserName As String
        Dim Credential_Password As String
        Dim Email_FROM As String
        Dim Fix_BCC As String
        Dim EnableSSL As Boolean
        Dim Port As String
        Dim IP As String
        Dim Use_Default_Credentials As Boolean
        Dim Is_Body_HTML As Boolean
    End Structure

    Public Structure STRUCT_FP_EMAIL_PARAMS
        Dim FixText_Key As String
        Dim Email_TO As String
        Dim Email_CC As String
        Dim Email_BCC As String
        Dim Email_SUBJECT As String
        Dim Email_TEXT As String
        Dim Replace_Params As String
        Dim SMTP_Params As STRUCT_FP_EMAIL_SMTP_PARAMS
    End Structure

    Public ReadOnly Property Attached_File_List As List(Of String)
        Get
            Return Email_Attached_files
        End Get
    End Property

    Public Enum ENUM_FP_EMAIL_HANDLING As Integer
        SEND = 1
        SHOW = 2
    End Enum

    Private FixText_Key As String
    Private Email_Params As String

    Private Email_FROM As String
    Private Email_TO As String
    Private Email_CC As String
    Private Email_BCC As String
    Private Email_SUBJECT As String
    Private Email_TEXT As String
    Private Replace_Params As String
    Private Email_Footer_html As String = ""
    Private Email_Attached_files As New List(Of String)
    Private SMTP_Connect_Params As New STRUCT_FP_EMAIL_SMTP_PARAMS

    Public Sub New()
        SMTP_PARAMS_SET_FROM_FIXTEXT()
    End Sub

    Public Sub New(FixText_Key As String, Optional MyReplace_Params As String = "")
        P_FixText_Key = FixText_Key
        P_Replace_Params = MyReplace_Params

        Params_SET_FROM_FIXTEXT()
        SMTP_PARAMS_SET_FROM_FIXTEXT()
    End Sub

    Public Sub New(Params As STRUCT_FP_EMAIL_PARAMS)
        With Params
            P_FixText_Key = .FixText_Key
            If P_FixText_Key > "" Then
                Params_SET_FROM_FIXTEXT()

                If .Email_TO = "" Then
                    P_Email_TO = .Email_TO
                End If

                If .Email_CC > "" Then
                    P_Email_CC = .Email_CC
                End If

                If .Email_BCC > "" Then
                    P_Email_BCC = .Email_BCC
                End If

                If .Email_SUBJECT > "" Then
                    P_Email_Subject = .Email_SUBJECT
                End If

                If .Email_TEXT > "" Then
                    P_Email_Text = .Email_TEXT
                End If

                P_Replace_Params = .Replace_Params

                SMTP_PARAMS_SET_FROM_FIXTEXT()
            Else
                P_Email_TO = .Email_TO
                P_Email_CC = .Email_CC
                P_Email_BCC = .Email_BCC
                P_Email_Subject = .Email_SUBJECT
                P_Email_Text = .Email_TEXT
                P_Replace_Params = .Replace_Params

                With .SMTP_Params
                    SMTP_Connect_Params.Credential_UserName = .Credential_UserName
                    SMTP_Connect_Params.Credential_Password = .Credential_Password
                    SMTP_Connect_Params.Email_FROM = .Email_FROM
                    SMTP_Connect_Params.EnableSSL = .EnableSSL
                    SMTP_Connect_Params.Port = .Port
                    SMTP_Connect_Params.IP = .IP
                End With
            End If
        End With
    End Sub

    Public Property P_FixText_Key As String
        Get
            Return FixText_Key
        End Get
        Set(value As String)
            FixText_Key = value
        End Set
    End Property

    Public Property P_Replace_Params As String
        Get
            Return Replace_Params
        End Get
        Set(value As String)
            Replace_Params = value
        End Set
    End Property

    Public Property P_Email_FROM As String
        Get
            Return Trim(gl_FPApp.Text_ParametersErsetzen(Email_FROM, Replace_Params, "|"))
        End Get
        Set(value As String)
            Email_FROM = value
        End Set
    End Property

    Public Property P_Email_TO As String
        Get
            Return Trim(Email_TO)   'Mert az email cim @-ot tartalmaz, nem lehet Text_ParametersErsetzen
        End Get
        Set(value As String)
            Email_TO = value
        End Set
    End Property

    Public Property P_Email_CC As String
        Get
            Return Trim(Email_CC)   'Mert az email cim @-ot tartalmaz, nem lehet Text_ParametersErsetzen
        End Get
        Set(value As String)
            Email_CC = value
        End Set
    End Property

    Public Property P_Email_BCC As String
        Get
            Return Trim(Email_BCC)   'Mert az email cim @-ot tartalmaz, nem lehet Text_ParametersErsetzen
        End Get
        Set(value As String)
            Email_BCC = value
        End Set
    End Property

    Public Property P_Email_Text As String
        Get
            Return gl_FPApp.Text_ParametersErsetzen(Email_TEXT, Replace_Params, "|")
        End Get
        Set(value As String)
            Email_TEXT = value
        End Set
    End Property

    Public Property P_Email_Subject As String
        Get
            Return gl_FPApp.Text_ParametersErsetzen(Email_SUBJECT, Replace_Params, "|")
        End Get
        Set(value As String)
            Email_SUBJECT = value
        End Set
    End Property

    Public Property P_Email_Footer_html As String
        Get
            Return Email_Footer_html
        End Get
        Set(value As String)
            Email_Footer_html = value
        End Set
    End Property

    Public Property P_SMTP_Params As STRUCT_FP_EMAIL_SMTP_PARAMS
        Get
            Return SMTP_Connect_Params
        End Get
        Set(value As STRUCT_FP_EMAIL_SMTP_PARAMS)
            SMTP_Connect_Params = value
        End Set
    End Property

    Public Function P_Email_Attached_Files_ADD(FileName As String) As Boolean
        Dim OUT As Boolean = True

        FileName = Trim(FileName)

        If OUT = True Then
            If FileName = "" Then
                OUT = False
            End If
        End If

        If OUT = True Then
            If Email_Attached_files.Contains(FileName) Then
                OUT = False
            End If
        End If

        If OUT = True Then
            If Vorhanden(FileName) = False Then
                OUT = False
            End If
        End If

        If OUT = True Then
            Email_Attached_files.Add(FileName)
        End If

        Return OUT
    End Function

    Public Function Params_SET_FROM_FIXTEXT() As Boolean
        Dim OUT As Boolean = False

        If FixText_Key > "" Then
            Dim EMail_Params_FixText_Key As String = "EMAIL_" + FixText_Key
            Dim Email_Params_FixText_Key_User As String = EMail_Params_FixText_Key + "_" + UserKurzName
            Dim Footer_html_Fixtext_Code As String = ""

            Dim EMail_Params As String = gl_FPApp.getFixText(Email_Params_FixText_Key_User + "_PARAMS", False)
            Email_TEXT = gl_FPApp.Text_ParametersErsetzen(gl_FPApp.getFixText(Email_Params_FixText_Key_User + "_TEXT", False), Replace_Params)

            If EMail_Params = "" Then
                EMail_Params = gl_FPApp.getFixText(EMail_Params_FixText_Key + "_PARAMS", False)
            End If

            If Email_TEXT = "" Then
                Email_TEXT = gl_FPApp.Text_ParametersErsetzen(gl_FPApp.getFixText(EMail_Params_FixText_Key + "_TEXT", False), Replace_Params)
            End If

            Dim P_DIC As Dictionary(Of String, String) = Nothing

            gl_FPApp.FIXTEXT_SPLIT_PARAMS(EMail_Params, P_DIC)
            Email_TO = gl_FPApp.FIXTEXT_getParam("TO", P_DIC)
            Email_CC = gl_FPApp.FIXTEXT_getParam("CC", P_DIC)
            Email_BCC = gl_FPApp.FIXTEXT_getParam("BCC", P_DIC)
            Email_SUBJECT = gl_FPApp.FIXTEXT_getParam("SUBJECT", P_DIC)

            Footer_html_Fixtext_Code = gl_FPApp.FIXTEXT_getParam("FOOTER_FIXTEXT_CODE", P_DIC)
            If Footer_html_Fixtext_Code > "" Then
                Email_Footer_html = gl_FPApp.Text_ParametersErsetzen(gl_FPApp.getFixText(Footer_html_Fixtext_Code, False), Replace_Params)
            End If

            OUT = True
        End If

        Return OUT
    End Function

    Public Function SMTP_PARAMS_SET_FROM_FIXTEXT(Optional FixText_Key As String = "") As Boolean
        Dim OUT As Boolean = False

        If FixText_Key = "" Then
            FixText_Key = "SMTP_PARAMS"
        End If

        If FixText_Key > "" Then
            Dim SMTP_Params_STR As String = gl_FPApp.getFixText(FixText_Key, False)

            If SMTP_Params_STR > "" Then
                Dim P_DIC As Dictionary(Of String, String) = Nothing

                gl_FPApp.FIXTEXT_SPLIT_PARAMS(SMTP_Params_STR, P_DIC)

                With SMTP_Connect_Params
                    .IP = gl_FPApp.FIXTEXT_getParam("IP", P_DIC)
                    .Credential_UserName = gl_FPApp.FIXTEXT_getParam("CREDENTIAL_USERNAME", P_DIC)
                    .Credential_Password = gl_FPApp.FIXTEXT_getParam("CREDENTIAL_PASSWORD", P_DIC)
                    .Email_FROM = gl_FPApp.FIXTEXT_getParam("FROM", P_DIC)
                    .EnableSSL = TEXT_Is_YES(gl_FPApp.FIXTEXT_getParam("SSL", P_DIC))
                    .Port = gl_FPApp.FIXTEXT_getParam("PORT", P_DIC)
                End With

                OUT = True
            End If
        End If

        Return OUT
    End Function


    Public Function Params_CHECK(Optional WithDialog As Boolean = True, Optional CHECK_SMTP_DATA As Boolean = False) As Boolean
        Dim OUT As Boolean = False
        Dim ErrNum As Integer = 0

        If P_Email_TO = "" Then
            ErrNum = 1601 'Nincs megadva a "TO" parameter
        ElseIf P_Email_Subject = "" Then
            ErrNum = 1602 'Nincs megadva a "SUBJECT" parameter
        ElseIf CHECK_SMTP_DATA Then
            With SMTP_Connect_Params
                If .IP = "" Or .Port = "" Or .Email_FROM = "" Then
                    ErrNum = 1603 'Nincs megadva a "FROM" parameter
                Else
                    OUT = True
                End If
            End With
        Else
            OUT = True
        End If

        If OUT = False Then
            If WithDialog = True Then
                gl_FPApp.DoMyMsgBox(ErrNum)
            End If
        End If

        Return OUT
    End Function

    Public Function EMAIL_SEND_via_SMTP(Optional WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = True

        If Params_CHECK(WithDialog, True) Then
            Try
                Dim Mail As New MailMessage
                Dim SMTP As New SmtpClient
                With SMTP
                    .UseDefaultCredentials = SMTP_Connect_Params.Use_Default_Credentials
                    .Credentials = New Net.NetworkCredential(SMTP_Connect_Params.Credential_UserName, SMTP_Connect_Params.Credential_Password)
                    .Port = SMTP_Connect_Params.Port
                    .EnableSsl = SMTP_Connect_Params.EnableSSL
                    .Host = SMTP_Connect_Params.IP
                End With

                Dim Mail_TO_STR() As String = Split(P_Email_TO, ";")
                Dim Mail_CC_STR() As String = Split(P_Email_CC, ";")
                Dim Mail_BCC_STR() As String = Split(P_Email_BCC, ";")

                For Each ToAddr As String In Mail_TO_STR
                    If Trim(ToAddr) > "" Then
                        Mail.To.Add(ToAddr)
                    End If
                Next

                For Each CCAddr As String In Mail_CC_STR
                    If Trim(CCAddr) > "" Then
                        Mail.CC.Add(CCAddr)
                    End If
                Next

                For Each BCCAddr As String In Mail_BCC_STR
                    If Trim(BCCAddr) > "" Then
                        Mail.Bcc.Add(BCCAddr)
                    End If
                Next

                With Mail
                    Mail.From = New MailAddress(SMTP_Connect_Params.Email_FROM)
                    Mail.Subject = P_Email_Subject
                    Mail.IsBodyHtml = True
                    Mail.Body = P_Email_Text
                    If Email_Attached_files.Count > 0 Then
                        For Each cFileName In Email_Attached_files
                            Try
                                Dim A As New Attachment(cFileName)
                                .Attachments.Add(A)
                            Catch ex As Exception
                                'Nothing to do
                            End Try
                        Next
                    End If
                End With
                With SMTP
                    .Send(Mail)
                End With

            Catch ex As Exception
                gl_FPApp.DoErrorMsgBox("FP_MAIL.EMAIL_SEND_via_SMTP", Err.Number, Err.Description)
            End Try
        End If

        Return OUT
    End Function

    Public Function EMAIL_SEND_via_MAILTO(Optional WithDialog As Boolean = True) As Boolean
        'Egyszeru e-mail kuldes a MAILTO formatummal. (nem lehet mellekletet kuldeni, de outlook nelkul is mukodik)

        Dim OUT As Boolean = False

        If Params_CHECK(WithDialog) = True Then
            Dim MAILTO_Email_Text As String = Email_TEXT

            MAILTO_Email_Text = Replace(Email_TEXT, vbCrLf, "%0D%0A")
            MAILTO_Email_Text = Replace(Email_TEXT, vbCr, "%0D%0A")
            MAILTO_Email_Text = Replace(Email_TEXT, vbLf, "%0D%0A")

            Try
                Process.Start(String.Format("mailto:{0}?subject={1}&body={2}", Email_TO, Email_SUBJECT, Email_TEXT))
                OUT = True

            Catch ex As System.Exception
                gl_FPApp.DoErrorMsgBox("FP_MAIL.EMAIL_SEND_via_MAILTO", Err.Number, Err.Description)
            End Try
        End If

        Return OUT
    End Function

    Public Function EMAIL_SEND_via_OUTLOOK(Action_Type As ENUM_FP_EMAIL_HANDLING) As Boolean
        Dim OUT As Boolean = True

        Try
            Dim Outlook_App As New Outlook.Application
            Dim OutlookMessage As Outlook.MailItem
            OutlookMessage = Outlook_App.CreateItem(Outlook.OlItemType.olMailItem)
            With OutlookMessage
                .BodyFormat = Outlook.OlBodyFormat.olFormatHTML
                .Subject = P_Email_Subject
                .To = P_Email_TO
                .CC = P_Email_CC
                .BCC = P_Email_BCC
                .Body = P_Email_Text
                .Importance = Outlook.OlImportance.olImportanceNormal

                If Email_Attached_files.Count > 0 Then
                    For Each cFileName In Email_Attached_files
                        Try
                            .Attachments.Add(cFileName)

                        Catch ex As Exception
                            'Nothing to do
                        End Try
                    Next
                End If

                If Email_Footer_html > "" Then
                    .HTMLBody = Email_Footer_html
                End If

                '.Save()

                Select Case Action_Type
                    Case ENUM_FP_EMAIL_HANDLING.SEND
                        .Send()

                    Case ENUM_FP_EMAIL_HANDLING.SHOW
                        .Display()

                    Case Else
                        gl_FPApp.DoErrorMsgBox("FP_EMAIL.EMAIL_SEND_via_OUTLOOK", 0, String.Format("Unknown action type ({0})", Action_Type))
                        OUT = False
                End Select
            End With


        Catch ex As System.Exception
            gl_FPApp.DoErrorMsgBox("FP_EMAIL.EMAIL_SEND_via_OUTLOOK", Err.Number, Err.Description)
        End Try
        Return OUT
    End Function
End Class
