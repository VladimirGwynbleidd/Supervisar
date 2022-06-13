<Serializable()>
Public Class Solicitud
    Private TblEstatusServicio As String = "BDS_C_GR_ESTATUS_SERVICIO"
    Private TblSolicitudServicio_D As String = "BDS_D_GR_SOLICITUD_SERVICIO"
    Private TblSolicitudServicio_R As String = "BDS_R_GR_SOLICITUD_SERVICIO"
    Private TblSolicitudServicioDoc As String = "BDS_D_GR_SOLICITUD_SERVICIO_DOCUMENTOS"
#Region "Propiedades"

    'TABLA DE SOLICITUDES
    Public Property Identificador As Integer
    Public Property FolioAnio As Integer = -1
    Public Property IdFolio As Integer = -1
    Public Property UsuarioSolicitante As String
    Public Property FolioUsario As Integer
    Public Property UsuarioSolicitud As String
    Public Property Extension As Integer?
    Public Property Autorizador As String
    Public Property IdEstatus As Integer
    Public Property Notas As String
    Public Property FechRegistro As Date
    Public Property FechModificacion As Date
    Public Property FechAprobacion As Date? = Nothing
    Public Property FechTermino As Date? = Nothing
    Public Property FechAtendioIngeniero As Date? = Nothing
    Public Property FechTentativa As Date? = Nothing
    Public Property FechCierre As Date? = Nothing
    Public Property FechRechazo As Date? = Nothing
    Public Property FechRecepcion As Date? = Nothing
    Public Property Existe As Boolean
    Public Property CausaRechazo As String
    Public Property AreaSolicitante As String
    Public Property SelloSolicitud As String
    Public Property SelloAcuse As String

    'TABLA DE SERVICIOS
    Public Property NivelServicio As Integer
    Public Property EstatusServicio As Integer = -1
    Public Property FechaLimite As Date? = Nothing
    Public Property FechaTerminoServ As Date? = Nothing
    Public Property FechaRechazoIng As Date? = Nothing
    Public Property FechaRechazoSolic As Date? = Nothing
    Public Property TiempoEjecucion As Integer = 0


    'TABLA DE DOCUMENTOS
    Public Property NivelServicioDoc As Integer
    Public Property IdDocumento As Integer
    Public Property NombreDocumento As String


    'GENERAL
    Public Property IdFlujo As Integer

#End Region
#Region "Constructores"

    Public Sub New()

    End Sub
    Public Sub New(ByVal idSol As Integer)
        Me.Identificador = idSol
        CargarDatosSolicitud()
    End Sub

    Public Sub New(ByVal folioSol As String)
        Me.Identificador = ObtenerIDSolicitud(folioSol)
        CargarDatosSolicitud()
    End Sub

#End Region

