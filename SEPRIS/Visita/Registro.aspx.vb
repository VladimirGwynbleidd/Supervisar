Imports System.Web.Configuration
Imports System.Net
Imports Clases
Imports Entities
Imports Utilerias
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Web.Services.Protocols

Public Class Vista
    Inherits System.Web.UI.Page
    Dim enc As New YourCompany.Utils.Encryption.Encryption64

#Region "Propiedades"
    Public Property lstMensajes As List(Of String)
    Public Property Mensaje As String
    Public Property Mensaje2 As String
    Public Property visita As Visita

    'RRA VisitaConjunta
    Private InsElimVisCon As Boolean = False
    Private QueryInsVC As String = ""
    ''' <summary>
    ''' Visita que se esta editando
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property pgIdVisitaEditar As Integer
        Get
            If IsNothing(ViewState("pgIdVisitaEditar")) Then
                Return 0
            Else
                Return CInt(ViewState("pgIdVisitaEditar"))
            End If
        End Get
        Set(value As Integer)
            Dim liAux As Integer = 0
            If Int32.TryParse(value, liAux) Then
                ViewState("pgIdVisitaEditar") = liAux
            Else
                ViewState("pgIdVisitaEditar") = 0
            End If
        End Set
    End Property


    Private Property pLstTipoEntidad As List(Of TipoSubEntidad)
        Get
            Return ViewState("pLstTipoEntidad")
        End Get
        Set(value As List(Of TipoSubEntidad))
            ViewState("pLstTipoEntidad") = value
        End Set
    End Property


    ''' <summary>
    ''' Consecutivo de la visita que se esta editando
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property pgIdConsecutivo As String
        Get
            If IsNothing(ViewState("pgIdConsecutivo")) Then
                Return ""
            Else
                Return ViewState("pgIdConsecutivo").ToString()
            End If
        End Get
        Set(value As String)
            ViewState("pgIdConsecutivo") = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que guarda el area del usuario logueado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>agc</remarks>
    Public Property pgAreaUsuario As String
        Get
            If IsNothing(ViewState("pgAreaUsuario")) Then
                Return ""
            Else
                Return ViewState("pgAreaUsuario").ToString()
            End If
        End Get
        Set(value As String)
            ViewState("pgAreaUsuario") = value
        End Set
    End Property

    Public Property pgEstaEditandoSubvisita As Boolean
        Get
            If IsNothing(ViewState("pgEstaEditandoSubvisita")) Then
                Return False
            Else
                Dim lbAux As Boolean = False
                If Boolean.TryParse(ViewState("pgEstaEditandoSubvisita"), lbAux) Then
                    Return lbAux
                Else
                    Return False
                End If
            End If
        End Get
        Set(value As Boolean)
            ViewState("pgEstaEditandoSubvisita") = value
        End Set
    End Property


    ''' <summary>
    ''' Guarda una relacion de las entidades y sus tipos
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListEntidadesTipo As List(Of Entities.EntidadTipo)
        Get
            If IsNothing(ViewState("ListEntidadesTipo")) Then
                Return New List(Of Entities.EntidadTipo)
            Else
                Dim lstET = TryCast(ViewState("ListEntidadesTipo"), List(Of Entities.EntidadTipo))

                If Not IsNothing(lstET) Then
                    Return lstET
                Else
                    Return New List(Of Entities.EntidadTipo)
                End If
            End If
        End Get
        Set(value As List(Of Entities.EntidadTipo))
            ViewState("ListEntidadesTipo") = value
        End Set
    End Property

    ''' <summary>
    ''' Guarda la visita que se esta consultando
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ppObjVisita As Visita
        Get
            If IsNothing(Session("REGISTRO_VISITA")) Then
                Return Nothing
            Else
                Dim objVisita As Visita = CType(Session("REGISTRO_VISITA"), Visita)
                If Not IsNothing(objVisita) Then
                    Return objVisita
                Else
                    Return Nothing
                End If
            End If
        End Get
    End Property

    Public Property puObjUsuario As Usuario
        Get
            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                Return CType(Session(Entities.Usuario.SessionID), Usuario)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Usuario)
            Session(Entities.Usuario.SessionID) = value
        End Set
    End Property

    ''' <summary>
    ''' Devuelve el tipo de entidad almacenado
    ''' </summary>
    ''' <param name="idEntidad"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenerTipoEntidad(idEntidad As Integer) As Integer

        For Each entidad As Entities.EntidadTipo In ListEntidadesTipo
            If entidad.IdEntidad = idEntidad Then
                Return entidad.IdTipoEntidad
            End If
        Next

        Return -1
    End Function

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'txbFechaRegistro.Attributes.Add("readonly", "readonly")
        'txbFechaInicio.Attributes.Add("readonly", "readonly")

        If Not IsPostBack Then

            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                ''Guarda el area del usuario en el viewstate
                Dim usuario As New Entities.Usuario()
                usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

                calendarioFechaRegistro.SelectedDate = DateTime.Now
                ''Se eliminan los tipos de entidades
                cargadllTiposEntidades()
                cargarEntidadesV2()
                cargarUsuariosDisponibles()
                cargarTiposVisita()
                validarPerfil(usuario.IdArea)
                cargarObjetoVisita(usuario)
                'Dim trForm As HtmlTableRow = Master.FindControl("MainContent").FindControl("tbDatosCaptura").FindControl("trSubentidad")
                'trForm.Attributes.Add("class", "OcultaFila")

                'Carga visitas conjuntas excepto PRESIDENCIA y area en sesion
                cargaListaVConjunta()

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

                Dim objAreas As New Entities.Areas(usuario.IdArea)
                objAreas.CargarDatos()

                pgAreaUsuario = objAreas.Descripcion

                ''Se ocultan las subvisitas para todo excepto financiero 
                If usuario.IdArea <> Constantes.AREA_VF Then
                    trSubvisitas.Visible = False
                End If

                ''Ocultar siempre la fecha vulneravilidades
                trAcuerdoVul.Visible = False

                ''inicializar la lista de tipos de entidades
                pLstTipoEntidad = New List(Of TipoSubEntidad)

                ''Valida si es que se esta editando una visita
                If Not IsNothing(Request.QueryString("up")) Then
                    ccfCopiarFolios.Visible = False
                    lnkTituloPag.InnerText = "Editar Visita"

                    ''Valida si se esta editando
                    If Request.QueryString("up").ToString() = "1" Then
                        If Not IsNothing(Session("idVisitaEditar")) Then
                            pgIdVisitaEditar = Session("idVisitaEditar")
                            If Not IsNothing(Request.QueryString("sb")) Then
                                pgEstaEditandoSubvisita = True
                                hfVisitaPadre.Value = Request.QueryString("sb").ToString()
                            Else
                                pgEstaEditandoSubvisita = False
                            End If

                            Session("IdVisitaGenerado") = Session("idVisitaEditar")
                            CargaFormularioParaEditar(pgIdVisitaEditar, usuario.IdArea)

                            ''Validar el area, si es VO no se muestran las subvisitas pero si es VF 
                            ''hay que mostrarlas e impedir que se seleccione mas de una sub visita ya que esta editando
                            chkSubEntidad.Attributes.Add("onclick", "javascript:SeleccionSimple(this);")
                        End If
                    End If
                Else
                    ''Ocultar la opcion para copiar folios si no es de operaciones
                    If usuario.IdArea <> Constantes.AREA_VO Then
                        ccfCopiarFolios.Visible = False
                    Else
                        ccfCopiarFolios.pagina = Page
                        ccfCopiarFolios.Visible = RecuperaParametroValidaCheckCopiarFolios()
                    End If

                    ''Quitar el postback a las subvisitas
                    chkSubEntidad.AutoPostBack = False

                End If
            End If
        Else
            Page.MaintainScrollPositionOnPostBack = True
        End If

        'If txtObjetoVisita.Text.Length > 0 Then lblConttxtObjetoVisita.Text = (txtObjetoVisita.MaxLength - txtObjetoVisita.Text.Length).ToString()

        lblFolioVisita.Text = ConfigurationManager.AppSettings("msgFolioPaginaDetalle").ToString() & "Sin folio"
        lblPasoVisita.Text = ConfigurationManager.AppSettings("msgPasoPaginaDetalle").ToString() & "0" & " - " & Conexion.SQLServer.ObtenerDscPaso(0)

        ''Habilita cajas dependiente
        HabilitarCajas()

    End Sub

    Private Sub cargadllTiposEntidades()

        dplEntidad.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
        dplEntidad.Enabled = False

        'ddlSubentidad.Items.Insert(0, New ListItem("-Seleccione una opción-", "-1"))
        'ddlSubentidad.Enabled = False

        Dim dsEntidades As WR_SICOD.Diccionario()

        Dim Usuario As String = WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        Try
            Dim mycredentialCache As CredentialCache = New CredentialCache()
            Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
            Dim proxySICOD As New WR_SICOD.ws_SICOD
            proxySICOD.Credentials = credentials
            dsEntidades = proxySICOD.GetTipoEntidades

         ddlTipoEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))
         Dim list = From p In dsEntidades
                       Where p.Key = 1 _
                       Or p.Key = 12 _
                       Or p.Key = 7
                       Select p

            If dsEntidades IsNot Nothing Then
                ddlTipoEntidad.DataSource = list
                ddlTipoEntidad.DataTextField = "value"
                ddlTipoEntidad.DataValueField = "key"
                ddlTipoEntidad.DataBind()
            ddlTipoEntidad.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
         End If

        Catch ex As Exception
            catch_cone(ex, "cargadllTiposEntidades()")
        End Try
    End Sub

    Public Sub cargarEntidadesV2()

        Dim dsEntidades As New DataSet

        Dim Usuario As String = WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        Dim vecTipoEntidades As New List(Of Integer)

        Dim usuarioActual As New Entities.Usuario()
        usuarioActual = Session(Entities.Usuario.SessionID) ' se obtiene de la session

        vecTipoEntidades.Add(1)
        vecTipoEntidades.Add(7)
        vecTipoEntidades.Add(12)

        Try
            If Not IsNothing(usuarioActual) Then
                Dim mycredentialCache As CredentialCache = New CredentialCache()
                Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
                Dim proxySICOD As New WR_SICOD.ws_SICOD
                proxySICOD.Credentials = credentials
                Dim lstEntidadesSicod As New List(Of Entities.EntidadSicod)

                For Each ent As Integer In vecTipoEntidades
                    dsEntidades = proxySICOD.GetEntidadesComplete(ent)
                    For Each ltTable As DataTable In dsEntidades.Tables
                        For Each lrRow As DataRow In ltTable.Rows
                            Dim objEntidadSicod As New Entities.EntidadSicod
                            If Int32.TryParse(lrRow("cve_id_ent"), objEntidadSicod.ID) Then
                                ''Guardar el tipo de entidad de cada entidad
                                ''objEntidadSicod.ID = CInt(objEntidadSicod.ID.ToString() & ent.ToString())
                                objEntidadSicod.ID = CInt(objEntidadSicod.ID.ToString())
                                objEntidadSicod.DSC = lrRow("siglas_ent").ToString().Trim()

                                ''Diferenciar por el descripcion
                                Dim cont As Integer = (From e In lstEntidadesSicod Where e.ID = objEntidadSicod.ID Or e.DSC = objEntidadSicod.DSC).Count()

                                If cont < 1 Then
                                    ''No se agrega procesar para financiero
                                    If usuarioActual.IdArea = Constantes.AREA_VF And objEntidadSicod.ID = 1 And objEntidadSicod.DSC = "PROCESAR" Then
                                        Continue For
                                    Else
                                        lstEntidadesSicod.Add(objEntidadSicod)
                                    End If
                                End If
                            End If
                        Next
                    Next
                Next

                Dim lstEntidadesOrdenada = From l In lstEntidadesSicod Distinct Order By l.DSC

                dplEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))
                dplEntidad.Enabled = False
                If dsEntidades IsNot Nothing Then
                    If dsEntidades.Tables(0).Rows.Count > 0 Then
                        dplEntidad.DataSource = lstEntidadesOrdenada
                        dplEntidad.DataTextField = "DSC"
                        dplEntidad.DataValueField = "ID"
                        dplEntidad.DataBind()
                        dplEntidad.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
                  dplEntidad.Enabled = False
               End If
                End If
            End If
        Catch ex As Exception
            catch_cone(ex, "cargaddlEntidades()")
        End Try
    End Sub

    Public Sub cargarTiposVisita()
        Dim lstTiposVisita As New List(Of TiposVisita)
        lstTiposVisita = AccesoBD.getTiposVisita()
        If Not IsNothing(lstTiposVisita) And lstTiposVisita.Count > 0 Then
            dplTipoVisita.DataSource = lstTiposVisita
            dplTipoVisita.DataTextField = "DescripcionTipoVisita"
            dplTipoVisita.DataValueField = "IdTipoVisita"
            dplTipoVisita.DataBind()
            dplTipoVisita.Items.FindByValue("-1").Selected = True
        End If
    End Sub

    Public Sub cargarObjetoVisita(objUsuario As Entities.Usuario)

        SelObjetos.cargarObjetoVisita(objUsuario)

        'Dim dt As DataTable = Nothing
        'Dim objAccesoBD As New AccesoBD

        'dt = objAccesoBD.ObtenObjetoVisita(objUsuario.IdArea)

        'If dt.Rows.Count > 0 Then
        '    chkObjetoVisita.DataValueField = "ID"
        '    chkObjetoVisita.DataTextField = "DSC"
        '    chkObjetoVisita.DataSource = dt
        '    chkObjetoVisita.DataBind()

        'End If

    End Sub

    Public Sub cargarUsuariosDisponibles()
        Try

            Dim usuario As New Entities.Usuario()
            usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

            If Not IsNothing(usuario) And usuario.IdArea > 0 Then
                lsbUsuariosDisponibles.DataSource = AccesoBD.ObtenerUsuarioInvolucradosVisitaDS(usuario.IdArea)
            End If

            lsbUsuariosDisponibles.DataTextField = "NOMBRE_COMPLETO"
            lsbUsuariosDisponibles.DataValueField = "USUARIO"
            lsbUsuariosDisponibles.DataBind()

            'NHM INICIA LOG           
            Utilerias.ControlErrores.EscribirEvento("Termina la carga de usuarios disponibles de forma correcta. Núm usuarios: " + lsbUsuariosDisponibles.Items.Count.ToString(), EventLogEntryType.Information)
            'NHM FIN LOG

        Catch ex As Exception
            'NHM INICIA LOG           
            Utilerias.ControlErrores.EscribirEvento("Ocurrió un error al cargar los usuarios disponibles. EXCEPTION: " + ex.ToString(), EventLogEntryType.Error)
            'NHM FIN LOG
        End Try

    End Sub

    Private Sub agregarUsuarioSupervisor()
        Try
            ''AGC se quita validacion para agregar mas de un usuario
            ''If lsbSupervisor.Items.Count = 0 Then
            If lsbUsuariosDisponibles.SelectedIndex > -1 Then
                Dim item As System.Web.UI.WebControls.ListItem = lsbUsuariosDisponibles.SelectedItem
                If Not lsbSupervisor.Items.Contains(item) Then
                    lsbSupervisor.Items.Add(item)
                End If
                item.Selected = False
                lsbUsuariosDisponibles.Items.Remove(item)
            Else
                'modalMensaje("Debe seleccionar un Usuario")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub agregarUsuarioInspector()
        Try
            If lsbUsuariosDisponibles.SelectedIndex > -1 Then
                Dim item As System.Web.UI.WebControls.ListItem = lsbUsuariosDisponibles.SelectedItem
                If Not lsbInspectores.Items.Contains(item) Then
                    lsbInspectores.Items.Add(item)
                End If
                item.Selected = False
                lsbUsuariosDisponibles.Items.Remove(item)
            Else
                'modalMensaje("Debe seleccionar un Usuario")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub quitarSupervisor()
        Try
            If lsbSupervisor.SelectedIndex > -1 Then
                Dim item As System.Web.UI.WebControls.ListItem = lsbSupervisor.SelectedItem

                lsbUsuariosDisponibles.Items.Insert(0, item)
                Dim lstAnterior As New ListBox
                lstAnterior.Items.AddRange((From ltItem As ListItem In lsbUsuariosDisponibles.Items Order By ltItem.Value).ToArray())

                lsbUsuariosDisponibles.Items.Clear()
                lsbUsuariosDisponibles.Items.AddRange((From ltItem As ListItem In lstAnterior.Items Select ltItem).ToArray())

                item.Selected = False
                lsbSupervisor.Items.Remove(item)
            Else
                'modalMensaje("Debe seleccionar una rúbrica")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub quitarInspector()
        Try
            If lsbInspectores.SelectedIndex > -1 Then
                Dim item As System.Web.UI.WebControls.ListItem = lsbInspectores.SelectedItem

                lsbUsuariosDisponibles.Items.Insert(0, item)
                Dim lstAnterior As New ListBox
                lstAnterior.Items.AddRange((From ltItem As ListItem In lsbUsuariosDisponibles.Items Order By ltItem.Value).ToArray())

                lsbUsuariosDisponibles.Items.Clear()
                lsbUsuariosDisponibles.Items.AddRange((From ltItem As ListItem In lstAnterior.Items Select ltItem).ToArray())

                item.Selected = False
                lsbInspectores.Items.Remove(item)
            Else
                'modalMensaje("Debe seleccionar una rúbrica")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub imgAsignarSupervisor_Click(sender As Object, e As ImageClickEventArgs) Handles imgAsignarSupervisor.Click
        agregarUsuarioSupervisor()
    End Sub

    Protected Sub imgDesasignarSupervisor_Click(sender As Object, e As ImageClickEventArgs) Handles imgDesasignarSupervisor.Click
        quitarSupervisor()
    End Sub

    Protected Sub imgAsignarInspector_Click(sender As Object, e As ImageClickEventArgs) Handles imgAsignarInspector.Click
        agregarUsuarioInspector()
    End Sub

    Protected Sub imgDesasignarInspector_Click(sender As Object, e As ImageClickEventArgs) Handles imgDesasignarInspector.Click
        quitarInspector()
    End Sub

    Protected Sub imgRegistrarVisita_Click(sender As Object, e As ImageClickEventArgs) Handles imgRegistrarVisita.Click
        ''Valida si se esta editando o insertando
        If Not IsNothing(Request.QueryString("up")) Then

            If Request.QueryString("up").ToString() = "1" Then
                If datosCapturadosValidos() Then

                    'Las siguientes dos líneas se pasan al paso 1
                    ''Valida si hay documentos obligatorios sin cargar
                    'If cuDocumentos.HayDoctosObligatoriosSinCargarPorVisita() Then Exit Sub

                    GuardaEditarVisita()
                    Exit Sub
                ElseIf Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                    Mensaje = "<ul>"
                    For Each msg In lstMensajes
                        Mensaje += "<li>" + msg + "</li>"
                    Next
                    Mensaje += "</ul>"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If
            End If
        End If

        If datosCapturadosValidos(Constantes.Verdadero) Then
            Dim con As Conexion.SQLServer = Nothing
            Dim tran As SqlClient.SqlTransaction = Nothing
            Dim guardo As Boolean = True
            Dim lsVisitasGeneradas As String = "<ul>"
            Dim lsVisitaPadre As String = ""
            Dim idVisitaPadre As Integer = 0
            Dim lsFoliosVisitasHijas As String = ""

            Try
                con = New Conexion.SQLServer()
                tran = con.BeginTransaction()

                Dim i As Integer
                Dim liAuxSE As Integer
                Dim objSubEntidad As Entities.SubEntidad

                ''Guardar la visita padre
                guardo = False

                ''Registrar la visita inicial
                Dim objUsuario As New Entities.Usuario()
                objUsuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

                'MCS
                If registrarVisitaPadre(con, tran) = True Then
                    If registrarObjetoVisitaReg(con, tran) Then
                        If registrarSupervisores(con, tran) = True And registrarInspectores(con, tran) = True Then

                            registrarPasoCero(con, tran)

                            If registrarPasoUno(con, tran) = True Then
                                If registrarEstatusPaso(con, tran) = True Then
                                    guardo = True
                                    lsVisitaPadre = Session("IdVisitaGeneradoCadena").ToString()
                                    lsVisitasGeneradas &= "<li>" & Session("IdVisitaGeneradoCadena").ToString() & "</li>"
                                    idVisitaPadre = visita.IdVisitaGenerado

                                    'INICIO Agrega funcionalidad de insertar y eliminar registros de visitas conjuntas
                                    Dim area As New Areas 'Para ocupar metodo "InsertaElimina_VisitasConjuntas"
                                    If InsElimVisCon = True Then
                                        'area.InsertaElimina_VisitasConjuntas("1", QueryInsVC)
                                        area.InsertaElimina_VisitasConjuntas(Session("IdVisitaGenerado"), QueryInsVC)
                                    Else
                                        area.InsertaElimina_VisitasConjuntas(Session("IdVisitaGenerado"), "-")
                                    End If
                                    'FIN Agrega funcionalidad de insertar y eliminar registros de visitas conjuntas

                                    If Not IsNothing(objUsuario) Then
                                        Dim bitacora As New Conexion.Bitacora("Registro visita(" & lsVisitaPadre & ")", System.Web.HttpContext.Current.Session.SessionID, objUsuario.IdentificadorUsuario)
                                        bitacora.Finalizar(True)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If

                'MCS
                ''Registrar las subvisitas
                If chkSubVisitas.Checked Then
                    If chkSubEntidad.Items.Count > 0 Then
                        For i = 0 To chkSubEntidad.Items.Count - 1
                            If (chkSubEntidad.Items(i).Selected) And guardo = True Then

                                If Int32.TryParse(chkSubEntidad.Items(i).Value, liAuxSE) Then
                                    objSubEntidad = New Entities.SubEntidad
                                    objSubEntidad.IdSubEntidad = liAuxSE
                                    objSubEntidad.DscSubEntidad = chkSubEntidad.Items(i).Text
                                Else
                                    Continue For
                                End If

                                If registrarVisitasHijas(objSubEntidad, con, tran, idVisitaPadre, i, lsVisitaPadre) = True Then
                                    If registrarObjetoVisitaReg(con, tran) Then
                                        If registrarSupervisores(con, tran) = True And registrarInspectores(con, tran) = True Then

                                            registrarPasoCero(con, tran)

                                            If registrarPasoUno(con, tran) = True Then
                                                If registrarEstatusPaso(con, tran) = True Then
                                                    guardo = True
                                                    lsVisitasGeneradas &= "<li>" & Session("IdVisitaGeneradoCadena").ToString() & "</li>"
                                                    lsFoliosVisitasHijas &= Session("IdVisitaGeneradoCadena").ToString() & ","
                                                    'INICIO Agrega funcionalidad de insertar y eliminar registros de visitas conjuntas
                                                    Dim area As New Areas 'Para ocupar metodo "InsertaElimina_VisitasConjuntas"
                                                    If InsElimVisCon = True Then
                                                        'area.InsertaElimina_VisitasConjuntas("1", QueryInsVC)
                                                        area.InsertaElimina_VisitasConjuntas(Session("IdVisitaGenerado"), QueryInsVC)
                                                    Else
                                                        area.InsertaElimina_VisitasConjuntas(Session("IdVisitaGenerado"), "-")
                                                    End If
                                                    'FIN Agrega funcionalidad de insertar y eliminar registros de visitas conjuntas

                                                    If Not IsNothing(objUsuario) Then
                                                        Dim bitacora As New Conexion.Bitacora("Registro Subvisita(" & Session("IdVisitaGeneradoCadena").ToString() & ") de la visita(" & lsVisitaPadre & ")", System.Web.HttpContext.Current.Session.SessionID, objUsuario.IdentificadorUsuario)
                                                        bitacora.Finalizar(True)
                                                    End If
                                                Else
                                                    guardo = False
                                                End If
                                            Else
                                                guardo = False
                                            End If
                                        Else
                                            guardo = False
                                        End If
                                    Else
                                        guardo = False
                                    End If
                                Else
                                    guardo = False
                                End If
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                'Registro fallido
                guardo = False
                Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Registro.aspx.vb, imgRegistrarVisita_Click", "")
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgError
                Dim errores As New Entities.EtiquetaError(2122)
                Mensaje = errores.Descripcion
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Finally
                'MCS
                If Not IsNothing(tran) Then
                    If guardo Then

                        'Registro exitoso
                        tran.Commit()
                        ''Limpiar sesiones
                        Session("IdOficioComision") = Nothing
                        Session("IdActaInicio") = Nothing
                        Session("IdVisitaGenerado") = Nothing

                        ''Actualiza el folio de la visita en el objeto visita
                        visita.FolioVisita = lsVisitaPadre
                        visita.IdVisitaGenerado = idVisitaPadre
                        visita.Fecha_LimitePasoActual = AccesoBD.ObtenFechaLimiteAtender_Paso(visita.IdVisitaGenerado, PasoProcesoVisita.Pasos.Uno)
                        If lsFoliosVisitasHijas.Trim.Length > 0 Then visita.FoliosSubVisitasSeleccionadas = lsFoliosVisitasHijas.Substring(0, lsFoliosVisitasHijas.Length - 1)
                        ''Eliminar Documentos temporales y pasar a tabla operativa

                        'Notifica al supervisor asignado a la visita
                        Dim usuario As New Entities.Usuario()
                        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

                        If Not IsNothing(usuario) Then
                            Dim objNotif As New NotificacionesVisita(usuario, Server, txbComentarios.Text)
                            objNotif.NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_REGISTRO, visita, True, False, False, Nothing, True)
                        End If


                    Else
                        'Registro fallido
                        tran.Rollback()
                        imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgError
                        Dim errores As New Entities.EtiquetaError(2122)
                        Mensaje = errores.Descripcion
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
                    End If
                    tran.Dispose()
                End If

                If Not IsNothing(con) Then
                    con.CerrarConexion()
                    con = Nothing
                End If
                If guardo Then
                    Try
                        Dim objUsuario As New Entities.Usuario()
                        objUsuario = Session(Entities.Usuario.SessionID)

                        Dim visita As New Visita()

                        If IsNothing(ppObjVisita) Then
                            visita = AccesoBD.getDetalleVisita(hfIdVisitaGen.Value, objUsuario.IdArea)
                        Else
                            visita = ppObjVisita
                        End If

                        Dim conex = New Conexion.SQLServer()
                        Dim query As String = "UPDATE BDS_D_VS_VISITA SET T_ESTATUS_VULNERABILIDAD = 0, T_VULNERA_REALIZADA = 0, F_FECH_ACUERDO_VULNERA = NULL WHERE I_ID_VISITA = " + visita.IdVisitaGenerado.ToString
                        conex.Ejecutar(query)
                    Catch ex As Exception
                        Console.WriteLine(ex)
                    End Try



                    hdVisitasGen.Value = lsVisitasGeneradas.Replace("<", "--").Replace("</", "|").Replace(">", "..")
                    'Analizar el paso de los siguientes 3 renglones al ultimo botón que se ejecuta en una de las ventanas de alerta
                    imgConfirmaRegistroVisita.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                    'Mensaje = lsVisitaPadre
                    Mensaje = "Se ha registrado satisfactoriamente la visita: <br /><br /> " & hdVisitasGen.Value.Replace("--", "<").Replace("|", "</").Replace("..", ">") & "</ul>"
                    'Mensaje2 = "¿Ya se ha llevado a cabo la  revisión de vulnerabilidades?"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmaRegistroVisita();", True)
                    'Mensaje = "Se ha registrado satisfactoriamente la visita: <br /><br />" & hdVisitasGen.Value.Replace("--", "<").Replace("|", "</").Replace("..", ">") & "</ul>"
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmaRegistroVisita();", True)

                    'hdVisitasGen.Value = lsVisitasGeneradas.Replace("<", "--").Replace("</", "|").Replace(">", "..")
                    ''Analizar el paso de los siguientes 3 renglones al ultimo botón que se ejecuta en una de las ventanas de alerta
                    'imgConfirmaRegistroVisita.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                    'Mensaje2 = "¿Ya se ha llevado a cabo la  revisión de vulnerabilidades?"
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionFechaVulnerabilidad();", True)

                End If
            End Try

        ElseIf Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        'End If
    End Sub

    Public Function registrarVisitaPadre(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
        Dim regExitoso As Boolean = False
        visita = New Visita()
        Try
            visita.FolioVisita = txbFolioVisita.Text.Trim()
            visita.Usuario = New Entities.Usuario()
            visita.Usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session
            visita.IdArea = visita.Usuario.IdArea ' 35 - VICEPRESIDENCIA DE OPERACIONES, 36 - VICEPRESIDENCIA FINANCIERA, ODT dice Área que ejecuta: VO/VF, se debe obtener del perfil del usuario
            visita.FechaRegistro = Convert.ToDateTime(txbFechaRegistro.Text.Trim() & " " & DateTime.Now.Hour & ":" & DateTime.Now.Minute & ":" & DateTime.Now.Second)
            If txbFechaInicio.Text.Trim() <> "" Then
                visita.FechaInicioVisita = Convert.ToDateTime(txbFechaInicio.Text.Trim())
            End If

            visita.IdTipoEntidad = ddlTipoEntidad.SelectedValue
            visita.DscTipoEntidad = ddlTipoEntidad.SelectedItem.Text
            visita.IdEntidad = dplEntidad.SelectedValue
            visita.IdTipoVisita = Convert.ToInt32(dplTipoVisita.SelectedValue)
            visita.IdEstatusActual = 1 ' 1 - Registrado - BDS_C_GR_ESTATUS_CAT
            visita.IdPasoActual = 1 ' 1 - Elaborar Oficios y Acta Inicial usando requisitos de VJ y enviar a VJ - BDS_C_GR_PASOS
            visita.NombreEntidad = dplEntidad.SelectedItem.Text
            visita.EsCancelada = False
            visita.MotivoCancelacion = String.Empty
            visita.FechaCancela = Nothing
            visita.IdSubentidad = 0 ''En la visita padre se deja vacia la sub entidad
            visita.DscSubentidad = "" ''En la visita padre se deja vacia la sub entidad
            visita.DescripcionVisita = txtDescripcionVisita.Text

            visita.DscObjetoVisitaOtro = SelObjetos.txtOtro

            Dim psIdVisita As String = ""
            visita.ComentariosIniciales = txbComentarios.Text

            If txtAcuerdoVul.Text = "" Then
                visita.Fecha_AcuerdoVul = Nothing
            Else
                visita.Fecha_AcuerdoVul = Fechas.toDate(txtAcuerdoVul.Text.Trim())
            End If

            visita.OrdenVisita = txtOrdenVisita.Text.Trim()

            visita.IdVisitaGenerado = AccesoBD.registrarVisita(visita, con, tran, psIdVisita)
            Session.Add("IdVisitaGenerado", visita.IdVisitaGenerado)
            hfIdVisitaGen.Value = visita.IdVisitaGenerado
            Session.Add("IdVisitaGeneradoCadena", psIdVisita)
        Catch ex As Exception
            regExitoso = False
        End Try
        If visita.IdVisitaGenerado > 0 Then
            regExitoso = True
        Else
            regExitoso = False
        End If
        Return regExitoso
    End Function

    Public Function registrarSupervisores(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
        Dim regExitoso As Boolean = True
        Try
            If lsbSupervisor.Items.Count > 0 Then
                visita.LstSupervisoresAsignados = New List(Of SupervisorAsignado)

                For indice As Integer = 0 To lsbSupervisor.Items.Count - 1
                    Dim supervisor As New SupervisorAsignado()
                    supervisor.Id = lsbSupervisor.Items(indice).Value
                    supervisor.Nombre = lsbSupervisor.Items(indice).Text
                    supervisor.Correo = ObtenerCorreoUsuario(supervisor.Id)

                    visita.LstSupervisoresAsignados.Add(supervisor)
                Next

                For Each supervisor In visita.LstSupervisoresAsignados
                    If AccesoBD.registrarSupervisor(visita.IdVisitaGenerado, supervisor, con, tran) = False Then
                        regExitoso = False
                    End If
                Next

            End If

        Catch ex As Exception
            regExitoso = False
        End Try
        Return regExitoso
    End Function

    Public Function registrarInspectores(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
        Dim regExitoso As Boolean = True
        Try
            If lsbInspectores.Items.Count > 0 Then
                visita.LstInspectoresAsignados = New List(Of InspectorAsignado)

                For indice As Integer = 0 To lsbInspectores.Items.Count - 1
                    Dim inspector As New InspectorAsignado()
                    inspector.Id = lsbInspectores.Items(indice).Value
                    inspector.Nombre = lsbInspectores.Items(indice).Text
                    inspector.Correo = ObtenerCorreoUsuario(inspector.Id)

                    visita.LstInspectoresAsignados.Add(inspector)
                Next

                For Each inspector In visita.LstInspectoresAsignados
                    If AccesoBD.registrarInspector(visita.IdVisitaGenerado, inspector, con, tran) = False Then
                        regExitoso = False
                    End If
                Next

            End If
        Catch ex As Exception
            regExitoso = False
        End Try
        Return regExitoso
    End Function

    Public Function registrarPasoCero(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
        Dim regExitoso As Boolean = True
        Try
            Dim Paso As New PasoProcesoVisita()

            Paso.IdVisitaGenerado = visita.IdVisitaGenerado
            Paso.IdPaso = 0 ' 0 - Reunión con Vicepresidencia y VJ para vulnerabilidades
            Paso.FechaInicio = visita.FechaRegistro
            Paso.FechaFin = DateTime.Now
            Paso.EsNotificado = False
            Paso.IdAreaNotificada = Constantes.AREA_SN
            Paso.IdUsuarioNotificado = Nothing
            Paso.EmailUsuarioNotificado = Nothing
            Paso.TieneProrroga = False
            Paso.FechaNotifica = Nothing
            'Paso.IdMovimiento = Constantes.MovimientoPaso.RegistroVisita

            If AccesoBD.registrarPaso_V17(Paso, con, tran) = False Then
                regExitoso = False
            End If

        Catch ex As Exception
            regExitoso = False
        End Try

        Return regExitoso
    End Function

    Public Function registrarPasoUno(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
        Dim regExitoso As Boolean = True
        Try
            Dim Paso As New PasoProcesoVisita()

            Paso.IdVisitaGenerado = visita.IdVisitaGenerado
            Paso.IdPaso = visita.IdPasoActual ' 1 - Elaborar Oficios y Acta Inicial usando requisitos de VJ y enviar a VJ - BDS_C_GR_PASOS
            Paso.FechaInicio = visita.FechaRegistro
            Paso.FechaFin = Nothing
            Paso.EsNotificado = True
            Paso.IdAreaNotificada = Constantes.AREA_SN
            Paso.IdUsuarioNotificado = Nothing
            Paso.EmailUsuarioNotificado = Nothing
            Paso.TieneProrroga = False
            Paso.FechaNotifica = Nothing

            If AccesoBD.registrarPaso(Paso, con, tran) = False Then
                regExitoso = False
            End If

        Catch ex As Exception
            regExitoso = False
        End Try

        Return regExitoso
    End Function

    Public Function registrarEstatusPaso(ByVal con As Conexion.SQLServer, ByVal tran As SqlClient.SqlTransaction) As Boolean
        Dim regExitoso As Boolean = True
        Try

            Dim estatusPaso As New EstatusPaso()
            Dim PasoPrV As New PasoProcesoVisita()
            estatusPaso.IdVisitaGenerado = visita.IdVisitaGenerado
            estatusPaso.IdPaso = visita.IdPasoActual ' 1 - Elaborar Oficios y Acta Inicial usando requisitos de VJ y enviar a VJ - BDS_C_GR_PASOS
            estatusPaso.IdEstatus = visita.IdEstatusActual ' 1 - Registrado - BDS_C_GR_ESTATUS_CAT
            estatusPaso.FechaRegistro = visita.FechaRegistro
            estatusPaso.IdUsuario = visita.Usuario.IdentificadorUsuario
            estatusPaso.Comentarios = txbComentarios.Text.Trim()
            estatusPaso.EsRegistro = Constantes.Verdadero
            estatusPaso.TipoComentario = "USUARIO"
            'PasoPrV.IdMovimiento = 

            If AccesoBD.registrarEstatusPaso(PasoPrV, estatusPaso, con, tran) = False Then
                regExitoso = False
            End If

        Catch ex As Exception
            regExitoso = False
        End Try

        Return regExitoso
    End Function

    Public Function datosCapturadosValidos(Optional piBanEstaInsertando As Integer = Constantes.Falso) As Boolean
        Dim sinError As Boolean = True
        lstMensajes = New List(Of String)

        'RRA VALIDACION VISITAS CONJUTAS
        Dim indices As New List(Of Integer)

        If ChkVConjunta.Checked = True Then
            indices = CheckListValida(chkListVC)
            If indices.Count = 0 Then
                chkListVC.Visible = False
                ChkVConjunta.Checked = False
            Else
                InsElimVisCon = True
                QueryInsVC = generaIdsIn(indices)
            End If
        End If

        'FIN VALIDACIONES VISITAS CONJUTAS

        If txbFolioVisita.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(2112)
            lstMensajes.Add(errores.Descripcion)
            sinError = False
            ast1.Attributes.Remove("class")
            ast1.Attributes.Add("class", "AsteriscoShow")
        Else
            ast1.Attributes.Remove("class")
            ast1.Attributes.Add("class", "AsteriscoHide")
        End If



        If txbFechaRegistro.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(2113)
            lstMensajes.Add(errores.Descripcion)
            sinError = False
            ast2.Attributes.Remove("class")
            ast2.Attributes.Add("class", "AsteriscoShow")
        Else
            ast2.Attributes.Remove("class")
            ast2.Attributes.Add("class", "AsteriscoHide")
        End If

        Dim fechaHoy As Date = DateTime.Now.Date
        ''NO VALIDA FECHA DE REGISTRO SI ESTA EDITANDO DESDE DETALLE
        If IsNothing(Request.QueryString("r")) Then
            If txbFechaRegistro.Text.Trim() <> String.Empty Then
                Dim fechaRegistro As Date
                If Date.TryParse(txbFechaRegistro.Text.Trim(), fechaRegistro) Then
                    Dim res1 As Integer = Date.Compare(fechaRegistro, fechaHoy)
                    '0  - es la misma fecha
                    '-1 - es mas pequeña la fecha de registro que la fecha de hoy
                    '1  - es mas grande la fecha de registro que la fecha de hoy
                    If res1 <> 0 Then
                        Dim errores As New Entities.EtiquetaError(2114)
                        lstMensajes.Add(errores.Descripcion)
                        sinError = False
                        ast2.Attributes.Remove("class")
                        ast2.Attributes.Add("class", "AsteriscoShow")
                    Else
                        ast2.Attributes.Remove("class")
                        ast2.Attributes.Add("class", "AsteriscoHide")
                    End If
                Else
                    Dim errores As New Entities.EtiquetaError(2169)
                    lstMensajes.Add(errores.Descripcion)
                    sinError = False
                    ast2.Attributes.Remove("class")
                    ast2.Attributes.Add("class", "AsteriscoShow")
                End If
            End If
        End If

        ''QUITA VALIDACION DE LA FECHA DE VISITA AGC
        'If txbFechaInicio.Text.Trim() = String.Empty Then
        '    Dim errores As New Entities.EtiquetaError(2115)
        '    lstMensajes.Add(errores.Descripcion)
        '    sinError = False
        '    ast3.Attributes.Remove("class")
        '    ast3.Attributes.Add("class", "AsteriscoShow")
        'End If
        If txbFechaInicio.Text.Trim() <> String.Empty Then
            Dim fechaInicio As Date
            If Date.TryParse(txbFechaInicio.Text.Trim(), fechaInicio) Then
                Dim res2 As Integer = Date.Compare(fechaInicio, fechaHoy)
                If res2 < 0 Then
                    Dim errores As New Entities.EtiquetaError(2128)
                    lstMensajes.Add(errores.Descripcion)
                    sinError = False
                    ast3.Attributes.Remove("class")
                    ast3.Attributes.Add("class", "AsteriscoShow")
                Else
                    ast3.Attributes.Remove("class")
                    ast3.Attributes.Add("class", "AsteriscoHide")
                End If
            Else
                txbFechaInicio.Text = ""
            End If
        End If

        If ddlTipoEntidad.SelectedValue = -1 Then
            Dim errores As New Entities.EtiquetaError(2127)
            lstMensajes.Add(errores.Descripcion)
            sinError = False
            ast9.Attributes.Remove("class")
            ast9.Attributes.Add("class", "AsteriscoShow")
        Else
            ast9.Attributes.Remove("class")
            ast9.Attributes.Add("class", "AsteriscoHide")
        End If

        ''NO VALIDA FECHA DE REGISTRO SI ESTA EDITANDO DESDE DETALLE
        If IsNothing(Request.QueryString("r")) Then
            If dplEntidad.SelectedValue = "-1" Or dplEntidad.SelectedValue = "" Then
                Dim errores As New Entities.EtiquetaError(2117)
                lstMensajes.Add(errores.Descripcion)
                sinError = False
                ast4.Attributes.Remove("class")
                ast4.Attributes.Add("class", "AsteriscoShow")
            Else
                ast4.Attributes.Remove("class")
                ast4.Attributes.Add("class", "AsteriscoHide")
            End If
        End If

        'If ddlSubentidad.SelectedValue = -1 And ddlTipoEntidad.SelectedValue = "2" Then

        'If chkSubEntidad.SelectedValue = "" Then
        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

        ''Solo para financiero
        If Not IsNothing(usuario) Then
            If usuario.IdArea = Constantes.AREA_VF And chkSubVisitas.Checked Then
                If chkSubEntidad.Items.Count > 0 Then
                    If chkSubEntidad.SelectedValue = "" Then
                        Dim errores As New Entities.EtiquetaError(2134)
                        lstMensajes.Add(errores.Descripcion)
                        sinError = False
                        ast10.Attributes.Remove("class")
                        ast10.Attributes.Add("class", "AsteriscoShow")
                    Else
                        ast10.Attributes.Remove("class")
                        ast10.Attributes.Add("class", "AsteriscoHide")
                    End If
                End If
            End If
        End If

        If dplTipoVisita.SelectedValue = -1 Then
            Dim errores As New Entities.EtiquetaError(2118)
            lstMensajes.Add(errores.Descripcion)
            sinError = False
            ast5.Attributes.Remove("class")
            ast5.Attributes.Add("class", "AsteriscoShow")
        Else
            ast5.Attributes.Remove("class")
            ast5.Attributes.Add("class", "AsteriscoHide")
        End If

        ''agc objeto visita
        Dim objSelect As List(Of ListItem) = SelObjetos.GetObjetosSeleccionados()
        If objSelect.Count() <= 0 Then
            Dim errores As New Entities.EtiquetaError(2136)
            lstMensajes.Add(errores.Descripcion)
            sinError = False
            ast12.Attributes.Remove("class")
            ast12.Attributes.Add("class", "AsteriscoShow")
        Else
            ast12.Attributes.Remove("class")
            ast12.Attributes.Add("class", "AsteriscoHide")

            ''Valida si se seleccino otro en el objeto de la visita
            'For Each liChkList As ListItem In objSelect
            '    If txtObjetoVisita.Text.Trim() = "" Then
            '        Dim errores As New Entities.EtiquetaError(2137)
            '        lstMensajes.Add(errores.Descripcion)
            '        sinError = False
            '        ast13.Attributes.Remove("class")
            '        ast13.Attributes.Add("class", "AsteriscoShow")
            '    Else
            '        ast13.Attributes.Remove("class")
            '        ast13.Attributes.Add("class", "AsteriscoHide")
            '    End If
            'Next
        End If

        ''Valida fecha
        ''Solo para financiero
        If Not IsNothing(usuario) Then
            If usuario.IdArea = Constantes.AREA_VF Then
                If txtAcuerdoVul.Text.Trim() <> String.Empty Then
                    Dim fechaAcuerdoVul As Date
                    If Not Date.TryParse(txtAcuerdoVul.Text.Trim(), fechaAcuerdoVul) Then
                        Dim errores As New Entities.EtiquetaError(2170)
                        lstMensajes.Add(errores.Descripcion)
                        sinError = False
                        divAcuerdoVul.Attributes.Remove("class")
                        divAcuerdoVul.Attributes.Add("class", "AsteriscoShow")
                    Else
                        divAcuerdoVul.Attributes.Remove("class")
                        divAcuerdoVul.Attributes.Add("class", "AsteriscoHide")
                    End If
                End If
            End If
        End If

        If lsbSupervisor.Items.Count = 0 Then
            Dim errores As New Entities.EtiquetaError(2119)
            lstMensajes.Add(errores.Descripcion)
            sinError = False
            ast6.Attributes.Remove("class")
            ast6.Attributes.Add("class", "AsteriscoShow")
        Else
            ast6.Attributes.Remove("class")
            ast6.Attributes.Add("class", "AsteriscoHide")
        End If

        If lsbInspectores.Items.Count = 0 Then
            Dim errores As New Entities.EtiquetaError(2120)
            lstMensajes.Add(errores.Descripcion)
            sinError = False
            ast7.Attributes.Remove("class")
            ast7.Attributes.Add("class", "AsteriscoShow")
        Else
            ast7.Attributes.Remove("class")
            ast7.Attributes.Add("class", "AsteriscoHide")
        End If

        If txtDescripcionVisita.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(2204)
            lstMensajes.Add(errores.Descripcion)
            sinError = False
            ast11.Attributes.Remove("class")
            ast11.Attributes.Add("class", "AsteriscoShow")
        Else
            ast11.Attributes.Remove("class")
            ast11.Attributes.Add("class", "AsteriscoHide")
        End If

        Return sinError
    End Function

    Public Sub validarPerfil(piIdArea As Integer)

        activarBtnRegistrarVisita()

        ''Validar entre areas para impedir que la VO pueda generar subvisitas
        If piIdArea <> Constantes.AREA_VF Then
            chkSubEntidad.Attributes.Add("onclick", "javascript:SeleccionSimple(this);")
        End If
    End Sub

    Public Sub activarBtnRegistrarVisita()

        Dim perfiles_act_BtnRegistrarVisita As New List(Of Integer)
        perfiles_act_BtnRegistrarVisita.Add(Constantes.PERFIL_ADM)
        perfiles_act_BtnRegistrarVisita.Add(Constantes.PERFIL_SUP)
        perfiles_act_BtnRegistrarVisita.Add(Constantes.PERFIL_INS)

        Dim areas_act_BtnRegistrarVisita As New List(Of Integer)
        areas_act_BtnRegistrarVisita.Add(Constantes.AREA_VO)
        areas_act_BtnRegistrarVisita.Add(Constantes.AREA_VF)
        areas_act_BtnRegistrarVisita.Add(Constantes.AREA_PR)
        areas_act_BtnRegistrarVisita.Add(Constantes.AREA_CGIV)
        areas_act_BtnRegistrarVisita.Add(Constantes.AREA_PLD)

        Dim pOK As Boolean
        pOK = perfilValido(perfiles_act_BtnRegistrarVisita)

        Dim aOK As Boolean
        aOK = areaValida(areas_act_BtnRegistrarVisita)

    End Sub

    Public Function perfilValido(ByVal lstPerfilesRequeridos As List(Of Integer)) As Boolean
        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

        For Each id As Integer In lstPerfilesRequeridos
            If id = usuario.IdentificadorPerfilActual Then
                Return True
            End If
        Next

        Return False
    End Function
    Public Function areaValida(ByVal lstAreasRequeridas As List(Of Integer)) As Boolean
        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

        For Each id As Integer In lstAreasRequeridas
            If id = usuario.IdArea Then
                Return True
            End If
        Next

        Return False
    End Function

    Protected Sub imgInicio_Click(sender As Object, e As ImageClickEventArgs) Handles imgInicio.Click

        Response.Redirect("../Visita/Bandeja.aspx")

    End Sub

    Private Sub catch_cone(ByVal e As Exception, ByVal s As String)
        EventLogWriter.EscribeEntrada("Funcion " & s & ": " & e.ToString(), EventLogEntryType.Error)
    End Sub

    Protected Sub BtnConfirmaRegistro_Click(sender As Object, e As EventArgs) Handles BtnConfirmaRegistro.Click
        If Not IsNothing(Request.QueryString("up")) Then
            ''Regresar a detalle
            If Not IsNothing(Request.QueryString("r")) Then
                If pgIdVisitaEditar <> 0 Then
                    If Not pgEstaEditandoSubvisita Then
                        Session("ID_VISITA") = pgIdVisitaEditar
                        Response.Redirect("../Procesos/DetalleVisita_V17.aspx#tab1")
                    Else
                        Session("ID_VISITA") = hfVisitaPadre.Value
                        Response.Redirect("../Procesos/DetalleVisita_V17.aspx#tab1")
                    End If
                End If
            End If

            ''Regresar a bandeja
            If Not IsNothing(Request.QueryString("sb")) Then
                If hfVisitaPadre.Value <> -1 Then
                    Response.Redirect("../Visita/Bandeja.aspx?sb=" & hfVisitaPadre.Value)
                End If
            End If
        End If

        Response.Redirect("../Visita/Bandeja.aspx")
    End Sub

    Protected Sub dplEntidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dplEntidad.SelectedIndexChanged
        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

        If Not IsNothing(usuario) Then
            If usuario.IdArea = Constantes.AREA_VF Then
                Dim dtSubentidades As New DataTable
                Dim objVisita As New Visita
                Dim con1 As New OracleConexion()
                Dim descripcion As String = Nothing

                Dim lsIdEntidad As String = ""
                Dim lsQuery As String = ""

                If dplEntidad.SelectedValue <> "" Then
                    lsIdEntidad = dplEntidad.SelectedValue.Substring(1)
                End If

                'Se comenta el if el 051120019 y ya no se muestra esta Siefore que ya no esta vigente
                'If lsIdEntidad = "52" Then ''TRAER PARA BANAMEX LA ENTIDAD 652 AV1
                'lsQuery = "SELECT S.ID_SUBENT,S.SGL_SUBENT DSC_SUBENT,S.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD S JOIN osiris.BDV_C_T_ENTIDAD TE ON(S.ID_T_ENT = TE.ID_T_ENT) where S.CVE_ID_ENT in (3" & lsIdEntidad & ",4" & lsIdEntidad & ",5" & lsIdEntidad & ",6" & lsIdEntidad & ") and S.ID_T_ENT in (1,2,3,4,17) ANd S.VIG_FLAG= CASE WHEN S.ID_SUBENT = 1 THEN S.VIG_FLAG ELSE 1 END ORDER BY S.SGL_SUBENT "
                'Else
                lsQuery = "SELECT F.ID_SUBENT,F.SGL_SUBENT DSC_SUBENT,F.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD_F F JOIN osiris.BDV_C_T_ENTIDAD TE ON(F.ID_T_ENT = TE.ID_T_ENT) where F.CVE_ID_ENT in (3" & lsIdEntidad & ",4" & lsIdEntidad & ",5" & lsIdEntidad & ",6" & lsIdEntidad & ") and F.ID_T_ENT in (2,3,4,17) ANd F.VIG_FLAG=1"
                'lsQuery = "SELECT S.ID_SUBENT,S.SGL_SUBENT DSC_SUBENT,S.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD S JOIN osiris.BDV_C_T_ENTIDAD TE ON(S.ID_T_ENT = TE.ID_T_ENT) where S.CVE_ID_ENT in (3" & lsIdEntidad & ",4" & lsIdEntidad & ",5" & lsIdEntidad & ",6" & lsIdEntidad & ") and S.ID_T_ENT in (2,3,4,17) ANd S.VIG_FLAG=1 UNION SELECT F.ID_SUBENT,F.SGL_SUBENT DSC_SUBENT,F.ID_T_ENT,TE.DESC_T_ENT FROM osiris.BDV_C_SUBENTIDAD_F F JOIN osiris.BDV_C_T_ENTIDAD TE ON(F.ID_T_ENT = TE.ID_T_ENT) where F.CVE_ID_ENT in (3" & lsIdEntidad & ",4" & lsIdEntidad & ",5" & lsIdEntidad & ",6" & lsIdEntidad & ") and F.ID_T_ENT in (2,3,4,17) ANd F.VIG_FLAG=1"
                'End If

                Dim dt As DataSet = con1.Datos(lsQuery)

                If dt IsNot Nothing Then
                    If dt.Tables(0).Rows.Count > 0 Then
                        chkSubEntidad.DataSource = dt
                        chkSubEntidad.DataTextField = "DSC_SUBENT"
                        chkSubEntidad.DataValueField = "ID_SUBENT"
                        chkSubEntidad.DataBind()

                        Dim objTipoEntidad As TipoSubEntidad
                        pLstTipoEntidad.Clear()

                        Dim i As Integer = 0
                        For Each dtTable As DataTable In dt.Tables
                            For Each drTable As DataRow In dtTable.Rows
                                objTipoEntidad = New TipoSubEntidad With {.IdItem = i, .IdSubEntidad = drTable("ID_SUBENT"), .IdTipoEntidad = drTable("ID_T_ENT"), .DscTipoEntidad = drTable("DESC_T_ENT")}
                                pLstTipoEntidad.Add(objTipoEntidad)
                                i = i + 1
                            Next
                        Next
                    Else
                        chkSubEntidad.Items.Clear()
                    End If
                Else
                    chkSubEntidad.Items.Clear()
                End If
            End If
        End If


        GeneraFolioVisita()
    End Sub

    ''' <summary>
    ''' Habilita alguna caja de texto dependiente de un combo, posterior a ello hay que habilitarlo entre postbacks
    ''' </summary>
    ''' <param name="ddlPadre"></param>
    ''' <param name="txtDependiente"></param>
    ''' <param name="piOpHabilita"></param>
    ''' <param name="pbOpcion"></param>
    ''' <remarks></remarks>
    Private Sub HabilitarCajaDependiente(ddlPadre As DropDownList, txtDependiente As TextBox, piOpHabilita As Integer, pbOpcion As Boolean)
        Dim cadena As String = String.Empty
        cadena = "var combo = document.getElementById('" & ddlPadre.ClientID & "');"
        cadena &= "var valor = combo.options[combo.selectedIndex].value;"

        cadena &= "if(valor == " & piOpHabilita & "){ "

        cadena &= "document.getElementById('" & txtDependiente.ClientID & "').disabled=false;"

        cadena &= "}else{"

        cadena &= "document.getElementById('" & txtDependiente.ClientID & "').disabled=true;"

        cadena &= "}"

        ddlPadre.Attributes.Add("onChange", cadena)
    End Sub

    ''' <summary>
    ''' Muestra un renglon completo de la fila
    ''' </summary>
    ''' <param name="ddlPadre"></param>
    ''' <param name="prRenglonHtml"></param>
    ''' <param name="piOpHabilita"></param>
    ''' <param name="pbOpcion"></param>
    ''' <remarks></remarks>
    Private Sub HabilitarCajaDependiente(ddlPadre As DropDownList, prRenglonHtml As HtmlTableRow, piOpHabilita As Integer, pbOpcion As Boolean, Optional prRenglonHtml2 As HtmlTableRow = Nothing)
        Dim cadena As String = String.Empty
        cadena = "var combo = document.getElementById('" & ddlPadre.ClientID & "');"
        cadena &= "var valor = combo.options[combo.selectedIndex].value;"

        cadena &= "if(valor == " & piOpHabilita & "){ "

        cadena &= "$('#" & prRenglonHtml.ClientID & "').removeClass('Oculto');"
        If Not IsNothing(prRenglonHtml2) Then
            cadena &= "$('#" & prRenglonHtml2.ClientID & "').removeClass('Oculto');"
        End If

        cadena &= "}else{"

        cadena &= "$('#" & prRenglonHtml.ClientID & "').addClass('Oculto');"
        If Not IsNothing(prRenglonHtml2) Then
            cadena &= "$('#" & prRenglonHtml2.ClientID & "').addClass('Oculto');"
        End If

        cadena &= "}"

        ddlPadre.Attributes.Add("onChange", cadena)
    End Sub

    ''' <summary>
    ''' Habilita las cajas entre postbacks
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub HabilitarCajas()
        'OtroObjVisita.Attributes.Remove("class")
        'OtroObjVisitaDsc.Attributes.Remove("class")
        'OtroObjVisita.Visible = True
        'OtroObjVisitaDsc.Visible = True
        'For Each liChkList As ListItem In chkObjetoVisita.Items
        '    If liChkList.Value = "1" Then
        '        If liChkList.Selected Then
        '            OtroObjVisita.Attributes.Remove("class")
        '            OtroObjVisitaDsc.Attributes.Remove("class")
        '        Else
        '            OtroObjVisita.Attributes.Add("class", "Oculto")
        '            OtroObjVisitaDsc.Attributes.Add("class", "Oculto")
        '        End If
        '        Exit For
        '    Else
        '        OtroObjVisita.Attributes.Add("class", "Oculto")
        '        OtroObjVisitaDsc.Attributes.Add("class", "Oculto")
        '    End If
        'Next

        'If chkObjetoVisita.SelectedValue = "1" Then
        '    OtroObjVisita.Attributes.Remove("class")
        '    OtroObjVisitaDsc.Attributes.Remove("class")
        'Else
        '    OtroObjVisita.Attributes.Add("class", "Oculto")
        '    OtroObjVisitaDsc.Attributes.Add("class", "Oculto")
        'End If

        ''Se ocultan las subentidades para todo excepto financiero
        If chkSubVisitas.Checked Then
            trSubentidad.Attributes.Remove("class")
        Else
            trSubentidad.Attributes.Add("class", "Oculto")
        End If
    End Sub

    ''' <summary>
    ''' GENERAR EL FOLIO DE LA VISITA
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GeneraFolioVisita()
        ''No generarlo si se esta editando
        If Not IsNothing(Request.QueryString("up")) Then
            If Request.QueryString("up").ToString() = "1" Then
                Exit Sub
            End If
        End If

        If dplEntidad.SelectedValue <> "-1" And dplEntidad.SelectedValue <> "" Then
            If pgIdVisitaEditar > 0 Then
                If pgEstaEditandoSubvisita Then ''Hay subvisitas
                    If chkSubEntidad.SelectedValue.Trim() <> "" Then
                        Dim objTipoSub As TipoSubEntidad = (From TS As TipoSubEntidad In pLstTipoEntidad Where TS.IdSubEntidad = chkSubEntidad.SelectedValue).FirstOrDefault()

                        If Not IsNothing(objTipoSub) Then
                            txbFolioVisita.Text = pgIdConsecutivo & "/" & pgAreaUsuario & "/" & dplEntidad.SelectedItem.ToString() & "/" & Today.ToString("MMyy") & "/" & objTipoSub.DscTipoEntidad & objTipoSub.IdSubEntidad
                        End If
                    End If
                Else
                    txbFolioVisita.Text = pgIdConsecutivo & "/" & pgAreaUsuario & "/" & dplEntidad.SelectedItem.ToString() & "/" & Today.ToString("MMyy")
                End If
            Else
                txbFolioVisita.Text = "999" & "/" & pgAreaUsuario & "/" & dplEntidad.SelectedItem.ToString() & "/" & Today.ToString("MMyy")
            End If
        Else
            txbFolioVisita.Text = ""
        End If
    End Sub

    Private Function ObtenerIdTipoEntidad() As Integer
        Dim lsAux2 As String = ""
        Dim lsAux As String = ""
        Dim liAux As Integer = 0

        If dplEntidad.SelectedValue = "-1" Or dplEntidad.SelectedValue = "" Then
            Return -1
        Else
            lsAux = dplEntidad.SelectedValue

            If lsAux.Length > 1 Then
                lsAux2 = lsAux.Substring(lsAux.Length - 1)
                If Int32.TryParse(lsAux2, liAux) Then
                    Return liAux
                Else
                    Return -1
                End If
            Else
                Return CInt(lsAux)
            End If
        End If
    End Function

    Private Function ObtenerIdEntidad() As Integer
        Dim lsAux As String = ""
        Dim lsAux2 As String = ""
        Dim liAux As Integer = 0

        If dplEntidad.SelectedValue = "-1" Or dplEntidad.SelectedValue = "" Then
            Return -1
        Else
            lsAux = dplEntidad.SelectedValue

            If lsAux.Length > 1 Then
                lsAux2 = lsAux.Substring(0, lsAux.Length - 1)
                If Int32.TryParse(lsAux2, liAux) Then
                    Return liAux
                Else
                    Return -1
                End If
            Else
                Return CInt(lsAux)
            End If
        End If
    End Function

    ''' <summary>
    ''' Registra una subvisita
    ''' </summary>
    ''' <param name="objSubentidad"></param>
    ''' <param name="con"></param>
    ''' <param name="tran"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function registrarVisitasHijas(objSubentidad As Entities.SubEntidad, ByVal con As Conexion.SQLServer,
                                              ByVal tran As SqlClient.SqlTransaction, Optional idVisitaPadre As Integer = 0,
                                              Optional index As Integer = -1,
                                              Optional psFolioVisitaPadre As String = "") As Boolean
        Dim regExitoso As Boolean = False
        Dim lsFolio As String = ""
        Dim objTipoSub As New TipoSubEntidad

        visita = New Visita()
        Try
            If chkSubEntidad.SelectedValue.Trim() <> "" And index <> -1 Then
                ''Siempre debe de estar solo uno seleccionado
                objTipoSub = (From TS As TipoSubEntidad In pLstTipoEntidad Where TS.IdItem = index And TS.IdSubEntidad = objSubentidad.IdSubEntidad Select TS).FirstOrDefault()

                If Not IsNothing(objTipoSub) Then
                    If psFolioVisitaPadre <> "" Then
                        lsFolio = psFolioVisitaPadre & "/" & objTipoSub.DscTipoEntidad & objTipoSub.IdSubEntidad
                    Else
                        lsFolio = "999/" & pgAreaUsuario & "/" & dplEntidad.SelectedItem.ToString() & "/" & Today.ToString("MMyy") & "/" & objTipoSub.DscTipoEntidad & objTipoSub.IdSubEntidad
                    End If
                End If
            Else
                lsFolio = txbFolioVisita.Text.Trim()
            End If

            visita.FolioVisita = lsFolio
            visita.Usuario = New Entities.Usuario()
            visita.Usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session
            visita.IdArea = visita.Usuario.IdArea ' 35 - VICEPRESIDENCIA DE OPERACIONES, 36 - VICEPRESIDENCIA FINANCIERA, ODT dice Área que ejecuta: VO/VF, se debe obtener del perfil del usuario
            visita.FechaRegistro = Convert.ToDateTime(txbFechaRegistro.Text.Trim() & " " & DateTime.Now.Hour & ":" & DateTime.Now.Minute & ":" & DateTime.Now.Second)
            If txbFechaInicio.Text.Trim() <> "" Then
                visita.FechaInicioVisita = Convert.ToDateTime(txbFechaInicio.Text.Trim())
            End If

            visita.IdEntidad = dplEntidad.SelectedValue
            visita.IdTipoVisita = Convert.ToInt32(dplTipoVisita.SelectedValue)
            visita.IdEstatusActual = 1 ' 1 - Registrado - BDS_C_GR_ESTATUS_CAT
            visita.IdPasoActual = 1 ' 1 - Elaborar Oficios y Acta Inicial usando requisitos de VJ y enviar a VJ - BDS_C_GR_PASOS
            visita.NombreEntidad = dplEntidad.SelectedItem.Text
            visita.EsCancelada = False
            visita.MotivoCancelacion = String.Empty
            visita.FechaCancela = Nothing
            visita.IdSubentidad = objSubentidad.IdSubEntidad
            visita.DscSubentidad = objSubentidad.DscSubEntidad
            visita.IdTipoEntidad = objTipoSub.IdTipoEntidad
            visita.DscTipoEntidad = objTipoSub.DscTipoEntidad
            visita.DescripcionVisita = txtDescripcionVisita.Text
            'visita.IdObjetoVisita = ddlObjetoVisita.SelectedValue

            visita.DscObjetoVisitaOtro = SelObjetos.txtOtro

            Dim psIdVisita As String = ""

            visita.IdVisitaPadreSubvisita = idVisitaPadre
            visita.ComentariosIniciales = txbComentarios.Text
            If txtAcuerdoVul.Text = "" Then
                visita.Fecha_AcuerdoVul = Nothing
            Else
                visita.Fecha_AcuerdoVul = Fechas.toDate(txtAcuerdoVul.Text.Trim())
            End If
            visita.OrdenVisita = txtOrdenVisita.Text.Trim()

            visita.IdVisitaGenerado = AccesoBD.registrarVisita(visita, con, tran, psIdVisita)
            Session.Add("IdVisitaGenerado", visita.IdVisitaGenerado)
            Session.Add("IdVisitaGeneradoCadena", psIdVisita)
        Catch ex As Exception
            regExitoso = False
        End Try
        If visita.IdVisitaGenerado > 0 Then
            regExitoso = True
        Else
            regExitoso = False
        End If
        Return regExitoso
    End Function

    ''' <summary>
    ''' Metodo que carga los datos de una visita en el formulario para su edicion
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaFormularioParaEditar(idVisita As Integer, piIdArea As Integer)
        Dim visita As New Visita()
        visita = AccesoBD.getDetalleVisita(idVisita, piIdArea)

        If Not IsNothing(visita) And visita.FolioVisita <> String.Empty And visita.IdVisitaGenerado > 0 Then

            'RRA Agregar llenado de info de Visita Conjunta*****************************************
            CargaVisCon_ConIdVisita(idVisita.ToString)
            'FIN LLENADO VISITA CONJUNTA**********************************************************

            ''Obtienelos inspectores y sus correos
            visita.LstSupervisoresAsignados = AccesoBD.getSupervisoresAsignados(idVisita)
            visita.LstInspectoresAsignados = AccesoBD.getInspectoresAsignados(idVisita)

            txbFolioVisita.Text = visita.FolioVisita

            Dim vec() As String = visita.FolioVisita.Split("/")
            pgIdConsecutivo = vec(0)

            txbFechaRegistro.Text = visita.FechaRegistro.ToString("dd/MM/yyyy")
            txbFechaRegistro.ReadOnly = True
            calendarioFechaRegistro.Enabled = False
            calendarioFechaRegistro.EnabledOnClient = False
            txbFechaRegistro.Attributes.Add("onkeydown", "return false;")

            imgCalendario1.Visible = False

            If visita.FechaInicioVisita <> Date.MinValue Then
                txbFechaInicio.Text = visita.FechaInicioVisita
            End If

            ddlTipoEntidad.SelectedValue = visita.IdTipoEntidad
            dplEntidad.SelectedValue = visita.IdEntidad.ToString()
            dplEntidad_SelectedIndexChanged(dplEntidad, New EventArgs())

            dplEntidad.Enabled = False

            ''Solo financiero puede seleccionar subentidades
            If visita.IdSubentidad <> 0 And piIdArea = Constantes.AREA_VF Then
                ''Elige correctamente la sub entidad, ya que como se repiten en el combo los id de las sub entidades no seleccionaba bien la correcta de ahi toda la 
                ''bronca con estas madres, entidades y sub entidades :/

                Dim objTipoEentidad As TipoSubEntidad = (From TS As TipoSubEntidad In pLstTipoEntidad Where TS.IdSubEntidad = visita.IdSubentidad And TS.IdTipoEntidad = visita.IdTipoEntidad Select TS).FirstOrDefault()

                If Not IsNothing(objTipoEentidad) Then
                    chkSubEntidad.SelectedIndex = objTipoEentidad.IdItem
                    hfOpAct.Value = objTipoEentidad.IdItem ''Guarda la opcion seleccionada

                    If chkSubEntidad.SelectedValue = "" Then
                        chkSubEntidad.SelectedValue = visita.IdSubentidad

                        ''Busca la opcion seleccionada
                        For i As Integer = 0 To chkSubEntidad.Items.Count - 1
                            If chkSubEntidad.Items(i).Selected Then
                                hfOpAct.Value = i
                                Exit For
                            End If
                        Next
                    End If
                Else
                    chkSubEntidad.SelectedValue = visita.IdSubentidad
                    ''Busca la opcion seleccionada
                    For i As Integer = 0 To chkSubEntidad.Items.Count - 1
                        If chkSubEntidad.Items(i).Selected Then
                            hfOpAct.Value = i
                            Exit For
                        End If
                    Next
                End If

                chkSubVisitas.Checked = True
            End If

            dplTipoVisita.SelectedValue = visita.IdTipoVisita

            'For Each liIdObjeto As Visita.ObjetoVisita In visita.LstObjetoVisita
            '    For Each liChkList As ListItem In chkObjetoVisita.Items
            '        If liChkList.Value = liIdObjeto.Id Then
            '            liChkList.Selected = True
            '            Exit For
            '        End If
            '    Next

            '    ''Valida si se selecciono 'otro'
            '    If liIdObjeto.Id = 1 Then
            '        txtObjetoVisita.Text = visita.DscObjetoVisitaOtro
            '        txtObjetoVisita.Enabled = True
            '        txtObjetoVisita.Visible = True
            '    End If
            'Next


            Dim liItem As New ListItem
            'liItem = lsbUsuariosDisponibles.Items.FindByValue(visita.IdInspectorResponsable)
            'If Not IsNothing(liItem) Then
            '    lsbSupervisor.Items.Add(liItem)
            '    lsbUsuariosDisponibles.Items.Remove(liItem)
            'End If

            For Each liItemIns As SupervisorAsignado In visita.LstSupervisoresAsignados
                liItem = lsbUsuariosDisponibles.Items.FindByValue(liItemIns.Id)
                If Not IsNothing(liItem) Then
                    lsbSupervisor.Items.Add(liItem)
                    lsbUsuariosDisponibles.Items.Remove(liItem)
                End If
            Next

            For Each liItemIns As InspectorAsignado In visita.LstInspectoresAsignados
                liItem = lsbUsuariosDisponibles.Items.FindByValue(liItemIns.Id)
                If Not IsNothing(liItem) Then
                    lsbInspectores.Items.Add(liItem)
                    lsbUsuariosDisponibles.Items.Remove(liItem)
                End If
            Next

            txtDescripcionVisita.Text = visita.DescripcionVisita
            txbComentarios.Text = visita.ComentariosIniciales
            If txtAcuerdoVul.Text = "" Then
                txtAcuerdoVul.Text = Nothing
            Else
                txtAcuerdoVul.Text = Fechas.Valor(visita.Fecha_AcuerdoVul)
            End If
            txtOrdenVisita.Text = visita.OrdenVisita

            ' ''Inicializar la visita del control documento agc
            ' ''**************************************************************************************
            'Dim objPropiedades As New ucDocumentos.PropiedadesDoc

            'objPropiedades.piIdWidthPanel = 65
            'objPropiedades.piIdHeightPanel = 300
            'objPropiedades.ppObjVisita = visita
            'objPropiedades.piIdPasoDocumentos = visita.IdPasoActual
            'objPropiedades.pbEstaEditandoVisita = True

            'cuDocumentos.VisiblePanelEncabezado = False
            'cuDocumentos.pObjPropiedades = objPropiedades
            ' ''**************************************************************************************
        End If
    End Sub

    ''' <summary>
    ''' Guarda una visita que ha sido editada
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GuardaEditarVisita()
        Dim con As Conexion.SQLServer = Nothing
        Dim tran As SqlClient.SqlTransaction = Nothing
        Dim guardo As Boolean = False
        Dim psIdVisita As String = ""

        visita = ObtenerDatosFormulario()
        Dim visitaSisvig As New Entities.Sisvig() 'MCS

        Try
            con = New Conexion.SQLServer()
            tran = con.BeginTransaction()

            visita.IdVisitaGenerado = AccesoBD.registrarVisita(visita, con, tran, psIdVisita, Constantes.OPERCION.Actualizar)

            If visita.IdVisitaGenerado > 0 Then
                If registrarObjetoVisitaReg(con, tran) Then
                    If registrarSupervisores(con, tran) = True And registrarInspectores(con, tran) = True Then
                        guardo = True
                        'INICIO Agrega funcionalidad de insertar y eliminar registros de visitas conjuntas
                        Dim area As New Areas 'Para ocupar metodo "InsertaElimina_VisitasConjuntas"
                        If InsElimVisCon = True Then
                            'area.InsertaElimina_VisitasConjuntas("1", QueryInsVC)
                            area.InsertaElimina_VisitasConjuntas(visita.IdVisitaGenerado, QueryInsVC)
                        Else
                            area.InsertaElimina_VisitasConjuntas(visita.IdVisitaGenerado, "-")
                        End If

                        ''===============================================================================================
                        'ACTUALIZAR EN SISVIG MCS en visitas_inspeccion
                        'CREAR SP PARA ACTUALIZAR EN LA TABLA DE SISVIG
                        ''===============================================================================================
                        If visita.VisitaSisvig Then
                            'Actualiza en la TABLA BSIS_X_VISITA_INSPECCION - OK
                            visitaSisvig.ActualizaSepris_Sisvig(visita.IdVisitaSisvig, psIdVisita, visita.IdTipoVisita, visita.FechaInicioVisita, visita.DescripcionVisita, visita.IdInspectorResponsable, visita.NombreInspectorResponsable, visita.OrdenVisita)

                            'Actualiza o inserta en sisvig(BSIS_X_INSPECTOR)
                            visita.LstInspectoresAsignados = New List(Of InspectorAsignado)
                            Dim nombInspect As String = ""
                            For Each inspector In visita.LstInspectoresAsignados
                                nombInspect &= inspector.Id.ToString() & "|"
                            Next

                            'Ejecuta SP para actualizar inspectores
                            visitaSisvig.ActualizaInspectores_Sisvig(nombInspect, visita.IdVisitaSisvig)

                            'Obtiene los objetos de la visita
                            Dim idsProcesos As String = ""
                            For Each liIdObjeto As Visita.ObjetoVisita In visita.LstObjetoVisita
                                If liIdObjeto.IdSisan <> -1 Then
                                    idsProcesos &= liIdObjeto.IdSisan.ToString() & "|"
                                End If
                            Next

                            'Ejecuta SP para actualizar inspectores
                            Dim lstObjVisita As New Visita.ObjetoVisita
                            visitaSisvig.ActualizaProcesos_Sisvig(visita.IdVisitaSisvig, idsProcesos)
                        End If

                        'FIN Agrega funcionalidad de insertar y eliminar registros de visitas conjuntas
                        'If registrarPasoUno(con, tran) = True Then
                        'If registrarEstatusPaso(con, tran) = True Then

                        'End If
                        'End If
                    End If
                End If
            End If
        Catch ex As Exception
            'Registro fallido
            guardo = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Registro.aspx.vb, imgRegistrarVisita_Click", "")
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgError
            Dim errores As New Entities.EtiquetaError(2122)
            Mensaje = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
        Finally
            If Not IsNothing(tran) Then
                If guardo Then
                    'Registro exitoso
                    tran.Commit()
                    'Dim objDocumentoTemp As New Entities.DocumentoTemp
                    'objDocumentoTemp.EliminarDocumentosTemporales(idOficios, idActa)
                    Session("IdOficioComision") = Nothing
                    Session("IdActaInicio") = Nothing
                    Session("IdVisitaGenerado") = Nothing
                    ''Actualiza el folio de la visita en el objeto visita
                    visita.FolioVisita = psIdVisita
                    visita.Fecha_LimitePasoActual = AccesoBD.ObtenFechaLimiteAtender_Paso(visita.IdVisitaGenerado, PasoProcesoVisita.Pasos.Uno)

                    Dim objUsuario As New Entities.Usuario()
                    objUsuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

                    If Not IsNothing(objUsuario) Then
                        Dim bitacora As New Conexion.Bitacora("Se edito la visita(" & psIdVisita & ")", System.Web.HttpContext.Current.Session.SessionID, objUsuario.IdentificadorUsuario)
                        bitacora.Finalizar(True)

                        'Notifica al supervisor asignado a la visita
                        Dim objNegVisita As New NegocioVisita(visita, objUsuario, Server, "Edición: " & txbComentarios.Text)
                        ''Notificar la edicion y guardar es nuevo comentario
                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual, , , , , ,
                                                                                True, Constantes.CORREO_EDICION_VISITA,,,,,,,,,,,,, Constantes.MovimientoPaso.RegistroVisita
                                                                                )
                    End If
                Else
                    'Registro fallido
                    tran.Rollback()
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgError
                    Dim errores As New Entities.EtiquetaError(2122)
                    Mensaje = errores.Descripcion
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
                End If
                tran.Dispose()
            End If

            If Not IsNothing(con) Then
                con.CerrarConexion()
                con = Nothing
            End If
            If guardo Then
                imgConfirmaRegistroVisita.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                Mensaje = "Se ha actualizado satisfactoriamente la visita: <br /><br />" & psIdVisita & "</ul>"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmaRegistroVisita();", True)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Retorna una entidad de visita llena
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ObtenerDatosFormulario() As Visita
        Dim objVisita = New Visita()
        Dim objTipoSub As New TipoSubEntidad
        Dim lsFolio As String = ""

        ''SI ESTA EDITANDO DESDE DETALLE OBTIENE EL ESTATUS Y EL PASO DESDE BASE DE DATOS
        If Not IsNothing(Request.QueryString("r")) Then
            If Not Int32.TryParse(Session("idPasoActualEditada"), objVisita.IdPasoActual) Then objVisita.IdPasoActual = 1
            If Not Int32.TryParse(Session("idEstatusActualEditada"), objVisita.IdEstatusActual) Then objVisita.IdEstatusActual = 1
        Else
            objVisita.IdEstatusActual = 1 ' 1 - Registrado - BDS_C_GR_ESTATUS_CAT
            objVisita.IdPasoActual = 1 ' 1 - Elaborar Oficios y Acta Inicial usando requisitos de VJ y enviar a VJ - BDS_C_GR_PASOS
        End If

        If chkSubEntidad.SelectedValue.Trim() <> "" Then
            ''Siempre debe de estar solo uno seleccionado
            For i As Integer = 0 To chkSubEntidad.Items.Count - 1
                If chkSubEntidad.Items(i).Selected Then
                    objTipoSub = (From TS As TipoSubEntidad In pLstTipoEntidad Where TS.IdItem = i And TS.IdSubEntidad = chkSubEntidad.SelectedValue Select TS).FirstOrDefault()
                    Exit For
                End If
            Next

            If Not IsNothing(objTipoSub) Then
                lsFolio = pgIdConsecutivo & "/" & pgAreaUsuario & "/" & dplEntidad.SelectedItem.ToString() & "/" & Today.ToString("MMyy") & "/" & objTipoSub.DscTipoEntidad & objTipoSub.IdSubEntidad
            End If
        Else
            lsFolio = txbFolioVisita.Text.Trim()
        End If

        objVisita.IdVisitaGenerado = pgIdVisitaEditar
        objVisita.FolioVisita = lsFolio
        objVisita.Usuario = New Entities.Usuario()
        objVisita.Usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session
        objVisita.IdArea = objVisita.Usuario.IdArea ' 35 - VICEPRESIDENCIA DE OPERACIONES, 36 - VICEPRESIDENCIA FINANCIERA, ODT dice Área que ejecuta: VO/VF, se debe obtener del perfil del usuario
        objVisita.FechaRegistro = Convert.ToDateTime(txbFechaRegistro.Text.Trim() & " " & DateTime.Now.Hour & ":" & DateTime.Now.Minute & ":" & DateTime.Now.Second)
        If txbFechaInicio.Text.Trim() <> "" Then
            objVisita.FechaInicioVisita = Convert.ToDateTime(txbFechaInicio.Text.Trim())
        End If

        objVisita.IdTipoEntidad = objTipoSub.IdTipoEntidad
        objVisita.DscTipoEntidad = objTipoSub.DscTipoEntidad
        objVisita.IdEntidad = dplEntidad.SelectedValue
        objVisita.IdTipoVisita = Convert.ToInt32(dplTipoVisita.SelectedValue)
        objVisita.NombreEntidad = dplEntidad.SelectedItem.Text
        objVisita.EsCancelada = False
        objVisita.MotivoCancelacion = String.Empty
        objVisita.FechaCancela = Nothing
        objVisita.IdSubentidad = If(chkSubEntidad.SelectedValue <> "", Convert.ToInt32(chkSubEntidad.SelectedValue), 0)
        objVisita.DscSubentidad = If(chkSubEntidad.SelectedValue <> "", chkSubEntidad.SelectedItem.Text, "")
        objVisita.DescripcionVisita = txtDescripcionVisita.Text
        'objVisita.IdObjetoVisita = ddlObjetoVisita.SelectedValue

        objVisita.DscObjetoVisitaOtro = SelObjetos.txtOtro

        objVisita.ComentariosIniciales = txbComentarios.Text
        If txtAcuerdoVul.Text = "" Then
            objVisita.Fecha_AcuerdoVul = Nothing
        Else
            objVisita.Fecha_AcuerdoVul = Fechas.toDate(txtAcuerdoVul.Text)
        End If

        objVisita.OrdenVisita = txtOrdenVisita.Text

        Return objVisita
    End Function

    Protected Sub chkSubEntidad_SelectedIndexChanged(sender As Object, e As EventArgs)
        If pgEstaEditandoSubvisita Then ''Hay subvisitas
            If chkSubEntidad.SelectedValue.Trim() <> "" Then
                Dim objTipoSub As New TipoSubEntidad

                ''Siempre debe de estar solo uno seleccionado
                For i As Integer = 0 To chkSubEntidad.Items.Count - 1
                    If chkSubEntidad.Items(i).Selected Then
                        objTipoSub = (From TS As TipoSubEntidad In pLstTipoEntidad Where TS.IdItem = i And TS.IdSubEntidad = chkSubEntidad.SelectedValue Select TS).FirstOrDefault()
                        Exit For
                    End If
                Next

                If Not IsNothing(objTipoSub) Then
                    txbFolioVisita.Text = pgIdConsecutivo & "/" & pgAreaUsuario & "/" & dplEntidad.SelectedItem.ToString() & "/" & Today.ToString("MMyy") & "/" & objTipoSub.DscTipoEntidad & objTipoSub.IdSubEntidad
                End If
            End If
        End If
    End Sub
    Private Function registrarObjetoVisitaReg(con As Conexion.SQLServer, tran As SqlClient.SqlTransaction) As Boolean
        Dim lsListaObjetoVisita As String = ""

        Dim objSelect As List(Of ListItem) = SelObjetos.GetObjetosSeleccionados()

        For Each liChkList As ListItem In objSelect
            lsListaObjetoVisita &= liChkList.Value.ToString() & "|"
        Next

        lsListaObjetoVisita = lsListaObjetoVisita.Substring(0, lsListaObjetoVisita.Length - 1)
        Return AccesoBD.registrarObjetoVisita(visita.IdVisitaGenerado, lsListaObjetoVisita, con, tran)
    End Function

    Private Function ObtieneDestinatariosRegistro() As List(Of Persona)
        Dim lstPersonas As New List(Of Persona)
        Dim objUsuario As New Entities.Usuario(visita.Usuario.IdentificadorUsuario)
        Dim datosUsuario As New Dictionary(Of String, String)
        Dim objPersona As New Persona

        If Not IsNothing(objUsuario) Then
            objPersona.Nombre = objUsuario.IdentificadorUsuario

            If objUsuario.Mail <> "" Then
                objPersona.Correo = objUsuario.Mail
                lstPersonas.Add(objPersona)
            Else
                datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(visita.Usuario.IdentificadorUsuario)

                If datosUsuario.Count > 0 Then
                    objPersona.Correo = datosUsuario.Item("mail")
                    lstPersonas.Add(objPersona)
                End If
            End If
        Else
            datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(visita.Usuario.IdentificadorUsuario)

            If datosUsuario.Count > 0 Then
                objPersona.Correo = datosUsuario.Item("mail")
                objPersona.Nombre = ""

                lstPersonas.Add(objPersona)
            End If
        End If


        objUsuario = New Entities.Usuario(visita.LstSupervisoresAsignados(0).Id)
        objPersona = New Persona

        If Not IsNothing(objUsuario) Then
            objPersona.Nombre = objUsuario.IdentificadorUsuario

            If objUsuario.Mail <> "" Then
                objPersona.Correo = objUsuario.Mail
                lstPersonas.Add(objPersona)
            Else
                datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(visita.LstSupervisoresAsignados(0).Id)

                If datosUsuario.Count > 0 Then
                    objPersona.Correo = datosUsuario.Item("mail")
                    lstPersonas.Add(objPersona)
                End If
            End If
        Else
            datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(visita.LstSupervisoresAsignados(0).Id)

            If datosUsuario.Count > 0 Then
                objPersona.Correo = datosUsuario.Item("mail")
                lstPersonas.Add(objPersona)
            End If
        End If

        Return lstPersonas
    End Function


    Private Function ObtenerCorreoUsuario(psIdUsuario As String) As String
        Dim datosUsuario As New Dictionary(Of String, String)
        Dim lsCorreo As String = ""

        If Not IsNothing(psIdUsuario) Then
            If psIdUsuario.Trim().Length > 1 Then
                Dim objUsuario As New Entities.Usuario(psIdUsuario)

                If Not IsNothing(objUsuario) Then
                    If objUsuario.Mail <> "" Then
                        lsCorreo = objUsuario.Mail
                    Else
                        datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(psIdUsuario)

                        If datosUsuario.Count > 0 Then
                            lsCorreo = datosUsuario.Item("mail")
                        End If
                    End If
                Else
                    datosUsuario = Conexion.ActiveDirectory.ObtenerUsuario(psIdUsuario)

                    If datosUsuario.Count > 0 Then
                        lsCorreo = datosUsuario.Item("mail")
                    End If
                End If
            End If
        End If

        Return lsCorreo
    End Function

    'CAMBIOS DE RRA
    Protected Sub ChkVConjunta_CheckedChanged(sender As Object, e As EventArgs) Handles ChkVConjunta.CheckedChanged
        If (ChkVConjunta.Checked) Then
            chkListVC.Visible = True
            chkListVC.ClearSelection()
            reseteaObjetosVisita_UsuariosDisponibles()
        Else
            chkListVC.Visible = False
            chkListVC.ClearSelection()
            reseteaObjetosVisita_UsuariosDisponibles()
        End If
    End Sub

    Public Sub reseteaObjetosVisita_UsuariosDisponibles()
        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session
        cargarObjetoVisita(usuario)
        cargarUsuariosDisponibles()

        lsbSupervisor.Items.Clear()
        lsbInspectores.Items.Clear()

        udpAsignacionUsuarios.Update()
    End Sub

    Protected Sub chkListVC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkListVC.SelectedIndexChanged
        'Actualiza Objetos de la visita de acuerdo al id en sesion y opciones seleccionadas
        AgregaQuita_ObjetoVisita()
        AgregaQuita_UsuariosDisponibles()
    End Sub
    Public Sub AgregaQuita_ObjetoVisita()
        Dim area As New Areas
        Dim indices As New List(Of Integer)
        Dim ids As String = ""
        Dim dsObjVisita As DataSet
        Dim incremento As Integer

        reseteaObjetosVisita_UsuariosDisponibles()
        'Agrego los Ids a lista para crear in
        indices = CheckListValida(chkListVC)

        'Se crea el IN para la consulta
        For Each datos In indices
            incremento += 1
            If indices.Count = incremento Then
                ids += datos.ToString
            Else
                ids += datos.ToString + ","
            End If
        Next

        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session
        SelObjetos.ClearList()
        SelObjetos.cargarObjetoVisita(usuario)

        'If indices.Count > 0 Then
        '    dsObjVisita = area.ObjVisita_AreaSeleccion(ids)

        '    'Agrego los Objetos visita al checklist
        '    For Each row As DataRow In dsObjVisita.Tables(0).Rows
        '        chkObjetoVisita.Items.Insert(0, New ListItem(row("DSC"), row("ID_OBJVIS").ToString()))
        '    Next
        'End If
    End Sub

    Public Sub AgregaQuita_UsuariosDisponibles()
        Dim Area As New Areas
        Dim dsUsuariosDisponibles As New DataSet
        Dim indices As New List(Of Integer)
        cargarUsuariosDisponibles()

        'Agrego los Ids a lista para crear in
        indices = CheckListValida(chkListVC)

        'Agrego los Objetos visita al checklist
        If indices.Count > 0 Then
            dsUsuariosDisponibles = Area.Usuarios_AreaSeleccion(generaIdsIn(indices))
            For Each row As DataRow In dsUsuariosDisponibles.Tables(0).Rows
                lsbUsuariosDisponibles.Items.Add(New ListItem(row("NOMBRE").ToString, row("T_ID_USUARIO").ToString()))
            Next
            lsbSupervisor.Items.Clear()
            lsbInspectores.Items.Clear()
        End If
        udpAsignacionUsuarios.Update()
    End Sub

    Public Sub cargaListaVConjunta()
        Dim area As New Areas
        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session
        chkListVC.DataSource = area.AreasActivas_SinSesion(usuario.IdArea)
        chkListVC.DataTextField = "T_DSC_AREA"
        chkListVC.DataValueField = "I_ID_AREA"
        chkListVC.DataBind()
    End Sub

    'Public Sub InsertaElimina_VisitasConjuntas()
    '    Dim area As New Areas
    '    Dim indices As New List(Of Integer)
    '    Dim incremento As Integer
    '    Dim ids As String = ""

    '    'Agrego los Ids a lista para crear in
    '    indices = CheckListValida(chkListVC)

    '    'Se crea el IN para la consulta
    '    For Each datos In indices
    '        incremento += 1
    '        If indices.Count = incremento Then
    '            ids += datos.ToString
    '        Else
    '            ids += datos.ToString + ","
    '        End If
    '    Next

    '    area.InsertaElimina_VisitasConjuntas(2, ids)

    'End Sub

    'valida total de chechk seleccionados en un CheckBoxList
    Public Function CheckListValida(ByVal checkBoxlist As CheckBoxList) As List(Of Integer)
        Dim indices As New List(Of Integer)
        'Agrego los Ids a lista para crear in
        For Each conteo As ListItem In checkBoxlist.Items
            If conteo.Selected = True Then
                indices.Add(conteo.Value)
            End If
        Next
        Return indices
    End Function

    'Recupera de lista la cadena para consultas que incluyen IN
    Public Function generaIdsIn(ByVal indices As List(Of Integer)) As String
        Dim incremento As Integer
        Dim ids As String = ""

        For Each datos In indices
            incremento += 1
            If indices.Count = incremento Then
                ids += datos.ToString
            Else
                ids += datos.ToString + ","
            End If
        Next
        Return ids
    End Function

    Public Sub CargaVisCon_ConIdVisita(ByVal idVisita As String)
        Dim area As New Areas
        Dim Tabla As New DataSet

        Tabla = area.ObtenerAreasVisitaConjunta_IdVisita(idVisita)
        If Tabla.Tables(0).Rows.Count > 0 Then
            ChkVConjunta.Checked = True
            chkListVC.Visible = True
            For Each row As DataRow In Tabla.Tables(0).Rows
                chkListVC.Items.FindByValue(row("I_ID_AREA_VC").ToString).Selected = True
            Next
        End If

        AgregaQuita_ObjetoVisita()
        AgregaQuita_UsuariosDisponibles()

    End Sub

    Private Function RecuperaParametroValidaCheckCopiarFolios() As Boolean
        Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.Activa_DesactivaCopiarFolio)
        Dim Estatus As Boolean

        ''Conexion.SQLServer
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("T_DSC_VALOR").ToString = "1" Then
                Estatus = True
            Else
                Estatus = False
            End If
        End If
        Return Estatus
    End Function

    Private Sub imgProcesoVisitaAmbos_Click(sender As Object, e As ImageClickEventArgs) Handles imgProcesoVisitaAmbos.Click
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroOpcionesImgProcesos();", True)
    End Sub

    Protected Sub btnConfirmacionVulnerabilidadSi_Click(sender As Object, e As EventArgs) Handles btnConfirmacionVulnerabilidadSi.Click

        ScriptManager.RegisterStartupScript(Me, GetType(Page), "HOLA1", "SolicitarFechaVulnerabilidadRegresa();", True)

        Exit Sub
    End Sub

    Protected Sub btnConfirmacionSiHabraRevision_Click(sender As Object, e As EventArgs) Handles btnConfirmacionSiHabraRevision.Click

        Dim objUsuario As New Entities.Usuario()
        objUsuario = Session(Entities.Usuario.SessionID)

        Dim visita As New Visita()

        If IsNothing(ppObjVisita) Then
            visita = AccesoBD.getDetalleVisita(hfIdVisitaGen.Value, objUsuario.IdArea)
        Else
            visita = ppObjVisita
        End If

        Dim objNegVisita As New NegocioVisita(ppObjVisita, puObjUsuario, Server, txbComentarios.Text)

        Dim conex = New Conexion.SQLServer()
        Dim query As String = "UPDATE BDS_D_VS_VISITA SET T_ESTATUS_VULNERABILIDAD = 1, T_VULNERA_REALIZADA = 0, F_FECH_ACUERDO_VULNERA = NULL WHERE I_ID_VISITA = " + visita.IdVisitaGenerado.ToString
        conex.Ejecutar(query)

        If MandaCorreoSandraPachecoVulnerabilidad(objNegVisita, Constantes.CORREO_VULNERABILIDAD_NOTIFICAR_SANDRA, hfIdVisitaGen.Value) Then
            Mensaje = "Se ha registrado satisfactoriamente la visita: <br /><br />" & hdVisitasGen.Value.Replace("--", "<").Replace("|", "</").Replace("..", ">") & "</ul>"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmaRegistroVisita();", True)
        End If

    End Sub

    Protected Sub btnConfirmacionNoHabraRevision_Click(sender As Object, e As EventArgs) Handles btnConfirmacionNoHabraRevision.Click

        Dim objUsuario As New Entities.Usuario()
        objUsuario = Session(Entities.Usuario.SessionID)

        Dim visita As New Visita()

        If IsNothing(ppObjVisita) Then
            visita = AccesoBD.getDetalleVisita(hfIdVisitaGen.Value, objUsuario.IdArea)
        Else
            visita = ppObjVisita
        End If

        Dim conex = New Conexion.SQLServer()
        Dim query As String = "UPDATE BDS_D_VS_VISITA SET T_ESTATUS_VULNERABILIDAD = 0, T_VULNERA_REALIZADA = 0, F_FECH_ACUERDO_VULNERA = NULL WHERE I_ID_VISITA = " + visita.IdVisitaGenerado.ToString
        conex.Ejecutar(query)

        Mensaje = "Se ha registrado satisfactoriamente la visita: <br /><br />" & hdVisitasGen.Value.Replace("--", "<").Replace("|", "</").Replace("..", ">") & "</ul>"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmaRegistroVisita();", True)

    End Sub

    Protected Sub btnConfirmacionVulnerabilidadNo_Click(sender As Object, e As EventArgs) Handles btnConfirmacionVulnerabilidadNo.Click

        Mensaje2 = "¿Habrá reunión de revisión de vulnerabilidades?"
        Mensaje2 += "<ul>" +
                            "<li>" +
                                "Si: Se enviará correo solicitando fecha para la revisión de vulnerabilidades y estará habilitada la opción para ingresar la fecha hasta el paso 3." +
                            "</li>" +
                            "<li>" +
                                "No: No se enviará correo de solicitud de fecha para la revisión de vulnerabilidades." +
                            "</li>" +
                        "</ul>"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ConfirmacionVulnerabilidad", "ConfirmacionHabraRevision();", True)
        Exit Sub

    End Sub

    Protected Sub btnGuardaFecVulnerabilidad_Click(sender As Object, e As EventArgs) Handles btnGuardaFecVulnerabilidad.Click
        Dim lsFecha As String = txtFechaGeneral.Text.Trim()
        Dim ldFecha As DateTime
        Dim objUsuario As New Entities.Usuario()
        objUsuario = Session(Entities.Usuario.SessionID)

        Dim visita As New Visita()

        If IsNothing(ppObjVisita) Then
            visita = AccesoBD.getDetalleVisita(hfIdVisitaGen.Value, objUsuario.IdArea)
        Else
            visita = ppObjVisita
        End If

        'Notifica a los usuarios involucrados que tengan el perfil seleccionado
        Dim objNegVisita As NegocioVisita
        objNegVisita = New NegocioVisita(visita, objUsuario, Server, txbComentarios.Text)

        ldFecha = ValidarFechaGeneral(lsFecha, lblFechaGeneral, imgErrorGeneral, "SolicitarFechaGeneral('" & Constantes.MensajesModal.Vulnerabilidades & "', 'btnGuardaFecVulnerabilidad');", False, False, False, False, )

        If ldFecha = DateTime.MinValue Then
            Exit Sub

        Else

            ''Validar si antes ya se habia ingresado alguna fecha de vulnerabilidades de lo contrario validar que la nueva fecha sea igual o menor que el dia de hoy
            'If ppObjVisita.FechaAcuerdoVul.Trim().Length <= 0 Then
            'If ldFecha.Date > visita.FechaInicioVisita Then
            If ldFecha.Date > DateTime.Now.Date Then
                lblFechaGeneral.Text = "La fecha ingresada no puede ser mayor a la fecha actual."
                lblFechaGeneral.Visible = True
                imgErrorGeneral.ImageUrl = Constantes.Imagenes.Fallo
                ScriptManager.RegisterStartupScript(Me, GetType(Page), "valor", "SolicitarFechaGeneral('" & Constantes.MensajesModal.Vulnerabilidades & "', 'btnGuardaFecVulnerabilidad');", True)
                Exit Sub
            End If
            'End If


            Dim conex = New Conexion.SQLServer()
            Dim query As String = "UPDATE  BDS_D_VS_VISITA  SET  T_ESTATUS_VULNERABILIDAD = 1, T_VULNERA_REALIZADA = 1,  F_FECH_ACUERDO_VULNERA ='" + Convert.ToDateTime(lsFecha).ToString("yyyy/MM/dd") + "' WHERE I_ID_VISITA = " + visita.IdVisitaGenerado.ToString
            conex.Ejecutar(query)

            'INSERTA LA FECHA ESTIMADA
            AccesoBD.InsertaFechaInicioVisita_TblFechasEstimadas(visita.IdVisitaGenerado, 4, visita.IdArea, 1, ldFecha)
            'ACTUALIZA LA FECHA REAL
            AccesoBD.ActualizaFechaInicioVisita_TblFechasEstimadas(visita.IdVisitaGenerado, 4, ldFecha)

            Dim visitaN As New Visita()

            If IsNothing(ppObjVisita) Then
                visitaN = AccesoBD.getDetalleVisita(hfIdVisitaGen.Value, objUsuario.IdArea)
            Else
                visitaN = ppObjVisita
            End If

            objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_REUNION_VULNERA_REALIZADA, visitaN, True, False, False, , True)

            Mensaje = "Se ha registrado satisfactoriamente la visita: <br /><br />" & hdVisitasGen.Value.Replace("--", "<").Replace("|", "</").Replace("..", ">") & "</ul>"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmaRegistroVisita();", True)

        End If

    End Sub

    Protected Sub btnNoFecVulnerabilidad_Click(sender As Object, e As EventArgs) Handles btnNoFecVulnerabilidad.Click

        imgConfirmaRegistroVisita.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
        Mensaje2 = "¿Ya se ha llevado a cabo la  revisión de vulnerabilidades?"

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionFechaVulnerabilidad();", True)

        Exit Sub
    End Sub

    Protected Sub btnCancelaFecVulnerabilidad_Click(sender As Object, e As EventArgs) Handles btnCancelaFecVulnerabilidad.Click

        imgConfirmaRegistroVisita.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
        Mensaje2 = "¿Ya se ha llevado a cabo la  revisión de vulnerabilidades?"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionFechaVulnerabilidad();", True)

    End Sub

    ''' <summary>
    ''' Valida una fecha
    ''' </summary>
    ''' <param name="lsFecha"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidarFechaGeneral(ByVal lsFecha As String,
                                         ByRef lblFecha As Label,
                                         ByRef imgImagenError As Image,
                                         ByVal psScripAEjecutar As String,
                                         ByVal pbValidaFechaInicioPasoActual As Boolean,
                                         Optional pbAgregaFormato24horas As Boolean = False,
                                         Optional pbValidaNdiasHabiles As Boolean = False,
                                         Optional pbValidaFechaFinalizaPasoSig As Boolean = False,
                                         Optional piPasoSiguiente As PasoProcesoVisita.Pasos = 0) As DateTime
        Dim ldFecha As Date

        If lsFecha.Length < 1 Then
            imgImagenError.ImageUrl = Constantes.Imagenes.Fallo
            Dim errores As New Entities.EtiquetaError(2149)
            lblFecha.Visible = True
            lblFecha.Text = errores.Descripcion

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", psScripAEjecutar, True)
            Return DateTime.MinValue
        Else
            If Not Date.TryParse(lsFecha & IIf(pbAgregaFormato24horas, " 23:59:00", ""), ldFecha) Then
                imgImagenError.ImageUrl = Constantes.Imagenes.Fallo
                Dim errores As New Entities.EtiquetaError(2150)
                lblFecha.Visible = True
                lblFecha.Text = errores.Descripcion

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", psScripAEjecutar, True)
                Return DateTime.MinValue
            Else
                If Not EsDiaHabil(ldFecha) Then
                    imgImagenError.ImageUrl = Constantes.Imagenes.Fallo
                    Dim errores As New Entities.EtiquetaError(2142)
                    lblFecha.Visible = True
                    lblFecha.Text = errores.Descripcion

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", psScripAEjecutar, True)
                    Return DateTime.MinValue
                Else
                    Dim liNumDiasAux As Integer = 0

                    'Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.DiasFechasAnteriores), liNumDiasAux)

                    liNumDiasAux = 100

                    Dim ldFechaAuxAnterior As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDiasAux, Constantes.IncremeteDecrementa.Decrementa)

                    If ldFecha.Date < ldFechaAuxAnterior.Date Then
                        imgImagenError.ImageUrl = Constantes.Imagenes.Fallo
                        Dim errores As New Entities.EtiquetaError(2165)
                        lblFecha.Visible = True
                        lblFecha.Text = errores.Descripcion.Replace("[FECHA]", ldFechaAuxAnterior.Date.ToString("dd/MM/yyyy"))

                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", psScripAEjecutar, True)
                        Return DateTime.MinValue
                    Else
                        ''VALIDA SI LA FECHA ES MAYOR A LA FECHA DEL INICIAL DEL PASO ACTUAL
                        If pbValidaFechaInicioPasoActual Then
                            Dim ldFechaAux As Date
                            If txbFechaInicio.Text <> DateTime.MinValue Then
                                ldFechaAux = txbFechaInicio.Text
                            Else
                                ldFechaAux = ldFecha
                            End If

                            If ldFecha.Date < ldFechaAux.Date Then
                                imgImagenError.ImageUrl = Constantes.Imagenes.Fallo
                                Dim errores As New Entities.EtiquetaError(2164)
                                lblFecha.Visible = True
                                lblFecha.Text = errores.Descripcion.Replace("[FECHA]", ldFechaAux.ToString("dd/MM/yyyy"))

                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", psScripAEjecutar, True)
                                Return DateTime.MinValue
                            End If
                        End If

                        ''VALIDAR FECHA MAYOR A 3 DIAS HABILES A PARTIR DE LA FECHA ACTUAL
                        'If pbValidaNdiasHabiles Then
                        '    Dim liNumDias As Integer = RecuperaParametroDiasFecha()
                        '    Dim ldFechaEnviaVj As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDias)

                        '    If ldFecha.Date < ldFechaEnviaVj.Date Then
                        '        imgImagenError.ImageUrl = Constantes.Imagenes.Fallo
                        '        Dim errores As New Entities.EtiquetaError(2162)
                        '        lblFecha.Visible = True
                        '        lblFecha.Text = errores.Descripcion.Replace("[NUM_DIAS]", liNumDias.ToString())

                        '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", psScripAEjecutar, True)
                        '        Return DateTime.MinValue
                        '    End If
                        'End If

                        ''VALIDA FECHA MENOR A LA FECHA DE FINALIZACION DEL PASO SIGUIENTE
                        'If pbValidaFechaFinalizaPasoSig And piPasoSiguiente <> 0 Then
                        '    Dim ldFechafinalizaPaso As DateTime = ObtenerFechaFinalizaPaso(piPasoSiguiente)

                        '    If Not IsNothing(ldFechafinalizaPaso) Then
                        '        If ldFecha.Date > ldFechafinalizaPaso.Date Then
                        '            imgImagenError.ImageUrl = Constantes.Imagenes.Fallo
                        '            Dim errores As New Entities.EtiquetaError(2163)
                        '            lblFecha.Visible = True
                        '            lblFecha.Text = errores.Descripcion.Replace("[FECHA]", ldFechafinalizaPaso.ToString("dd/MM/yyyy"))

                        '            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", psScripAEjecutar, True)
                        '            Return DateTime.MinValue
                        '        End If
                        '    End If
                        'End If
                    End If
                End If
            End If
        End If

        imgImagenError.ImageUrl = Constantes.Imagenes.Aviso
        lblFecha.Visible = False

        Return ldFecha
    End Function

    Private Function RecuperaParametroDiasFecha() As Integer
        Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.RangoFechaValidaPaso9_16)
        Dim liDias As Integer

        ''Conexion.SQLServer
        If dt.Rows.Count > 0 Then
            If Not Int32.TryParse(dt.Rows(0)("T_DSC_VALOR"), liDias) Then
                liDias = 3
            End If
        Else
            liDias = 3
        End If

        Return liDias
    End Function

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

    Private Function MandaCorreoSandraPachecoVulnerabilidad(objNegVisita As NegocioVisita, ByVal idCorreo As Integer, idVisita As Integer) As Boolean
        Dim lstPersonasDestinatarios As New List(Of Persona)
        Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.SandraPacheco)
        Dim objCorreoBD As New Entities.Correo(idCorreo)

        Dim objUsuario As New Entities.Usuario()
        objUsuario = Session(Entities.Usuario.SessionID)

        Dim visita As New Visita()

        If IsNothing(ppObjVisita) Then
            visita = AccesoBD.getDetalleVisita(idVisita, objUsuario.IdArea)
        Else
            visita = ppObjVisita
        End If

        If dt.Rows.Count > 0 Then
            lstPersonasDestinatarios.Add(New Persona With {.Nombre = Constantes.Parametros.SandraPacheco, .Correo = dt.Rows(0)("T_DSC_VALOR")})

            objNegVisita.getObjNotificacion().NotificarCorreo(objCorreoBD, visita,
                                                          True, True, False, lstPersonasDestinatarios, True)
            Return True
        Else
            Me.Mensaje = "No se encontro el parametro con el nombre: " & Constantes.Parametros.SandraPacheco
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Return False
        End If
    End Function

    Protected Sub ddlTipoEntidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoEntidad.SelectedIndexChanged
        If Not (ddlTipoEntidad.SelectedValue <> "" AndAlso ddlTipoEntidad.SelectedValue <> "-1") Then Exit Sub
        Dim usuario As New Entities.Usuario()
        Dim _idTE As Integer = ddlTipoEntidad.SelectedValue

        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session


        Dim dsEntidades As DataSet

        Dim UsuarioWs As String = WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")


        Dim mycredentialCache As CredentialCache = New CredentialCache()
        Dim credentials As NetworkCredential = New NetworkCredential(UsuarioWs, Password, Dominio)
        Dim proxySICOD As New WR_SICOD.ws_SICOD
        proxySICOD.Credentials = credentials
        proxySICOD.ConnectionGroupName = "SEPRIS"
        Dim lstEntidadesSicod As New List(Of Entities.EntidadSicod)

        dplEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))

        dsEntidades = proxySICOD.GetEntidadesComplete(_idTE)

        Dim lstEntidadesOrdenada = From l In lstEntidadesSicod Distinct Order By l.DSC


        dplEntidad.Enabled = False
      If dsEntidades IsNot Nothing And ddlTipoEntidad.SelectedIndex >= 0 Then
         If dsEntidades.Tables(0).Rows.Count > 0 Then
            dplEntidad.DataSource = dsEntidades.Tables(0)
            dplEntidad.DataTextField = "SIGLAS_ENT"
            dplEntidad.DataValueField = "CVE_ID_ENT"
            dplEntidad.DataBind()
            dplEntidad.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
            dplEntidad.Enabled = True
            If ddlTipoEntidad.SelectedIndex < 1 Then
               dplEntidad.Enabled = False
            End If
         End If
      End If
      If _idTE = 1 And usuario.IdArea = 36 Then
            chkSubVisitas.Checked = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MuestraSubVistasCB();", True)
            trSubvisitas.Visible = True
        Else
            chkSubVisitas.Checked = False
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "OcultaSubVistasCB()", True)
            trSubvisitas.Visible = False
        End If

    End Sub
End Class