﻿Imports System.IO

Public Class Files

    Dim DirFiles As New List(Of Label)

    Public Function RetrieveFilesIn(DirPath As String)
        Me.DirPath.Text = DirPath
        TextBox1.Text = DirPath
        Dim labelX = 205
        Dim labelY = 115
        Debug.Print("Deleting old labels")
        Dim lb As Label
        For Each lb In DirFiles
            Debug.Print("Iter: " & lb.Text)
            RemoveHandler lb.Click, AddressOf Me.Files_Click
            lb.Dispose()
        Next
        Debug.Print("Getting files in " & Me.DirPath.Text)
        Dim files As String() = Directory.GetFiles(DirPath)
        Debug.Print(files.ToString)
        Dim fname As String
        For Each fname In files
            Debug.Print("Iter: " & fname)
            Dim label As Label = New Label()
            label.Size = New Size(Me.Width - labelX, 20)
            label.Location = New Point(labelX, labelY)
            label.Text = Path.GetFileName(fname)
            Me.Controls.Add(label)
            AddHandler label.Click, AddressOf Me.Files_Click
            label.Show()
            label.Visible = True
            label.BringToFront()
            DirFiles.Add(label)
            labelY += 25
        Next
        Return 0
    End Function

    Private Sub UnauthFiles_Click(sender As Object, e As EventArgs) Handles SysFilesLabel.Click, FolderIcon3.Click
        ModalBox.ShowModal("Access Denied", "You don't have permissions to view this folder.", YesNoModal:=False)
        ModalBox.Close()
    End Sub

    Private Sub UsrFiles_Click(sender As Object, e As EventArgs) Handles UsrFilesLabel.Click, FolderIcon2.Click
        RetrieveFilesIn(My.Computer.FileSystem.SpecialDirectories.Desktop)
    End Sub

    Private Sub Files_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Hide()
        RetrieveFilesIn(My.Computer.FileSystem.SpecialDirectories.MyDocuments)
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Me.Close()
    End Sub

    Private Sub Files_Click(sender As Object, e As EventArgs)
        Notes.OpenNote(DirPath.Text & "\" & sender.Text)
    End Sub

    Private Sub Docs_Click(sender As Object, e As EventArgs) Handles Label3.Click, PictureBox4.Click
        RetrieveFilesIn(My.Computer.FileSystem.SpecialDirectories.MyDocuments)
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Return Then
            TextBox1.Hide()
            Try
                RetrieveFilesIn(TextBox1.Text)
            Catch ex As DirectoryNotFoundException
                ModalBox.ShowModal("Directory Not Found", DirPath.Text & " is not a directory.", YesNoModal:=False)
                ModalBox.Close()
                RetrieveFilesIn(My.Computer.FileSystem.SpecialDirectories.MyDocuments)
            Catch ex As Exception
                ModalBox.ShowModal("An Error Occurred", "An unexpected error occurred while fetching directory contents." & vbCrLf & vbCrLf & ex.ToString, YesNoModal:=False)
                ModalBox.Close()
                RetrieveFilesIn(My.Computer.FileSystem.SpecialDirectories.MyDocuments)
            End Try
        End If
    End Sub

    Private Sub DirPath_Click(sender As Object, e As EventArgs) Handles DirPath.Click
        TextBox1.Show()
    End Sub
End Class