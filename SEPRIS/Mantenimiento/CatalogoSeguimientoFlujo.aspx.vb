Imports Entities
Public Class CatalogoSeguimientoFlujo
    Inherits System.Web.UI.Page

    Public Property Mensaje As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargaFlujos()
        End If
    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        gvUsados.ArmaMultiScript()
    End Sub

#Region "Carga Datos"
    ''' <summary>
    ''' Carga los Flujos en el DropDownList ddlFlujo
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaFlujos()
        Dim entFlujo As New Flujo
        Dim dvFlujo As DataView = entFlujo.ObtenerTodos
        dvFlujo.RowFilter = "B_FLAG_VIG=1"

        Utilerias.Generales.CargarCombo(ddlFlujo, dvFlujo.ToTable, "T_DSC_FLUJO", "N_ID_FLUJO")

    End Sub

    ''' <summary>
    ''' Carga los Pasos en los GridViews
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaFunciones()
        If ddlFlujo.SelectedValue <> "-1" Then
            pnlPasos.Style.Add("display", "block")
            btnAceptar.Visible = True
            btnCancelar.Visible = True

            Dim entFlujo As New Flujo

            gvUsados.DataSource = entFlujo.ObtenerPasos(Convert.ToInt32(ddlFlujo.SelectedValue))
            gvUsados.DataBind()

            gvDisponibles.DataSource = entFlujo.ObtenerPasosDisponibles(Convert.ToInt32(ddlFlujo.SelectedValue))
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            pnlPasos.Style.Add("display", "none")
            btnAceptar.Visible = False
            btnCancelar.Visible = False
        End If

    End Sub
#End Region

