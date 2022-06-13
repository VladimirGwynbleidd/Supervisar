Imports Entities

Public Class NotificacionesConfiguracionEstilos
    Inherits System.Web.UI.Page

    '***********************************************************************************************************
    ' Fecha Creación:       18 Julio 2013
    ' Codificó:             Jorge Alberto Rangel Ruiz
    ' Empresa:              Softtek
    ' Descripción           Pantalla para configuracion de estilos para notificaciones
    '***********************************************************************************************************

    Public Property Mensaje As String

#Region "Constantes"

    Private Const ViewStateColor = "ViewStateColor"
    Private Const ViewStateAlineacion = "ViewStateAlineacion"
    Private Const ViewStateTamanioFuente = "ViewStateTamanioFuente"
    Private Const ViewStateTipoFuente = "ViewStateTipoFuente"
    Private Const ViewStateEstilos = "ViewStateEstilos"

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Try

                ' cargamos catalogos y datos de configuracion de estuilos
                CargaCatalogos()
                CargaDatos()


            Catch ex As ApplicationException

                lblError.Text = ex.Message
                btnAceptar.Enabled = False
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosError", "MostrarError();", True)

            End Try

        End If

    End Sub

    Private Sub grvDatosConfiguracion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grvDatosConfiguracion.RowDataBound

        If e.Row.RowType = Web.UI.WebControls.DataControlRowType.DataRow Then

            ' Obtenemos controles
            Dim ddlTipoLetra As DropDownList = CType(e.Row.FindControl("ddlTipoLetra"), DropDownList)
            Dim ddlTamanioLetra As DropDownList = CType(e.Row.FindControl("ddlTamanioLetra"), DropDownList)
            Dim ddlAlineacion As DropDownList = CType(e.Row.FindControl("ddlAlineacion"), DropDownList)
            Dim ddlColor As DropDownList = CType(e.Row.FindControl("ddlColor"), DropDownList)

            Dim chkNegrita As CheckBox = CType(e.Row.FindControl("chkNegrita"), CheckBox)
            Dim chkItalica As CheckBox = CType(e.Row.FindControl("chkItalica"), CheckBox)

            ' cargamos catalogos
            CargaCombo(ddlTipoLetra, ViewState(ViewStateTipoFuente), "Identificador", "Descripcion")
            CargaCombo(ddlTamanioLetra, ViewState(ViewStateTamanioFuente), "Identificador", "Descripcion")
            CargaCombo(ddlAlineacion, ViewState(ViewStateAlineacion), "Identificador", "Descripcion")
            CargaCombo(ddlColor, ViewState(ViewStateColor), "Identificador", "Descripcion")

            Dim rowEstilo As Entities.Notificaciones.EstiloContenido = CType(e.Row.DataItem, Entities.Notificaciones.EstiloContenido)

            ' seleccionamos valores
            ddlTipoLetra.SelectedValue = rowEstilo.TipoFuente.Identificador.ToString
            ddlTamanioLetra.SelectedValue = rowEstilo.TamanioFuente.Identificador.ToString
            ddlAlineacion.SelectedValue = rowEstilo.Alineacion.Identificador.ToString
            ddlColor.SelectedValue = rowEstilo.Color.Identificador.ToString

            chkNegrita.Checked = rowEstilo.IsNegrita
            chkNegrita.Checked = rowEstilo.IsItalica


            ' definimos que es visibe y qué no
            Select Case rowEstilo.Identificador



                Case Entities.Notificaciones.TipoEstiloNotificacion.estilofondo, Entities.Notificaciones.TipoEstiloNotificacion.Estilofondo2, Entities.Notificaciones.TipoEstiloNotificacion.estilobordediv

                    ddlTipoLetra.Visible = False
                    ddlTamanioLetra.Visible = False
                    ddlAlineacion.Visible = False

                    chkItalica.Visible = False
                    chkNegrita.Visible = False

                Case Entities.Notificaciones.TipoEstiloNotificacion.Estiloletra2, Entities.Notificaciones.TipoEstiloNotificacion.Estiloletra3

                    ddlAlineacion.Visible = False

                Case Else

                    Console.WriteLine("Se carga todo")

            End Select



        End If

    End Sub

