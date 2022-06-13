Imports System.Web
Imports System.Web.Services

Public Class ReiniciaSesion
    Implements System.Web.IHttpHandler, IRequiresSessionState

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Try
            Dim objUsuario As New Entities.Usuario()
            objUsuario = context.Session(Entities.Usuario.SessionID)
            Dim lsUsu As String = ""

            If Not IsNothing(objUsuario) Then
                lsUsu = objUsuario.IdentificadorUsuario
                objUsuario.IdentificadorUsuario = "1232323"
                objUsuario.IdentificadorUsuario = lsUsu
            End If
        Catch : End Try
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class