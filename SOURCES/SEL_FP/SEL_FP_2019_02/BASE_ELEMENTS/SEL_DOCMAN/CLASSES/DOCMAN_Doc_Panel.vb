Imports System.Collections.Specialized
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Public Class DOCMAN_Doc_Panel
    Public Structure Struct_DOCMAN_Doc_Panel_Scanned_Doc_Params
        Dim bitmap As FP_L_Bitmap
        Dim FileName As String
        Dim EstSize As Double
        Dim FileExt As ENUM_Scan_Doc_Ext
    End Structure

    Public Event DOC_SCANNED(ByVal sender As DOCMAN_Doc_Panel, ByRef Params As Struct_DOCMAN_Doc_Panel_Scanned_Doc_Params, ByRef Handled As Boolean)

#Region "DECLARE"

#Region "ENUM"

    Private Enum Enum_Attachement_Status As Integer
        Already_Saved = 0
        Save_It = 1
        Delete_It = 2
    End Enum

    Public Enum ENUM_DocTypes As Integer
        NOT_DEFINED = 0
        INTERNAL_DOCS = -11 'pl. telepito file-ok
        RATESERVER_EXCEL_RATETABLE = -101 'Tarifaszerver excel tablai
        INVOICE = -1001
        CMR = -1002
        PHOTO = -1003
        PROF_OF_RESIDENCE = -1004 'Tartozkodasi igazolas
        INV_Authentic_Copy = -1005
        ORDER = -1006 'Fuvarozasi megbizas
        CONFIRMATION = -1007
        CONTRACT = -1008
        OFFER = -1009
        BL = -1010
        DELIVERY_NOTE = -1011
        CSHBK_Authentic_Copy = -1012
        EMAIL = -1013
        CMR_INSURANCE = -1017
        OTHER_DOC = -1999
    End Enum

    Public Enum ENUM_Security_Level As Integer
        Normal = 0
        Confidential = 1 'Bizalmas
        Top_Secret = 2 'Szigoruan bizalmas
    End Enum

    Public Enum ENUM_Scan_Doc_Ext As Integer
        UNKNOWN = 0
        JPG = 1
        PNG = 2
        TIFF = 3
        GIFF = 4
        BMP = 5
        PDF = 6
    End Enum

#End Region

#Region "STRUCTURE"
    Public Structure Struct_DOCMAN_Docs_Panel_CONTROL_COLLECTION
        Dim FP_Parent As FP
        Dim DOCMAN_Panel As Panel
        Dim Fieldprefix As String
        Dim Subprefix As String
        Dim Parent_TableName As String
    End Structure

    Private Structure Struct_DOCMAN_Attached_Files
        Dim Pages_ID As Long
        Dim FileName As String
        Dim Status As Enum_Attachement_Status
        Dim Bytes() As Byte
    End Structure

    Public Structure Struct_DOCMAN_Doc_Params
        Dim FileName_with_Path As String
        Dim DOCMAN_Doc_Types_ID As Long
        Dim DOCMAN_RefNum As String
        Dim DOCMAN_CUST_ID As Long
        Dim DOCMAN_CUST_Name1 As String
        Dim DOCMAN_Doc_Date As DateTime
        Dim DOCMAN_Scan_Date As DateTime
        Dim DOCMAN_Descr As String
        Dim DOCMAN_Doc_Status_ID As Long
        Dim DOCMAN_Doc_Security_Level As ENUM_Security_Level
        Dim DOCMAN_Parent_TableName As String
        Dim DOCMAN_Parent_Record_ID As Long
        Dim DOCMAN_Sent_date As DateTime
        Dim DOCMAN_Message As String
        Dim DOCMAN_Origin As String
    End Structure

#End Region

    Public DOCMAN_Panel As Panel
    Public FPf As FP_Form
    Public WithEvents FP_DOCMAN_Docs As FP

    Private Subprefix As String
    Public Fieldprefix As String
    Public WithEvents FP_Parent As FP
    Public WithEvents FPp_DOCMAN_ButtonRefresh As FP_PictureBox = Nothing
    Public WithEvents FPp_DOCMAN_ButtonAdd As FP_PictureBox = Nothing
    Public WithEvents FPp_DOCMAN_ButtonSave As FP_PictureBox = Nothing
    Public WithEvents FPp_DOCMAN_ButtonScan As FP_PictureBox = Nothing
    Private Parent_TableName As String

    Private GRID_Label As Label
    Private WithEvents GRID As DataGridView
    Private DOCMAN_GRID_SavePoint As TextBox
    Private Picturebox_ButtonRefresh As PictureBox
    Private Picturebox_ButtonAdd As PictureBox
    Private Picturebox_ButtonSave As PictureBox
    Private Picturebox_ButtonScan As PictureBox
    Private Picturebox_Attachements_SHOW As PictureBox
    Private Attachements_Panel As Panel
    Private Attachements_Label As Label
    Private WithEvents Attachements As ListView
    Private Attachements_IM As ImageList
    Private Attachements_BG_COLOR As Color = COLORS_FIELD_NORMAL_BG

    Private WithEvents FPc_DOCMAN_Doc_Types_ID As FP_Control
    Private FPc_DOCMAN_RefNum As FP_Control
    Private FPc_DOCMAN_Descr As FP_Control
    Private FPc_DOCMAN_Doc_Date As FP_Control
    Private FPc_DOCMAN_Scan_Date As FP_Control
    Private FPc_DOCMAN_Doc_Attached As FP_Control
    Private FPc_DOCMAN_Doc_Attached_PictureCell As FP_L_PictureTextbox
    Private FPc_DOCMAN_Doc_Security_Level As FP_Control
    Private FPc_DOCMAN_Valid_Date As FP_Control

    Private Attached_Files(0) As Struct_DOCMAN_Attached_Files
    Private Attached_Files_BeforeSave() As Struct_DOCMAN_Attached_Files
    Private Attachement_DragData As DataObject

    Private Disposed As Boolean = False

    Private Control_Prefix = "DOCMAN_"

    Private Attached_FilesWithPath() As String
#End Region

