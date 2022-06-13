'- Fecha de creación:8/07/2013
'- Fecha de modificación: 
'- Nombre del Responsable: ARGC1
'- Empresa: Softtek
'- Pagina del carrusel de asignaciones



Imports Negocio
Imports Utilerias



Public Class CarruselAsignacion
    Inherits Page

  

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            RellenaGrid()

            If Not (Convert.ToBoolean(Web.Configuration.WebConfigurationManager.AppSettings("Desarrollo").ToString()) = True) Then
                BtnSiguienteAsig.Style.Value = "Display:none"
            End If

        End If
    End Sub

    Public Sub RellenaGrid()

        Dim oDs As New DataTable
        oDs = Carrusel.ObtenerUsuarios

        gvCarrusel.DataSource = oDs
        gvCarrusel.DataBind()

        '---------------------------------------------------
        '   Verifica que el grid contenga elementos
        '   Si no contiene ningú elemento el grid es ocultado
        '---------------------------------------------------

        If gvCarrusel.Rows.Count() = 0 Then
            gvCarrusel.Style.Value = "display:none"

        Else

            gvCarrusel.Style.Value = "display:block"
            gvCarrusel.Visible = True

        End If

    End Sub

    Private Sub rellenaGrid_preInit()
        ViewState("cUsuario") = Nothing

    End Sub


    Public Function verificaRecibe(ByVal recibe As Object) As Boolean
        If Not IsDBNull(recibe) Then
            If Not (recibe.ToString.Equals("0")) Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function


    Protected Friend Sub chkRecibe_CheckedChanged(ByVal source As Object, e As EventArgs)

        '-----------------------------------------------------------
        'Convertir a Checkbox
        '-----------------------------------------------------------
        Dim chk As CheckBox = TryCast(source, CheckBox)
        '-----------------------------------------------------------
        'Obtener el DataGridItem
        '-----------------------------------------------------------
        'Dim row As DataGridItem = TryCast(chk.NamingContainer, DataGridItem)
        Dim row As GridViewRow = TryCast(chk.NamingContainer, GridViewRow)

        Dim usuario As String = row.Cells(1).Text

        '-----------------------------------------------------------
        ' ver si existe el usuario en la tabla del carrusel
        '-----------------------------------------------------------

        If Not Carrusel.EstaENCarrusel(usuario) Then

            '-----------------------------------------------------------
            ' insertar el usuario en el carrusel
            '-----------------------------------------------------------
            If Not Carrusel.InsertaEnCarusel(usuario) Then

                Dim errores As New Entities.EtiquetaError(137)
                lblTextoMensaje.Text = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "mensaje", "MensajeModal()", True)
                RellenaGrid()
                Exit Sub

            End If
        End If

        If Carrusel.RecibeChanged(usuario, chk.Checked) Then
            rellenaGrid_preInit()
            RellenaGrid()

        Else
            Dim errores As New Entities.EtiquetaError(137)
            lblTextoMensaje.Text = errores.Descripcion
            imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "mensaje", "MensajeModal()", True)
        End If

    End Sub



    Protected Sub gvCarrusel_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCarrusel.RowCommand
        Dim posicion_actual As Integer = Nothing
        Dim usuario_actual As String = Nothing

        Try

            '-------------------------------------------------------------
            ' Verificar que esté seleccionado la casilla de "Recibir", si no está habilitado no podrá subir o bajar de posición o seleccionarse.
            '-------------------------------------------------------------

            ' Convert the row index stored in the CommandArgument
            ' property to an Integer.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ' Retrieve the row that contains the button clicked 
            ' by the user from the Rows collection.
            Dim row As GridViewRow = gvCarrusel.Rows(index)

            Dim chk As CheckBox = TryCast(row.Cells(6).Controls.Item(1), CheckBox) ' columna Recibe

            If Not chk.Checked Then
                Dim errores As New Entities.EtiquetaError(138)
                lblTextoMensaje.Text = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "mensaje", "MensajeModal()", True)
                Exit Sub
            End If

            '--------------------------------------------------------------
            ' Verificar que la posición 3 (orden) no sea nulo o cero.
            '--------------------------------------------------------------

            If String.IsNullOrEmpty(row.Cells(3).Text) OrElse row.Cells(3).Text = "&nbsp;" OrElse row.Cells(3).Text = 0 Then
                posicion_actual = 0
                usuario_actual = row.Cells(1).Text
            Else
                posicion_actual = CInt(row.Cells(3).Text)
                usuario_actual = row.Cells(1).Text
            End If


            If Carrusel.ActualizaOrden(usuario_actual, e.CommandName, posicion_actual) Then
                RellenaGrid()
            Else

                Dim errores As New Entities.EtiquetaError(139)
                lblTextoMensaje.Text = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "mensaje", "MensajeModal()", True)
            End If

        Catch ex As Exception
            'ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        End Try

    End Sub

    Private Sub cmdModificar_Click(sender As Object, e As EventArgs) Handles cmdModificar.Click
        Try

            If Not gvCarrusel.SelectedIndex = "-1" Then

                If gvCarrusel.SelectedRow.Cells(3).Text = "0" Then 'si no tiene orden no recibe

                    Dim errores As New Entities.EtiquetaError(138)
                    lblTextoMensaje.Text = errores.Descripcion
                    imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "mensaje", "MensajeModal()", True)

                Else
                    If Not String.IsNullOrEmpty(gvCarrusel.SelectedRow.Cells(1).Text) Then
                        Session("cUsuario") = gvCarrusel.SelectedRow.Cells(1).Text
                        Session("nUsuario") = gvCarrusel.SelectedRow.Cells(2).Text

                        Response.Redirect("PeriodosNoAsignacion.aspx", False)

                    End If

                End If
            Else
                Dim errores As New Entities.EtiquetaError(140)
                lblTextoMensaje.Text = errores.Descripcion
                imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & errores.Imagen.Ruta
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "mensaje", "MensajeModal()", True)
            End If

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        End Try
    End Sub


   

    Protected Sub BtnSiguienteAsig_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSiguienteAsig.Click
        Dim mensaje As New Entities.EtiquetaError(141)
        lblTextoMensaje.Text = mensaje.Descripcion & Carrusel.ObtenerSiguienteAsignacion()
        imgUnBotonNoAccion.ImageUrl = Entities.Imagen.RutaCarpeta & mensaje.Imagen.Ruta
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "mensaje", "MensajeModal()", True)

    End Sub


    Private Sub btnExportaExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportaExcel.Click

        Dim utl As New Utilerias.ExportarExcel
        Dim referencias As New List(Of String)
        referencias.Add(CType(Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario.ToString)
        referencias.Add(Now.ToString)

        Dim dt As DataTable = TryCast(gvCarrusel.DataSourceSession, DataTable)
        dt.Columns("N_FLAG_VIG").ColumnName = "Recibe"
        dt.Columns.Add("Subir")
        dt.Columns.Add("Bajar")

        utl.ExportaGrid(dt, gvCarrusel, "Carrusel de Asignaciones", referencias)

    End Sub

End Class