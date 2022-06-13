Imports System
Imports System.Web
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

'***********************************************************************************************************
' Fecha Creación:       18 Julio 2013
' Codificó:             Jorge Alberto Rangel Ruiz
' Empresa:              Softtek
' Descripción           Clase para objetos, métodos y funciones para el proceso de notificaciones
'***********************************************************************************************************

<Serializable()> _
Public Class Notificaciones

    ''' <summary>
    ''' Enumerador del tipo de estilos definidos
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()>
    Public Enum TipoEstiloNotificacion

        EstilodeTítulo = 0 'titulo
        Estiloletra1 = 1 ' texto 1
        Estiloletra2 = 2 'texto 2
        Estiloletra3 = 3 'texto 3
        estilofondo = 4 'fondo
        Estilohypervinculo = 5 'estilo hypervinculos
        Estilofondo2 = 6 ' etiqueta
        estilobordediv = 7 ' recuadro

    End Enum

#Region "Clases"

    ''' <summary>
    ''' Clase para el elemento CSS de color
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class Color

#Region "Variables de Trabajo"

        Private idIdentificador As Integer
        Private stringDescripcion As String
        Private stringHexadecimal As String

#End Region

#Region "Propiedades"

        Public Property Identificador As Integer
            Get
                Return idIdentificador
            End Get
            Set(ByVal value As Integer)
                idIdentificador = value
            End Set
        End Property

        Public Property Descripcion As String
            Get
                Return stringDescripcion
            End Get
            Set(ByVal value As String)
                stringDescripcion = value
            End Set
        End Property

        Public Property Hexadecimal As String
            Get
                Return stringHexadecimal
            End Get
            Set(ByVal value As String)
                stringHexadecimal = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()

        End Sub

        Public Sub New(ByVal Identificador As Integer, ByVal Descripcion As String, ByVal Hexadecimal As String)

            Me.idIdentificador = Identificador
            Me.stringDescripcion = Descripcion
            Me.stringHexadecimal = Hexadecimal

        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Clase para el elemento CSS de alineacion
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class Alineacion

#Region "Variables de Trabajo"

        Private idIdentificador As Integer
        Private stringDescripcion As String
        Private stringCSS As String

#End Region

#Region "Propiedades"

        Public Property Identificador As Integer
            Get
                Return idIdentificador
            End Get
            Set(ByVal value As Integer)
                idIdentificador = value
            End Set
        End Property

        Public Property Descripcion As String
            Get
                Return stringDescripcion
            End Get
            Set(ByVal value As String)
                stringDescripcion = value
            End Set
        End Property

        Public Property CSS As String
            Get
                Return stringCSS
            End Get
            Set(ByVal value As String)
                stringCSS = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()

        End Sub

        Public Sub New(ByVal Identificador As Integer, ByVal Descripcion As String, ByVal CSS As String)

            Me.idIdentificador = Identificador
            Me.stringDescripcion = Descripcion
            Me.stringCSS = CSS

        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Clase para el elemento CSS del tamaño de fuente
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class TamanioFuente

#Region "Variables de Trabajo"

        Private idIdentificador As Integer
        Private stringDescripcion As String
        Private stringCSS As String

#End Region

#Region "Propiedades"

        Public Property Identificador As Integer
            Get
                Return idIdentificador
            End Get
            Set(ByVal value As Integer)
                idIdentificador = value
            End Set
        End Property

        Public Property Descripcion As String
            Get
                Return stringDescripcion
            End Get
            Set(ByVal value As String)
                stringDescripcion = value
            End Set
        End Property

        Public Property CSS As String
            Get
                Return stringCSS
            End Get
            Set(ByVal value As String)
                stringCSS = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()

        End Sub

        Public Sub New(ByVal Identificador As Integer, ByVal Descripcion As String, ByVal CSS As String)

            Me.idIdentificador = Identificador
            Me.stringDescripcion = Descripcion
            Me.stringCSS = CSS

        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Clase para el elemento CSS de Tipo de fuente
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class TipoFuente

#Region "Variables de Trabajo"

        Private idIdentificador As Integer
        Private stringDescripcion As String

#End Region

#Region "Propiedades"

        Public Property Identificador As Integer
            Get
                Return idIdentificador
            End Get
            Set(ByVal value As Integer)
                idIdentificador = value
            End Set
        End Property

        Public Property Descripcion As String
            Get
                Return stringDescripcion
            End Get
            Set(ByVal value As String)
                stringDescripcion = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()

        End Sub

        Public Sub New(ByVal Identificador As Integer, ByVal Descripcion As String)

            Me.idIdentificador = Identificador
            Me.stringDescripcion = Descripcion

        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Clase para el objeto Estilo de contenido
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class EstiloContenido

#Region "Variables de Trabajo"

        Private idIdentificador As Integer
        Private stringDescripcion As String
        Private stringTag As String
        Private esNegrita As Boolean
        Private esItalica As Boolean
        Private objColor As Notificaciones.Color
        Private objAlineacion As Notificaciones.Alineacion
        Private objTamanioFuente As Notificaciones.TamanioFuente
        Private objTipoFuente As Notificaciones.TipoFuente

#End Region

#Region "Propiedades"

        Public Property Identificador As Integer
            Get
                Return idIdentificador
            End Get
            Set(ByVal value As Integer)
                idIdentificador = value
            End Set
        End Property

        Public Property Descripcion As String
            Get
                Return stringDescripcion
            End Get
            Set(ByVal value As String)
                stringDescripcion = value
            End Set
        End Property

        Public Property TAG As String
            Get
                Return stringTag
            End Get
            Set(ByVal value As String)
                stringTag = value
            End Set
        End Property

        Public Property IsItalica As Boolean
            Get
                Return esItalica
            End Get
            Set(ByVal value As Boolean)
                esItalica = value
            End Set
        End Property

        Public Property IsNegrita As Boolean
            Get
                Return esNegrita
            End Get
            Set(ByVal value As Boolean)
                esNegrita = value
            End Set
        End Property

        Public Property Color As Notificaciones.Color
            Get
                Return objColor
            End Get
            Set(ByVal value As Notificaciones.Color)
                objColor = value
            End Set
        End Property

        Public Property Alineacion As Notificaciones.Alineacion
            Get
                Return objAlineacion
            End Get
            Set(ByVal value As Notificaciones.Alineacion)
                objAlineacion = value
            End Set
        End Property

        Public Property TamanioFuente As Notificaciones.TamanioFuente
            Get
                Return objTamanioFuente
            End Get
            Set(ByVal value As Notificaciones.TamanioFuente)
                objTamanioFuente = value
            End Set
        End Property

        Public Property TipoFuente As Notificaciones.TipoFuente
            Get
                Return objTipoFuente
            End Get
            Set(ByVal value As Notificaciones.TipoFuente)
                objTipoFuente = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()

        End Sub

        Public Sub New(ByVal Identificador As Integer, ByVal Descripcion As String, ByVal Tag As String, ByVal EsNegrita As Boolean, _
                       ByVal EsItalica As Boolean, ByVal IdentificadorColor As Integer, ByVal IdentificadorAlineacion As Integer, _
                       ByVal IdentificadorTamanioFuente As Integer, ByVal IdentificadorTipoFuente As Integer)


            Me.idIdentificador = Identificador
            Me.stringDescripcion = Descripcion
            Me.stringTag = Tag
            Me.esNegrita = EsNegrita
            Me.esItalica = EsItalica
            Me.Color = Notificaciones.ColorGetOne(IdentificadorColor)
            Me.Alineacion = Notificaciones.AlineacionGetOne(IdentificadorAlineacion)
            Me.TamanioFuente = Notificaciones.TamanioFuenteGetOne(IdentificadorTamanioFuente)
            Me.TipoFuente = Notificaciones.TipoFuenteGetOne(IdentificadorTipoFuente)

        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Clase para el elemento de notificaciones pantalla
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class NotificacionPantalla

