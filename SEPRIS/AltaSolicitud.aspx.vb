Imports Entities
Imports System.Web.Configuration
Imports System.Xml
Imports Utilerias
Imports System.Net

Public Class AltaSolicitud
    Inherits System.Web.UI.Page
    Public Property Mensaje As String
    Public Shared idSol As String
    Public Shared idPaquete As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not CBool(ViewState("CargarInicial")) Then
                'SE TUVO QUE AGREGAR ESTA VALIDACION PORQUE TENIA UN COMPORTAMIENTO EXTRAÑO CON EL BANNER DE JQUERY, AVECES NO DETECTABA EL POSTBACK
                ViewState("CargarInicial") = True
                Session("DocsAgr") = Nothing
                Dim table As New DataTable
                table.Columns.Add("IdSol", GetType(Integer))
                table.Columns.Add("NivelSol", GetType(Integer))
                table.Columns.Add("DescServ", GetType(String))
                table.Columns.Add("NomDoc", GetType(String))
                table.Columns.Add("BinDoc", GetType(Byte()))
                table.Columns.Add("EsNuevo", GetType(Integer))
                Session("DocsAgr") = table

                If Not Session("idPaquete") Is Nothing Then
                    idPaquete = Session("idPaquete").ToString
                    Session("idPaquete") = Nothing
                Else
                    idPaquete = Nothing
                End If
                CargarDatos()
                If idPaquete <> Nothing Then
                    CargarGridPaquete(idPaquete)
                    If idPaquete = "0" Then
                        trCarrusel.Visible = False
                        trBusqueda.Visible = False
                        btnAgregaTodos.Visible = False
                        btnRemueveTodos.Visible = False
                    Else
                        trCarrusel.Visible = True
                        trBusqueda.Visible = True
                        btnAgregaTodos.Visible = True
                        btnRemueveTodos.Visible = True
                    End If
                End If
            End If
        ElseIf Request.Params.Get("__EVENTARGUMENT").Equals("ItemSelected") Then
            acServicios_ItemSelected()
        End If
        CargarBanner()
    End Sub
    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        gvDisponibles.ArmaMultiScript()
        gvAgregados.ArmaMultiScript()
    End Sub

    Public Sub CargarDatos()
        Dim objTServicio As New TipoServicio
        ddlAplicativoDisp.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))
        pnlApp.Visible = False
        idSol = String.Empty
        CargaPosiblesAutorizadores()
        ddlFuncionario.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))
        If Not Session("idSolicitud") Is Nothing Then
            idSol = Session("idSolicitud").ToString
            Session("idSolicitud") = Nothing
            Dim objSolicitud As New Solicitud(Integer.Parse(idSol))
            ViewState("usuario") = objSolicitud.UsuarioSolicitante
            Dim usuario As New Usuario(objSolicitud.UsuarioSolicitante)
            txtUsuarioRegistro.Text = usuario.Nombre & " " & usuario.Apellido & " " & usuario.ApellidoAuxiliar
            txtUsuarioSolicitud.Text = objSolicitud.UsuarioSolicitud
            txtExtension.Text = objSolicitud.Extension
            ddlFuncionario.SelectedValue = objSolicitud.Autorizador
            txtNota.Text = objSolicitud.Notas
        Else
            'Nuevo registro
            Dim usuario As Usuario = TryCast(Session(Entities.Usuario.SessionID), Usuario)
            ViewState("usuario") = usuario.IdentificadorUsuario
            Dim objSolicitud As New Solicitud
            idSol = objSolicitud.ObtenerSiguienteIdentificador(usuario.IdentificadorUsuario).ToString()
            txtUsuarioRegistro.Text = usuario.Nombre & " " & usuario.Apellido & " " & usuario.ApellidoAuxiliar
            txtExtension.Text = usuario.Telefono
        End If
        CargarArchivosBD()
        CargarGridAgreg()
        If idPaquete <> "0" Then
            CargarGridDisp()
        End If
    End Sub

    Public Function ObtenerAreaUsuario(ByVal usuarioConsulta As String) As String
        Dim area As String = String.Empty
        Dim Usuario As String = WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("wsSicodPwd"))
        Dim Dominio As String = WebConfigurationManager.AppSettings("wsSicodDomain")
        Dim mycredentialCache As CredentialCache = New CredentialCache()
        Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
        Dim proxySICOD As New WR_SICOD.ws_SICOD
        proxySICOD.Credentials = credentials
        Dim dtArea As New DataTable
        dtArea = proxySICOD.GetUnidadAdministrativaUsuario(usuarioConsulta, WR_SICOD.UnidadAdministrativaTipo.Funcional)
        Try
            If dtArea IsNot Nothing Then
                If dtArea.Rows.Count > 0 Then
                    For Each r1 As DataRow In dtArea.Rows
                        area = r1("DSC_CODIGO_UNIDAD_ADM")
                    Next
                End If
            Else
            End If
        Catch ex As Exception
        End Try
        Return area
    End Function

    Protected Sub ddlAplicativoDisp_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAplicativoDisp.SelectedIndexChanged
        CargarGridDisp()
    End Sub
    Public Sub CargarGridDisp(Optional ByVal idNivel As Integer = -1)
        'Buscamos lo servicios asociados al aplicativo seleccionado segun el tipo de servicio y que NO han sido ya agregados a la solicitud de servicio
        Dim objNServicio As New NivelServicio
        objNServicio.IdentificadorTipoServicio = hdfImagen.Value.Substring(hdfImagen.Value.LastIndexOf("=") + 1)
        If pnlApp.Visible Then
            objNServicio.IdentificadorAplicativo = ddlAplicativoDisp.SelectedValue
        Else
            objNServicio.IdentificadorAplicativo = "-1"
        End If
        Dim listIndex As New List(Of Integer)
        Dim index As Integer = 0
        Dim tablaDis As DataTable = objNServicio.ObtenerNivelesDisp(idSol)
        If tablaDis.Rows.Count > 0 Then
            If Not gvAgregados.DataSourceSession Is Nothing Then
                Dim dt As DataTable = TryCast(gvAgregados.DataSourceSession, DataTable)
                If dt.Rows.Count > 0 Then
                    For Each dtrow As DataRow In dt.Rows
                        index = 0
                        For Each row As DataRow In tablaDis.Rows
                            If row("N_ID_NIVELES_SERVICIO") = dtrow("N_ID_NIVELES_SERVICIO") Then
                                listIndex.Add(index)
                            End If
                            index = index + 1
                        Next
                    Next
                End If
            End If
            For Each i As Integer In listIndex
                tablaDis.Rows(i).Delete()
            Next
            tablaDis.AcceptChanges()
        End If
        gvDisponibles.DataSource = tablaDis
        gvDisponibles.DataBind()

        If idNivel > 0 Then
            If gvDisponibles.Rows.Count > 0 Then
                For Each row As GridViewRow In gvDisponibles.Rows
                    If CInt(gvDisponibles.DataKeys(row.RowIndex)("N_ID_NIVELES_SERVICIO").ToString()) = idNivel Then
                        Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                        elemento.Checked = True
                    End If
                Next
            End If
        End If

        MuestraImagenes()
    End Sub
    Public Sub CargarGridAgreg(Optional ByVal limpiar As Boolean = False)
        'Buscamos lo servicios asociados al aplicativo seleccionado segun el tipo de servicio y que NO han sido ya agregados a la solicitud de servicio
        Dim objNServicio As New NivelServicio
        Dim tablaAgr As DataTable = objNServicio.ObtenerNivelesAgr(idSol)

        If limpiar Then
            tablaAgr.Clear()
        End If

        ddlServicios.DataSource = tablaAgr
        ddlServicios.DataValueField = "N_ID_NIVELES_SERVICIO"
        ddlServicios.DataTextField = "T_DSC_SERVICIO"
        ddlServicios.DataBind()
        ddlServicios.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))

        gvAgregados.DataSource = tablaAgr
        gvAgregados.DataBind()
        MuestraImagenes()
    End Sub
    Public Sub CargarGridPaquete(ByVal idPaquete As String)
        'Buscamos lo servicios asociados al aplicativo seleccionado segun el tipo de servicio y que NO han sido ya agregados a la solicitud de servicio
        Dim objNServicio As New NivelServicio
        Dim tablaAgr As DataTable = objNServicio.ObtenerNivelesPaquete(idPaquete)

        If idPaquete = "0" Then
            If idSol > 0 Then
                Dim objSolicitud As New Solicitud(CInt(idSol))
                Dim dvServicios As DataView = objSolicitud.ObtenerServiciosSolicitud
                If dvServicios.ToTable.Rows.Count > 0 Then
                    Dim idNivel As String = dvServicios.ToTable.Rows(0)("N_ID_NIVELES_SERVICIO").ToString
                    tablaAgr = objNServicio.ObtenerNivelesPaquete(idPaquete, idNivel)
                End If
            End If
            gvDisponibles.DataSource = tablaAgr
            gvDisponibles.DataBind()
        Else
            gvAgregados.DataSource = tablaAgr
            gvAgregados.DataBind()
            ddlServicios.DataSource = tablaAgr
            ddlServicios.DataValueField = "N_ID_NIVELES_SERVICIO"
            ddlServicios.DataTextField = "T_DSC_SERVICIO"
            ddlServicios.DataBind()
            ddlServicios.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))
        End If
        MuestraImagenes()
    End Sub
    Protected Sub Limpiar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Limpiar.Click
        txtExtension.Text = ""
        txtUsuarioSolicitud.Text = ""
        ddlFuncionario.SelectedValue = "-1"
        ddlAplicativoDisp.SelectedValue = "-1"
        txtNota.Text = ""
        txtServicios.Text = ""
        CargarGridAgreg(True)
        gvDisponibles.DataSource = Nothing
        gvDisponibles.DataBind()
        Dim table As DataTable = TryCast(Session("DocsAgr"), DataTable)
        table.Clear()
        Session("DocsAgr") = table
        gvConsultaSolicitudes.DataSource = table
        gvConsultaSolicitudes.DataBind()
        MuestraImagenes()
    End Sub

    Protected Sub btnAgrega_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgrega.Click
        Dim lstGVRow As List(Of GridViewRow) = gvDisponibles.SelectedMultiRows()
        Dim dtUsados As DataTable = TryCast(gvAgregados.DataSourceSession, DataTable)
        If (lstGVRow.Count > 0 AndAlso idPaquete <> "0") OrElse (lstGVRow.Count = 1 AndAlso idPaquete = "0" AndAlso dtUsados.Rows.Count = 0) Then
            Dim dtDisponibles As DataTable = TryCast(gvDisponibles.DataSourceSession, DataTable)
            Dim index As Integer = 0
            For Each gvRow In lstGVRow
                index = 0
                For Each row As DataRow In dtDisponibles.Rows
                    If Convert.ToInt32(gvRow.Cells(1).Text) = row("N_ID_NIVELES_SERVICIO") Then
                        Dim nRow As DataRow
                        If dtUsados Is Nothing Then
                            dtUsados = New DataTable
                            dtUsados.Columns.Add("N_ID_NIVELES_SERVICIO")
                            dtUsados.Columns.Add("T_DSC_ACRONIMO_APLICATIVO")
                            dtUsados.Columns.Add("T_DSC_SERVICIO")
                        End If
                        nRow = dtUsados.NewRow()
                        nRow("N_ID_NIVELES_SERVICIO") = row("N_ID_NIVELES_SERVICIO")
                        nRow("T_DSC_ACRONIMO_APLICATIVO") = row("T_DSC_ACRONIMO_APLICATIVO")
                        nRow("T_DSC_SERVICIO") = row("T_DSC_SERVICIO")
                        dtUsados.Rows.Add(nRow)
                        dtDisponibles.Rows(index).Delete()
                        dtDisponibles.AcceptChanges()
                        dtUsados.AcceptChanges()
                        Exit For
                    End If
                    index = index + 1
                Next
            Next

            ddlServicios.DataSource = dtUsados
            ddlServicios.DataValueField = "N_ID_NIVELES_SERVICIO"
            ddlServicios.DataTextField = "T_DSC_SERVICIO"
            ddlServicios.DataBind()
            ddlServicios.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))

            gvAgregados.DataSource = dtUsados
            gvAgregados.DataBind()
            gvDisponibles.DataSource = dtDisponibles
            gvDisponibles.DataBind()

            MuestraImagenes()
        ElseIf lstGVRow.Count > 1 AndAlso idPaquete = "0" Then
            Dim errores As New Entities.EtiquetaError(2088)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            Mensaje = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        ElseIf lstGVRow.Count = 1 AndAlso idPaquete = "0" AndAlso dtUsados.Rows.Count > 0 Then
            Dim errores As New Entities.EtiquetaError(2089)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            Mensaje = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        Else
            Dim errores As New Entities.EtiquetaError(1193)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            Mensaje = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If

    End Sub

    Protected Sub btnRemueve_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemueve.Click
        Dim lstGVRow As List(Of GridViewRow) = gvAgregados.SelectedMultiRows()
        If lstGVRow.Count > 0 Then
            Dim dtUsados As DataTable = TryCast(gvAgregados.DataSourceSession, DataTable)
            Dim dtDisponibles As DataTable = TryCast(gvDisponibles.DataSourceSession, DataTable)
            Dim index As Integer = 0
            Dim index2 As Integer = 0
            Dim table As DataTable = TryCast(Session("DocsAgr"), DataTable)
            For Each gvRow In lstGVRow
                index = 0
                For Each row As DataRow In dtUsados.Rows
                    If Convert.ToInt32(gvRow.Cells(1).Text) = row("N_ID_NIVELES_SERVICIO") Then
                        Dim nRow As DataRow
                        If dtDisponibles Is Nothing Then
                            dtDisponibles = New DataTable
                            dtDisponibles.Columns.Add("N_ID_NIVELES_SERVICIO")
                            dtDisponibles.Columns.Add("T_DSC_ACRONIMO_APLICATIVO")
                            dtDisponibles.Columns.Add("T_DSC_SERVICIO")
                        End If
                        nRow = dtDisponibles.NewRow()
                        nRow("N_ID_NIVELES_SERVICIO") = row("N_ID_NIVELES_SERVICIO")
                        nRow("T_DSC_ACRONIMO_APLICATIVO") = row("T_DSC_ACRONIMO_APLICATIVO")
                        nRow("T_DSC_SERVICIO") = row("T_DSC_SERVICIO")

                        'Eliminamos los documentos agregados temporalmente, los que ya estan en BD se agregaran cuando se de clic en aceptar

                        If table.Rows.Count > 0 Then
                            For i As Integer = table.Rows.Count - 1 To 0 Step -1
                                If row("N_ID_NIVELES_SERVICIO") = table.Rows(i)("NivelSol") And table.Rows(i)("EsNuevo") = 1 Then
                                    table.Rows(i).Delete()
                                    table.AcceptChanges()
                                End If
                            Next
                        End If
                        Session("DocsAgr") = table

                        dtDisponibles.Rows.Add(nRow)
                        dtUsados.Rows(index).Delete()
                        dtDisponibles.AcceptChanges()
                        dtUsados.AcceptChanges()
                        Exit For
                    End If
                    index = index + 1
                Next
            Next

            ddlServicios.DataSource = dtUsados
            ddlServicios.DataValueField = "N_ID_NIVELES_SERVICIO"
            ddlServicios.DataTextField = "T_DSC_SERVICIO"
            ddlServicios.DataBind()
            ddlServicios.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))

            gvAgregados.DataSource = dtUsados
            gvAgregados.DataBind()
            gvDisponibles.DataSource = dtDisponibles
            gvDisponibles.DataBind()

            table = TryCast(Session("DocsAgr"), DataTable)
            gvConsultaSolicitudes.DataSource = table
            gvConsultaSolicitudes.DataBind()

            MuestraImagenes()
        Else
            Dim errores As New Entities.EtiquetaError(1193)
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            Mensaje = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "MuestraMensajeUnBotonNoAccion();", True)
        End If
    End Sub
    ''' <summary>
    ''' Muestra y esconde las imagenes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraImagenes()
        If gvDisponibles.Rows.Count > 0 Then
            imgNoDisp.Visible = False
        Else
            imgNoDisp.Visible = True
        End If

        If gvAgregados.Rows.Count > 0 Then
            imgNoAgr.Visible = False
        Else
            imgNoAgr.Visible = True
        End If
        If gvConsultaSolicitudes.Rows.Count > 0 Then
            imgNoArch.Visible = False
        Else
            imgNoArch.Visible = True
        End If
    End Sub

    Public Sub CargarBanner()
        Dim objTServicio As New TipoServicio
        Dim seleccion As Boolean = False
        Dim seleccionFinal As Boolean = False
        Dim ltlSel As New LiteralControl
        Dim ltlSel2 As New LiteralControl
        Dim dtServicios As DataTable = objTServicio.ObtenerTServiciosImagen()
        pnlCarrusel.Controls.Add(New LiteralControl("<div id='carouselh'>"))
        For Each row As DataRow In dtServicios.Rows()
            If Not String.IsNullOrEmpty(hdfImagen.Value) Then
                If row("N_ID_TIPO_SERVICIO").ToString.Equals(hdfImagen.Value.Substring(hdfImagen.Value.LastIndexOf("=") + 1)) Then
                    ltlSel = New LiteralControl("<div><img alt='' src='Imagenes/Errores/" & row("T_DSC_RUTA_IMAGEN") & "?id=" & row("N_ID_TIPO_SERVICIO") & "' id='grupo_seleccionado' /><br />")
                    seleccion = True
                    seleccionFinal = True
                Else
                    pnlCarrusel.Controls.Add(New LiteralControl("<div><img alt='' src='Imagenes/Errores/" & row("T_DSC_RUTA_IMAGEN") & "?id=" & row("N_ID_TIPO_SERVICIO") & "' /><br />"))
                    seleccion = False
                End If
            Else
                pnlCarrusel.Controls.Add(New LiteralControl("<div><img alt='' src='Imagenes/Errores/" & row("T_DSC_RUTA_IMAGEN") & "?id=" & row("N_ID_TIPO_SERVICIO") & "' /><br />"))
                seleccion = False
            End If
            If seleccion Then
                ltlSel2 = New LiteralControl("<span class='thumbnail-text'>" & row("T_DSC_TIPO_SERVICIO") & "</span></div>")
            Else
                pnlCarrusel.Controls.Add(New LiteralControl("<span class='thumbnail-text'>" & row("T_DSC_TIPO_SERVICIO") & "</span></div>"))
            End If
        Next
        If seleccionFinal Then
            pnlCarrusel.Controls.Add(ltlSel)
            pnlCarrusel.Controls.Add(ltlSel2)
        End If
        pnlCarrusel.Controls.Add(New LiteralControl("</div>"))
    End Sub

    Protected Sub btnRemueveTodos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemueveTodos.Click
        Dim dtUsados As DataTable = TryCast(gvAgregados.DataSourceSession, DataTable)
        Dim dtDisponibles As DataTable = TryCast(gvDisponibles.DataSourceSession, DataTable)
        Dim index As Integer = 0
        Dim index2 As Integer = 0
        Dim table As DataTable = TryCast(Session("DocsAgr"), DataTable)
        For Each row As DataRow In dtUsados.Rows
            Dim nRow As DataRow
            If dtDisponibles Is Nothing Then
                dtDisponibles = New DataTable
                dtDisponibles.Columns.Add("N_ID_NIVELES_SERVICIO")
                dtDisponibles.Columns.Add("T_DSC_ACRONIMO_APLICATIVO")
                dtDisponibles.Columns.Add("T_DSC_SERVICIO")
            End If
            nRow = dtDisponibles.NewRow()
            nRow("N_ID_NIVELES_SERVICIO") = row("N_ID_NIVELES_SERVICIO")
            nRow("T_DSC_ACRONIMO_APLICATIVO") = row("T_DSC_ACRONIMO_APLICATIVO")
            nRow("T_DSC_SERVICIO") = row("T_DSC_SERVICIO")

            'Eliminamos los documentos agregados temporalmente, los que ya estan en BD se agregaran cuando se de clic en aceptar

            If table.Rows.Count > 0 Then
                For i As Integer = table.Rows.Count - 1 To 0 Step -1
                    If row("N_ID_NIVELES_SERVICIO") = table.Rows(i)("NivelSol") And table.Rows(i)("EsNuevo") = 1 Then
                        table.Rows(i).Delete()
                        table.AcceptChanges()
                    End If
                Next
            End If
            Session("DocsAgr") = table

            dtDisponibles.Rows.Add(nRow)
            dtDisponibles.AcceptChanges()
            index = index + 1
        Next
        dtUsados.Clear()

        ddlServicios.DataSource = dtUsados
        ddlServicios.DataValueField = "N_ID_NIVELES_SERVICIO"
        ddlServicios.DataTextField = "T_DSC_SERVICIO"
        ddlServicios.DataBind()
        ddlServicios.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))

        table = TryCast(Session("DocsAgr"), DataTable)
        gvConsultaSolicitudes.DataSource = table
        gvConsultaSolicitudes.DataBind()

        gvAgregados.DataSource = dtUsados
        gvAgregados.DataBind()
        gvDisponibles.DataSource = dtDisponibles
        gvDisponibles.DataBind()
        MuestraImagenes()
    End Sub

    Protected Sub btnAgregaTodos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregaTodos.Click
        Dim dtUsados As DataTable = TryCast(gvAgregados.DataSourceSession, DataTable)
        Dim dtDisponibles As DataTable = TryCast(gvDisponibles.DataSourceSession, DataTable)
        Dim index As Integer = 0
        For Each row As DataRow In dtDisponibles.Rows
            Dim nRow As DataRow
            If dtUsados Is Nothing Then
                dtUsados = New DataTable
                dtUsados.Columns.Add("N_ID_NIVELES_SERVICIO")
                dtUsados.Columns.Add("T_DSC_ACRONIMO_APLICATIVO")
                dtUsados.Columns.Add("T_DSC_SERVICIO")
            End If
            nRow = dtUsados.NewRow()
            nRow("N_ID_NIVELES_SERVICIO") = row("N_ID_NIVELES_SERVICIO")
            nRow("T_DSC_ACRONIMO_APLICATIVO") = row("T_DSC_ACRONIMO_APLICATIVO")
            nRow("T_DSC_SERVICIO") = row("T_DSC_SERVICIO")
            dtUsados.Rows.Add(nRow)
            dtUsados.AcceptChanges()
            index = index + 1
        Next
        dtDisponibles.Clear()

        ddlServicios.DataSource = dtUsados
        ddlServicios.DataValueField = "N_ID_NIVELES_SERVICIO"
        ddlServicios.DataTextField = "T_DSC_SERVICIO"
        ddlServicios.DataBind()
        ddlServicios.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))

        gvAgregados.DataSource = dtUsados
        gvAgregados.DataBind()
        gvDisponibles.DataSource = dtDisponibles
        gvDisponibles.DataBind()
        MuestraImagenes()
    End Sub

    Public Sub CargarArchivosBD()
        Dim resultado As New DataTable
        Dim objSolicitud As New Solicitud(Integer.Parse(idSol))
        Dim enc As Boolean = False
        Dim Dummy() As Byte = Nothing
        resultado = objSolicitud.ObtenerDocumentosBD()
        Dim table As DataTable = TryCast(Session("DocsAgr"), DataTable)
        If resultado.Rows.Count > 0 Then
            For Each dr As DataRow In resultado.Rows
                If table.Rows.Count > 0 Then
                    For Each dr2 As DataRow In table.Rows
                        If dr2("EsNuevo") = 0 And dr("T_DSC_ARCHIVO_ADJUNTO").Equals(dr2("NomDoc")) Then
                            enc = True
                        End If
                    Next
                    If Not enc Then
                        table.Rows.Add(CInt(dr("N_ID_SOLICITUD_SERVICIO")), CInt(dr("N_ID_NIVELES_SERVICIO")), CStr(dr("T_DSC_SERVICIO")), CStr(dr("T_DSC_ARCHIVO_ADJUNTO")), Dummy, 0)
                        Session("DocsAgr") = table
                    End If
                Else
                    table.Rows.Add(CInt(dr("N_ID_SOLICITUD_SERVICIO")), CInt(dr("N_ID_NIVELES_SERVICIO")), CStr(dr("T_DSC_SERVICIO")), CStr(dr("T_DSC_ARCHIVO_ADJUNTO")), Dummy, 0)
                    Session("DocsAgr") = table
                End If
            Next
        End If
        gvConsultaSolicitudes.DataSource = table
        gvConsultaSolicitudes.DataBind()
    End Sub

    'Esta función obtiene de Sharepoint todos los archivos asociados a la solicitud
    Public Function CargarArchivos() As DataTable
        Dim objSP As Utilerias.SharePointManager = New Utilerias.SharePointManager
        Dim querySharePoint = String.Empty
        Dim errorSHP As Boolean = False
        querySharePoint = "<Where>" +
                      "<Eq><FieldRef Name='NUM_SOLICITUD' /><Value Type='Number'>" + idSol + "</Value></Eq> " +
                      "</Where><OrderBy><FieldRef Name='ID' Ascending='True'></FieldRef>" +
                      "</OrderBy>"
        objSP.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSEDI").ToString()
        objSP.Biblioteca = WebConfigurationManager.AppSettings("SharePointBibliotecaSEDI").ToString()
        objSP.Usuario = WebConfigurationManager.AppSettings("SharePointUserSEDI").ToString()
        objSP.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SharePointPasswordSEDI"))
        objSP.Dominio = WebConfigurationManager.AppSettings("SharePointDomainSEDI").ToString()
        objSP.CondicionBusquedaArchivo = querySharePoint
        Dim bandeja As String = Nothing
        Dim seguimiento As String = Nothing
        Dim dt As DataTable
        dt = New DataTable()
        dt.Columns.Add("IdArch")
        dt.Columns.Add("NombreArchivo")
        dt.Columns.Add("N_ID_SOLICITUD_SERVICIO")
        dt.Columns.Add("NIVEL_SERVICIO")
        dt.Columns.Add("NOMBRE_ORIGINAL")
        Try
            Dim nodo As System.Xml.XmlNode = objSP.ConsultarDocumentos(False)
            Dim xmlDoc As XmlDocument
            xmlDoc = New XmlDocument()
            xmlDoc.LoadXml(nodo.OuterXml)
            Dim xmlNs As XmlNamespaceManager
            xmlNs = New XmlNamespaceManager(xmlDoc.NameTable)
            xmlNs.AddNamespace("z", "#RowsetSchema")
            Dim rows As XmlNodeList = xmlDoc.SelectNodes("//z:row", xmlNs)
            For Each row As XmlNode In rows
                Dim fila As DataRow = dt.NewRow()
                fila("IdArch") = Int32.Parse(row.Attributes("ows_ID").Value)
                fila("NombreArchivo") = row.Attributes("ows_LinkFilename").Value
                If row.Attributes("ows_NUM_SOLICITUD") Is Nothing Then
                    fila("N_ID_SOLICITUD_SERVICIO") = row.Attributes("ows_NUM_SOLICITUD").Value.Substring(0, row.Attributes("ows_NUM_SOLICITUD").Value.IndexOf("."))
                End If
                If (row.Attributes("ows_NOMBRE_ORIGINAL") Is Nothing) Then
                    fila("NOMBRE_ORIGINAL") = row.Attributes("ows_LinkFilename").Value
                Else
                    fila("NOMBRE_ORIGINAL") = row.Attributes("ows_NOMBRE_ORIGINAL").Value
                End If
                If Not row.Attributes("ows_NIVEL_SERVICIO") Is Nothing Then
                    fila("NIVEL_SERVICIO") = row.Attributes("ows_NIVEL_SERVICIO").Value
                End If
                dt.Rows.Add(fila)
            Next

        Catch ex As Exception
            'EventLogWriter.EscribeEntrada("Error al listar los documentos de SISAT: " + ex.ToString(), EventLogEntryType.Error)
            errorSHP = True
        Finally
            If errorSHP Then
                'lblLeyendaErrorSHP.Text = objFun.MensajeErrorSharePoint("ERR_SHP_ARCH_ASOC")
                'lblLeyendaErrorSHP.Visible = errorSHP
            End If
        End Try
        Return dt
    End Function

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click

        btnAceptarM2B1A.CommandArgument = "btnGuardar"
        If Not ValidaCampos() Then
            Exit Sub
        End If
        Dim objSolicitud As New Solicitud(Integer.Parse(idSol))
        If Not objSolicitud.Existe Then
            Dim errores As New Entities.EtiquetaError(1198)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(1199)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        If fupDocSolicitud.HasFile Then
            ViewState("archivo_nombre") = fupDocSolicitud.FileName
            ViewState("archivo_bin") = fupDocSolicitud.FileBytes
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
    End Sub
    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click

        Select Case btnAceptarM2B1A.CommandArgument

            Case "btnCancelar"
                Response.Redirect("CrearSolicitud.aspx")

            Case "btnGuardar"
                GuardarSolicitud()
                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)
            Case "btnEnviar"
                If idPaquete <> "0" Then
                    If GuardarSolicitud() Then
                        Dim objSolicitud As New Solicitud(Integer.Parse(idSol))
                        objSolicitud.IdEstatus = 11
                        objSolicitud.FechModificacion = DateTime.Now()
                        If Not objSolicitud.FolioAnio > 0 Then
                            objSolicitud.FolioAnio = Date.Now.Year
                        End If
                        If Not objSolicitud.IdFolio > 0 Then
                            objSolicitud.IdFolio = objSolicitud.ObtenerSiguienteConsecutivoAnio()
                        End If
                        If objSolicitud.CambiarEstatus() Then
                            If GenerarSolicitudPDF() Then
                                GuardarHistorial("Envío a autorización")
                                CorreoNotificacion()
                                Session("FolioEnviado") = objSolicitud.IdFolio.ToString & "-" & objSolicitud.FolioAnio.ToString
                            End If
                        End If
                    End If
                    btnAceptarM2B1A.CommandArgument = "btnCancelar"
                    btnAceptarM2B1A_Click(sender, e)
                Else
                    If GuardarSolicitud() Then
                        Dim objSolicitud As New Solicitud(Integer.Parse(idSol))
                        objSolicitud.IdEstatus = 2
                        objSolicitud.FechModificacion = DateTime.Now()
                        objSolicitud.FechRecepcion = DateTime.Now
                        objSolicitud.FechAprobacion = DateTime.Now
                        If Not objSolicitud.FolioAnio > 0 Then
                            objSolicitud.FolioAnio = Date.Now.Year
                        End If
                        If Not objSolicitud.IdFolio > 0 Then
                            objSolicitud.IdFolio = objSolicitud.ObtenerSiguienteConsecutivoAnio()
                        End If
                        objSolicitud.Actualizar()
                        If objSolicitud.CambiarEstatus Then
                            If GenerarSolicitudPDF() Then
                                GuardarHistorial("Generación de solicitud de soporte")
                                Session("FolioEnviado") = objSolicitud.IdFolio.ToString & "-" & objSolicitud.FolioAnio.ToString
                                If Conexion.SQLServer.Parametro.ObtenerValor("Agendar Soporte") = "Agendar" Then
                                    Agendar(objSolicitud, True)
                                Else
                                    Agendar(objSolicitud, False)
                                End If
                                GuardarHistorial("Solicitud agendada a los ingenieros")
                                NotificarIngeniero()
                            End If
                        End If
                    End If
                    btnAceptarM2B1A.CommandArgument = "btnCancelar"
                    btnAceptarM2B1A_Click(sender, e)
                    End If
            Case "imbBorrar"
                    If Not ViewState("NomDoc") Is Nothing Then
                        EliminarDocumento()
                    End If
            Case "btnAgregaDoc"
                    Dim objSolicitud As New Solicitud(Integer.Parse(idSol))
                    GuardarDocumentoTemp(objSolicitud)
                    ddlServicios.SelectedValue = "-1"
            Case ("btnCancelar")
                    Response.Redirect("CrearSolicitud.aspx")
        End Select

    End Sub

