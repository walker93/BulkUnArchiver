Imports System.IO

Public Class Form1
    Public archives As New List(Of String)
    Public dest_dir As String
    Public start_dir As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btn_sorgente.Click
        Dim dialog = FolderBrowserDialog1.ShowDialog
        If dialog = DialogResult.OK Then
            stato_label.Text = "Ricerca degli archivi..."
            btn_estrai.Enabled = False
            start_dir = FolderBrowserDialog1.SelectedPath
            ScanDir(start_dir)
            Label1.Text = FolderBrowserDialog1.SelectedPath
            stato_label.Text = "Pronto"
            btn_estrai.Enabled = True
        End If
    End Sub

    Public Sub unzip(path As String, Optional overwrite As Boolean = True)
        Dim file = New FileInfo(path)
        Dim Dir_info = New DirectoryInfo(dest_dir)
        Dim start_info = New DirectoryInfo(start_dir)
        Dim commonpath = getcommonpath(file.Directory.Parent.FullName, dest_dir)
        Dim combiningdestdir = dest_dir & "\"
        Dim tmpDir = "_temp"
        If Not path.Contains(dest_dir) Then
            combiningdestdir += file.DirectoryName.Replace(commonpath & start_info.Name & "\", "") & "\" & file.Name
        Else
            combiningdestdir = file.DirectoryName & "\" & file.Name & tmpDir
        End If

        Dim dir = IO.Directory.CreateDirectory(combiningdestdir)
        Try
            Dim p As New ProcessStartInfo()
            p.FileName = "7z.exe"
            p.Arguments = String.Format("x ""{0}"" -o""{1}"" -ao{2}", path, dir.FullName, If(overwrite, "a", "s"))
            p.WindowStyle = ProcessWindowStyle.Hidden
            p.UseShellExecute = False
            p.RedirectStandardOutput = True
            p.RedirectStandardError = True
            p.StandardOutputEncoding = System.Text.Encoding.ASCII
            p.StandardErrorEncoding = System.Text.Encoding.ASCII
            Dim proc = Process.Start(p)
            Dim reader = proc.StandardOutput
            Dim err = proc.StandardError
            proc.WaitForExit()
            'PrintLog("=================")
            'PrintLog(reader.ReadToEnd)
            'PrintLog("________________")
            'PrintLog(err.ReadToEnd)
            'PrintLog("=================")
            Select Case proc.ExitCode
                Case 0
                    PrintLog("'" & path & "' processato correttamente")
                Case 1
                    PrintLog("WARNING: '" & path & "' ha dato problemi")
                Case 2
                    PrintLog("FATAL: '" & path & "' Errore Fatale")
                Case 7
                    PrintLog("ERROR: '" & path & "' riga di comando non valida")
                Case 8
                    PrintLog("ERROR: '" & path & "' memoria esaurita")
                Case 255
                    PrintLog("ERROR: '" & path & "'. Processo interrotto dall'utente")
                Case Else
                    PrintLog("ERRORE IMPREVISTO: '" & path & "'")
            End Select
        Catch ex As Exception
            PrintLog("ERRORE: '" & path & "' estrazione non riuscita.")
        End Try
        If path.Contains(dest_dir) Then
            IO.File.Delete(path)
            dir.MoveTo(dir.FullName.Replace(tmpDir, ""))
        End If
        ScanDir(dir.FullName, True)
        archives.Remove(path)
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
        stato_label.Text = "Estrazione in corso..."

        btn_estrai.Enabled = False
        Dim i As Integer = archives.Count - 1
        While archives.Count > 0
            i = archives.Count - 1
            Application.DoEvents()
            unzip(archives(i), CheckBox1.Checked)
        End While
        btn_estrai.Enabled = True
        stato_label.Text = "Pronto"
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        RichTextBox1.Clear()
    End Sub

    Public Sub ScanDir(dir_path As String, Optional output As Boolean = False)
        AddDirectory(dir_path, output)
    End Sub

    Private Sub SalvaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalvaToolStripMenuItem.Click
        IO.File.WriteAllText(dest_dir & "\LOG.txt", RichTextBox1.Text)
    End Sub
End Class
