Public Class BitacoraOPI
    Public Shared Function ObtenerEntradas(Query As String) As DataTable
        Dim conexion As New Conexion.SQLServer
        Try
            Return conexion.ConsultarDT("SELECT * FROM [dbo].[BDS_D_OPI_BITACORA ] " + Query)
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
            Return conexion.ConsultarDT("SELECT DISTINCT(T_DSC_USUARIO) FROM [dbo].[BDS_D_OPI_BITACORA] WHERE N_ID_FOLIO=" + Folio.ToString() + " ORDER BY T_DSC_USUARIO")
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
            Return conexion.ConsultarDT("SELECT DISTINCT(T_DSC_PASO) FROM [dbo].[BDS_D_OPI_BITACORA ] WHERE N_ID_FOLIO=" + Folio.ToString() + " ORDER BY T_DSC_PASO")
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
            Return conexion.ConsultarDT("SELECT DISTINCT(T_DSC_ACCION) FROM [dbo].[BDS_D_OPI_BITACORA ] WHERE N_ID_FOLIO=" + Folio.ToString() + " ORDER BY T_DSC_ACCION")
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
    Public Shared Function ObtenerDCSPaso(Paso As String) As String

        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT [T_DSC_PASO] FROM [dbo].[BDS_C_OPI_PASOS] WHERE  N_ID_PASO =  " & Paso & " AND B_FLAG_VIGENTE = 1")

        conexion.CerrarConexion()

        Dim rows As String = ""
        If data.Rows.Count > 0 Then
            rows = data.Rows(0).ItemArray(0).ToString()
        End If

        Return rows.ToString()
    End Function
    Public Shared Sub AgregarEntrada(Folio As Integer, Usuario As String, Paso As String, Accion As String, Optional Comentarios As String = Nothing)
        Dim conexion As New Conexion.SQLServer
        Dim query As String = "INSERT INTO [dbo].[BDS_D_OPI_BITACORA ] ([N_ID_FOLIO],[F_FECH_REGISTRO],[T_DSC_USUARIO],[T_DSC_PASO],[T_DSC_ACCION], [T_DSC_COMENTARIOS]) " &
            "VALUES (" & Folio & ",GETDATE(),'" & Usuario & "','" & Paso & "','" & Accion & "','" & Comentarios & "') SELECT 0 AS ExisteError"
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
