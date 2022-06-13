Public Class PasoProcesoVisita
    Private _idVisitaGenerado As Integer
    Private _idPaso As Integer
    Private _fechaInicio As DateTime
    Private _fechaFin As DateTime
    Private _esNotificado As Boolean
    Private _idAreaNotificada As Integer
    Private _idUsuarioNotificado As String
    Private _emailUsuarioNotificado As String
    Private _tieneProrroga As Boolean
    Private _fechaNotifica As DateTime
    Private _diasTranscurridos As Integer
    Private _diasEstimadosPaso As Integer
    Private _subVisitasSeleccionadas As String
    Private _idPasoCancelo As Integer
    Private _idMovimiento As Integer

    Public Property DiasTranscurridos() As Integer
        Get
            Return _diasTranscurridos
        End Get
        Set(ByVal value As Integer)
            _diasTranscurridos = value
        End Set
    End Property

    Public Property DiasEstimadosPaso() As Integer
        Get
            Return _diasEstimadosPaso
        End Get
        Set(ByVal value As Integer)
            _diasEstimadosPaso = value
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

    Public Property IdPaso() As Integer
        Get
            Return _idPaso
        End Get
        Set(ByVal value As Integer)
            _idPaso = value
        End Set
    End Property

    Public Property FechaInicio() As DateTime
        Get
            Return _fechaInicio
        End Get
        Set(ByVal value As DateTime)
            _fechaInicio = value
        End Set
    End Property

    Public Property FechaFin() As DateTime
        Get
            Return _fechaFin
        End Get
        Set(ByVal value As DateTime)
            _fechaFin = value
        End Set
    End Property

    Public Property EsNotificado() As Boolean
        Get
            Return _esNotificado
        End Get
        Set(ByVal value As Boolean)
            _esNotificado = value
        End Set
    End Property

    Public Property IdAreaNotificada() As Integer
        Get
            Return _idAreaNotificada
        End Get
        Set(ByVal value As Integer)
            _idAreaNotificada = value
        End Set
    End Property

    Public Property IdUsuarioNotificado() As String
        Get
            Return _idUsuarioNotificado
        End Get
        Set(ByVal value As String)
            _idUsuarioNotificado = value
        End Set
    End Property

    Public Property EmailUsuarioNotificado() As String
        Get
            Return _emailUsuarioNotificado
        End Get
        Set(ByVal value As String)
            _emailUsuarioNotificado = value
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

    Public Property FechaNotifica() As DateTime
        Get
            Return _fechaNotifica
        End Get
        Set(ByVal value As DateTime)
            _fechaNotifica = value
        End Set
    End Property


    Public Property SubVisitasSeleccionadas() As String
        Get
            Return _subVisitasSeleccionadas
        End Get
        Set(ByVal value As String)
            _subVisitasSeleccionadas = value
        End Set
    End Property

    Public Property IdPasoCancelo() As Integer
        Get
            Return _idPasoCancelo
        End Get
        Set(ByVal value As Integer)
            _idPasoCancelo = value
        End Set
    End Property
    Public Property IdMovimiento() As Integer
        Get
            Return _idMovimiento
        End Get
        Set(value As Integer)
            _idMovimiento = value
        End Set
    End Property

    ''' <summary>
    ''' Lista de pasos en sepris
    ''' </summary>
    ''' <remarks></remarks>
    Enum Pasos
        Cero = 0
        Uno = 1
        Dos = 2
        Tres = 3
        Cuatro = 4
        Cinco = 5
        Seis = 6
        Siete = 7
        Ocho = 8
        Nueve = 9
        Diez = 10
        Once = 11
        Doce = 12
        Trese = 13
        Catorce = 14
        Quince = 15
        Diesiseis = 16
        Diesisiete = 17
        Diesiocho = 18
        Veintiseis = 26
        TreintaYTres = 33
    End Enum

End Class
