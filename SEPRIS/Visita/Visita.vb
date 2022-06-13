<Serializable>
Public Class Visita
   Private _idVisitaGenerado As Integer
   Private _idVisitaSisvig As Integer
   Private _idVisitaSepris As Integer
   Private _folioVisita As String
   Private _idArea As Integer
   Private _usuario As Entities.Usuario
   Private _fechaRegistro As DateTime
   Private _fechaInicioVisita As DateTime
   Private _idTipoEntidad As Integer
   Private _dscTipoEntidad As String
   Private _idEntidad As Integer
   Private _idSubentidad As Integer
   Private _dscSubentidad As String
   Private _idTipoVisita As Integer
   Private _idObjetoVisita As Integer
   Private _dscObjetoVisitaOtro As String
   Private _idInspectorResponsable As String
   Private _nombreInspectorResponsable As String
   Private _idAbogadoSancion As String
   Private _nombreAbogadoSancion As String
   Private _idAbogadoAsesor As String
   Private _nombreAbogadoAsesor As String
   Private _idAbogadoConstencioso As String
   Private _nombreAbogadoContencioso As String
   Private _idAbogadoSupervisor As String
   Private _nombreAbogadoSupervisor As String

   Private _banderaCambioNormativa As Boolean

   Private _banderaYaSeRealizoReunionVulnera As Boolean

   Private _idEstatusActual As Integer
   Private _idPasoActual As Integer
   Private _lstInspectoresAsignados As List(Of InspectorAsignado)
   Private _lstSupervisoresAsignados As List(Of SupervisorAsignado)
   Private _lstAbogadosSupAsesorAsig As List(Of Abogado)
   Private _lstAbogadosAsesorAsignados As List(Of Abogado)
   Private _lstAbogadosSupSancionAsig As List(Of Abogado)
   Private _lstAbogadosSancionAsignados As List(Of Abogado)
   Private _lstAbogadosSupContenAsig As List(Of Abogado)
   Private _lstAbogadosContenAsignados As List(Of Abogado)

   Private _nombreEntidad As String
   Private _esCancelada As Boolean
   Private _motivoCancelacion As String
   Private _descripcionTipoVisita As String
   Private _fechaCancela As DateTime
   Private _FechaReunionPresidencia As DateTime?
   Private _FechaReunionAfore As DateTime?
   Private _descripcionVisita As String
   Private _comentariosIniciales As String
   Private _pbEsPasoReactivado As Boolean
   Private _idVisitaPadreSubvisita As Integer
   Private _idVisitaPadreCopia As Integer
   Private _comentariosPasoEstatus As String
   Private _folioSisan As String

   Private __FECH_VISITA_CAMPO_INI As Date
   Private __FECH_VISITA_CAMPO_FIN As Date
   Private __FECH_REUNION_PRESIDENCIA As Date
   Private _RANGO_SANCION_INI As Decimal
   Private _RANGO_SANCION_FIN As Decimal
   Private _COMENTARIO_RANGO_SANCION As String
   Private __FECH_REUNION_AFORE As Date
   Private __fechaLimitePasoActual As Date
   Private __FechaMaximaLimitePasoActual As Date
   Private _fechaInicioPasoActual As DateTime?
   Private _diasTranscurridosPasoActual As Integer
   Private _esSubVisitaOsubFolio As Boolean
   Private _esSubVisita As Boolean
   Private _tieneProrroga As Boolean
   Private _tieneProrrogaAprobada As Boolean
   Private _tieneSancion As Boolean
   Private _descripcionPasoActual As String

   Private _ultimoUsuarioComentario As String
   Private _ultimoComentario As String
   Private _ultimoUsuarioDocumento As String
   Private _ultimosDocumentos As List(Of DocumentoCargado)

   Private _lstSubVisitas As List(Of SubVisitas)
   Private _lstSubVisitasSeleccionadas As List(Of SubVisitas)

   Public _lstObjetoVisita As List(Of ObjetoVisita)
   Private _tieneSubVisitas As Boolean
   Private _subVisitasSeleccionadas As String
   Private _foliosSubVisitasSeleccionadas As String

   Private __FechaReunionVjPaso9 As Date
   Private __FechaReunionVjPaso16 As Date

   Private _solicitoFechaPaso9 As Boolean
   Private _solicitoFechaPaso16 As Boolean

   Private __FechaInSituActaCircunstanciada As Date
   Private __FechaLevantamientoActaConclusion As Date

   Private __FechaImpSancion As Date
   Private _MontoImpSan As Decimal
   Private _ComentariosImpSan As String

   Private __FechaAntAcuerdoVul As Date
   Private __FechaAcuerdoVul As Date

   Private __FechaReunionVoPaso25 As Date
   Private __FechaReunionVjPaso32 As Date

   Private _solicitoFechaPaso25 As Boolean
   Private _solicitoFechaPaso32 As Boolean

   Private _SubEntidadesSeleccionadas As List(Of Entities.TipoSubEntidad)
   Private _UsuarioEstaOcupando As String
   Private _EstaVisitaOcupada As Boolean
   Private _DiasHabilesTotalesConsumidos As Integer
   Public CambioDePaso As Boolean = False
   'RRA Agrego Propiedad Visita para USUARIO CANCELA y VULNERABILIDAD

   Private _IdUsuarioCancela As String
   Private _OrdenVisita As String
   Private _ExisteReunionPaso8 As Boolean
   Private _EstatusVulnerabilidad As Boolean

   Private _documentoRevisionPasoSeis As String
   Private _motivoProrroga As String
   Private _diasHabilesDesdePaso4 As Int32

   Private _ExisteVisita As Boolean

   Private _SolicitoRangoSancion As Boolean
   Private _VisitaSisvig As Boolean
   Private _SubVisitasRangos As String
   
   Private _comentariosAprobacionDocumentos As String

   Public Property IdUsuarioCancela As String
      Get
         Return _IdUsuarioCancela
      End Get
      Set(value As String)
         _IdUsuarioCancela = value
      End Set
   End Property
   'FIN CAMBIO USUARIO CANCELA

   Public Property OrdenVisita As String
      Get
         Return _OrdenVisita
      End Get
      Set(value As String)
         _OrdenVisita = value
      End Set
   End Property

   Public Property LstSubVisitas As List(Of SubVisitas)
      Get
         If IsNothing(_lstSubVisitas) Then
            Return New List(Of SubVisitas)
         Else
            Return _lstSubVisitas
         End If
      End Get
      Set(value As List(Of SubVisitas))
         _lstSubVisitas = value
      End Set
   End Property

   Public Property LstSubVisitasSeleccionadas As List(Of SubVisitas)
      Get
         If IsNothing(_lstSubVisitasSeleccionadas) Then
            Return New List(Of SubVisitas)
         Else
            Return _lstSubVisitasSeleccionadas
         End If
      End Get
      Set(value As List(Of SubVisitas))
         _lstSubVisitasSeleccionadas = value
      End Set
   End Property

   Public Property UltimosDocumentos As List(Of Visita.DocumentoCargado)
      Get
         If IsNothing(_ultimosDocumentos) Then
            Return New List(Of DocumentoCargado)
         Else
            Return _ultimosDocumentos
         End If
      End Get
      Set(value As List(Of Visita.DocumentoCargado))
         _ultimosDocumentos = value
      End Set
   End Property

   Public Property UltimoUsuarioDocumento() As String
      Get
         Return _ultimoUsuarioDocumento
      End Get
      Set(ByVal value As String)
         _ultimoUsuarioDocumento = value
      End Set
   End Property

   Public Property UltimoComentario() As String
      Get
         Return _ultimoComentario
      End Get
      Set(ByVal value As String)
         _ultimoComentario = value
      End Set
   End Property

   Public Property UltimoUsuarioComentario() As String
      Get
         Return _ultimoUsuarioComentario
      End Get
      Set(ByVal value As String)
         _ultimoUsuarioComentario = value
      End Set
   End Property

   Public Property DescripcionVisita() As String
      Get
         Return _descripcionVisita
      End Get
      Set(ByVal value As String)
         _descripcionVisita = value
      End Set
   End Property

   Public Property DescripcionPasoActual() As String
      Get
         Return _descripcionPasoActual
      End Get
      Set(ByVal value As String)
         _descripcionPasoActual = value
      End Set
   End Property

   Public Property ComentariosIniciales() As String
      Get
         Return _comentariosIniciales
      End Get
      Set(ByVal value As String)
         _comentariosIniciales = value
      End Set
   End Property
   Public Property ComentariosPasoEstatus() As String
      Get
         Return _comentariosPasoEstatus
      End Get
      Set(ByVal value As String)
         _comentariosPasoEstatus = value
      End Set
   End Property
   Public Property FolioSisan() As String
        Get
            Return _folioSisan
        End Get
        Set(ByVal value As String)
            _folioSisan = value
        End Set
   End Property

   Public Property IdVisitaGenerado() As Integer
      Get
         Return _idVisitaGenerado
      End Get
      Set(ByVal value As Integer)
         _idVisitaGenerado = value
      End Set
   End Property

   Public Property IdVisitaSisvig() As Integer
      Get
         Return _idVisitaSisvig
      End Get
      Set(ByVal value As Integer)
         _idVisitaSisvig = value
      End Set
   End Property

   Public Property IdVisitaSepris() As Integer
      Get
         Return _idVisitaSepris
      End Get
      Set(ByVal value As Integer)
         _idVisitaSepris = value
      End Set
   End Property

   Public Property DiasTranscurridosPasoActual() As Integer
      Get
         Return _diasTranscurridosPasoActual
      End Get
      Set(ByVal value As Integer)
         _diasTranscurridosPasoActual = value
      End Set
   End Property

   Public Property IdVisitaPadreCopia() As Integer
      Get
         Return _idVisitaPadreCopia
      End Get
      Set(ByVal value As Integer)
         _idVisitaPadreCopia = value
      End Set
   End Property
   Public Property IdVisitaPadreSubvisita() As Integer
      Get
         Return _idVisitaPadreSubvisita
      End Get
      Set(ByVal value As Integer)
         _idVisitaPadreSubvisita = value
      End Set
   End Property

   Public Property FolioVisita() As String
      Get
         Return _folioVisita
      End Get
      Set(ByVal value As String)
         _folioVisita = value
      End Set
   End Property

   Public Property IdArea() As Integer
      Get
         Return _idArea
      End Get
      Set(ByVal value As Integer)
         _idArea = value
      End Set
   End Property

   Public Property Usuario() As Entities.Usuario
      Get
         Return _usuario
      End Get
      Set(ByVal value As Entities.Usuario)
         _usuario = value
      End Set
   End Property

   Public Property FechaRegistro() As DateTime
      Get
         Return _fechaRegistro
      End Get
      Set(ByVal value As DateTime)
         _fechaRegistro = value
      End Set
   End Property

   Public Property FechaInicioVisita() As DateTime
      Get
         Return _fechaInicioVisita
      End Get
      Set(ByVal value As DateTime)
         _fechaInicioVisita = value
      End Set
   End Property
   Public Property IdTipoEntidad() As Integer
      Get
         Return _idTipoEntidad
      End Get
      Set(ByVal value As Integer)
         _idTipoEntidad = value
      End Set
   End Property
   Public Property IdEntidad() As Integer
      Get
         Return _idEntidad
      End Get
      Set(ByVal value As Integer)
         _idEntidad = value
      End Set
   End Property
   Public Property IdSubentidad() As Integer
      Get
         Return _idSubentidad
      End Get
      Set(ByVal value As Integer)
         _idSubentidad = value
      End Set
   End Property

   Public Property IdTipoVisita() As Integer
      Get
         Return _idTipoVisita
      End Get
      Set(ByVal value As Integer)
         _idTipoVisita = value
      End Set
   End Property

   Public Property IdObjetoVisita() As Integer
      Get
         Return _idObjetoVisita
      End Get
      Set(ByVal value As Integer)
         _idObjetoVisita = value
      End Set
   End Property

   Public Property DscObjetoVisitaOtro() As String
      Get
         Return _dscObjetoVisitaOtro
      End Get
      Set(ByVal value As String)
         _dscObjetoVisitaOtro = value
      End Set
   End Property

   Public Property IdInspectorResponsable() As String
      Get
         Return _idInspectorResponsable
      End Get
      Set(ByVal value As String)
         _idInspectorResponsable = value
      End Set
   End Property

   Public Property NombreInspectorResponsable() As String
      Get
         Return _nombreInspectorResponsable
      End Get
      Set(ByVal value As String)
         _nombreInspectorResponsable = value
      End Set
   End Property

   Public Property IdAbogadoSancion() As String
      Get
         Return _idAbogadoSancion
      End Get
      Set(ByVal value As String)
         _idAbogadoSancion = value
      End Set
   End Property

   Public Property IdAbogadoAsesor() As String
      Get
         Return _idAbogadoAsesor
      End Get
      Set(ByVal value As String)
         _idAbogadoAsesor = value
      End Set
   End Property


   Public Property NombreAbogadoSancion() As String
      Get
         Return _nombreAbogadoSancion
      End Get
      Set(ByVal value As String)
         _nombreAbogadoSancion = value
      End Set
   End Property

   Public Property NombreAbogadoAsesor() As String
      Get
         Return _nombreAbogadoAsesor
      End Get
      Set(ByVal value As String)
         _nombreAbogadoAsesor = value
      End Set
   End Property

   Public Property IdEstatusActual() As Integer
      Get
         Return _idEstatusActual
      End Get
      Set(ByVal value As Integer)
         _idEstatusActual = value
      End Set
   End Property

   Public Property IdPasoActual() As Integer
      Get
         Return _idPasoActual
      End Get
      Set(ByVal value As Integer)
         _idPasoActual = value
      End Set
   End Property

   Public Property LstInspectoresAsignados() As List(Of InspectorAsignado)
      Get
         Return _lstInspectoresAsignados
      End Get
      Set(ByVal value As List(Of InspectorAsignado))
         _lstInspectoresAsignados = value
      End Set
   End Property

   Public Property LstAbogadosSupAsesorAsig As List(Of Abogado)
      Get
         Return _lstAbogadosSupAsesorAsig
      End Get
      Set(value As List(Of Abogado))
         _lstAbogadosSupAsesorAsig = value
      End Set
   End Property

   Public Property LstAbogadosAsesorAsignados As List(Of Abogado)
      Get
         Return _lstAbogadosAsesorAsignados
      End Get
      Set(value As List(Of Abogado))
         _lstAbogadosAsesorAsignados = value
      End Set
   End Property

   Public Property LstAbogadosSupSancionAsig As List(Of Abogado)
      Get
         Return _lstAbogadosSupSancionAsig
      End Get
      Set(value As List(Of Abogado))
         _lstAbogadosSupSancionAsig = value
      End Set
   End Property

   Public Property LstAbogadosSancionAsignados As List(Of Abogado)
      Get
         Return _lstAbogadosSancionAsignados
      End Get
      Set(value As List(Of Abogado))
         _lstAbogadosSancionAsignados = value
      End Set
   End Property

   Public Property LstAbogadosSupContenAsig As List(Of Abogado)
      Get
         Return _lstAbogadosSupContenAsig
      End Get
      Set(value As List(Of Abogado))
         _lstAbogadosSupContenAsig = value
      End Set
   End Property

   Public Property LstAbogadosContenAsignados As List(Of Abogado)
      Get
         Return _lstAbogadosContenAsignados
      End Get
      Set(value As List(Of Abogado))
         _lstAbogadosContenAsignados = value
      End Set
   End Property

   Public Property LstSupervisoresAsignados() As List(Of SupervisorAsignado)
      Get
         Return _lstSupervisoresAsignados
      End Get
      Set(ByVal value As List(Of SupervisorAsignado))
         _lstSupervisoresAsignados = value
      End Set
   End Property

   Public Property NombreEntidad() As String
      Get
         Return _nombreEntidad
      End Get
      Set(ByVal value As String)
         _nombreEntidad = value
      End Set
   End Property

   Public Property DscTipoEntidad() As String
      Get
         Return _dscTipoEntidad
      End Get
      Set(ByVal value As String)
         _dscTipoEntidad = value
      End Set
   End Property

   Public Property DscSubentidad() As String
      Get
         Return _dscSubentidad
      End Get
      Set(ByVal value As String)
         _dscSubentidad = value
      End Set
   End Property

   Public Property EsCancelada() As Boolean
      Get
         Return _esCancelada
      End Get
      Set(ByVal value As Boolean)
         _esCancelada = value
      End Set
   End Property

   Public Property EsPasoReactivado() As Boolean
      Get
         Return _pbEsPasoReactivado
      End Get
      Set(ByVal value As Boolean)
         _pbEsPasoReactivado = value
      End Set
   End Property


   Public Property MotivoCancelacion() As String
      Get
         Return _motivoCancelacion
      End Get
      Set(ByVal value As String)
         _motivoCancelacion = value
      End Set
   End Property

   Public Property DescripcionTipoVisita() As String
      Get
         Return _descripcionTipoVisita
      End Get
      Set(ByVal value As String)
         _descripcionTipoVisita = value
      End Set
   End Property

   Public Property FechaCancela() As DateTime
      Get
         Return _fechaCancela
      End Get
      Set(ByVal value As DateTime)
         _fechaCancela = value
      End Set
   End Property

   Public Property FechaReunionPresidencia() As DateTime?
      Get
         Return _FechaReunionPresidencia
      End Get
      Set(ByVal value As DateTime?)
         _FechaReunionPresidencia = value
      End Set
   End Property

   Public Property FechaReunionAfore() As DateTime?
      Get
         Return _FechaReunionAfore
      End Get
      Set(ByVal value As DateTime?)
         _FechaReunionAfore = value
      End Set
   End Property

   ''Objeto expediente
   Public Expediente As Expediente


   Public Function ObtieneSubentidad(ByVal idTipoEntidad As Integer, ByVal idEntidad As Integer) As DataTable
      Dim dt As New DataTable
      Try
         dt = AccesoBD.consultarSubentidad(idTipoEntidad, idEntidad)
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Visita, CargaVisita", "")
      End Try
      Return dt
   End Function


   Public Function ObtieneSubentidadST(ByVal idEntidad As Integer) As DataTable
      Dim dt As New DataTable
      Try
         dt = AccesoBD.consultarSubentidadSinTipo(idEntidad)
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Visita, CargaVisita", "")
      End Try
      Return dt
   End Function

   Public Property IdAbogadoConstencioso() As String
      Get
         Return _idAbogadoConstencioso
      End Get
      Set(ByVal value As String)
         _idAbogadoConstencioso = value
      End Set
   End Property

   Public Property IdAbogadoSupervisor() As String
      Get
         Return _idAbogadoSupervisor
      End Get
      Set(ByVal value As String)
         _idAbogadoSupervisor = value
      End Set
   End Property



   Public Property NombreAbogadoContencioso() As String
      Get
         Return _nombreAbogadoContencioso
      End Get
      Set(ByVal value As String)
         _nombreAbogadoContencioso = value
      End Set
   End Property

   Public Property BanderaCambioNormativa() As Boolean
      Get
         Return _banderaCambioNormativa
      End Get
      Set(ByVal value As Boolean)
         _banderaCambioNormativa = value
      End Set
   End Property

   Public Property BanderaYaSeRealizoReunionVulnera() As Boolean
      Get
         Return _banderaYaSeRealizoReunionVulnera
      End Get
      Set(ByVal value As Boolean)
         _banderaYaSeRealizoReunionVulnera = value
      End Set
   End Property

   Public Property NombreAbogadoSupervisor() As String
      Get
         Return _nombreAbogadoSupervisor
      End Get
      Set(ByVal value As String)
         _nombreAbogadoSupervisor = value
      End Set
   End Property

   Public Property FECH_VISITA_CAMPO__INI As Date
      Get
         Return __FECH_VISITA_CAMPO_INI
      End Get
      Set(value As Date)
         __FECH_VISITA_CAMPO_INI = value
      End Set
   End Property

   Public Property FECH_VISITA_CAMPO__FIN As Date
      Get
         Return __FECH_VISITA_CAMPO_FIN
      End Get
      Set(value As Date)
         __FECH_VISITA_CAMPO_FIN = value
      End Set
   End Property

   Public Property FECH_REUNION__PRESIDENCIA As Date
      Get
         Return __FECH_REUNION_PRESIDENCIA
      End Get
      Set(value As Date)
         __FECH_REUNION_PRESIDENCIA = value
      End Set
   End Property
   Public Property RANGO_SANCION_INI As Decimal
      Get
         Return _RANGO_SANCION_INI
      End Get
      Set(value As Decimal)
         _RANGO_SANCION_INI = value
      End Set
   End Property
   Public Property RANGO_SANCION_FIN As Decimal
      Get
         Return _RANGO_SANCION_FIN
      End Get
      Set(value As Decimal)
         _RANGO_SANCION_FIN = value
      End Set
   End Property

   Public Property COMENTARIO_RANGO_SANCION As String
      Get
         Return _COMENTARIO_RANGO_SANCION
      End Get
      Set(value As String)
         _COMENTARIO_RANGO_SANCION = value
      End Set
   End Property
   Public Property FECH_REUNION__AFORE As Date
      Get
         Return __FECH_REUNION_AFORE
      End Get
      Set(value As Date)
         __FECH_REUNION_AFORE = value
      End Set
   End Property

   Public Property Fecha_LimitePasoActual As Date
      Get
         Return __fechaLimitePasoActual
      End Get
      Set(value As Date)
         __fechaLimitePasoActual = value
      End Set
   End Property

   Public Property Fecha_MaximaLimitePasoActual As Date
      Get
         Return __FechaMaximaLimitePasoActual
      End Get
      Set(value As Date)
         __FechaMaximaLimitePasoActual = value
      End Set
   End Property

   Public Property FechaInicioPasoActual As DateTime?
      Get
         Return _fechaInicioPasoActual
      End Get
      Set(value As DateTime?)
         _fechaInicioPasoActual = value
      End Set
   End Property

   Public Property EsSubVisitaOsubFolio() As Boolean
      Get
         Return _esSubVisitaOsubFolio
      End Get
      Set(ByVal value As Boolean)
         _esSubVisitaOsubFolio = value
      End Set
   End Property

   Public Property EsSubVisita() As Boolean
      Get
         Return _esSubVisita
      End Get
      Set(ByVal value As Boolean)
         _esSubVisita = value
      End Set
   End Property

   Public Property TieneProrrogaAprobada() As Boolean
      Get
         Return _tieneProrrogaAprobada
      End Get
      Set(ByVal value As Boolean)
         _tieneProrrogaAprobada = value
      End Set
   End Property

   Public Property TieneSancion() As Boolean
      Get
         Return _tieneSancion
      End Get
      Set(ByVal value As Boolean)
         _tieneSancion = value
      End Set
   End Property

   Public Property TieneProrroga() As Boolean
      Get
         Return _tieneProrroga
      End Get
      Set(ByVal value As Boolean)
         _tieneProrroga = value
      End Set
   End Property


   Public Property TieneSubVisitas() As Boolean
      Get
         Return _tieneSubVisitas
      End Get
      Set(ByVal value As Boolean)
         _tieneSubVisitas = value
      End Set
   End Property

   Public Property LstObjetoVisita As List(Of ObjetoVisita)
      Get
         If IsNothing(_lstObjetoVisita) Then
            Return New List(Of ObjetoVisita)
         Else
            Return _lstObjetoVisita
         End If
      End Get
      Set(value As List(Of ObjetoVisita))
         _lstObjetoVisita = value
      End Set
   End Property

   Public Property SubVisitasSeleccionadas As String
      Get
         If IsNothing(_subVisitasSeleccionadas) Then
            Return ""
         Else
            Return _subVisitasSeleccionadas
         End If
      End Get
      Set(value As String)
         _subVisitasSeleccionadas = value
      End Set
   End Property

   Public Property FoliosSubVisitasSeleccionadas As String
      Get
         If IsNothing(_foliosSubVisitasSeleccionadas) Then
            Return ""
         Else
            Return _foliosSubVisitasSeleccionadas
         End If
      End Get
      Set(value As String)
         _foliosSubVisitasSeleccionadas = value
      End Set
   End Property


   Public Property Fecha_ReunionVjPaso9 As Date
      Get
         Return __FechaReunionVjPaso9
      End Get
      Set(value As Date)
         __FechaReunionVjPaso9 = value
      End Set
   End Property

   Public Property Fecha_ReunionVjPaso16 As Date
      Get
         Return __FechaReunionVjPaso16
      End Get
      Set(value As Date)
         __FechaReunionVjPaso16 = value
      End Set
   End Property

   Public Property SolicitoFechaPaso9() As Boolean
      Get
         Return _solicitoFechaPaso9
      End Get
      Set(ByVal value As Boolean)
         _solicitoFechaPaso9 = value
      End Set
   End Property

   Public Property SolicitoFechaPaso16() As Boolean
      Get
         Return _solicitoFechaPaso16
      End Get
      Set(ByVal value As Boolean)
         _solicitoFechaPaso16 = value
      End Set
   End Property

   Public Property Fecha_InSituActaCircunstanciada As Date
      Get
         Return __FechaInSituActaCircunstanciada
      End Get
      Set(value As Date)
         __FechaInSituActaCircunstanciada = value
      End Set
   End Property

   Public Property Fecha_LevantamientoActaConclusion As Date
      Get
         Return __FechaLevantamientoActaConclusion
      End Get
      Set(value As Date)
         __FechaLevantamientoActaConclusion = value
      End Set
   End Property

   Public Property Fecha_ImpSancion As Date
      Get
         Return __FechaImpSancion
      End Get
      Set(value As Date)
         __FechaImpSancion = value
      End Set
   End Property

   Public Property MontoImpSan As Decimal
      Get
         Return _MontoImpSan
      End Get
      Set(value As Decimal)
         _MontoImpSan = value
      End Set
   End Property

   Public Property ComentariosImpSan As String
      Get
         Return _ComentariosImpSan
      End Get
      Set(value As String)
         _ComentariosImpSan = value
      End Set
   End Property
   Public Property Fecha_AcuerdoVul As Date
      Get
         Return __FechaAcuerdoVul
      End Get
      Set(value As Date)
         __FechaAcuerdoVul = value
      End Set
   End Property

   Public Property Fecha_AntAcuerdoVul As Date
      Get
         Return __FechaAntAcuerdoVul
      End Get
      Set(value As Date)
         __FechaAntAcuerdoVul = value
      End Set
   End Property

   Public Property Fecha_ReunionVoPaso25 As Date
      Get
         Return __FechaReunionVoPaso25
      End Get
      Set(value As Date)
         __FechaReunionVoPaso25 = value
      End Set
   End Property

   Public Property Fecha_ReunionVjPaso32 As Date
      Get
         Return __FechaReunionVjPaso32
      End Get
      Set(value As Date)
         __FechaReunionVjPaso32 = value
      End Set
   End Property

   Public Property SolicitoFechaPaso25() As Boolean
      Get
         Return _solicitoFechaPaso25
      End Get
      Set(ByVal value As Boolean)
         _solicitoFechaPaso25 = value
      End Set
   End Property

   Public Property SolicitoFechaPaso32() As Boolean
      Get
         Return _solicitoFechaPaso32
      End Get
      Set(ByVal value As Boolean)
         _solicitoFechaPaso32 = value
      End Set
   End Property

   Public Property SubEntidadesSeleccionadas As List(Of Entities.TipoSubEntidad)
      Get
         Return _SubEntidadesSeleccionadas
      End Get
      Set(value As List(Of Entities.TipoSubEntidad))
         _SubEntidadesSeleccionadas = value
      End Set
   End Property

   Public Property UsuarioEstaOcupando As String
      Get
         Return _UsuarioEstaOcupando
      End Get
      Set(value As String)
         _UsuarioEstaOcupando = value
      End Set
   End Property

   Public Property EstaVisitaOcupada() As Boolean
      Get
         Return _EstaVisitaOcupada
      End Get
      Set(ByVal value As Boolean)
         _EstaVisitaOcupada = value
      End Set
   End Property

   Public Property DiasHabilesTotalesConsumidos() As Integer
      Get
         Return _DiasHabilesTotalesConsumidos
      End Get
      Set(ByVal value As Integer)
         _DiasHabilesTotalesConsumidos = value
      End Set
   End Property


   Public Property ExisteReunionPaso8() As Boolean
      Get
         Return _ExisteReunionPaso8
      End Get
      Set(ByVal value As Boolean)
         _ExisteReunionPaso8 = value
      End Set
   End Property

   Public Property EstatusVulnerabilidad() As Boolean
      Get
         Return _EstatusVulnerabilidad
      End Get
      Set(ByVal value As Boolean)
         _EstatusVulnerabilidad = value
      End Set
   End Property

   Public Property DocumentoRevisionPasoSeis As String
      Get
         Return _documentoRevisionPasoSeis
      End Get
      Set(value As String)
         _documentoRevisionPasoSeis = value
      End Set
   End Property

   Public Property MotivoProrroga As String
      Get
         Return _motivoProrroga
      End Get
      Set(value As String)
         _motivoProrroga = value
      End Set
   End Property

   Public Property DiasHabilesDesdePaso4 As Int32
      Get
         Return _diasHabilesDesdePaso4
      End Get
      Set(value As Int32)
         _diasHabilesDesdePaso4 = value
      End Set
   End Property

   Public Property SolicitoRangoSancion() As Boolean
      Get
         Return _SolicitoRangoSancion
      End Get
      Set(ByVal value As Boolean)
         _SolicitoRangoSancion = value
      End Set
   End Property

   Public Property VisitaSisvig() As Boolean
      Get
         Return _VisitaSisvig
      End Get
      Set(ByVal value As Boolean)
         _VisitaSisvig = value
      End Set
   End Property

   Public Property SubVisitasRangos() As String
      Get
         Return _SubVisitasRangos
      End Get
      Set(ByVal value As String)
         _SubVisitasRangos = value
      End Set
   End Property

   Public Property ExisteVisita() As Boolean
      Get
         Return _ExisteVisita
      End Get
      Set(ByVal value As Boolean)
         _ExisteVisita = value
      End Set
   End Property

   Public Property ComentariosAprobacionDocumentos() As String
      Get
         Return _comentariosAprobacionDocumentos
      End Get
      Set(ByVal value As String)
         _comentariosAprobacionDocumentos = value
      End Set
   End Property

   <Serializable>
   Public Class DocumentoCargado
      Public Property Nombre_SP As String = ""
      Public Property Nombre_Original As String = ""
   End Class

   <Serializable>
   Public Class ObjetoVisita
      Public Property Id As Integer
      Public Property Descripcion As String
      Public Property IdSisan As Integer = -1
   End Class

   <Serializable>
   Public Class SubVisitas
      Public Property Id As Integer
      Public Property Descripcion As String
      Public Property Folio As String
      Public Property EstaSeleccionada As Boolean
   End Class
End Class
