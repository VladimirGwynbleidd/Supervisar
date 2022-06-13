Imports Conexion
Imports Utilerias

<Serializable()> _
Public Class SubMenu

#Region "Variables de trabajo"

    Private idMenu As Int32
    Private idSubMenu As Int32
    Private descripcionSubMenu As String
    'Private urlImagenSubMenu As String
    Private urlPaginaSubMenu As String
    Private descripcionPaginaSubMenu As String
    Private esVigenteSubMenu As Boolean
    Private inicioVigenciaSubMenu As DateTime
    Private finVigenciaSubMenu As DateTime
    Private numeroOrden As Int32
    Private visible As Boolean
    Private idPerfil As Int32

#End Region

#Region "Propiedades"

    Public Property EsVisible As Boolean
        Get
            Return visible
        End Get
        Set(value As Boolean)
            visible = value
        End Set
    End Property


    Public Property IdentificadorMenu As Int32
        Get
            Return idMenu
        End Get
        Set(value As Int32)
            idMenu = value
        End Set
    End Property

    Public Property IdentificadorSubMenu As Int32
        Get
            Return idSubMenu
        End Get
        Set(ByVal value As Int32)
            idSubMenu = value
        End Set
    End Property

    Public Property Descripcion As String
        Get
            Return descripcionSubMenu
        End Get
        Set(ByVal value As String)
            descripcionSubMenu = value
        End Set
    End Property

    Public Property DescripcionPagina As String
        Get
            Return descripcionPaginaSubMenu
        End Get
        Set(ByVal value As String)
            descripcionPaginaSubMenu = value
        End Set
    End Property


    Public Property UrlPagina As String
        Get
            Return urlPaginaSubMenu
        End Get
        Set(ByVal value As String)
            urlPaginaSubMenu = value
        End Set
    End Property


    Public Property EsVigente As Boolean
        Get
            Return esVigenteSubMenu
        End Get
        Set(ByVal value As Boolean)
            esVigenteSubMenu = value
        End Set
    End Property

    Public Property InicioVigencia As DateTime
        Get
            Return inicioVigenciaSubMenu
        End Get
        Set(ByVal value As DateTime)
            inicioVigenciaSubMenu = value
        End Set
    End Property

    Public Property FinVigencia As DateTime
        Get
            Return finVigenciaSubMenu
        End Get
        Set(ByVal value As DateTime)
            finVigenciaSubMenu = value
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

    Public Property IdentificadorPerfil As Int32
        Get
            Return idPerfil
        End Get
        Set(ByVal value As Int32)
            idPerfil = value
        End Set
    End Property

    Public Property Existe As Boolean = False
#End Region

#Region "Constructores"

    Public Sub New()

    End Sub

    Public Sub New(ByVal idMenu, ByVal idSubmenu)
        Me.idMenu = idMenu
        Me.idSubMenu = idSubmenu

        CargarDatos()
    End Sub


    Public Sub New(ByVal idSubMenu As Int32, ByVal descripcion As String, ByVal urlImagenSubmenu As String, ByVal esVigente As Boolean, _
                   ByVal fechaInicioVigencia As DateTime, ByVal fechaFinVigencia As DateTime, ByVal orden As Int32, ByVal visible As Boolean)

        Me.idSubMenu = idSubMenu
        Me.descripcionSubMenu = descripcion
        Me.UrlPagina = urlImagenSubmenu
        Me.EsVigente = esVigente
        Me.inicioVigenciaSubMenu = fechaInicioVigencia
        Me.finVigenciaSubMenu = fechaFinVigencia
        Me.numeroOrden = orden
        Me.EsVisible = visible

    End Sub

