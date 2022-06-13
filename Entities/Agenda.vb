'- Fecha de creación:##/##/####
'- Fecha de modificación:  31/03/2014
'- Nombre del Responsable: 
'- Empresa: Softtek
'- Clase de Agenda

Public Class Agenda
    Private TblAgenda As String = "BDS_D_TI_AGENDA"
    Private TblRegistroAgenda As String = "BDS_D_TI_REGISTRO_AGENDA"

    Public Enum TipoTarea
        NivelServicio = 1
        RegistroAgenda = 2
    End Enum

    '-- TABLA BDS_D_TI_AGENDA --
    Public Property Identificador As Integer
    Public Property Ingeniero As String
    Public Property FechaHoraTarea As DateTime
    Public Property IdTipoTarea As TipoTarea
    Public Property IdSolicitud As Integer
    Public Property IdNivelServicio As Integer
    Public Property IdRegistroAgendaFK As Integer
    Public Property TareaAgenda As String
    Public Property idConsecutivo As Integer

    '-- TABLA BDS_D_TI_REGISTRO_AGENDA --
    Public Property IdRegistroAgenda As Integer
    Public Property IngSolict As String
    Public Property Autorizador As String
    Public Property TipoRegistro As Integer
    Public Property TipoActividad As Integer
    Public Property TipoAusencia As Integer
    Public Property Ciclica As Integer
    Public Property FechIniReg As DateTime
    Public Property FechFinReg As DateTime? = Nothing
    Public Property Lunes As Integer = 0
    Public Property Martes As Integer = 0
    Public Property Miercoles As Integer = 0
    Public Property Jueves As Integer = 0
    Public Property Viernes As Integer = 0
    Public Property NotaRegistro As String
    Public Property NotaAutorizador As String = ""
    Public Property VigFlag As Integer
    Public Property FlagAprobado As Integer = 0
    Public Property FechReg As DateTime
    Public Property FechAutorizacion As DateTime
    Public Property FlujoEspecial As Boolean = False

    Public Sub New()

    End Sub

    Public Sub New(ByVal idAgenda)
        Me.Identificador = idAgenda
        CargarDatos()
    End Sub

    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer


        Try

            Return conexion.ConsultarRegistrosDT(TblAgenda).DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function

    ''' <summary>
    ''' Obtiene todos los registro dentro de una transaccion
    ''' </summary>
    ''' <param name="con">Objeto Conexion</param>
    ''' <param name="tran">Objeto Transaccion</param>
    ''' <returns>DataView con los registros</returns>
    ''' <remarks></remarks>
    Public Function ObtenerTodos(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As DataView
        Try
            Return con.ConsultarRegistrosDTConTransaccion(TblAgenda, tran).DefaultView
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerTodosIngeniero() As DataTable
        Dim conexion As New Conexion.SQLServer
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)
        Try
            listCamposCondicion.Add("T_ID_INGENIERO") : listValoresCondicion.Add(Me.Ingeniero)
            listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(Me.IdSolicitud)
            listCamposCondicion.Add("N_ID_NIVELES_SERVICIO") : listValoresCondicion.Add(Me.IdNivelServicio)
            Return conexion.ConsultarRegistrosDT(TblAgenda, listCamposCondicion, listValoresCondicion, "F_FECH_FECHA_HORA_TAREA")

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function

    Public Function ObtenerFechaFinIngeniero() As DataTable
        Dim conexion As New Conexion.SQLServer
        Dim query As String = String.Empty
        Try
            'query = "SELECT TOP 1 AG.T_ID_INGENIERO, AG.T_DSC_TAREA_AGENDA, AG.F_FECH_FECHA_HORA_TAREA , SS.F_FECH_TERMINO_SERVICIO FROM BDS_D_TI_AGENDA AG " & _
            '        "INNER JOIN BDS_R_GR_SOLICITUD_SERVICIO SS ON SS.N_ID_NIVELES_SERVICIO = AG.N_ID_NIVELES_SERVICIO " & _
            '        "WHERE AG.T_ID_INGENIERO = '{0}' AND AG.N_ID_CONSECUTIVO_SOPORTE IS NULL ORDER BY SS.F_FECH_TERMINO_SERVICIO DESC "
            query = "SELECT TOP(1) AG.T_ID_INGENIERO, AG.T_DSC_TAREA_AGENDA, AG.F_FECH_FECHA_HORA_TAREA, SS.F_FECH_TERMINO_SERVICIO " & _
                    "FROM   BDS_D_TI_AGENDA AS AG INNER JOIN" & _
                    "       BDS_R_GR_SOLICITUD_SERVICIO AS SS ON AG.N_ID_SOLICITUD_SERVICIO = SS.N_ID_SOLICITUD_SERVICIO " & _
                    "WHERE  (AG.T_ID_INGENIERO = '{0}') AND (AG.N_ID_CONSECUTIVO_SOPORTE IS NULL) " & _
                    "ORDER BY AG.F_FECH_FECHA_HORA_TAREA DESC"
            query = String.Format(query, Me.Ingeniero)
            Dim dt As DataTable = conexion.ConsultarDT(query)
            Return dt
        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function


    Public Function ObtenerFechaFinIngeniero(ByVal conexion As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As DataTable
        Dim query As String = String.Empty
        Try
            query = "SELECT TOP(1) AG.T_ID_INGENIERO, AG.T_DSC_TAREA_AGENDA, AG.F_FECH_FECHA_HORA_TAREA, SS.F_FECH_TERMINO_SERVICIO " & _
                    "FROM   BDS_D_TI_AGENDA AS AG INNER JOIN" & _
                    "       BDS_R_GR_SOLICITUD_SERVICIO AS SS ON AG.N_ID_SOLICITUD_SERVICIO = SS.N_ID_SOLICITUD_SERVICIO " & _
                    "WHERE  (AG.T_ID_INGENIERO = '{0}') AND (AG.N_ID_CONSECUTIVO_SOPORTE IS NULL) " & _
                    "ORDER BY AG.F_FECH_FECHA_HORA_TAREA DESC"
            query = String.Format(query, Me.Ingeniero)
            Dim dt As DataTable = conexion.ConsultarDTConTransaccion(query, tran)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerEstimadaInicio(ByVal idSol As String) As String
        Dim conexion As New Conexion.SQLServer
        Dim query As String = String.Empty
        Dim fecha As String = String.Empty
        Try
            query = "SELECT TOP 1 F_FECH_FECHA_HORA_TAREA FROM BDS_D_TI_AGENDA " & _
                    "WHERE N_ID_SOLICITUD_SERVICIO = {0} ORDER BY F_FECH_FECHA_HORA_TAREA ASC "
            query = String.Format(query, idSol)
            Dim dt As DataTable = conexion.ConsultarDT(query)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    fecha = CStr(dr("F_FECH_FECHA_HORA_TAREA"))
                Next
            End If
            Return fecha
        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function
    Public Sub ObtenerFechasTermino(ByVal idSol As String, ByRef fechaEstimada As String, ByRef fechaProvisional As String)
        Dim conexion As New Conexion.SQLServer
        Dim query As String = String.Empty
        Try
            query = "SELECT SS.F_FECH_TERMINO_SERVICIO, (SELECT COUNT(SS1.N_ID_NIVELES_SERVICIO) FROM  BDS_R_GR_SOLICITUD_SERVICIO SS1 WHERE SS1.N_ID_SOLICITUD_SERVICIO = {0}) AS TOTAL " & _
                    "FROM BDS_R_GR_SOLICITUD_SERVICIO SS WHERE SS.N_ID_SOLICITUD_SERVICIO = {0} AND SS.F_FECH_TERMINO_SERVICIO IS NOT NULL " & _
                    "ORDER BY SS.F_FECH_TERMINO_SERVICIO DESC "
            query = String.Format(query, idSol)
            Dim dt As DataTable = conexion.ConsultarDT(query)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    If dt.Rows.Count = CInt(dr("TOTAL")) Then
                        fechaEstimada = CStr(dr("F_FECH_TERMINO_SERVICIO"))
                        Exit For
                    ElseIf dt.Rows.Count < CInt(dr("TOTAL")) Then
                        fechaProvisional = CStr(dr("F_FECH_TERMINO_SERVICIO"))
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Sub

    Public Function ObtenerUltimaFechaIniIngeniero() As DataTable
        Dim conexion As New Conexion.SQLServer
        Dim query As String = String.Empty
        Try
            query = "SELECT TOP 1 F_FECH_FECHA_HORA_TAREA FROM BDS_D_TI_AGENDA " & _
                    "WHERE T_ID_INGENIERO LIKE '{0}' ORDER BY F_FECH_FECHA_HORA_TAREA DESC "
            query = String.Format(query, Me.Ingeniero)
            Dim dt As DataTable = conexion.ConsultarDT(query)
            Return dt
        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    Public Function ObtenerUltimaFechaIniIngeniero(ByVal conexion As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As DataTable
        Dim query As String = String.Empty
        Try
            query = "SELECT TOP 1 F_FECH_FECHA_HORA_TAREA FROM BDS_D_TI_AGENDA " & _
                    "WHERE T_ID_INGENIERO LIKE '{0}' ORDER BY F_FECH_FECHA_HORA_TAREA DESC "
            query = String.Format(query, Me.Ingeniero)
            Dim dt As DataTable = conexion.ConsultarDTConTransaccion(query, tran)
            Return dt
        Catch ex As Exception

            Throw ex

        End Try

    End Function


    Public Sub CargarDatos()
        'Dim conexion As New Conexion.SQLServer
        'Try

        '    Dim listCampos As New List(Of String)
        '    Dim listValores As New List(Of Object)
        '    Dim dr As SqlClient.SqlDataReader = Nothing

        '    listCampos.Add("N_ID_AREA") : listValores.Add(Me.Identificador)

        '    Try


        '        Existe = conexion.BuscarUnRegistro("BDS_C_GR_AREA", listCampos, listValores)

        '        If Existe Then

        '            dr = conexion.ConsultarRegistrosDR("BDS_C_GR_AREA", listCampos, listValores)

        '            If dr.Read() Then

        '                Me.Descripcion = CStr(dr("T_DSC_AREA"))
        '                Me.IdSubdirector = CStr(dr("T_ID_SUBDIRECTOR"))
        '                Me.IdBackup = CStr(dr("T_ID_BACKUP"))
        '                Me.Vigente = CBool(dr("B_FLAG_VIG"))



        '                dr = conexion.ConsultarRegistrosDR("BDS_C_GR_USUARIO", New List(Of String) From {"T_ID_USUARIO"}, New List(Of Object) From {Me.IdSubdirector})

        '                If dr.Read() Then
        '                    Me.Subdirector = CStr(dr("T_DSC_NOMBRE")) & " " & CStr(dr("T_DSC_APELLIDO"))
        '                End If

        '                dr = conexion.ConsultarRegistrosDR("BDS_C_GR_USUARIO", New List(Of String) From {"T_ID_USUARIO"}, New List(Of Object) From {Me.IdBackup})

        '                If dr.Read Then
        '                    Me.Backup = CStr(dr("T_DSC_NOMBRE")) & " " & CStr(dr("T_DSC_APELLIDO"))
        '                End If



        '            End If

        '        End If
        '    Catch ex As Exception
        '        Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        '    Finally
        '        If dr IsNot Nothing Then
        '            If Not dr.IsClosed Then
        '                dr.Close() : dr = Nothing
        '            End If
        '        End If
        '    End Try

        'Catch ex As Exception
        '    Throw ex
        'Finally

        '    If Not IsNothing(conexion) Then
        '        conexion.CerrarConexion()
        '    End If

        'End Try
    End Sub

    Public Function ObtenerSiguienteIdentificador() As Integer

        Dim resultado As Integer = 1



        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_AGENDA) + 1) N_ID_AGENDA FROM " & TblAgenda)

            If dr.Read() Then

                If IsDBNull(dr("N_ID_AGENDA")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_AGENDA"))
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
    ''' Obtener el siguiente identificador dentro de transaccion para una insercion en bloque
    ''' </summary>
    ''' <param name="conexion">Objeto conexion</param>
    ''' <param name="tran">Objeto Transaccion</param>
    ''' <returns>Siguiente identificador</returns>
    ''' <remarks></remarks>
    Public Function ObtenerSiguienteIdentificador(ByVal conexion As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Integer

        Dim resultado As Integer = 1
        Dim dr As SqlClient.SqlDataReader = Nothing

        Try

            dr = conexion.ConsultarDRConTransaccion("SELECT (MAX(N_ID_AGENDA) + 1) N_ID_AGENDA FROM " & TblAgenda, tran)

            If dr.Read() Then

                If IsDBNull(dr("N_ID_AGENDA")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_AGENDA"))
                End If

            End If

            Return resultado

        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(dr) Then
                dr.Close()
                dr = Nothing
            End If
        End Try

    End Function

    Public Function ObtenerSiguienteIdentificadorRAgenda() As Integer

        Dim resultado As Integer = 1



        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_REGISTRO_AGENDA) + 1) N_ID_REGISTRO_AGENDA FROM " & TblRegistroAgenda)

            If dr.Read() Then

                If IsDBNull(dr("N_ID_REGISTRO_AGENDA")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_REGISTRO_AGENDA"))
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

    Public Function AusenciaIngeniero(ByVal fechaInicio As DateTime) As Boolean
        Dim resultado As Boolean = True
        Dim query As String = "SELECT A.N_ID_TIPO_TAREA " & _
                              "FROM BDS_D_TI_AGENDA AS A INNER JOIN " & _
                              "BDS_D_TI_REGISTRO_AGENDA AS RA ON A.N_ID_REGISTRO_AGENDA = RA.N_ID_REGISTRO_AGENDA " & _
                              "WHERE (A.T_ID_INGENIERO = '" & Me.Ingeniero & "') AND (ISNULL(RA.N_ID_TIPO_AUSENCIA, - 1) <> - 1) AND (A.N_ID_TIPO_TAREA = 2) AND " & _
                              "(A.F_FECH_FECHA_HORA_TAREA = '" & fechaInicio.ToString("yyyy/MM/dd HH:mm:ss") & "')"
        Dim conexion As New Conexion.SQLServer

        Try
            Dim dt As DataTable = conexion.ConsultarDT(query)
            If dt.Rows.Count > 0 Then
                resultado = True
            Else
                resultado = False
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

    Public Function ObtenerConsecutivoSoporte() As Integer
        Dim resultado As Integer = 1

        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_CONSECUTIVO_SOPORTE) + 1) N_ID_CONSECUTIVO_SOPORTE FROM " & TblAgenda & _
                                                                     " WHERE T_ID_INGENIERO = '" & Me.Ingeniero & "' AND F_FECH_FECHA_HORA_TAREA = '" & _
                                                                     Me.FechIniReg.ToString("yyyy/MM/dd HH:mm:ss") & "'")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_CONSECUTIVO_SOPORTE")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_CONSECUTIVO_SOPORTE"))
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

    Public Function AgregarRegistroAgenda() As Boolean

        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Try

            Dim bitacora As New Conexion.Bitacora("Alta de registro en agenda " & Me.IdRegistroAgenda & " de asignación de servicio " & Me.IdNivelServicio, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_REGISTRO_AGENDA") : listValores.Add(Me.IdRegistroAgenda)
            listCampos.Add("T_ID_INGENIERO_SOLICITANTE") : listValores.Add(Me.IngSolict)
            listCampos.Add("T_ID_AUTORIZADOR") : listValores.Add(Me.Autorizador)
            listCampos.Add("N_ID_TIPO_REGISTRO") : listValores.Add(Me.TipoRegistro)
            listCampos.Add("N_ID_TIPO_ACTIVIDAD") : listValores.Add(Me.TipoActividad)
            listCampos.Add("N_ID_TIPO_AUSENCIA") : listValores.Add(Me.TipoAusencia)
            listCampos.Add("B_FLAG_CICLICA") : listValores.Add(Me.Ciclica)
            listCampos.Add("F_FECH_INICIO_REGISTRO") : listValores.Add(Me.FechIniReg)
            listCampos.Add("B_FLAG_LUNES") : listValores.Add(Me.Lunes)
            listCampos.Add("B_FLAG_MARTES") : listValores.Add(Me.Martes)
            listCampos.Add("B_FLAG_MIERCOLES") : listValores.Add(Me.Miercoles)
            listCampos.Add("B_FLAG_JUEVES") : listValores.Add(Me.Jueves)
            listCampos.Add("B_FLAG_VIERNES") : listValores.Add(Me.Viernes)
            listCampos.Add("T_DSC_NOTAS_REGISTRO") : listValores.Add(Me.NotaRegistro)
            listCampos.Add("T_DSC_NOTAS_AUTORIZADOR") : listValores.Add(Me.NotaAutorizador)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(Me.VigFlag)
            listCampos.Add("B_FLAG_APROBADO") : listValores.Add(Me.FlagAprobado)
            listCampos.Add("F_FECH_REGISTRO") : listValores.Add(Me.FechReg)
            listCampos.Add("F_FECH_AUTORIZACION") : listValores.Add(Me.FechAutorizacion)

            resultado = conexion.Insertar(TblRegistroAgenda, listCampos, listValores)
            bitacora.Insertar(TblRegistroAgenda, listCampos, listValores, resultado, "Error al registrar una entrada en la agenda")
            bitacora.Finalizar(resultado)

        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally


            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If


        End Try

        Return resultado

    End Function

    Public Function AgregarAgendaServicios() As Boolean

        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Try

            Dim bitacora As New Conexion.Bitacora("Alta de registro en agenda " & Me.Identificador & " de asignación de servicio " & Me.IdNivelServicio, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_AGENDA") : listValores.Add(Me.Identificador)
            listCampos.Add("T_ID_INGENIERO") : listValores.Add(Me.Ingeniero)
            listCampos.Add("F_FECH_FECHA_HORA_TAREA") : listValores.Add(Me.FechaHoraTarea)
            listCampos.Add("N_ID_TIPO_TAREA") : listValores.Add(Me.IdTipoTarea)
            If Me.IdTipoTarea = TipoTarea.NivelServicio Then
                listCampos.Add("N_ID_SOLICITUD_SERVICIO") : listValores.Add(Me.IdSolicitud)
                listCampos.Add("N_ID_NIVELES_SERVICIO") : listValores.Add(Me.IdNivelServicio)
                listCampos.Add("N_ID_REGISTRO_AGENDA") : listValores.Add(Me.IdRegistroAgendaFK)
            ElseIf Me.IdTipoTarea = TipoTarea.RegistroAgenda Then
                listCampos.Add("N_ID_SOLICITUD_SERVICIO") : listValores.Add(DBNull.Value)
                listCampos.Add("N_ID_NIVELES_SERVICIO") : listValores.Add(DBNull.Value)
                listCampos.Add("N_ID_REGISTRO_AGENDA") : listValores.Add(Me.IdRegistroAgendaFK)
            End If

            If Me.idConsecutivo > 0 Then
                listCampos.Add("N_ID_CONSECUTIVO_SOPORTE") : listValores.Add(Me.idConsecutivo)
            Else
                listCampos.Add("N_ID_CONSECUTIVO_SOPORTE") : listValores.Add(DBNull.Value)
            End If

            listCampos.Add("T_DSC_TAREA_AGENDA") : listValores.Add(Me.TareaAgenda)

            resultado = conexion.Insertar(TblAgenda, listCampos, listValores)
            bitacora.Insertar(TblAgenda, listCampos, listValores, resultado, "Error al registrar una entrada en la agenda")
            bitacora.Finalizar(resultado)

        Catch ex As Exception

            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally


            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If


        End Try

        Return resultado

    End Function

    ''' <summary>
    ''' Agrega un registro a Agenda para un ingeniero en una hora especifica
    ''' </summary>
    ''' <param name="con">Objeto Conexion</param>
    ''' <param name="tran">Objeto transaccion</param>
    ''' <param name="bitac">Objeto Bitacora</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AgregarAgendaServicios(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction, ByVal bitac As Conexion.Bitacora) As Boolean

        Dim resultado As Boolean = False

        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)

        listCampos.Add("N_ID_AGENDA") : listValores.Add(Me.Identificador)
        listCampos.Add("T_ID_INGENIERO") : listValores.Add(Me.Ingeniero)
        listCampos.Add("F_FECH_FECHA_HORA_TAREA") : listValores.Add(Me.FechaHoraTarea)
        listCampos.Add("N_ID_TIPO_TAREA") : listValores.Add(Me.IdTipoTarea)
        If Me.IdTipoTarea = TipoTarea.NivelServicio Then
            listCampos.Add("N_ID_SOLICITUD_SERVICIO") : listValores.Add(Me.IdSolicitud)
            listCampos.Add("N_ID_NIVELES_SERVICIO") : listValores.Add(Me.IdNivelServicio)
            listCampos.Add("N_ID_REGISTRO_AGENDA") : listValores.Add(Me.IdRegistroAgendaFK)
        ElseIf Me.IdTipoTarea = TipoTarea.RegistroAgenda Then
            listCampos.Add("N_ID_SOLICITUD_SERVICIO") : listValores.Add(DBNull.Value)
            listCampos.Add("N_ID_NIVELES_SERVICIO") : listValores.Add(DBNull.Value)
            listCampos.Add("N_ID_REGISTRO_AGENDA") : listValores.Add(Me.IdRegistroAgendaFK)
        End If

        If Me.idConsecutivo > 0 Then
            listCampos.Add("N_ID_CONSECUTIVO_SOPORTE") : listValores.Add(Me.IdRegistroAgendaFK)
        Else
            listCampos.Add("N_ID_CONSECUTIVO_SOPORTE") : listValores.Add(DBNull.Value)
        End If

        Try
            listCampos.Add("T_DSC_TAREA_AGENDA") : listValores.Add(Me.TareaAgenda)

            resultado = con.InsertarConTransaccion(TblAgenda, listCampos, listValores, tran)
            bitac.InsertarConTransaccion(TblAgenda, listCampos, listValores, resultado, "Registro de entrada en la agenda")

        Catch ex As Exception

            resultado = False
            bitac.InsertarConTransaccion(TblAgenda, listCampos, listValores, resultado, "Error al registrar una entrada en la agenda")
            Throw ex

        End Try

        Return resultado

    End Function

    ''' <summary>
    ''' Elimina un registro de agenda
    ''' </summary>
    ''' <param name="idAgenda">Id</param>
    ''' <param name="con">Objeto Conexion</param>
    ''' <param name="bitacora">Objeto Botacora</param>
    ''' <param name="tran">Objeto Transaccion</param>
    ''' <remarks></remarks>
    Public Sub Eliminar(ByVal idAgenda As Integer, ByVal con As Conexion.SQLServer, ByVal bitacora As Conexion.Bitacora, ByVal tran As SqlClient.SqlTransaction)
        Dim lstCamposCondicion As New List(Of String)
        Dim lstCamposCondicionValores As New List(Of Object)
        lstCamposCondicion.Add("N_ID_AGENDA") : lstCamposCondicionValores.Add(idAgenda)
        Try
            Dim res = con.EliminarConTransaccion(TblAgenda, lstCamposCondicion, lstCamposCondicionValores, tran)
            If Not bitacora Is Nothing Then
                bitacora.EliminarConTransaccion(TblAgenda, lstCamposCondicion, lstCamposCondicionValores, res, "")
            End If

        Catch ex As Exception
            If Not bitacora Is Nothing Then
                bitacora.EliminarConTransaccion(TblAgenda, lstCamposCondicion, lstCamposCondicionValores, False, ex.Message)
            End If

            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex
        End Try
    End Sub

    Public Shared Function FechaDisponibleAgenda(ByVal fecha As DateTime, ByVal Ingeniero As String) As Boolean
        Dim disponible As Boolean = False
        Dim strQuery As String = "SELECT * FROM BDS_D_TI_AGENDA " + _
                                " WHERE T_ID_INGENIERO = '" + Ingeniero + "' " + _
                                " AND F_FECH_FECHA_HORA_TAREA = '" + fecha.ToString("yyyy-MM-dd HH:mm") + "'"
        Dim con As Conexion.SQLServer = Nothing
        Try
            con = New Conexion.SQLServer
            Dim dt As DataTable = con.ConsultarDT(strQuery)
            If dt.Rows.Count > 0 Then
                disponible = False
            Else
                disponible = True
            End If
        Catch ex As Exception
            disponible = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw New Exception With {.Source = "Error al Validar Fecha Disponible"}
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
                con = Nothing
            End If
        End Try
        Return disponible
    End Function

    ''' <summary>
    ''' Determina si una fecha dada esta disponible
    ''' </summary>
    ''' <param name="fecha">Fecha</param>
    ''' <param name="Ingeniero">Ingeniero a validar</param>
    ''' <param name="con">Objeto conexion</param>
    ''' <param name="tran">Objeto Transaccion</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FechaDisponibleAgenda(ByVal fecha As DateTime, ByVal Ingeniero As String,
                                                 ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
        Dim disponible As Boolean = False
        Dim strQuery As String = "SELECT * FROM BDS_D_TI_AGENDA " + _
                                " WHERE T_ID_INGENIERO = '" + Ingeniero + "' " + _
                                " AND F_FECH_FECHA_HORA_TAREA = '" + fecha.ToString("yyyy-MM-dd HH:mm") + "'"
        Try
            Dim dt As DataTable = con.ConsultarDTConTransaccion(strQuery, tran)
            If dt.Rows.Count > 0 Then
                disponible = False
            Else
                disponible = True
            End If
        Catch ex As Exception
            disponible = False
            Throw New Exception With {.Source = "Error al Validar Fecha Disponible"}
        End Try
        Return disponible
    End Function

    Public Sub EliminarGrupoTareas(ByVal ingeniero As String, ByVal idSol As Integer, ByVal idNivel As Integer)
        Dim resultado As Boolean
        Dim con As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Eliminación de tareas de la agenda " & Me.Identificador, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)
        listCamposCondicion.Add("T_ID_INGENIERO") : listValoresCondicion.Add(ingeniero)
        listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(idSol)
        listCamposCondicion.Add("N_ID_NIVELES_SERVICIO") : listValoresCondicion.Add(idNivel)
        Try
            resultado = con.Eliminar(TblAgenda, listCamposCondicion, listValoresCondicion)
        Catch ex As Exception
            resultado = False
            bitacora.Eliminar(TblAgenda, listCamposCondicion, listValoresCondicion, resultado, "")
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Se eliminan las tareas asiganadas a un ingeniero de un nivel de servicio
    ''' </summary>
    ''' <param name="ingeniero">Ingeniero que tiene asignadas las tareas</param>
    ''' <param name="idSol">Id de la solicitud</param>
    ''' <param name="idNivel">Id de nivel de servicio</param>
    ''' <param name="con">Objeto conexion</param>
    ''' <param name="bitac">Objeto bitacora</param>
    ''' <param name="tran">objeto transaccion</param>
    ''' <remarks></remarks>
    Public Sub EliminarGrupoTareas(ByVal ingeniero As String, ByVal idSol As Integer, ByVal idNivel As Integer, ByVal con As Conexion.SQLServer, ByVal bitac As Conexion.Bitacora, ByVal tran As SqlClient.SqlTransaction)
        Dim resultado As Boolean
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)
        listCamposCondicion.Add("T_ID_INGENIERO") : listValoresCondicion.Add(ingeniero)
        listCamposCondicion.Add("N_ID_SOLICITUD_SERVICIO") : listValoresCondicion.Add(idSol)
        listCamposCondicion.Add("N_ID_NIVELES_SERVICIO") : listValoresCondicion.Add(idNivel)
        Try            
            resultado = con.EliminarConTransaccion(TblAgenda, listCamposCondicion, listValoresCondicion, tran)
            bitac.EliminarConTransaccion(TblAgenda, listCamposCondicion, listValoresCondicion, resultado, "")
        Catch ex As Exception
            resultado = False
            bitac.EliminarConTransaccion(TblAgenda, listCamposCondicion, listValoresCondicion, resultado, ex.ToString)
            Throw ex
        End Try
    End Sub

    Public Function ObtenerDatosAgenda(ByVal idSol As Integer, ByVal idNivel As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer
        Dim query As String = String.Empty
        Dim idReg As Integer = 1
        Try
            query = "SELECT TOP 1 N_ID_REGISTRO_AGENDA, T_ID_INGENIERO, T_DSC_TAREA_AGENDA  FROM BDS_D_TI_AGENDA WHERE N_ID_SOLICITUD_SERVICIO = {0} AND N_ID_NIVELES_SERVICIO = {1}"
            query = String.Format(query, idSol, idNivel)
            Dim dt As DataTable = conexion.ConsultarDT(query)
            Return dt
        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try
    End Function
End Class
