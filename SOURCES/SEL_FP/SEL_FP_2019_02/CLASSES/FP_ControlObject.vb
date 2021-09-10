Public Class FP_ControlObject
    Public FPf As FP_Form
    Public FP As FP
    Private Disposed As Boolean = False

    Protected Friend ParametersFrom_SubPrefix As String = ""

    Public Sub Dispose()
        Disposed = True
    End Sub

    Public ReadOnly Property P_FieldName() As String
        Get
            Dim OUT As String = ""

            If TypeOf (Me) Is FP_PictureBox Then
                OUT = CType(Me, FP_PictureBox).FieldName

            ElseIf TypeOf (Me) Is FP_Control Then
                OUT = CType(Me, FP_Control).FieldName

            Else
                FPf.FPApp.DoErrorMsgBox("", 0, "Invalid use of P_NAME property")
                OUT = ""
            End If

            Return OUT
        End Get
    End Property

    Protected Overrides Sub Finalize()
        Dispose()
        MyBase.Finalize()
    End Sub
End Class
