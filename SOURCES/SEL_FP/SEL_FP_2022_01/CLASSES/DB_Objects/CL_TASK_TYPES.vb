Public Class CL_TASK_TYPES
    Public Enum ENUM_TASKMAN_TYPE_OF_ANSWER As Integer
        OPEN_CLOSE = 0
        YES_NO = 1
    End Enum

    Public WithEvents FPApp_for_Messages As FP_App

    Private DT_TM_STATUS_TYPES As DataTable

    Sub New()
        FPApp_for_Messages = gl_FPApp

        REFRESH()
    End Sub

    Public Sub REFRESH()
        If P_TASKMAN_INSTALLED Then
            With gl_FPApp.DC
                .Qdf_Fill_DT("SELECT * FROM TM_TASK_TYPES", DT_TM_STATUS_TYPES)
            End With
        End If
    End Sub

    Public ReadOnly Property P_TASKMAN_INSTALLED As Boolean
        Get
            Return gl_FPApp.Installed_Products_Exists("TASKMAN")
        End Get
    End Property

    Private Function GET_TM_TASK_TYPES_DRow_BY_ID(TM_STATUSCODES_ID As Long) As DataRow
        Dim OUT As DataRow = Nothing

        Dim Crit As String = String.Format("ID={0}", TM_STATUSCODES_ID)

        If DT_TM_STATUS_TYPES.Select(Crit).Count > 0 Then
            OUT = DT_TM_STATUS_TYPES.Select(Crit).First
        End If

        Return OUT
    End Function

    Public Function GET_Type_of_Answer(Current_TM_TASK_TYPES_ID As Long) As CL_TASK_TYPES.ENUM_TASKMAN_TYPE_OF_ANSWER
        Dim OUT As CL_TASK_TYPES.ENUM_TASKMAN_TYPE_OF_ANSWER = CL_TASK_TYPES.ENUM_TASKMAN_TYPE_OF_ANSWER.OPEN_CLOSE

        If Current_TM_TASK_TYPES_ID <> 0 Then
            Dim DRow As DataRow = GET_TM_TASK_TYPES_DRow_BY_ID(Current_TM_TASK_TYPES_ID)

            If Not (DRow Is Nothing) Then
                OUT = DRow!Type_of_Answer
            End If
        End If

        Return OUT
    End Function
End Class
