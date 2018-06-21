Imports System.IO
Imports System.Runtime.CompilerServices

Module extensions

    Dim supported_extensions() As String = {".rar", ".7zip", ".zip", ".tar", ".gz"}
    Public Function isSupportedFile(file As FileInfo) As Boolean
        Return supported_extensions.Contains(file.Extension.ToLower)
    End Function

    Public Sub AddFiles(path As String, Output As Boolean)
        Dim file = New FileInfo(path)
        If isSupportedFile(file) Then
            Dim p = file.FullName
            Form1.archives.Insert(0, p)
            'If Output Then IO.File.Delete(p)
            Form1.PrintLog("Aggiunto archivio: " & p)
            Application.DoEvents()
        Else
            'Form1.PrintLog("Not supported:  " + file.FullName)
        End If
    End Sub

    Public Sub AddFiles(paths As String(), output As Boolean)
        For Each p In paths
            Try
                AddFiles(p, output)
            Catch ex As Exception
                Form1.PrintLog("ERROR: " & ex.Message)
            End Try
        Next
    End Sub
    Public Sub AddDirectory(path As String, Optional output As Boolean = True)
        Try
            Dim directories = Directory.GetDirectories(path)
            For Each dire In directories
                AddDirectory(dire, output)
            Next
            Dim files = Directory.GetFiles(path)
            AddFiles(files, output)
            Application.DoEvents()
        Catch ex As Exception
            Form1.PrintLog("ERROR: " & ex.Message)
        End Try
    End Sub

    Public Function getcommonpath(path1 As String, path2 As String) As String
        Dim commonpath As String = ""
        Dim folders1() = path1.Split("\")
        Dim folders2() = path2.Split("\")
        If folders1.Length >= folders2.Length Then
            For i = 0 To folders2.Length - 1
                If folders2(i) = folders1(i) Then commonpath &= folders2(i) & "\"
            Next
        Else
            For i = 0 To folders1.Length - 1
                If folders2(i) = folders1(i) Then commonpath &= folders1(i) & "\"
            Next
        End If
        Return commonpath
    End Function
End Module

Module RtfExtensions
    public reg_font As Font = New Font(FontFamily.GenericSansSerif, FontStyle.Regular)
    Public LogFonts As New Dictionary(Of String, Font) From {
        {"err", New Font(FontFamily.GenericSansSerif, FontStyle.Bold)},
        {"regular", New Font(FontFamily.GenericSansSerif, FontStyle.Regular)}
    }

    <Extension()>
    Public Function ToRtf(s As String) As String
        Return "{\rtf1\ansi" + s + "}"
    End Function

    <Extension()>
    Public Function ToBold(s As String) As String
        Return String.Format("\b {0}\b0 ", s)
    End Function

End Module