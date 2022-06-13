Public Class CopiarFolios
    Inherits System.Web.UI.UserControl

    Public Property pagina As System.Web.UI.Page
    Public Property Mensaje As String

    Public Property LlenoGrid As Boolean
        Get
            If IsNothing(ViewState("LlenoGrid")) Then
                Return False
            Else
                Dim lbAux As Boolean = False
                If Boolean.TryParse(ViewState("LlenoGrid"), lbAux) Then
                    Return lbAux
                Else
                    Return False
                End If
            End If
        End Get
        Set(value As Boolean)
            ViewState("LlenoGrid") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub LlenaGrid()
        Dim usuario As New Entities.Usuario()
        usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

        If Not IsNothing(usuario) Then
            Dim dt As DataTable = AccesoBD.consultarVisitasDT(usuario.IdArea, Constantes.CopFoliosTConsul.DelPasoUnoAl17,
                                                              usuario.IdentificadorUsuario, usuario.IdentificadorPerfilActual)

            gvConsulta.DataSource = dt
            gvConsulta.DataBind()
        End If
    End Sub

    Protected Sub chkCopiarFolio_CheckedChanged(sender As Object, e As EventArgs) Handles chkCopiarFolio.CheckedChanged
        chkCopiarFolio.Checked = False
        If Not LlenoGrid Then
            LlenaGrid()
            LlenoGrid = True
        End If

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MuestraModal();", True)
        Exit Sub
    End Sub


    Protected Sub gvConsulta_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvConsulta.RowCreated
        If e.Row.RowType = WebControls.DataControlRowType.DataRow Then
            If Not IsNothing(pagina) Then
                e.Row.Attributes.Add("ondblclick", pagina.ClientScript.GetPostBackEventReference(btnConsulta, e.Row.RowIndex.ToString(), False))
            End If
        End If
    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsulta.Click

        Dim index As Integer = Convert.ToInt32(Request("__EVENTARGUMENT"))

        Dim idVisitaGenerado As Integer = CInt(gvConsulta.DataKeys(index)("I_ID_VISITA").ToString())
        Session("ID_VISITA") = idVisitaGenerado
        Response.Redirect("../Procesos/DetalleVisita.aspx#tab1")
    End Sub


    ''' <summary>
    ''' Crea la copia de folio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAceptarM2B1A_Click(sender As Object, e As EventArgs) Handles btnAceptarCopiar.Click
        Dim index As Integer = 0
        Dim idVisitaPadre As Integer = 0
        Dim chkElemento As CheckBox
        Dim lsVisitaGenerada As String = "<ul>"


        For Each row As GridViewRow In gvConsulta.Rows
            chkElemento = row.FindControl("chkElemento")

            If Not IsNothing(chkElemento) Then
                If chkElemento.Checked Then
                    index = row.RowIndex
                    idVisitaPadre = CInt(gvConsulta.DataKeys(index)("I_ID_VISITA").ToString())

                    Exit For
                End If
            End If
        Next

        ''Replicar la visita
        If idVisitaPadre <> 0 Then
            Dim usuario As New Entities.Usuario()
            usuario = Session(Entities.Usuario.SessionID) ' se obtiene de la session

            If Not IsNothing(usuario) Then
                Dim liIdVisitaConsec As Integer = 0
                Dim lsAux As String = AccesoBD.registrarCopiaVisita(idVisitaPadre, usuario.IdArea, liIdVisitaConsec)

                If lsAux <> "" Then
                    Session.Add("idVisitaEditar", liIdVisitaConsec)
                    lsVisitaGenerada &= "<li>" & lsAux & "</li>"
                    imgConfirmaRegistroVisitaCC.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgOk
                    Mensaje = "Se ha registrado satisfactoriamente la nueva visita. Deseas editar la información de la visita? <br /><br />" & lsVisitaGenerada & "</ul>"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionCopia();", True)
                Else
                    Mensaje = "No se pudo registrar la visita. <br /><br />"
                    imgAviso.ImageUrl = Entities.Imagen.RutaCarpeta & Constantes.imgError
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "AvisoCCF();", True)
                End If
            End If
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "Mensaje();MuestraModal();", True)
            Exit Sub
        End If
    End Sub

    Protected Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        Response.Redirect("../Visita/Registro.aspx?up=1")
    End Sub

    Public Sub HabilitaControles()
        chkCopiarFolio.Enabled = True
    End Sub

    Protected Sub btnConfirmarNo_Click(sender As Object, e As EventArgs)
        Session.Remove("idVisitaEditar")
        Response.Redirect("../Visita/Bandeja.aspx")
    End Sub
End Class