#Region "CLASS CONSTRUCTOR"

    Sub New(ByVal CONTROL_COLLECTION As Struct_DOCMAN_Docs_Panel_CONTROL_COLLECTION)
        With CONTROL_COLLECTION
            DOCMAN_Panel = .DOCMAN_Panel
            Subprefix = nz(.Subprefix, "")
            Fieldprefix = nz(.Fieldprefix, "")
            FP_Parent = .FP_Parent
            Parent_TableName = .Parent_TableName
        End With

        FPf = FP_Parent.FPf

        GRID_Label = New Label
        With GRID_Label
            .Name = Control_FieldPrefix_And_Prefix() + "GRID_Label"
            .Parent = DOCMAN_Panel
            .Visible = True
            .BackColor = Color.FromArgb(25, 25, 111)
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        End With
        FPf.CONTROLS_ADD(GRID_Label)

        GRID = New DataGridView
        With GRID
            .Name = Control_FieldPrefix_And_Prefix() + "GRID"
            .Parent = DOCMAN_Panel
            .Visible = True
            .BackgroundColor = Color.DarkGray
            .AllowDrop = True
        End With
        FPf.CONTROLS_ADD(GRID)

        DOCMAN_GRID_SavePoint = New TextBox
        With DOCMAN_GRID_SavePoint
            .Name = Control_FieldPrefix_And_Prefix() + "SavePoint"
            .Parent = DOCMAN_Panel
            .BackColor = Color.DarkGray
            .Width = 20
            .SendToBack()
            .Visible = True
        End With
        FPf.CONTROLS_ADD(DOCMAN_GRID_SavePoint)

        Picturebox_ButtonRefresh = New PictureBox

        With Picturebox_ButtonRefresh
            .Parent = DOCMAN_Panel
            .Name = Control_FieldPrefix_And_Prefix() + "ButtonRefresh"
            .Left = 0
            .Top = 0
            .Width = 44
            .Height = 44
            .Visible = True
            .BringToFront()
        End With
        DOCMAN_Panel.Controls.Add(Picturebox_ButtonRefresh)
        FPf.CONTROLS_ADD(Picturebox_ButtonRefresh)

        Picturebox_ButtonAdd = New PictureBox

        With Picturebox_ButtonAdd
            .Parent = DOCMAN_Panel
            .Name = Control_FieldPrefix_And_Prefix() + "ButtonAdd"
            .Left = 0
            .Top = 44
            .Width = 44
            .Height = 44
            .Visible = True
            .BringToFront()
        End With
        DOCMAN_Panel.Controls.Add(Picturebox_ButtonAdd)
        FPf.CONTROLS_ADD(Picturebox_ButtonAdd)

        Picturebox_ButtonSave = New PictureBox

        With Picturebox_ButtonSave
            .Parent = DOCMAN_Panel
            .Name = Control_FieldPrefix_And_Prefix() + "ButtonSave"
            .Left = 0
            .Top = 88
            .Width = 44
            .Height = 44
            .Visible = True
            .BringToFront()
        End With
        DOCMAN_Panel.Controls.Add(Picturebox_ButtonSave)
        FPf.CONTROLS_ADD(Picturebox_ButtonSave)

        Picturebox_ButtonScan = New PictureBox

        With Picturebox_ButtonScan
            .Parent = DOCMAN_Panel
            .Name = Control_FieldPrefix_And_Prefix() + "ButtonScan"
            .Left = 0
            .Top = 88
            .Width = 44
            .Height = 44
            .Visible = True
            .BringToFront()
        End With
        DOCMAN_Panel.Controls.Add(Picturebox_ButtonScan)
        FPf.CONTROLS_ADD(Picturebox_ButtonScan)

        Attachements_Panel = New Panel
        With Attachements_Panel
            .Parent = DOCMAN_Panel
            .Name = Control_FieldPrefix_And_Prefix() + "Attachements_Panel"
            .Visible = True
            .BringToFront()
        End With
        DOCMAN_Panel.Controls.Add(Attachements_Panel)
        FPf.CONTROLS_ADD(Attachements_Panel)

        Attachements_Label = New Label
        With Attachements_Label
            .Parent = Attachements_Panel
            .Name = Control_FieldPrefix_And_Prefix() + "Attachements_Label"
            .Visible = True
            .BackColor = Color.FromArgb(25, 25, 111)
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
            .BringToFront()
        End With
        Attachements_Panel.Controls.Add(Attachements_Label)
        FPf.CONTROLS_ADD(Attachements_Label)

        Picturebox_Attachements_SHOW = New PictureBox
        With Picturebox_Attachements_SHOW
            .Parent = DOCMAN_Panel
            .Name = Control_FieldPrefix_And_Prefix() + "Attachements_SHOW"
            .Left = 0
            .Top = 0
            .Width = 22
            .Height = 22
            .Visible = True
            .BringToFront()
        End With
        DOCMAN_Panel.Controls.Add(Picturebox_Attachements_SHOW)
        FPf.CONTROLS_ADD(Picturebox_Attachements_SHOW)

        Attachements_IM = New ImageList

        Dim Image_CLIP As Bitmap = gl_FPApp.SKIN_GET_IMAGE("Clip.png")

        With Attachements_IM
            .Images.Add("CLIP", Image_CLIP)
        End With

        Attachements = New ListView
        With Attachements
            .Parent = Attachements_Panel
            .Name = Control_FieldPrefix_And_Prefix() + "Attachements"
            .View = View.SmallIcon
            .SmallImageList = Attachements_IM
            .AllowDrop = True
        End With
        Attachements_Panel.Controls.Add(Attachements)
        FPf.CONTROLS_ADD(Attachements)

        FP_DOCMAN_Docs = New FP(Fieldprefix, FPf, "FP_DOCMAN_Docs_Panel", Subprefix, FP_Parent, "DOCMAN_Parent_Record_ID")

        If Parent_TableName = "" Then
            gl_FPApp.DoErrorMsgBox("DOCMAN_Doc_Panel.New", 0, "A Parent_TableName parameter nincs megadva!")
        Else
            FP_DOCMAN_Docs.FORM_SubWHERE_FIX = String.Format("DOCMAN_Parent_TableName = '{0}'", CONTROL_COLLECTION.Parent_TableName)
        End If

        Dim FP_CONTROLS_COLLECTION As New Struct_FP_CONTROLS_COLLECTION
        With FP_CONTROLS_COLLECTION
            .FieldPrefix = Fieldprefix
            With .GRID
                .Label = GRID_Label
                .GRID = GRID
                .Footer_Panel = Attachements_Panel
                .Btn_FooterVisible = Picturebox_Attachements_SHOW
            End With
        End With

        FP_DOCMAN_Docs.INIT_CONTROLS(FP_CONTROLS_COLLECTION)
        FP_DOCMAN_Docs.GRID.FOOTER_SHOW()

        FPc_DOCMAN_Doc_Attached_PictureCell = New FP_L_PictureTextbox(FPc_DOCMAN_Doc_Attached)
        With FPc_DOCMAN_Doc_Attached_PictureCell
            .BITMAPS_ADD("0", "")
            .BITMAPS_ADD("1", "Clip.png")
        End With

        '-----------------------------------------------------------------------------------------------------------------------
        '-- Register this DOCMAN in FP_Parent.DOCMAN
        '-----------------------------------------------------------------------------------------------------------------------

        If (FP_Parent Is Nothing) Then
            FPf.DoErrorMsgBox("DOCMAN_Doc_Panel.New", 0, "FP_Parent is nothing!!!")
        End If
    End Sub

#End Region

#Region "PROPERTIES"

    Public Property P_Parent_TableName As String
        Get
            Return Parent_TableName
        End Get
        Set(value As String)
            Parent_TableName = value
            FP_DOCMAN_Docs.FORM_SubWHERE_FIX = String.Format("DOCMAN_Parent_TableName = '{0}'", Parent_TableName)
        End Set
    End Property

    Public ReadOnly Property P_Subprefix As String
        Get
            Return Subprefix
        End Get
    End Property

    Public ReadOnly Property P_TEMP_FolderName As String
        Get
            Return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\"
        End Get
    End Property

#End Region

#Region "FP_DOCMAN_DOCS EVENTS"

    Private Sub FP_DOCMAN_Docs_CONTROLS_INITIALIZED(ByVal sender_FP As FP) Handles FP_DOCMAN_Docs.CONTROLS_INITIALIZED
        If Not Disposed Then
            With FP_DOCMAN_Docs
                FPp_DOCMAN_ButtonRefresh = .PICTUREBOXES_GET(Control_FieldPrefix_And_Prefix() + "ButtonRefresh")
                FPp_DOCMAN_ButtonAdd = .PICTUREBOXES_GET(Control_FieldPrefix_And_Prefix() + "ButtonAdd")
                FPp_DOCMAN_ButtonSave = .PICTUREBOXES_GET(Control_FieldPrefix_And_Prefix() + "ButtonSave")
                FPp_DOCMAN_ButtonScan = .PICTUREBOXES_GET(Control_FieldPrefix_And_Prefix() + "ButtonScan")
                FPc_DOCMAN_Doc_Types_ID = .CONTROLS_GET_FPc(Control_FieldPrefix_And_Prefix() + "Doc_Types_ID")
                FPc_DOCMAN_RefNum = .CONTROLS_GET_FPc(Control_FieldPrefix_And_Prefix() + "RefNum")
                FPc_DOCMAN_Descr = .CONTROLS_GET_FPc(Control_FieldPrefix_And_Prefix() + "Descr")
                FPc_DOCMAN_Doc_Date = .CONTROLS_GET_FPc(Control_FieldPrefix_And_Prefix() + "Doc_Date")
                FPc_DOCMAN_Scan_Date = .CONTROLS_GET_FPc(Control_FieldPrefix_And_Prefix() + "Scan_Date")
                FPc_DOCMAN_Doc_Attached = .CONTROLS_GET_FPc(Control_FieldPrefix_And_Prefix() + "Doc_Attached")
                FPc_DOCMAN_Doc_Security_Level = .CONTROLS_GET_FPc(Control_FieldPrefix_And_Prefix() + "Doc_Security_Level")
                If gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_IsInstalled("DOCMAN_DEV_2021_03") Then
                    FPc_DOCMAN_Valid_Date = .CONTROLS_GET_FPc(Control_FieldPrefix_And_Prefix() + "Valid_Date")
                End If
            End With

            FPc_DOCMAN_Doc_Types_ID.P_Marker = FP_Control.ENUM_Markertypes.Right_Arrow
        End If
    End Sub

    Private Sub FP_DOCMAN_Docs_Form_AfterUpdate(sender_FP As FP) Handles FP_DOCMAN_Docs.Form_AfterUpdate
        If Not Disposed Then
            Attachements_Manage()
            Attachements_DeleteOriginFiles()
        End If
    End Sub

    Private Sub FP_DOCMAN_Docs_Form_BeforeUpdate(ByVal sender_FP As FP, ByRef Cancel As Integer) Handles FP_DOCMAN_Docs.Form_BeforeUpdate
        If Not Disposed Then
            Fill_MustFields()
            FP_DOCMAN_Docs.DATA_Field_setValue("DOCMAN_Parent_TableName", Parent_TableName)
            Attached_Files_BeforeSave = Attached_Files
        End If
        If gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_IsInstalled("DOCMAN_DEV_2021_03") Then
            FPc_DOCMAN_Valid_Date.P.Mandatory = Valid_Date_Required()
        End If
    End Sub

    Private Sub FP_DOCMAN_Docs_Form_BeginEdit(sender_FP As FP) Handles FP_DOCMAN_Docs.Form_BeginEdit
        If Not Disposed Then
            If FP_DOCMAN_Docs.P_DATA_NewRecord Then
                If Not FPc_DOCMAN_Scan_Date.c.Focused Then
                    FPc_DOCMAN_Scan_Date.P_VALUE = gl_FPApp.GET_SERVER_CURRENT_DATE(True)

                    FP_DOCMAN_Docs.COLORING_ALL()
                End If
            End If
        End If
    End Sub

    Private Sub FP_DOCMAN_Docs_Form_Current(sender_FP As FP) Handles FP_DOCMAN_Docs.Form_Current
        If Not Disposed Then
            Attachements_ListBox_SET(FP_DOCMAN_Docs.DATA_Field_getSavedValue("DOCMAN_Attached_Docs_STR"))
            FP_DOCMAN_Docs_SET_LAYOUT()
        End If
    End Sub

    Private Sub FP_DOCMAN_Docs_Form_NoRecord(sender_FP As FP) Handles FP_DOCMAN_Docs.Form_NoRecord
        If Not Disposed Then
            Attachements_ListBox_SET("")
        End If
    End Sub
