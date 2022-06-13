Imports Entities
Imports System.Web.Configuration

Public Class DetallePC
    Inherits System.Web.UI.Page

    Public ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property

    Public ReadOnly Property SubProcesoView
        Get
            Return Session("SUB_PROCESO")
        End Get
    End Property

    Public ReadOnly Property Usuario
        Get
         Return Session("ID_USR")
      End Get
    End Property

    Public ReadOnly Property EstatusPC
        Get
            Dim PC As New Entities.PC(Folio)
            PC = Session("PC")
            Return PC.IdEstatus
        End Get
    End Property
    Public ReadOnly Property EstatusPCant
        Get
            Dim PC As New Entities.PC(Folio)
            PC = Session("PC")
            Return PC.IdEstatusAnt
        End Get
    End Property
    Public ReadOnly Property IdResolucion
        Get
            Dim PC As New Entities.PC(Folio)
            PC = Session("PC")
            Return PC.IdResolucion
        End Get
    End Property

    Public ReadOnly Property IdArea
        Get
            Dim usuario As New Entities.Usuario()
            Return usuario.IdArea
        End Get
    End Property

    Public Property ShwAct As String
    Public Property puObjUsuario As Entities.Usuario
        Get
            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                Return CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Entities.Usuario)
            Session(Entities.Usuario.SessionID) = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    Dim PC As New Entities.PC(Folio)
        Session("PC") = PC
        If Not Page.IsPostBack Then
            '****************ESTOS CONTROLES SIEMPRE SE EJECUTAN Y SON VISIBLES*******************
            DetalleSICOD1.Inicializar()
            Bitacora1.Inicializar()
            '*************************************************************************************
            lblFolio.Text = PC.FolioSupervisar
            lblPaso.Text = "Paso " + PC.IdPaso.ToString() + ": " + ObtenerDCSPaso(PC.IdPaso.ToString())
            Supervisor1.Cargar()
            pnlSupervisores.Enabled = False
            PnlInspector.Enabled = False

            For Each cntrl As Control In pnlDetalleSICOD.Controls
                If cntrl.TemplateControl.ToString() = "ASP.pc_usercontrols_detallesicod_ascx" Then
                    For Each cntrlD As Control In cntrl.Controls
                        If cntrlD.ID.Equals("pnlRegistro") Then
                            For Each cntrlD2 As Control In cntrlD.Controls
                                If TypeOf cntrlD2 Is Panel Then
                                    If cntrlD2.ID.Equals("pnlEnabled") Then
                                        'Array.ForEach(cntrlD2.Controls.OfType(Of Control), Sub(x As Button) x.Enabled = False)
                                        'yourButton.Enabled = True
                                        For Each cntrlD3 As Control In cntrlD2.Controls
                                            If TypeOf cntrlD3 Is Button Then
                                                If cntrlD3.ID.Equals("btnVerDoc") Then
                                                    Dim btn As Button = DirectCast(cntrlD3, Button)
                                                    btn.Enabled = True
                                                End If
                                            End If
                                            Exit For
                                        Next
                                        Exit For
                                    End If
                                End If
                            Next
                            Exit For
                        End If
                    Next
                    Exit For
                End If

            Next

            SancionPC.Visible = False
            'FUNCION CARGA PASOS
            CargaPasos()
        Else
            ''FUNCION CARGA PASOS
            CargaPasos()
        End If

    End Sub

    Function CargaPasos()
        Dim PC As New Entities.PC(Folio)

        If (PC.IdPaso >= 2 And PC.IdResolucion <> 3) Or PC.IdEstatus = 108 Then
            Dim dt_Actividad As DataTable = Actividad.ObtenerTodas(Folio)
            If (dt_Actividad.Rows.Count > 0) Then
                pnlActiv.Visible = True
                Dim ac As Actividades = LoadControl("UserControls/Actividades.ascx")
                pnlActiv.Controls.Add(ac)
                ac.InicializarR()
                ShwAct = "true"
            Else
                ShwAct = "false"
            End If
        Else
            ShwAct = "false"
        End If

        VisibilidadTabDatSisan(False)

        Select Case PC.IdPaso
            Case 1

                Select Case PC.IdEstatus

                    Case 0
                        pnlSupervisores.Enabled = True

                    Case 1
                        'Paso en el cual el supervisor tiene que aceptar o rechazar el PC
                        pnlSupervisores.Enabled = False
                        'Se Oculta el control de inspectores e Irregularidades
                        Inspector.Visible = False
                        'Se le asigna al boton guardar el metodo para acutalizar el estatus del PC
                        btnGuardar.Attributes.Add("onclick", "AceptarPC();")
                        pnlPaso1.Visible = True

                    Case 2
                        hrIns.Visible = True
                        Supervisor1.Cargar()
                        'Se muestra el control de inspectores
                        Inspector.Visible = True
                        'Se oculta irregularidad
                        pnlIrregularidad.Visible = False

                        ViewState("ValidationGroup") = "ucInspector"
                        'ValidationSummaryGeneral.Attributes.Add("ValidationGroup", "ucInspector")
                        ValidationSummaryGeneral.ValidationGroup = "ucInspector"

                        'Se asigna al boton guardar el metodo de guardar inspectores
                        btnGuardar.Attributes.Add("onclick", "ActulizarSupervisor_GuardarInspector();")
                        PnlBtnInspector.Visible = False
                        PnlInspector.Enabled = True
                        pnlPaso1.Visible = False
                        pnlPaso1_1.Visible = True
                        pnlSupervisores.Enabled = True

                    Case 4
                        ValidationSummaryGeneral.Enabled = False
                        Supervisor1.Cargar()
                        'Se muestra el control de inspectores
                        Inspector.Visible = True
                        Inspector.Inicializar()
                        PnlInspector.Enabled = False
                        pnlSupervisores.Enabled = False
                        'Se oculta irregularidad
                        pnlIrregularidad.Visible = False
                        btnReasignarP1_E1.Visible = False
                        pnlPaso1_1.Visible = True
                        ' btnAceptarP1_E1.Visible = True

                        btnAceptarP1_E1.Visible = True
                        btnReasignarP1_E1.Attributes.Remove("onclick")
                        btnGuardar.Attributes.Add("onclick", "GuardarInspectorInfo_Status(); GuardarBitacora(" + Folio.ToString() + ",'" + Usuario + "','" + PC.IdPaso.ToString() + "','Se notifica a supervisores e inspectores asignados.',' ');")
                        'Se asigna al boton guardar el metodo de guardar inspectores
                    Case Else
                        pnlIrregularidad.Visible = False

                End Select

            Case 2
        lblNota.Text = " Nota: Se habilitará documento de nota de cancelación en el expediente. "
                Select Case PC.IdEstatus
                    Case 4
                        hrIrr.Visible = True
                        Dim uc As Irregularidad = LoadControl("UserControls/Irregularidad.ascx")
                        pnlIrreg.Controls.Add(uc)
                        uc.Inicializar()
                        ValidationSummaryGeneral.ValidationGroup = "ucIrregularidad"
                        pnlIrregularidad.Visible = True
                        pnlPaso2.Visible = True
                        pnlPaso2.Enabled = True



                        btnGuardarIrregularidad.Attributes.Add("onclick", "GuardarBitacora(" + Folio.ToString() + ",'" + Usuario + "','" + PC.IdPaso.ToString() + "','Se agregan las primeras irregularidades',' '); CambiaEstatusFolio(2,5);")
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) Then
                            btnCancelarPC_P2.Visible = True
                            btnCancelarPC.Attributes.Add("onclick", "_SolicitarCancelacion_e()")
                        End If

                    Case 5
                        Checklist1.Visible = True
                        Checklist1.Inicializar()
                        'ViewState("ValidationGroup") = "ucResolucion" ' MMOB - VALIDACIÓN CUANDO ES "NO"
                        ViewState("ValidationGroup") = "ucResolucionNO"
                        ValidationSummaryGeneral.Enabled = True
                        ValidationSummaryGeneral.ValidationGroup = "ucResolucionNO"
                        btnNotifica_tab4.Visible = False
                        btnEnvio_tab4.Visible = False
                        Checklist1.ControlesResolucion = False
                        ValidationSummaryGeneral.Enabled = False
                        btnADetalle_tab4.Visible = False
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        btnCancelarPC_P2_A.Visible = True
                        btnCancelarPC.Attributes.Add("onclick", "_SolicitarCancelacion_e()")
                        btnGuardar.Attributes.Add("onclick", "GuardarCheckList(); GuardarBitacora(" + Folio.ToString() + ",'" + Usuario + "','" + PC.IdPaso.ToString() + "','Se determina si el PC cumplío o no con el checklist',' ' );")
                        pnlIrr.Visible = True
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.Inicializar()

                    Case 6
                        Checklist1.Visible = True
                        Checklist1.InicializarFolio()
                        btnNotifica_tab4.Visible = False
                        btnEnvio_tab4.Visible = False
                        Checklist1.ControlesResolucion = False
                        ViewState("ValidationGroup") = "ucIrregularidad"
                        ValidationSummaryGeneral.Enabled = True
                        ValidationSummaryGeneral.ValidationGroup = "ucIrregularidad"
                        btnADetalle_tab4.Visible = True
                        btnADetalle_tab4.Attributes.Add("onclick", "Requerimientos();")
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        btnGuardar.Attributes.Add("onclick", "")
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) Then
                            btnCancelarPC_P2_A.Visible = True
                            btnCancelarPC.Attributes.Add("onclick", "_SolicitarCancelacion_e()")
                        End If

                        pnlIrr.Visible = True
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.Inicializar()


                    Case 7
                        Checklist1.Visible = True
                        Checklist1.InicializarFolioRes("MensajeGuardaResolucion();", 1)
                        ViewState("ValidationGroup") = "ucResolucion"
                        ValidationSummaryGeneral.ValidationGroup = "ucResolucion"
                        ValidationSummaryGeneral.Enabled = True
                        btnADetalle_tab4.Visible = False
                        Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = True
                        btnADetalle_tab4.Visible = False
                        btnNotifica_tab4.Visible = False
                        btnEnvio_tab4.Visible = False
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        btnGuardar.Attributes.Add("onclick", "GuardarResolucionP3();")
                        btnNotifica_tab4.Visible = False
                        pnlIrr.Visible = True
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) Then
                            btnCancelarPC_P2_A.Visible = True
                            btnCancelarPC.Attributes.Add("onclick", "_SolicitarCancelacion_e()")
                        End If
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.Inicializar()

                    Case 8
                        Checklist1.Visible = True
                        Checklist1.InicializarFolio()
                        btnNotifica_tab4.Visible = False
                        btnEnvio_tab4.Visible = False
                        Checklist1.ControlesResolucion = False
                        ViewState("ValidationGroup") = "ucIrregularidad"
                        ValidationSummaryGeneral.Enabled = True
                        ValidationSummaryGeneral.ValidationGroup = "ucIrregularidad"
                        btnADetalle_tab4.Visible = False
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        btnGuardar.Attributes.Add("onclick", "")
                        pnlIrr.Visible = True
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) Then
                            btnCancelarPC_P2_A.Visible = True
                            btnCancelarPC.Attributes.Add("onclick", "_SolicitarCancelacion_e()")
                        End If
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.Inicializar()

                    Case 9
                        Checklist1.Visible = True
                        Checklist1.InicializarFolioRes("MensajeGuardaResolucion();", 1)
                        ViewState("ValidationGroup") = "ucResolucion"
                        ValidationSummaryGeneral.ValidationGroup = "ucResolucion"
                        ValidationSummaryGeneral.Enabled = True
                        btnADetalle_tab4.Visible = False
                        Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = True
                        btnADetalle_tab4.Visible = False
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = True
                        btnGuardar.Attributes.Add("onclick", "GuardarResolucionP3();")
                        btnNotifica_tab4.Visible = False
                        btnEnvio_tab4.Visible = False
                        pnlIrr.Visible = True
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) Then
                            btnCancelarPC_P2_A.Visible = True
                            btnCancelarPC.Attributes.Add("onclick", "_SolicitarCancelacion_e()")
                        End If
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.Inicializar()

                    Case 101
                        Checklist1.Visible = True
                        pnlIrr.Visible = True
                        Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = True
                        Checklist1.InicializarFolioResOk()
                        ViewState("ValidationGroup") = "ucResolucion"
                        ValidationSummaryGeneral.ValidationGroup = "ucResolucion"
                        ValidationSummaryGeneral.Enabled = True
                        btnADetalle_tab4.Visible = False
                        btnADetalle_tab4.Visible = False
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        btnGuardar.Attributes.Add("onclick", "GuardarResolucionP3();")
                        btnNotifica_tab4.Visible = False
                        btnEnvio_tab4.Visible = False
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) Then
                            btnCancelarPC_P2_A.Visible = True
                            btnCancelarPC.Attributes.Add("onclick", "_SolicitarCancelacion_e()")
                        End If
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.InicializarR()



                    Case 10
                        ValidationSummaryGeneral.Enabled = False
                        btnADetalle_tab4.Visible = False
                        btnNotifica_tab4.Visible = True
                        Checklist1.Visible = True
                        Checklist1.InicializarFolioResOk()
                        Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = False
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        btnGuardar.Attributes.Add("onclick", "CambiaEstatusFolio(3,102); GuardarBitacora(" + Folio.ToString() + ",'" + Usuario + "','" + PC.IdPaso.ToString() + "','Se realiza resolución', );") ' MMOB  ANTES "CambiaEstatusFolio(2,10)" AHORA "CambiaEstatusFolio(3,102)"
                        btnNotifica_tab4.Disabled = False
                        BtnAceptarTab4.Visible = False
                        btnEnvio_tab4.Visible = False
                        pnlIrr.Visible = True
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.InicializarR()

                    Case 13
                        ValidationSummaryGeneral.Enabled = False
                        btnADetalle_tab4.Visible = False
                        btnNotifica_tab4.Visible = False
                        Checklist1.Visible = True
                        Checklist1.InicializarFolioResOk()
                        Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = False
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        btnNotifica_tab4.Disabled = False
                        BtnAceptarTab4.Visible = False
                        btnEnvio_tab4.Visible = False

                    Case 14
                        ValidationSummaryGeneral.Enabled = False
                        btnADetalle_tab4.Visible = False
                        btnNotifica_tab4.Visible = True
                        Checklist1.Visible = True
                        Checklist1.InicializarFolioResOk()
                        Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = False
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        btnGuardar.Attributes.Add("onclick", "CambiaEstatusFolio(2,13); GuardarBitacora(" + Folio.ToString() + ",'" + Usuario + "','" + PC.IdPaso.ToString() + "','Se notifica resolución');")
                        btnNotifica_tab4.Disabled = False
                        BtnAceptarTab4.Visible = False
                        btnEnvio_tab4.Visible = False

                    Case 21
                        lblPaso.Text = "Paso 2: En proceso de cancelación"
                        ValidationSummaryGeneral.Enabled = False
                        btnADetalle_tab4.Visible = False
                        BtnAceptarTab4.Visible = False
                        btnNotifica_tab4.Visible = False
                        Checklist1.Visible = True
                  Checklist1.InicializarFolio()
                  Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = False
                        Checklist1.ControlesComentariosVisible = True
                        Checklist1.ControlesComentariosHabilitados = False
                        pnlIrr.Visible = True
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        btnEnvio_tab4.Visible = False

                        btnCancelarPC_P2.Visible = False
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM AndAlso
                            puObjUsuario.IdArea = Constantes.AREA_PR) Then
                            btnAprobarCancelarPC_P2.Visible = True
                            btnRechazarPC_P2.Visible = True
                        End If

                        pnlIrr.Visible = True
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.Inicializar()


               Case 22
                        lblPaso.Text = "Paso 2: Cancelado"
                        ValidationSummaryGeneral.Enabled = False
                        btnADetalle_tab4.Visible = False
                        BtnAceptarTab4.Visible = False
                        btnNotifica_tab4.Visible = False
                        Checklist1.Visible = True
                        Checklist1.InicializarFolioResOk()
                        Checklist1.ControlesResolucion = False
                        Checklist1.ControlesResolucionHabiliado = False
                        Checklist1.ControlesComentariosVisible = True
                        Checklist1.ControlesComentariosHabilitados = False
                        pnlIrr.Visible = True
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        btnEnvio_tab4.Visible = False
                        btnCancelarPC_P2.Visible = False
                        btnAprobarCancelarPC_P2.Visible = False
                        btnRechazarPC_P2.Visible = False

                End Select

            Case 3
                Select Case PC.IdEstatus

                    Case 102
                        ValidationSummaryGeneral.Enabled = False
                        btnADetalle_tab4.Visible = False
                        btnNotifica_tab4.Visible = True
                        pnlIrr.Visible = True
                        Checklist1.Visible = True
                        Checklist1.InicializarFolioResOk()
                        Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = False
                        Checklist1.ControlesComentariosVisible = True
                        Checklist1.ControlesComentariosHabilitados = True
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False

                        btnGuardar.Attributes.Add("onclick", "GuardaComentario();")
                        btnNotifica_tab4.Visible = False
                        btnEnvio_tab4.Visible = False
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.InicializarR()


                    Case 103

                        ValidationSummaryGeneral.Enabled = False
                        btnADetalle_tab4.Visible = False
                        btnNotifica_tab4.Visible = True
                        Checklist1.Visible = True
                        Checklist1.InicializarFolioResOk()
                        Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = False
                        Checklist1.ControlesComentariosVisible = True
                        Checklist1.ControlesComentariosHabilitados = False
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        pnlIrr.Visible = True
                        btnSicodMensg.Attributes.Add("onclick", "")
                        If PC.IdResolucion = 2 Or PC.IdResolucion = 1 Then 'MMOB NUEVA VALIDACION "Si es Procede o no procede" 
                            btnGuardar.Attributes.Add("onclick", "CambiaEstatusFolio(4,105); GuardarBitacora(" + Folio.ToString() + ",'" + Usuario + "','" + PC.IdPaso.ToString() + "','Se guarda dictamen');")
                            btnEnvio_tab4.Visible = False
                        Else
                            If PC.IdResolucion = 4 Then
                                btnGuardar.Attributes.Add("onclick", "RegistraOPI();")
                                btnEnvio_tab4.Visible = False
                            Else
                                btnGuardar.Attributes.Add("onclick", "CambiaEstatusFolio(3,104); GuardarBitacora(" + Folio.ToString() + ",'" + Usuario + "','" + PC.IdPaso.ToString() + "','Se notifica primer resolución');")
                                btnEnvio_tab4.Visible = False
                            End If
                        End If

                        btnNotifica_tab4.Visible = True
                        BtnAceptarTab4.Visible = False
                        'btnEnvio_tab4.Visible = True
                        'btnEnvio_tab4.Visible = False
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.InicializarR()



                    Case 104
                        ValidationSummaryGeneral.Enabled = False
                        btnADetalle_tab4.Visible = False
                        BtnAceptarTab4.Visible = True
                        btnNotifica_tab4.Visible = False
                        Checklist1.Visible = True
                        Checklist1.InicializarFolioResOk()
                        Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = False
                        Checklist1.ControlesComentariosVisible = True
                        Checklist1.ControlesComentariosHabilitados = False
                        pnlIrr.Visible = True
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        btnSicodMensg.Attributes.Add("onclick", "")


                        If (PC.IdResolucion = 3) Then
                            btnGuardar.Attributes.Add("onclick", "GuardarActividades();")
                            btnEnvio_tab4.Visible = False
                            pnlActiv.Visible = True
                            Dim ac As Actividades = LoadControl("UserControls/Actividades.ascx")
                            pnlActiv.Controls.Add(ac)
                            ac.Inicializar()
                        End If
                        If (PC.IdResolucion = 4) Then
                            BtnAceptarTab4.Visible = False
                            btnGuardar.Visible = False
                            btnEnvio_tab4.Visible = False
                        End If
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.InicializarR()

                    Case 108
                        Checklist1.Visible = True
                        Checklist1.InicializarFolioRes("MensajeGuardaResolucion();", 1)
                        ViewState("ValidationGroup") = "ucResolucion"
                        ValidationSummaryGeneral.ValidationGroup = "ucResolucion"
                        ValidationSummaryGeneral.Enabled = True
                        btnADetalle_tab4.Visible = False
                        Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = True
                        btnADetalle_tab4.Visible = False
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        btnGuardar.Attributes.Add("onclick", "GuardarResolucionP3(); GuardarBitacora(" + Folio.ToString() + ",'" + Usuario + "','" + PC.IdPaso.ToString() + "','Se notifica segunda resolución');")
                        btnNotifica_tab4.Visible = False
                        btnEnvio_tab4.Visible = False
                        pnlIrr.Visible = True
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.InicializarR()

                End Select

            Case 4 ' MMOB - aqui debe de solicitar el documento de dictamen
                Select Case PC.IdEstatus
                    Case 105 'MMOB - NUEVO CASO
                        lblPaso.Text = "Paso 4: En espera de Dictamen"
                        ValidationSummaryGeneral.Enabled = False
                        btnADetalle_tab4.Visible = False
                        btnNotifica_tab4.Visible = True
                        Checklist1.Visible = True
                        Checklist1.InicializarFolioResOk()
                        Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = False
                        Checklist1.ControlesComentariosVisible = True
                        Checklist1.ControlesComentariosHabilitados = True
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        pnlIrr.Visible = True
                        If PC.IdResolucion = 2 Or PC.IdResolucion = 1 Or PC.IdResolucion = 4 Then 'MMOB NUEVA VALIDACIÓN "Si es Procede o no procede"
                            btnGuardar.Attributes.Add("onclick", "GuardaComentarioDictamen()")
                            btnEnvio_tab4.Visible = False
                            btnNotifica_tab4.Visible = False
                            BtnAceptarTab4.Visible = True
                        Else
                            btnNotifica_tab4.Visible = True
                            BtnAceptarTab4.Visible = False
                        End If

                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.InicializarR()

                    Case 106 'MMOB - NUEVO CASO para poder enviar a sanción
                        ValidationSummaryGeneral.Enabled = False
                        btnADetalle_tab4.Visible = False
                        btnNotifica_tab4.Visible = True
                        Checklist1.Visible = True
                        Checklist1.InicializarFolioResOk()
                        Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = False
                        Checklist1.ControlesComentariosVisible = True
                        Checklist1.ControlesComentariosHabilitados = True
                        pnlPaso2.Visible = False
                        pnlIrr.Visible = True
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        If PC.IdResolucion = 2 Or PC.IdResolucion = 1 Or PC.IdResolucion = 4 Then 'Si es Procede O  No Procede
                            btnGuardar.Attributes.Add("onclick", "ObtenerValidacionSISAN()")
                            btnEnvio_tab4.Visible = True
                            btnNotifica_tab4.Visible = False
                            btnADetalle_tab4.Visible = False
                        Else
                            btnNotifica_tab4.Visible = True
                        End If
                        If PC.IdResolucion = 1 Or PC.IdResolucion = 4 Then 'Si es Procede O  No Procede
                            Checklist1.VisibleRequerimientos()
                        End If

                        BtnAceptarTab4.Visible = False
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.InicializarR()
                End Select

            Case 5
                Select Case PC.IdEstatus
                    Case 15 'MMOB - NUEVO CASO cuando ya se envió a SISAN
                        VisibilidadTabDatSisan(True, PC.FolioSisan)
                        ValidationSummaryGeneral.Enabled = False
                        btnADetalle_tab4.Visible = False
                        btnNotifica_tab4.Visible = True
                        Checklist1.Visible = True
                        pnlIrr.Visible = True
                        Checklist1.InicializarFolioResOk()
                        Checklist1.ControlesResolucion = True
                        Checklist1.ControlesResolucionHabiliado = False
                        Checklist1.ControlesComentariosVisible = True
                        Checklist1.ControlesComentariosHabilitados = False
                        pnlPaso2.Visible = False
                        pnlSupervisores.Enabled = False
                        pnlIrregularidad.Visible = False
                        btnSicodMensg.Attributes.Add("onclick", "")
                        btnGuardar.Attributes.Add("onclick", "")
                        btnNotifica_tab4.Visible = False
                        btnEnvio_tab4.Visible = False
                        BtnAceptarTab4.Visible = False
                        Dim uc As Irregularidades = LoadControl("UserControls/Irregularidades.ascx")
                        pnlIrr.Controls.Add(uc)
                        uc.InicializarR()
                End Select
        End Select
    End Function
    Private Sub VisibilidadTabDatSisan(isVisible As Boolean, Optional folioSisan As String = "")
        If isVisible Then
            If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then

                liExpedientes.Visible = True
                tabExpedienteSISAN.Visible = True
                SancionPC1.Visible = True
                SancionPC1.Inicializar(folioSisan, True)

            End If
        Else
            liExpedientes.Visible = False
            tabExpedienteSISAN.Visible = False
            SancionPC1.Visible = False
        End If
    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function AprobacionPC(Folio As Integer) As Boolean
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

    ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(1)
    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(2)
        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(1)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)

        Try
            Dim Notifica As NotificacionesPC = New NotificacionesPC
            Dim usuario As New Entities.Usuario()
            usuario = HttpContext.Current.Session(Entities.Usuario.SessionID)
            Notifica.Folio = Folio
            Notifica.Usuario = usuario.Nombre + " " + usuario.Apellido + " " + usuario.ApellidoAuxiliar
            Notifica.NotificarCorreo(98)
        Catch ex As Exception

        End Try

        Return BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)

    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function SendToOPI(Folio As Integer) As String
        Dim DatosPC As New Entities.PC(Folio)
        Dim Res As String = String.Empty
        Dim Msj As String = String.Empty
        Dim FolioOPI As String = String.Empty

        If RegistroOPI.GuardarOPIfromPC(Folio, DatosPC, FolioOPI) Then
            Dim ListaCampos As New List(Of String)
            Dim ListaValores As New List(Of Object)
            Dim ListaCamposCondicion As New List(Of String)
            Dim ListaValoresCondicion As New List(Of Object)

      ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(103)
      ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(104)
            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(3)
            ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)
            If BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion) Then
                Res = "True"
                Msj = "Se ha registrado el Oficio de Observaciones para éste programa de corrección, por favor revisa en la bandeja de Oficios el folio: " & FolioOPI
            Else
                Res = "False"
                Msj = "Se ha registrado el Oficio de Observaciones para éste programa de corrección, por favor revisa en la bandeja de Oficios el folio: " & FolioOPI & ", contacte al administrador."
            End If
        Else
            Res = "False"
            Msj = "Error al registrar el registro en OPI"
        End If
        Return Res & "|" & Msj
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function Notifica(Folio As Integer, Paso As Integer, Estatus As Integer) As Boolean
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

        ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(4)
        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(Estatus)
        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(Paso)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)

        Try
            Dim Notif As NotificacionesPC = New NotificacionesPC
            Dim usuario As New Entities.Usuario()
            usuario = HttpContext.Current.Session(Entities.Usuario.SessionID)
            Notif.Folio = Folio
            Notif.Usuario = usuario.Nombre + " " + usuario.Apellido + " " + usuario.ApellidoAuxiliar

            Select Case Estatus
                Case 8
                    Notif.NotificarCorreo(102)
                Case 9
                    Notif.NotificarCorreo(103)
                Case 102
                    Notif.NotificarCorreo(104)
                Case 103
                    Notif.NotificarCorreo(104)
                Case 104
                    Notif.NotificarCorreo(104)
            End Select


        Catch ex As Exception

        End Try

        Return BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)

    End Function

    Public Shared Function ObtenerDCSPaso(Paso As String) As String

        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT [T_DSC_PASO] FROM [dbo].[BDS_C_PC_PASOS] WHERE  N_ID_PASO =  " & Paso & " AND B_FALG_VIGENTE = 1")

        conexion.CerrarConexion()

        Dim rows As String = ""
        If data.Rows.Count > 0 Then
            rows = data.Rows(0).ItemArray(0).ToString()
        End If

        Return rows.ToString()
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerPreguntas() As List(Of ListItem)
        Dim preguntas As DataTable = Entities.Checklist.Preguntas


        Dim respuesta As New List(Of ListItem)()

        For Each row As Data.DataRow In preguntas.Rows
            respuesta.Add(New ListItem() With {
                              .Value = row("I_ID_CHECKLIST").ToString(),
                              .Text = row("T_DSC_PREGUNTA").ToString()})

        Next

        Return respuesta
    End Function

    Public Class Check
        Public Property Value As String
        Public Property Text As String
        Public Property Check As String

        Public Sub New()

        End Sub

        Public Sub New(ByVal valuei As String,
                       ByVal texti As String,
                       ByVal checki As String)
            Value = valuei
            Text = texti
            Check = checki
        End Sub
    End Class

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerPreguntasFolio(Folio As Integer) As List(Of Check)
        Dim preguntas As DataTable = Entities.Checklist.PreguntasFolio(Folio)


        Dim respuesta As New List(Of Check)()

        For Each row As Data.DataRow In preguntas.Rows

            respuesta.Add(New Check(row("I_ID_CHECKLIST").ToString(), row("T_DSC_PREGUNTA").ToString(), row("B_CHECK").ToString()))

        Next

        Return respuesta
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerSubprocesos(Proceso As String) As List(Of ListItem)
        Dim dsSubprocesos As DataTable = ConexionSISAN.ObtenerSubprocesos(Proceso)


        Dim subprocesos As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSubprocesos.Rows
            subprocesos.Add(New ListItem() With {
                          .Value = row("ID_SUBPROCESO").ToString(),
                          .Text = row("DESC_SUBPROCESO").ToString()})

        Next

        Return subprocesos
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerIrregularidadesSISAN(Proceso As Integer, SubProceso As Integer, Conducta As Integer) As List(Of ListItem)
        Dim dsSubprocesos As DataTable = ConexionSISAN.ObtenerIrregularidades(Proceso, SubProceso, Conducta)


        Dim subprocesos As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSubprocesos.Rows
            subprocesos.Add(New ListItem() With {
                          .Value = row("ID_IRREGULARIDAD").ToString(),
                          .Text = row("DESC_IRREGULARIDAD").ToString()})

        Next

        Return subprocesos
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerConducta(Proceso As String, SubProceso As String) As List(Of ListItem)
        Dim dsSubprocesos As DataTable = ConexionSISAN.ObtenerConducta(Proceso, SubProceso)


        Dim subprocesos As New List(Of ListItem)()

        For Each row As Data.DataRow In dsSubprocesos.Rows
            subprocesos.Add(New ListItem() With {
                          .Value = row("ID_CONDUCTA").ToString(),
                          .Text = row("DESC_CONDUCTA").ToString()})

        Next

        Return subprocesos
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerIrregularidades(Folio As Integer) As List(Of Entities.Irregularidad.GridView)
        Dim dtIrregularidades As DataTable = Entities.Irregularidad.ObtenerTodas(Folio)

        Dim Irregularidades As New List(Of Entities.Irregularidad.GridView)

        For index = 0 To dtIrregularidades.Rows.Count - 1
            Dim Irregularidad As New Entities.Irregularidad.GridView()
            Irregularidad.IdIrregularidad = dtIrregularidades(index)("I_ID_IRREGULARIDAD")
            Irregularidad.Numero = dtIrregularidades(index)("Row#")
            Irregularidad.Fecha = dtIrregularidades(index)("F_FECH_IRREGULARIDAD")
            Irregularidad.Proceso = dtIrregularidades(index)("DESC_PROCESO")
            Irregularidad.Subproceso = dtIrregularidades(index)("DESC_SUBPROCESO")
            Irregularidad.Conducta = dtIrregularidades(index)("DESC_CONDUCTA")
            Irregularidad.Irregularidad = dtIrregularidades(index)("DESC_IRREGULARIDAD")
            Irregularidad.Participante = dtIrregularidades(index)("DESC_PARTICIPANTE")
            Irregularidad.Gravedad = dtIrregularidades(index)("DESC_GRAVEDAD")
            Irregularidad.Comentarios = dtIrregularidades(index)("T_DSC_COMENTARIO")
            Irregularidades.Add(Irregularidad)
        Next

        Return Irregularidades

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarIrregularidad(Folio As Integer, Fecha As String, Proceso As String, SubProceso As String, Conducta As String, Irregularidad As String, Comentarios As String) As Boolean
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)

        With lstCampos
            .Add("N_ID_FOLIO")
            .Add("F_FECH_IRREGULARIDAD")
            .Add("I_ID_PROCESO")
            .Add("I_ID_SUBPROCESO")
            .Add("I_ID_CONDUCTA")
            .Add("I_ID_IRREGULARIDAD_POR_SANCIONAR")
            .Add("T_DSC_COMENTARIO")
        End With

        With lstValores
            .Add(Folio)
            .Add(Date.ParseExact(Fecha, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
            .Add(Proceso)
            .Add(SubProceso)
            .Add(Conducta)
            .Add(Irregularidad)
            .Add(Comentarios)

        End With
        Entities.Irregularidad.Guardar(lstCampos, lstValores)

        Return True
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ActualizarIrregularidad(Folio As Integer,
                                                   IdIrregularidad As Integer,
                                                   Fecha As String,
                                                   Proceso As String,
                                                   SubProceso As String,
                                                   Conducta As String,
                                                   Irregularidad As String,
                                                   Comentarios As String) As Boolean
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)
        Dim lstCaposCondicion As New List(Of String)
        Dim lstValoresCondicion As New List(Of Object)
        'IdIrregularidad As Integer,

        With lstCampos
            .Add("N_ID_FOLIO")
            .Add("F_FECH_IRREGULARIDAD")
            .Add("I_ID_PROCESO")
            .Add("I_ID_SUBPROCESO")
            .Add("I_ID_CONDUCTA")
            .Add("I_ID_IRREGULARIDAD_POR_SANCIONAR")
            .Add("T_DSC_COMENTARIO")
        End With

        With lstValores
            .Add(Folio)
            .Add(Date.ParseExact(Fecha, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
            .Add(Proceso)
            .Add(SubProceso)
            .Add(Conducta)
            .Add(Irregularidad)
            .Add(Comentarios)

        End With

        lstCaposCondicion.Add("I_ID_IRREGULARIDAD") : lstValoresCondicion.Add(IdIrregularidad)

        Entities.Irregularidad.Actualizar(lstCampos, lstValores, lstCaposCondicion, lstValoresCondicion)

        Return True
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerInspectores(Subproceso As String) As List(Of ListItem)

        Dim ListInspectores As New List(Of ListItem)()
        Dim dsSubProcesos As DataSet = Inspector.ObtenerVigentesPorSubproceso(Subproceso, 0)



        For Each row As Data.DataRow In dsSubProcesos.Tables(0).Rows
            ListInspectores.Add(New ListItem() With {
                          .Value = row("T_ID_USUARIO").ToString(),
                          .Text = row("NOMBRE").ToString()})
        Next
        ListInspectores = ListInspectores.OrderBy(Function(item) item.Text).ToList()

        Return ListInspectores
    End Function


    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function GuardarInfoInspector(Folio As Integer, Inspectores As String) As Boolean


        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)
    ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(1)
    ListaCampos.Add("N_ID_PASO") : ListaValores.Add(1)
        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(4)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)

        BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)

        Dim Result2() As String = Split(Inspectores, ",")
        Dim ListaCampos_Inspector As New List(Of String)
        Dim ListaValores_Inspector As New List(Of Object)

        BandejaPC.EliminarInspectores(Folio)

        For Each val As String In Result2
            ListaCampos_Inspector.Add("N_ID_FOLIO") : ListaValores_Inspector.Add(Folio)
            ListaCampos_Inspector.Add("T_ID_USUARIO") : ListaValores_Inspector.Add(val)
            BandejaPC.GuardarRel_PC_INSPECTORES(ListaCampos_Inspector, ListaValores_Inspector)
            ListaCampos_Inspector.Clear()
            ListaValores_Inspector.Clear()
        Next

        Dim DetallePC As DetallePC = New DetallePC()
        Dim usuario As New Entities.Usuario()
        DetallePC.GuardarBitacota(Folio, usuario.IdentificadorUsuario, "1", "Se asignaron inspectores.", " ")

        Try
            Dim Notifica As NotificacionesPC = New NotificacionesPC

            usuario = HttpContext.Current.Session(Entities.Usuario.SessionID)
            Notifica.Folio = Folio
            Notifica.Usuario = usuario.Nombre + " " + usuario.Apellido + " " + usuario.ApellidoAuxiliar
            Notifica.NotificarCorreo(100)
        Catch ex As Exception

        End Try


        Return True
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function SepararSubfolios(Folio As Integer, Subfolios As String) As Boolean


        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

        'ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
        'ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(101)
        'ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)

        'BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)

        GuardarSeparacion(Folio, Subfolios)

        Dim Result2() As String = Split(Subfolios, ",")
        Dim ListaCampos_Inspector As New List(Of String)
        Dim ListaValores_Inspector As New List(Of Object)


        For Each val As String In Result2

            Dim conexion As New Conexion.SQLServer
            Dim query As String = "Exec [dbo].[SepararSubfolios] " + val
            Try
                conexion.ConsultarDT(query)
            Catch ex As Exception
                Throw ex
            Finally
                If Not IsNothing(conexion) Then
                    conexion.CerrarConexion()
                End If
            End Try

            ListaCampos_Inspector.Clear()
            ListaValores_Inspector.Clear()
        Next

        Return True
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarCheckList(Folio As Integer, Checklist As String, Cumple As Integer, Motivo As String) As Boolean


        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

        If Cumple = 1 Then
      ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
      ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(5)
      ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(6)
            ListaCampos.Add("I_ID_PC_CUMPLE") : ListaValores.Add(Cumple)
            ListaCampos.Add("T_DSC_MOTIVO_NO") : ListaValores.Add(Motivo)
            ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)
        End If

        If Cumple = 0 Then
      ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
      ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(5)
      ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(7)
            ListaCampos.Add("I_ID_PC_CUMPLE") : ListaValores.Add(Cumple)
            ListaCampos.Add("T_DSC_MOTIVO_NO") : ListaValores.Add(Motivo)
            ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)
        End If

        BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)


        Dim Result2() As String = Split(Checklist, ",")
        Dim ListaCampos_CheckList As New List(Of String)
        Dim ListaValores_CheckList As New List(Of Object)

        BandejaPC.EliminarCheckList(Folio)

        For Each val As String In Result2
            ListaCampos_CheckList.Add("N_ID_FOLIO") : ListaValores_CheckList.Add(Folio)
            ListaCampos_CheckList.Add("I_ID_CHECKLIST") : ListaValores_CheckList.Add(val.Split("|")(0))
            ListaCampos_CheckList.Add("B_CHECK") : ListaValores_CheckList.Add(val.Split("|")(1))
            BandejaPC.GuardarRel_PC_CheckList(ListaCampos_CheckList, ListaValores_CheckList)
            ListaCampos_CheckList.Clear()
            ListaValores_CheckList.Clear()
        Next



        Return True
    End Function


    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function GuardarInspectorInfo_Status(Folio As Integer) As Boolean

        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(4)
        ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(3)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)

        BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)

        Try
            Dim Notifica As NotificacionesPC = New NotificacionesPC
            Dim usuario As New Entities.Usuario()
            usuario = HttpContext.Current.Session(Entities.Usuario.SessionID)
            Notifica.Folio = Folio
            Notifica.Usuario = usuario.Nombre + " " + usuario.Apellido + " " + usuario.ApellidoAuxiliar
            Notifica.NotificarCorreo(101)
        Catch ex As Exception

        End Try

    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ReasignarSupervisor(Folio As Integer, Proceso As Integer, SubProceso As Integer, Supervisor As String) As Boolean
        BandejaPC.EliminarSupervisores(Folio)

        Dim Result2() As String = Split(Supervisor, ",")
        Dim ListaCampos_Supervisor As New List(Of String)
        Dim ListaValores_Supervisor As New List(Of Object)

        If (Result2(0) <> "undefined" Or Result2(0) <> "") Then
            For Each val As String In Result2
                ListaCampos_Supervisor.Add("N_ID_FOLIO") : ListaValores_Supervisor.Add(Folio)
                ListaCampos_Supervisor.Add("T_ID_USUARIO") : ListaValores_Supervisor.Add(val)
                BandejaPC.AsignarSupervidores(ListaCampos_Supervisor, ListaValores_Supervisor)
                ListaCampos_Supervisor.Clear()
                ListaValores_Supervisor.Clear()
            Next
        End If

        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

        ListaCampos.Add("I_ID_PROCESO") : ListaValores.Add(Proceso)
        ListaCampos.Add("I_ID_SUBPROCESO") : ListaValores.Add(SubProceso)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)

        BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)

        Return True
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaIrregularidadesComp(Folio As Integer) As String
        Dim dtIrregularidades As DataTable = Entities.Irregularidad.ObtenerCompletas(Folio)
        Dim Res As String = ""
        If dtIrregularidades.Rows.Count > 0 Then
            For Each row As Data.DataRow In dtIrregularidades.Rows
                Res = row("RES").ToString()
            Next
        Else
            Res = "No"
        End If

        Return Res
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ValidarIrregularidades(Folio As Integer) As String
        If Entities.Irregularidad.ObtenerTodas(Folio).Rows.Count > 0 Then

            Return "Si"
        Else
            Return "No"
        End If
    End Function


    Protected Sub btnActualizaOculto_Click(sender As Object, e As EventArgs) Handles btnActualizaOculto.Click
        ActualizarPaso4()
    End Sub



    Protected Sub ActualizarPaso4()
        Supervisor1.Cargar()
        Inspector.CargarInpectoresSeleccion()
        'Se muestra el control de inspectores
        Inspector.Visible = True

        PnlInspector.Enabled = False
        pnlSupervisores.Enabled = False
        'Se oculta irregularidad
        pnlIrregularidad.Visible = False

        pnlPaso1_1.Visible = True
        ' btnAceptarP1_E1.Visible = True

        btnAceptarP1_E1.Style("display") = "block"
        btnReasignarP1_E1.Attributes.Remove("onclick")
        btnGuardar.Attributes.Add("onclick", "GuardarInspectorInfo_Status()")
    End Sub


