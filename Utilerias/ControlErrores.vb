Option Strict On
Option Explicit On

Imports System
Imports System.Diagnostics
Imports System.Web.Configuration



''' <summary>
''' Controla los errores escribiendolos en el visor de eventos
''' </summary>
''' <remarks></remarks>
Public Class ControlErrores

    <System.Runtime.InteropServices.DllImport("C:\\WINDOWS\\System32\\advapi32.dll")> _
    Private Shared Function LogonUser(ByVal lpszUsername As String, ByVal lpszDomain As String, ByVal lpszPassword As String, _
        ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, ByRef phToken As Integer) As Boolean
    End Function

    ''' <summary>
    ''' Escribe un mensaje en el visor de eventos
    ''' </summary>
    ''' <param name="pMensaje">Mensaje que se desea escribir</param>
    ''' <param name="pTipoMensaje">Tipo de mensaje que se desea escribir</param>
    ''' <param name="pAplicacion">Nombre de la aplicación</param>
    ''' <param name="pEventLogSourceOpcional">Nombre complementario del source, este nombre se concatenará al que se recupera del WebConfig de la aplicación y es opcional</param>
    ''' <remarks></remarks>
    Public Overloads Shared Sub EscribirEvento(ByVal pMensaje As String, ByVal pTipoMensaje As EventLogEntryType, ByVal pAplicacion As String, ByVal pEventLogSourceOpcional As String)
        Dim eventLogSource As String
        Dim eventLogSitio As String
        Dim token As Integer
        Dim logon As Boolean
        Dim contexto As System.Security.Principal.WindowsImpersonationContext = Nothing

        '' Return

        ''Obtiene variables de configuración
        Try
            eventLogSource = WebConfigurationManager.AppSettings.Item("EventLogSource")
            eventLogSitio = WebConfigurationManager.AppSettings.Item("EventLogSitio")
        Catch ex As Exception
            eventLogSource = pAplicacion
            eventLogSitio = "Application"
        End Try

        If Not pEventLogSourceOpcional = String.Empty Then
            eventLogSource = eventLogSource & "-" & pEventLogSourceOpcional
        End If

        Dim dominio As String = WebConfigurationManager.AppSettings("dominioImp")
        Dim usuario As String = WebConfigurationManager.AppSettings("UsuarioImp")
        Dim passEnc As String = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("PassEncImp"))

        logon = LogonUser(usuario, dominio, passEnc, 9, 0, token)

        Dim token2 As IntPtr = New IntPtr(token)
        Dim identidad As System.Security.Principal.WindowsIdentity = New System.Security.Principal.WindowsIdentity(token2)
        contexto = identidad.Impersonate

        Try
            If Not EventLog.SourceExists(eventLogSource) Then
                EventLog.CreateEventSource(eventLogSource, eventLogSitio)
            End If
        Catch ex As Exception
            Console.WriteLine("ControlErrores.vb Error: " + ex.ToString())
            'Throw New Exception("Ocurrió un error en el método EscribirEvento de la clase ControlErrores.", ex)
        End Try

        Dim nombreAplicacion As String = WebConfigurationManager.AppSettings("NombreAplicacion")
        Dim mensaje As String = String.Format("Aplicación: {0} {1}{2}", nombreAplicacion, vbCrLf, pMensaje)
        'EventLog.WriteEntry(eventLogSource, mensaje, pTipoMensaje)

        ''Regresa la identidad del servicio a LocalSystem
        If Not contexto Is Nothing Then
            contexto.Undo()
        End If
    End Sub

    ''' <summary>
    ''' Escribe un mensaje en el visor de eventos
    ''' </summary>
    ''' <param name="pEventLogSitio">Cadena que indica el nombre del log donde se escribirá</param>
    ''' <param name="pEventLogSource">Cadena que indica la descripción del Source que se escribirá en el visor de eventos</param>
    ''' <param name="pMensaje">Mensaje que se desea escribir</param>
    ''' <param name="pTipoMensaje">Tipo de mensaje que se desea escribir</param>
    ''' <remarks></remarks>
    Public Overloads Shared Sub EscribirEvento(ByVal pEventLogSitio As String, ByVal pEventLogSource As String, ByVal pMensaje As String, ByVal pTipoMensaje As EventLogEntryType)
        Dim token As Integer
        Dim logon As Boolean
        Dim contexto As System.Security.Principal.WindowsImpersonationContext = Nothing

        Dim dominio As String = WebConfigurationManager.AppSettings("dominioImp")
        Dim usuario As String = WebConfigurationManager.AppSettings("UsuarioImp")
        Dim passEnc As String = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("PassEncImp"))

        logon = LogonUser(usuario, dominio, passEnc, 9, 0, token)

        Dim token2 As IntPtr = New IntPtr(token)
        Dim identidad As System.Security.Principal.WindowsIdentity = New System.Security.Principal.WindowsIdentity(token2)
        contexto = identidad.Impersonate

        Try
            If Not EventLog.SourceExists(pEventLogSource) Then
                EventLog.CreateEventSource(pEventLogSource, pEventLogSitio)
            End If
        Catch ex As Exception
            Throw New Exception("Ocurrió un error en el método EscribirEvento de la clase ControlErrores.", ex)
        End Try

        Dim nombreAplicacion As String = WebConfigurationManager.AppSettings("NombreAplicacion")
        Dim mensaje As String = String.Format("Aplicación: {0} {1}{2}", nombreAplicacion, vbCrLf, pMensaje)
        EventLog.WriteEntry(pEventLogSource, mensaje, pTipoMensaje)

        ''Regresa la identidad del servicio a LocalSystem
        If Not contexto Is Nothing Then
            contexto.Undo()
        End If
    End Sub

    Public Overloads Shared Sub EscribirEvento(ByVal Mensaje As String, ByVal TipoMensaje As EventLogEntryType)
        'Return

        Dim eventLogSource As String
        Dim eventLogSitio As String
        Dim token As Integer
        Dim logon As Boolean
        Dim contexto As System.Security.Principal.WindowsImpersonationContext = Nothing


        ''Obtiene variables de configuración

        eventLogSource = WebConfigurationManager.AppSettings.Item("EventLogSource")
        eventLogSitio = WebConfigurationManager.AppSettings.Item("EventLogSitio")
        
        Dim dominio As String = WebConfigurationManager.AppSettings("dominioImp")
        Dim usuario As String = WebConfigurationManager.AppSettings("UsuarioImp")
        Dim passEnc As String = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("PassEncImp"))


        logon = LogonUser(usuario, dominio, passEnc, 9, 0, token)

        Dim token2 As IntPtr = New IntPtr(token)
        Dim identidad As System.Security.Principal.WindowsIdentity = New System.Security.Principal.WindowsIdentity(token2)
        contexto = identidad.Impersonate

        Try
            If Not EventLog.SourceExists(eventLogSource) Then
                EventLog.CreateEventSource(eventLogSource, eventLogSitio)
            End If
        Catch ex As Exception
            Console.WriteLine("ControlErrores.vb Error: " + ex.ToString())
            ''Throw New Exception("Ocurrió un error en el método EscribirEvento de la clase ControlErrores.", ex)
        End Try

        Dim nombreAplicacion As String = WebConfigurationManager.AppSettings("NombreAplicacion")
        Mensaje = String.Format("Aplicación: {0} {1}{2}", nombreAplicacion, vbCrLf, Mensaje)
        ' EventLog.WriteEntry(eventLogSource, Mensaje, TipoMensaje)

        ''Regresa la identidad del servicio a LocalSystem
        If Not contexto Is Nothing Then
            contexto.Undo()
        End If
    End Sub


    ''' <summary>
    ''' Escribe un mensaje en el visor de eventos
    ''' </summary>
    ''' <param name="pEventLogSitio">Cadena que indica el nombre del log donde se escribirá</param>
    ''' <param name="pEventLogSource">Cadena que indica la descripción del Source que se escribirá en el visor de eventos</param>
    ''' <param name="pMensaje">Mensaje que se desea escribir</param>
    ''' <param name="pTipoMensaje">Tipo de mensaje que se desea escribir</param>
    ''' <param name="pEventLogSourceOpcional">Nombre complementario del source, este nombre se concatenará al que se recupera del WebConfig de la aplicación y es opcional</param>
    ''' <remarks></remarks>
    Public Overloads Shared Sub EscribirEvento(ByVal pEventLogSitio As String, ByVal pEventLogSource As String, ByVal pMensaje As String, ByVal pTipoMensaje As EventLogEntryType, ByVal pEventLogSourceOpcional As String)
        Dim eventLogSource As String
        Dim token As Integer
        Dim logon As Boolean
        Dim contexto As System.Security.Principal.WindowsImpersonationContext = Nothing

        eventLogSource = pEventLogSource
        If Not pEventLogSourceOpcional = String.Empty Then
            eventLogSource = eventLogSource & "-" & pEventLogSourceOpcional
        End If

        Dim dominio As String = WebConfigurationManager.AppSettings("dominioImp")
        Dim usuario As String = WebConfigurationManager.AppSettings("UsuarioImp")
        Dim passEnc As String = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("PassEncImp"))
        logon = LogonUser(usuario, dominio, passEnc, 9, 0, token)

        Dim token2 As IntPtr = New IntPtr(token)
        Dim identidad As System.Security.Principal.WindowsIdentity = New System.Security.Principal.WindowsIdentity(token2)
        contexto = identidad.Impersonate

        Try
            If Not EventLog.SourceExists(eventLogSource) Then
                EventLog.CreateEventSource(eventLogSource, pEventLogSitio)
            End If
        Catch ex As Exception
            Throw New Exception("Ocurrió un error en el método EscribirEvento de la clase ControlErrores.", ex)
        End Try

        Dim nombreAplicacion As String = WebConfigurationManager.AppSettings("NombreAplicacion")
        Dim mensaje As String = String.Format("Aplicación: {0} {1}{2}", nombreAplicacion, vbCrLf, pMensaje)
        EventLog.WriteEntry(eventLogSource, mensaje, pTipoMensaje)

        ''Regresa la identidad del servicio a LocalSystem
        If Not contexto Is Nothing Then
            contexto.Undo()
        End If
    End Sub
End Class