#Region "Metodos de Carga"

    ''' <summary>
    ''' Método para obtener los datos de catálogos necesarios para la página
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaCatalogos()

        ViewState(ViewStateColor) = Entities.Notificaciones.ColorGetAll
        ViewState(ViewStateAlineacion) = Entities.Notificaciones.AlineacionGetAll
        ViewState(ViewStateTamanioFuente) = Entities.Notificaciones.TamanioFuenteGetAll
        ViewState(ViewStateTipoFuente) = Entities.Notificaciones.TipoFuenteGetAll

        ' Si algun catálogo no tiene datos, lanzamos error
        If IsNothing(ViewState(ViewStateColor)) OrElse IsNothing(ViewState(ViewStateAlineacion)) OrElse IsNothing(ViewState(ViewStateTamanioFuente)) _
            OrElse IsNothing(ViewState(ViewStateTipoFuente)) Then

            'TODO: AQUI DEBO LANZAR EL ERROR DEL CATALOGO DE ERRORES
            Throw New ApplicationException("Aqui mostrar mensaje del catalogo de errores")

        End If


    End Sub

    ''' <summary>
    ''' Método para obtener los datos de los estilos existentes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaDatos()

        Dim ListaEstilos As List(Of Entities.Notificaciones.EstiloContenido) = Entities.Notificaciones.EstiloContenidoGetAll()

        If IsNothing(ListaEstilos) Then

            'TODO: AQUI DEBO LANZAR EL ERROR DEL CATALOGO DE ERRORES
            Throw New ApplicationException("Aqui mostrar mensaje del catalogo de errores")

        End If

        ViewState(ViewStateEstilos) = ListaEstilos

        grvDatosConfiguracion.DataSource = ListaEstilos
        grvDatosConfiguracion.DataBind()

    End Sub

#End Region

#Region "Metodos Generales"

    ''' <summary>
    ''' Método para cargar un control dropdownlist
    ''' </summary>
    ''' <param name="ddl">control a cargar</param>
    ''' <param name="datasource">Origen de datos a cargar</param>
    ''' <param name="Valor">Campo que se cargara como el value del control</param>
    ''' <param name="Texto">Campo que se cargara como el text del control</param>
    ''' <remarks></remarks>
    Private Sub CargaCombo(ByVal ddl As DropDownList, ByVal datasource As Object, ByVal Valor As String, ByVal Texto As String)

        ddl.DataSource = datasource
        ddl.DataTextField = Texto
        ddl.DataValueField = Valor
        ddl.DataBind()

    End Sub

    ''' <summary>
    ''' Método para guardar los datos de los estilos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Guardar()

        Dim resultados As New List(Of Entities.Notificaciones.EstiloContenido)

        For Each renglon As GridViewRow In grvDatosConfiguracion.Rows

            Dim ddlTipoLetra As DropDownList = CType(renglon.FindControl("ddlTipoLetra"), DropDownList)
            Dim ddlTamanioLetra As DropDownList = CType(renglon.FindControl("ddlTamanioLetra"), DropDownList)
            Dim ddlAlineacion As DropDownList = CType(renglon.FindControl("ddlAlineacion"), DropDownList)
            Dim ddlColor As DropDownList = CType(renglon.FindControl("ddlColor"), DropDownList)

            Dim chkNegrita As CheckBox = CType(renglon.FindControl("chkNegrita"), CheckBox)
            Dim chkItalica As CheckBox = CType(renglon.FindControl("chkItalica"), CheckBox)



            resultados.Add(New Entities.Notificaciones.EstiloContenido With {.Identificador = Convert.ToInt32(grvDatosConfiguracion.DataKeys(renglon.RowIndex).Value), _
                                                                             .Color = New Entities.Notificaciones.Color With {.Identificador = ddlColor.SelectedValue}, _
                                                                             .Alineacion = New Entities.Notificaciones.Alineacion With {.Identificador = ddlAlineacion.SelectedValue}, _
                                                                             .TamanioFuente = New Entities.Notificaciones.TamanioFuente With {.Identificador = ddlTamanioLetra.SelectedValue}, _
                                                                             .TipoFuente = New Entities.Notificaciones.TipoFuente With {.Identificador = ddlTipoLetra.SelectedValue}, _
                                                                             .IsItalica = chkItalica.Checked, _
                                                                             .IsNegrita = chkNegrita.Checked})


        Next


        Try
            Dim errores As EtiquetaError
            If Not Entities.Notificaciones.GuardaConfiguracionEstilos(resultados, CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario) Then

                errores = New EtiquetaError(90)
                lblError.Text = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosError", "MostrarError();", True)

                Exit Sub

            End If

            errores = New EtiquetaError(91)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosConfiguracion", "MostrarConfirmacion();", True)


        Catch ex As Exception

            lblError.Text = ex.Message
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MostramosError", "MostrarError();", True)


        End Try

    End Sub



#End Region

#Region "Botones"

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        ' regresamos a página padre
        Response.Redirect("Notificaciones.aspx", False)

    End Sub

    Private Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptarM2B1A.Click

        ' invocamos guardado de datos
        Guardar()

    End Sub

    Private Sub btnAceptarM1B1A_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptarM1B1A.Click

        ' regresamos a página padre
        Response.Redirect("Notificaciones.aspx", False)

    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Dim errores As New EtiquetaError(92)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PreguntaGuardar", "PreguntaGuardar();", True)
    End Sub

#End Region

End Class