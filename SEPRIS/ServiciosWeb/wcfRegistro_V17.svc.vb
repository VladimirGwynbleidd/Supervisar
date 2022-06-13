' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de clase "wcfRegistro_V17" en el código, en svc y en el archivo de configuración a la vez.
' NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione wcfRegistro_V17.svc o wcfRegistro_V17.svc.vb en el Explorador de soluciones e inicie la depuración.
Imports SEPRIS.Visita

Imports Utilerias

Public Class wcfRegistro_V17
   Implements IwcfRegistro_V17
   Public Property pvVisita As Visita
   Public Property Existe As Boolean = False

   Public Function Notificar(ByVal idVisitaSEPRIS As Integer, ByVal idCorreo As Integer, _
                             ByVal idUsuario As String, _
                             Optional pbAreaVoVf As Boolean = False,
                                    Optional pbAreaVj As Boolean = False,
                                    Optional pbAreaPresidencia As Boolean = False,
                                    Optional lstPersonasDestinatarios As List(Of Persona) = Nothing,
                                    Optional pbSuperUsuarios As Boolean = False) As List(Of String) Implements IwcfRegistro_V17.Notificar

      Dim liError As Integer = -1
      Dim listErrs As New List(Of String)
      Dim objRegistro As New Registro

      Dim objUsuario As Entities.Usuario
      objUsuario = objRegistro.InsertaUsuario(idUsuario)
      If Not objUsuario.Existe Then
         listErrs.Add("414")
         Return listErrs
      End If

      Try
         Dim objVisita As Visita
         objVisita = OperacionesVisita.ObtenerDetalleVisita(idVisitaSEPRIS, Constantes.AREA_VO)

         Dim objNotif As New NotificacionesVisita(objUsuario, objVisita.ComentariosIniciales)
         Dim mensaje2 As String = Constantes.CORREO_ENVIADO_OK

#If Not Debuug Then
         mensaje2 = objNotif.NotificarCorreo(idCorreo, objVisita, pbAreaVoVf, pbAreaVj, pbAreaPresidencia, lstPersonasDestinatarios, pbSuperUsuarios)
