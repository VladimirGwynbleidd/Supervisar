Imports Entities
Public Class CatalogoMenuPerfil
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
        Dim lstPerfiles As List(Of Perfil)
        Dim entPerfil As New Perfil()
        lstPerfiles = entPerfil.ObtenerPerfilesVigentes()
        'Se quita el Perfil 0 "Sin Acceso" ya que solo se utiliza para mostrar el menu antes de firmarse al sistema
        Dim perfilesSeleccionables = From s In lstPerfiles Where s.IdentificadorPerfil <> 0 Select New With {.Descripcion = s.Descripcion, .IdentificadorPerfil = s.IdentificadorPerfil}
        Utilerias.Generales.CargarCombo(ddlPerfil, perfilesSeleccionables, "Descripcion", "IdentificadorPerfil")

        Dim entMenu As New Menu()
        Dim lstMenu As DataView = entMenu.ObtenerTodos()
        'Solo se muestran los vigentes y se elimina el Menu 0 "Sin Acceso" ya que solo existe fuera del sistema
        lstMenu.RowFilter = "N_FLAG_VIG = 1 AND N_ID_MENU <> 0"
        Utilerias.Generales.CargarCombo(ddlMenu, lstMenu.ToTable, "T_DSC_MENU", "N_ID_MENU")
    End Sub

    Private Sub CargaSubMenus()
        If ddlMenu.SelectedValue <> "-1" And ddlPerfil.SelectedValue <> "-1" Then
            pnlSubMenus.Style.Add("display", "block")
            Dim entMenu As New SubMenu()
            Dim lstUsados As List(Of SubMenu) = entMenu.ObtenerSubMenuPerfil(Convert.ToInt32(ddlPerfil.SelectedValue), Convert.ToInt32(ddlMenu.SelectedValue))
            gvUsados.DataSource = lstUsados
            gvUsados.DataBind()

            Dim lstDisponibles As List(Of SubMenu) = entMenu.ObtenerSubMenuNoEnPerfil(Convert.ToInt32(ddlPerfil.SelectedValue), Convert.ToInt32(ddlMenu.SelectedValue))
            gvDisponibles.DataSource = lstDisponibles
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            pnlSubMenus.Style.Add("display", "none")
        End If
    End Sub

#End Region

