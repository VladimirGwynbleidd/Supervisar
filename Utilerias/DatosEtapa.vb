Public Class DatosEtapa

    Private _numEtapa As Integer
    Private _dscEtapa As String
    Private _diasTiempoEstimado As Integer
    Private _diasTiempoReal As Integer
    Private _dscRangoDias As String

    Public Property NumEtapa() As Integer
        Get
            Return _numEtapa
        End Get
        Set(ByVal value As Integer)
            _numEtapa = value
        End Set
    End Property

    Public Property DscEtapa() As String
        Get
            Return _dscEtapa
        End Get
        Set(ByVal value As String)
            _dscEtapa = value
        End Set
    End Property


    Public Property DiasTiempoEstimado() As Integer
        Get
            Return _diasTiempoEstimado
        End Get
        Set(ByVal value As Integer)
            _diasTiempoEstimado = value
        End Set
    End Property

    Public Property DiasTiempoReal() As Integer
        Get
            Return _diasTiempoReal
        End Get
        Set(ByVal value As Integer)
            _diasTiempoReal = value
        End Set
    End Property

    Public Property DscRangoDias() As String
        Get
            Return _dscRangoDias
        End Get
        Set(ByVal value As String)
            _dscRangoDias = value
        End Set
    End Property
End Class
