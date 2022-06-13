﻿Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports Utilerias
Imports System.Reflection
Imports System.ComponentModel

Public Class AccesoBD
   ''' <summary>
   ''' Trae las visitas de la base de datos
   ''' </summary>
   ''' <param name="idArea">Area del usuario</param>
   ''' <param name="idTipoConsulta">si es Constantes.CopFoliosTConsul.TodosLosPasos trae todos los registros
   '''                              si es Constantes.CopFoliosTConsul.Del1al17 trae todas las visitas del paso 1 al 17, para la funcionalidad de coppiar folios.
   ''' </param>
   ''' <returns></returns>
   ''' <remarks>agc modifico</remarks>
   ''' 
   Public Property ObtenerQuery As String

   Public Shared Function consultarVisitas(Optional idArea As Integer = -1,
                                           Optional idTipoConsulta As Integer = Constantes.CopFoliosTConsul.TodosLosPasos,
                                           Optional psUsuarioActual As String = "",
                                           Optional psPerfilUsuActual As Integer = 0,
                                           Optional piIdSubvisitaPadre As Integer = 0,
                                           Optional psIdRespInsp As String = "",
                                           Optional psInspOperativo As String = "",
                                           Optional psAbogadosVisita As String = "") As DataTable

      Dim dt As DataTable = Nothing

      Dim lstBandejaVisitas As New List(Of BandejaVisita)

      Dim con As Conexion.SQLServer = Nothing

      Try
         ''Obtiene el paso 18
         Dim objPaso18 As Paso
         objPaso18 = (From obPaso As Paso In AccesoBD.getCatalogoPasos(18) Where obPaso.IdPaso = 18 Select obPaso).FirstOrDefault()


         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_BANDEJA_VISITA")

         Dim sqlParameter(7) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_AREA", idArea)
         sqlParameter(1) = New SqlParameter("@I_ID_TIPO_CONSULTA", idTipoConsulta)
         sqlParameter(2) = New SqlParameter("@I_ID_USUARIO", psUsuarioActual)
         sqlParameter(3) = New SqlParameter("@I_ID_PERFIL", psPerfilUsuActual)
         sqlParameter(4) = New SqlParameter("@I_ID_VISITA_PADRE", piIdSubvisitaPadre)
         sqlParameter(5) = New SqlParameter("@I_ID_SUP_INSP", psIdRespInsp)
         sqlParameter(6) = New SqlParameter("@I_ID_INS_INSP", psInspOperativo)
         sqlParameter(7) = New SqlParameter("@I_ID_ABOGADO", psAbogadosVisita)

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, sqlParameter)

         While dataReader.Read()

            Dim datosVisita As New BandejaVisita()

                datosVisita.IdVisitaGenerado = dataReader.GetInt32(0)

                datosVisita.FolioVisita = dataReader.GetString(1)

            If dataReader.IsDBNull(2) Then
               datosVisita.FechaInicioVisita = DateTime.MinValue
            Else
               datosVisita.FechaInicioVisita = dataReader.GetDateTime(2)
            End If

            If datosVisita.FechaInicioVisita = DateTime.MinValue Then
               datosVisita.StrFechaInicioVisita = String.Empty
            Else
               datosVisita.StrFechaInicioVisita = datosVisita.FechaInicioVisita.ToString("dd/MM/yyyy")
            End If

            If datosVisita.FechaInicioVisita = DateTime.MinValue Then
               datosVisita.FechaInicioFormatoEu = String.Empty
            Else
               datosVisita.FechaInicioFormatoEu = datosVisita.FechaInicioVisita.ToString("yyyy/MM/dd")
            End If

            datosVisita.IdEntidad = dataReader.GetInt32(3)

                If dataReader.IsDBNull(4) Then
                    datosVisita.DscEntidad = String.Empty
                Else
                    datosVisita.DscEntidad = dataReader.GetString(4)
                End If

                If dataReader.IsDBNull(5) Then
                    datosVisita.DscSubEntidad = String.Empty
                Else
                    datosVisita.DscSubEntidad = dataReader.GetString(5)
                End If

                datosVisita.IdArea = dataReader.GetInt32(6)

                If dataReader.IsDBNull(7) Then
                    datosVisita.DscArea = String.Empty
                Else
                    datosVisita.DscArea = dataReader.GetString(7)
                End If

                datosVisita.IdTipoVisita = dataReader.GetInt32(8)

                If dataReader.IsDBNull(9) Then
                    datosVisita.DscTipoVisita = String.Empty
                Else
                    datosVisita.DscTipoVisita = dataReader.GetString(9)
                End If

                datosVisita.IdPasoActual = dataReader.GetInt32(10)
                datosVisita.StrIdPasoActual = datosVisita.IdPasoActual.ToString()

                If dataReader.IsDBNull(11) Then
                    datosVisita.FechaIniciaPaso = DateTime.MinValue
                Else
                    datosVisita.FechaIniciaPaso = dataReader.GetDateTime(11)
                End If

                If dataReader.IsDBNull(12) Then
                    datosVisita.IdInspectorResponsable = String.Empty
                Else
                    datosVisita.IdInspectorResponsable = dataReader.GetString(12)
                End If

                If dataReader.IsDBNull(13) Then
                    datosVisita.NombreInspectorResponsable = String.Empty
                Else
                    datosVisita.NombreInspectorResponsable = dataReader.GetString(13)
                End If

                If dataReader.IsDBNull(14) Then
                    datosVisita.IdAbogadoSancion = String.Empty
                Else
                    datosVisita.IdAbogadoSancion = dataReader.GetString(14)
                End If

                If dataReader.IsDBNull(15) Then
                    datosVisita.NombreAbogadoSancion = String.Empty
                Else
                    datosVisita.NombreAbogadoSancion = dataReader.GetString(15)
                End If

                If dataReader.IsDBNull(16) Then
                    datosVisita.IdAbogadoAsesor = String.Empty
                Else
                    datosVisita.IdAbogadoAsesor = dataReader.GetString(16)
                End If

                If dataReader.IsDBNull(17) Then
                    datosVisita.NombreAbogadoAsesor = String.Empty
                Else
                    datosVisita.NombreAbogadoAsesor = dataReader.GetString(17)
                End If

                If dataReader.IsDBNull(18) Then
                    datosVisita.IdAbogadoContencioso = String.Empty
                Else
                    datosVisita.IdAbogadoContencioso = dataReader.GetString(18)
                End If

                If dataReader.IsDBNull(19) Then
                    datosVisita.NombreAbogadoContencioso = String.Empty
                Else
                    datosVisita.NombreAbogadoContencioso = dataReader.GetString(19)
                End If

                If dataReader.IsDBNull(20) Then
                    datosVisita.TieneSubvisitas = Constantes.Falso
                Else
                    datosVisita.TieneSubvisitas = dataReader.GetInt32(20)
                End If

                If dataReader.IsDBNull(21) Then
                    datosVisita.DscObjetoVisita = String.Empty
                Else
                    datosVisita.DscObjetoVisita = dataReader.GetString(21)
                End If

                If dataReader.IsDBNull(22) Then
                    datosVisita.IdSubEntidad = 0
                Else
                    datosVisita.IdSubEntidad = dataReader.GetInt32(22)
                End If

                'If dataReader.IsDBNull(22) Then
                '   datosVisita.DscSubEntidad = String.Empty
                'Else
                '   datosVisita.DscSubEntidad = dataReader.GetString(22)
                'End If

                If dataReader.IsDBNull(23) Then
               datosVisita.FechaRegistro = String.Empty
            Else
               datosVisita.FechaRegistro = dataReader.GetDateTime(23).ToString("yyyy/MM/dd")
            End If

            If dataReader.IsDBNull(23) Then
               datosVisita.FechaRegistroD = DateTime.MinValue
            Else
               datosVisita.FechaRegistroD = dataReader.GetDateTime(23)
            End If

            If dataReader.IsDBNull(23) Then
               datosVisita.FechaRegistroFormateada = String.Empty
            Else
               datosVisita.FechaRegistroFormateada = dataReader.GetDateTime(23).ToString("dd/MM/yyyy")
            End If

            If dataReader.IsDBNull(24) Then
               datosVisita.DuracionMinimaPasoActual = 0
            Else
               datosVisita.DuracionMinimaPasoActual = dataReader.GetInt32(24)
            End If

            If dataReader.IsDBNull(25) Then
               datosVisita.FechaFinPaso = DateTime.MinValue
            Else
               datosVisita.FechaFinPaso = dataReader.GetDateTime(25)
            End If

            If dataReader.IsDBNull(26) Then
               datosVisita.EstatusVisitaInt = String.Empty
            Else
               datosVisita.EstatusVisitaInt = dataReader.GetInt32(26)
            End If

            If dataReader.IsDBNull(27) Then
               datosVisita.EstatusVisitaDsc = String.Empty
            Else
               datosVisita.EstatusVisitaDsc = dataReader.GetString(27)
            End If

            If dataReader.IsDBNull(28) Then
               datosVisita.IdEstatusActual = Constantes.EstatusPaso.EnRevisionEspera
            Else
               datosVisita.IdEstatusActual = dataReader.GetInt32(28)
            End If

            If dataReader.IsDBNull(29) Then
               datosVisita.OrdenVisita = String.Empty
            Else
               datosVisita.OrdenVisita = dataReader.GetString(29)
            End If

            If dataReader.IsDBNull(30) Then
               datosVisita.DiasPlazosLegalesVisita = 0
            Else
               datosVisita.DiasPlazosLegalesVisita = dataReader.GetInt32(30)
            End If
                'AQUI EL NUEVO CAMPO FOLIO SISAN 
                If dataReader.IsDBNull(31) Then
                    datosVisita.Folio_SISAN = "Sin Folio"
                Else
                    datosVisita.Folio_SISAN = dataReader.GetString(31)
                End If

                If dataReader.IsDBNull(32) Then
                    datosVisita.ProrrogaAprobada = False
                Else
                    datosVisita.ProrrogaAprobada = dataReader.GetBoolean(32)
                End If

                ''''''''''EMPEZAR DE AQUI PARA ATRAS LO SIGUIENTE NO TOCAR, DIGO SI SE AGREGAN NUEVOS DATOS DE CONSULTA
                '''''''''' INCREMENTAR EN UNO liUltimaPos
                Dim liUltimaPos As Integer = 33

                If datosVisita.EstatusVisitaInt = Constantes.EstatusVisita.Cerrada Or
                    datosVisita.EstatusVisitaInt = Constantes.EstatusVisita.Cancelada Then
               datosVisita.DiasPlazosLegalesVisitaDsc = "----"
            Else
               If dataReader.IsDBNull(liUltimaPos) Then
                  datosVisita.DiasPlazosLegalesVisitaDsc = String.Empty
               Else
                  datosVisita.DiasPlazosLegalesVisitaDsc = dataReader.GetString(liUltimaPos)
               End If
            End If

            If dataReader.IsDBNull(liUltimaPos + 1) Then
               datosVisita.DiasHabilesTotalesVisita = 0
            Else
               datosVisita.DiasHabilesTotalesVisita = dataReader.GetInt32(liUltimaPos + 1)
            End If


            If dataReader.IsDBNull(liUltimaPos + 2) Then
               datosVisita.DiasPasoActualTranscurridos = 0
            Else
               datosVisita.DiasPasoActualTranscurridos = dataReader.GetInt32(liUltimaPos + 2)
               If datosVisita.DiasPasoActualTranscurridos < 0 Then datosVisita.DiasPasoActualTranscurridos = 0 ''TRUQUEAR :)
            End If

            If datosVisita.EstatusVisitaInt = Constantes.EstatusVisita.Cerrada Or
                    datosVisita.EstatusVisitaInt = Constantes.EstatusVisita.Cancelada Then
               datosVisita.DiasTranscurridos = "----"
            Else
               If datosVisita.FechaIniciaPaso = DateTime.MinValue Then
                  datosVisita.DiasTranscurridos = "0" + " de " + datosVisita.DuracionMinimaPasoActual.ToString()
                  datosVisita.IdEstatusSemaforo = Constantes.Semaforo.Verde
               Else
                  ''CONDICIONAR EL ESTATUS YA QUE CUANDO EL PASO SE REACTIVA YA TIENE FECHA FIN PERO DEBE DE SEGUIR CONTANDO
                  If datosVisita.FechaFinPaso <> DateTime.MinValue And (datosVisita.IdPasoActual = 19 Or datosVisita.IdPasoActual = 37) Then
                     datosVisita.DiasTranscurridos = datosVisita.DiasPasoActualTranscurridos.ToString()
                  Else
                     If datosVisita.IdPasoActual = 19 Then
                        ''Poner el maximo si la visita tiene menos de 80 dias y no tiene prorroga
                        If datosVisita.DiasHabilesTotalesVisita < Constantes.DiasPaso19 And datosVisita.EstatusVisitaInt <> Constantes.EstatusVisita.ConProrroga Then
                           datosVisita.DiasTranscurridos = datosVisita.DiasPasoActualTranscurridos.ToString() + " de " + objPaso18.NumDiasMax.ToString()
                        Else
                           datosVisita.DiasTranscurridos = datosVisita.DiasPasoActualTranscurridos.ToString() + " de " + objPaso18.NumDiasMin.ToString()
                        End If
                     Else
                        'If datosVisita.IdPasoActual = 3 Then
                        '    Dim fechaRegVisita As Date = CDate(datosVisita.FechaRegistro.ToString())
                        '    Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

                        '    If (Convert.ToDateTime(fechaRegVisita).ToString("yyyy/MM/dd") < Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
                        '        datosVisita.DiasTranscurridos = datosVisita.DiasPasoActualTranscurridos.ToString() + " de " + datosVisita.DuracionMinimaPasoActual.ToString()
                        '    Else
                        '        datosVisita.DiasTranscurridos = datosVisita.DiasPasoActualTranscurridos.ToString() + " de 0"
                        '    End If
                        'Else
                        datosVisita.DiasTranscurridos = datosVisita.DiasPasoActualTranscurridos.ToString() + " de " + datosVisita.DuracionMinimaPasoActual.ToString()
                        'End If
                     End If
                  End If
               End If
            End If


            'PARA LAS VISITAS DEL PROCESO ANTERIOR NO APLICA PONER EL SUPERÁVIT/DÉFICIT
            Dim fechaRegVisita As Date = CDate(datosVisita.FechaRegistro.ToString())
            Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)
            If (Convert.ToDateTime(fechaRegVisita).ToString("yyyy/MM/dd") >= Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) And datosVisita.IdPasoActual > 3 Then
               If datosVisita.ProrrogaAprobada Then
                  'Con prórroga
                  If datosVisita.IdArea = 36 Then
                     datosVisita.DiasAcumCSPro = consultarDiasAcumCSProrroga(datosVisita.IdArea, datosVisita.IdPasoActual, 8) - datosVisita.DiasPasoActualTranscurridos
                  Else
                     datosVisita.DiasAcumCSPro = consultarDiasAcumCSProrroga(-1, datosVisita.IdPasoActual, 8) - datosVisita.DiasPasoActualTranscurridos
                  End If
               Else
                  'Sin prórroga
                  If datosVisita.IdArea = 36 Then
                     datosVisita.DiasAcumCSPro = consultarDiasAcumCSProrroga(datosVisita.IdArea, datosVisita.IdPasoActual, 4) - datosVisita.DiasPasoActualTranscurridos
                  Else
                     datosVisita.DiasAcumCSPro = consultarDiasAcumCSProrroga(-1, datosVisita.IdPasoActual, 4) - datosVisita.DiasPasoActualTranscurridos
                  End If
               End If
            Else
               datosVisita.DiasAcumCSPro = 9999999
            End If


            ''Semaforo
            If datosVisita.EstatusVisitaInt = Constantes.EstatusVisita.Cerrada Or
                datosVisita.EstatusVisitaInt = Constantes.EstatusVisita.Cancelada Then
               datosVisita.IdEstatusSemaforo = Constantes.Semaforo.Gris
            Else
               If datosVisita.DiasPasoActualTranscurridos < (datosVisita.DuracionMinimaPasoActual - 1) Then
                  datosVisita.IdEstatusSemaforo = Constantes.Semaforo.Verde
               Else
                  If datosVisita.DiasPasoActualTranscurridos < datosVisita.DuracionMinimaPasoActual Then
                     datosVisita.IdEstatusSemaforo = Constantes.Semaforo.Amarillo
                  Else
                     datosVisita.IdEstatusSemaforo = Constantes.Semaforo.Rojo
                  End If
               End If
            End If

            If datosVisita.IdPasoActual = 3 Then
               'Dim fechaRegVisita As Date = CDate(datosVisita.FechaRegistro.ToString())
               'Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

               'SI LA VISITA SE REGISTRÓ DESPUÉS DE LA FECHA DE PUESTA EN PRODUCCIÓN DEL PROCESO DE INSPECCIÓN 2017 SE PONE EL SEMÁFORO EN VERDE Y DETIENE CONTEO PARA PASO 3
               If (Convert.ToDateTime(fechaRegVisita).ToString("yyyy/MM/dd") >= Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
                  datosVisita.IdEstatusSemaforo = Constantes.Semaforo.Verde
               End If
            End If

            ''Adrega a la lista
            lstBandejaVisitas.Add(datosVisita)

         End While

         If Not IsNothing(lstBandejaVisitas) And lstBandejaVisitas.Count > 0 Then

            Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(GetType(BandejaVisita))
            dt = New DataTable()
            For i As Integer = 0 To properties.Count - 1
               Dim [property] As PropertyDescriptor = properties(i)
               dt.Columns.Add([property].Name, [property].PropertyType)
            Next
            Dim values As Object() = New Object(properties.Count - 1) {}
            For Each item As BandejaVisita In lstBandejaVisitas
               For i As Integer = 0 To values.Length - 1
                  values(i) = properties(i).GetValue(item)
               Next
               dt.Rows.Add(values)
            Next

         End If

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getEncabezadoReporteVisita", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try


      'F_FECH_ENVIA_SANSIONES
      dt.Columns.Add("F_FECH_ENVIA_SANSIONES", System.Type.GetType("System.String"))

      'Recorrer La columna FOLIO SISAN'
      For i As Integer = 0 To dt.Rows.Count - 1
         Dim dr As DataRow = dt.Rows(i)
         dr("F_FECH_ENVIA_SANSIONES") = ConexionSISAN.ObtenerFechaSancion(dr("Folio_SISAN"))
      Next



      Return dt
   End Function

   ''' <summary>
   ''' Trae los días acumulados con o sin prórroga
   ''' </summary>
   ''' <param name="idArea">Area del usuario</param>
   ''' <param name="idPaso">Es el id del paso a obtener valores.</param>
   ''' <param name="idParam"> Es el id de la opción con base a la siguiente lista:
   ''' 1 = Sin prórroga, SI presentación, SI sanción
   ''' 2 = Sin prórroga, SI presentación, NO sanción
   ''' 3 = Sin prórroga, NO presentación, SI sanción
   ''' 4 = Días acumulados sin prórroga
   ''' 5 = Con prórroga, SI presentación, SI sanción
   ''' 6 = Con prórroga, SI presentación, NO sanción
   ''' 7 = Con prórroga, NO presentación, SI sanción
   ''' 8 = Días acumulados con prórroga</param>
   ''' </param>
   ''' <returns></returns>
   ''' <remarks>MCS creó</remarks>
   ''' 
   Public Shared Function consultarDiasAcumCSProrroga(ByVal idArea As Integer,
                                           ByVal idPaso As Integer, ByVal idParam As Integer) As Integer

      Dim con As New Conexion.SQLServer

      Dim dr As SqlClient.SqlDataReader = Nothing
      Dim dias As Integer = 0
      Dim campoDias As String = ""
      Dim sqlQuery = ""
      Try

         If idParam = 1 Then
            campoDias = "I_SIN_PROR_SI_PRES_SI_SAN"
         ElseIf idParam = 2 Then
            campoDias = "I_SIN_PROR_SI_PRES_NO_SAN"
         ElseIf idParam = 3 Then
            campoDias = "I_SIN_PROR_NO_PRES_SI_SAN"
         ElseIf idParam = 4 Then
            campoDias = "I_ACUM_SIN_PRORROGA"
         ElseIf idParam = 5 Then
            campoDias = "I_CON_PROR_SI_PRES_SI_SAN"
         ElseIf idParam = 6 Then
            campoDias = "I_CON_PROR_SI_PRES_NO_SAN"
         ElseIf idParam = 7 Then
            campoDias = "I_CON_PROR_NO_PRES_SI_SAN"
         ElseIf idParam = 8 Then
            campoDias = "I_ACUM_CON_PRORROGA"
         End If

         sqlQuery = "SELECT " & campoDias & " FROM dbo.BDS_C_GR_DIAS_ACUM_CON_SIN_PRORROGA_V17 WHERE I_ID_AREA = " & idArea & " AND I_ID_PASO = " & idPaso & ";"

         dr = con.ConsultarDR(sqlQuery)

         If dr IsNot Nothing Then
            dr.Read()
            If dr.HasRows Then
               If Not IsDBNull(dr.Item(0)) Then
                  dias = dr.Item(0)
               End If
            Else
               dias = 0
            End If
         End If

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, consultarDiasAcumCSProrroga", "")
      Finally
         If dr IsNot Nothing Then
            dr.Close()
            dr = Nothing
         End If
         If con IsNot Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try

      Return dias

   End Function

   Public Shared Function consultarEsVisitaDeSISVIG(ByVal idVisita As Integer) As Boolean

      Dim con As New Conexion.SQLServer

      Dim dr As SqlClient.SqlDataReader = Nothing
      Dim aux As Boolean = False
      Dim sqlQuery = ""
      Try

         sqlQuery = "SELECT * FROM dbo.BDS_D_VS_VISITA WHERE I_ID_VISITA = " & idVisita & " AND B_FLAG_SISVIG = 1 AND ID_VISITA_SISVIG > 0;"

         dr = con.ConsultarDR(sqlQuery)

         If dr IsNot Nothing Then
            dr.Read()
            If dr.HasRows Then
               If Not IsDBNull(dr.Item(0)) Then
                  'aux = dr.Item(0)
                  aux = True
               End If
            Else
               aux = False
            End If
         End If

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, consultarEsVisitaDeSISVIG", "")
      Finally
         If dr IsNot Nothing Then
            dr.Close()
            dr = Nothing
         End If
         If con IsNot Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try

      Return aux

   End Function

   Public Shared Function registrarVisita(ByVal vistia As Visita, ByVal con As Conexion.SQLServer,
                                          ByVal tran As SqlClient.SqlTransaction,
                                          Optional ByRef psIdVisita As String = "",
                                          Optional piTipoOperacion As Integer = Constantes.OPERCION.Insertar) As Integer
      Dim idVisitaGenerado As Integer = 0

      Dim dataReader As SqlDataReader = Nothing
      Try
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_REGISTRAR_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_FOLIO", vistia.FolioVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA", vistia.IdArea))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO", vistia.Usuario.IdentificadorUsuario))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_REGISTRO", vistia.FechaRegistro))

         If vistia.FechaInicioVisita <> DateTime.MinValue Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_VISITA", vistia.FechaInicioVisita))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_VISITA", DBNull.Value))
         End If

         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_ENTIDAD", vistia.IdEntidad))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_TIPO_VISITA", vistia.IdTipoVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_ESTATUS_ACTUAL", vistia.IdEstatusActual))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO_ACTUAL", vistia.IdPasoActual))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_ENTIDAD", vistia.NombreEntidad))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_ES_CANCELADA", vistia.EsCancelada))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_MOTIVO_CANCELACION", vistia.MotivoCancelacion))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_TIPO_ENTIDAD", vistia.IdTipoEntidad))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@N_ID_OBJETO_VISITA", vistia.IdObjetoVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_OBJETO_VISITA_OTRO", vistia.DscObjetoVisitaOtro))

         If vistia.FechaCancela <> DateTime.MinValue Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_CANCELA", vistia.FechaCancela))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_CANCELA", DBNull.Value))
         End If

         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@DSC_TIPO_ENTIDAD", vistia.DscTipoEntidad))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_SUBENTIDAD", vistia.IdSubentidad))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@DSC_SUBENTIDAD", vistia.DscSubentidad))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@DSC_VISITA", vistia.DescripcionVisita))

         If Not IsNothing(vistia.IdVisitaPadreSubvisita) Then
            If vistia.IdVisitaPadreSubvisita <> 0 Then
               SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA_PADRE_SB", vistia.IdVisitaPadreSubvisita))
            End If
         End If

         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@COMENTARIOS", vistia.ComentariosIniciales))

         If piTipoOperacion = Constantes.OPERCION.Insertar Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@TIPO_OPERACION", Constantes.OPERCION.Insertar))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@TIPO_OPERACION", Constantes.OPERCION.Actualizar))
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@ID_VISITA_ACTUALIZAR", vistia.IdVisitaGenerado))
         End If

         If Not Fechas.Vacia(vistia.Fecha_AcuerdoVul) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_ACUERDO_VULNERA", vistia.Fecha_AcuerdoVul))
         End If

         If vistia.OrdenVisita <> "" Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_ORDEN_VISITA", vistia.OrdenVisita))
         End If

         If vistia.IdVisitaSisvig <> 0 Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@ID_VISITA_SISVIG", vistia.IdVisitaSisvig))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@ID_VISITA_SISVIG", DBNull.Value))
         End If

         dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
            idVisitaGenerado = Convert.ToInt32(dataReader("ID"))
            psIdVisita = dataReader("ID_CADENA").ToString()
         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarVisita", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return idVisitaGenerado

   End Function
   Public Shared Function registrarSupervisor(ByVal idVisitaGenerado As Integer, ByVal supervisor As SupervisorAsignado, ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
      Dim registroExitoso As Boolean = True

      Dim dataReader As SqlDataReader = Nothing
      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_REGISTRAR_SUPERVISOR")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_SUPERVISOR_ASIGNADO", supervisor.Id))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_NOMBRE_SUPERVISOR_ASIGNADO", supervisor.Nombre))

         dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
         End While

      Catch ex As Exception
         registroExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarSupervisor", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return registroExitoso

   End Function
   Public Shared Function registrarInspector(ByVal idVisitaGenerado As Integer, ByVal inspector As InspectorAsignado, ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
      Dim registroExitoso As Boolean = True

      Dim dataReader As SqlDataReader = Nothing
      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_REGISTRAR_INSPECTOR")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_INSPECTOR_ASIGNADO", inspector.Id))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_NOMBRE_INSPECTOR_ASIGNADO", inspector.Nombre))

         dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
         End While

      Catch ex As Exception
         registroExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarInspector", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return registroExitoso

   End Function

   Public Shared Function registrarPaso(ByVal paso As PasoProcesoVisita, ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
      Dim registroExitoso As Boolean = True

      Dim dataReader As SqlDataReader = Nothing
      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_REGISTRAR_PASO_PROCESO_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", paso.IdVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", paso.IdPaso))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_INI_PASO", paso.FechaInicio))
         If paso.FechaFin <> DateTime.MinValue Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_FIN_PASO", paso.FechaFin))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_FIN_PASO", DBNull.Value))
         End If
         If Not IsNothing(paso.EsNotificado) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_NOTIFICADO", paso.EsNotificado))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_NOTIFICADO", DBNull.Value))
         End If
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA_NOTIFICADA", paso.IdAreaNotificada))
         If Not IsNothing(paso.IdUsuarioNotificado) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO_NOTIFICADO", paso.IdUsuarioNotificado))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO_NOTIFICADO", DBNull.Value))
         End If
         If Not IsNothing(paso.EmailUsuarioNotificado) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_EMAIL_USUARIO_NOTIFICADO", paso.EmailUsuarioNotificado))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_EMAIL_USUARIO_NOTIFICADO", DBNull.Value))
         End If
         If Not IsNothing(paso.TieneProrroga) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_TIENE_PRORROGA", paso.TieneProrroga))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_TIENE_PRORROGA", DBNull.Value))
         End If
         If paso.FechaNotifica <> DateTime.MinValue Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_NOTIFICA", paso.FechaNotifica))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_NOTIFICA", DBNull.Value))
         End If

            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@SUB_VISITAS", paso.SubVisitasSeleccionadas))
            'SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_MOVIMIENTO", paso.IdMovimiento))

            dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
         End While

      Catch ex As Exception
         registroExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarPaso", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return registroExitoso

   End Function

   Public Shared Function registrarPaso_V17(ByVal paso As PasoProcesoVisita, ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
      Dim registroExitoso As Boolean = True

      Dim dataReader As SqlDataReader = Nothing
      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_REGISTRAR_PASO_PROCESO_VISITA_V17")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", paso.IdVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", paso.IdPaso))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_INI_PASO", paso.FechaInicio))
         If paso.FechaFin <> DateTime.MinValue Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_FIN_PASO", paso.FechaFin))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_FIN_PASO", DBNull.Value))
         End If
         If Not IsNothing(paso.EsNotificado) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_NOTIFICADO", paso.EsNotificado))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_NOTIFICADO", DBNull.Value))
         End If
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA_NOTIFICADA", paso.IdAreaNotificada))
         If Not IsNothing(paso.IdUsuarioNotificado) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO_NOTIFICADO", paso.IdUsuarioNotificado))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO_NOTIFICADO", DBNull.Value))
         End If
         If Not IsNothing(paso.EmailUsuarioNotificado) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_EMAIL_USUARIO_NOTIFICADO", paso.EmailUsuarioNotificado))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_EMAIL_USUARIO_NOTIFICADO", DBNull.Value))
         End If
         If Not IsNothing(paso.TieneProrroga) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_TIENE_PRORROGA", paso.TieneProrroga))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_TIENE_PRORROGA", DBNull.Value))
         End If
         If paso.FechaNotifica <> DateTime.MinValue Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_NOTIFICA", paso.FechaNotifica))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_NOTIFICA", DBNull.Value))
         End If

         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@SUB_VISITAS", paso.SubVisitasSeleccionadas))
            'SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_MOVIMIENTO", paso.IdMovimiento))
            dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
         End While

      Catch ex As Exception
         registroExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarPaso_V17", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return registroExitoso

   End Function

   Public Shared Function registrarPasoSinTransaccion(ByVal paso As PasoProcesoVisita) As Boolean
      Dim registroExitoso As Boolean = True

      Dim con As Conexion.SQLServer = Nothing

      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_REGISTRAR_PASO_PROCESO_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", paso.IdVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", paso.IdPaso))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_INI_PASO", paso.FechaInicio))
         If paso.FechaFin <> DateTime.MinValue Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_FIN_PASO", paso.FechaFin))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_FIN_PASO", DBNull.Value))
         End If
         If Not IsNothing(paso.EsNotificado) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_NOTIFICADO", paso.EsNotificado))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_NOTIFICADO", DBNull.Value))
         End If
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA_NOTIFICADA", paso.IdAreaNotificada))
         If Not IsNothing(paso.IdUsuarioNotificado) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO_NOTIFICADO", paso.IdUsuarioNotificado))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO_NOTIFICADO", DBNull.Value))
         End If
         If Not IsNothing(paso.EmailUsuarioNotificado) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_EMAIL_USUARIO_NOTIFICADO", paso.EmailUsuarioNotificado))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_EMAIL_USUARIO_NOTIFICADO", DBNull.Value))
         End If
         If Not IsNothing(paso.TieneProrroga) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_TIENE_PRORROGA", paso.TieneProrroga))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_TIENE_PRORROGA", DBNull.Value))
         End If
         If paso.FechaNotifica <> DateTime.MinValue Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_NOTIFICA", paso.FechaNotifica))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_NOTIFICA", DBNull.Value))
         End If

         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@SUB_VISITAS", paso.SubVisitasSeleccionadas))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
         End While

      Catch ex As Exception
         registroExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarPaso", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return registroExitoso

   End Function

    Public Shared Function registrarEstatusPaso(ByVal PasoPrV As PasoProcesoVisita, ByVal estatusPaso As EstatusPaso,
                                               ByVal con As Conexion.SQLServer,
                                               ByVal tran As SqlClient.SqlTransaction) As Boolean
        Dim registroExitoso As Boolean = True

        Dim dataReader As SqlDataReader = Nothing
        Try

            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_REGISTRAR_ESTATUS_PASO")

            Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", estatusPaso.IdVisitaGenerado))
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", estatusPaso.IdPaso))
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_ESTATUS", estatusPaso.IdEstatus))
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_REGISTRO", estatusPaso.FechaRegistro))
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO", estatusPaso.IdUsuario))

            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@ES_REGISTRO", estatusPaso.EsRegistro))
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@ID_AREA_ACTUAL", estatusPaso.IdAreaActual))


            If Not IsNothing(estatusPaso.Comentarios) Then
                SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_COMENTARIOS", estatusPaso.Comentarios))
            Else
                SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_COMENTARIOS", WebConfigurationManager.AppSettings("msgLblSinComentarios").ToString()))
            End If


            If IsNothing(estatusPaso.TipoComentario) Then
                SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_TIPO_COMENTARIO", Constantes.TipoComentario.USUARIO))
            ElseIf estatusPaso.TipoComentario.Trim().Length < 1 Then
                SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_TIPO_COMENTARIO", Constantes.TipoComentario.USUARIO))
            Else
                SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_TIPO_COMENTARIO", estatusPaso.TipoComentario))
            End If

            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@SUB_VISITAS", estatusPaso.SubVisitasSeleccionadas))
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_MOVIMIENTO", PasoPrV.IdMovimiento))

            dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

            While dataReader.Read()
            End While

        Catch ex As Exception
            registroExitoso = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarEstatusPaso", "")
        Finally
            If Not IsNothing(dataReader) Then
                dataReader.Close()
                dataReader = Nothing
            End If
        End Try

        Return registroExitoso

    End Function
    Public Shared Function registrarEstatusPasoSinTransaccion(ByVal estatusPaso As EstatusPaso) As Boolean
      Dim registroExitoso As Boolean = True

      Dim con As Conexion.SQLServer = Nothing
      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_REGISTRAR_ESTATUS_PASO")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", estatusPaso.IdVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", estatusPaso.IdPaso))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_ESTATUS", estatusPaso.IdEstatus))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_REGISTRO", estatusPaso.FechaRegistro))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO", estatusPaso.IdUsuario))

         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@ES_REGISTRO", estatusPaso.EsRegistro))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@ID_AREA_ACTUAL", estatusPaso.IdAreaActual))

         If Not IsNothing(estatusPaso.Comentarios) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_COMENTARIOS", estatusPaso.Comentarios))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_COMENTARIOS", WebConfigurationManager.AppSettings("msgLblSinComentarios").ToString()))
         End If

         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@SUB_VISITAS", estatusPaso.SubVisitasSeleccionadas))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
         End While

      Catch ex As Exception
         registroExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarEstatusPaso", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return registroExitoso

   End Function


   Public Shared Function registrarObjetoVisita(ByVal idVisitaGenerado As Integer, psObjetosVisita As String, ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
      Dim registroExitoso As Boolean = True

      Dim dataReader As SqlDataReader = Nothing
      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_REGISTRA_OBJETO_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@V_LISTA_OBJETO_VISITA", psObjetosVisita))

         dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
         End While

      Catch ex As Exception
         registroExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarSupervisor", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return registroExitoso

   End Function


   Public Shared Function actualizarPasoNotificadoSinTransaccion(ByVal idVisitaGenerado As Integer, ByVal idPaso As Integer,
                                                                 ByVal esNotificado As Boolean, ByVal idAreaNotificada As Integer,
                                                                 ByVal usuarioNotificado As String, ByVal emailUsuarioNotificado As String,
                                                                 ByVal fechaNotifica As DateTime) As Boolean
      Dim registroExitoso As Boolean = True

      Dim con As Conexion.SQLServer = Nothing
      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spU_BDS_GRL_ACTUALIZAR_PASO_NOTIFICADO")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", idPaso))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_NOTIFICADO", esNotificado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA_NOTIFICADA", idAreaNotificada))

         If Not IsNothing(usuarioNotificado) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO_NOTIFICADO", usuarioNotificado))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO_NOTIFICADO", DBNull.Value))
         End If

         If Not IsNothing(emailUsuarioNotificado) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_EMAIL_USUARIO_NOTIFICADO", emailUsuarioNotificado))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_EMAIL_USUARIO_NOTIFICADO", DBNull.Value))
         End If

         If fechaNotifica <> DateTime.MinValue Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_NOTIFICA", fechaNotifica))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_NOTIFICA", DBNull.Value))
         End If

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
         End While

      Catch ex As Exception
         registroExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, actualizarPasoNotificado", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return registroExitoso

   End Function

   Public Shared Function actualizarPasoNotificado(ByVal idVisitaGenerado As Integer, ByVal idPaso As Integer, ByVal esNotificado As Boolean, ByVal idAreaNotificada As Integer, ByVal usuarioNotificado As String, ByVal emailUsuarioNotificado As String, ByVal fechaNotifica As DateTime, ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
      Dim registroExitoso As Boolean = True

      Dim dataReader As SqlDataReader = Nothing
      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spU_BDS_GRL_ACTUALIZAR_PASO_NOTIFICADO")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", idPaso))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@B_FLAG_NOTIFICADO", esNotificado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA_NOTIFICADA", idAreaNotificada))

         If Not IsNothing(usuarioNotificado) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO_NOTIFICADO", usuarioNotificado))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO_NOTIFICADO", DBNull.Value))
         End If

         If Not IsNothing(emailUsuarioNotificado) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_EMAIL_USUARIO_NOTIFICADO", emailUsuarioNotificado))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_EMAIL_USUARIO_NOTIFICADO", DBNull.Value))
         End If

         If fechaNotifica <> DateTime.MinValue Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_NOTIFICA", fechaNotifica))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_NOTIFICA", DBNull.Value))
         End If

         dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
         End While

      Catch ex As Exception
         registroExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, actualizarPasoNotificado", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return registroExitoso

   End Function

   Public Shared Function getTiposVisita() As List(Of TiposVisita)

      Dim con As Conexion.SQLServer = Nothing
      Dim lstTiposVisita As New List(Of TiposVisita)
      Dim tipoVisita As New TiposVisita()
      tipoVisita.IdTipoVisita = -1
      tipoVisita.DescripcionTipoVisita = "- Seleccionar -"

      lstTiposVisita.Add(tipoVisita)

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_TIPOS_VISITA")

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp)

         While dataReader.Read()

            tipoVisita = New TiposVisita()
            tipoVisita.IdTipoVisita = dataReader.GetInt32(0)
            tipoVisita.DescripcionTipoVisita = dataReader.GetString(1)

            lstTiposVisita.Add(tipoVisita)
         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getTiposVisita", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstTiposVisita

   End Function

   Public Shared Function getAbogadosAsignados(ByVal idVisitaGenerado As Integer,
                                               Optional piIdPerfilAbogado As Integer = Constantes.Todos,
                                               Optional piIdsubPerfilAbogado As Integer = Constantes.Todos) As List(Of Abogado)
      Dim lstAbogadosAsignados As New List(Of Abogado)
      Dim datosUsuario As New Dictionary(Of String, String)

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_ABOGADOS_ASIGNADOS")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@N_ID_PERFIL", piIdPerfilAbogado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@N_ID_SUB_PERFIL", piIdsubPerfilAbogado))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
            Dim objSupAsignado As New Abogado()
            objSupAsignado.Id = dataReader.GetString(0)
            objSupAsignado.Nombre = dataReader.GetString(1)

            If Not IsDBNull(dataReader.Item("T_DSC_MAIL")) Then
               objSupAsignado.Correo = dataReader.Item("T_DSC_MAIL").ToString().Trim()
            Else
               objSupAsignado.Correo = ""
            End If

            If Not dataReader.IsDBNull(3) Then
               objSupAsignado.Perfil = CInt(dataReader.GetDecimal(3))
            Else
               objSupAsignado.Perfil = -1
            End If

            If Not dataReader.IsDBNull(4) Then
               objSupAsignado.SubPerfil = CInt(dataReader.GetDecimal(4))
            Else
               objSupAsignado.SubPerfil = -1
            End If

            ''Si no se llenan los mails de la base de datos obtener de el active directory
