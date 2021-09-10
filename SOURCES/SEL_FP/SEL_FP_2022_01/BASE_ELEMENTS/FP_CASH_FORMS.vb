Public Class FP_CASH_FORMS
    Public Enum ENUM_CASH_FORMS_SEARCH_TYPE
        ONLY_ACTIVE_STATE = 0
        ONLY_CASHED_STATE = 1
        ALL = 99
    End Enum

    Public FPApp As FP_App

    Private DIC_Cash_Forms As New Dictionary(Of Integer, FP_Form)

    Sub New(MyFPApp As FP_App)
        FPApp = MyFPApp
    End Sub

    Public Function Get_FPf_from_Cash(Identifier As String, Search_Type As ENUM_CASH_FORMS_SEARCH_TYPE, ByRef OUT_FPf As FP_Form) As Boolean
        Dim OUT As Boolean = False

        OUT_FPf = Nothing

        For Each AktKey As String In DIC_Cash_Forms.Keys
            Dim CurrentIdentifier As String = DIC_Cash_Forms(AktKey).P_Cash_Identifier

            If CurrentIdentifier = Identifier Then
                Dim Tmp_FPf As FP_Form = DIC_Cash_Forms(AktKey)

                Select Case Search_Type
                    Case ENUM_CASH_FORMS_SEARCH_TYPE.ALL
                        'Nothing to do

                    Case ENUM_CASH_FORMS_SEARCH_TYPE.ONLY_CASHED_STATE
                        If Tmp_FPf.P_Cash_State = FP_Form.ENUM_Cash_State.CASHED Then
                            Tmp_FPf = DIC_Cash_Forms(AktKey)
                        End If

                    Case ENUM_CASH_FORMS_SEARCH_TYPE.ONLY_ACTIVE_STATE
                        If Tmp_FPf.P_Cash_State = FP_Form.ENUM_Cash_State.ACTIVE Then
                            Tmp_FPf = DIC_Cash_Forms(AktKey)
                        End If

                    Case Else
                        FPApp.DoErrorMsgBox("FP_CASH_FORMS.Get_FPf_from_Cash", 0, String.Format("Unknown Search Type ({0})", Search_Type))
                End Select

                If Not (Tmp_FPf Is Nothing) Then
                    If Not (Tmp_FPf.Frm Is Nothing) Then
                        If Tmp_FPf.Frm.IsDisposed = False Then
                            OUT = True
                            OUT_FPf = Tmp_FPf
                            Exit For
                        End If
                    End If
                End If
            End If
        Next

        Return OUT
    End Function

    Public Function IsCashed(FPf As FP_Form) As Boolean
        Dim OUT As Boolean = False

        If Not (FPf Is Nothing) Then
            If Not FPf.Frm Is Nothing Then
                If Not FPf.Frm.IsDisposed Then
                    OUT = FPApp.Cashed_Forms.DIC_Cash_Forms.ContainsKey(FPf.Frm_Handle)
                End If
            End If
        End If

        Return OUT
    End Function

    Public Function Get_FPf_from_Cash(ServerObject_Prefix As String, SubPrefix As String, Search_Type As ENUM_CASH_FORMS_SEARCH_TYPE, ByRef OUT_FPf As FP_Form) As Boolean
        Dim OUT As Boolean = False
        Dim Identifier As String = Identifier_GET(ServerObject_Prefix, SubPrefix)

        OUT = Get_FPf_from_Cash(Identifier, Search_Type, OUT_FPf)

        Return OUT
    End Function

    Public Sub Add_to_Cash(FPf As FP_Form)
        If DIC_Cash_Forms.ContainsKey(FPf.Frm_Handle) Then
            DIC_Cash_Forms(FPf.Frm_Handle) = FPf
        Else
            DIC_Cash_Forms.Add(FPf.Frm_Handle, FPf)
        End If
    End Sub

    Public Sub Add_to_CashAs(AsIdentifier As String, FPf As FP_Form)
        FPf.P_Cash_Identifier = AsIdentifier

        If DIC_Cash_Forms.ContainsKey(FPf.Frm_Handle) Then
            DIC_Cash_Forms(FPf.Frm_Handle) = FPf
        Else
            DIC_Cash_Forms.Add(FPf.Frm_Handle, FPf)
        End If
    End Sub

    Private Function Identifier_GET(ServerObject_Prefix As String, SubPrefix As String) As String
        Return ServerObject_Prefix + "|" + SubPrefix
    End Function

    Public Function ReOpen_FPf_from_Cash(ServerObject_Prefix As String, SubPrefix As String, ByRef OUT_FPf As FP_Form) As Boolean
        Dim OUT As Boolean = False

        OUT_FPf = Nothing

        If Get_FPf_from_Cash(ServerObject_Prefix, SubPrefix, ENUM_CASH_FORMS_SEARCH_TYPE.ONLY_CASHED_STATE, OUT_FPf) Then
            Dim Cancel As Boolean = False

            OUT_FPf.RAISEEVENT_FORM_CASH_BEFORE_REOPEN(Cancel)

            If Cancel = False Then
                OUT_FPf.RAISEEVENT_FORM_CASH_REOPEN()
                OUT = True
            End If
        End If

        Return OUT
    End Function

    Public Function ReOpen_FPf_from_Cash(Identifier As String, ByRef OUT_FPf As FP_Form) As Boolean
        Dim OUT As Boolean = False

        OUT_FPf = Nothing

        If Get_FPf_from_Cash(Identifier, ENUM_CASH_FORMS_SEARCH_TYPE.ONLY_CASHED_STATE, OUT_FPf) Then
            Dim Cancel As Boolean = False

            OUT_FPf.RAISEEVENT_FORM_CASH_BEFORE_REOPEN(Cancel)

            If Cancel = False Then
                OUT_FPf.RAISEEVENT_FORM_CASH_REOPEN()
                OUT = True
            End If
        End If

        Return OUT
    End Function

    'Public Function ReOpen_FPf_from_Cash_As_Dialog(ServerObject_Prefix As String, SubPrefix As String, Parent_FPf As FP_Form, OUT_DialogResult As System.Windows.Forms.DialogResult) As Boolean
    '    Dim OUT As Boolean = False
    '    Dim FPf As FP_Form = Nothing

    '    OUT_DialogResult = DialogResult.None

    '    If Get_FPf_from_Cash(ServerObject_Prefix, SubPrefix, ENUM_CASH_FORMS_SEARCH_TYPE.ONLY_ACTIVE_STATE, FPf) Then
    '        Dim RootFrm_setted As Boolean = False
    '        Parent_FPf.P_Enabled_Form_Child = FPf.Frm

    '        If FPApp.ShowDialogForm_RootFrm Is Nothing Then
    '            FPApp.ShowDialogForm_RootFrm = Parent_FPf.Frm
    '            RootFrm_setted = True
    '        End If

    '        Dim Disabled_Forms_LST As New List(Of Long)

    '        For Each Key As Integer In FPApp.Forms.Keys
    '            If Key <> FPf.Frm_Handle Then
    '                If FPApp.Forms(Key).P_ENABLED Then
    '                    Disabled_Forms_LST.Add(Key)
    '                    FPApp.Forms(Key).P_ENABLED = False
    '                End If
    '            End If
    '        Next

    '        OUT = True
    '        FPf.Frm.DialogResult = DialogResult.None
    '        FPf.RAISEEVENT_FORM_CASH_REOPEN()
    '        OUT_DialogResult = FPf.Frm.ShowDialog()

    '        If RootFrm_setted Then
    '            FPApp.ShowDialogForm_RootFrm = Nothing
    '            RootFrm_setted = False
    '        End If

    '        Parent_FPf.P_Enabled_Form_Child = Nothing

    '        For Each Key As String In Disabled_Forms_LST
    '            If FPApp.Forms.ContainsKey(Key) Then
    '                If FPApp.Forms(Key).P_ENABLED = False Then
    '                    FPApp.Forms(Key).P_ENABLED = True
    '                End If
    '            End If
    '        Next

    '        Parent_FPf.Frm.Activate()
    '    End If

    '    Return OUT
    'End Function
End Class
