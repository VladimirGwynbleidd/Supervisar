'- Fecha de creación:18/07/2013
'- Fecha de modificación: 
'- Nombre del Responsable: ARGC1
'- Empresa: Softtek
'- Pagina de responsables

Public Class Responsables
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            lblTitulo.Text = Conexion.SQLServer.Parametro.ObtenerValor("TituloResponsables")
        End If

    End Sub


    ''' <summary>
    ''' Crea el codigo html para mostrar en pantalla la	MESA DE AYUDA TÉCNICA 
    ''' </summary>
    Public Sub Mesa_Ayuda_Tecnica()

        Dim Dr As DataTable = Nothing
        Dim Valores As String()
        Dim Responsable As String
        Dim Puesto As String
        Dim Telefono As String
        Dim Correo As String
        Dim NombrePuesto As String
        Dim Titulo As String
        Dim Descripcion As String
        Try

            Dr = Conexion.SQLServer.Parametro.ObtenerValores("MESA AYUDA TECNICA")
            With HttpContext.Current.Response
                Titulo = Conexion.SQLServer.Parametro.ObtenerValor("MAT Descripcion").Split("-")(0)
                Descripcion = Conexion.SQLServer.Parametro.ObtenerValor("MAT Descripcion").Split("-")(1)

                .Write("<Table class=""Tabla_Responsables""" & _
                "cellSpacing=""1"" cellPadding=""0""  width=""600"" borderColorLight=""#576979"" border=""1"">" & vbCrLf)
                .Write("<TR class=""Span_h2"">")
                .Write("<TD colSpan=""3"">" & vbCrLf)
                .Write("<P>")
                .Write("<SPAN>")
                .Write(Titulo & vbCrLf)
                .Write("</SPAN>" & vbCrLf)
                .Write("</P>" & vbCrLf)
                .Write("<P>" & vbCrLf)
                .Write("<SPAN>" & vbCrLf)
                .Write(Descripcion & vbCrLf)
                .Write("</SPAN>" & vbCrLf)
                .Write("</P>" & vbCrLf)
                .Write("</TD>" & vbCrLf)
                .Write("</TR>" & vbCrLf)
                'For Each Menu In ListaSubMenu
                For Each linea As DataRow In Dr.Rows
                    NombrePuesto = linea("T_DSC_VALOR").ToString()
                    Valores = NombrePuesto.Split(","c)
                    Responsable = Valores(0)
                    Puesto = Valores(1)
                    Telefono = Valores(3)
                    Correo = Valores(2)

                    .Write("<TR class=""Span_h1"">" & vbCrLf)
                    .Write("<TD class=""td_resp"">" & vbCrLf)
                    .Write("<P>" & vbCrLf)
                    .Write("<SPAN>" & vbCrLf)
                    .Write("<A href=""mailto:" & Correo & "?Subject=Comentarios al Sistema "">" & vbCrLf)
                    .Write(Responsable & vbCrLf)
                    .Write("</A>" & vbCrLf)
                    .Write("</SPAN>" & vbCrLf)
                    .Write("</P>" & vbCrLf)
                    .Write("</TD>" & vbCrLf)
                    .Write("<TD>" & vbCrLf)
                    .Write("<P>" & vbCrLf)
                    .Write("<SPAN>" & vbCrLf)
                    .Write(Puesto & vbCrLf)
                    .Write("</SPAN>" & vbCrLf)
                    .Write("</P>" & vbCrLf)
                    .Write("</TD>" & vbCrLf)
                    .Write("<TD>" & vbCrLf)
                    .Write("<P>" & vbCrLf)
                    .Write("<SPAN>" & vbCrLf)
                    .Write(Telefono & vbCrLf)
                    .Write("</SPAN>" & vbCrLf)
                    .Write("</P>" & vbCrLf)
                    .Write("</TD>" & vbCrLf)
                    .Write("</TR>" & vbCrLf)
                Next

                .Write("</Table>" & vbCrLf)

            End With
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            If Not Dr Is Nothing Then
                Dr = Nothing
            End If
        End Try
    End Sub


    ''' <summary>
    ''' Crea el codigo html para mostrar en pantalla la	MESA DE AYUDA FUNCIONAL 
    ''' </summary>
    Public Sub Mesa_Ayuda_Funcional()

        Dim Dr As DataTable = Nothing
        Dim Valores As String()
        Dim Responsable, Puesto, correo, Telefono, NombrePuesto As String
        Dim Titulo As String
        Dim Descripcion As String
        Try
            Dr = Conexion.SQLServer.Parametro.ObtenerValores("MESA AYUDA FUNCIONAL")

            With HttpContext.Current.Response
                Titulo = Conexion.SQLServer.Parametro.ObtenerValor("MAF Descripcion").Split("-")(0)
                Descripcion = Conexion.SQLServer.Parametro.ObtenerValor("MAF Descripcion").Split("-")(1)

                .Write("<Table class=""Tabla_Responsables""" & _
                "cellSpacing=""1"" cellPadding=""0""  width=""600"" borderColorLight=""#576979"" border=""1"">" & vbCrLf)
                .Write("<TR class=""Span_h2"">" & vbCrLf)
                .Write("<TD colSpan=""3"">" & vbCrLf)
                .Write("<P>" & vbCrLf)
                .Write("<SPAN>" & vbCrLf)
                .Write(Titulo & vbCrLf)
                .Write("</SPAN>" & vbCrLf)
                .Write("</P>" & vbCrLf)
                .Write("<P>" & vbCrLf)
                .Write("<SPAN>" & vbCrLf)
                .Write(Descripcion & vbCrLf)
                .Write("</SPAN>" & vbCrLf)
                .Write("</P>" & vbCrLf)
                .Write("</TD>" & vbCrLf)
                .Write("</TR>" & vbCrLf)
                'For Each Menu In ListaSubMenu
                For Each linea As DataRow In Dr.Rows
                    NombrePuesto = linea("T_DSC_VALOR").ToString()
                    Valores = NombrePuesto.Split(","c)
                    Responsable = Valores(0)
                    Puesto = Valores(1)
                    Telefono = Valores(3)
                    correo = Valores(2)

                    .Write("<TR class=""Span_h1"">" & vbCrLf)
                    .Write("<TD>" & vbCrLf)
                    .Write("<P>" & vbCrLf)
                    .Write("<SPAN>" & vbCrLf)
                    .Write("<A href=""mailto:" & correo & "?Subject=Comentarios al Sistema"">" & vbCrLf)
                    .Write(Responsable & vbCrLf)
                    .Write("</A>" & vbCrLf)
                    .Write("</SPAN>" & vbCrLf)
                    .Write("</P>" & vbCrLf)
                    .Write("</TD>" & vbCrLf)
                    .Write("<TD>" & vbCrLf)
                    .Write("<P>" & vbCrLf)
                    .Write("<SPAN>" & vbCrLf)
                    .Write(Puesto & vbCrLf)
                    .Write("</SPAN>" & vbCrLf)
                    .Write("</P>" & vbCrLf)
                    .Write("</TD>" & vbCrLf)
                    .Write("<TD>" & vbCrLf)
                    .Write("<P>" & vbCrLf)
                    .Write("<SPAN>" & vbCrLf)
                    .Write(Telefono & vbCrLf)
                    .Write("</SPAN>" & vbCrLf)
                    .Write("</P>" & vbCrLf)
                    .Write("</TD>" & vbCrLf)
                    .Write("</TR>" & vbCrLf)
                Next

                .Write("</Table>" & vbCrLf)

            End With
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            If Not Dr Is Nothing Then
                Dr = Nothing
            End If

        End Try
    End Sub

    Protected Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("Login.aspx")
    End Sub
End Class