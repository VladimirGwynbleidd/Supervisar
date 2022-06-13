'- Fecha de creación: 18/04/2017
'- Fecha de modificación:  18/04/2017
'- Nombre del Responsable: JMCS
'- Empresa: INDRA
'- Clase de Sisvig_

Imports System.Data.SqlClient
Imports System.Web.Configuration

<Serializable()> _
Public Class Sisvig

   Public Property IdVisita As Integer
   Public Property NumPaso As Integer
   Public Property idEstatus As Integer
   Public Property ComentariosPasoE As String

   Public Property IdVisitaSisvig As Integer
   Public Property FolioVisita As String
   Public Property IdTipoVisita As Integer
   Public Property FechaInicioVisita As Date
   Public Property DescripcionVisita As String
   Public Property IdInspectorResponsable As Integer
   Public Property NombreInspectorResponsable As String
   Public Property OrdenVisita As Integer

   Public Property comentariosSisan As String

   Private conexionExt As New Conexion.SQLServer

   Public Sub New()

   End Sub
   Public Sub New(ByVal IdVisita As Integer, ByVal NumPaso As Integer, ByVal IdEstatus As Integer, ByVal ComentariosPE As String)
      Me.IdVisita = IdVisita
      Me.NumPaso = NumPaso
      Me.idEstatus = IdEstatus
      Me.ComentariosPasoE = ComentariosPE

   End Sub

