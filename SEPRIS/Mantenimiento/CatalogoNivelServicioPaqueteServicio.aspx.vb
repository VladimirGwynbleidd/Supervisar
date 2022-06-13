Public Class CatalogoNivelServicioPaqueteServicio
    Inherits System.Web.UI.Page

    Public Property Mensaje As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargaDDL()
        End If
    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        gvDisponibles.ArmaMultiScript()
    End Sub

#Region "Carga Datos"

    Private Sub CargaDDL()
        Dim paquete As New Entities.Paquete
        Dim dvPaquete As DataView = paquete.ObtenerTodos()
        dvPaquete.RowFilter = "B_FLAG_VIG=1"
        Utilerias.Generales.CargarCombo(ddlPaqueteServicio, dvPaquete.ToTable, "T_DSC_GRUPO_SERVICIO", "N_ID_GRUPO_SERVICIO")
    End Sub

    Private Sub CargaOpciones()
        If ddlPaqueteServicio.SelectedIndex > 0 Then

            pnlSubMenus.Style.Add("display", "block")

            Dim paquete As New Entities.Paquete(ddlPaqueteServicio.SelectedValue)
            gvUsados.DataSource = paquete.ObtenerNivelesServicio()
            gvUsados.DataBind()


            gvDisponibles.DataSource = paquete.ObtenerNivelesServicioDisponibles()
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            pnlSubMenus.Style.Add("display", "none")
        End If
    End Sub

#End Region

