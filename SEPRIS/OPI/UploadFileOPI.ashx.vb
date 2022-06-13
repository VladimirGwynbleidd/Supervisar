Imports System.Web
Imports System.Web.Configuration
Imports System.Web.Services

Public Class UploadFileOPI
    Implements System.Web.IHttpHandler


    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim folio As Integer = context.Request("Folio")
        Dim idDocumento As Integer = context.Request("IdDocumento")
        Dim usuario As String = context.Request("Usuario")
        Dim prefijo As String = context.Request("prefijo")
        Dim t_idFolio As String = context.Request("FolioOPI")
        Dim EsNuevo As Integer = context.Request("EsNuevo")

        If context.Request.Files.Count > 0 Then
            Dim files As HttpFileCollection = context.Request.Files
            For Each key As String In files
                Dim file As HttpPostedFile = files(key)
                If EsNuevo = 1 Then
                    CargarArchivoNuevo(folio, file, idDocumento, usuario, prefijo, t_idFolio)
                Else
                    CargarArchivo(folio, file, idDocumento, usuario, prefijo, t_idFolio)
                End If
            Next
        End If
    End Sub

    Private Function CargarArchivoNuevo(folio As Integer, file As HttpPostedFile, IdDocumento As Integer, usuario As String, Optional prefijo As String = "", Optional FolioOPI As String = "") As Boolean
        'CAGC Para obtener solo el nombre del documento (file.FileName)
        ' Tambein en este metodo se reemplazó "file.FileName" por  NombreArchivo
        Dim s = file.FileName
        Dim words As String() = s.Split(New Char() {"\"c})
        Dim NombreArchivo As String
        If words.Length > 0 Then
            NombreArchivo = words(words.Length - 1)
        Else
            NombreArchivo = file.FileName
        End If

        'Validar Extensiones
        Dim lsExtArchivo As String = System.IO.Path.GetExtension(NombreArchivo)

        'Validat Tamaño
        Dim liLimiteArchivoCarga As Integer = ObtenerTamMaximoArch()
        If file.ContentLength > liLimiteArchivoCarga Then
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
        lsAuxNombreDoc = ObtenerNombreDocumento(prefijo, FolioOPI) + lsExtArchivo

        lsAuxOriNombre = NombreArchivo



        'Guarda el archivo en el servidor de APP
        Dim lsRutaTemp As String = System.IO.Path.GetTempPath()

        'Lo elimina si existe
        EliminaArchivoTemporal(lsRutaTemp & NombreArchivo)

        Try
            file.SaveAs(lsRutaTemp & NombreArchivo)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento("Faltan permisos para CREAR el documento temporal que se genera en el servidor en la carpeta [" & System.IO.Path.GetTempPath() & "] antes de enviarse a SharePoint" &
                                           ex.ToString(), EventLogEntryType.Error, "ucDocumentos.aspx.vb, CargarArchivoSharePoint", "")
        End Try

        If Not System.IO.File.Exists(lsRutaTemp & NombreArchivo) Then
            'MensajeDocs = "No se pudo guardar temporalmente el documento en Servidor Web."
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
            Exit Function
        End If


        Shp.RutaArchivo = lsRutaTemp


        Dim reemplazar As Boolean = False
        Dim id_archivo As Integer


        Dim archivos As DataTable = Entities.DocumentoOPI.ObtenerArchivos(folio, IdDocumento)

        lsAuxNombreDoc = lsAuxNombreDoc.Substring(0, lsAuxNombreDoc.Length - System.IO.Path.GetExtension(lsAuxNombreDoc).Length) + "V1_" + (archivos.Rows.Count + 1).ToString() + System.IO.Path.GetExtension(lsAuxNombreDoc)

        'Obtiene nombre real del documento a como quedar en sharepoint
        Shp.NombreArchivo = Utilerias.Generales.ObtenerNombreArchivo(lsAuxNombreDoc)
        Shp.NombreArchivoOri = NombreArchivo

        If Not Shp.UploadFileToSharePoint() Then

            Utilerias.ControlErrores.EscribirEvento("El archivo no pudo cargarse en sharepoint", EventLogEntryType.Error, "UploadFileOPI.ashx.vb", "")

            If reemplazar = False Then
                Dim doc As New Entities.DocumentoOPI
                doc.Folio = folio
                doc.IdDocumento = IdDocumento
                doc.Prefijo = prefijo
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Agregar()
            Else
                Dim doc As New Entities.DocumentoOPI
                doc.Folio = folio
                doc.IdDocumento = id_archivo
                doc.Prefijo = prefijo
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Actualizar()
            End If
        Else

            If reemplazar = False Then
                Dim doc As New Entities.DocumentoOPI
                doc.Folio = folio
                doc.IdDocumento = IdDocumento
                doc.Prefijo = prefijo
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Agregar()
            Else
                Dim doc As New Entities.DocumentoOPI
                doc.Folio = folio
                doc.IdDocumento = id_archivo
                doc.Prefijo = prefijo
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Actualizar()
            End If
        End If

        'Elimina el archivo en el servidor de APP
        EliminaArchivoTemporal(lsRutaTemp & NombreArchivo)

    End Function


    Private Function CargarArchivo(folio As Integer, file As HttpPostedFile, IdDocumento As Integer, usuario As String, Optional prefijo As String = "", Optional FolioOPI As String = "") As Boolean
        'CAGC Para obtener solo el nombre del documento (file.FileName)
        ' Tambein en este metodo se reemplazó "file.FileName" por  NombreArchivo
        Dim s = file.FileName
        Dim words As String() = s.Split(New Char() {"\"c})
        Dim NombreArchivo As String
        If words.Length > 0 Then
            NombreArchivo = words(words.Length - 1)
        Else
            NombreArchivo = file.FileName
        End If

        'Validar Extensiones
        Dim lsExtArchivo As String = System.IO.Path.GetExtension(NombreArchivo)

        'Validat Tamaño
        Dim liLimiteArchivoCarga As Integer = ObtenerTamMaximoArch()
        If file.ContentLength > liLimiteArchivoCarga Then
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
        lsAuxNombreDoc = ObtenerNombreDocumento(prefijo, FolioOPI) + lsExtArchivo

        lsAuxOriNombre = NombreArchivo



        'Guarda el archivo en el servidor de APP
        Dim lsRutaTemp As String = System.IO.Path.GetTempPath()

        'Lo elimina si existe
        EliminaArchivoTemporal(lsRutaTemp & NombreArchivo)

        Try
            file.SaveAs(lsRutaTemp & NombreArchivo)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento("Faltan permisos para CREAR el documento temporal que se genera en el servidor en la carpeta [" & System.IO.Path.GetTempPath() & "] antes de enviarse a SharePoint" &
                                           ex.ToString(), EventLogEntryType.Error, "ucDocumentos.aspx.vb, CargarArchivoSharePoint", "")
        End Try

        If Not System.IO.File.Exists(lsRutaTemp & NombreArchivo) Then
            'MensajeDocs = "No se pudo guardar temporalmente el documento en Servidor Web."
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Validacion", "AvisoDocumentos('" & Me.divAvisoDocs.ClientID & "');", True)
            Exit Function
        End If


        Shp.RutaArchivo = lsRutaTemp


        Dim reemplazar As Boolean = False
        Dim id_archivo As Integer
        Dim nom_archivo As String
        Dim nom_archivo_simple As String
        Dim ver_archivo As String
        Dim ext_archivo As String
        Dim nva_ver_archivo As Integer


        Dim archivos As DataTable = Entities.DocumentoOPI.ObtenerArchivos(folio, IdDocumento)
        If archivos.Rows.Count > 0 Then
            id_archivo = archivos.Rows(archivos.Rows.Count - 1)("I_ID")
            nom_archivo = archivos.Rows(archivos.Rows.Count - 1)("T_DSC_NOMBRE_DOCUMENTO")
            ext_archivo = System.IO.Path.GetExtension(lsAuxNombreDoc)
            nom_archivo_simple = nom_archivo.Replace(System.IO.Path.GetExtension(nom_archivo), "")
            Dim partes As String() = nom_archivo_simple.Split("_")
            ver_archivo = partes(2)
            nom_archivo_simple = nom_archivo_simple.Replace(nom_archivo_simple.Substring(nom_archivo_simple.IndexOf("_"), 3), "")

            nva_ver_archivo = CInt(ver_archivo.Replace("V", "")) + 1
            lsAuxNombreDoc = partes(0) + "_" + partes(1) + "_V" + nva_ver_archivo.ToString() + "_" + partes(3) + ext_archivo
            reemplazar = True
        Else
            lsAuxNombreDoc = lsAuxNombreDoc.Substring(0, lsAuxNombreDoc.Length - 4) + "_V1" + lsAuxNombreDoc.Substring(lsAuxNombreDoc.Length - 4, 4)
        End If

        'Obtiene nombre real del documento a como quedar en sharepoint
        Shp.NombreArchivo = Utilerias.Generales.ObtenerNombreArchivo(lsAuxNombreDoc)
        Shp.NombreArchivoOri = NombreArchivo

        If Not Shp.UploadFileToSharePoint() Then

            Utilerias.ControlErrores.EscribirEvento("El archivo no pudo cargarse en sharepoint", EventLogEntryType.Error, "UploadFileOPI.ashx.vb", "")

            If reemplazar = False Then
                Dim doc As New Entities.DocumentoOPI
                doc.Folio = folio
                doc.IdDocumento = IdDocumento
                doc.Prefijo = prefijo
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Agregar()
            Else
                Dim doc As New Entities.DocumentoOPI
                doc.Folio = folio
                doc.IdDocumento = id_archivo
                doc.Prefijo = prefijo
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Actualizar()
            End If
        Else

            If reemplazar = False Then
                Dim doc As New Entities.DocumentoOPI
                doc.Folio = folio
                doc.IdDocumento = IdDocumento
                doc.Prefijo = prefijo
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Agregar()
            Else
                Dim doc As New Entities.DocumentoOPI
                doc.Folio = folio
                doc.IdDocumento = id_archivo
                doc.Prefijo = prefijo
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Actualizar()
            End If
        End If

        'Elimina el archivo en el servidor de APP
        EliminaArchivoTemporal(lsRutaTemp & NombreArchivo)

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
        If System.IO.File.Exists(lsRutaTemp) Then
            Try
                System.IO.File.Delete(lsRutaTemp)
            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento("Faltan permisos para borrar el documento temporal que se genera en el servidor en la carpeta [" & System.IO.Path.GetTempPath() & "] antes de enviarse a SharePoint" &
                                                    ex.ToString(), EventLogEntryType.Error, "ucDocumentos.aspx.vb, CargarArchivoSharePoint", "")
            End Try
        End If
    End Sub

    Private Sub ConfigurarSharePointSepris(ByRef Shp As Utilerias.SharePointManager)
        Shp.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSEPRIS").ToString()
        Shp.Usuario = WebConfigurationManager.AppSettings("SharePointUserSEPRIS").ToString()
        Shp.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SharePointPasswordSEPRIS").ToString())
        Shp.Dominio = WebConfigurationManager.AppSettings("SharePointDomainSEPRIS").ToString()
        Shp.Biblioteca = WebConfigurationManager.AppSettings("SharePointBibliotecaSEPRIS-PC-OPI").ToString()
    End Sub

    Private Function ObtenerNombreDocumento(prefijo As String, FolioOPI As String) As String
        Dim lsNombreNew As String = ""
        lsNombreNew = prefijo + "_" + FolioOPI.Replace("/", "") + "_"
        Return lsNombreNew
    End Function

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property


End Class