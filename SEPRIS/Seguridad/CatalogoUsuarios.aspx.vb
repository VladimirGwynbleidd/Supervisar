'- Fecha de modificación:  15/11/1013
'- Nombre del Responsable: Rafael Rodriguez RARS1
'- Empresa: Softtek
'- Se agrego funcionalidad de Fecha de Inicio

Imports System.Web
Imports System.Web.Configuration
Imports System.Globalization


Public Class CatalogoUsuarios
    Inherits System.Web.UI.Page

    Public Property Mensaje As String


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        For argument As Integer = 0 To gvConsulta.Rows.Count - 1
            ClientScript.RegisterForEventValidation(btnConsulta.UniqueID, argument)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            ConfigurarPagina()
            CargarFiltros()
            CargarCatalogo()
            CargarImagenesEstatus()

        End If

    End Sub

    Private Sub ConfigurarPagina()
        Select Case Conexion.SQLServer.Parametro.ObtenerValor("TipoLogin")
            Case "Interno"

                pnlControlesUsuarioInterno.Visible = True

            Case "Externo"

                pnlControles.Visible = True

        End Select
    End Sub

    Private Sub CargarImagenesEstatus()

        imgOK.ImageUrl = ObtenerImagenEstatus(True)
        imgERROR.ImageUrl = ObtenerImagenEstatus(False)

    End Sub

    Private Sub CargarFiltros()

        Dim objUsuario As Entities.Usuario = Nothing
        If Not IsNothing(TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)) Then
            objUsuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario)
        End If

        Dim dtDatosFiltro As DataSet

        If Not IsNothing(objUsuario) Then
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.CatalgogoUsuarios, objUsuario.IdentificadorUsuario, objUsuario.IdArea)
        Else
            dtDatosFiltro = AccesoBD.ObtenerDatosFiltro(Constantes.Pantalla.CatalgogoUsuarios, "", 0)
        End If

        ucFiltro1.resetSession()
        ucFiltro1.AddFilter("Vigencia  ", ucFiltro.AcceptedControls.DropDownList, Utilerias.Generales.VigenciaDataSource, "Vigencia", "N_FLAG_VIG", ucFiltro.DataValueType.BoolType, False, True, False, True, True, -1)
        'ucFiltro1.AddFilter("Usuario", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_ID_USUARIO", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 16)
        ucFiltro1.AddFilter("Usuario", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(0), "T_ID_USUARIO", "T_ID_USUARIO", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Nombre(s)", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_NOMBRE", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 100)
        ucFiltro1.AddFilter("Nombre(s)", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(0), "NOMBRE_COMPLETO", "T_ID_USUARIO", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Apellido Paterno", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_APELLIDO", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 100)
        ucFiltro1.AddFilter("Apellido Paterno", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(1), "T_DSC_APELLIDO", "T_DSC_APELLIDO", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Apellido Materno", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_APELLIDO_AUX", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 100)
        ucFiltro1.AddFilter("Apellido Materno", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(2), "T_DSC_APELLIDO_AUX", "T_DSC_APELLIDO_AUX", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("Teléfono", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_TELEFONO", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 15)
        ucFiltro1.AddFilter("Teléfono", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(3), "T_DSC_TELEFONO", "T_DSC_TELEFONO", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

        'ucFiltro1.AddFilter("E-mail", ucFiltro.AcceptedControls.TextBox, Nothing, "", "T_DSC_MAIL", ucFiltro.DataValueType.StringType, False, True)
        ucFiltro1.AddFilter("E-mail", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(4), "T_DSC_MAIL", "T_DSC_MAIL", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

      If objUsuario.IdentificadorPerfilActual = 1 And objUsuario.IdArea = Constantes.AREA_PR Then
         ucFiltro1.AddFilter("Área", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(5), "T_DSC_AREA", "T_DSC_AREA", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)
      End If

      ucFiltro1.AddFilter("Perfil", ucFiltro.AcceptedControls.DropDownList, dtDatosFiltro.Tables(6), "T_DSC_PERFIL", "T_DSC_PERFIL", ucFiltro.DataValueType.StringType, False, False, False, , False, "", 50)

      'NHM Inicia
      'Dim tipo1 = New With {Key .B_FLAG_ES_INGENIERO = 1, .descripcion = "Si"}
      'Dim tipo2 = New With {Key .B_FLAG_ES_INGENIERO = 0, .descripcion = "No"}
      'Dim lstIngeniero = New With {tipo1, tipo2}
      'ucFiltro1.AddFilter("Es ingeniero", ucFiltro.AcceptedControls.CheckBox, lstIngeniero, "descripcion", "B_FLAG_ES_INGENIERO", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, )

      'Dim tipo3 = New With {Key .B_FLAG_ES_AUTORIZADOR = 1, .descripcion = "Si"}
      'Dim tipo4 = New With {Key .B_FLAG_ES_AUTORIZADOR = 0, .descripcion = "No"}
      'Dim lstAutorizador = New With {tipo3, tipo4}
      'ucFiltro1.AddFilter("Es Autorizador", ucFiltro.AcceptedControls.CheckBox, lstAutorizador, "descripcion", "B_FLAG_ES_AUTORIZADOR", ucFiltro.DataValueType.IntegerType, False, False, False, False, False, )
      'NHM fin

      ucFiltro1.LoadDDL("CatalogoUsuario.aspx")


   End Sub

    Private Sub CargarCatalogo()
        Dim usuario As Entities.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
        Dim dv As DataView

        Dim consulta As String = "1=1"

        If Not IsNothing(usuario) Then
            If Constantes.EsAreaSeprisSnPrec(usuario.IdArea) Then
                dv = usuario.ObtenerTodosPorArea(usuario.IdArea)
            Else
                dv = usuario.ObtenerTodos
            End If
        Else
            dv = usuario.ObtenerTodos
        End If

        For Each filtro In ucFiltro1.getFilterSelection
            consulta += " AND " + filtro
        Next

        dv.RowFilter = consulta

        gvConsulta.DataSourceSession = dv.ToTable
        gvConsulta.DataSource = dv.ToTable
        gvConsulta.DataBind()
        MuestraGridViewImagen()
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click

        Page.Validate("Forma")

        If Not Page.IsValid Then

            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub

        End If

        btnAceptarM2B1A.CommandArgument = "btnAceptar"

        If txtUsuario.Text = String.Empty Then
            txtUsuario.Text = txtUsuarioInterno.Text
        End If

        Dim usuario As New Entities.Usuario(txtUsuario.Text)

        If Not usuario.Existe Then
            Dim errores As New Entities.EtiquetaError(93)
            Mensaje = errores.Descripcion
            'imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        Else
            Dim errores As New Entities.EtiquetaError(94)
            Mensaje = errores.Descripcion
            'imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub

    Protected Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ucFiltro1.Filtrar
        CargarCatalogo()
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click

        lblTituloRegistro.Text = "Alta de Usuario"

        Dim dvPerfiles As DataView = New Entities.Perfil().ObtenerTodos()
        dvPerfiles.RowFilter = "N_FLAG_VIG = 1"

        'NHM Inicia
        Dim dvAreas As DataView = New Entities.Areas().ObtenerTodos()

        Dim usuario As Entities.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
        If Not IsNothing(usuario) Then
            If Constantes.EsAreaSeprisSnPrec(usuario.IdArea) Then
                dvAreas.RowFilter = "B_FLAG_VIGENTE = 1 AND I_ID_AREA = " & usuario.IdArea
            Else
                dvAreas.RowFilter = "B_FLAG_VIGENTE = 1"
            End If
        End If
        'NHM Fin



        Select Case Conexion.SQLServer.Parametro.ObtenerValor("TipoLogin")
            Case "Interno"
                ddlNombre.Visible = True
                txtNombreMod.Visible = False
                Utilerias.Generales.CargarComboOrdenado(ddlNombre, Conexion.ActiveDirectory.ObtenerUsuarios(Entities.Usuario.ObtenerTodosFiltro()), "Value", "Key")
                txtUsuarioInterno.Text = String.Empty
                txtFechaIngreso.Text = String.Empty
                txtRFC.Enabled = True
                txtRFC.Text = String.Empty
                Utilerias.Generales.CargarCombo(ddlPerfilInterno, dvPerfiles.ToTable(), "T_DSC_PERFIL", "N_ID_PERFIL")
                'NHM inicia
                Utilerias.Generales.CargarCombo(ddlArea, dvAreas.ToTable(), "T_DSC_AREA", "I_ID_AREA")
                'NHM fin
                chkActualizarContrasena.Checked = True
                trActualizarContrasena.Visible = False
                EnabledCtrls(True)
            Case "Externo"

                txtNombre.Text = String.Empty
                txtApellidoPaterno.Text = String.Empty
                txtApellidoMaterno.Text = String.Empty
                txtTelefono.Text = String.Empty
                txtEmail.Text = String.Empty
                txtUsuario.Text = String.Empty
                Utilerias.Generales.CargarCombo(ddlPerfil, dvPerfiles.ToTable(), "T_DSC_PERFIL", "N_ID_PERFIL")
                'NHM inicia
                Utilerias.Generales.CargarCombo(ddlArea, dvAreas.ToTable(), "T_DSC_AREA", "I_ID_AREA")
                'NHM fin

                chkActualizarContrasena.Checked = True
                trActualizarContrasena.Visible = False

                txtUsuario.CssClass = "txt_gral"
                txtUsuario.Enabled = True
                csvUsuario.Enabled = True

        End Select


        pnlRegistro.Visible = True
        pnlConsulta.Visible = False

    End Sub

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
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
        End If

        Return haySeleccion

    End Function

    Protected Sub btnModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnModificar.Click

        lblTituloRegistro.Text = "Modificación de Usuario"

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If

        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim usuario As New Entities.Usuario(gvConsulta.DataKeys(row.RowIndex)("T_ID_USUARIO").ToString())

                If Not usuario.Vigente Then
                    Dim errores As New Entities.EtiquetaError(98)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If

                Dim dvPerfiles As DataView = New Entities.Perfil().ObtenerTodos()
                dvPerfiles.RowFilter = "N_FLAG_VIG = 1"

                'NHM Inicia
                Dim dvAreas As DataView = New Entities.Areas().ObtenerTodos()
                Dim objUsuario As Entities.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
                If Not IsNothing(objUsuario) Then
                    If Constantes.EsAreaSeprisSnPrec(objUsuario.IdArea) Then
                        dvAreas.RowFilter = "B_FLAG_VIGENTE = 1 AND I_ID_AREA = " & objUsuario.IdArea
                    Else
                        dvAreas.RowFilter = "B_FLAG_VIGENTE = 1"
                    End If
                End If
                'NHM Fin

                Select Case Conexion.SQLServer.Parametro.ObtenerValor("TipoLogin")

                    Case "Interno"
                        'ddlNombre.SelectedValue = usuario.IdentificadorUsuario
                        ddlNombre.Visible = False
                        txtNombreMod.Visible = True
                        txtNombreMod.Text = usuario.Nombre + " " + usuario.Apellido + " " + usuario.ApellidoAuxiliar
                        txtUsuarioInterno.Text = usuario.IdentificadorUsuario
                        'txtRFC.Enabled = True
                        'txtRFC.Text = usuario.RFC
                        Utilerias.Generales.CargarCombo(ddlPerfilInterno, dvPerfiles.ToTable(), "T_DSC_PERFIL", "N_ID_PERFIL")
                        ddlPerfilInterno.SelectedValue = usuario.Perfiles(0).IdentificadorPerfil
                        'NHM inicia
                        Utilerias.Generales.CargarCombo(ddlArea, dvAreas.ToTable(), "T_DSC_AREA", "I_ID_AREA")
                        ddlArea.SelectedValue = usuario.IdArea.ToString()
                        'NHM fin
                        txtFechaIngreso.Text = usuario.FechaIngreso.Date.ToString("dd/MM/yyy")
                        'chkEsIngeniero.Checked = usuario.Ingeniero
                        'chkEsAutorizador.Checked = usuario.Autorizador
                        EnabledCtrls(True)
                    Case "Externo"

                        txtNombre.Text = usuario.Nombre
                        txtApellidoPaterno.Text = usuario.Apellido
                        txtApellidoMaterno.Text = usuario.ApellidoAuxiliar
                        txtTelefono.Text = usuario.Telefono
                        txtEmail.Text = usuario.Mail
                        txtUsuario.Text = usuario.IdentificadorUsuario
                        Utilerias.Generales.CargarCombo(ddlPerfil, dvPerfiles.ToTable(), "T_DSC_PERFIL", "N_ID_PERFIL")
                        ddlPerfil.SelectedValue = usuario.Perfiles(0).IdentificadorPerfil
                        'NHM inicia
                        Utilerias.Generales.CargarCombo(ddlArea, dvAreas.ToTable(), "T_DSC_AREA", "I_ID_AREA")
                        ddlArea.SelectedValue = usuario.IdArea.ToString()
                        'NHM fin
                        chkActualizarContrasena.Checked = False
                        trActualizarContrasena.Visible = True

                        txtUsuario.CssClass = "txt_solo_lectura"
                        txtUsuario.Enabled = False
                        csvUsuario.Enabled = False

                End Select



                pnlRegistro.Visible = True
                pnlConsulta.Visible = False

                Exit For

            End If
        Next

    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"

        btnAceptarM2B1A_Click(sender, e)

    End Sub

    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegresar.Click

        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        btnAceptarM2B1A_Click(sender, e)


    End Sub

    Protected Sub btnEliminar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEliminar.Click

        btnAceptarM2B1A.CommandArgument = "btnEliminar"

        If Not HayRegistroSeleccionado() Then
            Exit Sub
        End If
        Dim errores As Entities.EtiquetaError
        For Each row As GridViewRow In gvConsulta.Rows
            Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
            If elemento.Checked Then

                Dim usuario As New Entities.Usuario(gvConsulta.DataKeys(row.RowIndex)("T_ID_USUARIO").ToString())

                If Not usuario.Vigente Then
                    errores = New Entities.EtiquetaError(98)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If


            End If
        Next

        btnAceptarM2B1A_Click(sender, e)
    End Sub

    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click


        Select Case btnAceptarM2B1A.CommandArgument

            Case "btnCancelar"

                pnlControles.Enabled = True

                pnlBotones.Visible = True
                pnlRegresar.Visible = False

                pnlRegistro.Visible = False

                pnlConsulta.Visible = True



            Case "btnAceptar"


                Dim usuario As Entities.Usuario = Nothing

                If pnlControles.Visible Then

                    usuario = New Entities.Usuario(txtUsuario.Text)
                    usuario.Nombre = txtNombre.Text
                    usuario.Apellido = txtApellidoPaterno.Text
                    usuario.ApellidoAuxiliar = txtApellidoMaterno.Text
                    usuario.Telefono = txtTelefono.Text
                    usuario.Mail = txtEmail.Text
                    usuario.Restablecer = chkActualizarContrasena.Checked
                    usuario.Perfiles.Clear()
                    usuario.Perfiles.Add(New Entities.Perfil(ddlPerfil.SelectedValue))
                    usuario.FechaIngreso = Convert.ToDateTime(DateTime.Now)

                End If

                If pnlControlesUsuarioInterno.Visible Then

                    usuario = New Entities.Usuario(txtUsuarioInterno.Text)
                    usuario.Perfiles.Clear()
                    usuario.Perfiles.Add(New Entities.Perfil(ddlPerfilInterno.SelectedValue))
                    usuario.FechaIngreso = Convert.ToDateTime(txtFechaIngreso.Text)
                    'NHM inicio
                    usuario.IdArea = Convert.ToInt32(ddlArea.SelectedItem.Value)
                    usuario.DescArea = ddlArea.SelectedItem.Text
                    'NHM fin

                    'usuario.RFC = txtRFC.Text
                    'usuario.Autorizador = chkEsAutorizador.Checked
                    'usuario.Ingeniero = chkEsIngeniero.Checked
                    usuario.CargarDatosAD()

                End If


                If Not usuario.Existe Then
                    usuario.Agregar()
                Else
                    usuario.Actualizar()
                End If

                If chkActualizarContrasena.Checked Then

                    'usuario.ActualizarContrasena()

                    'SE COMENTA EL SIGUIENTE FRAGMENTO DE CÓDIGO  DEBIDO A QUE CUANDO SE REGISTRA UN USUARIO
                    'NO EXISTEN NOTIFICACIONES VÍA CORREO ELECTRÓNICO NI EXISTE LA RECUPERACIÓN DE CONTRASEÑA
                    'Notificar(usuario)

                End If

                CargarCatalogo()

                btnAceptarM2B1A.CommandArgument = "btnCancelar"
                btnAceptarM2B1A_Click(sender, e)

            Case "btnEliminar"


                For Each row As GridViewRow In gvConsulta.Rows
                    Dim elemento As CheckBox = TryCast(row.FindControl("chkElemento"), CheckBox)
                    If elemento.Checked Then

                        Dim usuario As New Entities.Usuario(gvConsulta.DataKeys(row.RowIndex)("T_ID_USUARIO").ToString())

                        usuario.Baja()

                        If Negocio.Carrusel.EstaENCarrusel(usuario.IdentificadorUsuario) Then
                            Negocio.Carrusel.RecibeChanged(usuario.IdentificadorUsuario, False)
                        End If

                        Exit For

                    End If
                Next

                CargarCatalogo()

        End Select

    End Sub

    Public Sub Notificar(ByVal usuario As Entities.Usuario)

        'SE COMENTA EL SIGUIENTE FRAGMENTO DE CÓDIGO  DEBIDO A QUE CUANDO SE REGISTRA UN USUARIO
        'NO EXISTEN NOTIFICACIONES VÍA CORREO ELECTRÓNICO NI EXISTE LA RECUPERACIÓN DE CONTRASEÑA

        'Dim mail As New Utilerias.Mail()
        'Dim destinatarios As New List(Of String)
        'Dim etiquetas As New Dictionary(Of String, String)

        'etiquetas.Add("<usuario>", usuario.IdentificadorUsuario)
        'etiquetas.Add("<contraseña>", usuario.Contrasena)
        'etiquetas.Add("<correo>", WebConfigurationManager.AppSettings("CorreoAtencion"))

        'Dim template As New Negocio.Correo(0, etiquetas)

        'If (Convert.ToBoolean(WebConfigurationManager.AppSettings("Desarrollo").ToString()) = True) Then
        '    destinatarios.Add("david.perez@softtek.com")
        '    destinatarios.Add("victor.leyva@softtek.com")
        '    destinatarios.Add("ivan.rivera@softtek.com")
        'Else
        '    destinatarios.Add(usuario.Mail)
        'End If


        'If template.EsVigente Then

        '    mail.EsHTML = True
        '    mail.Asunto = template.Asunto
        '    mail.Destinatarios = destinatarios
        '    mail.Mensaje = template.Mensaje
        '    mail.Enviar()

        'Else
        '    Utilerias.ControlErrores.EscribirEvento("No se realizó envío de mail por vigencia", EventLogEntryType.Warning)
        'End If

    End Sub

    Public Function ObtenerImagen(ByVal imagen As String) As String

        Return "~/Imagenes/Errores/" + imagen

    End Function

    Public Function ObtenerImagenEstatus(ByVal estatus As Boolean) As String

        If estatus Then
            Return "~/Imagenes/OK.gif"
        Else
            Return "~/Imagenes/Error.gif"
        End If

    End Function

    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated

        If e.Row.RowType = WebControls.DataControlRowType.DataRow Then

            e.Row.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))

        End If

    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click



        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        'Dim usuario As New Entities.Usuario(gvConsulta.DataKeys(index)("T_ID_USUARIO").ToString())


        'txtNombre.Text = usuario.Nombre
        'txtApellidoPaterno.Text = usuario.Apellido
        'txtApellidoMaterno.Text = usuario.ApellidoAuxiliar
        'txtTelefono.Text = usuario.Telefono
        'txtEmail.Text = usuario.Mail

        Dim usuario As New Entities.Usuario(gvConsulta.DataKeys(index)("T_ID_USUARIO").ToString())

        lblTituloRegistro.Text = "Consulta de Usuario"

        Dim dvPerfiles As DataView = New Entities.Perfil().ObtenerTodos()
        dvPerfiles.RowFilter = "N_FLAG_VIG = 1"

        'NHM Inicia
        Dim dvAreas As DataView = New Entities.Areas().ObtenerTodos()
        Dim objUsuario As Entities.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
        If Not IsNothing(objUsuario) Then
            If Constantes.EsAreaSeprisSnPrec(objUsuario.IdArea) Then
                dvAreas.RowFilter = "B_FLAG_VIGENTE = 1 AND I_ID_AREA = " & objUsuario.IdArea
            Else
                dvAreas.RowFilter = "B_FLAG_VIGENTE = 1"
            End If
        End If
        'NHM Fin

        Select Case Conexion.SQLServer.Parametro.ObtenerValor("TipoLogin")

            Case "Interno"
                'ddlNombre.SelectedValue = usuario.IdentificadorUsuario
                ddlNombre.Visible = False
                txtNombreMod.Visible = True
                txtNombreMod.Text = usuario.Nombre + " " + usuario.Apellido + " " + usuario.ApellidoAuxiliar
                txtUsuarioInterno.Text = usuario.IdentificadorUsuario
                'txtRFC.Text = usuario.RFC
                'txtRFC.Enabled = False
                Utilerias.Generales.CargarCombo(ddlPerfilInterno, dvPerfiles.ToTable(), "T_DSC_PERFIL", "N_ID_PERFIL")
                ddlPerfilInterno.SelectedValue = usuario.Perfiles(0).IdentificadorPerfil
                'NHM inicia
                Utilerias.Generales.CargarCombo(ddlArea, dvAreas.ToTable(), "T_DSC_AREA", "I_ID_AREA")
                ddlArea.SelectedValue = usuario.IdArea.ToString()
                'NHM fin
                txtFechaIngreso.Text = usuario.FechaIngreso.Date.ToString("dd/MM/yyy")
                'chkEsIngeniero.Checked = usuario.Ingeniero
                'chkEsAutorizador.Checked = usuario.Autorizador
                EnabledCtrls(False)
            Case "Externo"

                txtNombre.Text = usuario.Nombre
                txtApellidoPaterno.Text = usuario.Apellido
                txtApellidoMaterno.Text = usuario.ApellidoAuxiliar
                txtTelefono.Text = usuario.Telefono
                txtEmail.Text = usuario.Mail
                txtUsuario.Text = usuario.IdentificadorUsuario
                Utilerias.Generales.CargarCombo(ddlPerfil, dvPerfiles.ToTable(), "T_DSC_PERFIL", "N_ID_PERFIL")
                ddlPerfil.SelectedValue = usuario.Perfiles(0).IdentificadorPerfil
                'NHM inicia
                Utilerias.Generales.CargarCombo(ddlArea, dvAreas.ToTable(), "T_DSC_AREA", "I_ID_AREA")
                ddlArea.SelectedValue = usuario.IdArea.ToString()
                'NHM fin
                chkActualizarContrasena.Checked = False
                trActualizarContrasena.Visible = True

                txtUsuario.CssClass = "txt_solo_lectura"
                txtUsuario.Enabled = False
                csvUsuario.Enabled = False

        End Select

        pnlRegistro.Visible = True
        pnlControles.Enabled = False
        pnlBotones.Visible = False
        pnlRegresar.Visible = True

        pnlConsulta.Visible = False

    End Sub



    Protected Sub ddlNombre_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlNombre.SelectedIndexChanged
        If ddlNombre.SelectedIndex > 0 Then
            txtUsuarioInterno.Text = ddlNombre.SelectedValue
        Else
            txtUsuarioInterno.Text = String.Empty
        End If

    End Sub

    Protected Sub csvNombre_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvNombre.ServerValidate

        If txtNombre.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(33)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False

        End If

    End Sub

    Protected Sub csvApellidoPaterno_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvApellidoPaterno.ServerValidate

        If txtApellidoPaterno.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(34)

            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False

        End If

    End Sub

    Protected Sub csvTelefono_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvTelefono.ServerValidate

        If txtTelefono.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(35)

            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False

        End If

    End Sub

    Protected Sub csvEmail_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvEmail.ServerValidate

        If txtEmail.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(36)

            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub

        End If

        Dim emailRegex As New System.Text.RegularExpressions.Regex("^(?<user>[^@]+)@(?<host>.+)$")
        Dim emailMatch As System.Text.RegularExpressions.Match = emailRegex.Match(txtEmail.Text.Trim())

        If Not emailMatch.Success Then

            Dim errores As New Entities.EtiquetaError(40)

            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub

        End If

    End Sub

    Protected Sub csvPefil_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvPefil.ServerValidate

        If Not ddlPerfil.SelectedIndex > 0 Then
            Dim errores As New Entities.EtiquetaError(37)

            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub

        End If

    End Sub

    Protected Sub csvUsuario_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvUsuario.ServerValidate
        If txtUsuario.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(38)

            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub

        End If

        Dim usuario As New Entities.Usuario(txtUsuario.Text)
        If usuario.Existe Then
            Dim errores As New Entities.EtiquetaError(39)

            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub

        End If



    End Sub

    Protected Sub csvNombreInterno_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvNombreInterno.ServerValidate
        If ddlNombre.Visible Then
            If Not ddlNombre.SelectedIndex > 0 Then
                Dim errores As New Entities.EtiquetaError(33)

                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If
        End If
    End Sub

    Protected Sub csvPerfilInterno_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvPerfilInterno.ServerValidate
        If Not ddlPerfilInterno.SelectedIndex > 0 Then
            Dim errores As New Entities.EtiquetaError(37)

            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub

        End If
    End Sub

    'NHM Inicia
    Protected Sub csvArea_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvArea.ServerValidate
        If Not ddlArea.SelectedIndex > 0 Then
            Dim errores As New Entities.EtiquetaError(2126)

            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub

        End If
    End Sub
    'NHM fin


    Protected Sub csvFechaIngreso_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvFechaIngreso.ServerValidate
        If txtFechaIngreso.Text = "" Then
            Dim errores As New Entities.EtiquetaError(56)

            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        Else
            Dim culture As CultureInfo
            culture = CultureInfo.CreateSpecificCulture("es-MX")
            '---------------------------------------------
            '---------------------------------------------
            Dim styles As DateTimeStyles
            styles = DateTimeStyles.None

            If Not ucFiltro1.FechaValida(txtFechaIngreso.Text, culture, styles) Then
                Dim errores As New Entities.EtiquetaError(57)

                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If
        End If
    End Sub

    Private Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportar.Click

        ' Exportamos a Excel

        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvConsulta.DataSourceSession, DataTable)
        dt.Columns("N_FLAG_VIG").ColumnName = "Estatus"

        utl.ExportaGrid(dt, gvConsulta, "Catálogo de Usuarios", referencias)

    End Sub

    ''' <summary>
    ''' Dependiendo si el GridView no tiene registros muestra una imagen
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraGridViewImagen()
        If gvConsulta.Rows.Count() > 0 Then
            gvConsulta.Visible = True
            pnlNoExiste.Visible = False
        Else
            gvConsulta.Visible = False
            pnlNoExiste.Visible = True
        End If
    End Sub

    Private Sub gvConsulta_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvConsulta.Sorting

        gvConsulta.Ordenar(e)

    End Sub

    Private Sub EnabledCtrls(ByVal flag As Boolean)        
        txtFechaIngreso.Enabled = flag
        imgFec3.Visible = flag
        ddlPerfilInterno.Enabled = flag
        'NHM Inicia
        ddlArea.Enabled = flag
        'NHM Fin
    End Sub

    Function ValidaRFC(ByVal RFC As String) As Boolean
        Dim regExp As New Regex("[A-Z,Ñ,&]{4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]{3}")
        Dim valido = regExp.IsMatch(RFC)
        If valido.Equals(True) Then
            Dim regExpFecha As New Regex("[0-9]{2}")
            Dim fecha = regExpFecha.Matches(RFC)
            Dim Anio = CInt(fecha.Item(0).ToString())
            Dim Mes = CInt(fecha.Item(1).ToString())
            Dim Dia = CInt(fecha.Item(2).ToString())
            valido = ValidaFecha(Anio, Mes, Dia)
        End If
        Return valido
    End Function

    Function ValidaFecha(ByVal Anio As Integer, ByVal Mes As Integer, ByVal Dia As Integer) As Byte
        Dim valido As Boolean
        Dim auxAnio = Date.Now().Year
        If (Anio + 2000) > auxAnio Then
            Anio += 1900
        Else
            Anio += 2000
        End If
        Try
            Dim Fecha As New Date(Anio, Mes, Dia)
            valido = True
        Catch ex As Exception
            valido = False
        End Try
        Return valido
    End Function
End Class