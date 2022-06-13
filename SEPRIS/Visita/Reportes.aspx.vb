Imports System.Web.Configuration
Imports System.Net
Imports Clases
Imports Entities
Imports Utilerias
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Web.Services.Protocols

Public Class Reportes
   Inherits System.Web.UI.Page

#Region "Propiedades"
   Public Property Mensaje As String
#End Region

   Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

      If Not IsPostBack Then

         If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
            ''Guarda el area del usuario en el viewstate
            Dim usuario As New Entities.Usuario()
            usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session
         End If
      Else
         Page.MaintainScrollPositionOnPostBack = True
      End If

   End Sub

   Protected Sub imgInicio_Click(sender As Object, e As ImageClickEventArgs) Handles imgInicio.Click
      Response.Redirect("../Visita/Bandeja.aspx")
   End Sub

   Private Sub catch_cone(ByVal e As Exception, ByVal s As String)
      EventLogWriter.EscribeEntrada("Funcion " & s & ": " & e.ToString(), EventLogEntryType.Error)
   End Sub

   Private Sub chkGrafica1_CheckedChanged(sender As Object, e As EventArgs) Handles chkGrafica1.CheckedChanged
      chkGrafica2.Checked = False
      chkGrafica3.Checked = False
      chkGrafica4.Checked = False

      containerG1.Visible = False

   End Sub
   
   Protected Sub chkGrafica2_CheckedChanged(sender As Object, e As EventArgs) Handles chkGrafica2.CheckedChanged
      chkGrafica1.Checked = False
      chkGrafica3.Checked = False
      chkGrafica4.Checked = False

      containerG1.Visible = False

      Response.Redirect("../Visita/Reporte2.aspx")
   End Sub

   Protected Sub chkGrafica3_CheckedChanged(sender As Object, e As EventArgs) Handles chkGrafica3.CheckedChanged
      chkGrafica1.Checked = False
      chkGrafica2.Checked = False
      chkGrafica4.Checked = False

      containerG1.Visible = False

      Response.Redirect("../Visita/Reporte3.aspx")
   End Sub

   Protected Sub chkGrafica4_CheckedChanged(sender As Object, e As EventArgs) Handles chkGrafica4.CheckedChanged
      chkGrafica1.Checked = False
      chkGrafica2.Checked = False
      chkGrafica3.Checked = False

      containerG1.Visible = False
      Response.Redirect("../Visita/Reporte4.aspx")

   End Sub

   Protected Sub btnBuscar_Click(sender As Object, e As ImageClickEventArgs) Handles btnBuscar.Click
      Dim fechaInicio As Date
      Dim fechaFin As Date
      Dim grafica As New Graficas()

      If Date.TryParse(txtFechaRangoIni.Text.Trim(), fechaInicio) And Date.TryParse(txtFechaRangoFin.Text.Trim(), fechaFin) Then
         Dim res1 As Integer = Date.Compare(fechaInicio, fechaFin)
         '0  - es la misma fecha
         '-1 - es mas pequeña la fecha de inicio que la fecha de fin
         '1  - es mas grande la fecha de inicio que la fecha de fin
         If res1 = 1 Then
            Mensaje = "La fecha inicial no puede ser mayor a la fecha, por favor verifica."
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
         Else
            'SE VALIDARÁ CON BASE EN EL CHECK SELECCIONADO
            If chkGrafica1.Checked Then
               '1 = Total de Visitas por Área
               '2 = Total de Visitas por Área y Estatus
               '3 = Total de Visitas por Área y Entidad
               '4 = Total de Visitas por Tipo de Visita y Área
               '36 = VF
               '35 = VO
               '1 = CGIV
               '42 = PLD

               grafica = AccesoBD.getDatosGrafica(1, fechaInicio, fechaFin, 0)

               'Total de Visitas por Área
               lblTotalVFG1.Text = grafica.VisitasVF
               lblTotalVOG1.Text = grafica.VisitasVO
               lblTotalCGIVG1.Text = grafica.VisitasCGIV
               lblTotalPLDG1.Text = grafica.VisitasPLD

               containerG1.Visible = True
               
               End If

         End If

      Else

            Mensaje = "Por favor ingresa una fecha válida."

            If Not Date.TryParse(txtFechaRangoIni.Text.Trim(), fechaInicio) Then
               txtFechaRangoIni.Text = ""
            End If

            If Not Date.TryParse(txtFechaRangoFin.Text.Trim(), fechaFin) Then
               txtFechaRangoFin.Text = ""
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
      End If

   End Sub

End Class