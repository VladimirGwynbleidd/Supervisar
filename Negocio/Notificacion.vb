'- Fecha de creación: 01/08/2014
'- Fecha de modificación:  ##/##/####
'- Nombre del Responsable: Rafael Rodriguez
'- Empresa: Softtek
'- Clase para reasignar servicios

Imports Entities
Imports System.Web.Configuration
Imports Utilerias

Public Class Notificacion

#Region "Reagignacion"

    Public Function CorreoNotificacionReasignacion(ByVal idCorreo As Integer, ByVal idSol As Integer, ByVal idNivel As Integer, ByVal idInge As String, Optional ByVal idIngeAnt As String = Nothing) As Boolean
        Dim objSolicitud As New Solicitud(Integer.Parse(idSol))
        Dim continua As Boolean = False
        Dim resultado As Boolean = False
        Dim objNivel As New NivelServicio(idNivel)
        Dim objCorreo As New Entities.Correo(idCorreo)
        Dim objEmail As Utilerias.Mail
        Dim ingeActual As New Usuario(idInge)
        Dim ingeAnterior As New Usuario
        If idIngeAnt <> Nothing Then ingeAnterior = New Usuario(idIngeAnt)
        Dim dtSubdirectores As DataTable = Nothing
        Dim datosUsuario As New Dictionary(Of String, String)
        objEmail = New Utilerias.Mail
        If objCorreo.Vigencia Then
            Dim destinatarios As List(Of String) = New List(Of String)
            If (Convert.ToBoolean(WebConfigurationManager.AppSettings("Desarrollo").ToString()) = True) Then
                destinatarios.Add("david.perez@softtek.com")
                destinatarios.Add("victor.leyva@softtek.com")
                destinatarios.Add("ivan.rivera@softtek.com")
            Else
                datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(objSolicitud.UsuarioSolicitante)
                destinatarios.Add(datosUsuario.Item("mail"))
                destinatarios.Add(ingeActual.Mail)
                If ingeAnterior.Vigente Then destinatarios.Add(ingeAnterior.Mail)
                dtSubdirectores = objSolicitud.DestinatariosSubdirectores()
                If dtSubdirectores.Rows.Count > 0 Then
                    For Each dr As DataRow In dtSubdirectores.Rows
                        If Not destinatarios.Contains(dr("T_ID_SUBDIRECTOR")) Then
                            datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(dr("T_ID_SUBDIRECTOR"))
                            destinatarios.Add(datosUsuario.Item("mail"))
                        End If
                        If Not destinatarios.Contains(dr("T_ID_BACKUP")) Then
                            datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(dr("T_ID_BACKUP"))
                            destinatarios.Add(datosUsuario.Item("mail"))
                        End If
                    Next
                End If
            End If
            Try
                objEmail.ServidorMail = WebConfigurationManager.AppSettings("MailServer").ToString()
                objEmail.Usuario = WebConfigurationManager.AppSettings("MailUsuario").ToString()
                objEmail.Password = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("MailPass"))
                objEmail.Dominio = WebConfigurationManager.AppSettings("MailDominio").ToString()
                objEmail.DireccionRemitente = WebConfigurationManager.AppSettings("MailUsuario").ToString()
                objCorreo.Asunto = objCorreo.Asunto.Replace("[FOLIO]", objSolicitud.IdFolio.ToString + "-" + objSolicitud.FolioAnio.ToString)
                objEmail.Asunto = objCorreo.Asunto
                objEmail.EsHTML = True

                Select Case idCorreo
                    Case 19
                        objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[USUARIO]", ingeActual.Nombre + " " + ingeActual.Apellido + " " + ingeActual.ApellidoAuxiliar)
                        objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[SERVICIO]", objNivel.Descripcion)
                        objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[FOLIO]", objSolicitud.IdFolio.ToString + "-" + objSolicitud.FolioAnio.ToString)
                        objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[F_FECH_ESTIMADA_TERMINO]", CalcularFechaTermino(idSol))
                    Case 20
                        objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[SERVICIO]", objNivel.Descripcion)
                        objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[FOLIO]", objSolicitud.IdFolio.ToString + "-" + objSolicitud.FolioAnio.ToString)
                        objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[F_FECH_ESTIMADA_TERMINO]", CalcularFechaTermino(idSol))
                End Select
                objEmail.Mensaje = objCorreo.Cuerpo
                objEmail.Destinatarios = destinatarios
                objEmail.NombreAplicacion = WebConfigurationManager.AppSettings("EventLogSource").ToString()
                objEmail.EventLogSource = "ENVIAR_EMAIL"
                resultado = objEmail.Enviar()
            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            End Try
        End If
        Return resultado
    End Function

    Public Function CalcularFechaTermino(ByVal idSol As Integer) As String
        Dim objAgenda As New Agenda()
        Dim fE As String = String.Empty
        Dim fP As String = String.Empty
        objAgenda.ObtenerFechasTermino(idSol, fE, fP)
        If fE <> "" Then
            Return fE
        End If
        Return fP
    End Function

#End Region

End Class