#If Not Debug Then
                If objSupAsignado.Correo = "" Then
                    datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(objSupAsignado.Id)
                    If datosUsuario.Count > 0 Then
                        If Not IsNothing(datosUsuario.Item("mail")) Then
                            objSupAsignado.Correo = datosUsuario.Item("mail").ToString()
                        End If
                    End If
                End If
#End If

            lstAbogadosAsignados.Add(objSupAsignado)
         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getAbogadosAsignados", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstAbogadosAsignados

   End Function

   Public Shared Function getSupervisoresAsignados(ByVal idVisitaGenerado As Integer) As List(Of SupervisorAsignado)
      Dim lstSupAsignados As New List(Of SupervisorAsignado)
      Dim datosUsuario As New Dictionary(Of String, String)

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_SUPERVISORES_ASIGNADOS")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
            Dim objSupAsignado As New SupervisorAsignado()
            objSupAsignado.Id = dataReader.GetString(0)
            objSupAsignado.Nombre = dataReader.GetString(1)

            If Not IsDBNull(dataReader.Item("T_DSC_MAIL")) Then
               objSupAsignado.Correo = dataReader.Item("T_DSC_MAIL").ToString().Trim()
            Else
               objSupAsignado.Correo = ""
            End If

            ''Si no se llenan los mails de la base de datos obtener de el active directory
