Imports Dynamsoft.PDF
Imports Dynamsoft.Core
Imports Dynamsoft.Core.Enums

Public Class FP_PDF
    Implements ISave

    Private m_StrProductKey As String = "f0068WQAAAKA1fP6KVphz/vPpvgguvkCN8hbMnVecWe+SpQ6yy09Bjvx4KFRA4SibWulVqw/pGpi7QeGT7XrrNDTYTsnSqFQ="
    Private m_ImageCore As ImageCore = Nothing
    Private m_IndexList As New List(Of Short)
    Private m_PDFCreator As PDFCreator = Nothing

    Public Sub New()
        m_ImageCore = New ImageCore()
        m_PDFCreator = New PDFCreator(m_StrProductKey)
        m_IndexList.Add(0)
    End Sub

    Public Sub SaveToSinglePDF(BitmapList As List(Of Bitmap), FileNameWithPath As String)
        For Each Bitmap As Bitmap In BitmapList
            m_ImageCore.IO.LoadImage(Bitmap)
        Next
        m_PDFCreator.Save(TryCast(Me, ISave), FileNameWithPath)
    End Sub

    Public Sub SaveToMultiplePDF(bit As Bitmap, FileNameWithPath As String)
        m_ImageCore.IO.LoadImage(bit)
        m_PDFCreator.Save(TryCast(Me, ISave), FileNameWithPath)
    End Sub

#Region "ISave Members"

    Private Function GetAnnotations(iPageNumber As Integer) As Object Implements ISave.GetAnnotations
        Return Nothing
    End Function

    Private Function GetImage(iPageNumber As Integer) As System.Drawing.Bitmap Implements ISave.GetImage
        Return m_ImageCore.ImageBuffer.GetBitmap(CShort(iPageNumber))
    End Function

    Private Function GetPageCount() As Integer Implements ISave.GetPageCount
        Return m_ImageCore.ImageBuffer.HowManyImagesInBuffer
    End Function

#End Region

End Class

