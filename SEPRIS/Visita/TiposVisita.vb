Public Class TiposVisita
    Private _idTipoVisita As Integer
    Private _descripcionTipoVisita As String

    

    Public Property IdTipoVisita() As Integer
        Get
            Return _idTipoVisita
        End Get
        Set(ByVal value As Integer)
            _idTipoVisita = value
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

End Class