#Region "Métodos para ejecutar stored procedures"
   Public Function NotificaSisvig(ByVal IdVisita As Integer, ByVal NumPaso As Integer, ByVal IdEstatus As Integer, ByVal ComentariosPaso As String _
                                , Optional ByVal AjusteSeg As Integer = 0) As Boolean
      Dim resultado As Boolean = False

      Me.IdVisita = IdVisita
      Me.NumPaso = NumPaso
      Me.idEstatus = IdEstatus
      Me.ComentariosPasoE = ComentariosPaso

      Try
         conexionExt = ObtieneCadena()

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_ACTUALIZA_NOTIFICACION_SISVIG")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA", IdVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@PASONUMERO", NumPaso))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@ESTATUSID", IdEstatus))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@COMENTARIOSPASO", ComentariosPasoE))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@AjusteSeg", IIf(AjusteSeg = 0, DBNull.Value, AjusteSeg)))

         resultado = conexionExt.EjecutarSP(sp, SqlParameters.ToArray)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "NotificaSisvig clase Sisvig_", "")
         resultado = False
         Throw New Exception("Ocurrió un error en la función NotificaSisvig de la clase Sisvig_.", ex)
      Finally
         If Not IsNothing(conexionExt) Then
            conexionExt.CerrarConexion()
         End If
      End Try

      Return resultado

   End Function

   Public Function GuardaBitacoraSisvigSepris(ByVal IdVisita As Integer, ByVal Usuario As String, ByVal descOperacion As String, ByVal NumPaso As Integer) As Boolean
      Dim resultado As Boolean = False

      Try
         conexionExt = ObtieneCadena()

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "sppSupervisarBitacoraSet")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@idVisita", IdVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@Ori", 2))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@Usr", Usuario))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@Desc", descOperacion))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@Paso", NumPaso))

         resultado = conexionExt.EjecutarSP(sp, SqlParameters.ToArray)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "GuardaBitacoraSisvigSepris clase Sisvig_", "")
         resultado = False
         Throw New Exception("Ocurrió un error en la función GuardaBitacoraSisvigSepris de la clase Sisvig_.", ex)
      Finally
         If Not IsNothing(conexionExt) Then
            conexionExt.CerrarConexion()
         End If
      End Try

      Return resultado

   End Function

   Public Function ObtieneNombreArchivo(ByVal IdVisita As Integer, ByVal idDoc As Integer, ByVal idExt As Integer, _
                                        ByVal Paso As Integer, ByVal NomArc As String) As String()
      Dim resultado(1) As String
      Dim Dt As DataTable = Nothing
      Try
         conexionExt = ObtieneCadena()

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "sppGetDocumento")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@idVisitaSEPRIS", IdVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@idDoc", idDoc))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@NomSEPRIS", NomArc))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@idPaso", Paso))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@idExt", idExt))

         Dt = conexionExt.EjecutarSPConsultaDT(sp, SqlParameters.ToArray)

         If Dt.Rows.Count > 0 Then
            resultado(0) = Dt.Rows(0)(0).ToString()
            resultado(1) = Dt.Rows(0)(1).ToString()

            If resultado(0).Substring(0, 1) = "-" Then
               Dim i As Integer
               'Dim s As String
               'i = resultado(0).IndexOf(".")
               's = resultado(0).Substring(i)

               'resultado(0) = resultado(0).Replace(s, "").Substring(1)
               'resultado(0) = resultado(0).Substring(0, resultado(0).Length - 15) & s
               resultado(0) = resultado(0).Substring(1)
            End If
         End If

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "ObtieneNombreArchivo clase Sisvig_", "")
         resultado(0) = ""
         resultado(1) = ""
         Throw New Exception("Ocurrió un error en la función ObtieneNombreArchivo de la clase Sisvig_.", ex)
      Finally
         If Not Dt Is Nothing Then
            Dt.Dispose()
            Dt = Nothing
         End If

         If Not IsNothing(conexionExt) Then
            conexionExt.CerrarConexion()
         End If
      End Try

      Return resultado
   End Function

   Public Shared Function ObtieneCadena() As Conexion.SQLServer
      Dim conexionExt As Conexion.SQLServer

      If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
         conexionExt = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSISVIG").ToString())
      Else
         conexionExt = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSISVIG").ToString())
      End If

      Return conexionExt
   End Function

   Public Function ActualizaSepris_Sisvig(ByVal IdVisitaSisvig As Integer, ByVal FolioVisita As String, ByVal IdTipoVisita As Integer, ByVal FechaInicioVisita As Date, ByVal DescripcionVisita As String, ByVal IdInspectorResponsable As String, ByVal NombreInspectorResponsable As String, ByVal OrdenVisita As String) As Boolean
      Dim resultado As Boolean = False

      Me.IdVisitaSisvig = IdVisitaSisvig
      Me.FolioVisita = FolioVisita
      Me.IdTipoVisita = IdTipoVisita
      Me.FechaInicioVisita = FechaInicioVisita
      Me.DescripcionVisita = DescripcionVisita
      Me.IdInspectorResponsable = IdInspectorResponsable
      Me.NombreInspectorResponsable = NombreInspectorResponsable
      Me.OrdenVisita = OrdenVisita

      Try
         conexionExt = ObtieneCadena()

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_ACTUALIZA_ACTUALIZA_SEPRIS_SISVIG")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA_INSPECCION", IdVisitaSisvig))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_FOLIO_VISITA_SUPERV", FolioVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@TIPO_VISITA", IdTipoVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@FECHA_INI_VISITA", FechaInicioVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@DESCRIPCION", DescripcionVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@USER_SUPERVISOR", IdInspectorResponsable))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@NOM_SUPERVISOR", NombreInspectorResponsable))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@NUM_ORDEN_VISITA", OrdenVisita))

         resultado = conexionExt.EjecutarSP(sp, SqlParameters.ToArray)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "ActualizaSepris_Sisvig clase Sisvig", "")
         resultado = False
         Throw New Exception("Ocurrió un error en la función EjecutarSP de la clase Sisvig.", ex)
      Finally
         If Not IsNothing(conexionExt) Then
            conexionExt.CerrarConexion()
         End If
      End Try

      Return resultado

   End Function

   Public Function ActualizaInspectores_Sisvig(ByVal nombInspect As String, ByVal IdVisitaS As Integer) As Boolean
      Dim resultado As Boolean = False

      Me.IdVisitaSisvig = IdVisitaS

      Try
         conexionExt = ObtieneCadena()

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_ACTUALIZA_INSPECTORES_SISVIG")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@CAD_USUARIOS", nombInspect))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA_INSPECCION", IdVisitaS))

         resultado = conexionExt.EjecutarSP(sp, SqlParameters.ToArray)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "ActualizaInspectores_Sisvig clase Sisvig", "")
         resultado = False
         Throw New Exception("Ocurrió un error en la función EjecutarSP de la clase Sisvig.", ex)
      Finally
         If Not IsNothing(conexionExt) Then
            conexionExt.CerrarConexion()
         End If
      End Try

      Return resultado

   End Function

   Public Function ActualizaProcesos_Sisvig(ByVal idsProcesos As String, ByVal IdVisitaS As Integer) As Boolean
      Dim resultado As Boolean = False

      Me.IdVisitaSisvig = IdVisitaS
      Me.comentariosSisan = comentariosSisan

      Try
         conexionExt = ObtieneCadena()

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_ACTUALIZA_PROCESOS_SISVIG")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA_INSPECCION", IdVisitaS))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@IDS_PROCESOS", idsProcesos))

         resultado = conexionExt.EjecutarSP(sp, SqlParameters.ToArray)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "ActualizaProcesos_Sisvig clase Sisvig", "")
         resultado = False
         Throw New Exception("Ocurrió un error en la función EjecutarSP de la clase Sisvig.", ex)
      Finally
         If Not IsNothing(conexionExt) Then
            conexionExt.CerrarConexion()
         End If
      End Try

      Return resultado

   End Function

   Public Shared Function ActualizaSepris_Sisvig2(ByVal IdVisitaSisvig As Integer, ByVal idFolioVisita As Integer, ByVal FolioVisita As String) As Boolean
      Dim resultado As Boolean = False
      Dim conexionExt As New Conexion.SQLServer

      Try
         conexionExt = ObtieneCadena()

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_GRL_ACTUALIZA_ACTUALIZA_ID_FOLIO")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_ID_VISITA_INSPECCION", IdVisitaSisvig))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@I_FOLIO_VISITA_SUPERV", idFolioVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@FOLIO_VISITA_SUPERV", FolioVisita))

         resultado = conexionExt.EjecutarSP(sp, SqlParameters.ToArray)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "ActualizaSepris_Sisvig2 clase Sisvig", "")
         resultado = False
         Throw New Exception("Ocurrió un error en la función EjecutarSP de la clase ActualizaSepris_Sisvig2 en Sisvig.", ex)
      Finally
         If Not IsNothing(conexionExt) Then
            conexionExt.CerrarConexion()
         End If
      End Try

      Return resultado

   End Function

   Public Shared Function EstableceSesion(ByVal IdVisita As Integer, ByVal Valor As Boolean) As Boolean
      Dim resultado As Boolean = False
      Dim conexionExt As New Conexion.SQLServer

      Try
         conexionExt = ObtieneCadena()

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "sppVisitaVulnerabilidadesSet")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@idVisita", IdVisita))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@Sesion", Valor))

         resultado = conexionExt.EjecutarSP(sp, SqlParameters.ToArray)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "NotificaSisvig clase Sisvig_", "")
         resultado = False
         Throw New Exception("Ocurrió un error en la función NotificaSisvig de la clase Sisvig_.", ex)
      Finally
         If Not IsNothing(conexionExt) Then
            conexionExt.CerrarConexion()
         End If
      End Try

      Return resultado

   End Function

   Public Shared Sub SetCambioNorma(ByVal IdVisitaSEPRIS As Integer, ByVal Cambio As Boolean)
      Dim conexionExt As New Conexion.SQLServer

      Try
         conexionExt = ObtieneCadena()

         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp = String.Format("{0}[{1}]", Owner, "sppVisitaCambioNormatividad")

         Dim SqlParameters As New List(Of System.Data.SqlClient.SqlParameter)
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@idVisitaSEPRIS", IdVisitaSEPRIS))
         SqlParameters.Add(New System.Data.SqlClient.SqlParameter("@Cambio", Cambio))

         conexionExt.EjecutarSP(sp, SqlParameters.ToArray)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "SetCambioNorma clase Sisvig", "")
         Throw New Exception("Ocurrió un error en la función SetCambioNorma de la clase Sisvig.", ex)
      Finally
         If Not IsNothing(conexionExt) Then
            conexionExt.CerrarConexion()
         End If
      End Try

   End Sub
#End Region

End Class