Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports System.Net
Imports System.Xml
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports SharePoint
'Imports ADExtender
Imports System.Web.Configuration

Public Class SharePointManager
    'Private mRutaArchivo As String
    'Private mServidorSharePoint As String
    'Private mUsuario As String
    'Private mPassword As String
    'Private mBiblioteca As String
    'Private mNombreArchivo As String
    'Private mDominio As String
    'Private mCondicionBusquedaArchivo As String
    'Private mStrBatch As String
    'Private mBinFile() As Byte
    'Private mQueryOptions As String
    'Private mViewFields As String
    'Private mRowLimit As String

    ''' <summary>
    ''' Ruta de donde se va a cargar un archivo o´ ruta a la cual se va a descargar un archivo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RutaArchivo As String


    ''' <summary>
    ''' Url del servidor de sharepoint
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ServidorSharePoint As String


    ''' <summary>
    ''' Usuario del servidor de sharepoint
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Usuario As String


    ''' <summary>
    ''' Password del servidor de sharepoint
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Password As String


    ''' <summary>
    ''' Nombre de la biblioteca del sitio de sharepoint
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Biblioteca As String


    ''' <summary>
    ''' Nombre del archivo a cargar o descargar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NombreArchivo As String
    Public Property NombreArchivoOri As String


    ''' <summary>
    ''' Dominio del usuario del servidor de sharepoint
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Dominio As String


    ''' <summary>
    ''' Condición para busqueda de archvios con CAML(Collaborative Application Markup Language) Query Syntax
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CondicionBusquedaArchivo As String


    ''' <summary>
    ''' Cadena que contiene la sintaxis para actualizar listItems 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StrBatch As String


    ''' <summary>
    ''' Arreglo de byte que contiene la información del archivo que se desea subir a la biblioteca de un sitio de SharePoint
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BinFile As Byte()


    ''' <summary>
    ''' Propiedades del objeto SPQuery (CondicionBusquedaArchivo) con CAML(Collaborative Application Markup Language)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property QueryOptions As String


    ''' <summary>
    ''' Elemento que especifica que campos regresa la consulta y en que orden con CAML(Collaborative Application Markup Language)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ViewFields As String


    ''' <summary>
    ''' Número de registros que va a regresar la consulta 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RowLimit As String = "90000"

    <System.Runtime.InteropServices.DllImport("C:\\WINDOWS\\System32\\advapi32.dll")> _
    Private Shared Function LogonUser(ByVal lpszUsername As String, ByVal lpszDomain As String, ByVal lpszPassword As String, _
        ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, ByRef phToken As Integer) As Boolean
    End Function

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Constructor que recibe como parámetros los datos para conectarse al servidor de SharePoint
    ''' </summary>
    ''' <param name="ServidorSharePoint">Url del servidor de sharepoint</param>
    ''' <param name="Usuario">Usuario del servidor de sharepoint</param>
    ''' <param name="Password">Password del usuario del servidor de sharepoint</param>
    ''' <param name="Dominio">Dominio del usuario del servidor de sharepoint</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ServidorSharePoint As String, ByVal Usuario As String, ByVal Password As String, ByVal Dominio As String)
        Me.ServidorSharePoint = ServidorSharePoint
        Me.Usuario = Usuario
        Me.Password = Password
        Me.Dominio = Dominio
    End Sub

    ''' <summary>
    ''' Carga un archivo a una biblioteca del servidor de sharepoint de una ruta especifica
    ''' IMPORTANTE: Si se intenta subir archivos Office (.docx, pptx, xlsx) usar el método posterior (UploadFileToSharePoint)
    ''' </summary>
    ''' <returns>Regresa un valor boleano indicando si el archivo se cargo exitosamente true de lo contrario false</returns>
    ''' <remarks></remarks>
    Public Function CargarArchivo() As Boolean
        Dim cliente As WebClientEx
        Dim resultado As Boolean = False
        Dim fs As FileStream
        Try
            If BinFile Is Nothing Then
                fs = New FileStream(String.Format("{0}\{1}", RutaArchivo, NombreArchivo), FileMode.Open)
                Dim binFile(CType(fs.Length, Integer)) As Byte
                fs.Read(binFile, 0, CType(fs.Length, Integer))
                fs.Close()
                Me.BinFile = binFile
            End If

            cliente = New WebClientEx
            cliente.Credentials = New NetworkCredential(Usuario, Password, Dominio)

            Dim ruta As String = String.Format("{0}/{1}/{2}", ServidorSharePoint, Biblioteca, NombreArchivo)
            Dim r As New System.Web.UI.Control
            cliente.UploadData(r.ResolveUrl(ruta), "PUT", BinFile)

            resultado = True

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento("Error guardando el archivo WORD en SharePoint, vuelve a intentarlo..." & ex.Message, EventLogEntryType.Error, "SEPRIS", "")
            Return False
        End Try

        Return resultado

    End Function

    ''' <summary>
    '''  Subir archivo a Sharepoint en base a este doc de MS (pero adecuado a funcionar con esta clase)
    ''' http://msdn.microsoft.com/en-us/library/dd902097%28v=office.12%29.aspx
    ''' Usar para subir archivos Office (docx, pptx, xlsx)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UploadFileToSharePoint() As Boolean

        Dim UploadedFilePath As String = RutaArchivo + NombreArchivoOri
        Dim SharePointPath As String = String.Format("{0}/{1}/{2}", ServidorSharePoint, Biblioteca, NombreArchivo)
        Dim resultado As Boolean = False
        Dim response As WebResponse = Nothing

        Try
            'Create a PUT Web request to upload the file.
            Dim request As WebRequest = WebRequest.Create(SharePointPath)
            request.Credentials = New NetworkCredential(Usuario, Password, Dominio)
            request.Method = "PUT"
            request.Timeout = 7200000

            ' Allocate a 1 KB buffer to transfer the file contents.
            ' You can adjust the buffer size as needed, depending on
            ' the number and size of files being uploaded.

            Dim buffer() As Byte = New Byte(1024) {}

            ' Write the contents of the local file to the
            ' request stream.

            Using stream As Stream = request.GetRequestStream()
                Using fsWorkbook As FileStream = File.Open(UploadedFilePath, FileMode.Open, FileAccess.Read)
                    Dim i As Integer = fsWorkbook.Read(buffer, 0, buffer.Length)

                    While i > 0
                        stream.Write(buffer, 0, i)
                        i = fsWorkbook.Read(buffer, 0, buffer.Length)
                    End While

                End Using

            End Using

            ' Make the PUT request.
            response = request.GetResponse()
            resultado = True

        Catch ex As Exception
            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            If Not IsNothing(response) Then
                response.Close()
            End If
        End Try
        Return resultado
    End Function


    Private Function InicializarSiteData() As WSSiteData.SiteData
        Dim spSiteData As WSSiteData.SiteData = Nothing
        Try
            spSiteData = New WSSiteData.SiteData()
            spSiteData.Credentials = New Net.NetworkCredential(Usuario, Password, Dominio)
            spSiteData.PreAuthenticate = True
            spSiteData.Url = ServidorSharePoint & "/_vti_bin/sitedata.asmx"
        Catch es As SoapException
            Throw es
        Catch ex As Exception
            Throw ex
        End Try
        Return spSiteData
    End Function

    Private Function InicializarLists() As WSLists.Lists
        Dim listas As WSLists.Lists = Nothing
        Try
            listas = New WSLists.Lists
            listas.Credentials = New Net.NetworkCredential(Usuario, Password, Dominio)
            listas.PreAuthenticate = True
            listas.Url = ServidorSharePoint & "/_vti_bin/Lists.asmx"
        Catch es As SoapException
            Throw es
        Catch ex As Exception
            Throw ex
        End Try
        Return listas
    End Function


    Private Function ObtenerGUIDSitioSharePoint() As Guid
        Dim guidSitio As Guid
        Try
            Dim siteData As WSSiteData.SiteData = InicializarSiteData()
            Dim webMetaData As New WSSiteData._sWebMetadata
            Dim arrWebWithTime() As WSSiteData._sWebWithTime
            Dim arrListWithTime() As WSSiteData._sListWithTime
            Dim arrUrls As WSSiteData._sFPUrl()
            Dim roles As String
            Dim rolUsuarios() As String
            Dim rolGrupos() As String

            Dim datosSitioSharePoint As UInteger = siteData.GetWeb(webMetaData, arrWebWithTime, arrListWithTime, arrUrls, roles, rolUsuarios, rolGrupos)
            guidSitio = New Guid(webMetaData.WebID)
            Return guidSitio
        Catch es As SoapException
            Throw es
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Descarga un archivo de una biblioteca del servidor de sharepoint a una ruta especifica
    ''' </summary>
    ''' <returns>Regresa un valor boleano indicando si el archivo se descargo exitosamente true de lo contrario false</returns>
    ''' <remarks></remarks>
    Public Function DescargarArchivo() As Boolean
        Dim resultado As Boolean = False
        Try
            resultado = BajarArchivo(ObtenerRutaArchivo(), RutaArchivo, NombreArchivo)
        Catch ex As Exception
            Throw New Exception("Ocurrió un error en la función DescargarArchivo de la clase FuncionesSharePoint.", ex)
        End Try
        Return resultado
    End Function

    Private Function BajarArchivo(ByVal RutaOrigen As String, ByVal RutaDestino As String, ByVal NombreArchivo As String) As Boolean
        Dim cliente As WebClient
        Dim binArchivo() As Byte
        Dim resultado As Boolean = False
        Try
            cliente = New WebClient
            cliente.Credentials = New NetworkCredential(Usuario, Password, Dominio)
            Dim r As New System.Web.UI.Control
            binArchivo = cliente.DownloadData(r.ResolveUrl(RutaOrigen))
            File.WriteAllBytes(String.Format("{0}\{1}", RutaDestino, NombreArchivo), binArchivo)
            resultado = True
        Catch ex As Exception
            Throw ex
        End Try
        Return resultado
    End Function

    ''' <summary>
    ''' Consulta todos los documentos de una biblioteca de un sitio del servidor de sharepoint
    ''' </summary>
    ''' <returns>Regresa un nodo xml</returns>
    ''' <remarks></remarks>
    Public Function ConsultarDocumentos() As XmlNode
        Dim NodoListaDocumentos As XmlNode = Nothing
        Try
            Dim lista As WSLists.Lists = InicializarLists()
            Dim XMLdoc As New XmlDocument()
            Dim NodoQuery As XmlNode = XMLdoc.CreateNode(XmlNodeType.Element, "Query", "")
            If Not CondicionBusquedaArchivo = String.Empty Then
                NodoQuery.InnerXml = CondicionBusquedaArchivo
            End If
            Dim NodoViewFields As XmlNode = XMLdoc.CreateNode(XmlNodeType.Element, "ViewFields", "")
            If Not ViewFields = String.Empty Then
                NodoViewFields.InnerText = ViewFields
            End If
            Dim NodoQueryOptions As XmlNode = XMLdoc.CreateNode(XmlNodeType.Element, "QueryOptions", "")
            If Not QueryOptions = String.Empty Then
                NodoQueryOptions.InnerText = QueryOptions
            End If

            Dim guidSitio As Guid = ObtenerGUIDSitioSharePoint()
            NodoListaDocumentos = lista.GetListItems(Biblioteca, String.Empty, NodoQuery, NodoViewFields, RowLimit, NodoQueryOptions, guidSitio.ToString())

        Catch es As SoapException
            Throw New SoapException("Ocurrió un error en la función ConsultarDocumentos de la clase FuncionesSharePoint.", es.Code, es.Actor, es.Role, es.Lang, es.Detail, es.SubCode, es)
        Catch ex As Exception
            Throw New Exception("Ocurrió un error en la función ConsultarDocumentos de la clase FuncionesSharePoint.", ex)
        End Try
        Return NodoListaDocumentos
    End Function

    ''' <summary>
    ''' Consulta todos los documentos de una biblioteca de un sitio del servidor de SharePoint
    ''' </summary>
    ''' <param name="EnviarWebId">Indica si se va a obtener el WebId del sitio true o false si se va a mandar en nulo</param>
    ''' <returns>Regresa un nodo xml</returns>
    ''' <remarks></remarks>
    Public Function ConsultarDocumentos(ByVal EnviarWebId As Boolean) As XmlNode
        Dim NodoListaDocumentos As XmlNode = Nothing
        Dim rowLimit As String = String.Empty
        Try
            Dim lista As WSLists.Lists = InicializarLists()
            Dim XMLdoc As New XmlDocument()
            Dim NodoQuery As XmlNode = XMLdoc.CreateNode(XmlNodeType.Element, "Query", "")
            If Not CondicionBusquedaArchivo = String.Empty Then
                NodoQuery.InnerXml = CondicionBusquedaArchivo
            End If
            Dim NodoViewFields As XmlNode = XMLdoc.CreateNode(XmlNodeType.Element, "ViewFields", "")
            If Not ViewFields = String.Empty Then
                NodoViewFields.InnerText = ViewFields
            End If
            Dim NodoQueryOptions As XmlNode = XMLdoc.CreateNode(XmlNodeType.Element, "QueryOptions", "")
            If Not QueryOptions = String.Empty Then
                NodoQueryOptions.InnerText = QueryOptions
            End If

            If EnviarWebId Then
                Dim guidSitio As Guid = ObtenerGUIDSitioSharePoint()
                NodoListaDocumentos = lista.GetListItems(Biblioteca, String.Empty, NodoQuery, NodoViewFields, rowLimit, NodoQueryOptions, guidSitio.ToString)
            Else
                NodoListaDocumentos = lista.GetListItems(Biblioteca, String.Empty, NodoQuery, NodoViewFields, rowLimit, NodoQueryOptions, Nothing)
            End If
        Catch es As SoapException
            Throw New SoapException("Ocurrió un error en la función ConsultarDocumentos de la clase FuncionesSharePoint.", es.Code, es.Actor, es.Role, es.Lang, es.Detail, es.SubCode, es)
        Catch ex As Exception
            Throw New Exception("Ocurrió un error en la función ConsultarDocumentos de la clase FuncionesSharePoint.", ex)
        End Try
        Return NodoListaDocumentos
    End Function


    ''' <summary>
    ''' Consulta todos los documentos de una biblioteca de un sitio del servidor de sharepoint
    ''' </summary>
    ''' <returns>Regresa un dataTable con Id y Nombre de archivo</returns>
    ''' <remarks></remarks>
    Public Function ConsultarDocumentosBiblioteca() As DataTable
        Dim dt As DataTable
        dt = New DataTable()
        dt.Columns.Add("IdArch")
        dt.Columns.Add("NombreArchivo")
        Dim NodoListaDocumentos As XmlNode = ConsultarDocumentos()
        Dim xmlDoc As XmlDocument
        xmlDoc = New XmlDocument()
        xmlDoc.LoadXml(NodoListaDocumentos.OuterXml)
        Dim xmlNs As XmlNamespaceManager
        xmlNs = New XmlNamespaceManager(xmlDoc.NameTable)
        xmlNs.AddNamespace("z", "#RowsetSchema")
        Dim rows As XmlNodeList = xmlDoc.SelectNodes("//z:row", xmlNs)
        For Each row As XmlNode In rows
            Dim fila As DataRow = dt.NewRow()
            fila("IdArch") = row.Attributes("ows_ID").Value
            fila("NombreArchivo") = row.Attributes("ows_LinkFilename").Value
            dt.Rows.Add(fila)
        Next
        Return dt
    End Function

    ''' <summary>
    ''' Consulta todos los documentos de una biblioteca de un sitio del servidor de SharePoint
    ''' </summary>
    ''' <param name="pEnviarWebId">Indica si se va a obtener el WebId del sitio true o false si se va a mandar en nulo</param>
    ''' <returns>Regresa un dataTable con Id y Nombre de archivo</returns>
    ''' <remarks></remarks>
    Public Function ConsultarDocumentosBiblioteca(ByVal pEnviarWebId As Boolean) As DataTable
        Dim dt As DataTable
        dt = New DataTable()
        dt.Columns.Add("IdArch")
        dt.Columns.Add("NombreArchivo")
        Dim NodoListaDocumentos As XmlNode = ConsultarDocumentos(pEnviarWebId)
        Dim xmlDoc As XmlDocument
        xmlDoc = New XmlDocument()
        xmlDoc.LoadXml(NodoListaDocumentos.OuterXml)
        Dim xmlNs As XmlNamespaceManager
        xmlNs = New XmlNamespaceManager(xmlDoc.NameTable)
        xmlNs.AddNamespace("z", "#RowsetSchema")
        Dim rows As XmlNodeList = xmlDoc.SelectNodes("//z:row", xmlNs)
        For Each row As XmlNode In rows
            Dim fila As DataRow = dt.NewRow()
            fila("IdArch") = row.Attributes("ows_ID").Value
            fila("NombreArchivo") = row.Attributes("ows_LinkFilename").Value
            dt.Rows.Add(fila)
        Next
        Return dt

    End Function


    ''' <summary>
    ''' Obtiene el Id asignado dentro de la biblioteca del sitio del servidor de sharepoint
    ''' </summary>
    ''' <returns>Regresa el Id valor tipo cadena</returns>
    ''' <remarks></remarks>
    Public Function ObtenerIdArchivo() As String
        Dim id As String = String.Empty


        Dim ListsSD As WSLists.Lists = New WSLists.Lists
        ListsSD.Url = ServidorSharePoint & "/_vti_bin/lists.asmx"
        ListsSD.Credentials = New System.Net.NetworkCredential(Usuario, Password, Dominio)
        ListsSD.PreAuthenticate = True

        Dim doc As New System.Xml.XmlDocument()
        doc.LoadXml("<Document><Query /><ViewFields /><QueryOptions /></Document>")

        Dim listQuery As System.Xml.XmlNode = doc.SelectSingleNode("//Query")

        listQuery.InnerXml = "<Where><Eq><FieldRef Name='FileLeafRef' /><Value Type='File'>" & NombreArchivo & "</Value></Eq></Where>"

        Dim listViewFields As System.Xml.XmlNode = doc.SelectSingleNode("//ViewFields")
        listViewFields.InnerXml = "<FieldRef Name='ID' />"
        Dim listQueryOptions As System.Xml.XmlNode = doc.SelectSingleNode("//QueryOptions")
        Dim g As Guid = GetWebID(ServidorSharePoint, Biblioteca, Usuario, Password, Dominio)
        Dim items As System.Xml.XmlNode = ListsSD.GetListItems(Biblioteca, String.Empty, listQuery, listViewFields, Me.RowLimit, listQueryOptions, Nothing)

        Dim xmlResultsDoc As New XmlDocument()

        xmlResultsDoc.LoadXml(items.OuterXml)
        Dim allListsDoc As New XmlDocument()
        Dim ns As New XmlNamespaceManager(allListsDoc.NameTable)
        ns = New XmlNamespaceManager(xmlResultsDoc.NameTable)

        ns.AddNamespace("z", "#RowsetSchema")

        Dim rows As XmlNodeList = xmlResultsDoc.SelectNodes("//z:row", ns)

        If rows.Count > 0 Then
            Return rows.Item(0).Attributes("ows_ID").Value()

        Else
            Console.WriteLine("No existen documentos")
            Return ""
        End If

    End Function


    Private Function GetWebID(ByVal webPath As String, ByVal biblioteca As String, ByVal usuario As String, ByVal passwd As String, ByVal dominio As String) As Guid

        Dim SiteDataSD As New WSSiteData.SiteData()

        SiteDataSD.UseDefaultCredentials = True
        SiteDataSD.Credentials = New System.Net.NetworkCredential(usuario, passwd, dominio)

        Dim webMetaData As New WSSiteData._sWebMetadata
        Dim arrWebWithTime As WSSiteData._sWebWithTime()
        Dim arrListWithTime As WSSiteData._sListWithTime()

        Dim arrUrls As WSSiteData._sFPUrl()

        Dim roles As String
        Dim roleUsers As String()
        Dim roleGroups As String()

        SiteDataSD.Url = webPath & "/" & biblioteca & "/_vti_bin/sitedata.asmx"

        Dim i As UInteger = SiteDataSD.GetWeb(webMetaData, arrWebWithTime, arrListWithTime, arrUrls, roles, roleUsers, roleGroups)

        Dim g As New Guid(webMetaData.WebID)

        Return g
    End Function


    ''' <summary>
    ''' Agrega, elimina o actualiza los elementos especificados en una biblioteca en el sitio del servidor de sharepoint
    ''' </summary>
    ''' <returns>Regresa el resultado de la ejecución</returns>
    ''' <remarks></remarks>

    Public Function ActualizarListItems() As String
        Dim resultado As String = String.Empty
        Try
            Dim lista As WSLists.Lists = InicializarLists()
            Dim xmlDoc = New XmlDocument()
            Dim xmlBatch As XmlElement = xmlDoc.CreateElement("Batch")
            xmlBatch.SetAttribute("OnError", "Continue")
            xmlBatch.SetAttribute("ListVersion", "1")
            xmlBatch.InnerXml = StrBatch
            Dim nodoResultado As XmlNode = lista.UpdateListItems(Biblioteca, xmlBatch)
            resultado = nodoResultado.OuterXml
        Catch es As SoapException
            Throw New SoapException("Ocurrió un error en la función ActualizarListItems de la clase FuncionesSharePoint.", es.Code, es.Actor, es.Role, es.Lang, es.Detail, es.SubCode, es)
        Catch ex As Exception
            Throw New Exception("Ocurrió un error en la función ActualizarListItems de la clase FuncionesSharePoint.", ex)
        End Try
        Return resultado
    End Function

    Private Function ObtenerRutaArchivo() As String
        Dim rutaOrigen As String = String.Empty
        Try
            Dim lista As WSLists.Lists = InicializarLists()
            'Crea Objeto del Documento XML.
            Dim XMLdoc As New XmlDocument()

            'Utilizamos el método CreateNode del Objeto del Documento XML para crear elementos de los parámetros que utiliza XML.
            Dim NodoQuery As XmlNode = XMLdoc.CreateNode(XmlNodeType.Element, "Query", "")
            Dim NodoViewFields As XmlNode = XMLdoc.CreateNode(XmlNodeType.Element, "ViewFields", "")
            Dim NodoQueryOptions As XmlNode = XMLdoc.CreateNode(XmlNodeType.Element, "QueryOptions", "")
            Dim guidSitio As Guid = ObtenerGUIDSitioSharePoint()
            Dim NodoListaDocumentos As XmlNode = lista.GetListItems(Biblioteca, String.Empty, NodoQuery, NodoViewFields, String.Empty, NodoQueryOptions, guidSitio.ToString())

            For Each nodo As XmlNode In NodoListaDocumentos
                If nodo.Name = "rs:data" Then
                    For count As Integer = 0 To nodo.ChildNodes.Count - 1
                        If Not nodo.ChildNodes(count).Attributes Is Nothing Then
                            If (nodo.ChildNodes(count).Name = "z:row" And nodo.ChildNodes(count).Attributes("ows_ContentType").Value.ToString() = "Documento") Then
                                If nodo.ChildNodes(count).Attributes("ows_LinkFilenameNoMenu").Value.ToString() = NombreArchivo Then
                                    rutaOrigen = nodo.ChildNodes(count).Attributes("ows_EncodedAbsUrl").Value.ToString()
                                    Exit For
                                End If
                            End If
                        End If

                    Next
                End If
            Next
        Catch es As SoapException
            Throw es
        Catch ex As Exception
            Throw ex
        End Try
        Return rutaOrigen
    End Function

    ''' <summary>
    ''' Permite visualizar o guardar un archivo de una biblioteca del servidor de sharepoint
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub VisualizarArchivo()
        Dim cliente As WebClient
        Dim binArchivo() As Byte
        Try
            Dim rutaOrigen As String = ObtenerRutaArchivo()
            cliente = New WebClient
            cliente.Credentials = New NetworkCredential(Usuario, Password, Dominio)
            Dim r As New System.Web.UI.Control
            binArchivo = cliente.DownloadData(r.ResolveUrl(rutaOrigen))

            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + NombreArchivo)
            HttpContext.Current.Response.ContentType = "text/ms-word"
            HttpContext.Current.Response.BinaryWrite(binArchivo)

        Catch es As SoapException
            Throw New SoapException("Ocurrió un error en la función VisualizarArchivo de la clase FuncionesSharePoint.", es.Code, es.Actor, es.Role, es.Lang, es.Detail, es.SubCode, es)
            Utilerias.ControlErrores.EscribirEvento("Ocurrió un error en la función VisualizarArchivo de la clase FuncionesSharePoint.", EventLogEntryType.Error, "SEPRIS", "")
        Catch ex As Exception
            Throw New Exception("Ocurrió un error en el método VisualizarArchivo de la clase FuncionesSharePoint.", ex)
            Utilerias.ControlErrores.EscribirEvento("Ocurrió un error en el método VisualizarArchivo de la clase FuncionesSharePoint.", EventLogEntryType.Error, "SEPRIS", "")
        End Try

    End Sub
    Public Function CargarDocumentoSepris() As Boolean

        Dim UploadedFilePath As String = RutaArchivo + NombreArchivo
        Dim SharePointPath As String = String.Format("{0}/{1}/{2}", ServidorSharePoint, Biblioteca, NombreArchivo)

        Dim response As WebResponse = Nothing
        'Create a PUT Web request to upload the file.
        Dim request As WebRequest = WebRequest.Create(SharePointPath)

        Try
            request.Credentials = New NetworkCredential(Usuario, Password, Dominio)
            request.Method = "PUT"

            Using stream As Stream = request.GetRequestStream()
                Dim i As Integer = Me.BinFile.Length
                stream.Write(Me.BinFile, 0, i)
            End Using

            ' Make the PUT request.
            response = request.GetResponse()
            Return True

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento("Error guardando el archivo WORD en SharePoint, " & ex.Message, EventLogEntryType.Error, "SEPRIS", "")
            Return False
        Finally
            If Not IsNothing(response) Then
                response.Close()
            End If
        End Try
    End Function
    Public Sub VisualizarArchivoSepris(psNomArchOri As String)
        Dim cliente As WebClientEx
        Dim binArchivo() As Byte
        'Dim rutaOrigen As String = ""
        Dim Url As String = String.Empty
        Try
            'rutaOrigen = ObtenerRutaArchivo()
            cliente = New WebClientEx
            cliente.Credentials = New NetworkCredential(Usuario, Password, Dominio)
            Dim r As New System.Web.UI.Control
            Url = Me.ServidorSharePoint & "/" & Me.Biblioteca & "/" & Me.NombreArchivo
            binArchivo = cliente.DownloadData(r.ResolveUrl(Url))

            'ProcessRequest(HttpContext.Current, Me.NombreArchivo, binArchivo)

            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ClearHeaders()
            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + psNomArchOri)
            HttpContext.Current.Response.ContentType = "text/ms-word"
            HttpContext.Current.Response.ContentType = "application/vnd.ms-word"
            HttpContext.Current.Response.BinaryWrite(binArchivo)
            HttpContext.Current.Response.Flush()
            HttpContext.Current.Response.End()

            'Dim _fileStream As New System.IO.FileStream(NombreArchivo, System.IO.FileMode.Create, System.IO.FileAccess.Write)
            '_fileStream.Write(binArchivo, 0, binArchivo.Length)
            'Utilerias.ControlErrores.EscribirEvento("TODO BIEN", EventLogEntryType.Error, "SEPRIS", "2")
            'Dim pathDocumento As String = _fileStream.Name.ToString
            '_fileStream.Close()
            'Process.Start(pathDocumento)            
        Catch ext As Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
        Catch ae As System.ArgumentException
            'Throw New System.ArgumentException("Ocurrio un error al recuperar el archivo de la ruta: " + Url, Url, ae)
            Utilerias.ControlErrores.EscribirEvento("Ocurrio un error al recuperar el archivo de la ruta: " + Url, EventLogEntryType.Error, "SEPRIS", "")
        Catch es As SoapException
            'Throw New SoapException("Ocurrió un error en la función VisualizarArchivo de la clase FuncionesSharePoint.", es.Code, es.Actor, es.Role, es.Lang, es.Detail, es.SubCode, es)
            Utilerias.ControlErrores.EscribirEvento("Ocurrió un error en la función VisualizarArchivo de la clase FuncionesSharePoint.", EventLogEntryType.Error, "SEPRIS", "")
        Catch ex As Exception
            'Throw New Exception("Ocurrió un error en el método VisualizarArchivo de la clase FuncionesSharePoint.", ex)
            Utilerias.ControlErrores.EscribirEvento(ex.Message & " - Ocurrió un error en el método VisualizarArchivo de la clase FuncionesSharePoint.", EventLogEntryType.Error, "SEPRIS", "")
        End Try
    End Sub
    Public Sub ProcessRequest(context As HttpContext, nombreArchivo As String, bytesFile() As Byte)
        'Dim mediaName As String = "myFile.zip"
        'If String.IsNullOrEmpty(mediaName) Then
        '    Return
        'End If
        'Dim destPath As String = context.Server.MapPath("~/" + nombreArchivo)
        Dim destPath As String = Path.GetTempPath & nombreArchivo
        Dim fi As New FileInfo(destPath)
        Try
            If fi.Exists Then
                File.Delete(destPath)
                'HttpContext.Current.Response.ClearHeaders()
                'HttpContext.Current.Response.ClearContent()
                'HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fi.Name)
                'HttpContext.Current.Response.AppendHeader("Content-Length", fi.Length.ToString())
                'HttpContext.Current.Response.ContentType = "application/octet-stream"
                'HttpContext.Current.Response.TransmitFile(fi.FullName)
                'HttpContext.Current.Response.Flush()
                Dim fs As FileStream
                fs = fi.Create()
                'Add some information to the file.
                fs.Write(bytesFile, 0, bytesFile.Length)
                fs.Close()
                Process.Start(destPath)
                'HttpContext.Current.Response.ClearHeaders()
                'HttpContext.Current.Response.ClearContent()
                'HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fi.Name)
                'HttpContext.Current.Response.AppendHeader("Content-Length", fi.Length.ToString())
                'HttpContext.Current.Response.ContentType = "application/octet-stream"
                'HttpContext.Current.Response.TransmitFile(fi.FullName)
                'HttpContext.Current.Response.Flush()
                'Else            
                '    HttpContext.Current.Response.ClearHeaders()
                '    HttpContext.Current.Response.ClearContent()
                '    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fi.Name)
                '    HttpContext.Current.Response.AppendHeader("Content-Length", fi.Length.ToString())
                '    HttpContext.Current.Response.ContentType = "application/octet-stream"
                '    HttpContext.Current.Response.TransmitFile(fi.FullName)
                '    HttpContext.Current.Response.Flush()
            End If
            'File.Delete(destPath)
        Catch ex As Exception
            EventLog.WriteEntry("ProcessRequest", ex.Message)
            HttpContext.Current.Response.ContentType = "text/plain"
            HttpContext.Current.Response.Write(ex.Message)
        Finally
            HttpContext.Current.Response.End()
        End Try
    End Sub

    Public Sub ConfigurarSharePointSepris(ByRef Shp As Utilerias.SharePointManager)
        Shp.ServidorSharePoint = WebConfigurationManager.AppSettings("SharePointServerSEPRIS").ToString()
        Shp.Usuario = WebConfigurationManager.AppSettings("SharePointUserSEPRIS").ToString()
        Shp.Password = Utilerias.Cifrado.DescifrarAES(WebConfigurationManager.AppSettings("SharePointPasswordSEPRIS").ToString())
        Shp.Dominio = WebConfigurationManager.AppSettings("SharePointDomainSEPRIS").ToString()
        Shp.Biblioteca = WebConfigurationManager.AppSettings("SharePointBibliotecaSEPRIS").ToString()
    End Sub

    Public Sub ConfigurarSharePointSeprisSicod(ByRef Shp As Utilerias.SharePointManager)
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

End Class
Public Class WebClientEx
    Inherits WebClient
    Protected Overrides Function GetWebRequest(address As Uri) As WebRequest
        Dim request = MyBase.GetWebRequest(address)
        request.Timeout = 7200000
        Return request
    End Function
End Class


'<SubsetCallableTypeAttribute> _
'Public Class SPWeb _
'	Inherits SPSecurableObject _
'	Implements IDisposable
'End Class