Imports System.Data
Imports System.Data.SqlClient

Public Class FP_Menu

    Public FPApp As FP_App = Nothing
    Public WithEvents FPf As FP_Form = Nothing
    Public Parent_FPf As FP_Form = Nothing
    Public FP As FP = Nothing
    Public MenuNo As Integer = 0
    Public SubPrefix As String = ""
    Public Menu_State As Enum_Menu_State = Enum_Menu_State.Hided
    Public SUBMENU As FP_Menu = Nothing
    Public DIC_MenuItems As New Dictionary(Of Integer, FP_MenuItem)
    Public Next_Orientation As Enum_Direction
    Private Activated_Title_Background As Color = COLORS_NULL
    Private Deactivated_Title_Background As Color = Color.FromArgb(196, 196, 196)

    Sub New(ByVal MyFPApp As FP_App, ByVal MyParent_FPf As FP_Form, ByVal MyMenuNo As Integer, ByVal MyNext_Orientation As Enum_Direction)
        InitializeComponent()

        FPApp = MyFPApp
        Parent_FPf = MyParent_FPf
        MenuNo = MyMenuNo

        FPf = New FP_Form("FP_MENU_BASE", FPApp, Me, False)

        SubPrefix = SUBPREFIX_GET()

        FP = New FP(FPf, "FP_MENU", SubPrefix, True)

        Me.Show()

        Next_Orientation = MyNext_Orientation
        Me.Location = Parent_FPf.GET_Rect_Next_to_me(FPf.Frm.Size, Next_Orientation, 0, Next_Orientation).Location
    End Sub

    Public Overloads Sub Dispose()
        MyBase.Dispose()

        If Not (SUBMENU Is Nothing) Then
            SUBMENU.Dispose()
            SUBMENU = Nothing
        End If
        For Each AktKey As String In DIC_MenuItems.Keys
            DIC_MenuItems(AktKey).Dispose()
        Next
        DIC_MenuItems.Clear()
        FPf = Nothing
        Parent_FPf = Nothing
        FP = Nothing
        FPApp = Nothing
    End Sub

    Public Sub CLOSE_ALL_MENU()
        CLOSE_ME()
        If Not (Parent_FPf Is Nothing) Then
            If TypeOf Parent_FPf.Frm Is FP_Menu Then
                CType(Parent_FPf.Frm, FP_Menu).CLOSE_ALL_MENU()
            End If
        End If
    End Sub

    Private Function SUBPREFIX_GET() As String
        Dim OUT As String = ""

        Dim sqlComm As SqlCommand = FPApp.DC.CNN.CreateCommand()

        With FPf
            .Qdf_set_SP(sqlComm, "FP_MENU_GET_SUBPREFIX")
            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            .Qdf_AddParameter(sqlComm, "@MenuNo", SqlDbType.Int, , , , , MenuNo)
            .Qdf_AddParameter(sqlComm, "@Is_DEBUG_Mode", SqlDbType.Bit, , , , , Val(FPApp.Is_DEBUG_MODE()))

            .Qdf_AddParameter(sqlComm, "@SubPrefix", SqlDbType.NVarChar, ParameterDirection.Output, 50)

            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
        End With
        CURSOR_SHOW_WAIT()
        Try
            If FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE) Then
                OUT = sqlComm.Parameters("@SubPrefix").Value
            End If

        Catch ex As Exception
            FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        Return OUT
    End Function

    Public Sub CLOSE_ME()
        If Not (Parent_FPf Is Nothing) Then
            If TypeOf Parent_FPf.Frm Is FP_Menu Then
                With CType(Parent_FPf.Frm, FP_Menu)
                    .SUBMENU = Nothing
                End With
            End If
            Close()
        End If
    End Sub

    Private Sub FP_Menu_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Title_Label.BackColor = Activated_Title_Background
    End Sub

    Private Sub FP_Menu_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
        If Activated_Title_Background = COLORS_NULL Then
            Activated_Title_Background = Title_Label.BackColor
        End If
        Title_Label.BackColor = Deactivated_Title_Background
    End Sub

    Private Sub FP_Menu_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                CLOSE_ME()
                FPApp.FORMS_BringToFront(Parent_FPf)
        End Select
    End Sub

    Private Sub MenuItems_INIT()
        For Each AktKey As String In DIC_MenuItems.Keys
            DIC_MenuItems(AktKey).Dispose()
        Next
        DIC_MenuItems.Clear()

        Dim ItemNo As Integer = 0

        For Each AktKey As String In FP.CONTROLS.Keys
            Dim FPc As FP_Control = FP.CONTROLS(AktKey)

            If FPc.P.Tag > "" Then
                If FPApp.MENUITEM_IsHidden(FPc.P.Tag) Then
                    FPc.P_VISIBLE = False
                Else
                    Dim Tag_P() As String = Split(FPc.P.Tag, "|")
                    ReDim Preserve Tag_P(2)
                    Dim P As New FP_MenuItem.Struct_FP_MenuItem_Params

                    With P
                        .FPApp = FPApp
                        .Menu = Me
                        .FPo_MenuItem = FPc
                        .MenuItem_Text = FPc.c.Text
                        .Action = Tag_P(0)
                        .OpenArgs = Tag_P(1)
                    End With
                    ItemNo += 1
                    DIC_MenuItems.Add(ItemNo, New FP_MenuItem(P))
                End If
            End If
        Next
    End Sub

    Private Sub FP_Menu_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        FPf.INIT_CONTROLS(Nothing)
        FP.INIT_CONTROLS(Nothing)

        MenuItems_INIT()
        FOCUS_ME()
    End Sub

    Public Sub FOCUS_ME()
        Me.Focus()
        If Menu_Item_1.Visible Then
            FPf.FOCUS_ON_AT_THE_END(Menu_Item_1)
        ElseIf Menu_Item_2.Visible Then
            FPf.FOCUS_ON_AT_THE_END(Menu_Item_2)
        ElseIf Menu_Item_3.Visible Then
            FPf.FOCUS_ON_AT_THE_END(Menu_Item_3)
        ElseIf Menu_Item_4.Visible Then
            FPf.FOCUS_ON_AT_THE_END(Menu_Item_4)
        ElseIf Menu_Item_5.Visible Then
            FPf.FOCUS_ON_AT_THE_END(Menu_Item_5)
        ElseIf Menu_Item_6.Visible Then
            FPf.FOCUS_ON_AT_THE_END(Menu_Item_6)
        ElseIf Menu_Item_7.Visible Then
            FPf.FOCUS_ON_AT_THE_END(Menu_Item_7)
        ElseIf Menu_Item_8.Visible Then
            FPf.FOCUS_ON_AT_THE_END(Menu_Item_8)
        ElseIf Menu_Item_9.Visible Then
            FPf.FOCUS_ON_AT_THE_END(Menu_Item_9)
        ElseIf Menu_Item_10.Visible Then
            FPf.FOCUS_ON_AT_THE_END(Menu_Item_10)
        End If
    End Sub


    Private Sub FPf_FORM_CLOSING(ByVal sender As Object, ByRef e As System.Windows.Forms.FormClosingEventArgs) Handles FPf.FORM_CLOSING
        If Not (SUBMENU Is Nothing) Then
            SUBMENU.Close()
        End If
    End Sub

End Class
