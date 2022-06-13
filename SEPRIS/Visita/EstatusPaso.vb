Public Class EstatusPaso
    Private _idVisitaGenerado As Integer
    Private _idPaso As Integer
    Private _idEstatus As Integer
    Private _fechaRegistro As DateTime
    Private _idUsuario As String
    Private _comentarios As String
    Private _tipoComentarios As String
    Private _esDetalle As Integer
    Private _idAreaActual As Integer
    Private _subVisitasSeleccionadas As String

    Public Property IdVisitaGenerado() As Integer
        Get
            Return _idVisitaGenerado
        End Get
        Set(ByVal value As Integer)
            _idVisitaGenerado = value
        End Set
    End Property

    Public Property EsRegistro() As Integer
        Get
            Return _esDetalle
        End Get
        Set(ByVal value As Integer)
            _esDetalle = value
        End Set
    End Property

    Public Property IdAreaActual() As Integer
        Get
            Return _idAreaActual
        End Get
        Set(ByVal value As Integer)
            _idAreaActual = value
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

    Public Property IdEstatus() As Integer
        Get
            Return _idEstatus
        End Get
        Set(ByVal value As Integer)
            _idEstatus = value
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

    Public Property IdUsuario() As String
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As String)
            _idUsuario = value
        End Set
    End Property

    Public Property Comentarios() As String
        Get
            Return _comentarios
        End Get
        Set(ByVal value As String)
            _comentarios = value
        End Set
    End Property

    Public Property TipoComentario() As String
        Get
            Return _tipoComentarios
        End Get
        Set(ByVal value As String)
            _tipoComentarios = value
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
