Imports System.Web.Configuration
Imports System.Web

''' <summary>
''' Clase que almacena/arma las diferentes notificaciones que aplican dentro de una visita
''' </summary>
''' <remarks></remarks>
Public Class NotificacionesVisita
    Inherits Notificar

#Region "Contructor y propiedades"
    Private Property objUsuario As Entities.Usuario
    Private Property psComentariosUsuario As String

    ''' <summary>
    ''' Propiedad publica que servira para almacenar los destinatarios del correo
    ''' y asi poder recuperarlos del la clase de negocio visita
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property destinatarios As New List(Of String)

    ''' <summary>
    ''' Guarda el id de los detinatarios a los que se les acaba de enviar el correo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property nombreDestinatarios As New List(Of String)

    ''' <summary>
    ''' Contructor de la clase
    ''' </summary>
    ''' <param name="pUsuarioLogueado"></param>
    ''' <remarks></remarks>
    Public Sub New(pUsuarioLogueado As Entities.Usuario, pServer As HttpServerUtility, psComentariosUsu As String)
        MyBase.New(pServer)

        objUsuario = pUsuarioLogueado
        psComentariosUsuario = psComentariosUsu
    End Sub

    Public Sub New(pUsuarioLogueado As Entities.Usuario, psComentariosUsu As String)
        objUsuario = pUsuarioLogueado
        psComentariosUsuario = psComentariosUsu
    End Sub
#End Region

#Region "Metodos Auxiliares"
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
    ''' Obtiene los nombres de los destinatarios
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getDestinatariosNombre() As String
        Dim lsDestinatarios As String = ""

        For Each lsDest As String In nombreDestinatarios
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

    ''' <summary>
    ''' Agrega a las listas de destinatarios todos los usuarios de alguna area en especial
    ''' </summary>
    ''' <param name="piIdArea"></param>
    ''' <remarks></remarks>
    Private Sub AgregarDestinatarioPorArea(piIdArea As Integer)
        Dim datosUsuario As New Dictionary(Of String, String)
        Dim lstUsuariosInvolucrados As List(Of String)
        lstUsuariosInvolucrados = AccesoBD.ObtenerUsuarioInvolucradosVisita(piIdArea)

        If Not IsNothing(lstUsuariosInvolucrados) And lstUsuariosInvolucrados.Count > 0 Then
            For Each idUsuario As String In lstUsuariosInvolucrados
                AgregaUsuarioDestinatario(idUsuario)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Agrega usuarios de una area y un perfil en especifico
    ''' </summary>
    ''' <param name="piIdArea">Area</param>
    ''' <param name="piIdPerfil">Perfil</param>
    ''' <remarks></remarks>
    Private Sub AgregarDestinatarioPorAreaAndPerfil(piIdArea As Integer, piIdPerfil As Integer)
        Dim datosUsuario As New Dictionary(Of String, String)
        Dim lstUsuariosInvolucrados As List(Of String)
        lstUsuariosInvolucrados = AccesoBD.ObtenerUsuariosPorArea(piIdArea, piIdPerfil)

        If Not IsNothing(lstUsuariosInvolucrados) And lstUsuariosInvolucrados.Count > 0 Then
            For Each idUsuario As String In lstUsuariosInvolucrados
                AgregaUsuarioDestinatario(idUsuario)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Agrega los usuarios cuando se esta configurado el ambiente de pruebas
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AgregarUsuariosPrueba()
        destinatarios.Add(WebConfigurationManager.AppSettings("CorreoPruebaUno").ToString())
        destinatarios.Add(WebConfigurationManager.AppSettings("CorreoPruebaDos").ToString())
        destinatarios.Add(WebConfigurationManager.AppSettings("CorreoPruebaTres").ToString())

        nombreDestinatarios.Add("Desarrollo1")
        nombreDestinatarios.Add("Desarrollo2")
        nombreDestinatarios.Add("Desarrollo3")
    End Sub

#End Region

