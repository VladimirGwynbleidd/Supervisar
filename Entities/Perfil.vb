Imports System
Imports System.Web
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient


<Serializable()> _
Public Class Perfil

#Region "Variables de trabajo"

    Private idPerfil As Int32
    Private descripcionPerfil As String
    Private dscArea As String
    Private abrArea As String
    Private esVigentePerfil As Boolean
    Private inicioVigenciaPerfil As DateTime
    Private finVigenciaPerfil As DateTime
    Private menusPerfil As List(Of Menu)
    Public existe As Boolean

#End Region

#Region "Propiedades"

    Public Property IdentificadorPerfil As Int32
        Get
            Return idPerfil
        End Get
        Set(ByVal value As Int32)
            idPerfil = value
        End Set
    End Property

    Public Property DescArea As String
        Get
            Return dscArea
        End Get
        Set(ByVal value As String)
            dscArea = value
        End Set
    End Property


    'Public Property AbreArea As String 'MMOB: YA NO SE USA
    '    Get
    '        Return abrArea
    '    End Get
    '    Set(ByVal value As String)
    '        abrArea = value
    '    End Set
    'End Property

    Public Property Descripcion As String
        Get
            Return descripcionPerfil
        End Get
        Set(ByVal value As String)
            descripcionPerfil = value
        End Set
    End Property

    Public Property EsVigente As Boolean
        Get
            Return esVigentePerfil
        End Get
        Set(ByVal value As Boolean)
            esVigentePerfil = value
        End Set
    End Property

    Public Property InicioVigencia As DateTime
        Get
            Return inicioVigenciaPerfil
        End Get
        Set(ByVal value As DateTime)
            inicioVigenciaPerfil = value
        End Set
    End Property

    Public Property FinVigencia As DateTime
        Get
            Return finVigenciaPerfil
        End Get
        Set(ByVal value As DateTime)
            finVigenciaPerfil = value
        End Set
    End Property

    Public Property Menus As List(Of Menu)
        Get
            Return menusPerfil
        End Get
        Set(ByVal value As List(Of Menu))
            menusPerfil = value
        End Set
    End Property


#End Region