#Region "Agendar"
    Public Sub Agendar(ByVal obj As Solicitud, ByVal seAgenda As Boolean)
        Dim objNServicio As New NivelServicio
        Dim tablaAgr As DataTable = objNServicio.ObtenerNivelesAgr(obj.Identificador)
        Dim fechIni As DateTime = Nothing
        Dim fechFin As DateTime = Nothing
        Dim fechaValidar As DateTime = Nothing
        Dim contador As Integer = 0
        For Each dr As DataRow In tablaAgr.Rows
            Dim objAgenda As New Agenda()
            DeterminarFechaInicio(dr, objAgenda, fechIni, seAgenda)
            objAgenda.IdRegistroAgenda = objAgenda.ObtenerSiguienteIdentificadorRAgenda()
            objAgenda.IngSolict = obj.UsuarioSolicitante
            objAgenda.Autorizador = obj.Autorizador
            objAgenda.TipoRegistro = 1
            objAgenda.TipoActividad = 1
            objAgenda.TipoAusencia = -1
            objAgenda.Ciclica = 0
            objAgenda.FechIniReg = fechIni
            If Not seAgenda Then
                objAgenda.idConsecutivo = objAgenda.ObtenerConsecutivoSoporte
            End If
            objAgenda.NotaRegistro = "ATENCION DEL SERVICIO: " & CStr(dr("T_DSC_SERVICIO"))
            objAgenda.VigFlag = 1
            objAgenda.FechReg = obj.FechModificacion
            objAgenda.FechAutorizacion = obj.FechModificacion
            Dim final As Integer = 1
            If seAgenda Then
                final = CInt(dr("N_NUM_TIEMPO_EJECUCION"))
            End If
            objAgenda.IdNivelServicio = CInt(dr("N_ID_NIVELES_SERVICIO"))
            If objAgenda.AgregarRegistroAgenda() Then
                For hora As Integer = 1 To final
                    objAgenda.Identificador = objAgenda.ObtenerSiguienteIdentificador()
                    'El ingeniero ya lo trae asignado en el metodo DeterminarFechaInicioFin
                    objAgenda.FechaHoraTarea = fechIni
                    objAgenda.IdTipoTarea = 1
                    objAgenda.IdSolicitud = obj.Identificador
                    objAgenda.IdRegistroAgendaFK = objAgenda.IdRegistroAgenda
                    objAgenda.TareaAgenda = "ATENCION DEL SERVICIO: " & CStr(dr("T_DSC_SERVICIO"))
                    objAgenda.AgregarAgendaServicios()
                    'SE INSERTARÁ HORA POR HORA Y OBVIAMENTE CADA HORA SE VALIDA

                    fechaValidar = fechIni.AddHours(1)

                    fechFin = fechaValidar
                    fechIni = ValidaFechaFinal(fechaValidar, objAgenda.Ingeniero)
                Next
                If seAgenda Then
                    fechFin = ValidaFechaFinalDefinitiva(fechFin, objAgenda.Ingeniero)
                End If
                obj.NivelServicio = CInt(dr("N_ID_NIVELES_SERVICIO"))
                obj.FechaLimite = fechFin
                obj.TiempoEjecucion = CInt(dr("N_NUM_TIEMPO_EJECUCION"))
                obj.ActualizarServicioAgenda()
            End If
        Next
    End Sub
    Public Sub DeterminarFechaInicio(ByVal nivel As DataRow, ByRef objAgenda As Agenda, ByRef fechIni As DateTime, ByVal seAgenda As Boolean)
        Dim dt As New DataTable
        Dim fechaF1 As DateTime? = Nothing
        Dim fechaF2 As DateTime? = Nothing
        Dim fechaSel As DateTime? = Nothing
        Dim fech As String = String.Empty

        If seAgenda Then
            'OBTENER FECHA EN QUE SE LIBERA ING_RESP
            objAgenda.Ingeniero = CStr(nivel("ING_RESP"))
            dt = objAgenda.ObtenerFechaFinIngeniero()
            fechaF1 = ValidaFechaFinal(DateTime.Now.ToString("yyyy/MM/dd") & " " & DateTime.Now.AddHours(1).Hour.ToString & ":00:00", objAgenda.Ingeniero)

            If fechaF1.HasValue AndAlso fechaF1 < DateTime.Now Then
                fechaF1 = Nothing
                objAgenda.Ingeniero = Nothing
            End If

            'OBTENER FECHA EN QUE SE LIBERA ING_BACKUP 
            objAgenda.Ingeniero = CStr(nivel("ING_BACKUP"))
            fechaF2 = ValidaFechaFinal(DateTime.Now.ToString("yyyy/MM/dd") & " " & DateTime.Now.AddHours(1).Hour.ToString & ":00:00", objAgenda.Ingeniero)

            If fechaF2.HasValue AndAlso fechaF2 < DateTime.Now Then
                fechaF2 = Nothing
                objAgenda.Ingeniero = Nothing
            End If
        Else
            'Para flujos de soporte se busca si el ingeniero está ausente el día de la asignación, de lo contrario se asigna la tarea de acuerdo con el parámetro
            Dim horaRegistro As String = Conexion.SQLServer.Parametro.ObtenerValor("Hora Registro Soporte")
            Dim horaAtencion As String = Conexion.SQLServer.Parametro.ObtenerValor("Hora Atención Soporte")
            Dim fechaActual As DateTime = DateTime.Now
            Dim fechaRegistro As DateTime
            Dim fechaAtencion As DateTime

            DateTime.TryParse(CStr(fechaActual.Year.ToString & "/" & fechaActual.Month.ToString & "/" & fechaActual.Day.ToString & " " & horaRegistro), fechaRegistro)
            DateTime.TryParse(CStr(fechaActual.Year.ToString & "/" & fechaActual.Month.ToString & "/" & fechaActual.Day.ToString & " " & horaAtencion), fechaAtencion)

            If fechaActual > fechaRegistro Then
                fechaAtencion = fechaAtencion.AddDays(1)
                Do While Not Negocio.Agendar.EsDiaHabil(fechaAtencion)
                    fechaAtencion = fechaAtencion.AddDays(1)
                Loop
            End If

            objAgenda.Ingeniero = CStr(nivel("ING_RESP"))
            fechaF1 = fechaAtencion.AddHours(-1)
            Do Until Not objAgenda.AusenciaIngeniero(fechaF1)
                fechaF1 = fechaF1.Value.AddDays(1)
                Do While Not Negocio.Agendar.EsDiaHabil(fechaF1)
                    fechaF1 = fechaF1.Value.AddDays(1)
                Loop
            Loop

            objAgenda.Ingeniero = CStr(nivel("ING_BACKUP"))
            fechaF2 = fechaAtencion.AddHours(-1)
            Do Until Not objAgenda.AusenciaIngeniero(fechaF2)
                fechaF2 = fechaF2.Value.AddDays(1)
                Do While Not Negocio.Agendar.EsDiaHabil(fechaF2)
                    fechaF2 = fechaF2.Value.AddDays(1)
                Loop
            Loop

        End If

        'QUIEN SE DESOCUPE PRIMERO ATENDERA LA SOLICITUD
        If fechaF1.HasValue And fechaF2.HasValue Then
            If fechaF1 > fechaF2 Then
                objAgenda.Ingeniero = CStr(nivel("ING_BACKUP"))
                fechaSel = fechaF2
            Else
                objAgenda.Ingeniero = CStr(nivel("ING_RESP"))
                fechaSel = fechaF1
            End If
        ElseIf Not fechaF1.HasValue Then
            objAgenda.Ingeniero = CStr(nivel("ING_RESP"))
            fechaSel = fechaF1
        ElseIf Not fechaF2.HasValue Then
            objAgenda.Ingeniero = CStr(nivel("ING_BACKUP"))
            fechaSel = fechaF2
        End If

        Dim horaIni As Integer
        Dim diaIni As Date

        'SI YA EXISTE ALGUN REGISTRO DE FECHA TOMARA ESE COMO BASE, DE LO CONTRARIO TOMA LA HORA SIGUIENTE DE LA HORA ACTUAL
        If fechaSel.HasValue Then
            horaIni = Convert.ToDateTime(fechaSel).Hour
            diaIni = Convert.ToDateTime(fechaSel).Date
        Else
            horaIni = DateTime.Now.AddHours(1).Hour
            diaIni = DateTime.Now.Date
        End If

        'VALIDA LA FECHA SELECCIONADA Y REGRESA UNA HORA Y DIA CORRECTOS PARA ESTABLECERLOS COMO FECHA INICIAL Y POSTERIORMENTE VALIDA CONTRA LA AGENDA
        ValidaDiasHoras(diaIni, horaIni)
        fech = diaIni.ToString("yyyy/MM/dd") & " " & horaIni & ":00:00"
        fechIni = DateTime.Parse(fech)
        If seAgenda Then
            fechIni = ValidaAgenda(fechIni, objAgenda.Ingeniero)
        End If

    End Sub
    Public Function ValidaFechaFinalDefinitiva(ByVal fechaValidar As DateTime, ByVal ingeniero As String) As DateTime
        Dim hora As Integer = fechaValidar.Hour
        Dim dia As Date = fechaValidar.Date
        'VALIDA LA FECHA SELECCIONADA Y REGRESA UNA HORA Y DIA CORRECTOS PARA ESTABLECERLOS COMO FECHA INICIAL Y POSTERIORMENTE VALIDA CONTRA LA AGENDA
        ValidaDiasHorasFinal(dia, hora)
        Dim fechaTemp As String = dia.ToString("yyyy/MM/dd") & " " & hora & ":00:00"
        Dim fechaValida As DateTime = DateTime.Parse(fechaTemp)
        fechaValida = ValidaAgenda(fechaValida, ingeniero)
        Return fechaValida
    End Function
    Public Function ValidaFechaFinal(ByVal fechaValidar As DateTime, ByVal ingeniero As String) As DateTime
        Dim hora As Integer = fechaValidar.Hour
        Dim dia As Date = fechaValidar.Date
        'VALIDA LA FECHA SELECCIONADA Y REGRESA UNA HORA Y DIA CORRECTOS PARA ESTABLECERLOS COMO FECHA INICIAL Y POSTERIORMENTE VALIDA CONTRA LA AGENDA
        ValidaDiasHoras(dia, hora)
        Dim fechaTemp As String = dia.ToString("yyyy/MM/dd") & " " & hora & ":00:00"
        Dim fechaValida As DateTime = DateTime.Parse(fechaTemp)
        fechaValida = ValidaAgenda(fechaValida, ingeniero)
        Return fechaValida
    End Function
    Public Function ValidaAgenda(ByVal fecha As DateTime, ByVal ingeniero As String) As DateTime
        Dim hora As Integer
        Dim dia As Date
        Dim EstaDisponibleAg As Boolean = Agenda.FechaDisponibleAgenda(fecha, ingeniero)
        Do While Not EstaDisponibleAg
            hora = DateTime.Parse(fecha).AddHours(1).Hour
            dia = DateTime.Parse(fecha).Date
            ValidaDiasHoras(dia, hora)
            Dim fechaTemp As String = dia.ToString("yyyy/MM/dd") & " " & hora & ":00:00"
            fecha = DateTime.Parse(fechaTemp)
            EstaDisponibleAg = Agenda.FechaDisponibleAgenda(fecha, ingeniero)
        Loop
        Return fecha
    End Function
    Public Sub ValidaDiasHoras(ByRef diaIni As Date, ByRef horaIni As Integer)
        If horaIni > 13 And horaIni < 16 Then
            horaIni = 16
        ElseIf horaIni > 18 And horaIni < 24 Then
            horaIni = 9
            diaIni = diaIni.AddDays(1)
        ElseIf horaIni >= 0 And horaIni < 9 Then
            horaIni = 9
        End If

        Dim EsDiaHabil As Boolean = Negocio.Agendar.EsDiaHabil(diaIni)
        Do While Not EsDiaHabil
            diaIni = diaIni.AddDays(1)
            EsDiaHabil = Negocio.Agendar.EsDiaHabil(diaIni)
        Loop
    End Sub
    Public Sub ValidaDiasHorasFinal(ByRef diaFin As Date, ByRef horaFin As Integer)
        If horaFin > 14 And horaFin < 16 Then
            horaFin = 17
        ElseIf horaFin > 18 Then
            horaFin = 9
            diaFin = diaFin.AddDays(1)
        End If

        Dim EsDiaHabil As Boolean = Negocio.Agendar.EsDiaHabil(diaFin)
        Do While Not EsDiaHabil
            diaFin = diaFin.AddDays(1)
            EsDiaHabil = Negocio.Agendar.EsDiaHabil(diaFin)
        Loop
    End Sub