#End Region

#Region "FPC EVENTS"

    Private Sub FPc_DOCMAN_Doc_Types_ID_Field_BeforeUpdate(sender_FPc As FP_Control, ByRef Cancel As Integer) Handles FPc_DOCMAN_Doc_Types_ID.Field_BeforeUpdate
        If FPc_DOCMAN_Doc_Types_ID.P_VALUE = ENUM_DocTypes.INV_Authentic_Copy Or FPc_DOCMAN_Doc_Types_ID.P_VALUE = ENUM_DocTypes.CSHBK_Authentic_Copy Then
            Cancel = True
            gl_FPApp.DoMyMsgBox(83007) 'Ezt a dokumentum tipust nem lehet kivalasztani.
        End If
    End Sub

    Private Sub FPc_DOCMAN_Doc_Types_ID_Field_Marker_Click(sender_FPc As FP_Control, sender As Object, e As EventArgs, ByRef Handled As Boolean) Handles FPc_DOCMAN_Doc_Types_ID.Field_Marker_Click, FPc_DOCMAN_Doc_Types_ID.Field_Doubleclick
        gl_FPApp.RAISEEVENT_Marker_Clicked(sender_FPc, "SEL_DOCMAN_DOCTYPES_DIALOG", Nothing, Handled)
    End Sub

#End Region

