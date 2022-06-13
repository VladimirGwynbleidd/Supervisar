' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de clase "wcfRegistro" en el código, en svc y en el archivo de configuración a la vez.
' NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione wcfRegistro.svc o wcfRegistro.svc.vb en el Explorador de soluciones e inicie la depuración.
Imports SEPRIS.Visita

Public Class wcfRegistro
   Implements IwcfRegistro
   Public Property pvVisita As Visita
   Public Property Existe As Boolean = False

   Public Function InsertaVisita(IdVisitaSisvig As Integer,
                                   IdentificadorUsuario As String,
                                   pdFechaInicio As DateTime,
                                   piIdEntidad As Integer,
                                   psNombreEntidad As String,
                                   piTipoVisita As Integer,
                                   psLstObjetoVisita As List(Of Integer),
                                   psDscOtroObjVisita As String,
                                   psSupervisor As String,
                                   psLstInspector As List(Of String),
                                   psDescripcionVisita As String,
                                   psComentarios As String,
                                   psOrdenVisita As String,
                                   pbHayRevicioVul As Boolean,
                                   Optional pdFechaVul As DateTime = Nothing,
                                   Optional IdVisitaSepris As Integer = 0) As List(Of String) Implements IwcfRegistro.InsertaVisita
      Dim liError As Integer = -1
      Dim objAux As New Auxiliares
      Dim psFolioVisita As String = objAux.GeneraFolioVisita(piIdEntidad, Constantes.AREA_VO, psNombreEntidad)
      Dim pdFechaRegistro As DateTime = DateTime.Today
      Dim lstResultado As New List(Of String)
      Dim objRegistrar As New Registro
      Dim ldFecha As DateTime = pdFechaVul
      Dim guardo As Boolean = False
      Dim listCampos As New List(Of String)
      Dim listValores As New List(Of Object)

      If IdVisitaSepris > 0 Then
         Try
            Dim objOpVisita As OperacionesVisita
            Dim objVisita As SEPRIS.Visita

            objOpVisita = New OperacionesVisita With {.IdVisita = IdVisitaSepris, .Fecha = ldFecha}
            objVisita = objOpVisita.ObtenerDetalleVisita()

            liError = objOpVisita.ActualizaNuevaFechaVulnera(IdVisitaSepris, ldFecha, objVisita.FechaRegistro)

            lstResultado.Add(liError.ToString())

         Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro.csv.vb, Error general al registrar desde sisvig", "")
         End Try

      Else
         Try

            ''CONVERTIR EL TIPO DE VISITA
            piTipoVisita = objAux.HomologaTipoVisita(piTipoVisita)

            ''CONVERTIR EL OBJETO VISITA
            psLstObjetoVisita = objAux.HomologaObjetoVisita(psLstObjetoVisita)

            ''VALIDA LOS PARAMETROS
            liError = objAux.datosCapturadosValidos(IdVisitaSisvig, IdentificadorUsuario, psFolioVisita,
                                             pdFechaInicio, piIdEntidad, piTipoVisita, psLstObjetoVisita,
                                             psDscOtroObjVisita, psSupervisor, psLstInspector, Constantes.Verdadero)

            ''Regresa DateTime.MinValue si alguna validacion falla
            If pbHayRevicioVul And liError = -1 Then

               If ldFecha <> Date.MinValue Then
                  liError = objAux.ValidarFechaGeneral(ldFecha, False, DateTime.Today.Date, False, False, False, False)
               End If

            End If

            ''VALIDA SI NO HUBO ERRORES
            If liError = -1 Then
               Dim con As Conexion.SQLServer = Nothing
               Dim tran As SqlClient.SqlTransaction = Nothing

               Dim objUsuario As New Entities.Usuario(IdentificadorUsuario)
               Dim lsSesionSisvig As String = DateTime.Now.ToString("ddMMyyyyhhmmss")
               objUsuario.RegistrarSesionSisvig(lsSesionSisvig)

               objRegistrar.objVisita.IdArea = Constantes.AREA_VO
               objRegistrar.objVisita.Usuario = objUsuario
               objRegistrar.objVisita.FolioVisita = psFolioVisita
               objRegistrar.objVisita.FechaInicioVisita = pdFechaInicio
               objRegistrar.objVisita.IdEntidad = piIdEntidad
               objRegistrar.objVisita.NombreEntidad = psNombreEntidad
               objRegistrar.objVisita.IdTipoVisita = piTipoVisita
               objRegistrar.objVisita.DscObjetoVisitaOtro = psDscOtroObjVisita
               objRegistrar.objVisita.LstInspectoresAsignados = objAux.ConvierteInspectores(psLstInspector)
               objRegistrar.objVisita.LstSupervisoresAsignados = objAux.ConvierteSupervisores(psSupervisor)
               objRegistrar.objVisita.IdVisitaSisvig = IdVisitaSisvig
               objRegistrar.objVisita.DescripcionVisita = psDescripcionVisita
               objRegistrar.objVisita.ComentariosIniciales = psComentarios
               objRegistrar.objVisita.OrdenVisita = psOrdenVisita

               Try
                  con = New Conexion.SQLServer()
                  tran = con.BeginTransaction()

                  ''Guardar la visita padre
                  guardo = False

                  ''VALIDA QUE LOS USUARIOS QUE VIENEN EXISTAN EN SEPRIS
                  If objRegistrar.InsertaUsuarios(IdentificadorUsuario, psSupervisor, psLstInspector) Then
                     ''Registrar la visita inicial
                     If objRegistrar.registrarVisitaPadre(con, tran) = True Then
                        If objRegistrar.registrarObjetoVisitaReg(psLstObjetoVisita, con, tran) Then
                           If objRegistrar.registrarSupervisores(con, tran) = True And objRegistrar.registrarInspectores(con, tran) = True Then
                              If objRegistrar.registrarPasoUno(con, tran) = True Then
                                 If objRegistrar.registrarEstatusPaso(con, tran) = True Then
                                    ''REGISTRAR FECHA VULNEREBILIDAD                        
                                    If pbHayRevicioVul Then
                                       If AccesoBD.ActualizaFechaInicioVisitaConTransaccion(objRegistrar.objVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaVulneravilidades, con, tran, "") Then
                                          guardo = True
                                       Else
                                          liError = 417
                                       End If
                                    Else
                                       AccesoBD.ActualizaEstatusVulneraConTransaccion(objRegistrar.objVisita.IdVisitaGenerado, con, tran)
                                       guardo = True
                                    End If

                                    If IdentificadorUsuario.Trim().Length > 0 And guardo Then
                                       Dim bitacora As New Conexion.Bitacora("Registro visita(" & objRegistrar.objVisita.IdVisitaGenerado.ToString() & ")", lsSesionSisvig, IdentificadorUsuario)
                                       bitacora.Finalizar(True)
                                    End If
                                 Else
                                    liError = 409
                                 End If
                              Else
                                 liError = 408
                              End If
                           Else
                              liError = 407
                           End If
                        Else
                           liError = 406
                        End If
                     Else
                        liError = 405
                     End If
                  Else
                     liError = 411
                  End If
               Catch ex As Exception
                  'Registro fallido
                  liError = 403
                  guardo = False
                  Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro.csv.vb, Error al registrar desde sisvig", "")
               Finally
                  If Not IsNothing(tran) Then
                     If guardo Then
                        'Registro exitoso
                        tran.Commit()

                        ''Actualiza el folio de la visita en el objeto visita
                        objRegistrar.objVisita.Fecha_LimitePasoActual = AccesoBD.ObtenFechaLimiteAtender_Paso(objRegistrar.objVisita.IdVisitaGenerado, PasoProcesoVisita.Pasos.Uno)

                        ''Eliminar Documentos temporales y pasar a tabla operativa
                        ''No toma en cuanta el usuario porque la consulta solo es por visita sisvig
                        If Not AccesoBD.MigrarDocumentosSinVisita(objRegistrar.objVisita.IdVisitaGenerado, "", PasoProcesoVisita.Pasos.Uno, Constantes.Todos, IdVisitaSisvig) Then
                           liError = 404
                        Else
                           'Notifica al supervisor asignado a la visita
                           If liError = -1 Then
                              Try
                                 If Not IsNothing(objRegistrar.objVisita.Usuario) Then
                                    Dim objNotif As New NotificacionesVisita(objRegistrar.objVisita.Usuario, objRegistrar.objVisita.ComentariosIniciales)

                                    Dim mensaje As String = objNotif.NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_REGISTRO, objRegistrar.objVisita, True, False, False, Nothing, True)

                                    If mensaje <> Constantes.CORREO_ENVIADO_OK Then
                                       liError = 412
                                    End If

                                    'Si hay revision de vulnerabilidad
                                    If pbHayRevicioVul Then
                                       Dim mensaje2 As String = objNotif.NotificarCorreo(Constantes.CORREO_VULNERABILIDAD_NOTIFICAR_SANDRA, objRegistrar.objVisita, True, False, False, Nothing, True)
                                       If mensaje2 <> Constantes.CORREO_ENVIADO_OK Then
                                          liError = 2166
                                       End If
                                    End If

                                 End If
                              Catch
                                 liError = 410
                              End Try
                           End If

                           ''AVANZAR A PASO 2, SI NO HAY ERROR
                           If liError = -1 Then
                              Dim objOpVisita As New OperacionesVisita() With {.IdVisita = objRegistrar.objVisita.IdVisitaGenerado, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = psComentarios}
                              liError = objOpVisita.Avanza()
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
            End If

            ''LLENA EL RESULTADO
            lstResultado.Add(liError.ToString())

            If Not guardo Then
               lstResultado.Add("-1")
               lstResultado.Add("-1")
            Else
               lstResultado.Add(objRegistrar.objVisita.IdVisitaGenerado)
               lstResultado.Add(objRegistrar.objVisita.FolioVisita)
            End If

         Catch ex As Exception
            lstResultado.Add("403")
            lstResultado.Add("-1")
            lstResultado.Add("-1")
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro.csv.vb, Error general al registrar desde sisvig", "")
         End Try
      End If
      Return lstResultado
   End Function

   Public Function EditaVisita(IdVisitaSepris As Integer,
                               IdentificadorUsuario As String,
                               pdFechaInicio As DateTime,
                               piTipoVisita As Integer,
                               psLstObjetoVisita As List(Of Integer),
                               psSupervisor As String,
                               psLstInspector As List(Of String),
                               psDescripcionVisita As String,
                               psComentarios As String,
                               psOrdenVisita As String,
                               Optional pdFechaVul As DateTime = Nothing) As List(Of String) Implements IwcfRegistro.EditaVisita

      Dim con As Conexion.SQLServer = Nothing
      Dim tran As SqlClient.SqlTransaction = Nothing
      Dim guardo As Boolean = False
      Dim psIdVisita As String = ""
      Dim objRegistrar As New Registro
      Dim liError As Integer = -1
      Dim objAux As New Auxiliares
      Dim objOpVisita As New OperacionesVisita
      Dim lstResultado As New List(Of String)

      Try

         ''CONVERTIR EL TIPO DE VISITA
         piTipoVisita = objAux.HomologaTipoVisita(piTipoVisita)

         ''CONVERTIR EL OBJETO VISITA
         psLstObjetoVisita = objAux.HomologaObjetoVisita(psLstObjetoVisita)

         Dim objUsuario As New Entities.Usuario(IdentificadorUsuario)

         objOpVisita.IdVisita = IdVisitaSepris
         objRegistrar.objVisita = objOpVisita.ObtenerDetalleVisita()

         If objRegistrar.objVisita.ExisteVisita Then
            objRegistrar.objVisita.IdArea = Constantes.AREA_VO
            objRegistrar.objVisita.Usuario = objUsuario
            objRegistrar.objVisita.FechaInicioVisita = pdFechaInicio
            objRegistrar.objVisita.IdTipoVisita = piTipoVisita
            objRegistrar.objVisita.LstInspectoresAsignados = objAux.ConvierteInspectores(psLstInspector)
            objRegistrar.objVisita.LstSupervisoresAsignados = objAux.ConvierteSupervisores(psSupervisor)
            objRegistrar.objVisita.DescripcionVisita = psDescripcionVisita
            objRegistrar.objVisita.ComentariosIniciales = IIf(psComentarios.Trim().Length > 0, psComentarios, objRegistrar.objVisita.ComentariosIniciales)
            objRegistrar.objVisita.OrdenVisita = psOrdenVisita

            con = New Conexion.SQLServer()
            tran = con.BeginTransaction()

            ''VALIDA QUE LOS USUARIOS QUE VIENEN EXISTAN EN SEPRIS
            If objRegistrar.InsertaUsuarios(IdentificadorUsuario, psSupervisor, psLstInspector) Then
               objRegistrar.objVisita.IdVisitaGenerado = AccesoBD.registrarVisita(objRegistrar.objVisita, con, tran, psIdVisita, Constantes.OPERCION.Actualizar)

               If objRegistrar.objVisita.IdVisitaGenerado > 0 Then
                  If objRegistrar.registrarObjetoVisitaReg(psLstObjetoVisita, con, tran) Then
                     If objRegistrar.registrarSupervisores(con, tran) = True And objRegistrar.registrarInspectores(con, tran) = True Then
                        guardo = True
                     Else
                        liError = 407
                     End If
                  Else
                     liError = 406
                  End If
               Else
                  liError = 405
               End If
            Else
               liError = 411
            End If
         Else
            liError = 413
         End If
      Catch ex As Exception
         'Registro fallido
         guardo = False
         liError = 2122
      Finally
         If Not IsNothing(tran) Then
            If guardo Then
               'Registro exitoso
               tran.Commit()

               ''Actualiza el folio de la visita en el objeto visita
               objRegistrar.objVisita.FolioVisita = psIdVisita
               objRegistrar.objVisita.Fecha_LimitePasoActual = AccesoBD.ObtenFechaLimiteAtender_Paso(objRegistrar.objVisita.IdVisitaGenerado, PasoProcesoVisita.Pasos.Uno)

               Dim objUsuario As New Entities.Usuario(IdentificadorUsuario)

               If Not IsNothing(objUsuario) Then
                  'Notifica al supervisor asignado a la visita
                  Dim objNegVisita As New NegocioVisita(objRegistrar.objVisita, objUsuario, Nothing, "Edición: " & psComentarios)
                  ''Notificar la edicion y guardar es nuevo comentario
                  objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objRegistrar.objVisita.IdEstatusActual, , , , , ,
                                                                          True, Constantes.CORREO_EDICION_VISITA)
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

      ''LLENA EL RESULTADO
      lstResultado.Add(liError.ToString())

      objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, liError, "Solicitud de Prorroga", psComentarios)

      Return lstResultado
   End Function

   'Public Function ActualizaFechaVulnera(IdVisitaSepris As Integer, Fecha As DateTime) As List(Of String) Implements IwcfRegistro.ActualizaFechaVulnera
   '    Dim lstResultado As New List(Of String)
   '    Dim objOpVisita As OperacionesVisita

   '    objOpVisita = New OperacionesVisita With {.IdVisitaSepris = IdVisitaSepris, .Fecha = Fecha}

   '    lstResultado.Add(objOpVisita.ActualizaNuevaFechaVulnera(IdVisitaSepris, Fecha))

   '    Return lstResultado
   'End Function

   Public Function AvanzaPaso(IdVisita As Integer,
                               IdentificadorUsuario As String,
                               psComentarios As String,
                               Optional opcNotifica As Integer = 0,
                               Optional idDocumento As Integer = 0,
                               Optional versionDocto As Integer = 0,
                               Optional nombreDocto As String = "") As List(Of String) Implements IwcfRegistro.AvanzaPaso

      Dim lstResultado As New List(Of String)
      Dim objOpVisita As OperacionesVisita

      Dim objVisita As Visita

      objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = psComentarios}

      objVisita = objOpVisita.ObtenerDetalleVisita()
      Dim objUsuario As Entities.Usuario
      Dim objRegistro As New Registro
      Dim Documento As New Documento
      Dim peObjExp As New Expediente
      objUsuario = objRegistro.InsertaUsuario(IdentificadorUsuario)
      Dim objNegVisita As New NegocioVisita(objVisita, objUsuario, Nothing, "")

      'If idDocumento > 1 Then
      '    ''BUSCA ARCHIVO EN EXPEDIENTE
      '    objVisita.DocumentoRevisionPasoSeis = peObjExp.ConsultaNombreDocumentos(6, IdVisita, idDocumento, versionDocto).ToString()
      objVisita.DocumentoRevisionPasoSeis = nombreDocto.ToString()
      objOpVisita.validaDoctosPaso6 = nombreDocto.ToString()
      'End If

      'NOTIFICACION DE REVISIÓN DE DOCUMENTOS PASO 6
      If opcNotifica = 1 Then
         objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesRealizados,
                                                                 False, -1,
                                                                 False, -1, -1,
                                                                 True, Constantes.CORREO_DOCUMENTOS_REVISION,
                                                                 False, True, False)

         Documento.ActualizaEstatusDocumento(IdVisita, idDocumento, Constantes.EstatusPaso.Inicia_revision, versionDocto)

         lstResultado.Add(-1)

         'NOTIFICACION DE VERSIÓN FINAL DE DOCUMENTOS PASO 6
      ElseIf opcNotifica = 2 Then

         objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                 False, -1,
                                                                 False, -1, -1,
                                                                 True, Constantes.CORREO_DOCUMENTOS_NOTIFICAR_CORRECTO,
                                                                 False, True, False)

         Documento.ActualizaEstatusDocumento(IdVisita, idDocumento, Constantes.EstatusPaso.Revisado, versionDocto)

         lstResultado.Add(-1)

      Else
         lstResultado.Add(objOpVisita.Avanza())
      End If


      Return lstResultado
   End Function

   Public Function RechazaPaso(IdVisita As Integer,
                               IdentificadorUsuario As String,
                               psComentarios As String) As List(Of String) Implements IwcfRegistro.RechazaPaso
      Dim lstResultado As New List(Of String)
      Dim objOpVisita As OperacionesVisita

      objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = psComentarios}
      lstResultado.Add(objOpVisita.Rechaza())

      Return lstResultado
   End Function

   Public Function ActualizaFecha(IdVisita As Integer, IdentificadorUsuario As String, psComentarios As String, Fecha As DateTime, TipoFecha As Constantes.TipoFecha,
                           Optional NotificarCambio As Boolean = True) As List(Of String) Implements IwcfRegistro.ActualizaFecha
      Dim lstResultado As New List(Of String)
      Dim objOpVisita As OperacionesVisita

      objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = psComentarios, .Fecha = Fecha, .TipoFecha = TipoFecha}

      If Not NotificarCambio Then
         lstResultado.Add(objOpVisita.ActualizaFecha())
      Else
         lstResultado.Add(objOpVisita.ActualizaNuevaFecha())
      End If

      Return lstResultado
   End Function

   'Public Function ActualizaFecha(IdVisita As Integer, IdentificadorUsuario As String, psComentarios As String, Fecha As DateTime, TipoFecha As Constantes.TipoFecha) As List(Of String) Implements IwcfRegistro.ActualizaFechaSinAccion
   '    Dim lstResultado As New List(Of String)
   '    Dim objOpVisita As OperacionesVisita

   '    objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = psComentarios, .Fecha = Fecha, .TipoFecha = TipoFecha}
   '    lstResultado.Add(objOpVisita.ActualizaNuevaFecha())

   '    Return lstResultado
   'End Function

   Function FinalizaCargaDocumentos(IdVisita As Integer,
                            IdentificadorUsuario As String,
                            psCadenaParametros As String,
                            psComentarios As String) As List(Of String) Implements IwcfRegistro.FinalizaCargaDocumentos
      Dim lstResultado As New List(Of String)
      Dim objOpVisita As OperacionesVisita

      objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = psComentarios}
      lstResultado.Add(objOpVisita.FinalizaCargaDocumentos(psCadenaParametros))

      Return lstResultado
   End Function

   Function SolicitaProrroga(IdVisita As Integer,
                            IdentificadorUsuario As String,
                            motivoProrroga As String) As List(Of String) Implements IwcfRegistro.SolicitaProrroga
      Dim lstResultado As New List(Of String)
      Dim objOpVisita As OperacionesVisita

      objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .MotivoProrroga = motivoProrroga}
      lstResultado.Add(objOpVisita.SolicitaProrroga(motivoProrroga))

      Return lstResultado
   End Function


   Public Function AvanzaPasoSiete(IdVisita As Integer,
                               IdentificadorUsuario As String,
                               psComentarios As String,
                               pbFlagReunHallazgos As Boolean,
                               pbFlagSancion As Boolean) As List(Of String) Implements IwcfRegistro.AvanzaPasoSiete
      Dim lstResultado As New List(Of String)
      Dim objOpVisita As OperacionesVisita

      objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = psComentarios}
      objOpVisita.FlagReunionHallazgos = pbFlagReunHallazgos
      objOpVisita.FlagSancion = pbFlagSancion

      lstResultado.Add(objOpVisita.Avanza())

      Return lstResultado
   End Function

   Public Function AvanzaPasoOcho(IdVisita As Integer,
                               IdentificadorUsuario As String,
                               psComentarios As String,
                               pbFlagSancion As Boolean,
                               Optional pbPrimeraNotificacion As Boolean = False) As List(Of String) Implements IwcfRegistro.AvanzaPasoOcho
      Dim lstResultado As New List(Of String)
      Dim objOpVisita As OperacionesVisita

      objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = psComentarios}
      objOpVisita.FlagSancion = pbFlagSancion
      objOpVisita.PrimeraNotificacionOcho = pbPrimeraNotificacion

      lstResultado.Add(objOpVisita.Avanza())

      Return lstResultado
   End Function

   'Function ObtenerDetalleVisita(IdVisita As Integer) As Visita Implements IwcfRegistro.ObtenerDetalleVisita
   '    Dim objOpVisita As OperacionesVisita

   '    objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita}

   '    Return objOpVisita.ObtenerDetalleVisita()
   'End Function

   Function getErrorValidacionSupervisar(ByVal id_error As String) As String Implements IwcfRegistro.getErrorValidacionSupervisar
      Return AccesoBD.getErrorValidacionSupervisar(id_error)
   End Function

   Function CancelarVisita(IdVisita As Integer,
                            IdentificadorUsuario As String,
                            motivoCancela As String) As List(Of String) Implements IwcfRegistro.CancelarVisita
      Dim lstResultado As New List(Of String)
      Dim objOpVisita As OperacionesVisita

      objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = motivoCancela}
      lstResultado.Add(objOpVisita.CancelarVisita())

      Return lstResultado
   End Function

   Function ConsultaDocumentosObligatoriosSinCargarSinVisita(IdVisitaSisvig As Integer) As Boolean Implements IwcfRegistro.ConsultaDocumentosObligatoriosSinCargarSinVisita
      Dim objOpVisita As New Auxiliares

      Return objOpVisita.ConsultaDocumentosObligatoriosSinCargarPorPasoUsuario("", PasoProcesoVisita.Pasos.Uno, Constantes.Obligatorio.Obligatorios, , , IdVisitaSisvig)
   End Function
End Class
