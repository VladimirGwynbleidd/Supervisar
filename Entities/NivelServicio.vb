
<Serializable()> _
Public Class NivelServicio

    Public Property Identificador As String

    Public Property IdentificadorServicio As String

    Public Property IdentificadorAplicativo
    Public Property IdentificadorArea
    Public Property IdentificadorFlujo
    Public Property IngenieroResponsable
    Public Property IngenieroBackup
    Public Property Descripcion
    Public Property TiempoEjecucion
    Public Property IdentificadorTipoServicio

    Public Property Existe As Boolean = False
    Public Property Vigente As Boolean = False



#Region "Constructores"
    Public Sub New()

    End Sub

    Public Sub New(ByVal idNivelServicio As Integer)
        Me.Identificador = idNivelServicio

        CargarDatos()
    End Sub
    Public Sub New(ByVal dscNivelServicio As String)
        Me.Descripcion = dscNivelServicio

        CargarDatosPorDescripcion()
    End Sub

#End Region

    Public Sub CargarDatosPorDescripcion()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("T_DSC_NIVELES_SERVICIO") : listValores.Add(Me.Descripcion)

            Existe = conexion.BuscarUnRegistro("BDS_C_GR_NIVELES_SERVICIO", listCampos, listValores)

            If Existe Then

                Dim dr As SqlClient.SqlDataReader = conexion.ConsultarRegistrosDR("BDS_C_GR_NIVELES_SERVICIO", listCampos, listValores)

                If dr.Read() Then

                    Me.IdentificadorTipoServicio = CStr(dr("N_ID_TIPO_SERVICIO"))
                    Me.IdentificadorServicio = CStr(dr("N_ID_SERVICIO"))
                    Me.IdentificadorAplicativo = CStr(dr("N_ID_APLICATIVO"))
                    Me.IdentificadorArea = CStr(dr("N_ID_AREA"))
                    Me.IdentificadorFlujo = CStr(dr("N_ID_FLUJO"))
                    Me.IngenieroResponsable = CStr(dr("T_ID_INGENIERO_RESPONSABLE"))
                    Me.IngenieroBackup = CStr(dr("T_ID_INGENIERO_BACKUP"))
                    Me.Descripcion = CStr(dr("T_DSC_NIVELES_SERVICIO"))
                    Me.TiempoEjecucion = CStr(dr("N_NUM_TIEMPO_EJECUCION"))
                    Me.Vigente = CBool(dr("B_FLAG_VIG"))
                    Me.Identificador = CInt(dr("N_ID_NIVELES_SERVICIO"))

                End If

            End If

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Sub


    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_NIVELES_SERVICIO") : listValores.Add(Me.Identificador)

            Existe = conexion.BuscarUnRegistro("BDS_C_GR_NIVELES_SERVICIO", listCampos, listValores)

            If Existe Then

                Dim dr As SqlClient.SqlDataReader = conexion.ConsultarRegistrosDR("BDS_C_GR_NIVELES_SERVICIO", listCampos, listValores)

                If dr.Read() Then

                    Me.IdentificadorTipoServicio = CStr(dr("N_ID_TIPO_SERVICIO"))
                    Me.IdentificadorServicio = CStr(dr("N_ID_SERVICIO"))
                    Me.IdentificadorAplicativo = CStr(dr("N_ID_APLICATIVO"))
                    Me.IdentificadorArea = CStr(dr("N_ID_AREA"))
                    Me.IdentificadorFlujo = CStr(dr("N_ID_FLUJO"))
                    Me.IngenieroResponsable = CStr(dr("T_ID_INGENIERO_RESPONSABLE"))
                    Me.IngenieroBackup = CStr(dr("T_ID_INGENIERO_BACKUP"))
                    Me.Descripcion = CStr(dr("T_DSC_NIVELES_SERVICIO"))
                    Me.TiempoEjecucion = CStr(dr("N_NUM_TIEMPO_EJECUCION"))
                    Me.Vigente = CBool(dr("B_FLAG_VIG"))

                End If

            End If

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Sub

    Public Function ObtenerSiguienteIdentificador() As Integer

        Dim resultado As Integer = 1



        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_NIVELES_SERVICIO) + 1) N_ID_NIVELES_SERVICIO FROM BDS_C_GR_NIVELES_SERVICIO")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_NIVELES_SERVICIO")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_NIVELES_SERVICIO"))
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


    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataView
            dv = conexion.ConsultarDT("SELECT NS.*, TS.T_DSC_TIPO_SERVICIO, S.T_DSC_SERVICIO, AP.T_DSC_APLICATIVO, A.T_DSC_AREA, F.T_DSC_FLUJO," + _
                                      "    U_IR.T_DSC_NOMBRE + ' ' + U_IR.T_DSC_APELLIDO + ' ' + U_IR.T_DSC_APELLIDO_AUX AS NOMBRE_RESPONSABLE, " + _
                                      "    U_IB.T_DSC_NOMBRE + ' ' + U_IB.T_DSC_APELLIDO + ' ' + U_IB.T_DSC_APELLIDO_AUX AS NOMBRE_BACKUP " + _
                                      " FROM BDS_C_GR_NIVELES_SERVICIO NS JOIN BDS_C_GR_TIPO_SERVICIO TS ON NS.N_ID_TIPO_SERVICIO = TS.N_ID_TIPO_SERVICIO JOIN" + _
                                      " BDS_C_GR_SERVICIOS S ON NS.N_ID_SERVICIO = S.N_ID_SERVICIO JOIN BDS_C_GR_APLICATIVO AP ON NS.N_ID_APLICATIVO = AP.N_ID_APLICATIVO JOIN" + _
                                      " BDS_C_GR_AREA A ON NS.N_ID_AREA = A.N_ID_AREA JOIN BDS_C_GR_FLUJO F ON NS.N_ID_FLUJO = F.N_ID_FLUJO " + _
                                      "LEFT JOIN BDS_C_GR_USUARIO U_IR ON U_IR.T_ID_USUARIO = NS.T_ID_INGENIERO_RESPONSABLE " + _
                                      "LEFT JOIN BDS_C_GR_USUARIO U_IB ON U_IB.T_ID_USUARIO = NS.T_ID_INGENIERO_BACKUP ").DefaultView
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

    Public Function ObtenerLista(ByVal Texto As String, ByRef SetCount As String) As List(Of String)
        Dim lstNiveles As New List(Of String)
        Dim conexion As New Conexion.SQLServer

        Try
            Dim dt As New DataTable
            Dim query As String = "SELECT  T_DSC_NIVELES_SERVICIO FROM BDS_C_GR_NIVELES_SERVICIO WHERE T_DSC_NIVELES_SERVICIO LIKE '%" & Texto & "%'"
            dt = conexion.ConsultarDT(query)
            If dt.Rows.Count > 0 Then
                For Each row In dt.Rows
                    lstNiveles.Add(If(Not IsDBNull(row("T_DSC_NIVELES_SERVICIO")), CStr(row("T_DSC_NIVELES_SERVICIO")), Nothing))
                Next
            End If
        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try
        Return lstNiveles
    End Function

    Public Function ObtenerAplicativosPorNivel() As DataView
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataView
            Dim query As String = "SELECT DISTINCT(NS.N_ID_APLICATIVO), AP.T_DSC_ACRONIMO_APLICATIVO FROM BDS_C_GR_NIVELES_SERVICIO NS INNER JOIN BDS_C_GR_APLICATIVO AP ON AP.N_ID_APLICATIVO = NS.N_ID_APLICATIVO WHERE NS.N_ID_TIPO_SERVICIO = {0} AND NS.B_FLAG_VIG = 1"
            query = String.Format(query, Me.IdentificadorTipoServicio)
            dv = conexion.ConsultarDT(query).DefaultView
            dv.Sort = "N_ID_APLICATIVO"
            Return dv

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    Public Function ObtenerNivelesDisp(ByVal idSol As String) As DataTable
        Dim conexion As New Conexion.SQLServer
        Dim idApp As Integer = 0
        Try

            Dim query As String = "SELECT NV.N_ID_NIVELES_SERVICIO, AP.T_DSC_ACRONIMO_APLICATIVO, SR.T_DSC_SERVICIO FROM BDS_C_GR_NIVELES_SERVICIO NV " & _
                                    "INNER JOIN BDS_C_GR_SERVICIOS SR ON SR.N_ID_SERVICIO = NV.N_ID_SERVICIO " & _
                                    "INNER JOIN BDS_C_GR_APLICATIVO AP ON AP.N_ID_APLICATIVO = NV.N_ID_APLICATIVO " & _
                                    "WHERE NV.N_ID_TIPO_SERVICIO = {0} AND NV.N_ID_APLICATIVO = {1} AND NV.N_ID_NIVELES_SERVICIO NOT IN " & _
                                    "(SELECT SS.N_ID_NIVELES_SERVICIO FROM BDS_R_GR_SOLICITUD_SERVICIO SS WHERE SS.N_ID_SOLICITUD_SERVICIO = {2} AND SS.B_DELETE_TEMP = 0) ORDER BY  NV.N_ID_NIVELES_SERVICIO"

            If Integer.Parse(Me.IdentificadorAplicativo) > 0 Then
                idApp = Integer.Parse(Me.IdentificadorAplicativo)
            End If

            query = String.Format(query, Me.IdentificadorTipoServicio, idApp.ToString, idSol)
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


    Public Function ObtenerNivelesAgr(ByVal idSol As String) As DataTable
        Dim conexion As New Conexion.SQLServer
        Try
            Dim query As String = "SELECT NV.N_ID_NIVELES_SERVICIO, AP.T_DSC_ACRONIMO_APLICATIVO, SR.T_DSC_SERVICIO, " & _
                                    "(SELECT TOP 1 AG.T_ID_INGENIERO FROM BDS_D_TI_AGENDA AG WHERE AG.N_ID_SOLICITUD_SERVICIO = {0} AND AG.N_ID_NIVELES_SERVICIO =  NV.N_ID_NIVELES_SERVICIO) AS ING_ASIG, " & _
                                    "(SELECT US.T_DSC_NOMBRE + ' ' + US.T_DSC_APELLIDO FROM BDS_C_GR_USUARIO US WHERE US.T_ID_USUARIO IN " & _
                                    "(SELECT TOP 1 AG.T_ID_INGENIERO FROM BDS_D_TI_AGENDA AG WHERE AG.N_ID_SOLICITUD_SERVICIO = {0} AND AG.N_ID_NIVELES_SERVICIO =  NV.N_ID_NIVELES_SERVICIO)) AS T_ID_INGENIERO_RESPONSABLE, " & _
                                    "N_ID_FLUJO, NV.T_ID_INGENIERO_RESPONSABLE AS ING_RESP, NV.T_ID_INGENIERO_BACKUP AS ING_BACKUP,  " & _
                                    "NV.N_NUM_TIEMPO_EJECUCION FROM BDS_C_GR_NIVELES_SERVICIO NV " & _
                                    "INNER JOIN BDS_C_GR_SERVICIOS SR ON SR.N_ID_SERVICIO = NV.N_ID_SERVICIO " & _
                                    "INNER JOIN BDS_C_GR_APLICATIVO AP ON AP.N_ID_APLICATIVO = NV.N_ID_APLICATIVO " & _
                                    "WHERE NV.N_ID_NIVELES_SERVICIO IN " & _
                                    "(SELECT SS.N_ID_NIVELES_SERVICIO FROM BDS_R_GR_SOLICITUD_SERVICIO SS WHERE SS.N_ID_SOLICITUD_SERVICIO = {0})  ORDER BY  NV.N_ID_NIVELES_SERVICIO"
            query = String.Format(query, idSol)
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

    Public Function ObtenerNivelesPaquete(ByVal idPaq As String, Optional ByVal idNiv As String = "-1") As DataTable
        Dim conexion As New Conexion.SQLServer
        Try
            Dim query As String = "SELECT NV.N_ID_NIVELES_SERVICIO, AP.T_DSC_ACRONIMO_APLICATIVO, SR.T_DSC_SERVICIO FROM BDS_R_GR_GRUPO_SERVICIOS GS " & _
                                    "INNER JOIN BDS_C_GR_NIVELES_SERVICIO NV ON NV.N_ID_NIVELES_SERVICIO = GS.N_ID_NIVELES_SERVICIO " & _
                                    "INNER JOIN BDS_C_GR_APLICATIVO AP ON NV.N_ID_APLICATIVO = AP.N_ID_APLICATIVO " & _
                                    "INNER JOIN BDS_C_GR_SERVICIOS SR ON NV.N_ID_SERVICIO = SR.N_ID_SERVICIO " & _
                                    "WHERE GS.N_ID_GRUPO_SERVICIO = {0}"
            query = String.Format(query, idPaq)
            If idPaq = 0 Then
                query += " AND NV.N_ID_NIVELES_SERVICIO <> " + idNiv
            End If
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

    Public Function ObtenerIngenierosArea(Optional ByVal IncluyeInge As Boolean = True) As DataTable
        Dim conexion As New Conexion.SQLServer
        Try

            Dim query As String = "SELECT DISTINCT (NV.T_ID_INGENIERO_RESPONSABLE), (US.T_DSC_NOMBRE + ' ' + US.T_DSC_APELLIDO + ' ' + US.T_DSC_APELLIDO_AUX) AS INGENIERO_ASIGNADO FROM BDS_C_GR_NIVELES_SERVICIO NV  " & _
                             "INNER JOIN BDS_C_GR_USUARIO US ON US.T_ID_USUARIO = NV.T_ID_INGENIERO_RESPONSABLE " & _
                             "WHERE NV.N_ID_AREA = {0} AND US.N_FLAG_VIG = 1 AND US.B_FLAG_ES_INGENIERO = 1 "

            'Dim query As String = "SELECT DISTINCT (NV.T_ID_INGENIERO_RESPONSABLE), (US.T_DSC_NOMBRE + ' ' + US.T_DSC_APELLIDO) AS INGENIERO_ASIGNADO FROM BDS_C_GR_NIVELES_SERVICIO NV  " & _
            '                        "INNER JOIN BDS_C_GR_USUARIO US ON US.T_ID_USUARIO = NV.T_ID_INGENIERO_RESPONSABLE " & _
            '                        "WHERE NV.N_ID_AREA = {0} AND US.N_FLAG_VIG = 1 AND US.B_FLAG_ES_INGENIERO = 1 AND NV.T_ID_INGENIERO_RESPONSABLE NOT LIKE '%{1}%'"

            If IncluyeInge Then
                query = query & "AND NV.T_ID_INGENIERO_RESPONSABLE NOT LIKE '%{1}%'"
            End If
            query = String.Format(query, Me.IdentificadorArea, Me.IngenieroResponsable)
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


    ''' <summary>
    ''' Registra un Nivel de Servicio
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Registro de nuevo nivel de servicio", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Usuario).IdentificadorUsuario)
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)

        Try

            listCampos.Add("N_ID_NIVELES_SERVICIO") : listValores.Add(Me.Identificador)
            listCampos.Add("N_ID_TIPO_SERVICIO") : listValores.Add(Me.IdentificadorTipoServicio)
            listCampos.Add("N_ID_SERVICIO") : listValores.Add(Me.IdentificadorServicio)
            listCampos.Add("N_ID_APLICATIVO") : listValores.Add(Me.IdentificadorAplicativo)
            listCampos.Add("N_ID_AREA") : listValores.Add(Me.IdentificadorArea)
            listCampos.Add("N_ID_FLUJO") : listValores.Add(Me.IdentificadorFlujo)
            listCampos.Add("T_ID_INGENIERO_RESPONSABLE") : listValores.Add(Me.IngenieroResponsable)
            listCampos.Add("T_ID_INGENIERO_BACKUP") : listValores.Add(Me.IngenieroBackup)
            listCampos.Add("T_DSC_NIVELES_SERVICIO") : listValores.Add(Me.Descripcion)
            listCampos.Add("N_NUM_TIEMPO_EJECUCION") : listValores.Add(Me.TiempoEjecucion)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            resultado = conexion.Insertar("BDS_C_GR_NIVELES_SERVICIO", listCampos, listValores)
            bitacora.Insertar("BDS_C_GR_NIVELES_SERVICIO", listCampos, listValores, resultado, "")

        Catch ex As Exception
            resultado = False
            bitacora.Insertar("BDS_C_GR_NIVELES_SERVICIO", listCampos, listValores, resultado, "Error al guardar el nivel de servicio")
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally

            Try : bitacora.Finalizar(resultado) : Catch ex As Exception : End Try
            If conexion IsNot Nothing Then
                conexion.CerrarConexion() : conexion = Nothing
            End If
            listValores = Nothing
            listCampos = Nothing
        End Try

        Return resultado

    End Function

    ''' <summary>
    ''' Actualiza un nivel de servicio
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualización de nivel de servicio", System.Web.HttpContext.Current.Session.SessionID, _
                                              CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Usuario).IdentificadorUsuario)

        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        Try

            listCampos.Add("N_ID_TIPO_SERVICIO") : listValores.Add(Me.IdentificadorTipoServicio)
            listCampos.Add("N_ID_SERVICIO") : listValores.Add(Me.IdentificadorServicio)
            listCampos.Add("N_ID_APLICATIVO") : listValores.Add(Me.IdentificadorAplicativo)
            listCampos.Add("N_ID_AREA") : listValores.Add(Me.IdentificadorArea)
            listCampos.Add("N_ID_FLUJO") : listValores.Add(Me.IdentificadorFlujo)
            listCampos.Add("T_ID_INGENIERO_RESPONSABLE") : listValores.Add(Me.IngenieroResponsable)
            listCampos.Add("T_ID_INGENIERO_BACKUP") : listValores.Add(Me.IngenieroBackup)
            listCampos.Add("T_DSC_NIVELES_SERVICIO") : listValores.Add(Me.Descripcion)
            listCampos.Add("N_NUM_TIEMPO_EJECUCION") : listValores.Add(Me.TiempoEjecucion)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)


            listCamposCondicion.Add("N_ID_NIVELES_SERVICIO") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_NIVELES_SERVICIO", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_NIVELES_SERVICIO", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Actualizar("BDS_C_GR_NIVELES_SERVICIO", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar menú")
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally
            Try : bitacora.Finalizar(resultado) : Catch ex As Exception : End Try
            If conexion IsNot Nothing Then
                conexion.CerrarConexion() : conexion = Nothing
            End If
        End Try

        Return resultado

    End Function

    ''' <summary>
    ''' Elimina un nivel de servicio
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Baja() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Borrar nivel de servicio", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Usuario).IdentificadorUsuario)
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        Try
            listCampos.Add("B_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_NIVELES_SERVICIO") : listValoresCondicion.Add(Me.Identificador)

            resultado = conexion.Actualizar("BDS_C_GR_NIVELES_SERVICIO", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_NIVELES_SERVICIO", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Actualizar("BDS_C_GR_NIVELES_SERVICIO", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar menú")
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally
            Try : bitacora.Finalizar(resultado) : Catch ex As Exception : End Try
            If conexion IsNot Nothing Then
                conexion.CerrarConexion() : conexion = Nothing
            End If

        End Try

        Return resultado

    End Function
End Class
