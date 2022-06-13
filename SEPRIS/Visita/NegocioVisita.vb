''' <summary>
''' Clase con los metodos necesarios para atender las diferentes transacciones/estatus de un visita.
''' </summary>
''' <remarks>AGC</remarks>
Public Class NegocioVisita

#Region "Propiedades"
    ''' <summary>
    ''' Propiedad que almacena la visita que se esta atendiendo.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>agc</remarks>
    Private Property visita As Visita

    ''' <summary>
    ''' Propiedad privada que almacena el usuario logueado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property objUsuario As Entities.Usuario

    ''' <summary>
    ''' Prupiedad que almacena las observaciones de la pantalla visita, si es que las hay.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ppObservaciones As String

    ''' <summary>
    ''' Ayuda a eviar un correo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property objNotifVisita As NotificacionesVisita

#End Region

#Region "Contructor"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    ''' <param name="pvVisita">Visita en cuestion</param>
    ''' <remarks>agc</remarks>
    Public Sub New(pvVisita As Visita, pUsuarioLogueado As Entities.Usuario, pServer As HttpServerUtility, psComentariosUsu As String)
        visita = pvVisita
        objUsuario = pUsuarioLogueado
        ppObservaciones = psComentariosUsu
        objNotifVisita = New NotificacionesVisita(pUsuarioLogueado, pServer, psComentariosUsu)
    End Sub
#End Region

    ''' <summary>
    ''' Regresa el objeto de notificacion
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getObjNotificacion() As NotificacionesVisita
        Return Me.objNotifVisita
    End Function


#Region "MetodosGenericos"


    ''' <summary>
    ''' Metodo generico para modificar estatus/finalizar paso/registrar nuevo/notificar
    ''' </summary>
    ''' <param name="pbRegistraComentario"></param>
    ''' <param name="piEstatusComentario"></param>
    ''' <param name="pbFinalizaPasoActual"></param>
    ''' <param name="piEstatusFinalizacion"></param>
    ''' <param name="pbRegistraPasoSiguiente"></param>
    ''' <param name="piEstatusPasoSiguiente"></param>
    ''' <param name="pbNotificar"></param>
    ''' <param name="piCorreoNotigfica"></param>
    ''' <param name="pbAreaVoVf">Ya no se usa</param>
    ''' <param name="pbAreaVj">Ya no se usa</param>
    ''' <param name="pbAreaPresidencia">Ya no se usa</param>
    ''' <remarks></remarks>
    Public Sub PasoGenerericEstatusPasoNotificarReactivar(ByVal pbRegistraComentario As Boolean,
                                                           ByVal piEstatusComentario As Integer,
                                                           Optional pbFinalizaPasoActual As Boolean = False,
                                                           Optional piEstatusFinalizacion As Integer = -1,
                                                           Optional pbRegistraPasoSiguiente As Boolean = False,
                                                           Optional piPasoSiguiente As Integer = -1,
                                                           Optional piEstatusPasoSiguiente As Integer = -1,
                                                           Optional pbNotificar As Boolean = False,
                                                           Optional piCorreoNotigfica As Integer = -1,
                                                           Optional pbAreaVoVf As Boolean = False,
                                                           Optional pbAreaVj As Boolean = False,
                                                           Optional pbAreaPresidencia As Boolean = False,
                                                           Optional pbReactivaPaso As Boolean = False,
                                                           Optional piTipoReactivacion As Integer = -1,
                                                           Optional piPasoActual As Integer = -1,
                                                           Optional piPasoReactivado As Integer = -1,
                                                           Optional piEstatusReactivacion As Integer = -1,
                                                           Optional pdFechaFinalizacionPaso As DateTime? = Nothing,
                                                           Optional pbNotificarSuperUsuarios As Boolean = False,
                                                           Optional pbAvanceAutomaticoP8 As Boolean = False,
                                                           Optional pbBanderaCambioNormativa As Boolean = False,
                                                           Optional piMovimiento As Integer = -1
                                                         )

        Dim con As Conexion.SQLServer = Nothing
        Dim tran As SqlClient.SqlTransaction = Nothing
        Dim guardo As Boolean = True
        Dim ldFechaAux As DateTime = Nothing

        If IsNothing(pdFechaFinalizacionPaso) Then
            'Validar que la fecha de finalizacion del paso actual
            ldFechaAux = DateTime.Now

            'Si la fecha de hoy es menor a la fecha de inicio del paso que se quiere finalizar entonces tomar la fecha de inicio mas N minutos
            If pbFinalizaPasoActual Then
                Dim ldFechaAuxPaso As DateTime

                If Not IsNothing(visita.FechaInicioPasoActual) Then
                    ldFechaAuxPaso = visita.FechaInicioPasoActual

                    If DateTime.Today.Date < ldFechaAuxPaso.Date Then
                        Dim liAuxMinutos As Integer = 0
                        If Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.MinutosExtraFechaInicio), liAuxMinutos) Then
                            ldFechaAux = ldFechaAuxPaso.AddMinutes(liAuxMinutos)
                        End If
                    End If
                End If
            End If
        Else
            ldFechaAux = pdFechaFinalizacionPaso
        End If

        Try
            con = New Conexion.SQLServer()
            tran = con.BeginTransaction()

            Dim estatusPaso As New EstatusPaso()
            Dim PasosProV As New PasoProcesoVisita()
            ''Registrar el comentario
            If pbRegistraComentario Then
                guardo = False

                estatusPaso.IdAreaActual = objUsuario.IdArea
                estatusPaso.IdVisitaGenerado = visita.IdVisitaGenerado
                estatusPaso.IdPaso = visita.IdPasoActual
                estatusPaso.IdEstatus = piEstatusComentario
                estatusPaso.FechaRegistro = DateTime.Now
                estatusPaso.IdUsuario = objUsuario.IdentificadorUsuario
                estatusPaso.Comentarios = ppObservaciones
                estatusPaso.SubVisitasSeleccionadas = visita.SubVisitasSeleccionadas
                PasosProV.IdMovimiento = piMovimiento


                'DEBEN GUARDARSE LOS COMENTARIOS DE RECHAZO POR DOCUMENTO EN EL CAMPO [T_DSC_COMENTARIO] DE LA TABLA DE [BDS_D_VS_DOCUMENTO_PASO] donde:
                'APROBADO = 5
                'RECHAZADO = 33
                If AccesoBD.registrarEstatusPaso(PasosProV, estatusPaso, con, tran) = True Then
                    guardo = True
                End If
            End If


            'Se finaliza el paso actual
            If guardo Then
                If pbFinalizaPasoActual Then
                    guardo = False ''Reiniciar el estatus

                    estatusPaso = New EstatusPaso()
                    estatusPaso.IdAreaActual = objUsuario.IdArea
                    estatusPaso.IdVisitaGenerado = visita.IdVisitaGenerado
                    estatusPaso.IdPaso = visita.IdPasoActual
                    estatusPaso.IdEstatus = piEstatusFinalizacion
                    estatusPaso.FechaRegistro = DateTime.Now
                    estatusPaso.IdUsuario = objUsuario.IdentificadorUsuario ' se obtiene de la session
                    estatusPaso.Comentarios = "Finaliza paso " & visita.IdPasoActual.ToString()
                    estatusPaso.TipoComentario = Constantes.TipoComentario.SISTEMA
                    estatusPaso.SubVisitasSeleccionadas = visita.SubVisitasSeleccionadas
                    PasosProV.IdMovimiento = piMovimiento

                    If AccesoBD.registrarEstatusPaso(PasosProV, estatusPaso, con, tran) = True Then
                        If AccesoBD.finalizarPaso(visita.IdVisitaGenerado, visita.IdPasoActual, con, tran, ldFechaAux, visita.SubVisitasSeleccionadas) = True Then
                            ''ACTUALIZAR EL ESTATUS DE LA VISITA
                            If Not pbRegistraPasoSiguiente Then ''ACTUALIZA EL ESTATUS SOLO SI NO VA  REGISTRAR UN PASO NUEVO, SI NO SE ACTUALIZA DESPUES DE REGISTRARLO
                                If AccesoBD.ActualizaEstatusVisita(con, tran, visita.IdVisitaGenerado) Then
                                    guardo = True
                                End If
                            Else
                                guardo = True
                            End If
                        End If
                    End If
                End If
            End If


            'Registra el paso siguiente
            If guardo Then
                If pbRegistraPasoSiguiente Then
                    guardo = False ''Reiniciar el estatus

                    Dim Paso As New PasoProcesoVisita()

                    Paso.IdVisitaGenerado = visita.IdVisitaGenerado
                    Paso.IdPaso = piPasoSiguiente 'Paso siguiente
                    If IsNothing(ldFechaAux) Then
                        Paso.FechaInicio = DateTime.Now
                    Else
                        Paso.FechaInicio = ldFechaAux
                    End If
                    Paso.FechaFin = Nothing
                    Paso.EsNotificado = False
                    Paso.IdAreaNotificada = Constantes.AREA_SN
                    Paso.IdUsuarioNotificado = Nothing
                    Paso.EmailUsuarioNotificado = Nothing
                    Paso.TieneProrroga = False
                    Paso.FechaNotifica = Nothing
                    Paso.SubVisitasSeleccionadas = visita.SubVisitasSeleccionadas
                    Paso.IdMovimiento = piMovimiento


                    If AccesoBD.registrarPaso(Paso, con, tran) = True Then
                        estatusPaso = New EstatusPaso()
                        estatusPaso.IdAreaActual = objUsuario.IdArea
                        estatusPaso.IdVisitaGenerado = visita.IdVisitaGenerado
                        estatusPaso.IdPaso = piPasoSiguiente 'Paso siguiente
                        estatusPaso.IdEstatus = piEstatusPasoSiguiente
                        estatusPaso.FechaRegistro = DateTime.Now
                        estatusPaso.IdUsuario = objUsuario.IdentificadorUsuario
                        estatusPaso.Comentarios = "Finaliza el paso " & visita.IdPasoActual.ToString() &
                                            " e inicia el paso " & piPasoSiguiente.ToString() & " automáticamente."
                        estatusPaso.TipoComentario = Constantes.TipoComentario.SISTEMA
                        estatusPaso.SubVisitasSeleccionadas = visita.SubVisitasSeleccionadas
                        Paso.IdMovimiento = piMovimiento

                        If AccesoBD.registrarEstatusPaso(Paso, estatusPaso, con, tran) Then
                            ''ACTUALIZAR EL ESTATUS DE LA VISITA
                            If AccesoBD.ActualizaEstatusVisita(con, tran, visita.IdVisitaGenerado) Then
                                guardo = True

                                'Actualiza paso 2 en BSIS_X_C_PASO_SUPERVISAR_VISITA
                                If visita.VisitaSisvig Then
                                    Dim visitaSisvig As New Entities.Sisvig() 'MCS 
                                    visitaSisvig.NotificaSisvig(visita.IdVisitaGenerado, piPasoSiguiente, piEstatusPasoSiguiente, ppObservaciones)
                                End If

                            End If
                        End If
                    End If
                End If
            End If

            'GUARDA LA BANDERA DE CAMBIO NORMATIVO SI ES QUE SE REACTIVA EL PASO 1 DESDE EL PASO 3
            If pbBanderaCambioNormativa Then
                AccesoBD.ActualizaBanderaCambioNormativa(con, tran, visita.IdVisitaGenerado)
            End If


            ''Valida si se reactiva paso
            If guardo Then
                If pbReactivaPaso Then
                    guardo = False

                    estatusPaso = New EstatusPaso()
                    estatusPaso.IdAreaActual = objUsuario.IdArea
                    estatusPaso.IdVisitaGenerado = visita.IdVisitaGenerado
                    estatusPaso.FechaRegistro = DateTime.Now
                    estatusPaso.IdUsuario = objUsuario.IdentificadorUsuario
                    estatusPaso.TipoComentario = Constantes.TipoComentario.SISTEMA
                    estatusPaso.IdEstatus = piEstatusReactivacion
                    estatusPaso.SubVisitasSeleccionadas = visita.SubVisitasSeleccionadas

                    If piTipoReactivacion = Constantes.TipoReactivacion.Reactivado Then
                        estatusPaso.IdPaso = piPasoReactivado
                        Dim fechaRegVisita As Date = CDate(visita.FechaRegistro.ToString())
                        Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

                        If piPasoActual = 3 And (Convert.ToDateTime(fechaRegVisita).ToString("yyyy/MM/dd") >= Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
                            estatusPaso.Comentarios = "Paso " & piPasoReactivado.ToString() & " reactivado por cambio en la Normatividad."
                        Else
                            estatusPaso.Comentarios = "Paso " & piPasoReactivado.ToString() & " reactivado por rechazo de documentos."
                        End If
                    Else
                        estatusPaso.IdPaso = piPasoActual
                        estatusPaso.Comentarios = "Paso " & piPasoActual.ToString() & " retomado por revisión de documentos."
                    End If

                    If AccesoBD.registrarEstatusPaso(PasosProV, estatusPaso, con, tran) = True Then
                        If AccesoBD.SaveUpdateReactivacionPaso(visita.IdVisitaGenerado, piPasoActual, piPasoReactivado, piTipoReactivacion, con, tran, visita.SubVisitasSeleccionadas, objUsuario.IdentificadorUsuario, objUsuario.IdArea) = True Then
                            guardo = True

                            'Actualiza paso 2 en BSIS_X_C_PASO_SUPERVISAR_VISITA
                            If visita.VisitaSisvig Then
                                Dim visitaSisvig As New Entities.Sisvig() 'MCS 

                                If piTipoReactivacion = Constantes.TipoReactivacion.Reactivado Then
                                    visitaSisvig.NotificaSisvig(visita.IdVisitaGenerado, piPasoReactivado, piEstatusReactivacion, ppObservaciones, -5)
                                Else
                                    visitaSisvig.NotificaSisvig(visita.IdVisitaGenerado, piPasoActual, piEstatusReactivacion, ppObservaciones, -5)
                                End If
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            'Registro fallido
            guardo = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, imgSiguiente_Click", "")
        Finally
            If Not IsNothing(tran) Then
                If guardo Then
                    'Registro exitoso
                    tran.Commit()

                    If pbNotificar Then
                        If Constantes.CORREO_ENVIADO_OK = objNotifVisita.NotificarCorreo(piCorreoNotigfica, visita, pbAreaVoVf, pbAreaVj, pbAreaPresidencia, Nothing, pbNotificarSuperUsuarios) Then
                            AccesoBD.actualizarPasoNotificadoSinTransaccion(visita.IdVisitaGenerado, visita.IdPasoActual, True, visita.Usuario.IdArea, objNotifVisita.getDestinatariosNombre(), objNotifVisita.getDestinatariosCorreo(), DateTime.Now)
                        End If
                    End If

                    If pbAvanceAutomaticoP8 Then
                        AccesoBD.actualizarPasoNotificadoSinTransaccion(visita.IdVisitaGenerado, visita.IdPasoActual, True, visita.Usuario.IdArea, objNotifVisita.getDestinatariosNombre(), objNotifVisita.getDestinatariosCorreo(), DateTime.Now)
                    End If
                Else
                    'Registro fallido
                    tran.Rollback()
                End If
                tran.Dispose()
            End If

            If Not IsNothing(con) Then
                con.CerrarConexion()
                con = Nothing
            End If
        End Try
    End Sub


    ''' <summary>
    ''' Metodo generico para modificar estatus/finalizar paso/registrar nuevo/notificar, recibe un objeto correo
    ''' </summary>
    ''' <param name="pbRegistraComentario"></param>
    ''' <param name="piEstatusComentario"></param>
    ''' <param name="pbFinalizaPasoActual"></param>
    ''' <param name="piEstatusFinalizacion"></param>
    ''' <param name="pbRegistraPasoSiguiente"></param>
    ''' <param name="piEstatusPasoSiguiente"></param>
    ''' <param name="pbNotificar"></param>
    ''' <param name="objCorreoBD"></param>
    ''' <param name="pbAreaVoVf">Ya no se usa</param>
    ''' <param name="pbAreaVj">Ya no se usa</param>
    ''' <param name="pbAreaPresidencia">Ya no se usa</param>
    ''' <remarks></remarks>
    Public Sub PasoGenerericEstatusPasoNotificarReactivar(ByVal pbRegistraComentario As Boolean,
                                                            ByVal piEstatusComentario As Integer,
                                                            Optional pbFinalizaPasoActual As Boolean = False,
                                                            Optional piEstatusFinalizacion As Integer = -1,
                                                            Optional pbRegistraPasoSiguiente As Boolean = False,
                                                            Optional piPasoSiguiente As Integer = -1,
                                                            Optional piEstatusPasoSiguiente As Integer = -1,
                                                            Optional pbNotificar As Boolean = False,
                                                            Optional objCorreoBD As Entities.Correo = Nothing,
                                                            Optional pbAreaVoVf As Boolean = False,
                                                            Optional pbAreaVj As Boolean = False,
                                                            Optional pbAreaPresidencia As Boolean = False,
                                                            Optional pbReactivaPaso As Boolean = False,
                                                            Optional piTipoReactivacion As Integer = -1,
                                                            Optional piPasoActual As Integer = -1,
                                                            Optional piPasoReactivado As Integer = -1,
                                                            Optional piEstatusReactivacion As Integer = -1,
                                                            Optional pdFechaFinalizacionPaso As DateTime? = Nothing,
                                                            Optional pbNotificarSuperUsuarios As Boolean = False)

        Dim con As Conexion.SQLServer = Nothing
        Dim tran As SqlClient.SqlTransaction = Nothing
        Dim guardo As Boolean = True

        Try
            con = New Conexion.SQLServer()
            tran = con.BeginTransaction()

            Dim estatusPaso As New EstatusPaso()
            Dim PasoPrV As New PasoProcesoVisita()
            Dim ldFechaAux As DateTime = Nothing

            If IsNothing(pdFechaFinalizacionPaso) Then
                ''Validar que la fecha de finalizacion del paso actual.
                ldFechaAux = DateTime.Now

                ''Si la fecha de hoy es menor a la fecha de inicio del paso que se quiere finalizar entonces tomar la fecha de inicio mas N minutos
                If pbFinalizaPasoActual Then
                    Dim ldFechaAuxPaso As DateTime

                    If Not IsNothing(visita.FechaInicioPasoActual) Then
                        ldFechaAuxPaso = visita.FechaInicioPasoActual

                        If DateTime.Today.Date < ldFechaAuxPaso.Date Then
                            Dim liAuxMinutos As Integer = 0
                            If Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.MinutosExtraFechaInicio), liAuxMinutos) Then
                                ldFechaAux = ldFechaAuxPaso.AddMinutes(liAuxMinutos)
                            End If
                        End If
                    End If
                End If
            Else
                ldFechaAux = pdFechaFinalizacionPaso
            End If

            ''Registrar el comentario
            If pbRegistraComentario Then
                guardo = False

                estatusPaso.IdAreaActual = objUsuario.IdArea
                estatusPaso.IdVisitaGenerado = visita.IdVisitaGenerado
                estatusPaso.IdPaso = visita.IdPasoActual
                estatusPaso.IdEstatus = piEstatusComentario
                estatusPaso.FechaRegistro = DateTime.Now
                estatusPaso.IdUsuario = objUsuario.IdentificadorUsuario
                estatusPaso.Comentarios = ppObservaciones
                estatusPaso.SubVisitasSeleccionadas = visita.SubVisitasSeleccionadas

                If AccesoBD.registrarEstatusPaso(PasoPrV, estatusPaso, con, tran) = True Then
                    guardo = True
                End If
            End If


            'Se finaliza el paso actual
            If guardo Then
                If pbFinalizaPasoActual Then
                    guardo = False ''Reiniciar el estatus

                    estatusPaso = New EstatusPaso()
                    estatusPaso.IdAreaActual = objUsuario.IdArea
                    estatusPaso.IdVisitaGenerado = visita.IdVisitaGenerado
                    estatusPaso.IdPaso = visita.IdPasoActual
                    estatusPaso.IdEstatus = piEstatusFinalizacion
                    estatusPaso.FechaRegistro = DateTime.Now
                    estatusPaso.IdUsuario = objUsuario.IdentificadorUsuario ' se obtiene de la session
                    estatusPaso.Comentarios = "Finaliza paso " & visita.IdPasoActual.ToString()
                    estatusPaso.TipoComentario = Constantes.TipoComentario.SISTEMA
                    estatusPaso.SubVisitasSeleccionadas = visita.SubVisitasSeleccionadas

                    If AccesoBD.registrarEstatusPaso(PasoPrV, estatusPaso, con, tran) = True Then
                        If AccesoBD.finalizarPaso(visita.IdVisitaGenerado, visita.IdPasoActual, con, tran, ldFechaAux, visita.SubVisitasSeleccionadas) = True Then
                            ''ACTUALIZAR EL ESTATUS DE LA VISITA
                            If Not pbRegistraPasoSiguiente Then ''ACTUALIZA EL ESTATUS SOLO SI NO VA  REGISTRAR UN PASO NUEVO, SI NO SE ACTUALIZA DESPUES DE REGISTRARLO
                                If AccesoBD.ActualizaEstatusVisita(con, tran, visita.IdVisitaGenerado) Then
                                    guardo = True
                                End If
                            Else
                                guardo = True
                            End If
                        End If
                    End If
                End If
            End If


            'Registra el paso siguiente
            If guardo Then
                If pbRegistraPasoSiguiente Then
                    guardo = False ''Reiniciar el estatus

                    Dim Paso As New PasoProcesoVisita()

                    Paso.IdVisitaGenerado = visita.IdVisitaGenerado
                    Paso.IdPaso = piPasoSiguiente 'Paso siguiente
                    If IsNothing(ldFechaAux) Then
                        Paso.FechaInicio = DateTime.Now
                    Else
                        Paso.FechaInicio = ldFechaAux
                    End If
                    Paso.FechaFin = Nothing
                    Paso.EsNotificado = False
                    Paso.IdAreaNotificada = Constantes.AREA_SN
                    Paso.IdUsuarioNotificado = Nothing
                    Paso.EmailUsuarioNotificado = Nothing
                    Paso.TieneProrroga = False
                    Paso.FechaNotifica = Nothing
                    Paso.SubVisitasSeleccionadas = visita.SubVisitasSeleccionadas

                    If AccesoBD.registrarPaso(Paso, con, tran) = True Then
                        estatusPaso = New EstatusPaso()
                        estatusPaso.IdAreaActual = objUsuario.IdArea
                        estatusPaso.IdVisitaGenerado = visita.IdVisitaGenerado
                        estatusPaso.IdPaso = piPasoSiguiente 'Paso siguiente
                        estatusPaso.IdEstatus = piEstatusPasoSiguiente
                        estatusPaso.FechaRegistro = DateTime.Now
                        estatusPaso.IdUsuario = objUsuario.IdentificadorUsuario
                        estatusPaso.Comentarios = "Finaliza el paso " & visita.IdPasoActual.ToString() &
                                                  " e inicia el paso " & piPasoSiguiente.ToString() & " automáticamente."
                        estatusPaso.TipoComentario = Constantes.TipoComentario.SISTEMA
                        estatusPaso.SubVisitasSeleccionadas = visita.SubVisitasSeleccionadas

                        If AccesoBD.registrarEstatusPaso(Paso, estatusPaso, con, tran) Then
                            ''ACTUALIZAR EL ESTATUS DE LA VISITA
                            If AccesoBD.ActualizaEstatusVisita(con, tran, visita.IdVisitaGenerado) Then
                                guardo = True

                                'Actualiza paso 2 en BSIS_X_C_PASO_SUPERVISAR_VISITA
                                If visita.VisitaSisvig Then
                                    Dim visitaSisvig As New Entities.Sisvig() 'MCS 
                                    visitaSisvig.NotificaSisvig(visita.IdVisitaGenerado, piPasoSiguiente, piEstatusPasoSiguiente, ppObservaciones)
                                End If

                            End If
                        End If
                    End If
                End If
            End If


            ''Valida si se reactiva paso
            If guardo Then
                If pbReactivaPaso Then
                    guardo = False

                    estatusPaso = New EstatusPaso()
                    Dim Paso = New PasoProcesoVisita()
                    estatusPaso.IdAreaActual = objUsuario.IdArea
                    estatusPaso.IdVisitaGenerado = visita.IdVisitaGenerado
                    estatusPaso.FechaRegistro = DateTime.Now
                    estatusPaso.IdUsuario = objUsuario.IdentificadorUsuario
                    estatusPaso.TipoComentario = Constantes.TipoComentario.SISTEMA
                    estatusPaso.IdEstatus = piEstatusReactivacion

                    If piTipoReactivacion = Constantes.TipoReactivacion.Reactivado Then
                        estatusPaso.IdPaso = piPasoReactivado
                        estatusPaso.Comentarios = "Paso " & piPasoReactivado.ToString() & " reactivado por rechazo de documentos."
                    Else
                        estatusPaso.IdPaso = piPasoActual
                        estatusPaso.Comentarios = "Paso " & piPasoActual.ToString() & " retomado por revisión de documentos."
                    End If
                    estatusPaso.SubVisitasSeleccionadas = visita.SubVisitasSeleccionadas

                    If AccesoBD.registrarEstatusPaso(Paso, estatusPaso, con, tran) = True Then
                        If AccesoBD.SaveUpdateReactivacionPaso(visita.IdVisitaGenerado, piPasoActual, piPasoReactivado, piTipoReactivacion, con, tran, visita.SubVisitasSeleccionadas, objUsuario.IdentificadorUsuario, objUsuario.IdArea) = True Then
                            guardo = True

                            'Actualiza paso 2 en BSIS_X_C_PASO_SUPERVISAR_VISITA
                            If visita.VisitaSisvig Then
                                Dim visitaSisvig As New Entities.Sisvig() 'MCS 

                                If piTipoReactivacion = Constantes.TipoReactivacion.Reactivado Then
                                    visitaSisvig.NotificaSisvig(visita.IdVisitaGenerado, piPasoReactivado, piEstatusReactivacion, ppObservaciones)
                                Else
                                    visitaSisvig.NotificaSisvig(visita.IdVisitaGenerado, piPasoActual, piEstatusReactivacion, ppObservaciones)
                                End If
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            'Registro fallido
            guardo = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, imgSiguiente_Click", "")
        Finally
            If Not IsNothing(tran) Then
                If guardo Then
                    'Registro exitoso
                    tran.Commit()

                    If pbNotificar Then
                        If Constantes.CORREO_ENVIADO_OK = objNotifVisita.NotificarCorreo(objCorreoBD, visita, pbAreaVoVf, pbAreaVj, pbAreaPresidencia, Nothing, pbNotificarSuperUsuarios) Then
                            AccesoBD.actualizarPasoNotificadoSinTransaccion(visita.IdVisitaGenerado, visita.IdPasoActual, True, visita.Usuario.IdArea, objNotifVisita.getDestinatariosNombre(), objNotifVisita.getDestinatariosCorreo(), DateTime.Now)
                        End If
                    End If
                Else
                    'Registro fallido
                    tran.Rollback()
                End If
                tran.Dispose()
            End If

            If Not IsNothing(con) Then
                con.CerrarConexion()
                con = Nothing
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Abogado supervisor asigna a algun abogado, le notifica al abogado asignado
    ''' </summary>
    ''' <param name="piIdAbogado"></param>
    ''' <param name="psNomAbogado"></param>
    ''' <param name="piTipoAbogado"></param>
    ''' <param name="piEstatusPaso"></param>
    ''' <remarks></remarks>
    Public Sub PGAbogadoSupervisorAsignaAbogado(piIdAbogado As String, psNomAbogado As String,
                                                         Optional piTipoAbogado As Integer = Constantes.ABOGADOS.PERFIL_ABO_ASESOR,
                                                         Optional piEstatusPaso As Integer = Constantes.EstatusPaso.AsesorAsignado)
        Dim iniciarPaso2 As Boolean = False

        Dim con As Conexion.SQLServer = Nothing
        Dim tran As SqlClient.SqlTransaction = Nothing
        Dim guardo As Boolean = False

        Try
            con = New Conexion.SQLServer()
            tran = con.BeginTransaction()

            visita.IdAbogadoAsesor = piIdAbogado
            visita.NombreAbogadoAsesor = psNomAbogado

            'Asignar Responsable VJ seleccionado
            If AccesoBD.asignarAbogadoVisita(visita, con, tran, piTipoAbogado, visita.SubVisitasSeleccionadas) = True Then

                Dim estatusPaso As New EstatusPaso()
                Dim PasoPrV As New PasoProcesoVisita()
                estatusPaso.IdAreaActual = objUsuario.IdArea
                estatusPaso.IdVisitaGenerado = visita.IdVisitaGenerado
                estatusPaso.IdPaso = visita.IdPasoActual
                estatusPaso.IdEstatus = piEstatusPaso
                estatusPaso.FechaRegistro = DateTime.Now
                estatusPaso.IdUsuario = objUsuario.IdentificadorUsuario
                estatusPaso.Comentarios = ppObservaciones
                estatusPaso.SubVisitasSeleccionadas = visita.SubVisitasSeleccionadas
                '' PasoPrV.IdMovimiento = piMovimiento

                If AccesoBD.registrarEstatusPaso(PasoPrV, estatusPaso, con, tran) = True Then
                    guardo = True
                End If
            End If
        Catch ex As Exception
            'Registro fallido
            guardo = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, imgSiguiente_Click", "")
        Finally
            If Not IsNothing(tran) Then
                If guardo Then
                    'Registro exitoso
                    tran.Commit()
                    'Notifica al abogado seleccionado
                    'If pbNotificar Then
                    '    objNotifVisita.NotificarCorreoAbogadoAsignado(Constantes.CORREO_ID_NOTIFICA_ABOGADO_ASESOR, visita, piTipoAbogado)
                    'End If
                Else
                    'Registro fallido
                    tran.Rollback()
                End If
                tran.Dispose()
            End If

            If Not IsNothing(con) Then
                con.CerrarConexion()
                con = Nothing
            End If
        End Try
    End Sub


    Public Sub CancelarVisita()
        Dim con As Conexion.SQLServer = Nothing
        Dim tran As SqlClient.SqlTransaction = Nothing
        Dim guardo As Boolean = False
        Dim idVistaCancelar As Integer

        Try
            con = New Conexion.SQLServer()
            tran = con.BeginTransaction()

            idVistaCancelar = visita.IdVisitaGenerado
            Dim motivoCancelacion As String = ppObservaciones

            If AccesoBD.cancelarVisita(idVistaCancelar, motivoCancelacion, con, tran) = True Then
                guardo = True
            End If

        Catch ex As Exception
            guardo = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, btnAceptarM2B1A_Click-imgCancelarVisita", "")
        Finally
            If Not IsNothing(tran) Then
                If guardo Then
                    'Cancelación exitosa
                    tran.Commit()
                    objNotifVisita.NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_CANCELACION, visita, True, True, False)
                Else
                    'Cancelación fallida
                    tran.Rollback()
                End If
                tran.Dispose()
            End If

            If Not IsNothing(con) Then
                con.CerrarConexion()
                con = Nothing
            End If
        End Try
    End Sub
#End Region

End Class