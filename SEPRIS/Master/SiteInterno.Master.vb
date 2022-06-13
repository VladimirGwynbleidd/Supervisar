Imports Entities

Public Class SiteInterno
    Inherits System.Web.UI.MasterPage

    '***********************************************************************************************************
    ' Fecha Creación:       09 Julio 2013
    ' Codificó:             Jorge Alberto Rangel Ruiz
    ' Empresa:              Softtek
    ' Descripción           Master Page que se empleará para los sistemas, funciona tanto para las páginas 
    ' de Negocio (funcionales), como las de Login, Logout y las que se definan para el perfil Sin Accceso 
    ' (Tipicamente permiten ver las páginas Acerca de, Manual de Usuario, Responsables, etc.
    '***********************************************************************************************************

#Region "Constantes"

    ' Constantes para identificar las variables de sesión de los valores de Tiempo de sesión y confirmacion
    Public Const SessionTiempoName As String = "SessionTiempo"
    Public Const SessionTiempoSegundosName As String = "SessionTiempoSegundos"
    Public Const SessionTiempoConfirmacionName As String = "SessionTiempoConfirmacion"

    ' Propiedad Publica de solo lectura para identificar la variable de sesión que almacena el Menu seleccionado actual
    Public ReadOnly Property SessionCurrentMenu As String
        Get
            Return "SessionCurrentMenu"
        End Get
    End Property

    ' Propiedad Publica de solo lectura para identificar la variable de sesión que almacena el SubMenu seleccionado actual
    Public ReadOnly Property SessionCurrentSubMenu As String
        Get
            Return "SessionCurrentSubMenu"
        End Get
    End Property

    ' Propiedad Publica de solo lectura para identificar la variable de sesión que almacena la URL del SubMenu seleccionado actual (segun Base de datos)
    Public ReadOnly Property SessionCurrentURLSubMenu As String
        Get
            Return "SessionCurrentURLSubMenu"
        End Get
    End Property


    ''' <summary>
    ''' Enumerador para identificar el estado de la sesión
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Estado
        ConSesion
        SinSesion
        Externo
    End Enum

#End Region

    Public ReadOnly Property ppObjVisita As Visita
        Get
         If IsNothing(Session("DETALLE_VISITA")) Then
            'Return Nothing
            Dim objVisita As Visita = CType(Session("DETALLE_VISITA_V17"), Visita)
            If Not IsNothing(objVisita) Then
               Return objVisita
            Else
               Return Nothing
            End If
         Else
            Dim objVisita As Visita = CType(Session("DETALLE_VISITA"), Visita)
            If Not IsNothing(objVisita) Then
               Return objVisita
            Else
               Return Nothing
            End If
         End If
        End Get
    End Property

    Public Property puObjUsuario As Usuario
        Get
            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                Return CType(Session(Entities.Usuario.SessionID), Usuario)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Usuario)
            Session(Entities.Usuario.SessionID) = value
        End Set
    End Property

   Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

      'liveclock.Text = UppercaseWords(DateTime.Now.ToString("dddd dd MMMM 'de' yyyyy")) + " " + DateTime.Now.ToString("hh:mm:ss tt")

      ' Verificamos el estado de la sesion
      Select Case VerificaSesion()

         Case Estado.SinSesion

            ' Si no hay sesión abierta, termina con el flujo
            Exit Sub

         Case Estado.Externo

            ' Si es una página externa, se carga el menu indicado y termina el flujo
            CargaSubMenuExterno()

            Exit Sub

         Case Estado.ConSesion

            ' Si se tiene sesión.

            ' Se reinicia el conteo para manejo de sesión
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Reinicia", "ReiniciaTiempo();", True)

                ' Se carga el menu y submenu actuales
                CargaMenu()
                CargaSubMenu()

         Case Else
            Throw New NotImplementedException("Valor de estado de sesion no contemplado")

      End Select

      Session(SessionCurrentURLSubMenu) = Me.Page.AppRelativeVirtualPath

      If Not IsPostBack Then

         CargaTiemposSesion()
         CargaPie()
         CargaNombreUsuario()
         CargaAlineacionMenu()

      End If


   End Sub


   Private Sub LiberaVisita()

      If IsNothing(Session("DETALLE_VISITA")) Then
         If Not IsNothing(ppObjVisita) And Not IsNothing(puObjUsuario) Then
            If ppObjVisita.UsuarioEstaOcupando = puObjUsuario.IdentificadorUsuario Then
               AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, Date.Now, Constantes.TipoFecha.LimpiaBanderaDeApropiacion, "", puObjUsuario.IdentificadorUsuario)
               Session.Remove("DETALLE_VISITA_V17")
            End If
         End If
      Else
         If Not IsNothing(ppObjVisita) And Not IsNothing(puObjUsuario) Then
            If ppObjVisita.UsuarioEstaOcupando = puObjUsuario.IdentificadorUsuario Then
               AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, Date.Now, Constantes.TipoFecha.LimpiaBanderaDeApropiacion, "", puObjUsuario.IdentificadorUsuario)
               Session.Remove("DETALLE_VISITA")
            End If
         End If
      End If

   End Sub

