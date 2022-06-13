Public Class NotificacionesDatos
    Inherits System.Web.UI.Page

    '***********************************************************************************************************
    ' Fecha Creación:       23 Julio 2013
    ' Codificó:             Jorge Alberto Rangel Ruiz
    ' Empresa:              Softtek
    ' Descripción           Pantalla de captura de notificación
    '***********************************************************************************************************

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            ' asignamos el valor del viewstate IdMensaje para definir la página como Agregar
            ViewState("IdMensaje") = 0

            ' En caso de que se solicite la modificacion de un mensaje existente, 
            If Not IsNothing(Request.QueryString("idmensaje")) Then

                ' almacenamos el identificador del mensaje a modificar
                ViewState("IdMensaje") = Request.QueryString("idmensaje")

                ' cargamos datos del mensaje
                CargaDatos()

                ' cambiamos titulo de la pagina
                lblTitulo.Text = "Modificación de Mensaje"

            End If

            ' Preparamos la página
            CargaFiltros()
            MuestraControlesAsignacion()
            CargaUsuariosNoAsignados()

        Else

            Dim CtrlID As String = String.Empty
            If Request.Form("__EVENTTARGET") IsNot Nothing And _
               Request.Form("__EVENTTARGET") <> String.Empty Then
                CtrlID = Request.Form("__EVENTTARGET")
            Else
                'Buttons and ImageButtons
                If Request.Form(hidSourceID.UniqueID) IsNot Nothing And _
                   Request.Form(hidSourceID.UniqueID) <> String.Empty Then
                    CtrlID = Request.Form(hidSourceID.UniqueID)
                End If
            End If

            If ScriptManager.GetCurrent(Me).IsInAsyncPostBack OrElse CtrlID = "btnAceptarM1B1A" Then
                grvAsignados.DataBindEnPostBack = True
                grvNoAsignados.DataBindEnPostBack = True
            Else
                grvAsignados.DataBindEnPostBack = False
                grvNoAsignados.DataBindEnPostBack = False
            End If


        End If

        ' colocamos fechas default
        Me.txtFechaInicial.Text = Now.ToString("dd/MM/yyyy")
        Me.txtFechaFinal.Text = Now.AddDays(7).ToString("dd/MM/yyyy")

    End Sub

    ''' <summary>
    ''' Método que carga los datos de un mensaje
    ''' </summary>
    ''' <remarks>Toma los valores del campo IdMensaje del Viewstate</remarks>
    Private Sub CargaDatos()

        Dim Notificacion As Entities.Notificaciones.NotificacionPantalla = Entities.Notificaciones.NotificacionesPantallaGetOne(CInt(ViewState("IdMensaje")))

        Me.txtNombre.Text = Notificacion.Titulo
        Me.txtMensaje.Text = Notificacion.Texto

        If Notificacion.ArchivoAdjunto <> "" Then

            Me.btnArchivo.Text = Notificacion.ArchivoAdjunto
            Me.btnArchivo.Visible = True
            Me.btnReemplazaArchivo.Visible = True
            Me.fulArchivoAdjunto.Visible = False

        End If

        CargaUsuariosAsignados()

    End Sub

