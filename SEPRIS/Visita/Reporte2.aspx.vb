Imports System.Web.Configuration
Imports System.Net
Imports Clases
Imports Entities
Imports Utilerias
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Web.Services.Protocols

Public Class Reporte2
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

      'containerG1.Visible = False

      Response.Redirect("../Visita/Reportes.aspx")
   End Sub

   Protected Sub chkGrafica2_CheckedChanged(sender As Object, e As EventArgs) Handles chkGrafica2.CheckedChanged
      chkGrafica1.Checked = False
      chkGrafica3.Checked = False
      chkGrafica4.Checked = False

      'containerG1.Visible = False

   End Sub

   Protected Sub chkGrafica3_CheckedChanged(sender As Object, e As EventArgs) Handles chkGrafica3.CheckedChanged
      chkGrafica1.Checked = False
      chkGrafica2.Checked = False
      chkGrafica4.Checked = False

      'containerG1.Visible = False
      Response.Redirect("../Visita/Reporte3.aspx")

   End Sub

   Protected Sub chkGrafica4_CheckedChanged(sender As Object, e As EventArgs) Handles chkGrafica4.CheckedChanged
      chkGrafica1.Checked = False
      chkGrafica2.Checked = False
      chkGrafica3.Checked = False

      'containerG1.Visible = False
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

            listaAreasEstatus.InnerHtml = "<table id='datatable' style='border-collapse: collapse; border: 1px solid;'><thead> <tr><th style='border-collapse: collapse; border: 1px solid;'>Área</th>"
            listaAreasEstatus.InnerHtml += "<th style='border-collapse: collapse; border: 1px solid;'>Vigente</th><th style='border-collapse: collapse; border: 1px solid;'>Con prórroga</th>"
            listaAreasEstatus.InnerHtml += "<th style='border-collapse: collapse; border: 1px solid;'>Proceso emplazamiento</th>"
            listaAreasEstatus.InnerHtml += "<th style='border-collapse: collapse; border: 1px solid;'>Emplazada</th><th style='border-collapse: collapse; border: 1px solid;'>Proceso Sanción</th>"
            listaAreasEstatus.InnerHtml += "<th style='border-collapse: collapse; border: 1px solid;'>Sancionada</th><th style='border-collapse: collapse; border: 1px solid;'>En espera de acción jurídica</th>"
            listaAreasEstatus.InnerHtml += "<th style='border-collapse: collapse; border: 1px solid;'>Revocación</th><th style='border-collapse: collapse; border: 1px solid;'>Juicio Nulidad</th>"
            listaAreasEstatus.InnerHtml += "<th style='border-collapse: collapse; border: 1px solid;'>Cancelada</th><th style='border-collapse: collapse; border: 1px solid;'>Visita Cerrada</th></tr></thead><tbody style='text-align:center;'>"

            grafica = AccesoBD.getDatosGraficaAreaEstatus(2, fechaInicio, fechaFin, 36) 'VF
            If Not grafica Is Nothing Then
               'For Each fila As DataRow In dtVF.Rows
               listaAreasEstatus.InnerHtml += "<tr><th>" + grafica.AreaVisita.ToString() + "</th>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalVigentes.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalConProrroga.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalProcEmplaza.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalEmplazadas.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalProcSan.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalSancionadas.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalEspAccJurid.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalRevoca.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalJuicioNul.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalCanceladas.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalCerradas.ToString() + "</td></tr>"
               'Next
            End If

            grafica = AccesoBD.getDatosGraficaAreaEstatus(2, fechaInicio, fechaFin, 35) 'VO
            If Not grafica Is Nothing Then
               'For Each fila2 As DataRow In dtVO.Rows
               listaAreasEstatus.InnerHtml += "<tr><th>" + grafica.AreaVisita.ToString() + "</th>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalVigentes.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalConProrroga.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalProcEmplaza.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalEmplazadas.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalProcSan.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalSancionadas.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalEspAccJurid.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalRevoca.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalJuicioNul.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalCanceladas.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalCerradas.ToString() + "</td></tr>"
               'Next
            End If

            grafica = AccesoBD.getDatosGraficaAreaEstatus(2, fechaInicio, fechaFin, 1) 'CGIV
            If Not grafica Is Nothing Then
               'For Each fila3 As DataRow In dtCGIV.Rows
               listaAreasEstatus.InnerHtml += "<tr><th>" + grafica.AreaVisita.ToString() + "</th>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalVigentes.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalConProrroga.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalProcEmplaza.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalEmplazadas.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalProcSan.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalSancionadas.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalEspAccJurid.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalRevoca.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalJuicioNul.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalCanceladas.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalCerradas.ToString() + "</td></tr>"
               'Next
            End If

            grafica = AccesoBD.getDatosGraficaAreaEstatus(2, fechaInicio, fechaFin, 42) 'PLD
            'If dtPLD.Rows.Count > 0 Then
            If Not grafica Is Nothing Then
               'For Each fila4 As DataRow In dtPLD.Rows
               listaAreasEstatus.InnerHtml += "<tr><th>" + grafica.AreaVisita.ToString() + "</th>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalVigentes.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalConProrroga.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalProcEmplaza.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalEmplazadas.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalProcSan.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalSancionadas.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalEspAccJurid.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalRevoca.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalJuicioNul.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalCanceladas.ToString() + "</td>"
               listaAreasEstatus.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalCerradas.ToString() + "</td></tr>"
               'Next
            End If

            listaAreasEstatus.InnerHtml += "</tbody></table>"

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