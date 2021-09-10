Public Class FP_PictureBox
    Inherits FP_ControlObject

    Public Event CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs)

    Public WithEvents c As System.Windows.Forms.PictureBox

    Public FieldName As String
    Public ToggleButton As Boolean = False
    Public Image_Normal As String = ""
    Public Image_Pressed As String = ""
    Public Image_Focused As String = ""
    Public Image_Locked As String = ""
    Public CreatedBy As Enum_FP_CONTROL_Created_by = Enum_FP_CONTROL_Created_by.User

    Private Disposed As Boolean = False

    Private Current_Image As String = ""
    Private Current_Size As Size = New Size(0, 0)

    Private Locked As Boolean = False
    Private Pressed As Boolean = False

    Protected Friend CreatedAtRuntime As Boolean = False

    Public Property P_LOCKED() As Boolean
        Get
            Return Locked
        End Get
        Set(ByVal value As Boolean)
            Locked = value
            SHOW()
        End Set
    End Property
    Public Property P_VISIBLE() As Boolean
        Get
            P_Visible = c.Visible
        End Get
        Set(ByVal value As Boolean)
            c.Visible = value
            If c.Visible Then
                SHOW()
            End If
        End Set
    End Property
    Public Property P_PRESSED() As Boolean
        Get
            Return Pressed
        End Get
        Set(ByVal value As Boolean)
            Pressed = value
            SHOW()
        End Set
    End Property

    Public Sub RAISEEVENT_CLICK()
        Dim ee As New System.Windows.Forms.MouseEventArgs(MouseButtons.Left, 1, c.Left, c.Top, 0)

        RaiseEvent CLICK(Me, ee)
    End Sub


    Sub New(ByVal MyCreatedAtRuntime As Boolean)
        CreatedAtRuntime = MyCreatedAtRuntime
    End Sub
    Sub New(ByRef MyFPf As FP_Form, ByRef MyFP As FP, ByRef MyPictureBox As PictureBox, ByVal MyCreatedAtRuntime As Boolean)
        INIT(MyFPf, MyFP, MyPictureBox, MyCreatedAtRuntime)
    End Sub

    Sub New(ByRef MyFPf As FP_Form, ByRef MyFP As FP, ByRef MyPictureBox As PictureBox, Optional ByVal MyToggleButton As Boolean = False, Optional ByVal MyImage_Normal As String = "", Optional ByVal MyImage_Pressed As String = "", Optional ByVal MyImage_Focused As String = "", Optional ByVal MyImage_Locked As String = "")
        INIT(MyFPf, MyFP, MyPictureBox, False)
        ToggleButton = MyToggleButton

        PICTURES_SET(MyImage_Normal, MyImage_Pressed, MyImage_Focused, MyImage_Locked, False)

        If FPf.FPApp.Is_DEBUG_MODE() Then
            c.ContextMenuStrip = FPf.ContextMenu_DEBUG
        End If
    End Sub

    Public Sub PICTURES_SET(ByVal MyImage_Normal As String, Optional ByVal MyImage_Pressed As String = "", Optional ByVal MyImage_Focused As String = "", Optional ByVal MyImage_Locked As String = "", Optional SHOW_It As Boolean = True)
        Image_Normal = Trim(MyImage_Normal)
        Image_Pressed = Trim(MyImage_Pressed)
        Image_Focused = Trim(MyImage_Focused)
        Image_Locked = Trim(MyImage_Locked)

        If Image_Pressed = "" Then
            If Image_Normal > "" Then
                Dim p As Integer

                p = InStrRev(Image_Normal, ".")
                If p = 0 Then
                    FPf.FPApp.DoErrorMsgBox("FP_PictureBox.New", 0, String.Format("Extension of image '{0}' not declared. You have to define images with extension. (Example: {0}.png", Image_Normal))
                    Image_Pressed = Image_Normal
                Else
                    Image_Pressed = Left(Image_Normal, p - 1) + "_" + Mid(Image_Normal, p)
                End If
            End If
        End If

        If SHOW_It Then
            SHOW()
        End If
    End Sub

    Private Sub INIT(ByRef MyFPf As FP_Form, ByRef MyFP As FP, ByRef MyPictureBox As PictureBox, ByVal MyCreatedAtRuntime As Boolean)
        CreatedAtRuntime = MyCreatedAtRuntime

        FPf = MyFPf
        FP = MyFP

        If Not (FP Is Nothing) Then
            If Not FP.FPf.Equals(FPf) Then
                FPf.FPApp.DoErrorMsgBox("FP_PictureBox.New", 0, String.Format("FP_Picturebox '{0}' FP.FPf <> FPf", MyPictureBox.Name))
            End If
        End If

        c = MyPictureBox

        If FPf.FPApp.Is_DEBUG_MODE() Then
            c.ContextMenuStrip = FPf.ContextMenu_DEBUG
        End If
    End Sub

    Public Overloads Sub Dispose()
        If Disposed = False Then
            MyBase.Dispose()

            c = Nothing

            Disposed = True
        End If
    End Sub

    Sub SHOW(Optional Allways_Repaint As Boolean = False)
        If Not (c Is Nothing) Then
            If c.Visible Then
                Dim ResourceName As String = Image_Normal

                If Locked And Image_Locked > "" Then
                    ResourceName = Image_Locked
                ElseIf Pressed And Image_Pressed > "" Then
                    ResourceName = Image_Pressed
                ElseIf c.Focused And Image_Focused > "" Then
                    ResourceName = Image_Focused
                ElseIf Image_Normal > "" Then
                    ResourceName = Image_Normal
                Else
                    ResourceName = ""
                End If

                If ResourceName > "" Then
                    Dim asm As Reflection.Assembly = Nothing

                    If FPf.FPApp.SKIN_getASM_And_OBJECTNAME(ResourceName, asm, ResourceName) Then
                        If Allways_Repaint = True Or Current_Image <> ResourceName Or Current_Size <> c.Size Then
                            If c.Width > 0 And c.Height > 0 Then
                                Try
                                    Dim im As Bitmap

                                    im = New Bitmap(asm.GetManifestResourceStream(ResourceName))

                                    If InStr(ResourceName.ToUpper, ".GIF") = 0 And im.Size <> c.Size Then
                                        Dim im2 As New Bitmap(im, c.Size)

                                        c.Image = im2
                                    Else
                                        c.Image = im
                                    End If

                                    Current_Image = ResourceName
                                    Current_Size = c.Size

                                Catch ex As Exception
                                    FPf.FPApp.DoErrorMsgBox("FP_PictureBox.SHOW", 0, String.Format("Image not found '{0}'", ResourceName))
                                End Try
                            End If
                        End If
                    End If
                Else
                    If nz(Current_Image, "") <> "" Then
                        c.Image = Nothing
                        Current_Image = ""
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub EVENT_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles c.MouseDown
        If Disposed = False Then
            If e.Button = MouseButtons.Left Then
                If Not Locked Then
                    If ToggleButton Then
                        P_PRESSED = (Not P_PRESSED)
                    Else
                        Pressed = True
                    End If
                    SHOW()
                End If
            End If
        End If
    End Sub
    Private Sub EVENT_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles c.MouseUp
        If Disposed = False Then
            If Not Locked Then
                If ToggleButton Then
                    'Nothing to do
                Else
                    Pressed = False
                End If
                SHOW()

                Dim Akt_Locked As Boolean = Locked
                Locked = True
                RaiseEvent CLICK(Me, e)
                Locked = Akt_Locked
                SHOW()
            End If
        End If
    End Sub
    Public Sub EVENT_MOUSEENTER(ByVal sender As Object, ByVal e As System.EventArgs) Handles c.MouseEnter
        If Disposed = False Then
            FPf.HELP_SHOW(c)
        End If
    End Sub
    Public Sub EVENT_MOUSELEAVE(ByVal sender As Object, ByVal e As System.EventArgs) Handles c.MouseLeave
        FPf.HELP_HIDE()
    End Sub
    Private Sub EVENT_PAINT(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles c.Paint
        If Disposed = False Then
            SHOW()
        End If
    End Sub

    Protected Overrides Sub Finalize()
        If Disposed = False Then
            Dispose()
        End If
        MyBase.Finalize()
    End Sub
End Class
