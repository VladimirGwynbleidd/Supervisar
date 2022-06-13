Public Class PasoReporte
    Private _idVisitaGenerado As Integer
    Private _idPaso As Integer
    Private _numDiasTiempoEstimado As Integer
    Private _numDiasTiempoReal As Integer
    Private _lstDocumentos As List(Of DatosDocumento)
    Private _lstComentarios As List(Of DatosDocumento)
    Private _lstUsuarioComentarios As List(Of UsuarioComentario)

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

    Public Property NumDiasTiempoEstimado() As Integer
        Get
            Return _numDiasTiempoEstimado
        End Get
        Set(ByVal value As Integer)
            _numDiasTiempoEstimado = value
        End Set
    End Property

    Public Property NumDiasTiempoReal() As Integer
        Get
            Return _numDiasTiempoReal
        End Get
        Set(ByVal value As Integer)
            _numDiasTiempoReal = value
        End Set
    End Property

    Public Property LstDocumentos() As List(Of DatosDocumento)
        Get
            Return _lstDocumentos
        End Get
        Set(ByVal value As List(Of DatosDocumento))
            _lstDocumentos = value
        End Set
    End Property


    Public Property LstComentarios() As List(Of DatosDocumento)
        Get
            Return _lstComentarios
        End Get
        Set(ByVal value As List(Of DatosDocumento))
            _lstComentarios = value
        End Set
    End Property


    Public Property LstUsuarioComentarios() As List(Of UsuarioComentario)
        Get
            Return _lstUsuarioComentarios
        End Get
        Set(ByVal value As List(Of UsuarioComentario))
            _lstUsuarioComentarios = value
        End Set
    End Property

End Class
