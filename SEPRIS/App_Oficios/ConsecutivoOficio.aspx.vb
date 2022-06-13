Imports SICOD.Generales
Imports LogicaNegocioSICOD
Imports Clases
Imports System.Web

Public Class ConsecutivoOficio
    Inherits System.Web.UI.Page

#Region "Propiedades"

    Public Property ID_UNIDAD() As Integer
        Get
            Return CInt(ViewState("ID_UNIDAD"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_UNIDAD") = value
        End Set
    End Property

    Public Property ID_T_UNIDAD() As Integer
        Get
            Return CInt(ViewState("ID_T_UNIDAD"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_T_UNIDAD") = value
        End Set
    End Property

    Public Property ID_ANIO As Integer
        Get
            Return CInt(ViewState("ID_ANIO"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_ANIO") = value
        End Set
    End Property

    Public Property ID_TIPO_DOC As Integer
        Get
            Return CInt(ViewState("ID_TIPO_DOC"))
        End Get
        Set(ByVal value As Integer)
            ViewState("ID_TIPO_DOC") = value
        End Set
    End Property

    Public Property NUM_CONSECUTIVO As String
        Get
            Return CStr(ViewState("NUM_CONSECUTIVO"))
        End Get
        Set(ByVal value As String)
            ViewState("NUM_CONSECUTIVO") = value
        End Set
    End Property

    Public Property TEXTO As String
        Get
            Return CStr(ViewState("Texto"))
        End Get
        Set(ByVal value As String)
            ViewState("Texto") = value
        End Set
    End Property

    Public Property B_APLICA_NUM_CONSEC As String
        Get
            Return CStr(ViewState("B_APLICA_NUM_CONSEC"))
        End Get
        Set(ByVal value As String)
            ViewState("B_APLICA_NUM_CONSEC") = value
        End Set
    End Property

#End Region

#Region "Eventos de la Pagina"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            CargaCombos()

        End If

    End Sub

#End Region

#Region "Carga de Pagina"

    Private Sub CargaCombos()

        ''Area
        'CargarCombo(ddlarea, BusinessRules.BDS_C_AREA.GetAll(2), "DSC_COMPOSITE", "ID_UNIDAD_ADM")
        CargarCombo(ddlarea, LogicaNegocioSICOD.UnidadAdministrativa.GetList(UnidadAdministrativaTipo.Funcional, _
                                                                        UnidadAdministrativaEstatus.Activo), _
                                                                    "DSC_CODIGO_UNIDAD_ADM", "ID_UNIDAD_ADM")

        ''TipoDocto
        CargarCombo(ddlTipoDoc, BusinessRules.BDA_TIPO_DOCUMENTO.ConsultarTipoDocumento(), "T_TIPO_DOCUMENTO", "ID_TIPO_DOCUMENTO")

        ' NO QUIERO MOSTRAR EL -1
        ''Anio  '' 
        ddlanio.DataSource = BusinessRules.BDA_ANIO.ConsultarAnio
        ddlanio.DataTextField = "CICLO"
        ddlanio.DataValueField = "CICLO"
        ddlanio.DataBind()

        Try

            ddlanio.SelectedValue = Now.Year

        Catch ex As Exception
        End Try

    End Sub

#End Region

#Region "Eventos Botones"

    Protected Sub btnfiltrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnfiltrar.Click

        Filtraje()

    End Sub

    Protected Sub btnDefine_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDefine.Click

        Me.lblCapturaTitulo.Text = TEXTO
        txtConsecutivo.Text = ""
        If Not NUM_CONSECUTIVO = "N/D" Then txtConsecutivo.Text = NUM_CONSECUTIVO

        UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "PonFoco", "Foco();", True)

        ModalPopUpCaptura.Show()

    End Sub

    Private Sub BtnCapturaOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCapturaOK.Click

        Dim con As Conexion
        Dim Valores As String = ""
        Dim Campos As String = ""
        Dim Condicion As String = ""

        Try

            con = New Conexion()

            If Not NUM_CONSECUTIVO = "N/D" Then

                Valores = String.Format("NUM_CONSECUTIVO = {0}, F_FECH_MODIFICA = GETDATE(), USUARIO_MODIFICA = '{1}'", txtConsecutivo.Text, Session("Usuario").ToString.ToLower)

                Condicion = String.Format("ID_UNIDAD_ADM = {0} AND ID_T_UNIDAD_ADM = {1} AND ID_ANIO = {2} AND ID_TIPO_DOCUMENTO = {3}", _
                                          ID_UNIDAD.ToString(), ID_T_UNIDAD.ToString(), ID_ANIO.ToString(), ID_TIPO_DOC.ToString())

                If Not con.Actualizar(Conexion.Owner & "BDA_C_CONSECUTIVO_OFICIOS", Valores, Condicion) Then

                    Throw New Exception("No se pudo actualizar el consecutivo")

                End If

            Else

                Campos = "ID_UNIDAD_ADM,ID_T_UNIDAD_ADM,ID_ANIO,ID_TIPO_DOCUMENTO,NUM_CONSECUTIVO,F_FECH_MODIFICA,USUARIO_MODIFICA"

                Valores = ID_UNIDAD.ToString() & "," & _
                    ID_T_UNIDAD.ToString() & "," & _
                    ID_ANIO.ToString() & "," & _
                    ID_TIPO_DOC.ToString() & "," & _
                    txtConsecutivo.Text & "," & _
                    "GETDATE(),'" & _
                    Session("Usuario").ToString.ToLower & "'"

                If Not con.Insertar(Conexion.Owner & "BDA_C_CONSECUTIVO_OFICIOS", Campos, Valores) Then

                    Throw New Exception("No se pudo guardar el consecutivo")

                End If

            End If


            Filtraje()
            MostrarMensajeError("Mensaje", "Consecutivo guardado satisfactoriamente")


        Catch ex As Exception

            MostrarMensajeError("Error", ex.Message)

        End Try

    End Sub

#End Region

#Region "GridView"

    Private Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand

        If e.CommandName = "Selecciona" Then

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            gvDatos.SelectedIndex = index

            gvDatos.SelectedRowStyle.BackColor = System.Drawing.Color.FromArgb(208, 114, 95)
            gvDatos.SelectedRowStyle.Font.Bold = True
            gvDatos.SelectedRowStyle.ForeColor = Drawing.Color.White


            'GridPrincipal.SelectedItemStyle.BackColor = System.Drawing.Color.FromArgb(208, 114, 95)
            'GridPrincipal.SelectedItemStyle.Font.Bold = True
            'GridPrincipal.SelectedItemStyle.ForeColor = System.Drawing.Color.White



            With gvDatos

                ID_UNIDAD = CInt(.DataKeys(index)("ID_UNIDAD_ADM").ToString())
                ID_T_UNIDAD = CInt(.DataKeys(index)("ID_T_UNIDAD_ADM").ToString())
                ID_ANIO = CInt(.DataKeys(index)("ID_ANIO").ToString())
                ID_TIPO_DOC = CInt(.DataKeys(index)("ID_TIPO_DOCUMENTO").ToString())
                NUM_CONSECUTIVO = CStr(.DataKeys(index)("NUM_CONSECUTIVO").ToString())
                B_APLICA_NUM_CONSEC = CStr(.DataKeys(index)("B_APLICA_NUM_CONSEC").ToString())

                btnDefine.Text = "Modificar Consecutivo"
                If NUM_CONSECUTIVO = "N/D" Then btnDefine.Text = "Definir"


                ''ES VISIBLE SI NO HAY YA EXISTENTES OFICIOS CREADOS PARA EL AREA, AÑO Y TIPO DOCTO
                btnDefine.Visible = CInt(.DataKeys(index)("CURRENT_CONSECUTIVO").ToString()) = 0


                TEXTO = Server.HtmlDecode(gvDatos.Rows(index).Cells(1).Text) & _
                    " - " & Server.HtmlDecode(gvDatos.Rows(index).Cells(3).Text) & _
                    " - Año: " & ID_ANIO

            End With

        End If

    End Sub

#End Region

#Region "Metodos"

    Private Sub Filtraje()

        Try

            gvDatos.DataSource = BusinessRules.BDA_C_CONSECUTIVO_OFICIOS.GetAllCompositeByFilter(ddlanio.SelectedValue, _
                                                                                                 ddlarea.SelectedValue, _
                                                                                                 ddlTipoDoc.SelectedValue)
            gvDatos.DataBind()
            gvDatos.SelectedIndex = -1

            btnDefine.Visible = False
            txtConsecutivo.Text = ""

        Catch ex As Exception

            MostrarMensajeError("", ex.Message)

        End Try



    End Sub

    Private Sub MostrarMensajeError(ByVal Titulo As String, ByVal Texto As String)

        lblErroresTitulo.Text = Titulo
        lblErroresPopup.Text = "<ul><li>" & Texto & "</li></ul>"

        ModalPopupExtenderErrores.Show()

    End Sub

#End Region


    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Response.Redirect("./ConsecutivoOficioAdd.aspx")
    End Sub

    Protected Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click

        If ID_UNIDAD = 0 OrElse ID_T_UNIDAD = 0 OrElse ID_ANIO = 0 OrElse ID_TIPO_DOC = 0 Then
            MostrarMensajeError("Mensaje", "Debe seleccionar un registro")
        Else
            Response.Redirect("./ConsecutivoOficioEdit.aspx?D1=" & ID_UNIDAD.ToString() & "&D2=" & ID_T_UNIDAD & "&D3=" & ID_ANIO & "&D4=" & ID_TIPO_DOC & "&D5=" & NUM_CONSECUTIVO & "&D6=" & B_APLICA_NUM_CONSEC)
        End If


    End Sub
End Class