Public Class CL_AddressTypes
    Private DT_AddressTypes As DataTable

    Sub New()
        gl_FPApp.DC.Qdf_Fill_DT("SELECT * FROM AddressTypes", DT_AddressTypes)
    End Sub

    Private Function DRow_GET_FROM_AddressTypes_ID(AddressTypes_ID As ENUM_AddressTypes) As DataRow
        Dim OUT As DataRow = Nothing

        Dim Crit As String = String.Format("ID = {0}", CInt(AddressTypes_ID))

        If DT_AddressTypes.Select(Crit).Count = 1 Then
            OUT = DT_AddressTypes.Select(Crit).First
        End If

        Return OUT
    End Function

    Public Function GET_ParentTable_ID(AddressTypes_ID As ENUM_AddressTypes) As Integer
        Dim OUT As Integer = 0
        Dim DRow As DataRow = DRow_GET_FROM_AddressTypes_ID(AddressTypes_ID)

        If Not (DRow Is Nothing) Then
            OUT = DRow!ParentTable_ID
        End If

        Return OUT
    End Function
End Class
