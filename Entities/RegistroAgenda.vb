'- Fecha de creación: 31/03/2014
'- Fecha de modificación:  ##/##/####
'- Nombre del Responsable: Rafael Rodriguez
'- Empresa: Softtek
'- Clase de Registro Agenda

<Serializable()>
Public Class RegistroAgenda
    Private Tabla As String = "BDS_D_TI_REGISTRO_AGENDA"

    ''' <summary>
    ''' Contiene los tipos de registro disponibles
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TipoRegistro
        Actividad = 1
        Ausencia = 2
    End Enum

#Region "Propiedades"
    Public Property Id As Integer = 0
    Public Property IngenieroSolicta As String
    Public Property Autorizador As String
    Public Property IdTipoRegistro As TipoRegistro
    Public Property TipoActividad As Integer?
    Public Property TipoAusencia As Integer?
    Public Property Ciclica As Boolean
    Public Property FechIniReg As DateTime
    Public Property FechFinReg As DateTime
    Public Property Lunes As Boolean?
    Public Property Martes As Boolean?
    Public Property Miercoles As Boolean?
    Public Property Jueves As Boolean?
    Public Property Viernes As Boolean?
    Public Property NotaRegistro As String
    Public Property NotaAutorizador As String
    Public Property Vigente As Boolean
    Public Property Aprobado As Boolean?
    Public Property FechReg As DateTime
    Public Property FechAutorizacion As DateTime
#End Region

#Region "Constructores"
    Public Sub New()

    End Sub

    Public Sub New(ByVal id As Integer)
        Me.Id = id
        CargaDatos()
    End Sub

    Public Sub New(ByVal id As Integer, ByVal ingenieroSolicita As String, ByVal autorizador As String, ByVal idTipoRegistro As TipoRegistro,
                   ByVal tipoActividad As Integer?, ByVal tipoAusencia As Integer?, ByVal ciclica As Boolean, ByVal fechiniReg As DateTime,
                   ByVal fechFinReg As DateTime, ByVal lunes As Boolean?, ByVal martes As Boolean?, ByVal miercoles As Boolean?, ByVal jueves As Boolean?,
                   ByVal viernes As Boolean?, ByVal notasRegistro As String, ByVal notasAutorizador As String, ByVal vigente As Boolean,
                   ByVal aprobado As Boolean?, ByVal fechReg As DateTime, ByVal fechaAutorizacion As DateTime)
        Me.Id = id
        Me.IngenieroSolicta = ingenieroSolicita
        Me.Autorizador = autorizador
        Me.IdTipoRegistro = idTipoRegistro
        Me.TipoActividad = tipoActividad
        Me.TipoAusencia = tipoAusencia
        Me.Ciclica = ciclica
        Me.FechIniReg = fechiniReg
        Me.FechFinReg = fechFinReg
        Me.Lunes = lunes
        Me.Martes = martes
        Me.Miercoles = miercoles
        Me.Jueves = jueves
        Me.Viernes = viernes
        Me.NotaRegistro = notasRegistro
        Me.NotaAutorizador = notasAutorizador
        Me.Vigente = vigente
        Me.Aprobado = aprobado
        Me.FechReg = fechReg
        Me.FechAutorizacion = fechaAutorizacion
    End Sub
#End Region

