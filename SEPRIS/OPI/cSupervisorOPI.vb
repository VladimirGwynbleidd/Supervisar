Public Class cSupervisorOPI
    Public Shared Function ObtenerVigentesPorSubproceso(Optional _id_Proceso As Integer = -99, Optional _id_subproceso As Integer = -99) As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataSet


        If _id_subproceso = -99 OrElse _id_Proceso = -99 Then
            data = conexion.ConsultarDS(" SELECT DISTINCT S.T_ID_USUARIO, CASE ISNULL(LEN(LTRIM(RTRIM(T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX))),0)" &
                                         " WHEN 0 THEN S.T_ID_USUARIO ELSE T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX END NOMBRE " &
                                         " FROM [dbo].[BDS_C_GR_SUPERVISOR] S LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON S.T_ID_USUARIO = U.T_ID_USUARIO " &
                                         " WHERE B_FLAG_VIGENTE = 1")
        Else
            data = conexion.ConsultarDS(" SELECT DISTINCT S.T_ID_USUARIO, CASE ISNULL(LEN(LTRIM(RTRIM(T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX))),0)" &
                                         " WHEN 0 THEN S.T_ID_USUARIO ELSE T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX END NOMBRE " &
                                         " FROM [dbo].[BDS_C_GR_SUPERVISOR] S LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON S.T_ID_USUARIO = U.T_ID_USUARIO " &
                                         " WHERE B_FLAG_VIGENTE = 1 AND I_ID_PROCESO = " & _id_Proceso & " AND I_ID_SUBPROCESO = " & _id_subproceso)
        End If

        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerPorFolio(Folio As String) As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataSet

        data = conexion.ConsultarDS("SELECT	R.T_ID_USUARIO,	ISNULL(U.T_DSC_NOMBRE + ' ' + U.T_DSC_APELLIDO + ' ' + U.T_DSC_APELLIDO_AUX,'')	NOMBRE FROM	[dbo].[BDS_R_PC_SUPERVISOR_PC] R LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON R.T_ID_USUARIO = U.T_ID_USUARIO WHERE R.N_ID_FOLIO=" + Folio)
        Return data
    End Function
End Class
