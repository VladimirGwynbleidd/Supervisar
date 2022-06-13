Imports SICOD.Generales
Imports LogicaNegocioSICOD
Imports Clases
Imports System.Web

Public Class ConsecutivoOficioEdit
    Inherits System.Web.UI.Page

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

    Public Property B_APLICA_NUM_CONSEC As String
        Get
            Return CStr(ViewState("B_APLICA_NUM_CONSEC"))
        End Get
        Set(ByVal value As String)
            ViewState("B_APLICA_NUM_CONSEC") = value
        End Set
    End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            CargaCombos()
            CargarDatos()
        End If

    End Sub

    Private Sub CargarDatos()

        If Request.QueryString.Count > 0 Then
            If Request.QueryString("D1") <> Nothing Then
                ID_UNIDAD = Convert.ToInt32(Request.QueryString("D1"))
            End If
            If Request.QueryString("D2") <> Nothing Then
                ID_T_UNIDAD = Convert.ToInt32(Request.QueryString("D2"))
            End If
            If Request.QueryString("D3") <> Nothing Then
                ID_ANIO = Convert.ToInt32(Request.QueryString("D3"))
            End If
            If Request.QueryString("D4") <> Nothing Then
                ID_TIPO_DOC = Convert.ToInt32(Request.QueryString("D4"))
            End If
            If Request.QueryString("D5") <> Nothing Then
                NUM_CONSECUTIVO = Request.QueryString("D5").ToString()
            End If
            If Request.QueryString("D6") <> Nothing Then
                B_APLICA_NUM_CONSEC = Request.QueryString("D6").ToString()
            End If

        End If
        If ID_UNIDAD = 0 OrElse ID_T_UNIDAD = 0 OrElse ID_ANIO = 0 OrElse ID_TIPO_DOC = 0 Then
        Else
            ddlarea.Items.FindByValue(ID_UNIDAD).Selected = True
            ddlTipoDoc.Items.FindByValue(ID_TIPO_DOC).Selected = True
            ddlanio.Items.FindByValue(ID_ANIO).Selected = True
            txbNumConsecutivo.Text = NUM_CONSECUTIVO
            If B_APLICA_NUM_CONSEC = "SI" Then
                ddlAplicaNumCons.Items.FindByValue("1").Selected = True
            Else
                ddlAplicaNumCons.Items.FindByValue("0").Selected = True
            End If
        End If



    End Sub
    Private Sub CargaCombos()
        ''Area
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

        Dim lstAplicaNumCons As New List(Of Datos)()

        lstAplicaNumCons.Add(New Datos(1, "SI"))
        lstAplicaNumCons.Add(New Datos(0, "NO"))

        ddlAplicaNumCons.DataSource = lstAplicaNumCons
        ddlAplicaNumCons.DataTextField = "valor"
        ddlAplicaNumCons.DataValueField = "id"
        ddlAplicaNumCons.DataBind()


    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("./ConsecutivoOficio.aspx")
    End Sub

    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        ValidarDatos()
    End Sub

    Private Sub ValidarDatos()

        Try

            If (ddlarea.SelectedValue = -1) Then
                MostrarMensajeError("Mensaje", "Debe seleccionar un área.")
                Return
            End If

            If (ddlTipoDoc.SelectedValue = -1) Then
                MostrarMensajeError("Mensaje", "Debe seleccionar un tipo de documento.")
                Return
            End If


            If (txbNumConsecutivo.Text.Trim() = String.Empty) Then
                MostrarMensajeError("Mensaje", "Debe capturar el número consecutivo.")
                Return
            End If


            Actualizar()

            BtnModalOk.CommandArgument = "OK"
            lblModalPostBack.Text = "OK"
            MostrarMensajeError("Mensaje", "La información se actualizó de forma correcta.")

        Catch ex As Exception
            MostrarMensajeError("Mensaje", "Ocurrió un error al actualizar el registro. Contacte al Administrador.")
            EscribirError(ex, "Actualizar Consecutivo")

        End Try
    End Sub


    Private Sub Actualizar()

        Try


            Dim con As Conexion
            Dim Valores As String = ""
            Dim Campos As String = ""
            Dim Condicion As String = ""

            con = New Conexion()



            Valores = String.Format("NUM_CONSECUTIVO = {0}, F_FECH_MODIFICA = GETDATE(), USUARIO_MODIFICA = '{1}', B_APLICA_NUM_CONSEC = {2}", txbNumConsecutivo.Text, Session("Usuario").ToString.ToLower, ddlAplicaNumCons.SelectedValue)

            Condicion = String.Format("ID_UNIDAD_ADM = {0} AND ID_T_UNIDAD_ADM = {1} AND ID_ANIO = {2} AND ID_TIPO_DOCUMENTO = {3}", _
                                      ID_UNIDAD.ToString(), ID_T_UNIDAD.ToString(), ID_ANIO.ToString(), ID_TIPO_DOC.ToString())

            If Not con.Actualizar(Conexion.Owner & "BDA_C_CONSECUTIVO_OFICIOS", Valores, Condicion) Then

                Throw New Exception("No se pudo actualizar el consecutivo")

            End If


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub MostrarMensajeError(ByVal Titulo As String, ByVal Texto As String)

        lblErroresTitulo.Text = Titulo
        lblErroresPopup.Text = "<ul><li>" & Texto & "</li></ul>"

        ModalPopupExtenderErrores.Show()

    End Sub

    Protected Sub BtnModalOk_Click(sender As Object, e As EventArgs) Handles BtnModalOk.Click
        If lblModalPostBack.Text = "OK" Then
            Response.Redirect("./ConsecutivoOficio.aspx")
        End If
    End Sub
End Class