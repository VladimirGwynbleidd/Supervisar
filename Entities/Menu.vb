

' Fecha de creación: 24/07/2013
' Fecha de modificación: 
' Nombre del Responsable: ARGC1
' Empresa: Softtek
' contiene los metodos que son parte del negocio del catalogo de menús



<Serializable()> _
Public Class Menu

#Region "Variables de trabajo"

    Private idMenu As Int32
    Private descripcionMenu As String
    Private urlImagenSubm As String
    Private esVigenteMenu As Boolean
    Private inicioVigenciaMenu As DateTime
    Private finVigenciaMenu As DateTime
    Private numeroOrden As Int32
    Private subMenuMenu As List(Of SubMenu)

#End Region

#Region "Propiedades"

    Public Property IdentificadorMenu As Int32
        Get
            Return idMenu
        End Get
        Set(ByVal value As Int32)
            idMenu = value
        End Set
    End Property

    Public Property Descripcion As String
        Get
            Return descripcionMenu
        End Get
        Set(ByVal value As String)
            descripcionMenu = value
        End Set
    End Property

    Public Property UrlImagenSubmenu As String
        Get
            Return urlImagenSubm
        End Get
        Set(ByVal value As String)
            urlImagenSubm = value
        End Set
    End Property

    Public Property EsVigente As Boolean
        Get
            Return esVigenteMenu
        End Get
        Set(ByVal value As Boolean)
            esVigenteMenu = value
        End Set
    End Property

    Public Property InicioVigencia As DateTime
        Get
            Return inicioVigenciaMenu
        End Get
        Set(ByVal value As DateTime)
            inicioVigenciaMenu = value
        End Set
    End Property

    Public Property FinVigencia As DateTime
        Get
            Return finVigenciaMenu
        End Get
        Set(ByVal value As DateTime)
            finVigenciaMenu = value
        End Set
    End Property

    Public Property Orden As Int32
        Get
            Return numeroOrden
        End Get
        Set(ByVal value As Int32)
            numeroOrden = value
        End Set
    End Property

    Public Property SubMenus As List(Of SubMenu)
        Get
            Return subMenuMenu
        End Get
        Set(ByVal value As List(Of SubMenu))
            subMenuMenu = value
        End Set
    End Property

    Public Property Existe As Boolean = False

#End Region

#Region "Constructores"

    Public Sub New()

    End Sub

    Public Sub New(ByVal idMenu As Int32, ByVal descripcion As String, ByVal urlImagenSubmenu As String, ByVal esVigente As Boolean, _
                   ByVal fechaInicioVigencia As DateTime, ByVal fechaFinVigencia As DateTime, ByVal orden As Int32)

        Me.idMenu = idMenu
        Me.descripcionMenu = descripcion
        Me.urlImagenSubm = urlImagenSubmenu
        Me.EsVigente = esVigente
        Me.inicioVigenciaMenu = fechaInicioVigencia
        Me.finVigenciaMenu = fechaFinVigencia
        Me.numeroOrden = orden

    End Sub

    Public Sub New(ByVal idMenu)
        Me.idMenu = idMenu
        CargarDatos()
    End Sub

#End Region


    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataView
            dv = conexion.ConsultarRegistrosDT("BDS_C_GR_MENU").DefaultView
            dv.Sort = "N_ID_MENU"
            Return dv

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function


    Public Function ObtenerSiguienteIdentificador() As Integer

        Dim resultado As Integer = 1
        Dim conexion As Conexion.SQLServer = Nothing
        Dim dr As SqlClient.SqlDataReader = Nothing

        Try
            conexion = New Conexion.SQLServer
            dr = conexion.ConsultarDR("SELECT (MAX(N_ID_MENU) + 1) N_ID_MENU FROM BDS_C_GR_MENU")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_MENU")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_MENU"))
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
            If conexion IsNot Nothing Then
                conexion.CerrarConexion() : conexion = Nothing
            End If
        End Try

       

        Return resultado

    End Function


    Public Sub CargarDatos()

        Dim conexion As Conexion.SQLServer = Nothing
        Dim dr As SqlClient.SqlDataReader = Nothing
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)

        Try
            conexion = New Conexion.SQLServer
            listCampos.Add("N_ID_MENU") : listValores.Add(Me.IdentificadorMenu)

            Existe = conexion.BuscarUnRegistro("BDS_C_GR_MENU", listCampos, listValores)

            If Existe Then

                dr = conexion.ConsultarRegistrosDR("BDS_C_GR_MENU", listCampos, listValores)

                If dr.Read() Then

                    Me.Descripcion = CStr(dr("T_DSC_MENU"))
                    Me.UrlImagenSubmenu = CStr(dr("T_DSC_URL_IMAGEN_SUBM"))
                    Me.EsVigente = CBool(dr("N_FLAG_VIG"))
                    Me.Orden = CStr(dr("N_NUM_ORDEN"))

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
            If conexion IsNot Nothing Then
                conexion.CerrarConexion() : conexion = Nothing
            End If
        End Try

    End Sub


    ''' <summary>
    ''' Registra un menu
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Registro de nuevo menú", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)

        Try

            listCampos.Add("N_ID_MENU") : listValores.Add(Me.IdentificadorMenu)
            listCampos.Add("T_DSC_MENU") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_DSC_URL_IMAGEN_SUBM") : listValores.Add(Me.UrlImagenSubmenu)
            listCampos.Add("N_NUM_ORDEN") : listValores.Add(Me.Orden)
            listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            resultado = conexion.Insertar("BDS_C_GR_MENU", listCampos, listValores)
            bitacora.Insertar("BDS_C_GR_MENU", listCampos, listValores, resultado, "")

        Catch ex As Exception
            resultado = False
            bitacora.Insertar("BDS_C_GR_MENU", listCampos, listValores, resultado, "Error al guardar menú")
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
    ''' Actualiza un menú
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualización de menú", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        Try

            listCampos.Add("T_DSC_MENU") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_DSC_URL_IMAGEN_SUBM") : listValores.Add(Me.UrlImagenSubmenu)
            listCampos.Add("N_NUM_ORDEN") : listValores.Add(Me.Orden)

            listCamposCondicion.Add("N_ID_MENU") : listValoresCondicion.Add(Me.IdentificadorMenu)

            resultado = conexion.Actualizar("BDS_C_GR_MENU", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_MENU", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Actualizar("BDS_C_GR_MENU", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar menú")
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
    ''' Elimina un correo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Baja() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer
        Dim bitacora As New Conexion.Bitacora("Borrar munú", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        Try
            listCampos.Add("N_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_MENU") : listValoresCondicion.Add(Me.IdentificadorMenu)

            resultado = conexion.Actualizar("BDS_C_GR_MENU", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_MENU", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Actualizar("BDS_C_GR_MENU", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar menú")
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
