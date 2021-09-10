Public Class CL_Statuscodes
    Public Enum ENUM_STATUS_CHANGE_TYPE As Integer
        MANUAL = 0
        MANUAL_WITH_CONFIRM = 1
        AUTOMATED = 99
        AUTOMATED_ALL_CALC_INVOICED = 2 'Automatikusan, ha minden kalkuláció számlába van állítva.
        AUTOMATED_ALL_OUTGOING_INVOICED = 3 'Automatikusan, ha minden kimenő kalkuláció számlába van állítva.
        AUTOMATED_ALL_INCOMING_INVOICED = 4 'Automatikusan, ha minden bejövő kalkuláció számlába van állítva.
        AUTOMATED_ALL_OUTGOING_INVOICED_AND_PAYED = 5 'Automatikusan, ha minden kimenő számla fizetve van.
        AUTOMATED_ALL_INCOMING_INVOICED_AND_PAYED = 6 'Automatikusan, ha minden bejövő számla fizetve van.
        AUTOMATED_ALL_CALC_INVOICED_AND_PAYED = 7 'Automatikusan, ha minden számla fizetve van.
        AUTOMATED_AGAIN_NOT_ALL_OUTGOING_INVOICED = 8 'Automatikusan, ha ismét nincs minden kimenő kalkuláció kiszámlázva.
        AUTOMATED_AGAIN_NOT_ALL_INCOMING_INVOICED = 9 'Automatikusan, ha ismét nincs minden bejövő kalkuláció kiszámlázva.
        AUTOMATED_AGAIN_NOT_ALL_CALC_INVOICED = 10 'Automatikusan, ha ismét nincs minden kalkuláció számlába állítva.
        AUTOMATED_AGAIN_NOT_ALL_OUTGOING_PAYED = 11 'Automatikusan, ha ismét nincs minden kimenő számla kifizetve.
        AUTOMATED_AGAIN_NOT_ALL_INCOMING_PAYED = 12 'Automatikusan, ha ismét nincs minden bejövő számla kifizetve.
        AUTOMATED_AGAIN_NOT_ALL_INVOICED = 13 'Automatikusan, ha ismét nincs minden számla kifizetve.

        NOT_ALLOWED = -999
    End Enum

    Public Enum ENUM_STATUS_TYPES As Integer
        OFFER = -1005    'Ajanlatbol indito statusz
        FIRST = 1        'Kezdo statusz
        NORMAL = 0       'Normal, folyamat kozben kiadhato statusz
        CLOSE = 2        'Lezaro statusz
        CANCEL = 3       'Sztorno statusz
    End Enum

    Public Enum ENUM_STATUS_LEVELS As Integer
        NOT_DEFINED = 0
        ORD = 1
        ORD_L = 2
        ORD_GOODS = 21
        TM = 101
        ORD_CONT = 201
        ORDER_WRHS = 11
        ORDER_WRHS_L = 20
    End Enum

    Public Enum ENUM_STATUS_MANAGEMENT_TYPE As Integer
        EASY = 0
        COMPLEX = 1
    End Enum

    Public WithEvents FPApp_for_Messages As FP_App

    Private DT_Statuscodes As DataTable
    Private DT_Statuscodes_Next As DataTable
    Private DT_Statuscodes_Hierarchy As DataTable
    Private SEQ_STATUS_CHANGE_CONFIRM As FP_SEQ = Nothing
    Private SEQ_STATUS_ERR_MSG As FP_SEQ = Nothing

    Sub New()
        FPApp_for_Messages = gl_FPApp

        REFRESH()
    End Sub

    Public Sub REFRESH()
        With gl_FPApp.DC
            .Qdf_Fill_DT("SELECT * FROM StatusCodes", DT_Statuscodes)
            .Qdf_Fill_DT("SELECT * FROM Statuscodes_After", DT_Statuscodes_Next)
            .Qdf_Fill_DT("SELECT * FROM Statuscodes_Hierarchy", DT_Statuscodes_Hierarchy)
        End With

        SEQ_STATUS_CHANGE_CONFIRM = New FP_SEQ(gl_FPApp, "STATUSCODES_AFTER_CONFIRM_MSG")
        SEQ_STATUS_ERR_MSG = New FP_SEQ(gl_FPApp, "STATUSCODES_RULES_ERR_MSG")
    End Sub

    Public ReadOnly Property P_Statusmanagement_Type As ENUM_STATUS_MANAGEMENT_TYPE
        Get
            Dim OUT As ENUM_STATUS_MANAGEMENT_TYPE = ENUM_STATUS_MANAGEMENT_TYPE.EASY

            If gl_FPApp.Installed_Products_Exists("STATUSCODES_COMPLEX") Then
                OUT = ENUM_STATUS_MANAGEMENT_TYPE.COMPLEX
            End If

            Return OUT
        End Get
    End Property

    Private Function GET_Statuscodes_DRow_BY_ID(StatusID As Long) As DataRow
        Dim OUT As DataRow = Nothing
        Dim Crit As String = String.Format("ID={0}", StatusID)

        If DT_Statuscodes.Select(Crit).Count > 0 Then
            OUT = DT_Statuscodes.Select(Crit).First
        End If

        Return OUT
    End Function

    Private Function GET_Statuscodes_After_DRow_BY_ID(StatusID As Long, Next_StatusID As Long) As DataRow
        Dim OUT As DataRow = Nothing
        Dim Crit As String = String.Format("StatusID = {0} AND After_StatusID = {1}", StatusID, Next_StatusID)

        If DT_Statuscodes_Next.Select(Crit).Count > 0 Then
            OUT = DT_Statuscodes_Next.Select(Crit).First
        End If

        Return OUT
    End Function

    Public Function GET_Status_Change_Type(Current_StatusID As Long, Next_StatusID As Long) As ENUM_STATUS_CHANGE_TYPE
        Dim OUT As ENUM_STATUS_CHANGE_TYPE = ENUM_STATUS_CHANGE_TYPE.NOT_ALLOWED

        If Current_StatusID = 0 Then
            Dim DRow As DataRow = GET_Statuscodes_DRow_BY_ID(Next_StatusID)

            If Not (DRow Is Nothing) Then
                If nz(DRow!Default_Status, False) = True Then
                    OUT = ENUM_STATUS_CHANGE_TYPE.MANUAL
                End If
            End If
        Else
            Dim DRow As DataRow = GET_Statuscodes_After_DRow_BY_ID(Current_StatusID, Next_StatusID)

            If Not (DRow Is Nothing) Then
                OUT = DRow!StatusChange_Type
            End If
        End If


        Return OUT
    End Function

    Public Function GET_Last_StatusID(for_RecordID As Long) As Long
        Dim OUT As Long = 0

        Dim MySQL As String = String.Format("SELECT dbo.STATUSCODES_GET_PREV({0}) PREV_STATUS_ID", for_RecordID)
        Dim DRow As DataRow = gl_FPApp.DC.Qdf_get_DataRow(MySQL)

        If Not (DRow Is Nothing) Then
            OUT = DRow!PREV_STATUS_ID
        End If

        Return OUT
    End Function

    Public Function GET_Next_StatusIDs_with_Separator(Current_StatusID As Long, Optional Separator As String = ", ") As String
        Dim OUT As String = ""
        Dim Crit As String = String.Format("", Current_StatusID, ENUM_STATUS_CHANGE_TYPE.MANUAL, ENUM_STATUS_CHANGE_TYPE.MANUAL_WITH_CONFIRM)
        Dim Current_Separator As String = ""
        Dim DT_of_selected As DataTable = DT_Statuscodes_Next.Select(Crit).Clone

        For Each DRow As DataRow In DT_of_selected.Rows
            OUT += Current_Separator + (DRow!After_StatusID).ToString
            Current_Separator = Separator
        Next

        Return OUT
    End Function

    Public Function GET_Status_Change_CONFIRM_MSG(Current_StatusID As Long, Next_StatusID As Long) As String
        Dim OUT As String = ""

        If GET_Status_Change_Type(Current_StatusID, Next_StatusID) = ENUM_STATUS_CHANGE_TYPE.MANUAL_WITH_CONFIRM Then
            Dim Statuscodes_Next_ID As Long = GET_Statuscodes_After_DRow_BY_ID(Current_StatusID, Next_StatusID)!ID

            OUT = SEQ_STATUS_CHANGE_CONFIRM.GET_SEQ_BY_NUMBER(Statuscodes_Next_ID).Text1
        End If

        Return OUT
    End Function

    Public Function GET_Status_Change_Allowed(Current_StatusID As Long, Next_StatusID As Long) As Boolean
        Dim OUT As Boolean = False

        If GET_Status_Change_Type(Current_StatusID, Next_StatusID) <> ENUM_STATUS_CHANGE_TYPE.NOT_ALLOWED Then
            OUT = True
        End If

        Return OUT
    End Function

    Public Function GET_Status_Code_BY_ID(StatusID As Long) As String
        Dim OUT As String = ""

        Dim DRow As DataRow = GET_Statuscodes_DRow_BY_ID(StatusID)

        If Not (DRow Is Nothing) Then
            OUT = DRow!KurzZeichen
        End If

        Return OUT
    End Function

    Public Function GET_Status_PictureCode(StatusID As Long) As String
        Dim OUT As String = ""
        Dim DRow As DataRow = GET_Statuscodes_DRow_BY_ID(StatusID)

        If Not (DRow Is Nothing) Then
            OUT = DRow!PictureCode
        End If

        Return OUT
    End Function

    Public Function GET_StatusField_BackColor(StatusID As Long) As Color
        Dim OUT As Color = COLORS_FIELD_NORMAL_BG

        Dim DRow As DataRow = GET_Statuscodes_DRow_BY_ID(StatusID)

        If Not (DRow Is Nothing) Then
            OUT = Color.FromArgb(DRow!StatusField_BackColor)
        End If

        If OUT.Name = "0" Then
            OUT = COLORS_FIELD_NORMAL_BG
        End If

        Return OUT
    End Function

    Public Function GET_StatusField_ForeColor(StatusID As Long) As Color
        Dim OUT As Color = COLORS_FIELD_NORMAL_FORE

        Dim DRow As DataRow = GET_Statuscodes_DRow_BY_ID(StatusID)

        If Not (DRow Is Nothing) Then
            OUT = Color.FromArgb(DRow!StatusField_ForeColor)
        End If

        If OUT.Name = "0" Then
            OUT = COLORS_FIELD_NORMAL_FORE
        End If
        Return OUT
    End Function

    Private Function GET_Statuscodes_Hierarchy_DRow_BY_Status_Level_Code(Status_Level_Code As String) As DataRow
        Dim OUT As DataRow = Nothing

        If Status_Level_Code > "" Then
            Dim Crit As String = String.Format("Status_Level_Code = '{0}'", Status_Level_Code)

            If DT_Statuscodes_Hierarchy.Select(Crit).Count > 0 Then
                OUT = DT_Statuscodes_Hierarchy.Select(Crit).First
            End If
        End If

        Return OUT
    End Function

    Private Function GET_Statuscodes_Hierarchy_DRow_BY_Hierarchy_Code_AND_Status_Level(Hierarchy_Code As String, Status_Level As Integer) As DataRow
        Dim OUT As DataRow = Nothing

        Dim Crit As String = String.Format("Hierarchy_Code = '{0}' AND Status_Level = {1}", Hierarchy_Code, Status_Level)

        If DT_Statuscodes_Hierarchy.Select(Crit).Count > 0 Then
            OUT = DT_Statuscodes_Hierarchy.Select(Crit).First
        End If

        Return OUT
    End Function

    Private Function GET_Statuscodes_Hierarchy_DRow_BY_Hierarchy_Code_AND_Parent_Status_Level(Hierarchy_Code As String, Parent_Status_Level As Integer) As DataRow
        Dim OUT As DataRow = Nothing

        Dim Crit As String = String.Format("Hierarchy_Code = '{0}' AND Parent_Status_Level = {1}", Hierarchy_Code, Parent_Status_Level)

        If DT_Statuscodes_Hierarchy.Select(Crit).Count > 0 Then
            OUT = DT_Statuscodes_Hierarchy.Select(Crit).First
        End If

        Return OUT
    End Function

    Public Function GET_Status_Type(Status_ID As Long) As ENUM_STATUS_TYPES
        Dim OUT As ENUM_STATUS_TYPES = ENUM_STATUS_TYPES.NORMAL
        Dim DRow As DataRow = GET_Statuscodes_DRow_BY_ID(Status_ID)

        If Not (DRow Is Nothing) Then
            OUT = DRow!Statuscode_Type
        End If

        Return OUT
    End Function

    Public Function GET_Default_Status_ID(Hierarchy_Code As String, Status_Level As Integer) As Long
        Dim OUT As Long = 0

        Dim Crit As String = String.Format("Hierarchy_Code = '{0}' AND OrderLevel = {1} AND Default_Status = 1", Hierarchy_Code, Status_Level)

        If DT_Statuscodes.Select(Crit).Count > 0 Then
            OUT = DT_Statuscodes.Select(Crit).First!ID
        End If

        Return OUT
    End Function

    Public Function GET_Statuscodes_Hierarchy_Parent_Status_Level(Status_Level_Code As String) As ENUM_STATUS_LEVELS
        Dim OUT As ENUM_STATUS_LEVELS = ENUM_STATUS_LEVELS.NOT_DEFINED

        Dim DRow As DataRow = GET_Statuscodes_Hierarchy_DRow_BY_Status_Level_Code(Status_Level_Code)

        If Not (DRow Is Nothing) Then
            Dim DRow_2 As DataRow = GET_Statuscodes_Hierarchy_DRow_BY_Hierarchy_Code_AND_Status_Level(DRow!Hierarchy_Code, DRow!Parent_Status_Level)

            If Not (DRow_2 Is Nothing) Then
                OUT = DRow_2!Status_Level
            End If
        End If

        Return OUT
    End Function

    Public Function GET_Statuscodes_Hierarchy_Parent_Status_Level_Code(Status_Level_Code As String) As String
        Dim OUT As String = ""

        Dim DRow As DataRow = GET_Statuscodes_Hierarchy_DRow_BY_Status_Level_Code(Status_Level_Code)

        If Not (DRow Is Nothing) Then
            Dim DRow_2 As DataRow = GET_Statuscodes_Hierarchy_DRow_BY_Hierarchy_Code_AND_Status_Level(DRow!Hierarchy_Code, DRow!Parent_Status_Level)

            If Not (DRow_2 Is Nothing) Then
                OUT = DRow_2!Status_Level_Code
            End If
        End If

        Return OUT
    End Function

    Public Function GET_Statuscodes_Hierarchy_Child_Status_Level(Status_Level_Code As String) As ENUM_STATUS_LEVELS
        Dim OUT As ENUM_STATUS_LEVELS = ENUM_STATUS_LEVELS.NOT_DEFINED

        Dim DRow As DataRow = GET_Statuscodes_Hierarchy_DRow_BY_Status_Level_Code(Status_Level_Code)

        If Not (DRow Is Nothing) Then
            Dim Hierarchy_Code = DRow!Hierarchy_Code
            Dim Current_Status_Level = DRow!Status_Level

            Dim DRow_2 As DataRow = GET_Statuscodes_Hierarchy_DRow_BY_Hierarchy_Code_AND_Parent_Status_Level(Hierarchy_Code, Current_Status_Level)

            If Not (DRow_2 Is Nothing) Then
                OUT = DRow_2!Status_Level
            End If
        End If

        Return OUT
    End Function

    Public Function GET_Statuscodes_Hierarchy_Child_Status_Level_Code(Status_Level_Code As String) As String
        Dim OUT As String = ""

        Dim DRow As DataRow = GET_Statuscodes_Hierarchy_DRow_BY_Status_Level_Code(Status_Level_Code)

        If Not (DRow Is Nothing) Then
            Dim Hierarchy_Code = DRow!Hierarchy_Code
            Dim Current_Status_Level = DRow!Status_Level

            Dim DRow_2 As DataRow = GET_Statuscodes_Hierarchy_DRow_BY_Hierarchy_Code_AND_Parent_Status_Level(Hierarchy_Code, Current_Status_Level)

            If Not (DRow_2 Is Nothing) Then
                OUT = DRow_2!Status_Level_Code
            End If
        End If

        Return OUT
    End Function

    Private Sub FPApp_Message(sender As FP_App, From_FPf As FP_Form, MessageCode As String, ByRef Individual_Params As Object, ByRef Handled As Boolean) Handles FPApp_for_Messages.Message
        Select Case MessageCode
            Case "STATUSCODES_REFRESHED"
                REFRESH()

            Case Else
                'Nothing to do
        End Select
    End Sub
End Class
