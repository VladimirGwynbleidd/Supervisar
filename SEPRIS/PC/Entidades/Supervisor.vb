Public Class Supervisor
    Public Shared Function ObtenerVigentesPorSubproceso(Subproceso As Integer, Area As Integer) As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim FiltroArea As String
        Dim Data As DataSet

        If Area = 0 Then
            FiltroArea = "1=1"
        Else
            FiltroArea = "I_ID_AREA = " + Area.ToString()
        End If

        If Subproceso = -99 Then
            Data = conexion.ConsultarDS("SELECT  DISTINCT S.T_ID_USUARIO, ISNULL(T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX, S.T_ID_USUARIO) NOMBRE FROM [dbo].[BDS_C_GR_SUPERVISOR] S LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON S.T_ID_USUARIO = U.T_ID_USUARIO WHERE B_FLAG_VIGENTE = 1 AND S.T_ID_USUARIO IN (SELECT T_ID_USUARIO FROM [dbo].[BDS_R_GR_USUARIO_PERFIL] WHERE N_FLAG_VIG = 1 AND " + FiltroArea + ")")
        Else
            Data = conexion.ConsultarDS("SELECT  DISTINCT S.T_ID_USUARIO, ISNULL(T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX, S.T_ID_USUARIO) NOMBRE FROM [dbo].[BDS_C_GR_SUPERVISOR] S LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON S.T_ID_USUARIO = U.T_ID_USUARIO WHERE B_FLAG_VIGENTE = 1 AND I_ID_SUBPROCESO = " & Subproceso)
        End If


        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerPorFolio(Folio As String) As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataSet

        data = conexion.ConsultarDS("SELECT	R.T_ID_USUARIO,	ISNULL(U.T_DSC_NOMBRE + ' ' + U.T_DSC_APELLIDO + ' ' + U.T_DSC_APELLIDO_AUX,'')	NOMBRE FROM	[dbo].[BDS_R_PC_SUPERVISOR_PC] R LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON R.T_ID_USUARIO = U.T_ID_USUARIO WHERE R.N_ID_FOLIO=" + Folio)
        conexion.CerrarConexion()
        Return data
    End Function

End Class
