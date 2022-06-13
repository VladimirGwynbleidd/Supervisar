Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Collections
Imports System.Collections.Generic
Imports System.Web.Configuration



Public Class PDF

    Public Property EsAutorizada As Boolean = False
    Public Property FechaTimbre As Date
    Public Property Certificado As String
    Public Property Sello As String
    Public Property EsSolicitud As Boolean = False
    Public Property Folio As String
    Public Property FechaSol As String
    Public Property ClaveDoc As String
    Public Property Titulo As String

    Public Sub New()
    End Sub

    Public Function CreateDocument(ByVal template As String, ByVal etiquetas As Dictionary(Of String, String), Optional ByVal horizontal As Boolean = False, Optional ByVal marcaAgua As Boolean = False) As String
        Dim PDFile As String = System.IO.Path.GetTempFileName()
        Dim doc
        If Me.EsSolicitud Then
            If horizontal Then
                doc = New Document(PageSize.A4.Rotate(), 50, 50, 170, 100)
            Else
                doc = New Document(PageSize.A4, 50, 50, 170, 100)
            End If
        Else
            If horizontal Then
                doc = New Document(PageSize.A4.Rotate(), 50, 50, 50, 100)
            Else
                doc = New Document(PageSize.A4, 50, 50, 50, 100)
            End If
        End If

        Dim output As New System.IO.FileStream(PDFile, IO.FileMode.Create)
        Dim writer As PdfWriter = PdfWriter.GetInstance(doc, output)
        'Pie de pagina
        Dim ev As New itsEvents
        ev.EsAutorizada = Me.EsAutorizada
        ev.Certificado = Me.Certificado
        ev.FechaTimbre = Me.FechaTimbre
        ev.Sello = Me.Sello
        ev.Titulo = Me.Titulo
        ev.Folio = Me.Folio
        ev.FechaSol = Me.FechaSol
        ev.ClaveDoc = Me.ClaveDoc
        ev.EsSolicitud = Me.EsSolicitud
        writer.PageEvent = ev
        doc.Open()

        Dim contents As String = System.IO.File.ReadAllText(template)
        Dim pair As KeyValuePair(Of String, String)
        For Each pair In etiquetas
            contents = contents.Replace(pair.Key, pair.Value)
        Next

        Dim parsedHtmlElements As List(Of IElement) = html.simpleparser.HTMLWorker.ParseToList(New System.IO.StringReader(contents), Nothing)
        For Each htmlElement In parsedHtmlElements
            doc.Add(CType(htmlElement, IElement))
        Next

        doc.Close()
        If marcaAgua Then
            Dim PDFileAgua As String = System.IO.Path.GetTempFileName()
            agregarMarcaAguaImagen(PDFile, PDFileAgua, System.Web.Hosting.HostingEnvironment.MapPath("~/Imagenes/fondo_PDF.png"))
            PDFile = PDFileAgua
        End If

        Dim FileInfo As New IO.FileInfo(PDFile)
        FileInfo.MoveTo(IO.Path.ChangeExtension(PDFile, ".pdf"))
        PDFile = IO.Path.ChangeExtension(PDFile, ".pdf")

        Return PDFile

    End Function

    Public Function CreateDocumentSeguimientoReporte(ByVal etiquetas As Dictionary(Of String, String), ByVal lstDatosEtapa As List(Of DatosEtapa), Optional ByVal marcaAgua As Boolean = False) As String

        'Dim PDFile As String = System.IO.Path.GetTempFileName()
        Dim PDFile As String = System.Web.Hosting.HostingEnvironment.MapPath("~/Reportes/" + Guid.NewGuid().ToString() + ".tmp")

        Dim doc As New Document(New Rectangle(288.0F, 144.0F), 50.0F, 50.0F, 50.0F, 135.0F)

        Dim output As New System.IO.FileStream(PDFile, IO.FileMode.Create)
        Dim writer As PdfWriter = PdfWriter.GetInstance(doc, output)

        doc.SetPageSize(iTextSharp.text.PageSize.LETTER)

        Dim encabezado As Image = Image.GetInstance(System.Web.Hosting.HostingEnvironment.MapPath("~/Imagenes/CONSAR_PDF_SMALL.png"))
        encabezado.ScalePercent(50.0F)
        encabezado.Alignment = 5

        'Dim logo As Image = Image.GetInstance(System.Web.Hosting.HostingEnvironment.MapPath("~/Imagenes/logo_consar.png"))
        'logo.ScalePercent(100.0F)


        doc.Open()

        doc.Add(encabezado)
        'logo.SetAbsolutePosition(doc.PageSize.Width - doc.LeftMargin - logo.ScaledWidth, doc.PageSize.Height - doc.TopMargin - logo.ScaledHeight)
        'doc.Add(logo)

        'TABLA ENCABEZADO
        Dim tablaEncabezado As New PdfPTable(etiquetas.Count * 2)
        tablaEncabezado.WidthPercentage = 100.0F
        tablaEncabezado.HorizontalAlignment = Element.ALIGN_CENTER

        'celdas vacías sin color
        Dim cv0 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv0.Border = 0
        cv0.Colspan = etiquetas.Count * 2
        tablaEncabezado.AddCell(cv0)

        'celdas vacías color verde
        Dim cv1 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv1.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cv1.Border = 0
        cv1.Colspan = etiquetas.Count * 2
        tablaEncabezado.AddCell(cv1)

        For Each pair In etiquetas

            Dim cell As New PdfPCell(New Phrase(pair.Key, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
            cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
            cell.Border = 0

            Dim _cell As New PdfPCell(New Phrase(pair.Value, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
            _cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            _cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            _cell.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
            _cell.Border = 0

            tablaEncabezado.AddCell(cell)
            tablaEncabezado.AddCell(_cell)

        Next

        'celdas vacías color verde
        Dim cv2 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv2.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cv2.Border = 0
        cv2.Colspan = etiquetas.Count * 2
        tablaEncabezado.AddCell(cv2)

        'celdas vacías sin color
        Dim cv3 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv3.Border = 0
        cv3.Colspan = etiquetas.Count * 2
        tablaEncabezado.AddCell(cv3)

        Dim cv4 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv4.Border = 0
        cv4.Colspan = etiquetas.Count * 2
        tablaEncabezado.AddCell(cv4)

        doc.Add(tablaEncabezado)

        'TABLA DE ETAPAS
        For Each dEtapa As DatosEtapa In lstDatosEtapa

            Dim tablaEtapa As New PdfPTable(2)
            tablaEtapa.WidthPercentage = 100
            tablaEtapa.HorizontalAlignment = Element.ALIGN_CENTER

            'Se define la anccho de las 2 columnas
            Dim sglTblHdWidths(1) As Single
            sglTblHdWidths(0) = 13
            sglTblHdWidths(1) = 87

            tablaEtapa.SetWidths(sglTblHdWidths)

            'Agrega la descripción de la etapa
            Dim c1 As New PdfPCell(New Phrase(dEtapa.DscEtapa, FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK)))
            c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c1.Border = 0
            c1.Colspan = 2

            tablaEtapa.AddCell(c1)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Agrega el renglón para la barra del tiempo estimado
            Dim c2 As New PdfPCell(New Phrase("Tiempo Estimado", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)))
            c2.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            c2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c2.Border = 0

            tablaEtapa.AddCell(c2)

            'Crea una tabla para pintar la barrra del tiempo estimado
            Dim tablaBarraTE As New PdfPTable(2)
            tablaBarraTE.WidthPercentage = 100
            tablaBarraTE.HorizontalAlignment = Element.ALIGN_CENTER

            'Se define la ancho de las 2 columnas
            Dim sglTblHdWidths_TE(1) As Single

            Dim anchoPorDiaTE As Single = 0
            If dEtapa.DiasTiempoEstimado < 90 And dEtapa.DiasTiempoReal < 90 Then
                anchoPorDiaTE = 90 / 90
            Else
                If dEtapa.DiasTiempoEstimado > dEtapa.DiasTiempoReal Then
                    anchoPorDiaTE = 90 / dEtapa.DiasTiempoEstimado
                Else
                    anchoPorDiaTE = 90 / dEtapa.DiasTiempoReal
                End If
            End If

            sglTblHdWidths_TE(0) = anchoPorDiaTE * dEtapa.DiasTiempoEstimado
            sglTblHdWidths_TE(1) = 100 - (anchoPorDiaTE * dEtapa.DiasTiempoEstimado)

            tablaBarraTE.SetWidths(sglTblHdWidths_TE)


            'celdas para pintar la barra de tiempo estimado
            Dim cTEBarra As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6)))
            cTEBarra.Border = 0
            cTEBarra.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
            tablaBarraTE.AddCell(cTEBarra)

            'celdas para pintar los días de tiempo estimado
            Dim strDiasTE As String = String.Empty
            If dEtapa.DiasTiempoEstimado = 1 Then
                strDiasTE = " día"
            Else
                strDiasTE = " días"
            End If
            Dim cTEDias As New PdfPCell(New Phrase(dEtapa.DiasTiempoEstimado.ToString() + strDiasTE, FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            cTEDias.Border = 0
            tablaBarraTE.AddCell(cTEDias)

            'Agrega al renglón la barra del tiempo estimado
            Dim c3 As New PdfPCell(tablaBarraTE)
            c3.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            c3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c3.Border = 0

            tablaEtapa.AddCell(c3)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Agrega el renglón para la barra del tiempo real
            Dim c4 As New PdfPCell(New Phrase("Tiempo Real", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)))
            c4.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            c4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c4.Border = 0

            tablaEtapa.AddCell(c4)

            'Crea una tabla para pintar la barrra del tiempo real
            Dim tablaBarraTR As New PdfPTable(2)
            tablaBarraTR.WidthPercentage = 100
            tablaBarraTR.HorizontalAlignment = Element.ALIGN_CENTER

            'Se define la ancho de las 2 columnas
            Dim sglTblHdWidths_TR(1) As Single

            Dim anchoPorDiaTR As Single = 0
            If dEtapa.DiasTiempoEstimado < 90 And dEtapa.DiasTiempoReal < 90 Then
                anchoPorDiaTR = 90 / 90
            Else
                If dEtapa.DiasTiempoEstimado > dEtapa.DiasTiempoReal Then
                    anchoPorDiaTR = 90 / dEtapa.DiasTiempoEstimado
                Else
                    anchoPorDiaTR = 90 / dEtapa.DiasTiempoReal
                End If
            End If

            sglTblHdWidths_TR(0) = anchoPorDiaTR * dEtapa.DiasTiempoReal
            sglTblHdWidths_TR(1) = 100 - (anchoPorDiaTR * dEtapa.DiasTiempoReal)

            tablaBarraTR.SetWidths(sglTblHdWidths_TR)

            'celdas para pintar la barra de tiempo real
            Dim cTRBarra As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6)))
            cTRBarra.Border = 0
            cTRBarra.BackgroundColor = New BaseColor(88, 250, 208) '#58FAD0
            tablaBarraTR.AddCell(cTRBarra)

            'celdas para pintar los días de tiempo real
            Dim strDiasTR As String = String.Empty
            If dEtapa.DiasTiempoReal = 1 Then
                strDiasTR = "  día"
            Else
                strDiasTR = " días"
            End If
            Dim cTRDias As New PdfPCell(New Phrase(dEtapa.DiasTiempoReal.ToString() + strDiasTR, FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            cTRDias.Border = 0
            tablaBarraTR.AddCell(cTRDias)

            'Agrega al renglón la barra del tiempo real
            Dim c5 As New PdfPCell(tablaBarraTR)
            c5.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            c5.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c5.Border = 0

            tablaEtapa.AddCell(c5)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim cv6 As New PdfPCell(New Phrase(dEtapa.DscRangoDias, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)))
            cv6.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            cv6.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cv6.Border = 0
            cv6.Colspan = 2
            tablaEtapa.AddCell(cv6)

            'celdas vacías sin color
            Dim cv5 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
            cv5.Border = 0
            cv5.Colspan = 2
            tablaEtapa.AddCell(cv5)

            doc.Add(tablaEtapa)
        Next

        doc.Close()
        If marcaAgua Then
            Dim PDFileAgua As String = System.IO.Path.GetTempFileName()
            agregarMarcaAguaImagen(PDFile, PDFileAgua, System.Web.Hosting.HostingEnvironment.MapPath("~/Imagenes/fondo_PDF.png"))
            PDFile = PDFileAgua
        End If

        Dim FileInfo As New IO.FileInfo(PDFile)
        FileInfo.MoveTo(IO.Path.ChangeExtension(PDFile, ".pdf"))
        PDFile = IO.Path.ChangeExtension(PDFile, ".pdf")

        Return PDFile

    End Function

    Public Function CreateDocumentSeguimientoDetalleOriginal(ByVal lstDatosEncabezado As List(Of String), ByVal lstDatosPaso As List(Of PasoReporte), Optional ByVal marcaAgua As Boolean = False) As String

        'Dim PDFile As String = System.IO.Path.GetTempFileName()
        Dim PDFile As String = System.Web.Hosting.HostingEnvironment.MapPath("~/Reportes/" + Guid.NewGuid().ToString() + ".tmp")

        Dim doc As New Document(New Rectangle(288.0F, 144.0F), 50.0F, 50.0F, 50.0F, 135.0F)

        Dim output As New System.IO.FileStream(PDFile, IO.FileMode.Create)
        Dim writer As PdfWriter = PdfWriter.GetInstance(doc, output)

        doc.SetPageSize(iTextSharp.text.PageSize.LETTER)

        Dim encabezado As Image = Image.GetInstance(System.Web.Hosting.HostingEnvironment.MapPath("~/Imagenes/CONSAR_PDF_SMALL.png"))
        encabezado.ScalePercent(50.0F)
        encabezado.Alignment = 5

        'Dim logo As Image = Image.GetInstance(System.Web.Hosting.HostingEnvironment.MapPath("~/Imagenes/logo_consar.png"))
        'logo.ScalePercent(100.0F)


        doc.Open()

        doc.Add(encabezado)
        'logo.SetAbsolutePosition(doc.PageSize.Width - doc.LeftMargin - logo.ScaledWidth, doc.PageSize.Height - doc.TopMargin - logo.ScaledHeight)
        'doc.Add(logo)

        'TABLA ENCABEZADO
        Dim numColumnas As Integer = 8
        Dim tablaEncabezado As New PdfPTable(numColumnas)
        tablaEncabezado.WidthPercentage = 100.0F
        tablaEncabezado.HorizontalAlignment = Element.ALIGN_CENTER

        'Se define la ancho de las 8 columnas
        Dim sglTblHdWidths_TbEncabezado(7) As Single
        sglTblHdWidths_TbEncabezado(0) = 15
        sglTblHdWidths_TbEncabezado(1) = 15
        sglTblHdWidths_TbEncabezado(2) = 5
        sglTblHdWidths_TbEncabezado(3) = 15
        sglTblHdWidths_TbEncabezado(4) = 5
        sglTblHdWidths_TbEncabezado(5) = 20
        sglTblHdWidths_TbEncabezado(6) = 5
        sglTblHdWidths_TbEncabezado(7) = 20

        tablaEncabezado.SetWidths(sglTblHdWidths_TbEncabezado)

        'celdas vacías sin color
        Dim cv0 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv0.Border = 0
        cv0.Colspan = numColumnas
        tablaEncabezado.AddCell(cv0)

        'celdas vacías color verde
        Dim cv1 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv1.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cv1.Border = 0
        cv1.Colspan = numColumnas
        tablaEncabezado.AddCell(cv1)

        'CELDAS DEL ENCABEZADO
        Dim cEn1 As New PdfPCell(New Phrase("Visita: ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cEn1.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
        cEn1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn1.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn1.Border = 0

        Dim cEn2 As New PdfPCell(New Phrase(lstDatosEncabezado(0), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn2.HorizontalAlignment = PdfPCell.ALIGN_LEFT
        cEn2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn2.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn2.Border = 0

        Dim cEn3 As New PdfPCell(New Phrase("/", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn3.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn3.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn3.Border = 0

        Dim cEn4 As New PdfPCell(New Phrase(lstDatosEncabezado(1), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn4.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn4.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn4.Border = 0


        Dim cEn5 As New PdfPCell(New Phrase("/", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn5.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn5.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn5.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn5.Border = 0

        Dim cEn6 As New PdfPCell(New Phrase(lstDatosEncabezado(2), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn6.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn6.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn6.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn6.Border = 0


        Dim cEn7 As New PdfPCell(New Phrase("/", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn7.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn7.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn7.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn7.Border = 0

        Dim cEn8 As New PdfPCell(New Phrase(lstDatosEncabezado(3), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn8.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn8.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn8.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn8.Border = 0

        tablaEncabezado.AddCell(cEn1)
        tablaEncabezado.AddCell(cEn2)
        tablaEncabezado.AddCell(cEn3)
        tablaEncabezado.AddCell(cEn4)
        tablaEncabezado.AddCell(cEn5)
        tablaEncabezado.AddCell(cEn6)
        tablaEncabezado.AddCell(cEn7)
        tablaEncabezado.AddCell(cEn8)

        'celdas vacías color verde
        Dim cv2 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv2.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cv2.Border = 0
        cv2.Colspan = numColumnas
        tablaEncabezado.AddCell(cv2)

        'celdas vacías sin color
        Dim cv3 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv3.Border = 0
        cv3.Colspan = numColumnas
        tablaEncabezado.AddCell(cv3)

        Dim cv4 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv4.Border = 0
        cv4.Colspan = numColumnas
        tablaEncabezado.AddCell(cv4)

        doc.Add(tablaEncabezado)

        'TABLA DE PASOS
        Dim tablaPasos As New PdfPTable(4)
        tablaPasos.WidthPercentage = 100
        tablaPasos.HorizontalAlignment = Element.ALIGN_CENTER


        'Se define la anccho de las 4 columnas
        Dim sglTblHdWidths_TbPasos(3) As Single
        sglTblHdWidths_TbPasos(0) = 10
        sglTblHdWidths_TbPasos(1) = 40
        sglTblHdWidths_TbPasos(2) = 25
        sglTblHdWidths_TbPasos(3) = 25

        tablaPasos.SetWidths(sglTblHdWidths_TbPasos)

        'CELDAS ENCABEZDO TABLA DE PASOS
        Dim cell1 As New PdfPCell(New Phrase("Paso", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell1.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell1.Border = 1


        tablaPasos.AddCell(cell1)

        Dim cell2 As New PdfPCell(New Phrase("Días efectivos vs Objetivo", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell2.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell2.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell2.Border = 1

        tablaPasos.AddCell(cell2)

        Dim cell3 As New PdfPCell(New Phrase("Archivos", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell3.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell3.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell3.Border = 1

        tablaPasos.AddCell(cell3)


        Dim cell4 As New PdfPCell(New Phrase("Comentarios", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell4.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell4.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell4.Border = 0

        tablaPasos.AddCell(cell4)

        For Each dPasoReporte As PasoReporte In lstDatosPaso

            'Agrega el numero de paso
            Dim c1 As New PdfPCell(New Phrase(dPasoReporte.IdPaso, FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c1.Border = Rectangle.BOX
            c1.BorderColor = New BaseColor(153, 153, 153) '#999

            tablaPasos.AddCell(c1)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Crea una tabla para englobar las 2 tablas de barras 
            Dim tablaDosBarras As New PdfPTable(1)
            tablaDosBarras.WidthPercentage = 100
            tablaDosBarras.HorizontalAlignment = Element.ALIGN_CENTER


            'Crea una tabla para pintar la barra del tiempo estimado 
            Dim tablaBarraTE As New PdfPTable(2)
            tablaBarraTE.WidthPercentage = 100
            tablaBarraTE.HorizontalAlignment = Element.ALIGN_CENTER

            'Crea una tabla para pintar la barra del tiempo real 
            Dim tablaBarraTR As New PdfPTable(2)
            tablaBarraTR.WidthPercentage = 100
            tablaBarraTR.HorizontalAlignment = Element.ALIGN_CENTER

            'Se define la ancho de las 2 columnas
            Dim sglTblHdWidths_TE(1) As Single
            'Se define la ancho de las 2 columnas
            Dim sglTblHdWidths_TR(1) As Single

            Dim anchoPorDia As Single = 0
            If dPasoReporte.NumDiasTiempoEstimado < 90 And dPasoReporte.NumDiasTiempoReal < 90 Then
                anchoPorDia = 90 / 90
            Else
                If dPasoReporte.NumDiasTiempoEstimado > dPasoReporte.NumDiasTiempoReal Then
                    anchoPorDia = 90 / dPasoReporte.NumDiasTiempoEstimado
                Else
                    anchoPorDia = 90 / dPasoReporte.NumDiasTiempoReal
                End If
            End If

            sglTblHdWidths_TE(0) = anchoPorDia * dPasoReporte.NumDiasTiempoEstimado
            sglTblHdWidths_TE(1) = 100 - (anchoPorDia * dPasoReporte.NumDiasTiempoEstimado)

            sglTblHdWidths_TR(0) = anchoPorDia * dPasoReporte.NumDiasTiempoReal
            sglTblHdWidths_TR(1) = 100 - (anchoPorDia * dPasoReporte.NumDiasTiempoReal)

            tablaBarraTE.SetWidths(sglTblHdWidths_TE)
            tablaBarraTR.SetWidths(sglTblHdWidths_TR)

            'celdas para pintar la barra de tiempo estimado
            Dim cTEBarra As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6)))
            cTEBarra.Border = 0
            cTEBarra.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
            tablaBarraTE.AddCell(cTEBarra)

            'celdas para pintar los días de tiempo estimado          
            Dim cTEDias As New PdfPCell(New Phrase(dPasoReporte.NumDiasTiempoEstimado.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            cTEDias.Border = 0
            tablaBarraTE.AddCell(cTEDias)


            'celdas para pintar la barra de tiempo real
            Dim cTRBarra As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6)))
            cTRBarra.Border = 0
            cTRBarra.BackgroundColor = New BaseColor(88, 250, 208) '#58FAD0
            tablaBarraTR.AddCell(cTRBarra)

            'celdas para pintar los días de tiempo real
            Dim cTRDias As New PdfPCell(New Phrase(dPasoReporte.NumDiasTiempoReal.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            cTRDias.Border = 0
            tablaBarraTR.AddCell(cTRDias)

            Dim cTabla1 As New PdfPCell(tablaBarraTE)
            cTabla1.Border = 0

            Dim cTabla2 As New PdfPCell(tablaBarraTR)
            cTabla2.Border = 0

            tablaDosBarras.AddCell(cTabla1)
            tablaDosBarras.AddCell(cTabla2)


            'Agrega el las barras del tiempo estimado y tiempo real
            Dim c2 As New PdfPCell(tablaDosBarras)
            c2.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            c2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c2.Border = Rectangle.BOX
            c2.BorderColor = New BaseColor(153, 153, 153) '#999

            tablaPasos.AddCell(c2)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Crea una tabla para agregar los documentos adjtuntos
            Dim tablaDocumentosAdjuntos As New PdfPTable(1)
            tablaDocumentosAdjuntos.WidthPercentage = 100
            tablaDocumentosAdjuntos.HorizontalAlignment = Element.ALIGN_CENTER

            'Crea una tabla para agregar los comentarios
            Dim tablaComentarios As New PdfPTable(1)
            tablaComentarios.WidthPercentage = 100
            tablaComentarios.HorizontalAlignment = Element.ALIGN_CENTER

            ''DOCUMENTOS
            For Each documento As DatosDocumento In dPasoReporte.LstDocumentos

                'celdas para pintar el documento adjunto
                Dim cDoc As New PdfPCell(New Phrase(documento.DscNombreArchivo, FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                cDoc.Border = 0
                tablaDocumentosAdjuntos.AddCell(cDoc)
            Next

            ''COMENTARIOS
            For Each documento As DatosDocumento In dPasoReporte.LstComentarios
                'celdas para pintar el documento adjunto
                Dim cCom As New PdfPCell(New Phrase(documento.DscComentarios, FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                cCom.Border = 0
                tablaComentarios.AddCell(cCom)
            Next

            '''''''''''''''''''''''''''''''''''''''''
            'Agrega al renglón la tabla de documentos adjuntos
            Dim c3 As New PdfPCell(tablaDocumentosAdjuntos)
            c3.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            c3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c3.Border = Rectangle.BOX
            c3.BorderColor = New BaseColor(153, 153, 153) '#999

            tablaPasos.AddCell(c3)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Agrega al renglón la tabla de comentarios
            Dim c4 As New PdfPCell(tablaComentarios)
            c4.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            c4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c4.Border = Rectangle.BOX
            c4.BorderColor = New BaseColor(153, 153, 153) '#999

            tablaPasos.AddCell(c4)

            'Crea una tabla para pintar la barrra del tiempo real
            'Dim tablabarratr As New PdfPTable(2)
            'tablabarratr.WidthPercentage = 100
            'tablabarratr.HorizontalAlignment = Element.ALIGN_CENTER

            ''se define la ancho de las 2 columnas
            'Dim sgltblhdwidths_tr(1) As Single

            'Dim anchopordiatr As Single = 0
            'If detapa.diastiempoestimado < 90 And detapa.diastiemporeal < 90 Then
            '    anchopordiatr = 90 / 90
            'Else
            '    If detapa.diastiempoestimado > detapa.diastiemporeal Then
            '        anchopordiatr = 90 / detapa.diastiempoestimado
            '    Else
            '        anchopordiatr = 90 / detapa.diastiemporeal
            '    End If
            'End If

            'sgltblhdwidths_tr(0) = anchopordiatr * detapa.diastiemporeal
            'sgltblhdwidths_tr(1) = 100 - (anchopordiatr * detapa.diastiemporeal)

            'tablabarratr.SetWidths(sgltblhdwidths_tr)



            'Agrega al renglón la barra del tiempo real
            'Dim c5 As New PdfPCell(tablaBarraTR)
            'c5.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            'c5.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            'c5.Border = 0

            'tablaEtapa.AddCell(c5)

            ' ''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''celdas vacías sin color
            'Dim cv5 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
            'cv5.Border = 0
            'cv5.Colspan = 2
            'tablaEtapa.AddCell(cv5)


        Next

        doc.Add(tablaPasos)

        doc.Close()
        If marcaAgua Then
            Dim PDFileAgua As String = System.IO.Path.GetTempFileName()
            agregarMarcaAguaImagen(PDFile, PDFileAgua, System.Web.Hosting.HostingEnvironment.MapPath("~/Imagenes/fondo_PDF.png"))
            PDFile = PDFileAgua
        End If

        Dim FileInfo As New IO.FileInfo(PDFile)
        FileInfo.MoveTo(IO.Path.ChangeExtension(PDFile, ".pdf"))
        PDFile = IO.Path.ChangeExtension(PDFile, ".pdf")

        Return PDFile

    End Function


    Public Function CreateDocumentSeguimientoDetalle(ByVal lstDatosEncabezado As List(Of String), ByVal lstDatosPaso As List(Of PasoReporte), Optional ByVal marcaAgua As Boolean = False) As String

        'Dim PDFile As String = System.IO.Path.GetTempFileName()
        Dim PDFile As String = System.Web.Hosting.HostingEnvironment.MapPath("~/Reportes/" + Guid.NewGuid().ToString() + ".tmp")

        Dim doc As New Document(New Rectangle(288.0F, 144.0F), 50.0F, 50.0F, 50.0F, 100.0F)

        Dim output As New System.IO.FileStream(PDFile, IO.FileMode.Create)
        Dim writer As PdfWriter = PdfWriter.GetInstance(doc, output)

        doc.SetPageSize(iTextSharp.text.PageSize.LETTER)

        Dim encabezado As Image = Image.GetInstance(System.Web.Hosting.HostingEnvironment.MapPath("~/Imagenes/CONSAR_PDF_SMALL.png"))
        encabezado.ScalePercent(50.0F)
        encabezado.Alignment = 5

        'Dim logo As Image = Image.GetInstance(System.Web.Hosting.HostingEnvironment.MapPath("~/Imagenes/logo_consar.png"))
        'logo.ScalePercent(100.0F)


        doc.Open()

        doc.Add(encabezado)
        'logo.SetAbsolutePosition(doc.PageSize.Width - doc.LeftMargin - logo.ScaledWidth, doc.PageSize.Height - doc.TopMargin - logo.ScaledHeight)
        'doc.Add(logo)

        'TABLA ENCABEZADO
        Dim numColumnas As Integer = 8
        Dim tablaEncabezado As New PdfPTable(numColumnas)
        tablaEncabezado.WidthPercentage = 100.0F
        tablaEncabezado.HorizontalAlignment = Element.ALIGN_CENTER

        'Se define la ancho de las 8 columnas
        Dim sglTblHdWidths_TbEncabezado(7) As Single
        sglTblHdWidths_TbEncabezado(0) = 13
        sglTblHdWidths_TbEncabezado(1) = 22
        sglTblHdWidths_TbEncabezado(2) = 2
        sglTblHdWidths_TbEncabezado(3) = 12
        sglTblHdWidths_TbEncabezado(4) = 14
        sglTblHdWidths_TbEncabezado(5) = 2
        sglTblHdWidths_TbEncabezado(6) = 11
        sglTblHdWidths_TbEncabezado(7) = 24

        tablaEncabezado.SetWidths(sglTblHdWidths_TbEncabezado)

        'celdas vacías sin color
        Dim cv0 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv0.Border = 0
        cv0.Colspan = numColumnas
        tablaEncabezado.AddCell(cv0)

        'celdas vacías color verde
        Dim cv1 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv1.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cv1.Border = 0
        cv1.Colspan = numColumnas
        tablaEncabezado.AddCell(cv1)

        'CELDAS DEL ENCABEZADO
        Dim cEn1 As New PdfPCell(New Phrase("Folio de Visita: ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cEn1.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
        cEn1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn1.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn1.Border = 0

        Dim cEn2 As New PdfPCell(New Phrase(lstDatosEncabezado(0), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn2.HorizontalAlignment = PdfPCell.ALIGN_LEFT
        cEn2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn2.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn2.Border = 0

        Dim cEn3 As New PdfPCell(New Phrase("/", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cEn3.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn3.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn3.Border = 0

        Dim cEn4 As New PdfPCell(New Phrase("Fecha de Registro: ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cEn4.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
        cEn4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn4.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn4.Border = 0

        Dim cEn7 As New PdfPCell(New Phrase(lstDatosEncabezado(1), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn7.HorizontalAlignment = PdfPCell.ALIGN_LEFT
        cEn7.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn7.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn7.Border = 0

        Dim cEn5 As New PdfPCell(New Phrase("/", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cEn5.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn5.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn5.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn5.Border = 0

        Dim cEn6 As New PdfPCell(New Phrase("Paso Actual: ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cEn6.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
        cEn6.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn6.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn6.Border = 0

        Dim cEn8 As New PdfPCell(New Phrase(lstDatosEncabezado(2), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn8.HorizontalAlignment = PdfPCell.ALIGN_LEFT
        cEn8.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn8.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn8.Border = 0

        tablaEncabezado.AddCell(cEn1)
        tablaEncabezado.AddCell(cEn2)
        tablaEncabezado.AddCell(cEn3)
        tablaEncabezado.AddCell(cEn4)
        tablaEncabezado.AddCell(cEn7)
        tablaEncabezado.AddCell(cEn5)
        tablaEncabezado.AddCell(cEn6)
        tablaEncabezado.AddCell(cEn8)

        'celdas vacías color verde
        Dim cv2 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv2.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cv2.Border = 0
        cv2.Colspan = numColumnas
        tablaEncabezado.AddCell(cv2)

        'celdas vacías sin color
        Dim cv3 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv3.Border = 0
        cv3.Colspan = numColumnas
        tablaEncabezado.AddCell(cv3)

        Dim cv4 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv4.Border = 0
        cv4.Colspan = numColumnas
        tablaEncabezado.AddCell(cv4)

        doc.Add(tablaEncabezado)

        'TABLA DE PASOS
        Dim tablaPasos As New PdfPTable(6)
        tablaPasos.WidthPercentage = 100
        tablaPasos.HorizontalAlignment = Element.ALIGN_CENTER


        'Se define la anccho de las 4 columnas
        Dim sglTblHdWidths_TbPasos(5) As Single
        sglTblHdWidths_TbPasos(0) = 7
        sglTblHdWidths_TbPasos(1) = 30
        sglTblHdWidths_TbPasos(2) = 10
        sglTblHdWidths_TbPasos(3) = 10
        sglTblHdWidths_TbPasos(4) = 22
        sglTblHdWidths_TbPasos(5) = 21

        tablaPasos.SetWidths(sglTblHdWidths_TbPasos)

        'CELDAS ENCABEZDO TABLA DE PASOS
        Dim cell1 As New PdfPCell(New Phrase("Paso", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell1.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell1.Border = 1


        tablaPasos.AddCell(cell1)

        Dim cell2 As New PdfPCell(New Phrase("Días efectivos vs Objetivo", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell2.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell2.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell2.Border = 1

        tablaPasos.AddCell(cell2)

        Dim cellFecha As New PdfPCell(New Phrase("Fecha", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cellFecha.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cellFecha.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cellFecha.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cellFecha.Border = 1

        tablaPasos.AddCell(cellFecha)

        Dim cell3 As New PdfPCell(New Phrase("Usuario", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell3.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell3.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell3.Border = 0

        tablaPasos.AddCell(cell3)

        Dim cell4 As New PdfPCell(New Phrase("Comentarios", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell4.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell4.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell4.Border = 0

        tablaPasos.AddCell(cell4)

        Dim cell5 As New PdfPCell(New Phrase("Archivos", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell5.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell5.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell5.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell5.Border = 1

        tablaPasos.AddCell(cell5)




        For Each dPasoReporte As PasoReporte In lstDatosPaso

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Crea una tabla para englobar las 2 tablas de barras 
            Dim tablaDosBarras As New PdfPTable(1)
            tablaDosBarras.WidthPercentage = 100
            tablaDosBarras.HorizontalAlignment = Element.ALIGN_CENTER


            'Crea una tabla para pintar la barra del tiempo estimado 
            Dim tablaBarraTE As New PdfPTable(2)
            tablaBarraTE.WidthPercentage = 100
            tablaBarraTE.HorizontalAlignment = Element.ALIGN_CENTER

            'Crea una tabla para pintar la barra del tiempo real 
            Dim tablaBarraTR As New PdfPTable(2)
            tablaBarraTR.WidthPercentage = 100
            tablaBarraTR.HorizontalAlignment = Element.ALIGN_CENTER

            'Se define la ancho de las 2 columnas
            Dim sglTblHdWidths_TE(1) As Single
            'Se define la ancho de las 2 columnas
            Dim sglTblHdWidths_TR(1) As Single

            Dim anchoPorDia As Single = 0
            If dPasoReporte.NumDiasTiempoEstimado < 90 And dPasoReporte.NumDiasTiempoReal < 90 Then
                anchoPorDia = 90 / 90
            Else
                If dPasoReporte.NumDiasTiempoEstimado > dPasoReporte.NumDiasTiempoReal Then
                    anchoPorDia = 90 / dPasoReporte.NumDiasTiempoEstimado
                Else
                    anchoPorDia = 90 / dPasoReporte.NumDiasTiempoReal
                End If
            End If

            sglTblHdWidths_TE(0) = anchoPorDia * dPasoReporte.NumDiasTiempoEstimado
            sglTblHdWidths_TE(1) = 100 - (anchoPorDia * dPasoReporte.NumDiasTiempoEstimado)

            sglTblHdWidths_TR(0) = anchoPorDia * dPasoReporte.NumDiasTiempoReal
            sglTblHdWidths_TR(1) = 100 - (anchoPorDia * dPasoReporte.NumDiasTiempoReal)

            tablaBarraTE.SetWidths(sglTblHdWidths_TE)
            tablaBarraTR.SetWidths(sglTblHdWidths_TR)

            'celdas para pintar la barra de tiempo estimado
            Dim cTEBarra As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6)))
            cTEBarra.Border = 0
            cTEBarra.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
            tablaBarraTE.AddCell(cTEBarra)

            'celdas para pintar los días de tiempo estimado          
            Dim cTEDias As New PdfPCell(New Phrase(dPasoReporte.NumDiasTiempoEstimado.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            cTEDias.Border = 0
            tablaBarraTE.AddCell(cTEDias)


            'celdas para pintar la barra de tiempo real
            Dim cTRBarra As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6)))
            cTRBarra.Border = 0
            cTRBarra.BackgroundColor = New BaseColor(88, 250, 208) '#58FAD0
            tablaBarraTR.AddCell(cTRBarra)

            'celdas para pintar los días de tiempo real
            Dim cTRDias As New PdfPCell(New Phrase(dPasoReporte.NumDiasTiempoReal.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            cTRDias.Border = 0
            tablaBarraTR.AddCell(cTRDias)

            Dim cTabla1 As New PdfPCell(tablaBarraTE)
            cTabla1.Border = 0

            Dim cTabla2 As New PdfPCell(tablaBarraTR)
            cTabla2.Border = 0

            tablaDosBarras.AddCell(cTabla1)
            tablaDosBarras.AddCell(cTabla2)

            

            Dim liNumIte As Integer = 0
            Dim liNumIteAux As Integer = 0

            ''OBTENER EL NUEMRO TOTAL DE ITERACIONES
            If dPasoReporte.LstUsuarioComentarios.Count > 0 Then
                liNumIte = dPasoReporte.LstUsuarioComentarios.Count
                For i As Integer = 0 To dPasoReporte.LstUsuarioComentarios.Count - 1
                    Dim objUsuCom As UsuarioComentario = dPasoReporte.LstUsuarioComentarios.Item(i)
                    liNumIteAux = liNumIteAux + objUsuCom.ListaComentarios.Count
                Next

                If liNumIteAux > liNumIte Then
                    liNumIte = liNumIteAux
                End If
            End If

            'Agrega el numero de paso
            Dim c1 As New PdfPCell(New Phrase(dPasoReporte.IdPaso, FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c1.Border = Rectangle.BOX
            c1.BorderColor = New BaseColor(153, 153, 153) '#999
            If dPasoReporte.LstUsuarioComentarios.Count > 0 Then c1.Rowspan = dPasoReporte.LstUsuarioComentarios.Count

            tablaPasos.AddCell(c1)

            'Agrega el las barras del tiempo estimado y tiempo real
            Dim c2 As New PdfPCell(tablaDosBarras)
            c2.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            c2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c2.Border = Rectangle.BOX
            c2.BorderColor = New BaseColor(153, 153, 153) '#999
            If dPasoReporte.LstUsuarioComentarios.Count > 0 Then c2.Rowspan = dPasoReporte.LstUsuarioComentarios.Count

            tablaPasos.AddCell(c2)

            ''USUARIOS Y SUS COMENTARIOS
            If dPasoReporte.LstUsuarioComentarios.Count > 0 Then
                Dim liAux As Integer = 0
                For Each objUsuCom As UsuarioComentario In dPasoReporte.LstUsuarioComentarios

                    Dim cFec = New PdfPCell(New Phrase(objUsuCom.FechaRegistrCom, FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                    cFec.Border = Rectangle.BOX
                    cFec.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                    cFec.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cFec.BorderColor = New BaseColor(153, 153, 153) '#999

                    tablaPasos.AddCell(cFec)

                    'celdas para pintar el documento adjunto
                    Dim cUsu As New PdfPCell(New Phrase(objUsuCom.NombreCompleto, FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                    cUsu.Border = Rectangle.BOX
                    cUsu.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                    cUsu.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cUsu.BorderColor = New BaseColor(153, 153, 153) '#999

                    tablaPasos.AddCell(cUsu)

                    Dim lsComentVisita As String = objUsuCom.ContenidoCom.Trim()
                    Dim estatusSaltos As String

                    If lsComentVisita.Length > 0 Then
                        If lsComentVisita.Contains("<ul>") Then
                            estatusSaltos = vbCrLf + Chr(155) + " "
                            lsComentVisita = lsComentVisita.Replace("<ul><li>", estatusSaltos).
                                                            Replace("</li><li>", estatusSaltos).
                                                            Replace("</li>", "").
                                                            Replace("</ul>", vbCrLf).
                                                            Replace("<br />", vbCrLf)
                        End If
                    End If

                    Dim cCom = New PdfPCell(New Phrase(IIf(objUsuCom.ContenidoCom.Trim().Length > 0, lsComentVisita,
                                                          WebConfigurationManager.AppSettings("msgReporteSinComentario").ToString()), FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                    cCom.Border = Rectangle.BOX
                    cCom.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                    cCom.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cCom.BorderColor = New BaseColor(153, 153, 153) '#999

                    tablaPasos.AddCell(cCom)

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Crea una tabla para agregar los documentos adjtuntos
                    Dim tablaDocumentosAdjuntos As New PdfPTable(1)
                    tablaDocumentosAdjuntos.WidthPercentage = 100
                    tablaDocumentosAdjuntos.HorizontalAlignment = Element.ALIGN_CENTER

                    ''DOCUMENTOS
                    Dim ldFechaArchivo As New DateTime
                    Dim lsUsuarioCargaArchivo As String = ""
                    For Each documento As DatosDocumento In objUsuCom.ListaDocumentos
                        'celdas para pintar el documento adjunto
                        Dim cDoc As New PdfPCell(New Phrase(documento.DscNombreArchivo, FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                        cDoc.Border = 0
                        tablaDocumentosAdjuntos.AddCell(cDoc)
                    Next

                    Dim cDocs = New PdfPCell(tablaDocumentosAdjuntos)
                    cDocs.Border = Rectangle.BOX
                    cDocs.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                    cDocs.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cDocs.BorderColor = New BaseColor(153, 153, 153) '#999

                    tablaPasos.AddCell(cDocs)
                Next
            Else
                Dim cFecha As New PdfPCell(New Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                cFecha.Border = Rectangle.BOX
                cFecha.BorderColor = New BaseColor(153, 153, 153) '#999

                tablaPasos.AddCell(cFecha)

                Dim cUsu As New PdfPCell(New Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                cUsu.Border = Rectangle.BOX
                cUsu.BorderColor = New BaseColor(153, 153, 153) '#999

                tablaPasos.AddCell(cUsu)

                Dim cCom As New PdfPCell(New Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                cCom.Border = Rectangle.BOX
                cCom.BorderColor = New BaseColor(153, 153, 153) '#999

                tablaPasos.AddCell(cCom)

                Dim cDocs As New PdfPCell(New Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                cDocs.Border = Rectangle.BOX
                cDocs.BorderColor = New BaseColor(153, 153, 153) '#999

                tablaPasos.AddCell(cDocs)
            End If
        Next

        Dim cv6 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv6.Border = 0
        cv6.Colspan = 5
        tablaPasos.AddCell(cv6)

        doc.Add(tablaPasos)


        'Agrega la tabla de colores
        Dim tablaContColores As New PdfPTable(1)
        Dim tablaColores As New PdfPTable(6)
        tablaColores.WidthPercentage = 50
        tablaColores.HorizontalAlignment = Element.ALIGN_CENTER
        Dim tamaniosTablaColores(5) As Single
        tamaniosTablaColores(0) = 10
        tamaniosTablaColores(1) = 8
        tamaniosTablaColores(2) = 32
        tamaniosTablaColores(3) = 10
        tamaniosTablaColores(4) = 8
        tamaniosTablaColores(5) = 32
        tablaColores.SetWidths(tamaniosTablaColores)

        'celdas para pintar la barra de tiempo estimado
        Dim cTVacia1 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
        cTVacia1.Border = 0
        tablaColores.AddCell(cTVacia1)

        Dim cTEBarraEst As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6)))
        cTEBarraEst.Border = 0
        cTEBarraEst.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cTEBarraEst.HorizontalAlignment = Element.ALIGN_RIGHT
        tablaColores.AddCell(cTEBarraEst)

        'celdas para pintar los días de tiempo estimado          
        Dim cTEstima As New PdfPCell(New Phrase(" = Tiempo Estimado", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
        cTEstima.Border = 0
        tablaColores.AddCell(cTEstima)

        Dim cTVacia2 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
        cTVacia2.Border = 0
        tablaColores.AddCell(cTVacia2)

        'celdas para pintar la barra de tiempo real
        Dim cTRBarraReal As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6)))
        cTRBarraReal.Border = 0
        cTRBarraReal.BackgroundColor = New BaseColor(88, 250, 208) '#58FAD0
        tablaColores.AddCell(cTRBarraReal)

        'celdas para pintar los días de tiempo real
        Dim cTReal As New PdfPCell(New Phrase(" = Tiempo Real", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
        cTReal.Border = 0
        tablaColores.AddCell(cTReal)

        Dim cv8 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv8.Border = 0
        tablaContColores.AddCell(cv8)

        Dim c7 As New PdfPCell(tablaColores)
        c7.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        c7.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        c7.Border = Rectangle.BOX
        c7.BorderColor = New BaseColor(153, 153, 153) '#999
        c7.Colspan = 3

        tablaContColores.AddCell(c7)

        Dim cv9 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv9.Border = 0
        tablaContColores.AddCell(cv9)

        doc.Add(tablaContColores)

        doc.Close()
        If marcaAgua Then
            Dim PDFileAgua As String = System.IO.Path.GetTempFileName()
            agregarMarcaAguaImagen(PDFile, PDFileAgua, System.Web.Hosting.HostingEnvironment.MapPath("~/Imagenes/fondo_PDF.png"))
            PDFile = PDFileAgua
        End If

        Dim FileInfo As New IO.FileInfo(PDFile)
        FileInfo.MoveTo(IO.Path.ChangeExtension(PDFile, ".pdf"))
        PDFile = IO.Path.ChangeExtension(PDFile, ".pdf")

        Return PDFile

    End Function

    ''' <summary>
    ''' NO MUESTRA AL 100 LAS TABLAS
    ''' </summary>
    ''' <param name="lstDatosEncabezado"></param>
    ''' <param name="lstDatosPaso"></param>
    ''' <param name="marcaAgua"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateDocumentSeguimientoDetalleV2(ByVal lstDatosEncabezado As List(Of String), ByVal lstDatosPaso As List(Of PasoReporte), Optional ByVal marcaAgua As Boolean = False) As String

        'Dim PDFile As String = System.IO.Path.GetTempFileName()
        Dim PDFile As String = System.Web.Hosting.HostingEnvironment.MapPath("~/Reportes/" + Guid.NewGuid().ToString() + ".tmp")

        Dim doc As New Document(New Rectangle(288.0F, 144.0F), 50.0F, 50.0F, 50.0F, 135.0F)

        Dim output As New System.IO.FileStream(PDFile, IO.FileMode.Create)
        Dim writer As PdfWriter = PdfWriter.GetInstance(doc, output)

        doc.SetPageSize(iTextSharp.text.PageSize.LETTER)

        Dim encabezado As Image = Image.GetInstance(System.Web.Hosting.HostingEnvironment.MapPath("~/Imagenes/CONSAR_PDF_SMALL.png"))
        encabezado.ScalePercent(50.0F)
        encabezado.Alignment = 5

        'Dim logo As Image = Image.GetInstance(System.Web.Hosting.HostingEnvironment.MapPath("~/Imagenes/logo_consar.png"))
        'logo.ScalePercent(100.0F)


        doc.Open()

        doc.Add(encabezado)
        'logo.SetAbsolutePosition(doc.PageSize.Width - doc.LeftMargin - logo.ScaledWidth, doc.PageSize.Height - doc.TopMargin - logo.ScaledHeight)
        'doc.Add(logo)

        'TABLA ENCABEZADO
        Dim numColumnas As Integer = 8
        Dim tablaEncabezado As New PdfPTable(numColumnas)
        tablaEncabezado.WidthPercentage = 100.0F
        tablaEncabezado.HorizontalAlignment = Element.ALIGN_CENTER

        'Se define la ancho de las 8 columnas
        Dim sglTblHdWidths_TbEncabezado(7) As Single
        sglTblHdWidths_TbEncabezado(0) = 7
        sglTblHdWidths_TbEncabezado(1) = 23
        sglTblHdWidths_TbEncabezado(2) = 5
        sglTblHdWidths_TbEncabezado(3) = 15
        sglTblHdWidths_TbEncabezado(4) = 5
        sglTblHdWidths_TbEncabezado(5) = 20
        sglTblHdWidths_TbEncabezado(6) = 5
        sglTblHdWidths_TbEncabezado(7) = 20

        tablaEncabezado.SetWidths(sglTblHdWidths_TbEncabezado)

        'celdas vacías sin color
        Dim cv0 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv0.Border = 0
        cv0.Colspan = numColumnas
        tablaEncabezado.AddCell(cv0)

        'celdas vacías color verde
        Dim cv1 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv1.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cv1.Border = 0
        cv1.Colspan = numColumnas
        tablaEncabezado.AddCell(cv1)

        'CELDAS DEL ENCABEZADO
        Dim cEn1 As New PdfPCell(New Phrase("Visita: ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cEn1.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
        cEn1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn1.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn1.Border = 0

        Dim cEn2 As New PdfPCell(New Phrase(lstDatosEncabezado(0), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn2.HorizontalAlignment = PdfPCell.ALIGN_LEFT
        cEn2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn2.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn2.Border = 0

        Dim cEn3 As New PdfPCell(New Phrase("/", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn3.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn3.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn3.Border = 0

        Dim cEn4 As New PdfPCell(New Phrase(lstDatosEncabezado(1), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn4.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn4.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn4.Border = 0


        Dim cEn5 As New PdfPCell(New Phrase("/", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn5.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn5.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn5.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn5.Border = 0

        Dim cEn6 As New PdfPCell(New Phrase(lstDatosEncabezado(2), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn6.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn6.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn6.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn6.Border = 0


        Dim cEn7 As New PdfPCell(New Phrase("/", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn7.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn7.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn7.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn7.Border = 0

        Dim cEn8 As New PdfPCell(New Phrase(lstDatosEncabezado(3), FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE)))
        cEn8.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cEn8.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cEn8.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cEn8.Border = 0

        tablaEncabezado.AddCell(cEn1)
        tablaEncabezado.AddCell(cEn2)
        tablaEncabezado.AddCell(cEn3)
        tablaEncabezado.AddCell(cEn4)
        tablaEncabezado.AddCell(cEn5)
        tablaEncabezado.AddCell(cEn6)
        tablaEncabezado.AddCell(cEn7)
        tablaEncabezado.AddCell(cEn8)

        'celdas vacías color verde
        Dim cv2 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv2.BackgroundColor = New BaseColor(40, 145, 111) '#28916F
        cv2.Border = 0
        cv2.Colspan = numColumnas
        tablaEncabezado.AddCell(cv2)

        'celdas vacías sin color
        Dim cv3 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv3.Border = 0
        cv3.Colspan = numColumnas
        tablaEncabezado.AddCell(cv3)

        Dim cv4 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv4.Border = 0
        cv4.Colspan = numColumnas
        tablaEncabezado.AddCell(cv4)

        doc.Add(tablaEncabezado)

        'TABLA DE PASOS
        Dim tablaPasos As New PdfPTable(5)
        tablaPasos.WidthPercentage = 100
        tablaPasos.HorizontalAlignment = Element.ALIGN_CENTER


        'Se define la anccho de las 4 columnas
        Dim sglTblHdWidths_TbPasos(4) As Single
        sglTblHdWidths_TbPasos(0) = 10
        sglTblHdWidths_TbPasos(1) = 35
        sglTblHdWidths_TbPasos(2) = 15
        sglTblHdWidths_TbPasos(3) = 20
        sglTblHdWidths_TbPasos(4) = 20

        tablaPasos.SetWidths(sglTblHdWidths_TbPasos)

        'CELDAS ENCABEZDO TABLA DE PASOS
        Dim cell1 As New PdfPCell(New Phrase("Paso", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell1.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell1.Border = 1


        tablaPasos.AddCell(cell1)

        Dim cell2 As New PdfPCell(New Phrase("Días efectivos vs Objetivo", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell2.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell2.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell2.Border = 1

        tablaPasos.AddCell(cell2)

        Dim cell3 As New PdfPCell(New Phrase("Usuario", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell3.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell3.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell3.Border = 0

        tablaPasos.AddCell(cell3)

        Dim cell4 As New PdfPCell(New Phrase("Comentarios", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell4.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell4.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell4.Border = 0

        tablaPasos.AddCell(cell4)

        Dim cell5 As New PdfPCell(New Phrase("Archivos", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cell5.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        cell5.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell5.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cell5.Border = 1

        tablaPasos.AddCell(cell5)




        For Each dPasoReporte As PasoReporte In lstDatosPaso

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Crea una tabla para englobar las 2 tablas de barras 
            Dim tablaDosBarras As New PdfPTable(1)
            tablaDosBarras.WidthPercentage = 100
            tablaDosBarras.HorizontalAlignment = Element.ALIGN_CENTER


            'Crea una tabla para pintar la barra del tiempo estimado 
            Dim tablaBarraTE As New PdfPTable(2)
            tablaBarraTE.WidthPercentage = 100
            tablaBarraTE.HorizontalAlignment = Element.ALIGN_CENTER

            'Crea una tabla para pintar la barra del tiempo real 
            Dim tablaBarraTR As New PdfPTable(2)
            tablaBarraTR.WidthPercentage = 100
            tablaBarraTR.HorizontalAlignment = Element.ALIGN_CENTER

            'Se define la ancho de las 2 columnas
            Dim sglTblHdWidths_TE(1) As Single
            'Se define la ancho de las 2 columnas
            Dim sglTblHdWidths_TR(1) As Single

            Dim anchoPorDia As Single = 0
            If dPasoReporte.NumDiasTiempoEstimado < 90 And dPasoReporte.NumDiasTiempoReal < 90 Then
                anchoPorDia = 90 / 90
            Else
                If dPasoReporte.NumDiasTiempoEstimado > dPasoReporte.NumDiasTiempoReal Then
                    anchoPorDia = 90 / dPasoReporte.NumDiasTiempoEstimado
                Else
                    anchoPorDia = 90 / dPasoReporte.NumDiasTiempoReal
                End If
            End If

            sglTblHdWidths_TE(0) = anchoPorDia * dPasoReporte.NumDiasTiempoEstimado
            sglTblHdWidths_TE(1) = 100 - (anchoPorDia * dPasoReporte.NumDiasTiempoEstimado)

            sglTblHdWidths_TR(0) = anchoPorDia * dPasoReporte.NumDiasTiempoReal
            sglTblHdWidths_TR(1) = 100 - (anchoPorDia * dPasoReporte.NumDiasTiempoReal)

            tablaBarraTE.SetWidths(sglTblHdWidths_TE)
            tablaBarraTR.SetWidths(sglTblHdWidths_TR)

            'celdas para pintar la barra de tiempo estimado
            Dim cTEBarra As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6)))
            cTEBarra.Border = 0
            cTEBarra.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
            tablaBarraTE.AddCell(cTEBarra)

            'celdas para pintar los días de tiempo estimado          
            Dim cTEDias As New PdfPCell(New Phrase(dPasoReporte.NumDiasTiempoEstimado.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            cTEDias.Border = 0
            tablaBarraTE.AddCell(cTEDias)


            'celdas para pintar la barra de tiempo real
            Dim cTRBarra As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6)))
            cTRBarra.Border = 0
            cTRBarra.BackgroundColor = New BaseColor(88, 250, 208) '#58FAD0
            tablaBarraTR.AddCell(cTRBarra)

            'celdas para pintar los días de tiempo real
            Dim cTRDias As New PdfPCell(New Phrase(dPasoReporte.NumDiasTiempoReal.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            cTRDias.Border = 0
            tablaBarraTR.AddCell(cTRDias)

            Dim cTabla1 As New PdfPCell(tablaBarraTE)
            cTabla1.Border = 0

            Dim cTabla2 As New PdfPCell(tablaBarraTR)
            cTabla2.Border = 0

            tablaDosBarras.AddCell(cTabla1)
            tablaDosBarras.AddCell(cTabla2)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Crea una tabla para agregar los documentos adjtuntos
            Dim tablaDocumentosAdjuntos As New PdfPTable(1)
            tablaDocumentosAdjuntos.WidthPercentage = 100
            tablaDocumentosAdjuntos.HorizontalAlignment = Element.ALIGN_CENTER

            'Crea una tabla para agregar los comentarios
            Dim tablaComentarios As New PdfPTable(2)
            tablaComentarios.WidthPercentage = 100
            tablaComentarios.HorizontalAlignment = Element.ALIGN_CENTER

            ''DOCUMENTOS
            For Each documento As DatosDocumento In dPasoReporte.LstDocumentos
                'celdas para pintar el documento adjunto
                Dim cDoc As New PdfPCell(New Phrase(documento.DscNombreArchivo, FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                cDoc.Border = 0
                tablaDocumentosAdjuntos.AddCell(cDoc)
            Next

            ''USUARIOS Y SUS COMENTARIOS
            For Each objUsuCom As UsuarioComentario In dPasoReporte.LstUsuarioComentarios
                'celdas para pintar el documento adjunto
                Dim cUsu As New PdfPCell(New Phrase(objUsuCom.NombreCompleto, FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                cUsu.Border = Rectangle.BOX
                cUsu.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                cUsu.HorizontalAlignment = Element.ALIGN_CENTER
                cUsu.BorderColor = New BaseColor(153, 153, 153) '#999

                tablaComentarios.AddCell(cUsu)

                If objUsuCom.ListaComentarios.Count > 0 Then
                    Dim lstComentarios As New iTextSharp.text.List

                    For Each lsComentario As Comentario In objUsuCom.ListaComentarios
                        lstComentarios.Add(New iTextSharp.text.ListItem(lsComentario.Contenido, FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                    Next

                    Dim cCom As New PdfPCell(New Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                    cCom.Border = Rectangle.BOX
                    cCom.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cCom.HorizontalAlignment = Element.ALIGN_CENTER
                    cCom.BorderColor = New BaseColor(153, 153, 153) '#999

                    cCom.AddElement(lstComentarios)
                    tablaComentarios.AddCell(cCom)
                Else
                    Dim cCom As New PdfPCell(New Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                    cCom.Border = Rectangle.BOX
                    cCom.BorderColor = New BaseColor(153, 153, 153) '#999

                    tablaComentarios.AddCell(cCom)
                End If
            Next

            'Agrega el numero de paso
            Dim c1 As New PdfPCell(New Phrase(dPasoReporte.IdPaso, FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            c1.HorizontalAlignment = PdfPCell.ALIGN_CENTER
            c1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c1.Border = Rectangle.BOX
            c1.BorderColor = New BaseColor(153, 153, 153) '#999

            tablaPasos.AddCell(c1)

            'Agrega el las barras del tiempo estimado y tiempo real
            Dim c2 As New PdfPCell(tablaDosBarras)
            c2.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            c2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c2.Border = Rectangle.BOX
            c2.BorderColor = New BaseColor(153, 153, 153) '#999

            tablaPasos.AddCell(c2)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Agrega a la celda la tabla de usuario
            'Dim c3 As New PdfPCell(tablaUsuarios)
            'c3.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            'c3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            'c3.Border = Rectangle.BOX
            'c3.BorderColor = New BaseColor(153, 153, 153) '#999
            'c3.Colspan = 2

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Agrega a la celda la tabla de comentarios
            Dim c4 As New PdfPCell(tablaComentarios)
            c4.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            c4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c4.Border = Rectangle.BOX
            c4.BorderColor = New BaseColor(153, 153, 153) '#999
            c4.Colspan = 2


            '''''''''''''''''''''''''''''''''''''''''
            'Agrega a la celda la tabla de documentos adjuntos
            Dim c5 As New PdfPCell(tablaDocumentosAdjuntos)
            c5.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            c5.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            c5.Border = Rectangle.BOX
            c5.BorderColor = New BaseColor(153, 153, 153) '#999

            ''AGREGAR LAS COLUMNAS
            tablaPasos.AddCell(c4)
            tablaPasos.AddCell(c5)
        Next

        Dim cv6 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv6.Border = 0
        cv6.Colspan = 5
        tablaPasos.AddCell(cv6)

        doc.Add(tablaPasos)


        'Agrega la tabla de colores
        Dim tablaContColores As New PdfPTable(1)
        Dim tablaColores As New PdfPTable(6)
        tablaColores.WidthPercentage = 50
        tablaColores.HorizontalAlignment = Element.ALIGN_CENTER
        Dim tamaniosTablaColores(5) As Single
        tamaniosTablaColores(0) = 10
        tamaniosTablaColores(1) = 8
        tamaniosTablaColores(2) = 32
        tamaniosTablaColores(3) = 10
        tamaniosTablaColores(4) = 8
        tamaniosTablaColores(5) = 32
        tablaColores.SetWidths(tamaniosTablaColores)

        'celdas para pintar la barra de tiempo estimado
        Dim cTVacia1 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
        cTVacia1.Border = 0
        tablaColores.AddCell(cTVacia1)

        Dim cTEBarraEst As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6)))
        cTEBarraEst.Border = 0
        cTEBarraEst.BackgroundColor = New BaseColor(11, 11, 97) '#0B0B61
        cTEBarraEst.HorizontalAlignment = Element.ALIGN_RIGHT
        tablaColores.AddCell(cTEBarraEst)

        'celdas para pintar los días de tiempo estimado          
        Dim cTEstima As New PdfPCell(New Phrase(" = Tiempo Estimado", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
        cTEstima.Border = 0
        tablaColores.AddCell(cTEstima)

        Dim cTVacia2 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
        cTVacia2.Border = 0
        tablaColores.AddCell(cTVacia2)

        'celdas para pintar la barra de tiempo real
        Dim cTRBarraReal As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 6)))
        cTRBarraReal.Border = 0
        cTRBarraReal.BackgroundColor = New BaseColor(88, 250, 208) '#58FAD0
        tablaColores.AddCell(cTRBarraReal)

        'celdas para pintar los días de tiempo real
        Dim cTReal As New PdfPCell(New Phrase(" = Tiempo Real", FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
        cTReal.Border = 0
        tablaColores.AddCell(cTReal)

        Dim cv8 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv8.Border = 0
        tablaContColores.AddCell(cv8)

        Dim c7 As New PdfPCell(tablaColores)
        c7.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        c7.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        c7.Border = Rectangle.BOX
        c7.BorderColor = New BaseColor(153, 153, 153) '#999
        c7.Colspan = 3

        tablaContColores.AddCell(c7)

        Dim cv9 As New PdfPCell(New Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.WHITE)))
        cv9.Border = 0
        tablaContColores.AddCell(cv9)

        doc.Add(tablaContColores)

        doc.Close()
        If marcaAgua Then
            Dim PDFileAgua As String = System.IO.Path.GetTempFileName()
            agregarMarcaAguaImagen(PDFile, PDFileAgua, System.Web.Hosting.HostingEnvironment.MapPath("~/Imagenes/fondo_PDF.png"))
            PDFile = PDFileAgua
        End If

        Dim FileInfo As New IO.FileInfo(PDFile)
        FileInfo.MoveTo(IO.Path.ChangeExtension(PDFile, ".pdf"))
        PDFile = IO.Path.ChangeExtension(PDFile, ".pdf")

        Return PDFile

    End Function

    'Ejemplo obtenido de internet http://jpsprogramacion.es/?p=286
    Private Sub agregarMarcaAguaImagen(ByVal strRutaFicheroOriginal As String, ByVal strRutaFicheroDestino As String, ByVal strRutaImagen As String)
        Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
        Dim stamper As iTextSharp.text.pdf.PdfStamper = Nothing
        Dim img As iTextSharp.text.Image = Nothing
        Dim underContent As iTextSharp.text.pdf.PdfContentByte = Nothing
        Dim rect As iTextSharp.text.Rectangle = Nothing
        Dim X, Y As Single
        Dim numeroDePaginas As Integer = 0

        Try
            reader = New iTextSharp.text.pdf.PdfReader(strRutaFicheroOriginal)
            rect = reader.GetPageSizeWithRotation(1)
            stamper = New iTextSharp.text.pdf.PdfStamper(reader, New System.IO.FileStream(strRutaFicheroDestino, IO.FileMode.Create))
            img = iTextSharp.text.Image.GetInstance(strRutaImagen)
            'Centar imagen.....................................................
            'Si el alto o el ancho de la imagen es mayor que la superficie del documento,
            'se redimensiona : img.ScaleToFit(rect.Width, rect.Height)
            X = (rect.Width - img.Width) / 2
            Y = (rect.Height - img.Height) / 2
            Y = Y - 50
            img.SetAbsolutePosition(X, Y)
            'Obtenermos el número de páginas. Para cada una de ellas insertamos la marca de agua.
            numeroDePaginas = reader.NumberOfPages()
            For i As Integer = 1 To numeroDePaginas
                underContent = stamper.GetUnderContent(i)
                underContent.AddImage(img)
            Next

        Catch ex As Exception
            Throw ex

        Finally

            stamper.Close()
            reader.Close()
            underContent = Nothing
            stamper = Nothing
            reader = Nothing

        End Try
    End Sub



End Class

Public Class itsEvents
    Inherits PdfPageEventHelper

    Public Property EsAutorizada As Boolean = False
    Public Property FechaTimbre As Date
    Public Property Certificado As String
    Public Property Sello As String
    Public Property EsSolicitud As Boolean = False
    Public Property Folio As String
    Public Property FechaSol As String
    Public Property ClaveDoc As String
    Public Property Titulo As String

    Dim tpl As PdfTemplate
    Dim tplhead As PdfTemplate
    Dim cb As PdfContentByte
    Dim bf As BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
    Dim bfBold As BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)


    Public Overrides Sub OnOpenDocument(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)
        MyBase.OnOpenDocument(writer, document)
        cb = writer.DirectContent
        tpl = cb.CreateTemplate(550, 95)
        tplhead = cb.CreateTemplate(550, 200)
    End Sub

    Public Overrides Sub OnEndPage(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)
        MyBase.OnStartPage(writer, document)

        Dim RutaPie As New RutaImagen
        Dim ImgPie As Image = Image.GetInstance(RutaPie.ObtenRuta())
        ImgPie.ScalePercent(60.0F)
        ImgPie.SetAbsolutePosition(0, 0)
        Dim area As Rectangle = document.PageSize
        FontFactory.RegisterDirectories()
        Dim fuenteNegrita As New Font(FontFactory.GetFont("Adobe Caslon Pro", 7, Font.BOLD))
        Dim fuente As New Font(FontFactory.GetFont("Adobe Caslon Pro", 7, Font.NORMAL))

        Dim fuenteArialS As New Font(FontFactory.GetFont("Arial", 8, Font.BOLD))
        Dim fuenteArialSR As New Font(FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.RED))

        '<<-------- SECCION QUE SOLO SE MUESTRA CUANDO LA SOLICITUD YA FUE AUTORIZADA----------
        If Me.EsAutorizada Then

            Dim tableP As New PdfPTable(1)
            Dim table As New PdfPTable(2)
            table.TotalWidth = 400

            Dim widths() As Integer = {80, 320}
            table.SetWidths(widths)

            Dim seccion1 As New PdfPCell(New Paragraph("Fecha Timbre:", fuenteNegrita))
            seccion1.VerticalAlignment = PdfPCell.ALIGN_BOTTOM
            seccion1.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            seccion1.Border = 0

            Dim seccion1_N As New PdfPCell(New Paragraph(Me.FechaTimbre.ToString, fuente))
            seccion1_N.VerticalAlignment = PdfPCell.ALIGN_BOTTOM
            seccion1_N.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            seccion1_N.Border = 0

            Dim seccion2 As New PdfPCell(New Paragraph("No. de Certificado SAT:", fuenteNegrita))
            seccion2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            seccion2.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            seccion2.Border = 0

            Dim seccion2_N As New PdfPCell(New Paragraph(Me.Certificado, fuente))
            seccion2_N.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            seccion2_N.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            seccion2_N.Border = 0

            Dim seccion3 As New PdfPCell(New Paragraph("Sello Digital SISTIC:", fuenteNegrita))
            seccion3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            seccion3.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            seccion3.Border = 0

            Dim seccion3_N As New PdfPCell(New Paragraph(Me.Sello, fuente))
            seccion3_N.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            seccion3_N.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            seccion3_N.Border = 0

            Dim seccion4 As New PdfPCell(New Paragraph("     ", fuenteNegrita))
            seccion4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            seccion4.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            seccion4.Border = 0

            Dim seccion4_N As New PdfPCell(New Paragraph("       ", fuente))
            seccion4_N.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            seccion4_N.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            seccion4_N.Border = 0

            table.AddCell(seccion1)
            table.AddCell(seccion1_N)
            table.AddCell(seccion2)
            table.AddCell(seccion2_N)
            table.AddCell(seccion3)
            table.AddCell(seccion3_N)
            table.AddCell(seccion4)
            table.AddCell(seccion4_N)

            Dim cuadro As New PdfPCell(table)
            cuadro.BorderWidth = 1
            tableP.AddCell(cuadro)
            tableP.TotalWidth = area.Width - document.Left - document.Left
            tableP.WriteSelectedRows(0, -1, document.Left, document.Bottom, writer.DirectContent)
        End If
        '-------- SECCION QUE SOLO SE MUESTRA CUANDO LA SOLICITUD YA FUE AUTORIZADA ---------->>

        If EsSolicitud Then
            '---------------------ENCABEZADO---------------------------------------
            Dim RutaHead As New RutaImagen
            Dim ImgHead As Image = Image.GetInstance(RutaPie.ObtenRutaLogoSHCP())
            Dim ImgHead2 As Image = Image.GetInstance(RutaPie.ObtenRutaLogoCONSAR())
            Dim titulo As String = Me.Titulo
            ImgHead.ScalePercent(80.0F)
            ImgHead.SetAbsolutePosition(0, 100)
            ImgHead2.ScalePercent(80.0F)
            ImgHead2.SetAbsolutePosition(400, 100)
            tplhead.AddImage(ImgHead)
            tplhead.AddImage(ImgHead2)
            cb.BeginText()
            cb.SetFontAndSize(bf, 18)
            cb.SetTextMatrix(90, 720)
            cb.ShowText(titulo)
            cb.EndText()
            cb.AddTemplate(tplhead, 60.0, 650.0)

            Dim tableHead As New PdfPTable(3)
            Dim fechaSol As New PdfPCell(New Paragraph("Fecha de Solicitud", fuenteArialS))
            fechaSol.VerticalAlignment = PdfPCell.ALIGN_BOTTOM
            fechaSol.HorizontalAlignment = PdfPCell.ALIGN_CENTER
            fechaSol.Border = 0

            Dim claveDoc As New PdfPCell(New Paragraph(Me.ClaveDoc, fuenteArialS))
            claveDoc.VerticalAlignment = PdfPCell.ALIGN_BOTTOM
            claveDoc.HorizontalAlignment = PdfPCell.ALIGN_CENTER
            claveDoc.Border = 0

            Dim noSol As New PdfPCell(New Paragraph("                  " & Me.Folio, fuenteArialSR))
            noSol.VerticalAlignment = PdfPCell.ALIGN_BOTTOM
            noSol.HorizontalAlignment = PdfPCell.ALIGN_CENTER
            noSol.Border = 0

            Dim fechaSolDato As New PdfPCell(New Paragraph(Me.FechaSol, fuenteArialS))
            fechaSolDato.VerticalAlignment = PdfPCell.ALIGN_BOTTOM
            fechaSolDato.HorizontalAlignment = PdfPCell.ALIGN_CENTER
            fechaSolDato.Border = 0

            Dim espacio As New PdfPCell(New Paragraph("       ", fuente))
            espacio.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            espacio.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            espacio.Border = 0

            Dim leyenda As String = "No. de Solicitud:"
            cb.BeginText()
            cb.SetFontAndSize(bfBold, 8)
            cb.SetTextMatrix(400, 700)
            cb.ShowText(leyenda)
            cb.EndText()

            tableHead.AddCell(fechaSol)
            tableHead.AddCell(claveDoc)
            tableHead.AddCell(noSol)
            tableHead.AddCell(fechaSolDato)
            tableHead.AddCell(espacio)
            tableHead.AddCell(espacio)
            tableHead.TotalWidth = area.Width - document.Left - document.Left
            tableHead.WriteSelectedRows(0, -1, 50, 710, writer.DirectContent)
        End If

        '---------------------PIE DE PAGINA------------------------------------
        Dim paginas As String = "Página " & writer.PageNumber.ToString & " de "
        tpl.AddImage(ImgPie)
        cb.BeginText()
        cb.SetFontAndSize(bf, 8)
        cb.SetTextMatrix(52, 52)
        cb.ShowText(paginas)
        cb.EndText()
        cb.AddTemplate(tpl, 60.0, 5.0)
    End Sub

    Public Overrides Sub OnCloseDocument(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)
        MyBase.OnCloseDocument(writer, document)
        tpl.BeginText()
        tpl.SetFontAndSize(bf, 8)
        tpl.SetTextMatrix(38, 47)
        tpl.ShowText("" & (writer.PageNumber - 1).ToString)
        tpl.EndText()
    End Sub

End Class

Public Class RutaImagen
    Inherits System.Web.UI.Page
    Public Function ObtenRuta() As String
        Dim Ruta As String = Server.MapPath("~/Imagenes/direcc_arbol.png")
        Return Ruta
    End Function
    Public Function ObtenRutaLogoSHCP() As String
        Dim Ruta As String = Server.MapPath("~/Imagenes/Encabezado.png")
        Return Ruta
    End Function
    Public Function ObtenRutaLogoCONSAR() As String
        Dim Ruta As String = Server.MapPath("~/Imagenes/logo_consar.png")
        Return Ruta
    End Function
End Class
