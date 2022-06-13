Imports LogicaNegocioSICOD
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports System.IO
Imports System.Web
Imports System.Net
Imports SICOD.Generales
Imports Clases

Public Class WebForm1
    Inherits System.Web.UI.Page

    Public Enum TIPO_DOCUMENTO
        OFICIO_EXTERNO = 1
        DICTAMEN = 2
        ATENTA_NOTA = 3
        OFICIO_INTERNO = 4
    End Enum

#Region "Propiedades de la página"

    'NHM INI
    Public Property ID_EXPEDIENTE_EXT() As Integer
        Get
            Return CInt(ViewState("ID_EXPEDIENTE_EXT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_EXPEDIENTE_EXT") = value
        End Set
    End Property
    'NHM FIN

    Public Property USUARIO() As String
        Get
            Return ViewState("Usuario").ToString
        End Get
        Set(ByVal value As String)
            ViewState("Usuario") = value
        End Set
    End Property

    Public Property ID_UNIDAD_ADM() As Integer
        Get
            Return CInt(ViewState("ID_UNIDAD_ADM"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_UNIDAD_ADM") = value
        End Set
    End Property

    Public Property ID_ANIO() As Integer
        Get
            Return CInt(ViewState("ID_ANIO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_ANIO") = value
        End Set
    End Property

    Public Property ID_TIPO_DOCUMENTO() As Integer
        Get
            Return CInt(ViewState("ID_TIPO_DOCUMENTO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_TIPO_DOCUMENTO") = value
        End Set
    End Property

    Public Property I_OFICIO_CONSECUTIVO() As Integer
        Get
            Return CInt(ViewState("I_OFICIO_CONSECUTIVO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("I_OFICIO_CONSECUTIVO") = value
        End Set
    End Property

    Public Property CODIGO_AREA As Integer
        Get
            Return CInt(ViewState("CODIGO_AREA"))
        End Get
        Set(ByVal value As Integer)
            ViewState("CODIGO_AREA") = value
        End Set
    End Property

    Public Property NUMERO_OFICIO As String
        Get
            Return ViewState("NUMERO_OFICIO").ToString
        End Get
        Set(ByVal value As String)
            ViewState("NUMERO_OFICIO") = value
        End Set
    End Property

    Public Property PUEDE_AGREGAR As Boolean
        Get
            Return CBool(ViewState("PUEDE_AGREGAR"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("PUEDE_AGREGAR") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            ID_UNIDAD_ADM = CInt(Session("ID_UNIDAD_ADM"))
            ID_ANIO = CInt(Session("ID_ANIO"))
            ID_TIPO_DOCUMENTO = CInt(Session("ID_TIPO_DOCUMENTO"))
            I_OFICIO_CONSECUTIVO = CInt(Session("I_OFICIO_CONSECUTIVO"))
            USUARIO = Session("Usuario").ToString
            CODIGO_AREA = CInt(Session("CODIGO_AREA"))
            NUMERO_OFICIO = Session("NUMERO_OFICIO").ToString
            txtFechaAcuse.Attributes.Add("readonly", "readonly")
            setTitle()
            ConsultaExisteArchivo()
            hideNonOficioExternoControls()
            hideNonDictamenControls()


            VerificaDocumentoConcluido()

            'NHM INI
            validarTipoDocumento(ID_TIPO_DOCUMENTO)
            'NHM FIN

        End If

    End Sub

    'NHM INI
    Private Sub validarTipoDocumento(ByVal idTipoDoc As Integer)

        If ID_TIPO_DOCUMENTO = TIPO_DOCUMENTO.DICTAMEN Then
            fileUpPDF.Enabled = False
            fileUpCNE.Enabled = False
            FileUpAnexo1.Enabled = False
            FileUpFirmaDigital.Enabled = False
            FileUpCedulaDigital.Enabled = False
            FileUpAnexo2.Enabled = False
            calFechaRecepcion.Enabled = False
            FileUpRespuesta.Enabled = False

        End If
    End Sub
    'NHM FIN

    Private Sub hideNonOficioExternoControls()
        If Not ID_TIPO_DOCUMENTO = TIPO_DOCUMENTO.OFICIO_EXTERNO Then

            '-------------------------------------
            ' Esconde Firma Digital
            '-------------------------------------
            lblDocFirmaDigital.Visible = False
            FileUpFirmaDigital.Visible = False
            btnDeleteFirmaDigital.Visible = False
            LinkDocFirmaDigital.Visible = False

            '-------------------------------------
            ' Esconde Cédula Digital
            '-------------------------------------
            lblDocCedulaDigital.Visible = False
            FileUpCedulaDigital.Visible = False
            btnDeleteCNE.Visible = False
            LinkDocCNE.Visible = False

            '-------------------------------------
            ' Esconde Cédula PDF
            '-------------------------------------
            lblDocCedulaPDF.Visible = False
            fileUpCNE.Visible = False
            btnDeleteCNE.Visible = False
            LinkDocCNE.Visible = False
        End If
    End Sub

    Private Sub hideNonDictamenControls()
        If Not ID_TIPO_DOCUMENTO = TIPO_DOCUMENTO.DICTAMEN Then
            '-------------------------------------
            ' Esconde Expediente
            '-------------------------------------
            lblDocExpediente.Visible = False
            FileUpExpediente.Visible = False
            btnDeleteExpediente.Visible = False
            LinkDocExpediente.Visible = False
        End If
    End Sub

    Private Sub setTitle()

        PUEDE_AGREGAR = verificaUsuario()

        If Not PUEDE_AGREGAR Then
            lblTitulo.Text = "Archivos de Sólo Lectura para Oficio <strong>" & NUMERO_OFICIO & "</strong>"
        Else
            lblTitulo.Text = "Adjuntar Documentos a <strong>" & NUMERO_OFICIO & "</strong>"
        End If
    End Sub

    ''' <summary>
    '''  Consulta si existe el documento y valida si es un archivo de sharepoint o anterior, si es anterior no permite que lo eliminen
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ConsultaExisteArchivo()

        Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
        Dim ExtensionOriginal As String = ""

        '-----------------------------------------------
        ' Archivo Word
        '-----------------------------------------------
        If (BusinessRules.BDA_OFICIO.ConsultaDocumentoWord(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) IsNot String.Empty) Then
            Dim archivo As String = BusinessRules.BDA_OFICIO.ConsultaDocumentoWord(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            LinkDocWord.CommandName = archivo

            If archivo.Contains("#") Then
                archivo = archivo.Substring(0, archivo.IndexOf("#"))
            ElseIf archivo.Contains("@") Then
                archivo = archivo.Substring(0, archivo.IndexOf("@"))
            End If

            LinkDocWord.Text = archivo
            LinkDocWord.Visible = True
            btnDeleteWord.Visible = True
            fileUpWord.Visible = False

            If (archivoSharepoint(archivo) = False) Then
                btnDeleteWord.Enabled = False
            End If

            If Not PUEDE_AGREGAR Then btnDeleteWord.Visible = False

        ElseIf Not PUEDE_AGREGAR Then
            fileUpWord.Enabled = False
        End If

        '-----------------------------------------------
        ' Archivo PDF
        '-----------------------------------------------
        If (BusinessRules.BDA_OFICIO.ConsultaDocumentoPDF(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) IsNot String.Empty) Then
            Dim archivo As String = BusinessRules.BDA_OFICIO.ConsultaDocumentoPDF(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            LinkDocPDF.CommandName = archivo

            If archivo.Contains("#") Then
                archivo = archivo.Substring(0, archivo.IndexOf("#"))
            ElseIf archivo.Contains("@") Then
                archivo = archivo.Substring(0, archivo.IndexOf("@"))
            End If

            LinkDocPDF.Text = archivo
            LinkDocPDF.Visible = True
            btnDeletePDF.Visible = True
            fileUpPDF.Visible = False

            If (archivoSharepoint(archivo) = False) Then
                btnDeletePDF.Enabled = False
            End If

            If Not PUEDE_AGREGAR Then btnDeletePDF.Visible = False

        ElseIf Not PUEDE_AGREGAR Then
            fileUpPDF.Enabled = False
        End If

        '-----------------------------------------------
        ' Cédula
        '-----------------------------------------------
        If (BusinessRules.BDA_OFICIO.ConsultaDocCedulaPDF(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) IsNot String.Empty) Then
            Dim archivo As String = BusinessRules.BDA_OFICIO.ConsultaDocCedulaPDF(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            LinkDocCNE.CommandName = archivo

            If archivo.Contains("#") Then
                archivo = archivo.Substring(0, archivo.IndexOf("#"))
            ElseIf archivo.Contains("@") Then
                archivo = archivo.Substring(0, archivo.IndexOf("@"))
            End If

            LinkDocCNE.Text = archivo
            LinkDocCNE.Visible = True
            btnDeleteCNE.Visible = True
            fileUpCNE.Visible = False

            If (archivoSharepoint(archivo) = False) Then btnDeleteCNE.Enabled = False

            If Not PUEDE_AGREGAR Then btnDeleteCNE.Visible = False

        ElseIf Not PUEDE_AGREGAR Then
            fileUpCNE.Enabled = False
        End If

        '-----------------------------------------------
        ' Anexo 1
        '-----------------------------------------------
        If (BusinessRules.BDA_OFICIO.ConsultaDocAnexo1(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) IsNot String.Empty) Then
            Dim archivo As String = BusinessRules.BDA_OFICIO.ConsultaDocAnexo1(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            LinkAnexo1.CommandName = archivo

            If archivo.Contains("#") Then
                archivo = archivo.Substring(0, archivo.IndexOf("#"))
            ElseIf archivo.Contains("@") Then
                archivo = archivo.Substring(0, archivo.IndexOf("@"))
            End If

            LinkAnexo1.Text = archivo
            LinkAnexo1.Visible = True
            btnDeleteAnexo1.Visible = True
            FileUpAnexo1.Visible = False

            If (archivoSharepoint(archivo) = False) Then btnDeleteAnexo1.Enabled = False

            If Not PUEDE_AGREGAR Then btnDeleteAnexo1.Visible = False

        ElseIf Not PUEDE_AGREGAR Then
            FileUpAnexo1.Enabled = False

        End If

        '-----------------------------------------------
        ' Anexo 2
        '-----------------------------------------------
        If Not onReglasDeNegocioOK("T_ANEXO_DOS") Then
            FileUpAnexo2.Enabled = False

        Else

            If (BusinessRules.BDA_OFICIO.ConsultaDocAnexo2(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) IsNot String.Empty) Then

                Dim archivo As String = BusinessRules.BDA_OFICIO.ConsultaDocAnexo2(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

                If archivo.Equals("0") Then
                    ExtensionOriginal = IO.Path.GetExtension(_dt.Rows(0).Item("T_ANEXO_UNO").ToString)
                    Dim result As String = revisarSBM("T_ANEXO_UNO", ExtensionOriginal)
                    If Not result.Equals(String.Empty) Then
                        LinkAnexo2.Text = result
                        LinkAnexo2.Visible = True
                        btnDeleteAnexo2.Visible = True
                        FileUpAnexo2.Visible = False

                    End If
                Else

                    If archivo.Contains("#") Then
                        archivo = archivo.Substring(0, archivo.IndexOf("#"))
                    Else


                        If archivo.Contains("@") Then
                            archivo = archivo.Substring(archivo.IndexOf("@") + 1, archivo.Length - (archivo.IndexOf("@") + 1))
                        End If

                        LinkAnexo2.CommandName = archivo

                        LinkAnexo2.Text = archivo
                        LinkAnexo2.Visible = True
                        btnDeleteAnexo2.Visible = True
                        FileUpAnexo2.Visible = False

                        If (archivoSharepoint(archivo) = False) Then btnDeleteAnexo2.Enabled = False

                        If Not PUEDE_AGREGAR Then btnDeleteAnexo2.Visible = False

                        'End If

                    End If


                End If





            ElseIf Not PUEDE_AGREGAR Then
                FileUpAnexo2.Enabled = False
            End If

        End If



        '-----------------------------------------------
        ' Firma SBM
        '-----------------------------------------------
        If Not onReglasDeNegocioOK("T_HYP_FIRMADIGITAL") Then
            FileUpFirmaDigital.Enabled = False

        Else

            If (BusinessRules.BDA_OFICIO.ConsultaDocFirmaDigital(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) IsNot String.Empty) Then


                Dim archivo As String = BusinessRules.BDA_OFICIO.ConsultaDocFirmaDigital(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

                If archivo.Equals("0") Then
                    ExtensionOriginal = IO.Path.GetExtension(_dt.Rows(0).Item("T_HYP_ARCHIVOSCAN").ToString)
                    Dim result As String = revisarSBM("T_HYP_FIRMADIGITAL", ExtensionOriginal)
                    If Not result.Equals(String.Empty) Then
                        LinkDocFirmaDigital.Text = result
                        LinkDocFirmaDigital.Visible = True
                        btnDeleteFirmaDigital.Visible = True
                        FileUpFirmaDigital.Visible = False

                    End If
                Else

                    LinkDocFirmaDigital.CommandName = archivo

                    If archivo.Contains("#") Then
                        archivo = archivo.Substring(0, archivo.IndexOf("#"))
                    ElseIf archivo.Contains("@") Then
                        archivo = archivo.Substring(0, archivo.IndexOf("@"))
                    End If

                    LinkDocFirmaDigital.Text = archivo
                    LinkDocFirmaDigital.Visible = True
                    btnDeleteFirmaDigital.Visible = True
                    FileUpFirmaDigital.Visible = False

                    If (archivoSharepoint(archivo) = False) Then
                        btnDeleteFirmaDigital.Enabled = False
                    End If
                End If


                If Not PUEDE_AGREGAR Then btnDeleteFirmaDigital.Visible = False



            ElseIf Not PUEDE_AGREGAR Then
                FileUpFirmaDigital.Enabled = False
            End If

        End If


        '-----------------------------------------------
        ' Cédula SBM
        '-----------------------------------------------
        If Not onReglasDeNegocioOK("T_CEDULADIGITAL") Then

            FileUpCedulaDigital.Enabled = False

        Else

            If BusinessRules.BDA_OFICIO.ConsultaDocCedulaDigital(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) IsNot String.Empty Then



                Dim archivo As String = BusinessRules.BDA_OFICIO.ConsultaDocCedulaDigital(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

                If archivo.Equals("0") Then
                    ExtensionOriginal = IO.Path.GetExtension(_dt.Rows(0).Item("T_HYP_CEDULAPDF").ToString)
                    Dim result As String = revisarSBM("T_CEDULADIGITAL", ExtensionOriginal)
                    If Not result.Equals(String.Empty) Then

                        LinkDocCedulaDigital.Text = result
                        LinkDocCedulaDigital.Visible = True
                        btnDeleteCedulaDigital.Visible = True
                        FileUpCedulaDigital.Visible = False

                    End If
                Else

                    LinkDocCedulaDigital.CommandName = archivo

                    If archivo.Contains("#") Then
                        archivo = archivo.Substring(0, archivo.IndexOf("#"))
                    ElseIf archivo.Contains("@") Then
                        archivo = archivo.Substring(0, archivo.IndexOf("@"))
                    End If

                    LinkDocCedulaDigital.Text = archivo
                    LinkDocCedulaDigital.Visible = True
                    btnDeleteCedulaDigital.Visible = True
                    FileUpCedulaDigital.Visible = False

                    If (archivoSharepoint(archivo) = False) Then btnDeleteCedulaDigital.Enabled = False

                    If Not PUEDE_AGREGAR Then btnDeleteCedulaDigital.Visible = False

                End If



            ElseIf Not PUEDE_AGREGAR Then
                FileUpCedulaDigital.Enabled = False
            End If

        End If


        '-----------------------------------------------
        ' Acuse
        '-----------------------------------------------
        If (BusinessRules.BDA_OFICIO.ConsultaDocAcuseRespuesta(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) IsNot String.Empty) Then
            Dim archivo As String = BusinessRules.BDA_OFICIO.ConsultaDocAcuseRespuesta(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            LinkDocAcuse.CommandName = archivo

            If archivo.Contains("#") Then
                archivo = archivo.Substring(0, archivo.IndexOf("#"))
            ElseIf archivo.Contains("@") Then
                archivo = archivo.Substring(0, archivo.IndexOf("@"))
            End If

            LinkDocAcuse.Text() = archivo
            LinkDocAcuse.Visible = True
            btnDeleteAcuse.Visible = True
            txtFechaAcuse.Visible = False
            imgCalFechaAcuse.Visible = False

            FileUpAcuse.Visible = False

            If (archivoSharepoint(archivo) = False) Then btnDeleteAcuse.Enabled = False

            If Not PUEDE_AGREGAR Then btnDeleteAcuse.Visible = False

        ElseIf Not PUEDE_AGREGAR Then
            FileUpAcuse.Enabled = False
            txtFechaAcuse.Style.Add("display", "none")
            imgCalFechaAcuse.Style.Add("display", "none")
        End If

        '-----------------------------------------------
        ' Respuesta
        '-----------------------------------------------
        If (BusinessRules.BDA_OFICIO.ConsultaDocRespuestaOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) IsNot String.Empty) Then
            Dim archivo As String = BusinessRules.BDA_OFICIO.ConsultaDocRespuestaOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            LinkDocRespuesta.CommandName = archivo

            If archivo.Contains("#") Then
                archivo = archivo.Substring(0, archivo.IndexOf("#"))
            ElseIf archivo.Contains("@") Then
                archivo = archivo.Substring(0, archivo.IndexOf("@"))
            End If

            LinkDocRespuesta.Text() = archivo
            LinkDocRespuesta.Visible = True
            btnDeleteRespuesta.Visible = True
            FileUpRespuesta.Visible = False

            If (archivoSharepoint(archivo) = False) Then btnDeleteRespuesta.Enabled = False

            If Not PUEDE_AGREGAR Then btnDeleteRespuesta.Visible = False

        ElseIf Not PUEDE_AGREGAR Then
            FileUpRespuesta.Enabled = False
        End If

        '-----------------------------------------------
        ' Expediente
        '-----------------------------------------------
        If (BusinessRules.BDA_OFICIO.ConsultaDocExpediente(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO) IsNot String.Empty) Then
            Dim archivo As String = BusinessRules.BDA_OFICIO.ConsultaDocExpediente(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            LinkDocExpediente.CommandName = archivo

            If archivo.Contains("#") Then
                archivo = archivo.Substring(0, archivo.IndexOf("#"))
            ElseIf archivo.Contains("@") Then
                archivo = archivo.Substring(0, archivo.IndexOf("@"))
            End If

            LinkDocExpediente.Text() = archivo
            LinkDocExpediente.Visible = True
            btnDeleteExpediente.Visible = True
            FileUpExpediente.Visible = False

            If (archivoSharepoint(archivo) = False) Then btnDeleteAcuse.Enabled = False

            If Not PUEDE_AGREGAR Then btnDeleteAcuse.Visible = False

        ElseIf Not PUEDE_AGREGAR Then
            FileUpExpediente.Enabled = False
        End If

    End Sub

    ''' <summary>
    ''' Valida cada fileupload que el documento sea de la extensión que necesita
    ''' </summary>
    ''' <param name="controlFileUpLoad"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function validatipoDocumentoWord(ByRef controlFileUpLoad) As Boolean
        Dim fileExt As String = String.Empty
        fileExt = System.IO.Path.GetExtension(controlFileUpLoad.FileName)
        Dim archivoValido As Boolean = False

        'valida que el tipo de documento sea  word
        If (fileExt = ".doc") Or (fileExt = ".docx") Then

            archivoValido = True

        End If

        Return archivoValido

    End Function


    ''' <summary>
    ''' Valida que el tipo de documento sea pdf
    ''' </summary>
    ''' <param name="controlFileUpLoad"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function validatipoDocumentoPDF(ByRef controlFileUpLoad) As Boolean

        Dim fileExt As String = String.Empty
        Dim archivoValido As Boolean = False
        fileExt = System.IO.Path.GetExtension(controlFileUpLoad.FileName)
        'valida que el documento sea de tipo PDF

        If (fileExt = ".pdf") Then
            archivoValido = True
        End If

        Return archivoValido
    End Function

    'NHM INI
    ''' <summary>
    ''' Valida que el tipo de documento sea pdf
    ''' </summary>
    ''' <param name="controlFileUpLoad"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function validatipoDocumentoPDF_ZIP(ByRef controlFileUpLoad) As Boolean

        Dim fileExt As String = String.Empty
        Dim archivoValido As Boolean = False
        fileExt = System.IO.Path.GetExtension(controlFileUpLoad.FileName)
        'valida que el documento sea de tipo PDF o ZIP
        
        If (fileExt.ToLower() = ".pdf") Or (fileExt.ToLower() = ".zip") Then

            archivoValido = True

        End If

        Return archivoValido
    End Function
    'NHM FIN

    ''' <summary>
    ''' Valida que el tipo de documento sea sbm
    ''' </summary>
    ''' <param name="controlFileUpLoad"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function validatipoDocumentoSBM(ByRef controlFileUpLoad) As Boolean
        Dim fileExt As String = String.Empty
        fileExt = System.IO.Path.GetExtension(controlFileUpLoad.FileName)
        Dim archivoValido As Boolean = False
        'valida que el documento sea de tipo sbm
        If fileExt.Contains(".sbm") Then
            archivoValido = True

        End If
        Return archivoValido

    End Function

    ''' <summary>
    ''' Valida que el nombre de archivo tenga extensión.
    ''' Esta es la validación para cualquier tipo de archivo.
    ''' </summary>
    ''' <param name="controlFileUpLoad"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function validatipoDocumentoGenerico(ByRef controlFileUpLoad) As Boolean
        Dim fileExt As String = String.Empty
        fileExt = System.IO.Path.GetExtension(controlFileUpLoad.FileName)
        Dim archivoValido As Boolean = False
        'valida que el tenga extensión
        If (fileExt <> String.Empty) Then
            archivoValido = True

        End If

        Return archivoValido
    End Function

    ''' <summary>
    ''' Valida que el tipo de documento sea xlsx 
    ''' </summary>
    ''' <param name="controlFileUpLoad"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function validatipoDocumentoXls(ByRef controlFileUpLoad) As Boolean
        Dim fileExt As String = String.Empty
        fileExt = System.IO.Path.GetExtension(controlFileUpLoad.FileName)
        Dim archivoValido As Boolean = False

        If (fileExt = ".xlsx") Then
            archivoValido = True

        End If

        Return archivoValido
    End Function

    Private Function GenerarNombreSharepoint(ByVal controlFileUpload As FileUpload, ByVal tipoArchivo As String, Optional ByVal ExtensionOriginalParaSBMX As String = "") As String
        Dim NombreSharepoint As String = String.Empty
        Dim FilePath As String = String.Empty
        Dim tipoDocumentoShort As String = String.Empty

        Try
            '---------------------------------------------
            ' Verificar que exista documento en el FileUpload
            '---------------------------------------------
            If (controlFileUpload.FileName <> String.Empty) Then


                If Not IO.Path.GetExtension(controlFileUpload.FileName).ToUpper = ".SBMX" Then
                    ExtensionOriginalParaSBMX = ""
                End If

                '----------------------------------------------------
                ' Seleccionar el prefijo del tipo de documento.
                '----------------------------------------------------
                Select Case ID_TIPO_DOCUMENTO
                    Case TIPO_DOCUMENTO.OFICIO_EXTERNO
                        tipoDocumentoShort = "EX"
                    Case TIPO_DOCUMENTO.OFICIO_INTERNO
                        tipoDocumentoShort = "IN"
                    Case TIPO_DOCUMENTO.ATENTA_NOTA
                        tipoDocumentoShort = "AN"
                    Case TIPO_DOCUMENTO.DICTAMEN
                        tipoDocumentoShort = "DI"

                End Select

                NombreSharepoint =
                                    tipoDocumentoShort & "_" & _
                                    Format(CODIGO_AREA, "000").ToString + "_" & _
                                    Format(I_OFICIO_CONSECUTIVO, "0000").ToString() & "_" & _
                                    ID_ANIO.ToString

                Select Case tipoArchivo
                    Case "T_HYP_ARCHIVOWORD"
                        NombreSharepoint = "WRD_" & NombreSharepoint & Path.GetExtension(controlFileUpload.FileName)
                    Case "T_HYP_ARCHIVOSCAN"
                        NombreSharepoint = "PDF_" & NombreSharepoint & Path.GetExtension(controlFileUpload.FileName)
                    Case "T_CEDULADIGITAL"
                        NombreSharepoint = "CNE_" & NombreSharepoint & ExtensionOriginalParaSBMX & Path.GetExtension(controlFileUpload.FileName)
                    Case "T_HYP_CEDULAPDF"
                        NombreSharepoint = "CNE_" & NombreSharepoint & Path.GetExtension(controlFileUpload.FileName)
                    Case "T_HYP_FIRMADIGITAL"
                        NombreSharepoint = "PDF_" & NombreSharepoint & ExtensionOriginalParaSBMX & Path.GetExtension(controlFileUpload.FileName)
                    Case "T_HYP_RESPUESTAOFICIO"
                        NombreSharepoint = "RES_" & NombreSharepoint & Path.GetExtension(controlFileUpload.FileName)
                    Case "T_HYP_ACUSERESPUESTA"
                        NombreSharepoint = "ACU_" & NombreSharepoint & Path.GetExtension(controlFileUpload.FileName)
                    Case "T_ANEXO_UNO"
                        NombreSharepoint = "AX1_" & NombreSharepoint & ExtensionOriginalParaSBMX & Path.GetExtension(controlFileUpload.FileName)
                    Case "T_ANEXO_DOS"
                        NombreSharepoint = "AX1_" & NombreSharepoint & ExtensionOriginalParaSBMX & Path.GetExtension(controlFileUpload.FileName)
                    Case "T_HYP_EXPEDIENTE"
                        NombreSharepoint = "EXP_" & NombreSharepoint & Path.GetExtension(controlFileUpload.FileName)
                End Select

                '---------------------------------------------
                'Esta ruta me la da el usuario por que es donde se va a guardar el archivo
                '---------------------------------------------
                FilePath = System.IO.Path.GetTempPath() + NombreSharepoint
                controlFileUpload.SaveAs(FilePath)

                Dim enc As New YourCompany.Utils.Encryption.Encryption64
                Dim objSP As New Clases.nsSharePoint.FuncionesSharePoint

                objSP.ServidorSharePoint = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("SharePointServerOficios"), "webCONSAR")
                objSP.Biblioteca = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("DocLibraryOficios"), "webCONSAR")
                objSP.Usuario = WebConfigurationManager.AppSettings("UsuarioSp").ToString()
                objSP.Password = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR")
                objSP.Dominio = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("Domain"), "webCONSAR")
                objSP.RutaArchivo = System.IO.Path.GetTempPath()
                objSP.NombreArchivo = NombreSharepoint

                If Not objSP.UploadFileToSharePoint() Then Throw New ApplicationException("Error Cargando Archivo a Sharepoint")


            End If

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
            NombreSharepoint = String.Empty
        Catch ex As Exception
            NombreSharepoint = String.Empty
        Finally

            If System.IO.File.Exists(FilePath) Then System.IO.File.Delete(FilePath)

        End Try

        Return NombreSharepoint
    End Function

    Private Function crearNombreArchivo(ByVal nombreArchivo As String, ByVal tipoArchivo As String, Optional ByVal ExtensionOriginalParaSBMX As String = "") As String
        Dim tipoDocumentoShort As String = String.Empty

        '----------------------------------------------------
        ' Seleccionar el prefijo del tipo de documento.
        '----------------------------------------------------
        Select Case ID_TIPO_DOCUMENTO
            Case TIPO_DOCUMENTO.OFICIO_EXTERNO
                tipoDocumentoShort = "EX"
            Case TIPO_DOCUMENTO.OFICIO_INTERNO
                tipoDocumentoShort = "IN"
            Case TIPO_DOCUMENTO.ATENTA_NOTA
                tipoDocumentoShort = "AN"
            Case TIPO_DOCUMENTO.DICTAMEN
                tipoDocumentoShort = "DI"
        End Select

        Dim fileName As String =
                                tipoDocumentoShort & "_" & _
                                Format(CODIGO_AREA, "000").ToString + "_" & _
                                Format(I_OFICIO_CONSECUTIVO, "0000").ToString() & "_" & _
                                ID_ANIO.ToString

        Select Case tipoArchivo
            Case "T_HYP_ARCHIVOWORD"
                fileName = "WRD_" & fileName & Path.GetExtension(nombreArchivo)
            Case "T_HYP_ARCHIVOSCAN"
                fileName = "PDF_" & fileName & Path.GetExtension(nombreArchivo)
            Case "T_CEDULADIGITAL"
                fileName = "CNE_" & fileName & ExtensionOriginalParaSBMX & Path.GetExtension(nombreArchivo)
            Case "T_HYP_CEDULAPDF"
                fileName = "CNE_" & fileName & Path.GetExtension(nombreArchivo)
            Case "T_HYP_FIRMADIGITAL"
                fileName = "PDF_" & fileName & ExtensionOriginalParaSBMX & Path.GetExtension(nombreArchivo)
            Case "T_HYP_RESPUESTAOFICIO"
                fileName = "RES_" & fileName & Path.GetExtension(nombreArchivo)
            Case "T_HYP_ACUSERESPUESTA"
                fileName = "ACU_" & fileName & Path.GetExtension(nombreArchivo)
            Case "T_ANEXO_UNO"
                fileName = "AX1_" & fileName & ExtensionOriginalParaSBMX & Path.GetExtension(nombreArchivo)
            Case "T_ANEXO_DOS"
                fileName = "AX2_" & fileName & ExtensionOriginalParaSBMX & Path.GetExtension(nombreArchivo)
            Case "T_HYP_EXPEDIENTE"
                fileName = "EXP_" & fileName & Path.GetExtension(nombreArchivo)
        End Select

        Return fileName

    End Function

    Private Function AdjuntarDocumentos() As Boolean
        Dim objOficio As LogicaNegocioSICOD.Entities.BDA_OFICIO
        objOficio = New LogicaNegocioSICOD.Entities.BDA_OFICIO

        Dim archivoValido As Boolean = True

        lblErroresPopup.Text = String.Empty
        Try
            '---------------------------------------
            ' asigna valores a las variables para poder guardar
            '---------------------------------------


            Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            Dim fechaVencimientoValidacion As String = "NULL"
            If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
            End If

            objOficio.IdAnio = ID_ANIO
            objOficio.IdArea = ID_UNIDAD_ADM
            objOficio.IdTipoDocumento = ID_TIPO_DOCUMENTO
            objOficio.IOficioConsecutivo = I_OFICIO_CONSECUTIVO
            objOficio.UsuarioElaboro = USUARIO
            objOficio.Comentario = fechaVencimientoValidacion

            If fileUpWord.FileName <> String.Empty AndAlso btnDeleteWord.Visible = False Then

                '---------------------------------------
                ' Archivo de Trabajo (word)
                '---------------------------------------
                If validatipoDocumentoWord(fileUpWord) Then
                    objOficio.ArchivoWord = GenerarNombreSharepoint(fileUpWord, "T_HYP_ARCHIVOWORD")

                    Dim stream As Stream = fileUpWord.PostedFile.InputStream
                    Dim binFile(stream.Length) As Byte

                    If objOficio.ArchivoWord = String.Empty Then
                        Throw New ApplicationException("Error guardando el archivo " & fileUpWord.FileName & " en SharePoint")
                    Else
                        BusinessRules.BDA_OFICIO.ActualizarArchivoWord(objOficio)
                    End If
                Else

                    Throw New ApplicationException("El archivo " & fileUpWord.FileName & " no es un tipo de archivo valido para el Archivo Word")
                End If

            End If

            '---------------------------------------
            ' Archivo PDF del documento
            '---------------------------------------
            If fileUpPDF.FileName <> String.Empty AndAlso btnDeletePDF.Visible = False Then

                If validatipoDocumentoPDF(fileUpPDF) Then
                    objOficio.ArchivoPDF = GenerarNombreSharepoint(fileUpPDF, "T_HYP_ARCHIVOSCAN")
                    If objOficio.ArchivoPDF = String.Empty Then
                        Throw New ApplicationException("Error guardando el archivo " & fileUpPDF.FileName & " en SharePoint")
                    Else
                        BusinessRules.BDA_OFICIO.ActualizarArchivoPDF(objOficio)
                    End If
                Else
                    Throw New ApplicationException("El archivo " & fileUpPDF.FileName & " no es un tipo de archivo valido para el Archivo PDF")
                End If

            End If

            '---------------------------------------
            ' Archivo PDF de la cédula
            '---------------------------------------
            If (fileUpCNE.FileName <> String.Empty) And (btnDeleteCNE.Visible = False) Then
                If validatipoDocumentoPDF(fileUpCNE) Then
                    objOficio.ArchivoCedulaPDF = GenerarNombreSharepoint(fileUpCNE, "T_HYP_CEDULAPDF")
                    If objOficio.ArchivoCedulaPDF = String.Empty Then
                        Throw New ApplicationException("Error guardando el archivo " & fileUpCNE.FileName & " en SharePoint")
                    Else
                        BusinessRules.BDA_OFICIO.ActualizarArchivoCedulaPDF(objOficio)

                    End If
                Else

                    Throw New ApplicationException("El archivo " & fileUpCNE.FileName & " no es un tipo de archivo valido para la Cédula")
                End If

            End If

            '---------------------------------------
            ' Adjuntar Anexo 1
            '---------------------------------------
            If (FileUpAnexo1.FileName <> String.Empty) And (btnDeleteAnexo1.Visible = False) Then

                If (validatipoDocumentoGenerico(FileUpAnexo1)) Then
                    objOficio.ArchivoAnexo1 = GenerarNombreSharepoint(FileUpAnexo1, "T_ANEXO_UNO")
                    If objOficio.ArchivoAnexo1 = String.Empty Then
                        Throw New ApplicationException("Error guardando el archivo " & FileUpAnexo1.FileName & " en SharePoint")
                    Else
                        BusinessRules.BDA_OFICIO.ActualizarArchivoAnexo1(objOficio)
                    End If
                Else
                    Throw New ApplicationException("El archivo " & FileUpAnexo1.FileName & " no es un tipo de archivo valido para el Anexo 1")
                End If
            End If

            '---------------------------------------
            ' Adjuntar Anexo 2
            '---------------------------------------
            If (FileUpAnexo2.FileName <> String.Empty) And (btnDeleteAnexo2.Visible = False) Then
                If (validatipoDocumentoSBM(FileUpAnexo2)) Then
                    'objOficio.ArchivoAnexo2 = objOficio.ArchivoAnexo1 & "@" & GenerarNombreSharepoint(FileUpAnexo2, "T_ANEXO_DOS")
                    objOficio.ArchivoAnexo2 = GenerarNombreSharepoint(FileUpAnexo2, "T_ANEXO_UNO", IO.Path.GetExtension(_dt.Rows(0).Item("T_ANEXO_UNO").ToString))
                    If objOficio.ArchivoAnexo2 = String.Empty Then
                        Throw New ApplicationException("Error guardando el archivo " & FileUpAnexo2.FileName & " en SharePoint")
                    Else
                        BusinessRules.BDA_OFICIO.ActualizarArchivoAnexo2(objOficio, True)
                    End If
                Else
                    Throw New ApplicationException("El archivo " & FileUpAnexo2.FileName & " no es un tipo de archivo valido para el Anexo SBM")
                End If
            End If

            '---------------------------------------
            ' Oficio con firma digital
            '---------------------------------------
            If (FileUpFirmaDigital.FileName <> String.Empty) And (btnDeleteFirmaDigital.Visible = False) Then

                If (validatipoDocumentoSBM(FileUpFirmaDigital)) Then
                    objOficio.ArchivoFirmaDigital = GenerarNombreSharepoint(FileUpFirmaDigital, "T_HYP_FIRMADIGITAL", IO.Path.GetExtension(_dt.Rows(0).Item("T_HYP_ARCHIVOSCAN").ToString))
                    If objOficio.ArchivoFirmaDigital = String.Empty Then
                        Throw New ApplicationException("Error guardando el archivo " & FileUpFirmaDigital.FileName & " en SharePoint")
                    Else
                        BusinessRules.BDA_OFICIO.ActualizarArchivoFirmaDigital(objOficio)

                    End If
                Else
                    Throw New ApplicationException("El archivo " & FileUpFirmaDigital.FileName & " no es un tipo de archivo para la Firma Digital")
                End If

            End If

            '---------------------------------------
            ' Cédula Digital
            '---------------------------------------
            If (FileUpCedulaDigital.FileName <> String.Empty) And (btnDeleteCedulaDigital.Visible = False) Then
                If (validatipoDocumentoSBM(FileUpCedulaDigital)) Then
                    objOficio.ArchivoCedulaDigital = GenerarNombreSharepoint(FileUpCedulaDigital, "T_CEDULADIGITAL", IO.Path.GetExtension(_dt.Rows(0).Item("T_HYP_CEDULAPDF").ToString))
                    If objOficio.ArchivoCedulaDigital = String.Empty Then
                        Throw New ApplicationException("Error guardando el archivo " & FileUpCedulaDigital.FileName & " en SharePoint")
                    Else
                        BusinessRules.BDA_OFICIO.ActualizarArchivoCedulaDigital(objOficio)
                    End If
                Else
                    Throw New ApplicationException("El archivo " & FileUpCedulaDigital.FileName & " no es un tipo de archivo valido para la Cédula Electrónica")
                End If

            End If

            '---------------------------------------
            ' Acuse del documento
            '---------------------------------------
            If (FileUpAcuse.FileName <> String.Empty) And (btnDeleteAcuse.Visible = False) Then
                If (validatipoDocumentoPDF(FileUpAcuse)) Then

                    objOficio.ArchivoAcuse = GenerarNombreSharepoint(FileUpAcuse, "T_HYP_ACUSERESPUESTA")
                    If objOficio.ArchivoAcuse = String.Empty Then
                        Throw New ApplicationException("Error guardando el archivo " & FileUpAcuse.FileName & " en SharePoint")
                    Else
                        If Not txtFechaAcuse.Text.Trim = String.Empty Then
                            objOficio.FechaAcuse = CType(txtFechaAcuse.Text, DateTime)
                            If BusinessRules.BDA_OFICIO.ActualizarFechaAcuse(objOficio) Then
                                BusinessRules.BDA_OFICIO.ActualizarArchivoAcuse(objOficio)
                            Else
                                Throw New ApplicationException("Error actualizando fecha del acuse, intente de nuevo")

                            End If
                        Else
                            Throw New ApplicationException("Debe agregar fecha del acuse")
                        End If
                    End If
                Else
                    Throw New ApplicationException("El archivo" & FileUpAcuse.FileName & " no es un tipo de archivo valido para el Acuse")
                End If
            End If

            '---------------------------------------
            ' Respuesta electrónica
            '---------------------------------------
            If (FileUpRespuesta.FileName <> String.Empty) And (btnDeleteRespuesta.Visible = False) Then

                If (validatipoDocumentoPDF(FileUpRespuesta)) Then
                    objOficio.ArchivoRespuesta = GenerarNombreSharepoint(FileUpRespuesta, "T_HYP_RESPUESTAOFICIO")
                    If objOficio.ArchivoRespuesta = String.Empty Then
                        Throw New ApplicationException("Error guardando el archivo " & FileUpRespuesta.FileName & " en SharePoint")
                    Else
                        BusinessRules.BDA_OFICIO.ActualizarArchivoRespuesta(objOficio)
                    End If
                Else

                    Throw New ApplicationException("El archivo " & FileUpRespuesta.FileName & " no es un tipo de archivo valido para la Respuesta")
                End If

            End If

            '---------------------------------------
            ' Expendiente
            '---------------------------------------
            If (FileUpExpediente.FileName <> String.Empty) And (btnDeleteExpediente.Visible = False) Then

                'NHM INI - Agrega validacion .zip para dictamenes
                If ID_TIPO_DOCUMENTO = OficioTipo.Dictamen Then

                    Dim tieneArchivoWord As Boolean
                    tieneArchivoWord = BusinessRules.BDA_OFICIO.ConsultarTieneArchivoWord(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

                    If tieneArchivoWord Then

                        If (validatipoDocumentoPDF_ZIP(FileUpExpediente)) Then

                            objOficio.ArchivoExpediente = GenerarNombreSharepoint(FileUpExpediente, "T_HYP_EXPEDIENTE")

                            If objOficio.ArchivoExpediente = String.Empty Then
                                Throw New ApplicationException("Error guardando el archivo " & FileUpExpediente.FileName & " en SharePoint")
                            Else
                                'NHM INI
                                'BusinessRules.BDA_OFICIO.ActualizarArchivoExpediente(objOficio)
                                'NHM FIN

                                'NHM - metodos nuevos
                                If Not LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ActualizarArchivoExpediente(objOficio, OficioTipo.Dictamen) Then
                                    Throw New ApplicationException("Error guardando el archivo")
                                Else
                                    GenerarSeguimientoExpediente(objOficio.ArchivoExpediente)
                                End If
                                'NHM FIN


                            End If

                        Else
                            Throw New ApplicationException("El archivo " & FileUpExpediente.FileName & " no es un tipo de archivo valido para el Expediente")
                        End If
                    Else
                        Throw New ApplicationException("Otros archivos deben ser generados primero")
                    End If


                Else
                    If (validatipoDocumentoPDF(FileUpExpediente)) Then
                        objOficio.ArchivoExpediente = GenerarNombreSharepoint(FileUpExpediente, "T_HYP_EXPEDIENTE")
                        If objOficio.ArchivoExpediente = String.Empty Then
                            Throw New ApplicationException("Error guardando el archivo " & FileUpExpediente.FileName & " en SharePoint")
                        Else
                            BusinessRules.BDA_OFICIO.ActualizarArchivoExpediente(objOficio)
                        End If
                    Else
                        Throw New ApplicationException("El archivo " & FileUpExpediente.FileName & " no es un tipo de archivo valido para el Expediente")
                    End If

                End If
                'NHM FIN

            End If

            VerificaDocumentoConcluido()

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
            archivoValido = False
        Catch ex As Exception
            EscribirError(ex, "Error al adjuntar documentos")
        End Try

        Return archivoValido

    End Function

    'NHM INI
    Public Sub GenerarSeguimientoExpediente(ByVal NombreArchivoNuevo As String)

        Dim objSeguimiento As New LogicaNegocioSICOD.Entities.BDA_SEGUIMIENTO_OFICIOEnt
        Dim FilePath As String = String.Empty
        Dim objBitacora As Bitacora.nsBitacora.Bitacora = Nothing
        Dim objBd As ConexionBaseDatos.ConexionBD.ConexionSql
        Dim arrValoresSeguimiento(9) As Object
        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim objSP As New nsSharePoint.FuncionesSharePoint


        Dim resultado As Boolean = False
        Dim fechaVencimientoValidacion As String = "NULL"

        Try


            objSeguimiento.ID_ANIO = ID_ANIO
            objSeguimiento.ID_AREA_OFICIO = ID_UNIDAD_ADM
            objSeguimiento.ID_TIPO_DOCUMENTO = ID_TIPO_DOCUMENTO
            objSeguimiento.I_OFICIO_CONSECUTIVO = I_OFICIO_CONSECUTIVO

            objSeguimiento.ID_USUARIO = USUARIO
            objSeguimiento.F_SEGUIMIENTO = DateTime.Now
            objSeguimiento.T_SEGUIMIENTO = HttpUtility.HtmlEncode("Se vincula por default el expediente")

            objSeguimiento.OFICIOPROCEDENTE_FLAG = 1
            objSeguimiento.T_OFICIO_PROCEDENTE = NombreArchivoNuevo

            'ID_EXPEDIENTE_EXT
            If ID_EXPEDIENTE_EXT = 0 Then

                'hay que crear expediente
                Dim Campos As String
                Dim Valores As String
                Dim Con As Conexion = Nothing
                Dim Dr As Odbc.OdbcDataReader = Nothing


                Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO.ToString, _
                                              ID_UNIDAD_ADM.ToString, _
                                              ID_TIPO_DOCUMENTO.ToString, _
                                              I_OFICIO_CONSECUTIVO.ToString)

                fechaVencimientoValidacion = "NULL"
                If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                    fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
                End If


                Con = New Conexion()

                '' creamos nuevo expediente (nadie tiene expediente)
                Dr = Con.Consulta("SELECT ISNULL(MAX(ID_EXPEDIENTE),0)+1 AS ID_EXPEDIENTE FROM " & Conexion.Owner & "BDA_C_EXPEDIENTE")
                Dr.Read()
                ID_EXPEDIENTE_EXT = Convert.ToInt32(Dr.Item("ID_EXPEDIENTE"))
                Dr.Close()

                Campos = "ID_EXPEDIENTE,DSC_EXPEDIENTE,USUARIO_CREACION,VIG_FLAG,FECH_INI_VIG"
                Valores = ID_EXPEDIENTE_EXT.ToString & ", '', '" & Session("Usuario") & "', 1, GETDATE()"
                If Not Con.Insertar(Conexion.Owner & "BDA_C_EXPEDIENTE", Campos, Valores) Then
                    Throw New ApplicationException("No pudo crearse el expediente ")
                End If


                Campos = "ID_EXPEDIENTE,ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_REGISTRO,VIG_FLAG,FECH_INI_VIG"
                Valores = ID_EXPEDIENTE_EXT.ToString & "," & ID_UNIDAD_ADM.ToString & "," & ID_TIPO_DOCUMENTO.ToString & "," & ID_ANIO.ToString & "," & _
                    I_OFICIO_CONSECUTIVO.ToString & ", '" & Session("Usuario") & "', 1, GETDATE()"
                If Not Con.Insertar(Conexion.Owner & "BDA_R_EXPEDIENTE_OFICIO", Campos, Valores) Then
                    Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                End If

                'GUARDAMOS EN BITACORA PARA OFICIO
                Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                    ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                Valores = ID_UNIDAD_ADM.ToString & "," & _
                    ID_TIPO_DOCUMENTO.ToString & "," & _
                    ID_ANIO.ToString & "," & _
                    I_OFICIO_CONSECUTIVO.ToString & ",'" & _
                    Session("Usuario") & "','" & Session("Usuario") & "','" & Session("Usuario") & "',GETDATE(),13,'AGREGADO AL EXPEDIENTE Num. " & ID_EXPEDIENTE_EXT.ToString() & "'," & fechaVencimientoValidacion

                If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores) Then
                    Throw New ApplicationException("No pudo vincularse el oficio al expediente ")
                End If


            End If


            objSeguimiento.ID_EXPEDIENTE = ID_EXPEDIENTE_EXT


            'Asigna valores para la bitacora

            arrValoresSeguimiento(1) = objSeguimiento.ID_USUARIO
            arrValoresSeguimiento(2) = objSeguimiento.I_OFICIO_CONSECUTIVO
            arrValoresSeguimiento(3) = objSeguimiento.ID_ANIO
            arrValoresSeguimiento(4) = objSeguimiento.ID_TIPO_DOCUMENTO
            arrValoresSeguimiento(5) = objSeguimiento.ID_AREA_OFICIO
            arrValoresSeguimiento(6) = objSeguimiento.F_SEGUIMIENTO
            arrValoresSeguimiento(7) = objSeguimiento.T_SEGUIMIENTO
            arrValoresSeguimiento(8) = objSeguimiento.OFICIOPROCEDENTE_FLAG
            arrValoresSeguimiento(9) = objSeguimiento.T_OFICIO_PROCEDENTE


            objBitacora = New Bitacora.nsBitacora.Bitacora()
            objBd = New ConexionBaseDatos.ConexionBD.ConexionSql()

            'Inserta los datos en la tabla seguimiento
            resultado = LogicaNegocioSICOD.BusinessRules.BDA_SEGUIMIENTO_OFICIO.InsertarComentario(objSeguimiento)

            'Valida si se ha guardado o no y manda un mensaje
            If (resultado) Then

            Else
                Throw New ApplicationException("Ocurrio un error al guardar el archivo")
            End If

            'Obtiene el id del seguimiento insertado
            arrValoresSeguimiento(0) = LogicaNegocioSICOD.BusinessRules.BDA_SEGUIMIENTO_OFICIO.ObtenerMaximoIdSeguimiento()

            objBitacora.IniciarBitacora("Insertar comentario/archivo", objBd, Session("IDsesion").ToString(), USUARIO, CType(Session("Perfil"), Integer), ID_UNIDAD_ADM)
            Dim arrCamposSeguimiento() As String = {"ID_SEGUIMIENTO_OFICIO", "ID_USUARIO", _
                                        "I_OFICIO_CONSECUTIVO", "ID_ANIO", "ID_TIPO_DOCUMENTO", "ID_AREA_OFICIO", _
                                        "F_SEGUIMIENTO", "T_SEGUIMIENTO", "OFICIOPROCEDENTE_FLAG", "T_OFICIO_PROCEDENTE"}

            objBitacora.BitacoraIniciarTransaccion()
            objBitacora.BitacoraInsertar("BDA_SEGUIMIENTO_OFICIO", arrCamposSeguimiento, arrValoresSeguimiento, True, resultado, "")
            objBitacora.BitacoraFinalizarTransaccion(True)

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
            If Not objBitacora Is Nothing Then objBitacora.BitacoraFinalizarTransaccion(False)
        Catch ex As Exception
            EscribirError(ex, "Error al insertar el comentario")
            If Not objBitacora Is Nothing Then objBitacora.BitacoraFinalizarTransaccion(False)
        Finally
            If System.IO.File.Exists(FilePath) Then System.IO.File.Delete(FilePath)
        End Try

    End Sub
    'NHM FIN

    Private Sub EliminaDocumento(ByVal nombreControl As String)
        Dim objOficio As LogicaNegocioSICOD.Entities.BDA_OFICIO
        objOficio = New LogicaNegocioSICOD.Entities.BDA_OFICIO

        Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

        Dim fechaVencimientoValidacion As String = "NULL"
        If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
            fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
        End If

        objOficio.IdAnio = ID_ANIO
        objOficio.IdArea = ID_UNIDAD_ADM
        objOficio.IdTipoDocumento = ID_TIPO_DOCUMENTO
        objOficio.IOficioConsecutivo = I_OFICIO_CONSECUTIVO
        objOficio.UsuarioElaboro = USUARIO
        objOficio.Comentario = fechaVencimientoValidacion

        Select Case nombreControl
            Case "btnDeleteWord"

                LinkDocWord.Visible = False
                btnDeleteWord.Visible = False
                fileUpWord.Visible = True
                objOficio.ArchivoWord = String.Empty
                BusinessRules.BDA_OFICIO.ActualizarArchivoWord(objOficio)
            Case "btnDeletePDF"
                LinkDocPDF.Visible = False
                btnDeletePDF.Visible = False
                fileUpPDF.Visible = True
                objOficio.ArchivoPDF = String.Empty
                BusinessRules.BDA_OFICIO.ActualizarArchivoPDF(objOficio)
            Case "btnDeleteCNE"
                LinkDocCNE.Visible = False
                btnDeleteCNE.Visible = False
                fileUpCNE.Visible = True
                objOficio.ArchivoCedulaPDF = String.Empty
                BusinessRules.BDA_OFICIO.ActualizarArchivoCedulaPDF(objOficio)
            Case "btnDeleteAnexo1"
                LinkAnexo1.Visible = False
                btnDeleteAnexo1.Visible = False
                FileUpAnexo1.Visible = True
                objOficio.ArchivoAnexo1 = String.Empty
                BusinessRules.BDA_OFICIO.ActualizarArchivoAnexo1(objOficio)
            Case "btnDeleteAnexo2"
                LinkAnexo2.Visible = False
                btnDeleteAnexo2.Visible = False
                FileUpAnexo2.Visible = True
                objOficio.ArchivoAnexo2 = ""
                BusinessRules.BDA_OFICIO.ActualizarArchivoAnexo2(objOficio, True)
            Case "btnDeleteFirmaDigital"
                LinkDocFirmaDigital.Visible = False
                btnDeleteFirmaDigital.Visible = False
                FileUpFirmaDigital.Visible = True
                objOficio.ArchivoFirmaDigital = String.Empty
                BusinessRules.BDA_OFICIO.ActualizarArchivoFirmaDigital(objOficio)
            Case "btnDeleteCedulaDigital"
                LinkDocCedulaDigital.Visible = False
                btnDeleteCedulaDigital.Visible = False
                FileUpCedulaDigital.Visible = True
                objOficio.ArchivoCedulaDigital = String.Empty
                BusinessRules.BDA_OFICIO.ActualizarArchivoCedulaDigital(objOficio)
            Case "btnDeleteAcuse"
                LinkDocAcuse.Visible = False
                btnDeleteAcuse.Visible = False
                FileUpAcuse.Visible = True
                txtFechaAcuse.Visible = True
                txtFechaAcuse.Text = String.Empty
                imgCalFechaAcuse.Visible = True
                objOficio.ArchivoAcuse = String.Empty
                BusinessRules.BDA_OFICIO.ActualizarArchivoAcuse(objOficio)
            Case "btnDeleteRespuesta"
                LinkDocRespuesta.Visible = False
                btnDeleteRespuesta.Visible = False
                FileUpRespuesta.Visible = True
                objOficio.ArchivoRespuesta = String.Empty
                BusinessRules.BDA_OFICIO.ActualizarArchivoRespuesta(objOficio)
            Case "btnDeleteExpediente"
                LinkDocExpediente.Visible = False
                btnDeleteExpediente.Visible = False
                FileUpExpediente.Visible = True
                objOficio.ArchivoExpediente = String.Empty
                BusinessRules.BDA_OFICIO.ActualizarArchivoExpediente(objOficio)
        End Select

    End Sub

    Private Function archivoSharepoint(ByVal nombreArchivo As String) As Boolean
        Dim archivoNuevo As Boolean = True

        If (InStr(nombreArchivo, "\") > 0) Then archivoNuevo = False

        Return archivoNuevo

    End Function

    Protected Sub AbreArchivoLink(ByVal NombreArchivo As String)

        Dim cliente As System.Net.WebClient = New System.Net.WebClient
        Dim usuario As String
        Dim passwd As String
        Dim ServSharepoint As String
        Dim Dominio As String
        Dim Biblioteca As String
        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim urlEncode As String = String.Empty
        Dim filename As String = String.Empty
        Dim Archivo() As Byte = Nothing

        Try

            Try

                If NombreArchivo.Contains("#") AndAlso NombreArchivo.ToLower.Contains(WebConfigurationManager.AppSettings("FILES_PATH").ToLower.ToString) Then
                    NombreArchivo = NombreArchivo.Substring(0, NombreArchivo.IndexOf("#"))
                    Archivo = cliente.DownloadData(NombreArchivo)

                Else

                    If NombreArchivo.Contains("@") Then NombreArchivo = NombreArchivo.Substring(0, NombreArchivo.IndexOf("@"))

                    usuario = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioSp")
                    passwd = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR")
                    ServSharepoint = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointServerOficios"), "webCONSAR")
                    Dominio = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("Domain"), "webCONSAR")
                    Biblioteca = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("DocLibraryOficios"), "webCONSAR")

                    cliente.Credentials = New NetworkCredential(usuario, passwd, Dominio)

                    Dim url As String = String.Empty
                    url = ServSharepoint & "/" & Biblioteca & "/" & NombreArchivo
                    urlEncode = Server.UrlPathEncode(url)

                    Archivo = cliente.DownloadData(ResolveUrl(urlEncode))
                End If
                filename = "attachment; filename=" & Server.UrlPathEncode(NombreArchivo)

            Catch ex As Exception
                ControlErrores.nsControlErrores.ControlErrores.EscribirEvento("Función AbreArchivoLink: " & ex.Message, EventLogEntryType.Error, "Application", "")
                Throw New ApplicationException("Hubo un error abriendo el documento. Posiblemente no existe o no tiene permisos para verlo.")
            End Try

            If Not Archivo Is Nothing Then

                Dim tipo_arch As String = NombreArchivo.Substring(NombreArchivo.LastIndexOf(".") + 1)

                Select Case tipo_arch
                    Case "zip"
                        Response.ContentType = "application/x-zip-compressed"
                    Case "pdf"
                        Response.ContentType = "application/pdf"
                    Case "csv"
                        Response.ContentType = "text/csv"
                    Case "doc"
                        Response.ContentType = "application/doc"
                    Case "docx"
                        Response.ContentType = "application/docx"
                    Case "xls"
                        Response.ContentType = "application/xls"
                    Case "xlsx"
                        Response.ContentType = "application/xlsx"
                    Case "png"
                        Response.ContentType = "image/png"
                    Case "gif"
                        Response.ContentType = "image/gif"
                    Case "jpg"
                        Response.ContentType = "image/jpeg"
                    Case "jpeg"
                        Response.ContentType = "image/jpeg"
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
                    Case "sbm"
                        Response.ContentType = "application/octet-stream"
                    Case Else
                        Response.ContentType = "application/octet-stream"
                End Select

                Response.AddHeader("content-disposition", filename)

                Response.BinaryWrite(Archivo)

                Response.End()
                '---------------------------------------------
                ' No usamos HttpContext.Current.ApplicationInstance.CompleteRequest()
                ' porque en archivos de texto (txt, csv, etc...) agregaba al final el código de la página.
                '---------------------------------------------
            End If
        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "AbreArchivoLink")
        End Try
    End Sub

    Protected Sub btnAdjuntar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdjuntar.Click

        If (AdjuntarDocumentos()) Then
            modalMensaje("Se adjuntaron correctamente los archivos", "OKAdjuntar", "CONFIRMACIÓN")
        End If
    End Sub

#Region "Modal Mensaje y respuesta"
    Private Sub modalMensaje(
                                ByVal mensaje As String, Optional ByVal PostBackCall As String = "",
                                    Optional ByVal Titulo As String = "ALERTA",
                                        Optional ByVal showCancelButton As Boolean = False,
                                            Optional ByVal AcceptButtonText As String = "Aceptar",
                                                Optional ByVal CancelButtonText As String = "Cancelar"
                            )

        lblErroresTitulo.Style.Add("display", "block")
        lblErroresTitulo.Text = Titulo
        lblErroresPopup.Text = "<ul>" & mensaje & "</ul>"
        lblErroresPopup.Style.Add("display", "block")
        lblPostBack.Text = PostBackCall
        BtnModalOk.Text = AcceptButtonText
        BtnContinua.Visible = showCancelButton
        BtnContinua.Text = CancelButtonText
        ModalPopupExtenderErrores.Show()
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        Session("ID_ANIO") = ID_ANIO
        Session("ID_UNIDAD_ADM") = ID_UNIDAD_ADM
        Session("ID_TIPO_DOCUMENTO") = ID_TIPO_DOCUMENTO
        Session("I_OFICIO_CONSECUTIVO") = I_OFICIO_CONSECUTIVO
        Response.Redirect("~/App_Oficios/Registro.aspx?Modificar=1")
    End Sub

    Protected Sub BtnModalOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnModalOk.Click

        Select Case lblPostBack.Text
            Case "OKAdjuntar"
                Response.Redirect("~/App_Oficios/AdjuntarDocumentos.aspx", False)
            Case "btnDeleteWord",
                    "btnDeletePDF",
                    "btnDeleteCedulaPDF",
                    "btnDeleteAnexo1",
                    "btnDeleteAnexo2",
                    "btnDeleteFirmaDigital",
                    "btnDeleteCNE",
                    "btnDeleteAcuse",
                    "btnDeleteRespuesta",
                    "btnDeleteExpediente",
                    "btnDeleteCedulaDigital"

                EliminaDocumento(lblPostBack.Text)
        End Select

    End Sub
#End Region

#Region "Eventos de botones para abrir archivo"
    Protected Sub LinkDocWord_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkDocWord.Click
        AbreArchivoLink(LinkDocWord.CommandName)
    End Sub

    Protected Sub LinkDocPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkDocPDF.Click

        AbreArchivoLink(LinkDocPDF.CommandName)
    End Sub

    Protected Sub LinkDocCedulaPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkDocCNE.Click
        AbreArchivoLink(LinkDocCNE.CommandName)
    End Sub

    Protected Sub LinkAnexo1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkAnexo1.Click
        AbreArchivoLink(LinkAnexo1.CommandName)
    End Sub

    Protected Sub LinkAnexo2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkAnexo2.Click
        AbreArchivoLink(LinkAnexo2.CommandName)
    End Sub

    Protected Sub LinkDocFirmaDigital_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkDocFirmaDigital.Click
        AbreArchivoLink(LinkDocFirmaDigital.CommandName)
    End Sub

    Protected Sub LinkDocCNE_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkDocCNE.Click
        AbreArchivoLink(LinkDocCNE.CommandName)
    End Sub

    Protected Sub LinkDocAcuse_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkDocAcuse.Click
        AbreArchivoLink(LinkDocAcuse.CommandName)
    End Sub

    Protected Sub LinkDocRespuesta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkDocRespuesta.Click
        AbreArchivoLink(LinkDocRespuesta.CommandName)
    End Sub

    Protected Sub LinkDocExpediente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkDocExpediente.Click
        AbreArchivoLink(LinkDocExpediente.CommandName)
    End Sub
#End Region

#Region "Verificar usuario"
    Private Function verificaUsuario() As Boolean

        If Session("PERFIL_ASISTENTE") Is Nothing Then Session("PERFIL_ASISTENTE") = BusinessRules.BDS_C_PERFIL.ConsultarPerfilPorNombre("ASISTENTE")

        If CInt(Session("perfil")) = CInt(Session("PERFIL_ASISTENTE")) Then
            Dim dtUsuarios As DataTable = BusinessRules.BDA_R_USUARIO_ASISTENTE.getUsuarios(USUARIO)
            Dim list As New List(Of String)
            If dtUsuarios.Rows.Count > 0 Then
                For Each row As DataRow In dtUsuarios.Rows
                    list.Add(row("USUARIO").ToString())
                Next
                If BusinessRules.BDA_OFICIO.ConsultarUsuarioPuedeModificar(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, list) Then
                    Return True
                End If
            End If
        End If

        Return BusinessRules.BDA_OFICIO.ConsultarUsuarioPuedeModificar(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, USUARIO)
    End Function
#End Region

    Protected Sub btnDeleteWord_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles _
            btnDeleteWord.Click, btnDeletePDF.Click, btnDeleteCNE.Click, btnDeleteAnexo1.Click, btnDeleteAnexo2.Click, btnDeleteFirmaDigital.Click,
            btnDeleteCNE.Click, btnDeleteAcuse.Click, btnDeleteRespuesta.Click, btnDeleteExpediente.Click, btnDeleteCedulaDigital.Click

        Dim ib As ImageButton = TryCast(sender, ImageButton)

        modalMensaje("¿Realmente desea borrar archivo?", ib.ID, "ALERTA", True)
    End Sub

    Private Function revisarSBM(ByVal tipoArchivo As String, ByVal ExtensionOriginalParaSBMX As String) As String

        Dim resultado As String = String.Empty

        Dim objOficio As New Entities.BDA_OFICIO
        objOficio.IdAnio = ID_ANIO
        objOficio.IdTipoDocumento = ID_TIPO_DOCUMENTO
        objOficio.IOficioConsecutivo = I_OFICIO_CONSECUTIVO
        objOficio.UsuarioElaboro = USUARIO
        objOficio.IdArea = ID_UNIDAD_ADM

        Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

        Dim fechaVencimientoValidacion As String = "NULL"
        If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
            fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
        End If
        objOficio.Comentario = fechaVencimientoValidacion

        Dim enc As New YourCompany.Utils.Encryption.Encryption64()

        Dim UsuarioSp As String = AppSettings("UsuarioSp")
        Dim PassEncSp As String = enc.DecryptFromBase64String(AppSettings("PassEncSp"), "webCONSAR")
        Dim Domain As String = enc.DecryptFromBase64String(AppSettings("Domain"), "webCONSAR")
        Dim ServidorSp As String = enc.DecryptFromBase64String(AppSettings("SharePointServerOficios"), "webCONSAR")

        Dim bibliotecaSp As String = enc.DecryptFromBase64String(AppSettings("DocLibraryOficios"), "webCONSAR")

        Dim proposedFileName As String = crearNombreArchivo("dummy.sbm", tipoArchivo)
        Dim proposedUrl As String = ServidorSp & "/" & bibliotecaSp & "/" & proposedFileName

        If UrlExiste(proposedUrl, New NetworkCredential(UsuarioSp, PassEncSp, Domain)) Then


            resultado = proposedFileName
        Else

            proposedFileName = crearNombreArchivo("dummy.sbmx", tipoArchivo, ExtensionOriginalParaSBMX)
            proposedUrl = ServidorSp & "/" & bibliotecaSp & "/" & proposedFileName


            If UrlExiste(proposedUrl, New NetworkCredential(UsuarioSp, PassEncSp, Domain)) Then

                resultado = proposedFileName
            Else

                resultado = String.Empty

            End If

        End If

        If Not resultado = String.Empty Then

            Select Case tipoArchivo

                Case "T_CEDULADIGITAL"
                    objOficio.ArchivoCedulaDigital = proposedFileName
                    BusinessRules.BDA_OFICIO.ActualizarArchivoCedulaDigital(objOficio)

                Case "T_HYP_FIRMADIGITAL"
                    objOficio.ArchivoFirmaDigital = proposedFileName
                    BusinessRules.BDA_OFICIO.ActualizarArchivoFirmaDigital(objOficio)

                Case "T_ANEXO_UNO"
                    objOficio.ArchivoAnexo2 = proposedFileName
                    BusinessRules.BDA_OFICIO.ActualizarArchivoAnexo2(objOficio, True)


            End Select

        End If

        Return resultado
    End Function

    ''' <summary>
    ''' Revisa si el archivo remoto existe en el Servidor
    ''' </summary>
    ''' <param name="url"></param>
    ''' <param name="credentials"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UrlExiste(ByVal url As String, ByVal credentials As NetworkCredential) As Boolean
        Dim oRequest As System.Net.WebRequest
        oRequest = System.Net.WebRequest.Create(url)
        oRequest.Credentials = credentials
        Try
            Dim myResponse As System.Net.HttpWebResponse = CType(oRequest.GetResponse(), HttpWebResponse)
            myResponse.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub VerificaDocumentoConcluido()

        Dim dt As DataTable = BusinessRules.BDA_OFICIO.ConsultarEstatusOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

        '' EN CASO QUE EL OFICIO ESTE CONCLUIDO, NOTIFICADO O CANCELADO, NO SE DEBE PODER ELIMINAR DOCUMENTOS
        If Convert.ToInt32(dt.Rows(0)("ID_ESTATUS")) = 4 OrElse Convert.ToInt32(dt.Rows(0)("ID_ESTATUS")) = 6 OrElse _
            Convert.ToInt32(dt.Rows(0)("ID_ESTATUS")) = 7 OrElse Convert.ToInt32(dt.Rows(0)("ID_ESTATUS")) = 8 Then

            btnDeleteWord.Visible = False
            btnDeletePDF.Visible = False
            btnDeleteCNE.Visible = False
            btnDeleteAnexo1.Visible = False
            btnDeleteFirmaDigital.Visible = False
            btnDeleteCedulaDigital.Visible = False
            btnDeleteAnexo2.Visible = False
            btnDeleteAcuse.Visible = False
            btnDeleteRespuesta.Visible = False
            btnDeleteExpediente.Visible = False

        End If


    End Sub

    Private Function onReglasDeNegocioOK(ByVal tipoArchivo As String) As Boolean

        Select Case tipoArchivo

            Case "T_CEDULADIGITAL"
                '----------------------------------------
                ' No se puede generar cédula digital sin cédula
                '----------------------------------------
                Return BusinessRules.BDA_OFICIO.ConsultarTieneArchivoCedula(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            Case "T_HYP_FIRMADIGITAL"
                '----------------------------------------
                ' No se puede generar la firma digital sin PDF
                '----------------------------------------
                Return BusinessRules.BDA_OFICIO.ConsultarTieneArchivoPDF(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            Case "T_ANEXO_DOS"
                '----------------------------------------
                ' No se puede generar el anexo digital sin el anexo
                '----------------------------------------
                Return Not String.IsNullOrEmpty(BusinessRules.BDA_OFICIO.ConsultaDocAnexo1(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO))

            Case Else
                Return True

        End Select

    End Function

End Class