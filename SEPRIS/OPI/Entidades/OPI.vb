<Serializable>
Public Class OPI
    Private _opiid As Int32
    Public Property Id_OPI() As Int32
        Get
            Return _opiid
        End Get
        Set(ByVal value As Int32)
            _opiid = value
        End Set
    End Property

    Private _tipoEnt_id As Int32
    Public Property Id_Tipo_Entidad() As Int32
        Get
            Return _tipoEnt_id
        End Get
        Set(ByVal value As Int32)
            _tipoEnt_id = value
        End Set
    End Property

    Private _entidadId As Int32
    Public Property Id_Entidad() As Int32
        Get
            Return _entidadId
        End Get
        Set(ByVal value As Int32)
            _entidadId = value
        End Set
    End Property

    Private _subEntidadId As Int32
    Public Property Id_Sub_Entidad() As Int32
        Get
            Return _subEntidadId
        End Get
        Set(ByVal value As Int32)
            _subEntidadId = value
        End Set
    End Property

    Private _fechaPI As DateTime
    Public Property Fecha_PI() As DateTime
        Get
            Return _fechaPI
        End Get
        Set(ByVal value As DateTime)
            _fechaPI = value
        End Set
    End Property

    Private _procesoPI As Int32
    Public Property Id_ProcesoPI() As Int32
        Get
            Return _procesoPI
        End Get
        Set(ByVal value As Int32)
            _procesoPI = value
        End Set
    End Property

    Private _descripcionPI As String
    Public Property Descripcion_PI() As String
        Get
            Return _descripcionPI
        End Get
        Set(ByVal value As String)
            _descripcionPI = value
        End Set
    End Property

    'Private _spervisores As List(Of super
    'Public Property NewProperty() As List(Of super
    '    Get
    '        Return _spervisores
    '    End Get
    '    Set(ByVal value As List(Of super)
    '        _spervisores = value
    '    End Set
    'End Property

    Private _observaciones As String
    Public Property Observaciones_OPI() As String
        Get
            Return _observaciones
        End Get
        Set(ByVal value As String)
            _observaciones = value
        End Set
    End Property
End Class
