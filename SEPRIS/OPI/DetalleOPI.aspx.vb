Imports Entities

Public Class DetalleOPI
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Public Property Mensaje As String
    Public Property Mensaje2 As String
    Public Property Folio As Integer
    Public Property lstMensajes As List(Of String)
    Public Property Ahora As String
#End Region
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


    Private _opiDetalle As New OPI_Incumplimiento

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim _opiFunc As New Registro_OPI
        lblTitle.Text = ""
        _opiDetalle = _opiFunc.GetOPIDetail(Session("I_ID_OPI"))

        Dim ctrlName As String = Page.Request.Params.Get("__EVENTTARGET")

        If _opiDetalle.I_ID_ESTATUS = 13 Then
            calEx.StartDate = DateTime.Now.ToString("dd/MM/yyyy")
        End If

        If Not Page.IsPostBack Then
            txtDiasProrroga.Text = Conexion.SQLServer.Parametro.ObtenerValor("DiasProrrogaOPI").ToString()
            Bitacora2.Inicializar()
            Call Carga_Pasos()
        End If

    End Sub
    Private Sub Carga_Pasos()
        DetalleComentOPI1.Visible = True
        DetalleClasifOPI1.Visible = False
        DetalleSupuestoAvisoOPI1.Visible = False
        DetalleRespAforeOPI1.Visible = False
        DetalleIrregularidad1.Visible = False
        DetallePosibIncumplim.Visible = False
        DetalleAltaIrregularidad.Visible = False
        VisibilidadTabDatSisan(False)
        xpnlPaso1__1.Visible = False
        xpnlPaso2_0.Visible = False
        xpnlPaso2_1.Visible = False
        xpnlPaso3.Visible = False
        xpnlPaso4.Visible = False
        xpnlPaso4_1.Visible = False
        xpnlPaso5.Visible = False
        xpnlPaso6.Visible = False
        xpnlPaso7.Visible = False
        xpnlPaso8.Visible = False
        xpnlPaso9.Visible = False
        xpnlPaso10.Visible = False

        Session("EstatusPasoOPI") = _opiDetalle.I_ID_ESTATUS
        Session("ClasificacionOPI") = _opiDetalle.T_DSC_CLASIFICACION
        ' En los pasos se almacena el ultimo y en base a éste se mostrará el siguiente paso
        lblFolio.Text = "FOLIO : " & _opiDetalle.T_ID_FOLIO

        Select Case _opiDetalle.N_ID_PASO
            Case 1  ' Paso 1 : Detección de Posible incumplimiento

                Select Case _opiDetalle.I_ID_ESTATUS
                    'Select Case _opiDetalle.N_ID_SUBPASO '' PCMT: CAMBIO

                    Case 1 ''Estatus: Registrado
                        lblPaso.Text = "Paso 1 : Registro de Oficio"
                        xpnlPaso1__1.Visible = True
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnEditar1.Visible = True
                        End If
                        'If _opiDetalle.N_ID_SUBPASO = -1 Then
                        'btnNotificar.Visible = False
                        'Else
                        btnAceptar1.Visible = False
                        btnNotificar.Visible = True
                        'pnlDetalleComentOPI.Enabled = False
                        'DetalleComentOPI1.ValComentarios = _opiDetalle.T_COMENTARIOS_PASOS
                        'End If

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnNotificar.Enabled = True
                        Else
                            btnNotificar.Enabled = False
                        End If
                    Case 2 ''Estatus : OPI Notificado
                        lblPaso.Text = "Paso 2 : Clasificación de Oficio"
                        xpnlPaso2_0.Visible = True
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnEditar2.Visible = True
                        End If
                        btnNotificarP2.Visible = False
                        btnAceptar2.Visible = True
                        DetalleClasifOPI1.ComboVisible = True
                        DetalleClasifOPI1.Visible = True

                        ''DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar2.Enabled = True
                        Else
                            btnAceptar2.Enabled = False
                        End If
                End Select
            Case 2  ' Paso 2 : Clasificación de Posible incumplimiento

                Select Case _opiDetalle.I_ID_ESTATUS
                    Case 3 'OPI Clasificado
                        lblPaso.Text = "Paso 2 : Clasificación de Oficio"
                        xpnlPaso2_0.Visible = True
                        btnNotificarP2.Visible = True

                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        DetalleClasifOPI1.Visible = True

                        'DetalleComentOPI1.ValComentarios = _opiDetalle.T_COMENTARIOS_PASOS
                        'pnlDetalleComentOPI.Enabled = False

                        btnAceptar2.Visible = False

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnNotificarP2.Enabled = True
                        Else
                            btnNotificarP2.Enabled = False
                        End If
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnEditar2.Visible = True
                        End If
                    Case 4, 45 'Clasificacion OPi Notificado - En espera de información
                        lblPaso.Text = "Paso 2 : Clasificación de Oficio"
                        xpnlPaso2_1.Visible = True
                        btnAceptar21.Visible = True
                        btnNotificarP21.Visible = False
                        btnEditar21.Visible = False

                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        If _opiDetalle.T_DSC_CLASIFICACION = "Aviso de conocimiento" Then
                            DetalleSupuestoAvisoOPI1.Visible = True
                        End If

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar21.Enabled = True
                        Else
                            btnAceptar21.Enabled = False
                        End If

                    Case 5, 7 ''Aviso Elaborado
                        lblPaso.Text = "Paso 2 : Clasificación de Oficio"
                        xpnlPaso2_1.Visible = True
                        btnAceptar21.Visible = False
                        btnNotificarP21.Visible = True

                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        If _opiDetalle.T_DSC_CLASIFICACION = "Aviso de conocimiento" Then
                            DetalleSupuestoAvisoOPI1.ValSupuestoAviso = _opiDetalle.I_ID_SUPUESTO
                            pnlSupuestoAviso.Enabled = False
                            DetalleSupuestoAvisoOPI1.Visible = True
                            DetalleSupuestoAvisoOPI1.ComboReadOnly = True
                        End If

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            If _opiDetalle.N_ID_PASO_ANT = 2 Then
                                btnNotificarP21.Enabled = True
                                btnNotificarP21.CommandName = "Notificar_Paso2.4"
                            Else
                                btnNotificarP21.Enabled = True
                                btnNotificarP21.CommandName = "Notificar_Paso2.4.2da"
                            End If
                        Else
                            btnNotificarP21.Enabled = False
                        End If

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnEditar21.Visible = True

                        End If

                    Case 6 ''Aviso Notificado
                        lblPaso.Text = "Paso 2 : Clasificación de Oficio"
                        xpnlPaso2_1.Visible = True
                        btnAceptar21.Visible = False
                        btnNotificarP21.Visible = False

                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        If _opiDetalle.T_DSC_CLASIFICACION = "Aviso de conocimiento" Then
                            DetalleSupuestoAvisoOPI1.ValSupuestoAviso = _opiDetalle.I_ID_SUPUESTO
                            DetalleSupuestoAvisoOPI1.Visible = True
                            DetalleSupuestoAvisoOPI1.ComboReadOnly = True
                        End If

                        pnlDetalleComentOPI.Enabled = False
                        DetalleComentOPI1.ValComentarios = _opiDetalle.T_COMENTARIOS_PASOS

                    Case 8
                        xpnlPaso3.Visible = True

                        lblPaso.Text = "Paso 3 : Respuesta AFORE"

                        DetalleRespAforeOPI1.Visible = True
                        DetalleRespAforeOPI1.ComboVisible = True


                        divFechaEstimadaEntrega.Visible = True

                        txtFecEstimadaEntrega.Text = IIf(IsDBNull(_opiDetalle.F_FECH_ESTIM_ENTREGA), "", _opiDetalle.F_FECH_ESTIM_ENTREGA)
                        txtFecEstimadaEntrega.ReadOnly = True

                        btnNotificarP3.Visible = False
                        btnAceptar3.Visible = True
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                                puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                                puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar3.Enabled = True
                        Else
                            btnAceptar3.Enabled = False
                        End If

                    Case 9
                        lblPaso.Text = "Paso 2 : Requerimiento de informacion adicional"
                        xpnlPaso2_1.Visible = True
                        btnAceptar21.Visible = True
                        btnNotificarP21.Visible = False

                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        If _opiDetalle.T_DSC_CLASIFICACION = "Aviso de conocimiento" Then
                            DetalleSupuestoAvisoOPI1.Visible = True
                        End If

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                                puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                                puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar21.Enabled = True
                        Else
                            btnAceptar21.Enabled = False
                        End If

                End Select

            Case 3  ''"Paso 3 : Respuesta AFORE"

                Select Case _opiDetalle.I_ID_ESTATUS
                    Case 8
                        xpnlPaso3.Visible = True

                        lblPaso.Text = "Paso 3 : Respuesta AFORE"

                        DetalleRespAforeOPI1.Visible = True
                        DetalleRespAforeOPI1.ComboVisible = True


                        divFechaEstimadaEntrega.Visible = True

                        txtFecEstimadaEntrega.Text = _opiDetalle.F_FECH_ESTIM_ENTREGA
                        txtFecEstimadaEntrega.ReadOnly = True

                        btnNotificarP3.Visible = False
                        btnAceptar3.Visible = True
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                                puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                                puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar3.Enabled = True
                        Else
                            btnAceptar3.Enabled = False
                        End If


                    Case 9
                        xpnlPaso3.Visible = True

                        lblPaso.Text = "Paso 3 : Respuesta AFORE"

                        DetalleRespAforeOPI1.Visible = True
                        DetalleRespAforeOPI1.ComboVisible = True

                        divFechaEstimadaEntrega.Visible = True

                        If _opiDetalle.F_FECH_ESTIM_ENTREGA Is Nothing Then
                            txtFecEstimadaEntrega.Text = ""
                        Else
                            txtFecEstimadaEntrega.Text = _opiDetalle.F_FECH_ESTIM_ENTREGA
                        End If
                        txtFecEstimadaEntrega.ReadOnly = True

                        btnNotificarP3.Visible = False
                        btnAceptar3.Visible = True
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar3.Enabled = True
                        Else
                            btnAceptar3.Enabled = False
                        End If
                    Case 10
                        xpnlPaso3.Visible = True

                        lblPaso.Text = "Paso 3 : Respuesta AFORE"

                        DetalleRespAforeOPI1.Visible = True
                        DetalleRespAforeOPI1.ComboVisible = False
                        DetalleRespAforeOPI1.ValRespAfore = _opiDetalle.T_DSC_RESP_AFORE

                        btnAceptar3.Visible = False

                        If _opiDetalle.T_DSC_RESP_AFORE = "Respuesta de requerimiento" Then

                            btnNotificarP3.Visible = True

                        ElseIf _opiDetalle.T_DSC_RESP_AFORE = "Próroga de entrega de información" Then
                            txtFecEstimadaEntrega.Visible = True
                        Else
                            btnNotificarP3.Visible = False
                        End If

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnNotificarP3.Enabled = True
                        Else
                            btnNotificarP3.Enabled = False
                        End If
                End Select


            Case 4  ' Paso 5 : Determinación hay o no irregularidad
                Select Case _opiDetalle.I_ID_ESTATUS
                    Case 12
                        lblPaso.Text = "Paso 4 :  Análisis  del área de vigilancia"
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION


                        DetalleIrregularidad1.MuestraIrregStd = "True"
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG

                        btnCorreo4.Visible = False
                        btnSalir4.Visible = False
                        btnAceptar4.Visible = False
                        btnSalir4.Visible = False
                        xpnlPaso4.Visible = True

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar4_1.Enabled = True
                        Else
                            btnAceptar4_1.Enabled = False
                        End If



                    Case 13
                        lblPaso.Text = "Paso 5 : Determinación hay o no irregularidad"

                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        btnAceptar4.Visible = False
                        btnAceptar1.Visible = False
                        btnDetalle4.Visible = True
                        btnSalir4.Visible = False
                        btnInfoSol4.Visible = False
                        btnAceptar4_1.Visible = False
                        xpnlPaso4.Visible = True
                        pnlIrregularidad.Enabled = False

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                             puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnCorreo4.Enabled = True
                        Else
                            btnCorreo4.Enabled = False
                        End If

                        Ahora = Date.Now.ToString("dd/MM/yyyy")
                        valDateMustBeWithinMinMaxRange.MinimumValue = Ahora


                End Select
            Case 5  '  Paso 6 : Revisión de irregularidad con VJ
                Select Case _opiDetalle.I_ID_ESTATUS
                    Case 14
                        lblPaso.Text = "Paso 5 : hubo irregularidad"

                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        DetalleIrregularidad1.HabilitaCampos = True
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True

                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        DetalleClasifOPI1.Visible = True
                        btnAceptar5.Visible = False
                        btnSalir5.Visible = False
                        btnCorreo5.Visible = False
                        xpnlPaso5.Visible = True

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar5x15.Enabled = True
                        Else
                            btnAceptar5x15.Enabled = False
                        End If

                    Case 15

                        lblPaso.Text = "Paso 5 : No hubo irregularidad"
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.MuestraIrregStd = False
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.HabilitaCampos = _opiDetalle.B_EXISTE_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        'de aqui para abajo se compia de estatus 17
                        btnAceptar5.Visible = False
                        btnSalir5.Visible = False
                        btnAceptar5x15.Visible = False

                        xpnlPaso5.Visible = True
                        btnCorreo5.Visible = False
                        pnlDetalleComentOPI.Visible = False

                        'Se comenta para ya no habilitar botpn guardar
                        'If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                        '    puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                        '    puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                        '    btnAceptar5x15.Enabled = False ' se cambia a false para ya no volver a hablitar
                        'Else
                        '    btnAceptar5x15.Enabled = False
                        'End If

                    Case 16
                        lblPaso.Text = "Paso 5 : No hubo irregularidad"
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.HabilitaCampos = _opiDetalle.B_EXISTE_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        btnAceptar5.Visible = False
                        btnSalir5.Visible = False
                        btnAceptar5x15.Visible = False

                        xpnlPaso5.Visible = True
                        btnCorreo5.Visible = True
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) Then
                            btnCorreo5.Enabled = True
                        Else
                            btnCorreo5.Enabled = False
                        End If
                    Case 17
                        lblPaso.Text = "Paso 5 : No hubo irregularidad"
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.HabilitaCampos = _opiDetalle.B_EXISTE_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        btnAceptar5.Visible = False
                        btnSalir5.Visible = False
                        btnAceptar5x15.Visible = False

                        xpnlPaso5.Visible = True
                        btnCorreo5.Visible = False
                        pnlDetalleComentOPI.Visible = False
                End Select
            Case 6  '  Paso 7 : Notifica OPI Inicial

                Select Case _opiDetalle.I_ID_ESTATUS
                    Case 18
                        lblPaso.Text = "Paso 6: Revisión de irregularidad con VJ"

                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG

                        DetalleOPIuc.Visible = True
                        DetalleIrregularidad1.Visible = True
                        DetalleClasifOPI1.Visible = True
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.HabilitaCampos = False
                        btnSalir6.Visible = False
                        btnAceptar6.Visible = False
                        xpnlPaso6.Visible = True
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnCorreo6.Enabled = True
                        Else
                            btnCorreo6.Enabled = False
                        End If

                    Case 9
                        lblPaso.Text = "Paso 6: En espera de información"

                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG

                        DetalleOPIuc.Visible = True
                        DetalleIrregularidad1.Visible = True
                        DetalleClasifOPI1.Visible = True
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.HabilitaCampos = False
                        btnSalir6.Visible = False
                        btnAceptar6.Visible = False
                        xpnlPaso6.Visible = True
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnCorreo6.Enabled = True
                        Else
                            btnCorreo6.Enabled = False
                        End If


                End Select

            Case 7  'Paso 7 :  Notifica OPI INICIAL
                'jj.v.s
                lblNota.Text = " Nota: Al realizar una cancelación del oficio de observaciones, el registro quedará en modo de solo lectura. "
                If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM AndAlso
                    puObjUsuario.IdArea = Constantes.AREA_PR) Then
                    btnCancelarOpiP7.Visible = True
                Else
                    btnCancelarOpiP7.Visible = False
                End If
                Select Case _opiDetalle.I_ID_ESTATUS
                    Case 9, 46
                        lblPaso.Text = "Paso 7 : Requerimiento de información Adicional"
                        xpnlPaso7.Visible = True
                        btnNotificarP7.Visible = False
                        btnAceptar7.Enabled = False
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        DetalleIrregularidad1.HabilitaCampos = True
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar7.Enabled = True
                        Else
                            btnAceptar7.Enabled = False
                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If


                    Case 19, 20, 21, 4
                        lblPaso.Text = "Paso 7 : Elaboración de Oficio de Observaciones"
                        xpnlPaso7.Visible = True
                        'btnNotificarP7.Enabled = False
                        btnNotificarP7.Visible = False
                        'btnAceptar7.Enabled = True
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        DetalleIrregularidad1.HabilitaCampos = True
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar7.Enabled = True
                        Else
                            btnAceptar7.Enabled = False

                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                    Case 22 'OPI Clasificado
                        lblPaso.Text = "Paso 7 : Notifica Oficio de Observaciones"
                        xpnlPaso7.Visible = True
                        'FSW RLZ SOFTTEK Inicia Correccion de incidencia 52, Documento 38 
                        If _opiDetalle.N_ID_PASO_ANT = 9 Then
                            btnNotificarP7.CommandName = "Notificar_Paso7a"
                        End If
                        'FSW RLZ SOFTTEK Termina Correccion de incidencia 52, Documento 38 
                        'btnNotificarP7.Enabled = True
                        'btnAceptar7.Enabled = False
                        btnAceptar7.Visible = False
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        DetalleIrregularidad1.HabilitaCampos = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnNotificarP7.Enabled = True
                        Else
                            btnNotificarP7.Enabled = False
                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If
                    Case 48
                        lblPaso.Text = "Paso 7 : Cancelado"
                        xpnlPaso7.Visible = True
                        btnCancelarOpiP7.Visible = False
                        btnNotificarP7.Visible = False
                        btnAceptar7.Visible = False
                        ImageButton3.Visible = True
                        btnDetalle7.Visible = True
                End Select
            Case 8
                Select Case _opiDetalle.I_ID_ESTATUS
                    Case 9

                        lblPaso.Text = "Paso 8 : Respuesta AFORE "
                        xpnlPaso8.Visible = True

                        btnNotificarP8.Visible = False
                        'btnAceptar8.Visible = true
                        DetalleRespAforeOPI1.SetRespOPIInicial = True
                        DetalleRespAforeOPI1.Visible = True
                        DetalleRespAforeOPI1.ComboVisible = True

                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        pnlIrregularidad.Visible = True
                        pnlIrregularidad.Enabled = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        DetalleIrregularidad1.Visible = True
                        divFechaEstimadaEntrega.Visible = True
                        txtFecEstimadaEntrega.Text = IIf(IsDBNull(_opiDetalle.F_FECH_ESTIM_ENTREGA), "", _opiDetalle.F_FECH_ESTIM_ENTREGA)
                        txtFecEstimadaEntrega.ReadOnly = True

                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar8.Enabled = True
                        Else
                            btnAceptar8.Enabled = False
                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                    Case 23
                        lblPaso.Text = "Paso 8 : Respuesta AFORE a Oficio de Observaciones"
                        xpnlPaso8.Visible = True

                        btnNotificarP8.Visible = False
                        btnAceptar8.Visible = True

                        DetalleRespAforeOPI1.SetRespOPIInicial = True
                        DetalleRespAforeOPI1.Visible = True
                        DetalleRespAforeOPI1.ComboVisible = True

                        divFechaEstimadaEntrega.Visible = True

                        pnlIrregularidad.Visible = True
                        pnlIrregularidad.Enabled = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG

                        'Validar la fecha de estimacion de entrega
                        If IsNothing(_opiDetalle.F_FECH_ESTIM_ENTREGA) Then
                            txtFecEstimadaEntrega.Text = ""
                        Else
                            txtFecEstimadaEntrega.Text = _opiDetalle.F_FECH_ESTIM_ENTREGA
                        End If

                        txtFecEstimadaEntrega.ReadOnly = True



                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar8.Enabled = True
                        Else
                            btnAceptar8.Enabled = False
                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                    Case 25

                        lblPaso.Text = "Paso 8 : Respuesta AFORE"
                        xpnlPaso8.Visible = True

                        btnNotificarP8.Visible = True
                        btnAceptar8.Visible = False

                        DetalleRespAforeOPI1.Visible = True
                        DetalleRespAforeOPI1.ComboVisible = False
                        DetalleRespAforeOPI1.ValRespAfore = _opiDetalle.T_DSC_RESP_AFORE

                        divFechaEstimadaEntrega.Visible = True
                        txtFecEstimadaEntrega.Text = _opiDetalle.F_FECH_ESTIM_ENTREGA
                        txtFecEstimadaEntrega.ReadOnly = True

                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        pnlIrregularidad.Visible = True
                        pnlIrregularidad.Enabled = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.Visible = True

                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnNotificarP8.Enabled = True
                        Else
                            btnNotificarP8.Enabled = False
                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                End Select

            Case 9  ' Paso 10 : No Procede OPI
                Select Case _opiDetalle.I_ID_ESTATUS
                    Case 12 ' Notifica Respuesta AFORE
                        lblPaso.Text = "Paso 9 : Análisis del área de vigilancia"
                        xpnlPaso9.Visible = True
                        btnSolicitarP9.Visible = True
                        btnNotificarP9.Visible = False
                        btnAceptar9.Enabled = True
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        pnlIrregularidad.Enabled = False

                        'DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        'DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        'DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        'DetalleIrregularidad1.HabilitaCampos = True
                        'DetalleIrregularidad1.MuestraIrregStd = True
                        'DetalleIrregularidad1.Visible = True
                        'DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar9.Enabled = True
                        Else
                            btnAceptar9.Enabled = False
                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                    Case 13 ' Finaliza Análisis
                        lblPaso.Text = "Paso 9 : Finaliza análisis del área de vigilancia"
                        xpnlPaso9.Visible = True
                        btnSolicitarP9.Visible = False
                        btnNotificarP9.Visible = True
                        btnAceptar9.Visible = False
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        DetalleIrregularidad1.HabilitaCampos = True
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG

                        DetallePosibIncumplim.Visible = True
                        DetallePosibIncumplim.AsignaPosibIncumpl = _opiDetalle.B_POSIBLE_INC
                        DetallePosibIncumplim.AsignaMotivoNOProcedencia = _opiDetalle.T_DSC_MOTIV_NO_PROC
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) Then
                            btnNotificarP9.Enabled = True
                        Else
                            btnNotificarP9.Enabled = False
                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                End Select
            Case 10
                lblNota.Text = "Nota: Se habilitará el documento de nota de cancelación en el expediente. "
                btnAprobarSEfecto.Visible = False
                btnRechazarSEfecto.Visible = False
                btnDejarSEfecto.Visible = False

                Select Case _opiDetalle.I_ID_ESTATUS
                    Case 27   ' (27) Notifica No Procede OPI
                        lblPaso.Text = "Paso 10 :  No procede Oficio de Observaciones"
                        xpnlPaso10.Visible = True
                        btnNotificarP10.Visible = False
                        btnAceptar10a.Visible = True
                        btnAceptar10b.Visible = False
                        btnAceptar10b2.Visible = False
                        btnAceptar10b3.Visible = False
                        btnSiSAN.Visible = False
                        btnDejarSEfecto.Visible = False

                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        DetalleIrregularidad1.HabilitaCampos = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        DetallePosibIncumplim.Visible = True
                        DetallePosibIncumplim.AsignaPosibIncumpl = _opiDetalle.B_POSIBLE_INC
                        DetallePosibIncumplim.AsignaMotivoNOProcedencia = _opiDetalle.T_DSC_MOTIV_NO_PROC
                        DetallePosibIncumplim.ValActivaCampos = False
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar10a.Enabled = True
                        Else
                            btnAceptar10a.Enabled = False
                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                    Case 28   '(28) Oficio Cierre Elaborado
                        lblPaso.Text = "Paso 10 :  No procede Oficio de Observaciones"
                        xpnlPaso10.Visible = True
                        btnNotificarP10.Visible = True
                        btnAceptar10a.Visible = False
                        btnAceptar10b.Visible = False
                        btnAceptar10b2.Visible = False
                        btnAceptar10b3.Visible = False
                        btnSiSAN.Visible = False
                        btnNotificarP10.CommandName = "Notificar_Paso10a"
                        btnDejarSEfecto.Visible = False

                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        DetalleIrregularidad1.HabilitaCampos = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        DetallePosibIncumplim.Visible = True
                        DetallePosibIncumplim.AsignaPosibIncumpl = _opiDetalle.B_POSIBLE_INC
                        DetallePosibIncumplim.AsignaMotivoNOProcedencia = _opiDetalle.T_DSC_MOTIV_NO_PROC
                        DetallePosibIncumplim.ValActivaCampos = False
                        If _opiDetalle.B_PROCEDE AndAlso (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnNotificarP10.Enabled = True
                        Else
                            btnNotificarP10.Enabled = False
                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                    Case 26    '(26) Notifica Procede OPI
                        lblPaso.Text = "Paso 10 :  Notifica Procede OPI"
                        xpnlPaso10.Visible = True
                        btnNotificarP10.Visible = False
                        btnAceptar10a.Visible = False
                        btnAceptar10b.Visible = True
                        btnAceptar10b2.Visible = False
                        btnAceptar10b3.Visible = False
                        btnSiSAN.Visible = False
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        DetalleIrregularidad1.HabilitaCampos = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetallePosibIncumplim.Visible = True
                        DetallePosibIncumplim.AsignaPosibIncumpl = _opiDetalle.B_POSIBLE_INC
                        DetallePosibIncumplim.AsignaMotivoNOProcedencia = _opiDetalle.T_DSC_MOTIV_NO_PROC
                        DetallePosibIncumplim.ValActivaCampos = False
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar10b.Enabled = True
                        Else
                            btnAceptar10b.Enabled = False
                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                        If _opiDetalle.B_PROCEDE AndAlso (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                                                    puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                                                    puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnNotificarP10.Enabled = True

                        Else
                            btnNotificarP10.Enabled = False
                        End If

                        If _opiDetalle.B_PROCEDE AndAlso (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                                                    puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) Then
                            btnDejarSEfecto.Visible = True
                        Else
                            btnDejarSEfecto.Visible = False
                        End If


                    Case 30   '(30) Dictamen Elaborado
                        lblPaso.Text = "Paso 10 :  Notifica Resolución"
                        xpnlPaso10.Visible = True
                        btnNotificarP10.Visible = True
                        btnAceptar10a.Visible = False
                        btnAceptar10b.Visible = False
                        btnAceptar10b2.Visible = False
                        btnAceptar10b3.Visible = False
                        btnSiSAN.Visible = False
                        btnNotificarP10.CommandName = "Notificar_Paso10b"
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION

                        DetalleIrregularidad1.HabilitaCampos = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        DetallePosibIncumplim.Visible = True
                        DetallePosibIncumplim.AsignaPosibIncumpl = _opiDetalle.B_POSIBLE_INC
                        DetallePosibIncumplim.AsignaMotivoNOProcedencia = _opiDetalle.T_DSC_MOTIV_NO_PROC
                        DetallePosibIncumplim.ValActivaCampos = False
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnNotificarP10.Enabled = True
                        Else
                            btnNotificarP10.Enabled = False
                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                        If _opiDetalle.B_PROCEDE AndAlso (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnNotificarP10.Enabled = True
                        Else
                            btnNotificarP10.Enabled = False
                        End If

                        If _opiDetalle.B_PROCEDE AndAlso (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                                                    puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) Then
                            btnDejarSEfecto.Visible = True
                        Else
                            btnDejarSEfecto.Visible = False
                        End If


                    Case 31   '(31) Dictamen Notificado
                        lblPaso.Text = "Paso 10 :  Identifica Irregularidades"
                        xpnlPaso10.Visible = True
                        btnNotificarP10.Visible = False
                        btnAceptar10a.Visible = False
                        btnAceptar10b.Visible = False
                        btnAceptar10b2.Visible = True
                        btnAceptar10b3.Visible = False
                        btnSiSAN.Visible = False
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        DetalleIrregularidad1.HabilitaCampos = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        DetalleAltaIrregularidad.Inicializar()
                        DetalleAltaIrregularidad.ActivaAltaIrregularidad = True
                        DetalleAltaIrregularidad.ActivaBandejaIrregularidad = True
                        DetalleAltaIrregularidad.Visible = True
                        pnlDetalleComentOPI.Visible = False
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar10b2.Enabled = True
                        Else
                            btnAceptar10b2.Enabled = False
                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                        If _opiDetalle.B_PROCEDE AndAlso (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnNotificarP10.Enabled = True
                        Else
                            btnNotificarP10.Enabled = False
                        End If

                        If _opiDetalle.B_PROCEDE AndAlso (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                                                    puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) Then
                            btnDejarSEfecto.Visible = True
                        Else
                            btnDejarSEfecto.Visible = False
                        End If

                    Case 42   '(42) Irregularidades Registradas
                        lblPaso.Text = "Paso 10 :  Genera Dictamen"
                        xpnlPaso10.Visible = True
                        btnNotificarP10.Visible = False
                        btnAceptar10a.Visible = False
                        btnAceptar10b.Visible = False
                        btnAceptar10b2.Visible = False
                        btnAceptar10b3.Visible = True
                        btnSiSAN.Visible = False
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        DetalleIrregularidad1.HabilitaCampos = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        DetalleAltaIrregularidad.Inicializar()
                        DetalleAltaIrregularidad.ActivaAltaIrregularidad = True
                        DetalleAltaIrregularidad.ActivaBandejaIrregularidad = True
                        DetalleAltaIrregularidad.Visible = True
                        pnlDetalleComentOPI.Visible = False
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnAceptar10b3.Enabled = True
                        Else
                            btnAceptar10b3.Enabled = False
                        End If

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                        If _opiDetalle.B_PROCEDE AndAlso (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnNotificarP10.Enabled = True
                        Else
                            btnNotificarP10.Enabled = False
                        End If

                        If _opiDetalle.B_PROCEDE AndAlso (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) Then
                            btnDejarSEfecto.Visible = True
                        Else
                            btnDejarSEfecto.Visible = False
                        End If

                    Case 43    '(43) Enviar a SISAN
                        lblPaso.Text = "Paso 10 :  Enviar a SISAN"
                        xpnlPaso10.Visible = True
                        btnNotificarP10.Visible = False
                        btnAceptar10a.Visible = False
                        btnAceptar10b.Visible = False
                        btnAceptar10b2.Visible = False
                        btnAceptar10b3.Visible = False
                        btnSiSAN.Visible = True
                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        DetalleIrregularidad1.HabilitaCampos = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG
                        DetalleAltaIrregularidad.Inicializar()
                        DetalleAltaIrregularidad.ActivaAltaIrregularidad = True
                        DetalleAltaIrregularidad.ActivaBandejaIrregularidad = True
                        DetalleAltaIrregularidad.Visible = True
                        pnlDetalleComentOPI.Visible = False
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnSiSAN.Enabled = True
                        Else
                            btnSiSAN.Enabled = False
                        End If
                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If
                        If _opiDetalle.B_PROCEDE AndAlso (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnNotificarP10.Enabled = True
                        Else
                            btnNotificarP10.Enabled = False
                        End If

                        btnDejarSEfecto.Visible = False

                    Case 44    '(44) Enviado a SISAN
                        VisibilidadTabDatSisan(True)

                        lblPaso.Text = "Paso 10 :  Enviado a SISAN"
                        xpnlPaso10.Visible = True
                        btnNotificarP10.Visible = False
                        btnAceptar10a.Visible = False
                        btnAceptar10b.Visible = False
                        btnAceptar10b2.Visible = False
                        btnAceptar10b3.Visible = False
                        btnSiSAN.Visible = False
                        pnlDetalleComentOPI.Visible = False

                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        DetalleIrregularidad1.HabilitaCampos = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                        If _opiDetalle.B_PROCEDE AndAlso (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then
                            btnNotificarP10.Enabled = True
                        Else
                            btnNotificarP10.Enabled = False
                        End If

                        btnDejarSEfecto.Visible = False

                        hrSan.Visible = True

                    Case 29    '(29) Oficio Cierre Notificado
                        VisibilidadTabDatSisan(True)

                        lblPaso.Text = "Paso 10 :  Oficio de Observaciones Enviado"
                        xpnlPaso10.Visible = True
                        btnNotificarP10.Visible = False
                        btnAceptar10a.Visible = False
                        btnAceptar10b.Visible = False
                        btnAceptar10b2.Visible = False
                        btnAceptar10b3.Visible = False
                        btnSiSAN.Visible = False
                        pnlDetalleComentOPI.Visible = False

                        btnDejarSEfecto.Visible = False

                    Case 50 '(50) en proceso de cancelación

                        lblPaso.Text = "Paso 10 :  En Proceso de Cancelación"
                        xpnlPaso10.Visible = True
                        btnNotificarP10.Visible = False
                        btnAceptar10a.Visible = False
                        btnAceptar10b.Visible = False
                        btnAceptar10b2.Visible = False
                        btnAceptar10b3.Visible = False
                        btnSiSAN.Visible = False
                        pnlDetalleComentOPI.Visible = False

                        DetallePosibIncumplim.AsignaPosibIncumpl = _opiDetalle.B_POSIBLE_INC
                        DetallePosibIncumplim.Visible = True
                        pnlDetallePosibIncumplim.Enabled = False

                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        DetalleIrregularidad1.HabilitaCampos = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                        btnDejarSEfecto.Visible = False
                        If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM AndAlso
                            puObjUsuario.IdArea = Constantes.AREA_PR) Then
                            btnAprobarSEfecto.Visible = True
                            btnRechazarSEfecto.Visible = True
                        End If

                    Case 49
                        lblPaso.Text = "Paso 10 :  Se deja sin efecto"
                        xpnlPaso10.Visible = True
                        btnNotificarP10.Visible = False
                        btnAceptar10a.Visible = False
                        btnAceptar10b.Visible = False
                        btnAceptar10b2.Visible = False
                        btnAceptar10b3.Visible = False
                        btnSiSAN.Visible = False
                        pnlDetalleComentOPI.Visible = False

                        DetallePosibIncumplim.AsignaPosibIncumpl = _opiDetalle.B_POSIBLE_INC
                        DetallePosibIncumplim.Visible = True
                        pnlDetallePosibIncumplim.Enabled = False

                        DetalleClasifOPI1.Visible = True
                        DetalleClasifOPI1.ComboVisible = False
                        DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                        DetalleIrregularidad1.HabilitaCampos = False
                        DetalleIrregularidad1.MuestraIrregStd = True
                        DetalleIrregularidad1.MuestraExisteIrregularidad = True
                        DetalleIrregularidad1.Visible = True
                        DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                        DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                        DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG

                        If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                            DetalleIrregularidad1.MuestraIrregStd = False
                            DetalleIrregularidad1.Visible = False
                            DetalleIrregularidad1.MuestraExisteIrregularidad = False
                        End If

                        btnDejarSEfecto.Visible = False
                        btnAprobarSEfecto.Visible = False
                        btnRechazarSEfecto.Visible = False

                End Select
            Case 11
                lblPaso.Text = "Paso 10 :  Notifica Resolución"
                pnlDetalleOPI.Enabled = False
                pnlDetalleClasifOPI.Enabled = False
                pnlSupuestoAviso.Enabled = False
                pnlIrregularidad.Enabled = False
                pnlRespAfore.Enabled = False
                pnlDetallePosibIncumplim.Enabled = False
                pnlDetalleAltaIrregularidad.Enabled = False
                pnlDetalleComentOPI.Enabled = False


                xpnlPaso10.Visible = True
                btnNotificarP10.Visible = False
                btnAceptar10a.Visible = False
                btnAceptar10b.Visible = False
                btnAceptar10b2.Visible = False
                btnAceptar10b3.Visible = False
                btnSiSAN.Visible = True
                DetalleClasifOPI1.Visible = True
                DetalleClasifOPI1.ComboVisible = False
                DetalleClasifOPI1.ValClasificacion = _opiDetalle.T_DSC_CLASIFICACION
                DetalleIrregularidad1.HabilitaCampos = False
                DetalleIrregularidad1.MuestraIrregStd = True
                DetalleIrregularidad1.MuestraExisteIrregularidad = True
                DetalleIrregularidad1.Visible = True
                DetalleIrregularidad1.ValJustificacion = _opiDetalle.T_DSC_JUST_NO_IRREG
                DetalleIrregularidad1.ValIrregStand = _opiDetalle.B_IRREG_STD
                DetalleIrregularidad1.ValIsIrregularidad = _opiDetalle.B_EXISTE_IRREG

                pnlDetalleComentOPI.Visible = False

                If (_opiDetalle.T_DSC_CLASIFICACION = "Oficio de Observaciones") Then
                    DetalleIrregularidad1.MuestraIrregStd = False
                    DetalleIrregularidad1.Visible = False
                    DetalleIrregularidad1.MuestraExisteIrregularidad = False
                End If

            Case Else

        End Select
    End Sub

    Private Sub VisibilidadTabDatSisan(isVisible As Boolean)
        If isVisible Then
            If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP OrElse
                            puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS) Then

                liExpedientes.Visible = True
                tabExpedienteSISAN.Visible = True
                SancionPC1.Visible = True
                SancionPC1.Inicializar(_opiDetalle.T_ID_FOLIO_SISAN, True)

            End If
        Else
            liExpedientes.Visible = False
            tabExpedienteSISAN.Visible = False
            SancionPC1.Visible = False
        End If
    End Sub

    Protected Sub btnAceptar1_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar1.Click
        If ValidaDatos(1, , True) Then
            btnAceptarM2B1A.CommandArgument = "SaveStep1.-1"
            lblTitle.Text = "Confirmación"
            'btnAceptarM2B1A.CommandArgument = "Guardar1"
            Mensaje = "¿Estás seguro que deseas completar la información?"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

        Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            lblTitle.Text = "Alerta - Información Incompleta"
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If
    End Sub

    Protected Sub btnAceptar2_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar2.Click
        If ValidaDatos(2, , True) Then
            lblTitle.Text = "Confirmación"
            btnAceptarM2B1A.CommandArgument = "SaveStep2.0"
            'btnAceptarM2B1A.CommandArgument = "Guardar2"
            Mensaje = "¿Estás seguro que deseas completar la información?"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            lblTitle.Text = "Alerta - Información Incompleta"
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If
    End Sub

    Private Function ValidaDatos(_step As Integer, Optional _substep As Integer = -1, Optional _blnValidaComentarios As Boolean = False) As Boolean
        'Private Function ValidaDatos(blnValidaComentarios As Boolean, blnValidaClasificacion As Boolean) ''PCMT: Cambio
        Dim blnSinError As Boolean = True

        lstMensajes = New List(Of String)

        If _blnValidaComentarios Then
            If DetalleComentOPI1.ValComentarios = "" Then
                lstMensajes.Add("Debe capturar los comentarios.")
                blnSinError = False
            End If
        End If

        Select Case _step
            Case 1

            Case 2
                Select Case _substep
                    Case -1
                        If DetalleClasifOPI1.ValIndexClasificacion <= 0 Then
                            lstMensajes.Add("Debe seleccionar la clasificación del Oficio")
                            blnSinError = False
                        End If
                    Case 1

                        If _opiDetalle.T_DSC_CLASIFICACION = "Aviso de conocimiento" Then
                            If DetalleSupuestoAvisoOPI1.ValIndexSupuestoAviso <= 0 Then
                                lstMensajes.Add("Debe seleccionar el supuesto aviso de conocimiento.")
                                blnSinError = False
                            End If
                        End If


                        If Not DocumentoAdjunto() Then
                            lstMensajes.Add("Hay documentos obligatorios que adjuntar.")
                            blnSinError = False
                        End If


                End Select

            Case 3

                If DetalleRespAforeOPI1.ValIndexRespuesta <= 0 Then
                    lstMensajes.Add("Debes indicar el tipo de respuesta de la AFORE.")
                    blnSinError = False
                End If

                If Not DocumentoAdjunto(DetalleRespAforeOPI1.ValIndexRespuesta) Then
                    lstMensajes.Add("Hay documentos obligatorios que adjuntar.")
                    blnSinError = False
                End If

                Select Case _substep
                    Case 1
                        If String.IsNullOrEmpty(txtDiasProrroga.Text) OrElse txtDiasProrroga.Text = "0" Then
                            lstMensajes.Add("Debes indicar los días de prórroga.")
                            blnSinError = False
                        End If
                    Case Else

                End Select
            Case 4
                Select Case _substep
                    Case 1
                        If (txtFechaReunion4.Text = "") Then
                            lstMensajes.Add("Debe seleccionar una fecha para la reunión")
                            blnSinError = False
                        End If
                        If txtFechaReunion4.Text <> String.Empty Then
                            If Date.ParseExact(txtFechaReunion4.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) < Date.Now.Date.ToShortDateString() Then
                                lstMensajes.Add("La fecha asignada para la reunion debe ser posterior o igual al  día de hoy")
                                blnSinError = False
                            End If
                        End If
                    Case 2
                        If Not DocumentoAdjunto(1) Then
                            lstMensajes.Add("Hay información que debes completar para poder continuar, revisa que ya este registrado el documento en SICOD.")
                            blnSinError = False
                        End If

                    Case 3
                        If Not DocumentoAdjunto(2) Then
                            lstMensajes.Add("Hay información que debes completar para poder continuar, revisa que ya este registrado el documento en SICOD.")
                            blnSinError = False
                        End If
                        If (DetalleIrregularidad1.ValIsIrregularidad = "0") Then
                            If (DetalleIrregularidad1.ValJustificac = "") Then
                                lstMensajes.Add("Debe capturar una justificación de no irregularidad")
                                blnSinError = False
                            End If
                        End If
                End Select


            Case 5
                If Not DocumentoAdjunto() Then
                    lstMensajes.Add("Hay información que debes completar para poder continuar, revisa que ya este registrado el documento en SICOD.")
                    blnSinError = False
                End If

            Case 6
                Select Case _substep
                    Case 1
                        If Not DocumentoAdjunto() Then
                            lstMensajes.Add("Hay documentos obligatorios que adjuntar.")
                            blnSinError = False
                        End If
                    Case 3
                        'FSW RLZ SOFTTEK Inicia Correccion de incidencia 102, Documento 38 
                        'If txtFechaReunion4.Text <> String.Empty Then
                        '    If Date.ParseExact(txtFechaReunion4.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) < Date.Now.Date.ToShortDateString() Then
                        '        lstMensajes.Add("La fecha asignada para la reunion debe ser posterior o igual al  día de hoy")
                        '        blnSinError = False
                        '    End If
                        'End If
                        'FSW RLZ SOFTTEK Termina Correccion de incidencia 102, Documento 38 
                End Select
            Case 7
                If Not DocumentoAdjunto() Then
                    lstMensajes.Add("Hay documentos obligatorios que adjuntar.")
                    blnSinError = False
                End If
            Case 8
                Select Case _substep
                    Case 1
                        If String.IsNullOrEmpty(txtDiasProrroga.Text) OrElse txtDiasProrroga.Text = "0" Then
                            lstMensajes.Add("Debes indicar los días de prórroga.")
                            blnSinError = False
                        End If
                    Case 2
                        If (_opiDetalle.I_ID_ESTATUS = 25 And txtFechaReunion4.Text = "") Then
                            lstMensajes.Add("Debe seleccionar una fecha para la reunión")
                            blnSinError = False
                        End If

                    Case Else
                        If DetalleRespAforeOPI1.ValIndexRespuesta <= 0 Then
                            lstMensajes.Add("Debes indicar el tipo de respuesta de la AFORE.")
                            blnSinError = False
                        End If

                        If Not DocumentoAdjunto(DetalleRespAforeOPI1.ValIndexRespuesta) Then
                            lstMensajes.Add("Hay documentos obligatorios que adjuntar.")
                            blnSinError = False
                        End If

                        If txtFechaReunion4.Text <> String.Empty Then
                            If Date.ParseExact(txtFechaReunion4.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) < Date.Now.Date.ToShortDateString() Then
                                lstMensajes.Add("La fecha asignada para la reunion debe ser mayor o igual al día de hoy")
                                blnSinError = False
                            End If
                        End If


                End Select


            Case 9
                Select Case _substep
                    Case 2
                        If DetallePosibIncumplim.ValPosibIncumplim = "0" Then
                            If DetallePosibIncumplim.ValMotivoNOProced = "" Then
                                lstMensajes.Add("Debe especificar el motivo de NO PROCEDENCIA.")
                                blnSinError = False
                            End If
                        End If
                End Select

            Case 10
                Select Case _substep
                    Case 1
                        If Not DocumentoAdjunto() Then
                            lstMensajes.Add("Hay documentos obligatorios que debes adjuntar.")
                            blnSinError = False
                        End If
                    Case 2
                        If Not Entities.IrregularidadOPI.ExistenIrregularidades(Session("I_ID_OPI")) Then
                            lstMensajes.Add("Es necesario agregar por lo menos una irregularidad.")
                            blnSinError = False
                        End If
                        If Not DocumentoAdjunto() Then
                            lstMensajes.Add("Hay documentos obligatorios que debes adjuntar.")
                            blnSinError = False
                        End If
                    Case 3
                        If Not DocumentoAdjunto() Then
                            'FSW CAGC SOFTTEK Incidencia Corregida Núm 82, Documento 38
                            lstMensajes.Add("Debes registrar el dictamen en SICOD para enviar a Sanciones.")
                            blnSinError = False
                        End If
                End Select
            Case 11
                If DocumentoAdjunto() Then
                    lstMensajes.Add("Existen documentos registrados.")
                    blnSinError = False
                End If
            Case 12
                If DetalleOPIuc.FechaPI.Trim = String.Empty Then
                    lstMensajes.Add("Debe de seleccionar la fecha del oficio.")
                    blnSinError = False
                End If
                If DetalleOPIuc.ProcesoPO = "-1" Or String.IsNullOrEmpty(DetalleOPIuc.ProcesoPO) Then
                    lstMensajes.Add("Debe seleccionar un proceso.")
                    blnSinError = False
                End If
                If DetalleOPIuc.SubprocesoPO = "-1" Or String.IsNullOrEmpty(DetalleOPIuc.SubprocesoPO) Then
                    lstMensajes.Add("Debe seleccionar un Subproceso.")
                    blnSinError = False
                End If
                If DetalleOPIuc.GetSupervisoresSel().Count() = 0 Then
                    lstMensajes.Add("Debe de seleccionar al menos un supervisor.")
                    blnSinError = False
                End If
                If DetalleOPIuc.GetInspectoresSel().Count() = 0 Then
                    lstMensajes.Add("Debe de seleccionar al menos un Inspector.")
                    blnSinError = False
                End If
        End Select

        Return blnSinError
    End Function

    Protected Sub GuardarDatos1(Optional _primero As Boolean = False)
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String

        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(1)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
        scomentarios = DetalleComentOPI1.ValComentarios

        If _primero Then
            ' Ya lleva el estatus 1
            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(_opiDetalle.I_ID_ESTATUS)
            'Session("ComentariosOPI") = scomentarios
        Else
            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(2)
            'DetalleComentOPI1.ValComentarios = _opiDetalle.T_COMENTARIOS_PASOS ' Session("ComentariosOPI")
            'scomentarios = ""
        End If

        blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(1), "Notifica registro de un Oficio", scomentarios)

        If blnResultado Then
            If _primero Then
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                lblTitle.Text = "Guardado Correcto"
                imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                Mensaje = "Se ha guardado correctamente la información." 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
            Else
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
            End If

        Else
            ''Notificar Error
        End If

    End Sub

    Protected Sub btnHome1_Click(sender As Object, e As ImageClickEventArgs) Handles btnHome1.Click
        'btnAceptarM2B1A.CommandArgument = "HomeOPI"
        'lblTitle.Text = "Confirmación"
        'Mensaje = "Está a punto de salir de la pantalla actual y regresar a pantalla inicial. ¿Deseas Continuar?"
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeSalir();", True)
    End Sub

    Protected Sub btnHome_Command(sender As Object, e As CommandEventArgs)
        btnAceptarM2B1A.CommandArgument = "HomeOPI"
        lblTitle.Text = "Confirmación"
        Mensaje = "Está a punto de salir de la pantalla actual y regresar a pantalla inicial. ¿Deseas Continuar?"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeSalir();", True)
    End Sub

    Protected Sub GuardarDatos2(_paso As Integer)
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String
        Dim EstadoBitacora As String = ""
        scomentarios = DetalleComentOPI1.ValComentarios

        Select Case _paso
            Case 1 '' Inserta la Clasificacion,  comentarios y actualiza el estatus a OPI Clasificado
                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
                ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                ListaCampos.Add("T_DSC_CLASIFICACION") : ListaValores.Add(DetalleClasifOPI1.ValClasificacion)
                'Session("ComentariosOPI") = scomentarios
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(3)
                EstadoBitacora = "Realiza la clasificación del Oficio"

            Case 2 ' Envia notificacion y cambia estatus a OPI Notificado

                Select Case _opiDetalle.T_DSC_CLASIFICACION
                    Case "Aviso de conocimiento"
                        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
                        ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(1)
                        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(4)
                    Case "Requerimiento de información"
                        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
                        ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(2)
                        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(45)
                    Case "Oficio de Observaciones"
                        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(7)
                        ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(4)
                End Select
                EstadoBitacora = "Notifica la clasificación del Oficio"



            Case 3 '' Actualiza el estatus y el Subpaso dependiendo el tipo de clasificacion 

                If ValidaDatos(2, 2, True) Then


                    ListaCampos.Add("I_ID_SUPUESTO") : ListaValores.Add(DetalleSupuestoAvisoOPI1.ValSupuestoAviso)

                    'TODO: Recuperar la fecha de acuse del documento
                    ListaCampos.Add("F_FECH_ACUSE_DOCTO") : ListaValores.Add(Date.Now)
                    'TODO: Recuperar la fecha de acuse del documento


                    Select Case _opiDetalle.T_DSC_CLASIFICACION
                        Case "Aviso de conocimiento"
                            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
                            'ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                            ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(1)
                            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(5)
                            Dim FechaEstimada As String = ObtenerFechaEstimada(_opiDetalle.I_ID_OPI, 1)
                            If FechaEstimada = "" Then
                                ListaCampos.Add("F_FECH_ESTIM_ENTREGA") : ListaValores.Add("")
                            Else
                                ListaCampos.Add("F_FECH_ESTIM_ENTREGA") : ListaValores.Add(Date.ParseExact(FechaEstimada.Substring(0, 10), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
                            End If
                            EstadoBitacora = "Elabora Aviso de conocimiento"
                        Case "Requerimiento de información"
                            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
                            'ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                            ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(2)
                            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(7)
                            Dim FechaEstimada As String = ObtenerFechaEstimada(_opiDetalle.I_ID_OPI, 2)
                            If FechaEstimada = "" Then
                                ListaCampos.Add("F_FECH_ESTIM_ENTREGA") : ListaValores.Add("")
                            Else
                                ListaCampos.Add("F_FECH_ESTIM_ENTREGA") : ListaValores.Add(Date.ParseExact(FechaEstimada.Substring(0, 10), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
                            End If
                            EstadoBitacora = "Elabora Requerimiento de información"
                        Case "Oficio de Observaciones"
                            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(7)
                            'ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                            ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(4)
                            EstadoBitacora = "Elabora Oficio de Observaciones"
                    End Select

                    ''GUARDAR LOS DOCUMENTOS ADJUNTOS
                End If

            Case 4 ''Notifica clasificacion y actualiza estatus
                If ValidaDatos(1, , True) Then
                    Select Case _opiDetalle.T_DSC_CLASIFICACION
                        Case "Aviso de conocimiento"
                            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
                            ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                            ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(1)
                            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(6)
                            EstadoBitacora = "Notifica Aviso de conocimiento"
                        Case "Requerimiento de información"
                            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(3)
                            ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                            ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(3)
                            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(8)
                            EstadoBitacora = "Notifica Requerimiento de Información"
                        Case "Oficio de Observaciones"
                            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(7)
                            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(22)
                            ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                            ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                            EstadoBitacora = "Notifica Oficio de Observaciones"
                    End Select
                End If
        End Select

        blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)

        '*******VALIDAR BITACORA*********
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(2), EstadoBitacora, scomentarios)

        If blnResultado Then
            If blnResultado Then
                If _paso = 1 Or _paso = 3 Then
                    lblTitle.Text = "Guardado Correcto"
                    btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                    imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                    Mensaje = "Se ha guardado correctamente la información." 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
                Else
                    btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                    btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
                End If

            Else
                ''Notificar Error
            End If
        End If
    End Sub

    Protected Sub btnAceptar21_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar21.Click
        If ValidaDatos(2, 1, True) Then
            btnAceptarM2B1A.CommandArgument = "SaveStep2.3"
            'btnAceptarM2B1A.CommandArgument = "Guardar2"
            lblTitle.Text = "Confirmación"
            Mensaje = "¿Estás seguro que deseas completar la información?"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            lblTitle.Text = "Alerta - Información Incompleta"
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If

    End Sub

    Protected Sub GuardarDatos31()
        btnAceptarM2B1A.CommandArgument = "SaveStep3.1"
        lblTitle.Text = "Confirmación"
        Mensaje = "Se actualizará la fecha de entrega de información, ¿Deseas continuar?"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

    End Sub
    Protected Sub GuardarDatos3(Optional _step As Int16 = 1)
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String
        Dim _bvalidado As Boolean
        Dim EstadoBitacora As String = ""
        Dim _blnProrroga As Boolean = False
        Dim _diasprorroga As Double
        Dim _newFechaProrroga As Date
        Dim strNewFoliOOPI As String

        scomentarios = DetalleComentOPI1.ValComentarios
        Select Case _step
            Case 1
                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(3)
                ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                ListaCampos.Add("T_DSC_RESP_AFORE") : ListaValores.Add(DetalleRespAforeOPI1.ValRespAfore)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(10)
                _bvalidado = True
                EstadoBitacora = "Guarda Respuesta de AFORE [RESPUESTA A REQ]"
            Case 2
                If ValidaDatos(3, 1, True) Then
                    ListaCampos.Add("N_ID_PASO") : ListaValores.Add(3)
                    ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                    ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                    ListaCampos.Add("T_DSC_RESP_AFORE") : ListaValores.Add(DetalleRespAforeOPI1.ValRespAfore)
                    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(9)

                    _diasprorroga = Double.Parse(txtDiasProrroga.Text)

                    '_newFechaProrroga = DateAdd("d", _diasprorroga, _opiDetalle.F_FECH_ESTIM_ENTREGA)
                    _newFechaProrroga = BandejaPC.DiasHabiles(_opiDetalle.F_FECH_ESTIM_ENTREGA, _diasprorroga)
                    ListaCampos.Add("F_FECH_ESTIM_ENTREGA") : ListaValores.Add(_newFechaProrroga)

                    _bvalidado = True
                    _blnProrroga = True
                    EstadoBitacora = "Guarda Respuesta de AFORE [PRORROGA]"
                Else

                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                    Mensaje = "<ul>"
                    For Each msg In lstMensajes
                        Mensaje += "<li>" + msg + "</li>"
                    Next
                    Mensaje += "</ul>"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)

                    _bvalidado = False
                End If

            Case 3
                If ValidaDatos(8, 3, False) Then
                    ListaCampos.Add("N_ID_PASO") : ListaValores.Add(3)
                    ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                    ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                    ListaCampos.Add("T_DSC_RESP_AFORE") : ListaValores.Add(DetalleRespAforeOPI1.ValRespAfore)
                    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(9)

                    Dim _opicopia As New Registro_OPI
                    Dim strFoliosOPI As String = ""

                    _opicopia.T_ID_FOLIO = _opiDetalle.T_ID_FOLIO.Substring(_opiDetalle.T_ID_FOLIO.IndexOf("/", 0))
                    _opicopia.N_ID_PASO = 7
                    _opicopia.N_ID_SUBPASO = 0
                    _opicopia.I_ID_TIPO_ENTIDAD = _opiDetalle.I_ID_TIPO_ENTIDAD
                    _opicopia.I_ID_ENTIDAD = _opiDetalle.I_ID_ENTIDAD

                    ''PCMT --> NO habia definicion sobre las sub entidades
                    _opicopia.I_ID_SUBENTIDAD = _opiDetalle.I_ID_SUBENTIDAD
                    _opicopia.F_FECH_POSIBLE_INC = _opiDetalle.F_FECH_POSIBLE_INC
                    _opicopia.I_ID_PROCESO_POSIBLE_INC = _opiDetalle.I_ID_PROCESO_POSIBLE_INC
                    _opicopia.I_ID_SUBPROCESO = _opiDetalle.I_ID_SUBPROCESO
                    _opicopia.T_DSC_POSIBLE_INC = _opiDetalle.T_DSC_POSIBLE_INC + " " + _opiDetalle.T_ID_FOLIO + " No hay respuesta por falta de entrega de informacion en tiempo establecido" '
                    _opicopia.T_DSC_PROC_POSIB_INCUMP = _opiDetalle.T_DSC_PROC_POSIB_INCUMP
                    _opicopia.T_OBSERVACIONES_OPI = _opiDetalle.T_OBSERVACIONES_OPI
                    '_opicopia.T_OBSERVACIONES_OPI = _opiDetalle.T_OBSERVACIONES_OPI
                    _opicopia.I_ID_ESTATUS = 4
                    _opicopia.I_ID_AREA = _opiDetalle.I_ID_AREA
                    _opicopia.F_FECH_PASO_ACTUAL = DateTime.Now
                    _opicopia.N_ID_PASO_ANT = 3

                    _opicopia.T_DSC_CLASIFICACION = "Oficio de Observaciones"
                    _opicopia.T_DSC_RESP_AFORE = DetalleRespAforeOPI1.ValRespAfore
                    _opicopia.F_FECH_ESTIM_ENTREGA = BandejaPC.DiasHabiles(_opiDetalle.F_FECH_ACUSE_DOCTO, txtDiasProrroga.Text)
                    _opicopia.F_FECH_ACUSE_DOCTO = _opiDetalle.F_FECH_ACUSE_DOCTO
                    _opicopia.T_ID_SUPERVISORES = _opiDetalle.T_ID_SUPERVISORES
                    _opicopia.T_ID_INSPECTORES = _opiDetalle.T_ID_INSPECTORES


                    _bvalidado = _opicopia.Agregar(strFoliosOPI)
                    EstadoBitacora = "Guarda Respuesta de AFORE [NO HAY RESPUESTA A REQ]"
                Else

                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                    Mensaje = "<ul>"
                    For Each msg In lstMensajes
                        Mensaje += "<li>" + msg + "</li>"
                    Next
                    Mensaje += "</ul>"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)

                    _bvalidado = False
                End If
            Case 4
                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(4)
                ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(12)
                _bvalidado = True
                EstadoBitacora = "Notifica Respuesta AFORE"
        End Select

        If _bvalidado = True Then
            blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
            BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(3), EstadoBitacora, scomentarios)
        End If
        If _blnProrroga Then
            blnResultado = GuardarHistoricoProrroga(_opiDetalle.I_ID_OPI, 3, _diasprorroga, _newFechaProrroga.ToString)
        End If

        If blnResultado Then
            If _step = 3 Then
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                lblTitle.Text = "Guardado Correcto"
                imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                strNewFoliOOPI = _opiDetalle.T_ID_FOLIO.Substring(_opiDetalle.T_ID_FOLIO.IndexOf("/", 0))
                strNewFoliOOPI = _opiFunc.ObtenerNuevoFolioOPI(strNewFoliOOPI)
                Mensaje = "Se registró correctamente el nuevo Oficio de Observaciones " & strNewFoliOOPI & "." 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
            ElseIf _step < 4 And _step <> 3 Then
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                lblTitle.Text = "Guardado Correcto"
                imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                Mensaje = "Se ha guardado correctamente la información." 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
            Else
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
            End If

        Else
            ''Notificar Error
        End If

    End Sub
    Protected Sub GuardaDatos4_13(Optional _primero As Boolean = False)
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String


        If _primero Then

        Else
            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(5)
            ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
            ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(15)  'OPI Inicial Notificado
        End If

        scomentarios = DetalleComentOPI1.ValComentarios

        blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(4), "Notifica No existe irregularidad", scomentarios)
        If blnResultado Then
            If _primero Then

                lblTitle.Text = "Confirmación"
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                Mensaje = "Se ha guardado correctamente la información." 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
            Else
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
            End If

        Else
            ''Notificar Error
        End If


    End Sub

    Protected Sub SolicitaFechaReunion(_commandargument As String)
        btnAceptarM2B1A.CommandArgument = _commandargument
        Mensaje = "Ingresa la fecha estimada para revisar con la Vicepresidencia Jurídica la irregularidad identificada."
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "FechaReunion", " FechaReunion();", True)
    End Sub
    Protected Sub GuardaDatos4_13_2()
        btnAceptarM2B1A.CommandArgument = "GuardaDatos4_13_3"
        Mensaje = "Ingresa la fecha estimada para revisar con la Vicepresidencia Jurídica la irregularidad identificada."
        btnAceptarM3B1A.CommandArgument = "SaveStep4_13"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "FechaReunion", " FechaReunion();", True)
    End Sub

    Protected Sub GuardaDatos4_13_3()
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String

        If ValidaDatos(4, 1, True) Then
            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(6)
            ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
            ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(18)  'OPI Inicial Notificado
            ListaCampos.Add("F_FECH_REUNION") : ListaValores.Add(Date.ParseExact(txtFechaReunion4.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))

            scomentarios = DetalleComentOPI1.ValComentarios
            blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
            BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(4), "Notifica si existe irregularidad", scomentarios)
            If blnResultado Then
                EnviaNotificacionOPI(118) 'ammm 29-07-2019 agrego para enviar correo una vez que ya se guardo
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                lblTitle.Text = "Confirmación"
                imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                Mensaje = "Se ha guardado correctamente la información." 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
            End If
        Else
            lblTitle.Text = "Alerta - Información Incompleta"
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If


    End Sub


    Protected Sub GuardaDatos5(Optional _primero As Boolean = False)
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String


        If _primero Then
            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(5)
            ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
            ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(16)  'OPI Inicial Elaborado
        Else
            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(5)
            ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
            ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(17)  'OPI Inicial Notificado
        End If

        scomentarios = DetalleComentOPI1.ValComentarios

        blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)

        If blnResultado Then
            If _primero Then
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                lblTitle.Text = "Confirmación"
                imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                Mensaje = "Se ha guardado correctamente la información." 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
            Else
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
            End If

        Else
            ''Notificar Error
        End If


    End Sub
    ''' <summary>
    ''' Reunion realizada
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub GuardaDatos6_1()
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String

        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(7)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
        ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(19)  'OPI Inicial Notificado

        scomentarios = DetalleComentOPI1.ValComentarios
        blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(6), "Notifica Reunión con VJ realizada", scomentarios)
        If blnResultado Then
            btnAceptarM1B1A.CommandArgument = "RedirectSelf"
            btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
        End If

    End Sub
    ''' <summary>
    ''' Reunion NO realizada
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub GuardaDatos6_2()
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String

        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(7)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
        ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(21)  'OPI Inicial Notificado

        scomentarios = DetalleComentOPI1.ValComentarios
        blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(6), "Notifica Reunión con VJ no realizada", scomentarios)
        If blnResultado Then
            btnAceptarM1B1A.CommandArgument = "RedirectSelf"
            btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
        End If
    End Sub

    ''' <summary>
    ''' Reunion Relaizada en fecha diferente
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub GuardaDatos6_3()

        Mensaje = "Ingresa la fecha real en la que se llevó a cabo la reunión. Recuerda que la fecha a ingresar no puede ser mayor al día actual."
        calEx.EndDate = DateTime.Now
        btnAceptarM2B1A.CommandArgument = "SaveStep6_3_1"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "FechaReunion", " FechaReunion();", True)

    End Sub

    Protected Sub GuardaDatos6_3_1()
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String


        If (ValidaDatos(6, 3, True)) Then

            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(7)
            ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
            ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(20)  'OPI Inicial Notificado
            ListaCampos.Add("F_FECH_REUNION_REAL") : ListaValores.Add(Date.ParseExact(txtFechaReunion4.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))

            scomentarios = DetalleComentOPI1.ValComentarios
            blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
            BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(6), "Notifica estatus de  Reunión con VJ", scomentarios)
            If blnResultado Then
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
            End If

        Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            lblTitle.Text = "Alerta - Información Incompleta"
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If

    End Sub
    Protected Sub GuardaPaso6_4()
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String

        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(6)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
        ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(9)  'OPI Inicial Notificado

        scomentarios = DetalleComentOPI1.ValComentarios
        blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)

        If blnResultado Then
            btnAceptarM1B1A.CommandArgument = "RedirectSelf"
            btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
        End If
    End Sub

    Protected Sub GuardarDatos7(Optional _primero As Boolean = False)
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String

        Dim FechaEstimada As String = ObtenerFechaEstimada(_opiDetalle.I_ID_OPI, 7)

        If _primero Then
            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(7)
            ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(1)
            'FSW RLZ SOFTTEK Inicia Correccion de incidencia 52, Documento 38 
            'ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
            'FSW RLZ SOFTTEK Termina Correccion de incidencia 52, Documento 38 
            ListaCampos.Add("B_IRREG_STD") : ListaValores.Add(DetalleIrregularidad1.ValIrregStand)
            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(22)  'OPI Inicial Elaborado
            ListaCampos.Add("F_FECH_ESTIM_ENTREGA") : ListaValores.Add(Date.ParseExact(FechaEstimada.Substring(0, 10), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
        Else
            ListaCampos.Add("N_ID_PASO") : ListaValores.Add(8)
            ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
            ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
            ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(23)  'OPI Inicial Notificado
        End If

        scomentarios = DetalleComentOPI1.ValComentarios

        blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
        If _opiDetalle.I_ID_ESTATUS = 9 Then
            BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(7), "Requerimiento de información", scomentarios)
        Else
            BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(7), "Notifica Oficio de Observaciones", scomentarios)
        End If
        If blnResultado Then
            If _primero Then

                lblTitle.Text = "Confirmación"
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                Mensaje = "Se ha guardado correctamente la información." 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
            Else
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
            End If

        Else
            ''Notificar Error
        End If
    End Sub

    Protected Sub GuardarDatos9(iAccion As Integer)
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String

        Select Case iAccion
            Case 0
                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(9)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(1)
                ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                ListaCampos.Add("B_IRREG_STD") : ListaValores.Add(DetalleIrregularidad1.ValIrregStand)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(13)  'Finaliza análisis
            Case 1
                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(7)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(46)  'En espera de información
            Case 2, 3   ' Notificación de SI PROCEDE y NO PROCEDE
                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(10)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                If DetallePosibIncumplim.ValPosibIncumplim = "1" Then
                    ListaCampos.Add("B_PROCEDE") : ListaValores.Add(1)
                    ListaCampos.Add("T_DSC_MOTIV_NO_PROC") : ListaValores.Add("")
                Else
                    ListaCampos.Add("B_PROCEDE") : ListaValores.Add(0)
                    ListaCampos.Add("T_DSC_MOTIV_NO_PROC") : ListaValores.Add(DetallePosibIncumplim.ValMotivoNOProced)
                End If
                If iAccion = 2 Then        ' SI PROCEDE
                    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(26)  '(26) Notifica Procede OPI
                ElseIf iAccion = 3 Then    ' NO PROCEDE
                    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(27)  '(27) Notifica No Procede OPI
                End If
        End Select

        scomentarios = DetalleComentOPI1.ValComentarios

        blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(9), "Análisis de información del área de vigilancia", scomentarios)

        If blnResultado Then
            If iAccion = 0 Then
                lblTitle.Text = "Confirmación"
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                Mensaje = "Se ha guardado correctamente la información." 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
            Else
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
            End If
        Else
            ''Notificar Error
        End If
    End Sub

    Protected Sub GuardarDatos10(strAccion As String)
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String
        Dim FolioSisan As String

        Select Case strAccion
            Case "a"
                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(10)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(1)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(28)  '(28) Oficio Cierre Elaborado
            Case "aN"
                'FSW CAGC  SOFTTEK
                'Validacion de proceso, cuando estaba en el paso 10 y no procedia el OPI, pasaba al paso 11
                'La siguiente validacion se integro para que al no proceder el OPI en el paso 10 y ser notificado, ->
                ' quedara  solo de lectura.
                If _opiDetalle.B_PROCEDE = False Then
                    ListaCampos.Add("N_ID_PASO") : ListaValores.Add(10)
                    ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(29)  '(29) Oficio Cierre Notificado , queda en modo lectura.
                Else
                    ListaCampos.Add("N_ID_PASO") : ListaValores.Add(11)
                    ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(29)  '(29) Oficio Cierre Notificado
                End If

            Case "b1"
                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(10)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(1)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(30)  '(30) Dictamen Elaborado   
            Case "b1N"
                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(10)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(2)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(31)  '(31) Dictamen Notificado
            Case "b2"
                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(10)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(3)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(42)  '(42) Irregularidades Registradas
            Case "b3"
                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(10)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(3)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(43)  '(43) Enviar a SISAN
            Case "b4"
                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(10)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(3)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(44)  '(44) Enviado a SISAN
        End Select
        scomentarios = DetalleComentOPI1.ValComentarios

        blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
        Select Case _opiDetalle.I_ID_ESTATUS
            Case 27
                BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(10), "No procede Oficio de observaciones", scomentarios)
            Case 26
                BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(10), "Procede Oficio de observaciones", scomentarios)
            Case 31
                BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(10), "Identifica irregularidades", scomentarios)
            Case 42
                BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(10), "Elabora y registra dictamen en SICOD", scomentarios)
            Case 43
                BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(10), "Envia asunto a SISAN", scomentarios)
        End Select

        If blnResultado Then
            If strAccion = "a" Or strAccion = "b1" Or strAccion = "b2" Or strAccion = "b3" Then
                lblTitle.Text = "Guardado correcto"
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                Mensaje = "Se ha guardado correctamente la información." 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
            ElseIf strAccion = "aN" Or strAccion = "b4" Then

                If strAccion = "b4" Then
                    FolioSisan = ValidacionSISAN()
                    EnviaNotificacionOPI(128) 'AMMM 06082019 se agrega esta instrucción aquí para mandar correo de envío a SISAN despues de guardar el dato 
                    lblTitle.Text = "Guardado correcto"
                    btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                    imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                    Mensaje = "Se ha registrado correctamente en SISAN con el número de Folio " + FolioSisan + "."
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
                Else

                    btnAceptarM1B1A.CommandArgument = "Redirect"
                    btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
                End If
            Else
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
            End If
        Else
            ''Notificar Error
        End If
    End Sub
    'Sye/EC-Juan.Jose.Velazquez
    Protected Sub CancelarOPI()
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String = CancelarOPI1.ValMotivo1

        'Dim scomentarios As String = cancelarMotivoOPI.ValMotivo
        'cancelarMotivoOPI.ValMotivo
        If CancelarOPI1.ValMotivo1.ToString.Trim.Length() <= 0 Then
            lblTitle.Text = "Alerta - Información Incompleta"

            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            Mensaje = "Se requiere indicar el motivo de la cancelación"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
            Exit Sub
        End If

        'ListaCampos.Add("B_IRREG_STD") : ListaValores.Add(DetalleIrregularidad1.ValIrregStand)
        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(7)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(_opiDetalle.N_ID_SUBPASO)
        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(48)  'Cancelado
        blnResultado = _opiFunc.ActualizarEstatus(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(7), "Cancelación de OPI", scomentarios)

        If blnResultado Then
            lblTitle.Text = "Confirmación"
            btnAceptarM1B1A.CommandArgument = "RedirectSelf"
            imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
            Mensaje = "Se ha cancelado el OPI" 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
        Else
            lblTitle.Text = "Alerta - Problemas al grabar"
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            Mensaje = "Problemas al registrar la operación"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If
    End Sub
    Protected Sub DejarSEfecto()
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String = CancelarOPI1.ValMotivo1


        'ListaCampos.Add("B_IRREG_STD") : ListaValores.Add(DetalleIrregularidad1.ValIrregStand)
        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(10)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(_opiDetalle.N_ID_SUBPASO)
        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(50) 'En proceso de cancelación
        ListaCampos.Add("I_ID_ESTATUS_ANT") : ListaValores.Add(_opiDetalle.I_ID_ESTATUS)
        blnResultado = _opiFunc.ActualizarEstatus(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(10), "Solicitud para dejar sin efectos el oficio", scomentarios)

        If blnResultado Then
            EnviaNotificacionOPI(129)
            lblTitle.Text = "Confirmación"
            btnAceptarM1B1A.CommandArgument = "RedirectSelf"
            imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
            Mensaje = "Se procesó Solicitud para dejar sin efectos el oficio" 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
        Else
            ''Notificar Error
        End If
    End Sub
    Protected Sub AprobarSEfecto(sender As Object, e As CommandEventArgs)
        'Sye/EC-Juan.Jose.Velazquez
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False

        'ListaCampos.Add("B_IRREG_STD") : ListaValores.Add(DetalleIrregularidad1.ValIrregStand)
        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(10)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(_opiDetalle.N_ID_SUBPASO)
        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(49)  'Se deja sin efectos
        blnResultado = _opiFunc.ActualizarEstatus(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores)
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(10), "Se aprueba Solicitud para dejar sin efectos el oficio", "")

        If blnResultado Then
            EnviaNotificacionOPI(130)
            lblTitle.Text = "Confirmación"
            btnAceptarM1B1A.CommandArgument = "RedirectSelf"
            imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
            Mensaje = "Se aprobó solicitud para dejar sin efectos el oficio" 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
        Else
            ''Notificar Error
        End If
    End Sub
    Protected Sub RechazarSEfecto(sender As Object, e As CommandEventArgs)
        'Sye/EC-Juan.Jose.Velazquez
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False

        'ListaCampos.Add("B_IRREG_STD") : ListaValores.Add(DetalleIrregularidad1.ValIrregStand)
        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(10)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(_opiDetalle.N_ID_SUBPASO)
        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(_opiDetalle.I_ID_ESTATUS_ANT)
        blnResultado = _opiFunc.ActualizarEstatus(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores)
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(10), "Se rechaza solicitud para dejar sin efectos el oficio", "")

        If blnResultado Then
            EnviaNotificacionOPI(131)
            lblTitle.Text = "Confirmación"
            btnAceptarM1B1A.CommandArgument = "RedirectSelf"
            imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
            Mensaje = "Se rechazó solicitud para dejar sin efectos el oficio" 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
        Else
            ''Notificar Error
        End If
    End Sub

    Protected Sub btnAceptar7_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar7.Click
        If ValidaDatos(7, , True) Then
            btnAceptarM2B1A.CommandArgument = "SaveStep7.22"
            'btnAceptarM2B1A.CommandArgument = "Guardar1"
            lblTitle.Text = "Confirmación"
            Mensaje = "¿Estás seguro que deseas completar la información del Oficio de Observaciones?"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            lblTitle.Text = "Alerta - Información Incompleta"
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If
    End Sub



    Protected Sub btnAceptar9_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar9.Click
        If ValidaDatos(9, , True) Then
            btnAceptarM2B1A.CommandArgument = "SaveStep9"
            lblTitle.Text = "Confirmación"
            'btnAceptarM2B1A.CommandArgument = "Guardar1"
            Mensaje = "Se procederá a notificar que ha finalizado el análisis de la información y que se determinará si Procede o no el Oficio de Observaciones, ¿Deseas continuar?"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            lblTitle.Text = "Alerta - Información Incompleta"
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If
    End Sub

    Protected Sub btnAceptar10_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar10a.Click
        If ValidaDatos(10, 1, True) Then
            btnAceptarM2B1A.CommandArgument = "SaveStep10a"
            'btnAceptarM2B1A.CommandArgument = "Guardar1"
            lblTitle.Text = "Confirmación"
            Mensaje = "Se notificará  que se ha envíado a la AFORE la procedencia de irregularidades, ¿Deseas continuar?"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            lblTitle.Text = "Alerta - Información Incompleta"
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If
    End Sub

    Protected Sub btnAceptar10b_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar10b.Click
        If ValidaDatos(10, 1, True) Then
            btnAceptarM2B1A.CommandArgument = "SaveStep10b"
            lblTitle.Text = "Confirmación"
            'btnAceptarM2B1A.CommandArgument = "Guardar1"
            Mensaje = "¿Estás seguro que deseas guardar la información?"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            lblTitle.Text = "Alerta - Información Incompleta"
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If
    End Sub

    Protected Sub btnAceptar10b2_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar10b2.Click
        If ValidaDatos(10, 2, False) Then
            btnAceptarM2B1A.CommandArgument = "SaveStep10b2"
            lblTitle.Text = "Confirmación"
            'btnAceptarM2B1A.CommandArgument = "Guardar1"
            Mensaje = "¿Estás seguro que deseas completar la información del Oficio de Observaciones?"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If
    End Sub

    Protected Sub btnAceptar10b3_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar10b3.Click
        If ValidaDatos(10, 3, False) Then
            btnAceptarM2B1A.CommandArgument = "SaveStep10b3"
            lblTitle.Text = "Confirmación"
            'btnAceptarM2B1A.CommandArgument = "Guardar1"
            Mensaje = "¿Estás seguro que deseas completar la información del Oficio de Observaciones?"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If
    End Sub

    Protected Sub btnSiSAN_Click(sender As Object, e As ImageClickEventArgs) Handles btnSiSAN.Click
        If ValidaDatos(10, 4, False) Then
            btnAceptarM2B1A.CommandArgument = "SaveStep10b4"
            lblTitle.Text = "Confirmación"
            'btnAceptarM2B1A.CommandArgument = "Guardar1"
            Mensaje = "Se enviarán las irregularidades y el dictamen al área de sanciones, ¿Deseas continuar?"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If
    End Sub

    Protected Sub btnAceptar3_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar3.Click
        Select Case DetalleRespAforeOPI1.ValRespAfore
            Case "0"
                If Not ValidaDatos(3, , True) Then
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                    lblTitle.Text = "Alerta - Información Incompleta"
                    Mensaje = "<ul>"
                    For Each msg In lstMensajes
                        Mensaje += "<li>" + msg + "</li>"
                    Next
                    Mensaje += "</ul>"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
                End If
            Case "Respuesta de requerimiento"
                If ValidaDatos(3, , True) Then
                    btnAceptarM2B1A.CommandArgument = "SaveStep2.8"
                    lblTitle.Text = "Confirmación"
                    Mensaje = "¿Estás seguro que deseas guardar la información?"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
                Else

                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                    lblTitle.Text = "Alerta - Información Incompleta"
                    Mensaje = "<ul>"
                    For Each msg In lstMensajes
                        Mensaje += "<li>" + msg + "</li>"
                    Next
                    Mensaje += "</ul>"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)

                End If

            Case "Prórroga de entrega de información"

                btnAceptarM2B1A.CommandArgument = "ConfirmaProrroga3"
                lblTitle.Text = "Confirmación"
                Mensaje = "El tipo de respuesta de la AFORE fue una prórroga a la entrega de información, para continuar debes ingresar el total de días otorgados para la prórroga, ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionProrroga();", True)

            Case "No hay respuesta en tiempo establecido"
                btnAceptarM2B1A.CommandArgument = "SaveStep3.2"
                lblTitle.Text = "Confirmación"
                Mensaje = "No se recibió respuesta al requerimiento de información por parte de la AFORE, se deberá registrar un Nuevo Oficio de Observaciones por "" falta de entrega de información "" ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        End Select
    End Sub

    Protected Sub btnAceptar4_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar4.Click
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)

        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(4)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
        ListaCampos.Add("B_EXISTE_IRREG") : ListaValores.Add(DetalleIrregularidad1.ValExisteIrreg)
        ListaCampos.Add("T_DSC_JUST_NO_IRREG") : ListaValores.Add(DetalleIrregularidad1.ValJustificac)
        ListaCampos.Add("B_IRREG_STD") : ListaValores.Add(DetalleIrregularidad1.ValIrregStand)

        If DetalleComentOPI1.ValComentarios = "" Then
            _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores)
        Else
            _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, DetalleComentOPI1.ValComentarios)
        End If
        lblTitle.Text = "Guardado Correcto"
        Mensaje = "Se ha guardado correctamente la información."
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MostrarAceptar();", True)


    End Sub

    Protected Sub btnAceptar41_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar41.Click
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)

        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(4)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(1)
        ListaCampos.Add("B_EXISTE_IRREG") : ListaValores.Add(DetalleIrregularidad1.ValExisteIrreg)
        ListaCampos.Add("T_DSC_JUST_NO_IRREG") : ListaValores.Add(DetalleIrregularidad1.ValJustificac)
        ListaCampos.Add("B_IRREG_STD") : ListaValores.Add(DetalleIrregularidad1.ValIrregStand)

        If DetalleComentOPI1.ValComentarios = "" Then
            _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores)
        Else
            _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, DetalleComentOPI1.ValComentarios)
        End If
        lblTitle.Text = "Confirmación"
        Mensaje = "Se ha guardado correctamente la información."
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MostrarAceptar();", True)
    End Sub

    Protected Sub btnAceptar4_1_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar4_1.Click

        If (DetalleIrregularidad1.ValIsIrregularidad) Then


            If ValidaDatos(4, 3, False) Then
                btnAceptarM2B1A.CommandArgument = "SaveStep4.12"
                lblTitle.Text = "Confirmación"
                Mensaje = "Se procederá a notificar que ha finalizado el análisis de la información y que se ha determinado si hay o no  irregularidad  para este requerimiento de información,  ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
            Else
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                lblTitle.Text = "Alerta - Información Incompleta"
                Mensaje = "<ul>"
                For Each msg In lstMensajes
                    Mensaje += "<li>" + msg + "</li>"
                Next
                Mensaje += "</ul>"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
            End If

        Else

            If ValidaDatos(4, 2, False) Then
                btnAceptarM2B1A.CommandArgument = "SaveStep4.12"
                lblTitle.Text = "Confirmación"
                Mensaje = "Se procederá a notifcar que ha finalizado el análisis de la información y que se ha determinado si hay o no  irregularidad  para este requerimiento de información,  ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
            Else
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                lblTitle.Text = "Alerta - Información Incompleta"
                Mensaje = "<ul>"
                For Each msg In lstMensajes
                    Mensaje += "<li>" + msg + "</li>"
                Next
                Mensaje += "</ul>"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
            End If

        End If


    End Sub



    Protected Sub btnCorreo4_Click(sender As Object, e As ImageClickEventArgs) Handles btnCorreo4.Click

        If ValidaDatos(4, , True) Then
            If (DetalleIrregularidad1.ValIsIrregularidad) Then
                btnAceptarM2B1A.CommandArgument = "SaveStep4.13"
                lblTitle.Text = "Confirmación"
                Mensaje = "Se notificará que  existe irregularidad para este Requerimiento de Información  ,  ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
            Else
                btnAceptarM2B1A.CommandArgument = "SaveStep4.13_1"
                lblTitle.Text = "Confirmación"
                Mensaje = "Se notificará que no existe irregularidad para este Requerimiento de Información,  ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
            End If
        Else
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            lblTitle.Text = "Alerta - Información Incompleta"
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)

        End If
    End Sub


    Protected Sub ConfirmarReunion(_commandargument As String)
        btnAceptarM2B1A.CommandArgument = _commandargument
        btnAceptarM3B1A.CommandArgument = "RedirectSelf"
        lblTitle.Text = "Confirmación"
        '       calEx.StartDate = DateTime.Now.ToString("dd/MM/yyyy")
        Mensaje = "¿Se realizará reunión con la Vicepresidencia Juridica para revisar la irregularidad?"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ConfirmacionReunion", "ConfitmaReunion();", True)

    End Sub

    Protected Sub btnInfoSol4_Click(sender As Object, e As ImageClickEventArgs) Handles btnInfoSol4.Click
        If ValidaDatos(4, 4, False) Then
            btnAceptarM2B1A.CommandArgument = "SaveStep4.12_1"
            lblTitle.Text = "Confirmación"
            'FSW RLZ SOFTTEK Inicia Correccion de incidencia 91, Documento 38 
            Mensaje = "Se iniciará el proceso de solicitud de información para el envío de requerimientos de información adicional a la AFORE, ¿Deseas continuar?"
            'Mensaje = "Se avanzará y se enviará correo informando que se ha notificado a la entidad de requerimiento de información adicional, ¿Deseas continuar?"
            'FSW RLZ SOFTTEK Termina Correccion de incidencia 91, Documento 38 
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        Else
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            lblTitle.Text = "Alerta - Información Incompleta"
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)

        End If
    End Sub
    Protected Sub MostrarMensajeReunion()
        btnAceptarM2B1A.CommandArgument = "SaveStep4.13_2"
        btnAceptarM3B1A.CommandArgument = "SaveStep4_13"
        lblTitle.Text = "Confirmación"
        Mensaje = "¿Se realizará reunión con la Vicepresidencia Juridica para revisar la irregularidad?"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ConfirmacionReunion", "ConfitmaReunion();", True)

    End Sub

    Protected Sub btnAceptar5_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar5.Click
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)

        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(5)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
        ListaCampos.Add("T_DSC_RESP_AFORE") : ListaValores.Add(DetalleRespAforeOPI1.ValRespAfore)

        If DetalleComentOPI1.ValComentarios = "" Then
            _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores)
        Else
            _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, DetalleComentOPI1.ValComentarios)
        End If
        lblTitle.Text = "Guardado Correcto"
        Mensaje = "Se ha guardado correctamente la información."
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MostrarAceptar();", True)
    End Sub


    Protected Sub btnAceptar5x15_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar5x15.Click
        'If ValidaDatos(5, , True) Then
        btnAceptarM2B1A.CommandArgument = "SaveStep5.15"
        'btnAceptarM2B1A.CommandArgument = "Guardar1"
        lblTitle.Text = "Confirmación"
        Mensaje = "¿Estás seguro que deseas completar la información del Oficio?"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        'Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
        '    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
        '    Mensaje = "<ul>"
        '    For Each msg In lstMensajes
        '        Mensaje += "<li>" + msg + "</li>"
        '    Next
        '    Mensaje += "</ul>"
        '    lblTitle.Text = "Alerta - Información Incompleta"
        '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        'End If
    End Sub


    Protected Sub btnCorreo5_Click(sender As Object, e As ImageClickEventArgs) Handles btnCorreo5.Click
        'If ValidaDatos(5, , False) Then
        btnAceptarM2B1A.CommandArgument = "SaveStep5.16"
        'btnAceptarM2B1A.CommandArgument = "Guardar1"
        lblTitle.Text = "Confirmación"
        Mensaje = "Se informará que no existe irregularidad para este Requerimiento de Información,  ¿Deseas continuar?"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        'Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
        'imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
        'lblTitle.Text = "Alerta - Información Incompleta"
        'Mensaje = "<ul>"
        'For Each msg In lstMensajes
        '    Mensaje += "<li>" + msg + "</li>"
        'Next
        'Mensaje += "</ul>"

        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        'End If

    End Sub

    Protected Sub btnAceptar6_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar6.Click
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)

        'ListaCampos.Add("N_ID_PASO") : ListaValores.Add(6)
        'ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
        'ListaCampos.Add("T_DSC_RESP_AFORE") : ListaValores.Add(DetalleRespAforeOPI1.ValRespAfore)

        'If DetalleComentOPI1.ValComentarios = "" Then
        '    _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores)
        'Else
        '    _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, DetalleComentOPI1.ValComentarios)
        'End If
        lblTitle.Text = "Guardado Correcto"
        Mensaje = "Se ha guardado correctamente la información."
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MostrarAceptar();", True)
    End Sub
    Protected Sub btnCorreo6_Click(sender As Object, e As ImageClickEventArgs) Handles btnCorreo6.Click

        btnAceptarM2B1A.CommandArgument = "SaveStep6.16_1"
        btnAceptarM1B1A.CommandArgument = "SaveStep6.16_2"
        btnAceptarM3B1A.CommandArgument = "SaveStep6_16_3"
        Mensaje = "Se llevó acabo la reunión con la vicepresidencia Juridica para revisar la  irregularidad  en la fecha estimada:"
        lblP6Fecha.InnerText = _opiDetalle.F_FECH_REUNION.ToString().Substring(0, 10)
        lblFecha.InnerText = "Fecha real de la reunión"
        lblFecha.Attributes.Remove("Text")
        lblTitle.Text = "Confirmación"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ConfirmacionFechaReal", "P6ConfirmaReunion();", True)

    End Sub

    Protected Sub btnSolInfo6_Click(sender As Object, e As ImageClickEventArgs) Handles btnSolInfo6.Click
        Mensaje = "Se iniciará el proceso de solicitud de información para el envío de requerimientos de información adicional a la AFORE, ¿Deseas continuar?"
        lblTitle.Text = "Confirmación"
        btnAceptarM2B1A.CommandArgument = "SaveStep6_4"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
    End Sub

    Protected Function GuardarDatos8(Optional _step As Int16 = 1) As Boolean

        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String
        Dim _bvalidado As Boolean
        Dim EstadoBitacora As String = ""
        Dim _blnProrroga As Boolean
        Dim _diasprorroga As Double
        Dim _newFechaProrroga As Date
        Dim strNewFoliOOPI As String

        scomentarios = DetalleComentOPI1.ValComentarios
        Select Case _step
            Case 1

                Dim a = DetalleIrregularidad1.ValExisteIrreg

                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(8)
                ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                ListaCampos.Add("T_DSC_RESP_AFORE") : ListaValores.Add(DetalleRespAforeOPI1.ValRespAfore)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(25) 'respuesta AFORE a OPI inicial

                Dim resp = DetalleIrregularidad1.ValIrregStand

                ListaCampos.Add("B_IRREG_STD") : ListaValores.Add(resp)

                _bvalidado = True
                EstadoBitacora = "Guarda Respuesta de AFORE [RESPUESTA A REQ]"

            Case 2
                If ValidaDatos(8, 1, True) Then
                    ListaCampos.Add("N_ID_PASO") : ListaValores.Add(8)
                    ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                    ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                    ListaCampos.Add("T_DSC_RESP_AFORE") : ListaValores.Add(DetalleRespAforeOPI1.ValRespAfore)
                    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(9) 'en espera de información

                    'Dim _diasprorroga As Double
                    _diasprorroga = Double.Parse(txtDiasProrroga.Text)

                    'Dim _newFechaProrroga As Date

                    '_newFechaProrroga = DateAdd("d", _diasprorroga, _opiDetalle.F_FECH_ESTIM_ENTREGA)
                    _newFechaProrroga = BandejaPC.DiasHabiles(_opiDetalle.F_FECH_ESTIM_ENTREGA, _diasprorroga)

                    ListaCampos.Add("F_FECH_ESTIM_ENTREGA") : ListaValores.Add(_newFechaProrroga)

                    _bvalidado = True
                    _blnProrroga = True ' AMMM 06082019 faltaba este campo comparando con paso 3
                    EstadoBitacora = "Guarda Respuesta de AFORE [PRORROGA]"

                Else

                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                    Mensaje = "<ul>"
                    For Each msg In lstMensajes
                        Mensaje += "<li>" + msg + "</li>"
                    Next
                    Mensaje += "</ul>"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)

                    _bvalidado = False
                End If

            Case 3
                If ValidaDatos(8, 3, False) Then
                    ListaCampos.Add("N_ID_PASO") : ListaValores.Add(8)
                    ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                    ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                    ListaCampos.Add("T_DSC_RESP_AFORE") : ListaValores.Add(DetalleRespAforeOPI1.ValRespAfore)
                    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(9)

                    Dim _opicopia As New Registro_OPI
                    Dim strFoliosOPI As String = ""

                    _opicopia.T_ID_FOLIO = _opiDetalle.T_ID_FOLIO.Substring(_opiDetalle.T_ID_FOLIO.IndexOf("/", 0))
                    _opicopia.N_ID_PASO = 7
                    _opicopia.N_ID_SUBPASO = 0
                    _opicopia.I_ID_TIPO_ENTIDAD = _opiDetalle.I_ID_TIPO_ENTIDAD
                    _opicopia.I_ID_ENTIDAD = _opiDetalle.I_ID_ENTIDAD

                    ''PCMT --> NO habia definicion sobre las sub entidades
                    _opicopia.I_ID_SUBENTIDAD = _opiDetalle.I_ID_SUBENTIDAD
                    _opicopia.F_FECH_POSIBLE_INC = _opiDetalle.F_FECH_POSIBLE_INC
                    _opicopia.I_ID_PROCESO_POSIBLE_INC = _opiDetalle.I_ID_PROCESO_POSIBLE_INC
                    _opicopia.I_ID_SUBPROCESO = _opiDetalle.I_ID_SUBPROCESO
                    _opicopia.T_DSC_POSIBLE_INC = _opiDetalle.T_DSC_POSIBLE_INC + " " + _opiDetalle.T_ID_FOLIO + " No hay respuesta por falta de entrega de informacion en tiempo establecido" '
                    _opicopia.T_DSC_PROC_POSIB_INCUMP = _opiDetalle.T_DSC_PROC_POSIB_INCUMP
                    _opicopia.T_OBSERVACIONES_OPI = _opiDetalle.T_OBSERVACIONES_OPI
                    _opicopia.I_ID_ESTATUS = 4
                    _opicopia.I_ID_AREA = _opiDetalle.I_ID_AREA
                    _opicopia.F_FECH_PASO_ACTUAL = DateTime.Now
                    _opicopia.N_ID_PASO_ANT = 8

                    _opicopia.T_DSC_CLASIFICACION = "Oficio de Observaciones"
                    _opicopia.T_DSC_RESP_AFORE = DetalleRespAforeOPI1.ValRespAfore
                    _opicopia.F_FECH_ESTIM_ENTREGA = _opiDetalle.F_FECH_ESTIM_ENTREGA
                    _opicopia.F_FECH_ACUSE_DOCTO = _opiDetalle.F_FECH_ACUSE_DOCTO
                    _opicopia.T_ID_SUPERVISORES = _opiDetalle.T_ID_SUPERVISORES
                    _opicopia.T_ID_INSPECTORES = _opiDetalle.T_ID_INSPECTORES
                    _bvalidado = _opicopia.Agregar(strFoliosOPI)
                    EstadoBitacora = "Guarda Respuesta de AFORE [NO HAY RESPUESTA A REQ]"
                Else

                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                    Mensaje = "<ul>"
                    For Each msg In lstMensajes
                        Mensaje += "<li>" + msg + "</li>"
                    Next
                    Mensaje += "</ul>"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)

                    _bvalidado = False
                End If

            Case 4 ''NOTIFICA - PREGUNTA DE REUNION

                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(9)
                ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(12)
                _bvalidado = True
                EstadoBitacora = "Notifica Respuesta AFORE"

            Case 5
                If ValidaDatos(8, 2, True) Then
                    ListaCampos.Add("N_ID_PASO") : ListaValores.Add(9)
                    ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                    ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                    ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(12)
                    ListaCampos.Add("F_FECH_REUNION") : ListaValores.Add(Date.ParseExact(txtFechaReunion4.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
                    _bvalidado = True
                    EstadoBitacora = "Se registra fecha para reunión con VJ"

                Else
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                    Mensaje = "<ul>"
                    For Each msg In lstMensajes
                        Mensaje += "<li>" + msg + "</li>"
                    Next
                    Mensaje += "</ul>"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)

                    _bvalidado = False
                End If
            Case 6
                ListaCampos.Add("N_ID_PASO") : ListaValores.Add(10)
                ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
                ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
                ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(26)
                ListaCampos.Add("B_PROCEDE") : ListaValores.Add(1)
                _bvalidado = True

                EstadoBitacora = "Guarda Respuesta de AFORE [NO HAY RESPUESTA A REQ]"

        End Select

        If _bvalidado = True Then
            blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
            BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(8), EstadoBitacora, scomentarios)
        End If

        If _blnProrroga Then
            blnResultado = GuardarHistoricoProrroga(_opiDetalle.I_ID_OPI, 8, _diasprorroga, _newFechaProrroga.ToString)
        End If

        If blnResultado Then
            If _step = 3 Then
                btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                lblTitle.Text = "Guardado Correcto"
                imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                strNewFoliOOPI = _opiDetalle.T_ID_FOLIO.Substring(_opiDetalle.T_ID_FOLIO.IndexOf("/", 0))
                strNewFoliOOPI = _opiFunc.ObtenerNuevoFolioOPI(strNewFoliOOPI)
                Mensaje = "Se registró correctamente el Oficio de Observaciones " & strNewFoliOOPI & "." 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
            Else
                If _step <> 4 And Not (_step = 3) Then
                    lblTitle.Text = "Guardado Correcto"
                    btnAceptarM1B1A.CommandArgument = "RedirectSelf"
                    imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                    Mensaje = "Se ha guardado correctamente la información." 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
                Else


                End If
            End If
        Else
            ''Notificar Error
            Utilerias.ControlErrores.EscribirEvento("Error al guardar información OPI subpaso" + _step.ToString(), EventLogEntryType.Information)
        End If

        Return blnResultado

    End Function

    Protected Sub GuardarDatos81()
        btnAceptarM2B1A.CommandArgument = "SaveStep8.1"
        lblTitle.Text = "Confirmación"
        Mensaje = "Se actualizará la fecha de entrega de información, ¿Deseas continuar?"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
    End Sub

    Protected Sub btnAceptar8_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar8.Click

        Select Case DetalleRespAforeOPI1.ValRespAfore
            Case "Respuesta a Oficio de Observaciones"
                If ValidaDatos(8, , True) Then
                    btnAceptarM2B1A.CommandArgument = "SaveStep8"
                    lblTitle.Text = "Confirmación"
                    Mensaje = "¿Estás seguro que deseas completar la información del Oficio de Observaciones?"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
                Else

                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                    lblTitle.Text = "Alerta - Información Incompleta"
                    Mensaje = "<ul>"
                    For Each msg In lstMensajes
                        Mensaje += "<li>" + msg + "</li>"
                    Next
                    Mensaje += "</ul>"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)

                End If

            Case "Prórroga de entrega de información"
                ' If (ValidaDatos(8, 2, True)) Then
                If (ValidaDatos(8, , True)) Then

                    btnAceptarM2B1A.CommandArgument = "ConfirmaProrroga8"
                    lblTitle.Text = "Confirmación"
                    Mensaje = "El tipo de respuesta de la AFORE fue una prórroga a la entrega de información, para continuar debes ingresar el total de días otorgados para la prórroga, ¿Deseas continuar?"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionProrroga();", True)

                Else

                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                    lblTitle.Text = "Alerta - Información Incompleta"
                    Mensaje = "<ul>"
                    For Each msg In lstMensajes
                        Mensaje += "<li>" + msg + "</li>"
                    Next
                    Mensaje += "</ul>"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)

                End If

            Case "No hay respuesta en tiempo establecido"
                btnAceptarM2B1A.CommandArgument = "SaveStep8.2"
                btnAceptarM3B1A.CommandArgument = "SaveStep8.3"
                lblTitle.Text = "Confirmación"
                'Mensaje = "No se recibió respuesta al requerimiento de información" + vbCr + "¿Desea abrir un nuevo Oficio de Observaciones por la falta de respuesta de la AFORE?"
                Mensaje = "No se recibió respuesta por parte del AFORE, " + vbCr + "¿Desea abrir un nuevo Oficio de Observaciones por la falta de respuesta de la AFORE?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmaNoHayRespuesta();", True)

            Case Else

                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                lblTitle.Text = "Alerta - Información Incompleta"
                Mensaje = "Debes indicar el tipo de respuesta de la AFORE."
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)

        End Select
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function Notifica(Folio As Integer, Paso As Integer, Subpaso As Integer) As Boolean

    End Function

    Private Sub btnAceptarM2B1A_Click(sender As Object, e As EventArgs) Handles btnAceptarM2B1A.Click

        Select Case btnAceptarM2B1A.CommandArgument
            Case "SaveStep1.-1"
                GuardarDatos1(True)
            Case "GuardaNotifica1" '"SaveStep1.0"
                'If EnviarNotificacionPaso1 = True Then
                If EnviaNotificacionOPI(108) = True Then    ' Informa posible incumplimiento
                    GuardarDatos1()
                End If
            Case "SaveStep2.0"
                GuardarDatos2(1)
            Case "GuardaNotifica2.1" '"SaveStep2.1"
                If EnviaNotificacionOPI(109) Then           ' Se avisa que se ha clasificado el OPI
                    GuardarDatos2(2)
                End If
            Case "SaveStep2.3"
                GuardarDatos2(3)
            Case "GuardaNotifica2.4" '"SaveStep2.4"
                If EnviaNotificacionOPI(110) Then           ' Se mandan los oficios a la entidad notificandole el incuplimiento
                    GuardarDatos2(4)
                End If
            Case "SaveStep2.8"
                GuardarDatos3(1)
            Case "ConfirmaProrroga3"
                GuardarDatos31()
            Case "SaveStep3.1"
                If EnviaNotificacionOPI(112) Then          ' Se mandan correo notificando Prórroga
                    GuardarDatos3(2)
                End If
            Case "SaveStep3.2"
                If EnviaNotificacionOPI(111) Then        ' Se mandan correo notificando No hay respuesta
                    GuardarDatos3(3)
                End If
            Case "GuardaNotifica3"
                If EnviaNotificacionOPI(113) Then         ' Se mandan correo notificando Si hay respuesta
                    GuardarDatos3(4)
                End If
            Case "SaveStep4.12"
                If EnviaNotificacionOPI(114) Then        'correo de paso 4 finaliza análisis
                    GuardaDatos4_12()
                End If
            Case "SaveStep4.12_1"
                If EnviaNotificacionOPI(115) Then        'correo de paso 4 solicita info adicional
                    GuardaDatos4_12_1()
                End If
            Case "SaveStep4.13_1"
                If EnviaNotificacionOPI(117) = True Then
                    GuardaDatos4_13(False)
                End If

            Case "SaveStep4.13"

                If EnviaNotificacionOPI(116) = True Then
                    MostrarMensajeReunion()
                End If

            Case "SaveStep4.13_2"
                GuardaDatos4_13_2()
                'EnviaNotificacionOPI(118) notifica reunión paso 6 se pasa dentro de guardar 4_13_2


                'If EnviaNotificacionOPI(118) = True Then
                '    GuardaDatos4_13_2()
                'End If
            Case "GuardaDatos4_13_3"
                GuardaDatos4_13_3()



            Case "SaveStep5.15"
                GuardaDatos5(True)
            Case "SaveStep5.16"
                If EnviaNotificacionOPI(117) = True Then
                    GuardaDatos5(False)
                End If

            Case "SaveStep6.16_1"
                If EnviaNotificacionOPI(119) Then
                    GuardaDatos6_1()
                End If
            Case "SaveStep6_3_1"
                GuardaDatos6_3_1()
            Case "SaveStep6_4"
                ValidaDatos(6, 1, False)
                GuardaPaso6_4()
            Case "SaveStep7.22"  ' Guardado de Paso 7 
                GuardarDatos7(True)
            Case "GuardaNotifica7"
                If EnviaNotificacionOPI(120) = True Then
                    GuardarDatos7(False)
                End If
            Case "SaveStep8"
                GuardarDatos8(1)
            Case "ConfirmaProrroga8"
                GuardarDatos81()

            Case "SaveStep8.1"
                If EnviaNotificacionOPI(122) Then ' se cambia de 121 a 122
                    GuardarDatos8(2)
                End If
            Case "SaveStep8.2"
                If GuardarDatos8(3) Then
                    EnviaNotificacionOPI(121)
                End If
            Case "GuardaNotifica8"
                If EnviaNotificacionOPI(123) Then
                    If GuardarDatos8(4) Then
                        ConfirmarReunion("SolicitaFechaReunion")
                    End If
                End If
            Case "SolicitaFechaReunion"
                SolicitaFechaReunion("SaveStep8.3")
            Case "SaveStep8.3"
                If GuardarDatos8(5) Then
                    EnviaNotificacionOPI(118) 'SE CAMBIA DE 122 A 118
                End If
            Case "SaveStep9"  ' Guardado de Paso 9 
                If EnviaNotificacionOPI(124) Then
                    GuardarDatos9(0)
                End If
            Case "GuardaSolicita9"
                If EnviaNotificacionOPI(125) Then
                    GuardarDatos9(1)
                End If
            Case "GuardaNotifica9a"
                If EnviaNotificacionOPI(126) Then
                    GuardarDatos9(2)
                End If
            Case "GuardaNotifica9b"
                If EnviaNotificacionOPI(127) Then
                    GuardarDatos9(3)
                End If
            Case "SaveStep10a"
                GuardarDatos10("a")
            Case "GuardaNotifica10a"
                GuardarDatos10("aN")
            Case "SaveStep10b"
                GuardarDatos10("b1")
            Case "GuardaNotifica10b1"
                GuardarDatos10("b1N")
            Case "SaveStep10b2"
                GuardarDatos10("b2")
            Case "SaveStep10b3"
                GuardarDatos10("b3")
            Case "SaveStep10b4"
                GuardarDatos10("b4")

                'If EnviaNotificacionOPI(128) Then 'AMMM 06082019 se comenta debido a que primero debe guardar y despues notifica
                '    GuardarDatos10("b4")
                'End If
            Case "btnCancelar"
                'Response.Redirect("../OPI/BandejaOPI.aspx")
            Case "HomeOPI"
                Response.Redirect("../OPI/BandejaOPI.aspx")
            Case "Guardar1"
                'Call GuardarDatos1()
            Case "Guardar2"
                'Call GuardarDatos2()
            Case "CancelarOPI"
                'Sye/EC-Juan.Jose.Velazquez

                CancelarOPI()
            Case "DejarSEfecto"
                'Sye/EC-Juan.Jose.Velazquez
                DejarSEfecto()


        End Select

    End Sub

    Protected Sub btnAceptarM1B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM1B1A.Click

        Select Case btnAceptarM1B1A.CommandArgument
            Case "Redirect"
                Response.Redirect("../OPI/BandejaOPI.aspx")
            Case "RedirectSelf"
                Response.Redirect("../OPI/DetalleOPI.aspx")
            Case "SaveStep6.16_2"
                GuardaDatos6_2()
            Case Else

        End Select

    End Sub

    Protected Sub btnNotificar_Command(sender As Object, e As CommandEventArgs)

        Select Case e.CommandName
            Case "Notificar_Paso1"
                btnAceptarM2B1A.CommandArgument = "GuardaNotifica1"
                lblTitle.Text = "Confirmación"
                Mensaje = "Se procederá a realizar la clasificación del Oficio, ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
            Case "Notificar_Paso2"
                btnAceptarM2B1A.CommandArgument = "GuardaNotifica2.1"
                lblTitle.Text = "Confirmación"
                Mensaje = "Se ha realizado la clasificación del Oficio, se procederá a realizar las actividades de acuerdo a la clasificación asignada, ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
            Case "Notificar_Paso2.4"
                If ValidaDatos(1, -1, True) Then
                    btnAceptarM2B1A.CommandArgument = "GuardaNotifica2.4"
                    lblTitle.Text = "Confirmación"
                    'FSW RLZ SOFTTEK Inicia Correccion de incidencia 92, Documento 38  
                    Mensaje = "Se avanzará y se notificará a la entidad que la clasificación del Oficio derivó en " & _opiDetalle.T_DSC_CLASIFICACION & ", ¿Deseas continuar?"
                    'Mensaje = "Se avanzará y se enviará correo informando que se ha notificado a la entidad de requerimiento de información adicional, ¿Deseas continuar?"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

                Else
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                    lblTitle.Text = "Alerta - Información Incompleta"
                    Mensaje = "<ul>"
                    For Each msg In lstMensajes
                        Mensaje += "<li>" + msg + "</li>"
                    Next
                    Mensaje += "</ul>"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
                End If
                'FSW RLZ SOFTTEK Termina Correccion de incidencia 92, Documento 38  
            Case "Notificar_Paso2.4.2da"
                btnAceptarM2B1A.CommandArgument = "GuardaNotifica2.4"
                lblTitle.Text = "Confirmación"
                'FSW RLZ SOFTTEK Inicia Correccion de incidencia 92, Documento 38  
                'Mensaje = "Se avanzará y se notificará a la entidad que el OPI derivó en " & _opiDetalle.T_DSC_CLASIFICACION & ", ¿Deseas continuar?"
                Mensaje = "Se avanzará y se enviará correo informando que se ha notificado a la entidad de requerimiento de información adicional, ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
                'FSW RLZ SOFTTEK Termina Correccion de incidencia 92, Documento 38  
            Case "Notificar_Paso3"
                btnAceptarM2B1A.CommandArgument = "GuardaNotifica3"
                lblTitle.Text = "Confirmación"
                Mensaje = "Se ha recibidó la respuesta al requerimiento de información por parte de la AFORE, se procederá a realizar el análisis de la información, ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

            Case "Notificar_Paso7"
                btnAceptarM2B1A.CommandArgument = "GuardaNotifica7"
                lblTitle.Text = "Confirmación"
                Mensaje = "Se avanzará y se enviará el correo informando que se ha notificado a la entidad el Oficio de Observaciones, ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

            Case "Notificar_Paso7a"
                btnAceptarM2B1A.CommandArgument = "GuardaNotifica7"
                lblTitle.Text = "Confirmación"
                Mensaje = "Se avanzará y se enviará el correo informando que se ha notificado a la entidad de requerimiento de información adicional, ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
            Case "Notificar_Paso8"
                btnAceptarM2B1A.CommandArgument = "GuardaNotifica8"
                lblTitle.Text = "Confirmación"
                Mensaje = "Se ha recibidó la respuesta  por parte de la AFORE, se procederá a realizar el análisis de la información, ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)

            Case "Notificar_Paso9"
                If ValidaDatos(9, 2, True) Then
                    If DetallePosibIncumplim.ValPosibIncumplim = "1" Then
                        btnAceptarM2B1A.CommandArgument = "GuardaNotifica9a"
                        lblTitle.Text = "Confirmación"
                        Mensaje = "Se notificará que SI PROCEDE el oficio de Observaciones, ¿Deseas continuar?"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
                    ElseIf DetallePosibIncumplim.ValPosibIncumplim = "0" Then
                        btnAceptarM2B1A.CommandArgument = "GuardaNotifica9b"
                        lblTitle.Text = "Confirmación"
                        Mensaje = "Se notificará que NO PROCEDE el oficio de Observaciones, ¿Deseas continuar?"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
                    End If
                Else
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                    lblTitle.Text = "Alerta - Información Incompleta"
                    Mensaje = "<ul>"
                    For Each msg In lstMensajes
                        Mensaje += "<li>" + msg + "</li>"
                    Next
                    Mensaje += "</ul>"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
                End If
            Case "Notificar_Paso10a"
                btnAceptarM2B1A.CommandArgument = "GuardaNotifica10a"
                lblTitle.Text = "Confirmación"
                Mensaje = "Se avisará y se mandará correo informando que la Resolución es No Procede, ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
            Case "Notificar_Paso10b"
                btnAceptarM2B1A.CommandArgument = "GuardaNotifica10b1"
                lblTitle.Text = "Confirmación"
                Mensaje = "Se avanzará y se enviará correo informando que se ha notificado a la entidad la procedencia de irregularidades en caso de desvirtuar parcialmente alguna irregularidad. Se procederá a identificar irregularidades y enviará a VJ el dictamen, ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        End Select
        Select Case e.CommandArgument
            Case ""
        End Select
    End Sub

    Protected Sub btnConfirmaCancelacionOPI(sender As Object, e As CommandEventArgs)
        'Sye-Juan.Jose.Velazquez
        If ValidaDatos(11) Then  'valida que no existan documentos  
            btnAceptarM2B1A.CommandArgument = "CancelarOPI"
            lblTitle.Text = "Cancelación de oficio de observaciones"
            Mensaje = "Ingresa el motivo de la cancelación del oficio de observaciones"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfrimaCancelaOPI();", True)
        Else 'If Not IsNothing(lstMensajes) And lstMensajes.Count > 0 Then
            lblTitle.Text = "Alerta - Existen documentos en SICOD"
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
        End If
    End Sub
    Protected Sub btnDejarSinEfecto(sender As Object, e As CommandEventArgs)
        'Sye-Juan.Jose.Velazquez
        btnAceptarM2B1A.CommandArgument = "DejarSEfecto"
        lblTitle.Text = "Solicitud para dejar sin efectos el oficio"
        Mensaje = "Se enviará solicitud para dejar sin efectos el oficio de observaciones al administrador del sistema, por favor ingresa el motivo por el cual se dejará sin efectos el oficio de observaciones"
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeDejarSinEfecto();", True)
    End Sub
    Private Function EnviaNotificacionOPI(nIDNotificacion As Integer) As Boolean
        Dim correo As New NotificacionesOPI
        correo.Folio = _opiDetalle.I_ID_OPI
        correo.Usuario = puObjUsuario.IdentificadorUsuario
        correo.NotificarCorreoOPI(nIDNotificacion, DetalleComentOPI1.ValComentarios)

        Return True

    End Function

    Function DocumentoAdjunto(Optional _subpaso As Integer = 0) As Boolean
        Dim _objdocto As New ExpedienteOPI

        Return _objdocto.ExpedienteValido(_subpaso)


    End Function

    Protected Sub btnCancelarM2B1A_Click(sender As Object, e As EventArgs) Handles btnCancelarM2B1A.Click
        Response.Redirect("../OPI/DetalleOPI.aspx")
        ''Notificar Error
    End Sub
    Protected Sub btnAceptarM3B1A_Click(sender As Object, e As EventArgs) Handles btnAceptarM3B1A.Click

        Select Case btnAceptarM3B1A.CommandArgument
            Case "SaveStep4_13"
                GuardaDatos4_13()
            Case "SaveStep6_16_3"
                GuardaDatos6_3()
            Case "SaveStep8.3"
                GuardarDatos8(6)
            Case "RedirectSelf"
                Response.Redirect("../OPI/DetalleOPI.aspx")
        End Select
        ''Notificar Error
    End Sub

    Protected Sub GuardaDatos4_12()
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String

        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(4)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
        ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(13)  'OPI Inicial Notificado
        ListaCampos.Add("B_EXISTE_IRREG") : ListaValores.Add(DetalleIrregularidad1.ValIsIrregularidad)
        ListaCampos.Add("T_DSC_JUST_NO_IRREG") : ListaValores.Add(DetalleIrregularidad1.ValJustificac)
        ListaCampos.Add("B_IRREG_STD") : ListaValores.Add(DetalleIrregularidad1.ValIrregStand)

        scomentarios = DetalleComentOPI1.ValComentarios
        blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(4), "Finaliza análisis de información", scomentarios)
        If blnResultado Then
            btnAceptarM1B1A.CommandArgument = "RedirectSelf"
            btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
        End If


    End Sub

    Protected Sub GuardaDatos4_12_1()
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String

        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(2)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
        ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(9)  'OPI Inicial Notificado


        scomentarios = DetalleComentOPI1.ValComentarios
        blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(4), "Solicita información adicional a AFORE", scomentarios)
        If blnResultado Then
            btnAceptarM1B1A.CommandArgument = "RedirectSelf"
            btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
        End If


    End Sub


    Protected Sub GuardaDatos4_13()
        Dim _opiFunc As New Registro_OPI
        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim blnResultado As Boolean = False
        Dim scomentarios As String

        ListaCampos.Add("N_ID_PASO") : ListaValores.Add(7)
        ListaCampos.Add("N_ID_SUBPASO") : ListaValores.Add(0)
        ListaCampos.Add("N_ID_PASO_ANT") : ListaValores.Add(_opiDetalle.N_ID_PASO)
        ListaCampos.Add("I_ID_ESTATUS") : ListaValores.Add(21)  'OPI Inicial Notificado

        scomentarios = DetalleComentOPI1.ValComentarios
        blnResultado = _opiFunc.Guardar(_opiDetalle.I_ID_OPI, ListaCampos, ListaValores, scomentarios)
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(4), "Notifica si existe irregularidad", scomentarios)
        If blnResultado Then
            btnAceptarM1B1A.CommandArgument = "RedirectSelf"
            btnAceptarM1B1A_Click(btnAceptarM1B1A, New EventArgs())
        End If
    End Sub


    Protected Sub btnSolicitar_Command(sender As Object, e As CommandEventArgs)
        Select Case e.CommandName
            Case "Solicitar_Paso9"
                btnAceptarM2B1A.CommandArgument = "GuardaSolicita9" '"SaveStep1.0"
                lblTitle.Text = "Confirmación"
                'Mensaje = "Se iniciará el proceso de solicitud de información para el envío de requerimientos de información adicional a la AFORE, ¿Deseas continuar?"
                'Mensaje = "Se avanzará y se enviará correo informando que se ha notificado a la entidad de requerimiento de información adicional, ¿Deseas continuar?"
                Mensaje = "Se iniciará el proceso de solicitud de información para el envío de requerimientos de información adicional, ¿Deseas continuar?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
        End Select
        Select Case e.CommandArgument
            Case ""
        End Select
    End Sub

    Protected Sub btnSolicitarP9_Click(sender As Object, e As ImageClickEventArgs) Handles btnSolicitarP9.Click

    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function wsModificandoIrreg2(IDIrreg As Integer) As Boolean  '
        Dim lstCampos As New List(Of String)
        Dim lstValores As New List(Of Object)
        Dim dFechaNueva As Date
        Dim dFechaAnterior As Date
        Dim dtDatos As New DataTable
        Dim idRequerimiento As Integer = IDIrreg

        dtDatos = Entities.RequerimientoPC.TraerDatos(IDIrreg)
        dFechaAnterior = dtDatos.Rows(0)("F_FECH_ACUSE").ToString

        'dFechaNueva = dFechaAnterior.AddDays(iDias)
        With lstCampos
            .Add("i_ID_ESTATUS")
            .Add("F_FECH_ESTIMADA")
        End With

        With lstValores
            .Add(2)
            .Add(Date.ParseExact(dFechaNueva, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
        End With

        Return True
    End Function

    'Elimina irregularidad
    <System.Web.Services.WebMethod()>
    Public Shared Function wsEliminandoIrreg(IDIrreg As Integer) As Boolean  '
        Dim conexion As New Conexion.SQLServer()
        Return conexion.Ejecutar("DELETE FROM [dbo].[BDS_R_OPI_IRREGULARIDAD] WHERE I_ID_IRREGULARIDAD = " + IDIrreg.ToString())
    End Function

    'Modifica irregularidad
    <System.Web.Services.WebMethod()>
    Public Shared Function wsModificandoIrreg(IDIrreg As Integer) As Boolean  '
        Dim conexion As New Conexion.SQLServer()

        Return conexion.Ejecutar("DELETE FROM [dbo].[BDS_R_OPI_IRREGULARIDAD] WHERE I_ID_IRREGULARIDAD = " + IDIrreg.ToString())
    End Function

    'Cargando Irregularidad
    <System.Web.Services.WebMethod()>
    Public Shared Function wsCargandoIrreg(IDIrreg As Integer) As String  '
        Dim dt As New DataTable
        Dim query As String = ""
        Dim conexion As New Conexion.SQLServer()
        Dim cIrreg As New Entities.IrregularidadOPI
        Dim strCadena As String = ""

        query = "Select * FROM [dbo].[BDS_R_OPI_IRREGULARIDAD] WHERE I_ID_IRREGULARIDAD = " + IDIrreg.ToString()
        dt = conexion.ConsultarDT(query)

        With dt.Rows(0)
            strCadena = .Item("F_FECH_IRREGULARIDAD").ToString.Substring(0, 10) & "|"
            strCadena = strCadena & .Item("I_ID_PROCESO").ToString & "|"
            strCadena = strCadena & .Item("I_ID_SUBPROCESO").ToString & "|"
            strCadena = strCadena & .Item("I_ID_CONDUCTA").ToString & "|"
            strCadena = strCadena & .Item("I_ID_IRREGULARIDADES").ToString & "|"
            strCadena = strCadena & .Item("T_DSC_COMENTARIO").ToString & "|"
            strCadena = strCadena & .Item("I_ID_IRREGULARIDAD").ToString & "|"
        End With

        Return strCadena
    End Function

    Private Function GuardarHistoricoProrroga(nIDOPI As Integer, nIDPaso As Integer, nDias As Integer, strFecFinProrroga As String) As Boolean
        Dim _opiFunc As New Registro_OPI
        Dim lstCamposPro As New List(Of String)
        Dim lstValoresPro As New List(Of Object)
        Dim blnCorrecto As Boolean

        lstCamposPro.Add("I_ID_OPI") : lstValoresPro.Add(nIDOPI)
        lstCamposPro.Add("I_ID_PASO") : lstValoresPro.Add(nIDPaso)
        lstCamposPro.Add("F_FECH_REGISTRO") : lstValoresPro.Add("GETDATE()")
        lstCamposPro.Add("I_NUM_DURACION") : lstValoresPro.Add(nDias)
        lstCamposPro.Add("T_DSC_MOTIVO") : lstValoresPro.Add("")
        lstCamposPro.Add("F_FECH_FIN_PRORROGA") : lstValoresPro.Add(Date.ParseExact(strFecFinProrroga.Substring(0, 10), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"))
        lstCamposPro.Add("T_DSC_USUARIO") : lstValoresPro.Add(puObjUsuario.Nombre)

        blnCorrecto = _opiFunc.GuardarHistoricoProrrogas("BDS_D_OPI_PRORROGAS", lstCamposPro, lstValoresPro)

        Return blnCorrecto
    End Function


    Protected Sub imgProcesoVisita_Click(sender As Object, e As ImageClickEventArgs) Handles imgProcesoVisita.Click
        Dim val As String = puObjUsuario.DescArea
        If (puObjUsuario.DescArea = "VICEPRESIDENCIA DE OPERACIONES") Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ImagenOperaciones", "ImagenMostrar(1);", True)
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ImagenFinanciera", "ImagenMostrar(0);", True)
        End If
    End Sub

    Public Function ValidacionSISAN() As String
        Dim USUARIOS = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioSisan")
        Dim PASSWORDS = Utilerias.Cifrado.DescifrarAES(System.Web.Configuration.WebConfigurationManager.AppSettings("PassEncSisan").ToString())

        Dim Registro As New wsSisanRegV2.RegistroExternoV2
        Dim credentialsS As System.Net.NetworkCredential = New System.Net.NetworkCredential(USUARIOS, PASSWORDS, "ADCONSAR")
        Registro.Credentials = credentialsS
        Registro.ConnectionGroupName = "SEPRIS"

        'Dim subentidad As Integer = -1 'En caso de no enviar
        'Dim tipoEntidad As Integer = 1 'En caso de no enviar
        'Dim entidad As Integer = _opiDetalle.I_ID_ENTIDAD

        Dim subentidad As Integer = _opiDetalle.I_ID_SUBENTIDAD_SB 'se asigna valor de la bd
        Dim tipoEntidad As Integer = _opiDetalle.I_ID_TIPO_ENTIDAD 'se asigna valor de la bd
        Dim entidad As Integer = _opiDetalle.I_ID_ENTIDAD
        Dim sistema As String = "SEPRIS" 'sistema 2 sepris
        Dim clasificacion As Integer = 2 'OPI
        Dim participante As Integer = 0 ' Viene la irregularidad
        Dim oficioSICOD As String = "D00/VF/DVF/326/030/2019"
        Dim usuario As String = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario

        'Oficio del dictamen
        Dim dtDictamen As DataTable = Entities.DocumentoOPI.ObtenerArchivos(_opiDetalle.I_ID_OPI, 13)
        If dtDictamen.Rows.Count > 0 Then
            oficioSICOD = dtDictamen.Rows(0)("T_FOLIO_SICOD")
        End If


        Dim folioSISAN As String = ""

        Dim ListaCampos As New List(Of String)
        Dim ListaValores As New List(Of Object)
        Dim ListaCamposCondicion As New List(Of String)
        Dim ListaValoresCondicion As New List(Of Object)
        Dim mensajes As String() = {}
        Dim resultado As Boolean = True
        Dim MensajeResult As String = ""

        Dim resultadoRegistro As wsSisanRegV2.WsResultado = Registro.RegistroExterno(_opiDetalle.T_ID_FOLIO, usuario, sistema, tipoEntidad, entidad, subentidad, 1, clasificacion, "ninguno", Date.Now, Date.Now.AddDays(1), oficioSICOD, _opiDetalle.T_ID_FOLIO, usuario, "ninguno")

        If Not resultadoRegistro.isError Then

            MensajeResult = resultadoRegistro.Folio
            folioSISAN = resultadoRegistro.Folio

            Dim irregularidades As DataTable = Entities.IrregularidadOPI.ObtenerTodas(_opiDetalle.I_ID_OPI)
            If irregularidades.Rows.Count > 0 Then
                For Each irregularidad As DataRow In irregularidades.Rows
                    Dim resultadoIrregularidad As wsSisanRegV2.WsResultado = Registro.RegistroIncidencias(irregularidad("I_ID_IRREGULARIDADES"), resultadoRegistro.Irregularidad, irregularidad("F_FECH_IRREGULARIDAD"), irregularidad("T_DSC_COMENTARIO"))
                    If resultadoIrregularidad.isError Then
                        mensajes = resultadoIrregularidad.lstMensajes
                        resultado = False
                        Exit For
                    End If
                Next
                'se comenta esta linea para atender registro de estatus incorrecto en SISAN
                'Registro.SolicitarAutorizacion(resultadoRegistro.Irregularidad, usuario)

            End If
        Else
            mensajes = resultadoRegistro.lstMensajes
            resultado = False
        End If

        Dim Folio As Integer = _opiDetalle.I_ID_OPI
        Dim _opiFunc As New Registro_OPI


        If resultado Then
            ListaCampos.Add("T_ID_FOLIO_SISAN") : ListaValores.Add(folioSISAN)
            ListaCamposCondicion.Add("I_ID_OPI") : ListaValoresCondicion.Add(Folio)
            _opiFunc.ActualizarSISAN(ListaCampos, ListaValores, ListaCamposCondicion, ListaValoresCondicion)

        Else
            MensajeResult = "Error"
            For Each msg As String In mensajes
                MensajeResult += "<br\>" + msg
            Next

        End If

        Return MensajeResult

    End Function

    Public Shadows Function ObtenerFechaEstimada(I_ID_OPI As Integer, I_ID_DOCUMENTO As Integer) As String

        Dim FolioOficio As DataTable = Entities.DocumentoOPI.ObtenerArchivos(I_ID_OPI, I_ID_DOCUMENTO)

        Dim fecha As String
        If FolioOficio.Rows.Count > 0 Then
            fecha = ConexionSICOD.ObtenerFechaAcuse(FolioOficio.Rows(0).Item("T_FOLIO_SICOD").ToString())
        End If
        If (fecha = "") Then
            Return ""
        Else
            Return BandejaPC.DiasHabiles(fecha, Registro_OPI.ObtenerDiasVencimiento())
        End If

    End Function
    Public Sub HabilitarEdicion()
        pnlDetalleOPI.Enabled = True

        btnNotificar.Visible = False
        btnAceptar2.Visible = False
        btnNotificarP2.Visible = False
        btnNotificarP21.Visible = False
        btnAceptar21.Visible = False
        btnAceptar3.Visible = False

        btnEditar1.Visible = False
        btnEditar2.Visible = False
        btnEditar21.Visible = False
        btnEditar23.Visible = False
        btnActuOpi1.Visible = True
        btnActuOpi2.Visible = True
        btnActuOpi21.Visible = True
        btnActuOpi23.Visible = True
        DetalleOPIuc.Habilita()

        DetalleClasifOPI1.ComboVisible = True
        DetalleClasifOPI1.Inicializar()

        If Not _opiDetalle.T_DSC_CLASIFICACION Is Nothing AndAlso Not String.IsNullOrEmpty(_opiDetalle.T_DSC_CLASIFICACION) Then
            DetalleClasifOPI1.ActClasificacion(_opiDetalle.T_DSC_CLASIFICACION)
        End If

    End Sub
    Public Sub DesHabilitarEdicion()
        pnlDetalleOPI.Enabled = False

        btnEditar1.Visible = True
        btnEditar2.Visible = True
        btnEditar21.Visible = True
        btnEditar23.Visible = True
        btnActuOpi1.Visible = False
        btnActuOpi2.Visible = False
        btnActuOpi21.Visible = False
        DetalleOPIuc.Habilita()
    End Sub
    Public Sub ActualizaOPI()

        Dim _opiFunc As New Registro_OPI
        Dim blnResultado As Boolean = False
        Dim scomentarios As String

        If Not ValidaDatos(12) Then
            lblTitle.Text = "Alerta - Información Incompleta"
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
            Mensaje = "<ul>"
            For Each msg In lstMensajes
                Mensaje += "<li>" + msg + "</li>"
            Next
            Mensaje += "</ul>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Error", "MensajeValidacion();", True)
            Exit Sub
        End If

        _opiFunc.I_ID_OPI = _opiDetalle.I_ID_OPI
        _opiFunc.F_FECH_POSIBLE_INC = Convert.ToDateTime(DetalleOPIuc.FechaPI.Trim)
        _opiFunc.I_ID_PROCESO_POSIBLE_INC = Convert.ToInt64(DetalleOPIuc.ProcesoPO)
        _opiFunc.I_ID_SUBPROCESO = Convert.ToInt64(DetalleOPIuc.SubprocesoPO)
        _opiFunc.T_DSC_POSIBLE_INC = DetalleOPIuc.DescripcionOPI
        _opiFunc.T_ID_SUPERVISORES = DetalleOPIuc.GetSupervisoresSel()
        _opiFunc.T_ID_INSPECTORES = DetalleOPIuc.GetInspectoresSel()
        If DetalleClasifOPI1.ValClasificacion <> "0" Then
            _opiFunc.T_DSC_CLASIFICACION = DetalleClasifOPI1.ValClasificacion
        End If

        scomentarios = DetalleComentOPI1.ValComentarios

        blnResultado = _opiFunc.ActualizarOPI()
        BitacoraOPI.AgregarEntrada(_opiDetalle.I_ID_OPI, puObjUsuario.IdentificadorUsuario, BitacoraOPI.ObtenerDCSPaso(1), "Actualizacion registro de Oficio", scomentarios)

        If blnResultado Then
            btnAceptarM1B1A.CommandArgument = "RedirectSelf"
            lblTitle.Text = "Guardado Correcto"
            imgUnBotonUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
            Mensaje = "Se ha guardado correctamente la información." 'Mensaje = "Se ha actualizado correctamente el OPI " & _opiDetalle.T_ID_FOLIO

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeFinalizar();", True)
        Else
            ''Notificar Error
        End If
    End Sub

End Class