#Region "SECCION PARA EL MANEJO DE SESIONES, trabaja junto con el control wucVencimientoSesion.ascx nombrado aqui como 'ucManejaSesion'"

   ''' <summary>
   ''' Método encargado de cargas los tiempos de sesion y confirmacion para el manejo de sesiones
   ''' </summary>
   ''' <remarks>Debe emplar los parámetros TiempoSesion y TiempoConfirmacion del Catálogo de Parametros</remarks>
   Private Sub CargaTiemposSesion()

      If IsNothing(Session(SessionTiempoName)) Then

         Session(SessionTiempoName) = Conexion.SQLServer.Parametro.ObtenerValor("TiempoSesion")
         Session(SessionTiempoSegundosName) = (CInt(Session(SessionTiempoName)) * 60) - 1
         Session(SessionTiempoConfirmacionName) = Conexion.SQLServer.Parametro.ObtenerValor("TiempoConfirmacion")

      End If

      ucManejaSesion.TiempoSesion = Session(SessionTiempoName).ToString()
      ucManejaSesion.TiempoSesionSegundos = Session(SessionTiempoSegundosName).ToString()
      ucManejaSesion.TiempoConfirmacion = Session(SessionTiempoConfirmacionName).ToString()


   End Sub

   ''' <summary>
   '''  Método de boton oculto
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Private Sub btnSalirVencimientoSesion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalirVencimientoSesion.Click
      'CType(Session(Entities.Usuario.SessionID), Entities.Usuario).FinalizarSesion()
      LiberaVisita()
      RedireccionaLogOut()

   End Sub

   ''' <summary>
   ''' Método para continuar con la sesion
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Private Sub btnContinuarVencimientoSesion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinuarVencimientoSesion.Click

      CargaTiemposSesion()

      ManejaSesion()

   End Sub