#Region "Consultas"

    ''' <summary>
    ''' Obtiene el siguiente identificador de Registro en agenda
    ''' </summary>
    ''' <returns>Entero con el siguiente identificador</returns>
    ''' <remarks></remarks>
    Public Function ObtenerSiguienteIdentificador() As Integer
        Dim resultado As Integer = 1
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_REGISTRO_AGENDA) + 1) N_ID_REGISTRO_AGENDA FROM " & Tabla)
            If dr.Read() Then
                If IsDBNull(dr("N_ID_REGISTRO_AGENDA")) Then
                    resultado = 1
                Else
                    resultado = Convert.ToInt32(dr("N_ID_REGISTRO_AGENDA"))
                End If
            End If
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return resultado
    End Function

    ''' <summary>
    ''' Carga los datos del identificador en el objeto
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargaDatos()
        Dim strQuery As String = "SELECT N_ID_REGISTRO_AGENDA, T_ID_INGENIERO_SOLICITANTE, T_ID_AUTORIZADOR, N_ID_TIPO_REGISTRO, N_ID_TIPO_ACTIVIDAD, " + _
                                "     N_ID_TIPO_AUSENCIA, B_FLAG_CICLICA, F_FECH_INICIO_REGISTRO, F_FECH_FIN_REGISTRO, B_FLAG_LUNES, B_FLAG_MARTES, " + _
                                "     B_FLAG_MIERCOLES, B_FLAG_JUEVES, B_FLAG_VIERNES, T_DSC_NOTAS_REGISTRO, T_DSC_NOTAS_AUTORIZADOR, B_FLAG_VIG, " + _
                                "     B_FLAG_APROBADO, F_FECH_REGISTRO, F_FECH_AUTORIZACION " + _
                                " FROM " + Tabla + _
                                " WHERE N_ID_REGISTRO_AGENDA = " + Me.Id.ToString()


        Dim conexion As Conexion.SQLServer = Nothing
        Try
            conexion = New Conexion.SQLServer()

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)
            If dt.Rows.Count > 0 Then
                CargaRegistroAgenda(dt.Rows(0))
            End If
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene todos los registros de Registro agenda para mostrar en la bandeja
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenRegistrosAgenda() As DataView
        Dim dv As New DataView
        Dim strQuery = "SELECT RA.N_ID_REGISTRO_AGENDA, " + _
                      "     RA.T_ID_INGENIERO_SOLICITANTE, US.T_DSC_NOMBRE + ' ' + US.T_DSC_APELLIDO + ' ' + US.T_DSC_APELLIDO_AUX as SOLICITANTE, " + _
                      "     RA.T_ID_AUTORIZADOR, " + _
                      "     CASE WHEN RA.T_ID_AUTORIZADOR IS NULL THEN 'N/A' " + _
                      "     ELSE UA.T_DSC_NOMBRE + ' ' + UA.T_DSC_APELLIDO + ' ' + UA.T_DSC_APELLIDO_AUX END AS AUTORIZADOR, " + _
                      "     RA.N_ID_TIPO_REGISTRO, " + _
                      "     CASE RA.N_ID_TIPO_REGISTRO WHEN 1 THEN 'Actividad' " + _
                      "     WHEN 2 THEN 'Ausencia' " + _
                      "     ELSE 'N/A' END AS TIPO_REGISTRO, " + _
                      "     RA.F_FECH_INICIO_REGISTRO, RA.F_FECH_FIN_REGISTRO, RA.B_FLAG_VIG, RA.B_FLAG_APROBADO " + _
                      " FROM BDS_D_TI_REGISTRO_AGENDA RA " + _
                      " INNER JOIN BDS_C_GR_USUARIO US ON RA.T_ID_INGENIERO_SOLICITANTE = US.T_ID_USUARIO " + _
                      " LEFT JOIN BDS_C_GR_USUARIO UA ON RA.T_ID_AUTORIZADOR = UA.T_ID_USUARIO " + _
                      " LEFT JOIN BDS_D_TI_AGENDA A ON A.N_ID_REGISTRO_AGENDA = RA.N_ID_REGISTRO_AGENDA AND A.N_ID_TIPO_TAREA = 1 " + _
                      " WHERE A.N_ID_REGISTRO_AGENDA IS NULL " +
                      " ORDER BY RA.N_ID_REGISTRO_AGENDA DESC "
        Dim conexion As Conexion.SQLServer = Nothing
        Try
            conexion = New Conexion.SQLServer()
            dv = conexion.ConsultarDT(strQuery).DefaultView

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return dv
    End Function


#End Region

#Region "Persistencia"

    ''' <summary>
    ''' Inserta un registro en Registro Agenda
    ''' </summary>
    ''' <returns>True si se incerto correctamente, de lo contrario false</returns>
    ''' <remarks></remarks>
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)

        Me.Id = ObtenerSiguienteIdentificador()

        listCampos.Add("N_ID_REGISTRO_AGENDA") : listValores.Add(Me.Id)
        listCampos.Add("T_ID_INGENIERO_SOLICITANTE") : listValores.Add(Me.IngenieroSolicta)
        listCampos.Add("T_ID_AUTORIZADOR") : listValores.Add(Me.Autorizador)
        listCampos.Add("N_ID_TIPO_REGISTRO") : listValores.Add(Me.IdTipoRegistro)
        If Me.IdTipoRegistro = TipoRegistro.Actividad Then
            listCampos.Add("N_ID_TIPO_ACTIVIDAD") : listValores.Add(Me.TipoActividad)
            listCampos.Add("N_ID_TIPO_AUSENCIA") : listValores.Add(DBNull.Value)
        ElseIf Me.IdTipoRegistro = TipoRegistro.Ausencia Then
            listCampos.Add("N_ID_TIPO_ACTIVIDAD") : listValores.Add(DBNull.Value)
            listCampos.Add("N_ID_TIPO_AUSENCIA") : listValores.Add(Me.TipoAusencia)
        End If
        listCampos.Add("B_FLAG_CICLICA") : listValores.Add(Me.Ciclica)
        listCampos.Add("F_FECH_INICIO_REGISTRO") : listValores.Add(Me.FechIniReg)
        listCampos.Add("F_FECH_FIN_REGISTRO") : listValores.Add(Me.FechFinReg)
        If Me.Ciclica Then
            listCampos.Add("B_FLAG_LUNES") : listValores.Add(Me.Lunes)
            listCampos.Add("B_FLAG_MARTES") : listValores.Add(Me.Martes)
            listCampos.Add("B_FLAG_MIERCOLES") : listValores.Add(Me.Miercoles)
            listCampos.Add("B_FLAG_JUEVES") : listValores.Add(Me.Jueves)
            listCampos.Add("B_FLAG_VIERNES") : listValores.Add(Me.Viernes)
        Else
            listCampos.Add("B_FLAG_LUNES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_MARTES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_MIERCOLES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_JUEVES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_VIERNES") : listValores.Add(DBNull.Value)
        End If
        listCampos.Add("T_DSC_NOTAS_REGISTRO") : listValores.Add(Me.NotaRegistro)
        listCampos.Add("T_DSC_NOTAS_AUTORIZADOR") : listValores.Add(DBNull.Value)
        listCampos.Add("B_FLAG_VIG") : listValores.Add(Me.Vigente)
        If Me.Aprobado Is Nothing Then
            listCampos.Add("B_FLAG_APROBADO") : listValores.Add(DBNull.Value)
        Else
            listCampos.Add("B_FLAG_APROBADO") : listValores.Add(Me.Aprobado)
        End If
        listCampos.Add("F_FECH_REGISTRO") : listValores.Add(Me.FechReg)
        If IsNothing(Me.FechAutorizacion) Then
            listCampos.Add("F_FECH_AUTORIZACION") : listValores.Add(DBNull.Value)
        Else
            listCampos.Add("F_FECH_AUTORIZACION") : listValores.Add(Me.FechAutorizacion)
        End If

        Dim conexion As Conexion.SQLServer = Nothing
        Dim bitacora As Conexion.Bitacora = Nothing

        Try
            conexion = New Conexion.SQLServer
            bitacora = New Conexion.Bitacora("Alta de registro agenda", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            resultado = conexion.Insertar(Tabla, listCampos, listValores)
            bitacora.Insertar(Tabla, listCampos, listValores, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Insertar(Tabla, listCampos, listValores, resultado, ex.Message)
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex

        Finally
            If Not IsNothing(bitacora) Then
                Try : bitacora.Finalizar(resultado) : Catch : End Try
            End If

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return resultado

    End Function

    ''' <summary>
    ''' Inserta un registro en Registro Agenda
    ''' </summary>
    ''' <param name="con">Objeto Conexion</param>
    ''' <param name="bitacora">Objeto Bitacora</param>
    ''' <param name="tran">Objeto transaccion</param>
    ''' <remarks></remarks>
    Public Sub Agregar(ByVal con As Conexion.SQLServer, ByVal bitacora As Conexion.Bitacora, ByVal tran As SqlClient.SqlTransaction)

        Dim resultado As Boolean = False
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)

        Me.Id = ObtenerSiguienteIdentificador()

        listCampos.Add("N_ID_REGISTRO_AGENDA") : listValores.Add(Me.Id)
        listCampos.Add("T_ID_INGENIERO_SOLICITANTE") : listValores.Add(Me.IngenieroSolicta)
        If IsNothing(Me.Autorizador) OrElse Me.Autorizador = "" Then
            listCampos.Add("T_ID_AUTORIZADOR") : listValores.Add(DBNull.Value)
        Else
            listCampos.Add("T_ID_AUTORIZADOR") : listValores.Add(Me.Autorizador)
        End If
        listCampos.Add("N_ID_TIPO_REGISTRO") : listValores.Add(Me.IdTipoRegistro)
        If Me.IdTipoRegistro = TipoRegistro.Actividad Then
            listCampos.Add("N_ID_TIPO_ACTIVIDAD") : listValores.Add(Me.TipoActividad)
            listCampos.Add("N_ID_TIPO_AUSENCIA") : listValores.Add(DBNull.Value)
        ElseIf Me.IdTipoRegistro = TipoRegistro.Ausencia Then
            listCampos.Add("N_ID_TIPO_ACTIVIDAD") : listValores.Add(DBNull.Value)
            listCampos.Add("N_ID_TIPO_AUSENCIA") : listValores.Add(Me.TipoAusencia)
        End If
        listCampos.Add("B_FLAG_CICLICA") : listValores.Add(Me.Ciclica)
        listCampos.Add("F_FECH_INICIO_REGISTRO") : listValores.Add(Me.FechIniReg)
        listCampos.Add("F_FECH_FIN_REGISTRO") : listValores.Add(Me.FechFinReg)
        If Me.Ciclica Then
            listCampos.Add("B_FLAG_LUNES") : listValores.Add(Me.Lunes)
            listCampos.Add("B_FLAG_MARTES") : listValores.Add(Me.Martes)
            listCampos.Add("B_FLAG_MIERCOLES") : listValores.Add(Me.Miercoles)
            listCampos.Add("B_FLAG_JUEVES") : listValores.Add(Me.Jueves)
            listCampos.Add("B_FLAG_VIERNES") : listValores.Add(Me.Viernes)
        Else
            listCampos.Add("B_FLAG_LUNES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_MARTES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_MIERCOLES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_JUEVES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_VIERNES") : listValores.Add(DBNull.Value)
        End If
        listCampos.Add("T_DSC_NOTAS_REGISTRO") : listValores.Add(Me.NotaRegistro)
        listCampos.Add("T_DSC_NOTAS_AUTORIZADOR") : listValores.Add(DBNull.Value)
        listCampos.Add("B_FLAG_VIG") : listValores.Add(Me.Vigente)
        If Me.Aprobado Is Nothing Then
            listCampos.Add("B_FLAG_APROBADO") : listValores.Add(DBNull.Value)
        Else
            listCampos.Add("B_FLAG_APROBADO") : listValores.Add(Me.Aprobado)
        End If
        listCampos.Add("F_FECH_REGISTRO") : listValores.Add(Me.FechReg)
        If Me.FechAutorizacion = Nothing Then
            listCampos.Add("F_FECH_AUTORIZACION") : listValores.Add(DBNull.Value)
        Else
            listCampos.Add("F_FECH_AUTORIZACION") : listValores.Add(Me.FechAutorizacion)
        End If

        Try

            resultado = con.InsertarConTransaccion(Tabla, listCampos, listValores, tran)
            bitacora.InsertarConTransaccion(Tabla, listCampos, listValores, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.InsertarConTransaccion(Tabla, listCampos, listValores, resultado, ex.Message)
            Throw ex

        End Try

    End Sub

    ''' <summary>
    ''' Modifica un registro agenda
    ''' </summary>
    ''' <returns>True si la actualizacion es correcta, de otro modo false</returns>
    ''' <remarks></remarks>
    Public Function Modificar() As Boolean
        Dim resultado As Boolean = False
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        listCamposCondicion.Add("N_ID_REGISTRO_AGENDA") : listValoresCondicion.Add(Me.Id)

        If IsNothing(Me.Autorizador) OrElse Me.Autorizador = "" Then
            listCampos.Add("T_ID_AUTORIZADOR") : listValores.Add(DBNull.Value)
        Else
            listCampos.Add("T_ID_AUTORIZADOR") : listValores.Add(Me.Autorizador)
        End If
        listCampos.Add("N_ID_TIPO_REGISTRO") : listValores.Add(Me.IdTipoRegistro)
        If Me.IdTipoRegistro = TipoRegistro.Actividad Then
            listCampos.Add("N_ID_TIPO_ACTIVIDAD") : listValores.Add(Me.TipoActividad)
            listCampos.Add("N_ID_TIPO_AUSENCIA") : listValores.Add(DBNull.Value)
        ElseIf Me.IdTipoRegistro = TipoRegistro.Ausencia Then
            listCampos.Add("N_ID_TIPO_ACTIVIDAD") : listValores.Add(DBNull.Value)
            listCampos.Add("N_ID_TIPO_AUSENCIA") : listValores.Add(Me.TipoAusencia)
        End If
        listCampos.Add("B_FLAG_CICLICA") : listValores.Add(Me.Ciclica)
        listCampos.Add("F_FECH_INICIO_REGISTRO") : listValores.Add(Me.FechIniReg)
        listCampos.Add("F_FECH_FIN_REGISTRO") : listValores.Add(Me.FechFinReg)
        If Me.Ciclica Then
            listCampos.Add("B_FLAG_LUNES") : listValores.Add(Me.Lunes)
            listCampos.Add("B_FLAG_MARTES") : listValores.Add(Me.Martes)
            listCampos.Add("B_FLAG_MIERCOLES") : listValores.Add(Me.Miercoles)
            listCampos.Add("B_FLAG_JUEVES") : listValores.Add(Me.Jueves)
            listCampos.Add("B_FLAG_VIERNES") : listValores.Add(Me.Viernes)
        Else
            listCampos.Add("B_FLAG_LUNES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_MARTES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_MIERCOLES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_JUEVES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_VIERNES") : listValores.Add(DBNull.Value)
        End If
        listCampos.Add("T_DSC_NOTAS_REGISTRO") : listValores.Add(Me.NotaRegistro)
        listCampos.Add("T_DSC_NOTAS_AUTORIZADOR") : listValores.Add(DBNull.Value)
        listCampos.Add("B_FLAG_VIG") : listValores.Add(Me.Vigente)
        If Me.Aprobado Is Nothing Then
            listCampos.Add("B_FLAG_APROBADO") : listValores.Add(DBNull.Value)
        Else
            listCampos.Add("B_FLAG_APROBADO") : listValores.Add(Me.Aprobado)
        End If
        If Me.FechAutorizacion = Nothing Then
            listCampos.Add("F_FECH_AUTORIZACION") : listValores.Add(DBNull.Value)
        Else
            listCampos.Add("F_FECH_AUTORIZACION") : listValores.Add(Me.FechAutorizacion)
        End If

        Dim conexion As Conexion.SQLServer = Nothing
        Dim bitacora As Conexion.Bitacora = Nothing

        Try
            conexion = New Conexion.SQLServer
            bitacora = New Conexion.Bitacora("Modificacion de registro agenda", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            resultado = conexion.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Insertar(Tabla, listCampos, listValores, resultado, ex.Message)
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally
            If Not IsNothing(bitacora) Then
                Try : bitacora.Finalizar(resultado) : Catch : End Try
            End If

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return resultado
    End Function

    ''' <summary>
    ''' Modifica un registro de agenda
    ''' </summary>
    ''' <param name="con">Objeto Conexion</param>
    ''' <param name="bitacora">Objeto Bitacora</param>
    ''' <param name="tran">Objeto Transaccion</param>
    ''' <remarks></remarks>
    Public Sub Modificar(ByVal con As Conexion.SQLServer, ByVal bitacora As Conexion.Bitacora, ByVal tran As SqlClient.SqlTransaction)
        Dim resultado As Boolean = False
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        listCamposCondicion.Add("N_ID_REGISTRO_AGENDA") : listValoresCondicion.Add(Me.Id)

        If IsNothing(Me.Autorizador) OrElse Me.Autorizador = "" Then
            listCampos.Add("T_ID_AUTORIZADOR") : listValores.Add(DBNull.Value)
        Else
            listCampos.Add("T_ID_AUTORIZADOR") : listValores.Add(Me.Autorizador)
        End If
        listCampos.Add("N_ID_TIPO_REGISTRO") : listValores.Add(Me.IdTipoRegistro)
        If Me.IdTipoRegistro = TipoRegistro.Actividad Then
            listCampos.Add("N_ID_TIPO_ACTIVIDAD") : listValores.Add(Me.TipoActividad)
            listCampos.Add("N_ID_TIPO_AUSENCIA") : listValores.Add(DBNull.Value)
        ElseIf Me.IdTipoRegistro = TipoRegistro.Ausencia Then
            listCampos.Add("N_ID_TIPO_ACTIVIDAD") : listValores.Add(DBNull.Value)
            listCampos.Add("N_ID_TIPO_AUSENCIA") : listValores.Add(Me.TipoAusencia)
        End If
        listCampos.Add("B_FLAG_CICLICA") : listValores.Add(Me.Ciclica)
        listCampos.Add("F_FECH_INICIO_REGISTRO") : listValores.Add(Me.FechIniReg)
        listCampos.Add("F_FECH_FIN_REGISTRO") : listValores.Add(Me.FechFinReg)
        If Me.Ciclica Then
            listCampos.Add("B_FLAG_LUNES") : listValores.Add(Me.Lunes)
            listCampos.Add("B_FLAG_MARTES") : listValores.Add(Me.Martes)
            listCampos.Add("B_FLAG_MIERCOLES") : listValores.Add(Me.Miercoles)
            listCampos.Add("B_FLAG_JUEVES") : listValores.Add(Me.Jueves)
            listCampos.Add("B_FLAG_VIERNES") : listValores.Add(Me.Viernes)
        Else
            listCampos.Add("B_FLAG_LUNES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_MARTES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_MIERCOLES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_JUEVES") : listValores.Add(DBNull.Value)
            listCampos.Add("B_FLAG_VIERNES") : listValores.Add(DBNull.Value)
        End If
        listCampos.Add("T_DSC_NOTAS_REGISTRO") : listValores.Add(Me.NotaRegistro)
        If Me.NotaAutorizador Is Nothing OrElse Me.NotaAutorizador = "" Then
            listCampos.Add("T_DSC_NOTAS_AUTORIZADOR") : listValores.Add(DBNull.Value)
        Else
            listCampos.Add("T_DSC_NOTAS_AUTORIZADOR") : listValores.Add(Me.NotaAutorizador)
        End If
        listCampos.Add("B_FLAG_VIG") : listValores.Add(Me.Vigente)
        If Me.Aprobado Is Nothing Then
            listCampos.Add("B_FLAG_APROBADO") : listValores.Add(DBNull.Value)
        Else
            listCampos.Add("B_FLAG_APROBADO") : listValores.Add(Me.Aprobado)
        End If
        If Me.FechAutorizacion = Nothing Then
            listCampos.Add("F_FECH_AUTORIZACION") : listValores.Add(DBNull.Value)
        Else
            listCampos.Add("F_FECH_AUTORIZACION") : listValores.Add(Me.FechAutorizacion)
        End If

        Try

            resultado = con.ActualizarConTransaccion(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, tran)
            If Not bitacora Is Nothing Then
                bitacora.ActualizarConTransaccion(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")
            End If


        Catch ex As Exception

            resultado = False
            If Not bitacora Is Nothing Then
                bitacora.ActualizarConTransaccion(Tabla, listCampos, listValores, resultado, ex.Message)
            End If
            Throw ex

        End Try

    End Sub

    ''' <summary>
    ''' Baja logica de registro agenda
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Baja() As Boolean
        Dim resultado As Boolean = False
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        listCamposCondicion.Add("N_ID_REGISTRO_AGENDA") : listValoresCondicion.Add(Me.Id)

        listCampos.Add("B_FLAG_VIG") : listValores.Add(0)

        Dim conexion As Conexion.SQLServer = Nothing
        Dim bitacora As Conexion.Bitacora = Nothing

        Try
            conexion = New Conexion.SQLServer
            bitacora = New Conexion.Bitacora("Baja de registro agenda", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            resultado = conexion.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Insertar(Tabla, listCampos, listValores, resultado, ex.Message)
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally
            If Not IsNothing(bitacora) Then
                Try : bitacora.Finalizar(resultado) : Catch : End Try
            End If

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return resultado
    End Function

    ''' <summary>
    ''' Baja logica de registro agenda, dentro de una transaccion
    ''' </summary>
    ''' <param name="con">Objeto Conexion</param>
    ''' <param name="bitacora">Objeto Bitacora</param>
    ''' <param name="tran">Objeto transaccion</param>
    ''' <remarks></remarks>
    Public Sub Baja(ByVal con As Conexion.SQLServer, ByVal bitacora As Conexion.Bitacora, ByVal tran As SqlClient.SqlTransaction)
        Dim resultado As Boolean = False
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        listCamposCondicion.Add("N_ID_REGISTRO_AGENDA") : listValoresCondicion.Add(Me.Id)

        listCampos.Add("B_FLAG_VIG") : listValores.Add(0)


        Try
            con = New Conexion.SQLServer
            bitacora = New Conexion.Bitacora("Baja de registro agenda " & Me.Id, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            resultado = con.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Insertar(Tabla, listCampos, listValores, resultado, ex.Message)
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex
        End Try
    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Carga los datos del objeto actual con los datos del datarow
    ''' </summary>
    ''' <param name="dr">DataRow de la tabla BDS_D_TI_REGISTRO_AGENDA</param>
    ''' <remarks></remarks>
    Public Sub CargaRegistroAgenda(ByVal dr As DataRow)
        Me.Id = Convert.ToInt32(dr("N_ID_REGISTRO_AGENDA"))
        Me.IngenieroSolicta = dr("T_ID_INGENIERO_SOLICITANTE").ToString
        If Not IsDBNull(dr("T_ID_AUTORIZADOR")) Then
            Me.Autorizador = dr("T_ID_AUTORIZADOR").ToString
        Else
            Me.Autorizador = Nothing
        End If
        Me.IdTipoRegistro = Convert.ToInt32(dr("N_ID_TIPO_REGISTRO"))
        If Not IsDBNull(dr("N_ID_TIPO_ACTIVIDAD")) Then
            Me.TipoActividad = Convert.ToInt32(dr("N_ID_TIPO_ACTIVIDAD"))
        Else
            Me.TipoActividad = Nothing
        End If
        If Not IsDBNull(dr("N_ID_TIPO_AUSENCIA")) Then
            Me.TipoAusencia = Convert.ToInt32(dr("N_ID_TIPO_AUSENCIA"))
        Else
            Me.TipoAusencia = Nothing
        End If
        Me.Ciclica = Convert.ToBoolean(dr("B_FLAG_CICLICA"))
        Me.FechIniReg = Convert.ToDateTime(dr("F_FECH_INICIO_REGISTRO"))
        If Not IsDBNull(dr("F_FECH_FIN_REGISTRO")) Then
            Me.FechFinReg = Convert.ToDateTime(dr("F_FECH_FIN_REGISTRO"))
        Else
            Me.FechFinReg = Nothing
        End If
        If Not IsDBNull(dr("B_FLAG_LUNES")) Then
            Me.Lunes = Convert.ToBoolean(dr("B_FLAG_LUNES"))
        Else
            Me.Lunes = Nothing
        End If
        If Not IsDBNull(dr("B_FLAG_MARTES")) Then
            Me.Martes = Convert.ToBoolean(dr("B_FLAG_MARTES"))
        Else
            Me.Martes = Nothing
        End If
        If Not IsDBNull(dr("B_FLAG_MIERCOLES")) Then
            Me.Miercoles = Convert.ToBoolean(dr("B_FLAG_MIERCOLES"))
        Else
            Me.Miercoles = Nothing
        End If
        If Not IsDBNull(dr("B_FLAG_JUEVES")) Then
            Me.Jueves = Convert.ToBoolean(dr("B_FLAG_JUEVES"))
        Else
            Me.Jueves = Nothing
        End If
        If Not IsDBNull(dr("B_FLAG_VIERNES")) Then
            Me.Viernes = Convert.ToBoolean(dr("B_FLAG_VIERNES"))
        Else
            Me.Viernes = Nothing
        End If
        Me.NotaRegistro = dr("T_DSC_NOTAS_REGISTRO").ToString
        If Not IsDBNull(dr("T_DSC_NOTAS_AUTORIZADOR")) Then
            Me.NotaAutorizador = dr("T_DSC_NOTAS_AUTORIZADOR").ToString
        Else
            Me.NotaAutorizador = Nothing
        End If
        Me.Vigente = Convert.ToBoolean(dr("B_FLAG_VIG"))
        If Not IsDBNull(dr("B_FLAG_APROBADO")) Then
            Me.Aprobado = Convert.ToBoolean(dr("B_FLAG_APROBADO"))
        Else
            Me.Aprobado = Nothing
        End If
        Me.FechReg = Convert.ToDateTime(dr("F_FECH_REGISTRO"))
        If Not IsDBNull(dr("F_FECH_AUTORIZACION")) Then
            Me.FechAutorizacion = Convert.ToDateTime(dr("F_FECH_AUTORIZACION"))
        Else
            Me.FechAutorizacion = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Crea un objeto Registro agenda a partir de un datarow
    ''' </summary>
    ''' <param name="dr">DataRow de la tabla BDS_D_TI_REGISTRO_AGENDA</param>
    ''' <returns>Objeto RegistroAgenda</returns>
    ''' <remarks></remarks>
    Public Function CreaRegistroAgenda(ByVal dr As DataRow) As RegistroAgenda
        Dim objRegistroAgenda As New RegistroAgenda()
        objRegistroAgenda.Id = Convert.ToInt32(dr("N_ID_REGISTRO_AGENDA"))
        objRegistroAgenda.IngenieroSolicta = dr("T_ID_INGENIERO_SOLICITANTE").ToString
        If Not IsDBNull(dr("T_ID_AUTORIZADOR")) Then
            objRegistroAgenda.Autorizador = dr("T_ID_AUTORIZADOR").ToString
        Else
            objRegistroAgenda.Autorizador = Nothing
        End If
        objRegistroAgenda.IdTipoRegistro = Convert.ToInt32(dr("N_ID_TIPO_REGISTRO"))
        If Not IsDBNull(dr("N_ID_TIPO_ACTIVIDAD")) Then
            objRegistroAgenda.TipoActividad = Convert.ToInt32(dr("N_ID_TIPO_ACTIVIDAD"))
        Else
            objRegistroAgenda.TipoActividad = Nothing
        End If
        If Not IsDBNull(dr("N_ID_TIPO_AUSENCIA")) Then
            objRegistroAgenda.TipoAusencia = Convert.ToInt32(dr("N_ID_TIPO_AUSENCIA"))
        Else
            objRegistroAgenda.TipoAusencia = Nothing
        End If
        objRegistroAgenda.Ciclica = Convert.ToBoolean(dr("B_FLAG_CICLICA"))
        objRegistroAgenda.FechIniReg = Convert.ToDateTime(dr("F_FECH_INICIO_REGISTRO"))
        If Not IsDBNull(dr("F_FECH_FIN_REGISTRO")) Then
            objRegistroAgenda.FechFinReg = Convert.ToDateTime(dr("F_FECH_FIN_REGISTRO"))
        Else
            objRegistroAgenda.FechFinReg = Nothing
        End If
        If Not IsDBNull(dr("B_FLAG_LUNES")) Then
            objRegistroAgenda.Lunes = Convert.ToBoolean(dr("B_FLAG_LUNES"))
        Else
            objRegistroAgenda.Lunes = Nothing
        End If
        If Not IsDBNull(dr("B_FLAG_MARTES")) Then
            objRegistroAgenda.Martes = Convert.ToBoolean(dr("B_FLAG_MARTES"))
        Else
            objRegistroAgenda.Martes = Nothing
        End If
        If Not IsDBNull(dr("B_FLAG_MIERCOLES")) Then
            objRegistroAgenda.Miercoles = Convert.ToBoolean(dr("B_FLAG_MIERCOLES"))
        Else
            objRegistroAgenda.Miercoles = Nothing
        End If
        If Not IsDBNull(dr("B_FLAG_JUEVES")) Then
            objRegistroAgenda.Jueves = Convert.ToBoolean(dr("B_FLAG_JUEVES"))
        Else
            objRegistroAgenda.Jueves = Nothing
        End If
        If Not IsDBNull(dr("B_FLAG_VIERNES")) Then
            objRegistroAgenda.Viernes = Convert.ToBoolean(dr("B_FLAG_VIERNES"))
        Else
            objRegistroAgenda.Viernes = Nothing
        End If
        objRegistroAgenda.NotaRegistro = dr("T_DSC_NOTAS_REGISTRO").ToString
        If Not IsDBNull(dr("T_DSC_NOTAS_AUTORIZADOR")) Then
            objRegistroAgenda.NotaAutorizador = dr("T_DSC_NOTAS_AUTORIZADOR").ToString
        Else
            objRegistroAgenda.NotaAutorizador = Nothing
        End If
        objRegistroAgenda.Vigente = Convert.ToBoolean(dr("B_FLAG_VIG"))
        If Not IsDBNull(dr("B_FLAG_APROBADO")) Then
            objRegistroAgenda.Aprobado = Convert.ToBoolean(dr("B_FLAG_APROBADO"))
        Else
            objRegistroAgenda.Aprobado = Nothing
        End If
        objRegistroAgenda.FechReg = Convert.ToDateTime(dr("F_FECH_REGISTRO"))
        If Not IsDBNull(dr("F_FECH_AUTORIZACION")) Then
            objRegistroAgenda.FechAutorizacion = Convert.ToDateTime(dr("F_FECH_AUTORIZACION"))
        Else
            objRegistroAgenda.FechAutorizacion = Nothing
        End If
        Return objRegistroAgenda
    End Function

#End Region

End Class
