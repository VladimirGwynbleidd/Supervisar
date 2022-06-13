Public Class ImgProceso
    Inherits System.Web.UI.Page
    Public Property Mensaje As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnCargarImg_Click(sender As Object, e As EventArgs) Handles btnCargarImg.Click
        imgUnBotonNoAccion.ImageUrl = Constantes.Imagenes.Aviso

        ''Valida que haya una archivo
        If fuImagenProc.HasFile Then

            Dim lsExtArchivo As String = System.IO.Path.GetExtension(fuImagenProc.FileName)

            ''Valida el tipo de archivo
            If lsExtArchivo.ToLower() <> ".jpg" And lsExtArchivo.ToLower() <> ".png" Then
                Dim errores As New Entities.EtiquetaError(2168)
                Mensaje = errores.Descripcion
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MostrarAviso();", True)
                Exit Sub
            Else
                ''Cargar la imagen al servidor en la ruta de las imagenes del proceso de inspeccion
                Try

                    Dim tipoImagenProceso As String = ""

                    Select Case CInt(rdOpcImgProc.SelectedValue)
                        Case 1
                            tipoImagenProceso = Constantes.Parametros.NombreImagenProcesoInspeccionSancVF
                        Case 2
                            tipoImagenProceso = Constantes.Parametros.NombreImagenProcesoInspeccionSancOtras
                    End Select

                    If AccesoBD.ActualizaParametroBD(fuImagenProc.FileName, tipoImagenProceso) Then
                        Dim path As String = Server.MapPath("~/Imagenes/ProcesoInspSan/")

                        If path <> "" Then
                            fuImagenProc.SaveAs(path & fuImagenProc.FileName)

                            Mensaje = "Se actualizo la imagen correctamente."
                            imgUnBotonNoAccion.ImageUrl = Constantes.Imagenes.Exito
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MostrarAviso();", True)
                            Exit Sub
                        Else
                            Mensaje = "No se pudo obtener la ruta para almacenar la imagen en el servidor, reportar a sistemas."
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MostrarAviso();", True)
                            Exit Sub
                        End If
                    Else
                        imgUnBotonNoAccion.ImageUrl = Constantes.Imagenes.Fallo
                        Mensaje = "No se pudo actualizar la imagen en Base de Datos, reportar a sistemas."
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MostrarAviso();", True)
                        Exit Sub
                    End If


                Catch ex As Exception
                    imgUnBotonNoAccion.ImageUrl = Constantes.Imagenes.Fallo
                    Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "ImgProceso.aspx.vb, btnCargarImg_Click", "")
                    Mensaje = "No se pudo adjuntar la imagen, reportar a sistemas."
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MostrarAviso();", True)
                    Exit Sub
                End Try
            End If
        Else
            Dim errores As New Entities.EtiquetaError(2168)
            Mensaje = errores.Descripcion
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MostrarAviso();", True)
            Exit Sub
        End If
    End Sub
End Class