#End Region


   ''' <summary>
   ''' Verificamos que la sesion del usuario esté activa, de lo contrario se redirecciona a la salida del sistema
   ''' </summary>
   ''' <remarks></remarks>
   Private Function VerificaSesion() As SiteInterno.Estado

      ' Obtenemos la página que nos está cargando
      Dim _pagina As String = Web.VirtualPathUtility.GetFileName(Me.Page.AppRelativeVirtualPath).Trim.ToLower



      ' En caso de que la sesion ya se haya terminado
      If IsNothing(Session(Entities.Usuario.SessionID)) Then



         ' Si la página que nos invoca es el login o logout, regresamos Estado.Externo
         If _pagina = SEPRIS.Login.Nombre.Trim.ToLower OrElse
             _pagina = SEPRIS.Logout.Nombre.Trim.ToLower Then

            Return Estado.Externo

         End If




         ' Obtenemos los menus del perfil sin acceso
         Dim perfilCero As New Entities.Perfil(0)

         For Each menuPerfilCero As Entities.SubMenu In perfilCero.Menus(0).SubMenus

            ' Si la página que nos invoca pertenece al perfil cero, regresamos Estado.Externo
            If _pagina = Web.VirtualPathUtility.GetFileName(menuPerfilCero.UrlPagina).Trim.ToLower Then
               Return Estado.Externo
            End If

         Next



         ' Si la página que nos invoca NO es el login o logout o alguna del perfil sin acceso, regresamos Estado.SinSesion
         RedireccionaLogOut()
         Return Estado.SinSesion


      End If

      ' En caso de tener sesion y ser llamado por la página de CambioContrasenia
      If Not IsNothing(Session(Entities.Usuario.SessionID)) Then


         If _pagina = SEPRIS.CambioContrasenia.Nombre.Trim.ToLower Then

            Return Estado.Externo

         End If

      End If

      ' Se valida si no existe una sesion mas nueva del usuario
      If Entities.Usuario.BuscaSecionNueva() Then
         RedireccionaLogOut()
         Return Estado.SinSesion
      End If


      Return Estado.ConSesion

   End Function


   ''' <summary>
   ''' Método que redirecciona a la página de salida del sistema
   ''' </summary>
   ''' <remarks></remarks>
   Public Sub RedireccionaLogOut()

      Response.Redirect("~/Logout.aspx", False)

   End Sub

   ''' <summary>
   ''' Método que redirecciona a la página principal cada vez que se cambia entre menús
   ''' </summary>
   ''' <remarks></remarks>
   Public Sub RedireccionaPrincipal()

      Response.Redirect("~/Principal.aspx", False)

   End Sub


    ''' <summary>
    ''' Método encargado de cargar y presentar los datos del pie de página
    ''' </summary>
    ''' <remarks></remarks>



    Private Sub CargaPie()

        lblPie.Text = Conexion.SQLServer.Parametro.ObtenerValor("PiePagina")

    End Sub

    ''' <summary>
    ''' Método que carga el menu segun el perfil del usuario firmado en el sistema
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaMenu()

      TablaMenu.Rows.Clear()

      Dim _row As New Web.UI.HtmlControls.HtmlTableRow()

      Dim _cell As Web.UI.HtmlControls.HtmlTableCell
      Dim _cellComodin As Web.UI.HtmlControls.HtmlTableCell

      ' Tomamos el menu del usuario firmado en el sistema y por cada elemento, creamos su elemento
      For Each _menu As Entities.Menu In CType(Session(Entities.Usuario.SessionID), Entities.Usuario).Perfiles(0).Menus.OrderBy(Function(x) x.Orden).ToList()

         _cell = New Web.UI.HtmlControls.HtmlTableCell()
         _cellComodin = New Web.UI.HtmlControls.HtmlTableCell()

         _cell.InnerText = _menu.Descripcion
         _cell.Attributes.Add("class", "txt_menu")
            _cell.Attributes.Add("onclick", "ClickMenu(" & _menu.IdentificadorMenu.ToString() & "); return false;")

            _cellComodin.InnerText = " * "
            _cellComodin.Attributes.Add("class", "txt_menu_comodin")

            _row.Cells.Add(_cell)
         _row.Cells.Add(_cellComodin)

      Next

      ' Creamos el elemento para Cerrar Sesion
      _cell = New Web.UI.HtmlControls.HtmlTableCell()
      _cell.InnerText = "Cerrar Sesión"
      _cell.Attributes.Add("class", "txt_menu")
      _cell.Attributes.Add("onclick", "ClickMenu(-1)")
      _row.Cells.Add(_cell)

      ' agregamos el renglon al Menu
      TablaMenu.Rows.Add(_row)

        TablaMenu.EnableViewState = True


    End Sub

   ''' <summary>
   ''' Método que carga el submenu segun el menu elegido
   ''' </summary>
   ''' <remarks></remarks>
   Private Sub CargaSubMenu()

      If CInt(Session(SessionCurrentMenu)) <= 0 Then
         Exit Sub
      End If

      Dim _row As Web.UI.HtmlControls.HtmlTableRow
      Dim _cell As Web.UI.HtmlControls.HtmlTableCell
      Dim lsNombreMenu As String = ""

      ' Tomamos el menu del usuario firmado en el sistema segun el seleccionado
      Dim subMenu As IEnumerable(Of Entities.Menu) = From items In CType(Session(Entities.Usuario.SessionID), Entities.Usuario).Perfiles(0).Menus
                    Where items.IdentificadorMenu = CInt(Session(SessionCurrentMenu))

      TablaSubMenu.Rows.Clear()


      _row = New Web.UI.HtmlControls.HtmlTableRow()
      _cell = New Web.UI.HtmlControls.HtmlTableCell()
      lsNombreMenu = CType(subMenu(0), Entities.Menu).Descripcion

      ' Creamos el Header del SubMenu
      _cell.InnerText = lsNombreMenu
      _cell.Attributes.Add("class", "txt_submenu_header TemaFondoOscuro")
      _row.Cells.Add(_cell)
      TablaSubMenu.Rows.Add(_row)


      'METODO ANTIGUO PARA DESPLEGAR SUBMENUS, NO FUNCIONADA ADECUADAMENTE CUANDO A LOS MENUS SE LES AGREGO LA COLUMNA DE ORDEN
      ' Por cada elemento del submenu del menu seleccionad, creamos su elemento
      'For Each _menu As Entities.SubMenu In CType(subMenu(0), Entities.Menu).SubMenus.OrderBy(Function(x) x.Orden).ToList()
      '    _row = New Web.UI.HtmlControls.HtmlTableRow()
      '    _cell = New Web.UI.HtmlControls.HtmlTableCell()

      '    _cell.InnerText = _menu.Descripcion
      '    _cell.Attributes.Add("class", "txt_submenu")
      '    _cell.Attributes.Add("onclick", "ClickSubMenu('" & _menu.UrlPagina & "', " & _menu.IdentificadorSubMenu.ToString() & ")")

      '    _row.Cells.Add(_cell)

      '    TablaSubMenu.Rows.Add(_row)

      'Next

      'NUEVO METODO PARA MOSTRAR LOS SUBMENUS
      Dim objSubM As New Entities.SubMenu
      objSubM.IdentificadorMenu = CInt(Session(SessionCurrentMenu))
      objSubM.IdentificadorPerfil = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).Perfiles(0).IdentificadorPerfil

      Dim objUsu As Entities.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
      Dim dtSubMenus As DataTable

      If Not IsNothing(objUsu) And lsNombreMenu.ToUpper().Contains("Seguridad".ToUpper()) Then
         If objUsu.IdentificadorPerfilActual = Constantes.PERFIL_ADM And Constantes.EsAreaSeprisSnPrec(objUsu.IdArea) Then
            dtSubMenus = objSubM.MuestraSubmenusFiltrado()
         Else
            dtSubMenus = objSubM.MuestraSubmenus()
         End If
      ElseIf Not IsNothing(objUsu) And lsNombreMenu.ToUpper().Contains("Mantenimiento".ToUpper()) Then
         If objUsu.IdArea = Constantes.AREA_PR Then
            dtSubMenus = objSubM.MuestraSubmenus()
         Else
            dtSubMenus = objSubM.MuestraSubmenusSinImgProceso()
         End If
      Else
         dtSubMenus = objSubM.MuestraSubmenus()
      End If



      If dtSubMenus.Rows.Count > 0 Then
         For Each dr As DataRow In dtSubMenus.Rows

            _row = New Web.UI.HtmlControls.HtmlTableRow()
            _cell = New Web.UI.HtmlControls.HtmlTableCell()

            _cell.InnerText = dr("T_DSC_SUBMENU")
            _cell.Attributes.Add("class", "txt_submenu")
                _cell.Attributes.Add("onclick", "ClickSubMenu('" & dr("T_DSC_URL_PAGINA") & "', " & dr("N_ID_SUBMENU").ToString() & "); return false;")

                _row.Cells.Add(_cell)

            TablaSubMenu.Rows.Add(_row)
         Next
      End If

      ' agregamos el renglon al Menu
      TablaSubMenu.EnableViewState = True


   End Sub

   ''' <summary>
   ''' Método que carga el submenu para las páginas externas
   ''' </summary>
   ''' <remarks></remarks>
   Private Sub CargaSubMenuExterno()

      CargaPie()

      ' ajustamos estilo de pie
      Me.Pie.Attributes.Remove("class")
      Me.Pie.Attributes.Add("class", "TemaPieHeaderCentro div_pie_externo")

      TablaMenu.Rows.Clear()
      ucManejaSesion.Visible = False

      ' Cargamos el menu para el prefil sin Acceso
      Dim perfilCero As New Entities.Perfil(0)

      Dim _row As Web.UI.HtmlControls.HtmlTableRow
      Dim _cell As Web.UI.HtmlControls.HtmlTableCell

      _row = New Web.UI.HtmlControls.HtmlTableRow()
      _cell = New Web.UI.HtmlControls.HtmlTableCell()

      ' Creamos el Header del SubMenu
      _cell.InnerHtml = "&nbsp;"
      _cell.Attributes.Add("class", "txt_submenu_header TemaFondoOscuro")
      _row.Cells.Add(_cell)
      TablaSubMenu.Rows.Add(_row)


      ' Tomamos el menu cargado y por cada elemento, creamos su elemento
      For Each menuPerfilCero As Entities.SubMenu In perfilCero.Menus(0).SubMenus.OrderBy(Function(x) x.Orden).ToList()

         _row = New Web.UI.HtmlControls.HtmlTableRow()
         _cell = New Web.UI.HtmlControls.HtmlTableCell()

         _cell.InnerText = menuPerfilCero.Descripcion
         _cell.Attributes.Add("class", "txt_submenu")
         _cell.Attributes.Add("onclick", "ClickSubMenu('" & menuPerfilCero.UrlPagina & "', " & menuPerfilCero.IdentificadorSubMenu.ToString() & ")")
         _row.Cells.Add(_cell)

         TablaSubMenu.Rows.Add(_row)

      Next

      TablaSubMenu.EnableViewState = True

   End Sub

   ''' <summary>
   ''' Método encargado de tomar el nombre del usuario firmado actualmente en el sistema para mostrarlo
   ''' </summary>
   ''' <remarks></remarks>
   Public Sub CargaNombreUsuario()

      lblUsuarioNombre.Text = "Bienvenido: " & CType(Session(Entities.Usuario.SessionID), Entities.Usuario).Nombre & _
          " " & CType(Session(Entities.Usuario.SessionID), Entities.Usuario).Apellido & _
          " " & CType(Session(Entities.Usuario.SessionID), Entities.Usuario).ApellidoAuxiliar
      hdnCurrentUser.Value = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario


   End Sub

   Public Sub ManejaSesion()

   End Sub

   ''' <summary>
   ''' Método que atrapa en el evento de selección de un elemento del Menu
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Private Sub btnClickMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClickMenu.Click
      ''Libera la visita
      LiberaVisita()

      ' Verificamos sesion (se hace debido a que se invoca mediante javascript
      Select Case VerificaSesion()

         Case Estado.SinSesion

            Exit Sub

      End Select

      ' se dio clic en Cerrar Sesion
      If CInt(hdnCurrentMenu.Value) = -1 Then

         CType(Session(Entities.Usuario.SessionID), Entities.Usuario).FinalizarSesion()
         RedireccionaLogOut()

         Exit Sub
      Else
         RedireccionaPrincipal()
      End If

      ' Obtenemos menu selecciponado y cargamos submenu
      Session(SessionCurrentMenu) = CInt(hdnCurrentMenu.Value)
      CargaSubMenu()
   End Sub

   ''' <summary>
   '''  Método que atrapa en el evento de selección de un elemento del SubMenu
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Private Sub btnClickSubMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClickSubMenu.Click
      ''Libera la visita
      LiberaVisita()

      ' Verificamos sesion (se hace debido a que se invoca mediante javascript
      Select Case VerificaSesion()

         Case Estado.SinSesion

            Exit Sub

      End Select

      Session(SessionCurrentSubMenu) = 0

      ' Si el elemento seleccionado tiene página se redirecciona a ésta
      If hdnCurrentSubMenu.Value.Trim <> "" Then

         Response.Redirect(hdnCurrentSubMenu.Value, False)
         Session(SessionCurrentSubMenu) = CInt(hdnCurrentIdentificadorSubMenu.Value)
         Session(SessionCurrentURLSubMenu) = hdnCurrentSubMenu.Value

      End If
   End Sub

   ''' <summary>
   ''' Método que determina la alineación del Menú según la configuración en catalogo de parametros
   ''' </summary>
   ''' <remarks></remarks>
   Private Sub CargaAlineacionMenu()

      Try

         ' Obtenemos parametro
         Dim alineacion As String = Conexion.SQLServer.Parametro.ObtenerValor("MenuAlineacion")

         Select Case alineacion


            Case "I"
               ' En caso de alinearse a la izquierda
               TablaMenu.Attributes.Add("align", "left")
               TablaMenu.Style.Remove("width")


            Case "D"
               ' En caso de alinearse a la derecha

               TablaMenu.Attributes.Add("align", "right")
               TablaMenu.Style.Remove("width")

            Case Else

               ' En caso contrario, lo centramos
               Throw New Exception("Lo centramos")

         End Select

      Catch ex As Exception

         ' En caso de no alinearse a la izquierda, derecha o no encontrarse el parametro, se toma el valor default, centrado
         TablaMenu.Attributes.Add("align", "center")
         TablaMenu.Style.Add("width", "100%")

      End Try

   End Sub

   'Protected Sub TimerTime_Tick(sender As Object, e As EventArgs) Handles TimerTime.Tick
   '    liveclock.Text = UppercaseWords(DateTime.Now.ToString("dddd dd MMMM 'de' yyyy")) + " " + DateTime.Now.ToString("hh:mm:ss tt")
   'End Sub

   Private Function UppercaseWords(ByVal texto As String) As String

      Dim array() As Char = texto.ToCharArray()

      If array.Length >= 1 Then
         If Char.IsLower(array(0)) Then
            array(0) = Char.ToUpper(array(0))
         End If
      End If

      For index = 1 To array.Length
         If array(index - 1) = " " Then
            If Char.IsLower(array(index)) Then
               array(index) = Char.ToUpper(array(index))
            End If
         End If
      Next
      Return array
   End Function

   Public Sub SetTexto(psTexto As String)
      hdnCurrentMenu.Value = psTexto
   End Sub

   Public Function GetTexto() As String
      Return hdnCurrentMenu.Value
   End Function

   Protected Overrides Sub Finalize()
      MyBase.Finalize()
   End Sub

    Protected Sub btnSalirVencimientoSesion_Click1(sender As Object, e As EventArgs)
        Response.Redirect("~/Login.aspx")
    End Sub
End Class