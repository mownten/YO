﻿Public Class rangeserv

    Structure ranginf
        Dim startpoint As Object
        Dim endpoint As Object
        Dim stepsc As Integer
        Dim ignorelastpoint As Boolean
    End Structure
    Public Shared Function get_range_info(rangestring As xmlunpkd.linecodestruc) As ranginf
        If rangestring.tokenid <> tokenhared.token.RANGE Then Return Nothing
        Dim rvalue As String = rangestring.value
        authfunc.rem_fr_and_en(rvalue)
        Dim rninf As New ranginf
        rninf.stepsc = 1
        rninf.startpoint = rvalue.Remove(rvalue.IndexOf(conrex.DBDOT))
        rninf.endpoint = rvalue.Remove(0, rvalue.IndexOf(conrex.DBDOT) + 2)
        If rninf.endpoint.ToString.StartsWith(conrex.EQU) Then
            rninf.endpoint = rninf.endpoint.ToString.Remove(0, 1)
            rninf.ignorelastpoint = False ' [0..=5]
        Else
            rninf.ignorelastpoint = True ' [0..5]
        End If
        Return rninf
    End Function
End Class
