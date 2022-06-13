' Fecha de creación: 06/08/2013
' Fecha de modificación: 
' Nombre del Responsable: Rafael Rodriguez Sanchez RARS1
' Empresa: Softtek
' Bandeja que muestra la combinacion de varios componentes
Public Class Bandeja
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

            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                PersonalizaColumnas.IdentificadorGridView = 2
                PersonalizaColumnas.GridViewPersonalizar = gvConsulta
                PersonalizaColumnas.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
                PersonalizaColumnas.Personalizar()
            End If

        End If

    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        If gvConsulta.Columns(5).Visible Then
            pnlSignificadoImagen.Visible = True
        Else
            pnlSignificadoImagen.Visible = False
        End If
    End Sub

#Region "Carga inicial"
    ''' <summary>
    ''' Realiza la carga inicial de los filtros dinamicos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarFiltros()

        ucFiltro1.resetSession()

        ucFiltro1.AddFilter("Vigencia  ", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSource, "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, False, -1)
        ucFiltro1.AddFilter("Parámetro", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_PARAMETRO", ucFiltro.DataValueType.StringType, False, True, False, , , , 255)
        ucFiltro1.AddFilter("Descripción", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_OBS_PARAMETRO", ucFiltro.DataValueType.StringType, False, True, False, , , , 255)

        ucFiltro1.AddFilter("Valor", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_VALOR", ucFiltro.DataValueType.StringType, False, True, False, , , , 4000)

        ucFiltro1.LoadDDL("Bandeja.aspx")

    End Sub

    ''' <summary>
    ''' Realiza la carga inicial de los datos del gridview
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarCatalogo()

        Dim consulta As String = "1=1"

        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        Dim parametro As New Entities.Parametros()
        Dim dv As DataView = parametro.ObtenerTodos()

        dv.RowFilter = consulta
        gvConsulta.DataSource = dv.ToTable
        gvConsulta.DataBind()

        If dv.ToTable.Rows.Count > 0 Then
            btnExportaExcel.Visible = True
            pnlGrid.Visible = True
            pnlNoExiste.Visible = False
        Else
            btnExportaExcel.Visible = False
            pnlGrid.Visible = False
            pnlNoExiste.Visible = True
        End If

    End Sub
#End Region

#Region "Eventos de los controles"
    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar

        CargarCatalogo()

    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        pnlConsulta.Visible = False
        pnlDetalle.Visible = True
        lblDetalle.Text = "Inserta Registro"

    End Sub

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click
        Dim lstSeleccionados As List(Of Integer) = gvConsulta.MultiSelectedIndex

        Dim seleccionados As Integer = lstSeleccionados.Count

        If seleccionados = 0 Then
            Dim er As Entities.EtiquetaError = New Entities.EtiquetaError(52)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & er.Imagen.Ruta
            Mensaje += er.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        ElseIf seleccionados > 1 Then
            Dim er As Entities.EtiquetaError = New Entities.EtiquetaError(53)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & er.Imagen.Ruta
            Mensaje += er.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim parametros As New Entities.Parametros(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_PARAMETRO").ToString()))

                If Not parametros.Vigente Then
                    Dim er As Entities.EtiquetaError = New Entities.EtiquetaError(54)
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & er.Imagen.Ruta
                    Mensaje += er.Descripcion
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                Else
                    pnlConsulta.Visible = False
                    pnlDetalle.Visible = True
                    lblDetalle.Text = "Modifica un Registro"
                End If

                Exit For

            End If
        Next

    End Sub

    Protected Sub btnEliminar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEliminar.Click

        btnAceptarM2B1A.CommandArgument = "btnEliminar"

        Dim lstSeleccionados As List(Of Integer) = gvConsulta.MultiSelectedIndex

        Dim seleccionados As Integer = lstSeleccionados.Count

        If seleccionados <= 0 Then
            Dim er As Entities.EtiquetaError = New Entities.EtiquetaError(51)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & er.Imagen.Ruta
            Mensaje += er.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim parametros As New Entities.Parametros(CInt(gvConsulta.DataKeys(row.RowIndex)("N_ID_PARAMETRO").ToString()))

                If Not parametros.Vigente Then
                    Dim er As Entities.EtiquetaError = New Entities.EtiquetaError(50)
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & er.Imagen.Ruta
                    Mensaje += er.Descripcion
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If

            End If
        Next

        Mensaje = "¿Está seguro que desea eliminar el/los registro/s?"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Eliminar", "ConfirmacionEliminar();", True)

    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click

        Select Case btnAceptarM2B1A.CommandArgument

            Case "btnEliminar"

                Mensaje = "En este escenario se da de baja el registro"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                CargarCatalogo()
        End Select

    End Sub

    Protected Sub btnPersonalizaColumnas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPersonalizaColumnas.Click
        PersonalizaColumnas.IdentificadorGridView = 2
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.Mostrar()
    End Sub

    Protected Sub btnExportaExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportaExcel.Click
        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns("N_FLAG_VIG").ColumnName = "Estatus"

        utl.ExportaGrid(dt, gvConsulta, "Ejemplo de Bandeja", referencias)
    End Sub

    Protected Sub chkSeleccionaTodos_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkSeleccionaTodos.CheckedChanged
        Dim chkselects As CheckBox = TryCast(sender, CheckBox)
        For Each gvrow As GridViewRow In gvConsulta.Rows
            Dim chkSelected As CheckBox = TryCast(gvrow.FindControl("chkElemento"), CheckBox)
            chkSelected.Checked = chkselects.Checked
        Next
        gvConsulta.CargaSeleccion()
    End Sub

    Protected Sub cbSelecteds2_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim allSelect As Boolean = False
        For Each gvrow As GridViewRow In gvConsulta.Rows
            Dim chkSelected2 As CheckBox = TryCast(gvrow.FindControl("chkElemento"), CheckBox)
            allSelect = chkSelected2.Checked
            If allSelect = False Then
                Exit For
            End If
        Next
        chkSeleccionaTodos.Checked = allSelect
        gvConsulta.CargaSeleccion()
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click
        pnlConsulta.Visible = False
        pnlDetalle.Visible = True
        lblDetalle.Text = "Consulta Registro"
    End Sub

    Private Sub btnRegresa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRegresa.Click
        pnlConsulta.Visible = True
        pnlDetalle.Visible = False
    End Sub

    Protected Sub PersonalizaColumnas_event(ByVal sender As Object, ByVal e As EventArgs) Handles PersonalizaColumnas.FinPersonalizacion
        PersonalizaColumnas.IdentificadorGridView = 2
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.GuardarPersonalizacion()
        gvConsulta.DataSource = gvConsulta.DataSourceSession
        gvConsulta.DataBind()
    End Sub

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            'e.Row.Attributes.Add("onMouseOver", "tooltip.show('Para consultar el registro hacer Doble Click sobre el registro.');hideTimerId = setTimeout(""tooltip.hide('" + e.Row.ClientID + "','" & e.Row.DataItemIndex + 1 & "', '#C7F4F8','#44698D',0,100)"",3000);")
            'e.Row.Attributes.Add("onMouseOut", "tooltip.hide('" + e.Row.ClientID + "','" & e.Row.DataItemIndex + 1 & "', 'transparent','#44698D',0,100);clearTimeout(hideTimerId);")
            
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
        End If
    End Sub


    Protected Sub gvConsulta_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting
        gvConsulta.Ordenar(e)
    End Sub

    Protected Sub gvConsulta_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvConsulta.PageIndexChanging
        gvConsulta.PageIndex = e.NewPageIndex
        gvConsulta.DataSource = gvConsulta.DataSourceSession
        gvConsulta.DataBind()

        'Se agrega segmento para controlar el checkbox Selecctionar Todo
        'Al cambiar de pagina se pierde la seleccion
        chkSeleccionaTodos.Checked = False
    End Sub

#End Region

#Region "Metodos"
    ''' <summary>
    ''' Obtiene la ruta de la imagen de estatus
    ''' </summary>
    ''' <param name="estatus">Estatus del registro</param>
    ''' <returns>Ruta de la imagen</returns>
    ''' <remarks></remarks>
    Public Function ObtenerImagenEstatus(ByVal estatus As Boolean) As String

        If estatus Then
            Return "~/Imagenes/vigente.gif"
        Else
            Return "~/Imagenes/no_vigente.gif"
        End If

    End Function

    ''' <summary>
    ''' Realiza la carga de la imagen de los estatus
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub

#End Region

End Class