Public Class Estatus
    Public Shared Function ObtenerTodos() As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        data = conexion.ConsultarDT("SELECT * FROM [dbo].[BDS_C_PC_ESTATUS]")
        conexion.CerrarConexion()
        Return data
    End Function
End Class