#If Not Debug Then
                If objSupAsignado.Correo = "" Then
                    datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(objSupAsignado.Id)
                    If datosUsuario.Count > 0 Then
                        If Not IsNothing(datosUsuario.Item("mail")) Then
                            objSupAsignado.Correo = datosUsuario.Item("mail").ToString()
                        End If
                    End If
                End If
#End If

            lstSupAsignados.Add(objSupAsignado)
         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getSupervisoresAsignados", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstSupAsignados

   End Function

   Public Shared Function getInspectoresAsignados(ByVal idVisitaGenerado As Integer) As List(Of InspectorAsignado)
      Dim lstInspectoresAsignados As New List(Of InspectorAsignado)
      Dim datosUsuario As New Dictionary(Of String, String)

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_INSPECTORES_ASIGNADOS")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
            Dim inspectorAsignado As New InspectorAsignado()
            inspectorAsignado.Id = dataReader.GetString(0)
            inspectorAsignado.Nombre = dataReader.GetString(1)

            If Not IsDBNull(dataReader.Item("T_DSC_MAIL")) Then
               inspectorAsignado.Correo = dataReader.Item("T_DSC_MAIL").ToString().Trim()
            Else
               inspectorAsignado.Correo = ""
            End If

            ''Si no se llenan los mails de la base de datos obtener de el active directory
#If Not Debug Then
                If inspectorAsignado.Correo = "" Then
                    datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(inspectorAsignado.Id)
                    If datosUsuario.Count > 0 Then
                        If Not IsNothing(datosUsuario.Item("mail")) Then
                            inspectorAsignado.Correo = datosUsuario.Item("mail").ToString()
                        End If
                    End If
                End If
#End If

            lstInspectoresAsignados.Add(inspectorAsignado)
         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getInspectoresAsignados", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstInspectoresAsignados

   End Function

   ''' <summary>
   ''' Obtiene los usuarios asignados configurados para un correo en BD
   ''' </summary>
   ''' <param name="idCorreo"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function getUsuariosAsignadosCorreo(ByVal idCorreo As Integer,
                                                     ByVal idVisita As Integer,
                                                     Optional idArea As Integer = -1,
                                                     Optional idPerfil As Integer = -1) As List(Of Persona)
      Dim lstPersonas As New List(Of Persona)
      Dim datosUsuario As New Dictionary(Of String, String)

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_DBS_GRL_CORREOS_USUARIOS_ENVIO_POR_ID")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@N_ID_CORREO", idCorreo))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@N_ID_PERFIL", idPerfil))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA", idArea))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
            Dim objPersona As New Persona()
            objPersona.Id = dataReader.GetString(0)
            objPersona.Nombre = dataReader.GetString(1)

            If Not IsDBNull(dataReader.Item("T_DSC_MAIL")) Then
               objPersona.Correo = dataReader.Item("T_DSC_MAIL").ToString().Trim()
            Else
               objPersona.Correo = ""
            End If

            ''Si no se llenan los mails de la base de datos obtener de el active directory
#If Not Debug Then
                If objPersona.Correo = "" Then
                    datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(objPersona.Id)
                    If datosUsuario.Count > 0 Then
                        If Not IsNothing(datosUsuario.Item("mail")) Then
                            objPersona.Correo = datosUsuario.Item("mail").ToString()
                        End If
                    End If
                End If
