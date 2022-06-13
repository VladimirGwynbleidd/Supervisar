Imports System
Imports System.Web.Configuration
Imports System.DirectoryServices
Imports ADExtender

Public Class ActiveDirectory

    Public Shared Function EsValido(ByVal Usuario As String, ByVal Contrasena As String) As Boolean
        If (Convert.ToBoolean(WebConfigurationManager.AppSettings("Desarrollo").ToString()) = True) Then
            Return True
        Else
            Using Account As New AccountManagement.PrincipalContext(AccountManagement.ContextType.Domain, WebConfigurationManager.AppSettings("ActiveDirectoryDominio"), WebConfigurationManager.AppSettings("ActiveDirectoryUsuario"), Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("ActiveDirectoryPassEnc")))
                Dim Valido As Boolean = Account.ValidateCredentials(Usuario, Contrasena)
                Dim ValidTitle As Boolean = False
                Dim dcUser As Dictionary(Of String, String) = ObtenerUsuario(Usuario)
                If dcUser.Item("titulo") = WebConfigurationManager.AppSettings("ActiveDirectoryFiltroTitle") Then
                    ValidTitle = True
                End If
                If Valido AndAlso ValidTitle Then
                    Return True
                Else
                    Return False
                End If
            End Using
        End If
    End Function

    Public Shared Function ObtenerUsuario(ByVal usuario As String) As Dictionary(Of String, String)

        Dim datos As New Dictionary(Of String, String)

        Try
            Using Account As New AccountManagement.PrincipalContext(AccountManagement.ContextType.Domain, WebConfigurationManager.AppSettings("ActiveDirectoryDominio"), WebConfigurationManager.AppSettings("ActiveDirectoryUsuario"), Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("ActiveDirectoryPassEnc")))

                Dim k As UserPrincipalEx = UserPrincipalEx.FindByIdentity(Account, System.DirectoryServices.AccountManagement.IdentityType.SamAccountName, usuario)
                If Not IsNothing(k) Then
                    If IsNothing(k.GivenName) Then
                        datos.Add("nombre", "")
                    Else
                        datos.Add("nombre", k.GivenName)
                    End If

                    If IsNothing(k.Surname) Then
                        datos.Add("apellidos", "")
                    Else
                        datos.Add("apellidos", k.Surname)
                    End If

                    If IsNothing(k.VoiceTelephoneNumber) Then
                        datos.Add("telefono", "")
                    Else
                        datos.Add("telefono", k.VoiceTelephoneNumber)
                    End If

                    If IsNothing(k.EmailAddress) Then
                        datos.Add("mail", "")
                    Else
                        datos.Add("mail", k.EmailAddress)
                    End If

                    If IsNothing(k.Title) Then
                        datos.Add("titulo", "")
                    Else
                        datos.Add("titulo", k.Title)
                    End If
                Else
                    datos.Add("nombre", "")
                    datos.Add("apellidos", "")
                    datos.Add("telefono", "")
                    datos.Add("mail", "")
                    datos.Add("titulo", "")
                End If
            End Using
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        End Try

        Return datos

    End Function

    Public Shared Function ObtenerUsuarios() As DataTable


        Dim tblUsuarios As New DataTable()

        tblUsuarios.Columns.Add("Key", GetType(String))
        tblUsuarios.Columns.Add("Value", GetType(String))

        Try

            Using Account As New AccountManagement.PrincipalContext(AccountManagement.ContextType.Domain, WebConfigurationManager.AppSettings("ActiveDirectoryDominio"), WebConfigurationManager.AppSettings("ActiveDirectoryUsuario"), Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("ActiveDirectoryPassEnc")))
                Dim searcher As New AccountManagement.PrincipalSearcher()
                Dim user As New UserPrincipalEx(Account)

                user.Name = "*"
                user.Title = WebConfigurationManager.AppSettings("ActiveDirectoryFiltroTitle")

                searcher.QueryFilter = user

                Dim result As AccountManagement.PrincipalSearchResult(Of AccountManagement.Principal) = searcher.FindAll()

                For Each u As AccountManagement.Principal In result
                    Try
                        Dim k As AccountManagement.UserPrincipal = AccountManagement.UserPrincipal.FindByIdentity(Account, u.SamAccountName)
                        tblUsuarios.Rows.Add(k.SamAccountName, k.SamAccountName + " - " + k.Name + k.MiddleName)

                    Catch ex As Exception

                    End Try
                Next

            End Using

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento("Error al obtener usuarios de AD. " + ex.ToString(), EventLogEntryType.Error)
        End Try

        Return tblUsuarios

    End Function

    Public Shared Function ObtenerUsuarios(ByVal dtUserDB As DataTable) As DataTable


        Dim tblUsuarios As New DataTable()

        tblUsuarios.Columns.Add("Key", GetType(String))
        tblUsuarios.Columns.Add("Value", GetType(String))

        Try

            Using Account As New AccountManagement.PrincipalContext(AccountManagement.ContextType.Domain, WebConfigurationManager.AppSettings("ActiveDirectoryDominio"), WebConfigurationManager.AppSettings("ActiveDirectoryUsuario"), Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("ActiveDirectoryPassEnc")))
                Dim searcher As New AccountManagement.PrincipalSearcher()
                Dim user As New UserPrincipalEx(Account)

                user.Name = "*"
                user.Title = WebConfigurationManager.AppSettings("ActiveDirectoryFiltroTitle")

                searcher.QueryFilter = user

                Dim result As AccountManagement.PrincipalSearchResult(Of AccountManagement.Principal) = searcher.FindAll()

                For Each u As AccountManagement.Principal In result
                    Try
                        Dim k As AccountManagement.UserPrincipal = AccountManagement.UserPrincipal.FindByIdentity(Account, u.SamAccountName)
                        Dim result2() As DataRow = dtUserDB.Select("T_ID_USUARIO = '" + k.SamAccountName + "'")
                        If (result2.Count = 0) Then
                            tblUsuarios.Rows.Add(k.SamAccountName, k.SamAccountName + " - " + k.Name + k.MiddleName)
                        End If
                    Catch ex As Exception

                    End Try
                Next

            End Using

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento("Error al obtener usuarios de AD. " + ex.ToString(), EventLogEntryType.Error)
        End Try

        Return tblUsuarios

    End Function

End Class
