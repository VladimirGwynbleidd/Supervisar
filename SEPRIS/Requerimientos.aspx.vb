
' Fecha de creación: 18/07/2013
' Fecha de modificación: 
' Nombre del Responsable: ARGC1
' Empresa: Softtek
' Página de requerimientos minimos

Imports Utilerias

Public Class Requerimientos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            gvRequerimientos.GridLines = GridLines.None

            Try
                gvRequerimientos.DataSource = ObtieneListaRequirimientos()
                gvRequerimientos.DataBind()

            Catch ex As Exception
                Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
            End Try
           
        End If

    End Sub


    Private Function ObtieneListaRequirimientos() As DataTable
        Dim dt As New DataTable

        Try
            dt = Conexion.SQLServer.Parametro.ObtenerValores("REQUERIMIENTOS")
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        End Try

        Return dt
    End Function


    Protected Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("Login.aspx")
    End Sub
End Class