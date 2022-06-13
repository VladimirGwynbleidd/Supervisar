Imports Clases
Imports System.Diagnostics
Public Class FolioConsecutivo
    Inherits System.Web.UI.Page
    Private Const ForFecha As String = "yyyy/MM/dd HH:mm:ss"
    Private sModulo As String = ConfigurationSettings.AppSettings("EventLogModuloSecurity")
    Private SQL As String = "SELECT * FROM " & Conexion.Owner & "BDA_VIEW_INI_FOLIOS "
    Private Const PAGINA As String = "Folio_s_Consecutivo.aspx"
    Private Const CVECATALOGO As String = "ID_INI_FOLIO"
    Private TABLA As String = Conexion.Owner & "BDA_INI_FOLIOS"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        verificaSesion()
        verificaPerfil()
        Try
            If Not IsPostBack Then
                'Selecciona la opción de solo mostrar los registros vigentes
                TxtVigencia.Text = RbtnEstatus.SelectedValue

                'Hace un ordenamiento inicial de los datos por clave
                TxtOrder.Text = " ORDER BY ID_INI_FOLIO "

                'Llena el grid con los datos
                RellenaGrid()

                'Llena el Dropdownlist con las opciones para filtro
                DrpFiltro.Items.Add(New ListItem("SIN FILTRO", ""))
                DrpFiltro.Items.Add(New ListItem("CLAVE", CVECATALOGO))
                DrpFiltro.Items.Add(New ListItem("AREA", "ID_UNIDAD_ADM"))
                DrpFiltro.Items.Add(New ListItem("TIPO_DOC", "TIPO_DOC"))
                DrpFiltro.Items.Add(New ListItem("AÑO", "ANIO"))

            End If
        Catch ex As Exception
            catch_cone(ex, "Page_Load")
        End Try
    End Sub

    Private Sub cmdFiltrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFiltrar.Click
        Dim Funciones As New Fun_Generales
        Dim exito As Boolean
        Try
            'Quita carácteres alfanuméricos inválidos y convierte a mayúsculas
            TxtBuscar.Text = UCase(Funciones.Acentos(TxtBuscar.Text))

            'Verifica que no sea vacía o inválida
            exito = Funciones.AlfaNumericoEspacios(TxtBuscar.Text)

            Select Case DrpFiltro.SelectedValue
                Case ""
                    'Elimina el filtrado
                    TxtSQL.Text = ""
                Case CVECATALOGO
                    'Filtra por clave
                    'If Funciones.Numerico(TxtBuscar.Text) Then
                    '    TxtSQL.Text = " AND CONVERT(" & DrpFiltro.SelectedValue & ",'US7ASCII') LIKE " & "'%" & TxtBuscar.Text & "%'"
                    'Else
                    '    MsgBox1.ShowMessage("No se Puede Realizar el Filtro de Registros, Revise el Filtro")
                    'End If

                    TxtSQL.Text = " AND " & DrpFiltro.SelectedValue & " = " & "'" & ddlOpciones.SelectedItem.Value & "'"
                Case Else
                    'Filtra por cualquier tipo de campo que sea de tipo carácter
                    'TxtSQL.Text = " AND " & DrpFiltro.SelectedValue & " LIKE  '%" & TxtBuscar.Text & "%'"
                    TxtSQL.Text = " AND " & DrpFiltro.SelectedValue & " =" & ddlOpciones.SelectedItem.Value & ""
            End Select
            'Pone al grid en la primera página
            GridPrincipal.CurrentPageIndex = 0

            'Quita cualquier selección de registro
            GridPrincipal.SelectedIndex = -1
            Clave.Text = ""

            'Llena el grid con los datos
            RellenaGrid()
        Catch ex As Exception
            catch_cone(ex, "btnfiltrar")
        Finally
        End Try
    End Sub


    Sub LimpiarControles()
        Try
            ddlOpciones.Items.Clear()
            ddlOpciones.Visible = False
            DrpFiltro.SelectedValue = ""
            TxtSQL.Text = String.Empty
        Catch ex As Exception

        End Try
    End Sub

    Public Sub RellenaGrid()
        Dim Con As New Conexion()
        Try
            Dim oDs As New DataSet
            oDs = Con.Datos(SQL & TxtVigencia.Text & TxtSQL.Text & TxtOrder.Text, False)
            Session("ExportaGridIniFolios") = oDs
            GridPrincipal.DataSource = oDs
            GridPrincipal.DataBind()
            'Verifica que el grid contenga elementos
            'Si no contiene ningú elemento el grid es ocultado
            If GridPrincipal.Items.Count() = 0 Then
                GridPrincipal.Visible = False
                LblPrincipal.Visible = True
                bExcel.Visible = False
            Else
                GridPrincipal.Visible = True
                LblPrincipal.Visible = False
                bExcel.Visible = True
            End If
        Catch ex As Exception
            catch_cone(ex, "RellenaGrid")
        Finally
            Con.Cerrar()
            Con = Nothing
        End Try
    End Sub
    Private Sub verificaSesion()
        Dim logout As Boolean = False
        Dim Sesion As Seguridad = Nothing
        Try
            Sesion = New Seguridad
            'Verifica la sesion de usuario
            Select Case Sesion.ContinuarSesionAD()
                Case -1
                    logout = True
                Case 0, 3
                    logout = True
            End Select
        Catch ex As Exception
            catch_cone(ex, "verificaSesion")
        Finally
            If Not Sesion Is Nothing Then
                Sesion.CerrarCon()
                Sesion = Nothing
            End If
        End Try
        If logout Then
            If Request.Browser.EcmaScriptVersion.Major >= 1 Then
                Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
                Response.End()
            Else
                Response.Redirect("~/logout.aspx")
            End If
        End If
    End Sub
    Private Sub verificaPerfil()
        Dim logout As Boolean = False
        Dim Perfil As Perfil = Nothing
        Try
            Perfil = New Perfil
            'Verifica que el usuario este autorizado para ver esta página
            If Not Perfil.Autorizado("Security/seg_c_areas.aspx") Then
                logout = True
            End If
        Catch ex As Exception
            catch_cone(ex, "verificaPerfil")
        Finally
            If Not Perfil Is Nothing Then
                Perfil.CerrarCon()
                Perfil = Nothing
            End If
        End Try
        If logout Then
            If Request.Browser.EcmaScriptVersion.Major >= 1 Then
                Response.Write("<script>window.open(""../logout.aspx"",""_top"");</script>")
                Response.End()
            Else
                Response.Redirect("~/logout.aspx")
            End If
        End If
    End Sub
    Public Sub catch_cone(ByVal e As Exception, ByVal s As String)
        EventLogWriter.EscribeEntrada("Modulo: " & sModulo & " Funcion " & s & ": " & e.Message & " at " & e.StackTrace(), EventLogEntryType.Error)
    End Sub

    'Devuelve la ruta y nombre de archivo de la imagen correspondiente a la vigencia
    Public Function ImagenVigencia(ByVal Vigente As Integer) As String
        If CBool(Vigente) Then
            Return "../imagenes/OK.jpg"
        Else
            Return "../imagenes/ERROR.jpg"
        End If
    End Function

    Protected Sub RbtnEstatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RbtnEstatus.SelectedIndexChanged
        Try

            TxtVigencia.Text = RbtnEstatus.SelectedValue.ToString

            'Pone al grid en la primera página
            GridPrincipal.CurrentPageIndex = 0
            'Quita cualquier selección de registro
            GridPrincipal.SelectedIndex = -1
            Clave.Text = ""
            LimpiarControles()
            'Llena el grid con los datos
            RellenaGrid()
        Catch ex As Exception
            catch_cone(ex, "RbtnEstatus_SelectedIndexChanged")
        Finally
        End Try
    End Sub

    Protected Sub DrpFiltro_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DrpFiltro.SelectedIndexChanged
        Select Case DrpFiltro.SelectedItem.Text.ToUpper()
            Case "SIN FILTRO"
                RellenaGrid()
                ddlOpciones.Visible = False
            Case "CLAVE"
                CargaComboOpciones1("ID_INI_FOLIO", "ID_INI_FOLIO")
                EliminaRepetidosCombo(ddlOpciones)
            Case "AREA"
                CargaComboOpciones("ID_UNIDAD_ADM,DSC_UNIDAD_ADM", "ID_UNIDAD_ADM", "BDA_C_UNIDAD_ADM")
                EliminaRepetidosCombo(ddlOpciones)
            Case "TIPO_DOC"
                CargaComboOpciones("ID_TIPO_DOCUMENTO,T_TIPO_DOCUMENTO", "ID_TIPO_DOCUMENTO", "BDA_TIPO_DOCUMENTO")
                EliminaRepetidosCombo(ddlOpciones)
            Case "AÑO"
                CargaComboOpciones1("ANIO", "ANIO")
                EliminaRepetidosCombo(ddlOpciones)

        End Select
    End Sub
    Public Sub EliminaRepetidosCombo(ByVal oCombo As System.Web.UI.WebControls.DropDownList)
        Dim iElementos, iIndices As Int32
        Try
            For iElementos = 0 To oCombo.Items.Count - 2
                For iIndices = oCombo.Items.Count - 1 To iElementos + 1 Step -1
                    If oCombo.Items(iElementos).ToString = oCombo.Items(iIndices).ToString Then
                        oCombo.Items.RemoveAt(iIndices)
                    End If
                Next
            Next
        Catch
        End Try
    End Sub
    Private Sub CargaComboOpciones(ByVal sCampo As String, ByVal sValor As String, ByVal Tabla As String)
        Dim Con As New Conexion()
        Dim oDs As New DataSet
        Dim sSql As String = "SELECT " & sCampo & " FROM " & Conexion.Owner & Tabla

        Try

            oDs = Con.Datos(sSql, False)
            If oDs.Tables(0).Rows.Count > 0 Then
                With ddlOpciones
                    .DataSource = oDs
                    .DataTextField = oDs.Tables(0).Columns.Item(1).ToString
                    .DataValueField = oDs.Tables(0).Columns.Item(0).ToString
                    .DataBind()
                    .Visible = True
                End With
                ddlOpciones.Items.Insert(0, New ListItem("-Seleccionar-", "-1"))
            Else
                With ddlOpciones
                    .Items.Insert(0, "No hay opciones disponibles")
                    .Visible = True
                End With
            End If

        Catch ex As Exception
            Dim lMensaje As String = ex.ToString()
        Finally
            Clave.Text = ""
            Con.Cerrar()
            Con = Nothing
        End Try

    End Sub
    Private Sub CargaComboOpciones1(ByVal sCampo As String, ByVal sValor As String)
        Dim Con As New Conexion()
        Dim oDs As New DataSet
        Dim sSql As String = "SELECT " & sCampo & " FROM " & Conexion.Owner & "BDA_INI_FOLIOS"

        Try

            oDs = Con.Datos(sSql, False)
            If oDs.Tables(0).Rows.Count > 0 Then
                With ddlOpciones
                    .DataSource = oDs
                    .DataTextField = sValor
                    .DataValueField = sValor
                    .DataBind()
                    .Visible = True
                End With
                ddlOpciones.Items.Insert(0, New ListItem("-Seleccionar-", "-1"))
            Else
                With ddlOpciones
                    .Items.Insert(0, "No hay opciones disponibles")
                    .Visible = True
                End With
            End If

        Catch ex As Exception
            Dim lMensaje As String = ex.ToString()
        Finally
            Clave.Text = ""
            Con.Cerrar()
            Con = Nothing
        End Try

    End Sub

    Protected Sub bExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bExcel.Click
        ExportarGrid()
    End Sub
    Private Sub GridPrincipal_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GridPrincipal.DeleteCommand
        'Selecciona el registro
        Dim Index As Integer
        Try
            Clave.Text = e.Item.Cells(1).Text
            Index = e.Item.ItemIndex
            'Cambio del color el registro seleccionado en el grid
            GridPrincipal.SelectedItemStyle.BackColor = System.Drawing.Color.FromArgb(208, 114, 95)
            GridPrincipal.SelectedItemStyle.Font.Bold = True
            GridPrincipal.SelectedItemStyle.ForeColor = System.Drawing.Color.White
            GridPrincipal.SelectedIndex = Index
        Catch ex As Exception
            catch_cone(ex, "GridPrincipal_DeleteCommand")
        Finally
        End Try
    End Sub
    Public Sub ExportarGrid()
        Try

            Dim oClases As New Clases.Fun_Generales

            Session("NombreExcel") = "CatalogoIniFolios.xls"
            Session("TituloExcel") = "Cat&aacute;logo de Inicializador de Folios"
            Session("ExportaGrid") = oClases.ExportarGrid(CType(Session("ExportaGridIniFolios"), DataSet), GridPrincipal)
            Response.Redirect("../exportagrid.aspx", False)
        Catch ex As Exception
            catch_cone(ex, "Exportar_Grid")
        Finally

        End Try
    End Sub

    Protected Sub cmdEliminar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdEliminar.Click
        'Verifica que haya un registro seleccionado
        If Clave.Text <> "" Then
            'Verifica que el tipo de registro seleccionado se pueda eliminar
            MsgBox1.ShowConfirmation("Esta seguro que desea eliminar el Registro ?", "BORRAR", True, False)
        Else
            MsgBox1.ShowMessage("Seleccione un registro.")
        End If
    End Sub
    Private Sub MsgBox1_YesChoosed(ByVal sender As Object, ByVal Key As String) Handles MsgBox1.YesChoosed
        Dim Con As New Conexion()
        Dim Sesion As Seguridad = Nothing
        Dim Condicion, Valores As String
        Dim Exito As Boolean = False
        Dim Tran As Odbc.OdbcTransaction
        Try
            Sesion = New Seguridad
            'Exito = Con.IniciaTran
            Tran = Con.BeginTran
            Exito = True

            If Exito Then
                Condicion = CVECATALOGO & "=" & Clave.Text & ""
                Valores = "VIG_FLAG=0, FECHA_FIN_VIG= " & Conexion.Owner & "TO_DATE('" & Date.Now.ToString(ForFecha) & "','YYYY/MM/DD HH24:MI:SS')"
                'Hace el borrado lógico, actualizando la vigencia a cero 
                'y poniendo la fecha final de vigencia a la fecha y hora actual
                Exito = Con.Actualizar(TABLA, Valores, Condicion, Tran)
                If Exito Then
                    'Inserta el registro correspondiente a la actividad realizada en la Bitácora
                    'exito=sesion.bitacora(101)
                    'Quita cualquier selección de registro
                    GridPrincipal.SelectedIndex = -1
                    If Exito Then
                        Tran.Commit()
                        'Con.CommitTran()

                        Exito = Sesion.bitacora(6, "SE ELIMINÓ UN ID INICIALIZADOR CON CLAVE " & Clave.Text)

                        'Pone al grid en la primera página
                        GridPrincipal.CurrentPageIndex = 0

                        'Llena el grid con los datos
                        RellenaGrid()
                    Else
                        Tran.Rollback()
                        'Con.RollBackTran()
                        MsgBox1.ShowMessage("Error al eliminar los datos, intente nuevamente.")
                    End If
                    Clave.Text = ""
                Else
                    Tran.Rollback()
                    'Con.RollBackTran()
                    MsgBox1.ShowMessage("Error al eliminar los datos, intente nuevamente.")
                End If
            Else
                MsgBox1.ShowMessage("Error al eliminar los datos, intente nuevamente.")
            End If
        Catch ex As Exception
            catch_cone(ex, "msgbox1yeschoosed")
        Finally
            If Not Sesion Is Nothing Then
                Sesion.CerrarCon()
                Sesion = Nothing
            End If
            If Not Con Is Nothing Then
                Con.Cerrar()
                Con = Nothing

            End If
        End Try
    End Sub

    Protected Sub cmdAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAgregar.Click
        Response.Redirect(PAGINA & "?Id=-1", False)
    End Sub
    Private Sub GridPrincipal_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles GridPrincipal.PageIndexChanged
        'Cambia el grid a la nueva página
        'y quita cualquier selección de registro
        Try
            GridPrincipal.CurrentPageIndex = e.NewPageIndex
            GridPrincipal.SelectedIndex = -1
            Clave.Text = ""

            'Llena el grid con los datos
            RellenaGrid()
        Catch ex As Exception
            catch_cone(ex, "GridPrincipal_PageIndexChanged")
        Finally
        End Try
    End Sub
    Private Sub GridPrincipal_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GridPrincipal.SortCommand
        'Cambia el ordenamiento de los datos del grid
        Try
            TxtOrder.Text = " ORDER BY " & e.SortExpression

            'Quita cualquier selección de registro
            GridPrincipal.SelectedIndex = -1
            Clave.Text = ""

            'Llena el grid con los datos
            RellenaGrid()
        Catch ex As Exception
            catch_cone(ex, "GridPrincipal_SortCommand")
        Finally
        End Try
    End Sub


    Protected Sub cmdModificar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdModificar.Click
        'Verifica que haya un registro seleccionado
        If Clave.Text <> "" Then
            'Verifica que el tipo de registro seleccionado se pueda modificar

            Response.Redirect(PAGINA & "?Id=" & Clave.Text & "")

          
        End If

        MsgBox1.ShowMessage("Seleccione un registro.")

    End Sub
End Class