#Region "Eventos de Controles"
    Protected Sub ddlPerfil_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFlujo.SelectedIndexChanged
        CargaFunciones()
    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click
        Select Case btnAceptarM2B1A.CommandArgument
            Case "btnAceptar"
                If ddlFlujo.SelectedValue <> "-1" Then
                    Guardar()
                Else
                    Dim errores As New Entities.EtiquetaError(1017)
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    lblMensaje.Text = errores.Descripcion
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
                End If
            Case "btnCancelar"
                ddlFlujo.SelectedValue = "-1"
                CargaFunciones()
        End Select


    End Sub

    Protected Sub btnMostrarMensaje_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Page.Validate("Forma")

        If Page.IsValid() Then
            btnAceptarM2B1A.CommandArgument = "btnAceptar"

            Dim errores As New EtiquetaError(1018)
            imgDosBotonesUnaAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
            Mensaje = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Guardar", "LevantaVentanaConfirma();", True)
        Else
            lblMensaje.Text = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        Dim errores As New EtiquetaError(1019)
        imgDatos.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
        Mensaje = errores.Descripcion
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cancelar", "LevantaVentanaConfirma();", True)
    End Sub

    Protected Sub btnRemueve_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemueve.Click
        Dim lstGVRow As List(Of GridViewRow) = gvUsados.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstUsados As List(Of PasoFlujo) = TryCast(gvUsados.DataSourceSession, List(Of PasoFlujo))
            Dim lstDisponibles As List(Of PasoFlujo) = TryCast(gvDisponibles.DataSourceSession, List(Of PasoFlujo))
            For Each gvRow In lstGVRow
                For index = 0 To lstUsados.Count - 1
                    If Convert.ToInt32(gvRow.Cells(1).Text) = lstUsados(index).Identificador Then
                        lstDisponibles.Add(lstUsados(index))
                        lstUsados.Remove(lstUsados(index))
                        Exit For
                    End If
                Next
            Next

            gvUsados.DataSource = lstUsados
            gvUsados.DataBind()

            gvDisponibles.DataSource = lstDisponibles
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            Dim errores As New Entities.EtiquetaError(1020)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnAgrega_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgrega.Click
        Dim lstGVRow As List(Of GridViewRow) = gvDisponibles.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstUsados As List(Of PasoFlujo) = TryCast(gvUsados.DataSourceSession, List(Of PasoFlujo))
            Dim lstDisponibles As List(Of PasoFlujo) = TryCast(gvDisponibles.DataSourceSession, List(Of PasoFlujo))
            For Each gvRow In lstGVRow
                For index = 0 To lstDisponibles.Count - 1
                    If Convert.ToInt32(gvRow.Cells(1).Text) = lstDisponibles(index).Identificador Then
                        lstUsados.Add(lstDisponibles(index))
                        lstDisponibles.Remove(lstDisponibles(index))
                        Exit For
                    End If
                Next
            Next

            gvUsados.DataSource = lstUsados
            gvUsados.DataBind()

            gvDisponibles.DataSource = lstDisponibles
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            Dim errores As New Entities.EtiquetaError(1020)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Private Sub btnExportaExcelAsigandos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportaExcelUsados.Click
        Dim lstUsados As List(Of PasoFlujo) = TryCast(gvUsados.DataSourceSession, List(Of PasoFlujo))
        Dim dt As New DataTable()
        Dim dc As New DataColumn("Identificador", GetType(Integer))
        Dim dc2 As New DataColumn("Descripcion", GetType(String))
        Dim dc3 As New DataColumn("Orden", GetType(String))
        dt.Columns.Add(dc)
        dt.Columns.Add(dc2)
        dt.Columns.Add(dc3)
        For Each entPasoFlujo As PasoFlujo In lstUsados
            Dim dr As DataRow = dt.NewRow()
            dr.Item("Identificador") = entPasoFlujo.Identificador
            dr.Item("Descripcion") = entPasoFlujo.Descripcion
            dr.Item("Orden") = entPasoFlujo.Orden
            dt.Rows.Add(dr)
        Next

        Dim lstComentarios As New List(Of String)
        lstComentarios.Add("Flujo: " + ddlFlujo.SelectedItem.Text)

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvUsados, "Pasos agregados al flujo", lstComentarios)
    End Sub

    Private Sub btnExportaExcelDisponibles_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportaExcelDisponibles.Click
        Dim lstDisponibles As List(Of PasoFlujo) = TryCast(gvDisponibles.DataSourceSession, List(Of PasoFlujo))
        Dim dt As New DataTable()
        Dim dc As New DataColumn("Identificador", GetType(Integer))
        Dim dc2 As New DataColumn("Descripcion", GetType(String))
        Dim dc3 As New DataColumn("Orden", GetType(String))
        dt.Columns.Add(dc)
        dt.Columns.Add(dc2)
        dt.Columns.Add(dc3)
        For Each entPasoFlujo As PasoFlujo In lstDisponibles
            Dim dr As DataRow = dt.NewRow()
            dr.Item("Identificador") = entPasoFlujo.Identificador
            dr.Item("Descripcion") = entPasoFlujo.Descripcion
            dr.Item("Orden") = entPasoFlujo.Orden
            dt.Rows.Add(dr)
        Next

        Dim lstComentarios As New List(Of String)
        lstComentarios.Add("Flujo: " + ddlFlujo.SelectedItem.Text)

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvDisponibles, "Pasos disponibles para agregar", lstComentarios)
    End Sub

#End Region

