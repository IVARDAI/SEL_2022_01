Imports Dynamsoft.TWAIN
Imports Dynamsoft.TWAIN.Interface
Imports System.Data.SqlClient

Public Class FP_Scan

#Region "DECLARE"

    Implements IAcquireCallback

    Private _FPf As FP_Form

    Private m_StrProductKey As String = "f0068NQAAAC6WIiKdLzvnEO+bSGGZ5zYh26DR2tUGp+ckPwb++ODsi9SMhww2CxVnJI1tMd4Dk0cU3kD5xbu7ZUUbB/lPY3U="
    Private m_TwainManager As TwainManager = Nothing

    Private OUT_BitmapList As New List(Of Bitmap)

    Private Disposed As Boolean = False

#End Region

#Region "CLASS CONSTRUCTOR"

    Public Sub New(ByVal FPf As FP_Form)
        _FPf = FPf

        m_TwainManager = New TwainManager(m_StrProductKey)
    End Sub

#End Region

#Region "CLASS DESTRUCTOR"
    Public Sub Dispose()
        If Not Disposed Then
            If Not (m_TwainManager Is Nothing) Then
                m_TwainManager.Dispose()

                m_TwainManager = Nothing
            End If

            Disposed = True
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()

        MyBase.Finalize()
    End Sub

#End Region

#Region "PUBLIC FUNCTIONS"

    Public Function ScanDocument(Optional ByVal PFDKey_for_Scanner As String = "SCANNER_LAST_SELECTED", Optional ByRef OUT_SelectedScanner As String = "") As Boolean
        Dim OUT As Boolean = False

        If Not Disposed Then
            Dim SelectedScanner As String = ""
            Dim tempList As New Dictionary(Of String, Integer)
            Dim tempListName As String
            Dim sqlComm As SqlCommand = gl_FPApp.DC.CNN.CreateCommand
            Dim P As New Struct_Simple_SELECT_Params
            Dim P_OUT As New Struct_Simple_SELECT_OutputParams

            OUT_SelectedScanner = ""

            gl_FPApp.DC.Qdf_RunSQL(String.Format("DELETE FROM Dispo1 WHERE Terminal='{0}' AND Art='FP_SCAN_SELECT'", Terminal))

            For i As Integer = 0 To m_TwainManager.SourceCount - 1
                tempListName = m_TwainManager.SourceNameItems(CShort(i))
                tempList.Add(tempListName, i)
                gl_FPApp.DC.Qdf_RunSQL(String.Format("INSERT INTO Dispo1 (Terminal, Art, LNr, Dispo1Text) SELECT '{0}', 'FP_SCAN_SELECT', {1}, '{2}'", Terminal, i, tempListName))
            Next

            If tempList.Count = 0 Then
                gl_FPApp.DoMyMsgBox(133) 'Nem találtam telepített szkennert
                ScanDocument = False
                Exit Function
            End If

            Dim CtrlPressed = My.Computer.Keyboard.CtrlKeyDown

            If CtrlPressed = False Then
                gl_FPApp.PFDlesen(PFDKey_for_Scanner, SelectedScanner)
                If Not tempList.ContainsKey(SelectedScanner) Then
                    SelectedScanner = ""
                End If
            End If

            If SelectedScanner = "" Then
                If tempList.Count = 1 Then
                    SelectedScanner = tempList.Keys.First
                Else
                    With P
                        .FixText_Key = "FP_SCAN_SELECT"
                        .SQL_WHERE = String.Format("Terminal='{0}'", Terminal)
                    End With
                    If Not gl_FPApp.SIMPLE_SELECT(P, P_OUT) Then
                        ScanDocument = False
                        Exit Function
                    End If
                    SelectedScanner = P_OUT.Selected_String
                End If
                gl_FPApp.PFDinsertOrUpdate(PFDKey_for_Scanner, SelectedScanner)
            End If

            m_TwainManager.IfDisableSourceAfterAcquire = True
            m_TwainManager.IfShowUI = True
            m_TwainManager.SelectSourceByIndex(CShort(tempList(SelectedScanner)))
            m_TwainManager.AcquireImage(TryCast(Me, IAcquireCallback))
            If OUT_BitmapList.Count = 0 Then
                ScanDocument = False
                Exit Function
            End If

            OUT = True
        End If

        Return OUT
    End Function

#End Region

#Region "PUBLIC PROPERTIES"

    Public ReadOnly Property P_BitmapList As List(Of Bitmap)
        Get
            Dim OUT As List(Of Bitmap) = Nothing

            If Not Disposed Then
                OUT = OUT_BitmapList
            End If

            Return OUT
        End Get
    End Property

#End Region

#Region "PRIVATE FUNCTIONS"

    Private Function OnPostTransfer(bit As Bitmap) As Boolean Implements IAcquireCallback.OnPostTransfer
        OUT_BitmapList.Add(bit)
        Return True
    End Function

    Private Function OnPreTransfer() As Boolean Implements IAcquireCallback.OnPreTransfer
        Return True
    End Function

#End Region

#Region "PRIVATE SUBS"

    Private Sub OnPostAllTransfers() Implements IAcquireCallback.OnPostAllTransfers

    End Sub

    Private Sub OnPreAllTransfers() Implements IAcquireCallback.OnPreAllTransfers

    End Sub

    Private Sub OnSourceUIClose() Implements IAcquireCallback.OnSourceUIClose

    End Sub

    Private Sub OnTransferCancelled() Implements IAcquireCallback.OnTransferCancelled

    End Sub

    Private Sub OnTransferError() Implements IAcquireCallback.OnTransferError

    End Sub

#End Region

End Class
