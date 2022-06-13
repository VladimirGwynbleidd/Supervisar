Public Class Subproceso

    Public Shared Function ObtenerVigentesPorProceso(Proceso As Integer) As DataSet
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataSet

        If Proceso = -99 Then
            data = conexion.ConsultarDS("SELECT I_ID_SUBPROCESO, T_DSC_DESCRIPCION FROM [dbo].[BDS_C_GR_SUBPROCESO] WHERE B_FLAG_VIGENTE = 1")
        Else
            data = conexion.ConsultarDS("SELECT I_ID_SUBPROCESO, T_DSC_DESCRIPCION FROM [dbo].[BDS_C_GR_SUBPROCESO] WHERE B_FLAG_VIGENTE = 1 AND I_ID_PROCESO = " & Proceso)
        End If

        conexion.CerrarConexion()

        Return data

    End Function

End Class
