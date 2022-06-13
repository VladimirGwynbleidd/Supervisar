Imports System.Web.Configuration
Imports System.Web.Services
Imports System.IO

Imports Utilerias

Public Class ucDoctosProrroga

   Inherits System.Web.UI.UserControl
   Public Property MensajeDocs As String
   Private Property piColumnaDocs As Integer = 3

#Region "Props"

   Public Property pObjPropiedadesProrroga As PropiedadesDocProrroga
      Get
         If IsNothing(Session("pObjPropiedadesProrroga_" & Me.ID)) Then
            Return Nothing
         Else
            Return CType(Session("pObjPropiedadesProrroga_" & Me.ID), PropiedadesDocProrroga)
         End If
      End Get
      Set(value As PropiedadesDocProrroga)
         Session("pObjPropiedadesProrroga_" & Me.ID) = value
      End Set
   End Property

   'Public WriteOnly Property VisiblePanelEncabezado As Boolean
   '   Set(value As Boolean)
   '      pnlEncabezado.Visible = value
   '   End Set
   'End Property

   Public ReadOnly Property puObjUsuario As Entities.Usuario
      Get
         If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
            Return CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
         Else
            Return Nothing
         End If
      End Get
   End Property

   Private ReadOnly Property pgIdentificadorUsuario As String
      Get
         If IsNothing(Session(Entities.Usuario.SessionID)) Then
            Return ""
         Else
            Dim usuario As New Entities.Usuario()
            usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

            Return usuario.IdentificadorUsuario
         End If
      End Get
   End Property

   Public Property idGrid As String
      Get
         Return gvConsultaDocs.ID
      End Get
      Set(value As String)
         gvConsultaDocs.ID = value
      End Set
   End Property


   Public Sub ModificaIds(piIdVisita As Integer)
      If Not gvConsultaDocs.ID.Contains("_" & piIdVisita.ToString()) Then gvConsultaDocs.ID = gvConsultaDocs.ID & "_" & piIdVisita.ToString()

      If IsNothing(Me.ID) Then
         Me.ID = "ucDocumentos_" & piIdVisita.ToString()
      Else
         If Not Me.ID.Contains("_" & piIdVisita.ToString()) Then Me.ID = "ucDocumentos_" & piIdVisita.ToString()
      End If

      If Not pnlGrid.ID.Contains("_" & piIdVisita.ToString()) Then pnlGrid.ID = pnlGrid.ID & "_" & piIdVisita.ToString()
      If Not pnlGridInt.ID.Contains("_" & piIdVisita.ToString()) Then pnlGridInt.ID = pnlGridInt.ID & "_" & piIdVisita.ToString()
      If Not hfValorScrollDocs.ID.Contains("_" & piIdVisita.ToString()) Then hfValorScrollDocs.ID = hfValorScrollDocs.ID & "_" & piIdVisita.ToString()
      If Not hfSelectedValue.ID.Contains("_" & piIdVisita.ToString()) Then hfSelectedValue.ID = hfSelectedValue.ID & "_" & gvConsultaDocs.ID
      If Not hfGridView1SV.ID.Contains("_" & piIdVisita.ToString()) Then hfGridView1SV.ID = hfGridView1SV.ID & "_" & gvConsultaDocs.ID
      If Not hfGridView1SH.ID.Contains("_" & piIdVisita.ToString()) Then hfGridView1SH.ID = hfGridView1SH.ID & "_" & gvConsultaDocs.ID

      If Not hfIdRenglon.ID.Contains("_" & piIdVisita.ToString()) Then hfIdRenglon.ID = hfIdRenglon.ID & "_" & piIdVisita.ToString()
      If Not hfIdDocSicod.ID.Contains("_" & piIdVisita.ToString()) Then hfIdDocSicod.ID = hfIdDocSicod.ID & "_" & piIdVisita.ToString()
      If Not hfIdVerDocSicod.ID.Contains("_" & piIdVisita.ToString()) Then hfIdVerDocSicod.ID = hfIdVerDocSicod.ID & "_" & piIdVisita.ToString()
      If Not hfNumVerDocSicod.ID.Contains("_" & piIdVisita.ToString()) Then hfNumVerDocSicod.ID = hfNumVerDocSicod.ID & "_" & piIdVisita.ToString()
      If Not hfVisita.ID.Contains("_" & piIdVisita.ToString()) Then hfVisita.ID = hfVisita.ID & "_" & piIdVisita.ToString()
   End Sub

   Public Class PropiedadesDocProrroga
      Public Property ColumnasCongeladas As Integer

      Public Property ppObjVisita As Visita

      Public Property pgIdTxtComentarios As String

      ''' <summary>
      ''' Gaurda si el usuario logueado tiene permisos para editar
      ''' </summary>
      ''' <value></value>
      ''' <returns></returns>
      ''' <remarks></remarks>
      Public Property PermisoEditarDocs As Boolean


      Public Property PermisoEditarPDF As Boolean


      ''' <summary>
      ''' Guarda un valor cuando se estan cargando documentos sin un id de visita
      ''' </summary>
      ''' <value></value>
      ''' <returns></returns>
      ''' <remarks></remarks>
      Public Property pbEsSubFolioSubVisita As Boolean

      Public Property pbEstaEditandoVisita As Boolean

      Public Property pbEstaInsertandoVisita As Boolean
      ''' <summary>
      ''' Guarda el id de la visita que se acaba de generar 
      ''' </summary>
      ''' <value></value>
      ''' <returns></returns>
      ''' <remarks></remarks>
      Public Property piIdVisitaNueva As Integer

      Public Property piIdVisitaActualDoc As Integer

      Public ReadOnly Property psIdVisita As String
         Get
            Return IIf(piIdVisitaActualDoc = Constantes.Todos, "1", piIdVisitaActualDoc.ToString())
         End Get
      End Property

      ''' <summary>
      ''' Para cuando se requiera mostrar la funcionalidad para un solo paso
      ''' </summary>
      ''' <value></value>
      ''' <returns></returns>
      ''' <remarks></remarks>
      Public Property piIdPasoDocumentos As Integer
      Public Property peObjExp As Expediente
      Public Property piIdWidthPanel As Integer
      Public Property piIdHeightPanel As Integer

      Public Property DataSourceSession As Object
   End Class
#End Region

