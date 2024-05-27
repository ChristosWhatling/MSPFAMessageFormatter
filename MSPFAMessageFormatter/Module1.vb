Module Module1

    Public Class characterFormat
        Public Property characterHandle As String = ""
        Public Property characterFont As String = ""
        Public Property characterColor As String = ""
    End Class
    Sub Main()
        'Init
        Dim clipboard As String = My.Computer.Clipboard.GetText()
        Dim formatted As String = ""
        Dim characterFormats As New List(Of characterFormat)

        'Get formatting data
        Dim EOFReached As Boolean = False
        Dim fileLine As Integer = 1
        Dim currentCharacter As Integer = 0
        Dim readLine As String
        Do Until EOFReached
            readLine = ReadTextFile("\data.txt", fileLine)
            If readLine = "[EOF]" Then
                EOFReached = True
            Else
                If InStr(readLine, "[handle]") Then
                    currentCharacter += 1
                    Dim newCharacter As New characterFormat
                    newCharacter.characterHandle = Mid(readLine, 10)
                    characterFormats.Add(newCharacter)
                ElseIf InStr(readLine, "[font]") Then
                    characterFormats(currentCharacter - 1).characterFont = Mid(readLine, 8)
                ElseIf InStr(readLine, "[color]") Then
                    characterFormats(currentCharacter - 1).characterColor = Mid(readLine, 9)
                End If
                Console.WriteLine(readLine)
                fileLine += 1
            End If
        Loop

        'Format text
        Dim clipboardSplit As String() = Split(clipboard, Environment.NewLine)
        For Each x In clipboardSplit
            Dim currentHandle As String = Split(x, ":")(0)
            For Each y In characterFormats
                Dim closeColor As Boolean = False
                Dim closeFont As Boolean = False
                If currentHandle = y.characterHandle Then
                    If y.characterColor <> "" Then
                        formatted = formatted & "[color=#" & y.characterColor & "]"
                        closeColor = True
                    End If
                    formatted = formatted & y.characterHandle & ":"
                    If y.characterFont <> "" Then
                        formatted = formatted & "[font=" & y.characterFont & "]"
                        closeFont = True
                    End If
                    formatted = formatted & Split(x, ":")(1)
                    If closeColor Then
                        formatted = formatted & "[/color]"
                    End If
                    If closeFont Then
                        formatted = formatted & "[/font]"
                    End If
                    formatted = formatted & Environment.NewLine
                End If
            Next
        Next

        'Print formatted text
        Console.Clear()
        Console.WriteLine(formatted)
        Console.WriteLine("")
        Console.WriteLine("[Text Formatted!]")
        Console.WriteLine("[Enter - Exit]")
        Console.ReadLine()
    End Sub
    Public Function ReadTextFile(filedir As String, lineToRead As Integer)
        Dim currentLine As String = ""
        Dim returnline As String = ""
        Dim counter As Integer = 0
        Dim lineExistsCheck As Boolean = False
        FileOpen(1, CurDir() & filedir, OpenMode.Input)
        Do Until EOF(1) Or lineExistsCheck = True
            currentLine = LineInput(1)
            counter += 1
            If lineToRead = counter Then
                returnline = currentLine
                lineExistsCheck = True
            End If
        Loop

        If lineExistsCheck = False Then
            returnline = "[EOF]"
        End If

        FileClose(1)

        Return returnline
    End Function

End Module
