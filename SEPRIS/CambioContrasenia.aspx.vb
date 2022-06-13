Public Class CambioContrasenia
    Inherits System.Web.UI.Page

    '***********************************************************************************************************
    ' Fecha Creación:       12 Septiembre 2013
    ' Codificó:             Jorge Alberto Rangel Ruiz
    ' Empresa:              Softtek
    ' Descripción           Pantalla para cambio de contraseña (se muestra cuando se genera una automáticamente por el sistema
    '                       por ejemplo, usuarios nuevos, olvido de contraseña, etc.)
    '***********************************************************************************************************

#Region "Propiedades y constantes"

    ''' <summary>
    ''' Propiedad que regresa el nombre de la página
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>SE EMPLEA EN LA MASTER PAGE</remarks>
    Public Shared ReadOnly Property Nombre As String
        Get
            Return "CambioContrasenia.aspx"
        End Get
    End Property

    Private Const Redirecciona As String = "Redirecciona"

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then

                ' si no es necesario cambiar la contraseña, nos vamos a la prinicipal
                If CType(Session(Entities.Usuario.SessionID), Entities.Usuario).CambiarContrasenia = 0 Then

                    Response.Redirect("Principal.aspx", False)

                End If

                ' pintamos nombre del usuario
                lblUsuario.Text = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).Nombre & _
                    " " & CType(Session(Entities.Usuario.SessionID), Entities.Usuario).Apellido & _
                    " " & CType(Session(Entities.Usuario.SessionID), Entities.Usuario).ApellidoAuxiliar

            End If

        End If

    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click

        ' obtenemos el error genérico, para en caso de cualquier error, colocar imagen, y si aplica, el mensaje
        Dim errores As New Entities.EtiquetaError(29)

        ' asignamos imagen
        'imgUnBotonAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta


        Try

            Dim usuarioCambio As New Entities.Usuario

            ' definimos datos para el proces0
            usuarioCambio.IdentificadorUsuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
            usuarioCambio.Contrasena = txtPassword.Text

            ' invocamos proceso
            If Not usuarioCambio.CambioContrasenia() Then

                ' si no se realizó, generamos excepción
                Throw New Exception

            End If


            ' actualizamos el campo para que no se pueda volver a entrar e ésta página manualmente en la sesion
            CType(Session(Entities.Usuario.SessionID), Entities.Usuario).CambiarContrasenia = 0


            ' redirigimos al flujo normal
            Response.Redirect("Principal.aspx", False)


        Catch ex As Entities.Usuario.ValidaContraseniaException

            ' asignamos mensaje de error desde capa de negocio
            Me.lblError.Text = ex.Message
            btnAceptarM1B1A.CommandName = ""

            ' registramos llamada a mensaje modal
            ScriptManager.RegisterStartupScript(btnAceptar, Me.GetType(), "MostramosMensajeAccion", "MostrarMensajeAccion();", True)


        Catch ex01 As Exception

            ' asignamos mensaje de error genérico
            Me.lblError.Text = errores.Descripcion
            btnAceptarM1B1A.CommandName = Redirecciona

            ' registramos llamada a mensaje modal
            ScriptManager.RegisterStartupScript(btnAceptar, Me.GetType(), "MostramosMensajeAccion", "MostrarMensajeAccion();", True)


        End Try

    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        ' redireccionamos a logout
        RedireccionaLogOut()

    End Sub

    Private Sub btnAceptarM1B1A_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptarM1B1A.Click

        If btnAceptarM1B1A.CommandName = Redirecciona Then

            ' redireccionamos a logout
            RedireccionaLogOut()

        End If


    End Sub

    ''' <summary>
    ''' Método que redirecciona a la página de salida del sistema
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RedireccionaLogOut()

        Response.Redirect("~/Logout.aspx", False)

    End Sub

End Class