Public Class Inspector
    Public Shared Function ObtenerVigentesPorSubproceso(Subproceso As Integer, Area As Integer) As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim FiltroArea As String
        Dim data As DataSet

        If Area = 0 Then
            FiltroArea = "1=1"
        Else
            FiltroArea = "I_ID_AREA = " + Area.ToString()
        End If

        If Subproceso = -99 Then
            data = conexion.ConsultarDS("SELECT distinct  I.T_ID_USUARIO, ISNULL(T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX, I.T_ID_USUARIO) NOMBRE FROM [dbo].[BDS_C_GR_INSPECTOR] I LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON I.T_ID_USUARIO = U.T_ID_USUARIO WHERE B_FLAG_VIGENTE = 1 AND U.T_ID_USUARIO IN (SELECT T_ID_USUARIO FROM [dbo].[BDS_R_GR_USUARIO_PERFIL] WHERE N_FLAG_VIG = 1 AND " + FiltroArea + ") ORDER by NOMBRE ")
        Else
            data = conexion.ConsultarDS("SELECT distinct I.T_ID_USUARIO, ISNULL(T_DSC_NOMBRE + ' ' + T_DSC_APELLIDO + ' ' + T_DSC_APELLIDO_AUX, I.T_ID_USUARIO) NOMBRE FROM [dbo].[BDS_C_GR_INSPECTOR] I LEFT JOIN [dbo].[BDS_C_GR_USUARIO] U ON I.T_ID_USUARIO = U.T_ID_USUARIO WHERE B_FLAG_VIGENTE = 1 AND I_ID_SUBPROCESO = " & Subproceso & " ORDER by NOMBRE")
        End If


        conexion.CerrarConexion()


        Return data

    End Function

    Public Shared Function ObtenerVigentesPorFolio(Folio As Integer) As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataSet

        data = conexion.ConsultarDS("SELECT U.T_ID_USUARIO, ISNULL(I.T_DSC_NOMBRE + ' ' + I.T_DSC_APELLIDO + ' ' + I.T_DSC_APELLIDO_AUX, U.T_ID_USUARIO) NOMBRE FROM [dbo].[BDS_R_PC_INSPECTOR_PC] U LEFT JOIN [dbo].[BDS_C_GR_USUARIO] I  ON I.T_ID_USUARIO = U.T_ID_USUARIO WHERE U.N_ID_FOLIO =" & Folio & " ORDER by NOMBRE")
        conexion.CerrarConexion()

        Return data

    End Function


End Class
