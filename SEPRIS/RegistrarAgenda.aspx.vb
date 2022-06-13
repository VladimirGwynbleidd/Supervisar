Imports System.Web.Configuration
Imports System.Xml
Imports Utilerias
Imports System.Net
Imports Entities
Imports Negocio

Public Class RegistrarAgenda
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Public Property Mensaje As String
    Private Property IdRegistroAgenda As Integer
        Get
            If IsNothing(ViewState("IdRegistroAgenda")) Then
                Return 0
            Else
                Return Convert.ToInt32(ViewState("IdRegistroAgenda"))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("IdRegistroAgenda") = value
        End Set
    End Property
    Private Property EsAutorizacion As Boolean
        Get
            Return Convert.ToBoolean(ViewState("EsAutorizacion"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("EsAutorizacion") = value
        End Set
    End Property
    Private Property EsActualizacion As Boolean
        Get
            If Not IsNothing(ViewState("EsActualizacion")) Then
                Return Convert.ToBoolean(ViewState("EsActualizacion"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("EsActualizacion") = value
        End Set
    End Property
    Private Property EsConsulta As Boolean
        Get
            If Not IsNothing(ViewState("EsConsulta")) Then
                Return Convert.ToBoolean(ViewState("EsConsulta"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("EsConsulta") = value
        End Set
    End Property
    Private Property PropiedadRegistroAgenda As RegistroAgenda
        Get
            Return TryCast(ViewState("PropiedadRegistroAgenda"), RegistroAgenda)
        End Get
        Set(ByVal value As RegistroAgenda)
            ViewState("PropiedadRegistroAgenda") = value
        End Set
    End Property
    Private Property EsVacaciones As Boolean
        Get
            If Not IsNothing(ViewState("EsVacaciones")) Then
                Return Convert.ToBoolean(ViewState("EsVacaciones"))
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("EsVacaciones") = value
        End Set
    End Property
#End Region
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If Not IsNothing(Session("IdRegistroAgenda")) Then
                IdRegistroAgenda = Session("IdRegistroAgenda")
                EsActualizacion = True
                Session("IdRegistroAgenda") = Nothing
                lblTitulo.InnerHtml = "Modificación Registro en Agenda"
            End If

            If Not IsNothing(Session("EsConsulta")) Then
                If Convert.ToBoolean(Session("EsConsulta")) Then
                    EsConsulta = True
                    DeshabilitaCamposConsulta()
                End If
                Session("EsConsulta") = Nothing
                lblTitulo.InnerHtml = "Consulta Registro en Agenda"
            End If

            If Not IsNothing(Session("EsAutorizacion")) Then
                If Convert.ToBoolean(Session("EsAutorizacion")) Then
                    EsAutorizacion = Convert.ToBoolean(Session("EsAutorizacion"))
                    DeshabilitaCamposAutorizacion()
                Else
                    EsAutorizacion = False
                End If
                Session("EsAutorizacion") = Nothing
                lblTitulo.InnerHtml = "Autorización Registro en Agenda"
            End If

            CargaDLL()

            If EsActualizacion Or EsAutorizacion Then
                CargaRegistroAgenda()                
            End If

        Else
            lblRestanteDescripcionActividad.InnerText = 1000 - txtDescripcionActividad.Text.Length
            lblRestanteNotasAutorizador.InnerText = 1000 - txtNotasAutorizador.Text.Length
        End If
    End Sub

#Region "Carga Datos"

    ''' <summary>
    ''' Carga la informacion de los DropDownList
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaDLL()
        CargaDLLActividad()
        CargaDLLAusencia()

        CargaHoras()
    End Sub

    ''' <summary>
    ''' Carga la informacion de DropDownList Tipo de Actividad
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaDLLActividad()
        Dim objTipoActividad As New TipoActividad()
        Dim dv As DataView = objTipoActividad.ObtenerTodos()
        Dim consulta As String = " B_FLAG_VIG = 1 AND N_ID_TIPO_ACTIVIDAD <> 1 "

        dv.RowFilter = consulta
        dv.Sort = " T_DSC_TIPO_ACTIVIDAD ASC "

        Utilerias.Generales.CargarCombo(ddlTipoActividad, dv, "T_DSC_TIPO_ACTIVIDAD", "N_ID_TIPO_ACTIVIDAD")

    End Sub

    ''' <summary>
    ''' Carga la informacion de DropDownList Tipo de Ausencia
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaDLLAusencia()
        Dim objTipoAusencia As New TipoAusencia()
        Dim dv As DataView = objTipoAusencia.ObtenerTodos()
        Dim consulta As String = " B_FLAG_VIG = 1"

        dv.RowFilter = consulta
        dv.Sort = " T_DSC_TIPO_AUSENCIA ASC "

        Utilerias.Generales.CargarCombo(ddlTipoAusencia, dv, "T_DSC_TIPO_AUSENCIA", "N_ID_TIPO_AUSENCIA")

    End Sub

    ''' <summary>
    ''' Carga el DropDownList de autorizadores
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaDLLAutorizadores(ByVal Activo As Boolean)        
        If EsAutorizacion Or EsConsulta Then
            Dim objUsusario As New Entities.Usuario(New RegistroAgenda(IdRegistroAgenda).Autorizador)
            ddlFuncionario.Items.Add(New ListItem(objUsusario.Nombre + " " + objUsusario.Apellido + " " + objUsusario.ApellidoAuxiliar, 1))
            tbAutoriza.Visible = True
        Else
            If Activo Then
                tbAutoriza.Visible = True
                CargaPosiblesAutorizadores()
                ddlFuncionario.Items.Insert(0, New ListItem("- Seleccionar -", "-1"))
            Else
                tbAutoriza.Visible = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' Carga el cuadro de vacaciones
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaVacaciones(ByVal Activo As Boolean, Optional ByVal idUsuario As String = Nothing)
        If Activo Then
            tbVacaciones.Visible = True
            ddlHoraInicioCiclico.SelectedValue = 9
            ddlHoraInicioCiclico.Enabled = False
            ddlHoraInicioContinuo.SelectedValue = 9
            ddlHoraInicioContinuo.Enabled = False
            ddlHoraFinCiclico.SelectedValue = 19
            ddlHoraFinCiclico.Enabled = False
            ddlHoraFinContinuo.SelectedValue = 19
            ddlHoraFinContinuo.Enabled = False

            Dim objVacaciones As New Vacaciones()

            Dim UsuarioActual As Usuario
            If idUsuario Is Nothing Then
                UsuarioActual = TryCast(Session(Entities.Usuario.SessionID), Usuario)
            Else
                UsuarioActual = New Usuario(idUsuario)
            End If

            Dim lstObjVacaciones As List(Of Vacaciones) = objVacaciones.ObtnenPeriodosActivos(UsuarioActual.IdentificadorUsuario)

            Select Case lstObjVacaciones.Count
                Case 0
                    lblNoHayVacaciones.Visible = True
                    TieneVacaciones.Visible = False
                Case 1
                    lblNoHayVacaciones.Visible = False
                    TieneVacaciones.Visible = True
                    PeriodoAnterior.Visible = False
                    lblPeriodoActual.InnerText = lblPeriodoActual.InnerText.Replace("#INICIO#", lstObjVacaciones(0).InicioPeriodo.ToString("yyyy-MM-dd"))
                    lblPeriodoActual.InnerText = lblPeriodoActual.InnerText.Replace("#FIN#", lstObjVacaciones(0).FinPeriodo.ToString("yyyy-MM-dd"))
                    lblRestanteActual.InnerText = lblRestanteActual.InnerText.Replace("#DIAS#", (lstObjVacaciones(0).DiasAsignados - lstObjVacaciones(0).DiasConsumidos).ToString)
                Case 2
                    lblNoHayVacaciones.Visible = False
                    TieneVacaciones.Visible = True
                    PeriodoAnterior.Visible = True
                    lblPeriodoAnterior.InnerText = lblPeriodoAnterior.InnerText.Replace("#INICIO#", lstObjVacaciones(0).InicioPeriodo.ToString("yyyy-MM-dd"))
                    lblPeriodoAnterior.InnerText = lblPeriodoAnterior.InnerText.Replace("#FIN#", lstObjVacaciones(0).FinPeriodo.ToString("yyyy-MM-dd"))
                    lblRestantesAnterior.InnerText = lblRestantesAnterior.InnerText.Replace("#DIAS#", (lstObjVacaciones(0).DiasAsignados - lstObjVacaciones(0).DiasConsumidos).ToString)

                    lblPeriodoActual.InnerText = lblPeriodoActual.InnerText.Replace("#INICIO#", lstObjVacaciones(1).InicioPeriodo.ToString("yyyy-MM-dd"))
                    lblPeriodoActual.InnerText = lblPeriodoActual.InnerText.Replace("#FIN#", lstObjVacaciones(1).FinPeriodo.ToString("yyyy-MM-dd"))
                    lblRestanteActual.InnerText = lblRestanteActual.InnerText.Replace("#DIAS#", (lstObjVacaciones(1).DiasAsignados - lstObjVacaciones(1).DiasConsumidos).ToString)
                Case Else
                    lblNoHayVacaciones.Visible = False
                    TieneVacaciones.Visible = True
                    PeriodoAnterior.Visible = True
                    Dim numero As Integer = lstObjVacaciones.Count - 1

                    lblPeriodoAnterior.InnerText = lblPeriodoAnterior.InnerText.Replace("#INICIO#", lstObjVacaciones(numero - 1).InicioPeriodo.ToString("yyyy-MM-dd"))
                    lblPeriodoAnterior.InnerText = lblPeriodoAnterior.InnerText.Replace("#FIN#", lstObjVacaciones(numero - 1).FinPeriodo.ToString("yyyy-MM-dd"))
                    lblRestantesAnterior.InnerText = lblRestantesAnterior.InnerText.Replace("#DIAS#", (lstObjVacaciones(numero - 1).DiasAsignados - lstObjVacaciones(numero - 1).DiasConsumidos).ToString)

                    lblPeriodoActual.InnerText = lblPeriodoActual.InnerText.Replace("#INICIO#", lstObjVacaciones(numero).InicioPeriodo.ToString("yyyy-MM-dd"))
                    lblPeriodoActual.InnerText = lblPeriodoActual.InnerText.Replace("#FIN#", lstObjVacaciones(numero).FinPeriodo.ToString("yyyy-MM-dd"))
                    lblRestanteActual.InnerText = lblRestanteActual.InnerText.Replace("#DIAS#", (lstObjVacaciones(numero).DiasAsignados - lstObjVacaciones(numero).DiasConsumidos).ToString)
            End Select
        Else
            tbVacaciones.Visible = False
            ddlHoraInicioCiclico.SelectedValue = -1
            ddlHoraInicioCiclico.Enabled = True
            ddlHoraInicioContinuo.SelectedValue = -1
            ddlHoraInicioContinuo.Enabled = True
            ddlHoraFinCiclico.SelectedValue = -1
            ddlHoraFinCiclico.Enabled = True
            ddlHoraFinContinuo.SelectedValue = -1
            ddlHoraFinContinuo.Enabled = True
        End If

    End Sub

    ''' <summary>
    ''' Carga los DropDownList de horas
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaHoras()
        Dim lstHoraInicio As New List(Of Object)
        lstHoraInicio.Add(New With {.HoraInicio = "9:00 am", .value = 9})
        lstHoraInicio.Add(New With {.HoraInicio = "10:00 am", .value = 10})
        lstHoraInicio.Add(New With {.HoraInicio = "11:00 am", .value = 11})
        lstHoraInicio.Add(New With {.HoraInicio = "12:00 pm", .value = 12})
        lstHoraInicio.Add(New With {.HoraInicio = "1:00 pm", .value = 13})
        lstHoraInicio.Add(New With {.HoraInicio = "4:00 pm", .value = 16})
        lstHoraInicio.Add(New With {.HoraInicio = "5:00 pm", .value = 17})
        lstHoraInicio.Add(New With {.HoraInicio = "6:00 pm", .value = 18})

        Utilerias.Generales.CargarCombo(ddlHoraInicioContinuo, lstHoraInicio, "HoraInicio", "value")
        Utilerias.Generales.CargarCombo(ddlHoraInicioCiclico, lstHoraInicio, "HoraInicio", "value")

        Dim lstHoraFin As New List(Of Object)        
        lstHoraFin.Add(New With {.HoraFin = "10:00 am", .value = 10})
        lstHoraFin.Add(New With {.HoraFin = "11:00 am", .value = 11})
        lstHoraFin.Add(New With {.HoraFin = "12:00 pm", .value = 12})
        lstHoraFin.Add(New With {.HoraFin = "1:00 pm", .value = 13})
        lstHoraFin.Add(New With {.HoraFin = "4:00 pm", .value = 16})
        lstHoraFin.Add(New With {.HoraFin = "5:00 pm", .value = 17})
        lstHoraFin.Add(New With {.HoraFin = "6:00 pm", .value = 18})
        lstHoraFin.Add(New With {.HoraFin = "7:00 pm", .value = 19})

        Utilerias.Generales.CargarCombo(ddlHoraFinCiclico, lstHoraFin, "HoraFin", "value")
        Utilerias.Generales.CargarCombo(ddlHoraFinContinuo, lstHoraFin, "HoraFin", "value")

    End Sub

    ''' <summary>
    ''' Carga los datos de un RegistroAgenda
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaRegistroAgenda()
        Dim objRegistroAgenda As New RegistroAgenda(IdRegistroAgenda)

        rblTipo.SelectedValue = objRegistroAgenda.IdTipoRegistro
        If objRegistroAgenda.IdTipoRegistro = RegistroAgenda.TipoRegistro.Actividad Then
            ddlTipoActividad.SelectedValue = objRegistroAgenda.TipoActividad
            tbAusencia.Visible = False
            tbActividad.Visible = True
        Else
            ddlTipoAusencia.SelectedValue = objRegistroAgenda.TipoAusencia
            Dim objTipoAusencia As New TipoAusencia(objRegistroAgenda.TipoAusencia)
            If objTipoAusencia.Identificador = 1 Then                
                CargaVacaciones(True, objRegistroAgenda.IngenieroSolicta)
            End If
            tbAusencia.Visible = True
            tbActividad.Visible = False
        End If

        If Not objRegistroAgenda.Autorizador Is Nothing Then
            CargaDLLAutorizadores(True)
            ddlFuncionario.SelectedValue = objRegistroAgenda.Autorizador
        End If

        chkCiclica.Checked = objRegistroAgenda.Ciclica
        If objRegistroAgenda.Ciclica Then
            txtFechaInicioCiclico.Text = objRegistroAgenda.FechIniReg.ToString("dd/MM/yyyy")
            txtFechaFinCiclico.Text = objRegistroAgenda.FechFinReg.ToString("dd/MM/yyyy")
            ddlHoraInicioCiclico.SelectedValue = objRegistroAgenda.FechIniReg.Hour
            ddlHoraFinCiclico.SelectedValue = objRegistroAgenda.FechFinReg.Hour
            chkLunes.Checked = objRegistroAgenda.Lunes
            chkMartes.Checked = objRegistroAgenda.Martes
            chkMiercoles.Checked = objRegistroAgenda.Miercoles
            chkJueves.Checked = objRegistroAgenda.Jueves
            chkViernes.Checked = objRegistroAgenda.Viernes
            tbTiempoCiclico.Visible = True
            tbTiempoContinuo.Visible = False
        Else
            txtFechaInicioContinuo.Text = objRegistroAgenda.FechIniReg.ToString("dd/MM/yyyy")
            ddlHoraInicioContinuo.SelectedValue = objRegistroAgenda.FechIniReg.Hour
            txtFechaFinContinuo.Text = objRegistroAgenda.FechFinReg.ToString("dd/MM/yyyy")
            ddlHoraFinContinuo.SelectedValue = objRegistroAgenda.FechFinReg.Hour
            tbTiempoCiclico.Visible = False
            tbTiempoContinuo.Visible = True
        End If

        txtDescripcionActividad.Text = objRegistroAgenda.NotaRegistro

        If Not IsNothing(objRegistroAgenda.NotaAutorizador) Then
            txtNotasAutorizador.Text = objRegistroAgenda.NotaAutorizador
            txtNotasAutorizador.ReadOnly = objRegistroAgenda.Autorizador <> CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
            tbNotasAutorizador.Visible = True
        End If

        lblRestanteDescripcionActividad.InnerText = 1000 - txtDescripcionActividad.Text.Length
        lblRestanteNotasAutorizador.InnerText = 1000 - txtNotasAutorizador.Text.Length

    End Sub

    ''' <summary>
    ''' Pone los controles como deshabilitados
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeshabilitaCamposConsulta()
        rblTipo.Enabled = False
        rblTipo.CssClass = "txt_solo_lectura"

        ddlTipoActividad.Enabled = False
        ddlTipoActividad.CssClass = "txt_solo_lectura"

        ddlTipoAusencia.Enabled = False
        ddlTipoAusencia.CssClass = "txt_solo_lectura"

        ddlFuncionario.Enabled = False
        ddlFuncionario.CssClass = "txt_solo_lectura"

        chkCiclica.Enabled = False
        'chkCiclica.CssClass = "txt_solo_lectura"

        txtFechaInicioContinuo.Enabled = False
        txtFechaInicioContinuo.CssClass = "txt_solo_lectura"

        ddlHoraInicioContinuo.Enabled = False
        ddlHoraInicioContinuo.CssClass = "txt_solo_lectura"

        txtFechaFinContinuo.Enabled = False
        txtFechaFinContinuo.CssClass = "txt_solo_lectura"

        ddlHoraFinContinuo.Enabled = False
        ddlHoraFinContinuo.CssClass = "txt_solo_lectura"

        txtFechaInicioCiclico.Enabled = False
        txtFechaInicioCiclico.CssClass = "txt_solo_lectura"

        txtFechaFinCiclico.Enabled = False
        txtFechaFinCiclico.CssClass = "txt_solo_lectura"

        ddlHoraInicioCiclico.Enabled = False
        ddlHoraInicioCiclico.CssClass = "txt_solo_lectura"

        ddlHoraFinCiclico.Enabled = False
        ddlHoraFinCiclico.CssClass = "txt_solo_lectura"

        chkLunes.Enabled = False
        'chkLunes.CssClass = "txt_solo_lectura"

        chkMartes.Enabled = False
        'chkMartes.CssClass = "txt_solo_lectura"

        chkMiercoles.Enabled = False
        'chkMiercoles.CssClass = "txt_solo_lectura"

        chkJueves.Enabled = False
        'chkJueves.CssClass = "txt_solo_lectura"

        chkViernes.Enabled = False
        'chkViernes.CssClass = "txt_solo_lectura"

        txtDescripcionActividad.Enabled = False
        txtDescripcionActividad.CssClass = "txt_solo_lectura"

        txtNotasAutorizador.Enabled = False
        txtNotasAutorizador.CssClass = "txt_solo_lectura"

        imgFec1.Visible = False
        imgFec2.Visible = False
        imgFec3.Visible = False
        imgFec4.Visible = False

        btnAceptar.Visible = False
        btnAutorizar.Visible = False
        btnRechazar.Visible = False

    End Sub

    Private Sub DeshabilitaCamposAutorizacion()
        rblTipo.Enabled = False
        rblTipo.CssClass = "txt_solo_lectura"

        ddlTipoActividad.Enabled = False
        ddlTipoActividad.CssClass = "txt_solo_lectura"

        ddlTipoAusencia.Enabled = False
        ddlTipoAusencia.CssClass = "txt_solo_lectura"

        ddlFuncionario.Enabled = False
        ddlFuncionario.CssClass = "txt_solo_lectura"

        chkCiclica.Enabled = False
        'chkCiclica.CssClass = "txt_solo_lectura"

        txtFechaInicioContinuo.Enabled = False
        txtFechaInicioContinuo.CssClass = "txt_solo_lectura"

        ddlHoraInicioContinuo.Enabled = False
        ddlHoraInicioContinuo.CssClass = "txt_solo_lectura"

        txtFechaFinContinuo.Enabled = False
        txtFechaFinContinuo.CssClass = "txt_solo_lectura"

        ddlHoraFinContinuo.Enabled = False
        ddlHoraFinContinuo.CssClass = "txt_solo_lectura"

        txtFechaInicioCiclico.Enabled = False
        txtFechaInicioCiclico.CssClass = "txt_solo_lectura"

        txtFechaFinCiclico.Enabled = False
        txtFechaFinCiclico.CssClass = "txt_solo_lectura"

        ddlHoraInicioCiclico.Enabled = False
        ddlHoraInicioCiclico.CssClass = "txt_solo_lectura"

        ddlHoraFinCiclico.Enabled = False
        ddlHoraFinCiclico.CssClass = "txt_solo_lectura"

        chkLunes.Enabled = False
        'chkLunes.CssClass = "txt_solo_lectura"

        chkMartes.Enabled = False
        'chkMartes.CssClass = "txt_solo_lectura"

        chkMiercoles.Enabled = False
        'chkMiercoles.CssClass = "txt_solo_lectura"

        chkJueves.Enabled = False
        'chkJueves.CssClass = "txt_solo_lectura"

        chkViernes.Enabled = False
        'chkViernes.CssClass = "txt_solo_lectura"

        txtDescripcionActividad.Enabled = False
        txtDescripcionActividad.CssClass = "txt_solo_lectura"

        imgFec1.Visible = False
        imgFec2.Visible = False
        imgFec3.Visible = False
        imgFec4.Visible = False

        tbNotasAutorizador.Visible = True

        btnAceptar.Visible = False
        btnAutorizar.Visible = True
        btnRechazar.Visible = True

    End Sub

#End Region

#Region "Controles"

    Private Sub rblTipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblTipo.SelectedIndexChanged
        Select Case rblTipo.SelectedValue
            Case 1  'Actividad
                tbActividad.Visible = True
                tbAusencia.Visible = False
            Case 2  'Ausencia
                tbActividad.Visible = False
                tbAusencia.Visible = True
        End Select
        ddlTipoActividad.SelectedIndex = 0
        ddlTipoAusencia.SelectedIndex = 0
        CargaDLLAutorizadores(False)
        CargaVacaciones(False)
    End Sub

    Private Sub ddlTipoActividad_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoActividad.SelectedIndexChanged
        Dim objTipoActividad As New TipoActividad(ddlTipoActividad.SelectedValue)
        CargaDLLAutorizadores(objTipoActividad.RequiereAutorizacion)
    End Sub

    Private Sub ddlTipoAusencia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoAusencia.SelectedIndexChanged
        Dim objTipoAusencia As New TipoAusencia(ddlTipoAusencia.SelectedValue)
        CargaDLLAutorizadores(objTipoAusencia.RequiereAutorizacion)

        'Si son vacaciones se activa el modulo de vacaciones
        CargaVacaciones(IIf(objTipoAusencia.Identificador = 1, True, False))

    End Sub

    Private Sub chkCiclica_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCiclica.CheckedChanged
        If chkCiclica.Checked Then
            tbTiempoCiclico.Visible = True
            tbTiempoContinuo.Visible = False
        Else
            tbTiempoCiclico.Visible = False
            tbTiempoContinuo.Visible = True
        End If
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Page.Validate("Forma")

        If Not Page.IsValid Then
            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim valido As Boolean = False
        Try
            Dim UsuarioActual As Usuario = TryCast(Session(Entities.Usuario.SessionID), Usuario)
            Dim fechaInicio As DateTime
            Dim fechaFin As DateTime
            If chkCiclica.Checked Then
                fechaInicio = Convert.ToDateTime(txtFechaInicioCiclico.Text)
                fechaInicio = New Date(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day, ddlHoraInicioCiclico.SelectedValue, 0, 0)
                fechaFin = Convert.ToDateTime(txtFechaFinCiclico.Text)
                fechaFin = New Date(fechaFin.Year, fechaFin.Month, fechaFin.Day, ddlHoraFinCiclico.SelectedValue, 0, 0)
            Else
                fechaInicio = Convert.ToDateTime(txtFechaInicioContinuo.Text)
                fechaInicio = New Date(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day, ddlHoraInicioContinuo.SelectedValue, 0, 0)
                fechaFin = Convert.ToDateTime(txtFechaFinContinuo.Text)
                fechaFin = New Date(fechaFin.Year, fechaFin.Month, fechaFin.Day, ddlHoraFinContinuo.SelectedValue, 0, 0)
            End If

            Dim objTipoActividad As TipoActividad
            Dim objTipoAusencias As TipoAusencia
            Dim aprobado As Boolean?
            Dim autoriza As String

            If rblTipo.SelectedValue = 1 Then
                objTipoActividad = New TipoActividad(Convert.ToInt32(ddlTipoActividad.SelectedValue))
                aprobado = IIf(objTipoActividad.RequiereAutorizacion, Nothing, True)
                autoriza = IIf(objTipoActividad.RequiereAutorizacion, ddlFuncionario.SelectedValue, "")
            Else
                objTipoAusencias = New TipoAusencia(Convert.ToInt32(ddlTipoAusencia.SelectedValue))
                aprobado = IIf(objTipoAusencias.RequiereAutorizacion, Nothing, True)
                autoriza = IIf(objTipoAusencias.RequiereAutorizacion, ddlFuncionario.SelectedValue, "")
            End If

            Dim tipoRegistro As RegistroAgenda.TipoRegistro
            If rblTipo.SelectedValue = 1 Then
                tipoRegistro = RegistroAgenda.TipoRegistro.Actividad
            Else
                tipoRegistro = RegistroAgenda.TipoRegistro.Ausencia
            End If

            PropiedadRegistroAgenda = New RegistroAgenda(IdRegistroAgenda,
                                                        UsuarioActual.IdentificadorUsuario,
                                                        autoriza,
                                                        tipoRegistro,
                                                        ddlTipoActividad.SelectedValue,
                                                        ddlTipoAusencia.SelectedValue,
                                                        chkCiclica.Checked,
                                                        fechaInicio,
                                                        fechaFin,
                                                        chkLunes.Checked,
                                                        chkMartes.Checked,
                                                        chkMiercoles.Checked,
                                                        chkJueves.Checked,
                                                        chkViernes.Checked,
                                                        txtDescripcionActividad.Text,
                                                        txtNotasAutorizador.Text,
                                                        True,
                                                        aprobado,
                                                        Nothing,
                                                        Nothing)

            valido = Agendar.ValidaHorarioDisponible(PropiedadRegistroAgenda, EsActualizacion)

            If valido Then
                If Agendar.NumeroDiasSeleccionados(PropiedadRegistroAgenda) = 0 Then
                    valido = False
                    Dim errores As New Entities.EtiquetaError(2066)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                End If

                If valido Then
                    valido = ValidaVacaciones()
                End If
            Else
                Dim errores As New Entities.EtiquetaError(2040)
                Mensaje = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If

            If Not valido Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            End If
        Catch ex As Exception
            valido = False
            Mensaje = ex.Source
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End Try

        If valido Then
            btnAceptarM2B1A.CommandArgument = "btnAceptar"

            If EsActualizacion Then
                Dim errores As New Entities.EtiquetaError(2045)
                Mensaje = errores.Descripcion
                imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            Else
                Dim errores As New Entities.EtiquetaError(2046)
                Mensaje = errores.Descripcion
                imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

        End If

    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        btnAceptarM2B1A.CommandArgument = "btnCancelar"
        Dim errores As New Entities.EtiquetaError(2078)
        Mensaje = errores.Descripcion
        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)        
    End Sub

    Private Sub btnAutorizar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAutorizar.Click
        Page.Validate("Forma")

        If Not Page.IsValid Then
            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim valido As Boolean = False
        Try

            PropiedadRegistroAgenda = New RegistroAgenda(IdRegistroAgenda)
            PropiedadRegistroAgenda.NotaAutorizador = txtNotasAutorizador.Text

            valido = Agendar.ValidaHorarioDisponible(PropiedadRegistroAgenda, EsActualizacion)

            If valido Then
                If Agendar.NumeroDiasSeleccionados(PropiedadRegistroAgenda) = 0 Then
                    valido = False
                    Dim errores As New Entities.EtiquetaError(2069)
                    Mensaje = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                End If

                If valido Then
                    valido = ValidaVacaciones()
                End If
            Else
                Dim errores As New Entities.EtiquetaError(2070)
                Mensaje = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            End If

            If Not valido Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            End If
        Catch ex As Exception
            valido = False
            Mensaje = ex.Source
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End Try

        If valido Then
            btnAceptarM2B1A.CommandArgument = "btnAutorizar"

            Dim errores As New Entities.EtiquetaError(2071)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

        End If
    End Sub

    Private Sub btnRechazar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRechazar.Click
        Page.Validate("Forma")

        If Not Page.IsValid Then
            Mensaje = String.Empty
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End If

        Dim valido As Boolean = False
        Try

            PropiedadRegistroAgenda = New RegistroAgenda(IdRegistroAgenda)
            PropiedadRegistroAgenda.NotaAutorizador = txtNotasAutorizador.Text

            valido = True
        Catch ex As Exception
            valido = False
            Mensaje = ex.Source
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End Try

        If valido Then
            btnAceptarM2B1A.CommandArgument = "btnRechazar"

            Dim errores As New Entities.EtiquetaError(2072)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

        End If
    End Sub

    Private Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptarM2B1A.Click
        Try
            Select Case btnAceptarM2B1A.CommandArgument
                Case "btnAceptar"
                    Dim objAgendar As New Agendar()
                    PropiedadRegistroAgenda.FechReg = Date.Now
                    If PropiedadRegistroAgenda.Aprobado Then
                        PropiedadRegistroAgenda.FechAutorizacion = Date.Now
                    End If
                    Dim guardo As Boolean = objAgendar.GuardarTarea(PropiedadRegistroAgenda, EsVacaciones)
                    If Not guardo Then
                        Dim errores As New Entities.EtiquetaError(2047)
                        Mensaje = errores.Descripcion
                        imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
                        Exit Sub
                    Else
                        If EsActualizacion Then
                            CorreoNotificacion(4)
                        Else
                            CorreoNotificacion(3)
                        End If
                    End If
                    Response.Redirect("BandejaRegistrarAgenda.aspx", True)
                Case "btnAutorizar"
                    Dim objAgendar As New Agendar()
                    PropiedadRegistroAgenda.Aprobado = True
                    PropiedadRegistroAgenda.FechAutorizacion = Date.Now

                    Dim guardo As Boolean = objAgendar.GuardarTarea(PropiedadRegistroAgenda, EsVacaciones)

                    If Not guardo Then
                        Dim errores As New Entities.EtiquetaError(2073)
                        Mensaje = errores.Descripcion
                        imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
                        Exit Sub
                    Else
                        CorreoNotificacion(1)
                    End If
                    Response.Redirect("BandejaRegistrarAgenda.aspx", True)
                Case "btnRechazar"
                        Dim objAgendar As New Agendar()
                        PropiedadRegistroAgenda.Aprobado = False
                        PropiedadRegistroAgenda.FechAutorizacion = Date.Now
                        PropiedadRegistroAgenda.Vigente = False

                        Dim guardo As Boolean = objAgendar.GuardarTarea(PropiedadRegistroAgenda, EsVacaciones)

                        If Not guardo Then
                            Dim errores As New Entities.EtiquetaError(2074)
                            Mensaje = errores.Descripcion
                            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
                            Exit Sub
                        Else
                            CorreoNotificacion(2)
                        End If
                        Response.Redirect("BandejaRegistrarAgenda.aspx", True)
                Case "btnCancelar"
                        Response.Redirect("BandejaRegistrarAgenda.aspx", True)
            End Select
        Catch ex As Exception
            Dim errores As New Entities.EtiquetaError(2048)
            Mensaje = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
            Exit Sub
        End Try
        
    End Sub

#End Region

#Region "Metodos"

    Private Sub CorreoNotificacion(ByVal Tipo As Integer)

        Dim ObjActividad As TipoActividad
        Dim objAusencia As TipoAusencia
        Dim requiereAutorizacion As Boolean = False

        If rblTipo.SelectedValue = 2 Then
            objAusencia = New TipoAusencia(ddlTipoAusencia.SelectedValue)
            requiereAutorizacion = objAusencia.RequiereAutorizacion
        ElseIf rblTipo.SelectedValue = 1 Then
            ObjActividad = New TipoActividad(ddlTipoActividad.SelectedValue)
            requiereAutorizacion = ObjActividad.RequiereAutorizacion
        End If


        Dim continua As Boolean = False
        Dim objCorreo As Entities.Correo

        Select Case Tipo
            Case 1
                objCorreo = New Entities.Correo(14)
            Case 2
                objCorreo = New Entities.Correo(15)
            Case 3
                objCorreo = New Entities.Correo(17)
            Case Else
                objCorreo = New Entities.Correo(18)
        End Select

        Dim usuario As Usuario = TryCast(Session(Entities.Usuario.SessionID), Usuario)
        Dim objEmail As New Utilerias.Mail
        If objCorreo.Vigencia Then
            Dim destinatarios As List(Of String) = New List(Of String)
            If (Convert.ToBoolean(WebConfigurationManager.AppSettings("Desarrollo").ToString()) = True) Then
                destinatarios.Add("david.perez@softtek.com")
                destinatarios.Add("victor.leyva@softtek.com")
                destinatarios.Add("ivan.rivera@softtek.com")
            Else
                destinatarios.Add(usuario.Mail)
                If requiereAutorizacion Then
                    destinatarios.Add(New Usuario(ddlFuncionario.SelectedValue).Mail)
                End If
                If ddlFuncionario.SelectedIndex <> 1 Then
                    destinatarios.Add(New Usuario(ddlFuncionario.Items(1).Value).Mail)
                End If
            End If
            Try
                objEmail.ServidorMail = WebConfigurationManager.AppSettings("MailServer").ToString()
                objEmail.Usuario = WebConfigurationManager.AppSettings("MailUsuario").ToString()
                objEmail.Password = Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("MailPass"))
                objEmail.Dominio = WebConfigurationManager.AppSettings("MailDominio").ToString()
                objEmail.DireccionRemitente = WebConfigurationManager.AppSettings("MailUsuario").ToString()
                objCorreo.Asunto = objCorreo.Asunto.Replace("[ACTIVIDAD]", txtDescripcionActividad.Text.Trim)
                objEmail.Asunto = objCorreo.Asunto
                objEmail.EsHTML = True
                objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[ACTIVIDAD]", txtDescripcionActividad.Text.Trim)
                If Tipo = 2 Then
                    objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[CAUSAS_RECHAZO]", txtNotasAutorizador.Text.Trim)
                End If
        objEmail.Mensaje = objCorreo.Cuerpo
        objEmail.Destinatarios = destinatarios
        objEmail.NombreAplicacion = WebConfigurationManager.AppSettings("EventLogSource").ToString()
        objEmail.EventLogSource = "ENVIAR_EMAIL"
        objEmail.Enviar()
            Catch ex As Exception

        End Try
        End If
    End Sub

    ''' <summary>
    ''' Carga los autorizadores para el usuario actual
    ''' </summary>
    ''' <remarks></remarks>
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
                                        nRow("T_DSC_NOMBRE_COMPLETO") = r2("T_DSC_NOMBRE") & " " & r2("T_DSC_APELLIDO")
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
    End Sub

    ''' <summary>
    ''' Valida que las vacaciones capturadas sean correctas
    ''' </summary>
    ''' <returns>True si la captura es correcta o si no hay captura de vacaciones, de lo contrario false</returns>
    ''' <remarks></remarks>
    Private Function ValidaVacaciones() As Boolean
        Dim valido As Boolean = True
        EsVacaciones = False
        If rblTipo.SelectedValue = 2 Then
            If ddlTipoAusencia.SelectedValue = 1 Then
                EsVacaciones = True
                Dim objVacaciones As New Vacaciones()
                Dim lstObjVacaciones As List(Of Vacaciones) = objVacaciones.ObtnenPeriodosActivos(PropiedadRegistroAgenda.IngenieroSolicta)

                Select Case lstObjVacaciones.Count
                    Case 0
                        Dim errores As New Entities.EtiquetaError(2036)
                        Mensaje = errores.Descripcion
                        imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                        valido = False
                    Case 1
                        valido = ValidaVacacionesDePeriodo(lstObjVacaciones(0))
                    Case 2
                        valido = ValidaVacacionesDePeriodo(lstObjVacaciones(1), lstObjVacaciones(0).DiasAsignados - lstObjVacaciones(0).DiasConsumidos)
                    Case Else
                        Dim numero As Integer = lstObjVacaciones.Count - 1
                        valido = ValidaVacacionesDePeriodo(lstObjVacaciones(numero), lstObjVacaciones(numero - 1).DiasAsignados - lstObjVacaciones(numero - 1).DiasConsumidos)
                End Select
            End If

        End If
        Return valido
    End Function

    ''' <summary>
    ''' Valida las vacaciones de un periodo
    ''' </summary>
    ''' <param name="objVacaciones"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidaVacacionesDePeriodo(ByVal objVacaciones As Vacaciones, Optional ByVal diasRestantesPeriodoAnterior As Integer = 0) As Boolean
        Dim valido As Boolean = True

        Dim diasSolicitados As Integer = Agendar.NumeroDiasSeleccionados(PropiedadRegistroAgenda) ' NumeroDiasVacaciones()
        Dim diasRestantes As Integer = objVacaciones.DiasAsignados - objVacaciones.DiasConsumidos + diasRestantesPeriodoAnterior
        If diasRestantes <= 0 Or diasRestantes < diasSolicitados Then
            Dim errores As New Entities.EtiquetaError(2037)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            valido = False
        End If

        'La regla establece que no se pueden gastar mas de ciertos dias antes de la mitad del periodo
        Dim objParametroFechaMitadPeriodo As New Parametros(38)
        Dim Mes As Int32 = Convert.ToInt32(objParametroFechaMitadPeriodo.ValorParametro.Substring(0, 2))
        Dim Dia As Int32 = Convert.ToInt32(objParametroFechaMitadPeriodo.ValorParametro.Substring(3, 2))

        Dim objParametroDias As New Parametros(39)
        Dim DiasAntesMedioPeriodo As Integer = Convert.ToInt32(objParametroDias.ValorParametro)

        Dim fechaMedioPeriodo As New Date(objVacaciones.InicioPeriodo.Year, Mes, Dia)

        Dim FechaSolicitudInicio As Date
        Dim FechaSolicitudFin As Date
        If chkCiclica.Checked Then
            FechaSolicitudInicio = Convert.ToDateTime(txtFechaInicioCiclico.Text)
            FechaSolicitudFin = Convert.ToDateTime(txtFechaFinCiclico.Text)
        Else
            FechaSolicitudInicio = Convert.ToDateTime(txtFechaInicioContinuo.Text)
            FechaSolicitudFin = Convert.ToDateTime(txtFechaFinContinuo.Text)
        End If

        If valido AndAlso FechaSolicitudFin < fechaMedioPeriodo AndAlso _
            (objVacaciones.DiasConsumidos - diasRestantesPeriodoAnterior) + diasSolicitados > DiasAntesMedioPeriodo Then

            Dim errores As New Entities.EtiquetaError(2038)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            valido = False
        End If

        'Dim DiasCapturadosAntesMedioPeriodo As Integer = NumeroDiasVacaciones(fechaMedioPeriodo.AddDays(-1)) + objVacaciones.DiasConsumidos - diasRestantesPeriodoAnterior
        Dim DiasCapturadosAntesMedioPeriodo As Integer = Agendar.NumeroDiasSeleccionados(PropiedadRegistroAgenda, fechaMedioPeriodo.AddDays(-1)) + objVacaciones.DiasConsumidos - diasRestantesPeriodoAnterior
        If valido AndAlso (FechaSolicitudInicio < fechaMedioPeriodo AndAlso FechaSolicitudFin >= fechaMedioPeriodo) _
            AndAlso (DiasCapturadosAntesMedioPeriodo > DiasAntesMedioPeriodo) Then
            Dim errores As New Entities.EtiquetaError(2038)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            valido = False
        End If

        'No pueden ser mas del numero maximo de dias contiguos
        Dim objParametroDiasContiguos As New Parametros(40)
        Dim DiasMaximosContiguos As Integer = Convert.ToInt32(objParametroDiasContiguos.ValorParametro)
        If valido AndAlso (Not chkCiclica.Checked OrElse (chkCiclica.Checked AndAlso (chkLunes.Checked AndAlso chkMartes.Checked _
                                                                                      AndAlso chkMiercoles.Checked AndAlso chkJueves.Checked _
                                                                                      AndAlso chkViernes.Checked))) _
            AndAlso diasSolicitados > DiasMaximosContiguos Then

            Dim errores As New Entities.EtiquetaError(2039)
            Mensaje = errores.Descripcion
            imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            valido = False

        End If

        Return valido
    End Function

#End Region

#Region "Validaciones"

    Private Sub csvDescripcionActividad_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvDescripcionActividad.ServerValidate
        If txtDescripcionActividad.Text.Trim.Length = 0 Then
            Dim errores As New Entities.EtiquetaError(2018)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        End If

        If txtDescripcionActividad.Text.Length > 1000 Then
            Dim errores As New Entities.EtiquetaError(2019)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        End If
    End Sub

    Private Sub csvFechaFinCiclico_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvFechaFinCiclico.ServerValidate
        'Solo valida si la bandera de ciclica esta seleccionada
        If chkCiclica.Checked Then
            Dim tmpDateFin As Date = Nothing
            Dim tmpDateInicio As Date = Nothing
            'Valida que se pueda convertir en una fecha
            If Not Date.TryParse(txtFechaFinCiclico.Text, tmpDateFin) Then
                Dim errores As New Entities.EtiquetaError(2020)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If

            'Valida que la fecha sea un dia habil
            If Not Agendar.EsDiaHabil(tmpDateFin) Then
                Dim errores As New Entities.EtiquetaError(2041)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If

            'Valida que sea mayor a la fecha de inicio
            If Date.TryParse(txtFechaInicioCiclico.Text, tmpDateInicio) Then
                If tmpDateFin <= tmpDateInicio Then
                    Dim errores As New Entities.EtiquetaError(2021)
                    source.ErrorMessage = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    args.IsValid = False
                    Exit Sub
                End If
            End If

        End If
    End Sub

    Private Sub csvFechaFinContinuo_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvFechaFinContinuo.ServerValidate
        'Solo valida si la bandera de ciclica esta seleccionada
        If Not chkCiclica.Checked Then
            Dim tmpDateFin As Date = Nothing
            Dim tmpDateInicio As Date = Nothing
            'Valida que se pueda convertir en una fecha
            If Not Date.TryParse(txtFechaFinContinuo.Text, tmpDateFin) Then
                Dim errores As New Entities.EtiquetaError(2022)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If

            'Valida que la fecha sea un dia habil
            If Not Agendar.EsDiaHabil(tmpDateFin) Then
                Dim errores As New Entities.EtiquetaError(2042)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If

            'Valida que sea mayor a la fecha de inicio
            If Date.TryParse(txtFechaInicioContinuo.Text, tmpDateInicio) Then
                If tmpDateFin < tmpDateInicio Then
                    Dim errores As New Entities.EtiquetaError(2023)
                    source.ErrorMessage = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    args.IsValid = False
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub csvFechaInicioCiclico_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvFechaInicioCiclico.ServerValidate
        If chkCiclica.Checked Then
            Dim tmpDateInicio As Date = Nothing
            'Valida que se pueda convertir en una fecha
            If Not Date.TryParse(txtFechaInicioCiclico.Text, tmpDateInicio) Then
                Dim errores As New Entities.EtiquetaError(2024)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If

            'Valida que la fecha sea un dia habil
            If Not Agendar.EsDiaHabil(tmpDateInicio) Then
                Dim errores As New Entities.EtiquetaError(2043)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If
        End If
    End Sub

    Private Sub csvFechaInicioContinuo_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvFechaInicioContinuo.ServerValidate
        If Not chkCiclica.Checked Then
            Dim tmpDateInicio As Date = Nothing
            'Valida que se pueda convertir en una fecha
            If Not Date.TryParse(txtFechaInicioContinuo.Text, tmpDateInicio) Then
                Dim errores As New Entities.EtiquetaError(2025)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If

            'Valida que la fecha sea un dia habil
            If Not Agendar.EsDiaHabil(tmpDateInicio) Then
                Dim errores As New Entities.EtiquetaError(2044)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If
        End If
    End Sub

    Private Sub csvFuncionario_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvFuncionario.ServerValidate
        If rblTipo.SelectedValue = 1 Then
            If ddlTipoActividad.SelectedValue <> -1 Then
                Dim objTipoActividad As New TipoActividad(ddlTipoActividad.SelectedValue)
                If objTipoActividad.RequiereAutorizacion Then
                    If ddlFuncionario.SelectedValue = "-1" Then
                        Dim errores As New Entities.EtiquetaError(2026)
                        source.ErrorMessage = errores.Descripcion
                        imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                        args.IsValid = False
                        Exit Sub
                    End If
                End If
            End If
        Else
            If ddlTipoAusencia.SelectedValue <> -1 Then
                Dim objTipoAusencia As New TipoAusencia(ddlTipoAusencia.SelectedValue)
                If objTipoAusencia.RequiereAutorizacion Then
                    If ddlFuncionario.SelectedValue = "-1" Then
                        Dim errores As New Entities.EtiquetaError(2026)
                        source.ErrorMessage = errores.Descripcion
                        imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                        args.IsValid = False
                        Exit Sub
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub csvHoraFinCiclico_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvHoraFinCiclico.ServerValidate
        If chkCiclica.Checked Then
            If ddlHoraFinCiclico.SelectedValue = -1 Then
                Dim errores As New Entities.EtiquetaError(2027)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If

            'Valida que la hora de inicio sea menor a la de termino
            If Convert.ToInt32(ddlHoraInicioCiclico.SelectedValue) >= Convert.ToInt32(ddlHoraFinCiclico.SelectedValue) Then
                Dim errores As New Entities.EtiquetaError(2029)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If

        End If
    End Sub

    Private Sub csvHoraFinContinuo_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvHoraFinContinuo.ServerValidate
        If Not chkCiclica.Checked Then
            If ddlHoraFinContinuo.SelectedValue = -1 Then
                Dim errores As New Entities.EtiquetaError(2028)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If

            'Si la fecha de inicio y fin es el mismo dia, la hora inicial debe ser menor a la hora final
            If txtFechaInicioContinuo.Text = txtFechaFinContinuo.Text Then
                If Convert.ToInt32(ddlHoraInicioContinuo.SelectedValue) >= Convert.ToInt32(ddlHoraFinContinuo.SelectedValue) Then
                    Dim errores As New Entities.EtiquetaError(2030)
                    source.ErrorMessage = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    args.IsValid = False
                    Exit Sub
                End If
            End If

        End If
    End Sub

    Private Sub csvHoraInicioCiclico_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvHoraInicioCiclico.ServerValidate
        If chkCiclica.Checked Then
            If ddlHoraInicioCiclico.SelectedValue = -1 Then
                Dim errores As New Entities.EtiquetaError(2031)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If
        End If
    End Sub

    Private Sub csvHoraInicioContinuo_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvHoraInicioContinuo.ServerValidate
        If Not chkCiclica.Checked Then
            If ddlHoraInicioContinuo.SelectedValue = -1 Then
                Dim errores As New Entities.EtiquetaError(2032)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If
        End If
    End Sub

    Private Sub csvNotasAutorizador_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvNotasAutorizador.ServerValidate
        If EsAutorizacion Then
            If txtNotasAutorizador.Text.Trim.Length() = 0 Then
                Dim errores As New Entities.EtiquetaError(2033)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If
        End If
    End Sub

    Private Sub csvTipo_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvTipo.ServerValidate
        If rblTipo.SelectedValue <> 1 And rblTipo.SelectedValue <> 2 Then
            Dim errores As New Entities.EtiquetaError(2017)
            source.ErrorMessage = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            args.IsValid = False
            Exit Sub
        End If
    End Sub

    Private Sub csvTipoActividad_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvTipoActividad.ServerValidate
        If rblTipo.SelectedValue = 1 Then
            If ddlTipoActividad.SelectedValue = -1 Then
                Dim errores As New Entities.EtiquetaError(2034)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If
        End If
    End Sub

    Private Sub csvTipoAusencia_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles csvTipoAusencia.ServerValidate
        If rblTipo.SelectedValue = 2 Then
            If ddlTipoAusencia.SelectedValue = -1 Then
                Dim errores As New Entities.EtiquetaError(2035)
                source.ErrorMessage = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                args.IsValid = False
                Exit Sub
            End If
        End If
    End Sub
#End Region

End Class