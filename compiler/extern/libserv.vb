﻿Imports System.Reflection
Imports YOCA

Public Class libserv

    Friend Shared Function find_extern_name(name As String) As Boolean
        Return libreg.assemblymap.find(name, True).issuccessful
    End Function
    Friend Shared Function get_extern_assembly(indexasm As Integer) As String
        Dim getextername As String = String.Empty
        libreg.assemblymap.get_index(indexasm, getextername)
        Return getextername
    End Function
    Friend Shared Function get_extern_index_class(ByRef classname As String, ByRef namespaceptr As Integer, ByRef classptr As Integer) As Integer
        Dim classchename As String = String.Empty
        Dim resultclassindex As mapstoredata.dataresult = libreg.externtypes.find(classname, True, classchename)
        If resultclassindex.issuccessful Then
            classname = classchename
            Dim spdata As String = resultclassindex.result
            namespaceptr = spdata.Remove(spdata.IndexOf(conrex.CMA))
            classptr = spdata.Remove(0, spdata.IndexOf(conrex.CMA) + 1)
            Return 1
        Else
            Return -1
        End If
    End Function

    Friend Shared Function get_extern_index_method(_ilmethod As ilformat._ilmethodcollection, cargcodestruc() As xmlunpkd.linecodestruc, ByRef funcname As String, namespaceindex As Integer, classindex As Integer, ByRef methodinfo As tknformat._method) As Integer
        Dim funcnametolower As String = funcname.ToLower
        cargldr = Nothing
        For Each method In libreg.types(namespaceindex)(classindex).GetMethods()
            If method.Name.ToLower = funcnametolower AndAlso check_overloading(_ilmethod, method, cargcodestruc) Then
                funcname = method.Name
                get_method_info(method, methodinfo)
                Return 1
            End If
        Next
        Return -1
    End Function

    Friend Shared cargldr() As xmlunpkd.linecodestruc = Nothing
    Private Shared Function check_overloading(_ilmethod As ilformat._ilmethodcollection, method As MethodInfo, cargcodestruc() As xmlunpkd.linecodestruc) As Boolean
        'TODO : Check Return-Type
        Dim cargcodelen As Integer = 0
        If IsNothing(cargcodestruc) = False Then cargcodelen = cargcodestruc.Length
        If cargcodelen <> method.GetParameters.Length Then
            Return False
        ElseIf method.GetParameters.Length = 0 AndAlso cargcodelen = 0 Then
            Return True
        End If

        Dim paramtypes As New ArrayList
        For index = 0 To method.GetParameters.Length - 1
            paramtypes.Add(servinterface.vb_to_cil_common_data_type(method.GetParameters(index).ParameterType.Name))
        Next

        cargldr = cargcodestruc
        Return parampt.check_param_types(_ilmethod, paramtypes, cargcodestruc)
    End Function

    Friend Shared Sub get_method_info(method As Reflection.MethodInfo, ByRef methodinfo As tknformat._method)
        methodinfo.name = method.Name
        methodinfo.isexpr = False
        If method.ReturnType.Name = "Void" Then
            methodinfo.returntype = "void"
        Else
            methodinfo.returntype = servinterface.vb_to_cil_common_data_type(method.ReturnType.Name)
        End If

        For index = 0 To method.GetParameters().Length - 1
            Array.Resize(methodinfo.parameters, index + 1)
            methodinfo.parameters(index) = New tknformat._parameter
            methodinfo.parameters(index).name = method.GetParameters(index).Name
            methodinfo.parameters(index).ptype = method.GetParameters(index).ParameterType.Name
        Next

    End Sub

End Class
