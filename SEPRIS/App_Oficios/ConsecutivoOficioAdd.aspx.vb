Imports SICOD.Generales
Imports LogicaNegocioSICOD
Imports Clases
Imports System.Web

Public Class ConsecutivoOficioAdd
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            CargaCombos()

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

            Guardar()

            BtnModalOk.CommandArgument = "OK"
            lblModalPostBack.Text = "OK"
            MostrarMensajeError("Mensaje", "La información se guardó de forma correcta.")

        Catch ex As Exception
            MostrarMensajeError("Mensaje", "Ocurrió un error al guardar el registro. Contacte al Administrador.")
            EscribirError(ex, "Guardar Consecutivo")

        End Try
    End Sub

    Private Sub Guardar()
        Try

            Dim con As Conexion
            Dim Valores As String = ""
            Dim Campos As String = ""
            Dim Condicion As String = ""

            con = New Conexion()

            Campos = "ID_UNIDAD_ADM,ID_T_UNIDAD_ADM,ID_ANIO,ID_TIPO_DOCUMENTO,NUM_CONSECUTIVO,F_FECH_MODIFICA,USUARIO_MODIFICA,B_APLICA_NUM_CONSEC"

            Valores = ddlarea.SelectedValue & "," & _
                "2" & "," & _
                ddlanio.SelectedValue.ToString() & "," & _
                ddlTipoDoc.SelectedValue.ToString() & "," & _
                txbNumConsecutivo.Text & "," & _
                "GETDATE(),'" & _
                Session("Usuario").ToString.ToLower & "'," & _
                ddlAplicaNumCons.SelectedValue

            If Not con.Insertar(Conexion.Owner & "BDA_C_CONSECUTIVO_OFICIOS", Campos, Valores) Then

                Throw New Exception("No se pudo guardar el consecutivo")

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