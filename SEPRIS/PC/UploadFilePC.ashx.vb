Imports System.Web
Imports System.Web.Configuration
Imports System.Web.Services

Public Class UploadFilePC
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim folio As Integer = context.Request("Folio")
        Dim idDocumento As Integer = context.Request("IdDocumento")
        Dim usuario As String = context.Request("Usuario")
        Dim EsNuevo As Integer = context.Request("EsNuevo")

        'Un requerimiento
        If idDocumento = 2 And EsNuevo = 1 Then
            Dim req As New Entities.RequerimientoPC
            req.Folio = folio
            req.Usuario = usuario
            req.Agregar()
        End If


        If context.Request.Files.Count > 0 Then
            Dim files As HttpFileCollection = context.Request.Files
            For Each key As String In files
                Dim file As HttpPostedFile = files(key)
                If EsNuevo = 1 Then
                    CargarArchivoNuevo(folio, file, idDocumento, usuario)
                Else
                    CargarArchivo(folio, file, idDocumento, usuario)
                End If
            Next
        End If

    End Sub

    Private Function CargarArchivoNuevo(folio As Integer, file As HttpPostedFile, IdDocumento As Integer, usuario As String) As Boolean
        'MMOB Para obtener solo el nombre del docuemnto (file.FileName)
        ' Tambien en este metodo se reemplazó "file.FileName" por  NombreArchivo
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
        lsAuxNombreDoc = ObtenerNombreDocumento(folio, IdDocumento) + lsExtArchivo

        lsAuxOriNombre = NombreArchivo



        'Guarda el archivo en el servidor de APP
        Dim lsRutaTemp As String = System.IO.Path.GetTempPath()

        'Lo elimina si existe
        EliminaArchivoTemporal(lsRutaTemp & NombreArchivo)

        Try

            Dim tempor = lsRutaTemp & NombreArchivo '.Substring()
            file.SaveAs(tempor)
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


        Dim archivos As DataTable = Entities.DocumentoPC.ObtenerArchivos(folio, IdDocumento)
        'If accion = "R" Then

        '    id_archivo = archivos.Rows(archivos.Rows.Count - 1)("I_ID")
        '    nom_archivo = archivos.Rows(archivos.Rows.Count - 1)("T_DSC_NOMBRE_DOCUMENTO")
        '    nom_archivo_simple = nom_archivo.Substring(0, nom_archivo.Length - 7)
        '    ver_archivo = nom_archivo.Substring(nom_archivo.Length - 6, 2)
        '    ext_archivo = lsAuxNombreDoc.Substring(lsAuxNombreDoc.Length - 4, 4)
        '    nva_ver_archivo = CInt(ver_archivo.Replace("V", "")) + 1
        '    lsAuxNombreDoc = nom_archivo_simple + "_V" + nva_ver_archivo.ToString() + ext_archivo
        '    reemplazar = True
        'Else
        '    lsAuxNombreDoc = lsAuxNombreDoc.Substring(0, lsAuxNombreDoc.Length - 4) + "_V1" + lsAuxNombreDoc.Substring(lsAuxNombreDoc.Length - 4, 4)
        'End If

        lsAuxNombreDoc = lsAuxNombreDoc.Substring(0, lsAuxNombreDoc.Length - System.IO.Path.GetExtension(lsAuxNombreDoc).Length) + "V1_" + (archivos.Rows.Count + 1).ToString() + System.IO.Path.GetExtension(lsAuxNombreDoc)

        'Obtiene nombre real del documento a como quedar en sharepoint
        Shp.NombreArchivo = Utilerias.Generales.ObtenerNombreArchivo(lsAuxNombreDoc)
        Shp.NombreArchivoOri = NombreArchivo


        If Not Shp.UploadFileToSharePoint() Then

            Utilerias.ControlErrores.EscribirEvento("El archivo no pudo cargarse en sharepoint", EventLogEntryType.Error, "UploadFilePC.ashx.vb", "")

            If reemplazar = False Then
                Dim doc As New Entities.DocumentoPC
                doc.Folio = folio
                doc.IdDocumento = IdDocumento
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Agregar()


            Else
                Dim doc As New Entities.DocumentoPC
                doc.Folio = folio
                doc.IdDocumento = id_archivo
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Actualizar()

            End If

        Else
            If reemplazar = False Then
                Dim doc As New Entities.DocumentoPC
                doc.Folio = folio
                doc.IdDocumento = IdDocumento
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Agregar()


            Else
                Dim doc As New Entities.DocumentoPC
                doc.Folio = folio
                doc.IdDocumento = id_archivo
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Actualizar()

            End If

        End If

        'Elimina el archivo en el servidor de APP
        EliminaArchivoTemporal(lsRutaTemp & NombreArchivo)

    End Function


    Private Function CargarArchivo(folio As Integer, file As HttpPostedFile, IdDocumento As Integer, usuario As String) As Boolean
        'MMOB Para obtener solo el nombre del docuemnto (file.FileName)
        ' Tambien en este metodo se reemplazó "file.FileName" por  NombreArchivo
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
        lsAuxNombreDoc = ObtenerNombreDocumento(folio, IdDocumento) + lsExtArchivo

        lsAuxOriNombre = NombreArchivo



        'Guarda el archivo en el servidor de APP
        Dim lsRutaTemp As String = System.IO.Path.GetTempPath()

        'Lo elimina si existe
        EliminaArchivoTemporal(lsRutaTemp & NombreArchivo)

        Try

            Dim tempor = lsRutaTemp & NombreArchivo '.Substring()
            file.SaveAs(tempor)
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


        Dim archivos As DataTable = Entities.DocumentoPC.ObtenerArchivos(folio, IdDocumento)
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
            Utilerias.ControlErrores.EscribirEvento("El archivo no pudo cargarse en sharepoint", EventLogEntryType.Error, "UploadFilePC.ashx.vb", "")

            If reemplazar = False Then
                Dim doc As New Entities.DocumentoPC
                doc.Folio = folio
                doc.IdDocumento = IdDocumento
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Agregar()


            Else
                Dim doc As New Entities.DocumentoPC
                doc.Folio = folio
                doc.IdDocumento = id_archivo
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Actualizar()

            End If

        Else
            If reemplazar = False Then
                Dim doc As New Entities.DocumentoPC
                doc.Folio = folio
                doc.IdDocumento = IdDocumento
                doc.NombreDocumento = Shp.NombreArchivo
                doc.NombreDocumentoSh = Shp.NombreArchivo
                doc.Usuario = usuario
                doc.Agregar()


            Else
                Dim doc As New Entities.DocumentoPC
                doc.Folio = folio
                doc.IdDocumento = id_archivo
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

    Private Function ObtenerNombreDocumento(folio As Integer, IdDocumento As Integer) As String
        Dim lsNombreNew As String = ""
        Dim PC As New Entities.PC(folio)
        lsNombreNew = Entities.DocumentoPC.ObtenerUno(IdDocumento).Rows(0)("T_PREFIJO").ToString() + "_" + PC.FolioSupervisar.Replace("/", "") + "_"
        Return lsNombreNew
    End Function

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class