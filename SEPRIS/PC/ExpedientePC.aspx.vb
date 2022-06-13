Imports Entities

Public Class ExpedientePC
    Inherits System.Web.UI.Page

    Public ReadOnly Property Folio
        Get
            Return Session("ID_FOLIO")
        End Get
    End Property

    Public ReadOnly Property Usuario
        Get
            Return TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario
        End Get
    End Property
    Public ReadOnly Property Area
        Get
            Return TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdArea
        End Get
    End Property

    Public ReadOnly Property PasoActual
        Get
            Dim PC As New Entities.PC(Folio)
            Return PC.IdPaso
        End Get
    End Property

    Public ReadOnly Property PC As Entities.PC
        Get
            Return DirectCast(Session("PC"), Entities.PC)
        End Get
    End Property

    Public ReadOnly Property DocumentoPC
        Get
            Return Session("DocumentoPC-SICOD")
        End Get
    End Property

    Public Property puObjUsuario As Entities.Usuario
        Get
            If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
                Return CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Entities.Usuario)
            Session(Entities.Usuario.SessionID) = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session("usuario") = TryCast(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario

        If Not Page.IsPostBack Then

            CargarFiltros()

            btnActulizarGrid_Click(sender, e)

            Dim enc As New YourCompany.Utils.Encryption.Encryption64
            Dim Usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodUser")
            Dim Password As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
            Dim Dominio As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")
            Dim mycredentialCache As System.Net.CredentialCache = New System.Net.CredentialCache()
            Dim credentials As System.Net.NetworkCredential = New System.Net.NetworkCredential(Usuario, Password, Dominio)
            Dim proxySICOD As New WR_SICOD.ws_SICOD
            proxySICOD.Credentials = credentials
            proxySICOD.ConnectionGroupName = "SEPRIS"

            Try
                ddlPuestoDestinatario.DataSource = proxySICOD.GetCatalogoCargoDestinatarioOficios(False)
                ddlPuestoDestinatario.DataTextField = "Value"
                ddlPuestoDestinatario.DataValueField = "Key"
                ddlPuestoDestinatario.DataBind()
                ddlPuestoDestinatario.Items.Insert(0, New ListItem("-Seleccionar-", "-1"))
            Catch ex As Exception
                Throw ex
            End Try

            Dim Todas As DataTable = proxySICOD.GetUnidadesAdministrativas(WR_SICOD.UnidadAdministrativaTipo.Funcional, WR_SICOD.UnidadAdministrativaEstatus.Activo).Tables(0)
            'Dim Filtrado As DataTable = Todas.Select("ID_UNIDAD_ADM IN (" + ObtenerAreasSicod(Me.Area) + ")").CopyToDataTable()
            Generales.CargarCombo(ddlAreaOficio, Todas, "DSC_CODIGO_UNIDAD_ADM", "ID_UNIDAD_ADM")
            Generales.CargarCombo(ddlAreaFirma, Todas, "DSC_CODIGO_UNIDAD_ADM", "ID_UNIDAD_ADM")
            Generales.CargarCombo(ddlAreaRubrica, Todas, "DSC_CODIGO_UNIDAD_ADM", "ID_UNIDAD_ADM")

            Dim objParametroDestinatario As New Parametros(83)
            txtDestinatarioOficio.Text = proxySICOD.GetNombreCompleto(objParametroDestinatario.ValorParametro)
            ddlPuestoDestinatario.SelectedValue = ""

        End If

    End Sub
    Public Shared Function ObtenerAreasSicod(idAreaSupervisar As String) As String
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable

        data = conexion.ConsultarDT("SELECT T_DSC_VALOR FROM DBO.BDS_C_GR_PARAMETRO WHERE T_DSC_PARAMETRO = 'AREA_" + idAreaSupervisar + "'")
        conexion.CerrarConexion()
        If (data.Rows.Count > 0) Then
            Return data.Rows(0)("T_DSC_VALOR").ToString()
        Else
            Return ""
        End If
    End Function
    Private Sub CargarFiltros()

        ucFiltroExp.SessionID = "Expediente"
        ucFiltroExp.resetSession()

        ucFiltroExp.AddFilter("Paso                ", ucFiltro.AcceptedControls.DropDownList, BandejaPC.ObtenerPasos, "T_DSC_PASO", "N_ID_PASO", ucFiltro.DataValueType.IntegerType)
        ucFiltroExp.AddFilter("Documento           ", ucFiltro.AcceptedControls.DropDownList, BandejaPC.ObtenerPasos, "T_DSC_PASO", "N_ID_PASO", ucFiltro.DataValueType.IntegerType)
        ucFiltroExp.AddFilter("Documentos Adjuntos ", ucFiltro.AcceptedControls.DropDownList, BandejaPC.ObtenerPasos, "T_DSC_PASO", "N_ID_PASO", ucFiltro.DataValueType.IntegerType)
        ucFiltroExp.AddFilter("Núm. Oficio SICOD   ", ucFiltro.AcceptedControls.DropDownList, BandejaPC.ObtenerPasos, "T_DSC_PASO", "N_ID_PASO", ucFiltro.DataValueType.IntegerType)

        ucFiltroExp.LoadDDL("ExpedientePC.aspx")

    End Sub


    Private Sub gvExpedientePC_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvExpedientePC.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim btnAgregarDocumento As ImageButton = CType(e.Row.FindControl("btnAgregarDocumento"), ImageButton)
            Dim btnReemplazarDocumento As ImageButton = CType(e.Row.FindControl("btnReemplazarDocumento"), ImageButton)
            Dim btnRegistroSICOD As Button = CType(e.Row.FindControl("btnRegistroSICOD"), Button)
            Dim btnBuscarSICOD As Button = CType(e.Row.FindControl("btnBuscarSICOD"), Button)

            btnAgregarDocumento.OnClientClick = "SubirArchivo(" + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + "); return false;"
            btnReemplazarDocumento.OnClientClick = "ReemplazarArchivo(" + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + "); return false;"
            btnRegistroSICOD.OnClientClick = "RegistroSICOD(" + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", -1, '" + e.Row.DataItem("T_OFICIO_SICOD").ToString() + "'); return false;"

            btnReemplazarDocumento.Visible = False

            If e.Row.DataItem("B_REG_SICOD") Then
                btnRegistroSICOD.Visible = True
            Else
                btnRegistroSICOD.Visible = False
            End If

            If (IIf(IsDBNull(e.Row.DataItem("B_BUSCAR_SICOD")), False, e.Row.DataItem("B_BUSCAR_SICOD")) AndAlso
                (puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM OrElse
                puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_INS OrElse
                puObjUsuario.IdentificadorPerfilActual = Constantes.PERFIL_SUP)) Then

                btnBuscarSICOD.Visible = True
                btnBuscarSICOD.OnClientClick = "return LevantaVentanaOficio(" + e.Row.RowIndex.ToString() + ", -1, " + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", " + puObjUsuario.IdArea.ToString + ", '" + e.Row.DataItem("T_OFICIO_SICOD").ToString() + "')"

            Else
                btnBuscarSICOD.Visible = False
            End If

            If PC.IdEstatus = 21 Then
                btnAgregarDocumento.Visible = False
                btnReemplazarDocumento.Visible = False
                btnRegistroSICOD.Visible = False
                btnBuscarSICOD.Visible = False
                If e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 15 Then
                    btnAgregarDocumento.Visible = True
                End If
            End If

            If PC.IdEstatus = 22 Then
                btnAgregarDocumento.Visible = False
                btnReemplazarDocumento.Visible = False
                btnRegistroSICOD.Visible = False
                btnBuscarSICOD.Visible = False
            End If


            Dim dtArchivos As DataTable = Entities.DocumentoPC.ObtenerArchivos(Folio, e.Row.DataItem("I_ID_DOCUMENTO").ToString())
            If dtArchivos.Rows.Count > 0 Then

                Dim tablaArchivos As New Table
                Dim tablaOficios As New Table

                For Each archivo As DataRow In dtArchivos.Rows

                    Dim rowArchivo As New TableRow
                    Dim cellArchivo As New TableCell

                    Dim linkArchivo As New LinkButton

                    linkArchivo.Text = archivo("T_DSC_NOMBRE_DOCUMENTO")
                    linkArchivo.OnClientClick = "__doPostBack('" + Button1.UniqueID + "', '" + archivo("T_DSC_NOMBRE_DOCUMENTO") + "'); return false;"

                    cellArchivo.Controls.Add(linkArchivo)
                    rowArchivo.Cells.Add(cellArchivo)
                    tablaArchivos.Rows.Add(rowArchivo)

                    'si hay documento, no se debe permitir cargar uno nuevo
                    btnAgregarDocumento.Visible = False
                    btnReemplazarDocumento.Visible = True
                    'habilitar el boton de registro 
                    btnRegistroSICOD.OnClientClick = "RegistroSICOD(" + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", " + archivo("I_ID").ToString() + ", '" + e.Row.DataItem("T_OFICIO_SICOD").ToString() + "'); return false;"
                    btnRegistroSICOD.Visible = True
                    btnBuscarSICOD.OnClientClick = "return LevantaVentanaOficio(" + e.Row.RowIndex.ToString() + ", " + archivo("I_ID").ToString() + ", " + e.Row.DataItem("I_ID_DOCUMENTO").ToString() + ", " + puObjUsuario.IdArea.ToString + ", '" + e.Row.DataItem("T_OFICIO_SICOD").ToString() + "')"

                    If archivo("T_FOLIO_SICOD").ToString() <> "" Then
                        Dim rowOficio As New TableRow
                        Dim cellOficio As New TableCell

                        btnRegistroSICOD.Visible = False
                        btnReemplazarDocumento.Visible = False
                        btnBuscarSICOD.Visible = False

                        Dim linkFolioSICOD As New LinkButton
                        linkFolioSICOD.Text = archivo("T_FOLIO_SICOD").ToString()
                        linkFolioSICOD.OnClientClick = "ConsultarOficios('" + archivo("T_FOLIO_SICOD").ToString().Replace("/", "-") + "'); return false;"

                        cellOficio.Controls.Add(linkFolioSICOD)
                        rowOficio.Cells.Add(cellOficio)
                        tablaOficios.Rows.Add(rowOficio)


                        'If ConexionSICOD.FolioFinalizado(archivo("T_FOLIO_SICOD").ToString()) Then
                        '    'Si el folio esta finalizado se pertimete ingresar otro archivo
                        '    btnAgregarDocumento.Visible = True
                        'End If
                    Else



                    End If

                    'Agregamos esta validación porque ya se tiene un documento
                    btnRegistroSICOD.Visible = False
                    btnBuscarSICOD.Visible = False

                Next

                e.Row.Cells(2).Controls.Add(tablaArchivos)
                e.Row.Cells(5).Controls.Add(tablaOficios)
            Else

                btnReemplazarDocumento.Visible = False
                If e.Row.DataItem("B_REG_SICOD") Then
                    btnRegistroSICOD.Visible = True
                    btnAgregarDocumento.Visible = True
                End If


                If e.Row.DataItem("B_BUSCAR_SICOD") Then
                    btnBuscarSICOD.Visible = True
                    btnAgregarDocumento.Visible = True
                End If
            End If





            'Programa de Corrección
            If e.Row.DataItem("I_PASO_INICIAL").ToString() = 1 And e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 1 Then
                btnRegistroSICOD.Visible = False
                'btnAgregarDocumento.Visible = False

                Dim tablaArchivos As New Table
                Dim rowArchivo As New TableRow
                Dim cellArchivo As New TableCell
                Dim linkArchivo As New LinkButton

                linkArchivo.Text = DocumentoPC
                linkArchivo.OnClientClick = "__doPostBack('" + Button2.UniqueID + "', '" + DocumentoPC + "'); return false;"

                cellArchivo.Controls.Add(linkArchivo)
                rowArchivo.Cells.Add(cellArchivo)
                tablaArchivos.Rows.Add(rowArchivo)
                e.Row.Cells(2).Controls.Add(tablaArchivos)

            End If

            If e.Row.DataItem("I_PASO_INICIAL").ToString() <> PasoActual Then
                btnRegistroSICOD.Visible = False
                'btnAgregarDocumento.Visible = False
                btnBuscarSICOD.Visible = False

            Else
                If PasoActual = 2 Then
                    If PC.IdEstatus <> 8 Then
                        'AMMM SE AGREGA IF ARA EL DOCUMENTOS SOLICITADO EN SE 2019
                        If e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 6 Then

                            'btnAgregarDocumento.Visible = True
                        Else
                            btnRegistroSICOD.Visible = False
                            'btnAgregarDocumento.Visible = False
                        End If
                    End If
                    'If PC.IdEstatus = 21 Or 22 Then
                    '    btnAgregarDocumento.Visible = False
                    '    btnReemplazarDocumento.Visible = False
                    '    btnRegistroSICOD.Visible = False
                    '    btnBuscarSICOD.Visible = False
                    '    If e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 15 Then
                    '        btnAgregarDocumento.Visible = True
                    '    End If
                    'End If
                End If
                'End If
            End If

            If PasoActual = 3 Then

                'I_ID_DOCUMENTO  T_NOM_DOCUMENTO	I_PASO_INICIAL
                '3   Aviso de No procedencia	3
                '4   PDF Aviso de No procedencia	3
                '5   Aviso de No existió irregularidad	3
                '6   PDF Aviso de No existió irregularidad	3
                '7   Oficio de Procede con plazo	3
                '8   PDF Oficio Procede con plazo	3
                '9   Plan de trabajo de AFORE	3
                '10  Oficio  Procedencia o ratificación	3
                '11  PDF Oficio Procedencia o ratificación	3
                '12  Oficio de No presentado	3
                '13  PDF Oficio de No presentado	3

                Select Case PC.IdResolucion
                    Case 1
                        '<asp:ListItem Value = "1" > No procede</asp: ListItem>
                        If e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 5 Or
                           e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 6 Or
                           e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 7 Or
                           e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 8 Or
                           e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 9 Or
                           e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 10 Or
                           e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 11 Or
                           e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 12 Or
                           e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 13 Then

                            'btnRegistroSICOD.Visible = False
                            'btnAgregarDocumento.Visible = False


                        End If

                    Case 2
                        '<asp:ListItem Value="2">Procede</asp:ListItem>
                        If e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 3 Or
                       e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 4 Or
                       e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 5 Or
                       e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 6 Or
                       e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 7 Or
                       e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 8 Or
                       e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 9 Or
                       e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 12 Or
                       e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 13 Then

                            'btnRegistroSICOD.Visible = False
                            'btnAgregarDocumento.Visible = False


                        End If
                    Case 3
                        Select Case PC.IdEstatus

                            Case 8
                                '<asp:ListItem Value = "3" > Procede con plazo</asp: ListItem>
                                If e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 3 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 4 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 5 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 6 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 8 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 9 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 10 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 11 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 12 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 13 Then

                                    'btnRegistroSICOD.Visible = False
                                    'btnAgregarDocumento.Visible = False
                                End If

                            Case 102
                                If e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 3 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 4 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 5 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 6 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 8 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 9 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 10 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 11 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 12 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 13 Then

                                    'btnRegistroSICOD.Visible = False
                                    'btnAgregarDocumento.Visible = False
                                End If

                            Case 103
                                If e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 3 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 4 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 5 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 6 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 7 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 8 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 9 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 10 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 11 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 12 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 13 Then

                                    'btnRegistroSICOD.Visible = False
                                    'btnAgregarDocumento.Visible = False
                                End If

                            Case 104
                                If e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 3 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 4 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 5 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 6 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 7 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 10 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 11 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 12 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 13 Then

                                    'btnRegistroSICOD.Visible = False
                                    'btnAgregarDocumento.Visible = False
                                End If

                            Case 108
                                If e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 3 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 4 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 5 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 6 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 7 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 8 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 9 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 10 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 11 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 12 Or
                                   e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 13 Then

                                    'btnRegistroSICOD.Visible = False
                                    'btnAgregarDocumento.Visible = False
                                End If

                        End Select
                    Case 4

                        Select Case PC.IdEstatus
                            Case 102
                                '<asp:ListItem Value="4">No presentado</asp:ListItem>
                                If e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 3 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 4 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 5 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 6 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 7 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 8 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 9 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 10 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 11 Then

                                    'btnRegistroSICOD.Visible = False
                                    'btnAgregarDocumento.Visible = False

                                End If
                            Case 103
                                '<asp:ListItem Value="4">No presentado</asp:ListItem>
                                If e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 3 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 4 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 5 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 6 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 7 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 8 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 9 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 10 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 11 Then

                                    'btnRegistroSICOD.Visible = False
                                    'btnAgregarDocumento.Visible = False

                                End If
                            Case 104
                                '<asp:ListItem Value="4">No presentado</asp:ListItem>
                                If e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 3 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 4 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 5 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 6 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 7 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 8 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 9 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 10 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 11 Or
                                    e.Row.DataItem("I_ID_DOCUMENTO").ToString() = 12 Then

                                    'btnRegistroSICOD.Visible = False
                                    'btnAgregarDocumento.Visible = False

                                End If
                        End Select
                End Select
            End If





        End If
    End Sub

    Protected Sub btnActulizarGrid_Click(sender As Object, e As EventArgs) Handles ucFiltroExp.Filtrar
        Dim dtDocumentos As DataTable = Entities.DocumentoPC.ObtenerTodos
        Dim dvDocumentos As DataView = dtDocumentos.DefaultView

        If (PC.IdPaso >= 2 And PC.IdResolucion <> 3) Or PC.IdEstatus = 108 Then
            Dim dt_Actividad As DataTable = Actividad.ObtenerTodas(Folio)
            If (dt_Actividad.Rows.Count > 0) Then
                dvDocumentos.RowFilter = "I_PASO_INICIAL >= 1 AND I_PASO_FINAL <= 3"
                If PC.IdPaso = 4 Then
                    dvDocumentos.RowFilter = "I_PASO_INICIAL >= 1 AND I_PASO_FINAL <= 4"
                End If
            Else
                'dvDocumentos.RowFilter = "I_ID_DOCUMENTO <> 9 AND I_PASO_INICIAL >= 1 AND I_PASO_FINAL <= " + PasoActual.ToString()
                dvDocumentos.RowFilter = "NOT(I_ID_DOCUMENTO= 8 or I_ID_DOCUMENTO=9) AND I_PASO_INICIAL >= 1 AND I_PASO_FINAL <= " + PasoActual.ToString()
                If PC.IdResolucion = 1 Or 4 Then
                    dvDocumentos.Delete(2)
                End If
            End If
        Else
            If PC.IdResolucion = 3 Then
                dvDocumentos.RowFilter = "I_PASO_INICIAL >= 1 AND I_PASO_FINAL <= " + PasoActual.ToString()
            Else
                dvDocumentos.RowFilter = "NOT(I_ID_DOCUMENTO= 8 or I_ID_DOCUMENTO=9) AND I_PASO_INICIAL >= 1 AND I_PASO_FINAL <= " + PasoActual.ToString()
            End If
        End If

        gvExpedientePC.DataSource = dvDocumentos
        gvExpedientePC.DataBind()

        If Not hfFolioSICOD.Value = String.Empty Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Muetra folio", "ConsultarOficios('" + hfFolioSICOD.Value + "');", True)
            hfFolioSICOD.Value = String.Empty
        End If


    End Sub



    Public Function ExpedienteValido(Folio As Integer) As Boolean

        Dim valido As Boolean = True
        Dim dvDocumentos As DataView


        Select Case PasoActual
            Case 1
            Case 2
                Select Case PC.IdEstatus
                    Case 6
                        Return True
                    Case 8
                        Dim dtRequerimientos As DataTable = Entities.RequerimientoPC.ObtenerRequerimientos(Folio)
                        If dtRequerimientos.Rows.Count = 0 Then
                            'No se han registrado requerimientos
                            Return False
                        Else
                            Dim isValid As Boolean = Entities.RequerimientoPC.RequerimientosCompletos(Folio)
                            If isValid Then
                                dvDocumentos = Entities.DocumentoPC.ObtenerArchivos(Folio, 2).DefaultView

                                If dvDocumentos.Count = 0 Then
                                    'No se cargado ningun archivo
                                    isValid = False
                                Else

                                    For Each documento As DataRowView In dvDocumentos
                                        If documento("T_FOLIO_SICOD").ToString() <> String.Empty Then
                                            If ConexionSICOD.FolioFinalizado(documento("T_FOLIO_SICOD")) Then
                                                'OK
                                            Else
                                                'Tiene archivo y folio pero no esta finalizado
                                                isValid = False
                                                Exit For
                                            End If
                                        Else
                                            'Tiene archivo pero no folio sicod
                                            isValid = False
                                            Exit For
                                        End If
                                    Next
                                End If
                            Else
                                Return isValid
                            End If
                        End If
                End Select


            Case 3
                'Validar la resolución
                '<asp:ListItem Value = "1" > No procede</asp: ListItem>
                '<asp:ListItem Value="2">Procede</asp:ListItem>
                '<asp:ListItem Value = "3" > Procede con plazo</asp: ListItem>
                '<asp:ListItem Value="4">No presentado</asp:ListItem>

                Select Case PC.IdResolucion
                    Case 1
                        dvDocumentos = Entities.DocumentoPC.ObtenerArchivos(Folio, 3).DefaultView
                    Case 2
                        dvDocumentos = Entities.DocumentoPC.ObtenerArchivos(Folio, 10).DefaultView
                    Case 3
                        Select Case PC.IdEstatus
                            Case 102
                                dvDocumentos = Entities.DocumentoPC.ObtenerArchivos(Folio, 7).DefaultView

                            Case 104
                                Dim dt_Actividad As DataTable = Actividad.ObtenerTodas(Folio)
                                If (dt_Actividad.Rows.Count = 0) Then
                                    Return False
                                Else
                                    Dim row As DataRow = dt_Actividad.Select("I_ID_ESTATUS = 'En Proceso'").FirstOrDefault
                                    If row Is Nothing Then
                                        dvDocumentos = Entities.DocumentoPC.ObtenerArchivos(Folio, 8).DefaultView

                                        If dvDocumentos.Count = 0 Then
                                            'No se cargado ningun archivo
                                            Return False
                                        Else
                                            Return True
                                        End If
                                    Else
                                        Return False
                                    End If
                                End If

                        End Select



                    'dvDocumentos = Entities.DocumentoPC.ObtenerArchivos(Folio, 7).DefaultView
                    '+Validar los requirimientos




                    Case 4
                        dvDocumentos = Entities.DocumentoPC.ObtenerArchivos(Folio, 12).DefaultView
                End Select

            Case 4
                Select Case PC.IdEstatus
                    Case 105 'MMOB  NUEVO CASO
                        dvDocumentos = Entities.DocumentoPC.ObtenerArchivos(Folio, 14).DefaultView
                End Select
            Case Else
                valido = True
        End Select

        If dvDocumentos.Count = 0 Then
            'No se cargado ningun archivo
            valido = False
        Else

            For Each documento As DataRowView In dvDocumentos
                If (Entities.DocumentoOPI.ValidaObligatorioSICOD(documento("I_ID_DOCUMENTO"))) Then
                    If documento("T_FOLIO_SICOD").ToString() <> String.Empty OrElse documento("T_DSC_NOMBRE_DOCUMENTO").ToString() <> String.Empty Then
                        If documento("T_FOLIO_SICOD").ToString() <> String.Empty Then
                            If ConexionSICOD.FolioFinalizado(documento("T_FOLIO_SICOD")) Then
                                'OK

                            Else
                                'Tiene archivo y folio pero no esta finalizado
                                valido = False
                                Exit For
                            End If

                        End If
                    Else
                        'Tiene archivo pero no folio sicod
                        valido = False
                        Exit For
                    End If
                End If
            Next
        End If
        If (PasoActual = 2 And PC.IdEstatus <> 8) Then
            valido = True
        End If

        'Return valido
        Return valido

    End Function


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
                usuario = System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointUserSEPRIS")
                passwd = Utilerias.Cifrado.DescifrarAES(System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointPasswordSEPRIS"))
                ServSharepoint = System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointServerSEPRIS")
                dominio = System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointDomainSEPRIS")
                biblioteca = System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointBibliotecaSEPRIS-PC-OPI")

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

        btnActulizarGrid_Click(sender, e)
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

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
                usuario = System.Web.Configuration.WebConfigurationManager.AppSettings("UsuarioSpSICOD")
                passwd = Utilerias.Cifrado.DescifrarAES(System.Web.Configuration.WebConfigurationManager.AppSettings("PassEncSpSICOD"))
                ServSharepoint = System.Web.Configuration.WebConfigurationManager.AppSettings("SharePointServerSICOD")
                dominio = System.Web.Configuration.WebConfigurationManager.AppSettings("DomainSICOD")
                biblioteca = System.Web.Configuration.WebConfigurationManager.AppSettings("DocLibrarySICOD")

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

        btnActulizarGrid_Click(sender, e)
    End Sub
End Class