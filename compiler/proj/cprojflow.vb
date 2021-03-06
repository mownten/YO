﻿Imports System.IO
Imports YODA

Public Class cprojflow

    Private yodagen As YODA_Format
    Private cproj As cprojdt
    Public Sub New()
        yodagen = New YODA_Format
    End Sub

    Public Sub start_project_flow()
        check_prerequisites()
    End Sub

    Private Sub check_prerequisites()
        procresult.rp_init("Preparation & analysis of 'Labra.yoda'")
        If File.Exists(conrex.ENVCURDIR & "\labra.yoda") = False Then
            'set error
        End If
        load_cproj_data()
        procresult.rs_set_result(True)
        procresult.rp_init("Check the path of project files")
        If Directory.Exists(conrex.ENVCURDIR & cproj.get_val("sourcepath")) = False Then
            dserr.args.Add("\src")
            dserr.new_error(conserr.errortype.PROJECTSTRUCTERROR, -1, Nothing)
        End If

        If Directory.Exists(conrex.ENVCURDIR & cproj.get_val("assetspath")) = False Then
            dserr.args.Add("\assets")
            dserr.new_error(conserr.errortype.PROJECTSTRUCTERROR, -1, Nothing)
        End If

        procresult.rs_set_result(True)

        libreg.init_libraries(conrex.ENVCURDIR & cproj.get_val("assetspath"))

        impfiles.import_directory(conrex.ENVCURDIR & cproj.get_val("sourcepath"))
    End Sub

    Public Sub load_cproj_data()
        Dim getlabrasetting As String = File.ReadAllText(conrex.ENVCURDIR & "\labra.yoda")
        Dim labradt As YODA_Format.YODAMapFormat = yodagen.ReadYODA_Map(getlabrasetting)
        cproj = New cprojdt(labradt)
        cproj.conv_to_mapstore()
    End Sub
End Class
