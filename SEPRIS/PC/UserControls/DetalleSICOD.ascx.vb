Imports System.Web.Configuration
Imports System.Data

Public Class DetalleSICOD
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO_SICOD")
        End Get
    End Property

    Public ReadOnly Property FechaRecepcion
        Get
            Return Session("FechaRecepcion")
        End Get
    End Property


    Private Function ConexionSICOD() As Conexion.SQLServer
        Dim conexion As Conexion.SQLServer = Nothing
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNencAux").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNencAux").ToString())
        End If

        Return conexion
    End Function

    Public Sub Inicializar()

        btnVerDoc.Enabled = True

        Dim Con As Conexion.SQLServer
        Dim dsF As New DataSet
        '

        'Calcula caracteres restantes
        If TxtAsunto.Text.Length() <> 600 And TxtAsunto.Text.Length() > 0 And lblAsuntoCaracteres.Text.Length() <> 0 Then
            lblAsuntoCaracteres.Text = 600 - TxtAsunto.Text.Length()
        ElseIf TxtAsunto.Text.Length() = 600 Then
            lblAsuntoCaracteres.Text = 0
        End If

        If Not IsPostBack Then

            Try

                Con = ConexionSICOD()
                Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."

                Dim DsTDocto As DataSet = Con.ConsultarDS("SELECT ID_T_DOC, DSC_T_DOC FROM " & Owner & "BDA_C_T_DOC WHERE VIG_FLAG = 1")
                ddlTDocto.DataValueField = "ID_T_DOC"
                ddlTDocto.DataTextField = "DSC_T_DOC"
                ddlTDocto.DataSource = DsTDocto
                ddlTDocto.DataBind()
                If ddlTDocto.Items.Count > 0 Then
                    ddlTDocto.Items.Insert(0, New ListItem("-Seleccione uno-", "-1"))
                Else
                    ddlTDocto.Items.Insert(0, New ListItem("No hay opciones", "-1"))
                End If

                ddlArea.DataSource = Con.ConsultarDS("SELECT ID_UNIDAD_ADM, CAST(I_CODIGO_AREA AS VARCHAR) + ' - ' + DSC_UNIDAD_ADM AS [DSC_CODIGO_UNIDAD_ADM] FROM " & Owner & "BDA_C_UNIDAD_ADM WHERE VIG_FLAG = 1 AND ID_T_UNIDAD_ADM = " & rdbEstructura.SelectedValue & " ORDER BY I_CODIGO_AREA")
                ddlArea.DataValueField = "ID_UNIDAD_ADM"
                ddlArea.DataTextField = "DSC_CODIGO_UNIDAD_ADM"
                ddlArea.DataBind()

                If ddlArea.Items.Count > 0 Then
                    ddlArea.Items.Insert(0, New ListItem("-Seleccione una-", "-1"))
                Else
                    ddlArea.Items.Insert(0, New ListItem("No hay opciones", "-1"))

                End If
                ddlAreaCC.DataSource = Con.ConsultarDS("SELECT ID_UNIDAD_ADM, CAST(I_CODIGO_AREA AS VARCHAR) + ' - ' + DSC_UNIDAD_ADM AS [DSC_CODIGO_UNIDAD_ADM] FROM " & Owner & "BDA_C_UNIDAD_ADM WHERE VIG_FLAG = 1 AND ID_T_UNIDAD_ADM = " & rdbEstructuraCC.SelectedValue & " ORDER BY I_CODIGO_AREA")

                ddlAreaCC.DataValueField = "ID_UNIDAD_ADM"
                ddlAreaCC.DataTextField = "DSC_CODIGO_UNIDAD_ADM"
                ddlAreaCC.DataBind()

                If ddlAreaCC.Items.Count > 0 Then
                    ddlAreaCC.Items.Insert(0, New ListItem("-Seleccione una-", "-1"))
                Else
                    ddlAreaCC.Items.Insert(0, New ListItem("No hay opciones", "-1"))

                End If

                Dim DsFol As DataSet = Con.ConsultarDS("SELECT FECH_REGISTRO, DSC_REFERENCIA, CONVERT(VARCHAR(10),FECH_DOC,103) FECH_DOC, CONVERT(VARCHAR(10),FECH_RECEPCION,103) FECH_RECEPCION, ID_T_DOC, USUARIO_DESTINATARIO, ID_UNIDAD_ADM, " &
                                            "ARCHIVO, ID_REMITENTE, DSC_NOMB_FIRMNT, DSC_AP_PAT_FIRMNT, DSC_AP_MAT_FIRMNT, " &
                                            "DSC_CARGO_FIRMNT, DSC_ASUNTO, isnull(DSC_NUM_OFICIO,'') DSC_NUM_OFICIO, isnull(COPIA_CONOCIMIENTO,0) COPIA_CONOCIMIENTO, ID_SISTEMA FROM " & Owner & "BDA_INFO_DOC WHERE ID_FOLIO = " & Folio.ToString())
                Dim ds As DataSet = Con.ConsultarDS("SELECT  DSC_REMITENTE FROM dbo.BDA_C_REMITENTE WHERE ID_REMITENTE = " & DsFol.Tables(0).Rows(0)("ID_REMITENTE") & "")
                txtFechaRegistro.Text = IIf(IsDBNull(DsFol.Tables(0).Rows(0)("FECH_REGISTRO")), "", DsFol.Tables(0).Rows(0)("FECH_REGISTRO"))
                txtReferencia.Text = IIf(IsDBNull(DsFol.Tables(0).Rows(0)("DSC_REFERENCIA")), "", DsFol.Tables(0).Rows(0)("DSC_REFERENCIA"))
                txtFechaDocto.Text = IIf(IsDBNull(DsFol.Tables(0).Rows(0)("FECH_DOC")), "", DsFol.Tables(0).Rows(0)("FECH_DOC"))
                txtFechaRecepcion.Text = IIf(IsDBNull(DsFol.Tables(0).Rows(0)("FECH_RECEPCION")), "", DsFol.Tables(0).Rows(0)("FECH_RECEPCION"))

                Session("FechaRecepcion") = txtFechaRecepcion.Text


                lnkDocumento.Text = IIf(IsDBNull(DsFol.Tables(0).Rows(0)("ARCHIVO")), "", DsFol.Tables(0).Rows(0)("ARCHIVO"))

                Session("DocumentoPC-SICOD") = lnkDocumento.Text

                txtFirmanteNombre.Text = IIf(IsDBNull(DsFol.Tables(0).Rows(0)("DSC_NOMB_FIRMNT")), "", DsFol.Tables(0).Rows(0)("DSC_NOMB_FIRMNT"))
                txtApPaterno.Text = IIf(IsDBNull(DsFol.Tables(0).Rows(0)("DSC_AP_PAT_FIRMNT")), "", DsFol.Tables(0).Rows(0)("DSC_AP_PAT_FIRMNT"))
                txtApMaterno.Text = IIf(IsDBNull(DsFol.Tables(0).Rows(0)("DSC_AP_MAT_FIRMNT")), "", DsFol.Tables(0).Rows(0)("DSC_AP_MAT_FIRMNT"))
                TxtCargoFirmante.Text = IIf(IsDBNull(DsFol.Tables(0).Rows(0)("DSC_CARGO_FIRMNT")), "", DsFol.Tables(0).Rows(0)("DSC_CARGO_FIRMNT"))
                TxtAsunto.Text = IIf(IsDBNull(DsFol.Tables(0).Rows(0)("DSC_ASUNTO")), "", DsFol.Tables(0).Rows(0)("DSC_ASUNTO"))
                txtNumeroOficio.Text = IIf(IsDBNull(DsFol.Tables(0).Rows(0)("DSC_NUM_OFICIO")), "", DsFol.Tables(0).Rows(0)("DSC_NUM_OFICIO"))
                lblAsuntoCaracteres.Text = lblAsuntoCaracteres.Text - TxtAsunto.Text.Length()
                ddlTDocto.SelectedValue = IIf(IsDBNull(DsFol.Tables(0).Rows(0)("ID_T_DOC")), "", DsFol.Tables(0).Rows(0)("ID_T_DOC"))
                ddlArea.SelectedValue = IIf(IsDBNull(DsFol.Tables(0).Rows(0)("ID_UNIDAD_ADM")), "", DsFol.Tables(0).Rows(0)("ID_UNIDAD_ADM"))
                chkCopiaConocimiento.Checked = Convert.ToBoolean(DsFol.Tables(0).Rows(0)("COPIA_CONOCIMIENTO"))
                txtRemitente.Text = ds.Tables(0).Rows(0)("DSC_REMITENTE")

                Try

                    'Destinatario

                    Dim dtDestinatario As DataTable = Con.ConsultarDT("SELECT USUARIO, ESTATUS_TRAMITE FROM " & Owner & "BDA_R_DOC_COPIAS WHERE ORIGINAL_FLAG = 1 AND ID_FOLIO = " & Folio.ToString())
                    Dim dtUnidadFuncional As DataTable = Con.ConsultarDT("SELECT ID_UNIDAD_ADM FROM " & Owner & "BDA_R_USUARIO_UNIDAD_ADM WHERE USUARIO = '" & dtDestinatario.Rows(0)("USUARIO") & "' AND ID_T_UNIDAD_ADM = 2")
                    ddlArea.SelectedValue = IIf(IsDBNull(dtUnidadFuncional(0)("ID_UNIDAD_ADM")), "", dtUnidadFuncional(0)("ID_UNIDAD_ADM"))

                    ddlDestinatario.DataSource = Con.ConsultarDT("SELECT U.USUARIO, U.NOMBRE + ' ' + ISNULL(U.APELLIDOS,'') AS NOMBRE FROM  BDS_USUARIO  U JOIN BDA_R_USUARIO_UNIDAD_ADM UUA ON U.USUARIO = UUA.USUARIO WHERE UUA.ID_UNIDAD_ADM = " & ddlArea.SelectedValue & " AND UUA.ID_T_UNIDAD_ADM = " & rdbEstructura.SelectedValue & " ORDER BY U.NOMBRE + ' ' + ISNULL(U.APELLIDOS,'')")
                    ddlDestinatario.DataTextField = "NOMBRE"
                    ddlDestinatario.DataValueField = "USUARIO"
                    ddlDestinatario.DataBind()
                    ddlDestinatario.SelectedValue = dtDestinatario.Rows(0)("USUARIO")


                    Dim dtCC As DataTable = Con.ConsultarDT("SELECT U.USUARIO, U.NOMBRE + ' ' + ISNULL(U.APELLIDOS, '') NOMBRE, C.ID_UNIDAD_ADM FROM " & Owner & "BDA_R_DOC_COPIAS C JOIN " & Owner & "BDS_USUARIO U ON C.USUARIO = U.USUARIO  WHERE C.ORIGINAL_FLAG = 0 AND C.ID_FOLIO = " & Folio.ToString() & " ORDER BY U.USUARIO, U.NOMBRE + ' ' + ISNULL(U.APELLIDOS, '')")
                    ViewState("TotCC") = dtCC.Rows.Count



                    For Each rowCC In dtCC.Rows
                        dtUnidadFuncional = Con.ConsultarDT("SELECT ID_UNIDAD_ADM FROM " & Owner & "BDA_R_USUARIO_UNIDAD_ADM WHERE USUARIO = '" & rowCC("USUARIO") & "' AND ID_T_UNIDAD_ADM = 2 AND VIG_FLAG = 1")

                        Try
                            ddlAreaCC.SelectedValue = dtUnidadFuncional(0)("ID_UNIDAD_ADM")
                        Catch ex As Exception
                            ddlAreaCC.SelectedIndex = 0
                        End Try

                        ListFuncionarioCC.Items.Add(New ListItem(rowCC("NOMBRE"), rowCC("USUARIO")))
                    Next


                    'ddlAreaCC_SelectedIndexChanged(sender, e)


                    For Each rowCC In dtCC.Rows
                        ListFuncionario.Items.Remove(New ListItem(rowCC("NOMBRE"), rowCC("USUARIO")))
                    Next

                    Dim Ds_Anexos = Con.ConsultarDS("SELECT A.ID_ANEXO, DSC_ANEXO, DSC_OTRO FROM " & Owner & "BDA_R_DOC_ANEXO A, " & Owner & "BDA_C_ANEXO B" &
                                          " WHERE A.ID_FOLIO = " & Folio.ToString() & " AND A.ID_ANEXO = B.ID_ANEXO")
                    ViewState("TotAnexos") = Ds_Anexos.Tables(0).Rows.Count
                    If Ds_Anexos.Tables(0).Rows.Count > 0 Then
                        For Each Fila In Ds_Anexos.Tables(0).Rows
                            Select Case Fila("ID_ANEXO")
                                Case 1
                                    chkCarpeta.Checked = True
                                Case 2
                                    chkCDDVD.Checked = True
                                Case 3
                                    chkSobreC.Checked = True
                                Case 4
                                    chkPaquete.Checked = True
                                Case 5
                                    chkRevistas.Checked = True
                                Case 6
                                    TxtOtros.Visible = True
                                    chkOtros.Checked = True
                                    TxtOtros.Text = Fila("DSC_OTRO")
                            End Select
                        Next
                    End If

                    Con.CerrarConexion()

                Catch ex As Exception
                    'catch_cone(ex, "No es posible leer los datos del folio")
                End Try


            Catch ex As Exception
                'catch_cone(ex, "No es posible leer los datos del folio")
            Finally

            End Try

        End If

    End Sub

    Protected Sub lnkDocumento_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub btnVerDoc_Click(sender As Object, e As EventArgs) Handles btnVerDoc.Click
        Dim usuario As String = Session("Usuario")
        Dim passwd As String = Session("Password")
        Dim dominio As String
        Dim nom_archivo As String = String.Empty
        Dim biblioteca As String
        Dim ServSharepoint As String
        Dim cliente As System.Net.WebClient = New System.Net.WebClient
        Dim enc As New YourCompany.Utils.Encryption.Encryption64


        nom_archivo = lnkDocumento.Text

        If nom_archivo <> "Sin imagen" Then
            Dim Archivo() As Byte = Nothing
            Dim filename As String = "attachment; filename=" & nom_archivo

            Try
                usuario = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioSpSICOD")
                passwd = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("PassEncSpSICOD"), "webCONSAR")
                ServSharepoint = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointServerSICOD"), "webCONSAR")
                dominio = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("DomainSICOD"), "webCONSAR")
                biblioteca = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("DocLibrarySICOD"), "webCONSAR")

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


                'Response.ContentType = "application/vnd.ms-word"

                Response.AddHeader("content-disposition", filename)

                Response.BinaryWrite(Archivo)

                Response.End()
            End If

        End If
    End Sub
End Class