#Region "Metodos Filtrado Usuarios"

    ''' <summary>
    ''' Método para definir y cargar los filtros de usuarios
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaFiltros()

        Filtros.resetSession()

        Dim ListaUsuarios As List(Of Entities.Usuario) = Entities.Usuario.UsuariosSoloDatosGetCustom("WHERE N_FLAG_VIG = 1")
        Dim usuarios = From p In ListaUsuarios Select New With {.T_ID_USUARIO = p.IdentificadorUsuario, .Identificador = p.IdentificadorUsuario}
        Filtros.AddFilter("Usuario ", ucFiltro.AcceptedControls.DropDownList, usuarios, "Identificador", "T_ID_USUARIO", ucFiltro.DataValueType.StringType, False, False, False, False, False, Nothing)

        Filtros.AddFilter("Nombre ", ucFiltro.AcceptedControls.TextBox, Nothing, "T_DSC_NOMBRE", "T_DSC_NOMBRE", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 100)
        Filtros.AddFilter("Apellido ", ucFiltro.AcceptedControls.TextBox, Nothing, "T_DSC_APELLIDO", "T_DSC_APELLIDO", ucFiltro.DataValueType.StringType, False, True, False, False, False, Nothing, 100)

        Filtros.LoadDDL(Me.ClientID)

    End Sub

    ' evento de filtrado
    Private Sub Filtros_Filtrar(ByVal sender As Object, ByVal e As System.EventArgs) Handles Filtros.Filtrar

        CargaUsuariosNoAsignados()

    End Sub

    ''' <summary>
    ''' Método para obtener el filtro de búsqueda con los elementos seleccionados
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFiltro() As String

        Dim filtros = Me.Filtros.getFilterSelection
        Dim where = "WHERE 1=1 "
        For Each filtro As String In filtros
            where += " AND " + filtro
        Next

        Return where

    End Function

    ''' <summary>
    ''' Método para obtener los usuarios NO asignados hasta el momento
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaUsuariosNoAsignados()

        Dim ListaUsuarios As New List(Of Entities.Usuario)
        Dim where As String = GetFiltro()

        ' si es modificacion de mensaje, excluimos a los ya asignados
        If CInt(ViewState("IdMensaje")) > 0 Then

            where += Entities.Notificaciones.CadenaUsuariosExlusionNotificacionPantalla(CInt(ViewState("IdMensaje")))

        End If

        ' mostramos solo vigentes
        where += " AND N_FLAG_VIG = 1 "

        ListaUsuarios.AddRange(Entities.Usuario.UsuariosSoloDatosGetCustom(where))

        grvNoAsignados.DataSource = ListaUsuarios
        grvNoAsignados.DataBind()

    End Sub


#End Region

#Region "Metodos generales"

    ''' <summary>
    ''' Método para mostrar u ocultar controles de asignacion de usuarios
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MuestraControlesAsignacion()

        If CInt(ViewState("IdMensaje")) = 0 Then

            celdaAsignados.Style.Add("display", "none")
            celdaBotones.Style.Add("display", "none")
            celdaNoAsignados.Style.Add("width", "100%")
            grvNoAsignados.Width = Web.UI.WebControls.Unit.Pixel(800)

        End If

    End Sub

    ''' <summary>
    ''' Método para obtener los usuarios asignados hasta el momento
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaUsuariosAsignados()

        Dim ListaUsuariosAsignados As List(Of Entities.Notificaciones.UsuariosNotificacionPantalla) = Entities.Notificaciones.ListaUsuariosNotificacionPantalla(CInt(ViewState("IdMensaje")))

        grvAsignados.DataSource = ListaUsuariosAsignados
        grvAsignados.DataBind()

    End Sub

    ''' <summary>
    ''' Método para indicar la salida de la página
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Salida()

        lblConfirmaSalida.Text = "Datos Guardados"
        TablaFechas.Visible = False
        ScriptManager.RegisterStartupScript(btnAsignaUsuario, Me.GetType(), "MostramosMensajeAccion", "MostrarMensajeAccion();", True)

        btnAceptar.CommandArgument = "Salida"

    End Sub

#End Region

