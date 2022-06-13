'- Fecha de creación: 14/11/2013
'- Nombre del Responsable: Rafael Rodríguez Sánchez
'- Empresa: Softtek
'- Permite agregar o remover copias de correos a un usuario

Imports Entities

Public Class CopiaCorreoUsuario
    Inherits System.Web.UI.Page

    Public Property Mensaje As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargaPerfiles()
        End If
    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        gvAsignados.ArmaMultiScript()
    End Sub

#Region "Carga Datos"
    ''' <summary>
    ''' Carga los perfiles en el DropDownList ddlPerfil
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaPerfiles()
        Dim entUsuario As New Usuario()
        Dim dvUsuario As DataView
        dvUsuario = entUsuario.ObtenerTodos()

        Dim consulta As String = "N_FLAG_VIG=1"
        dvUsuario.RowFilter = consulta

        Utilerias.Generales.CargarCombo(ddlUsuario, dvUsuario, "T_DSC_NOMBRE", "T_ID_USUARIO")

    End Sub

    ''' <summary>
    ''' Carga los correos en los GridViews
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaCorreos()
        If ddlUsuario.SelectedValue <> "-1" Then
            pnlCorreos.Style.Add("display", "block")
            btnMostrarMensaje.Visible = True
            btnCancelar.Visible = True

            Dim entCorreo As New Correo
            Dim lstCorreoAsignados As List(Of Correo) = entCorreo.ObtenerCorreosCopia(ddlUsuario.SelectedValue)
            Dim lstCorreoNoAsignado As List(Of Correo) = entCorreo.ObtenerCorreosDisponibleCopia(ddlUsuario.SelectedValue)

            gvAsignados.DataSource = lstCorreoAsignados
            gvAsignados.DataBind()

            gvDisponibles.DataSource = lstCorreoNoAsignado
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            pnlCorreos.Style.Add("display", "none")
            btnMostrarMensaje.Visible = False
            btnCancelar.Visible = False
        End If
        

    End Sub

#End Region

#Region "Eventos Controles"
    Private Sub ddlUsuario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUsuario.SelectedIndexChanged
        CargaCorreos()
    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click
        Select Case btnAceptarM2B1A.CommandArgument
            Case "btnAceptar"
                If ddlUsuario.SelectedValue <> "-1" Then
                    Guardar()
                Else
                    Dim errores As New Entities.EtiquetaError(25)
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    lblMensaje.Text = errores.Descripcion
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
                End If
            Case "btnCancelar"
                Response.Redirect("~/Correos/MenuOpciones.aspx", True)
        End Select

    End Sub

    Protected Sub btnMostrarMensaje_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMostrarMensaje.Click
        Page.Validate("Forma")

        If Page.IsValid() Then
            btnAceptarM2B1A.CommandArgument = "btnAceptar"

            Dim errores As New Entities.EtiquetaError(168)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Guardar", "LevantaVentanaConfirma();", True)
        Else
            lblMensaje.Text = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        Dim errores As New Entities.EtiquetaError(169)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Cancelar", "LevantaVentanaConfirma();", True)
    End Sub

    Protected Sub btnRemueve_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemueve.Click
        Dim lstGVRow As List(Of GridViewRow) = gvAsignados.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim lstExcluidos As List(Of Correo) = TryCast(gvAsignados.DataSourceSession, List(Of Correo))
            Dim lstEnvian As List(Of Correo) = TryCast(gvDisponibles.DataSourceSession, List(Of Correo))
            For Each gvRow In lstGVRow
                For index = 0 To lstExcluidos.Count - 1
                    If Convert.ToInt32(gvRow.Cells(1).Text) = lstExcluidos(index).Identificador Then
                        lstEnvian.Add(lstExcluidos(index))
                        lstExcluidos.Remove(lstExcluidos(index))
                        Exit For
                    End If
                Next
            Next

            gvAsignados.DataSource = lstExcluidos
            gvAsignados.DataBind()

            gvDisponibles.DataSource = lstEnvian
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
            Dim lstExcluidos As List(Of Correo) = TryCast(gvAsignados.DataSourceSession, List(Of Correo))
            Dim lstEnvian As List(Of Correo) = TryCast(gvDisponibles.DataSourceSession, List(Of Correo))
            For Each gvRow In lstGVRow
                For index = 0 To lstEnvian.Count - 1
                    If Convert.ToInt32(gvRow.Cells(1).Text) = lstEnvian(index).Identificador Then
                        lstExcluidos.Add(lstEnvian(index))
                        lstEnvian.Remove(lstEnvian(index))
                        Exit For
                    End If
                Next
            Next

            gvAsignados.DataSource = lstExcluidos
            gvAsignados.DataBind()

            gvDisponibles.DataSource = lstEnvian
            gvDisponibles.DataBind()

            MuestraImagenes()
        Else
            Dim errores As New Entities.EtiquetaError(23)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

