Imports System.Web.Configuration
Imports System.Net
Imports Clases
Imports Entities
Imports Utilerias
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Web.Services.Protocols

Public Class Reporte4
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

      Response.Redirect("../Visita/Reporte2.aspx")

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

            listaTipoVisArea.InnerHtml = "<table id='datatable' style='border-collapse: collapse; border: 1px solid;'><thead> <tr><th style='border-collapse: collapse; border: 1px solid;'>Área</th>"
            listaTipoVisArea.InnerHtml += "<th style='border-collapse: collapse; border: 1px solid;'>Integral</th><th style='border-collapse: collapse; border: 1px solid;'>Seguimiento</th>"
            listaTipoVisArea.InnerHtml += "<th style='border-collapse: collapse; border: 1px solid;'>Especial</th>"
            listaTipoVisArea.InnerHtml += "<th style='border-collapse: collapse; border: 1px solid;'>Orientada a vulnerabilidades</th><th style='border-collapse: collapse; border: 1px solid;'>Ordinaria</th>"
            listaTipoVisArea.InnerHtml += "</tr></thead><tbody style='text-align:center;'>"

            grafica = AccesoBD.getDatosGraficaTipoVisita(4, fechaInicio, fechaFin, 36) 'VF
            If Not grafica Is Nothing Then
               'For Each fila As DataRow In dtVF.Row
               listaTipoVisArea.InnerHtml += "<tr><th>" + grafica.AreaVisita + "</th>"
               listaTipoVisArea.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalIntegrales.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalSeguimientos.ToString() + "</td>"
               listaTipoVisArea.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalEspeciales.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalOrientadas.ToString() + "</td>"
               listaTipoVisArea.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalOrdinarias.ToString() + "</td></tr>"
               'Next
            End If

            grafica = AccesoBD.getDatosGraficaTipoVisita(4, fechaInicio, fechaFin, 35) 'VO            
            If Not grafica Is Nothing Then
               'For Each fila2 As DataRow In dtVO.Rows
               listaTipoVisArea.InnerHtml += "<tr><th>" + grafica.AreaVisita + "</th>"
               listaTipoVisArea.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalIntegrales.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalSeguimientos.ToString() + "</td>"
               listaTipoVisArea.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalEspeciales.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalOrientadas.ToString() + "</td>"
               listaTipoVisArea.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalOrdinarias.ToString() + "</td></tr>"
               'Next
            End If

            grafica = AccesoBD.getDatosGraficaTipoVisita(4, fechaInicio, fechaFin, 1) 'CGIV
            If Not grafica Is Nothing Then
               'For Each fila3 As DataRow In dtCGIV.Rows
               listaTipoVisArea.InnerHtml += "<tr><th>" + grafica.AreaVisita + "</th>"
               listaTipoVisArea.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalIntegrales.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalSeguimientos.ToString() + "</td>"
               listaTipoVisArea.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalEspeciales.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalOrientadas.ToString() + "</td>"
               listaTipoVisArea.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalOrdinarias.ToString() + "</td></tr>"
               'Next
            End If

            grafica = AccesoBD.getDatosGraficaTipoVisita(4, fechaInicio, fechaFin, 42) 'PLD
            If Not grafica Is Nothing Then
               'For Each fila4 As DataRow In dtPLD.Rows
               listaTipoVisArea.InnerHtml += "<tr><th>" + grafica.AreaVisita + "</th>"
               listaTipoVisArea.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalIntegrales.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalSeguimientos.ToString() + "</td>"
               listaTipoVisArea.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalEspeciales.ToString() + "</td><td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalOrientadas.ToString() + "</td>"
               listaTipoVisArea.InnerHtml += "<td style='border-collapse: collapse; border: 1px solid;'>" + grafica.TotalOrdinarias.ToString() + "</td></tr>"
               'Next
            End If

            listaTipoVisArea.InnerHtml += "</tbody></table>"

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