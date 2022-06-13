Public Class Sesion
    Private Property Tabla As String = "BDS_D_RG_SESION"

    Public Sub New()

    End Sub

    Public Function ObtenerRango() As DataView
        Dim conexion As New Conexion.SQLServer

        Try
            Dim dvConsulta As DataView
            Dim campos As New System.Text.StringBuilder
            campos.Append("T_ID_SESION,")
            campos.Append("F_FECH_INI,")
            campos.Append("LEFT(CONVERT(VARCHAR, F_FECH_INI, 120), 10) AS FECHA,")
            campos.Append("T_ID_USUARIO,")
            campos.Append("T_DSC_DIRECCION_IP,")
            campos.Append("N_FLAG_ACTIVO,")
            campos.Append("LEFT(CONVERT(TIME, F_FECH_INI, 100), 8) + ' hrs' AS HORAINICIO,")
            campos.Append("LEFT(CONVERT(TIME, F_FECH_FIN, 100), 8) + ' hrs' AS HORAFIN")

            dvConsulta = conexion.ConsultarDT("SELECT " + campos.ToString() + " FROM " + Tabla + " ORDER BY F_FECH_INI DESC").DefaultView

            Return dvConsulta

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

End Class
