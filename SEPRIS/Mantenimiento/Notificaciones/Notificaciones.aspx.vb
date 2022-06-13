Public Class Notificaciones
    Inherits System.Web.UI.Page

    '***********************************************************************************************************
    ' Fecha Creación:       18 Julio 2013
    ' Codificó:             Jorge Alberto Rangel Ruiz
    ' Empresa:              Softtek
    ' Descripción           Pantalla para selección de acción deseada en Notificaciones
    '***********************************************************************************************************

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' cargamos algo si es necesario

    End Sub

    Private Sub btnBandeja_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBandeja.Click

        ' redireccionamos a la página de la bandeja de notificaciones
        Response.Redirect("NotificacionesBandeja.aspx", False)

    End Sub

    Private Sub btnConfiguraEstilos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfiguraEstilos.Click

        ' Redireccionamos a la página de la configuracion de estilos de notificaciones
        Response.Redirect("NotificacionesConfiguracionEstilos.aspx", False)

    End Sub

End Class