#Region "Variables de trabajo"

        Private idIdentificador As Integer
        Private descripcionTitulo As String
        Private descripcionTexto As String
        Private cadenaArchivoAdjunto As String
        Private banderaVigencia As Boolean
        Private fechaInicio As Date
        Private fechaFin As Date

#End Region

#Region "Propiedades"

        Public Property Identificador As Integer
            Get
                Return idIdentificador
            End Get
            Set(ByVal value As Integer)
                idIdentificador = value
            End Set
        End Property

        Public Property Titulo As String
            Get
                Return descripcionTitulo
            End Get
            Set(ByVal value As String)
                descripcionTitulo = value
            End Set
        End Property

        Public Property Texto As String
            Get
                Return descripcionTexto
            End Get
            Set(ByVal value As String)
                descripcionTexto = value
            End Set
        End Property

        Public Property ArchivoAdjunto As String
            Get
                Return cadenaArchivoAdjunto
            End Get
            Set(ByVal value As String)
                cadenaArchivoAdjunto = value
            End Set
        End Property

        Public Property Vigente As Boolean
            Get
                Return banderaVigencia
            End Get
            Set(ByVal value As Boolean)
                banderaVigencia = value
            End Set
        End Property

        Public Property FechaInicioVigencia As Date
            Get
                Return fechaInicio
            End Get
            Set(ByVal value As Date)
                fechaInicio = value
            End Set
        End Property

        Public Property FechaFinVigencia As Date
            Get
                Return fechaFin
            End Get
            Set(ByVal value As Date)
                fechaFin = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()

        End Sub

        Public Sub New(ByVal Identificador As Integer, ByVal Titulo As String, ByVal Texto As String, ByVal ArchivoAdjunto As String, _
                       ByVal Vigente As Boolean, ByVal FechaInicio As Date, ByVal FechaFin As Date)

            Me.idIdentificador = Identificador
            Me.descripcionTitulo = Titulo
            Me.descripcionTexto = Texto
            Me.cadenaArchivoAdjunto = ArchivoAdjunto
            Me.banderaVigencia = Vigente
            Me.fechaInicio = FechaInicio
            Me.fechaFin = FechaFin

        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Clase para las notificaciones asignadas a usuarios
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class UsuariosNotificacionPantalla

        Private idIdentificador As Integer
        Private claveUsuario As String
        Private fechaIni As Date
        Private fechaFin As Date
        Private usuarioNombre As String


        Public Property IdentificadorNotificacion As Integer
            Get
                Return idIdentificador
            End Get
            Set(ByVal value As Integer)
                idIdentificador = value
            End Set
        End Property

        Public Property Usuario As String
            Get
                Return claveUsuario
            End Get
            Set(ByVal value As String)
                claveUsuario = value
            End Set
        End Property

        Public Property FechaInicio As Date
            Get
                Return fechaIni
            End Get
            Set(ByVal value As Date)
                fechaIni = value
            End Set
        End Property

        Public Property FechaFinalizacion As Date
            Get
                Return fechaFin
            End Get
            Set(ByVal value As Date)
                fechaFin = value
            End Set
        End Property

        Public Property Nombre As String
            Get
                Return usuarioNombre
            End Get
            Set(ByVal value As String)
                usuarioNombre = value
            End Set
        End Property

    End Class

    ''' <summary>
    ''' Clase para convertir de markup a HTML
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ConvertidorMarkup

        Public Shared Function MatchReplace(ByVal pattern As String, ByVal match As String, ByVal content As String) As String
            Return MatchReplace(pattern, match, content, False, False, False)
        End Function

        Public Shared Function MatchReplace(ByVal pattern As String, ByVal match As String, ByVal content As String, ByVal multi As Boolean) As String
            Return MatchReplace(pattern, match, content, multi, False, False)
        End Function

        Public Shared Function MatchReplace(ByVal pattern As String, ByVal match As String, ByVal content As String, ByVal multi As Boolean, ByVal white As Boolean) As String
            Return MatchReplace(pattern, match, content, multi, white)
        End Function

        ''' <summary>
        ''' Match and replace a specific pattern with formatted text
        ''' </summary>
        '''<param name="pattern">Regular expression pattern</param>
        '''<param name="match">Match replacement</param>
        '''<param name="content">Text to format</param>
        '''<param name="multi">Multiline text (optional)</param>
        '''<param name="white">Ignore white space (optional)</param>
        ''' <returns>HTML Formatted from the original BBCode</returns>
        Public Shared Function MatchReplace(ByVal pattern As String, ByVal match As String, ByVal content As String, ByVal multi As Boolean, ByVal white As Boolean, ByVal cult As Boolean) As String
            If multi AndAlso white AndAlso cult Then
                Return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant)
            ElseIf multi AndAlso white Then
                Return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.IgnoreCase)
            ElseIf multi AndAlso cult Then
                Return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.CultureInvariant)
            ElseIf white AndAlso cult Then
                Return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.CultureInvariant)
            ElseIf multi Then
                Return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase Or RegexOptions.Multiline)
            ElseIf white Then
                Return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase Or RegexOptions.IgnorePatternWhitespace)
            ElseIf cult Then
                Return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant)
            End If

            ' Default
            Return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase)
        End Function



    End Class

#End Region

#Region "Métodos de consulta"

    ''' <summary>
    ''' Método para obtener todos los elementos del catálogo de colores
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ColorGetAll() As List(Of Notificaciones.Color)

        Dim resultado As New List(Of Notificaciones.Color)
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtColores As DataTable = conexion.ConsultarDT("SELECT N_ID_COLOR, T_DSC_COLOR, T_DSC_COLOR_HEX FROM BDS_C_GR_COLOR WHERE N_FLAG_VIG = 1")

            For Each rowColor As DataRow In dtColores.Rows

                resultado.Add(New Notificaciones.Color With {.Identificador = CInt(rowColor("N_ID_COLOR")), .Descripcion = rowColor("T_DSC_COLOR").ToString(), .Hexadecimal = rowColor("T_DSC_COLOR_HEX").ToString()})

            Next

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function

    ''' <summary>
    ''' Método para obtener todos los elementos del catálogo de alineaciones
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AlineacionGetAll() As List(Of Notificaciones.Alineacion)

        Dim resultado As New List(Of Notificaciones.Alineacion)
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtAlineaciones As DataTable = conexion.ConsultarDT("SELECT N_ID_ALINEACION, T_DSC_ALINEACION, T_DSC_ALINEACION_CSS FROM BDS_C_GR_ALINEACION WHERE N_FLAG_VIG = 1")

            For Each rowAlineacion As DataRow In dtAlineaciones.Rows

                resultado.Add(New Notificaciones.Alineacion With {.Identificador = CInt(rowAlineacion("N_ID_ALINEACION")), .Descripcion = rowAlineacion("T_DSC_ALINEACION").ToString(), .CSS = rowAlineacion("T_DSC_ALINEACION_CSS").ToString()})

            Next

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try




    End Function

    ''' <summary>
    ''' Método para obtener todos los elementos del catálogo de Tamaños de fuente
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function TamanioFuenteGetAll() As List(Of Notificaciones.TamanioFuente)

        Dim resultado As New List(Of Notificaciones.TamanioFuente)
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtTamanioFuente As DataTable = conexion.ConsultarDT("SELECT N_ID_TAMANO_FUENTE, T_DSC_TAMANO_FUENTE, T_DSC_TAMANO_CSS FROM BDS_C_GR_TAMANO_FUENTE WHERE N_FLAG_VIG = 1")

            For Each rowTamanioFuente As DataRow In dtTamanioFuente.Rows

                resultado.Add(New Notificaciones.TamanioFuente With {.Identificador = CInt(rowTamanioFuente("N_ID_TAMANO_FUENTE")), .Descripcion = rowTamanioFuente("T_DSC_TAMANO_FUENTE").ToString(), .CSS = rowTamanioFuente("T_DSC_TAMANO_CSS").ToString()})

            Next

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function

    ''' <summary>
    ''' Método para obtener todos los elementos del catálogo de Tipos de fuente
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function TipoFuenteGetAll() As List(Of Notificaciones.TipoFuente)

        Dim resultado As New List(Of Notificaciones.TipoFuente)
        Dim conexion As New Conexion.SQLServer


        Try

            Dim dtTipoFuente As DataTable = conexion.ConsultarDT("SELECT N_ID_TIPO_FUENTE, T_DSC_TIPO_FUENTE FROM BDS_C_GR_TIPO_FUENTE WHERE N_FLAG_VIG = 1")

            For Each rowTipoFuente As DataRow In dtTipoFuente.Rows

                resultado.Add(New Notificaciones.TipoFuente With {.Identificador = CInt(rowTipoFuente("N_ID_TIPO_FUENTE")), .Descripcion = rowTipoFuente("T_DSC_TIPO_FUENTE").ToString()})

            Next

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function

    ''' <summary>
    ''' Método para obtener todos los elementos la tabla de Estilos de contenido
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function EstiloContenidoGetAll() As List(Of Notificaciones.EstiloContenido)

        Dim resultado As New List(Of Notificaciones.EstiloContenido)
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtEstilo As DataTable = conexion.ConsultarDT("SELECT N_ID_ESTILO_CONTENIDO, T_DSC_ESTILO_CONTENIDO, T_DSC_TAG, N_ID_COLOR, N_ID_TAMANO_FUENTE, N_ID_TIPO_FUENTE, N_ID_ALINEACION, N_FLAG_NEGRITAS, N_FLAG_ITALICA FROM BDS_C_GR_ESTILO_CONTENIDO WHERE N_FLAG_VIG = 1 ORDER BY N_ID_ESTILO_CONTENIDO")

            For Each rowEstilo As DataRow In dtEstilo.Rows

                resultado.Add(New EstiloContenido(CInt(rowEstilo("N_ID_ESTILO_CONTENIDO")), rowEstilo("T_DSC_ESTILO_CONTENIDO").ToString, rowEstilo("T_DSC_TAG").ToString, _
                                                   CBool(rowEstilo("N_FLAG_NEGRITAS")), CBool(rowEstilo("N_FLAG_ITALICA")), CInt(rowEstilo("N_ID_COLOR")), _
                                                   CInt(rowEstilo("N_ID_ALINEACION")), CInt(rowEstilo("N_ID_TAMANO_FUENTE")), CInt(rowEstilo("N_ID_TIPO_FUENTE"))))

            Next

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function

    ''' <summary>
    ''' Método para obtener todos los elementos de la tabla de Notificaciones de pantalla
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function NotificacionesPantallaGetAll() As List(Of Entities.Notificaciones.NotificacionPantalla)

        Dim resultado As New List(Of Notificaciones.NotificacionPantalla)
        Dim conexion As New Conexion.SQLServer
        'where

        Try

            Dim dtNotificaciones As DataTable = conexion.ConsultarDT("SELECT N_ID_NOTIFICACION_PANTALLA, T_DSC_TITULO, T_DSC_TEXTO, " & _
                                                                     " T_DSC_ARCHIVO_ADJUNTO, N_FLAG_VIG, F_FECH_INI_VIG, F_FECH_FIN_VIG " & _
                                                                     " FROM BDS_C_GR_NOTIFICACION_PANTALLA " & _
                                                                     " ORDER BY N_ID_NOTIFICACION_PANTALLA")

            For Each renglon As DataRow In dtNotificaciones.Rows

                resultado.Add(New Entities.Notificaciones.NotificacionPantalla(CInt(renglon("N_ID_NOTIFICACION_PANTALLA")), renglon("T_DSC_TITULO").ToString, _
                                                                               renglon("T_DSC_TEXTO"), renglon("T_DSC_ARCHIVO_ADJUNTO").ToString, _
                                                                               CBool(renglon("N_FLAG_VIG")), CDate(renglon("F_FECH_INI_VIG")), Now))

            Next

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function

    ''' <summary>
    ''' Método para obtener los elementos de la tabla de notificaciones de pantalla segun el filtro proporcionado
    ''' </summary>
    ''' <param name="where">Filtro para la búsqueda</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function NotificacionesPantallaGetCustom(ByVal where As String) As List(Of Entities.Notificaciones.NotificacionPantalla)

        Dim resultado As New List(Of Notificaciones.NotificacionPantalla)
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtNotificaciones As DataTable = conexion.ConsultarDT("SELECT N_ID_NOTIFICACION_PANTALLA, T_DSC_TITULO, T_DSC_TEXTO, " & _
                                                                     " T_DSC_ARCHIVO_ADJUNTO, N_FLAG_VIG, F_FECH_INI_VIG, F_FECH_FIN_VIG " & _
                                                                     " FROM BDS_C_GR_NOTIFICACION_PANTALLA " & where & _
                                                                     " ORDER BY N_ID_NOTIFICACION_PANTALLA")

            For Each renglon As DataRow In dtNotificaciones.Rows

                resultado.Add(New Entities.Notificaciones.NotificacionPantalla(CInt(renglon("N_ID_NOTIFICACION_PANTALLA")), renglon("T_DSC_TITULO").ToString, _
                                                                               renglon("T_DSC_TEXTO"), renglon("T_DSC_ARCHIVO_ADJUNTO").ToString, _
                                                                               CBool(renglon("N_FLAG_VIG")), CDate(renglon("F_FECH_INI_VIG")), Now))

            Next

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Método para obtener todos las notificaciones vigentes de un usuario
    ''' </summary>
    ''' <param name="Usuario">Identificador del Usuario de quien se buscan notificaciones</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function NotificacionesUsuarioGetAll(ByVal Usuario As String) As List(Of Entities.Notificaciones.UsuariosNotificacionPantalla)

        Dim resultado As New List(Of Entities.Notificaciones.UsuariosNotificacionPantalla)
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtNotificacionUsuario As DataTable = conexion.ConsultarDT("SELECT a.N_ID_NOTIFICACION_PANTALLA FROM BDS_R_GR_USUARIO_NOTIFICACION_PANTALLA a " & _
                                                                          " INNER JOIN BDS_C_GR_NOTIFICACION_PANTALLA b " & _
                                                                          " ON a.N_ID_NOTIFICACION_PANTALLA = b.N_ID_NOTIFICACION_PANTALLA " & _
                                                                          " WHERE b.N_FLAG_VIG = 1  AND a.T_ID_USUARIO = '" & Usuario & "' AND CAST(CONVERT(VARCHAR(20), GETDATE(), 112) AS DATETIME) " & _
                                                                          " BETWEEN a.F_FECH_INI_VIG AND a.F_FECH_FIN_VIG ")


            For Each renglon As DataRow In dtNotificacionUsuario.Rows

                resultado.Add(New Entities.Notificaciones.UsuariosNotificacionPantalla With {.Usuario = Usuario, _
                                                                                            .IdentificadorNotificacion = CInt(renglon("N_ID_NOTIFICACION_PANTALLA"))})

            Next

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Método para obtener la notificación de pantalla deseada
    ''' </summary>
    ''' <param name="identificador">Identificador de la notificación buscada</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function NotificacionesPantallaGetOne(ByVal identificador As Integer) As Entities.Notificaciones.NotificacionPantalla

        Dim resultado As Notificaciones.NotificacionPantalla = Nothing
        Dim conexion As New Conexion.SQLServer
        'where

        Try

            Dim dtNotificaciones As DataTable = conexion.ConsultarDT("SELECT N_ID_NOTIFICACION_PANTALLA, T_DSC_TITULO, T_DSC_TEXTO, " & _
                                                                     " T_DSC_ARCHIVO_ADJUNTO, N_FLAG_VIG, F_FECH_INI_VIG, F_FECH_FIN_VIG " & _
                                                                     " FROM BDS_C_GR_NOTIFICACION_PANTALLA WHERE N_ID_NOTIFICACION_PANTALLA = " & _
                                                                     identificador.ToString)

            If dtNotificaciones.Rows.Count > 0 Then



                With dtNotificaciones.Rows(0)

                    resultado = New Entities.Notificaciones.NotificacionPantalla(CInt(.Item("N_ID_NOTIFICACION_PANTALLA")), .Item("T_DSC_TITULO").ToString, _
                                                                   .Item("T_DSC_TEXTO"), .Item("T_DSC_ARCHIVO_ADJUNTO").ToString, _
                                                                   CBool(.Item("N_FLAG_VIG")), CDate(.Item("F_FECH_INI_VIG")), Now)

                End With

            End If

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Método para obtener un color
    ''' </summary>
    ''' <param name="IdColor">Identificador del color</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ColorGetOne(ByVal IdColor As Integer) As Notificaciones.Color

        Dim resultado As Notificaciones.Color = Nothing
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtColores As DataTable = conexion.ConsultarDT("SELECT N_ID_COLOR, T_DSC_COLOR, T_DSC_COLOR_HEX FROM BDS_C_GR_COLOR WHERE N_ID_COLOR = " & IdColor.ToString)

            If dtColores.Rows.Count > 0 Then

                With dtColores.Rows(0)

                    resultado = New Notificaciones.Color(CInt(.Item("N_ID_COLOR")), .Item("T_DSC_COLOR").ToString, .Item("T_DSC_COLOR_HEX").ToString)

                End With

            End If

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Método para obtener una alineación
    ''' </summary>
    ''' <param name="IdAlineacion">Identificador de la alineación</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AlineacionGetOne(ByVal IdAlineacion As Integer) As Notificaciones.Alineacion

        Dim resultado As Notificaciones.Alineacion = Nothing
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtAlineaciones As DataTable = conexion.ConsultarDT("SELECT N_ID_ALINEACION, T_DSC_ALINEACION, T_DSC_ALINEACION_CSS FROM BDS_C_GR_ALINEACION WHERE N_ID_ALINEACION = " & IdAlineacion.ToString)

            If dtAlineaciones.Rows.Count > 0 Then

                With dtAlineaciones.Rows(0)

                    resultado = New Notificaciones.Alineacion(CInt(.Item("N_ID_ALINEACION")), .Item("T_DSC_ALINEACION").ToString, .Item("T_DSC_ALINEACION_CSS").ToString)

                End With


            End If

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Método para obtener un tamaño de fuente
    ''' </summary>
    ''' <param name="IdTamanioFuente">Identificador del tamaño de fuente</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function TamanioFuenteGetOne(ByVal IdTamanioFuente As Integer) As Notificaciones.TamanioFuente

        Dim resultado As Notificaciones.TamanioFuente = Nothing
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtTamanioFuente As DataTable = conexion.ConsultarDT("SELECT N_ID_TAMANO_FUENTE, T_DSC_TAMANO_FUENTE, T_DSC_TAMANO_CSS FROM BDS_C_GR_TAMANO_FUENTE WHERE N_ID_TAMANO_FUENTE = " & IdTamanioFuente.ToString)

            If dtTamanioFuente.Rows.Count > 0 Then

                With dtTamanioFuente.Rows(0)

                    resultado = New Notificaciones.TamanioFuente(CInt(.Item("N_ID_TAMANO_FUENTE")), .Item("T_DSC_TAMANO_FUENTE").ToString, .Item("T_DSC_TAMANO_CSS").ToString)

                End With


            End If

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Método para obtener un tipo de fuente
    ''' </summary>
    ''' <param name="IdTipoFuente">Identificador del tipo de fuente</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function TipoFuenteGetOne(ByVal IdTipoFuente As Integer) As Notificaciones.TipoFuente

        Dim resultado As Notificaciones.TipoFuente = Nothing
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtTipoFuente As DataTable = conexion.ConsultarDT("SELECT N_ID_TIPO_FUENTE, T_DSC_TIPO_FUENTE FROM BDS_C_GR_TIPO_FUENTE WHERE N_ID_TIPO_FUENTE = " & IdTipoFuente.ToString)

            If dtTipoFuente.Rows.Count > 0 Then

                With dtTipoFuente.Rows(0)

                    resultado = New Notificaciones.TipoFuente(CInt(.Item("N_ID_TIPO_FUENTE")), .Item("T_DSC_TIPO_FUENTE").ToString)

                End With

            End If

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Método para obtener un estilo de contenido
    ''' </summary>
    ''' <param name="IdEstiloContenido">Identificador del estilo de contenido</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function EstiloContenidoGetOne(ByVal IdEstiloContenido As Integer) As Notificaciones.EstiloContenido

        Dim resultado As Notificaciones.EstiloContenido = Nothing
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtEstilo As DataTable = conexion.ConsultarDT("SELECT N_ID_ESTILO_CONTENIDO, T_DSC_ESTILO_CONTENIDO, T_DSC_TAG, N_ID_COLOR, " & _
                                                             " N_ID_TAMANO_FUENTE, N_ID_TIPO_FUENTE, N_ID_ALINEACION, N_FLAG_NEGRITAS, N_FLAG_ITALICA " & _
                                                             "FROM BDS_C_GR_ESTILO_CONTENIDO WHERE N_ID_ESTILO_CONTENIDO = " & IdEstiloContenido.ToString)

            With dtEstilo.Rows(0)

                resultado = New EstiloContenido(CInt(.Item("N_ID_ESTILO_CONTENIDO")), .Item("T_DSC_ESTILO_CONTENIDO").ToString, .Item("T_DSC_TAG").ToString, _
                                       CBool(.Item("N_FLAG_NEGRITAS")), CBool(.Item("N_FLAG_ITALICA")), CInt(.Item("N_ID_COLOR")), _
                                       CInt(.Item("N_ID_ALINEACION")), CInt(.Item("N_ID_TAMANO_FUENTE")), CInt(.Item("N_ID_TIPO_FUENTE")))


            End With

            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Método para obtener la lista de usuarios asignados a una notificación
    ''' </summary>
    ''' <param name="identificadorNotificacion">Identificador de la notificación</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ListaUsuariosNotificacionPantalla(ByVal identificadorNotificacion As Integer) As List(Of Entities.Notificaciones.UsuariosNotificacionPantalla)


        Dim resultado As New List(Of Entities.Notificaciones.UsuariosNotificacionPantalla)
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtUsuariosNotifica As DataTable = conexion.ConsultarDT("SELECT n.T_ID_USUARIO, n.F_FECH_INI_VIG, n.F_FECH_FIN_VIG, (u.T_DSC_NOMBRE + ISNULL(' ' + u.T_DSC_APELLIDO, '') + ISNULL('' + u.T_DSC_APELLIDO_AUX, ' ')) NombreCompleto FROM " & _
                                                                 "BDS_R_GR_USUARIO_NOTIFICACION_PANTALLA n INNER JOIN dbo.BDS_C_GR_USUARIO u ON n.T_ID_USUARIO = u.T_ID_USUARIO WHERE N_ID_NOTIFICACION_PANTALLA = " & _
                                                                 identificadorNotificacion.ToString)

            For Each renglon As DataRow In dtUsuariosNotifica.Rows

                resultado.Add(New Entities.Notificaciones.UsuariosNotificacionPantalla With {.IdentificadorNotificacion = identificadorNotificacion, _
                                                                                            .Usuario = renglon("T_ID_USUARIO").ToString, _
                                                                                            .FechaInicio = CDate(renglon("F_FECH_INI_VIG")), _
                                                                                            .FechaFinalizacion = CDate(renglon("F_FECH_FIN_VIG")), _
                                                                                             .Nombre = renglon("NombreCompleto").ToString})

            Next


            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Método para obtener la cadena de exclusión de usuarios ya asignados a una notificación
    ''' </summary>
    ''' <param name="identificadorNotificacion">Identificador de la notificación</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CadenaUsuariosExlusionNotificacionPantalla(ByVal identificadorNotificacion As Integer) As String

        ' Obtenemos los ya asignados
        Dim ListaUsuariosAsignados As List(Of Entities.Notificaciones.UsuariosNotificacionPantalla) = Entities.Notificaciones.ListaUsuariosNotificacionPantalla(identificadorNotificacion)
        Dim whereAuxiliar As String = ""

        ' formamos cadena de exclusion
        If ListaUsuariosAsignados.Any Then

            whereAuxiliar = " AND T_ID_USUARIO NOT IN ("

            For Each usuario As Entities.Notificaciones.UsuariosNotificacionPantalla In ListaUsuariosAsignados

                whereAuxiliar &= "'" & usuario.Usuario & "',"

            Next

            whereAuxiliar = whereAuxiliar.TrimEnd(",") & ")"

        End If

        Return whereAuxiliar

    End Function

    ''' <summary>
    ''' Método para obtener la descrpción CSS completa de un estilo
    ''' </summary>
    ''' <param name="IdentificadorEstilo">Identificador del estilo</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenEstilo(ByVal IdentificadorEstilo As Entities.Notificaciones.TipoEstiloNotificacion) As String

        Dim estilo As String = ""

        Dim resultado As Notificaciones.Color = Nothing
        Dim conexion As New Conexion.SQLServer

        Try

            Dim dtEstilo As DataTable = conexion.ConsultarDT("SELECT N_ID_ESTILO_CONTENIDO, T_DSC_COLOR_HEX, T_DSC_TAMANO_CSS, T_DSC_TIPO_FUENTE, " & _
                                                                     " T_DSC_ALINEACION_CSS, N_FLAG_NEGRITAS, N_FLAG_ITALICA " & _
                                                                     " FROM BDS_C_GR_ESTILO_CONTENIDO a INNER JOIN BDS_C_GR_COLOR b ON a.N_ID_COLOR = b.N_ID_COLOR " & _
                                                                     " INNER JOIN BDS_C_GR_TAMANO_FUENTE c ON a.N_ID_TAMANO_FUENTE = c.N_ID_TAMANO_FUENTE " & _
                                                                     " INNER JOIN BDS_C_GR_TIPO_FUENTE d ON a.N_ID_TIPO_FUENTE = d.N_ID_TIPO_FUENTE " & _
                                                                     " INNER JOIN BDS_C_GR_ALINEACION e ON a.N_ID_ALINEACION = e.N_ID_ALINEACION " & _
                                                                     " WHERE N_ID_ESTILO_CONTENIDO = " & IdentificadorEstilo)


            With dtEstilo.Rows(0)

                Select Case IdentificadorEstilo


                    Case Notificaciones.TipoEstiloNotificacion.estilofondo, Notificaciones.TipoEstiloNotificacion.Estilofondo2, Notificaciones.TipoEstiloNotificacion.estilobordediv
                        ' solo color
                        estilo = .Item("T_DSC_COLOR_HEX").ToString()

                    Case Notificaciones.TipoEstiloNotificacion.Estiloletra2, Notificaciones.TipoEstiloNotificacion.Estiloletra3
                        ' sin alineacion
                        estilo &= ColorCSS(.Item("T_DSC_COLOR_HEX").ToString)
                        estilo &= TamanioFuenteCSS(.Item("T_DSC_TAMANO_CSS").ToString)
                        estilo &= TipoFuenteCSS(.Item("T_DSC_TIPO_FUENTE").ToString)
                        estilo &= NegritaCSS(CBool(.Item("N_FLAG_NEGRITAS")))
                        estilo &= ItalicaCSS(CBool(.Item("N_FLAG_ITALICA")))

                    Case Else
                        ' todo
                        estilo &= ColorCSS(.Item("T_DSC_COLOR_HEX").ToString)
                        estilo &= TamanioFuenteCSS(.Item("T_DSC_TAMANO_CSS").ToString)
                        estilo &= TipoFuenteCSS(.Item("T_DSC_TIPO_FUENTE").ToString)
                        estilo &= AlineacionCSS(.Item("T_DSC_ALINEACION_CSS").ToString)
                        estilo &= NegritaCSS(CBool(.Item("N_FLAG_NEGRITAS")))
                        estilo &= ItalicaCSS(CBool(.Item("N_FLAG_ITALICA")))


                End Select


            End With


            Return estilo

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try


    End Function

    ''' <summary>
    ''' Método para transformar un texto Markup en HTML
    ''' </summary>
    ''' <param name="texto">Texto a ser transformado</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function TransformaTextoHtml(ByVal texto As String) As String

        Dim HTML As String = ""

        Dim Estilo_01 As String = ObtenEstilo(Entities.Notificaciones.TipoEstiloNotificacion.EstilodeTítulo)
        Dim Estilo_02 As String = ObtenEstilo(Entities.Notificaciones.TipoEstiloNotificacion.Estiloletra1)
        Dim Estilo_03 As String = ObtenEstilo(Entities.Notificaciones.TipoEstiloNotificacion.Estiloletra2)
        Dim Estilo_04 As String = ObtenEstilo(Entities.Notificaciones.TipoEstiloNotificacion.Estiloletra3)
        Dim Estilo_06 As String = ObtenEstilo(Entities.Notificaciones.TipoEstiloNotificacion.Estilohypervinculo)
        Dim Estilo_07 As String = ObtenEstilo(Entities.Notificaciones.TipoEstiloNotificacion.Estilofondo2)
        Dim Estilo_08 As String = ObtenEstilo(Entities.Notificaciones.TipoEstiloNotificacion.estilobordediv)


        HTML = Net.WebUtility.HtmlEncode(texto)
        HTML = HTML.Replace(vbCrLf, "</br>")
        HTML = String.Format("<p> {0} </p>", HTML)
        HTML = ConvertidorMarkup.MatchReplace("\[re\](.*?)\[\/re\]", String.Format("<div class='esqnas2' style='background-color:{1};padding: 1px;'><div class='esqnas' style='background-color:{0};padding: 4px; margin: 1px;'>$1</div> </div>", Estilo_08, Estilo_08), HTML, True)
        HTML = ConvertidorMarkup.MatchReplace("\[T\](.*?)\[\/T\]", String.Format("<p style='{0}'>$1</p>", Estilo_01), HTML, True)
        'Convertir Bold
        HTML = ConvertidorMarkup.MatchReplace("\[N\](.*?)\[\/N\]", "<strong>$1</strong>", HTML, True)
        'Convertir Italicas
        HTML = ConvertidorMarkup.MatchReplace("\[C\](.*?)\[\/C\]", "<em>$1</em>", HTML, True)
        'Convertir Letra 1
        HTML = ConvertidorMarkup.MatchReplace("\[L1\](.*?)\[\/L1\]", String.Format("<p style='{0}'>$1</p>", Estilo_02), HTML, True)
        'Convertir Letra 2
        HTML = ConvertidorMarkup.MatchReplace("\[L2\](.*?)\[\/L2\]", String.Format("<span style='{0}'>$1</span>", Estilo_03), HTML, True)
        'Convertir Letra 3
        HTML = ConvertidorMarkup.MatchReplace("\[L3\](.*?)\[\/L3\]", String.Format("<span style='{0}'>$1</span>", Estilo_04), HTML, True)
        'HTML Links
        HTML = ConvertidorMarkup.MatchReplace("\[h=([^\]]+)](.*?)\[\/h\]", String.Format("<a style='{0}' href=""http://$1"" target='_blank'>$2</a>", Estilo_06), HTML)
        HTML = ConvertidorMarkup.MatchReplace("\[h\](.*?)\[\/h\]", String.Format("<a style='{0}' href=""http://$1"" target='_blank'>$1</a>", Estilo_06), HTML)
        HTML = ConvertidorMarkup.MatchReplace("\[d\]([^\]]+)\[\/d\]", String.Format("<a style='color:{0}' class='click_descarga' href=""#"">$1</a>", Estilo_07), HTML)


        Return HTML

    End Function





#End Region

#Region "Metodos Persistentes"

    ''' <summary>
    ''' Método para guardar la configuración de estilos
    ''' </summary>
    ''' <param name="Estilos">Lista de estilos</param>
    ''' <param name="Usuario">Usuario que realiza la operacion</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GuardaConfiguracionEstilos(ByVal Estilos As List(Of Entities.Notificaciones.EstiloContenido), ByVal Usuario As String) As Boolean

        Dim conex As New Conexion.SQLServer
        Dim transaccion As Data.SqlClient.SqlTransaction

        ' Iniciamos la transaccion
        transaccion = conex.BeginTransaction

        Dim bitacora As New Conexion.Bitacora("Actualización de Configuracion Estilos Notificaciones", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Dim resultado As Boolean = False


        Dim Campo As New List(Of String)
        Dim Valor As New List(Of Object)

        Dim CampoCondicion As New List(Of String)
        Dim ValorCondicion As New List(Of Object)

        ' Agregamos los campos a actualizar
        Campo.Add("N_ID_COLOR")
        Campo.Add("N_ID_TAMANO_FUENTE")
        Campo.Add("N_ID_TIPO_FUENTE")
        Campo.Add("N_ID_ALINEACION")
        Campo.Add("N_FLAG_NEGRITAS")
        Campo.Add("N_FLAG_ITALICA")


        ' Agregamos el campo llave
        CampoCondicion.Add("N_ID_ESTILO_CONTENIDO")



        ' recorremos estilos para actualizar
        For Each estilo In Estilos

            ValorCondicion.Clear()
            Valor.Clear()

            ValorCondicion.Add(estilo.Identificador)

            Valor.Add(estilo.Color.Identificador)
            Valor.Add(estilo.TamanioFuente.Identificador)
            Valor.Add(estilo.TipoFuente.Identificador)
            Valor.Add(estilo.Alineacion.Identificador)

            Valor.Add(IIf(estilo.IsNegrita, 1, 0))
            Valor.Add(IIf(estilo.IsItalica, 1, 0))

            resultado = conex.ActualizarConTransaccion("BDS_C_GR_ESTILO_CONTENIDO", Campo, Valor, CampoCondicion, ValorCondicion, transaccion)
            bitacora.ActualizarConTransaccion("BDS_C_GR_ESTILO_CONTENIDO", Campo, Valor, CampoCondicion, ValorCondicion, resultado, "")


            If Not resultado Then

                Exit For

            End If

        Next


        Select Case resultado

            Case True
                transaccion.Commit()

            Case False
                transaccion.Rollback()

        End Select


        bitacora.Finalizar(resultado)

        Return resultado

    End Function

    ''' <summary>
    ''' Método para eliminar lógicamente una notificación 
    ''' </summary>
    ''' <param name="IdentificadorNotificacion">Identificador de la notificación</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function EliminaNotificacionPantalla(ByVal IdentificadorNotificacion As Integer) As Boolean

        Dim conex As New Conexion.SQLServer

        Try

            Dim Campo As New List(Of String)
            Dim Valor As New List(Of Object)

            Dim CampoCondicion As New List(Of String)
            Dim ValorCondicion As New List(Of Object)


            ' Agregamos los campos a actualizar
            Campo.Add("N_FLAG_VIG")
            Campo.Add("F_FECH_FIN_VIG")

            Valor.Add(0)
            Valor.Add("GETDATE()")


            ' Agregamos el campo llave
            CampoCondicion.Add("N_ID_NOTIFICACION_PANTALLA")

            ValorCondicion.Add(IdentificadorNotificacion)


            Return conex.Actualizar("BDS_C_GR_NOTIFICACION_PANTALLA", Campo, Valor, CampoCondicion, ValorCondicion)

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conex) Then
                conex.CerrarConexion()
            End If

        End Try




    End Function

    ''' <summary>
    ''' Método para Desasignar usuarios de una notificación
    ''' </summary>
    ''' <param name="ListaDesasigna">Lista de usuarios a desasignar</param>
    ''' <param name="Usuario">Usuario que realiza la operación</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DesasignaUsuariosNotificación(ByVal ListaDesasigna As List(Of Entities.Notificaciones.UsuariosNotificacionPantalla), ByVal Usuario As String) As Boolean

        Dim conex As New Conexion.SQLServer
        Dim transaccion As Data.SqlClient.SqlTransaction
        Dim resultado As Boolean = False

        Dim CampoCondicion As New List(Of String)
        Dim ValorCondicion As New List(Of Object)

        CampoCondicion.Add("T_ID_USUARIO")
        CampoCondicion.Add("N_ID_NOTIFICACION_PANTALLA")

        ' Iniciamos la transaccion
        transaccion = conex.BeginTransaction

        Dim bitacora As New Conexion.Bitacora("Desasignacion Usuario Notificaciones", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        For Each Usuarios As Entities.Notificaciones.UsuariosNotificacionPantalla In ListaDesasigna

            ValorCondicion.Clear()

            ValorCondicion.Add(Usuarios.Usuario)
            ValorCondicion.Add(Usuarios.IdentificadorNotificacion)

            resultado = conex.EliminarConTransaccion("BDS_R_GR_USUARIO_NOTIFICACION_PANTALLA", CampoCondicion, ValorCondicion, transaccion)
            bitacora.EliminarConTransaccion("BDS_R_GR_USUARIO_NOTIFICACION_PANTALLA", CampoCondicion, ValorCondicion, resultado, "")


            If Not resultado Then

                Exit For

            End If

        Next

        Select Case resultado

            Case True
                transaccion.Commit()

            Case False
                transaccion.Rollback()

        End Select


        bitacora.Finalizar(resultado)


        Return resultado

    End Function

    ''' <summary>
    ''' Método para Asignar usuarios de una notificación
    ''' </summary>
    ''' <param name="ListaAsigna">Lista de usuarios a desasignar</param>
    ''' <param name="Usuario">Usuario que realiza la operación</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AsignaUsuariosNotificación(ByVal ListaAsigna As List(Of Entities.Notificaciones.UsuariosNotificacionPantalla), ByVal Usuario As String) As Boolean

        Dim conex As New Conexion.SQLServer
        Dim transaccion As Data.SqlClient.SqlTransaction
        Dim resultado As Boolean = False

        Dim Campo As New List(Of String)
        Dim Valor As New List(Of Object)

        Campo.Add("T_ID_USUARIO")
        Campo.Add("N_ID_NOTIFICACION_PANTALLA")
        Campo.Add("F_FECH_INI_VIG")
        Campo.Add("F_FECH_FIN_VIG")


        ' Iniciamos la transaccion
        transaccion = conex.BeginTransaction

        Dim bitacora As New Conexion.Bitacora("Asignacion Usuario Notificaciones", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        For Each Usuarios As Entities.Notificaciones.UsuariosNotificacionPantalla In ListaAsigna

            Valor.Clear()

            Valor.Add(Usuarios.Usuario)
            Valor.Add(Usuarios.IdentificadorNotificacion)
            Valor.Add(Usuarios.FechaInicio.ToString("yyyy-MM-dd"))
            Valor.Add(Usuarios.FechaFinalizacion.ToString("yyyy-MM-dd"))

            resultado = conex.InsertarConTransaccion("BDS_R_GR_USUARIO_NOTIFICACION_PANTALLA", Campo, Valor, transaccion)
            bitacora.InsertarConTransaccion("BDS_R_GR_USUARIO_NOTIFICACION_PANTALLA", Campo, Valor, resultado, "")


            If Not resultado Then

                Exit For

            End If

        Next

        Select Case resultado

            Case True
                transaccion.Commit()

            Case False
                transaccion.Rollback()

        End Select


        bitacora.Finalizar(resultado)


        Return resultado

    End Function

    ''' <summary>
    ''' Método para guardar notificaciones
    ''' </summary>
    ''' <param name="notificacion">Notificación a ser guardada</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GuardaNotificacionPantalla(ByVal notificacion As Entities.Notificaciones.NotificacionPantalla) As Integer

        Dim conexion As New Conexion.SQLServer
        Dim resultado As Integer = 0

        Try

            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(N_ID_NOTIFICACION_PANTALLA) + 1) N_ID_NOTIFICACION_PANTALLA FROM BDS_C_GR_NOTIFICACION_PANTALLA")

            If dr.Read() Then

                If IsDBNull(dr("N_ID_NOTIFICACION_PANTALLA")) Then
                    resultado = 1
                Else
                    resultado = CInt(dr("N_ID_NOTIFICACION_PANTALLA"))
                End If

            End If

            dr.Close()

            Dim Campo As New List(Of String)
            Dim Valor As New List(Of Object)

            Campo.Add("N_ID_NOTIFICACION_PANTALLA")
            Campo.Add("T_DSC_TITULO")
            Campo.Add("T_DSC_TEXTO")
            Campo.Add("T_DSC_ARCHIVO_ADJUNTO")
            Campo.Add("N_FLAG_VIG")
            Campo.Add("F_FECH_INI_VIG")

            Valor.Add(resultado)
            Valor.Add(notificacion.Titulo)
            Valor.Add(notificacion.Texto)
            Valor.Add(notificacion.ArchivoAdjunto)
            Valor.Add(1)
            Valor.Add("GETDATE()")


            If Not conexion.Insertar("BDS_C_GR_NOTIFICACION_PANTALLA", Campo, Valor) Then

                resultado = 0

            End If


            Return resultado

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try



    End Function

    ''' <summary>
    ''' Método para actualizar notificaciones
    ''' </summary>
    ''' <param name="notificacion">Notificación a ser actualizada</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ActualizaNotificacionPantalla(ByVal notificacion As Entities.Notificaciones.NotificacionPantalla) As Boolean

        Dim conex As New Conexion.SQLServer

        Try

            Dim Campo As New List(Of String)
            Dim Valor As New List(Of Object)

            Dim CampoCondicion As New List(Of String)
            Dim ValorCondicion As New List(Of Object)

            ' Agregamos los campos a actualizar
            Campo.Add("T_DSC_TITULO")
            Campo.Add("T_DSC_TEXTO")

            Valor.Add(notificacion.Titulo)
            Valor.Add(notificacion.Texto)

            ' en caso de enviarse archivo adjunto
            If notificacion.ArchivoAdjunto <> "" Then

                Campo.Add("T_DSC_ARCHIVO_ADJUNTO")
                Valor.Add(notificacion.ArchivoAdjunto)

            End If


            ' Agregamos el campo llave
            CampoCondicion.Add("N_ID_NOTIFICACION_PANTALLA")

            ValorCondicion.Add(notificacion.Identificador)

            Return conex.Actualizar("BDS_C_GR_NOTIFICACION_PANTALLA", Campo, Valor, CampoCondicion, ValorCondicion)

        Catch ex As Exception

            Throw ex

        Finally

            If Not IsNothing(conex) Then
                conex.CerrarConexion()
            End If

        End Try


    End Function

