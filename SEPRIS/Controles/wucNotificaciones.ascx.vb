Public Class wucNotificaciones
    Inherits System.Web.UI.UserControl

    '***********************************************************************************************************
    ' Fecha Creación:       24 Julio 2013
    ' Codificó:             Jorge Alberto Rangel Ruiz
    ' Empresa:              Softtek
    ' Descripción           Control de Usuario encargado de mostrar notificaciones segun configuración
    '***********************************************************************************************************

#Region "Propiedades"

    Private vistaPrevia As Boolean = False
    Public Property EsVistaPrevia As Boolean
        Get
            Return vistaPrevia
        End Get
        Set(ByVal value As Boolean)
            vistaPrevia = value
        End Set
    End Property


#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Si ya está enterado, no se hace nada
        If Not IsNothing(Session("Enterado")) Then

            Exit Sub

        End If

        ' si no es vista previa, preparamos el mensaje
        If Not vistaPrevia Then

            PreparaMensaje()

        End If


    End Sub

    ''' <summary>
    ''' Método para cargar el mensaje a mostrar
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargaMensaje(ByVal IdentificadorMensaje As Integer) As String

        Return Entities.Notificaciones.NotificacionesPantallaGetOne(IdentificadorMensaje).Texto

    End Function

    Private Sub btnEnterado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEnterado.Click


        ' Si no es vista previa, se marca la sesion para no volver a mostrar el control
        If Not vistaPrevia Then

            Session("Enterado") = 1

        End If


        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CerramosNotificacion", "CerramosNotificacion();", True)


    End Sub

    ''' <summary>
    ''' Método publico para levantar la vista previa del mensaje de notificacion
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LevantarVistaPrevia(ByVal mensaje As String)

        PreparaMensaje(mensaje)

    End Sub

    ''' <summary>
    ''' Metodo para generar y mostrar el mensaje
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PreparaMensaje()

        If IsNothing(Session(Entities.Usuario.SessionID)) Then
            Exit Sub
        End If

        Dim notificaciones As List(Of Entities.Notificaciones.UsuariosNotificacionPantalla) = Entities.Notificaciones.NotificacionesUsuarioGetAll(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)

        If Not notificaciones.Any Then

            'No hay mensajes, levantamos la bandera para indicar que ya no se consulta nada
            Session("Enterado") = 1
            Exit Sub

        End If


        TransformaMensaje(CargaMensaje(notificaciones(0).IdentificadorNotificacion))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosNotificacion", "MostramosNotificacionUsuario();", True)

    End Sub

    ''' <summary>
    ''' Metodo sobrecargado para generar y mostrar el mensaje en vista previa
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PreparaMensaje(ByVal mensaje As String)

        TransformaMensaje(mensaje)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosNotificacion", "MostramosNotificacionUsuario();", True)

    End Sub

    ''' <summary>
    ''' Método que transforma el mensaje plano en HTML interpretable
    ''' </summary>
    ''' <param name="mensaje"></param>
    ''' <remarks></remarks>
    Private Sub TransformaMensaje(ByVal mensaje As String)

        divmensaje.Style.Add("background-color", Entities.Notificaciones.ObtenEstilo(Entities.Notificaciones.TipoEstiloNotificacion.estilofondo))

        lblMensajeNotificacion.Text = Entities.Notificaciones.TransformaTextoHtml(mensaje)

    End Sub


End Class