#Region "Acciones de Botones Generales"

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click

        ' verificamos campos no vacios
        Page.Validate("Notificaciones")

        If Not Page.IsValid Then

            Me.lblError.Text = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "MostramosMensaje", "MostrarMensaje();", True)
            Exit Sub

        End If


        If CInt(ViewState("IdMensaje")) = 0 Then

            Guardar()

        Else

            Actualizar()

        End If


    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        ' redireccionamos a la pantalla de la baneja de mensajes
        Response.Redirect("NotificacionesBandeja.aspx", False)

    End Sub

    ''' <summary>
    ''' Metodo para gurdar datos de una nueva notificacion
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Guardar()

        ' Si ya se selecciono un archivo, se guarda temporalmente y se almacena el nombre del archivo
        ViewState("nombrearchivo") = ""
        If fulArchivoAdjunto.FileName <> "" Then

            fulArchivoAdjunto.SaveAs(System.IO.Path.GetTempPath() + fulArchivoAdjunto.FileName)

            ' Este se tomará posteriormente
            ViewState("nombrearchivo") = fulArchivoAdjunto.FileName

        End If

        ' llamamos a método que hace todo
        btnAsignaUsuario_Click(Nothing, Nothing)

    End Sub

    ''' <summary>
    ''' Metodo para actualizar una notificacion existente
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Actualizar()

        Dim notificacion As New Entities.Notificaciones.NotificacionPantalla

        notificacion.Identificador = CInt(ViewState("IdMensaje"))
        notificacion.Texto = Me.txtMensaje.Text.Trim
        notificacion.Titulo = Me.txtNombre.Text.Trim
        notificacion.ArchivoAdjunto = ""

        ' En caso de capturarse/cambiarse archivo
        ViewState("nombrearchivo") = ""
        If fulArchivoAdjunto.FileName <> "" Then

            fulArchivoAdjunto.SaveAs(System.IO.Path.GetTempPath() + fulArchivoAdjunto.FileName)

            ' Este se tomará posteriormente
            ViewState("nombrearchivo") = fulArchivoAdjunto.FileName

        End If

        notificacion.ArchivoAdjunto = ViewState("nombrearchivo")

        If Not Entities.Notificaciones.ActualizaNotificacionPantalla(notificacion) Then

            Dim errores As New Entities.EtiquetaError(29)
            Me.lblError.Text = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosMensaje", "MostrarMensaje();", True)

            Exit Sub

        End If

        If ViewState("nombrearchivo") <> "" Then

            'TODO: AQUI SE DEBE IMPLEMENTAR LA LOGICA PARA GUARDAR EL ARCHIVO SEGUN REGLAS DE NEGOCIO DEL SISTEMA A IMPLEMENTAR
            ' ya se encuentra remporalmente guardado en "System.IO.Path.GetTempPath() + ViewState("nombrearchivo")"

        End If

        '' Indicamos salida de la pagina
        Salida()



    End Sub

    Private Sub btnReemplazaArchivo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReemplazaArchivo.Click

        Me.btnArchivo.Visible = False
        Me.btnReemplazaArchivo.Visible = False
        Me.fulArchivoAdjunto.Visible = True

    End Sub

    Private Sub btnArchivo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnArchivo.Click

        'TODO: AQUI SE DEBE IMPLEMENTAR LA LOGICA PARA DESCARGAR EL ARCHIVO DEL REPOSITORIO DE NEGOCIO DEL SISTEMA A IMPLEMENTAR
        ' EL ARCHIVO ES btnArchivo.TEXT 

    End Sub

#End Region

#Region "Acciones de Botones de Mensajes"


    Private Sub Boton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTitulo.Click, btnLetra1.Click, btnLetra2.Click, _
        btnLetra3.Click, btnVinculo1.Click, btnVinculo2.Click, btnRecuadro.Click, btnArchivoAdjunto.Click

        ' Agregamos etiqueta segun el boton seleccionado
        Dim _etiqueta As String = CType(sender, Button).ToolTip

        ' si hay texto, se agrega un salto de linea
        If txtMensaje.Text = "" Then
            txtMensaje.Text = _etiqueta
        Else
            txtMensaje.Text = txtMensaje.Text & vbNewLine & _etiqueta
        End If

    End Sub

    Private Sub btnVistaPrevia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVistaPrevia.Click

        ' Generamos vista previa del mensaje
        ucNotificaciones.LevantarVistaPrevia(Me.txtMensaje.Text)

    End Sub

#End Region

