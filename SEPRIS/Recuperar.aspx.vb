Imports System.Web
Imports System.Web.Configuration

Public Class Recuperar
    Inherits System.Web.UI.Page


    Protected Mensaje As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("Login.aspx", False)
    End Sub

    Protected Sub btnEnviar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEnviar.Click

        Dim usuario As New Entities.Usuario



        If usuario.EstaRegistrado(txtUsuario.Text, txtCorreo.Text) Then

            usuario.CargarDatos(txtUsuario.Text)

            If usuario.Vigente Then

                If usuario.ActualizarContrasenaAnonimo() Then

                    Dim mail As New Utilerias.Mail()
                    Dim destinatarios As New List(Of String)
                    Dim etiquetas As New Dictionary(Of String, String)

                    etiquetas.Add("<usuario>", usuario.IdentificadorUsuario)
                    etiquetas.Add("<contraseña>", usuario.Contrasena)
                    etiquetas.Add("<correo>", WebConfigurationManager.AppSettings("CorreoAtencion"))

                    Dim template As New Negocio.Correo(1, etiquetas)

                    destinatarios.Add(txtCorreo.Text)

                    If template.EsVigente Then

                        mail.EsHTML = True
                        mail.Asunto = template.Asunto
                        mail.Destinatarios = destinatarios
                        mail.Mensaje = template.Mensaje
                        mail.Enviar()

                        Mensaje = "Su solicitud se ha procesado con éxito. En breve recibirá un correo electrónico con su contraseña."
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Exito", "MostrarMensaje();", True)

                    Else
                        Utilerias.ControlErrores.EscribirEvento("No se realizó envío de mail por vigencia", EventLogEntryType.Warning)
                    End If

                End If

            Else


                Mensaje = "El usuario ha sido dado de baja, por favor contacte al administrador del sistema: AtencionSISTEMA@consar.gob.mx"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Exito", "MostrarMensaje();", True)

            End If

        Else
            Mensaje = "El correo proporcionado no existe en la Base de Datos, por favor contacte al administrador del sistema: AtencionSISTEMA@consar.gob.mx"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Exito", "MostrarMensaje();", True)
        End If



    End Sub
End Class