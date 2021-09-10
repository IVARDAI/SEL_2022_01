Imports MSDN.Html.Editor

Public Class FP_HTML_EDITOR_TEXTBOX
    Public Structure STRUCT_FP_HTML_EDITOR_PANEL_PARAMS
        Dim FPc_Parent_Textbox As FP_Control
    End Structure

    Public WithEvents FPc_Parent_Textbox As FP_Control
    Public WithEvents Parent_Textbox As TextBox
    Public WithEvents C_HTML_Editor_Textbox As MSDN.Html.Editor.HtmlEditorControl

    Sub New(P As STRUCT_FP_HTML_EDITOR_PANEL_PARAMS)
        With P
            FPc_Parent_Textbox = P.FPc_Parent_Textbox
            Parent_Textbox = FPc_Parent_Textbox.c
        End With

        CREATE_CONTROLS()
    End Sub

    Private Sub CREATE_CONTROLS()
        c_HTML_Editor_Textbox = New MSDN.Html.Editor.HtmlEditorControl

        With c_HTML_Editor_Textbox
            .Visible = True
            .Name = Parent_Textbox.Name + "_HTML"
            .Parent = Parent_Textbox.Parent
            .ToolbarDock = DockStyle.Top
        End With

        Parent_Textbox.Parent.Controls.Add(c_HTML_Editor_Textbox)

        ARRANGE()
    End Sub

    Public Sub ARRANGE()
        c_HTML_Editor_Textbox.Location = New Point(0, 0)
        c_HTML_Editor_Textbox.Size = Parent_Textbox.Size
    End Sub
End Class
