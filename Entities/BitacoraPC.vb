Public Class BitacoraPC
    Public Shared Function ObtenerEntradas(Query As String) As DataTable
        Dim conexion As New Conexion.SQLServer
        Try
            Return conexion.ConsultarDT("SELECT * FROM [dbo].[BDS_C_PC_BITACORA] " + Query)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
    Public Shared Function ObtenerUsuarios(Folio As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer
        Try
            Return conexion.ConsultarDT("SELECT DISTINCT(T_DSC_USUARIO) FROM [dbo].[BDS_C_PC_BITACORA] WHERE N_ID_FOLIO=" + Folio.ToString() + " ORDER BY T_DSC_USUARIO")
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
    Public Shared Function ObtenerPasos(Folio As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer
        Try
            Return conexion.ConsultarDT("SELECT DISTINCT(T_DSC_PASO) FROM [dbo].[BDS_C_PC_BITACORA] WHERE N_ID_FOLIO=" + Folio.ToString() + " ORDER BY T_DSC_PASO")
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
    Public Shared Function ObtenerAccion(Folio As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer
        Try
            Return conexion.ConsultarDT("SELECT DISTINCT(T_DSC_ACCION) FROM [dbo].[BDS_C_PC_BITACORA] WHERE N_ID_FOLIO=" + Folio.ToString() + " ORDER BY T_DSC_ACCION")
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function

    Public Shared Sub AgregarEntrada(Folio As Integer, Usuario As String, Paso As String, Accion As String, Optional Comentarios As String = Nothing)
        Dim conexion As New Conexion.SQLServer
        Dim query As String = "INSERT INTO [dbo].[BDS_C_PC_BITACORA] ([N_ID_FOLIO],[F_FECH_REGISTRO],[T_DSC_USUARIO],[T_DSC_PASO],[T_DSC_ACCION], [T_DSC_COMENTARIOS]) " &
            "VALUES (" & Folio & ",GETDATE(),'" & Usuario & "','" & Paso & "','" & Accion & "', '" & Comentarios & "') SELECT 0 AS ExisteError"
        Try
            conexion.ConsultarDT(query)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Sub
End Class
