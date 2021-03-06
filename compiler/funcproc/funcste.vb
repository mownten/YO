﻿Imports YOCA

Public Class funcste
    Friend Shared attribute As yocaattribute.yoattribute
    Friend Shared Sub invoke_method(clinecodestruc() As xmlunpkd.linecodestruc, ByRef _ilmethod As ilformat._ilmethodcollection, funcresult As funcvalid._resultfuncvaild, Optional leftassign As Boolean = True)
        fscmawait = False
        If funcresult.callintern Then
            inv_internal_method(clinecodestruc, _ilmethod, funcresult.exclass, funcresult, leftassign)
        Else
            inv_external_method(clinecodestruc, _ilmethod, funcresult.exclass, funcresult, leftassign)
        End If
    End Sub

    Friend Shared Sub inv_external_method(clinecodestruc() As xmlunpkd.linecodestruc, ByRef _ilmethod As ilformat._ilmethodcollection, classname As String, funcresult As funcvalid._resultfuncvaild, leftassign As Boolean)
        Dim classindex, namespaceindex As Integer
        If libserv.get_extern_index_class(classname, namespaceindex, classindex) = -1 Then
            dserr.args.Add("Class " & classname & " not found.")
            dserr.new_error(conserr.errortype.METHODERROR, clinecodestruc(0).line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc(0)), clinecodestruc(0).value))
            Return
        End If
        funcresult.asmextern = libserv.get_extern_assembly(namespaceindex)

        Dim methodinfo As New tknformat._method
        Dim methodindex As Integer = libserv.get_extern_index_method(_ilmethod, get_argument_list(clinecodestruc), funcresult.clmethod, namespaceindex, classindex, methodinfo)

        If methodindex = -1 Then
            dserr.args.Add("Method " & funcresult.clmethod & "(...) not found.")
            dserr.new_error(conserr.errortype.METHODERROR, clinecodestruc(0).line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc(0)), clinecodestruc(0).value))
        End If

        Dim paramtype As ArrayList
        Dim cargcodestruc() As xmlunpkd.linecodestruc = libserv.cargldr
        libserv.cargldr = Nothing

        If IsNothing(methodinfo.parameters) = False Then
            ' Print Tokens :
            '  coutputdata.print_token(clinecodestruc)
            load_param_in_stack(clinecodestruc, _ilmethod, methodinfo, funcresult, paramtype, cargcodestruc)
        End If
        Dim getdatatype As String = methodinfo.returntype
        cil.call_extern_method(_ilmethod.codes, getdatatype, funcresult.asmextern, classname, funcresult.clmethod, paramtype)
        If leftassign AndAlso getdatatype <> Nothing AndAlso getdatatype <> "void" Then
            cil.pop(_ilmethod.codes)
        End If
    End Sub

    Friend Shared Sub inv_internal_method(clinecodestruc() As xmlunpkd.linecodestruc, ByRef _ilmethod As ilformat._ilmethodcollection, classname As String, funcresult As funcvalid._resultfuncvaild, leftassign As Boolean)
        Dim classindex As Integer = funcdtproc.get_index_class(classname)
        If classindex = -1 Then
            dserr.args.Add("Class " & classname & " not found.")
            dserr.new_error(conserr.errortype.METHODERROR, clinecodestruc(0).line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc(0)), clinecodestruc(0).value))
            Return
        End If
        Dim methodindex As Integer = funcdtproc.get_index_method(funcresult.clmethod, classindex)
        If methodindex = -1 Then
            dserr.args.Add("Method " & funcresult.clmethod & "(...) not found.")
            dserr.new_error(conserr.errortype.METHODERROR, clinecodestruc(0).line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc(0)), clinecodestruc(0).value))
        End If

        Dim methodinfo As tknformat._method = funcdtproc.get_method_info(classindex, methodindex)
        Dim paramtype As ArrayList
        If IsNothing(methodinfo.parameters) = False Then
            ' Print Tokens :
            '  coutputdata.print_token(clinecodestruc)
            load_param_in_stack(clinecodestruc, _ilmethod, methodinfo, funcresult, paramtype)
        End If
        Dim getdatatype As String = methodinfo.returntype
        cil.call_intern_method(_ilmethod.codes, getdatatype, classname, funcresult.clmethod, paramtype)
        If leftassign AndAlso getdatatype <> Nothing AndAlso getdatatype <> "void" Then
            cil.pop(_ilmethod.codes)
        End If
    End Sub

    Private Shared Sub load_param_in_stack(clinecodestruc() As xmlunpkd.linecodestruc, ByRef _ilmethod As ilformat._ilmethodcollection, methodinfo As tknformat._method, funcresult As funcvalid._resultfuncvaild, ByRef paramtypes As ArrayList, Optional cargcodestruc() As xmlunpkd.linecodestruc = Nothing)
        If IsNothing(cargcodestruc) Then cargcodestruc = get_argument_list(clinecodestruc)

        If IsNothing(cargcodestruc) Or cargcodestruc.Length <> methodinfo.parameters.Length Then
            'TODO : PARAMARRAY
            dserr.args.Add("Argument Not specified For parameter")
            dserr.new_error(conserr.errortype.ARGUMENTERROR, clinecodestruc(clinecodestruc.Length - 1).line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc(clinecodestruc.Length - 1)), clinecodestruc(clinecodestruc.Length - 1).value))
            Return
        End If

        check_expression_method(clinecodestruc, methodinfo)
        paramtypes = New ArrayList
        For index = 0 To methodinfo.parameters.Length - 1
            Dim getcildatatype As String = servinterface.vb_to_cil_common_data_type(methodinfo.parameters(index).ptype)
            If methodinfo.parameters(index).ptype <> getcildatatype OrElse servinterface.is_common_data_type(getcildatatype, getcildatatype) Then
                If methodinfo.parameters(index).byreference Then getcildatatype &= "&"
                paramtypes.Add(getcildatatype)
            Else
                'Other Types...
            End If
        Next
        set_stack_space(_ilmethod, paramtypes, cargcodestruc)
    End Sub

    Private Shared Sub check_expression_method(clinecodestruc() As xmlunpkd.linecodestruc, methodinfo As tknformat._method)
        If methodinfo.isexpr Then
            For index = 0 To clinecodestruc.Length - 1
                If clinecodestruc(index).tokenid = tokenhared.token.NULL OrElse clinecodestruc(index).tokenid = tokenhared.token.TYPE_CO_STR OrElse clinecodestruc(index).tokenid = tokenhared.token.TYPE_DU_STR Then
                    dserr.args.Add(clinecodestruc(index).value)
                    dserr.new_error(conserr.errortype.EXPRMETHODERROR, clinecodestruc(index).line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc(index)), clinecodestruc(index).value))
                End If
            Next
        End If
    End Sub

    Private Shared Sub set_stack_space(ByRef _ilmethod As ilformat._ilmethodcollection, paramtypes As ArrayList, cargcodestruc As xmlunpkd.linecodestruc())
        Dim ldlc As New illdloc(_ilmethod)
        _ilmethod = ldlc.load_in_stack(paramtypes, cargcodestruc)
    End Sub
    Private Shared Function get_argument_list(clinecodestruc() As xmlunpkd.linecodestruc) As xmlunpkd.linecodestruc()
        Dim cargcodestruc() As xmlunpkd.linecodestruc
        Dim icarg As Integer = 0
        Dim stateparam As Boolean = False
        For index = 0 To clinecodestruc.Length - 1
            If stateparam = False AndAlso clinecodestruc(index).tokenid = tokenhared.token.PRSTART Then
                stateparam = True
                Continue For
            ElseIf stateparam = True AndAlso clinecodestruc(index).tokenid = tokenhared.token.PREND Then
                Return cargcodestruc
            ElseIf stateparam = False Then
                Continue For
            Else
                Dim cargprtester As xmlunpkd.linecodestruc
                If define_carg_store(clinecodestruc(index), cargprtester) Then
                    If IsNothing(cargcodestruc) = False Then
                        icarg = cargcodestruc.Length
                    End If
                    Array.Resize(cargcodestruc, icarg + 1)
                    cargcodestruc(icarg) = cargprtester
                End If
            End If
        Next
        Return cargcodestruc
    End Function

    Private Shared fscmawait As Boolean = False
    Private Shared Function define_carg_store(clinecodestruc As xmlunpkd.linecodestruc, ByRef cargcodestruc As xmlunpkd.linecodestruc) As Boolean
        If fscmawait Then
            If clinecodestruc.tokenid = tokenhared.token.CMA Then
                fscmawait = False
                Return False
            Else
                dserr.args.Add(conrex.CMA)
                dserr.new_error(conserr.errortype.EXPECTEDERROR, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
            End If
        Else
            If servinterface.check_argument_token(clinecodestruc.tokenid) Then
                cargcodestruc = clinecodestruc
                fscmawait = True
                Return True
            Else
                dserr.args.Add("'" & clinecodestruc.value & "' Cannot be identified as a parameter.")
                dserr.new_error(conserr.errortype.ARGUMENTERROR, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
            End If
        End If
        Return False
    End Function
End Class
