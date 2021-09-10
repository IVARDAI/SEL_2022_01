Public Class FP_MsgBox
    Private FPApp As FP_App
    Private FPf As FP_Form
    Private DialNr As Long = 0
    Private IsOk As Boolean
    Private ModuleIdentifier As String

    Sub New(ByVal MyFPApp As FP_App, ByVal MyModuleIdentifier As String, ByVal MyDialNr As Long, Optional ByVal Ersetz As String = "", Optional ByVal Button1 As String = "SEQ,OK", Optional ByVal Button2 As String = "", Optional ByVal Button3 As String = "")
        InitializeComponent()

        FPApp = MyFPApp
        ModuleIdentifier = MyModuleIdentifier

        Try
            If Not FPApp.DC.CNN_IsConnected Then
                Me.DialogResult = Windows.Forms.DialogResult.None
                MsgBox("Not connected to database. DialNr=" & DialNr & vbCrLf)
                IsOk = False
                Me.Close()
                Exit Sub
            End If

            DialNr = MyDialNr

            If Button1.Trim = "" Then
                Button1 = Button2
                Button2 = Button3
                Button3 = ""
            End If

            If Button2.Trim = "" Then
                Button2 = Button3
                Button3 = ""
            End If

            Dim MySQL As String = ""
            Dim DT As New DataTable
            Dim Texts() As String
            Dim LinkText As String = ""
            Dim URL As String = ""

            If ModuleIdentifier = "" Then
                Me.Text = FPApp.Text_TextVonSEQ(Me.Text) + String.Format(" (DialNo: {0})", DialNr)

                MySQL = String.Format("SELECT Number, Text1, Text2 FROM VB_SEQ WHERE [Language] = '{0}' And SEQ_Key = {1}  ORDER BY Number", FPApp.LandDialog, "'DIALOG" + Format(DialNr, "000000") + "'")

                If FPApp.DC.P_USE_LocalDB Then
                    FPApp.DC.LocalDB_SEL.Fill_DT(MySQL, DT)
                Else
                    FPApp.DC.Qdf_Fill_DT(MySQL, DT)
                End If

                ReDim Preserve Texts(4)

                If DT.Rows.Count > 0 Then
                    For i = 0 To DT.Rows.Count - 1
                        If DT.Rows(i)!Number <> 99 Then
                            Texts(i) = FPApp.Text_ParametersErsetzen(DT.Rows(i)!Text1, Ersetz)
                        Else
                            LinkText = DT.Rows(i)!Text1
                            URL = DT.Rows(i)!Text2
                        End If
                    Next
                End If
            Else
                Me.Text = FPApp.Text_TextVonSEQ(Me.Text) + String.Format(" (DialNo: {0}.{1})", ModuleIdentifier, DialNr)

                MySQL = String.Format("SELECT VALUE FROM RS_Texts{0}_View WHERE LANG = '{1}' And MODULE = '{2}' And ServerObject_Prefix = '' AND SubPrefix = '' AND IDX = {3} And GROUP_Code = 'DIALOG' And CTRL_Code = '' And SUB_Code = ''", IIf(FPApp.Is_DEBUG_MODE, "_DEBUG", ""), FPApp.LandDialog, ModuleIdentifier, MyDialNr)

                If FPApp.DC.P_USE_LocalDB Then
                    FPApp.DC.LocalDB_SEL.Fill_DT(MySQL, DT)
                Else
                    FPApp.DC.Qdf_Fill_DT(MySQL, DT)
                End If

                If DT.Rows.Count = 1 Then
                    Texts = Split(FPApp.Text_ParametersErsetzen(DT.Rows(0)!VALUE, Ersetz), "|")
                End If

                ReDim Preserve Texts(4)
            End If

            Me.Titel.Text = Texts(0)
            Me.Message_Text.Text = Texts(1) + vbCrLf + Texts(2) + vbCrLf + Texts(3)
            Me.LinkLabel.Text = LinkText
            Me.LinkLabel.Links.Add(0, Len(LinkText), URL)

            Me.Taste1.Text = FPApp.Text_TextVonSEQ(Button1)
            Me.Taste2.Text = FPApp.Text_TextVonSEQ(Button2)
            Me.Taste3.Text = FPApp.Text_TextVonSEQ(Button3)

            IsOk = True

        Catch ex As Exception
            MsgBox("Error in Sel_MyMsgBox.New. " & vbCrLf & "ErrorCode: " & Err.Number & vbCrLf & ". " & Err.Description)
            Exit Sub
        End Try
    End Sub

    Public ReadOnly Property P_DialNr()
        Get
            P_DialNr = DialNr
        End Get
    End Property

    Public ReadOnly Property P_IsOK() As Boolean
        Get
            Return IsOk
        End Get
    End Property

    Private Sub Taste1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Taste1.Click
        Try
            Me.DialogResult = 1
            Me.Close()

        Catch ex As Exception
            Me.DialogResult = System.Windows.Forms.DialogResult.None
            FPApp.DoErrorMsgBox("Sel_MyMsgBox.Taste1_Click", Err.Number, Err.Description)
            Me.Close()
        End Try
    End Sub

    Private Sub Taste2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Taste2.Click
        Try
            Me.DialogResult = 2
            Me.Close()

        Catch ex As Exception
            Me.DialogResult = System.Windows.Forms.DialogResult.None
            FPApp.DoErrorMsgBox("Sel_MyMsgBox.Taste2_Click", Err.Number, Err.Description)
            Me.Close()
        End Try
    End Sub

    Private Sub Taste3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Taste3.Click
        Try
            Me.DialogResult = 3
            Me.Close()

        Catch ex As Exception
            Me.DialogResult = System.Windows.Forms.DialogResult.None
            FPApp.DoErrorMsgBox("Sel_MyMsgBox.Taste3_Click", Err.Number, Err.Description)
            Me.Close()
        End Try

    End Sub

    Private Sub FP_MsgBox_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Dim TasteTop As Long
        Dim CountOfButtons As Long = 0

        Try
            Me.Taste1.Visible = (Me.Taste1.Text > "")
            Me.Taste2.Visible = (Me.Taste2.Text > "")
            Me.Taste3.Visible = (Me.Taste3.Text > "")

            CountOfButtons = IIf(Me.Taste1.Visible, 1, 0) + IIf(Me.Taste2.Visible, 1, 0) + IIf(Me.Taste3.Visible, 1, 0)
            Select Case CountOfButtons
                Case 1 : Me.Taste1.Left = Me.Taste2.Left
                Case 2 : Me.Taste2.Left = Me.Taste3.Left
                Case 3 'Muss nichts tun
            End Select
            Me.Message_Text.Height = Me.Message_Text.Font.Height() * (3 + FPApp.Text_getAnzahlReihen(Me.Message_Text.Text)) '8+2=FontSize+Zeilenzwischenpunkte
            TasteTop = Me.Message_Text.Top + Me.Message_Text.Height

            Me.Taste1.Top = TasteTop
            Me.Taste2.Top = TasteTop
            Me.Taste3.Top = TasteTop
            Me.LinkLabel.Top = TasteTop + Me.Taste1.Height

            Me.Height = Me.Taste1.Top + Me.Taste1.Height + IIf(Me.LinkLabel.Text <> "", Me.LinkLabel.Height + 5, 0) + 40

            If Me.Taste1.Visible = False And Me.Taste2.Visible = False And Me.Taste3.Visible = False Then
                Me.DialogResult = System.Windows.Forms.DialogResult.None
                Me.Close()
                Exit Sub
            End If

        Catch ex As Exception
            FPApp.DoErrorMsgBox("Sel_MyMsgBox.Form_Shown", Err.Number, Err.Description)
            Exit Sub
        End Try
    End Sub

    Private Sub LinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel.LinkClicked
        Dim i As Integer = 0

        Process.Start(e.Link.LinkData.ToString)

        i += IIf(Me.Taste1.Visible = True, 1, 0)
        i += IIf(Me.Taste2.Visible = True, 1, 0)
        i += IIf(Me.Taste3.Visible = True, 1, 0)

        If i = 1 Then
            Close()
        End If
    End Sub

End Class
