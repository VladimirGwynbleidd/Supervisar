Imports Utilerias
Imports System.Web.Configuration
Imports System.IO

Public Class SeguimientoDetalle
    Inherits System.Web.UI.Page
    Public Property Mensaje As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                If Not IsNothing(Session("ID_VISITA_SEGUIMIENTO")) Then
                    Dim idVisitaSeguimiento As Integer = Session("ID_VISITA_SEGUIMIENTO")
                    cargarEncabezadoReporteVisitaASPX(idVisitaSeguimiento)
                    cargarPasosVisitaASPX(idVisitaSeguimiento)
                End If
            End If

            'Dim usuario As New Entities.Usuario()
            'usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

            ' ''Cargar imagen proceso ins sancion
            'If usuario.IdArea = 36 Then
            '    imgProcesoInspSancion.ImageUrl = "~/Imagenes/ProcesoInspSan/" & Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.NombreImagenProcesoInspeccionSancVF)
            'Else
            '    imgProcesoInspSancion.ImageUrl = "~/Imagenes/ProcesoInspSan/" & Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.NombreImagenProcesoInspeccionSancOtras)
            'End If
        Else 'si es postbak
            If Not IsNothing(Session("ID_VISITA_SEGUIMIENTO")) Then
                Dim idVisitaSeguimiento As Integer = Session("ID_VISITA_SEGUIMIENTO")
                cargarPasosVisitaASPX(idVisitaSeguimiento)
            End If
        End If

        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

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

    End Sub

    Protected Sub imgInicio_Click(sender As Object, e As ImageClickEventArgs) Handles imgInicio.Click
        Response.Redirect("../Visita/Bandeja.aspx")
    End Sub

    Protected Sub linkButton_Click(sender As Object, e As EventArgs)
        Dim link As LinkButton = CType(sender, LinkButton)
        Dim datosLinkHizoClic As String = link.CommandArgument

        Dim datosArray() As String = datosLinkHizoClic.Split("|")

        Dim datos As New Dictionary(Of String, String)

        For Each dato As String In datosArray
            Dim dArray() As String = dato.Split(":")
            datos.Add(dArray(0), dArray(1))
        Next

        Try

            Dim Shp As New Utilerias.SharePointManager

            If datos.Item("NomArchivo").Contains("__scd") Then
                Shp.NombreArchivo = datos.Item("NomArchivo").Replace("__scd", "")
                ConfigurarSharePointSeprisSicod(Shp)
            ElseIf datos.Item("NomArchivo").Contains("__svg") Then
                Shp.NombreArchivo = datos.Item("NomArchivo").Replace("__svg", "")
                ConfigurarSharePointSeprisSisvig(Shp)
            Else
                ConfigurarSharePointSepris(Shp)
                Shp.NombreArchivo = datos.Item("NomArchivo")
            End If
            Shp.VisualizarArchivoSepris(link.Text)
        Catch ex As Exception
            'Se comento porque manda erroraun descargando el archivo de forma correcta
            'Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            'Utilerias.ControlErrores.EscribirEvento("Ocurrio un error al recuperar el archivo.", EventLogEntryType.Error, "SEPRIS", ex.Message)
            Mensaje = "Ocurrio un error al recuperar el archivo."
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AquiMuestroMensaje();", True)
        End Try


    End Sub

    Protected Sub imgVistaPreliminar_Click(sender As Object, e As ImageClickEventArgs) Handles imgVistaPreliminar.Click

        Dim RutaPDF As String = String.Empty

        If Not IsNothing(Session("ID_VISITA_SEGUIMIENTO")) Then

            Dim idVisitaSeguimiento As Integer = Session("ID_VISITA_SEGUIMIENTO")

            RutaPDF = generarSeguimientoDetallePDF(idVisitaSeguimiento)

            Dim nombreArchivo As String
            nombreArchivo = Path.GetFileName(RutaPDF)

            If Not IsNothing(nombreArchivo) And nombreArchivo <> String.Empty Then
                Dim url As String
                url = "../Reportes/" + nombreArchivo

                Dim script As String
                script = "window.open(""{0}"", ""{1}"")"
                script = String.Format(script, url, "_blank")
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Redirect", script, True)
                ' '' Limpiamos la salida
                'Response.Clear()
                ' '' Con esto le decimos al browser que la salida sera descargable
                'Response.ContentType = "application/octet-stream"
                ' '' esta linea es opcional, en donde podemos cambiar el nombre del fichero a descargar (para que sea diferente al original)
                'Response.AddHeader("Content-Disposition", "attachment; filename=" & nombreArchivo)
                ' '' Escribimos el fichero a enviar 
                'Response.WriteFile(RutaPDF)
                ' '' volcamos el stream 
                'Response.Flush()

                ''Eliminanos el archivo temporal
                'If File.Exists(RutaPDF) Then
                '    File.Delete(RutaPDF)
                'End If

                ' '' Enviamos todo el encabezado ahora
                'Response.End()

            End If

        End If
    End Sub

    Public Sub cargarEncabezadoReporteVisitaASPX(ByVal idVisita As Integer)

        If idVisita > 0 Then
            Dim encabezadoReporte As New EncabezadoReporteVisita()
            encabezadoReporte = AccesoBD.getEncabezadoReporteVisita(idVisita)

            lblFolioVisita.Text = encabezadoReporte.FolioVisita
            lblFechaRegistro.Text = encabezadoReporte.FechaRegistro.ToString("dd/MM/yyyy H:mm")
            lblPasoActual.Text = encabezadoReporte.DscPasoActual
        End If
    End Sub

    Public Sub cargarPasosVisitaASPX(ByVal idVisita As Integer)

        If idVisita > 0 Then

            Dim lstPasosReporte As New List(Of PasoReporte)
            lstPasosReporte = getPasosReporte(idVisita)

            Dim contadorLabels As Integer = 0
            Dim contadorLinks As Integer = 0
            For Each pasoReporte As PasoReporte In lstPasosReporte

                'CALCULA EL ANCHO DE LAS BARRAS (DIVS) DE TIEMPO ESTIMADO Y REAL
                Dim anchoPorDia As Single = 0
                If pasoReporte.NumDiasTiempoEstimado < 90 And pasoReporte.NumDiasTiempoReal < 90 Then
                    anchoPorDia = 90 / 90
                Else
                    If pasoReporte.NumDiasTiempoEstimado > pasoReporte.NumDiasTiempoReal Then
                        anchoPorDia = 90 / pasoReporte.NumDiasTiempoEstimado
                    Else
                        anchoPorDia = 90 / pasoReporte.NumDiasTiempoReal
                    End If
                End If

                Dim _diasEstimadosAnchoBarra As Integer = anchoPorDia * pasoReporte.NumDiasTiempoEstimado
                Dim _diasRealesAnchoBarra As Integer = anchoPorDia * pasoReporte.NumDiasTiempoReal


                'PREPARA EL RENGLON
                Dim trRenglonPaso As HtmlTableRow
                Dim lstReenglones As New List(Of HtmlTableRow)

                'CREA COLUMNA 1
                Dim tdPaso As New HtmlTableCell()
                tdPaso.InnerHtml = pasoReporte.IdPaso

                'CREA COLUMNA 2
                Dim tdBarras As New HtmlTableCell()

                ''''''''''''''''''''''''''''''''
                Dim divEnglobaTE As New HtmlGenericControl("DIV")
                divEnglobaTE.Attributes.Add("style", "width:100%;")

                Dim divBarraTE As New HtmlGenericControl("DIV")
                divBarraTE.Attributes.Add("style", "width:" + _diasEstimadosAnchoBarra.ToString() + "%; background-color:#0B0B61; height:10px; float: left; ")

                Dim divNumDiasTE As New HtmlGenericControl("DIV")
                divNumDiasTE.Attributes.Add("style", "float: left; margin-left:10px;")

                contadorLabels = contadorLabels + 1
                Dim lblNumDiasTE As New Label()
                lblNumDiasTE.ID = String.Format("lblNumDiasTEE_{0}_{1}", contadorLabels, pasoReporte.IdPaso)
                lblNumDiasTE.Text = pasoReporte.NumDiasTiempoEstimado.ToString()

                divNumDiasTE.Controls.Add(lblNumDiasTE)

                divEnglobaTE.Controls.Add(divBarraTE)
                divEnglobaTE.Controls.Add(divNumDiasTE)

                tdBarras.Controls.Add(divEnglobaTE)

                tdBarras.Controls.Add(New Literal With {.Text = "<br />"})
                ''''''''''''''''''''''''''''''''''''''
                Dim divEnglobaTR As New HtmlGenericControl("DIV")
                divEnglobaTR.Attributes.Add("style", "width:100%;")

                Dim divBarraTR As New HtmlGenericControl("DIV")
                divBarraTR.Attributes.Add("style", "width:" + _diasRealesAnchoBarra.ToString() + "%; background-color:#58FAD0; height:10px; float: left;")


                Dim divNumDiasTR As New HtmlGenericControl("DIV")
                divNumDiasTR.Attributes.Add("style", "float: left; margin-left:10px;")


                contadorLabels = contadorLabels + 1
                Dim lblNumDiasTR As New Label()
                lblNumDiasTR.ID = String.Format("lblNumDiasTER_{0}_{1}", contadorLabels, pasoReporte.IdPaso)
                lblNumDiasTR.Text = pasoReporte.NumDiasTiempoReal.ToString()

                divNumDiasTR.Controls.Add(lblNumDiasTR)

                divEnglobaTR.Controls.Add(divBarraTR)
                divEnglobaTR.Controls.Add(divNumDiasTR)
                tdBarras.Controls.Add(divEnglobaTR)

                tdPaso.Attributes.Add("style", "border:1px solid #999;")
                tdBarras.Attributes.Add("style", "border:1px solid #999;")

                'CREA COLUMNA USUARIOS Y COMENTARIOS
                If pasoReporte.LstUsuarioComentarios.Count > 0 Then
                    tdPaso.RowSpan = pasoReporte.LstUsuarioComentarios.Count
                    tdBarras.RowSpan = pasoReporte.LstUsuarioComentarios.Count

                    For i = 0 To pasoReporte.LstUsuarioComentarios.Count - 1

                        ''CREA MI NUEVO REENGLON
                        trRenglonPaso = New HtmlTableRow()

                        ''AGREGAR CELDAS INICIALES
                        If i = 0 Then
                            trRenglonPaso.Controls.Add(tdPaso)
                            trRenglonPaso.Controls.Add(tdBarras)
                        End If

                        ''Objeto usuario comentario
                        Dim objUsuComent As UsuarioComentario = pasoReporte.LstUsuarioComentarios.Item(i)

                        If Not IsNothing(objUsuComent) Then
                            Dim tdFechas As New HtmlTableCell()
                            Dim tdUsuario As New HtmlTableCell()
                            Dim tdComentarios As New HtmlTableCell()
                            Dim tdArchivos As New HtmlTableCell()

                            tdFechas.Attributes.Add("style", "border:1px solid #999;")
                            tdUsuario.Attributes.Add("style", "border:1px solid #999;")
                            tdComentarios.Attributes.Add("style", "border:1px solid #999;")
                            tdArchivos.Attributes.Add("style", "border:1px solid #999;")

                            tdFechas.InnerHtml = objUsuComent.FechaRegistrCom
                            tdUsuario.InnerHtml = objUsuComent.NombreCompleto
                            tdComentarios.InnerHtml = IIf(objUsuComent.ContenidoCom.Trim().Length > 0, objUsuComent.ContenidoCom.Trim(),
                                                          WebConfigurationManager.AppSettings("msgReporteSinComentario").ToString())

                            For Each objDoc As DatosDocumento In objUsuComent.ListaDocumentos
                                Dim divArchivosAdjuntos As New HtmlGenericControl("DIV")
                                divArchivosAdjuntos.Attributes.Add("style", "width:100%;")

                                Dim linkButton As New LinkButton()
                                linkButton.Text = objDoc.DscNombreArchivoOri
                                linkButton.CommandArgument = String.Format("Paso:{0}|NumDoc:{1}|NomArchivo:{2}", pasoReporte.IdPaso, objDoc.NumDocumento, objDoc.DscNombreArchivo)

                                AddHandler linkButton.Click, AddressOf linkButton_Click

                                divArchivosAdjuntos.Controls.Add(linkButton)
                                tdArchivos.Controls.Add(divArchivosAdjuntos)
                            Next

                            trRenglonPaso.Cells.Add(tdFechas)
                            trRenglonPaso.Cells.Add(tdUsuario)
                            trRenglonPaso.Cells.Add(tdComentarios)
                            trRenglonPaso.Cells.Add(tdArchivos)
                            tbPasos.Rows.Add(trRenglonPaso)
                        End If
                    Next
                Else
                    trRenglonPaso = New HtmlTableRow()
                    Dim tdRenglonUsu As New HtmlTableCell()
                    Dim tdRenglonCom As New HtmlTableCell()
                    Dim tdFecha As New HtmlTableCell()
                    Dim tdArchivos As New HtmlTableCell()

                    tdRenglonUsu.Attributes.Add("style", "border:1px solid #999;")
                    tdRenglonCom.Attributes.Add("style", "border:1px solid #999;")
                    tdFecha.Attributes.Add("style", "border:1px solid #999;")
                    tdArchivos.Attributes.Add("style", "border:1px solid #999;")

                    trRenglonPaso.Cells.Add(tdPaso)
                    trRenglonPaso.Cells.Add(tdBarras)
                    trRenglonPaso.Cells.Add(tdFecha)
                    trRenglonPaso.Cells.Add(tdRenglonUsu)
                    trRenglonPaso.Cells.Add(tdRenglonCom)
                    trRenglonPaso.Cells.Add(tdArchivos)

                    'AGREGA RENGLON A LA TABLA DE PASOS
                    tbPasos.Rows.Add(trRenglonPaso)
                End If
            Next

        End If

    End Sub


    Public Sub cargarPasosVisitaASPXRespado(ByVal idVisita As Integer)

        If idVisita > 0 Then

            Dim lstPasosReporte As New List(Of PasoReporte)
            lstPasosReporte = getPasosReporte(idVisita)

            Dim contadorLabels As Integer = 0
            Dim contadorLinks As Integer = 0
            For Each pasoReporte As PasoReporte In lstPasosReporte

                'CALCULA EL ANCHO DE LAS BARRAS (DIVS) DE TIEMPO ESTIMADO Y REAL
                Dim anchoPorDia As Single = 0
                If pasoReporte.NumDiasTiempoEstimado < 90 And pasoReporte.NumDiasTiempoReal < 90 Then
                    anchoPorDia = 90 / 90
                Else
                    If pasoReporte.NumDiasTiempoEstimado > pasoReporte.NumDiasTiempoReal Then
                        anchoPorDia = 90 / pasoReporte.NumDiasTiempoEstimado
                    Else
                        anchoPorDia = 90 / pasoReporte.NumDiasTiempoReal
                    End If
                End If

                Dim _diasEstimadosAnchoBarra As Integer = anchoPorDia * pasoReporte.NumDiasTiempoEstimado
                Dim _diasRealesAnchoBarra As Integer = anchoPorDia * pasoReporte.NumDiasTiempoReal


                'PREPARA EL RENGLON
                Dim trRenglonPaso As New HtmlTableRow()

                'CREA COLUMNA 1
                Dim tdPaso As New HtmlTableCell()
                tdPaso.InnerHtml = pasoReporte.IdPaso

                'CREA COLUMNA 2
                Dim tdBarras As New HtmlTableCell()

                ''''''''''''''''''''''''''''''''
                Dim divEnglobaTE As New HtmlGenericControl("DIV")
                divEnglobaTE.Attributes.Add("style", "width:100%;")

                Dim divBarraTE As New HtmlGenericControl("DIV")
                divBarraTE.Attributes.Add("style", "width:" + _diasEstimadosAnchoBarra.ToString() + "%; background-color:#0B0B61; height:10px; float: left; ")

                Dim divNumDiasTE As New HtmlGenericControl("DIV")
                divNumDiasTE.Attributes.Add("style", "float: left; margin-left:10px;")

                contadorLabels = contadorLabels + 1
                Dim lblNumDiasTE As New Label()
                lblNumDiasTE.ID = String.Format("lblNumDiasTEE_{0}_{1}", contadorLabels, pasoReporte.IdPaso)
                lblNumDiasTE.Text = pasoReporte.NumDiasTiempoEstimado.ToString()

                divNumDiasTE.Controls.Add(lblNumDiasTE)

                divEnglobaTE.Controls.Add(divBarraTE)
                divEnglobaTE.Controls.Add(divNumDiasTE)

                tdBarras.Controls.Add(divEnglobaTE)


                ''''''''''''''''''''''''''''''''''''''
                Dim divEnglobaTR As New HtmlGenericControl("DIV")
                divEnglobaTR.Attributes.Add("style", "width:100%;")

                Dim divBarraTR As New HtmlGenericControl("DIV")
                divBarraTR.Attributes.Add("style", "width:" + _diasRealesAnchoBarra.ToString() + "%; background-color:#58FAD0; height:10px; float: left;")


                Dim divNumDiasTR As New HtmlGenericControl("DIV")
                divNumDiasTR.Attributes.Add("style", "float: left; margin-left:10px;")


                contadorLabels = contadorLabels + 1
                Dim lblNumDiasTR As New Label()
                lblNumDiasTR.ID = String.Format("lblNumDiasTER_{0}_{1}", contadorLabels, pasoReporte.IdPaso)
                lblNumDiasTR.Text = pasoReporte.NumDiasTiempoReal.ToString()

                divNumDiasTR.Controls.Add(lblNumDiasTR)

                divEnglobaTR.Controls.Add(divBarraTR)
                divEnglobaTR.Controls.Add(divNumDiasTR)

                tdBarras.Controls.Add(divEnglobaTR)


                'CREA COLUMNA USUARIOS Y COMENTARIOS
                Dim tdUsuarios As New HtmlTableCell()

                Dim ltTablaUsu As New HtmlTable
                ltTablaUsu.Attributes.Add("style", "width:100%; Height:100%; border-collapse: collapse;")

                Dim trRenglonUsu As HtmlTableRow
                Dim i As Integer = 1

                For Each ldUsuComentario As UsuarioComentario In pasoReporte.LstUsuarioComentarios
                    trRenglonUsu = New HtmlTableRow
                    Dim tcRenglonUsu As New HtmlTableCell()
                    Dim tcRenglonCom As New HtmlTableCell()

                    tcRenglonUsu.InnerHtml = ldUsuComentario.IdUsuario
                    ''tcRenglonCom.InnerHtml = ldUsuComentario.ListaComentarios.toListHtml()

                    If pasoReporte.LstUsuarioComentarios.Count > 1 Then
                        If i = 1 Then
                            tcRenglonCom.Attributes.Add("style", "border-left:1px solid #999; border-bottom:1px solid #999; width:65.7%; Height:100%")
                            tcRenglonUsu.Attributes.Add("style", "border-right:1px solid #999; border-bottom:1px solid #999; width:34.3%; Height:100%")
                        Else
                            If i = pasoReporte.LstUsuarioComentarios.Count Then
                                tcRenglonCom.Attributes.Add("style", "border-left:1px solid #999; border-top:1px solid #999; width:65.7%; Height:100%")
                                tcRenglonUsu.Attributes.Add("style", "border-right:1px solid #999; border-top:1px solid #999; width:34.3%; Height:100%")
                            Else
                                tcRenglonCom.Attributes.Add("style", "border-left:1px solid #999; border-bottom:1px solid #999; border-top:1px solid #999; width:65.7%; Height:100%")
                                tcRenglonUsu.Attributes.Add("style", "border-right:1px solid #999; border-bottom:1px solid #999; border-top:1px solid #999; width:34.3%; Height:100%")
                            End If
                        End If

                    Else
                        tcRenglonCom.Attributes.Add("style", "border-left:1px solid #999; width:65.7%; Height:100%")
                        tcRenglonUsu.Attributes.Add("style", "border-right:1px solid #999; width:34.3%; Height:100%")
                    End If

                    trRenglonUsu.Cells.Add(tcRenglonUsu)
                    trRenglonUsu.Cells.Add(tcRenglonCom)

                    ltTablaUsu.Rows.Add(trRenglonUsu)

                    i = i + 1
                Next

                tdUsuarios.Controls.Add(ltTablaUsu)
                'tdComentarios.Controls.Add(ltTablaComentarios)


                'CREA COLUMNA ARCHIVOS POR PASO
                Dim tdArchivos As New HtmlTableCell()


                For Each documento As DatosDocumento In pasoReporte.LstDocumentos

                    contadorLinks = contadorLinks + 1

                    Dim divArchivosAdjuntos As New HtmlGenericControl("DIV")
                    divArchivosAdjuntos.Attributes.Add("style", "width:100%;")

                    Dim linkButton As New LinkButton()
                    'linkButton.ID = String.Format("linkDoc{0}_Paso{1}_NumDoc{2}", contadorLinks, pasoReporte.IdPaso, documento.NumDocumento)
                    linkButton.Text = documento.DscNombreArchivoOri
                    linkButton.CommandArgument = String.Format("Paso:{0}|NumDoc:{1}|NomArchivo:{2}", pasoReporte.IdPaso, documento.NumDocumento, documento.DscNombreArchivo)

                    AddHandler linkButton.Click, AddressOf linkButton_Click

                    divArchivosAdjuntos.Controls.Add(linkButton)
                    tdArchivos.Controls.Add(divArchivosAdjuntos)

                Next

                Dim liPixeles As Integer = 15
                Dim liPixelesExtras As Integer = 30

                'AGREGA EL BORDE A LAS COLUMNAS (CELDAS)
                tdPaso.Attributes.Add("style", "border:1px solid #999;")
                tdBarras.Attributes.Add("style", "border:1px solid #999;")
                tdArchivos.Attributes.Add("style", "border:1px solid #999;")
                'tdComentarios.Attributes.Add("style", "border:1px solid #999;")

                If pasoReporte.LstDocumentos.Count > 1 Then
                    tdUsuarios.Attributes.Add("style", "border:1px solid #999; Height:" & ((pasoReporte.LstDocumentos.Count * liPixeles) + liPixelesExtras).ToString() & "px")
                Else
                    tdUsuarios.Attributes.Add("style", "border:1px solid #999;")
                End If

                tdUsuarios.ColSpan = 2

                'AGREGA LAS COLUMNAS (CELDAS) AL RENGLON
                trRenglonPaso.Cells.Add(tdPaso)
                trRenglonPaso.Cells.Add(tdBarras)
                trRenglonPaso.Cells.Add(tdUsuarios)
                ' trRenglonPaso.Cells.Add(tdComentarios)
                trRenglonPaso.Cells.Add(tdArchivos)

                'AGREGA RENGLON A LA TABLA DE PASOS
                tbPasos.Rows.Add(trRenglonPaso)

            Next

        End If

    End Sub

    Public Function generarSeguimientoDetallePDF(ByVal idVisita As Integer) As String

        Dim RutaPDF As String = String.Empty

        If idVisita > 0 Then

            Dim encabezadoReporte As New EncabezadoReporteVisita()
            encabezadoReporte = AccesoBD.getEncabezadoReporteVisita(idVisita)

            Dim lstEncabezado As New List(Of String)


            lstEncabezado.Add(encabezadoReporte.FolioVisita)
            lstEncabezado.Add(encabezadoReporte.FechaRegistro.ToString("dd/MM/yyyy HH:mm"))
            lstEncabezado.Add(encabezadoReporte.DscPasoActual)

            Dim lstPasoReporte As New List(Of PasoReporte)
            lstPasoReporte = getPasosReporte(idVisita)

            Dim PDFManager As New Utilerias.PDF()
            RutaPDF = PDFManager.CreateDocumentSeguimientoDetalle(lstEncabezado, lstPasoReporte, False)

        End If

        Return RutaPDF

    End Function

    Public Function getPasosReporte(ByVal idVisita As Integer) As List(Of PasoReporte)

        Dim lstPasosReporte As New List(Of PasoReporte)

        'Obtiene el catalog de pasos
        Dim lstCatalgoPasos As New List(Of Paso)
        lstCatalgoPasos = AccesoBD.getCatalogoPasos()

        'Obtiene  todos los paso de la visita
        Dim lstTodosPasosVisita As New List(Of PasoProcesoVisita)
        lstTodosPasosVisita = AccesoBD.getTodosPasosVisita(idVisita)

        For Each pasoVisita As PasoProcesoVisita In lstTodosPasosVisita

            Dim pasoReporte As New PasoReporte()
            pasoReporte.IdVisitaGenerado = pasoVisita.IdVisitaGenerado
            pasoReporte.IdPaso = pasoVisita.IdPaso


            Dim diasTiempoEstimado As Integer = 0
            Dim diasTiempoReal As Integer = 0

            pasoReporte.NumDiasTiempoEstimado = pasoVisita.DiasEstimadosPaso


            'OBTIENE TIEMPO REAL
            If pasoVisita.FechaFin = DateTime.MinValue Then

                'Significa que el paso aun no finaliza,
                'entonces se toma la fecha actual como fecha fin
                pasoVisita.FechaFin = DateTime.Now

            End If

            pasoReporte.NumDiasTiempoReal = pasoVisita.DiasTranscurridos

            Dim lstDocumentos As New List(Of DatosDocumento)
            lstDocumentos = AccesoBD.getDocumentosPasosVisita(idVisita, pasoVisita.IdPaso)

            pasoReporte.LstDocumentos = lstDocumentos

            Dim lstUsuarioComentarios As New List(Of UsuarioComentario)
            lstUsuarioComentarios = AccesoBD.getUsuarioComentariosPasosVisita(idVisita, pasoVisita.IdPaso)

            ''3 MOMENTOS PARA RECUPERAR LOS DOCUMENTOS DE UN COMENTARIO
            ''1ro: CUANDO J = 0, UNICAMENTE SE CONSIRERA LA FECHA SIGUIENTE
            ''2do: J > 0, SE DEBEN DE OBTENER TODOS LOS DOCUMETOS ENTRE LA FECHA DEL ESTATUS Y LA FECHA SIGUIENTE
            ''3er: ULTIMO MOVIMIENTO, NO SE CONSIDERA LA FECHA SIGUIENTE SOLO LA FECHA ACTUAL
            'Dim ldFechaSig As DateTime = Nothing
            'For Each objUsuCom As UsuarioComentario In lstUsuarioComentarios
            '    For j As Integer = 0 To objUsuCom.ListaComentarios.Count - 2
            '        ldFechaSig = objUsuCom.ListaComentarios(j + 1).FechaRegistro
            '        If j = 0 Then
            '            objUsuCom.ListaComentarios(j).ListaDocumentos = (From objDocs As DatosDocumento In lstDocumentos
            '                                      Where objDocs.FechaRegistro < ldFechaSig
            '                                      Select objDocs).ToList()
            '        Else
            '            objUsuCom.ListaComentarios(j).ListaDocumentos = (From objDocs As DatosDocumento In lstDocumentos
            '                                      Where objDocs.FechaRegistro >= objUsuCom.ListaComentarios(j).FechaRegistro And
            '                                      objDocs.FechaRegistro < ldFechaSig
            '                                      Select objDocs).ToList()
            '        End If
            '    Next

            '    objUsuCom.ListaComentarios(objUsuCom.ListaComentarios.Count - 1).ListaDocumentos = (From objDocs As DatosDocumento In lstDocumentos
            '                                      Where objDocs.FechaRegistro > objUsuCom.ListaComentarios(objUsuCom.ListaComentarios.Count - 1).FechaRegistro
            '                                      Select objDocs).ToList()
            'Next

            pasoReporte.LstUsuarioComentarios = lstUsuarioComentarios

            lstPasosReporte.Add(pasoReporte)
        Next

        Return lstPasosReporte

    End Function


    Function calcularDiasTranscurridos(ByVal fecha_ini As DateTime, ByVal fecha_fin As DateTime) As Integer
        Dim numDias As Integer = 0

        Dim lstDiasFeriados As New List(Of DateTime)
        lstDiasFeriados = AccesoBD.getDiasFeriados()

        'Este proceso se hace para tener solo las fechas, sin las horas, minutos y segundos, 
        'ya que por estos datos puede ser mayor o menor una fecha y las comparaciones entre fechas serían incorrectas
        Dim lstDiasFeriadosSoloFechas As New List(Of DateTime)
        For Each df As DateTime In lstDiasFeriados
            Dim value As DateTime = New DateTime(df.Year, df.Month, df.Day)
            lstDiasFeriadosSoloFechas.Add(value)
        Next
        Dim fechaInicio As DateTime = New DateTime(fecha_ini.Year, fecha_ini.Month, fecha_ini.Day)
        Dim fechaFin As DateTime = New DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day)


        Dim res1 As Integer = Date.Compare(fechaInicio, fechaFin)
        '0  - es la misma fecha
        '-1 - es mas pequeña la fechaInicio que la fechaFin
        '1  - es mas grande la fechaInicio que la fechaFin
        If res1 = -1 Then

            Dim fechaTemp As DateTime
            fechaTemp = fechaInicio

            While res1 = -1

                fechaTemp = fechaTemp.AddDays(1)

                If fechaTemp.DayOfWeek = DayOfWeek.Saturday Or fechaTemp.DayOfWeek = DayOfWeek.Sunday Then
                    'Es fin de semana, no se contabiliza
                Else



                    Dim esFeriado As Boolean = False

                    If Not IsNothing(lstDiasFeriadosSoloFechas) Then

                        Dim res2 As Integer

                        For Each diaFeriado As DateTime In lstDiasFeriadosSoloFechas

                            res2 = Date.Compare(fechaTemp, diaFeriado)

                            If res2 = 0 Then
                                esFeriado = True
                                Exit For
                            End If

                        Next

                    End If

                    If esFeriado = True Then
                        'Es feriado, no se contabiliza
                    Else
                        numDias = numDias + 1

                    End If

                End If

                res1 = Date.Compare(fechaTemp, fechaFin)

            End While
        End If

        Return numDias

    End Function

    Private Sub ConfigurarSharePointSepris(ByRef Shp As Utilerias.SharePointManager)

        Shp.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSEPRIS").ToString()
        Shp.Usuario = WebConfigurationManager.AppSettings("SharePointUserSEPRIS").ToString()
        Shp.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SharePointPasswordSEPRIS").ToString())
        Shp.Dominio = WebConfigurationManager.AppSettings("SharePointDomainSEPRIS").ToString()
        Shp.Biblioteca = WebConfigurationManager.AppSettings("SharePointBibliotecaSEPRIS").ToString()

    End Sub

    Private Sub ConfigurarSharePointSeprisSicod(ByRef Shp As Utilerias.SharePointManager)

        Shp.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSICOD").ToString()
        Shp.Usuario = WebConfigurationManager.AppSettings("UsuarioSpSICOD").ToString()
        Shp.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("PassEncSpSICOD").ToString())
        Shp.Dominio = WebConfigurationManager.AppSettings("DomainSICOD").ToString()
        Shp.Biblioteca = WebConfigurationManager.AppSettings("DocLibrarySICOD").ToString()

    End Sub

    Public Sub ConfigurarSharePointSeprisSisvig(ByRef Shp As Utilerias.SharePointManager)
        Shp.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSISVIG").ToString()
        Shp.Usuario = WebConfigurationManager.AppSettings("SISVIGUSUARIOSp").ToString()
        Shp.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SISVIGPassEncSp").ToString())
        Shp.Dominio = WebConfigurationManager.AppSettings("SISVIGDomainSp").ToString()
        Shp.Biblioteca = WebConfigurationManager.AppSettings("DocLibrarySISVIG").ToString()
    End Sub

    Protected Sub imgAnterior_Click(sender As Object, e As ImageClickEventArgs)
        Response.Redirect("../Procesos/SeguimientoReporte.aspx")
    End Sub

    Private Sub imgProcesoVisitaAmbos_Click(sender As Object, e As ImageClickEventArgs) Handles imgProcesoVisitaAmbos.Click
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroOpcionesImgProcesos();", True)
    End Sub

End Class