#Region "Eventos controles"
    Protected Sub ddlPerfil_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPerfil.SelectedIndexChanged
        CargaSubMenus()
    End Sub

    Protected Sub ddlMenu_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlMenu.SelectedIndexChanged
        CargaSubMenus()
    End Sub

    Protected Sub btnExportaExcelUsados_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportaExcelUsados.Click
        Dim lstUsados As List(Of SubMenu) = TryCast(gvUsados.DataSourceSession, List(Of SubMenu))
        Dim dt As New DataTable()
        Dim dc As New DataColumn("IdentificadorSubMenu", GetType(Integer))
        Dim dc2 As New DataColumn("Descripcion", GetType(String))
        dt.Columns.Add(dc)
        dt.Columns.Add(dc2)
        For Each entSubMenu As SubMenu In lstUsados
            Dim dr As DataRow = dt.NewRow()
            dr.Item("IdentificadorSubMenu") = entSubMenu.IdentificadorSubMenu
            dr.Item("Descripcion") = entSubMenu.Descripcion
            dt.Rows.Add(dr)
        Next

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvUsados, "Catálogo Perfiles de Menú Asignados")

    End Sub

    Protected Sub btnExportaExcelDisponibles_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportaExcelDisponibles.Click
        Dim lstDisponibles As List(Of SubMenu) = TryCast(gvDisponibles.DataSourceSession, List(Of SubMenu))
        Dim dt As New DataTable()
        Dim dc As New DataColumn("IdentificadorSubMenu", GetType(Integer))
        Dim dc2 As New DataColumn("Descripcion", GetType(String))
        dt.Columns.Add(dc)
        dt.Columns.Add(dc2)
        For Each entSubMenu As SubMenu In lstDisponibles
            Dim dr As DataRow = dt.NewRow()
            dr.Item("IdentificadorSubMenu") = entSubMenu.IdentificadorSubMenu
            dr.Item("Descripcion") = entSubMenu.Descripcion
            dt.Rows.Add(dr)
        Next

        Dim exportaExcel As New Utilerias.ExportarExcel
        exportaExcel.ExportaGrid(dt, gvUsados, "Catálogo Perfiles de Menú Disponibles")
    End Sub

    Protected Sub btnRemueve_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemueve.Click
        Dim lstGVRow As List(Of GridViewRow) = gvUsados.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstUsados As List(Of SubMenu) = TryCast(gvUsados.DataSourceSession, List(Of SubMenu))
            Dim lstDisponibles As List(Of SubMenu) = TryCast(gvDisponibles.DataSourceSession, List(Of SubMenu))
            For Each gvRow In lstGVRow
                For index = 0 To lstUsados.Count - 1
                    If Convert.ToInt32(gvRow.Cells(1).Text) = lstUsados(index).IdentificadorSubMenu Then
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
            Dim lstUsados As List(Of SubMenu) = TryCast(gvUsados.DataSourceSession, List(Of SubMenu))
            Dim lstDisponibles As List(Of SubMenu) = TryCast(gvDisponibles.DataSourceSession, List(Of SubMenu))
            For Each gvRow In lstGVRow
                For index = 0 To lstDisponibles.Count - 1
                    If Convert.ToInt32(gvRow.Cells(1).Text) = lstDisponibles(index).IdentificadorSubMenu Then
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
                If ddlMenu.SelectedValue <> "-1" And ddlPerfil.SelectedValue <> "-1" Then
                    Guardar()
                Else
                    Dim errores As New Entities.EtiquetaError(23)
                    lblMensaje.Text = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
                End If
            Case "btnCancelar"
                Response.Redirect("CatalogoMenuPerfil.aspx", True)
        End Select

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        Dim errores As New EtiquetaError(117)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cancelar", "LevantaVentanaConfirma();", True)

    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        If ddlMenu.SelectedValue <> "-1" And ddlPerfil.SelectedValue <> "-1" Then
            btnAceptarM2B1A.CommandArgument = "btnAceptar"

            Dim errores As New EtiquetaError(118)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
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
    Private Function ObtenAgregados() As List(Of SubMenu)
        Dim entSubMenu As New SubMenu()
        Dim lstUsados As List(Of SubMenu) = entSubMenu.ObtenerSubMenuPerfil(Convert.ToInt32(ddlPerfil.SelectedValue), Convert.ToInt32(ddlMenu.SelectedValue))

        Dim lstAgregados As New List(Of SubMenu)

        For Each gvrow As GridViewRow In gvUsados.Rows
            Dim existe As Boolean = False
            For index = 0 To lstUsados.Count - 1
                If lstUsados(index).IdentificadorSubMenu = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstAgregados.Add(New SubMenu(Convert.ToInt32(ddlMenu.SelectedValue), Convert.ToInt32(gvrow.Cells(1).Text)))
            End If
        Next

        Return lstAgregados
    End Function

    ''' <summary>
    ''' Devuelve los registros agregados a gvDisponibles
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenEliminados() As List(Of SubMenu)
        Dim entSubMenu As New SubMenu()
        Dim lstDisponibles As List(Of SubMenu) = entSubMenu.ObtenerSubMenuNoEnPerfil(Convert.ToInt32(ddlPerfil.SelectedValue), Convert.ToInt32(ddlMenu.SelectedValue))

        Dim lstEliminados As New List(Of SubMenu)

        For Each gvrow As GridViewRow In gvDisponibles.Rows
            Dim existe As Boolean = False
            For index = 0 To lstDisponibles.Count - 1
                If lstDisponibles(index).IdentificadorSubMenu = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstEliminados.Add(New SubMenu(Convert.ToInt32(ddlMenu.SelectedValue), Convert.ToInt32(gvrow.Cells(1).Text)))
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
            Dim lstAgregados As List(Of SubMenu) = ObtenAgregados()

            Dim lstEliminados As List(Of SubMenu) = ObtenEliminados()

            Dim entSubMenu As New SubMenu()

            entSubMenu.ActualizaSubMenuPerfil(Convert.ToInt32(ddlPerfil.SelectedValue), lstAgregados, lstEliminados)

            CargaSubMenus()

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

End Class