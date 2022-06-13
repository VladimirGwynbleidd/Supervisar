Public Class SancionPC
    Inherits System.Web.UI.UserControl

    Public Property IdSancion As String

    Public Sub Inicializar(FolioSisan As String, Optional isDocumentos As Boolean = False)

        IdSancion = FolioSisan

        lblFolioSISAN.Text = IdSancion

        Dim datosSancion As DataTable = ConexionSISAN.ObtenerSancion(IdSancion)

        If datosSancion.Rows.Count > 0 Then
            lblEstatusSISAN.Text = ConexionSISAN.ObtenerEstatus(datosSancion.Rows(0)("I_ID_STATUS"))

            txtAbogadosSancionesSISAN.Text = ConexionSISAN.ObtenerNombre(datosSancion.Rows(0)("T_ID_USUARIO_RECIBE").ToString())
            txtAbogadoSuperiorSISAN.Text = ConexionSISAN.ObtenerNombre(datosSancion.Rows(0)("T_ID_USUARIO_ASIGNO").ToString())

            Dim dtObserviaciones As DataTable = ConexionSISAN.ObtenerObservaciones(datosSancion.Rows(0)("I_ID_IRREGULARIDAD").ToString())
            txtComentariosSISAN.Text = dtObserviaciones.Rows(0)("T_OBSERVACIONES").ToString()
            txtUsuarioSISAN.Text = ConexionSISAN.ObtenerNombre(dtObserviaciones.Rows(0)("T_ID_USUARIO_REMITENTE").ToString())


            If datosSancion.Rows(0)("I_ID_STATUS") = 10 Then
                trMonto.Visible = True

                txtFechaSISAN.Text = datosSancion.Rows(0)("F_FECH_PAGO_AFORE").ToString()
                txtMontoSISAN.Text = datosSancion.Rows(0)("D_MTO_TOTAL_MULTAS").ToString()
                txtPagoSISAN.Text = datosSancion.Rows(0)("F_FECH_PAGO_MULTA").ToString()
            End If

            If isDocumentos Then
                trDocumentos.Visible = True
                DocumentosSISANPC(datosSancion.Rows(0)("I_ID_IRREGULARIDAD").ToString())

            End If

        End If

    End Sub


    Private Sub DocumentosSISANPC(idIrr As String)
        Dim dtDocumentosSisan As DataTable = ConexionSISAN.ObtenerDocumentos(idIrr)

        gvDocumentosSISAN.DataSource = dtDocumentosSisan
        gvDocumentosSISAN.DataBind()
    End Sub



    Protected Sub gvDocumentosSISAN_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDocumentosSISAN.RowDataBound


        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim FechaAcuse As Label = Nothing

            'FechaAcuse = TryCast(e.Row.Cells(4).FindControl("F_FECH_ACUSE"), Label)

            'Dim resultado As String
            'resultado = ConexionSISAN.ObtenerFechaAcuse(e.Row.DataItem("I_ID_IRREGULARIDAD"))
            'FechaAcuse.Text = resultado



            Dim btnRegistroSICOD As System.Web.UI.WebControls.Button = CType(e.Row.FindControl("btnRegistroSICOD"), System.Web.UI.WebControls.Button)
            Dim DocumentosLink As System.Web.UI.WebControls.LinkButton = CType(e.Row.FindControl("Link"), System.Web.UI.WebControls.LinkButton)
            'Dim tablaArchivos As New Table
            'Dim tablaOficios As New Table

            'Dim rowArchivo As New TableRow
            'Dim cellArchivo As New TableCell
            'Dim linkArchivo As New LinkButton
            ' DocumentosLink.Text = e.Row.Cells(2).Text
            'linkArchivo.Text = e.Row.Cells(2).Text
            'DocumentosLink.OnClientClick = "__doPostBack('" + Button1.UniqueID + "', '" + e.Row.Cells(2).Text + "'); return false;"
            DocumentosLink.OnClientClick = "__doPostBack('" + Button1.UniqueID + "', '" + DocumentosLink.Text + "'); return false;"
            'cellArchivo.Controls.Add(linkArchivo)
            'rowArchivo.Cells.Add(cellArchivo)
            'tablaArchivos.Rows.Add(rowArchivo)


            e.Row.Cells(0).Text = Constantes.TipoDocumentoSicod(e.Row.Cells(0).Text)


        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim usuario As String
        Dim passwd As String
        Dim dominio As String
        Dim nom_archivo As String = String.Empty
        Dim biblioteca As String
        Dim ServSharepoint As String
        Dim cliente As System.Net.WebClient = New System.Net.WebClient
        Dim enc As New YourCompany.Utils.Encryption.Encryption64


        nom_archivo = Request("__EVENTARGUMENT")

        If nom_archivo <> "Sin imagen" Then
            Dim Archivo() As Byte = Nothing
            Dim filename As String = "attachment; filename=" & nom_archivo

            Try
                usuario = System.Web.Configuration.WebConfigurationManager.AppSettings("SPSISVIGSICODUsr")
                passwd = Utilerias.Cifrado.DescifrarAES(System.Web.Configuration.WebConfigurationManager.AppSettings("SPSISVIGSICODPas"))
                ServSharepoint = System.Web.Configuration.WebConfigurationManager.AppSettings("SPSISVIGSICODSrv")
                dominio = System.Web.Configuration.WebConfigurationManager.AppSettings("SPSISVIGSICODDom")
                biblioteca = System.Web.Configuration.WebConfigurationManager.AppSettings("SPSISVIGSICODLib")

                cliente.Credentials = New System.Net.NetworkCredential(usuario, passwd, dominio)

                Dim Url As String = ServSharepoint & "/" & biblioteca & "/" & nom_archivo

                Archivo = cliente.DownloadData(ResolveUrl(Url))

            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento("No se puede abrir el documento: " & nom_archivo, EventLogEntryType.Error)
            End Try

            If Not Archivo Is Nothing Then

                Dim dotPosicion As Integer = nom_archivo.LastIndexOf(".")

                Dim tipo_arch As String = nom_archivo.Substring(dotPosicion + 1)

                Select Case tipo_arch
                    Case "zip"
                        Response.ContentType = "application/x-zip-compressed"
                    Case "pdf"
                        Response.ContentType = "application/pdf"
                    Case "csv"
                        Response.ContentType = "application/csv"
                    Case "doc"
                        Response.ContentType = "application/doc"
                    Case "docx"
                        Response.ContentType = "application/docx"
                    Case "xls"
                        Response.ContentType = "application/xls"
                    Case "xlsx"
                        Response.ContentType = "application/xlsx"
                    Case "png"
                        Response.ContentType = "application/png"
                    Case "gif"
                        Response.ContentType = "application/gif"
                    Case "jpg"
                        Response.ContentType = "application/jpg"
                    Case "csv"
                        Response.ContentType = "application/csv"
                    Case "txt"
                        Response.ContentType = "application/txt"
                    Case "ppt"
                        Response.ContentType = "application/vnd.ms-project"
                    Case "pptx"
                        Response.ContentType = "application/vnd.ms-project"
                    Case "bmp"
                        Response.ContentType = "image/bmp"
                    Case "tif"
                        Response.ContentType = "image/tiff"
                End Select

                Response.AddHeader("content-disposition", filename)

                Response.BinaryWrite(Archivo)

                Response.End()
            End If

        End If

        ''btnActulizarGrid_Click(sender, e)
        'DocumentosSISANPC(Session("I_ID_IRREGULARIDAD").ToString())

    End Sub


End Class