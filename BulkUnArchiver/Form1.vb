Imports System.IO

Public Class Form1
    Public archives As New List(Of String)
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim dialog = FolderBrowserDialog1.ShowDialog
        If dialog = DialogResult.OK Then
            AddDirectory(FolderBrowserDialog1.SelectedPath)
        End If
    End Sub

    Public Sub unzip(path As String)

    End Sub

    Public Sub PrintLog(Text As String)
        RichTextBox1.DeselectAll()
        Dim ora As String = Date.Now & ": "
        Dim rtf As String = ora & Text & vbNewLine
        RichTextBox1.Text += rtf
    End Sub


End Class
