Public Class FP_SpecControl
    Public Event SelectedIndexChanged(ByVal sender_FPt As FP_SpecControl, ByRef Handled As Boolean)

    Public WithEvents FPf As FP_Form
    Public WithEvents Frm As Form
    Public WithEvents c_TabControl As TabControl
    Public WithEvents c_SplitContainer As SplitContainer

    Public SelectedTab_Label_ForeColor As Color = Color.WhiteSmoke
    Public SelectedTab_Label_BackColor As Color = Color.DimGray

    Private Frm_Last_WindowState As System.Windows.Forms.FormWindowState = FormWindowState.Normal

    Sub New(ByVal MyFPf As FP_Form, ByVal MySpecControl As Control)
        FPf = MyFPf
        Frm = FPf.Frm
        Frm_Last_WindowState = Frm.WindowState

        If TypeOf (MySpecControl) Is TabControl Then
            c_TabControl = MySpecControl
            'c_TabControl.DrawMode = TabDrawMode.OwnerDrawFixed

            Dim P As TabPage

            For Each P In c_TabControl.TabPages
                CType(P, Control).TabIndex = c_TabControl.TabIndex
            Next
        ElseIf TypeOf (MySpecControl) Is SplitContainer Then
            c_SplitContainer = MySpecControl
            CONTROLS_SPLITCONTAINER_SET_NAME_OF_PANELS(c_SplitContainer)
            FPf.FPApp.CONTROLS_SPLITCONTAINER_SPLITTER_DISTANCE_LOAD(c_SplitContainer, FPf.Frm.Name, Frm_Last_WindowState)
        Else
            FPf.FPApp.DoErrorMsgBox("FP_SpecControl.New", 0, String.Format("Invalid controltype of Parameter 'MySpeccontrol' (Name of Control: '{0}')", MySpecControl.Name))
        End If
    End Sub

    Public Sub Dispose()
        c_TabControl = Nothing
        c_SplitContainer = Nothing

        Disposed = True
    End Sub

    Private Disposed As Boolean = False

    Private Sub c_TabControl_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles c_TabControl.DrawItem
        Dim g As Graphics = e.Graphics
        Dim tp As TabPage = c_TabControl.TabPages(e.Index)
        Dim br As Brush
        Dim sf As New StringFormat
        Dim r As New RectangleF(e.Bounds.X, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2)
        Dim r1 As Rectangle = e.Bounds

        sf.Alignment = StringAlignment.Center
        'r1.Location = New Point(r1.X + 4, r1.Y)
        'r1.Size = New Size(r1.Width - 4, r1.Height)
        r1.Location = New Point(r1.X, r1.Y)
        r1.Size = New Size(r1.Width, r1.Height)

        If c_TabControl.SelectedIndex = e.Index Then
            br = New SolidBrush(SelectedTab_Label_BackColor)
            g.FillRectangle(br, r1)
            br = New SolidBrush(SelectedTab_Label_ForeColor)
        Else
            br = New SolidBrush(tp.BackColor)
            g.FillRectangle(br, r1)
            br = New SolidBrush(Color.Black)
        End If

        g.DrawString(tp.Text, tp.Font, br, r, sf)
    End Sub

    Private Sub c_TabControl_Refresh()
        FPf.CONTROLS_ARRANGE_ALL()

        For Each Current_FP_ID In FPf.FPs.Keys
            With FPf.FPs(Current_FP_ID)
                If .GRID_EXISTS Then
                    .GRID.COLUMNS_Frozen_ACTIVATE()
                End If
            End With
        Next
    End Sub

    Private Sub c_TabControl_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles c_TabControl.SelectedIndexChanged
        Dim Handled As Boolean = False

        c_TabControl_Refresh()

        RaiseEvent SelectedIndexChanged(Me, Handled)
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Private Sub c_SplitContainer_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles c_SplitContainer.SizeChanged
        FPf.CONTROLS_ARRANGE_ALL()
    End Sub

    Private Sub c_SplitContainer_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles c_SplitContainer.SplitterMoved
        FPf.CONTROLS_ARRANGE_ALL()
    End Sub

    Private Sub FPf_FORM_CLOSING(sender As Object, ByRef e As FormClosingEventArgs) Handles FPf.FORM_CLOSING
        If Not (c_SplitContainer Is Nothing) Then
            If Not (FPf Is Nothing) Then
                FPf.FPApp.CONTROLS_SPLITCONTAINER_SPLITTER_DISTANCE_SAVE(c_SplitContainer, Frm.Name, Frm.WindowState)
            End If
        End If
    End Sub

    'VI - 20200423
    'Kiszedtem a kódot, mert tálcára lerakás után elrontotta a splitcontainer splitter beállítását, meg úgy egyébként sincs erre a kódra semmi szükség
    'Private Sub Frm_Move(sender As Object, e As EventArgs) Handles Frm.Move
    '    If Frm.WindowState <> Frm_Last_WindowState And Frm.WindowState <> FormWindowState.Minimized Then
    '        FPf.FPApp.CONTROLS_SPLITCONTAINER_SPLITTER_DISTANCE_SAVE(c_SplitContainer, Frm.Name, Frm_Last_WindowState)
    '        Frm_Last_WindowState = Frm.WindowState
    '        FPf.FPApp.CONTROLS_SPLITCONTAINER_SPLITTER_DISTANCE_LOAD(c_SplitContainer, Frm.Name, Frm_Last_WindowState)
    '    End If
    'End Sub
End Class
