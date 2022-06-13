Imports System.Web.Configuration
Imports System.IO
Imports System.Web
Imports LogicaNegocioSICOD
Imports SICOD.Generales
Imports Clases
Imports System.Net

Public Class Seguimiento
    Inherits System.Web.UI.Page

#Region "Propiedades de la página"

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

    Public Property ID_UNIDAD_ADM_EDIT() As Integer
        Get
            Return CInt(ViewState("ID_UNIDAD_ADM_EDIT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_UNIDAD_ADM_EDIT") = value
        End Set
    End Property

    Public Property ID_ANIO_EDIT() As Integer
        Get
            Return CInt(ViewState("ID_ANIO_EDIT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_ANIO_EDIT") = value
        End Set
    End Property

    Public Property ID_TIPO_DOCUMENTO_EDIT() As Integer
        Get
            Return CInt(ViewState("ID_TIPO_DOCUMENTO_EDIT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_TIPO_DOCUMENTO_EDIT") = value
        End Set
    End Property

    Public Property I_OFICIO_CONSECUTIVO_EDIT() As Integer
        Get
            Return CInt(ViewState("I_OFICIO_CONSECUTIVO_EDIT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("I_OFICIO_CONSECUTIVO_EDIT") = value
        End Set
    End Property

    Public Property ID_EXPEDIENTE() As Integer
        Get
            Return CInt(ViewState("ID_EXPEDIENTE"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_EXPEDIENTE") = value
        End Set
    End Property

    Public Property ID_FOLIO_EDIT() As Integer
        Get
            Return CInt(ViewState("ID_FOLIO_EDIT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_FOLIO_EDIT") = value
        End Set
    End Property

    Public Property USUARIO_SICOD_EDIT() As String
        Get
            Return ViewState("USUARIO_SICOD_EDIT").ToString
        End Get
        Set(ByVal value As String)
            ViewState("USUARIO_SICOD_EDIT") = value
        End Set
    End Property

    Public Property ORIGINAL_FLAG_EDIT() As Integer
        Get
            Return CInt(ViewState("ORIGINAL_FLAG_EDIT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ORIGINAL_FLAG_EDIT") = value
        End Set
    End Property

    Public Property DESTINATARIO_EDIT() As String
        Get
            Return ViewState("DESTINATARIO_EDIT").ToString
        End Get
        Set(ByVal value As String)
            ViewState("DESTINATARIO_EDIT") = value
        End Set
    End Property

    Public Property ID_EXPEDIENTE_DOC() As Integer
        Get
            Return CInt(ViewState("ID_EXPEDIENTE_DOC"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_EXPEDIENTE_DOC") = value
        End Set
    End Property
    Public Property ID_SEGUIMIENTO_OFICIO() As Integer
        Get
            Return CInt(ViewState("ID_SEGUIMIENTO_OFICIO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_SEGUIMIENTO_OFICIO") = value
        End Set
    End Property

    Public Property CODIGO_AREA() As Integer
        Get
            Return CInt(ViewState("CODIGO_AREA"))
        End Get
        Set(ByVal value As Integer)
            ViewState("CODIGO_AREA") = value
        End Set
    End Property

    Public Property EDITAR() As Boolean
        Get
            Return CBool(ViewState("EDITAR"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("EDITAR") = value
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

    Public Property NOMBRE_ARCHIVO_EDIT As String
        Get
            Return ViewState("NOMBRE_ARCHIVO_EDIT").ToString
        End Get
        Set(ByVal value As String)
            ViewState("NOMBRE_ARCHIVO_EDIT") = value
        End Set
    End Property

    Public Property ID_ARCHIVO_EDIT As Integer
        Get
            Return CInt(ViewState("ID_ARCHIVO_EDIT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_ARCHIVO_EDIT") = value
        End Set
    End Property

    Public Property DSC_T_DOC_EDIT As String
        Get
            Return ViewState("DSC_T_DOC_EDIT").ToString
        End Get
        Set(ByVal value As String)
            ViewState("DSC_T_DOC_EDIT") = value
        End Set
    End Property

    Public Property ID_EXPEDIENTE_EXT() As Integer
        Get
            Return CInt(ViewState("ID_EXPEDIENTE_EXT"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_EXPEDIENTE_EXT") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '-----------------------------------------------
        ' Verificar Sesión y Perfil de usuario
        '-----------------------------------------------
        verificaSesion()
        verificaPerfil()

        If Not IsPostBack Then

            If Session("Usuario") Is Nothing Then logOut()
            USUARIO = Session("Usuario").ToString

            '-----------------------------------------------
            ' Trae variables de sesión de bandeja y establece propiedades de la página en el Viewstate
            '-----------------------------------------------
            ID_UNIDAD_ADM = CInt(Session("ID_UNIDAD_ADM"))
            ID_ANIO = CInt(Session("ID_ANIO"))
            ID_TIPO_DOCUMENTO = CInt(Session("ID_TIPO_DOCUMENTO"))
            I_OFICIO_CONSECUTIVO = CInt(Session("I_OFICIO_CONSECUTIVO"))
            ID_EXPEDIENTE = CInt(Session("ID_EXPEDIENTE"))
            CODIGO_AREA = CInt(Session("CODIGO_AREA"))
            NUMERO_OFICIO = Session("NUMERO_OFICIO").ToString
            ID_EXPEDIENTE_EXT = 0

            Dim _dt1 As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_R_EXPEDIENTE_OFICIO.GetByOFICIO(ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, ID_ANIO, I_OFICIO_CONSECUTIVO)

            _dt1.DefaultView.RowFilter = "ISORIGINAL_FLAG=1"

            If _dt1.DefaultView.ToTable.Rows.Count > 0 Then ID_EXPEDIENTE_EXT = Convert.ToInt32(_dt1.DefaultView.ToTable.Rows(0)("ID_EXPEDIENTE").ToString())



            EDITAR = verificaUsuario()
            If Not EDITAR Then
                btnAgregarOficio.Enabled = False
                btnAgregarDocSICOD.Enabled = False
                btnNuevoComentario.Enabled = False
                lblTitulo.Text = "Seguimiento y Vinculación para <strong>" & NUMERO_OFICIO & "</strong> (sólo lectura)"
            Else

                lblTitulo.Text = "Seguimiento y Vinculación para <strong>" & NUMERO_OFICIO & "</strong>"
            End If

            ID_EXPEDIENTE_DOC = BusinessRules.BDA_R_OFICIO_DOC.ConsultarExpedienteOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

            txtFechaEnvio.Attributes.Add("readonly", "readonly")
            '----------------------------------------
            ' Consulta la tabla seguimiento con los datos que trae de la página de registro Oficio
            '----------------------------------------
            CargaDatosSeguimiento(BusinessRules.BDA_SEGUIMIENTO_OFICIO.GetAll(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO))
            '----------------------------------------
            ' Consultar otro oficios relacionados
            '----------------------------------------
            cargarOficiosRelacionados()
            cargarDocumentosSicodRelacionados()

            'NHM INI - Valida si en sisan es enviado a Sanciones
            Try
                Dim _dtOficio As DataTable = Oficio.GetByKey(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
                If Not IsNothing(_dtOficio) AndAlso _dtOficio.Rows.Count > 0 Then
                    If IsDBNull(_dtOficio.Rows.Item(0)("IS_ENVIADO_SANCIONES_SISAN_FLAG")) Then
                    ElseIf Convert.ToInt32(_dtOficio.Rows.Item(0)("IS_ENVIADO_SANCIONES_SISAN_FLAG")) = 1 Then
                        Dim colEliminar_gdv1 As Integer = 5
                        Dim colEliminar_gdv2 As Integer = 11
                        Dim colEliminar_gdv3 As Integer = 13
                        dgComentarioSeguimiento.Columns(colEliminar_gdv1).Visible = False
                        gvVincularOficios.Columns(colEliminar_gdv2).Visible = False
                        gvVincularDocsSICOD.Columns(colEliminar_gdv3).Visible = False
                    End If
                End If
            Catch ex As Exception
            End Try
            'NHM FIN

            pnlNuevoComentario.Visible = False


            Dim _dtExp As DataTable = BusinessRules.BDA_R_EXPEDIENTE_OFICIO.GetByOFICIO(ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, ID_ANIO, I_OFICIO_CONSECUTIVO)

            _dt1.DefaultView.RowFilter = "ISORIGINAL_FLAG=1"
            If _dtExp.DefaultView.ToTable.Rows.Count > 0 Then

                With _dtExp.DefaultView.ToTable.Rows(0)

                    lblDescripcion.Text = "El Oficio se encuentra ligado al expediente número " & .Item("ID_EXPEDIENTE").ToString()
                    hdnExpediente.Value = .Item("ID_EXPEDIENTE").ToString()

                End With

            End If


        End If

    End Sub

    Private Sub cargarDocumentosSicodRelacionados()
        '-----------------------------------
        ' Obtener expediente del oficio de la tabla de relación con docs SICOD
        '-----------------------------------
        If ID_EXPEDIENTE_DOC > 0 OrElse ID_EXPEDIENTE_EXT > 0 Then
            'ORIGINAL_FLAG
            Dim dtDocsSicodRelacionados = BusinessRules.BDA_R_OFICIO_DOC.GetDocumentos(ID_EXPEDIENTE_DOC, ID_EXPEDIENTE_EXT)
            dtDocsSicodRelacionados.DefaultView.RowFilter = "DESTINATARIO = 'O'"
            Dim dt As DataTable = dtDocsSicodRelacionados.DefaultView.ToTable()

            If dtDocsSicodRelacionados.Rows.Count > 0 Then

                '-----------------------------------
                ' Funciones auxliares para relacionar documento a Sharepoint
                '-----------------------------------
                AgregarColumnasAnexo(dt)
                AgregarColumnasSharePoint(dt)

                gvVincularDocsSICOD.DataSource = dt
                gvVincularDocsSICOD.DataBind()
            Else
                lblNoHayDocsSICOD.Style.Add("display", "")
                gvVincularDocsSICOD.Style.Add("display", "none")
            End If
            Panel1.Visible = True
            Panel3.Visible = True
        Else
            lblNoHayDocsSICOD.Style.Add("display", "")
            gvVincularDocsSICOD.Style.Add("display", "none")
            Panel1.Visible = False
            Panel3.Visible = False
        End If

    End Sub

    Private Sub cargarOficiosRelacionados()
        Dim dtOficiosRelacionados As DataTable = BusinessRules.BDA_R_OFICIOS.GetAll(ID_EXPEDIENTE, ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO, ID_EXPEDIENTE_EXT)
        If dtOficiosRelacionados.Rows.Count > 0 Then
            gvVincularOficios.DataSource = dtOficiosRelacionados
            gvVincularOficios.DataBind()
            ''Panel2.Visible = True
            pnlVincularOficios.Visible = True
        Else
            lblNoHayOficiosRelacionados.Style.Add("display", "")
            gvVincularOficios.Style.Add("display", "none")
            ''Panel2.Visible = False
        End If
    End Sub

    Private Sub CargaDatosSeguimiento(ByVal dtSeguimiento As DataTable)

        Session("_DT_SEGUIMIENTO") = dtSeguimiento.DefaultView

        If dtSeguimiento.Rows.Count > 0 Then
            dgComentarioSeguimiento.DataSource = dtSeguimiento
            dgComentarioSeguimiento.DataBind()
            lblNoHayComentarios.Style.Add("display", "none")
            dgComentarioSeguimiento.Style.Add("display", "")

        Else
            lblNoHayComentarios.Style.Add("display", "")
            dgComentarioSeguimiento.Style.Add("display", "none")
        End If

        textOficio.Text() = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ObtenerT_NumeroOficio(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)
        txtFechaEnvio.Text() = DateTime.Now.ToString("dd/MM/yyyy")
        textOficio.ReadOnly = True
        txtFechaEnvio.ReadOnly = True

    End Sub

    ''' <summary>
    ''' Guarda el nuevo comentario en la tabla seguimiento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Dim objSeguimiento As New LogicaNegocioSICOD.Entities.BDA_SEGUIMIENTO_OFICIOEnt
        Dim NombreArchivoNuevo As String = String.Empty
        Dim FilePath As String = String.Empty
        Dim objBitacora As Bitacora.nsBitacora.Bitacora = Nothing
        Dim objBd As ConexionBaseDatos.ConexionBD.ConexionSql
        Dim arrValoresSeguimiento(9) As Object
        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim objSP As New nsSharePoint.FuncionesSharePoint

        Dim VerExpediente As Boolean = False
        Dim resultado As Boolean = False
        Dim fechaVencimientoValidacion As String = "NULL"

        Try
            If (textComentario.Text() <> String.Empty) Then

                objSeguimiento.ID_ANIO = ID_ANIO
                objSeguimiento.ID_AREA_OFICIO = ID_UNIDAD_ADM
                objSeguimiento.ID_TIPO_DOCUMENTO = ID_TIPO_DOCUMENTO
                objSeguimiento.I_OFICIO_CONSECUTIVO = I_OFICIO_CONSECUTIVO

                objSeguimiento.ID_USUARIO = USUARIO
                objSeguimiento.F_SEGUIMIENTO = DateTime.Now
                objSeguimiento.T_SEGUIMIENTO = HttpUtility.HtmlEncode(textComentario.Text)

                If (fUpOficioProcedente.FileName IsNot String.Empty) Then

                    NombreArchivoNuevo =
                                            "S_" &
                                            ID_ANIO.ToString() & "_" &
                                            Format(CODIGO_AREA, "000").ToString & "_" &
                                            ID_TIPO_DOCUMENTO.ToString & "_" &
                                            Format(I_OFICIO_CONSECUTIVO, "000").ToString() & "_" &
                                            fUpOficioProcedente.FileName


                    FilePath = System.IO.Path.GetTempPath() + NombreArchivoNuevo

                    fUpOficioProcedente.SaveAs(FilePath)
                    fUpOficioProcedente.Dispose()

                    objSP.ServidorSharePoint = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("SharePointServerOficios"), "webCONSAR")
                    objSP.Biblioteca = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("DocLibraryOficios"), "webCONSAR")
                    objSP.Usuario = WebConfigurationManager.AppSettings("UsuarioSp").ToString()
                    objSP.Password = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR")
                    objSP.Dominio = enc.DecryptFromBase64String(WebConfigurationManager.AppSettings("Domain"), "webCONSAR")
                    objSP.RutaArchivo = Path.GetTempPath
                    objSP.NombreArchivo = NombreArchivoNuevo

                    Dim cargoArchivo As Boolean = objSP.UploadFileToSharePoint()

                    objSeguimiento.OFICIOPROCEDENTE_FLAG = 1
                    objSeguimiento.T_OFICIO_PROCEDENTE = NombreArchivoNuevo
                    VerExpediente = True
                Else
                    objSeguimiento.T_OFICIO_PROCEDENTE = String.Empty
                    objSeguimiento.OFICIOPROCEDENTE_FLAG = 0
                    objSeguimiento.ID_EXPEDIENTE = 0
                End If

                'ID_EXPEDIENTE_EXT
                If VerExpediente And ID_EXPEDIENTE_EXT = 0 Then

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

                If VerExpediente Then
                    objSeguimiento.ID_EXPEDIENTE = ID_EXPEDIENTE_EXT
                End If

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
                    modalMensaje("Se ha guardado exitosamente el archivo")

                    CargaDatosSeguimiento(LogicaNegocioSICOD.BusinessRules.BDA_SEGUIMIENTO_OFICIO.GetAll(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO))
                    pnlSeguimiento.Visible = True
                    pnlNuevoComentario.Visible = False
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

            Else
                Throw New ApplicationException("Debe escribir un comentario para continuar")
            End If

            pnlNuevoComentario.Visible = False
            pnlSeguimiento.Visible = True
            pnlVincularDocsSICOD.Visible = True
            pnlVincularOficios.Visible = True
            btnCancelar.Visible = True

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

    Protected Sub btnNuevoComentario_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevoComentario.Click
        Try
            pnlSeguimiento.Visible = False
            pnlNuevoComentario.Visible = True
            textComentario.Text() = String.Empty
            pnlVincularDocsSICOD.Visible = False
            pnlVincularOficios.Visible = False
            btnCancelar.Visible = False
            ''Panel2.Visible = False
            Panel1.Visible = False
            Panel3.Visible = False
        Catch ex As Exception
            EscribirError(ex, "Error al limpiar campos para un nuevo comentario")
        End Try
    End Sub

    'Cancelar el seguimiento regresa a modificación o registro oficio
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click

        Session("ID_ANIO") = ID_ANIO
        Session("ID_UNIDAD_ADM") = ID_UNIDAD_ADM
        Session("ID_TIPO_DOCUMENTO") = ID_TIPO_DOCUMENTO
        Session("I_OFICIO_CONSECUTIVO") = I_OFICIO_CONSECUTIVO
        Response.Redirect("~/App_Oficios/Registro.aspx?Modificar=1", False)
    End Sub

    ''' <summary>
    ''' Cancelar nuevo comentario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCancelarNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelarNuevo.Click
        Try
            pnlNuevoComentario.Visible = False
            pnlSeguimiento.Visible = True
            pnlVincularDocsSICOD.Visible = True
            pnlVincularOficios.Visible = True
            btnCancelar.Visible = True
            Panel1.Visible = True
            ''Panel2.Visible = True
            Panel3.Visible = True

        Catch ex As Exception
            EscribirError(ex, "Error al cancelar un nuevo comentario")
        End Try
    End Sub
    ''' <summary>
    ''' Metdodo para abrir el documento que se adjuntó
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub AbrirDocumento_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            AbreArchivoLink(sender.Text.ToString.Trim)

        Catch ex As System.Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "AbrirDocumento_Click")
        End Try

    End Sub

    Protected Sub AbreArchivoLink(ByVal NombreArchivo As String, Optional ByVal isOficio As Boolean = True)

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
                    usuario = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioSp")
                    passwd = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("PassEncSp"), "webCONSAR")
                    Dominio = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("Domain"), "webCONSAR")

                    '-----------------------------------------
                    ' Si es archivo SICOD o archivo Oficios.
                    '-----------------------------------------
                    If isOficio Then
                        ServSharepoint = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointServerOficios"), "webCONSAR")
                        Biblioteca = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("DocLibraryOficios"), "webCONSAR")
                    Else
                        ServSharepoint = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointServer"), "webCONSAR")
                        Biblioteca = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("DocLibrary"), "webCONSAR")
                    End If

                    cliente.Credentials = New NetworkCredential(usuario, passwd, Dominio)

                    Dim url As String = ServSharepoint & "/" & Biblioteca & "/" & NombreArchivo
                    urlEncode = Server.UrlPathEncode(Url)
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
        Catch ex As System.Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "AbreArchivoLink")
        End Try
    End Sub

    Protected Sub dgComentarioSeguimiento_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgComentarioSeguimiento.RowCommand

        Try
            If (e.CommandName = "Eliminar") Then

                If Not EDITAR Then Throw New ApplicationException("No se permite elminar documento")

                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                ID_SEGUIMIENTO_OFICIO = dgComentarioSeguimiento.DataKeys(index)("ID_SEGUIMIENTO_OFICIO")

                lblErroresTitulo.Style.Add("display", "none")
                modalMensaje("¿Desea eliminar este archivo?", "EliminarComentario", "ALERTA", True)
            End If

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As Exception
            EscribirError(ex, "Error al eliminar el archivo")
        End Try

    End Sub

    Private Sub modalMensaje(
                                ByVal mensaje As String, Optional ByVal PostBackCall As String = "",
                                    Optional ByVal Titulo As String = "ALERTA",
                                        Optional ByVal showCancelButton As Boolean = False,
                                            Optional ByVal AcceptButtonText As String = "Aceptar",
                                                Optional ByVal CancelButtonText As String = "Cancelar"
                                                                                                )

        lblErroresTitulo.Style.Add("display", "block")
        lblErroresTitulo.Text = Titulo
        lblErroresPopup.Text = "<ul><li>" & mensaje & "</li></ul>"
        lblErroresPopup.Style.Add("display", "block")
        lblModalPostBack.Text = PostBackCall
        BtnModalOk.Text = AcceptButtonText
        BtnCancelarModal.Visible = showCancelButton
        BtnCancelarModal.Text = CancelButtonText
        ModalPopupExtenderErrores.Show()
    End Sub
    Protected Sub BtnModalOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnModalOk.Click
        If lblModalPostBack.Text = "EliminarComentario" Then
            EliminarComentario()
        ElseIf lblModalPostBack.Text = "EliminarDocSICOD" Then
            If Not EDITAR Then Throw New ApplicationException("No se permite elminar documento")

            '-------------------------------------------
            ' Desasociar
            ' borrar relación en base al expediente, el oficio y la llave del documento SICOD (usuario, destinatario e id_folio)
            '-------------------------------------------
            Dim obj_R_Oficio_Doc As New Entities.BDA_R_OFICIO_DOC

            obj_R_Oficio_Doc.ID_EXPEDIENTE = ID_EXPEDIENTE_DOC

            obj_R_Oficio_Doc.ID_ANIO = ID_ANIO
            obj_R_Oficio_Doc.ID_AREA_OFICIO = ID_UNIDAD_ADM
            obj_R_Oficio_Doc.ID_TIPO_DOCUMENTO = ID_TIPO_DOCUMENTO
            obj_R_Oficio_Doc.I_OFICIO_CONSECUTIVO = I_OFICIO_CONSECUTIVO

            obj_R_Oficio_Doc.USUARIO = USUARIO_SICOD_EDIT
            obj_R_Oficio_Doc.ORIGINAL_FLAG = ORIGINAL_FLAG_EDIT
            obj_R_Oficio_Doc.ID_FOLIO = ID_FOLIO_EDIT

            BusinessRules.BDA_R_OFICIO_DOC.BorrarRelacion(obj_R_Oficio_Doc)

            cargarDocumentosSicodRelacionados()
        ElseIf lblModalPostBack.Text = "EliminarOficio" Then
            If Not EDITAR Then Throw New ApplicationException("No se permite elminar documento")

            Dim maxConsecutivo As Integer = BusinessRules.BDA_R_OFICIOS.ObtenerMaximoConsecutivo
            Dim objROficios As New Entities.BDA_R_OFICIOS
            objROficios.ID_EXPEDIENTE = maxConsecutivo
            objROficios.ID_ANIO = ID_ANIO_EDIT
            objROficios.ID_AREA_OFICIO = ID_UNIDAD_ADM_EDIT
            objROficios.ID_TIPO_DOCUMENTO = ID_TIPO_DOCUMENTO_EDIT
            objROficios.I_OFICIO_CONSECUTIVO = I_OFICIO_CONSECUTIVO_EDIT
            objROficios.INICIAL_FLAG = 0
            objROficios.USUARIO_ASOCIO = USUARIO

            If Not BusinessRules.BDA_R_OFICIOS.Actualizar(objROficios) Then Throw New ApplicationException("Error desvinculando oficio, intente de nuevo")

            cargarOficiosRelacionados()
        End If
    End Sub

    Private Sub EliminarComentario()

        Try
            Dim resultado As Boolean = LogicaNegocioSICOD.BusinessRules.BDA_SEGUIMIENTO_OFICIO.ActualizarComentario(ID_SEGUIMIENTO_OFICIO, txtFechaEnvio.Text)
            CargaDatosSeguimiento(LogicaNegocioSICOD.BusinessRules.BDA_SEGUIMIENTO_OFICIO.GetAll(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO))

        Catch ex As Exception

            EscribirError(ex, "EliminarComentario")
        End Try
    End Sub

    Protected Sub btnAgregarOficio_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregarOficio.Click
        'Session("ID_ANIO") = ID_ANIO
        'Session("ID_UNIDAD_ADM") = ID_UNIDAD_ADM
        'Session("ID_TIPO_DOCUMENTO") = ID_TIPO_DOCUMENTO
        'Session("I_OFICIO_CONSECUTIVO") = I_OFICIO_CONSECUTIVO
        'Session("ID_EXPEDIENTE") = ID_EXPEDIENTE
        'Response.Redirect("~/App_Oficios/VincularOficios.aspx", False)




    End Sub

#Region "Verificar Sesión y perfil de Usuario"
    Private Sub verificaSesion()
        Dim logout As Boolean = False
        Dim Sesion As Seguridad = Nothing
        Try
            Sesion = New Seguridad
            'Verifica la sesion de usuario
            Select Case Sesion.ContinuarSesionAD()
                Case -1
                    logout = True
                Case 0, 3
                    logout = True
            End Select
        Catch ex As Exception
            EscribirError(ex, "verificaSesion")
        Finally
            If Not Sesion Is Nothing Then
                Sesion.CerrarCon()
                Sesion = Nothing
            End If
        End Try
        If logout Then
            If Request.Browser.EcmaScriptVersion.Major >= 1 Then
                Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
                Response.End()
            Else
                Response.Redirect("~/logout.aspx")
            End If
        End If
    End Sub

    Private Sub verificaPerfil()
        Dim logout As Boolean = False
        Dim Perfil As Perfil = Nothing
        Try
            Perfil = New Perfil
            'Verifica que el usuario este autorizado para ver esta página
            If Not Perfil.Autorizado("App_Oficios/Registro.aspx") Then
                logout = True
            End If

        Catch ex As Exception
            EscribirError(ex, "verificaPerfil")
        Finally
            If Not Perfil Is Nothing Then
                Perfil.CerrarCon()
                Perfil = Nothing
            End If
        End Try
        If logout Then
            If Request.Browser.EcmaScriptVersion.Major >= 1 Then
                Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
                Response.End()
            Else
                Response.Redirect("~/logout.aspx")
            End If
        End If
    End Sub

    Private Sub logOut()
        If Request.Browser.EcmaScriptVersion.Major >= 1 Then
            Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
            Response.End()
        Else
            Response.Redirect("~/logout.aspx")
        End If
    End Sub

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

    Private Sub gvVincularOficios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvVincularOficios.RowCommand
        Try
            Dim linkArchivoPDF As String = String.Empty

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            With gvVincularOficios
                ID_ANIO_EDIT = .DataKeys(index)("ID_ANIO").ToString()
                ID_TIPO_DOCUMENTO_EDIT = .DataKeys(index)("ID_TIPO_DOCUMENTO").ToString()
                I_OFICIO_CONSECUTIVO_EDIT = .DataKeys(index)("I_OFICIO_CONSECUTIVO").ToString()
                ID_UNIDAD_ADM_EDIT = .DataKeys(index)("ID_UNIDAD_ADM").ToString()
                If Not String.IsNullOrEmpty(.DataKeys(index)("T_HYP_ARCHIVOSCAN").ToString) Then linkArchivoPDF = .DataKeys(index)("T_HYP_ARCHIVOSCAN").ToString
            End With

            If e.CommandName = "Eliminar" Then
                If gvVincularOficios.DataKeys(index)("INICIAL_FLAG") = 1 Then
                    Throw New ApplicationException("Dictámen u Oficio Externo relacionado, para desvincular edite el oficio")
                Else
                    modalMensaje("¿Realmente desea desvincular el Oficio?", "EliminarOficio", , True)
                End If

            ElseIf e.CommandName = "Editar" Then
                Session("ID_ANIO") = ID_ANIO_EDIT
                Session("ID_TIPO_DOCUMENTO") = ID_TIPO_DOCUMENTO_EDIT
                Session("I_OFICIO_CONSECUTIVO") = I_OFICIO_CONSECUTIVO_EDIT
                Session("ID_UNIDAD_ADM") = ID_UNIDAD_ADM_EDIT
                Response.Redirect("~/App_Oficios/Registro.aspx?Modificar=1", False)
            ElseIf e.CommandName = "VerPDF" Then
                If Not String.IsNullOrEmpty(linkArchivoPDF) Then
                    AbreArchivoLink(linkArchivoPDF)
                Else
                    Throw New ApplicationException("No hay archivo PDF relacionado al Oficio")
                End If
            End If

        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As System.Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "gvVincularOficios_RowCommand")
        End Try
    End Sub

    Private Sub gvVincularOficios_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvVincularOficios.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rowView As DataRowView = TryCast(e.Row.DataItem, DataRowView)
            '--------------------------------------
            ' Vencimiento
            '--------------------------------------
            Dim img As Image = Nothing
            img = TryCast(e.Row.Cells(10).Controls(1), Image)
            If Not IsDBNull(rowView("ID_ESTATUS")) Then
                If CInt(rowView("ID_ESTATUS")) = OficioEstatus.Cancelado Then
                    img.ImageUrl = "~/imagenes/ATENDIDO.png"
                    img.AlternateText = "Cancelado"
                    img.ToolTip = "Cancelado"

                ElseIf CInt(rowView("ID_ESTATUS")) = OficioEstatus.Concluido Then
                    img.ImageUrl = "~/imagenes/ATENDIDO.png"
                    img.AlternateText = "Concluido"
                    img.ToolTip = "Concluido"
                Else
                    If Not IsDBNull(rowView("F_FECHA_VENCIMIENTO")) Then
                        '---------------------------------------------------
                        'Llama a la función para calcular estatus del vencimiento
                        '---------------------------------------------------
                        Dim fVencimiento As Date = CType(rowView("F_FECHA_VENCIMIENTO"), Date)
                        Dim status As String = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultaEstatusVencimiento(fVencimiento)
                        Select Case status
                            Case "Vencido"
                                img.ImageUrl = "~/imagenes/VENCIDO.png"
                                img.AlternateText = "Vencido"
                                img.ToolTip = "Vencido"
                            Case "Por Vencer"
                                img.ImageUrl = "~/imagenes/PREVENTIVO.png"
                                img.AlternateText = "Por Vencer"
                                img.ToolTip = "Por Vencer"
                            Case "Normal"
                                img.ImageUrl = "~/imagenes/statusNormal.png"
                                img.AlternateText = "Normal"
                                img.ToolTip = "Normal"
                            Case Else
                                img.Visible = False
                        End Select
                    Else
                        img.Visible = False
                    End If
                End If
            End If
        End If


        '--------------------------------------
        ' Establece la clase para la fila impar
        '--------------------------------------
        If e.Row.RowIndex Mod 2 = 1 Then e.Row.CssClass = "tr_odd"
    End Sub

    Private Sub gvVincularDocsSICOD_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvVincularDocsSICOD.RowCommand

        Dim index As Integer = Convert.ToInt32(e.CommandArgument)

        Try
            With gvVincularDocsSICOD
                USUARIO_SICOD_EDIT = .DataKeys(index)("USUARIO").ToString()
                DESTINATARIO_EDIT = .DataKeys(index)("DESTINATARIO").ToString()
                ORIGINAL_FLAG_EDIT = .DataKeys(index)("ORIGINAL_FLAG").ToString()
                ID_FOLIO_EDIT = .DataKeys(index)("ID_FOLIO").ToString()
                NOMBRE_ARCHIVO_EDIT = .DataKeys(index)("NOMBRE_ARCHIVO").ToString()
                If NOMBRE_ARCHIVO_EDIT.Trim = "" Then
                    ID_ARCHIVO_EDIT = 0
                Else
                    ID_ARCHIVO_EDIT = .DataKeys(index)("ID_ARCHIVO").ToString
                End If
                DSC_T_DOC_EDIT = .DataKeys(index)("DSC_T_DOC").ToString
            End With

            If e.CommandName = "Eliminar" Then

                modalMensaje("¿Realmente desea desvincular el documento?", "EliminarDocSICOD", , True)

            ElseIf e.CommandName = "IraFolio" Then

                Response.Redirect("~/Modifica.aspx?NumFolio=" & _
                                    HttpUtility.UrlEncode(ID_FOLIO_EDIT.ToString) & _
                                    "&nombreArch= " & HttpUtility.UrlEncode(NOMBRE_ARCHIVO_EDIT) & _
                                    "&IdArch=" & HttpUtility.UrlEncode(ID_ARCHIVO_EDIT.ToString) & _
                                    "&tDocto=" & HttpUtility.UrlDecode(DSC_T_DOC_EDIT) & _
                                    "&EsCopia=" & HttpUtility.UrlDecode(DESTINATARIO_EDIT) & _
                                    "&Usuario=" & HttpUtility.UrlDecode(USUARIO_SICOD_EDIT), False)
            ElseIf e.CommandName = "verPDFFolio" Then

                If Not String.IsNullOrEmpty(NOMBRE_ARCHIVO_EDIT) Then AbreArchivoLink(NOMBRE_ARCHIVO_EDIT, False)

            End If
        Catch ex As ApplicationException
            modalMensaje(ex.Message)
        Catch ex As System.Threading.ThreadAbortException
            '---------------------------------------------
            ' Atrapamos esta excepción porque la lanza con Response.End pero aún permite descargar el archivo.
            '---------------------------------------------
        Catch ex As Exception
            EscribirError(ex, "gvVincularDocsSICOD_RowCommand")
        End Try

    End Sub
    Private Sub gvVincularDocsSICOD_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvVincularDocsSICOD.RowDataBound
        '--------------------------------------
        ' Establece la clase para la fila impar
        '--------------------------------------
        If e.Row.RowIndex Mod 2 = 1 Then e.Row.CssClass = "tr_odd"
    End Sub

#Region "Funciones llamadas desde la página aspx durante el databinding"
    Public Function ImagenEstatusCopia(ByVal Estatus As String, ByVal Bloq As String) As String
        Try
            If Estatus = "O" And Bloq = "0" Then
                Return "~/imagenes/original.gif"
            ElseIf Estatus = "O" And Bloq = "1" Then
                Return "~/imagenes/original_disabled.gif"
            ElseIf Estatus = "C" And Bloq = "0" Then
                Return "~/imagenes/copia.gif"
            ElseIf Estatus = "C" And Bloq = "1" Then
                Return "~/imagenes/copia_disabled.gif"
            ElseIf Estatus = "TO" And Bloq = "0" Then
                Return "~/imagenes/TemplateTab2.gif"
            ElseIf Estatus = "TO" And Bloq = "1" Then
                Return "~/imagenes/TemplateTab2_disabled.gif"
            ElseIf Estatus = "TC" And Bloq = "0" Then
                Return "~/imagenes/TemplateTab1.gif"
            ElseIf Estatus = "TC" And Bloq = "1" Then
                Return "~/imagenes/TemplateTab1_disabled.gif"
            End If
        Catch ex As Exception
            EventLogWriter.EscribeEntrada("Funcion ImagenEstatus: " & ex.ToString(), EventLogEntryType.Error)
        End Try
        Return ""
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Estatus_Atender"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ImagenFechaLimite(ByVal Estatus_Atender As String)
        Try
            Dim imagenRegreso As String = String.Empty

            If Estatus_Atender = "Atendido" Then
                imagenRegreso = "~/imagenes/ATENDIDO.png"
            ElseIf Estatus_Atender = "Normal" Then
                imagenRegreso = "~/imagenes/statusNormal.png"
            ElseIf Estatus_Atender = "Por Vencer" Then
                imagenRegreso = "~/imagenes/PREVENTIVO.png"
            ElseIf Estatus_Atender = "Vencido" Then
                imagenRegreso = "~/imagenes/VENCIDO.png"
            ElseIf Estatus_Atender = "En Espera VoBo" Then
                imagenRegreso = "~/imagenes/question.png"
            ElseIf Estatus_Atender = "Atendido en Espera" Then
                imagenRegreso = "~/imagenes/question_espera.png"
            ElseIf Estatus_Atender = "Complemento" Then
                imagenRegreso = "~/imagenes/COMPLEMENTO.png"
            ElseIf Estatus_Atender = "En trámite" Then
                imagenRegreso = "~/imagenes/tramite.png"
            ElseIf Estatus_Atender = "En Proceso" Then
                imagenRegreso = "~/imagenes/PROCESO.png"
            End If
            Return imagenRegreso
        Catch ex As Exception
        End Try
        Return ""

    End Function
#End Region

    Protected Sub btnAgregarDocSICOD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregarDocSICOD.Click
        'Session("ID_ANIO") = ID_ANIO
        'Session("ID_UNIDAD_ADM") = ID_UNIDAD_ADM
        'Session("ID_TIPO_DOCUMENTO") = ID_TIPO_DOCUMENTO
        'Session("I_OFICIO_CONSECUTIVO") = I_OFICIO_CONSECUTIVO
        'Session("ID_EXPEDIENTE") = ID_EXPEDIENTE

        'Response.Redirect("~/App_Oficios/VincularDocumentosSICOD.aspx", False)
    End Sub

#Region "Funciones auxiliares documentos SICOD"

    Private Sub AgregarColumnasSharePoint(ByVal dtConsulta As DataTable)
        Try

            Dim i As Int16 = 0
            Dim x As Int16 = 0
            Dim y As Int16 = 0

            Dim leerShare As New LeerSharep
            Dim liARCH As List(Of ARCH) = New List(Of ARCH)()

            'Documento
            dtConsulta.Columns.Add(New DataColumn("NOMBRE_ARCHIVO", System.Type.GetType("System.String")))
            dtConsulta.Columns.Add(New DataColumn("ID_ARCHIVO", System.Type.GetType("System.String")))

            For a = 0 To dtConsulta.Rows.Count - 1
                Dim oad = New ARCH()
                oad.folio = dtConsulta.Rows(a).ItemArray(0)
                liARCH.Add(oad)
                oad = Nothing
            Next

            Try
                Dim listDestinatario As New List(Of String)
                leerShare.LeerFoliosDoctos(liARCH, "")

            Catch ex As Exception
                EventLogWriter.EscribeEntrada("Error de SharePoint al consultar: " & ex.ToString(), EventLogEntryType.Error)
            End Try

            For Each row As DataRow In dtConsulta.Rows

                If liARCH.Item(i).nomArch <> Nothing Then

                    If liARCH.Item(i).nomArch.ToString().Length > 30 Then

                        ID = dtConsulta.Rows(i)(0).ToString()

                        y = i

                        For cont = 0 To 1
                            If y <= dtConsulta.Rows.Count - 1 Then
                                If ID = dtConsulta.Rows(i)(0).ToString And ID = dtConsulta.Rows(y)(0).ToString() And i <> y Then
                                    dtConsulta.Rows(y)("NOM_ARCH") = liARCH.Item(y - 1).nomArch.ToString()
                                    dtConsulta.AcceptChanges()
                                Else
                                    dtConsulta.Rows(i)("NOM_ARCH") = liARCH.Item(i).nomArch.ToString()
                                    dtConsulta.AcceptChanges()
                                End If
                            End If
                            y += 1
                        Next

                    Else
                        ID = dtConsulta.Rows(i)(0).ToString()
                        y = i
                        For cont = 0 To 1
                            If y <= dtConsulta.Rows.Count - 1 Then
                                If ID = dtConsulta.Rows(i)(0).ToString And ID = dtConsulta.Rows(y)(0).ToString() And i <> y Then
                                    dtConsulta.Rows(y)("NOM_ARCH") = liARCH.Item(y - 1).nomArch.ToString()
                                    dtConsulta.AcceptChanges()

                                Else
                                    dtConsulta.Rows(i)("NOM_ARCH") = liARCH.Item(i).nomArch.ToString()
                                    dtConsulta.AcceptChanges()
                                End If
                            End If
                            y += 1
                        Next

                    End If
                    y = i

                    For cont = 0 To 1
                        If y <= dtConsulta.Rows.Count - 1 Then
                            If ID = dtConsulta.Rows(i)(0).ToString And ID = dtConsulta.Rows(y)(0).ToString() And i <> y Then
                                dtConsulta.Rows(y)("NOMBRE_ARCHIVO") = liARCH.Item(y - 1).nomArch.ToString()
                                dtConsulta.AcceptChanges()
                                dtConsulta.Rows(y)("ID_ARCHIVO") = liARCH.Item(y - 1).idArch.ToString()
                                dtConsulta.AcceptChanges()
                            Else
                                dtConsulta.Rows(i)("NOMBRE_ARCHIVO") = liARCH.Item(i).nomArch.ToString()
                                dtConsulta.AcceptChanges()
                                dtConsulta.Rows(i)("ID_ARCHIVO") = liARCH.Item(i).idArch.ToString()
                                dtConsulta.AcceptChanges()
                            End If
                        End If
                        y += 1
                    Next

                End If
                i += 1
            Next

        Catch ex As Exception
            EscribirError(ex, "AgregarColumnasSharePoint")
        End Try
    End Sub

    Private Sub AgregarColumnasAnexo(ByVal dtConsulta As DataTable)

        Dim i As Int16 = 0
        Dim x As Int16 = 0

        Dim Con As Conexion = New Conexion()

        dtConsulta.Columns.Add(New DataColumn("ANEXO", System.Type.GetType("System.String")))
        dtConsulta.Columns.Add(New DataColumn("NOM_ARCH", System.Type.GetType("System.String")))

        For Each row As DataRow In dtConsulta.Rows

            If Con.BusquedaReg("BDA_R_DOC_ANEXO", "ID_FOLIO = " & row("ID_FOLIO").ToString().Trim()) Then
                row("ANEXO") = "Si"
            Else
                row("ANEXO") = "No"
            End If


        Next

        dtConsulta.AcceptChanges()


    End Sub
#End Region


#Region "Transicion a modal bandejas como expediente"

    Private Sub lnkVentanaOficio_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkVentanaOficio.Click

        If Session(LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.SessionAtencionResult) IsNot Nothing Then

            Dim Con As Conexion = Nothing
            Dim tran As Odbc.OdbcTransaction = Nothing
            Dim Sesion As Seguridad = Nothing
            Dim Campos As String
            Dim Valores As String
            Dim continua As Boolean = False
            Dim fechaVencimientoValidacion As String = "NULL"
            Dim _dtRE As DataTable
            Dim Dr As Odbc.OdbcDataReader = Nothing
            Dim LigarNuevo As Boolean = True

            Dim ExpedientePaso As String = hdnExpediente.Value

            Try

                Sesion = New Seguridad
                Con = New Conexion()
                Sesion.BitacoraInicia("Agrega Oficio Expediente ", Con)
                tran = Con.BeginTran()

                'lo que viene en la variable de sesion es la llave del (los) oficio seleccionado o creado separados por pipe y asterisco:
                '= ID_UNIDAD_ADM | ID_TIPO_DOCUMENTO | ID_ANIO | I_OFICIO_CONSECUTIVO * ID_UNIDAD_ADM | ID_TIPO_DOCUMENTO | ID_ANIO | I_OFICIO_CONSECUTIVO .....

                Dim Oficios() As String = Session(LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.SessionAtencionResult).ToString.Split("*")

                For Each oficio As String In Oficios

                    If oficio.Trim <> "" Then


                        ' Llave del oficio que viene
                        Dim key = oficio.Split("|")



                        ' Verificamos si el oficio que viene ya tiene expediente
                        _dtRE = LogicaNegocioSICOD.BusinessRules.BDA_R_EXPEDIENTE_OFICIO.GetByOFICIO(Convert.ToInt32(key(0)), _
                                                                            Convert.ToInt32(key(1)), _
                                                                            Convert.ToInt32(key(2)), _
                                                                            Convert.ToInt32(key(3)))

                        _dtRE.DefaultView.RowFilter = "ISORIGINAL_FLAG=1"

                        ' si el oficio que viene no tiene expediente, hay que ligarlo
                        LigarNuevo = _dtRE.DefaultView.ToTable.Rows.Count = 0


                        ' Si el oficio actual no tiene expediente
                        If hdnExpediente.Value = 0 Then


                            ' el oficio que viene ya tiene expediente
                            If Not LigarNuevo Then

                                ' El que viene ya tiene expediente
                                ExpedientePaso = _dtRE.DefaultView.ToTable.Rows(0)("ID_EXPEDIENTE").ToString()

                            Else

                                ' El que viene no tiene expediente, hay que crearlo 
                                Dr = Con.Consulta("SELECT ISNULL(MAX(ID_EXPEDIENTE),0)+1 AS ID_EXPEDIENTE FROM " & Conexion.Owner & "BDA_C_EXPEDIENTE", tran)
                                Dr.Read()
                                ExpedientePaso = Dr.Item("ID_EXPEDIENTE").ToString
                                Dr.Close()

                                '' creamos expediente
                                Campos = "ID_EXPEDIENTE,DSC_EXPEDIENTE,USUARIO_CREACION,VIG_FLAG,FECH_INI_VIG"
                                Valores = ExpedientePaso & ", '', '" & Session("Usuario") & "', 1, GETDATE()"
                                If Not Con.Insertar(Conexion.Owner & "BDA_C_EXPEDIENTE", Campos, Valores, tran) Then

                                    Throw New Exception("No fué posible crear el expediente")

                                End If


                                'GUARDAMOS EN CREACION BITACORA PARA OFICIO
                                Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                                    ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                                Valores = ID_UNIDAD_ADM.ToString & ", " & ID_TIPO_DOCUMENTO.ToString & ", " & ID_ANIO.ToString & ", " & I_OFICIO_CONSECUTIVO.ToString & ", '" & _
                                    Session("Usuario") & "','" & Session("Usuario") & "','" & Session("Usuario") & "',GETDATE(),13,'NUEVO EXPEDIENTE CREADO: " & ExpedientePaso & "'," & fechaVencimientoValidacion

                                If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran) Then

                                    Throw New Exception("No fué posible guardar en bitácora oficios")

                                End If


                            End If


                            ' ligas oficio actual al expediente
                            Dim _dtO As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

                            fechaVencimientoValidacion = "NULL"
                            If Not IsDBNull(_dtO.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                                fechaVencimientoValidacion = "'" & CType(_dtO.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
                            End If


                            ' Guardar relacion expediente-oficio actual
                            Campos = "ID_EXPEDIENTE,ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_REGISTRO,VIG_FLAG,FECH_INI_VIG"
                            Valores = ExpedientePaso & ", " & ID_UNIDAD_ADM.ToString & ", " & ID_TIPO_DOCUMENTO.ToString & ", " & ID_ANIO.ToString & ", " _
                                & I_OFICIO_CONSECUTIVO.ToString & ", '" & Session("Usuario") & "', 1, GETDATE()"
                            If Not Con.Insertar(Conexion.Owner & "BDA_R_EXPEDIENTE_OFICIO", Campos, Valores, tran) Then

                                Throw New Exception("No fué posible agregar el oficio actual al expediente")

                            End If



                            'GUARDAMOS EN BITACORA PARA OFICIO ACTUAL
                            Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                                ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                            Valores = ID_UNIDAD_ADM.ToString & ", " & ID_TIPO_DOCUMENTO.ToString & ", " & ID_ANIO.ToString & ", " & I_OFICIO_CONSECUTIVO.ToString & ", '" & _
                                Session("Usuario") & "','" & Session("Usuario") & "','" & Session("Usuario") & "',GETDATE(),13,'AGREGADO AL EXPEDIENTE Num. " & ExpedientePaso & "'," & fechaVencimientoValidacion

                            If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran) Then

                                Throw New Exception("No fué posible guardar en bitácora de oficios")

                            End If



                        End If



                        ID_EXPEDIENTE = CInt(ExpedientePaso)


                        If LigarNuevo Then


                            Dim _dt As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(key(2), _
                                                      key(0), _
                                                      key(1), _
                                                      key(3))

                            fechaVencimientoValidacion = "NULL"
                            If Not IsDBNull(_dt.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                                fechaVencimientoValidacion = "'" & CType(_dt.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
                            End If

                            ' Guardar relacion expediente-oficio
                            Campos = "ID_EXPEDIENTE,ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_REGISTRO,VIG_FLAG,FECH_INI_VIG"
                            Valores = ID_EXPEDIENTE.ToString() & ", " & oficio.Replace("|", ",") & ", '" & Session("Usuario") & "', 1, GETDATE()"

                            If Not Con.Insertar(Conexion.Owner & "BDA_R_EXPEDIENTE_OFICIO", Campos, Valores, tran) Then

                                Throw New Exception("No fué posible agregar el oficio al expediente")

                            End If


                            'GUARDAMOS EN BITACORA PARA OFICIO
                            Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                                ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                            Valores = key(0) & "," & _
                                key(1) & "," & _
                                key(2) & "," & _
                                key(3) & ",'" & _
                                Session("Usuario") & "','" & Session("Usuario") & "','" & Session("Usuario") & "',GETDATE(),13,'AGREGADO AL EXPEDIENTE Num. " & ID_EXPEDIENTE.ToString() & "'," & fechaVencimientoValidacion

                            If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran) Then

                                Throw New Exception("No fué posible guardar en bitácora de oficios")

                            End If


                        End If


                    End If

                Next

                tran.Commit()
                Sesion.BitacoraFinaliza(True)


                Response.Redirect("~/App_Oficios/Seguimiento.aspx", False)


            Catch ex As Exception

                tran.Rollback()
                Sesion.BitacoraFinaliza(False)
                modalMensaje(ex.Message)

            Finally

                If Not IsNothing(Con) Then
                    Con.Cerrar()
                End If

                '' limpia variable de session
                Session(LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.SessionAtencionResult) = Nothing


            End Try

        End If

    End Sub

    Private Sub lnkVentanaFolio_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkVentanaFolio.Click

        If Session(LogicaNegocioSICOD.BusinessRules.BDA_C_EXPEDIENTE.SessionExpResult) IsNot Nothing Then

            Dim Con As Conexion = Nothing
            Dim tran As Odbc.OdbcTransaction = Nothing
            Dim Sesion As Seguridad = Nothing
            Dim Campos As String
            Dim Valores As String
            Dim LigarNuevo As Boolean = True
            Dim _dtRE As DataTable
            Dim Dr As Odbc.OdbcDataReader = Nothing
            Dim fechaVencimientoValidacion As String = "NULL"

            Dim ExpedientePaso As String = hdnExpediente.Value

            Try

                Sesion = New Seguridad
                Con = New Conexion()
                Sesion.BitacoraInicia("Agrega Folio Expediente ", Con)
                tran = Con.BeginTran()


                Dim Folios() As String = Session(LogicaNegocioSICOD.BusinessRules.BDA_C_EXPEDIENTE.SessionExpResult).ToString.Split("*")
                Dim NumFolio As String = ""


                For Each folio As String In Folios

                    If folio.Trim <> "" Then

                        ' Verificamos si el folio que viene ya tiene expediente
                        _dtRE = LogicaNegocioSICOD.BusinessRules.BDA_R_EXPEDIENTE_INFO_DOC.GetByFOLIO(Convert.ToInt32(folio))

                        _dtRE.DefaultView.RowFilter = "ISORIGINAL_FLAG=1"

                        ' si el folio que viene no tiene expediente, hay que ligarlo
                        LigarNuevo = _dtRE.DefaultView.ToTable.Rows.Count = 0


                        ' Si el oficio actual no tiene expediente
                        If hdnExpediente.Value = 0 Then


                            ' el folio que viene ya tiene expediente
                            If Not LigarNuevo Then

                                ' El que viene ya tiene expediente
                                ExpedientePaso = _dtRE.DefaultView.ToTable.Rows(0)("ID_EXPEDIENTE").ToString()

                            Else

                                ' El que viene no tiene expediente, hay que crearlo 
                                Dr = Con.Consulta("SELECT ISNULL(MAX(ID_EXPEDIENTE),0)+1 AS ID_EXPEDIENTE FROM " & Conexion.Owner & "BDA_C_EXPEDIENTE", tran)
                                Dr.Read()
                                ExpedientePaso = Dr.Item("ID_EXPEDIENTE").ToString
                                Dr.Close()

                                '' creamos expediente
                                Campos = "ID_EXPEDIENTE,DSC_EXPEDIENTE,USUARIO_CREACION,VIG_FLAG,FECH_INI_VIG"
                                Valores = ExpedientePaso & ", '', '" & Session("Usuario") & "', 1, GETDATE()"
                                If Not Con.Insertar(Conexion.Owner & "BDA_C_EXPEDIENTE", Campos, Valores, tran) Then

                                    Throw New Exception("No fué posible crear el expediente")

                                End If


                                'GUARDAMOS EN CREACION BITACORA PARA OFICIO
                                Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                                    ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                                Valores = ID_UNIDAD_ADM.ToString & ", " & ID_TIPO_DOCUMENTO.ToString & ", " & ID_ANIO.ToString & ", " & I_OFICIO_CONSECUTIVO.ToString & ", '" & _
                                    Session("Usuario") & "','" & Session("Usuario") & "','" & Session("Usuario") & "',GETDATE(),13,'NUEVO EXPEDIENTE CREADO: " & ExpedientePaso & "'," & fechaVencimientoValidacion

                                If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran) Then

                                    Throw New Exception("No fué posible guardar en bitácora oficios")

                                End If


                            End If


                            ' ligas oficio actual al expediente
                            Dim _dtO As DataTable = LogicaNegocioSICOD.BusinessRules.BDA_OFICIO.ConsultarOficioPorLlave(ID_ANIO, ID_UNIDAD_ADM, ID_TIPO_DOCUMENTO, I_OFICIO_CONSECUTIVO)

                            fechaVencimientoValidacion = "NULL"
                            If Not IsDBNull(_dtO.Rows(0)("F_FECHA_VENCIMIENTO")) Then
                                fechaVencimientoValidacion = "'" & CType(_dtO.Rows(0)("F_FECHA_VENCIMIENTO"), DateTime).ToString("yyyyMMdd") & "'"
                            End If


                            ' Guardar relacion expediente-oficio actual
                            Campos = "ID_EXPEDIENTE,ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_REGISTRO,VIG_FLAG,FECH_INI_VIG"
                            Valores = ExpedientePaso & ", " & ID_UNIDAD_ADM.ToString & ", " & ID_TIPO_DOCUMENTO.ToString & ", " & ID_ANIO.ToString & ", " _
                                & I_OFICIO_CONSECUTIVO.ToString & ", '" & Session("Usuario") & "', 1, GETDATE()"
                            If Not Con.Insertar(Conexion.Owner & "BDA_R_EXPEDIENTE_OFICIO", Campos, Valores, tran) Then

                                Throw New Exception("No fué posible agregar el oficio actual al expediente")

                            End If



                            'GUARDAMOS EN BITACORA PARA OFICIO ACTUAL
                            Campos = "ID_AREA_OFICIO,ID_TIPO_DOCUMENTO,ID_ANIO,I_OFICIO_CONSECUTIVO,USUARIO_SISTEMA,USUARIO_ORIGEN,USUARIO_DESTINO,FECH_MOVIMIENTO" & _
                                ",ID_MOVIMIENTO,DESCRIPCION,F_FECHA_VENCIMIENTO"
                            Valores = ID_UNIDAD_ADM.ToString & ", " & ID_TIPO_DOCUMENTO.ToString & ", " & ID_ANIO.ToString & ", " & I_OFICIO_CONSECUTIVO.ToString & ", '" & _
                                Session("Usuario") & "','" & Session("Usuario") & "','" & Session("Usuario") & "',GETDATE(),13,'AGREGADO AL EXPEDIENTE Num. " & ExpedientePaso & "'," & fechaVencimientoValidacion

                            If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA_OFICIOS", Campos, Valores, tran) Then

                                Throw New Exception("No fué posible guardar en bitácora de oficios")

                            End If




                        End If


                        ID_EXPEDIENTE = CInt(ExpedientePaso)


                        If LigarNuevo Then


                            ' Guardamos relacion de expediente-folio
                            Campos = "ID_EXPEDIENTE,ID_FOLIO,USUARIO_REGISTRO,VIG_FLAG,FECH_INI_VIG"
                            Valores = ID_EXPEDIENTE.ToString() & ", " & folio & ", '" & Session("Usuario") & "', 1, GETDATE()"
                            If Not Con.Insertar(Conexion.Owner & "BDA_R_EXPEDIENTE_INFO_DOC", Campos, Valores, tran) Then

                                Throw New Exception("No fué posible agregar el documento al expediente")

                            End If

                            ' Guardamos en bitacora
                            Campos = "ID_FOLIO, USUARIO_SISTEMA, ID_MOVIMIENTO, DESCRIPCION, USUARIO_ORIGEN, USUARIO_DESTINO"
                            Valores = folio + ", '" & Session("Usuario") & "', 13, 'AGREGADO AL EXPEDIENTE Num. " & ID_EXPEDIENTE.ToString() & "', '" & Session("Usuario") & "', '" & Session("Usuario") & "'"

                            If Not Con.Insertar(Conexion.Owner & "BDA_BITACORA", Campos, Valores, tran) Then

                                Throw New Exception("No fué posible guardar en bitácora")

                            End If



                        End If


                    End If

                Next


                tran.Commit()
                Sesion.BitacoraFinaliza(True)


                Response.Redirect("~/App_Oficios/Seguimiento.aspx", False)


            Catch ex As Exception

                tran.Rollback()
                Sesion.BitacoraFinaliza(False)
                modalMensaje(ex.Message)

            Finally

                If Not IsNothing(Con) Then
                    Con.Cerrar()
                End If

                '' limpia variable de session
                Session(LogicaNegocioSICOD.BusinessRules.BDA_C_EXPEDIENTE.SessionExpResult) = Nothing


            End Try


        End If

    End Sub

#End Region




End Class