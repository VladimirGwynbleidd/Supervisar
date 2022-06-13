
'- Fecha de creación:15/07/2013
'- Fecha de modificación: 
'- Nombre del Responsable: ARGC1
'- Empresa: Softtek
'- Página de periodos de no asignacion por usuarios 


Imports Utilerias
Imports Negocio

Public Class PeriodosNoAsignacion
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            If Session("cUsuario") IsNot Nothing Then
                ViewState("cUsuario") = Session("cUsuario")
                Session.Remove("cUsuario")
            End If

            'nUsuario-- nombre completo del usuario
            If Session("nUsuario") IsNot Nothing Then
                ViewState("nUsuario") = Session("nUsuario")
                Session.Remove("nUsuario")
            End If

            If ViewState("cUsuario") Is Nothing Then
                Response.Redirect("CarruselAsignacion.aspx")

            End If

            lblNombreUsuario.Text = ViewState("nUsuario").ToString

            If Not IsPostBack Then

                RellenaGrid()

            End If
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        End Try
    End Sub

    Public Sub RellenaGrid()

        Dim ds As DataTable = Carrusel.ObtienePeriodosDeNoAsignacion(ViewState("cUsuario").ToString)


        gvGridAsignaciones.DataSource = ds
        gvGridAsignaciones.DataBind()

        If gvGridAsignaciones.Rows.Count = 0 Then


            gvGridAsignaciones.Style.Value = "display:none"
            pnlNoExiste.Visible = True
        Else
            gvGridAsignaciones.Style.Value = "display:block"
            pnlNoExiste.Visible = False
        End If


    End Sub

    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Dim objCarrusel As New Carrusel

        If objCarrusel.VerificaFechas(TxtFechaIni.Text, TxtFechaFin.Text, ViewState("cUsuario")) Then
            '----------------------------------------
            '   hacer el insert en la tabla de relación [dbo].[BDS_R_GR_USUARIO_CARRUSEL_PERIODO_NO_ASIGNADO]
            '   esta tabla mantiene los periodos de NO asignados a cada usuario (si tiene).
            '---------------------------------------
            Dim FechaInicial, FechaFinal As String

         
            FechaInicial = Convert.ToDateTime(TxtFechaIni.Text).ToString("MM/dd/yyyy")
            FechaFinal = Convert.ToDateTime(TxtFechaFin.Text).ToString("MM/dd/yyyy")

            If Carrusel.AgregarPeriodo(FechaInicial, FechaFinal, ViewState("cUsuario")) Then

                RellenaGrid()

                Exit Sub
            Else
                Dim errores As New Entities.EtiquetaError(142)
                lblTextoMensaje.Text = errores.Descripcion
                imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "mensaje", "MensajeModal()", True)
            End If
        Else
            If Carrusel.FechaValida(TxtFechaIni.Text) And Carrusel.FechaValida(TxtFechaFin.Text) Then
                Dim FechaInicial, FechaFinal As Date
                FechaInicial = DateTime.ParseExact(TxtFechaIni.Text, "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture)
                FechaFinal = DateTime.ParseExact(TxtFechaFin.Text, "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture)

                If FechaInicial < Date.Today Then
                    Dim errores As New Entities.EtiquetaError(143)
                    lblTextoMensaje.Text = errores.Descripcion
                    imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "mensaje", "MensajeModal()", True)
                ElseIf FechaInicial > FechaFinal Then
                    Dim errores As New Entities.EtiquetaError(144)
                    lblTextoMensaje.Text = errores.Descripcion
                    imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "mensaje", "MensajeModal()", True)

                Else

                    If Not Carrusel.RevisaNoTraslape(TxtFechaIni.Text, TxtFechaFin.Text, ViewState("cUsuario")) Then
                        Dim errores As New Entities.EtiquetaError(145)
                        lblTextoMensaje.Text = errores.Descripcion
                        imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "mensaje", "MensajeModal()", True)
                    End If

                End If

            Else
                Dim errores As New Entities.EtiquetaError(146)
                lblTextoMensaje.Text = errores.Descripcion
                imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "mensaje", "MensajeModal()", True)
            End If

            End If


    End Sub

   
    Protected Sub cmdRegresar_Click(sender As Object, e As EventArgs) Handles cmdRegresar.Click
        Response.Redirect("CarruselAsignacion.aspx", True)
    End Sub

    Protected Sub gvGridAsignaciones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvGridAsignaciones.RowCommand

        ' Convert the row index stored in the CommandArgument
        ' property to an Integer.

        Dim index As Integer = Convert.ToInt32(e.CommandArgument)

        ' Retrieve the row that contains the button clicked 
        ' by the user from the Rows collection.
        Dim row As GridViewRow = gvGridAsignaciones.Rows(index)

        '''''''''''
        Dim FechaInicial As String = row.Cells(0).Text

        Dim FechaFinal As String = row.Cells(1).Text

        If Carrusel.EliminaPeriodo(ViewState("cUsuario"), FechaInicial, FechaFinal) Then

            RellenaGrid()
        Else
            Dim errores As New Entities.EtiquetaError(147)
            lblTextoMensaje.Text = errores.Descripcion
            imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "mensaje", "MensajeModal()", True)
        End If

    End Sub
End Class