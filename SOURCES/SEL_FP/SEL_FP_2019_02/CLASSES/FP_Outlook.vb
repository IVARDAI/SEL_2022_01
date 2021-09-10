Imports Microsoft.Office.Interop.Outlook

Public Class FP_Outlook

    Public Structure Struct_OutlookEmail
        Dim Subject As String
        Dim Body As String
        Dim Importance As OlImportance
    End Structure

    Dim OutlookApplication As Application
    Dim OutlookMessage As MailItem

    Dim lRecipients As New List(Of String)
    Dim lCC As New List(Of String)
    Dim lBCC As New List(Of String)
    Dim Attachments As New List(Of String)
    Dim Subject As String = ""
    Dim Body As String = ""
    Dim Importance As OlImportance = OlImportance.olImportanceNormal

    Public Sub New(Optional Struct_OutlookEmail As Struct_OutlookEmail = Nothing)
        OutlookApplication = New Application
        OutlookMessage = OutlookApplication.CreateItem(OlItemType.olMailItem)

        Subject = Struct_OutlookEmail.Subject
        Body = Struct_OutlookEmail.Body
        Importance = Struct_OutlookEmail.Importance
    End Sub

    Public Sub AddAttachment(FilePath As String)
        Attachments.Add(FilePath)
    End Sub

    Public Sub AddRecipients(EmailAddress As String)
        lRecipients.Add(EmailAddress)
    End Sub

    Public Sub AddCC(EmailAddress As String)
        lCC.Add(EmailAddress)
    End Sub

    Public Sub AddBCC(EmailAddress As String)
        lBCC.Add(EmailAddress)
    End Sub

    Public Sub ShowEmail()
        If INIT() Then
            OutlookMessage.Display()
        End If
    End Sub

    Public Sub SendEmail()
        If INIT() Then
            OutlookMessage.Send()
        End If
    End Sub

    Private Function INIT() As Boolean
        Dim Recipients As Recipients = Nothing
        Dim MailTo As Recipient = Nothing
        Dim MailCC As Recipient = Nothing
        Dim MailBCC As Recipient = Nothing

        Try
            With OutlookMessage
                Recipients = OutlookMessage.Recipients

                'Cimzett
                For Each MailAddress As String In lRecipients
                    MailTo = Recipients.Add(MailAddress)
                    MailTo.Type = OlMailRecipientType.olTo
                Next
                If Not IsNothing(MailTo) Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(MailTo)
                End If

                'Masolat
                For Each MailAddress As String In lCC
                    MailCC = Recipients.Add(MailAddress)
                    MailCC.Type = OlMailRecipientType.olCC
                Next
                If Not IsNothing(MailCC) Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(MailCC)
                End If

                'Titkos masolat
                For Each MailAddress As String In lBCC
                    MailBCC = Recipients.Add(MailAddress)
                    MailBCC.Type = OlMailRecipientType.olBCC
                Next
                If Not IsNothing(MailBCC) Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(MailBCC)
                End If

                'Targy
                .Subject = Subject

                'Melleklet
                For Each Attachment As String In Attachments
                    .Attachments.Add(Attachment, OlAttachmentType.olByValue)
                Next

                'Email szovege
                .HTMLBody = Body

                'Megjelelos fontoskent
                .Importance = Importance
            End With

        Catch ex As System.Exception
            INIT = False
            Exit Function
        End Try

        INIT = True
    End Function
End Class
