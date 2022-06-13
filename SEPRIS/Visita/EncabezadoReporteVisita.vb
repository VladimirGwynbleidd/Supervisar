Public Class EncabezadoReporteVisita

    Private _idVisitaGenerado As Integer
    Private _folioVisita As String
    Private _fechaRegistro As DateTime
    Private _idEntidad As Integer
    Private _dscEntidad As String
    Private _idArea As Integer
    Private _dscArea As String
    Private _idPasoActual As Integer
    Private _dscPasoActual As String
    Private _idEstatusActual As Integer
    Private _dscEstatusActual As String


    Public Property IdVisitaGenerado() As Integer
        Get
            Return _idVisitaGenerado
        End Get
        Set(ByVal value As Integer)
            _idVisitaGenerado = value
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

    Public Property FechaRegistro() As DateTime
        Get
            Return _fechaRegistro
        End Get
        Set(ByVal value As DateTime)
            _fechaRegistro = value
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

    Public Property IdPasoActual() As Integer
        Get
            Return _idPasoActual
        End Get
        Set(ByVal value As Integer)
            _idPasoActual = value
        End Set
    End Property

    Public Property DscPasoActual() As String
        Get
            Return _dscPasoActual
        End Get
        Set(ByVal value As String)
            _dscPasoActual = value
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

    Public Property DscEstatusActual() As String
        Get
            Return _dscEstatusActual
        End Get
        Set(ByVal value As String)
            _dscEstatusActual = value
        End Set
    End Property


End Class