#Region "Pestaña Irregularidades"

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerDatosModificarIrregularidad(Irregularidad As Integer) As List(Of String)
        Dim aDatos As String()
        Dim lDatos As New List(Of String)
        aDatos = cIrregularidades.CargaDatosCompletar(Irregularidad)

        '     If lDatos.Count = 0 Then
        '    Exit Function
        '   End If

        lDatos.Add(aDatos(0))
        lDatos.Add(aDatos(1))
        lDatos.Add(aDatos(2))
        lDatos.Add(aDatos(3))
        lDatos.Add(aDatos(4))
        lDatos.Add(aDatos(5))
        'Descripcion de como se corrigio
        lDatos.Add(aDatos(7))
        'Fecha de correcion
        lDatos.Add(aDatos(8))
        'Descripcion de Corregido
        lDatos.Add(aDatos(9))
        lDatos.Add(aDatos(10))

        Return lDatos

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function EliminarIrregularidad(idIrregularidad As Integer) As Boolean
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)

        lstCampos.Add("I_ID_IRREGULARIDAD") : lstValores.Add(idIrregularidad)

        Return Entities.Irregularidad.EliminarIrregularidad(lstCampos, lstValores)
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerDatosIrregularidad(Irregularidad As Integer) As List(Of String)
        Dim aDatos As String()
        Dim lDatos As New List(Of String)
        aDatos = cIrregularidades.CargaDatosCompletar(Irregularidad)

        lDatos.Add(aDatos(0))
        lDatos.Add(aDatos(1))
        lDatos.Add(ConexionSISAN.TraeDescProcesosxID(CInt(aDatos(2))))
        lDatos.Add(ConexionSISAN.TraeDescSubprocesosxID(CInt(aDatos(3))))
        lDatos.Add(ConexionSISAN.TraeDescConductaxID(CInt(aDatos(4))))
        lDatos.Add(ConexionSISAN.TraeDescIrregularidadxID(CInt(aDatos(5))))
        'Descripcion de como se corrigio
        lDatos.Add(aDatos(7))
        'Fecha de correcion
        lDatos.Add(aDatos(8))
        'Descriocion de Corregido
        lDatos.Add(aDatos(9))
        'isCompleta
        lDatos.Add(aDatos(11))


        Return lDatos

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function CompletarIrregularidad(Irregularidad As Integer, TipoCorreccion As String, Comentarios As String, Fecha As String) As Boolean

        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)
        Dim lstCamposCond As New List(Of String)
        Dim lstValorCond As New List(Of Object)

        With lstCampos
            .Add("T_DSC_CORREGIDO")
            .Add("T_DSC_COMOCORRIGE")
            If TipoCorreccion <> "Nula" Then
                .Add("F_FECH_CORRECCION")
            End If
            .Add("B_COMPLETA")
        End With

        With lstValores
            .Add(TipoCorreccion)
            .Add(Comentarios)
            If TipoCorreccion <> "Nula" Then
                .Add(Date.ParseExact(Fecha, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
            End If
            .Add(1)
        End With

        With lstCamposCond
            .Add("I_ID_IRREGULARIDAD")
        End With

        With lstValorCond
            .Add(Irregularidad)
        End With
        cIrregularidades.GuardaCompletarIrregularidad(lstCampos, lstValores, lstCamposCond, lstValorCond)

        Return True

    End Function


#End Region

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarResolucion(Folio As Integer, Resolucion As Integer, Descripcion As String) As Boolean

        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2) 'MMOB ANTES "3" AHORA "2"
    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(10) 'MMOB ANTES "102" AHORA "10"
    ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(9)
    ListaCampos.Add("I_ID_RESOLUCION") : ListaValores.Add(Resolucion)
        ListaCampos.Add("T_DSC_RESOLUCION") : ListaValores.Add(Descripcion)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)

        BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)
        Return True
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarResolucionP(Folio As Integer, Resolucion As Integer, Descripcion As String) As Boolean

        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

    ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
    ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(13)
    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(14)
        ListaCampos.Add("I_ID_RESOLUCION") : ListaValores.Add(Resolucion)
        ListaCampos.Add("T_DSC_RESOLUCION") : ListaValores.Add(Descripcion)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)

        BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)
        Return True
    End Function


    Public Shared Function GuardarSeparacion(Folio As Integer, SubEntidadRel As String) As Boolean

        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

        If (SubEntidadRel <> "") Then

            Dim Result2() As String = Split(SubEntidadRel, ",")
            Dim ListaCampos_Relacion As New List(Of String)
            Dim ListaValores_Relacion As New List(Of Object)

            Dim ListaCamposCondicion_Rel As New List(Of String)
            Dim ListaValoresCondicion_Rel As New List(Of Object)


            For Each val As String In Result2
                ListaCampos_Relacion.Add("B_ISRESOLUCION") : ListaValores_Relacion.Add(1)
                ListaCamposCondicion_Rel.Add("N_ID_RELACION") : ListaValoresCondicion_Rel.Add(val)

                BandejaPC.Actualiza_Entidad_SubEntidad_PC(ListaCampos_Relacion, ListaValores_Relacion, ListaCamposCondicion_Rel, ListaValoresCondicion_Rel)
                ListaCamposCondicion_Rel.Clear()
                ListaValoresCondicion_Rel.Clear()
                ListaCampos_Relacion.Clear()
                ListaValores_Relacion.Clear()
            Next
        End If
        Return True
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerProcesos() As List(Of ListItem)
        Dim Procesos As New List(Of ListItem)()



        For Each row As Data.DataRow In ConexionSISAN.ObtenerProcesos.Rows
            Procesos.Add(New ListItem() With {
                          .Value = row("ID_PROCESO").ToString(),
                          .Text = row("DESC_PROCESO").ToString()})

        Next

        Return Procesos
    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerSubProcesos(Proceso As Integer) As List(Of ListItem)
        Dim SubProcesos As New List(Of ListItem)()

        For Each row As Data.DataRow In ConexionSISAN.ObtenerSubprocesos(Proceso).Rows
            SubProcesos.Add(New ListItem() With {
                          .Value = row("ID_SUBPROCESO").ToString(),
                          .Text = row("DESC_SUBPROCESO").ToString()})

        Next

        Return SubProcesos
    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerConducta(Proceso As Integer, SubProceso As Integer) As List(Of ListItem)
        Dim ListConducta As New List(Of ListItem)()

        For Each row As Data.DataRow In ConexionSISAN.ObtenerConducta(Proceso, SubProceso).Rows
            ListConducta.Add(New ListItem() With {
                          .Value = row("ID_CONDUCTA").ToString(),
                          .Text = row("DESC_CONDUCTA").ToString()})

        Next

        Return ListConducta
    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerIrregularidad(Proceso As Integer, SubProceso As Integer, Conducta As Integer) As List(Of ListItem)
        Dim ListIrregularidad As New List(Of ListItem)()

        For Each row As Data.DataRow In ConexionSISAN.ObtenerIrregularidades(Proceso, SubProceso, Conducta).Rows
            ListIrregularidad.Add(New ListItem() With {
                          .Value = row("ID_IRREGULARIDAD").ToString(),
                          .Text = row("DESC_IRREGULARIDAD").ToString()})

        Next

        Return ListIrregularidad
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerSubEntidadesMensaje(Folio As Integer) As List(Of ListItem)
        Dim subEntidades As New List(Of ListItem)()



        For Each row As Data.DataRow In Entities.Entidad.ObtenerSubFoliosPorFolio(Folio).Tables(0).Rows
            subEntidades.Add(New ListItem() With {
                          .Value = row("N_ID_RELACION").ToString(),
                          .Text = row("I_ID_FOLIO_SUPERVISAR_SUB_ENTIDAD").ToString()})

        Next

        Return subEntidades
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerSubEntidadesComplete(Folio As Integer) As List(Of ListItem)
        Dim subEntidades As New List(Of ListItem)()



        For Each row As Data.DataRow In Entities.Entidad.ObtenerSubFoliosPorFolioComplete(Folio).Tables(0).Rows
            subEntidades.Add(New ListItem() With {
                          .Value = row("N_ID_RELACION").ToString(),
                          .Text = row("I_ID_FOLIO_SUPERVISAR_SUB_ENTIDAD").ToString()})

        Next

        Return subEntidades
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ValidaExpediente(Folio As Integer) As Boolean
        Dim Expediente As ExpedientePC = New ExpedientePC
        Return Expediente.ExpedienteValido(Folio)
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerSubFolios(Folio As Integer) As List(Of ListItem)
        Dim subEntidades As New List(Of ListItem)()



        For Each row As Data.DataRow In Entities.Entidad.ObtenerSubFoliosPorFolio(Folio).Tables(0).Rows
            subEntidades.Add(New ListItem() With {
                          .Value = row("N_ID_RELACION").ToString(),
                          .Text = row("I_ID_FOLIO_SUPERVISAR_SUB_ENTIDAD").ToString()})

        Next

        Return subEntidades
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarComentarioRes(Folio As Integer, Comentario As String) As Boolean
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

    ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(101)
    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(103) 'MMOB - CAMBIA STATUS PARA PODER SOLICITAR DOC OBLIGATORIOS
        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(3) 'MMOB - CAMBIA EL PASO PARA PODER SOLICITAR DOC OBLIGATORIOS
        ListaCampos.Add("T_DSC_COMENTARIOS") : ListaValores.Add(Comentario)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)
        BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarComentarioResASisan(Folio As Integer, Comentario As String) As Boolean
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

    ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(105)
    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(106) 'MMOB - CAMBIA STATUS PARA PODER CONTINUAR CON ENVIO A SISAN
        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(4) 'MMOB - CAMBIA EL PASO PARA PODER CONTINUAR CON ENVIO A SISAN
        ListaCampos.Add("T_DSC_COMENTARIOS") : ListaValores.Add(Comentario)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)
        BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)
    End Function



    ''' <summary>
    ''' Valida que exista alguna sub entidad realcionada con el Folio
    ''' </summary>
    ''' <param name="Folio"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function ValidarFolioSubEntidades(Folio As Integer) As Boolean
        Return Entidad.ValidarSubFoliosPorFolio(Folio)
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardarBitacota(Folio As Integer, Usuario As String, Paso As String, Accion As String, Comentarios As String) As Boolean
        Entities.BitacoraPC.AgregarEntrada(Folio, Usuario, DetallePC.ObtenerDCSPaso(Paso), Accion, Comentarios)
        Return True
    End Function
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function ValidacionSISAN(Folio As Integer, Usuario As String) As String
        Dim USUARIOS = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioSisan")
        Dim PASSWORDS = Utilerias.Cifrado.DescifrarAES(System.Web.Configuration.WebConfigurationManager.AppSettings("PassEncSisan").ToString())

        Dim PC As New Entities.PC(Folio)

        Dim Registro As New wsSisanRegV2.RegistroExternoV2
        Dim credentialsS As System.Net.NetworkCredential = New System.Net.NetworkCredential(USUARIOS, PASSWORDS, "ADCONSAR")
        Registro.Credentials = credentialsS
        Registro.ConnectionGroupName = "SEPRIS"

        Dim subentidad As Integer = -1 'En caso de no enviar
        Dim sistema As String = "SEPRIS" 'sistema 2 sepris
        Dim tipoEntidad As Integer = 1
        If PC.IdEntidad = 1 Then
            'PROCESAR
            tipoEntidad = 7
        End If
        Dim entidad As Integer = PC.IdEntidad
        Dim clasificacion As Integer = 1 'PAC
        Dim participante As Integer = 0 ' Viene la irregularidad
        Dim oficioSICOD As String = "CONSAR/DI/VF/DVF/090/JBS/2018" 'sistema 2 sepris

        'Oficio del dictamen
        Dim dtDictamen As DataTable = Entities.DocumentoPC.ObtenerArchivos(Folio, 14)
        If dtDictamen.Rows.Count > 0 Then
            If (IsDBNull(dtDictamen.Rows(0)("T_FOLIO_SICOD"))) Then
                oficioSICOD = oficioSICOD
            Else
                oficioSICOD = dtDictamen.Rows(0)("T_FOLIO_SICOD")
            End If
        End If

        Dim folioSISAN As String = ""

        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)
        Dim mensajes As String() = {}
        Dim resultado As Boolean = True
        Dim MensajeResult As String = ""


        Dim resultadoRegistro As wsSisanRegV2.WsResultado = Registro.RegistroExterno("FOL. REF", Usuario, sistema, tipoEntidad, entidad, subentidad, 1, clasificacion, "ninguno", Date.Now, Date.Now.AddDays(1), oficioSICOD, "prueba", Usuario, "ninguno")



        If Not resultadoRegistro.isError Then

            MensajeResult = resultadoRegistro.Folio
            folioSISAN = resultadoRegistro.Folio

            Dim irregularidades As DataTable = Entities.Irregularidad.ObtenerTodas(Folio)
            If irregularidades.Rows.Count > 0 Then
                For Each irregularidad As DataRow In irregularidades.Rows
                    Dim resultadoIrregularidad As wsSisanRegV2.WsResultado = Registro.RegistroIncidencias(irregularidad("I_ID_IRREGULARIDAD_POR_SANCIONAR"), resultadoRegistro.Irregularidad, irregularidad("F_FECH_IRREGULARIDAD"), irregularidad("T_DSC_COMENTARIO"))
                    If resultadoIrregularidad.isError Then
                        mensajes = resultadoIrregularidad.lstMensajes
                        resultado = False
                        Exit For
                    End If
                Next

                'se comenta esta linea para atender registro de estatus incorrecto en SISAN
                'Registro.SolicitarAutorizacion(resultadoRegistro.Irregularidad, Usuario)

            End If
        Else
            mensajes = resultadoRegistro.lstMensajes
            resultado = False
        End If



        If resultado Then
      ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(14)
      ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(15)
            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(5)
            ListaCampos.Add("T_ID_FOLIO_SISAN") : ListaValores.Add(folioSISAN)
            ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)
            BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)

            Try
                Dim Notifica As NotificacionesPC = New NotificacionesPC
                Dim usr As New Entities.Usuario()
                usr = HttpContext.Current.Session(Entities.Usuario.SessionID)
                Notifica.Folio = Folio
                Notifica.Usuario = usr.Nombre + " " + usr.Apellido + " " + usr.ApellidoAuxiliar
                Notifica.NotificarCorreo(105)
            Catch ex As Exception

            End Try

        Else
            MensajeResult = "Error"
            For Each msg As String In mensajes
                MensajeResult += "<br\>" + msg
            Next
        End If


        Return MensajeResult
    End Function


