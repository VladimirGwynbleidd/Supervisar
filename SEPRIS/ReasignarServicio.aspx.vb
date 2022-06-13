Imports System
Imports System.Web
Imports System.Web.Configuration
Imports Utilerias
Imports Entities
Imports System.Net

Public Class ReasignarServicio
    Inherits System.Web.UI.Page
    Public Property Mensaje As String
    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        gvServicios.ArmaMultiScript()
        gvIngenieros.ArmaMultiScript()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MuestraImagenes()
    End Sub

    ''' <summary>
    ''' Obtiene la ruta de la imagen de estatus
    ''' </summary>
    ''' <param name="estatus">Estatus del registro</param>
    ''' <returns>Ruta de la imagen</returns>
    ''' <remarks></remarks>
    Public Function ObtenerImagenEstatus(ByVal estatus As Integer) As String
        Dim img As Imagen
        If estatus = 3 Then
            img = New Imagen(73)
            Return "~/Imagenes/Errores/" + img.Ruta
        ElseIf estatus = 11 Then
            img = New Imagen(72)
            Return "~/Imagenes/Errores/" + img.Ruta
        Else
            img = New Imagen(71)
            Return "~/Imagenes/Errores/" + img.Ruta
        End If

    End Function

    ''' <summary>
    ''' Muestra y esconde las imagenes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraImagenes()
        Dim img As Imagen
        If gvServicios.Rows.Count > 0 Then
            imgNoServicios.Visible = False
        Else
            imgNoServicios.Visible = True
        End If
        If gvIngenieros.Rows.Count > 0 Then
            imgNoInges.Visible = False
        Else
            imgNoInges.Visible = True
        End If
        img = New Imagen(71)
        imgERROR.ImageUrl = "~/Imagenes/Errores/" + img.Ruta
        img = New Imagen(72)
        Image1.ImageUrl = "~/Imagenes/Errores/" + img.Ruta
        img = New Imagen(73)
        Image2.ImageUrl = "~/Imagenes/Errores/" + img.Ruta
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        If String.IsNullOrEmpty(txtFolioSolicitud.Text) Then
            Dim errores As New Entities.EtiquetaError(2075)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If
        If regexFolio.IsValid Then
            ViewState("Folio") = Nothing
            CargarServiciosFolio()
        End If
    End Sub

    Public Sub CargarServiciosFolio()
        If Not ViewState("Folio") Is Nothing Then
            txtFolioSolicitud.Text = CStr(ViewState("Folio"))
        Else
            ViewState("Folio") = txtFolioSolicitud.Text
        End If
        Dim objSol As New Solicitud(txtFolioSolicitud.Text)
        gvServicios.DataSource = objSol.ObtenerServiciosAtencion(True)
        gvServicios.DataBind()
        MuestraImagenes()
    End Sub

    Protected Sub btnReasignar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReasignar.Click
        If Not HayRegistroSeleccionado(gvServicios) Then
            Exit Sub
        End If
        Dim dtInge As New DataTable
        For Each row As GridViewRow In gvServicios.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then
                Dim idNivel As Integer = CInt(gvServicios.DataKeys(row.RowIndex)("N_ID_NIVELES_SERVICIO").ToString())
                ViewState("NivelSeleccionado") = idNivel
                Dim objNivel As New NivelServicio(idNivel)
                Dim dt As DataTable = TryCast(gvServicios.DataSourceSession, DataTable)
                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        If CInt(dr("N_ID_NIVELES_SERVICIO")) = idNivel Then
                            objNivel.IngenieroResponsable = CStr(dr("T_ID_INGENIERO"))
                        End If
                    Next
                End If
                dtInge = objNivel.ObtenerIngenierosArea()
                Dim objAgenda As New Agenda()
                dtInge.Columns.Add("FECHA_DISPONIBLE")
                For Each dr As DataRow In dtInge.Rows
                    objAgenda.Ingeniero = dr("T_ID_INGENIERO_RESPONSABLE")
                    Dim fechIni As DateTime
                    Negocio.ReasignarServicio.DeterminarFechaInicio(objAgenda, fechIni)
                    dr("FECHA_DISPONIBLE") = fechIni
                Next
                Exit For
            End If
        Next
        gvIngenieros.DataSource = dtInge
        gvIngenieros.DataBind()
        MuestraImagenes()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SEDI", "MensajeAtencion();", True)
    End Sub

    Private Function HayRegistroSeleccionado(ByVal grid As CustomGridView.CustomGridView) As Boolean

        Dim haySeleccion As Boolean = False

        For Each row As GridViewRow In grid.Rows

            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)

            If elemento.Checked Then

                haySeleccion = True
                Exit For

            End If

        Next

        If Not haySeleccion Then
            Dim errores As New Entities.EtiquetaError(2056)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

        Return haySeleccion

    End Function

    Protected Sub btnAgendarFinal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgendarFinal.Click
        If Not HayRegistroSeleccionado(gvServicios) Then
            Exit Sub
        End If
        For Each row As GridViewRow In gvServicios.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then
                Dim idNivel As Integer = CInt(gvServicios.DataKeys(row.RowIndex)("N_ID_NIVELES_SERVICIO").ToString())
                ViewState("NivelSeleccionado") = idNivel
                Exit For
            End If
        Next
        Reagendar()
        'btnAceptarM2B1A.CommandArgument = "Reagendar"
        'Dim errores As New Entities.EtiquetaError(2068)
        'Mensaje = errores.Descripcion
        'imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "MensajeConfirmacion();", True)
    End Sub

    Private Sub btnAceptarInges_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptarInges.Click
        'btnAceptarM2B1A.CommandArgument = "Reasignar"
        'Dim errores As New Entities.EtiquetaError(2067)
        'Mensaje = errores.Descripcion
        'imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        Reasignar()
    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click
        Select Case btnAceptarM2B1A.CommandArgument
            Case "Reasignar"
                Reasignar()
            Case "Reagendar"
                Reagendar()
        End Select
    End Sub

    Public Sub Reasignar()
        Dim reasigno As Boolean = False
        If Not HayRegistroSeleccionado(gvIngenieros) Then
            Exit Sub
        End If

        For Each row As GridViewRow In gvIngenieros.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim idInge As String = CStr(gvIngenieros.DataKeys(row.RowIndex)("T_ID_INGENIERO_RESPONSABLE").ToString())
                Dim idNivel As Integer = CInt(ViewState("NivelSeleccionado"))

                Dim objReasignarServicio As New Negocio.ReasignarServicio

                reasigno = objReasignarServicio.Reagendar(ViewState("Folio").ToString, idNivel, Negocio.ReasignarServicio.Tipo.Reasignar, idInge)
                Dim inge As New Usuario(idInge)
                GuardarHistorial(inge.Nombre + " " + inge.Apellido + " " + inge.ApellidoAuxiliar)
                Exit For
            End If
        Next
        CargarServiciosFolio()

        If reasigno Then

            Dim errores As New Entities.EtiquetaError(2076)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        Else
            Dim errores As New Entities.EtiquetaError(2110)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

    End Sub

    Private Sub GuardarHistorial(ByVal Ingeniero As String)
        Dim idSol As Integer = New Solicitud(ViewState("Folio").ToString).Identificador
        Dim objHistorial As New HistorialSolicitud
        Dim usuario As Usuario = TryCast(Session(Entities.Usuario.SessionID), Usuario)
        Dim objAgenda As New Agenda()
        Dim fE As String = String.Empty
        Dim fP As String = String.Empty
        objAgenda.ObtenerFechasTermino(idSol, fE, fP)
        objHistorial.Identificador = objHistorial.ObtenerSiguienteIdentificador
        objHistorial.IdSolicitudConsecutivo = objHistorial.ObtenerSiguienteConsecutivo(idSol)
        objHistorial.IdSolicitud = idSol
        objHistorial.FechaRegistro = DateTime.Now
        objHistorial.IdUsuario = usuario.IdentificadorUsuario
        objHistorial.DescAccion = "El servicio " + ViewState("NivelSeleccionado").ToString + " fue reasignado al ingeniero " + Ingeniero
        If Not String.IsNullOrEmpty(fE) Then
            objHistorial.FechaVencimiento = CDate(fE)
        ElseIf Not String.IsNullOrEmpty(fP) Then
            objHistorial.FechaVencimiento = CDate(fP)
        End If
        objHistorial.Agregar()
    End Sub

    Public Sub Reagendar()
        Dim reagendo As Boolean = False

        Dim idNivel As Integer = CInt(ViewState("NivelSeleccionado"))

        Dim objReasignarServicio As New Negocio.ReasignarServicio

        reagendo = objReasignarServicio.Reagendar(ViewState("Folio").ToString, idNivel, Negocio.ReasignarServicio.Tipo.Reagendar)

        CargarServiciosFolio()

        If reagendo Then
            Dim errores As New Entities.EtiquetaError(2077)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        Else
            Dim errores As New Entities.EtiquetaError(2111)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If
        
    End Sub


End Class