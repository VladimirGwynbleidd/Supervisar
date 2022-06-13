Imports System
Imports System.Web
Imports System.Web.Configuration

Public Class PruebaConexion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnProbarConexion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProbarConexion.Click

        Try
            Dim conex As Conexion.SQLServer

            If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
                conex = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNenc").ToString())
            Else
                conex = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNenc").ToString())
            End If

            lblMensaje.Text = "Conexión correcta."

        Catch
            lblMensaje.Text = "Error."
        End Try

    End Sub

    Protected Sub btnBulkCopy_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBulkCopy.Click

        Try
            Dim conex As Conexion.SQLServer

            If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
                conex = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNenc").ToString())
            Else
                conex = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNenc").ToString())
            End If

            Dim dt As New DataTable

            dt.Columns.Add("id", GetType(Integer))
            dt.Columns.Add("descripcion", GetType(String))



            dt.Rows.Add(1, "prueba")


            conex.Insertar(dt, "TablaPrueba")


            lblBulkCopy.Text = "OK."

        Catch ex As Exception

            lblBulkCopy.Text = ex.ToString()

        End Try

    End Sub

    Protected Sub btnInicializar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInicializar.Click

        Try
            Dim conex As Conexion.SQLServer

            If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
                conex = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNenc").ToString())
            Else
                conex = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNenc").ToString())
            End If


            conex.EjecutarSP("InicializarTablasPrueba")


            lblInicializar.Text = "OK."

        Catch ex As Exception

            lblInicializar.Text = ex.ToString()

        End Try


    End Sub

    Protected Sub btnVarbinary_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVarbinary.Click
        Try
            Dim conex As Conexion.SQLServer

            If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
                conex = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNenc").ToString())
            Else
                conex = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNenc").ToString())
            End If

            Dim Campos As New List(Of String)
            Dim Valores As New List(Of Object)

            Campos.Add("id")
            Campos.Add("descripcion")
            Campos.Add("dato")

            Valores.Add(100)
            Valores.Add("Prueba Varbinary")
            Valores.Add(New Byte() {1, 2, 3, 4, 5})


            conex.Insertar("TablaPruebaVarbinary", Campos, Valores)


            lblVarbinary.Text = "OK."

        Catch ex As Exception

            lblVarbinary.Text = ex.ToString()

        End Try
    End Sub
End Class