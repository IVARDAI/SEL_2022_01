Public Class FP_MenuItem
    Public Structure Struct_FP_MenuItem_Params
        Dim FPApp As FP_App
        Dim Menu As FP_Menu
        Dim FPo_MenuItem As FP_ControlObject
        Dim MenuItem_Text As String
        Dim Action As String
        Dim OpenArgs As String
    End Structure

    Public P As New Struct_FP_MenuItem_Params
    Public WithEvents c As Control = Nothing
    Public WithEvents c_Button As Button
    Private WithEvents FPp As FP_PictureBox = Nothing
    Private Disposed As Boolean = False

    Sub New(ByVal Params As Struct_FP_MenuItem_Params)
        P = Params

        If (P.FPApp Is Nothing) Then
            P.FPApp.DoErrorMsgBox("FP_Menu_Item.New", 0, "P.FPApp is nothing!")
        End If

        If TypeOf (P.FPo_MenuItem) Is FP_Control Then
            With CType(P.FPo_MenuItem, FP_Control)
                c = .c

                If TypeOf (.c) Is Button Then
                    c_Button = CType(.c, Button)
                End If
            End With
        ElseIf TypeOf (P.FPo_MenuItem) Is FP_PictureBox Then
            FPp = CType(P.FPo_MenuItem, FP_PictureBox)
        Else
            P.FPApp.DoErrorMsgBox("FP_Menu_Item.New", 0, "Params.FPo has an unknown type")
        End If
    End Sub

    Public Sub Dispose()
        With P
            .Menu = Nothing
            .FPo_MenuItem = Nothing
            .Action = ""
            .OpenArgs = ""
        End With

        c = Nothing
        c_Button = Nothing
        FPp = Nothing

        Disposed = True
    End Sub

    Private Sub FPp_CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FPp.CLICK
        P.FPApp.RAISEEVENT_MenuItem_Activated(P)
    End Sub

    Private Sub c_Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles c_Button.Click
        If Not Disposed Then
            If Val(P.Action) > 0 Then
                Dim DoIt As Boolean = True

                If Not (P.Menu.SUBMENU) Is Nothing Then
                    With P.Menu
                        If .SUBMENU.MenuNo = Val(P.Action) Then
                            P.FPApp.FORMS_BringToFront(.SUBMENU)
                            DoIt = False
                        Else
                            .SUBMENU.Close()
                            .SUBMENU = Nothing
                        End If
                    End With
                End If

                If DoIt Then
                    P.Menu.SUBMENU = New FP_Menu(P.FPApp, Me.P.Menu.FPf, Val(P.Action), P.Menu.Next_Orientation)
                    DoIt = False
                End If
            Else
                If Not (P.Menu.SUBMENU) Is Nothing Then
                    With P.Menu
                        .SUBMENU.Close()
                        .SUBMENU = Nothing
                    End With
                End If
                P.Menu.CLOSE_ALL_MENU()
                P.FPApp.RAISEEVENT_MenuItem_Activated(P)
            End If
        End If
    End Sub

    Private Sub c_Button_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles c_Button.MouseDown
        Dim CtrlPressed As Boolean = My.Computer.Keyboard.CtrlKeyDown

        If CtrlPressed Then
            Dim DraggedData As String = String.Format("FP_MENUItem_DragDrop|{0}|{1}|{2}|||", P.Action, P.OpenArgs, c_Button.Text)
            c_Button.DoDragDrop(DraggedData, DragDropEffects.Copy)
        End If
    End Sub
End Class