#Region "MetodosAux"
   Private Sub ActualizaAnchoDiv()
      Dim liNumeroDoctos As Integer = CType(pObjPropiedadesProrroga.DataSourceSession, DataTable).Rows.Count
      Dim liNumeroVersiones As Integer = pObjPropiedadesProrroga.peObjExp.lstDocumentos.Count
      Dim liAnchoGrid As Integer = 0
      '25 de lo ancho de cada renglon normal
      'reenglon extra es por reenglon +-
      Dim liTamRenExtra As Integer = 20
      ''Mas 1 del encabezado y 1 comodin
      liAnchoGrid = (((liNumeroDoctos + 2) * 25) + ((liNumeroVersiones - liNumeroDoctos) * liTamRenExtra))

      ''400 de tamanio normal
      If liAnchoGrid < 500 Then
         pnlGridInt.Attributes.Remove("style")
         pnlGridInt.Attributes.Add("style", "overflow-x:hidden; OVERFLOW:auto; BACKGROUND: #ffffff; Z-INDEX: 0; width:95%; height:" & liAnchoGrid.ToString() & "px; margin-left:2%;  text-align:left;")
         gvEncabecados.CssClass = "gridDocsCompleta"
         gvConsultaDocs.CssClass = "gridDocsCompleta"
         'Else
         '    pnlGridInt.Attributes.Remove("style")
         '    pnlGridInt.Attributes.Add("style", "overflow-x:hidden; OVERFLOW:auto; BACKGROUND: #ffffff; Z-INDEX: 0; width:95%; height:400px;")
         '    gvEncabecados.CssClass = "anchoGriDocsEncabezado"
         '    gvConsultaDocs.CssClass = "anchoGriDocs"
      End If
   End Sub

   Private Sub ActualizaAnchoDivFiltrar()
      Dim liNumeroDoctos As Integer = gvConsultaDocs.Rows.Count
      Dim lstLinkBtn As List(Of LinkButton) = gvConsultaDocs.GetAllControlsOfType(Of LinkButton)()
      Dim liNumeroVersiones As Integer = lstLinkBtn.Count
      Dim liAnchoGrid As Integer = 0
      '25 de lo ancho de cada renglon normal
      'reenglon extra es por reenglon +-
      Dim liTamRenExtra As Integer = 20
      ''Mas 1 del encabezado y 1 comodin
      liAnchoGrid = (((liNumeroDoctos + 2) * 25) + ((liNumeroVersiones - liNumeroDoctos) * liTamRenExtra))

      ''400 de tamanio normal
      If liAnchoGrid < 500 Then
         pnlGridInt.Attributes.Remove("style")
         pnlGridInt.Attributes.Add("style", "overflow-x:hidden; OVERFLOW:auto; BACKGROUND: #ffffff; Z-INDEX: 0; width:95%; height:" & liAnchoGrid.ToString() & "px; margin-left:2%;  text-align:left;")
         gvEncabecados.CssClass = "gridDocsCompleta"
         gvConsultaDocs.CssClass = "gridDocsCompleta"
      Else
         pnlGridInt.Attributes.Remove("style")
         pnlGridInt.Attributes.Add("style", "overflow-x:hidden; OVERFLOW:auto; BACKGROUND: #ffffff; Z-INDEX: 0; width:95%; height:400px; margin-left:2%;  text-align:left;")
         gvEncabecados.CssClass = "anchoGriDocsEncabezado"
         gvConsultaDocs.CssClass = "anchoGriDocs"
      End If
   End Sub

   ''' <summary>
   ''' Creaun nuevo nombre para los documentos cargados
   ''' </summary>
   ''' <param name="piIdDoc"></param>
   ''' <param name="piVer"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Private Function ObtenerNombreDocumentoArchivoSepris(piIdDoc As Integer, piVer As Integer, psExtencion As String,
                                                        piNumVersiones As Integer, objDocumento As Documento, Optional idVisita As Integer = 0) As String
      Dim lsNombreNew As String = ""
      Dim lsNombreDoc As String = ""
      Dim visita As New Visita()

      If IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
         visita = AccesoBD.getDetalleVisita(Convert.ToInt32(Session("ID_VISITA")), puObjUsuario.IdArea)
      Else
         visita = pObjPropiedadesProrroga.ppObjVisita
      End If

      If Not IsNothing(objDocumento) Then
         lsNombreDoc = IIf(objDocumento.T_NOM_CORTO.Length < 1, objDocumento.T_NOM_DOCUMENTO_CAT, objDocumento.T_NOM_CORTO)

         If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
            lsNombreNew = lsNombreDoc & "_" & objDocumento.N_ID_VERSION.ToString() & "_" & pObjPropiedadesProrroga.ppObjVisita.FolioVisita.Replace("/", "") & "_V" & piNumVersiones.ToString()
         Else
            lsNombreNew = lsNombreDoc & "_" & objDocumento.N_ID_VERSION.ToString() & "_SNF_" & DateTime.Now.ToString("yyMMddhhmmss") & "_V1"
         End If
      End If

      Return lsNombreNew & psExtencion
   End Function

   'Protected Sub btnExportaExcel_Click(sender As Object, e As EventArgs) Handles btnExportaExcelDocs.Click
   '   'Dim utl As New Utilerias.ExportarExcel
   '   'Dim referencias As New List(Of String)
   '   'referencias.Add(puObjUsuario.IdentificadorUsuario.ToString)
   '   'referencias.Add(Now.ToString)

   '   'If Not IsNothing(gvConsultaDocs.DataSource) Then
   '   '    utl.ExportaGrid(gvConsultaDocs.DataSource(), gvConsultaDocs, "Bandeja de expediente", referencias)
   '   'Else
   '   '    utl.ExportaGrid(pObjPropiedadesProrroga.DataSourceSession, gvConsultaDocs, "Bandeja de expediente", referencias)
   '   'End If

   '   Dim utl As New Utilerias.ExportarExcel
   '   Dim referencias As New List(Of String)
   '   referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
   '   referencias.Add(Now.ToString)

   '   Dim dt As DataTable

   '   dt = CargaDocumentosExcel()

   '   If Not IsNothing(dt) Then
   '      utl.ExportaGrid(dt, gvEncabecados, "Bandeja de expediente", referencias)
   '   End If

   'End Sub

   Public Function CargaDocumentosExcel() As DataTable

      Dim dt As New DataTable
      Dim objDocumentos As New Documento

      If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
         If pObjPropiedadesProrroga.pbEstaEditandoVisita And pObjPropiedadesProrroga.ppObjVisita.EsSubVisita Then
            dt = objDocumentos.getDocumentosAgrupados(Constantes.Vigencia.Vigente, pObjPropiedadesProrroga.ppObjVisita.IdPasoActual, pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado)
         Else
            dt = objDocumentos.getDocumentosAgrupados(Constantes.Vigencia.Vigente, Constantes.Todos, pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado)
         End If
      Else
         dt = objDocumentos.getDocumentosAgrupados(Constantes.Vigencia.Vigente, pObjPropiedadesProrroga.piIdPasoDocumentos, Constantes.Todos, pgIdentificadorUsuario)
      End If

      Return dt
   End Function

   'Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltroDocs.Filtrar
   '   CargarCatalogo()
   'End Sub

   'Private Sub CargarFiltros()
   '   ucFiltroDocs.resetSession()

   '   Dim dtDatosFiltro As DataSet = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.DocumentosControl, puObjUsuario.IdentificadorUsuario, puObjUsuario.IdArea)

   '   'ucFiltro1.AddFilter("Vigencia  ", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSource, "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, False, -1)
   '   'ucFiltroDocs.AddFilter("Paso", ucFiltro.AcceptedControls.TextBox, Nothing, "", "I_ID_PASO_INI", ucFiltro.DataValueType.IntegerType, False, False, False, , , , 2)
   '   ucFiltroDocs.AddFilter("Rango de pasos", ucFiltro.AcceptedControls.DropDownListR, dtDatosFiltro.Tables(0), "DSC_PASO", "I_ID_PASO_INI", ucFiltro.DataValueType.RangeType, False, False, False, , False, "", 50)

   '   'ucFiltroDocs.AddFilter("Documento", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_NOM_DOCUMENTO_CAT", ucFiltro.DataValueType.StringType, False, True, False, , , , 255)
   '   ucFiltroDocs.AddFilter("Documento", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(1), "T_NOM_DOCUMENTO_CAT", "N_ID_DOCUMENTO", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

   '   ucFiltroDocs.LoadDDL("Documentos.ascx")

   'End Sub

   Private Sub CargarCatalogo()

      If Not IsNothing(puObjUsuario) Then
         Dim consulta As String = "1=1"

         'For Each filtro In ucFiltroDocs.getFilterSelection
         '   consulta += " AND " + filtro
         'Next

         Dim dv As DataView
         Dim dt As DataTable

         dt = pObjPropiedadesProrroga.DataSourceSession

         If Not IsNothing(dt) Then
            If dt.Rows.Count > 0 Then
               dv = dt.DefaultView

               'se utiliza para filtar el grid
               dv.RowFilter = consulta
               gvConsultaDocs.DataSource = dv.ToTable
               gvConsultaDocs.DataBind()

               ActualizaAnchoDivFiltrar()

               If dv.ToTable.Rows.Count > 0 Then
                  'btnExportaExcelDocs.Visible = True
                  pnlGrid.Visible = True
                  pnlNoExiste.Visible = False
               Else
                  'btnExportaExcelDocs.Visible = False
                  pnlGrid.Visible = False
                  pnlNoExiste.Visible = True
               End If
            Else
               'btnExportaExcelDocs.Visible = False
               pnlGrid.Visible = False
               pnlNoExiste.Visible = True
            End If
         Else
            'btnExportaExcelDocs.Visible = False
            pnlGrid.Visible = False
            pnlNoExiste.Visible = True
         End If
      End If
   End Sub

   ''' <summary>
   ''' Muestra msg si hay documentos obligat
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Function HayDoctosObligatoriosSinCargarPorVisita() As Boolean
      ''Se esta insertando
      If pObjPropiedadesProrroga.PermisoEditarDocs Then
         If ConsultaDocumentosObligatoriosSinCargarVisita(pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado) Then
            Dim errores As New Entities.EtiquetaError(2140)
            MensajeDocs = errores.Descripcion & "<br /><br /> Visita: " & Me.pObjPropiedadesProrroga.ppObjVisita.FolioVisita
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
            Return True
         End If
      End If

      Return False
   End Function

   Public Function HayDoctosObligatoriosSinCargarPorVisitaSinMensage() As Boolean
      ''Se esta insertando
      If pObjPropiedadesProrroga.PermisoEditarDocs Then

         'VALIDAR QUE EN EL PASO 5 PARA EL NUEVO PROCESO LOS DOCUMENTOS SEAN OBLIGATORIOS AUNQUE EN EL PROCESO ANTERIOR SEAN OBLIGATORIOS HASTA EL PASO 18

         If ConsultaDocumentosObligatoriosSinCargarVisita(pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado) Then
            Return True
         End If
      End If

      Return False
   End Function

   Public Function HayDoctosObligatoriosSinCargarSinVista() As Boolean
      ''Se esta insertando
      If pObjPropiedadesProrroga.PermisoEditarDocs Then
         If ConsultaDocumentosObligatoriosSinCargarPorPasoUsuario(pObjPropiedadesProrroga.ppObjVisita.IdPasoActual, Constantes.Obligatorio.Obligatorios,
                                                                  Constantes.TipoArchivo.TODOS) Then
            Dim errores As New Entities.EtiquetaError(2140)
            MensajeDocs = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
            Return True
         End If
      End If

      Return False
   End Function

   Public Sub ConfigurarSharePointSepris(ByRef Shp As Utilerias.SharePointManager)
      Shp.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSEPRIS").ToString()
      Shp.Usuario = WebConfigurationManager.AppSettings("SharePointUserSEPRIS").ToString()
      Shp.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SharePointPasswordSEPRIS").ToString())
      Shp.Dominio = WebConfigurationManager.AppSettings("SharePointDomainSEPRIS").ToString()
      Shp.Biblioteca = WebConfigurationManager.AppSettings("SharePointBibliotecaSEPRIS").ToString()
   End Sub

   Public Sub ConfigurarSharePointSeprisSicod(ByRef Shp As Utilerias.SharePointManager)
      Shp.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSICOD").ToString()
      Shp.Usuario = WebConfigurationManager.AppSettings("UsuarioSpSICOD").ToString()
      Shp.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("PassEncSpSICOD").ToString())
      Shp.Dominio = WebConfigurationManager.AppSettings("DomainSICOD").ToString()
      Shp.Biblioteca = WebConfigurationManager.AppSettings("DocLibrarySICOD").ToString()
   End Sub

   Public Sub ConfigurarSharePointSisvigSicod(ByRef Shp As Utilerias.SharePointManager)
      Shp.ServidorSharePoint = WebConfigurationManager.AppSettings("SPSISVIGSICODSrv").ToString()
      Shp.Usuario = WebConfigurationManager.AppSettings("SPSISVIGSICODUsr").ToString()
      Shp.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SPSISVIGSICODPas").ToString())
      Shp.Dominio = WebConfigurationManager.AppSettings("SPSISVIGSICODDom").ToString()
      Shp.Biblioteca = WebConfigurationManager.AppSettings("SPSISVIGSICODLib").ToString()
   End Sub

   Public Sub ConfigurarSharePointSeprisSisvig(ByRef Shp As Utilerias.SharePointManager)
      Shp.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSISVIG").ToString()
      Shp.Usuario = WebConfigurationManager.AppSettings("SISVIGUSUARIOSp").ToString()
      Shp.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SISVIGPassEncSp").ToString())
      Shp.Dominio = WebConfigurationManager.AppSettings("SISVIGDomainSp").ToString()
      Shp.Biblioteca = WebConfigurationManager.AppSettings("DocLibrarySISVIG").ToString()
   End Sub

   Public Function ObtenSM() As ScriptManager
      Return CType(Page.Master.FindControl("SM"), ScriptManager)
   End Function

   Private Function ObtenerTamMaximoArch() As Integer
      'Obtener el maximo permitido
      Dim liLimiteArchivoCarga As Integer
      Try
         liLimiteArchivoCarga = CInt(WebConfigurationManager.AppSettings("LimiteTamArchivo").ToString())
      Catch
         liLimiteArchivoCarga = 49152000
      End Try
      Return liLimiteArchivoCarga
   End Function

   ''' <summary>
   ''' Busca si hay documentos obligatorios sin cargar en el paso actual de la visita
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Function ConsultaDocumentosObligatoriosSinCargarVisita(idVisita As Integer) As Boolean
      Dim lstDocMin As Integer = 0

      lstDocMin = AccesoBD.ObtenerDocumentosObligatoriosProrroga(idVisita)

      If lstDocMin = 3 Then
         Return True
      Else
         Return False
      End If
   End Function

   ''' <summary>
   ''' Obtiene los docs obligatorios por paso y usuario
   ''' </summary>
   ''' <param name="idPaso"></param>
   ''' <param name="iBanderaObligatorio"></param>
   ''' <param name="iTipoDocumento"></param>
   ''' <param name="idDocumento"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Function ConsultaDocumentosObligatoriosSinCargarPorPasoUsuario(idPaso As Integer,
                                                      Optional iBanderaObligatorio As Integer = Constantes.Todos,
                                                      Optional iTipoDocumento As Integer = Constantes.TipoArchivo.WORD,
                                                      Optional idDocumento As Integer = Constantes.Todos,
                                                      Optional idVisita As Integer = Constantes.Todos) As Boolean
      Dim lstDocMin As List(Of Documento.DocumentoMini)

      'lstDocMin = AccesoBD.ObtenerDocumentosObligatoriosPorPasoUsuario(pgIdentificadorUsuario, idPaso, idDocumento, iBanderaObligatorio, iTipoDocumento)
      lstDocMin = AccesoBD.ObtenerDocumentosObligatoriosPorPasoUsuario("", idPaso, idDocumento, iBanderaObligatorio, iTipoDocumento, idVisita)

      If lstDocMin.Count > 0 Then
         Return True
      Else
         Return False
      End If
   End Function

   ''' <summary>
   ''' Busca los comentarios de la pagina principal
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Private Function BuscaComentarios() As String
      Dim lsComentarios As String = ""

      ''Busca los comentarios
      If pObjPropiedadesProrroga.pgIdTxtComentarios <> "" Then
         Dim lstTxt As List(Of TextBox) = Me.Page.GetAllControlsOfType(Of TextBox)()

         If lstTxt.Count > 0 Then
            Dim txtComentarios As TextBox = (From lTxt In lstTxt Where lTxt.ID = pObjPropiedadesProrroga.pgIdTxtComentarios).FirstOrDefault()

            If Not IsNothing(txtComentarios) Then
               lsComentarios = txtComentarios.Text
            End If
         End If
      End If

      Return lsComentarios
   End Function

   Private Sub PonerClaseVisibleOculta(ByRef btnObj As Object, psClaseDefault As String)

      If btnObj.Visible Then
         'btnObj.Attributes.Remove("style")delete
         btnObj.CssClass = psClaseDefault.Replace("OcultarControl", "")
      Else
         'btnObj.Attributes.Add("style", "display:none")
         btnObj.CssClass = psClaseDefault & " OcultarControl"
         btnObj.Visible = True
      End If
   End Sub

   ''' <summary>
   ''' Valida si el usuario logueado tiene permisos para adjuntar documentos
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Private Function ValidaPermisoCargaDocumentos() As Boolean
      Dim lbTienePermiso As Boolean = True
      Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

      If (pObjPropiedadesProrroga.ppObjVisita.UsuarioEstaOcupando.Trim() <> puObjUsuario.IdentificadorUsuario.Trim()) And
                  pObjPropiedadesProrroga.ppObjVisita.UsuarioEstaOcupando.Trim().Length > 0 Then
         Exit Function
      End If


      ''Valida los permisos
      Select Case pObjPropiedadesProrroga.ppObjVisita.IdPasoActual
         Case 1
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.Registrado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.EnAjustes
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS ''Atiende observaciones VJ
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.AjustesRealizados, Constantes.EstatusPaso.AjustesEnviados ''Enviar nuevamente a VJ
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ', Constantes.PERFIL_INS
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 2
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.Enviado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.SupervisorAsignado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Supervisor asigna/modifica abogado asesor
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.AsesorAsignado ''Guarda despues de asignar asesor
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.ABOGADOS.PERFIL_ABO_ASESOR, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Revisado ''revisaso y asesor asignado paso 2
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.EnAjustes ''Guarda paso 2, EnAjustes
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.AjustesRealizados  ''Guarda paso 2, AjustesRealizados
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 3
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 3 guardar, en revision espera
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select

               Case Constantes.EstatusPaso.Revisado ''Notifica que ya todos los documentos estan bien, o rechaza paso 3
                  Select Case puObjUsuario.IdentificadorPerfilActual ''Rechaza los documentos que aprobo VJ paso 3
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 4
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 4, EnRevisionEspera
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Revisado  ''Paso 4, Revisado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 6
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual ''Paso 6 inicia visita
               Case Constantes.EstatusPaso.EnRevisionEspera
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                        If EsAreaOperativa(puObjUsuario.IdArea) Or puObjUsuario.IdArea = Constantes.AREA_VJ Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Visita_iniciada ''Paso 6 detener visita
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 7
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.En_diagnostico_de_hallazgos, Constantes.EstatusPaso.SinReunionPresidencia  ''paso 7 En_diagnostico_de_hallazgos
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP  ' Solo el inspector puede: *adjuntar documentos (hallazgos de visita)
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 8
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 8, EnRevisionEspera
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.HallazgosGuardados ''Paso 8 HallazgosGuardados
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select

               Case Constantes.EstatusPaso.Elaborada
                  If (pObjPropiedadesProrroga.ppObjVisita.FechaRegistro > fechaProcesoNvo) Then
                     Select Case puObjUsuario.IdentificadorPerfilActual
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP  'envia al paso 10
                           If EsAreaOperativa(puObjUsuario.IdArea) Then
                              lbTienePermiso = True
                           End If

                           ''Permitir editar la fecha de presentacion interna en el paso 8 a supervisor
                           If pObjPropiedadesProrroga.ppObjVisita.IdPasoActual = 8 And
                               (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP Or puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM) And
                               (puObjUsuario.IdArea <> Constantes.AREA_VJ) And
                               Not Fechas.Vacia(pObjPropiedadesProrroga.ppObjVisita.FECH_REUNION__PRESIDENCIA) Then
                              lbTienePermiso = True
                           End If
                     End Select
                  End If

               Case Constantes.EstatusPaso.EnAjustes
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP 'adjuntar los documentos
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                  End Select

               Case Constantes.EstatusPaso.EnEsperaPresentarHallazgos
                  lbTienePermiso = True
               Case Else
                  lbTienePermiso = False
            End Select
         Case 9
            'SI LA FECHA DE REGISTRO DE LA VISITA ES MAYOR A LA FECHA PARAMETRIZADA PARA EL NUEVO PROCESO
            If (pObjPropiedadesProrroga.ppObjVisita.FechaRegistro > fechaProcesoNvo) Then
               Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual ''Paso 10 EnRevisionEspera, EnAjustes
                  Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.EnAjustes
                     Select Case puObjUsuario.IdentificadorPerfilActual
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                           Select Case puObjUsuario.IdArea
                              Case Constantes.AREA_PR, Constantes.AREA_VJ
                                 lbTienePermiso = True
                              Case Else
                                 lbTienePermiso = False
                           End Select
                        Case Else
                           lbTienePermiso = False
                     End Select

                  Case Constantes.EstatusPaso.Revisado
                     Select Case puObjUsuario.IdentificadorPerfilActual
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Una vez revisado supervisor rechaza o avanza

                           Select Case puObjUsuario.IdArea
                              Case Constantes.AREA_PR, Constantes.AREA_VJ
                                 lbTienePermiso = True
                              Case Else
                                 lbTienePermiso = False
                           End Select

                        Case Else
                           lbTienePermiso = False
                     End Select
                  Case Constantes.EstatusPaso.AjustesRealizados
                     Select Case puObjUsuario.IdentificadorPerfilActual ''Paso 10 AjustesRealizados
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Una vez revisado supervisor rechaza o avanza
                           Select Case puObjUsuario.IdArea
                              Case Constantes.AREA_PR, Constantes.AREA_VJ
                                 lbTienePermiso = True
                              Case Else
                                 lbTienePermiso = False
                           End Select
                        Case Else
                           lbTienePermiso = False
                     End Select
                  Case Else
                     lbTienePermiso = False
               End Select
            Else
               'SI LA FECHA DE REGISTRO DE LA VISITA ES MENOR A LA FECHA PARAMETRIZADA DEL NUEVO PROCESO
               Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
                  Case Constantes.EstatusPaso.EnRevisionEspera
                     Select Case puObjUsuario.IdentificadorPerfilActual
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP 'adjuntar los documentos

                           If EsAreaOperativa(puObjUsuario.IdArea) Then
                              lbTienePermiso = True
                           Else
                              lbTienePermiso = False
                           End If

                        Case Else
                           lbTienePermiso = False
                     End Select
                  Case Constantes.EstatusPaso.Elaborada
                     Select Case puObjUsuario.IdentificadorPerfilActual
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP  'envia al paso 10
                           If EsAreaOperativa(puObjUsuario.IdArea) Then
                              lbTienePermiso = True
                           Else
                              lbTienePermiso = False
                           End If
                        Case Else
                           lbTienePermiso = False
                     End Select
                  Case Constantes.EstatusPaso.EnAjustes
                     Select Case puObjUsuario.IdentificadorPerfilActual
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP 'adjuntar los documentos

                           If (pObjPropiedadesProrroga.ppObjVisita.FechaRegistro > fechaProcesoNvo) Then
                              If EsAreaOperativa(puObjUsuario.IdArea) Then
                                 lbTienePermiso = False
                              Else
                                 lbTienePermiso = True
                              End If
                           Else
                              If EsAreaOperativa(puObjUsuario.IdArea) Then
                                 lbTienePermiso = True
                              Else
                                 lbTienePermiso = False
                              End If
                           End If

                        Case Else
                           lbTienePermiso = False
                     End Select
                  Case Constantes.EstatusPaso.AjustesRealizados
                     Select Case puObjUsuario.IdentificadorPerfilActual
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP  'envia al paso 10

                           If (pObjPropiedadesProrroga.ppObjVisita.FechaRegistro > fechaProcesoNvo) Then
                              If EsAreaOperativa(puObjUsuario.IdArea) Then
                                 lbTienePermiso = False
                              Else
                                 lbTienePermiso = True
                              End If
                           Else
                              If EsAreaOperativa(puObjUsuario.IdArea) Then
                                 lbTienePermiso = True
                              Else
                                 lbTienePermiso = False
                              End If
                           End If

                        Case Else
                           lbTienePermiso = False
                     End Select
                  Case Else
                     lbTienePermiso = False
               End Select
            End If
         Case 10
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual ''Paso 10 EnRevisionEspera, EnAjustes
               Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.EnAjustes
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                        If (pObjPropiedadesProrroga.ppObjVisita.FechaRegistro > fechaProcesoNvo) Then
                           Select Case puObjUsuario.IdArea
                              Case Constantes.AREA_PR, Constantes.AREA_VJ
                                 lbTienePermiso = False
                              Case Else
                                 lbTienePermiso = True
                           End Select
                        Else
                           Select Case puObjUsuario.IdArea
                              Case Constantes.AREA_PR, Constantes.AREA_VJ
                                 lbTienePermiso = True
                              Case Else
                                 lbTienePermiso = False
                           End Select
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select

               Case Constantes.EstatusPaso.Revisado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Una vez revisado supervisor rechaza o avanza
                        If (pObjPropiedadesProrroga.ppObjVisita.FechaRegistro > fechaProcesoNvo) Then
                           Select Case puObjUsuario.IdArea
                              Case Constantes.AREA_PR, Constantes.AREA_VJ
                                 lbTienePermiso = False
                              Case Else
                                 lbTienePermiso = True
                           End Select
                        Else
                           Select Case puObjUsuario.IdArea
                              Case Constantes.AREA_PR, Constantes.AREA_VJ
                                 lbTienePermiso = True
                              Case Else
                                 lbTienePermiso = False
                           End Select
                        End If

                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.AjustesRealizados
                  Select Case puObjUsuario.IdentificadorPerfilActual ''Paso 10 AjustesRealizados
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Una vez revisado supervisor rechaza o avanza
                        If (pObjPropiedadesProrroga.ppObjVisita.FechaRegistro > fechaProcesoNvo) Then
                           Select Case puObjUsuario.IdArea
                              Case Constantes.AREA_PR, Constantes.AREA_VJ
                                 lbTienePermiso = False
                              Case Else
                                 lbTienePermiso = True
                           End Select
                        Else
                           Select Case puObjUsuario.IdArea
                              Case Constantes.AREA_PR, Constantes.AREA_VJ
                                 lbTienePermiso = True
                              Case Else
                                 lbTienePermiso = False
                           End Select
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 12
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.Notificado ''Paso 12 rechaza
                  Select Case puObjUsuario.IdentificadorPerfilActual


                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS   'Supervisor avisa de la version final de los documentos

                        If (pObjPropiedadesProrroga.ppObjVisita.FechaRegistro > fechaProcesoNvo) Then
                           If EsAreaOperativa(puObjUsuario.IdArea) Then
                              lbTienePermiso = True
                           Else
                              lbTienePermiso = False
                           End If
                        ElseIf puObjUsuario.IdentificadorPerfilActual <> Constantes.PERFIL_INS Then
                           If EsAreaOperativa(puObjUsuario.IdArea) Then
                              lbTienePermiso = True
                           Else
                              lbTienePermiso = False
                           End If
                        End If

                     Case Else
                        lbTienePermiso = False



                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 13
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS ''adjunta documentos del paso 13
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select

               Case Constantes.EstatusPaso.Hallazgos_presentados
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Manda fecha que confirmo sandra pacheco
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 14
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Registrado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select
         Case 15
            If (pObjPropiedadesProrroga.ppObjVisita.FechaRegistro > fechaProcesoNvo) Then
               Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
                  Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.EnAjustes ''Flujo inicial paso 17, EnRevisionEspera, ''Flujo 2 paso 17, EnAjustes
                     Select Case puObjUsuario.IdentificadorPerfilActual
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_ASESOR, Constantes.PERFIL_INS 'El Supervisor / Superior  puede:  *Enviar documentos a revisión y por lo tanto dar vo.bo, el abogado e inspector pueden adjuntar documentos (hacer ajustes a los docs)
                           If EsAreaOperativa(puObjUsuario.IdArea) Then
                              lbTienePermiso = True
                           Else
                              lbTienePermiso = False
                           End If
                     End Select
               End Select
            End If

         Case 16

            If (pObjPropiedadesProrroga.ppObjVisita.FechaRegistro > fechaProcesoNvo) Then
               Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
                  Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.EnAjustes ''Flujo inicial paso 17, EnRevisionEspera, ''Flujo 2 paso 17, EnAjustes
                     Select Case puObjUsuario.IdentificadorPerfilActual
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_ASESOR, Constantes.PERFIL_INS 'El Supervisor / Superior  puede:  *Enviar documentos a revisión y por lo tanto dar vo.bo, el abogado e inspector pueden adjuntar documentos (hacer ajustes a los docs)
                           Select Case puObjUsuario.IdArea
                              Case Constantes.AREA_PR, Constantes.AREA_VJ
                                 lbTienePermiso = True
                              Case Else
                                 lbTienePermiso = False
                           End Select
                        Case Else
                           lbTienePermiso = False
                     End Select
                  Case Constantes.EstatusPaso.Revisado, Constantes.EstatusPaso.AjustesRealizados ''Flujo inicial paso 17, Revisado, ''Flujo inicial paso 17, Revisado, regresa paso 16
                     Select Case puObjUsuario.IdentificadorPerfilActual
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Flujo 2 paso 17, AjustesRealizados, ''Flujo 2 paso 17 regresa paso 16, AjustesRealizados
                           Select Case puObjUsuario.IdArea
                              Case Constantes.AREA_PR, Constantes.AREA_VJ
                                 lbTienePermiso = True
                              Case Else
                                 lbTienePermiso = False
                           End Select
                        Case Else
                           lbTienePermiso = False
                     End Select
                  Case Else
                     lbTienePermiso = False
               End Select
            Else
               Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
                  Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.EnAjustes ''Flujo inicial paso 16, ''Flujo 2 paso 16, EnAjustes
                     Select Case puObjUsuario.IdentificadorPerfilActual
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS

                           If (pObjPropiedadesProrroga.ppObjVisita.FechaRegistro > fechaProcesoNvo) Then
                              If EsAreaOperativa(puObjUsuario.IdArea) Then
                                 lbTienePermiso = False
                              Else
                                 lbTienePermiso = True
                              End If
                           Else
                              If EsAreaOperativa(puObjUsuario.IdArea) Then
                                 lbTienePermiso = True
                              Else
                                 lbTienePermiso = False
                              End If
                           End If
                        Case Else
                           lbTienePermiso = False
                     End Select
                  Case Constantes.EstatusPaso.Revisado, Constantes.EstatusPaso.AjustesRealizados ''Flujo inicial paso 16, revisado, ''Flujo 2 paso 16, AjustesRealizados
                     Select Case puObjUsuario.IdentificadorPerfilActual
                        Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP

                           If (pObjPropiedadesProrroga.ppObjVisita.FechaRegistro > fechaProcesoNvo) Then
                              If EsAreaOperativa(puObjUsuario.IdArea) Then
                                 lbTienePermiso = False
                              Else
                                 lbTienePermiso = True
                              End If
                           Else
                              If EsAreaOperativa(puObjUsuario.IdArea) Then
                                 lbTienePermiso = True
                              Else
                                 lbTienePermiso = False
                              End If
                           End If

                        Case Else
                           lbTienePermiso = False
                     End Select
                  Case Else
                     lbTienePermiso = False
               End Select
            End If

         Case 17
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.EnAjustes ''Flujo inicial paso 17, EnRevisionEspera, ''Flujo 2 paso 17, EnAjustes
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_ASESOR, Constantes.PERFIL_INS 'El Supervisor / Superior  puede:  *Enviar documentos a revisión y por lo tanto dar vo.bo, el abogado e inspector pueden adjuntar documentos (hacer ajustes a los docs)
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Revisado, Constantes.EstatusPaso.AjustesRealizados ''Flujo inicial paso 17, Revisado, ''Flujo inicial paso 17, Revisado, regresa paso 16
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Flujo 2 paso 17, AjustesRealizados, ''Flujo 2 paso 17 regresa paso 16, AjustesRealizados
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select
         Case 18
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Flujo inicial paso 18, EnRevisionEspera
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Revisado ''Flujo inicial paso 18, Revisado, ''Flujo inicial paso 18, Revisado, regresa paso 17
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 19
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS  ''Paso 19 guardar comentarios
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Revisado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Paso 19 manda a paso 20
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 20
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera 'Tiene que asignar el abogado que asesorara en la sancion
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.SupervisorAsignado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Supervisor asigna/modifica abogado sancion
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Asignado  ''Paso 20 pasa a paso 21, Asignado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select

               Case Constantes.EstatusPaso.Revisado   ''Paso 20 pasa a paso 21, Revisado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select

               Case Constantes.EstatusPaso.AjustesRealizados  ''Paso 20 pasa a paso 21
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select

               Case Constantes.EstatusPaso.EnAjustes  ''Regresa de paso 21
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 21
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 21 guarda comentarios, EnRevisionEspera
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Revisado ''Paso 21 guarda comentarios, Revisado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 22
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Notifica fecha de posible emplazamiento
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 23
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 23
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select
         Case 24
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 24
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 25
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 25
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 26
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 26, Elaborada
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                        If EsAreaOperativa(puObjUsuario.IdArea) Or puObjUsuario.IdArea = Constantes.AREA_VJ Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 27
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 27, Notificado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 28
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 28, Impuesto
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 29
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 29, Revisado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Revisado   ''Paso 29, Pagado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 30
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 30
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.SupervisorAsignado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Supervisor asigna/modifica abogado contencioso
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Asignado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select
         Case 31
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 31
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select
         Case 32
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.Revocacion, Constantes.EstatusPaso.Nulidad, Constantes.EstatusPaso.Sin_respuesta
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                        If EsAreaOperativa(puObjUsuario.IdArea) Or puObjUsuario.IdArea = Constantes.AREA_VJ Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 33
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 33, EnRevisionEspera
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Respuesta_Elaborada ''Paso 33, Respuesta_Elaborada
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 35
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 35, Inicia_revision
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Solo el Supervisor / Superior   puede:  *Enviar documentos a revisión
                        If EsAreaOperativa(puObjUsuario.IdArea) Then
                           lbTienePermiso = True
                        Else
                           lbTienePermiso = False
                        End If
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Respuesta_en_revisión ''Paso 35, Respuesta_en_revisión
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Constantes.EstatusPaso.Modificado ''Paso 35, Modificado
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select

         Case 36
            Select Case pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 36
                  Select Case puObjUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                        Select Case puObjUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              lbTienePermiso = True
                           Case Else
                              lbTienePermiso = False
                        End Select
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select
      End Select


      Return lbTienePermiso
   End Function

   ''' <summary>
   ''' Valida que una area sea operativa o sea presidencia
   ''' </summary>
   ''' <param name="idAreaActual"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Private Function EsAreaOperativa(idAreaActual As Integer) As Boolean
      If (From objA As Entities.Area In Constantes.GetAreasOperativas() Where objA.Identificador = idAreaActual Select objA).Count > 0 Then
         Return True
      Else
         Return False
      End If
   End Function

   Private Function ValidaPermisoCargaDocumentosPdf() As Boolean
      Dim lbTienePermiso As Boolean = False

      ''Valida los permisos
      Select Case pObjPropiedadesProrroga.ppObjVisita.IdPasoActual
         Case 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18
            Select Case puObjUsuario.IdentificadorPerfilActual
               Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                  If EsAreaOperativa(puObjUsuario.IdArea) Then
                     lbTienePermiso = True
                  Else
                     lbTienePermiso = False
                  End If
               Case Else
                  lbTienePermiso = False
            End Select
         Case 23
            Select Case puObjUsuario.IdentificadorPerfilActual
               Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_ASESOR, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                  Select Case puObjUsuario.IdArea
                     Case Constantes.AREA_PR, Constantes.AREA_VJ
                        lbTienePermiso = True
                     Case Else
                        lbTienePermiso = False
                  End Select
               Case Else
                  lbTienePermiso = False
            End Select
         Case Else
            lbTienePermiso = False
      End Select

      Return lbTienePermiso
   End Function
