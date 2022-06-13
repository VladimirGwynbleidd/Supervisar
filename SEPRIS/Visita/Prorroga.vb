Public Class Prorroga
    Private _idVisitaGenerado As Integer
    Private _idPaso As Integer
    Private _fechaRegistro As DateTime
    Private _numDiasDeProrroga As Integer
    Private _motivoProrroga As String
    Private _fechaFinProrroga As DateTime
    Private _FinalizaProrroga As Integer
    Private _subVisitasSeleccionadas As String

    Public Property IdVisitaGenerado() As Integer
        Get
            Return _idVisitaGenerado
        End Get
        Set(ByVal value As Integer)
            _idVisitaGenerado = value
        End Set
    End Property

    Public Property ApruebaProrroga() As Integer
        Get
            Return _FinalizaProrroga
        End Get
        Set(ByVal value As Integer)
            _FinalizaProrroga = value
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

    Public Property FechaRegistro() As DateTime
        Get
            Return _fechaRegistro
        End Get
        Set(ByVal value As DateTime)
            _fechaRegistro = value
        End Set
    End Property

    Public Property NumDiasDeProrroga() As Integer
        Get
            Return _numDiasDeProrroga
        End Get
        Set(ByVal value As Integer)
            _numDiasDeProrroga = value
        End Set
    End Property

    Public Property MotivoProrroga() As String
        Get
            Return _motivoProrroga
        End Get
        Set(ByVal value As String)
            _motivoProrroga = value
        End Set
    End Property

    Public Property FechaFinProrroga() As DateTime
        Get
            Return _fechaFinProrroga
        End Get
        Set(ByVal value As DateTime)
            _fechaFinProrroga = value
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


End Class
