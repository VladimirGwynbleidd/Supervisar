Public Class ProcesosVigilancia
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Para que el browser no guarde en cache
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Response.Expires = -1

            'verificaSesion()
            'verificaPerfil()
        Catch ex As Exception

        End Try
    End Sub
    'Escribe dentro de una tabla HTML el submenú
    Public Sub SubMenu()
        Dim procesosVigilancia As New Entities.ProcesosVigilancia

        Dim arrLista As New ArrayList
        'Try

        Dim dv As DataView = procesosVigilancia.ObtenerTodos()

        If Not IsNothing(Session(Entities.Usuario.SessionID)) Then
            Dim objUsuario As Entities.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario)

            'If ((objUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM) And Constantes.EsAreaSeprisSnPrec(objUsuario.IdArea)) Then
            'dv.RowFilter = "1=1 AND N_ID_CAT_ADMBLE IN('BDS_C_GR_TIPOS_VISITA', 'BDS_C_GR_OBJETO_VISITA')"
            'End If

            If dv.Count > 0 Then
                For Each fila As DataRow In dv.ToTable.Rows

                    Dim arr(5) As String

                    arr(0) = fila("N_ID_CAT_PV").ToString()
                    arr(1) = fila("T_DSC_CAT_PV").ToString()
                    arrLista.Add(arr)
                Next

                With HttpContext.Current.Response
                    Dim Menu(1) As String
                    .Write("<TABLE cellSpacing=""0"" cellPadding=""0"" width=""400""  >" & vbCrLf)
                    For Each Menu In arrLista
                        .Write("<tr aling =""center"">")
                        .Write("<td width=""232"">")
                        .Write("<table ")
                        .Write("cellspacing=""0"" width=""400"" border=""0"">")
                        .Write("<TR>")
                        .Write("<td valign=""middle"" align=""center"" width=""16"" height=""30""><img src=""../Imagenes/flecha_verde_0.gif""></td>")
                        .Write("<td style=""CURSOR: pointer; font-family: Verdana, Helvetica;font-style: normal; font-size:11px; font-weight: normal; font-variant: normal; text-transform: none; text-decoration: none"" width=""384"" height=""30""")
                        .Write("onclick=""location.href='ProcesoVigilancia.aspx?catalogo=" & Menu(0) & "'"" align=""left"">" & Menu(1))
                        .Write("</td>")
                        .Write("</Tr>")
                        .Write("</table>")
                        .Write("</td>")
                        .Write("</tr>")

                    Next
                    .Write("</table>")
                End With
                'End If
                'Catch ex As Exception
                '  catch_cone(ex, "SubMenu")
                'Finally
                ' If Not Con Is Nothing Then
                'Con.Cerrar()
                'Con = Nothing
            End If
            'End Try
        End If
    End Sub
End Class