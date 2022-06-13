Imports System.Web.Configuration
Imports System.Web

Public Class NotificacionesPC
    Inherits Notificar

    Private Property objUsuario As Entities.Usuario
    Private Property psComentariosUsuario As String
    Private Property destinatarios As New List(Of String)
    Private Property nombreDestinatarios As New List(Of String)
    Public Property Usuario As String
    Public Property Folio As Integer
    ''' <summary>
    ''' Agrega algun usuario a los destinatarios del correo
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AgregaUsuarioDestinatario(psIdUsuario As String)
        Dim datosUsuario As New Dictionary(Of String, String)

        If Not IsNothing(psIdUsuario) Then
            If psIdUsuario.Trim().Length > 1 Then
                If Not nombreDestinatarios.Contains(psIdUsuario) Then
                    Dim objUsuario As New Entities.Usuario(psIdUsuario)

                    If Not IsNothing(objUsuario) Then
                        If objUsuario.Mail <> "" Then
                            destinatarios.Add(objUsuario.Mail)
                            nombreDestinatarios.Add(psIdUsuario)
                        Else
                            datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(psIdUsuario)

                            If datosUsuario.Count > 0 Then
                                destinatarios.Add(datosUsuario.Item("mail"))
                                nombreDestinatarios.Add(psIdUsuario)
                            End If
                        End If
                    Else
                        datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(psIdUsuario)

                        If datosUsuario.Count > 0 Then
                            destinatarios.Add(datosUsuario.Item("mail"))
                            nombreDestinatarios.Add(psIdUsuario)
                        End If
                    End If
                End If
            End If
        End If
    End Sub


    Private Sub AgregarUsuariosPrueba()
        destinatarios.Add(WebConfigurationManager.AppSettings("CorreoPruebaUno").ToString())
        destinatarios.Add(WebConfigurationManager.AppSettings("CorreoPruebaDos").ToString())
        destinatarios.Add(WebConfigurationManager.AppSettings("CorreoPruebaTres").ToString())

        nombreDestinatarios.Add("Desarrollo1")
        nombreDestinatarios.Add("Desarrollo2")
        nombreDestinatarios.Add("Desarrollo3")
    End Sub


    Public Function NotificarCorreo(IdCorreo As Integer) As String
        Dim mensaje As String = String.Empty
        Dim objCorreoBD As New Entities.Correo(IdCorreo)

        Try
            If objCorreoBD.Vigencia Then

                'personalizar destinatarios
                Dim datosUsuario As New Dictionary(Of String, String)
                destinatarios.Clear()
                nombreDestinatarios.Clear()

                If (Convert.ToBoolean(WebConfigurationManager.AppSettings("DesarrolloCorreo").ToString()) = True) Then
                    AgregarUsuariosPrueba()
                Else
                    AgregarUsuariosReales()
                End If

                'personalizar asunto y cuerpo del correo
                If Not IsNothing(destinatarios) And destinatarios.Count > 0 Then
                    Dim asunto As String = objCorreoBD.Asunto
                    Dim body As String = objCorreoBD.Cuerpo

                    ReemplazarInformacionCorreo(body, asunto)

                    ''---------------------------------------------------------
                    objCorreoBD.Asunto = asunto
                    objCorreoBD.Cuerpo = body

                    Dim MailManager As New Utilerias.Mail
                    Utilerias.Generales.ConfigurarServerMail(MailManager)

                    Dim lstCorreoDestinatario As New List(Of String)
                    Dim lstCorreoCopia As New List(Of String)
                    Dim lstCorreoCopiaOculta As New List(Of String)
                    Dim lstArchivos As New List(Of String)


                    mensaje = Notificar(objCorreoBD, destinatarios)


                Else
                    mensaje = "No hay destinatario"

                End If

            Else
                mensaje = String.Format("El correo con ID: {0}, no está vigente.", objCorreoBD.Identificador)
            End If

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, NotificarCorreoPorArea", "")
            mensaje = "Ocurrió un error al Notificar Correo"
        End Try

        Return mensaje

    End Function

    Private Sub ReemplazarInformacionCorreo(ByRef body As String, ByRef asunto As String)

        Dim PC As New Entities.PC(Folio)
        Dim supervisores As String = String.Empty
        Dim inspectores As String = String.Empty

        For Each supervisor As KeyValuePair(Of String, String) In PC.Supervisores
            supervisores += supervisor.Value + "<br />"
        Next

        For Each inspector As KeyValuePair(Of String, String) In PC.Inspectores
            inspectores += inspector.Value + "<br />"
        Next

        asunto = asunto.Replace("[FOLIO SuperviSAR]", PC.FolioSupervisar)


        body = body.Replace("[Folio SuperviSAR]", PC.FolioSupervisar)
        body = body.Replace("[FOLIO SuperviSAR]", PC.FolioSupervisar)
        body = body.Replace("[Nombre Supervisores]", supervisores)
        body = body.Replace("[Comentarios agregados]", String.Empty)
        body = body.Replace("[Supervisor Acepta]", Usuario)
        body = body.Replace("[Supervisor Rechaza]", Usuario)
        body = body.Replace("[Supervisor Asigna]", Usuario)
        body = body.Replace("[Nombre Inspectores]", inspectores)
        body = body.Replace("[ENTIDAD]", PC.FolioSupervisar.Split("/")(3))
        body = body.Replace("[AREA]", If(PC.IdArea = 35, "Vicepresidencia Operativa", If(PC.IdArea = 36, "Vicepresidencia Financiera", "")))
        body = body.Replace("[RESOLUCIÓN_PC]", If(Convert.ToInt32(PC.IdResolucion) = 1, "No Procede", If(Convert.ToInt32(PC.IdResolucion) = 2, "Procede", If(Convert.ToInt32(PC.IdResolucion) = 3, "Procede Con Plazo", If(Convert.ToInt32(PC.IdResolucion) = 4, "No Presentado", "")))))
        body = body.Replace("[FOLIO_SISAN]", PC.FolioSisan)
        body = body.Replace("[Fecha de imposición]", String.Empty)
        body = body.Replace("[Monto_Sancion]", String.Empty)
        body = body.Replace("[Comentarios_Sancion]", String.Empty)
        body = body.Replace("[Fecha_PAGO]", String.Empty)

    End Sub

    Private Sub AgregarUsuariosReales()

        Dim PC As New Entities.PC(Folio)
        Dim destinatarios As New List(Of Persona)
        'Dim datosUsuario As New Dictionary(Of String, String)
        Dim supervisores As String = String.Empty
        Dim inspectores As String = String.Empty

        'For Each supervisor As KeyValuePair(Of String, String) In PC.Supervisores
        '    datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(supervisor.Key)
        '    destinatarios.Add(New Persona With {.Nombre = supervisor.Value, .Correo = datosUsuario.Item("mail")})
        'Next

        'For Each inspector As KeyValuePair(Of String, String) In PC.Inspectores
        '    datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(inspector.Key)
        '    destinatarios.Add(New Persona With {.Nombre = inspector.Value, .Correo = datosUsuario.Item("mail")})
        'Next

        AgregarPorLista(AccesoBD.getSupervisoresAsignadosVIG(Folio, 2))

        AgregarPorLista(AccesoBD.getInspectoresAsignadosVIG(Folio, 2))

    End Sub



    Private Sub AgregarPorLista(lstPersonasDestinatarios As List(Of Persona))
        For Each objPersona As Persona In lstPersonasDestinatarios
            If objPersona.Correo <> "" And Not destinatarios.Contains(objPersona.Correo) Then
                destinatarios.Add(objPersona.Correo)
                nombreDestinatarios.Add(objPersona.Id)
            End If
        Next
    End Sub


End Class
