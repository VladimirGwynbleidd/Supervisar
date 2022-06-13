'- Fecha de creación:25/07/2013
'- Fecha de modificación:  NA
'- Nombre del Responsable: Julio Cesar Vieyra Tena
'- Empresa: Softtek
'- Clase para Catalogo de Ayuda

<Serializable()>
Public Class Ayuda
    Public Property IdentificadorMenu As Integer
    Public Property IdentificadorSubmenu As Integer
    Public Property IdentificadorAyuda As Integer
    Public Property IdenticadorPadre As Integer
    Public Property Titulo As String
    Public Property Contenido As String
    Public Property orden As String
    Public Property Existe As Boolean = False
    Public Property Vigente As Boolean = False
    Public Sub New()

    End Sub
    Public Sub New(ByVal idMenu As Integer, ByVal idSubmenu As Integer, ByVal idAyuda As Integer)
        Me.IdentificadorMenu = idMenu
        Me.IdentificadorSubmenu = idSubmenu
        Me.IdentificadorAyuda = idAyuda
        CargarDatos()
    End Sub
    Public Sub CargarDatos()

        Dim conexion As New Conexion.SQLServer

        Try

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_MENU") : listValores.Add(Me.IdentificadorMenu)
            listCampos.Add("N_ID_SUBMENU") : listValores.Add(Me.IdentificadorSubmenu)
            listCampos.Add("N_ID_AYUDA") : listValores.Add(Me.IdentificadorAyuda)

            Existe = conexion.BuscarUnRegistro("BDS_R_GR_MENU_SUBMENU_PAGINA_AYUDA", listCampos, listValores)

            If Existe Then

                Dim dr As SqlClient.SqlDataReader = conexion.ConsultarRegistrosDR("BDS_R_GR_MENU_SUBMENU_PAGINA_AYUDA", listCampos, listValores)

                If dr.Read() Then

                    Me.Titulo = CStr(dr("T_DSC_TITULO"))
                    Me.Contenido = CStr(dr("T_DSC_CONTENIDO"))
                    Me.orden = If(IsDBNull(dr("N_NUM_ORDEN")), Nothing, CStr(dr("N_NUM_ORDEN")))
                    Me.IdenticadorPadre = CStr(dr("N_ID_AYUDA_PADRE"))
                    Me.Vigente = CBool(dr("N_FLAG_VIG"))

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
    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer
        Try

            Return conexion.ConsultarRegistrosDT("BDS_R_GR_MENU_SUBMENU_PAGINA_AYUDA", "N_ID_AYUDA").DefaultView

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function
    Public Function Agregar() As Boolean

        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer
        Try


            Dim bitacora As New Conexion.Bitacora("Registro de nueva Ayuda" & "(" & Me.Titulo.ToString() & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add("N_ID_MENU") : listValores.Add(Me.IdentificadorMenu)
            listCampos.Add("N_ID_SUBMENU") : listValores.Add(Me.IdentificadorSubmenu)
            listCampos.Add("N_ID_AYUDA") : listValores.Add(Me.IdentificadorAyuda)
            listCampos.Add("N_ID_AYUDA_PADRE") : listValores.Add(Me.IdenticadorPadre)
            listCampos.Add("T_DSC_TITULO") : listValores.Add(Me.Titulo)
            listCampos.Add("T_DSC_CONTENIDO") : listValores.Add(Me.Contenido)
            listCampos.Add("N_NUM_ORDEN") : listValores.Add(If(Me.orden <> String.Empty, Me.orden, DBNull.Value))
            listCampos.Add("N_FLAG_VIG") : listValores.Add(1)
            listCampos.Add("F_FECH_INI_VIG") : listValores.Add(Date.Now)

            resultado = conexion.Insertar("BDS_R_GR_MENU_SUBMENU_PAGINA_AYUDA", listCampos, listValores)
            bitacora.Insertar("BDS_R_GR_MENU_SUBMENU_PAGINA_AYUDA", listCampos, listValores, resultado, "Error al registrar nueva ayuda")
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


            Dim bitacora As New Conexion.Bitacora("Actualización de Ayuda" & "(" & Me.Titulo.ToString() & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("T_DSC_TITULO") : listValores.Add(Me.Titulo)
            listCampos.Add("T_DSC_CONTENIDO") : listValores.Add(Me.Contenido)
            listCampos.Add("N_NUM_ORDEN") : listValores.Add(If(Me.orden <> String.Empty, Me.orden, DBNull.Value))


            listCamposCondicion.Add("N_ID_MENU") : listValoresCondicion.Add(Me.IdentificadorMenu)
            listCamposCondicion.Add("N_ID_SUBMENU") : listValoresCondicion.Add(Me.IdentificadorSubmenu)
            listCamposCondicion.Add("N_ID_AYUDA") : listValoresCondicion.Add(Me.IdentificadorAyuda)


            resultado = conexion.Actualizar("BDS_R_GR_MENU_SUBMENU_PAGINA_AYUDA", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_R_GR_MENU_SUBMENU_PAGINA_AYUDA", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al actualizar ayuda")
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


            Dim bitacora As New Conexion.Bitacora("Borrar Ayuda" & "(" & Me.IdentificadorMenu.ToString() & ")", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)
            Dim listCamposCondicion As New List(Of String)
            Dim listValoresCondicion As New List(Of Object)

            listCampos.Add("N_FLAG_VIG") : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)

            listCamposCondicion.Add("N_ID_MENU") : listValoresCondicion.Add(Me.IdentificadorMenu)
            listCamposCondicion.Add("N_ID_SUBMENU") : listValoresCondicion.Add(Me.IdentificadorSubmenu)
            listCamposCondicion.Add("N_ID_AYUDA") : listValoresCondicion.Add(Me.IdentificadorAyuda)


            resultado = conexion.Actualizar("BDS_R_GR_MENU_SUBMENU_PAGINA_AYUDA", listCampos, listValores, listCamposCondicion, listValoresCondicion)
            bitacora.Actualizar("BDS_R_GR_MENU_SUBMENU_PAGINA_AYUDA", listCampos, listValores, listCamposCondicion, listValoresCondicion, resultado, "Error al eliminar ayuda")
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
    Public Function ObtenerSiguienteIdentificadorAyuda() As Integer

        Dim resultado As Integer = 1

        Dim conexion As New Conexion.SQLServer

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_AYUDA) + 1) N_ID_AYUDA FROM BDS_R_GR_MENU_SUBMENU_PAGINA_AYUDA")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_AYUDA")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_AYUDA"))
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
    Public Function ObtenerTodosFiltro(ByVal Menu As Integer, ByVal SubMenu As Integer) As DataSet

        Dim listCamposCondicion As New List(Of String)
        Dim listValoresCondicion As New List(Of Object)
        listCamposCondicion.Add("N_ID_MENU") : listValoresCondicion.Add(Menu)
        listCamposCondicion.Add("N_ID_SUBMENU") : listValoresCondicion.Add(SubMenu)
        Dim conexion As New Conexion.SQLServer
        Try

            Return conexion.ConsultarRegistrosDS("BDS_R_GR_MENU_SUBMENU_PAGINA_AYUDA", listCamposCondicion, listValoresCondicion, "N_ID_AYUDA")


        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Obtiene un Datatable con los temas de ayuda ordenados
    ''' </summary>
    ''' <returns>Datatable con ayuda ordenada</returns>
    ''' <remarks></remarks>
    Public Function ObtenerIndice(Optional ByVal filtro As String = "") As DataTable

        Dim strQuery As String = "SELECT MSPA.N_ID_MENU, M.T_DSC_MENU, MSPA.N_ID_SUBMENU, MSP.T_DSC_SUBMENU, MSPA.N_ID_AYUDA, " + _
                                 "MSPA.N_ID_AYUDA_PADRE, MSPA.T_DSC_TITULO, MSPA.T_DSC_CONTENIDO, MSPA.N_NUM_ORDEN " + _
                                 "FROM BDS_R_GR_MENU_SUBMENU_PAGINA_AYUDA MSPA " + _
                                 "INNER JOIN BDS_C_GR_MENU M ON M.N_ID_MENU = MSPA.N_ID_MENU " + _
                                 "INNER JOIN BDS_R_GR_MENU_SUBMENU_PAGINA MSP ON MSP.N_ID_SUBMENU = MSPA.N_ID_SUBMENU AND M.N_ID_MENU = MSP.N_ID_MENU " + _
                                 "WHERE MSPA.N_FLAG_VIG = 1 " + filtro + _
                                 "ORDER BY MSPA.N_ID_MENU, MSPA.N_ID_SUBMENU, " + _
                                 "CASE WHEN MSPA.N_ID_AYUDA_PADRE = 0 THEN 0 ELSE 1 END, " + _
                                 "MSPA.N_NUM_ORDEN,  MSPA.N_ID_AYUDA, MSPA.N_ID_AYUDA_PADRE "

        Dim conexion As New Conexion.SQLServer

        Try

            Dim dt As DataTable = conexion.ConsultarDT(strQuery)

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
