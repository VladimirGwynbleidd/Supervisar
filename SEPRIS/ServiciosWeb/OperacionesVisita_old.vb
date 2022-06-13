Public Class OperacionesVisita
    Public Property IdVisita As Integer
    Public Property IdVisitaSisvig As Integer
    Public Property IdVisitaSepris As Integer
    Public Property IdentificadorUsuario As String
    Public Property validaDoctosPaso6 As String
    Public Property Comentarios As String
    Public Property Fecha As DateTime
    Public Property TipoFecha As Constantes.TipoFecha
    Public Property MotivoProrroga As String
    Public Property FlagReunionHallazgos As Boolean
    Public Property FlagSancion As Boolean
    Public Property PrimeraNotificacionOcho As Boolean
    Dim objAux As New Auxiliares

    Public Function ActualizaFecha() As Integer
        Dim liError As Integer = 0
        Dim pbjPaso As New Paso
        pbjPaso.cargaPasoActualVisita(IdVisita)

        ''VALIDA LA FEHCHA DE ENTRADA
        Select Case TipoFecha
            Case Constantes.TipoFecha.FechaInicio           ''Fecha de inicio de la Visita
                liError = objAux.ValidarFechaGeneral(Fecha, pbjPaso.TieneProrroga, pbjPaso.FechaInicioEnVisita, False)
                'InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig pbjPaso.IdPaso, IdentificadorUsuario, "Valida la fecha de inicio de la Visita")
            Case Constantes.TipoFecha.FechaVulneravilidades ''Fecha de revisión de vulnerabilidades
                liError = objAux.ValidarFechaGeneral(Fecha, pbjPaso.TieneProrroga, pbjPaso.FechaInicioEnVisita, False)
                'InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig pbjPaso.IdPaso, IdentificadorUsuario, "Valida la fecha de revisión de vulnerabilidades")
            Case Constantes.TipoFecha.FechaCampoInicial ''Fecha de inicio de visita in situ.
                liError = objAux.ValidarFechaGeneral(Fecha, pbjPaso.TieneProrroga, pbjPaso.FechaInicioEnVisita, False)
                'InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig pbjPaso.IdPaso, IdentificadorUsuario, "Valida la fecha de inicio de visita in situ")
            Case Constantes.TipoFecha.FechaCampoFinal   ''Fecha fin de visita in situ
                liError = objAux.ValidarFechaGeneral(Fecha, pbjPaso.TieneProrroga, pbjPaso.FechaInicioEnVisita, True, True)
                'InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig pbjPaso.IdPaso, IdentificadorUsuario, "Valida la fecha de fin de visita in situ")
            Case Constantes.TipoFecha.FechaReunionPresi ''Fecha en que se realizará la presentación del paso 8
                liError = objAux.ValidarFechaGeneral(Fecha, pbjPaso.TieneProrroga, pbjPaso.FechaInicioEnVisita, True, True)
                'InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig pbjPaso.IdPaso, IdentificadorUsuario, "Valida la fecha en que se realizará la presentacipon paso 8")
            Case Constantes.TipoFecha.FechaReunionVjp9  ''Fecha de sesión para revisión de acta circunstanciada 
                liError = objAux.ValidarFechaGeneral(Fecha, pbjPaso.TieneProrroga, pbjPaso.FechaInicioEnVisita, False, False, True, True, PasoProcesoVisita.Pasos.Diez)
                'InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig pbjPaso.IdPaso, IdentificadorUsuario, "Valida la fecha de sesión para revisión de acta circunstanciada")
            Case Constantes.TipoFecha.FechaReunionAfore '' Fecha de la reunión para la presentación paso 13
                liError = objAux.ValidarFechaGeneral(Fecha, pbjPaso.TieneProrroga, pbjPaso.FechaInicioEnVisita, True, True)
                'InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig pbjPaso.IdPaso, IdentificadorUsuario, "Valida la fecha de la reunipon para la presentación paso 13")
            Case Constantes.TipoFecha.FechaInSituActaCircunstanciada    ''Fecha en que se realizó el levantamiento in situ paso 14
                liError = objAux.ValidarFechaGeneral(Fecha, pbjPaso.TieneProrroga, pbjPaso.FechaInicioEnVisita, True, True)
                'InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig pbjPaso.IdPaso, IdentificadorUsuario, "Valida la fecha en que se realizó el levantamiento in situ paso 14")
            Case Constantes.TipoFecha.FechaReunionVjp16 ''Fecha de sesión para revisión de acta de conclusión paso 16
                liError = objAux.ValidarFechaGeneral(Fecha, pbjPaso.TieneProrroga, pbjPaso.FechaInicioEnVisita, False, False, True, True, PasoProcesoVisita.Pasos.Diesisiete)
                'InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig pbjPaso.IdPaso, IdentificadorUsuario, "Valida la fecha de sesión para revisión de acta de conclusión paso 16")
            Case Constantes.TipoFecha.FechaLevantamientoActaConclucion ''Fecha se llevará a cabo la reunión con la Vicepresidencia Jurídica paso 16
                liError = objAux.ValidarFechaGeneral(Fecha, pbjPaso.TieneProrroga, pbjPaso.FechaInicioEnVisita, True, True)
                'InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig pbjPaso.IdPaso, IdentificadorUsuario, "Valida la fecha se llevará a cabo la reunión con la Vicepresidencia Jurídica paso 16")

        End Select

        If liError = -1 Then
            If AccesoBD.ActualizaFechaInicioVisita(IdVisita, Fecha, TipoFecha, "") Then
                liError = -1
                ''EJECUTA ALGUNA ACCION DESPUES DE ACTUALIZAR ALGUNA FECHA
                ''DE QUITA LA FUNCIONALIDAD PARA EVITAR CONFUNDIRSE, YA QUE NO TODAS LAS FECHAS AVANZAN.. :(..
                'Select Case TipoFecha
                '    Case Constantes.TipoFecha.FechaCampoInicial, Constantes.TipoFecha.FechaCampoFinal
                '        liError = Me.Avanza()
                '    Case Else
                '        liError = -1
                'End Select
            Else
                liError = 418
            End If
        End If

        objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, liError, "ActualizaFecha", "Se actualizo la fecha [" + TipoFecha.ToString() + "]. " & Comentarios)

        Return liError
    End Function

    Public Function Avanza() As Integer
        Dim objVisita As Visita
        Dim objUsuario As Entities.Usuario
        Dim objRegistro As New Registro
        Dim liError As Integer = -1
        Dim liPasoActual As Integer = 0

        objVisita = ObtenerDetalleVisita()

        ''RECUPERAR LA VISITA
        If objVisita.ExisteVisita Then
            liPasoActual = objVisita.IdPasoActual

            If Not DocumentosObligatoriosSinCargar(liPasoActual, validaDoctosPaso6) Then

                objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, liError, "después de entrar a validar documentos obligatorios : " & objVisita.IdEstatusActual.ToString(), "")

                ''REGISTRAR/OBTENER USUARIO
                objUsuario = objRegistro.InsertaUsuario(IdentificadorUsuario)
                If objUsuario.Existe Then
                    Dim objNegVisita As New NegocioVisita(objVisita, objUsuario, Nothing, Comentarios)

                    Select Case objVisita.IdPasoActual
                        Case 1
                            Select Case objVisita.IdEstatusActual
                                Case Constantes.EstatusPaso.Registrado
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                                            True, Constantes.EstatusPaso.Enviado,
                                                                                            True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.Enviado,
                                                                                            True, Constantes.CORREO_ID_NOTIFICA_VJ_REVISAR_OF_COM_ACT_INI)

                                    InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Notifica a VJ revisión de Oficios y Acta incial")

                                Case Else
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesEnviados,
                                                                                            False, -1,
                                                                                            False, -1, -1,
                                                                                            True, Constantes.CORREO_ID_NOTIFICA_VJ_REVISAR_OF_COM_ACT_INI,
                                                                                            True, True, False,
                                                                                            True, Constantes.TipoReactivacion.FinalizaReactivacion,
                                                                                            (objVisita.IdPasoActual + 1), objVisita.IdPasoActual,
                                                                                            Constantes.EstatusPaso.AsesorAsignado)

                                    InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Notifica a VJ revisión de Oficios y Acta incial")

                            End Select
                        Case 3
                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                                    True, Constantes.EstatusPaso.Aprobado,
                                                                                    True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                                    True, Constantes.CORREO_VERSION_FINAL_DOCUMENTOS)

                            InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Notifica versión final de documentos paso 3")

                        Case 4
                            ''iniciar el paso 5
                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                                    True, Constantes.EstatusPaso.Notificado,
                                                                                    True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                                    True, Constantes.CORREO_FECHA_INICIO_VISITA)

                            InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Notifica fecha de inicio de visita paso 5")

                            ''iniciar tambien el paso 6
                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                                                    False, -1,
                                                                                    True, 6, Constantes.EstatusPaso.EnRevisionEspera,
                                                                                    False, -1)

                            InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Inicia paso 6")

                        Case 6
                            Select Case objVisita.IdEstatusActual
                                Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 6 inicia visita
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Visita_iniciada,
                                                                                    False, -1,
                                                                                    False, -1, -1,
                                                                                    True, Constantes.CORREO_ID_NOTIFICA_ABOGADO_Y_PRESIDENCIA_VISITA_INICIA)

                                    ActualizaEstatusSisvig(objVisita, objVisita.IdPasoActual, Constantes.EstatusPaso.Visita_iniciada, Comentarios)
                                    InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, objUsuario.IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Notifica a abogado y presidencia inicio de visita paso 6")

                                Case Constantes.EstatusPaso.Visita_iniciada, Constantes.EstatusPaso.AjustesRealizados ''Paso 6 detener visita
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                                    True, Constantes.EstatusPaso.Visita_Finalizada,
                                                                                    False, -1, -1,
                                                                                    False, -1,
                                                                                    False, False, False, False, -1, -1, -1, -1, objVisita.FECH_VISITA_CAMPO_FIN)

                                    ActualizaEstatusSisvig(objVisita, objVisita.IdPasoActual, Constantes.EstatusPaso.EnRevisionEspera, Comentarios)
                                    InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Se pone en pausa paso 6")

                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1, False, -1,
                                                                                    True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.En_diagnostico_de_hallazgos,
                                                                                    False, -1,
                                                                                    False, False, False, False, -1, -1, -1, -1, AccesoBD.ObtenerFecha(objVisita.FECH_VISITA_CAMPO_FIN, 1, Constantes.IncremeteDecrementa.Incrementa)) ''ya que empieza al dia siguiente del fin de la visita de campo)


                            End Select
                        Case 7

                            AvanzaPasoSiete(objNegVisita, objVisita)
                            InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Avanza paso 7")

                        Case 8
                            Select Case objVisita.IdEstatusActual
                                Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.HallazgosGuardados, Constantes.EstatusPaso.EnEsperaPresentarHallazgos, Constantes.EstatusPaso.AjustesRealizados
                                    Dim Paso As Integer
                                    If Not PrimeraNotificacionOcho Then
                                        Paso = IIf(FlagSancion, 9, 13)
                                    End If

                                    'Si es que se envía la fecha de presentación
                                    If PrimeraNotificacionOcho Then

                                        'ActualizaEstatusSisvig(objVisita, objVisita.IdPasoActual, Constantes.EstatusPaso.Enviado, Comentarios)

                                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnEsperaPresentarHallazgos,
                                                                                    False, -1,
                                                                                    False, objVisita.IdPasoActual, Constantes.EstatusPaso.EnRevisionEspera,
                                                                                    True, Constantes.CORREO_PRESENTA_DIAG_HALLAZGOS)
                                    Else

                                        If Paso = 9 Then

                                            ActualizaEstatusSisvig(objVisita, 9, Constantes.EstatusPaso.Enviado, Comentarios)

                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnEsperaPresentarHallazgos,
                                                                                        False, -1,
                                                                                        True, 9, Constantes.EstatusPaso.EnRevisionEspera,
                                                                                        False, -1,
                                                                                        False, False, False, False,
                                                                                        -1, -1, -1, -1,
                                                                                        Nothing, False, True)
                                        Else

                                            ActualizaEstatusSisvig(objVisita, 13, Constantes.EstatusPaso.Enviado, Comentarios)

                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnEsperaPresentarHallazgos,
                                                                                        False, -1,
                                                                                        True, 13, Constantes.EstatusPaso.EnRevisionEspera,
                                                                                        False, -1,
                                                                                        False, False, False, False,
                                                                                        -1, -1, -1, -1,
                                                                                        Nothing, False, True)

                                        End If

                                    End If

                                    InsertaBitacoraSisvigSepris(IdVisita, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & IdVisita & "-Notifica la presentación de hallazgos paso 8")

                                    If Not FlagSancion Then
                                        If Not AccesoBD.ActualizaSancionVisita(objVisita.IdVisitaGenerado, objVisita.IdPasoActual) Then
                                            liError = 422
                                        End If
                                    End If
                            End Select

                        Case 9
                            Select Case objVisita.IdEstatusActual
                                Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.Elaborada '' Paso 9 avanza paso 10, solicita fecha con vj
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                                        True, Constantes.EstatusPaso.Enviado,
                                                                                        True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                                        True, Constantes.CORREO_ID_NOTIFICA_ENVIO_ACTA_CIRCUNSTANCIADA,
                                                                                        True, True, False, , , , , , , True)

                                    InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Notifica envío de acta circunstanciada paso 9")

                                Case Else
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesEnviados,
                                                                                        False, -1,
                                                                                        False, -1, -1,
                                                                                        True, Constantes.CORREO_ID_NOTIFICA_ENVIO_ACTA_CIRCUNSTANCIADA,
                                                                                        True, True, False,
                                                                                        True, Constantes.TipoReactivacion.FinalizaReactivacion,
                                                                                        (objVisita.IdPasoActual + 1), objVisita.IdPasoActual,
                                                                                        Constantes.EstatusPaso.EnRevisionEspera, , True)

                                    InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Notifica envío de acta circunstanciada con ajustes paso 9")

                            End Select
                        Case 12
                            Select Case objVisita.IdEstatusActual
                                Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.Aprobado ''Paso 12
                                    ''Manda un segundo correo a sandra pacheco, configurada en parametros
                                    ''VALIDAR SI EXISTIO UNA REUNION EN PASO 8, SI NO EXISTIO NO ENVIAR CORREO
                                    If objVisita.ExisteReunionPaso8 Then
                                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                  True, Constantes.EstatusPaso.Enviado,
                                                                  True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                  True, Constantes.CORREO_SANDRA_PACHECO)

                                        InsertaBitacoraSisvigSepris(IdVisita, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & IdVisita & "-Envío de notificación de versión final de acta circunstanciada paso 12")

                                        If Not MandaCorreoSandraPachecoPaso12(objNegVisita, objVisita) Then
                                            liError = 423
                                        End If
                                    Else ''SI NO EXISTIO LA REUNION MANDA A PASO 14 DIRECTAMENTE
                                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                  True, Constantes.EstatusPaso.Enviado,
                                                                  True, 14, Constantes.EstatusPaso.EnRevisionEspera,
                                                                  True, Constantes.CORREO_VER_FINAL_ACTA_CIRCUNSTANCIADA)

                                        InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Envío de notificación de versión final de acta circunstanciada no existió reunión, paso 14 directo")

                                    End If
                            End Select

                        Case 13
                            Select Case objVisita.IdEstatusActual
                                Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.Hallazgos_presentados, Constantes.EstatusPaso.AjustesRealizados
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Notificado,
                                                                                False, -1, False, -1, -1,
                                                                                True, Constantes.CORREO_PRESENTACION_DIAGNOSTICO_HALLAZGOS)

                                    ActualizaEstatusSisvig(objVisita, objVisita.IdPasoActual, Constantes.EstatusPaso.Notificado, Comentarios)
                                    InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Envío de notificación de presentacipon de diagnóstico de hallazgos paso 13")

                            End Select
                        Case 14
                            Select Case objVisita.IdEstatusActual
                                Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.Registrado
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                             True, Constantes.EstatusPaso.Notificado,
                                                                             True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                             True, Constantes.CORREO_ID_NOTIFICA_VJ_PR_HA_LEVANTADO_ACTA_CIR_IN_SITU,
                                                                             False, True, True, , , , , , objVisita.FechaInSituActaCircunstanciada)

                                    ''Actualiza el paso actual, manda a paso 16
                                    objVisita.IdPasoActual = (objVisita.IdPasoActual + 1)
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                             False, -1,
                                                             True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                             False, -1)

                                    InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Envío de notificación a VJ y Presidencia levantamiento de acta circunstanciada in situ")

                            End Select
                        Case 16
                            Select Case objVisita.IdEstatusActual
                                Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.Revisado  ''Flujo inicial paso 16, revisado
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                             True, Constantes.EstatusPaso.Elaborada,
                                                                             True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                             True, Constantes.CORREO_ID_NOTIFICA_VJ_ADJUNTA_ACTA_CONCLUSION_OF_RECOMENDACIONES,
                                                                             True, True, , , , , , , , True)

                                    InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Envío de notificación a VJ adjunta acta circunstanciada con conclusión y recomendaciones, inicia paso 16")

                                Case Constantes.EstatusPaso.EnAjustes, Constantes.EstatusPaso.AjustesRealizados ''Flujo 2 paso 16, AjustesRealizados
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesEnviados,
                                                                             False, -1,
                                                                             False, -1, -1,
                                                                             True, Constantes.CORREO_ID_NOTIFICA_VJ_ADJUNTA_ACTA_CONCLUSION_OF_RECOMENDACIONES,
                                                                             True, True, False,
                                                                             True, Constantes.TipoReactivacion.FinalizaReactivacion,
                                                                             (objVisita.IdPasoActual + 1), objVisita.IdPasoActual,
                                                                             Constantes.EstatusPaso.EnRevisionEspera, , True)

                                    InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Envío de notificación a VJ adjunta acta circunstanciada con conclusión y recomendaciones, paso 16 con ajustes")

                            End Select

                        Case 18
                            objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, liError, "Estatus del paso al querer pasar del paso 18 : " & objVisita.IdEstatusActual.ToString(), "")

                            Select Case objVisita.IdEstatusActual
                                Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.Revisado, Constantes.EstatusPaso.Enviado  ''Flujo inicial paso 18, Revisado, ''Flujo inicial paso 18, Revisado, regresa paso 17
                                    If objVisita.TieneSancion Then
                                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                             True, Constantes.EstatusPaso.Notificado,
                                                             True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                             True, Constantes.CORREO_ID_NOTIFICA_VJ_PR_HA_LEVANTADO_ACTA_CON_IN_SITU,
                                                             True, True, False, , , , , , objVisita.FechaLevantamientoActaConclucion, True)

                                        InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Notificación a VJ y Presidencia levantamiento acta con in situ revisado paso 18")

                                    Else
                                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                             True, Constantes.EstatusPaso.Notificado,
                                                             False, -1, -1,
                                                             True, Constantes.CORREO_ID_NOTIFICA_VJ_PR_HA_LEVANTADO_ACTA_CON_IN_SITU,
                                                             True, True, False, , , , , , objVisita.FechaLevantamientoActaConclucion, True)
                                    End If

                                Case Constantes.EstatusPaso.Notificado 'Avanza al paso 19
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                             True, Constantes.EstatusPaso.Notificado,
                                                             True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                             True, Constantes.CORREO_ID_NOTIFICA_VJ_PR_HA_LEVANTADO_ACTA_CON_IN_SITU,
                                                             True, True, False, , , , , , objVisita.FechaLevantamientoActaConclucion, True)

                                    InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Notificación a VJ y Presidencia levantamiento acta con in situ revisado paso 18 - avanza paso 19")
                            End Select
                        Case 19
                            Select Case objVisita.IdEstatusActual
                                Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.Revisado
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                            True, Constantes.EstatusPaso.Notificado,
                                                            True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                            True, Constantes.CORREO_ID_NOTIFICA_VJ_DICTAMEN)

                                    InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Notificación a VJ dictamen paso 19")

                            End Select
                    End Select
                Else
                    liError = 414
                End If

                'Actualiza la nomenclatura de todos los documentos temporales
                If Not AccesoBD.MigrarDocumentosSinVisita(IdVisita, "", objVisita.IdPasoActual, Constantes.Todos, IdVisitaSisvig) Then
                    liError = 404
                End If

            Else
                liError = 2140
            End If

        Else
            liError = 413
        End If

        objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, liError, "Avanza paso: " & liPasoActual.ToString(), Comentarios)

        Return liError
    End Function

    Public Function Rechaza() As Integer
        Dim objVisita As Visita
        Dim objUsuario As Entities.Usuario
        Dim objRegistro As New Registro
        Dim liError As Integer = -1
        Dim liPasoActual As Integer = 0

        If Comentarios.Trim().Length > 0 Then
            objVisita = ObtenerDetalleVisita()
            ''RECUPERAR LA VISITA
            If objVisita.ExisteVisita Then
                liPasoActual = objVisita.IdPasoActual

                ''REGISTRAR/OBTENER USUARIO
                objUsuario = objRegistro.InsertaUsuario(IdentificadorUsuario)
                If objUsuario.Existe Then
                    Dim objNegVisita As New NegocioVisita(objVisita, objUsuario, Nothing, Comentarios)

                    Select Case objVisita.IdPasoActual
                        Case 3
                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                    False, -1,
                                                                                    False, -1, -1,
                                                                                    True, Constantes.CORREO_RECHAZO_PASO_DOS,
                                                                                    True, True, False,
                                                                                    True, Constantes.TipoReactivacion.Reactivado,
                                                                                    objVisita.IdPasoActual, (objVisita.IdPasoActual - 1),
                                                                                    Constantes.EstatusPaso.EnAjustes)

                            InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Notificación de rechazo en paso 2")

                        Case 12
                            Select Case objVisita.IdEstatusActual
                                Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 12 rechaza
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                    False, -1,
                                                                                    False, -1, -1,
                                                                                    True, Constantes.CORREO_PASO_12_RECHAZA_VERSION_FIN_DOC,
                                                                                    True, True, False,
                                                                                    True, Constantes.TipoReactivacion.Reactivado,
                                                                                    objVisita.IdPasoActual, 10,
                                                                                    Constantes.EstatusPaso.EnAjustes, , True)
                                    ActualizaEstatusSisvig(objVisita, 10, Constantes.EstatusPaso.EnRevisionEspera, "")
                                    InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Notificación de rechazo versión final de documentos en paso 12")

                            End Select
                        Case 18
                            'Select Case objVisita.IdEstatusActual
                            'Case Constantes.EstatusPaso.Revisado, Constantes.EstatusPaso.Notificado  ''Flujo inicial paso 18, Revisado, regresa paso 17
                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                            False, -1,
                                                                            False, -1, -1,
                                                                            True, Constantes.CORREO_RECHAZO_COMENTARIOS,
                                                                            True, True, False,
                                                                            True, Constantes.TipoReactivacion.Reactivado,
                                                                            objVisita.IdPasoActual, (objVisita.IdPasoActual - 1),
                                                                            Constantes.EstatusPaso.EnAjustes, , True)

                            InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, objVisita.IdPasoActual, IdentificadorUsuario, "Visita: " & objVisita.IdVisitaSisvig & "-Notificación de rechazo en paso 18 con comentarios")

                            'End Select
                    End Select
                Else
                    liError = 414
                End If
            Else
                liError = 413
            End If
        Else
            liError = 415
        End If

        objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, liError, "Rechaza paso: " & liPasoActual.ToString(), Comentarios)

        Return liError
    End Function

    Public Function FinalizaCargaDocumentos(ByVal psCadenaComentarios As String) As Integer

        Dim vecDatos() As String = psCadenaComentarios.Split("|")
        Dim liIdDoc As Integer = vecDatos(0)
        Dim liIdPaso As Integer = vecDatos(1)
        Dim liTipoArchivo As Integer = vecDatos(2)
        Dim liHeredar As Integer = vecDatos(3)
        Dim liHeredarSbVisitas As Integer = vecDatos(4)
        Dim liError As Integer = -1

        Dim objRegistro As New Registro
        Dim objUsuario As Entities.Usuario

        Dim ppObjVisita As Visita
        ppObjVisita = ObtenerDetalleVisita()

        If ppObjVisita.ExisteVisita Then
            objUsuario = objRegistro.InsertaUsuario(IdentificadorUsuario)
            If objUsuario.Existe Then
                ''Finalizar el paso en que estan los docs y notificar.
                If liTipoArchivo = Constantes.TipoArchivo.WORD Then
                    If AccesoBD.finalizarPasoSinTransaccion(ppObjVisita.IdVisitaGenerado, liIdPaso, liTipoArchivo, Constantes.Verdadero, 0, "") Then
                        liError = -1
                    Else
                        liError = 419
                    End If
                Else
                    If AccesoBD.finalizarPasoSinTransaccion(ppObjVisita.IdVisitaGenerado, liIdPaso, liTipoArchivo, Constantes.Falso, Constantes.Verdadero, "") Then
                        liError = -1
                    Else
                        liError = 420
                    End If
                End If

                ''A quien notificar?, dejemoslo estatico ya que no se definio funcionalidad para esto
                ''SI EL PASO ES 5 NOTIFICAR A SUPERVISOR VO/VF Y A VJ

                If liIdPaso = PasoProcesoVisita.Pasos.Cinco Or liIdPaso = PasoProcesoVisita.Pasos.Seis Or liIdPaso = PasoProcesoVisita.Pasos.Quince Then
                    If Not IsNothing(objUsuario) Then
                        If Not IsNothing(ppObjVisita) Then
                            Dim objNotif As New NotificacionesVisita(objUsuario, Nothing, Comentarios)
                            Dim objCorreoBD As New Entities.Correo(Constantes.CORREO_DOCUMENTOS_ADJUNTADOS)
                            objCorreoBD.Cuerpo = objCorreoBD.Cuerpo.Replace("[PASO]", liIdPaso.ToString())
                            If Constantes.CORREO_ENVIADO_OK = objNotif.NotificarCorreo(objCorreoBD, ppObjVisita, True, True, False) Then
                                AccesoBD.actualizarPasoNotificadoSinTransaccion(ppObjVisita.IdVisitaGenerado, liIdPaso, True, objUsuario.IdArea, objNotif.getDestinatariosNombre(), objNotif.getDestinatariosCorreo(), DateTime.Now)
                                liError = -1
                            Else
                                liError = 412
                            End If
                        End If
                    End If
                End If
            Else
                liError = 414
            End If
        Else
            liError = 413
        End If

        objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, liError, "Finalizar_Paso5", Comentarios)

        Return liError
    End Function


    Public Function SolicitaProrroga(ByVal motivoProrroga As String) As Integer
        Dim liError As Integer = -1

        Dim objNegVisita As NegocioVisita
        Dim objVisita As Visita
        objVisita = ObtenerDetalleVisita()

        Dim objRegistro As New Registro
        Dim objUsuario As Entities.Usuario

        If objVisita.ExisteVisita Then
            objUsuario = objRegistro.InsertaUsuario(IdentificadorUsuario)
            objNegVisita = New NegocioVisita(objVisita, objUsuario, Nothing, motivoProrroga)

            Dim prorroga As New Prorroga()

            prorroga.IdVisitaGenerado = objVisita.IdVisitaGenerado
            prorroga.IdPaso = objVisita.IdPasoActual
            prorroga.FechaRegistro = DateTime.Now
            prorroga.NumDiasDeProrroga = 0
            prorroga.MotivoProrroga = motivoProrroga.Trim()
            prorroga.FechaFinProrroga = Nothing

            prorroga.ApruebaProrroga = Constantes.Verdadero

            prorroga.SubVisitasSeleccionadas = objVisita.SubVisitasSeleccionadas
            objVisita.MotivoProrroga = motivoProrroga.Trim()

            Dim con As Conexion.SQLServer = Nothing
            Dim tran As SqlClient.SqlTransaction = Nothing
            Dim guardo As Boolean = False

            Try
                con = New Conexion.SQLServer()
                tran = con.BeginTransaction()
                If AccesoBD.registrarProrroga(prorroga, con, tran) > 0 Then
                    guardo = True
                    liError = -1
                End If
            Catch ex As Exception
                'Registro fallido
                guardo = False
                liError = 424
                Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "OperacionesVisita.vb, SolicitaProrroga", "")
            Finally
                If Not IsNothing(tran) Then
                    If guardo Then
                        'Solicitud de prorroga exitosa
                        tran.Commit()

                        ''Notificar prorroga 
                        If Constantes.CORREO_ENVIADO_OK <> objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_VJ_PR_VISITA_ENTRA_EN_PRORROGA,
                                                                          objVisita, True, True, True) Then
                            liError = 410
                        End If
                    Else
                        'Solicitud de prorroga fallida
                        liError = 421
                        tran.Rollback()

                    End If
                    tran.Dispose()
                End If

                If Not IsNothing(con) Then
                    con.CerrarConexion()
                    con = Nothing
                End If
            End Try
        Else
            liError = 413
        End If

        objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, liError, "Solicitud de Prorroga", motivoProrroga)

        Return liError
    End Function

    Public Function ObtenerDetalleVisita() As Visita
        Dim objVisita As Visita
        objVisita = AccesoBD.getDetalleVisita(IdVisita, Constantes.AREA_VO)

        If objVisita.ExisteVisita Then
            objVisita.LstSupervisoresAsignados = AccesoBD.getSupervisoresAsignados(IdVisita)
            objVisita.LstInspectoresAsignados = AccesoBD.getInspectoresAsignados(IdVisita)

            Dim lstAbogados As List(Of Abogado) = AccesoBD.getAbogadosAsignados(IdVisita)
            objVisita.LstAbogadosSupAsesorAsig = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.PERFIL_SUP And objAbo.SubPerfil = Constantes.ABOGADOS.PERFIL_ABO_ASESOR Select objAbo).ToList()
            objVisita.LstAbogadosAsesorAsignados = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.ABOGADOS.PERFIL_ABO_ASESOR Select objAbo).ToList()

            objVisita.LstAbogadosSupSancionAsig = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.PERFIL_SUP And objAbo.SubPerfil = Constantes.ABOGADOS.PERFIL_ABO_SANCIONES Select objAbo).ToList()
            objVisita.LstAbogadosSancionAsignados = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.ABOGADOS.PERFIL_ABO_SANCIONES Select objAbo).ToList()

            objVisita.LstAbogadosSupContenAsig = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.PERFIL_SUP And objAbo.SubPerfil = Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO Select objAbo).ToList()
            objVisita.LstAbogadosContenAsignados = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO Select objAbo).ToList()
        End If

        Return objVisita
    End Function

    Private Sub AvanzaPasoSiete(objNegVisita As NegocioVisita, objVisita As Visita)
        ''Valida si hay presentacion de hallazgos
        Dim liError As Integer = -1

        If FlagReunionHallazgos Then
            If AccesoBD.ActualizaFechaInicioVisita(IdVisita, Date.Now, Constantes.TipoFecha.BanderaDeReunionPaso8, "") Then

                ActualizaEstatusSisvig(objVisita, objVisita.IdPasoActual, Constantes.EstatusPaso.Hallazgos_presentados_notificado, Comentarios)

                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                    True, Constantes.EstatusPaso.Hallazgos_presentados_notificado,
                                                                    True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                    False, -1)

                ''Manda correo a sandra pacheco, configurada en parametros
                Dim lstPersonasDestinatarios As New List(Of Persona)
                Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.SandraPacheco)

                ''Conexion.SQLServer
                If dt.Rows.Count > 0 Then
                    lstPersonasDestinatarios.Add(New Persona With {.Nombre = Constantes.Parametros.SandraPacheco, .Correo = dt.Rows(0)("T_DSC_VALOR")})
                End If

                ''Notifica si todo salio bien, a area VJ y a presidencia
                If Constantes.CORREO_ENVIADO_OK = objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_FINALIZA_DIAG_HALLAZGOS, objVisita, True, True, True, lstPersonasDestinatarios, True) Then
                    AccesoBD.actualizarPasoNotificadoSinTransaccion(objVisita.IdVisitaGenerado, objVisita.IdPasoActual, True, objVisita.Usuario.IdArea, objNegVisita.getObjNotificacion().getDestinatariosNombre(), objNegVisita.getObjNotificacion().getDestinatariosCorreo(), DateTime.Now)
                End If
            End If
        Else

            ActualizaEstatusSisvig(objVisita, objVisita.IdPasoActual, Constantes.EstatusPaso.Hallazgos_presentados_notificado, "")

            If FlagSancion Then
                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                        True, Constantes.EstatusPaso.Hallazgos_presentados_notificado,
                                                                        True, 9, Constantes.EstatusPaso.EnRevisionEspera,
                                                                        False, -1)
            Else
                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                        True, Constantes.EstatusPaso.Hallazgos_presentados_notificado,
                                                                        True, 16, Constantes.EstatusPaso.EnRevisionEspera,
                                                                        False, -1)

                If Not AccesoBD.ActualizaSancionVisita(objVisita.IdVisitaGenerado, objVisita.IdPasoActual) Then
                    liError = 422
                End If

            End If


        End If
    End Sub
    Private Function MandaCorreoSandraPachecoPaso12(objNegVisita As NegocioVisita, objVisita As Visita) As Boolean
        Dim lstPersonasDestinatarios As New List(Of Persona)
        Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.SandraPacheco)

        ''Conexion.SQLServer
        If dt.Rows.Count > 0 Then
            ''Folio, area, entidades
            lstPersonasDestinatarios.Add(New Persona With {.Nombre = Constantes.Parametros.SandraPacheco, .Correo = dt.Rows(0)("T_DSC_VALOR")})
            objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_SANDRA_PACHECO, objVisita,
                                                          True, True, False, lstPersonasDestinatarios, True)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function ActualizaNuevaFechaVulnera(IdVisitaSepris As Integer, FechaVulnera As Date) As Integer
        Dim objVisita As Visita
        Dim objUsuario As Entities.Usuario
        Dim objRegistro As New Registro
        Dim liError As Integer = -1
        Dim liPasoActual As Integer = 0

        objVisita = ObtenerDetalleVisita()
        objUsuario = objRegistro.InsertaUsuario(IdentificadorUsuario)

        Dim objNegVisita As New NegocioVisita(objVisita, objUsuario, Nothing, Nothing)

        ''RECUPERAR LA VISITA
        If objVisita.ExisteVisita Then

            If objVisita.FechaAcuerdoVul Is String.Empty Then
                If AccesoBD.ActualizaFechaVulnerabilidad(IdVisitaSepris, FechaVulnera) Then
                    objVisita.FechaAcuerdoVul = FechaVulnera.ToString("dd/MM/yyyy")
                    objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_VULNERABILIDAD_NOTIFICAR_FECHA, objVisita, True, True, True, , True)
                Else
                    liError = 417
                End If
            Else
                If AccesoBD.ActualizaFechaVulnerabilidad(IdVisitaSepris, FechaVulnera) Then
                    objVisita.FechaAcuerdoVul = FechaVulnera.ToString("dd/MM/yyyy")
                    objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_VULNERABILIDAD_NOTIFICAR_CAMBIO_FECHA, objVisita, True, True, True, , True)
                Else
                    liError = 417
                End If
            End If

        End If

        objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, liError, "ActualizaNuevaFechaVulnera: " & liPasoActual.ToString(), Comentarios)

        Return liError
    End Function

    Public Function ActualizaNuevaFecha() As Integer
        Dim objVisita As Visita
        Dim objUsuario As Entities.Usuario
        Dim objRegistro As New Registro
        Dim liError As Integer = -1
        Dim liPasoActual As Integer = 0

        objVisita = ObtenerDetalleVisita()
        ''RECUPERAR LA VISITA
        If objVisita.ExisteVisita Then
            liPasoActual = objVisita.IdPasoActual

            ''REGISTRAR/OBTENER USUARIO
            objUsuario = objRegistro.InsertaUsuario(IdentificadorUsuario)
            If objUsuario.Existe Then
                Dim objNegVisita As New NegocioVisita(objVisita, objUsuario, Nothing, Comentarios)

                liError = ActualizaFecha()

                If liError = -1 Then
                    ''VALIDA LA FEHCHA DE ENTRADA
                    Select Case TipoFecha
                        Case Constantes.TipoFecha.FechaReunionPresi ''Fecha en que se realizará la presentación del paso 8
                            Dim objCorreo As New Entities.Correo(Constantes.CORREO_CAMBIO_FECHA_EXTERNA_INTERNA)

                            objCorreo.Asunto = objCorreo.Asunto.Replace("[TIPO_FECHA]", "interna")
                            objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[FECHA_REUNION_ANT]", objVisita.FECH_REUNION_PRESIDENCIA).
                                                                Replace("[FECHA_REUNION_ACTUAL]", Fecha.ToString("dd/MM/yyyy")).
                                                                Replace("[TIPO_FECHA]", "interna")

                            objVisita.FechaReunionPresidencia = Fecha
                            objNegVisita.getObjNotificacion().NotificarCorreo(objCorreo, objVisita, True, True, True, , True)

                            'objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesRealizados,
                            '                                        False, -1,
                            '                                        False, -1, -1,
                            '                                        True, Constantes.CORREO_CAMBIO_FECHA_EXTERNA_INTERNA,
                            '                                        False, True, False, 8, Fecha.ToString("dd/MMYyyy"))

                            objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, liError, "Envia notificación de fecha actualizada en paso: 8", Comentarios)

                        Case Constantes.TipoFecha.FechaReunionAfore '' Fecha de la reunión para la presentación paso 13
                            Dim objCorreo As New Entities.Correo(Constantes.CORREO_CAMBIO_FECHA_EXTERNA_INTERNA)

                            objCorreo.Asunto = objCorreo.Asunto.Replace("[TIPO_FECHA]", "externa")
                            objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[FECHA_REUNION_ANT]", objVisita.FECH_REUNION_AFORE).
                                                                Replace("[FECHA_REUNION_ACTUAL]", Fecha.ToString("dd/MM/yyyy")).
                                                                Replace("[TIPO_FECHA]", "externa")
                            objNegVisita.getObjNotificacion().NotificarCorreo(objCorreo, objVisita, True, True, True, , True)

                            objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, liError, "Envia notificación de fecha actualizada en paso: 13", Comentarios)

                            'objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesRealizados,
                            '                                        False, -1,
                            '                                        False, -1, -1,
                            '                                        True, Constantes.CORREO_CAMBIO_FECHA_EXTERNA_INTERNA,
                            '                                        False, True, False, 13, Fecha.ToString("dd/MMYyyy"))

                    End Select
                End If
            Else
                liError = 414
            End If
        Else
            liError = 413
        End If

        objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, liError, "ActualizaNuevaFecha paso: " & liPasoActual.ToString(), Comentarios)

        Return liError
    End Function

    ''' <summary>
    ''' SOLO SE MANDA LLAMAR CUANDO LA VISITA NO CAMBIA DE PASO, DE LO CONTRARIO SE MANDA LLAMAR EN AUTOMATICO
    ''' </summary>
    ''' <param name="objVisita"></param>
    ''' <param name="piPasoSiguiente"></param>
    ''' <param name="piEstatusPasoSiguiente"></param>
    ''' <param name="ppObservaciones"></param>
    ''' <remarks></remarks>
    'Public Sub ActualizaEstatusSisvig(objVisita As Visita, piPasoSiguiente As Integer, piEstatusPasoSiguiente As Integer, ppObservaciones As String, idUser As String, descripcionOpera As String)
    Public Sub ActualizaEstatusSisvig(objVisita As Visita, piPasoSiguiente As Integer, piEstatusPasoSiguiente As Integer, ppObservaciones As String)
        'Actualiza paso 2 en BSIS_X_C_PASO_SUPERVISAR_VISITA
        If objVisita.VisitaSisvig Then
            Dim visitaSisvig As New Entities.Sisvig() 'MCS 

            'GUARDA BITÁCORA DE ACTIVIDADES SISVIG-SEPRIS
            'visitaSisvig.GuardaBitacoraSisvigSepris(objVisita.IdVisitaGenerado, idUser, descripcionOpera, piPasoSiguiente)

            visitaSisvig.NotificaSisvig(objVisita.IdVisitaGenerado, piPasoSiguiente, piEstatusPasoSiguiente, ppObservaciones)
        End If
    End Sub

    Public Sub InsertaBitacoraSisvigSepris(IdVisita As Integer, piPasoSiguiente As Integer, idUser As String, descripcionOpera As String)

        Dim visitaSisvig As New Entities.Sisvig() 'MCS 

        'GUARDA BITÁCORA DE ACTIVIDADES SISVIG-SEPRIS
        If IdVisitaSepris > 0 Then
            visitaSisvig.GuardaBitacoraSisvigSepris(IdVisita, idUser, descripcionOpera, piPasoSiguiente)
        End If

    End Sub


    Public Function DocumentosObligatoriosSinCargar(piIdPaso As Integer, Optional validarDoctosPaso6 As String = "") As Boolean
        Dim lstDocMin As List(Of Documento.DocumentoMini)
        Dim objVisita As Visita
        objVisita = ObtenerDetalleVisita()

        If validarDoctosPaso6 Is Nothing Then
            validarDoctosPaso6 = ""
        End If

        If validarDoctosPaso6.Equals("NoValidarPaso6") Then
            lstDocMin = AccesoBD.ObtenerDocumentosObligatorios(IdVisita, 6, Constantes.Todos, Constantes.Obligatorio.Obligatorios, Constantes.TipoArchivo.TODOS)

            If lstDocMin.Count > 0 Then
                Return True
            Else
                Return False
            End If
        ElseIf Not piIdPaso.Equals(6) Then
            lstDocMin = AccesoBD.ObtenerDocumentosObligatorios(IdVisita, piIdPaso, Constantes.Todos, Constantes.Obligatorio.Obligatorios, Constantes.TipoArchivo.TODOS)

            objVisita.DocumentoRevisionPasoSeis = ""

            If lstDocMin.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End If

    End Function

    ''' <summary>
    ''' Cancela una visita de sepris
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CancelarVisita() As Integer
        Dim liError As Integer = -1

        Dim objNegVisita As NegocioVisita
        Dim objVisita As Visita
        objVisita = ObtenerDetalleVisita()

        Dim objRegistro As New Registro
        Dim objUsuario As Entities.Usuario

        If objVisita.ExisteVisita Then
            objUsuario = objRegistro.InsertaUsuario(IdentificadorUsuario)
            objNegVisita = New NegocioVisita(objVisita, objUsuario, Nothing, MotivoProrroga)

            If Me.Comentarios.Trim() <> String.Empty Then

                Dim con As Conexion.SQLServer = Nothing
                Dim tran As SqlClient.SqlTransaction = Nothing
                Dim guardo As Boolean = False
                Dim idVistaCancelar As Integer

                Try
                    con = New Conexion.SQLServer()
                    tran = con.BeginTransaction()

                    idVistaCancelar = IdVisita
                    Dim motivoCancelacion As String = Comentarios

                    If AccesoBD.cancelarVisita(idVistaCancelar, motivoCancelacion, con, tran, IdentificadorUsuario) = True Then
                        guardo = True
                    End If

                Catch ex As Exception
                    guardo = False
                    Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "OperacionesVisita.vb, CancelarVisita", "")
                    liError = 425
                Finally
                    If Not IsNothing(tran) Then
                        If guardo Then
                            'Cancelación exitosa
                            tran.Commit()

                            objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_CANCELACION, objVisita, True, True, False, , True)
                        Else
                            tran.Rollback()
                            liError = 2131
                        End If

                        tran.Dispose()
                    End If

                    If Not IsNothing(con) Then
                        con.CerrarConexion()
                        con = Nothing
                    End If
                End Try
            Else
                liError = 2129
            End If
        Else
            liError = 413
        End If

        objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, liError, "Cancela visita", Comentarios)

        Return liError
    End Function

End Class
