Public Class HistorialSolicitud
    Private Tabla As String = "BDS_D_GR_HISTORIAL_SOLICITUD"
#Region "Propiedades"
    Public Property Identificador As Integer
    Public Property IdSolicitudConsecutivo As Integer
    Public Property IdSolicitud As Integer
    Public Property FechaRegistro As DateTime
    Public Property IdUsuario As String
    Public Property DescAccion As String
    Public Property FechaVencimiento As DateTime? = Nothing
    Public Property Comentarios As String
#End Region
#Region "Constructores"

    Public Sub New()

    End Sub

#End Region

    Public Function ObtenerHistorial() As DataTable
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataTable
            Dim query As String = "SELECT N_ID_HISTORIAL, N_ID_SOLICITUD_CONSECUTIVO, N_ID_SOLICITUD_SERVICIO, F_FECH_REGISTRO, HS.T_ID_USUARIO, (US.T_DSC_NOMBRE + ' '+ US.T_DSC_APELLIDO + ' ' + US.T_DSC_APELLIDO_AUX) AS USUARIO ,T_DSC_ACCION, F_FECH_VENCIMIENTO " & _
                                    "FROM BDS_D_GR_HISTORIAL_SOLICITUD HS INNER JOIN BDS_C_GR_USUARIO US ON US.T_ID_USUARIO = HS.T_ID_USUARIO WHERE N_ID_SOLICITUD_SERVICIO = {0} ORDER BY N_ID_SOLICITUD_CONSECUTIVO DESC"
            query = String.Format(query, Me.IdSolicitud)
            dv = conexion.ConsultarDT(query)
            Return dv
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function

    Public Function ObtenerHistorialComentarios() As DataTable
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataTable
            Dim query As String = "SELECT N_ID_HISTORIAL, N_ID_SOLICITUD_CONSECUTIVO, N_ID_SOLICITUD_SERVICIO, F_FECH_REGISTRO, HS.T_ID_USUARIO, (US.T_DSC_NOMBRE + ' '+ US.T_DSC_APELLIDO + ' ' + US.T_DSC_APELLIDO_AUX) AS USUARIO ,T_DSC_ACCION, F_FECH_VENCIMIENTO, HS.T_DSC_COMENTARIO " & _
                                    "FROM BDS_D_GR_HISTORIAL_SOLICITUD HS INNER JOIN BDS_C_GR_USUARIO US ON US.T_ID_USUARIO = HS.T_ID_USUARIO WHERE N_ID_SOLICITUD_SERVICIO = {0}  AND HS.T_DSC_COMENTARIO IS NOT NULL ORDER BY N_ID_SOLICITUD_CONSECUTIVO DESC "
            query = String.Format(query, Me.IdSolicitud)
            dv = conexion.ConsultarDT(query)
            Return dv
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function

    ''' <summary>
    ''' Obtiene el siguiente identificador del catalogo
    ''' </summary>
    ''' <returns>Identificador siguiente</returns>
    ''' <remarks></remarks>
    Public Function ObtenerSiguienteIdentificador() As Integer

        Dim resultado As Integer = 1

        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_HISTORIAL) + 1) N_ID_HISTORIAL FROM " + Tabla)

            If dr.Read() Then

                If IsDBNull(dr("N_ID_HISTORIAL")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_HISTORIAL"))
                End If

            End If

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    ''' <summary>
    ''' Obtiene el siguiente consecutivo por solicitud
    ''' </summary>
    ''' <returns>Identificador siguiente</returns>
    ''' <remarks></remarks>
    Public Function ObtenerSiguienteConsecutivo(ByVal idSol As String) As Integer

        Dim resultado As Integer = 1

        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_SOLICITUD_CONSECUTIVO) + 1) N_ID_SOLICITUD_CONSECUTIVO FROM " + Tabla + " WHERE N_ID_SOLICITUD_SERVICIO = " + idSol)

            If dr.Read() Then

                If IsDBNull(dr("N_ID_SOLICITUD_CONSECUTIVO")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_SOLICITUD_CONSECUTIVO"))
                End If

            End If

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    ''' <summary>
    ''' Agregar el registro historico
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim objSolicitud As New Solicitud(Me.IdSolicitud)
        Dim folio As String = If(objSolicitud.IdFolio > 0 AndAlso objSolicitud.FolioAnio > 0, objSolicitud.IdFolio.ToString & "-" & objSolicitud.FolioAnio.ToString, objSolicitud.FolioUsario)

        Dim bitacora As New Conexion.Bitacora("Registro en el historial de la solicitud " & folio, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_HISTORIAL") : listValores.Add(Me.Identificador)
            listCampos.Add("N_ID_SOLICITUD_CONSECUTIVO") : listValores.Add(Me.IdSolicitudConsecutivo)
            listCampos.Add("N_ID_SOLICITUD_SERVICIO") : listValores.Add(Me.IdSolicitud)
            listCampos.Add("F_FECH_REGISTRO") : listValores.Add(Me.FechaRegistro)
            listCampos.Add("T_ID_USUARIO") : listValores.Add(Me.IdUsuario)
            listCampos.Add("T_DSC_ACCION") : listValores.Add(Me.DescAccion)
            If Me.FechaVencimiento.HasValue Then
                listCampos.Add("F_FECH_VENCIMIENTO") : listValores.Add(Me.FechaVencimiento)
            End If
            If Not String.IsNullOrEmpty(Me.Comentarios) Then
                listCampos.Add("T_DSC_COMENTARIO") : listValores.Add(Me.Comentarios)
            End If
            
            Try
                resultado = conexion.Insertar(Tabla, listCampos, listValores)
                bitacora.Insertar(Tabla, listCampos, listValores, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Insertar(Tabla, listCampos, listValores, resultado, ex.Message.ToString)
                Throw ex
            End Try

        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally

            bitacora.Finalizar(resultado)

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return resultado

    End Function
End Class