#End If

         If mensaje2 <> Constantes.CORREO_ENVIADO_OK Then
            liError = 2166
         End If

      Catch ex As Exception
         liError = 410
      End Try
      listErrs.Add(liError)

      Return listErrs
   End Function


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
                                   pdFechaVul As DateTime,
                                   pbHabraRevicionVul As Boolean) As List(Of String) Implements IwcfRegistro_V17.InsertaVisita
      Dim liError As Integer = -1
      Dim objAux As New Auxiliares_V17
      Dim psFolioVisita As String = objAux.GeneraFolioVisita(piIdEntidad, Constantes.AREA_VO, psNombreEntidad)
      Dim pdFechaRegistro As DateTime = DateTime.Today
      Dim lstResultado As New List(Of String)
      Dim objRegistrar As New Registro_V17
      Dim ldFecha As DateTime = pdFechaVul
      Dim guardo As Boolean = False
      Dim listCampos As New List(Of String)
      Dim listValores As New List(Of Object)

      Try

         ''CONVERTIR EL TIPO DE VISITA
         piTipoVisita = objAux.HomologaTipoVisita(piTipoVisita)

         ''CONVERTIR EL OBJETO VISITA
         psLstObjetoVisita = objAux.HomologaObjetoVisita(psLstObjetoVisita)

         ''VALIDA LOS PARAMETROS
         liError = objAux.datosCapturadosValidos(IdVisitaSisvig, IdentificadorUsuario, psFolioVisita,
                                          pdFechaInicio, piIdEntidad, piTipoVisita, psLstObjetoVisita,
                                          psDscOtroObjVisita, psSupervisor, psLstInspector, Constantes.Verdadero)


         If liError < 0 Then
            ' Sin error al validar los datos

            If Not Fechas.Vacia(pdFechaVul) Then
               If pdFechaInicio < ldFecha Then
                  liError = 2167
               Else
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
               objRegistrar.objVisita.Fecha_AcuerdoVul = pdFechaVul
               objRegistrar.objVisita.EstatusVulnerabilidad = IIf(Fechas.Vacia(pdFechaVul), pbHabraRevicionVul, True)

               Try
                  con = New Conexion.SQLServer()
                  tran = con.BeginTransaction()

                  ''Guardar la visita padre
                  guardo = False

                  ''VALIDA QUE LOS USUARIOS QUE VIENEN EXISTAN EN SEPRIS
                  If objRegistrar.InsertaUsuarios(IdentificadorUsuario, psSupervisor, psLstInspector) Then
                     ''Registrar la visita inicial
                     If objRegistrar.registrarVisitaPadre(con, tran) Then
                        If objRegistrar.registrarObjetoVisitaReg(psLstObjetoVisita, con, tran) Then
                           If objRegistrar.registrarSupervisores(con, tran) AndAlso objRegistrar.registrarInspectores(con, tran) = True Then
                              If objRegistrar.registrarPasoCero(con, tran) Then
                                 If objRegistrar.registrarPasoUno(con, tran) Then
                                    If objRegistrar.registrarEstatusPaso(con, tran) Then
                                       'Registro exitoso

                                       If Fechas.Vacia(pdFechaVul) Then
                                          AccesoBD.ActualizaEstatusVulneraConTransaccion(objRegistrar.objVisita.IdVisitaGenerado, IIf(pbHabraRevicionVul, 1, 0), con, tran)
                                       Else
                                          AccesoBD.ActualizaEstatusVulneraConTransaccion(objRegistrar.objVisita.IdVisitaGenerado, -1, con, tran)
                                       End If

                                       ' Registro de datos en SISVIG
                                       Dim vig As Entities.Sisvig = New Entities.Sisvig()

                                       If Entities.Sisvig.ActualizaSepris_Sisvig2(IdVisitaSisvig, objRegistrar.objVisita.IdVisitaGenerado, objRegistrar.objVisita.FolioVisita) Then
                                          If Entities.Sisvig.EstableceSesion(objRegistrar.objVisita.IdVisitaGenerado, Not Fechas.Vacia(pdFechaVul)) Then
                                             guardo = True
                                             tran.Commit()

                                             vig = Nothing
                                             If IdentificadorUsuario.Trim().Length > 0 And guardo Then
                                                Dim bitacora As New Conexion.Bitacora("Registro visita(" & objRegistrar.objVisita.IdVisitaGenerado.ToString() & ")", lsSesionSisvig, IdentificadorUsuario)
                                                bitacora.Finalizar(True)
                                             End If
                                          Else
                                             liError = 2201
                                          End If
                                       Else
                                          liError = 2201
                                       End If
                                    Else
                                       liError = 408
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
                  Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro_V17.csv.vb, Error al registrar desde sisvig", "")
               Finally
                  If Not IsNothing(tran) Then
                     If guardo Then

                        'Notifica al supervisor asignado a la visita
                        If liError = -1 Then
                           ''Actualiza el folio de la visita en el objeto visita
                           objRegistrar.objVisita.Fecha_LimitePasoActual = AccesoBD.ObtenFechaLimiteAtender_Paso(objRegistrar.objVisita.IdVisitaGenerado, PasoProcesoVisita.Pasos.Uno)

                           Try
                              If Not IsNothing(objRegistrar.objVisita.Usuario) Then
                                 Dim objNotif As New NotificacionesVisita(objRegistrar.objVisita.Usuario, objRegistrar.objVisita.ComentariosIniciales)

                                 Dim mensaje As String = objNotif.NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_VISITA_CREADA, objRegistrar.objVisita, True, False, False, Nothing, True)
                                 If mensaje <> Constantes.CORREO_ENVIADO_OK Then
                                    liError = 412
                                 End If


                                 If pbHabraRevicionVul And Fechas.Vacia(pdFechaVul) Then
                                    ' La sesion de vulnerabilidades sera realizará
                                    mensaje = objNotif.NotificarCorreo(Constantes.CORREO_SOLICITUD_REVISION_VULNERABILIDADES, objRegistrar.objVisita, True, False, False, Nothing, True)
                                    If mensaje <> Constantes.CORREO_ENVIADO_OK Then
                                       liError = 2166
                                    End If
                                 ElseIf Not Fechas.Vacia(pdFechaVul) Then
                                    ' La sesion de vulnerabilidades se realizó
                                    mensaje = objNotif.NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_REUNION_VULNERA_REALIZADA, objRegistrar.objVisita, True, False, False, Nothing, True)
                                    If mensaje <> Constantes.CORREO_ENVIADO_OK Then
                                       liError = 2166
                                    End If
                                 End If

                              End If
                           Catch
                              liError = 410
                           End Try
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

            If guardo Then
               lstResultado.Add(objRegistrar.objVisita.IdVisitaGenerado)
               lstResultado.Add(objRegistrar.objVisita.FolioVisita)
            End If
         Else
            lstResultado.Add(liError)
         End If
      Catch ex As Exception
         lstResultado.Add("403")
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro_V17.csv.vb, Error general al registrar desde sisvig", "")
      End Try

      Return lstResultado
   End Function

   Public Function Paso1(ByVal IdVisitaSEPRIS As Integer,
                         ByVal IdentificadorUsuario As String,
                         ByVal HaySesion As Boolean,
                         ByVal FechaSesVulnerabilidad As DateTime,
                         ByVal psComentarios As String,
                         ByVal cambioNormativa As Boolean) As List(Of String) Implements IwcfRegistro_V17.Paso1
      Dim liError As Integer = -1
      Dim objAux As New Auxiliares_V17
      Dim lstResultado As New List(Of String)
      Dim objRegistrar As New Registro_V17
      Dim guardo As Boolean = False
      Dim listErrs As New List(Of String)
      Dim listValores As New List(Of Object)
      Dim objVisita As Visita

      Try
         If liError < 0 Then
            ''VALIDA SI NO HUBO ERRORES
            If liError = -1 Then
               Dim con As Conexion.SQLServer = Nothing
               'Dim tran As SqlClient.SqlTransaction = Nothing

               Try
                  Dim objUsuario As New Entities.Usuario(IdentificadorUsuario)
                  Dim lsSesionSisvig As String = DateTime.Now.ToString("ddMMyyyyhhmmss")
                  objUsuario.RegistrarSesionSisvig(lsSesionSisvig)


                  con = New Conexion.SQLServer()
                  'tran = con.BeginTransaction()

                  objRegistrar.objVisita.IdVisitaGenerado = IdVisitaSEPRIS
                  objRegistrar.objVisita.IdPasoActual = 1 ' 1 - Elaborar Oficios y Acta Inicial usando requisitos de VJ y enviar a VJ - BDS_C_GR_PASOS
                  objRegistrar.objVisita.FechaRegistro = DateTime.Now
                  objRegistrar.objVisita.Usuario = objUsuario

                  guardo = False
                  ' Cargar datos de la visita
                  objVisita = OperacionesVisita.ObtenerDetalleVisita(IdVisitaSEPRIS, Constantes.AREA_VO)

                  If (HaySesion) Then
                     If Not Fechas.Vacia(FechaSesVulnerabilidad) Then
                        Dim objOpVisita As OperacionesVisita
                        objOpVisita = New OperacionesVisita With {.IdVisita = IdVisitaSEPRIS, .Fecha = FechaSesVulnerabilidad, .IdentificadorUsuario = IdentificadorUsuario}

                        liError = objOpVisita.ActualizaNuevaFechaVulnera(IdVisitaSEPRIS, FechaSesVulnerabilidad, objVisita.FechaRegistro)

                        'lstResultado.Add(liError.ToString())
                     End If
                  End If

                  If cambioNormativa Then
                     Dim Pars(1) As System.Data.SqlClient.SqlParameter
                     Dim Dt As DataTable
                     Dim Dr As DataRow
                     Dim Arcs As String = "."

                     con = New Conexion.SQLServer()

                     Pars(0) = New SqlClient.SqlParameter("@I_ID_VISITA", IdVisitaSEPRIS)
                     Pars(1) = New SqlClient.SqlParameter("@I_ID_PASO", 1)

                     Dt = con.EjecutarSPConsultaDT("sps_bds_grl_get_Documentos_v2_Supervisar_V17", Pars)

                     For Each Dr In Dt.Rows
                        Arcs &= Dr("T_NOM_Documento").ToString() & "."
                     Next

                     Pars(0) = New SqlClient.SqlParameter("@idVisitaSEPRIS", IdVisitaSEPRIS)
                     Pars(1) = New SqlClient.SqlParameter("@Archivos", Arcs)

                     Dt.Dispose()
                     Dt = con.EjecutarSPConsultaDT("dbo.SPI_BDS_GRL_VISITA_ARCHIVOSOBSOLETOS_GET", Pars)

                     Arcs = ""
                     For Each Dr In Dt.Rows
                        Arcs &= Dr("Archivo").ToString() & ", "
                     Next

                     If Arcs <> "" Then
                        liError = 2200
                        'listErrs.Add(Arcs.Substring(0, Arcs.Length - 1))
                     Else
                        liError = -1
                     End If
                  Else
                     liError = -1
                  End If

                  'lstResultado.Add(liError.ToString())

                  If liError = -1 And objVisita.IdPasoActual = 1 Then
                     listErrs = AvanzaPaso(IdVisitaSEPRIS, IdentificadorUsuario, psComentarios)
                     If listErrs(0) = -1 Then
                        guardo = True

                        Dim bitacora As New Conexion.Bitacora("Envio de Paso 1 desde SISVIG(" & objRegistrar.objVisita.IdVisitaGenerado.ToString() & ")", lsSesionSisvig, IdentificadorUsuario)
                        bitacora.Finalizar(True)
                     Else
                        liError = listErrs(0)
                     End If
                  End If

               Catch ex As Exception
                  'Registro fallido
                  liError = 403
                  guardo = False
                  Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro_V17.csv.vb, Error al registrar el paso 1", "")
               Finally
                  'If Not IsNothing(tran) Then
                  'If guardo Then

                  '   'Notifica al supervisor asignado a la visita
                  '   If liError = -1 Then
                  '      ''Actualiza el folio de la visita en el objeto visita
                  '      objRegistrar.objVisita.Fecha_LimitePasoActual = AccesoBD.ObtenFechaLimiteAtender_Paso(objRegistrar.objVisita.IdVisitaGenerado, PasoProcesoVisita.Pasos.Uno)

                  '      Try
                  '         If Not IsNothing(objRegistrar.objVisita.Usuario) Then
                  '            Dim objNotif As New NotificacionesVisita(objRegistrar.objVisita.Usuario, psComentarios)

                  '            If Not Fechas.Vacia(FechaSesVulnerabilidad) Then
                  '               Dim mensaje2 As String = objNotif.NotificarCorreo(Constantes.CORREO_VULNERABILIDAD_NOTIFICAR_SANDRA, objRegistrar.objVisita, True, False, False, Nothing, True)
                  '               If mensaje2 <> Constantes.CORREO_ENVIADO_OK Then
                  '                  liError = 2166
                  '               End If
                  '            End If
                  '         End If

                  '      Catch
                  '         liError = 410
                  '      End Try
                  '   End If

                  'Else
                  '   'Registro fallido
                  '   'tran.Rollback()
                  'End If
                  'tran.Dispose()
                  'End If

                  If Not IsNothing(con) Then
                     con.CerrarConexion()
                     con = Nothing
                  End If
               End Try
            End If

            ''LLENA EL RESULTADO
            lstResultado.Add(liError.ToString())

            If guardo Then
               lstResultado.Add(objRegistrar.objVisita.IdVisitaGenerado)
               lstResultado.Add(objRegistrar.objVisita.FolioVisita)
            End If
         Else
            lstResultado.Add(liError)
         End If
      Catch ex As Exception
         lstResultado.Add("403")
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro_V17.csv.vb, Error general al registrar desde sisvig", "")
      End Try

      Return lstResultado
   End Function

   Public Function Paso3(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, _
                         ByVal PrimeraVez As Boolean, _
                         ByVal Aceptado As Boolean, _
                         ByVal CambioNorma As Boolean, _
                         ByVal Comentarios As String) As List(Of String) Implements IwcfRegistro_V17.Paso3
      Dim liError As Integer = -1
      Dim objAux As New Auxiliares_V17
      Dim lstResultado As New List(Of String)
      Dim objRegistrar As New Registro_V17
      Dim guardo As Boolean = False
      Dim listErrs As New List(Of String)
      Dim objRegistro As New Registro

      Dim Arcs As String = ""
      Dim con As Conexion.SQLServer = Nothing
      Dim tran As SqlClient.SqlTransaction = Nothing

      Dim objUsuario As Entities.Usuario
      objUsuario = objRegistro.InsertaUsuario(Usr)
      If Not objUsuario.Existe Then
         listErrs.Add("414")
         Return listErrs
      End If

      Dim objVisita As Visita
      objVisita = OperacionesVisita.ObtenerDetalleVisita(idVisitaSEPRIS, Constantes.AREA_VO)

      Try

         Entities.Sisvig.SetCambioNorma(idVisitaSEPRIS, CambioNorma)

         'If PrimeraVez AndAlso Aceptado AndAlso CambioNorma Then
         If CambioNorma Then
            ' Primera vez que se pasa por el paso 3
            Dim Pars(1) As SqlClient.SqlParameter ' = New List(Of SqlClient.SqlParameter)()
            Dim Dt As DataTable
            Dim Dr As DataRow
            Arcs = ""

            con = New Conexion.SQLServer()
            tran = con.BeginTransaction

            Pars(0) = New SqlClient.SqlParameter("@I_ID_VISITA", idVisitaSEPRIS)
            Pars(1) = New SqlClient.SqlParameter("@I_ID_PASO", 1)

            Dt = con.EjecutarSPConsultaDTConTransaccion("sps_bds_grl_get_Documentos_v2_Supervisar_V17", Pars, tran)

            'Pars.Clear()
            'Pars.Add("@idVisitaSEPRIS")
            'Pars.Add("@Archivo")

            Dim Vals As List(Of Object) = New List(Of Object)
            Dim Noms As List(Of String) = New List(Of String)
            Noms.Add("@idVisitaSEPRIS")
            Noms.Add("@Archivo")
            Vals.Add(Nothing)
            Vals.Add(Nothing)
            For Each Dr In Dt.Rows
               Arcs = Dr("T_NOM_Documento").ToString()

               If (Arcs <> "") Then
                  Vals(0) = idVisitaSEPRIS  ' New SqlClient.SqlParameter("@idVisitaSEPRIS", idVisitaSEPRIS)
                  Vals(1) = Arcs            ' New SqlClient.SqlParameter("@Archivo", Arcs)

                  con.EjecutarSPConTransaccion("dbo.SPI_BDS_GRL_VISITA_ARCHIVOSOBSOLETOS_SET", Noms, Vals, tran)
               End If
            Next

            objRegistrar.objVisita.IdVisitaGenerado = idVisitaSEPRIS
            objRegistrar.objVisita.IdPasoActual = 1 ' 1 - Elaborar Oficios y Acta Inicial usando requisitos de VJ y enviar a VJ - BDS_C_GR_PASOS
            objRegistrar.objVisita.FechaRegistro = DateTime.Now

            Dim objNegVisita As New NegocioVisita(objVisita, objUsuario, Nothing, Comentarios)
            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                                    False, -1,
                                                                    True, 1, Constantes.EstatusPaso.EnRevisionEspera,
                                                                    False, -1)
            tran.Commit()
            listErrs.Add(liError.ToString())

         ElseIf Aceptado And Not CambioNorma Then

            'Dim Pars(1) As System.Data.SqlClient.SqlParameter
            'Dim Dt As DataTable
            'Dim Dr As DataRow
            'Arcs = "."

            'con = New Conexion.SQLServer()

            'Pars(0) = New SqlClient.SqlParameter("@I_ID_VISITA", idVisitaSEPRIS)
            ''Pars(1) = New SqlClient.SqlParameter("@I_ID_USUARIO", "")
            'Pars(1) = New SqlClient.SqlParameter("@I_ID_PASO", 1)

            'Dt = con.EjecutarSPConsultaDT("sps_bds_grl_get_Documentos_v2_Supervisar_V17", Pars)

            'For Each Dr In Dt.Rows
            '   Arcs &= Dr("T_NOM_Documento").ToString() & "."
            'Next

            'Pars(0) = New SqlClient.SqlParameter("@idVisitaSEPRIS", idVisitaSEPRIS)
            'Pars(1) = New SqlClient.SqlParameter("@Archivos", Arcs)

            'Dt.Dispose()
            'Dt = con.EjecutarSPConsultaDT("dbo.SPI_BDS_GRL_VISITA_ARCHIVOSOBSOLETOS_GET", Pars)

            'Arcs = ""
            'For Each Dr In Dt.Rows
            '   Arcs &= Dr("Archivo").ToString() & ", "
            'Next

            'If Arcs <> "" Then
            '   listErrs.Add("2200")
            '   listErrs.Add(Arcs.Substring(0, Arcs.Length - 1))
            'Else
            listErrs = AvanzaPaso(idVisitaSEPRIS, Usr, Comentarios, 0, 0, 0, "")
            'End If

         ElseIf Not Aceptado And Not CambioNorma Then
            '   listErrs = AvanzaPaso(idVisitaSEPRIS, Usr, Comentarios, 0, 0, 0, "")
            'Else
            listErrs = RechazaPaso(idVisitaSEPRIS, Usr, Comentarios)
         End If

      Catch ex As Exception
         lstResultado.Add("403")
         If Not tran Is Nothing Then
            tran.Rollback()
         End If
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro_V17.csv.vb, Error general al registrar desde sisvig", "")
      End Try

      Return listErrs
   End Function

   Function Paso4(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal FechaInicioP4 As DateTime, ByVal Comentarios As String) As List(Of String) Implements IwcfRegistro_V17.Paso4
      Dim Res As List(Of String)

      Res = ActualizaFecha(idVisitaSEPRIS, Usr, "", FechaInicioP4, SEPRIS.Constantes.TipoFecha.FechaInicio, False)

      If Res(0) = "-1" Then
         Res = AvanzaPaso(idVisitaSEPRIS, Usr, Comentarios, 0, 0, 0, "")
      End If

      Return Res
   End Function

   Function Paso5(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal idVisitaSISVIG As Integer, ByVal HuboRespuesta As Boolean, _
                                   ByVal FechaInSitu As DateTime) As List(Of String) Implements IwcfRegistro_V17.Paso5
      Dim Res As List(Of String)
      Dim Aux As Auxiliares_V17 = New Auxiliares_V17()
      Dim objUsuario As New Entities.Usuario(Usr)

      Dim objNotif As New NotificacionesVisita(objUsuario, "")
      Dim mensaje2 As String = ""
      Dim objOpVisita As Visita

      objOpVisita = OperacionesVisita.ObtenerDetalleVisita(idVisitaSEPRIS, Constantes.AREA_VO)

      If HuboRespuesta Then
         If OperacionesVisita.DocumentosObligatoriosSinCargar(idVisitaSEPRIS, 5, "") Then
            Res = New List(Of String)()
            Res.Add(2140)
            Return Res
         End If
      End If

      Res = AvanzaPaso(idVisitaSEPRIS, Usr, "", 0, 0, 0, "")
      
      If Res(0) = "-1" Then
		 Res = ActualizaFecha(idVisitaSEPRIS, Usr, "", FechaInSitu, SEPRIS.Constantes.TipoFecha.FechaCampoInicial, False)
         If Res(0) = "-1" Then

            If HuboRespuesta Then
               mensaje2 = objNotif.NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_SI_RESPUESTA_AFORE_OFICIOS_INICIO, objOpVisita, True, False, False, Nothing, True)
            Else
               mensaje2 = objNotif.NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_NO_RESPUESTA_AFORE_OFICIOS_INICIO, objOpVisita, True, False, False, Nothing, True)
            End If

            If mensaje2 <> Constantes.CORREO_ENVIADO_OK Then
               Res(0) = 2166
            End If
         End If
      End If

      Return Res
   End Function

   Function Paso6(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal FechaInSitu As DateTime) As List(Of String) Implements IwcfRegistro_V17.Paso6
      Dim Res As List(Of String)

      Res = ActualizaFecha(idVisitaSEPRIS, Usr, "", FechaInSitu, Constantes.TipoFecha.FechaCampoFinal, False)
      If Res(0) = "-1" Then
         Res = AvanzaPaso(idVisitaSEPRIS, Usr, "", 0, 0, 0, "")
      End If

      Return Res
   End Function

   Function Paso6y7(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal FechaInSitu As DateTime, _
                  ByVal HayPres As Boolean, ByVal HaySancion As Boolean, ByVal FechaP7 As DateTime) As List(Of String) Implements IwcfRegistro_V17.Paso6y7
      Dim Res As List(Of String)
      Dim objUsuario As Entities.Usuario
      Dim objRegistro As New Registro
      Dim objVisita As Visita
      objVisita = OperacionesVisita.ObtenerDetalleVisita(idVisitaSEPRIS, Constantes.AREA_VO)

      objUsuario = objRegistro.InsertaUsuario(Usr)
      If Not objUsuario.Existe Then
         Res = New List(Of String)()
         Res.Add("414")
         Res.Add(Usr)
         Return Res
      End If

      Res = Paso6(idVisitaSEPRIS, Usr, FechaInSitu & " 23:59:00")

      If Res(0) = "-1" Then
         Res = Paso7(idVisitaSEPRIS, Usr, HayPres, HaySancion, FechaP7)
         AccesoBD.ActualizaFechaInicioVisita(idVisitaSEPRIS, objVisita.FechaInicioPasoActual, Constantes.TipoFecha.FechaCampoInicialP7, 0)
         AccesoBD.ActualizaFechaInicioVisita(idVisitaSEPRIS, FechaInSitu & " 23:59:00", Constantes.TipoFecha.FechaCampoFinalP7, 0)
      End If

      Return Res
   End Function

   Function Paso7(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, _
                  ByVal HayPres As Boolean, ByVal HaySancion As Boolean, _
                  ByVal Fecha As DateTime) As List(Of String) Implements IwcfRegistro_V17.Paso7
      Dim Res As List(Of String) = New List(Of String)()
      Dim objUsuario As Entities.Usuario
      Dim objRegistro As New Registro
      Dim liError As Integer = -1
      Dim Aux As Auxiliares
      Dim SubError As Integer = -1

      objUsuario = objRegistro.InsertaUsuario(Usr)
      If Not objUsuario.Existe Then
         liError = 414
      Else
         Dim objVisita As Visita
         objVisita = OperacionesVisita.ObtenerDetalleVisita(idVisitaSEPRIS, Constantes.AREA_VO)

         Dim objNegVisita As NegocioVisita = New NegocioVisita(objVisita, objUsuario, Nothing, "")

         Aux = New Auxiliares()

         Dim pbjPaso As New Paso
         pbjPaso.cargaPasoActualVisita(idVisitaSEPRIS)
         If HayPres Then
            liError = Aux.ValidarFechaGeneral(Fecha, objVisita.TieneProrroga, pbjPaso.FechaInicioEnVisita, True, False, True, , , 4, True)
         Else
            liError = -1
         End If

         If liError < 0 Then
            liError = OperacionesVisita.AvanzaPasoSiete_V17(Usr, idVisitaSEPRIS, HayPres, HaySancion, objNegVisita, objVisita, Fecha)
            If liError < 0 Then
               ActualizaFecha(idVisitaSEPRIS, Usr, "", Fecha, Constantes.TipoFecha.Fecha81)
            End If
         Else
            SubError = 4
         End If

         objVisita = Nothing
         objNegVisita = Nothing
      End If

      objRegistro = Nothing
      objUsuario = Nothing

      Res = New List(Of String)()
      Res.Add(liError.ToString())

      If SubError > 0 Then
         Res.Add(SubError.ToString())
      End If
      Return Res
   End Function

   Function Paso8(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, _
                  ByVal FecSesion As DateTime, ByVal Comentarios As String) As List(Of String) Implements IwcfRegistro_V17.Paso8
      Dim Res As List(Of String) = New List(Of String)()
      Dim objUsuario As Entities.Usuario
      Dim objRegistro As New Registro
      Dim liError As Integer = -1

      objUsuario = objRegistro.InsertaUsuario(Usr)
      If Not objUsuario.Existe Then
         Res.Add("414")
         Res.Add(Usr)
      Else
         Res = ActualizaFecha(idVisitaSEPRIS, Usr, "", FecSesion, Constantes.TipoFecha.FechaReunionVjp9, False)

         If Res(0) = "-1" Then
            Res = AvanzaPaso(idVisitaSEPRIS, Usr, Comentarios, 0, 0, 0, "")
         End If
      End If

      Return Res
   End Function

   Function Paso9(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, _
                  ByVal Aceptado As Boolean, ByVal FechaVOBO As DateTime, _
                  ByVal Comentarios As String) As List(Of String) Implements IwcfRegistro_V17.Paso9
      Dim Res As List(Of String) = New List(Of String)()
      Dim objUsuario As Entities.Usuario
      Dim objRegistro As New Registro
      Dim liError As Integer = -1
      'Dim objAux As New Auxiliares

      objUsuario = objRegistro.InsertaUsuario(Usr)
      'objAux.EscribeBitacoraYEventLog(idVisitaSEPRIS, 1, "Antes de validar si existe el objeto usuario: ", " Sin comentarios")
      If Not objUsuario.Existe Then
         Res.Add("414")
         Res.Add(Usr)
      Else
         Dim objVisita As Visita
         objVisita = OperacionesVisita.ObtenerDetalleVisita(idVisitaSEPRIS, Constantes.AREA_VO)

         Dim OpeVis As OperacionesVisita
         OpeVis = New OperacionesVisita()
         'objAux.EscribeBitacoraYEventLog(idVisitaSEPRIS, 1, "Antes de validar Aceptado: " & FechaVOBO & ":-:" & idVisitaSEPRIS, " Sin comentarios")
         If Aceptado Then
            Dim objNegVisita As New NegocioVisita(objVisita, objUsuario, Nothing, Comentarios)
            If objVisita.ExisteReunionPaso8 Then

               'objAux.EscribeBitacoraYEventLog(idVisitaSEPRIS, 1, "Antes de actualizar fecha en paso 9: " & FechaVOBO & ":-:" & idVisitaSEPRIS, " Sin comentarios")
               Res = ActualizaFecha(idVisitaSEPRIS, Usr, Comentarios, FechaVOBO, Constantes.TipoFecha.FechaReunionVjPaso10VOBO, False)
               'objAux.EscribeBitacoraYEventLog(idVisitaSEPRIS, 1, "Después de actualizar fecha en paso 9", " Sin comentarios")

               If Res(0) = "-1" Then
                  objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnRevisionEspera,
                                                                      True, Constantes.EstatusPaso.Enviado,
                                                                      True, 11, Constantes.EstatusPaso.EnRevisionEspera,
                                                                      True, Constantes.CORREO_VER_FINAL_ACTA_CIRCUNSTANCIADA)

                  OperacionesVisita.ActualizaEstatusSisvig(objVisita, 11, Constantes.EstatusPaso.EnRevisionEspera, Comentarios)
                  OpeVis.InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, 11, Usr, "Visita: " & objVisita.IdVisitaSisvig)

                  ' Que ya se halla cumplido la fecha del paso 11
                  'If Not Fechas.Vacia(objVisita.FechaReunionPresidencia) AndAlso DateTime.Now <= objVisita.FechaReunionPresidencia Then
                  If Not objVisita.FechaReunionPresidencia Is Nothing Then
                     If DateTime.Now.Date <= objVisita.FechaReunionPresidencia Then
                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                                  True, Constantes.EstatusPaso.Enviado,
                                                                  True, 12, Constantes.EstatusPaso.EnRevisionEspera,
                                                                  False, -1, , , , , , , , , objVisita.FechaReunionPresidencia)

                        If Not OpeVis.MandaCorreoSandraPachecoPaso12(objNegVisita, objVisita) Then
                           liError = 423
                        End If
                     End If
                  Else

                     'SI AUN NO SE CUENTA CON LA FECHA DE REUNIÓN INTERNA DE PRESIDENCIA, SE MANDA CORREO A SANDRA
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
                  liError = Res(0)
               End If
            Else
               Res = ActualizaFecha(idVisitaSEPRIS, Usr, Comentarios, FechaVOBO, Constantes.TipoFecha.FechaReunionVjPaso10VOBO, False)
               If Res(0) = "-1" Then
                  ' Ir a paso 13
                  objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnRevisionEspera,
                                                                      True, Constantes.EstatusPaso.Enviado,
                                                                      True, 13, Constantes.EstatusPaso.EnRevisionEspera,
                                                                      True, Constantes.CORREO_PASO_12_VERSION_FINAL_DOCTOS)
                  OperacionesVisita.ActualizaEstatusSisvig(objVisita, 13, Constantes.EstatusPaso.EnRevisionEspera, Comentarios)
                  OpeVis.InsertaBitacoraSisvigSepris(objVisita.IdVisitaSisvig, 13, Usr, "Visita: " & objVisita.IdVisitaSisvig)
               Else
                  liError = Res(0)
               End If
            End If
            Res.Add(liError)
         Else
            Res = RechazaPaso(idVisitaSEPRIS, Usr, Comentarios)
         End If
      End If

      Return Res
   End Function

   Function Paso11(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, _
                   ByVal Nuevo As Boolean, ByVal FecPresVJ As DateTime, ByVal Notifica As Boolean) As List(Of String) Implements IwcfRegistro_V17.Paso11
      Dim Res As List(Of String) = New List(Of String)()
      Dim objUsuario As Entities.Usuario
      Dim objRegistro As New Registro
      Dim liError As Integer = -1

      objUsuario = objRegistro.InsertaUsuario(Usr)
      If Not objUsuario.Existe Then
         Res.Add("414")
         Res.Add(Usr)
      Else
         Dim objVisita As Visita
         objVisita = OperacionesVisita.ObtenerDetalleVisita(idVisitaSEPRIS, Constantes.AREA_VO)

         If objVisita.FechaReunionPresidencia Is Nothing OrElse Fechas.Vacia(objVisita.FechaReunionPresidencia) Then
            Res = ActualizaFecha(idVisitaSEPRIS, Usr, "", FecPresVJ, Constantes.TipoFecha.FechaReunionPresi, Notifica)
         Else
            If objVisita.FechaReunionPresidencia <> FecPresVJ.Date Then
               Res = ActualizaFecha(idVisitaSEPRIS, Usr, "", FecPresVJ, Constantes.TipoFecha.FechaReunionPresi, Notifica)
            Else
               Res.Add(-1)
            End If
         End If

         'If Res(0) = "-1" Then
         '   If objVisita.IdPasoActual = 11 Then
         '      Res = AvanzaPaso(idVisitaSEPRIS, Usr, "", 0, 0, 0, "")
         '   End If
         'End If
      End If

      Return Res
   End Function

   Function Paso12(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, _
                   ByVal Nuevo As Boolean, ByVal FecPresAfore As DateTime) As List(Of String) Implements IwcfRegistro_V17.Paso12
      Dim Res As List(Of String) = New List(Of String)()
      Dim objUsuario As Entities.Usuario
      Dim objRegistro As New Registro
      Dim liError As Integer = -1

      objUsuario = objRegistro.InsertaUsuario(Usr)
      If Not objUsuario.Existe Then
         Res.Add("414")
         Res.Add(Usr)
      Else
         If Nuevo Then
            Res = ActualizaFecha(idVisitaSEPRIS, Usr, "", FecPresAfore, Constantes.TipoFecha.FechaReunionAfore, True)

            'AvanzaPaso(idVisitaSEPRIS, Usr, "", 0, 0, 0, "")
         Else
            Res = ActualizaFecha(idVisitaSEPRIS, Usr, "", FecPresAfore, Constantes.TipoFecha.FechaReunionAfore, True)
         End If

      End If

      Return Res
   End Function

   Function Paso13(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, _
                   ByVal FecLevantamiento As DateTime) As List(Of String) Implements IwcfRegistro_V17.Paso13
      Dim Res As List(Of String) = New List(Of String)()
      Dim objUsuario As Entities.Usuario
      Dim objRegistro As New Registro
      Dim liError As Integer = -1

      objUsuario = objRegistro.InsertaUsuario(Usr)
      If Not objUsuario.Existe Then
         Res.Add("414")
         Res.Add(Usr)
      Else
         Res = ActualizaFecha(idVisitaSEPRIS, Usr, "", FecLevantamiento & " 23:59:00", Constantes.TipoFecha.FechaInSituActaCircunstanciada, False)

         If Res(0) = "-1" Then
            Res = AvanzaPaso(idVisitaSEPRIS, Usr, "", 0, 0, 0, "")
            Res = ActualizaFecha(idVisitaSEPRIS, Usr, "", FecLevantamiento & " 23:59:00", Constantes.TipoFecha.FechaInSituActaCircunstanciada, False)
         End If
      End If

      Return Res
   End Function

   Function Paso14(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, _
                   ByVal HayRespAfore As Boolean) As List(Of String) Implements IwcfRegistro_V17.Paso14
      Dim Res As List(Of String) = New List(Of String)()
      Dim objUsuario As Entities.Usuario
      Dim objRegistro As New Registro
      Dim liError As Integer = -1

      objUsuario = objRegistro.InsertaUsuario(Usr)
      If Not objUsuario.Existe Then
         Res.Add("414")
         Res.Add(Usr)
      Else
         Res.Add("-1")
         If HayRespAfore Then
            If OperacionesVisita.DocumentosObligatoriosSinCargar(idVisitaSEPRIS, 14, "") Then
               Res(0) = 2140
               Return Res
            End If
         End If

         If Res(0) = "-1" Then
            Dim objOpVisita As Visita
            objOpVisita = OperacionesVisita.ObtenerDetalleVisita(idVisitaSEPRIS, Constantes.AREA_VO)

            Res = AvanzaPaso(idVisitaSEPRIS, Usr, "", 0, 0, 0, "")
            If Res(0) = "-1" Then
               Dim objNotif As New NotificacionesVisita(objUsuario, "")
               Dim mensaje2 As String = ""


               If HayRespAfore Then
                  mensaje2 = objNotif.NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_SI_RESPUESTA_AFORE_OFICIOS_INICIO, objOpVisita, True, False, False, Nothing, True)
               Else
                  mensaje2 = objNotif.NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_NO_RESPUESTA_AFORE_OFICIOS_INICIO, objOpVisita, True, False, False, Nothing, True)
               End If

               If mensaje2 <> Constantes.CORREO_ENVIADO_OK Then
                  Res(0) = 2166
               End If
            End If
         End If
      End If

      Return Res
   End Function

   Function Paso15(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, _
                      ByVal FecC As DateTime, ByVal Comentarios As String) As List(Of String) Implements IwcfRegistro_V17.Paso15
      Dim Res As List(Of String) = New List(Of String)()
      Dim objUsuario As Entities.Usuario
      Dim objRegistro As New Registro
      Dim liError As Integer = -1

      objUsuario = objRegistro.InsertaUsuario(Usr)
      If Not objUsuario.Existe Then
         Res.Add("414")
         Res.Add(Usr)
      Else
         Res = ActualizaFecha(idVisitaSEPRIS, Usr, Comentarios, FecC, Constantes.TipoFecha.FechaReunionVjp16, False)

         If Res(0) = "-1" Then
            Res = AvanzaPaso(idVisitaSEPRIS, Usr, Comentarios, 0, 0, 0, "")
         End If
      End If

      Return Res
   End Function

   Function Paso16(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal Aceptado As Boolean, ByVal FechaVobo As DateTime, _
                   ByVal Comentarios As String) As List(Of String) Implements IwcfRegistro_V17.Paso16
      Dim Res As List(Of String) = New List(Of String)()
      Dim objUsuario As Entities.Usuario
      Dim objRegistro As New Registro
      Dim liError As Integer = -1

      objUsuario = objRegistro.InsertaUsuario(Usr)
      If Not objUsuario.Existe Then
         Res.Add("414")
         Res.Add(Usr)
      Else
         If Aceptado Then
                Res = ActualizaFecha(idVisitaSEPRIS, Usr, Comentarios, FechaVobo, Constantes.TipoFecha.FechaLevantamientoActaConclusion, False)

                If Res(0) = "-1" Then
                    Res = AvanzaPaso(idVisitaSEPRIS, Usr, Comentarios, 0, 0, 0, "")
                End If
            Else
                Res = RechazaPaso(idVisitaSEPRIS, Usr, Comentarios)
            End If
        End If

        Return Res
    End Function

    Function Paso18(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal Acepta As Boolean,
                         ByVal FecVoBo As DateTime, ByVal Comentarios As String) As List(Of String) Implements IwcfRegistro_V17.Paso18
        Dim Res As List(Of String) = New List(Of String)()
        Dim objUsuario As Entities.Usuario
        Dim objRegistro As New Registro
        Dim liError As Integer = -1

        objUsuario = objRegistro.InsertaUsuario(Usr)
        If Not objUsuario.Existe Then
            Res.Add("414")
            Res.Add(Usr)
        Else
            If Acepta Then
                Res = ActualizaFecha(idVisitaSEPRIS, Usr, Comentarios, FecVoBo, Constantes.TipoFecha.FechaVoBoP18, False)

                If Res(0) = "-1" Then
                    Res = AvanzaPaso(idVisitaSEPRIS, Usr, Comentarios, 0, 0, 0, "")
                End If
            Else
                Res = RechazaPaso(idVisitaSEPRIS, Usr, Comentarios)
            End If
        End If
        Return Res
    End Function

    Function Paso19(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal FechaLev As DateTime) As List(Of String) Implements IwcfRegistro_V17.Paso19
        Dim Res As List(Of String) = New List(Of String)()
        Dim objUsuario As Entities.Usuario
        Dim objRegistro As New Registro
        Dim liError As Integer = -1

        objUsuario = objRegistro.InsertaUsuario(Usr)
        If Not objUsuario.Existe Then
            Res.Add("414")
            Res.Add(Usr)
        Else

            Res = ActualizaFecha(idVisitaSEPRIS, Usr, "", FechaLev, Constantes.TipoFecha.FechaLevantamientoActaConclusion, False)

            If Res(0) = "-1" Then
            AccesoBD.ActualizaFechaInicioVisita(idVisitaSEPRIS, FechaLev, Constantes.TipoFecha.FechaCampoFinalP19, 0)
            Res = AvanzaPaso(idVisitaSEPRIS, Usr, "", 0, 0, 0, "")
         End If
      End If

      Return Res
   End Function

   Function Paso20(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal FechaLev As DateTime) As List(Of String) Implements IwcfRegistro_V17.Paso20
      Dim Res As List(Of String) = New List(Of String)()
      Dim objUsuario As Entities.Usuario
      Dim objRegistro As New Registro

      objUsuario = objRegistro.InsertaUsuario(Usr)
      If Not objUsuario.Existe Then
         Res.Add("414")
         Res.Add(Usr)
      Else
         Res = AvanzaPaso(idVisitaSEPRIS, Usr, "", 0, 0, 0, "")
      End If

      Return Res
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
                               Optional pdFechaVul As DateTime = Nothing) As List(Of String) Implements IwcfRegistro_V17.EditaVisita

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

   'Public Function ActualizaFechaVulnera(IdVisitaSepris As Integer, Fecha As DateTime) As List(Of String) Implements IwcfRegistro_V17.ActualizaFechaVulnera
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
                               Optional nombreDocto As String = "") As List(Of String) Implements IwcfRegistro_V17.AvanzaPaso

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

         'NOTIFICACION DE VERSIÓN FINAL DE DOCUMENTOS PASO 6
      ElseIf opcNotifica = 2 Then

         objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                 False, -1,
                                                                 False, -1, -1,
                                                                 True, Constantes.CORREO_DOCUMENTOS_NOTIFICAR_CORRECTO,
                                                                 False, True, False)

         Documento.ActualizaEstatusDocumento(IdVisita, idDocumento, Constantes.EstatusPaso.Revisado, versionDocto)

      Else
         lstResultado.Add(objOpVisita.Avanza_V17())
      End If


      Return lstResultado
   End Function

   Public Function RechazaPaso(IdVisita As Integer,
                               IdentificadorUsuario As String,
                               psComentarios As String) As List(Of String) Implements IwcfRegistro_V17.RechazaPaso
      Dim lstResultado As New List(Of String)
      Dim objOpVisita As OperacionesVisita

      objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = psComentarios}
      lstResultado.Add(objOpVisita.Rechaza_V17())

      Return lstResultado
   End Function

   Public Function ActualizaFecha(IdVisita As Integer, IdentificadorUsuario As String, psComentarios As String, Fecha As DateTime, TipoFecha As Constantes.TipoFecha,
                           Optional NotificarCambio As Boolean = True) As List(Of String) Implements IwcfRegistro_V17.ActualizaFecha
      Dim lstResultado As New List(Of String)
      Dim objOpVisita As OperacionesVisita
      Dim objAux As New Auxiliares

      objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = psComentarios, .Fecha = Fecha, .TipoFecha = TipoFecha}

      If Not NotificarCambio Then
         'objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, 1, "Entra a ActualizarFecha(): " & NotificarCambio & " Presentación Interna de hallazgos", " viene en false")
         lstResultado.Add(objOpVisita.ActualizaFecha())
      Else
         lstResultado.Add(objOpVisita.ActualizaNuevaFecha())
      End If

      Return lstResultado
   End Function

   Function BloquearCargaDeArchivos(ByVal IdVisita As Integer, IdentificadorUsuario As String, ByVal Edo As Boolean) As List(Of String) Implements IwcfRegistro_V17.BloquearCargaDeArchivos
      Dim objOpVisita As OperacionesVisita
      Dim lstResultado As New List(Of String)
      Dim lierror As Integer = -1
      Dim objAux As New Auxiliares
      Dim vis As SEPRIS.Visita
      Try
         objOpVisita = New OperacionesVisita()
         objOpVisita.IdVisita = IdVisita

         vis = objOpVisita.ObtenerDetalleVisita()

         objOpVisita.BloquearCargaDocumentos(IdVisita, Edo)

         objOpVisita.InsertaBitacoraSisvigSepris(IdVisita, vis.IdPasoActual, IdentificadorUsuario, "Bloquear carga de documento: " & vis.IdVisitaSisvig)
      Catch ex As Exception
         lierror = 410
      End Try
      objAux.EscribeBitacoraYEventLog(IdentificadorUsuario, lierror, "Bloquear carga de documentos", "")

      lstResultado.Add(lierror)

      Return lstResultado
   End Function


   Function FinalizaCargaDocumentos(IdVisita As Integer,
                            IdentificadorUsuario As String,
                            psCadenaParametros As String,
                            psComentarios As String) As List(Of String) Implements IwcfRegistro_V17.FinalizaCargaDocumentos
      Dim lstResultado As New List(Of String)
      Dim objOpVisita As OperacionesVisita

      objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = psComentarios}
      lstResultado.Add(objOpVisita.FinalizaCargaDocumentos(psCadenaParametros))

      Return lstResultado
   End Function

   Function SolicitaProrroga(IdVisita As Integer,
                            IdentificadorUsuario As String,
                            motivoProrroga As String) As List(Of String) Implements IwcfRegistro_V17.SolicitaProrroga
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
                               pbFlagSancion As Boolean) As List(Of String) Implements IwcfRegistro_V17.AvanzaPasoSiete
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
                               Optional pbPrimeraNotificacion As Boolean = False) As List(Of String) Implements IwcfRegistro_V17.AvanzaPasoOcho
      Dim lstResultado As New List(Of String)
      Dim objOpVisita As OperacionesVisita

      objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = psComentarios}
      objOpVisita.FlagSancion = pbFlagSancion
      objOpVisita.PrimeraNotificacionOcho = pbPrimeraNotificacion

      lstResultado.Add(objOpVisita.Avanza())

      Return lstResultado
   End Function

   'Function ObtenerDetalleVisita(IdVisita As Integer) As Visita Implements IwcfRegistro_V17.ObtenerDetalleVisita
   '    Dim objOpVisita As OperacionesVisita

   '    objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita}

   '    Return objOpVisita.ObtenerDetalleVisita()
   'End Function

   Function getErrorValidacionSupervisar(ByVal id_error As String) As String Implements IwcfRegistro_V17.getErrorValidacionSupervisar
      Return AccesoBD.getErrorValidacionSupervisar(id_error)
   End Function

   Function CancelarVisita(IdVisita As Integer,
                            IdentificadorUsuario As String,
                            motivoCancela As String) As List(Of String) Implements IwcfRegistro_V17.CancelarVisita
      Dim lstResultado As New List(Of String)
      Dim objOpVisita As OperacionesVisita

      objOpVisita = New OperacionesVisita With {.IdVisita = IdVisita, .IdentificadorUsuario = IdentificadorUsuario, .Comentarios = motivoCancela}
      lstResultado.Add(objOpVisita.CancelarVisita())

      Return lstResultado
   End Function

   Function ConsultaDocumentosObligatoriosSinCargarSinVisita(IdVisitaSisvig As Integer) As Boolean Implements IwcfRegistro_V17.ConsultaDocumentosObligatoriosSinCargarSinVisita
      Dim objOpVisita As New Auxiliares

      Return objOpVisita.ConsultaDocumentosObligatoriosSinCargarPorPasoUsuario("", PasoProcesoVisita.Pasos.Uno, Constantes.Obligatorio.Obligatorios, , , IdVisitaSisvig)
   End Function
End Class
