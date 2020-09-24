﻿Public Class execln
    Friend Shared Sub nv_check_command(result As parseargs._parseresult)
        If result.command = Nothing Or result.command = conrex.SPACE Then
            Return
        End If
        Dim getindex As Int16 = cmdlnproc.check_master_key(result.command)
        If getindex = -1 Then
            'Set Error , command not found .
            dserr.set_error("Command Error", "'" & result.command & "' - Command not found." & vbLf & "This command may be used in updated versions of Labra.")
            Return
        End If

        If cmdlnproc.cmd(getindex).withargs = False AndAlso result.args.Count > 0 Then
            dserr.set_error("Parameter Error", "'" & result.command & "' - This command does not support any parameters, enter this command without entering a parameter.")
            Return
        ElseIf cmdlnproc.cmd(getindex).maxargs < result.args.Count Then
            dserr.set_error("Parameter Error", "'" & result.command & "' - The number of parameters of this command is too much.")
            Return
        End If

        Dim cmdexeclninstance As New execln
        CallByName(cmdexeclninstance, "rp_" & result.command, CallType.Method, result.args)
    End Sub
    Public Sub New()
    End Sub
    Public Sub rp_test(args As ArrayList)
        Dim peconsolecolor As Int16 = Console.ForegroundColor
        Console.ForegroundColor = System.ConsoleColor.DarkGreen
        Console.Write("Bravo !!!!! Google is ready to execute your commands.
You can type 'Help' to view commands.")
        Console.ForegroundColor = peconsolecolor
    End Sub

End Class