#Region "Botones de Asignacion Usuarios"

    Private Sub btnAsignaUsuario_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAsignaUsuario.Click

        Dim listaIndices As List(Of Integer) = grvNoAsignados.MultiSelectedIndex


        ' verificamos que haya algo seleccionado
        If Not listaIndices.Any() Then

            Dim errores As New Entities.EtiquetaError(30)
            Me.lblError.Text = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosMensaje", "MostrarMensaje();", True)

            Exit Sub
        End If



        ' levantamos ventana para que seleccionen fechas
        ScriptManager.RegisterStartupScript(btnAsignaUsuario, Me.GetType(), "MostramosMensajeAccion", "MostrarMensajeAccion();", True)


    End Sub

    Private Sub btnAceptarM1B1A_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptarM1B1A.Click

        ' Si se indica, regresamos a la página padre
        If btnAceptar.CommandArgument = "Salida" Then

            btnCancelar_Click(Nothing, Nothing)
            Exit Sub

        End If


        ' Validamos fechas
        If txtFechaInicial.Text.Trim = String.Empty OrElse txtFechaFinal.Text.Trim = String.Empty Then

            Dim errores As New Entities.EtiquetaError(32)
            Me.lblError.Text = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosMensaje", "MostrarMensaje();", True)

            Exit Sub

        End If


        ' variable para determinar si al terminar debo regresar a la pantalla padre
        Dim reenvia As Boolean = False

        ' es un mensaje nuevo, hay que guardar
        If CInt(ViewState("IdMensaje")) = 0 Then

            reenvia = True

            ' asignamos datos al objeto
            Dim notificacion As New Entities.Notificaciones.NotificacionPantalla
            notificacion.Identificador = 0
            notificacion.Titulo = Me.txtNombre.Text.Trim
            notificacion.Texto = Me.txtMensaje.Text.Trim
            notificacion.ArchivoAdjunto = ViewState("nombrearchivo").ToString()
            notificacion.FechaInicioVigencia =
            notificacion.FechaFinVigencia


            ' guardamos notificacion y asignamos resultado
            ViewState("IdMensaje") = Entities.Notificaciones.GuardaNotificacionPantalla(notificacion)


            ' Si no se pudo guardar la notificacion
            If CInt(ViewState("IdMensaje")) = 0 Then

                Dim errores As New Entities.EtiquetaError(29)
                Me.lblError.Text = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosMensaje", "MostrarMensaje();", True)
                Exit Sub


            End If


            If ViewState("nombrearchivo") <> "" Then

                'TODO: AQUI SE DEBE IMPLEMENTAR LA LOGICA PARA GUARDAR EL ARCHIVO SEGUN REGLAS DE NEGOCIO DEL SISTEMA A IMPLEMENTAR
                ' ya se encuentra remporalmente guardado en "System.IO.Path.GetTempPath() + ViewState("nombrearchivo")"

            End If


        End If

        ' Ahora generamos asignacion de usuarios
        Dim listaAsignados As New List(Of Entities.Notificaciones.UsuariosNotificacionPantalla)
        Dim listaIndices As List(Of Integer) = grvNoAsignados.MultiSelectedIndex

        ' generamos datos para guardar     
        For Each renglon As GridViewRow In grvNoAsignados.Rows

            If listaIndices.Contains(renglon.RowIndex) Then


                listaAsignados.Add(New Entities.Notificaciones.UsuariosNotificacionPantalla With {.Usuario = renglon.Cells(1).Text, _
                                                                                                   .IdentificadorNotificacion = CInt(ViewState("IdMensaje")),
                                                                                                  .FechaInicio = CDate(txtFechaInicial.Text), _
                                                                                                  .FechaFinalizacion = CDate(txtFechaFinal.Text)})

            End If

        Next


        ' realizamos operación de asignacion
        If Not Entities.Notificaciones.AsignaUsuariosNotificación(listaAsignados, CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario) Then

            '' Si no se pudo asignar
            Dim errores As New Entities.EtiquetaError(31)
            Me.lblError.Text = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosMensaje", "MostrarMensaje();", True)


        End If

        ' en caso necesario redireccionamos a pagina padre
        If reenvia Then

            Salida()

            Exit Sub
        End If

        ' cargamos grid de usuarios
        CargaUsuariosAsignados()
        CargaUsuariosNoAsignados()


    End Sub

    Private Sub btnDesasignaUsuario_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDesasignaUsuario.Click

        Dim listaIndices As List(Of Integer) = grvAsignados.MultiSelectedIndex

        ' verificamos que haya seleccionados
        If Not listaIndices.Any() Then

            Dim errores As New Entities.EtiquetaError(30)
            Me.lblError.Text = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosMensaje", "MostrarMensaje();", True)

            Exit Sub
        End If

        Dim ListaDesasignar As New List(Of Entities.Notificaciones.UsuariosNotificacionPantalla)

        ' generamos datos para guardar
        For Each renglon As GridViewRow In grvAsignados.Rows

            If listaIndices.Contains(renglon.RowIndex) Then


                ListaDesasignar.Add(New Entities.Notificaciones.UsuariosNotificacionPantalla With {.Usuario = renglon.Cells(1).Text, _
                                                                                                   .IdentificadorNotificacion = CInt(ViewState("IdMensaje"))})

            End If

        Next

        ' realizamos operación de eliminado
        If Not Entities.Notificaciones.DesasignaUsuariosNotificación(ListaDesasignar, CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario) Then

            ' Si no se pudo eliminar
            Dim errores As New Entities.EtiquetaError(29)
            Me.lblError.Text = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosMensaje", "MostrarMensaje();", True)
            Exit Sub

        End If

        ' cargamos grid de usuarios
        CargaUsuariosAsignados()
        CargaUsuariosNoAsignados()

    End Sub



#End Region

#Region "Validaciones"

    Protected Sub cvNombre_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvNombre.ServerValidate

        If txtNombre.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(27)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If

    End Sub


    Protected Sub cvMensaje_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvMensaje.ServerValidate

        If txtMensaje.Text.Trim() = String.Empty Then
            Dim errores As New Entities.EtiquetaError(28)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
        End If

    End Sub

#End Region

End Class