#End Region

#Region "Para obtener estilos CSS"

    ''' <summary>
    ''' Método que regresa el texto completo de la etiqueta CSS
    ''' </summary>
    ''' <param name="Tamanio">parámetro a ser completado</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function TamanioFuenteCSS(ByVal Tamanio As String) As String

        Return String.Format("font-size: {0};", Tamanio)

    End Function

    ''' <summary>
    ''' Método que regresa el texto completo de la etiqueta CSS
    ''' </summary>
    ''' <param name="NombreFuente">parámetro a ser completado</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function TipoFuenteCSS(ByVal NombreFuente As String) As String

        Return String.Format("font-family: {0};", NombreFuente)

    End Function

    ''' <summary>
    ''' Método que regresa el texto completo de la etiqueta CSS
    ''' </summary>
    ''' <param name="Alineacion">parámetro a ser completado</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function AlineacionCSS(ByVal Alineacion As String) As String

        Return String.Format("text-align: {0};", Alineacion)

    End Function

    ''' <summary>
    ''' Método que regresa el texto completo de la etiqueta CSS
    ''' </summary>
    ''' <param name="IsNegrita">Define si se quiere o no letra en negritas</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function NegritaCSS(ByVal IsNegrita As Boolean) As String

        Dim resultado As String = ""

        If IsNegrita Then

            resultado = "font-weight:bold;"

        End If

        Return resultado

    End Function

    ''' <summary>
    ''' Método que regresa el texto completo de la etiqueta CSS
    ''' </summary>
    ''' <param name="IsItalica">Define si se quiere o no letra en itálica</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function ItalicaCSS(ByVal IsItalica As Boolean) As String

        Dim resultado As String = ""

        If IsItalica Then

            resultado = "font-style:italic;"

        End If

        Return resultado

    End Function

    ''' <summary>
    ''' Método que regresa el texto completo de la etiqueta CSS
    ''' </summary>
    ''' <param name="colorHex">Código hexadecimal del color</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function ColorCSS(ByVal colorHex As String) As String

        Return String.Format("color: {0};", colorHex)

    End Function

#End Region

End Class
