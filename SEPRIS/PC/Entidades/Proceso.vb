Public Class Proceso


    Public Shared Function ObtenerPcVigentes(Area As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        'Area = 35
        data = conexion.ConsultarDT("SELECT * FROM [dbo].[BDS_C_GR_PROCESO] WHERE T_DSC_FLUJO = 'PC' AND B_FLAG_VIGENTE = 1 AND I_ID_AREA =" + Area.ToString())

        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerOpiVigentes(Area As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT * FROM [dbo].[BDS_C_GR_PROCESO] WHERE T_DSC_FLUJO = 'OPI' AND B_FLAG_VIGENTE = 1 AND I_ID_AREA =" + Area.ToString())

        conexion.CerrarConexion()

        Return data

    End Function


End Class
