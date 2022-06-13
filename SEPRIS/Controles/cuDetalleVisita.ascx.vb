Imports Utilerias

Public Class cuDetalleVisita
   Inherits System.Web.UI.UserControl

   Public Property MensajeDty As String

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

    Public Property pObjPropiedades As ucDocumentosVisita.PropiedadesDoc
        Get
            If IsNothing(Session("pObjPropiedades_ucDocumentos_" & hfIdVisita.Value)) Then
                Return Nothing
            Else
                Return CType(Session("pObjPropiedades_ucDocumentos_" & hfIdVisita.Value), ucDocumentosVisita.PropiedadesDoc)
            End If
        End Get
        Set(value As ucDocumentosVisita.PropiedadesDoc)
            Session("pObjPropiedades_ucDocumentos_" & hfIdVisita.Value) = value
        End Set
    End Property

    Public Property pObjPropiedadesProrroga As ucDoctosProrroga.PropiedadesDocProrroga
      Get
         If IsNothing(Session("pObjPropiedadesProrroga_ucDoctosProrroga_" & hfIdVisita.Value)) Then
            Return Nothing
         Else
            Return CType(Session("pObjPropiedadesProrroga_ucDoctosProrroga_" & hfIdVisita.Value), ucDoctosProrroga.PropiedadesDocProrroga)
         End If
      End Get
      Set(value As ucDoctosProrroga.PropiedadesDocProrroga)
         Session("pObjPropiedadesProrroga_ucDoctosProrroga_" & hfIdVisita.Value) = value
      End Set
   End Property

   Public Property piIdVisitaActual As Integer

   Public Property pvVisita As Visita

   Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
      If Not IsPostBack Then
         If Not IsNothing(puObjUsuario) Then
            cargarDatosVisita(piIdVisitaActual)

            If Not IsNothing(pvVisita) Then
               cargarResponsablesAsignados(piIdVisitaActual)

               hfIdVisita.Value = piIdVisitaActual
               imgEditarVisita.CommandArgument = pvVisita.IdVisitaPadreSubvisita

               If puObjUsuario.IdArea = Constantes.AREA_VO Or Not pvVisita.EsSubVisitaOsubFolio Then
                  tdsubentidadT.Visible = False
                  tdsubentidadV.Visible = False
               End If

               If pvVisita.EsSubVisita Then
                  trFolioVisita.Visible = True
                  txtFolioVisita.Text = pvVisita.FolioVisita
               End If

               HabilitaCtrlsEditarSupAdm()
               EsVisitaCancelada()
            End If
         End If
      End If
   End Sub


   Public Sub cargarDatosVisita(ByVal idVisita As Integer)

      Dim objVisita As New Visita()

      If IsNothing(pvVisita) Then
         objVisita = AccesoBD.getDetalleVisita(idVisita, puObjUsuario.IdArea)
      Else
         objVisita = pvVisita
      End If

        If Not IsNothing(objVisita) And objVisita.FolioVisita <> String.Empty And objVisita.IdVisitaGenerado > 0 Then
            ''Obtienelos inspectores y sus correos
            objVisita.LstInspectoresAsignados = AccesoBD.getInspectoresAsignados(idVisita)
            objVisita.LstSupervisoresAsignados = AccesoBD.getSupervisoresAsignados(idVisita)

            If IsNothing(pvVisita) Then
                pvVisita = objVisita
            End If

            txtFechaRegistro.Text = objVisita.FechaRegistro.ToString("dd/MM/yyyy")
            If objVisita.FechaInicioVisita <> Date.MinValue Then txtFechaInicioVisita.Text = objVisita.FechaInicioVisita.ToString("dd/MM/yyyy")
            txtEntidad.Text = objVisita.NombreEntidad
            txtTipoVisita.Text = objVisita.DescripcionTipoVisita

            'If objVisita.NombreAbogadoSancion <> String.Empty Then
            '    txtAbogadoAsignado.Text = objVisita.NombreAbogadoSancion
            '    hfAbogadoAsignado.Value = objVisita.IdAbogadoSancion
            'End If

            'txtAbogadoAsesor.Text = objVisita.NombreAbogadoAsesor
            'hfAbogadoAsesor.Value = objVisita.IdAbogadoAsesor
            'txtContencioso.Text = objVisita.NombreAbogadoContencioso
            'hfContencioso.Value = objVisita.IdAbogadoConstencioso
            'txtSupJuridico.Text = objVisita.NombreAbogadoSupervisor
            'hfSupJuridico.Value = objVisita.IdAbogadoSupervisor

            ''Cargar abogados
            CargaListaPersonas(objVisita.LstAbogadosSupAsesorAsig, lstSupJuridicoA)
            CargaListaPersonas(objVisita.LstAbogadosAsesorAsignados, lstAbogadoAsesor)
            CargaListaPersonas(objVisita.LstAbogadosSupSancionAsig, lstSupJuridicoS)
            CargaListaPersonas(objVisita.LstAbogadosSancionAsignados, lstAbogadoSancion)
            CargaListaPersonas(objVisita.LstAbogadosSupContenAsig, lstSupJuridicoC)
            CargaListaPersonas(objVisita.LstAbogadosContenAsignados, lstContencioso)

            txtDescrip.Text = objVisita.DescripcionVisita
            txtOrdenVisita.Text = objVisita.OrdenVisita

            Dim liItemObjVisitas As ListItem
            For Each objObjetoVisita As Visita.ObjetoVisita In objVisita.LstObjetoVisita
                liItemObjVisitas = New ListItem
                liItemObjVisitas.Text = objObjetoVisita.Descripcion
                liItemObjVisitas.Value = objObjetoVisita.Id
                lbObjetoVisita.Items.Add(liItemObjVisitas)

                If objObjetoVisita.Id = 1 Then
                    trObjetoVisitaOtro.Visible = True
                    txtDscObjetoV.Text = objVisita.DscObjetoVisitaOtro
                End If
            Next

            txtSubEntidad.Text = objVisita.DscSubentidad

            If objVisita.IdPasoActual >= 8 And Not Fechas.Vacia(objVisita.FECH_REUNION__PRESIDENCIA) Then
                txtFechaPresentacionInt.Text = objVisita.FECH_REUNION__PRESIDENCIA.ToString("dd/MM/yyyy")
                txtFechaPresentacionInt.Attributes.Add("style", "width:98%")

                If objVisita.IdPasoActual >= 13 And Not Fechas.Vacia(objVisita.FECH_REUNION__AFORE) Then
                    txtFechaPresentacionExt.Text = Fechas.Valor(objVisita.FECH_REUNION__AFORE)
                    txtFechaPresentacionExt.Attributes.Add("style", "width:98%")
                Else
                    tdFechaExtT.InnerText = ""
                    tdFechaExtC.InnerText = ""
                End If
            Else
                fsFechasAllazgos.Visible = False
                divFechasAllazgos.Visible = False
            End If

            txtUltimosComentarios.Text = objVisita.UltimoComentario.Replace("<ul>", "").Replace("</ul>", "").Replace("</li>", "").Replace("<li>", " - ").Replace("<br/>", " ").Replace("<", "").Replace(">", "")
            txtUsuarioComentarios.Text = objVisita.UltimoUsuarioComentario
            txtUsuarioAdjunto.Text = objVisita.UltimoUsuarioDocumento
            txtAcuerdoVul.Text = Fechas.Valor(objVisita.Fecha_AcuerdoVul)

            Dim ltItem As ListItem
            blUltimosDocs.BulletStyle = BulletStyle.Numbered
            blUltimosDocs.FirstBulletNumber = 1
            blUltimosDocs.DisplayMode = BulletedListDisplayMode.LinkButton

            For Each lsDoc As Visita.DocumentoCargado In objVisita.UltimosDocumentos
                ltItem = New ListItem
                ltItem.Text = lsDoc.Nombre_Original
                ltItem.Value = lsDoc.Nombre_SP

                blUltimosDocs.Items.Add(ltItem)
            Next

            ''Monto de la sancion
            If objVisita.IdPasoActual <= 30 Then
                fsDatosSancion.Visible = False
                divDatosSancion.Visible = False
            Else
                If Not objVisita.TieneSancion Then
                    fsDatosSancion.Visible = False
                    divDatosSancion.Visible = False
                Else
                    txtFechaImposicion.Text = Fechas.Valor(objVisita.Fecha_ImpSancion)
                    txtMntoSancion.Text = "$" & FormatNumber(objVisita.MontoImpSan.ToString(), 0, , , TriState.UseDefault)
                    txtComenSancion.Text = objVisita.ComentariosImpSan
                End If
            End If
            If objVisita.IdPasoActual = 17 Then
                fsIrregularidades.Visible = True
                AltaIrregularidad.Visible = True
                AltaIrregularidad.Inicializar(idVisita)
                AltaIrregularidad.ModificaIds(idVisita)
            End If
        End If
    End Sub


   Public Sub cargarResponsablesAsignados(ByVal idVisita As Integer)

      Dim liaInspectoresAsignados As New List(Of InspectorAsignado)

      If Not IsNothing(piIdVisitaActual) Then

         If Not IsNothing(pvVisita) Then
            liaInspectoresAsignados = pvVisita.LstInspectoresAsignados

            If Not IsNothing(liaInspectoresAsignados) And liaInspectoresAsignados.Count > 0 Then
               Dim liItmLst As ListItem
               For Each objInspector As InspectorAsignado In liaInspectoresAsignados
                  liItmLst = New ListItem
                  liItmLst.Text = objInspector.Nombre
                  liItmLst.Value = objInspector.Id
                  lstInspectoresAsignados.Items.Add(liItmLst)
               Next
            End If
         End If
      End If


      Dim lstSupAsignados As New List(Of SupervisorAsignado)

      If Not IsNothing(piIdVisitaActual) Then

         If Not IsNothing(pvVisita) Then
            lstSupAsignados = pvVisita.LstSupervisoresAsignados

            If Not IsNothing(lstSupAsignados) And lstSupAsignados.Count > 0 Then
               Dim liItmLst As ListItem
               For Each objSup As SupervisorAsignado In lstSupAsignados
                  liItmLst = New ListItem
                  liItmLst.Text = objSup.Nombre
                  liItmLst.Value = objSup.Id
                  lstResponsableInspeccion.Items.Add(liItmLst)
               Next
            End If
         End If
      End If
   End Sub

   Protected Sub blUltimosDocs_Click(sender As Object, e As BulletedListEventArgs) Handles blUltimosDocs.Click
      Dim lsDocumento As String = blUltimosDocs.Items(e.Index).Value

      If lsDocumento.Trim() <> "" Then
         Try
            ''El nombre real del documento en el sharepoint debe de llegar en el comandArgument
            ''Si no llega ahi, buscar el archivo mediante el nombre que hay en la propiedad text
            Dim Shp As New Utilerias.SharePointManager
            Shp.NombreArchivo = lsDocumento

            If lsDocumento.Contains("__scd") Then
               Shp.NombreArchivo = lsDocumento.Replace("__scd", "")
               Shp.ConfigurarSharePointSeprisSicod(Shp)
            ElseIf lsDocumento.Contains("__svg") Then
               Shp.NombreArchivo = lsDocumento.Replace("__svg", "")
               Shp.ConfigurarSharePointSeprisSisvig(Shp)
            Else
               Shp.ConfigurarSharePointSepris(Shp)
            End If

            Shp.VisualizarArchivoSepris(blUltimosDocs.Items(e.Index).Text)
         Catch ex As Exception
            'Se comento porque manda erroraun descargando el archivo de forma correcta
            'Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            'Utilerias.ControlErrores.EscribirEvento("Ocurrio un error al recuperar el archivo.", EventLogEntryType.Error, "SEPRIS", ex.Message)
            Dim Mensaje = "Ocurrio un error al recuperar el archivo."
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos();", True)
         End Try
      End If
   End Sub

   Protected Sub btnEditFechaPI_Click(sender As Object, e As ImageClickEventArgs)
      ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.EditarFecPresentacion & "', 'btnEditarFecPI');", True)
   End Sub

   Protected Sub btnEditFechaPE_Click(sender As Object, e As ImageClickEventArgs)
      ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.EditarFecPresentacion & "', 'btnEditarFecPE');", True)
   End Sub

   Protected Sub btnEditarFecPI_Click(sender As Object, e As EventArgs)
      ''Validar que este llena y sea correcta la fecha
      Dim lsFecha As String = BuscaFechaGeneral()
      Dim ldFecha As Date

      Dim objUsuario As New Entities.Usuario()
      objUsuario = Session(Entities.Usuario.SessionID)

      Dim objNegVisita As NegocioVisita

      ''Objeto negocio visita
      If Not IsNothing(Session("DETALLE_VISITA")) Then
         pvVisita = CType(Session("DETALLE_VISITA"), Visita)
      End If

      If IsNothing(objUsuario) Or IsNothing(pvVisita) Then
         Exit Sub
      ElseIf pvVisita.IdVisitaGenerado <= 0 Then
         Exit Sub
      End If

      If lsFecha.Length < 1 Then
         Dim errores As New Entities.EtiquetaError(2149)
         PonerErrorFechaGeneral(errores.Descripcion)

         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.EditarFecPresentacion & "', 'btnEditarFecPI');", True)
         Exit Sub
      Else
         If Not Date.TryParse(lsFecha, ldFecha) Then
            Dim errores As New Entities.EtiquetaError(2150)
            PonerErrorFechaGeneral(errores.Descripcion)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.EditarFecPresentacion & "', 'btnEditarFecPI');", True)
            Exit Sub
         Else
            If Not EsDiaHabil(ldFecha) Then
               Dim errores As New Entities.EtiquetaError(2142)
               PonerErrorFechaGeneral(errores.Descripcion)

               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.EditarFecPresentacion & "', 'btnEditarFecPI');", True)
               Exit Sub
            Else
               Dim liNumDiasAux As Integer = 0
               Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.DiasFechasAnteriores), liNumDiasAux)
               Dim ldFechaAuxAnterior As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDiasAux, Constantes.IncremeteDecrementa.Decrementa)

               If ldFecha.Date < ldFechaAuxAnterior.Date Then
                  Dim errores As New Entities.EtiquetaError(2165)
                  PonerErrorFechaGeneral(errores.Descripcion.Replace("[FECHA]", ldFechaAuxAnterior.Date.ToString("dd/MM/yyyy")))

                  ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.EditarFecPresentacion & "', 'btnEditarFecPI');", True)
                  Exit Sub
               Else
                  Dim ldFechaAux As Date
                  If pvVisita.FechaInicioPasoActual <> DateTime.MinValue Then
                     ldFechaAux = pvVisita.FechaInicioPasoActual
                  Else
                     ldFechaAux = ldFecha
                  End If

                  If ldFecha.Date < ldFechaAux.Date Then
                     Dim errores As New Entities.EtiquetaError(2164)
                     PonerErrorFechaGeneral(errores.Descripcion.Replace("[FECHA]", ldFechaAux.ToString("dd/MM/yyyy")))

                     ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.EditarFecPresentacion & "', 'btnEditarFecPI');", True)
                     Exit Sub
                  Else
                     PonerErrorFechaGeneral()
                  End If
               End If
            End If
         End If
      End If

      Dim objCorreo As New Entities.Correo(Constantes.CORREO_CAMBIO_FECHA_EXTERNA_INTERNA)
      objNegVisita = New NegocioVisita(pvVisita, objUsuario, Server, BuscaComentarios())

      objCorreo.Asunto = objCorreo.Asunto.Replace("[TIPO_FECHA]", "interna")
      objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[FECHA_REUNION_ANT]", pvVisita.FECH_REUNION__PRESIDENCIA.ToString("dd/MM/yyyy")).
                                          Replace("[FECHA_REUNION_ACTUAL]", ldFecha.ToString("dd/MM/yyyy")).
                                          Replace("[TIPO_FECHA]", "interna")

      ''NOTOTA: aque si deben de considerar permitirle al usuarios seleccionar para que subvisitas quiere replicar la info pero como la lider no lo especifico vamos a hacer nos de la vista gorda.. jejeje... hasta que si lo requiere diga algo y lo defina.
      If AccesoBD.ActualizaFechaInicioVisita(pvVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionPresi, "") Then
         ''Manda correo a los involucrados en la visita es decir a VO/VF y VJ
         objNegVisita.getObjNotificacion().NotificarCorreo(objCorreo, pvVisita, True, True, False)

         Response.Redirect("../Procesos/DetalleVisita.aspx")
      End If
   End Sub

   Protected Sub btnEditarFecPE_Click(sender As Object, e As EventArgs)
      ''Validar que este llena y sea correcta la fecha
      Dim lsFecha As String = BuscaFechaGeneral()
      Dim ldFecha As Date

      Dim objUsuario As New Entities.Usuario()
      objUsuario = Session(Entities.Usuario.SessionID)

      Dim objNegVisita As NegocioVisita

      If Not IsNothing(Session("DETALLE_VISITA")) Then
         pvVisita = CType(Session("DETALLE_VISITA"), Visita)
      End If

      ''Objeto negocio visita
      If IsNothing(objUsuario) Or IsNothing(pvVisita) Then
         Exit Sub
      ElseIf pvVisita.IdVisitaGenerado <= 0 Then
         Exit Sub
      End If

      If lsFecha.Length < 1 Then
         Dim errores As New Entities.EtiquetaError(2149)
         PonerErrorFechaGeneral(errores.Descripcion)

         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.EditarFecPresentacion & "', 'btnEditarFecPE');", True)
         Exit Sub
      Else
         If Not Date.TryParse(lsFecha, ldFecha) Then
            Dim errores As New Entities.EtiquetaError(2150)
            PonerErrorFechaGeneral(errores.Descripcion)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.EditarFecPresentacion & "', 'btnEditarFecPE');", True)
            Exit Sub
         Else
            If Not EsDiaHabil(ldFecha) Then
               Dim errores As New Entities.EtiquetaError(2142)
               PonerErrorFechaGeneral(errores.Descripcion)

               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.EditarFecPresentacion & "', 'btnEditarFecPE');", True)
               Exit Sub
            Else
               Dim liNumDiasAux As Integer = 0
               Int32.TryParse(Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.DiasFechasAnteriores), liNumDiasAux)
               Dim ldFechaAuxAnterior As DateTime = AccesoBD.ObtenerFecha(DateTime.Now, liNumDiasAux, Constantes.IncremeteDecrementa.Decrementa)

               If ldFecha.Date < ldFechaAuxAnterior.Date Then
                  Dim errores As New Entities.EtiquetaError(2165)
                  PonerErrorFechaGeneral(errores.Descripcion.Replace("[FECHA]", ldFechaAuxAnterior.Date.ToString("dd/MM/yyyy")))

                  ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.EditarFecPresentacion & "', 'btnEditarFecPE');", True)
                  Exit Sub
               Else
                  Dim ldFechaAux As Date
                  If pvVisita.FechaInicioPasoActual <> DateTime.MinValue Then
                     ldFechaAux = pvVisita.FechaInicioPasoActual
                  Else
                     ldFechaAux = ldFecha
                  End If

                  If ldFecha.Date < ldFechaAux.Date Then
                     Dim errores As New Entities.EtiquetaError(2164)
                     PonerErrorFechaGeneral(errores.Descripcion.Replace("[FECHA]", ldFechaAux.ToString("dd/MM/yyyy")))

                     ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "SolicitarFechaGeneral('" & Constantes.MensajesModal.EditarFecPresentacion & "', 'btnEditarFecPE');", True)
                     Exit Sub
                  Else
                     PonerErrorFechaGeneral()
                  End If
               End If
            End If
         End If
      End If

      Dim objCorreo As New Entities.Correo(Constantes.CORREO_CAMBIO_FECHA_EXTERNA_INTERNA)
      objNegVisita = New NegocioVisita(pvVisita, objUsuario, Server, BuscaComentarios())

      objCorreo.Asunto = objCorreo.Asunto.Replace("[TIPO_FECHA]", "externa")
      objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[FECHA_REUNION_ANT]", pvVisita.FECH_REUNION__AFORE.ToString("dd/MM/yyyy")).
                                          Replace("[FECHA_REUNION_ACTUAL]", ldFecha.ToString("dd/MM/yyyy")).
                                          Replace("[TIPO_FECHA]", "externa")

      ''NOTOTA: aque si deben de considerar permitirle al usuarios seleccionar para que subvisitas quiere replicar la info pero como la lider no lo especifico vamos a hacer nos de la vista gorda.. jejeje... hasta que si lo requiere diga algo y lo defina.
      If AccesoBD.ActualizaFechaInicioVisita(pvVisita.IdVisitaGenerado, ldFecha, Constantes.TipoFecha.FechaReunionAfore, "") Then
         ''Manda correo a los involucrados en la visita es decir a VO/VF y VJ
         objNegVisita.getObjNotificacion().NotificarCorreo(objCorreo, pvVisita, True, True, False)

         Response.Redirect("../Procesos/DetalleVisita.aspx")
      End If
   End Sub

   Private Function BuscaComentarios() As String
      Dim lsComentarios As String = ""

      ''Busca los comentarios
      Dim lstTxt As List(Of TextBox) = Me.Page.GetAllControlsOfType(Of TextBox)()

      If lstTxt.Count > 0 Then
         Dim txtComentarios As TextBox = (From lTxt In lstTxt Where lTxt.ID = "txbComentarios").FirstOrDefault()

         If Not IsNothing(txtComentarios) Then
            lsComentarios = txtComentarios.Text
         End If
      End If

      Return lsComentarios
   End Function

   Private Function BuscaFechaGeneral() As String
      Dim lsFecha As String = ""

      ''Busca los comentarios
      Dim lstTxt As List(Of TextBox) = Me.Page.GetAllControlsOfType(Of TextBox)()

      If lstTxt.Count > 0 Then
         Dim txtFechaGeneral As TextBox = (From lTxt In lstTxt Where lTxt.ID = "txtFechaGeneral").FirstOrDefault()

         If Not IsNothing(txtFechaGeneral) Then
            lsFecha = txtFechaGeneral.Text
         End If
      End If

      Return lsFecha
   End Function

   Private Sub PonerErrorFechaGeneral(Optional lsError As String = "")
      ''Busca los comentarios
      Dim lstTxt As List(Of Label) = Me.Page.GetAllControlsOfType(Of Label)()

      If lstTxt.Count > 0 Then
         Dim lblFechaGeneral As Label = (From lTxt In lstTxt Where lTxt.ID = "lblFechaGeneral").FirstOrDefault()

         If Not IsNothing(lblFechaGeneral) Then
            lblFechaGeneral.Text = lsError
            If lsError = "" Then
               lblFechaGeneral.Visible = False
            Else
               lblFechaGeneral.Visible = True
            End If
         End If
      End If
   End Sub

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

   'Private Sub cargarAbogadosAsesores(piPerfilAbogado As Integer)
   '    Try
   '        ddlListAbogados.DataSource = AccesoBD.ObtenerUsuarioInvolucradosVisitaDS(Constantes.AREA_VJ, piPerfilAbogado)

   '        ddlListAbogados.DataTextField = "NOMBRE_COMPLETO"
   '        ddlListAbogados.DataValueField = "T_ID_USUARIO"
   '        ddlListAbogados.DataBind()

   '    Catch ex As Exception
   '        'NHM INICIA LOG           
   '        Utilerias.ControlErrores.EscribirEvento("Ocurrió un error al cargar los abogados disponibles. EXCEPTION: " + ex.ToString(), EventLogEntryType.Error)
   '        'NHM FIN LOG
   '    End Try
   'End Sub

   ''' <summary>
   ''' Mostrar abogados para editar
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub imgMuestraAbogado_Click(sender As Object, e As ImageClickEventArgs)
      Dim btnEditar As ImageButton = CType(sender, ImageButton)
      Dim lstAbogados As New List(Of Abogado)

      If Not IsNothing(btnEditar) Then
         Dim litipoAbogado As Integer = CInt(btnEditar.CommandArgument)

         If IsNothing(pvVisita) Then
            If Not IsNothing(pObjPropiedades) Then
               If Not IsNothing(pObjPropiedades.ppObjVisita) Then
                  pvVisita = pObjPropiedades.ppObjVisita
               End If
            End If

            If Not IsNothing(pObjPropiedadesProrroga) Then
               If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                  pvVisita = pObjPropiedadesProrroga.ppObjVisita
               End If
            End If

         End If

         If IsNothing(pvVisita) Then
            pvVisita = AccesoBD.getDetalleVisita(hfIdVisita.Value, puObjUsuario.IdArea)
         End If

         If Not IsNothing(pvVisita) Then
            ''LLena los abogados disponibles
            Dim lsTipoAbogadoAux As String = litipoAbogado.ToString()
            If lsTipoAbogadoAux = "25" Or lsTipoAbogadoAux = "26" Or lsTipoAbogadoAux = "27" Then
               lsTipoAbogadoAux = lsTipoAbogadoAux.Substring(0, 1)
            End If

            lstUsuariosDisponibles.Items.Clear()
            lstUsuariosDisponibles.DataSource = AccesoBD.ObtenerUsuarioInvolucradosVisitaDS(Constantes.AREA_VJ, CInt(lsTipoAbogadoAux))
            lstUsuariosDisponibles.DataTextField = "NOMBRE_COMPLETO"
            lstUsuariosDisponibles.DataValueField = "USUARIO"
            lstUsuariosDisponibles.DataBind()


            If litipoAbogado = Constantes.ABOGADOS.PERFIL_ABO_ASESOR Then
               lblTituloInterno.Text = "Asesor:"
               lstAbogados = pvVisita.LstAbogadosAsesorAsignados
            ElseIf litipoAbogado = Constantes.ABOGADOS.PERFIL_ABO_SANCIONES Then
               lblTituloInterno.Text = "Sanciones"
               lstAbogados = pvVisita.LstAbogadosSancionAsignados
            ElseIf litipoAbogado = Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO Then
               lblTituloInterno.Text = "Contencioso:"
               lstAbogados = pvVisita.LstAbogadosContenAsignados
            ElseIf litipoAbogado = 25 Then
               lblTituloInterno.Text = "Supervisor Asesor:"
               lstAbogados = pvVisita.LstAbogadosSupAsesorAsig
            ElseIf litipoAbogado = 26 Then
               lblTituloInterno.Text = "Supervisor Sanciones:"
               lstAbogados = pvVisita.LstAbogadosSupSancionAsig
            ElseIf litipoAbogado = 27 Then
               lblTituloInterno.Text = "Supervisor Contencioso:"
               lstAbogados = pvVisita.LstAbogadosSupContenAsig
            End If


            CargaListaPersonas(lstAbogados, lstUsuariosAsignados)

            Dim ltItem As ListItem

            ''Remover usuarios de os disponibles
            For Each objAbo As Abogado In lstAbogados
               ltItem = (From ltIt As ListItem In lstUsuariosDisponibles.Items Where ltIt.Value = objAbo.Id Select ltIt).FirstOrDefault()

               If Not IsNothing(ltItem) Then
                  lstUsuariosDisponibles.Items.Remove(ltItem)
               End If
            Next

            imgAsignar.Visible = True
            imgAsignar.Enabled = True
            imgDesasignar.Enabled = True
            imgDesasignar.Visible = True

            ''La funcion de ModificarAbogadosVisita dee de estar en la pagina donde se ocupa el control
            btnGuardarAbogado.CommandArgument = litipoAbogado
            lblErrorAbogado.Text = ""
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "ModificarAbogadosVisita('" & divAbogados.ClientID & "','" & btnGuardarAbogado.ClientID & "');", True)
         End If
      End If
   End Sub

   ''' <summary>
   ''' Guarda el nuevo abogado seleccionado
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub btnGuardarAbogado_Click(sender As Object, e As EventArgs)
      Dim btnGuardar As Button = CType(sender, Button)
      Dim lstAbogados As New List(Of Abogado)

      If Not IsNothing(btnGuardar) Then
         Dim lsComentarios As String = ""
         Dim liTipoAbogado As Integer = btnGuardar.CommandArgument
         Dim lbAux As Boolean = False
         Dim objCorreo As New Entities.Correo(Constantes.CORREO_CAMBIO_ABOGADOS_VISITA)

         If Not lbAux Then
            If IsNothing(pvVisita) Then

               If Not IsNothing(pObjPropiedades) Then
                  If Not IsNothing(pObjPropiedades.ppObjVisita) Then
                     pvVisita = pObjPropiedades.ppObjVisita
                  End If
               End If

               If Not IsNothing(pObjPropiedadesProrroga) Then
                  If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                     pvVisita = pObjPropiedadesProrroga.ppObjVisita
                  End If
               End If

            End If

            If IsNothing(pvVisita) Then
               pvVisita = AccesoBD.getDetalleVisita(hfIdVisita.Value, puObjUsuario.IdArea)
            End If

            Dim lsAbogadosAux As String = GetAbogadosSeleccionados(lstAbogados)
            If lstAbogados.Count > 0 Then
               If AccesoBD.ActualizaResponsablesVisita(pvVisita.IdVisitaGenerado, lsAbogadosAux, liTipoAbogado) Then
                  Dim lsListaAbogados As String = (From objA In lstAbogados Select objA.Nombre).ToList().toListHtml()
                  Dim lsPerfilAbogado As String = ""

                  objCorreo.Cuerpo = objCorreo.Cuerpo.Replace("[CAMBIO_RESPONSABLE_ABOGADO]", lsListaAbogados)

                  ''Arma el comentario
                  If liTipoAbogado = Constantes.ABOGADOS.PERFIL_ABO_ASESOR Then
                     lsComentarios = "Abogados Asesores, de " & (From objA In pvVisita.LstAbogadosAsesorAsignados Select objA.Nombre).ToList().toListHtml() & " a " & lsListaAbogados
                     pvVisita.LstAbogadosAsesorAsignados = lstAbogados
                     lsPerfilAbogado = "Abogados Asesores"
                     CargaListaPersonas(pvVisita.LstAbogadosAsesorAsignados, lstAbogadoAsesor)
                  ElseIf liTipoAbogado = Constantes.ABOGADOS.PERFIL_ABO_SANCIONES Then
                     lsComentarios = "Abogados Sanciones, de " & (From objA In pvVisita.LstAbogadosSancionAsignados Select objA.Nombre).ToList().toListHtml() & " a " & lsListaAbogados
                     pvVisita.LstAbogadosSancionAsignados = lstAbogados
                     lsPerfilAbogado = "Abogados Sanciones"
                     CargaListaPersonas(pvVisita.LstAbogadosSancionAsignados, lstAbogadoSancion)
                  ElseIf liTipoAbogado = Constantes.ABOGADOS.PERFIL_ABO_CONTENCIOSO Then
                     lsComentarios = "Abogados Contenciosos, de " & (From objA In pvVisita.LstAbogadosContenAsignados Select objA.Nombre).ToList().toListHtml() & " a " & lsListaAbogados
                     lsPerfilAbogado = "Abogados Contenciosos"
                     pvVisita.LstAbogadosContenAsignados = lstAbogados
                     CargaListaPersonas(pvVisita.LstAbogadosContenAsignados, lstContencioso)
                  ElseIf liTipoAbogado = 25 Then
                     lsComentarios = "Supervisores Asesores, de " & (From objA In pvVisita.LstAbogadosSupAsesorAsig Select objA.Nombre).ToList().toListHtml() & " a " & lsListaAbogados
                     lsPerfilAbogado = "Abogados Supervisores Asesores"
                     pvVisita.LstAbogadosSupAsesorAsig = lstAbogados
                     CargaListaPersonas(pvVisita.LstAbogadosSupAsesorAsig, lstSupJuridicoA)
                  ElseIf liTipoAbogado = 26 Then
                     lsComentarios = "Supervisores Sanciones, de " & (From objA In pvVisita.LstAbogadosSupSancionAsig Select objA.Nombre).ToList().toListHtml() & " a " & lsListaAbogados
                     lsPerfilAbogado = "Abogados Supervisores Sanciones"
                     pvVisita.LstAbogadosSupSancionAsig = lstAbogados
                     CargaListaPersonas(pvVisita.LstAbogadosSupSancionAsig, lstSupJuridicoS)
                  ElseIf liTipoAbogado = 27 Then
                     lsComentarios = "Supervisores Contenciosos, de " & (From objA In pvVisita.LstAbogadosSupContenAsig Select objA.Nombre).ToList().toListHtml() & " a " & lsListaAbogados
                     lsPerfilAbogado = "Abogados Supervisores Contenciosos"
                     pvVisita.LstAbogadosSupContenAsig = lstAbogados
                     CargaListaPersonas(pvVisita.LstAbogadosSupContenAsig, lstSupJuridicoC)
                  End If

                  lsComentarios = (puObjUsuario.Nombre & " " & puObjUsuario.Apellido + " modifico " & lsComentarios & "<br />" & BuscaComentarios()).Trim()

                  objCorreo.Asunto = objCorreo.Asunto.Replace("[PERFIL_ABOGADO]", lsPerfilAbogado)

                  Dim objNegocio As New NegocioVisita(pvVisita, puObjUsuario, Server, lsComentarios)
                  If Not IsNothing(objCorreo) Then
                     objNegocio.PasoGenerericEstatusPasoNotificarReactivar(True, pvVisita.IdEstatusActual,
                                                                         False, -1,
                                                                         False, -1, -1,
                                                                         True, objCorreo,
                                                                         True, True, False, , , , , , , True)
                  End If
               End If
            Else
               btnGuardarAbogado.CommandArgument = liTipoAbogado
               ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "ModificarAbogadosVisita('" & divAbogados.ClientID & "','" & btnGuardarAbogado.ClientID & "');", True)
            End If
         End If
      End If
   End Sub

   Public Function GetAbogadosSeleccionados(ByRef lstAbogados As List(Of Abogado)) As String
      Dim lsAboSup As String = ""
      Dim objAbog As Abogado
      Dim lstAboSelec As New List(Of Abogado)
      Dim lsErrores As String = "<ul>"

      If lstUsuariosAsignados.Items.Count > 0 Then
         For indice As Integer = 0 To lstUsuariosAsignados.Items.Count - 1
            objAbog = New Abogado
            objAbog.Id = lstUsuariosAsignados.Items(indice).Value
            objAbog.Nombre = lstUsuariosAsignados.Items(indice).Text

            lstAboSelec.Add(objAbog)
            lsAboSup &= lstUsuariosAsignados.Items(indice).Value & "|" & lstUsuariosAsignados.Items(indice).Text & ","
         Next
      Else
         lsErrores &= "<li> Debes seleccionar algún abogado </li>"
      End If

      If lsAboSup.Length > 0 Then
         lsAboSup = lsAboSup.Substring(0, lsAboSup.Length - 1)
      Else
         lsAboSup = ""
      End If

      lsErrores &= "</ul>"

      If lstAboSelec.Count <= 0 Then
         lblErrorAbogado.Text = lsErrores
      Else
         lblErrorAbogado.Text = ""
      End If

      lstAbogados = lstAboSelec
      Return lsAboSup
   End Function

   ''' <summary>
   ''' Muestra los responsables de la visita, supervisor area e inspectores
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   ''' <remarks></remarks>
   Protected Sub imgMuestraResponsables_Click(sender As Object, e As ImageClickEventArgs)
      Dim btnEditar As ImageButton = CType(sender, ImageButton)
      If IsNothing(pvVisita) Then

         If Not IsNothing(pObjPropiedades) Then
            If Not IsNothing(pObjPropiedades.ppObjVisita) Then
               pvVisita = pObjPropiedades.ppObjVisita
            End If
         End If

         If Not IsNothing(pObjPropiedadesProrroga) Then
            If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
               pvVisita = pObjPropiedadesProrroga.ppObjVisita
            End If
         End If

      End If

      If IsNothing(pvVisita) Then
         pvVisita = AccesoBD.getDetalleVisita(hfIdVisita.Value, puObjUsuario.IdArea)
      End If

      If Not IsNothing(btnEditar) And Not IsNothing(pvVisita) Then
         Dim litipoResponsable As Integer = CInt(btnEditar.CommandArgument)

         ucRespVisita.pObjVisitaCuRespV = pvVisita
         ucRespVisita.piTipoResponsable = litipoResponsable
         ucRespVisita.CargarControl()

         ''La funcion de ModificarResponsablesVisita dee de estar en la pagina donde se ocupa el control
         btnGuardarRespVisita.CommandArgument = litipoResponsable
         ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "ModificarResponsablesVisita('" & divRespInsp.ClientID & "','" & btnGuardarRespVisita.ClientID & "');", True)
      End If
   End Sub

   Protected Sub btnGuardarRespVisita_Click(sender As Object, e As EventArgs)
      Dim btnGuardar As Button = CType(sender, Button)
      Dim liTipoResp As Constantes.ResponsablesVisita = CType(btnGuardar.CommandArgument, Constantes.ResponsablesVisita)

      If IsNothing(pvVisita) Then
         If Not IsNothing(pObjPropiedades) Then
            If Not IsNothing(pObjPropiedades.ppObjVisita) Then
               pvVisita = pObjPropiedades.ppObjVisita
            End If
         End If

         If Not IsNothing(pObjPropiedadesProrroga) Then
            If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
               pvVisita = pObjPropiedadesProrroga.ppObjVisita
            End If
         End If

      End If

      If IsNothing(pvVisita) Then
         pvVisita = AccesoBD.getDetalleVisita(hfIdVisita.Value, puObjUsuario.IdArea)
      End If

      Dim lsRespVisitas As String = ucRespVisita.GetUsuariosSeleccionados()

      If AccesoBD.ActualizaResponsablesVisita(pvVisita.IdVisitaGenerado, lsRespVisitas, liTipoResp) Then
         Dim lsComentarios As String = ""
         Dim lstUsuario As List(Of Persona) = ucRespVisita.GetListaUsuariosSeleccionados()

         ''Arma el comentario
         If liTipoResp = Constantes.ResponsablesVisita.Supervisor Then
            lsComentarios = "Supervisor(es), de " & (From objInsp As SupervisorAsignado In pvVisita.LstSupervisoresAsignados Select objInsp.Nombre).ToList().toListHtml() &
            " <br/> a <br/> " & (From objInsp As Persona In lstUsuario Select objInsp.Nombre).ToList().toListHtml()
         ElseIf liTipoResp = Constantes.ResponsablesVisita.Inspector Then
            lsComentarios = "Inspector(es), de " & (From objInsp As InspectorAsignado In pvVisita.LstInspectoresAsignados Select objInsp.Nombre).ToList().toListHtml() &
                            " <br/> a <br/> " & (From objInsp As Persona In lstUsuario Select objInsp.Nombre).ToList().toListHtml()
         End If

         lsComentarios = ("Supervisor de presidencia modifico " & lsComentarios & " " & BuscaComentarios()).Trim()

         Dim objNegocio As New NegocioVisita(pvVisita, puObjUsuario, Server, lsComentarios)
         objNegocio.PasoGenerericEstatusPasoNotificarReactivar(True, pvVisita.IdEstatusActual, False, -1, False, -1, -1, False, -1)
         objNegocio.getObjNotificacion().NotificarCorreo(Constantes.CORREO_CAMBIO_RESPONSABLES_VISITA, pvVisita, False, False, False, lstUsuario)

         Response.Redirect("../Procesos/DetalleVisita.aspx")
      Else
         ucRespVisita.MuestraError("No se pudieron actualizar los usuarios.")
      End If
   End Sub

   Private Sub HabilitaCtrlsEditarSupAdm()
      Dim liPorentaje As Integer = 80

      ''mostrar u ocultar los renglones
      If pvVisita.LstAbogadosSupAsesorAsig.Count <= 0 And pvVisita.LstAbogadosAsesorAsignados.Count <= 0 Then
         renAsesor.Visible = False
      End If

      If pvVisita.LstAbogadosSupSancionAsig.Count <= 0 And pvVisita.LstAbogadosSancionAsignados.Count <= 0 Then
         renSanciones.Visible = False
      End If

      If pvVisita.LstAbogadosSupContenAsig.Count <= 0 And pvVisita.LstAbogadosContenAsignados.Count <= 0 Then
         renContencioso.Visible = False
      End If

      ''Mostras u ocultar el reenglon del titulo si un reenglon de abogados esta visible
      renTituloJuridico.Visible = False
      If renAsesor.Visible Or renSanciones.Visible Or renContencioso.Visible Then
         renTituloJuridico.Visible = True
      End If

      If (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM Or
          puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP) And
          pObjPropiedades.ppObjVisita.IdPasoActual <= 3 And
          puObjUsuario.IdArea <> Constantes.AREA_VJ Then
         imgEditarVisita.Visible = True
         imgEditarVisita.Enabled = True
      Else
         imgEditarVisita.Visible = False
      End If

      If (pObjPropiedades.ppObjVisita.UsuarioEstaOcupando.Trim() <> puObjUsuario.IdentificadorUsuario.Trim()) And
          pObjPropiedades.ppObjVisita.UsuarioEstaOcupando.Trim().Length > 0 Then
         Exit Sub
      End If

      ''Salir si es cancelada
      If pvVisita.EsCancelada Then
         Exit Sub
      End If

      If puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM Then
         If puObjUsuario.IdArea = Constantes.AREA_PR Or puObjUsuario.IdArea = Constantes.AREA_VJ Then
            If pvVisita.LstAbogadosSupAsesorAsig.Count > 0 Then
               imgSupJuridicoA.Visible = True
               imgSupJuridicoA.Enabled = True
               lstSupJuridicoA.Width = New Unit(liPorentaje, UnitType.Percentage)
            End If

            If pvVisita.LstAbogadosSupSancionAsig.Count > 0 Then
               imgSupJuridicoS.Visible = True
               imgSupJuridicoS.Enabled = True
               lstSupJuridicoS.Width = New Unit(liPorentaje, UnitType.Percentage)
            End If

            If pvVisita.LstAbogadosSupContenAsig.Count > 0 Then
               imgSupJuridicoC.Visible = True
               imgSupJuridicoC.Enabled = True
               lstSupJuridicoC.Width = New Unit(liPorentaje, UnitType.Percentage)
            End If

            If pvVisita.LstAbogadosAsesorAsignados.Count > 0 Then
               imgMuestraAsesor.Visible = True
               imgMuestraAsesor.Enabled = True
               lstAbogadoAsesor.Width = New Unit(liPorentaje, UnitType.Percentage)
            End If

            If pvVisita.LstAbogadosSancionAsignados.Count > 0 Then
               imgMuestraSancion.Visible = True
               imgMuestraSancion.Enabled = True
               lstAbogadoSancion.Width = New Unit(liPorentaje, UnitType.Percentage)
            End If

            If pvVisita.LstAbogadosContenAsignados.Count > 0 Then
               imgMuestraContencioso.Visible = True
               imgMuestraContencioso.Enabled = True
               lstContencioso.Width = New Unit(liPorentaje, UnitType.Percentage)
            End If
         End If

         If EsAreaOperativa(puObjUsuario.IdArea) Then
            imgResponsableInspeccion.Visible = True
            imgResponsableInspeccion.Enabled = True
            lstResponsableInspeccion.Width = New Unit(liPorentaje, UnitType.Percentage)

            imgInspectoresAsignados.Visible = True
            imgInspectoresAsignados.Enabled = True
            lstInspectoresAsignados.Width = New Unit(liPorentaje, UnitType.Percentage)
         End If
      End If

      'Deshabilita y oculta botones por permisos si visita viene de sisvig y si usuario es de VO
      If pvVisita.VisitaSisvig And pvVisita.IdPasoActual <= 19 And EsAreaOperativa(puObjUsuario.IdArea) Then

         imgResponsableInspeccion.Visible = False
         imgResponsableInspeccion.Enabled = False

         imgInspectoresAsignados.Visible = False
         imgInspectoresAsignados.Enabled = False

         imgMuestraSancion.Visible = False
         imgMuestraSancion.Enabled = False

         imgMuestraContencioso.Visible = False
         imgMuestraContencioso.Enabled = False

         imgMuestraAsesor.Visible = False
         imgMuestraAsesor.Enabled = False

         imgSupJuridicoC.Visible = False
         imgSupJuridicoC.Enabled = False

         imgSupJuridicoS.Visible = False
         imgSupJuridicoS.Enabled = False

         imgSupJuridicoA.Visible = False
         imgSupJuridicoA.Enabled = False

         imgEditarVisita.Visible = False
         imgEditarVisita.Enabled = False

      End If

   End Sub

   Private Sub CargaListaPersonas(lstPersonas As List(Of Abogado), ByRef lstLbPersonas As ListBox)
      If Not IsNothing(lstPersonas) Then
         If lstPersonas.Count > 0 Then
            Dim liItmLst As ListItem
            lstLbPersonas.Items.Clear()
            For Each objAbogado As Abogado In lstPersonas
               liItmLst = New ListItem
               liItmLst.Text = objAbogado.Nombre
               liItmLst.Value = objAbogado.Id
               lstLbPersonas.Items.Add(liItmLst)
            Next
         End If
      End If
   End Sub

   Protected Sub imgAsignar_Click(sender As Object, e As ImageClickEventArgs) Handles imgAsignar.Click
      agregarUsuario()
   End Sub

   Protected Sub imgDesasignar_Click(sender As Object, e As ImageClickEventArgs) Handles imgDesasignar.Click
      quitaUsuario()
   End Sub

   Private Sub agregarUsuario()
      Try
         If lstUsuariosDisponibles.SelectedIndex > -1 Then
            Dim item As System.Web.UI.WebControls.ListItem = lstUsuariosDisponibles.SelectedItem
            If Not lstUsuariosAsignados.Items.Contains(item) Then
               lstUsuariosAsignados.Items.Add(item)
            End If
            item.Selected = False
            lstUsuariosDisponibles.Items.Remove(item)
         End If

      Catch ex As Exception
      End Try
   End Sub

   Private Sub quitaUsuario()
      Try
         If lstUsuariosAsignados.SelectedIndex > -1 Then
            Dim item As System.Web.UI.WebControls.ListItem = lstUsuariosAsignados.SelectedItem

            lstUsuariosDisponibles.Items.Insert(0, item)
            Dim lstAnterior As New ListBox
            lstAnterior.Items.AddRange((From ltItem As ListItem In lstUsuariosDisponibles.Items Order By ltItem.Value).ToArray())

            lstUsuariosDisponibles.Items.Clear()
            lstUsuariosDisponibles.Items.AddRange((From ltItem As ListItem In lstAnterior.Items Select ltItem).ToArray())

            item.Selected = False
            lstUsuariosAsignados.Items.Remove(item)
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

   Protected Sub imgEditarVisita_Click(sender As Object, e As ImageClickEventArgs) Handles imgEditarVisita.Click
      Dim imgEditar As ImageButton = CType(sender, ImageButton)
      Dim liVisitaPadre As Integer = 0

      If Not IsNothing(imgEditar) Then
         If IsNothing(pvVisita) Then
            If Not IsNothing(pObjPropiedades) Then
               If Not IsNothing(pObjPropiedades.ppObjVisita) Then
                  pvVisita = pObjPropiedades.ppObjVisita
               End If
            End If

            If Not IsNothing(pObjPropiedadesProrroga) Then
               If Not IsNothing(pObjPropiedadesProrroga.ppObjVisita) Then
                  pvVisita = pObjPropiedadesProrroga.ppObjVisita
               End If
            End If

         End If

         If IsNothing(pvVisita) Then
            pvVisita = AccesoBD.getDetalleVisita(hfIdVisita.Value, puObjUsuario.IdArea)
         End If

         Int32.TryParse(imgEditar.CommandArgument, liVisitaPadre)
         Session.Add("idPasoActualEditada", pvVisita.IdPasoActual)
         Session.Add("idEstatusActualEditada", pvVisita.IdEstatusActual)

         If liVisitaPadre <> 0 Then
            Session.Add("idVisitaEditar", hfIdVisita.Value)
            Response.Redirect("../Visita/Registro.aspx?r=1&up=1&sb=" & imgEditar.CommandArgument)
         Else
            Session.Add("idVisitaEditar", hfIdVisita.Value)
            Response.Redirect("../Visita/Registro.aspx?r=1&up=1")
         End If
      End If
   End Sub

   Public Sub EsVisitaCancelada()
      If pvVisita.EsCancelada = True Then
         FVCancelada.Visible = True
         txtMotivoCancela.Text = pvVisita.MotivoCancelacion
         txtFechaCancela.Text = pvVisita.FechaCancela
         txtUsuarioCancela.Text = pvVisita.IdUsuarioCancela
      End If
   End Sub

End Class