#End Region

   Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
      If IsNothing(pObjPropiedadesProrroga) Then Exit Sub

      If Not IsPostBack Then
         pnlNoExiste.Visible = False

         'gvConsultaDocs.WidthScroll = pObjPropiedadesProrroga.piIdWidthPanel

         If Not IsNothing(puObjUsuario) Then
            ''Carga el filtro
            'CargarFiltros()

            'Permiso para editar pdf
            If puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM Or pObjPropiedadesProrroga.pbEstaInsertandoVisita Or pObjPropiedadesProrroga.pbEstaEditandoVisita Or pObjPropiedadesProrroga.pbEsSubFolioSubVisita Then
               pObjPropiedadesProrroga.PermisoEditarPDF = True
            Else
               pObjPropiedadesProrroga.PermisoEditarPDF = False
            End If

            If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
               ''Permisos editar documentos que no son PDF
               pObjPropiedadesProrroga.PermisoEditarDocs = ValidaPermisoCargaDocumentos()

               ''VALIDA PERMISO DE SOLO LECTURA
               If puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SOLO_LEC Or
                   pObjPropiedadesProrroga.ppObjVisita.EsCancelada Then
                  pObjPropiedadesProrroga.PermisoEditarDocs = False
               End If
            Else
               pObjPropiedadesProrroga.PermisoEditarDocs = True
            End If

            ''Genera los documentos
            CargaDocumentos()
         End If
      Else
         CargaDocumentos()

         ''Capturar el documento SICOD
         If Not IsNothing(Session("T_HYP_ARCHIVOSCAN")) Then
            Dim liRen As Integer = 0
            If hfIdRenglon.Value <> "-1" And Int32.TryParse(hfIdRenglon.Value, liRen) And (Me.ID.Contains("_" & hfVisita.Value) Or (hfVisita.Value = Constantes.Todos And (pObjPropiedadesProrroga.pbEstaEditandoVisita Or pObjPropiedadesProrroga.pbEstaInsertandoVisita))) Then
               If Session("T_HYP_ARCHIVOSCAN").ToString().Trim() <> "&nbsp;" And Session("T_HYP_ARCHIVOSCAN").ToString().Trim() <> "" Then
                  Dim splitName As String() = Session("T_HYP_ARCHIVOSCAN").ToString.Split(New Char() {"\"c})
                  Session.Remove("T_HYP_ARCHIVOSCAN")
                  CargarArchivoSharePointDesdeSicod(splitName.Last & "__scd")
                  hfIdRenglon.Value = "-1"

                  CargaMasiva_SICOD()
               Else
                  MensajeDocs = "El oficio seleccionado en SICOD esta vacío."
                  ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
               End If
            End If
         End If
      End If

      'btnCargaMasiva.Visible = pObjPropiedadesProrroga.PermisoEditarDocs
      'btnCargaMasiva.Enabled = pObjPropiedadesProrroga.PermisoEditarDocs

      'If pObjPropiedadesProrroga.ppObjVisita.IdPasoActual = 1 Then
      '   imgRegresarDocs.Enabled = True
      '   imgRegresarDocs.Visible = True
      'Else
      '   If pObjPropiedadesProrroga.pbEstaInsertandoVisita Or pObjPropiedadesProrroga.pbEstaEditandoVisita Then
      '      imgRegresarDocs.Enabled = False
      '      imgRegresarDocs.Visible = False
      '   Else
      '      imgRegresarDocs.Enabled = True
      '      imgRegresarDocs.Visible = True
      '   End If
      'End If

      pnlGridInt.Attributes.Add("onscroll", "GuardaScrollDocs('" & pnlGridInt.ID & "','" & hfValorScrollDocs.ID & "');")
      pnlGridInt.Attributes.Remove("scrollTop")
      pnlGridInt.Attributes.Add("scrollTop", hfValorScrollDocs.Value)
      ScriptManager.RegisterStartupScript(Me.Page, pnlGridInt.GetType(), "Scroll", "SetScroll('" & pnlGridInt.ID & "','" & hfValorScrollDocs.ID & "');", True)
   End Sub

   Private Sub CargaMasiva_SICOD()

      ''Recorrer todo el grid en busca de fileuploads
      Dim lstImgBtn As List(Of ImageButton) = gvConsultaDocs.GetAllControlsOfType(Of ImageButton)()
      Dim lstFiltradaBtns = From imgBtn In lstImgBtn Where imgBtn.ID.Contains("btnCargaDoc") Select imgBtn

      Dim FechaRegVisita = CDate(pObjPropiedadesProrroga.ppObjVisita.FechaRegistro.ToString())
      Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

      If lstFiltradaBtns.Count > 0 Then
         ''Dim liLimiteArchivoCarga As Integer = ObtenerTamMaximoArch()
         Dim lstErrores As New List(Of String)

         Dim lstfileUp As List(Of FileUpload) = gvConsultaDocs.GetAllControlsOfType(Of FileUpload)()
         Dim liFiUpConArchivo As Integer = (From fuFile In lstfileUp Where fuFile.HasFile = True).Count()

         If liFiUpConArchivo > 0 Then
            For Each btnCargar As ImageButton In lstFiltradaBtns
               CargarArchivoSharePointMasiva(btnCargar, 1, lstErrores)
            Next

            If (Convert.ToDateTime(FechaRegVisita).ToString("yyyy/MM/dd") < Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
               Response.Redirect("../Procesos/DetalleVisita.aspx")
            Else
               Response.Redirect("../Procesos/DetalleVisita_V17.aspx")
            End If

         End If
      End If

   End Sub

   ''' <summary>
   ''' Carga los documentos en el grid
   ''' </summary>
   ''' <remarks></remarks>
   Public Sub CargaDocumentos()

      Dim objDocumentos As New Documento

      If IsNothing(pObjPropiedadesProrroga.peObjExp) OrElse IsNothing(pObjPropiedadesProrroga.DataSourceSession) Then
         If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
            If pObjPropiedadesProrroga.pbEstaEditandoVisita And pObjPropiedadesProrroga.ppObjVisita.EsSubVisita Then ''EDITANDO SUBVISITAS
               If IsNothing(pObjPropiedadesProrroga.DataSourceSession) Then pObjPropiedadesProrroga.DataSourceSession = objDocumentos.getDocumentos(pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado, Constantes.Vigencia.Vigente, pObjPropiedadesProrroga.ppObjVisita.IdPasoActual, 9)
               If IsNothing(pObjPropiedadesProrroga.peObjExp) Then
                  Dim objExp As New Expediente(Constantes.Vigencia.Vigente, pObjPropiedadesProrroga.ppObjVisita.IdPasoActual, pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado)
                  pObjPropiedadesProrroga.peObjExp = objExp
               End If
            ElseIf pObjPropiedadesProrroga.pbEstaEditandoVisita And pObjPropiedadesProrroga.ppObjVisita.EsSubVisitaOsubFolio Then ''EDITANDO SUBFOLIOS
               If IsNothing(pObjPropiedadesProrroga.DataSourceSession) Then pObjPropiedadesProrroga.DataSourceSession = objDocumentos.getDocumentos(pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado, Constantes.Vigencia.Vigente, 9)
               If IsNothing(pObjPropiedadesProrroga.peObjExp) Then
                  Dim objExp As New Expediente(Constantes.Vigencia.Vigente, Constantes.Todos, pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado)
                  pObjPropiedadesProrroga.peObjExp = objExp
               End If
            ElseIf pObjPropiedadesProrroga.pbEstaEditandoVisita Then ''EDITANDO CUALQUIER OTRA COSAS
               If IsNothing(pObjPropiedadesProrroga.DataSourceSession) Then pObjPropiedadesProrroga.DataSourceSession = objDocumentos.getDocumentos(pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado, Constantes.Vigencia.Vigente, pObjPropiedadesProrroga.ppObjVisita.IdPasoActual, 9)
               If IsNothing(pObjPropiedadesProrroga.peObjExp) Then
                  Dim objExp As New Expediente(Constantes.Vigencia.Vigente, pObjPropiedadesProrroga.ppObjVisita.IdPasoActual, pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado)
                  pObjPropiedadesProrroga.peObjExp = objExp
               End If
            Else ''CONSULTANDO DESDE DETALLE
               If IsNothing(pObjPropiedadesProrroga.DataSourceSession) Then pObjPropiedadesProrroga.DataSourceSession = objDocumentos.getDocumentos(pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado, Constantes.Vigencia.Vigente, -1, -1, "", -1, 9)
               If IsNothing(pObjPropiedadesProrroga.peObjExp) Then
                  Dim objExp As New Expediente(Constantes.Vigencia.Vigente, Constantes.Todos, pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado)
                  pObjPropiedadesProrroga.peObjExp = objExp
               End If
            End If

            pObjPropiedadesProrroga.piIdVisitaActualDoc = pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado
         Else ''REGISTRO
            ''Ya debe estar lleno el paso externamente
            If IsNothing(pObjPropiedadesProrroga.DataSourceSession) Then pObjPropiedadesProrroga.DataSourceSession = objDocumentos.getDocumentos(pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado, Constantes.Vigencia.Vigente, pObjPropiedadesProrroga.piIdPasoDocumentos, 9)
            If IsNothing(pObjPropiedadesProrroga.peObjExp) Then
               Dim objExp As New Expediente(Constantes.Vigencia.Vigente, pObjPropiedadesProrroga.piIdPasoDocumentos, Constantes.Todos, pgIdentificadorUsuario)
               pObjPropiedadesProrroga.peObjExp = objExp
            End If
            pObjPropiedadesProrroga.piIdVisitaActualDoc = Constantes.Todos
         End If
      End If

      ''Genera el grid la primera vez
      If Not IsNothing(pObjPropiedadesProrroga.peObjExp) Then
         ''Agregar el origen de datos al grid
         If Not IsNothing(pObjPropiedadesProrroga.DataSourceSession) Then

            gvConsultaDocs.DataSource = pObjPropiedadesProrroga.DataSourceSession
            Dim objAnonimo = New With {.I_ID_PASO_INI = "PASO", .T_NOM_DOCUMENTO_CAT = "DOCUMENTO", .T_NOM_DOCUMENTO_ADJ = "DOCUMENTOS ADJUNTOS"}
            'Dim objAnonimo = New With {.T_NOM_DOCUMENTO_CAT = "DOCUMENTO", .T_NOM_DOCUMENTO_ADJ = "DOCUMENTOS ADJUNTOS"}
            Dim lstObjects As New List(Of Object)
            lstObjects.Add(objAnonimo)
            gvEncabecados.DataSource = lstObjects
            gvEncabecados.DataBind()
            Try
               gvConsultaDocs.DataBind()
               ActualizaAnchoDiv()
            Catch ex As Exception
               Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "ucDocumentos.aspx.vb, GeneraDocumentos", "")
            End Try
         End If
      End If
   End Sub

   ''' <summary>
   ''' Metodo generico que muestra los archivos almacenados en el sharepoint
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Private Sub MostrarArchivo(sender As Object, e As CommandEventArgs)
      Dim lnkLink As LinkButton = CType(sender, LinkButton)
      If Not IsNothing(lnkLink) Then
         Try
            ''El nombre real del documento en el sharepoint debe de llegar en el comandArgument
            ''Si no llega ahi, buscar el archivo mediante el nombre que hay en la propiedad text
            If lnkLink.CommandArgument.Length > 0 Then
               Dim Shp As New Utilerias.SharePointManager
               Shp.NombreArchivo = lnkLink.CommandArgument

               If lnkLink.CommandArgument.Contains("__scd") Then
                  Shp.NombreArchivo = lnkLink.CommandArgument.Replace("__scd", "")
                  ConfigurarSharePointSeprisSicod(Shp)
               ElseIf lnkLink.CommandArgument.Contains("__svg") Then
                  Shp.NombreArchivo = lnkLink.CommandArgument.Replace("__svg", "")
                  ConfigurarSharePointSeprisSisvig(Shp)
               Else
                  ConfigurarSharePointSepris(Shp)
               End If

               Shp.VisualizarArchivoSepris(lnkLink.Text)
            ElseIf lnkLink.Text.Length > 0 Then
               Dim Shp As New Utilerias.SharePointManager
               Shp.NombreArchivo = lnkLink.Text

               If lnkLink.Text.Contains("__scd") Then
                  Shp.NombreArchivo = lnkLink.Text.Replace("__scd", "")
                  ConfigurarSharePointSeprisSicod(Shp)
               ElseIf lnkLink.Text.Contains("__svg") Then
                  Shp.NombreArchivo = lnkLink.Text.Replace("__svg", "")
                  ConfigurarSharePointSeprisSisvig(Shp)
               Else
                  ConfigurarSharePointSepris(Shp)
               End If

               Shp.VisualizarArchivoSepris(lnkLink.Text)
            End If
         Catch ex As Exception
            'Se comento porque manda erroraun descargando el archivo de forma correcta
            'Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            'Utilerias.ControlErrores.EscribirEvento("Ocurrio un error al recuperar el archivo.", EventLogEntryType.Error, "SEPRIS", ex.Message)
            MensajeDocs = "Ocurrio un error al recuperar el archivo."
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
         End Try
      End If
   End Sub

   ''' <summary>
   ''' Metodo generico que carga un archivo en el sharepoint y actualiza en la base de datos el archivo
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Private Sub CargarArchivoSharePoint(sender As Object, e As CommandEventArgs)
      Dim btnCargar As ImageButton = TryCast(sender, ImageButton)
      Dim lsNomControl As String = btnCargar.ID.Replace("btn", "")
      ''REENGLON|ID_DOCUMENTO|VERSION|TIPO_DOCUMENTO[WORD o PDF]|PASO_DOCUMENTO|BANDERA_NOTIFICACION|BANDERA_TERMINA_CARGA
      Dim vecDatos() As String = btnCargar.CommandArgument.Split("|")

      Dim liIdRen As Integer = vecDatos(0)
      Dim liIdDoc As Integer = vecDatos(1)
      Dim liVer As Integer = vecDatos(2)
      Dim liIdTipoDoc As Integer = vecDatos(3)
      Dim liIdPaso As Integer = vecDatos(4)
      Dim liBanNotificacion As Integer = vecDatos(5)
      Dim liBanTerminaCarga As Integer = vecDatos(6)
      Dim liIdTipoDocTermCarga As Integer = vecDatos(7)
      Dim liNumVersiones As Integer = vecDatos(8)
      Dim liHeredar As Integer = vecDatos(9)
      Dim liHeredarSbVisitas As Integer = vecDatos(10)
      Dim lbArchivoSisvig = False

      If IsNothing(puObjUsuario) Then Exit Sub

      Dim gvGridRow As GridViewRow = gvConsultaDocs.Rows(liIdRen)
      Dim btnLink As LinkButton = gvGridRow.FindControl("lnk" & lsNomControl)
      'Dim btnImg As ImageButton = gvGridRow.FindControl("img" & lsNomControl)
      Dim fuFileUp As FileUpload = gvGridRow.FindControl("fu" & lsNomControl)

      Dim FechaRegVisita = CDate(pObjPropiedadesProrroga.ppObjVisita.FechaRegistro.ToString())
      Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

      If Not IsNothing(fuFileUp) Then
         If fuFileUp.FileBytes.Length <= 0 Then
            MensajeDocs = "Seleccione un archivo."
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "MensajeConfirmacionProrroga();AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
            Exit Sub
         Else
            Dim lsExtArchivo As String = System.IO.Path.GetExtension(fuFileUp.FileName)

            ''Valida el tipo de archivo
            If (liIdTipoDoc = Constantes.TipoArchivo.WORD And (lsExtArchivo <> ".doc" And lsExtArchivo <> ".docx")) Or
                (liIdTipoDoc = Constantes.TipoArchivo.PDF And lsExtArchivo <> ".pdf") Then
               btnCargar.Visible = True
               fuFileUp.Visible = True
               btnLink.Visible = False
               'btnImg.Visible = False

               PonerClaseVisibleOculta(btnCargar, btnCargar.CssClass)
               PonerClaseVisibleOculta(fuFileUp, fuFileUp.CssClass)
               PonerClaseVisibleOculta(btnLink, btnLink.CssClass)
               'PonerClaseVisibleOculta(btnImg, btnImg.CssClass)

               If liIdTipoDoc = Constantes.TipoArchivo.WORD Then
                  MensajeDocs = "Archivo WORD no válido."
               Else
                  MensajeDocs = "Archivo PDF no válido."
               End If

               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
               Exit Sub
            End If

            ''VALIDAR SI ES UN ARCHIVO DE SISVIG
            If btnLink.CommandArgument.Contains("__svg") Then
               lbArchivoSisvig = True
            End If

            Dim Shp As New Utilerias.SharePointManager

            If lbArchivoSisvig Then
               ConfigurarSharePointSeprisSisvig(Shp)
            Else
               ConfigurarSharePointSepris(Shp)
            End If

            '---------------------------------------
            ' Guarda el archivo en Sharepoint
            '---------------------------------------
            Dim lsAuxNombreDoc As String = ""
            Dim lsAuxOriNombre As String = ""

            ''BUSCA ARCHIVO EN EXPEDIENTE
            Dim objDocumentoExp As Documento = (From doc In pObjPropiedadesProrroga.peObjExp.lstDocumentos Where doc.N_ID_DOCUMENTO = liIdDoc And doc.N_ID_VERSION = liVer).FirstOrDefault()

            If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
               lsAuxNombreDoc = ObtenerNombreDocumentoArchivoSepris(liIdDoc, liVer, lsExtArchivo, (liNumVersiones + 1), objDocumentoExp)
            Else
               lsAuxNombreDoc = ObtenerNombreDocumentoArchivoSepris(liIdDoc, liVer, lsExtArchivo, liNumVersiones, objDocumentoExp)
            End If

            lsAuxNombreDoc = IIf((lsAuxNombreDoc.Trim().Length > 1), lsAuxNombreDoc, fuFileUp.FileName)

            ''Obtiene nombre original del documento, como lo vera el usuario, ya sea con la nomeclatura o sin ella
            ''SIEMPRE SE DEBE DE GENERAR UNA NOMECLATURA YA QUE ESE ES EL MOMBRE CON EL QUE VA A QUEDAR EN SHAREPOINT
            'PORQUE SI NO EN DIFERENTES VISITAS SE PODRIA GENERAR EL MISMO ARCHIVO
            If Not IsNothing(objDocumentoExp) Then
               If objDocumentoExp.N_FLAG_APLICA_NOMENCLATURA = Constantes.Verdadero Then
                  lsAuxOriNombre = lsAuxNombreDoc
               Else
                  lsAuxOriNombre = fuFileUp.FileName
               End If
            Else
               lsAuxOriNombre = fuFileUp.FileName
            End If

            ''Obtiene nombre real del documento a como quedar en sharepoint
            Shp.NombreArchivo = Utilerias.Generales.ObtenerNombreArchivo(lsAuxNombreDoc)

            ''Guarda el archivo en el servidor de APP
            Dim lsRutaTemp As String = Path.GetTempPath()

            ''Lo elimina si existe
            EliminaArchivoTemporal(lsRutaTemp & fuFileUp.FileName)

            Try
               fuFileUp.SaveAs(lsRutaTemp & fuFileUp.FileName)
            Catch ex As Exception
               Utilerias.ControlErrores.EscribirEvento("Faltan permisos para CREAR el documento temporal que se genera en el servidor en la carpeta [" & Path.GetTempPath() & "] antes de enviarse a SharePoint" &
                                                   ex.ToString(), EventLogEntryType.Error, "ucDoctosProrroga.aspx.vb, CargarArchivoSharePoint", "")
            End Try

            If Not File.Exists(lsRutaTemp & fuFileUp.FileName) Then
               MensajeDocs = "No se pudo guardar temporalmente el documento en Servidor Web."
               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
               Exit Sub
            End If

            Shp.RutaArchivo = lsRutaTemp
            Shp.NombreArchivoOri = fuFileUp.FileName

            If Not Shp.UploadFileToSharePoint() Then
               ''Elimina el archivo en el servidor de APP
               EliminaArchivoTemporal(lsRutaTemp & fuFileUp.FileName)
               MensajeDocs = "No se pudo guardar el documento en SharePoint."
               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
               Exit Sub
            Else

               Dim objDocumento As New Entities.FileSepris

               If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                  objDocumento.I_ID_VISITA = pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado
                  objDocumento.I_ID_PASO = liIdPaso
                  objDocumento.N_ID_DOCUMENTO = liIdDoc
                  objDocumento.T_NOM_DOCUMENTO = IIf(lbArchivoSisvig, Shp.NombreArchivo & "__svg", Shp.NombreArchivo)
                  objDocumento.T_NOM_DOCUMENTO_ORI = lsAuxOriNombre
                  objDocumento.N_ID_VERSION = liVer
                  objDocumento.T_DSC_COMENTARIO = BuscaComentarios()
                  objDocumento.N_ID_TIPO_DOCUMENTO = liIdTipoDoc
                  objDocumento.T_ID_USUARIO = puObjUsuario.IdentificadorUsuario
                  objDocumento.I_ID_ESTATUS = pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
                  'objDocumento.N_ID_DOCUMENTO_PASO = pObjPropiedadesProrroga.ppObjVisita.IdPasoActual 'MCS
                  objDocumento.N_ID_DOCUMENTO_PASO = liIdPaso
                  objDocumento.NUM_VERSIONES = liNumVersiones

                  If Not objDocumento.AltaDocumento() Then
                     MensajeDocs = "No se pudo reemplazar el documento en base de datos."
                     ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
                     Exit Sub
                  Else
                     If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                        Dim bitacora As New Conexion.Bitacora("Adjunta documento" & "(" & lsAuxOriNombre & "), a la visita" & "(" & pObjPropiedadesProrroga.ppObjVisita.FolioVisita & ")", System.Web.HttpContext.Current.Session.SessionID, puObjUsuario.IdentificadorUsuario)
                        bitacora.Finalizar(True)
                     End If

                     If (AccesoBD.consultarEsVisitaDeSISVIG(objDocumento.I_ID_VISITA)) Then
                        CargarEnSisvig(objDocumento.I_ID_VISITA, liIdDoc, liIdPaso, liIdTipoDoc, Shp.NombreArchivo, lsRutaTemp, fuFileUp.FileName)
                     End If

                     ''Refrescar el expediente en memoria y el grid de consulta
                     RefrezcaGridExpediente()

                     ''Filtra de nuevo
                     CargarCatalogo()
                     End If
               Else
                     objDocumento.I_ID_PASO = liIdPaso
                     objDocumento.N_ID_DOCUMENTO = liIdDoc
                     objDocumento.T_NOM_DOCUMENTO = IIf(lbArchivoSisvig, Shp.NombreArchivo & "__svg", Shp.NombreArchivo)
                     objDocumento.T_NOM_DOCUMENTO_ORI = lsAuxOriNombre
                     objDocumento.N_ID_VERSION = liVer
                     objDocumento.T_DSC_COMENTARIO = BuscaComentarios()
                     objDocumento.N_ID_TIPO_DOCUMENTO = liIdTipoDoc
                     objDocumento.T_ID_USUARIO = puObjUsuario.IdentificadorUsuario

                     If Not objDocumento.AltaDocumentoUsuario() Then
                        MensajeDocs = "No se pudo reemplazar el documento en base de datos."
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
                        Exit Sub
                     Else

                     If (AccesoBD.consultarEsVisitaDeSISVIG(objDocumento.I_ID_VISITA)) Then
                        CargarEnSisvig(objDocumento.I_ID_VISITA, liIdDoc, liIdPaso, liIdTipoDoc, Shp.NombreArchivo, lsRutaTemp, fuFileUp.FileName)
                     End If
                     ''Refrescar el expediente en memoria y el grid de consulta
                     RefrezcaGridExpediente()

                     ''Filtra de nuevo
                     CargarCatalogo()

                     ''Deja registro en bitacora
                     If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                        Dim bitacora As New Conexion.Bitacora("Carga Expediente Visita" & "(" & pObjPropiedadesProrroga.ppObjVisita.FolioVisita & ")", System.Web.HttpContext.Current.Session.SessionID, puObjUsuario.IdentificadorUsuario)
                        bitacora.Finalizar(True)
                     End If
                     End If
               End If
               ''Elimina el archivo en el servidor de APP
               EliminaArchivoTemporal(lsRutaTemp & fuFileUp.FileName)

            End If
            ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            btnLink.Visible = True
            btnLink.CommandArgument = IIf(lbArchivoSisvig, Shp.NombreArchivo & "__svg", Shp.NombreArchivo)
            btnLink.Text = lsAuxOriNombre

            'If liIdTipoDoc = Constantes.TipoArchivo.PDF Then
            '   btnImg.Visible = pObjPropiedadesProrroga.PermisoEditarPDF
            'Else
            '   btnImg.Visible = True
            'End If

            fuFileUp.Visible = False
            btnCargar.Visible = False

            PonerClaseVisibleOculta(btnCargar, btnCargar.CssClass)
            PonerClaseVisibleOculta(fuFileUp, fuFileUp.CssClass)
            PonerClaseVisibleOculta(btnLink, btnLink.CssClass)
            'PonerClaseVisibleOculta(btnImg, btnImg.CssClass)

            ''Valida si despues de cargar el documento es nacesario pedir una confirmacion para terminar la carga de documentos
            ''valida que haya una visita en especifico
            ''Que no pida la confirmacion si esta editando una sub visita un un subfolio
            If liBanNotificacion = Constantes.Verdadero And liBanTerminaCarga = Constantes.Falso And
                pObjPropiedadesProrroga.piIdVisitaActualDoc <> Constantes.Todos And Not pObjPropiedadesProrroga.pbEstaEditandoVisita Then
               ''Dim objEtiqueta As New Entities.EtiquetaError(2139)
               MensajeDocs = Constantes.MensajesModal.FinalizaCargaDoctos

               btnConfirmarDocsSI.CommandArgument = liIdDoc.ToString() &
                                                   "|" & liIdPaso.ToString() &
                                                   "|" & liIdTipoDocTermCarga.ToString() &
                                                   "|" & liHeredar.ToString() &
                                                   "|" & liHeredarSbVisitas.ToString()
               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "ConfirmacionDocumentos('" & divConfirmacionDocs.ClientID & "','" & btnConfirmarDocsSI.ClientID & "','" & btnConfirmarDocsNO.ClientID & "', 0);", True)
            Else
               If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                  'If (pObjPropiedadesProrroga.ppObjVisita.TieneSubVisitas And liHeredar = Constantes.Verdadero) Or
                  '    (pObjPropiedadesProrroga.ppObjVisita.EsSubVisita And liHeredarSbVisitas = Constantes.Verdadero) Then ''Refrezcar toda la pantalla por la herencia de docs a subvisitas

                  ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionProrroga();", True)
                  'End If
            End If
            End If
         End If
      End If
   End Sub

   Public Sub CargarEnSisvig(ByVal idVisita As Integer, ByVal idDoc As Integer, ByVal Paso As Integer, _
                             ByVal idExt As Integer, ByVal Arc As String, _
                             ByVal Ruta As String, ByVal Nombre As String)
      Dim visitaSisvig As New Entities.Sisvig() 'MCS 
      Dim Archivo() As String

      Archivo = visitaSisvig.ObtieneNombreArchivo(idVisita, idDoc, idExt, Paso, Arc)

      If Archivo(0) <> "" Then
         Dim Shp As New Utilerias.SharePointManager

         If Archivo(1) = "SICOD" Then
            ConfigurarSharePointSisvigSicod(Shp)
         Else
            ConfigurarSharePointSeprisSisvig(Shp)
         End If

         Shp.NombreArchivo = Archivo(0)
         Shp.RutaArchivo = Ruta
         Shp.NombreArchivoOri = Nombre

         If Not Shp.UploadFileToSharePoint() Then
            ''Elimina el archivo en el servidor de APP
            EliminaArchivoTemporal(Ruta & Nombre)
            MensajeDocs = "No se pudo guardar el documento en SharePoint de SISVIG."
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
         End If

         Shp = Nothing
      End If
   End Sub

   ''' <summary>
   ''' Carga un archivo desde sicod
   ''' </summary>
   ''' <param name="psNomArchivoSicod"></param>
   ''' <remarks></remarks>
   Private Sub CargarArchivoSharePointDesdeSicod(psNomArchivoSicod As String)
      Dim lrRenglonGrid = gvConsultaDocs.Rows(hfIdRenglon.Value)
      Dim lsIdVisita As String = ""

      Dim FechaRegVisita = CDate(pObjPropiedadesProrroga.ppObjVisita.FechaRegistro.ToString())
      Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

      If pObjPropiedadesProrroga.pbEstaEditandoVisita Or pObjPropiedadesProrroga.pbEstaInsertandoVisita Then
         If pObjPropiedadesProrroga.pbEstaEditandoVisita Then
            lsIdVisita = pObjPropiedadesProrroga.piIdVisitaActualDoc
         Else
            lsIdVisita = "1"
         End If
      Else
         lsIdVisita = IIf(hfVisita.Value = "-1", "1", hfVisita.Value)
      End If

      Dim btnCargar As ImageButton = lrRenglonGrid.FindControl("btnCargaDoc_" & hfIdDocSicod.Value & "_" & hfIdVerDocSicod.Value & "_" & hfNumVerDocSicod.Value & "_" & lsIdVisita)
      If IsNothing(btnCargar) Then Exit Sub

      Dim lsNomControl As String = btnCargar.ID.Replace("btn", "")

      ''REENGLON|ID_DOCUMENTO|VERSION|TIPO_DOCUMENTO[WORD o PDF]|PASO_DOCUMENTO|BANDERA_NOTIFICACION|BANDERA_TERMINA_CARGA
      Dim vecDatos() As String = btnCargar.CommandArgument.Split("|")
      If vecDatos.Length < 8 Then Exit Sub

      Dim liIdRen As Integer = vecDatos(0)
      Dim liIdDoc As Integer = vecDatos(1)
      Dim liVer As Integer = vecDatos(2)
      Dim liIdTipoDoc As Integer = vecDatos(3)
      Dim liIdPaso As Integer = vecDatos(4)
      Dim liBanNotificacion As Integer = vecDatos(5)
      Dim liBanTerminaCarga As Integer = vecDatos(6)
      Dim liIdTipoDocTermCarga As Integer = vecDatos(7)
      Dim liNumVersiones As Integer = vecDatos(8)
      Dim liHeredar As Integer = vecDatos(9)
      Dim liHeredarSbVisitas As Integer = vecDatos(10)

      If IsNothing(puObjUsuario) Then Exit Sub

      Dim gvGridRow As GridViewRow = gvConsultaDocs.Rows(liIdRen)
      Dim btnLink As LinkButton = gvGridRow.FindControl("lnk" & lsNomControl)
      'Dim btnImg As ImageButton = gvGridRow.FindControl("img" & lsNomControl)
      Dim fuFileUp As FileUpload = gvGridRow.FindControl("fu" & lsNomControl)

      Dim lsNomArchivo As String = psNomArchivoSicod.Replace("__scd", "")
      Dim lsExtArchivo As String = System.IO.Path.GetExtension(lsNomArchivo)

      Dim lsAuxNombreDoc As String = ""
      Dim lsAuxOriNombre As String = ""

      ''BUSCA ARCHIVO EN EXPEDIENTE
      Dim objDocumentoExp As Documento = (From doc In pObjPropiedadesProrroga.peObjExp.lstDocumentos Where doc.N_ID_DOCUMENTO = liIdDoc And doc.N_ID_VERSION = liVer).FirstOrDefault()

      If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
         lsAuxNombreDoc = ObtenerNombreDocumentoArchivoSepris(liIdDoc, liVer, lsExtArchivo, (liNumVersiones + 1), objDocumentoExp)
      Else
         lsAuxNombreDoc = ObtenerNombreDocumentoArchivoSepris(liIdDoc, liVer, lsExtArchivo, liNumVersiones, objDocumentoExp)
      End If

      lsAuxNombreDoc = IIf((lsAuxNombreDoc.Trim().Length > 1), lsAuxNombreDoc, lsNomArchivo)

      ''Obtiene nombre original del documento, como lo vera el usuario, ya sea con la nomeclatura o sin ella
      ''SIEMPRE SE DEBE DE GENERAR UNA NOMECLATURA YA QUE ESE ES EL MOMBRE CON EL QUE VA A QUEDAR EN SHAREPOINT
      'PORQUE SI NO EN DIFERENTES VISITAS SE PODRIA GENERAR EL MISMO ARCHIVO
      If Not IsNothing(objDocumentoExp) Then
         If objDocumentoExp.N_FLAG_APLICA_NOMENCLATURA = Constantes.Verdadero Then
            lsAuxOriNombre = lsAuxNombreDoc
         Else
            lsAuxOriNombre = psNomArchivoSicod.Replace("__scd", "")
         End If
      Else
         lsAuxOriNombre = psNomArchivoSicod.Replace("__scd", "")
      End If

      ''Valida el tipo de archivo
      If (liIdTipoDoc = Constantes.TipoArchivo.WORD And (lsExtArchivo <> ".doc" And lsExtArchivo <> ".docx")) Or
          (liIdTipoDoc = Constantes.TipoArchivo.PDF And lsExtArchivo <> ".pdf") Then
         btnCargar.Visible = True
         fuFileUp.Visible = True
         btnLink.Visible = False
         'btnImg.Visible = False

         PonerClaseVisibleOculta(btnCargar, btnCargar.CssClass)
         PonerClaseVisibleOculta(fuFileUp, fuFileUp.CssClass)
         PonerClaseVisibleOculta(btnLink, btnLink.CssClass)
         'PonerClaseVisibleOculta(btnImg, btnImg.CssClass)

         If liIdTipoDoc = Constantes.TipoArchivo.WORD Then
            MensajeDocs = "Archivo WORD no válido."
         Else
            MensajeDocs = "Archivo PDF no válido."
         End If

         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
         Exit Sub
      End If

      Dim objDocumento As New Entities.FileSepris

      If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
         objDocumento.I_ID_VISITA = pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado
         objDocumento.I_ID_PASO = liIdPaso
         objDocumento.N_ID_DOCUMENTO = liIdDoc
         objDocumento.T_NOM_DOCUMENTO = psNomArchivoSicod
         objDocumento.T_NOM_DOCUMENTO_ORI = lsAuxOriNombre
         objDocumento.N_ID_VERSION = liVer
         objDocumento.T_DSC_COMENTARIO = BuscaComentarios()
         objDocumento.N_ID_TIPO_DOCUMENTO = liIdTipoDoc
         objDocumento.T_ID_USUARIO = puObjUsuario.IdentificadorUsuario
         objDocumento.I_ID_ESTATUS = pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
         'objDocumento.N_ID_DOCUMENTO_PASO = pObjPropiedadesProrroga.ppObjVisita.IdPasoActual 'MCS
         objDocumento.N_ID_DOCUMENTO_PASO = liIdPaso
         objDocumento.NUM_VERSIONES = liNumVersiones

         If Not objDocumento.AltaDocumento() Then
            MensajeDocs = "No se pudo reemplazar el documento en base de datos."
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
            Exit Sub
         Else

            If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
               Dim bitacora As New Conexion.Bitacora("Adjunta documento desde SICOD" & "(" & psNomArchivoSicod & "), a la visita" & "(" & pObjPropiedadesProrroga.ppObjVisita.FolioVisita & ")", System.Web.HttpContext.Current.Session.SessionID, puObjUsuario.IdentificadorUsuario)
               bitacora.Finalizar(True)
            End If

            ''Refrescar el expediente en memoria y el grid de consulta
            RefrezcaGridExpediente()

            ''Filtra de nuevo
            CargarCatalogo()
         End If
      Else
         objDocumento.I_ID_PASO = liIdPaso
         objDocumento.N_ID_DOCUMENTO = liIdDoc
         objDocumento.T_NOM_DOCUMENTO = psNomArchivoSicod
         objDocumento.T_NOM_DOCUMENTO_ORI = lsAuxOriNombre
         objDocumento.N_ID_VERSION = liVer
         objDocumento.T_DSC_COMENTARIO = BuscaComentarios()
         objDocumento.N_ID_TIPO_DOCUMENTO = liIdTipoDoc
         objDocumento.T_ID_USUARIO = puObjUsuario.IdentificadorUsuario

         If Not objDocumento.AltaDocumentoUsuario() Then
            MensajeDocs = "No se pudo reemplazar el documento en base de datos."
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
            Exit Sub
         Else
            ''Refrescar el expediente en memoria y el grid de consulta
            RefrezcaGridExpediente()

            ''Filtra de nuevo
            CargarCatalogo()

            ''Deja registro en bitacora
            If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
               Dim bitacora As New Conexion.Bitacora("Carga Expediente Visita" & "(" & pObjPropiedadesProrroga.ppObjVisita.FolioVisita & ")", System.Web.HttpContext.Current.Session.SessionID, puObjUsuario.IdentificadorUsuario)
               bitacora.Finalizar(True)
            End If
         End If
      End If

      ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      btnLink.Visible = True
      btnLink.CommandArgument = psNomArchivoSicod
      btnLink.Text = lsAuxOriNombre
      btnLink.CssClass = btnLink.CssClass.Replace("OcultarControl", "")

      If liIdTipoDoc = Constantes.TipoArchivo.PDF Then
         'btnImg.Visible = pObjPropiedadesProrroga.PermisoEditarPDF
         If pObjPropiedadesProrroga.PermisoEditarPDF Then
            btnLink.CssClass = btnLink.CssClass.Replace("OcultarControl", "")
         End If
      Else
         'btnImg.Visible = True
         btnLink.CssClass = btnLink.CssClass.Replace("OcultarControl", "")
      End If

      fuFileUp.Visible = False
      btnCargar.Visible = False

      PonerClaseVisibleOculta(btnCargar, btnCargar.CssClass)
      PonerClaseVisibleOculta(fuFileUp, fuFileUp.CssClass)
      PonerClaseVisibleOculta(btnLink, btnLink.CssClass)
      'PonerClaseVisibleOculta(btnImg, btnImg.CssClass)

      ''Valida si despues de cargar el documento es nacesario pedir una confirmacion para terminar la carga de documentos
      ''valida que haya una visita en especifico
      ''Que no pida la confirmacion si esta editando una sub visita un un subfolio
      If liBanNotificacion = Constantes.Verdadero And liBanTerminaCarga = Constantes.Falso And
          pObjPropiedadesProrroga.piIdVisitaActualDoc <> Constantes.Todos And Not pObjPropiedadesProrroga.pbEstaEditandoVisita Then
         ''Dim objEtiqueta As New Entities.EtiquetaError(2139)
         MensajeDocs = Constantes.MensajesModal.FinalizaCargaDoctos

         btnConfirmarDocsSI.CommandArgument = liIdDoc.ToString() &
                                             "|" & liIdPaso.ToString() &
                                             "|" & liIdTipoDocTermCarga.ToString() &
                                             "|" & liHeredar.ToString() &
                                             "|" & liHeredarSbVisitas.ToString()

         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "ConfirmacionDocumentos('" & divConfirmacionDocs.ClientID & "','" & btnConfirmarDocsSI.ClientID & "','" & btnConfirmarDocsNO.ClientID & "', 0);", True)
      Else
         If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
            If (pObjPropiedadesProrroga.ppObjVisita.TieneSubVisitas And liHeredar = Constantes.Verdadero) Or
                (pObjPropiedadesProrroga.ppObjVisita.EsSubVisita And liHeredarSbVisitas = Constantes.Verdadero) Then ''Refrezcar toda la pantalla por la herencia de docs a subvisitas

               If (Convert.ToDateTime(FechaRegVisita).ToString("yyyy/MM/dd") < Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
                  Response.Redirect("../Procesos/DetalleVisita.aspx#tab3")
               Else
                  Response.Redirect("../Procesos/DetalleVisita_V17.aspx#tab3")
               End If

            End If
         End If
      End If
   End Sub

   ''' <summary>
   ''' Se ejecuta cuando se confirma que si se terminaron de subir los documentos
   ''' Si preciona que si, se debe de finalizar el paso en el que estaban los documentos que se acaban de subir y en ciertos casos se debe notificar a ciertas areas
   ''' que deberias ser tambien dinamicas es decir, relacionar por cada documento en caso que sea notificable a que usuarios o perfiles o areas se tiene que notidficar. :)..
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnConfirmarDocsSI_Click(sender As Object, e As EventArgs) Handles btnConfirmarDocsSI.Click

      Dim btnConfirmar As Button = CType(sender, Button)
      If IsNothing(btnConfirmar) Then Exit Sub

      Dim vecDatos() As String = btnConfirmar.CommandArgument.Split("|")
      Dim liIdDoc As Integer = vecDatos(0)
      Dim liIdPaso As Integer = vecDatos(1)
      Dim liTipoArchivo As Integer = vecDatos(2)
      Dim liHeredar As Integer = vecDatos(3)
      Dim liHeredarSbVisitas As Integer = vecDatos(4)

      Dim FechaRegVisita = CDate(pObjPropiedadesProrroga.ppObjVisita.FechaRegistro.ToString())
      Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

      ''Finalizar el paso en que estan los docs y notificar.
      If liTipoArchivo = Constantes.TipoArchivo.WORD Then
         AccesoBD.finalizarPasoSinTransaccion(pObjPropiedadesProrroga.piIdVisitaActualDoc, liIdPaso, liTipoArchivo, Constantes.Verdadero, 0, ObtenerSubVisitasSeleccionadas())
      Else
         AccesoBD.finalizarPasoSinTransaccion(pObjPropiedadesProrroga.piIdVisitaActualDoc, liIdPaso, liTipoArchivo, Constantes.Falso, Constantes.Verdadero, ObtenerSubVisitasSeleccionadas())
      End If

      ''A quien notificar?, dejemoslo estatico ya que no se definio funcionalidad para esto
      ''SI EL PASO ES 5 NOTIFICAR A SUPERVISOR VO/VF Y A VJ

      If liIdPaso = PasoProcesoVisita.Pasos.Cinco Or liIdPaso = PasoProcesoVisita.Pasos.Seis Or liIdPaso = PasoProcesoVisita.Pasos.Quince Then
         If Not IsNothing(puObjUsuario) Then
            If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
               Dim objNotif As New NotificacionesVisita(puObjUsuario, Server, BuscaComentarios())
               Dim objCorreoBD As New Entities.Correo(Constantes.CORREO_DOCUMENTOS_ADJUNTADOS)
               objCorreoBD.Cuerpo = objCorreoBD.Cuerpo.Replace("[PASO]", liIdPaso.ToString())

               If Constantes.CORREO_ENVIADO_OK = objNotif.NotificarCorreo(objCorreoBD, pObjPropiedadesProrroga.ppObjVisita, True, True, False) Then
                  AccesoBD.actualizarPasoNotificadoSinTransaccion(pObjPropiedadesProrroga.piIdVisitaActualDoc, liIdPaso, True, pObjPropiedadesProrroga.ppObjVisita.Usuario.IdArea, objNotif.getDestinatariosNombre(), objNotif.getDestinatariosCorreo(), DateTime.Now)
               End If
            End If
         End If
      End If

      If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
         If (pObjPropiedadesProrroga.ppObjVisita.TieneSubVisitas And liHeredar = Constantes.Verdadero) Or
             (pObjPropiedadesProrroga.ppObjVisita.EsSubVisita And liHeredarSbVisitas = Constantes.Verdadero) Then ''Refrezcar toda la pantalla por la herencia de docs a subvisitas

            If (Convert.ToDateTime(FechaRegVisita).ToString("yyyy/MM/dd") < Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
               Response.Redirect("../Procesos/DetalleVisita.aspx#tab3")
            Else
               Response.Redirect("../Procesos/DetalleVisita_V17.aspx#tab3")
            End If

         End If
      End If

      ''Refrescar el expediente en memoria y el grid de consulta
      RefrezcaGridExpediente()

      ''Filtra de nuevo
      CargarCatalogo()
   End Sub

   Private Sub RefrezcaGridExpediente()
      ''Genera el grid
      If Not IsNothing(pObjPropiedadesProrroga.peObjExp) Then
         ''Agregar el origen de datos al grid
         If Not IsNothing(pObjPropiedadesProrroga.DataSourceSession) Then

            pObjPropiedadesProrroga.peObjExp.RefreszarDocumentosExpediente()

            Try
               gvConsultaDocs.DataSource = pObjPropiedadesProrroga.DataSourceSession
               gvConsultaDocs.DataBind()
               ActualizaAnchoDiv()
            Catch ex As Exception
               Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "ucDocumentos.aspx.vb, RefrezcaGridExpediente", "")
            End Try
         End If
      End If
   End Sub

   Protected Sub btnAgregarDoc_Click(sender As Object, e As CommandEventArgs)
      Dim btnAgregar As ImageButton = CType(sender, ImageButton)

      If Not IsNothing(btnAgregar) Then
         CargaMasiva()

         ''REENGLON|ID_DOCUMENTO|VERSION|TIPO_DOCUMENTO[WORD o PDF]|PASO_DOCUMENTO|BANDERA_NOTIFICACION|BANDERA_TERMINA_CARGA
         Dim vecDatos() As String = btnAgregar.CommandArgument.Split("|")

         Dim liIdRen As Integer = vecDatos(0)
         Dim liIdDoc As Integer = vecDatos(1)
         Dim liVer As Integer = vecDatos(2)
         Dim liIdTipoDoc As Integer = vecDatos(3)
         Dim liIdPaso As Integer = vecDatos(4)
         Dim liBanConfirmacion As Integer = vecDatos(5)
         Dim liBanTerminaCarga As Integer = vecDatos(6)
         Dim liIdTipoDocTermCarga As Integer = vecDatos(7)
         Dim liBanSicod As Integer = vecDatos(8)

         Dim objDocumento As New Entities.FileSepris

         If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
            objDocumento.I_ID_VISITA = pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado
            objDocumento.I_ID_PASO = liIdPaso
            objDocumento.N_ID_DOCUMENTO = liIdDoc
            objDocumento.T_NOM_DOCUMENTO = ""
            objDocumento.T_NOM_DOCUMENTO_ORI = ""
            objDocumento.N_ID_VERSION = liVer
            objDocumento.T_DSC_COMENTARIO = ""
            objDocumento.N_ID_TIPO_DOCUMENTO = liIdTipoDoc
            objDocumento.T_ID_USUARIO = puObjUsuario.IdentificadorUsuario
            objDocumento.I_ID_ESTATUS = pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
            'objDocumento.N_ID_DOCUMENTO_PASO = pObjPropiedadesProrroga.ppObjVisita.IdPasoActual 'MCS
            objDocumento.N_ID_DOCUMENTO_PASO = liIdPaso

            If Not objDocumento.AltaDocumento() Then
               MensajeDocs = "No se pudo reemplazar el documento en base de datos."
               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
               Exit Sub
            Else
               ''Refrescar el expediente en memoria y el grid de consulta
               RefrezcaGridExpediente()

               ''Filtra de nuevo
               CargarCatalogo()
            End If
         Else
            objDocumento.I_ID_PASO = liIdPaso
            objDocumento.N_ID_DOCUMENTO = liIdDoc
            objDocumento.T_NOM_DOCUMENTO = ""
            objDocumento.T_NOM_DOCUMENTO_ORI = ""
            objDocumento.N_ID_VERSION = liVer
            objDocumento.T_DSC_COMENTARIO = ""
            objDocumento.N_ID_TIPO_DOCUMENTO = liIdTipoDoc
            objDocumento.T_ID_USUARIO = puObjUsuario.IdentificadorUsuario

            If Not objDocumento.AltaDocumentoUsuario() Then
               MensajeDocs = "No se pudo reemplazar el documento en base de datos."
               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
               Exit Sub
            Else
               ''Refrescar el expediente en memoria y el grid de consulta
               RefrezcaGridExpediente()

               ''Filtra de nuevo
               CargarCatalogo()
            End If
         End If
      End If

   End Sub

   ''' <summary>
   ''' Carga un archivo a partir de un file upload para la carga masiva
   ''' </summary>
   ''' <remarks></remarks>
   Private Function CargarArchivoSharePointMasiva(ByVal btnCargar As ImageButton, ByVal piTamMax As Integer,
                                                  ByRef lstErrores As List(Of String)) As Boolean
      Dim lsNomControl As String = btnCargar.ID.Replace("btn", "")
      ''REENGLON|ID_DOCUMENTO|VERSION|TIPO_DOCUMENTO[WORD o PDF]|PASO_DOCUMENTO|BANDERA_NOTIFICACION|BANDERA_TERMINA_CARGA
      Dim vecDatos() As String = btnCargar.CommandArgument.Split("|")

      Dim liIdRen As Integer = vecDatos(0)
      Dim liIdDoc As Integer = vecDatos(1)
      Dim liVer As Integer = vecDatos(2)
      Dim liIdTipoDoc As Integer = vecDatos(3)
      Dim liIdPaso As Integer = vecDatos(4)
      Dim liBanNotificacion As Integer = vecDatos(5)
      Dim liBanTerminaCarga As Integer = vecDatos(6)
      Dim liIdTipoDocTermCarga As Integer = vecDatos(7)
      Dim liNumVersiones As Integer = vecDatos(8)

      Dim gvGridRow As GridViewRow = gvConsultaDocs.Rows(liIdRen)
      Dim btnLink As LinkButton = gvGridRow.FindControl("lnk" & lsNomControl)
      'Dim btnImg As ImageButton = gvGridRow.FindControl("img" & lsNomControl)
      Dim fuFileUp As FileUpload = gvGridRow.FindControl("fu" & lsNomControl)

      If Not IsNothing(fuFileUp) Then
         If Not fuFileUp.HasFile Then
            Return True
         Else
            Dim lsExtArchivo As String = System.IO.Path.GetExtension(fuFileUp.FileName)

            ''Valida el tipo de archivo
            If (liIdTipoDoc = Constantes.TipoArchivo.WORD And (lsExtArchivo <> ".doc" And lsExtArchivo <> ".docx")) Or
                (liIdTipoDoc = Constantes.TipoArchivo.PDF And lsExtArchivo <> ".pdf") Then
               btnCargar.Visible = True
               fuFileUp.Visible = True
               btnLink.Visible = False
               'btnImg.Visible = False

               PonerClaseVisibleOculta(btnCargar, btnCargar.CssClass)
               PonerClaseVisibleOculta(fuFileUp, fuFileUp.CssClass)
               PonerClaseVisibleOculta(btnLink, btnLink.CssClass)
               'PonerClaseVisibleOculta(btnImg, btnImg.CssClass)

               If liIdTipoDoc = Constantes.TipoArchivo.WORD Then
                  lstErrores.Add("El archivo [" & fuFileUp.FileName & "] no es un documento WORD válido para el documento [" & gvConsultaDocs.DataKeys(liIdRen).Item("T_NOM_DOCUMENTO_CAT").ToString() & "]")
               Else
                  lstErrores.Add("El archivo [" & fuFileUp.FileName & "] no es un documento PDF válido para el documento [" & gvConsultaDocs.DataKeys(liIdRen).Item("T_NOM_DOCUMENTO_CAT").ToString() & "]")
               End If

               Return False
            End If

            Dim Shp As New Utilerias.SharePointManager
            Dim lbArchivoSisvig As Boolean = False

            ''VALIDAR SI ES UN ARCHIVO DE SISVIG
            If btnLink.CommandArgument.Contains("__svg") Then
               lbArchivoSisvig = True
            End If

            If lbArchivoSisvig Then
               ConfigurarSharePointSeprisSisvig(Shp)
            Else
               ConfigurarSharePointSepris(Shp)
            End If

            '---------------------------------------
            ' Guarda el archivo en Sharepoint
            '---------------------------------------
            Dim lsAuxNombreDoc As String = ""
            Dim lsAuxOriNombre As String = ""

            ''BUSCA ARCHIVO EN EXPEDIENTE
            Dim objDocumentoExp As Documento = (From doc In pObjPropiedadesProrroga.peObjExp.lstDocumentos Where doc.N_ID_DOCUMENTO = liIdDoc And doc.N_ID_VERSION = liVer).FirstOrDefault()

            If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
               lsAuxNombreDoc = ObtenerNombreDocumentoArchivoSepris(liIdDoc, liVer, lsExtArchivo, (liNumVersiones + 1), objDocumentoExp)
            Else
               lsAuxNombreDoc = ObtenerNombreDocumentoArchivoSepris(liIdDoc, liVer, lsExtArchivo, liNumVersiones, objDocumentoExp)
            End If

            lsAuxNombreDoc = IIf((lsAuxNombreDoc.Trim().Length > 1), lsAuxNombreDoc, fuFileUp.FileName)

            ''Obtiene nombre original del documento, como lo vera el usuario, ya sea con la nomeclatura o sin ella
            ''SIEMPRE SE DEBE DE GENERAR UNA NOMECLATURA YA QUE ESE ES EL MOMBRE CON EL QUE VA A QUEDAR EN SHAREPOINT
            'PORQUE SI NO EN DIFERENTES VISITAS SE PODRIA GENERAR EL MISMO ARCHIVO
            If Not IsNothing(objDocumentoExp) Then
               If objDocumentoExp.N_FLAG_APLICA_NOMENCLATURA = Constantes.Verdadero Then
                  lsAuxOriNombre = lsAuxNombreDoc
               Else
                  lsAuxOriNombre = fuFileUp.FileName
               End If
            Else
               lsAuxOriNombre = fuFileUp.FileName
            End If

            ''Obtiene nombre real del documento a como quedar en sharepoint
            Shp.NombreArchivo = Utilerias.Generales.ObtenerNombreArchivo(lsAuxNombreDoc)

            ''Guarda el archivo en el servidor de APP
            Dim lsRutaTemp As String = Path.GetTempPath()

            ''Lo elimina si existe
            EliminaArchivoTemporal(lsRutaTemp & fuFileUp.FileName)

            Try
               fuFileUp.SaveAs(lsRutaTemp & fuFileUp.FileName)
            Catch ex As Exception
               Utilerias.ControlErrores.EscribirEvento("Faltan permisos para CREAR el documento temporal que se genera en el servidor en la carpeta [" & Path.GetTempPath() & "] antes de enviarse a SharePoint" &
                                                   ex.ToString(), EventLogEntryType.Error, "ucDoctosProrroga.aspx.vb, CargarArchivoSharePoint", "")
            End Try

            If Not File.Exists(lsRutaTemp & fuFileUp.FileName) Then
               MensajeDocs = "No se pudo guardar temporalmente el documento en Servidor Web."
               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
               Exit Function
            End If

            Shp.RutaArchivo = lsRutaTemp
            Shp.NombreArchivoOri = fuFileUp.FileName

            If Not Shp.UploadFileToSharePoint() Then
               ''Elimina el archivo en el servidor de APP
               EliminaArchivoTemporal(lsRutaTemp & fuFileUp.FileName)

               lstErrores.Add("El archivo [" & fuFileUp.FileName & "] no se pudo guardar en SharePoint, para el documento [" & gvConsultaDocs.DataKeys(liIdRen).Item("T_NOM_DOCUMENTO_CAT").ToString() & "]")
               Return False
            Else

               Dim objDocumento As New Entities.FileSepris

               If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                  objDocumento.I_ID_VISITA = pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado
                  objDocumento.I_ID_PASO = liIdPaso
                  objDocumento.N_ID_DOCUMENTO = liIdDoc
                  objDocumento.T_NOM_DOCUMENTO = IIf(lbArchivoSisvig, Shp.NombreArchivo & "__svg", Shp.NombreArchivo)
                  objDocumento.T_NOM_DOCUMENTO_ORI = lsAuxOriNombre
                  objDocumento.N_ID_VERSION = liVer
                  objDocumento.T_DSC_COMENTARIO = BuscaComentarios()
                  objDocumento.N_ID_TIPO_DOCUMENTO = liIdTipoDoc
                  objDocumento.T_ID_USUARIO = puObjUsuario.IdentificadorUsuario
                  objDocumento.I_ID_ESTATUS = pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual
                  'objDocumento.N_ID_DOCUMENTO_PASO = pObjPropiedadesProrroga.ppObjVisita.IdPasoActual 'MCS
                  objDocumento.N_ID_DOCUMENTO_PASO = liIdPaso
                  objDocumento.NUM_VERSIONES = liNumVersiones

                  If Not objDocumento.AltaDocumento() Then
                     lstErrores.Add("El archivo [" & fuFileUp.FileName & "] no se pudo reemplazar en base de datos, para el documento [" & gvConsultaDocs.DataKeys(liIdRen).Item("T_NOM_DOCUMENTO_CAT").ToString() & "]")
                     Return False
                  Else
                     If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                        Dim bitacora As New Conexion.Bitacora("Adjunta documento" & "(" & lsAuxOriNombre & "), a la visita" & "(" & pObjPropiedadesProrroga.ppObjVisita.FolioVisita & ")", System.Web.HttpContext.Current.Session.SessionID, puObjUsuario.IdentificadorUsuario)
                        bitacora.Finalizar(True)
                     End If

                     If (AccesoBD.consultarEsVisitaDeSISVIG(objDocumento.I_ID_VISITA)) Then
                        CargarEnSisvig(objDocumento.I_ID_VISITA, liIdDoc, liIdPaso, liIdTipoDoc, Shp.NombreArchivo, lsRutaTemp, fuFileUp.FileName)
                     End If
                  End If
               Else
                     objDocumento.I_ID_PASO = liIdPaso
                     objDocumento.N_ID_DOCUMENTO = liIdDoc
                     objDocumento.T_NOM_DOCUMENTO = IIf(lbArchivoSisvig, Shp.NombreArchivo & "__svg", Shp.NombreArchivo)
                     objDocumento.T_NOM_DOCUMENTO_ORI = lsAuxOriNombre
                     objDocumento.N_ID_VERSION = liVer
                     objDocumento.T_DSC_COMENTARIO = BuscaComentarios()
                     objDocumento.N_ID_TIPO_DOCUMENTO = liIdTipoDoc
                     objDocumento.T_ID_USUARIO = puObjUsuario.IdentificadorUsuario

                     If Not objDocumento.AltaDocumentoUsuario() Then
                        lstErrores.Add("El archivo [" & fuFileUp.FileName & "] no se pudo reemplazar en base de datos, para el documento [" & gvConsultaDocs.DataKeys(liIdRen).Item("T_NOM_DOCUMENTO_CAT").ToString() & "]")
                        Return False
                     Else
                        ''Deja registro en bitacora
                        If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                           Dim bitacora As New Conexion.Bitacora("Carga Expediente Visita" & "(" & pObjPropiedadesProrroga.ppObjVisita.FolioVisita & ")", System.Web.HttpContext.Current.Session.SessionID, puObjUsuario.IdentificadorUsuario)
                           bitacora.Finalizar(True)
                        End If

                     If (AccesoBD.consultarEsVisitaDeSISVIG(objDocumento.I_ID_VISITA)) Then
                        CargarEnSisvig(objDocumento.I_ID_VISITA, liIdDoc, liIdPaso, liIdTipoDoc, Shp.NombreArchivo, lsRutaTemp, fuFileUp.FileName)
                     End If
                     End If
               End If
               ''Elimina el archivo en el servidor de APP
               EliminaArchivoTemporal(lsRutaTemp & fuFileUp.FileName)

            End If
            ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            btnLink.Visible = True
            btnLink.CommandArgument = IIf(lbArchivoSisvig, Shp.NombreArchivo & "__svg", Shp.NombreArchivo)
            btnLink.Text = lsAuxOriNombre

            'If liIdTipoDoc = Constantes.TipoArchivo.PDF Then
            '   btnImg.Visible = pObjPropiedadesProrroga.PermisoEditarPDF
            'Else
            '   btnImg.Visible = True
            'End If

            fuFileUp.Visible = False
            btnCargar.Visible = False

            PonerClaseVisibleOculta(btnCargar, btnCargar.CssClass)
            PonerClaseVisibleOculta(fuFileUp, fuFileUp.CssClass)
            PonerClaseVisibleOculta(btnLink, btnLink.CssClass)
            'PonerClaseVisibleOculta(btnImg, btnImg.CssClass)
         End If
      End If

      Return True
   End Function

   ''' <summary>
   ''' Carga masiva de documentos
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   'Protected Sub btnCargaMasiva_Click(sender As Object, e As ImageClickEventArgs) Handles btnCargaMasiva.Click

   '   ''Recorrer todo el grid en busca de fileuploads
   '   Dim lstImgBtn As List(Of ImageButton) = gvConsultaDocs.GetAllControlsOfType(Of ImageButton)()
   '   Dim lstFiltradaBtns = From imgBtn In lstImgBtn Where imgBtn.ID.Contains("btnCargaDoc") Select imgBtn

   '   Dim FechaRegVisita = CDate(pObjPropiedadesProrroga.ppObjVisita.FechaRegistro.ToString())

   '   If lstFiltradaBtns.Count > 0 Then
   '      ''Dim liLimiteArchivoCarga As Integer = ObtenerTamMaximoArch()
   '      Dim lstErrores As New List(Of String)

   '      Dim lstfileUp As List(Of FileUpload) = gvConsultaDocs.GetAllControlsOfType(Of FileUpload)()
   '      Dim liFiUpConArchivo As Integer = (From fuFile In lstfileUp Where fuFile.HasFile = True).Count()

   '      If liFiUpConArchivo > 0 Then
   '         For Each btnCargar As ImageButton In lstFiltradaBtns
   '            CargarArchivoSharePointMasiva(btnCargar, 1, lstErrores)
   '         Next

   '         ''Refrescar el expediente en memoria y el grid de consulta
   '         RefrezcaGridExpediente()

   '         ''Filtra de nuevo
   '         CargarCatalogo()

   '         If lstErrores.Count > 0 Then
   '            MensajeDocs = "Se encontraron los siguientes errores al adjuntar los documentos: " & lstErrores.toListHtml()
   '            imgAviso.ImageUrl = Constantes.Imagenes.Fallo
   '            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "MensajeConfirmacionProrroga();AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
   '         Else
   '            If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
   '               If pObjPropiedadesProrroga.ppObjVisita.TieneSubVisitas Or pObjPropiedadesProrroga.ppObjVisita.EsSubVisita Then

   '                  Response.Redirect("../Procesos/DetalleVisita_V17.aspx") ''Redireccionar por lo de la carga heredada a sub visitas

   '               End If
   '            End If

   '            MensajeDocs = "Carga de documentos completa."
   '            imgAviso.ImageUrl = Constantes.Imagenes.Exito
   '            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "MensajeConfirmacionProrroga();AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
   '         End If
   '      Else
   '         MensajeDocs = "No se encontraron archivos."
   '         imgAviso.ImageUrl = Constantes.Imagenes.Aviso
   '         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "MensajeConfirmacionProrroga();AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
   '      End If
   '   End If
   'End Sub

   ''' <summary>
   ''' Retorna las subvisitas seleccionadas de la visita principal, la mera mera
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Private Function ObtenerSubVisitasSeleccionadas() As String
      ''Solo si estamos cargando documentos en la visita principal
      If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
         If pObjPropiedadesProrroga.ppObjVisita.TieneSubVisitas And pObjPropiedadesProrroga.ppObjVisita.EsSubVisitaOsubFolio = False Then
            Dim objVisita As Visita = CType(Session("DETALLE_VISITA"), Visita)
            If Not IsNothing(objVisita) Then
               Return objVisita.SubVisitasSeleccionadas
            End If
         End If
      End If

      Return ""
   End Function

   ''' <summary>
   ''' Carga los documentos al ir agregando tipos desde el boton mas
   ''' </summary>
   ''' <remarks></remarks>
   Public Sub CargaMasiva()
      ''Recorrer todo el grid en busca de fileuploads
      Dim lstImgBtn As List(Of ImageButton) = gvConsultaDocs.GetAllControlsOfType(Of ImageButton)()
      Dim lstFiltradaBtns = From imgBtn In lstImgBtn Where imgBtn.ID.Contains("btnCargaDoc") Select imgBtn

      If lstFiltradaBtns.Count > 0 Then
         Dim liLimiteArchivoCarga As Integer = ObtenerTamMaximoArch()
         Dim lstErrores As New List(Of String)

         Dim lstfileUp As List(Of FileUpload) = gvConsultaDocs.GetAllControlsOfType(Of FileUpload)()
         Dim liFiUpConArchivo As Integer = (From fuFile In lstfileUp Where fuFile.HasFile = True).Count()

         If liFiUpConArchivo > 0 Then
            For Each btnCargar As ImageButton In lstFiltradaBtns
               CargarArchivoSharePointMasiva(btnCargar, liLimiteArchivoCarga, lstErrores)
            Next

            If lstErrores.Count > 0 Then
               MensajeDocs = "Se encontraron los siguientes errores al adjuntar los documentos: " & lstErrores.toListHtml()
               imgAviso.ImageUrl = Constantes.Imagenes.Fallo
               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
            End If
         End If
      End If
   End Sub

   Private Sub gvConsultaDocs_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvConsultaDocs.RowDataBound
      If IsNothing(pObjPropiedadesProrroga) Then Exit Sub
      If IsNothing(pObjPropiedadesProrroga.peObjExp) Then Exit Sub

      If e.Row.RowType = DataControlRowType.DataRow And pObjPropiedadesProrroga.peObjExp.HayDocumentos And Not IsNothing(e.Row.DataItem) Then
         Dim btnFu As FileUpload
         Dim btnLink As LinkButton
         'Dim btnImg As ImageButton
         Dim btnAdj As ImageButton
         Dim btnMas As ImageButton
         Dim lt As New Literal
         Dim lblObli As New Label

         ''RRA AGREGA IMAGENES PARA DOCUMENTOS
         Dim btnImgAceptar As ImageButton
         Dim btnImgRechazar As ImageButton
         Dim btnImgRechazar2 As ImageButton
         Dim btnImgNotificar As ImageButton
         Dim btnImgEnviar As ImageButton
         ''----------------------------------

         lt.Text = "<br />"
         lblObli.Text = " *"
         lblObli.Attributes.Add("style", "font-weight:bold; font-size:20px; color:red;")

         Dim liNumDoc As Integer = 0
         Dim liPasoIni As Integer = 0
         Dim liPasoFin As Integer = 0
         Dim liTerminaCarga As Integer = 0

         Dim liVersion As Integer = 0
         Dim liTipoDoc As Integer = 0
         Dim lsNomArch As String = ""
         Dim lbPermiteCarga As Boolean = False
         Dim liBanConfirmacion As Integer = 0
         Dim liBanSicod As Integer = 0
         Dim lbExisteVersionUno As Boolean = False
         Dim liNumVersiones As Integer = 1

         Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

         Try
            If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "N_ID_DOCUMENTO")) Then
               liNumDoc = CInt(DataBinder.Eval(e.Row.DataItem, "N_ID_DOCUMENTO"))
               liPasoIni = CInt(DataBinder.Eval(e.Row.DataItem, "I_ID_PASO_INI"))
               liPasoFin = CInt(DataBinder.Eval(e.Row.DataItem, "I_ID_PASO_FIN"))
               liTipoDoc = CInt(DataBinder.Eval(e.Row.DataItem, "N_ID_TIPO_DOCUMENTO"))
               liBanConfirmacion = CInt(DataBinder.Eval(e.Row.DataItem, "N_FLAG_CONFIRMACION"))
               liBanSicod = CInt(DataBinder.Eval(e.Row.DataItem, "N_FLAG_SICOD"))

               ''Validar si se permite la carga/reemplazo sobre el documento actual
               If IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then ''permiti la carga si no hay un visita
                  If pObjPropiedadesProrroga.PermisoEditarDocs And (pObjPropiedadesProrroga.piIdPasoDocumentos >= liPasoIni) And (pObjPropiedadesProrroga.piIdPasoDocumentos <= liPasoFin) Then
                     lbPermiteCarga = True

                     ''Si se permite la carga despues valida si es que aun no se han terminado de capturar los documentos
                     If lbPermiteCarga = Constantes.Verdadero Then
                        lbPermiteCarga = False
                     End If
                  End If
               Else
                  If pObjPropiedadesProrroga.PermisoEditarDocs And (((pObjPropiedadesProrroga.ppObjVisita.IdPasoActual >= liPasoIni) And (pObjPropiedadesProrroga.ppObjVisita.IdPasoActual <= liPasoFin)) Or
                                            pObjPropiedadesProrroga.pbEstaEditandoVisita) Then
                     lbPermiteCarga = True
                  End If
               End If

               ''SI SE TIENE PERMISOS PERO EL DOCUMENTO ES DEL PASO 6 NO HABILITARLO PARA VJ
               ''YA QUE SE HABILITARAN MAS A DELANTE SI SE HAN ENVIADO A VJ POR PARTE DE VO/VF/CGIV
               If liPasoIni = 6 And puObjUsuario.IdArea = Constantes.AREA_VJ Then
                  lbPermiteCarga = False
               End If

               If (pObjPropiedadesProrroga.ppObjVisita.FechaRegistro < fechaProcesoNvo) Then
                  ''FLUJO DEL PASO 8 Y 13
                  If Not IsNothing(pObjPropiedadesProrroga) Then
                     If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                        ''NO HAY REUNION PASO 8
                        If Not pObjPropiedadesProrroga.ppObjVisita.ExisteReunionPaso8 Then

                           If Not pObjPropiedadesProrroga.ppObjVisita.TieneSancion Then
                              ''Cuando no hay presentación, no hay sanción, al finalizar paso 7 se habilita campo para la presentación del paso 8 y 13.
                              If liPasoIni = 8 Or liPasoIni = 13 And pObjPropiedadesProrroga.ppObjVisita.IdPasoActual > 7 And pObjPropiedadesProrroga.ppObjVisita.IdPasoActual <= liPasoFin Then
                                 lbPermiteCarga = True
                              End If
                           Else
                              ''cuando no hay presentación y  si hay sanción, al finalizar paso 7 se habilita campo del paso 8 , y hasta que se finaliza  el paso 12 se habilita el campo del paso 13
                              If liPasoIni = 8 And pObjPropiedadesProrroga.ppObjVisita.IdPasoActual > 7 And pObjPropiedadesProrroga.ppObjVisita.IdPasoActual <= liPasoFin Then
                                 lbPermiteCarga = True
                              End If

                              If liPasoIni = 13 And pObjPropiedadesProrroga.ppObjVisita.IdPasoActual > 12 And pObjPropiedadesProrroga.ppObjVisita.IdPasoActual <= liPasoFin Then
                                 lbPermiteCarga = True
                              End If
                           End If
                        End If
                     End If
                  End If
               Else
                  ''FLUJO DEL PASO 8 Y 13
                  If Not IsNothing(pObjPropiedadesProrroga) Then
                     If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                        ''NO HAY REUNION PASO 8
                        If Not pObjPropiedadesProrroga.ppObjVisita.ExisteReunionPaso8 Then

                           If Not pObjPropiedadesProrroga.ppObjVisita.TieneSancion Then
                              ''Cuando no hay presentación, no hay sanción, al finalizar paso 7 se habilita campo para la presentación del paso 8 y 13.
                              'If liPasoIni = 8 Or liPasoIni = 13 And pObjPropiedadesProrroga.ppObjVisita.IdPasoActual > 7 And pObjPropiedadesProrroga.ppObjVisita.IdPasoActual <= liPasoFin Then
                              '    lbPermiteCarga = True
                              'End If
                           Else
                              ''cuando no hay presentación y  si hay sanción, al finalizar paso 7 se habilita campo del paso 8 , y hasta que se finaliza  el paso 12 se habilita el campo del paso 13
                              'If liPasoIni = 8 And pObjPropiedadesProrroga.ppObjVisita.IdPasoActual > 7 And pObjPropiedadesProrroga.ppObjVisita.IdPasoActual <= liPasoFin Then
                              '    lbPermiteCarga = True
                              'End If

                              'If liPasoIni = 13 And pObjPropiedadesProrroga.ppObjVisita.IdPasoActual > 12 And pObjPropiedadesProrroga.ppObjVisita.IdPasoActual <= liPasoFin Then
                              '    lbPermiteCarga = True
                              'End If
                           End If
                        End If
                     End If
                  End If
               End If

               ''Busca el archivo en el expediente
               Dim lstDocumento = From doc In pObjPropiedadesProrroga.peObjExp.lstDocumentos Where doc.N_ID_DOCUMENTO = liNumDoc Order By doc.N_ID_VERSION
               lbExisteVersionUno = False

               If lstDocumento.Count > 0 Then
                  ''Arma los controles
                  Dim liVersionDoc As Integer = 1
                  For Each objDocumento As Documento In lstDocumento
                     If Not IsNothing(objDocumento) Then

                        ''Crear los controles de los archivos word
                        btnFu = New FileUpload
                        btnLink = New LinkButton
                        'btnImg = New ImageButton
                        btnAdj = New ImageButton

                        ''RRA INICIALIZO
                        btnImgAceptar = New ImageButton
                        btnImgRechazar = New ImageButton
                        btnImgRechazar2 = New ImageButton
                        btnImgNotificar = New ImageButton
                        btnImgEnviar = New ImageButton
                        '' FIN INICIALIZO


                        Dim lsNombreControl As String = liNumDoc.ToString() & "_" &
                                                        objDocumento.N_ID_VERSION.ToString() & "_" &
                                                        IIf(objDocumento.NUM_VERSIONES = 0, 1, objDocumento.NUM_VERSIONES).ToString() & "_" &
                                                        pObjPropiedadesProrroga.psIdVisita

                        liTerminaCarga = objDocumento.N_FLAG_TERMINA_CARGA_DOCS

                        ''Si se permite la carga despues valida si es que aun no se han terminado de capturar los documentos
                        ''AGREGA VALIDACION PARA QUE EN EL PASO 8 Y ESTATUS SIN REUNION NO HABILITE LOS DOCUMENTOS
                        ''DESHABILITA LOS DOCUMENTOS DEL PASO 8 CUANDO NO HAY REUNION EN PASO 7 Y EL PASO ACTUAL DE LA VISITA ES MAYOR A 8

                        '(objDocumento.I_ID_PASO_INI = 8 And pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual = Constantes.EstatusPaso.SinReunionPresidencia) OrElse
                        '(objDocumento.I_ID_PASO_INI = 8 And pObjPropiedadesProrroga.ppObjVisita.IdPasoActual > 8 And Not pObjPropiedadesProrroga.ppObjVisita.ExisteReunionPaso8)
                        If Not IsNothing(pObjPropiedadesProrroga) Then
                           If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                              If objDocumento.N_FLAG_TERMINA_CARGA_DOCS = Constantes.Verdadero OrElse
                                  objDocumento.BANDERA_PASO_HABILITADO = Constantes.Falso Then

                                 If liPasoIni <> 8 And liPasoIni <> 13 Then ''NO APLICA EN PASO 8 Y 13
                                    lbPermiteCarga = False
                                 End If
                              End If
                           End If
                        End If


                        btnFu.ID = "fuCargaDoc_" & lsNombreControl
                        btnFu.Width = New Unit(70, UnitType.Percentage)
                        btnFu.ClientIDMode = UI.ClientIDMode.Static

                        btnAdj.ID = "btnCargaDoc_" & lsNombreControl
                        btnAdj.ImageUrl = "/Imagenes/adjuntarDocs2.png"
                        btnAdj.Width = 25
                        btnAdj.ClientIDMode = UI.ClientIDMode.Static

                        ''REENGLON|ID_DOCUMENTO|VERSION|TIPO_DOCUMENTO[WORD o PDF]|PASO_DOCUMENTO|BANDERA_TERMINA_CARGA
                        btnAdj.CommandArgument = e.Row.RowIndex.ToString() &
                                                    "|" & liNumDoc.ToString() &
                                                    "|" & objDocumento.N_ID_VERSION.ToString() &
                                                    "|" & liTipoDoc.ToString() &
                                                    "|" & objDocumento.I_ID_PASO_INI &
                                                    "|" & objDocumento.N_FLAG_CONFIRMACION &
                                                    "|" & objDocumento.N_FLAG_TERMINA_CARGA_DOCS &
                                                    "|" & Constantes.TipoArchivo.WORD &
                                                    "|" & objDocumento.NUM_VERSIONES &
                                                    "|" & objDocumento.N_FLAG_HEREDA &
                                                    "|" & objDocumento.N_FLAG_HEREDA_ENTRE_SBVISITA

                        ObtenSM().RegisterPostBackControl(btnAdj)
                        AddHandler btnAdj.Command, AddressOf Me.CargarArchivoSharePoint
                        btnAdj.OnClientClick = "MuestraImgCarga();"

                       btnLink.ID = "lnkCargaDoc_" & lsNombreControl
                        btnLink.ClientIDMode = UI.ClientIDMode.Static
                        ObtenSM().RegisterPostBackControl(btnLink)
                        AddHandler btnLink.Command, AddressOf Me.MostrarArchivo

                        'btnImg.ID = "imgCargaDoc_" & lsNombreControl
                        'btnImg.ImageUrl = "/Imagenes/Delete.png"
                        'btnImg.CommandArgument = e.Row.RowIndex
                        'btnImg.OnClientClick = "return HabilitaLink('" & btnImg.ID & "','" &
                        '                                                 btnLink.ID & "', '" &
                        '                                                 btnFu.ID & "', '" &
                        '                                                 btnAdj.ID & "', '" &
                        '                                                 "" &
                        '                                                 "')"
                        'btnImg.ClientIDMode = UI.ClientIDMode.Static

                        lblObli.ID = "lblObli_" & lsNombreControl

                        btnFu.Visible = lbPermiteCarga
                        btnAdj.Visible = lbPermiteCarga
                        btnLink.Visible = False
                        'btnImg.Visible = False
                        lblObli.Visible = lbPermiteCarga

                        ''Validar que controles se deben habilitar
                        ''Muestra el nombre original, el que tiene en el sharepoint lo oculta
                        If objDocumento.T_NOM_DOCUMENTO_ORI <> "" Then
                           lbExisteVersionUno = True
                           btnLink.Text = objDocumento.T_NOM_DOCUMENTO_ORI
                           ''Guarda el nombre del documento en el sharepoint
                           If objDocumento.T_NOM_DOCUMENTO <> "" Then
                              btnLink.CommandArgument = objDocumento.T_NOM_DOCUMENTO
                           End If

                           btnFu.Visible = False
                           btnAdj.Visible = False
                           btnLink.Visible = True
                           'btnImg.Visible = lbPermiteCarga
                           lblObli.Visible = False
                        Else
                           ''Ocultar controles del paso 1 si no se adjuntaron de un inicio
                           If Not IsNothing(pObjPropiedadesProrroga) And objDocumento.I_ID_PASO_INI = 1 And objDocumento.N_ID_VERSION = 1 Then
                              If Not pObjPropiedadesProrroga.pbEstaEditandoVisita And Not pObjPropiedadesProrroga.pbEstaInsertandoVisita And Not pObjPropiedadesProrroga.pbEsSubFolioSubVisita Then
                                 If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                                    If pObjPropiedadesProrroga.ppObjVisita.IdPasoActual >= 1 Then
                                       btnFu.Visible = False
                                       btnAdj.Visible = False
                                       btnLink.Visible = False
                                       'btnImg.Visible = True
                                       lblObli.Visible = False
                                       lbPermiteCarga = False
                                    End If
                                 End If
                              End If
                           End If
                        End If

                        ''antes de agregar hay que ponerlos todos visibles y ponerles una clase para ocultarlos a los que estan ocultos
                        'PonerClaseVisibleOculta(btnImg, btnImg.CssClass)
                        PonerClaseVisibleOculta(btnLink, btnLink.CssClass)
                        PonerClaseVisibleOculta(btnFu, btnFu.CssClass)
                        PonerClaseVisibleOculta(btnAdj, btnAdj.CssClass)

                        'e.Row.Cells(piColumnaDocs).Controls.Add(btnImg)
                        e.Row.Cells(piColumnaDocs).Controls.Add(btnLink)
                        e.Row.Cells(piColumnaDocs).Controls.Add(btnFu)
                        e.Row.Cells(piColumnaDocs).Controls.Add(btnAdj)

                        ''RRA VALIDACION PARA EFECTUAR CAMBIOS DOCUMENTOS
                        If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                           If objDocumento.T_NOM_DOCUMENTO <> "" And objDocumento.I_ID_PASO_INI = 6 And
                               pObjPropiedadesProrroga.ppObjVisita.IdPasoActual <= objDocumento.I_ID_PASO_FIN Then
                              Dim EstadoDoc As String = objDocumento.ValidaEstadoDocumento(pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado.ToString, objDocumento.N_ID_DOCUMENTO.ToString, objDocumento.N_ID_VERSION.ToString)
                              Dim lbBanderaBtn As Boolean = False
                              Dim ExtArchivo As String = System.IO.Path.GetExtension(objDocumento.T_NOM_DOCUMENTO)

                              ''ODT 2 SC1 AGC, OCULTAR EL BTN REEMPLZAZAR
                              'If Not btnImg.CssClass.Contains("OcultarControl") And Not lbBanderaBtn Then
                              '   btnImg.Visible = True 'MCS
                              '   PonerClaseVisibleOculta(btnImg, btnImg.CssClass)
                              'End If
                           End If
                        End If
                        '' FIN VALIDACION PARA EFECTUAR CAMBIOS DOCUMENTOS

                        If Not pObjPropiedadesProrroga.pbEstaEditandoVisita Then
                           If objDocumento.N_FLAG_OBLI = Constantes.Verdadero And lblObli.Visible And Not lbExisteVersionUno Then
                              e.Row.Cells(piColumnaDocs).Controls.Add(lblObli)
                           End If
                        End If

                        'e.Row.Cells(piColumnaDocs).Controls.Add(New LiteralControl("<br />"))
                     End If

                     If objDocumento.N_ID_VERSION > liVersionDoc Then liVersionDoc = objDocumento.N_ID_VERSION
                  Next

               End If
            End If
         Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "ucDocumentos.aspx.vb, gvConsultaDocs_RowCreated", "")
         End Try

      End If
   End Sub

   Protected Sub ActualizaEstatusDoc(sender As Object, e As CommandEventArgs)
      Dim btnImagen As ImageButton = TryCast(sender, ImageButton)
      Dim Documento As New Documento
      Dim vecDatos() As String = btnImagen.CommandArgument.Split("|")
      Dim idDocumento As String = vecDatos(1)
      Dim idVisita As String = pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado.ToString

      'ENVIAR CORREOS POR CORREO PERSONALIZADO ACEPTAR, RECHAZAR, ENVIAR, NOTIFICAR.
      Dim objNotif As New NotificacionesVisita(puObjUsuario, Server, BuscaComentarios())

      ''BUSCA ARCHIVO EN EXPEDIENTE
      Dim objDocumentoExp As Documento = (From doc In pObjPropiedadesProrroga.peObjExp.lstDocumentos Where doc.N_ID_DOCUMENTO = idDocumento And doc.N_ID_VERSION = vecDatos(2)).FirstOrDefault()
      If Not IsNothing(objDocumentoExp) Then
         pObjPropiedadesProrroga.ppObjVisita.DocumentoRevisionPasoSeis = objDocumentoExp.T_NOM_DOCUMENTO_CAT
      End If

      Try
         Select Case vecDatos(0)
            Case "aceptar"

               'AQUÍ SE AGREGA FUNCIONALIDAD PARA QUE SE PIDAN COMENTARIOS DE VJ SOBRE LA REVISIÓN Ý SE INCLUYAN EN EL CORREO DE NOTIFICACIÓN
               Me.btnComentAprueba.CommandArgument = btnImagen.CommandArgument
               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarComentariosAprobacionDocs('" & Me.divObservacionesAprobacion.ClientID & "', '" & Me.btnComentAprueba.ClientID & "');", True)

               'pObjPropiedadesProrroga.ppObjVisita.ComentariosAprobacionDocumentos = ""
               'objNotif.NotificarCorreo(Constantes.CORREO_DOCUMENTOS_APROBADOS, pObjPropiedadesProrroga.ppObjVisita, True, True, False)
               'Documento.ActualizaEstatusDocumento(idVisita, idDocumento, Constantes.EstatusPaso.Aprobado, vecDatos(2))

            Case "rechazar"
               'objNotif.NotificarCorreo(Constantes.CORREO_DOCUMENTOS_RECHAZO, pObjPropiedadesProrroga.ppObjVisita, True, True, False)
               'Documento.ActualizaEstatusDocumento(idVisita, idDocumento, Constantes.EstatusPaso.Rechazado, vecDatos(2))
               ''PEDIR OBSERVACIONES DE RECHAZO
               Me.btnComentRechazo.CommandArgument = btnImagen.CommandArgument
               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarComentariosRechazoDocs('" & Me.divObservacionesRechazo.ClientID & "', '" & Me.btnComentRechazo.ClientID & "');", True)
               Exit Sub
            Case "enviar"
               objNotif.NotificarCorreo(Constantes.CORREO_DOCUMENTOS_REVISION, pObjPropiedadesProrroga.ppObjVisita, True, True, False)
               Documento.ActualizaEstatusDocumento(idVisita, idDocumento, Constantes.EstatusPaso.Inicia_revision, vecDatos(2))
            Case "notificar"
               objNotif.NotificarCorreo(Constantes.CORREO_DOCUMENTOS_NOTIFICAR_CORRECTO, pObjPropiedadesProrroga.ppObjVisita, True, True, False)
               Documento.ActualizaEstatusDocumento(idVisita, idDocumento, Constantes.EstatusPaso.Revisado, vecDatos(2))
         End Select

      Catch ex As Exception
      End Try
      CargaDocumentos()

      ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "Scroll", "OcultaImagenCarga();", True)
   End Sub

   Private Sub EliminaArchivoTemporal(lsRutaTemp As String)
      If File.Exists(lsRutaTemp) Then
         Try
            File.Delete(lsRutaTemp)
         Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento("Faltan permisos para borrar el documento temporal que se genera en el servidor en la carpeta [" & Path.GetTempPath() & "] antes de enviarse a SharePoint" &
                                                    ex.ToString(), EventLogEntryType.Error, "ucDocumentos.aspx.vb, CargarArchivoSharePoint", "")
         End Try
      End If
   End Sub

   Protected Sub btnComentRechazo_Click(sender As Object, e As EventArgs)
      Dim btnComentRechazoL As Button = TryCast(sender, Button)

      ''Validar los comentarios
      lblFechaGeneralDocs.Visible = False
      If txtComentRechazo.Text.Trim().Length <= 0 Then
         Me.btnComentRechazo.CommandArgument = btnComentRechazoL.CommandArgument
         lblFechaGeneralDocs.Text = "* Es obligatorio ingresar los comentarios."
         lblFechaGeneralDocs.Visible = True
         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarComentariosRechazoDocs('" & Me.divObservacionesRechazo.ClientID & "', '" & Me.btnComentRechazo.ClientID & "');", True)
         Exit Sub
      End If

      Dim vecDatos() As String = btnComentRechazoL.CommandArgument.Split("|")
      Dim idDocumento As String = vecDatos(1)
      Dim idVisita As String = pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado.ToString

      ''BUSCA ARCHIVO EN EXPEDIENTE
      Dim objDocumentoExp As Documento = (From doc In pObjPropiedadesProrroga.peObjExp.lstDocumentos Where doc.N_ID_DOCUMENTO = idDocumento And doc.N_ID_VERSION = vecDatos(2)).FirstOrDefault()
      If Not IsNothing(objDocumentoExp) Then
         pObjPropiedadesProrroga.ppObjVisita.DocumentoRevisionPasoSeis = objDocumentoExp.T_NOM_DOCUMENTO_CAT
      End If

      'ENVIAR CORREOS POR CORREO PERSONALIZADO ACEPTAR, RECHAZAR, ENVIAR, NOTIFICAR.
      Dim objNegVisita As New NegocioVisita(pObjPropiedadesProrroga.ppObjVisita, puObjUsuario, Server, txtComentRechazo.Text)

      Try
         Select Case vecDatos(0)
            Case "rechazar"
               objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, pObjPropiedadesProrroga.ppObjVisita.IdEstatusActual, , , , , , True, Constantes.CORREO_DOCUMENTOS_RECHAZO, True, True, False)
               objDocumentoExp.ActualizaEstatusDocumentoComentarios(idVisita, idDocumento, Constantes.EstatusPaso.Rechazado, vecDatos(2), txtComentRechazo.Text)
         End Select

      Catch ex As Exception
      End Try
      CargaDocumentos()
      txtComentRechazo.Text = ""
   End Sub

   Protected Sub btnComentAprueba_Click(sender As Object, e As EventArgs)
      Dim btnComentApruebaL As Button = TryCast(sender, Button)

      ''Validar los comentarios
      lblFechaGeneralDocs.Visible = False
      If txtComentAprueba.Text.Trim().Length <= 0 Then
         Me.btnComentAprueba.CommandArgument = btnComentApruebaL.CommandArgument
         lblFechaGeneralDocsA.Text = "* Es obligatorio ingresar los comentarios."
         lblFechaGeneralDocsA.Visible = True
         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarComentariosAprobacionDocs('" & Me.divObservacionesAprobacion.ClientID & "', '" & Me.btnComentAprueba.ClientID & "');", True)
         Exit Sub
      End If

      Dim vecDatos() As String = btnComentApruebaL.CommandArgument.Split("|")
      Dim idDocumento As String = vecDatos(1)
      Dim idVisita As String = pObjPropiedadesProrroga.ppObjVisita.IdVisitaGenerado.ToString

      ''BUSCA ARCHIVO EN EXPEDIENTE
      Dim objDocumentoExp As Documento = (From doc In pObjPropiedadesProrroga.peObjExp.lstDocumentos Where doc.N_ID_DOCUMENTO = idDocumento And doc.N_ID_VERSION = vecDatos(2)).FirstOrDefault()
      If Not IsNothing(objDocumentoExp) Then
         pObjPropiedadesProrroga.ppObjVisita.DocumentoRevisionPasoSeis = objDocumentoExp.T_NOM_DOCUMENTO_CAT
      End If

      'ENVIAR CORREOS POR CORREO PERSONALIZADO ACEPTAR, RECHAZAR, ENVIAR, NOTIFICAR.
      Dim objNotif As New NotificacionesVisita(puObjUsuario, Server, txtComentAprueba.Text)

      pObjPropiedadesProrroga.ppObjVisita.ComentariosAprobacionDocumentos = txtComentAprueba.Text

      Try
         Select Case vecDatos(0)
            Case "aceptar"

               objNotif.NotificarCorreo(Constantes.CORREO_DOCUMENTOS_APROBADOS, pObjPropiedadesProrroga.ppObjVisita, True, True, False)
               objDocumentoExp.ActualizaEstatusDoctoAprueba(idVisita, idDocumento, Constantes.EstatusPaso.Aprobado, vecDatos(2), txtComentAprueba.Text)

         End Select

      Catch ex As Exception
      End Try
      CargaDocumentos()
      txtComentAprueba.Text = ""
   End Sub

   ''' <summary>
   ''' Aunq vj no tiene permisos en el paso 6 se le deben de mostrar los botones para la revicion de documentos
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Private Function ValidaVjPaso6() As Boolean
      If pObjPropiedadesProrroga.ppObjVisita.IdPasoActual = 6 And puObjUsuario.IdArea = Constantes.AREA_VJ Then
         Return True
      Else
         Return False
      End If
   End Function

End Class