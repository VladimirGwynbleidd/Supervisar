<Serializable()>
Public Class Encuesta
    Private Tabla As String = "BDS_C_GR_ENCUESTA"
    Private TablaAspectos As String = "BDS_R_GR_ENCUESTA_ASPECTOS_EVALUAR"
    Private TablaOpciones As String = "BDS_R_GR_ENCUESTA_OPCIONES"
    Private TablaRespuestaD As String = "BDS_D_TI_ENCUESTA_SOLICITUD_SERVICIO"
    Private TablaRespuestaR As String = "BDS_R_TI_ENCUESTA_SOLICITUD_SERVICIO"
#Region "Propiedades"

    Public Property Identificador As Integer
    Public Property Descripcion As String
    Public Property Vigente As Boolean
    Public Property InicioVigencia As Date
    Public Property FinVigencia As Date?
    Public Property Existe As Boolean = False
    Public Property ordenAsp As New List(Of String)
    Public Property idAsp As New List(Of String)
    Public Property ordenOpc As New List(Of String)
    Public Property idOpc As New List(Of String)

    'Propiedades para la respuesta
    Public Property FechRespuesta As Date? = Nothing
    Public Property RespAsp As String
    Public Property RespOpc As String

#End Region
#Region "Constructores"

    Public Sub New()

    End Sub

    Public Sub New(ByVal idEncuesta As Integer)
        Me.Identificador = idEncuesta
        CargarDatos()
    End Sub


