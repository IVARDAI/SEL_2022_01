Public Class FP_Services_DebugInfoPanel

#Region "DECLARE"

    Public FPApp As FP_App
    Public FPf As FP_Form
    Public WithEvents frm As Form

    Private Hover As Boolean
    Private Drag As Boolean
    Private Mouse_X As Integer
    Private Mouse_Y As Integer

#End Region

#Region "FORM CLASS"

    Public Sub New(ByVal MyFPf As FP_Form)
        InitializeComponent()

        FPf = MyFPf
        frm = MyFPf.Frm
        FPApp = FPf.FPApp

        Me.Show()

        ADD_Handlers(frm)
    End Sub

#End Region

    Public Sub Dispose_ME()
        If Not (frm Is Nothing) Then
            REMOVE_Handlers(frm)
            FPf.InfoPanel = Nothing
        End If
    End Sub

#Region "FORM EVENTS"

    Private Sub FP_DebugInfoPanel_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Dispose_ME()
    End Sub

    Private Sub FP_DebugInfoPanel_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape And Hover = True Then
            Close()
        End If
    End Sub


    Private Sub FP_DebugInfoPanel_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim c As Control

        For Each c In Controls
            AddHandler c.MouseEnter, AddressOf FP_DebugInfoPanel_MouseEnter
            AddHandler c.MouseDown, AddressOf FP_DebugInfoPanel_MouseDown
            AddHandler c.MouseMove, AddressOf FP_DebugInfoPanel_MouseMove
            AddHandler c.MouseUp, AddressOf FP_DebugInfoPanel_MouseUp
        Next
        ConnectionString.Text = FPApp.DC.CNN.ConnectionString
        TerminalString.Text = Terminal
        Titel_Label.Text = Titel_Label.Text.Replace("#FORM", frm.Name)
    End Sub

    Private Sub FP_DebugInfoPanel_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter
        Focus()
        Opacity = 1
    End Sub

    Private Sub FP_DebugInfoPanel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If TypeOf DirectCast(sender, Control) Is TextBox Then
                DirectCast(sender, TextBox).ContextMenu = New ContextMenu 'Azert kell ide, hogy ne ugorjon fel a textbox jobb klikkre a menu
            End If

            Drag = True
            Mouse_X = Cursor.Position.X - Me.Left
            Mouse_Y = Cursor.Position.Y - Me.Top
        End If
    End Sub

    Private Sub FP_DebugInfoPanel_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If Drag Then
            Me.Top = Cursor.Position.Y - Mouse_Y
            Me.Left = Cursor.Position.X - Mouse_X
        End If
    End Sub

    Private Sub FP_DebugInfoPanel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        Drag = False
    End Sub

#End Region

#Region "FP FORM EVENTS"

    Private Sub frm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles frm.FormClosing
        Dispose_ME()
    End Sub

    Private Sub frm_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles frm.MouseHover
        Dim c As Control = DirectCast(sender, Control)
        Dim FPo As FP_ControlObject

        ControlName.Text = ""
        ParentControlName.Text = ""
        ControlType.Text = ""
        FP_Name.Text = ""
        RecordID.Text = 0
        RS_ID.Text = 0
        ServerObjectPrefix.Text = ""
        Subprefix.Text = ""
        FPo = Nothing

        ControlName.Text = c.Name

        If c.Parent IsNot Nothing Then
            ParentControlName.Text = c.Parent.Name
        End If

        ControlType.Text = c.GetType.ToString

        If FPf.CONTROLS_GET_FPo_FROM_CONTROL(c, FPo) Then
            If (FPo.FP Is Nothing) Then
                FP_Name.Text = ""
                RecordID.Text = ""
                RS_ID.Text = ""

                ServerObjectPrefix.Text = FPo.FPf.ServerObject_Prefix
                Subprefix.Text = ""
            Else
                With FPo.FP
                    FP_Name.Text = .ServerObject_Prefix
                    RecordID.Text = .P_DATA_Current_ID.ToString
                    RS_ID.Text = .RS_ID.ToString
                    ServerObjectPrefix.Text = .ServerObject_Prefix
                    Subprefix.Text = .SubPrefix
                End With
            End If
        End If
    End Sub

#End Region

#Region "PRIVATE SUBS"

    Private Sub ADD_Handlers(ByVal ParentControl As Control)
        Dim c As Control

        For Each c In ParentControl.Controls
            AddHandler c.MouseHover, AddressOf frm_MouseHover
            ADD_Handlers(c)
        Next
    End Sub

    Private Sub REMOVE_Handlers(ByVal ParentControl As Control)
        Dim c As Control

        For Each c In ParentControl.Controls
            RemoveHandler c.MouseHover, AddressOf frm_MouseHover
            REMOVE_Handlers(c)
        Next
    End Sub
#End Region
End Class