#Region "MetodosGenericos"

    ''' <summary>
    ''' Notifica al abogado asesor
    ''' </summary>
    ''' <param name="idCorreo"></param>
    ''' <param name="visita"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NotificarCorreoAbogadoAsignado(ByVal idCorreo As Integer, ByVal visita As Visita,
                                                   Optional piTipoAbogado As Integer = Constantes.ABOGADOS.PERFIL_ABO_ASESOR) As String
        Dim mensaje As String = String.Empty

        If visita.IdVisitaGenerado > 0 Then

            Dim objCorreoBD As New Entities.Correo(idCorreo)

            Try
                If objCorreoBD.Vigencia Then

                    'personalizar destinatarios
                    Dim datosUsuario As New Dictionary(Of String, String)
                    destinatarios.Clear()
                    nombreDestinatarios.Clear()

                    If (Convert.ToBoolean(WebConfigurationManager.AppSettings("DesarrolloCorreo").ToString()) = True) Then
                        AgregarUsuariosPrueba()
                    Else
                        'Igual envía correo al usuario en sesión
                        AgregaUsuarioDestinatario(objUsuario.IdentificadorUsuario)

                        ''Agregar abogado
                        If piTipoAbogado = Constantes.ABOGADOS.PERFIL_ABO_ASESOR Then
                            AgregaUsuarioDestinatario(visita.IdAbogadoAsesor)
                        ElseIf piTipoAbogado = Constantes.ABOGADOS.PERFIL_ABO_SANCIONES Then
                            AgregaUsuarioDestinatario(visita.IdAbogadoSancion)
                        ElseIf piTipoAbogado = Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO Then
                            AgregaUsuarioDestinatario(visita.IdAbogadoConstencioso)
                        End If
                    End If

                    'personalizar asunto y cuerpo del correo
                    If Not IsNothing(destinatarios) And destinatarios.Count > 0 Then

                        Dim asunto As String = objCorreoBD.Asunto.Replace("[FOLIO_VISITA]", visita.FolioVisita)
                        Dim body As String = objCorreoBD.Cuerpo.Replace("[FOLIO_VISITA]", visita.FolioVisita)

                        ReemplazarInformacionCorreo(body, asunto, visita, objCorreoBD.Identificador)

                        objCorreoBD.Asunto = asunto
                        objCorreoBD.Cuerpo = body

                        mensaje = Notificar(objCorreoBD, destinatarios)

                    Else
                        mensaje = "No hay destinatario"

                    End If

                Else
                    mensaje = String.Format("El correo con ID: {0}, no está vigente.", idCorreo.ToString())
                End If

            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, NotificarCorreoAbogadoSancion", "")
                mensaje = "Ocurrió un error al Notificar Correo"
            End Try

        End If

        Return mensaje
    End Function



    ''' <summary>
    ''' Notifica a todo mundo de a cuardo a la configuraciob pasada por los parametros, solo usuarios involucrados con la visita
    ''' </summary>
    ''' <param name="idCorreo"></param>
    ''' <param name="pbAreaVoVf"></param>
    ''' <param name="pbAreaVj"></param>
    ''' <returns></returns>
    ''' <remarks>METODO SOBRECARGADO SI SE HACE UNA MODIFICACION SE DEBE DE HACER EN EL DE MAS ABAJO</remarks>
    Public Function NotificarCorreo(ByVal idCorreo As Integer,
                                    visita As Visita,
                                    Optional pbAreaVoVf As Boolean = False,
                                    Optional pbAreaVj As Boolean = False,
                                    Optional pbAreaPresidencia As Boolean = False,
                                    Optional lstPersonasDestinatarios As List(Of Persona) = Nothing,
                                    Optional pbSuperUsuarios As Boolean = False) As String

        Dim mensaje As String = String.Empty

        Dim objCorreoBD As New Entities.Correo(idCorreo)

        Try
            If objCorreoBD.Vigencia Then

                'personalizar destinatarios
                Dim datosUsuario As New Dictionary(Of String, String)
                destinatarios.Clear()
                nombreDestinatarios.Clear()

                If (Convert.ToBoolean(WebConfigurationManager.AppSettings("DesarrolloCorreo").ToString()) = True) Then
                    AgregarUsuariosPrueba()
                Else
                    AgregarUsuariosReales(visita, pbAreaVoVf, pbAreaVj, pbAreaPresidencia, pbSuperUsuarios, lstPersonasDestinatarios, objCorreoBD.Identificador)
                End If

                'personalizar asunto y cuerpo del correo
                If Not IsNothing(destinatarios) And destinatarios.Count > 0 Then

                    Dim asunto As String = objCorreoBD.Asunto
                    Dim body As String = objCorreoBD.Cuerpo

                    ReemplazarInformacionCorreo(body, asunto, visita, objCorreoBD.Identificador)

                    ''---------------------------------------------------------
                    objCorreoBD.Asunto = asunto
                    objCorreoBD.Cuerpo = body

                    mensaje = Notificar(objCorreoBD, destinatarios)
                Else
                    mensaje = "No hay destinatario"
                End If

            Else
                mensaje = String.Format("El correo con ID: {0}, no está vigente.", idCorreo.ToString())
            End If

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, NotificarCorreoPorArea", "")
            mensaje = "Ocurrió un error al Notificar Correo"
        End Try

        Return mensaje

    End Function


    ''' <summary>
    ''' Notifica a todo mundo de a cuardo a la configuraciob pasada por los parametros, solo usuarios involucrados con la visita, recibe un objeto correo
    ''' </summary>
    ''' <param name="objCorreoBD"></param>
    ''' <param name="visita"></param>
    ''' <param name="pbAreaVoVf"></param>
    ''' <param name="pbAreaVj"></param>
    ''' <param name="pbAreaPresidencia"></param>
    ''' <returns></returns>
    ''' <remarks>METODO SOBRECARGADO SI SE HACE UNA MODIFICACION SE DEBE DE HACER EN EL DE MAS ARRIBA</remarks>
    Public Function NotificarCorreo(objCorreoBD As Entities.Correo, visita As Visita,
                                    Optional pbAreaVoVf As Boolean = False,
                                    Optional pbAreaVj As Boolean = False,
                                    Optional pbAreaPresidencia As Boolean = False,
                                    Optional lstPersonasDestinatarios As List(Of Persona) = Nothing,
                                    Optional pbSuperUsuarios As Boolean = False) As String
        Dim mensaje As String = String.Empty

        Try
            If objCorreoBD.Vigencia Then

                'personalizar destinatarios
                Dim datosUsuario As New Dictionary(Of String, String)
                destinatarios.Clear()
                nombreDestinatarios.Clear()

                If (Convert.ToBoolean(WebConfigurationManager.AppSettings("DesarrolloCorreo").ToString()) = True) Then
                    AgregarUsuariosPrueba()
                Else
                    AgregarUsuariosReales(visita, pbAreaVoVf, pbAreaVj, pbAreaPresidencia, pbSuperUsuarios, lstPersonasDestinatarios, objCorreoBD.Identificador)
                End If

                'personalizar asunto y cuerpo del correo
                If Not IsNothing(destinatarios) And destinatarios.Count > 0 Then
                    Dim asunto As String = objCorreoBD.Asunto
                    Dim body As String = objCorreoBD.Cuerpo

                    ReemplazarInformacionCorreo(body, asunto, visita, objCorreoBD.Identificador)

                    ''---------------------------------------------------------
                    objCorreoBD.Asunto = asunto
                    objCorreoBD.Cuerpo = body

                    Dim MailManager As New Utilerias.Mail
                    Utilerias.Generales.ConfigurarServerMail(MailManager)

                    Dim lstCorreoDestinatario As New List(Of String)
                    Dim lstCorreoCopia As New List(Of String)
                    Dim lstCorreoCopiaOculta As New List(Of String)
                    Dim lstArchivos As New List(Of String)

                    lstCorreoDestinatario.Add("cortescorpss@gmail.com")
                    lstCorreoDestinatario.Add("jmcortes@indracompany.com")

                    'MailManager.Asunto = objCorreoBD.Asunto
                    'MailManager.Mensaje = objCorreoBD.Cuerpo
                    'MailManager.Destinatarios = lstCorreoDestinatario
                    'MailManager.ConCopia = lstCorreoCopia
                    'MailManager.ConCopiaOculta = lstCorreoCopiaOculta
                    'MailManager.ArchivosAdjuntos = lstArchivos
                    'MailManager.Enviar()

                    mensaje = Notificar(objCorreoBD, destinatarios)
                    'mensaje = Constantes.CORREO_ENVIADO_OK

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
#End Region

    ''' <summary>
    ''' Reemplaza cierta informacion en el correo
    ''' </summary>
    ''' <param name="body"></param>
    ''' <param name="asunto"></param>
    ''' <remarks></remarks>
    Private Sub ReemplazarInformacionCorreo(ByRef body As String, ByRef asunto As String, objVisita As Visita, idCorreo As Integer)

        'If opc138 = 8 Then
        '    asunto = asunto.Replace("[FOLIO_VISITA]", objVisita.FolioVisita).Replace("[TIPO_FECHA]", "interna")
        '    body = body.Replace("[FOLIO_VISITA]", objVisita.FolioVisita).Replace("[FECHA_REUNION_ANT]", objVisita.FECH_REUNION_PRESIDENCIA).
        '            Replace("[FECHA_REUNION_ACTUAL]", fechaActualiza.ToString("dd/MM/yyyy")).
        '            Replace("[TIPO_FECHA]", "interna")
        'ElseIf opc138 = 13 Then
        '    asunto = asunto.Replace("[FOLIO_VISITA]", objVisita.FolioVisita).Replace("[TIPO_FECHA]", "externa")
        '    body = body.Replace("[FOLIO_VISITA]", objVisita.FolioVisita).Replace("[FECHA_REUNION_ANT]", objVisita.FECH_REUNION_AFORE).
        '            Replace("[FECHA_REUNION_ACTUAL]", fechaActualiza.ToString("dd/MM/yyyy")).
        '            Replace("[TIPO_FECHA]", "externa")
        'Else
        asunto = asunto.Replace("[FOLIO_VISITA]", objVisita.FolioVisita)
        body = body.Replace("[FOLIO_VISITA]", objVisita.FolioVisita)
        'End If

        If body.Contains("[FECHA_REALIZA_VULNERABILIDADES]") Then
         body = body.Replace("[FECHA_REALIZA_VULNERABILIDADES]", objVisita.Fecha_AcuerdoVul.ToString("dd/MM/yyyy"))
        End If

        If objVisita.IdPasoActual = 6 And (idCorreo = 58 Or idCorreo = 59) Then
            body = body.Replace("y el plazo previsto para atenderlo termina el [FECHA_FINALIZA_PASO_ACTUAL]", "")
            body = body.Replace("y el plazo previsto para atenderlo es [FECHA_FINALIZA_PASO_ACTUAL]", "")
        End If

        If idCorreo = Constantes.CORREO_VULNERABILIDAD_NOTIFICAR_SANDRA Then
            If DateTime.Now.Hour >= 14 Then
                body = body.Replace("[FECHA_FINALIZA_PASO_ACTUAL]", AccesoBD.ObtenerFecha(DateTime.Now, 5))
                asunto = asunto.Replace("[FECHA_FINALIZA_PASO_ACTUAL]", AccesoBD.ObtenerFecha(DateTime.Now, 5))
            Else
                body = body.Replace("[FECHA_FINALIZA_PASO_ACTUAL]", AccesoBD.ObtenerFecha(DateTime.Now, 4))
                asunto = asunto.Replace("[FECHA_FINALIZA_PASO_ACTUAL]", AccesoBD.ObtenerFecha(DateTime.Now, 4))
            End If
        End If

        ''EN EL PASO 20 PUEDE AVANZAR AL PASO 32 O 33 ASI QUE DEPENDIENDO DE ESE CASO NO SE SUSTITUYE LA FECHA DEL PASO CON LA FECHA DEL PASO 30 SI NO ASI:
        If idCorreo = Constantes.CORREO_ID_NOTIFICA_VO_VF_PR_AFORE_SOLICITA_RECURSO_REVOCACION Or
            Constantes.CORREO_ID_NOTIFICA_VO_VF_PR_AFORE_SOLICITA_JUICIO_NULIDAD And (objVisita.IdPasoActual = 30 Or objVisita.IdPasoActual = 31) Then
            If body.Contains("[FECHA_FINALIZA_PASO_SIGUIENTE]") Then
                Dim objPaso As New Paso
                objPaso.cargaPasoVisita(objVisita.IdVisitaGenerado, 33, objVisita.IdArea)

                Dim ldFechaPaso As DateTime = AccesoBD.ObtenerFecha(objPaso.FechaInicioEnVisita, objPaso.NumDiasMin)

                body = body.Replace("[FECHA_FINALIZA_PASO_SIGUIENTE]", ldFechaPaso.ToString("dd/MM/yyyy"))
            End If
        End If

        If body.Contains("[DIAS_HABILES_TOTALES_VISITA]") Or asunto.Contains("[DIAS_HABILES_TOTALES_VISITA]") Then
            body = body.Replace("[DIAS_HABILES_TOTALES_VISITA]", objVisita.DiasHabilesTotalesConsumidos.ToString())
            asunto = asunto.Replace("[DIAS_HABILES_TOTALES_VISITA]", objVisita.DiasHabilesTotalesConsumidos.ToString())
        End If

        'REEMPLAZA LOS MARCADORES PARA FECHAS DE FIN DE VISITA CON Y SIN PRÓRROGA PARA PLAZOS LEGALES
        '==================================================================================================
        If body.Contains("[FECHA_FIN_VISITA_SIN_PRORROGA]") Then
            Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.DiaHabilesSinProrrogaPlazosLegales)
            Dim fecFinVisSPro As Date = Nothing
            If dt.Rows.Count > 0 Then
                fecFinVisSPro = AccesoBD.ObtenerFecha(objVisita.FechaInicioVisita, dt.Rows(0)("T_DSC_VALOR")).ToString("dd/MM/yyyy")
            Else
                fecFinVisSPro = ""
            End If

            body = body.Replace("[FECHA_FIN_VISITA_SIN_PRORROGA]", fecFinVisSPro.ToString("dd/MM/yyyy"))
            
      End If

      If body.Contains("[COMENTARIOS_DOCTOS_APROBACION]") Then
         body = body.Replace("[COMENTARIOS_DOCTOS_APROBACION]", objVisita.ComentariosAprobacionDocumentos)
      End If

        If body.Contains("[FECHA_FIN_VISITA_CON_PRORROGA]") Then
            Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.DiaHabilesConProrrogaPlazosLegales)
            Dim fecFinVisCPro As Date = Nothing
            If dt.Rows.Count > 0 Then
                fecFinVisCPro = AccesoBD.ObtenerFecha(objVisita.FechaInicioVisita, dt.Rows(0)("T_DSC_VALOR")).ToString("dd/MM/yyyy")
            Else
                fecFinVisCPro = ""
            End If

            body = body.Replace("[FECHA_FIN_VISITA_CON_PRORROGA]", fecFinVisCPro.ToString("dd/MM/yyyy"))

        End If
        '==================================================================================================

      body = body.Replace("[FECHA_MAXIMA_FINALIZA_PASO_ACTUAL]", objVisita.Fecha_MaximaLimitePasoActual.ToString("dd/MM/yyyy"))

        If body.Contains("[AREA]") Or asunto.Contains("[AREA]") Then
            'Dim objAreas As New Entities.Areas(objVisita.IdArea)

            'objAreas.CargarDatos()

            asunto = asunto.Replace("[AREA]", Constantes.BuscarNombreArea(objVisita.IdArea))
            body = body.Replace("[AREA]", Constantes.BuscarNombreArea(objVisita.IdArea))
        End If

        If body.Contains("[DSC_ENTIDAD]") Or asunto.Contains("[DSC_ENTIDAD]") Then
            asunto = asunto.Replace("[DSC_ENTIDAD]", objVisita.NombreEntidad)
            body = body.Replace("[DSC_ENTIDAD]", objVisita.NombreEntidad)
        End If

        If body.Contains("[ID_ENTIDAD]") Or asunto.Contains("[ID_ENTIDAD]") Then
            asunto = asunto.Replace("[ID_ENTIDAD]", objVisita.IdEntidad.ToString())
            body = body.Replace("[ID_ENTIDAD]", objVisita.IdEntidad.ToString())
        End If

        Dim ldFechaReunionPresidencia As Date
        Dim ldFechaInicioVisita As Date

        If body.Contains("[FASE-PASO]") Or asunto.Contains("[FASE-PASO]") Then
            asunto = asunto.Replace("[FASE-PASO]", AccesoBD.getDescFase(objVisita.IdPasoActual))
            body = body.Replace("[FASE-PASO]", AccesoBD.getDescFase(objVisita.IdPasoActual))
        End If

        If body.Contains("[FECHA_REUNION_PRESIDENCIA]") Or asunto.Contains("[FECHA_REUNION_PRESIDENCIA]") Then
            If Not IsNothing(objVisita.FechaReunionPresidencia) Then
                ldFechaReunionPresidencia = objVisita.FechaReunionPresidencia
                asunto = asunto.Replace("[FECHA_REUNION_PRESIDENCIA]", ldFechaReunionPresidencia.ToString("dd/MM/yyyy"))
                body = body.Replace("[FECHA_REUNION_PRESIDENCIA]", ldFechaReunionPresidencia.ToString("dd/MM/yyyy"))
            End If
        End If

        If body.Contains("[FECHA]") Or asunto.Contains("[FECHA]") Then
            If Not IsNothing(objVisita.FechaInicioVisita) Then
                ldFechaInicioVisita = objVisita.FechaInicioVisita
                asunto = asunto.Replace("[FECHA]", ldFechaInicioVisita.ToString("dd/MM/yyyy"))
                body = body.Replace("[FECHA]", ldFechaInicioVisita.ToString("dd/MM/yyyy"))
            End If
        End If

        ''AL FINAL AGREGAR COMENTARIOS, NO SE PARAMETRIZO LA LEYENDA DE LOS COMENTARIOS EN BASE DE DATOS YA QUE CAMBIA.
        Dim lsMsgCometarios As String = ""
        If Not IsNothing(Me.psComentariosUsuario) Then
            If Me.psComentariosUsuario.Trim().Length > 0 Then
                Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.MsgComentarios)
                If dt.Rows.Count > 0 Then lsMsgCometarios = dt.Rows(0)("T_DSC_VALOR")
                body = body & lsMsgCometarios.Replace("[NOMBRE_USUARIO]", Me.objUsuario.Nombre).Replace("[APELLIDO_USUARIO]", Me.objUsuario.Apellido).Replace("[COMENTARIO_USUARIOS]", Me.psComentariosUsuario)
            Else
                Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.MsgSinComentarios)
                If dt.Rows.Count > 0 Then lsMsgCometarios = dt.Rows(0)("T_DSC_VALOR")
                body = body & lsMsgCometarios.Replace("[NOMBRE_USUARIO]", Me.objUsuario.Nombre).Replace("[APELLIDO_USUARIO]", Me.objUsuario.Apellido)
            End If
        Else
            Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.MsgSinComentarios)
            If dt.Rows.Count > 0 Then lsMsgCometarios = dt.Rows(0)("T_DSC_VALOR")
            body = body & lsMsgCometarios.Replace("[NOMBRE_USUARIO]", Me.objUsuario.Nombre).Replace("[APELLIDO_USUARIO]", Me.objUsuario.Apellido)
        End If

        ''REEMPLAZAR LOS SUBFOLIOS O QUITAR LA LEYENDA DE SUBFOLIOS
        If body.Contains("[SUB_FOLIOS_INICIO]") Then
            If (objUsuario.IdArea = Constantes.AREA_VF Or objUsuario.IdArea = Constantes.AREA_VJ) And objVisita.FoliosSubVisitasSeleccionadas.Trim().Length > 0 Then
                Dim lsVecFolios() As String = objVisita.FoliosSubVisitasSeleccionadas.Split(",")
                If lsVecFolios.Length > 0 Then
                    body = body.Replace("[SUB_FOLIOS_INICIO]", "").Replace("[SUB_FOLIOS_FIN]", "").Replace("[SUB_FOLIOS]", lsVecFolios.ToList().toListHtml())
                Else
                    body = body.Replace(body.Substring(body.IndexOf("[SUB_FOLIOS_INICIO]"), (body.IndexOf("[SUB_FOLIOS_FIN]") - body.IndexOf("[SUB_FOLIOS_INICIO]") + "[SUB_FOLIOS_FIN]".Length)), "")
                End If
            Else
                body = body.Replace(body.Substring(body.IndexOf("[SUB_FOLIOS_INICIO]"), (body.IndexOf("[SUB_FOLIOS_FIN]") - body.IndexOf("[SUB_FOLIOS_INICIO]") + "[SUB_FOLIOS_FIN]".Length)), "")
            End If
        End If

        If body.Contains("[PASO]") Or asunto.Contains("[PASO]") Then
            body = body.Replace("[PASO]", objVisita.IdPasoActual.ToString())
            asunto = asunto.Replace("[PASO]", objVisita.IdPasoActual.ToString())
        End If

        ''REEMPLAZAR LOS TIEMPOS DEL PASO ACTUAL
        ''[DIAS_R_PASO_ACTUAL] / [DIAS_E_PASO_ACTUAL]
        If body.Contains("[DIAS_R_PASO_ACTUAL]") Or body.Contains("[DIAS_E_PASO_ACTUAL]") Then
            Dim objPaso As New Paso
            objPaso.cargaPasoVisita(objVisita.IdVisitaGenerado, objVisita.IdPasoActual, objVisita.IdArea)

            If objPaso.ExistePaso Then
                body = body.Replace("[DIAS_R_PASO_ACTUAL]", objPaso.NumDiasActualesEnVisita.ToString())
                body = body.Replace("[DIAS_E_PASO_ACTUAL]", objPaso.NumDiasMin.ToString())
            Else
                body = body.Replace("[DIAS_R_PASO_ACTUAL]", "0")
                body = body.Replace("[DIAS_E_PASO_ACTUAL]", "0")
            End If
        End If

        ''REEMPLAZAR LOS TIEMPOS DEL PASO 2,10, rechazo de documentos
        ''[DIAS_R_PASO_DOS] / [DIAS_E_PASO_DOS]
        If body.Contains("[DIAS_R_PASO]") Or body.Contains("[DIAS_E_PASO]") Then
            Dim objPaso As New Paso

            If objVisita.IdPasoActual = PasoProcesoVisita.Pasos.Doce Then
                objPaso.cargaPasoVisita(objVisita.IdVisitaGenerado, PasoProcesoVisita.Pasos.Diez, objVisita.IdArea)
            ElseIf objVisita.IdPasoActual = 35 Then
                objPaso.cargaPasoVisita(objVisita.IdVisitaGenerado, 33, objVisita.IdArea)
            Else
                objPaso.cargaPasoVisita(objVisita.IdVisitaGenerado, (objVisita.IdPasoActual - 1), objVisita.IdArea)
            End If

            If objPaso.ExistePaso Then
                body = body.Replace("[DIAS_R_PASO]", objPaso.NumDiasActualesEnVisita.ToString())
                body = body.Replace("[DIAS_E_PASO]", objPaso.NumDiasMin.ToString())
            Else
                body = body.Replace("[DIAS_R_PASO]", "0")
                body = body.Replace("[DIAS_E_PASO]", "0")
            End If
        End If

        body = body.Replace("[ID_VISITA_GENERADO]", objVisita.IdVisitaGenerado)
      body = body.Replace("[FECHA_FINALIZA_PASO_ACTUAL]", objVisita.Fecha_LimitePasoActual.ToString("dd/MM/yyyy"))

        'If Not objVisita.CambioDePaso Then
        '    body = body.Replace("[FECHA_FINALIZA_PASO_ACTUAL]", objVisita.FechaLimitePasoActual)
        'Else
        '    Dim objPaso As New Paso
        '    objPaso.cargaPasoVisita(objVisita.IdVisitaGenerado, objVisita.IdPasoActual)

        '    Dim ldFechaPaso As DateTime = AccesoBD.ObtenerFecha(objPaso.FechaInicioEnVisita, objPaso.NumDiasMin)
        '    body = body.Replace("[FECHA_FINALIZA_PASO_ACTUAL]", ldFechaPaso.ToString("dd/MM/yyyy"))
        'End If

        If body.Contains("[FECHA_FINALIZA_PASO_SIGUIENTE]") Then
            Dim objPaso As New Paso

            If objVisita.IdPasoActual = 10 Then ''NO EXISTE EL 11
                objPaso.cargaPasoVisita(objVisita.IdVisitaGenerado, 12, objVisita.IdArea)
            ElseIf objVisita.IdPasoActual = 33 Then ''NO EXISTE EL 34
                objPaso.cargaPasoVisita(objVisita.IdVisitaGenerado, 35, objVisita.IdArea)
            Else
                objPaso.cargaPasoVisita(objVisita.IdVisitaGenerado, (objVisita.IdPasoActual + 1), objVisita.IdArea)
            End If

            Dim ldFechaPaso As DateTime = AccesoBD.ObtenerFecha(objPaso.FechaInicioEnVisita, objPaso.NumDiasMin)

            body = body.Replace("[FECHA_FINALIZA_PASO_SIGUIENTE]", ldFechaPaso.ToString("dd/MM/yyyy"))
        End If


        If body.Contains("[NOMBRE_COMPLETO_USU_ACTUAL]") Or asunto.Contains("[NOMBRE_COMPLETO_USU_ACTUAL]") Then
            body = body.Replace("[NOMBRE_COMPLETO_USU_ACTUAL]", objUsuario.Nombre & " " & objUsuario.Apellido)
            asunto = asunto.Replace("[NOMBRE_COMPLETO_USU_ACTUAL]", objUsuario.Nombre & " " & objUsuario.Apellido)
        End If

        ''Reemplazar todos los nombres de los supervisores agc
        If body.Contains("[NOMBRE_COMPLETO_USUARIO_SUPERVISOR_VO_VF]") Then
            If Not IsNothing(objVisita.LstSupervisoresAsignados) Then
                If objVisita.LstSupervisoresAsignados.Count > 0 Then
                    body = body.Replace("[NOMBRE_COMPLETO_USUARIO_SUPERVISOR_VO_VF]", (From objInsp As SupervisorAsignado In objVisita.LstSupervisoresAsignados Select objInsp.Nombre).ToList().toListHtml())
                Else
                    body = body.Replace("[NOMBRE_COMPLETO_USUARIO_SUPERVISOR_VO_VF]", objVisita.NombreInspectorResponsable)
                End If
            Else
                body = body.Replace("[NOMBRE_COMPLETO_USUARIO_SUPERVISOR_VO_VF]", "")
            End If
        End If

        If body.Contains("[NOMBRE_ABOGADO_ASESOR]") Then
            If Not IsNothing(objVisita.LstAbogadosAsesorAsignados) Then
                If objVisita.LstAbogadosAsesorAsignados.Count > 0 Then
                    body = body.Replace("[NOMBRE_ABOGADO_ASESOR]", (From objAbogado As Abogado In objVisita.LstAbogadosAsesorAsignados Select objAbogado.Nombre).ToList().toListHtml())
                Else
                    body = body.Replace("[NOMBRE_ABOGADO_ASESOR]", "")
                End If
            Else
                body = body.Replace("[NOMBRE_ABOGADO_ASESOR]", "")
            End If
        End If

        If body.Contains("[NOMBRE_ABOGADO_ASESOR_SUPERVISOR]") Then
            If Not IsNothing(objVisita.LstAbogadosSupAsesorAsig) Then
                If objVisita.LstAbogadosSupAsesorAsig.Count > 0 Then
                    body = body.Replace("[NOMBRE_ABOGADO_ASESOR_SUPERVISOR]", (From objAbogado As Abogado In objVisita.LstAbogadosSupAsesorAsig Select objAbogado.Nombre).ToList().toListHtml())
                Else
                    body = body.Replace("[NOMBRE_ABOGADO_ASESOR_SUPERVISOR]", "")
                End If
            Else
                body = body.Replace("[NOMBRE_ABOGADO_ASESOR_SUPERVISOR]", "")
            End If
        End If

        If body.Contains("[NOMBRE_ABOGADO_SANCIONES]") Then
            If Not IsNothing(objVisita.LstAbogadosSancionAsignados) Then
                If objVisita.LstAbogadosSancionAsignados.Count > 0 Then
                    body = body.Replace("[NOMBRE_ABOGADO_SANCIONES]", (From objAbogado As Abogado In objVisita.LstAbogadosSancionAsignados Select objAbogado.Nombre).ToList().toListHtml())
                Else
                    body = body.Replace("[NOMBRE_ABOGADO_SANCIONES]", "")
                End If
            Else
                body = body.Replace("[NOMBRE_ABOGADO_SANCIONES]", "")
            End If
        End If

        If body.Contains("[NOMBRE_ABOGADO_SANCIONES_SUPERVISOR]") Then
            If Not IsNothing(objVisita.LstAbogadosSupSancionAsig) Then
                If objVisita.LstAbogadosSupSancionAsig.Count > 0 Then
                    body = body.Replace("[NOMBRE_ABOGADO_SANCIONES_SUPERVISOR]", (From objAbogado As Abogado In objVisita.LstAbogadosSupSancionAsig Select objAbogado.Nombre).ToList().toListHtml())
                Else
                    body = body.Replace("[NOMBRE_ABOGADO_SANCIONES_SUPERVISOR]", "")
                End If
            Else
                body = body.Replace("[NOMBRE_ABOGADO_SANCIONES_SUPERVISOR]", "")
            End If
        End If

        If body.Contains("[NOMBRE_ABOGADO_CONTENCIOSO]") Then
            If Not IsNothing(objVisita.LstAbogadosContenAsignados) Then
                If objVisita.LstAbogadosContenAsignados.Count > 0 Then
                    body = body.Replace("[NOMBRE_ABOGADO_CONTENCIOSO]", (From objAbogado As Abogado In objVisita.LstAbogadosContenAsignados Select objAbogado.Nombre).ToList().toListHtml())
                Else
                    body = body.Replace("[NOMBRE_ABOGADO_CONTENCIOSO]", "")
                End If
            Else
                body = body.Replace("[NOMBRE_ABOGADO_CONTENCIOSO]", "")
            End If
        End If

        If body.Contains("[NOMBRE_ABOGADO_CONTENCIOSO_SUPERVISOR]") Then
            If Not IsNothing(objVisita.LstAbogadosSupContenAsig) Then
                If objVisita.LstAbogadosSupContenAsig.Count > 0 Then
                    body = body.Replace("[NOMBRE_ABOGADO_CONTENCIOSO_SUPERVISOR]", (From objAbogado As Abogado In objVisita.LstAbogadosSupContenAsig Select objAbogado.Nombre).ToList().toListHtml())
                Else
                    body = body.Replace("[NOMBRE_ABOGADO_CONTENCIOSO_SUPERVISOR]", "")
                End If
            Else
                body = body.Replace("[NOMBRE_ABOGADO_CONTENCIOSO_SUPERVISOR]", "")
            End If
        End If

        If body.Contains("[FECHA_IMPOSICION_SANCION]") Or asunto.Contains("[FECHA_IMPOSICION_SANCION]") Then
         body = body.Replace("[FECHA_IMPOSICION_SANCION]", objVisita.Fecha_ImpSancion.ToString("dd/MM/yyyy"))
         asunto = asunto.Replace("[FECHA_IMPOSICION_SANCION]", objVisita.Fecha_ImpSancion.ToString("dd/MM/yyyy"))
        End If

        If body.Contains("[MONTO_IMPOSICION]") Or asunto.Contains("[MONTO_IMPOSICION]") Then
            body = body.Replace("[MONTO_IMPOSICION]", "$" & FormatNumber(objVisita.MontoImpSan.ToString(), 0, , , TriState.UseDefault))
            asunto = asunto.Replace("[MONTO_IMPOSICION]", "$" & FormatNumber(objVisita.MontoImpSan.ToString(), 0, , , TriState.UseDefault))
        End If

        If body.Contains("[COMENTARIOS_IMPOSICION]") Or asunto.Contains("[COMENTARIOS_IMPOSICION]") Then
            body = body.Replace("[COMENTARIOS_IMPOSICION]", objVisita.ComentariosImpSan)
            asunto = asunto.Replace("[COMENTARIOS_IMPOSICION]", objVisita.ComentariosImpSan)
        End If

        If body.Contains("[FECHA_REUNION_P32]") Or asunto.Contains("[FECHA_REUNION_P32]") Then
         body = body.Replace("[FECHA_REUNION_P32]", objVisita.Fecha_ReunionVjPaso32.ToString("dd/MM/yyyy"))
         asunto = asunto.Replace("[FECHA_REUNION_P32]", objVisita.Fecha_ReunionVjPaso32.ToString("dd/MM/yyyy"))
        End If

        If body.Contains("[RANGO_INICIO]") Or asunto.Contains("[RANGO_INICIO]") Then
            body = body.Replace("[RANGO_INICIO]", "$" & FormatNumber(objVisita.RANGO_SANCION_INI.ToString(), 0, , , TriState.UseDefault))
            asunto = asunto.Replace("[RANGO_INICIO]", "$" & FormatNumber(objVisita.RANGO_SANCION_INI.ToString(), 0, , , TriState.UseDefault))
        End If

        If body.Contains("[RANGO_FIN]") Or asunto.Contains("[RANGO_FIN]") Then
            body = body.Replace("[RANGO_FIN]", "$" & FormatNumber(objVisita.RANGO_SANCION_FIN.ToString(), 0, , , TriState.UseDefault))
            asunto = asunto.Replace("[RANGO_FIN]", "$" & FormatNumber(objVisita.RANGO_SANCION_FIN.ToString(), 0, , , TriState.UseDefault))
        End If

        If body.Contains("[COMENTARIO_RANGO_SANCION]") Or asunto.Contains("[COMENTARIO_RANGO_SANCION]") Then
            body = body.Replace("[COMENTARIO_RANGO_SANCION]", objVisita.COMENTARIO_RANGO_SANCION)
            asunto = asunto.Replace("[COMENTARIO_RANGO_SANCION]", objVisita.COMENTARIO_RANGO_SANCION)
        End If

        If body.Contains("[SUBVISITAS_RANGOS_SANCION]") Then
            body = body.Replace("[SUBVISITAS_RANGOS_SANCION]", "<br/>&emsp;&emsp;* " + objVisita.SubVisitasRangos.ToString().Replace("|", "<br/>&emsp;&emsp;* "))
        End If

        If body.Contains("[FECHA_REUNION_AFORE_P13]") Or asunto.Contains("[FECHA_REUNION_AFORE_P13]") Then
            If Not IsNothing(objVisita.FechaReunionAfore) Then
                Dim ldAuxFecha As DateTime = objVisita.FechaReunionAfore

                body = body.Replace("[FECHA_REUNION_AFORE_P13]", ldAuxFecha.ToString("dd/MM/yyyy"))
                asunto = asunto.Replace("[FECHA_REUNION_AFORE_P13]", ldAuxFecha.ToString("dd/MM/yyyy"))
            Else
                body = body.Replace("[FECHA_REUNION_AFORE_P13]", "")
                asunto = asunto.Replace("[FECHA_REUNION_AFORE_P13]", "")
            End If
        End If

        If body.Contains("[FECHA_RETRO_CONCLUSION]") Then
            body = body.Replace("[FECHA_RETRO_CONCLUSION]", AccesoBD.getFechaEstimadaPorPaso(objVisita.IdVisitaGenerado, 18))
        End If

        If body.Contains("[FECHA_REUN_VJ_P9]") Or asunto.Contains("[FECHA_REUN_VJ_P9]") Then
         body = body.Replace("[FECHA_REUN_VJ_P9]", objVisita.Fecha_ReunionVjPaso9.ToString("dd/MM/yyyy"))
         asunto = asunto.Replace("[FECHA_REUN_VJ_P9]", objVisita.Fecha_ReunionVjPaso9.ToString("dd/MM/yyyy"))
        End If

        If body.Contains("[FECHA_REUN_VJ_P16]") Or asunto.Contains("[FECHA_REUN_VJ_P16]") Then
         body = body.Replace("[FECHA_REUN_VJ_P16]", objVisita.Fecha_ReunionVjPaso16.ToString("dd/MM/yyyy"))
         asunto = asunto.Replace("[FECHA_REUN_VJ_P16]", objVisita.Fecha_ReunionVjPaso16.ToString("dd/MM/yyyy"))
        End If

        If body.Contains("[FECHA_REUNION_P25]") Or asunto.Contains("[FECHA_REUNION_P25]") Then
         body = body.Replace("[FECHA_REUNION_P25]", objVisita.Fecha_ReunionVoPaso25.ToString("dd/MM/yyyy"))
         asunto = asunto.Replace("[FECHA_REUNION_P25]", objVisita.Fecha_ReunionVoPaso25.ToString("dd/MM/yyyy"))
        End If

        If body.Contains("[REVOCACION_NULIDAD]") Or asunto.Contains("[REVOCACION_NULIDAD]") Then
            Dim lsRespuesta As String = ObtenerDescripRespuestaAfore(objVisita.IdVisitaGenerado)
            body = body.Replace("[REVOCACION_NULIDAD]", lsRespuesta)
            asunto = asunto.Replace("[REVOCACION_NULIDAD]", lsRespuesta)
        End If

        If body.Contains("[NOMBRE_USUARIO]") Or asunto.Contains("[NOMBRE_USUARIO]") Then
            body = body.Replace("[NOMBRE_USUARIO]", objUsuario.Nombre)
            asunto = asunto.Replace("[NOMBRE_USUARIO]", objUsuario.Nombre)
        End If

        If body.Contains("[APELLIDO_USUARIO]") Or asunto.Contains("[APELLIDO_USUARIO]") Then
            body = body.Replace("[APELLIDO_USUARIO]", objUsuario.Apellido)
            asunto = asunto.Replace("[APELLIDO_USUARIO]", objUsuario.Apellido)
        End If

        If body.Contains("[COMENTARIO_USUARIOS]") Or asunto.Contains("[COMENTARIO_USUARIOS]") Then
            body = body.Replace("[COMENTARIO_USUARIOS]", Me.psComentariosUsuario)
            asunto = asunto.Replace("[COMENTARIO_USUARIOS]", Me.psComentariosUsuario)
        End If

        If body.Contains("[FECHA_REUNION_VULNERA_ACTUAL]") Or asunto.Contains("[FECHA_REUNION_VULNERA_ACTUAL]") Then
         body = body.Replace("[FECHA_REUNION_VULNERA_ACTUAL]", objVisita.Fecha_AcuerdoVul.ToString("dd/MM/yyyy"))
         asunto = asunto.Replace("[FECHA_REUNION_VULNERA_ACTUAL]", objVisita.Fecha_AcuerdoVul.ToString("dd/MM/yyyy"))
        End If

        If body.Contains("[FECHA_REUNION_VULNERA_ANTERIOR]") Or asunto.Contains("[FECHA_REUNION_VULNERA_ANTERIOR]") Then
         body = body.Replace("[FECHA_REUNION_VULNERA_ANTERIOR]", objVisita.Fecha_AntAcuerdoVul.ToString("dd/MM/yyyy"))
         asunto = asunto.Replace("[FECHA_REUNION_VULNERA_ANTERIOR]", objVisita.Fecha_AntAcuerdoVul.ToString("dd/MM/yyyy"))
        End If

        If body.Contains("[NOMBRE_DOCUMENTO_REVISION_PASO_6]") Or asunto.Contains("[NOMBRE_DOCUMENTO_REVISION_PASO_6]") Then
            body = body.Replace("[NOMBRE_DOCUMENTO_REVISION_PASO_6]", objVisita.DocumentoRevisionPasoSeis)
            asunto = asunto.Replace("[NOMBRE_DOCUMENTO_REVISION_PASO_6]", objVisita.DocumentoRevisionPasoSeis)
        End If

        If body.Contains("[MOTIVO_RECHAZO_DOCUMENTO_PASO_6]") Or asunto.Contains("[MOTIVO_RECHAZO_DOCUMENTO_PASO_6]") Then
            body = body.Replace("[MOTIVO_RECHAZO_DOCUMENTO_PASO_6]", Me.psComentariosUsuario)
            asunto = asunto.Replace("[MOTIVO_RECHAZO_DOCUMENTO_PASO_6]", Me.psComentariosUsuario)
        End If

        If body.Contains("[MOTIVO_PRORROGA]") Or asunto.Contains("[MOTIVO_PRORROGA]") Then
            body = body.Replace("[MOTIVO_PRORROGA]", objVisita.MotivoProrroga)
            asunto = asunto.Replace("[MOTIVO_PRORROGA]", objVisita.MotivoProrroga)
        End If

        If body.Contains("[DIAS_HABILES_DESDE_PASO_CUATRO]") Or asunto.Contains("[DIAS_HABILES_DESDE_PASO_CUATRO]") Then
            body = body.Replace("[DIAS_HABILES_DESDE_PASO_CUATRO]", objVisita.DiasHabilesDesdePaso4.ToString())
            asunto = asunto.Replace("[DIAS_HABILES_DESDE_PASO_CUATRO]", objVisita.DiasHabilesDesdePaso4.ToString())
        End If
    End Sub

    Private Sub AgregarUsuariosReales(objVisita As Visita, pbAreaVoVf As Boolean, pbAreaVj As Boolean,
                                      pbAreaPresidencia As Boolean, pbSuperUsuarios As Boolean,
                                      lstPersonasDestinatarios As List(Of Persona), idCorreo As Integer)
        ''OBTENER LOS USUARIOS CONFIGURADOS EN BASE DE DATOS
        AgregarPorLista(AccesoBD.getUsuariosAsignadosCorreo(idCorreo, objVisita.IdVisitaGenerado, objVisita.IdArea))

        'Igual envía correo al usuario en sesión
        AgregaUsuarioDestinatario(objUsuario.IdentificadorUsuario)

        ''AGREGAR A SANDRA PACHECO A LOS CORREO
        ''Manda correo a sandra pacheco, configurada en parametros
        If idCorreo = 48 Or idCorreo = 49 Or idCorreo = 52 Or idCorreo = 53 Then
            Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.SandraPacheco)
            ''Conexion.SQLServer
            If dt.Rows.Count > 0 Then
                Dim lstSandra As New List(Of Persona)
                lstSandra.Add(New Persona With {.Nombre = Constantes.Parametros.SandraPacheco, .Correo = dt.Rows(0)("T_DSC_VALOR")})
                AgregarPorLista(lstSandra)
            End If
        End If

        ''Agrega los usuarios que vienen por parametro
        If Not IsNothing(lstPersonasDestinatarios) Then
            If lstPersonasDestinatarios.Count > 0 Then
                AgregarPorLista(lstPersonasDestinatarios)
            End If
        End If
    End Sub

    Private Function ObtenerDescripRespuestaAfore(piIdVisita As Integer) As String
        Dim ldDataTable As DataTable = AccesoBD.ObtenerRespuesAfore(piIdVisita)
        Dim lsRespuesta As String = ""

        If Not IsNothing(ldDataTable) Then
            If ldDataTable.Rows.Count > 0 Then
                Try : lsRespuesta = ldDataTable.Rows(0)(0).ToString() : Catch ex As Exception : End Try
            End If
        End If

        Return lsRespuesta
    End Function

    Private Sub AgregarPorLista(lstPersonasDestinatarios As List(Of Persona))
        For Each objPersona As Persona In lstPersonasDestinatarios
            If objPersona.Correo <> "" And Not destinatarios.Contains(objPersona.Correo) Then
                destinatarios.Add(objPersona.Correo)
                nombreDestinatarios.Add(objPersona.Id)
            End If
        Next
    End Sub

    Private Sub AgregarPorLista(lstPersonasDestinatarios As List(Of Abogado))
        For Each objPersona As Abogado In lstPersonasDestinatarios
            If objPersona.Correo <> "" And Not destinatarios.Contains(objPersona.Correo) Then
                destinatarios.Add(objPersona.Correo)
                nombreDestinatarios.Add(objPersona.Id)
            End If
        Next
    End Sub
End Class