#End Region


    Public Sub CargarDatos()

        Dim conexion As SQLServer = Nothing
        Dim dr As SqlClient.SqlDataReader = Nothing
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)

        Try
            conexion = New SQLServer
            listCampos.Add("N_ID_MENU") : listValores.Add(Me.IdentificadorMenu)
            listCampos.Add("N_ID_SUBMENU") : listValores.Add(Me.IdentificadorSubMenu)

            Existe = conexion.BuscarUnRegistro("BDS_R_GR_MENU_SUBMENU_PAGINA", listCampos, listValores)

            If Existe Then

                dr = conexion.ConsultarRegistrosDR("BDS_R_GR_MENU_SUBMENU_PAGINA", listCampos, listValores)

                If dr.Read() Then

                    Me.Descripcion = CStr(dr("T_DSC_SUBMENU"))
                    Me.DescripcionPagina = CStr(dr("T_DSC_PAGINA"))
                    Me.EsVigente = CBool(dr("N_FLAG_VIG"))
                    Me.Orden = CStr(dr("N_NUM_ORDEN"))
                    Me.UrlPagina = CStr(dr("T_DSC_URL_PAGINA"))
                    Me.EsVisible = CBool(dr("N_FLAG_VISIBLE"))

                End If

            End If
        Catch ex As Exception
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
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


    Public Function ObtenerTodos() As DataView
        Dim conexion As SQLServer = Nothing
        Dim dt As New DataView
        Try
            conexion = New SQLServer
            dt = conexion.ConsultarRegistrosDT("BDS_R_GR_MENU_SUBMENU_PAGINA").DefaultView
            dt.Sort = "N_ID_SUBMENU"

        Catch ex As Exception
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            If conexion IsNot Nothing Then
                conexion.CerrarConexion() : conexion = Nothing
            End If
        End Try

        Return dt

    End Function

    Public Function ObtenerSiguienteIdentificador(ByVal idMenu) As Integer

        Dim resultado As Integer = 1
        Dim conexion As SQLServer = Nothing
        Dim dr As SqlClient.SqlDataReader = Nothing

        Try
            conexion = New SQLServer
            dr = conexion.ConsultarDR("SELECT (MAX(N_ID_SUBMENU) + 1) N_ID_SUBMENU FROM BDS_R_GR_MENU_SUBMENU_PAGINA WHERE N_ID_MENU=" & idMenu)

            If dr.Read() Then

                If IsDBNull(dr("N_ID_SUBMENU")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_SUBMENU"))
                End If

            End If
        Catch ex As Exception
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
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


    ''' <summary>
    ''' Registra un menu
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False
        Dim conexion As New SQLServer
        Dim bitacora As New Conexion.Bitacora("Registro de nueva submenú" & "(" & Me.Descripcion & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)

        Try

            listCampos.Add("N_ID_MENU") : listValores.Add(Me.IdentificadorMenu)
            listCampos.Add("N_ID_SUBMENU") : listValores.Add(Me.IdentificadorSubMenu)
            listCampos.Add("T_DSC_SUBMENU") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_DSC_URL_PAGINA") : listValores.Add(Me.UrlPagina)
            listCampos.Add("T_DSC_PAGINA") : listValores.Add(Me.DescripcionPagina)
            listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)
            listCampos.Add("N_NUM_ORDEN") : listValores.Add(Me.Orden)
            listCampos.Add("N_FLAG_VISIBLE") : listValores.Add(Me.EsVisible)

            resultado = conexion.Insertar("BDS_R_GR_MENU_SUBMENU_PAGINA", listCampos, listValores)
            bitacora.Insertar("BDS_R_GR_MENU_SUBMENU_PAGINA", listCampos, listValores, resultado, "")

        Catch ex As Exception
            resultado = False
            bitacora.Insertar("BDS_R_GR_MENU_SUBMENU_PAGINA", listCampos, listValores, resultado, "Error al guardar submenú")
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
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
    ''' Actualiza un submenú
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New SQLServer
        Dim bitacora As New Conexion.Bitacora("Actualización de submenú" & "(" & Me.Descripcion & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        Try

            listCampos.Add("T_DSC_SUBMENU") : listValores.Add(Me.Descripcion)
            listCampos.Add("T_DSC_URL_PAGINA") : listValores.Add(Me.UrlPagina)
            listCampos.Add("T_DSC_PAGINA") : listValores.Add(Me.DescripcionPagina)
            listCampos.Add("N_NUM_ORDEN") : listValores.Add(Me.Orden)
            listCampos.Add("N_FLAG_VISIBLE") : listValores.Add(Me.EsVisible)

            listCamposCondicion.Add("N_ID_MENU") : listValoresCondicion.Add(Me.IdentificadorMenu)
            listCamposCondicion.Add("N_ID_SUBMENU") : listValoresCondicion.Add(Me.IdentificadorSubMenu)

            resultado = conexion.Actualizar("BDS_R_GR_MENU_SUBMENU_PAGINA", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_R_GR_MENU_SUBMENU_PAGINA", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Actualizar("BDS_R_GR_MENU_SUBMENU_PAGINA", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar submenú")
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

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

        Dim conexion As New SQLServer
        Dim bitacora As New Conexion.Bitacora("Borrar submenú" & "(" & Me.IdentificadorSubMenu & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Dim listCampos As New List(Of String)
        Dim listValores As New List(Of Object)
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)

        Try
            listCampos.Add("N_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_MENU") : listValoresCondicion.Add(Me.IdentificadorMenu)
            listCamposCondicion.Add("N_ID_SUBMENU") : listValoresCondicion.Add(Me.IdentificadorSubMenu)

            resultado = conexion.Actualizar("BDS_R_GR_MENU_SUBMENU_PAGINA", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_R_GR_MENU_SUBMENU_PAGINA", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "")

        Catch ex As Exception

            resultado = False
            bitacora.Actualizar("BDS_R_GR_MENU_SUBMENU_PAGINA", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar submenú")
            ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)

        Finally
            Try : bitacora.Finalizar(resultado) : Catch ex As Exception : End Try
            If conexion IsNot Nothing Then
                conexion.CerrarConexion() : conexion = Nothing
            End If

        End Try

        Return resultado

    End Function


    Public Function ObtenerTodosFiltro(ByVal Menu) As DataSet
        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)
        listCamposCondicion.Add("N_ID_MENU") : listValoresCondicion.Add(Menu)
        Dim conexion As New Conexion.SQLServer
        Try

            Return conexion.ConsultarRegistrosDS("BDS_R_GR_MENU_SUBMENU_PAGINA", listCamposCondicion, listValoresCondicion)

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    ''' <summary>
    ''' Obtiene los SubMenus de un Menu asignados a un perfil
    ''' </summary>
    ''' <param name="idPerfil">Id de Perfil</param>
    ''' <param name="idMenu">Id de Menu</param>
    ''' <returns>Lista de SubMenus</returns>
    ''' <remarks></remarks>
    Public Function ObtenerSubMenuPerfil(ByVal idPerfil As Integer, ByVal idMenu As Integer) As List(Of SubMenu)
        Dim lstSubMenu As New List(Of SubMenu)

        Dim strQuery As String = String.Format("SELECT MSP.N_ID_MENU, MSP.N_ID_SUBMENU, MSP.T_DSC_SUBMENU FROM BDS_R_GR_MENU_SUBMENU_PAGINA MSP " + _
                                               "INNER JOIN BDS_R_GR_PERFIL_MENU_SUBMENU_PAGINA PMSP ON PMSP.N_ID_MENU = MSP.N_ID_MENU " + _
                                               "    AND PMSP.N_ID_SUBMENU = MSP.N_ID_SUBMENU AND PMSP.N_ID_PERFIL = {0} " + _
                                               "WHERE MSP.N_FLAG_VIG = 1 AND MSP.N_ID_MENU = {1} ORDER BY MSP.N_ID_SUBMENU ASC ", _
                                               idPerfil.ToString, idMenu.ToString)

        Dim conexion As New SQLServer
        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                Dim entSubMenu As New SubMenu()
                entSubMenu.IdentificadorMenu = Convert.ToInt32(dr.Item("N_ID_MENU"))
                entSubMenu.IdentificadorSubMenu = Convert.ToInt32(dr.Item("N_ID_SUBMENU"))
                entSubMenu.Descripcion = dr.Item("T_DSC_SUBMENU").ToString
                lstSubMenu.Add(entSubMenu)
            Next

            Return lstSubMenu

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    ''' <summary>
    ''' Obtiene los SubMenus de un Menu no asignados a un perfil
    ''' </summary>
    ''' <param name="idPerfil">Id de Perfil</param>
    ''' <param name="idMenu">Id de Menu</param>
    ''' <returns>Lista de SubMenus</returns>
    ''' <remarks></remarks>
    Public Function ObtenerSubMenuNoEnPerfil(ByVal idPerfil As Integer, ByVal idMenu As Integer) As List(Of SubMenu)
        Dim lstSubMenu As New List(Of SubMenu)

        Dim strQuery As String = String.Format("SELECT MSP.N_ID_MENU, MSP.N_ID_SUBMENU, MSP.T_DSC_SUBMENU FROM BDS_R_GR_MENU_SUBMENU_PAGINA MSP " + _
                                               "WHERE MSP.N_FLAG_VIG = 1 AND MSP.N_ID_MENU = {0} " + _
                                               "AND MSP.N_ID_SUBMENU NOT IN ( SELECT MSP2.N_ID_SUBMENU " + _
                                               "    FROM BDS_R_GR_MENU_SUBMENU_PAGINA MSP2 " + _
                                               "    INNER JOIN BDS_R_GR_PERFIL_MENU_SUBMENU_PAGINA PMSP ON PMSP.N_ID_MENU = MSP2.N_ID_MENU " + _
                                               "        AND PMSP.N_ID_SUBMENU = MSP2.N_ID_SUBMENU AND PMSP.N_ID_PERFIL = {1} " + _
                                               "    WHERE MSP2.N_FLAG_VIG = 1 AND MSP2.N_ID_MENU = MSP.N_ID_MENU ) ORDER BY MSP.N_ID_SUBMENU ASC ", _
                                               idMenu.ToString, idPerfil.ToString)

        Dim conexion As New SQLServer
        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                Dim entSubMenu As New SubMenu()
                entSubMenu.IdentificadorMenu = Convert.ToInt32(dr.Item("N_ID_MENU"))
                entSubMenu.IdentificadorSubMenu = Convert.ToInt32(dr.Item("N_ID_SUBMENU"))
                entSubMenu.Descripcion = dr.Item("T_DSC_SUBMENU").ToString
                lstSubMenu.Add(entSubMenu)
            Next

            Return lstSubMenu

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    ''' <summary>
    ''' Borra SubMenu de un Perfil
    ''' </summary>
    ''' <param name="idPerfil">Id de Perfil</param>
    ''' <param name="lstSubMenu">Lista de SubMenu</param>
    ''' <remarks></remarks>
    Public Sub BorraSubMenuPerfil(ByVal idPerfil As Integer, ByVal lstSubMenu As List(Of SubMenu))

        If lstSubMenu.Count > 0 Then
            Dim lstCampos As New List(Of String)
            lstCampos.Add("N_ID_PERFIL")
            lstCampos.Add("N_ID_MENU")
            lstCampos.Add("N_ID_SUBMENU")

            Dim lstValores As New List(Of Object)
            Dim elimino As Boolean

            Dim conexion As New Conexion.SQLServer
            Dim bitacora As New Conexion.Bitacora("Borra SubMenus asignados a Perfil" & "(" & Me.descripcionSubMenu & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            Try
                For Each entSubMenu As SubMenu In lstSubMenu
                    lstValores.Clear()
                    lstValores.Add(idPerfil)
                    lstValores.Add(entSubMenu.IdentificadorMenu)
                    lstValores.Add(entSubMenu.IdentificadorSubMenu)
                    Try
                        elimino = conexion.Eliminar("BDS_R_GR_PERFIL_MENU_SUBMENU_PAGINA", lstCampos, lstValores)
                        bitacora.Eliminar("BDS_R_GR_PERFIL_MENU_SUBMENU_PAGINA", lstCampos, lstValores, elimino, "")
                    Catch ex As Exception
                        elimino = False
                        bitacora.Eliminar("BDS_R_GR_PERFIL_MENU_SUBMENU_PAGINA", lstCampos, lstValores, elimino, ex.ToString)
                        Throw ex
                    End Try                    
                Next
            Catch ex As Exception
                Throw ex
            Finally
                conexion.CerrarConexion()
                bitacora.Finalizar(elimino)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Inserta SubMenu a un Perfil
    ''' </summary>
    ''' <param name="idPerfil">Id de Perfil</param>
    ''' <param name="lstSubMenu">Lista de SubMenu</param>
    ''' <remarks></remarks>
    Public Sub InsertaSubMenuPerfil(ByVal idPerfil As Integer, ByVal lstSubMenu As List(Of SubMenu))
        If lstSubMenu.Count > 0 Then
            Dim lstCampos As New List(Of String)
            lstCampos.Add("N_ID_PERFIL")
            lstCampos.Add("N_ID_MENU")
            lstCampos.Add("N_ID_SUBMENU")

            Dim lstValores As New List(Of Object)
            Dim inserto As Boolean

            Dim conexion As New Conexion.SQLServer
            Dim bitacora As New Conexion.Bitacora("Inserta Asignación de Submenús a Perfil" & "(" & Me.IdentificadorSubMenu & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            Try
                For Each entSubMenu As SubMenu In lstSubMenu
                    lstValores.Clear()
                    lstValores.Add(idPerfil)
                    lstValores.Add(entSubMenu.IdentificadorMenu)
                    lstValores.Add(entSubMenu.IdentificadorSubMenu)
                    Try
                        inserto = conexion.Insertar("BDS_R_GR_PERFIL_MENU_SUBMENU_PAGINA", lstCampos, lstValores)
                        bitacora.Insertar("BDS_R_GR_PERFIL_MENU_SUBMENU_PAGINA", lstCampos, lstValores, inserto, "")
                    Catch ex As Exception
                        inserto = False
                        bitacora.Insertar("BDS_R_GR_PERFIL_MENU_SUBMENU_PAGINA", lstCampos, lstValores, inserto, ex.ToString)
                        Throw ex
                    End Try
                Next
            Catch ex As Exception
                Throw ex
            Finally
                conexion.CerrarConexion()
                bitacora.Finalizar(inserto)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Actualiza los SubMenus Asignados a un Perfil
    ''' </summary>
    ''' <param name="idPerfil">Id de Perfil</param>
    ''' <param name="lstAgregados">Lista de SubMenus Agregados</param>
    ''' <param name="lstEliminados">Lista de SubMenus Eliminados</param>
    ''' <remarks></remarks>
    Public Sub ActualizaSubMenuPerfil(ByVal idPerfil As Integer, ByVal lstAgregados As List(Of SubMenu), ByVal lstEliminados As List(Of SubMenu))

        Try
            BorraSubMenuPerfil(idPerfil, lstEliminados)

            InsertaSubMenuPerfil(idPerfil, lstAgregados)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            Throw ex
        End Try

    End Sub

    Public Function MuestraSubmenus() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT M.N_ID_SUBMENU, M.T_DSC_SUBMENU, M.T_DSC_URL_PAGINA, M.T_DSC_PAGINA, M.N_FLAG_VIG, M.F_FECH_INI_VIG, M.F_FECH_FIN_VIG, M.N_NUM_ORDEN " & _
                                    "FROM BDS_R_GR_MENU_SUBMENU_PAGINA M " & _
                                    "INNER JOIN BDS_R_GR_PERFIL_MENU_SUBMENU_PAGINA P ON P.N_ID_PERFIL = {0} AND P.N_ID_MENU = M.N_ID_MENU AND P.N_ID_SUBMENU = M.N_ID_SUBMENU " & _
                                    "WHERE M.N_ID_MENU = {1} AND M.N_FLAG_VIG = 1 AND M.N_FLAG_VISIBLE = 1 ORDER BY N_NUM_ORDEN"

            query = String.Format(query, Me.IdentificadorPerfil, Me.IdentificadorMenu)
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

    Public Function MuestraSubmenusFiltrado() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT M.N_ID_SUBMENU, M.T_DSC_SUBMENU, M.T_DSC_URL_PAGINA, M.T_DSC_PAGINA, M.N_FLAG_VIG, M.F_FECH_INI_VIG, M.F_FECH_FIN_VIG, M.N_NUM_ORDEN " & _
                                    "FROM BDS_R_GR_MENU_SUBMENU_PAGINA M " & _
                                    "INNER JOIN BDS_R_GR_PERFIL_MENU_SUBMENU_PAGINA P ON P.N_ID_PERFIL = {0} AND P.N_ID_MENU = M.N_ID_MENU AND P.N_ID_SUBMENU = M.N_ID_SUBMENU " & _
                                    "WHERE M.N_ID_MENU = {1} AND M.N_FLAG_VIG = 1 AND M.N_FLAG_VISIBLE = 1 AND M.N_ID_SUBMENU = 4 ORDER BY N_NUM_ORDEN"

            query = String.Format(query, Me.IdentificadorPerfil, Me.IdentificadorMenu)
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

    Public Function MuestraSubmenusSinImgProceso() As DataTable
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dv As New DataTable
            Dim query As String = "SELECT M.N_ID_SUBMENU, M.T_DSC_SUBMENU, M.T_DSC_URL_PAGINA, M.T_DSC_PAGINA, M.N_FLAG_VIG, M.F_FECH_INI_VIG, M.F_FECH_FIN_VIG, M.N_NUM_ORDEN " & _
                                    "FROM BDS_R_GR_MENU_SUBMENU_PAGINA M " & _
                                    "INNER JOIN BDS_R_GR_PERFIL_MENU_SUBMENU_PAGINA P ON P.N_ID_PERFIL = {0} AND P.N_ID_MENU = M.N_ID_MENU AND P.N_ID_SUBMENU = M.N_ID_SUBMENU " & _
                                    "WHERE M.N_ID_MENU = {1} AND M.N_FLAG_VIG = 1 AND M.N_FLAG_VISIBLE = 1 AND T_DSC_URL_PAGINA <> '~/Mantenimiento/ImgProceso.aspx' ORDER BY N_NUM_ORDEN"

            query = String.Format(query, Me.IdentificadorPerfil, Me.IdentificadorMenu)
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
End Class
