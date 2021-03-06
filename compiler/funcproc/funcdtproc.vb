﻿Public Class funcdtproc
    Structure _referfuncdata
        Dim classname As String
        Dim filename As String
        Dim methods() As tknformat._method
        Dim fields() As tknformat._pubfield
    End Structure

    Friend Shared reffunc() As _referfuncdata
    Friend Shared refrecord As New mapstoredata
    Friend Shared Sub import_method(classdt As tknformat._class)
        Dim index As Integer = 0
        If IsNothing(reffunc) Then
            Array.Resize(reffunc, 1)
            reffunc(0) = New _referfuncdata
        Else
            index = reffunc.Length
            Array.Resize(reffunc, reffunc.Length + 1)
            reffunc(index) = New _referfuncdata
        End If
        ' TODO : Fix Bug    reset_xml_data(classdt)
        reffunc(index).filename = classdt.name
        reffunc(index).fields = classdt.fields
        reffunc(index).methods = classdt.methods
        If classdt.attribute._app._namespace <> conrex.NULL Then
            reffunc(index).classname = classdt.attribute._app._namespace & conrex.DOT
        End If
        reffunc(index).classname &= classdt.attribute._app._classname
        refrecord.add(reffunc(index).classname, index)
    End Sub

    Private Shared Sub reset_xml_data(ByRef classdt As tknformat._class)
        If IsNothing(classdt.methods) Then Return
        For indexmethod = 0 To classdt.methods.Length - 1
            classdt.methods(indexmethod).bodyxmlfmt = String.Empty
        Next
    End Sub
    Friend Shared Function get_index_method(ByRef funcname As String, classindex As Integer) As Integer
        If IsNothing(reffunc(classindex).methods) Then Return -1
        funcname = funcname.ToLower
        For index = 0 To reffunc(classindex).methods.Length - 1
            If reffunc(classindex).methods(index).name.ToLower = funcname Then
                funcname = reffunc(classindex).methods(index).name
                Return index
            End If
        Next
        Return -1
    End Function

    Friend Shared Function get_index_class(ByRef classname As String) As Integer
        Dim classchename As String = String.Empty
        Dim resultclassindex As mapstoredata.dataresult = refrecord.find(classname, True, classchename)
        If resultclassindex.issuccessful Then
            classname = classchename
            Return resultclassindex.result
        Else
            Return -1
        End If
    End Function

    Friend Shared Function get_method_info(classindex As Integer, methodindex As Integer) As tknformat._method
        Return reffunc(classindex).methods(methodindex)
    End Function

    Friend Shared Function get_index_field(ByRef identifier As String, classindex As Integer) As Integer
        If IsNothing(reffunc(classindex).fields) Then Return -1
        identifier = identifier.ToLower
        For index = 0 To reffunc(classindex).fields.Length - 1
            If reffunc(classindex).fields(index).name.ToLower = identifier Then
                identifier = reffunc(classindex).fields(index).name
                Return index
            End If
        Next
        Return -1
    End Function
    Friend Shared Function get_field_info(classindex As Integer, fieldindex As Integer) As tknformat._pubfield
        Return reffunc(classindex).fields(fieldindex)
    End Function
End Class
