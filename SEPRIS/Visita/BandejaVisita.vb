Public Class BandejaVisita
    Private _idVisitaGenerado As Integer
    Private _folioVisita As String
    Private _fechaInicioVisita As DateTime
    Private _strFechaInicioVisita As String
    Private _fechaInicioFormatoEu As String
    Private _idEntidad As Integer
    Private _dscEntidad As String
    Private _idArea As Integer
    Private _dscArea As String
    Private _idTipoVisita As Integer
    Private _dscTipoVisita As String
    Private _idPasoActual As Integer
    Private _idEstatusActual As Integer
    Private _strIdPasoActual As String
    Private _fechaIniciaPaso As DateTime
    Private _fechaFinPaso As DateTime
    Private _diasTranscurridos As String
    Private _diasAcumCSPro As Integer
    Private _idInspectorResponsable As String
    Private _nombreInspectorResponsable As String
    Private _idAbogadoSancion As String
    Private _nombreAbogadoSancion As String
    Private _idAbogadoAsesor As String
    Private _nombreAbogadoAsesor As String
    Private _idAbogadoContencioso As String
    Private _nombreAbogadoContencioso As String
    Private _idObjetoVisita As Integer
    Private _dscObjetoVisita As String

    Private _idEstatusSemaforo As Integer
    Private _idTieneSubvisitas As Integer

    Private _idSubEntidad As Integer
    Private _dscSubEntidad As String

    Private _fechaRegistroD As DateTime
    Private _fechaRegistro As String
    Private _fechaRegistroFormateada As String

    Private _duracionMinimaPasoActual As Integer
    Private _duracionMaximaPasoActual As Integer
    Private _diasTranscurridosPasoActual As Integer

    Private _EstatusVisita As String
    Private _EstatusVisitaInt As String
    Private _OrdenVisita As String
    Private _diasHabilesTotalesVisita As Integer
    Private _diasPlazosLegalesVisita As Integer
    Private _diasPlazosLegalesVisitaDsc As String
    Private _prorrogaAprobada As Boolean
    Private _Folio_SISAN As String

    Public Property IdVisitaGenerado() As Integer
        Get
            Return _idVisitaGenerado
        End Get
        Set(ByVal value As Integer)
            _idVisitaGenerado = value
        End Set
    End Property

    Public Property DuracionMinimaPasoActual() As Integer
        Get
            Return _duracionMinimaPasoActual
        End Get
        Set(ByVal value As Integer)
            _duracionMinimaPasoActual = value
        End Set
    End Property

    Public Property DuracionMaximaPasoActual() As Integer
        Get
            Return _duracionMaximaPasoActual
        End Get
        Set(ByVal value As Integer)
            _duracionMaximaPasoActual = value
        End Set
    End Property

    Public Property DiasPasoActualTranscurridos() As Integer
        Get
            Return _diasTranscurridosPasoActual
        End Get
        Set(ByVal value As Integer)
            _diasTranscurridosPasoActual = value
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

    Public Property FechaInicioVisita() As DateTime
        Get
            Return _fechaInicioVisita
        End Get
        Set(ByVal value As DateTime)
            _fechaInicioVisita = value
        End Set
    End Property

    Public Property StrFechaInicioVisita() As String
        Get
            Return _strFechaInicioVisita
        End Get
        Set(ByVal value As String)
            _strFechaInicioVisita = value
        End Set
    End Property

    Public Property FechaInicioFormatoEu() As String
        Get
            Return _fechaInicioFormatoEu
        End Get
        Set(ByVal value As String)
            _fechaInicioFormatoEu = value
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

    Public Property DscEntidad() As String
        Get
            Return _dscEntidad
        End Get
        Set(ByVal value As String)
            _dscEntidad = value
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

    Public Property DscArea() As String
        Get
            Return _dscArea
        End Get
        Set(ByVal value As String)
            _dscArea = value
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

    Public Property DscTipoVisita() As String
        Get
            Return _dscTipoVisita
        End Get
        Set(ByVal value As String)
            _dscTipoVisita = value
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

    Public Property StrIdPasoActual() As String
        Get
            Return _strIdPasoActual
        End Get
        Set(ByVal value As String)
            _strIdPasoActual = value
        End Set
    End Property

    Public Property FechaIniciaPaso() As DateTime
        Get
            Return _fechaIniciaPaso
        End Get
        Set(ByVal value As DateTime)
            _fechaIniciaPaso = value
        End Set
    End Property

    Public Property FechaFinPaso() As DateTime
        Get
            Return _fechaFinPaso
        End Get
        Set(ByVal value As DateTime)
            _fechaFinPaso = value
        End Set
    End Property

    Public Property FechaRegistroD() As DateTime
        Get
            Return _fechaRegistroD
        End Get
        Set(ByVal value As DateTime)
            _fechaRegistroD = value
        End Set
    End Property

    Public Property DiasTranscurridos() As String
        Get
            Return _diasTranscurridos
        End Get
        Set(ByVal value As String)
            _diasTranscurridos = value
        End Set
    End Property

    Public Property DiasAcumCSPro() As Integer
        Get
            Return _diasAcumCSPro
        End Get
        Set(ByVal value As Integer)
            _diasAcumCSPro = value
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

    Public Property NombreAbogadoSancion() As String
        Get
            Return _nombreAbogadoSancion
        End Get
        Set(ByVal value As String)
            _nombreAbogadoSancion = value
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

    Public Property NombreAbogadoAsesor() As String
        Get
            Return _nombreAbogadoAsesor
        End Get
        Set(ByVal value As String)
            _nombreAbogadoAsesor = value
        End Set
    End Property

    Public Property IdAbogadoContencioso() As String
        Get
            Return _idAbogadoContencioso
        End Get
        Set(ByVal value As String)
            _idAbogadoContencioso = value
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

    Public Property IdObjetoVisita() As Integer
        Get
            Return _idObjetoVisita
        End Get
        Set(ByVal value As Integer)
            _idObjetoVisita = value
        End Set
    End Property

    Public Property DscObjetoVisita() As String
        Get
            Return _dscObjetoVisita
        End Get
        Set(ByVal value As String)
            _dscObjetoVisita = value
        End Set
    End Property

    Public Property IdEstatusSemaforo() As Integer
        Get
            Return _idEstatusSemaforo
        End Get
        Set(ByVal value As Integer)
            _idEstatusSemaforo = value
        End Set
    End Property

    Public Property TieneSubvisitas As Integer
        Get
            Return _idTieneSubvisitas
        End Get
        Set(value As Integer)
            _idTieneSubvisitas = value
        End Set
    End Property

    Public Property IdSubEntidad As Integer
        Get
            Return _idSubEntidad
        End Get
        Set(value As Integer)
            _idSubEntidad = value
        End Set
    End Property

    Public Property DscSubEntidad() As String
        Get
            Return _dscSubEntidad
        End Get
        Set(ByVal value As String)
            _dscSubEntidad = value
        End Set
    End Property


    Public Property FechaRegistro As String
        Get
            Return _fechaRegistro
        End Get
        Set(value As String)
            _fechaRegistro = value
        End Set
    End Property

    Public Property FechaRegistroFormateada As String
        Get
            Return _fechaRegistroFormateada
        End Get
        Set(value As String)
            _fechaRegistroFormateada = value
        End Set
    End Property

    Public Property EstatusVisitaDsc As String
        Get
            Return _EstatusVisita
        End Get
        Set(value As String)
            _EstatusVisita = value
        End Set
    End Property

    Public Property EstatusVisitaInt As String
        Get
            Return _EstatusVisitaInt
        End Get
        Set(value As String)
            _EstatusVisitaInt = value
        End Set
    End Property

    Public Property IdEstatusActual As Integer
        Get
            Return _idEstatusActual
        End Get
        Set(value As Integer)
            _idEstatusActual = value
        End Set
    End Property

    Public Property OrdenVisita As String
        Get
            Return _OrdenVisita
        End Get
        Set(value As String)
            _OrdenVisita = value
        End Set
    End Property

    Public Property DiasHabilesTotalesVisita As Integer
        Get
            Return _diasHabilesTotalesVisita
        End Get
        Set(value As Integer)
            _diasHabilesTotalesVisita = value
        End Set
    End Property

    Public Property DiasPlazosLegalesVisita As Integer
        Get
            Return _diasPlazosLegalesVisita
        End Get
        Set(value As Integer)
            _diasPlazosLegalesVisita = value
        End Set
    End Property

    Public Property DiasPlazosLegalesVisitaDsc As String
        Get
            Return _diasPlazosLegalesVisitaDsc
        End Get
        Set(value As String)
            _diasPlazosLegalesVisitaDsc = value
        End Set
    End Property

    Public Property ProrrogaAprobada As Boolean
        Get
            Return _prorrogaAprobada
        End Get
        Set(value As Boolean)
            _prorrogaAprobada = value
        End Set
    End Property

    Public Property Folio_SISAN As String
        Get
            Return _Folio_SISAN
        End Get
        Set(value As String)
            _Folio_SISAN = value
        End Set
    End Property
End Class
