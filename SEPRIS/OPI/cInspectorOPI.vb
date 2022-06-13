Public Class cInspectorOPI
    Public Shared Function ObtenerVigentesPorProceso(Optional _id_Proceso As Integer = -99, Optional _id_subproceso As Integer = -99) As DataSet
        ''Public Shared Function ObtenerVigentesPorProceso(iProceso As Integer, iSubproceso As Integer) As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataSet


        If _id_Proceso = -99 Then
            data = conexion.ConsultarDS(" SELECT DISTINCT I.T_ID_USUARIO, CASE ISNULL(LEN(LTRIM(RTRIM(T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX))),0)" &
                                         " WHEN 0 THEN I.T_ID_USUARIO ELSE T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX END NOMBRE " &
                                         " FROM [dbo].[BDS_C_GR_INSPECTOR] I LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON I.T_ID_USUARIO = U.T_ID_USUARIO " &
                                         " WHERE B_FLAG_VIGENTE = 1")
            'data = conexion.ConsultarDS("SELECT I.T_ID_USUARIO, ISNULL(T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX, I.T_ID_USUARIO) NOMBRE FROM [dbo].[BDS_C_GR_INSPECTOR] I LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON I.T_ID_USUARIO = U.T_ID_USUARIO WHERE B_FLAG_VIGENTE = 1")
        Else
            data = conexion.ConsultarDS(" SELECT DISTINCT I.T_ID_USUARIO, CASE ISNULL(LEN(LTRIM(RTRIM(T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX))),0)" &
                                         " WHEN 0 THEN I.T_ID_USUARIO ELSE T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX END NOMBRE " &
                                         " FROM [dbo].[BDS_C_GR_INSPECTOR] I LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON I.T_ID_USUARIO = U.T_ID_USUARIO " &
                                         " WHERE B_FLAG_VIGENTE = 1 AND I.I_ID_PROCESO = " & _id_Proceso & "AND I.I_ID_SUBPROCESO = " & _id_subproceso)
            'data = conexion.ConsultarDS("SELECT distinct I.T_ID_USUARIO, ISNULL(T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX, I.T_ID_USUARIO) NOMBRE FROM [dbo].[BDS_C_GR_INSPECTOR] I LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON I.T_ID_USUARIO = U.T_ID_USUARIO WHERE B_FLAG_VIGENTE = 1 " &
            '   "AND I_ID_SUBPROCESO In (Select SP.I_ID_SUBPROCESO From BDS_C_GR_SUBPROCESO SP WHERE SP.I_ID_PROCESO  =  " & iProceso & ")")
        End If

        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerInspectores(usuario As Entities.Usuario) As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataSet
        Dim strQuery As String

        strQuery = " SELECT DISTINCT I.T_ID_USUARIO AS 'T_ID_INSPECTOR_ASIGNADO', CASE ISNULL(LEN(LTRIM(RTRIM(T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX))),0)" &
                                         " WHEN 0 THEN I.T_ID_USUARIO ELSE T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX END NOMBRE " &
                                         " FROM [dbo].[BDS_C_GR_INSPECTOR] I LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON I.T_ID_USUARIO = U.T_ID_USUARIO "

        ' If usuario.IdentificadorPerfilActual <> Constantes.PERFIL_ADM Then
        If usuario.IdArea = 35 Or usuario.IdArea = 36 Then
            strQuery = strQuery & " Inner Join [dbo].[BDS_R_GR_USUARIO_PERFIL] USP ON USP.T_ID_USUARIO = U.T_ID_USUARIO "

        End If
        'End If

        strQuery = strQuery & " WHERE B_FLAG_VIGENTE = 1"

        'If usuario.IdentificadorPerfilActual <> Constantes.PERFIL_ADM Then
        If usuario.IdArea = 35 Or usuario.IdArea = 36 Then
            strQuery = strQuery & " And USP.I_ID_AREA = " & usuario.IdArea

        End If
        'End If

        data = conexion.ConsultarDS(strQuery)


        conexion.CerrarConexion()

        Return data

    End Function
End Class
