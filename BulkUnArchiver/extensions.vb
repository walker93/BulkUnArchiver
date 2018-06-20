Imports System.IO
Imports System.Runtime.CompilerServices

Module extensions

    Dim supported_extensions() As String = {".rar", ".7zip", ".zip", ".tar", ".gz"}
    Public Function isSupportedFile(file As FileInfo) As Boolean
        Return supported_extensions.Contains(file.Extension.ToLower)
    End Function

    Public Sub AddFiles(path As String)
        Dim file = New FileInfo(path)
        If isSupportedFile(file) Then
            Dim p = file.FullName
            Form1.archives.Add(p)
            Form1.PrintLog("Aggiunto archivio: " & p)
            Application.DoEvents()
        Else
            Form1.PrintLog("Not supported:  " + file.FullName)
        End If
    End Sub

    Public Sub AddFiles(paths As String())
        For Each p In paths
            AddFiles(p)
        Next
    End Sub
    Public Sub AddDirectory(path As String, Optional recursive As Boolean = True)
        If recursive Then
            Dim directories = Directory.GetDirectories(path)
            For Each dire In directories
                AddDirectory(dire, recursive)
            Next
        End If
        Dim files = Directory.GetFiles(path)
        AddFiles(files)
    End Sub


End Module

Module RtfExtensions

    <Extension()>
    Public Function ToRtf(s As String) As String
        Return "{\rtf1\ansi" + s + "}"
    End Function

    <Extension()>
    Public Function ToBold(s As String) As String
        Return String.Format("\b {0}\b0 ", s)
    End Function

End Module