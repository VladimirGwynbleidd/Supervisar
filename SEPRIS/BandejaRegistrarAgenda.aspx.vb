Imports Entities
Imports Negocio
Imports System.Web.Configuration
Imports Utilerias

Public Class BandejaRegistrarAgenda
    Inherits System.Web.UI.Page

#Region "Propiedades"

    Public Property Mensaje As String

    'Public ReadOnly Property EsAutorizador As Boolean
    '    Get
    '       Return CType(Session(Entities.Usuario.SessionID), Entities.Usuario).Autorizador
    '    End Get
    'End Property

#End Region

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For argument As Integer = 0 To gvConsulta.Rows.Count - 1
            ClientScript.RegisterForEventValidation(btnConsulta.UniqueID, argument)
        Next
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarImagenesEstatus()
            CargarFiltros()
            CargarBandeja()

            'If EsAutorizador Then
            '   btnAprobar.Visible = True
            'End If

            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                PersonalizaColumnas.IdentificadorGridView = 13
                PersonalizaColumnas.GridViewPersonalizar = gvConsulta
                PersonalizaColumnas.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
                PersonalizaColumnas.Personalizar()
            End If

        End If
    End Sub

#Region "Carga Datos"

    ''' <summary>
    ''' Carga las imagenes inferiores de significado de estatus y vigencia
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

        imgAprobado.ImageUrl = ObtenerImagenAutorizacion(True)
        imgRechazado.ImageUrl = ObtenerImagenAutorizacion(False)
        imgPendiente.ImageUrl = ObtenerImagenAutorizacion(DBNull.Value)

    End Sub

    ''' <summary>
    ''' Realiza la carga inicial de los filtros dinamicos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarFiltros()

        ucFiltro1.resetSession()

        Dim consulta As String = "1=1"
        'If Not EsAutorizador Then
        'consulta += " AND T_ID_INGENIERO_SOLICITANTE = '" + CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario + "' "
        'End If

        Dim objRegistroAgenda As New RegistroAgenda
        Dim dv As DataView = objRegistroAgenda.ObtenRegistrosAgenda()

        dv.RowFilter = consulta

        Dim dt As DataTable = dv.ToTable

        Dim objAutoriza = (From row In dt _
                                          Select T_ID_AUTORIZADOR = row.Field(Of String)("T_ID_AUTORIZADOR"), _
                                          AUTORIZADOR = row.Field(Of String)("AUTORIZADOR")).Distinct().ToList()

        Dim objEstatus() = {New With {.DSC_APROBADO = "Aprobado", .B_FLAG_APROBADO = 1},
                           New With {.DSC_APROBADO = "Rechazado", .B_FLAG_APROBADO = 0},
                           New With {.DSC_APROBADO = "Pendiente", .B_FLAG_APROBADO = 99}}

        ucFiltro1.AddFilter("Vigencia   ", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaBitDataSource, "Vigencia", "B_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, False, -1)
        ucFiltro1.AddFilter("ID   ", ucFiltro.AcceptedControls.TextBox, Nothing, "N_ID_REGISTRO_AGENDA", "N_ID_REGISTRO_AGENDA", ucFiltro.DataValueType.IntegerType, , , , , , , 8)
        ucFiltro1.AddFilter("Autoriza   ", ucFiltro.AcceptedControls.DropDownList, objAutoriza, "AUTORIZADOR", "T_ID_AUTORIZADOR", ucFiltro.DataValueType.StringType)
        ucFiltro1.AddFilter("Fecha de Inicio   ", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_INICIO_REGISTRO", "F_FECH_INICIO_REGISTRO", ucFiltro.DataValueType.StringType, False, , True)
        ucFiltro1.AddFilter("Fecha de Fin   ", ucFiltro.AcceptedControls.Calendar, Nothing, "F_FECH_FIN_REGISTRO", "F_FECH_FIN_REGISTRO", ucFiltro.DataValueType.StringType, False, , True)
        ucFiltro1.AddFilter("Estatus   ", ucFiltro.AcceptedControls.DropDownList, objEstatus, "DSC_APROBADO", "B_FLAG_APROBADO", ucFiltro.DataValueType.IntegerType)
        'If EsAutorizador Then
        '   Dim objRegistra = (From row In dt _
        '                                 Select T_ID_INGENIERO_SOLICITANTE = row.Field(Of String)("T_ID_INGENIERO_SOLICITANTE"), _
        '                                 SOLICITANTE = row.Field(Of String)("SOLICITANTE")).Distinct().ToList()
        'ucFiltro1.AddFilter("Registra   ", ucFiltro.AcceptedControls.DropDownList, objRegistra, "SOLICITANTE", "T_ID_INGENIERO_SOLICITANTE", ucFiltro.DataValueType.StringType)
        'ucFiltro1.AddFilterBottom("Solo míos   ", ucFiltro.AcceptedControls.CheckBox, Nothing, "SOLO_MIOS", "SOLO_MIOS", ucFiltro.DataValueType.BoolType, False, False, False, True, True, 0)
        'End If

        ucFiltro1.LoadDDL("BandejaRegistrarAgenda.aspx")

    End Sub

    ''' <summary>
    ''' Obtiene los filtros y carga los registros de la badeja
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarBandeja()

        Dim consulta As String = "1=1"
        For Each filtro In ucFiltro1.getFilterSelection
            If filtro.Contains("SOLO_MIOS") Then
                If Not filtro = "SOLO_MIOS=0" Then
                    consulta += " AND (T_ID_INGENIERO_SOLICITANTE = '" + CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario + "' " + _
                                " OR  T_ID_AUTORIZADOR= '" + CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario + "') "
                End If
                'ElseIf filtro.Contains("F_FECH_INICIO_REGISTRO") Then
                '    consulta += " AND " & filtro.Replace("BETWEEN", ">=").Replace("AND", "AND F_FECH_INICIO_REGISTRO <=")
                'ElseIf filtro.Contains("F_FECH_FIN_REGISTRO") Then
                '    consulta += " AND " & filtro.Replace("BETWEEN", ">=").Replace("AND", "AND F_FECH_FIN_REGISTRO <=")
            ElseIf filtro.Contains("B_FLAG_APROBADO") Then
                If filtro = "B_FLAG_APROBADO=99" Then
                    consulta += " AND B_FLAG_APROBADO is null "
                Else
                    consulta += " AND " + filtro
                End If
            Else
                consulta += " AND " + filtro
            End If
        Next

        'If Not EsAutorizador Then
        'consulta += " AND T_ID_INGENIERO_SOLICITANTE = '" + CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario + "' "
        'End If

        Dim objRegistroAgenda As New RegistroAgenda
        Dim dv As DataView = objRegistroAgenda.ObtenRegistrosAgenda()

        dv.RowFilter = consulta

        gvConsulta.DataSource = dv.ToTable()
        gvConsulta.DataBind()

        If gvConsulta.Rows.Count = 0 Then
            Noexisten.Visible = True
            gvConsulta.Visible = False
        Else
            Noexisten.Visible = False
            gvConsulta.Visible = True
        End If

    End Sub


#End Region

#Region "Controles"

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarBandeja()
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Response.Redirect("RegistrarAgenda.aspx", False)
    End Sub

    Private Sub btnModificar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        If gvConsulta.SelectedIndex = -1 Then
            Dim errores As New Entities.EtiquetaError(2050)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim objRegistroAgenda As New RegistroAgenda(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_REGISTRO_AGENDA").ToString()))

        If Not objRegistroAgenda.Vigente Then
            Dim errores As New Entities.EtiquetaError(2051)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        If objRegistroAgenda.IngenieroSolicta <> CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario Then
            Dim errores = New Entities.EtiquetaError(2057)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        If objRegistroAgenda.Aprobado AndAlso DateTime.Now >= objRegistroAgenda.FechIniReg Then
            Dim errores = New Entities.EtiquetaError(2064)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Session("IdRegistroAgenda") = objRegistroAgenda.Id
        Response.Redirect("RegistrarAgenda.aspx", False)

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        btnAceptarM2B1A.CommandArgument = "btnEliminar"
        Dim errores
        If gvConsulta.SelectedIndex = -1 Then
            errores = New Entities.EtiquetaError(2052)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim objRegistroAgenda As New RegistroAgenda(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_REGISTRO_AGENDA").ToString()))

        If Not objRegistroAgenda.Vigente Then
            errores = New Entities.EtiquetaError(2053)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        If objRegistroAgenda.IngenieroSolicta <> CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario Then
            errores = New Entities.EtiquetaError(2058)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        If objRegistroAgenda.Aprobado AndAlso objRegistroAgenda.FechIniReg <= DateTime.Now Then
            errores = New Entities.EtiquetaError(2065)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        errores = New Entities.EtiquetaError(2054)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Eliminar", "ConfirmacionEliminar();", True)

    End Sub

    Private Sub btnAprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAprobar.Click
        btnAceptarM2B1A.CommandArgument = "btnAprobar"
        Dim errores
        If gvConsulta.SelectedIndex = -1 Then
            errores = New Entities.EtiquetaError(2059)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim objRegistroAgenda As New RegistroAgenda(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_REGISTRO_AGENDA").ToString()))

        If Not objRegistroAgenda.Vigente Then
            errores = New Entities.EtiquetaError(2060)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        If Not IsNothing(objRegistroAgenda.Aprobado) Then
            errores = New Entities.EtiquetaError(2061)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        If objRegistroAgenda.Autorizador <> CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario Then
            Dim objUsuario As New Usuario(objRegistroAgenda.Autorizador)
            errores = New Entities.EtiquetaError(2062)
            Mensaje = errores.Descripcion.ToString.Replace("#APROBADOR#", objUsuario.Nombre + " " + objUsuario.Apellido + " " + objUsuario.ApellidoAuxiliar)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Session("EsAutorizacion") = True
        Session("IdRegistroAgenda") = objRegistroAgenda.Id
        Response.Redirect("RegistrarAgenda.aspx", False)

    End Sub

    Private Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptarM2B1A.Click
        Select Case btnAceptarM2B1A.CommandArgument
            Case "btnEliminar"
                Dim objRegistroAgenda As New RegistroAgenda(CInt(gvConsulta.DataKeys(gvConsulta.SelectedIndex)("N_ID_REGISTRO_AGENDA").ToString()))
                Dim objAgendar As New Agendar()
                Dim baja As Boolean = objAgendar.BajaRegistroAgenda(objRegistroAgenda)
                If Not baja Then
                    Dim errores As New Entities.EtiquetaError(2055)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                Else
                    EnviarCorreoBaja(objRegistroAgenda)
                End If
                CargarBandeja()
        End Select
    End Sub

    Private Sub EnviarCorreoBaja(ByVal objRegistroAgenda As RegistroAgenda)
        Dim objCorreo As New Entities.Correo(21)
        Dim objEmail As New Utilerias.Mail

        If objCorreo.Vigencia Then
            Dim destinatarios As List(Of String) = New List(Of String)
            If (Convert.ToBoolean(WebConfigurationManager.AppSettings("Desarrollo").ToString()) = True) Then
                destinatarios.Add("david.perez@softtek.com")
                destinatarios.Add("victor.leyva@softtek.com")
                destinatarios.Add("ivan.rivera@softtek.com")
            Else
                destinatarios.Add(New Usuario(objRegistroAgenda.IngenieroSolicta).Mail)
                If objRegistroAgenda.Autorizador <> Nothing AndAlso objRegistroAgenda.Autorizador <> "" Then
                    destinatarios.Add(New Usuario(objRegistroAgenda.Autorizador).Mail)
                End If
            End If
            Try
                objEmail.ServidorMail = WebConfigurationManager.AppSettings("MailServer").ToString()
                objEmail.Usuario = WebConfigurationManager.AppSettings("MailUsuario").ToString()
                objEmail.Password = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("MailPass"))
                objEmail.Dominio = WebConfigurationManager.AppSettings("MailDominio").ToString()
                objEmail.DireccionRemitente = WebConfigurationManager.AppSettings("MailUsuario").ToString()                
                objEmail.Mensaje = objCorreo.Cuerpo.Replace("[ACTIVIDAD]", objRegistroAgenda.NotaRegistro)
                objEmail.EsHTML = True
                objEmail.Destinatarios = destinatarios
                objEmail.NombreAplicacion = WebConfigurationManager.AppSettings("EventLogSource").ToString()
                objEmail.EventLogSource = "ENVIAR_EMAIL"
                objEmail.Enviar()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Session("IdRegistroAgenda") = CInt(gvConsulta.DataKeys(index)("N_ID_REGISTRO_AGENDA").ToString())
        Session("EsConsulta") = True

        Response.Redirect("RegistrarAgenda.aspx", False)

    End Sub

    Protected Sub btnExportaExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportaExcel.Click
        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns("B_FLAG_VIG").ColumnName = "Vigencia"
        dt.Columns("B_FLAG_APROBADO").ColumnName = "Estatus"

        utl.ExportaGrid(dt, gvConsulta, "Bandeja de Agenda", referencias)
    End Sub

    Private Sub gvConsulta_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting
        gvConsulta.Ordenar(e)
    End Sub

    Private Sub btnPersonalizarColumnas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPersonalizarColumnas.Click
        PersonalizaColumnas.IdentificadorGridView = 13
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.Mostrar()
    End Sub

    Protected Sub PersonalizaColumnas_event(ByVal sender As Object, ByVal e As EventArgs) Handles PersonalizaColumnas.FinPersonalizacion
        PersonalizaColumnas.IdentificadorGridView = 13
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.GuardarPersonalizacion()
        gvConsulta.DataSource = gvConsulta.DataSourceSession
        gvConsulta.DataBind()
    End Sub

#End Region


#Region "Metodos"

    ''' <summary>
    ''' Obtiene la imagen de Vigencia
    ''' </summary>
    ''' <param name="estatus">Indica el estado del registro</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenerImagenEstatus(ByVal estatus As Boolean) As String
        Dim img As Imagen
        If estatus Then
            img = New Imagen(77)
            Return "~/Imagenes/Errores/" + img.Ruta
        Else
            img = New Imagen(78)
            Return "~/Imagenes/Errores/" + img.Ruta
        End If
    End Function

    ''' <summary>
    ''' Obtiene la imagen de Autorizacion
    ''' </summary>
    ''' <param name="estatus">Estado del registro</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenerImagenAutorizacion(ByVal estatus As Object) As String
        Dim img As Imagen
        If IsDBNull(estatus) Then
            img = New Imagen(74)
            Return "~/Imagenes/Errores/" + img.Ruta
        ElseIf Convert.ToBoolean(estatus) Then
            img = New Imagen(75)
            Return "~/Imagenes/Errores/" + img.Ruta
        Else
            img = New Imagen(76)
            Return "~/Imagenes/Errores/" + img.Ruta
        End If

    End Function

#End Region

    Protected Sub gvConsulta_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvConsulta.PageIndexChanging
        gvConsulta.PageIndex = e.NewPageIndex
        gvConsulta.DataBind()
    End Sub

End Class