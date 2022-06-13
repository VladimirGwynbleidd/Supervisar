Option Strict On
Option Explicit On

Imports System.Net
Imports System.Net.Mail
Imports System.Diagnostics
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports Microsoft.Exchange.WebServices.Data
Imports System.Text
Imports System.Web
Imports System.Web.Configuration

Public Class Mail

    Private mDireccionFrom As String
    Private mCuerpoCorreo As String
    Private mAsuntoCorreo As String
    Private mEsHTML As Boolean
    Private mUsuario As String
    Private mPassword As String
    Private mDominio As String
    Private mServidorCorreo As String
    Private mArchivosAdjuntos As List(Of String)
    Private mDestinatarios As List(Of String)
    Private mConCopia As List(Of String)
    Private mConCopiaOculta As List(Of String)
    Private mReplyTo As String
    Private mNombreAplicacion As String
    Private mEventLogSource As String


    Public Property DireccionRemitente() As String
        Get
            Return mDireccionFrom
        End Get
        Set(ByVal value As String)
            mDireccionFrom = value
        End Set
    End Property


    ''' <summary>
    ''' Contenido del cuerpo del correo a enviar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Mensaje() As String
        Get
            Return mCuerpoCorreo
        End Get
        Set(ByVal value As String)
            mCuerpoCorreo = value
        End Set
    End Property

    ''' <summary>
    ''' Asunto del correo a enviar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Asunto() As String
        Get
            Return mAsuntoCorreo
        End Get
        Set(ByVal value As String)
            mAsuntoCorreo = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si el cuerpo del correo es formato HTML 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EsHTML As Boolean
        Get
            Return mEsHTML
        End Get
        Set(ByVal value As Boolean)
            mEsHTML = value
        End Set
    End Property

    ''' <summary>
    ''' Usuario del servidor de correo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Usuario As String
        Get
            Return mUsuario
        End Get
        Set(ByVal value As String)
            mUsuario = value
        End Set
    End Property

    ''' <summary>
    ''' Password del usuario del servidor de correo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Password As String
        Get
            Return mPassword
        End Get
        Set(ByVal value As String)
            mPassword = value
        End Set
    End Property

    ''' <summary>
    ''' Dominio del usuario del servidor de correo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Dominio As String
        Get
            Return mDominio
        End Get
        Set(ByVal value As String)
            mDominio = value
        End Set
    End Property

    ''' <summary>
    ''' Dirección IP del servidor de correo 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ServidorMail As String
        Get
            Return mServidorCorreo
        End Get
        Set(ByVal value As String)
            mServidorCorreo = value
        End Set
    End Property

    ''' <summary>
    ''' Lista de las rutas completas de los archivos adjuntos al correo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ArchivosAdjuntos As List(Of String)
        Get
            Return mArchivosAdjuntos
        End Get
        Set(ByVal value As List(Of String))
            mArchivosAdjuntos = value
        End Set
    End Property

    ''' <summary>
    ''' Dirección de correo electrónico de respuesta
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ReplyTo As String
        Get
            Return mReplyTo
        End Get
        Set(ByVal value As String)
            mReplyTo = value
        End Set
    End Property


    ''' <summary>
    ''' Lista de direcciones de correo electrónico de los destinatarios 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Destinatarios As List(Of String)
        Get
            Return mDestinatarios
        End Get
        Set(ByVal value As List(Of String))
            mDestinatarios = value
        End Set
    End Property

    ''' <summary>
    ''' Lista de direcciones de correo electrónico de los destinatarios con copia
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ConCopia As List(Of String)
        Get
            Return mConCopia
        End Get
        Set(ByVal value As List(Of String))
            mConCopia = value
        End Set
    End Property

    ''' <summary>
    ''' Lista de direcciones de correo electrónico de los destinatarios con copia oculta
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ConCopiaOculta As List(Of String)
        Get
            Return mConCopiaOculta
        End Get
        Set(ByVal value As List(Of String))
            mConCopiaOculta = value
        End Set
    End Property

    ''' <summary>
    ''' Nombre de la aplicación
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NombreAplicacion As String
        Get
            Return mNombreAplicacion
        End Get
        Set(ByVal value As String)
            mNombreAplicacion = value
        End Set
    End Property

    ''' <summary>
    ''' Nombre del Source con el que aparecerá en el visor de eventos el mensaje de bitacora de envío de correo electrónico
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EventLogSource As String
        Get
            Return mEventLogSource
        End Get
        Set(ByVal value As String)
            mEventLogSource = value
        End Set
    End Property

    Public Sub New()

        mServidorCorreo = WebConfigurationManager.AppSettings("MailServer").ToString()
        mUsuario = WebConfigurationManager.AppSettings("MailUsuario").ToString()
        mPassword = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("MailPass").ToString())
        mDominio = WebConfigurationManager.AppSettings("MailDominio").ToString()
        mDireccionFrom = WebConfigurationManager.AppSettings("MailCuenta").ToString()
    End Sub

    ''' <summary>
    ''' Constructor que recibe como parámetros los datos para conectarse al servidor de correo
    ''' </summary>
    ''' <param name="ServidorMail">Dirección IP del servidor de correo </param>
    ''' <param name="Usuario">Usuario del servidor de correo</param>
    ''' <param name="Password">Password del usuario del servidor de correo</param>
    ''' <param name="Dominio">Dominio del usuario del servidor de correo</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ServidorMail As String, ByVal Usuario As String, ByVal Password As String, ByVal Dominio As String)
        mServidorCorreo = ServidorMail
        mUsuario = Usuario
        mPassword = Password
        mDominio = Dominio
    End Sub

    Public Shared Function ValidateServerCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function

    ''' <summary>
    ''' Envia un correo electrónico utilizando Exchange
    ''' </summary>
    ''' <returns>Devuelve un valor booleano indica si se envío el correo exitosamente true, de lo contrario false</returns>
    ''' <remarks></remarks>
    Public Function Enviar() As Boolean
        Dim resultado As Boolean = False
        Dim servicio As ExchangeService
        Dim strMensaje As StringBuilder = Nothing
        Try
            ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)
            servicio = New ExchangeService()
            servicio.Credentials = New System.Net.NetworkCredential(mUsuario, mPassword, mDominio)

            If Not mServidorCorreo.Contains("http") Then
                mServidorCorreo = "https://" & mServidorCorreo
            End If
            servicio.Url = New Uri(mServidorCorreo & "/EWS/Exchange.asmx")
            Dim correo As EmailMessage
            correo = New EmailMessage(servicio)
            correo.Importance = Importance.High
            correo.From = mDireccionFrom
            correo.Subject = mAsuntoCorreo
            correo.Body = mCuerpoCorreo
            If mEsHTML Then
                correo.Body.BodyType = BodyType.HTML
            Else
                correo.Body.BodyType = BodyType.Text
            End If

            strMensaje = New StringBuilder("Aplicación desde la que se envío el correo electrónico: " & mNombreAplicacion & vbCrLf)
            strMensaje.Append("Destinatarios de correo electrónico:" & vbCrLf)

            For cic As Integer = 0 To mDestinatarios.Count - 1
                correo.ToRecipients.Add(mDestinatarios(cic).ToString)
                strMensaje.Append(mDestinatarios(cic).ToString & vbCrLf)
            Next

            If Not mConCopia Is Nothing Then
                For i As Integer = 0 To mConCopia.Count - 1
                    correo.CcRecipients.Add(mConCopia(i).ToString)
                    strMensaje.Append(mConCopia(i).ToString & vbCrLf)
                Next
            End If

            If Not mConCopiaOculta Is Nothing Then
                For reg As Integer = 0 To mConCopiaOculta.Count - 1
                    correo.BccRecipients.Add(mConCopiaOculta(reg).ToString)
                    strMensaje.Append(mConCopiaOculta(reg).ToString & vbCrLf)
                Next
            End If

            If Not mReplyTo = String.Empty Then
                correo.ReplyTo.Add(mReplyTo)
            End If

            If Not mArchivosAdjuntos Is Nothing Then
                For count As Integer = 0 To mArchivosAdjuntos.Count - 1
                    correo.Attachments.AddFileAttachment(mArchivosAdjuntos(count).ToString)
                Next
            End If
            strMensaje.Append("Asunto del correo electrónico: " & mAsuntoCorreo)
            correo.Send()
            ControlErrores.EscribirEvento(strMensaje.ToString, EventLogEntryType.Information)
            resultado = True
        Catch ex As Exception
            ControlErrores.EscribirEvento(strMensaje.ToString & vbCrLf & "FALLÓ EL ENVÍO DE CORREO ELECTRÓNICO. " & ex.Message, EventLogEntryType.Error)
            Throw New Exception("Ocurrió un error en la función EnviarCorreoExchange de la clase EnviarCorreo.", ex)
        End Try
        Return resultado
    End Function

End Class