#Region "FPP EVENTS"

    Private Sub FPp_DOCMAN_ButtonAdd_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPp_DOCMAN_ButtonAdd.CLICK
        If FP_Parent.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
            If FP_DOCMAN_Docs.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
                Dim OpenFileDialog As New OpenFileDialog
                Dim OpenFile As String = ""

                gl_FPApp.PFDlesen("FP_DOCMAN_OPENPATH", OpenFile)

                OpenFileDialog.InitialDirectory = OpenFile
                OpenFileDialog.Filter = "All files (*.*)|*.*"
                OpenFileDialog.Multiselect = True

                If OpenFileDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then
                    Exit Sub
                End If
                gl_FPApp.PFDinsertOrUpdate("FP_DOCMAN_OPENPATH", OpenFileDialog.FileName.Substring(0, OpenFileDialog.FileName.LastIndexOf("\")))

                Dim Separate_Attachements As Boolean = False

                If OpenFileDialog.FileNames.Length > 1 Then
                    Separate_Attachements = (gl_FPApp.DoMyMsgBox(83010, , "SEQ,JA", "SEQ,NEIN") = 2)
                End If

                Select Case Separate_Attachements
                    Case False
                        'Do not separate attachements
                        If FP_DOCMAN_Docs.FORM_DIRTY_SET Then
                            FPc_DOCMAN_Doc_Types_ID.c.Focus()

                            If FP_DOCMAN_Docs.P_DATA_RecordStatus = ENUM_RecordStatus.NEWRECORD Then
                                FPc_DOCMAN_Descr.P_VALUE = Get_Descr_from_FileName(OpenFileDialog.FileName, False)
                                FPc_DOCMAN_Doc_Date.P_VALUE = gl_FPApp.GET_SERVER_CURRENT_DATE()
                            End If

                            FP_DOCMAN_Docs.COLORING_ALL()

                            Dim ArrSize As Integer = OpenFileDialog.FileNames.Count
                            ReDim Preserve Attached_FilesWithPath(ArrSize)

                            Attached_FilesWithPath(0) = Nothing

                            OpenFileDialog.FileNames.CopyTo(Attached_FilesWithPath, 1)
                            Attachements_ListBox_ADD(Attached_FilesWithPath)
                        End If

                    Case True
                        'Separate attachements
                        For i As Integer = 0 To OpenFileDialog.FileNames.Count - 1
                            If FP_DOCMAN_Docs.FORM_GOTO_NEWRECORD Then
                                If FP_DOCMAN_Docs.FORM_DIRTY_SET Then
                                    FPc_DOCMAN_Descr.P_VALUE = Get_Descr_from_FileName(OpenFileDialog.FileNames(i))
                                    Fill_MustFields()
                                    ReDim Attached_FilesWithPath(1)
                                    Attached_FilesWithPath(0) = Nothing
                                    Attached_FilesWithPath(1) = OpenFileDialog.FileNames(i)
                                    Attachements_ListBox_ADD(Attached_FilesWithPath)
                                    FP_DOCMAN_Docs.FORM_RECORDS_SAVE_CURRENT()
                                End If
                            End If
                        Next
                End Select
            End If
        End If
    End Sub

    Private Sub FPp_DOCMAN_ButtonRefresh_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPp_DOCMAN_ButtonRefresh.CLICK
        DORESYNC()
    End Sub

    Private Sub FPp_DOCMAN_ButtonSave_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPp_DOCMAN_ButtonSave.CLICK
        'If FP_DOCMAN_Docs.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
        '    Dim FolderBrowserDialog As New FolderBrowserDialog
        '    Dim Folder As String = ""

        '    If Attachements.SelectedItems.Count = 0 Then
        '        Attachements.BeginUpdate()
        '        For i = 0 To Attachements.Items.Count - 1
        '            Attachements.Items(i).Selected = True
        '        Next
        '        Attachements.EndUpdate()
        '    End If

        '    gl_FPApp.PFDlesen("FP_DOCMAN_SAVEPATH", Folder)

        '    FolderBrowserDialog.SelectedPath = Folder

        '    If FolderBrowserDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then
        '        Exit Sub
        '    End If
        '    Folder = FolderBrowserDialog.SelectedPath
        '    gl_FPApp.PFDinsertOrUpdate("FP_DOCMAN_SAVEPATH", Folder)

        '    Attachements_COPY_SELECTED(Folder, False, True)
        '    Process.Start("explorer.exe", Folder)
        'End If
        If FP_DOCMAN_Docs.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
            Dim FolderBrowserDialog As New FolderBrowserDialog
            Dim Folder As String = ""
            Dim P As New Struct_Simple_SELECT_Params
            Dim P_OUT As New Struct_Simple_SELECT_OutputParams
            Dim da As SqlDataAdapter
            Dim dt As New DataTable
            Dim FileName_With_Path As String

            gl_FPApp.PFDlesen("FP_DOCMAN_SAVEPATH", Folder)

            FolderBrowserDialog.SelectedPath = Folder

            If FolderBrowserDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
            Folder = FolderBrowserDialog.SelectedPath
            gl_FPApp.PFDinsertOrUpdate("FP_DOCMAN_SAVEPATH", Folder)

            With P
                .FixText_Key = "DOCMAN_FileSave"
                .SQL_WHERE = String.Format("ParentTableName='{0}' AND ParentRecord_ID={1}", P_Parent_TableName, FP_Parent.P_DATA_Current_ID)
            End With
            If Not gl_FPApp.SIMPLE_SELECT(P, P_OUT) Then
                Exit Sub
            End If

            Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand

            With gl_FPApp.DC
                .Qdf_set_SP(sqlComm, "DOCMAN_FileSave_getIDs")
                .Qdf_AddParameter(sqlComm, "@RS_ID", SqlDbType.Int, , , , , P_OUT.RS_ID)

                da = New SqlDataAdapter(sqlComm)
                da.Fill(dt)
            End With

            For Each row As DataRow In dt.Rows
                FileName_With_Path = String.Format("{0}\{1}", Folder, row("FileName"))

                gl_FPApp.ByteArray_SaveFile(FileName_With_Path, row("ImageData"))
            Next

            Process.Start("explorer.exe", Folder)
        End If
    End Sub

    Private Sub FPp_DOCMAN_ButtonScan_CLICK(sender_FPc As FP_PictureBox, e As MouseEventArgs) Handles FPp_DOCMAN_ButtonScan.CLICK
        If FP_DOCMAN_Docs.FORM_DIRTY_SET Then
            Dim Scan As New FP_Scan(FPf)
            Dim FPF_P_ENABLED_OLD As Boolean = FPf.P_ENABLED
            Dim TryCount As Integer = 0
            Dim Handled As Boolean = False
            Dim DoIt As Boolean = True

            FPf.P_ENABLED = False

            Dim SE As New FP_Simple_Edit(gl_FPApp, "DOCMAN_SAVE_DOC", "DOCMAN_SAVE_DOC")

            SE.DATAFIELD_ADD("FileExt")
            SE.DATAFIELD_ADD("FileName")

            DoIt = (gl_FPApp.ShowDialogForm(SE, FPf) = Windows.Forms.DialogResult.OK)
            If DoIt Then
                If Scan.ScanDocument("DOCMAN_SCAN") Then
                    Attached_FilesWithPath = Nothing
                    ReDim Preserve Attached_FilesWithPath(Scan.P_BitmapList.Count)
                    Attached_FilesWithPath(0) = Nothing

                    For i As Integer = 0 To Scan.P_BitmapList.Count - 1
                        Dim Scanned_Image As New Struct_DOCMAN_Doc_Panel_Scanned_Doc_Params
                        Dim bitmap As New FP_L_Bitmap(Scan.P_BitmapList(i))

                        With Scanned_Image
                            .FileName = ""
                            .FileExt = SE.FP_SIMPLEEDIT.CONTROLS("FileExt").P_VALUE
                            .bitmap = bitmap
                            DoIt = (.FileExt <> ENUM_Scan_Doc_Ext.UNKNOWN)
                        End With

                        Dim OrigFileName As String = gl_FPApp.Text_Remove_IllegalCharacters_From_FileName(String.Format("{0}.{1}", SE.FP_SIMPLEEDIT.CONTROLS("FileName").P_VALUE, SE.FP_SIMPLEEDIT.CONTROLS("FileExt").c.Text))
                        Dim FileName As String = OrigFileName

                        If (Scanned_Image.FileExt = ENUM_Scan_Doc_Ext.PDF And i = Scan.P_BitmapList.Count - 1) Or Scanned_Image.FileExt <> ENUM_Scan_Doc_Ext.PDF Then
                            Do While Attachements.FindItemWithText(FileName) IsNot Nothing
                                TryCount += 1

                                FileName = String.Format("{0}({1}){2}", Path.GetFileNameWithoutExtension(OrigFileName), TryCount, Path.GetExtension(OrigFileName))
                            Loop
                            Scanned_Image.FileName = FileName

                            RaiseEvent DOC_SCANNED(Me, Scanned_Image, Handled)
                        End If

                        If DoIt Then
                            Dim FilePath As String = String.Format("{0}{1}", gl_FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP), Scanned_Image.FileName)

                            ReDim Preserve Attached_FilesWithPath(1)

                            Attached_FilesWithPath(0) = Nothing
                            Attached_FilesWithPath(1) = FilePath

                            Select Case Scanned_Image.FileExt
                                Case ENUM_Scan_Doc_Ext.JPG
                                    Scanned_Image.bitmap.SaveToJPEG(FilePath)
                                    Attachements_ListBox_ADD(Attached_FilesWithPath)

                                Case ENUM_Scan_Doc_Ext.PNG
                                    Scanned_Image.bitmap.SaveToPNG(FilePath)
                                    Attachements_ListBox_ADD(Attached_FilesWithPath)

                                Case ENUM_Scan_Doc_Ext.TIFF
                                    Scanned_Image.bitmap.SaveToTIFF(FilePath)
                                    Attachements_ListBox_ADD(Attached_FilesWithPath)

                                Case ENUM_Scan_Doc_Ext.GIFF
                                    Scanned_Image.bitmap.SaveToGIF(FilePath)
                                    Attachements_ListBox_ADD(Attached_FilesWithPath)

                                Case ENUM_Scan_Doc_Ext.BMP
                                    Scanned_Image.bitmap.SaveToBMP(FilePath)
                                    Attachements_ListBox_ADD(Attached_FilesWithPath)

                                Case ENUM_Scan_Doc_Ext.PDF
                                    If i = Scan.P_BitmapList.Count - 1 Then
                                        Dim PDF As New FP_PDF

                                        PDF.SaveToSinglePDF(Scan.P_BitmapList, FilePath)
                                        Attachements_ListBox_ADD(Attached_FilesWithPath)
                                    End If

                                Case Else
                                    gl_FPApp.DoErrorMsgBox(FPf, "FPp_DOCMAN_ButtonScan_CLICK", -1, "Ismeretlen fajl kiterjesztes!")
                            End Select
                        End If
                    Next

                End If
            End If
            Scan.Dispose()

            FPf.P_ENABLED = FPF_P_ENABLED_OLD
        End If
    End Sub

#End Region

#Region "CONTROL EVENTS"

    Private Sub Attachements_DoubleClick(sender As Object, e As EventArgs) Handles Attachements.DoubleClick
        Attachements_COPY_SELECTED_TO_Users_DOCMAN_Folder(True)
    End Sub

    Private Sub Attachements_DragDrop(sender As Object, e As DragEventArgs) Handles Attachements.DragDrop
        DragDrop_DoIt(e)
    End Sub

    Private Sub Attachements_GotFocus(sender As Object, e As EventArgs) Handles Attachements.GotFocus
        Attachements_BG_COLOR = Attachements.BackColor
        Attachements.BackColor = COLORS_FIELD_CURRENT_BG
    End Sub

    Private Sub Attachements_ItemDrag(sender As Object, e As ItemDragEventArgs) Handles Attachements.ItemDrag
        Dim ToFolder As String = ""

        If Attachements_COPY_SELECTED_TO_Users_DOCMAN_Folder(False, ToFolder) Then
            Dim DropList As New StringCollection

            For Each It As ListViewItem In Attachements.Items
                If It.Selected Then
                    DropList.Add(ToFolder + "\" + It.Text)
                End If
            Next

            Attachement_DragData = New DataObject
            Attachement_DragData.SetFileDropList(DropList)

            Attachements.DoDragDrop(Attachement_DragData, DragDropEffects.Move)
        End If
    End Sub

    Private Sub Attachements_KeyDown(sender As Object, e As KeyEventArgs) Handles Attachements.KeyDown
        Select Case e.KeyData
            Case Keys.Delete
                If FP_DOCMAN_Docs.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
                    e.Handled = True
                    gl_FPApp.DoMyMsgBox(83003) 'A csatolt dokumentumok torlese Ctrl-D-vel lehetseges.
                End If

            Case Keys.Enter
                Attachements_COPY_SELECTED_TO_Users_DOCMAN_Folder(True)
        End Select
    End Sub

    Private Sub Attachements_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Attachements.KeyPress
        Dim CtrlPressed = My.Computer.Keyboard.CtrlKeyDown

        If CtrlPressed Then
            Select Case Asc(e.KeyChar)
                Case 1 'Ctrl-a, Ctrl-A
                    Attachements_SELECT_ALL()

                Case 4      'Ctrl-d, Ctrl-D
                    e.Handled = True
                    If FP_DOCMAN_Docs.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                        If Attachements_Right_for_Manage() Then
                            Attachements_DEL_SELECTED()
                        End If
                    End If
            End Select

        Else
            Select Case Asc(e.KeyChar)
                Case 27 'Esc
                    FP_DOCMAN_Docs.UNDO()
            End Select
        End If
    End Sub

    Private Sub Attachements_LostFocus(sender As Object, e As EventArgs) Handles Attachements.LostFocus
        Attachements.BackColor = Attachements_BG_COLOR
    End Sub

    Private Sub GRID_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles GRID.DragOver, Attachements.DragOver
        Dim eData As New System.Windows.Forms.DataObject(e.Data)

        If Not eData.Equals(Attachement_DragData) Then
            If eData.ContainsFileDropList Then
                If eData.GetFileDropList.Count > 0 Then
                    e.Effect = DragDropEffects.Copy
                End If
            ElseIf e.Data.GetDataPresent("FileGroupDescriptor") Then
                'handle a message dragged from Outlook
                e.Effect = DragDropEffects.Copy
            End If
        End If
    End Sub

    Private Sub GRID_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles GRID.DragDrop
        If FP_DOCMAN_Docs.FORM_RECORDS_SAVE_CURRENT Then
            If FP_DOCMAN_Docs.P_DATA_RecordStatus <> ENUM_RecordStatus.NEWRECORD Then
                FP_DOCMAN_Docs.FORM_GOTO_NEWRECORD()
            End If

            DragDrop_DoIt(e, True)
        End If
    End Sub

#End Region

#Region "SUBS"
    Private Sub Attachements_ListBox_ADD(DragDrop_FileNames() As String)
        Attached_FilesWithPath = Nothing

        For i As Integer = 1 To UBound(DragDrop_FileNames)
            Dim FileBytes(0) As Byte

            If Trim(FPc_DOCMAN_RefNum.P_VALUE) = "" Then
                FPc_DOCMAN_RefNum.P_VALUE = "-"
            End If

            If Trim(FPc_DOCMAN_Descr.P_VALUE) = "" Then
                FPc_DOCMAN_Descr.P_VALUE = getFileName_without_Extension(Path.GetFileName(DragDrop_FileNames(1)))
            End If

            If FPc_DOCMAN_Doc_Date.P_VALUE = NULLDATE Then
                FPc_DOCMAN_Doc_Date.P_VALUE = gl_FPApp.GET_SERVER_CURRENT_DATE(False)
            End If

            If FPc_DOCMAN_Scan_Date.P_VALUE = NULLDATE Then
                FPc_DOCMAN_Scan_Date.P_VALUE = gl_FPApp.GET_SERVER_CURRENT_DATE(True)
            End If

            FP_DOCMAN_Docs.COLORING_ALL()

            If ATTACHEMENT_EXISTS(DragDrop_FileNames(i)) = False Then
                If gl_FPApp.ByteArray_getFile(DragDrop_FileNames(i), FileBytes) Then
                    ReDim Preserve Attached_Files(UBound(Attached_Files) + 1)
                    With Attached_Files(UBound(Attached_Files))
                        .FileName = Get_Descr_from_FileName(DragDrop_FileNames(i), True)
                        .Status = Enum_Attachement_Status.Save_It
                        .Bytes = FileBytes
                    End With
                End If
            End If

            If i = UBound(DragDrop_FileNames) Then
                ReDim Preserve Attached_FilesWithPath(UBound(DragDrop_FileNames))
                DragDrop_FileNames.CopyTo(Attached_FilesWithPath, 0)
            End If
        Next

        Attachements_ListBox_UPDATE()
        FPc_DOCMAN_Doc_Attached.P_VALUE = (UBound(Attached_Files) > 0)
    End Sub

    Private Sub Attachements_ListBox_CLEAR()
        Attachements.Clear()
    End Sub

    Private Sub Attachements_ListBox_UPDATE()
        Attachements_ListBox_CLEAR()

        For i As Integer = 1 To UBound(Attached_Files)
            If Attached_Files(i).Status <> Enum_Attachement_Status.Delete_It Then
                Dim CurrentItem As New ListViewItem

                With CurrentItem
                    .Name = i.ToString
                    .Text = Attached_Files(i).FileName
                    .ImageKey = "CLIP"
                End With

                Attachements.Items.Add(CurrentItem)
            End If
        Next
    End Sub

    Private Sub Attachements_ListBox_SET(FileNamesWithDelimiter As String)
        ReDim Attached_Files(0)

        If FileNamesWithDelimiter > "" Then
            Dim FileNames() As String = Split(FileNamesWithDelimiter, "|")

            For i As Integer = 0 To UBound(FileNames)
                ReDim Preserve Attached_Files(i + 1)

                Dim CurrFile_Data() As String = Split(FileNames(i), ":")    'kettospont nem lehet filenevben, ezert valasztottam ezt az elvalaszto jelet.

                With Attached_Files(i + 1)
                    .FileName = CurrFile_Data(0)
                    .Pages_ID = CurrFile_Data(1)
                    .Status = Enum_Attachement_Status.Already_Saved
                End With
            Next
        End If

        Attachements_ListBox_UPDATE()
    End Sub

    Private Sub DragDrop_DoIt(ByVal e As System.Windows.Forms.DragEventArgs, Optional Separate_Attachements As Boolean = False)
        If (FP_DOCMAN_Docs.P_DATA_RecordStatus = ENUM_RecordStatus.NORECORD) Then
            Exit Sub
        End If

        If FP_DOCMAN_Docs.P_FORM_Dirty Then
            Fill_MustFields()
            If Not FP_DOCMAN_Docs.FORM_RECORDS_SAVE_CURRENT() Then
                Exit Sub
            End If
        End If

        Dim eData As New System.Windows.Forms.DataObject(e.Data)

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            'If Separate_Attachements = False Then  - mert ha rahuzok egy dokumentumot egy sorra, akkor a sornak igenis dirty-be kell mennie
            If FP_DOCMAN_Docs.FORM_DIRTY_SET = False Then
                Exit Sub
            End If
            'End If

            Dim Dropped_Files(eData.GetFileDropList.Count) As String

            eData.GetFileDropList().CopyTo(Dropped_Files, 1)

            Attachements_ListBox_ADD(Dropped_Files)
        ElseIf e.Data.GetDataPresent("FileGroupDescriptor") Then
            'supports a drop of a Outlook message
            'End If

            Try
                Dim objOL As Microsoft.Office.Interop.Outlook.Application = GetObject(, "Outlook.Application")

                Dim TimeStamp As String = "_" + Format(Now, "yyMMdd_HHmmss") + "_"
                Dim TimeStamp_Num As Integer = 0
                Dim TempFolder As String = gl_FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP)

                For Each objMI As Microsoft.Office.Interop.Outlook.MailItem In objOL.ActiveExplorer.Selection()
                    Dim Subject_As_FileName As String = gl_FPApp.Text_Remove_IllegalCharacters_From_FileName(objMI.Subject)
                    Dim Msg_FileName As String = Subject_As_FileName + ".msg"

                    TimeStamp_Num += 1

                    Dim Temp_FileName As String = TempFolder + Subject_As_FileName + TimeStamp + TimeStamp_Num.ToString + ".msg"

                    Dim FileBytes(0) As Byte

                    If Separate_Attachements Then
                        FP_DOCMAN_Docs.FORM_GOTO_NEWRECORD()
                        If Not (FP_DOCMAN_Docs.P_DATA_RecordStatus = ENUM_RecordStatus.NEWRECORD) Then
                            Exit Sub
                        End If

                        'If Separate_Attachements = False Then  - mert ha rahuzok egy dokumentumot egy sorra, akkor a sornak igenis dirty-be kell mennie
                        If FP_DOCMAN_Docs.FORM_DIRTY_SET = False Then
                            Exit Sub
                        End If

                        FPc_DOCMAN_Descr.P_VALUE = objMI.Subject
                        FPc_DOCMAN_Doc_Date.P_VALUE = objMI.ReceivedTime
                        FPc_DOCMAN_Doc_Types_ID.P_VALUE = ENUM_DocTypes.EMAIL
                        FPc_DOCMAN_RefNum.P_VALUE = "-"

                        FP_DOCMAN_Docs.COLORING_ALL()
                    Else
                        If FP_DOCMAN_Docs.FORM_DIRTY_SET = False Then
                            Exit Sub
                        End If

                        If FPc_DOCMAN_Descr.P_VALUE = "" Then
                            FPc_DOCMAN_Descr.P_VALUE = objMI.Subject
                        End If

                        If FPc_DOCMAN_Doc_Date.P_VALUE = NULLDATE Then
                            FPc_DOCMAN_Doc_Date.P_VALUE = objMI.ReceivedTime
                        End If

                        If FPc_DOCMAN_Doc_Types_ID.P_VALUE = ENUM_DocTypes.NOT_DEFINED Then
                            FPc_DOCMAN_Doc_Types_ID.P_VALUE = ENUM_DocTypes.EMAIL
                        End If

                        If FPc_DOCMAN_RefNum.P_VALUE = "" Then
                            FPc_DOCMAN_RefNum.P_VALUE = "-"
                        End If
                    End If

                    If ATTACHEMENT_EXISTS(Msg_FileName) = False Then
                        objMI.SaveAs(Temp_FileName)

                        If gl_FPApp.ByteArray_getFile(Temp_FileName, FileBytes) Then
                            ReDim Preserve Attached_Files(UBound(Attached_Files) + 1)
                            With Attached_Files(UBound(Attached_Files))
                                .FileName = Msg_FileName
                                .Status = Enum_Attachement_Status.Save_It
                                .Bytes = FileBytes
                            End With
                        End If
                    End If
                Next

                Attachements_ListBox_UPDATE()
                FPc_DOCMAN_Doc_Attached.P_VALUE = (UBound(Attached_Files) > 0)

            Catch ex As Exception
                'Nothing to do
            End Try
        Else
            gl_FPApp.DoErrorMsgBox(FPf, "DOCMAN_Doc_Panel.DragDrop_DoIt", 0, "Unknown dragdrop format")
        End If
    End Sub

    Private Sub Fill_MustFields()
        If FP_DOCMAN_Docs.P_DATA_RecordStatus <> ENUM_RecordStatus.NORECORD Then
            If FP_DOCMAN_Docs.P_FORM_Dirty Then
                If FPc_DOCMAN_Doc_Types_ID.P_VALUE = ENUM_DocTypes.NOT_DEFINED Then
                    FPc_DOCMAN_Doc_Types_ID.P_VALUE = ENUM_DocTypes.OTHER_DOC
                End If

                If Trim(FPc_DOCMAN_Descr.P_VALUE = "") Then
                    If FPc_DOCMAN_Doc_Types_ID.P_VALUE = 0 Then
                        FPc_DOCMAN_Descr.P_VALUE = "???"
                    Else
                        FPc_DOCMAN_Descr.P_VALUE = FPc_DOCMAN_Doc_Types_ID.GET_TEXT
                    End If
                End If

                If FPc_DOCMAN_Doc_Date.P_VALUE = NULLDATE Then
                    FPc_DOCMAN_Doc_Date.P_VALUE = gl_FPApp.GET_SERVER_CURRENT_DATE(False)
                End If

                If FPc_DOCMAN_Scan_Date.P_VALUE = NULLDATE Then
                    FPc_DOCMAN_Scan_Date.P_VALUE = gl_FPApp.GET_SERVER_CURRENT_DATE(True)
                End If

                If FPc_DOCMAN_Doc_Types_ID.P_VALUE = ENUM_DocTypes.NOT_DEFINED Then
                    FPc_DOCMAN_Doc_Types_ID.P_VALUE = ENUM_DocTypes.OTHER_DOC
                End If

                If Trim(FPc_DOCMAN_RefNum.P_VALUE) = "" Then
                    FPc_DOCMAN_RefNum.P_VALUE = "-"
                End If
            End If
        End If
    End Sub

    Private Function Valid_Date_Required() As Boolean
        Dim OUT As Boolean = False
        Dim SQL As String = String.Format("SELECT ISNULL(Valid_Date_Required,0) V FROM doc_types WHERE ID = {0}", FPc_DOCMAN_Doc_Types_ID.P_VALUE)
        Dim DR As DataRow = gl_FPApp.DC.Qdf_get_DataRow(SQL)
        If DR IsNot Nothing Then
            If DR.Item("V") = 1 Then OUT = True
        End If
        Return OUT
    End Function

    Private Sub FP_DOCMAN_Docs_SET_LAYOUT()
        Dim Current_Is_Authentic_Copy As Boolean = (FPc_DOCMAN_Doc_Types_ID.P_VALUE = ENUM_DocTypes.INV_Authentic_Copy Or FPc_DOCMAN_Doc_Types_ID.P_VALUE = ENUM_DocTypes.CSHBK_Authentic_Copy)

        With FP_DOCMAN_Docs
            .P_FORM_AllowEdits = (Not Current_Is_Authentic_Copy)
            .P_FORM_AllowDeletions = (Not Current_Is_Authentic_Copy)
        End With

        If gl_FPApp.NEW_DEVELOPMENT_PARAMS_JSON_IsInstalled("DOCMAN_DEV_2021_03") Then
            FPc_DOCMAN_Valid_Date.P.Mandatory = Valid_Date_Required()
        End If

    End Sub

    Public Sub Attachements_SELECT_ALL()
        For Each It As ListViewItem In Attachements.Items
            It.Selected = True
        Next
    End Sub

    Public Sub Dispose()
        If Disposed = False Then
            If Not (FPc_DOCMAN_Doc_Attached_PictureCell Is Nothing) Then
                FPc_DOCMAN_Doc_Attached_PictureCell.Dispose()
            End If

            FP_DOCMAN_Docs.GRID.Dispose()
            FP_DOCMAN_Docs.Dispose()
            FP_DOCMAN_Docs = Nothing

            If Not (FPc_DOCMAN_Doc_Attached Is Nothing) Then
                FPc_DOCMAN_Doc_Attached.Dispose()
            End If

            If Not (FPc_DOCMAN_Doc_Date Is Nothing) Then
                FPc_DOCMAN_Doc_Date.Dispose()
            End If

            If Not (FPc_DOCMAN_Descr Is Nothing) Then
                FPc_DOCMAN_Descr.Dispose()
            End If

            If Not (FPc_DOCMAN_RefNum Is Nothing) Then
                FPc_DOCMAN_Descr.Dispose()
            End If

            If Not (FPc_DOCMAN_Doc_Types_ID Is Nothing) Then
                FPc_DOCMAN_Doc_Types_ID.Dispose()
            End If

            If Not (FPp_DOCMAN_ButtonRefresh Is Nothing) Then
                FPc_DOCMAN_Doc_Types_ID.Dispose()
            End If

            FP_Parent = Nothing
            FP_DOCMAN_Docs.Dispose()

            FPf = Nothing
            DOCMAN_Panel = Nothing


            GRID_Label.Dispose()
            GRID_Label = Nothing
            GRID.Dispose()
            GRID = Nothing
            DOCMAN_GRID_SavePoint.Dispose()
            DOCMAN_GRID_SavePoint = Nothing
            Attachements.Dispose()
            Attachements = Nothing

            If Not (Picturebox_ButtonRefresh Is Nothing) Then
                FPf.PICTUREBOXES_REMOVE(Picturebox_ButtonRefresh.Name)
                Picturebox_ButtonRefresh.Dispose()
                Picturebox_ButtonRefresh = Nothing
            End If

            If Not (Picturebox_ButtonAdd Is Nothing) Then
                FPf.PICTUREBOXES_REMOVE(Picturebox_ButtonAdd.Name)
                Picturebox_ButtonAdd.Dispose()
                Picturebox_ButtonAdd = Nothing
            End If

            If Not (Picturebox_ButtonSave Is Nothing) Then
                FPf.PICTUREBOXES_REMOVE(Picturebox_ButtonSave.Name)
                Picturebox_ButtonSave.Dispose()
                Picturebox_ButtonSave = Nothing
            End If

            Disposed = True
        End If
    End Sub

    Public Sub DORESYNC()
        FP_DOCMAN_Docs.FORM_DORESYNC(True)
    End Sub

#End Region

#Region "FUNCTIONS"

    Private Function Attachements_COPY_SELECTED_TO_Users_DOCMAN_Folder(WithOpen As Boolean, Optional ByRef OUT_ToFolder As String = "") As Boolean
        Dim OUT As Boolean = False

        OUT_ToFolder = gl_FPApp.SELEXPED_FolderName_GET(ENUM_SELEXPED_Folder_Types.TEMP) + "\DOCMAN_TEMP"
        Dim DoIt As Boolean = True

        Try
            If Directory.Exists(OUT_ToFolder) Then
                Try
                    Directory.Delete(OUT_ToFolder, True)

                Catch ex As Exception
                    'Nothing to do
                End Try
            End If
            If Not Directory.Exists(OUT_ToFolder) Then
                Directory.CreateDirectory(OUT_ToFolder)
            End If

        Catch ex As Exception
            DoIt = False
            gl_FPApp.DoErrorMsgBox("DOCMAN_Doc_Panel.Attachements_ItemDrag", Err.Number, Err.Description)
        End Try

        If DoIt Then
            OUT = Attachements_COPY_SELECTED(OUT_ToFolder, WithOpen)
        End If

        Return OUT
    End Function

    Private Function Attachements_DeleteOriginFiles() As Boolean
        If Attached_FilesWithPath Is Nothing Then
            GoTo Sub_End
        End If

        If Attached_FilesWithPath.Count > 1 Then
            If gl_FPApp.P.DOCMAN.DeleteAfterSave <> ENUM_DOCMAN.NO_DELETE Then
                If gl_FPApp.P.DOCMAN.DeleteAfterSave = ENUM_DOCMAN.DELETE_WITH_CONFIRM Then
                    Dim w As Integer = gl_FPApp.DoMyMsgBox(83008, , "SEQ,NEIN", "SEQ,JA")
                    If w = 1 Then
                        GoTo Sub_End
                    End If
                End If

                For i = 0 To Attached_FilesWithPath.Count - 1
                    If i <> 0 Then
                        Try
                            File.Delete(Attached_FilesWithPath(i))

                        Catch ex As Exception
                            gl_FPApp.DoMyMsgBox(83009)
                        End Try
                    End If
                Next
            End If
        End If

        GoTo Sub_End

Sub_End:
        Attachements_DeleteOriginFiles = True
        Attached_FilesWithPath = Nothing
        Exit Function
    End Function

    Public Function ATTACHEMENT_EXISTS(FileName As String, Optional with_Dialog As Boolean = True) As Boolean
        Dim OUT As Boolean = False

        FileName = Get_Descr_from_FileName(FileName, True)

        For Each It As ListViewItem In Attachements.Items
            If It.Text = FileName Then
                OUT = True

                If with_Dialog Then
                    gl_FPApp.DoMyMsgBox(83002, FileName)
                End If

                Exit For
            End If
        Next

        Return OUT
    End Function

    Private Function Attachements_Manage() As Boolean
        Dim OUT As Boolean = True

        If UBound(Attached_Files_BeforeSave) > 0 Then
            CURSOR_SHOW_WAIT()

            Dim Doc_Images_ID As Long = CURRENT_Doc_Images_ID_GET()



            For i As Integer = 1 To UBound(Attached_Files_BeforeSave)
                Select Case Attached_Files_BeforeSave(i).Status
                    Case Enum_Attachement_Status.Already_Saved
                        'Nothing to do

                    Case Else
                        Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

                        With gl_FPApp.DC
                            .Qdf_set_SP(sqlComm, "FP_DOCMAN_Docs_Panel_Manage_Doc")
                            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                            .Qdf_AddParameter(sqlComm, "@Doc_Images_ID", SqlDbType.Int, , , , , Doc_Images_ID)
                            .Qdf_AddParameter(sqlComm, "@Doc_Pages_ID", SqlDbType.Int, , , , , Attached_Files_BeforeSave(i).Pages_ID)
                            .Qdf_AddParameter(sqlComm, "@Manage_Type", SqlDbType.Int, , , , , Attached_Files_BeforeSave(i).Status)
                            .Qdf_AddParameter(sqlComm, "@Origin", SqlDbType.NVarChar, , 255, Get_Descr_from_FileName(Attached_Files_BeforeSave(i).FileName, True))

                            If Attached_Files_BeforeSave(i).Bytes Is Nothing Then
                                ReDim Attached_Files_BeforeSave(i).Bytes(0)
                            End If

                            .Qdf_AddParameter(sqlComm, "@ImageData", SqlDbType.VarBinary, , -1, , , , , , Attached_Files_BeforeSave(i).Bytes)

                            .Qdf_AddParameter(sqlComm, "@Barcode01", SqlDbType.NVarChar, , 50, "")

                            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
                            .Qdf_AddParameter(sqlComm, "@ErrField", SqlDbType.NVarChar, ParameterDirection.Output, 255)
                        End With

                        Try
                            OUT = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE)
                        Catch ex As Exception
                            gl_FPApp.DoErrorMsgBox("DOCMAN_Doc_Panel.CURRENT_DOC_DELETE_ALL_ATTACHEMENTS", Err.Number, Err.Description)
                        End Try
                End Select
            Next

            CURSOR_SHOW_DEFAULT()

            FP_DOCMAN_Docs.FORM_DORESYNC()
        End If

        Return OUT
    End Function

    Private Function Attachements_LOAD_DOC(Doc_Pages_ID As Long, ByRef OUT_Bytes() As Byte) As Boolean
        Dim OUT As Boolean = False
        Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

        ReDim OUT_Bytes(0)

        With gl_FPApp.DC
            .Qdf_set_SP(sqlComm, "FP_DOCMAN_Page_READ")
            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
            .Qdf_AddParameter(sqlComm, "@ID", SqlDbType.Int, , , , , Doc_Pages_ID)
            .Qdf_AddParameter(sqlComm, "@OldTransactID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@Doc_Images_ID", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@PageNum", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@Origin", SqlDbType.NVarChar, ParameterDirection.Output, 255)
            .Qdf_AddParameter(sqlComm, "@ImageData", SqlDbType.VarBinary, ParameterDirection.Output, -1)

            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
        End With

        CURSOR_SHOW_WAIT()
        Try
            OUT = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")
        Catch ex As Exception
            OUT = False
            FPf.FPApp.DoErrorMsgBox("FP.FORM_SET_RECORDSOURCE", Err.Number, Err.Description)
        End Try

        CURSOR_SHOW_DEFAULT()

        If OUT Then
            OUT_Bytes = sqlComm.Parameters("@ImageData").Value
        End If

        Return OUT
    End Function

    Private Function DOC_SAVE_TO_DOCMAN_SAVE_IMAGE(Doc_Images_ID As Long, Origin As String, DocData() As Byte) As Boolean
        Dim OUT As Boolean = False

        Dim sqlComm As SqlCommand = gl_FPApp.DC.CNN.CreateCommand

        With gl_FPApp.DC
            .Qdf_set_SP(sqlComm, "FP_DOCMAN_Docs_Panel_Manage_Doc")
            .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)

            .Qdf_AddParameter(sqlComm, "@Doc_Images_ID", SqlDbType.Int, , , , , Doc_Images_ID)
            .Qdf_AddParameter(sqlComm, "@Doc_Pages_ID", SqlDbType.Int, , , , , 0)
            .Qdf_AddParameter(sqlComm, "@Manage_Type", SqlDbType.Int, , , , , 1)
            .Qdf_AddParameter(sqlComm, "@Origin", SqlDbType.NVarChar, , 255, Origin)
            .Qdf_AddParameter(sqlComm, "@ImageData", SqlDbType.VarBinary, , -1, , , , , , DocData)
            .Qdf_AddParameter(sqlComm, "@Barcode01", SqlDbType.NVarChar, , 50, "")

            .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
            .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
            .Qdf_AddParameter(sqlComm, "@ErrField", SqlDbType.NVarChar, ParameterDirection.Output, 255)

        End With

        CURSOR_SHOW_WAIT()
        Try
            OUT = gl_FPApp.DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")

        Catch ex As Exception
            OUT = False
            gl_FPApp.DoErrorMsgBox("FP_Word.Doc_SAVE_TO_DOCMAN", Err.Number, Err.Description)
        End Try

        Return OUT
    End Function

    Private Function Get_Descr_from_FileName(FileNameWithPath As String, Optional withExtension As Boolean = False) As String
        Dim OUT As String = FileNameWithPath
        Dim p As Integer

        If withExtension = False Then
            p = InStrRev(FileNameWithPath, ".")
            If p > 0 Then
                OUT = Mid(OUT, 1, p - 1)
            End If
        End If

        p = InStrRev(FileNameWithPath, "\")
        If p > 0 Then
            OUT = Mid(OUT, p + 1)
        End If

        p = InStrRev(FileNameWithPath, "/")
        If p > 0 Then
            OUT = Mid(OUT, p + 1)
        End If

        Return OUT
    End Function

    Public Function Attachements_Right_for_Manage(Optional WithDialog As Boolean = True) As Boolean
        Dim OUT As Boolean = False

        If FP_DOCMAN_Docs.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
            If FPc_DOCMAN_Doc_Security_Level.P_VALUE > gl_FPApp.UserRights.DOCMAN_SecurityLevel Then
                OUT = False
                If WithDialog Then
                    gl_FPApp.DoMyMsgBox(83001) 'Nincs joga kezelni ezt a dokumentumot.
                End If
            Else
                OUT = True
            End If
        End If

        Return OUT
    End Function

    Public Function Attachements_COPY_SELECTED(ToFolder As String, Optional WithOpen As Boolean = False, Optional WithOverwrite As Boolean = True) As Boolean
        Dim OUT As Boolean = True

        For Each It As ListViewItem In Attachements.Items
            If It.Selected Then
                With Attached_Files(Val(It.Name))
                    If .Bytes Is Nothing Then
                        ReDim .Bytes(0)
                    End If
                    If UBound(.Bytes) = 0 Then
                        OUT = Attachements_LOAD_DOC(.Pages_ID, .Bytes)
                    End If
                    If OUT Then
                        Dim FileName_With_Path As String = ToFolder + "\" + It.Text
                        Dim w As Integer = 2

                        If WithOverwrite = False Then
                            If File.Exists(FileName_With_Path) Then
                                w = gl_FPApp.DoMyMsgBox(83011, FileName_With_Path, "SEQ,NEIN", "SEQ,JA")
                            End If
                        End If

                        If w = 2 Then
                            gl_FPApp.ByteArray_SaveFile(FileName_With_Path, .Bytes)
                            If WithOpen Then
                                Process.Start(FileName_With_Path)
                            End If
                        End If
                    End If
                End With
            End If
        Next

        Return OUT
    End Function

    Public Function Attachements_DEL_SELECTED() As Boolean
        Dim OUT As Boolean = True
        Dim SetDirty As Boolean = False

        For Each It As ListViewItem In Attachements.Items
            If It.Selected Then
                Attached_Files(Val(It.Name)).Status = Enum_Attachement_Status.Delete_It
                SetDirty = True
            End If
        Next

        If SetDirty Then
            FP_DOCMAN_Docs.FORM_DIRTY_SET()
            Attachements_ListBox_UPDATE()
        End If

        Return OUT
    End Function

    Public Function Control_FieldPrefix_And_Prefix() As String
        Dim OUT As String = Control_Prefix

        If Fieldprefix > "" Then
            OUT = Fieldprefix + OUT
        End If

        Return OUT
    End Function

    Public Function CURRENT_Doc_Images_ID_GET() As Long
        Return Val(FP_DOCMAN_Docs.DATA_Field_getSavedValue("DOCMAN_Doc_Images_ID"))
    End Function

    Public Function CURRENT_DOC_DELETE_ALL_ATTACHEMENTS() As Boolean
        Dim OUT As Boolean = True

        If FP_DOCMAN_Docs.FORM_RECORDS_SAVE_CURRENT Then
            If FP_DOCMAN_Docs.P_DATA_RecordStatus = ENUM_RecordStatus.EXISTS Then
                Dim Doc_Images_ID As Long = CURRENT_Doc_Images_ID_GET()

                If Doc_Images_ID <> 0 Then
                    If FP_DOCMAN_Docs.FORM_GETRIGHT("FORM_DELETE") Then
                        Dim sqlComm As SqlCommand = FPf.FPApp.DC.CNN.CreateCommand()

                        With gl_FPApp.DC
                            .Qdf_set_SP(sqlComm, "FP_DOCMAN_Docs_Panel_Del_All")
                            .Qdf_AddParameter(sqlComm, "@Doc_Images_ID", SqlDbType.Int, , , , , Doc_Images_ID)
                        End With

                        CURSOR_SHOW_WAIT()
                        Try
                            OUT = FPf.Qdf_Execute(sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE)
                        Catch ex As Exception
                            gl_FPApp.DoErrorMsgBox("DOCMAN_Doc_Panel.CURRENT_DOC_DELETE_ALL_ATTACHEMENTS", Err.Number, Err.Description)
                        End Try
                    End If
                End If
            End If
        End If

        Return OUT
    End Function

    Public Function SHOW_ARCHIV_DIALOG() As Boolean
        Dim OUT As Boolean = False
        Dim Frm_Archive_MSG As New SEL_DOCMAN_ARCHIV_MSG

        OUT = (gl_FPApp.ShowDialogForm(Frm_Archive_MSG) = Windows.Forms.DialogResult.OK)

        Return OUT
    End Function

    Public Function Doc_Types_GET_FROM_STR(Doc_Types_Name_Or_ID As String, ByRef OUT_Doc_Types_ID As Long, Optional WithDialog As Boolean = True, Optional DialNum As Long = 93001) As Boolean
        Dim OUT As Boolean = False

        OUT_Doc_Types_ID = 0

        Dim MySQL As String = String.Format("SELECT ID FROM Doc_Types WHERE Descr = '{0}'", Doc_Types_Name_Or_ID)
        Dim Drow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)

        If Drow Is Nothing Then
            Dim MyID As Long = CInt(Val(Doc_Types_Name_Or_ID))

            If MyID <> 0 Then
                MySQL = String.Format("SELECT ID FROM Doc_Types WHERE ID = {0}", MyID)

                Drow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)
            End If
        End If

        If Drow Is Nothing Then
            If WithDialog Then
                gl_FPApp.DoMyMsgBox(DialNum, Doc_Types_Name_Or_ID) 'Ismeretlen dokumentum tipus
            End If
        Else
            OUT_Doc_Types_ID = Drow!ID
            OUT = True
        End If

        Return OUT
    End Function

    Public Function DOC_SAVE_TO_DOCMAN(P As Struct_DOCMAN_Doc_Params, Optional WithDialog As Boolean = True, Optional ByRef OUT_ID As Long = 0) As Boolean
        Dim OUT As Boolean = True
        Dim DocData() As Byte = Nothing
        Dim PDF_DocData() As Byte = Nothing
        Dim Doc_Images_ID As Long = 0

        Dim FileName_without_Extension As String = getFileName_without_Extension(P.FileName_with_Path)

        If OUT Then
            If FileName_without_Extension > "" Then
                If WithDialog Then
                    OUT = (gl_FPApp.DoMyMsgBox(83005, FileName_without_Extension, "SEQ,YES", "SEQ,NO") = 1)
                End If
            End If
        End If

        If OUT Then
            If FileName_without_Extension > "" Then
                Try
                    Dim TimeStamp As String = Format(Now, "yyMMdd_HHmmss")

                    OUT = gl_FPApp.ByteArray_getFile(P.FileName_with_Path, DocData)

                Catch ex As Exception
                    OUT = False
                End Try

                If OUT = False Then
                    If WithDialog Then
                        gl_FPApp.DoMyMsgBox(83004) 'A dokumentumot ido kozben toroltek vagy atmozgattak masik helyre.
                    End If
                End If
            End If
        End If

        If OUT Then
            Dim sqlComm As SqlCommand = gl_FPApp.DC.CNN.CreateCommand()

            With gl_FPApp.DC
                .Qdf_set_SP(sqlComm, "FP_DOCMAN_Docs_Panel_SAVE")
                .Qdf_AddParameter(sqlComm, "@Terminal", SqlDbType.NVarChar, , 10, Terminal)
                .Qdf_AddParameter(sqlComm, "@ID", SqlDbType.Int, ParameterDirection.Output, , , , 0)
                .Qdf_AddParameter(sqlComm, "@OldTransactID", SqlDbType.Int, , , , , 0)

                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Images_ID", SqlDbType.Int, , , , , 0)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Types_ID", SqlDbType.Int, , , , , P.DOCMAN_Doc_Types_ID)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_RefNum", SqlDbType.NVarChar, , 50, P.DOCMAN_RefNum)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_CUST_ID", SqlDbType.Int, , , , , P.DOCMAN_CUST_ID)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_CUST_Name1", SqlDbType.NVarChar, , 50, P.DOCMAN_CUST_Name1)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Date", SqlDbType.DateTime, , , , P.DOCMAN_Doc_Date)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Scan_Date", SqlDbType.DateTime, , , , P.DOCMAN_Scan_Date)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Descr", SqlDbType.NVarChar, , 255, P.DOCMAN_Descr)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Status_ID", SqlDbType.Int, , , , , P.DOCMAN_Doc_Status_ID)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Doc_Security_Level", SqlDbType.Int, , , , , P.DOCMAN_Doc_Security_Level)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Parent_TableName", SqlDbType.NVarChar, , 50, P.DOCMAN_Parent_TableName)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Parent_Record_ID", SqlDbType.Int, , , , , P.DOCMAN_Parent_Record_ID)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Sent_date", SqlDbType.DateTime, , , , P.DOCMAN_Doc_Date)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Message", SqlDbType.NVarChar, , -1, P.DOCMAN_Message)
                .Qdf_AddParameter(sqlComm, "@DOCMAN_Origin", SqlDbType.NVarChar, , 255, P.DOCMAN_Origin)

                .Qdf_AddParameter(sqlComm, "@Result", SqlDbType.Int, ParameterDirection.Output)
                .Qdf_AddParameter(sqlComm, "@ErrText", SqlDbType.NVarChar, ParameterDirection.Output, 255)
                .Qdf_AddParameter(sqlComm, "@ErrField", SqlDbType.NVarChar, ParameterDirection.Output, 255)
            End With

            CURSOR_SHOW_WAIT()
            Try
                OUT = gl_FPApp.DC.Qdf_Execute("", sqlComm, , ENUM_ERRDIAL_TYPE.DIALNUM_RETVALUE, "@ErrText")

                OUT_ID = sqlComm.Parameters("@ID").Value
                Dim MySQL As String = String.Format("SELECT Doc_Images_ID FROM Doc_Parents WHERE ID = {0}", OUT_ID)
                Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)

                Doc_Images_ID = DRow!Doc_Images_ID

            Catch ex As Exception
                OUT = False
                gl_FPApp.DoErrorMsgBox("DOCMAN_Doc_Panel.REPORT_SAVE_TO_DOCMAN", Err.Number, Err.Description)
            End Try
        End If

        If OUT Then
            If FileName_without_Extension > "" Then
                OUT = DOC_SAVE_TO_DOCMAN_SAVE_IMAGE(Doc_Images_ID, P.DOCMAN_Origin, DocData)
            End If
        End If

        CURSOR_SHOW_DEFAULT()

        Return OUT
    End Function

#End Region

End Class
