Public Class wucVencimientoSesion
    Inherits System.Web.UI.UserControl

    '***********************************************************************************************************
    ' Fecha Creación:       05 Julio 2013
    ' Codificó:             Jorge Alberto Rangel Ruiz
    ' Empresa:              Softtek
    ' Descripción           Control de usuario que permite manejar la duracion de sesiones segun los parámetros 
    ' proporcionados,  en la página host deb en existir dos botones de nombre "btnContinuar" y "btnSalir", mismos
    ' que se invocan al dar clic en Continuar (invoca btnContinuar) y Cerrar (invoca btnSalir), y en ellos
    ' se programará la lógica deseada para tales eventos, ya sea continuar la sesión o terminar ésta.
    ' 
    ' Modificaciones:
    ' 
    '***********************************************************************************************************

#Region "Propiedades"

    ''' <summary>
    ''' Propiedad para asignar u obtener el tiempo en minutos de la duración de sesión
    ''' </summary>
    ''' <value>Tiempo de sesión</value>
    ''' <returns>Tiempo de sesión</returns>
    ''' <remarks></remarks>
    Public Property TiempoSesion As String
        Get
            Return hdnTiempoSesion.Value
        End Get
        Set(ByVal value As String)
            hdnTiempoSesion.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para asignar u obtener el tiempo en segundos de la duración de sesión
    ''' </summary>
    ''' <value>Tiempo de sesión en segundos</value>
    ''' <returns>Tiempo de sesión en segundos</returns>
    ''' <remarks></remarks>
    Public Property TiempoSesionSegundos As String
        Get
            Return hdnTiempoSesionSegundos.Value
        End Get
        Set(ByVal value As String)
            hdnTiempoSesionSegundos.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para asignar u obtener el tiempo en segundos de confirmación 
    ''' </summary>
    ''' <value>Tiempo de confirmación</value>
    ''' <returns>Tiempo de confirmación</returns>
    ''' <remarks></remarks>
    Public Property TiempoConfirmacion As String
        Get
            Return hdnTiempoConfirmacion.Value
        End Get
        Set(ByVal value As String)
            hdnTiempoConfirmacion.Value = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Cargar algo si es necesario

    End Sub

End Class