'- Fecha de creación:25/07/2013
'- Fecha de modificación:  NA
'- Nombre del Responsable: Julio Cesar Vieyra Tena
'- Empresa: Softtek
'- Catalogo de Temas de Ayuda


Public Class CatalogoAyuda
    Inherits System.Web.UI.Page

    Public Property Mensaje As String


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        For argument As Integer = 0 To gvConsulta.Rows.Count - 1
            ClientScript.RegisterForEventValidation(btnConsulta.UniqueID, argument)
        Next

        MyBase.Render(writer)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            CargarCatalogo()
            CargarCombos()
            CargarFiltros()
            CargarImagenesEstatus()

        End If

    End Sub

    Protected Sub btnExportaExcel_Click(sender As Object, e As EventArgs) Handles btnExportaExcel.Click
        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns("N_FLAG_VIG").ColumnName = "Estatus"

        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Ayuda", referencias)
    End Sub


    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub

    Private Sub CargarFiltros()

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Vigencia  ", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSource, "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, -1)
        ucFiltro1.AddFilter("Menú", ucFiltro.AcceptedControls.DropDownList, ViewState("MenusAyuda"), "T_DSC_MENU", "N_ID_MENU", ucFiltro.DataValueType.IntegerType, False, False, False, False, False)
        ucFiltro1.AddFilter("Submenú", ucFiltro.AcceptedControls.DropDownList, ViewState("SubMenusAyuda"), "T_DSC_SUBMENU", "N_ID_SUBMENU", ucFiltro.DataValueType.IntegerType, False, False, False, False, False)
        ucFiltro1.AddFilter("Clave", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_AYUDA", ucFiltro.DataValueType.IntegerType, False, False, False, False, , , 3)
        ucFiltro1.AddFilter("Título", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_TITULO", ucFiltro.DataValueType.StringType, False, True, False, , , , 100)
        ucFiltro1.AddFilter("Contenido", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_CONTENIDO", ucFiltro.DataValueType.StringType, False, True, False, , , , 100)
        ucFiltro1.LoadDDL("CatalogoAyuda.aspx")

    End Sub

    Private Sub CargarCombos()
        Dim menu As New Entities.Menu
        Dim dv As DataView = menu.ObtenerTodos
        Dim consulta As String = "N_FLAG_VIG=1"
        dv.RowFilter = consulta
        ViewState("MenusAyuda") = dv.ToTable

        Dim submenu As New Entities.SubMenu
        ddlSubMenu.DataSource = submenu.ObtenerTodos().ToTable
        ViewState("SubMenusAyuda") = submenu.ObtenerTodos().ToTable

        Utilerias.Generales.CargarCombo(ddlMenu, ViewState("MenusAyuda"), "T_DSC_MENU", "N_ID_MENU")
        Utilerias.Generales.CargarCombo(ddlSubMenu, ViewState("SubMenusAyuda"), "T_DSC_SUBMENU", "N_ID_SUBMENU")
    End Sub

    Private Sub CargarCatalogo()
        Dim ayuda As New Entities.Ayuda
        Dim dv As DataView = ayuda.ObtenerTodos
        Dim consulta As String = "1=1"
        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next
        dv.RowFilter = consulta
        gvConsulta.DataSourceSession = dv.ToTable
        gvConsulta.DataSource = dv.ToTable
        gvConsulta.DataBind()
        If gvConsulta.Rows.Count = 0 Then
            Noexisten.Visible = True
            gvConsulta.Visible = False
        Else
            Noexisten.Visible = False
            gvConsulta.Visible = True
        End If
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        Page.Validate("Forma")

        If Not Page.IsValid Then
            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        btnAceptarM2B1A.CommandArgument = "btnAceptar"
        Dim ayuda As New Entities.Ayuda(CInt(ddlMenu.SelectedValue), CInt(ddlSubMenu.SelectedValue), CInt(ddlAyuda.SelectedValue))

        If Not ayuda.Existe Then
            Dim errores As New Entities.EtiquetaError(148)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ElseIf ddlAyuda.SelectedValue > -1 And ayuda.Existe And ddlAyuda.Enabled = True And ddlMenu.Enabled = True And ddlSubMenu.Enabled = True Then
            Dim errores As New Entities.EtiquetaError(148)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(149)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub


    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        lblTituloRegistro.Text = "Alta de Tema de Ayuda"

        Utilerias.Generales.CargarCombo(ddlMenu, ViewState("MenusAyuda"), "T_DSC_MENU", "N_ID_MENU")
        Utilerias.Generales.CargarCombo(ddlSubMenu, ViewState("SubMenusAyuda"), "T_DSC_SUBMENU", "N_ID_SUBMENU")

        ddlMenu.Enabled = True
        ddlAyuda.SelectedValue = -2
        ddlSubMenu.Enabled = False
        ddlAyuda.Enabled = False


        txtTitulo.Text = String.Empty
        txtContenido.Text = String.Empty
        txtOrden.Text = String.Empty

        pnlRegistro.Visible = True
        pnlConsulta.Visible = False


    End Sub

    Private Function HayRegistroSeleccionado() As Boolean

        Dim haySeleccion As Boolean = False

        For Each row As GridViewRow In gvConsulta.Rows

            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)

            If elemento.Checked Then

                haySeleccion = True
                Exit For

            End If

        Next

        If Not haySeleccion Then
            Dim errores As New Entities.EtiquetaError(152)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

        Return haySeleccion

    End Function

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click
        Utilerias.Generales.CargarCombo(ddlMenu, ViewState("MenusAyuda"), "T_DSC_MENU", "N_ID_MENU")
        Utilerias.Generales.CargarCombo(ddlSubMenu, ViewState("SubMenusAyuda"), "T_DSC_SUBMENU", "N_ID_SUBMENU")

        lblTituloRegistro.Text = "Modificación de Tema de Ayuda"

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim ayuda As New Entities.Ayuda(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_MENU").ToString()), CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_SUBMENU").ToString()), CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_AYUDA").ToString()))


                If Not ayuda.Vigente Then
                    Dim errores As New Entities.EtiquetaError(153)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                Else
                    txtTitulo.Text = ayuda.Titulo
                    txtContenido.Text = ayuda.Contenido
                    txtOrden.Text = ayuda.orden

                    ddlMenu.SelectedValue = ayuda.IdentificadorMenu

                    CargarCombosSubMenu(ayuda.IdentificadorMenu)
                    ddlSubMenu.SelectedValue = ayuda.IdentificadorSubmenu

                    CargarCombosAyuda(ayuda.IdentificadorMenu, ayuda.IdentificadorSubmenu)
                    ddlAyuda.SelectedValue = ayuda.IdentificadorAyuda

                    ddlMenu.Enabled = False
                    ddlSubMenu.Enabled = False
                    ddlAyuda.Enabled = False


                    pnlRegistro.Visible = True
                    pnlConsulta.Visible = False
                End If


                Exit For

            End If
        Next

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        Dim errores As New Entities.EtiquetaError(150)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub


    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        btnAceptarM2B1A_Click(sender, e)


    End Sub

    Protected Sub btnEliminar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEliminar.Click



        btnAceptarM2B1A.CommandArgument = "btnEliminar"

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If

        Dim errores As Entities.EtiquetaError

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim ayuda As New Entities.Ayuda(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_MENU").ToString()), CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_SUBMENU").ToString()), CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_AYUDA").ToString()))

                If Not ayuda.Vigente Then
                    errores = New Entities.EtiquetaError(153)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If


            End If
        Next



        errores = New Entities.EtiquetaError(151)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Eliminar", "ConfirmacionEliminar();", True)





    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click


        Select Case btnAceptarM2B1A.CommandArgument

            Case "btnCancelar"

                pnlControles.Enabled = True
                pnlBotones.Visible = True
                pnlRegresar.Visible = False

                pnlRegistro.Visible = False

                pnlConsulta.Visible = True

            Case "btnAceptar"

                Dim ayuda As New Entities.Ayuda(CInt(ddlMenu.SelectedValue), CInt(ddlSubMenu.SelectedValue), CInt(ddlAyuda.SelectedValue))
                ayuda.orden = txtOrden.Text
                ayuda.Titulo = txtTitulo.Text
                ayuda.Contenido = txtContenido.Text



                If ddlAyuda.SelectedValue > -1 Then
                    ayuda.IdenticadorPadre = ddlAyuda.SelectedValue
                Else
                    ayuda.IdenticadorPadre = 0
                End If



                If Not ayuda.Existe Then
                    ayuda.IdentificadorAyuda = ayuda.ObtenerSiguienteIdentificadorAyuda()
                    ayuda.Agregar()
                ElseIf ddlAyuda.SelectedValue > -1 And ayuda.Existe And ddlAyuda.Enabled = True And ddlMenu.Enabled = True And ddlSubMenu.Enabled = True Then
                    ayuda.IdentificadorAyuda = ayuda.ObtenerSiguienteIdentificadorAyuda()
                    ayuda.Agregar()
                Else
                    ayuda.Actualizar()
                End If


                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"


                For Each row As GridViewRow In gvConsulta.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then
                        Dim ayuda As New Entities.Ayuda(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_MENU").ToString()), CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_SUBMENU").ToString()), CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_AYUDA").ToString()))
                        ayuda.Baja()
                        Exit For
                    End If
                Next

                CargarCatalogo()

        End Select

    End Sub

    Public Function ObtenerImagen(ByVal imagen As String) As String

        Return "~/Imagenes/Errores/" + imagen

    End Function


    Public Function ObtenerImagenEstatus(ByVal estatus As Boolean) As String

        If estatus Then
            Return "~/Imagenes/vigente.gif"
        Else
            Return "~/Imagenes/no_vigente.gif"
        End If

    End Function

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated

        e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))

    End Sub


    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Tema de Ayuda"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim ayuda As New Entities.Ayuda(CInt(gvConsulta.DataKeys(index)("N_ID_MENU").ToString()), CInt(gvConsulta.DataKeys(index)("N_ID_SUBMENU").ToString()), CInt(gvConsulta.DataKeys(index)("N_ID_AYUDA").ToString()))

        txtTitulo.Text = ayuda.Titulo
        txtContenido.Text = ayuda.Contenido
        txtOrden.Text = ayuda.orden


        ddlMenu.SelectedValue = ayuda.IdentificadorMenu

        CargarCombosSubMenu(ayuda.IdentificadorMenu)
        ddlSubMenu.SelectedValue = ayuda.IdentificadorSubmenu

        CargarCombosAyuda(ayuda.IdentificadorMenu, ayuda.IdentificadorSubmenu)
        ddlAyuda.SelectedValue = ayuda.IdentificadorAyuda



        pnlRegistro.Visible = True
        pnlControles.Enabled = False
        pnlBotones.Visible = False
        pnlRegresar.Visible = True

        pnlConsulta.Visible = False

    End Sub


    Protected Sub cvContenido_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvContenido.ServerValidate

        If txtContenido.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(8)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If

    End Sub

    Protected Sub cvTitulo_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvTitulo.ServerValidate

        If txtTitulo.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(7)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If

    End Sub

    Private Sub ddlMenu_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlMenu.SelectedIndexChanged

        CargarCombosSubMenu(ddlMenu.SelectedValue)

    End Sub

    Private Sub CargarCombosSubMenu(ByVal menu As Integer)

        Dim submenu As New Entities.SubMenu
        ddlSubMenu.DataSource = submenu.ObtenerTodosFiltro(menu).Tables(0)
        ViewState("SubMenusAyuda") = submenu.ObtenerTodosFiltro(menu).Tables(0)
        Utilerias.Generales.CargarCombo(ddlSubMenu, ViewState("SubMenusAyuda"), "T_DSC_SUBMENU", "N_ID_SUBMENU")
        ddlSubMenu.Enabled = True

    End Sub

    Private Sub ddlSubMenu_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlSubMenu.SelectedIndexChanged

        CargarCombosAyuda(ddlMenu.SelectedValue, ddlSubMenu.SelectedValue)
       




    End Sub

    Private Sub CargarCombosAyuda(ByVal menu As Integer, ByVal submenu As Integer)


        'El llenado de este combo no se realiza con la clase de generales ya que es un caso especifico para el combo de Ayuda que se agrega un elemento llamado nuevo

        Dim Ayuda As New Entities.Ayuda
        ViewState("Ayuda") = Ayuda.ObtenerTodosFiltro(menu, submenu).Tables(0)
        ddlAyuda.DataSource = ViewState("Ayuda")
        ddlAyuda.DataTextField = "T_DSC_TITULO"
        ddlAyuda.DataValueField = "N_ID_AYUDA"
        ddlAyuda.DataBind()
        ddlAyuda.Items.Insert(0, New ListItem("Agregar Nuevo ", "-1"))
        ddlAyuda.Items.Insert(0, New ListItem("- Seleccione uno -", "-2"))
        ddlAyuda.Enabled = True
    End Sub

    Private Sub gvConsulta_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting
        gvConsulta.Ordenar(e)
    End Sub

    Private Sub cvddlMenu_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvddlMenu.ServerValidate

        If ddlMenu.SelectedValue = "-1" Then
            Dim errores As New Entities.EtiquetaError(170)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If

    End Sub

    Private Sub cvddlSubMenu_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvddlSubMenu.ServerValidate

        If ddlSubMenu.SelectedValue = "-1" Then
            Dim errores As New Entities.EtiquetaError(171)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If

    End Sub

    Private Sub cvddlAyuda_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvddlAyuda.ServerValidate

        If ddlAyuda.SelectedValue = "-2" Then
            Dim errores As New Entities.EtiquetaError(172)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If

    End Sub

End Class