Imports System.Web.Configuration
Imports Entities
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Net
Imports System.Web.Services

Imports Utilerias

Public Class DetalleVisita
   Inherits System.Web.UI.Page
   Public Property Mensaje As String
   Dim enc As New YourCompany.Utils.Encryption.Encryption64

#Region "Props"
   ''' <summary>
   ''' Guarda la visita que se esta consultando
   ''' </summary>
   ''' <value></value>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public ReadOnly Property ppObjVisita As Visita
      Get
         If IsNothing(Session("DETALLE_VISITA")) Then
            Return Nothing
         Else
            Dim objVisita As Visita = CType(Session("DETALLE_VISITA"), Visita)
            If Not IsNothing(objVisita) Then
               Return objVisita
            Else
               Return Nothing
            End If
         End If
      End Get
   End Property

   Public ReadOnly Property AuxIdVisitaGenerado As String
      Get
         If IsNothing(ppObjVisita) Then
            Return ""
         Else
            Return ppObjVisita.IdVisitaGenerado.ToString()
         End If
      End Get
   End Property
   Public ReadOnly Property AuxUsuarioEstaOcupando As String
      Get
         If IsNothing(ppObjVisita) Then
            Return ""
         Else
            Return ppObjVisita.UsuarioEstaOcupando.ToString()
         End If
      End Get
   End Property
   Public ReadOnly Property AuxIdentificadorUsuario As String
      Get
         If IsNothing(puObjUsuario) Then
            Return ""
         Else
            Return puObjUsuario.IdentificadorUsuario.ToString()
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
   ''' Guarda un 1 si se esta editando la fecha de presentacion de hallazgos interna o externa
   ''' </summary>
   ''' <value></value>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Property piBanderaModificacio As Integer
      Get
         If IsNothing(ViewState("piBanderaModificacio")) Then
            Return 0
         Else
            Return CInt(ViewState("piBanderaModificacio"))
         End If
      End Get
      Set(value As Integer)
         Dim liAux As Integer = 0
         If Int32.TryParse(value, liAux) Then
            ViewState("piBanderaModificacio") = liAux
         Else
            ViewState("piBanderaModificacio") = 0
         End If
      End Set
   End Property

   Public Property piBanderaBoton As Integer
      Get
         If IsNothing(ViewState("piBanderaBoton")) Then
            Return 0
         Else
            Return CInt(ViewState("piBanderaBoton"))
         End If
      End Get
      Set(value As Integer)
         Dim liAux As Integer = 0
         If Int32.TryParse(value, liAux) Then
            ViewState("piBanderaBoton") = liAux
         Else
            ViewState("piBanderaBoton") = 0
         End If
      End Set
   End Property

   Private Property pbBanRedireccion As Boolean
#End Region

   Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
      If Not IsNothing(puObjUsuario) Then
         If Not IsPostBack() Then
            Session.Remove("DETALLE_VISITA")
         End If


         cargarDatosVisitaPrincipal(Convert.ToInt32(Session("ID_VISITA")))
      End If

      Dim lsUsu = User.Identity.Name

   End Sub


   Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

      If Not Page.IsPostBack Then
         If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
            If IsNothing(Session("ID_VISITA")) Then
               Response.Redirect("../Visita/Bandeja.aspx")
            End If

            ''Validar si hay documentos que ya se paso el ultimo paso para ser adjuntados
            AccesoBD.ActualizaDocumentosNoAdjuntados(Convert.ToInt32(Session("ID_VISITA")))

            ''Cargar imagen proceso ins sancion
            imgProcesoInspSancion.ImageUrl = "~/Imagenes/ProcesoInspSan/" & Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.NombreImagenProcesoInspeccionSanc)

            imgProcesoVisita.Visible = True
            imgProcesoVisita.Enabled = True

            ReemplazarImgBotonSiguienteRechazar()
         End If
      End If
   End Sub


   Public Sub cargarDatosVisitaPrincipal(ByVal idVisita As Integer)
      Dim visita As New Visita()
      Dim lbPermisos As Boolean = True

      If IsNothing(ppObjVisita) Then
         visita = AccesoBD.getDetalleVisita(idVisita, puObjUsuario.IdArea)
      Else
         visita = ppObjVisita
      End If

      If Not IsNothing(visita) Then
         ''Obtienelos inspectores y sus correos

         If Not IsPostBack Then
            visita.LstSupervisoresAsignados = AccesoBD.getSupervisoresAsignados(idVisita)
            visita.LstInspectoresAsignados = AccesoBD.getInspectoresAsignados(idVisita)

            Dim lstAbogados As List(Of Abogado) = AccesoBD.getAbogadosAsignados(idVisita)
            visita.LstAbogadosSupAsesorAsig = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.PERFIL_SUP And objAbo.SubPerfil = Constantes.ABOGADOS.PERFIL_ABO_ASESOR Select objAbo).ToList()
            ''AccesoBD.getAbogadosAsignados(idVisita, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_ASESOR)
            visita.LstAbogadosAsesorAsignados = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.ABOGADOS.PERFIL_ABO_ASESOR Select objAbo).ToList()
            ''AccesoBD.getAbogadosAsignados(idVisita, Constantes.ABOGADOS.PERFIL_ABO_ASESOR)

            visita.LstAbogadosSupSancionAsig = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.PERFIL_SUP And objAbo.SubPerfil = Constantes.ABOGADOS.PERFIL_ABO_SANCIONES Select objAbo).ToList()
            ''AccesoBD.getAbogadosAsignados(idVisita, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES)
            visita.LstAbogadosSancionAsignados = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.ABOGADOS.PERFIL_ABO_SANCIONES Select objAbo).ToList()
            ''AccesoBD.getAbogadosAsignados(idVisita, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES)

            visita.LstAbogadosSupContenAsig = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.PERFIL_SUP And objAbo.SubPerfil = Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO Select objAbo).ToList()
            ''AccesoBD.getAbogadosAsignados(idVisita, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO)
            visita.LstAbogadosContenAsignados = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO Select objAbo).ToList()
            ''AccesoBD.getAbogadosAsignados(idVisita, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO)

            'RRA Vulnerabilidad
            txtFechaVulnerabilidad.Text = Fechas.Valor(visita.Fecha_AcuerdoVul)

            ''Guarda la visita en la sesion
            Session("DETALLE_VISITA") = visita
            'lblFolioVisita.Text = visita.FolioVisita
            hfFolioVisita.Value = visita.FolioVisita

            ''SI ES UNA SUBVISITA O UNA COPIA DE FOLIOS PRECARGAR LOS COMENTARIOS DE ACUERDO AL PASO Y AL ESTATUS
            If visita.IdVisitaPadreCopia > 0 Or visita.IdVisitaPadreSubvisita > 0 Then
               txbComentarios.Text = visita.ComentariosPasoEstatus
            End If

            ''Nuevo campos, folio y descripcion
            lblFolioVisita.Text = ConfigurationManager.AppSettings("msgFolioPaginaDetalle").ToString() & visita.FolioVisita
            lblPasoVisita.Text = ConfigurationManager.AppSettings("msgPasoPaginaDetalle").ToString() & visita.IdPasoActual.ToString() & " - " & visita.DescripcionPasoActual

            ''Primera pestania principal
            cuDetallePrincipal.pvVisita = visita
            cuDetallePrincipal.piIdVisitaActual = visita.IdVisitaGenerado
            cuDetallePrincipal.puObjUsuario = puObjUsuario

            tpPestaniaP.HeaderText = visita.NombreEntidad

            ''Ocultar y deshabilitar todos los controles de la pagina para ir habilitando solo los necesarios
            ''solo si no es cancelada ya que posterior mente se valida esto
            If Not visita.EsCancelada Then
               HabDesHabCtrls(Me.Page, False)
            End If

            ''Valida si tiene permisos el usuario sobre la visita
            If Not IsNothing(puObjUsuario) Then
               ''=====================================================================================================================
               ''Si es usuario supervisor de VJ, validar si es usuario SupAsesor, SupSanciones o SupContencioso
               If puObjUsuario.IdArea = Constantes.AREA_VJ And puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP Then
                  If ((From objAbo As Abogado In visita.LstAbogadosSupAsesorAsig
                      Where objAbo.Id = puObjUsuario.IdentificadorUsuario Select objAbo).Count > 0) Then
                     If visita.IdPasoActual > 19 Then
                        lbPermisos = False
                     Else
                        lbPermisos = True
                     End If
                  End If

                  If ((From objAbo As Abogado In visita.LstAbogadosSupSancionAsig
                      Where objAbo.Id = puObjUsuario.IdentificadorUsuario Select objAbo).Count > 0) Then
                     If (visita.IdPasoActual < 20 Or visita.IdPasoActual > 29) Then
                        lbPermisos = False
                     Else
                        lbPermisos = True
                     End If
                  End If

                  If ((From objAbo As Abogado In visita.LstAbogadosSupContenAsig
                      Where objAbo.Id = puObjUsuario.IdentificadorUsuario Select objAbo).Count > 0) Then
                     If (visita.IdPasoActual < 30 Or visita.IdPasoActual > 37) Then
                        lbPermisos = False
                     Else
                        lbPermisos = True
                     End If
                  End If
               End If


               ''VALIDAR PERMISOS PARA ANTES DEL PASO 19 PARA EL AREA DE VO SI UNA VISITA VIENE DE SISVIG
               If visita.VisitaSisvig And visita.IdPasoActual <= 19 And EsAreaOperativa(puObjUsuario.IdArea) Then
                  lbPermisos = False
               End If

               ''********************************************************************************************************************
               ''                  METODO PRINCIPAL QUE VALIDA LOS PERMISOS DEL USUARIO
               ''********************************************************************************************************************
               If lbPermisos Then
                  lbPermisos = HabilitarBotonesDetalle(puObjUsuario, visita)
               End If
               ''********************************************************************************************************************
               ''********************************************************************************************************************
               validarBotonesEspeciales(lbPermisos, puObjUsuario, visita)
               ''=====================================================================================================================

               ''La primera vez marca como apropiadas las visitas y sus subvisitas
               ''solo si el usuario tiene permisos sobre la visita
               If lbPermisos Then
                  If ppObjVisita.UsuarioEstaOcupando.Trim().Length < 1 Then
                     AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, Date.Now, Constantes.TipoFecha.BanderaDeApropiacion, "", puObjUsuario.IdentificadorUsuario)
                     ppObjVisita.EstaVisitaOcupada = True
                     ppObjVisita.UsuarioEstaOcupando = puObjUsuario.IdentificadorUsuario
                  Else
                     If ppObjVisita.UsuarioEstaOcupando.Trim() <> puObjUsuario.IdentificadorUsuario.Trim() Then
                        Dim objUsu As New Entities.Usuario(ppObjVisita.UsuarioEstaOcupando)

                        If Not IsNothing(objUsu) Then
                           Dim errores As New Entities.EtiquetaError(2175)
                           Mensaje = errores.Descripcion.Replace("[NOM_USUARIO]", objUsu.Nombre & " " & objUsu.Apellido)
                        Else
                           Mensaje = "Visita ocupada, se muestra en modo de solo lectura."
                        End If

                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
                     End If
                  End If
               Else
                  If Not IsNothing(puObjUsuario) And Not IsNothing(ppObjVisita) Then
                     If ppObjVisita.UsuarioEstaOcupando = puObjUsuario.IdentificadorUsuario Then
                        AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, Date.Now, Constantes.TipoFecha.LimpiaBanderaDeApropiacion, "", puObjUsuario.IdentificadorUsuario)
                        ppObjVisita.UsuarioEstaOcupando = ""
                     End If
                  End If
               End If
            End If
         End If

         ''Inicializar la visita del control documento agc principal
         ''**************************************************************************************
         ucDocumentos.ModificaIds(visita.IdVisitaGenerado)

         If Not IsNothing(ucDocumentos.pObjPropiedades) And Not IsPostBack Then
            ucDocumentos.pObjPropiedades = Nothing
         End If

         If IsNothing(ucDocumentos.pObjPropiedades) Then
            Dim objPropiedades As New ucDocumentos.PropiedadesDoc

            objPropiedades.ppObjVisita = visita
            objPropiedades.pgIdTxtComentarios = "txbComentarios"
            objPropiedades.pbEsSubFolioSubVisita = visita.EsSubVisitaOsubFolio
            objPropiedades.piIdWidthPanel = 69
            objPropiedades.PermisoEditarDocs = lbPermisos

            ucDocumentos.pObjPropiedades = objPropiedades

            tpPestaniasDocsP.HeaderText = visita.NombreEntidad
         End If
         ''**************************************************************************************

         ''cargar pestanias de subvisitas
         If (From objSubVisi As Visita.SubVisitas In visita.LstSubVisitas Where objSubVisi.EstaSeleccionada = True Select objSubVisi).Count > 0 Then
            cargaPestaniasSubVisitas(visita, lbPermisos)

            chkSubVisitasMod.DataSource = visita.LstSubVisitas
            chkSubVisitasMod.DataValueField = "Id"
            chkSubVisitasMod.DataTextField = "Folio"
            chkSubVisitasMod.DataBind()

            For Each ltItemLista As ListItem In chkSubVisitasMod.Items
               Dim objSubvita As Visita.SubVisitas = (From objSV In visita.LstSubVisitas Where objSV.Id = ltItemLista.Value).FirstOrDefault()

               If Not IsNothing(objSubvita) Then
                  If objSubvita.EstaSeleccionada Then
                     ltItemLista.Selected = True
                  End If
               End If
            Next
         End If

         If (((visita.IdPasoActual <> PasoProcesoVisita.Pasos.Uno Or
             (visita.IdPasoActual = PasoProcesoVisita.Pasos.Uno And
              (visita.IdEstatusActual <> Constantes.EstatusPaso.AjustesRealizados And
              visita.IdEstatusActual <> Constantes.EstatusPaso.Registrado))) Or puObjUsuario.IdArea <> Constantes.AREA_VF)) Or
             Not visita.TieneSubVisitas Then
            ''Si no hay subvisitas limpiar los botones
            imgSiguiente.OnClientClick = "MuestraImgCarga(this);"
            imgDetener.OnClientClick = "MuestraImgCarga(this);"
            imgNotificar3.OnClientClick = "MuestraImgCarga(this);"
            imgGuardarCambios.OnClientClick = "MuestraImgCarga(this);"
            imgNotificar.OnClientClick = "MuestraImgCarga(this);"
            imgAnterior.OnClientClick = "MuestraImgCarga(this);"
            imgNotificar2.OnClientClick = "MuestraImgCarga(this);"
            imgIniciarVisita.OnClientClick = "MuestraImgCarga(this);"
            imgRechazarProrroga.OnClientClick = "MuestraImgCarga(this);"
            imgAprobarProrroga.OnClientClick = "MuestraImgCarga(this);"
            btnEditAllazgosInt.OnClientClick = "MuestraImgCarga(this);"
            btnEditAllazgosExt.OnClientClick = "MuestraImgCarga(this);"
         End If
      End If
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

   Protected Sub imgCancelarVisita_Click(sender As Object, e As ImageClickEventArgs) Handles imgCancelarVisita.Click
      ast1.Attributes.Remove("class")
      ast1.Attributes.Add("class", "AsteriscoHide")
      btnAceptarM2B1A.CommandArgument = "imgCancelarVisita"
      btnCancelarM2B1A.CommandArgument = "imgCancelarVisita"
      Dim errores As New Entities.EtiquetaError(2129)
      Mensaje = errores.Descripcion
      ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionCancelar();", True)
   End Sub

   Protected Sub imgSolicitarProrroga_Click(sender As Object, e As ImageClickEventArgs) Handles imgSolicitarProrroga.Click
      ast2.Attributes.Remove("class")
      ast2.Attributes.Add("class", "AsteriscoHide")
      btnAceptarM2B1A.CommandArgument = "imgSolicitarProrroga"
      btnCancelarM2B1A.CommandArgument = "imgSolicitarProrroga"
      Dim errores As New Entities.EtiquetaError(2130)
      Mensaje = errores.Descripcion
      ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionProrroga();", True)
   End Sub

   Protected Sub btnAceptarM2B1A_Click(sender As Object, e As EventArgs) Handles btnAceptarM2B1A.Click

      Dim objUsuario As New Entities.Usuario()
      objUsuario = Session(Entities.Usuario.SessionID)

      Dim objNegVisita As NegocioVisita

      ''Objeto negocio visita
      If IsNothing(objUsuario) Then
         Exit Sub
      End If

      Dim visita As Visita = CType(Session("DETALLE_VISITA"), Visita)

      If IsNothing(visita) Or visita.IdVisitaGenerado <= 0 Then
         Exit Sub
      End If

      objNegVisita = New NegocioVisita(visita, objUsuario, Server, txbComentarios.Text)

      Select Case btnAceptarM2B1A.CommandArgument

         Case "imgCancelarVisita"

            If txbMotivoCancelacion.Text.Trim() = String.Empty Then
               ast1.Attributes.Remove("class")
               ast1.Attributes.Add("class", "AsteriscoShow")

               btnAceptarM2B1A.CommandArgument = "imgCancelarVisita"
               Dim errores As New Entities.EtiquetaError(2129)
               Mensaje = errores.Descripcion
               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionCancelar();", True)

            Else
               ast1.Attributes.Remove("class")
               ast1.Attributes.Add("class", "AsteriscoHide")

               'Se procede a cancelar la visita
               If Not IsNothing(Session("ID_VISITA")) And txbMotivoCancelacion.Text.Trim() <> String.Empty Then

                  Dim con As Conexion.SQLServer = Nothing
                  Dim tran As SqlClient.SqlTransaction = Nothing
                  Dim guardo As Boolean = False
                  Dim idVistaCancelar As Integer

                  Try
                     con = New Conexion.SQLServer()
                     tran = con.BeginTransaction()

                     idVistaCancelar = Convert.ToInt32(Session("ID_VISITA"))
                     Dim motivoCancelacion As String = txbMotivoCancelacion.Text.Trim()

                     If AccesoBD.cancelarVisita(idVistaCancelar, motivoCancelacion, con, tran) = True Then
                        guardo = True
                     End If

                  Catch ex As Exception
                     guardo = False
                     Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, btnAceptarM2B1A_Click-imgCancelarVisita", "")
                  Finally
                     If Not IsNothing(tran) Then
                        If guardo Then
                           'Cancelación exitosa
                           tran.Commit()
                           ''NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_CANCELACION, idVistaCancelar, hfFolioVisita.Value.ToString())

                           objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_CANCELACION, visita, True, True, False, , True)

                           'Mostra mensaje si desa registrar una nueva visita
                           ' SI: redirect registro visita
                           ' NO: redirect bandeja visitas

                           btnAceptarM2B1A.CommandArgument = "redirectDespuesDeCancelar"
                           btnCancelarM2B1A.CommandArgument = "redirectDespuesDeCancelar"


                           imgDosBotonesUnaAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                           Dim errores As New Entities.EtiquetaError(2132)
                           Mensaje = errores.Descripcion
                           ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionRedirect();", True)

                        Else
                           'Cancelación fallida
                           tran.Rollback()
                           imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgError
                           Dim errores As New Entities.EtiquetaError(2131)
                           Mensaje = errores.Descripcion
                           ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
                        End If
                        tran.Dispose()
                     End If

                     If Not IsNothing(con) Then
                        con.CerrarConexion()
                        con = Nothing
                     End If
                  End Try
               End If

            End If

         Case "imgSolicitarProrroga"

            If txbMotivoProrroga.Text.Trim() = String.Empty Then
               ast2.Attributes.Remove("class")
               ast2.Attributes.Add("class", "AsteriscoShow")

               btnAceptarM2B1A.CommandArgument = "imgSolicitarProrroga"
               Dim errores As New Entities.EtiquetaError(2130)
               Mensaje = errores.Descripcion
               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionProrroga();", True)

            Else
               ast2.Attributes.Remove("class")
               ast2.Attributes.Add("class", "AsteriscoHide")

               'Se procede a registrar la solicitud de prorroga
               If Not IsNothing(Session("DETALLE_VISITA")) And txbMotivoProrroga.Text.Trim() <> String.Empty Then

                  If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 Then

                     Dim prorroga As New Prorroga()

                     prorroga.IdVisitaGenerado = visita.IdVisitaGenerado
                     prorroga.IdPaso = visita.IdPasoActual
                     prorroga.FechaRegistro = DateTime.Now
                     prorroga.NumDiasDeProrroga = 0
                     prorroga.MotivoProrroga = txbMotivoProrroga.Text.Trim()
                     prorroga.FechaFinProrroga = Nothing
                     If objUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP Or
                         objUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM Then
                        prorroga.ApruebaProrroga = Constantes.Verdadero
                     Else
                        prorroga.ApruebaProrroga = Constantes.Falso
                     End If
                     prorroga.SubVisitasSeleccionadas = ppObjVisita.SubVisitasSeleccionadas
                     visita.MotivoProrroga = txbMotivoProrroga.Text.Trim()

                     Dim con As Conexion.SQLServer = Nothing
                     Dim tran As SqlClient.SqlTransaction = Nothing
                     Dim guardo As Boolean = False

                     Try
                        con = New Conexion.SQLServer()
                        tran = con.BeginTransaction()
                        If AccesoBD.registrarProrroga(prorroga, con, tran) > 0 Then
                           guardo = True
                        End If

                     Catch ex As Exception
                        'Registro fallido
                        guardo = False
                        Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, btnAceptarM2B1A_Click-imgSolicitarProrroga", "")
                     Finally
                        If Not IsNothing(tran) Then
                           If guardo Then
                              'Solicitud de prorroga exitosa
                              tran.Commit()

                              ''Notificar prorroga 
                              If objUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP Or
                                      objUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM Then
                                 objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_VJ_PR_VISITA_ENTRA_EN_PRORROGA,
                                                                               visita, True, True, True)
                              Else
                                 objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_A_SUPERVISOR_PRORROGA,
                                                                                   visita, True, False, False, , True)
                              End If
                           Else
                              'Solicitud de prorroga fallida
                              tran.Rollback()
                              imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgError
                              Dim errores As New Entities.EtiquetaError(2133)
                              Mensaje = errores.Descripcion
                              ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
                           End If
                           tran.Dispose()
                        End If

                        If Not IsNothing(con) Then
                           con.CerrarConexion()
                           con = Nothing
                        End If
                     End Try

                     Response.Redirect("../Procesos/DetalleVisita.aspx")
                  End If
               End If
            End If
         Case "redirectDespuesDeCancelar"
            LiberaVisita()
            Response.Redirect("../Visita/Registro.aspx")

         Case "confirmarFechaInicioVisita"
            Dim ldFecIni As DateTime

            ''Regresa DateTime.MinValue si alguna validacion falla
            ldFecIni = ValidarFechaGeneral(txtFecIniVista.Text.Trim(), lblErrorFecIni, imgUnBotonNoAccion, "MensajeConfirmacionFechaInicioVisita();", False)
            If ldFecIni = DateTime.MinValue Then
               Exit Sub
            End If

            ''Objeto negocio visita
            objNegVisita.ppObservaciones = txbComentarios.Text.Trim()

            ''objNegVisita.PasoCuatroSupervisorNotificaInicioVisita(ldFecIni)
            visita.FechaInicioVisita = ldFecIni

            If AccesoBD.ActualizaFechaInicioVisita(visita.IdVisitaGenerado, ldFecIni, Constantes.TipoFecha.FechaInicio, ppObjVisita.SubVisitasSeleccionadas) Then
               objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                            True, Constantes.EstatusPaso.Notificado,
                                                            True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                            True, Constantes.CORREO_FECHA_INICIO_VISITA, True, True, True, , , , , , , True)
               ''iniciar tambien el paso 6
               objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                            False, -1,
                                                            True, 6, Constantes.EstatusPaso.EnRevisionEspera,
                                                            False, -1)
            End If

            Response.Redirect("../Procesos/DetalleVisita.aspx")

         Case "ingresarFechaFinVisita" ''Paso 6 detener visita

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 Then
               ''Validar que este llena y sea correcta la fecha de campo
               Dim lsFecha As String = txbFechFinVisita.Text.Trim()
               Dim ldFechaCampo As Date

               ''Regresa DateTime.MinValue si alguna validacion falla
               ldFechaCampo = ValidarFechaGeneral(lsFecha, lblMensajeFechaFinVisitaObligatorio, imgUnBotonNoAccion, "MensajeConfirmacionSolicitarFechaFinVisita();", True, True)
               If ldFechaCampo = DateTime.MinValue Then
                  Exit Sub
               End If

               ''Objeto negocio visita
               If Not IsNothing(objUsuario) Then
                  objNegVisita.ppObservaciones = txbComentarios.Text.Trim()

                  If AccesoBD.ActualizaFechaInicioVisita(visita.IdVisitaGenerado, ldFechaCampo, Constantes.TipoFecha.FechaCampoFinal, ppObjVisita.SubVisitasSeleccionadas) Then
                     objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                             True, Constantes.EstatusPaso.Visita_Finalizada,
                                                                             False, -1, -1,
                                                                             False, -1,
                                                                             False, False, False, False, -1, -1, -1, -1, ldFechaCampo)

                     objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1, False, -1,
                                                                             True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.En_diagnostico_de_hallazgos,
                                                                             False, -1,
                                                                             False, False, False, False, -1, -1, -1, -1, AccesoBD.ObtenerFecha(ldFechaCampo, 1, Constantes.IncremeteDecrementa.Incrementa)) ''ya que empieza al dia siguiente del fin de la visita de campo)
                  End If

               End If
            End If
            Response.Redirect("../Procesos/DetalleVisita.aspx")

         Case "ingresarFechaEnvioDictamen"

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 Then

               Dim lsFecha As String = txbFechaEnvioDictamen.Text.Trim()
               Dim ldFechaCampo As Date

               ''Regresa DateTime.MinValue si alguna validacion falla
               ldFechaCampo = ValidarFechaGeneral(lsFecha, lblMensajeFechaEnvioDictamenObligatorio, imgUnBotonNoAccion, "MensajeConfirmacionSolicitarFechaEnviaDictamen();", True, True)
               If ldFechaCampo = DateTime.MinValue Then
                  Exit Sub
               End If

               ''Objeto negocio visita
               If Not IsNothing(objUsuario) Then
                  objNegVisita = New NegocioVisita(visita, objUsuario, Server, txbComentarios.Text)

                  objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                   True, Constantes.EstatusPaso.Notificado,
                                                                   True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                   True, Constantes.CORREO_ID_NOTIFICA_VJ_DICTAMEN, True, True, False, , , , , , , True)
               End If

               Response.Redirect("../Procesos/DetalleVisita.aspx")
            End If

         Case "ingresarFechaPosibleEmplazamiento"

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 Then

               If txbFechaPosibleEmplazamiento.Text = String.Empty Then
                  btnAceptarM2B1A.CommandArgument = "ingresarFechaPosibleEmplazamiento"
                  imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                  lblMensajeFechaPosibleEmplazamientoObligatorio.Visible = True
                  lblMensajeFechaPosibleEmplazamientoObligatorio.Text = "*Favor de ingresar la fecha posible de emplazamiento."
                  ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaPosibleEmplazamiento();", True)
               Else
                  Dim fechaPosibleEmplazamiento As DateTime
                  fechaPosibleEmplazamiento = Convert.ToDateTime(txbFechaPosibleEmplazamiento.Text.Trim())

                  Dim liNumDiasAux As Integer = 0
                  Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.DiasFechasAnteriores), liNumDiasAux)
                  Dim ldFechaAuxAnterior As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDiasAux, Constantes.IncremeteDecrementa.Decrementa)

                  If fechaPosibleEmplazamiento.Date < ldFechaAuxAnterior.Date Then
                     Dim errores As New Entities.EtiquetaError(2165)
                     lblMensajeFechaPosibleEmplazamientoObligatorio.Visible = True
                     lblMensajeFechaPosibleEmplazamientoObligatorio.Text = errores.Descripcion.Replace("[FECHA]", ldFechaAuxAnterior.Date.ToString("dd/MM/yyyy"))
                     btnAceptarM2B1A.CommandArgument = "ingresarFechaPosibleEmplazamiento"
                     ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaPosibleEmplazamiento();", True)
                     Exit Sub
                  Else ''NO SE VALIDA FECHA DE INICIO DEL PASO ACTUAL
                     Dim objCorreoBD As New Entities.Correo(Constantes.CORREO_ID_NOTIFICA_FECHA_POSIBLE_EMPLAZAMIENTO)
                     objCorreoBD.Cuerpo = objCorreoBD.Cuerpo.Replace("[FECHA_POSIBLE_EMPLAZAMIENTO]", fechaPosibleEmplazamiento.ToString("dd/MM/yyyy"))


                     objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                      True, Constantes.EstatusPaso.Notificado,
                                                                      True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                      True, objCorreoBD, True, True, True, , , , , , , True)
                     Response.Redirect("../Procesos/DetalleVisita.aspx")
                  End If
               End If
            End If

         Case "ingresarFechaAcuseAforeRecibeOfEmpl" ''Paso 23

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 Then

               If txbFechaAcuseAforeRecibeOfEmpl.Text = String.Empty Then
                  btnAceptarM2B1A.CommandArgument = "ingresarFechaAcuseAforeRecibeOfEmpl"
                  imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                  lblMensajeFechaAcuseAforeRecibeOfEmplObligatorio.Visible = True
                  lblMensajeFechaAcuseAforeRecibeOfEmplObligatorio.Text = "*Favor de ingresar la fecha  del acuse  en que la afore recibe el oficio de emplazamiento."
                  ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaAcuseAforeRecibeOfEmpl();", True)
               Else
                  Dim objCorreoBD As New Entities.Correo(Constantes.CORREO_ID_NOTIFICA_FECHA_ACUSE_AFORE_RECIBE_OF_EMPL)

                  Dim fechaAcuseAforeRecibeOfEmpl As DateTime
                  fechaAcuseAforeRecibeOfEmpl = Convert.ToDateTime(txbFechaAcuseAforeRecibeOfEmpl.Text.Trim() & " 23:59:00")

                  Dim liNumDiasAux As Integer = 0
                  Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.DiasFechasAnteriores), liNumDiasAux)
                  Dim ldFechaAuxAnterior As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDiasAux, Constantes.IncremeteDecrementa.Decrementa)

                  If fechaAcuseAforeRecibeOfEmpl.Date < ldFechaAuxAnterior.Date Then
                     Dim errores As New Entities.EtiquetaError(2165)
                     lblMensajeFechaAcuseAforeRecibeOfEmplObligatorio.Visible = True
                     lblMensajeFechaAcuseAforeRecibeOfEmplObligatorio.Text = errores.Descripcion.Replace("[FECHA]", ldFechaAuxAnterior.Date.ToString("dd/MM/yyyy"))
                     btnAceptarM2B1A.CommandArgument = "ingresarFechaAcuseAforeRecibeOfEmpl"
                     ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaAcuseAforeRecibeOfEmpl();", True)
                     Exit Sub
                  Else
                     Dim ldFechaAux As Date
                     If ppObjVisita.FechaInicioPasoActual <> DateTime.MinValue Then
                        ldFechaAux = ppObjVisita.FechaInicioPasoActual
                     Else
                        ldFechaAux = fechaAcuseAforeRecibeOfEmpl
                     End If

                     If fechaAcuseAforeRecibeOfEmpl.Date < ldFechaAux.Date Then
                        Dim errores As New Entities.EtiquetaError(2164)
                        lblMensajeFechaAcuseAforeRecibeOfEmplObligatorio.Visible = True
                        lblMensajeFechaAcuseAforeRecibeOfEmplObligatorio.Text = errores.Descripcion.Replace("[FECHA]", ldFechaAux.ToString("dd/MM/yyyy"))

                        btnAceptarM2B1A.CommandArgument = "ingresarFechaAcuseAforeRecibeOfEmpl"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaAcuseAforeRecibeOfEmpl();", True)
                        Exit Sub
                     Else
                        Dim body As String = objCorreoBD.Cuerpo.Replace("[FECHA_ACUSE_AFORE_RECIBE_OF_EMPL]", fechaAcuseAforeRecibeOfEmpl.ToString("dd/MM/yyyy"))
                        objCorreoBD.Cuerpo = body

                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                     True, Constantes.EstatusPaso.Notificado,
                                                                     True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                     True, objCorreoBD, True, True, True, , , , , , fechaAcuseAforeRecibeOfEmpl, True)
                        Response.Redirect("../Procesos/DetalleVisita.aspx")
                     End If
                  End If
               End If
            End If


         Case "ingresarFechaAcuseAforeContestoOfEmpl" ''Paso 24

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 Then

               If txbFechaAcuseAforeContestoOfEmpl.Text = String.Empty Then

                  btnAceptarM2B1A.CommandArgument = "ingresarFechaAcuseAforeContestoOfEmpl"
                  imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                  lblMensajeFechaAcuseAforeContestoOfEmplObligatorio.Visible = True
                  lblMensajeFechaAcuseAforeContestoOfEmplObligatorio.Text = "*Favor de ingresar la fecha  del acuse en que la AFORE contestó al oficio de emplazamiento."
                  ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaAcuseAforeContestoOfEmpl();", True)

               Else
                  Dim objCorreoBD As New Entities.Correo(Constantes.CORREO_ID_NOTIFICA_FECHA_ACUSE_AFORE_CONTESTO_OF_EMPL)

                  Dim fechaAcuseAforeRecibeOfEmpl As DateTime
                  fechaAcuseAforeRecibeOfEmpl = Convert.ToDateTime(txbFechaAcuseAforeContestoOfEmpl.Text.Trim() & " 23:59:00")

                  Dim liNumDiasAux As Integer = 0
                  Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.DiasFechasAnteriores), liNumDiasAux)
                  Dim ldFechaAuxAnterior As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDiasAux, Constantes.IncremeteDecrementa.Decrementa)

                  If fechaAcuseAforeRecibeOfEmpl.Date < ldFechaAuxAnterior.Date Then
                     Dim errores As New Entities.EtiquetaError(2165)
                     lblMensajeFechaAcuseAforeContestoOfEmplObligatorio.Visible = True
                     lblMensajeFechaAcuseAforeContestoOfEmplObligatorio.Text = errores.Descripcion.Replace("[FECHA]", ldFechaAuxAnterior.Date.ToString("dd/MM/yyyy"))
                     btnAceptarM2B1A.CommandArgument = "ingresarFechaAcuseAforeContestoOfEmpl"
                     ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaAcuseAforeContestoOfEmpl();", True)
                     Exit Sub
                  Else
                     Dim ldFechaAux As Date
                     If ppObjVisita.FechaInicioPasoActual <> DateTime.MinValue Then
                        ldFechaAux = ppObjVisita.FechaInicioPasoActual
                     Else
                        ldFechaAux = fechaAcuseAforeRecibeOfEmpl
                     End If

                     If fechaAcuseAforeRecibeOfEmpl.Date < ldFechaAux.Date Then
                        Dim errores As New Entities.EtiquetaError(2164)
                        lblMensajeFechaAcuseAforeContestoOfEmplObligatorio.Visible = True
                        lblMensajeFechaAcuseAforeContestoOfEmplObligatorio.Text = errores.Descripcion.Replace("[FECHA]", ldFechaAux.ToString("dd/MM/yyyy"))

                        btnAceptarM2B1A.CommandArgument = "ingresarFechaAcuseAforeContestoOfEmpl"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaAcuseAforeContestoOfEmpl();", True)
                        Exit Sub
                     Else
                        Dim body As String = objCorreoBD.Cuerpo.Replace("[FECHA_ACUSE_AFORE_CONTESTO_OF_EMPL]", fechaAcuseAforeRecibeOfEmpl.ToString("dd/MM/yyyy"))
                        objCorreoBD.Cuerpo = body

                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                     True, Constantes.EstatusPaso.Respuesta_Afore,
                                                                     True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                     True, objCorreoBD, True, False, False, , , , , , fechaAcuseAforeRecibeOfEmpl)
                        Response.Redirect("../Procesos/DetalleVisita.aspx")
                     End If
                  End If
               End If
            End If

         Case "ingresarFechaImposicionSancion"

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 Then ''Paso 27

               If txbFechaImposicionSancion.Text = String.Empty Then
                  btnAceptarM2B1A.CommandArgument = "ingresarFechaImposicionSancion"
                  imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                  lblMensajeFechaImposicionSancionObligatorio.Visible = True
                  lblMensajeFechaImposicionSancionObligatorio.Text = "*Favor de ingresar la fecha de imposición de la sanción."
                  ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaImposicionSancion();", True)
               Else
                  Dim objCorreoBD As New Entities.Correo(Constantes.CORREO_ID_NOTIFICA_VO_VF_PR_HUBO_SANCION_FECHA_IMPOSICION)

                  Dim fechaImposicionSancion As DateTime
                  fechaImposicionSancion = Convert.ToDateTime(txbFechaImposicionSancion.Text.Trim())

                  Dim liNumDiasAux As Integer = 0
                  Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.DiasFechasAnteriores), liNumDiasAux)
                  Dim ldFechaAuxAnterior As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDiasAux, Constantes.IncremeteDecrementa.Decrementa)

                  If fechaImposicionSancion.Date < ldFechaAuxAnterior.Date Then
                     Dim errores As New Entities.EtiquetaError(2165)
                     lblMensajeFechaImposicionSancionObligatorio.Visible = True
                     lblMensajeFechaImposicionSancionObligatorio.Text = errores.Descripcion.Replace("[FECHA]", ldFechaAuxAnterior.Date.ToString("dd/MM/yyyy"))
                     btnAceptarM2B1A.CommandArgument = "ingresarFechaImposicionSancion"
                     ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaImposicionSancion();", True)
                     Exit Sub
                  Else
                     Dim ldFechaAux As Date
                     If ppObjVisita.FechaInicioPasoActual <> DateTime.MinValue Then
                        ldFechaAux = ppObjVisita.FechaInicioPasoActual
                     Else
                        ldFechaAux = fechaImposicionSancion
                     End If

                     If fechaImposicionSancion.Date < ldFechaAux.Date Then
                        Dim errores As New Entities.EtiquetaError(2164)
                        lblMensajeFechaImposicionSancionObligatorio.Visible = True
                        lblMensajeFechaImposicionSancionObligatorio.Text = errores.Descripcion.Replace("[FECHA]", ldFechaAux.ToString("dd/MM/yyyy"))

                        btnAceptarM2B1A.CommandArgument = "ingresarFechaImposicionSancion"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaImposicionSancion();", True)
                        Exit Sub
                     Else
                        Dim ldMonto As Decimal
                        Dim lsComentario As String = txtComentImpSan.Text

                        If Not Decimal.TryParse(txtMontoImpSan.Text.Trim().Replace("$", "").Replace(",", ""), ldMonto) Then
                           Dim errores As New Entities.EtiquetaError(2166)
                           lblMensajeFechaImposicionSancionObligatorio.Visible = True
                           lblMensajeFechaImposicionSancionObligatorio.Text = errores.Descripcion

                           btnAceptarM2B1A.CommandArgument = "ingresarFechaImposicionSancion"
                           ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaImposicionSancion();", True)
                           Exit Sub
                        Else
                           If ldMonto < 1.0 Then
                              Dim errores As New Entities.EtiquetaError(2167)
                              lblMensajeFechaImposicionSancionObligatorio.Visible = True
                              lblMensajeFechaImposicionSancionObligatorio.Text = errores.Descripcion

                              btnAceptarM2B1A.CommandArgument = "ingresarFechaImposicionSancion"
                              ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaImposicionSancion();", True)
                              Exit Sub
                           End If
                        End If

                        If lsComentario.Trim().Length < 1 Then
                           Dim errores As New Entities.EtiquetaError(2121)
                           lblMensajeFechaImposicionSancionObligatorio.Visible = True
                           lblMensajeFechaImposicionSancionObligatorio.Text = errores.Descripcion

                           btnAceptarM2B1A.CommandArgument = "ingresarFechaImposicionSancion"
                           ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaImposicionSancion();", True)
                           Exit Sub
                        End If

                        visita.Fecha_ImpSancion = fechaImposicionSancion
                        visita.MontoImpSan = ldMonto
                        visita.ComentariosImpSan = lsComentario

                        If AccesoBD.ActualizarRangoImpSancion(visita.IdVisitaGenerado, fechaImposicionSancion, ldMonto, lsComentario, ppObjVisita.SubVisitasSeleccionadas) Then
                           objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                       True, Constantes.EstatusPaso.Notificado,
                                                                       True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                       True, objCorreoBD, True, True, True, , , , , , fechaImposicionSancion, True)
                           ''Si es supervisor solicitar confirmar fecha con VJ
                           If Not visita.SolicitoFechaPaso25 Then
                              Mensaje = Constantes.MensajesModal.ConfirmacionFechaVjPaso27 & visita.Fecha_ReunionVoPaso25.ToString("dd/MM/yyyy") & "."
                              txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_ReunionVoPaso25)
                              ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "ConfirmarFechaReunPaso25SiNo();", True)
                              Exit Sub
                           Else
                              Response.Redirect("../Procesos/DetalleVisita.aspx")
                           End If
                        End If
                     End If
                  End If
               End If
            End If

         Case "ingresarFechaImposicionSancionEstimada" ''Paso 28, Impuesto

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 Then

               If txbFechaImposicionSancionEstimada.Text = String.Empty Then

                  btnAceptarM2B1A.CommandArgument = "ingresarFechaImposicionSancionEstimada"
                  imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                  lblMensajeFechaImposicionSancionEstimadaObligatorio.Visible = True
                  lblMensajeFechaImposicionSancionEstimadaObligatorio.Text = "*Favor de ingresar la fecha de imposición de la sanción."
                  ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaImposicionSancionEstimada();", True)

               Else
                  Dim objCorreoBD As New Entities.Correo(Constantes.CORREO_ID_NOTIFICA_VO_VF_PR_HUBO_SANCION_FECHA_IMPOSICION_ESTIMADA)

                  Dim fechaImposicionSancionEstimada As DateTime
                  fechaImposicionSancionEstimada = Convert.ToDateTime(txbFechaImposicionSancionEstimada.Text.Trim() & " 23:59:00")

                  Dim liNumDiasAux As Integer = 0
                  Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.DiasFechasAnteriores), liNumDiasAux)
                  Dim ldFechaAuxAnterior As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDiasAux, Constantes.IncremeteDecrementa.Decrementa)

                  If fechaImposicionSancionEstimada.Date < ldFechaAuxAnterior.Date Then
                     Dim errores As New Entities.EtiquetaError(2165)
                     lblMensajeFechaImposicionSancionEstimadaObligatorio.Visible = True
                     lblMensajeFechaImposicionSancionEstimadaObligatorio.Text = errores.Descripcion.Replace("[FECHA]", ldFechaAuxAnterior.Date.ToString("dd/MM/yyyy"))
                     btnAceptarM2B1A.CommandArgument = "ingresarFechaImposicionSancionEstimada"
                     ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaImposicionSancionEstimada();", True)
                     Exit Sub
                  Else
                     Dim ldFechaAux As Date
                     If ppObjVisita.FechaInicioPasoActual <> DateTime.MinValue Then
                        ldFechaAux = ppObjVisita.FechaInicioPasoActual
                     Else
                        ldFechaAux = fechaImposicionSancionEstimada
                     End If

                     If fechaImposicionSancionEstimada.Date < ldFechaAux.Date Then
                        Dim errores As New Entities.EtiquetaError(2164)
                        lblMensajeFechaImposicionSancionEstimadaObligatorio.Visible = True
                        lblMensajeFechaImposicionSancionEstimadaObligatorio.Text = errores.Descripcion.Replace("[FECHA]", ldFechaAux.ToString("dd/MM/yyyy"))

                        btnAceptarM2B1A.CommandArgument = "ingresarFechaImposicionSancionEstimada"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaImposicionSancionEstimada();", True)
                        Exit Sub
                     Else
                        visita.Fecha_ImpSancion = fechaImposicionSancionEstimada
                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                        True, Constantes.EstatusPaso.Impuesto,
                                                                        True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                        True, objCorreoBD, True, True, True, , , , , , fechaImposicionSancionEstimada, True)
                        Response.Redirect("../Procesos/DetalleVisita.aspx")
                     End If
                  End If
               End If

            End If

         Case "ingresarFechaAcusePagoAfore" ''Paso 29, Pagado

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 Then

               If txbFechaAcusePagoAfore.Text = String.Empty Then

                  btnAceptarM2B1A.CommandArgument = "ingresarFechaAcusePagoAfore"
                  imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                  lblMensajeFechaAcusePagoAforeObligatorio.Visible = True
                  lblMensajeFechaAcusePagoAforeObligatorio.Text = "*Favor de ingresar la fecha de acuse del pago por parte de la afore."
                  ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaAcusePagoAfore();", True)

               Else
                  Dim objCorreoBD As New Entities.Correo(Constantes.CORREO_ID_NOTIFICA_VO_VF_PR_FECHA_ACUSE_PAGO_AFORE)

                  Dim fechaAcusePagoAfore As DateTime
                  fechaAcusePagoAfore = Convert.ToDateTime(txbFechaAcusePagoAfore.Text.Trim() & " 23:59:00")

                  Dim liNumDiasAux As Integer = 0
                  Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.DiasFechasAnteriores), liNumDiasAux)
                  Dim ldFechaAuxAnterior As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDiasAux, Constantes.IncremeteDecrementa.Decrementa)

                  If fechaAcusePagoAfore.Date < ldFechaAuxAnterior.Date Then
                     Dim errores As New Entities.EtiquetaError(2165)
                     lblMensajeFechaAcusePagoAforeObligatorio.Visible = True
                     lblMensajeFechaAcusePagoAforeObligatorio.Text = errores.Descripcion.Replace("[FECHA]", ldFechaAuxAnterior.Date.ToString("dd/MM/yyyy"))
                     btnAceptarM2B1A.CommandArgument = "ingresarFechaAcusePagoAfore"
                     ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaAcusePagoAfore();", True)
                     Exit Sub
                  Else
                     Dim ldFechaAux As Date
                     If ppObjVisita.FechaInicioPasoActual <> DateTime.MinValue Then
                        ldFechaAux = ppObjVisita.FechaInicioPasoActual
                     Else
                        ldFechaAux = fechaAcusePagoAfore
                     End If

                     If fechaAcusePagoAfore.Date < ldFechaAux.Date Then
                        Dim errores As New Entities.EtiquetaError(2164)
                        lblMensajeFechaAcusePagoAforeObligatorio.Visible = True
                        lblMensajeFechaAcusePagoAforeObligatorio.Text = errores.Descripcion.Replace("[FECHA]", ldFechaAux.ToString("dd/MM/yyyy"))

                        btnAceptarM2B1A.CommandArgument = "ingresarFechaAcusePagoAfore"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaAcusePagoAfore();", True)
                        Exit Sub
                     Else
                        Dim body2 As String = objCorreoBD.Cuerpo.Replace("[FECHA_ACUSE_PAGO_AFORE]", fechaAcusePagoAfore.ToString("dd/MM/yyyy"))
                        objCorreoBD.Cuerpo = body2

                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                        True, Constantes.EstatusPaso.Pagado,
                                                                        True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                        True, objCorreoBD, True, True, True, , , , , , fechaAcusePagoAfore, True)
                        Response.Redirect("../Procesos/DetalleVisita.aspx")
                     End If
                  End If
               End If

            End If

      End Select
   End Sub

   Protected Sub btnCancelarM2B1A_Click(sender As Object, e As EventArgs) Handles btnCancelarM2B1A.Click
      Select Case btnCancelarM2B1A.CommandArgument

         Case "imgCancelarVisita"
            txbMotivoCancelacion.Text = String.Empty

         Case "imgSolicitarProrroga"
            txbMotivoProrroga.Text = String.Empty

         Case "redirectDespuesDeCancelar"
            LiberaVisita()
            Response.Redirect("../Visita/Bandeja.aspx")

         Case "confirmarNotificar"
            Response.Redirect("../Procesos/DetalleVisita.aspx")

      End Select
   End Sub


   Public Function tieneEstatusCancelado() As Boolean
      Dim estaCancelada As Boolean = False

      If Not IsNothing(Session("DETALLE_VISITA")) Then
         Dim visita As New Visita()
         visita = CType(Session("DETALLE_VISITA"), Visita)

         If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 Then
            If visita.EsCancelada = True Then
               'La visita esta cancelada y solo será de consulta
               imgCancelarVisita.Visible = False
               imgSolicitarProrroga.Visible = False
               imgSiguiente.Visible = False
               imgNotificar.Visible = False
               imgNotificar2.Visible = False
               HabDesHabCtrls_Cancelar(Me.Page, False)
               estaCancelada = True
            End If
         End If
      End If
      Return estaCancelada
   End Function

   Public Sub HabDesHabCtrls(ByVal oControl As Control, ByVal lHabilita As Boolean)
      Dim oCtrl As Control
      For Each oCtrl In oControl.Controls
         If TypeOf oCtrl Is TextBox Then
            'If Not oCtrl.ID = "txbComentarios" Or Not oCtrl.ID = "txbMotivoCancelacion" Or Not oCtrl.ID = "txbDiasProrroga" Then
            'CType(oCtrl, TextBox).ReadOnly = Not lHabilita
            'End If
         ElseIf TypeOf oCtrl Is ListBox Then
            Continue For
         ElseIf TypeOf oCtrl Is ucFiltro Then
            Continue For
         ElseIf TypeOf oCtrl Is DropDownList Then
            CType(oCtrl, DropDownList).Enabled = lHabilita
         ElseIf TypeOf oCtrl Is CheckBox Then
            CType(oCtrl, CheckBox).Enabled = lHabilita
         ElseIf TypeOf oCtrl Is ListBox Then
            CType(oCtrl, ListBox).Enabled = lHabilita
         ElseIf TypeOf oCtrl Is Button Then
            If CType(oCtrl, Button).Text = "Adjuntar" Then
               CType(oCtrl, Button).Enabled = lHabilita
            End If
         ElseIf TypeOf oCtrl Is FileUpload Then
            CType(oCtrl, FileUpload).Enabled = lHabilita
         ElseIf TypeOf oCtrl Is RadioButtonList Then
            CType(oCtrl, RadioButtonList).Enabled = lHabilita
         ElseIf TypeOf oCtrl Is RadioButton Then
            CType(oCtrl, RadioButton).Enabled = lHabilita
         ElseIf TypeOf oCtrl Is ImageButton Then
            If CType(oCtrl, ImageButton).ID = "imgAgregarDiasProrroga" Or CType(oCtrl, ImageButton).ID = "imgQuitarDiasProrroga" Then
               CType(oCtrl, ImageButton).Enabled = True
               CType(oCtrl, ImageButton).Visible = True
            Else
               CType(oCtrl, ImageButton).Enabled = lHabilita
               CType(oCtrl, ImageButton).Visible = lHabilita
            End If
            'ElseIf TypeOf oCtrl Is System.Web.UI.WebControls.Image Then
            '    CType(oCtrl, System.Web.UI.WebControls.Image).Enabled = lHabilita
            '    CType(oCtrl, System.Web.UI.WebControls.Image).Visible = lHabilita
         ElseIf oCtrl.Controls.Count > 0 Then
            HabDesHabCtrls(oCtrl, lHabilita)
         End If
      Next
      imgRevisarDocs.Enabled = True
      imgInicio.Enabled = True
      imgRevisarDocs.Visible = True
      imgInicio.Visible = True
   End Sub

   Public Sub HabDesHabCtrls_Cancelar(ByVal oControl As Control, ByVal lHabilita As Boolean)
      Dim oCtrl As Control
      For Each oCtrl In oControl.Controls
         If TypeOf oCtrl Is TextBox Then
            CType(oCtrl, TextBox).ReadOnly = Not lHabilita
         ElseIf TypeOf oCtrl Is DropDownList Then
            CType(oCtrl, DropDownList).Enabled = lHabilita
         ElseIf TypeOf oCtrl Is CheckBox Then
            CType(oCtrl, CheckBox).Enabled = lHabilita
         ElseIf TypeOf oCtrl Is ListBox Then
            CType(oCtrl, ListBox).Enabled = lHabilita
         ElseIf TypeOf oCtrl Is Button Then
            If CType(oCtrl, Button).Text = "Adjuntar" Then
               CType(oCtrl, Button).Enabled = lHabilita
            End If
         ElseIf TypeOf oCtrl Is FileUpload Then
            CType(oCtrl, FileUpload).Enabled = lHabilita
         ElseIf TypeOf oCtrl Is RadioButtonList Then
            CType(oCtrl, RadioButtonList).Enabled = lHabilita
         ElseIf TypeOf oCtrl Is RadioButton Then
            CType(oCtrl, RadioButton).Enabled = lHabilita
         ElseIf TypeOf oCtrl Is ImageButton Then
            If CType(oCtrl, ImageButton).ID = "imgAgregarDiasProrroga" Or CType(oCtrl, ImageButton).ID = "imgQuitarDiasProrroga" Then
               CType(oCtrl, ImageButton).Enabled = True
               CType(oCtrl, ImageButton).Visible = True
            Else
               CType(oCtrl, ImageButton).Enabled = lHabilita
               CType(oCtrl, ImageButton).Visible = lHabilita
            End If

         ElseIf oCtrl.Controls.Count > 0 Then
            HabDesHabCtrls_Cancelar(oCtrl, lHabilita)
         End If
      Next
      imgRevisarDocs.Enabled = True
      imgInicio.Enabled = True
      imgRevisarDocs.Visible = True
      imgInicio.Visible = True
   End Sub


   Public Sub activarBtns_Cancelar_SepSubvisita(objUsuario As Usuario, objVisita As Visita)
      'MOFICACION CANCELAR VISITA RRA
      Dim perfiles_act_Btns_Cancelar_Prorroga As New List(Of Integer)
      perfiles_act_Btns_Cancelar_Prorroga.Add(Constantes.PERFIL_ADM)
      perfiles_act_Btns_Cancelar_Prorroga.Add(Constantes.PERFIL_SUP)

      Dim pOK As Boolean
      pOK = perfilValido(perfiles_act_Btns_Cancelar_Prorroga)

      If pOK Then
         'RRA CANCELA VISITA
         If ppObjVisita.IdPasoActual <= 4 And EsAreaOperativa(puObjUsuario.IdArea) = True Then
            imgCancelarVisita.Visible = True
            imgCancelarVisita.Enabled = True
         Else
            imgCancelarVisita.Visible = False
         End If
         'Fin CANCELA VISITA
      Else
         'No cumple con el perfil
         imgCancelarVisita.Visible = False
      End If

      If objUsuario.IdArea = Constantes.AREA_VF And objVisita.TieneSubVisitas Then
         imgSubvisitas.Visible = True
         imgSubvisitas.Enabled = True
      Else
         imgSubvisitas.Visible = False
      End If

      'Oculta botones de fecha de vulnerabilidad si visita viene de sisvig y área es VO y paso es menor a 19
      If objVisita.VisitaSisvig And objVisita.IdPasoActual <= 19 And EsAreaOperativa(puObjUsuario.IdArea) Then
         imgCancelarVisita.Visible = False
         imgCancelarVisita.Enabled = False
      End If

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

   Public Sub validarBotonesEspeciales(lbPermisos As Boolean, objUsuario As Usuario, objVisita As Visita)
      imgSolicitarProrroga.Visible = False
      imgSolicitarProrroga.Enabled = False
      imgSubvisitas.Visible = False
      imgCancelarVisita.Visible = False
      ccfCopiarFolios.Visible = False
      sbSubVisitas.Visible = False
      ''Validar que algun boton de accion este visible para ingresar comentarios
      divComentarios.Visible = lbPermisos

      CambiaTituloModalSubvisitas(objVisita, objUsuario)

      'NHM INICIA LOG           
      Utilerias.ControlErrores.EscribirEvento("El sistema detecta que la visita se encuentra en el paso: " + objVisita.IdPasoActual.ToString(), EventLogEntryType.Information)
      Utilerias.ControlErrores.EscribirEvento("El sistema detecta que la visita se encuentra en el estatus: " + objVisita.IdEstatusActual.ToString(), EventLogEntryType.Information)
      Utilerias.ControlErrores.EscribirEvento("El sistema detecta que el perifl del usuario en sesión es: " + objUsuario.IdentificadorPerfilActual.ToString(), EventLogEntryType.Information)
      Utilerias.ControlErrores.EscribirEvento("El sistema detecta que el usuario en sesión pertenece al área con ID: " + TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea.ToString(), EventLogEntryType.Information)
      'NHM FIN LOG

      Dim bitacora As New Conexion.Bitacora("Consulto visita(" & objVisita.FolioVisita & ")", System.Web.HttpContext.Current.Session.SessionID, objUsuario.IdentificadorUsuario)
      bitacora.Finalizar(True)

      ''Valida el usuario que esta consultando la visita
      ''Si no son iguales no muestra habilita ningun boton
      If objVisita.UsuarioEstaOcupando.Trim().Length < 1 Or
          objVisita.UsuarioEstaOcupando.Trim() = puObjUsuario.IdentificadorUsuario.Trim() Then
         ''Activar la prorroga
         If objUsuario.IdArea <> Constantes.AREA_VJ Then
            ActivarProrroga(objVisita, objUsuario)
         End If

         ''Validar funcionalida de Copiar folios y Subvisitas de a cuerdo al area del usuario
         HabilitaFuncionalidadCopia(objUsuario, objVisita)

         ''Cancelar visita y el de separar subvisitas
         activarBtns_Cancelar_SepSubvisita(objUsuario, objVisita)

         'RRA Vulnerabilidad
         If objVisita.EstatusVulnerabilidad Then
            If ppObjVisita.IdPasoActual >= 1 And ppObjVisita.IdPasoActual <= 4 And EsAreaOperativa(puObjUsuario.IdArea) = True And
            (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP Or
             puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM) Then
               tbVulnerabilidad.Visible = True
               ImgFecVulnerabilidad.Enabled = True
               ImgFecVulnerabilidad.Visible = True
            Else
               tbVulnerabilidad.Visible = False
               ImgFecVulnerabilidad.Enabled = False
               ImgFecVulnerabilidad.Visible = False
            End If
         Else
            tbVulnerabilidad.Visible = False
            ImgFecVulnerabilidad.Enabled = False
            ImgFecVulnerabilidad.Visible = False
         End If
         'Fin Vulnerabilidad
      Else
         imgAnterior.Visible = False
         imgDetener.Visible = False
         imgGuardarCambios.Visible = False
         imgSiguiente.Visible = False
         imgIniciarVisita.Visible = False
         imgRechazarProrroga.Visible = False
         imgAprobarProrroga.Visible = False
         imgNotificar2.Visible = False
         imgNotificar3.Visible = False
         divComentarios.Visible = False

         imgSolicitarProrroga.Visible = False
         imgSolicitarProrroga.Enabled = False
         imgSubvisitas.Visible = False
         imgCancelarVisita.Visible = False
         ccfCopiarFolios.Visible = False
         sbSubVisitas.Visible = False
         divComentarios.Visible = False
      End If

      'Oculta botones de fecha de vulnerabilidad si visita viene de sisvig y área es VO y paso es menor a 19
      If objVisita.VisitaSisvig And objVisita.IdPasoActual <= 19 And EsAreaOperativa(puObjUsuario.IdArea) Then
         tbVulnerabilidad.Visible = False
         ImgFecVulnerabilidad.Enabled = False
         ImgFecVulnerabilidad.Visible = False
      End If

      tieneEstatusCancelado()

      ''AGC INICIO    ***************************************************************
      ''Ocultar botones de cancelar visita, iniciar visita y detener visita para VJ
      If (objVisita.VisitaSisvig And objVisita.IdPasoActual <= 19 And EsAreaOperativa(puObjUsuario.IdArea)) Or (objUsuario.IdArea = Constantes.AREA_VJ) Then
         imgDetener.Visible = False
         imgIniciarVisita.Visible = False
         imgCancelarVisita.Visible = False
      End If
      ''AGC FIN       ***************************************************************
   End Sub

   Protected Sub imgNotificar3_Click(sender As Object, e As ImageClickEventArgs) Handles imgNotificar3.Click
      If Not IsNothing(Session("DETALLE_VISITA")) And Not IsNothing(Entities.Usuario.SessionID) Then

         ObtenerSubVisitasSeleccionadas()

         ''Valida si hay documentos obligatorios sin cargar
         If HayDocumentosHobligatorios() Then Exit Sub

         Dim visita As New Visita()
         visita = CType(Session("DETALLE_VISITA"), Visita)

         Dim Usuario As New Entities.Usuario()
         Usuario = Session(Entities.Usuario.SessionID)

         If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 And Not IsNothing(Usuario) Then
            ''Objeto negocio visita
            Dim objNegVisita As New NegocioVisita(visita, Usuario, Server, txbComentarios.Text)

            Select Case visita.IdPasoActual
               Case 3
                  Select Case visita.IdEstatusActual
                     Case Constantes.EstatusPaso.Revisado ''Notifica que ya todos los documentos estan bien, o rechaza paso 3
                        Select Case Usuario.IdentificadorPerfilActual
                           Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                              If EsAreaOperativa(Usuario.IdArea) Then
                                 'objNegVisita.PasoTresSupervisorApruebaDocumentos()
                                 objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                               True, Constantes.EstatusPaso.Aprobado,
                                                               True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                               True, Constantes.CORREO_VERSION_FINAL_DOCUMENTOS,
                                                               True, True, False, , , , , , , True)
                                 Response.Redirect("../Procesos/DetalleVisita.aspx")
                              End If
                        End Select
                  End Select

               Case 4

                  Select Case visita.IdEstatusActual
                     Case Constantes.EstatusPaso.Revisado ''Paso 4, Revisado
                        Select Case Usuario.IdentificadorPerfilActual
                           Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                              If EsAreaOperativa(Usuario.IdArea) Then
                                 If visita.EstatusVulnerabilidad And Fechas.Vacia(visita.Fecha_AcuerdoVul) Then
                                    btnGuardaFecVulnerabilidad.CommandArgument = "EnviarConcluido"
                                    btnCancelaFecVulnerabilidad.CommandArgument = "EnviarConcluido"
                                    btnConfirmacionVulnerabilidadSi.CommandArgument = "EnviarConcluido"
                                    btnConfirmacionVulnerabilidadNo.CommandArgument = "EnviarConcluido"

                                    Mensaje = "¿La sesión para la revisión de vulnerabilidades se llevó a cabo?"
                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionFechaVulnerabilidad();", True)
                                    Exit Sub
                                 Else
                                    '---------PEDIR ESTE CODIGO INICIO
                                    btnAceptarM2B1A.CommandArgument = "confirmarFechaInicioVisita"
                                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning

                                    If visita.FechaInicioVisita <> Date.MinValue Then
                                       txtFecIniVista.Text = visita.FechaInicioVisita.ToString("dd/MM/yyyy")
                                    End If

                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionFechaInicioVisita();", True)
                                    Exit Sub
                                    '---------PEDIR ESTE CODIGO FINAL    
                                 End If

                              End If
                        End Select
                  End Select

               Case 7
                  Select Case visita.IdEstatusActual
                     Case Constantes.EstatusPaso.En_diagnostico_de_hallazgos ''paso 7 En_diagnostico_de_hallazgos
                        Select Case Usuario.IdentificadorPerfilActual
                           Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                              If EsAreaOperativa(Usuario.IdArea) Then
                                 Me.Mensaje = "¿Habrá Presentación de Hallazgos con Presidencia y la Vicepresidencia Jurídica?"
                                 ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "PreguntaReunionVjPresidencia();", True)
                                 Exit Sub
                              End If
                        End Select
                  End Select

               Case 8
                  Select Case visita.IdEstatusActual
                     Case Constantes.EstatusPaso.HallazgosGuardados ''Paso 8 HallazgosGuardados
                        Select Case Usuario.IdentificadorPerfilActual
                           Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Solo el supervisor/superior puede: *Notificar 
                              If EsAreaOperativa(Usuario.IdArea) Then
                                 txtFechaReunion.Text = Fechas.Valor(visita.FECH_REUNION__PRESIDENCIA)
                                 ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaReunionPresidencia();", True)
                                 Exit Sub
                              End If
                        End Select
                  End Select

               Case 12
                  Select Case visita.IdEstatusActual
                     Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 12
                        Select Case Usuario.IdentificadorPerfilActual
                           Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP  'Supervisor avisa de la version final de los documentos
                              If EsAreaOperativa(Usuario.IdArea) Then
                                 ''Manda un segundo correo a sandra pacheco, configurada en parametros
                                 ''VALIDAR SI EXISTIO UNA REUNION EN PASO 8, SI NO EXISTIO NO ENVIAR CORREO
                                 If visita.ExisteReunionPaso8 Then
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                              True, Constantes.EstatusPaso.Enviado,
                                                              True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                              True, Constantes.CORREO_VER_FINAL_ACTA_CIRCUNSTANCIADA,
                                                              True, True, False, , , , , , , True)

                                    If MandaCorreoSandraPachecoPaso12(objNegVisita) Then
                                       Response.Redirect("../Procesos/DetalleVisita.aspx")
                                    End If
                                 Else ''SI NO EXISTIO LA REUNION MANDA A PASO 14 DIRECTAMENTE
                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                              True, Constantes.EstatusPaso.Enviado,
                                                              True, 14, Constantes.EstatusPaso.EnRevisionEspera,
                                                              True, Constantes.CORREO_VER_FINAL_ACTA_CIRCUNSTANCIADA,
                                                              True, True, False, , , , , , , True)

                                    Response.Redirect("../Procesos/DetalleVisita.aspx")
                                 End If
                              End If
                        End Select
                  End Select

               Case 13
                  Select Case visita.IdEstatusActual
                     Case Constantes.EstatusPaso.Hallazgos_presentados
                        Select Case Usuario.IdentificadorPerfilActual
                           Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Manda fecha que confirmo sandra pacheco
                              If EsAreaOperativa(Usuario.IdArea) Then
                                 txtFechaGeneral.Text = Fechas.Valor(visita.FECH_REUNION__AFORE)
                                 ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaReunionEntidad();", True)
                                 Exit Sub
                              End If
                        End Select
                  End Select

               Case 14
                  Select Case visita.IdEstatusActual
                     Case Constantes.EstatusPaso.Registrado ''Solicitar fecha acta circunstanciada in situ
                        Select Case Usuario.IdentificadorPerfilActual
                           Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                              If EsAreaOperativa(Usuario.IdArea) Then
                                 txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_InSituActaCircunstanciada)
                                 ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('Favor de ingresar la fecha de levantamiento in situ de acta circunstanciada:', 'btnFechaInSituActaC');", True)
                                 Exit Sub
                              End If
                        End Select
                  End Select

               Case 18
                  Select Case visita.IdEstatusActual
                     Case Constantes.EstatusPaso.Revisado ''Flujo inicial paso 18, Revisado
                        Select Case Usuario.IdentificadorPerfilActual
                           Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Solo el Supervisor / Superior  puede:  *Notificar 
                              If EsAreaOperativa(Usuario.IdArea) Then
                                            txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_LevantamientoActaConclusion)
                                            Mensaje = Constantes.MensajesModal.AlertaPaso18
                                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MuestraAlertaPaso18();", True)
                                            Exit Sub
                                        End If
                                End Select
                        End Select

                    Case 19
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Paso 19 manda a paso 20
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            ''Modificacion agc odt1 sc2 elimina fecha envio dictamen
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                          True, Constantes.EstatusPaso.Notificado,
                                                          True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                          True, Constantes.CORREO_ID_NOTIFICA_VJ_DICTAMEN, True, True, False, , , , , , , True)
                                            Response.Redirect("../Procesos/DetalleVisita.aspx")

                                            'Solicita fecha en que se envian docs a VJ
                                            'btnAceptarM2B1A.CommandArgument = "ingresarFechaEnvioDictamen"
                                            'imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                                            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaEnviaDictamen();", True)
                                        End If
                                End Select
                        End Select

                    Case 22
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Notifica fecha de posible emplazamiento
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                'Solicita fecha posible de emplazamiento
                                                btnAceptarM2B1A.CommandArgument = "ingresarFechaPosibleEmplazamiento"
                                                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaPosibleEmplazamiento();", True)
                                        End Select
                                End Select
                        End Select

                    Case 25
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 25
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_ReunionVoPaso25)
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.FechaPaso25.Replace("[AREA]", Constantes.BuscarNombreArea(visita.IdArea)) & "', 'btnFechaPaso25');", True)
                                                Exit Sub
                                        End Select

                                End Select

                        End Select

                    Case 27
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 27, Notificado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                'Solicita fecha de imposición de la sanción
                                                txbFechaImposicionSancion.Text = Fechas.Valor(visita.Fecha_ImpSancion)
                                                txtMontoImpSan.Text = "$" & FormatNumber(visita.MontoImpSan.ToString(), 0, , , TriState.UseDefault)
                                                txtComentImpSan.Text = visita.ComentariosImpSan

                                                btnAceptarM2B1A.CommandArgument = "ingresarFechaImposicionSancion"
                                                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaImposicionSancion();", True)
                                        End Select
                                End Select
                        End Select

                    Case 28
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 28, Impuesto
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                'Solicita la fecha que se tenía estimada de la imposición de la sanción
                                                btnAceptarM2B1A.CommandArgument = "ingresarFechaImposicionSancionEstimada"
                                                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaImposicionSancionEstimada();", True)
                                        End Select
                                End Select
                        End Select

                    Case 29
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado  ''Paso 29, Pagado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                'Solicita la fecha de acuse del pago por parte de la afore
                                                btnAceptarM2B1A.CommandArgument = "ingresarFechaAcusePagoAfore"
                                                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaAcusePagoAfore();", True)
                                        End Select
                                End Select
                        End Select
                    Case 36
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado ''Paso 36, Revisado, notifica
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                                             True, Constantes.EstatusPaso.Respuesta_Notificada,
                                                                                             True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                                             True, Constantes.CORREO_ID_NOTIFICA_VO_VF_ADJUNTA_ACUSE_ENTREGA_RESPUESTA)
                                                Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End Select
                                End Select
                        End Select
                    Case 37
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 37
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                                             True, Constantes.EstatusPaso.Respuesta_Notificada,
                                                                                             False, -1, -1,
                                                                                             True, Constantes.CORREO_ID_NOTIFICA_VO_VF_PR_RESPUESTA_ENVIO_VJ, True, True, True, , , , , , , True)
                                                Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End Select
                                End Select
                        End Select
                End Select
            End If
        End If
    End Sub

    Protected Sub imgSiguiente_Click(sender As Object, e As ImageClickEventArgs) Handles imgSiguiente.Click
        If Not IsNothing(Session("DETALLE_VISITA")) And Not IsNothing(Entities.Usuario.SessionID) Then

            ObtenerSubVisitasSeleccionadas()

            ''Valida si hay documentos obligatorios sin cargar
            If HayDocumentosHobligatorios() Then Exit Sub

            Dim visita As New Visita()
            visita = CType(Session("DETALLE_VISITA"), Visita)

            Dim Usuario As New Entities.Usuario()
            Usuario = Session(Entities.Usuario.SessionID)

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 And Not IsNothing(Usuario) Then
                ''Objeto negocio visita
                Dim objNegVisita As New NegocioVisita(visita, Usuario, Server, txbComentarios.Text)

                Select Case visita.IdPasoActual

                    Case 1
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Registrado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Solo el supervisor/superior puede:  *Notificar 
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            ''objNegVisita.PasoUnoVoVfEnviaAVj()
                                            ''Envia al paso 2, cierra paso 1 y notifica
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                                           True, Constantes.EstatusPaso.Enviado,
                                                                                           True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.Enviado,
                                                                                           True, Constantes.CORREO_ID_NOTIFICA_VJ_REVISAR_OF_COM_ACT_INI,
                                                                                           False, True, False, , , , , , , True)
                                            Mensaje = "¿Habrá sesión de revisión de vulnerabilidades?"
                                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ConfirmacionVulnerabilidad", "ConfirmacionFechaVulnerabilidad();", True)
                                            Exit Sub
                                        End If
                                End Select
                            Case Constantes.EstatusPaso.AjustesRealizados ''Enviar nuevamente a VJ
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesEnviados,
                                                                                         False, -1,
                                                                                         False, -1, -1,
                                                                                         True, Constantes.CORREO_ID_NOTIFICA_VJ_REVISAR_OF_COM_ACT_INI,
                                                                                         True, True, False,
                                                                                         True, Constantes.TipoReactivacion.FinalizaReactivacion,
                                                                                         (visita.IdPasoActual + 1), visita.IdPasoActual,
                                                                                         Constantes.EstatusPaso.AsesorAsignado, , True)
                                        End If
                                End Select
                        End Select

                    Case 2
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.AsesorAsignado, Constantes.EstatusPaso.Revisado ''revisaso y asesor asignado paso 2
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Envia a paso 3
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                'objNegVisita.PasoDosSupervisorFinalizaPasoDos()
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                                            True, Constantes.EstatusPaso.Aprobado,
                                                                                            True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                                            True, Constantes.CORREO_ID_NOTIFICA_AREA_REGISTRO_VISITA_APROBADO,
                                                                                            True, True, False, , , , , , , True)
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.AjustesRealizados, Constantes.EstatusPaso.EnAjustes  ''Guarda paso 2, AjustesRealizados
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Aprobado,
                                                                                              False, -1,
                                                                                              False, -1, -1,
                                                                                              True, Constantes.CORREO_ID_NOTIFICA_AREA_REGISTRO_VISITA_APROBADO,
                                                                                              True, True, False,
                                                                                              True, Constantes.TipoReactivacion.FinalizaReactivacion,
                                                                                              (visita.IdPasoActual + 1), visita.IdPasoActual,
                                                                                              Constantes.EstatusPaso.EnRevisionEspera, , True)
                                        End Select
                                End Select
                        End Select
                    Case 8
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.SinReunionPresidencia ''Paso 8 SinReunionPresidencia
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            Me.Mensaje = "¿Habrá Sanción para la Visita?"
                                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionDeSancionPaso8();", True)
                                            Exit Sub
                                        End If
                                End Select
                        End Select
                    Case 9
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Elaborada  ''Paso 9 avanza paso 10, solicita fecha con vj
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP  'Solo el supervisor/superior puede:  *Notificar 
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_ReunionVjPaso9)
                                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaReunPreciJuridico();", True)
                                            Exit Sub
                                        End If
                                End Select
                            Case Constantes.EstatusPaso.AjustesRealizados
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP  'envia al paso 10
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            If Not ValidaComentariosObligatorios() Then Exit Sub

                                            txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_ReunionVjPaso9)
                                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaReunPreciJuridico();", True)
                                            Exit Sub
                                        End If
                                End Select
                        End Select

                    Case 10
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Una vez revisado supervisor rechaza o avanza
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                txbRangoInicio.Text = "$" & FormatNumber(visita.RANGO_SANCION_INI.ToString(), 0, , , TriState.UseDefault)
                                                txbRangoFin.Text = "$" & FormatNumber(visita.RANGO_SANCION_FIN.ToString(), 0, , , TriState.UseDefault)
                                                txtRango.Text = visita.COMENTARIO_RANGO_SANCION

                                                Me.Mensaje = Constantes.MensajesModal.SolicitudSancionPasoDiez
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "PreguntaPorSancionPasoDiez();", True)
                                                Exit Sub
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.AjustesRealizados
                                Select Case Usuario.IdentificadorPerfilActual ''Paso 10 AjustesRealizados
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Una vez revisado supervisor rechaza o avanza
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                txbRangoInicio.Text = "$" & FormatNumber(visita.RANGO_SANCION_INI.ToString(), 0, , , TriState.UseDefault)
                                                txbRangoFin.Text = "$" & FormatNumber(visita.RANGO_SANCION_FIN.ToString(), 0, , , TriState.UseDefault)
                                                txtRango.Text = visita.COMENTARIO_RANGO_SANCION

                                                Me.Mensaje = Constantes.MensajesModal.SolicitudSancionPasoDiez
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "PreguntaPorSancionPasoDiez();", True)
                                                Exit Sub
                                        End Select
                                End Select
                        End Select

                    Case 16
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado  ''Flujo inicial paso 16, revisado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_ReunionVjPaso16)
                                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaReunPreciJuridicoP16();", True)
                                            Exit Sub
                                        End If
                                End Select
                            Case Constantes.EstatusPaso.AjustesRealizados ''Flujo 2 paso 16, AjustesRealizados
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Vuelve a mandar a paso 17
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_ReunionVjPaso16)
                                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaReunPreciJuridicoP16();", True)
                                            Exit Sub
                                        End If
                                End Select
                        End Select

                    Case 17
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado ''Flujo inicial paso 17, Revisado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                             True, Constantes.EstatusPaso.Aprobado,
                                                             True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                             True, Constantes.CORREO_ID_NOTIFICA_APRUEBA_ACTA_CONCLUSION, True, True, , , , , , , , True)
                                                ''Si es supervisor solicitar confirmar fecha con VJ
                                                If Not visita.SolicitoFechaPaso16 Then
                                                    Mensaje = "La fecha de la reunión con el área de Supervisión fue: " & visita.Fecha_ReunionVjPaso16.ToString("dd/MM/yyyy") & "."
                                                    txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_ReunionVjPaso16)
                                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmarFechaReunPreciJuridicoP16SiNo();", True)
                                                    Exit Sub
                                                Else
                                                    If Not MostrarSolicitudRangoSancion() Then
                                                        Exit Sub
                                                    End If
                                                End If
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.AjustesRealizados ''Flujo 2 paso 17, AjustesRealizados
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesEnviados,
                                                                                              False, -1,
                                                                                              False, -1, -1,
                                                                                              True, Constantes.CORREO_ID_NOTIFICA_APRUEBA_ACTA_CONCLUSION,
                                                                                              True, True, False,
                                                                                              True, Constantes.TipoReactivacion.FinalizaReactivacion,
                                                                                              (visita.IdPasoActual + 1), visita.IdPasoActual,
                                                                                              Constantes.EstatusPaso.EnRevisionEspera, , True)
                                                ''Si es supervisor solicitar confirmar fecha con VJ
                                                If Not visita.SolicitoFechaPaso16 Then
                                                    Mensaje = "La fecha de la reunión con el área de Supervisión fue: " & visita.Fecha_ReunionVjPaso16.ToString("dd/MM/yyyy") & "."
                                                    txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_ReunionVjPaso16)
                                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmarFechaReunPreciJuridicoP16SiNo();", True)
                                                    Exit Sub
                                                Else
                                                    If Not MostrarSolicitudRangoSancion() Then
                                                        Exit Sub
                                                    End If
                                                End If
                                        End Select
                                End Select
                        End Select

                    Case 20
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado  ''Paso 20 pasa a paso 21, Revisado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                                              True, Constantes.EstatusPaso.Enviado,
                                                                                              True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                                              True, Constantes.CORREO_ID_NOTIFICA_VO_VF_REVISAR_OF_EMPLAZAMIENTO,
                                                                                              True, True, , , , , , , , True)
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.AjustesRealizados  ''Paso 20 pasa a paso 21, AjustesRealizados
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                                              True, Constantes.EstatusPaso.AjustesEnviados,
                                                                                              True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                                              True, Constantes.CORREO_ID_NOTIFICA_VO_VF_REVISAR_OF_EMPLAZAMIENTO,
                                                                                              True, True, , , , , , , , True)
                                        End Select
                                End Select
                        End Select

                    Case 21
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado ''Paso 21 guarda comentarios, Revisado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                 True, Constantes.EstatusPaso.Aprobado,
                                                                 True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                 True, Constantes.CORREO_ID_NOTIFICA_APRUEBA_OF_EMPLAZAMIENTO,
                                                                 True, True, , , , , , , , True)
                                        End If
                                End Select
                        End Select
                    Case 33
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Respuesta_Elaborada ''Paso 33, Respuesta_Elaborada
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                                 True, Constantes.EstatusPaso.Respuesta_Afore_Notificada,
                                                                                 True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                                 True, Constantes.CORREO_ID_NOTIFICA_VO_VF_REVISAR_DOCUMENTO_RESPUESTA_SOLICITUD_AFORE)

                                                ''Finaliza 34  e inicia 35, lo se lo se, esta mal pero que quieren que haga
                                                visita.IdPasoActual = (visita.IdPasoActual + 1)
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                                                 True, Constantes.EstatusPaso.Respuesta_Elaborada,
                                                                                 True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                                 False, -1)
                                        End Select

                                End Select
                            Case Constantes.EstatusPaso.AjustesRealizados ''Paso 33, AjustesRealizados
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesEnviados,
                                                                                            False, -1,
                                                                                            False, -1, -1,
                                                                                            True, Constantes.CORREO_ID_NOTIFICA_VO_VF_REVISAR_DOCUMENTO_RESPUESTA_SOLICITUD_AFORE,
                                                                                            True, True, False,
                                                                                            True, Constantes.TipoReactivacion.FinalizaReactivacion,
                                                                                            35, visita.IdPasoActual, Constantes.EstatusPaso.EnRevisionEspera)
                                        End Select

                                End Select
                        End Select

                    Case 35
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.Revisado  ''Paso 35, EnRevisionEspera, acepta, ''Paso 35, Revisado, acepta
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                          True, Constantes.EstatusPaso.Respuesta_Aprobada,
                                                          True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                          True, Constantes.CORREO_ID_NOTIFICA_VJ_APRUEBA_DOCUMENTO_RESPUESTA_SOLICITUD_AFORE)

                                            ''Si es supervisor solicitar confirmar fecha con VJ
                                            If Not visita.SolicitoFechaPaso32 Then
                                                Mensaje = "La fecha de la reunión con el área de Supervisión fue: " & visita.Fecha_ReunionVjPaso32.ToString("dd/MM/yyyy") & "."
                                                txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_ReunionVjPaso32)
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "ConfirmarFechaReunPaso32SiNo();", True)
                                                Exit Sub
                                            End If
                                        End If
                                End Select
                            Case Constantes.EstatusPaso.EnAjustes, Constantes.EstatusPaso.AjustesRealizados  ''Paso 35, EnAjustes, acepta, ''Paso 35, AjustesRealizados, acepta
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesEnviados,
                                                             False, -1,
                                                             False, -1, -1,
                                                             True, Constantes.CORREO_ID_NOTIFICA_VJ_APRUEBA_DOCUMENTO_RESPUESTA_SOLICITUD_AFORE,
                                                             True, True, False,
                                                             True, Constantes.TipoReactivacion.FinalizaReactivacion,
                                                             (visita.IdPasoActual + 1), visita.IdPasoActual, Constantes.EstatusPaso.EnRevisionEspera)

                                            ''Si es supervisor solicitar confirmar fecha con VJ
                                            If Not visita.SolicitoFechaPaso32 Then
                                                Mensaje = "La fecha de la reunión con el área de Supervisión fue: " & visita.Fecha_ReunionVjPaso32.ToString("dd/MM/yyyy") & "."
                                                txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_ReunionVjPaso32)
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "ConfirmarFechaReunPaso32SiNo();", True)
                                                Exit Sub
                                            End If
                                        End If
                                End Select
                        End Select
                End Select

                Response.Redirect("../Procesos/DetalleVisita.aspx")
            End If
        End If
    End Sub

    Protected Sub imgGuardarCambios_Click(sender As Object, e As ImageClickEventArgs) Handles imgGuardarCambios.Click
        If Not IsNothing(Session("DETALLE_VISITA")) And Not IsNothing(Entities.Usuario.SessionID) Then

            ObtenerSubVisitasSeleccionadas()

            Dim visita As New Visita()
            visita = CType(Session("DETALLE_VISITA"), Visita)

            Dim Usuario As New Entities.Usuario()
            Usuario = Session(Entities.Usuario.SessionID)

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 And Not IsNothing(Usuario) Then
                ''Valida si hay documentos obligatorios sin cargar
                ''No valida documentos obligatorios en paso 18
                If visita.IdPasoActual <> 18 Then
                    If HayDocumentosHobligatorios() Then Exit Sub
                End If

                ''Objeto negocio visita
                Dim objNegVisita As New NegocioVisita(visita, Usuario, Server, txbComentarios.Text)

                Select Case visita.IdPasoActual
                    Case 1
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnAjustes
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS ''Atiende observaciones VJ
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            'objNegVisita.PasoUnoSupervisorInspAtiendenAjustes()
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesRealizados,
                                                                                         False, -1,
                                                                                         False, -1, -1,
                                                                                         True, Constantes.CORREO_ATIENDE_AJUSTES,
                                                                                         True, False, False)
                                        End If
                                End Select
                        End Select
                    Case 2
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Enviado 'Administrador asigna supervisor y abogado asesor
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                Dim lsAboSup As String = ""
                                                Dim lsAboOp As String = GetUsuariosSeleccionados(Usuario.IdentificadorPerfilActual, Constantes.ABOGADOS.PERFIL_ABO_ASESOR, lsAboSup)

                                                If lsAboOp = "" And lsAboSup = "" Then
                                                    Exit Sub
                                                Else
                                                    If AccesoBD.ActualizaResponsablesVisita(visita.IdVisitaGenerado, lsAboSup, Constantes.ResponsablesVisita.AbogadoSupAsesor, Constantes.OPERCION.Insertar) Then
                                                        If AccesoBD.ActualizaResponsablesVisita(visita.IdVisitaGenerado, lsAboOp, Constantes.ResponsablesVisita.Asesor, Constantes.OPERCION.Insertar) Then
                                                            ''AsesorAsignado
                                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.SupervisorAsignado,
                                                                                             False, -1,
                                                                                             False, -1, -1,
                                                                                             True, Constantes.CORREO_NOTIFICA_ABOGADO_ASESOR,
                                                                                             True, True, False, , , , , , , True)
                                                        End If
                                                    End If
                                                End If
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.SupervisorAsignado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Supervisor asigna/modifica abogado asesor
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                Dim lsAboSup As String = ""
                                                Dim lsAboOp As String = GetUsuariosSeleccionados(Usuario.IdentificadorPerfilActual, Constantes.ABOGADOS.PERFIL_ABO_ASESOR, lsAboSup)

                                                If lsAboOp = "" Then
                                                    Exit Sub
                                                Else
                                                    If AccesoBD.ActualizaResponsablesVisita(visita.IdVisitaGenerado, lsAboOp, Constantes.ResponsablesVisita.Asesor, IIf(visita.LstAbogadosAsesorAsignados.Count > 0, Constantes.OPERCION.Actualizar, Constantes.OPERCION.Insertar)) Then
                                                        ''AsesorAsignado
                                                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AsesorAsignado,
                                                                                          False, -1,
                                                                                          False, -1, -1,
                                                                                          True, Constantes.CORREO_NOTIFICA_ABOGADO_ASESOR,
                                                                                          True, True, False, , , , , , , True)
                                                    End If
                                                End If
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.AsesorAsignado ''Guarda despues de asignar asesor
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.ABOGADOS.PERFIL_ABO_ASESOR, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Revisado,
                                                                                            False, -1,
                                                                                            False, -1, -1,
                                                                                            True, Constantes.CORREO_REVISION_DOCUMENTOS,
                                                                                            False, True, False)
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.EnAjustes  ''Guarda paso 2, EnAjustes
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.ABOGADOS.PERFIL_ABO_ASESOR, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesRealizados,
                                                                                            False, -1,
                                                                                            False, -1, -1,
                                                                                            True, Constantes.CORREO_REVISION_DOCUMENTOS,
                                                                                            False, True, False)
                                        End Select
                                End Select
                        End Select

                    Case 3
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 3 guardar, en revision espera
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            'objNegVisita.PasoTresSupervisorInspRevisaComentariosVj()
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Revisado,
                                                                                         False, -1,
                                                                                         False, -1, -1,
                                                                                         False, -1)
                                        End If
                                End Select
                        End Select
                    Case 4
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 4, EnRevisionEspera
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Revisado,
                                                                                         False, -1,
                                                                                         False, -1, -1,
                                                                                         False, -1)
                                        End If
                                End Select
                        End Select

                    Case 8
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 8, EnRevisionEspera
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            'objNegVisita.PasoGenericoGuardaInspectorComentarios(Constantes.EstatusPaso.HallazgosGuardados)
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.HallazgosGuardados,
                                                                                         False, -1,
                                                                                         False, -1, -1,
                                                                                         False, -1)
                                        End If
                                End Select
                        End Select

                    Case 9
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP 'Solo el inspector puede:  *adjuntar documentos (acta circunstnaciada) 
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            ''objNegVisita.PasoGenericoGuardaInspectorComentarios(Constantes.EstatusPaso.Elaborada)
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Elaborada,
                                                                                         False, -1,
                                                                                         False, -1, -1,
                                                                                         True, Constantes.CORREO_ID_NOTIFICA_SUPERVISOR_ACTA_CIRCUNSTANCIADA_MODIFICADA,
                                                                                         True, False, False, , , , , , , True)
                                        End If
                                End Select
                            Case Constantes.EstatusPaso.EnAjustes
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP 'adjuntar los documentos
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            ''hace la revision de los documentos
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesRealizados, False, -1, False, -1, -1,
                                                                                         True, Constantes.CORREO_ID_NOTIFICA_SUPERVISOR_ACTA_CIRCUNSTANCIADA_MODIFICADA,
                                                                                         True, False, False, , , , , , , True)
                                        End If
                                End Select
                        End Select

                    Case 10

                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 10 EnRevisionEspera
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                ''Guarda comentarios, el paso 9 pasa al estatus revisado y reporta a Sup de VJ  que finalizo la revicion.
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Revisado, False, -1, False, -1, -1,
                                                                                              True, Constantes.CORREO_INSPECTOR_REVISA_DOCUMENTOS,
                                                                                              False, True, False, , , , , , , True)
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.EnAjustes ''Paso 10 EnRevisionEspera, EnAjustes
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                ''Guarda comentarios, el paso 9 pasa al estatus revisado y reporta a Sup de VJ  que finalizo la revicion.
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesRealizados,
                                                                                            False, -1, False, -1, -1,
                                                                                            True, Constantes.CORREO_INSPECTOR_REVISA_DOCUMENTOS,
                                                                                            False, True, False, , , , , , , True)
                                        End Select
                                End Select
                        End Select

                    Case 13
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS ''adjunta documentos del paso 13
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            ''Guarda comentarios, no cambia de paso y pasa al estatus de allazgos presentados
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Hallazgos_presentados,
                                                                                           False, -1, False, -1, -1, False, -1)
                                        End If
                                End Select
                        End Select

                    Case 14
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Registrado,
                                                          False, -1,
                                                          False, -1, -1,
                                                          False, -1)
                                        End If
                                End Select
                        End Select

                    Case 16
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Flujo inicial paso 16
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Revisado,
                                                                     False, -1, False, -1, -1,
                                                                     True, Constantes.CORREO_NOTIFICA_SUPERVISOR_PASO16,
                                                                     True, False, False, , , , , , , True)
                                        End If
                                End Select
                            Case Constantes.EstatusPaso.EnAjustes ''Flujo 2 paso 16, EnAjustes
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesRealizados,
                                                                     False, -1, False, -1, -1,
                                                                     False, -1)
                                        End If
                                End Select
                        End Select

                    Case 17
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Flujo inicial paso 17, EnRevisionEspera
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_ASESOR, Constantes.PERFIL_INS
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Revisado,
                                                                        False, -1, False, -1, -1,
                                                                        False, -1)
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.EnAjustes ''Flujo 2 paso 17, EnAjustes
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_ASESOR, Constantes.PERFIL_INS
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesRealizados,
                                                                        False, -1, False, -1, -1,
                                                                        False, -1)
                                        End Select
                                End Select

                            Case Constantes.EstatusPaso.Revisado, Constantes.EstatusPaso.AjustesRealizados
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                        False, -1, False, -1, -1,
                                                                        False, -1)
                                        End Select
                                End Select
                        End Select

                    Case 18
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Flujo inicial paso 18, EnRevisionEspera
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Revisado,
                                                                     False, -1, False, -1, -1,
                                                                     False, -1)
                                        End If
                                End Select
                        End Select

                    Case 19
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 19 guardar comentarios
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Revisado,
                                                                     False, -1, False, -1, -1,
                                                                     False, -1)
                                        End If
                                End Select
                        End Select

                    Case 20
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera 'Tiene que asignar el abogado que asesorara en la sancion
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                Dim lsAboSup As String = ""
                                                Dim lsAboOp As String = GetUsuariosSeleccionados(Usuario.IdentificadorPerfilActual, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES, lsAboSup)

                                                If lsAboOp = "" And lsAboSup = "" Then
                                                    Exit Sub
                                                Else
                                                    If AccesoBD.ActualizaResponsablesVisita(visita.IdVisitaGenerado, lsAboSup, Constantes.ResponsablesVisita.AbogadoSupSancion, Constantes.OPERCION.Insertar) Then
                                                        If AccesoBD.ActualizaResponsablesVisita(visita.IdVisitaGenerado, lsAboOp, Constantes.ResponsablesVisita.Sanciones, Constantes.OPERCION.Insertar) Then
                                                            ''Asignado
                                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.SupervisorAsignado,
                                                                                             False, -1,
                                                                                             False, -1, -1,
                                                                                             True, Constantes.CORREO_NOTIFICA_ABOGADO_SANCIONES,
                                                                                             True, True, False, , , , , , , True)
                                                        End If
                                                    End If
                                                End If
                                        End Select
                                    Case Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                        False, -1, False, -1, -1,
                                                                        False, -1)
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.SupervisorAsignado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Supervisor asigna/modifica abogado sancion
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                Dim lsAboSup As String = ""
                                                Dim lsAboOp As String = GetUsuariosSeleccionados(Usuario.IdentificadorPerfilActual, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES, lsAboSup)

                                                If lsAboOp = "" Then
                                                    Exit Sub
                                                Else
                                                    If AccesoBD.ActualizaResponsablesVisita(visita.IdVisitaGenerado, lsAboOp, Constantes.ResponsablesVisita.Sanciones, IIf(visita.LstAbogadosSancionAsignados.Count > 0, Constantes.OPERCION.Actualizar, Constantes.OPERCION.Insertar)) Then
                                                        ''AsesorAsignado
                                                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Asignado,
                                                                                          False, -1,
                                                                                          False, -1, -1,
                                                                                          True, Constantes.CORREO_NOTIFICA_ABOGADO_SANCIONES,
                                                                                          True, True, False, , , , , , , True)
                                                    End If
                                                End If
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.Asignado  ''Paso 20 pasa a paso 21, Asignado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Revisado,
                                                                        False, -1, False, -1, -1,
                                                                        False, -1)
                                        End Select
                                End Select

                            Case Constantes.EstatusPaso.EnAjustes  ''Regresa de paso 21, EnAjustes
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesRealizados,
                                                                        False, -1, False, -1, -1,
                                                                        False, -1)
                                        End Select
                                End Select
                        End Select

                    Case 21
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 21 guarda comentarios, EnRevisionEspera
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Revisado,
                                                                    False, -1, False, -1, -1,
                                                                    False, -1)
                                        End If
                                End Select
                            Case Constantes.EstatusPaso.Revisado  ''Paso 21 guarda comentarios, Revisado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                    False, -1, False, -1, -1,
                                                                    False, -1)
                                        End If
                                End Select
                        End Select
                    Case 23
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 23
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                'Solicita fecha del acuse  en que la afore recibe el oficio de emplazamiento
                                                btnAceptarM2B1A.CommandArgument = "ingresarFechaAcuseAforeRecibeOfEmpl"
                                                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaAcuseAforeRecibeOfEmpl();", True)
                                                Exit Sub
                                        End Select
                                End Select
                        End Select

                    Case 24
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 24
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                'Solicita fecha  del acuse en que la AFORE contestó al oficio de emplazamiento
                                                btnAceptarM2B1A.CommandArgument = "ingresarFechaAcuseAforeContestoOfEmpl"
                                                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning
                                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaAcuseAforeContestoOfEmpl();", True)
                                                Exit Sub
                                        End Select
                                End Select
                        End Select

                    Case 26
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 26, Elaborada
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                                        If EsAreaOperativa(Usuario.IdArea) Or Usuario.IdArea = Constantes.AREA_VJ Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                              True, Constantes.EstatusPaso.Elaborada,
                                                                              True, (visita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                              True, Constantes.CORREO_ULTIMA_VERSION_OF_SANCION,
                                                                              True, True, False, , , , , , , True)
                                        End If
                                End Select
                        End Select
                    Case 29
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 29, Revisado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Revisado,
                                                                                             False, -1,
                                                                                             False, -1, -1,
                                                                                             False, -1)
                                        End Select
                                End Select
                        End Select
                    Case 30
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 30
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                Dim lsAboSup As String = ""
                                                Dim lsAboOp As String = GetUsuariosSeleccionados(Usuario.IdentificadorPerfilActual, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO, lsAboSup)

                                                If lsAboOp = "" And lsAboSup = "" Then
                                                    Exit Sub
                                                Else
                                                    If AccesoBD.ActualizaResponsablesVisita(visita.IdVisitaGenerado, lsAboSup, Constantes.ResponsablesVisita.AbogadoSupContecioso, Constantes.OPERCION.Insertar) Then
                                                        If AccesoBD.ActualizaResponsablesVisita(visita.IdVisitaGenerado, lsAboOp, Constantes.ResponsablesVisita.Contencioso, Constantes.OPERCION.Insertar) Then
                                                            ''Asignado
                                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.SupervisorAsignado,
                                                                                             False, -1,
                                                                                             False, -1, -1,
                                                                                             True, Constantes.CORREO_NOTIFICA_ABOGADO_CONTENCIOSO,
                                                                                             True, True, False, , , , , , , True)
                                                        End If
                                                    End If
                                                End If
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.SupervisorAsignado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Supervisor asigna/modifica abogado contencioso
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                Dim lsAboSup As String = ""
                                                Dim lsAboOp As String = GetUsuariosSeleccionados(Usuario.IdentificadorPerfilActual, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO, lsAboSup)

                                                If lsAboOp = "" Then
                                                    Exit Sub
                                                Else
                                                    If AccesoBD.ActualizaResponsablesVisita(visita.IdVisitaGenerado, lsAboOp, Constantes.ResponsablesVisita.Contencioso, IIf(visita.LstAbogadosContenAsignados.Count > 0, Constantes.OPERCION.Actualizar, Constantes.OPERCION.Insertar)) Then
                                                        ''AsesorAsignado
                                                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Asignado,
                                                                                          False, -1,
                                                                                          False, -1, -1,
                                                                                          True, Constantes.CORREO_NOTIFICA_ABOGADO_CONTENCIOSO,
                                                                                          True, True, False, , , , , , , True)
                                                    End If
                                                End If
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.Asignado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                If rdbRecurosRevocacion.Checked Then
                                                    txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_ReunionVjPaso32)
                                                    btnFechaPaso32.CommandArgument = Constantes.EstatusPaso.Revocacion
                                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.FechaPaso32 & "', 'btnFechaPaso32');", True)
                                                    Exit Sub
                                                Else
                                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                   True, Constantes.EstatusPaso.Sin_respuesta,
                                                   True, 37, Constantes.EstatusPaso.EnRevisionEspera,
                                                   True, Constantes.CORREO_ID_NOTIFICA_VO_VF_PR_AFORE_SOLICITA_NINGUNO_DE_LOS_DOS, True, True, True, , , , , , , True)
                                                End If
                                        End Select
                                End Select
                        End Select
                    Case 31
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 31
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                If rdbJuicioNulidad.Checked Then
                                                    txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_ReunionVjPaso32)
                                                    btnFechaPaso32.CommandArgument = Constantes.EstatusPaso.Nulidad
                                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.FechaPaso32 & "', 'btnFechaPaso32');", True)
                                                    Exit Sub
                                                Else ''PASA EN AUTOMATICO AL 37
                                                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                                True, Constantes.EstatusPaso.Sin_respuesta,
                                                                                True, 37, Constantes.EstatusPaso.EnRevisionEspera,
                                                                                True, Constantes.CORREO_ID_NOTIFICA_VO_VF_PR_AFORE_SOLICITA_NINGUNO_DE_LOS_DOS, True, True, True, , , , , , , True)
                                                End If
                                        End Select
                                End Select
                        End Select

                    Case 33
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 33, EnRevisionEspera
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Respuesta_Elaborada,
                                                                                             False, -1,
                                                                                             False, -1, -1,
                                                                                             False, -1)
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.EnAjustes ''Paso 33, EnAjustes
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesRealizados,
                                                                                             False, -1,
                                                                                             False, -1, -1,
                                                                                             False, -1)
                                        End Select
                                End Select
                        End Select

                    Case 35
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 35, EnRevisionEspera
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Revisado,
                                                                                          False, -1,
                                                                                          False, -1, -1,
                                                                                          True, Constantes.CORREO_ID_NOTIFICA_VJ_AJUSTA_DOCUMENTO_RESPUESTA_SOLICITUD_AFORE)
                                        End If
                                End Select
                            Case Constantes.EstatusPaso.EnAjustes  ''Paso 35, EnAjustes
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesRealizados,
                                                                                          False, -1,
                                                                                          False, -1, -1,
                                                                                          True, Constantes.CORREO_ID_NOTIFICA_VJ_AJUSTA_DOCUMENTO_RESPUESTA_SOLICITUD_AFORE)
                                        End If
                                End Select
                        End Select

                    Case 36
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 36, EnRevisionEspera
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Revisado,
                                                                                         False, -1,
                                                                                         False, -1, -1,
                                                                                         False, -1)
                                        End Select
                                End Select
                        End Select
                End Select

                Response.Redirect("../Procesos/DetalleVisita.aspx")
            End If
        End If

    End Sub

    Protected Sub imgRevisarDocs_Click(sender As Object, e As ImageClickEventArgs) Handles imgRevisarDocs.Click
        Response.Redirect("../Procesos/DetalleVisita.aspx#tab2")
    End Sub

    Protected Sub imgInicio_Click(sender As Object, e As ImageClickEventArgs) Handles imgInicio.Click
        LiberaVisita()

        Response.Redirect("../Visita/Bandeja.aspx")
    End Sub

    Protected Sub imgDetener_Click(sender As Object, e As ImageClickEventArgs) Handles imgDetener.Click

        If Not IsNothing(Session("DETALLE_VISITA")) And Not IsNothing(Entities.Usuario.SessionID) Then

            ObtenerSubVisitasSeleccionadas()

            Dim visita As New Visita()
            visita = CType(Session("DETALLE_VISITA"), Visita)

            Dim Usuario As New Entities.Usuario()
            Usuario = Session(Entities.Usuario.SessionID)

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 And Not IsNothing(Usuario) Then

                Select Case visita.IdPasoActual

                    Case 6
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Visita_iniciada ''Paso 6 detener visita
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            btnAceptarM2B1A.CommandArgument = "ingresarFechaFinVisita"
                                            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning

                                            If Fechas.Vacia(visita.FECH_VISITA_CAMPO__FIN) Then
                                                txbFechFinVisita.Text = DateTime.Today.ToString("dd/MM/yyyy")
                                            Else
                                                txbFechFinVisita.Text = visita.FECH_VISITA_CAMPO__FIN.ToString("dd/MM/yyyy")
                                            End If

                                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionSolicitarFechaFinVisita();", True)
                                            Exit Sub
                                        End If
                                End Select
                        End Select
                End Select
            End If

        End If

    End Sub

    Protected Sub imgAnterior_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnterior.Click
        If Not IsNothing(Session("DETALLE_VISITA")) And Not IsNothing(Entities.Usuario.SessionID) Then

            ObtenerSubVisitasSeleccionadas()

            Dim visita As New Visita()
            visita = CType(Session("DETALLE_VISITA"), Visita)

            Dim Usuario As New Entities.Usuario()
            Usuario = Session(Entities.Usuario.SessionID)

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 And Not IsNothing(Usuario) Then
                ''Objeto negocio visita
                Dim objNegVisita As New NegocioVisita(visita, Usuario, Server, txbComentarios.Text)

                Select Case visita.IdPasoActual
                    Case 2
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado, Constantes.EstatusPaso.AsesorAsignado ''revisaso y asesor asignado paso 2
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Regresa a paso 1
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                If Not ValidaComentariosObligatorios() Then Exit Sub
                                                'objNegVisita.PasoDosSupervisorRegresaPasoUno()
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                              False, -1,
                                                                                              False, -1, -1,
                                                                                              True, Constantes.CORREO_ID_NOTIFICA_AREA_REGISTRO_VISITA_RECHAZADO,
                                                                                              True, True, False,
                                                                                              True, Constantes.TipoReactivacion.Reactivado,
                                                                                              visita.IdPasoActual, (visita.IdPasoActual - 1),
                                                                                              Constantes.EstatusPaso.EnAjustes, , True)
                                                Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.AjustesRealizados, Constantes.EstatusPaso.EnAjustes  ''Guarda paso 2, AjustesRealizados
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                If Not ValidaComentariosObligatorios() Then Exit Sub
                                                ''SE HACE EN EL STORED DE INSERTAR ESTATUS PASO YA QUE NO ESTA BIEN QUE CIERRE LA REACTIVACION SI SE RECHAZA MAS DE UN PASO                                                
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                              False, -1,
                                                                                              False, -1, -1,
                                                                                              True, Constantes.CORREO_ID_NOTIFICA_AREA_REGISTRO_VISITA_RECHAZADO,
                                                                                              True, True, False,
                                                                                              True, Constantes.TipoReactivacion.Reactivado,
                                                                                              visita.IdPasoActual, (visita.IdPasoActual - 1),
                                                                                              Constantes.EstatusPaso.EnAjustes, , True)
                                                Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End Select
                                End Select
                        End Select

                    Case 3 '' Observaciones obligatorias
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado ''Rechaza los documentos que aprobo VJ paso 3
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            If Not ValidaComentariosObligatorios() Then Exit Sub
                                            'objNegVisita.PasoTresSupRechazaDocsMandaPasoDos()
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                         False, -1,
                                                                                         False, -1, -1,
                                                                                         True, Constantes.CORREO_RECHAZO_PASO_DOS,
                                                                                         True, True, False,
                                                                                         True, Constantes.TipoReactivacion.Reactivado,
                                                                                         visita.IdPasoActual, (visita.IdPasoActual - 1),
                                                                                         Constantes.EstatusPaso.EnAjustes, , True)
                                            Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End If
                                End Select
                        End Select

                    Case 10
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Una vez revisado supervisor rechaza o avanza
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                If Not ValidaComentariosObligatorios() Then Exit Sub
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                              False, -1,
                                                                                              False, -1, -1,
                                                                                              True, Constantes.CORREO_ID_NOTIFICA_RECHAZA_ACTA_CIRCUNSTANCIADA,
                                                                                              True, True, False,
                                                                                              True, Constantes.TipoReactivacion.Reactivado,
                                                                                              visita.IdPasoActual, (visita.IdPasoActual - 1),
                                                                                              Constantes.EstatusPaso.EnAjustes, , True)
                                                Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.AjustesRealizados
                                Select Case Usuario.IdentificadorPerfilActual ''Paso 10 AjustesRealizados
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Una vez revisado supervisor rechaza o avanza
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                If Not ValidaComentariosObligatorios() Then Exit Sub

                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                              False, -1,
                                                                                              False, -1, -1,
                                                                                              True, Constantes.CORREO_ID_NOTIFICA_RECHAZA_ACTA_CIRCUNSTANCIADA,
                                                                                              True, True, False,
                                                                                              True, Constantes.TipoReactivacion.Reactivado,
                                                                                              visita.IdPasoActual, (visita.IdPasoActual - 1),
                                                                                              Constantes.EstatusPaso.EnAjustes, , True)
                                                Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End Select
                                End Select
                        End Select

                    Case 12
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 12 rechaza
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP  'Supervisor avisa de la version final de los documentos
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            If Not ValidaComentariosObligatorios() Then Exit Sub
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                           False, -1,
                                                                                           False, -1, -1,
                                                                                           True, Constantes.CORREO_PASO_12_RECHAZA_VERSION_FIN_DOC,
                                                                                           True, True, False,
                                                                                           True, Constantes.TipoReactivacion.Reactivado,
                                                                                           visita.IdPasoActual, 10,
                                                                                           Constantes.EstatusPaso.EnAjustes, , True)
                                            Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End If
                                End Select
                        End Select

                    Case 17
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado ''Flujo inicial paso 17, Revisado, regresa paso 16
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                If Not ValidaComentariosObligatorios() Then Exit Sub
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                              False, -1,
                                                                                              False, -1, -1,
                                                                                              True, Constantes.CORREO_RECHAZO_PASO_17,
                                                                                              True, False, False,
                                                                                              True, Constantes.TipoReactivacion.Reactivado,
                                                                                              visita.IdPasoActual, (visita.IdPasoActual - 1),
                                                                                              Constantes.EstatusPaso.EnAjustes)
                                                Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End Select
                                End Select
                            Case Constantes.EstatusPaso.AjustesRealizados  ''Flujo 2 paso 17 regresa paso 16, AjustesRealizados
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                If Not ValidaComentariosObligatorios() Then Exit Sub

                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                              False, -1,
                                                                                              False, -1, -1,
                                                                                              True, Constantes.CORREO_RECHAZO_PASO_17,
                                                                                              True, False, False,
                                                                                              True, Constantes.TipoReactivacion.Reactivado,
                                                                                              visita.IdPasoActual, (visita.IdPasoActual - 1),
                                                                                              Constantes.EstatusPaso.EnAjustes)
                                                Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End Select
                                End Select
                        End Select
                    Case 18
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado ''Flujo inicial paso 18, Revisado, regresa paso 17
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            If Not ValidaComentariosObligatorios() Then Exit Sub
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                           False, -1,
                                                                                           False, -1, -1,
                                                                                           True, Constantes.CORREO_RECHAZO_COMENTARIOS,
                                                                                           True, True, False,
                                                                                           True, Constantes.TipoReactivacion.Reactivado,
                                                                                           visita.IdPasoActual, (visita.IdPasoActual - 1),
                                                                                           Constantes.EstatusPaso.EnAjustes, , True)
                                            Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End If
                                End Select
                        End Select
                    Case 21
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado ''Paso 21 guarda comentarios, Revisado, rechaza
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            If Not ValidaComentariosObligatorios() Then Exit Sub
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                           False, -1,
                                                                                           False, -1, -1,
                                                                                           True, Constantes.CORREO_ID_NOTIFICA_RECHAZA_OF_EMPLAZAMIENTO,
                                                                                           False, True, False,
                                                                                           True, Constantes.TipoReactivacion.Reactivado,
                                                                                           visita.IdPasoActual, (visita.IdPasoActual - 1),
                                                                                           Constantes.EstatusPaso.EnAjustes)
                                            Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End If
                                End Select
                        End Select

                    Case 35
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.Revisado  ''Paso 35, EnRevisionEspera, rechaza, ''Paso 35, Revisado, rechaza
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            If Not ValidaComentariosObligatorios() Then Exit Sub
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                         False, -1,
                                                                                         False, -1, -1,
                                                                                         True, Constantes.CORREO_ID_NOTIFICA_VJ_RECHAZA_DOCUMENTO_RESPUESTA_SOLICITUD_AFORE,
                                                                                         True, True, False,
                                                                                         True, Constantes.TipoReactivacion.Reactivado,
                                                                                         visita.IdPasoActual, 33,
                                                                                         Constantes.EstatusPaso.EnAjustes)
                                            Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End If
                                End Select
                            Case Constantes.EstatusPaso.EnAjustes, Constantes.EstatusPaso.AjustesRealizados  ''Paso 35, EnAjustes, rechaza, ''Paso 35, AjustesRealizados, rechaza
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        If EsAreaOperativa(Usuario.IdArea) Then
                                            If Not ValidaComentariosObligatorios() Then Exit Sub
                                            ''COMO VIENE DE UN RECHAZO Y SE VUELVE A RECHAZAR, SE HACE EN EL STORED DE INSERTAR ESTATUS PASO YA QUE NO ESTA BIEN QUE CIERRE LA REACTIVACION SI SE RECHAZA MAS DE UN PASO                                                
                                            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                         False, -1,
                                                                                         False, -1, -1,
                                                                                         True, Constantes.CORREO_ID_NOTIFICA_VJ_RECHAZA_DOCUMENTO_RESPUESTA_SOLICITUD_AFORE,
                                                                                         True, True, False,
                                                                                         True, Constantes.TipoReactivacion.Reactivado,
                                                                                         visita.IdPasoActual, 33,
                                                                                         Constantes.EstatusPaso.EnAjustes)
                                            Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End If
                                End Select
                        End Select
                    Case 36
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.Revisado ''Paso 36, Revisado, rechaza
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                                        Select Case Usuario.IdArea
                                            Case Constantes.AREA_PR, Constantes.AREA_VJ
                                                If Not ValidaComentariosObligatorios() Then Exit Sub
                                                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnAjustes,
                                                                                        False, -1,
                                                                                        False, -1, -1,
                                                                                        True, Constantes.CORREO_RECHAZO_PASO_36,
                                                                                        True, True, False,
                                                                                        True, Constantes.TipoReactivacion.Reactivado,
                                                                                        visita.IdPasoActual, (visita.IdPasoActual - 1),
                                                                                        Constantes.EstatusPaso.EnAjustes)
                                                Response.Redirect("../Procesos/DetalleVisita.aspx")
                                        End Select
                                End Select
                        End Select
                End Select
            End If
        End If
    End Sub


    ''' <summary>
    ''' Valida que el usuario haya ingresado comentarios en caso de ser obligatorios por el paso de la visita
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidaComentariosObligatorios() As Boolean
        If txbComentarios.Text.Trim().Length < 1 Then
            lblMensajeComentarioObligatorio.Visible = True
            Return False
        End If
        lblMensajeComentarioObligatorio.Visible = False
        Return True
    End Function

    ''' <summary>
    ''' Evento de el boton iniciar visita
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub imgIniciarVisita_Click(sender As Object, e As ImageClickEventArgs) Handles imgIniciarVisita.Click
        If Not IsNothing(Session("DETALLE_VISITA")) And Not IsNothing(Entities.Usuario.SessionID) Then

            ObtenerSubVisitasSeleccionadas()

            ''Valida si hay documentos obligatorios sin cargar
            If HayDocumentosHobligatorios() Then Exit Sub

            Dim visita As New Visita()
            visita = CType(Session("DETALLE_VISITA"), Visita)

            Dim Usuario As New Entities.Usuario()
            Usuario = Session(Entities.Usuario.SessionID)

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 And Not IsNothing(Usuario) Then
                ''Objeto negocio visita
                Dim objNegVisita As New NegocioVisita(visita, Usuario, Server, txbComentarios.Text)

                Select Case visita.IdPasoActual

                    Case 6
                        Select Case visita.IdEstatusActual
                            Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 6 inicia visita
                                Select Case Usuario.IdentificadorPerfilActual
                                    Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                                        If EsAreaOperativa(Usuario.IdArea) Or Usuario.IdArea = Constantes.AREA_VJ Then
                                            txtFechaCampo.Text = Fechas.Valor(visita.FECH_VISITA_CAMPO__INI)
                                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaCampo();", True)
                                            Exit Sub
                                        End If
                                End Select
                        End Select
                End Select
            End If
        End If
    End Sub

    ''' <summary>
    ''' No se valida la fecha inicial del paso 6 ya que esta fecha se considera como la fecha inicial del paso 6
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFechaCampo_Click(sender As Object, e As EventArgs) ''Paso 6 inicia visita
        ''Validar que este llena y sea correcta la fecha de campo
        Dim lsFecha As String = txtFechaCampo.Text.Trim()
        Dim ldFechaCampo As Date

        ''Regresa DateTime.MinValue si alguna validacion falla
        ldFechaCampo = ValidarFechaGeneral(lsFecha, lblFechaCampo, Image17, "SolicitarFechaCampo();", False)
        If ldFechaCampo = DateTime.MinValue Then
            Exit Sub
        End If

        Dim visita As New Visita()
        visita = CType(Session("DETALLE_VISITA"), Visita)

        Dim Usuario As New Entities.Usuario()
        Usuario = Session(Entities.Usuario.SessionID)

        If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 And Not IsNothing(Usuario) Then
            Dim objNegVisita As New NegocioVisita(visita, Usuario, Server, txbComentarios.Text)

            ''Iniciar la visita
            ''objNegVisita.PasoSeisIniciarVisitaCampo(ldFechaCampo)

            If AccesoBD.ActualizaFechaInicioVisita(visita.IdVisitaGenerado, ldFechaCampo, Constantes.TipoFecha.FechaCampoInicial, ppObjVisita.SubVisitasSeleccionadas) Then
                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Visita_iniciada,
                                                                    False, -1,
                                                                    False, -1, -1,
                                                                    True, Constantes.CORREO_ID_NOTIFICA_ABOGADO_Y_PRESIDENCIA_VISITA_INICIA,
                                                                    True, True, True, , , , , , , True)
            End If

            Response.Redirect("../Procesos/DetalleVisita.aspx")
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
    ''' Guarda la fecha para la reunion con presidencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFechaReunion_Click(sender As Object, e As EventArgs) ''Paso 8 HallazgosGuardados
        ''Validar que este llena y sea correcta la fecha de campo
        Dim lsFecha As String = txtFechaReunion.Text.Trim()
        Dim ldFechaReunion As Date

        ''Regresa DateTime.MinValue si alguna validacion falla
        ldFechaReunion = ValidarFechaGeneral(lsFecha, lblFechaReunion, Image15, "SolicitarFechaReunionPresidencia();", True, True)
        If ldFechaReunion = DateTime.MinValue Then
            Exit Sub
        End If

        Dim visita As New Visita()
        visita = CType(Session("DETALLE_VISITA"), Visita)

        Dim Usuario As New Entities.Usuario()
        Usuario = Session(Entities.Usuario.SessionID)

        If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 And Not IsNothing(Usuario) Then
            Dim objNegVisita As New NegocioVisita(visita, Usuario, Server, txbComentarios.Text)

            ''Iniciar la visita
            If piBanderaModificacio = 0 Then
                ''objNegVisita.PasoOchoSupervisorNotificaPresentacionHallazgos(ldFechaReunion)
                visita.FechaReunionPresidencia = ldFechaReunion
                If AccesoBD.ActualizaFechaInicioVisita(visita.IdVisitaGenerado, ldFechaReunion, Constantes.TipoFecha.FechaReunionPresi, ppObjVisita.SubVisitasSeleccionadas) Then
                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.EnEsperaPresentarHallazgos,
                                                                       False, -1,
                                                                       False, -1, -1,
                                                                       True, Constantes.CORREO_PRESENTA_DIAG_HALLAZGOS,
                                                                       True, True, True, , , , , , , True)
                End If

                ''Preguntar si hubo sancion o no
                ''Si hubo, entonces no hacer nada, ya que todas las visitas estan marcadas inicialmente como que si hay sancion, seria solo redirigir a la misma pagina.
                ''Si no, entonces marcar la visita con no sancion pasar a paso 13 y de este al 16 y de ahi se termina en el paso 18 todo el flujo
                Me.Mensaje = "¿Existe sanción para la visita?"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionDetalleVisita();", True)
                Exit Sub
            Else '' si no es 0 es que se esta modificando
                Dim objCorreo As New Entities.Correo(Constantes.CORREO_CAMBIO_FECHA_EXTERNA_INTERNA)

                objCorreo.Asunto = objCorreo.Asunto.Replace("[TIPO_FECHA]", "interna")
                objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[FECHA_REUNION_ANT]", visita.FECH_REUNION__PRESIDENCIA.ToString("dd/MM/yyyy")).
                                                Replace("[FECHA_REUNION_ACTUAL]", ldFechaReunion.ToString("dd/MM/yyyy")).
                                                Replace("[TIPO_FECHA]", "interna")

                visita.FechaReunionPresidencia = ldFechaReunion
                If AccesoBD.ActualizaFechaInicioVisita(visita.IdVisitaGenerado, ldFechaReunion, Constantes.TipoFecha.FechaReunionPresi, ppObjVisita.SubVisitasSeleccionadas) Then
                    objNegVisita.getObjNotificacion().NotificarCorreo(objCorreo, visita, True, True, True, , True)
                End If

                Response.Redirect("../Procesos/DetalleVisita.aspx")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Guarda el rango se sancion, inicia paso 10 y finaliza paso 11, asi como suena, ya que paso 11 ni deberia de existir. :)..
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnRangoSancion_Click(sender As Object, e As EventArgs)

        If txbRangoInicio.Text.Trim() = String.Empty Or txbRangoFin.Text.Trim() = String.Empty Then
            lblMensajeRangoSancionObligatorio.Text = New Entities.EtiquetaError(2146).Descripcion
            lblMensajeRangoSancionObligatorio.Visible = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarRangoSancion();", True)
            Exit Sub
        Else
            lblMensajeRangoSancionObligatorio.Visible = False
            Dim visita As New Visita()
            visita = CType(Session("DETALLE_VISITA"), Visita)

            Dim Usuario As New Entities.Usuario()
            Usuario = Session(Entities.Usuario.SessionID)

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 And Not IsNothing(Usuario) Then
                Dim objNegVisita As New NegocioVisita(visita, Usuario, Server, txbComentarios.Text)

                Dim rango1 As Decimal
                Dim rango2 As Decimal

                If Not Decimal.TryParse(txbRangoInicio.Text.Trim().Replace("$", "").Replace(",", ""), rango1) Then
                    lblMensajeRangoSancionObligatorio.Text = New Entities.EtiquetaError(2145).Descripcion
                    lblMensajeRangoSancionObligatorio.Visible = True
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarRangoSancion();", True)
                    Exit Sub
                End If

                If Not Decimal.TryParse(txbRangoFin.Text.Trim().Replace("$", "").Replace(",", ""), rango2) Then
                    lblMensajeRangoSancionObligatorio.Text = New Entities.EtiquetaError(2145).Descripcion
                    lblMensajeRangoSancionObligatorio.Visible = True
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarRangoSancion();", True)
                    Exit Sub
                End If

                If rango2 < rango1 Then
                    lblMensajeRangoSancionObligatorio.Text = New Entities.EtiquetaError(2147).Descripcion
                    lblMensajeRangoSancionObligatorio.Visible = True
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarRangoSancion();", True)
                    Exit Sub
                End If

                If txtRango.Text.Trim().Length < 1 Then
                    lblMensajeRangoSancionObligatorio.Text = New Entities.EtiquetaError(2148).Descripcion
                    lblMensajeRangoSancionObligatorio.Visible = True
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarRangoSancion();", True)
                    Exit Sub
                End If

                Dim objCorreoBD As Entities.Correo

                ''Finaliza paso 10 y 11 registra el 12 y notifica a todas las areas
                If AccesoBD.ActualizarRangoSancion(visita.IdVisitaGenerado, rango1, rango2, txtRango.Text.Trim(), ppObjVisita.SubVisitasSeleccionadas) Then
                    objNegVisita.ppObservaciones = txbComentarios.Text.Trim()

                    ''CUANDO ES LA PRIMERA VEZ QUE ENTRA
                    If visita.IdEstatusActual = Constantes.EstatusPaso.Revisado Then
                        objCorreoBD = New Entities.Correo(Constantes.CORREO_ID_NOTIFICA_RANGO_SANCION)
                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                        True, Constantes.EstatusPaso.Aprobado,
                                                                        True, 11, Constantes.EstatusPaso.EnRevisionEspera,
                                                                        False, -1)
                    Else
                        objCorreoBD = New Entities.Correo(Constantes.CORREO_NUEVO_RANGO_SANCION)
                        objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                        True, Constantes.EstatusPaso.Aprobado,
                                                                        True, 11, Constantes.EstatusPaso.EnRevisionEspera,
                                                                        False, -1, False, False, False,
                                                                        True, Constantes.TipoReactivacion.FinalizaReactivacion,
                                                                        12, 10, Constantes.EstatusPaso.Aprobado)
                    End If

                    visita.IdPasoActual = 11
                    visita.FechaInicioPasoActual = DateTime.Now
                    visita.RANGO_SANCION_INI = rango1
                    visita.RANGO_SANCION_FIN = rango2
                    visita.COMENTARIO_RANGO_SANCION = txtRango.Text.Trim()
                    objNegVisita.ppObservaciones = txtRango.Text.Trim()

                    objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_APRUEBA_ACTA_CIRCUNSTANCIADA, visita, True, True, False, , True)
                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, visita.IdEstatusActual,
                                                                         True, Constantes.EstatusPaso.Revisado,
                                                                         True, 12, Constantes.EstatusPaso.EnRevisionEspera,
                                                                         True, objCorreoBD,
                                                                         True, True, True, , , , , , , True)

                    ''SOLICITA LA CONFIRMACION DE LA REUNION
                    ''Si es supervisor solicitar confirmar fecha con VJ
                    If Not visita.SolicitoFechaPaso9 Then
                        Mensaje = "La fecha de la reunión con el área de Supervisión fue: " & visita.Fecha_ReunionVjPaso9.ToString("dd/MM/yyyy") & "."
                        txtFechaGeneral.Text = Fechas.Valor(visita.Fecha_ReunionVjPaso9)
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "ConfirmarFechaReunPreciJuridicoSiNo();", True)
                        Exit Sub
                    End If
                Else
                    Mensaje = "No se pudo actualizar el rango de sancion."
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AquiMuestroMensaje();", True)
                    Exit Sub
                End If

                Response.Redirect("../Procesos/DetalleVisita.aspx")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Guarda el rango se sancion en el paso 17
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnRangoSancionPaso17_Click(sender As Object, e As EventArgs)

        If txbRangoInicio.Text.Trim() = String.Empty Or txbRangoFin.Text.Trim() = String.Empty Then
            lblMensajeRangoSancionObligatorio.Text = New Entities.EtiquetaError(2146).Descripcion
            lblMensajeRangoSancionObligatorio.Visible = True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarRangoSancionPaso17();", True)
            Exit Sub
        Else
            lblMensajeRangoSancionObligatorio.Visible = False
            Dim visita As New Visita()
            visita = CType(Session("DETALLE_VISITA"), Visita)

            Dim Usuario As New Entities.Usuario()
            Usuario = Session(Entities.Usuario.SessionID)

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 And Not IsNothing(Usuario) Then
                Dim objNegVisita As New NegocioVisita(visita, Usuario, Server, txbComentarios.Text)

                Dim rango1 As Decimal
                Dim rango2 As Decimal

                If Not Decimal.TryParse(txbRangoInicio.Text.Trim().Replace("$", "").Replace(",", ""), rango1) Then
                    lblMensajeRangoSancionObligatorio.Text = New Entities.EtiquetaError(2145).Descripcion
                    lblMensajeRangoSancionObligatorio.Visible = True
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarRangoSancionPaso17();", True)
                    Exit Sub
                End If

                If Not Decimal.TryParse(txbRangoFin.Text.Trim().Replace("$", "").Replace(",", ""), rango2) Then
                    lblMensajeRangoSancionObligatorio.Text = New Entities.EtiquetaError(2145).Descripcion
                    lblMensajeRangoSancionObligatorio.Visible = True
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarRangoSancionPaso17();", True)
                    Exit Sub
                End If

                If rango2 < rango1 Then
                    lblMensajeRangoSancionObligatorio.Text = New Entities.EtiquetaError(2147).Descripcion
                    lblMensajeRangoSancionObligatorio.Visible = True
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarRangoSancionPaso17();", True)
                    Exit Sub
                End If

                If txtRango.Text.Trim().Length < 1 Then
                    lblMensajeRangoSancionObligatorio.Text = New Entities.EtiquetaError(2148).Descripcion
                    lblMensajeRangoSancionObligatorio.Visible = True
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarRangoSancionPaso17();", True)
                    Exit Sub
                End If


                ''Finaliza paso 10 y 11 registra el 12 y notifica a todas las areas
                If Not AccesoBD.ActualizarRangoSancion(visita.IdVisitaGenerado, rango1, rango2, txtRango.Text.Trim(), ppObjVisita.SubVisitasSeleccionadas) Then
                    Mensaje = "No se pudo actualizar el rango de sancion."
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AquiMuestroMensaje();", True)
                    Exit Sub
                Else
                    visita.IdPasoActual = 11
                    visita.FechaInicioPasoActual = DateTime.Now
                    visita.RANGO_SANCION_INI = rango1
                    visita.RANGO_SANCION_FIN = rango2
                    visita.COMENTARIO_RANGO_SANCION = txtRango.Text.Trim()

                    objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_RANGO_SANCION, visita)

                    Response.Redirect("../Procesos/DetalleVisita.aspx")
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Guarda la fecha de reunion de la afore y presidencia, paso 13. 'Manda fecha que confirmo sandra pacheco
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFechaAforePresi_Click(sender As Object, e As EventArgs)
        ''Validar que este llena y sea correcta la fecha
        Dim lsFecha As String = txtFechaGeneral.Text.Trim()
        Dim ldFecha As Date

        ''Regresa DateTime.MinValue si alguna validacion falla
        ldFecha = ValidarFechaGeneral(lsFecha, lblFechaGeneral, imgErrorGeneral, "SolicitarFechaReunionEntidad();", True, True)
        If ldFecha = DateTime.MinValue Then
            Exit Sub
        End If

        Dim visita As New Visita()
        visita = CType(Session("DETALLE_VISITA"), Visita)

        Dim Usuario As New Entities.Usuario()
        Usuario = Session(Entities.Usuario.SessionID)

        If Not IsNothing(visita) And Not IsNothing(Usuario) Then
            Dim objNegVisita As New NegocioVisita(visita, Usuario, Server, txbComentarios.Text)

            ''Actualiza fecha para la reunion
            If AccesoBD.ActualizaFechaInicioVisita(visita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionAfore, ppObjVisita.SubVisitasSeleccionadas) Then

                ''Iniciar la visita
                ''Validar si existe la sancion, si no avanzar hasta el paso 16
                ''Validar si no se esta editando la fecha
                visita.FechaReunionAfore = ldFecha

                If piBanderaModificacio = 0 Then
                    objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.Notificado,
                                                                       False, -1, False, -1, -1,
                                                                       True, Constantes.CORREO_PRESENTACION_DIAGNOSTICO_HALLAZGOS,
                                                                       True, True, True, , , , , , , True)
                Else
                    Dim objCorreo As New Entities.Correo(Constantes.CORREO_CAMBIO_FECHA_EXTERNA_INTERNA)

                    objCorreo.Asunto = objCorreo.Asunto.Replace("[TIPO_FECHA]", "externa")
                    objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[FECHA_REUNION_ANT]", visita.FECH_REUNION__AFORE.ToString("dd/MM/yyyy")).
                                                   Replace("[FECHA_REUNION_ACTUAL]", ldFecha.ToString("dd/MM/yyyy")).
                                                   Replace("[TIPO_FECHA]", "externa")
                    objNegVisita.getObjNotificacion().NotificarCorreo(objCorreo, visita, True, True, True, , True)
                End If
            End If

            Response.Redirect("../Procesos/DetalleVisita.aspx")
        End If
    End Sub

    Private Sub HabilitaFuncionalidadCopia(Usuario As Usuario, visita As Visita)
        If Usuario.IdArea = Constantes.AREA_VO Then
            ccfCopiarFolios.HabilitaControles()
            ccfCopiarFolios.Visible = RecuperaParametroValidaCheckCopiarFolios()
            sbSubVisitas.Visible = False
        ElseIf Usuario.IdArea = Constantes.AREA_VF And
          Not visita.EsSubVisitaOsubFolio And
          visita.IdPasoActual > 1 Then
            sbSubVisitas.Visible = True
            ccfCopiarFolios.Visible = False
            sbSubVisitas.HabilitaControles()
            sbSubVisitas.TextoCheck = "Subvisitas"

            ''Configurar el control de subvisitas
            sbSubVisitas.piIdEntidad = visita.IdEntidad
            sbSubVisitas.piVistaPadreSb = visita.IdVisitaGenerado
            sbSubVisitas.psDscEntidadSb = visita.NombreEntidad
            sbSubVisitas.psFolioVisitaPadreSb = visita.FolioVisita
            sbSubVisitas.psSubEntidadesSeleccionadas = visita.SubEntidadesSeleccionadas
        Else
            ccfCopiarFolios.Visible = False
            sbSubVisitas.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Activa el boton prorroga, solo si esta dentro del paso 6 al 8 y si no tieneprorroga
    ''' </summary>
    ''' <param name="visita"></param>
    ''' <remarks></remarks>
    Private Sub ActivarProrroga(visita As Visita, pObjUsuario As Usuario)
        If visita.IdPasoActual >= 6 And visita.IdPasoActual <= 8 Then
            If Not visita.TieneProrroga Then
                imgSolicitarProrroga.Visible = True
                imgSolicitarProrroga.Enabled = True
            ElseIf visita.TieneProrroga And Not visita.TieneProrrogaAprobada Then
                If pObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP Or
                pObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM Then
                    imgAprobarProrroga.Visible = True
                    imgAprobarProrroga.Enabled = True

                    imgRechazarProrroga.Visible = True
                    imgRechazarProrroga.Enabled = True
                End If
            ElseIf Not visita.TieneProrroga And Not visita.TieneProrrogaAprobada Then
                imgSolicitarProrroga.Visible = True
                imgSolicitarProrroga.Enabled = True
            End If
        End If

        'Oculta botones de activación de prórroga si visita viene de sisvig y área es VO y paso es menor a 19
        If visita.VisitaSisvig And visita.IdPasoActual <= 19 And EsAreaOperativa(puObjUsuario.IdArea) Then
            imgSolicitarProrroga.Visible = False
            imgSolicitarProrroga.Enabled = False

            imgAprobarProrroga.Visible = False
            imgAprobarProrroga.Enabled = False

            imgRechazarProrroga.Visible = False
            imgRechazarProrroga.Enabled = False
        End If

    End Sub

    ''' <summary>
    ''' Supervisor aprueba la prorroga
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub imgAprobarProrroga_Click(sender As Object, e As ImageClickEventArgs) Handles imgAprobarProrroga.Click
        If Not IsNothing(Session("DETALLE_VISITA")) And Not IsNothing(Entities.Usuario.SessionID) Then

            ObtenerSubVisitasSeleccionadas()

            Dim visita As New Visita()
            visita = CType(Session("DETALLE_VISITA"), Visita)

            Dim objUsuario As New Entities.Usuario()
            objUsuario = Session(Entities.Usuario.SessionID)

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 And Not IsNothing(objUsuario) Then
                Dim objNegVisita As New NegocioVisita(visita, objUsuario, Server, txbComentarios.Text)

                Dim prorroga As New Prorroga()

                prorroga.IdVisitaGenerado = visita.IdVisitaGenerado
                prorroga.IdPaso = visita.IdPasoActual
                prorroga.FechaRegistro = DateTime.Now
                prorroga.NumDiasDeProrroga = 0
                prorroga.MotivoProrroga = ""
                prorroga.FechaFinProrroga = Nothing
                prorroga.ApruebaProrroga = Constantes.Verdadero
                prorroga.SubVisitasSeleccionadas = ppObjVisita.SubVisitasSeleccionadas

                Dim con As Conexion.SQLServer = Nothing
                Dim tran As SqlClient.SqlTransaction = Nothing
                Dim guardo As Boolean = False

                Try
                    con = New Conexion.SQLServer()
                    tran = con.BeginTransaction()
                    If AccesoBD.registrarProrroga(prorroga, con, tran) > 0 Then
                        guardo = True
                    End If

                Catch ex As Exception
                    'Registro fallido
                    guardo = False
                    Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, imgAprobarProrroga_Click", "")
                Finally
                    If Not IsNothing(tran) Then
                        If guardo Then
                            'Solicitud de prorroga exitosa
                            tran.Commit()

                            ''Notificar prorroga 
                            objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_VJ_PR_VISITA_ENTRA_EN_PRORROGA,
                                                                       visita, True, True, , , True)
                        Else
                            'Solicitud de prorroga fallida
                            tran.Rollback()
                            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgError
                            Dim errores As New Entities.EtiquetaError(2133)
                            Mensaje = errores.Descripcion
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
                        End If
                        tran.Dispose()
                    End If

                    If Not IsNothing(con) Then
                        con.CerrarConexion()
                        con = Nothing
                    End If
                End Try

                Response.Redirect("../Procesos/DetalleVisita.aspx")
            End If
        End If
    End Sub

    Protected Sub imgRechazarProrroga_Click(sender As Object, e As ImageClickEventArgs) Handles imgRechazarProrroga.Click
        If Not IsNothing(Session("DETALLE_VISITA")) And Not IsNothing(Entities.Usuario.SessionID) Then

            ObtenerSubVisitasSeleccionadas()

            Dim visita As New Visita()
            visita = CType(Session("DETALLE_VISITA"), Visita)

            Dim objUsuario As New Entities.Usuario()
            objUsuario = Session(Entities.Usuario.SessionID)

            If Not IsNothing(visita) And visita.IdVisitaGenerado > 0 And Not IsNothing(objUsuario) Then
                Dim objNegVisita As New NegocioVisita(visita, objUsuario, Server, txbComentarios.Text)

                Dim prorroga As New Prorroga()

                prorroga.IdVisitaGenerado = visita.IdVisitaGenerado
                prorroga.IdPaso = visita.IdPasoActual
                prorroga.FechaRegistro = DateTime.Now
                prorroga.NumDiasDeProrroga = 0
                prorroga.MotivoProrroga = ""
                prorroga.FechaFinProrroga = Nothing
                prorroga.ApruebaProrroga = Constantes.Falso
                prorroga.SubVisitasSeleccionadas = ppObjVisita.SubVisitasSeleccionadas

                Dim con As Conexion.SQLServer = Nothing
                Dim tran As SqlClient.SqlTransaction = Nothing
                Dim guardo As Boolean = False

                Try
                    con = New Conexion.SQLServer()
                    tran = con.BeginTransaction()
                    If AccesoBD.registrarProrroga(prorroga, con, tran) > 0 Then
                        guardo = True
                    End If

                Catch ex As Exception
                    'Registro fallido
                    guardo = False
                    Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, imgAprobarProrroga_Click", "")
                Finally
                    If Not IsNothing(tran) Then
                        If guardo Then
                            'Solicitud de prorroga exitosa
                            tran.Commit()

                            ''Notificar prorroga
                            Dim lstDest As New List(Of Persona)
                            Dim objPersona As Persona

                            For Each objInspector As InspectorAsignado In visita.LstInspectoresAsignados
                                objPersona = New Persona
                                objPersona.Nombre = objInspector.Nombre
                                objPersona.Correo = objInspector.Correo

                                lstDest.Add(objPersona)
                            Next

                            If lstDest.Count > 0 Then
                                objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_ID_NOTIFICA_RECHAZO_PRORROGA,
                                                                      visita, True, False, False, lstDest, True)
                            End If
                        Else
                            'Solicitud de prorroga fallida
                            tran.Rollback()
                            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgError
                            Dim errores As New Entities.EtiquetaError(2133)
                            Mensaje = errores.Descripcion
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
                        End If
                        tran.Dispose()
                    End If

                    If Not IsNothing(con) Then
                        con.CerrarConexion()
                        con = Nothing
                    End If
                End Try

                Response.Redirect("../Procesos/DetalleVisita.aspx")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Marca la visita que no hubo sancion y avanza al paso 13, ''Paso 8 HallazgosGuardados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnConfirmarNoDv_Click(sender As Object, e As EventArgs)
        If AccesoBD.ActualizaSancionVisita(ppObjVisita.IdVisitaGenerado, ppObjVisita.IdPasoActual) Then
            Response.Redirect("../Procesos/DetalleVisita.aspx")
        End If
    End Sub

    ''Redirecciona cuando se elije que si hay sancion en el paso 8
    Protected Sub btnConfirmarDv_Click(sender As Object, e As EventArgs)
        Response.Redirect("../Procesos/DetalleVisita.aspx")
    End Sub

    Protected Sub btnEditAllazgosInt_Click(sender As Object, e As ImageClickEventArgs) Handles btnEditAllazgosInt.Click
        piBanderaModificacio = 1
        ObtenerSubVisitasSeleccionadas()
        txtFechaReunion.Text = Fechas.Valor(ppObjVisita.FECH_REUNION__PRESIDENCIA)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaReunionPresidencia();", True)
        Exit Sub
    End Sub

    Protected Sub btnEditAllazgosExt_Click(sender As Object, e As ImageClickEventArgs) Handles btnEditAllazgosExt.Click
        piBanderaModificacio = 1
        ObtenerSubVisitasSeleccionadas()
        txtFechaGeneral.Text = Fechas.Valor(ppObjVisita.FECH_REUNION__AFORE)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaReunionEntidad();", True)
        Exit Sub
    End Sub

    ''' <summary>
    ''' Genera todas las pestanias de las visitas
    ''' </summary>
    ''' <param name="pObjVisita"></param>
    ''' <remarks></remarks>
    Private Sub cargaPestaniasSubVisitas(pObjVisita As Visita, pbPermisosDoctos As Boolean)
        Dim ltpNewTab As AjaxControlToolkit.TabPanel
        Dim ltpNewTabDocs As AjaxControlToolkit.TabPanel
        Dim lcuDetalleSubVisita As cuDetalleVisita
        Dim lcuDocumentos As ucDocumentos
        Dim lAuxVisita As Visita
        Dim objPropiedades As ucDocumentos.PropiedadesDoc

        ''recorrer las sub visitas e ir creaucDocumentosndo los controles de usuario
        For Each lObjSubVisita As Visita.SubVisitas In pObjVisita.LstSubVisitas
            If Not lObjSubVisita.EstaSeleccionada Then
                Continue For
            End If

            lcuDetalleSubVisita = CType(LoadControl("../Controles/cuDetalleVisita.ascx"), cuDetalleVisita)
            lcuDocumentos = CType(LoadControl("../Controles/ucDocumentos.ascx"), ucDocumentos)
            lAuxVisita = AccesoBD.getDetalleVisita(lObjSubVisita.Id, puObjUsuario.IdArea)
            lAuxVisita.LstInspectoresAsignados = AccesoBD.getInspectoresAsignados(lAuxVisita.IdVisitaGenerado)
            lAuxVisita.LstSupervisoresAsignados = AccesoBD.getSupervisoresAsignados(lAuxVisita.IdVisitaGenerado)

            Dim lstAbogados As List(Of Abogado) = AccesoBD.getAbogadosAsignados(lAuxVisita.IdVisitaGenerado)
            lAuxVisita.LstAbogadosSupAsesorAsig = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.PERFIL_SUP And objAbo.SubPerfil = Constantes.ABOGADOS.PERFIL_ABO_ASESOR Select objAbo).ToList()
            ''AccesoBD.getAbogadosAsignados(idVisita, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_ASESOR)
            lAuxVisita.LstAbogadosAsesorAsignados = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.ABOGADOS.PERFIL_ABO_ASESOR Select objAbo).ToList()
            ''AccesoBD.getAbogadosAsignados(idVisita, Constantes.ABOGADOS.PERFIL_ABO_ASESOR)

            lAuxVisita.LstAbogadosSupSancionAsig = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.PERFIL_SUP And objAbo.SubPerfil = Constantes.ABOGADOS.PERFIL_ABO_SANCIONES Select objAbo).ToList()
            ''AccesoBD.getAbogadosAsignados(idVisita, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES)
            lAuxVisita.LstAbogadosSancionAsignados = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.ABOGADOS.PERFIL_ABO_SANCIONES Select objAbo).ToList()
            ''AccesoBD.getAbogadosAsignados(idVisita, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES)

            lAuxVisita.LstAbogadosSupContenAsig = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.PERFIL_SUP And objAbo.SubPerfil = Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO Select objAbo).ToList()
            ''AccesoBD.getAbogadosAsignados(idVisita, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO)
            lAuxVisita.LstAbogadosContenAsignados = (From objAbo As Abogado In lstAbogados Where objAbo.Perfil = Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO Select objAbo).ToList()
            ''AccesoBD.getAbogadosAsignados(idVisita, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO)

            If lAuxVisita.UsuarioEstaOcupando.Trim().Length < 1 Then
                lAuxVisita.EstaVisitaOcupada = True
                lAuxVisita.UsuarioEstaOcupando = puObjUsuario.IdentificadorUsuario
            End If

            lcuDetalleSubVisita.pvVisita = lAuxVisita
            lcuDetalleSubVisita.piIdVisitaActual = lObjSubVisita.Id
            lcuDetalleSubVisita.ID = "cuDetalleSubVisita" & lObjSubVisita.Id.ToString()

            ltpNewTab = New AjaxControlToolkit.TabPanel
            ltpNewTab.Controls.Add(lcuDetalleSubVisita)
            ltpNewTab.ID = "tpPestaniaS" & lObjSubVisita.Id.ToString()
            ltpNewTab.CssClass = "tabPestanias"
            ltpNewTab.HeaderText = lObjSubVisita.Descripcion

            objPropiedades = New ucDocumentos.PropiedadesDoc

            objPropiedades.ppObjVisita = lAuxVisita
            objPropiedades.pgIdTxtComentarios = "txbComentarios"
            objPropiedades.pbEsSubFolioSubVisita = True
            objPropiedades.piIdWidthPanel = 69
            objPropiedades.PermisoEditarDocs = pbPermisosDoctos
            lcuDocumentos.ModificaIds(lObjSubVisita.Id.ToString())

            If Not IsPostBack Then
                lcuDocumentos.pObjPropiedades = objPropiedades
            ElseIf IsNothing(lcuDocumentos.pObjPropiedades) Then
                lcuDocumentos.pObjPropiedades = objPropiedades
            End If

            ltpNewTabDocs = New AjaxControlToolkit.TabPanel
            ltpNewTabDocs.Controls.Add(lcuDocumentos)
            ltpNewTabDocs.ID = "tpPestaniasDocsP" & lObjSubVisita.Id.ToString()
            ltpNewTabDocs.CssClass = "tabPestanias"
            ltpNewTabDocs.HeaderText = lObjSubVisita.Descripcion

            idContPestanias.Tabs.Add(ltpNewTab)
            tcContPestaDocs.Tabs.Add(ltpNewTabDocs)
        Next
    End Sub

    Protected Sub btnSubvisitasModal_Click(sender As Object, e As EventArgs)
        ''GUARDAR EN BD LAS SUBVISITAS SELECCIONADAS
        If IsNothing(puObjUsuario) Then
            Exit Sub
        End If

        ObtenerSubVisitasSeleccionadas()
        AccesoBD.ActualizaBanderaSubvisita(ppObjVisita.IdVisitaGenerado, ppObjVisita.SubVisitasSeleccionadas, puObjUsuario.IdentificadorUsuario)

        Select Case hfBotonPresionado.Value
            Case "imgSiguiente"
                imgSiguiente_Click(imgSiguiente, New ImageClickEventArgs(1, 1))
            Case "imgDetener"
                imgDetener_Click(imgDetener, New ImageClickEventArgs(1, 1))
            Case "imgNotificar3"
                imgNotificar3_Click(imgNotificar3, New ImageClickEventArgs(1, 1))
            Case "imgGuardarCambios"
                imgGuardarCambios_Click(imgGuardarCambios, New ImageClickEventArgs(1, 1))
            'Case "imgNotificar"
            '    imgNotificar_Click(imgNotificar, New ImageClickEventArgs(1, 1))
            Case "imgAnterior"
                imgAnterior_Click(imgAnterior, New ImageClickEventArgs(1, 1))
            'Case "imgNotificar2"
            '    imgNotificar2_Click(imgNotificar2, New ImageClickEventArgs(1, 1))
            Case "imgIniciarVisita"
                imgIniciarVisita_Click(imgIniciarVisita, New ImageClickEventArgs(1, 1))
            Case "imgRechazarProrroga"
                imgRechazarProrroga_Click(imgRechazarProrroga, New ImageClickEventArgs(1, 1))
            Case "imgAprobarProrroga"
                imgAprobarProrroga_Click(imgAprobarProrroga, New ImageClickEventArgs(1, 1))
            Case "btnEditAllazgosExt"
                btnEditAllazgosExt_Click(btnEditAllazgosExt, New ImageClickEventArgs(1, 1))
            Case "btnEditAllazgosInt"
                btnEditAllazgosInt_Click(btnEditAllazgosInt, New ImageClickEventArgs(1, 1))
            Case "imgSubvisitas"
                Dim ltpNewTab As AjaxControlToolkit.TabPanel
                Dim ltpNewTabDocs As AjaxControlToolkit.TabPanel
                Dim lcControl As Control

                For Each chkItem As ListItem In chkSubVisitasMod.Items
                    lcControl = idContPestanias.FindControl("tpPestaniaS" & chkItem.Value)
                    ltpNewTab = CType(IIf(Not IsNothing(lcControl), lcControl, New AjaxControlToolkit.TabPanel), AjaxControlToolkit.TabPanel)
                    If Not IsNothing(ltpNewTab) Then ltpNewTab.Visible = chkItem.Selected

                    lcControl = tcContPestaDocs.FindControl("tpPestaniasDocsP" & chkItem.Value)
                    ltpNewTabDocs = CType(IIf(Not IsNothing(lcControl), lcControl, New AjaxControlToolkit.TabPanel), AjaxControlToolkit.TabPanel)
                    If Not IsNothing(ltpNewTabDocs) Then ltpNewTabDocs.Visible = chkItem.Selected
                Next
        End Select
    End Sub

    ''' <summary>
    ''' Obtiene las visitas que selecciono el usuario
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ObtenerSubVisitasSeleccionadas()
        Try
            If Me.ppObjVisita.TieneSubVisitas And chkSubVisitasMod.SelectedValue <> "" Then
                ppObjVisita.SubVisitasSeleccionadas = ""
                ppObjVisita.FoliosSubVisitasSeleccionadas = ""

                For Each chkItem As ListItem In chkSubVisitasMod.Items
                    If chkItem.Selected Then
                        ppObjVisita.SubVisitasSeleccionadas &= chkItem.Value & ","
                        ppObjVisita.FoliosSubVisitasSeleccionadas &= chkItem.Text & ","
                    End If
                Next

                ''ELIMINA LA ULTIMA COMA
                ppObjVisita.SubVisitasSeleccionadas = ppObjVisita.SubVisitasSeleccionadas.Substring(0, ppObjVisita.SubVisitasSeleccionadas.Length - 1)
                ppObjVisita.FoliosSubVisitasSeleccionadas = ppObjVisita.FoliosSubVisitasSeleccionadas.Substring(0, ppObjVisita.FoliosSubVisitasSeleccionadas.Length - 1)
            Else
                ppObjVisita.SubVisitasSeleccionadas = ""
                ppObjVisita.FoliosSubVisitasSeleccionadas = ""
            End If
        Catch : ppObjVisita.SubVisitasSeleccionadas = "" : ppObjVisita.FoliosSubVisitasSeleccionadas = "" : End Try
    End Sub

    ''' <summary>
    ''' Busca documentos obligatorios en todas las visitas y subvisitas
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function HayDocumentosHobligatorios() As Boolean
        'If ucDocumentos.HayDoctosObligatoriosSinCargarPorVisita() Then Exit Sub

        Dim lstExpedientes As List(Of ucDocumentos) = tcContPestaDocs.GetAllControlsOfType(Of ucDocumentos)()
        Dim lsListaFolios As String = "<ul>"

        For Each ucDocumentos As ucDocumentos In lstExpedientes
            If ppObjVisita.SubVisitasSeleccionadas.Contains(ucDocumentos.pObjPropiedades.ppObjVisita.IdVisitaGenerado.ToString()) Or ppObjVisita.IdVisitaGenerado = ucDocumentos.pObjPropiedades.ppObjVisita.IdVisitaGenerado Then
                If ucDocumentos.HayDoctosObligatoriosSinCargarPorVisitaSinMensage() Then
                    lsListaFolios &= "<li>" & ucDocumentos.pObjPropiedades.ppObjVisita.FolioVisita & "</li>"
                End If
            End If
        Next

        If lsListaFolios.Length > 5 Then
            Dim errores As New Entities.EtiquetaError(2140)
            Mensaje = errores.Descripcion & "<br /><br /> Visita(s): " & lsListaFolios & "</ul>"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AvisoCatalogoDoc();", True)
            Return True
        End If

        Return False
    End Function

    ''' <summary>
    ''' Paso 9 avanza paso 10, solicita fecha con vj
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFechaReunionVj_Click(sender As Object, e As EventArgs)

        ''Validar que este llena y sea correcta la fecha
        Dim lsFecha As String = txtFechaGeneral.Text.Trim()
        Dim ldFecha As Date

        ''Regresa DateTime.MinValue si alguna validacion falla
        ldFecha = ValidarFechaGeneral(lsFecha, lblFechaGeneral, imgErrorGeneral, "SolicitarFechaReunPreciJuridico();", False, False, True, True, PasoProcesoVisita.Pasos.Diez)
        If ldFecha = DateTime.MinValue Then
            Exit Sub
        End If

        Dim objUsuario As New Entities.Usuario()
        objUsuario = Session(Entities.Usuario.SessionID)

        Dim objNegVisita As NegocioVisita

        ''Objeto negocio visita
        If IsNothing(objUsuario) Or IsNothing(ppObjVisita) Then
            Exit Sub
        ElseIf ppObjVisita.IdVisitaGenerado <= 0 Then
            Exit Sub
        End If

        Dim objVisita As Visita = ppObjVisita

        objNegVisita = New NegocioVisita(objVisita, objUsuario, Server, txbComentarios.Text)

        ''Actualiza fecha para la reunion
        If AccesoBD.ActualizaFechaInicioVisita(objVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionVjp9, objVisita.SubVisitasSeleccionadas) Then

            ''Fecha reunion con vj paso 9
            objVisita.Fecha_ReunionVjPaso9 = ldFecha

            ''Envia al paso 10, cierra paso 9 y notifica
            If objVisita.IdEstatusActual = Constantes.EstatusPaso.Elaborada Then
                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                    True, Constantes.EstatusPaso.Enviado,
                                                                    True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                    True, Constantes.CORREO_ID_NOTIFICA_ENVIO_ACTA_CIRCUNSTANCIADA,
                                                                    True, True, False, , , , , , , True)
            Else
                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesEnviados,
                                                                    False, -1,
                                                                    False, -1, -1,
                                                                    True, Constantes.CORREO_ID_NOTIFICA_ENVIO_ACTA_CIRCUNSTANCIADA,
                                                                    True, True, False,
                                                                    True, Constantes.TipoReactivacion.FinalizaReactivacion,
                                                                    (objVisita.IdPasoActual + 1), objVisita.IdPasoActual,
                                                                    Constantes.EstatusPaso.EnRevisionEspera, , True)
            End If
        End If

        Response.Redirect("../Procesos/DetalleVisita.aspx")
    End Sub

    ''' <summary>
    ''' Flujo inicial paso 16, revisado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFechaReunionVjP16_Click(sender As Object, e As EventArgs)
        ''Validar que este llena y sea correcta la fecha
        Dim lsFecha As String = txtFechaGeneral.Text.Trim()
        Dim ldFecha As Date

        ''Regresa DateTime.MinValue si alguna validacion falla
        ldFecha = ValidarFechaGeneral(lsFecha, lblFechaGeneral, imgErrorGeneral, "SolicitarFechaReunPreciJuridicoP16();", False, False, True, True, PasoProcesoVisita.Pasos.Diesisiete)
        If ldFecha = DateTime.MinValue Then
            Exit Sub
        End If

        Dim objUsuario As New Entities.Usuario()
        objUsuario = Session(Entities.Usuario.SessionID)

        Dim objNegVisita As NegocioVisita

        ''Objeto negocio visita
        If IsNothing(objUsuario) Or IsNothing(ppObjVisita) Then
            Exit Sub
        ElseIf ppObjVisita.IdVisitaGenerado <= 0 Then
            Exit Sub
        End If

        Dim objVisita As Visita = ppObjVisita

        objNegVisita = New NegocioVisita(objVisita, objUsuario, Server, txbComentarios.Text)

        ''Actualiza fecha para la reunion
        If AccesoBD.ActualizaFechaInicioVisita(objVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionVjp16, objVisita.SubVisitasSeleccionadas) Then

            ''Fecha reunion con vj paso 16
            objVisita.Fecha_ReunionVjPaso16 = ldFecha

            ''Envia al paso 17, cierra paso 16 y notifica
            If objVisita.IdEstatusActual = Constantes.EstatusPaso.Revisado Then
                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                     True, Constantes.EstatusPaso.Elaborada,
                                                                     True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                     True, Constantes.CORREO_ID_NOTIFICA_VJ_ADJUNTA_ACTA_CONCLUSION_OF_RECOMENDACIONES, True, True, , , , , , , , True)
            Else
                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, Constantes.EstatusPaso.AjustesEnviados,
                                                                    False, -1,
                                                                    False, -1, -1,
                                                                    True, Constantes.CORREO_ID_NOTIFICA_VJ_ADJUNTA_ACTA_CONCLUSION_OF_RECOMENDACIONES,
                                                                    True, True, False,
                                                                    True, Constantes.TipoReactivacion.FinalizaReactivacion,
                                                                    (objVisita.IdPasoActual + 1), objVisita.IdPasoActual,
                                                                    Constantes.EstatusPaso.EnRevisionEspera, , True)
            End If
        End If

        Response.Redirect("../Procesos/DetalleVisita.aspx")
    End Sub

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
#Region "Extrae Check Copiar Folios"
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
#End Region

    Protected Sub btnConfFechaReunionVjp9_Click(sender As Object, e As EventArgs)
        ''Validar que este llena y sea correcta la fecha
        Dim lsFecha As String = txtFechaGeneral.Text.Trim()
        Dim ldFecha As Date

        ''Regresa DateTime.MinValue si alguna validacion falla
        ldFecha = ValidarFechaGeneral(lsFecha, lblFechaGeneral, imgErrorGeneral, "ConfirmarFechaReunPreciJuridico();", False, False, True, True, PasoProcesoVisita.Pasos.Diez)
        If ldFecha = DateTime.MinValue Then
            Exit Sub
        End If

        Dim objUsuario As New Entities.Usuario()
        objUsuario = Session(Entities.Usuario.SessionID)

        Dim objNegVisita As NegocioVisita

        ''Objeto negocio visita
        If IsNothing(objUsuario) Or IsNothing(ppObjVisita) Then
            Exit Sub
        ElseIf ppObjVisita.IdVisitaGenerado <= 0 Then
            Exit Sub
        End If

        Dim objVisita As Visita = ppObjVisita

        objNegVisita = New NegocioVisita(objVisita, objUsuario, Server, txbComentarios.Text)

        ''Actualiza fecha para la reunion
        If AccesoBD.ActualizaFechaInicioVisita(objVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionVjp9Confirmacion, ppObjVisita.SubVisitasSeleccionadas) Then
            Response.Redirect("../Procesos/DetalleVisita.aspx")
        End If
    End Sub

    Protected Sub btnConfFechaReunionVjp16_Click(sender As Object, e As EventArgs)
        ''Validar que este llena y sea correcta la fecha
        Dim lsFecha As String = txtFechaGeneral.Text.Trim()
        Dim ldFecha As Date

        ''Regresa DateTime.MinValue si alguna validacion falla
        ldFecha = ValidarFechaGeneral(lsFecha, lblFechaGeneral, imgErrorGeneral, "ConfirmarFechaReunPreciJuridicoP16();", False, False, True, True, PasoProcesoVisita.Pasos.Diesisiete)
        If ldFecha = DateTime.MinValue Then
            Exit Sub
        End If

        Dim objUsuario As New Entities.Usuario()
        objUsuario = Session(Entities.Usuario.SessionID)

        Dim objNegVisita As NegocioVisita

        ''Objeto negocio visita
        If IsNothing(objUsuario) Or IsNothing(ppObjVisita) Then
            Exit Sub
        ElseIf ppObjVisita.IdVisitaGenerado <= 0 Then
            Exit Sub
        End If

        Dim objVisita As Visita = ppObjVisita

        objNegVisita = New NegocioVisita(objVisita, objUsuario, Server, txbComentarios.Text)

        ''Actualiza fecha para la reunion
        If AccesoBD.ActualizaFechaInicioVisita(objVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionVjp16Confirmacion, ppObjVisita.SubVisitasSeleccionadas) Then
            If MostrarSolicitudRangoSancion() Then
                Response.Redirect("../Procesos/DetalleVisita.aspx")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Solicitar fecha acta circunstanciada in situ, ''Solicitar fecha acta circunstanciada in situ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFechaInSituActaC_Click(sender As Object, e As EventArgs)
        ''Validar que este llena y sea correcta la fecha
        Dim lsFecha As String = txtFechaGeneral.Text.Trim()
        Dim ldFecha As Date

        ''Regresa DateTime.MinValue si alguna validacion falla
        ldFecha = ValidarFechaGeneral(lsFecha, lblFechaGeneral, imgErrorGeneral, "SolicitarFechaGeneral('" & Constantes.MensajesModal.ActaCircunstanciada & "', 'btnFechaInSituActaC');", True, True)
        If ldFecha = DateTime.MinValue Then
            Exit Sub
        End If

        Dim objUsuario As New Entities.Usuario()
        objUsuario = Session(Entities.Usuario.SessionID)

        Dim objNegVisita As NegocioVisita

        ''Objeto negocio visita
        If IsNothing(objUsuario) Or IsNothing(ppObjVisita) Then
            Exit Sub
        ElseIf ppObjVisita.IdVisitaGenerado <= 0 Then
            Exit Sub
        End If

        Dim objVisita As Visita = ppObjVisita

        objNegVisita = New NegocioVisita(objVisita, objUsuario, Server, txbComentarios.Text)

        If AccesoBD.ActualizaFechaInicioVisita(objVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaInSituActaCircunstanciada, ppObjVisita.SubVisitasSeleccionadas) Then
            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                      True, Constantes.EstatusPaso.Notificado,
                                                                      True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                      True, Constantes.CORREO_ID_NOTIFICA_VJ_PR_HA_LEVANTADO_ACTA_CIR_IN_SITU,
                                                                      False, True, True, , , , , , ldFecha)

            ''Actualiza el paso actual, manda a paso 16
            objVisita.IdPasoActual = (objVisita.IdPasoActual + 1)
            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                  False, -1,
                                  True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                  False, -1)
            Response.Redirect("../Procesos/DetalleVisita.aspx")
        End If
    End Sub

    ''' <summary>
    ''' ''Flujo inicial paso 18, Revisado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFechaLevantaActa_Click(sender As Object, e As EventArgs)
        ''Validar que este llena y sea correcta la fecha
        Dim lsFecha As String = txtFechaGeneral.Text.Trim()
        Dim ldFecha As Date

        ''Regresa DateTime.MinValue si alguna validacion falla
        ldFecha = ValidarFechaGeneral(lsFecha, lblFechaGeneral, imgErrorGeneral, "SolicitarFechaGeneral('" & Constantes.MensajesModal.FechaLevantamientoInSitu & "', 'btnFechaLevantaActa');", True, True)
        If ldFecha = DateTime.MinValue Then
            Exit Sub
        End If

        Dim objUsuario As New Entities.Usuario()
        objUsuario = Session(Entities.Usuario.SessionID)

        Dim objNegVisita As NegocioVisita

        ''Objeto negocio visita
        If IsNothing(objUsuario) Or IsNothing(ppObjVisita) Then
            Exit Sub
        ElseIf ppObjVisita.IdVisitaGenerado <= 0 Then
            Exit Sub
        End If

        Dim objVisita As Visita = ppObjVisita

        objNegVisita = New NegocioVisita(objVisita, objUsuario, Server, txbComentarios.Text)

        If AccesoBD.ActualizaFechaInicioVisita(objVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaLevantamientoActaConclusion, ppObjVisita.SubVisitasSeleccionadas) Then
            ''Si tiene sancion sigue el flujo normal, si no se concluye la visita
            If objVisita.TieneSancion Then
                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                 True, Constantes.EstatusPaso.Notificado,
                                 True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                 True, Constantes.CORREO_ID_NOTIFICA_VJ_PR_HA_LEVANTADO_ACTA_CON_IN_SITU, True, True, False, , , , , , ldFecha, True)
            Else
                objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                 True, Constantes.EstatusPaso.Notificado,
                                 False, -1, -1,
                                 True, Constantes.CORREO_ID_NOTIFICA_VJ_PR_HA_LEVANTADO_ACTA_CON_IN_SITU, True, True, False, , , , , , ldFecha, True)
            End If

            Response.Redirect("../Procesos/DetalleVisita.aspx")
        End If

    End Sub


    Protected Sub btnAlertaPaso18_Click(sender As Object, e As EventArgs)
        txtFechaGeneral.Text = Fechas.Valor(ppObjVisita.Fecha_LevantamientoActaConclusion)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.FechaLevantamientoInSitu & "', 'btnFechaLevantaActa');", True)
      Exit Sub
   End Sub

   ''' <summary>
   ''' Boton si paso 9, actualiza bandera de solicitud
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnConfFechaReunionP9ok_Click(sender As Object, e As EventArgs)
      If Not Fechas.Vacia(ppObjVisita.Fecha_ReunionVjPaso9) Then
         Dim ldVecFecha As String() = ppObjVisita.Fecha_ReunionVjPaso9.ToString("dd/MM/yyyy").Split("/")

         If ldVecFecha.Length = 3 Then
            Dim ldFecha As Date = New Date(ldVecFecha(2), ldVecFecha(1), ldVecFecha(0))

            ''Actualiza fecha para la reunion
            If AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionVjp9Confirmacion, ppObjVisita.SubVisitasSeleccionadas) Then
               Response.Redirect("../Procesos/DetalleVisita.aspx")
            End If
         End If
      End If
   End Sub

   ''' <summary>
   ''' Retorna la fecha de finalizacion de algun paso considerando la prorroga
   ''' </summary>
   ''' <param name="piPaso"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Private Function ObtenerFechaFinalizaPaso(piPaso As Integer) As DateTime
      Dim ldFechafinalizaPaso As DateTime
      Dim diasFinalizaPaso As Integer = 0
      Dim lstCatalgoPasos As List(Of Paso) = AccesoBD.getCatalogoPasos(piPaso)
      Dim objPaso As Paso = (From lPaso In lstCatalgoPasos Where lPaso.IdPaso = piPaso Select lPaso).FirstOrDefault()

      If Not IsNothing(objPaso) Then
         If ppObjVisita.TieneProrrogaAprobada Then
            diasFinalizaPaso = objPaso.NumDiasMax
         ElseIf objPaso.EnProrroga = 1 Then
            diasFinalizaPaso = objPaso.NumDiasMin
         Else
            diasFinalizaPaso = objPaso.NumDiasMax
         End If

         ldFechafinalizaPaso = AccesoBD.ObtenerFecha(DateTime.Now, diasFinalizaPaso)
      Else
         ldFechafinalizaPaso = Nothing
      End If

      Return ldFechafinalizaPaso
   End Function

   ''' <summary>
   ''' No se llevo a cabo paso 9
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnConfFechaReunionP9NoDos_Click(sender As Object, e As EventArgs)
      ''Actualiza fecha para la reunion
      If AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, Date.Now, Constantes.TipoFecha.FechaReunionVjp9NoSeRealizo, ppObjVisita.SubVisitasSeleccionadas) Then
         Response.Redirect("../Procesos/DetalleVisita.aspx")
      End If
   End Sub

   Protected Sub btnConfFechaReunionVjpok_Click(sender As Object, e As EventArgs)
      If Not Fechas.Vacia(ppObjVisita.Fecha_ReunionVjPaso16) Then
         Dim ldVecFecha As String() = ppObjVisita.Fecha_ReunionVjPaso16.ToString("dd/MM/yyyy").Split("/")

         If ldVecFecha.Length = 3 Then
            Dim ldFecha As Date = New Date(ldVecFecha(2), ldVecFecha(1), ldVecFecha(0))

            ''Actualiza fecha para la reunion
            If AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionVjp16Confirmacion, ppObjVisita.SubVisitasSeleccionadas) Then
               If Not MostrarSolicitudRangoSancion() Then
                  Exit Sub
               End If
            End If
         End If
      End If

      Response.Redirect("../Procesos/DetalleVisita.aspx")
   End Sub

   Protected Sub btnConfFechaReunionVjp16NoDos_Click(sender As Object, e As EventArgs)
      ''Actualiza fecha para la reunion
      If AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, Date.Now, Constantes.TipoFecha.FechaReunionVjp16NoSeRealizo, ppObjVisita.SubVisitasSeleccionadas) Then
         If MostrarSolicitudRangoSancion() Then
            Response.Redirect("../Procesos/DetalleVisita.aspx")
         End If
      End If
   End Sub

   Private Function MandaCorreoSandraPachecoPaso12(objNegVisita As NegocioVisita) As Boolean
      Dim lstPersonasDestinatarios As New List(Of Persona)
      Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.SandraPacheco)

      ''Conexion.SQLServer
      If dt.Rows.Count > 0 Then
         ''Folio, area, entidades
         lstPersonasDestinatarios.Add(New Persona With {.Nombre = Constantes.Parametros.SandraPacheco, .Correo = dt.Rows(0)("T_DSC_VALOR")})
         objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_SANDRA_PACHECO, ppObjVisita,
                                                       True, True, False, lstPersonasDestinatarios, True)
         Return True
      Else
         Return False
      End If
   End Function

   Private Function MandaCorreoSandraPachecoVulnerabilidad(objNegVisita As NegocioVisita, ByVal idCorreo As Integer) As Boolean
      Dim lstPersonasDestinatarios As New List(Of Persona)
      Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.SandraPacheco)
      Dim objCorreoBD As New Entities.Correo(idCorreo)

      If dt.Rows.Count > 0 Then
         lstPersonasDestinatarios.Add(New Persona With {.Nombre = Constantes.Parametros.SandraPacheco, .Correo = dt.Rows(0)("T_DSC_VALOR")})

         objNegVisita.getObjNotificacion().NotificarCorreo(objCorreoBD, ppObjVisita,
                                                       True, True, False, lstPersonasDestinatarios, True)
         Return True
      Else
         Me.Mensaje = "No se encontro el parametro con el nombre: " & Constantes.Parametros.SandraPacheco
         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
         Return False
      End If
   End Function
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
               Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.DiasFechasAnteriores), liNumDiasAux)
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
                     If ppObjVisita.FechaInicioPasoActual <> DateTime.MinValue Then
                        ldFechaAux = ppObjVisita.FechaInicioPasoActual
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
                  If pbValidaNdiasHabiles Then
                     Dim liNumDias As Integer = RecuperaParametroDiasFecha()
                     Dim ldFechaEnviaVj As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDias)

                     If ldFecha.Date < ldFechaEnviaVj.Date Then
                        imgImagenError.ImageUrl = Constantes.Imagenes.Fallo
                        Dim errores As New Entities.EtiquetaError(2162)
                        lblFecha.Visible = True
                        lblFecha.Text = errores.Descripcion.Replace("[NUM_DIAS]", liNumDias.ToString())

                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", psScripAEjecutar, True)
                        Return DateTime.MinValue
                     End If
                  End If

                  ''VALIDA FECHA MENOR A LA FECHA DE FINALIZACION DEL PASO SIGUIENTE
                  If pbValidaFechaFinalizaPasoSig And piPasoSiguiente <> 0 Then
                     Dim ldFechafinalizaPaso As DateTime = ObtenerFechaFinalizaPaso(piPasoSiguiente)

                     If Not IsNothing(ldFechafinalizaPaso) Then
                        If ldFecha.Date > ldFechafinalizaPaso.Date Then
                           imgImagenError.ImageUrl = Constantes.Imagenes.Fallo
                           Dim errores As New Entities.EtiquetaError(2163)
                           lblFecha.Visible = True
                           lblFecha.Text = errores.Descripcion.Replace("[FECHA]", ldFechafinalizaPaso.ToString("dd/MM/yyyy"))

                           ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", psScripAEjecutar, True)
                           Return DateTime.MinValue
                        End If
                     End If
                  End If
               End If
            End If
         End If
      End If

      imgImagenError.ImageUrl = Constantes.Imagenes.Aviso
      lblFecha.Visible = False

      Return ldFecha
   End Function


   ''' <summary>
   ''' Fecha del ''Paso 25
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnFechaPaso25_Click(sender As Object, e As EventArgs)
      ''Validar que este llena y sea correcta la fecha
      Dim lsFecha As String = txtFechaGeneral.Text.Trim()
      Dim ldFecha As DateTime

      ''Regresa DateTime.MinValue si alguna validacion falla
      ldFecha = ValidarFechaGeneral(lsFecha, lblFechaGeneral, imgErrorGeneral, "SolicitarFechaGeneral('" & Constantes.MensajesModal.FechaPaso25.Replace("[AREA]", Constantes.BuscarNombreArea(ppObjVisita.IdArea)) & "', 'btnFechaPaso25');", False, False, True, True, PasoProcesoVisita.Pasos.Veintiseis)
      If ldFecha = DateTime.MinValue Then
         Exit Sub
      End If

      Dim objUsuario As New Entities.Usuario()
      objUsuario = Session(Entities.Usuario.SessionID)

      Dim objNegVisita As NegocioVisita

      ''Objeto negocio visita
      If IsNothing(objUsuario) Or IsNothing(ppObjVisita) Then
         Exit Sub
      ElseIf ppObjVisita.IdVisitaGenerado <= 0 Then
         Exit Sub
      End If

      Dim objVisita As Visita = ppObjVisita

      objNegVisita = New NegocioVisita(objVisita, objUsuario, Server, txbComentarios.Text)

      ''Actualiza fecha para la reunion
      If AccesoBD.ActualizaFechaInicioVisita(objVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionVjp25, objVisita.SubVisitasSeleccionadas) Then

         ''Fecha reunion paso 25
         objVisita.Fecha_ReunionVoPaso25 = ldFecha

         objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                 True, Constantes.EstatusPaso.Respuesta_Afore_Notificada,
                                                                 True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                 True, Constantes.CORREO_ID_NOTIFICA_VO_VF_HUBO_RESPUESTA_AFORE,
                                                                 True, True, False, , , , , , , True)
      End If

      Response.Redirect("../Procesos/DetalleVisita.aspx")
   End Sub


   ''' <summary>
   ''' ''Paso 30
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnFechaPaso32_Click(sender As Object, e As EventArgs)
      ''Validar que este llena y sea correcta la fecha
      Dim lsFecha As String = txtFechaGeneral.Text.Trim()
      Dim ldFecha As DateTime

      ''Regresa DateTime.MinValue si alguna validacion falla
      ldFecha = ValidarFechaGeneral(lsFecha, lblFechaGeneral, imgErrorGeneral, "SolicitarFechaGeneral('" & Constantes.MensajesModal.FechaPaso32 & "', 'btnFechaPaso32');", False, False, True, True, PasoProcesoVisita.Pasos.TreintaYTres)
      If ldFecha = DateTime.MinValue Then
         Exit Sub
      End If

      Dim objUsuario As New Entities.Usuario()
      objUsuario = Session(Entities.Usuario.SessionID)

      Dim objNegVisita As NegocioVisita

      ''Objeto negocio visita
      If IsNothing(objUsuario) Or IsNothing(ppObjVisita) Then
         Exit Sub
      ElseIf ppObjVisita.IdVisitaGenerado <= 0 Then
         Exit Sub
      End If

      Dim objVisita As Visita = ppObjVisita

      objNegVisita = New NegocioVisita(objVisita, objUsuario, Server, txbComentarios.Text)

      ''Actualiza fecha para la reunion
      If AccesoBD.ActualizaFechaInicioVisita(objVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionVjp32, objVisita.SubVisitasSeleccionadas) Then

         If rdbRecurosRevocacion.Checked Or rdbJuicioNulidad.Checked Then
            objVisita.Fecha_ReunionVjPaso32 = ldFecha

            If btnFechaPaso32.CommandArgument = Constantes.EstatusPaso.Nulidad Then
               Dim objCorreoBD As New Entities.Correo(Constantes.CORREO_ID_NOTIFICA_VO_VF_PR_AFORE_SOLICITA_JUICIO_NULIDAD)

               objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                        True, btnFechaPaso32.CommandArgument,
                                                        True, 33, Constantes.EstatusPaso.EnRevisionEspera,
                                                        True, objCorreoBD, True, True, True, , , , , , , True)
            Else
               Dim objCorreoBD As New Entities.Correo(Constantes.CORREO_ID_NOTIFICA_VO_VF_PR_AFORE_SOLICITA_RECURSO_REVOCACION)

               objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                        True, btnFechaPaso32.CommandArgument,
                                                        True, 33, Constantes.EstatusPaso.EnRevisionEspera,
                                                        True, objCorreoBD, True, True, True, , , , , , , True)
            End If
         Else ''PASA EN AUTOMATICO AL 37
            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                     True, Constantes.EstatusPaso.Sin_respuesta,
                                                     True, 37, Constantes.EstatusPaso.EnRevisionEspera,
                                                     True, Constantes.CORREO_ID_NOTIFICA_VO_VF_PR_AFORE_SOLICITA_NINGUNO_DE_LOS_DOS, True, True, True, , , , , , , True)
         End If
      End If

      Response.Redirect("../Procesos/DetalleVisita.aspx")
   End Sub

   ''' <summary>
   ''' Preciona boton si en ''paso 25
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnFechaPaso25Ok_Click(sender As Object, e As EventArgs)
      If Not Fechas.Vacia(ppObjVisita.Fecha_ReunionVoPaso25) Then
         Dim ldVecFecha As String() = ppObjVisita.Fecha_ReunionVoPaso25.ToString("dd/MM/yyyy").Split("/")

         If ldVecFecha.Length = 3 Then
            Dim ldFecha As Date = New Date(ldVecFecha(2), ldVecFecha(1), ldVecFecha(0))

            ''Actualiza fecha para la reunion
            If AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionVjp25Confirmacion, ppObjVisita.SubVisitasSeleccionadas) Then
               Response.Redirect("../Procesos/DetalleVisita.aspx")
            End If
         End If
      End If
   End Sub

   Protected Sub btnFechaPaso25NoDos_Click(sender As Object, e As EventArgs)
      ''Actualiza fecha para la reunion
      If AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, Date.Now, Constantes.TipoFecha.FechaReunionVjp25NoSeRealizo, ppObjVisita.SubVisitasSeleccionadas) Then
         Response.Redirect("../Procesos/DetalleVisita.aspx")
      End If
   End Sub

   ''' <summary>
   ''' Paso 32 si
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnFechaPaso32Ok_Click(sender As Object, e As EventArgs)
      If Not Fechas.Vacia(ppObjVisita.Fecha_ReunionVjPaso32) Then
         Dim ldVecFecha As String() = ppObjVisita.Fecha_ReunionVjPaso32.ToString("dd/MM/yyyy").Split("/")

         If ldVecFecha.Length = 3 Then
            Dim ldFecha As Date = New Date(ldVecFecha(2), ldVecFecha(1), ldVecFecha(0))

            ''Actualiza fecha para la reunion
            If AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionVjp32Confirmacion, ppObjVisita.SubVisitasSeleccionadas) Then
               Response.Redirect("../Procesos/DetalleVisita.aspx")
            End If
         End If
      End If
   End Sub

   Protected Sub btnFechaPaso32NoDos_Click(sender As Object, e As EventArgs)
      ''Actualiza fecha para la reunion
      If AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, Date.Now, Constantes.TipoFecha.FechaReunionVjp32NoSeRealizo, ppObjVisita.SubVisitasSeleccionadas) Then
         Response.Redirect("../Procesos/DetalleVisita.aspx")
      End If
   End Sub

   ''' <summary>
   ''' Modifica la fecha del ''paso 25
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnFechaPaso25Conf_Click(sender As Object, e As EventArgs)
      ''Validar que este llena y sea correcta la fecha
      Dim lsFecha As String = txtFechaGeneral.Text.Trim()
      Dim ldFecha As DateTime

      ''Regresa DateTime.MinValue si alguna validacion falla
      ldFecha = ValidarFechaGeneral(lsFecha, lblFechaGeneral, imgErrorGeneral, "SolicitarFechaGeneral('" & Constantes.MensajesModal.FechaPaso25.Replace("[AREA]", Constantes.BuscarNombreArea(ppObjVisita.IdArea)) & "', 'btnFechaPaso25Conf');", False, False, True, True, PasoProcesoVisita.Pasos.Veintiseis)
      If ldFecha = DateTime.MinValue Then
         Exit Sub
      End If

      Dim objUsuario As New Entities.Usuario()
      objUsuario = Session(Entities.Usuario.SessionID)

      Dim objNegVisita As NegocioVisita

      ''Objeto negocio visita
      If IsNothing(objUsuario) Or IsNothing(ppObjVisita) Then
         Exit Sub
      ElseIf ppObjVisita.IdVisitaGenerado <= 0 Then
         Exit Sub
      End If

      Dim objVisita As Visita = ppObjVisita

      objNegVisita = New NegocioVisita(objVisita, objUsuario, Server, txbComentarios.Text)

      ''Actualiza fecha para la reunion
      If AccesoBD.ActualizaFechaInicioVisita(objVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionVjp25Confirmacion, objVisita.SubVisitasSeleccionadas) Then
         Response.Redirect("../Procesos/DetalleVisita.aspx")
      End If
   End Sub

   ''' <summary>
   ''' Modifica fecha ''paso 32
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnFechaPaso32Conf_Click(sender As Object, e As EventArgs)
      ''Validar que este llena y sea correcta la fecha
      Dim lsFecha As String = txtFechaGeneral.Text.Trim()
      Dim ldFecha As DateTime

      ''Regresa DateTime.MinValue si alguna validacion falla
      ldFecha = ValidarFechaGeneral(lsFecha, lblFechaGeneral, imgErrorGeneral, "SolicitarFechaGeneral('" & Constantes.MensajesModal.FechaPaso32 & "', 'btnFechaPaso32Conf');", False, False, True, True, PasoProcesoVisita.Pasos.TreintaYTres)
      If ldFecha = DateTime.MinValue Then
         Exit Sub
      End If

      Dim objUsuario As New Entities.Usuario()
      objUsuario = Session(Entities.Usuario.SessionID)

      Dim objNegVisita As NegocioVisita

      ''Objeto negocio visita
      If IsNothing(objUsuario) Or IsNothing(ppObjVisita) Then
         Exit Sub
      ElseIf ppObjVisita.IdVisitaGenerado <= 0 Then
         Exit Sub
      End If

      Dim objVisita As Visita = ppObjVisita

      objNegVisita = New NegocioVisita(objVisita, objUsuario, Server, txbComentarios.Text)

      ''Actualiza fecha para la reunion
      If AccesoBD.ActualizaFechaInicioVisita(objVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionVjp32Confirmacion, objVisita.SubVisitasSeleccionadas) Then
         Response.Redirect("../Procesos/DetalleVisita.aspx")
      End If
   End Sub


   Private Sub CambiaTituloModalSubvisitas(visita As Visita, objUsuario As Usuario)
      If objUsuario.IdArea = Constantes.AREA_VF Then
         If visita.IdPasoActual < 2 Then
            divSubVisitas.Attributes.Remove("Title")
            divSubVisitas.Attributes.Add("Title", "Seleccione las subvisitas para replicar la información")
         End If
      End If
   End Sub

   Private Sub LiberaVisita()
      If Not IsNothing(puObjUsuario) And Not IsNothing(ppObjVisita) Then
         If ppObjVisita.UsuarioEstaOcupando = puObjUsuario.IdentificadorUsuario Then
            AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, Date.Now, Constantes.TipoFecha.LimpiaBanderaDeApropiacion, "", puObjUsuario.IdentificadorUsuario)
            Session.Remove("DETALLE_VISITA")
         End If
      End If
   End Sub

   ''' <summary>
   ''' Libera la visita
   ''' </summary>
   ''' <remarks></remarks>
   <System.Web.Services.WebMethod()>
   Public Shared Function LiberaVisitaShared(idVisita As String, idUsuOcupa As String, idUsuLogin As String) As String
      Try
         If idUsuOcupa.Trim() <> "" And idUsuLogin.Trim() <> "" Then
            If idUsuOcupa.Trim() = idUsuLogin.Trim() Then
               AccesoBD.ActualizaFechaInicioVisita(idVisita, Date.Now, Constantes.TipoFecha.LimpiaBanderaDeApropiacion, "", idUsuLogin)
            End If
         End If
      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "DetalleVisita.aspx.vb, DetalleVisita-LiberaVisitaShared", "")
         Return ex.Message
      End Try

      Return "OK"
   End Function

   ''' <summary>
   ''' NOTA: SI SE MOFICA ESTE METODO MODIFICAR EL METODO ValidaPermisoCargaDocumentos del control de usuarios ucDocumentos.ascx.vb
   ''' </summary>
   ''' <param name="objUsuario"></param>
   ''' <param name="objVisita"></param>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Private Function HabilitarBotonesDetalle(objUsuario As Usuario, objVisita As Visita) As Boolean
      ''Ocultar botones
      imgAnterior.Visible = False
      imgIniciarVisita.Visible = False

      ''Objeto negocio visita
      Dim objNegVisita As New NegocioVisita(objVisita, objUsuario, Server, txbComentarios.Text)
      Dim lbTienePermiso As Boolean = False

      If objVisita.EsCancelada Then
         Return False
      End If

      Select Case objVisita.IdPasoActual
         Case 1
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.Registrado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgSiguiente.Visible = True
                           imgSiguiente.Enabled = True
                           lbTienePermiso = True
                        End If
                     Case Constantes.PERFIL_INS
                        lbTienePermiso = True
                  End Select
               Case Constantes.EstatusPaso.EnAjustes
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS ''Atiende observaciones VJ
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Enabled = True
                           imgGuardarCambios.Visible = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.AjustesRealizados ''Enviar nuevamente a VJ
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgSiguiente.Visible = True
                           imgSiguiente.Enabled = True
                           lbTienePermiso = True
                        End If
                     Case Constantes.PERFIL_INS
                        lbTienePermiso = True
                  End Select
            End Select

         Case 2
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.Enviado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM 'Administrador asigna supervisor y abogado asesor
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              HabilitarControlesAbogados(Constantes.ABOGADOS.PERFIL_ABO_ASESOR, Constantes.EstatusPaso.Enviado)

                              ''Mod agc
                              imgGuardarCambios.Enabled = True
                              imgGuardarCambios.Visible = True
                              lbTienePermiso = True
                        End Select
                  End Select
               Case Constantes.EstatusPaso.SupervisorAsignado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Supervisor asigna/modifica abogado asesor
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              HabilitarControlesAbogados(Constantes.ABOGADOS.PERFIL_ABO_ASESOR, Constantes.EstatusPaso.SupervisorAsignado)

                              ''Mod agc
                              imgGuardarCambios.Enabled = True
                              imgGuardarCambios.Visible = True
                              lbTienePermiso = True
                        End Select
                  End Select
               Case Constantes.EstatusPaso.AsesorAsignado ''Guarda despues de asignar asesor
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.ABOGADOS.PERFIL_ABO_ASESOR, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True

                              ''Nuevos botones
                              imgSiguiente.Visible = True
                              imgSiguiente.Enabled = True

                              imgAnterior.Visible = True
                              imgAnterior.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
               Case Constantes.EstatusPaso.Revisado ''revisaso y asesor asignado paso 2
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              ''Nuevos botones
                              imgSiguiente.Visible = True
                              imgSiguiente.Enabled = True

                              imgAnterior.Visible = True
                              imgAnterior.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
               Case Constantes.EstatusPaso.EnAjustes ''Guarda paso 2, EnAjustes
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True

                              ''Nuevos botones
                              imgSiguiente.Visible = True
                              imgSiguiente.Enabled = True

                              imgAnterior.Visible = True
                              imgAnterior.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
               Case Constantes.EstatusPaso.AjustesRealizados  ''Guarda paso 2, AjustesRealizados
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              ''Nuevos botones
                              imgSiguiente.Visible = True
                              imgSiguiente.Enabled = True

                              imgAnterior.Visible = True
                              imgAnterior.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select

         Case 3
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 3 guardar, en revision espera
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select

               Case Constantes.EstatusPaso.Revisado ''Notifica que ya todos los documentos estan bien, o rechaza paso 3
                  Select Case objUsuario.IdentificadorPerfilActual ''Rechaza los documentos que aprobo VJ paso 3
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgAnterior.Visible = True
                           imgAnterior.Enabled = True

                           imgNotificar3.Visible = True
                           imgNotificar3.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 4
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 4, EnRevisionEspera
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.Revisado  ''Paso 4, Revisado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgNotificar3.Visible = True
                           imgNotificar3.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 6
            Select Case objVisita.IdEstatusActual ''Paso 6 inicia visita
               Case Constantes.EstatusPaso.EnRevisionEspera
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                        'Ocultar tabla
                        divComentarios.Visible = False

                        tbDiasTranscurridosVisitaInSitu.Visible = True

                        lblDiasTranscurridosVisitaInSitu.Text = "0"

                        If EsAreaOperativa(objUsuario.IdArea) Or objUsuario.IdArea = Constantes.AREA_VJ Then
                           imgIniciarVisita.Enabled = True
                           imgIniciarVisita.Visible = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.Visita_iniciada ''Paso 6 detener visita
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                        'Ocultar tabla
                        divComentarios.Visible = False

                        tbDiasTranscurridosVisitaInSitu.Visible = True

                        lblDiasTranscurridosVisitaInSitu.Text = objVisita.DiasTranscurridosPasoActual

                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgDetener.Visible = True
                           imgDetener.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 7
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.En_diagnostico_de_hallazgos ''paso 7 En_diagnostico_de_hallazgos
                  'Ocultar tabla
                  divComentarios.Visible = False

                  tbDiasTranscurridosDiagnosticoHallazgos.Visible = True

                  lblDiasTranscurridosDiagnosticoHallazgos.Text = objVisita.DiasTranscurridosPasoActual

                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP  ' Solo el inspector puede: *adjuntar documentos (hallazgos de visita)
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgNotificar3.Visible = True
                           imgNotificar3.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 8
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 8, EnRevisionEspera
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.HallazgosGuardados ''Paso 8 HallazgosGuardados
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgNotificar3.Visible = True
                           imgNotificar3.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select

               Case Constantes.EstatusPaso.EnEsperaPresentarHallazgos
                  tbHallazgozInt.Visible = True
                  txtEditAllazgosInt.Text = Fechas.Valor(objVisita.FECH_REUNION__PRESIDENCIA)
                  ''Permitir editar la fecha de presentacion interna en el paso 8 a supervisor
                  If objVisita.IdPasoActual = 8 And
                      (objUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP Or objUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM) And
                      (objUsuario.IdArea <> Constantes.AREA_VJ) And
                      Not Fechas.Vacia(objVisita.FECH_REUNION__PRESIDENCIA) Then
                     btnEditAllazgosInt.Visible = True
                     btnEditAllazgosInt.Enabled = True
                  End If

                  ''Pasa en automatico al paso 9 una vez que la fecha de reunion llego
                  Dim fechaHoy As Date
                  fechaHoy = Date.Now

                  If Not IsNothing(objVisita.FechaReunionPresidencia) Then
                     Dim fechaReunion As Date = objVisita.FechaReunionPresidencia

                     If fechaHoy.Date > fechaReunion.Date Then
                        If objVisita.TieneSancion Then
                           objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                        True, Constantes.EstatusPaso.Hallazgos_presentados,
                                                        False, -1, -1,
                                                        False, -1, , , , , , , , , objVisita.FechaReunionPresidencia)

                           objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                            False, -1,
                                                            True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                            False, -1, , , , , , , , , AccesoBD.ObtenerFecha(fechaReunion.Date, 1, Constantes.IncremeteDecrementa.Incrementa))
                        Else
                           ''Manda correo a sandra pacheco, configurada en parametros
                           If MandaCorreoSandraPachecoPaso12(objNegVisita) Then
                              objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                      True, Constantes.EstatusPaso.Hallazgos_presentados,
                                                      False, -1, -1,
                                                      False, -1, , , , , , , , , objVisita.FechaReunionPresidencia)
                              ''INICIA EL PASO 13
                              objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                               False, -1,
                                                               True, 13, Constantes.EstatusPaso.EnRevisionEspera,
                                                               False, -1, , , , , , , , , AccesoBD.ObtenerFecha(fechaReunion.Date, 1, Constantes.IncremeteDecrementa.Incrementa).Date)
                           End If
                        End If

                        Response.Redirect("../Procesos/DetalleVisita.aspx")
                     End If
                  End If
               Case Constantes.EstatusPaso.SinReunionPresidencia ''Paso 8 SinReunionPresidencia
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgSiguiente.Visible = True
                           imgSiguiente.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 9
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP 'adjuntar los documentos
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.Elaborada
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP  'envia al paso 10
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgSiguiente.Visible = True
                           imgSiguiente.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.EnAjustes
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP 'adjuntar los documentos
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.AjustesRealizados
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP  'envia al paso 10
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgSiguiente.Visible = True
                           imgSiguiente.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 10
            Select Case objVisita.IdEstatusActual ''Paso 10 EnRevisionEspera, EnAjustes
               Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.EnAjustes
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select

               Case Constantes.EstatusPaso.Revisado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Una vez revisado supervisor rechaza o avanza
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgSiguiente.Visible = True
                              imgSiguiente.Enabled = True

                              imgAnterior.Visible = True
                              imgAnterior.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
               Case Constantes.EstatusPaso.AjustesRealizados
                  Select Case objUsuario.IdentificadorPerfilActual ''Paso 10 AjustesRealizados
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Una vez revisado supervisor rechaza o avanza
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgSiguiente.Visible = True
                              imgSiguiente.Enabled = True

                              imgAnterior.Visible = True
                              imgAnterior.Enabled = True

                              lbTienePermiso = True
                        End Select
                  End Select
            End Select

         Case 12
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 12 rechaza
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP  'Supervisor avisa de la version final de los documentos
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgAnterior.Visible = True
                           imgAnterior.Enabled = True

                           imgNotificar3.Visible = True
                           imgNotificar3.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 13
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS ''adjunta documentos del paso 13
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select

               Case Constantes.EstatusPaso.Hallazgos_presentados
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Manda fecha que confirmo sandra pacheco
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgNotificar3.Visible = True
                           imgNotificar3.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.Notificado ''Paso automatico, cuando alguin consulta la visita y ya esta en la fecha de reunion afore, pasa la visita al paso 14
                  tbHallazgozExt.Visible = True
                  txtEditAllazgosExt.Text = Fechas.Valor(objVisita.FECH_REUNION__AFORE)

                  ''Permitir editar la fecha de presentacion externa en el paso 8 a supervisor
                  If objVisita.IdPasoActual = 13 And
                      (objUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP Or objUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM) And
                      (objUsuario.IdArea <> Constantes.AREA_VJ) And
                      Not Fechas.Vacia(objVisita.FECH_REUNION__AFORE) Then
                     btnEditAllazgosExt.Visible = True
                     btnEditAllazgosExt.Enabled = True
                  End If

                  ''Pasa en automatico al paso 14 una vez que la fecha de reunion llego
                  Dim fechaHoy As Date
                  fechaHoy = Date.Now

                  If Not IsNothing(objVisita.FechaReunionAfore) Then
                     Dim fechaReunionAfore As Date = objVisita.FechaReunionAfore

                     If fechaHoy.Date > fechaReunionAfore.Date Then
                        If objVisita.TieneSancion Then
                           objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                        True, Constantes.EstatusPaso.Notificado,
                                                        True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                        False, -1, , , , , , , , , objVisita.FechaReunionAfore)
                        Else
                           objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                       True, Constantes.EstatusPaso.Notificado,
                                                       False, -1, -1,
                                                       False, -1, , , , , , , , , objVisita.FechaReunionAfore)

                           objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                               False, -1,
                                                               True, 16, Constantes.EstatusPaso.EnRevisionEspera,
                                                               False, -1, , , , , , , , , AccesoBD.ObtenerFecha(fechaReunionAfore, 1, Constantes.IncremeteDecrementa.Incrementa).Date)
                        End If

                        Response.Redirect("../Procesos/DetalleVisita.aspx")
                     End If
                  End If
            End Select

         Case 14
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.Registrado
                  Select Case objUsuario.IdentificadorPerfilActual ''Solicitar fecha acta circunstanciada in situ
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgNotificar3.Visible = True
                           imgNotificar3.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 16

            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.EnAjustes ''Flujo inicial paso 16, ''Flujo 2 paso 16, EnAjustes
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.Revisado, Constantes.EstatusPaso.AjustesRealizados ''Flujo inicial paso 16, revisado, ''Flujo 2 paso 16, AjustesRealizados
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgSiguiente.Visible = True
                           imgSiguiente.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 17
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.EnAjustes ''Flujo inicial paso 17, EnRevisionEspera, ''Flujo 2 paso 17, EnAjustes
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_ASESOR, Constantes.PERFIL_INS 'El Supervisor / Superior  puede:  *Enviar documentos a revisión y por lo tanto dar vo.bo, el abogado e inspector pueden adjuntar documentos (hacer ajustes a los docs)
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
               Case Constantes.EstatusPaso.Revisado, Constantes.EstatusPaso.AjustesRealizados ''Flujo inicial paso 17, Revisado, ''Flujo inicial paso 17, Revisado, regresa paso 16
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Flujo 2 paso 17, AjustesRealizados, ''Flujo 2 paso 17 regresa paso 16, AjustesRealizados
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgSiguiente.Visible = True
                              imgSiguiente.Enabled = True

                              imgAnterior.Visible = True
                              imgAnterior.Enabled = True

                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True

                              lbTienePermiso = True
                        End Select
                  End Select
            End Select

         Case 18
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Flujo inicial paso 18, EnRevisionEspera
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.Revisado ''Flujo inicial paso 18, Revisado, ''Flujo inicial paso 18, Revisado, regresa paso 17
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgNotificar3.Visible = True
                           imgNotificar3.Enabled = True

                           imgAnterior.Visible = True
                           imgAnterior.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 19
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS  ''Paso 19 guardar comentarios
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.Revisado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP ''Paso 19 manda a paso 20
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgNotificar3.Visible = True
                           imgNotificar3.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 20
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera 'Tiene que asignar el abogado que asesorara en la sancion
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              HabilitarControlesAbogados(Constantes.ABOGADOS.PERFIL_ABO_SANCIONES, Constantes.EstatusPaso.EnRevisionEspera)

                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                     Case Constantes.ABOGADOS.PERFIL_ABO_ASESOR
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
               Case Constantes.EstatusPaso.SupervisorAsignado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Supervisor asigna/modifica abogado sancion
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              HabilitarControlesAbogados(Constantes.ABOGADOS.PERFIL_ABO_SANCIONES, Constantes.EstatusPaso.SupervisorAsignado)
                              imgGuardarCambios.Enabled = True
                              imgGuardarCambios.Visible = True
                              lbTienePermiso = True
                        End Select
                  End Select
               Case Constantes.EstatusPaso.Asignado  ''Paso 20 pasa a paso 21, Asignado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select

               Case Constantes.EstatusPaso.Revisado   ''Paso 20 pasa a paso 21, Revisado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgSiguiente.Visible = True
                              imgSiguiente.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select

               Case Constantes.EstatusPaso.AjustesRealizados  ''Paso 20 pasa a paso 21, AjustesRealizados
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgSiguiente.Visible = True
                              imgSiguiente.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select

               Case Constantes.EstatusPaso.EnAjustes  ''Regresa de paso 21, EnAjustes
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select

         Case 21
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 21 guarda comentarios, EnRevisionEspera
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.Revisado ''Paso 21 guarda comentarios, Revisado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then

                           ''Validar que si no e ha dado respuesta en 3 dias, aprobar automaticamente
                           If objVisita.FechaInicioPasoActual <> DateTime.MinValue Then
                              Dim ldFechaAux As DateTime = AccesoBD.ObtenerFecha(objVisita.FechaInicioPasoActual, 3, Constantes.IncremeteDecrementa.Incrementa)

                              If DateTime.Now.Date >= ldFechaAux.Date Then
                                 objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                         True, Constantes.EstatusPaso.Aprobado,
                                                         True, (objVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                         True, Constantes.CORREO_ID_NOTIFICA_APRUEBA_OF_EMPLAZAMIENTO,
                                                         True, True, , , , , , , , True)
                                 Response.Redirect("../Procesos/DetalleVisita.aspx")
                              End If
                           End If


                           imgSiguiente.Visible = True
                           imgSiguiente.Enabled = True

                           imgAnterior.Visible = True
                           imgAnterior.Enabled = True

                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 22
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Notifica fecha de posible emplazamiento
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              ''Validar que en el dia 2 del paso actual se haimgSolicitarProrrogabilita el boton de notificar
                              'If visita.FechaInicioPasoActual <> DateTime.MinValue Then
                              '    Dim ldFechaAux As DateTime = visita.FechaInicioPasoActual

                              '    If DateTime.Now.Date >= ldFechaAux.Date.AddDays(2) Then
                              '        imgNotificar3.Visible = True
                              '        imgNotificar3.Enabled = True
                              '    End If
                              'End If

                              imgNotificar3.Visible = True
                              imgNotificar3.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select

         Case 23
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 23
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select

         Case 24
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 24
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select

         Case 25
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 25
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgNotificar3.Visible = True
                              imgNotificar3.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select

         Case 26
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 26, Elaborada
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                        If EsAreaOperativa(objUsuario.IdArea) Or objUsuario.IdArea = Constantes.AREA_VJ Then
                           lbTienePermiso = True

                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                        End If
                  End Select
            End Select

         Case 27
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 27, Notificado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgNotificar3.Visible = True
                              imgNotificar3.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select

         Case 28
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 28, Impuesto
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgNotificar3.Visible = True
                              imgNotificar3.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select

         Case 29
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 29, Revisado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
               Case Constantes.EstatusPaso.Revisado   ''Paso 29, Pagado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgNotificar3.Visible = True
                              imgNotificar3.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select

         Case 30
            'Ocultar tabla
            divComentarios.Visible = False

            tbDiasTranscurridosPosterioresPago.Visible = True

            lblDiasTranscurridosPosterioresPago.Text = objVisita.DiasTranscurridosPasoActual

            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 30
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              HabilitarControlesAbogados(Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO, Constantes.EstatusPaso.EnRevisionEspera)

                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select

                  End Select
               Case Constantes.EstatusPaso.SupervisorAsignado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP 'Supervisor asigna/modifica abogado contencioso
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              HabilitarControlesAbogados(Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO, Constantes.EstatusPaso.SupervisorAsignado)
                              imgGuardarCambios.Enabled = True
                              imgGuardarCambios.Visible = True
                              lbTienePermiso = True
                        End Select
                  End Select
               Case Constantes.EstatusPaso.Asignado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              tbAforeSolicita.Visible = True

                              rdbNinguno.Visible = True
                              rdbNinguno.Enabled = True

                              If objVisita.DiasTranscurridosPasoActual <= 15 Then
                                 rdbRecurosRevocacion.Visible = True
                                 rdbRecurosRevocacion.Enabled = True
                              ElseIf objVisita.DiasTranscurridosPasoActual >= 16 And objVisita.DiasTranscurridosPasoActual <= 45 Then
                                 objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                          True, Constantes.EstatusPaso.Sin_respuesta,
                                                                          True, 37, Constantes.EstatusPaso.EnRevisionEspera,
                                                                          True, Constantes.CORREO_ID_NOTIFICA_VO_VF_PR_AFORE_SOLICITA_NINGUNO_DE_LOS_DOS, True, True, True, , , , , , , True)
                                 Response.Redirect("../Procesos/DetalleVisita.aspx")
                              End If

                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select
         Case 31 ''Paso 31
            'Ocultar tabla
            divComentarios.Visible = False

            tbDiasTranscurridosPosterioresPago.Visible = True

            lblDiasTranscurridosPosterioresPago.Text = objVisita.DiasTranscurridosPasoActual

            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              tbAforeSolicita.Visible = True

                              rdbNinguno.Visible = True
                              rdbNinguno.Enabled = True

                              If objVisita.DiasTranscurridosPasoActual >= 16 And objVisita.DiasTranscurridosPasoActual <= 45 Then
                                 rdbJuicioNulidad.Visible = True
                                 rdbJuicioNulidad.Enabled = True
                              Else
                                 objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, objVisita.IdEstatusActual,
                                                                          True, Constantes.EstatusPaso.Sin_respuesta,
                                                                          True, 37, Constantes.EstatusPaso.EnRevisionEspera,
                                                                          True, Constantes.CORREO_ID_NOTIFICA_VO_VF_PR_AFORE_SOLICITA_NINGUNO_DE_LOS_DOS, True, True, True, , , , , , , True)
                                 Response.Redirect("../Procesos/DetalleVisita.aspx")
                              End If

                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select

         Case 32
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.Revocacion, Constantes.EstatusPaso.Nulidad, Constantes.EstatusPaso.Sin_respuesta
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_INS, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                        If EsAreaOperativa(objUsuario.IdArea) Or objUsuario.IdArea = Constantes.AREA_VJ Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 33
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera, Constantes.EstatusPaso.EnAjustes ''Paso 33, EnRevisionEspera, Paso 33, EnAjustes
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
               Case Constantes.EstatusPaso.Respuesta_Elaborada, Constantes.EstatusPaso.AjustesRealizados ''Paso 33, Respuesta_Elaborada, Paso 33, AjustesRealizados
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgSiguiente.Visible = True
                              imgSiguiente.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select

         Case 35
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera  ''Paso 35, EnRevisionEspera
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_INS
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True

                           imgSiguiente.Visible = True
                           imgSiguiente.Enabled = True ''Paso 35, EnRevisionEspera, acepta

                           imgAnterior.Visible = True ''Paso 35, EnRevisionEspera, rechaza
                           imgAnterior.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.Revisado  ''Paso 35, Revisado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgSiguiente.Visible = True
                           imgSiguiente.Enabled = True ''Paso 35, Revisado, acepta

                           imgAnterior.Visible = True ''Paso 35, Revisado, rechaza
                           imgAnterior.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.EnAjustes  ''Paso 35, EnAjustes
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_INS
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True
                           lbTienePermiso = True
                        End If
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgGuardarCambios.Visible = True
                           imgGuardarCambios.Enabled = True

                           imgSiguiente.Visible = True
                           imgSiguiente.Enabled = True ''Paso 35, EnAjustes, acepta

                           imgAnterior.Visible = True ''Paso 35, EnAjustes, rechaza
                           imgAnterior.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
               Case Constantes.EstatusPaso.AjustesRealizados  ''Paso 35, AjustesRealizados
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        If EsAreaOperativa(objUsuario.IdArea) Then
                           imgSiguiente.Visible = True
                           imgSiguiente.Enabled = True ''Paso 35, AjustesRealizados, acepta

                           imgAnterior.Visible = True ''Paso 35, AjustesRealizados, rechaza
                           imgAnterior.Enabled = True
                           lbTienePermiso = True
                        End If
                  End Select
            End Select

         Case 36
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 36, EnRevisionEspera
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP, Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgGuardarCambios.Visible = True
                              imgGuardarCambios.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
               Case Constantes.EstatusPaso.Revisado ''Paso 36, Revisado
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgAnterior.Visible = True  ''Paso 36, Revisado, rechaza
                              imgAnterior.Enabled = True

                              imgNotificar3.Visible = True ''Paso 36, Revisado, notifica
                              imgNotificar3.Enabled = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select
         Case 37
            Select Case objVisita.IdEstatusActual
               Case Constantes.EstatusPaso.EnRevisionEspera ''Paso 37
                  Select Case objUsuario.IdentificadorPerfilActual
                     Case Constantes.PERFIL_ADM, Constantes.PERFIL_SUP
                        Select Case objUsuario.IdArea
                           Case Constantes.AREA_PR, Constantes.AREA_VJ
                              imgNotificar3.Enabled = True
                              imgNotificar3.Visible = True
                              lbTienePermiso = True
                        End Select
                  End Select
            End Select
      End Select

      Return lbTienePermiso
   End Function

   Private Sub HabilitarControlesAbogados(piPerfilAbogado As Integer, piPerfilUsuLogueado As Constantes.EstatusPaso)
      t_ddlAbogadoAsignado.Visible = True

      Dim lstAboSup As List(Of Abogado)
      Dim lstAboOp As List(Of Abogado)

      Select Case piPerfilAbogado
         Case Constantes.ABOGADOS.PERFIL_ABO_ASESOR
            lblAsignaAbogado.Text = "Asesor(es):"
            lstAboSup = ppObjVisita.LstAbogadosSupAsesorAsig
            lstAboOp = ppObjVisita.LstAbogadosAsesorAsignados
         Case Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
            lblAsignaAbogado.Text = "Sanciones(es):"
            lstAboSup = ppObjVisita.LstAbogadosSupSancionAsig
            lstAboOp = ppObjVisita.LstAbogadosSancionAsignados
         Case Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
            lblAsignaAbogado.Text = "Contencioso(es):"
            lstAboSup = ppObjVisita.LstAbogadosSupContenAsig
            lstAboOp = ppObjVisita.LstAbogadosContenAsignados
         Case Else
            lstAboSup = New List(Of Abogado)
            lstAboOp = New List(Of Abogado)
      End Select

      Utilerias.ControlErrores.EscribirEvento("Inicia la carga de abogados disponibles.", EventLogEntryType.Information)

      ''LLena los abogados supervisores disponibles
      lsbUsuariosDisponibles.DataSource = AccesoBD.ObtenerUsuarioInvolucradosVisitaDS(Constantes.AREA_VJ, Constantes.PERFIL_SUP)

      lsbUsuariosDisponibles.DataTextField = "NOMBRE_COMPLETO"
      lsbUsuariosDisponibles.DataValueField = "USUARIO"
      lsbUsuariosDisponibles.DataBind()

      ''LLena los abogados operativos disponibles
      lsbAbogadoDisponibles.DataSource = AccesoBD.ObtenerUsuarioInvolucradosVisitaDS(Constantes.AREA_VJ, piPerfilAbogado)

      lsbAbogadoDisponibles.DataTextField = "NOMBRE_COMPLETO"
      lsbAbogadoDisponibles.DataValueField = "USUARIO"
      lsbAbogadoDisponibles.DataBind()


      ''Validar si ya se tiene un abogado de la visita padre
      Dim liItem As New ListItem

      For Each liItemIns As Abogado In lstAboSup
         liItem = lsbUsuariosDisponibles.Items.FindByValue(liItemIns.Id)
         If Not IsNothing(liItem) Then
            lsbSupervisor.Items.Add(liItem)
            lsbUsuariosDisponibles.Items.Remove(liItem)
         End If
      Next

      For Each liItemIns As Abogado In lstAboOp
         liItem = lsbAbogadoDisponibles.Items.FindByValue(liItemIns.Id)
         If Not IsNothing(liItem) Then
            lsbAbogado.Items.Add(liItem)
            lsbAbogadoDisponibles.Items.Remove(liItem)
         End If
      Next

      Utilerias.ControlErrores.EscribirEvento("Termina la carga de abogados disponibles de forma correcta. Núm abogados: " + lsbUsuariosDisponibles.Items.Count.ToString(), EventLogEntryType.Information)

      lsbUsuariosDisponibles.Enabled = True : lsbUsuariosDisponibles.Visible = True
      imgAsignarSupervisor.Enabled = True : imgAsignarSupervisor.Visible = True
      lsbSupervisor.Enabled = True : lsbSupervisor.Visible = True
      imgDesasignarSupervisor.Enabled = True : imgDesasignarSupervisor.Visible = True

      lsbAbogadoDisponibles.Enabled = True : lsbAbogadoDisponibles.Visible = True
      imgAsignarAbogado.Enabled = True : imgAsignarAbogado.Visible = True
      lsbAbogado.Enabled = True : lsbAbogado.Visible = True
      imgDesasignarAbogado.Enabled = True : imgDesasignarAbogado.Visible = True

      ''Ovulta los controles ya que, si esta en este estatus es porque ya se han asignado supervisores
      If piPerfilUsuLogueado = Constantes.EstatusPaso.SupervisorAsignado Then
         trRenSupTit.Visible = False
         trRenSupUsu.Visible = False
         trRenSupFle.Visible = False
      End If
   End Sub

   Public Function GetUsuariosSeleccionados(piIdPerfilUsuAct As Integer, piPerfilAbogadoOp As Integer, ByRef lsSupSelec As String) As String
      Dim lsAboSup As String = ""
      Dim objAbog As Abogado
      Dim lstAboSup As New List(Of Abogado)
      Dim lstAboOp As New List(Of Abogado)
      Dim lsErrores As String = "<ul>"

      If piIdPerfilUsuAct = Constantes.PERFIL_ADM Then
         If lsbSupervisor.Items.Count > 0 Then
            For indice As Integer = 0 To lsbSupervisor.Items.Count - 1
               objAbog = New Abogado
               objAbog.Id = lsbSupervisor.Items(indice).Value
               objAbog.Nombre = lsbSupervisor.Items(indice).Text

               lstAboSup.Add(objAbog)
               lsAboSup &= lsbSupervisor.Items(indice).Value & "|" & lsbSupervisor.Items(indice).Text & ","
            Next
         Else
            lsErrores &= "<li> Debes seleccionar algún abogado supervisor </li>"
         End If

         If lsAboSup.Length > 0 Then
            lsSupSelec = lsAboSup.Substring(0, lsAboSup.Length - 1)
            lsAboSup = ""
         Else
            lsSupSelec = ""
            lsAboSup = ""
         End If
      End If

      If lsbAbogado.Items.Count > 0 Then
         For indice As Integer = 0 To lsbAbogado.Items.Count - 1
            objAbog = New Abogado
            objAbog.Id = lsbAbogado.Items(indice).Value
            objAbog.Nombre = lsbAbogado.Items(indice).Text

            lstAboOp.Add(objAbog)

            lsAboSup &= lsbAbogado.Items(indice).Value & "|" & lsbAbogado.Items(indice).Text & ","
         Next
      Else
         If piIdPerfilUsuAct <> Constantes.PERFIL_ADM Then
            lsErrores &= "<li> Debes seleccionar algún abogado operativo  </li>"
         End If
      End If

      Select Case piPerfilAbogadoOp
         Case Constantes.ABOGADOS.PERFIL_ABO_ASESOR
            ppObjVisita.LstAbogadosSupAsesorAsig = lstAboSup
            ppObjVisita.LstAbogadosAsesorAsignados = lstAboOp
         Case Constantes.ABOGADOS.PERFIL_ABO_SANCIONES
            ppObjVisita.LstAbogadosSupSancionAsig = lstAboSup
            ppObjVisita.LstAbogadosSancionAsignados = lstAboOp
         Case Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO
            ppObjVisita.LstAbogadosSupContenAsig = lstAboSup
            ppObjVisita.LstAbogadosContenAsignados = lstAboOp
      End Select

      If lsAboSup.Length > 0 Then
         lsAboSup = lsAboSup.Substring(0, lsAboSup.Length - 1)
      End If

      lsErrores &= "</ul>"

      If lstAboSup.Count <= 0 And lstAboOp.Count <= 0 Then
         Mensaje = lsErrores
         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AquiMuestroMensaje();", True)
      End If

      Return lsAboSup
   End Function

   Protected Sub imgAsignarSupervisor_Click(sender As Object, e As ImageClickEventArgs) Handles imgAsignarSupervisor.Click
      agregarUsuarioSupervisor()
   End Sub

   Protected Sub imgDesasignarSupervisor_Click(sender As Object, e As ImageClickEventArgs) Handles imgDesasignarSupervisor.Click
      quitarSupervisor()
   End Sub

   Protected Sub imgAsignarAbogado_Click(sender As Object, e As ImageClickEventArgs) Handles imgAsignarAbogado.Click
      agregarUsuarioInspector()
   End Sub

   Protected Sub imgDesasignarAbogado_Click(sender As Object, e As ImageClickEventArgs) Handles imgDesasignarAbogado.Click
      quitarInspector()
   End Sub

   Private Sub agregarUsuarioSupervisor()
      Try
         If lsbUsuariosDisponibles.SelectedIndex > -1 Then
            Dim item As System.Web.UI.WebControls.ListItem = lsbUsuariosDisponibles.SelectedItem
            If Not lsbSupervisor.Items.Contains(item) Then
               lsbSupervisor.Items.Add(item)
            End If
            item.Selected = False
            lsbUsuariosDisponibles.Items.Remove(item)
         End If

      Catch ex As Exception
      End Try
   End Sub

   Private Sub agregarUsuarioInspector()
      Try
         If lsbAbogadoDisponibles.SelectedIndex > -1 Then
            Dim item As System.Web.UI.WebControls.ListItem = lsbAbogadoDisponibles.SelectedItem
            If Not lsbAbogado.Items.Contains(item) Then
               lsbAbogado.Items.Add(item)
            End If
            item.Selected = False
            lsbAbogadoDisponibles.Items.Remove(item)
         End If

      Catch ex As Exception
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
         End If

      Catch ex As Exception
      End Try
   End Sub

   Private Sub quitarInspector()
      Try
         If lsbAbogado.SelectedIndex > -1 Then
            Dim item As System.Web.UI.WebControls.ListItem = lsbAbogado.SelectedItem

            lsbAbogadoDisponibles.Items.Insert(0, item)
            Dim lstAnterior As New ListBox
            lstAnterior.Items.AddRange((From ltItem As ListItem In lsbAbogadoDisponibles.Items Order By ltItem.Value).ToArray())

            lsbAbogadoDisponibles.Items.Clear()
            lsbAbogadoDisponibles.Items.AddRange((From ltItem As ListItem In lstAnterior.Items Select ltItem).ToArray())

            item.Selected = False
            lsbAbogado.Items.Remove(item)
         End If

      Catch ex As Exception
      End Try
   End Sub

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

   ''' <summary>
   ''' Cuando se tienen que aceptar o rechazar documentos se reemplaza la imagen del boton aceptar y rechazar por otra.
   ''' </summary>
   ''' <remarks></remarks>
   Private Sub ReemplazarImgBotonSiguienteRechazar()
      Dim lstPasosS As List(Of Paso) = Constantes.GetListaPasosBtnSig()
      ''Dim lstPasosR As List(Of Paso) = Constantes.GetListaPasosBtnRechazar() ''Esta se cambia de raiz el icono

      ''Validar si se debe de cambiar la imagen del boton siguiente
      If (From objPaso In lstPasosS Where objPaso.IdPaso = ppObjVisita.IdPasoActual And objPaso.EstatusPaso = ppObjVisita.IdEstatusActual Select objPaso).Count() > 0 Then
         imgSiguiente.ImageUrl = "../Imagenes/AprobarDocto.png"
         imgSiguiente.ToolTip = "Aprobar Documentos"
      End If

      ''Validar si se debe de cambiar la imagen del boton rechazar
      'If (From objPaso In lstPasosR Where objPaso.IdPaso = ppObjVisita.IdPasoActual And objPaso.EstatusPaso = ppObjVisita.IdEstatusActual Select objPaso).Count() > 0 Then
      '    imgAnterior.ImageUrl = "../Imagenes/RechazarDocto.png"
      'End If
   End Sub

   ''' <summary>
   ''' ''paso 7 En_diagnostico_de_hallazgos, SI
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnPasoSieteConfirmSi_Click(sender As Object, e As EventArgs)
      ''Objeto negocio visita

      If Not IsNothing(ppObjVisita) And Not IsNothing(puObjUsuario) Then
         Dim objNegVisita As New NegocioVisita(ppObjVisita, puObjUsuario, Server, txbComentarios.Text)

         If AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, Date.Now, Constantes.TipoFecha.BanderaDeReunionPaso8, ppObjVisita.SubVisitasSeleccionadas) Then
            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, ppObjVisita.IdEstatusActual,
                                                                True, Constantes.EstatusPaso.Hallazgos_presentados_notificado,
                                                                True, (ppObjVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                False, -1)

            ''Manda correo a sandra pacheco, configurada en parametros
            Dim lstPersonasDestinatarios As New List(Of Persona)
            Dim dt As DataTable = Conexion.SQLServer.Parametro.ObtenerValores(Constantes.Parametros.SandraPacheco)

            ''Conexion.SQLServer
            If dt.Rows.Count > 0 Then
               lstPersonasDestinatarios.Add(New Persona With {.Nombre = Constantes.Parametros.SandraPacheco, .Correo = dt.Rows(0)("T_DSC_VALOR")})
            End If

            ''Notifica si todo salio bien, a area VJ y a presidencia
            If Constantes.CORREO_ENVIADO_OK = objNegVisita.getObjNotificacion().NotificarCorreo(Constantes.CORREO_FINALIZA_DIAG_HALLAZGOS, ppObjVisita, True, True, True, lstPersonasDestinatarios, True) Then
               AccesoBD.actualizarPasoNotificadoSinTransaccion(ppObjVisita.IdVisitaGenerado, ppObjVisita.IdPasoActual, True, ppObjVisita.Usuario.IdArea, objNegVisita.getObjNotificacion().getDestinatariosNombre(), objNegVisita.getObjNotificacion().getDestinatariosCorreo(), DateTime.Now)
            End If

            Response.Redirect("../Procesos/DetalleVisita.aspx")
         End If
      End If
   End Sub

   ''' <summary>
   ''' ''paso 7 En_diagnostico_de_hallazgos, NO
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks>AGC PREGUNTAR POR LA SANCION, SI SI HAY MANDAR A PASO 9, SI NO HAY MANDAR A PASO 16</remarks>
   Protected Sub btnPasoSieteConfirmNo_Click(sender As Object, e As EventArgs)
      Me.Mensaje = "¿Existe sanción para la visita?"
      ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "PreguntarSancionPasoSiete();", True)
      Exit Sub
   End Sub

   ''' <summary>
   ''' Manda a paso 9 con sancion, ''paso 7 En_diagnostico_de_hallazgos con sancion
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnConfSanPasoSieteSi_Click(sender As Object, e As EventArgs)
      If Not IsNothing(ppObjVisita) And Not IsNothing(puObjUsuario) Then
         Dim objNegVisita As New NegocioVisita(ppObjVisita, puObjUsuario, Server, txbComentarios.Text)

         objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, ppObjVisita.IdEstatusActual,
                                                                 True, Constantes.EstatusPaso.Hallazgos_presentados_notificado,
                                                                 True, 9, Constantes.EstatusPaso.EnRevisionEspera,
                                                                 False, -1)
         Response.Redirect("../Procesos/DetalleVisita.aspx")
      End If
   End Sub


   ''' <summary>
   ''' Manda a paso 16 sin sancion, ''paso 7 En_diagnostico_de_hallazgos sin sancion
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnConfSanPasoSieteNo_Click(sender As Object, e As EventArgs)
      If Not IsNothing(ppObjVisita) And Not IsNothing(puObjUsuario) Then
         If AccesoBD.ActualizaSancionVisita(ppObjVisita.IdVisitaGenerado, ppObjVisita.IdPasoActual) Then
            Dim objNegVisita As New NegocioVisita(ppObjVisita, puObjUsuario, Server, txbComentarios.Text)

            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, ppObjVisita.IdEstatusActual,
                                                                    True, Constantes.EstatusPaso.Hallazgos_presentados_notificado,
                                                                    True, 16, Constantes.EstatusPaso.EnRevisionEspera,
                                                                    False, -1)
            Response.Redirect("../Procesos/DetalleVisita.aspx")
         End If
      End If
   End Sub

   ''' <summary>
   ''' ''Paso 8 SinReunionPresidencia, SI, Avanza al paso 9
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnSancionPaso8Si_Click(sender As Object, e As EventArgs)
      If Not IsNothing(ppObjVisita) And Not IsNothing(puObjUsuario) Then
         Dim objNegVisita As New NegocioVisita(ppObjVisita, puObjUsuario, Server, txbComentarios.Text)

         objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, ppObjVisita.IdEstatusActual,
                                                                 True, Constantes.EstatusPaso.Hallazgos_presentados_notificado,
                                                                 True, (ppObjVisita.IdPasoActual + 1), Constantes.EstatusPaso.EnRevisionEspera,
                                                                 False, -1)
         Response.Redirect("../Procesos/DetalleVisita.aspx")
      End If
   End Sub

   ''' <summary>
   ''' ''Paso 8 SinReunionPresidencia, NO
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnSancionPaso8No_Click(sender As Object, e As EventArgs)
      If Not IsNothing(ppObjVisita) And Not IsNothing(puObjUsuario) Then
         If AccesoBD.ActualizaSancionVisita(ppObjVisita.IdVisitaGenerado, ppObjVisita.IdPasoActual) Then
            Dim objNegVisita As New NegocioVisita(ppObjVisita, puObjUsuario, Server, txbComentarios.Text)

            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, ppObjVisita.IdEstatusActual,
                                                                    True, Constantes.EstatusPaso.Hallazgos_presentados_notificado,
                                                                    True, 16, Constantes.EstatusPaso.EnRevisionEspera,
                                                                    False, -1)

            Response.Redirect("../Procesos/DetalleVisita.aspx")
         End If
      End If
   End Sub

   Protected Sub btnConfirmacionVulnerabilidadSi_Click(sender As Object, e As EventArgs) Handles btnConfirmacionVulnerabilidadSi.Click
      Dim objNegVisita As New NegocioVisita(ppObjVisita, puObjUsuario, Server, txbComentarios.Text)
      'If ppObjVisita.IdPasoActual = 1 Then

      If btnConfirmacionVulnerabilidadSi.CommandArgument = "EnviarConcluido" Then
         ScriptManager.RegisterStartupScript(Me, GetType(Page), "HOLA1", "SolicitarFechaVulnerabilidadRegresa();", True)
      Else
         Dim conex = New Conexion.SQLServer()
         Dim query As String = "UPDATE BDS_D_VS_VISITA SET T_ESTATUS_VULNERABILIDAD = 1 WHERE I_ID_VISITA = " + ppObjVisita.IdVisitaGenerado.ToString
         conex.Ejecutar(query)

         If MandaCorreoSandraPachecoVulnerabilidad(objNegVisita, Constantes.CORREO_VULNERABILIDAD_NOTIFICAR_SANDRA) Then
            Response.Redirect("../Procesos/DetalleVisita.aspx")
         End If
      End If
   End Sub

   Protected Sub btnConfirmacionVulnerabilidadNo_Click(sender As Object, e As EventArgs) Handles btnConfirmacionVulnerabilidadNo.Click
      Dim conex = New Conexion.SQLServer()
      Dim query As String = "UPDATE BDS_D_VS_VISITA SET T_ESTATUS_VULNERABILIDAD = 0, F_FECH_ACUERDO_VULNERA = NULL WHERE I_ID_VISITA = " + ppObjVisita.IdVisitaGenerado.ToString
      conex.Ejecutar(query)

      If btnConfirmacionVulnerabilidadNo.CommandArgument = "EnviarConcluido" Then
         'CAMBIA ESTATUS BASE BANDERA NO
         'INICIA CODIGO SIGUIENTE MODIFICACION
         Dim visita As New Visita()
         visita = CType(Session("DETALLE_VISITA"), Visita)

         btnAceptarM2B1A.CommandArgument = "confirmarFechaInicioVisita"
         imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning

         If visita.FechaInicioVisita <> Date.MinValue Then
            txtFecIniVista.Text = visita.FechaInicioVisita.ToString("dd/MM/yyyy")
         End If
         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionFechaInicioVisita();", True)
         'TERMINA CODIGO SIGUIENTE MODIFICACION
      Else
         Response.Redirect("../Procesos/DetalleVisita.aspx")
         Exit Sub
      End If
   End Sub

   Protected Sub btnCancelaFecVulnerabilidad_Click(sender As Object, e As EventArgs) Handles btnCancelaFecVulnerabilidad.Click
      If btnCancelaFecVulnerabilidad.CommandArgument = "EnviarConcluido" Then
         Mensaje = "¿La sesión para la revisión de vulnerabilidades se llevó a cabo?"
         ScriptManager.RegisterStartupScript(Me, GetType(Page), "valor", "ConfirmacionFechaVulnerabilidad()", True)
      Else
         ScriptManager.RegisterStartupScript(Me, GetType(Page), "valor", "SolicitarFechaVulnerabilidad()", True)
      End If
   End Sub

   Protected Sub btnGuardaFecVulnerabilidad_Click(sender As Object, e As EventArgs) Handles btnGuardaFecVulnerabilidad.Click
      Dim objNegVisita As New NegocioVisita(ppObjVisita, puObjUsuario, Server, txbComentarios.Text)
      Dim lsFecha As String = txtFechaGeneral.Text.Trim()
      Dim ldFecha As DateTime

      ''Regresa DateTime.MinValue si alguna validacion falla
      ldFecha = ValidarFechaGeneral(lsFecha, lblFechaGeneral, imgErrorGeneral, "SolicitarFechaGeneral('" & Constantes.MensajesModal.Vulnerabilidades & "', 'btnGuardaFecVulnerabilidad');", False, False, False, False, )
      If ldFecha = DateTime.MinValue Then
         Exit Sub
      End If

      ''Validar si antes ya se habia ingresado alguna fecha de vulnerabilidades de lo contrario validar que la nueva fecha sea igual o menor que el dia de hoy
      If Fechas.Vacia(ppObjVisita.Fecha_AcuerdoVul) Then
         If ldFecha.Date > DateTime.Now.Date Then
            lblFechaGeneral.Text = "La fecha ingresada no puede ser mayor a la fecha actual."
            lblFechaGeneral.Visible = True
            imgErrorGeneral.ImageUrl = Constantes.Imagenes.Fallo
            ScriptManager.RegisterStartupScript(Me, GetType(Page), "valor", "SolicitarFechaGeneral('" & Constantes.MensajesModal.Vulnerabilidades & "', 'btnGuardaFecVulnerabilidad');", True)
            Exit Sub
         End If
      End If

      'GUARDA INFORMACION DE FECHA DATE CON ESTATUS TRUE
      If AccesoBD.ActualizaFechaInicioVisita(ppObjVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaVulneravilidades, ppObjVisita.SubVisitasSeleccionadas) Then
         If btnGuardaFecVulnerabilidad.CommandArgument = "EnviarConcluido" Then
            'INICIA CODIGO SIGUIENTE MODIFICACION

            btnAceptarM2B1A.CommandArgument = "confirmarFechaInicioVisita"
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgWarning

            If ppObjVisita.FechaInicioVisita <> Date.MinValue Then
               txtFecIniVista.Text = ppObjVisita.FechaInicioVisita.ToString("dd/MM/yyyy")
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacionFechaInicioVisita();", True)
            'TERMINA CODIGO SIGUIENTE MODIFICACION
         Else
            If Not ppObjVisita.Fecha_AcuerdoVul.ToString("dd/MM/yyyy") = txtFechaGeneral.Text Then
               ppObjVisita.Fecha_AntAcuerdoVul = ppObjVisita.Fecha_AcuerdoVul

               If Fechas.Vacia(ppObjVisita.Fecha_AcuerdoVul) Then
                  ppObjVisita.Fecha_AcuerdoVul = ldFecha
                  MandaCorreoSandraPachecoVulnerabilidad(objNegVisita, Constantes.CORREO_VULNERABILIDAD_NOTIFICAR_FECHA)
               Else
                  ppObjVisita.Fecha_AcuerdoVul = ldFecha
                  MandaCorreoSandraPachecoVulnerabilidad(objNegVisita, Constantes.CORREO_VULNERABILIDAD_NOTIFICAR_CAMBIO_FECHA)
               End If

            End If
            Response.Redirect("../Procesos/DetalleVisita.aspx")
         End If
      End If
   End Sub


   Protected Sub btnCancelaFechaReunionVjp9_Click(sender As Object, e As EventArgs)
      If Not IsNothing(ppObjVisita) Then
         Mensaje = "La fecha de la reunión con el área de Supervisión fue: " & ppObjVisita.Fecha_ReunionVjPaso9.ToString("dd/MM/yyyy") & "."
         txtFechaGeneral.Text = Fechas.Valor(ppObjVisita.Fecha_ReunionVjPaso9)
         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "ConfirmarFechaReunPreciJuridicoSiNo();", True)
      End If
   End Sub

   Protected Sub btnCancelaFechaReunionVjp16_Click(sender As Object, e As EventArgs)
      If Not IsNothing(ppObjVisita) Then
         Mensaje = "La fecha de la reunión con el área de Supervisión fue: " & ppObjVisita.Fecha_ReunionVjPaso16.ToString("dd/MM/yyyy") & "."
         txtFechaGeneral.Text = Fechas.Valor(ppObjVisita.Fecha_ReunionVjPaso16)
         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmarFechaReunPreciJuridicoP16SiNo();", True)
      End If
   End Sub

   Protected Sub btnCancelaFechaPaso25_Click(sender As Object, e As EventArgs)
      If Not IsNothing(ppObjVisita) Then
         Mensaje = Constantes.MensajesModal.ConfirmacionFechaVj & ppObjVisita.Fecha_ReunionVoPaso25.ToString("dd/MM/yyyy") & "."
         txtFechaGeneral.Text = Fechas.Valor(ppObjVisita.Fecha_ReunionVoPaso25)
         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "ConfirmarFechaReunPaso25SiNo();", True)
      End If
   End Sub

   Protected Sub btnCancelaFechaPaso32_Click(sender As Object, e As EventArgs)
      If Not IsNothing(ppObjVisita) Then
         Mensaje = "La fecha de la reunión con el área de Supervisión fue: " & ppObjVisita.Fecha_ReunionVjPaso32.ToString("dd/MM/yyyy") & "."
         txtFechaGeneral.Text = Fechas.Valor(ppObjVisita.Fecha_ReunionVjPaso32)
         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "ConfirmarFechaReunPaso32SiNo();", True)
      End If
   End Sub

   Protected Sub btnPasoDiezConfSancionNo_Click(sender As Object, e As EventArgs)
      If Not IsNothing(ppObjVisita) And Not IsNothing(puObjUsuario) Then

         Dim objNegVisita As New NegocioVisita(ppObjVisita, puObjUsuario, Server, txbComentarios.Text)

         ''CUANDO ES LA PRIMERA VEZ QUE ENTRA, FINALIZA EL 10, INICIA EL 11, INICIA EL 12 SIN CERRAR EL 11
         If ppObjVisita.IdEstatusActual = Constantes.EstatusPaso.Revisado Then
            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, ppObjVisita.IdEstatusActual,
                                                                  True, Constantes.EstatusPaso.Aprobado,
                                                                  True, 11, Constantes.EstatusPaso.EnRevisionEspera,
                                                                  True, Constantes.CORREO_ID_NOTIFICA_APRUEBA_ACTA_CIRCUNSTANCIADA)

            ppObjVisita.IdPasoActual = 11
            ppObjVisita.FechaInicioPasoActual = DateTime.Now

            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(False, -1,
                                                                    False, -1,
                                                                    True, 12, Constantes.EstatusPaso.EnRevisionEspera,
                                                                    False, -1)
         Else ''FINALIZA LA REACTIVACION EN EL PASO 10 Y 12
            objNegVisita.PasoGenerericEstatusPasoNotificarReactivar(True, ppObjVisita.IdEstatusActual,
                                                                  False, -1,
                                                                  False, -1, -1,
                                                                  True, Constantes.CORREO_ID_NOTIFICA_APRUEBA_ACTA_CIRCUNSTANCIADA,
                                                                  True, True, True,
                                                                  True, Constantes.TipoReactivacion.FinalizaReactivacion,
                                                                  12, 10, Constantes.EstatusPaso.Aprobado)
         End If



         ''SOLICITA LA CONFIRMACION DE LA REUNION
         ''Si es supervisor solicitar confirmar fecha con VJ
         If Not ppObjVisita.SolicitoFechaPaso9 Then
            Mensaje = "La fecha de la reunión con el área de Supervisión fue: " & ppObjVisita.Fecha_ReunionVjPaso9.ToString("dd/MM/yyyy") & "."
            txtFechaGeneral.Text = Fechas.Valor(ppObjVisita.Fecha_ReunionVjPaso9)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "ConfirmarFechaReunPreciJuridicoSiNo();", True)
            Exit Sub
         End If

         Response.Redirect("../Procesos/DetalleVisita.aspx")
      End If
   End Sub

   ''' <summary>
   ''' Abre una modal solicitando el rango de sancion
   ''' </summary>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Private Function MostrarSolicitudRangoSancion() As Boolean
      If Not IsNothing(ppObjVisita) And Not IsNothing(puObjUsuario) Then
         ''Validar si no se ha solicidato antes el rango de la sancion
         If Not ppObjVisita.SolicitoRangoSancion Then
            txbRangoInicio.Text = "$" & FormatNumber(ppObjVisita.RANGO_SANCION_INI.ToString(), 0, , , TriState.UseDefault)
            txbRangoFin.Text = "$" & FormatNumber(ppObjVisita.RANGO_SANCION_FIN.ToString(), 0, , , TriState.UseDefault)
            txtRango.Text = ppObjVisita.COMENTARIO_RANGO_SANCION

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarRangoSancionPaso17();", True)
         End If

         Return ppObjVisita.SolicitoRangoSancion
      End If

      Return True
   End Function

End Class