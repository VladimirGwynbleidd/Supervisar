'- Fecha de creación: 11/02/2014
'- Fecha de modificación:  NA
'- Nombre del Responsable: Rivera Martiñón Iván
'- Empresa: Softtek
'- Clase para Catálogo De Flujos

<Serializable()> _
Public Class Flujo

    Private Tabla As String = "BDS_C_GR_FLUJO"

#Region "Propiedades"

    Public Property Identificador As Integer
    Public Property Descripcion As String
    Public Property Vigente As Boolean
    Public Property InicioVigencia As Date
    Public Property FinVigencia As Date?
    Public Property Existe As Boolean = False

#End Region

#Region "Constructores"

    Public Sub New()

    End Sub

    Public Sub New(ByVal idFlujo As Integer)
        Me.Identificador = idFlujo

        CargarDatos()
    End Sub


#End Region

#Region "Metodos"

    ''' <summary>
    ''' Carga los datos de Flujo tomando el Identificador almacenado en la propiedad
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim dr As SqlClient.SqlDataReader = Nothing

            listCampos.Add("N_ID_FLUJO") : listValores.Add(Me.Identificador)

            Try

                Existe = conexion.BuscarUnRegistro(Tabla, listCampos, listValores)

                If Existe Then

                    dr = conexion.ConsultarRegistrosDR(Tabla, listCampos, listValores)

                    If dr.Read() Then
                        Me.Descripcion = CStr(dr("T_DSC_FLUJO"))
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

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_FLUJO) + 1) N_ID_FLUJO FROM " + Tabla)

            If dr.Read() Then

                If IsDBNull(dr("N_ID_FLUJO")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_FLUJO"))
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
    ''' Agrega el Flujo al catalogo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Dim bitacora As New Conexion.Bitacora("Registro de nuevo Flujo", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_FLUJO") : listValores.Add(Me.Identificador)
            listCampos.Add("T_DSC_FLUJO") : listValores.Add(Me.Descripcion)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(True)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

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

    ''' <summary>
    ''' Actualiza un Tipo de Flujo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualización de Flujo", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("T_DSC_FLUJO") : listValores.Add(Me.Descripcion)
            listCampos.Add("B_FLAG_VIG") : listValores.Add(True)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_FLUJO") : listValoresCondicion.Add(Me.Identificador)

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
    ''' Termina la vigencia de un registro
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Baja() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Borrar Flujo", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("B_FLAG_VIG") : listValores.Add(False)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_FLUJO") : listValoresCondicion.Add(Me.Identificador)

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

    Public Function ActualizarPasosFlujo(ByVal agregados As List(Of Entities.PasoFlujo), ByVal eliminados As List(Of Entities.PasoFlujo), ByVal modificados As List(Of Entities.PasoFlujo), ByVal idFlujo As Integer) As Boolean

        Dim campos As List(Of Object)
        Dim condicion As List(Of String)
        Dim valores As List(Of Object)

        Dim conexion As New Conexion.SQLServer
        Try

            For Each agregado As Entities.PasoFlujo In agregados
                campos = New List(Of Object)
                campos.Add(idFlujo)
                campos.Add(agregado.Identificador)
                campos.Add(agregado.Orden)
                conexion.Insertar("BDS_R_GR_SEGUIMIENTO_FLUJO", campos)
            Next

            For Each eliminado As Entities.PasoFlujo In eliminados
                condicion = New List(Of String)
                valores = New List(Of Object)

                condicion.Add("N_ID_FLUJO") : valores.Add(idFlujo)
                condicion.Add("N_ID_PASO") : valores.Add(eliminado.Identificador)

                conexion.Eliminar("BDS_R_GR_SEGUIMIENTO_FLUJO", condicion, valores)
            Next

            For Each modificado As Entities.PasoFlujo In modificados
                condicion = New List(Of String)
                valores = New List(Of Object)
                Dim campo = New List(Of String)
                Dim valorCampo As New List(Of Object)

                condicion.Add("N_ID_FLUJO") : valores.Add(idFlujo)
                condicion.Add("N_ID_PASO") : valores.Add(modificado.Identificador)
                campo.Add("N_ID_ORDEN") : valorCampo.Add(modificado.Orden)
                conexion.Actualizar("BDS_R_GR_SEGUIMIENTO_FLUJO", campo, valorCampo, condicion, valores)
            Next


        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

        Return True
    End Function

    Public Function ObtenerPasos(ByVal idFlujo As Integer) As List(Of Entities.PasoFlujo)


        Dim lstPasosFlujo As New List(Of PasoFlujo)

        Dim strQuery As String = "SELECT PF.N_ID_PASO, PF.T_DSC_PASO_TOOLTIP, SF.N_ID_ORDEN " & _
                                "FROM BDS_C_GR_FLUJO F " & _
                                "JOIN BDS_R_GR_SEGUIMIENTO_FLUJO SF ON F.N_ID_FLUJO = SF.N_ID_FLUJO " & _
                                "JOIN BDS_C_GR_PASOS_SEGUIMIENTO PF ON SF.N_ID_PASO = PF.N_ID_PASO " & _
                                "WHERE F.N_ID_FLUJO =  " & idFlujo & " AND F.B_FLAG_VIG = 1 "


        Dim conexion As New Conexion.SQLServer
        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                Dim entPasoFlujo As New PasoFlujo
                entPasoFlujo.Identificador = dr.Item("N_ID_PASO")
                entPasoFlujo.Descripcion = dr.Item("T_DSC_PASO_TOOLTIP")
                entPasoFlujo.Orden = dr.Item("N_ID_ORDEN")
                lstPasosFlujo.Add(entPasoFlujo)
            Next

            Return lstPasosFlujo

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function



    Public Function ObtenerPasosDisponibles(ByVal idFlujo As Integer) As List(Of Entities.PasoFlujo)

        Dim lstPasoFlujo As New List(Of PasoFlujo)

        Dim strQuery As String = "SELECT N_ID_PASO, T_DSC_PASO_TOOLTIP " & _
                                "FROM BDS_C_GR_PASOS_SEGUIMIENTO " & _
                                "WHERE N_ID_PASO NOT IN " & _
                                "( " & _
                                "SELECT PF.N_ID_PASO " & _
                                "FROM BDS_C_GR_FLUJO F " & _
                                "JOIN BDS_R_GR_SEGUIMIENTO_FLUJO SF ON F.N_ID_FLUJO = SF.N_ID_FLUJO " & _
                                "JOIN BDS_C_GR_PASOS_SEGUIMIENTO PF ON SF.N_ID_PASO = PF.N_ID_PASO " & _
                                "WHERE F.N_ID_FLUJO =  " & idFlujo & _
                                " ) AND B_FLAG_VIG = 1 "


        Dim conexion As New Conexion.SQLServer
        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                Dim entPasoFlujo As New PasoFlujo
                entPasoFlujo.Identificador = dr.Item("N_ID_PASO")
                entPasoFlujo.Descripcion = dr.Item("T_DSC_PASO_TOOLTIP")
                lstPasoFlujo.Add(entPasoFlujo)
            Next

            Return lstPasoFlujo

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    Public Function ObtenerEstatusAnterior(ByVal estatusSol As Integer) As Integer
        Dim conexion As New Conexion.SQLServer
        Dim estatus As Integer = -1
        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT PS.N_ID_ESTATUS_SERVICIO FROM BDS_R_GR_SEGUIMIENTO_FLUJO SF " & _
                                    "INNER JOIN BDS_C_GR_PASOS_SEGUIMIENTO PS ON PS.N_ID_PASO = SF.N_ID_PASO " & _
                                    "WHERE  N_ID_FLUJO ={0} AND N_ID_ORDEN IN ( " & _
                                    "SELECT SF.N_ID_ORDEN - 1 AS ORDEN FROM BDS_R_GR_SEGUIMIENTO_FLUJO SF " & _
                                    "INNER JOIN BDS_C_GR_PASOS_SEGUIMIENTO PS ON PS.N_ID_PASO = SF.N_ID_PASO " & _
                                    "WHERE SF.N_ID_FLUJO = {0} AND PS.N_ID_ESTATUS_SERVICIO = {1})"
            query = String.Format(query, Me.Identificador, estatusSol)
            dv = conexion.ConsultarDT(query)
            If dv.Rows.Count > 0 Then
                For Each row As DataRow In dv.Rows
                    estatus = CInt(row("N_ID_ESTATUS_SERVICIO"))
                Next
            End If
            Return estatus
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