#Region "PESTAÑA DE ACTIVIDADES"

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardaCamposAlta_Actividades(Folio As Integer,
                                                        ActividadDes As String,
                                                        Usuario As String,
                                                        FechaEntrega As String) As Boolean
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)
        Dim FechaInicio As String

        FechaInicio = Now().ToString("dd/MM/yyyy")
        With lstCampos
            .Add("N_ID_FOLIO_SICOD")
            '.Add("I_ID_ACTIVIDAD")
            .Add("T_DSC_ACTIVIDAD")
            .Add("T_ID_USUARIO")
            .Add("F_FECH_ENTREGA")
            .Add("I_ID_ESTATUS")
            .Add("B_FLAG_VIGENTE")
            .Add("F_FECH_INI_VIG")
            '            .Add("F_FECH_FIN_VIG")
        End With

        With lstValores
            .Add(Folio)
            '.Add(Actividad.TraUltFolActividad(Folio))
            .Add(ActividadDes)
            .Add(Usuario)
            .Add(Date.ParseExact(FechaEntrega, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
            .Add("En Proceso")
            .Add(1)
            .Add(Date.ParseExact(FechaInicio, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
        End With
        Actividad.GuardaAltaActividad(lstCampos, lstValores)
        Return True
    End Function




    <System.Web.Services.WebMethod()>
    Public Shared Function CargaCampos_Actividades(IdActividad As Integer) As List(Of String)
        Dim lDatos As New List(Of String)
        Dim aDatos As String()
        aDatos = Actividad.CargaDatosModificacion(IdActividad)

        lDatos.Add(aDatos(0))
        lDatos.Add(aDatos(1))
        lDatos.Add(aDatos(2))
        lDatos.Add(aDatos(3))
        Return lDatos

    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function ModificaCampos_Actividades(IdActividad As Integer, Estatus As String, Usuario As String, Comentarios As String) As Boolean
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)
        Dim lstCamposCon As New List(Of String)
        Dim lstValoresCon As New List(Of Object)
        Dim FechaComentario As String

        FechaComentario = Now().ToString("dd/MM/yyyy")


        lstCampos.Add("I_ID_ACTIVIDAD") : lstValores.Add(IdActividad)
        lstCampos.Add("T_DSC_COMENTARIO") : lstValores.Add(Comentarios)
        lstCampos.Add("F_FECH_COMENTARIO") : lstValores.Add(Date.ParseExact(FechaComentario, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
        lstCampos.Add("T_ID_USUARIO") : lstValores.Add(Usuario)
        Actividad.GuardaModifActividad(lstCampos, lstValores, Estatus)

        Return True
    End Function


    <System.Web.Services.WebMethod()>
    Public Shared Function GuardandoProrroga(IDReq As Integer, iDias As Integer, Usuario As String) As Boolean
        Dim lstCampos As New List(Of String)
        Dim lstCamposProrroga As New List(Of String)
        Dim lstValores As New List(Of Object)
        Dim lstValoresProrroga As New List(Of Object)
        Dim dFechaNueva As Date
        Dim dFechaAnterior As Date
        Dim dtDatos As New DataTable
        Dim idRequerimiento As Integer = IDReq

        dtDatos = Entities.RequerimientoPC.TraerDatos(IDReq)
        dFechaAnterior = dtDatos.Rows(0)("F_FECH_ACUSE").ToString

        If dtDatos.Rows(0)("F_FECH_ESTIMADA").ToString <> String.Empty Or dtDatos.Rows(0)("F_FECH_ESTIMADA").ToString <> "" Then
            dFechaAnterior = dtDatos.Rows(0)("F_FECH_ESTIMADA").ToString
        End If

        dFechaNueva = BandejaPC.DiasHabiles(dFechaAnterior, iDias)
        'dFechaNueva = dFechaAnterior.AddDays(iDias)
        With lstCampos
            .Add("i_ID_ESTATUS")
            .Add("F_FECH_ESTIMADA")
        End With

        With lstValores
            .Add(2)
            .Add(Date.ParseExact(dFechaNueva, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
        End With

        Entities.RequerimientoPC.ActualizarDatos(lstCampos, lstValores, idRequerimiento)

        With lstCamposProrroga
            .Add("I_ID_REQUERIMIENTO")
            .Add("T_DSC_PRORROGA")
            .Add("I_NUM_DIA_PRORROGA")
            .Add("T_ID_USUARIO")
            .Add("F_FECH_PRORROGA")
            .Add("F_FECH_ENTREGA")
        End With

        With lstValoresProrroga
            .Add(idRequerimiento)
            .Add("PRORROGA")
            .Add(iDias)
            .Add(Usuario)
            .Add(Date.ParseExact(dFechaAnterior, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
            .Add(Date.ParseExact(dFechaNueva, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
        End With

        Entities.RequerimientoPC.InsertarProrroga(lstCampos, lstValores)
        Return True
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardandoEntregado(IDReq As Integer, dNvaFecha As String) As Integer
        Dim iIDRequerimiento As Integer = 1
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)

        With lstCampos
            .Add("i_ID_ESTATUS")
            .Add("F_FECH_REAL")
        End With

        With lstValores
            .Add(3)
            .Add(Date.ParseExact(dNvaFecha, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))

        End With
        Entities.RequerimientoPC.ActualizarDatos(lstCampos, lstValores, IDReq)
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardandoNoEntregado(IDReq As Integer) As Integer  'IDRequerimiento As Integer, NumRow As Integer, iDias As Integer
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)

        With lstCampos
            .Add("i_ID_ESTATUS")
        End With

        With lstValores
            .Add(4)
        End With
        Entities.RequerimientoPC.ActualizarDatos(lstCampos, lstValores, IDReq)
    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function GuardaCamposProrroga(IdActividad As Integer, MotivoProrroga As String, DiasProrroga As Integer, FechaEntrega As String, Usuario As String) As Boolean
        Dim lstCamposPro As New List(Of String)
        Dim lstValoresPro As New List(Of Object)
        Dim FechaProrroga As String = Now().ToString("dd/MM/yyyy")



        lstCamposPro.Add("I_ID_ACTIVIDAD") : lstValoresPro.Add(IdActividad)
        '.Add("I_ID_PRORROGA")
        lstCamposPro.Add("T_DSC_PRORROGA") : lstValoresPro.Add(MotivoProrroga)
        lstCamposPro.Add("I_NUM_DIA_PRORROGA") : lstValoresPro.Add(DiasProrroga)
        lstCamposPro.Add("T_ID_USUARIO") : lstValoresPro.Add(Usuario)
        lstCamposPro.Add("F_FECH_PRORROGA") : lstValoresPro.Add(Date.ParseExact(FechaProrroga, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
        lstCamposPro.Add("F_FECH_ENTREGA") : lstValoresPro.Add(Date.ParseExact(FechaEntrega, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))


        Actividad.GuardaAltaProrrogaActividad(lstCamposPro, lstValoresPro, "Prorrogada")

    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function SolicitarCancelacion(Folio As Integer, EstatusPC As Integer) As Boolean
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(21)
        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
        ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(EstatusPC)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)

        Try
            Dim Notifica As NotificacionesPC = New NotificacionesPC
            Dim usuario As New Entities.Usuario()
            usuario = HttpContext.Current.Session(Entities.Usuario.SessionID)
            Notifica.Folio = Folio
            Notifica.Usuario = usuario.Nombre + " " + usuario.Apellido + " " + usuario.ApellidoAuxiliar
            Notifica.NotificarCorreo(132)
        Catch ex As Exception

        End Try

        Return BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)

    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function AprobarCancelacion(Folio As Integer) As Boolean
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(22)
        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)

        Try
            Dim Notifica As NotificacionesPC = New NotificacionesPC
            Dim usuario As New Entities.Usuario()
            usuario = HttpContext.Current.Session(Entities.Usuario.SessionID)
            Notifica.Folio = Folio
            Notifica.Usuario = usuario.Nombre + " " + usuario.Apellido + " " + usuario.ApellidoAuxiliar
            Notifica.NotificarCorreo(133)
        Catch ex As Exception

        End Try

        Return BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)

    End Function
    <System.Web.Services.WebMethod()>
    Public Shared Function RechazarCancelacion(Folio As Integer, EstatusAnterior As Integer) As Boolean
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)

        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(EstatusAnterior) 'anterior
        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
        ListaCamposCondicion.Add("N_ID_FOLIO") : ListaValoresCondicion.Add(Folio)

        Try
            Dim Notifica As NotificacionesPC = New NotificacionesPC
            Dim usuario As New Entities.Usuario()
            usuario = HttpContext.Current.Session(Entities.Usuario.SessionID)
            Notifica.Folio = Folio
            Notifica.Usuario = usuario.Nombre + " " + usuario.Apellido + " " + usuario.ApellidoAuxiliar
            Notifica.NotificarCorreo(134)
        Catch ex As Exception

        End Try

        Return BandejaPC.ActualizarPC(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)

    End Function
#End Region



    Protected Sub btnGeneraOpi_Click(sender As Object, e As EventArgs)

    End Sub

    'Protected Sub imgProcesoVisita_Click(sender As Object, e As ImageClickEventArgs) Handles imgProcesoVisita.Click
    '    Dim usuario As New Entities.Usuario()
    '    If (usuario.IdArea = 35) Then
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ImagenOperaciones", "ImagenMostrar(1);", True)
    '    Else
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ImagenFinanciera", "ImagenMostrar(0);", True)
    '    End If
    'End Sub

    Protected Sub imgProcesoVisita_Click(sender As Object, e As ImageClickEventArgs) Handles imgProcesoVisita.Click
        Dim usuario As New Entities.Usuario()
        If (usuario.IdArea = 35) Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ImagenOperaciones", "ImagenMostrar(1);", True)
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ImagenFinanciera", "ImagenMostrar(0);", True)
        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function CalculaFecha(Fecha As String, Dias As Integer) As String
        Dim NewDate As Date
        NewDate = Convert.ToDateTime(Fecha)

        Dim DiasHab As Integer = 0

        While DiasHab < Dias
            NewDate = CDate(NewDate).AddDays(1).ToString("dd/MM/yyyy")
            If Not (NewDate.DayOfWeek = DayOfWeek.Sunday Or NewDate.DayOfWeek = DayOfWeek.Saturday Or ConexionSICOD.IsFestivo(NewDate.ToString("yyyy-MM-dd")) = "Si") Then
                DiasHab = DiasHab + 1
            End If
        End While

        Return NewDate.ToString("dd/MM/yyyy")
    End Function
End Class

