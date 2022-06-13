Imports Utilerias
Imports System.IO
Public Class SeguimientoReporte
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                If Not IsNothing(Session("ID_VISITA_SEGUIMIENTO")) Then
                    Dim idVisitaSeguimiento As Integer = Session("ID_VISITA_SEGUIMIENTO")
                    cargarEncabezadoReporteVisitaASPX(idVisitaSeguimiento)
                    cargarGraficaASPX(idVisitaSeguimiento)
                End If
            End If

            ' ''Cargar imagen proceso ins sancion
            'If usuario.IdArea = 36 Then
            '    imgProcesoInspSancion.ImageUrl = "~/Imagenes/ProcesoInspSan/" & Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.NombreImagenProcesoInspeccionSancVF)
            'Else
            '    imgProcesoInspSancion.ImageUrl = "~/Imagenes/ProcesoInspSan/" & Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.NombreImagenProcesoInspeccionSancOtras)
            'End If
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

    Protected Sub btnSeguimientoDetalle_Click(sender As Object, e As ImageClickEventArgs) Handles btnSeguimientoDetalle.Click
        Response.Redirect("../Procesos/SeguimientoDetalle.aspx")
    End Sub

    Protected Sub imgVistaPreliminar_Click(sender As Object, e As ImageClickEventArgs) Handles imgVistaPreliminar.Click

        Dim RutaPDF As String = String.Empty

        If Not IsNothing(Session("ID_VISITA_SEGUIMIENTO")) Then

            Dim idVisitaSeguimiento As Integer = Session("ID_VISITA_SEGUIMIENTO")

            RutaPDF = generarSeguimientoReportePDF(idVisitaSeguimiento)

            Dim nombreArchivo As String
            nombreArchivo = Path.GetFileName(RutaPDF)

            If Not IsNothing(nombreArchivo) And nombreArchivo <> String.Empty Then

                Dim url As String
                url = "../Reportes/" + nombreArchivo

                Dim script As String
                script = "window.open(""{0}"", ""{1}"")"
                script = String.Format(script, url, "_blank")
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Redirect", script, True)

            End If
            
        End If

    End Sub

    Public Sub cargarEncabezadoReporteVisitaASPX(ByVal idVisita As Integer)

        If idVisita > 0 Then
            Dim encabezadoReporte As New EncabezadoReporteVisita()
            encabezadoReporte = AccesoBD.getEncabezadoReporteVisita(idVisita)

            lblFolioVisita.Text = encabezadoReporte.FolioVisita
            lblFechaRegistro.Text = encabezadoReporte.FechaRegistro.ToString("dd/MM/yyyy HH:mm")
            lblEstatus.Text = encabezadoReporte.DscPasoActual

        End If

    End Sub

    Public Sub cargarGraficaASPX(ByVal idVisita As Integer)

        If idVisita > 0 Then

            Dim lstDiasTiempoEstimadoReal As New List(Of DatosEtapa)

            lstDiasTiempoEstimadoReal = getDias_Estimados_y_Reales(idVisita)

            For Each dEtapa As DatosEtapa In lstDiasTiempoEstimadoReal
                graficarEtapaASPX(dEtapa.NumEtapa, dEtapa.DiasTiempoEstimado, dEtapa.DiasTiempoReal, dEtapa.DscRangoDias)
            Next

        End If

    End Sub

    Public Sub graficarEtapaASPX(ByVal numEtapa As Integer, ByVal diasEstimados As Integer, ByVal diasReales As Integer, ByVal lblRangoPasos As String)

        'Los divs que simulan la barra de días transurridos son de un de hasta width 93%, 93 días, ancho del día 1%
        'Si los días transcurridos exceden los 93 días, se dividen los dias transcurridos entre 93 para obtener el ancho del día 
        Dim anchoPorDia As Single = 0
        If diasEstimados < 93 And diasReales < 93 Then
            anchoPorDia = 93 / 93
        Else
            If diasEstimados > diasReales Then
                anchoPorDia = 93 / diasEstimados
            Else
                anchoPorDia = 93 / diasReales
            End If
        End If

        Dim _diasEstimadosAnchoBarra As Integer = anchoPorDia * diasEstimados
        Dim _diasRealesAnchoBarra As Integer = anchoPorDia * diasReales

        Dim strDiasTE As String = String.Empty
        Dim strDiasTR As String = String.Empty

        If diasEstimados = 1 Then
            strDiasTE = " día"
        Else
            strDiasTE = " días"
        End If

        If diasReales = 1 Then
            strDiasTR = " día"
        Else
            strDiasTR = " días"
        End If

      Select Case numEtapa
         Case 0
            divTotalDias.Attributes.Add("style", "width:" + _diasEstimadosAnchoBarra.ToString() + "%; background-color:#0B0B61; height:10px; float: left; ")
            divTotalDiasR.Attributes.Add("style", "width:" + _diasRealesAnchoBarra.ToString() + "%; background-color:#58FAD0; height:10px; float: left; ")
            lblTETotal.Text = diasEstimados.ToString + strDiasTE
            lblTRTotal.Text = diasReales.ToString() + strDiasTR
         Case 1
            divTEE1.Attributes.Add("style", "width:" + _diasEstimadosAnchoBarra.ToString() + "%; background-color:#0B0B61; height:10px; float: left; ")
            divTRE1.Attributes.Add("style", "width:" + _diasRealesAnchoBarra.ToString() + "%; background-color:#58FAD0; height:10px; float: left; ")
            lblNumDiasTEE1.Text = diasEstimados.ToString + strDiasTE
            lblNumDiasTRE1.Text = diasReales.ToString() + strDiasTR
            lblRangoPaso1.Text = lblRangoPasos.ToString()
         Case 2
            divTEE2.Attributes.Add("style", "width:" + _diasEstimadosAnchoBarra.ToString() + "%; background-color:#0B0B61; height:10px; float: left; ")
            divTRE2.Attributes.Add("style", "width:" + _diasRealesAnchoBarra.ToString() + "%; background-color:#58FAD0; height:10px; float: left; ")
            lblNumDiasTEE2.Text = diasEstimados.ToString + strDiasTE
            lblNumDiasTRE2.Text = diasReales.ToString() + strDiasTR
            lblRangoPaso2.Text = lblRangoPasos.ToString()
         Case 3
            divTEE3.Attributes.Add("style", "width:" + _diasEstimadosAnchoBarra.ToString() + "%; background-color:#0B0B61; height:10px; float: left; ")
            divTRE3.Attributes.Add("style", "width:" + _diasRealesAnchoBarra.ToString() + "%; background-color:#58FAD0; height:10px; float: left; ")
            lblNumDiasTEE3.Text = diasEstimados.ToString + strDiasTE
            lblNumDiasTRE3.Text = diasReales.ToString() + strDiasTR
            lblRangoPaso3.Text = lblRangoPasos.ToString()
         Case 4
            divTEE4.Attributes.Add("style", "width:" + _diasEstimadosAnchoBarra.ToString() + "%; background-color:#0B0B61; height:10px; float: left; ")
            divTRE4.Attributes.Add("style", "width:" + _diasRealesAnchoBarra.ToString() + "%; background-color:#58FAD0; height:10px; float: left; ")
            lblNumDiasTEE4.Text = diasEstimados.ToString + strDiasTE
            lblNumDiasTRE4.Text = diasReales.ToString() + strDiasTR
            lblRangoPaso4.Text = lblRangoPasos.ToString()
         Case 5
            divTEE5.Attributes.Add("style", "width:" + _diasEstimadosAnchoBarra.ToString() + "%; background-color:#0B0B61; height:10px; float: left; ")
            divTRE5.Attributes.Add("style", "width:" + _diasRealesAnchoBarra.ToString() + "%; background-color:#58FAD0; height:10px; float: left; ")
            lblNumDiasTEE5.Text = diasEstimados.ToString + strDiasTE
            lblNumDiasTRE5.Text = diasReales.ToString() + strDiasTR
            lblRangoPaso5.Text = lblRangoPasos.ToString()
         Case 6
            divTEE6.Attributes.Add("style", "width:" + _diasEstimadosAnchoBarra.ToString() + "%; background-color:#0B0B61; height:10px; float: left; ")
            divTRE6.Attributes.Add("style", "width:" + _diasRealesAnchoBarra.ToString() + "%; background-color:#58FAD0; height:10px; float: left; ")
            lblNumDiasTEE6.Text = diasEstimados.ToString + strDiasTE
            lblNumDiasTRE6.Text = diasReales.ToString() + strDiasTR
            lblRangoPaso6.Text = lblRangoPasos.ToString()
         Case 7
            divTEE7.Attributes.Add("style", "width:" + _diasEstimadosAnchoBarra.ToString() + "%; background-color:#0B0B61; height:10px; float: left; ")
            divTRE7.Attributes.Add("style", "width:" + _diasRealesAnchoBarra.ToString() + "%; background-color:#58FAD0; height:10px; float: left; ")
            lblNumDiasTEE7.Text = diasEstimados.ToString + strDiasTE
            lblNumDiasTRE7.Text = diasReales.ToString() + strDiasTR
            lblRangoPaso7.Text = lblRangoPasos.ToString()
      End Select

    End Sub

    Public Function generarSeguimientoReportePDF(ByVal idVisita As Integer) As String

        Dim RutaPDF As String = String.Empty

        If idVisita > 0 Then

            Dim encabezadoReporte As New EncabezadoReporteVisita()
            encabezadoReporte = AccesoBD.getEncabezadoReporteVisita(idVisita)

            Dim etiquetas As New Dictionary(Of String, String)
            etiquetas.Add("Folio: ", encabezadoReporte.FolioVisita)
            etiquetas.Add("Fecha de Registro: ", encabezadoReporte.FechaRegistro.ToString("dd/MM/yyyy HH:mm"))
            etiquetas.Add("Paso actual: ", encabezadoReporte.DscPasoActual)

            Dim lstDatosEtapa As New List(Of DatosEtapa)
            lstDatosEtapa = getDias_Estimados_y_Reales(idVisita)

            Dim PDFManager As New Utilerias.PDF()
            RutaPDF = PDFManager.CreateDocumentSeguimientoReporte(etiquetas, lstDatosEtapa, False)

        End If

        Return RutaPDF

    End Function

    Public Function getDias_Estimados_y_Reales(ByVal idVisita As Integer) As List(Of DatosEtapa)

        Dim lstDatosEtapas As New List(Of DatosEtapa)
        Dim datosEtapa As DatosEtapa = Nothing
        Dim datosEtapaV17 As DatosEtapa = Nothing

        'Validar la fecha de registro de la visita
        Dim objVisita As New Visita()
        objVisita = AccesoBD.getDetalleVisita(idVisita, 35)

        Dim fechaRegVisita As Date = CDate(objVisita.FechaRegistro.ToString())
        Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

      lblFechIniVis.Text = objVisita.FechaInicioVisita

        If (Convert.ToDateTime(fechaRegVisita).ToString("yyyy/MM/dd") > Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
         'CONSIDERA EL PROCESO 2017

         'PARA LA GRÁFICA DE TIEMPOS TOTALES
         datosEtapaV17 = New DatosEtapa()
         datosEtapaV17.NumEtapa = 0
         datosEtapaV17.DscEtapa = "Total de días hábiles desde el registro de la visita"
         datosEtapaV17.DiasTiempoEstimado = calcularDiasEstimadosTotales(4, 19, idVisita)

         'Dim diasT = calcularDiasReales(idVisita, 1, 5)
         'diasT = diasT + calcularDiasReales(idVisita, 6, 6)
         'diasT = diasT + calcularDiasReales(idVisita, 8, 14)
         'diasT = diasT + calcularDiasReales(idVisita, 15, 19)


         datosEtapaV17.DiasTiempoReal = calcularDiasReales(idVisita, 4, 19, 1)
         datosEtapaV17.DscRangoDias = "Pasos del 1 al 19"
         lstDatosEtapas.Add(datosEtapaV17)

            datosEtapaV17 = New DatosEtapa()
            datosEtapaV17.NumEtapa = 1
            datosEtapaV17.DscEtapa = "Inicio de visita de inspección"
            datosEtapaV17.DiasTiempoEstimado = calcularDiasEstimados_v15_5(1, 5, idVisita)
            datosEtapaV17.DiasTiempoReal = calcularDiasReales(idVisita, 1, 5)
            datosEtapaV17.DscRangoDias = "Pasos del 1 al 5"
            lstDatosEtapas.Add(datosEtapaV17)

            datosEtapaV17 = New DatosEtapa()
            datosEtapaV17.NumEtapa = 2
            datosEtapaV17.DscEtapa = "Desarrollo de la visita de inspección"
            datosEtapaV17.DiasTiempoEstimado = calcularDiasEstimados(6, 6, idVisita)
            datosEtapaV17.DiasTiempoReal = calcularDiasReales(idVisita, 6, 6)
            datosEtapaV17.DscRangoDias = "Pasos del 6 al 7"
            lstDatosEtapas.Add(datosEtapaV17)

            datosEtapaV17 = New DatosEtapa()
            datosEtapaV17.NumEtapa = 3
            datosEtapaV17.DscEtapa = "Resultados de la visita de inspección"
            datosEtapaV17.DiasTiempoEstimado = calcularDiasEstimados_V17(8, 14, idVisita, objVisita.IdArea)
            datosEtapaV17.DiasTiempoReal = calcularDiasReales(idVisita, 8, 14)
            datosEtapaV17.DscRangoDias = "Pasos del 8 al 14"
            lstDatosEtapas.Add(datosEtapaV17)

            datosEtapaV17 = New DatosEtapa()
            datosEtapaV17.NumEtapa = 4
            datosEtapaV17.DscEtapa = "Cierre de visita de inspección"
            datosEtapaV17.DiasTiempoEstimado = calcularDiasEstimados_V17_4(15, 19, idVisita, objVisita.IdArea)
            datosEtapaV17.DiasTiempoReal = calcularDiasReales(idVisita, 15, 19)
            datosEtapaV17.DscRangoDias = "Pasos del 15 al 19"
            lstDatosEtapas.Add(datosEtapaV17)

            datosEtapaV17 = New DatosEtapa()
            datosEtapaV17.NumEtapa = 5
            datosEtapaV17.DscEtapa = "Sanción de visita de inspección"
            datosEtapaV17.DiasTiempoEstimado = calcularDiasEstimados_v15(21, 31, idVisita)
            datosEtapaV17.DiasTiempoReal = calcularDiasReales(idVisita, 21, 31)
            datosEtapaV17.DscRangoDias = "Pasos del 21 al 31"
            lstDatosEtapas.Add(datosEtapaV17)

            datosEtapaV17 = New DatosEtapa()
            datosEtapaV17.NumEtapa = 6
            datosEtapaV17.DscEtapa = "Contencioso"
            datosEtapaV17.DiasTiempoEstimado = calcularDiasEstimados_v15(32, 41, idVisita)
            datosEtapaV17.DiasTiempoReal = calcularDiasReales(idVisita, 32, 41)
            datosEtapaV17.DscRangoDias = "Paso del 32 al 41"
            lstDatosEtapas.Add(datosEtapaV17)

            datosEtapaV17 = New DatosEtapa()
            datosEtapaV17.NumEtapa = 7
            datosEtapaV17.DscEtapa = "Proceso de inspección - Sanción"
            datosEtapaV17.DiasTiempoEstimado = calcularDiasEstimados_v15_5(1, 19, idVisita)
            datosEtapaV17.DiasTiempoReal = calcularDiasReales(idVisita, 1, 19)
            datosEtapaV17.DscRangoDias = "Pasos del 1 al 19"
            lstDatosEtapas.Add(datosEtapaV17)
      Else
         'PARA LA GRÁFICA DE TIEMPOS TOTALES
         datosEtapa = New DatosEtapa()
         datosEtapa.NumEtapa = 0
         datosEtapa.DscEtapa = "Total de días hábiles desde el registro de la visita"
         datosEtapa.DiasTiempoEstimado = calcularDiasEstimadosTotales(4, 19, idVisita)
         datosEtapa.DiasTiempoReal = calcularDiasReales(idVisita, 4, 19)
         datosEtapa.DscRangoDias = "Pasos de 1 al 19"
         lstDatosEtapas.Add(datosEtapa)

         'CONSIDERA EL PROCESO 2015
         datosEtapa = New DatosEtapa()
         datosEtapa.NumEtapa = 1
         datosEtapa.DscEtapa = "Inicio de visita de inspección"
         datosEtapa.DiasTiempoEstimado = calcularDiasEstimados_v15(1, 4, idVisita)
         datosEtapa.DiasTiempoReal = calcularDiasReales(idVisita, 1, 4)
         datosEtapa.DscRangoDias = "Pasos del 1 al 4"
         lstDatosEtapas.Add(datosEtapa)

         datosEtapa = New DatosEtapa()
         datosEtapa.NumEtapa = 2
         datosEtapa.DscEtapa = "Desarrollo de la visita de inspección"
         datosEtapa.DiasTiempoEstimado = calcularDiasEstimados_v15(5, 8, idVisita)
         datosEtapa.DiasTiempoReal = calcularDiasReales(idVisita, 5, 8)
         datosEtapa.DscRangoDias = "Pasos del 5 al 8"
         lstDatosEtapas.Add(datosEtapa)

         datosEtapa = New DatosEtapa()
         datosEtapa.NumEtapa = 3
         datosEtapa.DscEtapa = "Resultados de la visita de inspección"
         datosEtapa.DiasTiempoEstimado = calcularDiasEstimados(9, 15, idVisita)
         datosEtapa.DiasTiempoReal = calcularDiasReales(idVisita, 9, 15)
         datosEtapa.DscRangoDias = "Pasos del 9 al 15"
         lstDatosEtapas.Add(datosEtapa)

         datosEtapa = New DatosEtapa()
         datosEtapa.NumEtapa = 4
         datosEtapa.DscEtapa = "Cierre de visita de inspección"
         datosEtapa.DiasTiempoEstimado = calcularDiasEstimados(16, 19, idVisita)
         datosEtapa.DiasTiempoReal = calcularDiasReales(idVisita, 16, 19)
         datosEtapa.DscRangoDias = "Pasos del 16 al 19"
         lstDatosEtapas.Add(datosEtapa)

         datosEtapa = New DatosEtapa()
         datosEtapa.NumEtapa = 5
         datosEtapa.DscEtapa = "Sanción de visita de inspección"
         datosEtapa.DiasTiempoEstimado = calcularDiasEstimados_v15(20, 29, idVisita)
         datosEtapa.DiasTiempoReal = calcularDiasReales(idVisita, 20, 29)
         datosEtapa.DscRangoDias = "Pasos del 20 al 29"
         lstDatosEtapas.Add(datosEtapa)

         datosEtapa = New DatosEtapa()
         datosEtapa.NumEtapa = 6
         datosEtapa.DscEtapa = "Contencioso"
         datosEtapa.DiasTiempoEstimado = calcularDiasEstimados_v15(30, 37, idVisita)
         datosEtapa.DiasTiempoReal = calcularDiasReales(idVisita, 30, 27)
         datosEtapa.DscRangoDias = "Pasos del 30 al 37"
         lstDatosEtapas.Add(datosEtapa)

         datosEtapa = New DatosEtapa()
         datosEtapa.NumEtapa = 7
         datosEtapa.DscEtapa = "Proceso de inspección - Sanción"
         datosEtapa.DiasTiempoEstimado = calcularDiasEstimados_v15(1, 29, idVisita)
         datosEtapa.DiasTiempoReal = calcularDiasReales(idVisita, 1, 29)
         datosEtapa.DscRangoDias = "Pasos del 1 al 29"
         lstDatosEtapas.Add(datosEtapa)
        End If

        Return lstDatosEtapas

    End Function

    Public Function calcularDiasEstimados_V17(ByVal idPasoInicia As Integer, ByVal idPasoFin As Integer,
                                          Optional ByVal piIdVista As Integer = Constantes.Todos,
                                          Optional ByVal piIdArea As Integer = 0) As Integer
        Dim diasEstimados As Integer = 0

        Try
            Dim lstPasos As New List(Of Paso)

            lstPasos = AccesoBD.getCatalogoPasos(, piIdVista)

            If Not IsNothing(lstPasos) And lstPasos.Count > 0 Then
                For Each paso As Paso In lstPasos

                    If piIdArea = 36 Then

                        If paso.IdPaso >= idPasoInicia And paso.IdPaso <= idPasoFin And Not paso.IdPaso.Equals(11) Then
                            diasEstimados = diasEstimados + IIf(paso.EnProrroga = Constantes.Verdadero, paso.NumDiasMax, paso.NumDiasMin)
                        End If
                    Else
                        'If paso.EnProrroga = Constantes.Verdadero Then
                        '    If paso.IdPaso >= idPasoInicia And paso.IdPaso <= idPasoFin Then
                        '        diasEstimados = diasEstimados + IIf(paso.EnProrroga = Constantes.Verdadero, paso.NumDiasMax, paso.NumDiasMin)
                        '    End If
                        'Else
                        If paso.IdPaso >= idPasoInicia And paso.IdPaso <= idPasoFin And Not paso.IdPaso.Equals(11) Then
                            diasEstimados = diasEstimados + IIf(paso.EnProrroga = Constantes.Verdadero, paso.NumDiasMax, paso.NumDiasMin)
                        End If
                        'End If
                    End If

                Next
            End If

        Catch ex As Exception

        End Try

        Return diasEstimados

    End Function

    Public Function calcularDiasEstimados_V17_4(ByVal idPasoInicia As Integer, ByVal idPasoFin As Integer,
                                          Optional ByVal piIdVista As Integer = Constantes.Todos,
                                          Optional ByVal piIdArea As Integer = 0) As Integer
        Dim diasEstimados As Integer = 0

        Try
            Dim lstPasos As New List(Of Paso)

            lstPasos = AccesoBD.getCatalogoPasos(, piIdVista)

            If Not IsNothing(lstPasos) And lstPasos.Count > 0 Then
                For Each paso As Paso In lstPasos

                    If piIdArea = 36 Then

                        If paso.IdPaso >= idPasoInicia And paso.IdPaso <= idPasoFin And Not paso.IdPaso.Equals(17) Then
                            diasEstimados = diasEstimados + IIf(paso.EnProrroga = Constantes.Verdadero, paso.NumDiasMax, paso.NumDiasMin)
                        End If
                    Else
                        'If paso.EnProrroga = Constantes.Verdadero Then
                        '    If paso.IdPaso >= idPasoInicia And paso.IdPaso <= idPasoFin Then
                        '        diasEstimados = diasEstimados + IIf(paso.EnProrroga = Constantes.Verdadero, paso.NumDiasMax, paso.NumDiasMin)
                        '    End If
                        'Else
                        If paso.IdPaso >= idPasoInicia And paso.IdPaso <= idPasoFin And Not paso.IdPaso.Equals(17) Then
                            diasEstimados = diasEstimados + IIf(paso.EnProrroga = Constantes.Verdadero, paso.NumDiasMax, paso.NumDiasMin)
                            'End If
                        End If
                    End If

                Next
            End If

        Catch ex As Exception

        End Try

        Return diasEstimados

    End Function

    Public Function calcularDiasEstimados_v15(ByVal idPasoInicia As Integer, ByVal idPasoFin As Integer,
                                          Optional ByVal piIdVista As Integer = Constantes.Todos) As Integer
        Dim diasEstimados As Integer = 0

        Try
            Dim lstPasos As New List(Of Paso)

            lstPasos = AccesoBD.getCatalogoPasos(, piIdVista)

            If Not IsNothing(lstPasos) And lstPasos.Count > 0 Then
                For Each paso As Paso In lstPasos
                    If paso.IdPaso >= idPasoInicia And paso.IdPaso <= idPasoFin Then
                  diasEstimados = diasEstimados + IIf(paso.EnProrroga = Constantes.Verdadero, paso.NumDiasMin, paso.NumDiasMax)
                    End If
                Next
            End If

        Catch ex As Exception

        End Try

        Return diasEstimados

    End Function

   Public Function calcularDiasEstimadosTotales(ByVal idPasoInicia As Integer, ByVal idPasoFin As Integer,
                                          Optional ByVal piIdVista As Integer = Constantes.Todos) As Integer
      Dim diasEstimados As Integer = 0

      Try
         Dim lstPasos As New List(Of Paso)

         lstPasos = AccesoBD.getCatalogoPasos(, piIdVista)

         If Not IsNothing(lstPasos) And lstPasos.Count > 0 Then
            For Each paso As Paso In lstPasos
               If paso.IdPaso >= idPasoInicia And paso.IdPaso <= idPasoFin And Not paso.IdPaso.Equals(7) And Not paso.IdPaso.Equals(11) And Not paso.IdPaso.Equals(17) Then
                  diasEstimados = diasEstimados + IIf(paso.EnProrroga = Constantes.Verdadero, paso.NumDiasMax, paso.NumDiasMin)
               End If
            Next
         End If

      Catch ex As Exception

      End Try

      Return diasEstimados

   End Function

    Public Function calcularDiasEstimados_v15_5(ByVal idPasoInicia As Integer, ByVal idPasoFin As Integer,
                                          Optional ByVal piIdVista As Integer = Constantes.Todos) As Integer
        Dim diasEstimados As Integer = 0

        Try
            Dim lstPasos As New List(Of Paso)

            lstPasos = AccesoBD.getCatalogoPasos(, piIdVista)

            If Not IsNothing(lstPasos) And lstPasos.Count > 0 Then
                For Each paso As Paso In lstPasos
                    If paso.IdPaso >= idPasoInicia And paso.IdPaso <= idPasoFin Then
                        diasEstimados = diasEstimados + IIf(paso.EnProrroga = Constantes.Verdadero, paso.NumDiasMax, paso.NumDiasMin)
                    End If
                Next
            End If

        Catch ex As Exception

        End Try

        Return diasEstimados

    End Function

    Public Function calcularDiasEstimados(ByVal idPasoInicia As Integer, ByVal idPasoFin As Integer,
                                          Optional ByVal piIdVista As Integer = Constantes.Todos) As Integer
        Dim diasEstimados As Integer = 0

        Try
            Dim lstPasos As New List(Of Paso)

            lstPasos = AccesoBD.getCatalogoPasos(, piIdVista)

            If Not IsNothing(lstPasos) And lstPasos.Count > 0 Then
                For Each paso As Paso In lstPasos
                    If paso.IdPaso >= idPasoInicia And paso.IdPaso <= idPasoFin Then
                        diasEstimados = diasEstimados + IIf(paso.EnProrroga = Constantes.Verdadero, paso.NumDiasMax, paso.NumDiasMin)
                    End If
                Next
            End If

        Catch ex As Exception

        End Try

        Return diasEstimados

    End Function

   Public Function calcularDiasReales(ByVal idVisita As Integer,
                                      ByVal idPasoInicia As Integer,
                                      ByVal idPasoFin As Integer,
                                     Optional numSec As Integer = 0) As Integer
      Dim diasReales As Integer = 0
      Dim fechaIniciaEtapa As DateTime = DateTime.MinValue
      Dim fechaFinEtapa As DateTime = DateTime.MinValue


      Try
         'Obtiene  todos los paso de la visita
         Dim lstTodosPasosVisita As New List(Of PasoProcesoVisita)
         lstTodosPasosVisita = AccesoBD.getTodosPasosVisita(idVisita)

         'obtiene los pasos de la visita correspondientes a la etapa
         Dim lstPasosVisitaEtapa As New List(Of PasoProcesoVisita)

         For Each pasoVisitaEtapa In lstTodosPasosVisita
            If pasoVisitaEtapa.IdPaso >= idPasoInicia And pasoVisitaEtapa.IdPaso <= idPasoFin Then
               lstPasosVisitaEtapa.Add(pasoVisitaEtapa)
            End If
         Next

         If Not IsNothing(lstPasosVisitaEtapa) And lstPasosVisitaEtapa.Count > 0 Then

            For Each paso As PasoProcesoVisita In lstPasosVisitaEtapa

               If paso.IdPaso = idPasoInicia Then
                  fechaIniciaEtapa = paso.FechaInicio
               End If

               If paso.IdPaso = idPasoFin Or paso.IdPaso = paso.IdPasoCancelo Then
                  fechaFinEtapa = paso.FechaFin
               End If

            Next

            If fechaIniciaEtapa = DateTime.MinValue Then
               'Signfica que no ha iniciado la etapa
               Return 0
            End If

            If fechaFinEtapa = DateTime.MinValue Then

               'Significa que no ha llegado al último paso de la etapa o el útlimo paso de la etapa aun no finaliza,
               'entonces se toma la fecha actual como fecha fin
               fechaFinEtapa = DateTime.Now

            End If

            If fechaIniciaEtapa <> DateTime.MinValue And fechaFinEtapa <> DateTime.MinValue Then

               'Esto se hace para tener solo las fechas, sin las horas, minutos y segundos, 
               'ya que por estos datos puede ser mayor o menor una fecha y las comparaciones entre fechas serían incorrectas
               Dim fechaInicio As DateTime = New DateTime(fechaIniciaEtapa.Year, fechaIniciaEtapa.Month, fechaIniciaEtapa.Day)
               Dim fechaFin As DateTime = New DateTime(fechaFinEtapa.Year, fechaFinEtapa.Month, fechaFinEtapa.Day)

               Dim res1 As Integer = Date.Compare(fechaInicio, fechaFin)
               '0  - es la misma fecha
               '-1 - es mas pequeña la fechaInicio que la fechaFin
               '1  - es mas grande la fechaInicio que la fechaFin

               If res1 = 0 Then
                  Return 1
               Else
                  If numSec = 1 Then
                     diasReales = calcularDiasTranscurridos(fechaIniciaEtapa, Date.Now)
                  Else
                     diasReales = calcularDiasTranscurridos(fechaIniciaEtapa, fechaFinEtapa)
                  End If
               End If
            End If

         End If

      Catch ex As Exception

      End Try

      Return diasReales

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

    Private Sub imgProcesoVisitaAmbos_Click(sender As Object, e As ImageClickEventArgs) Handles imgProcesoVisitaAmbos.Click
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Mostraremos", "AquiMuestroOpcionesImgProcesos();", True)
    End Sub


End Class