#End Region
#Region "Metodos"

    ''' <summary>
    ''' Carga los datos de Servicio tomando el Identificador almacenado en la propiedad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim dr As SqlClient.SqlDataReader = Nothing

            listCampos.Add("N_ID_ENCUESTA") : listValores.Add(Me.Identificador)

            Try

                Existe = conexion.BuscarUnRegistro(Tabla, listCampos, listValores)

                If Existe Then

                    dr = conexion.ConsultarRegistrosDR(Tabla, listCampos, listValores)

                    If dr.Read() Then
                        Me.Descripcion = CStr(dr("T_DSC_ENCUESTA"))
                        Me.Vigente = Convert.ToBoolean(dr("B_FLAG_VIG"))
                        Me.InicioVigencia = Convert.ToDateTime(dr("F_FECH_INI_VIG"))

                        If Not IsDBNull(dr("F_FECH_FIN_VIG")) Then
                            Me.FinVigencia = Convert.ToDateTime(dr("F_FECH_FIN_VIG"))
                        Else
                            Me.FinVigencia = Nothing
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

    ''' <summary>
    ''' Obtiene todos los registros del catalogo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer

        Try

            Return conexion.ConsultarRegistrosDT(Tabla).DefaultView

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

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_ENCUESTA) + 1) N_ID_ENCUESTA FROM " + Tabla)

            If dr.Read() Then

                If IsDBNull(dr("N_ID_ENCUESTA")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_ENCUESTA"))
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
    ''' Obtiene id de la encuesta vigente
    ''' </summary>
    ''' <returns>Identificador siguiente</returns>
    ''' <remarks></remarks>
    Public Function ObtenerIDEncuestaVigente() As Integer

        Dim resultado As Integer = -1

        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT TOP 1 N_ID_ENCUESTA N_ID_ENCUESTA FROM " + Tabla + " WHERE B_FLAG_VIG = 1")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_ENCUESTA")) Then
                    resultado = -1
                Else
                    resultado = CInt(dr("N_ID_ENCUESTA"))
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
    ''' Obtiene los aspectos para una nueva encuesta
    ''' </summary>
    ''' <returns>Tabla de aspectos</returns>
    ''' <remarks></remarks>
    Public Function CargarAspectosNuevo() As DataTable

        Dim resultado As New DataTable
        resultado.Columns.Add("N_ID_ENCUESTA_ASPECTO_EVALUAR")
        resultado.Columns.Add("T_DSC_ASPECTO_EVALUAR")
        resultado.Columns.Add("N_ID_ORDEN")

        Dim conexion As New Conexion.SQLServer
        Dim sql As String = String.Empty

        Try
            sql = "SELECT A.*, RE.N_ID_ORDEN FROM dbo.BDS_C_GR_ENCUESTA E JOIN dbo.BDS_R_GR_ENCUESTA_ASPECTOS_EVALUAR RE ON E.N_ID_ENCUESTA = RE.N_ID_ENCUESTA " & _
                  "JOIN dbo.BDS_C_GR_ENCUESTA_ASPECTOS_EVALUAR A ON RE.N_ID_ENCUESTA_ASPECTO_EVALUAR = A.N_ID_ENCUESTA_ASPECTO_EVALUAR " & _
                  "WHERE E.N_ID_ENCUESTA = {0}  UNION SELECT *, NULL FROM dbo.BDS_C_GR_ENCUESTA_ASPECTOS_EVALUAR C " & _
                  "WHERE C.N_ID_ENCUESTA_ASPECTO_EVALUAR NOT IN ( Select A.N_ID_ENCUESTA_ASPECTO_EVALUAR " & _
                  "FROM dbo.BDS_C_GR_ENCUESTA E JOIN dbo.BDS_R_GR_ENCUESTA_ASPECTOS_EVALUAR RE ON E.N_ID_ENCUESTA = RE.N_ID_ENCUESTA " & _
                  "JOIN dbo.BDS_C_GR_ENCUESTA_ASPECTOS_EVALUAR A ON RE.N_ID_ENCUESTA_ASPECTO_EVALUAR = A.N_ID_ENCUESTA_ASPECTO_EVALUAR " & _
                  "WHERE E.N_ID_ENCUESTA = {0}) AND C.B_FLAG_VIG = 1"

            sql = String.Format(sql, Me.Identificador.ToString)

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR(sql)

            While dr.Read()
                Dim row As DataRow
                row = resultado.NewRow()
                row("N_ID_ENCUESTA_ASPECTO_EVALUAR") = dr("N_ID_ENCUESTA_ASPECTO_EVALUAR")
                row("T_DSC_ASPECTO_EVALUAR") = dr("T_DSC_ASPECTO_EVALUAR")
                row("N_ID_ORDEN") = dr("N_ID_ORDEN")
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

    ''' <summary>
    ''' Obtiene las opciones para una nueva encuesta
    ''' </summary>
    ''' <returns>Tabla de opciones</returns>
    ''' <remarks></remarks>
    Public Function CargarOpcionesNuevo() As DataTable

        Dim resultado As New DataTable
        resultado.Columns.Add("N_ID_ENCUESTA_OPCION")
        resultado.Columns.Add("T_DSC_OPCION")
        resultado.Columns.Add("N_ID_ORDEN")

        Dim conexion As New Conexion.SQLServer
        Dim sql As String = String.Empty

        Try

            sql = "SELECT A.*, RE.N_ID_ORDEN FROM dbo.BDS_C_GR_ENCUESTA E JOIN dbo.BDS_R_GR_ENCUESTA_OPCIONES RE ON E.N_ID_ENCUESTA = RE.N_ID_ENCUESTA " & _
                  "JOIN dbo.BDS_C_GR_ENCUESTA_OPCIONES A ON RE.N_ID_ENCUESTA_OPCION = A.N_ID_ENCUESTA_OPCION " & _
                  "WHERE E.N_ID_ENCUESTA = {0}  UNION SELECT *, NULL FROM dbo.BDS_C_GR_ENCUESTA_OPCIONES C " & _
                  "WHERE C.N_ID_ENCUESTA_OPCION NOT IN ( Select A.N_ID_ENCUESTA_OPCION " & _
                  "FROM dbo.BDS_C_GR_ENCUESTA E JOIN dbo.BDS_R_GR_ENCUESTA_OPCIONES RE ON E.N_ID_ENCUESTA = RE.N_ID_ENCUESTA " & _
                  "JOIN dbo.BDS_C_GR_ENCUESTA_OPCIONES A ON RE.N_ID_ENCUESTA_OPCION = A.N_ID_ENCUESTA_OPCION " & _
                  "WHERE E.N_ID_ENCUESTA = {0}) AND C.B_FLAG_VIG = 1"

            sql = String.Format(sql, Me.Identificador.ToString)

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR(sql)

            While dr.Read()
                Dim row As DataRow
                row = resultado.NewRow()
                row("N_ID_ENCUESTA_OPCION") = dr("N_ID_ENCUESTA_OPCION")
                row("T_DSC_OPCION") = dr("T_DSC_OPCION")
                row("N_ID_ORDEN") = dr("N_ID_ORDEN")
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

    ''' <summary>
    ''' Agrega el Servicio al catalogo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim bitacora As New Conexion.Bitacora("Registro de nueva Encuesta", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_ENCUESTA") : listValores.Add(Me.Identificador)
            listCampos.Add("T_DSC_ENCUESTA") : listValores.Add(Me.Descripcion)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(True)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            Try
                resultado = conexion.Insertar(Tabla, listCampos, listValores)
                If resultado Then
                    resultado = RegistrarAspectos()
                    If resultado Then
                        resultado = RegistrarOpciones()
                    End If
                End If
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

    ''' <summary>
    ''' Actualiza un Tipo de Servicio
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualización de Encuesta", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("T_DSC_ENCUESTA") : listValores.Add(Me.Descripcion)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(True)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_ENCUESTA") : listValoresCondicion.Add(Me.Identificador)

            Try
                resultado = conexion.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion)
                bitacora.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")
                If resultado Then
                    resultado = RegistrarAspectos()
                        If resultado Then
                            resultado = RegistrarOpciones()
                        End If
                End If
            Catch ex As Exception
                resultado = False
                bitacora.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, ex.Message.ToString)
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
    ''' Agrega los aspectos asociados a la encuesta
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RegistrarAspectos() As Boolean

        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim bitacora As New Conexion.Bitacora("Registro de nueva Encuesta - Aspectos", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_ENCUESTA") : listValores.Add(Me.Identificador)
            resultado = conexion.Eliminar(TablaAspectos, listCampos, listValores)
            bitacora.Eliminar(TablaAspectos, listCampos, listValores, resultado, "")

            Dim i As Integer
            For i = 0 To Me.idAsp.Count - 1
                listCampos.Clear()
                listValores.Clear()
                listCampos.Add("N_ID_ENCUESTA") : listValores.Add(Me.Identificador)
                listCampos.Add("N_ID_ENCUESTA_ASPECTO_EVALUAR") : listValores.Add(Me.idAsp.Item(i))
                listCampos.Add("N_ID_ORDEN") : listValores.Add(Me.ordenAsp.Item(i))
                Try
                    resultado = conexion.Insertar(TablaAspectos, listCampos, listValores)
                    bitacora.Insertar(TablaAspectos, listCampos, listValores, resultado, "")
                Catch ex As Exception
                    resultado = False
                    bitacora.Insertar(Tabla, listCampos, listValores, resultado, ex.Message.ToString)
                    Throw ex
                End Try
            Next i

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
    ''' Agrega las opciones asociados a la encuesta
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RegistrarOpciones() As Boolean

        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim bitacora As New Conexion.Bitacora("Registro de nueva Encuesta - Opciones", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_ENCUESTA") : listValores.Add(Me.Identificador)
            resultado = conexion.Eliminar(TablaOpciones, listCampos, listValores)
            bitacora.Eliminar(TablaOpciones, listCampos, listValores, resultado, "")

            Dim i As Integer
            For i = 0 To Me.idOpc.Count - 1
                listCampos.Clear()
                listValores.Clear()
                listCampos.Add("N_ID_ENCUESTA") : listValores.Add(Me.Identificador)
                listCampos.Add("N_ID_ENCUESTA_OPCION") : listValores.Add(Me.idOpc.Item(i))
                listCampos.Add("N_ID_ORDEN") : listValores.Add(Me.ordenOpc.Item(i))
                Try
                    resultado = conexion.Insertar(TablaOpciones, listCampos, listValores)
                    bitacora.Insertar(TablaOpciones, listCampos, listValores, resultado, "")
                Catch ex As Exception
                    resultado = False
                    bitacora.Insertar(Tabla, listCampos, listValores, resultado, ex.Message.ToString)
                    Throw ex
                End Try
            Next i

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
    ''' Termina la vigencia de un registro
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Baja() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Borrar Encuesta", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("B_FLAG_VIG") : listValores.Add(False)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_ENCUESTA") : listValoresCondicion.Add(Me.Identificador)

            Try
                resultado = conexion.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion)
                bitacora.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, ex.Message.ToString)
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
    ''' Consulta si existe una encuesta vigente
    ''' </summary>
    ''' <returns>Datos de la encuesta vigente</returns>
    ''' <remarks></remarks>
    Public Function ConsultaEncuestaVigente() As DataTable

        Dim resultado As New DataTable
        resultado.Columns.Add("N_ID_ENCUESTA")
        resultado.Columns.Add("T_DSC_ENCUESTA")

        Dim conexion As New Conexion.SQLServer
        Dim sql As String = String.Empty

        Try

            sql = "SELECT N_ID_ENCUESTA, T_DSC_ENCUESTA FROM BDS_C_GR_ENCUESTA WHERE B_FLAG_VIG = 1"
            sql = String.Format(sql, Me.Identificador.ToString)
            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR(sql)

            While dr.Read()
                Dim row As DataRow
                row = resultado.NewRow()
                row("N_ID_ENCUESTA") = dr("N_ID_ENCUESTA")
                row("T_DSC_ENCUESTA") = dr("T_DSC_ENCUESTA")
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

    ''' <summary>
    ''' Desactiva la encuesta indicada
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DesactivarEncuesta() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Desactivación de Encuesta", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("B_FLAG_VIG") : listValores.Add(False)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_ENCUESTA") : listValoresCondicion.Add(Me.Identificador)

            Try
                resultado = conexion.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion)
                bitacora.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")
                If resultado Then
                    resultado = RegistrarAspectos()
                    If resultado Then
                        resultado = RegistrarOpciones()
                    End If
                End If
            Catch ex As Exception
                resultado = False
                bitacora.Actualizar(Tabla, listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, ex.Message.ToString)
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

    Public Function ObtenerAspectosEncuestaActiva() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT AE.N_ID_ENCUESTA_ASPECTO_EVALUAR, AE.T_DSC_ASPECTO_EVALUAR FROM BDS_C_GR_ENCUESTA_ASPECTOS_EVALUAR AE " & _
                                    "INNER JOIN BDS_R_GR_ENCUESTA_ASPECTOS_EVALUAR RE ON RE.N_ID_ENCUESTA_ASPECTO_EVALUAR = AE.N_ID_ENCUESTA_ASPECTO_EVALUAR " & _
                                    "INNER JOIN BDS_C_GR_ENCUESTA EN ON EN.N_ID_ENCUESTA = RE.N_ID_ENCUESTA " & _
                                    "WHERE EN.B_FLAG_VIG = 1 ORDER BY RE.N_ID_ORDEN"
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

    Public Function ObtenerOpcionesEncuestaActiva() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT EO.N_ID_ENCUESTA_OPCION, EO.T_DSC_OPCION FROM BDS_C_GR_ENCUESTA_OPCIONES EO " & _
                                    "INNER JOIN BDS_R_GR_ENCUESTA_OPCIONES RO ON RO.N_ID_ENCUESTA_OPCION = EO.N_ID_ENCUESTA_OPCION " & _
                                    "INNER JOIN BDS_C_GR_ENCUESTA EN ON EN.N_ID_ENCUESTA = RO.N_ID_ENCUESTA " & _
                                    "WHERE EN.B_FLAG_VIG = 1 ORDER BY RO.N_ID_ORDEN"
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

    Public Function RegistrarRespuestasD(ByVal idSol As String, ByRef bitacora As Conexion.Bitacora) As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer        
        Try
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            listCampos.Add("N_ID_SOLICITUD_SERVICIO") : listValores.Add(idSol)
            listCampos.Add("N_ID_ENCUESTA") : listValores.Add(Me.Identificador)
            listCampos.Add("F_FECH_RESPUESTA") : listValores.Add(Me.FechRespuesta)
            Try
                resultado = conexion.Insertar(TablaRespuestaD, listCampos, listValores)
                bitacora.Insertar(TablaRespuestaD, listCampos, listValores, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Insertar(TablaRespuestaD, listCampos, listValores, resultado, ex.Message.ToString)
                Throw ex
            End Try
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

    Public Function RegistrarRespuestasR(ByVal idSol As String, ByRef bitacora As Conexion.Bitacora) As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer        
        Try
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            listCampos.Add("N_ID_SOLICITUD_SERVICIO") : listValores.Add(idSol)
            listCampos.Add("N_ID_ENCUESTA_ASPECTO_EVALUAR") : listValores.Add(Me.RespAsp)
            listCampos.Add("N_ID_ENCUESTA_OPCION") : listValores.Add(Me.RespOpc)
            Try
                resultado = conexion.Insertar(TablaRespuestaR, listCampos, listValores)
                bitacora.Insertar(TablaRespuestaR, listCampos, listValores, resultado, "")
            Catch ex As Exception
                resultado = False
                bitacora.Insertar(TablaRespuestaR, listCampos, listValores, resultado, ex.Message.ToString)
                Throw ex
            End Try
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

    Public Function ObtenerRespuestasEnc(ByVal idSol As String, ByVal idAsp As String) As DataTable
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataTable
            Dim query As String = "SELECT N_ID_ENCUESTA_OPCION FROM BDS_R_TI_ENCUESTA_SOLICITUD_SERVICIO RSS " & _
                                    "WHERE RSS.N_ID_SOLICITUD_SERVICIO = {0} AND RSS.N_ID_ENCUESTA_ASPECTO_EVALUAR = {1}"
            query = String.Format(query, idSol, idAsp)
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

    Public Function ObtenerAspectosEncuestaResp(ByVal idSol As String) As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT AE.N_ID_ENCUESTA_ASPECTO_EVALUAR, AE.T_DSC_ASPECTO_EVALUAR FROM BDS_C_GR_ENCUESTA_ASPECTOS_EVALUAR AE " & _
                                    "INNER JOIN BDS_R_GR_ENCUESTA_ASPECTOS_EVALUAR RE ON RE.N_ID_ENCUESTA_ASPECTO_EVALUAR = AE.N_ID_ENCUESTA_ASPECTO_EVALUAR " & _
                                    "INNER JOIN BDS_C_GR_ENCUESTA EN ON EN.N_ID_ENCUESTA = RE.N_ID_ENCUESTA " & _
                                    "INNER JOIN BDS_D_TI_ENCUESTA_SOLICITUD_SERVICIO ETI ON ETI.N_ID_ENCUESTA = EN.N_ID_ENCUESTA " & _
                                    "WHERE ETI.N_ID_SOLICITUD_SERVICIO = {0} ORDER BY RE.N_ID_ORDEN"
            query = String.Format(query, idSol)
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

    Public Function ObtenerOpcionesEncuestaResp(ByVal idSol As String) As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT EO.N_ID_ENCUESTA_OPCION, EO.T_DSC_OPCION FROM BDS_C_GR_ENCUESTA_OPCIONES EO " & _
                                    "INNER JOIN BDS_R_GR_ENCUESTA_OPCIONES RO ON RO.N_ID_ENCUESTA_OPCION = EO.N_ID_ENCUESTA_OPCION " & _
                                    "INNER JOIN BDS_C_GR_ENCUESTA EN ON EN.N_ID_ENCUESTA = RO.N_ID_ENCUESTA " & _
                                    "INNER JOIN BDS_D_TI_ENCUESTA_SOLICITUD_SERVICIO ETI ON ETI.N_ID_ENCUESTA = EN.N_ID_ENCUESTA " & _
                                    "WHERE ETI.N_ID_SOLICITUD_SERVICIO = {0} ORDER BY RO.N_ID_ORDEN"
            query = String.Format(query, idSol)
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


#End Region
End Class