#Region "Metodos"

    Public Function ObtenerIDSolicitud(ByVal folio As String) As Integer
        Dim conexion As New Conexion.SQLServer
        Try
            Dim datosSol As String() = folio.Split(New Char() {"-"c})
            Dim dv As New DataTable
            Dim query As String = "SELECT N_ID_SOLICITUD_SERVICIO FROM BDS_D_GR_SOLICITUD_SERVICIO WHERE N_NUM_FOLIO_ID = {0} AND N_NUM_FOLIO_ANIO = {1}"
            query = String.Format(query, datosSol(0), datosSol(1))
            dv = conexion.ConsultarDT(query)
            If dv.Rows.Count > 0 Then
                For Each dr As DataRow In dv.Rows
                    Return dr("N_ID_SOLICITUD_SERVICIO")
                Next
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return 0
    End Function

    Public Shared Function ObtenerAreasSolicitantes() As DataTable
        Dim dt As New DataTable
        Dim conexion As New Conexion.SQLServer
        Try
            Dim query As String = "SELECT DISTINCT T_DSC_AREA_SOLICITANTE FROM BDS_D_GR_SOLICITUD_SERVICIO WHERE N_ID_ESTATUS_SERVICIO NOT IN (1, 10)"
            dt = conexion.ConsultarDT(query)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return dt
    End Function

    Public Shared Function ObtenerSolicitudesPorIngeniero(ByVal Ingeniero As String) As String
        Dim lstSolicitudes As String = ""
        Dim conexion As New Conexion.SQLServer
        Try
            Dim query As String = "SELECT DISTINCT N_ID_SOLICITUD_SERVICIO FROM BDS_D_TI_AGENDA WHERE (T_ID_INGENIERO = '{0}') AND (N_ID_SOLICITUD_SERVICIO IS NOT NULL)"
            query = String.Format(query, Ingeniero)
            Dim dt As DataTable = conexion.ConsultarDT(query)
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    lstSolicitudes += (CStr(dt.Rows(i)("N_ID_SOLICITUD_SERVICIO"))) & If(i < dt.Rows.Count - 1, ", ", "")
                Next
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return lstSolicitudes
    End Function

    Public Shared Function ObtenerSolicitudesPorEstatusServicio(ByVal idEstatus As String) As String
        Dim lstSolicitudes As String = ""
        Dim conexion As New Conexion.SQLServer
        Try
            Dim query As String = "SELECT DISTINCT N_ID_SOLICITUD_SERVICIO FROM BDS_R_GR_SOLICITUD_SERVICIO WHERE (N_ID_ESTATUS_SERVICIO = '{0}') AND (N_ID_SOLICITUD_SERVICIO IS NOT NULL)"
            query = String.Format(query, idEstatus)
            Dim dt As DataTable = conexion.ConsultarDT(query)
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    lstSolicitudes += (CStr(dt.Rows(i)("N_ID_SOLICITUD_SERVICIO"))) & If(i < dt.Rows.Count - 1, ", ", "")
                Next
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return lstSolicitudes
    End Function

    ''' <summary>
    ''' Regresa un objeto de tipo solicitud en base al sello digital proporcionado
    ''' </summary>
    ''' <param name="Sello">
    ''' Sello digital de la solicitud o sello digital del acuse
    ''' </param>
    ''' <returns>
    ''' Objeto de tipo Solicitud
    ''' </returns>
    ''' <remarks></remarks>
    Public Function ObtenerSolicitudSelloDigital(ByVal Sello As String) As Solicitud
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataTable
            Dim query As String = "SELECT N_ID_SOLICITUD_SERVICIO FROM BDS_D_GR_SOLICITUD_SERVICIO WHERE T_DSC_SELLO_DIGITAL_SOLICITUD = '{0}' OR T_DSC_SELLO_DIGITAL_ACUSE = '{0}'"
            query = String.Format(query, Sello)
            dv = conexion.ConsultarDT(query)
            If dv.Rows.Count > 0 Then
                For Each dr As DataRow In dv.Rows
                    Return New Solicitud(CInt(dr("N_ID_SOLICITUD_SERVICIO")))
                Next
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Carga las solicitudes segun el perfil del usuario consultante
    ''' </summary>
    ''' <remarks></remarks>
    Public Function CargarDatosPerfil(ByVal idPerfil As Integer, ByVal usuario As String) As DataView
        Dim dv As New DataView
        Dim conexion As New Conexion.SQLServer
        Dim sql As String = String.Empty

        Try
            sql = "SELECT (convert(varchar(4),A.N_NUM_FOLIO_ID) + '-' + convert(varchar(6),A.N_NUM_FOLIO_ANIO)) AS FOLIO, A.N_ID_SOLICITUD_SERVICIO, A.F_FECH_REGISTRO_SOLICITUD, A.F_FECH_ULTIMA_MODIFICACION, " & _
                  "A.F_FECH_REGISTRO_SOLICITUD AS F_FECH_ENVIO_AUTORIZACION, A.F_FECH_APROBACION_SOLICITUD, A.F_FECH_TERMINO_SOLICITUD,  " & _
                  "A.F_FECH_RECHAZO, A.T_DSC_AREA_SOLICITANTE, (SELECT U.T_DSC_NOMBRE + ' ' + U.T_DSC_APELLIDO + ' ' + U.T_DSC_APELLIDO_AUX FROM BDS_C_GR_USUARIO U WHERE U.T_ID_USUARIO = A.T_ID_SOLICITANTE) AS T_ID_SOLICITANTE, A.N_NUM_EXTENSION, " & _
                  "(SELECT U.T_DSC_NOMBRE + ' ' + U.T_DSC_APELLIDO + ' ' + U.T_DSC_APELLIDO_AUX FROM BDS_C_GR_USUARIO U WHERE U.T_ID_USUARIO = A.T_ID_AUTORIZADOR) AS T_ID_AUTORIZADOR, (SELECT COUNT(*) FROM BDS_R_GR_SOLICITUD_SERVICIO B WHERE B.N_ID_SOLICITUD_SERVICIO = A.N_ID_SOLICITUD_SERVICIO) AS NO_SERVICIOS, " & _
                  "(SELECT COUNT(DISTINCT AGE.T_ID_INGENIERO) FROM BDS_D_TI_AGENDA AGE WHERE AGE.N_ID_SOLICITUD_SERVICIO = A.N_ID_SOLICITUD_SERVICIO) AS NO_INGENIEROS, " & _
                  "DATEDIFF(dd,F_FECH_APROBACION_SOLICITUD,F_FECH_TERMINO_SOLICITUD) AS TIEMPO_RESPUESTA, A.N_ID_ESTATUS_SERVICIO, EST.T_DSC_ESTATUS_SERVICIO, ISNULL(A.T_DSC_SELLO_DIGITAL_ACUSE,'') AS T_DSC_SELLO_DIGITAL_ACUSE " & _
                  "FROM BDS_D_GR_SOLICITUD_SERVICIO A " & _
                  "INNER JOIN BDS_C_GR_ESTATUS_SERVICIO EST ON EST.N_ID_ESTATUS_SERVICIO = A.N_ID_ESTATUS_SERVICIO "

            'Administrador SISTIC con idPerfil 1 y Subdirector con idPerfil 7 PUEDEN VER TODO
            Select Case idPerfil
                Case 1, 7
                    sql = sql + "WHERE A.N_ID_ESTATUS_SERVICIO NOT IN (1,10)"
                Case 4
                    'Solicitante
                    sql = sql + "WHERE A.T_ID_SOLICITANTE = '" & usuario & "' AND A.N_ID_ESTATUS_SERVICIO NOT IN (1,10)"
                Case 5
                    'Autorizador Solicitud TIC
                    sql = sql + "WHERE (A.T_ID_AUTORIZADOR = '" & usuario & "' AND A.N_ID_ESTATUS_SERVICIO = 11) OR ( A.T_ID_SOLICITANTE = '" & usuario & "' AND A.N_ID_ESTATUS_SERVICIO NOT IN (1,10)) "
                Case 6
                    'Ingeniero  
                    sql = sql + "WHERE (A.T_ID_SOLICITANTE = '" & usuario & "' OR A.N_ID_SOLICITUD_SERVICIO IN ( " & _
                                "SELECT DISTINCT(AG.N_ID_SOLICITUD_SERVICIO) FROM BDS_D_TI_AGENDA AG WHERE AG.T_ID_INGENIERO LIKE '" & usuario & "')) AND A.N_ID_ESTATUS_SERVICIO NOT IN (1,10)"
            End Select


            sql = sql + " ORDER BY A.N_NUM_FOLIO_ANIO DESC, A.N_NUM_FOLIO_ID DESC"
            sql = String.Format(sql, TblSolicitudServicio_R, TblSolicitudServicio_D)
            dv = conexion.ConsultarDT(sql).DefaultView

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
    ''' Carga las solicitudes creadas y que no has sido enviadas a aprobación o a las cuales se les hicieron observaciones
    ''' </summary>
    ''' <remarks></remarks>
    Public Function CargarDatos(ByVal idUsu As String) As DataTable
        Dim resultado As New DataTable
        resultado.Columns.Add("FOLIO")
        resultado.Columns.Add("N_ID_SOLICITUD_SERVICIO")
        resultado.Columns.Add("F_FECH_REGISTRO_SOLICITUD")
        resultado.Columns.Add("F_FECH_ULTIMA_MODIFICACION")
        resultado.Columns.Add("NO_SERVICIOS")
        resultado.Columns.Add("T_ID_AUTORIZADOR")
        resultado.Columns.Add("N_NUM_EXTENSION")
        resultado.Columns.Add("N_NUM_FOLIO_USUARIO")

        Dim conexion As New Conexion.SQLServer
        Dim sql As String = String.Empty

        Try
            sql = "SELECT (convert(varchar(4), A.N_NUM_FOLIO_ANIO) + '/'+ convert(varchar(8), A.N_ID_SOLICITUD_SERVICIO) ) AS FOLIO, A.N_ID_SOLICITUD_SERVICIO, A.N_NUM_FOLIO_USUARIO, A.F_FECH_REGISTRO_SOLICITUD, A.F_FECH_ULTIMA_MODIFICACION, " & _
                  "(SELECT COUNT(*) FROM {0} B WHERE B.N_ID_SOLICITUD_SERVICIO = A.N_ID_SOLICITUD_SERVICIO) AS NO_SERVICIOS, (US.T_DSC_NOMBRE + ' ' + US.T_DSC_APELLIDO + ' ' + US.T_DSC_APELLIDO_AUX) AS T_ID_AUTORIZADOR, A.N_NUM_EXTENSION " & _
                  "FROM  {1} A INNER JOIN BDS_C_GR_USUARIO US ON US.T_ID_USUARIO = A.T_ID_AUTORIZADOR " & _
                  "WHERE N_ID_ESTATUS_SERVICIO = 1 AND A.T_ID_SOLICITANTE LIKE '{2}' ORDER BY A.N_ID_SOLICITUD_SERVICIO DESC"

            sql = String.Format(sql, TblSolicitudServicio_R, TblSolicitudServicio_D, idUsu)
            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR(sql)

            While dr.Read()
                Dim row As DataRow
                row = resultado.NewRow()
                row("FOLIO") = dr("FOLIO")
                row("N_ID_SOLICITUD_SERVICIO") = dr("N_ID_SOLICITUD_SERVICIO")
                row("F_FECH_REGISTRO_SOLICITUD") = dr("F_FECH_REGISTRO_SOLICITUD")
                row("F_FECH_ULTIMA_MODIFICACION") = dr("F_FECH_ULTIMA_MODIFICACION")
                row("NO_SERVICIOS") = dr("NO_SERVICIOS")
                row("T_ID_AUTORIZADOR") = dr("T_ID_AUTORIZADOR")
                row("N_NUM_EXTENSION") = dr("N_NUM_EXTENSION")
                row("N_NUM_FOLIO_USUARIO") = dr("N_NUM_FOLIO_USUARIO")
                resultado.Rows.Add(row)
            End While


            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try
    End Function

    Public Function TieneDocumentos(ByVal idSolicitud As Integer, ByVal idServicio As Integer) As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer
        Dim query As String = "SELECT N_ID_SOLICITUD_SERVICIO, N_ID_NIVELES_SERVICIO, N_ID_DOCUMENTO FROM BDS_D_GR_SOLICITUD_SERVICIO_DOCUMENTOS WHERE (N_ID_SOLICITUD_SERVICIO = {0}) AND (N_ID_NIVELES_SERVICIO = {1})"

        Try
            query = String.Format(query, idSolicitud, idServicio)
            Dim dv As DataTable = conexion.ConsultarDT(query)
            If dv.Rows.Count > 0 Then
                resultado = True
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try

        Return resultado
    End Function

    ''' <summary>
    ''' Carga los datos de la Solicitud tomando el Identificador almacenado en la propiedad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargarDatosSolicitud()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim dr As SqlClient.SqlDataReader = Nothing

            listCampos.Add("N_ID_SOLICITUD_SERVICIO") : listValores.Add(Me.Identificador)

            Try

                Existe = conexion.BuscarUnRegistro(TblSolicitudServicio_D, listCampos, listValores)

                If Existe Then

                    dr = conexion.ConsultarRegistrosDR(TblSolicitudServicio_D, listCampos, listValores)

                    If dr.Read() Then
                        Me.FolioAnio = CInt(dr("N_NUM_FOLIO_ANIO"))
                        Me.IdFolio = CInt(dr("N_NUM_FOLIO_ID"))
                        Me.UsuarioSolicitante = CStr(dr("T_ID_SOLICITANTE"))
                        Me.FolioUsario = CInt(dr("N_NUM_FOLIO_USUARIO"))
                        Me.UsuarioSolicitud = CStr(dr("T_DSC_USUARIO_SOLICITUD"))
                        Me.Autorizador = CStr(dr("T_ID_AUTORIZADOR"))
                        Me.IdEstatus = CInt(dr("N_ID_ESTATUS_SERVICIO"))
                        Me.Extension = CInt(dr("N_NUM_EXTENSION"))
                        Me.Notas = CStr(dr("T_DSC_NOTAS"))
                        Me.AreaSolicitante = CStr(dr("T_DSC_AREA_SOLICITANTE"))
                        If Not IsDBNull(dr("T_DSC_CAUSAS_RECHAZO")) Then
                            Me.CausaRechazo = CStr(dr("T_DSC_CAUSAS_RECHAZO"))
                        End If
                        'FECHAS
                        If Not IsDBNull(dr("F_FECH_REGISTRO_SOLICITUD")) Then
                            Me.FechRegistro = CDate(dr("F_FECH_REGISTRO_SOLICITUD"))
                        End If

                        If Not IsDBNull(dr("F_FECH_APROBACION_SOLICITUD")) Then
                            Me.FechAprobacion = CDate(dr("F_FECH_APROBACION_SOLICITUD"))
                        End If

                        If Not IsDBNull(dr("F_FECH_CIERRE_SOLICITUD")) Then
                            Me.FechCierre = CDate(dr("F_FECH_CIERRE_SOLICITUD"))
                        End If

                        If Not IsDBNull(dr("F_FECH_RECHAZO")) Then
                            Me.FechRechazo = CDate(dr("F_FECH_RECHAZO"))
                        End If

                        If Not IsDBNull(dr("F_FECH_RECEPCION")) Then
                            Me.FechRecepcion = CDate(dr("F_FECH_RECEPCION"))
                        End If

                        If Not IsDBNull(dr("F_FECH_TERMINO_SOLICITUD")) Then
                            Me.FechTermino = CDate(dr("F_FECH_TERMINO_SOLICITUD"))
                        End If

                    End If
                End If
            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Finally
                If dr IsNot Nothing Then
                    If Not dr.IsClosed Then
                        dr.Close() : dr = Nothing
                    End If
                End If
            End Try

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Sub

    Public Function ObtenerSiguienteIdentificador(ByVal usuario As String) As Integer

        Dim resultado As Integer = 1



        Dim conexion As New Conexion.SQLServer

        Try
            'Dim query As String = "SELECT (MAX(N_ID_SOLICITUD_SERVICIO) + 1) N_ID_SOLICITUD_SERVICIO FROM BDS_D_GR_SOLICITUD_SERVICIO " & _
            '                        "WHERE N_NUM_FOLIO_ANIO = {0} AND T_ID_SOLICITANTE LIKE '{1}'"
            'query = String.Format(query, DateTime.Now.Year.ToString(), usuario)
            Dim query As String = "SELECT (MAX(N_ID_SOLICITUD_SERVICIO) + 1) N_ID_SOLICITUD_SERVICIO FROM BDS_D_GR_SOLICITUD_SERVICIO "
            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR(query)

            If dr.Read() Then

                If IsDBNull(dr("N_ID_SOLICITUD_SERVICIO")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_SOLICITUD_SERVICIO"))
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

    Public Function ObtenerSiguienteConsecutivoAnio() As Integer

        Dim resultado As Integer = 1
        Dim conexion As New Conexion.SQLServer
        Try
            Dim query As String = "SELECT (MAX(N_NUM_FOLIO_ID) + 1) N_NUM_FOLIO_ID FROM BDS_D_GR_SOLICITUD_SERVICIO WHERE N_NUM_FOLIO_ANIO = {0}"
            query = String.Format(query, Me.FolioAnio.ToString)
            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR(query)

            If dr.Read() Then

                If IsDBNull(dr("N_NUM_FOLIO_ID")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_NUM_FOLIO_ID"))
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
    Public Function ObtenerSiguienteConsecutivoUsuario() As Integer

        Dim resultado As Integer = 1
        Dim conexion As New Conexion.SQLServer
        Try
            Dim query As String = "SELECT (MAX(N_NUM_FOLIO_USUARIO) + 1) N_NUM_FOLIO_USUARIO FROM BDS_D_GR_SOLICITUD_SERVICIO WHERE T_ID_SOLICITANTE = '{0}'"
            query = String.Format(query, Me.UsuarioSolicitante)
            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR(query)
            If dr.Read() Then

                If IsDBNull(dr("N_NUM_FOLIO_USUARIO")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_NUM_FOLIO_USUARIO"))
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

    Public Function ObtenerSiguienteConsecutivoDocumento() As Integer

        Dim resultado As Integer = 1
        Dim conexion As New Conexion.SQLServer
        Try
            Dim query As String = "SELECT (MAX(N_ID_DOCUMENTO) + 1) N_ID_DOCUMENTO FROM BDS_D_GR_SOLICITUD_SERVICIO_DOCUMENTOS " & _
                                    "WHERE N_ID_SOLICITUD_SERVICIO = {0} AND N_ID_NIVELES_SERVICIO = {1}"
            query = String.Format(query, Me.Identificador.ToString, Me.NivelServicioDoc.ToString)
            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR(query)
            If dr.Read() Then

                If IsDBNull(dr("N_ID_DOCUMENTO")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_DOCUMENTO"))
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

    Public Function ObtenerServiciosSolicitud() As DataView
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataView
            Dim query As String = "SELECT SS.N_ID_NIVELES_SERVICIO, SR.T_DSC_SERVICIO FROM BDS_R_GR_SOLICITUD_SERVICIO SS " & _
                                    "INNER JOIN BDS_C_GR_NIVELES_SERVICIO NV ON SS.N_ID_NIVELES_SERVICIO = NV.N_ID_NIVELES_SERVICIO " & _
                                    "INNER JOIN BDS_C_GR_SERVICIOS SR ON NV.N_ID_SERVICIO = SR.N_ID_SERVICIO " & _
                                    "WHERE SS.N_ID_SOLICITUD_SERVICIO = {0}"
            query = String.Format(query, Me.Identificador.ToString)
            dv = conexion.ConsultarDT(query).DefaultView
            dv.Sort = "N_ID_NIVELES_SERVICIO"
            Return dv

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    Public Function ObtenerDocumentosBD() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT SD.N_ID_SOLICITUD_SERVICIO, NS.N_ID_NIVELES_SERVICIO,  SD.N_ID_DOCUMENTO, SD.T_DSC_ARCHIVO_ADJUNTO, SR.T_DSC_SERVICIO, 0 AS ES_NUEVO FROM BDS_D_GR_SOLICITUD_SERVICIO_DOCUMENTOS SD " & _
                                    "INNER JOIN BDS_C_GR_NIVELES_SERVICIO NS ON SD.N_ID_NIVELES_SERVICIO = NS.N_ID_NIVELES_SERVICIO " & _
                                    "INNER JOIN BDS_C_GR_SERVICIOS SR ON NS.N_ID_SERVICIO = SR.N_ID_SERVICIO " & _
                                    "WHERE SD.N_ID_SOLICITUD_SERVICIO = {0}"
            query = String.Format(query, Me.Identificador.ToString)
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
    ''' Agrega la Solicitud
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim solicitud As String
        solicitud = If((Me.IdFolio > 0 AndAlso Me.FolioAnio > 0), Me.IdFolio & "-" & Me.FolioAnio, Me.FolioUsario)

        Dim bitacora As New Conexion.Bitacora("Registro de nueva solicitud " & Solicitud, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_SOLICITUD_SERVICIO") : listValores.Add(Me.Identificador)
            listCampos.Add("N_NUM_FOLIO_ANIO") : listValores.Add(Me.FolioAnio)
            listCampos.Add("N_NUM_FOLIO_ID") : listValores.Add(Me.IdFolio)
            listCampos.Add("T_ID_SOLICITANTE") : listValores.Add(Me.UsuarioSolicitante)
            listCampos.Add("N_NUM_FOLIO_USUARIO") : listValores.Add(Me.FolioUsario)
            listCampos.Add("T_DSC_USUARIO_SOLICITUD") : listValores.Add(Me.UsuarioSolicitud)
            listCampos.Add("T_ID_AUTORIZADOR") : listValores.Add(Me.Autorizador)
            listCampos.Add("N_ID_ESTATUS_SERVICIO") : listValores.Add(Me.IdEstatus)
            listCampos.Add("F_FECH_REGISTRO_SOLICITUD") : listValores.Add(Me.FechRegistro)
            listCampos.Add("F_FECH_ULTIMA_MODIFICACION") : listValores.Add(Me.FechModificacion)
            listCampos.Add("N_NUM_EXTENSION") : listValores.Add(Me.Extension)
            listCampos.Add("T_DSC_NOTAS") : listValores.Add(Me.Notas)
            listCampos.Add("T_DSC_AREA_SOLICITANTE") : listValores.Add(Me.AreaSolicitante)

            Try
                resultado = conexion.Insertar(TblSolicitudServicio_D, listCampos, listValores)
                bitacora.Insertar(TblSolicitudServicio_D, listCampos, listValores, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Insertar(TblSolicitudServicio_D, listCampos, listValores, resultado, ex.Message.ToString)
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

    ''' <summary>
    ''' Actualiza un la solicitud
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim solicitud As String
        solicitud = If((Me.IdFolio > 0 AndAlso Me.FolioAnio > 0), Me.IdFolio & "-" & Me.FolioAnio, Me.FolioUsario)

        Dim bitacora As New Conexion.Bitacora("Actualización de Solicitud " & solicitud, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("T_DSC_USUARIO_SOLICITUD") : listValores.Add(Me.UsuarioSolicitud)
            listCampos.Add("F_FECH_REGISTRO_SOLICITUD") : listValores.Add(Me.FechRegistro)
            listCampos.Add("T_ID_AUTORIZADOR") : listValores.Add(Me.Autorizador)
            listCampos.Add("N_NUM_EXTENSION") : listValores.Add(Me.Extension)
            listCampos.Add("N_ID_ESTATUS_SERVICIO") : listValores.Add(Me.IdEstatus)
            listCampos.Add("F_FECH_ULTIMA_MODIFICACION") : listValores.Add(Me.FechModificacion)
            listCampos.Add("T_DSC_NOTAS") : listValores.Add(Me.Notas)

            If Me.FechRecepcion.HasValue Then
                listCampos.Add("F_FECH_RECEPCION") : listValores.Add(Me.FechRecepcion)
            End If
            If Me.FechRechazo.HasValue Then
                listCampos.Add("F_FECH_RECHAZO") : listValores.Add(Me.FechRechazo)
            End If
            If Me.FechCierre.HasValue Then
                listCampos.Add("F_FECH_CIERRE_SOLICITUD") : listValores.Add(Me.FechCierre)
            End If
            If Me.FechAprobacion.HasValue Then
                listCampos.Add("F_FECH_APROBACION_SOLICITUD") : listValores.Add(Me.FechAprobacion)
            End If
            If Me.FechTermino.HasValue Then
                listCampos.Add("F_FECH_TERMINO_SOLICITUD") : listValores.Add(Me.FechTermino)
            End If
            If Not String.IsNullOrEmpty(Me.CausaRechazo) And Not String.IsNullOrWhiteSpace(Me.CausaRechazo) Then
                listCampos.Add("T_DSC_CAUSAS_RECHAZO") : listValores.Add(Me.CausaRechazo)
            End If
            If Not String.IsNullOrEmpty(Me.SelloSolicitud) And Not String.IsNullOrWhiteSpace(Me.SelloSolicitud) Then
                listCampos.Add("T_DSC_SELLO_DIGITAL_SOLICITUD") : listValores.Add(Me.SelloSolicitud)
            End If
            If Not String.IsNullOrEmpty(Me.SelloAcuse) And Not String.IsNullOrWhiteSpace(Me.SelloAcuse) Then
                listCampos.Add("T_DSC_SELLO_DIGITAL_ACUSE") : listValores.Add(Me.SelloAcuse)
            End If

            listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(Me.Identificador)

            Try
                resultado = conexion.Actualizar(TblSolicitudServicio_D, listCampos, listValores, listCamposCondicion, listValoresCondicion)
                bitacora.Actualizar(TblSolicitudServicio_D, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Actualizar(TblSolicitudServicio_D, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, ex.Message.ToString)
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


    ''' <summary>
    ''' Registra los servicios asociados a una solicitud
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GuardarServicios() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer

        Dim solicitud As String
        solicitud = If((Me.IdFolio > 0 AndAlso Me.FolioAnio > 0), Me.IdFolio & "-" & Me.FolioAnio, Me.FolioUsario)

        Dim bitacora As New Conexion.Bitacora("Registro de servicio " & Me.NivelServicio & " de la solicitud " & Solicitud, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_SOLICITUD_SERVICIO") : listValores.Add(Me.Identificador)
            listCampos.Add("N_ID_NIVELES_SERVICIO") : listValores.Add(Me.NivelServicio)
            listCampos.Add("N_ID_ESTATUS_SERVICIO") : listValores.Add(Me.EstatusServicio)
            Try
                resultado = conexion.Insertar(TblSolicitudServicio_R, listCampos, listValores)
                bitacora.Insertar(TblSolicitudServicio_R, listCampos, listValores, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Insertar(TblSolicitudServicio_R, listCampos, listValores, resultado, ex.ToString)
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

    ''' <summary>
    ''' Actualiza un servicio asociado a una solicitud, asignandole la fecha limite de termino
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ActualizarServicioAgenda() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim bitacora As New Conexion.Bitacora("Actualización de servicio " & Me.NivelServicio, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("F_FECH_TERMINO_SERVICIO") : listValores.Add(Me.FechaLimite)

            If Me.TiempoEjecucion > 0 Then
                listCampos.Add("N_NUM_TIEMPO_EJECUCION") : listValores.Add(Me.TiempoEjecucion)
            End If

            listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(Me.Identificador)
            listCamposCondicion.Add("N_ID_NIVELES_SERVICIO") : listValoresCondicion.Add(Me.NivelServicio)

            Try
                resultado = conexion.Actualizar(TblSolicitudServicio_R, listCampos, listValores, listCamposCondicion, listValoresCondicion)
                bitacora.Actualizar(TblSolicitudServicio_R, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Actualizar(TblSolicitudServicio_R, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, ex.Message.ToString)
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

    ''' <summary>
    ''' Actualiza un servicio asociado a una solicitud, asignandole la fecha limite de termino
    ''' </summary>
    ''' <param name="conexion">Objeto conexion</param>
    ''' <param name="bitacora">Objeto bitacora</param>
    ''' <param name="tran">objeto transaccion</param>
    ''' <remarks></remarks>
    Public Sub ActualizarServicioAgenda(ByVal conexion As Conexion.SQLServer, ByVal bitacora As Conexion.Bitacora, ByVal tran As SqlClient.SqlTransaction)
        Dim resultado As Boolean = False

        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        listCampos.Add("F_FECH_TERMINO_SERVICIO") : listValores.Add(Me.FechaLimite)

        If Me.TiempoEjecucion > 0 Then
            listCampos.Add("N_NUM_TIEMPO_EJECUCION") : listValores.Add(Me.TiempoEjecucion)
        End If

        listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(Me.Identificador)
        listCamposCondicion.Add("N_ID_NIVELES_SERVICIO") : listValoresCondicion.Add(Me.NivelServicio)

        Try
            resultado = conexion.ActualizarConTransaccion(TblSolicitudServicio_R, listCampos, listValores, listCamposCondicion, listValoresCondicion, tran)
            bitacora.ActualizarConTransaccion(TblSolicitudServicio_R, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")
        Catch ex As Exception
            resultado = False
            bitacora.ActualizarConTransaccion(TblSolicitudServicio_R, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, ex.Message.ToString)
            Throw ex
        End Try

    End Sub


    ''' <summary>
    ''' Registra los documentos asociados a una solicitud
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GuardarDocumentos() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer
        Dim cosa = Me.Identificador
        Dim solicitud As String
        solicitud = If((Me.IdFolio > 0 AndAlso Me.FolioAnio > 0), Me.IdFolio & "-" & Me.FolioAnio, Me.FolioUsario)

        Dim bitacora As New Conexion.Bitacora("Registro de documento " & Me.IdDocumento & " de la solicitud " & solicitud, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_SOLICITUD_SERVICIO") : listValores.Add(Me.Identificador)
            listCampos.Add("N_ID_NIVELES_SERVICIO") : listValores.Add(Me.NivelServicioDoc)
            listCampos.Add("N_ID_DOCUMENTO") : listValores.Add(Me.IdDocumento)
            listCampos.Add("T_DSC_ARCHIVO_ADJUNTO") : listValores.Add(Me.NombreDocumento)
            Try
                resultado = conexion.Insertar(TblSolicitudServicioDoc, listCampos, listValores)
                bitacora.Insertar(TblSolicitudServicioDoc, listCampos, listValores, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Insertar(TblSolicitudServicioDoc, listCampos, listValores, resultado, ex.ToString)
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

    ''' <summary>
    ''' Elimina los servicios asociados a una solicitud
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EliminarServicios() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer

        Dim solicitud As String
        solicitud = If((Me.IdFolio > 0 AndAlso Me.FolioAnio > 0), Me.IdFolio & "-" & Me.FolioAnio, Me.FolioUsario)

        Dim bitacora As New Conexion.Bitacora("Eliminación de servicios de la solicitud " & solicitud, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)
            listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(Me.Identificador)

            Try
                resultado = conexion.Eliminar(TblSolicitudServicio_R, listCamposCondicion, listValoresCondicion)
            Catch ex As Exception
                resultado = False
                bitacora.Eliminar(TblSolicitudServicio_R, listCamposCondicion, listValoresCondicion, resultado, "")
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

    ''' <summary>
    ''' Elimina el servicio indicado asociado a una solicitud
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EliminarServicio() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer

        Dim solicitud As String
        solicitud = If((Me.IdFolio > 0 AndAlso Me.FolioAnio > 0), Me.IdFolio & "-" & Me.FolioAnio, Me.FolioUsario)

        Dim bitacora As New Conexion.Bitacora("Eliminación de servicio " & Me.NivelServicio & " de la solicitud " & solicitud, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)
            listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(Me.Identificador)
            listCamposCondicion.Add("N_ID_NIVELES_SERVICIO") : listValoresCondicion.Add(Me.NivelServicio)
            Try
                resultado = conexion.Eliminar(TblSolicitudServicio_R, listCamposCondicion, listValoresCondicion)
            Catch ex As Exception
                resultado = False
                bitacora.Eliminar(TblSolicitudServicio_R, listCamposCondicion, listValoresCondicion, resultado, "")
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

    ''' <summary>
    ''' Elimina los documentos asociado a una solicitud
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EliminarDocumentos() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer

        Dim solicitud As String
        solicitud = If((Me.IdFolio > 0 AndAlso Me.FolioAnio > 0), Me.IdFolio & "-" & Me.FolioAnio, Me.FolioUsario)

        Dim bitacora As New Conexion.Bitacora("Eliminación de documentos de la Solicitud " & solicitud, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)
            listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(Me.Identificador)

            Try
                resultado = conexion.Eliminar(TblSolicitudServicioDoc, listCamposCondicion, listValoresCondicion)
            Catch ex As Exception
                resultado = False
                bitacora.Eliminar(TblSolicitudServicioDoc, listCamposCondicion, listValoresCondicion, resultado, "")
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


    ''' <summary>
    ''' Elimina un documento asociado a una solicitud
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EliminarDocumento() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer

        Dim objSolicitud As New Solicitud(Identificador)
        Dim solicitud As String
        solicitud = If((objSolicitud.IdFolio > 0 AndAlso objSolicitud.FolioAnio > 0), objSolicitud.IdFolio & "-" & objSolicitud.FolioAnio, objSolicitud.FolioUsario)
        Dim objDocumento As New DocumentoSolicitud(Me.NombreDocumento)
        Dim bitacora As New Conexion.Bitacora("Eliminación de documento " & objDocumento.Documento & " de la solicitud " & solicitud, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)
            listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(Me.Identificador)
            listCamposCondicion.Add("T_DSC_ARCHIVO_ADJUNTO") : listValoresCondicion.Add(Me.NombreDocumento)

            Try
                resultado = conexion.Eliminar(TblSolicitudServicioDoc, listCamposCondicion, listValoresCondicion)
            Catch ex As Exception
                resultado = False
                bitacora.Eliminar(TblSolicitudServicioDoc, listCamposCondicion, listValoresCondicion, resultado, "")
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
    ''' <summary>
    ''' Elimina un documento asociado a una solicitud
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EliminarDocumentoID() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer

        Dim solicitud As String
        solicitud = If((Me.IdFolio > 0 AndAlso Me.FolioAnio > 0), Me.IdFolio & "-" & Me.FolioAnio, Me.FolioUsario)

        Dim bitacora As New Conexion.Bitacora("Eliminación de documentos del servicio " & Me.NivelServicioDoc & " sobre la solicitud " & solicitud, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)
            listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(Me.Identificador)
            listCamposCondicion.Add("N_ID_NIVELES_SERVICIO") : listValoresCondicion.Add(Me.NivelServicioDoc)

            Try
                resultado = conexion.Eliminar(TblSolicitudServicioDoc, listCamposCondicion, listValoresCondicion)
            Catch ex As Exception
                resultado = False
                bitacora.Eliminar(TblSolicitudServicioDoc, listCamposCondicion, listValoresCondicion, resultado, "")
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


    ''' <summary>
    ''' "Elimina" la solicitud cambio su estatus a "Cancelado por Solicitante"
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Eliminar() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim solicitud As String
        solicitud = If((Me.IdFolio > 0 AndAlso Me.FolioAnio > 0), Me.IdFolio & "-" & Me.FolioAnio, Me.FolioUsario)

        Dim bitacora As New Conexion.Bitacora("Eliminación de solicitud " & solicitud, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("N_ID_ESTATUS_SERVICIO") : listValores.Add("10")

            listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(Me.Identificador)

            Try
                resultado = conexion.Actualizar(TblSolicitudServicio_D, listCampos, listValores, listCamposCondicion, listValoresCondicion)
                bitacora.Actualizar(TblSolicitudServicio_D, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Actualizar(TblSolicitudServicio_D, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, ex.Message.ToString)
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


    ''' <summary>
    ''' Actualiza el estatus de la solicitud por el parametro proporcionado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CambiarEstatus() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim solicitud As String
        solicitud = If((Me.IdFolio > 0 AndAlso Me.FolioAnio > 0), Me.IdFolio & "-" & Me.FolioAnio, Me.FolioUsario)
        Dim Estatus As String = New EstatusServicio(Me.IdEstatus).Descripcion

        Dim bitacora As New Conexion.Bitacora("Cambio en la solicitud " & solicitud & " al estatus " & Estatus, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            If Me.FechaRechazoSolic.HasValue Then
                listCampos.Add("F_FECH_RECHAZO") : listValores.Add(Me.FechaRechazoSolic)
            End If

            listCampos.Add("N_ID_ESTATUS_SERVICIO") : listValores.Add(Me.IdEstatus)
            listCampos.Add("F_FECH_ULTIMA_MODIFICACION") : listValores.Add(Me.FechModificacion)
            If Me.FolioAnio > 0 Then
                listCampos.Add("N_NUM_FOLIO_ANIO") : listValores.Add(Me.FolioAnio)
            End If
            If Me.IdFolio > 0 Then
                listCampos.Add("N_NUM_FOLIO_ID") : listValores.Add(Me.IdFolio)
            End If

            listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(Me.Identificador)
            Try
                resultado = conexion.Actualizar(TblSolicitudServicio_D, listCampos, listValores, listCamposCondicion, listValoresCondicion)
                bitacora.Actualizar(TblSolicitudServicio_D, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Actualizar(TblSolicitudServicio_D, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, ex.Message.ToString)
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

    ''' <summary>
    ''' Actualiza el estatus de la solicitud por el parametro proporcionado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ActualizarServicio() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim solicitud As String
        solicitud = If((Me.IdFolio > 0 AndAlso Me.FolioAnio > 0), Me.IdFolio & "-" & Me.FolioAnio, Me.FolioUsario)
        Dim Estatus As String = New EstatusServicio(EstatusServicio).Descripcion

        Dim bitacora As New Conexion.Bitacora("Cambio en el servicio " & Me.NivelServicio & " de la solicitud " & solicitud & " al estatus " & Estatus, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            If Me.EstatusServicio > 0 Then
                listCampos.Add("N_ID_ESTATUS_SERVICIO") : listValores.Add(Me.EstatusServicio)
            End If
            If Me.FechaLimite.HasValue Then
                listCampos.Add("F_FECH_TERMINO_SERVICIO") : listValores.Add(Me.FechaLimite)
            End If
            If Me.FechaTerminoServ.HasValue Then
                listCampos.Add("F_FECH_TERMINO_SERVICIO_DEF") : listValores.Add(Me.FechaTerminoServ)
            End If
            If Me.FechaRechazoIng.HasValue Then
                listCampos.Add("F_FECH_RECHAZO_ING") : listValores.Add(Me.FechaRechazoIng)
            End If
            If Me.FechaRechazoSolic.HasValue Then
                listCampos.Add("F_FECH_RECHAZO_SOLIC") : listValores.Add(Me.FechaRechazoSolic)
            End If
            If Me.TiempoEjecucion > 0 Then
                listCampos.Add("N_NUM_TIEMPO_EJECUCION") : listValores.Add(Me.TiempoEjecucion)
            End If

            If Me.FechAtendioIngeniero.HasValue Then
                listCampos.Add("F_FECH_ATENCION") : listValores.Add(DateTime.Now)
            End If

            listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(Me.Identificador)
            listCamposCondicion.Add("N_ID_NIVELES_SERVICIO") : listValoresCondicion.Add(Me.NivelServicio)

            Try
                resultado = conexion.Actualizar(TblSolicitudServicio_R, listCampos, listValores, listCamposCondicion, listValoresCondicion)
                bitacora.Actualizar(TblSolicitudServicio_R, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Actualizar(TblSolicitudServicio_R, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, ex.Message.ToString)
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

    Public Function ConsultaNombreDocumento(ByVal nombre As String) As String

        Dim resultado As String = String.Empty
        Dim conexion As New Conexion.SQLServer
        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT T_DSC_ARCHIVO_ADJUNTO FROM BDS_D_GR_SOLICITUD_SERVICIO_DOCUMENTOS WHERE T_DSC_ARCHIVO_ADJUNTO = '" + nombre + "'")

            If dr.Read() Then
                If Not IsDBNull(dr("T_DSC_ARCHIVO_ADJUNTO")) Then
                    resultado = CStr(dr("T_DSC_ARCHIVO_ADJUNTO"))
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

    Public Function ObtenerTipoFlujo() As Integer
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT NV.N_ID_FLUJO FROM BDS_R_GR_SOLICITUD_SERVICIO SS " & _
                                    "INNER JOIN dbo.BDS_C_GR_NIVELES_SERVICIO NV ON NV.N_ID_NIVELES_SERVICIO = SS.N_ID_NIVELES_SERVICIO " & _
                                    "WHERE SS.N_ID_SOLICITUD_SERVICIO = {0}"
            query = String.Format(query, Me.Identificador.ToString)
            dv = conexion.ConsultarDT(query)

            Dim dvFlujo As DataView = dv.DefaultView
            dvFlujo.RowFilter = "N_ID_FLUJO = 3"
            If dvFlujo.ToTable.Rows.Count > 0 Then
                Return 3
            End If
            dvFlujo = dv.DefaultView
            dvFlujo.RowFilter = "N_ID_FLUJO = 2"
            If dvFlujo.ToTable.Rows.Count > 0 Then
                Return 2
            Else
                Return 1
            End If
        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try
    End Function
    Public Function ObtenerFlujo() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT PS.T_DSC_PASO_TOOLTIP AS TOOLTIP, IM.T_DSC_RUTA_IMAGEN AS IMAGEN, IM2.T_DSC_RUTA_IMAGEN AS IMAGEN_INACTIVA, PS.T_DSC_COLOR_HEX_ANTES AS COLOR_ANTES, " & _
                                    "PS.T_DSC_COLOR_HEX_ACTUAL AS COLOR_ACTUAL, PS.T_DSC_COLOR_HEX_POSTERIOR AS COLOR_POSTERIOR, SF.N_ID_ORDEN, PS.N_ID_ESTATUS_SERVICIO, PS.B_FLAG_VISIBLE AS VISIBLE " & _
                                    "FROM BDS_R_GR_SEGUIMIENTO_FLUJO SF " & _
                                    "INNER JOIN BDS_C_GR_PASOS_SEGUIMIENTO PS ON PS.N_ID_PASO = SF.N_ID_PASO " & _
                                    "INNER JOIN BDS_C_GR_IMAGEN IM ON IM.N_ID_IMAGEN = PS.N_ID_IMAGEN " & _
                                    "INNER JOIN BDS_C_GR_IMAGEN IM2 ON IM2.N_ID_IMAGEN = PS.N_ID_IMAGEN_INACTIVA " & _
                                    "WHERE SF.N_ID_FLUJO = {0} ORDER BY SF.N_ID_ORDEN"
            query = String.Format(query, Me.IdFlujo.ToString())
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

    'Public Function BajaTemporal() As Boolean
    '    Dim resultado As Boolean
    '    Dim conexion As New Conexion.SQLServer
    '    Dim bitacora As New Conexion.Bitacora("Eliminación temporal de un servicio asociado a una solicitud", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
    '    Try

    '        Dim listCampos As New List(Of String)
    '        Dim listValores As New List(Of Object)
    '        Dim listCamposCondicion As New List(Of String)
    '        Dim listValoresCondicion As New List(Of Object)

    '        listCampos.Add("B_DELETE_TEMP") : listValores.Add(Me.DeleteTempSS)
    '        listCampos.Add("B_INSERT_TEMP") : listValores.Add(Me.InsertTempSS)

    '        listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(Me.Identificador)
    '        listCamposCondicion.Add("N_ID_NIVELES_SERVICIO") : listValoresCondicion.Add(Me.NivelServicio)

    '        Try
    '            resultado = conexion.Actualizar(TblSolicitudServicio_R, listCampos, listValores, listCamposCondicion, listValoresCondicion)
    '            bitacora.Actualizar(TblSolicitudServicio_R, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")
    '        Catch ex As Exception
    '            resultado = False
    '            bitacora.Actualizar(TblSolicitudServicio_R, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, ex.Message.ToString)
    '            Throw ex
    '        End Try

    '    Catch ex As Exception

    '        resultado = False
    '        Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

    '    Finally

    '        bitacora.Finalizar(resultado)

    '        If Not IsNothing(conexion) Then
    '            conexion.CerrarConexion()
    '        End If

    '    End Try

    '    Return resultado
    'End Function

    Public Function ObtenerServiciosAtencion(Optional ByVal EsReasignacion As Boolean = False) As DataTable
        Dim conexion As New Conexion.SQLServer

        Try
            '08/05/2014 Se modifica la query para obtener el nombre completo del ingeniero
            Dim dv As New DataTable
            Dim query As String = "SELECT DISTINCT AG.N_ID_NIVELES_SERVICIO, AG.N_ID_SOLICITUD_SERVICIO, SS.F_FECH_ATENCION, " & _
                                    "(TS.T_DSC_TIPO_SERVICIO + '\' + AP.T_DSC_ACRONIMO_APLICATIVO + '\' + SE.T_DSC_SERVICIO) AS SERVICIO,  " & _
                                    "FL.N_ID_FLUJO, FL.T_DSC_FLUJO, (US.T_DSC_NOMBRE + ' ' + US.T_DSC_APELLIDO + ' ' + US.T_DSC_APELLIDO_AUX) AS INGENIERO_ASIGNADO,  " & _
                                    "SS.F_FECH_TERMINO_SERVICIO AS F_FECH_TENTATIVA_TERMINO_SOLICITUD, SS.F_FECH_TERMINO_SERVICIO_DEF AS F_FECH_TERMINO_SOLICITUD, " & _
                                    "SS.F_FECH_RECHAZO_ING , SS.F_FECH_RECHAZO_SOLIC, SS.N_ID_ESTATUS_SERVICIO,ST.T_DSC_ESTATUS_SERVICIO, AG.T_ID_INGENIERO, SR.T_ID_SOLICITANTE " & _
                                    "FROM BDS_D_TI_AGENDA AG  " & _
                                    "INNER JOIN BDS_R_GR_SOLICITUD_SERVICIO SS ON SS.N_ID_NIVELES_SERVICIO = AG.N_ID_NIVELES_SERVICIO " & _
                                    "INNER JOIN BDS_C_GR_ESTATUS_SERVICIO ST ON ST.N_ID_ESTATUS_SERVICIO = SS.N_ID_ESTATUS_SERVICIO " & _
                                    "INNER JOIN dbo.BDS_C_GR_NIVELES_SERVICIO NS ON NS.N_ID_NIVELES_SERVICIO = AG.N_ID_NIVELES_SERVICIO " & _
                                    "INNER JOIN dbo.BDS_C_GR_SERVICIOS SE ON SE.N_ID_SERVICIO = NS.N_ID_SERVICIO  " & _
                                    "INNER JOIN BDS_C_GR_APLICATIVO AP ON AP.N_ID_APLICATIVO = NS.N_ID_APLICATIVO " & _
                                    "INNER JOIN BDS_C_GR_TIPO_SERVICIO TS ON TS.N_ID_TIPO_SERVICIO = NS.N_ID_TIPO_SERVICIO  " & _
                                    "INNER JOIN BDS_C_GR_FLUJO FL ON FL.N_ID_FLUJO = NS.N_ID_FLUJO  " & _
                                    "INNER JOIN BDS_C_GR_USUARIO US ON US.T_ID_USUARIO = AG.T_ID_INGENIERO  " & _
                                    "INNER JOIN BDS_D_GR_SOLICITUD_SERVICIO SR ON SR.N_ID_SOLICITUD_SERVICIO = SS.N_ID_SOLICITUD_SERVICIO "
            If EsReasignacion Then
                query = query & "WHERE AG.N_ID_SOLICITUD_SERVICIO = {0} AND SS.N_ID_SOLICITUD_SERVICIO = {0} AND SS.N_ID_ESTATUS_SERVICIO IN (13,21) ORDER BY SERVICIO ASC"
            Else
                query = query & "WHERE AG.N_ID_SOLICITUD_SERVICIO = {0} AND SS.N_ID_SOLICITUD_SERVICIO = {0} ORDER BY SERVICIO ASC"
            End If
            query = String.Format(query, Me.Identificador)
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

    Public Function ObtenerTotales() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT COUNT(A.N_ID_SOLICITUD_SERVICIO) AS TOTAL,  " & _
                                    "(SELECT COUNT(B.N_ID_SOLICITUD_SERVICIO) FROM BDS_R_GR_SOLICITUD_SERVICIO B WHERE B.N_ID_SOLICITUD_SERVICIO = A.N_ID_SOLICITUD_SERVICIO AND B.N_ID_ESTATUS_SERVICIO = 14 ) AS TOTAL_ATENDIDAS, " & _
                                    "(SELECT COUNT(B.N_ID_SOLICITUD_SERVICIO) FROM BDS_R_GR_SOLICITUD_SERVICIO B WHERE B.N_ID_SOLICITUD_SERVICIO = A.N_ID_SOLICITUD_SERVICIO AND B.N_ID_ESTATUS_SERVICIO = 15 ) AS TOTAL_RECHAZADAS " & _
                                    "FROM BDS_R_GR_SOLICITUD_SERVICIO A WHERE A.N_ID_SOLICITUD_SERVICIO = {0} GROUP BY A.N_ID_SOLICITUD_SERVICIO "
            query = String.Format(query, Me.Identificador)
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
    Public Function ObtenerTotalesVB() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT COUNT(A.N_ID_SOLICITUD_SERVICIO) AS TOTAL,  " & _
                                    "(SELECT COUNT(B.N_ID_SOLICITUD_SERVICIO) FROM BDS_R_GR_SOLICITUD_SERVICIO B WHERE B.N_ID_SOLICITUD_SERVICIO = A.N_ID_SOLICITUD_SERVICIO AND B.N_ID_ESTATUS_SERVICIO = 18 ) AS TOTAL_ATENDIDAS, " & _
                                    "(SELECT COUNT(B.N_ID_SOLICITUD_SERVICIO) FROM BDS_R_GR_SOLICITUD_SERVICIO B WHERE B.N_ID_SOLICITUD_SERVICIO = A.N_ID_SOLICITUD_SERVICIO AND B.N_ID_ESTATUS_SERVICIO = 19 ) AS TOTAL_RECHAZADAS, " & _
                                    "(SELECT COUNT(B.N_ID_SOLICITUD_SERVICIO) FROM BDS_R_GR_SOLICITUD_SERVICIO B WHERE B.N_ID_SOLICITUD_SERVICIO = A.N_ID_SOLICITUD_SERVICIO AND B.N_ID_ESTATUS_SERVICIO = 15 ) AS TOTAL_RECHAZADAS_ING " & _
                                    "FROM BDS_R_GR_SOLICITUD_SERVICIO A WHERE A.N_ID_SOLICITUD_SERVICIO = {0} GROUP BY A.N_ID_SOLICITUD_SERVICIO "
            query = String.Format(query, Me.Identificador)
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
            Dim query As String = "SELECT N_ID_HISTORICO, N_ID_SOLICITUD_SERVICIO, N_ID_NIVELES_SERVICIO, F_FECHA_NOTA, T_DSC_NOTA, NT.T_ID_USUARIO, T_DSC_ESTATUS_SERVICIO,  " & _
                                    "(US.T_DSC_NOMBRE + ' ' + US.T_DSC_APELLIDO) AS USUARIO " & _
                                    "FROM dbo.BDS_D_GR_HISTORICO_NOTAS NT  " & _
                                    "INNER JOIN BDS_C_GR_USUARIO US ON US.T_ID_USUARIO = NT.T_ID_USUARIO WHERE N_ID_SOLICITUD_SERVICIO = {0} AND NT.N_ID_NIVELES_SERVICIO = {1}"
            query = String.Format(query, Me.Identificador, Me.NivelServicio)
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


    Public Function DestinatariosSubdirectores() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT AR.T_ID_SUBDIRECTOR, AR.T_ID_BACKUP FROM BDS_C_GR_AREA AR " & _
                                    "INNER JOIN BDS_C_GR_NIVELES_SERVICIO NV ON NV.N_ID_AREA = AR.N_ID_AREA " & _
                                    "INNER JOIN BDS_R_GR_SOLICITUD_SERVICIO SS ON SS.N_ID_NIVELES_SERVICIO = NV.N_ID_NIVELES_SERVICIO " & _
                                    "WHERE SS.N_ID_SOLICITUD_SERVICIO = {0} "
            query = String.Format(query, Me.Identificador)
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
    Public Function DestinatariosSubdirectoresServicio() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT AR.T_ID_SUBDIRECTOR, AR.T_ID_BACKUP FROM BDS_C_GR_AREA AR " & _
                                    "INNER JOIN BDS_C_GR_NIVELES_SERVICIO NV ON NV.N_ID_AREA = AR.N_ID_AREA " & _
                                    "INNER JOIN BDS_R_GR_SOLICITUD_SERVICIO SS ON SS.N_ID_NIVELES_SERVICIO = NV.N_ID_NIVELES_SERVICIO " & _
                                    "WHERE SS.N_ID_SOLICITUD_SERVICIO = {0} AND SS.N_ID_NIVELES_SERVICIO = {1}"
            query = String.Format(query, Me.Identificador, Me.NivelServicio)
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

    Public Function DestinatariosIngenieros() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT DISTINCT T_ID_INGENIERO FROM BDS_D_TI_AGENDA WHERE N_ID_SOLICITUD_SERVICIO = {0} "
            query = String.Format(query, Me.Identificador)
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

    Public Function DestinatariosIngenierosServicio() As String
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT DISTINCT T_ID_INGENIERO FROM BDS_D_TI_AGENDA WHERE N_ID_SOLICITUD_SERVICIO = {0} AND N_ID_NIVELES_SERVICIO = {1} "
            query = String.Format(query, Me.Identificador, Me.NivelServicio)
            dv = conexion.ConsultarDT(query)
            If dv.Rows.Count > 0 Then
                For Each dr As DataRow In dv.Rows
                    Return dr("T_ID_INGENIERO")
                Next
            End If
            Return String.Empty
        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try
    End Function
    Public Function TieneServiciosIngePendientes(ByVal inge As String) As Boolean
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataTable
            Dim query As String = " SELECT DISTINCT(AG.N_ID_NIVELES_SERVICIO), AG.T_ID_INGENIERO, SR.N_ID_ESTATUS_SERVICIO FROM BDS_D_TI_AGENDA AG " & _
                                    "INNER JOIN BDS_R_GR_SOLICITUD_SERVICIO SR ON AG.N_ID_NIVELES_SERVICIO = SR.N_ID_NIVELES_SERVICIO " & _
                                    "WHERE AG.T_ID_INGENIERO = '{0}' AND AG.N_ID_SOLICITUD_SERVICIO = {1} AND SR.N_ID_SOLICITUD_SERVICIO = {1} AND (SR.N_ID_ESTATUS_SERVICIO = 13 OR SR.N_ID_ESTATUS_SERVICIO = 19 OR SR.N_ID_ESTATUS_SERVICIO = 21) "
            query = String.Format(query, inge, Me.Identificador)
            dv = conexion.ConsultarDT(query)
            If dv.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
#End Region

End Class