#End Region

    Public Function GuardarSolicitud() As Boolean
        Dim objSolicitud As New Solicitud(Integer.Parse(idSol))
        objSolicitud.UsuarioSolicitante = ViewState("usuario")
        If objSolicitud.FolioUsario < 1 Then
            objSolicitud.FolioUsario = objSolicitud.ObtenerSiguienteConsecutivoUsuario()
        End If
        objSolicitud.Extension = txtExtension.Text
        objSolicitud.UsuarioSolicitud = txtUsuarioSolicitud.Text
        objSolicitud.Autorizador = ddlFuncionario.SelectedValue
        objSolicitud.Notas = txtNota.Text
        objSolicitud.IdEstatus = 1
        objSolicitud.FechRegistro = Date.Now
        objSolicitud.FechModificacion = objSolicitud.FechRegistro
        objSolicitud.AreaSolicitante = ObtenerAreaUsuario(ViewState("usuario"))

        If Not objSolicitud.Existe Then
            objSolicitud.Identificador = objSolicitud.ObtenerSiguienteIdentificador(ViewState("usuario"))
            If objSolicitud.Agregar() Then
                If GuardarServicios(objSolicitud) Then
                    GuardarDocumentos(objSolicitud)
                    GuardarHistorial("Creación de la solicitud")
                    Return True
                End If
            End If
        Else
            If objSolicitud.Actualizar() Then
                If GuardarServicios(objSolicitud) Then
                    GuardarDocumentos(objSolicitud)
                    GuardarHistorial("Actualización de la solicitud")
                    Return True
                End If
            End If
        End If
        Return False
    End Function
    Public Sub GuardarHistorial(ByVal mensaje As String)
        Dim objHistorial As New HistorialSolicitud
        Dim usuario As Usuario = TryCast(Session(Entities.Usuario.SessionID), Usuario)
        Dim objAgenda As New Agenda()
        Dim fE As String = String.Empty
        Dim fP As String = String.Empty
        objAgenda.ObtenerFechasTermino(idSol, fE, fP)
        objHistorial.Identificador = objHistorial.ObtenerSiguienteIdentificador
        objHistorial.IdSolicitudConsecutivo = objHistorial.ObtenerSiguienteConsecutivo(idSol)
        objHistorial.IdSolicitud = idSol
        objHistorial.FechaRegistro = DateTime.Now
        objHistorial.IdUsuario = usuario.IdentificadorUsuario
        objHistorial.DescAccion = mensaje
        If Not String.IsNullOrEmpty(fE) Then
            objHistorial.FechaVencimiento = CDate(fE)
        ElseIf Not String.IsNullOrEmpty(fP) Then
            objHistorial.FechaVencimiento = CDate(fP)
        End If
        objHistorial.Agregar()
    End Sub

    Public Function ValidaCampos() As Boolean
        If String.IsNullOrWhiteSpace(txtExtension.Text) Or ddlFuncionario.SelectedValue.Equals("-1") Then
            Dim errores As New Entities.EtiquetaError(1190)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Return False
        End If
        Dim dt As DataTable = TryCast(gvAgregados.DataSourceSession, DataTable)
        If dt.Rows.Count = 0 Then
            Dim errores As New Entities.EtiquetaError(1191)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Return False
        End If
        Return True
    End Function

    Public Sub EliminarDocumento()
        Dim encTemp As Boolean = False
        Dim dt As DataTable = TryCast(Session("DocsAgr"), DataTable)
        If Not Session("DocsAgr") Is Nothing Then
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    If ViewState("NomDoc").Equals(dt.Rows(i)("NomDoc")) And dt.Rows(i)("EsNuevo") = 1 Then
                        dt.Rows(i).Delete()
                        dt.AcceptChanges()
                        encTemp = True
                        Exit For
                    End If
                Next
            End If
            Session("DocsAgr") = dt
        End If
        If Not encTemp Then
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    If ViewState("NomDoc").Equals(dt.Rows(i)("NomDoc")) And dt.Rows(i)("EsNuevo") = 0 Then
                        dt.Rows(i).Delete()
                        dt.AcceptChanges()
                        Dim objSolicitud As New Solicitud()
                        objSolicitud.Identificador = idSol
                        objSolicitud.NombreDocumento = ViewState("NomDoc")
                        objSolicitud.EliminarDocumento()
                        Exit For
                    End If
                Next
            End If
            Session("DocsAgr") = dt
        End If
        CargarArchivosBD()
        MuestraImagenes()
    End Sub
    Public Function GuardarServicios(ByVal objS As Solicitud) As Boolean
        Dim resultado As Boolean = True
        Dim muere As Boolean = True
        Dim nuevo As Boolean = True
        Dim objNServicio As New NivelServicio
        'resultado = objS.EliminarServicios()
        Try
            Dim dtTemp As DataTable = objNServicio.ObtenerNivelesAgr(objS.Identificador)
            If Not gvAgregados.DataSourceSession Is Nothing Then
                Dim dt As DataTable = TryCast(gvAgregados.DataSourceSession, DataTable)
                'Primero se borran los que ya no esten incluidos.
                If dt.Rows.Count > 0 Then
                    For Each rowTemp As DataRow In dtTemp.Rows
                        For Each dtrow As DataRow In dt.Rows
                            If CStr(rowTemp("N_ID_NIVELES_SERVICIO")).Equals(CStr(dtrow("N_ID_NIVELES_SERVICIO"))) Then
                                muere = False
                                Exit For
                            End If
                        Next
                        If muere Then
                            objS.NivelServicioDoc = rowTemp("N_ID_NIVELES_SERVICIO")
                            objS.NivelServicio = rowTemp("N_ID_NIVELES_SERVICIO")
                            If objS.TieneDocumentos(objS.Identificador, objS.NivelServicio) Then
                                objS.EliminarDocumentoID()
                            End If
                            objS.EliminarServicio()
                        Else
                            muere = True
                        End If
                    Next
                    'Posteriormente insertamos los nuevos que se hayan agregado
                    dtTemp = objNServicio.ObtenerNivelesAgr(objS.Identificador)
                    For Each dtrow As DataRow In dt.Rows
                        If dtTemp.Rows.Count > 0 Then
                            nuevo = True
                            For Each rowTemp As DataRow In dtTemp.Rows
                                If CStr(dtrow("N_ID_NIVELES_SERVICIO")).Equals(CStr(rowTemp("N_ID_NIVELES_SERVICIO"))) Then
                                    nuevo = False
                                    Exit For
                                End If
                            Next
                        Else
                            nuevo = True
                        End If
                        If nuevo Then
                            objS.NivelServicio = dtrow("N_ID_NIVELES_SERVICIO")
                            objS.EstatusServicio = 13
                            objS.GuardarServicios()
                        End If
                    Next
                Else
                    resultado = objS.EliminarDocumentos()
                    resultado = objS.EliminarServicios()
                End If
            End If
        Catch ex As Exception
            resultado = False
        End Try
        Return resultado
    End Function

    Public Sub GuardarDocumentoTemp(ByVal objS As Solicitud)
        If Not ViewState("archivo_nombre") Is Nothing And Not ViewState("archivo_bin") Is Nothing And Not ddlServicios.SelectedValue.Equals("-1") Then
            Dim table As DataTable = TryCast(Session("DocsAgr"), DataTable)
            table.Rows.Add(objS.Identificador, ddlServicios.SelectedValue, ddlServicios.SelectedItem, CStr(ViewState("archivo_nombre")), ViewState("archivo_bin"), 1)
            Session("DocsAgr") = table
            gvConsultaSolicitudes.DataSource = table
            gvConsultaSolicitudes.DataBind()
            MuestraImagenes()
        End If
    End Sub

    Public Function GuardarDocumentos(ByVal objS As Solicitud) As Boolean
        Dim resultado As Boolean = False
        Dim RutaArchivo As String = System.IO.Path.GetTempPath()
        Dim Nombre_Original As String = String.Empty
        Dim Nombre_Archivo As String = String.Empty

        Dim table As DataTable = TryCast(Session("DocsAgr"), DataTable)
        If table.Rows.Count > 0 Then
            For Each dr As DataRow In table.Rows
                'AGREGAR SOLO LOS QUE ESTEN GUARDADOS COMO TEMPORALES
                If dr("EsNuevo") = 1 Then
                    If Not dr("NomDoc") Is Nothing And Not dr("BinDoc") Is Nothing Then
                        Nombre_Original = TryCast(dr("NomDoc"), String)
                        Nombre_Archivo = Nombre_Original
                        Dim existe As String = objS.ConsultaNombreDocumento(Nombre_Original)
                        If Not String.IsNullOrEmpty(existe) Then
                            Dim Div_nombreArchivo() As String = Nombre_Original.Split(".")
                            Nombre_Archivo = ""
                            For cuenta As Int16 = 0 To Div_nombreArchivo.Count - 2
                                Nombre_Archivo = Nombre_Archivo & Div_nombreArchivo(cuenta)
                            Next
                            Nombre_Archivo = Nombre_Archivo & "[" & Date.Now().ToString("yyyyMMddHHmmss") & "]"
                            Nombre_Archivo = Nombre_Archivo & "." & Div_nombreArchivo(Div_nombreArchivo.Count - 1)
                        End If

                        Dim ShpManager As New Utilerias.SharePointManager()
                        ShpManager.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSEDI")
                        ShpManager.Dominio = WebConfigurationManager.AppSettings("SharePointDomainSEDI")
                        ShpManager.Usuario = WebConfigurationManager.AppSettings("SharePointUserSEDI")
                        ShpManager.Password = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SharePointPasswordSEDI"))
                        ShpManager.Biblioteca = WebConfigurationManager.AppSettings("SharePointBibliotecaSEDI")
                        ShpManager.NombreArchivo = Nombre_Archivo
                        ShpManager.BinFile = TryCast(dr("BinDoc"), Byte())
                        ShpManager.CargarArchivo()

                        Dim ID As String = ShpManager.ObtenerIdArchivo()
                        objS.NivelServicioDoc = CInt(dr("NivelSol"))
                        objS.IdDocumento = CInt(ID)
                        objS.NombreDocumento = Nombre_Archivo
                        ShpManager.StrBatch = "<Method ID='1' Cmd='Update'>" + _
                                                "<Field Name = 'ID'>" & ID & "</Field>" + _
                                                "<Field Name = 'NUM_SOLICITUD'>" & objS.Identificador & "</Field>" + _
                                                "<Field Name = 'NIVEL_SERVICIO'>" & objS.NivelServicioDoc & "</Field>" + _
                                                "<Field Name = 'NOMBRE_ORIGINAL'>" & Nombre_Original & "</Field>" + _
                                              "</Method>"
                        ShpManager.ActualizarListItems()
                        resultado = objS.GuardarDocumentos()
                    Else
                        resultado = True
                    End If
                End If
            Next
        End If
        Return resultado
    End Function

    Protected Sub gvConsultaSolicitudes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvConsultaSolicitudes.RowCommand
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        ViewState("NomDoc") = gvConsultaSolicitudes.DataKeys(index)("NomDoc").ToString()
        Dim errores
        If e.CommandName.Equals("Descargar") Then
            If Not ViewState("NomDoc") Is Nothing Then
                DescargarArchivo()
            End If
        ElseIf e.CommandName.Equals("Borrar") Then
            btnAceptarM2B1A.CommandArgument = "imbBorrar"
            errores = New Entities.EtiquetaError(1197)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Eliminar", "ConfirmacionEliminar();", True)
        End If
    End Sub

    Public Sub DescargarArchivo()
        Dim encTemp As Boolean = False
        If Not Session("DocsAgr") Is Nothing Then
            Dim dt As DataTable = TryCast(Session("DocsAgr"), DataTable)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    If CStr(ViewState("NomDoc")).Equals(dr("NomDoc")) And dr("EsNuevo") = 1 Then
                        'GENERAR ARCHIVO DEL BINARIO Y DESCARGARLO
                        Dim params As String() = CStr(dr("NomDoc")).Split(".")
                        Dim extension As String = params(1)
                        Dim Archivo() As Byte = dr("BinDoc")
                        Dim filename As String = "attachment; filename=" & Server.UrlPathEncode(CStr(dr("NomDoc")))
                        If Not Archivo Is Nothing Then
                            Select Case extension
                                Case "zip"
                                    Response.ContentType = "application/x-zip-compressed"
                                Case "pdf"
                                    Response.ContentType = "application/pdf"
                                Case "csv"
                                    Response.ContentType = "application/csv"
                                Case "doc"
                                    Response.ContentType = "application/doc"
                                Case "docx"
                                    Response.ContentType = "application/docx"
                                Case "xls"
                                    Response.ContentType = "application/xls"
                                Case "xlsx"
                                    Response.ContentType = "application/xlsx"
                                Case "png"
                                    Response.ContentType = "application/png"
                                Case "gif"
                                    Response.ContentType = "application/gif"
                                Case "jpg"
                                    Response.ContentType = "application/jpg"
                                Case "csv"
                                    Response.ContentType = "application/csv"
                                Case "txt"
                                    Response.ContentType = "application/txt"
                                Case "ppt"
                                    Response.ContentType = "application/vnd.ms-project"
                                Case "pptx"
                                    Response.ContentType = "application/vnd.ms-project"
                                Case "bmp"
                                    Response.ContentType = "image/bmp"
                                Case "tif"
                                    Response.ContentType = "image/tiff"
                            End Select
                            Response.AddHeader("content-disposition", filename)
                            Response.BinaryWrite(Archivo)
                            Response.End()
                        End If
                        encTemp = True
                        Exit For
                    End If
                Next
            End If
        End If
        If Not encTemp Then
            ObtenerArchivoSharePoint(CStr(ViewState("NomDoc")))
        End If
    End Sub

    Public Sub ObtenerArchivoSharePoint(ByVal nom_archivo As String)
        Dim cliente As System.Net.WebClient = New System.Net.WebClient
        Dim usuario As String
        Dim passwd As String
        Dim ServSharepoint As String
        Dim Dominio As String
        Dim Biblioteca As String
        Dim Url As String = String.Empty
        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim urlEncode As String
        Dim params As String() = nom_archivo.Split(".")
        Dim extension As String = params(1)

        Dim Archivo() As Byte = Nothing
        Dim filename As String = "attachment; filename=" & Server.UrlPathEncode(nom_archivo)
        Try
            ServSharepoint = WebConfigurationManager.AppSettings("SharePointServerSEDI")
            Biblioteca = WebConfigurationManager.AppSettings("SharePointBibliotecaSEDI")
            usuario = WebConfigurationManager.AppSettings("SharePointUserSEDI")
            passwd = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SharePointPasswordSEDI"))
            Dominio = WebConfigurationManager.AppSettings("SharePointDomainSEDI")

            cliente.Credentials = New NetworkCredential(usuario, passwd, Dominio)
            Url = ServSharepoint & "/" & Biblioteca & "/" & nom_archivo
            urlEncode = Server.UrlPathEncode(Url)
            Archivo = cliente.DownloadData(ResolveUrl(urlEncode))
        Catch ex As Exception
            'EventLogWriter.EscribeEntrada("Funcion ObtenerArchivoSharePoint - No se puede abrir el documento: " & nom_archivo, EventLogEntryType.Error)
        End Try
        If Not Archivo Is Nothing Then
            Select Case extension
                Case "zip"
                    Response.ContentType = "application/x-zip-compressed"
                Case "pdf"
                    Response.ContentType = "application/pdf"
                Case "csv"
                    Response.ContentType = "application/csv"
                Case "doc"
                    Response.ContentType = "application/doc"
                Case "docx"
                    Response.ContentType = "application/docx"
                Case "xls"
                    Response.ContentType = "application/xls"
                Case "xlsx"
                    Response.ContentType = "application/xlsx"
                Case "png"
                    Response.ContentType = "application/png"
                Case "gif"
                    Response.ContentType = "application/gif"
                Case "jpg"
                    Response.ContentType = "application/jpg"
                Case "csv"
                    Response.ContentType = "application/csv"
                Case "txt"
                    Response.ContentType = "application/txt"
                Case "ppt"
                    Response.ContentType = "application/vnd.ms-project"
                Case "pptx"
                    Response.ContentType = "application/vnd.ms-project"
                Case "bmp"
                    Response.ContentType = "image/bmp"
                Case "tif"
                    Response.ContentType = "image/tiff"
            End Select
            Response.AddHeader("content-disposition", filename)
            Response.BinaryWrite(Archivo)
            Response.End()
        End If
    End Sub

    'Caso de uso: 1.2.2	Notificar envío a autorización
    Private Function CorreoNotificacion() As Boolean
        Dim objSolicitud As New Solicitud(Integer.Parse(idSol))
        Dim continua As Boolean = False
        Dim resultado As Boolean = False
        Dim objCorreo As New Correo(1)
        Dim datosUsuario As New Dictionary(Of String, String)
        Dim objEmail As Utilerias.Mail
        objEmail = New Utilerias.Mail
        If objCorreo.Vigencia Then
            Dim destinatarios As List(Of String) = New List(Of String)
            If (Convert.ToBoolean(WebConfigurationManager.AppSettings("Desarrollo").ToString()) = True) Then
                destinatarios.Add("david.perez@softtek.com")
                destinatarios.Add("victor.leyva@softtek.com")
                destinatarios.Add("ivan.rivera@softtek.com")
            Else
                datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(CStr(ViewState("usuario")))
                destinatarios.Add(datosUsuario.Item("mail"))
                datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(ddlFuncionario.SelectedValue)
                destinatarios.Add(datosUsuario.Item("mail"))
            End If
            objEmail.ServidorMail = WebConfigurationManager.AppSettings("MailServer").ToString()
            objEmail.Usuario = WebConfigurationManager.AppSettings("MailUsuario").ToString()
            objEmail.Password = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("MailPass"))
            objEmail.Dominio = WebConfigurationManager.AppSettings("MailDominio").ToString()
            objEmail.DireccionRemitente = WebConfigurationManager.AppSettings("MailUsuario").ToString()
            objCorreo.Asunto = objCorreo.Asunto.Replace("[FOLIO]", objSolicitud.IdFolio.ToString + "-" + objSolicitud.FolioAnio.ToString)
            objEmail.Asunto = objCorreo.Asunto
            objEmail.EsHTML = True
            objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[FOLIO]", objSolicitud.IdFolio.ToString + "-" + objSolicitud.FolioAnio.ToString)
            objEmail.Mensaje = objCorreo.Cuerpo
            objEmail.Destinatarios = destinatarios
            objEmail.NombreAplicacion = WebConfigurationManager.AppSettings("EventLogSource").ToString()
            objEmail.EventLogSource = "ENVIAR_EMAIL"

            resultado = objEmail.Enviar()
        End If
        Return resultado
    End Function

    Public Function CalcularFechaTermino() As String
        Dim objAgenda As New Agenda()
        Dim fE As String = String.Empty
        Dim fP As String = String.Empty
        objAgenda.ObtenerFechasTermino(idSol, fE, fP)
        Return fE
    End Function

    Private Function NotificarIngeniero() As Boolean
        Dim objSolicitud As New Solicitud(Integer.Parse(idSol))
        Dim continua As Boolean = False
        Dim resultado As Boolean = False
        Dim objCorreo As New Entities.Correo(16)
        Dim objEmail As Utilerias.Mail
        Dim dtSubdirectores As DataTable = Nothing
        Dim dtIngenieros As DataTable = Nothing
        Dim datosUsuario As New Dictionary(Of String, String)
        objEmail = New Utilerias.Mail
        If objCorreo.Vigencia Then
            Dim destinatarios As List(Of String) = New List(Of String)
            If (Convert.ToBoolean(WebConfigurationManager.AppSettings("Desarrollo").ToString()) = True) Then
                destinatarios.Add("david.perez@softtek.com")
                destinatarios.Add("victor.leyva@softtek.com")
                destinatarios.Add("ivan.rivera@softtek.com")
            Else
                datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(objSolicitud.UsuarioSolicitante)
                destinatarios.Add(datosUsuario.Item("mail"))
                dtIngenieros = objSolicitud.DestinatariosIngenieros()
                If dtIngenieros.Rows.Count > 0 Then
                    For Each dr As DataRow In dtIngenieros.Rows
                        If Not destinatarios.Contains(dr("T_ID_INGENIERO")) Then
                            datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(dr("T_ID_INGENIERO"))
                            destinatarios.Add(datosUsuario.Item("mail"))
                        End If
                    Next
                End If
                dtSubdirectores = objSolicitud.DestinatariosSubdirectores()
                If dtSubdirectores.Rows.Count > 0 Then
                    For Each dr As DataRow In dtSubdirectores.Rows
                        If Not destinatarios.Contains(dr("T_ID_SUBDIRECTOR")) Then
                            datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(dr("T_ID_SUBDIRECTOR"))
                            destinatarios.Add(datosUsuario.Item("mail"))
                        End If
                        If Not destinatarios.Contains(dr("T_ID_BACKUP")) Then
                            datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(dr("T_ID_BACKUP"))
                            destinatarios.Add(datosUsuario.Item("mail"))
                        End If
                    Next
                End If
            End If
            Try
                objEmail.ServidorMail = WebConfigurationManager.AppSettings("MailServer").ToString()
                objEmail.Usuario = WebConfigurationManager.AppSettings("MailUsuario").ToString()
                objEmail.Password = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("MailPass"))
                objEmail.Dominio = WebConfigurationManager.AppSettings("MailDominio").ToString()
                objEmail.DireccionRemitente = WebConfigurationManager.AppSettings("MailUsuario").ToString()
                objCorreo.Asunto = objCorreo.Asunto.Replace("[FOLIO]", objSolicitud.IdFolio.ToString + "-" + objSolicitud.FolioAnio.ToString)
                objEmail.Asunto = objCorreo.Asunto
                objEmail.EsHTML = True
                objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[FOLIO]", objSolicitud.IdFolio.ToString + "-" + objSolicitud.FolioAnio.ToString)
                objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[F_FECH_ESTIMADA_TERMINO]", CalcularFechaTermino)
                objEmail.Mensaje = objCorreo.Cuerpo
                objEmail.Destinatarios = destinatarios
                objEmail.NombreAplicacion = WebConfigurationManager.AppSettings("EventLogSource").ToString()
                objEmail.EventLogSource = "ENVIAR_EMAIL"
                resultado = objEmail.Enviar()
            Catch ex As Exception

            End Try
        End If
        Return resultado
    End Function

    Protected Sub btnEnviar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEnviar.Click
        If Not ValidaCampos() Then
            Exit Sub
        End If
        btnAceptarM2B1A.CommandArgument = "btnEnviar"
        Dim errores As New Entities.EtiquetaError(1189)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        Dim errores As New Entities.EtiquetaError(2003)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
    End Sub

    Protected Sub lnkImagen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkImagen.Click
        Dim objNServicio As New NivelServicio
        objNServicio.IdentificadorTipoServicio = hdfImagen.Value.Substring(hdfImagen.Value.LastIndexOf("=") + 1)
        If objNServicio.IdentificadorTipoServicio.Equals("1") Then
            pnlApp.Visible = True
            Dim tabla As DataTable = objNServicio.ObtenerAplicativosPorNivel().ToTable
            ddlAplicativoDisp.DataSource = tabla
            ddlAplicativoDisp.DataBind()
            ddlAplicativoDisp.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))
        Else
            pnlApp.Visible = False
        End If
        CargarGridDisp()
    End Sub

    Private Sub CargaPosiblesAutorizadores()
        Dim Usuario As String = WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("wsSicodPwd"))
        Dim Dominio As String = WebConfigurationManager.AppSettings("wsSicodDomain")
        Dim mycredentialCache As CredentialCache = New CredentialCache()
        Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
        Dim proxySICOD As New WR_SICOD.ws_SICOD
        proxySICOD.Credentials = credentials
        Dim dsUDM As New DataSet
        Dim UsuarioActual As Usuario = TryCast(Session(Entities.Usuario.SessionID), Usuario)
        Dim objUsu As New Usuario
        Dim dtUsu As DataTable = objUsu.ObtenerAutorizadores()
        Dim dtAuto As New DataTable
        dtAuto.Columns.Add("T_ID_USUARIO")
        dtAuto.Columns.Add("T_DSC_NOMBRE_COMPLETO")
        'dsUDM = proxySICOD.GetAutorizadoresPosiblesSEDI(UsuarioActual.IdentificadorUsuario)
        If idPaquete = "0" Then
            dtUsu.DefaultView.RowFilter = "T_ID_USUARIO = 'sistema'"
            ddlFuncionario.DataSource = dtUsu
            ddlFuncionario.DataTextField = "T_DSC_NOMBRE_COMPLETO"
            ddlFuncionario.DataValueField = "T_ID_USUARIO"
            ddlFuncionario.DataBind()
            ddlFuncionario.SelectedValue = "sistema"
            ddlFuncionario.Enabled = False
        Else
            ddlFuncionario.Enabled = True
            Try
                If dsUDM IsNot Nothing Then
                    If dsUDM.Tables(0).Rows.Count > 0 Then
                        If dtUsu IsNot Nothing Then
                            If dtUsu.Rows.Count > 0 Then
                                For Each r1 As DataRow In dsUDM.Tables(0).Rows
                                    For Each r2 As DataRow In dtUsu.Rows
                                        If r1("usuario").Equals(r2("T_ID_USUARIO")) Then
                                            Dim nRow As DataRow = dtAuto.NewRow()
                                            nRow("T_ID_USUARIO") = r2("T_ID_USUARIO")
                                            nRow("T_DSC_NOMBRE_COMPLETO") = r2("T_DSC_NOMBRE") & " " & r2("T_DSC_APELLIDO") & " " & r2("T_DSC_APELLIDO_AUX")
                                            dtAuto.Rows.Add(nRow)
                                        End If
                                    Next
                                Next
                                dtAuto.AcceptChanges()
                                ddlFuncionario.DataSource = dtAuto
                                ddlFuncionario.DataTextField = "T_DSC_NOMBRE_COMPLETO"
                                ddlFuncionario.DataValueField = "T_ID_USUARIO"
                                ddlFuncionario.DataBind()
                            End If
                        End If
                    End If
                Else
                End If
            Catch ex As Exception
            Finally
            End Try
        End If
    End Sub

    Protected Sub btnAgregarDoc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregarDoc.Click
        If fupDocSolicitud.HasFile Then
            If Not ddlServicios.SelectedValue.Equals("-1") Then
                btnAceptarM2B1A.CommandArgument = "btnAgregaDoc"
                ViewState("archivo_nombre") = fupDocSolicitud.FileName
                ViewState("archivo_bin") = fupDocSolicitud.FileBytes
                Dim errores As New Entities.EtiquetaError(1195)
                Mensaje = errores.Descripcion
                imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
            Else
                Dim errores As New Entities.EtiquetaError(2049)
                Mensaje = errores.Descripcion
                imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
            End If
        Else
            Dim errores As New Entities.EtiquetaError(1196)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MuestraMensajeUnBotonNoAccion();", True)
        End If
    End Sub

    Public Function GenerarSolicitudPDF() As Boolean
        Dim resultado As Boolean = False
        Dim etiquetas As New Dictionary(Of String, String)
        etiquetas.Add("<LOGO1/>", System.Web.HttpContext.Current.Server.MapPath("/Imagenes/Encabezado.png"))
        Dim objParam As New Parametros(33)
        etiquetas.Add("<TITULO/>", objParam.ValorParametro)
        etiquetas.Add("<LOGO2/>", System.Web.HttpContext.Current.Server.MapPath("/Imagenes/logo_consar.png"))
        objParam = New Parametros(34)
        etiquetas.Add("<CLAVEDOC/>", objParam.ValorParametro)
        objParam = New Parametros(35)
        etiquetas.Add("<LEYENDA1/>", objParam.ValorParametro)
        objParam = New Parametros(36)
        etiquetas.Add("<LEYENDA2/>", objParam.ValorParametro)
        objParam = New Parametros(48)
        etiquetas.Add("<LEYENDA3/>", objParam.ValorParametro)
        objParam = New Parametros(49)
        etiquetas.Add("<LEYENDA4/>", objParam.ValorParametro)
        objParam = New Parametros(37)
        etiquetas.Add("<PIEPAGINA/>", objParam.ValorParametro)

        etiquetas.Add("<TITULITO1/>", "Datos del Solicitante:")
        etiquetas.Add("<TITULITO2/>", "Definición de la Solicitud:")

        Dim objSolicitud As New Solicitud(Integer.Parse(idSol))
        etiquetas.Add("<FECHASOL/>", objSolicitud.FechRegistro.ToString("D"))
        etiquetas.Add("<FOLIO/>", objSolicitud.IdFolio.ToString + "-" + objSolicitud.FolioAnio.ToString)
        etiquetas.Add("<AREA/>", objSolicitud.AreaSolicitante)
        Dim usuario As New Usuario(objSolicitud.Autorizador)
        etiquetas.Add("<FUNCIONARIO/>", usuario.Nombre & " " & usuario.Apellido & " " & usuario.ApellidoAuxiliar)
        usuario = New Usuario(objSolicitud.UsuarioSolicitante)
        etiquetas.Add("<USUARIOREGISTRA/>", usuario.Nombre & " " & usuario.Apellido & " " & usuario.ApellidoAuxiliar)
        etiquetas.Add("<EXTENSION/>", objSolicitud.Extension)
        etiquetas.Add("<USUARIOSOLICITUD/>", objSolicitud.UsuarioSolicitud)

        Dim htmlServicios As String = String.Empty
        Dim objNServicio As New NivelServicio
        Dim tablaAgr As DataTable = objNServicio.ObtenerNivelesAgr(idSol)
        If tablaAgr.Rows.Count > 0 Then
            htmlServicios = "<UL type = square>"
            For Each dr As DataRow In tablaAgr.Rows
                htmlServicios = htmlServicios + "<LI style='line-height:10px; font-size:8px;'><b>Clave:</b>" + dr("N_ID_NIVELES_SERVICIO").ToString() + " <b>Aplicativo:</b> " + dr("T_DSC_ACRONIMO_APLICATIVO") + " <b>Servicio:</b> " + dr("T_DSC_SERVICIO")
            Next
            htmlServicios = htmlServicios + "</UL>"
            etiquetas.Add("<SERVICIOS/>", htmlServicios)
        Else
            etiquetas.Add("<SERVICIOS/>", "N/A")
        End If

        Dim htmlDocumentos As String = String.Empty
        Dim tablaDocs As DataTable = objSolicitud.ObtenerDocumentosBD()
        If tablaDocs.Rows.Count > 0 Then
            htmlDocumentos = "<UL type = square>"
            For Each dr As DataRow In tablaDocs.Rows
                htmlDocumentos = htmlDocumentos + "<LI style='line-height:10px; font-size:8px;'><b>Nombre del archivo:</b>" + dr("T_DSC_ARCHIVO_ADJUNTO").ToString() + " <b>Servicio asociado al archivo:</b> " + dr("T_DSC_SERVICIO")
            Next
            htmlDocumentos = htmlDocumentos + "</UL>"
            etiquetas.Add("<DOCUMENTOS/>", htmlDocumentos)
        Else
            etiquetas.Add("<DOCUMENTOS/>", "N/A")
        End If
        If Not String.IsNullOrEmpty(objSolicitud.Notas) Then
            etiquetas.Add("<NOTAS/>", objSolicitud.Notas)
        Else
            etiquetas.Add("<NOTAS/>", "N/A")
        End If

        Dim PdfAcuse As String

        Dim PDFManager As New Utilerias.PDF
        PDFManager.EsSolicitud = True
        objParam = New Parametros(33)
        PDFManager.Titulo = objParam.ValorParametro
        objParam = New Parametros(34)
        PDFManager.ClaveDoc = objParam.ValorParametro
        PDFManager.FechaSol = objSolicitud.FechRegistro.ToString("D")
        PDFManager.Folio = objSolicitud.IdFolio.ToString + "-" + objSolicitud.FolioAnio.ToString
        PdfAcuse = PDFManager.CreateDocument(Server.MapPath("/Plantilla/PlantillaSolicitud.htm"), etiquetas, False, True)

        'Guardar solicitud generarada
        Try
            Dim ShpManager As New Utilerias.SharePointManager()
            ShpManager.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSEDI")
            ShpManager.Dominio = WebConfigurationManager.AppSettings("SharePointDomainSEDI")
            ShpManager.Usuario = WebConfigurationManager.AppSettings("SharePointUserSEDI")
            ShpManager.Password = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SharePointPasswordSEDI"))
            ShpManager.Biblioteca = WebConfigurationManager.AppSettings("SharePointBibliotecaSEDI")
            ShpManager.NombreArchivo = "SOLICITUD_TIC_" + objSolicitud.IdFolio.ToString() + "_" + objSolicitud.FolioAnio.ToString + ".pdf"
            ShpManager.BinFile = System.IO.File.ReadAllBytes(PdfAcuse)
            ShpManager.CargarArchivo()

            Dim ID As String = ShpManager.ObtenerIdArchivo()

            ShpManager.StrBatch = "<Method ID='1' Cmd='Update'>" + _
                                    "<Field Name = 'ID'>" & ID & "</Field>" + _
                                    "<Field Name = 'NUM_SOLICITUD'>" & idSol.ToString & "</Field>" + _
                                    "<Field Name = 'ES_SOLICITUD'>1</Field>" + _
                                  "</Method>"
            ShpManager.ActualizarListItems()
            resultado = True
        Catch ex As Exception
            resultado = False
        End Try
        Return resultado
    End Function

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        '-----------------------------------------SOLICITUD------------------------------
        Dim resultado As Boolean = False
        Dim etiquetas As New Dictionary(Of String, String)
        etiquetas.Add("<LOGO1/>", System.Web.HttpContext.Current.Server.MapPath("/Imagenes/Encabezado.png"))
        Dim objParam As New Parametros(33)
        etiquetas.Add("<TITULO/>", objParam.ValorParametro)
        etiquetas.Add("<LOGO2/>", System.Web.HttpContext.Current.Server.MapPath("/Imagenes/logo_consar.png"))
        objParam = New Parametros(34)
        etiquetas.Add("<CLAVEDOC/>", objParam.ValorParametro)
        objParam = New Parametros(35)
        etiquetas.Add("<LEYENDA1/>", objParam.ValorParametro)
        objParam = New Parametros(36)
        etiquetas.Add("<LEYENDA2/>", objParam.ValorParametro)
        objParam = New Parametros(48)
        etiquetas.Add("<LEYENDA3/>", objParam.ValorParametro)
        objParam = New Parametros(49)
        etiquetas.Add("<LEYENDA4/>", objParam.ValorParametro)
        objParam = New Parametros(37)
        etiquetas.Add("<PIEPAGINA/>", objParam.ValorParametro)
        etiquetas.Add("<TITULITO1/>", "Datos del Solicitante:")
        etiquetas.Add("<TITULITO2/>", "Definición de la Solicitud:")
        Dim objSolicitud As New Solicitud(32)
        etiquetas.Add("<FECHASOL/>", objSolicitud.FechRegistro.ToString("D"))
        etiquetas.Add("<FOLIO/>", objSolicitud.IdFolio.ToString + "-" + objSolicitud.FolioAnio.ToString)
        etiquetas.Add("<AREA/>", objSolicitud.AreaSolicitante)
        Dim usuario As New Usuario(objSolicitud.Autorizador)
        etiquetas.Add("<FUNCIONARIO/>", usuario.Nombre & " " & usuario.Apellido)
        usuario = New Usuario(objSolicitud.UsuarioSolicitante)
        etiquetas.Add("<USUARIOREGISTRA/>", usuario.Nombre & " " & usuario.Apellido)
        etiquetas.Add("<EXTENSION/>", objSolicitud.Extension)
        etiquetas.Add("<USUARIOSOLICITUD/>", objSolicitud.UsuarioSolicitud)
        Dim htmlServicios As String = String.Empty
        Dim objNServicio As New NivelServicio
        Dim tablaAgr As DataTable = objNServicio.ObtenerNivelesAgr(objSolicitud.Identificador.ToString())
        If tablaAgr.Rows.Count > 0 Then
            htmlServicios = "<UL type = square>"
            For Each dr As DataRow In tablaAgr.Rows
                htmlServicios = htmlServicios + "<LI style='line-height:10px; font-size:8px;'><b>Clave:</b>" + dr("N_ID_NIVELES_SERVICIO").ToString() + " <b>Aplicativo:</b> " + dr("T_DSC_ACRONIMO_APLICATIVO") + " <b>Servicio:</b> " + dr("T_DSC_SERVICIO")
            Next
            htmlServicios = htmlServicios + "</UL>"
            etiquetas.Add("<SERVICIOS/>", htmlServicios)
        Else
            etiquetas.Add("<SERVICIOS/>", "NO HAY")
        End If
        Dim htmlDocumentos As String = String.Empty
        Dim tablaDocs As DataTable = objSolicitud.ObtenerDocumentosBD()
        If tablaDocs.Rows.Count > 0 Then
            htmlDocumentos = "<UL type = square>"
            For Each dr As DataRow In tablaDocs.Rows
                htmlDocumentos = htmlDocumentos + "<LI style='line-height:10px; font-size:8px;'><b>Nombre del archivo:</b>" + dr("T_DSC_ARCHIVO_ADJUNTO").ToString() + " <b>Servicio asociado al archivo:</b> " + dr("T_DSC_SERVICIO")
            Next
            htmlDocumentos = htmlDocumentos + "</UL>"
            etiquetas.Add("<DOCUMENTOS/>", htmlDocumentos)
        Else
            etiquetas.Add("<DOCUMENTOS/>", "N/A")
        End If
        If Not String.IsNullOrEmpty(objSolicitud.Notas) Then
            etiquetas.Add("<NOTAS/>", objSolicitud.Notas)
        Else
            etiquetas.Add("<NOTAS/>", "N/A")
        End If
        Dim PdfAcuse As String
        Dim PDFManager As New Utilerias.PDF
        PDFManager.EsAutorizada = True
        PDFManager.FechaTimbre = DateTime.Now
        PDFManager.Certificado = "292233162870206001759766198388986002352876564528"
        PDFManager.Sello = "mV2prq3UlqmRwfIKj+mqdQ0/BFmiOAgK/WbCbWfOIeUA3vKNCV0CMAqrPu2tFd4nc/sZrxHeTTWyS7Dx/GC6Yllp3MzEzYBT6O6xWJyotZg/PHe9Yr8ihPz2cSmMXisqWBTN9ynd4kEA4="

        PDFManager.EsSolicitud = True
        objParam = New Parametros(33)
        PDFManager.Titulo = objParam.ValorParametro
        objParam = New Parametros(34)
        PDFManager.ClaveDoc = objParam.ValorParametro
        PDFManager.FechaSol = objSolicitud.FechRegistro.ToString("D")
        PDFManager.Folio = objSolicitud.IdFolio.ToString + "-" + objSolicitud.FolioAnio.ToString

        PdfAcuse = PDFManager.CreateDocument(Server.MapPath("/Plantilla/PlantillaSolicitud.htm"), etiquetas, False, True)
        '-----------------------------------------SOLICITUD------------------------------

        'Dim t3 As String = "CEER7412201I1"
        'Dim t4 As String = "292233162870206001759766198388986002352876564528"
        'Dim fechaTimbre As String = DateTime.Now.ToString
        'Dim objSolicitud As New Solicitud(30)
        'Dim usuario As New Usuario(objSolicitud.Autorizador)
        'Dim etiquetas As New Dictionary(Of String, String)
        'etiquetas.Add("<LOGO1/>", System.Web.HttpContext.Current.Server.MapPath("/Imagenes/Encabezado.png"))
        'etiquetas.Add("<LOGO2/>", System.Web.HttpContext.Current.Server.MapPath("/Imagenes/logo_consar.png"))
        'Dim objParam As New Parametros(43)
        'etiquetas.Add("<FECHA/>", String.Format(objParam.ValorParametro, DateTime.Now.Day.ToString, DateTime.Now.ToString("MMMM").ToUpper, DateTime.Now.Year.ToString))
        'objParam = New Parametros(44)
        'etiquetas.Add("<NOMBRE/>", String.Format(objParam.ValorParametro, usuario.Nombre & " " & usuario.Apellido & " " & usuario.ApellidoAuxiliar).ToUpper)
        'etiquetas.Add("<NOSOLICITUD/>", objSolicitud.IdFolio.ToString + "-" + objSolicitud.FolioAnio.ToString)
        'etiquetas.Add("<RFC/>", t3)
        'etiquetas.Add("<NOCERTIFICADO/>", t4)
        'etiquetas.Add("<FECHTIMBRE/>", fechaTimbre)
        'objParam = New Parametros(41)
        'etiquetas.Add("<LEYENDA1/>", objParam.ValorParametro)
        'objParam = New Parametros(42)
        'etiquetas.Add("<LEYENDA2/>", String.Format(objParam.ValorParametro, "<b>00001.P7S</b>", "<b>22 23 23 34 24 54 13 21 23 12</b>"))
        'Dim PdfAcuse As String
        'Dim PDFManager As New Utilerias.PDF
        'PDFManager.EsAutorizada = True
        'PDFManager.FechaTimbre = DateTime.Now
        'PDFManager.Certificado = "292233162870206001759766198388986002352876564528"
        'PDFManager.Sello = "mV2prq3UlqmRwfIKj+mqdQ0/BFmiOAgK/WbCbWfOIeUA3vKNCV0CMAqrPu2tFd4nc/sZrxHeTTWyS7Dx/GC6Yllp3MzEzYBT6O6xWJyotZg/PHe9Yr8ihPz2cSmMXisqWBTN9ynd4kEA4="
        'PdfAcuse = PDFManager.CreateDocument(Server.MapPath("/Plantilla/PlantillaAcuse.htm"), etiquetas, False, True)
        '----------------------------------------------ACUSE------------------------------

        'GENERAR ARCHIVO DEL BINARIO Y DESCARGARLO
        Dim extension As String = "pdf"
        Dim Archivo() As Byte = System.IO.File.ReadAllBytes(PdfAcuse)
        Dim filename As String = "attachment; filename=ACUSE_PDF.pdf"
        Select Case extension
            Case "pdf"
                Response.ContentType = "application/pdf"
        End Select
        Response.AddHeader("content-disposition", filename)
        Response.BinaryWrite(Archivo)
        Response.End()

    End Sub

    Protected Sub acServicios_ItemSelected()
        Try
            Dim dscNivel As String = txtServicios.Text.Trim
            Dim objNServicio As New NivelServicio(dscNivel)
            hdfImagen.Value = objNServicio.IdentificadorTipoServicio
            pnlCarrusel.Controls.Clear()
            'CargarBanner()
            If objNServicio.IdentificadorTipoServicio.Equals("1") Then
                pnlApp.Visible = True
                Dim tabla As DataTable = objNServicio.ObtenerAplicativosPorNivel().ToTable
                ddlAplicativoDisp.DataSource = tabla
                ddlAplicativoDisp.DataBind()
                ddlAplicativoDisp.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))
                ddlAplicativoDisp.SelectedValue = objNServicio.IdentificadorAplicativo.ToString
            Else
                pnlApp.Visible = False
            End If
            CargarGridDisp(objNServicio.Identificador)

        Catch ex As Exception

        End Try
    End Sub

End Class