#Region "Constructores"

    Public Sub New()

    End Sub

    Public Sub New(ByVal idPerfil As Int32)

        Dim conex As New Conexion.SQLServer

        Try

            Dim dsMenus As DataSet

            'Dim dtDatos As DataTable = conex.ConsultarDT("SELECT N_ID_PERFIL, T_DSC_PERFIL, N_FLAG_VIG, F_FECH_INI_VIG  FROM BDS_C_GR_PERFIL")
            Dim dtDatos As DataTable = conex.ConsultarDT("SELECT N_ID_PERFIL, T_DSC_PERFIL, N_FLAG_VIG, F_FECH_INI_VIG  FROM BDS_C_GR_PERFIL where N_ID_PERFIL = " & idPerfil.ToString)

            If dtDatos.Rows.Count = 0 Then

                Exit Sub

            End If

            Me.IdentificadorPerfil = dtDatos.Rows(0)("N_ID_PERFIL").ToString().ToString()
            Me.Descripcion = dtDatos.Rows(0)("T_DSC_PERFIL").ToString().ToString()
            Me.EsVigente = dtDatos.Rows(0)("N_FLAG_VIG").ToString().ToString()
            Me.InicioVigencia = dtDatos.Rows(0)("F_FECH_INI_VIG").ToString().ToString()

            Dim parametro1(0) As SqlParameter
            parametro1(0) = New SqlParameter("N_ID_PERFIL", idPerfil)

            dsMenus = conex.EjecutarSPConsultaDS("ConsultarMenu", parametro1)

            Dim listMenus As New List(Of Menu)

            For indexMenu = 0 To dsMenus.Tables(0).Rows.Count - 1

                Dim menu1 As New Menu

                menu1.IdentificadorMenu = dsMenus.Tables(0).Rows(indexMenu)("N_ID_MENU").ToString().ToString()
                menu1.Descripcion = dsMenus.Tables(0).Rows(indexMenu)("T_DSC_MENU").ToString().ToString()
                menu1.EsVigente = dsMenus.Tables(0).Rows(indexMenu)("N_FLAG_VIG").ToString().ToString()
                menu1.InicioVigencia = dsMenus.Tables(0).Rows(indexMenu)("F_FECH_INI_VIG").ToString().ToString()
                'menu1.FinVigencia = dsMenus.Tables(0).Rows(indexMenu)("F_FECH_FIN_VIG").ToString().ToString()

                'For indexSubMenu = 1 To dsMenus.Tables.Count - 1

                Dim listSubMenus As New List(Of SubMenu)

                For indexSubMenuRow = 0 To dsMenus.Tables(indexMenu + 1).Rows.Count - 1

                    Dim submenu1 As New SubMenu
                    submenu1.IdentificadorSubMenu = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("N_ID_SUBMENU").ToString().ToString()
                    submenu1.Descripcion = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("T_DSC_SUBMENU").ToString().ToString()
                    submenu1.UrlPagina = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("T_DSC_URL_PAGINA").ToString().ToString()
                    submenu1.DescripcionPagina = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("T_DSC_PAGINA").ToString().ToString()
                    submenu1.EsVigente = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("N_FLAG_VIG").ToString().ToString()
                    submenu1.InicioVigencia = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("F_FECH_INI_VIG").ToString().ToString()
                    'submenu1.FinVigencia = dsMenus.Tables(indexSubMenu).Rows(indexSubMenuRow)("F_FECH_FIN_VIG").ToString().ToString()
                    submenu1.Orden = dsMenus.Tables(indexMenu + 1).Rows(indexSubMenuRow)("N_NUM_ORDEN").ToString().ToString()

                    listSubMenus.Add(submenu1)

                Next

                menu1.SubMenus = listSubMenus
                'Next

                listMenus.Add(menu1)



            Next

            Me.Menus = listMenus

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conex) Then
                conex.CerrarConexion()
            End If

        End Try



    End Sub

    Public Sub New(ByVal idPerfil As Int32, ByVal descripcion As String, ByVal esVigente As Boolean, ByVal inicioVigencia As DateTime, _
                   ByVal finVigencia As DateTime, ByVal menus As List(Of Menu))

        Me.idPerfil = idPerfil
        Me.descripcionPerfil = descripcion
        Me.esVigente = esVigente
        Me.inicioVigenciaPerfil = inicioVigencia
        Me.finVigenciaPerfil = finVigencia
        Me.menusPerfil = menus

    End Sub

    ''' <summary>
    ''' Constructor para crear un perfir con su Id y descripcion
    ''' </summary>
    ''' <param name="idPerfil">Identificador del perfil</param>
    ''' <param name="descripcion">Descripcion del perfil</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal idPerfil As Int32, ByVal descripcion As String)
        Me.idPerfil = idPerfil
        'If descripcion <> "" Then
        CargarDatos()
        'Else
        '    Me.descripcionPerfil = descripcion
        'End If
    End Sub

#End Region



    '- Fecha de creación:26/07/2013
    '- Fecha de modificación:  NA
    '- Nombre del Responsable: Julio Cesar Vieyra Tena
    '- Empresa: Softtek
    '- Metodos de Perfiles

#Region "Metodos"

    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer
        Try

            Return conexion.ConsultarRegistrosDT("BDS_C_GR_PERFIL", "N_ID_PERFIL").DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function
    Public Function ExistePerfil() As Boolean
        Dim conexion As New Conexion.SQLServer
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            listCampos.Add("N_ID_PERFIL") : listValores.Add(Me.idPerfil)
            existe = conexion.BuscarUnRegistro("BDS_C_GR_PERFIL", listCampos, listValores)
            Return existe

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function
    Public Sub CargarDatos()
        Dim conexion As New Conexion.SQLServer
        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            listCampos.Add("N_ID_PERFIL") : listValores.Add(Me.idPerfil)
            existe = conexion.BuscarUnRegistro("BDS_C_GR_PERFIL", listCampos, listValores)
            If existe Then
                Dim dr As SqlClient.SqlDataReader = conexion.ConsultarRegistrosDR("BDS_C_GR_PERFIL", listCampos, listValores)
                If dr.Read() Then
                    Me.descripcionPerfil = CStr(dr("T_DSC_PERFIL"))
                    Me.EsVigente = CBool(dr("N_FLAG_VIG"))
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
    Public Function Agregar() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Try


            Dim bitacora As New Conexion.Bitacora("Registro de nuevo perfil" & "(" & Me.Descripcion & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_PERFIL") : listValores.Add(Me.idPerfil)
            listCampos.Add("T_DSC_PERFIL") : listValores.Add(Me.Descripcion)
            listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)
            resultado = conexion.Insertar("BDS_C_GR_PERFIL", listCampos, listValores)
            bitacora.Insertar("BDS_C_GR_PERFIL", listCampos, listValores, resultado, "Error al registrar nuevo perfil")

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
    Public Function Actualizar() As Boolean
        Dim resultado As Boolean = False

        Dim conexion As New Conexion.SQLServer

        Try


            Dim bitacora As New Conexion.Bitacora("Actualización de perfil" & "(" & Me.Descripcion & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)


            listCampos.Add("T_DSC_PERFIL") : listValores.Add(Me.Descripcion)
            listCamposCondicion.Add("N_ID_PERFIL") : listValoresCondicion.Add(Me.IdentificadorPerfil)
            resultado = conexion.Actualizar("BDS_C_GR_PERFIL", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_PERFIL", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar ayuda")

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
    Public Function Baja() As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer

        Try


            Dim bitacora As New Conexion.Bitacora("Borrar Perfil" & "(" & Me.IdentificadorPerfil & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("N_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_PERFIL") : listValoresCondicion.Add(Me.IdentificadorPerfil)

            resultado = conexion.Actualizar("BDS_C_GR_PERFIL", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_C_GR_PERFIL", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar el perfil")
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
    Public Function ObtenerSiguienteIdentificadorPerfil() As Integer
        Dim resultado As Integer = 1
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_PERFIL) + 1) N_ID_PERFIL FROM BDS_C_GR_PERFIL")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_PERFIL")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_PERFIL"))
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
    ''' Crea un Perfil con Id y descripcion
    ''' </summary>
    ''' <param name="row">Datarow de la tabla BDS_C_GR_PERFIL</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreaPerfil(ByVal row As DataRow) As Perfil
        Return New Perfil(Convert.ToInt32(row.Item("N_ID_PERFIL")),
                          row.Item("T_DSC_PERFIL").ToString)
    End Function

    ''' <summary>
    ''' Obtiene una lista de perfiles vigentes
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenerPerfilesVigentes() As List(Of Perfil)
        Dim strQuery As String = ""

        Dim conexion As New Conexion.SQLServer
        Try
            Dim cadena As String = "SELECT N_ID_PERFIL, T_DSC_PERFIL FROM BDS_C_GR_PERFIL WHERE N_FLAG_VIG = 1"

            Dim dt As DataTable = conexion.ConsultarDT(cadena)

            Dim lstPerfil As New List(Of Perfil)

            For Each dr As DataRow In dt.Rows
                lstPerfil.Add(CreaPerfil(dr))
            Next

            Return lstPerfil

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    'Metodo Sobrecargado RRA: Agrega area al buscar un perfil para correos
    Public Function ObtenerPerfilesVigentes(ByVal valor As String) As List(Of Perfil)
        Dim strQuery As String = ""

        Dim conexion As New Conexion.SQLServer
        Try
            Dim cadena As String = "SELECT DISTINCT  P.N_ID_PERFIL, P.T_DSC_PERFIL FROM  BDS_R_GR_USUARIO_PERFIL UP " + _
                                    "INNER JOIN BDS_C_GR_PERFIL P ON P.N_ID_PERFIL = UP.N_ID_PERFIL " + _
                                    "WHERE UP.I_ID_AREA = " + valor + " AND P.N_FLAG_VIG = 1"

            Dim dt As DataTable = conexion.ConsultarDT(cadena)

            Dim lstPerfil As New List(Of Perfil)

            For Each dr As DataRow In dt.Rows
                lstPerfil.Add(CreaPerfil(dr))
            Next

            Return lstPerfil

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    Public Function ActualizaFunciones(ByVal agregados As List(Of Grupo), ByVal eliminados As List(Of Grupo), ByVal idPerfil As Integer) As Boolean
        Dim campos As List(Of Object)
        Dim condicion As List(Of String)
        Dim valores As List(Of Object)

        Dim conexion As New Conexion.SQLServer
        Try

            For Each agregado As Grupo In agregados
                campos = New List(Of Object)
                campos.Add(idPerfil)
                campos.Add(agregado.Identificador)
                conexion.Insertar("BDS_R_GR_PERFIL_GRUPO", campos)
            Next

            For Each eliminado As Grupo In eliminados
                condicion = New List(Of String)
                valores = New List(Of Object)

                condicion.Add("N_ID_PERFIL") : valores.Add(idPerfil)
                condicion.Add("N_ID_GRUPO") : valores.Add(eliminado.Identificador)

                conexion.Eliminar("BDS_R_GR_PERFIL_GRUPO", condicion, valores)
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

    Public Function ObtenerGrupos(ByVal idPerfil As Integer) As List(Of Grupo)
        Dim lstGrupo As New List(Of Grupo)
        Dim strQuery As String = "SELECT G.N_ID_GRUPO, G.T_DSC_GRUPO FROM BDS_C_GR_GRUPO G INNER JOIN BDS_R_GR_PERFIL_GRUPO PG " & _
                                 "ON G.N_ID_GRUPO = PG.N_ID_GRUPO INNER JOIN BDS_C_GR_PERFIL P ON P.N_ID_PERFIL = PG.N_ID_PERFIL " & _
                                 "WHERE P.N_ID_PERFIL = " & idPerfil
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                Dim objGrupo As New Grupo
                objGrupo.Identificador = dr.Item("N_ID_GRUPO").ToString
                objGrupo.Descripcion = dr.Item("T_DSC_GRUPO").ToString
                lstGrupo.Add(objGrupo)
            Next

            Return lstGrupo

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function

    Public Function ObtenerGruposDisponibles(ByVal idPerfil As Integer) As List(Of Grupo)
        Dim lstGrupo As New List(Of Grupo)
        Dim strQuery As String = "SELECT N_ID_GRUPO, T_DSC_GRUPO FROM BDS_C_GR_GRUPO WHERE N_ID_GRUPO NOT IN ( " & _
                                 "SELECT G.N_ID_GRUPO FROM BDS_C_GR_GRUPO G INNER JOIN BDS_R_GR_PERFIL_GRUPO PG " & _
                                 "ON G.N_ID_GRUPO = PG.N_ID_GRUPO INNER JOIN BDS_C_GR_PERFIL P ON P.N_ID_PERFIL = PG.N_ID_PERFIL " & _
                                 "WHERE P.N_ID_PERFIL = " & idPerfil & " ) AND B_FLAG_VIG = 1 "
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

            For Each dr As DataRow In dt.Rows
                Dim objGrupo As New Grupo
                objGrupo.Identificador = dr.Item("N_ID_GRUPO").ToString
                objGrupo.Descripcion = dr.Item("T_DSC_GRUPO").ToString
                lstGrupo.Add(objGrupo)
            Next

            Return lstGrupo

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
