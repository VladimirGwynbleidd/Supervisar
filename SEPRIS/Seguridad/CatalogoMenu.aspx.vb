
' Fecha de creación: 25/07/2013
' Fecha de modificación: 
' Nombre del Responsable: ARGC1
' Empresa: Softtek
' página de catálogo de menús

Imports Entities


Public Class CatalogoMenu
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
            CargarFiltros()
            CargarCatalogo()
            CargarImagenesEstatus()

        End If
    End Sub



    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub

    Public Function ObtenerImagenEstatus(ByVal estatus As Boolean) As String

        If estatus Then
            Return "~/Imagenes/vigente.gif"
        Else
            Return "~/Imagenes/no_vigente.gif"
        End If

    End Function


    Private Sub CargarFiltros()
        Dim objUsuario As Entities.Usuario = Nothing
        If Not IsNothing(TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)) Then
            objUsuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)
        End If

        Dim dtDatosFiltro As DataSet

        If Not IsNothing(objUsuario) Then
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.CatalgoMenus, objUsuario.IdentificadorUsuario, objUsuario.IdArea)
        Else
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.CatalgoMenus, "", 0)
        End If

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Vigencia", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSource(), "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, False, False, True, True, -1)
        'ucFiltro1.AddFilter("Clave", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_MENU", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 3)
        'ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_MENU", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 255)
        ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(0), "T_DSC_MENU", "N_ID_MENU", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        ucFiltro1.AddFilter("Url Imagen", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_URL_IMAGEN_SUBM", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 255)
        ucFiltro1.LoadDDL("CatalogoMenu.aspx")

        btnFiltrar_Click(Nothing, Nothing)
    End Sub

    Private Sub CargarCatalogo()
        Dim menu As New Menu
        Dim dv As DataView = menu.ObtenerTodos
        Dim consulta As String = "1=1"

        For Each filtro In ucFiltro1.getFilterSelection

            consulta += " AND " + filtro

        Next

        dv.RowFilter = consulta

        gvConsulta.DataSourceSession = dv.ToTable
        gvConsulta.DataSource = dv.ToTable
        gvConsulta.DataBind()

        If gvConsulta.Rows.Count > 0 Then
            btnExportaExcel.Visible = True
            gvConsulta.Visible = True
            pnlNoExiste.Visible = False
        Else
            btnExportaExcel.Visible = False
            gvConsulta.Visible = False
            pnlNoExiste.Visible = True
        End If
    End Sub


    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click

        lblTituloRegistro.Text = "Alta de Menú"

        cvDescripcion.Enabled = True
        cvOrden.Enabled = True
        cvUrlImagen.Enabled = True


        Dim menu As New Menu

        txtClave.Text = menu.ObtenerSiguienteIdentificador()
        txtDescripcion.Text = String.Empty
        txtUrlImagen.Text = String.Empty
        txtOrden.Text = String.Empty


        pnlRegistro.Visible = True
        pnlConsulta.Visible = False
        pnlRegresar.Visible = False

    End Sub

    Protected Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        lblTituloRegistro.Text = "Modificación de Menú"
        cvDescripcion.Enabled = True
        cvOrden.Enabled = True
        cvUrlImagen.Enabled = True

        If gvConsulta.SelectedIndex = "-1" Then
            Dim errores As New EtiquetaError(103)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If


        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim menu As New Menu(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_MENU").ToString()))

                If Not menu.EsVigente Then
                    Dim errores As New EtiquetaError(104)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If

                txtClave.Text = menu.IdentificadorMenu
                txtDescripcion.Text = menu.Descripcion
                txtOrden.Text = menu.Orden
                txtUrlImagen.Text = menu.UrlImagenSubmenu


                pnlRegistro.Visible = True
                pnlConsulta.Visible = False
                pnlRegresar.Visible = False

                Exit For

            End If
        Next

    End Sub

    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        btnAceptarM2B1A.CommandArgument = "btnEliminar"

        Dim errores As EtiquetaError

        If gvConsulta.SelectedIndex = "-1" Then
            errores = New EtiquetaError(103)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If



        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim menu As New Menu(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_MENU").ToString()))

                If Not menu.EsVigente Then
                    errores = New EtiquetaError(104)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If

                Exit For

            End If
        Next

        errores = New EtiquetaError(102)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Eliminar", "ConfirmacionEliminar();", True)

    End Sub

    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click

        pnlControles.Enabled = True

        Page.Validate("Forma")

        If Not Page.IsValid Then

            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            imgUnBotonNoAccion.ImageUrl = String.Empty
            Exit Sub

        End If

        btnAceptarM2B1A.CommandArgument = "btnAceptar"

        Dim menu As New Menu(txtClave.Text)
        Dim errores As EtiquetaError
        If Not menu.Existe Then
            errores = New EtiquetaError(99)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            errores = New EtiquetaError(100)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        Dim errores As New EtiquetaError(101)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub

    Protected Sub btnAceptarM2B1A_Click(sender As Object, e As EventArgs) Handles btnAceptarM2B1A.Click
        Select Case btnAceptarM2B1A.CommandArgument

            Case "btnCancelar"

                pnlControles.Enabled = True

                pnlBotones.Visible = True

                pnlRegistro.Visible = False

                pnlConsulta.Visible = True



            Case "btnAceptar"
              
                Dim menu As New Menu(txtClave.Text) _
                With {.Descripcion = txtDescripcion.Text, .UrlImagenSubmenu = txtUrlImagen.Text, .Orden = txtOrden.Text}

                    If Not menu.Existe Then
                        menu.Agregar()
                    Else
                        menu.Actualizar()
                    End If


                CargarCatalogo()

                btnFiltrar_Click(sender, e)

                    btnAceptarM2B1A.CommandArgument = "btnCancelar"
                    btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"


                    For Each row As GridViewRow In gvConsulta.Rows
                        Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                        If elemento.Checked Then

                            Dim menu As New Menu(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_MENU").ToString()))

                            If menu.Baja() Then
                                CargarCatalogo()
                                btnFiltrar_Click(sender, e)
                            End If
                            Exit For

                        End If
                    Next
        End Select

        imgUnBotonNoAccion.ImageUrl = String.Empty

    End Sub

    Protected Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        btnAceptarM2B1A_Click(sender, e)
    End Sub

    Protected Sub gvConsulta_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        lblTituloRegistro.Text = "Consulta de Menú"

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim menu As New Menu(CInt(gvConsulta.DataKeys(index)("N_ID_MENU").ToString()))


        txtClave.Text = menu.IdentificadorMenu
        txtDescripcion.Text = menu.Descripcion
        txtUrlImagen.Text = menu.UrlImagenSubmenu
        txtOrden.Text = menu.Orden

      
        pnlRegistro.Visible = True
        pnlControles.Enabled = False
        pnlBotones.Visible = False
        pnlRegresar.Visible = True

        pnlConsulta.Visible = False

    End Sub

    Protected Sub cvDescripcion_ServerValidate(source As Object, args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvDescripcion.ServerValidate
        If txtDescripcion.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(14)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False

            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If

        End If
    End Sub

    Protected Sub cvUrlImagen_ServerValidate(source As Object, args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvUrlImagen.ServerValidate

        If txtUrlImagen.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(15)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False

            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If
        End If
    End Sub

    Protected Sub cvOrden_ServerValidate(source As Object, args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvOrden.ServerValidate
        If txtOrden.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(21)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False

            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If
        Else
            Dim Orden As Int16
            Try
                Orden = CType(txtOrden.Text, Int16)
            Catch ex As Exception
                Dim errores As New EtiquetaError(44)
                source.ErrorMessage = errores.Descripcion

                args.IsValid = False
                If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                End If

            End Try
        End If
    End Sub

    Protected Sub btnExportaExcel_Click(sender As Object, e As EventArgs) Handles btnExportaExcel.Click
        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)

        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns("N_FLAG_VIG").ColumnName = "Estatus"

        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Menús", referencias)


    End Sub

    Protected Sub gvConsulta_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting
        gvConsulta.Ordenar(e)
    End Sub
End Class