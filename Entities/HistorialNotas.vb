Public Class HistorialNotas
    Private Tabla As String = "BDS_D_GR_HISTORICO_NOTAS"
#Region "Propiedades"
    Public Property Identificador As Integer
    Public Property IdSolicitud As Integer
    Public Property NivelServicio As Integer
    Public Property FechaNota As DateTime
    Public Property DescNota As String
    Public Property IdUsuario As String
    Public Property DescEstatusServ As String
#End Region
#Region "Constructores"

    Public Sub New()

    End Sub

#End Region

    Public Function ObtenerHistorial() As DataTable
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataTable
            Dim query As String = "SELECT N_ID_HISTORICO, N_ID_SOLICITUD_SERVICIO, N_ID_NIVELES_SERVICIO, F_FECHA_NOTA, T_DSC_NOTA, T_ID_USUARIO, T_DSC_ESTATUS_SERVICIO " & _
                                    "FROM dbo.BDS_D_GR_HISTORICO_NOTAS WHERE N_ID_SOLICITUD_SERVICIO = {0} AND N_ID_NIVELES_SERVICIO = {1} ORDER BY F_FECHA_NOTA DESC"
            query = String.Format(query, Me.IdSolicitud, Me.NivelServicio)
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

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_HISTORICO) + 1) N_ID_HISTORICO FROM " + Tabla)

            If dr.Read() Then

                If IsDBNull(dr("N_ID_HISTORICO")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_HISTORICO"))
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
        Dim bitacora As New Conexion.Bitacora("Registro de nota en la solicitud " & folio, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_HISTORICO") : listValores.Add(Me.Identificador)
            listCampos.Add("N_ID_SOLICITUD_SERVICIO") : listValores.Add(Me.IdSolicitud)
            listCampos.Add("N_ID_NIVELES_SERVICIO") : listValores.Add(Me.NivelServicio)
            listCampos.Add("F_FECHA_NOTA") : listValores.Add(Me.FechaNota)
            listCampos.Add("T_DSC_NOTA") : listValores.Add(Me.DescNota)
            listCampos.Add("T_ID_USUARIO") : listValores.Add(Me.IdUsuario)
            listCampos.Add("T_DSC_ESTATUS_SERVICIO") : listValores.Add(Me.DescEstatusServ)

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
