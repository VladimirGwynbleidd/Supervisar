using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace ADExtender
{
    public class PDF_SEDI
    {
        public static String sRutaImgen;
        public String RutaImagen
        {
            get { return sRutaImgen; }
            set { sRutaImgen = value; }
        }
        public static DateTime dFechaTimbre;
        public DateTime FechaTimbre
        {
            get { return dFechaTimbre; }
            set { dFechaTimbre = value; }
        }
        public static String sSello;
        public String Sello
        {
            get { return sSello; }
            set { sSello = value; }
        }
        public static String sNombreDocumentoPDF;
        public String NombreDocumentoPDF
        {
            get { return sNombreDocumentoPDF; }
            set { sNombreDocumentoPDF = value; }
        }
        public static String sNombreDocumentoOriginal;
        public String NombreDocumentoOriginal
        {
            get { return sNombreDocumentoOriginal; }
            set { sNombreDocumentoOriginal = value; }
        }
        public static Int32 nNumeroPaginas;
        public Int32 NumeroPaginas
        {
            get { return nNumeroPaginas; }
            set { nNumeroPaginas = value; }
        }

        public static String sNombreUsuarioDocumento;
        public String NombreUsuarioDocumento
        {
            get { return sNombreUsuarioDocumento; }
            set { sNombreUsuarioDocumento = value; }
        }
        //El certificado no se considera en esta etapa
        //public String sCertificado; 
        //public String Certificado{get { return sCertificado; } set { sCertificado = value; } }
        public static void GetNumeroPaginas(String pathIn)
        {
            PdfReader reader = new PdfReader(pathIn);
            nNumeroPaginas = reader.NumberOfPages;
            reader.Close();
        }
        public static Boolean GenerarPDF(String pathIn, String pathout)
        {
            string oldFile = pathIn;
            string newFile = pathout;
            Boolean resultado = false;
            // open the reader
            PdfReader reader = new PdfReader(oldFile);
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);
            try
            {
                // open the writer                
                using (FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, fs);
                    _events e = new _events();
                    e.sSello = sSello;
                    e.sNombreDocumentoPDF = sNombreDocumentoOriginal;
                    e.sNombreDocumentoPDF = sNombreDocumentoPDF;
                    e.dFechaTimbre = dFechaTimbre;
                    e.nPaginas = nNumeroPaginas;
                    e.sRutaImgen = sRutaImgen;
                    e.sNombreUsuarioDocumento = sNombreUsuarioDocumento;
                    writer.PageEvent = e;
                    document.Open();
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        document.NewPage();
                        // the pdf content
                        PdfContentByte cb = writer.DirectContent;
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        cb.AddTemplate(page, 0, 0);
                    }
                    // close the streams and voilá the file should be changed :)
                    document.Close();
                    writer.Close();
                }
                reader.Close();
                resultado = true;
            }
            catch (Exception ex)
            {
                EventLog.CreateEventSource(ex.ToString(), ex.Message);
                resultado = false;
            }
            return resultado;
        }
    }

    class _events : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // cell height 
            float cellHeight = document.TopMargin;
            // PDF document size      
            Rectangle page = document.PageSize;
            // create two column table
            PdfPTable head = new PdfPTable(2);
            //head.WidthPercentage = 80;
            head.TotalWidth = 400;
            head.LockedWidth = true;
            Int32[] widthsHead = { 320, 80 };
            head.SetWidths(widthsHead);
            head.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell c;
            // add the header text
            c = new PdfPCell(new Phrase(
              "[" + Sello + "]", new Font(FontFactory.GetFont("Adobe Caslon Pro", 7, Font.BOLD))));
            c.Border = PdfPCell.BOTTOM_BORDER;
            c.VerticalAlignment = Element.ALIGN_BOTTOM;
            c.FixedHeight = cellHeight;
            head.AddCell(c);

            // add Fecha Corta
            c = new PdfPCell(new Phrase(
            GetMes(DateTime.Now.Month) + ", " + DateTime.Now.Year, new Font(FontFactory.GetFont("Adobe Caslon Pro", 12, Font.BOLD, BaseColor.WHITE))));
            c.Border = PdfPCell.BOTTOM_BORDER;
            c.VerticalAlignment = Element.ALIGN_BOTTOM;
            c.FixedHeight = cellHeight;
            c.BackgroundColor = BaseColor.RED;
            c.HorizontalAlignment = Element.ALIGN_CENTER;
            c.VerticalAlignment = Element.ALIGN_MIDDLE;
            head.AddCell(c);

            PdfPCell cuadroAux = new PdfPCell(head);

            PdfPTable tablePAux = new PdfPTable(1);
            cuadroAux.BorderWidth = 0;
            tablePAux.AddCell(cuadroAux);
            tablePAux.TotalWidth = page.Width - document.Left - document.Left;
            tablePAux.WriteSelectedRows(0, -1, document.Left, document.Top + 10, writer.DirectContent);

            FontFactory.RegisterDirectories();
            Font fuenteNegrita = new Font(FontFactory.GetFont("Adobe Caslon Pro", 7, Font.BOLD));
            Font fuente = new Font(FontFactory.GetFont("Adobe Caslon Pro", 7, Font.NORMAL));
            Font fuenteFooter = new Font(FontFactory.GetFont("Adobe Caslon Pro", 6, Font.NORMAL));
            Font fuenteArialS = new Font(FontFactory.GetFont("Arial", 8, Font.BOLD));
            Font fuenteArialSR = new Font(FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.RED));

            PdfPTable tableP = new PdfPTable(1);
            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = 400;

            Int32[] widths = { 105, 110, 105 };
            table.SetWidths(widths);

            /*Aquí va el logotipo*/
            Image ImgHead2 = Image.GetInstance(RutaImagen);
            ImgHead2.ScaleAbsolute(40, 30);
            PdfPCell SeccionLogotipo = new PdfPCell(ImgHead2, true);
            SeccionLogotipo.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            SeccionLogotipo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            SeccionLogotipo.FixedHeight = cellHeight;
            SeccionLogotipo.Border = PdfPCell.NO_BORDER;

            /*Esto va en una tabla dentro de una celda*/
            PdfPTable tableTextoCentro = new PdfPTable(2);
            tableTextoCentro.TotalWidth = 110;
            Int32[] widthsCentro = { 55, 55 };
            tableTextoCentro.SetWidths(widthsCentro);

            //Tabla de dos columnas
            //Columna 1,1
            PdfPCell seccionDGPO = new PdfPCell(new Paragraph("Firmado digitalmente por Dirección General de Planeación Operativa – DGPO", fuente));
            seccionDGPO.VerticalAlignment = PdfPCell.ALIGN_TOP;
            seccionDGPO.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            seccionDGPO.Border = PdfPCell.NO_BORDER;
            seccionDGPO.Colspan = 2;

            //Columna 2,1
            String sFecha = FechaTimbre.Year.ToString("0000") + ":" +
                            FechaTimbre.Month.ToString("00") + ":" +
                            FechaTimbre.Day.ToString("00") + " " +
                            FechaTimbre.Hour.ToString("00") + ":" +
                            FechaTimbre.Minute.ToString("00") + ":" +
                            FechaTimbre.Second.ToString("00");

            PdfPCell seccionFechaTimbre = new PdfPCell(new Paragraph("Fecha: " + sFecha, fuente));
            seccionFechaTimbre.VerticalAlignment = PdfPCell.ALIGN_TOP;
            seccionFechaTimbre.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            seccionFechaTimbre.Border = PdfPCell.NO_BORDER;
            seccionFechaTimbre.Colspan = 2;

            PdfPCell cadenaFechaTimbre = new PdfPCell(new Paragraph(FechaTimbre.ToString(), fuente));
            cadenaFechaTimbre.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cadenaFechaTimbre.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            cadenaFechaTimbre.Border = PdfPCell.NO_BORDER;

            PdfPCell seccionUsuarioDocumento = new PdfPCell(new Paragraph("Firma: " + NombreUsuarioDocumento.ToString(), fuente));
            seccionUsuarioDocumento.VerticalAlignment = PdfPCell.ALIGN_TOP;
            seccionUsuarioDocumento.HorizontalAlignment = PdfPCell.LEFT_BORDER;
            seccionUsuarioDocumento.Border = PdfPCell.NO_BORDER;
            seccionUsuarioDocumento.Colspan = 2;

            PdfPCell cadenaUsuarioDocumento = new PdfPCell(new Paragraph(NombreUsuarioDocumento.ToString(), fuente));
            cadenaUsuarioDocumento.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cadenaUsuarioDocumento.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            cadenaUsuarioDocumento.Border = PdfPCell.NO_BORDER;

            tableTextoCentro.AddCell(seccionDGPO);
            //tableTextoCentro.AddCell(cadenaDGPO);
            tableTextoCentro.AddCell(seccionFechaTimbre);
            tableTextoCentro.AddCell(seccionUsuarioDocumento);

            PdfPCell cuadroTextoCentro = new PdfPCell(tableTextoCentro);
            cuadroTextoCentro.Border = PdfPCell.NO_BORDER;
            cuadroTextoCentro.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

            String paginas = "Página " + writer.PageNumber.ToString() + " de " + Paginas;

            PdfPCell seccionNpaginas = new PdfPCell(new Paragraph(paginas, fuente));
            seccionNpaginas.VerticalAlignment = PdfPCell.ALIGN_TOP;
            seccionNpaginas.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            seccionNpaginas.Border = PdfPCell.NO_BORDER;

            table.AddCell(SeccionLogotipo);
            table.AddCell(cuadroTextoCentro);
            table.AddCell(seccionNpaginas);

            PdfPCell cuadro = new PdfPCell(table);
            cuadro.BorderWidth = 0;
            tableP.AddCell(cuadro);
            tableP.TotalWidth = page.Width - document.Left - document.Left;
            tableP.WriteSelectedRows(0, -1, document.Left, document.Bottom + 10, writer.DirectContent);

            //---------------------PIE DE PAGINA------------------------------------
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(52, 52);
            cb.EndText();
            //cb.AddTemplate(tplhead, 50, 650);
            //cb.AddTemplate(tplhead, 60.0, 5.0);
        }

        public String sRutaImgen;
        public String RutaImagen
        {
            get { return sRutaImgen; }
            set { sRutaImgen = value; }
        }
        public DateTime dFechaTimbre;
        public DateTime FechaTimbre
        {
            get { return dFechaTimbre; }
            set { dFechaTimbre = value; }
        }
        public String sSello;
        public String Sello
        {
            get { return sSello; }
            set { sSello = value; }
        }
        public String sNombreDocumentoPDF;
        public String NombreDocumentoPDF
        {
            get { return sNombreDocumentoPDF; }
            set { sNombreDocumentoPDF = value; }
        }
        public String sNombreDocumentoOriginal;
        public String NombreDocumentoOriginal
        {
            get { return sNombreDocumentoOriginal; }
            set { sNombreDocumentoOriginal = value; }
        }

        public String sNombreUsuarioDocumento;
        public String NombreUsuarioDocumento
        {
            get { return sNombreUsuarioDocumento; }
            set { sNombreUsuarioDocumento = value; }
        }

        //El certificado no se considera en esta etapa
        //public String sCertificado; 
        //public String Certificado{get { return sCertificado; } set { sCertificado = value; } }
        public Int32 nPaginas;
        PdfContentByte cb;
        PdfTemplate tpl;
        PdfTemplate tplhead;
        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont bfBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        public Int32 Paginas
        {
            get { return nPaginas; }
            set { nPaginas = value; }
        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            tpl.BeginText();
            tpl.SetFontAndSize(bf, 8);
            tpl.SetTextMatrix(38, 47);
            tpl.ShowText("" + (writer.PageNumber - 1).ToString());
            tpl.EndText();
            base.OnCloseDocument(writer, document);
        }
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            cb = writer.DirectContent;
            tpl = cb.CreateTemplate(550, 95);
            tplhead = cb.CreateTemplate(550, 200);

            base.OnOpenDocument(writer, document);
            //PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            //tabFot.SpacingAfter = 10F;
            //PdfPCell cell;
            //tabFot.TotalWidth = 300F;
            //cell = new PdfPCell(new Phrase("Header"));
            //tabFot.AddCell(cell);
            //tabFot.WriteSelectedRows(0, -1, 150, document.Top, writer.DirectContent);
        }
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        public static string GetMes(int nMes)
        {
            string sMes = "";
            switch (nMes)
            {
                case 1: sMes = "Enero"; break;
                case 2: sMes = "Febrero"; break;
                case 3: sMes = "Marzo"; break;
                case 4: sMes = "Abril"; break;
                case 5: sMes = "Mayo"; break;
                case 6: sMes = "Junio"; break;
                case 7: sMes = "Julio"; break;
                case 8: sMes = "Agosto"; break;
                case 9: sMes = "Septiembre"; break;
                case 10: sMes = "Octubre"; break;
                case 11: sMes = "Noviembre"; break;
                case 12: sMes = "Diciembre"; break;
            }
            return sMes;
        }
    }
}