#End Region

#Region "Metodos"
    ''' <summary>
    ''' Muestra y esconde las imagenes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraImagenes()
        If gvAsignados.Rows.Count > 0 Then
            imgAsignados.Visible = False
        Else
            imgAsignados.Visible = True
        End If

        If gvDisponibles.Rows.Count > 0 Then
            imgDisponibles.Visible = False
        Else
            imgDisponibles.Visible = True
        End If

        If gvDisponibles.Rows.Count = 0 And gvAsignados.Rows.Count = 0 Then
            imgDatos.Visible = True
            pnlCorreos.Visible = False
        Else
            imgDatos.Visible = False
            pnlCorreos.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Devuelve los registros agregados a gvAsignados
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenAgregados() As List(Of Correo)
        Dim entCorreo As New Correo()
        Dim lstAsignados As List(Of Correo) = entCorreo.ObtenerCorreosCopia(ddlUsuario.SelectedValue)

        Dim lstAgregados As New List(Of Correo)

        For Each gvrow As GridViewRow In gvAsignados.Rows
            Dim existe As Boolean = False
            For index = 0 To lstAsignados.Count - 1
                If lstAsignados(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstAgregados.Add(New Correo(Convert.ToInt32(gvrow.Cells(1).Text)))
            End If
        Next

        Return lstAgregados
    End Function

    ''' <summary>
    ''' Devuelve los registros agregados a gvDisponibles
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenEliminados() As List(Of Correo)
        Dim entCorreo As New Correo()
        Dim lstDisponibles As List(Of Correo) = entCorreo.ObtenerCorreosDisponibleCopia(ddlUsuario.SelectedValue)

        Dim lstEliminados As New List(Of Correo)

        For Each gvrow As GridViewRow In gvDisponibles.Rows
            Dim existe As Boolean = False
            For index = 0 To lstDisponibles.Count - 1
                If lstDisponibles(index).Identificador = Convert.ToInt32(gvrow.Cells(1).Text) Then
                    existe = True
                End If
            Next
            If existe = False Then
                lstEliminados.Add(New Correo(Convert.ToInt32(gvrow.Cells(1).Text)))
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

            Dim entCorreo As New Correo()

            entCorreo.GuardaCopiaCorreo(lstAgregar, lstQuitar, ddlUsuario.SelectedValue)

            CargaCorreos()
        Catch ex As Exception
            Dim errores As New Entities.EtiquetaError(55)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            lblMensaje.Text = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End Try
    End Sub

#End Region

#Region "Validadores"
    Private Sub cvPerfil_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvUsuario.ServerValidate
        If ddlUsuario.SelectedValue = "-1" Then
            Dim errores As New Entities.EtiquetaError(25)

            source.ErrorMessage = errores.Descripcion
            args.IsValid = False
            If String.IsNullOrEmpty(imgUnBotonNoAccion.ImageUrl) Then
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If
        End If
    End Sub
#End Region

    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("~/Correos/MenuOpciones.aspx")
    End Sub
End Class