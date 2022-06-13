Imports System.Web.Configuration
Imports System.IO
Imports Utilerias

Public Class Documentos
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property Usuario As String
        Get
            Return TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        End Get
    End Property
    Public ReadOnly Property Area As Integer
        Get
            Return TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea
        End Get
    End Property
    Public ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property

    Public Property FolioSICOD
        Get
            Return Session("IdOficioSEPRIS")
        End Get
        Set(value)
            Session("IdOficioSEPRIS") = value
        End Set
    End Property



    Public Sub Inicializar()

        'Se inicializa sesion para ser utilizada por SICOD
        Session("Usuario") = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario

        If Not Page.IsPostBack Then



            Dim PC As Entities.PC = TryCast(Session("PC"), Entities.PC)





            Dim dtDocumentos As DataTable = Entities.DocumentoPC.ObtenerTodos
            Dim dvDocumentos As DataView = dtDocumentos.DefaultView
            dvDocumentos.RowFilter = "I_PASO_INICIAL<=" + PC.IdPaso.ToString() + " AND I_PASO_FINAL<=" + PC.IdPaso.ToString()
            gvConsultaDocs.DataSource = dvDocumentos
            gvConsultaDocs.DataBind()


            Dim enc As New YourCompany.Utils.Encryption.Encryption64
            Dim Usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodUser")
            Dim Password As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
            Dim Dominio As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")
            Dim mycredentialCache As System.Net.CredentialCache = New System.Net.CredentialCache()
            Dim credentials As System.Net.NetworkCredential = New System.Net.NetworkCredential(Usuario, Password, Dominio)
            Dim proxySICOD As New WR_SICOD.ws_SICOD
            proxySICOD.Credentials = credentials
            proxySICOD.ConnectionGroupName = "SEPRIS"

            'Generales.CargarCombo(ddlPuestoDestinatario, proxySICOD.GetCatalogoCargoDestinatarioOficios(False).ToList(), "Value", "Key")
            Try
                ddlPuestoDestinatario.DataSource = proxySICOD.GetCatalogoCargoDestinatarioOficios(False)
                ddlPuestoDestinatario.DataTextField = "Value"
                ddlPuestoDestinatario.DataValueField = "Key"
                ddlPuestoDestinatario.DataBind()
                ddlPuestoDestinatario.Items.Insert(0, New ListItem("-Seleccionar-", "-1"))
            Catch ex As Exception
                Throw ex
            End Try

            Generales.CargarCombo(ddlAreaFirma, proxySICOD.GetUnidadesAdministrativas(WR_SICOD.UnidadAdministrativaTipo.Funcional, WR_SICOD.UnidadAdministrativaEstatus.Activo).Tables(0), "DSC_CODIGO_UNIDAD_ADM", "ID_UNIDAD_ADM")
            Generales.CargarCombo(ddlAreaRubrica, proxySICOD.GetUnidadesAdministrativas(WR_SICOD.UnidadAdministrativaTipo.Funcional, WR_SICOD.UnidadAdministrativaEstatus.Activo).Tables(0), "DSC_CODIGO_UNIDAD_ADM", "ID_UNIDAD_ADM")

        End If


    End Sub

    Protected Sub btnAdjuntarDocumento_Click(sender As Object, e As ImageClickEventArgs)
        Dim a As Integer = 0
        Dim imageFileUpload As System.Web.UI.WebControls.ImageButton = CType(sender, System.Web.UI.WebControls.ImageButton)

        Dim id As Integer = gvConsultaDocs.Rows(0).DataItem("I_ID_DOCUMENTO")
        Dim rowIndex As Integer = imageFileUpload.CommandArgument

        Dim lstUploads As List(Of FileUpload) = gvConsultaDocs.Rows(0).GetAllControlsOfType(Of FileUpload)()




    End Sub


    Private Sub gvConsultaDocs_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvConsultaDocs.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim imageFileUpload As System.Web.UI.WebControls.ImageButton = CType(e.Row.FindControl("btnAdjuntarDocumento"), System.Web.UI.WebControls.ImageButton)
            Dim imageBuscarSICOD As System.Web.UI.WebControls.ImageButton = CType(e.Row.FindControl("btnBuscarSICOD"), System.Web.UI.WebControls.ImageButton)
            Dim buttonRegistrar As System.Web.UI.WebControls.Button = CType(e.Row.FindControl("btnRegitrar"), System.Web.UI.WebControls.Button)
            Dim linkSeguimiento As System.Web.UI.WebControls.LinkButton = CType(e.Row.FindControl("lnkSeguimiento"), System.Web.UI.WebControls.LinkButton)


            Dim dtArchivos As DataTable = Entities.DocumentoPC.ObtenerArchivos(Folio, e.Row.DataItem("I_ID_DOCUMENTO").ToString()) ' MMOB ANTES "Folio" AHORA "FolioSicod"
            If dtArchivos.Rows.Count > 0 Then
                Dim linkArchivo As New LinkButton

                imageBuscarSICOD.Visible = False

                linkArchivo.Text = dtArchivos.Rows(0)("T_DSC_NOMBRE_DOCUMENTO")
                e.Row.Cells(2).Controls.Add(linkArchivo)


                If e.Row.DataItem("B_REG_SICOD") Then
                    buttonRegistrar.Visible = True
                End If


                If dtArchivos.Rows(0)("T_FOLIO_SICOD").ToString() <> "" Then
                    buttonRegistrar.Visible = False
                    Dim linkFolioSICOD As New LinkButton
                    linkFolioSICOD.Text = dtArchivos.Rows(0)("T_FOLIO_SICOD").ToString()
                    linkFolioSICOD.OnClientClick = "ConsultarOficios('" + dtArchivos.Rows(0)("T_FOLIO_SICOD").ToString().Replace("/", "-") + "'); return false;"

                    e.Row.Cells(5).Controls.Add(linkFolioSICOD)



                End If

            Else
                'Si no hay documentos, presentar un upload
                Dim uploadcontrol As New FileUpload
                e.Row.Cells(2).Controls.Add(uploadcontrol)
            End If

            If e.Row.DataItem("B_BUSCAR_SICOD") Then
                imageBuscarSICOD.Visible = True
            End If


        End If



    End Sub



    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim lstUploads As List(Of FileUpload) = gvConsultaDocs.Rows(hfRowIndex.Value).GetAllControlsOfType(Of FileUpload)()

        Dim IdDocumento As Integer = CInt(gvConsultaDocs.DataKeys(hfRowIndex.Value).Value.ToString())

        For Each upload As FileUpload In lstUploads
            If upload.HasFile Then
                CargarArchivo(upload, IdDocumento)
            End If
        Next
    End Sub

    Private Sub ConfigurarSharePointSepris(ByRef Shp As Utilerias.SharePointManager)
        Shp.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSEPRIS").ToString()
        Shp.Usuario = WebConfigurationManager.AppSettings("SharePointUserSEPRIS").ToString()
        Shp.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SharePointPasswordSEPRIS").ToString())
        Shp.Dominio = WebConfigurationManager.AppSettings("SharePointDomainSEPRIS").ToString()
        Shp.Biblioteca = WebConfigurationManager.AppSettings("SharePointBibliotecaSEPRIS").ToString()
    End Sub

    Private Function ObtenerNombreDocumento() As String
        Dim lsNombreNew As String = ""
        Dim lsNombreDoc As String = ""

        lsNombreNew = "PC" + Folio.ToString()

        Return lsNombreNew
    End Function

    Private Function ObtenerTamMaximoArch() As Integer
        'Obtener el maximo permitido
        Dim liLimiteArchivoCarga As Integer
        Try
            liLimiteArchivoCarga = CInt(WebConfigurationManager.AppSettings("LimiteTamArchivo").ToString())
        Catch
            liLimiteArchivoCarga = 49152000
        End Try
        Return liLimiteArchivoCarga
    End Function

    Private Sub EliminaArchivoTemporal(lsRutaTemp As String)
        If File.Exists(lsRutaTemp) Then
            Try
                File.Delete(lsRutaTemp)
            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento("Faltan permisos para borrar el documento temporal que se genera en el servidor en la carpeta [" & Path.GetTempPath() & "] antes de enviarse a SharePoint" &
                                                    ex.ToString(), EventLogEntryType.Error, "ucDocumentos.aspx.vb, CargarArchivoSharePoint", "")
            End Try
        End If
    End Sub

    Private Function CargarArchivo(file As FileUpload, IdDocumento As Integer) As Boolean

        'Validar Extensiones
        Dim lsExtArchivo As String = System.IO.Path.GetExtension(file.FileName)

        'Validat Tamaño
        Dim liLimiteArchivoCarga As Integer = ObtenerTamMaximoArch()
        If file.FileBytes.Length > liLimiteArchivoCarga Then
            'lstErrores.Add("El archivo [" & fuFileUp.FileName & "] para el documento [" & gvConsultaDocs.DataKeys(liIdRen).Item("T_NOM_DOCUMENTO_CAT").ToString() & "] sobrepasa los " & (liLimiteArchivoCarga / 1024 / 1024).ToString() & " Mb permitidos, comuniquese al area de sistemas")
            Return False
        End If

        Dim Shp As New Utilerias.SharePointManager

        ConfigurarSharePointSepris(Shp)

        '---------------------------------------
        ' Guarda el archivo en Sharepoint
        '---------------------------------------
        Dim lsAuxNombreDoc As String = ""
        Dim lsAuxOriNombre As String = ""



        'Obtiene nombre original del documento, como lo vera el usuario, ya sea con la nomeclatura o sin ella
        'SIEMPRE SE DEBE DE GENERAR UNA NOMECLATURA YA QUE ESE ES EL MOMBRE CON EL QUE VA A QUEDAR EN SHAREPOINT
        'PORQUE SI NO EN DIFERENTES VISITAS SE PODRIA GENERAR EL MISMO ARCHIVO
        lsAuxNombreDoc = ObtenerNombreDocumento() + lsExtArchivo

        lsAuxOriNombre = file.FileName

        'Obtiene nombre real del documento a como quedar en sharepoint
        Shp.NombreArchivo = Utilerias.Generales.ObtenerNombreArchivo(lsAuxNombreDoc)

        'Guarda el archivo en el servidor de APP
        Dim lsRutaTemp As String = Path.GetTempPath()

        'Lo elimina si existe
        EliminaArchivoTemporal(lsRutaTemp & file.FileName)

        Try
            file.SaveAs(lsRutaTemp & file.FileName)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento("Faltan permisos para CREAR el documento temporal que se genera en el servidor en la carpeta [" & Path.GetTempPath() & "] antes de enviarse a SharePoint" &
                                           ex.ToString(), EventLogEntryType.Error, "ucDocumentos.aspx.vb, CargarArchivoSharePoint", "")
        End Try

        If Not System.IO.File.Exists(lsRutaTemp & file.FileName) Then
            'MensajeDocs = "No se pudo guardar temporalmente el documento en Servidor Web."
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
            Exit Function
        End If

        Shp.RutaArchivo = lsRutaTemp
        Shp.NombreArchivoOri = file.FileName

        If Not Shp.UploadFileToSharePoint() Then
            ''Elimina el archivo en el servidor de APP

            'lstErrores.Add("El archivo [" & fuFileUp.FileName & "] no se pudo guardar en SharePoint, para el documento [" & gvConsultaDocs.DataKeys(liIdRen).Item("T_NOM_DOCUMENTO_CAT").ToString() & "]")
            Dim doc As New Entities.DocumentoPC
            doc.Folio = Folio
            doc.IdDocumento = IdDocumento
            doc.NombreDocumento = file.FileName
            doc.NombreDocumentoSh = Shp.NombreArchivoOri
            doc.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
            doc.Agregar()
        Else
            Dim doc As New Entities.DocumentoPC
            doc.Folio = Folio
            doc.IdDocumento = IdDocumento
            doc.NombreDocumento = file.FileName
            doc.NombreDocumentoSh = Shp.NombreArchivoOri
            doc.Usuario = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
            doc.Agregar()
        End If

        'Elimina el archivo en el servidor de APP
        EliminaArchivoTemporal(lsRutaTemp & file.FileName)

    End Function



    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

    End Sub
End Class