#End If

            lstPersonas.Add(objPersona)
         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento("getUsuariosAsignadosCorreo: " & ex.ToString(), EventLogEntryType.Error, "AccesoBD, getUsuariosAsignadosCorreo", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstPersonas

   End Function


   Public Shared Function getPerfilArea(ByVal idUsuario As String) As List(Of Integer)
      Dim lstPerfilArea As New List(Of Integer)
      Dim con As Conexion.SQLServer = Nothing
      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_AREA_PERFIL_USUARIO")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO", idUsuario))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
            lstPerfilArea.Add(dataReader.GetInt32(0))
            lstPerfilArea.Add(dataReader.GetInt32(1))
         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getPerfilArea", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstPerfilArea

   End Function
   Public Shared Function consultarSubentidad(ByVal idTipoEntidad As String, ByVal idEntidad As Integer) As DataTable
      Dim dt As DataTable = Nothing
      Dim con As Conexion.SQLServer = Nothing
      If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
         con = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNencAux").ToString())
      Else
         con = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNencAux").ToString())
      End If
      Dim query As String = ""
      query = " SELECT ID_T_ENT " & _
              " ,CVE_ID_ENT " & _
              " ,ID_SUBENT " & _
              " ,DSC_SUBENT " & _
              " ,SGL_SUBENT " & _
              " FROM BDV_C_SUBENTIDAD " & _
              " WHERE  " & _
              " VIG_FLAG = 1 " & _
              " AND ID_T_ENT = " & idTipoEntidad & _
              " AND CVE_ID_ENT = " & idEntidad & _
              " ORDER BY ID_SUBENT ASC "
      Try
         dt = con.ConsultarDT(query)
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, consultarVisitaRegistro", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return dt
   End Function

   Public Shared Function cancelarVisita(ByVal idVisitaCancelar As Integer, ByVal motivoCancelacion As String,
                                         ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction, Optional piIdUsuarioCancela As String = "") As Boolean
      Dim registroExitoso As Boolean = True

      Dim dataReader As SqlDataReader = Nothing
      Try
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spU_BDS_GRL_CANCELAR_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaCancelar))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_MOTIVO_CANCELACION", motivoCancelacion))

         If piIdUsuarioCancela <> "" Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO_CANCELA", piIdUsuarioCancela))
         End If

         dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
         End While

      Catch ex As Exception
         registroExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarInspector", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return registroExitoso

   End Function

   Public Shared Sub BloqueCargaDocumentos(ByVal idVisita As Integer, ByVal Edo As Boolean)
      Dim con As Conexion.SQLServer = Nothing
      Dim sql As String = ""

      Try
         sql = String.Format("UPDATE BSIS_X_VISITA_INSPECCION SET I_ID_ESTATUS_ADJUNTAR = {0} WHERE ID_FOLIO_VISITA_SUPERVISAR = {1}", IIf(Edo, 1, 0), idVisita)
         con = New Conexion.SQLServer()

         con.Ejecutar(sql)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarInspector", "")
         Throw ex
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try

   End Sub

   Public Shared Function getUsuariosInvolucrados(ByVal idVisita As Integer) As DataSet

      Dim ds As DataSet = Nothing

      Dim con As Conexion.SQLServer = Nothing
      Try
         con = New Conexion.SQLServer

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_USUARIOS_INVOLUCRADOS_VISITA")

         Dim parametro(0) As SqlParameter
         parametro(0) = New SqlParameter("@I_ID_VISITA", idVisita)

         ds = con.EjecutarSPConsultaDS(sp, parametro)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, consultarVisitas", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try

      Return ds

   End Function

   Public Shared Function registrarProrroga(ByVal prorroga As Prorroga, ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Integer
      Dim idProrrogaGenerado As Integer = 0

      Dim dataReader As SqlDataReader = Nothing
      Try
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_REGISTRAR_PRORROGA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", prorroga.IdVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", prorroga.IdPaso))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_REGISTRO", prorroga.FechaRegistro))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_NUM_DURACION", prorroga.NumDiasDeProrroga))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_MOTIVO", prorroga.MotivoProrroga))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@APRUEBA_PRORROGA", prorroga.ApruebaProrroga))

         If prorroga.FechaFinProrroga <> DateTime.MinValue Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_FIN_PRORROGA", prorroga.FechaFinProrroga))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_FIN_PRORROGA", DBNull.Value))
         End If

         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@SUB_VISITAS", prorroga.SubVisitasSeleccionadas))

         dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
            idProrrogaGenerado = Convert.ToInt32(dataReader("ID"))
         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, registrarVisita", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return idProrrogaGenerado

   End Function

   Public Shared Function getTieneProrrogaPasoActual(ByVal idVisita As Integer, ByVal idPaso As Integer) As DataSet

      Dim ds As DataSet = Nothing

      Dim con As Conexion.SQLServer = Nothing
      Try
         con = New Conexion.SQLServer

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_TIENE_PRORROGA_PASO_ACTUAL")

         Dim parametro(1) As SqlParameter
         parametro(0) = New SqlParameter("@I_ID_VISITA", idVisita)
         parametro(1) = New SqlParameter("@I_ID_PASO", idPaso)

         ds = con.EjecutarSPConsultaDS(sp, parametro)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, consultarVisitas", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try

      Return ds

   End Function

   Public Shared Function finalizarDiasProrrgoa(ByVal idVisitaGenerado As Integer, ByVal idPaso As Integer) As Boolean

      Dim resultadoExitoso As Boolean = False

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spU_BDS_GRL_FINALIZAR_PRORROGA_PASO_ACTUAL")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", idPaso))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
         End While

         resultadoExitoso = True

      Catch ex As Exception
         resultadoExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getDetalleVisita", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return resultadoExitoso

   End Function

   Public Shared Function ObtenerUsuariosPorArea(ByVal idArea As Integer, Optional piIDPerfil As Integer = -1) As List(Of String)

      Dim lstUsuariosPorArea As New List(Of String)

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_USUARIOS_POR_AREA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA", idArea))

         If piIDPerfil <> -1 Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PERFIL", piIDPerfil))
         End If

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
            lstUsuariosPorArea.Add(dataReader.GetString(0))
         End While

      Catch ex As Exception
         lstUsuariosPorArea = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getDetalleVisita", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstUsuariosPorArea

   End Function

   Public Shared Function ObtenerUsuariosPorAreaConTransaccion(ByVal idArea As Integer, ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As List(Of String)


      Dim lstUsuariosPorArea As New List(Of String)

      Dim dataReader As SqlDataReader = Nothing
      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_USUARIOS_POR_AREA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA", idArea))


         dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
            lstUsuariosPorArea.Add(dataReader.GetString(0))
         End While

      Catch ex As Exception
         lstUsuariosPorArea = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getDetalleVisita", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return lstUsuariosPorArea

   End Function

   Public Shared Function asignarAbogadoSancion(ByVal visita As Visita, ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction, psSubVisitas As String) As Boolean

      Dim resultadoExitoso As Boolean = False

      Dim dataReader As SqlDataReader = Nothing
      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spU_BDS_GRL_ASIGNAR_ABOGADO_SANCION")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", visita.IdVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_ABOGADO_SANCION", visita.IdAbogadoSancion))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_NOMBRE_ABOGADO_SANCION", visita.NombreAbogadoSancion))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@SUB_VISITAS", psSubVisitas))

         dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
         End While

         resultadoExitoso = True

      Catch ex As Exception
         resultadoExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getDetalleVisita", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return resultadoExitoso

   End Function

   ''' <summary>
   ''' Asigna un abogado asesor a la visita
   ''' </summary>
   ''' <param name="visita"></param>
   ''' <param name="con"></param>
   ''' <param name="tran"></param>
   ''' <returns></returns>
   ''' <remarks>agc</remarks>
   Public Shared Function asignarAbogadoVisita(ByVal visita As Visita, ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction,
                                            piTipoAbogado As Integer, psSubVisitas As String) As Boolean

      Dim resultadoExitoso As Boolean = False

      Dim dataReader As SqlDataReader = Nothing
      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spU_BDS_GRL_ASIGNAR_ABOGADO_SANCION")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", visita.IdVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_ABOGADO_SANCION", visita.IdAbogadoAsesor))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_DSC_NOMBRE_ABOGADO_SANCION", visita.NombreAbogadoAsesor))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@TIPO_ABOGADO", piTipoAbogado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@SUB_VISITAS", psSubVisitas))

         dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
         End While

         resultadoExitoso = True

      Catch ex As Exception
         resultadoExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getDetalleVisita", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return resultadoExitoso

   End Function

   Public Shared Function finalizarPasoSinTransaccion(ByVal idVisitaGenerado As Integer,
                                                      ByVal idPaso As Integer,
                                                      Optional idAccion As Integer = 0,
                                                      Optional idBanWord As Integer = Constantes.Falso,
                                                      Optional idBanPdf As Integer = Constantes.Falso,
                                                      Optional psSubVisitas As String = "") As Boolean

      Dim resultadoExitoso As Boolean = False

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spU_BDS_GRL_FINALIZAR_PASO")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", idPaso))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_FIN_PASO", DateTime.Now))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@ACCION", idAccion))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@BAN_OFFICE", idBanWord))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@BAN_PDF", idBanPdf))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@SUB_VISITAS", psSubVisitas))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
         End While

         resultadoExitoso = True

      Catch ex As Exception
         resultadoExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getDetalleVisita", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return resultadoExitoso

   End Function

   Public Shared Function finalizarPaso(ByVal idVisitaGenerado As Integer, ByVal idPaso As Integer, ByVal con As Conexion.SQLServer,
                                        ByVal tran As SqlClient.SqlTransaction, Optional pdFechaFinaliza As DateTime? = Nothing, Optional psSubVisitas As String = "") As Boolean

      Dim resultadoExitoso As Boolean = False
      Dim dataReader As SqlDataReader = Nothing
      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spU_BDS_GRL_FINALIZAR_PASO")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", idPaso))

         If IsNothing(pdFechaFinaliza) Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_FIN_PASO", DateTime.Now))
         Else
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_FIN_PASO", pdFechaFinaliza))
         End If

         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@SUB_VISITAS", psSubVisitas))

         dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
         End While

         resultadoExitoso = True

      Catch ex As Exception
         resultadoExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getDetalleVisita", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return resultadoExitoso

   End Function

   Public Shared Function consultarEstatus(ByVal idVisita As Integer, ByVal idPaso As Integer, ByVal idEstatus As Integer) As EstatusPaso

      Dim con As Conexion.SQLServer = Nothing
      Dim estatusPaso As EstatusPaso = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_CONSULTAR_ESTATUS")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", idPaso))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_ESTATUS", idEstatus))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()

            estatusPaso = New EstatusPaso()
            estatusPaso.IdVisitaGenerado = dataReader.GetInt32(0)
            estatusPaso.IdPaso = dataReader.GetInt32(1)
            estatusPaso.IdEstatus = dataReader.GetInt32(2)
            estatusPaso.FechaRegistro = dataReader.GetDateTime(3)
            estatusPaso.IdUsuario = dataReader.GetString(4)
            estatusPaso.Comentarios = dataReader.GetString(5)

         End While

      Catch ex As Exception
         estatusPaso = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getTiposVisita", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return estatusPaso

   End Function


   Public Shared Function finalizarVisitaSinTransaccion(ByVal idVisitaGenerado As Integer, ByVal idPaso As Integer, ByVal fechaFinVisita As DateTime) As Boolean

      Dim resultadoExitoso As Boolean = False

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spU_BDS_GRL_FINALIZAR_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", idPaso))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_FIN_PASO", fechaFinVisita))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
         End While

         resultadoExitoso = True

      Catch ex As Exception
         resultadoExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getDetalleVisita", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return resultadoExitoso

   End Function

   Public Shared Function finalizarVisita(ByVal idVisitaGenerado As Integer, ByVal idPaso As Integer, ByVal fechaFinVisita As DateTime, ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean

      Dim resultadoExitoso As Boolean = False

      Dim dataReader As SqlDataReader = Nothing
      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spU_BDS_GRL_FINALIZAR_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", idPaso))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@F_FECH_FIN_PASO", fechaFinVisita))

         dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
         End While

         resultadoExitoso = True

      Catch ex As Exception
         resultadoExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getDetalleVisita", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return resultadoExitoso

   End Function

   Public Shared Function getUsuariosCodigoAreaRango(ByVal idCodigoAreaInicia As Integer, ByVal idCodigoAreaFin As Integer) As DataSet
      Dim ds As DataSet = Nothing
      Dim con As Conexion.SQLServer = Nothing

      If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
         con = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNencAux").ToString())
      Else
         con = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNencAux").ToString())
      End If

      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_USUARIOS_CODIGO_AREA_RANGO")

      Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
      SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_CODIGO_AREA_INI", idCodigoAreaInicia))
      SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_CODIGO_AREA_FIN", idCodigoAreaFin))


      Try
         ds = con.EjecutarSPConsultaDS(sp, SqlParameters.ToArray)
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, consultarVisitaRegistro", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return ds
   End Function

   ''' <summary>
   ''' Devuelve las entidades en sicod sin considerar el tipo de entidad
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function getEntidadesSinTipo() As DataSet
      Dim ds As DataSet = Nothing
      Dim con As Conexion.SQLServer = Nothing

      If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
         con = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNencAux").ToString())
      Else
         con = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNencAux").ToString())
      End If

      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_USUARIOS_CODIGO_AREA_RANGO")


      Try
         ds = con.EjecutarSPConsultaDS(sp)
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, consultarVisitaRegistro", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return ds
   End Function

   Public Shared Function getDiasFeriados(Optional pdFecha As Date? = Nothing) As List(Of DateTime)

      Dim lstDiasFeriados As New List(Of DateTime)

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DIAS_FERIADOS")
         Dim dataReader As SqlDataReader

         If Not IsNothing(pdFecha) Then
            Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@FECHA", pdFecha))

            dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)
         Else
            dataReader = con.EjecutarSPConsultaDR(sp)
         End If

         While dataReader.Read()
            lstDiasFeriados.Add(dataReader.GetDateTime(0))
         End While

      Catch ex As Exception
         lstDiasFeriados = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getTiposVisita", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstDiasFeriados

   End Function

   Public Shared Function getEncabezadoReporteVisita(ByVal idVisitaGenerado As Integer) As EncabezadoReporteVisita

      Dim datosVisita As EncabezadoReporteVisita = Nothing

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_ENCABEZADO_REPORTE_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()

            datosVisita = New EncabezadoReporteVisita()

            datosVisita.IdVisitaGenerado = dataReader.GetInt32(0)
            datosVisita.FolioVisita = dataReader.GetString(1)
            datosVisita.FechaRegistro = dataReader.GetDateTime(2)
            datosVisita.IdEntidad = dataReader.GetInt32(3)

            If dataReader.IsDBNull(4) Then
               datosVisita.DscEntidad = String.Empty
            Else
               datosVisita.DscEntidad = dataReader.GetString(4)
            End If

            datosVisita.IdArea = dataReader.GetInt32(5)

            If dataReader.IsDBNull(6) Then
               datosVisita.DscArea = String.Empty
            Else
               datosVisita.DscArea = dataReader.GetString(6)
            End If

            datosVisita.IdPasoActual = dataReader.GetInt32(7)

            If dataReader.IsDBNull(8) Then
               datosVisita.DscPasoActual = String.Empty
            Else
               datosVisita.DscPasoActual = dataReader.GetString(8)
            End If

            datosVisita.IdEstatusActual = dataReader.GetInt32(9)

            If dataReader.IsDBNull(10) Then
               datosVisita.DscEstatusActual = String.Empty
            Else
               datosVisita.DscEstatusActual = dataReader.GetString(10)
            End If

         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getEncabezadoReporteVisita", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return datosVisita

   End Function

   Public Shared Function getCatalogoPasos(Optional piPaso As Integer = Constantes.Todos,
                                           Optional ByVal piIdVista As Integer = Constantes.Todos) As List(Of Paso)

      Dim lstPasos As List(Of Paso) = Nothing

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         'Validar la fecha de registro de la visita
         Dim objVisita As New Visita()
         objVisita = AccesoBD.getDetalleVisita(piIdVista, 35)
         Dim sp As String = Nothing

         Dim fechaRegVisita As Date = CDate(objVisita.FechaRegistro.ToString())
         Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         If (Convert.ToDateTime(fechaRegVisita).ToString("yyyy/MM/dd") > Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
            sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_CATALOGO_PASOS_V17")
         Else
            sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_CATALOGO_PASOS")
         End If

         Dim lstParam(1) As SqlParameter
         lstParam(0) = New SqlParameter("@I_ID_PASO", piPaso)
         lstParam(1) = New SqlParameter("@I_ID_VISITA", piIdVista)

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, lstParam)

         lstPasos = New List(Of Paso)

         While dataReader.Read()

            Dim paso As New Paso()

            paso.IdPaso = dataReader.GetInt32(0)

            If dataReader.IsDBNull(1) Then
               paso.DscPaso = String.Empty
            Else
               paso.DscPaso = dataReader.GetString(1)
            End If

            paso.NumDiasMin = dataReader.GetInt32(2)
            paso.NumDiasMax = dataReader.GetInt32(3)

            paso.EnProrroga = dataReader.GetInt32(5)

            lstPasos.Add(paso)

         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getEncabezadoReporteVisita", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstPasos

   End Function

   Public Shared Function getTodosPasosVisita(ByVal idVisita As Integer) As List(Of PasoProcesoVisita)

      Dim lstPasos As List(Of PasoProcesoVisita) = Nothing

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp As String = Nothing

         'Validar la fecha de registro de la visita
         Dim objVisita As New Visita()
         objVisita = AccesoBD.getDetalleVisita(idVisita, 35)

         Dim fechaRegVisita As Date = CDate(objVisita.FechaRegistro.ToString())
         Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

         If (Convert.ToDateTime(fechaRegVisita).ToString("yyyy/MM/dd") > Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
            sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_TODOS_PASOS_VISITA_V17")
         Else
            sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_TODOS_PASOS_VISITA")
         End If


         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisita))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         lstPasos = New List(Of PasoProcesoVisita)

         While dataReader.Read()

            Dim paso As New PasoProcesoVisita()

            paso.IdVisitaGenerado = dataReader.GetInt32(0)
            paso.IdPaso = dataReader.GetInt32(1)
            paso.FechaInicio = dataReader.GetDateTime(2)

            If dataReader.IsDBNull(3) Then
               paso.FechaFin = DateTime.MinValue
            Else
               paso.FechaFin = dataReader.GetDateTime(3)
            End If

            If dataReader.IsDBNull(4) Then
               paso.EsNotificado = False
            Else
               paso.EsNotificado = dataReader.GetBoolean(4)
            End If

            paso.IdAreaNotificada = dataReader.GetInt32(5)

            If dataReader.IsDBNull(6) Then
               paso.IdUsuarioNotificado = String.Empty
            Else
               paso.IdUsuarioNotificado = dataReader.GetString(6)
            End If

            If dataReader.IsDBNull(7) Then
               paso.EmailUsuarioNotificado = String.Empty
            Else
               paso.EmailUsuarioNotificado = dataReader.GetString(7)
            End If

            If dataReader.IsDBNull(8) Then
               paso.TieneProrroga = False
            Else
               paso.TieneProrroga = dataReader.GetBoolean(8)
            End If

            If dataReader.IsDBNull(9) Then
               paso.FechaNotifica = DateTime.MinValue
            Else
               paso.FechaNotifica = dataReader.GetDateTime(9)
            End If

            If dataReader.IsDBNull(10) Then
               paso.DiasTranscurridos = 0
            Else
               paso.DiasTranscurridos = dataReader.GetInt32(10)
            End If

            If dataReader.IsDBNull(11) Then
               paso.DiasEstimadosPaso = 0
            Else
               paso.DiasEstimadosPaso = dataReader.GetInt32(11)
            End If

            If dataReader.IsDBNull(13) Then
               paso.IdPasoCancelo = -1
            Else
               paso.IdPasoCancelo = dataReader.GetInt32(13)
            End If

            lstPasos.Add(paso)

         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getTodosPasosVisita", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstPasos

   End Function



   Public Shared Function getDocumentosPasosVisita(ByVal idVisita As Integer, ByVal idPaso As Integer) As List(Of DatosDocumento)

      Dim lstDocumentosPasoVisita As List(Of DatosDocumento) = Nothing

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DOCUMENTOS_PASOS_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", idPaso))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         lstDocumentosPasoVisita = New List(Of DatosDocumento)

         While dataReader.Read()

            Dim documento As New DatosDocumento()

            documento.IdVisitaGenerado = dataReader.GetInt32(0)
            documento.IdPaso = dataReader.GetInt32(1)

            If dataReader.IsDBNull(2) Then
               documento.NumDocumento = 0
            Else
               documento.NumDocumento = dataReader.GetInt32(2)
            End If

            documento.DscNombreArchivo = dataReader.GetString(3)
            documento.FechaRegistro = dataReader.GetDateTime(4)

            If dataReader.IsDBNull(5) Then
               documento.DscComentarios = String.Empty
            Else
               documento.DscComentarios = dataReader.GetString(5)
            End If

            If dataReader.IsDBNull(6) Then
               documento.DscNombreArchivoOri = String.Empty
            Else
               documento.DscNombreArchivoOri = dataReader.GetString(6)
            End If

            If dataReader.IsDBNull(7) Then
               documento.UsuarioCarga = String.Empty
            Else
               documento.UsuarioCarga = dataReader.GetString(7)
            End If

            lstDocumentosPasoVisita.Add(documento)

         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getEncabezadoReporteVisita", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstDocumentosPasoVisita

   End Function


   Public Shared Function getComentariosPasosVisita(ByVal idVisita As Integer, ByVal idPaso As Integer) As List(Of DatosDocumento)

      Dim lstDocumentosPasoVisita As List(Of DatosDocumento) = Nothing

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_COMENTARIOS_PASOS_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", idPaso))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         lstDocumentosPasoVisita = New List(Of DatosDocumento)

         While dataReader.Read()

            Dim documento As New DatosDocumento()

            documento.IdVisitaGenerado = dataReader.GetInt32(0)
            documento.IdPaso = dataReader.GetInt32(1)

            If dataReader.IsDBNull(2) Then
               documento.NumDocumento = 0
            Else
               documento.NumDocumento = dataReader.GetInt32(2)
            End If

            documento.DscNombreArchivo = dataReader.GetString(3)
            documento.FechaRegistro = dataReader.GetDateTime(4)

            If dataReader.IsDBNull(5) Then
               documento.DscComentarios = String.Empty
            Else
               documento.DscComentarios = dataReader.GetString(5)
            End If

            lstDocumentosPasoVisita.Add(documento)

         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getEncabezadoReporteVisita", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstDocumentosPasoVisita

   End Function

   Public Shared Function getUsuarioComentariosPasosVisita(ByVal idVisita As Integer, ByVal idPaso As Integer) As List(Of UsuarioComentario)

      Dim lstUsuarioComentarios As List(Of UsuarioComentario) = Nothing

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_USUARIO_COMENTARIOS_PV")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", idPaso))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         lstUsuarioComentarios = New List(Of UsuarioComentario)
         Dim objDtsDocs As DatosDocumento

         While dataReader.Read()
            If Not dataReader.IsDBNull(0) Then
               Dim objUsuComentarios As New UsuarioComentario()
               objUsuComentarios.ListaComentarios = New List(Of Comentario)

               objUsuComentarios.IdUsuario = dataReader.GetString(0)
               objUsuComentarios.NombreCompleto = dataReader.GetString(1)
               objUsuComentarios.ContenidoCom = dataReader.GetString(2)
               objUsuComentarios.FechaRegistrCom = dataReader.GetString(3)
               objUsuComentarios.ListaDocumentos = New List(Of DatosDocumento)

               If dataReader.GetString(4).ToString().Length > 0 Then
                  Dim lvVecRes() As String = dataReader.GetString(4).ToString().Split(";")
                  Dim lvVecDocs() As String

                  If lvVecRes.Length > 0 Then
                     For i As Integer = 0 To lvVecRes.Length - 1
                        lvVecDocs = lvVecRes(i).Split(",")
                        If lvVecDocs.Length = 2 Then
                           objDtsDocs = New DatosDocumento
                           objDtsDocs.DscNombreArchivo = lvVecDocs(0)
                           objDtsDocs.DscNombreArchivoOri = lvVecDocs(1)

                           objUsuComentarios.ListaDocumentos.Add(objDtsDocs)
                        End If
                     Next
                  End If
               End If

               lstUsuarioComentarios.Add(objUsuComentarios)
            End If
         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getEncabezadoReporteVisita", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstUsuarioComentarios

   End Function
   'Public Shared Function getUsuarioComentariosPasosVisita(ByVal idVisita As Integer, ByVal idPaso As Integer) As List(Of UsuarioComentario)

   '    Dim lstUsuarioComentarios As List(Of UsuarioComentario) = Nothing

   '    Dim con As Conexion.SQLServer = Nothing

   '    Try

   '        con = New Conexion.SQLServer
   '        Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
   '        Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_USUARIO_COMENTARIOS_PV")

   '        Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
   '        SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisita))
   '        SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO", idPaso))

   '        Dim dataReader As SqlDataReader
   '        dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

   '        lstUsuarioComentarios = New List(Of UsuarioComentario)

   '        While dataReader.Read()
   '            Dim lsCadenaRes As String

   '            If Not dataReader.IsDBNull(0) Then
   '                Dim objUsuComentarios As New UsuarioComentario()

   '                lsCadenaRes = dataReader.GetString(0)

   '                Dim lvVecRes() As String = lsCadenaRes.Split("\")
   '                Dim lvVecCom() As String
   '                Dim lvVecDocs() As String
   '                Dim lvVecComFecha() As String
   '                Dim lvVecNamesDocs() As String
   '                Dim objCom As Comentario
   '                Dim objDtsDocs As DatosDocumento

   '                If lvVecRes.Length = 4 Then
   '                    objUsuComentarios.IdUsuario = lvVecRes(0)
   '                    objUsuComentarios.NombreCompleto = lvVecRes(2)
   '                    lvVecCom = lvVecRes(1).Split("|")
   '                    lvVecDocs = lvVecRes(3).Split(";")

   '                    If lvVecCom.Length > 0 Then
   '                        objUsuComentarios.ListaComentarios = New List(Of Comentario)
   '                        For i As Integer = 0 To lvVecCom.Length - 1
   '                            objCom = New Comentario
   '                            If lvVecCom(i).Trim().Length > 0 Then
   '                                lvVecComFecha = lvVecCom(i).Split("#")
   '                                objCom.Contenido = lvVecComFecha(0)
   '                                objCom.FechaRegistro = lvVecComFecha(1)
   '                                objUsuComentarios.ListaComentarios.Add(objCom)
   '                            End If
   '                        Next
   '                    End If

   '                    If lvVecDocs.Length > 0 Then
   '                        objUsuComentarios.ListaDocumentos = New List(Of DatosDocumento)
   '                        For i As Integer = 0 To lvVecDocs.Length - 1
   '                            objDtsDocs = New DatosDocumento
   '                            If lvVecDocs(i).Trim().Length > 0 Then
   '                                lvVecNamesDocs = lvVecDocs(i).Split(",")
   '                                objDtsDocs.DscNombreArchivo = lvVecNamesDocs(0)
   '                                objDtsDocs.DscNombreArchivoOri = lvVecNamesDocs(1)
   '                                objUsuComentarios.ListaDocumentos.Add(objDtsDocs)
   '                            End If
   '                        Next
   '                    End If

   '                    lstUsuarioComentarios.Add(objUsuComentarios)
   '                End If
   '            End If
   '        End While

   '    Catch ex As Exception
   '        Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getEncabezadoReporteVisita", "")
   '    Finally
   '        If Not IsNothing(con) Then
   '            con.CerrarConexion()
   '        End If
   '    End Try

   '    Return lstUsuarioComentarios

   'End Function


   Public Shared Function calcularDiasTranscurridos(ByVal fecha_ini As DateTime, ByVal fecha_fin As DateTime) As Integer
      Dim numDias As Integer = 0

      Dim lstDiasFeriados As New List(Of DateTime)
      lstDiasFeriados = AccesoBD.getDiasFeriados()

      'Este proceso se hace para tener solo las fechas, sin las horas, minutos y segundos, 
      'ya que por estos datos puede ser mayor o menor una fecha y las comparaciones entre fechas serían incorrectas
      Dim lstDiasFeriadosSoloFechas As New List(Of DateTime)
      For Each df As DateTime In lstDiasFeriados
         Dim value As DateTime = New DateTime(df.Year, df.Month, df.Day)
         lstDiasFeriadosSoloFechas.Add(value)
      Next
      Dim fechaInicio As DateTime = New DateTime(fecha_ini.Year, fecha_ini.Month, fecha_ini.Day)
      Dim fechaFin As DateTime = New DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day)


      Dim res1 As Integer = Date.Compare(fechaInicio, fechaFin)
      '0  - es la misma fecha
      '-1 - es mas pequeña la fechaInicio que la fechaFin
      '1  - es mas grande la fechaInicio que la fechaFin
      If res1 = -1 Then

         Dim fechaTemp As DateTime
         fechaTemp = fechaInicio

         While res1 = -1

            fechaTemp = fechaTemp.AddDays(1)

            If fechaTemp.DayOfWeek = DayOfWeek.Saturday Or fechaTemp.DayOfWeek = DayOfWeek.Sunday Then
               'Es fin de semana, no se contabiliza
            Else



               Dim esFeriado As Boolean = False

               If Not IsNothing(lstDiasFeriadosSoloFechas) Then

                  Dim res2 As Integer

                  For Each diaFeriado As DateTime In lstDiasFeriadosSoloFechas

                     res2 = Date.Compare(fechaTemp, diaFeriado)

                     If res2 = 0 Then
                        esFeriado = True
                        Exit For
                     End If

                  Next

               End If

               If esFeriado = True Then
                  'Es feriado, no se contabiliza
               Else
                  numDias = numDias + 1

               End If

            End If

            res1 = Date.Compare(fechaTemp, fechaFin)

         End While

      End If

      Return numDias

   End Function

   ''' <summary>
   ''' Obtiene el catalgo de objeto de la visita
   ''' </summary>
   ''' <returns>Retorna un datatable</returns>
   ''' <remarks>agc</remarks>
   Function ObtenObjetoVisita(piIdArea As Integer) As DataTable
      Dim con As Conexion.SQLServer = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim strQuery As String = "SELECT N_ID_OBJETO_VISITA ID, T_DSC_OBJETO_VISITA DSC FROM " & Owner & "BDS_C_GR_OBJETO_VISITA WHERE N_FLAG_VIG = 1 AND (I_ID_AREA = " & piIdArea.ToString() & " OR N_ID_OBJETO_VISITA = 1)"

      strQuery += " ORDER BY N_NUM_ORDEN"

      Dim dt As DataTable = New DataTable()
      Try
         dt = con.ConsultarDT(strQuery)
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenObjetoVisita", "")
      Finally
         If Not con Is Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return dt
   End Function

   Function ObtenObjetoVisitaOri() As DataTable
      Dim con As Conexion.SQLServer = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim strQuery As String = "SELECT N_ID_OBJETO_VISITA, T_DSC_OBJETO_VISITA DscObjetoVisita FROM " & Owner & "BDS_C_GR_OBJETO_VISITA WHERE N_FLAG_VIG = 1 "

      strQuery += " ORDER BY N_NUM_ORDEN"

      Dim dt As DataTable = New DataTable()
      Try
         dt = con.ConsultarDT(strQuery)
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenObjetoVisita", "")
      Finally
         If Not con Is Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return dt
   End Function
   ''' <summary>
   ''' Funcion que guarda la reactivacion del paso
   ''' </summary>
   ''' <param name="piPasoActual"></param>
   ''' <param name="piPasoReactivado"></param>
   ''' <param name="piIdVisita"></param>
   ''' <param name="piEstatusReactivacion"></param>
   ''' <param name="con"></param>
   ''' <param name="tran"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function SaveUpdateReactivacionPaso(ByVal piIdVisita As Integer,
                                                  ByVal piPasoActual As Integer,
                                                  ByVal piPasoReactivado As Integer,
                                                  ByVal piEstatusReactivacion As Integer,
                                                  ByVal con As Conexion.SQLServer,
                                                  ByVal tran As SqlClient.SqlTransaction,
                                                  ByVal psSubVisitas As String,
                                                  ByVal psIdUsuario As String,
                                                  ByVal piIdArea As Integer) As Boolean
      Dim registroExitoso As Boolean = True

      Dim dataReader As SqlDataReader = Nothing
      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_GUARDAR_REACTIVACION")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", piIdVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO_ACTUAL", piPasoActual))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PASO_REACTIVAR", piPasoReactivado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_ESTATUS", piEstatusReactivacion))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@SUB_VISITAS", psSubVisitas))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@T_ID_USUARIO", psIdUsuario))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@ID_AREA_ACTUAL", piIdArea))

         dataReader = con.EjecutarSPConsultaDRConTransaccion(sp, SqlParameters.ToArray, tran)

         While dataReader.Read()
         End While

      Catch ex As Exception
         registroExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, GuardarActReactivacionPaso", "")
      Finally
         If Not IsNothing(dataReader) Then
            dataReader.Close()
            dataReader = Nothing
         End If
      End Try

      Return registroExitoso

   End Function

   ''' <summary>
   ''' Actualiza la fecha de vulnerabilidad
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ActualizaFechaVulnerabilidad(piIdVisita As Integer, pdFecvul As DateTime) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try
         Dim sqlParameter(1) As SqlParameter
         sqlParameter(0) = New SqlParameter("@ID_VISITA_SISVIG", piIdVisita)
         sqlParameter(1) = New SqlParameter("@F_FECHA_VULNERA", IIf(Fechas.Vacia(pdFecvul), DBNull.Value, pdFecvul))

         lbRes = con.EjecutarSP("spU_BDS_GRL_ACTUALIZA_FECHA_VULNERABILIDAD", sqlParameter)
      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function

   ''' <summary>
   ''' Actualiza la fecha de inicio de la visita
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ActualizaFechaInicioVisita(piIdVisita As Integer, pdFecIni As DateTime,
                                                     tipoFecha As Integer, psSubVisitas As String,
                                                     Optional psUsuario As String = "",
                                                     Optional pdFechaFinP8 As DateTime = Nothing) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False

      Try
            If Convert.ToDateTime(pdFechaFinP8).ToString() = "01/01/0001 12:00:00 a.m." Then
                pdFechaFinP8 = DateTime.Now
            End If


            Dim sqlParameter(5) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@F_FECH_VISITA", pdFecIni)
         sqlParameter(2) = New SqlParameter("@TIPO_FECHA", tipoFecha)
         sqlParameter(3) = New SqlParameter("@SUB_VISITAS", psSubVisitas)
         sqlParameter(4) = New SqlParameter("@I_ID_USUARIO", psUsuario)
         sqlParameter(5) = New SqlParameter("@F_FECH_FIN_VISITA_P8", pdFechaFinP8)

         lbRes = con.EjecutarSP("spU_BDS_GRL_ACTUALIZA_FECHA_INICIO_VISITA", sqlParameter)

      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function



   Public Shared Function ReseteaRangosSancion(piIdVisita As Integer) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False

      Try

         Dim sqlParameter(0) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)

         lbRes = con.EjecutarSP("spS_BDS_GRL_LIMPIA_RANGOS_SANCION", sqlParameter)

      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function

   Public Shared Function ActualizaEstatusCierreVisita(piIdVisita As Integer) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False

      Try

         Dim sqlParameter(0) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)

         lbRes = con.EjecutarSP("spU_BDS_GRL_ACTUALIZA_ESTATUS_CIERRE_VISITA_PADRE", sqlParameter)

      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function

   ''' <summary>
   ''' Actualiza la fecha de inicio de la visita en tabla de BDS_D_VS_FECHAS_ESTIMADAS
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ActualizaFechaInicioVisita_TblFechasEstimadas(piIdVisita As Integer,
                                                                        piIdPaso As Integer,
                                                                        Optional pdFecReal As DateTime = Nothing,
                                                                        Optional piOpc As Integer = -1) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try
         Dim sqlParameter(3) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@F_FECH_VISITA_REAL", pdFecReal)
         sqlParameter(2) = New SqlParameter("@OPC_PASO", piOpc)
         sqlParameter(3) = New SqlParameter("@I_ID_PASO", piIdPaso)

         lbRes = con.EjecutarSP("spU_BDS_GRL_ACTUALIZA_FECHA_ESTIMADA_INICIO_VISITA_V17", sqlParameter)
      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function

   ''' <summary>
   ''' Inserta la fecha de inicio de la visita en tabla de BDS_D_VS_FECHAS_ESTIMADAS
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function InsertaFechaInicioVisita_TblFechasEstimadas(piIdVisita As Integer,
                                                                        piIdPaso As Integer,
                                                                        piIdArea As Integer,
                                                                        pIdSeRealizo As Integer,
                                                                        Optional pdFecEst As DateTime = Nothing) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try
         Dim sqlParameter(4) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@I_ID_PASO", piIdPaso)
         sqlParameter(2) = New SqlParameter("@I_ID_AREA", piIdArea)
         sqlParameter(3) = New SqlParameter("@F_FECH_VISITA", pdFecEst)
         sqlParameter(4) = New SqlParameter("@I_FLAG_SE_LLEVO_A_CABO", pIdSeRealizo)

         lbRes = con.EjecutarSP("spU_BDS_GRL_INSERTA_FECHA_ESTIMADA_INICIO_VISITA_V17", sqlParameter)
      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function

   Public Shared Function ActualizaEstatusVulneraConTransaccion(piIdVisita As Integer,
                                                   ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
      Return ActualizaEstatusVulneraConTransaccion(piIdVisita, 0, con, tran)
   End Function

   Public Shared Function ActualizaEstatusVulneraConTransaccion(piIdVisita As Integer, ByVal Valor As Integer, _
                              ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean

      Dim lbRes As Boolean = False
      Try

         Dim sqlParameter(1) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@Valor", Valor)

         lbRes = con.EjecutarSP("spS_BDS_GRL_UP_BANDERA_VULNERABILIDAD", sqlParameter, tran)
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      End Try

      Return lbRes

   End Function

   Public Shared Function ActualizaFechaInicioVisitaConTransaccion(piIdVisita As Integer, pdFecIni As Date,
                                                     tipoFecha As Integer,
                                                     ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction, psSubVisitas As String) As Boolean
      Dim lbRes As Boolean = False
      Try

         Dim sqlParameter(3) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         If pdFecIni.Equals(Date.MinValue) Then
            sqlParameter(1) = New SqlParameter("@F_FECH_VISITA", "")
         Else
            sqlParameter(1) = New SqlParameter("@F_FECH_VISITA", pdFecIni)
         End If

         sqlParameter(2) = New SqlParameter("@TIPO_FECHA", tipoFecha)
         sqlParameter(3) = New SqlParameter("@SUB_VISITAS", psSubVisitas)

         lbRes = con.EjecutarSP("spU_BDS_GRL_ACTUALIZA_FECHA_INICIO_VISITA", sqlParameter, tran)
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      End Try

      Return lbRes
   End Function

   Public Shared Function ActualizaFechaVulnerabilidadConTransaccion(piIdVisita As Integer, pdFecIni As Date,
                                                     ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
      'Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try

         Dim sqlParameter(1) As SqlParameter
         sqlParameter(0) = New SqlParameter("@ID_VISITA_SISVIG", piIdVisita)
         sqlParameter(1) = New SqlParameter("@F_FECHA_VULNERA", pdFecIni)

         lbRes = con.EjecutarSP("spU_BDS_GRL_ACTUALIZA_FECHA_VULNERABILIDAD", sqlParameter, tran)
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento("ActualizaFechaVulnerabilidadConTransaccion [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      End Try

      Return lbRes
   End Function

   ''' <summary>
   ''' Consulta las sub entidades sin importar el tipo de entidad
   ''' </summary>
   ''' <param name="idEntidad"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function consultarSubentidadSinTipo(ByVal idEntidad As Integer) As DataTable
      Dim dt As DataTable = Nothing
      Dim con As Conexion.SQLServer = Nothing
      If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
         con = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNencAux").ToString())
      Else
         con = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNencAux").ToString())
      End If

      Dim sqlParameter(0) As SqlParameter
      Try
         sqlParameter(0) = New SqlParameter("@idEntidad", idEntidad)

         dt = con.EjecutarSPConsultaDT("spS_BDS_GRL_GET_SUBENTIDADES_SEPRIS", sqlParameter)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Conexion base de datos sicod, consultarSubentidadSinTipo", "")
         dt = Nothing
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try
      Return dt
   End Function


   ''' <summary>
   ''' AGC consulta las visitas en un DT
   ''' </summary>
   ''' <param name="idArea"></param>
   ''' <param name="idTipoConsulta"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function consultarVisitasDT(Optional idArea As Integer = -1,
                                             Optional idTipoConsulta As Integer = -1,
                                             Optional psUsuarioActual As String = "",
                                             Optional psPerfilUsuActual As Integer = 0) As DataTable
      Dim dt As DataTable = Nothing
      Dim con As Conexion.SQLServer = Nothing

      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_BANDEJA_VISITA")


         Dim sqlParameter(3) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_AREA", idArea)
         sqlParameter(1) = New SqlParameter("@I_ID_TIPO_CONSULTA", idTipoConsulta)
         sqlParameter(2) = New SqlParameter("@I_ID_USUARIO", psUsuarioActual)
         sqlParameter(3) = New SqlParameter("@I_ID_PERFIL", psPerfilUsuActual)

         dt = con.EjecutarSPConsultaDT(sp, sqlParameter)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getEncabezadoReporteVisita", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return dt
   End Function

   ''' <summary>
   ''' Registra la copia de una visita
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function registrarCopiaVisita(ByVal idVisitaPadre As Integer, ByVal piArea As Integer,
                                               Optional ByRef liIdVisita As Integer = 0,
                                               Optional piTipoCopia As Integer = Constantes.TipoCopia.CopiaFolio,
                                               Optional psFolioVisita As String = "",
                                               Optional piIdSubentidad As Integer = 0,
                                               Optional psDscSubEntidad As String = "",
                                               Optional psIdTipoEntidad As Integer = 0,
                                               Optional psDscTipoEntidad As String = "") As String
      Dim dt As DataTable = Nothing
      Dim con As Conexion.SQLServer = Nothing
      Dim lsRes As String = ""
      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_COPIA_VISITA")


         Dim sqlParameter(8) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA_PADRE", idVisitaPadre)
         sqlParameter(1) = New SqlParameter("@I_ID_AREA", piArea)
         sqlParameter(2) = New SqlParameter("@MES_ANIO", Today.ToString("MMyy"))
         sqlParameter(3) = New SqlParameter("@TIPO_COPIA", piTipoCopia)
         sqlParameter(4) = New SqlParameter("@FOLIO_VISITA", psFolioVisita)
         sqlParameter(5) = New SqlParameter("@ID_SUBENTIDAD", piIdSubentidad)
         sqlParameter(6) = New SqlParameter("@DSC_SUB_ENTIDAD", psDscSubEntidad)
         sqlParameter(7) = New SqlParameter("@ID_TIPO_ENTIDAD", psIdTipoEntidad)
         sqlParameter(8) = New SqlParameter("@DSC_TIPO_ENTIDAD", psDscTipoEntidad)

         dt = con.EjecutarSPConsultaDT(sp, sqlParameter)

         For Each row As DataRow In dt.Rows
            lsRes = IIf(IsNothing(row("DSC_ID_NUEVA_VISITA")), "", row("DSC_ID_NUEVA_VISITA").ToString())
            Int32.TryParse(IIf(IsNothing(row("I_ID_NUEVA_VISITA")), "0", row("I_ID_NUEVA_VISITA").ToString()), liIdVisita)
         Next

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, spI_BDS_GRL_COPIA_VISITA", "")
         lsRes = ""
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lsRes
   End Function

   ''' <summary>
   ''' Consulta todos los documentos de una visita
   ''' </summary>
   ''' <param name="idVisita"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function llenarExpedienteDocumentos(idVisita As Integer,
                                              Optional idPaso As Integer = -1,
                                              Optional idDocumento As Integer = -1,
                                              Optional idVigencia As Integer = Constantes.Vigencia.Vigente) As List(Of Documento)
      Dim dr As SqlDataReader = Nothing
      Dim lstDocs As New List(Of Documento)
      Dim con As Conexion.SQLServer = Nothing

      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DOCUMENTOS_VISITA")


         Dim sqlParameter(3) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", idVisita)
         sqlParameter(1) = New SqlParameter("@I_ID_PASO", idPaso)
         sqlParameter(2) = New SqlParameter("@N_ID_DOCUMENTO", idDocumento)
         sqlParameter(3) = New SqlParameter("@ID_VIGENCIA", idVigencia)

         dr = con.EjecutarSPConsultaDR(sp, sqlParameter)
         lstDocs = CType(dr.ToList(Of Documento)(), Global.System.Collections.Generic.List(Of Documento))
      Catch ex As Exception
         dr = Nothing
         lstDocs = New List(Of Documento)
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, consultarDocumentos", "")
      Finally
         If dr IsNot Nothing Then
            If Not dr.IsClosed Then
               dr.Close() : dr = Nothing
            End If
         End If
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstDocs
   End Function

   ''' <summary>
   ''' Obtner el datatable con los documentos
   ''' </summary>
   ''' <param name="idVisita"></param>
   ''' <param name="idPaso"></param>
   ''' <param name="idDocumento"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function consultarDocumentos(Optional idVisita As Integer = Constantes.Todos,
                                              Optional idPaso As Integer = Constantes.Todos,
                                              Optional idDocumento As Integer = Constantes.Todos) As DataTable
      Dim dt As DataTable
      Dim con As Conexion.SQLServer = Nothing

      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DOCUMENTOS_VISITA")


         Dim sqlParameter(2) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", idVisita)
         sqlParameter(1) = New SqlParameter("@I_ID_PASO", idPaso)
         sqlParameter(2) = New SqlParameter("@N_ID_DOCUMENTO", idDocumento)

         dt = con.EjecutarSPConsultaDT(sp, sqlParameter)
      Catch ex As Exception
         dt = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, consultarDocumentos", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return dt
   End Function

   ''' <summary>
   ''' Obtiene el numero maximo de versiones para una visita al momento
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ObtenerNumeroMaximoVersiones(Optional idVisita As Integer = Constantes.Todos,
                                       Optional idPaso As Integer = Constantes.Todos,
                                       Optional idDocumento As Integer = Constantes.Todos,
                                       Optional idVigencia As Integer = Constantes.Vigencia.Vigente) As Integer
      Dim con As Conexion.SQLServer = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim lsRes As Integer = 0

      Dim dt As DataTable = New DataTable()
      Try
         con = New Conexion.SQLServer
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_MAXIMO_DOCUMETO_VISITA")


         Dim sqlParameter(3) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", idVisita)
         sqlParameter(1) = New SqlParameter("@I_ID_PASO", idPaso)
         sqlParameter(2) = New SqlParameter("@N_ID_DOCUMENTO", idDocumento)
         sqlParameter(3) = New SqlParameter("@ID_VIGENCIA", idVigencia)

         dt = con.EjecutarSPConsultaDT(sp, sqlParameter)

         For Each row As DataRow In dt.Rows
            If Not (Int32.TryParse(IIf(IsDBNull(row("NUM_MAX")), 0, row("NUM_MAX").ToString()), lsRes)) Then
               lsRes = 0
            End If
         Next

      Catch ex As Exception
         lsRes = 0
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenObjetoVisita", "")
      Finally
         If Not con Is Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lsRes
   End Function

   Public Shared Function ExisteVisitaConFechaEstimada(ByVal idVisita As Integer, ByVal IdPasoActual As Integer, ByVal IdArea As Integer) As Boolean
      Dim con As Conexion.SQLServer = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim lsRes As Boolean = False

      Dim dt As DataTable = New DataTable()
      Try
         con = New Conexion.SQLServer
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_VISITA_CON_FECHA_ESTIMADA_V17")


         Dim sqlParameter(2) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", idVisita)
         sqlParameter(1) = New SqlParameter("@I_ID_PASO", IdPasoActual)
         sqlParameter(2) = New SqlParameter("@I_ID_AREA", IdArea)

         dt = con.EjecutarSPConsultaDT(sp, sqlParameter)

         For Each row As DataRow In dt.Rows
            If (row("CONTEO") > 0) Then
               lsRes = True
            End If
         Next

      Catch ex As Exception
         lsRes = 0
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ExisteVisitaConFechaEstimada", "")
      Finally
         If Not con Is Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lsRes
   End Function

   Public Shared Function ObtenerAsuntoConvocatoria(ByVal idAsunto As Integer) As String
      Dim con As Conexion.SQLServer = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim lsAsunto As String = Nothing

      Dim dt As DataTable = New DataTable()
      Try
         con = New Conexion.SQLServer
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_ASUNTO_CONVOCATORIA_V17")

         Dim sqlParameter(0) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_ASUNTO", idAsunto)

         dt = con.EjecutarSPConsultaDT(sp, sqlParameter)

         For Each row As DataRow In dt.Rows

            lsAsunto = row("T_DSC_ASUNTO_CONVOCATORIA").ToString()

         Next

      Catch ex As Exception
         lsAsunto = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenerAsuntoConvocatoria", "")
      Finally
         If Not con Is Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lsAsunto
   End Function

   ''' <summary>
   ''' Obtiene los documentos ogligatorios de alguna visita,[paso],[documento],[obligatorios o no],[words o pdfs]
   ''' </summary>
   ''' <param name="idVisita"></param>
   ''' <param name="idPaso"></param>
   ''' <param name="idDocumento"></param>
   ''' <param name="iBanderaObligatorio">costantes.obligatorios</param>
   ''' <param name="iTipoDocumento">constantes.tipoDocumento</param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ObtenerDocumentosObligatorios(idVisita As Integer,
                                       Optional idPaso As Integer = Constantes.Todos,
                                       Optional idDocumento As Integer = Constantes.Todos,
                                       Optional iBanderaObligatorio As Integer = Constantes.Todos,
                                       Optional iTipoDocumento As Integer = Constantes.TipoArchivo.WORD) As List(Of Documento.DocumentoMini)
      Dim con As Conexion.SQLServer = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim lstDocMin As List(Of Documento.DocumentoMini)
      Dim visita As New Visita()
      Dim sp As String = Nothing
      visita = AccesoBD.getDetalleVisita(idVisita, -1)
      Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)
      Dim dr As SqlDataReader = Nothing

      Try
         con = New Conexion.SQLServer

         If Not IsNothing(visita) Then
            If (Convert.ToDateTime(visita.FechaRegistro).ToString("yyyy/MM/dd") < Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
               sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_BUSCA_CARGA_DOCUMENTOS")
            Else
               sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_BUSCA_CARGA_DOCUMENTOS_V17")
            End If
         End If

         Dim sqlParameter(4) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", idVisita)
         sqlParameter(1) = New SqlParameter("@I_ID_PASO", idPaso)
         sqlParameter(2) = New SqlParameter("@N_ID_DOCUMENTO", idDocumento)
         sqlParameter(3) = New SqlParameter("@F_FLAG_OBLIG", iBanderaObligatorio)
         sqlParameter(4) = New SqlParameter("@TIPO_DOCUMENTO", iTipoDocumento)

         dr = con.EjecutarSPConsultaDR(sp, sqlParameter)

         lstDocMin = CType(dr.ToList(Of Documento.DocumentoMini)(), List(Of Documento.DocumentoMini))

      Catch ex As Exception
         lstDocMin = New List(Of Documento.DocumentoMini)
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenObjetoVisita", "")
      Finally
         If dr IsNot Nothing Then
            If Not dr.IsClosed Then
               dr.Close() : dr = Nothing
            End If
         End If
         If Not con Is Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try


        If ((idPaso > 5 And idPaso < 8) Or idPaso = 13) And lstDocMin.Count > 0 Then 'Or idPaso > 12' Then 
            For i = lstDocMin.Count - 1 To 0 Step -1
                If lstDocMin(i).I_ID_PASO = 5 Or lstDocMin(i).I_ID_PASO = 12 Then
                    lstDocMin.RemoveAt(i)
                End If
            Next

            'lstDocMin.RemoveAt(0)
        End If

        Return lstDocMin
   End Function

   Public Shared Function ObtenerDocumentosObligatoriosProrroga(idVisita As Integer) As Integer
      Dim con As Conexion.SQLServer = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim lsRes As Integer = 0

      Dim dt As DataTable = New DataTable()
      Try
         con = New Conexion.SQLServer
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_BUSCA_CARGA_DOCUMENTOS_V17_PRORROGA")


         Dim sqlParameter(0) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", idVisita)

         dt = con.EjecutarSPConsultaDT(sp, sqlParameter)

         For Each row As DataRow In dt.Rows
            If Not (Int32.TryParse(IIf(IsDBNull(row("TOTDOSCPRORROGA")), 0, row("TOTDOSCPRORROGA").ToString()), lsRes)) Then
               lsRes = 0
            End If
         Next

      Catch ex As Exception
         lsRes = 0
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenerDocumentosObligatoriosProrroga", "")
      Finally
         If Not con Is Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lsRes
   End Function

   ''' <summary>
   ''' Documentos por paso y usuario
   ''' </summary>
   ''' <param name="piUsuario"></param>
   ''' <param name="idPaso"></param>
   ''' <param name="idDocumento"></param>
   ''' <param name="iBanderaObligatorio"></param>
   ''' <param name="iTipoDocumento"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ObtenerDocumentosObligatoriosPorPasoUsuario(piUsuario As String,
                                       Optional idPaso As Integer = Constantes.Todos,
                                       Optional idDocumento As Integer = Constantes.Todos,
                                       Optional iBanderaObligatorio As Integer = Constantes.Todos,
                                       Optional iTipoDocumento As Integer = Constantes.TipoArchivo.WORD,
                                       Optional piIVisitaSisvig As Integer = Constantes.Todos) As List(Of Documento.DocumentoMini)
      Dim con As Conexion.SQLServer = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim lstDocMin As List(Of Documento.DocumentoMini)

      Dim dr As SqlDataReader = Nothing
      Try
         con = New Conexion.SQLServer
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_BUSCA_CARGA_DOCUMENTOS_SN_VISITA")


         Dim sqlParameter(5) As SqlParameter
         sqlParameter(0) = New SqlParameter("@T_ID_USUARIO", piUsuario)
         sqlParameter(1) = New SqlParameter("@I_ID_PASO", idPaso)
         sqlParameter(2) = New SqlParameter("@N_ID_DOCUMENTO", idDocumento)
         sqlParameter(3) = New SqlParameter("@F_FLAG_OBLIG", iBanderaObligatorio)
         sqlParameter(4) = New SqlParameter("@TIPO_DOCUMENTO", iTipoDocumento)
         sqlParameter(5) = New SqlParameter("@I_ID_VISITA_SISVIG", piIVisitaSisvig)

         dr = con.EjecutarSPConsultaDR(sp, sqlParameter)

         lstDocMin = CType(dr.ToList(Of Documento.DocumentoMini)(), List(Of Documento.DocumentoMini))

      Catch ex As Exception
         lstDocMin = New List(Of Documento.DocumentoMini)
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenObjetoVisita", "")
      Finally
         If dr IsNot Nothing Then
            If Not dr.IsClosed Then
               dr.Close() : dr = Nothing
            End If
         End If
         If Not con Is Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lstDocMin
   End Function

   ''' <summary>
   ''' Consulta los documentos que tiene un usuario
   ''' </summary>
   ''' <param name="idUsuario"></param>
   ''' <param name="idPaso"></param>
   ''' <param name="idDocumento"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function consultarDocumentosUsuario(idUsuario As String,
                                              Optional idPaso As Integer = Constantes.Todos,
                                              Optional idDocumento As Integer = Constantes.Todos,
                                              Optional idVigencia As Integer = Constantes.Vigencia.Vigente) As DataTable
      Dim dt As DataTable
      Dim con As Conexion.SQLServer = Nothing

      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DOCUMENTOS_USUARIO_V17")


         Dim sqlParameter(3) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_USUARIO", idUsuario)
         sqlParameter(1) = New SqlParameter("@I_ID_PASO", idPaso)
         sqlParameter(2) = New SqlParameter("@N_ID_DOCUMENTO", idDocumento)
         sqlParameter(3) = New SqlParameter("@ID_VIGENCIA", idVigencia)

         dt = con.EjecutarSPConsultaDT(sp, sqlParameter)
      Catch ex As Exception
         dt = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, consultarDocumentos", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return dt
   End Function

   ''' <summary>
   ''' Migrar los documentos sin visita hacia la tabla operativa ya con visita creada
   ''' </summary>
   ''' <param name="piIdVisita"></param>
   ''' <param name="piIdUsuario"></param>
   ''' <param name="piIdPaso"></param>
   ''' <param name="piIdDocumento"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function MigrarDocumentosSinVisita(con As Conexion.SQLServer, tran As SqlClient.SqlTransaction,
                                                    piIdVisita As Integer, piIdUsuario As String,
                                                    Optional piIdPaso As Integer = Constantes.Todos,
                                                    Optional piIdDocumento As Integer = Constantes.Todos) As Boolean
      'Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try
         Dim sqlParameter(3) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@T_ID_USUARIO", piIdUsuario)
         sqlParameter(2) = New SqlParameter("@I_ID_PASO", piIdPaso)
         sqlParameter(3) = New SqlParameter("@N_ID_DOCUMENTO", piIdDocumento)

         lbRes = con.EjecutarSP("spI_BDS_GRL_MIGRA_DOCS_USUARIO_A_VISITA", sqlParameter, tran)
      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function


   Public Shared Function MigrarDocumentosSinVisita(piIdVisita As Integer, piIdUsuario As String,
                                                    Optional piIdPaso As Integer = Constantes.Todos,
                                                    Optional piIdDocumento As Integer = Constantes.Todos,
                                                    Optional piIdVisitaSisvig As Integer = Constantes.Todos) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try
         Dim sqlParameter(4) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@T_ID_USUARIO", piIdUsuario)
         sqlParameter(2) = New SqlParameter("@I_ID_PASO", piIdPaso)
         sqlParameter(3) = New SqlParameter("@N_ID_DOCUMENTO", piIdDocumento)
         sqlParameter(4) = New SqlParameter("@I_ID_VISITA_SISVIG", piIdVisitaSisvig)

         lbRes = con.EjecutarSP("spI_BDS_GRL_MIGRA_DOCS_USUARIO_A_VISITA", sqlParameter)
      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function


   ''' <summary>
   ''' Copia los documentos de la visita padre a la visita hija
   ''' </summary>
   ''' <param name="piIdVisita"></param>
   ''' <param name="piIdPaso"></param>
   ''' <param name="piIdDocumento"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function MigrarDocumentos(piIdVisita As Integer,
                                           Optional piIdPaso As Integer = Constantes.Todos,
                                           Optional piIdDocumento As Integer = Constantes.Todos) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try
         Dim sqlParameter(2) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@I_ID_PASO", piIdPaso)
         sqlParameter(2) = New SqlParameter("@N_ID_DOCUMENTO", piIdDocumento)

         lbRes = con.EjecutarSP("spI_BDS_GRL_MIGRA_DOCS_VISITA", sqlParameter)
      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function

   Public Shared Function ExisteObjetoVisita(dscObjetoVisita As String) As DataTable
      Dim dt As DataTable
      Dim con As Conexion.SQLServer = Nothing

      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_OBJETO_VISITA")


         Dim sqlParameter(0) As SqlParameter
         sqlParameter(0) = New SqlParameter("@T_DSC_OBJETO_VISITA_SISAN", dscObjetoVisita)

         dt = con.EjecutarSPConsultaDT(sp, sqlParameter)
      Catch ex As Exception
         dt = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ExisteObjetoVisita", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return dt
   End Function

   Public Shared Function ObtenMensajeDeError(idError As Integer) As String
      Dim con As Conexion.SQLServer = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim strQuery As String = "spS_BDS_GRL_GET_MENSAJE_ERROR"
      Dim lsRes As String = ""

      Dim dataReader As SqlDataReader

      Try
         Dim sqlParameter(1) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_ERROR", idError)
         dataReader = con.EjecutarSPConsultaDR(strQuery, sqlParameter)

         While dataReader.Read()
            lsRes = dataReader.GetString(0).ToString()
         End While
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenMensajeDeError", "")
         lsRes = ""
      Finally
         If Not con Is Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try

      Return lsRes
   End Function

   Public Shared Function InsertaObjetoVisita(piIdObjetoVisitaSisvig As Integer, psDscObjeto As String, piIdArea As Integer) As Integer
      Dim con As New Conexion.SQLServer
      Dim liObjetoVis As Integer = -1

      Try
         Dim sqlParameter(2) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_OBJETO_VISITA_SISAN", piIdObjetoVisitaSisvig)
         sqlParameter(1) = New SqlParameter("@T_DSC_OBJETO_VISITA", psDscObjeto)
         sqlParameter(2) = New SqlParameter("@I_ID_AREA", piIdArea)

         Dim dt = con.EjecutarSPConsultaDT("spI_BDS_GRL_INS_OBJETO_VISITA", sqlParameter)

         If Not IsNothing(dt) Then
            If dt.Rows.Count > 0 Then
               liObjetoVis = CInt(dt.Rows(0)("N_ID_OBJETO_VISITA"))
            End If
         End If

      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("InsertaObjetoVisita [" & psDscObjeto.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         liObjetoVis = -1
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try

      Return liObjetoVis
   End Function

   ''' <summary>
   ''' Obtiene a los usuarios involucrados en una visita por area y perfil
   ''' </summary>
   ''' <param name="idArea"></param>
   ''' <param name="piIDPerfil"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ObtenerUsuarioInvolucradosVisita(ByVal idArea As Integer, Optional piIDPerfil As Integer = -1) As List(Of String)

      Dim lstUsuariosPorArea As New List(Of String)

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_USUARIOS_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA", idArea))

         If piIDPerfil <> -1 Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PERFIL", piIDPerfil))
         End If

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
            lstUsuariosPorArea.Add(dataReader.GetString(0))
         End While

      Catch ex As Exception
         lstUsuariosPorArea = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getDetalleVisita", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return lstUsuariosPorArea

   End Function

   ''' <summary>
   ''' Actualiza los rangos de sancion de la visita
   ''' </summary>
   ''' <param name="piIdVisita"></param>
   ''' <param name="piRangoIni"></param>
   ''' <param name="piRangoFin"></param>
   ''' <param name="psComentariosRango"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ActualizarRangoSancion(piIdVisita As Integer, piRangoIni As Decimal,
                                                 piRangoFin As Decimal, psComentariosRango As String, psSubVisitas As String) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try
         Dim sqlParameter(4) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@N_RANGO_SANCION_INI", piRangoIni)
         sqlParameter(2) = New SqlParameter("@N_RANGO_SANCION_FIN", piRangoFin)
         sqlParameter(3) = New SqlParameter("@T_COMENTARIO_RANGO_SANCION", psComentariosRango)
         sqlParameter(4) = New SqlParameter("@SUB_VISITAS", psSubVisitas)

         lbRes = con.EjecutarSP("spU_BDS_GRL_ACTUALIZA_RANGO_SANCION_VISITA", sqlParameter)
      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function

   Public Shared Function ActualizarRangoSancion_V17(piIdFolioVisita As String, piRangoIni As Decimal,
                                                 piRangoFin As Decimal, psComentariosRango As String,
                                                 Optional opcVS As Integer = 0) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try
         Dim sqlParameter(4) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_FOLIO_VISITA", piIdFolioVisita)
         sqlParameter(1) = New SqlParameter("@N_RANGO_SANCION_INI", piRangoIni)
         sqlParameter(2) = New SqlParameter("@N_RANGO_SANCION_FIN", piRangoFin)
         sqlParameter(3) = New SqlParameter("@T_COMENTARIO_RANGO_SANCION", psComentariosRango)
         sqlParameter(4) = New SqlParameter("@OPC_SV", opcVS)

         lbRes = con.EjecutarSP("spU_BDS_GRL_ACTUALIZA_RANGO_SANCION_VISITA_V17", sqlParameter)
      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdFolioVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function

   Public Shared Function ActualizarRangoImpSancion(piIdVisita As Integer, pdFecha As DateTime, psMonto As Decimal,
                                                    psComentariosRango As String, psSubVisitas As String) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try
         Dim sqlParameter(4) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@N_MONTO", psMonto)
         sqlParameter(2) = New SqlParameter("@T_COMENTARIO", psComentariosRango)
         sqlParameter(3) = New SqlParameter("@FECH_IMP_SAN", pdFecha)
         sqlParameter(4) = New SqlParameter("@SUB_VISITAS", psSubVisitas)

         lbRes = con.EjecutarSP("spU_BDS_GRL_ACTUALIZA_MONTO_IMP_SANCION", sqlParameter)
      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function

   Public Shared Function consultarEstatusVencimiento() As DataTable
      Dim dt As DataTable
      Dim con As Conexion.SQLServer = Nothing

      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_ESTATUS_VENCIMIENTO")

         dt = con.EjecutarSPConsultaDT(sp)
      Catch ex As Exception
         dt = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, spS_BDS_GRL_GET_ESTATUS_VENCIMIENTO", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return dt
   End Function

   Public Shared Function ObtenSigSubVisitaRangoSancion(piVisita As Integer) As String
      Dim con As Conexion.SQLServer = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim strQuery As String = "spS_BDS_GRL_GET_SIG_FOLIO_SUBVISITA_PARA_RANGO"
      Dim svRes As String = ""

      Dim dataReader As SqlDataReader

      Try
         Dim sqlParameter(0) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA_PADRE", piVisita)

         dataReader = con.EjecutarSPConsultaDR(strQuery, sqlParameter)

         While dataReader.Read()
            svRes = dataReader.GetString(0)
         End While
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenSigSubVisitaRangoSancion", "")
         svRes = ""
      Finally
         If Not con Is Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try

      Return svRes
   End Function

   ''' <summary>
   ''' Agc obtiene la fecha limite de atencion de un paso
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ObtenFechaLimiteAtender_Paso(piVisita As Integer, piPaso As Integer) As Date
      Dim con As Conexion.SQLServer = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim strQuery As String = "spS_BDS_GRL_GET_FECHA_FINALIZA_PASO"
      Dim lsRes As Date = Date.MinValue

      Dim dataReader As SqlDataReader

      Try
         Dim sqlParameter(1) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piVisita)
         sqlParameter(1) = New SqlParameter("@I_ID_PASO", piPaso)

         dataReader = con.EjecutarSPConsultaDR(strQuery, sqlParameter)

         While dataReader.Read()
            lsRes = dataReader.GetDateTime(0)
         End While
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenObjetoVisita", "")
         lsRes = Date.MinValue
      Finally
         If Not con Is Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try

      Return lsRes
   End Function

   ''' <summary>
   ''' Busca los documentos que a la fecha no fueron adjuntados y que ya se paso el ultimo dia del ultimo paso para ser adjuntados
   ''' </summary>
   ''' <param name="piIdVisita"></param>
   ''' <param name="piIdPaso"></param>
   ''' <param name="piIdDocumento"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   ''' 


   'Dim visita As New Visita()
   'Dim sp As String = Nothing
   '    visita = AccesoBD.getDetalleVisita(idVisita, -1)
   'Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)
   'Dim dr As SqlDataReader = Nothing

   '    Try
   '        con = New Conexion.SQLServer

   '        If Not IsNothing(visita) Then
   '            If (Convert.ToDateTime(visita.FechaRegistro).ToString("yyyy/MM/dd") < Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
   '                sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_BUSCA_CARGA_DOCUMENTOS")
   '            Else
   '                sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_BUSCA_CARGA_DOCUMENTOS_V17")
   '            End If
   '        End If




   Public Shared Function ActualizaDocumentosNoAdjuntados(piIdVisita As Integer, Optional piIdPaso As Integer = -1, Optional piIdDocumento As Integer = -1) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Dim visita As New Visita()

      visita = AccesoBD.getDetalleVisita(piIdVisita, -1)
      Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

      Try
         Dim sqlParameter(1) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@N_ID_DOCUMENTO", piIdDocumento)

         If Not IsNothing(visita) Then
            If (Convert.ToDateTime(visita.FechaRegistro).ToString("yyyy/MM/dd") < Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
               lbRes = con.EjecutarSP("spS_BDS_GRL_UP_DOCUMENTOS_NO_ADJUNTADOS", sqlParameter)
            Else
               lbRes = con.EjecutarSP("spS_BDS_GRL_UP_DOCUMENTOS_NO_ADJUNTADOS_V17", sqlParameter)
            End If
         End If

      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function

   Public Shared Function AlertaVisitasPorVencer(psUsusario As String) As DataTable
      Dim dt As DataTable
      Dim con As Conexion.SQLServer = Nothing

      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sqlParameter(0) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_USUARIO", psUsusario)

         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_ALERTA_VISITAS_POR_VENCER")

         dt = con.EjecutarSPConsultaDT(sp, sqlParameter)
      Catch ex As Exception
         dt = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, spS_BDS_GRL_GET_ALERTA_VISITAS_POR_VENCER", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return dt
   End Function

   Public Shared Function EsVisitaConjunta(IdVisitaConjunta As Integer) As Integer
      Dim dt As DataTable
      Dim con As Conexion.SQLServer = Nothing
      Dim IdVisitaC As Integer = 0

      con = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim sqlParameter(0) As SqlParameter
      sqlParameter(0) = New SqlParameter("@I_ID_VISITA", IdVisitaConjunta)

      Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_VISITA_CONJUNTA")

      dt = con.EjecutarSPConsultaDT(sp, sqlParameter)

      If dt.Rows.Count > 0 Then
         IdVisitaC = dt.Rows(0)("I_ID_AREA_VC")
      Else
         IdVisitaC = 0
      End If

      Return IdVisitaC
   End Function

   Public Shared Function ObtenerUsuarioInvolucradosVisitaDS(ByVal idArea As Integer, Optional piIDPerfil As Integer = -1, Optional idAreaC As Integer = 0) As DataTable

      Dim dt As New DataTable

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_USUARIOS_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)

         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA", idArea))
         If piIDPerfil <> -1 Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_PERFIL", piIDPerfil))
         End If

         If idAreaC > 0 Then
            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA_CONJUNTA", idAreaC))
         End If

         dt = con.EjecutarSPConsultaDT(sp, SqlParameters.ToArray)

      Catch ex As Exception
         dt = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenerUsuarioInvolucradosVisitaDS", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return dt

   End Function

   Private Shared Function ObtenerObjetoVisita(piIdVisita As Integer) As DataTable
      Dim dt As New DataTable

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_OBJETOS_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", piIdVisita))

         dt = con.EjecutarSPConsultaDT(sp, SqlParameters.ToArray)

      Catch ex As Exception
         dt = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenerObjetoVisita", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return dt
   End Function

   ''' <summary>
   ''' Obtiene algunos datatables para los filtros en bandeja
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ObtenerDatosFiltro(idTipoBandeja As Constantes.Pantalla, psIdUsu As String, piIdArea As Integer) As DataSet

      Dim ds As New DataSet

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DATOS_FILTRO")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@ID_TIPO_BANDEJA", idTipoBandeja))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@ID_USUARIO", psIdUsu))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@ID_AREA_USU", piIdArea))

         ds = con.EjecutarSPConsultaDS(sp, SqlParameters.ToArray)

      Catch ex As Exception
         ds = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenerDatosFiltro", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return ds

   End Function


   ''' <summary>
   ''' Obtiene una fecha de la base de datos agregado dias habiles o naturales
   ''' </summary>
   ''' <param name="pdFechaBase"></param>
   ''' <param name="piDias"></param>
   ''' <param name="piIncremetaDecremeta"></param>
   ''' <param name="piHabilesInhabiles"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ObtenerFecha(pdFechaBase As DateTime, piDias As Integer,
                                        Optional piIncremetaDecremeta As Integer = Constantes.IncremeteDecrementa.Incrementa,
                                        Optional piHabilesInhabiles As Integer = Constantes.TipoDias.Habiles) As DateTime
      Dim dt As New DataTable
      Dim ldFechaAux As DateTime
      Dim ldFechaGenerada As DateTime = DateTime.Now
      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim lsQuery As String = "SELECT " & Owner & "FUN_DIAS_HABILES ('" & pdFechaBase.ToString("yyyy-MM-dd") & "', " & piDias.ToString() & ", " & piIncremetaDecremeta.ToString() & ", " & piHabilesInhabiles.ToString() & ")"

         dt = con.ConsultarDT(lsQuery)

         If dt.Rows.Count > 0 Then
            If DateTime.TryParse(dt.Rows(0)(0), ldFechaAux) Then
               ldFechaGenerada = New DateTime(ldFechaAux.Year, ldFechaAux.Month, ldFechaAux.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
            Else
               ldFechaGenerada = Date.MinValue
            End If
         End If
      Catch ex As Exception
         ldFechaGenerada = Date.MinValue
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenerFecha", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return ldFechaGenerada
   End Function

   ''' <summary>
   ''' ACTUALIZA UNA VISITA CUANDO NO HAY SANCION
   ''' </summary>
   ''' <param name="piIdVisita"></param>
   ''' <param name="piIdPaso"></param>
   ''' <param name="pbExisteSancion"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ActualizaSancionVisita(piIdVisita As Integer, piIdPaso As Integer,
                                                Optional pbExisteSancion As Integer = Constantes.Falso) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try
         Dim sqlParameter(2) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@N_ID_PASO", piIdPaso)
         sqlParameter(2) = New SqlParameter("@B_FLAG_SANCION", pbExisteSancion)

         lbRes = con.EjecutarSP("spS_BDS_GRL_UP_BANDERA_SANCION", sqlParameter)
      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function


   ''' <summary>
   ''' Actualiza el valor de un parametro en base de datps
   ''' </summary>
   ''' <param name="psNuevoValorParam"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ActualizaParametroBD(psNuevoValorParam As String,
                                                 psDscParam As String,
                                                 Optional piIdParam As Integer = 0) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try
         Dim sqlParameter(2) As SqlParameter
         sqlParameter(0) = New SqlParameter("@T_DSC_VALOR", psNuevoValorParam)
         sqlParameter(1) = New SqlParameter("@T_DSC_PARAMETRO", psDscParam)
         sqlParameter(2) = New SqlParameter("@N_ID_PARAMETRO", piIdParam)

         lbRes = con.EjecutarSP("spU_BDS_GRL_ACTUALIZA_VALOR_PARAMETRO", sqlParameter)
      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("psNuevoValorParam [" & psNuevoValorParam & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function

   ''' <summary>
   ''' Actualiza la badera de las subvisitas para que caminen con la visita padre
   ''' </summary>
   ''' <param name="piIdVisita"></param>
   ''' <param name="psSubVisitas"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ActualizaBanderaSubvisita(piIdVisita As Integer, psSubVisitas As String, psIdUsuario As String) As Boolean
      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try
         Dim sqlParameter(2) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@SUB_VISITAS", psSubVisitas)
         sqlParameter(2) = New SqlParameter("@I_ID_USUARIO", psIdUsuario)

         lbRes = con.EjecutarSP("spU_BDS_GRL_ACTUALIZA_SUBVISITAS", sqlParameter)
      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("spU_BDS_GRL_ACTUALIZA_SUBVISITAS piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function

   ''' <summary>
   ''' Actualiza los responsables de la visita
   ''' </summary>
   ''' <param name="piIdVisita"></param>
   ''' <param name="psRespVisita"></param>
   ''' <param name="piTipoResponsables"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function ActualizaResponsablesVisita(piIdVisita As Integer, psRespVisita As String,
                                                      piTipoResponsables As Constantes.ResponsablesVisita,
                                                      Optional piTipoOperacion As Constantes.OPERCION = Constantes.OPERCION.Actualizar) As Boolean
      If psRespVisita = "" Then
         Return True
      End If

      Dim con As New Conexion.SQLServer
      Dim lbRes As Boolean = False
      Try
         Dim sqlParameter(3) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@V_RESP_VISITA", psRespVisita)
         sqlParameter(2) = New SqlParameter("@I_ID_TIPO_RESP", piTipoResponsables)
         sqlParameter(3) = New SqlParameter("@I_TIPO_OPERACION", piTipoOperacion)

         lbRes = con.EjecutarSP("spU_BDS_GRL_ACTUALIZA_RESPONSABLE_VISITA", sqlParameter)
      Catch ex As Exception
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
         Utilerias.ControlErrores.EscribirEvento("spU_BDS_GRL_ACTUALIZA_RESPONSABLE_VISITA piIdVisita [" & piIdVisita.ToString() & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Error)
         lbRes = False
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try
      Return lbRes
   End Function

   Public Shared Function ObtenerRespuesAfore(piIdVisita As Integer) As DataTable
      Dim dt As New DataTable

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_RESPUESTA_AFORE")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", piIdVisita))

         dt = con.EjecutarSPConsultaDT(sp, SqlParameters.ToArray)

      Catch ex As Exception
         dt = Nothing
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ObtenerObjetoVisita", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return dt
   End Function

   Public Shared Function ActualizaBanderaCambioNormativa(ByVal con As Conexion.SQLServer,
                                               ByVal tran As SqlClient.SqlTransaction, piIdVisita As Integer) As Boolean
      Dim registroExitoso As Boolean = True

      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spU_BDS_GRL_ACTUALIZA_BANDERA_CAMBIO_NORMATIVA")

         Dim sqlParameter(0) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)

         registroExitoso = con.EjecutarSP(sp, sqlParameter, tran)

      Catch ex As Exception
         registroExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ActualizaBanderaCambioNormativa", "")
      End Try

      Return registroExitoso
   End Function

   Public Shared Function ActualizaEstatusVisita(ByVal con As Conexion.SQLServer,
                                               ByVal tran As SqlClient.SqlTransaction, piIdVisita As Integer) As Boolean
      Dim registroExitoso As Boolean = True

      Try

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spU_BDS_GRL_ACTUALIZA_ESTATUS_VISITA")

         Dim sqlParameter(0) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)

         registroExitoso = con.EjecutarSP(sp, sqlParameter, tran)

      Catch ex As Exception
         registroExitoso = False
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ActualizaEstatusVisita", "")
      End Try

      Return registroExitoso

   End Function

   Public Shared Function getDescFase(ByVal id_Paso As Integer) As String
      Dim con As New Conexion.SQLServer

      Dim dr As SqlClient.SqlDataReader = Nothing
      Dim dscFase As String = ""
      Dim sqlQuery = ""
      Try

         sqlQuery = "SELECT F.T_DSC_FASE FROM dbo.BDS_C_GR_FASES_V17 F INNER JOIN [dbo].[BDS_C_GR_PASOS_V17] P ON P.I_ID_FASE = F.I_ID_FASE WHERE P.I_ID_PASO = " & id_Paso

         dr = con.ConsultarDR(sqlQuery)

         If dr IsNot Nothing Then
            dr.Read()
            If dr.HasRows Then
               If Not IsDBNull(dr.Item(0)) Then
                  dscFase = dr.Item(0).ToString
               End If
            End If
         End If

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getDescFase", "")
      Finally
         If dr IsNot Nothing Then
            dr.Close()
            dr = Nothing
         End If
         If con IsNot Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try

      Return dscFase

   End Function

   Public Shared Function getFechaEstimadaPorPaso(ByVal id_visita As Integer, ByVal id_Paso As Integer) As String
      Dim con As Conexion.SQLServer = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim strQuery As String = "spS_BDS_GRL_GET_FECHA_ESTIMADA_POR_PASO"
      Dim fechaEstimada As String = ""

      Dim dataReader As SqlDataReader

      Try
         Dim sqlParameter(1) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_PASO", id_Paso)
         sqlParameter(1) = New SqlParameter("@I_ID_VISITA", id_visita)

         dataReader = con.EjecutarSPConsultaDR(strQuery, sqlParameter)

         While dataReader.Read()
            fechaEstimada = dataReader.GetDateTime(0).ToString("dd/MM/yyyy")
         End While
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getFechaEstimadaPorPaso", "")
         fechaEstimada = Nothing
      Finally
         If Not con Is Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try

      Return fechaEstimada

   End Function

   Public Shared Function getFechaRegistroVisita(ByVal id_Folio As String) As String
      Dim con As Conexion.SQLServer = New Conexion.SQLServer
      Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
      Dim strQuery As String = "spS_BDS_GRL_GET_FECHA_REGISTRO_VISITA"
      Dim fechaRegistro As String = ""

      Dim dataReader As SqlDataReader

      Try
         Dim sqlParameter(0) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_FOLIO", id_Folio)

         dataReader = con.EjecutarSPConsultaDR(strQuery, sqlParameter)

         While dataReader.Read()
            fechaRegistro = dataReader.GetDateTime(0).ToString("dd/MM/yyyy")
         End While
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getFechaRegistroVisita", "")
         fechaRegistro = Nothing
      Finally
         If Not con Is Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try

      Return fechaRegistro

   End Function

   Public Shared Function getErrorValidacionSupervisar(ByVal id_error As String) As String
      Dim con As New Conexion.SQLServer

      Dim dr As SqlClient.SqlDataReader = Nothing
      Dim dscError As String = ""
      Dim sqlQuery = ""
      Try

         sqlQuery = "SELECT T_DSC_ERROR_VAL FROM dbo.BDS_C_GR_ERROR_VALIDACION WHERE N_ID_ERROR_VAL = " & id_error

         dr = con.ConsultarDR(sqlQuery)

         If dr IsNot Nothing Then
            dr.Read()
            If dr.HasRows Then
               If Not IsDBNull(dr.Item(0)) Then
                  dscError = dr.Item(0).ToString
               End If
            Else
               dscError = "Error desconocido. Contacte al administrador de la aplicación."
            End If
         End If

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getErrorValidacionSupervisar", "")
      Finally
         If dr IsNot Nothing Then
            dr.Close()
            dr = Nothing
         End If
         If con IsNot Nothing Then
            con.CerrarConexion()
            con = Nothing
         End If
      End Try

      If CInt(id_error) = 2165 Then
         Dim liNumDiasAux As Integer = 0
         Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.DiasFechasAnteriores), liNumDiasAux)
         Dim ldFechaAuxAnterior As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDiasAux, Constantes.IncremeteDecrementa.Decrementa)

         dscError = dscError.Replace("[DIAS]", Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.DiasFechasAnteriores))
         dscError = dscError.Replace("[FECHA]", ldFechaAuxAnterior.Date.ToString("dd/MM/yyyy"))
      End If

      If CInt(id_error) = 2164 Then
         dscError = dscError.Replace(": [FECHA]", "")
      End If

      If CInt(id_error) = 2163 Then
         dscError = dscError.Replace("al dia: [FECHA].", "a la fecha de termino del paso actual.")
      End If

      If CInt(id_error) = 2162 Then
         Dim liNumDias As Integer = RecuperaParametroDiasFecha()
         dscError = dscError.Replace("[NUM_DIAS]", liNumDias.ToString())
      End If

      Return dscError

   End Function

   Public Shared Function RecuperaParametroDiasFecha() As Integer
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

   ''' <summary>
   ''' CONSULTA EL DETALLE DE UNA VISITA
   ''' </summary>
   ''' <param name="idVisitaGenerado"></param>
   ''' <param name="idAreaActual"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared Function getDetalleVisita(ByVal idVisitaGenerado As Integer,
                                           ByVal idAreaActual As Integer) As Visita

      Dim visita As New Visita()

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DETALLE_VISITA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", idVisitaGenerado))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA_ACTUAL", idAreaActual))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)
         visita.ExisteVisita = False

         While dataReader.Read()
            visita.ExisteVisita = True
            visita.FolioVisita = dataReader.GetString(0)

            visita.FechaRegistro = dataReader.GetDateTime(1)
            If Not dataReader.IsDBNull(2) Then
               visita.FechaInicioVisita = dataReader.GetDateTime(2)
            Else
               visita.FechaInicioVisita = Date.MinValue
            End If

            visita.NombreEntidad = dataReader.GetString(3)
            visita.NombreInspectorResponsable = dataReader.GetString(4)
            visita.DescripcionTipoVisita = dataReader.GetString(5)
            visita.IdArea = dataReader.GetInt32(6)
            Dim usuario As New Entities.Usuario()
            usuario.CargarDatos(dataReader.GetString(7))
            visita.Usuario = New Entities.Usuario()
            visita.Usuario = usuario
            'visita.Usuario.IdentificadorUsuario = dataReader.GetString(7)
            visita.IdEntidad = dataReader.GetInt32(8)
            visita.IdTipoVisita = dataReader.GetInt32(9)
            visita.IdInspectorResponsable = dataReader.GetString(10)
            visita.IdAbogadoSancion = dataReader.GetString(11)
            visita.NombreAbogadoSancion = dataReader.GetString(12)
            visita.IdEstatusActual = dataReader.GetInt32(13)
            visita.IdPasoActual = dataReader.GetInt32(14)
            visita.EsCancelada = dataReader.GetBoolean(15)
            visita.MotivoCancelacion = dataReader.GetString(16)
            visita.IdTipoEntidad = dataReader.GetInt32(17)
            If dataReader.IsDBNull(18) Then
               visita.FechaCancela = Nothing
            Else
               visita.FechaCancela = dataReader.GetDateTime(18)
            End If
            visita.IdVisitaGenerado = dataReader.GetInt32(19)

            Dim liAux As Integer = 0

            If Int32.TryParse(dataReader.GetInt32(20), liAux) Then
               If liAux > 0 Then
                  visita.EsPasoReactivado = True
               Else
                  visita.EsPasoReactivado = False
               End If
            End If

            visita.IdAbogadoAsesor = dataReader.GetString(21)
            visita.NombreAbogadoAsesor = dataReader.GetString(22)
            visita.ComentariosIniciales = dataReader.GetString(23)
            visita.DescripcionVisita = dataReader.GetString(24)
            visita.IdObjetoVisita = dataReader.GetDecimal(25)
            visita.DscObjetoVisitaOtro = dataReader.GetString(26)
            visita.IdSubentidad = dataReader.GetInt32(27)
            visita.DscSubentidad = dataReader.GetString(28)

            visita.IdVisitaPadreSubvisita = dataReader.GetInt32(29)
            visita.IdVisitaPadreCopia = dataReader.GetInt32(30)
            If Not dataReader.IsDBNull(31) Then
               visita.ComentariosPasoEstatus = dataReader.GetString(31)
            Else
               visita.ComentariosPasoEstatus = ""
            End If

            If dataReader.IsDBNull(32) Then
               visita.FechaReunionPresidencia = Nothing
            Else
               visita.FechaReunionPresidencia = dataReader.GetDateTime(32)
            End If

            If dataReader.IsDBNull(33) Then
               visita.FechaReunionAfore = Nothing
            Else
               visita.FechaReunionAfore = dataReader.GetDateTime(33)
            End If

            visita.IdAbogadoConstencioso = dataReader.GetString(34)
            visita.NombreAbogadoContencioso = dataReader.GetString(35)

            If Not dataReader.IsDBNull(36) Then
               visita.FECH_VISITA_CAMPO__INI = dataReader.GetDateTime(36)
            Else
               visita.FECH_VISITA_CAMPO__INI = Date.MinValue
            End If

            If Not dataReader.IsDBNull(37) Then
               visita.FECH_VISITA_CAMPO__FIN = dataReader.GetDateTime(37)
            Else
               visita.FECH_VISITA_CAMPO__FIN = Date.MinValue
            End If

            If Not dataReader.IsDBNull(38) Then
               visita.FECH_REUNION__PRESIDENCIA = dataReader.GetDateTime(38)
            Else
               visita.FECH_REUNION__PRESIDENCIA = Date.MinValue
            End If
            visita.RANGO_SANCION_INI = dataReader.GetDecimal(39)
            visita.RANGO_SANCION_FIN = dataReader.GetDecimal(40)
            visita.COMENTARIO_RANGO_SANCION = dataReader.GetString(41)

            If Not dataReader.IsDBNull(42) Then
               visita.FECH_REUNION__AFORE = dataReader.GetDateTime(42)
            Else
               visita.FECH_REUNION__AFORE = Date.MinValue
            End If

            If Not dataReader.IsDBNull(43) Then
               visita.Fecha_LimitePasoActual = dataReader.GetDateTime(43)
            Else
               visita.Fecha_LimitePasoActual = AccesoBD.ObtenFechaLimiteAtender_Paso(visita.IdVisitaGenerado, visita.IdPasoActual)
            End If

            If Not dataReader.IsDBNull(44) Then
               visita.FechaInicioPasoActual = dataReader.GetDateTime(44)
            Else
               visita.FechaInicioPasoActual = DateTime.MinValue
            End If

            If Not dataReader.IsDBNull(45) Then
               visita.DiasTranscurridosPasoActual = dataReader.GetInt32(45)
            Else
               visita.DiasTranscurridosPasoActual = 0
            End If

            visita.EsSubVisitaOsubFolio = IIf(visita.IdVisitaPadreCopia <> 0 Or visita.IdVisitaPadreSubvisita <> 0, True, False)
            visita.EsSubVisita = IIf(visita.IdVisitaPadreSubvisita <> 0, True, False)

            If Not dataReader.IsDBNull(46) Then
               visita.TieneProrroga = dataReader.GetBoolean(46)
            Else
               visita.TieneProrroga = False
            End If

            If Not dataReader.IsDBNull(47) Then
               visita.TieneProrrogaAprobada = dataReader.GetBoolean(47)
            Else
               visita.TieneProrrogaAprobada = False
            End If

            If Not dataReader.IsDBNull(48) Then
               visita.TieneSancion = dataReader.GetBoolean(48)
            Else
               visita.TieneSancion = True
            End If

            If Not dataReader.IsDBNull(49) Then
               visita.DescripcionPasoActual = dataReader.GetString(49)
            Else
               visita.DescripcionPasoActual = ""
            End If

            If Not dataReader.IsDBNull(50) Then
               visita.IdAbogadoSupervisor = dataReader.GetString(50)
            Else
               visita.IdAbogadoSupervisor = ""
            End If

            If Not dataReader.IsDBNull(51) Then
               visita.NombreAbogadoSupervisor = dataReader.GetString(51)
            Else
               visita.NombreAbogadoSupervisor = ""
            End If

            If Not dataReader.IsDBNull(52) Then
               visita.UltimoUsuarioComentario = dataReader.GetString(52)
            Else
               visita.UltimoUsuarioComentario = ""
            End If

            If Not dataReader.IsDBNull(53) Then
               visita.UltimoComentario = dataReader.GetString(53)
            Else
               visita.UltimoComentario = ""
            End If

            If Not dataReader.IsDBNull(54) Then
               visita.UltimoUsuarioDocumento = dataReader.GetString(54)
            Else
               visita.UltimoUsuarioDocumento = ""
            End If


            If Not dataReader.IsDBNull(56) Then
               Dim lsSubVisitas As String = dataReader.GetString(56)
               Dim lsFoliosSubVisitas As String = IIf(dataReader.IsDBNull(57), "", dataReader.GetString(57))
               Dim lsFoliosSeleccionado As String = IIf(dataReader.IsDBNull(73), "", dataReader.GetString(73))

               Dim lstSubVisitas As New List(Of Visita.SubVisitas)

               ''Eliminamos ultimo "|"
               If lsSubVisitas.Length > 1 Then
                  lsSubVisitas = lsSubVisitas.Substring(0, lsSubVisitas.Length - 1)
                  lsFoliosSubVisitas = lsFoliosSubVisitas.Substring(0, lsFoliosSubVisitas.Length - 1)

                  Dim vecSubVisitas As String() = lsSubVisitas.Split("|")
                  Dim vecFoliosSubVisitas As String() = lsFoliosSubVisitas.Split("|")
                  Dim liAuxSubV As Integer = 0

                  Dim objSubVisitas As Visita.SubVisitas

                  If vecSubVisitas.Length <> vecFoliosSubVisitas.Length Then
                     For i As Integer = 0 To vecSubVisitas.Length - 1
                        vecFoliosSubVisitas(i) = "SB" & i.ToString()
                     Next
                  End If

                  For i As Integer = 0 To vecSubVisitas.Length - 1
                     If Int32.TryParse(vecSubVisitas(i), liAuxSubV) Then
                        Dim lsFolio As String = vecFoliosSubVisitas(i)

                        Try : lsFolio = lsFolio.Substring(lsFolio.LastIndexOf("/", lsFolio.Length - 1) + 1) : Catch : lsFolio = "SB" & i.ToString() : End Try

                        objSubVisitas = New Visita.SubVisitas With {.Id = liAuxSubV, .Descripcion = lsFolio, .Folio = vecFoliosSubVisitas(i),
                                                                    .EstaSeleccionada = IIf(lsFoliosSeleccionado.Contains(vecFoliosSubVisitas(i)), True, False)}
                        lstSubVisitas.Add(objSubVisitas)
                     End If
                  Next

                  If lstSubVisitas.Count > 0 Then
                     visita.TieneSubVisitas = True
                     visita.LstSubVisitas = lstSubVisitas
                  Else
                     visita.TieneSubVisitas = False
                     visita.LstSubVisitas = New List(Of Visita.SubVisitas)
                  End If
               Else
                  visita.TieneSubVisitas = False
                  visita.LstSubVisitas = New List(Of Visita.SubVisitas)
               End If
            Else
               visita.TieneSubVisitas = False
               visita.LstSubVisitas = New List(Of Visita.SubVisitas)
            End If

            If Not dataReader.IsDBNull(58) Then
               visita.Fecha_ReunionVjPaso9 = dataReader.GetDateTime(58)
            Else
               visita.Fecha_ReunionVjPaso9 = Date.MinValue
            End If

            If Not dataReader.IsDBNull(59) Then
               visita.Fecha_ReunionVjPaso16 = dataReader.GetDateTime(59)
            Else
               visita.Fecha_ReunionVjPaso16 = Date.MinValue
            End If

            If Not dataReader.IsDBNull(60) Then
               visita.SolicitoFechaPaso9 = dataReader.GetBoolean(60)
            Else
               visita.SolicitoFechaPaso9 = False
            End If

            If Not dataReader.IsDBNull(61) Then
               visita.SolicitoFechaPaso16 = dataReader.GetBoolean(61)
            Else
               visita.SolicitoFechaPaso16 = False
            End If

            If Not dataReader.IsDBNull(62) Then
               visita.Fecha_InSituActaCircunstanciada = dataReader.GetDateTime(62)
            Else
               visita.Fecha_InSituActaCircunstanciada = Date.MinValue
            End If

            If Not dataReader.IsDBNull(63) Then
                    visita.Fecha_LevantamientoActaConclusion = dataReader.GetDateTime(63)
                Else
                    visita.Fecha_LevantamientoActaConclusion = Date.MinValue
                End If

            If Not dataReader.IsDBNull(55) And Not dataReader.IsDBNull(64) Then
               Dim lsListaDocumentos As String = dataReader.GetString(55)
               Dim lsListaDocumentosOri As String = dataReader.GetString(64)

               If lsListaDocumentos.Length > 1 Then
                  Dim lstDocumentosString As New List(Of Visita.DocumentoCargado)
                  Dim objDoctoCargado As Visita.DocumentoCargado

                  ''Eliminamos ultimo |
                  lsListaDocumentos = lsListaDocumentos.Substring(0, lsListaDocumentos.Length - 1)
                  lsListaDocumentosOri = lsListaDocumentosOri.Substring(0, lsListaDocumentosOri.Length - 1)
                  Dim vecDocs As String() = lsListaDocumentos.Split("|")
                  Dim vecDocsOri As String() = lsListaDocumentosOri.Split("|")

                  If vecDocs.Length = vecDocsOri.Length Then
                     For i As Integer = 0 To vecDocs.Length - 1
                        objDoctoCargado = New Visita.DocumentoCargado
                        objDoctoCargado.Nombre_SP = vecDocs(i)
                        objDoctoCargado.Nombre_Original = vecDocsOri(i)
                        lstDocumentosString.Add(objDoctoCargado)
                     Next
                  End If

                  visita.UltimosDocumentos = lstDocumentosString
               Else
                  visita.UltimosDocumentos = New List(Of Visita.DocumentoCargado)
               End If
            Else
               visita.UltimosDocumentos = New List(Of Visita.DocumentoCargado)
            End If

            If Not dataReader.IsDBNull(65) Then
               visita.Fecha_ImpSancion = dataReader.GetDateTime(65)
            Else
               visita.Fecha_ImpSancion = Date.MinValue
            End If

            If Not dataReader.IsDBNull(66) Then
               visita.MontoImpSan = dataReader.GetDecimal(66)
            Else
               visita.MontoImpSan = 0.0
            End If

            If Not dataReader.IsDBNull(67) Then
               visita.ComentariosImpSan = dataReader.GetString(67)
            Else
               visita.ComentariosImpSan = ""
            End If

            If Not dataReader.IsDBNull(68) Then
               visita.Fecha_AcuerdoVul = dataReader.GetDateTime(68)
            Else
               visita.Fecha_AcuerdoVul = Date.MinValue
            End If

            If Not dataReader.IsDBNull(69) Then
               visita.Fecha_ReunionVoPaso25 = dataReader.GetDateTime(69)
            Else
               visita.Fecha_ReunionVoPaso25 = Date.MinValue
            End If

            If Not dataReader.IsDBNull(70) Then
               visita.Fecha_ReunionVjPaso32 = dataReader.GetDateTime(70)
            Else
               visita.Fecha_ReunionVjPaso32 = Date.MinValue
            End If

            If Not dataReader.IsDBNull(71) Then
               visita.SolicitoFechaPaso25 = dataReader.GetBoolean(71)
            Else
               visita.SolicitoFechaPaso25 = False
            End If

            If Not dataReader.IsDBNull(72) Then
               visita.SolicitoFechaPaso32 = dataReader.GetBoolean(72)
            Else
               visita.SolicitoFechaPaso32 = False
            End If

            ''Recuperar las subentidades con sus tipos
            Dim objtipoSub As Entities.TipoSubEntidad
            visita.SubEntidadesSeleccionadas = New List(Of Entities.TipoSubEntidad)
            If Not dataReader.IsDBNull(74) Then
               Dim lsCadSubEntidades As String = dataReader.GetString(74)
               If lsCadSubEntidades.Trim.Length > 0 Then ''eliminar la ultima coma
                  Dim vecTipoSub As String() = lsCadSubEntidades.Substring(0, lsCadSubEntidades.Length - 1).Split(",")
                  If vecTipoSub.Length > 0 Then
                     For i = 0 To vecTipoSub.Length - 1
                        Dim vecSubentidad As String() = vecTipoSub(i).Split("|")
                        If vecSubentidad.Length = 2 Then
                           objtipoSub = New Entities.TipoSubEntidad
                           objtipoSub.IdSubEntidad = vecSubentidad(0)
                           objtipoSub.IdTipoEntidad = vecSubentidad(1)
                           visita.SubEntidadesSeleccionadas.Add(objtipoSub)
                        End If
                     Next
                  End If
               End If
            End If

            If Not dataReader.IsDBNull(75) Then
               visita.UsuarioEstaOcupando = dataReader.GetString(75)
               visita.EstaVisitaOcupada = True
            Else
               visita.UsuarioEstaOcupando = ""
               visita.EstaVisitaOcupada = False
            End If

            If Not dataReader.IsDBNull(76) Then
               visita.DiasHabilesTotalesConsumidos = dataReader.GetInt32(76)
            Else
               visita.DiasHabilesTotalesConsumidos = 0
            End If

            If Not dataReader.IsDBNull(77) Then
               visita.Fecha_MaximaLimitePasoActual = dataReader.GetDateTime(77)
            Else
               visita.Fecha_MaximaLimitePasoActual = AccesoBD.ObtenFechaLimiteAtender_Paso(visita.IdVisitaGenerado, visita.IdPasoActual)
            End If

            'RRA AGREGAR USUARIO VISITA CANCELADA
            If Not dataReader.IsDBNull(78) Then
               visita.IdUsuarioCancela = dataReader.GetString(78)
            Else
               visita.IdUsuarioCancela = ""
            End If
            'FIN USUARIO VISITA CANCELADA

            'AGC AGREGAR ORDEN VISITA
            If Not dataReader.IsDBNull(79) Then
               visita.OrdenVisita = dataReader.GetString(79)
            Else
               visita.OrdenVisita = ""
            End If
            'FIN ORDEN VISITA

            If Not dataReader.IsDBNull(80) Then
               visita.ExisteReunionPaso8 = dataReader.GetBoolean(80)
            Else
               visita.ExisteReunionPaso8 = False
            End If

            ' RRA ESTATUS VULNERABILIDAD 
            If Not dataReader.IsDBNull(81) Then
               visita.EstatusVulnerabilidad = dataReader.GetBoolean(81)
            Else
               visita.EstatusVulnerabilidad = False
            End If

            'AGC AGREGAR MOTIVO PRORROGA
            If Not dataReader.IsDBNull(82) Then
               visita.MotivoProrroga = dataReader.GetString(82)
            Else
               visita.MotivoProrroga = ""
            End If
            'FIN MOTIVO PRORROGA

            'AGC AGREGAR DIAS HABILES DESDE PASO 4
            If Not dataReader.IsDBNull(83) Then
               visita.DiasHabilesDesdePaso4 = dataReader.GetInt32(83)
            Else
               visita.DiasHabilesDesdePaso4 = 0
            End If
            'FIN DIAS HABILES DESDE PASO 4

            If Not dataReader.IsDBNull(84) Then
               visita.SolicitoRangoSancion = dataReader.GetBoolean(84)
            Else
               visita.SolicitoRangoSancion = False
            End If

            If Not dataReader.IsDBNull(84) Then
               visita.SolicitoRangoSancion = dataReader.GetBoolean(84)
            Else
               visita.SolicitoRangoSancion = False
            End If

            If Not dataReader.IsDBNull(85) Then
               visita.VisitaSisvig = dataReader.GetBoolean(85)
            Else
               visita.VisitaSisvig = False
            End If

            'OBTIENE LA CADENA DE SUBVISITAS CON RANGOS
            If Not dataReader.IsDBNull(87) Then
               visita.SubVisitasRangos = dataReader.GetString(87)
            Else
               visita.SubVisitasRangos = ""
            End If

            If Not dataReader.IsDBNull(87) Then
               visita.SubVisitasRangos = dataReader.GetString(87)
            Else
               visita.SubVisitasRangos = ""
            End If

            If Not dataReader.IsDBNull(88) Then
               visita.BanderaCambioNormativa = dataReader.GetBoolean(88)
            Else
               visita.BanderaCambioNormativa = False
            End If

            If Not dataReader.IsDBNull(89) Then
               visita.BanderaYaSeRealizoReunionVulnera = dataReader.GetBoolean(89)
            Else
               visita.BanderaYaSeRealizoReunionVulnera = False
            End If

            If Not dataReader.IsDBNull(90) Then
                visita.FolioSisan = dataReader.GetString(90)
            Else
                visita.FolioSisan = ""
            End If

            'NHM INICIA LOG
            Dim strVisita As String
            strVisita = "visitaID: " + visita.IdVisitaGenerado.ToString() + ", visitaFolio: " + visita.FolioVisita + ", visitaPasoActual: " + visita.IdPasoActual.ToString() + ", visitaEstatusActual: " + visita.IdEstatusActual.ToString() + ", visitaCancelada: " + visita.EsCancelada.ToString()
            Utilerias.ControlErrores.EscribirEvento("DETALLE VISITA: " + strVisita, EventLogEntryType.Information)
            'NHM FIN LOG

         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getDetalleVisita", "")
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      ''Obtiene los objetos de la visita
      If visita.IdVisitaGenerado <> 0 Then
         Dim dt As DataTable = ObtenerObjetoVisita(visita.IdVisitaGenerado)
         Dim objObjetoVisita As Visita.ObjetoVisita
         Dim lstObjVisita As New List(Of Visita.ObjetoVisita)
         For Each lrRow As DataRow In dt.Rows
            objObjetoVisita = New Visita.ObjetoVisita
            If Not IsDBNull(lrRow("N_ID_OBJETO_VISITA")) Then objObjetoVisita.Id = CInt(lrRow("N_ID_OBJETO_VISITA"))
            If Not IsDBNull(lrRow("T_DSC_OBJETO_VISITA")) Then objObjetoVisita.Descripcion = lrRow("T_DSC_OBJETO_VISITA").ToString()
            If Not IsDBNull(lrRow("ID_OBJETO_SISAN")) Then objObjetoVisita.IdSisan = CInt(lrRow("ID_OBJETO_SISAN"))
            lstObjVisita.Add(objObjetoVisita)
         Next

         visita.LstObjetoVisita = lstObjVisita
      End If

      Return visita
   End Function

   Public Shared Function getDatosGrafica(ByVal tipoGrafo As Integer, ByVal FecIni As Date, ByVal FecFin As Date, ByVal idArea As Integer) As Graficas

      Dim grafica As New Graficas()

      Dim con As Conexion.SQLServer = Nothing

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DATOS_GRAFICA")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@TIPO_GRAFICA", tipoGrafo))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@FECHA_INI", FecIni.ToString("yyyyMMdd")))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@FECHA_FIN", FecFin.ToString("yyyyMMdd")))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_AREA", idArea))

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

         While dataReader.Read()
            grafica.VisitasVF = dataReader.GetInt32(0)
            grafica.VisitasVO = dataReader.GetInt32(1)
            grafica.VisitasCGIV = dataReader.GetInt32(2)
            grafica.VisitasPLD = dataReader.GetInt32(3)
         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento("Tipo de Gráfica: [" & tipoGrafo & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Information)
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return grafica

   End Function

   Public Shared Function getDatosGraficaAreaEstatus(ByVal tipoGrafo As Integer, ByVal FecIni As Date, ByVal FecFin As Date, ByVal idArea As Integer) As Graficas

      Dim con As Conexion.SQLServer = Nothing
      Dim dt As DataTable = Nothing
      Dim grafica As New Graficas()

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DATOS_GRAFICA")

         Dim sqlParameter(3) As SqlParameter
         sqlParameter(0) = New SqlParameter("@TIPO_GRAFICA", tipoGrafo)
         sqlParameter(1) = New SqlParameter("@FECHA_INI", FecIni.ToString("yyyyMMdd"))
         sqlParameter(2) = New SqlParameter("@FECHA_FIN", FecFin.ToString("yyyyMMdd"))
         sqlParameter(3) = New SqlParameter("@I_ID_AREA", idArea)

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, sqlParameter)

         While dataReader.Read()

            grafica.TotalVigentes = dataReader.GetInt32(0)
            grafica.TotalConProrroga = dataReader.GetInt32(1)
            grafica.TotalProcEmplaza = dataReader.GetInt32(2)
            grafica.TotalEmplazadas = dataReader.GetInt32(3)
            grafica.TotalProcSan = dataReader.GetInt32(4)
            grafica.TotalSancionadas = dataReader.GetInt32(5)
            grafica.TotalEspAccJurid = dataReader.GetInt32(6)
            grafica.TotalRevoca = dataReader.GetInt32(7)
            grafica.TotalJuicioNul = dataReader.GetInt32(8)
            grafica.TotalCanceladas = dataReader.GetInt32(9)
            grafica.TotalCerradas = dataReader.GetInt32(10)
            grafica.AreaVisita = dataReader.GetString(11)

         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento("Tipo de Gráfica: [" & tipoGrafo & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Information)
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return grafica

   End Function

   Public Shared Function getDatosGraficaTipoVisita(ByVal tipoGrafo As Integer, ByVal FecIni As Date, ByVal FecFin As Date, ByVal idArea As Integer) As Graficas

      'Dim lstGraficas As New List(Of Graficas)
      Dim con As Conexion.SQLServer = Nothing
      'Dim dt As DataTable = Nothing
      Dim grafica As New Graficas()

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DATOS_GRAFICA")

         Dim sqlParameter(3) As SqlParameter
         sqlParameter(0) = New SqlParameter("@TIPO_GRAFICA", tipoGrafo)
         sqlParameter(1) = New SqlParameter("@FECHA_INI", FecIni.ToString("yyyyMMdd"))
         sqlParameter(2) = New SqlParameter("@FECHA_FIN", FecFin.ToString("yyyyMMdd"))
         sqlParameter(3) = New SqlParameter("@I_ID_AREA", idArea)

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, sqlParameter)

         While dataReader.Read()

            grafica.TotalIntegrales = dataReader.GetInt32(0)
            grafica.TotalSeguimientos = dataReader.GetInt32(1)
            grafica.TotalEspeciales = dataReader.GetInt32(2)
            grafica.TotalOrientadas = dataReader.GetInt32(3)
            grafica.TotalOrdinarias = dataReader.GetInt32(4)
            grafica.AreaVisita = dataReader.GetString(5)

            'lstGraficas.Add(grafica)

         End While

         'If Not IsNothing(lstGraficas) And lstGraficas.Count > 0 Then

         '   Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(GetType(Graficas))
         '   dt = New DataTable()
         '   For i As Integer = 0 To properties.Count - 1
         '      Dim [property] As PropertyDescriptor = properties(i)
         '      dt.Columns.Add([property].Name, [property].PropertyType)
         '   Next
         '   Dim values As Object() = New Object(properties.Count - 1) {}
         '   For Each item As Graficas In lstGraficas
         '      For i As Integer = 0 To values.Length - 1
         '         values(i) = properties(i).GetValue(item)
         '      Next
         '      dt.Rows.Add(values)
         '   Next

         'End If

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento("Tipo de Gráfica: [" & tipoGrafo & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Information)
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return grafica

   End Function

   Public Shared Function getDatosGraficaAreaEntidad(ByVal tipoGrafo As Integer, ByVal FecIni As Date, ByVal FecFin As Date, ByVal idArea As Integer) As Graficas

      Dim con As Conexion.SQLServer = Nothing
      Dim dt As DataTable = Nothing
      Dim grafica As New Graficas()

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DATOS_GRAFICA")

         Dim sqlParameter(3) As SqlParameter
         sqlParameter(0) = New SqlParameter("@TIPO_GRAFICA", tipoGrafo)
         sqlParameter(1) = New SqlParameter("@FECHA_INI", FecIni.ToString("yyyyMMdd"))
         sqlParameter(2) = New SqlParameter("@FECHA_FIN", FecFin.ToString("yyyyMMdd"))
         sqlParameter(3) = New SqlParameter("@I_ID_AREA", idArea)

         Dim dataReader As SqlDataReader
         dataReader = con.EjecutarSPConsultaDR(sp, sqlParameter)

         While dataReader.Read()

            grafica.TotalAzteca = dataReader.GetInt32(0)
            grafica.TotalBanamex = dataReader.GetInt32(1)
            grafica.TotalCoppel = dataReader.GetInt32(2)
            grafica.TotalInbursa = dataReader.GetInt32(3)
            grafica.TotalInvercap = dataReader.GetInt32(4)
            grafica.TotalMetlife = dataReader.GetInt32(5)
            grafica.TotalPension = dataReader.GetInt32(6)
            grafica.TotalPrincipal = dataReader.GetInt32(7)
            grafica.TotalProfuturo = dataReader.GetInt32(8)
            grafica.TotalSura = dataReader.GetInt32(9)
            grafica.TotalBanorte = dataReader.GetInt32(10)
            grafica.AreaVisita = dataReader.GetString(11)

         End While

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento("Tipo de Gráfica: [" & tipoGrafo & "]" & ex.Message & vbCrLf & ex.ToString, EventLogEntryType.Information)
      Finally

         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return grafica

   End Function
    ''' <summary>
    ''' '''''Metodo para obtener datos de inspectores de vigilancia creado por AMMMM 2009219
    ''' </summary>
    ''' <param name="idFolioVIG"></param>
    ''' <param name="Proceso"></param> 'indica si 1 OPI ; 2 PC
    ''' <returns></returns>
    Public Shared Function getInspectoresAsignadosVIG(ByVal idFolioVIG As Integer, ByVal Proceso As Integer) As List(Of Persona)
        Dim lstInspectoresAsignados As New List(Of Persona)
        Dim datosUsuario As New Dictionary(Of String, String)
        Dim dataReader As SqlDataReader
        Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
        Dim sp As String

        Dim con As Conexion.SQLServer = Nothing

        Try
            If Proceso = 1 Then
                con = New Conexion.SQLServer
                Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
                sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_INSPECTORES_ASIGNADO_VIG")
            Else
                con = New Conexion.SQLServer
                Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
                sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_INSPECTORES_ASIGNADOS_PC")
            End If


            SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_FOLIOVIG", idFolioVIG))

            dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

            While dataReader.Read()
                Dim inspectorAsignado As New InspectorAsignado()
                inspectorAsignado.Id = dataReader.GetString(0)
                inspectorAsignado.Nombre = dataReader.GetString(1)

                If Not IsDBNull(dataReader.Item("T_DSC_MAIL")) Then
                    inspectorAsignado.Correo = dataReader.Item("T_DSC_MAIL").ToString().Trim()
                Else
                    inspectorAsignado.Correo = ""
                End If

                ''Si no se llenan los mails de la base de datos obtener de el active directory
#If Not DEBUG Then
                If inspectorAsignado.Correo = "" Then
                    datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(inspectorAsignado.Id)
                    If datosUsuario.Count > 0 Then
                        If Not IsNothing(datosUsuario.Item("mail")) Then
                            inspectorAsignado.Correo = datosUsuario.Item("mail").ToString()
                        End If
                    End If
                End If
#End If

                lstInspectoresAsignados.Add(inspectorAsignado)
            End While

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getInspectoresAsignados", "")
        Finally

            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return lstInspectoresAsignados

    End Function


    ''' <summary>
    ''' '''''Metodo para obtener datos de inspectores de vigilancia creado por AMMMM 2009219
    ''' </summary>
    ''' <param name="idFolioVIG"></param>
    '''  <param name="Proceso"></param> 'indica si 1 OPI ; 2 PC
    ''' <returns></returns>
    Public Shared Function getSupervisoresAsignadosVIG(ByVal idFolioVIG As Integer, ByVal Proceso As Integer) As List(Of Persona)
        Dim lstSupervisoresAsignados As New List(Of Persona)
        Dim datosUsuario As New Dictionary(Of String, String)
        Dim dataReader As SqlDataReader
        Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
        Dim sp As String
        Dim con As Conexion.SQLServer = Nothing



        Try
            If Proceso = 1 Then
                con = New Conexion.SQLServer
                Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
                sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_SUPERVISORES_ASIGNADOS_VIG")

                SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_FOLIOVIG", idFolioVIG))

            Else
                con = New Conexion.SQLServer
                Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
                sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_SUPERVISORES_ASIGNADOS_PC")

                SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_FOLIOVIG", idFolioVIG))


            End If

            dataReader = con.EjecutarSPConsultaDR(sp, SqlParameters.ToArray)

            While dataReader.Read()
                Dim inspectorAsignado As New InspectorAsignado()
                inspectorAsignado.Id = dataReader.GetString(0)
                inspectorAsignado.Nombre = dataReader.GetString(1)

                If Not IsDBNull(dataReader.Item("T_DSC_MAIL")) Then
                    inspectorAsignado.Correo = dataReader.Item("T_DSC_MAIL").ToString().Trim()
                Else
                    inspectorAsignado.Correo = ""
                End If

                ''Si no se llenan los mails de la base de datos obtener de el active directory
#If Not DEBUG Then
                If inspectorAsignado.Correo = "" Then
                    datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(inspectorAsignado.Id)
                    If datosUsuario.Count > 0 Then
                        If Not IsNothing(datosUsuario.Item("mail")) Then
                            inspectorAsignado.Correo = datosUsuario.Item("mail").ToString()
                        End If
                    End If
                End If
#End If

                lstSupervisoresAsignados.Add(inspectorAsignado)
            End While

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, getSupervisoresAsignados", "")
        Finally

            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return lstSupervisoresAsignados

    End Function
End Class
