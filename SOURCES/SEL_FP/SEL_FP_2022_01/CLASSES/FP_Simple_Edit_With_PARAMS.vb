Public Class FP_Simple_EDIT_With_PARAMS
    '-------------------------------------------------------------------------------------------
    'Simple_Edit, amelyben a mezok tulajdonsagat megnyitas elott valtoztatni kell.
    '-------------------------------------------------------------------------------------------
    '
    'Pelda a hasznalatra:
    '
    'Dim Simple_Edit_with_params_P As New FP_Simple_Edit_With_PARAMS.Struct_Simple_Edit_With_PARAMS
    '
    'With Simple_Edit_with_params_P
    '  .Parent_FPf = FPf
    '  .Identifier = Simple_Edit_SubPrefix
    'End With
    '
    'Dim Simple_Edit_with_Params As New FP_Simple_Edit_With_PARAMS(Simple_Edit_with_params_P)
    '
    'Simple_Edit_with_Params.FIELD_PARAMS_ADD("Cust_Name", ENUM_Field_Params.DT_WHERE2, "x=3")
    '
    'Simple_Edit_with_Params.ShowDialog()
    '
    ' DoIt = gl_Doit
    '
    'If DoIt Then
    '   (...)
    ' End If
    '-------------------------------------------------------------------------------------------

    Public Structure Struct_Simple_Edit_With_PARAMS
        Dim Parent_FPf As FP_Form
        Dim Identifier As String
    End Structure

    Public Structure Struct_Field_Params
        Dim FieldName As String
        Dim Param As ENUM_Field_Params
        Dim Value As String
    End Structure

    Public Parent_FPf As FP_Form = Nothing
    Public WithEvents Simple_Edit As FP_Simple_Edit = Nothing
    Public WithEvents FP_of_Simple_Edit As FP = Nothing
    Public Field_Params(0) As Struct_Field_Params
    Private Disposed As Boolean = False

    Public Sub New(P As Struct_Simple_Edit_With_PARAMS)
        With P
            Parent_FPf = .Parent_FPf

            Simple_Edit = New FP_Simple_Edit(gl_FPApp, P.Identifier)
            FP_of_Simple_Edit = Simple_Edit.FP_SIMPLEEDIT
        End With
    End Sub

    Public Sub Dispose()
        If Disposed = False Then
            Parent_FPf = Nothing
            FP_of_Simple_Edit = Nothing
            Simple_Edit.Dispose()

            Disposed = True
        End If
    End Sub

    Private Sub FP_Simple_Edit_CONTROLS_INITIALIZED(sender_FP As FP) Handles FP_of_Simple_Edit.CONTROLS_INITIALIZED
        For i As Integer = 1 To UBound(Field_Params)
            If Simple_Edit.FP_SIMPLEEDIT.CONTROLS.ContainsKey(Field_Params(i).FieldName) = False Then
                gl_FPApp.DoErrorMsgBox(Parent_FPf, "FP_Simple_Edit_With_PARAMS.FP_DoFilter_CONTROLS_INITIALIZED", 0, String.Format("CONTROL '{0} does not exists'", Field_Params(i).FieldName))
            Else
                With Simple_Edit.FP_SIMPLEEDIT.CONTROLS(Field_Params(i).FieldName)
                    Select Case Field_Params(i).Param
                        Case ENUM_Field_Params.VISIBLE
                            .P_VISIBLE = DBFORMAT_TO_Object(Field_Params(i).Value, "BIT")

                        Case ENUM_Field_Params.MANDATORY
                            .P.Mandatory = DBFORMAT_TO_Object(Field_Params(i).Value, "BIT")

                        Case ENUM_Field_Params.LOCKED
                            .P.Locked = DBFORMAT_TO_Object(Field_Params(i).Value, "BIT")

                        Case ENUM_Field_Params.XTYPE_VB
                            .P.xType_VB = Field_Params(i).Value

                        Case ENUM_Field_Params.F_FORMAT_NOSPACE
                            .F_Format_NOSPACE = DBFORMAT_TO_Object(Field_Params(i).Value, "BIT")

                        Case ENUM_Field_Params.F_FORMAT_TRIM
                            .F_Format_TRIM = DBFORMAT_TO_Object(Field_Params(i).Value, "BIT")

                        Case ENUM_Field_Params.F_FORMAT_UCASE
                            .F_Format_UCASE = DBFORMAT_TO_Object(Field_Params(i).Value, "BIT")

                        Case ENUM_Field_Params.F_FORMAT_NoShow0
                            .F_Format_NoShow0 = DBFORMAT_TO_Object(Field_Params(i).Value, "BIT")

                        Case ENUM_Field_Params.F_FORMAT_MinusAllowed
                            .F_Format_NoShow0 = DBFORMAT_TO_Object(Field_Params(i).Value, "BIT")

                        Case ENUM_Field_Params.F_FORMAT_LABEL_ALIGN
                            .F_Format_LABEL_ALIGN = DBFORMAT_TO_Object(Field_Params(i).Value, "BIT")

                        Case ENUM_Field_Params.F_FORMAT_FORMAT
                            .F_Format_FORMAT = Field_Params(i).Value

                        Case ENUM_Field_Params.F_FORMAT_ALIGN
                            .F_Format_ALIGN = DBFORMAT_TO_Object(Field_Params(i).Value, "BIT")

                        Case ENUM_Field_Params.COLOR_LABEL_BG
                            COLOR_GET_FROM_STR(Field_Params(i).Value, COLORS_LABEL_BG, Field_Params(i).FieldName, .P.COLOR_LABEL_BG)

                        Case ENUM_Field_Params.COLOR_LABEL_FORE
                            COLOR_GET_FROM_STR(Field_Params(i).Value, COLORS_LABEL_FORE, Field_Params(i).FieldName, .P.COLOR_LABEL_FORE)

                        Case ENUM_Field_Params.COLOR_NORMAL_BG
                            COLOR_GET_FROM_STR(Field_Params(i).Value, COLORS_FIELD_NORMAL_BG, Field_Params(i).FieldName, .P.COLOR_NORMAL_BG)

                        Case ENUM_Field_Params.COLOR_NORMAL_FORE
                            COLOR_GET_FROM_STR(Field_Params(i).Value, COLORS_FIELD_NORMAL_FORE, Field_Params(i).FieldName, .P.COLOR_NORMAL_FORE)

                        Case ENUM_Field_Params.COLOR_SELECTED_FORE
                            COLOR_GET_FROM_STR(Field_Params(i).Value, COLORS_FIELD_SELECTED_FORE, Field_Params(i).FieldName, .P.COLOR_SELECTED_FORE)

                        Case ENUM_Field_Params.BG_IMAGE
                            .P_BACKGROUNDIMAGE = Field_Params(i).Value

                        Case ENUM_Field_Params.TAG
                            .P.Tag = Field_Params(i).Value

                        Case ENUM_Field_Params.TABSTOP
                            .P_TABSTOP = DBFORMAT_TO_Object(Field_Params(i).Value, "BIT")

                        Case ENUM_Field_Params.DT_FIXTEXT_KEY
                            .P.DT_FixText_Key = Field_Params(i).Value

                        Case ENUM_Field_Params.DT_WHERE2
                            .P.DT_WHERE2 = Field_Params(i).Value

                        Case ENUM_Field_Params.BASE_VALUE
                            .P_VALUE = DBFORMAT_TO_Object(Field_Params(i).Value, .P.xType_VB)

                        Case Else
                            gl_FPApp.DoErrorMsgBox(Parent_FPf, "DoFilter_With_PARAMS.FP_DoFilter_CONTROLS_INITIALIZED", 0, String.Format("Unknown param {0}", CInt(Field_Params(i).Param)))
                    End Select

                    .COLORING()
                End With
            End If
        Next
    End Sub

    Public Function ShowDialog() As DialogResult
        Return gl_FPApp.ShowDialogForm(Simple_Edit, Parent_FPf)
    End Function

    Public Sub FIELD_PARAMS_ADD(FieldName As String, Param As ENUM_Field_Params, Value As Integer)
        ReDim Preserve Field_Params(UBound(Field_Params) + 1)

        With Field_Params(UBound(Field_Params))
            .FieldName = FieldName
            .Param = Param
            .Value = Value
        End With
    End Sub

    Public Sub FIELD_PARAMS_ADD(FieldName As String, Param As ENUM_Field_Params, Value As Double)
        ReDim Preserve Field_Params(UBound(Field_Params) + 1)

        With Field_Params(UBound(Field_Params))
            .FieldName = FieldName
            .Param = Param
            .Value = DBFORMAT_from_OBJECT(Value, FieldName, "FLOAT")
        End With
    End Sub

    Public Sub FIELD_PARAMS_ADD(FieldName As String, Param As ENUM_Field_Params, Value As String)
        ReDim Preserve Field_Params(UBound(Field_Params) + 1)

        With Field_Params(UBound(Field_Params))
            .FieldName = FieldName
            .Param = Param
            .Value = Value
        End With
    End Sub

    Public Sub FIELD_PARAMS_ADD(FieldName As String, Param As ENUM_Field_Params, Value As DateTime)
        ReDim Preserve Field_Params(UBound(Field_Params) + 1)

        With Field_Params(UBound(Field_Params))
            .FieldName = FieldName
            .Param = Param
            .Value = DBFORMAT_from_OBJECT(Value, FieldName, "DATETIME")
        End With
    End Sub

    Public Sub FIELD_PARAMS_ADD(FieldName As String, Param As ENUM_Field_Params, Value As Boolean)
        ReDim Preserve Field_Params(UBound(Field_Params) + 1)

        With Field_Params(UBound(Field_Params))
            .FieldName = FieldName
            .Param = Param
            .Value = DBFORMAT_from_OBJECT(Value, FieldName, "BIT")
        End With
    End Sub
End Class
