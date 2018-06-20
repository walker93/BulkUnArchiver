Imports System.IO

Public Class Form1
    Public archives As New List(Of String)
    Public dest_dir As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btn_sorgente.Click
        Dim dialog = FolderBrowserDialog1.ShowDialog
        If dialog = DialogResult.OK Then
            stato_label.Text = "Scansione degli archivi..."
            btn_estrai.Enabled = False
            AddDirectory(FolderBrowserDialog1.SelectedPath)
            Label1.Text = FolderBrowserDialog1.SelectedPath
            stato_label.Text = "Pronto"
            btn_estrai.Enabled = True
        End If
    End Sub

    Public Sub unzip(path As String, Optional overwrite As Boolean = True)
        Try
            Dim p As New ProcessStartInfo()
            p.FileName = "7za.exe"
            p.Arguments = String.Format("x {0} -o{1} -ao{2}", path, dest_dir, If(overwrite, "a", "s"))
            p.WindowStyle = ProcessWindowStyle.Hidden
            Dim proc = Process.Start(p)
            proc.WaitForExit()
            Select Case proc.ExitCode
                Case 0
                    PrintLog("'" & path & "'  processato correttamente")
                Case 1
                    PrintLog("WARNING: '" & path & "' ha dato problemi")
                Case 2
                    PrintLog("FATAL: '" & path & "' Errore Fatale")
                Case 7
                    PrintLog("ERROR: '" & path & "' riga di comando non valida")
                Case 8
                    PrintLog("ERROR: '" & path & "' memoria esaurita")
                Case 255
                    PrintLog("ERROR: '" & path & "' Processo interrotto dall'utente")
                Case Else
                    PrintLog("ERRORE IMPREVISTO: '" & path & "'")
            End Select
        Catch ex As Exception
            PrintLog("ERRORE: '" & path & "' estrazione non riuscita.")
        End Try
    End Sub

    Public Sub PrintLog(Text As String)
        RichTextBox1.DeselectAll()
        Dim ora As String = Date.Now & ": "
        Dim rtf As String = ora & Text & vbNewLine
        RichTextBox1.Text += rtf
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btn_dest.Click
        Dim dialog = FolderBrowserDialog1.ShowDialog
        If dialog = DialogResult.OK Then
            dest_dir = FolderBrowserDialog1.SelectedPath
            Label2.Text = dest_dir
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btn_estrai.Click
        PrintLog("Avvio estrazione di " & archives.Count & " archivi.")
        btn_estrai.Enabled = False
        For Each archive In archives
            unzip(archive, CheckBox1.Checked)
        Next
        btn_estrai.Enabled = True
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        RichTextBox1.Clear()
    End Sub
End Class
