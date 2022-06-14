Imports System.Web.Configuration
Imports System.Net
Imports Clases
Imports Entities
Imports Utilerias
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Web.Services.Protocols

Public Class RegistroOPI
    Inherits System.Web.UI.Page
    Dim enc As New YourCompany.Utils.Encryption.Encryption64

#Region "Propiedades"
    Public Property lstMensajes As List(Of String)
    Dim subEnt As New List(Of Integer)
    Public Property Mensaje As String
    Public Property Mensaje2 As String

    Dim objOPI As OPI_Incumplimiento
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

    Public Property plstSubentidades() As List(Of Integer)
        Get
            If IsNothing(ViewState("listSubEntidades")) Then
                Return New List(Of Integer)
            Else
                Dim lstET = TryCast(ViewState("listSubEntidades"), List(Of Integer))

                If Not IsNothing(lstET) Then
                    Return lstET
                Else
                    Return New List(Of Integer)
                End If
            End If
        End Get
        Set(ByVal value As List(Of Integer))
            ViewState("listSubEntidades") = value
        End Set
    End Property

    Private _lstSupervisores As Hashtable
    Public Property plstSupervisores() As Hashtable
        Get
            Return _lstSupervisores
        End Get
        Set(ByVal value As Hashtable)
            _lstSupervisores = value
        End Set
    End Property

    Private Shared _lstPcSupervisores As Hashtable
    Public Shared Property pcSupervisores() As Hashtable
        Get
            Return _lstPcSupervisores
        End Get
        Set(ByVal value As Hashtable)
            _lstPcSupervisores = value
        End Set
    End Property

    Private Shared _lstPcInspectores As Hashtable
    Public Shared Property pcInspectores() As Hashtable
        Get
            Return _lstPcInspectores
        End Get
        Set(ByVal value As Hashtable)
            _lstPcInspectores = value
        End Set
    End Property


    Private _lstInspectores As Hashtable
    Public Property plstInspectores() As Hashtable
        Get
            Return _lstInspectores
        End Get
        Set(ByVal value As Hashtable)
            _lstInspectores = value
        End Set
    End Property

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblTitle.Text = "Información"
        If Not IsPostBack Then

            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                ''Guarda el area del usuario en el viewstate
                Dim usuario As New Entities.Usuario()
                usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

                cargadllTiposEntidades(usuario.IdArea)
                cargarEntidadesV2()
                CargadlProceso(usuario.IdArea)
                Dim objAreas As New Entities.Areas(usuario.IdArea)
                objAreas.CargarDatos()


                pgAreaUsuario = objAreas.Descripcion

                ''Valida si es que se esta editando una OPI
                If Not IsNothing(Request.QueryString("up")) Then
                    lnkTituloPag.InnerText = "Editar OPI"

                    ''Valida si se esta editando
                    If Request.QueryString("up").ToString() = "1" Then
                        If Not IsNothing(Session("idOPIEditar")) Then
                            'CargaFormularioParaEditar()
                        End If
                    End If
                Else

                End If

            End If
        Else
            Page.MaintainScrollPositionOnPostBack = True
        End If

    End Sub

    Private Sub cargadllTiposEntidades(_id_area As Integer)

        Dim dsEntidades As WR_SICOD.Diccionario()

        Dim Usuario As String = WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        Try
            Dim mycredentialCache As CredentialCache = New CredentialCache()
            Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
            Dim proxySICOD As New WR_SICOD.ws_SICOD
            Dim list As IEnumerable(Of WR_SICOD.Diccionario) = Nothing
            proxySICOD.Credentials = credentials
            proxySICOD.ConnectionGroupName = "SEPRIS"
            'dplEntidad.Enabled = False


            ddlTipoEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))

            dsEntidades = proxySICOD.GetTipoEntidades

            Select Case _id_area

                Case 35 Or 1
                    list = From p In dsEntidades
                           Where p.Key = 1 _
                           Or p.Key = 7 _
                           Or p.Key = 12
                           Select p

                Case 1
                    list = From p In dsEntidades
                           Where p.Key = 1 _
                           Or p.Key = 7 _
                           Or p.Key = 12
                           Select p
                Case 36
                    ''PCMT: el requerimento solicita SIEFORE pero no la devuelve el catálogo
                    list = From p In dsEntidades
                           Where p.Key = 1 Or p.Key = 2 _
                           Or p.Key = 3 Or p.Key = 4 _
                           Or p.Key = 17 Or p.Key = 7 Or p.Key = 12
                           Select p
            End Select

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




        If usuarioActual.IdArea = 35 Or usuarioActual.IdArea = 1 Then
            '<Value>AFORE</Value>
            '<Value>EMPRESA OPERADORA</Value> 
            vecTipoEntidades.Add(1)
            vecTipoEntidades.Add(7)
            vecTipoEntidades.Add(12)
        Else
            If usuarioActual.IdArea = 36 Then
                '<Value>AFORE</Value>
                '<Value>SB</Value>
                '<Value>SIAC</Value>
                '<Value>SIPS</Value> 
                '<Value>SIAV</Value>
                vecTipoEntidades.Add(1)
                vecTipoEntidades.Add(2)
                vecTipoEntidades.Add(3)
                vecTipoEntidades.Add(4)
                vecTipoEntidades.Add(17)
                vecTipoEntidades.Add(7)
                vecTipoEntidades.Add(12)
            End If
        End If

        Try
            If Not IsNothing(usuarioActual) Then
                Dim mycredentialCache As CredentialCache = New CredentialCache()
                Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
                Dim proxySICOD As New WR_SICOD.ws_SICOD
                proxySICOD.Credentials = credentials
                proxySICOD.ConnectionGroupName = "SEPRIS"
                Dim lstEntidadesSicod As New List(Of Entities.EntidadSicod)

                dplEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))

                For Each ent As Integer In vecTipoEntidades
                    dsEntidades = proxySICOD.GetEntidadesComplete(ent)
                    For Each ltTable As DataTable In dsEntidades.Tables
                        For Each lrRow As DataRow In ltTable.Rows
                            Dim objEntidadSicod As New Entities.EntidadSicod
                            If Int32.TryParse(lrRow("cve_id_ent"), objEntidadSicod.ID) Then
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


                dplEntidad.Enabled = False
                If dsEntidades IsNot Nothing Then
                    If dsEntidades.Tables(0).Rows.Count > 0 Then
                        dplEntidad.DataSource = lstEntidadesOrdenada
                        dplEntidad.DataTextField = "DSC"
                        dplEntidad.DataValueField = "ID"
                        dplEntidad.DataBind()
                        dplEntidad.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
                        'dplEntidad.Enabled = True
                    End If
                End If
            End If
        Catch ex As Exception
            catch_cone(ex, "cargaddlEntidades()")
        End Try
    End Sub

    Public Sub CargadlProceso(_area As Integer)
        dplProcesoPO.DataTextField = "T_DSC_DESCRIPCION" '"DESC_PROCESO"
        dplProcesoPO.DataValueField = "I_ID_PROCESO" '"ID_PROCESO"
        dplProcesoPO.DataSource = ObtenerProcesosVigentes(_area) 'ConexionSISAN.ObtenerProcesos()
        dplProcesoPO.DataBind()
        dplProcesoPO.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
        ddlSubproceso.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
    End Sub


    Public Function ObtenerSubprocesos(Proceso As String) As List(Of ListItem)
        Dim dsSubprocesos As DataSet = Subproceso.ObtenerVigentesPorProceso(Proceso)


        Dim subprocesos As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSubprocesos.Tables(0).Rows
            subprocesos.Add(New ListItem() With {
                          .Value = row("I_ID_SUBPROCESO").ToString(),
                          .Text = row("T_DSC_DESCRIPCION").ToString()})

        Next

        Return subprocesos
    End Function

    Public Shared Function ObtenerProcesosVigentes(Area As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT I_ID_PROCESO, T_DSC_DESCRIPCION FROM [dbo].[BDS_C_GR_PROCESO] WHERE B_FLAG_VIGENTE = 1 AND I_ID_AREA =" + Area.ToString())

        conexion.CerrarConexion()

        Return data

    End Function
    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click

        Select Case btnAceptarM2B1A.CommandArgument

            Case "redirectHome"
                Response.Redirect("../OPI/BandejaOPI.aspx")
            Case "RegistrarOPI"
                GuardarOPI()
                'CorreoNotificacion()
        End Select

    End Sub

    Protected Sub btnAceptarM1B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM1B1A.Click

        Select Case btnAceptarM1B1A.CommandArgument
            Case "Redirect"
                Response.Redirect("../OPI/BandejaOPI.aspx")
            Case Else

        End Select

    End Sub

    Private Sub GuardarOPI()
        Try
            Dim usuario As New Entities.Usuario()
            Dim con As Conexion.SQLServer = Nothing
            Dim tran As SqlClient.SqlTransaction = Nothing
            Dim saveOk As Boolean = True
            Dim _opi_Reg As New Registro_OPI
            Dim strFoliosOPI As String = ""
            usuario = Session(Entities.Usuario.SessionID)

            'If (usuario.IdArea = 36) Then
            '    'ammm corrección de generación de folio cuando se selecciona un SB
            '    If (ddlTipoEntidad.SelectedValue <> 1) Then
            '        If (ddlTipoEntidad.SelectedValue = 12 Or ddlTipoEntidad.SelectedValue = 7) Then
            '            _opi_Reg.T_ID_FOLIO = "/" & pgAreaUsuario & "/" & Date.Now.ToString("MMyy")
            '            _opi_Reg.I_ID_SUBENTIDAD_SB = -1
            '        Else
            '            _opi_Reg.T_ID_FOLIO = "/" & pgAreaUsuario & "/" & Trim(ddlSubentidad.SelectedItem.Text) & "/" & Date.Now.ToString("MMyy")
            '            '_opi_Reg.T_ID_FOLIO = "/OPI/" & pgAreaUsuario & "/" & dplEntidad.SelectedItem.Text & "/" & Date.Now.ToString("MMyy") & "/" & GeneraSubfolio(ddlSubentidad.SelectedItem.Text)
            '            _opi_Reg.I_ID_SUBENTIDAD_SB = ddlSubentidad.SelectedValue
            '        End If
            '    Else
            '        _opi_Reg.T_ID_FOLIO = "/" & pgAreaUsuario & "/" & dplEntidad.SelectedItem.Text & "/" & Date.Now.ToString("MMyy")
            '        _opi_Reg.I_ID_SUBENTIDAD_SB = -1
            '    End If
            'Else
            '    _opi_Reg.T_ID_FOLIO = "/" & pgAreaUsuario & "/" & dplEntidad.SelectedItem.Text & "/" & Date.Now.ToString("MMyy")
            '    _opi_Reg.I_ID_SUBENTIDAD_SB = -1
            'End If

            If (usuario.IdArea = 36) Then
                'ammm corrección de generación de folio cuando se selecciona un SB
                If (ddlSubentidad.Items.Count <> 0) Then
                    _opi_Reg.T_ID_FOLIO = "/" & pgAreaUsuario & "/" & Trim(ddlSubentidad.SelectedItem.Text) & "/" & Date.Now.ToString("MMyy")
                    '_opi_Reg.T_ID_FOLIO = "/OPI/" & pgAreaUsuario & "/" & dplEntidad.SelectedItem.Text & "/" & Date.Now.ToString("MMyy") & "/" & GeneraSubfolio(ddlSubentidad.SelectedItem.Text)
                    _opi_Reg.I_ID_SUBENTIDAD_SB = ddlSubentidad.SelectedValue
                Else
                    _opi_Reg.T_ID_FOLIO = "/" & pgAreaUsuario & "/" & dplEntidad.SelectedItem.Text & "/" & Date.Now.ToString("MMyy")
                    _opi_Reg.I_ID_SUBENTIDAD_SB = -1
                End If
            Else
                _opi_Reg.T_ID_FOLIO = "/" & pgAreaUsuario & "/" & dplEntidad.SelectedItem.Text & "/" & Date.Now.ToString("MMyy")
                _opi_Reg.I_ID_SUBENTIDAD_SB = -1
            End If



            _opi_Reg.N_ID_PASO = 1
            _opi_Reg.N_ID_SUBPASO = -1
            _opi_Reg.I_ID_TIPO_ENTIDAD = Convert.ToInt64(ddlTipoEntidad.SelectedValue)
            _opi_Reg.I_ID_ENTIDAD = Convert.ToInt64(dplEntidad.SelectedValue)
            _opi_Reg.I_ID_SUBENTIDAD = plstSubentidades
            _opi_Reg.F_FECH_POSIBLE_INC = Convert.ToDateTime(txtFechaPI.Text.Trim)
            _opi_Reg.I_ID_PROCESO_POSIBLE_INC = Convert.ToInt64(dplProcesoPO.SelectedValue)
            _opi_Reg.I_ID_SUBPROCESO = Convert.ToInt64(ddlSubproceso.SelectedValue)
            _opi_Reg.T_DSC_POSIBLE_INC = txtDescOPI.Text.Trim()
            Call GetSupervisores()
            Call GetInspectores()
            _opi_Reg.T_ID_SUPERVISORES = plstSupervisores
            _opi_Reg.T_ID_INSPECTORES = plstInspectores
            _opi_Reg.T_OBSERVACIONES_OPI = txtComentariosOPI.Text.Trim()
            _opi_Reg.I_ID_ESTATUS = 1
            _opi_Reg.I_ID_AREA = usuario.IdArea
            _opi_Reg.F_FECH_PASO_ACTUAL = Date.Now

            saveOk = _opi_Reg.Agregar(strFoliosOPI)
            If saveOk Then
                btnAceptarM1B1A.CommandArgument = "Redirect"
                imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                'If subEnt.Count > 0 Then
                If InStr(strFoliosOPI, ",") > 0 Then
                    'lstMensajes.Add("Se ha registrado correctamente el OPI " & _opi_Reg.T_ID_FOLIO)
                    Mensaje = ("Se ha realizado el registro correctamente los Folios asignados son:<br><br> " & strFoliosOPI.Replace(",", "<br>"))
                    'For i = 0 To subEnt.Count - 1
                    '    Dim valor As String = _opi_Reg.T_ID_FOLIO & "/SB" & subEnt(i).ToString()
                    '    lstMensajes.Add(valor)
                    'Next

                    'For Each lis As String In lstMensajes
                    '    Dim valorx = (lis)
                    '    Mensaje += valorx + "</br>"
                    'Next

                Else

                    Mensaje = ("Se ha realizado el registro correctamente el Folio asignado es:  " & _opi_Reg.T_ID_FOLIO)
                End If

                lblTitle.Text = "Registro correcto"
                objOPI = New OPI_Incumplimiento()
                objOPI = _opi_Reg
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
            End If


        Catch ex As Exception

            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgError
            lblTitle.Text = "Error!"
            Mensaje = "Ha ocurrido un error al tratar de guardar el registro"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error al guardar", "AquiMuestroMensaje();", True)
        End Try
    End Sub


    Public Shared Function GuardarOPIfromPC(Folio As Integer, PCData As Entities.PC, ByRef FolioOPI As String) As Boolean
        Try

            Dim usuario As New Entities.Usuario()
            Dim con As Conexion.SQLServer = Nothing
            Dim tran As SqlClient.SqlTransaction = Nothing
            Dim saveOk As Boolean = True
            Dim _opi_Reg As New Registro_OPI

            _opi_Reg.T_ID_FOLIO = "/" & PCData.FolioSupervisar.Split("/")(2) & "/" & PCData.FolioSupervisar.Split("/")(3) & "/" & Date.Now.ToString("MMyy")
            _opi_Reg.N_ID_PASO = 1
            _opi_Reg.N_ID_SUBPASO = -1
            _opi_Reg.I_ID_TIPO_ENTIDAD = 1
            _opi_Reg.I_ID_ENTIDAD = PCData.IdEntidad
            Dim pcSubent As List(Of Integer) = GetSubEntidadesFromPC(Folio)
            _opi_Reg.I_ID_SUBENTIDAD = pcSubent
            _opi_Reg.F_FECH_POSIBLE_INC = Date.Now
            _opi_Reg.I_ID_PROCESO_POSIBLE_INC = PCData.IdProceso
            _opi_Reg.I_ID_SUBPROCESO = PCData.IdSubproceso

            GetSupervisoresFromPC(Folio)
            GetInspectoresFromPC(Folio)
            _opi_Reg.T_ID_SUPERVISORES = pcSupervisores
            _opi_Reg.T_ID_INSPECTORES = pcInspectores

            _opi_Reg.T_DSC_POSIBLE_INC = PCData.Descripcion
            _opi_Reg.T_OBSERVACIONES_OPI = "Registro proveniente del PC: " & PCData.FolioSupervisar
            _opi_Reg.I_ID_ESTATUS = 1
            _opi_Reg.I_ID_AREA = PCData.IdArea
            _opi_Reg.F_FECH_PASO_ACTUAL = Date.Now

            Return _opi_Reg.AgregarOPI(FolioOPI)

        Catch ex As Exception
        End Try
    End Function

    Protected Sub imgRegistrar_Click(sender As Object, e As ImageClickEventArgs) Handles imgRegistrar.Click
        If verifica_datos() Then
            btnAceptarM2B1A.CommandArgument = "RegistrarOPI"
            lblTitle.Text = "Confirmación"
            Mensaje = "¿Estás seguro que deseas completar el registro?"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

        ElseIf Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            lblTitle.Text = "Alerta - Información Incompleta"
            'divMensajeUnBotonNoAccion.Attributes("title") = Title
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "AquiMuestroMensaje();", True)

        End If
    End Sub

    Public Function verifica_datos() As Boolean

        Dim sinError As Boolean = True
        lstMensajes = New List(Of String)

        If (txtDescOPI.Text = "" Or txtDescOPI.Text = String.Empty) Then
            sinError = False
            lstMensajes.Add("Debe capturar una descripción para el Oficio")
        End If

        If (AddSubEntidades.Checked And ddlTipoEntidad.SelectedValue = 1) Then
            If (chkSubEntidad.SelectedValue.Count = 0) Then
                sinError = False
                lstMensajes.Add("Debe seleccionar por lo menos una sub entidad")
            End If
        End If

        If ddlTipoEntidad.SelectedValue = "-1" Or String.IsNullOrEmpty(ddlTipoEntidad.SelectedValue) Then
            Dim errores As New Entities.EtiquetaError(2127)
            lstMensajes.Add(errores.Descripcion)
            sinError = False
            ast.Attributes.Remove("class")
            ast.Attributes.Add("class", "AsteriscoShow")
        Else
            ast.Attributes.Remove("class")
            ast.Attributes.Add("class", "AsteriscoHide")
        End If

        If dplEntidad.SelectedValue = "-1" Or String.IsNullOrEmpty(dplEntidad.SelectedValue) Then
            Dim errores As New Entities.EtiquetaError(2117)
            lstMensajes.Add(errores.Descripcion)
            sinError = False
            ast1.Attributes.Remove("class")
            ast1.Attributes.Add("class", "AsteriscoShow")
        Else
            ast1.Attributes.Remove("class")
            ast1.Attributes.Add("class", "AsteriscoHide")
        End If

        'If ddlSubentidad.SelectedValue = "-1" Or ddlSubentidad.SelectedValue = "" Then
        '    Dim errores As New Entities.EtiquetaError(2134)
        '    lstMensajes.Add(errores.Descripcion)
        '    sinError = False
        '    ast2.Attributes.Remove("class")
        '    ast2.Attributes.Add("class", "AsteriscoShow")
        'Else
        '    ast2.Attributes.Remove("class")
        '    ast2.Attributes.Add("class", "AsteriscoHide")
        'End If

        If txtFechaPI.Text.Trim() = String.Empty Then
            'Dim errores As New Entities.EtiquetaError(2149)
            lstMensajes.Add("Debe ingresar la fecha del oficio") '(errores.Descripcion)
            sinError = False
            ast3.Attributes.Remove("class")
            ast3.Attributes.Add("class", "AsteriscoShow")
        Else
            ast3.Attributes.Remove("class")
            ast3.Attributes.Add("class", "AsteriscoHide")
        End If

        If dplProcesoPO.SelectedValue = "-1" Or String.IsNullOrEmpty(dplProcesoPO.SelectedValue) Then
            ''Dim errores As New Entities.EtiquetaError(2118)
            lstMensajes.Add("Debe seleccionar un proceso")
            sinError = False
            ast4.Attributes.Remove("class")
            ast4.Attributes.Add("class", "AsteriscoShow")
        Else
            ast4.Attributes.Remove("class")
            ast4.Attributes.Add("class", "AsteriscoHide")
        End If

        If ddlSubproceso.SelectedValue = "-1" Or String.IsNullOrEmpty(ddlSubproceso.SelectedValue) Then
            lstMensajes.Add("Debe seleccionar un Subproceso")
            sinError = False
            ast5.Attributes.Remove("class")
            ast5.Attributes.Add("class", "AsteriscoShow")
        Else
            ast5.Attributes.Remove("class")
            ast5.Attributes.Add("class", "AsteriscoHide")
        End If



        'If txtDescOPI.Text.Trim() = String.Empty Then
        '    Dim errores As New Entities.EtiquetaError(2135)
        '    lstMensajes.Add(errores.Descripcion)
        '    sinError = False
        '    ast11.Attributes.Remove("class")
        '    ast11.Attributes.Add("class", "AsteriscoShow")
        'Else
        '    ast11.Attributes.Remove("class")
        '    ast11.Attributes.Add("class", "AsteriscoHide")
        'End If

        'If txtComentariosOPI.Text.Trim() = String.Empty Then
        '    Dim errores As New Entities.EtiquetaError(2121)
        '    lstMensajes.Add(errores.Descripcion)
        '    sinError = False
        '    ast8.Attributes.Remove("class")
        '    ast8.Attributes.Add("class", "AsteriscoShow")
        'Else
        '    ast8.Attributes.Remove("class")
        '    ast8.Attributes.Add("class", "AsteriscoHide")
        'End If

        ''Valida si se ha solicitado incluir subentidades
        If (AddSubEntidades.Checked And ddlTipoEntidad.SelectedValue = 1) Then GetSubentidadescheck()


        If Not GetSupervisores() Then
            '    Dim errores As New Entities.EtiquetaError(2121)
            lstMensajes.Add("Debe de seleccionar al menos un supervisor")
            sinError = False
            'ast12.Attributes.Remove("class")
            'ast12.Attributes.Add("class", "AsteriscoShow")
        Else
            'ast12.Attributes.Remove("class")
            'ast12.Attributes.Add("class", "AsteriscoHide")
        End If

        If Not GetInspectores() Then
            '    Dim errores As New Entities.EtiquetaError(2121)
            lstMensajes.Add("Debe de seleccionar al menos un Inspector")
            sinError = False
            'ast14.Attributes.Remove("class")
            'ast14.Attributes.Add("class", "AsteriscoShow")
        Else
            'ast14.Attributes.Remove("class")
            'ast14.Attributes.Add("class", "AsteriscoHide")
        End If

        Return sinError
    End Function

    Protected Sub imgInicio_Click(sender As Object, e As ImageClickEventArgs) Handles imgInicio.Click

        btnAceptarM2B1A.CommandArgument = "redirectHome"
        lblTitle.Text = "Confirmación"
        Mensaje = "Al abandonar la pantalla perderás los datos  ingresados, ¿Estas seguro que deseas cancelar el registro?"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Private Sub catch_cone(ByVal e As Exception, ByVal s As String)
        EventLogWriter.EscribeEntrada("Funcion " & s & ": " & e.ToString(), EventLogEntryType.Error)
    End Sub


    Protected Sub dplEntidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dplEntidad.SelectedIndexChanged
        'FSW CAGC SOFTTEK Correccion de incidencia 38, Documento 38
        If dplEntidad.SelectedValue = 1 Or dplEntidad.Text = "PROCESAR" Then
            trSubentidad.Visible = False
        Else
            trSubentidad.Visible = True
        End If

        chkSubEntidad.Visible = True

        'If (dplEntidad.SelectedValue = "1") Then
        '    Return
        'End If

        'Verifica que se haya seleccionado un elemento en el combobox
        If ((dplEntidad.SelectedValue = "" Or dplEntidad.SelectedValue = "-1")) Then
            chkSubEntidad.Items.Clear()
            ddlSubentidad.Items.Clear()
            chkSubEntidad.Visible = False
            trSubentidad.Visible = False
            AddSubEntidades.Visible = False
            AddSubEntidades.Checked = False

            Exit Sub
        End If

        Dim usuarioActual As New Entities.Usuario()
        usuarioActual = Session(Entities.Usuario.SessionID) ' se obtiene de la session
        Dim dsSubEntidades As New DataSet
        Dim Usuario As String = WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        If Not IsNothing(usuarioActual) Then

            Dim mycredentialCache As CredentialCache = New CredentialCache()
            Dim credentials As NetworkCredential = New NetworkCredential(Usuario, Password, Dominio)
            Dim proxySICOD As New WR_SICOD.ws_SICOD
            proxySICOD.Credentials = credentials
            proxySICOD.ConnectionGroupName = "SEPRIS"
            Dim lsIdEntidad As Integer

            lsIdEntidad = dplEntidad.SelectedValue


            'dsSubEntidades = proxySICOD.GetEntidadesComplete(1)
            'dsSubEntidades = proxySICOD.GetSubEntidadesComplete(lsIdEntidad)
            'Dim dtEntidades As DataTable = ConexionSICOD.ObtenerSubEntidadesAFORE(lsIdEntidad) / AMMM 120719- se omenta y se pasa dentro del If
            'dtSubentidades = dsSubEntidades.Tables(0)


            If (usuarioActual.IdArea = Constantes.AREA_VF AndAlso usuarioActual.IdentificadorPerfilActual = Constantes.PERFIL_SUP Or usuarioActual.IdentificadorPerfilActual = Constantes.PERFIL_INS Or usuarioActual.IdentificadorPerfilActual = Constantes.PERFIL_ADM) Then



                If (ddlTipoEntidad.SelectedValue = 7 Or ddlTipoEntidad.SelectedValue = 1 Or ddlTipoEntidad.SelectedValue = 12) Then

                    If trSubentidad.Visible = True Then
                        Dim dtEntidades As DataTable = ConexionSICOD.ObtenerSubEntidadesAFORE(lsIdEntidad)
                        'Dim dtEntidades As DataTable = ConexionSICOD.ObtenerSubEntidadesAFORECompletas(lsIdEntidad)

                        AddSubEntidades.Visible = True

                        If dsSubEntidades IsNot Nothing Then
                            If dtEntidades.Rows.Count > 0 Then
                                chkSubEntidad.DataSource = dtEntidades
                                chkSubEntidad.DataTextField = "SGL_SUBENT" '"DSC_SUBENT" 
                                chkSubEntidad.DataValueField = "ID_SUBENT"
                                chkSubEntidad.DataBind()
                            Else
                                chkSubEntidad.Items.Clear()
                                AddSubEntidades.Visible = False
                            End If
                        Else
                            chkSubEntidad.Items.Clear()
                        End If
                    End If
                End If



                If trSubentidad1.Visible = True Then
                    Dim dtEntidades As DataTable = ConexionSICOD.ObtenerSubEntidadesAFORE(lsIdEntidad)
                    If dsSubEntidades IsNot Nothing Then
                        If dtEntidades.Rows.Count > 0 Then
                            ddlSubentidad.DataSource = dtEntidades
                            ddlSubentidad.DataTextField = "SGL_SUBENT" '"DSC_SUBENT"
                            ddlSubentidad.DataValueField = "ID_SUBENT"
                            ddlSubentidad.DataBind()
                        Else
                            ddlSubentidad.Items.Clear()
                        End If
                    Else
                        ddlSubentidad.Items.Clear()
                    End If
                Else
                    ddlSubentidad.Items.Clear()
                End If
            End If




        End If

        If (AddSubEntidades.Checked And dplEntidad.SelectedValue <> 1) Then

            trSubentidad.Visible = True
            chkSubEntidad.Visible = True
            chkSubEntidad.ClearSelection()
        Else
            trSubentidad.Visible = False
            chkSubEntidad.Visible = False
            chkSubEntidad.ClearSelection()
        End If

        If (usuarioActual.IdArea = 35 Or usuarioActual.IdArea = 1) Then
            trSubentidad.Visible = False
            chkSubEntidad.Visible = False
            chkSubEntidad.ClearSelection()
        End If

    End Sub

    Private Shared Function GetSubEntidadesFromPC(Folio As Integer) As List(Of Integer)
        Dim SubEntidadesRel As DataSet = Entities.Entidad.ObtenerPorFolio(Folio)

        Dim subentidades As New List(Of Integer)

        For Each row As Data.DataRow In SubEntidadesRel.Tables(0).Rows
            subentidades.Add(CInt(row("N_ID_SUB_ENTIDAD").ToString()))
        Next

        If subentidades.Count() = 0 Then Exit Function

        Return subentidades
    End Function

    Private Sub GetSubentidadescheck()
        Dim objSubentidad As Integer 'SubEntidad
        Dim _opi_Reg As New Registro_OPI
        Dim _lstSubentidades As New List(Of Integer) 'SubEntidad)

        ''Barremos el checklist para obtener los que estan seleccionados

        For i As Integer = 0 To chkSubEntidad.Items.Count - 1
            If chkSubEntidad.Items(i).Selected Then

                objSubentidad = chkSubEntidad.Items(i).Value

                _lstSubentidades.Add(objSubentidad)
                subEnt.Add(objSubentidad)

                objSubentidad = Nothing
            End If
        Next

        plstSubentidades = _lstSubentidades
        _opi_Reg.I_ID_SUBENTIDAD = plstSubentidades
    End Sub

    Private Shared Function GetSupervisoresFromPC(Folio As Integer)
        Dim SupervisoresListAsignado As List(Of ListItem) = Supervisor1.ObtenerSupervisoresFolioPC(Folio)

        If SupervisoresListAsignado.Count() = 0 Then Exit Function
        Dim lstPcSupervisores As New Hashtable

        For Each item In SupervisoresListAsignado
            lstPcSupervisores.Add(item.Value, item.Text)
        Next

        pcSupervisores = lstPcSupervisores
    End Function

    Function GetSupervisores() As Boolean

        If SupervisorOPI.GetSupervisoresSeleccionados().Count() = 0 Then Exit Function
        'Dim lstSupervisores As New List(Of String)
        Dim lstSupervisores As New Hashtable

        For Each item In SupervisorOPI.GetSupervisoresSeleccionados()
            'lstSupervisores.Add(item.Value)
            lstSupervisores.Add(item.Value, item.Text)
        Next

        plstSupervisores = lstSupervisores


        Return True
    End Function

    Private Shared Function GetInspectoresFromPC(Folio As Integer)
        Dim InspectoresListAsignado As List(Of ListItem) = Inspector.ObtenerInspectoresFolioPC(Folio)
        If InspectoresListAsignado.Count() = 0 Then Exit Function
        'Dim lstInspectores As New List(Of String)
        Dim lstPcInspectores As New Hashtable

        For Each item In InspectoresListAsignado
            'lstInspectores.Add(item.Value)
            lstPcInspectores.Add(item.Value, item.Text)
        Next

        pcInspectores = lstPcInspectores
    End Function

    Function GetInspectores()

        If InspectoresOPI.GetInspectoresSeleccionados().Count() = 0 Then Exit Function
        'Dim lstInspectores As New List(Of String)
        Dim lstInspectores As New Hashtable

        For Each item In InspectoresOPI.GetInspectoresSeleccionados()
            'lstInspectores.Add(item.Value)
            lstInspectores.Add(item.Value, item.Text)
        Next

        plstInspectores = lstInspectores

        Return True
    End Function

    Protected Sub AddSubEntidades_CheckedChanged(sender As Object, e As EventArgs) Handles AddSubEntidades.CheckedChanged
        If (AddSubEntidades.Checked) Then
            trSubentidad.Visible = True
            chkSubEntidad.Visible = True
            chkSubEntidad.ClearSelection()
        Else
            trSubentidad.Visible = False
            chkSubEntidad.Visible = False
            chkSubEntidad.ClearSelection()
        End If
    End Sub

    Protected Sub dplProcesoPO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dplProcesoPO.SelectedIndexChanged

        If Not (dplProcesoPO.SelectedValue <> "" AndAlso dplProcesoPO.SelectedValue <> "-1") Then Exit Sub

        ddlSubproceso.Items.Clear()
        SupervisorOPI.ClearList()
        InspectoresOPI.ClearList()

        ddlSubproceso.DataSource = ObtenerSubprocesos(dplProcesoPO.SelectedValue)
        ddlSubproceso.DataTextField = "text"
        ddlSubproceso.DataValueField = "value"
        ddlSubproceso.DataBind()

        If ddlSubproceso.Items.Count() <= 0 Then
            ddlTipoEntidad.Items.Insert(0, New ListItem("-No hay opciones-", "-1"))
        Else
            ddlSubproceso.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
        End If

    End Sub

    Protected Sub ddlSubproceso_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSubproceso.SelectedIndexChanged

        If Not (ddlSubproceso.SelectedValue <> "" AndAlso ddlSubproceso.SelectedValue <> "-1") Then Exit Sub

        SupervisorOPI.Id_Proceso = dplProcesoPO.SelectedValue
        SupervisorOPI.I_ID_SUBPROCESO = ddlSubproceso.SelectedValue
        SupervisorOPI.RefreshList()

        InspectoresOPI.Id_Proceso = dplProcesoPO.SelectedValue
        InspectoresOPI.I_ID_SUBPROCESO = ddlSubproceso.SelectedValue
        InspectoresOPI.RefreshList()

    End Sub

    Protected Sub ddlTipoEntidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoEntidad.SelectedIndexChanged

        If (ddlTipoEntidad.SelectedValue <> "" AndAlso ddlTipoEntidad.SelectedValue <> "-1") Then
            Dim usuario As New Entities.Usuario()
            Dim _idTE As Integer = ddlTipoEntidad.SelectedValue

            usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

            AddSubEntidades.Visible = False
            'FSW RLZ SOFTTEK Inicia Correccion de incidencia 48, Documento 38 
            trSubentidad.Visible = False
            trSubentidad1.Visible = False

            If usuario.IdArea = 36 Then

                If _idTE = 1 Then
                    AddSubEntidades.Visible = False
                ElseIf _idTE = 2 Or _idTE = 3 Or _idTE = 4 Or _idTE = 17 Then
                    AddSubEntidades.Visible = False
                    trSubentidad1.Visible = True

                End If

                'FSW RLZ SOFTTEK Termina Correccion de incidencia 48, Documento 38 



            End If

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


            If ((usuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) Or (usuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM) Or (usuario.IdentificadorPerfilActual = Constantes.PERFIL_INS)) Then
                'dplEntidad.Enabled = False
                'If dsEntidades IsNot Nothing Then
                If dsEntidades.Tables(0).Rows.Count > 0 Then
                    dplEntidad.DataSource = dsEntidades.Tables(0)
                    dplEntidad.DataTextField = "SIGLAS_ENT"
                    dplEntidad.DataValueField = "CVE_ID_ENT"
                    dplEntidad.DataBind()
                    dplEntidad.Items.Insert(0, New ListItem("- Seleccione una opción -", "-1"))
                    dplEntidad.Enabled = True
                    '    End If
                    dplEntidad_SelectedIndexChanged(vbNull, EventArgs.Empty)
                End If
            End If

        ElseIf (ddlTipoEntidad.SelectedValue = "" Or ddlTipoEntidad.SelectedValue = "-1") Then
            dplEntidad.Enabled = False
            dplEntidad.DataSource = ""
            dplEntidad.ClearSelection()
            dplEntidad_SelectedIndexChanged(vbNull, EventArgs.Empty)
        End If


    End Sub


    Protected Sub chkSubEntidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSubEntidad.SelectedIndexChanged
    End Sub

    Public Shared Function ObtenerDiasVencimiento() As Integer
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT T_DSC_VALOR FROM DBO.BDS_C_GR_PARAMETRO WHERE N_ID_PARAMETRO = '82'")

        conexion.CerrarConexion()


        Return Integer.Parse(data.Rows(0)("T_DSC_VALOR").ToString())

    End Function

    Public Shared Function GeneraSubfolio(Subentidad As String)

        Dim SiglasSubentidad As String = ""
        Dim caracter As String

        ''SiglasSubentidad = Subentidad.Substring(Subentidad.Length - 6, )

        ''For i = 0 To SiglasSubentidad.Length - 1
        'caracter = Subentidad.Substring(Subentidad.Length - 6, 1)
        'If caracter = "(" Then
        '    SiglasSubentidad = Subentidad.Substring(Subentidad.Length - 5, 4)
        'Else
        '    SiglasSubentidad = Subentidad.Substring(Subentidad.Length - 4, 3)
        'End If
        ''Next

        Dim i As Integer
        Dim J As Integer = Len(Subentidad)
        Dim inicia As Integer = 0
        Dim caracteres As Integer = 0

        For i = 0 To J
            caracter = Subentidad.Substring(i, 1)
            If caracter = "(" Then
                inicia = i + 1
            End If
            If caracter = ")" And inicia <= 0 Then
                SiglasSubentidad = Subentidad.Substring(inicia, caracteres)
            Else

                caracteres = caracteres + 1
            End If


        Next


        Return SiglasSubentidad

    End Function

End Class