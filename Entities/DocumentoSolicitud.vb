
Public Class DocumentoSolicitud
    Private idSolicitud As Integer
    Public Property Solicitud() As Integer
        Get
            Return idSolicitud
        End Get
        Set(ByVal value As Integer)
            idSolicitud = value
        End Set
    End Property

    Private idServicio As Integer
    Public Property Servicio() As Integer
        Get
            Return idServicio
        End Get
        Set(ByVal value As Integer)
            idServicio = value
        End Set
    End Property

    Private idDocumento As Integer
    Public Property Documento() As Integer
        Get
            Return idDocumento
        End Get
        Set(ByVal value As Integer)
            idDocumento = value
        End Set
    End Property

    Private srtNombre As String
    Public Property Nombre() As String
        Get
            Return srtNombre
        End Get
        Set(ByVal value As String)
            srtNombre = value
        End Set
    End Property

    Public Sub New(ByRef Nombre As String)
        Me.Nombre = Nombre
        CargaDatos()
    End Sub

    Public Sub CargaDatos()
        Dim query As String = "SELECT * FROM BDS_D_GR_SOLICITUD_SERVICIO_DOCUMENTOS WHERE T_DSC_ARCHIVO_ADJUNTO = '{0}'"
        query = String.Format(query, Me.Nombre)
        Dim conexion As New Conexion.SQLServer
        Dim dt As New DataTable
        Try
            dt = conexion.ConsultarDT(query)
            If dt.Rows.Count > 0 Then
                Me.Nombre = dt.Rows(0)("T_DSC_ARCHIVO_ADJUNTO")
                Me.Solicitud = dt.Rows(0)("N_ID_SOLICITUD_SERVICIO")
                Me.Servicio = dt.Rows(0)("N_ID_NIVELES_SERVICIO")
                Me.Documento = dt.Rows(0)("N_ID_DOCUMENTO")
            End If
        Catch ex As Exception
        Finally
            If Not conexion Is Nothing Then
                If conexion.EstadoConexion Then
                    conexion.CerrarConexion()
                End If
            End If
        End Try
    End Sub

End Class
