Public Class DatosDocumento

    Private _idVisitaGenerado As Integer
    Private _idPaso As Integer
    Private _numDocumento As Integer
    Private _dscNombreArchivo As String
    Private _fechaRegistro As DateTime
    Private _dscComentarios As String
    Private _dscNombreArchivoOri As String
    Private _usuarioCarga As String


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

    Public Property NumDocumento() As Integer
        Get
            Return _numDocumento
        End Get
        Set(ByVal value As Integer)
            _numDocumento = value
        End Set
    End Property

    Public Property DscNombreArchivo() As String
        Get
            Return _dscNombreArchivo
        End Get
        Set(ByVal value As String)
            _dscNombreArchivo = value
        End Set
    End Property

    Public Property DscNombreArchivoOri() As String
        Get
            Return _dscNombreArchivoOri
        End Get
        Set(ByVal value As String)
            _dscNombreArchivoOri = value
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

    Public Property DscComentarios() As String
        Get
            Return _dscComentarios
        End Get
        Set(ByVal value As String)
            _dscComentarios = value
        End Set
    End Property

    Public Property UsuarioCarga() As String
        Get
            Return _usuarioCarga
        End Get
        Set(ByVal value As String)
            _usuarioCarga = value
        End Set
    End Property
End Class

Public Class UsuarioComentario
    Public Property IdUsuario As String
    Public Property NombreCompleto As String
    Public Property ContenidoCom As String
    Public Property FechaRegistrCom As String
    Public Property ListaComentarios As List(Of Comentario)
    Public Property ListaDocumentos As List(Of DatosDocumento)
End Class


Public Class Comentario
    Public Property Contenido As String
    Public Property FechaRegistro As String
End Class