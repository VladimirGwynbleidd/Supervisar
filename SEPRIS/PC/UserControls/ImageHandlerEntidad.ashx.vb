Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging

Public Class ImageHandlerEntidad
    Implements System.Web.IHttpHandler

    Public ReadOnly Property IsReusable As Boolean Implements System.Web.IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

    Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest

        Dim _id As String = context.Request.QueryString("ide")
        Dim _ext As String = context.Request.QueryString("ext")
        Dim _imagen As Image
        Dim _ms As New MemoryStream()

        _imagen = System.Drawing.Image.FromFile(System.IO.Path.GetTempPath() & "ide_" & _id & "." & _ext.Trim("."))

        context.Response.ContentType = "image/gif"

        _imagen.Save(_ms, ImageFormat.Gif)
        _ms.WriteTo(context.Response.OutputStream)



    End Sub


End Class