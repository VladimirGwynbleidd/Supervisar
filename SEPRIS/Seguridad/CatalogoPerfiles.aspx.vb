'- Fecha de creación:26/07/2013
'- Fecha de modificación:  29/07/2013
'- Nombre del Responsable: Julio Cesar Vieyra Tena
'- Empresa: Softtek
'- Catalogo de Perfiles


Public Class CatalogoPerfiles
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

    Private Sub CargarFiltros()
        
        ucFiltro1.resetSession()

        Dim objUsuario As Entities.Usuario = Nothing
        If Not IsNothing(TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)) Then
            objUsuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)
        End If

        Dim dtDatosFiltro As DataSet

        If Not IsNothing(objUsuario) Then
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.CatalogoPerfiles, objUsuario.IdentificadorUsuario, objUsuario.IdArea)
        Else
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.CatalogoPerfiles, "", 0)
        End If

        ucFiltro1.AddFilter("Vigencia  ", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSource, "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, -1)
        'ucFiltro1.AddFilter("Clave", ucFiltro.AcceptedControls.TextBox, Nothing, "", "N_ID_PERFIL", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, Nothing, 4)
        'ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_PERFIL", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 255)

        ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(0), "T_DSC_PERFIL", "N_ID_PERFIL", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)
        ucFiltro1.LoadDDL("CatalogoPerfiles.aspx")

    End Sub

    Private Sub CargarCatalogo()
        Dim perfil As New Entities.Perfil
        Dim dv As DataView = perfil.ObtenerTodos()
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

        Dim perfil As New Entities.Perfil(txtClave.Text, txtDescripción.Text)

        If Not perfil.ExistePerfil() Then
            Dim errores As New Entities.EtiquetaError(111)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(112)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub


    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        Dim perfil As New Entities.Perfil()
        lblTituloRegistro.Text = "Alta de Perfil"
        txtClave.Text = perfil.ObtenerSiguienteIdentificadorPerfil().ToString
        txtDescripción.Text = String.Empty
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
            Dim errores As New Entities.EtiquetaError(115)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

        Return haySeleccion

    End Function

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Perfil"

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If
        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim perfiles As New Entities.Perfil(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_PERFIL").ToString()))

               

            End If
        Next
        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then
                Dim perfil As New Entities.Perfil(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_PERFIL").ToString()), "")

                If Not perfil.EsVigente Then
                    Dim errores As New Entities.EtiquetaError(116)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                Else
                    txtDescripción.Text = perfil.Descripcion
                    txtClave.Text = gvConsulta.DataKeys(row.RowIndex)("N_ID_PERFIL").ToString()
                    pnlRegistro.Visible = True
                    pnlConsulta.Visible = False
                End If
                Exit For
            End If
        Next

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        Dim errores As New Entities.EtiquetaError(113)
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

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim perfiles As New Entities.Perfil(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_PERFIL").ToString()), "")

                If Not perfiles.EsVigente Then
                    Dim errores As New Entities.EtiquetaError(116)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                Else
                    Dim errores As New Entities.EtiquetaError(114)
                    Mensaje = errores.Descripcion
                    imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Eliminar", "ConfirmacionEliminar();", True)
                End If

            End If
        Next
        

      

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

                Dim perfil As New Entities.Perfil(CInt(txtClave.Text), txtDescripción.Text)
                perfil.Descripcion = txtDescripción.Text

                If Not perfil.existe Then
                    perfil.Agregar()
                Else
                    perfil.Actualizar()
                End If

                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"

                For Each row As GridViewRow In gvConsulta.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then
                        Dim perfil As New Entities.Perfil(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_PERFIL").ToString()), "")
                        perfil.Baja()
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

        lblTituloRegistro.Text = "Consulta de Perfiles"
        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))
        Dim perfil As New Entities.Perfil(CInt(gvConsulta.DataKeys(index)("N_ID_PERFIL").ToString()), "")
        txtClave.Text = gvConsulta.DataKeys(index)("N_ID_PERFIL").ToString()
        txtDescripción.Text = perfil.Descripcion
        pnlRegistro.Visible = True
        pnlControles.Enabled = False
        pnlBotones.Visible = False
        pnlRegresar.Visible = True
        pnlConsulta.Visible = False

    End Sub


    Protected Sub cvDescripcion_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvDescripcion.ServerValidate
        If txtDescripción.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(6)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If
    End Sub


    Protected Sub btnExportaExcel_Click(sender As Object, e As EventArgs) Handles btnExportaExcel.Click
        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns("N_FLAG_VIG").ColumnName = "Estatus"

        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Perfiles", referencias)
    End Sub

    Private Sub gvConsulta_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting
        gvConsulta.Ordenar(e)
    End Sub
End Class