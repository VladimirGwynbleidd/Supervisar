Imports System.Web.Configuration
Imports System.Net
Imports Clases
Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports System.Web.Script.Services

Public Class Bandeja1
    Inherits System.Web.UI.Page
    Dim enc As New YourCompany.Utils.Encryption.Encryption64
    Public Property Mensaje As String

    ''' <summary>
    ''' Guarda la visita padre
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property pIdVisitaPadre As Integer
        Get
            If IsNothing(ViewState("pIdVisitaPadre")) Then
                Return 0
            Else
                Return CInt(ViewState("pIdVisitaPadre"))
            End If
        End Get
        Set(value As Integer)
            Dim liAux As Integer = 0
            If Int32.TryParse(value, liAux) Then
                ViewState("pIdVisitaPadre") = liAux
            Else
                ViewState("pIdVisitaPadre") = 0
            End If
        End Set
    End Property

    Public Property pIdNumeroFilasOriginales As Integer
        Get
            If IsNothing(Session("pIdNumeroFilasOriginales")) Then
                Return 0
            Else
                Return CInt(Session("pIdNumeroFilasOriginales"))
            End If
        End Get
        Set(value As Integer)
            Dim liAux As Integer = 0
            If Int32.TryParse(value, liAux) Then
                Session("pIdNumeroFilasOriginales") = liAux
            Else
                Session("pIdNumeroFilasOriginales") = 0
            End If
        End Set
    End Property

    Public Property pIdNumeroFilasOriginalesPen As Integer
        Get
            If IsNothing(Session("pIdNumeroFilasOriginalesPen")) Then
                Return 0
            Else
                Return CInt(Session("pIdNumeroFilasOriginalesPen"))
            End If
        End Get
        Set(value As Integer)
            Dim liAux As Integer = 0
            If Int32.TryParse(value, liAux) Then
                Session("pIdNumeroFilasOriginalesPen") = liAux
            Else
                Session("pIdNumeroFilasOriginalesPen") = 0
            End If
        End Set
    End Property

    ''' <summary>
    ''' Id's de las columnas
    ''' </summary>
    ''' <remarks></remarks>
    Enum Columnas
        Folio = 1
        SubEntidad = 4
        Subvisitas = 18
        Editar = 19
        Borrar = 20
    End Enum

    'Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

    '    For argument As Integer = 0 To gvConsulta.Rows.Count - 1
    '        ClientScript.RegisterForEventValidation(btnConsulta.UniqueID, argument)
    '    Next

    '    MyBase.Render(writer)

    'End Sub
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        If Not IsNothing(Page) Then
            If Not IsNothing(Page.ClientScript) Then
                For argument As Integer = 0 To gvConsulta.Rows.Count - 1
                    Page.ClientScript.RegisterForEventValidation(btnConsulta.UniqueID, argument)
                Next

                For argument As Integer = 0 To gvConsultaPendientes.Rows.Count - 1
                    Page.ClientScript.RegisterForEventValidation(btnConsultaPendientes.UniqueID, argument)
                Next

            End If
        End If

        gvConsulta.ArmaMultiScript()
        gvConsultaPendientes.ArmaMultiScript()

        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            ''Validar si se requiere mostrar una subvisita
            If Not IsNothing(Request.QueryString("sb")) Then
                Int32.TryParse(Request.QueryString("sb"), pIdVisitaPadre)
                gvConsulta.Columns(Columnas.Subvisitas).Visible = False
                gvConsulta.Columns(Columnas.SubEntidad).Visible = True
                gvConsulta.Columns(Columnas.Editar).Visible = True
                gvConsulta.Columns(Columnas.Borrar).Visible = True
                gvConsulta.Columns(Columnas.Folio).HeaderText = "Subfolio"
                lbTituloBandeja.InnerText = "Bandeja de Subvisitas"
                btnRegresarBandeja.Visible = True
            Else
                pIdVisitaPadre = 0
                ''Ocular columna de subvisitas cuando es VO ya que nunca va a haber subvisitas
                If Not IsNothing(TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)) Then
                    If TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea = Constantes.AREA_VO Then
                        gvConsulta.Columns(Columnas.Subvisitas).Visible = False
                    End If
                End If
            End If

            CargarFiltros()
            CargarCatalogo()
            CargarCatalogoPendientes()
            pIdNumeroFilasOriginales = gvConsulta.Rows.Count
            pIdNumeroFilasOriginales = gvConsultaPendientes.Rows.Count

            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                PersonalizaColumnas.IdentificadorGridView = 2
                PersonalizaColumnas.GridViewPersonalizar = gvConsulta
                PersonalizaColumnas.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
                PersonalizaColumnas.Personalizar()

                OcultaColumnas()
            End If

            Dim usuario As New Entities.Usuario()
            usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

            ''Cargar imagen proceso ins sancion
            If usuario.IdArea = 36 Then
                imgProcesoVisitaAmbos.Visible = False
                imgProcesoVisita.Visible = True
                imgProcesoInspSancion.ImageUrl = "~/Imagenes/ProcesoInspSan/" & Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.NombreImagenProcesoInspeccionSancVF)
            ElseIf usuario.IdArea = 34 Or usuario.IdArea = 37 Then
                imgProcesoVisitaAmbos.Visible = True
                imgProcesoVisita.Visible = False
                imgProcesoInspSancionVF.ImageUrl = "~/Imagenes/ProcesoInspSan/" & Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.NombreImagenProcesoInspeccionSancVF)
                imgProcesoInspSancionOtras.ImageUrl = "~/Imagenes/ProcesoInspSan/" & Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.NombreImagenProcesoInspeccionSancOtras)
            Else
                imgProcesoVisitaAmbos.Visible = False
                imgProcesoVisita.Visible = True
                imgProcesoInspSancion.ImageUrl = "~/Imagenes/ProcesoInspSan/" & Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.NombreImagenProcesoInspeccionSancOtras)
            End If

        End If

    End Sub

    Public Function cargarEntidadesBandeja() As DataTable
        Dim dsEntidades As New DataSet
        Dim dtRes As New DataTable

        Dim Usuario As String = WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        Dim vecTipoEntidades As New List(Of Integer)

        vecTipoEntidades.Add(1)
        vecTipoEntidades.Add(7)

        Try
            Dim mycredentialCache As CredentialCache = New CredentialCache()
            Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
            Dim proxySICOD As New WR_SICOD.ws_SICOD
            proxySICOD.Credentials = credentials

            Dim IdEntidad As New DataColumn("IdEntidad")
            IdEntidad.DataType = GetType(Integer)

            Dim DscEntidad As New DataColumn("DscEntidad")
            DscEntidad.DataType = GetType(String)

            dtRes.Columns.Add(IdEntidad)
            dtRes.Columns.Add(DscEntidad)
            Dim liAuxId As Integer = 0

            For Each ent As Integer In vecTipoEntidades
                dsEntidades = proxySICOD.GetEntidadesComplete(ent)

                If Not IsNothing(dsEntidades) Then
                    If Not IsNothing(dsEntidades.Tables.Count > 0) Then
                        If Not IsNothing(dsEntidades.Tables(0)) Then
                            If Not IsNothing(dsEntidades.Tables(0).Rows.Count > 0) Then
                                For Each lrRow As DataRow In dsEntidades.Tables(0).Rows
                                    If Int32.TryParse(lrRow("cve_id_ent"), liAuxId) Then
                                        dtRes.Rows.Add(liAuxId, lrRow("siglas_ent").ToString().Trim())
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If
            Next

        Catch ex As Exception
            catch_cone(ex, "cargarEntidadesBandeja()")
        End Try

        Return dtRes
    End Function

    Private Sub catch_cone(ByVal e As Exception, ByVal s As String)
        EventLogWriter.EscribeEntrada("Funcion " & s & ": " & e.ToString(), EventLogEntryType.Error)
    End Sub

    Protected Sub imgCierraModalPendientes_Click(sender As Object, e As ImageClickEventArgs)
        mpeProcesa.Hide()
    End Sub

    Private Sub CargarFiltros()
        Dim objAccesoBD As New AccesoBD
        Dim dtObjetoVisita As DataTable = objAccesoBD.ObtenObjetoVisitaOri()
        Dim dtEstatusVencimiento As DataTable = AccesoBD.consultarEstatusVencimiento()

        Dim objUsuario As Entities.Usuario = Nothing
        If Not IsNothing(TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)) Then
            objUsuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)
        End If

        Dim dtDatosFiltro As DataSet
      Dim dtDatosFiltroPen As DataSet
      Dim dtFechaEnvioSansiones As New DataTable


      If Not IsNothing(objUsuario) Then
         dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.Bandeja, objUsuario.IdentificadorUsuario, objUsuario.IdArea)
         dtFechaEnvioSansiones = ConexionSISAN.ObtenerFechaSancionFiltro()
      Else
         dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.Bandeja, "", 0)
         dtFechaEnvioSansiones = ConexionSISAN.ObtenerFechaSancionFiltro()
      End If

        dtDatosFiltroPen = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.PendientesVisita, objUsuario.IdentificadorUsuario, 0)
        'dtDatosFiltroPen = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.PendientesPaso, objUsuario.IdentificadorUsuario, 0)
        ddlFolioVisita.ClearSelection()
      ddlPasoActual.ClearSelection()
      ddlPasoActualFin.ClearSelection()

        Dim vistaPasos As New DataView(dtDatosFiltroPen.Tables(0))
        Dim dtPasosUnicos As DataTable = vistaPasos.ToTable(True, "IdPasoActual")

        If Not IsNothing(dtDatosFiltroPen) Then
            If dtDatosFiltroPen.Tables.Count > 0 Then
                Utilerias.Generales.CargarCombo(ddlFolioVisita, dtDatosFiltroPen.Tables(0), "FolioVisita", "IdVisitaGenerado")
            Utilerias.Generales.CargarCombo(ddlPasoActual, dtPasosUnicos, "IdPasoActual", "IdPasoActual")
            Utilerias.Generales.CargarCombo(ddlPasoActualFin, dtPasosUnicos, "IdPasoActual", "IdPasoActual")
            End If
        End If

        ucFiltro1.resetSession()

        If Not IsNothing(Request.QueryString("sb")) Then
            ucFiltro1.AddFilter("Subentidad", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(9), "DscSubEntidad", "IdSubEntidad", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)
            ucFiltro1.AddFilter("Subfolio", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(8), "FolioVisita", "IdVisitaGenerado", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)
        End If

        ucFiltro1.AddFilter("Estatus Visita", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(7), "EstatusVisitaDsc", "EstatusVisitaInt", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Vigencia  ", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSource, "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, False, -1)
        ''ucFiltro1.AddFilter("Folio", ucFiltro.AcceptedControls.TextBox, Nothing, "", "FolioVisita", ucFiltro.DataValueType.StringType, False, True, False, , , , 255)
        ucFiltro1.AddFilter("Folio", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(6), "FolioVisita", "IdVisitaGenerado", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Entidad", ucFiltro.AcceptedControls.TextBox, Nothing, "", "DscEntidad", ucFiltro.DataValueType.StringType, False, True, False, , , , 255)
        ucFiltro1.AddFilter("Entidad", ucFiltro.AcceptedControls.DropDownList, cargarEntidadesBandeja(), "DscEntidad", "IdEntidad", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Vicepresidencia", ucFiltro.AcceptedControls.TextBox, Nothing, "", "DscArea", ucFiltro.DataValueType.StringType, False, True, False, , , , 255)
        ucFiltro1.AddFilter("Vicepresidencia", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(4), "DscArea", "IdArea", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Tipo de visita", ucFiltro.AcceptedControls.TextBox, Nothing, "", "DscTipoVisita", ucFiltro.DataValueType.StringType, False, True, False, , , , 255)
        ucFiltro1.AddFilter("Tipo de visita", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(3), "DscTipoVisita", "IdTipoVisita", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("# Paso actual", ucFiltro.AcceptedControls.TextBox, Nothing, "", "StrIdPasoActual", ucFiltro.DataValueType.IntegerType, False, True, False, , , , 255)
      ucFiltro1.AddFilter("Rango de Pasos", ucFiltro.AcceptedControls.DropDownListR, dtDatosFiltro.Tables(2), "DscPasoActual", "StrIdPasoActual", ucFiltro.DataValueType.RangeType, False, False, False, , False, "", 50)

      'ucFiltro1.AddFilter("", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(2), "DscPasoActual", "StrIdPasoActual", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

      'ucFiltro1.AddFilter("al # Paso", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(2), "DscPasoActual", "StrIdPasoActual", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Responsable de inspección", ucFiltro.AcceptedControls.TextBox, Nothing, "", "NombreInspectorResponsable", ucFiltro.DataValueType.StringType, False, True, False, , , , 255)
        ucFiltro1.AddFilter("Responsable de Inspección", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(0), "NombreInspectorResponsable", "IdInspectorResponsable", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        ucFiltro1.AddFilter("Inspector Operativo", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(10), "NombreInspector", "IdInspector", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        ucFiltro1.AddFilter("Abogado Supervisor Asesor", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(11), "DscAbogado", "IdAbogado", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)
        ucFiltro1.AddFilter("Abogado Asesor", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(12), "DscAbogado", "IdAbogado", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        ucFiltro1.AddFilter("Abogado Supervisor Sanciones", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(13), "DscAbogado", "IdAbogado", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        ucFiltro1.AddFilter("Abogado Supervisor Contencioso", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(14), "DscAbogado", "IdAbogado", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)
        ucFiltro1.AddFilter("Abogado Contencioso", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(15), "DscAbogado", "IdAbogado", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Abogado sanciones", ucFiltro.AcceptedControls.TextBox, Nothing, "", "NombreAbogadoSancion", ucFiltro.DataValueType.StringType, False, True, False, , , , 255)
        ucFiltro1.AddFilter("Abogado Sanciones", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(1), "DscAbogado", "IdAbogado", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Objeto Visita", ucFiltro.AcceptedControls.DropDownList, dtObjetoVisita, "DscObjetoVisita", "DscObjetoVisita", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)
        ucFiltro1.AddFilter("Estatus Vencimiento", ucFiltro.AcceptedControls.DropDownList, dtEstatusVencimiento, "dscEstatusSemaforo", "IdEstatusSemaforo", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        ucFiltro1.AddFilter("# Días transcurridos en el paso actual", ucFiltro.AcceptedControls.TextBox, Nothing, "", "DiasTranscurridos", ucFiltro.DataValueType.StringType, False, True, False, , , , 255)
        ucFiltro1.AddFilter("Fecha inicia visita", ucFiltro.AcceptedControls.Calendar, Nothing, "", "FechaInicioVisita", ucFiltro.DataValueType.StringType, False, False, True, False, False, Date.Today, 50)
        ucFiltro1.AddFilter("Fecha de Registro", ucFiltro.AcceptedControls.Calendar, Nothing, "", "FechaRegistroD", ucFiltro.DataValueType.StringType, False, False, True, True, False, Date.Today.AddMonths(-12), 50)

        ucFiltro1.AddFilter("Orden de visita", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(16), "OrdenVisita", "OrdenVisita", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)
        ucFiltro1.AddFilter("# Días hábiles legales de la visita", ucFiltro.AcceptedControls.TextBox, Nothing, "DiasPlazosLegalesVisita", "DiasPlazosLegalesVisita", ucFiltro.DataValueType.StringType, False, False, False, , , , 10)

        ucFiltro1.AddFilter("Folio SISAN         ", ucFiltro.AcceptedControls.TextBox, Nothing, "T_ID_FOLIO_SISAN", "Folio_SISAN", ucFiltro.DataValueType.StringType, , True)
      ucFiltro1.AddFilter("Fecha de Envío a Sanciones", ucFiltro.AcceptedControls.Calendar, dtFechaEnvioSansiones, "Fecha de Envío a Sanciones", "F_FECH_ENVIA_SANSIONES", ucFiltro.DataValueType.StringType, False, False, False, False, False, Date.Today.AddMonths(-6), 10, False)

      ucFiltro1.LoadDDL("Bandeja.aspx")
    End Sub

    Private Sub CargarCatalogo()
        Dim usuario As Entities.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario)

        If Not IsNothing(usuario) Then
            Dim consulta As String = "1=1"
            Dim lsSupOperativo As String = ""
            Dim lsInspOperativo As String = ""
            Dim lsAbogadosVisita As String = ""

            For Each filtro In ucFiltro1.getFilterSelection
                If filtro.Contains("IdInspectorResponsable=") Then
                    lsSupOperativo = filtro.Replace("IdInspectorResponsable=", "").Replace("'", "")
                ElseIf filtro.Contains("IdInspector=") Then
                    lsInspOperativo = filtro.Replace("IdInspector=", "").Replace("'", "")
                ElseIf filtro.Contains("IdAbogado=") Then
                    lsAbogadosVisita &= filtro.Replace("IdAbogado=", "").Replace("'", "") & "|"
                ElseIf filtro.Contains("F_FECH_ENVIA_SANSIONES") Then
                    consulta += " AND " + filtro.Replace(" 12:00:00 am", "").Replace(" 11:59:59 pm", "")
                Else
                    consulta += " AND " + filtro
                End If
            Next

            Dim parametro As New Entities.Parametros()
            Dim dv As DataView
            Dim dt As DataTable

            ''Elimina el ultimo pipe "|"
            If lsAbogadosVisita.Length > 0 Then
                lsAbogadosVisita = lsAbogadosVisita.Substring(0, lsAbogadosVisita.Length - 1)
            End If

         'CargarFiltrosPen()

         dt = ObtieneDtConsulta(usuario, pIdVisitaPadre, lsSupOperativo, lsInspOperativo, lsAbogadosVisita)

         If Not IsNothing(dt) Then
                If dt.Rows.Count > 0 Then
                    dv = dt.DefaultView

               'se utiliza para filtar el grid
               dv.RowFilter = consulta


               'convierte de formato la fecha de Envia a Sansion
               Dim dtaux As DataTable = dv.ToTable

               For i As Integer = 0 To dtaux.Rows.Count - 1
                  Dim dr As DataRow = dtaux.Rows(i)
                  If Not dr("F_FECH_ENVIA_SANSIONES") = "" Then
                     dr("F_FECH_ENVIA_SANSIONES") = Convert.ToDateTime(dr("F_FECH_ENVIA_SANSIONES")).ToString("dd/MM/yyyy")
                  End If
               Next

               dv = dtaux.DefaultView

               gvConsulta.DataSource = dv.ToTable

                    gvConsulta.DataBind()

                    If dv.ToTable.Rows.Count > 0 Then
                        btnExportaExcel.Visible = True
                        pnlGrid.Visible = True
                        pnlNoExiste.Visible = False

                        If pIdNumeroFilasOriginales = 0 Then ''Aqui se truquea porque a veces al inicio no muestra los scrolls
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "bandeja", gvConsulta.RetornaArmaMultiScript(), True)
                            pIdNumeroFilasOriginales = 1
                        End If
                    Else ''Si la consulta normal no encuentra coincidencias hacer una busqueda pero sin el id de la visita padre
                        If Not IsNothing(Request.QueryString("sb")) Then
                            dt = ObtieneDtConsulta(usuario, Constantes.Todos, lsSupOperativo, lsInspOperativo, lsAbogadosVisita)

                            If Not IsNothing(dt) Then
                                If dt.Rows.Count > 0 Then
                                    dv = dt.DefaultView

                                    'se utiliza para filtar el grid
                                    dv.RowFilter = consulta
                                    gvConsulta.DataSource = dv.ToTable

                                    gvConsulta.DataBind()

                                    If dv.ToTable.Rows.Count > 0 Then
                                        btnExportaExcel.Visible = True
                                        pnlGrid.Visible = True
                                        pnlNoExiste.Visible = False
                                    Else
                                        btnExportaExcel.Visible = False
                                        pnlGrid.Visible = False
                                        pnlNoExiste.Visible = True
                                    End If
                                Else
                                    btnExportaExcel.Visible = False
                                    pnlGrid.Visible = False
                                    pnlNoExiste.Visible = True
                                End If
                            Else
                                btnExportaExcel.Visible = False
                                pnlGrid.Visible = False
                                pnlNoExiste.Visible = True
                            End If
                        Else
                            btnExportaExcel.Visible = False
                            pnlGrid.Visible = False
                            pnlNoExiste.Visible = True
                        End If
                    End If
                Else
                    btnExportaExcel.Visible = False
                    pnlGrid.Visible = False
                    pnlNoExiste.Visible = True
                End If
            Else
                btnExportaExcel.Visible = False
                pnlGrid.Visible = False
                pnlNoExiste.Visible = True
            End If

        End If
    End Sub

    Private Sub CargarCatalogoPendientes()
        Dim usuario As Entities.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario)

        If Not IsNothing(usuario) Then
            Dim consultaP As String = "1=1"

            If Not ddlPasoActual.SelectedValue.Equals("-1") and Not ddlPasoActualFin.SelectedValue.Equals("-1")Then
            consultaP += " AND I_ID_PASO_ACTUAL >='" + ddlPasoActual.SelectedValue + " ' AND I_ID_PASO_ACTUAL <= '" + ddlPasoActualFin.SelectedValue + " '"
            End If

         If Not ddlFolioVisita.SelectedItem.Text.Equals("- Seleccionar -") Then
            consultaP += " AND T_ID_FOLIO='" + ddlFolioVisita.SelectedItem.Text + "'"
         End If

         Dim dvp As DataView
         Dim dp As DataTable

         'Consulta Pendientes y Alertas
         dp = AccesoBD.AlertaVisitasPorVencer(usuario.IdentificadorUsuario.ToString())

         If Not IsNothing(dp) Then
            If dp.Rows.Count > 0 Then
               dvp = dp.DefaultView

               'se utiliza para filtar el grid
               dvp.RowFilter = consultaP
               gvConsultaPendientes.DataSource = dvp.ToTable
               gvConsultaPendientes.DataBind()

               If dvp.ToTable.Rows.Count > 0 Then
                  btnExportaExcelPendientes.Visible = True
                  pnlNoExistePendientes.Visible = False

                  If pIdNumeroFilasOriginales = 0 Then ''Aqui se truquea porque a veces al inicio no muestra los scrolls
                     ScriptManager.RegisterStartupScript(Me, Me.GetType(), "bandeja", gvConsultaPendientes.RetornaArmaMultiScript(), True)
                     pIdNumeroFilasOriginales = 1
                  End If

                  imgModalPendientes.Visible = True
                  UpdatePanel1Pendientes.Visible = True
               Else
                  imgModalPendientes.Visible = False
                  UpdatePanel1Pendientes.Visible = False
               End If
            Else
               btnExportaExcelPendientes.Visible = False
               pnlNoExistePendientes.Visible = True
            End If
         Else
            btnExportaExcelPendientes.Visible = False
            pnlNoExistePendientes.Visible = True
         End If
      End If
    End Sub

    'Private Sub CargarImagenesEstatus()
    '    imgOK.ImageUrl = ObtenerImagenEstatus(True)
    '    imgERROR.ImageUrl = ObtenerImagenEstatus(False)
    'End Sub

    ''' <summary>
    ''' Oculta las columnas despues de personalizarlas
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OcultaColumnas()
        ''Validar si se requiere mostrar una subvisita
        If Not IsNothing(Request.QueryString("sb")) Then
            gvConsulta.Columns(Columnas.Subvisitas).Visible = False
            'gvConsulta.Columns(Columnas.SubEntidad).Visible = True
            gvConsulta.Columns(Columnas.Editar).Visible = True
            gvConsulta.Columns(Columnas.Borrar).Visible = True
            gvConsulta.Columns(Columnas.Folio).HeaderText = "Subfolio"
        Else
            ''Ocular columna de subvisitas cuando es VO ya que nunca va a vee las subvisitas
            If TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea = Constantes.AREA_VO Then
                gvConsulta.Columns(Columnas.Subvisitas).Visible = False
            End If
            'If TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea = Constantes.AREA_VF Then
            '    gvConsulta.Columns(Columnas.SubEntidad).Visible = True
            'End If



        End If
    End Sub
    ''' <remarks></remarks>
    Public Function ObtenerImagenEstatus(ByVal estatus As Boolean) As String

        If estatus Then
            Return "~/Imagenes/vigente.gif"
        Else
            Return "~/Imagenes/no_vigente.gif"
        End If

    End Function

    Public Function CalcularDiasTrancurridos(ByVal fechaInicioPaso As DateTime) As String

        If 1 = 1 Then
            Return "~/Imagenes/vigente.gif"
        Else
            Return "~/Imagenes/no_vigente.gif"
        End If

    End Function

    Protected Sub btnExportaExcelPendientes_Click(sender As Object, e As EventArgs) Handles btnExportaExcelPendientes.Click
        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsultaPendientes.DataSourceSession, DataTable)

        utl.ExportaGrid(dt, gvConsultaPendientes, "Tareas Pendientes y Alertas", referencias)
    End Sub

    Protected Sub btnExportaExcel_Click(sender As Object, e As EventArgs) Handles btnExportaExcel.Click
        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns("DiasAcumCSPro").ColumnName = "Superávit / Déficit"
        dt.Columns.Add("Fecha de Envío a Sanciones")
        For index As Integer = 0 To dt.Rows.Count - 1

         'dt(index)("Fecha de Envío a Sanciones") = ConexionSISAN.ObtenerFechaSancion(dt(index)("Folio_SISAN").ToString())
      Next
        utl.ExportaGrid(dt, gvConsulta, lbTituloBandeja.InnerText, referencias)
    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub

    Protected Sub btnFiltrarPendientes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFiltrarPendientes.Click
        CargarCatalogoPendientes()
    End Sub

    Protected Sub gvConsultaPendientes_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsultaPendientes.RowCreated

        If e.Row.RowType = WebControls.DataControlRowType.DataRow Then

            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsultaPendientes, e.Row.RowIndex.ToString(), False))
        End If
    End Sub

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated

        If e.Row.RowType = WebControls.DataControlRowType.DataRow Then
            Dim liSemaforo As Integer = Integer.Parse(gvConsulta.DataKeys(e.Row.DataItemIndex).Item("IdEstatusSemaforo"))
            Dim TieneSubvisita As Integer = Integer.Parse(gvConsulta.DataKeys(e.Row.DataItemIndex).Item("TieneSubvisitas"))
            Dim liPasoActual As Integer = Integer.Parse(gvConsulta.DataKeys(e.Row.DataItemIndex).Item("IdPasoActual"))
            'Dim liEstatusVisita As Integer = Integer.Parse(gvConsulta.DataKeys(e.Row.DataItemIndex).Item("EstatusVisitaInt"))

            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
            ''Habilitar el semaforo

            'If liPasoActual <> 6 Then ''CONDICIONAR SIEMPRE EL VERDE SI ESTA EN PASO 6 MCS
            If liSemaforo <> Constantes.Semaforo.Verde Then
                Dim imgSemaforo As Image = CType(e.Row.FindControl("imgSemaforo"), Image)
                If Not IsNothing(imgSemaforo) Then
                    If liSemaforo = Constantes.Semaforo.Amarillo Then
                        imgSemaforo.ImageUrl = "~/Imagenes/semaforo2.png"
                    ElseIf liSemaforo = Constantes.Semaforo.Rojo Then
                        imgSemaforo.ImageUrl = "~/Imagenes/semaforo3.png"
                    Else
                        imgSemaforo.ImageUrl = "~/Imagenes/semaforo4.png"
                    End If
                End If
            End If
            'End If

            Dim lblDS As Label = CType(e.Row.FindControl("lblDeficitSuperavit"), Label)
            lblDS.Text = Integer.Parse(gvConsulta.DataKeys(e.Row.DataItemIndex).Item("DiasAcumCSPro"))

            If Integer.Parse(lblDS.Text) = 9999999 Then
                lblDS.Text = "N/A"
                lblDS.Attributes.CssStyle.Add("color", "black")
            Else
                If Integer.Parse(lblDS.Text) < 0 Then
                    lblDS.Attributes.CssStyle.Add("color", "red")
                    lblDS.Attributes.CssStyle.Add("font-weight", "bold")
                Else
                    lblDS.Attributes.CssStyle.Add("color", "green")
                    lblDS.Attributes.CssStyle.Add("font-weight", "bold")
                End If
            End If

            ''Habilitar el boton subvisitas

            If TieneSubvisita = Constantes.Verdadero Then
                Dim lmgSubvisitas As ImageButton = CType(e.Row.FindControl("imgVerSubvisita"), ImageButton)
                If Not IsNothing(lmgSubvisitas) Then
                    lmgSubvisitas.Visible = True
                End If
            End If

            ''Motrar la imagen del estatus de la visita
            'If liEstatusVisita <> Constantes.EstatusVisita.Vigente Then
            '    Dim imgEstatusV As Image = CType(e.Row.FindControl("imgEstatus"), Image)
            '    If Not IsNothing(imgEstatusV) Then
            '        If liSemaforo = Constantes.EstatusVisita.Prorroga Then
            '            imgEstatusV.ImageUrl = "~/Imagenes/prorroga.png"
            '        Else
            '            imgEstatusV.ImageUrl = "~/Imagenes/vencido.png"
            '        End If
            '    End If
            'End If

            ''Ocultar el boton editar cuando la visita este en un paso mayor al 1, solo se pueden editar las subvisitas
            'por eso se condiciona el id de visita padre
            If pIdVisitaPadre <> 0 Then
                If liPasoActual > PasoProcesoVisita.Pasos.Uno Then
                    Dim btnEditar As ImageButton = CType(e.Row.FindControl("btnEditar"), ImageButton)
                    If Not IsNothing(btnEditar) Then
                        btnEditar.Visible = False
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        'Validar si es del proceso anterior o del nuevo
        Dim idVisitaGenerado As Integer = CInt(gvConsulta.DataKeys(index)("IdVisitaGenerado").ToString())
        Session("ID_VISITA") = idVisitaGenerado

        Dim fechaRegVisita As Date = CDate(gvConsulta.DataKeys(index)("FechaRegistroFormateada").ToString())
        Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

        If (Convert.ToDateTime(fechaRegVisita).ToString("yyyy/MM/dd") < Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
            Response.Redirect("../Procesos/DetalleVisita.aspx#tab1")
        Else
            Response.Redirect("../Procesos/DetalleVisita_V17.aspx#tab1")
        End If

    End Sub

    Protected Sub btnConsultaPendientes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsultaPendientes.Click

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        'Validar si es del proceso anterior o del nuevo
        Dim idVisitaGenerado As Integer = CInt(gvConsultaPendientes.DataKeys(index)("I_ID_VISITA").ToString())
        Session("ID_VISITA") = idVisitaGenerado

        Dim fechaRegVisita As Date = AccesoBD.getFechaRegistroVisita(gvConsultaPendientes.DataKeys(index)("T_ID_FOLIO").ToString())
        Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

        If (Convert.ToDateTime(fechaRegVisita).ToString("yyyy/MM/dd") < Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
            Response.Redirect("../Procesos/DetalleVisita.aspx#tab1")
        Else
            Response.Redirect("../Procesos/DetalleVisita_V17.aspx#tab1")
        End If

    End Sub

    Protected Sub btnPersonalizaColumnas_Click(sender As Object, e As EventArgs) Handles btnPersonalizaColumnas.Click
        PersonalizaColumnas.IdentificadorGridView = 2
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.Mostrar()
    End Sub

    Protected Sub PersonalizaColumnas_event(ByVal sender As Object, ByVal e As EventArgs) Handles PersonalizaColumnas.FinPersonalizacion
        PersonalizaColumnas.IdentificadorGridView = 2
        PersonalizaColumnas.GridViewPersonalizar = gvConsulta
        PersonalizaColumnas.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        PersonalizaColumnas.GuardarPersonalizacion()
        gvConsulta.DataSource = gvConsulta.DataSourceSession

        ''Para que no muestra ni oculte columans de mas
        OcultaColumnas()

        gvConsulta.DataBind()
    End Sub

    Protected Sub gvConsultaPendientes_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsultaPendientes.Sorting
        gvConsultaPendientes.Ordenar(e)
    End Sub

    Protected Sub gvConsulta_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting
        gvConsulta.Ordenar(e)
    End Sub

    Protected Sub gvConsulta_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvConsulta.PageIndexChanging
        gvConsulta.PageIndex = e.NewPageIndex
        gvConsulta.DataSource = gvConsulta.DataSourceSession
        gvConsulta.DataBind()

        'Se agrega segmento para controlar el checkbox Selecctionar Todo
        'Al cambiar de pagina se pierde la seleccion
        chkSeleccionaTodos.Checked = False
    End Sub

    Protected Sub gvConsultaPendientes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvConsultaPendientes.PageIndexChanging
        gvConsultaPendientes.PageIndex = e.NewPageIndex
        gvConsultaPendientes.DataSource = gvConsultaPendientes.DataSourceSession
        gvConsultaPendientes.DataBind()

        'Se agrega segmento para controlar el checkbox Selecctionar Todo
        'Al cambiar de pagina se pierde la seleccion
        chkSeleccionaTodos.Checked = False
    End Sub

    Protected Sub chkSeleccionaTodos_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkSeleccionaTodos.CheckedChanged
        Dim chkselects As CheckBox = TryCast(sender, CheckBox)
        For Each gvrow As GridViewRow In gvConsulta.Rows
            Dim chkSelected As CheckBox = TryCast(gvrow.FindControl("chkElemento"), CheckBox)
            chkSelected.Checked = chkselects.Checked
        Next
        gvConsulta.CargaSeleccion()
    End Sub

    Protected Sub cbSelecteds2_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim allSelect As Boolean = False
        For Each gvrow As GridViewRow In gvConsulta.Rows
            Dim chkSelected2 As CheckBox = TryCast(gvrow.FindControl("chkElemento"), CheckBox)
            allSelect = chkSelected2.Checked
            If allSelect = False Then
                Exit For
            End If
        Next
        chkSeleccionaTodos.Checked = allSelect
        gvConsulta.CargaSeleccion()
    End Sub

    Private Function RemoveDuplicateRows(ByVal dTable As DataSet, ByVal colName As String) As DataSet
        Try
            Dim hTable As Hashtable = New Hashtable()
            Dim duplicateList As ArrayList = New ArrayList
            For Each drow As DataRow In dTable.Tables(0).Rows
                If (hTable.Contains(drow(colName))) Then
                    If Not drow(colName).ToString = "" Then
                        duplicateList.Add(drow)
                    End If
                Else
                    hTable.Add(drow(colName), String.Empty)
                End If
            Next drow
            For Each drow As DataRow In duplicateList
                dTable.Tables(0).Rows.Remove(drow)
            Next drow
            Return dTable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Function HayRegistroSeleccionado() As Boolean

        Dim haySeleccion As Boolean = False

        For Each row As GridViewRow In gvConsulta.Rows

            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)

            If elemento.Checked Then

                haySeleccion = True
                Exit For

            End If

        Next

        If Not haySeleccion Then
            Dim errores As New Entities.EtiquetaError(97)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

        Return haySeleccion

    End Function

    Protected Sub btnVerReporte_Click(sender As Object, e As ImageClickEventArgs) Handles btnVerReporte.Click

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then
                Dim idVisitaGenerado As Integer = CInt(gvConsulta.DataKeys(row.RowIndex)("IdVisitaGenerado").ToString())
                Session("ID_VISITA_SEGUIMIENTO") = idVisitaGenerado
                Response.Redirect("../Procesos/SeguimientoReporte.aspx")
            End If
        Next

    End Sub
    ''' <summary>
    ''' Muestra las subvisitas de una visita
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnkVerSubvisita_Click(sender As Object, e As EventArgs)
        Dim lnkVerSubvisita As LinkButton = CType(sender, LinkButton)
        If Not IsNothing(lnkVerSubvisita) Then
            Response.Redirect("Bandeja.aspx?sb=" & lnkVerSubvisita.CommandArgument)
        End If
    End Sub

    Protected Sub btnRegresar_Click(sender As Object, e As EventArgs)
        Response.Redirect("Bandeja.aspx")
    End Sub

    Protected Sub btnEditar_Click(sender As Object, e As ImageClickEventArgs)
        Dim imgEditar As ImageButton = CType(sender, ImageButton)

        If Not IsNothing(imgEditar) Then
            Session.Add("idVisitaEditar", imgEditar.CommandArgument)
            Response.Redirect("../Visita/Registro.aspx?up=1&sb=" & pIdVisitaPadre.ToString())
        End If
    End Sub

    ''' <summary>
    ''' Muestra la bandeja de subvisitas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub imgVerSubvisita_Click(sender As Object, e As ImageClickEventArgs)
        Dim imgVerSubvisita As ImageButton = CType(sender, ImageButton)
        If Not IsNothing(imgVerSubvisita) Then
            Response.Redirect("Bandeja.aspx?sb=" & imgVerSubvisita.CommandArgument)
        End If
    End Sub

    ''' <summary>
    ''' Valida si una fecha es una fecha habil
    ''' </summary>
    ''' <param name="pdFecha"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EsDiaHabil(pdFecha As Date) As Boolean
        If pdFecha.DayOfWeek = DayOfWeek.Saturday Or pdFecha.DayOfWeek = DayOfWeek.Sunday Then
            Return False
        Else
            Dim lstDiasFeriados As New List(Of DateTime)
            lstDiasFeriados = AccesoBD.getDiasFeriados(pdFecha)

            If lstDiasFeriados.Count > 0 Then
                Return False
            Else
                Return True
            End If
        End If
    End Function

    ''' <summary>
    ''' Guarda la fecha de cancelacion de la subvisita
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCancelarVisita_Click(sender As Object, e As EventArgs)
        ''Validar que este llena y sea correcta la fecha de campo
        ''Validar observaciones
        Dim lsAux As String = txtMotivoCancelacion.Text.Trim()
        Dim lsAux2 As String = ""

        For i As Integer = 0 To lsAux.Length - 1
            If Not lsAux.Chars(i) = "," Then
                lsAux2 = lsAux.Substring(i)
                Exit For
            End If
        Next

        If lsAux2.Length < 1 Then
            Dim errores As New Entities.EtiquetaError(2129)
            lblFechaGeneral.Visible = True
            lblFechaGeneral.Text = errores.Descripcion

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "CancelarVisita();", True)
            Exit Sub
        Else
            lblFechaGeneral.Visible = False
        End If

        Dim btnCancelar As Button = CType(sender, Button)

        If Not IsNothing(btnCancelar) Then
            Dim Usuario As New Entities.Usuario()
            Usuario = Session(Entities.Usuario.SessionID)

            Dim visita As New Visita()

            If Not IsNothing(Usuario) Then
                visita = AccesoBD.getDetalleVisita(btnCancelar.CommandArgument, Usuario.IdArea)

                If Not IsNothing(visita) And visita.FolioVisita <> String.Empty And visita.IdVisitaGenerado > 0 Then
                    ''Obtienelos inspectores y sus correos
                    visita.LstInspectoresAsignados = AccesoBD.getInspectoresAsignados(visita.IdVisitaGenerado)
                End If
            End If

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 And Not IsNothing(Usuario) Then
                Dim objNegVisita As New NegocioVisita(visita, Usuario, Server, txtMotivoCancelacion.Text.Trim())
                objNegVisita.ppObservaciones = lsAux2

                ''Detener la visita InSitu
                objNegVisita.CancelarVisita()

                'Mostra mensaje si desa registrar una nueva visita
                ' SI: redirect registro visita
                ' NO: redirect bandeja visitas
                Dim errores As New Entities.EtiquetaError(2132)
                Mensaje = errores.Descripcion
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MostrarMensajeRegistro();", True)
            End If
        End If
    End Sub

    Protected Sub btnSiRegistra_Click(sender As Object, e As EventArgs)
        Response.Redirect("../Visita/Registro.aspx")
    End Sub

    Protected Sub btnNoRegistra_Click(sender As Object, e As EventArgs)
        Response.Redirect("Bandeja.aspx?sb=" & pIdVisitaPadre)
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As ImageClickEventArgs)
        btnCancelarVisita.CommandArgument = CType(sender, ImageButton).CommandArgument
        txtMotivoCancelacion.Text = ""
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "CancelarVisita();", True)
    End Sub

    ''' <summary>
    ''' Hace la consulta para la bandeja principal
    ''' </summary>
    ''' <param name="objUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtieneDtConsulta(objUsuario As Entities.Usuario,
                                       pIdVisitaPadre As Integer,
                                       psSupOperativo As String,
                                       psInspOperativo As String,
                                       psAbogadosVisita As String) As DataTable
        Dim dt As New DataTable

        ''Validar que si es alguna area operativa la que consulte, considere el area
        If objUsuario.IdArea = Constantes.AREA_PR Or
            (objUsuario.IdArea = Constantes.AREA_VJ And objUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM) Then
            ''Si es el abogado supervisor o presidencia trae todo sin considerar el area
            dt = AccesoBD.consultarVisitas(, , , , pIdVisitaPadre, psSupOperativo, psInspOperativo, psAbogadosVisita)
        Else ''trae la informacion restringida por usuario logueado
            dt = AccesoBD.consultarVisitas(objUsuario.IdArea, Constantes.CopFoliosTConsul.TodosLosPasos, objUsuario.IdentificadorUsuario, objUsuario.IdentificadorPerfilActual, pIdVisitaPadre, psSupOperativo, psInspOperativo, psAbogadosVisita)
        End If

        Return dt
    End Function

    Private Sub imgProcesoVisitaAmbos_Click(sender As Object, e As ImageClickEventArgs) Handles imgProcesoVisitaAmbos.Click
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroOpcionesImgProcesos();", True)
    End Sub

   Protected Sub gvConsulta_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvConsulta.RowDataBound


      'If e.Row.RowType = DataControlRowType.DataRow Then
      '    'Recorrer La columna FOLIO SISAN'

      '    Dim FechaSancion As Label = Nothing

      ' FechaSancion = TryCast(e.Row.Cells(19).FindControl("F_FECH_ENVIA_SANSIONES"), Label)

      ' Dim resultado As String
      '    resultado = ConexionSISAN.ObtenerFechaSancion(e.Row.DataItem("Folio_SISAN"))
      '    FechaSancion.Text = resultado
      'End If
   End Sub
End Class