Imports System.Web.Configuration
Imports System.Web

Public Class NotificacionesOPI
    Inherits Notificar

    Private Property objUsuario As Entities.Usuario
    Private Property psComentariosUsuario As String
    Private Property destinatarios As New List(Of String)
    Private Property nombreDestinatarios As New List(Of String)
    Public Property Usuario As String
    Public Property Folio As Integer


    ''' <summary>
    ''' Obtiene los correos de los destinatarios
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getDestinatariosCorreo() As String
        Dim lsDestinatarios As String = ""

        For Each lsDest As String In destinatarios
            lsDestinatarios &= lsDest & ";"
        Next

        Return lsDestinatarios
    End Function
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
    Public Function NotificarCorreoOPI(IdCorreo As Integer, sComentarios As String) As String
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

                If IdCorreo = 118 Then
                    AgregarUsuariosVJ()
                End If

                'personalizar asunto y cuerpo del correo
                If Not IsNothing(destinatarios) And destinatarios.Count > 0 Then
                    Dim asunto As String = objCorreoBD.Asunto
                    Dim body As String = objCorreoBD.Cuerpo

                    ReemplazarInformacionCorreo(body, asunto, sComentarios)

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
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "DetalleOPI.aspx.vb, NotificarCorreoPorArea", "")
            mensaje = "Ocurrió un error al Notificar Correo"
        End Try

        Return mensaje

    End Function

    Private Sub ReemplazarInformacionCorreo(ByRef body As String, ByRef asunto As String, sComentarios As String)
        Dim _opiDetalle As New OPI_Incumplimiento
        Dim _opiFunc As New Registro_OPI
        _opiDetalle = _opiFunc.GetOPIDetail(Folio)


        Dim supervisores As String = String.Empty
        Dim inspectores As String = String.Empty
        Dim T_Entidad As String = String.Empty


        T_Entidad = ConexionSICOD.ObtenerNombreEntidadporTipoEntidad(_opiDetalle.I_ID_TIPO_ENTIDAD, _opiDetalle.I_ID_ENTIDAD)

        If _opiDetalle.T_ID_SUPERVISORES.Count > 0 Then
            For Each supervisor In _opiDetalle.T_ID_SUPERVISORES.Keys
                supervisores += supervisor.ToString() + "<br />"
            Next
        End If

        If _opiDetalle.T_ID_INSPECTORES.Count > 0 Then
            For Each inspector In _opiDetalle.T_ID_INSPECTORES.Keys
                inspectores += inspector.ToString() + "<br />"
            Next
        End If





        asunto = asunto.Replace("[FOLIO SuperviSAR]", _opiDetalle.T_ID_FOLIO)

        body = body.Replace("[FOLIO SuperviSAR]", _opiDetalle.T_ID_FOLIO)
        body = body.Replace("[ENTIDAD]", T_Entidad)
        body = body.Replace("[Comentarios agregados]", sComentarios)
        body = body.Replace("[CLASIFICACION_OPI]", _opiDetalle.T_DSC_CLASIFICACION)
        body = body.Replace("[FOLIO_NUEVO_OPI]", String.Empty)
        If Not _opiDetalle.F_FECH_REUNION Is Nothing Then
            body = body.Replace("[FECHA_ESTIMADA_REUNION_VJ]", _opiDetalle.F_FECH_REUNION.Value.ToString("dd/MM/yyyy"))
        End If
        'body = body.Replace("[FECHA_ESTIMADA_REUNION_VJ]", _opiDetalle.F_FECH_REUNION.Value.ToString("dd/MM/yyyy"))
        body = body.Replace("[FOLIO_SISAN]", _opiDetalle.T_ID_FOLIO_SISAN)


    End Sub

    Private Sub AgregarUsuariosReales()
        Dim _opiDetalle As New OPI_Incumplimiento
        Dim _opiFunc As New Registro_OPI
        'Dim destinatarios As New List(Of Persona) '''se comenta ya que en vsiita no se vuelve a declarar
        ''''Dim datosUsuario As New Dictionary(Of String, String)
        Dim supervisores As String = String.Empty
        Dim inspectores As String = String.Empty

        _opiDetalle = _opiFunc.GetOPIDetail(Folio)

        'If _opiDetalle.T_ID_SUPERVISORES.Count > 0 Then
        '    For Each supervisor In _opiDetalle.T_ID_SUPERVISORES.Keys
        '        datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(supervisor.ToString())
        '        destinatarios.Add(New Persona With {.Nombre = supervisor.ToString, .Correo = datosUsuario.Item("mail")})
        '    Next
        'End If
        AgregarPorLista(AccesoBD.getSupervisoresAsignadosVIG(Folio, 1))

        AgregarPorLista(AccesoBD.getInspectoresAsignadosVIG(Folio, 1))

        'If _opiDetalle.T_ID_INSPECTORES.Count > 0 Then
        '    For Each inspector In _opiDetalle.T_ID_INSPECTORES.Keys
        '        'datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(inspector.ToString())
        '        'destinatarios.Add(New Persona With {.Nombre = inspector.ToString, .Correo = datosUsuario.Item("mail")})
        '    Next
        'End If
        'ammm agregado 19092019
        'Igual envía correo al usuario en sesión
        'AgregaUsuarioDestinatario(objUsuario.IdentificadorUsuario)
        'se comenta por que manda error 

        'AgregarPorLista(destinatarios)

        '''Agrega los usuarios que vienen por parametro
        'If Not IsNothing(destinatarios) Then
        '    If destinatarios.Count > 0 Then
        '        AgregarPorLista(destinatarios)
        '    End If
        'End If
    End Sub

    'ammm 1992019  -----  lo pase arriba
    'Public Sub AgregaUsuarioDestinatario(psIdUsuario As String)
    '    Dim datosUsuario As New Dictionary(Of String, String)

    '    If Not IsNothing(psIdUsuario) Then
    '        If psIdUsuario.Trim().Length > 1 Then
    '            If Not nombreDestinatarios.Contains(psIdUsuario) Then
    '                Dim objUsuario As New Entities.Usuario(psIdUsuario)

    '                If Not IsNothing(objUsuario) Then
    '                    If objUsuario.Mail <> "" Then
    '                        destinatarios.Add(objUsuario.Mail)
    '                        nombreDestinatarios.Add(psIdUsuario)
    '                    Else
    '                        datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(psIdUsuario)

    '                        If datosUsuario.Count > 0 Then
    '                            destinatarios.Add(datosUsuario.Item("mail"))
    '                            nombreDestinatarios.Add(psIdUsuario)
    '                        End If
    '                    End If
    '                Else
    '                    datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(psIdUsuario)

    '                    If datosUsuario.Count > 0 Then
    '                        destinatarios.Add(datosUsuario.Item("mail"))
    '                        nombreDestinatarios.Add(psIdUsuario)
    '                    End If
    '                End If
    '            End If
    '        End If
    '    End If
    'End Sub


    Private Sub AgregarPorLista(lstPersonasDestinatarios As List(Of Persona))
        For Each objPersona As Persona In lstPersonasDestinatarios
            If objPersona.Correo <> "" And Not destinatarios.Contains(objPersona.Correo) Then
                destinatarios.Add(objPersona.Correo)
                nombreDestinatarios.Add(objPersona.Nombre)
            End If
        Next
    End Sub

    'se pasa arriba
    'Private Sub AgregarUsuariosPrueba()
    '    destinatarios.Add(WebConfigurationManager.AppSettings("CorreoPruebaUno").ToString())
    '    destinatarios.Add(WebConfigurationManager.AppSettings("CorreoPruebaDos").ToString())
    '    destinatarios.Add(WebConfigurationManager.AppSettings("CorreoPruebaTres").ToString())

    '    nombreDestinatarios.Add("Desarrollo1")
    '    nombreDestinatarios.Add("Desarrollo2")
    '    nombreDestinatarios.Add("Desarrollo3")
    'End Sub

    Private Sub AgregarUsuariosVJ()

        destinatarios.Add(WebConfigurationManager.AppSettings("CorreoVJVigilancia").ToString())
        nombreDestinatarios.Add("ContactoVJ-Vigilancia")

    End Sub

    'AMMM  este metodo se comenta para posteriormente completarlo que se asignane los usuarios de VJ por el perfil de administrador
    'Public Shared Function ObtenerusuariosVJ() As DataSet
    '    Dim conexion As New Conexion.SQLServer()
    '    Dim data As DataSet
    '    Dim strQuery As String
    '    'obtiene los que tengan perfil de administrador y que sean del área de la VJ
    '    strQuery = " Select [T_ID_USUARIO]   FROM [BD_SQL_SEPRIS_QA2].[dbo].[BDS_R_GR_USUARIO_PERFIL]  where N_ID_PERFIL=1 and I_ID_AREA=37 and N_FLAG_VIG=1"

    '    data = conexion.ConsultarDS(strQuery)


    '    conexion.CerrarConexion()

    '    Return data

    'End Function
End Class
