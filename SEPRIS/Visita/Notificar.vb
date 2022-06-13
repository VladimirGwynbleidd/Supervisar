Imports System.Web.Configuration
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Web

''' <summary>
''' Clase publica que se encarga de mandar correos
''' </summary>
''' <remarks></remarks>

Public Class Notificar
    Private Property Server As HttpServerUtility

    Public Sub New()
        Server = Nothing
    End Sub

    Public Sub New(pServer As HttpServerUtility)
        Server = pServer
    End Sub

    ''' <summary>
    ''' Metodo general que manda el correo
    ''' </summary>
    ''' <param name="objCorreoBD"></param>
    ''' <param name="destinatarios"></param>
    ''' <returns></returns>
    ''' <remarks>AGC</remarks>
    Protected Function Notificar(ByVal objCorreoBD As Entities.Correo, ByVal destinatarios As List(Of String)) As String

        Dim mensaje As String = String.Empty

        'Settings del mail.
        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim dom As String = System.Web.Configuration.WebConfigurationManager.AppSettings("MailServer")
        Dim usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("MailUsuario")
        Dim passEnc As String = System.Web.Configuration.WebConfigurationManager.AppSettings("MailPass")
        Dim fromCorreo As String = System.Web.Configuration.WebConfigurationManager.AppSettings("MailCuenta")
        Dim puertoCorreo As Integer = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings("puertoCorreo"))

        Try
            'Configurar correo
            Dim objCorreo As New System.Net.Mail.MailMessage()
            objCorreo.From = New System.Net.Mail.MailAddress(fromCorreo)
            objCorreo.Subject = objCorreoBD.Asunto

            Dim lsRutaImagenes As String = System.Web.Configuration.WebConfigurationManager.AppSettings("rutaAbsolutaImagenesServWindows")

            'Imagen
            Dim encabezado As LinkedResource
            If IsNothing(Server) Then
                encabezado = New LinkedResource(lsRutaImagenes & "/mail_Encabezado.png")
            Else
                encabezado = New LinkedResource(Server.MapPath("~/Imagenes/mail_Encabezado.png"))
            End If

            encabezado.ContentId = "ENCABEZADO"
            Dim logo As LinkedResource

            If IsNothing(Server) Then
                logo = New LinkedResource(lsRutaImagenes & "/mail_logo_consar.png")
            Else
                logo = New LinkedResource(Server.MapPath("~/Imagenes/mail_logo_consar.png"))
            End If

            logo.ContentId = "LOGO"

            Dim Pie As LinkedResource

            If IsNothing(Server) Then
                Pie = New LinkedResource(lsRutaImagenes & "/mail_direcc_arbol.png")
            Else
                Pie = New LinkedResource(Server.MapPath("~/Imagenes/mail_direcc_arbol.png"))
            End If

            Pie.ContentId = "PIE"

            Dim strCuerpoCorreo As String = String.Empty
            'Encabezado correo 
            strCuerpoCorreo += "<html>"
            strCuerpoCorreo += "<body>"
            strCuerpoCorreo += "<table width=""778"">"
            strCuerpoCorreo += "<tr>"
            strCuerpoCorreo += "<td align=""left"" width=""578"">"
            strCuerpoCorreo += "<img src=""cid:ENCABEZADO"" width=""300"" height=""97""/>"
            strCuerpoCorreo += "</td>"
            'strCuerpoCorreo += "<td align=""rigth"" width=""200"">"
            'strCuerpoCorreo += "<img src=""cid:LOGO"" width=""200"" height=""167""/>"
            'strCuerpoCorreo += "</td>"
            strCuerpoCorreo += "</tr>"
            strCuerpoCorreo += "</table>"
            strCuerpoCorreo += "<br><br>"
            'Mensaje correo
            strCuerpoCorreo += "<table width=""778"">"
            strCuerpoCorreo += "<tr><td>"
            strCuerpoCorreo += "<p style=""text-align:justify""> " + objCorreoBD.Cuerpo + "</p>"
            strCuerpoCorreo += "</td></tr>"
            strCuerpoCorreo += "</table>"
            'Pie correo
            strCuerpoCorreo += "<img src=""cid:PIE"" width=""778"" height=""122""/>"
            strCuerpoCorreo += "</body>"
            strCuerpoCorreo += "</html>"


            Dim av1 As AlternateView = AlternateView.CreateAlternateViewFromString(strCuerpoCorreo, Nothing, MediaTypeNames.Text.Html)
            av1.LinkedResources.Add(logo)
            av1.LinkedResources.Add(Pie)
            av1.LinkedResources.Add(encabezado)
            objCorreo.AlternateViews.Add(av1)

            objCorreo.IsBodyHtml = True
            'objCorreo.Body = objCorreoBD.Cuerpo

            'Destinatarios
            For Each emailDestinatario As String In destinatarios
                objCorreo.To.Add(emailDestinatario)
            Next

            'Envio de Correo
            If objCorreo.To.Count > 0 Then
                Try
                    Dim smtpCliente As New System.Net.Mail.SmtpClient(dom, puertoCorreo)
                    smtpCliente.Credentials = New System.Net.NetworkCredential(usuario, passEnc)
                    'NHM CORREO INICA
                    'Dim smtpcliente As New System.Net.Mail.SmtpClient("94.126.240.205")
                    'smtpcliente.Credentials = New System.Net.NetworkCredential("nhernandezma", "contraseña")
                    'objCorreo.From = New System.Net.Mail.MailAddress("nhernandezma@azertia.com.mx")
                    'NHM CORREO FIN
#If Not Debug Then
                    smtpCliente.Send(objCorreo)
#End If
                    mensaje = Constantes.CORREO_ENVIADO_OK

                Catch ex As Exception
                    mensaje = "Error al enviar correo"
                    Utilerias.ControlErrores.EscribirEvento("Envio correo smtpCliente.Send(objCorreo), ERROR: " + ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, Notificar", "")
                End Try
            Else
                mensaje = "No hay destinatario"
            End If
            'Envio de Correo
        Catch ex As Exception
            mensaje = "Error al notificar vía correo electrónico"
            Utilerias.ControlErrores.EscribirEvento("Error al armar el correo: " + ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, Notificar", "")
        End Try

        Return mensaje
    End Function
End Class