#Region "Eventos controles"


    Protected Sub btnExportaExcelUsados_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportaExcelUsados.Click
        Dim lstUsados As List(Of Entities.NivelServicio) = TryCast(gvUsados.DataSourceSession, List(Of Entities.NivelServicio))
        Dim dt As New DataTable()
        Dim dc As New DataColumn("Identificador", GetType(Integer))
        Dim dc2 As New DataColumn("Descripcion", GetType(String))
        dt.Columns.Add(dc)
        dt.Columns.Add(dc2)
        For Each entNivelServicio As Entities.NivelServicio In lstUsados
            Dim dr As DataRow = dt.NewRow()
            dr.Item("Identificador") = entNivelServicio.Identificador
            dr.Item("Descripcion") = entNivelServicio.Descripcion
            dt.Rows.Add(dr)
        Next

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvUsados, "Catálogo Paquetes de Servicio")

    End Sub

    Protected Sub btnExportaExcelDisponibles_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportaExcelDisponibles.Click
        Dim lstDisponibles As List(Of Entities.NivelServicio) = TryCast(gvDisponibles.DataSourceSession, List(Of Entities.NivelServicio))
        Dim dt As New DataTable()
        Dim dc As New DataColumn("Identificador", GetType(Integer))
        Dim dc2 As New DataColumn("Descripcion", GetType(String))
        dt.Columns.Add(dc)
        dt.Columns.Add(dc2)
        For Each entNivelServicio As Entities.NivelServicio In lstDisponibles
            Dim dr As DataRow = dt.NewRow()
            dr.Item("Identificador") = entNivelServicio.Identificador
            dr.Item("Descripcion") = entNivelServicio.Descripcion
            dt.Rows.Add(dr)
        Next

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvUsados, "Catálogo Paquete de Servicio")
    End Sub

    Protected Sub btnRemueve_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemueve.Click
        Dim lstGVRow As List(Of GridViewRow) = gvUsados.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstUsados As List(Of Entities.NivelServicio) = TryCast(gvUsados.DataSourceSession, List(Of Entities.NivelServicio))
            Dim lstDisponibles As List(Of Entities.NivelServicio) = TryCast(gvDisponibles.DataSourceSession, List(Of Entities.NivelServicio))
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
            Dim errores As New Entities.EtiquetaError(23)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnAgrega_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgrega.Click
        Dim lstGVRow As List(Of GridViewRow) = gvDisponibles.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstUsados As List(Of Entities.NivelServicio) = TryCast(gvUsados.DataSourceSession, List(Of Entities.NivelServicio))
            Dim lstDisponibles As List(Of Entities.NivelServicio) = TryCast(gvDisponibles.DataSourceSession, List(Of Entities.NivelServicio))
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
            Dim errores As New Entities.EtiquetaError(23)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click

        Select Case btnAceptarM2B1A.CommandArgument
            Case "btnAceptar"
                If ddlPaqueteServicio.SelectedIndex > 0 Then
                    Guardar()
                Else
                    Dim errores As New Entities.EtiquetaError(23)
                    lblMensaje.Text = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
                End If
            Case "btnCancelar"

                ddlPaqueteServicio.SelectedIndex = 0
                ddlPaqueteServicio_SelectedIndexChanged(sender, e)
        End Select

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        Dim errores As New Entities.EtiquetaError(1171)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cancelar", "LevantaVentanaConfirma();", True)

    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        If ddlPaqueteServicio.SelectedIndex > 0 Then
            btnAceptarM2B1A.CommandArgument = "btnAceptar"

            Dim errores As New Entities.EtiquetaError(1170)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cancelar", "LevantaVentanaConfirma();", True)
        Else
            Dim errores As New Entities.EtiquetaError(25)
            lblMensaje.Text = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Devuelve los registros agregados a gvUsados
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenAgregados() As List(Of Entities.NivelServicio)
        Dim entPaquete As New Entities.Paquete(ddlPaqueteServicio.SelectedValue)
        Dim lstUsados As List(Of Entities.NivelServicio) = entPaquete.ObtenerNivelesServicio
        Dim lstAgregados As New List(Of Entities.NivelServicio)

        For Each gvrow As GridViewRow In gvUsados.Rows
            Dim existe As Boolean = False
            For index = 0 To lstUsados.Count - 1
                If lstUsados(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstAgregados.Add(New Entities.NivelServicio(Convert.ToInt32(gvrow.Cells(1).Text)))
            End If
        Next

        Return lstAgregados
    End Function

    ''' <summary>
    ''' Devuelve los registros agregados a gvDisponibles
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenEliminados() As List(Of Entities.NivelServicio)
        Dim entPaquete As New Entities.Paquete(ddlPaqueteServicio.SelectedValue)
        Dim lstDisponibles As List(Of Entities.NivelServicio) = entPaquete.ObtenerNivelesServicioDisponibles()


        Dim lstEliminados As New List(Of Entities.NivelServicio)

        For Each gvrow As GridViewRow In gvDisponibles.Rows
            Dim existe As Boolean = False
            For index = 0 To lstDisponibles.Count - 1
                If lstDisponibles(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstEliminados.Add(New Entities.NivelServicio(Convert.ToInt32(gvrow.Cells(1).Text)))
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
            Dim lstAgregados As List(Of Entities.NivelServicio) = ObtenAgregados()

            Dim lstEliminados As List(Of Entities.NivelServicio) = ObtenEliminados()

            Dim entPaquete As New Entities.Paquete(ddlPaqueteServicio.SelectedValue)

            entPaquete.ActualizarNivelesServicio(lstAgregados, lstEliminados)

            CargaOpciones()

        Catch ex As Exception
            Dim errores As New Entities.EtiquetaError(55)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End Try
    End Sub

    ''' <summary>
    ''' Muestra y esconde las imagenes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraImagenes()
        If gvDisponibles.Rows.Count > 0 Then
            imgDisponibles.Visible = False
        Else
            imgDisponibles.Visible = True
        End If

        If gvUsados.Rows.Count > 0 Then
            imgUsados.Visible = False
        Else
            imgUsados.Visible = True
        End If
        If gvDisponibles.Rows.Count = 0 And gvUsados.Rows.Count = 0 Then
            imgDatos.Visible = True
            pnlSubMenus.Visible = False
        Else
            imgDatos.Visible = False
            pnlSubMenus.Visible = True
        End If
    End Sub

#End Region


    Protected Sub ddlPaqueteServicio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPaqueteServicio.SelectedIndexChanged

        If ddlPaqueteServicio.SelectedIndex > 0 Then
            CargaOpciones()
            tblBotones.Visible = True
        Else
            CargaOpciones()
            tblBotones.Visible = False
        End If


    End Sub
End Class