<Serializable>
Public Class Persona
    Private _Id As String
    Private _Nombre As String
    Private _Correo As String
    Private _Perfil As Integer
    Private _SubPerfil As Integer

    Public Property Id As String
        Get
            Return _Id
        End Get
        Set(value As String)
            _Id = value
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set(value As String)
            _Nombre = value
        End Set
    End Property

    Public Property Correo As String
        Get
            Return _Correo
        End Get
        Set(value As String)
            _Correo = value
        End Set
    End Property

    Public Property Perfil As Integer
        Get
            Return _Perfil
        End Get
        Set(value As Integer)
            _Perfil = value
        End Set
    End Property

    Public Property SubPerfil As Integer
        Get
            Return _SubPerfil
        End Get
        Set(value As Integer)
            _SubPerfil = value
        End Set
    End Property
End Class

Public Class InspectorAsignado
    Inherits Persona
End Class


Public Class SupervisorAsignado
    Inherits Persona
End Class

Public Class Abogado
    Inherits Persona
End Class
