Imports System.IO
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WinForms
Imports System.Drawing.Imaging
Imports System.Drawing.Printing

Public Structure Struct_FP_L_Dispo_Module
    Dim RS_Type As String
    Dim FPf As FP_Form
    Dim TV As TreeView
    Dim GRID_FP As FP
    Dim GRID_FPc_Selected_Checkbox As FP_Control
    Dim SubWHERE As String
    Dim SubWHERE_Do_Not_Insert_Terminal_In_WHERE As Boolean
    Dim No_MultiRoot As Boolean
    Dim SQL_Bind_Params As FP_L_Dispo_Module.Struct_SQL_Bind_Params
End Structure
Public Structure Struct_FP_L_DISPO_DragDropOptions
    Dim DISPO As FP_L_DISPO
    Dim FROM_Module As FP_L_Dispo_Module
    Dim FROM_Level As Integer
    Dim TO_Module As FP_L_Dispo_Module
    Dim TO_Level As Integer
    Dim Enabled As Boolean
End Structure
Public Structure Struct_FP_L_Dispo_Node
    Dim Dispo_Module As FP_L_Dispo_Module
    Dim Node As TreeNode
    Dim Level As Integer
    Dim ID As Long
    Dim FP As FP
    Dim Root_ID As Long
End Structure
Public Structure Struct_TreeNode_Props
    Dim Name As String
    Dim Level As Integer
    Dim Parent_Name As String
    Dim IsExpanded As Boolean
    Dim ImageKey As String
    Dim BackColor As Color
    Dim ForeColor As Color
    Dim Text As String
    Dim Tag As String
    Dim ToolTipText As String
    Dim SelectedImageKey As String
    Dim NodeFont As System.Drawing.Font
End Structure
Public Structure Struct_FP_L_TreeView_With_FP_CONTROLS
    Dim ServerObject_Prefix As String
    Dim FPf As FP_Form
    Dim TV As TreeView
    Dim MultiRoot As Boolean
    Dim SubWHERE As String
    Dim SubWHERE_Do_Not_Insert_Terminal_In_WHERE As Boolean
    Dim SQL_Bind_Params As FP_L_TreeView_With_FP.Struct_SQL_Bind_PARAMS
End Structure


Public Class FP_L_Array_Of_FP
    Public FPApp As FP_App

    Public Event ARRAY_OF_FP_CONTROLS_INITIALIZED(ByVal Index_Of_FP As Integer)
    Public Event ARRAY_OF_FP_Form_BeforeInsert(ByVal Index_Of_FP As Integer, ByRef Cancel As Integer, ByRef ID_of_Newrecord As Long)
    Public Event ARRAY_OF_FP_Data_Records_Loaded(ByVal Index_Of_FP As Integer)
    Public Event ARRAY_OF_FP_Form_BeforeUpdate(ByVal Index_Of_FP As Integer, ByRef Cancel As Integer)
    Public Event ARRAY_OF_FP_Form_BeforeDelete(ByVal Index_Of_FP As Integer, ByRef Cancel As Integer)
    Public Event ARRAY_OF_FP_Form_AfterUpdate(ByVal Index_Of_FP As Integer)
    Public Event ARRAY_OF_FP_Form_AfterDelete(ByVal Index_Of_FP As Integer)
    Public Event ARRAY_OF_FP_Form_Current(ByVal Index_Of_FP As Integer)
    Public Event ARRAY_OF_FP_Form_Current_AfterChildren(ByVal Index_Of_FP As Integer)
    Public Event ARRAY_OF_FP_Form_NoRecord(ByVal Index_Of_FP As Integer)
    Public Event ARRAY_OF_FP_Form_Dirty(ByVal Index_Of_FP As Integer, ByRef Cancel As Integer)
    Public Event ARRAY_OF_FP_Form_BeginEdit(ByVal Index_Of_FP As Integer)
    Public Event ARRAY_OF_FP_Form_Field_Enter(ByVal Index_Of_FP As Integer, ByVal FPc As FP_Control, ByRef Handled As Boolean)
    Public Event ARRAY_OF_FP_Form_Field_AfterUpdate(ByVal Index_Of_FP As Integer, ByVal FPc As FP_Control)
    Public Event ARRAY_OF_FP_Form_KeyPreview_KeyDown(ByVal Index_Of_FP As Integer, ByRef sender As Object, ByRef e As System.Windows.Forms.KeyEventArgs)
    Public Event ARRAY_OF_FP_Form_KeyPreview_KeyPress(ByVal Index_Of_FP As Integer, ByRef sender As Object, ByRef e As System.Windows.Forms.KeyPressEventArgs)
    Public Event ARRAY_OF_FP_Form_Print_Begin(ByVal Index_Of_FP As Integer, ByRef Cancel As Integer)
    Public Event ARRAY_OF_FP_Form_Print_Prepare(ByVal Index_Of_FP As Integer, ByVal Identifier As String, ByRef Prepared As Boolean, ByRef CancelOpenReport As Boolean)
    Public Event ARRAY_OF_FP_Form_Print_End(ByVal Index_Of_FP As Integer)
    Public Event ARRAY_OF_FP_GRID_Paint(ByVal Index_Of_FP As Integer, ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
    Public Event ARRAY_OF_FP_GRID_Row_Drag_Begin(ByVal Index_Of_FP As Integer, ByRef DATA As String, ByVal e As System.Windows.Forms.MouseEventArgs, ByRef Cancel As Boolean)
    Public Event ARRAY_OF_FP_GRID_Row_DoubleClick(ByVal Index_Of_FP As Integer)
    Public Event ARRAY_OF_FP_GRID_Row_MouseWheel(ByVal Index_Of_FP As Integer, ByVal sender As Object, ByRef e As System.Windows.Forms.MouseEventArgs, ByRef Handled As Boolean)
    Public Event ARRAY_OF_FP_GRID_Filter_Changed(ByVal Index_Of_FP As Integer)
    Public Event ARRAY_OF_FP_GRID_Sorted(ByVal Index_Of_FP As Integer, ByVal e As System.EventArgs)
    Public Event ARRAY_OF_FP_GRID_CellClick(ByVal Index_Of_FP As Integer, ByVal FPc As FP_Control, ByRef Handled As Boolean)
    Public Event ARRAY_OF_FP_EXCEL_IMPORT_Data_Records_Prepared(ByVal Index_Of_FP As Integer, ByVal sender As FP_XLS_Import, ByRef Cancel As Boolean)
    Public Event ARRAY_OF_FP_EXCEL_IMPORT_Check_Data(ByVal Index_Of_FP As Integer, ByVal sender As FP_XLS_Import)
    Public Event ARRAY_OF_FP_EXCEL_IMPORT_Import_Data(ByVal Index_Of_FP As Integer, ByVal sender As FP_XLS_Import, ByRef Cancel As Boolean)

    Public DIC_FPs As New Dictionary(Of Integer, FP_L_Array_Of_FP_Element)

    Private Disposed As Boolean = False

    Sub New(ByVal MyFPApp As FP_App)
        FPApp = MyFPApp
    End Sub

    Public Sub Dispose()
        DIC_FPs.Clear()

        Disposed = True
    End Sub

    Public Function ADD_FP(ByVal NewFP As FP) As Integer
        Dim OUT As Integer = -1

        Dim NewElement As New FP_L_Array_Of_FP_Element(NewFP, Me)

        DIC_FPs.Add(DIC_FPs.Count, NewElement)

        OUT = DIC_FPs.Count - 1

        ADD_FP = OUT
    End Function

    Public Sub ARRAY_GOTO_NORECORD(Optional ByVal FromLevel As Integer = 0, Optional ByVal ToLevel As Integer = -1)
        If ToLevel < 0 Then
            ToLevel = DIC_FPs.Count - 1
        End If

        For AktLevel As Integer = FromLevel To ToLevel
            If Not (DIC_FPs(AktLevel).FP Is Nothing) Then
                DIC_FPs(AktLevel).FP.FORM_GOTO_NORECORD()
            End If
        Next
    End Sub

    Public Function GET_INDEX_OF_FP(ByVal FP As FP) As Integer
        Dim OUT As Integer = -1

        If FP Is Nothing Then
            FPApp.DoErrorMsgBox("FP_L_Array_Of_FP.GET_INDEX_OF_FP", 0, "FP is nothing")
        Else
            For Each i As Integer In DIC_FPs.Keys
                If Not (DIC_FPs(i).FP Is Nothing) Then
                    If DIC_FPs(i).FP.Equals(FP) Then
                        OUT = i
                        Exit For
                    End If
                End If
            Next
        End If

        GET_INDEX_OF_FP = OUT
    End Function

    Public Sub RAISEEVENT_ARRAY_OF_FP_CONTROLS_INITIALIZED(ByVal sender_FP As FP)
        RaiseEvent ARRAY_OF_FP_CONTROLS_INITIALIZED(GET_INDEX_OF_FP(sender_FP))
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_BeforeInsert(ByVal sender_FP As FP, ByRef Cancel As Integer, ByRef ID_of_Newrecord As Long)
        RaiseEvent ARRAY_OF_FP_Form_BeforeInsert(GET_INDEX_OF_FP(sender_FP), Cancel, ID_of_Newrecord)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Data_Records_Loaded(ByVal sender_FP As FP)
        RaiseEvent ARRAY_OF_FP_Data_Records_Loaded(GET_INDEX_OF_FP(sender_FP))
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_BeforeUpdate(ByVal sender_FP As FP, ByRef Cancel As Integer)
        RaiseEvent ARRAY_OF_FP_Form_BeforeUpdate(GET_INDEX_OF_FP(sender_FP), Cancel)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_BeforeDelete(ByVal sender_FP As FP, ByRef Cancel As Integer)
        RaiseEvent ARRAY_OF_FP_Form_BeforeDelete(GET_INDEX_OF_FP(sender_FP), Cancel)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_AfterUpdate(ByVal sender_FP As FP)
        RaiseEvent ARRAY_OF_FP_Form_AfterUpdate(GET_INDEX_OF_FP(sender_FP))
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_AfterDelete(ByVal sender_FP As FP)
        RaiseEvent ARRAY_OF_FP_Form_AfterDelete(GET_INDEX_OF_FP(sender_FP))
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_Current(ByVal sender_FP As FP)
        RaiseEvent ARRAY_OF_FP_Form_Current(GET_INDEX_OF_FP(sender_FP))
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_Current_AfterChildren(ByVal sender_FP As FP)
        RaiseEvent ARRAY_OF_FP_Form_Current_AfterChildren(GET_INDEX_OF_FP(sender_FP))
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_NoRecord(ByVal sender_FP As FP)
        RaiseEvent ARRAY_OF_FP_Form_NoRecord(GET_INDEX_OF_FP(sender_FP))
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_Dirty(ByVal sender_FP As FP, ByRef Cancel As Integer)
        RaiseEvent ARRAY_OF_FP_Form_Dirty(GET_INDEX_OF_FP(sender_FP), Cancel)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_BeginEdit(ByVal sender_FP As FP)
        RaiseEvent ARRAY_OF_FP_Form_BeginEdit(GET_INDEX_OF_FP(sender_FP))
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_Field_Enter(ByVal sender_FP As FP, ByVal FPc As FP_Control, ByRef Handled As Boolean)
        RaiseEvent ARRAY_OF_FP_Form_Field_Enter(GET_INDEX_OF_FP(sender_FP), FPc, Handled)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_Field_AfterUpdate(ByVal FPc As FP_Control)
        RaiseEvent ARRAY_OF_FP_Form_Field_AfterUpdate(GET_INDEX_OF_FP(FPc.FP), FPc)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_KeyPreview_KeyDown(ByVal sender_FP As FP, ByRef sender As Object, ByRef e As System.Windows.Forms.KeyEventArgs)
        RaiseEvent ARRAY_OF_FP_Form_KeyPreview_KeyDown(GET_INDEX_OF_FP(sender_FP), sender, e)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_KeyPreview_KeyPress(ByVal sender_FP As FP, ByRef sender As Object, ByRef e As System.Windows.Forms.KeyPressEventArgs)
        RaiseEvent ARRAY_OF_FP_Form_KeyPreview_KeyPress(GET_INDEX_OF_FP(sender_FP), sender, e)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_Print_Begin(ByVal sender_FP As FP, ByVal Cancel As Integer)
        RaiseEvent ARRAY_OF_FP_Form_Print_Begin(GET_INDEX_OF_FP(sender_FP), Cancel)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_Print_Prepare(ByVal sender_FP As FP, ByVal Identifier As String, ByRef Prepared As Boolean, ByRef CancelOpenReport As Boolean)
        RaiseEvent ARRAY_OF_FP_Form_Print_Prepare(GET_INDEX_OF_FP(sender_FP), Identifier, Prepared, CancelOpenReport)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_Form_Print_End(ByVal sender_FP As FP)
        RaiseEvent ARRAY_OF_FP_Form_Print_End(GET_INDEX_OF_FP(sender_FP))
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_GRID_Paint(ByVal sender_FP As FP, ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        RaiseEvent ARRAY_OF_FP_GRID_Paint(GET_INDEX_OF_FP(sender_FP), sender, e)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_GRID_Row_Drag_Begin(ByVal sender_FP As FP, ByRef DATA As String, ByVal e As System.Windows.Forms.MouseEventArgs, ByRef Cancel As Boolean)
        RaiseEvent ARRAY_OF_FP_GRID_Row_Drag_Begin(GET_INDEX_OF_FP(sender_FP), DATA, e, Cancel)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_GRID_Row_DoubleClick(ByVal sender_FP As FP)
        RaiseEvent ARRAY_OF_FP_GRID_Row_DoubleClick(GET_INDEX_OF_FP(sender_FP))
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_GRID_Row_MouseWheel(ByVal sender_FP As FP, ByVal sender As Object, ByRef e As System.Windows.Forms.MouseEventArgs, ByRef Handled As Boolean)
        RaiseEvent ARRAY_OF_FP_GRID_Row_MouseWheel(GET_INDEX_OF_FP(sender_FP), sender, e, Handled)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_GRID_Filter_Changed(ByVal sender_FP As FP)
        RaiseEvent ARRAY_OF_FP_GRID_Filter_Changed(GET_INDEX_OF_FP(sender_FP))
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_GRID_Sorted(ByVal sender_FP As FP, ByVal e As System.EventArgs)
        RaiseEvent ARRAY_OF_FP_GRID_Sorted(GET_INDEX_OF_FP(sender_FP), e)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_GRID_CellClick(ByVal sender_FP As FP, ByVal FPc As FP_Control, ByRef Handled As Boolean)
        RaiseEvent ARRAY_OF_FP_GRID_CellClick(GET_INDEX_OF_FP(sender_FP), FPc, Handled)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_EXCEL_IMPORT_Data_Records_Prepared(ByVal sender_FP As FP, ByVal sender As FP_XLS_Import, ByRef Cancel As Boolean)
        RaiseEvent ARRAY_OF_FP_EXCEL_IMPORT_Data_Records_Prepared(GET_INDEX_OF_FP(sender_FP), sender, Cancel)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_EXCEL_IMPORT_Check_Data(ByVal sender_FP As FP, ByVal sender As FP_XLS_Import)
        RaiseEvent ARRAY_OF_FP_EXCEL_IMPORT_Check_Data(GET_INDEX_OF_FP(sender_FP), sender)
    End Sub
    Public Sub RAISEEVENT_ARRAY_OF_FP_EXCEL_IMPORT_Import_Data(ByVal sender_FP As FP, ByVal sender As FP_XLS_Import, ByRef Cancel As Boolean)
        RaiseEvent ARRAY_OF_FP_EXCEL_IMPORT_Import_Data(GET_INDEX_OF_FP(sender_FP), sender, Cancel)
    End Sub
End Class
Public Class FP_L_Array_Of_FP_Element

    Public Parent As FP_L_Array_Of_FP
    Public WithEvents FP As FP

    Sub New(ByVal MyFP As FP, ByVal MyParent As FP_L_Array_Of_FP)
        FP = MyFP
        Parent = MyParent
    End Sub

    Private Sub EVENT_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP.CONTROLS_INITIALIZED
        Parent.RAISEEVENT_ARRAY_OF_FP_CONTROLS_INITIALIZED(sender_FP)
    End Sub

    Private Sub EVENT_Form_BeforeInsert(ByVal sender_FP As FP, ByRef Cancel As Integer, ByRef ID_of_Newrecord As Long) Handles FP.Form_BeforeInsert
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_BeforeInsert(sender_FP, Cancel, ID_of_Newrecord)
    End Sub

    Private Sub EVENT_Data_Records_Loaded(ByVal sender_FP As FP) Handles FP.Data_Records_Loaded
        Parent.RAISEEVENT_ARRAY_OF_FP_Data_Records_Loaded(sender_FP)
    End Sub

    Private Sub EVENT_Form_BeforeUpdate(ByVal sender_FP As FP, ByRef Cancel As Integer) Handles FP.Form_BeforeUpdate
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_BeforeUpdate(sender_FP, Cancel)
    End Sub

    Private Sub EVENT_Form_BeforeDelete(ByVal sender_FP As FP, ByRef Cancel As Integer) Handles FP.Form_BeforeDelete
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_BeforeDelete(sender_FP, Cancel)
    End Sub

    Private Sub EVENT_Form_AfterUpdate(ByVal sender_FP As FP) Handles FP.Form_AfterUpdate
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_AfterUpdate(sender_FP)
    End Sub

    Private Sub EVENT_Form_AfterDelete(ByVal sender_FP As FP) Handles FP.Form_AfterDelete
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_AfterDelete(sender_FP)
    End Sub

    Private Sub EVENT_Form_Current(ByVal sender_FP As FP) Handles FP.Form_Current
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_Current(sender_FP)
    End Sub
    Private Sub EVENT_FP_Form_Current_AfterChildren(ByVal sender_FP As FP) Handles FP.Form_Current_AfterChildren
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_Current_AfterChildren(sender_FP)
    End Sub
    Private Sub EVENT_Form_NoRecord(ByVal sender_FP As FP) Handles FP.Form_NoRecord
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_NoRecord(sender_FP)
    End Sub
    Private Sub EVENT_Form_Dirty(ByVal sender_FP As FP, ByRef Cancel As Integer) Handles FP.Form_Dirty
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_Dirty(sender_FP, Cancel)
    End Sub
    Private Sub EVENT_Form_BeginEdit(ByVal sender_FP As FP) Handles FP.Form_BeginEdit
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_BeginEdit(sender_FP)
    End Sub
    Private Sub EVENT_FP_Form_Field_AfterUpdate(ByVal FPc As FP_Control) Handles FP.Form_Field_AfterUpdate
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_Field_AfterUpdate(FPc)
    End Sub
    Private Sub EVENT_FP_Form_Field_Enter(ByVal FPc As FP_Control, ByRef Handled As Boolean) Handles FP.Form_Field_Enter
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_Field_Enter(FPc.FP, FPc, Handled)
    End Sub
    Private Sub EVENT_Form_KeyPreview_KeyDown(ByVal sender_FP As FP, ByRef sender As Object, ByRef e As System.Windows.Forms.KeyEventArgs) Handles FP.Form_KeyPreview_KeyDown
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_KeyPreview_KeyDown(sender_FP, sender, e)
    End Sub
    Private Sub EVENT_Form_KeyPreview_KeyPress(ByVal sender_FP As FP, ByRef sender As Object, ByRef e As System.Windows.Forms.KeyPressEventArgs) Handles FP.Form_KeyPreview_KeyPress
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_KeyPreview_KeyPress(sender_FP, sender, e)
    End Sub
    Private Sub EVENT_FP_Form_Print_Begin(ByVal sender_FP As FP, ByRef Cancel As Integer) Handles FP.Form_Print_Begin
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_Print_Begin(sender_FP, Cancel)
    End Sub
    Private Sub EVENT_FP_Form_Print_Prepare(ByVal sender_FP As FP, ByVal Identifier As String, ByRef Prepared As Boolean, ByRef CancelOpenReport As Boolean) Handles FP.Form_Print_Prepare
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_Print_Prepare(sender_FP, Identifier, Prepared, CancelOpenReport)
    End Sub
    Private Sub EVENT_FP_Form_Print_End(ByVal sender_FP As FP) Handles FP.Form_Print_End
        Parent.RAISEEVENT_ARRAY_OF_FP_Form_Print_End(sender_FP)
    End Sub
    Private Sub EVENT_FP_GRID_Paint(ByVal sender_FP As FP, ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles FP.GRID_Paint
        Parent.RAISEEVENT_ARRAY_OF_FP_GRID_Paint(sender_FP, sender, e)
    End Sub
    Private Sub EVENT_GRID_Row_Drag_Begin(ByVal sender_FP As FP, ByRef DATA As String, ByVal e As System.Windows.Forms.MouseEventArgs, ByRef Cancel As Boolean) Handles FP.GRID_Row_Drag_Begin
        Parent.RAISEEVENT_ARRAY_OF_FP_GRID_Row_Drag_Begin(sender_FP, DATA, e, Cancel)
    End Sub
    Private Sub EVENT_GRID_Row_DoubleClick(ByVal sender_FP As FP, ByRef Handled As Boolean) Handles FP.GRID_Row_DoubleClick
        Parent.RAISEEVENT_ARRAY_OF_FP_GRID_Row_DoubleClick(sender_FP)
    End Sub
    Private Sub EVENT_GRID_Row_MouseWheel(ByVal sender_FP As FP, ByVal sender As Object, ByRef e As System.Windows.Forms.MouseEventArgs, ByRef Handled As Boolean) Handles FP.GRID_Row_MouseWheel
        Parent.RAISEEVENT_ARRAY_OF_FP_GRID_Row_MouseWheel(sender_FP, sender, e, Handled)
    End Sub
    Private Sub EVENT_GRID_Filter_Changed(ByVal sender_FP As FP) Handles FP.GRID_Filter_Changed
        Parent.RAISEEVENT_ARRAY_OF_FP_GRID_Filter_Changed(sender_FP)
    End Sub
    Private Sub EVENT_FP_GRID_Sorted(ByVal sender_FP As FP, ByVal e As System.EventArgs) Handles FP.GRID_Sorted
        Parent.RAISEEVENT_ARRAY_OF_FP_GRID_Sorted(sender_FP, e)
    End Sub
    Private Sub EVENT_GRID_CellClick(ByVal sender_FP As FP, ByVal FPc As FP_Control, ByRef Handled As Boolean) Handles FP.GRID_CellClick
        Parent.RAISEEVENT_ARRAY_OF_FP_GRID_CellClick(sender_FP, FPc, Handled)
    End Sub
    Private Sub EVENT_FP_EXCEL_IMPORT_Data_Records_Prepared(ByVal sender_FP As FP, ByVal sender As FP_XLS_Import, ByRef Cancel As Boolean) Handles FP.EXCEL_IMPORT_Data_Records_Prepared
        Parent.RAISEEVENT_ARRAY_OF_FP_EXCEL_IMPORT_Data_Records_Prepared(sender_FP, sender, Cancel)
    End Sub
    Private Sub EVENT_FP_EXCEL_IMPORT_Check_Data(ByVal sender_FP As FP, ByVal sender As FP_XLS_Import) Handles FP.EXCEL_IMPORT_Check_Data
        Parent.RAISEEVENT_ARRAY_OF_FP_EXCEL_IMPORT_Check_Data(sender_FP, sender)
    End Sub
    Private Sub EVENT_FP_EXCEL_IMPORT_Import_Data(ByVal sender_FP As FP, ByVal sender As FP_XLS_Import, ByRef Cancel As Boolean) Handles FP.EXCEL_IMPORT_Import_Data
        Parent.RAISEEVENT_ARRAY_OF_FP_EXCEL_IMPORT_Import_Data(sender_FP, sender, Cancel)
    End Sub
End Class
Public Class FP_L_ButtonToggleGroup

    Public Event SET_LAYOUT()
    Public Event BeforeChange(Last_FP_PictureBox As FP_PictureBox, ByRef Cancel As Integer)
    Public Event AfterChange()

    Private DIC_ToggleButtons As New Dictionary(Of String, FP_PictureBox)
    Private DefaultButton As FP_PictureBox
    Private LastPressedButton As FP_PictureBox

    Public Sub AddToggleButton(ByVal PictureBox As FP_PictureBox)
        If DIC_ToggleButtons.Count = 0 Then
            DefaultButton = PictureBox
        End If

        DIC_ToggleButtons.Add(PictureBox.c.Name, PictureBox)
        AddHandler PictureBox.CLICK, AddressOf PICTUREBOX_CLICK
    End Sub

    Public Sub SET_PRESSED(ByVal FPp As FP_PictureBox)
        PICTUREBOX_CLICK(FPp, Nothing)
    End Sub

    Public ReadOnly Property IS_ACTIVE() As FP_PictureBox
        Get
            Return LastPressedButton
        End Get
    End Property

    Private Sub PICTUREBOX_CLICK(ByVal sender_FPp As FP_PictureBox, ByVal e As MouseEventArgs)
        Dim Cancel As Integer = 0

        If LastPressedButton IsNot Nothing Then
            RaiseEvent BeforeChange(LastPressedButton, Cancel)
        End If

        If Cancel Then
            sender_FPp.P_PRESSED = False
            Exit Sub
        End If

        LastPressedButton = sender_FPp

        For Each P As KeyValuePair(Of String, FP_PictureBox) In DIC_ToggleButtons
            P.Value.P_PRESSED = (sender_FPp.c.Name = P.Value.c.Name)
        Next

        RaiseEvent SET_LAYOUT()
        RaiseEvent AfterChange()
    End Sub

    Public Sub INIT()
        PICTUREBOX_CLICK(DefaultButton, Nothing)
    End Sub

End Class
Public Class FP_L_CheckBoxToggleGroup
    Private DIC_CheckBox As New Dictionary(Of String, CheckBox)

    Public Sub AddCheckBox(ByVal CheckBox As CheckBox)
        DIC_CheckBox.Add(CheckBox.Name, CheckBox)
        AddHandler CheckBox.Click, AddressOf CHECKBOX_CLICK
    End Sub

    Private Sub CHECKBOX_CLICK(ByVal sender As CheckBox, ByVal e As MouseEventArgs)
        For Each P As KeyValuePair(Of String, CheckBox) In DIC_CheckBox
            If sender.Name = P.Value.Name Then
                If P.Value.Checked = True Then
                    P.Value.Checked = True
                Else
                    P.Value.Checked = False
                End If
            Else
                P.Value.Checked = False
            End If
        Next
    End Sub
End Class
Public Class FP_L_Dock_Forms
    Public Structure Struct_Dock_Form
        Dim ParentFP As FP
        Dim ChildFP As FP
        Dim ChildFormSize As Integer
        Dim ParentButtonName As String
        Dim ParentButtonFP As FP
        Dim ChildButtonName As String
        Dim ChildButtonFP As FP
        Dim DockTypes As ENUM_Direction
    End Structure

#Region "DECLARE"

    Public FPApp As FP_App

    Public WithEvents Parent_FP As FP
    Public WithEvents Parent_Form As Form
    Public WithEvents Parent_FP_Form As FP_Form
    Public WithEvents Parent_ToggleButton As FP_PictureBox

    Public WithEvents Child_FP As FP
    Public WithEvents Child_Form As Form
    Public WithEvents Child_FP_Form As FP_Form
    Public WithEvents Child_ToggleButton As FP_PictureBox

    Private Rate As Double
    Private DockTypes As ENUM_Direction
    Private Disposed As Boolean
    Private Docked As Boolean
    Private Cancel As Boolean = False

    Private OldForm_Parent_Location As Point
    Private OldForm_Parent_Height As Integer
    Private OldForm_Parent_Width As Integer
    Private OldForm_Parent_MaximizeBox As Boolean
    Private OldForm_Parent_WindowState As FormWindowState
    Private OldForm_Parent_BorderStyle As FormBorderStyle
    Private OldForm_Child_ControlBox As Boolean
    Private OldForm_Child_ShowInTaskbar As Boolean
    Private OldForm_Child_Height As Integer
    Private OldForm_Child_Width As Integer
    Private OldForm_Child_BorderStyle As FormBorderStyle

    Private NewForm_Child_Left As Integer
    Private NewForm_Child_Top As Integer
    Private NewForm_Child_Width As Integer
    Private NewForm_Child_Height As Integer
    Private NewForm_Child_BorderStyle As FormBorderStyle

    Private NewForm_Parent_Left As Integer
    Private NewForm_Parent_Top As Integer
    Private NewForm_Parent_Width As Integer
    Private NewForm_Parent_Height As Integer
    Private NewForm_Parent_BorderStyle As FormBorderStyle

    Private params As Struct_Dock_Form

#End Region

#Region "FORM CONSTRUCTOR"

    Public Sub New(ByVal MyParams As Struct_Dock_Form)
        params = MyParams
    End Sub

#End Region

#Region "FORM DESTRUCTOR"

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

#End Region

#Region "PUBLIC SUBS"

    Public Sub Dispose()
        If Disposed = False Then
            Disposed = True

            Parent_FP = Nothing
            Parent_Form = Nothing
            Parent_FP_Form = Nothing
            Parent_ToggleButton = Nothing
            Child_FP = Nothing
            Child_Form = Nothing
            Child_FP_Form = Nothing
            Child_ToggleButton = Nothing
        End If
    End Sub

    Public Sub INIT()
        If Disposed = False Then
            Parent_FP = params.ParentFP
            If Parent_FP Is Nothing Then
                MsgBox("FP_L_Dock_Forms.OpenDockForm hiba: a Parent_FP parameter megadasa kotelezo!")
                Exit Sub
            End If

            Parent_FP_Form = Parent_FP.FPf
            Parent_Form = Parent_FP_Form.Frm
            FPApp = Parent_FP_Form.FPApp

            Dim ParentButtonFP As FP = params.ParentButtonFP
            If ParentButtonFP Is Nothing Then
                ParentButtonFP = Parent_FP
            End If
            Try
                Parent_ToggleButton = ParentButtonFP.PICTUREBOXES(params.ParentButtonName)

            Catch ex As KeyNotFoundException
                MsgBox("FP_L_Dock_Forms.OpenDockForm hiba: nem talaltam a '" + params.ParentButtonName + "' picturebox-ot!")
                Exit Sub
            End Try

            Rate = params.ChildFormSize
            If Rate < 0 Or Rate > 100 Then
                MsgBox("FP_L_Dock_Forms.OpenDockForm hiba: a ChildFormSize paraméter értékének 0 és 100 közé kell esnie!")
                Exit Sub
            End If
            Rate = Rate / 100

            DockTypes = params.DockTypes
            If DockTypes < 0 Or DockTypes > 270 Then
                MsgBox("FP_L_Dock_Forms.OpenDockForm hiba: ervenytelen DockTypes ertek!")
                Exit Sub
            End If

            OldForm_Parent_Location = Parent_Form.Location
            OldForm_Parent_Height = Parent_Form.Height
            OldForm_Parent_Width = Parent_Form.Width
            OldForm_Parent_WindowState = Parent_Form.WindowState
            OldForm_Parent_MaximizeBox = Parent_Form.MaximizeBox
            OldForm_Parent_BorderStyle = Parent_Form.FormBorderStyle

            If params.ChildFP Is Nothing Then
                MsgBox("FP_L_Dock_Forms.OpenDockForm hiba: a ChildFP parameter megadasa kotelezo!")
                Exit Sub
            End If
            Child_FP = params.ChildFP
            Child_FP_Form = Child_FP.FPf
            Child_Form = Child_FP_Form.Frm

            OldForm_Child_ControlBox = Child_Form.ControlBox
            OldForm_Child_ShowInTaskbar = Child_Form.ShowInTaskbar
            OldForm_Child_Height = Child_Form.Height
            OldForm_Child_Width = Child_Form.Width
            OldForm_Child_BorderStyle = Child_Form.FormBorderStyle

            Child_Form.Owner = Parent_Form
        End If
    End Sub

    Public Sub REORDER()
        If Docked = True Then
            UNDOCKING()
        Else
            DOCKING()
        End If
    End Sub

#End Region

#Region "PUBLIC PROPERTIES"

    Public ReadOnly Property IsShown() As Boolean
        Get
            IsShown = Child_Form.Visible
        End Get
    End Property

    Public Property OpenCancel() As Boolean
        Get
            OpenCancel = Cancel
        End Get
        Set(ByVal value As Boolean)
            Cancel = value
        End Set
    End Property

#End Region

#Region "PRIVATE SUBS"

    Private Sub CLOSE()
        FPApp.DIC_Docks.Remove(Parent_Form.Name)

        If Child_FP.P_DATA_RecordStatus = ENUM_RecordStatus.NEWRECORD Then
            Child_FP.UNDO()
        End If

        If Docked = True Then
            UNDOCKING()
        End If

        Child_Form.Hide()
    End Sub

    Private Sub DOCKING()
        Docked = True
        Child_Form.Hide()

        SET_FORMS_SIZE_PARAMS()

        OldForm_Parent_WindowState = Parent_Form.WindowState
        Parent_Form.MaximizeBox = False
        Parent_Form.WindowState = FormWindowState.Normal

        Child_Form.ControlBox = False
        Child_Form.BringToFront()

        Select Case DockTypes
            Case ENUM_Direction.Right
                SET_PARENT_DIMENSION()
                SET_CHILD_DIMENSION()

            Case ENUM_Direction.Left
                SET_CHILD_DIMENSION()
                SET_PARENT_DIMENSION()

            Case ENUM_Direction.Bottom
                SET_PARENT_DIMENSION()
                SET_CHILD_DIMENSION()

            Case ENUM_Direction.Top
                SET_PARENT_DIMENSION()
                SET_CHILD_DIMENSION()
        End Select

        Child_ToggleButton.P_PRESSED = True
        Child_Form.Show()
    End Sub

    Private Sub SET_CHILD_DIMENSION()
        SET_FORMS_SIZE_PARAMS()

        Child_Form.Left = NewForm_Child_Left
        Child_Form.Top = NewForm_Child_Top
        Child_Form.Height = NewForm_Child_Height
        Child_Form.Width = NewForm_Child_Width
        Child_Form.FormBorderStyle = NewForm_Child_BorderStyle
    End Sub

    Private Sub SET_FORMS_SIZE_PARAMS()
        Select Case DockTypes
            Case ENUM_Direction.Right
                NewForm_Parent_Left = 0
                NewForm_Parent_Top = 0
                NewForm_Parent_Height = Screen.PrimaryScreen.WorkingArea.Height
                NewForm_Parent_Width = Screen.PrimaryScreen.WorkingArea.Width * (1 - Rate)
                NewForm_Parent_BorderStyle = FormBorderStyle.Sizable

                NewForm_Child_Left = Screen.PrimaryScreen.WorkingArea.Width * (1 - Rate)
                NewForm_Child_Top = 0
                NewForm_Child_Height = Screen.PrimaryScreen.WorkingArea.Height
                NewForm_Child_Width = Screen.PrimaryScreen.WorkingArea.Width - (Screen.PrimaryScreen.WorkingArea.Width * (1 - Rate))
                NewForm_Child_BorderStyle = FormBorderStyle.Sizable

            Case ENUM_Direction.Left
                NewForm_Child_Left = 0
                NewForm_Child_Top = 0
                NewForm_Child_Height = Screen.PrimaryScreen.WorkingArea.Height
                NewForm_Child_Width = Screen.PrimaryScreen.WorkingArea.Width - (Screen.PrimaryScreen.WorkingArea.Width * (1 - Rate))
                NewForm_Child_BorderStyle = FormBorderStyle.Sizable

                NewForm_Parent_Left = Screen.PrimaryScreen.WorkingArea.Width - (Screen.PrimaryScreen.WorkingArea.Width * (1 - Rate))
                NewForm_Parent_Top = 0
                NewForm_Parent_Height = Screen.PrimaryScreen.WorkingArea.Height
                NewForm_Parent_Width = Screen.PrimaryScreen.WorkingArea.Width * (1 - Rate)
                NewForm_Parent_BorderStyle = FormBorderStyle.FixedSingle

            Case ENUM_Direction.Bottom
                NewForm_Parent_Left = 0
                NewForm_Parent_Top = 0
                NewForm_Parent_Height = Screen.PrimaryScreen.WorkingArea.Height * (1 - Rate)
                NewForm_Parent_Width = Screen.PrimaryScreen.WorkingArea.Width
                NewForm_Parent_BorderStyle = FormBorderStyle.Sizable

                NewForm_Child_Left = 0
                NewForm_Child_Top = Screen.PrimaryScreen.WorkingArea.Height * (1 - Rate)
                NewForm_Child_Height = Screen.PrimaryScreen.WorkingArea.Height * Rate
                NewForm_Child_Width = Screen.PrimaryScreen.WorkingArea.Width
                NewForm_Child_BorderStyle = FormBorderStyle.Sizable

            Case ENUM_Direction.Top
        End Select
    End Sub

    Private Sub SET_PARENT_DIMENSION()
        SET_FORMS_SIZE_PARAMS()

        Parent_Form.Left = NewForm_Parent_Left
        Parent_Form.Top = NewForm_Parent_Top
        Parent_Form.Height = NewForm_Parent_Height
        Parent_Form.Width = NewForm_Parent_Width
        Parent_Form.FormBorderStyle = NewForm_Parent_BorderStyle
    End Sub

    Private Sub UNDOCKING()
        Docked = False
        Child_Form.Hide()

        Parent_Form.Location = OldForm_Parent_Location
        Parent_Form.Height = OldForm_Parent_Height
        Parent_Form.Width = OldForm_Parent_Width
        Parent_Form.MaximizeBox = OldForm_Parent_MaximizeBox
        Parent_Form.WindowState = OldForm_Parent_WindowState
        Parent_Form.FormBorderStyle = OldForm_Parent_BorderStyle

        Child_Form.WindowState = FormWindowState.Normal
        Child_Form.ShowInTaskbar = OldForm_Child_ShowInTaskbar
        Child_Form.Height = OldForm_Child_Height
        Child_Form.Width = OldForm_Child_Width
        Child_Form.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width / 4 * 1.1, Screen.PrimaryScreen.WorkingArea.Height / 4 * 1.1)
        Child_Form.BringToFront()

        If Child_ToggleButton IsNot Nothing Then
            Child_ToggleButton.P_PRESSED = False
        End If
        Child_Form.Show()
    End Sub

#End Region

#Region "PARENT_FORM EVENTS"

    Private Sub Parent_Form_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Parent_Form.FormClosed
        If Disposed = False Then
            Dispose()
        End If
    End Sub

    Private Sub Parent_Form_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Parent_Form.Move
        If Disposed = False Then
            If Docked = True Then
                If Child_Form.Visible = True Then
                    Parent_Form.Top = 0
                    Select Case DockTypes
                        Case ENUM_Direction.Left
                            Parent_Form.Left = Child_Form.Width

                        Case ENUM_Direction.Right
                            Parent_Form.Left = 0
                    End Select
                End If
            End If
        End If
    End Sub

    Private Sub Parent_Form_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Parent_Form.Resize
        If Disposed = False Then
            If Docked = True Then
                If Child_Form.Visible = True Then
                    Select Case DockTypes
                        Case ENUM_Direction.Right
                            Child_Form.Left = Parent_Form.Width
                            Child_Form.Height = Parent_Form.Height
                            Child_Form.Width = Screen.PrimaryScreen.WorkingArea.Width - Parent_Form.Width

                        Case ENUM_Direction.Bottom
                            Child_Form.Height = Screen.PrimaryScreen.WorkingArea.Height - Parent_Form.Height
                            Child_Form.Top = Parent_Form.Height
                            Child_Form.Width = Parent_Form.Width
                    End Select
                End If
            End If
        End If
    End Sub

#End Region

#Region "PARENT CONTROL EVENTS"

    Private Sub Parent_ToggleButton_CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Parent_ToggleButton.CLICK
        Dim OtherDocks As FP_L_Dock_Forms = Nothing

        If Cancel = True Then
            Cancel = False
            Parent_ToggleButton.P_PRESSED = False
            Exit Sub
        End If

        If Parent_ToggleButton.P_PRESSED = True Then
            If Not Parent_FP.FPf.SAVE_ALL Then
                Parent_ToggleButton.P_PRESSED = False
                Exit Sub
            End If

            If Parent_FP.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                Try
                    OtherDocks = FPApp.DIC_Docks(Parent_Form.Name)
                    OtherDocks.CLOSE()
                    OtherDocks.Parent_ToggleButton.P_PRESSED = False

                Catch ex As KeyNotFoundException

                Finally
                    FPApp.DIC_Docks.Add(Parent_Form.Name, Me)
                End Try

                Child_Form.Show()
                REORDER()
            Else
                Parent_ToggleButton.P_PRESSED = False
            End If
        Else
            CLOSE()
        End If
    End Sub

#End Region

#Region "PARENT_FP EVENTS"

    Private Sub Parent_FP_Form_Current(ByVal sender_FP As FP) Handles Parent_FP.Form_Current
        If Parent_ToggleButton.P_PRESSED = True Then
            If Parent_FP.P_DATA_RecordStatus <> ENUM_RecordStatus.EXISTS Then
                If Child_FP.P_FORM_AllowAdditions = True Then
                    Parent_ToggleButton.P_PRESSED = False
                    Parent_ToggleButton_CLICK(Nothing, Nothing)
                End If
            End If
        End If
    End Sub

#End Region

#Region "CHILD_FORM EVENTS"

    Private Sub Child_Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Child_Form.Load
        Dim ChildButtonFP As FP = params.ChildButtonFP
        If ChildButtonFP Is Nothing Then
            ChildButtonFP = params.ChildFP
        End If
        Try
            Child_ToggleButton = ChildButtonFP.PICTUREBOXES(params.ChildButtonName)
            Child_ToggleButton.P_PRESSED = True

        Catch ex As KeyNotFoundException
            MsgBox("FP_L_Dock_Forms.Child_Form_Load hiba: nem talaltam a '" + params.ChildButtonName + "' picturebox-ot!")
            Exit Sub
        End Try
    End Sub

    Private Sub Child_Form_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Child_Form.Move
        If Disposed = False Then
            If Docked = True Then
                If Child_Form.Visible = True Then
                    Select Case DockTypes
                        Case ENUM_Direction.Left
                            Child_Form.Top = 0
                            Child_Form.Left = 0

                        Case ENUM_Direction.Right
                            Child_Form.Top = 0

                        Case ENUM_Direction.Bottom
                            Child_Form.Left = 0

                        Case ENUM_Direction.Top
                    End Select
                End If
            End If
        End If
    End Sub

    Private Sub Child_Form_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Child_Form.Resize
        If Disposed = False Then
            If Docked = True Then
                If Child_Form.Visible = True Then
                    Select Case DockTypes
                        Case ENUM_Direction.Left
                            Child_Form.Height = Screen.PrimaryScreen.WorkingArea.Height
                            Parent_Form.Left = Child_Form.Width
                            Parent_Form.Width = Screen.PrimaryScreen.WorkingArea.Width - Child_Form.Width

                        Case ENUM_Direction.Right
                            Parent_Form.Height = Child_Form.Height
                            Parent_Form.Width = Screen.PrimaryScreen.WorkingArea.Width - Child_Form.Width

                        Case ENUM_Direction.Bottom
                            Child_Form.Top = Parent_Form.Height
                            Parent_Form.Height = Screen.PrimaryScreen.WorkingArea.Height - Child_Form.Height
                            Parent_Form.Width = Child_Form.Width
                    End Select
                End If
            End If
        End If
    End Sub

#End Region

#Region "CHILD CONTROL EVENTS"

    Private Sub Child_ToggleButton_CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Child_ToggleButton.CLICK
        REORDER()
    End Sub

#End Region

#Region "CHILD_FP EVENTS"

    Private Sub Child_FP_Form_FORM_CLOSING(ByVal sender As Object, ByRef e As System.Windows.Forms.FormClosingEventArgs) Handles Child_FP_Form.FORM_CLOSING
        If Disposed = False Then
            If e.CloseReason = CloseReason.UserClosing Then
                Parent_ToggleButton.P_PRESSED = False

                e.Cancel = True
            End If

            CLOSE()
        End If
    End Sub

#End Region

End Class

Public Class FP_L_Field_Sign
    'Kitesz egy pottyot a mezobe, megjelolve ezzel ot.

    Private FPf As FP_Form
    Private Sign_CONTROL As Button = Nothing
    Private Sign_ID As Integer = 0
    Private Disposed As Boolean = False

    Sub New(ByVal MyFPf As FP_Form, ByVal MySign_ID As Integer, ByVal MySign_BG_Color As Color)
        FPf = MyFPf
        Sign_ID = MySign_ID

        Sign_CONTROL = New Button
        With Sign_CONTROL
            .Name = String.Format("#FP_L_FIELD_SIGN_{0}", MySign_ID)
            .BackColor = MySign_BG_Color
            .Text = ""
            .Width = 15
            .Height = 15
        End With
    End Sub

    Public Sub Dispose()
        If Disposed = False Then
            HIDE()
            If Not (Sign_CONTROL Is Nothing) Then
                Sign_CONTROL.Dispose()
                Sign_CONTROL = Nothing
                Disposed = True
            End If
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Public Sub HIDE()
        If Disposed = False Then
            Sign_CONTROL.Parent = FPf.Frm
            Sign_CONTROL.Visible = False
        End If
    End Sub

    Public Sub SHOW(ByVal for_FPc As FP_Control)
        If Disposed = False Then
            HIDE()
            If FPc_HAS_FIELD(for_FPc) Then
                If FPf.CONTROLS.ContainsKey(for_FPc.FieldName) Then
                    'If Not for_Control.Visible Then
                    If for_FPc.P.ShowInGRID Then
                        for_FPc.FP.GRID.PREPARE_EDIT(for_FPc)
                    Else
                        FIELD_VISIBLE_WITH_ENSURE(for_FPc.c)
                    End If
                    'End If
                    With Sign_CONTROL
                        If for_FPc.c.Parent Is Nothing Then
                            .Parent = FPf.Frm
                            .Left = for_FPc.c.Right - Sign_CONTROL.Width
                            '.Left = for_FPc.c.Left
                            .Top = for_FPc.c.Top
                        ElseIf TypeOf (for_FPc.c) Is TabPage Then
                            .Parent = for_FPc.c
                            .Left = 0
                            .Top = 0
                        Else
                            If Not (TypeOf (for_FPc.c.Parent) Is TabControl Or TypeOf (for_FPc.c.Parent) Is SplitContainer) Then
                                .Parent = for_FPc.c.Parent
                            End If
                            .Left = for_FPc.c.Right - Sign_CONTROL.Width
                            '.Left = for_FPc.c.Left
                            .Top = for_FPc.c.Top
                        End If

                        .Visible = True
                        .BringToFront()
                    End With
                End If
            End If
        End If
    End Sub
End Class

Public Class FP_L_Form_with_PFD_Params
    Public Structure Struct_FP_L_Form_with_PFD_Params
        Dim FPf As FP_Form
        Dim Pfd_Key As String
    End Structure

    Public FPf As FP_Form
    Public Pfd_Key As String
    Public Pfd_Saved_Params() As String
    Private Saved_Params_DT As New DataTable
    Private Disposed As Boolean = False

    Sub New(Params As Struct_FP_L_Form_with_PFD_Params)
        With Params
            FPf = .FPf
            Pfd_Key = .Pfd_Key
        End With

        Saved_Params_DT = New DataTable
        With Saved_Params_DT
            .Columns.Add("FieldName", System.Type.GetType("System.String"))
            .Columns.Add("Value", System.Type.GetType("System.String"))
            .Columns.Add("Selected_ID", System.Type.GetType("System.Int32"))
            .Columns.Add("FieldLength", System.Type.GetType("System.Int32"))
        End With

        If Pfd_Key > "" Then
            Dim Params_Text As String = ""

            FPf.FPApp.PFDlesen(Pfd_Key, Params_Text)

            Pfd_Saved_Params = Split(Params_Text, "|")
        End If
    End Sub

    Public Sub Dispose()
        If Disposed = False Then
            FPf = Nothing

            Disposed = True
        End If
    End Sub

    Public Sub DATAFIELD_ADD(ByVal FieldName As String, ByVal DB_Formatted_Value As String, Optional ByVal Selected_ID_Value As Long = 0, Optional FieldLength As Integer = 0)
        If Saved_Params_DT.Select(String.Format("FieldName='{0}'", FieldName)).Count > 0 Then
            FPf.FPApp.DoErrorMsgBox("FP_L_Form_with_PFD_Params.DATAFIELD_ADD", 0, String.Format("FieldName '{0}' already exists.", FieldName))
        Else
            Dim Row As DataRow = Saved_Params_DT.NewRow

            With Row
                !FieldName = FieldName
                !Value = DB_Formatted_Value
                !Selected_ID = Selected_ID_Value
                !FieldLength = FieldLength
            End With

            Saved_Params_DT.Rows.Add(Row)
        End If
    End Sub

    Public Sub DATAFIELD_ADD(ByVal FieldName As String)
        Dim DB_Formatted_Value As String = ""
        Dim Selected_ID_Value As Long = 0

        If Pfd_Key > "" Then
            Dim IndexOfPfdParam As Integer = Saved_Params_DT.Rows.Count * 2
            If UBound(Pfd_Saved_Params) < IndexOfPfdParam + 1 Then
                ReDim Preserve Pfd_Saved_Params(IndexOfPfdParam + 1)
            Else
                DB_Formatted_Value = Pfd_Saved_Params(IndexOfPfdParam)
                Selected_ID_Value = Pfd_Saved_Params(IndexOfPfdParam + 1)
            End If
        End If

        DATAFIELD_ADD(FieldName, DB_Formatted_Value, Selected_ID_Value)
    End Sub

    Public Function LOAD_Fields_From_PFD() As Boolean
        Return DATAFIELD_SET_ALL_FIELDS_FROM_DT()
    End Function

    Public Function DATAFIELD_SET_ALL_FIELDS_FROM_DT() As Boolean
        Dim OUT As Boolean = False

        Dim AktKey As String

        For Each Current_FP_ID As Integer In FPf.FPs.Keys
            Dim Curr_FP As FP = FPf.FPs(Current_FP_ID)

            For Each AktKey In Curr_FP.CONTROLS.Keys
                If Saved_Params_DT.Select(String.Format("FieldName='{0}'", AktKey)).Count > 0 Then
                    Dim Row As DataRow = Saved_Params_DT.Select(String.Format("FieldName='{0}'", AktKey)).First

                    If Not Row Is Nothing Then
                        With Row
                            If !FieldLength > 0 Then
                                Curr_FP.CONTROLS(AktKey).P.xlength = !FieldLength
                            End If
                            Curr_FP.CONTROLS(AktKey).SET_VALUE_from_DBFORMAT(nz(!Value, ""))
                            Curr_FP.CONTROLS(AktKey).Selected_ID = nz(!Selected_ID, 0)
                        End With
                    End If
                End If
            Next

            Curr_FP.COLORING_ALL()
        Next Current_FP_ID

        Return OUT
    End Function

    Public Function DATAFIELD_WRITE_VALUES_INTO_DT() As Boolean
        Dim OUT As Boolean = True
        Dim AktKey As String = ""

        For Each DRow As DataRow In Saved_Params_DT.Rows
            If FPf.CONTROLS.ContainsKey(DRow!FieldName) Then
                Dim FPc As FP_Control = Nothing

                If FPf.CONTROLS_GET_FPo_FROM_CONTROL(FPf.CONTROLS(DRow!FieldName), FPc) Then
                    Dim DBFormattedValue As String = ""
                    FPc.GET_DBFORMAT_from_CONTROL(DBFormattedValue)
                    With DRow
                        .BeginEdit()
                        !FieldName = FPc.FieldName
                        !Value = DBFormattedValue
                        !Selected_ID = FPc.Selected_ID
                        .EndEdit()
                    End With
                End If
            End If
        Next DRow

        DATAFIELD_WRITE_VALUES_INTO_DT = OUT
    End Function

    Public Function DATAFIELD_GET(ByVal FieldName As String, Optional ByRef OUT_Selected_ID As Long = 0) As String
        Dim OUT As String = ""

        OUT_Selected_ID = 0

        If DATAFIELD_WRITE_VALUES_INTO_DT() Then
            If Saved_Params_DT.Select(String.Format("FieldName='{0}'", FieldName)).Count <> 1 Then
                FPf.FPApp.DoErrorMsgBox("FP_L_Form_with_PFD_Params.DataField_GET", 0, String.Format("Field '{0}' not found.", FieldName))
            Else
                Dim Row As DataRow = Saved_Params_DT.Select(String.Format("FieldName='{0}'", FieldName)).First

                If Row Is Nothing Then
                    FPf.FPApp.DoErrorMsgBox("FP_L_Form_with_PFD_Params.DATAFIELD_GET", 0, String.Format("Field '{0}' not found.", FieldName))
                Else
                    OUT = nz(Row!Value, "")
                    OUT_Selected_ID = nz(Row!Selected_ID, 0)
                End If
            End If
        End If

        Return OUT
    End Function
    Public Function Field_Contains(FieldName As String) As Boolean
        Dim OUT As Boolean = False
        Dim FieldCrit As String = String.Format("FieldName = '{0}'", FieldName)
        Dim DRows() As DataRow = Saved_Params_DT.Select(FieldCrit)
        If DRows.Length > 0 Then
            OUT = True
        End If
        Return OUT
    End Function

    Public Function DATAFIELD_GET_AS_DELIMITED_STRING() As String
        Dim OUT As String = ""

        If DATAFIELD_WRITE_VALUES_INTO_DT() Then
            For i As Integer = 0 To Saved_Params_DT.Rows.Count - 1
                Dim Row As DataRow = Saved_Params_DT.Rows(i)

                OUT += String.Format("{0};{1};{2}|", nz(Row!FieldName, ""), nz(Row!Value, ""), nz(Row!Selected_ID, 0))
            Next
        End If

        Return OUT
    End Function

    Public Function DATAFIELD_GET_AS_PFD_STRING() As String
        Dim OUT As String = ""

        If DATAFIELD_WRITE_VALUES_INTO_DT() Then
            For i As Integer = 0 To Saved_Params_DT.Rows.Count - 1
                Dim Row As DataRow = Saved_Params_DT.Rows(i)

                OUT += String.Format("{0}|{1}|", nz(Row!Value, ""), nz(Row!Selected_ID, 0))
            Next
        End If

        Return OUT
    End Function

    Public Sub SAVE_TO_PFD()
        If Pfd_Key > "" Then
            Dim Params As String = DATAFIELD_GET_AS_PFD_STRING()

            FPf.FPApp.PFDinsertOrUpdate(Pfd_Key, Params)
        End If
    End Sub
End Class
Public Class FP_L_GRID_ButtonShowAll

#Region "DECLARE"

    Public Structure Struct_FP_L_GRID_ButtonShowAll
        Dim Parent_FP As FP
        Dim Parent_ID_Name As String
        Dim Child_FP As FP
        Dim ToggleButton As FP_PictureBox
    End Structure

    Public FPf As FP_Form
    Public FPApp As FP_App
    Public WithEvents Parent_FP As FP
    Public WithEvents Child_FP As FP
    Public WithEvents ToggleButton As FP_PictureBox
    Public WithEvents PictureBox As PictureBox
    Public Parent_ID_Name As String = ""

    Private Disposed As Boolean
    Private DO_SET_GRID_RS As Boolean

#End Region

#Region "FORM CONSTRUCTOR"

    Public Sub New(ByVal MyParams As Struct_FP_L_GRID_ButtonShowAll)
        If Disposed = False Then
            Parent_FP = MyParams.Parent_FP
            If Parent_FP Is Nothing Then
                MsgBox("FP_L_GRID_ButtonShowAll.New hiba: a Parent_FP parameter megadasa kotelezo!")
                Exit Sub
            End If

            Child_FP = MyParams.Child_FP
            If Child_FP Is Nothing Then
                MsgBox("FP_L_GRID_ButtonShowAll.New hiba: a Child_FP parameter megadasa kotelezo!")
                Exit Sub
            End If

            ToggleButton = MyParams.ToggleButton
            If ToggleButton Is Nothing Then
                MsgBox("FP_L_GRID_ButtonShowAll.New hiba: a ToggleButton parameter megadasa kotelezo!")
                Exit Sub
            End If
            PictureBox = ToggleButton.c

            Parent_ID_Name = MyParams.Parent_ID_Name
            If Parent_ID_Name = "" Then
                MsgBox("FP_L_GRID_ButtonShowAll.New hiba: a Parent_ID_Name parameter megadasa kotelezo!")
                Exit Sub
            End If

            If Not (Parent_FP.FPf.HELP_Frm Is Nothing) Then
                With Parent_FP.FPf.HELP_Frm
                    .ADD_HELP_STANDARD_ITEM("###GRID_ButtonShowAll###", PictureBox.Name)
                End With
            End If
        End If
    End Sub

#End Region

#Region "FORM DESTRUCTOR"

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

#End Region

#Region "PUBLIC SUBS"

    Public Sub Dispose()
        If Disposed = False Then
            Parent_FP = Nothing
            Child_FP = Nothing
            ToggleButton = Nothing

            Disposed = True
        End If
    End Sub

#End Region

#Region "PARENT_FP EVENTS"

    Private Sub Parent_FP_Form_Current(ByVal sender_FP As FP) Handles Parent_FP.Form_Current
        Dim WHERE As String = String.Empty

        If ToggleButton.P_PRESSED = False Then
            WHERE = String.Format("Parent_RS_ID={0} AND {1}={2}", Parent_FP.RS_ID, Parent_ID_Name, Parent_FP.P_DATA_Current_ID)

            Dim NoRecordOK As Boolean = Not Child_FP.P_FORM_AllowAdditions
            Child_FP.FORM_RECORDS_LOAD(WHERE, , NoRecordOK, True)
        Else
            Dim Child_ID As Integer = Val(gl_FPApp.DC.DLookup("ID", Child_FP.SQL_BIND_Params.NameOf_WhereQuery, String.Format("{0}={1}", Parent_ID_Name, Parent_FP.P_DATA_Current_ID)))
            Child_FP.FORM_GOTO_RECORD_BY_ID(Child_ID)
        End If
    End Sub

#End Region

#Region "CHILD_FP EVENTS"

    Private Sub Child_FP_Form_Current(ByVal sender_FP As FP) Handles Child_FP.Form_Current
        If ToggleButton.P_PRESSED = True Then
            If Child_FP.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                Dim Parent_ID As Integer = gl_FPApp.DC.DLookup(Parent_ID_Name, Child_FP.SQL_BIND_Params.NameOf_WhereQuery, String.Format("ID={0}", Child_FP.P_DATA_Current_ID))
                Parent_FP.FORM_GOTO_RECORD_BY_ID(Parent_ID)
            End If
        End If
    End Sub

#End Region

#Region "CONTROL EVENTS"

    Private Sub PictureBox_ShowAll_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox.MouseUp
        Dim WHERE As String = String.Empty

        If ToggleButton.P_PRESSED = True Then
            WHERE = String.Format("Parent_RS_ID={0}", Parent_FP.RS_ID)
        Else
            WHERE = String.Format("Parent_RS_ID={0} AND {1}={2}", Parent_FP.RS_ID, Parent_ID_Name, Parent_FP.P_DATA_Current_ID)
        End If

        Dim NoRecordOK As Boolean = Not Child_FP.P_FORM_AllowAdditions
        Child_FP.FORM_RECORDS_LOAD(WHERE, , NoRecordOK, True)
    End Sub

#End Region

End Class
Public Class FP_L_GRID_RS_Select_Checkbox
    Public Event Selection_Changed(ByVal sender As FP_L_GRID_RS_Select_Checkbox, ByVal DTofChangedIDs As DataTable)

    Public WithEvents FPc_RS_Select_Checkbox As FP_Control
    Public SaveChangesImmediately As Boolean = True

    Public WithEvents FP As FP

    Private WithEvents c_RS_Select_Checkbox As CheckBox
    Private Disposed As Boolean = False
    Private Data_Binded_Internal As Boolean = True

    Sub New(ByVal MyFPc_RS_Select_Checkbox As FP_Control, Optional ByVal MySaveChangesImmediately As Boolean = True)
        If Not (TypeOf (MyFPc_RS_Select_Checkbox.c) Is CheckBox) Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_GRID_RS_Select_Checkbox.New", 0, "MyFPc_RS_Select_Checkbox is not a checkbox!")
        Else
            FPc_RS_Select_Checkbox = MyFPc_RS_Select_Checkbox
            FP = FPc_RS_Select_Checkbox.FP
            c_RS_Select_Checkbox = FPc_RS_Select_Checkbox.c
            SaveChangesImmediately = MySaveChangesImmediately
        End If
    End Sub

    Sub Dispose()
        If Not Disposed Then
            FPc_RS_Select_Checkbox = Nothing
            FP = Nothing
            Disposed = True
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Public ReadOnly Property P_Count_Of_Selected() As Integer
        Get
            Dim Criteria As String = String.Format("{0} = 1", FPc_RS_Select_Checkbox.FieldName)

            Return FP.GRID.DT.Select(Criteria).Count
        End Get
    End Property

    Public ReadOnly Property P_Is_All_Selected As Boolean
        Get
            Dim OUT As Boolean = False

            If FP.P_DATA_RecordCount > 0 Then
                If (P_Count_Of_Selected = FP.P_DATA_RecordCount) Then
                    OUT = True
                End If
            End If

            Return OUT
        End Get
    End Property

    Private Sub REFRESH_FPc_RS_Select_Checkbox()
        Dim gl_Data_Binded_OLD As Boolean = gl_Data_Binded
        gl_Data_Binded = False

        If SaveChangesImmediately Then
            Select Case FP.P_DATA_RecordStatus
                Case ENUM_RecordStatus.NORECORD
                    FPc_RS_Select_Checkbox.P_VALUE = False

                Case ENUM_RecordStatus.NEWRECORD
                    If Not (FPc_RS_Select_Checkbox.c Is Nothing) Then
                        FPc_RS_Select_Checkbox.P_VALUE = (FP.GRID.ROW_GET_CURRENT(True)).Item(FPc_RS_Select_Checkbox.FieldName)
                    End If

                Case ENUM_RecordStatus.EXISTS
                    Dim Crit As String = String.Format("RecordID = {0}", FP.P_DATA_Current_ID)

                    FPc_RS_Select_Checkbox.P_VALUE = FP.GRID.DT.Select(Crit).First.Item(FPc_RS_Select_Checkbox.FieldName)

                    'Dim SQL As String = String.Format("SELECT Selected FROM RS_L WITH (READUNCOMMITTED) WHERE RS_L.RS_ID = {0} And RS_L.RecordID = {1}", FP.RS_ID, FP.P_DATA_Current_ID)
                    'Dim DA As New SqlDataAdapter(SQL, FP.FPf.FPApp.DC.CNN)
                    'Dim DT As New DataTable

                    'Try
                    '    DA.Fill(DT)

                    'Catch ex As Exception
                    '    FP.FPf.FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_GRID_RS_Select_Checkbox.FP_Form_Current", Err.Number, Err.Description)
                    'End Try

                    'If DT.Rows.Count <> 1 Then
                    '    FP.FPf.FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_GRID_RS_Select_Checkbox.FP_Form_Current", 0, "Recordcount <> 1.")
                    'Else
                    '    FPc_RS_Select_Checkbox.P_VALUE = DT.Select.First!Selected
                    'End If
            End Select
        Else
            If Not (FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD) Then
                FPc_RS_Select_Checkbox.P_VALUE = (FP.GRID.ROW_GET_CURRENT(True)).Item(FPc_RS_Select_Checkbox.FieldName)
            End If
        End If

        gl_Data_Binded = gl_Data_Binded_OLD
    End Sub

    Private Sub REFRESH_GRID(Optional ByVal SetFocusOnSelectedCheckbox As Boolean = True)
        If FP.GRID_EXISTS Then
            Dim FirstDisplayedRowIndex As Integer = FP.GRID.GRID.FirstDisplayedScrollingRowIndex
            FP.GRID.FILL()
            FP.GRID.GOTO_ROW_BY_RECORDID(FP.P_DATA_Current_ID)
            'If FP.GRID.GRID.RowCount - 1 < FirstDisplayedRowIndex Then
            '    FirstDisplayedRowIndex = -1
            'End If
            'FP.GRID.GOTO_ROW_BY_RECORDID(FP.P_DATA_Current_ID, , , FirstDisplayedRowIndex)
            If SetFocusOnSelectedCheckbox Then
                FP.FPf.FOCUS_ON_AT_THE_END(FPc_RS_Select_Checkbox.c)
            End If
        End If
    End Sub

    Private Sub FP_Form_Current(ByVal sender_FP As FP) Handles FP.Form_Current
        REFRESH_FPc_RS_Select_Checkbox()
    End Sub

    Public Function COLLECT_IDs_To_DelimitedStr(Optional ByVal OnlySelected As Boolean = False) As String
        Dim OUT As String = ""

        If Not FP.GRID_EXISTS Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_GRID_RS_Select_Checkbox.COLLECT_IDs_To_DelimitedStr", 0, "GRID does not exists.")
        Else
            If Not (FP.GRID.DT Is Nothing) Then
                If FP.GRID.DT.Rows.Count > 0 Then
                    Dim Criteria As String = ""

                    If OnlySelected Then
                        Criteria = String.Format("{0} = 1", c_RS_Select_Checkbox.Name)
                    End If

                    For Each row As DataRow In FP.GRID.DT.Select(Criteria)
                        If Not row.RowState = DataRowState.Deleted Then
                            OUT += row.Item("RecordID").ToString + "|"
                        End If
                    Next
                End If
            End If
        End If

        COLLECT_IDs_To_DelimitedStr = OUT
    End Function

    Public Sub SET_CHECKBOXES_FROM_DelimitedStr(MyDelimitedStr As String, Optional Delimiter As String = "|")
        Dim sqlComm As SqlCommand = gl_FPApp.DC.CNN.CreateCommand()

        With gl_FPApp.DC
            .Qdf_set_SP(sqlComm, "RS_Selected_SET_FROM_Delimited_STR")
            .Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , FP.RS_ID)
            .Qdf_AddParameter(sqlComm, "@Delimited_STR", SqlDbType.NVarChar, , -1, MyDelimitedStr)
            .Qdf_AddParameter(sqlComm, "@Delimiter", SqlDbType.NVarChar, , 128, Delimiter)
        End With

        CURSOR_SHOW_WAIT()
        Try
            gl_FPApp.DC.Qdf_Execute("", sqlComm)

        Catch ex As Exception
            gl_FPApp.DoErrorMsgBox("FP_L_GRID_RS_Select_Checkbox.SET_CHECKBOXES_FROM_DelimitedStr", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        REFRESH_CHECKBOXES()
    End Sub

    Private Function CREATE_DT_for_IDs() As DataTable
        Dim OUT As DataTable = Nothing

        OUT = New DataTable("RecordIDs")
        OUT.Columns.Add("RecordID", System.Type.GetType("System.Int32"))
        OUT.Columns.Add("Selected", System.Type.GetType("System.Boolean"))

        CREATE_DT_for_IDs = OUT
    End Function

    Public Function COLLECT_CurrentID_To_DT() As DataTable
        Dim OUT As DataTable = CREATE_DT_for_IDs()

        If Not (FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD) Then
            Dim Row As DataRow = OUT.NewRow

            Row.BeginEdit()
            Row!RecordID = FP.P_DATA_Current_ID
            Row!Selected = (FPc_RS_Select_Checkbox.c_ChkBox.Checked)
            Row.EndEdit()

            OUT.Rows.Add(Row)
        End If

        COLLECT_CurrentID_To_DT = OUT
    End Function

    Public Function COLLECT_IDs_To_DT(Optional ByVal Collect_ALL As Boolean = False) As DataTable
        Dim OUT As DataTable = Nothing

        If Not FP.GRID_EXISTS Then
            FP.FPf.FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_GRID_RS_Select_Checkbox.COLLECT_IDs_To_DT", 0, "GRID does not exists.")
        Else
            OUT = CREATE_DT_for_IDs()

            If Not (FP.GRID.DT Is Nothing) Then
                If FP.GRID.DT.Rows.Count > 0 Then
                    Dim Criteria As String = ""

                    If Collect_ALL = False Then
                        Criteria = String.Format("{0} = 1", FPc_RS_Select_Checkbox.FieldName)
                    End If

                    For Each row As DataRow In FP.GRID.DT.Select(Criteria)
                        If Not row.RowState = DataRowState.Deleted Then
                            Dim NewRow As DataRow = OUT.NewRow

                            NewRow.BeginEdit()
                            NewRow!RecordID = row!RecordID
                            NewRow!Selected = row.Item(FP.CONTROLS_GET_FieldName_Without_FieldPrefix(c_RS_Select_Checkbox.Name))
                            NewRow.EndEdit()
                            OUT.Rows.Add(NewRow)
                        End If
                    Next
                End If
            End If
        End If

        Return OUT
    End Function

    Private Sub c_RS_Select_Checkbox_CheckedChanged(sender As Object, e As EventArgs) Handles c_RS_Select_Checkbox.CheckedChanged
        If Data_Binded_Internal Then
            If FP.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                If FP.FORM_RECORDS_SAVE_CURRENT Then
                    If SaveChangesImmediately Then
                        Dim SQL As String = String.Format("UPDATE RS_L SET Selected = {0} FROM RS_L WITH (READUNCOMMITTED) WHERE RS_L.RS_ID = {1} And RS_L.RecordID = {2}", IIf(c_RS_Select_Checkbox.Checked, "1", "0"), FP.RS_ID, FP.P_DATA_Current_ID)

                        FP.FPf.FPApp.DC.Qdf_RunSQL(SQL)
                        FP.FORM_DORESYNC()
                        'REFRESH_GRID()
                    Else
                        Dim Criteria As String = String.Format("RecordID = {0}", FP.P_DATA_Current_ID)
                        Dim Row As DataRow = FP.GRID.DT.Select(Criteria).First

                        If Row.Item(c_RS_Select_Checkbox.Name) <> (c_RS_Select_Checkbox.Checked) Then
                            Row.BeginEdit()
                            Row.Item(c_RS_Select_Checkbox.Name) = (c_RS_Select_Checkbox.Checked)
                            Row.EndEdit()
                        End If
                    End If

                    Dim DTofChangedIDs As DataTable = COLLECT_CurrentID_To_DT()

                    RaiseEvent Selection_Changed(Me, DTofChangedIDs)
                End If
            End If
        End If
    End Sub

    Public Sub REFRESH_CHECKBOXES(Optional ByVal SetFocusOnSelectedCheckbox As Boolean = True)
        REFRESH_GRID(SetFocusOnSelectedCheckbox)
        REFRESH_FPc_RS_Select_Checkbox()
    End Sub

    Public Sub SELECT_ALL()
        Dim e As New System.Windows.Forms.KeyPressEventArgs("+")

        EVENT_FPc_RS_Select_Checkbox_Field_KeyPreview_KeyPress(FPc_RS_Select_Checkbox, FPc_RS_Select_Checkbox.c, e)
    End Sub

    Public Sub UNSELECT_ALL()
        Dim e As New System.Windows.Forms.KeyPressEventArgs("-")

        EVENT_FPc_RS_Select_Checkbox_Field_KeyPreview_KeyPress(FPc_RS_Select_Checkbox, FPc_RS_Select_Checkbox.c, e)
    End Sub

    Public Sub SELECT_CURRENT()
        FPc_RS_Select_Checkbox.c_ChkBox.Checked = True
    End Sub

    Public Sub UNSELECT_CURRENT()
        FPc_RS_Select_Checkbox.c_ChkBox.Checked = False
    End Sub

    Public Sub EVENT_FPc_RS_Select_Checkbox_Field_KeyPreview_KeyPress(ByVal sender_FPc As FP_Control, ByVal sender As Object, ByRef e As System.Windows.Forms.KeyPressEventArgs) Handles FPc_RS_Select_Checkbox.Field_KeyPreview_KeyPress
        If FP.GRID_EXISTS Then
            If InStr("+-*", e.KeyChar) > 0 Then
                e.Handled = True
                If SaveChangesImmediately Then
                    If Not FP.GRID.P_FilterActive Then
                        Dim SQL As String = ""

                        If e.KeyChar = "+" Then
                            SQL = String.Format("UPDATE RS_L SET Selected = 1 FROM RS_L WITH (READUNCOMMITTED) WHERE RS_ID = {0}", FP.RS_ID)

                        ElseIf e.KeyChar = "-" Then
                            SQL = String.Format("UPDATE RS_L SET Selected = 0 FROM RS_L WITH (READUNCOMMITTED) WHERE RS_ID = {0}", FP.RS_ID)

                        ElseIf e.KeyChar = "*" Then
                            SQL = String.Format("UPDATE RS_L SET Selected = CASE WHEN Selected = 1 THEN 0 ELSE 1 END FROM RS_L WITH (READUNCOMMITTED) WHERE RS_ID = {0}", FP.RS_ID)

                        Else
                            FP.FPf.FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_GRID_RS_Select_Checkbox.c_RS_Select_Checkbox_KeyPress", 0, String.Format("Unknown function character '{0}'", e.KeyChar))
                        End If

                        If SQL > "" Then
                            FP.FPf.FPApp.DC.Qdf_RunSQL(SQL)
                            Dim Data_Binded_Internal_OLD = Data_Binded_Internal
                            Data_Binded_Internal = False
                            REFRESH_GRID()
                            REFRESH_FPc_RS_Select_Checkbox()
                            Data_Binded_Internal = Data_Binded_Internal_OLD
                            'e.Handled = True
                        End If
                    Else
                        Dim IDs As String = COLLECT_IDs_To_DelimitedStr()
                        Dim Result As Boolean

                        With FP.FPf.FPApp.DC
                            Dim sqlComm As SqlCommand = .CNN.CreateCommand()
                            .Qdf_set_SP(sqlComm, "RS_SET_Selected", 0)
                            .Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , FP.RS_ID)
                            .Qdf_AddParameter(sqlComm, "@IDs", SqlDbType.NVarChar, , -1, IDs)
                            .Qdf_AddParameter(sqlComm, "@Func", SqlDbType.NVarChar, , 10, e.KeyChar)
                            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)

                            CURSOR_SHOW_WAIT()
                            Try
                                Result = .Qdf_Execute(FP.FPf, sqlComm)

                            Catch ex As Exception
                                Result = False
                            End Try
                            CURSOR_SHOW_DEFAULT()
                        End With

                        If Result Then
                            Dim Data_Binded_Internal_OLD = Data_Binded_Internal
                            Data_Binded_Internal = False

                            REFRESH_GRID()
                            REFRESH_FPc_RS_Select_Checkbox()

                            Data_Binded_Internal = Data_Binded_Internal_OLD
                        End If
                    End If
                Else
                    Dim RecordIDs As DataTable = COLLECT_IDs_To_DT()
                    If FP.P_DATA_Binded Then
                        FP.DATA_Binded = False

                        CURSOR_SHOW_WAIT()
                        If FP.GRID.DT.Rows.Count > 0 Then
                            For Each row As DataRow In FP.GRID.DT.Rows
                                If Not row.RowState = DataRowState.Deleted Then
                                    row.BeginEdit()
                                    Select Case e.KeyChar
                                        Case "+" : row.Item(c_RS_Select_Checkbox.Name) = True
                                        Case "-" : row.Item(c_RS_Select_Checkbox.Name) = False
                                        Case "*" : row.Item(c_RS_Select_Checkbox.Name) = Not (row.Item(c_RS_Select_Checkbox.Name))
                                        Case Else
                                            FP.FPf.FPApp.DoErrorMsgBox("FP_FieldLogics.FPc_RS_Select_Checkbox", 0, String.Format("Unknown functionkey '{0}'", e.KeyChar))
                                    End Select
                                    row.EndEdit()
                                End If
                            Next

                            For Each row As DataRow In FP.GRID.DT_ALL_FIELDS.Rows
                                If Not row.RowState = DataRowState.Deleted Then
                                    row.BeginEdit()
                                    Select Case e.KeyChar
                                        Case "+" : row.Item(c_RS_Select_Checkbox.Name) = True
                                        Case "-" : row.Item(c_RS_Select_Checkbox.Name) = False
                                        Case "*" : row.Item(c_RS_Select_Checkbox.Name) = Not (row.Item(c_RS_Select_Checkbox.Name))
                                        Case Else
                                            FP.FPf.FPApp.DoErrorMsgBox("FP_FieldLogics.FPc_RS_Select_Checkbox", 0, String.Format("Unknown functionkey '{0}'", e.KeyChar))
                                    End Select
                                    row.EndEdit()
                                End If
                            Next
                        End If

                        REFRESH_FPc_RS_Select_Checkbox()

                        FP.DATA_Binded = True
                    End If
                    CURSOR_SHOW_DEFAULT()
                End If

                Dim DTofChangedIDs As DataTable = COLLECT_IDs_To_DT(True)

                RaiseEvent Selection_Changed(Me, DTofChangedIDs)

            End If
        End If
    End Sub

    Private Sub FP_GRID_CellClick(ByVal sender_FP As FP, ByVal FPc As FP_Control, ByRef Handled As Boolean) Handles FP.GRID_CellClick
        If FPc.Equals(FPc_RS_Select_Checkbox) Then
            FPc_RS_Select_Checkbox.P_VALUE = (Not FPc_RS_Select_Checkbox.P_VALUE)
        End If
    End Sub

    Private Sub FP_GRID_Filter_Deactivated(sender_FP As FP) Handles FP.GRID_Filter_Deactivated
        REFRESH_GRID(False)
    End Sub
End Class

Public Class FP_L_DISPO_DragDrop
    Public DISPO As FP_L_DISPO
    Public FROM_Module As FP_L_Dispo_Module
    WithEvents FROM_TV As TreeView
    Public FROM_Level As Integer

    Public TO_Module As FP_L_Dispo_Module
    WithEvents TO_TV As TreeView
    Public TO_Level As Integer

    Public Enabled As Boolean

    Private Disposed As Boolean = False

    Public Sub New(ByVal MyP As Struct_FP_L_DISPO_DragDropOptions)
        With MyP
            DISPO = .DISPO
            Enabled = .Enabled
            FROM_Module = .FROM_Module
            FROM_Level = .FROM_Level
            FROM_TV = FROM_Module.TV_With_FP.TV
            TO_Module = .TO_Module
            TO_Level = .TO_Level
            TO_TV = TO_Module.TV_With_FP.TV
        End With
        TO_TV.AllowDrop = True
    End Sub

    Public Sub Dispose()
        FROM_TV = Nothing
        TO_TV = Nothing

        Disposed = True
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Private Sub EVENT_FROM_TV_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FROM_TV.MouseDown
        If Enabled Then
            If e.Button = MouseButtons.Right Then
                Dim Cancel As Integer = 0
                Dim DraggedNode As TreeNode = FROM_TV.GetNodeAt(e.Location)
                Dim AktLevel As Integer = -1

                If Not (DraggedNode Is Nothing) Then
                    AktLevel = Val(Mid(DraggedNode.Name, 1, 1))
                End If

                If AktLevel = FROM_Level Then
                    Dim NewNode_FROM As New Struct_FP_L_Dispo_Node
                    With NewNode_FROM
                        .Dispo_Module = FROM_Module
                        .Node = DraggedNode
                        .Level = AktLevel
                        If AktLevel < 0 Then
                            .ID = 0
                            .FP = Nothing
                        Else
                            .ID = Val(Mid(DraggedNode.Name, 3))
                            .FP = FROM_Module.TV_With_FP.Array_Of_FP.DIC_FPs(AktLevel).FP
                        End If
                    End With

                    DISPO.RAISEEVENT_NodeDragBegin(NewNode_FROM, Cancel)
                    If Not Cancel Then
                        FROM_TV.DoDragDrop(DISPO.DragDrop_get_IdentifierKey, DragDropEffects.Move)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub EVENT_TO_TV_DRAGDROP(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TO_TV.DragDrop
        If Not Disposed Then
            Dim DragDrop_TO As New Struct_FP_L_Dispo_Node

            With DragDrop_TO
                .Dispo_Module = TO_Module
                .Node = TO_TV.GetNodeAt(TO_TV.PointToClient(New Point(e.X, e.Y)))
                If IsNothing(.Node) Then
                    .ID = 0
                    .Root_ID = 0
                    .Level = -1
                Else
                    .ID = Val(Mid(.Node.Name, 3))

                    Dim Root_Node As TreeNode = .Dispo_Module.TV_With_FP.NODE_GET_ROOT_Node(.Node)
                    .Root_ID = Val(Mid(Root_Node.Name, 3, 1))
                    .Level = Val(Mid(.Node.Name, 1, 1))
                End If
            End With

            If DragDrop_TO.Level = TO_Level Then
                DISPO.RAISEEVENT_NodeDrop(e, Me, DragDrop_TO)
            End If
        End If
    End Sub

    Private Sub TO_TV_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TO_TV.DragOver
        If Not Disposed Then
            e.Effect = DISPO.DragDrop_GET_Current_Effect(TO_TV)
        End If
    End Sub
End Class
Public Class FP_L_DISPO
    Public Event NodeDragBegin(ByVal sender As FP_L_DISPO, ByVal P As Struct_FP_L_Dispo_Node, ByRef Cancel As Boolean)
    Public Event NodeDrop(ByVal sender As FP_L_DISPO, ByVal e As System.Windows.Forms.DragEventArgs, ByVal Dispo_Node_FROM As Struct_FP_L_Dispo_Node, ByVal Dispo_Node_TO As Struct_FP_L_Dispo_Node)

    Public Dispo_ID As Long
    Public FPf As FP_Form
    Public DIC_Dispo_Modules As Dictionary(Of Integer, FP_L_Dispo_Module)
    Public DIC_DragDrops As Dictionary(Of Integer, FP_L_DISPO_DragDrop)
    Private Disposed As Boolean = False
    Protected Friend DragDrop_FROM As New Struct_FP_L_Dispo_Node
    Protected Friend DragDrop_Handled As Boolean = True

    Public Function DragDrop_get_IdentifierKey() As String
        DragDrop_get_IdentifierKey = FPf.Frm.Handle.ToString.ToString + "_FP_L_DISPO"
    End Function

    Private Function Node_GET_Root_Node(ByVal MyNode As TreeNode) As TreeNode
        Dim OUT As TreeNode = Nothing

        If Not (MyNode Is Nothing) Then
            Do While Not (MyNode.Parent Is Nothing)
                MyNode = MyNode.Parent
            Loop

            OUT = MyNode
        End If

        Return OUT
    End Function

    Public Function Node_GET_FP_L_DISPO_Module(ByVal MyNode As TreeNode) As FP_L_Dispo_Module
        Dim OUT As FP_L_Dispo_Module = Nothing

        If Not (MyNode Is Nothing) Then
            For Each Key As String In DIC_Dispo_Modules.Keys
                With DIC_Dispo_Modules(Key)
                    If Not (.TV_With_FP Is Nothing) Then
                        If MyNode.TreeView.Equals(.TV_With_FP.TV) Then
                            OUT = DIC_Dispo_Modules(Key)
                            Exit For
                        End If
                    End If
                End With
            Next
        End If

        Return OUT
    End Function

    Public Function Node_GET_FP(ByVal MyNode As TreeNode) As FP
        Dim OUT As FP = Nothing

        Dim MyModule As FP_L_Dispo_Module = Node_GET_FP_L_DISPO_Module(MyNode)

        If Not MyModule Is Nothing Then
            With MyModule
                If Not (.TV_With_FP Is Nothing) Then
                    If MyNode.TreeView.Equals(.TV_With_FP.TV) Then
                        OUT = .TV_With_FP.Array_Of_FP.DIC_FPs(Val(Mid(MyNode.Name, 1, 1))).FP
                    End If
                End If
            End With
        End If

        Return OUT
    End Function

    Public Function Node_GET_FP_L_Node(ByVal MyDispoModule As FP_L_Dispo_Module) As Struct_FP_L_Dispo_Node
        Dim OUT As New Struct_FP_L_Dispo_Node

        With OUT
            If Not (MyDispoModule Is Nothing) Then
                .Dispo_Module = MyDispoModule
            End If
            .Node = Nothing
            .ID = 0
            .Level = -1
            .FP = Nothing
            .Root_ID = 0
        End With

        Return OUT
    End Function

    Public Function Node_GET_FP_L_Node(ByVal MyNode As TreeNode) As Struct_FP_L_Dispo_Node
        Dim OUT As New Struct_FP_L_Dispo_Node

        If Not (MyNode Is Nothing) Then
            With OUT
                .Dispo_Module = Node_GET_FP_L_DISPO_Module(MyNode)
                .Node = MyNode
                .ID = Val(Mid(MyNode.Name, 3))
                .Level = Val(Mid(MyNode.Name, 1, 1))
                .FP = Node_GET_FP(MyNode)
                .Root_ID = Val(Mid(Node_GET_Root_Node(MyNode).Name, 3))
            End With
        End If

        Return OUT
    End Function

    Public Function Node_GET_FP_L_Root_Node(ByVal MyNode As TreeNode) As Struct_FP_L_Dispo_Node
        Return Node_GET_FP_L_Node(Node_GET_Root_Node(MyNode))
    End Function

    Protected Friend Function DragDrop_GET_Current_Effect(ByVal Active_TV As TreeView) As DragDropEffects
        Dim OUT As DragDropEffects = DragDropEffects.None

        For Each i As Integer In DIC_DragDrops.Keys
            With DIC_DragDrops(i)
                If .FROM_Module.Equals(DragDrop_FROM.Dispo_Module) And .FROM_Level = DragDrop_FROM.Level Then
                    Dim TO_TV As TreeView = .TO_Module.TV_With_FP.TV
                    If .TO_Module.TV_With_FP.TV.Equals(Active_TV) Then
                        Dim DroppedNode As TreeNode = TO_TV.GetNodeAt(TO_TV.PointToClient(Control.MousePosition))
                        Dim DroppedNode_Level As Integer = -1

                        If Not (DroppedNode Is Nothing) Then
                            DroppedNode_Level = Val(Mid(DroppedNode.Name, 1, 1))
                        End If

                        If .TO_Level = DroppedNode_Level Then
                            OUT = DragDropEffects.Move
                            Exit For
                        End If
                    End If
                End If
            End With
        Next

        Return OUT
    End Function

    Sub New(ByVal MyFPf As FP_Form, Optional ByVal MyDispo_ID As Long = 0)
        Dispo_ID = MyDispo_ID
        FPf = MyFPf
        DIC_Dispo_Modules = New Dictionary(Of Integer, FP_L_Dispo_Module)
        DIC_DragDrops = New Dictionary(Of Integer, FP_L_DISPO_DragDrop)
    End Sub

    Sub Dispose()
        If Not Disposed Then
            DIC_Dispo_Modules.Clear()
            DIC_DragDrops.Clear()

            Disposed = True
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Public Function ADD_DragDrop(ByVal MyDragDrop As FP_L_DISPO_DragDrop) As Integer
        Dim OUT As Integer = -1

        OUT = DIC_DragDrops.Count + 1
        DIC_DragDrops.Add(OUT, MyDragDrop)

        ADD_DragDrop = OUT
    End Function

    Public Function ADD_Module(ByVal MyModule As FP_L_Dispo_Module) As Integer
        Dim OUT As Integer = -1

        OUT = DIC_Dispo_Modules.Count + 1

        MyModule.Parent = Me
        DIC_Dispo_Modules.Add(OUT, MyModule)

        ADD_Module = OUT
    End Function

    Public Sub RAISEEVENT_NodeDragBegin(ByVal FP_L_Dispo_Node_FROM As Struct_FP_L_Dispo_Node, ByRef Cancel As Boolean)
        DragDrop_FROM = FP_L_Dispo_Node_FROM
        RaiseEvent NodeDragBegin(Me, FP_L_Dispo_Node_FROM, Cancel)
        If Not Cancel Then
            DragDrop_Handled = False
        End If
    End Sub

    Public Sub RAISEEVENT_NodeDrop(ByVal e As System.Windows.Forms.DragEventArgs, ByVal DispoDragDrop As FP_L_DISPO_DragDrop, ByVal DragDrop_TO As Struct_FP_L_Dispo_Node)
        If DragDrop_Handled = False Then
            'If (Not (DragDrop_FROM.Node Is Nothing) Or DragDrop_FROM.Level = -1) Then

            If DispoDragDrop.FROM_Module.Equals(DragDrop_FROM.Dispo_Module) And DispoDragDrop.FROM_Level = DragDrop_FROM.Level Then
                If (Not DragDrop_FROM.Dispo_Module.Equals(DragDrop_TO.Dispo_Module) Or DragDrop_FROM.Level <> DragDrop_TO.Level Or DragDrop_FROM.ID <> DragDrop_TO.ID) Then
                    RaiseEvent NodeDrop(Me, e, DragDrop_FROM, DragDrop_TO)
                End If
                DragDrop_Handled = True
                DragDrop_FROM = New Struct_FP_L_Dispo_Node 'Clear Variable
            End If
        End If
    End Sub
End Class
Public Class FP_L_Dispo_Module
    Public Structure Struct_SQL_Bind_Params
        Dim TreeView_With_FP_SQL_Bind_Params As FP_L_TreeView_With_FP.Struct_SQL_Bind_PARAMS
    End Structure

    Public Event Node_Delete(ByVal sender As FP_L_Dispo_Module, ByRef Handled As Boolean)
    Public Event Node_MoveUpDown(ByVal sender As FP_L_Dispo_Module, ByVal UpDown As ENUM_UpDown, ByRef Handled As Boolean)
    Public Event Node_MouseWheel(ByVal sender As FP_L_Dispo_Module, ByVal e As System.Windows.Forms.MouseEventArgs, ByRef Handled As Boolean)

    Public Parent As FP_L_DISPO = Nothing
    Public RS_Type As String = ""
    Public FPf As FP_Form = Nothing
    Public WithEvents GRID_FP As FP = Nothing
    Public WithEvents TV_With_FP As FP_L_TreeView_With_FP = Nothing

    Public WithEvents RS_Selected_Checkbox As FP_L_GRID_RS_Select_Checkbox = Nothing
    Public SQL_Bind_Params As Struct_SQL_Bind_Params

    Private Disposed As Boolean = False

    Sub New(ByVal MyP As Struct_FP_L_Dispo_Module)
        RS_Type = MyP.RS_Type
        FPf = MyP.FPf
        GRID_FP = MyP.GRID_FP
        If Not (MyP.TV Is Nothing) Then
            MyP.TV.CheckBoxes = (Not (MyP.GRID_FPc_Selected_Checkbox Is Nothing))
            SQL_Bind_Params = MyP.SQL_Bind_Params

            Dim P_TV_With_FP As New Struct_FP_L_TreeView_With_FP_CONTROLS
            With P_TV_With_FP
                .ServerObject_Prefix = GRID_FP.ServerObject_Prefix
                .FPf = FPf
                .TV = MyP.TV
                .MultiRoot = (Not MyP.No_MultiRoot)
                .SubWHERE = MyP.SubWHERE
                .SubWHERE_Do_Not_Insert_Terminal_In_WHERE = MyP.SubWHERE_Do_Not_Insert_Terminal_In_WHERE
                .SQL_Bind_Params = MyP.SQL_Bind_Params.TreeView_With_FP_SQL_Bind_Params
                TV_With_FP = New FP_L_TreeView_With_FP(P_TV_With_FP)
            End With
        End If

        If Not (MyP.GRID_FPc_Selected_Checkbox Is Nothing) Then
            RS_Selected_Checkbox = New FP_L_GRID_RS_Select_Checkbox(MyP.GRID_FPc_Selected_Checkbox, False)
        End If
    End Sub

    Sub Dispose()
        If Not (RS_Selected_Checkbox Is Nothing) Then
            RS_Selected_Checkbox.Dispose()
            RS_Selected_Checkbox = Nothing
        End If

        GRID_FP = Nothing
        Disposed = True
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Public Function TV_SELECTED_NODE_UPDOWN(ByVal UpDown As ENUM_UpDown) As Boolean
        Dim OUT As Boolean = False
        Dim AktNode As TreeNode = TV_With_FP.TV_SELECTED_NODE()

        If Not (AktNode Is Nothing) Then

            Dim AktNode_ID As Long = TV_With_FP.TV_SELECTED_NODE_GET_ID()
            Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()
            Dim Result As Boolean = False
            Dim DoFill As Boolean = False

            With FPf.FPApp.DC
                .Qdf_set_SP(sqlComm, "Dispo_Records_UpDown")
                .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                .Qdf_AddParameter(sqlComm, "@Dispo_ID", SqlDbType.Int, , , , , Parent.Dispo_ID)
                .Qdf_AddParameter(sqlComm, "@RS_Type", SqlDbType.NVarChar, , 127, RS_Type)
                .Qdf_AddParameter(sqlComm, "@Level", SqlDbType.Int, , , , , TV_With_FP.TV_SELECTED_NODE_GET_LEVEL)
                .Qdf_AddParameter(sqlComm, "@RecordID", SqlDbType.Int, , , , , TV_With_FP.TV_SELECTED_NODE_GET_ID)
                .Qdf_AddParameter(sqlComm, "@UpDown", SqlDbType.Int, , , , , UpDown)
                .Qdf_AddParameter(sqlComm, "@OUT_Handled", SqlDbType.Bit, ParameterDirection.Output)
            End With

            CURSOR_SHOW_WAIT()
            Try
                If FPf.Qdf_Execute(sqlComm) Then
                    DoFill = nz(sqlComm.Parameters("@OUT_Handled").Value, False)
                End If

            Catch ex As Exception
                FPf.FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
            End Try

            CURSOR_SHOW_DEFAULT()

            If DoFill Then
                TV_With_FP.TV_FILL_AT_THE_END(AktNode.Name, True)
            End If
        End If

        Return OUT
    End Function

    Public Function CurrentNode() As Struct_FP_L_Dispo_Node
        Dim OUT As New Struct_FP_L_Dispo_Node

        If Not (TV_With_FP.TV Is Nothing) Then
            If Not (TV_With_FP.TV.SelectedNode Is Nothing) Then
                With TV_With_FP
                    OUT.Node = .TV_SELECTED_NODE
                    OUT.ID = .TV_SELECTED_NODE_GET_ID
                    OUT.Level = .TV_SELECTED_NODE_GET_LEVEL
                    OUT.FP = .TV_SELECTED_NODE_GET_FP
                    OUT.Root_ID = .TV_SELECTED_NODE_GET_ROOT_ID
                End With
            End If
        End If

        CurrentNode = OUT
    End Function

    Private Sub RS_Select_Checkbox_Selection_Changed(ByVal sender As FP_L_GRID_RS_Select_Checkbox, ByVal DTofChangedIDs As System.Data.DataTable) Handles RS_Selected_Checkbox.Selection_Changed
        If Not (TV_With_FP Is Nothing) Then
            If Not TV_With_FP.TV Is Nothing Then
                GRID_FP.DATA_Binded = False
                If DTofChangedIDs.Rows.Count > 0 Then
                    Dim IndexOfFP As Integer = TV_With_FP.Array_Of_FP.GET_INDEX_OF_FP(GRID_FP)

                    If IndexOfFP >= 0 Then
                        For Each row As DataRow In DTofChangedIDs.Rows
                            Dim CurrentNodeKey As String = TV_With_FP.get_Node_Key_from_Elements(IndexOfFP, row!RecordID)
                            If TV_With_FP.DIC_NODES.ContainsKey(CurrentNodeKey) Then
                                If TV_With_FP.DIC_NODES(CurrentNodeKey).Checked <> row!Selected Then
                                    TV_With_FP.DIC_NODES(CurrentNodeKey).Checked = row!Selected
                                End If
                            End If
                        Next
                    End If
                End If
                GRID_FP.DATA_Binded = True
            End If
        End If
    End Sub

    Private Sub TV_With_FP_Node_CheckedChanged(ByVal sender As FP_L_TreeView_With_FP, ByVal e As System.Windows.Forms.TreeViewEventArgs, ByVal IndexOfFP As Integer, ByVal FP As FP, ByVal RecordID As Long) Handles TV_With_FP.Node_CheckedChanged
        If GRID_FP.P_DATA_Binded Then
            If Not (FP Is Nothing) Then
                If FP.Equals(GRID_FP) Then
                    Dim Criteria As String = String.Format("RecordID = {0}", RecordID)

                    If GRID_FP.GRID.DT.Select(Criteria).Count <> 1 Then
                        GRID_FP.GRID.P_FilterActive = False
                    End If

                    If GRID_FP.GRID.DT.Select(Criteria).Count <> 1 Then
                        FPf.FPApp.DoErrorMsgBox("FP_FieldLogics.TV_With_FP.Node_CheckedChanged", 0, "Row in GRID not found.")
                    Else
                        Dim Row As DataRow = GRID_FP.GRID.DT.Select(Criteria).First
                        Dim Row_ALL_FIELDS As DataRow = GRID_FP.GRID.DT_ALL_FIELDS.Select(Criteria).First

                        If Row.Item(RS_Selected_Checkbox.FPc_RS_Select_Checkbox.FieldName) <> e.Node.Checked Then
                            Row.Item(RS_Selected_Checkbox.FPc_RS_Select_Checkbox.FieldName) = e.Node.Checked
                        End If

                        If Row_ALL_FIELDS.Item(RS_Selected_Checkbox.FPc_RS_Select_Checkbox.FieldName) <> e.Node.Checked Then
                            Row_ALL_FIELDS.Item(RS_Selected_Checkbox.FPc_RS_Select_Checkbox.FieldName) = e.Node.Checked
                        End If

                        If Row!RecordID = GRID_FP.P_DATA_Current_ID Then
                            With RS_Selected_Checkbox
                                If .FPc_RS_Select_Checkbox.P_VALUE <> e.Node.Checked Then
                                    .FPc_RS_Select_Checkbox.P_VALUE = e.Node.Checked
                                End If
                            End With
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub GRID_FP_GRID_Filter_Changed(ByVal sender_FP As FP) Handles GRID_FP.GRID_Filter_Changed
        If Not (RS_Selected_Checkbox Is Nothing) Then
            For Each AktKey As String In TV_With_FP.DIC_NODES.Keys
                With TV_With_FP.DIC_NODES(AktKey)
                    Dim CurrentLevel As Integer = Val(Mid(.Name, 1, 1))
                    Dim CurrentID As Long = Val(Mid(.Name, 3, 20))

                    If Not (TV_With_FP.Array_Of_FP.DIC_FPs(CurrentLevel) Is Nothing) Then
                        If GRID_FP.Equals(TV_With_FP.Array_Of_FP.DIC_FPs(CurrentLevel).FP) Then
                            Dim Criteria As String = String.Format("RecordID = {0}", CurrentID)
                            If GRID_FP.GRID.DT.Select(Criteria).Count = 1 Then
                                Dim Row As DataRow = GRID_FP.GRID.DT.Select(Criteria).First

                                Row.BeginEdit()
                                Row.Item(RS_Selected_Checkbox.FPc_RS_Select_Checkbox.FieldName) = .Checked
                                Row.EndEdit()
                            End If
                        End If
                    End If
                End With
            Next
        End If
    End Sub

    Private Sub TV_With_FP_Node_KeyPress(ByVal sender As FP_L_TreeView_With_FP, ByVal e As System.Windows.Forms.KeyPressEventArgs, ByVal IndexOfFP As Integer, ByVal FP As FP, ByVal RecordID As Long) Handles TV_With_FP.Node_KeyPress
        If Not e.Handled Then
            If Not (RS_Selected_Checkbox Is Nothing) Then
                Select Case e.KeyChar
                    Case "+", "-", "*"
                        RS_Selected_Checkbox.EVENT_FPc_RS_Select_Checkbox_Field_KeyPreview_KeyPress(RS_Selected_Checkbox.FPc_RS_Select_Checkbox, RS_Selected_Checkbox.FPc_RS_Select_Checkbox.c, e)
                End Select
            End If
        End If
    End Sub

    Private Sub TV_With_FP_TV_DeleteNode(ByVal sender As FP_L_TreeView_With_FP, ByRef Handled As Boolean) Handles TV_With_FP.Node_Delete
        RaiseEvent Node_Delete(Me, Handled)
    End Sub

    Private Sub TV_With_FP_Node_MoveUpDown(ByVal sender As FP_L_TreeView_With_FP, ByVal UpDown As ENUM_UpDown, ByRef Handled As Boolean) Handles TV_With_FP.Node_MoveUpDown
        RaiseEvent Node_MoveUpDown(Me, UpDown, Handled)
    End Sub

    Private Sub TV_With_FP_Node_MouseWheel(ByVal sender As FP_L_TreeView_With_FP, ByVal e As System.Windows.Forms.MouseEventArgs, ByRef Handled As Boolean) Handles TV_With_FP.Node_MouseWheel
        RaiseEvent Node_MouseWheel(Me, e, Handled)
    End Sub
End Class

Public Class FP_L_FP_SubprefixChange
    Public Event Subprefix_Changed(Sender As FP_L_FP_SubprefixChange)

    Public Structure STRUCT_LayoutChange_Params
        Dim MyFP As FP
        Dim NewSubprefix As String
        Dim GotoCurrentRecord_AfterSubprefixChange As Boolean
        Dim Exec_Immediately As Boolean 'A valtozasokat timer hajtja vegre, vagyis nem a letrehozas pillanataban zajlik le a folyamat. Ha ez TRUE, akkor azonnal kivaltja a TICK esemenyt.
    End Structure

    Public MyFP As FP
    Public NewSubprefix As String
    Public GotoCurrentRecord_AfterSubprefixChange As Boolean = False

    Private WithEvents Timer_for_SubprefixChange As New System.Windows.Forms.Timer
    Private Disposed As Boolean = False

    Public Sub New(Params As STRUCT_LayoutChange_Params)
        With Params
            MyFP = .MyFP
            NewSubprefix = .NewSubprefix
            GotoCurrentRecord_AfterSubprefixChange = .GotoCurrentRecord_AfterSubprefixChange
        End With

        If Params.Exec_Immediately Then
            Dim ee As New EventArgs
            Timer_for_SubprefixChange_Tick(Timer_for_SubprefixChange, ee)
        Else
            Timer_for_SubprefixChange.Interval = 20
            Timer_for_SubprefixChange.Enabled = True
        End If
    End Sub

    Public Sub Dispose()
        If Disposed = False Then
            If Not (Timer_for_SubprefixChange Is Nothing) Then
                Timer_for_SubprefixChange.Enabled = False
                Timer_for_SubprefixChange = Nothing
            End If

            MyFP = Nothing

            Disposed = True
        End If
    End Sub

    Private Sub Timer_for_SubprefixChange_Tick(sender As Object, e As EventArgs) Handles Timer_for_SubprefixChange.Tick
        Timer_for_SubprefixChange.Enabled = False

        If Disposed = False Then
            Dim Current_c_Name As String = ""

            If Not (MyFP.FPf.ActiveControl Is Nothing) Then
                Current_c_Name = MyFP.FPf.ActiveControl.c.Name
            End If

            MyFP.SubPrefix = NewSubprefix

            MyFP.CONTROLS_REFRESH_FROM_RS(GotoCurrentRecord_AfterSubprefixChange)

            MyFP.DATA_Binded = False
            RaiseEvent Subprefix_Changed(Me)
            MyFP.DATA_Binded = True

            If Current_c_Name > "" Then
                If MyFP.FPf.CONTROLS.ContainsKey(Current_c_Name) Then
                    Dim Current_c As Control = MyFP.FPf.CONTROLS(Current_c_Name)
                    Dim Next_Control As Control = MyFP.FPf.CONTROLS_GET_NEXTCONTROL(Current_c)

                    If Not (Next_Control Is Nothing) Then
                        FOCUS_ON_IMMEDIATELY(Next_Control)
                    End If
                End If
            End If

            Dispose()
        End If
    End Sub

End Class

Public Class FP_L_FORM_GOTO_RECORDS
    'Ha egy FP_Form-on ra akarsz allni egy subrecord-ra, akkor tudod hasznalni ezt az osztalyt.
    'Ilyen esetben nem elegendo a fej rekordra raallni, hanem ra kell allni a gyerek FP-ken is a megfelelo rekordra (maskepp nem lesz beolvasva a rekord, amire ra szeretnel allni)

    Protected Structure STRUCT_FP_L_FORM_GOTO_RECORD_GOTO_PARAMS
        Dim FP As FP
        Dim GOTO_ID As Integer
    End Structure

    Protected Controlled_FPf As FP_Form = Nothing

    Protected GOTO_Records_Params(0) As STRUCT_FP_L_FORM_GOTO_RECORD_GOTO_PARAMS
    Protected Disposed As Boolean = False

    Public Sub New(MyControlled_FPf As FP_Form)
        Controlled_FPf = MyControlled_FPf
    End Sub

    Public Overridable Sub Dispose()
        If Disposed = False Then
            ReDim GOTO_Records_Params(0)
            Disposed = True
        End If
    End Sub

    Public Sub GOTO_RECORDS_ADD(MyFP As FP, MyGoto_ID As Long)
        If Disposed = False Then
            Dim SeqNum As Integer = UBound(GOTO_Records_Params) + 1

            ReDim Preserve GOTO_Records_Params(SeqNum)

            With GOTO_Records_Params(SeqNum)
                .FP = MyFP
                .GOTO_ID = MyGoto_ID
            End With
        End If
    End Sub

    Public Sub GOTO_RECORDS_ADD(GOTO_FP_ALIASES As String, GOTO_FP_IDs As String)
        Dim Array_of_FP_ALIASES() As String = Split(GOTO_FP_ALIASES, "|")
        Dim Array_of_FP_IDs() As String = Split(GOTO_FP_IDs, "|")

        Dim NewUbound As Integer = Math.Min(UBound(Array_of_FP_ALIASES), UBound(Array_of_FP_IDs))

        ReDim Preserve Array_of_FP_ALIASES(NewUbound)
        ReDim Preserve Array_of_FP_IDs(NewUbound)

        For i As Integer = 0 To NewUbound
            If Array_of_FP_ALIASES(i) = "" Then
                Exit For
            End If

            Dim CurrentFP As FP = Controlled_FPf.GET_FP_BY_ALIAS(Array_of_FP_ALIASES(i))

            If CurrentFP Is Nothing Then
                Exit For
            End If

            GOTO_RECORDS_ADD(CurrentFP, Array_of_FP_IDs(i))

            If Array_of_FP_IDs(i) = 0 Then
                Exit For
            End If
        Next
    End Sub

    Public Function GOTO_RECORDS() As Boolean
        Dim OUT As Boolean = True

        If Disposed Then
            OUT = False
        Else
            For i As Integer = 1 To UBound(GOTO_Records_Params)
                With GOTO_Records_Params(i)
                    If Not (GOTO_Records_Params(i).FP Is Nothing) Then
                        OUT = .FP.FORM_GOTO_RECORD_BY_ID(.GOTO_ID)

                        If OUT = False Then
                            Exit For
                        End If
                    End If
                End With
            Next
        End If

        Return OUT
    End Function
End Class

Public Class FP_L_FORM_GOTO_RECORD_ON_CURRENT_AFTER_CHILDREN
    Inherits FP_L_FORM_GOTO_RECORDS

    Public WithEvents ControlledFP As FP = Nothing

    Sub New(ByVal MyControlledFP As FP)
        MyBase.New(MyControlledFP.FPf)
        ControlledFP = MyControlledFP
    End Sub

    Public Overrides Sub Dispose()
        If Disposed = False Then
            ControlledFP = Nothing

            MyBase.Dispose()
        End If
    End Sub

    Private Sub ControlledFP_Form_Current_AfterChildren(ByVal sender_FP As FP) Handles ControlledFP.Form_Current_AfterChildren
        If ControlledFP.P_DATA_Current_ID <> 0 Then
            GOTO_RECORDS()

            Dispose()
        End If
    End Sub
End Class

Public Class FP_L_FORM_GOTO_RECORD_ON_OPEN
    Public Event RECORD_LOADED(ByVal sender As FP_L_FORM_GOTO_RECORD_ON_OPEN)
    Public Event RECORD_LOADED_FAILED(ByVal sender As FP_L_FORM_GOTO_RECORD_ON_OPEN)
    Public Event RECORD_FORM_AFTER_UPDATE(ByVal sender As FP_L_FORM_GOTO_RECORD_ON_OPEN)

    Public FPApp As FP_App = Nothing
    Public WithEvents ControlledFP As FP = Nothing
    Public GotoID As Integer
    Public ShowAllRecord As Boolean = False
    Private InProgress As Boolean = False
    Private Disposed As Boolean = False
    Private Record_Goto_done As Boolean = False

    Sub New(ByVal MyFP As FP, ByVal MyGotoID As Long, Optional MyShowAllRecord As Boolean = False)
        If (MyFP Is Nothing) Then
            gl_FPApp.DoErrorMsgBox("FP_FieldLogics..FP_L_FORM_GOTO_RECORD_ON_OPEN.New", 0, "A 'MyFP' parameter erteke NULL. Ahhoz, hogy ez az osztaly mukodjon, a Form-nak mar a New esemenyben letre kell hoznia az FP es az FPf parametereket!")
        Else
            FPApp = MyFP.FPf.FPApp

            If MyFP.P_FORM_AllowEdits Then
                ControlledFP = MyFP
            End If
        End If

        GotoID = MyGotoID
        ShowAllRecord = MyShowAllRecord
    End Sub

    Public Sub Dispose()
        ControlledFP = Nothing
        Disposed = True
    End Sub

    Private Sub ControlledFP_Form_AfterUpdate(ByVal sender_FP As FP) Handles ControlledFP.Form_AfterUpdate
        If GotoID = 0 Then
            GotoID = ControlledFP.P_DATA_Current_ID
            RaiseEvent RECORD_FORM_AFTER_UPDATE(Me)
        Else
            If ControlledFP.P_DATA_Current_ID = GotoID Then
                RaiseEvent RECORD_FORM_AFTER_UPDATE(Me)
            End If
        End If
    End Sub

    Private Sub ControlledFP_Form_Records_Loading(ByVal sender_FP As FP, ByVal SubWHERE As String, ByVal NoRecord_OK As Boolean, ByRef Result As Boolean, ByRef Handled As Boolean) Handles ControlledFP.Form_Records_Loading
        If Not Disposed Then
            If Record_Goto_done = False Then
                If InProgress = False Then
                    InProgress = True
                    If GotoID = 0 Then
                        If ControlledFP.P_FORM_AllowAdditions Then
                            Result = ControlledFP.FORM_RECORDS_LOAD(SubWHERE, True, NoRecord_OK, True)
                        End If
                    Else
                        If ShowAllRecord Then
                            Result = ControlledFP.FORM_RECORDS_LOAD(, , NoRecord_OK, True)
                            If Result Then
                                Result = ControlledFP.FORM_GOTO_RECORD_BY_ID(GotoID)
                            End If
                        Else
                            SubWHERE = TEXT_AND(SubWHERE, String.Format("ID={0}", GotoID))
                            Result = ControlledFP.FORM_RECORDS_LOAD(SubWHERE, , NoRecord_OK, True)
                        End If
                    End If

                    If Result Then
                        RaiseEvent RECORD_LOADED(Me)
                    Else
                        RaiseEvent RECORD_LOADED_FAILED(Me)
                    End If

                    Record_Goto_done = True
                    Handled = True
                    InProgress = False
                End If
            End If
        End If
    End Sub
End Class
Public Class FP_L_FORM_ON_OPEN_SET_FILTER
    Public Event RECORD_LOADED(ByVal sender As FP_L_FORM_ON_OPEN_SET_FILTER)
    Public Event RECORD_LOADED_FAILED(ByVal sender As FP_L_FORM_ON_OPEN_SET_FILTER)
    Public Event RECORD_FORM_AFTER_UPDATE(ByVal sender As FP_L_FORM_ON_OPEN_SET_FILTER)

    Public FPApp As FP_App = Nothing
    Public WithEvents ControlledFP As FP = Nothing
    Public SQL_WHERE As String = ""
    Private InProgress As Boolean = False
    Private Disposed As Boolean = False
    Private Record_Goto_done As Boolean = False

    Sub New(ByVal MyFPApp As FP_App, ByVal MyFP As FP, ByVal MySQL_WHERE As String)
        FPApp = MyFPApp
        If (MyFP Is Nothing) Then
            FPApp.DoErrorMsgBox("FP_FieldLogics..FP_L_FORM_SET_FILTER_ON_OPEN.New", 0, "A 'MyFP' parameter erteke NULL. Ahhoz, hogy ez az osztaly mukodjon, a Form-nak mar a New esemenyben letre kell hoznia az FP es az FPf parametereket!")
        Else
            If MyFP.P_FORM_AllowEdits Then
                ControlledFP = MyFP
            End If
        End If

        SQL_WHERE = MySQL_WHERE
    End Sub

    Public Sub Dispose()
        ControlledFP = Nothing
        Disposed = True
    End Sub

    Private Sub ControlledFP_Form_AfterUpdate(ByVal sender_FP As FP) Handles ControlledFP.Form_AfterUpdate
        RaiseEvent RECORD_FORM_AFTER_UPDATE(Me)
    End Sub

    Private Sub ControlledFP_Form_Records_Loading(ByVal sender_FP As FP, ByVal SubWHERE As String, ByVal NoRecord_OK As Boolean, ByRef Result As Boolean, ByRef Handled As Boolean) Handles ControlledFP.Form_Records_Loading
        If Not Disposed Then
            If Record_Goto_done = False Then
                If InProgress = False Then
                    InProgress = True
                    SubWHERE = TEXT_AND(SubWHERE, SQL_WHERE)
                    Result = ControlledFP.FORM_RECORDS_LOAD(SubWHERE, , NoRecord_OK, True)

                    If Result Then
                        RaiseEvent RECORD_LOADED(Me)
                    Else
                        RaiseEvent RECORD_LOADED_FAILED(Me)
                    End If

                    Record_Goto_done = True
                    Handled = True
                    InProgress = False
                End If
            End If
        End If
    End Sub
End Class
Public Class FP_L_PictureComboBox
    Public Event PictureComboBox_GRID_CELLPAINTING(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs)

    Public FPc As FP_Control
    Public ImageList_Of_Pictures As ImageList
    Public WithEvents GRID As DataGridView
    Private WithEvents c_Combobox As ComboBox
    Private Disposed As Boolean = False

    Sub New(ByVal MyFPc As FP_Control, ByVal MyImageList_Of_Pictures As ImageList)
        If Not TypeOf (MyFPc.c) Is ComboBox Then
            FPc.FPf.FPApp.DoErrorMsgBox("FP_L_PictureComboBox.New", 0, "FPc must be a combobox.")
        Else
            FPc = MyFPc
            c_Combobox = FPc.c_ComboBox
            ImageList_Of_Pictures = MyImageList_Of_Pictures

            If Not (FPc.DT Is Nothing) Then
                'Dim items(FPc.DT.Rows.Count - 1) As String
                'For i As Int32 = 0 To FPc.DT.Rows.Count - 1
                '    items(i) = FPc.DT.Rows(i).Item(FPc.c_ComboBox.ValueMember)
                'Next

                With FPc.c_ComboBox
                    .DropDownStyle = ComboBoxStyle.DropDownList
                    .DrawMode = DrawMode.OwnerDrawVariable
                End With
            End If

            If FPc.FP.GRID_EXISTS Then
                GRID = FPc.FP.GRID.GRID
            End If
        End If
    End Sub

    Public Sub Dispose()
        If Not Disposed Then
            GRID = Nothing
            c_Combobox = Nothing
            FPc = Nothing
            ImageList_Of_Pictures = Nothing

            Disposed = True
        End If
    End Sub

    Private Sub c_ComboBox_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles c_Combobox.DrawItem
        If c_Combobox.DrawMode = DrawMode.OwnerDrawVariable Then
            If (ImageList_Of_Pictures Is Nothing) Then
                Dim textBrush = New SolidBrush(e.ForeColor)
                Dim Bounds As Rectangle = e.Bounds
                Dim ItemText As String = c_Combobox.Items(e.Index).Row.Item(c_Combobox.DisplayMember)
                Dim ItemFormat As New StringFormat

                With ItemFormat
                    .LineAlignment = StringAlignment.Near
                    .Alignment = StringAlignment.Near
                End With

                e.DrawBackground()
                e.DrawFocusRectangle()
                'e.Graphics.DrawString(ComboBoxItem.ToString(), c_ComboBox.Font, textBrush, Bounds, ItemFormat)
                e.Graphics.DrawString(ItemText, c_Combobox.Font, textBrush, Bounds, ItemFormat)
            Else
                If e.Index <> -1 Then
                    Dim FieldBounds As New Rectangle(e.Bounds.Location, e.Bounds.Size)

                    Dim CurrBitmap As Bitmap = ImageList_Of_Pictures.Images(e.Index)
                    Dim Picture_X As Integer = FieldBounds.Left + (FieldBounds.Width - CurrBitmap.Size.Width) / 2
                    Dim Picture_Y As Integer = FieldBounds.Top

                    e.Graphics.DrawImage(CurrBitmap, Picture_X, Picture_Y)


                    'e.Graphics.DrawImage(ImageList_Of_Pictures.Images(e.Index), e.Bounds.Left, e.Bounds.Top)
                End If
            End If
        End If
    End Sub

    Private Sub PAINT(ByVal BitmapKey As String, ByVal Gr As System.Drawing.Graphics, ByVal FieldBounds As Rectangle, ByVal Field_BackColor As Color)
        Dim backColorBrush As New SolidBrush(Field_BackColor)

        Try
            Gr.FillRectangle(backColorBrush, FieldBounds)

            If BitmapKey > "" Then
                If ImageList_Of_Pictures.Images.ContainsKey(BitmapKey) Then
                    Dim CurrBitmap As Bitmap = ImageList_Of_Pictures.Images(BitmapKey)

                    If Not (CurrBitmap Is Nothing) Then
                        'If CurrBitmap.Height <> FieldBounds.Height Then
                        '    CurrBitmap = New Bitmap(CurrBitmap, New Size((FieldBounds.Height / CurrBitmap.Height) * CurrBitmap.Width, FieldBounds.Height))

                        '    ImageList_Of_Pictures.Images.RemoveByKey(BitmapKey)
                        '    ImageList_Of_Pictures.Images.Add(BitmapKey, CurrBitmap)
                        'End If

                        Dim Picture_X As Integer = FieldBounds.Left + (FieldBounds.Width - CurrBitmap.Size.Width) / 2
                        Dim Picture_Y As Integer = FieldBounds.Top

                        Gr.DrawImage(CurrBitmap, Picture_X, Picture_Y)
                    End If
                End If
            End If

        Finally
            backColorBrush.Dispose()
        End Try
    End Sub

    Private Sub GRID_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles GRID.CellPainting
        If Not Disposed Then
            If Not (FPc Is Nothing) Then
                If Not (FPc.c Is Nothing) Then
                    If Not GRID.Columns.Contains(FPc.FieldName) Then
                        FPc.FP.FPf.FPApp.DoErrorMsgBox("FP_L_PictureComboBox.GRID_CellPainting", 0, String.Format("A GRID nem tartalmaz '{0}' nevu oszlopot.", FPc.FieldName))
                    Else
                        Dim Col_Attached_Index = GRID.Columns(FPc.FieldName).Index
                        Dim BitmapKey As String = ""

                        If Col_Attached_Index = e.ColumnIndex And e.RowIndex >= 0 Then

                            If FPc.P.xType_VB = "BIT" Then
                                If Not IsDBNull(GRID(Col_Attached_Index, e.RowIndex).Value) Then
                                    If GRID(Col_Attached_Index, e.RowIndex).Value Then
                                        BitmapKey = "1"
                                    Else
                                        BitmapKey = "0"
                                    End If
                                End If
                            Else
                                If Not IsDBNull(GRID(Col_Attached_Index, e.RowIndex).Value) Then
                                    BitmapKey = GRID(Col_Attached_Index, e.RowIndex).Value
                                End If
                            End If

                            Dim FieldBounds As New Rectangle(e.CellBounds.Location, e.CellBounds.Size)

                            PAINT(BitmapKey, e.Graphics, FieldBounds, e.CellStyle.BackColor)

                            Dim gridBrush As New SolidBrush(GRID.GridColor)
                            Dim gridLinePen As New Pen(gridBrush)

                            e.Graphics.DrawLine(gridLinePen, FieldBounds.Left, FieldBounds.Bottom - 1, FieldBounds.Right - 1, FieldBounds.Bottom - 1)
                            e.Graphics.DrawLine(gridLinePen, FieldBounds.Right - 1, FieldBounds.Top, FieldBounds.Right - 1, FieldBounds.Bottom)
                            gridLinePen.Dispose()
                            gridBrush.Dispose()

                            e.Handled = True
                        End If
                    End If
                End If
            End If
        End If


        'If Not Disposed Then
        '    If Not (FPc Is Nothing) Then
        '        If Not (FPc.c Is Nothing) Then
        '            If GRID.Columns.Contains(FPc.FieldName) Then
        '                Dim MyCellIndex As Integer = GRID.Columns(FPc.FieldName).Index

        '                If MyCellIndex = e.ColumnIndex And e.RowIndex >= 0 Then
        '                    RaiseEvent PictureComboBox_GRID_CELLPAINTING(sender, e)

        '                    If Not e.Handled Then
        '                        Dim MyPictureKey As String = nz(GRID(MyCellIndex, e.RowIndex).Value, "")

        '                        Dim backColorBrush As New SolidBrush(e.CellStyle.BackColor)
        '                        Dim gridBrush As New SolidBrush(GRID.GridColor)
        '                        Dim gridLinePen As New Pen(gridBrush)

        '                        Try
        '                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds)

        '                            If ImageList_Of_Pictures.Images.ContainsKey(MyPictureKey) Then
        '                                e.Graphics.DrawImage(ImageList_Of_Pictures.Images(MyPictureKey), e.CellBounds.Left + 3, e.CellBounds.Top + 3)
        '                            End If
        '                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
        '                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom)

        '                            e.Handled = True

        '                        Finally
        '                            backColorBrush.Dispose()
        '                            gridBrush.Dispose()
        '                            gridLinePen.Dispose()
        '                        End Try
        '                    End If
        '                End If
        '            End If
        '        End If
        '    End If
        'End If
    End Sub
End Class
Public Class FP_L_PictureList
    Public Event PictureList_MouseHover(ByVal sender As FP_L_PictureList, ByVal PictureName As String)
    Public Event PictureList_MouseLeave(ByVal sender As FP_L_PictureList, ByVal e As System.EventArgs)
    Public Event PictureList_Picture_Clicked(ByVal sender As FP_L_PictureList, ByVal PictureName As String)

    Public FPApp As FP_App = Nothing
    Public FPf As FP_Form = Nothing

    Public Last_SELECTED_Picture As String

    Public PictureDistance As Integer = 5

    Public WithEvents Panel_Main As Panel

    Private Const Marker_Width As Integer = 3
    Private sLoc As Integer = 0
    Private ToolTip As New ToolTip
    Private WithEvents SelectMarker As PictureBox

    Public Sub New(ByVal MyFPf As FP_Form, ByVal MyPanel_Main As Panel)
        FPf = MyFPf
        FPApp = FPf.FPApp
        Panel_Main = MyPanel_Main

        PICTURES_REMOVE_ALL()

        Panel_Main.AutoScroll = True
    End Sub

    Public Sub PICTURES_ADD(ByVal Picture_RES_Name As String, ByVal Add_Picture_As_Name As String, ByVal MySize As Size, ByVal TipText As String)
        If Panel_Main.Controls.ContainsKey(Add_Picture_As_Name) Then
            FPf.DoErrorMsgBox("FP_L_PictureList.PICTURES_ADD", 0, String.Format("Picture '{0}' is already added.", Add_Picture_As_Name))
        Else
            Dim p As New PictureBox
            With p
                .Name = Add_Picture_As_Name
                .Visible = True
                .Parent = Panel_Main
                .Size = MySize
                .Location = New Point(sLoc, (Panel_Main.Height - .Size.Height) / 2)
                If TipText > "" Then
                    ToolTip.SetToolTip(p, TipText)
                End If
                Panel_Main.Controls.Add(p)

                AddHandler .MouseHover, AddressOf MainPanel_MouseHover
                AddHandler .MouseLeave, AddressOf MainPanel_MouseLeave
                AddHandler .Click, AddressOf p_Click
            End With

            sLoc += p.Width + PictureDistance
            FPApp.BACKGROUND_SET(p, Picture_RES_Name)
            p = Nothing
        End If
    End Sub

    Public Sub PICTURES_ADD(ByVal Pict As Bitmap, ByVal Add_Picture_As_Name As String, ByVal MySize As Size, ByVal TipText As String)
        If Panel_Main.Controls.ContainsKey(Add_Picture_As_Name) Then
            FPf.DoErrorMsgBox("FP_L_PictureList.PICTURES_ADD", 0, String.Format("Picture '{0}' is already added.", Add_Picture_As_Name))
        Else
            Dim p As New PictureBox
            With p
                .Name = Add_Picture_As_Name
                .Visible = True
                .Parent = Panel_Main
                .Size = MySize
                .Location = New Point(sLoc, (Panel_Main.Height - .Size.Height) / 2)
                If TipText > "" Then
                    ToolTip.SetToolTip(p, TipText)
                End If
                Panel_Main.Controls.Add(p)

                AddHandler .MouseHover, AddressOf MainPanel_MouseHover
                AddHandler .MouseLeave, AddressOf MainPanel_MouseLeave
                AddHandler .Click, AddressOf p_Click
            End With

            sLoc += p.Width + PictureDistance
            FPApp.BACKGROUND_SET(p, Pict)
            p = Nothing
        End If
    End Sub

    Private Sub SelectMarker_CREATE()
        Dim SelectMarker_Name As String = "###SelectMarker###"

        If Panel_Main.Controls.ContainsKey(SelectMarker_Name) = False Then
            SelectMarker = New PictureBox
            With SelectMarker
                .Name = SelectMarker_Name
                .Visible = False
                .Parent = Panel_Main
                .BackColor = Color.Red
                .Size = New Size(64, 64)
                .Location = New Point(0, 0)

                Panel_Main.Controls.Add(SelectMarker)
            End With
        End If
    End Sub

    Public Sub PICTURES_REMOVE_ALL()
        Panel_Main.Controls.Clear()
        SelectMarker_CREATE()
        sLoc = 2 * Marker_Width
        Last_SELECTED_Picture = ""
        Panel_Main.AutoScrollPosition = New Point(0, 0)
    End Sub

    Private Sub MainPanel_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel_Main.MouseHover
        Dim pt As New Point(Cursor.Position.X, Cursor.Position.Y)
        Dim c As Control = Panel_Main.GetChildAtPoint(Panel_Main.PointToClient(pt))

        If c IsNot Nothing And TypeOf c Is PictureBox Then
            RaiseEvent PictureList_MouseHover(Me, c.Name)
        End If
    End Sub

    Private Sub MainPanel_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel_Main.MouseLeave
        If Cursor.Position.Y < Panel_Main.Location.Y Then
            RaiseEvent PictureList_MouseLeave(Me, e)
        End If
    End Sub

    Private Sub SELECT_ME(ByVal p As PictureBox)
        With SelectMarker
            .Size = New Size(p.Width + 2 * Marker_Width, p.Height + 2 * Marker_Width)
            .Location = New Point(p.Left - Marker_Width, p.Top - Marker_Width)
            .Visible = True
        End With
        Last_SELECTED_Picture = p.Name
        p.BringToFront()
    End Sub

    Private Sub p_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        SELECT_ME(sender)
        RaiseEvent PictureList_Picture_Clicked(Me, CType(sender, PictureBox).Name)
    End Sub
End Class
Public Class FP_L_PictureTextbox
    'Ez az osztaly egy Textbox-ot alakit at ugy, hogy a benne levo ertekekhez rendelt kepeket jeleniti meg.
    'Alkalmazhato peldaul ugy, hogy a kepernyon megjeleno rekord valamely erteke (pl statusza) nem ertek szerint kiirva, hanem
    'egy-egy keppel jelolve (peldaul piros negyzet = hibas, zold negyzet = jo adat) jelenjen meg.
    'A mezo GRID-ben is megjelenhet, ott is mukodik.
    '
    'Hasznalat:
    '----------
    '
    'Hozz letre egy normal Textbox-ot es regisztraljad az FP-ben.
    'Probakeppen ellenorizzed, hogy a textbox-ban megjelennek-e betukkel a kivant ertekek.
    '
    'Az FP CONTROLS_INITIALISED esemenyeben leptesd elo a Textbox-ot FP_L_PictureTextbox-á a kovetkezo módon:
    '
    '   Dim MyPictureTextbox as FP_L_PictureTextbox
    '
    '   (...)
    '
    '   
    'Private Sub CONTROLS_INITIALIZED(ByVal sender_FP As SEL_FP.FP) Handles MyFP.CONTROLS_INITIALIZED
    '    (...)
    '
    '    MyPictureTextbox = New FP_L_PictureTextbox(FPc_MyTextbox)
    '    With MyPictureTextbox
    '        .BITMAPS_ADD("PIROS", "MyDLL..PIROS.png")
    '        .BITMAPS_ADD("SARGA", "MyDLL..SARGA.png")
    '        .BITMAPS_ADD("ZOLD", "MyDLL..ZOLD.png")
    '    End With
    'End Sub
    '
    '(...)
    '
    'A fenti kod ertelem szeru atirasa utan ha a TextBox erteke "PIROS", akkor a PIROS.png kep fog benne megjelenni.



    Private Class FP_L_TextBox_with_PAINT_EVENT
        Inherits TextBox

        Public Event PictureTextBox_PAINT(ByVal sender As Object, ByVal e As PaintEventArgs)

        Sub New()
            MyBase.New()

            Me.SetStyle(ControlStyles.UserPaint, True)
        End Sub

        Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
            MyBase.OnPaint(e)

            RaiseEvent PictureTextBox_PAINT(Me, e)
        End Sub
    End Class

    Public DIC_Bitmaps As New Dictionary(Of String, Bitmap)
    Public FPc As FP_Control
    Private WithEvents c As FP_L_TextBox_with_PAINT_EVENT
    Public WithEvents GRID As DataGridView = Nothing
    Private Disposed As Boolean = False

    Sub New(ByVal MyFPc As FP_Control)
        FPc = MyFPc
        c = New FP_L_TextBox_with_PAINT_EVENT

        Dim Old_c As TextBox = FPc.c
        Dim c_Name As String = Old_c.Name

        FPc.P.Locked = True

        FPc.c.Name = "###_OLD_###"
        With Old_c
            c.Name = c_Name
            c.BorderStyle = BorderStyle.FixedSingle
            If Not FPc.P.ShowInGRID Then
                c.Location = .Location
                c.Size = .Size
            End If
            c.Font = .Font
            c.TabStop = .TabStop
            c.TabIndex = .TabIndex
            c.Parent = .Parent
            c.ContextMenu = .ContextMenu
            c.ContextMenuStrip = .ContextMenuStrip
            c.Multiline = .Multiline
            c.Text = .Text
            c.AllowDrop = .AllowDrop
        End With
        c.Refresh()

        FPc.c = c
        FPc.FPf.CONTROLS(c.Name) = c
        Old_c.Dispose()
        Old_c = Nothing

        If FPc.FP.GRID_EXISTS Then
            GRID = FPc.FP.GRID.GRID
        End If
    End Sub

    Public Sub Dispose()
        If Not Disposed Then
            c = Nothing
            FPc = Nothing
            GRID = Nothing

            Disposed = True
        End If
    End Sub

    Public Sub BITMAPS_ADD(ByVal Key As String, ByVal AsmAndObjectName As String)
        Dim asm As Reflection.Assembly = Nothing
        Dim ResourceName As String = ""

        If AsmAndObjectName = "" Then
            DIC_Bitmaps.Add(Key, Nothing)
        Else
            If FPc.FP.FPf.FPApp.SKIN_getASM_And_OBJECTNAME(AsmAndObjectName, asm, ResourceName) Then
                Try
                    Dim im As Bitmap

                    im = New Bitmap(asm.GetManifestResourceStream(ResourceName))

                    BITMAPS_ADD(Key, im)

                Catch ex As Exception
                    FPc.FP.FPf.FPApp.DoErrorMsgBox("FP_L_GRID_PictureCell.BITMAPS_ADD", 0, String.Format("Image not found '{0}'", ResourceName))
                End Try
            End If
        End If
    End Sub

    Public Sub BITMAPS_ADD(ByVal Key As String, ByVal NewBitmap As Bitmap)
        If DIC_Bitmaps.Keys.Contains(Key) Then
            DIC_Bitmaps(Key) = NewBitmap
        Else
            DIC_Bitmaps.Add(Key, NewBitmap)
        End If
    End Sub

    Public Sub BITMAPS_ADD(From_ImageList As FP_L_ImageList)
        DIC_Bitmaps = From_ImageList.GET_ALL_PICTURES_IN_DIC()
    End Sub

    Private Sub PAINT(ByVal BitmapKey As String, ByVal Gr As System.Drawing.Graphics, ByVal FieldBounds As Rectangle, ByVal Field_BackColor As Color)
        Dim backColorBrush As New SolidBrush(Field_BackColor)

        Try
            Gr.FillRectangle(backColorBrush, FieldBounds)

            If BitmapKey > "" Then
                If DIC_Bitmaps.Keys.Contains(BitmapKey) Then
                    Dim CurrBitmap As Bitmap = DIC_Bitmaps(BitmapKey)

                    If Not (CurrBitmap Is Nothing) Then
                        If CurrBitmap.Height <> FieldBounds.Height Then
                            CurrBitmap = New Bitmap(CurrBitmap, New Size((FieldBounds.Height / CurrBitmap.Height) * CurrBitmap.Width, FieldBounds.Height))

                            DIC_Bitmaps(BitmapKey) = CurrBitmap
                        End If

                        Dim Picture_X As Integer = FieldBounds.Left + (FieldBounds.Width - CurrBitmap.Size.Width) / 2
                        Dim Picture_Y As Integer = FieldBounds.Top

                        Gr.DrawImage(CurrBitmap, Picture_X, Picture_Y)
                    End If
                End If
            End If

        Finally
            backColorBrush.Dispose()
        End Try
    End Sub

    Private Sub GRID_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles GRID.CellPainting
        If Not Disposed Then
            If Not (FPc Is Nothing) Then
                If Not (FPc.c Is Nothing) Then
                    If Not GRID.Columns.Contains(FPc.FieldName) Then
                        FPc.FP.FPf.FPApp.DoErrorMsgBox("FP_L_GRID_PictureCell.GRID_CellPainting", 0, String.Format("A GRID nem tartalmaz '{0}' nevu oszlopot.", FPc.FieldName))
                    Else
                        Dim Col_Attached_Index = GRID.Columns(FPc.FieldName).Index
                        Dim BitmapKey As String = ""

                        If Col_Attached_Index = e.ColumnIndex And e.RowIndex >= 0 Then

                            If FPc.P.xType_VB = "BIT" Then
                                If Not IsDBNull(GRID(Col_Attached_Index, e.RowIndex).Value) Then
                                    If GRID(Col_Attached_Index, e.RowIndex).Value Then
                                        BitmapKey = "1"
                                    Else
                                        BitmapKey = "0"
                                    End If
                                End If
                            Else
                                If Not IsDBNull(GRID(Col_Attached_Index, e.RowIndex).Value) Then
                                    BitmapKey = GRID(Col_Attached_Index, e.RowIndex).Value
                                End If
                            End If

                            Dim FieldBounds As New Rectangle(e.CellBounds.Location, e.CellBounds.Size)

                            PAINT(BitmapKey, e.Graphics, FieldBounds, e.CellStyle.BackColor)

                            Dim gridBrush As New SolidBrush(GRID.GridColor)
                            Dim gridLinePen As New Pen(gridBrush)

                            e.Graphics.DrawLine(gridLinePen, FieldBounds.Left, FieldBounds.Bottom - 1, FieldBounds.Right - 1, FieldBounds.Bottom - 1)
                            e.Graphics.DrawLine(gridLinePen, FieldBounds.Right - 1, FieldBounds.Top, FieldBounds.Right - 1, FieldBounds.Bottom)
                            gridLinePen.Dispose()
                            gridBrush.Dispose()

                            e.Handled = True
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub c_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles c.MouseDown
        c.Refresh()
    End Sub

    Private Sub c_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles c.MouseMove
        If e.Button = MouseButtons.Left Then
            c.Refresh()
        End If
    End Sub

    Private Sub c_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles c.PictureTextBox_PAINT
        If Disposed Then
            Exit Sub
        End If

        If FPc Is Nothing Then
            Exit Sub
        End If

        If FPc.c Is Nothing Then
            Exit Sub
        End If

        Dim BitmapKey As String = ""

        If FPc.FP.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Or FPc.FP.UnboundForm Then
            If FPc.FP.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Or FPc.FP.UnboundForm Then
                Select Case FPc.P.xType_VB
                    Case "BIT"
                        If FPc.P_VALUE = True Then
                            BitmapKey = "1"
                        Else
                            BitmapKey = "0"
                        End If
                    Case Else
                        BitmapKey = FPc.P_VALUE
                End Select
            End If
        End If

        Dim MyBounds As New Rectangle(0, 0, c.Width, c.Height)

        PAINT(BitmapKey, e.Graphics, MyBounds, FPc.c.BackColor)

        If FPc.P.ShowInGRID Then
            If Not (GRID Is Nothing) Then
                Dim gridBrush As New SolidBrush(GRID.GridColor)
                Dim gridLinePen As New Pen(gridBrush)

                e.Graphics.DrawLine(gridLinePen, MyBounds.Left, MyBounds.Bottom - 1, MyBounds.Right - 1, MyBounds.Bottom - 1)
                e.Graphics.DrawLine(gridLinePen, MyBounds.Right - 1, MyBounds.Top, MyBounds.Right - 1, MyBounds.Bottom)
                gridLinePen.Dispose()
                gridBrush.Dispose()
            End If
        End If
    End Sub
End Class
Public Class FP_L_ProgressForm
    Public ProgressForm As FP_ProgressForm
    Private FPApp As FP_App
    Private FP_ParentForm As FP_Form
    Private CancelPressed As Boolean = False
    Private Disposed As Boolean = False

    Public ReadOnly Property P_CancelPressed() As Boolean
        Get
            DOEVENTS()
            P_CancelPressed = CancelPressed
        End Get
    End Property

    Sub New(ByVal MyFP_ParentForm As FP_Form, ByVal MySubPrefix As String, ByVal MyMinValue As Integer, ByVal MyMaxValue As Integer)
        FP_ParentForm = MyFP_ParentForm
        FPApp = FP_ParentForm.FPApp
        CancelPressed = False

        ProgressForm = New FP_ProgressForm(FPApp, MySubPrefix, MyMinValue, MyMaxValue)

        FP_ParentForm.P_ENABLED = False
        FP_ParentForm.P_Enabled_Form_Child = ProgressForm

        ProgressForm.Show()
        ProgressForm.Activate()
        ProgressForm.BringToFront()
        ProgressForm.Refresh()

        Application.DoEvents()
    End Sub

    Sub New(ByVal MyFPApp As FP_App, ByVal MySubPrefix As String, ByVal MyMinValue As Integer, ByVal MyMaxValue As Integer)
        FP_ParentForm = Nothing
        FPApp = MyFPApp
        CancelPressed = False

        ProgressForm = New FP_ProgressForm(FPApp, MySubPrefix, MyMinValue, MyMaxValue)

        ProgressForm.Show()
        ProgressForm.Activate()
        ProgressForm.BringToFront()
        ProgressForm.Refresh()

        Application.DoEvents()
    End Sub

    Public Sub DOEVENTS()
        Application.DoEvents()
        If Not Disposed Then
            If Not (ProgressForm Is Nothing) Then
                CancelPressed = ProgressForm.CancelPressed
            End If
        End If
    End Sub

    Public Sub SET_ProgressBar_Value(ByVal NewValue As Integer, ByVal NewText As String)
        If Not (ProgressForm Is Nothing) Then
            If Not CancelPressed Then
                DOEVENTS()
                If Not CancelPressed Then
                    ProgressForm.SET_VALUES(NewValue, NewText)
                End If
            End If
        End If
    End Sub

    Public Sub Dispose()
        If Not Disposed Then
            If Not (ProgressForm Is Nothing) Then
                ProgressForm.Close()
                ProgressForm = Nothing
            End If

            If Not (FP_ParentForm Is Nothing) Then
                FP_ParentForm.P_ENABLED = True
                FP_ParentForm = Nothing
            End If

            Disposed = True
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub
End Class

Public Class FP_L_Bitmap

    Public bmp As Bitmap

    Sub New(MyBitmap As Bitmap)
        bmp = MyBitmap
    End Sub

    Public ReadOnly Property CurrentLength
        Get
            Dim ms As New MemoryStream

            bmp.Save(ms, ImageFormat.Bmp)
            CurrentLength = ms.Length
        End Get
    End Property

    Public Function GetFileSize(Optional FilePath As String = "") As Double
        If FilePath = "" Then
            FilePath = String.Format("{0}{1}", gl_FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP), "checkfilesize.bmp")
        End If

        If File.Exists(FilePath) Then
            File.Delete(FilePath)
        End If

        bmp.Save(FilePath, ImageFormat.Bmp)

        Dim infoReader As System.IO.FileInfo
        infoReader = My.Computer.FileSystem.GetFileInfo(FilePath)

        GetFileSize = infoReader.Length
    End Function

    Public Function ConvertToJPEG() As Byte()
        Dim ms As New MemoryStream

        bmp.Save(ms, ImageFormat.Jpeg)

        ConvertToJPEG = ms.ToArray
    End Function

    Public Function ConvertToPNG() As Byte()
        Dim ms As New MemoryStream

        bmp.Save(ms, ImageFormat.Png)

        ConvertToPNG = ms.ToArray
    End Function

    Public Function ConvertToTIFF() As Byte()
        Dim ms As New MemoryStream

        bmp.Save(ms, ImageFormat.Tiff)

        ConvertToTIFF = ms.ToArray
    End Function

    Public Function ConvertToGIF() As Byte()
        Dim ms As New MemoryStream

        bmp.Save(ms, ImageFormat.Gif)

        ConvertToGIF = ms.ToArray
    End Function

    Public Function ConvertToBMP() As Byte()
        Dim ms As New MemoryStream

        bmp.Save(ms, ImageFormat.Bmp)

        ConvertToBMP = ms.ToArray
    End Function

    Public Sub SaveToJPEG(ByVal FilePath As String, Optional WithOverwrite As Boolean = True)
        If WithOverwrite = True Then
            If File.Exists(FilePath) Then
                File.Delete(FilePath)
            End If
        End If

        File.WriteAllBytes(FilePath, ConvertToJPEG())
    End Sub

    Public Sub SaveToPNG(ByVal FilePath As String, Optional WithOverwrite As Boolean = True)
        If WithOverwrite = True Then
            If File.Exists(FilePath) Then
                File.Delete(FilePath)
            End If
        End If

        File.WriteAllBytes(FilePath, ConvertToPNG())
    End Sub

    Public Sub SaveToTIFF(ByVal FilePath As String, Optional WithOverwrite As Boolean = True)
        If WithOverwrite = True Then
            If File.Exists(FilePath) Then
                File.Delete(FilePath)
            End If
        End If

        File.WriteAllBytes(FilePath, ConvertToTIFF())
    End Sub

    Public Sub SaveToGIF(ByVal FilePath As String, Optional WithOverwrite As Boolean = True)
        If WithOverwrite = True Then
            If File.Exists(FilePath) Then
                File.Delete(FilePath)
            End If
        End If

        File.WriteAllBytes(FilePath, ConvertToGIF())
    End Sub

    Public Sub SaveToBMP(ByVal FilePath As String, Optional WithOverwrite As Boolean = True)
        If WithOverwrite = True Then
            If File.Exists(FilePath) Then
                File.Delete(FilePath)
            End If
        End If

        File.WriteAllBytes(FilePath, ConvertToBMP())
    End Sub

End Class

Public Class FP_L_Rect
    Public Rect As Rectangle = New Rectangle(0, 0, 0, 0)

    Sub New(ByVal MyRect As Rectangle)
        Rect = MyRect
    End Sub

    Sub New(ByVal p As Point, ByVal s As Size)
        Rect = New Rectangle(p, s)
    End Sub

    Sub New(ByVal x As Integer, ByVal y As Integer, ByVal dx As Integer, ByVal dy As Integer)
        Rect = New Rectangle(x, y, dx, dy)
    End Sub

    Public Property LeftTop() As Point
        Get
            Return Rect.Location
        End Get
        Set(ByVal value As Point)
            Rect.Location = value
        End Set
    End Property

    Public Property LeftBottom() As Point
        Get
            Return New Point(Rect.X, Rect.Bottom)
        End Get
        Set(ByVal value As Point)
            Rect = New Rectangle(value.X, Rect.Y, Right - value.X, value.Y - Top())
        End Set
    End Property

    Public Property RightTop() As Point
        Get
            Return New Point(Rect.Right, Rect.Y)
        End Get
        Set(ByVal value As Point)
            Rect = New Rectangle(Rect.Left, value.Y, value.X - Rect.Left, Rect.Bottom - value.Y)
        End Set
    End Property

    Public Property RightBottom() As Point
        Get
            Return New Point(Rect.Right, Rect.Bottom)
        End Get
        Set(ByVal value As Point)
            Rect = New Rectangle(Rect.X, Rect.Y, value.X - Rect.X, value.Y - Rect.Y)
        End Set
    End Property

    Public Property Left() As Integer
        Get
            Return Rect.Left
        End Get
        Set(ByVal value As Integer)
            Rect = New Rectangle(value, Rect.Top, Rect.Right - value, Rect.Height)
        End Set
    End Property

    Public Property Right() As Integer
        Get
            Return Rect.Right
        End Get
        Set(ByVal value As Integer)
            Rect = New Rectangle(Rect.Left, Rect.Top, value - Rect.Left, Rect.Bottom)
        End Set
    End Property

    Public Property Top() As Integer
        Get
            Return Rect.Top
        End Get
        Set(ByVal value As Integer)
            Rect = New Rectangle(Rect.Left, value, Rect.Width, Rect.Bottom - value)
        End Set
    End Property

    Public Property Bottom() As Integer
        Get
            Return Rect.Bottom
        End Get
        Set(ByVal value As Integer)
            Rect = New Rectangle(Rect.X, Rect.Y, Rect.Width, value - Rect.Y)
        End Set
    End Property

    Public Property Width() As Integer
        Get
            Return Rect.Width
        End Get
        Set(ByVal value As Integer)
            Rect = New Rectangle(Rect.X, Rect.Y, value, Rect.Height)
        End Set
    End Property

    Public Property Height() As Integer
        Get
            Return Rect.Height
        End Get
        Set(ByVal value As Integer)
            Rect = New Rectangle(Rect.X, Rect.Y, Rect.Width, value)
        End Set
    End Property

    Function Contains(ByVal Rect2 As Rectangle) As Boolean
        Return Rect.Contains(Rect2)
    End Function

    Function Contains(ByVal Rect2 As FP_L_Rect) As Boolean
        Return Rect.Contains(Rect2.Rect)
    End Function

    Function Contains_Width(ByVal Rect2 As Rectangle) As Boolean
        Return (Rect.Left <= Rect2.Left And Rect.Right >= Rect2.Right)
    End Function

    Function Contains_Width(ByVal Rect2 As FP_L_Rect) As Boolean
        Return (Rect.Left <= Rect2.Left And Rect.Right >= Rect2.Right)
    End Function

    Function Contains_Height(ByVal Rect2 As Rectangle) As Boolean
        Return (Rect.Top <= Rect2.Top And Rect.Bottom >= Rect2.Bottom)
    End Function

    Function Contains_Height(ByVal Rect2 As FP_L_Rect) As Boolean
        Return (Rect.Top <= Rect2.Top And Rect.Bottom >= Rect2.Bottom)
    End Function

    Function Location() As Point
        Return Rect.Location
    End Function

    Function Size() As Size
        Return Rect.Size
    End Function

    Public Function GET_Rect_Next_to_me(ByVal Screen_Rect As FP_L_Rect, ByVal MyRectSize As Size, ByVal Orientation As ENUM_Direction, Optional ByVal Distance As Integer = 0, Optional ByVal OUT_Next_Orientation As ENUM_Direction = ENUM_Direction.None) As FP_L_Rect
        Dim OUT As New FP_L_Rect(0, 0, 0, 0)

        Select Case Orientation
            Case ENUM_Direction.Left
                Dim MRect As New FP_L_Rect(Rect.Left - MyRectSize.Width - Distance, Rect.Top, MyRectSize.Width, MyRectSize.Height)

                If Screen_Rect.Contains(MRect) Then
                    OUT_Next_Orientation = ENUM_Direction.Left
                    OUT = MRect
                Else
                    If Screen_Rect.Contains_Width(MRect) = False Then
                        OUT_Next_Orientation = ENUM_Direction.Right
                        OUT = New FP_L_Rect(Screen_Rect.Left, Screen_Rect.Top, Screen_Rect.Left + MRect.Width, Screen_Rect.Top + MRect.Height)
                    Else
                        MRect.Top = MRect.Bottom - MRect.Height
                        If MRect.Top < 0 Then
                            MRect.Top = 0
                        End If
                        OUT_Next_Orientation = ENUM_Direction.Left
                        OUT = MRect
                    End If
                End If

            Case ENUM_Direction.Right
                Dim MRect As New FP_L_Rect(Rect.Right + Distance, Rect.Top, MyRectSize.Width, MyRectSize.Height)

                If Screen_Rect.Contains(MRect) Then
                    OUT_Next_Orientation = ENUM_Direction.Right
                    OUT = MRect
                Else
                    If Screen_Rect.Contains_Width(MRect) = False Then
                        OUT = GET_Rect_Next_to_me(Screen_Rect, MyRectSize, ENUM_Direction.Left, 0, OUT_Next_Orientation)
                    Else
                        MRect.Top = MRect.Bottom - MRect.Height
                        If MRect.Top < 0 Then
                            MRect.Top = 0
                        End If
                        OUT_Next_Orientation = ENUM_Direction.Right
                        OUT = MRect
                    End If
                End If

            Case ENUM_Direction.Top
                Dim MRect As New FP_L_Rect(Rect.Left, Rect.Top - MyRectSize.Height - Distance, MyRectSize.Width, MyRectSize.Height)

                If Screen_Rect.Contains(MRect) Then
                    OUT_Next_Orientation = ENUM_Direction.Right
                    OUT = MRect
                Else
                    OUT = GET_Rect_Next_to_me(Screen_Rect, MyRectSize, ENUM_Direction.Right, 0, OUT_Next_Orientation)
                End If

            Case ENUM_Direction.Bottom
                Dim MRect As New FP_L_Rect(Rect.Left, Rect.Top + Rect.Height + Distance, MyRectSize.Width, MyRectSize.Height)

                If Screen_Rect.Contains(MRect) Then
                    OUT_Next_Orientation = ENUM_Direction.Right
                    OUT = MRect
                Else
                    OUT = GET_Rect_Next_to_me(Screen_Rect, MyRectSize, ENUM_Direction.Top, 0, OUT_Next_Orientation)
                End If

            Case Else
                FP_Globals.DoErrorMsgBox_Without_SQL_Connection("FP_Menu.SHOW_ME", 0, "Unknown orientation")
        End Select

        Return OUT
    End Function

    Public Function Zoom(ByVal MySize As Size) As Size
        Dim OUT As New Size(0, 0)

        If Rect.Width > 0 Or Rect.Height > 0 And MySize.Width > 0 And MySize.Height > 0 Then
            Dim a1 As Double = MySize.Width / Rect.Width
            Dim a2 As Double = MySize.Height / Rect.Height

            If a1 > a2 Then
                OUT.Width = Rect.Width / a1
                OUT.Height = Rect.Height / a1
            Else
                OUT.Width = Rect.Width / a2
                OUT.Height = Rect.Height / a2
            End If
        End If

        Return OUT
    End Function

End Class
Public Class FP_L_ReportViewer
    Public FPApp As FP_App
    Public RdlcReportName As String
    Public WithEvents ReportViewerControl As ReportViewer
    Public OtherParameters As String = ""

    Public Rendered As Boolean = False
    Private Disposed As Boolean = False

    Private m_currentPageIndex As Integer
    Private m_streams As IList(Of Stream)

    Public DATASOURCES As New List(Of DataTable)
    Public SUBREPORTS As New Dictionary(Of String, DataTable)

    Public Sub New(ByVal MyFPApp As FP_App)
        FPApp = MyFPApp
    End Sub

    Public Sub New(ByVal MyFPApp As FP_App, ByVal MyReportViewerControl As ReportViewer, ByVal MyRdlcReportName As String, Optional ByVal OtherParameters As String = "")
        'Hasznalat:     1) Peldany letrehozasa New-val
        '               2) Subreportok hozzaadasa Add_Subreport-tal
        '               3) Datasource-ok hozzaadasa DATASOURCES_ADD-dal
        '               4) PrepareReport sub meghivasaval indul a folyamat

        FPApp = MyFPApp
        ReportViewerControl = MyReportViewerControl
        RdlcReportName = MyRdlcReportName
        OtherParameters = OtherParameters
    End Sub

    Public Sub Dispose()
        FPApp = Nothing

        ReportViewerControl = Nothing

        RdlcReportName = ""

        Disposed = True
    End Sub

    Public Sub DATASOURCES_ADD(DT As DataTable)
        DATASOURCES.Add(DT)
    End Sub

    Public Sub Add_Subreport(ByVal MySubreportName As String, ByVal MyDT As DataTable)
        SUBREPORTS.Add(MySubreportName.Replace(".rdlc", ""), MyDT)
    End Sub

    Public Sub INIT(ByVal MyReportViewerControl As ReportViewer, ByVal MyRdlcReportName As String, Optional ByVal OtherParameters As String = "")
        ReportViewerControl = MyReportViewerControl
        RdlcReportName = MyRdlcReportName

        Dim FileAndLoc As String = FPApp.P.Report_Params.ReportPath + RdlcReportName

        If Not File.Exists(FileAndLoc) Then
            FPApp.DoErrorMsgBox("FP_L_ReportViewer", 0, String.Format("File not found '{0}'", FileAndLoc))
        Else
            Try
                Me.ReportViewerControl.Reset()  ' Az esetlegesen korĂˇbban hozzĂˇadott localreport-ok tĂ¶rlĂ©se
                Me.ReportViewerControl.ProcessingMode = ProcessingMode.Local

                Me.ReportViewerControl.LocalReport.ReportPath = FileAndLoc

                Dim RdlDsNames As System.Collections.Generic.IList(Of String) = Me.ReportViewerControl.LocalReport.GetDataSourceNames

                If RdlDsNames.Count < 1 Then
                    FPApp.DoErrorMsgBox("FP-->FP_FieldLogics.vb-->FP_L_ReportViewer.INIT", 0, String.Format("Dataset not found in file {0}", MyRdlcReportName))
                Else
                    Me.ReportViewerControl.LocalReport.DataSources.Clear()

                    For i As Integer = 0 To DATASOURCES.Count - 1
                        Dim ReportDatasetName As String = RdlDsNames(i)
                        Dim MyRDS As New ReportDataSource(ReportDatasetName, DATASOURCES(i))
                        Me.ReportViewerControl.LocalReport.DataSources.Add(MyRDS)
                    Next

                    If OtherParameters > "" Then
                        Try
                            Dim ParInfo As Microsoft.Reporting.WinForms.ReportParameterInfoCollection = Nothing
                            Dim ParamList As New Generic.List(Of ReportParameter)

                            ParInfo = ReportViewerControl.LocalReport.GetParameters

                            Dim RepParameter(ParInfo.Count - 1) As Microsoft.Reporting.WinForms.ReportParameter

                            For i As Integer = 0 To ParInfo.Count - 1
                                Select Case ParInfo(i).Name.ToUpper
                                    Case "OTHERPARAMETERS"
                                        RepParameter(i) = New Microsoft.Reporting.WinForms.ReportParameter(ParInfo(i).Name, OtherParameters, False)

                                    Case Else
                                        RepParameter(i) = New Microsoft.Reporting.WinForms.ReportParameter(ParInfo(i).Name)
                                        'RepParameter(i) = New Microsoft.Reporting.WinForms.ReportParameter(ParInfo(i).Name, ParInfo(i).Values(0))
                                End Select
                                ParamList.Add(RepParameter(i))
                            Next
                            ReportViewerControl.LocalReport.SetParameters(ParamList)

                        Catch ex As Exception
                            'Nothing to do
                        End Try
                    End If

                    AddHandler ReportViewerControl.LocalReport.SubreportProcessing, AddressOf Me.SubreportProcessingEventHandler

                    Rendered = False
                    ReportViewerControl.RefreshReport()

                    While Rendered = False
                        System.Threading.Thread.Sleep(200)
                        Application.DoEvents()
                    End While
                End If

                'Dim PageSettings As New System.Drawing.Printing.PageSettings

                'With PageSettings
                '.Landscape = ReportViewerControl.LocalReport.GetDefaultPageSettings.IsLandscape
                '.PaperSize = ReportViewerControl.LocalReport.GetDefaultPageSettings.PaperSize
                '.Margins = ReportViewerControl.LocalReport.GetDefaultPageSettings.Margins
                'End With
                'ReportViewerControl.SetPageSettings(PageSettings)

            Catch ex As Exception
                FPApp.DoErrorMsgBox("FP_FieldLogics..INIT", Err.Number, Err.Description)
            End Try
        End If
    End Sub

    Public Sub PrepareReport()
        INIT(ReportViewerControl, RdlcReportName, OtherParameters)
    End Sub

    Private Sub SubreportProcessingEventHandler(ByVal sender As Object, ByVal e As SubreportProcessingEventArgs)
        If e.DataSourceNames(0) > "" Then
            e.DataSources.Add(New ReportDataSource(e.DataSourceNames(0), SUBREPORTS(System.IO.Path.GetFileNameWithoutExtension(e.ReportPath))))
        End If
    End Sub

    Public Function SaveToEXCEL(Optional ByVal FileName As String = "", Optional ShowIt As Boolean = False) As Boolean
        SaveToEXCEL = SaveToFile("Excel", FileName, ShowIt)
    End Function

    Public Function SaveToPDF(Optional ByVal FileName As String = "", Optional ShowIt As Boolean = False) As Boolean
        SaveToPDF = SaveToFile("PDF", FileName, ShowIt)
    End Function

    Public Function SaveToFile(ByVal FileFormat As String, Optional ByVal FileName As String = "", Optional ShowIt As Boolean = False) As Boolean
        Dim OUT As Boolean = False
        Dim StartDate As DateTime = Now

        CURSOR_SHOW_WAIT()
        While Rendered = False And DateDiff(DateInterval.Second, StartDate, Now) < 30
            System.Threading.Thread.Sleep(2000)
        End While
        CURSOR_SHOW_DEFAULT()

        If Rendered = True Then
            If FileFormat <> "PDF" And FileFormat <> "Excel" Then
                FPApp.DoErrorMsgBox("FP_Print.vb..SaveToFile", 0, String.Format("Unknown format '{0}'", FileFormat))
            Else
                If FileName = "" Then
                    Dim FileExtension As String = "*.*"

                    Select Case FileFormat
                        Case "PDF"
                            FileExtension = "PDF|*.pdf"

                        Case "Excel"
                            FileExtension = "Excel|*.xls"
                    End Select

                    FileName = FPApp.Windows_FileOpenDialogBox(FileExtension)
                End If

                If FileName > "" Then
                    Dim warnings As Warning() = Nothing
                    Dim streamids As String() = Nothing
                    Dim mimeType As String = Nothing
                    Dim encoding As String = Nothing
                    Dim extension As String = Nothing
                    Dim bytes As Byte()
                    Dim Done As Boolean = False

                    Try
                        bytes = ReportViewerControl.LocalReport.Render(FileFormat, Nothing, mimeType, encoding, extension, streamids, warnings)

                        Dim fs As New FileStream(FileName, FileMode.Create)

                        fs.Write(bytes, 0, bytes.Length)
                        fs.Close()
                        OUT = True
                        Done = True

                    Catch ex As Exception
                        FPApp.DoMyMsgBox(129) 'Nem sikerult a file mentese
                    End Try

                    If Done Then
                        If ShowIt Then
                            Try
                                Process.Start(FileName)

                            Catch ex As Exception
                                'Nothing to do
                            End Try
                        End If
                    Else

                    End If
                End If
            End If
        End If

        SaveToFile = OUT
    End Function

    Private Sub PrintPage(ByVal sender As Object, ByVal ev As PrintPageEventArgs)
        Dim pageImage As New Metafile(m_streams(m_currentPageIndex))


        ev.Graphics.DrawImage(pageImage, ev.PageBounds)

        m_currentPageIndex += 1
        ev.HasMorePages = (m_currentPageIndex < m_streams.Count)
    End Sub

    Private Function CreateStream(ByVal name As String, ByVal fileNameExtension As String, ByVal encoding As System.Text.Encoding, ByVal mimeType As String, ByVal willSeek As Boolean) As Stream
        Dim FileNum As Integer = FPApp.NachsteNummerVergeben
        Dim stream As Stream = New FileStream(name & FileNum & "." & fileNameExtension, FileMode.Create)
        m_streams.Add(stream)
        Return stream
    End Function

    Private Sub ExportReport(ByVal ExportType As String)
        Dim deviceInfo As String =
"  <DeviceInfo>" +
"  <OutputFormat>EMF</OutputFormat>" +
"  <PageWidth>#_PAGE_WIDTH_#</PageWidth>" +
"  <PageHeight>#_PAGE_HEIGHT_#</PageHeight>" +
"  <MarginTop>#_MARGIN_TOP_#</MarginTop>" +
"  <MarginLeft>#_MARGIN_LEFT_#</MarginLeft>" +
"  <MarginRight>#_MARGIN_RIGHT_#</MarginRight>" +
"  <MarginBottom>#_MARGIN_BOTTOM_#</MarginBottom>" +
"</DeviceInfo>"

        With ReportViewerControl.LocalReport.GetDefaultPageSettings
            deviceInfo = Strings.Replace(deviceInfo, "#_PAGE_WIDTH_#", .PaperSize.Width)
            deviceInfo = Strings.Replace(deviceInfo, "#_PAGE_HEIGHT_#", .PaperSize.Height)

            deviceInfo = Strings.Replace(deviceInfo, "#_MARGIN_TOP_#", .Margins.Top)
            deviceInfo = Strings.Replace(deviceInfo, "#_MARGIN_LEFT_#", .Margins.Left)
            deviceInfo = Strings.Replace(deviceInfo, "#_MARGIN_RIGHT_#", .Margins.Right)
            deviceInfo = Strings.Replace(deviceInfo, "#_MARGIN_BOTTOM_#", .Margins.Bottom)
        End With

        Dim warnings As Warning() = Nothing
        Try
            m_streams = New List(Of Stream)()
            ReportViewerControl.LocalReport.Render(ExportType, deviceInfo, AddressOf CreateStream, warnings)

            For Each stream As Stream In m_streams
                stream.Position = 0
            Next
        Catch ex As Exception
            Call FPApp.DoErrorMsgBox("FP_FieldLogics.vb..ExportReport", Err.Number, Err.Description)
        End Try
    End Sub

    Public Sub Print_With_Dialog()
        Rendered = False
        ReportViewerControl.RefreshReport()

        While Rendered = False
            System.Threading.Thread.Sleep(200)
            Application.DoEvents()
        End While

        ReportViewerControl.PrintDialog()
    End Sub

    Public Function Print(Optional ByVal PFDKey_for_Printer As String = "PRINTER_LAST_SELECTED", Optional ByVal DocumentName As String = "", Optional ByVal WithPrintDialog As Boolean = False, Optional ByVal TryAgainDialog As Boolean = False)
        Dim OUT As Boolean = False
        Dim warnings As Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim bytes As Byte()
        Dim DoIt As Boolean = True
        Dim PDoc As New Printing.PrintDocument
        Dim ControlPressed As Boolean = My.Computer.Keyboard.CtrlKeyDown

        CURSOR_SHOW_WAIT()

        bytes = ReportViewerControl.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
        ExportReport("Image")
        CURSOR_SHOW_DEFAULT()

        Dim oPS As New System.Drawing.Printing.PrinterSettings
        Dim SelectedPrinter As String = ""

        If ControlPressed = True Then
            SelectedPrinter = ""
            WithPrintDialog = True
        Else
            If PFDKey_for_Printer > "" Then
                FPApp.PFDlesen(PFDKey_for_Printer, SelectedPrinter)
                PDoc.PrinterSettings.PrinterName = SelectedPrinter
                If Not PDoc.PrinterSettings.IsValid Then
                    SelectedPrinter = ""
                    WithPrintDialog = True
                End If
            End If
        End If

        If WithPrintDialog Then
            Dim PDialog As New PrintDialog

            PDialog.PrinterSettings.PrinterName = SelectedPrinter
            'PDialog.Document.DefaultPageSettings.Landscape = True

            Dim F As Form = FPApp.ShowDialogForm_getOpacityForm(Form.ActiveForm)
            DoIt = (PDialog.ShowDialog() = DialogResult.OK)
            If Not (F Is Nothing) Then
                F.Close()
            End If

            If DoIt Then
                SelectedPrinter = PDialog.PrinterSettings.PrinterName
                If PFDKey_for_Printer > "" Then
                    FPApp.PFDinsertOrUpdate(PFDKey_for_Printer, SelectedPrinter)
                End If
            End If
        End If

        If DoIt Then
            If SelectedPrinter = "" Then
                Try
                    SelectedPrinter = oPS.PrinterName

                Catch ex As Exception
                    DoIt = False
                End Try
            End If
        End If

        If DoIt Then
            PDoc.PrinterSettings.PrinterName = SelectedPrinter
            'PDoc.DefaultPageSettings.PaperSize = New System.Drawing.Printing.PaperSize("A4 Landscape", 11000, 8700)
            'PDoc.DefaultPageSettings.Landscape = True

            If DocumentName.Trim = "" Then
                DocumentName = "Document"
            End If
            PDoc.DocumentName = DocumentName

            AddHandler PDoc.PrintPage, New PrintPageEventHandler(AddressOf PrintPage)
            PDoc.Print()
            OUT = True
        End If

        Print = OUT
    End Function

    Private Sub ReportViewerControl_RenderingComplete(ByVal sender As Object, ByVal e As Microsoft.Reporting.WinForms.RenderingCompleteEventArgs) Handles ReportViewerControl.RenderingComplete
        Rendered = True
    End Sub
End Class
Public Class FP_L_Rtf_InfoField_With_PlusButton
    Public Event EVENT_NAVIGATION_FORWARD(ByVal sender_FP_L As FP_L_Rtf_InfoField_With_PlusButton, ByRef Cancel As Integer)

    Public WithEvents FP As FP = Nothing
    Public WithEvents FPc_InfoField As FP_Control = Nothing
    Public WithEvents FPp_PlusButton As FP_PictureBox = Nothing
    Private WithEvents c_Plusbutton As PictureBox = Nothing
    Private WithEvents c_InfoField As Control = Nothing

    Public DoNotHandelCursorMove As Boolean = False

    Private Disposed As Boolean = False

    Sub New(ByVal MyFP As FP, ByVal MyFPc_InfoField As FP_Control, ByVal MyFPp_PlusButton As FP_PictureBox, Optional MyDoNotHandelCursorMove As Boolean = False)
        FP = MyFP
        FPc_InfoField = MyFPc_InfoField
        If Not (FPc_InfoField Is Nothing) Then
            c_InfoField = FPc_InfoField.c
        End If
        FPp_PlusButton = MyFPp_PlusButton
        DoNotHandelCursorMove = MyDoNotHandelCursorMove
    End Sub

    Public Sub Dispose()
        FP = Nothing
        FPc_InfoField = Nothing
        FPp_PlusButton = Nothing
        c_Plusbutton = Nothing
        c_InfoField = Nothing

        Disposed = True
    End Sub

    Public Function NAVIGATION_FORWARD() As Boolean
        Dim OUT As Boolean = False
        Dim Cancel As Integer = 0
        Dim Current_ID As Long = 0

        If Not (FPp_PlusButton Is Nothing) Then
            FOCUS_ON_IMMEDIATELY(FPp_PlusButton.c)
        End If

        If Not (FP Is Nothing) Then
            Cancel = (Not FP.FPf.SAVE_ALL())

            If Not Cancel Then
                Cancel = (FP.P_DATA_RecordStatus <> ENUM_RecordStatus.EXISTS)
            End If

            If Not Cancel Then
                Current_ID = FP.P_DATA_Current_ID
            End If

            If Not Cancel Then
                RaiseEvent EVENT_NAVIGATION_FORWARD(Me, Cancel)
            End If

            If Cancel Then
                If DoNotHandelCursorMove = False Then
                    If Not (c_InfoField Is Nothing) Then
                        FP.FPf.FOCUS_ON_AT_THE_END(c_InfoField, , True)
                    End If
                End If
            Else
                OUT = True
                If DoNotHandelCursorMove = False Then
                    FP.FPf.FOCUS_ON_AT_THE_END(FP.FPf.CONTROLS_GET_NEXTCONTROL(FPc_InfoField.c))
                End If
            End If
        End If
    End Function

    Public WriteOnly Property P_VISIBLE As Boolean
        Set(value As Boolean)
            If Not (FPp_PlusButton Is Nothing) Then
                FPp_PlusButton.P_VISIBLE = value
            End If
            If Not (FPc_InfoField Is Nothing) Then
                FPc_InfoField.P_VISIBLE = value
            End If
        End Set
    End Property

#Region "EVENTS"
    Private Sub EVENT_Details_Info_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles c_InfoField.KeyDown
        If FIELD_LOCKED_BUT_ENTER(e) Then
            NAVIGATION_FORWARD()
        End If
    End Sub
    Private Sub EVENT_Details_Info_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles c_InfoField.DoubleClick

    End Sub

#End Region

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Private Sub FPp_PlusButton_CLICK(ByVal sender_FPc As FP_PictureBox, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FPp_PlusButton.CLICK
        NAVIGATION_FORWARD()
    End Sub

    Private Sub FPc_InfoField_Field_Doubleclick(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Handled As Boolean) Handles FPc_InfoField.Field_Doubleclick
        NAVIGATION_FORWARD()
    End Sub

    Private Sub FPc_InfoField_Field_Marker_Click(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Handled As Boolean) Handles FPc_InfoField.Field_Marker_Click
        NAVIGATION_FORWARD()
    End Sub
End Class
Public Class FP_L_SplitContainer_Resize

    Private SplitContainer As SplitContainer
    Private WithEvents PictureBox As PictureBox
    Private SplitContainer_State As Integer = 1

    Public Sub New(MySplitContainer As SplitContainer, MyPictureBox As FP_PictureBox)
        SplitContainer = MySplitContainer
        PictureBox = MyPictureBox.c

        If Not (MyPictureBox.FPf.HELP_Frm Is Nothing) Then
            With MyPictureBox.FPf.HELP_Frm
                .ADD_HELP_STANDARD_ITEM("###SplitContainer_Resize###", PictureBox.Name)
            End With
        End If
    End Sub

    Private Sub PictureBox_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox.MouseUp
        If SplitContainer.Orientation = Orientation.Horizontal Then
            If SplitContainer_State = 0 Then
                SplitContainer.SplitterDistance = SplitContainer.Height / 2
                SplitContainer_State = 1
                Exit Sub
            End If
            If SplitContainer_State = 1 Then
                SplitContainer.SplitterDistance = SplitContainer.Height
                SplitContainer_State = 2
                Exit Sub
            End If
            If SplitContainer_State = 2 Then
                SplitContainer.SplitterDistance = 0
                SplitContainer_State = 0
                Exit Sub
            End If
        End If

        If SplitContainer.Orientation = Orientation.Vertical Then
            If SplitContainer_State = 0 Then
                SplitContainer.SplitterDistance = SplitContainer.Width / 2
                SplitContainer_State = 1
                Exit Sub
            End If
            If SplitContainer_State = 1 Then
                SplitContainer.SplitterDistance = SplitContainer.Width
                SplitContainer_State = 2
                Exit Sub
            End If
            If SplitContainer_State = 2 Then
                SplitContainer.SplitterDistance = 0
                SplitContainer_State = 0
                Exit Sub
            End If
        End If
    End Sub
End Class
Public Class FP_L_SplitContainer_with_Autosize_on_Enter
    Public WithEvents _MySplitContainer As SplitContainer
    Public WithEvents _MyForm As Form
    Public Control_Top As Object
    Public Control_Bottom As Object
    Public Sub New(MySplitContainer As SplitContainer, MyForm As Form)
        _MySplitContainer = MySplitContainer
        _MyForm = MyForm
    End Sub
    Private Sub Set_To_Bottom()
        If Not Control_Bottom Is Nothing Then
            Dim pTop As Integer
            Dim pHeight As Integer
            pTop = Control_Bottom.Top
            pHeight = Control_Bottom.Height
            _MySplitContainer.SplitterDistance = pTop + pHeight
        End If
    End Sub
    Private Sub Set_To_Top()
        If Not Control_Top Is Nothing Then
            Dim pTop As Integer
            Dim pHeight As Integer
            pTop = Control_Top.Top
            pHeight = Control_Top.Height
            _MySplitContainer.SplitterDistance = pTop + pHeight
        End If
    End Sub
    Private Sub SplitPanel1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles _MySplitContainer.Panel1.Enter
        Set_To_Bottom()
    End Sub
    Private Sub SplitPanel2_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles _MySplitContainer.Panel2.Enter
        Set_To_Top()
    End Sub
    Private Sub Myform_Size_Changed(sender As Object, e As System.EventArgs) Handles _MyForm.SizeChanged
        Set_To_Bottom()
    End Sub
End Class

Public Class FP_L_TreeView_RowMarker
    'Segitsegevel kijelolheto egy node a TreeView-ban alahuzassal.
    'Ertelme az osztalynak az, hogy a marker athelyezesenel automatikusan leveszi a regi markert.

    Public Enum ENUM_MARKER_TYPE
        UNDERLINE = 0
    End Enum
    Public Structure Struct_FP_L_TreeView_RowMarker_Params
        Dim TV As TreeView
        Dim Marker_Type As ENUM_MARKER_TYPE
    End Structure

    Public TV As TreeView
    Public Marker_Type As ENUM_MARKER_TYPE
    Private Marker_Visible As Boolean = True
    Private Disposed As Boolean = False
    Private Current_Marker_Pos As String = ""

    Sub New(P As Struct_FP_L_TreeView_RowMarker_Params)
        With P
            TV = .TV
            Marker_Type = .Marker_Type
        End With
    End Sub

    Sub DisposeMe()
        If Disposed = False Then
            Disposed = True

            TV = Nothing
        End If
    End Sub

    Public Sub REMOVE_MARKER()
        If Disposed = False Then
            If Current_Marker_Pos > "" Then
                Dim Current_Marked_Node As TreeNode = TREEVIEW_Node_GET_Direct_From_TV(TV, Current_Marker_Pos)

                If Not (Current_Marked_Node Is Nothing) Then
                    Select Case Marker_Type
                        Case ENUM_MARKER_TYPE.UNDERLINE
                            TREEVIEW_NODE_SET_UNDERLINE(TV, Current_Marked_Node, False)
                            Current_Marker_Pos = ""

                        Case Else
                            gl_FPApp.DoErrorMsgBox("FP_FieldLogics..FP_L_TreeView_RowMarker", 0, "Unknown marker type")
                    End Select
                End If
            End If
        End If
    End Sub

    Public Sub SET_MARKER_POSITION(OnNode_Name As String, Optional EnsureVisible As Boolean = True)
        If Disposed = False Then
            If Trim(nz(OnNode_Name, "")) = "" Then
                REMOVE_MARKER()

            ElseIf OnNode_Name <> Current_Marker_Pos Then
                REMOVE_MARKER()

                Dim OnNode As TreeNode = TREEVIEW_Node_GET_Direct_From_TV(TV, OnNode_Name)

                If Not (OnNode Is Nothing) Then
                    Select Case Marker_Type
                        Case ENUM_MARKER_TYPE.UNDERLINE
                            TREEVIEW_NODE_SET_UNDERLINE(TV, OnNode, True)
                            Current_Marker_Pos = OnNode.Name
                            If EnsureVisible Then
                                OnNode.EnsureVisible()
                            End If

                        Case Else
                            gl_FPApp.DoErrorMsgBox("FP_FieldLogics..SET_MARKER_POSITION", 0, "Unknown marker type")
                    End Select

                End If
            End If
        End If
    End Sub
End Class

Public Class FP_L_TreeView
    'Lemasol egy mar kitoltott treeview-t.
    'Letrehoz egy DT-t a treeview node-kbol. A DT felepitese megegyezik az FP_L_TreeView_With_FP-ben hasznalttal
    'Letrehoz egy DICTIONARY-t a treeview node-aibol.
    Public Structure Struct_FP_L_TreeView_Params
        Dim TV As TreeView
        Dim FPf As FP_Form
    End Structure

    Private Enum Enum_Timer_Func_Codes As Integer
        DO_AFTER_CHECK = 2
        GOTO_NODE = 3
    End Enum

    Public Event Node_CheckedChanged(ByVal sender As FP_L_TreeView, ByVal e As System.Windows.Forms.TreeViewEventArgs, ByVal IndexOfFP As Integer, ByVal FP As FP, ByVal RecordID As Long)
    Public Event Node_KeyPress(ByVal sender As FP_L_TreeView, ByVal e As System.Windows.Forms.KeyPressEventArgs, ByVal RecordID As Long)
    Public Event Node_Before_Select(sender As FP_L_TreeView, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs)
    Public Event Node_After_Select(sender As FP_L_TreeView, ByVal e As System.Windows.Forms.TreeViewEventArgs)

    Public WithEvents TV As TreeView
    Public TV_SOURCE As TreeView
    Public FPf As FP_Form
    Public COLOR_TV_NORMAL_BG As Color = Color.FromArgb(255, 255, 255)
    Public COLOR_TV_SELECTED_BG As Color = Color.FromArgb(200, 255, 200)
    Public COLOR_TV_NORMAL As Color = Color.FromArgb(0, 0, 0)
    Public COLOR_TV_SELECTED As Color = Color.FromArgb(0, 0, 0)

    Public DT As New DataTable
    Public DIC_NODES As New Dictionary(Of String, TreeNode)

    Private TV_LastSelectedNode_Key As String = ""
    Private TV_Binded As Boolean = True

    Private WithEvents T As New System.Windows.Forms.Timer
    Private T_FunctionsCode As Enum_Timer_Func_Codes = Enum_Timer_Func_Codes.GOTO_NODE
    Private T_GotoNode As String = ""
    Private T_Do_AfterCheck As Boolean = False
    Private T_Node As TreeNode
    Private T_Checked As Boolean = False

    Private Disposed As Boolean = False

    Sub New(ByVal P As Struct_FP_L_TreeView_Params)
        FPf = P.FPf
        TV = P.TV
        TV.HideSelection = False
    End Sub

    Public Sub Dispose()
        If Disposed = False Then
            TV = Nothing
            TV_SOURCE = Nothing
            DIC_NODES.Clear()
            DT.Rows.Clear()
        End If

        Disposed = True
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Private Function NODE_get_ParamLine_From_DT(ByVal NodeKey As String) As DataRow
        Dim OUT As DataRow = Nothing

        If DT Is Nothing Then
            gl_FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_TreeView_Copy.NODE_get_ParamLine_From_DT", 0, "DT is nothing")
        Else
            Dim Level As Integer = Val(Mid(NodeKey, 1, 1))
            Dim ID As Long = Val(Mid(NodeKey, 3, 20))
            Dim Criteria As String = String.Format("Level = {0} And ID = {1}", Level, ID)

            If DT.Select(Criteria).Count <> 1 Then
                gl_FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_TreeView_Copy.NODE_get_ParamLine_From_DT", 0, String.Format("Node '{0}' in DT not found.", NodeKey))
            Else
                OUT = DT.Select(Criteria).First
            End If
        End If

        NODE_get_ParamLine_From_DT = OUT
    End Function

    Private Sub NODE_COLORING(ByVal NodeKey As String)
        If Not DIC_NODES.ContainsKey(NodeKey) Then
            gl_FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_TreeView_With_FP.NODE_COLORING", 0, String.Format("DIC_NODES does not contains node '{0}'", NodeKey))
        Else
            With DIC_NODES(NodeKey)
                If TV_LastSelectedNode_Key = NodeKey Then
                    'DIC_NODES(NodeKey).BackColor = COLOR_TV_SELECTED_BG
                Else
                    Dim Props As DataRow = NODE_get_ParamLine_From_DT(NodeKey)

                    If Not (Props Is Nothing) Then
                        Dim GridFiltered As Boolean = False
                        Dim CurrentNormalBackColor As Color = IIf(Props!Text_BackColor = -1, COLOR_TV_NORMAL_BG, Color.FromArgb(Props!Text_BackColor))
                        Dim CurrentNormalForeColor As Color = IIf(Props!Text_ForeColor = -1, COLOR_TV_NORMAL, Color.FromArgb(Props!Text_ForeColor))

                        .BackColor = CurrentNormalBackColor
                        .ForeColor = CurrentNormalForeColor
                    End If
                End If
            End With
        End If
    End Sub

    Public Function TV_EXPANDED_STATE_GET() As List(Of String)
        Dim Lst_Not_Expanded As New List(Of String)

        For Each Key As String In DIC_NODES.Keys
            With DIC_NODES(Key)
                If .IsExpanded = False Then
                    Lst_Not_Expanded.Add(Key)
                End If
            End With
        Next

        Return Lst_Not_Expanded
    End Function

    Public Sub TV_EXPANDED_STATE_SET(ByVal Lst_Not_Expanded As List(Of String))
        TV.ExpandAll()

        For Each Key As String In Lst_Not_Expanded
            If DIC_NODES.Keys.Contains(Key) Then
                If DIC_NODES(Key).IsExpanded Then
                    DIC_NODES(Key).Collapse()
                End If
            End If
        Next
    End Sub

    Private Sub TV_NODE_REMOVE(ByVal Key As String)
        Dim Nodes_To_Remove As New List(Of String)

        TV_NODE_GetAllChildren(DIC_NODES(Key), Nodes_To_Remove)

        Nodes_To_Remove.Sort()

        For i As Integer = Nodes_To_Remove.Count - 1 To 0
            DIC_NODES(Nodes_To_Remove(i)).Remove()
            DIC_NODES.Remove(Nodes_To_Remove(i))
        Next
    End Sub

    Sub FILL_FROM_TV(From_TV As TreeView)
        DIC_NODES.Clear()
        DT.Dispose()

        TV.ImageList = Nothing

        If Not From_TV.ImageList Is Nothing Then
            TV.ImageList = From_TV.ImageList
        End If

        DT = New DataTable

        With DT.Columns
            .Add("Level", System.Type.GetType("System.Int32"))
            .Add("ID", System.Type.GetType("System.Int32"))
            .Add("TransactID", System.Type.GetType("System.Int32"))
            .Add("Root_RecordID", System.Type.GetType("System.Int32"))
            .Add("ParentID", System.Type.GetType("System.Int32"))
            .Add("SeqNum", System.Type.GetType("System.Int32"))
            .Add("Text", System.Type.GetType("System.String"))
            .Add("Image", System.Type.GetType("System.String"))
            .Add("Image_Selected", System.Type.GetType("System.String"))
            .Add("Text_BackColor", System.Type.GetType("System.Int32"))
            .Add("Text_ForeColor", System.Type.GetType("System.Int32"))
            .Add("Text_Underline", System.Type.GetType("System.Int32"))
        End With

        If From_TV.Nodes.Count > 0 Then
            Dim Parent_Node As TreeNode = Nothing
            Dim Current_Node As TreeNode = From_TV.Nodes(0)
            Dim Current_RootNode As TreeNode = Current_Node
            Dim SeqNum As Integer = 0

            Do
                SeqNum += 1

                If (Current_Node.Parent Is Nothing) Then
                    Current_RootNode = Current_Node
                End If

                Dim NewNode As New TreeNode(Current_Node.Name)

                With NewNode
                    .Name = Current_Node.Name
                    .Text = Current_Node.Text
                    .ImageKey = Current_Node.ImageKey
                    .SelectedImageKey = Current_Node.SelectedImageKey
                    .BackColor = Current_Node.BackColor
                    .ForeColor = Current_Node.ForeColor
                    If TREEVIEW_NODE_GET_UNDERLINE(Current_Node) Then
                        TREEVIEW_NODE_SET_UNDERLINE(TV, NewNode, True)
                    End If

                    If Current_Node.IsExpanded Then
                        .Expand()
                    End If
                End With

                DIC_NODES.Add(NewNode.Name, NewNode)

                Dim NewRow As DataRow = DT.NewRow

                With NewRow
                    !Level = NODE_GET_LEVEL_FROM_Key(Current_Node.Name)
                    !ID = NODE_GET_ID_FROM_Key(Current_Node.Name)
                    !TransactID = 0
                    !Root_RecordID = NODE_GET_ID_FROM_Key(Current_RootNode.Name)

                    If (Parent_Node Is Nothing) Then
                        !ParentID = 0
                    Else
                        !ParentID = NODE_GET_ID_FROM_Key(Parent_Node.Name)
                    End If

                    !SeqNum = SeqNum
                    !Text = Current_Node.Text
                    !Image = Current_Node.ImageKey
                    !Image_Selected = Current_Node.SelectedImageKey
                    !Text_BackColor = Current_Node.BackColor.ToArgb
                    !Text_ForeColor = Current_Node.ForeColor.ToArgb
                    !Text_Underline = TREEVIEW_NODE_GET_UNDERLINE(Current_Node)
                End With

                DT.Rows.Add(NewRow)

                If (Parent_Node Is Nothing) Then
                    TV.Nodes.Add(NewNode)
                Else
                    DIC_NODES(Parent_Node.Name).Nodes.Add(NewNode)
                End If

                If Current_Node.Nodes.Count > 0 Then
                    Parent_Node = Current_Node
                    Current_Node = Current_Node.Nodes(0)
                Else
                    Dim Next_Node As TreeNode = Current_Node.NextNode

                    While ((Next_Node Is Nothing) And Not (Current_Node Is Nothing))
                        Current_Node = Parent_Node
                        If Not (Current_Node Is Nothing) Then
                            Parent_Node = Current_Node.Parent
                            Next_Node = Current_Node.NextNode
                        End If
                    End While

                    Current_Node = Next_Node
                End If

            Loop While Not (Current_Node Is Nothing)
        End If
    End Sub

    Public Sub TV_GOTO_Node_AT_THE_END(ByVal Level As Integer, ByVal ID As Long)
        TV_GOTO_Node_AT_THE_END(TREEVIEW_get_Node_Key_from_Elements(Level, ID))
    End Sub

    Public Sub TV_GOTO_Node_AT_THE_END(ByVal GotoNode As String)
        If Not T.Enabled Then
            T_FunctionsCode = Enum_Timer_Func_Codes.GOTO_NODE
        End If

        T_GotoNode = GotoNode

        T.Interval = 20
        T.Enabled = True
    End Sub

    Sub TV_UNSELECT()
        If TV_LastSelectedNode_Key > "" Then
            Dim Old_Key As String = TV_LastSelectedNode_Key

            TV.SelectedNode = Nothing
            TV_LastSelectedNode_Key = ""
            If Not DIC_NODES.ContainsKey(Old_Key) Then
                gl_FPApp.DoErrorMsgBox("FP_L_TreeView_Copy.TV_UNSELECT", 0, String.Format("TV_LastSelectedNode_Key in DIC_NODES not found. ('{0}')", Old_Key))
            Else
                NODE_COLORING(Old_Key)
            End If
        End If
    End Sub

    Function TV_SELECT(ByVal NodeKey As String) As Boolean
        Dim Level As Integer = NODE_GET_LEVEL_FROM_Key(NodeKey)
        Dim ID As Integer = NODE_GET_ID_FROM_Key(NodeKey)

        Return TV_SELECT(Level, ID)
    End Function

    Function TV_SELECT(ByVal Level As Integer, ByVal ID As Long) As Boolean
        Dim OUT As Boolean = False
        Dim Key As String = TREEVIEW_get_Node_Key_from_Elements(Level, ID)

        If Key = TV_LastSelectedNode_Key Then
            OUT = True
        Else
            TV_UNSELECT()
            If Not DIC_NODES.ContainsKey(Key) Then
                gl_FPApp.DoErrorMsgBox("FP_L_TreeView_Copy.TV_SELECT", 0, String.Format("Node not found. (Key: '{0}'", Key))
            Else
                TV_LastSelectedNode_Key = Key
                TV.SelectedNode = DIC_NODES(Key)

                NODE_COLORING(Key)

                OUT = True
            End If
        End If

        If OUT = True Then
            DIC_NODES(Key).EnsureVisible()
        End If

        Return OUT
    End Function

    Public Function NODE_GET_LEVEL_FROM_Key(ByVal MyKey As String) As Integer
        Return Val(Mid(MyKey, 1, 1))
    End Function

    Public Function NODE_GET_ID_FROM_Key(ByVal MyKey As String) As Long
        Return Val(Mid(MyKey, 3))
    End Function

    Public Function TV_SELECTED_NODE_GET_LEVEL() As Integer
        Dim OUT As Integer = -1

        If Not TV Is Nothing Then
            If Not TV.SelectedNode Is Nothing Then
                OUT = NODE_GET_LEVEL_FROM_Key(TV.SelectedNode.Name)
            End If
        End If

        Return OUT
    End Function

    Public Function NODE_GET_ROOT_Node(ByVal MyNode As TreeNode) As TreeNode
        Dim OUT As TreeNode = MyNode

        If Not (OUT Is Nothing) Then
            Do While Not (OUT.Parent Is Nothing)
                OUT = OUT.Parent
            Loop
        End If

        Return OUT
    End Function

    Public Function TV_SELECTED_NODE_GET_ROOT_ID() As Long
        Dim OUT As Long = 0

        If Not TV Is Nothing Then
            If Not TV.SelectedNode Is Nothing Then
                Dim wNode As TreeNode = TV.SelectedNode

                Do While Not (wNode.Parent Is Nothing)
                    wNode = wNode.Parent
                Loop

                OUT = NODE_GET_ID_FROM_Key(wNode.Name)
            End If
        End If

        Return OUT
    End Function

    Public Function TV_SELECTED_NODE_GET_ID() As Long
        Dim OUT As Long = 0

        If Not TV Is Nothing Then
            If Not TV.SelectedNode Is Nothing Then
                OUT = NODE_GET_ID_FROM_Key(TV.SelectedNode.Name)
            End If
        End If

        Return OUT
    End Function

    Public Sub TV_SELECTED_NODE_EnsureVisible()
        Dim CurrNode As TreeNode = TV_SELECTED_NODE()

        If Not (CurrNode Is Nothing) Then
            CurrNode.EnsureVisible()
        End If
    End Sub

    Public Function TV_SELECTED_NODE() As TreeNode
        Dim OUT As TreeNode = Nothing

        If TV_LastSelectedNode_Key > "" Then
            If Not DIC_NODES.ContainsKey(TV_LastSelectedNode_Key) Then
                gl_FPApp.DoErrorMsgBox("FP_L_TreeView_Copy.TV_SELECTED_NODE", 0, String.Format("TV_LastSelectedNode_Key ('{0}') is invalid.", TV_LastSelectedNode_Key))
            Else
                OUT = DIC_NODES(TV_LastSelectedNode_Key)
            End If
        End If

        Return OUT
    End Function

    Private Sub TV_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TV.AfterCheck
        Dim IndexOf_CheckedFP As Integer = Val(Mid(e.Node.Name, 1, 1))
        Dim Checked_ID As Long = Val(Mid(e.Node.Name, 3, 20))
        Dim Checked_FP As FP = Nothing

        RaiseEvent Node_CheckedChanged(Me, e, IndexOf_CheckedFP, Checked_FP, Checked_ID)

        For Each AktKey As String In DIC_NODES.Keys
            With DIC_NODES(AktKey)
                If Not (.Parent Is Nothing) Then
                    If .Parent.Equals(e.Node) Then
                        .Checked = e.Node.Checked
                    End If
                End If
            End With
        Next

        If e.Action = TreeViewAction.ByMouse Then
            TV_DO_AFTER_CHECK_AT_THE_END(e)
        End If
    End Sub

    Private Sub TV_DO_AFTER_CHECK_AT_THE_END(ByVal e As System.Windows.Forms.TreeViewEventArgs)
        T_Node = e.Node
        T_Checked = e.Node.Checked

        If Not T.Enabled Then
            T_FunctionsCode = Enum_Timer_Func_Codes.DO_AFTER_CHECK
            T.Interval = 20
            T.Enabled = True
        End If

        T_Do_AfterCheck = True
    End Sub

    Private Sub TV_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TV.AfterSelect
        If TV_Binded Then
            Dim CurrentLevel = TV_SELECTED_NODE_GET_LEVEL()
            Dim CurrentID = TV_SELECTED_NODE_GET_ID()

            Select Case e.Action
                Case TreeViewAction.ByKeyboard, TreeViewAction.ByMouse
                    Dim DoIt As Boolean = True

                    If DoIt Then
                        If CurrentLevel > -1 Then
                            TV_SELECT(TREEVIEW_get_Node_Key_from_Elements(CurrentLevel, CurrentID))
                            RaiseEvent Node_After_Select(Me, e)
                        End If
                    End If

                Case Else
                    TV_Binded = False
                    If Not (TV.SelectedNode Is Nothing) Then
                        TV_LastSelectedNode_Key = TV.SelectedNode.Name
                    End If
                    TV_Binded = True
            End Select
        End If
    End Sub
    Private Sub TV_BeforeSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles TV.BeforeSelect
        If TV_Binded Then
            If Not FPf.SAVE_ALL Then
                e.Cancel = True
            End If

            If Not e.Cancel Then
                RaiseEvent Node_Before_Select(Me, e)
            End If
        End If
    End Sub

    Private Sub NODE_COLORING_ALL()
        For Each NodeKey As String In DIC_NODES.Keys
            NODE_COLORING(NodeKey)
        Next
    End Sub

    Public Function TV_GET_LastSelectedNode() As TreeNode
        Dim OUT As TreeNode = Nothing

        If TV_LastSelectedNode_Key > "" Then
            If Not DIC_NODES.ContainsKey(TV_LastSelectedNode_Key) Then
                gl_FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_TreeView_Copy.TV_GET_LastSelectedNode", 0, String.Format("Node in TV_LastSelectedNode_Key ('{0}' is invalid.)", TV_LastSelectedNode_Key))
            Else
                OUT = DIC_NODES(TV_LastSelectedNode_Key)
            End If
        End If

        TV_GET_LastSelectedNode = OUT
    End Function

    Public Function TV_GOTO_PrevNode() As Boolean
        Dim OUT As Boolean = False

        Dim AktNode As TreeNode = TV_GET_LastSelectedNode()

        If Not AktNode Is Nothing Then
            If Not (AktNode.PrevVisibleNode Is Nothing) Then
                With AktNode.PrevVisibleNode
                    OUT = TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                End With
            End If
        End If

        TV_GOTO_PrevNode = OUT
    End Function

    Public Function TV_GOTO_PrevNode_On_This_Level() As Boolean
        Dim OUT As Boolean = False

        Dim AktNode As TreeNode = TV_GET_LastSelectedNode()

        If Not AktNode Is Nothing Then
            If Not (AktNode.PrevNode Is Nothing) Then
                With AktNode.PrevNode
                    OUT = TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                End With
            End If
        End If

        TV_GOTO_PrevNode_On_This_Level = OUT
    End Function

    Public Function TV_GOTO_NextNode() As Boolean
        Dim OUT As Boolean = False

        Dim AktNode As TreeNode = TV_GET_LastSelectedNode()

        If Not AktNode Is Nothing Then
            If Not (AktNode.NextVisibleNode Is Nothing) Then
                With AktNode.NextVisibleNode
                    OUT = TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                End With
            End If
        End If

        TV_GOTO_NextNode = OUT
    End Function

    Public Function TV_GOTO_NextNode_On_This_Level() As Boolean
        Dim OUT As Boolean = False

        Dim AktNode As TreeNode = TV_GET_LastSelectedNode()

        If Not AktNode Is Nothing Then
            If Not (AktNode.NextNode Is Nothing) Then
                With AktNode.NextNode
                    OUT = TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                End With
            End If
        End If

        TV_GOTO_NextNode_On_This_Level = OUT
    End Function

    Public Function TV_GOTO_Node(ByVal NodeKey As String) As Boolean
        Dim Level As Integer = Val(Mid(NodeKey, 1, 1))
        Dim ID As Long = Val(Mid(NodeKey, 3))

        Return TV_GOTO_Node(Level, ID)
    End Function

    Public Function TV_GOTO_Node(ByVal Level As Integer, ByVal ID As Long) As Boolean
        Dim OUT As Boolean = FPf.SAVE_ALL

        If OUT Then
            OUT = TV_SELECT(Level, ID)
        End If

        Return OUT
    End Function



    Public Function TV_GOTO_ROOT_FIRST() As Boolean
        Dim OUT As Boolean = False

        If TV.Nodes.Count > 0 Then
            Dim FirstRootKey As String = TV.Nodes(0).Name
            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(FirstRootKey), NODE_GET_ID_FROM_Key(FirstRootKey))
        End If

        TV_GOTO_ROOT_FIRST = OUT
    End Function

    Public Function TV_GOTO_ROOT_LAST() As Boolean
        Dim OUT As Boolean = False

        If TV.Nodes.Count > 0 Then
            Dim LastKey As String = TV.Nodes(TV.Nodes.Count - 1).Name
            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(LastKey), NODE_GET_ID_FROM_Key(LastKey))
        End If
    End Function

    Public Function TV_GOTO_LAST_NODE() As Boolean
        Dim OUT As Boolean = False

        If TV.Nodes.Count > 0 Then
            OUT = True
            Dim MyLastNode As TreeNode = TV.Nodes(TV.Nodes.Count - 1)

            While Not (MyLastNode.LastNode Is Nothing)
                MyLastNode.Expand()
                MyLastNode = MyLastNode.LastNode
            End While

            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(MyLastNode.Name), NODE_GET_ID_FROM_Key(MyLastNode.Name))
        End If
    End Function

    Private Sub TV_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TV.KeyDown
        Dim CtrlPressed As Boolean = My.Computer.Keyboard.CtrlKeyDown
        Dim AktNode As TreeNode = TV_SELECTED_NODE()

        Select Case e.KeyCode
            Case Keys.PageUp
                If CtrlPressed Then
                    TV_GOTO_ROOT_FIRST()
                Else
                    If Not (TV.SelectedNode Is Nothing) Then
                        If TV.SelectedNode.Parent Is Nothing Then
                            TV_GOTO_PrevNode_On_This_Level()
                        Else
                            If TV.SelectedNode.Parent.FirstNode.Equals(TV.SelectedNode) Then
                                If (TV.SelectedNode.Parent.PrevNode Is Nothing) Then
                                    With TV.SelectedNode.Parent
                                        TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                    End With
                                Else
                                    If TV.SelectedNode.Parent.PrevNode.FirstNode Is Nothing Then
                                        With TV.SelectedNode.Parent.PrevNode
                                            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                        End With
                                    Else
                                        With TV.SelectedNode.Parent.PrevNode.FirstNode
                                            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                        End With
                                    End If
                                End If
                            Else
                                With TV.SelectedNode.Parent.FirstNode
                                    TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                End With
                            End If
                        End If
                    End If
                End If
                e.Handled = True

            Case Keys.PageDown
                If CtrlPressed Then
                    TV_GOTO_LAST_NODE()
                Else
                    If Not (TV.SelectedNode Is Nothing) Then
                        If TV.SelectedNode.Parent Is Nothing Then
                            TV_GOTO_NextNode_On_This_Level()
                        Else
                            If TV.SelectedNode.Parent.LastNode.Equals(TV.SelectedNode) Then
                                If (TV.SelectedNode.Parent.NextNode Is Nothing) Then
                                    'nothing to do
                                Else
                                    If TV.SelectedNode.Parent.NextNode.FirstNode Is Nothing Then
                                        With TV.SelectedNode.Parent.NextNode
                                            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                        End With
                                    Else
                                        With TV.SelectedNode.Parent.NextNode.FirstNode
                                            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                        End With
                                    End If
                                End If
                            Else
                                With TV.SelectedNode.Parent.LastNode
                                    TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                End With
                            End If
                        End If
                    End If
                End If
                e.Handled = True

            Case Keys.Home
                If CtrlPressed Then
                    TV_GOTO_ROOT_FIRST()
                Else
                    If Not (TV.SelectedNode Is Nothing) Then
                        If TV.SelectedNode.Parent Is Nothing Then
                            TV_GOTO_ROOT_FIRST()
                        Else
                            Dim Key As String = TV.SelectedNode.Parent.FirstNode.Name

                            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(Key), NODE_GET_ID_FROM_Key(Key))
                        End If
                    End If
                End If
                e.Handled = True

            Case Keys.End
                If CtrlPressed Then
                    TV_GOTO_LAST_NODE()
                Else
                    If Not (TV.SelectedNode Is Nothing) Then
                        If TV.SelectedNode.Parent Is Nothing Then
                            TV_GOTO_ROOT_LAST()
                        Else
                            Dim Key As String = TV.SelectedNode.Parent.LastNode.Name

                            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(Key), NODE_GET_ID_FROM_Key(Key))
                        End If
                    End If
                End If
                e.Handled = True

            Case Keys.Right
                If Not (AktNode Is Nothing) Then
                    If Not (AktNode.FirstNode Is Nothing) Then
                        TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(AktNode.FirstNode.Name), NODE_GET_ID_FROM_Key(AktNode.FirstNode.Name))
                        e.Handled = True
                    End If
                End If

            Case Keys.Left
                If Not (AktNode Is Nothing) Then
                    If Not (AktNode.Parent Is Nothing) Then
                        Dim ParentNode As TreeNode = AktNode.Parent

                        If TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(ParentNode.Name), NODE_GET_ID_FROM_Key(ParentNode.Name)) Then
                            ParentNode.Collapse()
                        End If
                        e.Handled = True
                    End If
                End If

            Case Keys.Enter
                If Not (AktNode Is Nothing) Then
                    If Not (AktNode.FirstNode Is Nothing) Then
                        If AktNode.IsExpanded Then
                            AktNode.Collapse()
                        Else
                            AktNode.Expand()
                        End If
                    End If
                End If
        End Select
    End Sub

    Private Sub TV_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TV.KeyPress
        Dim IndexOf_FP As Integer = TV_SELECTED_NODE_GET_LEVEL()
        Dim CurrentID As Long = TV_SELECTED_NODE_GET_ID()

        RaiseEvent Node_KeyPress(Me, e, CurrentID)
    End Sub

    Private Sub T_CLEAR()
        T.Enabled = False
        T_GotoNode = ""
    End Sub

    Private Sub T_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles T.Tick
        T.Enabled = False

        If Not (FPf Is Nothing) Then
            If Not (FPf.Frm Is Nothing) Then
                Select Case T_FunctionsCode
                    Case Enum_Timer_Func_Codes.GOTO_NODE
                        'Nothing to do (mert a kovetkezo resz ugy is raallitja a node-ra a TV-t.
                End Select

                If T_GotoNode > "" Then
                    TV_GOTO_Node(T_GotoNode)
                End If

                If T_Do_AfterCheck Then
                    T_Do_AfterCheck = False
                    Application.DoEvents()
                    System.Threading.Thread.Sleep(200)
                    Application.DoEvents()
                    DIC_NODES(T_GotoNode).Checked = T_Checked
                End If
            End If
        End If

        T_CLEAR()
    End Sub
End Class


Public Class FP_L_TreeView_With_FP
    Public Structure Struct_SQL_Bind_PARAMS
        Dim TV_SOURCE_View_Name As String
    End Structure

    Private Enum Enum_Fill_Timer_Code As Integer
        FILL = 0
        REFRESH = 1
        DO_AFTER_CHECK = 2
        GOTO_NODE = 3
    End Enum

    Public Event Node_CheckedChanged(ByVal sender As FP_L_TreeView_With_FP, ByVal e As System.Windows.Forms.TreeViewEventArgs, ByVal IndexOfFP As Integer, ByVal FP As FP, ByVal RecordID As Long)
    Public Event Node_KeyPress(ByVal sender As FP_L_TreeView_With_FP, ByVal e As System.Windows.Forms.KeyPressEventArgs, ByVal IndexOfFP As Integer, ByVal FP As FP, ByVal RecordID As Long)
    Public Event Node_MouseWheel(ByVal sender As FP_L_TreeView_With_FP, ByVal e As System.Windows.Forms.MouseEventArgs, ByRef Handled As Boolean)
    Public Event Node_Delete(ByVal sender As FP_L_TreeView_With_FP, ByRef Handled As Boolean)
    Public Event Node_MoveUpDown(ByVal sender As FP_L_TreeView_With_FP, ByVal UpDown As ENUM_UpDown, ByRef Handled As Boolean)

    Public WithEvents Array_Of_FP As FP_L_Array_Of_FP

    Public WithEvents TV As TreeView
    Public WithEvents TV_Button_Up As PictureBox
    Public WithEvents TV_Button_Down As PictureBox
    Public WithEvents TV_Button_Del As PictureBox
    Public WithEvents TV_Button_AddNew As PictureBox

    Public FPApp As FP_App
    Public FPf As FP_Form
    Public ServerObject_Prefix As String
    Public SubWHERE As String = ""
    Public SubWHERE_Do_Not_Insert_Terminal_In_WHERE As Boolean
    Public COLOR_TV_NORMAL_BG As Color = Color.FromArgb(255, 255, 255)
    Public COLOR_TV_SELECTED_BG As Color = Color.FromArgb(200, 255, 200)
    Public COLOR_TV_FILTER_PASSED_BG As Color = Color.FromArgb(215, 228, 242)

    Public COLOR_TV_NORMAL As Color = Color.FromArgb(0, 0, 0)
    Public COLOR_TV_SELECTED As Color = Color.FromArgb(0, 0, 0)
    Public COLOR_TV_FILTER_PASSED As Color = Color.FromArgb(0, 0, 0)

    Public DT As New DataTable

    Private WithEvents Fill_Timer As New System.Windows.Forms.Timer
    Private Fill_Timer_FunctionsCode As Enum_Fill_Timer_Code = Enum_Fill_Timer_Code.FILL
    Private Fill_Timer_AllwaysUpdate As Boolean = False
    Private Fill_Timer_GotoNode As String = ""
    Private Fill_Timer_Node As TreeNode
    Private Fill_Timer_Checked As Boolean
    Private Fill_Timer_Do_After_Check As Boolean = False

    Public SQL_Bind_Params As Struct_SQL_Bind_PARAMS

    Dim DT_Chk_OLD_COUNT As Integer = 0
    Dim DT_Chk_OLD_ID_SUM As Long = -1
    Dim DT_Chk_OLD_TR_SUM As Long = -1

    Public DIC_NODES As New Dictionary(Of String, TreeNode)

    Private MultiRoot As Boolean = False
    Private TV_LastSelectedNode_Key As String = ""
    Private TV_Binded As Boolean = True

    Private Disposed As Boolean = False

    Sub New(ByVal P As Struct_FP_L_TreeView_With_FP_CONTROLS)
        FPf = P.FPf
        FPApp = FPf.FPApp
        Array_Of_FP = New FP_L_Array_Of_FP(FPApp)
        TV = P.TV
        MultiRoot = P.MultiRoot
        TV.HideSelection = False
        ServerObject_Prefix = P.ServerObject_Prefix
        SubWHERE = P.SubWHERE
        SubWHERE_Do_Not_Insert_Terminal_In_WHERE = P.SubWHERE_Do_Not_Insert_Terminal_In_WHERE
        SQL_Bind_Params_SET(P.SQL_Bind_Params)
        DT_CLEAR_Chk_SUMS()
    End Sub

    Private Sub SQL_Bind_Params_SET(MyP As Struct_SQL_Bind_PARAMS)
        SQL_Bind_Params = MyP

        With SQL_Bind_Params
            If nz(.TV_SOURCE_View_Name, "") = "" Then
                .TV_SOURCE_View_Name = ServerObject_Prefix + "_SOURCE"
            End If
        End With
    End Sub

    Private Sub TV_DO_AFTER_CHECK_AT_THE_END(ByVal e As System.Windows.Forms.TreeViewEventArgs)
        Fill_Timer_Node = e.Node
        Fill_Timer_Checked = e.Node.Checked

        If Not Fill_Timer.Enabled Then
            Fill_Timer_FunctionsCode = Enum_Fill_Timer_Code.DO_AFTER_CHECK
            Fill_Timer.Interval = 20
            Fill_Timer.Enabled = True
        End If

        Fill_Timer_Do_After_Check = True
    End Sub

    Public Sub TV_FILL_AT_THE_END(Optional ByVal GotoNode As String = "", Optional ByVal AllwaysUpdate As Boolean = False)
        Fill_Timer_FunctionsCode = Enum_Fill_Timer_Code.FILL
        If Fill_Timer_AllwaysUpdate = False Then
            Fill_Timer_AllwaysUpdate = AllwaysUpdate
        End If
        Fill_Timer_GotoNode = GotoNode
        Fill_Timer.Interval = 20
        Fill_Timer.Enabled = True
    End Sub

    Private Sub Fill_Timer_CLEAR()
        Fill_Timer.Enabled = False
        Fill_Timer_AllwaysUpdate = False
        Fill_Timer_GotoNode = ""
    End Sub

    Public Sub TV_REFRESH_AT_THE_END(Optional ByVal GotoNode As String = "", Optional ByVal AllwaysUpdate As Boolean = False)
        If Fill_Timer.Enabled Then
            If Fill_Timer_FunctionsCode <> Enum_Fill_Timer_Code.FILL Then
                Fill_Timer_FunctionsCode = Enum_Fill_Timer_Code.REFRESH
            End If
        Else
            Fill_Timer_FunctionsCode = Enum_Fill_Timer_Code.REFRESH
        End If

        If Fill_Timer_AllwaysUpdate = False Then
            Fill_Timer_AllwaysUpdate = AllwaysUpdate
        End If

        Fill_Timer_GotoNode = GotoNode

        Fill_Timer.Interval = 20
        Fill_Timer.Enabled = True
    End Sub

    Public Sub Dispose()
        Fill_Timer = Nothing

        If Not (Array_Of_FP Is Nothing) Then
            Array_Of_FP.Dispose()
            Array_Of_FP = Nothing
        End If

        TV = Nothing

        TV_Button_Up = Nothing
        TV_Button_Down = Nothing
        TV_Button_Del = Nothing
        TV_Button_AddNew = Nothing

        DIC_NODES.Clear()
        DT.Rows.Clear()

        Disposed = True
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub

    Function ADD_FP(ByVal MyFP As FP) As Integer
        ADD_FP = Array_Of_FP.ADD_FP(MyFP)
    End Function

    Private Sub Array_Of_FP_ARRAY_OF_FP_Data_Records_Loaded(ByVal Index_Of_FP As Integer) Handles Array_Of_FP.ARRAY_OF_FP_Data_Records_Loaded
        If TV_Binded Then
            TV_Binded = False
            TV_FILL_AT_THE_END()
            TV_Binded = True
        End If
    End Sub

    Public Function get_Node_Key_from_Elements(ByVal Level As Integer, ByVal ID As Long) As String
        get_Node_Key_from_Elements = String.Format("{0}_{1}", Level, ID)
    End Function

    Private Function NODE_get_ParamLine_From_DT(ByVal NodeKey As String) As DataRow
        Dim OUT As DataRow = Nothing

        If DT Is Nothing Then
            FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_TreeView_With_FP.NODE_get_ParamLine_From_DT", 0, "DT is nothing")
        Else
            Dim Level As Integer = Val(Mid(NodeKey, 1, 1))
            Dim ID As Long = Val(Mid(NodeKey, 3, 20))
            Dim Criteria As String = String.Format("Level = {0} And ID = {1}", Level, ID)

            If DT.Select(Criteria).Count <> 1 Then
                FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_TreeView_With_FP.NODE_get_ParamLine_From_DT", 0, String.Format("Node '{0}' in DT not found.", NodeKey))
            Else
                OUT = DT.Select(Criteria).First
            End If
        End If

        NODE_get_ParamLine_From_DT = OUT
    End Function

    Private Sub NODE_COLORING(ByVal NodeKey As String)
        If Not DIC_NODES.ContainsKey(NodeKey) Then
            FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_TreeView_With_FP.NODE_COLORING", 0, String.Format("DIC_NODES does not contains node '{0}'", NodeKey))
        Else
            With DIC_NODES(NodeKey)
                If TV_LastSelectedNode_Key = NodeKey Then
                    'DIC_NODES(NodeKey).BackColor = COLOR_TV_SELECTED_BG
                Else
                    Dim Props As DataRow = NODE_get_ParamLine_From_DT(NodeKey)

                    If Not (Props Is Nothing) Then
                        Dim GridFiltered As Boolean = False
                        Dim CurrentFP As FP = Array_Of_FP.DIC_FPs(Props!level).FP
                        Dim CurrentNormalBackColor As Color = IIf(Props!Text_BackColor = -1, COLOR_TV_NORMAL_BG, Color.FromArgb(Props!Text_BackColor))
                        Dim CurrentNormalForeColor As Color = IIf(Props!Text_ForeColor = -1, COLOR_TV_NORMAL, Color.FromArgb(Props!Text_ForeColor))

                        If Not (Array_Of_FP.DIC_FPs(Props!level).FP Is Nothing) Then
                            If CurrentFP.GRID_EXISTS Then
                                GridFiltered = CurrentFP.GRID.P_FilterActive
                            End If
                        End If

                        .ImageKey = Props.Item("Image")
                        .SelectedImageKey = Props.Item("Image_Selected")

                        If GridFiltered Then
                            Dim Criteria As String = String.Format("RecordID = {0}", Props!ID)

                            If CurrentFP.GRID.DT.Select(Criteria).Count > 0 Then
                                .BackColor = COLOR_TV_FILTER_PASSED_BG
                                .ForeColor = COLOR_TV_FILTER_PASSED_BG
                            Else
                                .BackColor = CurrentNormalBackColor
                                .ForeColor = CurrentNormalForeColor
                            End If
                        Else
                            .BackColor = CurrentNormalBackColor
                            .ForeColor = CurrentNormalForeColor
                        End If
                    End If
                End If
            End With
        End If
    End Sub

    Public Function TV_EXPANDED_STATE_GET() As List(Of String)
        Dim Lst_Not_Expanded As New List(Of String)

        For Each Key As String In DIC_NODES.Keys
            With DIC_NODES(Key)
                If .IsExpanded = False Then
                    Lst_Not_Expanded.Add(Key)
                End If
            End With
        Next

        Return Lst_Not_Expanded
    End Function

    Public Sub TV_EXPANDED_STATE_SET(ByVal Lst_Not_Expanded As List(Of String))
        TV.ExpandAll()

        For Each Key As String In Lst_Not_Expanded
            If DIC_NODES.Keys.Contains(Key) Then
                If DIC_NODES(Key).IsExpanded Then
                    DIC_NODES(Key).Collapse()
                End If
            End If
        Next
    End Sub

    Public Sub TV_DoEvents()
        If TV_Binded = False Then
            FPApp.DoErrorMsgBox("FP_L_TreeView_With_FP.TV_DoEvents", 0, "TV_Binded = False. Igy nem lehet vegrehajtani az eloirt Event-eket a TV-n.")
        Else
            If Fill_Timer.Enabled Then
                Dim ee As New System.EventArgs

                Fill_Timer_Tick(TV, ee)
            End If
        End If
    End Sub


    Private Sub TV_NODE_REMOVE(ByVal Key As String)
        Dim Nodes_To_Remove As New List(Of String)

        TV_NODE_GetAllChildren(DIC_NODES(Key), Nodes_To_Remove)

        Nodes_To_Remove.Sort()

        For i As Integer = Nodes_To_Remove.Count - 1 To 0
            DIC_NODES(Nodes_To_Remove(i)).Remove()
            DIC_NODES.Remove(Nodes_To_Remove(i))
        Next
    End Sub

    Function TV_FILL_IMMEDIATELY(Optional ByVal AllwaysUpdate As Boolean = False) As Boolean
        Dim OUT As Boolean = False

        Dim DataChanged As Boolean = False

        If DATA_LOAD_TV(DataChanged, AllwaysUpdate) Then
            If DataChanged Or AllwaysUpdate Then
                Dim Lst_Not_Expanded As List(Of String) = TV_EXPANDED_STATE_GET()
                Dim TV_Binded_OLD As Boolean = TV_Binded

                TV_Binded = False
                TV_LastSelectedNode_Key = ""
                TV_UNSELECT()

                'Mar nem letezo node-ok torlese es meglevok frissitese...

                Dim ListOfKeys As List(Of String) = DIC_NODES.Keys.ToList

                ListOfKeys.Sort()

                Dim TempDIC As New Dictionary(Of String, DataRow)

                For Each Row As DataRow In DT.Rows
                    TempDIC.Add(String.Format("{0}_{1}", Row!Level, Row!ID), Row)
                Next

                Dim i As Integer = ListOfKeys.Count - 1

                Do While i >= 0
                    Dim AktLevel As Integer = Val(Mid(ListOfKeys(i), 1, 1))
                    Dim AktID = Val(Mid(ListOfKeys(i), 3))
                    Dim Criteria As String = String.Format("[Level] = {0} And ID = {1}", AktLevel, AktID)
                    Dim DoDel As Boolean = False
                    Dim CurrentRow As DataRow = Nothing
                    Dim CurrentNode As TreeNode = Nothing
                    Dim CurrentNode_Key As String = ""
                    Dim ParentNodeKey As String = ""

                    If Not TempDIC.ContainsKey(Criteria) Then
                        DoDel = True
                    Else
                        CurrentRow = TempDIC(Criteria)
                        CurrentNode = DIC_NODES(ListOfKeys(i))
                        CurrentNode_Key = ListOfKeys(i)
                        ParentNodeKey = ""

                        If CurrentRow![Level] > 0 Then
                            ParentNodeKey = get_Node_Key_from_Elements(CurrentRow![Level] - 1, CurrentRow!ParentID)
                            If CurrentNode.Parent.Name <> ParentNodeKey Then
                                DoDel = True
                            End If
                        End If
                    End If

                    If DoDel Then
                        DIC_NODES(ListOfKeys(i)).Remove()
                        DIC_NODES.Remove(ListOfKeys(i))
                    Else
                        CurrentNode.Text = IIf(CurrentRow!Text = String.Empty, "n/a", CurrentRow!Text)
                        CurrentNode.ImageKey = nz(CurrentRow!Image, "")
                        CurrentNode.SelectedImageKey = nz(CurrentRow!Image_Selected, "")
                        NODE_COLORING(CurrentNode_Key)
                    End If

                    i -= 1
                Loop

                'Uj node-k beszurasa...
                Dim Node_key As String = ""
                Dim Node_text As String = ""
                Dim Node_ImageIndex As String = ""
                Dim Node_ParentNodeKey As String = ""
                Dim Node_Underline As Boolean = False

                Dim LastIndexes(10) As Integer
                Dim LastParents(10) As TreeNode

                For ii As Integer = 0 To UBound(LastIndexes)
                    LastIndexes(ii) = -1
                Next

                For Each CurrentRow In DT.Rows
                    With CurrentRow
                        Node_key = get_Node_Key_from_Elements(!Level, !ID)
                        If !Level = 0 Then
                            Node_ParentNodeKey = ""
                        Else
                            Node_ParentNodeKey = String.Format("{0}_{1}", !Level - 1, !ParentID)
                        End If

                        If DIC_NODES.Keys.Contains(Node_key) Then
                            If !Level > 0 Then
                                Dim NewParent As TreeNode = DIC_NODES(Node_key).Parent
                                Dim OldParent As TreeNode = LastParents(!Level)

                                If Not NewParent.Equals(OldParent) Then
                                    LastParents(!Level) = NewParent
                                    LastIndexes(!Level) = -1
                                End If
                            End If

                            If DIC_NODES(Node_key).Index <> LastIndexes(!Level) + 1 Then
                                TV_NODE_MOVE(DIC_NODES(Node_key), DIC_NODES(Node_key).Parent, LastIndexes(!Level) + 1, DIC_NODES)
                                'Dim ParentNode As TreeNode = DIC_NODES(Node_key).Parent

                                'ParentNode.Nodes.Remove(DIC_NODES(Node_key))
                                'ParentNode.Nodes.Insert(LastIndexes(!Level), Node_key, !Text, !Image, !Image)
                                'DIC_NODES(Node_key) = ParentNode.Nodes(Node_key)
                            End If
                            LastIndexes(!Level) += 1
                            NODE_COLORING(Node_key)
                        Else
                            Node_text = IIf(!Text = String.Empty, "n/a", !Text)
                            Node_ImageIndex = !Image
                            Node_Underline = CBool(!Text_Underline)

                            If !Level = 0 Then
                                TV.Nodes.Insert(LastIndexes(0) + 1, Node_key, Node_text, Node_ImageIndex, Node_ImageIndex)
                                DIC_NODES.Add(Node_key, TV.Nodes(Node_key))
                            Else
                                DIC_NODES(Node_ParentNodeKey).Nodes.Insert(LastIndexes(!Level) + 1, Node_key, Node_text, Node_ImageIndex, Node_ImageIndex)
                                DIC_NODES.Add(Node_key, DIC_NODES(Node_ParentNodeKey).Nodes(Node_key))
                            End If
                            NODE_SET_UNDERLINE(DIC_NODES(Node_key), Node_Underline)

                            LastIndexes(!Level) += 1
                            NODE_COLORING(Node_key)
                        End If
                    End With
                Next

                If (Array_Of_FP.DIC_FPs(0).FP Is Nothing) Then
                    TV_UNSELECT()
                Else
                    With Array_Of_FP.DIC_FPs(0).FP
                        If .P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                            'Dim TV_Binded_Old As Boolean = TV_Binded

                            'TV_Binded = False
                            Dim DoSilent As Boolean = (Not AllwaysUpdate)
                            TV_SELECT(0, .P_DATA_Current_ID, DoSilent) 'ML_20160829
                            'TV_Binded = TV_Binded_Old
                        Else
                            TV_UNSELECT()
                        End If
                    End With
                End If

                TV_Binded = TV_Binded_OLD
                TV_EXPANDED_STATE_SET(Lst_Not_Expanded)
            End If
            OUT = True
        End If

        TV_FILL_IMMEDIATELY = OUT
    End Function

    Public Sub TV_GOTO_Node_AT_THE_END(ByVal Level As Integer, ByVal ID As Long)
        TV_GOTO_Node_AT_THE_END(get_Node_Key_from_Elements(Level, ID))
    End Sub

    Public Sub TV_GOTO_Node_AT_THE_END(ByVal GotoNode As String)
        If Not Fill_Timer.Enabled Then
            Fill_Timer_FunctionsCode = Enum_Fill_Timer_Code.GOTO_NODE
        End If

        Fill_Timer_GotoNode = GotoNode

        Fill_Timer.Interval = 20
        Fill_Timer.Enabled = True
    End Sub

    Private Sub NODE_SET_UNDERLINE(MyNode As TreeNode, Let_Underline As Boolean)
        Dim Curr_UnderLine As Boolean = False

        If Not (MyNode Is Nothing) Then
            With MyNode
                If Not (.NodeFont Is Nothing) Then
                    Curr_UnderLine = .NodeFont.Underline
                End If
                If Curr_UnderLine <> Let_Underline Then
                    Dim MyFont As Font = New Font(TV.Font, IIf(Let_Underline = False, FontStyle.Regular, FontStyle.Underline))
                    .NodeFont = MyFont
                End If
            End With
        End If
    End Sub

    Sub TV_REFRESH_IMMEDIATELY(Optional ByVal AllwaysUpdate As Boolean = False)
        If Fill_Timer.Enabled Then
            FPApp.DoErrorMsgBox("SEL_FP..FP_FieldLogics.TV_REFRESH_IMMEDIATELY", 0, "Fill Timer aktiv. Ha ez igy jo, akkor a kiadott utasitas elott hivd meg a 'FP_L_TreeView_With_FP.TV_DoEvents' eljarast es ezutan hivd meg a node szelekciora vonatkozo utasitast.")
        Else
            Dim DataChanged As Boolean = False

            If DATA_LOAD_TV(DataChanged, AllwaysUpdate) Then
                If DataChanged Or AllwaysUpdate Then
                    Dim Level As Long = 0
                    Dim Node_key As String = String.Empty
                    Dim Node_text As String = String.Empty
                    Dim Node_ParentNodeKey As String = String.Empty

                    For Each CurrentRow In DT.Rows
                        With CurrentRow
                            Level = !Level
                            Node_key = get_Node_Key_from_Elements(Level, !ID)
                            If Level > 0 Then
                                Node_ParentNodeKey = get_Node_Key_from_Elements(Level - 1, !ParentID)
                            Else
                                Node_ParentNodeKey = ""
                            End If
                            Node_text = IIf(!Text = String.Empty, "n/a", !Text)

                            If Node_ParentNodeKey = "" Then
                                If TV.Nodes.ContainsKey(Node_key) Then
                                    With TV.Nodes(Node_key)
                                        If .Text <> Node_text Then
                                            .Text = Node_text
                                        End If
                                        If .ImageKey <> CurrentRow!Image Then
                                            .ImageKey = CurrentRow!Image
                                        End If
                                        If CurrentRow!Text_BackColor <> -1 Then
                                            If .BackColor.Name <> Color.FromArgb(CurrentRow!Text_BackColor).Name Then
                                                .BackColor = Color.FromArgb(CurrentRow!Text_BackColor)
                                            End If
                                        Else
                                            If .BackColor.Name <> COLOR_TV_NORMAL_BG.Name Then
                                                .BackColor = COLOR_TV_NORMAL_BG
                                            End If
                                        End If
                                        If CurrentRow!Text_ForeColor <> -1 Then
                                            If .ForeColor.Name <> Color.FromArgb(CurrentRow!Text_ForeColor).Name Then
                                                .ForeColor = Color.FromArgb(CurrentRow!Text_ForeColor)
                                            End If
                                        Else
                                            If .ForeColor.Name <> COLOR_TV_NORMAL.Name Then
                                                .ForeColor = COLOR_TV_NORMAL
                                            End If
                                        End If

                                        NODE_SET_UNDERLINE(TV.Nodes(Node_key), CBool(CurrentRow!Text_Underline))
                                    End With
                                End If
                            Else
                                If TV.Nodes.ContainsKey(Node_ParentNodeKey) Then
                                    If TV.Nodes(Node_ParentNodeKey).Nodes.ContainsKey(Node_key) Then
                                        With TV.Nodes(Node_ParentNodeKey).Nodes(Node_key)
                                            If .Text <> Node_text Then
                                                .Text = Node_text
                                            End If
                                            If .ImageKey <> CurrentRow!Image Then
                                                .ImageKey = CurrentRow!Image
                                            End If
                                            If .SelectedImageKey <> CurrentRow!Image_Selected Then
                                                .SelectedImageKey = CurrentRow!Image_Selected
                                            End If
                                            If CurrentRow!Text_BackColor <> -1 Then
                                                If .BackColor.Name <> Color.FromArgb(CurrentRow!Text_BackColor).Name Then
                                                    .BackColor = Color.FromArgb(CurrentRow!Text_BackColor)
                                                End If
                                            Else
                                                If .BackColor.Name <> COLOR_TV_NORMAL_BG.Name Then
                                                    .BackColor = COLOR_TV_NORMAL_BG
                                                End If
                                            End If
                                            If CurrentRow!Text_ForeColor <> -1 Then
                                                If .ForeColor.Name <> Color.FromArgb(CurrentRow!Text_ForeColor).Name Then
                                                    .ForeColor = Color.FromArgb(CurrentRow!Text_ForeColor)
                                                End If
                                            Else
                                                If .ForeColor.Name <> COLOR_TV_NORMAL.Name Then
                                                    .ForeColor = COLOR_TV_NORMAL
                                                End If
                                            End If

                                            NODE_SET_UNDERLINE(TV.Nodes(Node_ParentNodeKey).Nodes(Node_key), CBool(CurrentRow!Text_Underline))
                                        End With
                                    End If
                                End If
                            End If
                        End With
                    Next
                End If
            End If
        End If
    End Sub

    Private Sub DT_CLEAR_Chk_SUMS()
        DT_Chk_OLD_COUNT = 0
        DT_Chk_OLD_ID_SUM = -1
        DT_Chk_OLD_TR_SUM = -1
    End Sub

    Private Function DATA_LOAD_TV(ByRef DataChanged As Boolean, Optional ByVal AllwaysUpdate As Boolean = False) As Boolean
        Dim OUT As Boolean = False
        Dim SQL As String
        Dim SQL_SELECT As String
        Dim SQL_FROM As String
        Dim SQL_WHERE As String
        Dim SQL_ORDERBY As String

        Dim Chk_NEW_COUNT As Integer
        Dim Chk_NEW_ID_SUM As Long = -2
        Dim Chk_NEW_TR_SUM As Long = -2

        Dim DT_New As New DataTable

        If MultiRoot Then
            SQL_SELECT = "ISNULL(Row.[Level], 0) [Level], ISNULL(Row.ID, 0) ID, ISNULL(Row.TransactID, 0) TransactID, ISNULL(Row.Root_RecordID, 0) Root_RecordID, ISNULL(Row.ParentID, 0) ParentID, ISNULL(Row.SeqNum, 0) SeqNum, ISNULL(Row.[Text], '') [Text], ISNULL(Row.[Image], '') [Image], ISNULL(Row.Image_Selected, '') Image_Selected, ISNULL(Row.Text_ForeColor, 0) Text_ForeColor, ISNULL(Row.Text_BackColor, 0) Text_BackColor, ISNULL(Row.Text_Underline, 0) Text_Underline"
            If SubWHERE_Do_Not_Insert_Terminal_In_WHERE Then
                SQL_WHERE = SubWHERE
                SQL_FROM = String.Format("{0} Row	LEFT JOIN {0} Parents ON Row.ParentID = Parents.ID", SQL_Bind_Params.TV_SOURCE_View_Name)
                If Array_Of_FP.DIC_FPs(0).FP IsNot Nothing Then
                    If FPf.FPs(1).ButtonFilterLine IsNot Nothing Then
                        If FPf.FPs(1).PICTUREBOXES.ContainsKey("Btn_Filter_Line") Then
                            Dim Btn As FP_PictureBox = FPf.FPs(1).PICTUREBOXES("Btn_Filter_Line")
                            If Btn.P_PRESSED Then
                                SQL_WHERE = String.Format("ID IN ({0},{1})", Array_Of_FP.DIC_FPs(0).FP.P_DATA_Current_ID, FPf.FPs(1).Sub_IDS(FPf.FPs(2)))
                            End If
                        End If
                    End If
                End If
            Else
                SQL_WHERE = TEXT_AND(String.Format("Row.Terminal = '{0}'", Terminal), SubWHERE)
                SQL_FROM = String.Format("{0} Row	LEFT JOIN {0} Parents ON Row.ParentID = Parents.ID And Row.Terminal = Parents.Terminal", SQL_Bind_Params.TV_SOURCE_View_Name)
                If Array_Of_FP.DIC_FPs(0).FP IsNot Nothing Then
                    If FPf.FPs(1).ButtonFilterLine IsNot Nothing Then
                        If FPf.FPs(1).PICTUREBOXES.ContainsKey("Btn_Filter_Line") Then
                            Dim Btn As FP_PictureBox = FPf.FPs(1).PICTUREBOXES("Btn_Filter_Line")
                            If Btn.P_PRESSED Then
                                SQL_WHERE = String.Format("({0}) AND Row.ID IN ({1},{2})", SQL_WHERE, Array_Of_FP.DIC_FPs(0).FP.P_DATA_Current_ID, FPf.FPs(1).Sub_IDS(FPf.FPs(2)))
                            End If
                        End If
                    End If
                End If
            End If
            SQL_ORDERBY = "Row.[Level], RIGHT(SPACE(15) + ISNULL(CONVERT(NVARCHAR(15), Parents.SeqNum), ''), 15) + RIGHT(SPACE(15) + CONVERT(NVARCHAR(15), Row.SeqNum), 15)"
        Else
            SQL_SELECT = "ISNULL(Level, 0) Level, ISNULL(ID, 0) ID, ISNULL(TransactID, 0) TransactID, ISNULL(Root_RecordID, 0) Root_RecordID, ISNULL(ParentID, 0) ParentID, ISNULL(SeqNum, 0) SeqNum, ISNULL(Text, '') Text, ISNULL(Image, '') Image, ISNULL(Image_Selected, '') Image_Selected, ISNULL(Text_ForeColor, 0) Text_ForeColor, ISNULL(Text_BackColor, 0) Text_BackColor, ISNULL(Text_Underline, 0) Text_Underline"
            SQL_WHERE = TEXT_AND(String.Format("Root_RecordID = {0}", Array_Of_FP.DIC_FPs(0).FP.P_DATA_Current_ID), SubWHERE)
            SQL_FROM = String.Format("{0}", SQL_Bind_Params.TV_SOURCE_View_Name)
            SQL_ORDERBY = "[Level], SeqNum"
            If FPf.FPs(1).ButtonFilterLine IsNot Nothing Then
                If FPf.FPs(1).PICTUREBOXES.ContainsKey("Btn_Filter_Line") Then
                    Dim Btn As FP_PictureBox = FPf.FPs(1).PICTUREBOXES("Btn_Filter_Line")
                    If Btn.P_PRESSED Then
                        SQL_WHERE = String.Format("ID IN ({0},{1})", Array_Of_FP.DIC_FPs(0).FP.P_DATA_Current_ID, FPf.FPs(1).Sub_IDS(FPf.FPs(2)))
                    End If
                End If
            End If
        End If

        If SQL_WHERE > "" Then
            SQL_WHERE = " WHERE " + SQL_WHERE
        End If

        If SQL_ORDERBY > "" Then
            SQL_ORDERBY = " ORDER BY " + SQL_ORDERBY
        End If

        SQL = String.Format("SELECT {0} FROM {1} {2} {3}", SQL_SELECT, SQL_FROM, SQL_WHERE, SQL_ORDERBY)

        DT_New.Clear()

        Try
            FPApp.DC.Qdf_Fill_DT(SQL, DT_New)

            Chk_NEW_COUNT = DT_New.Rows.Count
            If Chk_NEW_COUNT > 0 Then
                Chk_NEW_ID_SUM = DT_New.Compute("SUM(ID)", "")
                Chk_NEW_TR_SUM = DT_New.Compute("SUM(TransactID)", "")
            End If

            If DT_Chk_OLD_COUNT <> Chk_NEW_COUNT _
               Or DT_Chk_OLD_ID_SUM <> Chk_NEW_ID_SUM _
               Or DT_Chk_OLD_TR_SUM <> Chk_NEW_TR_SUM Then
                DT = DT_New.Copy
                DT_Chk_OLD_COUNT = Chk_NEW_COUNT
                DT_Chk_OLD_ID_SUM = Chk_NEW_ID_SUM
                DT_Chk_OLD_TR_SUM = Chk_NEW_TR_SUM
                DataChanged = True
            Else
                If AllwaysUpdate Then
                    DT = DT_New.Copy
                End If
            End If
            OUT = True

        Catch ex As Exception
            FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_TreeView_With_FP.DATA_LOAD_TV", Err.Number, Err.Description)
        End Try

        DATA_LOAD_TV = OUT
    End Function

    Sub TV_UNSELECT()
        If TV_LastSelectedNode_Key > "" Then
            Dim Old_Key As String = TV_LastSelectedNode_Key

            TV.SelectedNode = Nothing
            TV_LastSelectedNode_Key = ""
            If Not DIC_NODES.ContainsKey(Old_Key) Then
                FPApp.DoErrorMsgBox("FP_L_TreeView_With_FP.TV_UNSELECT", 0, String.Format("TV_LastSelectedNode_Key in DIC_NODES not found. ('{0}')", Old_Key))
            Else
                NODE_COLORING(Old_Key)
            End If
        End If
    End Sub

    Function TV_SELECT(ByVal NodeKey As String) As Boolean
        Dim Level As Integer = NODE_GET_LEVEL_FROM_Key(NodeKey)
        Dim ID As Integer = NODE_GET_ID_FROM_Key(NodeKey)

        Return TV_SELECT(Level, ID)
    End Function

    Function TV_SELECT(ByVal Level As Integer, ByVal ID As Long, Optional Silent As Boolean = False) As Boolean
        Dim OUT As Boolean = False
        Dim Key As String = get_Node_Key_from_Elements(Level, ID)

        If Key = TV_LastSelectedNode_Key Then
            OUT = True
        Else
            TV_UNSELECT()
            If Not DIC_NODES.ContainsKey(Key) Then
                FPApp.DoErrorMsgBox("FP_L_TreeView_With_FP.TV_SELECT", 0, String.Format("Node not found. (Key: '{0}'", Key))
            Else
                TV_LastSelectedNode_Key = Key
                If Silent = False Then
                    Array_Of_FP.ARRAY_GOTO_NORECORD(Level + 1)
                End If
                TV_LastSelectedNode_Key = Key 'Mert az elozo utasitas kinullazza a TV_LastSelectedNode_Key-t

                TV.SelectedNode = DIC_NODES(Key)

                NODE_COLORING(Key)

                OUT = True
            End If
        End If

        If OUT = True Then
            If DIC_NODES(Key).IsVisible = False Then
                DIC_NODES(Key).EnsureVisible()
            End If
        End If

        Return OUT
    End Function

    Public Function NODE_GET_LEVEL_FROM_Key(ByVal MyKey As String) As Integer
        Return Val(Mid(MyKey, 1, 1))
    End Function

    Public Function NODE_GET_ID_FROM_Key(ByVal MyKey As String) As Long
        Return Val(Mid(MyKey, 3))
    End Function

    Public Function TV_SELECTED_NODE_GET_LEVEL() As Integer
        Dim OUT As Integer = -1

        If Not TV Is Nothing Then
            If Not TV.SelectedNode Is Nothing Then
                OUT = NODE_GET_LEVEL_FROM_Key(TV.SelectedNode.Name)
            End If
        End If

        Return OUT
    End Function

    Public Function NODE_GET_ROOT_Node(ByVal MyNode As TreeNode) As TreeNode
        Dim OUT As TreeNode = MyNode

        If Not (OUT Is Nothing) Then
            Do While Not (OUT.Parent Is Nothing)
                OUT = OUT.Parent
            Loop
        End If

        Return OUT
    End Function

    Public Function TV_SELECTED_NODE_GET_ROOT_ID() As Long
        Dim OUT As Long = 0

        If Not TV Is Nothing Then
            If Not TV.SelectedNode Is Nothing Then
                Dim wNode As TreeNode = TV.SelectedNode

                Do While Not (wNode.Parent Is Nothing)
                    wNode = wNode.Parent
                Loop

                OUT = NODE_GET_ID_FROM_Key(wNode.Name)
            End If
        End If

        Return OUT
    End Function

    Public Function TV_SELECTED_NODE_GET_ID() As Long
        Dim OUT As Long = 0

        If Not TV Is Nothing Then
            If Not TV.SelectedNode Is Nothing Then
                OUT = NODE_GET_ID_FROM_Key(TV.SelectedNode.Name)
            End If
        End If

        Return OUT
    End Function

    Public Sub TV_SELECTED_NODE_EnsureVisible()
        Dim CurrNode As TreeNode = TV_SELECTED_NODE()

        If Not (CurrNode Is Nothing) Then
            CurrNode.EnsureVisible()
        End If
    End Sub

    Public Function TV_SELECTED_NODE() As TreeNode
        Dim OUT As TreeNode = Nothing

        If TV_LastSelectedNode_Key > "" Then
            If Not DIC_NODES.ContainsKey(TV_LastSelectedNode_Key) Then
                FPApp.DoErrorMsgBox("", 0, String.Format("TV_LastSelectedNode_Key ('{0}') is invalid.", TV_LastSelectedNode_Key))
            Else
                OUT = DIC_NODES(TV_LastSelectedNode_Key)
            End If
        End If

        Return OUT
    End Function

    Public Function TV_SELECTED_NODE_GET_FP() As FP
        Dim OUT As FP = Nothing
        Dim CurrentLevel = TV_SELECTED_NODE_GET_LEVEL()

        If CurrentLevel >= 0 Then
            OUT = Array_Of_FP.DIC_FPs(CurrentLevel).FP
        End If

        Return OUT
    End Function

    Private Sub TV_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TV.AfterCheck
        Dim IndexOf_CheckedFP As Integer = Val(Mid(e.Node.Name, 1, 1))
        Dim Checked_ID As Long = Val(Mid(e.Node.Name, 3, 20))
        Dim Checked_FP As FP = Nothing

        If Array_Of_FP.DIC_FPs.ContainsKey(IndexOf_CheckedFP) Then
            Checked_FP = Array_Of_FP.DIC_FPs(IndexOf_CheckedFP).FP
        End If
        RaiseEvent Node_CheckedChanged(Me, e, IndexOf_CheckedFP, Checked_FP, Checked_ID)

        For Each AktKey As String In DIC_NODES.Keys
            With DIC_NODES(AktKey)
                If Not (.Parent Is Nothing) Then
                    If .Parent.Equals(e.Node) Then
                        .Checked = e.Node.Checked
                    End If
                End If
            End With
        Next

        If e.Action = TreeViewAction.ByMouse Then
            TV_DO_AFTER_CHECK_AT_THE_END(e)
        End If
    End Sub

    Private Sub TV_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TV.AfterSelect
        If TV_Binded Then
            Dim CurrentFP As FP = TV_SELECTED_NODE_GET_FP()
            Dim CurrentLevel = TV_SELECTED_NODE_GET_LEVEL()
            Dim CurrentID = TV_SELECTED_NODE_GET_ID()

            Select Case e.Action
                Case TreeViewAction.ByKeyboard, TreeViewAction.ByMouse
                    Dim DoIt As Boolean = True

                    If Not (CurrentFP Is Nothing) Then
                        If Not CurrentFP.FORM_GOTO_RECORD_BY_ID(CurrentID) Then
                            Dim Found As Boolean = False

                            If CurrentFP.GRID_EXISTS Then
                                CurrentFP.GRID.P_FilterActive = False
                                Found = CurrentFP.FORM_GOTO_RECORD_BY_ID(CurrentID)
                            End If

                            If Found = False Then
                                If CurrentLevel > 0 Then
                                    Dim ParentIDs(CurrentLevel - 1) As Long
                                    Dim ParentNode As TreeNode = TV.SelectedNode.Parent

                                    For i As Integer = CurrentLevel - 1 To 0 Step -1
                                        ParentIDs(i) = NODE_GET_ID_FROM_Key(ParentNode.Name)
                                        If ParentIDs(i) = 0 Then
                                            DoIt = False
                                            Exit For
                                        End If

                                        If i > 0 Then
                                            ParentNode = ParentNode.Parent
                                        End If
                                    Next

                                    If DoIt Then
                                        For i As Integer = 0 To CurrentLevel - 1
                                            DoIt = Array_Of_FP.DIC_FPs(i).FP.FORM_GOTO_RECORD_BY_ID(ParentIDs(i))
                                            If DoIt = False Then
                                                Exit For
                                            End If
                                        Next
                                        If DoIt Then
                                            Found = CurrentFP.FORM_GOTO_RECORD_BY_ID(CurrentID)
                                        End If
                                    End If
                                End If
                            End If

                            If Found = False Then
                                DoIt = False
                                TV_UNSELECT()
                            End If
                        End If
                    End If

                    If DoIt Then
                        If CurrentLevel > -1 Then
                            TV_SELECT(get_Node_Key_from_Elements(CurrentLevel, CurrentID))
                        End If
                    End If

                Case Else
                    If CurrentFP Is Nothing Then
                        Dim TV_Binded_Old As Boolean = TV_Binded
                        TV_Binded = False
                        Array_Of_FP.ARRAY_GOTO_NORECORD(CurrentLevel + 1)
                        If Not (TV.SelectedNode Is Nothing) Then
                            TV_LastSelectedNode_Key = TV.SelectedNode.Name
                        End If
                        TV_Binded = TV_Binded_Old
                    Else
                        TV_LastSelectedNode_Key = get_Node_Key_from_Elements(CurrentLevel, CurrentID)
                    End If
            End Select
        End If
    End Sub
    Private Sub TV_BeforeSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles TV.BeforeSelect
        If TV_Binded Then
            If Not FPf.SAVE_ALL Then
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub Array_Of_FP_ARRAY_OF_FP_Form_Current(ByVal Index_Of_FP As Integer) Handles Array_Of_FP.ARRAY_OF_FP_Form_Current
        If TV_Binded Then
            TV_Binded = False
            Dim CurrentFP As FP = Array_Of_FP.DIC_FPs(Index_Of_FP).FP

            If CurrentFP.P_DATA_RecordStatus <> ENUM_RecordStatus.EXISTS Then
                TV_REFRESH_AT_THE_END(Fill_Timer_GotoNode)
            Else
                Dim GotoNodeName As String = get_Node_Key_from_Elements(Index_Of_FP, CurrentFP.P_DATA_Current_ID)

                If DIC_NODES.ContainsKey(GotoNodeName) Then
                    TV_REFRESH_AT_THE_END(GotoNodeName)
                Else
                    TV_FILL_AT_THE_END(GotoNodeName)
                End If
            End If

            TV_Binded = True
        End If
    End Sub

    Private Sub Array_Of_FP_ARRAY_OF_FP_Form_Field_AfterUpdate(ByVal Index_Of_FP As Integer, ByVal FPc As FP_Control) Handles Array_Of_FP.ARRAY_OF_FP_Form_Field_AfterUpdate
        TV_REFRESH_AT_THE_END()
    End Sub

    Private Sub Array_Of_FP_ARRAY_OF_FP_Form_NoRecord(ByVal Index_Of_FP As Integer) Handles Array_Of_FP.ARRAY_OF_FP_Form_NoRecord
        If TV_Binded Then
            TV_Binded = False

            TV_FILL_AT_THE_END()
            If Index_Of_FP = 0 Then
                TV_UNSELECT()
            Else
                Dim Index_Of_HeadFP As Integer = Index_Of_FP - 1
                Dim HeadFP As FP = Array_Of_FP.DIC_FPs(Index_Of_HeadFP).FP

                If HeadFP Is Nothing Then
                    TV_UNSELECT()
                Else
                    If HeadFP.P_DATA_RecordStatus <> ENUM_RecordStatus.EXISTS Then
                        TV_UNSELECT()
                    Else
                        'TV_SELECT(Index_Of_HeadFP, HeadFP.P_DATA_Current_ID)
                        TV_GOTO_Node_AT_THE_END(Index_Of_HeadFP, HeadFP.P_DATA_Current_ID)
                    End If
                End If
            End If

            TV_Binded = True
        End If
    End Sub

    Private Sub NODE_COLORING_ALL()
        For Each NodeKey As String In DIC_NODES.Keys
            NODE_COLORING(NodeKey)
        Next
    End Sub

    Private Sub Array_Of_FP_ARRAY_OF_FP_GRID_Filter_Changed(ByVal Index_Of_FP As Integer) Handles Array_Of_FP.ARRAY_OF_FP_GRID_Filter_Changed
        NODE_COLORING_ALL()
    End Sub

    Public Function TV_GET_LastSelectedNode() As TreeNode
        Dim OUT As TreeNode = Nothing

        If TV_LastSelectedNode_Key > "" Then
            If Not DIC_NODES.ContainsKey(TV_LastSelectedNode_Key) Then
                FPApp.DoErrorMsgBox("FP_FieldLogics.FP_L_TreeView_With_FP.TV_GET_LastSelectedNode", 0, String.Format("Node in TV_LastSelectedNode_Key ('{0}' is invalid.)", TV_LastSelectedNode_Key))
            Else
                OUT = DIC_NODES(TV_LastSelectedNode_Key)
            End If
        End If

        TV_GET_LastSelectedNode = OUT
    End Function

    Public Function TV_GOTO_PrevNode() As Boolean
        Dim OUT As Boolean = False

        Dim AktNode As TreeNode = TV_GET_LastSelectedNode()

        If Not AktNode Is Nothing Then
            If Not (AktNode.PrevVisibleNode Is Nothing) Then
                With AktNode.PrevVisibleNode
                    OUT = TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                End With
            End If
        End If

        TV_GOTO_PrevNode = OUT
    End Function

    Public Function TV_GOTO_PrevNode_On_This_Level() As Boolean
        Dim OUT As Boolean = False

        Dim AktNode As TreeNode = TV_GET_LastSelectedNode()

        If Not AktNode Is Nothing Then
            If Not (AktNode.PrevNode Is Nothing) Then
                With AktNode.PrevNode
                    OUT = TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                End With
            End If
        End If

        TV_GOTO_PrevNode_On_This_Level = OUT
    End Function

    Public Function TV_GOTO_NextNode() As Boolean
        Dim OUT As Boolean = False

        Dim AktNode As TreeNode = TV_GET_LastSelectedNode()

        If Not AktNode Is Nothing Then
            If Not (AktNode.NextVisibleNode Is Nothing) Then
                With AktNode.NextVisibleNode
                    OUT = TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                End With
            End If
        End If

        TV_GOTO_NextNode = OUT
    End Function

    Public Function TV_GOTO_NextNode_On_This_Level() As Boolean
        Dim OUT As Boolean = False

        Dim AktNode As TreeNode = TV_GET_LastSelectedNode()

        If Not AktNode Is Nothing Then
            If Not (AktNode.NextNode Is Nothing) Then
                With AktNode.NextNode
                    OUT = TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                End With
            End If
        End If

        TV_GOTO_NextNode_On_This_Level = OUT
    End Function

    Public Function TV_GOTO_Node(ByVal NodeKey As String) As Boolean
        Dim Level As Integer = Val(Mid(NodeKey, 1, 1))
        Dim ID As Long = Val(Mid(NodeKey, 3))

        Return TV_GOTO_Node(Level, ID)
    End Function

    Public Function TV_GOTO_Node(ByVal Level As Integer, ByVal ID As Long) As Boolean
        Dim OUT As Boolean = True

        If Not FPf.SAVE_ALL Then
            OUT = False
        Else
            If Level + 1 < Array_Of_FP.DIC_FPs.Count Then
                Dim ChildFP As FP = Array_Of_FP.DIC_FPs(Level + 1).FP

                If Not (ChildFP Is Nothing) Then
                    Dim TV_Binded_Old As Boolean = TV_Binded

                    TV_Binded = False
                    ChildFP.FORM_GOTO_NORECORD()
                    TV_Binded = TV_Binded_Old
                End If
            End If

            Dim CurrentFP As FP = Array_Of_FP.DIC_FPs(Level).FP

            If Not (CurrentFP Is Nothing) Then
                OUT = CurrentFP.FORM_GOTO_RECORD_BY_ID(ID)
            End If

            If OUT Then
                OUT = TV_SELECT(Level, ID)
            End If
        End If

        Return OUT
    End Function

    Public Function TV_GOTO_ROOT_FIRST() As Boolean
        Dim OUT As Boolean = False

        If TV.Nodes.Count > 0 Then
            Dim FirstRootKey As String = TV.Nodes(0).Name
            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(FirstRootKey), NODE_GET_ID_FROM_Key(FirstRootKey))
        End If

        TV_GOTO_ROOT_FIRST = OUT
    End Function

    Public Function TV_GOTO_ROOT_LAST() As Boolean
        If TV.Nodes.Count > 0 Then
            Dim LastKey As String = TV.Nodes(TV.Nodes.Count - 1).Name
            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(LastKey), NODE_GET_ID_FROM_Key(LastKey))
        End If
    End Function

    Public Function TV_GOTO_LAST_NODE() As Boolean
        If TV.Nodes.Count > 0 Then
            Dim MyLastNode As TreeNode = TV.Nodes(TV.Nodes.Count - 1)

            While Not (MyLastNode.LastNode Is Nothing)
                MyLastNode.Expand()
                MyLastNode = MyLastNode.LastNode
            End While

            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(MyLastNode.Name), NODE_GET_ID_FROM_Key(MyLastNode.Name))
        End If
    End Function

    Private Function TV_SELECTED_NODE_UPDOWN_WITH_RAISEEVENT(ByVal UpDown As ENUM_UpDown) As Boolean
        Dim OUT As Boolean = False

        Dim AktNode As TreeNode = TV_SELECTED_NODE()

        If Not (AktNode Is Nothing) Then
            Dim AktFP As FP = TV_SELECTED_NODE_GET_FP()

            If Not (AktFP Is Nothing) Then
                If AktFP.FORM_IsSubForm Then
                    AktFP.FORM_RECORD_UPDOWN(UpDown)
                    OUT = True
                End If
            End If
            If OUT = False Then
                RaiseEvent Node_MoveUpDown(Me, UpDown, OUT)
                If OUT Then
                    TV_REFRESH_AT_THE_END(AktNode.Name)
                End If
            End If
        End If

        Return OUT
    End Function

    Private Sub TV_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TV.KeyDown
        Dim CtrlPressed As Boolean = My.Computer.Keyboard.CtrlKeyDown
        Dim AktNode As TreeNode = TV_SELECTED_NODE()
        Dim AktFP As FP = Nothing

        If Not (AktNode Is Nothing) Then
            AktFP = Array_Of_FP.DIC_FPs(NODE_GET_LEVEL_FROM_Key(AktNode.Name)).FP
        End If

        Select Case e.KeyCode
            Case Keys.Up
                e.Handled = True
                If CtrlPressed Then
                    TV_SELECTED_NODE_UPDOWN_WITH_RAISEEVENT(ENUM_UpDown.UP)
                Else
                    TV_GOTO_PrevNode()
                End If

            Case Keys.Down
                e.Handled = True
                If CtrlPressed Then
                    TV_SELECTED_NODE_UPDOWN_WITH_RAISEEVENT(ENUM_UpDown.DOWN)
                Else
                    TV_GOTO_NextNode()
                End If

            Case Keys.PageUp
                If CtrlPressed Then
                    TV_GOTO_ROOT_FIRST()
                Else
                    If Not (TV.SelectedNode Is Nothing) Then
                        If TV.SelectedNode.Parent Is Nothing Then
                            TV_GOTO_PrevNode_On_This_Level()
                        Else
                            If TV.SelectedNode.Parent.FirstNode.Equals(TV.SelectedNode) Then
                                If (TV.SelectedNode.Parent.PrevNode Is Nothing) Then
                                    With TV.SelectedNode.Parent
                                        TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                    End With
                                Else
                                    If TV.SelectedNode.Parent.PrevNode.FirstNode Is Nothing Then
                                        With TV.SelectedNode.Parent.PrevNode
                                            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                        End With
                                    Else
                                        With TV.SelectedNode.Parent.PrevNode.FirstNode
                                            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                        End With
                                    End If
                                End If
                            Else
                                With TV.SelectedNode.Parent.FirstNode
                                    TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                End With
                            End If
                        End If
                    End If
                End If
                e.Handled = True

            Case Keys.PageDown
                If CtrlPressed Then
                    TV_GOTO_LAST_NODE()
                Else
                    If Not (TV.SelectedNode Is Nothing) Then
                        If TV.SelectedNode.Parent Is Nothing Then
                            TV_GOTO_NextNode_On_This_Level()
                        Else
                            If TV.SelectedNode.Parent.LastNode.Equals(TV.SelectedNode) Then
                                If (TV.SelectedNode.Parent.NextNode Is Nothing) Then
                                    'nothing to do
                                Else
                                    If TV.SelectedNode.Parent.NextNode.FirstNode Is Nothing Then
                                        With TV.SelectedNode.Parent.NextNode
                                            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                        End With
                                    Else
                                        With TV.SelectedNode.Parent.NextNode.FirstNode
                                            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                        End With
                                    End If
                                End If
                            Else
                                With TV.SelectedNode.Parent.LastNode
                                    TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(.Name), NODE_GET_ID_FROM_Key(.Name))
                                End With
                            End If
                        End If
                    End If
                End If
                e.Handled = True

            Case Keys.Home
                If CtrlPressed Then
                    TV_GOTO_ROOT_FIRST()
                Else
                    If Not (TV.SelectedNode Is Nothing) Then
                        If TV.SelectedNode.Parent Is Nothing Then
                            TV_GOTO_ROOT_FIRST()
                        Else
                            Dim Key As String = TV.SelectedNode.Parent.FirstNode.Name

                            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(Key), NODE_GET_ID_FROM_Key(Key))
                        End If
                    End If
                End If
                e.Handled = True

            Case Keys.End
                If CtrlPressed Then
                    TV_GOTO_LAST_NODE()
                Else
                    If Not (TV.SelectedNode Is Nothing) Then
                        If TV.SelectedNode.Parent Is Nothing Then
                            TV_GOTO_ROOT_LAST()
                        Else
                            Dim Key As String = TV.SelectedNode.Parent.LastNode.Name

                            TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(Key), NODE_GET_ID_FROM_Key(Key))
                        End If
                    End If
                End If
                e.Handled = True

            Case Keys.Right
                If Not (AktNode Is Nothing) Then
                    If Not (AktNode.FirstNode Is Nothing) Then
                        TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(AktNode.FirstNode.Name), NODE_GET_ID_FROM_Key(AktNode.FirstNode.Name))
                        e.Handled = True
                    End If
                End If

            Case Keys.Left
                If Not (AktNode Is Nothing) Then
                    If Not (AktNode.Parent Is Nothing) Then
                        Dim ParentNode As TreeNode = AktNode.Parent

                        If TV_GOTO_Node(NODE_GET_LEVEL_FROM_Key(ParentNode.Name), NODE_GET_ID_FROM_Key(ParentNode.Name)) Then
                            ParentNode.Collapse()
                        End If
                        e.Handled = True
                    End If
                End If

            Case Keys.Enter
                If Not (AktNode Is Nothing) Then
                    If Not (AktNode.FirstNode Is Nothing) Then
                        If AktNode.IsExpanded Then
                            AktNode.Collapse()
                        Else
                            AktNode.Expand()
                        End If
                    Else
                        If Not (AktFP Is Nothing) Then
                            AktFP.RAISEEVENT_GRID_Row_DoubleClick(e.Handled)
                        End If
                    End If
                End If

            Case Keys.D
                If CtrlPressed Then
                    If Not e.Handled Then
                        If Not (AktNode Is Nothing) Then
                            If Not (AktFP Is Nothing) Then
                                AktFP.FORM_RECORDS_DELETE_CURRENT()
                            Else
                                RaiseEvent Node_Delete(Me, False)
                            End If
                        End If
                    End If
                End If
        End Select
    End Sub

    Private Sub TV_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TV.KeyPress
        Dim IndexOf_FP As Integer = TV_SELECTED_NODE_GET_LEVEL()
        Dim CurrentID As Long = TV_SELECTED_NODE_GET_ID()
        Dim CurrentFP As FP = TV_SELECTED_NODE_GET_FP()

        RaiseEvent Node_KeyPress(Me, e, IndexOf_FP, CurrentFP, CurrentID)
    End Sub

    Private Sub Fill_Timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Fill_Timer.Tick
        Fill_Timer.Enabled = False

        If Not (FPf Is Nothing) Then
            If Not (FPf.Frm Is Nothing) Then
                Select Case Fill_Timer_FunctionsCode
                    Case Enum_Fill_Timer_Code.FILL
                        TV_FILL_IMMEDIATELY(Fill_Timer_AllwaysUpdate)

                    Case Enum_Fill_Timer_Code.REFRESH
                        TV_REFRESH_IMMEDIATELY(Fill_Timer_AllwaysUpdate)

                    Case Enum_Fill_Timer_Code.GOTO_NODE
                        'Nothing to do (mert a kovetkezo resz ugy is raallitja a node-ra a TV-t.
                End Select

                If Fill_Timer_GotoNode > "" Then
                    If DIC_NODES.Keys.Contains(Fill_Timer_GotoNode) Then
                        TV_GOTO_Node(Fill_Timer_GotoNode)
                    End If
                End If

                If Fill_Timer_Do_After_Check Then
                    Fill_Timer_Do_After_Check = False
                    Application.DoEvents()
                    System.Threading.Thread.Sleep(200)
                    Application.DoEvents()
                    Fill_Timer_Node.Checked = Fill_Timer_Checked
                End If
            End If
        End If

        Fill_Timer_CLEAR()
    End Sub

    Private Sub EVENT_TV_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TV.MouseWheel
        Dim Handled As Boolean = False

        RaiseEvent Node_MouseWheel(Me, e, Handled)
        If Not Handled Then
            Dim CtrlPressed As Boolean = My.Computer.Keyboard.CtrlKeyDown
            Dim AktNode As TreeNode = TV_GET_LastSelectedNode()

            If Not AktNode Is Nothing Then
                Dim DoIt As Boolean = True
                Dim FP As FP = Array_Of_FP.DIC_FPs(NODE_GET_LEVEL_FROM_Key(AktNode.Name)).FP

                If Not (FP Is Nothing) Then
                    DoIt = FP.FORM_RECORDS_SAVE_CURRENT
                End If
                If DoIt Then
                    Dim MoveRow As Boolean = False
                    If FP Is Nothing Then
                        MoveRow = CtrlPressed
                    Else
                        MoveRow = CtrlPressed And (FP.Button_Down Is Nothing) And (FP.Button_Up Is Nothing)
                    End If

                    If MoveRow Then 'Csak ezt kezelem, mert a masik esetben osszekeveredik a kod a TV automatikus scroll-javal. (A kodot bennehagytam, de ennek a feltetelnek a hatasara most inaktiv)

                        Dim i As Integer
                        Dim WheelCount = e.Delta / 120

                        If WheelCount > 0 Then
                            For i = 1 To WheelCount
                                If MoveRow Then
                                    If Not TV_SELECTED_NODE_UPDOWN_WITH_RAISEEVENT(ENUM_UpDown.UP) Then
                                        Exit For
                                    End If
                                Else
                                    If Not TV_GOTO_PrevNode() Then
                                        Exit For
                                    End If
                                End If
                            Next
                        Else
                            For i = -1 To WheelCount Step -1
                                If MoveRow Then
                                    If Not TV_SELECTED_NODE_UPDOWN_WITH_RAISEEVENT(ENUM_UpDown.DOWN) Then
                                        Exit For
                                    End If
                                Else
                                    If Not TV_GOTO_NextNode() Then
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    End If

                    Handled = True
                End If
            End If
        End If
    End Sub
End Class

Public Class FP_L_ImageList 'ML_20160519

    Public ImageList As ImageList
    Public AsmName As String

    Private Disposed As Boolean = False

    Public Sub New()
        ImageList = New ImageList
    End Sub

    Public Sub DisposeMe()
        If Disposed = False Then
            ImageList = Nothing
            Disposed = True
        End If
    End Sub

    Public Sub New(ASM_Name As String)
        ImageList = New ImageList
        LOAD_Pictures_FROM_ASM(ASM_Name)
    End Sub

    Public Sub LOAD_Pictures_FROM_SEL_SKIN()
        LOAD_Pictures_FROM_ASM("")
    End Sub

    Public Sub LOAD_Pictures_FROM_ASM(ByVal MyAsmName As String)
        Dim CurrentASM As Reflection.Assembly = Nothing

        AsmName = MyAsmName

        If AsmName <> "" Then
            AsmName = AsmName + ".."
        End If

        If gl_FPApp.SKIN_getASM_And_OBJECTNAME(String.Format("{0}TV_Empty.png", AsmName), CurrentASM, "") Then
            For Each ResName As String In CurrentASM.GetManifestResourceNames
                Dim Extension As String = ResName.Substring(ResName.LastIndexOf("."))

                Select Case Extension
                    Case ".bmp", ".jpg", ".png", ".gif"
                        Dim Add_Picture_As_Name As String = Mid(ResName, Len("SEL_SKIN") + 2)

                        If Mid(Add_Picture_As_Name, 1, Len("TV") + 1) = "TV_" Or Mid(Add_Picture_As_Name, 1, Len("W") + 1) = "W_" Then
                            Dim im As Bitmap
                            Dim Prepared_RES_Name As String = System.IO.Path.GetFileNameWithoutExtension(Add_Picture_As_Name)

                            im = New Bitmap(CurrentASM.GetManifestResourceStream(ResName))

                            ImageList.Images.Add(Prepared_RES_Name, im)
                        End If

                    Case Else
                        'Nothing to do
                End Select
            Next
        End If
    End Sub

    Public Function GET_ALL_PICTURES_IN_DIC() As Dictionary(Of String, Bitmap)
        Dim DIC As New Dictionary(Of String, Bitmap)

        For Each CurrentKey As String In ImageList.Images.Keys
            DIC.Add(CurrentKey, ImageList.Images(CurrentKey))
        Next

        Return DIC
    End Function
End Class

Public Class FP_L_CreditLimit_CUST_Field
    Public Structure STRUCT_FP_L_CreditLimit_CUST_Field
        Dim FPc As FP_Control
        Dim CL_Check_Need As Boolean
        Dim HWND_ID As Long
    End Structure

    Public WithEvents FPc As FP_Control
    Public WithEvents FP_of_FPc As FP
    Private CL_Need_Check As Boolean
    Private Disposed As Boolean = False
    Private LastChecked_RecordID As Long = 0
    Private LastChecked_Cust_ID As Long = 0
    Private HWND_ID As Long

    Public Sub New(P As STRUCT_FP_L_CreditLimit_CUST_Field)
        With P
            FPc = .FPc
            FP_of_FPc = FPc.FP
            CL_Need_Check = .CL_Check_Need
            HWND_ID = .HWND_ID
        End With
    End Sub

    Public Sub Dispose()
        If Disposed = False Then
            FPc = Nothing
            FP_of_FPc = Nothing

            Disposed = True
        End If
    End Sub

    Public Function Check(Optional CheckAllways As Boolean = False) As Boolean
        Dim OUT As Boolean = True
        If CL_Need_Check Then
            If FPc.Selected_ID <> 0 Then
                OUT = gl_FPApp.CL_CHECK(FPc.Selected_ID, True, HWND_ID)
            End If
        End If
        Return OUT
    End Function

    Private Sub FPc_Field_BeforeUpdate(sender_FPc As FP_Control, ByRef Cancel As Integer) Handles FPc.Field_BeforeUpdate
        If Not Disposed Then
            Cancel = Not Check()
        End If
    End Sub
End Class

Public Class FP_L_CreditLimit_Panel
    Public Structure STRUCT_FP_L_CreditLimit_Panel
        Dim CreditLimit_Panel As Panel
        Dim FP_CreditLimit_SubPrefix As String
        Dim FieldPrefix As String
        Dim FP_of_Customer As FP
        Dim FPc_Customer As FP_Control
        Dim Name_of_Field_of_Customer_ID As String 'Csak akkor kell, ha az FPc_Customer nincs megadva
    End Structure

    Public FP_CreditLimit As FP
    Public FPf As FP_Form
    Public WithEvents CreditLimit_Panel As Panel
    Public WithEvents FP_of_Customer As FP
    Public Name_of_Field_of_Customer_ID As String
    Public WithEvents FPc_Customer As FP_Control
    Public FPc_Creditlimit_Info_Rtf As RichTextBox
    Private Control_Prefix = "CL_"
    Public FP_CreditLimit_SubPrefix As String
    Public FieldPrefix As String
    Private Disposed As Boolean = False
    Private Control_Creditlimit_Info_Rtf As RichTextBox

    Public Sub New(P As STRUCT_FP_L_CreditLimit_Panel)
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

        If Fieldprefix > "" Then
            OUT = Fieldprefix + OUT
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
        End If
    End Sub


    Public Sub Dispose()
        If Disposed = False Then
            Disposed = True

            CreditLimit_Panel = Nothing
            FP_of_Customer = Nothing

            FPc_Creditlimit_Info_Rtf = Nothing
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
End Class

Public Class FP_L_Fields_for_Incoterms
    Public Enum ENUM_FP_L_Fields_for_Incoterms_RULES
        POREC_TO_REMARKS = 1
    End Enum

    Public Structure Struct_FP_L_Fields_for_Incoterms_Params
        Dim FPc_Field_Incoterms As FP_Control
        Dim FPc_Field_Incoterms_Remarks As FP_Control
    End Structure

    Private DT_Rules As DataTable = Nothing

    Public WithEvents FPc_Field_Incoterms As FP_Control
    Private WithEvents FP_of_Field As FP
    Public FPc_Field_Incoterms_Remarks As FP_Control
    Private Disposed As Boolean = False

    Public Sub New(P As Struct_FP_L_Fields_for_Incoterms_Params)
        With P
            FPc_Field_Incoterms = .FPc_Field_Incoterms
            FPc_Field_Incoterms_Remarks = .FPc_Field_Incoterms_Remarks
        End With

        If Not (FPc_Field_Incoterms Is Nothing) Then
            FP_of_Field = FPc_Field_Incoterms.FP
        End If

        Dim MySQL As String = "SELECT * FROM Incoterms_Rules ORDER BY SeqNum"
        gl_FPApp.DC.Qdf_Fill_DT(MySQL, DT_Rules)
    End Sub

    Public Sub Dispose()
        If Not Disposed Then
            FPc_Field_Incoterms = Nothing
            FP_of_Field = Nothing
            FPc_Field_Incoterms_Remarks = Nothing

            If Not (DT_Rules Is Nothing) Then
                DT_Rules.Clear()
                DT_Rules = Nothing
            End If

            Disposed = True
        End If
    End Sub

    Private Sub FP_Field_Form_Field_AfterUpdate(FPc As FP_Control) Handles FP_of_Field.Form_Field_AfterUpdate
        If FPc.Equals(FPc_Field_Incoterms) Then
            SET_FPc_Field_Incoterms_Rules()
        End If
    End Sub


    Public Sub SET_FPc_Field_Incoterms_Rules()
        If Not (FPc_Field_Incoterms Is Nothing) Then
            Dim Current_Incoterms_ID As Long = FPc_Field_Incoterms.P_ID

            If Current_Incoterms_ID <> 0 Then
                Dim Crit As String = String.Format("Incoterms_ID = {0}", Current_Incoterms_ID)
                For Each DRow As DataRow In DT_Rules.Select(Crit)
                    Dim Current_Action_ID As ENUM_FP_L_Fields_for_Incoterms_RULES = DRow!Action_ID
                    Dim Current_Params As String = DRow!Params

                    If Current_Params = "" Then
                        Current_Params = "#_CITY_#"
                    End If

                    Select Case Current_Action_ID
                        Case ENUM_FP_L_Fields_for_Incoterms_RULES.POREC_TO_REMARKS
                            If Not (FPc_Field_Incoterms_Remarks Is Nothing) Then
                                Dim Head_Ord_L_ID As Long
                                Dim Head_Ord_L_Crit As String = String.Format("SELECT Head_Ord_L_ID FROM Cont_OE_Ord_L WHERE Ord_L_ID = {0}", FPc_Field_Incoterms.FP.P_DATA_Current_ID)
                                Dim Head_Ord_L_DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(Head_Ord_L_Crit)
                                If Not Head_Ord_L_DRow Is Nothing Then
                                    Head_Ord_L_ID = Head_Ord_L_DRow.Item("Head_Ord_L_ID")
                                Else
                                    Head_Ord_L_ID = 0
                                End If
                                Dim PoRec_Crit As String = String.Format("ParentRecord_ID = {0} AND AddressTypes_ID = {1} AND ParentTable_ID = 201", Head_Ord_L_ID, CInt(ENUM_AddressTypes.CONT_PoRec))
                                Dim sPoRec_Addr_ID As String = nz(gl_FPApp.DC.DLookup("ID", "Addresses", PoRec_Crit), 0)
                                Dim PoRec_Addr_ID As Long
                                If IsNumeric(sPoRec_Addr_ID) Then
                                    PoRec_Addr_ID = CType(sPoRec_Addr_ID, Long)
                                Else
                                    PoRec_Addr_ID = 0
                                End If
                                Dim MySQL As String = String.Format("SELECT dbo.GET_ADDRESSES_STR_FROM_ELEMENTS({0}, '{1}', '{2}') REMARK_STR", PoRec_Addr_ID, Current_Params, gl_FPApp.LandDialog)
                                Dim Remarks_STR As String = nz(gl_FPApp.DC.Qdf_get_DataRow(MySQL)!REMARK_STR, "")

                                FPc_Field_Incoterms_Remarks.P_VALUE = Remarks_STR
                            End If

                        Case Else
                            gl_FPApp.DoErrorMsgBox("FP_L_Fields_for_Incoterms", 0, String.Format("Unknown Action_ID", Current_Action_ID))
                    End Select
                Next
            End If
        End If
    End Sub
End Class

Public Class FP_L_SplashForm

#Region "DECLARE"
    Implements IDisposable

    Private _ParentForm As Form
    Private t As Threading.Thread = Nothing
    Private f As FP_SplashForm = Nothing
    Private _Sleep As Integer

    Private Delegate Sub SetMessageTextDelegate(MessageText As String)

#End Region

#Region "CLASS CONSTRUCTOR"

    Public Sub New(ParentForm As Form)
        _ParentForm = ParentForm
    End Sub

    Public Sub New(ParentForm As Form, MessageText As String, Optional Sleep As Integer = 1000)
        _ParentForm = ParentForm

        OpenSplashForm()
        SetMessageText(MessageText, Sleep)
    End Sub

#End Region

#Region "IDISPOSABLE SUPPORT"

    Private disposedValue As Boolean

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If
        End If
        Me.disposedValue = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#Region "PRIVATE SUBS"

    Private Sub _Close()
        If f Is Nothing Then
            Exit Sub
        End If

        f.Close()
        f = Nothing
    End Sub

    Private Sub _ShowForm()
        f = New FP_SplashForm
        'f.TopMost = True
        f.ShowDialog()
    End Sub

#End Region

#Region "PUBLIC SUBS"

    Public Sub CloseSplashForm()
        If f Is Nothing Then
            Exit Sub
        End If

        f.Invoke(New MethodInvoker(AddressOf _Close))

        If t Is Nothing Then
            Exit Sub
        End If

        t.Join()
    End Sub

    Public Sub SetMessageText(MessageText As String, Optional Sleep As Integer = 0)
        System.Threading.Thread.Sleep(200) 'Azert kell ide, mert kulonben az InvokeRequired utasitas hibat general

        If f.InvokeRequired Then
            f.Invoke(New SetMessageTextDelegate(AddressOf SetMessageText), MessageText)

            If Sleep > 0 Then
                System.Threading.Thread.Sleep(Sleep)
            End If
        Else
            f.MessageText_Label.Text = MessageText
        End If
    End Sub

    Public Sub OpenSplashForm()
        t = New Threading.Thread(AddressOf _ShowForm)
        t.SetApartmentState(Threading.ApartmentState.STA)
        t.Priority = Threading.ThreadPriority.Highest
        t.Name = "SplashFormThread"

        t.Start()
    End Sub

#End Region

End Class

Public Class FP_L_StatusField
    Public Event Status_Changing(sender As FP_L_StatusField, ByRef Cancel As Boolean)
    Public Event Status_Changed(sender As FP_L_StatusField)

    Public Const CONST_STATUSCODES_PREV As Long = -1

    'Teljeskoru statuszkod kezelest tesz a control-ra
    Public Structure Struct_FP_L_StatusField
        Dim FP_Parent_of_StatusField As FP 'Nem az FPc FP-je, hanem ha van parent_fp, akkor az!!!
        Dim FPc As FP_Control                   'A control, amelyik a statuszokat tartalmazza
        Dim Field_StatusID_Name As String       'Ha nem combobox a statusz mezo es nincs megadva a simple_select, amibol valasztani kell a statuskodokat,
        'akkor a New eljaras automatikusan beteszi a "@@VB_SIMPLE_SELECT_STATUSCODES_AFTER" SIMPLE_SELECT-et es az itt megadott
        'mezot a StatusID tarolasara szolgalo mezokent.
        'Megadasa tehat akkor kell, ha nem combobox az FPc es nincs megadva hozza a SIMPLE_SELECT.
        Dim FPp_StatusPicture As FP_PictureBox  'Ebben a picturebox-ban megjelenhet a statuszhoz rendelt kep (megadasa nem kotelezo)
        Dim FPp_StatusChange As FP_PictureBox   'Ez a nyomogomb a "@@VB_SIMPLE_SELECT_FP_L_StatusField_STATUSCHANGE" ZDISPO segitsegevel hajtja vegre a statuszvaltast.
        Dim Status_Level As Integer
        Dim Hierarchy_Code As String
        Dim OrderEntry_OpenType As Integer
        'Statuscodes_Rules form szintu levalogatasahoz tartozo parameterek
        Dim Statuscodes_Rules_SubCode_FieldName As String
    End Structure

    Public WithEvents FPApp_for_Messages As FP_App = gl_FPApp
    Public WithEvents FP_Parent_of_StatusField As FP
    Public WithEvents FP_of_StatusField As FP
    Public WithEvents c_of_StatusField As Control
    Public WithEvents FPf As FP_Form
    Public WithEvents FPc As FP_Control
    Public WithEvents FPp_StatusPicture As FP_PictureBox
    Public WithEvents FPp_StatusChange As FP_PictureBox
    Public Status_Level As Integer
    Public Hierarchy_Code As String
    Public OrderEntry_OpenType As Integer
    Public Statuscodes_Rules_SubCode_FieldName As String
    Private WithEvents T As New Windows.Forms.Timer
    Private Disposed As Boolean = False

    Sub New(P As Struct_FP_L_StatusField)
        With P
            FPc = .FPc
            FPp_StatusPicture = .FPp_StatusPicture
            FPp_StatusChange = .FPp_StatusChange
            FP_Parent_of_StatusField = .FP_Parent_of_StatusField
            Status_Level = .Status_Level
            Hierarchy_Code = .Hierarchy_Code
            OrderEntry_OpenType = .OrderEntry_OpenType
            Statuscodes_Rules_SubCode_FieldName = .Statuscodes_Rules_SubCode_FieldName
        End With

        FP_of_StatusField = FPc.FP
        c_of_StatusField = FPc.c
        If FP_of_StatusField.Equals(FP_Parent_of_StatusField) Then
            FPf.DoErrorMsgBox("FP_L_StatusField.New", 0, String.Format("Hibas az FP_Parent_of_StatusField parameter!!! Itt a PARENT FP-t kell megadni (ha van))"))
        End If

        If FP_Parent_of_StatusField Is Nothing Then
            If Not (FP_of_StatusField.Parent_FP Is Nothing) Then
                FP_Parent_of_StatusField = FP_of_StatusField.Parent_FP
            End If
        Else
            If Not FP_Parent_of_StatusField.Equals(FP_of_StatusField.Parent_FP) Then
                FPf.DoErrorMsgBox("FP_L_StatusField.New", 0, String.Format("Hibas az FP_Parent_of_StatusField parameter!!! Itt a PARENT FP-t kell megadni (es most mas van megadva))"))
            End If
        End If

        FPf = FP_of_StatusField.FPf
        FPc.P_Marker = FP_Control.ENUM_Markertypes.Right_Arrow

        If FPc.P_IsDataField Then
            FPf.DoErrorMsgBox("FP_L_StatusField.New", 0, String.Format("A státusz mező ne legyen DataField (vedd ki a SAVE eljarasbol!)"))
        End If

        If FPc.P.DT_FixText_Key = "" Then
            If TypeOf (FPc.c) Is ComboBox Then
                With FPc.P
                    .DT_FixText_Key = "@@VB_COMBO_STATUSCODES"
                    .DT_WHERE2 = String.Format("OrderLevel IN (0, {0}) AND Lang IN ('', '@LANDDIALOG') AND Hierarchy_Code IN ('{1}', '')", Status_Level, Hierarchy_Code)
                End With
                FPc.DT_REFRESH()
            ElseIf TypeOf (FPc.c) Is TextBox Then
                With FPc.P
                    .DT_FixText_Key = "@@VB_SIMPLE_SELECT_STATUSCODES_AFTER"
                    .DT_ID_Field = P.Field_StatusID_Name
                    .DT_WHERE2 = String.Format("OrderLevel IN (0, {0}) AND Lang IN ('', '@LANDDIALOG') AND Hierarchy_Code IN ('{1}', '')", Status_Level, Hierarchy_Code)
                End With

            Else
                gl_FPApp.DoErrorMsgBox("FP_L_StatusField.New", 0, "FPc.c has an unknown controltype.")
            End If
        End If
    End Sub

    Public Sub Dispose_Me()
        If Disposed = False Then
            If Not (T Is Nothing) Then
                If T.Enabled Then
                    Dim ee As New EventArgs
                    T_Tick(T, ee)
                End If

                T.Dispose()
                T = Nothing
            End If

            FPc = Nothing
            FPp_StatusPicture = Nothing
            FPp_StatusChange = Nothing
            FP_of_StatusField = Nothing
            FP_Parent_of_StatusField = Nothing
            FPf = Nothing
            FPApp_for_Messages = Nothing
            c_of_StatusField = Nothing

            Disposed = True
        End If
    End Sub

    Public ReadOnly Property P_Is_ErrorForm_Opened As Boolean
        Get
            Dim OUT As Boolean = False

            For Each AktKey As Long In gl_FPApp.Forms.Keys
                Dim Current_Frm As Form = gl_FPApp.Forms(AktKey).Frm

                If Not (Current_Frm Is Nothing) Then
                    If TypeOf (Current_Frm) Is FP_ErrorList Then
                        With CType(Current_Frm, FP_ErrorList)
                            If Not (.Parent_FPf Is Nothing) Then
                                If FPf.Frm.Equals(.Parent_FPf.Frm) Then
                                    OUT = True
                                    Exit For
                                End If
                            End If
                        End With
                    End If
                End If
            Next

            Return OUT
        End Get
    End Property

    Public ReadOnly Property P_Default_Status_ID As Long
        Get
            Return gl_FPApp.CL_Statuscodes.GET_Default_Status_ID(Hierarchy_Code, Status_Level)
        End Get
    End Property

    Public Sub Close_All_ErrorForm()
        Dim ListOfForms As New List(Of Form)

        For Each AktKey As Long In gl_FPApp.Forms.Keys
            Dim Current_Frm As Form = gl_FPApp.Forms(AktKey).Frm

            If Not (Current_Frm Is Nothing) Then
                If TypeOf (Current_Frm) Is FP_ErrorList Then
                    With CType(Current_Frm, FP_ErrorList)
                        If Not (.Parent_FPf Is Nothing) Then
                            If FPf.Frm.Equals(.Parent_FPf.Frm) Then
                                ListOfForms.Add(Current_Frm)
                            End If
                        End If
                    End With
                End If
            End If
        Next

        For Each Frm As Form In ListOfForms
            Frm.Close()
        Next
    End Sub

    Public ReadOnly Property P_STATUS_ID_CURRENT As Long
        Get
            Dim OUT As Long = 0

            If TypeOf (FPc.c) Is ComboBox Then
                OUT = FPc.P_VALUE

            ElseIf TypeOf (FPc.c) Is TextBox Then
                OUT = FPc.Selected_ID

            Else
                gl_FPApp.DoErrorMsgBox("FP_L_StatusField.P_STATUS_ID_CURRENT", 0, "FPc has an unknown controltype.")
            End If

            Return FPc.P_ID
        End Get
    End Property

    Public ReadOnly Property P_STATUS_ID_SAVED As Long
        Get
            Return FPc.P_ID_SAVED
        End Get
    End Property

    Private CASHED_PREV_RecordID As Long = 0
    Private CASHED_PREV_OldTransactID As Long = 0
    Private CASHED_PREV_ID As Long = 0

    Private Sub T_START()
        T.Interval = 100
        T.Enabled = True
    End Sub

    Private Sub T_STOP()
        T.Enabled = False
    End Sub

    Public ReadOnly Property P_STATUS_ID_PREV As Long
        Get
            If FPc.FP.P_DATA_RecordStatus <> ENUM_RecordStatus.EXISTS Then
                Return 0
            End If

            Dim Current_RecordID As Long = FP_of_StatusField.P_DATA_Current_ID
            Dim Current_OldTransactID As Long = Val(FP_of_StatusField.DATA_Field_getSavedValue("OldTransactID"))

            If CASHED_PREV_RecordID = Current_RecordID And
                CASHED_PREV_OldTransactID = Current_OldTransactID Then
                Return CASHED_PREV_ID
            End If

            CASHED_PREV_RecordID = Current_RecordID
            CASHED_PREV_OldTransactID = Current_OldTransactID
            CASHED_PREV_ID = 0

            Dim MySQL As String = String.Format("SELECT dbo.STATUSCODES_GET_PREV({0}) PREV_STATUS_ID", FP_of_StatusField.P_DATA_Current_ID)
            Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)

            If Not (DRow Is Nothing) Then
                CASHED_PREV_ID = DRow!PREV_STATUS_ID
            End If

            Return CASHED_PREV_ID
        End Get
    End Property

    Private ReadOnly Property P_Parent_FP_CurrentID As Long
        Get
            Dim OUT As Long = 0

            If Not (FP_Parent_of_StatusField Is Nothing) Then
                OUT = FP_Parent_of_StatusField.P_DATA_Current_ID
            End If

            Return OUT
        End Get
    End Property

    Public Function CHECK_SELECTED_STATUS_BEFORE(ByRef OUT_TransactID As Long, NewStatusID As Long, Type_Of_StatusChange As CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE, WithDialog As Boolean) As Boolean
        Dim OUT As Boolean = True

        OUT_TransactID = 0

        If FPc.FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            Return True
        End If

        If NewStatusID = 0 Then
            OUT = False

            If WithDialog Then
                FPc.FPf.DoMyMsgBox_AT_THE_END(7) 'Adatmegadas kotelezo
            End If

            Return False
        End If

        If NewStatusID <> P_STATUS_ID_CURRENT Then
            If gl_FPApp.CL_Statuscodes.GET_Status_Change_Type(P_STATUS_ID_SAVED, NewStatusID) <> CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.NOT_ALLOWED Then
                Select Case gl_FPApp.CL_Statuscodes.GET_Status_Change_Type(P_STATUS_ID_SAVED, NewStatusID)
                    Case CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.NOT_ALLOWED
                        FP_of_StatusField.FORM_DORESYNC()
                        FPc.FPf.DoMyMsgBox_AT_THE_END(95002) 'Statuszvaltas nem engedelyezett
                        Return False

                    Case CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_ALL_CALC_INVOICED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_ALL_OUTGOING_INVOICED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_ALL_INCOMING_INVOICED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_ALL_OUTGOING_INVOICED_AND_PAYED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_ALL_INCOMING_INVOICED_AND_PAYED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_ALL_CALC_INVOICED_AND_PAYED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_AGAIN_NOT_ALL_OUTGOING_INVOICED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_AGAIN_NOT_ALL_INCOMING_INVOICED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_AGAIN_NOT_ALL_CALC_INVOICED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_AGAIN_NOT_ALL_OUTGOING_PAYED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_AGAIN_NOT_ALL_INCOMING_PAYED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_AGAIN_NOT_ALL_INVOICED

                        FP_of_StatusField.FORM_DORESYNC()
                        FPc.FPf.DoMyMsgBox_AT_THE_END(95003) 'Manualisan ez a statuszvaltas nem engedelyezett
                        Return False

                    Case CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.MANUAL_WITH_CONFIRM
                        Dim Confirm_Message As String = gl_FPApp.CL_Statuscodes.GET_Status_Change_CONFIRM_MSG(P_STATUS_ID_SAVED, P_STATUS_ID_CURRENT)
                        If FPc.FPf.DoMyMsgBox(95004, Confirm_Message, "SEQ,NO", "SEQ,YES") <> 2 Then
                            FP_of_StatusField.FORM_DORESYNC()
                            Return False
                        End If

                    Case CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.MANUAL
                        'Nothing to do

                    Case Else
                        gl_FPApp.DoErrorMsgBox(FPc.FPf, "FP_L_StatusField.CHECK_SELECTED_STATUS", 0, "Unknown Status change type")
                        Return False
                End Select

                Dim sqlComm As SqlCommand = FPc.FPf.FPApp.DC.CNN.CreateCommand()
                Dim SubCode_Value As String = ""

                If Statuscodes_Rules_SubCode_FieldName > "" Then
                    SubCode_Value = FP_of_StatusField.DATA_Field_getSavedValue(Statuscodes_Rules_SubCode_FieldName)
                End If

                If OUT_TransactID = 0 Then
                    OUT_TransactID = gl_FPApp.Tr_getNewTransactID("CHECK_SELECTED_STATUS", FP_of_StatusField.P_DATA_Current_ID)
                End If

                With FPf.FPApp.DC
                    .Qdf_set_SP(sqlComm, "StatusCodes_CHECK_1_RECORD")
                    .Qdf_AddParameter(sqlComm, "@Lang", SqlDbType.NVarChar, , 3, gl_FPApp.LandDialog)
                    .Qdf_AddParameter(sqlComm, "@Terminals_ID", SqlDbType.Int, , , , , Terminals_ID)
                    .Qdf_AddParameter(sqlComm, "@TransactID", SqlDbType.Int, , , , , OUT_TransactID)
                    .Qdf_AddParameter(sqlComm, "@From_Status_ID", SqlDbType.Int, , , , , FPc.P_ID_SAVED)
                    .Qdf_AddParameter(sqlComm, "@To_Status_ID", SqlDbType.Int, , , , , NewStatusID)
                    .Qdf_AddParameter(sqlComm, "@ForObject", SqlDbType.NVarChar, , 128, Hierarchy_Code)
                    .Qdf_AddParameter(sqlComm, "@SubCode", SqlDbType.NVarChar, , 128, SubCode_Value)
                    .Qdf_AddParameter(sqlComm, "@RecordID", SqlDbType.Int, , , , , FPc.FP.P_DATA_Current_ID)
                    .Qdf_AddParameter(sqlComm, "@Record_HeadID", SqlDbType.Int, , , , , P_Parent_FP_CurrentID)
                    .Qdf_AddParameter(sqlComm, "@OUT_Count_of_Errors", SqlDbType.Int, ParameterDirection.Output)

                    .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                    .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
                End With

                CURSOR_SHOW_WAIT()
                Try
                    OUT = FPc.FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG, "@ErrText")

                    If nz(sqlComm.Parameters("@OUT_Count_of_Errors").Value, 0) > 0 Then
                        OUT = False
                        FP_of_StatusField.FORM_DORESYNC()

                        If WithDialog Then
                            Dim FP_ErrorList_P As New FP_ErrorList.FP_ERRORLIST_PARAMS

                            With FP_ErrorList_P
                                .TransactID = OUT_TransactID
                                .Parent_FPf = FPf
                            End With

                            Dim ErrorFrm As New FP_ErrorList(FP_ErrorList_P)

                            ErrorFrm.Show()
                        End If
                    End If

                Catch ex As Exception
                    OUT = False
                    FPc.FPf.FPApp.DoErrorMsgBox("FP_FieldLogics..FP_L_StatusField", Err.Number, Err.Description)
                End Try

                CURSOR_SHOW_DEFAULT()
            End If
        End If

        Return OUT
    End Function

    Public Function CHECK_SELECTED_STATUS_AFTER(ByRef OUT_TransactID As Long, Optional WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = True

        OUT_TransactID = 0

        If FPc.FP.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
            Return True
        End If

        If P_STATUS_ID_CURRENT = 0 Then
            OUT = False

            If WithDialog Then
                FPc.FPf.DoMyMsgBox_AT_THE_END(7) 'Adatmegadas kotelezo
            End If
            Return False
        End If

        If P_STATUS_ID_CURRENT <> P_STATUS_ID_SAVED Then
            Dim NEXT_STATUS_ID_FOR_CHECK As Long = P_STATUS_ID_CURRENT

            If gl_FPApp.CL_Statuscodes.GET_Status_Change_Type(P_STATUS_ID_SAVED, NEXT_STATUS_ID_FOR_CHECK) = CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.NOT_ALLOWED Then
                FP_of_StatusField.FORM_DORESYNC()
                FPc.FPf.DoMyMsgBox_AT_THE_END(95002) 'Statuszvaltas nem engedelyezett
                Return False
            Else
                Select Case gl_FPApp.CL_Statuscodes.GET_Status_Change_Type(P_STATUS_ID_SAVED, NEXT_STATUS_ID_FOR_CHECK)
                    Case CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.NOT_ALLOWED
                        FP_of_StatusField.FORM_DORESYNC()
                        FPc.FPf.DoMyMsgBox_AT_THE_END(95002) 'Statuszvaltas nem engedelyezett
                        Return False

                    Case CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.MANUAL
                        'Nothing to do

                    Case CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_ALL_CALC_INVOICED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_ALL_OUTGOING_INVOICED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_ALL_INCOMING_INVOICED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_ALL_OUTGOING_INVOICED_AND_PAYED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_ALL_INCOMING_INVOICED_AND_PAYED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_ALL_CALC_INVOICED_AND_PAYED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_AGAIN_NOT_ALL_OUTGOING_INVOICED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_AGAIN_NOT_ALL_INCOMING_INVOICED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_AGAIN_NOT_ALL_CALC_INVOICED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_AGAIN_NOT_ALL_OUTGOING_PAYED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_AGAIN_NOT_ALL_INCOMING_PAYED,
                         CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.AUTOMATED_AGAIN_NOT_ALL_INVOICED
                        'Nothing to do
                        'Ebben az eljarasban mar az a stadium van, amikor a mezobe az adatbeiras megtortent.
                        FP_of_StatusField.FORM_DORESYNC()
                        FPc.FPf.DoMyMsgBox_AT_THE_END(95003) 'Manualisan ez a statuszvaltas nem engedelyezett
                        Return False

                    Case CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.MANUAL_WITH_CONFIRM
                        Dim Confirm_Message As String = gl_FPApp.CL_Statuscodes.GET_Status_Change_CONFIRM_MSG(P_STATUS_ID_SAVED, P_STATUS_ID_CURRENT)
                        If FPc.FPf.DoMyMsgBox(95004, Confirm_Message, "SEQ,NO", "SEQ,YES") <> 2 Then
                            FP_of_StatusField.FORM_DORESYNC()
                            Return False
                        End If

                    Case Else
                        gl_FPApp.DoErrorMsgBox(FPc.FPf, "FP_L_StatusField.CHECK_SELECTED_STATUS", 0, "Unknown Status change type")
                        Return False
                End Select

                Dim sqlComm As SqlCommand = FPc.FPf.FPApp.DC.CNN.CreateCommand()
                Dim SubCode_Value As String = ""

                If Statuscodes_Rules_SubCode_FieldName > "" Then
                    SubCode_Value = FP_of_StatusField.DATA_Field_getSavedValue(Statuscodes_Rules_SubCode_FieldName)
                End If

                If OUT_TransactID = 0 Then
                    OUT_TransactID = gl_FPApp.Tr_getNewTransactID("CHECK_SELECTED_STATUS", FP_of_StatusField.P_DATA_Current_ID)
                End If

                With FPf.FPApp.DC
                    .Qdf_set_SP(sqlComm, "StatusCodes_CHECK_1_RECORD")
                    .Qdf_AddParameter(sqlComm, "@Lang", SqlDbType.NVarChar, , 3, gl_FPApp.LandDialog)
                    .Qdf_AddParameter(sqlComm, "@Terminals_ID", SqlDbType.Int, , , , , Terminals_ID)
                    .Qdf_AddParameter(sqlComm, "@TransactID", SqlDbType.Int, , , , , OUT_TransactID)
                    .Qdf_AddParameter(sqlComm, "@From_Status_ID", SqlDbType.Int, , , , , FPc.P_ID_SAVED)
                    .Qdf_AddParameter(sqlComm, "@To_Status_ID", SqlDbType.Int, , , , , FPc.P_ID)
                    .Qdf_AddParameter(sqlComm, "@ForObject", SqlDbType.NVarChar, , 128, Hierarchy_Code)
                    .Qdf_AddParameter(sqlComm, "@SubCode", SqlDbType.NVarChar, , 128, SubCode_Value)
                    .Qdf_AddParameter(sqlComm, "@RecordID", SqlDbType.Int, , , , , FPc.FP.P_DATA_Current_ID)
                    .Qdf_AddParameter(sqlComm, "@Record_HeadID", SqlDbType.Int, , , , , P_Parent_FP_CurrentID)
                    .Qdf_AddParameter(sqlComm, "@OUT_Count_of_Errors", SqlDbType.Int, ParameterDirection.Output)

                    .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                    .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
                End With

                CURSOR_SHOW_WAIT()
                Try
                    OUT = FPc.FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG, "@ErrText")

                    If nz(sqlComm.Parameters("@OUT_Count_of_Errors").Value, 0) > 0 Then
                        OUT = False
                        FP_of_StatusField.FORM_DORESYNC()

                        If WithDialog Then
                            Dim FP_ErrorList_P As New FP_ErrorList.FP_ERRORLIST_PARAMS

                            With FP_ErrorList_P
                                .TransactID = OUT_TransactID
                                .Parent_FPf = FPf
                            End With

                            Dim ErrorFrm As New FP_ErrorList(FP_ErrorList_P)

                            ErrorFrm.Show()
                        End If
                    End If

                Catch ex As Exception
                    OUT = False
                    FPc.FPf.FPApp.DoErrorMsgBox("FP_FieldLogics..FP_L_StatusField", Err.Number, Err.Description)
                End Try

                CURSOR_SHOW_DEFAULT()
            End If
        End If

        Return OUT
    End Function

    Public Sub SET_LAYOUT()
        Dim BackColor As Color = gl_FPApp.CL_Statuscodes.GET_StatusField_BackColor(P_STATUS_ID_CURRENT)
        Dim ForeColor As Color = gl_FPApp.CL_Statuscodes.GET_StatusField_ForeColor(P_STATUS_ID_CURRENT)

        'If Not (FPc Is Nothing) Then
        '    If FP_of_StatusField.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD Then
        '        FPc.P.Locked = True
        '    Else
        '        FPc.P.Locked = False
        '    End If
        'End If
        If Not (FPc Is Nothing) Then
            FPc.c.BackColor = BackColor
            FPc.c.ForeColor = ForeColor
        End If

        If OrderEntry_OpenType = 2 Then
            FPc.P.Locked = True
        Else
            FPc.P.Locked = False
        End If

        If Not (FPp_StatusPicture Is Nothing) Then
            Dim PictureCode As String = gl_FPApp.CL_Statuscodes.GET_Status_PictureCode(P_STATUS_ID_CURRENT)

            If PictureCode > "" Then
                PictureCode += ".png"
            End If
            FPp_StatusPicture.Image_Normal = PictureCode

            FPp_StatusPicture.SHOW()
        End If
    End Sub

    Private Sub FPc_Field_Coloring(sender_FPc As FP_Control, ByRef Handled As Boolean) Handles FPc.Field_Coloring
        SET_LAYOUT()
    End Sub

    Private Sub FPc_Field_Marker_Click(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Handled As Boolean) Handles FPc.Field_Marker_Click, FPc.Field_Doubleclick
        FPApp_for_Messages.RAISEEVENT_Marker_Clicked(FPc, "SEL_STATUSCODES", Nothing, False)
    End Sub

    Public Sub CHANGE_STATUS(New_StatusID As Long)
        Dim OUT As Boolean = False

        gl_FPApp.RAISEEVENT_Message("STATUSCODE_CHANGING", FPf, Nothing, False) 'Ezt lekezeli az FP_L_StatusField minden peldanya es ha meg van nem elmentett statuszvaltas, akkor az letrejon.

        If FP_of_StatusField.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            If P_STATUS_ID_CURRENT <> New_StatusID Then
                Dim Binded_OLD As Boolean = FP_of_StatusField.DATA_Binded
                Dim Current_StatusID As Long = FPc.P_ID
                Dim Current_Text As String = FPc.c.Text

                FP_of_StatusField.DATA_Binded = False

                If TypeOf (FPc.c) Is TextBox Then
                    FPc.c.Text = gl_FPApp.CL_Statuscodes.GET_Status_Code_BY_ID(New_StatusID)
                    FPc.Selected_ID = New_StatusID
                ElseIf TypeOf (FPc.c) Is ComboBox Then
                    FPc.P_VALUE = New_StatusID
                End If

                Dim ee As New EventArgs
                Dim Cancel As Boolean = False

                FPc_Field_TextChanged(FPc, FPc.c, ee, Cancel)

                If Cancel = False Then
                    OUT = True
                Else
                    If TypeOf (FPc.c) Is ComboBox Then
                        FPc.P_VALUE = Current_StatusID

                    ElseIf TypeOf (FPc.c) Is TextBox Then
                        FPc.c.Text = Current_Text
                        FPc.Selected_ID = Current_StatusID
                    Else
                        gl_FPApp.DoErrorMsgBox("FP_FieldLogics..FP_L_StatusField.CHANGE_STATUS", 0, "FPc has an unknown fieldtype")
                    End If
                End If

                FP_of_StatusField.DATA_Binded = Binded_OLD
            End If
        End If
    End Sub

    Private Sub FPc_Field_TextChanged(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Cancel As Boolean) Handles FPc.Field_TextChanged
        If FP_of_StatusField.P_DATA_RecordStatus <> ENUM_RecordStatus.EXISTS Then
            Cancel = True
        Else
            Close_All_ErrorForm()

            If FPc.P_ID <> FPc.P_ID_SAVED Then
                gl_FPApp.RAISEEVENT_Message("STATUSCODE_CHANGING", FPf, Nothing, False) 'Ezt lekezeli az FP_L_StatusField minden peldanya es ha meg van nem elmentett statuszvaltas, akkor az letrejon.
                RaiseEvent Status_Changing(Me, Cancel)
                If Cancel = False Then
                    T_START()
                End If
            End If
        End If
    End Sub

    Private Sub T_Tick(sender As Object, e As EventArgs) Handles T.Tick
        T_STOP()

        Dim DoIt As Boolean = True
        Dim Current_StatusID As Long = P_STATUS_ID_CURRENT

        If DoIt Then
            If Current_StatusID = CONST_STATUSCODES_PREV Then
                Dim Prev_StatusID As Long = P_STATUS_ID_PREV
                If Prev_StatusID = 0 Then
                    DoIt = False
                    FP_of_StatusField.FORM_DORESYNC()
                    FP_of_StatusField.FPf.FOCUS_ON_AT_THE_END(FPc.c, 95005) 'Az adatsornak nincs megelozo statusza
                Else
                    Dim Data_Binded_OLD As Boolean = FP_of_StatusField.P_DATA_Binded_ByUser

                    FP_of_StatusField.P_DATA_Binded_ByUser = False

                    If TypeOf (FPc.c) Is ComboBox Or TypeOf (FPc.c) Is ListBox Then
                        FPc.P_VALUE = Prev_StatusID
                    ElseIf TypeOf (FPc.c) Is TextBox Then
                        FPc.P_VALUE = gl_FPApp.CL_Statuscodes.GET_Status_Code_BY_ID(Prev_StatusID)
                        FPc.Selected_ID = Prev_StatusID
                    Else
                        DoIt = False
                        FP_of_StatusField.FORM_DORESYNC()
                        FPc.FPf.DoErrorMsgBox("FP_FieldLogics..FP_L_StatusField.FPc_Field_TextChanged", 0, "Unknown fieldtype")
                    End If

                    FP_of_StatusField.P_DATA_Binded_ByUser = True
                End If
            End If
        End If

        'Dim CHECK_TransactID As Long = gl_FPApp.Tr_getNewTransactID("FP_L_StatusField.T_Tick", FP_of_StatusField.P_DATA_Current_ID)
        Dim CHECK_TransactID As Long = 0

        If DoIt Then
            If CHECK_SELECTED_STATUS_AFTER(CHECK_TransactID) Then
                StatusCodes_GROUPED_CHANGE_DO(CHECK_TransactID)
                If gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_IsInstalled("STATUSCODES_EComm") Then
                    Email_Send()
                End If
            End If
        End If
        SET_LAYOUT()
    End Sub
    Private ReadOnly Property Prop_Send_Via As ENUM_Ecomm_Send_Via
        Get
            '!!!SSS!!! Fixen outlook-ot adok vissza. Ezt a parametert at kellene tenni az emailtemlates-be
            ' vagy legalabbis template alapjan kellene meghatarozni.
            Return ENUM_Ecomm_Send_Via.Via_Outlook
            'Dim NumVal As Integer
            'Dim AlfanumVal As String = ""
            'gl_FPApp.ParmLesen("EC", "SEND_VIA", NumVal, AlfanumVal)
            'Select Case AlfanumVal.ToUpper
            '    Case "OUTLOOK"
            '        Return ENUM_Ecomm_Send_Via.Via_Outlook
            '    Case "SMTP"
            '        Return ENUM_Ecomm_Send_Via.Via_SMTP
            '    Case "MAILTO"
            '        Return ENUM_Ecomm_Send_Via.Via_MAILTO
            '    Case Else
            '        Return ENUM_Ecomm_Send_Via.None
            'End Select
        End Get
    End Property

    Private Sub Email_Send()
        Dim SqlComm As New SqlClient.SqlCommand

        gl_FPApp.DC.Qdf_set_SP(SqlComm, "Get_Email_Body_And_Subject")
        gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Record_ID", SqlDbType.Int, , , , , FP_of_StatusField.P_DATA_Current_ID)
        gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Status_ID", SqlDbType.Int, , , , , P_STATUS_ID_CURRENT)
        gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@OUT_Body", SqlDbType.NVarChar, ParameterDirection.Output, -1)
        gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@OUT_Subj", SqlDbType.NVarChar, ParameterDirection.Output, -1)
        gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Email_TO", SqlDbType.NVarChar, ParameterDirection.Output, -1)
        gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Email_CC", SqlDbType.NVarChar, ParameterDirection.Output, -1)
        gl_FPApp.DC.Qdf_AddParameter(SqlComm, "@Email_BCC", SqlDbType.NVarChar, ParameterDirection.Output, -1)

        If Not gl_FPApp.DC.Qdf_Execute(FPf, SqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE) Then
            Call gl_FPApp.DoErrorMsgBox("FP_L_StatusField.Email_Send", Err.Number, Err.Description)
            Exit Sub
        End If
        Dim Email_TO As String = SqlComm.Parameters("@Email_TO").Value
        Dim Email_CC As String = SqlComm.Parameters("@Email_CC").Value
        Dim Email_BCC As String = SqlComm.Parameters("@Email_BCC").Value
        Dim Email_HTML As String = SqlComm.Parameters("@OUT_Body").Value
        Dim Email_Subject As String = SqlComm.Parameters("@OUT_Subj").Value

        If Email_HTML <> "#_NONE_#" Then
            Dim E As New FP_EMAIL()
            Dim Send_OK As Boolean

            With E
                .P_Email_Subject = Email_Subject
                .P_Email_TO = Email_TO
                .P_Email_CC = Email_CC
                .P_Email_BCC = Email_BCC
                .P_Email_Footer_html = Email_HTML
                .P_Email_Text = Email_HTML

                '!!!SSS!!! mellekletek csatolasat egyelore nem kezelem statusz valtas eseten
                '.P_Email_Attached_Files_ADD(Nothing)
                'If Not Attached_Files Is Nothing Then
                '    For Each P As String In Attached_Files
                '        .P_Email_Attached_Files_ADD(P)
                '    Next
                'End If

                '!!!SSS!!! egyelore csak outook email kuldeset kezelem.
                Send_OK = .EMAIL_SEND_via_OUTLOOK(FP_EMAIL.ENUM_FP_EMAIL_HANDLING.SHOW)
                'Select Case Prop_Send_Via
                '    Case ENUM_Ecomm_Send_Via.Via_Outlook
                '        Send_OK = .EMAIL_SEND_via_OUTLOOK(FP_EMAIL.ENUM_FP_EMAIL_HANDLING.SHOW)
                'Case ENUM_Ecomm_Send_Via.Via_MAILTO
                '    Send_OK = .EMAIL_SEND_via_MAILTO(FP_EMAIL.ENUM_FP_EMAIL_HANDLING.SHOW)
                'Case ENUM_Ecomm_Send_Via.Via_SMTP
                '    .P_SMTP_Params = Get_SMTP_Params(SMTP_Params_OK)
                '    If SMTP_Params_OK Then
                '        If .P_SMTP_Params.Fix_BCC <> String.Empty Then
                '            If E.P_Email_BCC = String.Empty Then
                '                E.P_Email_BCC = .P_SMTP_Params.Fix_BCC
                '            Else
                '                E.P_Email_BCC &= ";" & .P_SMTP_Params.Fix_BCC
                '            End If
                '        End If
                '        Send_OK = .EMAIL_SEND_via_SMTP(FP_EMAIL.ENUM_FP_EMAIL_HANDLING.SHOW)
                '    End If
                'Case Else
                '        gl_FPApp.DoMyMsgBox(FPf, 100000)
                'End Select
            End With

        End If

    End Sub

    Public Sub StatusCodes_GROUPED_CHANGE_DO(MyTransactID As Long)
        Dim sqlComm As SqlCommand = FPc.FPf.FPApp.DC.CNN.CreateCommand()
        Dim Result As Boolean = False

        With FPf.FPApp.DC
            .Qdf_set_SP(sqlComm, "StatusCodes_GROUPED_CHANGE_DO")
            .Qdf_AddParameter(sqlComm, "@Lang", SqlDbType.NVarChar, , 3, gl_FPApp.LandDialog)
            .Qdf_AddParameter(sqlComm, "@Terminals_ID", SqlDbType.Int, , , , , Terminals_ID)
            .Qdf_AddParameter(sqlComm, "@TransactID", SqlDbType.Int, , , , , MyTransactID)
            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
        End With

        CURSOR_SHOW_WAIT()
        Try
            Result = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.NODIALOG, "@ErrText")
            CURSOR_SHOW_DEFAULT()

        Catch ex As Exception
            CURSOR_SHOW_DEFAULT()
            FPc.FPf.FPApp.DoErrorMsgBox("FP_L_StatusField..StatusCodes_GROUPED_CHANGE_DO", Err.Number, Err.Description)
        End Try

        If Result Then
            RaiseEvent Status_Changed(Me)
            gl_FPApp.RAISEEVENT_Message("STATUSCODE_CHANGED", FPf, Nothing, False)
        End If
    End Sub

    Private Sub FPApp_for_Messages_Message(sender As FP_App, From_FPf As FP_Form, MessageCode As String, ByRef Individual_Params As Object, ByRef Handled As Boolean) Handles FPApp_for_Messages.Message
        Select Case MessageCode
            Case "STATUSCODE_CHANGING"
                'Nothing to do

            Case "STATUSCODE_CHANGED"
                If T.Enabled = False Then
                    If FPf.Equals(From_FPf) Then

                        FP_of_StatusField.FORM_DORESYNC(, , True) 'Azert ne update-elje a child FP-ket, mert ha nem ez a statusfield kuldte a message-t, hanem a child FP-n levo, akkor elmegy a child FP a rekordrol
                    End If
                End If

            Case Else
                'Nothing to do
        End Select
    End Sub

    Private Sub c_of_StatusField_Enter(sender As Object, e As EventArgs) Handles c_of_StatusField.Enter
        If Not FPf.SAVE_ALL() Then
            If FPc.Equals(FPf.P_ActiveControl) Then
                'FP_of_StatusField.FORM_FOCUS_ON_AT_THE_END(FP_of_StatusField.CONTROLS_GET_FIRST_FPc.c)
            End If
        End If
    End Sub

    Private Sub FP_Parent_of_StatusField_Form_Current(sender_FP As FP) Handles FP_Parent_of_StatusField.Form_Current
        SET_LAYOUT()
    End Sub

    Private Sub FP_Parent_of_StatusField_Form_NoRecord(sender_FP As FP) Handles FP_Parent_of_StatusField.Form_NoRecord
        SET_LAYOUT()
    End Sub

    Private Sub FPp_StatusChange_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPp_StatusChange.CLICK
        If FP_of_StatusField.FPf.SAVE_ALL Then
            Dim P As New Struct_Simple_SELECT_Params
            Dim P_OUT As New Struct_Simple_SELECT_OutputParams

            With P
                .FixText_Key = FPc.P.DT_FixText_Key
                .Field_Mandatory = True

                .SQL_WHERE = String.Format("StatusID IN (-1, {0}) AND StatusChange_Type IN ({1}, {2})", FPc.P_ID, CInt(CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.MANUAL), CInt(CL_Statuscodes.ENUM_STATUS_CHANGE_TYPE.MANUAL_WITH_CONFIRM))
            End With

            If gl_FPApp.SIMPLE_SELECT(P, P_OUT) Then
                CHANGE_STATUS(P_OUT.Selected_ID)
            End If
        End If
    End Sub

    Private Sub FPf_FORM_CLOSING(sender As Object, ByRef e As FormClosingEventArgs) Handles FPf.FORM_CLOSING
        Dispose_Me()
    End Sub
End Class

Public Class FP_L_Listbox_with_Checkboxes
    Public Structure Struct_FP_L_Listbox_with_Checkboxes_Params
        Dim FPc As FP_Control
        Dim Dirty_Handling As Boolean
    End Structure

    Public FPc As FP_Control
    Public WithEvents c_of_FPc As ListView
    Private Dirty_Handling As Boolean
    Private Dirty_Handling_System As Boolean
    Private Dirty_Handling_User As Boolean

    Sub New(P As Struct_FP_L_Listbox_with_Checkboxes_Params)
        With P
            FPc = .FPc
            c_of_FPc = FPc.c
            Dirty_Handling = .Dirty_Handling
        End With

        Dirty_Handling_System = False
        Dirty_Handling_User = True
    End Sub

    Public Property P_Dirty_Handling As Boolean
        Get
            Return (Dirty_Handling And Dirty_Handling_System And Dirty_Handling_User)
        End Get
        Set(value As Boolean)
            Dirty_Handling_User = value
        End Set
    End Property

    Public Sub SET_CHECKBOXES_FROM_DT(DT As DataTable, Optional Name_of_ID_Field As String = "ID", Optional Name_of_Checked_Value_ID As String = "Checked_Value")
        If DT Is Nothing Then
            Exit Sub
        End If

        Dim Dirty_Handling_System_OLD = Dirty_Handling_System

        Dirty_Handling_System = False

        For Each CurrentItem As ListViewItem In c_of_FPc.Items
            Dim Crit As String = String.Format("{0}={1}", Name_of_ID_Field, CurrentItem.Name)

            If DT.Select(Crit).Count = 1 Then
                CurrentItem.Checked = DT.Select(Crit).First.Item(Name_of_Checked_Value_ID)
            Else
                CurrentItem.Checked = False
            End If
        Next

        Dirty_Handling_System = Dirty_Handling_System_OLD
    End Sub

    Public Sub SET_CHECKBOXES_FROM_STR(Checked_Row_IDs As String, Optional Delimiter As String = "|")
        Dim IDs() As String = Split(Checked_Row_IDs, Delimiter)
        Dim ListOfIDs As List(Of String) = IDs.ToList
        Dim Dirty_Handling_System_OLD = Dirty_Handling_System

        Dirty_Handling_System = False

        For Each CurrentItem As ListViewItem In c_of_FPc.Items
            CurrentItem.Checked = ListOfIDs.Contains(CurrentItem.Name)
        Next

        Dirty_Handling_System = Dirty_Handling_System_OLD
    End Sub

    Public Function GET_CHECKED_CHECKBOXES_INTO_STR(Optional Delimiter As String = "|") As String
        Dim OUT As String = ""
        Dim Current_Delimiter As String = ""

        For Each CurrentItem As ListViewItem In c_of_FPc.Items
            If CurrentItem.Checked Then
                OUT += Current_Delimiter + CurrentItem.Name
                Current_Delimiter = Delimiter
            End If
        Next

        Return OUT
    End Function

    Private Sub c_of_FPc_GotFocus(sender As Object, e As EventArgs) Handles c_of_FPc.GotFocus
        Dirty_Handling_System = True
    End Sub

    Private Sub c_of_FPc_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles c_of_FPc.ItemCheck
        If P_Dirty_Handling Then
            'Ez a bena esemeny akkor is fellep, ha a Listbox egy TabPage-en van es az eppen lathato lesz.
            'Ezert a dirty akkor kell, hogy fellepjen, ha tenylegesen valtozas van.
            'Abbol indulok ki, hogy a Listbox checkbox mezoinek aktualis erteket fel kell tolteni vagy a SET_CHECKBOXES_FROM_STR
            'vagy a SET_CHECKBOXES_FROM_DT eljarassal, amelyek beallitja a Dirty_Control_STR-t es ezzel ossze lehet hasonlitani, a
            'pillanatnyit, hogy tortent-e valtozas.
            'Egyebkent ez a benasag meg azt sem tudja, hogy Cancel. Szoval ha fontos,
            'hogy valamit ne lehessen valtoztatni, akkor arrol ugy kell gondoskodni, hogy a Listbox-ot letiltod.

            FPc.FP.FORM_DIRTY_SET()
        End If
    End Sub

    Private Sub c_of_FPc_LostFocus(sender As Object, e As EventArgs) Handles c_of_FPc.LostFocus
        Dirty_Handling_System = False
    End Sub
End Class

Public Class FP_L_MAP
    Public Enum ENUM_MapProvider As Integer
        TOMTOM = 1
    End Enum

    Public Structure Struct_FP_L_Map_Location
        Dim RecordID As Integer
        Dim ParentTable As String
        Dim Name As String
        Dim Country As String
        Dim ZIP As String
        Dim City As String
        Dim Addr_InOneField As String
        Dim Addr As String
        Dim District As String
        Dim Addr_ps_type As String
        Dim Addr_housenr As String
        Dim Addr_building As String
        Dim Addr_stairway As String
        Dim Addr_floor As String
        Dim Addr_door As String
    End Structure

    Private FPf As FP_Form
    Private Provider As ENUM_MapProvider

    Public Property Search_Result_JSON As String
    Public Property Routing_Result_JSON As String
    Public Property RequestString As String = ""
    Public Property ResponseMessage As String = ""

    Public Sub New(ByVal P_FPf As FP_Form)
        FPf = P_FPf

        If gl_FPApp.Installed_Products_Exists("MAP",, "TOMTOM") Then
            Provider = ENUM_MapProvider.TOMTOM
        End If

        If Provider = Nothing Then
            Throw New Exception("FP_L_MAP.New: INVALID MAP PROVIDER")
        End If
    End Sub

    Public Function GET_LOCATION(ByVal P As Struct_FP_L_Map_Location, Optional Map_P As String = "") As Boolean
        If Provider = ENUM_MapProvider.TOMTOM Then
            If P.Addr_InOneField = "" Then
                Dim Addresses_Elements As Struct_Addresses_Elements = Nothing

                With Addresses_Elements
                    .Addr = P.Addr
                    .Spec_Addr = ""
                    .District = P.District
                    .Addr_ps_type = P.Addr_ps_type
                    .Addr_housenr = P.Addr_housenr
                    .Addr_building = P.Addr_building
                    .Addr_stairway = P.Addr_stairway
                    .Addr_floor = P.Addr_floor
                    .Addr_door = P.Addr_door
                End With
                P.Addr_InOneField = Addresses_get_Full_Addr_from_Elements(Addresses_Elements)
            End If

            Dim request_params As FP_Map_TomTom.Struct_Request_Params
            With request_params
                .SelID = gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_GET_PARAM("TOMTOM_DEVELOPMENT", "ID")
                .Provider = Provider.ToString
                If Map_P = "" Then
                    .Action = "API:SEARCH"
                Else
                    .Action = "MAP:SEARCH"
                End If
                .Terminal = Terminal
                .UserID = SelUser
            End With

            Dim search_params As FP_Map_TomTom.Struct_Search_Params
            With search_params
                .Country = P.Country
                .ZIP = P.ZIP
                .City = P.City
                .Addr = P.Addr_InOneField
                .PopupTitle = ""
                .PopupHead = P.Name
                .PopupBody = String.Format("{0} {1} {2} {3}", P.Country, P.ZIP, P.City, P.Addr_InOneField)
            End With

            Dim map As New FP_Map_TomTom()

            If map.SEARCH(gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_GET_PARAM("TOMTOM_DEVELOPMENT", "REQUEST_URL"), request_params, search_params, Map_P) Then
                Search_Result_JSON = map.Search_Result_JSON
            Else
                GET_LOCATION = False
                Exit Function
            End If
        End If

        GET_LOCATION = True
    End Function

    Public Function GET_DISTANCE(ByVal P As List(Of FP_Map_TomTom.Struct_Routing_Params), Optional Map_P As String = "") As Boolean
        If Provider = ENUM_MapProvider.TOMTOM Then
            Dim request_params As FP_Map_TomTom.Struct_Request_Params
            With request_params
                .SelID = gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_GET_PARAM("TOMTOM_DEVELOPMENT", "ID")
                .Provider = Provider.ToString
                If Map_P = "" Then
                    .Action = "API:ROUTING"
                Else
                    .Action = "MAP:ROUTING"
                End If
                .Terminal = Terminal
                .UserID = SelUser
            End With

            Dim map As New FP_Map_TomTom()

            If map.ROUTING(gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_GET_PARAM("TOMTOM_DEVELOPMENT", "REQUEST_URL"), request_params, P, Map_P) Then
                Routing_Result_JSON = map.Routing_Result_JSON
            End If
        End If

        GET_DISTANCE = True
    End Function

    Public Function GET_LOCATION_FROM_CUST(ByVal ParentTable As String, ByVal RecordID As Integer, ByVal Cust_ID As Integer) As Boolean
        Dim DT As DataTable = Nothing
        Dim MySQL As String = String.Format("SELECT Location_Longitude, Location_Latitude, Location_Address FROM Cegek WHERE ID={0}", Cust_ID)
        gl_FPApp.DC.Qdf_Fill_DT(MySQL, DT)

        If DT.Rows.Count = 0 Then
            GET_LOCATION_FROM_CUST = False
            Exit Function
        End If

        Search_Result_JSON = String.Format("""lat"":""{0}"",""lon"":""{1}"",""freeformAddress"":""{2}""", DT.Rows(0)("Location_Latitude"), DT.Rows(0)("Location_Longitude"), DT.Rows(0)("Location_Address").ToString.Replace(",", " "))
        Search_Result_JSON = "{" + Search_Result_JSON + "}"

        GET_LOCATION_FROM_CUST = True
    End Function
End Class
Public Class FP_L_Map_Panel
    Public Structure Struct_FP_L_Map_Panel
        Dim Map_Panel As Panel
        Dim Fieldprefix As String
        Dim ParentTable As ENUM_ParentTable
        Dim ParentFP As FP
        Dim Name As TextBox
        Dim Country As TextBox
        Dim ZIP As TextBox
        Dim Place As TextBox
        Dim Addr As TextBox
        Dim District As TextBox
        Dim Addr_ps_type As TextBox
        Dim Addr_housenr As TextBox
        Dim Addr_building As TextBox
        Dim Addr_stairway As TextBox
        Dim Addr_floor As TextBox
        Dim Addr_door As TextBox
    End Structure

    Public Enum ENUM_ParentTable As Integer
        ADDRESSES = 1
        CUST = 2
    End Enum

    Public Event SET_MAP_SETTINGS(ByRef MapSettings As String)

    Private Map_Panel As Panel
    Private Fieldprefix As String
    Private ParentTable As ENUM_ParentTable
    Private WithEvents ParentFP As FP
    Private Name As TextBox
    Private Country As TextBox
    Private ZIP As TextBox
    Private Place As TextBox
    Private Addr As TextBox
    Private District As TextBox
    Private Addr_ps_type As TextBox
    Private Addr_housenr As TextBox
    Private Addr_building As TextBox
    Private Addr_stairway As TextBox
    Private Addr_floor As TextBox
    Private Addr_door As TextBox

    Private WithEvents FPf As FP_Form
    Private WithEvents FP_MAP As FP

    Private Title_Label As Label
    Private Latitude_Label As Label
    Private Latitude As TextBox
    Private Longitude_Label As Label
    Private Longitude As TextBox
    Private Address_Label As Label
    Private Address As TextBox
    Private WithEvents BTN_getLocation As Button
    Private WithEvents BTN_clearLocation As Button
    Private WithEvents BTN_ShowOnMap As Button

    Private WithEvents FPc_Location_Latitude As FP_Control
    Private WithEvents FPc_Location_Longitude As FP_Control
    Private WithEvents FPc_Location_Address As FP_Control

    Private Const Control_Prefix = "Location_"

    Public Sub New(ByVal P As Struct_FP_L_Map_Panel)
        With P
            Map_Panel = .Map_Panel
            Fieldprefix = .Fieldprefix
            ParentTable = .ParentTable
            ParentFP = .ParentFP
            Name = .Name
            Country = .Country
            ZIP = .ZIP
            Place = .Place
            Addr = .Addr
            District = .District
            Addr_ps_type = .Addr_ps_type
            Addr_housenr = .Addr_housenr
            Addr_building = .Addr_building
            Addr_stairway = .Addr_stairway
            Addr_floor = .Addr_floor
            Addr_door = .Addr_door
        End With

        FPf = ParentFP.FPf
        FP_MAP = New FP(FPf, String.Format("SEL_MAP_{0}", ParentTable.ToString))

        CREATE_CONTROLS()

        Dim FP_MAP_CONTROLS As New Struct_FP_CONTROLS_COLLECTION
        With FP_MAP_CONTROLS
            .FieldPrefix = Fieldprefix
        End With

        With FP_MAP
            .SQL_BIND_Params.NameOf_DEL = ""
        End With

        FP_MAP.INIT_CONTROLS(FP_MAP_CONTROLS)
    End Sub

    Private Sub CREATE_CONTROLS()
        Title_Label = New Label
        With Title_Label
            .Name = Control_FieldPrefix_And_Prefix() + "Title_Label"
            .Visible = True
            .Width = 20
            .BackColor = Color.DimGray
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            .Size = New Size(120, 22)
            .TextAlign = ContentAlignment.MiddleCenter
        End With
        Map_Panel.Controls.Add(Title_Label)

        Latitude_Label = New Label
        With Latitude_Label
            .Name = Control_FieldPrefix_And_Prefix() + "Latitude_Label"
            .Visible = True
            .Width = 20
            .BackColor = Color.DimGray
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            .Size = New Size(120, 22)
            .TextAlign = ContentAlignment.MiddleLeft
        End With
        Map_Panel.Controls.Add(Latitude_Label)

        Latitude = New TextBox
        With Latitude
            .Name = Control_FieldPrefix_And_Prefix() + "Latitude"
            .BackColor = Color.White
            .Width = 207
            .SendToBack()
            .Visible = True
        End With
        Map_Panel.Controls.Add(Latitude)

        Longitude_Label = New Label
        With Longitude_Label
            .Name = Control_FieldPrefix_And_Prefix() + "Longitude_Label"
            .Visible = True
            .Width = 20
            .BackColor = Color.DimGray
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            .Size = New Size(120, 22)
            .TextAlign = ContentAlignment.MiddleLeft
        End With
        Map_Panel.Controls.Add(Longitude_Label)

        Longitude = New TextBox
        With Longitude
            .Name = Control_FieldPrefix_And_Prefix() + "Longitude"
            .BackColor = Color.White
            .Width = 207
            .SendToBack()
            .Visible = True
        End With
        Map_Panel.Controls.Add(Longitude)

        Address_Label = New Label
        With Address_Label
            .Name = Control_FieldPrefix_And_Prefix() + "Address_Label"
            .Visible = True
            .Width = 20
            .BackColor = Color.DimGray
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            .Size = New Size(120, 22)
            .TextAlign = ContentAlignment.MiddleLeft
        End With
        Map_Panel.Controls.Add(Address_Label)

        Address = New TextBox
        With Address
            .Name = Control_FieldPrefix_And_Prefix() + "Address"
            .BackColor = Color.White
            .Width = 207
            .SendToBack()
            .Visible = True
        End With
        Map_Panel.Controls.Add(Address)

        BTN_getLocation = New Button
        With BTN_getLocation
            .Name = "BTN_getLocation"
            .Size = New Size(120, 30)
        End With
        Map_Panel.Controls.Add(BTN_getLocation)

        BTN_clearLocation = New Button
        With BTN_clearLocation
            .Name = "BTN_clearLocation"
            .Size = New Size(120, 30)
        End With
        Map_Panel.Controls.Add(BTN_clearLocation)

        BTN_ShowOnMap = New Button
        With BTN_ShowOnMap
            .Name = "BTN_ShowOnMap"
            .Size = New Size(120, 30)
        End With
        Map_Panel.Controls.Add(BTN_ShowOnMap)

        With FPf
            .CONTROLS_ADD(Title_Label)
            .CONTROLS_ADD(Latitude_Label)
            .CONTROLS_ADD(Latitude)
            .CONTROLS_ADD(Longitude_Label)
            .CONTROLS_ADD(Longitude)
            .CONTROLS_ADD(Address_Label)
            .CONTROLS_ADD(Address)
            .CONTROLS_ADD(BTN_getLocation)
            .CONTROLS_ADD(BTN_clearLocation)
            .CONTROLS_ADD(BTN_ShowOnMap)
        End With
    End Sub
    Private Function Control_FieldPrefix_And_Prefix() As String
        Dim OUT As String = Control_Prefix

        If Fieldprefix > "" Then
            OUT = String.Format("{0}_{1}", Fieldprefix, OUT)
        End If

        Return OUT
    End Function

    Private Sub ParentFP_Form_Current(sender_FP As FP) Handles ParentFP.Form_Current
        SET_RECORDSOURCE()
    End Sub

    Private Sub ParentFP_Form_NoRecord(sender_FP As FP) Handles ParentFP.Form_NoRecord
        SET_RECORDSOURCE()
    End Sub

    Private Sub SET_RECORDSOURCE()
        If Not FP_MAP.FORM_RECORDS_LOAD(String.Format("ParentID={0}", ParentFP.P_DATA_Current_ID),, True) Then
            MsgBox("")
        End If
    End Sub

    Private Sub BTN_getLocation_Click(sender As Object, e As EventArgs) Handles BTN_getLocation.Click
        SET_LOCATION()
    End Sub

    Private Sub FP_MAP_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_MAP.CONTROLS_INITIALIZED
        With FP_MAP
            FPc_Location_Latitude = .CONTROLS("Location_Latitude")
            FPc_Location_Longitude = .CONTROLS("Location_Longitude")
            FPc_Location_Address = .CONTROLS("Location_Address")
        End With
    End Sub

    Private Sub ParentFP_Form_AfterUpdate(sender_FP As FP) Handles ParentFP.Form_AfterUpdate
        FP_MAP.FORM_RECORDS_SAVE_CURRENT()
    End Sub

    Private Sub BTN_ShowOnMap_Click(sender As Object, e As EventArgs) Handles BTN_ShowOnMap.Click
        Dim URL As String = ""
        If gl_FPApp.Installed_Products_Exists("MAP",, "TOMTOM") Then
            If gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_IsInstalled("TOMTOM_DEVELOPMENT") Then
                If Not ParentFP.FORM_RECORDS_SAVE_CURRENT Then
                    Exit Sub
                End If
            End If
        End If

        Dim MapSettings As String = ""

        RaiseEvent SET_MAP_SETTINGS(MapSettings)

        If MapSettings = "" Then
            Exit Sub
        End If

        If SET_LOCATION(MapSettings, URL) Then
            Dim map As New FP_Map(URL)
            map.Show()
        End If
    End Sub

    Private Sub BTN_clearLocation_Click(sender As Object, e As EventArgs) Handles BTN_clearLocation.Click
        If FP_MAP.FORM_DIRTY_SET = True Then
            FPc_Location_Longitude.P_VALUE = 0
            FPc_Location_Latitude.P_VALUE = 0
            FPc_Location_Address.P_VALUE = ""

            FP_MAP.FORM_RECORDS_SAVE_CURRENT()
        End If
    End Sub

    Private Function SET_LOCATION(Optional MAP_P As String = "", Optional ByRef URL As String = "") As Boolean
        If ParentFP.FORM_RECORDS_SAVE_CURRENT = False Then
            SET_LOCATION = False
            Exit Function
        End If

        Dim P As New FP_L_MAP.Struct_FP_L_Map_Location
        With P
            .RecordID = ParentFP.P_DATA_Current_ID
            .ParentTable = ParentTable.ToString
            .Name = Name.Text
            .Country = Country.Text
            .ZIP = ZIP.Text
            .City = Place.Text
            .Addr = Addr.Text
            .District = District.Text
            .Addr_ps_type = Addr_ps_type.Text
            .Addr_housenr = Addr_housenr.Text
            .Addr_building = Addr_building.Text
            .Addr_stairway = Addr_stairway.Text
            .Addr_floor = Addr_floor.Text
            .Addr_door = Addr_door.Text
        End With

        Dim MAP As New FP_L_MAP(FPf)
        If MAP.GET_LOCATION(P, MAP_P) = True Then
            If FP_MAP.FORM_DIRTY_SET = True Then
                Try
                    Dim obj As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(MAP.Search_Result_JSON)
                    FPc_Location_Longitude.P_VALUE = Double.Parse(obj("lon").ToString)
                    FPc_Location_Latitude.P_VALUE = Double.Parse(obj("lat").ToString)
                    FPc_Location_Address.P_VALUE = String.Format("{0} ({1})", obj("freeformAddress").ToString, obj("type").ToString)

                    URL = obj("URL").ToString

                Catch ex As Exception

                Finally
                    FP_MAP.FORM_RECORDS_SAVE_CURRENT()

                End Try

            End If
        End If

        SET_LOCATION = True
    End Function

    Public Function LOAD_LOCAL_DATA(ByVal Cust_ID As Integer, Optional WithOutSaveRecord As Boolean = False) As Boolean
        If ParentTable <> ENUM_ParentTable.CUST Then
            Dim MAP As New FP_L_MAP(FPf)

            If FP_MAP.FORM_DIRTY_SET = True Then
                If MAP.GET_LOCATION_FROM_CUST(ParentTable.ToString, ParentFP.P_DATA_Current_ID, Cust_ID) Then
                    Try
                        Dim obj As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(MAP.Search_Result_JSON)
                        FPc_Location_Longitude.P_VALUE = Double.Parse(obj("lon").ToString)
                        FPc_Location_Latitude.P_VALUE = Double.Parse(obj("lat").ToString)
                        FPc_Location_Address.P_VALUE = obj("freeformAddress").ToString

                    Catch ex As Exception

                    Finally
                        If WithOutSaveRecord = False Then
                            FP_MAP.FORM_RECORDS_SAVE_CURRENT()
                        End If

                    End Try
                End If
            End If
        End If
    End Function
End Class

Public Class FP_L_Quick_Select_Panel
    Public WithEvents FP_ORD_QS As FP
    Dim FP_ORD_QS_Controls As New Struct_FP_CONTROLS_COLLECTION

    Public Event No_Records_Found(P As P_Search_Fields, ByRef Cancel As Boolean)

    Private WithEvents My_Panel As Panel
    Private WithEvents Label_Panel As New Label
    Private My_FP As FP
    Private My_FpF As FP_Form
    Private My_FixText_Code As String
    Private My_Prefix As String
    Private My_ListView As ListView
    Private Dic_FieldDefinitions As Dictionary(Of String, String)
    ReadOnly Control_Height As Integer = 22
    ReadOnly Label_Width_Percent As Integer = 50

    Private WithEvents Search_Field0 As TextBox
    Private WithEvents Search_Field1 As TextBox
    Private WithEvents Search_Field2 As TextBox
    Private WithEvents Search_Field3 As TextBox
    Private WithEvents Search_Field4 As TextBox
    Private Search_Field0_Label As Label
    Private Search_Field1_Label As Label
    Private Search_Field2_Label As Label
    Private Search_Field3_Label As Label
    Private Search_Field4_Label As Label
    Public WithEvents FPC_Search_Field0 As FP_Control
    Public WithEvents FPC_Search_Field1 As FP_Control
    Public WithEvents FPC_Search_Field2 As FP_Control
    Public WithEvents FPC_Search_Field3 As FP_Control
    Public WithEvents FPC_Search_Field4 As FP_Control
    Dim Lst_TextBox As New List(Of TextBox)
    Dim Lst_Label As New List(Of Label)
    Dim Dic_Fpc As New Dictionary(Of Integer, FP_Control)
    Private WithEvents BTN_Clear_Select As New PictureBox
    Private WithEvents FPP_BTN_Clear_Select As FP_PictureBox

    Public Structure Struct_FPL_Quick_Select_Panel
        Dim Prefix As String
        Dim Fpf As FP_Form
        Dim FP_Parent As FP                     'Az FP, amire a form legfelso szintje epeul
        Dim PanelControl As Panel
        Dim FixText_Code As String
        Dim LV As ListView
    End Structure
    Public Structure P_Search_Fields
        Dim Search_Field0 As FP_Control
        Dim Search_Field1 As FP_Control
        Dim Search_Field2 As FP_Control
        Dim Search_Field3 As FP_Control
        Dim Search_Field4 As FP_Control
    End Structure

    Public ReadOnly Property Max_Record_Count As Integer
        Get
            Dim OUT As Integer = 50
            If Dic_FieldDefinitions.ContainsKey("MAX_RECORD_COUNT") Then
                If IsNumeric(Dic_FieldDefinitions("MAX_RECORD_COUNT")) Then
                    OUT = Dic_FieldDefinitions("MAX_RECORD_COUNT")
                End If
            End If
            Return OUT
        End Get
    End Property
    Public ReadOnly Property ID_Field_Name As String
        Get
            Dim OUT As String = ""
            If Dic_FieldDefinitions.ContainsKey("ID_FIELD_NAME") Then
                OUT = Dic_FieldDefinitions("ID_FIELD_NAME")
            End If
            Return OUT
        End Get
    End Property
    Public Sub New(S As Struct_FPL_Quick_Select_Panel)
        My_Prefix = S.Prefix
        My_Panel = S.PanelControl
        My_FP = S.FP_Parent
        My_FpF = S.Fpf
        My_FixText_Code = S.FixText_Code
        My_ListView = S.LV
    End Sub
    Private Sub Set_Panel()
        Dim I As Integer

        Search_Field0 = New TextBox
        Search_Field1 = New TextBox
        Search_Field2 = New TextBox
        Search_Field3 = New TextBox
        Search_Field4 = New TextBox

        Search_Field0.Name = "Search_Field0"
        Search_Field1.Name = "Search_Field1"
        Search_Field2.Name = "Search_Field2"
        Search_Field3.Name = "Search_Field3"
        Search_Field4.Name = "Search_Field4"

        Search_Field0_Label = New Label
        Search_Field1_Label = New Label
        Search_Field2_Label = New Label
        Search_Field3_Label = New Label
        Search_Field4_Label = New Label

        Search_Field0_Label.Name = "Search_Field0_Label"
        Search_Field1_Label.Name = "Search_Field1_Label"
        Search_Field2_Label.Name = "Search_Field2_Label"
        Search_Field3_Label.Name = "Search_Field3_Label"
        Search_Field4_Label.Name = "Search_Field4_Label"

        Lst_TextBox.Add(Search_Field0)
        Lst_TextBox.Add(Search_Field1)
        Lst_TextBox.Add(Search_Field2)
        Lst_TextBox.Add(Search_Field3)
        Lst_TextBox.Add(Search_Field4)
        Lst_Label.Add(Search_Field0_Label)
        Lst_Label.Add(Search_Field1_Label)
        Lst_Label.Add(Search_Field2_Label)
        Lst_Label.Add(Search_Field3_Label)
        Lst_Label.Add(Search_Field4_Label)
        Dic_Fpc.Add(0, FPC_Search_Field0)
        Dic_Fpc.Add(1, FPC_Search_Field1)
        Dic_Fpc.Add(2, FPC_Search_Field2)
        Dic_Fpc.Add(3, FPC_Search_Field3)
        Dic_Fpc.Add(4, FPC_Search_Field4)

        My_FpF.CONTROLS_ADD(BTN_Clear_Select)
        My_FpF.CONTROLS_ADD(Label_Panel)

        For I = 0 To 4
            Dim Search_Field_Name As String
            Dim J As Integer = I + 1
            Search_Field_Name = String.Format("SEARCH_FIELD{0}", I)
            If Dic_FieldDefinitions(Search_Field_Name) <> String.Empty Then
                Lst_TextBox(I) = New TextBox
                Lst_Label(I) = New Label

                With Lst_Label(I)
                    .BackColor = System.Drawing.SystemColors.ControlDark
                    .Location = New System.Drawing.Point(0, (Control_Height * J) + J)
                    .Name = String.Format("Search_Field{0}_Label", I)
                    .Size = New System.Drawing.Size(My_Panel.Width * (Label_Width_Percent / 100), Control_Height)
                    .TabIndex = 0
                    .TabStop = False
                    .Text = String.Format("Search field {0}:", I)
                End With

                With Lst_TextBox(I)
                    .Location = New System.Drawing.Point(My_Panel.Width * (Label_Width_Percent / 100) + 1, (Control_Height * J) + J)
                    .Multiline = True
                    .Name = String.Format("Search_Field{0}", I)
                    .Size = New System.Drawing.Size((My_Panel.Width * ((100 - Label_Width_Percent) / 100)) - 1, Control_Height)
                    .TabIndex = I
                    .TabStop = True
                End With
                My_Panel.Controls.Add(Lst_TextBox(I))
                My_Panel.Controls.Add(Lst_Label(I))

                Dim Fp_C As New FP_Control(False)
                Dim Props As New Struct_FP_CONTROL_PROPS
                With Fp_C
                    .FieldName = Lst_TextBox(I).Name
                    .c_Label = Lst_Label(I)
                    .c = Lst_TextBox(I)
                    .FP = My_FP
                    .FPf = My_FpF
                    My_FpF.CONTROLS_ADD(Lst_TextBox(I))
                    My_FpF.CONTROLS_ADD(Lst_Label(I))
                End With
            End If
        Next
    End Sub
    Public Sub INIT()

        My_Panel.Controls.Add(Label_Panel)
        My_Panel.Controls.Add(BTN_Clear_Select)
        With Label_Panel
            .Name = "Label_Panel"
            .AutoSize = False
            .Dock = DockStyle.Top
            .Height = 22
            .BackColor = Color.DimGray
        End With
        With BTN_Clear_Select
            .Width = 44
            .Height = 44
            .Name = "BTN_Clear_Select"
        End With

        Dim FixText As String
        FixText = My_FpF.FPApp.getFixText(My_FixText_Code)
        If FixText = String.Empty Then
            If gl_FPApp.Is_DEBUG_MODE Then
                Label_Panel.Text = "please define the search panel [double click to here!]"
            Else
                Label_Panel.Text = "search panel is not definied"
            End If
        Else
            Dim Control_Definition As String = gl_FPApp.getFixText(My_FixText_Code)
            Dic_FieldDefinitions = New Dictionary(Of String, String)
            If gl_FPApp.FIXTEXT_SPLIT_PARAMS(FixText, Dic_FieldDefinitions) Then
                Set_Panel()

                FP_ORD_QS = New FP(My_FpF, My_Prefix, "", True)
                With FP_ORD_QS.SQL_BIND_Params
                    .NameOf_READ = ""
                    .NameOf_DEL = ""
                    .NameOf_SAVE = ""
                    .NameOf_WhereQuery = ""
                    .NameOf_GRID = ""
                End With
                With FP_ORD_QS
                    .INIT_CONTROLS(FP_ORD_QS_Controls)
                    .P_FORM_AllowAdditions = False
                    .P_FORM_AllowDeletions = False
                End With

            Else
                If gl_FPApp.Is_DEBUG_MODE Then
                    Label_Panel.Text = "please define the search panel [double click to here!]"
                Else
                    Label_Panel.Text = "search panel is not definied"
                End If
            End If
        End If
    End Sub
    Private Sub FP_ORD_QS_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP_ORD_QS.CONTROLS_INITIALIZED
        With FP_ORD_QS
            Dim I As Integer
            For I = 0 To 4
                If Dic_FieldDefinitions(Lst_TextBox(I).Name.ToUpper) <> String.Empty Then
                    Dic_Fpc(I) = .CONTROLS_GET_FPc(Lst_TextBox(I).Name)
                End If
            Next
            FPC_Search_Field0 = Dic_Fpc(0)
            FPC_Search_Field1 = Dic_Fpc(1)
            FPC_Search_Field2 = Dic_Fpc(2)
            FPC_Search_Field3 = Dic_Fpc(3)
            FPC_Search_Field4 = Dic_Fpc(4)

            FPP_BTN_Clear_Select = .PICTUREBOXES_GET(BTN_Clear_Select.Name)
        End With
    End Sub

    Private Sub Label_Panel_DoubleClick(sender As Object, e As EventArgs) Handles Label_Panel.DoubleClick
        If gl_FPApp.Is_DEBUG_MODE Then
            Dim FixText_Edit As FP_Simple_Edit
            FixText_Edit = New FP_Simple_Edit(gl_FPApp, "FP_L_QUICK_SELECT_" + My_Prefix)
            With FixText_Edit
                .DATAFIELD_ADD("LIST_VIEW", DIC_GET("LIST_VIEW", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("ID_FIELD_NAME", DIC_GET("ID_FIELD_NAME", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("MAX_RECORD_COUNT", DIC_GET("MAX_RECORD_COUNT", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("ORDER_BY_FIELD", DIC_GET("ORDER_BY_FIELD", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("SEARCH_FIELD0", DIC_GET("SEARCH_FIELD0", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("SEARCH_FIELD1", DIC_GET("SEARCH_FIELD1", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("SEARCH_FIELD2", DIC_GET("SEARCH_FIELD2", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("SEARCH_FIELD3", DIC_GET("SEARCH_FIELD3", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("SEARCH_FIELD4", DIC_GET("SEARCH_FIELD4", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("LIST_FIELD1", DIC_GET("LIST_FIELD1", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("LIST_FIELD2", DIC_GET("LIST_FIELD2", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("LIST_FIELD3", DIC_GET("LIST_FIELD3", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("LIST_FIELD4", DIC_GET("LIST_FIELD4", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("LIST_FIELD5", DIC_GET("LIST_FIELD5", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("LIST_FIELD1_Length", DIC_GET("LIST_FIELD1_LENGTH", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("LIST_FIELD2_Length", DIC_GET("LIST_FIELD2_LENGTH", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("LIST_FIELD3_Length", DIC_GET("LIST_FIELD3_LENGTH", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("LIST_FIELD4_Length", DIC_GET("LIST_FIELD4_LENGTH", Dic_FieldDefinitions), , 1024)
                .DATAFIELD_ADD("LIST_FIELD5_Length", DIC_GET("LIST_FIELD5_LENGTH", Dic_FieldDefinitions), , 1024)
            End With
            If gl_FPApp.ShowDialogForm(FixText_Edit) = Windows.Forms.DialogResult.OK Then
                Dim FixText As String = ""
                With FixText_Edit

                    FixText = "LIST_VIEW            = " + .DATAFIELD_GET("LIST_VIEW") + vbCrLf +
                                "MAX_RECORD_COUNT   = " + .DATAFIELD_GET("MAX_RECORD_COUNT") + vbCrLf +
                                "ID_FIELD_NAME   = " + .DATAFIELD_GET("ID_FIELD_NAME") + vbCrLf +
                                "ORDER_BY_FIELD            = " + .DATAFIELD_GET("ORDER_BY_FIELD") + vbCrLf +
                                "SEARCH_FIELD0           = " + .DATAFIELD_GET("SEARCH_FIELD0") + vbCrLf +
                                "SEARCH_FIELD1           = " + .DATAFIELD_GET("SEARCH_FIELD1") + vbCrLf +
                                "SEARCH_FIELD2           = " + .DATAFIELD_GET("SEARCH_FIELD2") + vbCrLf +
                                "SEARCH_FIELD3           = " + .DATAFIELD_GET("SEARCH_FIELD3") + vbCrLf +
                                "SEARCH_FIELD4           = " + .DATAFIELD_GET("SEARCH_FIELD4") + vbCrLf +
                                "LIST_FIELD1             = " + .DATAFIELD_GET("LIST_FIELD1") + vbCrLf +
                                "LIST_FIELD2             = " + .DATAFIELD_GET("LIST_FIELD2") + vbCrLf +
                                "LIST_FIELD3             = " + .DATAFIELD_GET("LIST_FIELD3") + vbCrLf +
                                "LIST_FIELD4             = " + .DATAFIELD_GET("LIST_FIELD4") + vbCrLf +
                                "LIST_FIELD5             = " + .DATAFIELD_GET("LIST_FIELD5") + vbCrLf +
                                "LIST_FIELD1_Length      = " + .DATAFIELD_GET("LIST_FIELD1_Length") + vbCrLf +
                                "LIST_FIELD2_Length      = " + .DATAFIELD_GET("LIST_FIELD2_Length") + vbCrLf +
                                "LIST_FIELD3_Length      = " + .DATAFIELD_GET("LIST_FIELD3_Length") + vbCrLf +
                                "LIST_FIELD4_Length      = " + .DATAFIELD_GET("LIST_FIELD4_Length") + vbCrLf +
                                "LIST_FIELD5_Length      = " + .DATAFIELD_GET("LIST_FIELD5_Length")
                End With
                If My_FixText_Code > "" Then
                    gl_FPApp.FIXTEXT_SAVE_SIMPLE(My_FixText_Code, FixText)
                    ' My_ListView.Refresh()
                End If

                MsgBox("OK")
            Else
                MsgBox("NOK")
            End If
        End If
    End Sub

    Private Function Get_Fpc_Where(MyFpC As FP_Control) As String
        Dim OUT As String = ""
        Dim WHERE_Segment As String = ""
        If Not MyFpC Is Nothing Then

            With MyFpC
                If .P.Tag <> String.Empty Then
                    Dim Params() As String = .P.Tag.Split("|")
                    Dim MyTag As String = Params(0)
                    Dim MyOp As String

                    If MyTag.IndexOf("<=") >= 0 Then
                        MyOp = "<="
                    ElseIf MyTag.IndexOf(">=") >= 0 Then
                        MyOp = ">="
                    ElseIf MyTag.IndexOf(">") >= 0 Then
                        MyOp = ">"
                    ElseIf MyTag.IndexOf("<") >= 0 Then
                        MyOp = "<"
                    ElseIf MyTag.ToUpper.IndexOf("LIKE") >= 0 Then
                        MyOp = "LIKE"
                    ElseIf MyTag.ToUpper.IndexOf(" IN ") >= 0 Then
                        MyOp = "IN"
                    ElseIf MyTag.IndexOf("=") >= 0 Then
                        MyOp = "="
                    Else
                        MyOp = ""
                    End If
                    If MyOp <> String.Empty Then
                        Dim MyTagArr() As String
                        MyTagArr = MyTag.ToUpper.Split(MyOp)

                        Dim FieldName As String = MyTagArr(0).Trim
                        Dim Text_Segment As String = MyTagArr(1).Trim

                        If .P_VALUE.ToString <> String.Empty Then
                            Select Case .P.xType_VB
                                Case ""
                                    WHERE_Segment = GET_FILTER_REPLACE_QUESTIONMARK(Params(0), SQLStr(.P_VALUE), .Selected_ID, SQLStr(.P_VALUE))
                                    If TypeOf (.c) Is ComboBox Then
                                        Text_Segment = Replace(Text_Segment, "?", .c.Text)
                                    Else
                                        Text_Segment = Replace(Text_Segment, "?", .P_VALUE)
                                    End If

                                Case "INT", "BIT"
                                    If TypeOf (.c) Is ListView Then
                                        If .c_ListView.CheckBoxes Then
                                            Dim SelectedIDs As String = .LISTVIEW_GET_CHECKED_IDs_WITH_SEPARATOR
                                            Dim SelectedTexts As String = .LISTVIEW_GET_CHECKED_TEXTS_WITH_SEPARATOR
                                            Dim Selected_QTexts As String = .LISTVIEW_GET_CHECKED_TEXTS_WITH_SEPARATOR(", ", "'")

                                            WHERE_Segment = GET_FILTER_REPLACE_QUESTIONMARK(Params(0), Selected_QTexts, SelectedIDs, SelectedIDs)
                                            Text_Segment = GET_FILTER_REPLACE_QUESTIONMARK(Text_Segment, SelectedTexts, SelectedIDs, SelectedTexts)
                                        Else
                                            WHERE_Segment = GET_FILTER_REPLACE_QUESTIONMARK(Params(0), nz(.c.Text, ""), DBFORMAT_from_OBJECT(.P_VALUE, .c.Name, .P.xType_VB), DBFORMAT_from_OBJECT(.P_VALUE, .c.Name, .P.xType_VB))
                                        End If
                                    Else
                                        WHERE_Segment = GET_FILTER_REPLACE_QUESTIONMARK(Params(0), nz(.c.Text, ""), DBFORMAT_from_OBJECT(.P_VALUE, .c.Name, .P.xType_VB), DBFORMAT_from_OBJECT(.P_VALUE, .c.Name, .P.xType_VB))
                                    End If

                                    If TypeOf (.c) Is ComboBox Then
                                        Text_Segment = Replace(Text_Segment, "?", nz(.c.Text, ""))
                                    ElseIf TypeOf (.c) Is ListView Then

                                    Else
                                        Text_Segment = Replace(Text_Segment, "?", getStrInt(.P_VALUE))
                                    End If

                                Case "FLOAT"
                                    WHERE_Segment = Replace(Params(0), "?(TEXT)", .P_VALUE.ToString)
                                    WHERE_Segment = Replace(WHERE_Segment, "?", .P_VALUE.ToString)
                                    Text_Segment = Replace(Text_Segment, "?(TEXT)", getStrFloat(.P_VALUE))
                                    Text_Segment = Replace(Text_Segment, "?", getStrFloat(.P_VALUE))

                                Case "DATETIME"
                                    Dim MyDate As DateTime = .P_VALUE
                                    If MyDate = NULLDATE Then
                                        'nothing to do
                                    Else
                                        If InStr(Params(0), "?(2359)") > 0 Then
                                            Params(0) = Replace(Params(0), "?(2359)", "?")
                                            MyDate = New DateTime(MyDate.Year, MyDate.Month, MyDate.Day, 23, 59, 59)
                                        End If
                                        WHERE_Segment = Replace(Params(0), "?", SQLDate(MyDate))
                                        Text_Segment = Replace(Text_Segment, "?", getStrDate(MyDate))
                                    End If

                                Case Else
                                    OUT = False
                                    WHERE_Segment = ""
                                    Text_Segment = ""
                                    gl_FPApp.DoErrorMsgBox("FP_DoFilter.GET_FILTER", 0, String.Format("Unknown xType_VB '{1}' for control '{0}'", .c.Name, .P.xType_VB))
                            End Select
                        End If
                    End If
                End If
            End With
        End If
        OUT = WHERE_Segment
        Return OUT
    End Function
    Private Sub ReLoad_Form()
        Dim Select_SQL As String = "SELECT TOP #MAX_RECORD_COUNT# #ID_FIELD_NAME# FROM #WHEREQUERY# #WHERE# #WHEREDEFINITION# GROUP BY #ID_FIELD_NAME#"
        Dim Where_Query_Name As String = My_FP.SQL_BIND_Params.NameOf_WhereQuery
        Dim WHERE_Segment As String = ""
        Dim s_AND As String = ""
        Dim S As String = Get_Fpc_Where(FPC_Search_Field0)
        If S <> String.Empty Then
            S = String.Format("({0})", S)
            WHERE_Segment &= s_AND & S
            s_AND = " AND "
        End If

        S = Get_Fpc_Where(FPC_Search_Field1)
        If S <> String.Empty Then
            S = String.Format("({0})", S)
            WHERE_Segment &= s_AND & S
            s_AND = " AND "
        End If

        S = Get_Fpc_Where(FPC_Search_Field2)
        If S <> String.Empty Then
            S = String.Format("({0})", S)
            WHERE_Segment &= s_AND & S
            s_AND = " AND "
        End If

        S = Get_Fpc_Where(FPC_Search_Field3)
        If S <> String.Empty Then
            S = String.Format("({0})", S)
            WHERE_Segment &= s_AND & S
            s_AND = " AND "
        End If

        S = Get_Fpc_Where(FPC_Search_Field4)
        If S <> String.Empty Then
            S = String.Format("({0})", S)
            WHERE_Segment &= s_AND & S
        End If

        Select_SQL = Select_SQL.Replace("#WHEREDEFINITION#", WHERE_Segment)
        If WHERE_Segment = String.Empty Then
            Select_SQL = Select_SQL.Replace("#WHERE#", "")
        Else
            Select_SQL = Select_SQL.Replace("#WHERE#", " WHERE ")
        End If
        Select_SQL = Select_SQL.Replace("#WHEREQUERY#", Where_Query_Name)
        Select_SQL = Select_SQL.Replace("#MAX_RECORD_COUNT#", Max_Record_Count)
        Select_SQL = Select_SQL.Replace("#ID_FIELD_NAME#", ID_Field_Name)

        Dim SelectDT As DataTable = Nothing
        gl_FPApp.DC.Qdf_Fill_DT(Select_SQL, SelectDT)

        If SelectDT.Rows.Count > 0 Then
            Dim Form_Sub_WHERE As String = "#ID_FIELD_NAME# IN (#IDS#)"
            Dim IDS As String = ""
            Dim V As String = ""
            Dim First_ID As Integer = SelectDT.Rows(0).Item(ID_Field_Name)
            Dim DRow As DataRow
            For Each DRow In SelectDT.Rows
                IDS &= V & DRow.Item(ID_Field_Name)
                V = ","
            Next
            My_FP.DOFILTER_ReturnedParams.FilterWHERE = ""
            Form_Sub_WHERE = Form_Sub_WHERE.Replace("#IDS#", IDS)
            Form_Sub_WHERE = Form_Sub_WHERE.Replace("#ID_FIELD_NAME#", ID_Field_Name)
            My_FP.DATA_RS_WHERE("")
            My_FP.FORM_RECORDS_LOAD(Form_Sub_WHERE, False, False, True)
            My_FP.DATA_GOTO_RECORD_BY_ID(First_ID)
        Else
            Dim Out_P As P_Search_Fields
            Dim MyCancel As Boolean
            With Out_P
                .Search_Field0 = FPC_Search_Field0
                .Search_Field1 = FPC_Search_Field1
                .Search_Field2 = FPC_Search_Field2
                .Search_Field3 = FPC_Search_Field3
                .Search_Field4 = FPC_Search_Field4
            End With
            RaiseEvent No_Records_Found(Out_P, MyCancel)
        End If

    End Sub
    Private Function GET_FILTER_REPLACE_QUESTIONMARK(MyText As String, Q_TEXT As String, Q_ID As String, Q As String) As String
        'Ez a fuggveny azert jott letre, mert nem jo vegyiteni a ?(TEXT) es a ? parametereket.
        'Miert? Mert ha a szoveg: "???", akkor eloszor kicserelodik a ?(TEXT) "???"-re, majd ez kicserelodik haromszor "???"-ra.
        '       Tehat kizarolagosan vagy az egyik helyettesites jon letre, vagy a masik.
        Dim OUT As String = MyText

        If InStr(OUT, "?(TEXT)") > 0 Then
            OUT = Replace(OUT, "?(TEXT)", nz(Q_TEXT, ""))
        ElseIf InStr(OUT, "?(ID)") > 0 Then
            OUT = Replace(OUT, "?(ID)", Q_ID)
        Else
            OUT = Replace(OUT, "?", nz(Q, ""))
        End If

        Return OUT
    End Function

    Private Function GET_FILTER_REPLACE_QUESTIONMARK(MyText As String, Q_TEXT As String, Q_ID As Long, Q As String) As String
        'Ez a fuggveny azert jott letre, mert nem jo vegyiteni a ?(TEXT) es a ? parametereket.
        'Miert? Mert ha a szoveg: "???", akkor eloszor kicserelodik a ?(TEXT) "???"-re, majd ez kicserelodik haromszor "???"-ra.
        '       Tehat kizarolagosan vagy az egyik helyettesites jon letre, vagy a masik.
        Dim OUT As String = MyText

        If InStr(OUT, "?(TEXT)") > 0 Then
            OUT = Replace(OUT, "?(TEXT)", nz(Q_TEXT, ""))
        ElseIf InStr(OUT, "?(ID)") > 0 Then
            OUT = Replace(OUT, "?(ID)", Q_ID)
        Else
            OUT = Replace(OUT, "?", nz(Q, ""))
        End If

        Return OUT
    End Function

    Private Sub FPC_Search_Field0_Field_BeforeUpdate(sender_FPc As FP_Control, ByRef Cancel As Integer) Handles FPC_Search_Field0.Field_BeforeUpdate,
                                                                                                                FPC_Search_Field1.Field_BeforeUpdate,
                                                                                                                FPC_Search_Field2.Field_BeforeUpdate,
                                                                                                                FPC_Search_Field3.Field_BeforeUpdate,
                                                                                                                FPC_Search_Field4.Field_BeforeUpdate
        ReLoad_Form()
    End Sub

    Private Sub Clear_Search_Fields()

        If Not FPC_Search_Field0 Is Nothing Then FPC_Search_Field0.P_VALUE = ""
        If Not FPC_Search_Field1 Is Nothing Then FPC_Search_Field1.P_VALUE = ""
        If Not FPC_Search_Field2 Is Nothing Then FPC_Search_Field2.P_VALUE = ""
        If Not FPC_Search_Field3 Is Nothing Then FPC_Search_Field3.P_VALUE = ""
        If Not FPC_Search_Field4 Is Nothing Then FPC_Search_Field4.P_VALUE = ""

        ReLoad_Form()
    End Sub

    Private Sub FPP_BTN_Clear_Select_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPP_BTN_Clear_Select.CLICK
        Clear_Search_Fields()
    End Sub
End Class
