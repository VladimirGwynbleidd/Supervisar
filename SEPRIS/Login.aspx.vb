'Fecha de creación:         11/07/2013
'Fecha de modificación:     
'Nombre del Responsable:    RSCE
'Empresa:                   Softtek
'Descrición:                Página de registro al sistema.

Imports System
Imports System.Web.Configuration

Public Class Login
    Inherits System.Web.UI.Page
    Public Property Mensaje As String

    Private ReadOnly Property Dominio As String
        Get
            Return WebConfigurationManager.AppSettings("ActiveDirectoryDominio")
        End Get
    End Property

    ''' <summary>
    ''' Propiedad que regresa el nombre de la página
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>SE EMPLEA EN LA MASTER PAGE</remarks>
    Public Shared ReadOnly Property Nombre As String
        Get
            Return "Login.aspx"
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            ConfigurarPagina()

            txtDominio.Text = Dominio
        End If

        'Dim objMasterUno As SiteInterno = Page.Master
        'Dim objMasterDos As SiteInterno = objMasterUno.Master

        'Dim lscad = objMasterUno.ppObjVisita.NombreAbogadoAsesor

        'objMasterUno.SetTexto("sss")
        'lsCad = objMasterUno.GetTexto()
    End Sub

    Protected Sub ConfigurarPagina()
        Select Case Conexion.SQLServer.Parametro.ObtenerValor("TipoLogin")
            Case "Interno"

                trRecuperar.Visible = False

            Case "Externo"

                trLblDomimio.Visible = False
                trTxtDominio.Visible = False

        End Select

    End Sub


    ''' <summary>
    ''' Limpia los campos de captura y mensaje.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        lblMensaje.Text = String.Empty
        txtUsuario.Text = String.Empty
        txtContrasena.Text = String.Empty

    End Sub

    ''' <summary>
    ''' Botón para realizar la validación del usuario.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        If String.IsNullOrEmpty(txtUsuario.Text.Trim) Or String.IsNullOrEmpty(txtContrasena.Text.Trim) Then
            lblMensaje.Text = "Debe Ingresar la Información de Usuario y Contraseña"
            Exit Sub
        End If

        txtUsuario.Text = txtUsuario.Text.ToLower.Trim

        Select Case Conexion.SQLServer.Parametro.ObtenerValor("TipoLogin")

            Case "Interno"

                If Conexion.ActiveDirectory.EsValido(txtUsuario.Text, txtContrasena.Text) Then

                    Dim usuario As New Entities.Usuario()
                    usuario.CargarDatos(txtUsuario.Text)

                    Dim dr As SqlClient.SqlDataReader = Nothing
                    Dim conexArea As New Conexion.SQLServer
                    'Dim parametroArea(0) As SqlClient.SqlParameter
                    'parametroArea(0) = New SqlClient.SqlParameter("@T_ID_USUARIO", txtUsuario.Text)
                    Dim datosaREA As New DataSet
                    Dim nuevos As New DataTable()
                    'datosaREA = conexArea.EjecutarSPConsultaDS("SP_BDS_GRL_ConsultaAreaUsuario", parametroArea)

                    nuevos = conexArea.ConsultarDT("Select UP.N_ID_PERFIL, ISNULL(A2.T_DSC_AREA,'Ninguno') AS T_ABR_AREA FROM BDS_R_GR_USUARIO_PERFIL UP LEFT JOIN dbo.BDS_D_VS_CONSECUTIVOS_AREAS A2 ON UP.I_ID_AREA = A2.I_ID_AREA WHERE T_ID_USUARIO = '" & txtUsuario.Text & "' AND UP.N_FLAG_VIG = 1")
                    Try
                        Session("ABR_AREA") = nuevos.Rows(0)("T_ABR_AREA")

                    Catch ex As Exception
                        Session("ABR_AREA") = ""
                    End Try
                    If Not IsNothing(conexArea) Then
                        conexArea.CerrarConexion()
                    End If
                    If dr IsNot Nothing Then
                        If Not dr.IsClosed Then
                            dr.Close() : dr = Nothing
                        End If
                    End If
                    'Session("ABR_AREA") = usuario.AbreArea

                    Session("ID_USR") = usuario.IdentificadorUsuario

                    If usuario.Vigente Then

                        'usuario.RegistrarSesion()
                        'Session(Entities.Usuario.SessionID) = usuario
                        'Dim objParam As New Entities.Parametros(50)
                        'Response.Redirect("~/" & objParam.ValorParametro, False)
                        usuario.RegistrarSesion()
                        Session(Entities.Usuario.SessionID) = usuario
                        'Response.Redirect("~/Principal.aspx")
                        'Response.Redirect("Principal.aspx")

                        ''BUSCA SI SE ESTA CONSULTANDO UNA VISITA DESDE EL CORREO O SE ESTA INGRESANDO DIRECTAMENTE AL LOGIN
                        ''SI YA VIENE UNA VISITA REDIRIGIR A  LA PAGINA DE DETALLE PARA ATENDERLA
                        If Not IsNothing(Request.QueryString("pr")) Then
                            Dim liIdVisitaGenerado As Integer = 0
                            If Int32.TryParse(Request.QueryString("pr"), liIdVisitaGenerado) Then
                                Session("ID_VISITA") = liIdVisitaGenerado
                                Response.Redirect("Procesos/DetalleVisita_V17.aspx")
                            End If
                        End If

                        Dim dt As DataTable = AccesoBD.AlertaVisitasPorVencer(txtUsuario.Text)
                        If dt.Rows.Count > 0 Then
                            Dim lsMsg As String = "<ul>"
                            For Each lrRow As DataRow In dt.Rows
                                lsMsg &= "<li> " & lrRow("MSG_ALERTA").ToString().Replace("||", "<br/>&emsp;&emsp;* ").Replace("|", "<br/>&emsp;&emsp;* ") & " </li>"
                            Next
                            lsMsg &= "</ul>"
                            Mensaje = lsMsg
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionLogin();", True)
                        Else
                            Response.Redirect("Principal.aspx", False)
                        End If


                        'NHM INICIA LOG
                        Dim strUsr As String
                        strUsr = "usuarioID: " + usuario.IdentificadorUsuario + ", perfil: " + usuario.IdentificadorPerfilActual.ToString() + ", área: " + usuario.IdArea.ToString() + ", vigencia: " + usuario.Vigente.ToString()
                        ''Utilerias.ControlErrores.EscribirEvento("USUARIO EN SESIÓN: " + strUsr, EventLogEntryType.Information)
                        'NHM FIN LOG

                    Else

                        lblMensaje.Text = "El usuario y/o contraseña son inválidos, por favor intente nuevamente."

                    End If

                Else

                    lblMensaje.Text = "El usuario y/o contraseña son inválidos, por favor intente nuevamente."

                End If

            Case "Externo"

                Dim usuario As New Entities.Usuario()
                If usuario.EsValido(txtUsuario.Text, txtContrasena.Text) Then

                    usuario.CargarDatos(txtUsuario.Text)
                    usuario.RegistrarSesion()
                    Session(Entities.Usuario.SessionID) = usuario

                    ' En caso que el usuario necesite cambiar contraseña
                    If usuario.CambiarContrasenia Then
                        Response.Redirect("CambioContrasenia.aspx", False)
                    Else

                        'Dim objParam As New Entities.Parametros(50)
                        'Response.Redirect("~/" & objParam.ValorParametro, False)
                        'Response.Redirect("Principal.aspx", False)
                        Response.Redirect("Visita/Bandeja.aspx", False)
                    End If

                    'NHM INICIA LOG
                    Dim strUsr As String
                    strUsr = "usuarioID: " + usuario.IdentificadorUsuario + ", perfil: " + usuario.IdentificadorPerfilActual.ToString() + ", área: " + usuario.IdArea.ToString() + ", vigencia: " + usuario.Vigente.ToString()
                    Utilerias.ControlErrores.EscribirEvento("USUARIO EN SESIÓN: " + strUsr, EventLogEntryType.Information)
                    'NHM FIN LOG

                Else

                    lblMensaje.Text = "El usuario y/o contraseña son inválidos, por favor intente nuevamente."

                End If

        End Select

    End Sub

    Protected Sub btnConfirmarDocsSI_Click(sender As Object, e As EventArgs)
        Response.Redirect("Principal.aspx", False)
        'Response.Redirect("Visita/Bandeja.aspx", False)
    End Sub
End Class