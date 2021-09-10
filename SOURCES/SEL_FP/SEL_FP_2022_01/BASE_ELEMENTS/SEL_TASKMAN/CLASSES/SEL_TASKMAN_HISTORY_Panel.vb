Imports System.Data
Imports System.Data.SqlClient

Public Class SEL_TASKMAN_HISTORY_Panel
    Public Structure Struct_SEL_TASKMAN_HISTORY_Panel_CONTROL_COLLECTION
        Dim FP_Parent As FP
        Dim BASE_Panel As Panel
        Dim Subprefix As String
        Dim FieldPrefix As String
        Dim CONNECTED_RECORDS_TYPES_CODE As String
        Dim FP_ImageList_for_Tasks As FP_L_ImageList      'Tartalmazza a TASK-ok allapotahoz hasznalt kepek listajat. Nem kotelezo megadni.
    End Structure

    Public WithEvents FPApp_for_Messages As FP_App = gl_FPApp

    Private WithEvents FP_Parent As FP
    Private WithEvents FPf As FP_Form
    Private BASE_Panel As Panel
    Private Subprefix As String
    Private FieldPrefix As String
    Private CONNECTED_RECORDS_TYPES_CODE As String
    Public FP_ImageList_for_Tasks As FP_L_ImageList
    Public WithEvents FP_TASKS As FP
    Public WithEvents FP_HISTORY As FP

    Public WithEvents FPp_TASKS_BTN_ADD_NEW_TASK As FP_PictureBox

    Private CONTROL_SC As SplitContainer
    Private CONTROL_TASKS_GRID_Label As Label
    Private CONTROL_TASKS_GRID As DataGridView
    Private CONTROL_TASKS_SavePoint As TextBox
    Private CONTROL_HISTORY_GRID_Label As Label
    Private CONTROL_HISTORY_GRID As DataGridView
    Private CONTROL_HISTORY_SavePoint As TextBox
    Private CONTROL_BTN_ADD_NEW_TASK As PictureBox

    Private Control_Prefix = "TASKMAN_"

    Private Disposed As Boolean = False

    Private CL_TASKMAN_Not_Installed As SEL_MODULE_NOT_INSTALLED_PANEL

    Private SEQ As New FP_SEQ(gl_FPApp, "VBSEQ_TASKMAN_HISTORY_INTERNAL")

    Public Sub New(CONTROL_COLLECTION As Struct_SEL_TASKMAN_HISTORY_Panel_CONTROL_COLLECTION)
        With CONTROL_COLLECTION
            FP_Parent = .FP_Parent
            BASE_Panel = .BASE_Panel
            Subprefix = .Subprefix
            FieldPrefix = .FieldPrefix
            CONNECTED_RECORDS_TYPES_CODE = .CONNECTED_RECORDS_TYPES_CODE
            FP_ImageList_for_Tasks = .FP_ImageList_for_Tasks
        End With

        FPf = FP_Parent.FPf

        If TASKMAN_INSTALLED() = False Then
            Dim CL_TASLMAN_Not_Installed_P As New SEL_MODULE_NOT_INSTALLED_PANEL.Struct_SEL_MODUL_NOT_INSTALLED_PANEL_PARAMS
            With CL_TASLMAN_Not_Installed_P
                .BASE_Panel = BASE_Panel
                .FPf = FPf
                .Fieldprefix = FieldPrefix
                .Subprefix = Subprefix
                .Info_URL = SEQ.GET_SEQ_BY_TEXT1("URL_NOT_INSTALLED").Text3
            End With
            CL_TASKMAN_Not_Installed = New SEL_MODULE_NOT_INSTALLED_PANEL(CL_TASLMAN_Not_Installed_P)
        Else
            FP_TASKS = New FP(FPf, "TM_PANEL_TASKS", Subprefix, FP_Parent, "PARAM_INT")
            With FP_TASKS
                With .SQL_BIND_Params
                    .NameOf_DEL = ""
                    .NameOf_SAVE = ""
                End With
                .P_FORM_AllowAdditions = False
                .P_FORM_AllowDeletions = False
                .P_FORM_AllowEdits = False
                .FORM_SubWHERE_FIX = String.Format("CONN_TYPE_Code = '{0}'", CONNECTED_RECORDS_TYPES_CODE)

                .P_FP_Refresh_Only_When_This_Field_Visible = BASE_Panel
            End With

            FP_HISTORY = New FP(FPf, "TM_PANEL_HISTORY", Subprefix, FP_TASKS, "TM_TASKS_ID")
            With FP_HISTORY
                With .SQL_BIND_Params
                    .NameOf_DEL = ""
                    .NameOf_SAVE = ""
                End With
                .P_FORM_AllowAdditions = False
                .P_FORM_AllowDeletions = False
                .P_FORM_AllowEdits = False

                .P_FP_Refresh_Only_When_This_Field_Visible = BASE_Panel
            End With

            If FP_ImageList_for_Tasks Is Nothing Then
                FP_ImageList_for_Tasks_CREATE_STANDARD()
            End If

            CREATE_CONTROLS()

            Dim FP_TASKS_CONTROLS_COLLECTION As New Struct_FP_CONTROLS_COLLECTION
            With FP_TASKS_CONTROLS_COLLECTION
                .FieldPrefix = FieldPrefix
                With .GRID
                    .Label = CONTROL_TASKS_GRID_Label
                    .GRID = CONTROL_TASKS_GRID
                End With
            End With
            FP_TASKS.INIT_CONTROLS(FP_TASKS_CONTROLS_COLLECTION)

            Dim FP_HISTORY_CONTROLS_COLLECTION As New Struct_FP_CONTROLS_COLLECTION
            With FP_HISTORY_CONTROLS_COLLECTION
                .FieldPrefix = FieldPrefix
                With .GRID
                    .Label = CONTROL_HISTORY_GRID_Label
                    .GRID = CONTROL_HISTORY_GRID
                End With
            End With
            FP_HISTORY.INIT_CONTROLS(FP_HISTORY_CONTROLS_COLLECTION)
        End If
    End Sub

    Public Sub DisposeMe()
        If Disposed = False Then
            Disposed = True

            If TASKMAN_INSTALLED() = False Then
                If Not (CL_TASKMAN_Not_Installed Is Nothing) Then
                    CL_TASKMAN_Not_Installed.DisposeMe()
                    CL_TASKMAN_Not_Installed = Nothing
                End If
            Else

                FP_ImageList_for_Tasks = Nothing

                FPp_TASKS_BTN_ADD_NEW_TASK.Dispose()
                FPp_TASKS_BTN_ADD_NEW_TASK = Nothing

                FP_HISTORY.Dispose()
                FP_TASKS.Dispose()

                FPf.CONTROLS_REMOVE(CONTROL_BTN_ADD_NEW_TASK.Name)
                CONTROL_BTN_ADD_NEW_TASK.Dispose()
                CONTROL_BTN_ADD_NEW_TASK = Nothing

                FPf.CONTROLS_REMOVE(CONTROL_TASKS_GRID_Label.Name)
                CONTROL_TASKS_GRID_Label.Dispose()
                CONTROL_TASKS_GRID_Label = Nothing

                FPf.CONTROLS_REMOVE(CONTROL_TASKS_GRID.Name)
                CONTROL_TASKS_GRID.Dispose()
                CONTROL_TASKS_GRID = Nothing

                FPf.CONTROLS_REMOVE(CONTROL_TASKS_SavePoint.Name)
                CONTROL_TASKS_SavePoint.Dispose()
                CONTROL_TASKS_SavePoint = Nothing

                FPf.CONTROLS_REMOVE(CONTROL_HISTORY_GRID_Label.Name)
                CONTROL_HISTORY_GRID_Label.Dispose()
                CONTROL_HISTORY_GRID_Label = Nothing

                FPf.CONTROLS_REMOVE(CONTROL_HISTORY_GRID.Name)
                CONTROL_HISTORY_GRID.Dispose()
                CONTROL_HISTORY_GRID = Nothing

                FPf.CONTROLS_REMOVE(CONTROL_HISTORY_SavePoint.Name)
                CONTROL_HISTORY_SavePoint.Dispose()
                CONTROL_HISTORY_SavePoint = Nothing

                FPf.CONTROLS_REMOVE(CONTROL_SC.Name)
                CONTROL_SC.Dispose()
                CONTROL_SC = Nothing

                FP_Parent = Nothing
                FPf = Nothing
                FPApp_for_Messages = Nothing
            End If
        End If
    End Sub

    Private Sub FP_ImageList_for_Tasks_CREATE_STANDARD()
        FP_ImageList_for_Tasks = New FP_L_ImageList()
        FP_ImageList_for_Tasks.LOAD_Pictures_FROM_SEL_SKIN()
    End Sub

    Public Function Control_FieldPrefix_And_Prefix() As String
        Dim OUT As String = Control_Prefix

        If FieldPrefix > "" Then
            OUT = FieldPrefix + OUT
        End If

        Return OUT
    End Function

    Public Sub REFRESH()
        If Not Disposed Then
            If TASKMAN_INSTALLED() Then
                FP_TASKS.FORM_DORESYNC(True)
                FP_TASKS.FORM_GOTO_RECORD_GRID_LASTROW()
            End If
        End If
    End Sub

    Private Sub CREATE_CONTROLS()
        CONTROL_SC = New SplitContainer
        With CONTROL_SC
            .Name = Control_FieldPrefix_And_Prefix() + "SC"
            .Parent = BASE_Panel
            .Visible = True
            .Orientation = Orientation.Horizontal
        End With
        FPf.CONTROLS_ADD(CONTROL_SC)

        CONTROL_TASKS_GRID_Label = New Label
        With CONTROL_TASKS_GRID_Label
            .Name = Control_FieldPrefix_And_Prefix() + "TASKS_GRID_Label"
            .Parent = CONTROL_SC.Panel1
            .Visible = True
            .BackColor = Color.FromArgb(25, 25, 111)
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        End With
        FPf.CONTROLS_ADD(CONTROL_TASKS_GRID_Label)

        CONTROL_TASKS_GRID = New DataGridView
        With CONTROL_TASKS_GRID
            .Name = Control_FieldPrefix_And_Prefix() + "TASKS_GRID"
            .Parent = CONTROL_SC.Panel1
            .Visible = True
            .BackgroundColor = Color.DarkGray
            .AllowDrop = True
        End With
        FPf.CONTROLS_ADD(CONTROL_TASKS_GRID)

        CONTROL_TASKS_SavePoint = New TextBox
        With CONTROL_TASKS_SavePoint
            .Name = Control_FieldPrefix_And_Prefix() + "TASKS_SavePoint"
            .Parent = CONTROL_SC.Panel1
            .BackColor = Color.DarkGray
            .Width = 20
            .SendToBack()
            .Visible = True
        End With
        FPf.CONTROLS_ADD(CONTROL_TASKS_SavePoint)
        CONTROL_TASKS_SavePoint.SendToBack()

        CONTROL_BTN_ADD_NEW_TASK = New PictureBox

        With CONTROL_BTN_ADD_NEW_TASK
            .Parent = CONTROL_SC.Panel1
            .Name = Control_FieldPrefix_And_Prefix() + "TASKS_BTN_ADD_NEW_TASK"
            .Left = 0
            .Top = 0
            .Width = 22
            .Height = 22
            .Visible = True
            .BringToFront()
        End With
        FPf.CONTROLS_ADD(CONTROL_BTN_ADD_NEW_TASK)

        CONTROL_HISTORY_GRID_Label = New Label
        With CONTROL_HISTORY_GRID_Label
            .Name = Control_FieldPrefix_And_Prefix() + "HISTORY_GRID_Label"
            .Parent = CONTROL_SC.Panel2
            .Visible = True
            .BackColor = Color.FromArgb(25, 25, 111)
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        End With
        FPf.CONTROLS_ADD(CONTROL_HISTORY_GRID_Label)

        CONTROL_HISTORY_GRID = New DataGridView
        With CONTROL_HISTORY_GRID
            .Name = Control_FieldPrefix_And_Prefix() + "HISTORY_GRID"
            .Parent = CONTROL_SC.Panel2
            .Visible = True
            .BackgroundColor = Color.DarkGray
            .AllowDrop = True
        End With
        FPf.CONTROLS_ADD(CONTROL_HISTORY_GRID)

        CONTROL_HISTORY_SavePoint = New TextBox
        With CONTROL_HISTORY_SavePoint
            .Name = Control_FieldPrefix_And_Prefix() + "HISTORY_SavePoint"
            .Parent = CONTROL_SC.Panel2
            .BackColor = Color.DarkGray
            .Width = 20
            .SendToBack()
            .Visible = True
        End With
        FPf.CONTROLS_ADD(CONTROL_HISTORY_SavePoint)
        CONTROL_HISTORY_SavePoint.SendToBack()
    End Sub

    Private Sub FP_TASKS_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_TASKS.CONTROLS_INITIALIZED
        With FP_TASKS
            FPp_TASKS_BTN_ADD_NEW_TASK = .PICTUREBOXES_GET(Control_FieldPrefix_And_Prefix() + "TASKS_BTN_ADD_NEW_TASK")
        End With
    End Sub

    Private Sub FPp_TASKS_BTN_ADD_NEW_TASK_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPp_TASKS_BTN_ADD_NEW_TASK.CLICK
        If FP_Parent.FPf.SAVE_ALL Then
            If FP_Parent.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then

                Dim CONNECTED_RECORD_P As New STRUCT_TASKMAN_PREDEFINED_CONNECTED_RECORDS

                With CONNECTED_RECORD_P
                    .TM_CONNECTED_RECORDS_TYPES_CODE = CONNECTED_RECORDS_TYPES_CODE
                    .PARAM_INT = FP_Parent.P_DATA_Current_ID
                End With

                Dim Frm As New SEL_TASKMAN_TASK(CONNECTED_RECORD_P)

                Frm.Show()
            End If
        End If
    End Sub

    Private Sub FPApp_for_Messages_Message(sender As FP_App, From_FPf As FP_Form, MessageCode As String, ByRef Individual_Params As Object, ByRef Handled As Boolean) Handles FPApp_for_Messages.Message
        If Disposed = False Then
            If MessageCode = "TM_REFRESH" Then
                Dim FP_TASKS_Current_ID As Long = FP_TASKS.P_DATA_Current_ID

                FP_TASKS.FORM_DORESYNC(True)

                If FP_TASKS_Current_ID <> 0 Then
                    FP_TASKS.FORM_GOTO_RECORD_BY_ID(FP_TASKS_Current_ID)
                End If
            End If
        End If
    End Sub

    Private Sub FP_TASKS_GRID_Row_DoubleClick(sender_FP As FP, ByRef Handled As Boolean) Handles FP_TASKS.GRID_Row_DoubleClick
        FP_TASKS_SHOW_CURRENT_TASK()
    End Sub

    Public Sub FP_TASKS_SHOW_CURRENT_TASK()
        If FP_TASKS.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then

            CURSOR_SHOW_WAIT()

            Dim TASKS_ID As Long = FP_TASKS.P_DATA_Current_ID
            TASKS_SHOW(TASKS_ID)

            CURSOR_SHOW_DEFAULT()

        End If
    End Sub

    Private Sub FPf_FORM_CLOSED(sender As Object) Handles FPf.FORM_CLOSED
        DisposeMe()
    End Sub
End Class
