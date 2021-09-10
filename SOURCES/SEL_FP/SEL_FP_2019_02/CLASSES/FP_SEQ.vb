Imports System.Data
Imports System.Data.SqlClient

Public Class FP_SEQ
    Private Disposed As Boolean = False
    Private FPApp As FP_App
    Public DT_SEQ As New DataTable
    Public SEQ_Key As String = ""

    Sub New(ByVal MyFPApp As FP_App)
        FPApp = MyFPApp
    End Sub
    Sub New(ByVal MyFPApp As FP_App, ByVal MySEQ_Key As String, Optional ByVal SubWHERE As String = "")
        FPApp = MyFPApp
        SEQ_Key = MySEQ_Key
        LOAD_SEQ(MySEQ_Key, SubWHERE)
    End Sub

    Public Sub Dispose()
        Disposed = True
    End Sub

    Public Sub CLEAR()
        DT_SEQ.Clear()
    End Sub
    Public Function LOAD_SEQ(ByVal SEQ_Key As String, Optional ByVal SubWHERE As String = "") As Boolean
        Dim OUT As Boolean = False

        Dim MyWHERE As String = ""

        If SEQ_Key > "" Then
            SEQ_Key = String.Format("SEQ_Key = '{0}'", SEQ_Key)
        End If

        MyWHERE = TEXT_AND(SEQ_Key, SubWHERE)
        MyWHERE = TEXT_AND(MyWHERE, String.Format("Language = '{0}'", FPApp.LandDialog))

        If MyWHERE > "" Then
            MyWHERE = " WHERE " + MyWHERE
        End If

        Dim MySQL As String = "SELECT SEQ_Key, Number, Text1, Text2, Text3, Text4 FROM VB_SEQ " + MyWHERE + " ORDER BY SEQ_Key, Number"

        If FPApp.DC.P_USE_LocalDB Then
            FPApp.DC.LocalDB_SEL.Fill_DT(MySQL, DT_SEQ)
        Else
            FPApp.DC.Qdf_Fill_DT(MySQL, DT_SEQ)
        End If

        LOAD_SEQ = OUT
    End Function
    Function COUNT(Optional ByVal SubWHERE As String = "SEQ_Key like '%'") As Long
        COUNT = DT_SEQ.Select(SubWHERE).Count
    End Function

    Public Function Struct_SEQ_Empty() As Struct_SEQ
        Dim OUT As New Struct_SEQ

        With OUT
            .SEQ_Key = ""
            .Number = 0
            .Text1 = ""
            .Text2 = ""
            .Text3 = ""
            .Text4 = ""
        End With

        Return OUT
    End Function

    Function GET_SEQ_BY_NUMBER(ByVal Number As Long) As Struct_SEQ
        Dim OUT As Struct_SEQ = Struct_SEQ_Empty()

        Dim Criteria As String = String.Format("Number = {0}", Number.ToString)

        OUT.SEQ_Key = SEQ_Key
        OUT.Number = 0

        If COUNT(Criteria) Then
            Dim Row As DataRow = DT_SEQ.Select(Criteria).First()

            With Row
                OUT.SEQ_Key = !SEQ_Key
                OUT.Number = !Number
                OUT.Text1 = FPApp.Text_Replace_Standard_Params(nz(!Text1, ""))
                OUT.Text2 = FPApp.Text_Replace_Standard_Params(nz(!Text2, ""))
                OUT.Text3 = FPApp.Text_Replace_Standard_Params(nz(!Text3, ""))
                OUT.Text4 = FPApp.Text_Replace_Standard_Params(nz(!Text4, ""))
            End With

        End If

        GET_SEQ_BY_NUMBER = OUT
    End Function

    Function GET_SEQ_BY_TEXT1(ByVal Text1 As String) As Struct_SEQ
        Dim OUT As Struct_SEQ = Struct_SEQ_Empty()
        Dim Criteria As String = String.Format("Text1 = '{0}'", Text1)

        OUT.SEQ_Key = SEQ_Key
        OUT.Number = 0

        If COUNT(Criteria) Then
            Dim Row As DataRow = DT_SEQ.Select(Criteria).First()

            With Row
                OUT.SEQ_Key = !SEQ_Key
                OUT.Number = !Number
                OUT.Text1 = FPApp.Text_Replace_Standard_Params(!Text1)
                OUT.Text2 = FPApp.Text_Replace_Standard_Params(!Text2)
                OUT.Text3 = FPApp.Text_Replace_Standard_Params(!Text3)
                OUT.Text4 = FPApp.Text_Replace_Standard_Params(!Text4)
            End With

        End If

        GET_SEQ_BY_TEXT1 = OUT
    End Function

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub
End Class
