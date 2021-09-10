Public Class CL_Panel

    Public Structure STRUCT_CL_Panel
        Dim CreditLimit_Panel As Panel
        Dim FP_CreditLimit_SubPrefix As String
        Dim FieldPrefix As String
        Dim FP_of_Customer As FP
        Dim FPc_Customer As FP_Control
        Dim Name_of_Field_of_Customer_ID As String 'Csak akkor kell, ha az FPc_Customer nincs megadva
    End Structure

    Public WithEvents FP_CreditLimit As FP
    Public WithEvents FPf As FP_Form
    Public WithEvents CreditLimit_Panel As Panel
    Public WithEvents FP_of_Customer As FP
    Public Name_of_Field_of_Customer_ID As String
    Public WithEvents FPc_Customer As FP_Control
    Private Control_Prefix = "CL_"
    Public FP_CreditLimit_SubPrefix As String
    Public FieldPrefix As String
    Private Disposed As Boolean = False

    Private Control_Creditlimit_Info_Rtf As RichTextBox
    Private BTN_Detail As PictureBox

    Public WithEvents FPc_CL_INFO As FP_Control
    Public WithEvents FPp_BTN_Detail As FP_PictureBox

    Public Sub New(P As STRUCT_CL_Panel)
        With P
            CreditLimit_Panel = .CreditLimit_Panel
            If Not (.FPc_Customer Is Nothing) Then
                FPc_Customer = .FPc_Customer
                FP_of_Customer = FPc_Customer.FP
            Else
                Name_of_Field_of_Customer_ID = .Name_of_Field_of_Customer_ID
                FP_of_Customer = .FP_of_Customer
            End If
            FP_CreditLimit_SubPrefix = .FP_CreditLimit_SubPrefix
        End With
        FPf = FP_of_Customer.FPf
        FP_CreditLimit = New FP(FPf, "FP_CL_PANEL")
        With FP_CreditLimit
            .P_FORM_AllowAdditions = False
            .P_FORM_AllowDeletions = False
            .P_FORM_AllowEdits = False

            With .SQL_BIND_Params
                .NameOf_SAVE = ""
                .NameOf_DEL = ""
            End With
        End With

        Dim FP_CreditLimit_P As New Struct_FP_CONTROLS_COLLECTION

        With FP_CreditLimit_P
            .FieldPrefix = Control_FieldPrefix_And_Prefix()
        End With

        CREATE_CONTROLS()
        FP_CreditLimit.INIT_CONTROLS(FP_CreditLimit_P)

        Refresh_Credit_Info(True)
    End Sub

    Public Function Control_FieldPrefix_And_Prefix() As String
        Dim OUT As String = Control_Prefix

        If FieldPrefix > "" Then
            OUT = FieldPrefix + OUT
        End If

        Return OUT
    End Function

    Private Sub CREATE_CONTROLS()
        If Not Disposed Then
            Control_Creditlimit_Info_Rtf = New RichTextBox
            With Control_Creditlimit_Info_Rtf
                .Name = Control_FieldPrefix_And_Prefix() + "CL_INFO"
                .Parent = CreditLimit_Panel
                .Visible = True
                .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            End With
            FPf.CONTROLS_ADD(Control_Creditlimit_Info_Rtf)

            BTN_Detail = New PictureBox
            With BTN_Detail
                .Name = Control_FieldPrefix_And_Prefix() + "CL_BTN_Detail"
                .Parent = CreditLimit_Panel
                .Size = New Size(44, 44)
                .Visible = True
            End With
            FPf.CONTROLS_ADD(BTN_Detail)
        End If
    End Sub

    Public Sub Dispose()
        If Disposed = False Then
            Disposed = True

            CreditLimit_Panel = Nothing
            FP_of_Customer = Nothing

            FPc_CL_INFO = Nothing
            FPp_BTN_Detail = Nothing
            FP_CreditLimit.Dispose()
            FP_CreditLimit = Nothing

            If Not (Control_Creditlimit_Info_Rtf Is Nothing) Then
                If FPf.CONTROLS.Keys.Contains(Control_Creditlimit_Info_Rtf.Name) Then
                    FPf.CONTROLS_REMOVE(Control_Creditlimit_Info_Rtf.Name)
                End If
                Control_Creditlimit_Info_Rtf.Dispose()
                Control_Creditlimit_Info_Rtf = Nothing
            End If

            FPf = Nothing
        End If
    End Sub

    Public ReadOnly Property P_CustID_GET As Long
        Get
            Dim OUT As Long = 0

            If Disposed = False Then
                If FPc_Customer Is Nothing Then
                    If FP_of_Customer.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
                        If Name_of_Field_of_Customer_ID = "ID" Then
                            OUT = FP_of_Customer.P_DATA_Current_ID
                        Else
                            OUT = Val(FP_of_Customer.DATA_Field_getSavedValue(Name_of_Field_of_Customer_ID))
                        End If
                    End If
                Else
                    If TypeOf (FPc_Customer.c) Is ComboBox Then
                        OUT = FPc_Customer.P_VALUE
                    Else
                        OUT = FPc_Customer.Selected_ID
                    End If
                End If
            End If

            Return OUT
        End Get
    End Property

    Public Sub Refresh_Credit_Info(Allways_Refresh As Boolean)
        Static Last_Customer_ID As Long = 0

        If CreditLimit_Panel.Visible = False Then
            If Allways_Refresh Then
                Last_Customer_ID = 0 'Majd ha lathato lesz a panel, akkor majd frissulni fog.
            End If
        Else
            Dim CustID As Long = P_CustID_GET
            Dim Crit As String = String.Format(String.Format("ID = {0}", CustID))

            FP_CreditLimit.FORM_RECORDS_LOAD(Crit)
        End If
    End Sub

    Private Sub FP_of_Customer_Form_Current(sender_FP As FP) Handles FP_of_Customer.Form_Current
        Refresh_Credit_Info(False)
    End Sub

    Private Sub FP_of_Customer_Form_Field_AfterUpdate(FPc As FP_Control) Handles FP_of_Customer.Form_Field_AfterUpdate
        Refresh_Credit_Info(False)
    End Sub

    Private Sub FP_of_Customer_Form_NoRecord(sender_FP As FP) Handles FP_of_Customer.Form_NoRecord
        Refresh_Credit_Info(False)
    End Sub

    Private Sub CreditLimit_Panel_VisibleChanged(sender As Object, e As EventArgs) Handles CreditLimit_Panel.VisibleChanged
        Refresh_Credit_Info(False)
    End Sub

    Private Sub FP_CreditLimit_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_CreditLimit.CONTROLS_INITIALIZED
        FPc_CL_INFO = FP_CreditLimit.CONTROLS("CL_INFO")
        FPp_BTN_Detail = FP_CreditLimit.PICTUREBOXES("CL_BTN_Detail")

        FPc_CL_INFO.P_Marker = FP_Control.ENUM_Markertypes.Right_Arrow
    End Sub

    Private Sub FPp_BTN_Detail_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPp_BTN_Detail.CLICK
        Dim Current_Cust_ID As Long = P_CustID_GET

        If Current_Cust_ID <> 0 Then
            Dim SEL_CL_DETAIL As New SEL_CL_DETAIL(Current_Cust_ID)
            gl_FPApp.ShowDialogForm(SEL_CL_DETAIL, FPf)
        End If
    End Sub

    Private Sub FPc_CL_INFO_Field_Marker_Click(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Handled As Boolean) Handles FPc_CL_INFO.Field_Marker_Click, FPc_CL_INFO.Field_Doubleclick
        FPp_BTN_Detail_CLICK(Nothing, Nothing)
    End Sub

End Class
