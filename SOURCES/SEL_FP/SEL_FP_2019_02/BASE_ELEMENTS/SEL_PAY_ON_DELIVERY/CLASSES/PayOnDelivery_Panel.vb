
Public Class PayOnDelivery_Panel

    Public FPf As FP_Form
    Public WithEvents FP_Parent As FP

    Private Fieldprefix As String = ""

    Private Amount_label As Label
    Private Amount As TextBox
    Private Curr_ID_Label As Label
    Private Curr_ID As ComboBox
    Private Paid_Label As Label
    Private Paid As CheckBox
    Private Descr_Label As Label
    Private Descr As TextBox
    Private Incoming_Date_Label As Label
    Private Incoming_Date As TextBox

    Public PayOnDelivery_Panel As Panel

    Public FPc_PayOnDelivery_Amount As FP_Control
    Public FPc_PayOnDelivery_Curr_ID As FP_Control
    Public FPc_PayOnDelivery_Paid As FP_Control
    Public FPc_PayOnDelivery_Descr As FP_Control
    Public FPc_PayOnDelivery_Incoming_Date As FP_Control

    Private Const Control_Prefix = "PayOnDelivery_"

    Private SEQ As New FP_SEQ(gl_FPApp, "VBSEQ_PAYONDELIVERY")

    Public Structure Struct_PayOnDelivery_Panel_CONTROL_COLLECTION
        Dim FP_Parent As FP
        Dim PayOnDelivery_Panel As Panel

        Dim Fieldprefix As String
        Dim Subprefix As String
    End Structure

    Public Sub New(Params As Struct_PayOnDelivery_Panel_CONTROL_COLLECTION)
        With Params
            FP_Parent = .FP_Parent
            PayOnDelivery_Panel = .PayOnDelivery_Panel
            Fieldprefix = .Fieldprefix
        End With

        FPf = FP_Parent.FPf

        Amount_label = New Label
        With Amount_label
            .Name = Control_FieldPrefix_And_Prefix() + "Amount_Label"
            .Visible = True
            .Width = 20
            .BackColor = Color.DimGray
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            .Size = New Size(120, 22)
            .TextAlign = ContentAlignment.MiddleLeft
        End With
        PayOnDelivery_Panel.Controls.Add(Amount_label)

        Amount = New TextBox
        With Amount
            .Name = Control_FieldPrefix_And_Prefix() + "Amount"
            .BackColor = Color.White
            .Width = 207
            .SendToBack()
            .Visible = True
        End With
        PayOnDelivery_Panel.Controls.Add(Amount)

        Curr_ID_Label = New Label
        With Curr_ID_Label
            .Name = Control_FieldPrefix_And_Prefix() + "Curr_ID_Label"
            .Visible = True
            .Width = 20
            .BackColor = Color.DimGray
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            .Size = New Size(120, 22)
            .TextAlign = ContentAlignment.MiddleLeft
        End With
        PayOnDelivery_Panel.Controls.Add(Curr_ID_Label)

        Curr_ID = New ComboBox
        With Curr_ID
            .Name = Control_FieldPrefix_And_Prefix() + "Curr_ID"
            .BackColor = Color.White
            .Width = 207
            .SendToBack()
            .Visible = True
        End With
        PayOnDelivery_Panel.Controls.Add(Curr_ID)

        Descr_Label = New Label
        With Descr_Label
            .Name = Control_FieldPrefix_And_Prefix() + "Descr_Label"
            .Visible = True
            .Width = 20
            .BackColor = Color.DimGray
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            .Size = New Size(120, 22)
            .TextAlign = ContentAlignment.MiddleLeft
        End With
        PayOnDelivery_Panel.Controls.Add(Descr_Label)

        Descr = New TextBox
        With Descr
            .Name = Control_FieldPrefix_And_Prefix() + "Descr"
            .BackColor = Color.White
            .Width = 207
            .SendToBack()
            .Visible = True
        End With
        PayOnDelivery_Panel.Controls.Add(Descr)

        Paid_Label = New Label
        With Paid_Label
            .Name = Control_FieldPrefix_And_Prefix() + "Paid_Label"
            .Visible = True
            .Width = 20
            .BackColor = Color.DimGray
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            .Size = New Size(120, 22)
            .TextAlign = ContentAlignment.MiddleLeft
        End With
        PayOnDelivery_Panel.Controls.Add(Paid_Label)

        Paid = New CheckBox
        With Paid
            .Name = Control_FieldPrefix_And_Prefix() + "Paid"
            .BackColor = Color.White
            .Width = 207
            .SendToBack()
            .Visible = True
        End With
        PayOnDelivery_Panel.Controls.Add(Paid)

        Incoming_Date_Label = New Label
        With Incoming_Date_Label
            .Name = Control_FieldPrefix_And_Prefix() + "Incoming_Date_Label"
            .Visible = True
            .Width = 20
            .BackColor = Color.DimGray
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            .Size = New Size(120, 22)
            .TextAlign = ContentAlignment.MiddleLeft
        End With
        PayOnDelivery_Panel.Controls.Add(Incoming_Date_Label)

        Incoming_Date = New TextBox
        With Incoming_Date
            .Name = Control_FieldPrefix_And_Prefix() + "Incoming_Date"
            .BackColor = Color.White
            .Width = 207
            .SendToBack()
            .Visible = True
        End With
        PayOnDelivery_Panel.Controls.Add(Incoming_Date)

        FPc_PayOnDelivery_Amount = New FP_Control(FP_Parent, Amount, Amount_label, False, Amount.Name, Amount_label.Name)
        FPc_PayOnDelivery_Amount = FP_Parent.CONTROLS(Control_FieldPrefix_And_Prefix() + "Amount")
        FPc_PayOnDelivery_Amount.P.xType_VB = "FLOAT"
        FPc_PayOnDelivery_Amount.c_Label.Text = SEQ.GET_SEQ_BY_TEXT1("Amount_Label").Text3

        FPc_PayOnDelivery_Curr_ID = New FP_Control(FP_Parent, Curr_ID, Curr_ID_Label, False, Curr_ID.Name, Curr_ID_Label.Name)
        FPc_PayOnDelivery_Curr_ID = FP_Parent.CONTROLS(Control_FieldPrefix_And_Prefix() + "Curr_ID")
        FPc_PayOnDelivery_Curr_ID.P.xType_VB = "INT"
        FPc_PayOnDelivery_Curr_ID.P.DT_FixText_Key = "@@VB_COMBO_CURRENCIES"
        FPc_PayOnDelivery_Curr_ID.c_Label.Text = SEQ.GET_SEQ_BY_TEXT1("Curr_ID_Label").Text3

        FPc_PayOnDelivery_Descr = New FP_Control(FP_Parent, Descr, Descr_Label, False, Descr.Name, Descr_Label.Name)
        FPc_PayOnDelivery_Descr = FP_Parent.CONTROLS(Control_FieldPrefix_And_Prefix() + "Descr")
        FPc_PayOnDelivery_Descr.P.xType_VB = ""
        FPc_PayOnDelivery_Descr.c_Label.Text = SEQ.GET_SEQ_BY_TEXT1("Descr_Label").Text3

        FPc_PayOnDelivery_Paid = New FP_Control(FP_Parent, Paid, Paid_Label, False, Paid.Name, Paid_Label.Name)
        FPc_PayOnDelivery_Paid = FP_Parent.CONTROLS(Control_FieldPrefix_And_Prefix() + "Paid")
        FPc_PayOnDelivery_Paid.P.xType_VB = "BIT"
        FPc_PayOnDelivery_Paid.c_Label.Text = SEQ.GET_SEQ_BY_TEXT1("Paid_Label").Text3

        FPc_PayOnDelivery_Incoming_Date = New FP_Control(FP_Parent, Incoming_Date, Incoming_Date_Label, False, Incoming_Date.Name, Incoming_Date_Label.Name)
        FPc_PayOnDelivery_Incoming_Date = FP_Parent.CONTROLS(Control_FieldPrefix_And_Prefix() + "Incoming_Date")
        FPc_PayOnDelivery_Incoming_Date.P.xType_VB = "DATETIME"
        FPc_PayOnDelivery_Incoming_Date.c_Label.Text = SEQ.GET_SEQ_BY_TEXT1("Incoming_Date_Label").Text3

        With FPf
            .ARRANGE_LEFTS(FPc_PayOnDelivery_Amount.c_Label, PayOnDelivery_Panel)
            .ARRANGE_TOPS(FPc_PayOnDelivery_Amount.c_Label, PayOnDelivery_Panel)
            .SIZE_WIDTH_TO(FPc_PayOnDelivery_Amount.c_Label, PayOnDelivery_Panel,,, 20)

            .ARRANGE_ON_RIGHT_TOP(FPc_PayOnDelivery_Amount.c, FPc_PayOnDelivery_Amount.c_Label)
            .SIZE_WIDTH_TO(FPc_PayOnDelivery_Amount.c, PayOnDelivery_Panel,,, 30)

            .ARRANGE_AS_NEXT_ROW(FPc_PayOnDelivery_Curr_ID.c_Label, FPc_PayOnDelivery_Amount.c_Label, Nothing, 0, 1)
            .ARRANGE_AS_NEXT_ROW(FPc_PayOnDelivery_Curr_ID.c, FPc_PayOnDelivery_Amount.c, Nothing, 0, 1)

            .ARRANGE_AS_NEXT_ROW(FPc_PayOnDelivery_Descr.c_Label, FPc_PayOnDelivery_Curr_ID.c_Label, Nothing, 0, 1)
            .ARRANGE_AS_NEXT_ROW(FPc_PayOnDelivery_Descr.c, FPc_PayOnDelivery_Curr_ID.c, Nothing, 0, 1)

            .ARRANGE_AS_NEXT_ROW(FPc_PayOnDelivery_Paid.c_Label, FPc_PayOnDelivery_Descr.c_Label, Nothing, 0, 1)
            .ARRANGE_AS_NEXT_ROW(FPc_PayOnDelivery_Paid.c, FPc_PayOnDelivery_Descr.c, Nothing, 0, 1)

            .ARRANGE_AS_NEXT_ROW(FPc_PayOnDelivery_Incoming_Date.c_Label, FPc_PayOnDelivery_Paid.c_Label, Nothing, 0, 1)
            .ARRANGE_AS_NEXT_ROW(FPc_PayOnDelivery_Incoming_Date.c, FPc_PayOnDelivery_Paid.c, Nothing, 0, 1)
        End With
    End Sub

    Public Function Control_FieldPrefix_And_Prefix() As String
        Dim OUT As String = Control_Prefix

        If Fieldprefix > "" Then
            OUT = String.Format("{0}_{1}", Fieldprefix, OUT)
        End If

        Return OUT
    End Function

End Class