#Region "Metodos"
    ''' <summary>
    ''' Muestra y esconde las imagenes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraImagenes()
        If gvUsados.Rows.Count > 0 Then
            imgUsados.Visible = False
        Else
            imgUsados.Visible = True
        End If

        If gvDisponibles.Rows.Count > 0 Then
            imgDisponibles.Visible = False
        Else
            imgDisponibles.Visible = True
        End If

        If gvDisponibles.Rows.Count = 0 And gvUsados.Rows.Count = 0 Then
            imgDatos.Visible = True
            btnAceptar.Visible = False
            pnlPasos.Visible = False
        Else
            imgDatos.Visible = False
            pnlPasos.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Devuelve una lista de los pasos a los que se le ha cambiado el orden
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Private Function ObtenerCambiosOrden() As List(Of PasoFlujo)
        Dim entFlujo As New Flujo()
        Dim lstUsados As List(Of PasoFlujo) = entFlujo.ObtenerPasos(Convert.ToInt32(ddlFlujo.SelectedValue))

        Dim lstModificados As New List(Of PasoFlujo)

        For Each gvrow As GridViewRow In gvUsados.Rows
            Dim existe As Boolean = False
            Dim objPaso As PasoFlujo = Nothing
            For index = 0 To lstUsados.Count - 1
                If lstUsados(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    objPaso = New PasoFlujo(Convert.ToInt32(gvrow.Cells(1).Text))
                    objPaso.Orden = lstUsados(index).Orden
                    existe = True
                End If
            Next
            If existe Then
                Dim txtOrden As TextBox = gvrow.FindControl("txtOrden")
                If Not objPaso.Orden = txtOrden.Text Then
                    objPaso.Orden = txtOrden.Text
                    lstModificados.Add(objPaso)
                End If
            End If
        Next

        Return lstModificados
    End Function

    ''' <summary>
    ''' Devuelve los registros agregados a gvAsignados
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenAgregados() As List(Of PasoFlujo)
        Dim entFlujo As New Flujo()
        Dim lstUsados As List(Of PasoFlujo) = entFlujo.ObtenerPasos(Convert.ToInt32(ddlFlujo.SelectedValue))

        Dim lstAgregados As New List(Of PasoFlujo)

        For Each gvrow As GridViewRow In gvUsados.Rows
            Dim existe As Boolean = False
            For index = 0 To lstUsados.Count - 1
                If lstUsados(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                Dim objPaso As New PasoFlujo(Convert.ToInt32(gvrow.Cells(1).Text))
                Dim txtOrden As TextBox = gvrow.FindControl("txtOrden")
                objPaso.Orden = txtOrden.Text
                lstAgregados.Add(objPaso)
            End If
        Next

        Return lstAgregados
    End Function

    ''' <summary>
    ''' Devuelve los registros agregados a gvDisponibles
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenEliminados() As List(Of PasoFlujo)
        Dim entFlujo As New Flujo()
        Dim lstDisponibles As List(Of PasoFlujo) = entFlujo.ObtenerPasosDisponibles(Convert.ToInt32(ddlFlujo.SelectedValue))

        Dim lstEliminados As New List(Of PasoFlujo)

        For Each gvrow As GridViewRow In gvDisponibles.Rows
            Dim existe As Boolean = False
            For index = 0 To lstDisponibles.Count - 1
                If lstDisponibles(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstEliminados.Add(New PasoFlujo(Convert.ToInt32(gvrow.Cells(1).Text)))
            End If
        Next

        Return lstEliminados
    End Function

    ''' <summary>
    ''' Actualiza las modificaciones realizadas
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Guardar()
        Try
            Dim lstAgregar = ObtenAgregados()
            Dim lstQuitar = ObtenEliminados()
            Dim lstModificar = ObtenerCambiosOrden()

            Dim entFlujo As New Flujo()

            entFlujo.ActualizarPasosFlujo(lstAgregar, lstQuitar, lstModificar, Convert.ToInt32(ddlFlujo.SelectedValue))

            CargaFunciones()
        Catch ex As Exception
            Dim errores As New Entities.EtiquetaError(1021)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End Try
    End Sub
#End Region

    Protected Sub cvOrden_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvOrden.ServerValidate
        Dim ordenOpciones As Boolean = False
        Dim intValue As Integer
        Dim errorIntOrden = False

        For Each row As GridViewRow In gvUsados.Rows
            Dim tb As TextBox = CType(row.FindControl("txtOrden"), TextBox)
            If Not tb.Text.Trim().Equals(String.Empty) Then
                ordenOpciones = True
                If Not Integer.TryParse(tb.Text, intValue) Then
                    errorIntOrden = True
                    Exit For
                End If
            Else
                ordenOpciones = False
                Exit For
            End If
        Next

        If Not ordenOpciones Then
            Dim errores As New Entities.EtiquetaError(1022)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        ElseIf errorIntOrden Then
            Dim errores As New Entities.EtiquetaError(1023)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        Else
            For index1 = 0 To (gvUsados.Rows.Count - 1)
                Dim tb As TextBox = CType(gvUsados.Rows(index1).FindControl("txtOrden"), TextBox)
                For index2 = (index1 + 1) To (gvUsados.Rows.Count - 1)
                    If tb.Text.Trim() = CType(gvUsados.Rows(index2).FindControl("txtOrden"), TextBox).Text.Trim Then
                        Dim errores As New Entities.EtiquetaError(1024)
                        source.ErrorMessage = errores.Descripcion
                        imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                        args.IsValid = False
                        Exit Sub
                    End If
                Next
            Next
        End If
    End Sub
End Class