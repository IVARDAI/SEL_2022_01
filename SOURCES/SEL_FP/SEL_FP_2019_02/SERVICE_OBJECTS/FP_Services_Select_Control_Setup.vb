Imports System.ComponentModel
Imports Newtonsoft.Json.Linq
Public Class FP_Services_Select_Control_Setup

    Dim MyFixtextKey As String
    Dim MyFixText As String
    Private DT_Fields As DataTable
    Private DT_Lang As DataTable
    Private Is_Canceled As Boolean = False
    Dim JO As JObject
    Private OUT_JSON As String = ""
    ReadOnly TABS(10) As String

    Public Sub New(Fixtext_Key As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MyFixtextKey = Fixtext_Key
    End Sub

    Public ReadOnly Property FixText As String
        Get
            Dim OUT As String = OUT_JSON
            Return OUT
        End Get
    End Property
    Private Sub FP_Services_Select_Control_Setup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Prepare_Form()
        Load_Form_Data()
    End Sub

    Private Sub Load_DT_Fields(Dic_Fields As Dictionary(Of String, Stru_Select_Control_Field_Prop))
        Dim S As Stru_Select_Control_Field_Prop
        For Each S In Dic_Fields.Values
            Dim D As DataRow
            D = DT_Fields.NewRow
            D.Item("FieldName") = S.FieldName
            D.Item("FieldLen") = S.FieldLen
            D.Item("Visible") = S.Visible
            Load_DT_Lang(S.HeaderTexts)
            DT_Fields.Rows.Add(D)
        Next
    End Sub
    Private Sub Load_DT_Lang(Dic_Headers As Dictionary(Of String, Stru_Select_Control_Field_Headers))
        Dim H As Stru_Select_Control_Field_Headers
        For Each H In Dic_Headers.Values
            Dim D As DataRow
            D = DT_Lang.NewRow
            D.Item("FieldName") = H.Fieldname
            D.Item("Lang") = H.Language
            D.Item("HeaderText") = H.HeaderText
            DT_Lang.Rows.Add(D)
        Next
    End Sub

    Private Sub Load_Form_Data()
        Me.FixText_Identifier.Text = MyFixtextKey
        MyFixText = gl_FPApp.getFixText(MyFixtextKey)
        Dim JS As JObject = Get_Json_From_FixText(FixText)

        Dim DIC_Params As New Dictionary(Of String, String)
        Dim Dic_Fields As New Dictionary(Of String, Stru_Select_Control_Field_Prop)
        If JSon_Select_Control_Load_Params(MyFixText, DIC_Params, Dic_Fields) Then
            Me.SQL_From1.Text = DIC_Params("SQL_From1")
            Me.SQL_From2.Text = DIC_Params("SQL_From2")
            Me.IdFieldName.Text = DIC_Params("IdFieldName")
            Me.Field_Text1.Text = DIC_Params("Field_Text1")
            Me.Field_Text2.Text = DIC_Params("Field_Text2")
            Me.OrderBy1.Text = DIC_Params("OrderBy1")
            Me.OrderBy2.Text = DIC_Params("OrderBy2")
            Me.No_Limit_To_List.Checked = DIC_Params("No_Limit_To_List") = 1
            Me.MaxLength.Text = DIC_Params("MaxLength")
            Me.PanelHeight.Text = DIC_Params("PanelHeight")
            Load_DT_Fields(Dic_Fields)
        Else
            Me.SQL_From1.Text = ""
            Me.SQL_From2.Text = ""
            Me.IdFieldName.Text = ""
            Me.Field_Text1.Text = ""
            Me.Field_Text2.Text = ""
            Me.OrderBy1.Text = ""
            Me.OrderBy2.Text = ""
            Me.No_Limit_To_List.Checked = False
            Me.MaxLength.Text = ""
            Me.PanelHeight.Text = ""
        End If
    End Sub
    Private Function Get_Json_From_FixText(Control_Definition As String) As JObject
        Dim OUT As JObject
        Try
            OUT = JObject.Parse(Control_Definition)
        Catch ex As Exception
            OUT = Nothing
        End Try
        Return OUT
    End Function
    Private Function Check_Fields() As Boolean
        Dim OUT As Boolean = True

        If Me.SQL_From1.Text = String.Empty Then
            OUT = False
            Me.SQL_From1.Focus()
        End If

        If OUT Then
            If Me.SQL_From2.Text = String.Empty Then Me.SQL_From2.Text = Me.SQL_From1.Text
        End If

        If OUT Then
            If Me.IdFieldName.Text = String.Empty Then
                OUT = False
                Me.IdFieldName.Focus()
            End If
        End If

        If OUT Then
            If Me.Field_Text1.Text = String.Empty Then
                OUT = False
                Me.Field_Text1.Focus()
            End If
        End If

        If OUT Then
            If Me.Field_Text2.Text = String.Empty Then Me.Field_Text2.Text = Me.Field_Text1.Text
        End If

        If OUT Then
            If Me.OrderBy1.Text = String.Empty Then
                OUT = False
                Me.OrderBy1.Focus()
            End If
        End If

        If OUT Then
            If Me.OrderBy2.Text = String.Empty Then Me.OrderBy2.Text = Me.OrderBy1.Text
        End If

        If OUT Then
            If Me.MaxLength.Text = String.Empty Then
                MaxLength.Text = 100
            End If
            If Not IsNumeric(MaxLength.Text) Then
                MaxLength.Focus()
                OUT = False
            End If
        End If

        If OUT Then
            If Me.PanelHeight.Text = String.Empty Then
                PanelHeight.Text = 100
            End If
            If Not IsNumeric(PanelHeight.Text) Then
                PanelHeight.Focus()
                OUT = False
            End If
        End If

        Return OUT
    End Function

    Private Sub Button_OK_Click(sender As Object, e As EventArgs) Handles Button_OK.Click
        Is_Canceled = False
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Cancel.Click
        Is_Canceled = True
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub JSON_Add_Line(OneLine As String)
        Dim AP As String = """"
        OneLine = OneLine.Replace("''", AP)
        OUT_JSON = String.Format("{0}{1}{2}", OUT_JSON, vbCrLf, OneLine)
    End Sub
    Private Function Json_Lang_Items(FieldName As String) As String
        Dim OUT As String = TABS(5) & "''items'':["
        Dim Crit As String = String.Format("FieldName = '{0}'", FieldName)
        Dim DRs() As DataRow
        Dim I As Integer
        DRs = DT_Lang.Select(Crit)

        For I = 0 To DRs.Length - 1
            Dim Lang As String = DRs(I).Item("Lang")
            Dim HeaderText As String = DRs(I).Item("HeaderText")
            Dim OneLine As String = TABS(6) & "{" & vbCrLf
            OneLine = String.Format("{0}{2}''Lang'':''{1}'',", OneLine, Lang, TABS(7))
            OUT = String.Format("{0}{1}{2}", OUT, vbCrLf, OneLine)

            OneLine = String.Format("{1}''HeaderText'':''{0}'',", HeaderText, TABS(7))
            OUT = String.Format("{0}{1}{2}", OUT, vbCrLf, OneLine)

            OneLine = String.Format("{1}''FieldName'':''{0}''", FieldName, TABS(7))
            OUT = String.Format("{0}{1}{2}", OUT, vbCrLf, OneLine)
            OUT = OUT + vbCrLf + TABS(6) + "},"
        Next
        OUT &= vbCrLf & TABS(5) & "]"
        Return OUT
    End Function

    Private Function Json_Field_Item(DR As DataRow) As String
        Dim OUT As String = "{"
        Dim FieldName As String = DR.Item("FieldName")
        Dim FieldLen As String = DR.Item("FieldLen")
        Dim Visible As String = DR.Item("Visible")
        Dim OneLine As String

        OneLine = String.Format("{1}''FieldName'':''{0}'',", FieldName, TABS(4))
        OUT = String.Format("{0}{1}{2}", OUT, vbCrLf, OneLine)

        OneLine = String.Format("{1}''FieldLen'':''{0}'',", FieldLen, TABS(4))
        OUT = String.Format("{0}{1}{2}", OUT, vbCrLf, OneLine)

        OUT &= vbCrLf & TABS(4) & "''Headers'' : {"

        OneLine = Json_Lang_Items(FieldName)
        OUT = String.Format("{0}{1}{2}", OUT, vbCrLf, OneLine)

        OUT &= vbCrLf & TABS(4) & "},"

        OneLine = String.Format("{1}''Visible'':''{0}''", Visible, TABS(4))
        OUT = String.Format("{0}{1}{2}", OUT, vbCrLf, OneLine)

        OUT = OUT + vbCrLf + TABS(3) & "}," & vbCrLf & TABS(2)

        Return OUT
    End Function

    Private Function Save_JSON() As Boolean
        JO = New JObject
        OUT_JSON = "{"
        Dim OUT As Boolean = True
        Dim DR1 As DataRow
        Dim Items As String = ""
        Dim OneLine As String
        OneLine = String.Format("{1}''SQL_From1'':''{0}'',", Me.SQL_From1.Text, TABS(1)) : JSON_Add_Line(OneLine)
        OneLine = String.Format("{1}''SQL_From2'':''{0}'',", Me.SQL_From2.Text, TABS(1)) : JSON_Add_Line(OneLine)
        OneLine = String.Format("{1}''IdFieldName'':''{0}'',", Me.IdFieldName.Text, TABS(1)) : JSON_Add_Line(OneLine)
        OneLine = String.Format("{1}''Field_Text1'':''{0}'',", Me.Field_Text1.Text, TABS(1)) : JSON_Add_Line(OneLine)
        OneLine = String.Format("{1}''Field_Text2'':''{0}'',", Me.Field_Text2.Text, TABS(1)) : JSON_Add_Line(OneLine)
        OneLine = String.Format("{1}''OrderBy1'':''{0}'',", Me.OrderBy1.Text, TABS(1)) : JSON_Add_Line(OneLine)
        OneLine = String.Format("{1}''OrderBy2'':''{0}'',", Me.OrderBy2.Text, TABS(1)) : JSON_Add_Line(OneLine)

        Dim NL As String = "0"
        If Me.No_Limit_To_List.CheckState = CheckState.Checked Then
            NL = "1"
        End If
        OneLine = String.Format("{1}''No_Limit_To_List'':''{0}'',", NL, TABS(1)) : JSON_Add_Line(OneLine)
        OneLine = String.Format("{1}''MaxLength'':''{0}'',", Me.MaxLength.Text, TABS(1)) : JSON_Add_Line(OneLine)
        OneLine = String.Format("{1}''PanelHeight'':''{0}'',", Me.PanelHeight.Text, TABS(1)) : JSON_Add_Line(OneLine)
        OneLine = TABS(1) & "''Field_Def'': {" & vbCrLf & TABS(2) & "''items'': [" & vbCrLf & TABS(3) & "##FIELD_ITEMS##]" & vbCrLf & TABS(1) & "}" : JSON_Add_Line(OneLine)

        For Each DR1 In DT_Fields.Rows
            Items &= Json_Field_Item(DR1)
        Next
        Items = Items.Replace("''", """")
        OUT_JSON = OUT_JSON.Replace("##FIELD_ITEMS##", Items)
        OUT_JSON &= vbCrLf & "}"

        Dim mypath As String = "E:\Telepites\TESZT_JSON2.txt"
        System.IO.File.WriteAllText(mypath, OUT_JSON)
        Return OUT
    End Function
    Private Sub Prepare_Form()

        DT_Fields = New DataTable("TB1")
        With DT_Fields
            .Columns.Add("FieldName", Type.GetType("System.String"))
            .Columns.Add("FieldLen", Type.GetType("System.Int16"))
            .Columns.Add("Visible", Type.GetType("System.Boolean"))
        End With

        Dim DT1_PK(1) As System.Data.DataColumn
        DT1_PK(0) = DT_Fields.Columns(0)
        DT_Fields.PrimaryKey = DT1_PK
        DGV_Field_Def.DataSource = DT_Fields
        DGV_Field_Def.AllowUserToDeleteRows = False

        DT_Lang = New DataTable("TB2")
        With DT_Lang
            .Columns.Add("Lang", Type.GetType("System.String"))
            .Columns.Add("HeaderText", Type.GetType("System.String"))
            .Columns.Add("FieldName", Type.GetType("System.String"))
        End With
        DGV_Lang.DataSource = DT_Lang
        DGV_Lang.AllowUserToDeleteRows = False

        With DGV_Field_Def
            .Columns("FieldName").HeaderText = "Field name"
            .Columns("FieldName").Width = 150
            .Columns("FieldLen").HeaderText = "Column length"
            .Columns("Visible").HeaderText = "Visible"
        End With

        With DGV_Lang
            .Columns("Lang").HeaderText = "Language"
            .Columns("HeaderText").HeaderText = "Header text"
            .Columns("FieldName").HeaderText = "FieldName"
            .Columns("FieldName").Visible = False
        End With

        Dim I As Integer
        Dim TB As String = ""

        TABS(0) = TB
        For I = 1 To 9
            TB &= vbTab
            TABS(I) = TB
        Next

    End Sub

#Region "Data handling"
    Private Function Get_Field_Name(RowIndex As Integer) As String
        Dim OUT As String
        If RowIndex >= 0 Then
            Dim CR As DataGridViewRow = DGV_Field_Def.Rows(RowIndex)
            If Not CR Is Nothing Then
                Dim RowIdx As Integer = CR.Index
                If Not CR Is Nothing Then
                    Dim CC As DataGridViewCell = DGV_Field_Def(0, RowIdx)
                    If Not IsDBNull(CC.Value) Then
                        OUT = CC.Value
                    Else
                        OUT = ""
                    End If
                Else
                    OUT = ""
                End If
            Else
                OUT = ""
            End If
        Else
            OUT = ""
        End If
        Return OUT
    End Function
    Private Sub DGV_Field_Def_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DGV_Field_Def.RowEnter
        Load_Lang(e.RowIndex)
    End Sub
    Private Sub Delete_Lang_By_FieldName(FieldName As String)
        Dim Crit As String = String.Format("FieldName = '{0}'", FieldName)
        Dim DRS() As DataRow = DT_Lang.Select(Crit)
        While DRS.Length > 0
            DT_Lang.Rows.Remove(DRS(0))
            DRS = DT_Lang.Select(Crit)
        End While
    End Sub
    Private Sub Delete_Lang()
        Dim RowIndex As Integer = DGV_Lang.CurrentRow.Index
        Dim Head_Row_Index As Integer = DGV_Field_Def.CurrentRow.Index
        Dim Lang As String
        Dim HeaderText As String
        Dim FieldName As String
        If Not DGV_Lang.Rows(RowIndex).IsNewRow Then
            If Not IsDBNull(DGV_Lang(0, RowIndex).Value) Then
                Lang = DGV_Lang(0, RowIndex).Value
            Else
                Lang = ""
            End If

            If Not IsDBNull(DGV_Lang(1, RowIndex).Value) Then
                HeaderText = DGV_Lang(1, RowIndex).Value
            Else
                HeaderText = ""
            End If

            If IsDBNull(DGV_Lang(2, RowIndex).Value) Then
                FieldName = Get_Field_Name(Head_Row_Index)
            Else
                FieldName = DGV_Lang(2, RowIndex).Value
            End If
            DGV_Lang.Rows.Remove(DGV_Lang.Rows(RowIndex))
            Dim Crit As String = String.Format("FieldName = '{0}' AND Lang = '{1}'", FieldName, Lang)
            Dim DRS() As DataRow = DT_Lang.Select(Crit)
            While DRS.Length > 0
                DT_Lang.Rows.Remove(DRS(0))
                DRS = DT_Lang.Select(Crit)
            End While
        End If
        DT_Lang.AcceptChanges()
        Load_Lang(Head_Row_Index)
    End Sub
    Private Sub Delete_Field()
        Dim RowIndex As Integer = DGV_Field_Def.CurrentRow.Index
        Dim FieldName As String = ""
        If Not DGV_Field_Def.Rows(RowIndex).IsNewRow Then

            If IsDBNull(DGV_Field_Def(0, RowIndex).Value) Then
                FieldName = ""
            Else
                FieldName = DGV_Field_Def(0, RowIndex).Value
            End If
            DGV_Field_Def.Rows.Remove(DGV_Field_Def.Rows(RowIndex))
            Dim Crit As String = String.Format("FieldName = '{0}'", FieldName)
            Dim DRS() As DataRow = DT_Fields.Select(Crit)
            While DRS.Length > 0
                DT_Fields.Rows.Remove(DRS(0))
                DRS = DT_Fields.Select(Crit)
            End While
        End If
        DGV_Field_Def.CurrentCell = DGV_Field_Def(0, 0)
        Delete_Lang_By_FieldName(FieldName)
        Load_Lang(0)
    End Sub
    Private Sub Save_Lang()
        Dim R As DataGridViewRow
        Dim Head_Row_Index As Integer
        If Not DGV_Field_Def.CurrentRow Is Nothing Then
            Head_Row_Index = DGV_Field_Def.CurrentRow.Index
        Else
            Head_Row_Index = -1
        End If

        Dim OK As Boolean = True
        For Each R In DGV_Lang.Rows
            If Not R.IsNewRow Then
                Dim Lang As String = ""
                Dim HeaderText As String = ""
                Dim FieldName As String

                Set_Joined_Field(Head_Row_Index, R.Index)

                If Not IsDBNull(DGV_Lang(0, R.Index).Value) Then
                    Lang = DGV_Lang(0, R.Index).Value
                Else
                    Lang = ""
                End If

                If Not IsDBNull(DGV_Lang(1, R.Index).Value) Then
                    HeaderText = DGV_Lang(1, R.Index).Value
                Else
                    HeaderText = ""
                End If

                If IsDBNull(DGV_Lang(2, R.Index).Value) Then
                    FieldName = Get_Field_Name(Head_Row_Index)
                Else
                    FieldName = DGV_Lang(2, R.Index).Value
                End If

                If Lang = String.Empty Then
                    OK = False
                End If

                If HeaderText = String.Empty Then
                    OK = False
                End If

                If FieldName = String.Empty Then
                    OK = False
                End If

                If OK Then
                    Dim drs() As DataRow
                    Dim SEL As String = String.Format("FieldName = '{0}' AND Lang = '{1}'", FieldName, Lang)
                    drs = DT_Lang.Select(SEL)
                    If drs.Length = 0 Then
                        Dim NewDR As DataRow = DT_Lang.NewRow
                        NewDR.Item("Lang") = Lang
                        NewDR.Item("HeaderText") = HeaderText
                        NewDR.Item("FieldName") = FieldName
                        DT_Lang.Rows.Add(NewDR)
                    Else
                        Dim MyDR As DataRow = drs(0)
                        MyDR.Item("HeaderText") = HeaderText
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub Load_Lang(RowIndex As Integer)
        Dim FieldName As String = Get_Field_Name(RowIndex)
        Dim SQL As String = String.Format("FieldName = '{0}'", FieldName)
        Dim drs() As DataRow = DT_Lang.Select(SQL)
        Dim D As DataTable = DT_Lang.Clone()
        Dim DR As DataRow
        If drs.Length = 0 Then
        Else
            For Each DR In drs
                Dim D2 As DataRow = D.NewRow
                D2.Item("Lang") = DR.Item("Lang")
                D2.Item("HeaderText") = DR.Item("HeaderText")
                D2.Item("FieldName") = DR.Item("FieldName")
                D.Rows.Add(D2)
            Next
        End If

        DGV_Lang.DataSource = D
        DGV_Lang.AllowUserToAddRows = True

    End Sub
    Private Sub Set_Joined_Field(HeadRowIndex As Integer, MyRowIndex As Integer)
        Dim FieldName As String = Get_Field_Name(HeadRowIndex)
        If FieldName = String.Empty Then
            'nothing to do
        Else
            DGV_Lang(2, MyRowIndex).Value = FieldName
        End If
    End Sub
    Private Sub DGV_Lang_RowValidated(sender As Object, e As DataGridViewCellEventArgs) Handles DGV_Lang.RowValidated
        Save_Lang()
    End Sub
    Private Sub DGV_Lang_KeyDown(sender As Object, e As KeyEventArgs) Handles DGV_Lang.KeyDown
        If e.KeyCode = Keys.D Then
            If e.Control Then
                Delete_Lang()
            End If
        End If
    End Sub
    Private Sub DGV_Field_Def_KeyDown(sender As Object, e As KeyEventArgs) Handles DGV_Field_Def.KeyDown
        If e.KeyCode = Keys.D Then
            If e.Control Then
                Delete_Field()
            End If
        End If
    End Sub

    Private Sub FP_Services_Select_Control_Setup_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Is_Canceled Then
            'Nothing to do
        Else
            If Not Check_Fields() Then
                e.Cancel = True
                MsgBox("nem!")
            Else
                If Not Save_JSON() Then
                    e.Cancel = True
                    MsgBox("Hiba!")
                End If
            End If
        End If
    End Sub

#End Region

End Class