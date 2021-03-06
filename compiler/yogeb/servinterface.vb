﻿Imports YOCA

Public Class servinterface
    Friend Shared clinecodestruc As xmlunpkd.linecodestruc
    Friend Shared Sub ldc_i_checker(ByRef codes As ArrayList, value As Object, Optional convtoi8 As Boolean = False, Optional datatype As String = "int32")
        rem_float(value)
        Select Case datatype
            Case "int64"
                If CDec(value) > Int32.MaxValue Or CDec(value) < Int32.MinValue Then
                    cil.push_int64_onto_stack(codes, CDec(value))
                Else
                    cil.push_int32_onto_stack(codes, CDec(value))
                    If convtoi8 Then
                        cil.conv_to_int64(codes)
                    End If
                End If
            Case "int32"
                If CDec(value) > Int32.MaxValue Or CDec(value) < Int32.MinValue Then
                    cil.push_int64_onto_stack(codes, CDec(value))
                Else
                    cil.push_int32_onto_stack(codes, CDec(value))
                    If convtoi8 Then
                        cil.conv_to_int64(codes)
                    End If
                End If
            Case "int16"
                If CDec(value) > Int16.MaxValue Or CDec(value) < Int16.MinValue Then
                    dserr.args.Add(value)
                    dserr.new_error(conserr.errortype.INTEGRALOVERFLOW, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
                Else
                    cil.push_int32_onto_stack(codes, CDec(value))
                End If
            Case "int8"
                If CDec(value) > SByte.MaxValue Or CDec(value) < SByte.MinValue Then
                    dserr.args.Add(value)
                    dserr.new_error(conserr.errortype.INTEGRALOVERFLOW, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
                Else
                    cil.push_int32_onto_stack(codes, CDec(value))
                End If

            Case "uint8"
                If CDec(value) < 0 Then
                    dserr.args.Add(value)
                    dserr.args.Add("u8")
                    dserr.new_error(conserr.errortype.ERRORINCONVERT, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
                End If
                If CDec(value) > Byte.MaxValue Then
                    dserr.args.Add(value)
                    dserr.args.Add("u8")
                    dserr.new_error(conserr.errortype.ERRORINCONVERT, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
                Else
                    cil.push_int32_onto_stack(codes, CDec(value))
                    If convtoi8 Then
                        cil.conv_to_int64(codes)
                    End If
                End If
            Case "uint16"
                If CDec(value) < 0 Then
                    dserr.args.Add(value)
                    dserr.args.Add("u16")
                    dserr.new_error(conserr.errortype.ERRORINCONVERT, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
                End If
                If CDec(value) > UInt16.MaxValue Then
                    dserr.args.Add(value)
                    dserr.args.Add("u16")
                    dserr.new_error(conserr.errortype.ERRORINCONVERT, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
                Else
                    cil.push_int32_onto_stack(codes, CDec(value))
                    If convtoi8 Then
                        cil.conv_to_int64(codes)
                    End If
                End If
            Case "uint32"
                If CDec(value) < 0 Then
                    dserr.args.Add(value)
                    dserr.args.Add("u32")
                    dserr.new_error(conserr.errortype.ERRORINCONVERT, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
                End If
                If CDec(value) > UInt32.MaxValue Or CDec(value) < UInt32.MinValue Then
                    cil.push_int64_onto_stack(codes, CDec(value))
                Else
                    cil.push_int32_onto_stack(codes, CDec(value))
                    If convtoi8 Then
                        cil.conv_to_int64(codes)
                    End If
                End If
            Case "uint64"
                If CDec(value) < 0 Then
                    dserr.args.Add(value)
                    dserr.args.Add("u64")
                    dserr.new_error(conserr.errortype.ERRORINCONVERT, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
                End If
                If CDec(value) > UInt32.MaxValue Or CDec(value) < UInt32.MinValue Then
                    cil.push_int64_onto_stack(codes, CDec(value))
                Else
                    cil.push_int32_onto_stack(codes, CDec(value))
                    If convtoi8 Then
                        cil.conv_to_int64(codes)
                    End If
                End If
            Case "bool"
                If CDec(value) <> 1 AndAlso CDec(value) <> 0 Then
                    dserr.args.Add(value)
                    dserr.args.Add("bool")
                    dserr.new_error(conserr.errortype.ERRORINCONVERT, clinecodestruc.line, ilbodybulider.path, authfunc.get_line_error(ilbodybulider.path, servinterface.get_target_info(clinecodestruc), clinecodestruc.value))
                Else
                    cil.push_int32_onto_stack(codes, CDec(value))
                End If
        End Select

    End Sub

    Friend Shared Sub ldc_r_checker(ByRef codes As ArrayList, value As Object, Optional convtor8 As Boolean = False)
        If convtor8 OrElse value > Single.MaxValue OrElse value < Single.MinValue Then
            cil.push_float64_onto_stack(codes, CDbl(value))
        Else
            cil.push_float32_onto_stack(codes, CSng(value))
        End If
    End Sub

    Friend Shared Function is_i8(datatype As String) As Boolean
        For index = 0 To compdt.i8cmtypes.Length - 1
            If datatype = compdt.i8cmtypes(index) Then
                Return True
            End If
        Next
        Return False
    End Function

    Friend Shared Function is_common_data_type(datatype As String, ByRef cildatatype As String) As Boolean
        datatype = datatype.ToLower
        For index = 0 To conrex.yocommondatatype.Length - 1
            If datatype = conrex.yocommondatatype(index) Then
                cildatatype = conrex.msilcommondatatype(index)
                Return True
            End If
        Next
        Return False
    End Function

    Friend Shared Function vb_to_cil_common_data_type(datatype As String) As String
        For index = 0 To conrex.plvbcommondatatype.Length - 1
            If datatype = conrex.plvbcommondatatype(index) Then
                Return conrex.msilcommondatatype(index)
            End If
        Next
        Return datatype
    End Function

    Friend Shared Function get_yo_common_data_type(datatype As String, ByRef yodatatype As String) As Boolean
        datatype = datatype.ToLower
        For index = 0 To conrex.yocommondatatype.Length - 1
            If datatype = conrex.msilcommondatatype(index) Then
                yodatatype = conrex.yocommondatatype(index)
                Return True
            End If
        Next
        Return False
    End Function

    Friend Shared Function is_cil_common_data_type(datatype As String) As Boolean
        For index = 0 To conrex.msilcommondatatype.Length - 1
            If datatype = conrex.msilcommondatatype(index) Then
                Return True
            End If
        Next
        Return False
    End Function

    Friend Shared Sub rem_float(ByRef value As Object)
        If value.ToString.Contains(conrex.DOT) Then
            Dim expr As String = value
            value = CObj(expr.Remove(expr.IndexOf(conrex.DOT)))
            Return
        End If
    End Sub

    Friend Shared Function get_target_info(clinecodestruc As xmlunpkd.linecodestruc) As lexer.targetinf
        Dim linecinf As New lexer.targetinf
        linecinf.lstart = clinecodestruc.ist
        linecinf.line = clinecodestruc.line
        linecinf.length = clinecodestruc.ile
        linecinf.lend = clinecodestruc.ien
        Return linecinf
    End Function

    Friend Shared Function get_line_code_struct(linecinf As lexer.targetinf, value As String, tokenid As tokenhared.token) As xmlunpkd.linecodestruc
        Dim clinecodestruc As New xmlunpkd.linecodestruc
        clinecodestruc.ist = linecinf.lstart
        clinecodestruc.line = linecinf.line
        clinecodestruc.ile = linecinf.length
        clinecodestruc.ien = linecinf.lend
        clinecodestruc.tokenid = tokenid
        clinecodestruc.value = value
        Return clinecodestruc
    End Function
    Friend Shared Function get_contain_clinecodestruc(clinecodestruc() As xmlunpkd.linecodestruc, ilinc As Integer) As xmlunpkd.linecodestruc()
        Dim rclinecodestruc(clinecodestruc.Length - ilinc - 1) As xmlunpkd.linecodestruc
        For index = ilinc To clinecodestruc.Length - 1
            rclinecodestruc(index - ilinc) = clinecodestruc(index)
        Next
        Return rclinecodestruc
    End Function

    Friend Shared Function check_argument_token(tokenid As tokenhared.token) As Boolean
        For index = 0 To compdt.argumentallow.Length - 1
            If tokenid = compdt.argumentallow(index) Then
                Return True
            End If
        Next
        Return False
    End Function

    Friend Shared Function is_pointer(ilmethod As ilformat._ilmethodcollection, varname As String) As Boolean
        If IsNothing(ilmethod.parameter) = False Then
            varname = varname.ToLower
            For index = 0 To ilmethod.parameter.Length - 1
                If ilmethod.parameter(index).name.ToLower = varname Then
                    If ilmethod.parameter(index).ispointer Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            Next
        End If
        Return False
    End Function

    Friend Shared Function is_variable(ilmethod As ilformat._ilmethodcollection, varname As String, ByRef getdatatype As String) As Boolean
        varname = varname.ToLower
        If IsNothing(ilmethod.parameter) = False Then
            For index = 0 To ilmethod.parameter.Length - 1
                If ilmethod.parameter(index).name.ToLower = varname Then
                    getdatatype = ilmethod.parameter(index).datatype
                    is_common_data_type(getdatatype, getdatatype)
                    Return True
                End If
            Next
        End If

        If IsNothing(ilmethod.locallinit) = False Then
            For index = 0 To ilmethod.locallinit.Length - 1
                If ilmethod.locallinit(index).name <> conrex.NULL AndAlso ilmethod.locallinit(index).name.ToLower = varname Then
                    getdatatype = ilmethod.locallinit(index).datatype
                    Return True
                End If
            Next
        End If

        If IsNothing(ilasmgen.classdata.fields) = False Then
            For index = 0 To ilasmgen.classdata.fields.Length - 1
                If ilasmgen.classdata.fields(index).name.ToLower = varname Then
                    getdatatype = ilasmgen.classdata.fields(index).ptype
                    Return True
                End If
            Next
        End If

        Return False
    End Function

    Friend Shared Function get_identifier_gb(varname As String, cargcodestruc As xmlunpkd.linecodestruc, ByRef getfield As tknformat._pubfield) As Boolean
        If varname.Contains("::") Then
            Dim hresult As identvalid._resultidentcvaild = identvalid.get_identifier_valid(cargcodestruc)
            If hresult.identvalid = True Then
                Dim getfieldindex As Integer = -1
                If hresult.callintern = True Then
                    getfieldindex = funcdtproc.get_index_field(hresult.clident, hresult.classindex)
                Else
                    'extern fields
                End If
                If getfieldindex <> -1 Then
                    getfield = funcdtproc.get_field_info(hresult.classindex, getfieldindex)
                    Return True
                End If
            End If
        End If

        Return False
    End Function
    Friend Shared classlist As New ArrayList
    Friend Shared Sub check_class_vaild(attr As yocaattribute.yoattribute, location As String)
        Dim getclassname As String = attr._app._classname.ToLower
        If attr._app._namespace <> conrex.NULL Then
            getclassname = attr._app._namespace.ToLower & conrex.DOT & getclassname
        End If
        For index = 0 To classlist.Count - 1
            If classlist(index) = getclassname Then
                dserr.args.Add(attr._app._classname)
                dserr.new_error(conserr.errortype.CLASSVAILDERROR, -1, location, "Specify a new name for your class or put it in a new namespace.", "#[app::classname(""yourclass"")]")
            End If
        Next
        classlist.Add(getclassname)
    End Sub

    Friend Shared Sub get_dt_proc(_ilmethod As ilformat._ilmethodcollection, clinecodestruc As xmlunpkd.linecodestruc, ByRef datatype As String)
        Select Case clinecodestruc.tokenid
            Case tokenhared.token.IDENTIFIER
                get_datatype(_ilmethod, clinecodestruc, datatype)
            Case tokenhared.token.TYPE_CO_STR
                datatype = "string"
            Case tokenhared.token.TYPE_DU_STR
                datatype = "string"
            Case tokenhared.token.TYPE_BOOL
                datatype = "bool"
            Case tokenhared.token.TRUE
                datatype = "bool"
            Case tokenhared.token.FALSE
                datatype = "bool"
            Case tokenhared.token.TYPE_INT
                datatype = "int32"
            Case tokenhared.token.TYPE_FLOAT
                datatype = "float32"
            Case Else

        End Select
    End Sub

    Friend Shared Sub get_datatype(_ilmethod As ilformat._ilmethodcollection, clinecodestruc As xmlunpkd.linecodestruc, ByRef getdatatype As String)
        servinterface.is_variable(_ilmethod, clinecodestruc.value, getdatatype)
        servinterface.is_common_data_type(getdatatype, getdatatype)
    End Sub
End Class
