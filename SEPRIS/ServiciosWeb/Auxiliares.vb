Imports System.Web.Configuration
Imports System.Net

Public Class Auxiliares
    Public Function datosCapturadosValidos(piIVisitaSisvig As Integer,
                                           psIdentificadorUsusario As String,
                                           psFolioVisita As String,
                                           pdFechaInicio As DateTime,
                                           piIdEntidad As Integer,
                                           piTipoVisita As Integer,
                                           psLstObjetoVisita As List(Of Integer),
                                           psDscOtroObjVisita As String,
                                           psSupervisor As String,
                                           psLstInspector As List(Of String),
                                          Optional piBanEstaInsertando As Integer = Constantes.Falso) As Integer

        If psFolioVisita.Trim() = String.Empty Then
            Return 2112
        End If

        Dim fechaHoy As Date = DateTime.Now.Date

        ''QUITA VALIDACION DE LA FECHA DE VISITA AGC
        Dim res2 As Integer = Date.Compare(pdFechaInicio, fechaHoy)
        If res2 < 0 Then
            Return 2128
        End If

        ''NO VALIDA FECHA DE REGISTRO SI ESTA EDITANDO DESDE DETALLE
        If piIdEntidad <= 0 Then
            Return 2117
        End If


        If piTipoVisita <= 0 Then
            Return 2118
        End If

        ''agc objeto visita
        If psLstObjetoVisita.Count < 1 Then
            Return 2136
        End If


        If psSupervisor.Trim() = "" Then
            Return 2119
        End If

        If psLstInspector.Count() = 0 Then
            Return 2120
        End If

        If piIdEntidad > 0 Then
            If piBanEstaInsertando = Constantes.Verdadero Then
                ''Se esta insertando
                If ConsultaDocumentosObligatoriosSinCargarPorPasoUsuario("", PasoProcesoVisita.Pasos.Uno, Constantes.Obligatorio.Obligatorios, , , piIVisitaSisvig) Then
                    Return 2140
                End If
            End If
        End If

        Return -1
    End Function

    Public Function ConsultaDocumentosObligatoriosSinCargarPorPasoUsuario(psIdentificadorUsusario As String, idPaso As Integer,
                                                       Optional iBanderaObligatorio As Integer = Constantes.Todos,
                                                       Optional iTipoDocumento As Integer = Constantes.TipoArchivo.WORD,
                                                       Optional idDocumento As Integer = Constantes.Todos,
                                                       Optional piIVisitaSisvig As Integer = Constantes.Todos) As Boolean
        Dim lstDocMin As List(Of Documento.DocumentoMini)

        ''ELIMINA EL USUARIO YA QUE LAS CONSULTAS SON POR ID VISITA SISVIG SIN IMPORTAR EL USUARIO
        lstDocMin = AccesoBD.ObtenerDocumentosObligatoriosPorPasoUsuario("", idPaso, idDocumento, iBanderaObligatorio, iTipoDocumento, piIVisitaSisvig)

        If lstDocMin.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function ExisteObjetoVisita(idObjetoVisita As Integer, ByRef liObjetoVis As Integer) As Boolean

        'Consulta a SISAN para obtener la descripción del objeto de visita con base en el ID
        Dim USUARIOS = WebConfigurationManager.AppSettings("UsuarioSisan")
        Dim PASSWORDS = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("PassEncSisan").ToString())

        Dim EnvioSisan As New wsSisanReg.RegistroExterno
        Dim credentialsS As NetworkCredential = New NetworkCredential(USUARIOS, PASSWORDS, "ADCONSAR")
        EnvioSisan.Credentials = credentialsS
        Dim dsProcesos = EnvioSisan.ObtenerCatalogoProcesos

        Dim dtss = dsProcesos.Catalogo.Tables(0)

        If Not IsNothing(dtss) Then
            If dtss.Rows.Count > 0 Then
                For Each lrRow As DataRow In dtss.Rows
                    If CInt(lrRow(0)) = idObjetoVisita Then
                        Dim dt As DataTable = AccesoBD.ExisteObjetoVisita(lrRow(2).ToString())
                        Try
                            If Not IsNothing(dtss) Then
                                If dt.Rows.Count > 0 Then
                                    liObjetoVis = CInt(dt.Rows(0)("N_ID_OBJETO_VISITA"))
                                    Utilerias.ControlErrores.EscribirEvento("", EventLogEntryType.Information, "wcfRegistro.csv.vb, ExisteObjetoVisita: " & lrRow(2).ToString(), "")
                                    Return True
                                End If
                            End If
                        Catch ex As Exception
                            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro.csv.vb, ExisteObjetoVisita", "")
                        End Try
                    End If
                Next
            End If
        End If



        'Dim dt As DataTable = AccesoBD.ExisteObjetoVisita(idObjetoVisita)
        'Try
        '    If Not IsNothing(dt) Then
        '        If dt.Rows.Count > 0 Then
        '            liObjetoVis = CInt(dt.Rows(0)("N_ID_OBJETO_VISITA"))
        '            Return True
        '        End If
        '    End If
        'Catch ex As Exception
        '    Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro.csv.vb, ExisteObjetoVisita", "")
        'End Try

        Return False
    End Function

    Public Function ConvierteInspectores(psLstInspector As List(Of String)) As List(Of InspectorAsignado)
        Dim lstUsuarios As New List(Of InspectorAsignado)
        Dim objUsuario As Entities.Usuario

        If psLstInspector.Count <= 0 Then
            Return lstUsuarios
        End If

        'For Each lsInspector As String In psLstInspector
        '    objUsuario = New Entities.Usuario(lsInspector)
        '    If Not objUsuario.Existe Then
        '        datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(lsInspector)
        '        If datosUsuario.Count > 0 Then
        '            If Not IsNothing(datosUsuario.Item("nombre")) Then
        '                objUsuario.Nombre = datosUsuario.Item("nombre").ToString()
        '            End If

        '            If Not IsNothing(datosUsuario.Item("apellidos")) Then
        '                objUsuario.Apellido = datosUsuario.Item("apellidos").ToString()
        '            End If

        '            If Not IsNothing(datosUsuario.Item("mail")) Then
        '                objUsuario.Mail = datosUsuario.Item("mail").ToString()
        '            End If

        '            objUsuario.IdentificadorPerfilActual = 2
        '            objUsuario.AgregarUsuarioVO()
        '        End If
        '    End If
        'Next


        For Each lsInspector As String In psLstInspector
            lstUsuarios.Add(New InspectorAsignado With {.Id = lsInspector, .Nombre = ""})
        Next

        Return lstUsuarios
    End Function

    Public Function ConvierteSupervisores(psSupervisor As String) As List(Of SupervisorAsignado)
        Dim lstUsuarios As New List(Of SupervisorAsignado)
        Dim objUsuario As Entities.Usuario

        If psSupervisor.Trim.Length <= 0 Then
            Return lstUsuarios
        End If

        Dim vecUsu() As String = psSupervisor.Split("|")

        For i As Integer = 0 To vecUsu.Length - 1
            objUsuario = New Entities.Usuario(vecUsu(i))
            lstUsuarios.Add(New SupervisorAsignado With {.Id = vecUsu(i), .Nombre = ""})
        Next

        Return lstUsuarios
    End Function

    Public Function GeneraFolioVisita(piIdEntidad As Integer, piArea As Integer, psDscEntidad As String) As String
        If piIdEntidad <> -1 Then
            Return "999" & "/VO/" & psDscEntidad & "/" & Today.ToString("MMyy")
        End If

        Return ""
    End Function

    ''' <summary>
    ''' ESPECIAL SISVIG ID 1 
    ''' ORDINARIA SISVIG ID 0
    ''' </summary>
    ''' <param name="piTipoVisita"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function HomologaTipoVisita(piTipoVisita As Integer) As Integer
        Select Case piTipoVisita
            Case 0
                Return 5
            Case 1
                Return 3
        End Select

        Return piTipoVisita
    End Function

    Public Function HomologaObjetoVisita(psLstObjetoVisita As List(Of Integer)) As List(Of Integer)
        Dim lstObjetoVisita As New List(Of Integer)
        Dim liObjetoVis As Integer = 0

        For Each liIdObjeto As Integer In psLstObjetoVisita
            If Not ExisteObjetoVisita(liIdObjeto, liObjetoVis) Then
                liObjetoVis = InsertaObjetoVisitaAux(liIdObjeto)

                If liObjetoVis <> -1 Then
                    lstObjetoVisita.Add(liObjetoVis)
                Else
                    Utilerias.ControlErrores.EscribirEvento("No se pudo insertar el objeto visita", EventLogEntryType.Error, "HomologaObjetoVisita", "")
                End If
            Else
                lstObjetoVisita.Add(liObjetoVis)
            End If
        Next

        Return lstObjetoVisita
    End Function

    ''' <summary>
    ''' Valida si una fecha es una fecha habil
    ''' </summary>
    ''' <param name="pdFecha"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EsDiaHabil(pdFecha As Date) As Boolean
        If pdFecha.DayOfWeek = DayOfWeek.Saturday Or pdFecha.DayOfWeek = DayOfWeek.Sunday Then
            Return False
        Else
            Dim lstDiasFeriados As New List(Of DateTime)
            lstDiasFeriados = AccesoBD.getDiasFeriados(pdFecha)

            If lstDiasFeriados.Count > 0 Then
                Return False
            Else
                Return True
            End If
        End If
    End Function

    Private Function RecuperaParametroDiasFecha() As Integer
        Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.RangoFechaValidaPaso9_16)
        Dim liDias As Integer

        ''Conexion.SQLServer
        If dt.Rows.Count > 0 Then
            If Not Int32.TryParse(dt.Rows(0)("T_DSC_VALOR"), liDias) Then
                liDias = 3
            End If
        Else
            liDias = 3
        End If

        Return liDias
    End Function

    Private Function ObtenerFechaFinalizaPaso(piPaso As Integer, TieneProrrogaAprobada As Boolean) As DateTime
        Dim ldFechafinalizaPaso As DateTime
        Dim diasFinalizaPaso As Integer = 0
        Dim lstCatalgoPasos As List(Of Paso) = AccesoBD.getCatalogoPasos(piPaso)
        Dim objPaso As Paso = (From lPaso In lstCatalgoPasos Where lPaso.IdPaso = piPaso Select lPaso).FirstOrDefault()

        If Not IsNothing(objPaso) Then
            If TieneProrrogaAprobada Then
                diasFinalizaPaso = objPaso.NumDiasMax
            ElseIf objPaso.EnProrroga = 1 Then
                diasFinalizaPaso = objPaso.NumDiasMin
            Else
                diasFinalizaPaso = objPaso.NumDiasMax
            End If

            ldFechafinalizaPaso = AccesoBD.ObtenerFecha(DateTime.Now, diasFinalizaPaso)
        Else
            ldFechafinalizaPaso = Nothing
        End If

        Return ldFechafinalizaPaso

    End Function
    ''' <summary>
    ''' Valida una fecha
    ''' </summary>
    ''' <param name="pdFecha"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
   Public Function ValidarFechaGeneral(ByVal pdFecha As DateTime, TieneProrrogaAprobada As Boolean, FechaInicioPasoActual As DateTime,
                                        ByVal pbValidaFechaInicioPasoActual As Boolean,
                                        Optional pbAgregaFormato24horas As Boolean = False,
                                        Optional pbValidaNdiasHabiles As Boolean = False,
                                        Optional pbValidaFechaFinalizaPasoSig As Boolean = False,
                                        Optional piPasoSiguiente As PasoProcesoVisita.Pasos = 0,
                                        Optional nDias As Integer = 0,
                                        Optional pbValidaHoy As Boolean = False) As Integer
      Dim ldFecha As Date

      Try
         'If Not Date.TryParse(pdFecha.ToString("dd/MM/yyyy") & IIf(pbAgregaFormato24horas, " 23:59:00", ""), ldFecha) Then
         '    pdFecha = DateTime.MinValue
         '    Return 2150
         'Else

         ldFecha = pdFecha

         If Not EsDiaHabil(ldFecha) Then
            pdFecha = DateTime.MinValue
            Return 2142
         Else
            Dim liNumDiasAux As Integer = 0
            Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.DiasFechasAnteriores), liNumDiasAux)
            Dim ldFechaAuxAnterior As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDiasAux, Constantes.IncremeteDecrementa.Decrementa)

            If ldFecha.Date < ldFechaAuxAnterior.Date Then
               pdFecha = DateTime.MinValue
               Return 2165
            Else
               ''VALIDA SI LA FECHA ES MAYOR A LA FECHA DEL INICIAL DEL PASO ACTUAL
               If pbValidaFechaInicioPasoActual Then
                  Dim ldFechaAux As Date
                  If FechaInicioPasoActual <> DateTime.MinValue Then
                     ldFechaAux = FechaInicioPasoActual
                  Else
                     ldFechaAux = ldFecha
                  End If

                  If ldFecha.Date < ldFechaAux.Date Then
                     pdFecha = DateTime.MinValue
                     Return 2164
                  End If
               End If

               ''VALIDAR que la FECHA SE MAYOR QUE HOY
               If pbValidaHoy AndAlso pdFecha <= DateTime.Now.Date Then
                  Return 2203
               End If

               ''VALIDAR FECHA MAYOR A 3 DIAS HABILES A PARTIR DE LA FECHA ACTUAL
               If pbValidaNdiasHabiles Then
                  Dim liNumDias As Integer = nDias
                  If liNumDias = 0 Then
                     liNumDias = RecuperaParametroDiasFecha()
                  End If

                  Dim ldFechaEnviaVj As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDias).Date

                  If ldFechaEnviaVj < ldFecha.Date Then
                     pdFecha = DateTime.MinValue
                     Return 2162
                  End If
               End If

               ''VALIDA FECHA MENOR A LA FECHA DE FINALIZACION DEL PASO SIGUIENTE
               If pbValidaFechaFinalizaPasoSig And piPasoSiguiente <> 0 Then
                  Dim ldFechafinalizaPaso As DateTime = ObtenerFechaFinalizaPaso(piPasoSiguiente, TieneProrrogaAprobada)

                  If Not IsNothing(ldFechafinalizaPaso) Then
                     If ldFecha.Date > ldFechafinalizaPaso.Date Then
                        pdFecha = DateTime.MinValue
                        Return 2163
                     End If
                  End If
               End If
            End If
         End If
         'End If
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro.csv.vb, ValidarFechaGeneral", "")
      End Try

      Return -1
   End Function

    ''' <summary>
    ''' Recupera e inserta el objeto de la visita
    ''' </summary>
    ''' <param name="liIdObjeto"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InsertaObjetoVisitaAux(liIdObjeto As Integer) As Integer
        Dim USUARIOS = WebConfigurationManager.AppSettings("UsuarioSisan")
        Dim PASSWORDS = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("PassEncSisan").ToString())

        Dim EnvioSisan As New wsSisanReg.RegistroExterno
        Dim credentialsS As NetworkCredential = New NetworkCredential(USUARIOS, PASSWORDS, "ADCONSAR")
        EnvioSisan.Credentials = credentialsS
        Dim dsProcesos = EnvioSisan.ObtenerCatalogoProcesos

        Dim dt = dsProcesos.Catalogo.Tables(0)

        If Not IsNothing(dt) Then
            If dt.Rows.Count > 0 Then
                For Each lrRow As DataRow In dt.Rows
                    If CInt(lrRow(0)) = liIdObjeto Then
                        Try
                            Return AccesoBD.InsertaObjetoVisita(liIdObjeto, lrRow(2).ToString(), Constantes.AREA_VO)
                        Catch ex As Exception
                            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "wcfRegistro.csv.vb, InsertaObjetoVisitaAux", "")
                        End Try
                    End If
                Next
            End If
        End If

        Return -1
    End Function

    Public Function EscribeBitacoraYEventLog(idUsuario As String, IdError As Integer, procedimiento As String, comentarios As String) As Boolean
        If idUsuario.Trim() <> "" Then
            Dim objUsuario As New Entities.Usuario(idUsuario)

            If objUsuario.Existe Then
                Dim lsSesionSisvig As String = DateTime.Now.ToString("ddMMyyyyhhmmss")
                objUsuario.RegistrarSesionSisvig(lsSesionSisvig)

                If IdError = -1 Then
                    Dim bitacora As New Conexion.Bitacora(procedimiento & " - comentarios(" & comentarios & "), resultado (El proceso se ejecutó correctamente).", lsSesionSisvig, objUsuario.IdentificadorUsuario)
                    bitacora.Finalizar(True)
                    'GUARDA EN EL LOG DE EVENTOS EL ERROR
                    Utilerias.ControlErrores.EscribirEvento(procedimiento & " - comentarios(" & comentarios & "), resultado (El proceso se ejecutó correctamente).", EventLogEntryType.Information, "EscribeBitacoraYEventLog", "")
                Else
                    'Buscar leyenda de error en bd
                    Dim mensajeError = AccesoBD.ObtenMensajeDeError(IdError)
                    Dim bitacora As New Conexion.Bitacora(procedimiento & " - comentarios(" & comentarios & "), mensaje de error(" & mensajeError.ToString() & ")", lsSesionSisvig, objUsuario.IdentificadorUsuario)
                    bitacora.Finalizar(True)

                    'GUARDA EN EL LOG DE EVENTOS EL ERROR
                    Utilerias.ControlErrores.EscribirEvento(procedimiento & " - comentarios(" & comentarios & "), mensaje de error(" & mensajeError.ToString() & ")", EventLogEntryType.Error, "EscribeBitacoraYEventLog", "")
                End If
            End If
        End If

        Return True
    End Function
End Class
