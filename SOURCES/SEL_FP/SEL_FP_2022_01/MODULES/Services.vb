Imports System.Data
Imports System.Data.SqlClient

Public Module Services
    Function Qdf_Print_SP(ByVal SqlComm As SqlCommand) As String
        'A Qdf osszes parameteret ugy tekinti, mint egy tarolt eljaras mezoit es egy olyan SQL parancsot ir, ami meghivja ezt a tarolt eljarast
        'a Qdf parametereinek ertekevel.

        Dim OUT As String = ""
        Dim MyVesszo As String = ""
        Dim MyWert As String = ""
        Dim MyWerts As String = ""
        Dim ParamLNr As Integer = 0
        Dim SpName As String = ""
        Dim w As Long = 0
        Dim w1 As Long = 0
        Dim MyDeclarations As String = ""
        Dim MyOutput As String = ""
        Dim MyType As String = ""

        MyVesszo = ""

        SpName = SqlComm.CommandText

        For ParamLNr = SqlComm.Parameters.Count - 1 To 0 Step -1
            With SqlComm.Parameters.Item(ParamLNr)

                If .ParameterName <> "@RetValue" Then
                    Select Case .SqlDbType
                        Case SqlDbType.SmallInt
                            MyType = "smallint"
                            If IsNull(.Value) Then
                                MyWert = "Null"
                            Else
                                MyWert = .Value
                            End If

                        Case SqlDbType.Int
                            MyType = "int"
                            If IsNull(.Value) Then
                                MyWert = "Null"
                            Else
                                MyWert = .Value
                            End If

                        Case SqlDbType.Bit
                            MyType = "bit"
                            If IsNull(.Value) Then
                                MyWert = "Null"
                            Else
                                MyWert = IIf(.Value <> 0, "1", "0")
                            End If

                        Case SqlDbType.Float
                            MyType = "float"
                            If IsNull(.Value) Then
                                MyWert = "Null"
                            Else
                                MyWert = KommaToPunkt(Trim(Str(.Value)))
                            End If

                        Case SqlDbType.DateTime
                            MyType = "datetime"
                            If IsDBNull(.Value) Then
                                MyWert = "Null"
                            Else
                                If .Value = NULLDATE Then
                                    MyWert = "Null"
                                Else
                                    Dim MyDate As DateTime = CDate(.Value)
                                    If MyDate.Hour = 0 And MyDate.Minute = 0 And MyDate.Second = 0 Then
                                        MyWert = String.Format("'{0}'", Format(CDate(.Value), "yyyy-MM-dd"))
                                    Else
                                        MyWert = String.Format("'{0}'", Format(CDate(.Value), "yyyy-MM-dd HH:mm:ss"))
                                    End If
                                End If
                            End If

                        Case SqlDbType.VarChar
                            MyType = String.Format("varchar({0})", .Size.ToString)
                            If IsNull(.Value) Then
                                MyWert = "Null"
                            Else
                                MyWert = String.Format("'{0}'", Replace(.Value.ToString, "'", "''"))
                            End If

                        Case SqlDbType.NVarChar
                            If .Size = -1 Then
                                MyType = "nvarchar(max)"
                            Else
                                MyType = String.Format("nvarchar({0})", .Size)
                            End If
                            If IsNull(.Value) Then
                                MyWert = "Null"
                            Else
                                MyWert = String.Format("'{0}'", Replace(.Value.ToString, "'", "''"))
                            End If

                        Case SqlDbType.VarBinary
                            If .Size = -1 Then
                                MyType = "varbinary(max)"
                            Else
                                MyType = String.Format("varbinary({0})", .Size)
                            End If
                        Case Else
                            MsgBox(String.Format("Sel_Services.Qdf_Print_SP: Type of field '{0}' unknown.", .ParameterName))
                            Qdf_Print_SP = ""
                            Exit Function
                    End Select

                    If .Direction = ParameterDirection.InputOutput Or .Direction = ParameterDirection.Output Then
                        MyOutput = String.Format("{0} {1}{2}", .ParameterName, Replace(.ParameterName, "@", ""), IIf(MyOutput > "", ", ", "")) + MyOutput
                        MyDeclarations = String.Format("DECLARE {0}{1}{2}", .ParameterName, vbTab + vbTab, MyType) + vbCrLf + _
                                          String.Format("SELECT {0} = {1}", .ParameterName, MyWert) + vbCrLf + MyDeclarations
                        MyWerts = vbTab + .ParameterName + vbTab + "= " + .ParameterName + vbTab + vbTab + "OUTPUT" + MyVesszo + vbCrLf + vbTab + vbTab + vbTab + vbTab + MyWerts
                    Else
                        MyWerts = vbTab + .ParameterName + vbTab + "= " + MyWert + MyVesszo + vbCrLf + vbTab + vbTab + vbTab + vbTab + vbTab + vbTab + vbTab + MyWerts
                    End If

                    MyVesszo = ","
                End If
            End With
        Next ParamLNr

        OUT = MyDeclarations + vbCrLf + _
              "declare @w int" + vbCrLf + vbCrLf + _
              "exec @w = " + SpName + vbTab + MyWerts + vbCrLf + _
              "SELECT @w EREDMENY" + vbCrLf
        If MyOutput > "" Then
            OUT += "SELECT " + MyOutput
        End If

        Qdf_Print_SP = OUT
    End Function
End Module
