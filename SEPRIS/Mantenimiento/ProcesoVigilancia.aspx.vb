Public Class ProcesoVigilancia
    Inherits System.Web.UI.Page
    Public Property Mensaje As String
    Private Property CadCamposMostrarGrid As String = "B_FLAG_VIG, F_FECH_INI_VIG, F_FECH_FIN_VIG, B_FLAG_VIGENTE, N_FLAG_VIG, N_FLAG_EN_PRORROGA, N_NUM_ORDEN, I_ID_AREA, T_DSC_FLUJO"
    Private Property psCampoEstatus As String
        Get
            If IsNothing(Session("psCampoEstatus")) Then
                Return ""
            Else
                Return Session("psCampoEstatus").ToString()
            End If
        End Get
        Set(value As String)
            Session("psCampoEstatus") = value
        End Set
    End Property
    Private Property psNombreCatalogo As String
        Get
            If IsNothing(Session("psNombreCatalogo")) Then
                Return ""
            Else
                Return Session("psNombreCatalogo").ToString()
            End If
        End Get
        Set(value As String)
            Session("psNombreCatalogo") = value
        End Set
    End Property
    Private Property pbPermisoEditarCat As Boolean
        Get
            If IsNothing(Session("pbPermisoEditarCat")) Then
                Return False
            Else
                Return CBool(Session("pbPermisoEditarCat"))
            End If
        End Get
        Set(value As Boolean)
            Session("pbPermisoEditarCat") = value
        End Set
    End Property
    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim objProcesosVigilancia As New Entities.ProcesosVigilancia

        If Not Request.QueryString("catalogo") Is Nothing Then
            If Not IsPostBack Then
                Dim dv As DataView = objProcesosVigilancia.ObtenerTodos()
                pbPermisoEditarCat = False

                ''Valida que la tabla que se envia por query string tenga permisos para ser editada
                If dv.Count > 0 Then
                    For Each fila As DataRow In dv.Table.Rows
                        If fila("N_ID_CAT_PV").ToString().Trim().ToUpper() = Request.QueryString("catalogo").ToString().Trim().ToUpper() Then
                            pbPermisoEditarCat = True
                            Exit For
                        End If
                    Next
                End If
            End If

            If pbPermisoEditarCat Then
                CargaDatosCatalogo(Request.QueryString("catalogo"))
                BuildDataGrid(Request.QueryString("catalogo"))
            Else
                Response.Redirect("Catalogos.aspx", False)
            End If
        Else
            Response.Redirect("Catalogos.aspx", False)
        End If

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim catTable As String = ""
                If Not Request.QueryString("catalogo") Is Nothing Then
                    catTable = Request.QueryString("catalogo")
                    CargaDatosCatalogo(catTable)
                    RellenaGrid()
                    RellenaControles()

                    If catTable.Trim().ToUpper() = "BDS_C_GR_SUPERVISOR" Or catTable.Trim().ToUpper() = "BDS_C_GR_INSPECTOR" Then
                        btnModificar.Visible = False
                        tdBtnModificar.Width = "5%"
                        tdBtnAgregar.Align = "right"
                        tdBtnEliminar.Align = "left"
                    End If
                Else
                    Response.Redirect("ProcesosVigilancia.aspx", False)
                End If
                Session.Remove("CatDataSet")
                ''Session.Remove("ExportaGridCatalogo")
            End If
        Catch ex As Exception
        End Try
    End Sub

#Region "Funciones"
    Private Sub CargaDatosCatalogo(ByVal catTable As String)
        Dim catalogoVigilancia As New Entities.ProcesosVigilancia(catTable)
        Dim dv As DataView = catalogoVigilancia.ObtenerCatalogoVigilancia()

        'Dim catalogo As New Entities.Catalogo(catTable)
        'Dim dv As DataView = catalogo.ObtenerCatalogo()
        lblCatalogo.Text = "Catálogo " & dv(0)("T_DSC_CAT_PV")
        psNombreCatalogo = dv(0)("T_DSC_CAT_PV").ToString()

        dv = catalogoVigilancia.ObtenerEstructura()
        dv.Table.TableName = catTable

        If Not Request.QueryString("catalogo").Equals("BDS_C_GR_PROCESO") Then

            Dim newRow As DataRowView = dv.AddNew()
            newRow("ColumnName") = "I_ID_AREA"
            newRow("ColumnID") = "0"
            newRow("TypeSchema") = "sys"
            newRow("Type") = "int"
            newRow("UserType") = ""
            newRow("Length") = "4"
            newRow("NumericPrecision") = "10"
            newRow("NumericScale") = "10"
            newRow("PrimaryKey") = "False"
            newRow("PosInPKey") = "0"
            newRow("OrderInPKey") = "0"
            newRow("ForeignKey") = "True"
            newRow("NotNull") = "True"
            newRow("Identity") = "False"
            newRow("Seed") = "0"
            newRow("Increment") = "0"
            newRow("RowGuidCol") = "False"
            newRow("Computed") = "False"
            newRow("ComputedText") = ""
            newRow("UsesDatabaseCollation") = "False"
            newRow("Description") = "Área"

            newRow.EndEdit()
            dv.Sort = "ColumnID"
        End If

        If dv.Count = 0 Then
            Throw New Exception("No existen datos de la tabla " & catTable)
        Else
            Session("CatalogoTemp") = dv
        End If
    End Sub

    Private Sub BuildDataGrid(ByVal catTable)

        If Not Session("CatalogoTemp") Is Nothing Then
            Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)
            'NHM ODT15-SC01-P24, se implementa el select case para la tabla [BDC_C_GR_SUBPROBLEMATICA]

            Select Case dv.Table.TableName
                Case "BDC_C_GR_SUBPROBLEMATICA"
                    Dim colPos As Integer = 2

                Case Else
                    Dim catalogo As String = catTable
                    'Construye columnas en el DataGrid
                    Dim colPos As Integer = 2

                    dv.Table.DefaultView.Sort = "ColumnID"
                    Dim dtDv As DataTable = dv.Table.DefaultView.ToTable

                    For Each row As DataRow In dtDv.Rows
                        Dim campo As String = Convert.ToString(row("ColumnName"))
                        If Convert.ToBoolean(row("ForeignKey")) Then
                            Dim c As New Entities.ProcesosVigilancia()
                            If campo.Equals("I_ID_AREA") Then
                                campo = c.ObtenerColumnaForanea("BDS_C_GR_PROCESO", campo)
                            Else
                                campo = c.ObtenerColumnaForanea(catalogo, campo)
                            End If

                            If Not CadCamposMostrarGrid.Contains(campo) Then
                                Dim col As New BoundColumn()
                                col.DataField = campo
                                col.SortExpression = campo
                                If Convert.ToString(row("Description")).Trim.Length > 0 Then
                                    col.HeaderText = Convert.ToString(row("Description"))
                                    col.HeaderStyle.Width = 250
                                Else
                                    col.HeaderText = campo '& "_" & Convert.ToString(row("ColumnName")).Substring(5)
                                    col.HeaderStyle.Width = 250
                                End If
                                GridPrincipal.Columns.AddAt(colPos, col)
                                colPos += 1
                            End If
                        Else
                            If Not CadCamposMostrarGrid.Contains(campo) Then
                                Dim col As New BoundColumn()
                                col.DataField = campo
                                col.SortExpression = campo
                                If Convert.ToString(row("Description")).Trim.Length > 0 Then
                                    col.HeaderText = Convert.ToString(row("Description"))
                                    col.HeaderStyle.Width = 250
                                Else
                                    col.HeaderText = campo
                                    col.HeaderStyle.Width = 250
                                End If
                                GridPrincipal.Columns.AddAt(colPos, col)

                                colPos += 1
                            End If
                        End If
                    Next
            End Select
        End If
    End Sub
    Public Sub RellenaGrid()
        Dim con As New Entities.ProcesosVigilancia()
        Try
            Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)
            Dim Campos As String = ""
            Dim Joins As String = ""
            Dim coma As String = ""
            'Arma campos a seleccionar

            For Each row As DataRow In dv.Table.Rows
                Dim campo As String = Convert.ToString(row("ColumnName"))
                If Convert.ToBoolean(row("ForeignKey")) Then
                    If campo.Equals("I_ID_AREA") And Not Request.QueryString("catalogo").Equals("BDS_C_GR_PROCESO") Then
                        Dim tablaForanea As String = con.ObtenerTablaForanea("BDS_C_GR_PROCESO", campo)
                        Dim campoForaneo As String = con.ObtenerColumnaForanea("BDS_C_GR_PROCESO", campo)

                        Campos &= coma & tablaForanea & "." & campoForaneo

                        coma = ", " & vbCrLf

                        Joins &= "LEFT JOIN " & tablaForanea & " ON BDS_C_GR_PROCESO." & campo & " = " & tablaForanea & "." & campo & vbCrLf
                    Else
                        Dim tablaForanea As String = con.ObtenerTablaForanea(dv.Table.TableName, campo)
                        Dim campoForaneo As String = con.ObtenerColumnaForanea(dv.Table.TableName, campo)
                        If campoForaneo.Equals("T_DSC_DESCRIPCION_PROCESO") Or campoForaneo.Equals("T_DSC_DESCRIPCION_SUBPROCESO") Then
                            campoForaneo = campoForaneo.Substring(0, 17)
                            Campos &= coma & tablaForanea & "." & campoForaneo & " AS " & campoForaneo & "_" & tablaForanea.Substring(9)
                        Else
                            Campos &= coma & tablaForanea & "." & campoForaneo
                        End If

                        coma = ", " & vbCrLf

                        Joins &= "LEFT JOIN " & tablaForanea & " ON A." & campo & " = " & tablaForanea & "." & campo & vbCrLf
                    End If

                Else
                    If Not CadCamposMostrarGrid.Contains(campo) Then
                        Campos &= coma & "A." & campo
                        coma = ", " & vbCrLf
                    End If
                End If
            Next

            Dim lsCampoVigencia As String = "A.B_FLAG_VIGENTE"
            Dim lsCondicionVigencia As String = " WHERE "

            Select Case dv.Table.TableName
                Case "BDS_C_GR_PROCESO", "BDS_C_GR_SUBPROCESO", "BDS_C_GR_SUPERVISOR", "BDS_C_GR_INSPECTOR"
                    lsCampoVigencia = "A.B_FLAG_VIGENTE"
                    psCampoEstatus = "B_FLAG_VIGENTE"
                Case Else
                    lsCampoVigencia = "A.N_FLAG_VIG"
                    psCampoEstatus = "N_FLAG_VIG"
            End Select

            If RbtnEstatus.SelectedValue <> "" Then
                Select Case CInt(RbtnEstatus.SelectedValue)
                    Case -1
                        lsCondicionVigencia &= "1=1"
                    Case 1
                        lsCondicionVigencia &= lsCampoVigencia & " = 1"
                    Case 0
                        lsCondicionVigencia &= lsCampoVigencia & " = 0"
                End Select
            Else
                lsCondicionVigencia &= "1=1"
            End If

            Dim dvCatalogo As DataView = con.ObtenerDatos("SELECT " & Campos & coma & lsCampoVigencia & " B_FLAG_VIG, A.F_FECH_INI_VIG FROM " & dv.Table.TableName & " A " & Joins & " " & lsCondicionVigencia & TxtSQL.Text & TxtOrder.Text)

            Session("ExportaGridCatalogo") = dvCatalogo
            GridPrincipal.DataSource = dvCatalogo
            GridPrincipal.DataBind()

            'Verifica que el grid contenga elementos
            'Si no contiene ningú elemento el grid es ocultado
            If GridPrincipal.Items.Count() = 0 Then
                GridPrincipal.Visible = False
                LblPrincipal.Visible = True
                btnExcel.Visible = False
            Else
                For i As Integer = 0 To GridPrincipal.Items.Count - 1
                    GridPrincipal.Items(i).Cells(2).HorizontalAlign = HorizontalAlign.Center
                Next

                GridPrincipal.Visible = True
                LblPrincipal.Visible = False
                btnExcel.Visible = True
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub catch_cone(ByVal ex As Exception, ByVal p2 As String)
        Throw New NotImplementedException
    End Sub
    Public Sub RellenaControles()
        Dim con As New Entities.ProcesosVigilancia()
        Try
            ddlFiltro.Items.Clear()
            ddlFiltro.Items.Add(New ListItem("AGREGAR CRITERIO", ""))
            Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)
            For Each row As DataRow In dv.Table.Rows
                'Campo
                Dim campo As String = Convert.ToString(row("ColumnName"))

                If Convert.ToBoolean(row("ForeignKey")) Then
                    If (Request.QueryString("catalogo").Equals("BDS_C_GR_PROCESO") And campo.Equals("I_ID_AREA")) Or Not campo.Equals("I_ID_AREA") Then
                        Dim tablaForanea As String = con.ObtenerTablaForanea(dv.Table.TableName, campo)
                        campo = con.ObtenerColumnaForanea(dv.Table.TableName, campo)
                    End If
                End If

                'Campos &= coma & tablaForanea & "." & campoForaneo

                If Not CadCamposMostrarGrid.Contains(campo) Then
                    Dim li As New ListItem(campo, campo)
                    If Convert.ToString(row("Description")).Trim.Length > 0 Then
                        li.Text = Convert.ToString(row("Description"))
                    End If
                    ddlFiltro.Items.Add(li)
                End If
            Next
        Catch ex As Exception
            catch_cone(ex, "RellenaControles")
        End Try
    End Sub
    Public Function ObtenerImagenEstatus(ByVal estatus As Boolean) As String

        If estatus Then
            Return "~/Imagenes/vigente.gif"
        Else
            Return "~/Imagenes/no_vigente.gif"
        End If

    End Function
    Sub LimpiarControles()

        ddlOpciones.Items.Clear()
        ddlOpciones.Visible = False
        ddlFiltro.SelectedValue = ""
        TxtSQL.Text = String.Empty


    End Sub
    Public Sub ExportarGrid()
        Dim Reference As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)
        Reference.Add("SIPAJ")
        Reference.Add(Session("Usuario").ToString())
        Reference.Add(DateTime.Today.ToString("dd/MM/yyyy"))

        Dim HeaderColumns As System.Collections.Generic.List(Of String()) = New System.Collections.Generic.List(Of String())
        For Each gridColumn As DataGridColumn In GridPrincipal.Columns
            Select Case gridColumn.GetType().Name
                Case "ButtonColumn"
                    'Select
                Case "BoundColumn"
                    If gridColumn.Visible Then
                        HeaderColumns.Add({DirectCast(gridColumn, BoundColumn).HeaderText, DirectCast(gridColumn, BoundColumn).DataField})
                    End If
                Case "TemplateColumn"
                    'Estatus
                    If gridColumn.Visible Then
                        HeaderColumns.Add({DirectCast(gridColumn, TemplateColumn).HeaderText, psCampoEstatus})
                    End If
            End Select
        Next
    End Sub

    Private Sub GridPrincipal_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles GridPrincipal.PageIndexChanged
        'Cambia el grid a la nueva página
        'y quita cualquier selección de registro
        Try
            GridPrincipal.CurrentPageIndex = e.NewPageIndex
            GridPrincipal.SelectedIndex = -1

            If Not Session("CatalogoTemp") Is Nothing Then
                RellenaGrid()
            End If

        Catch ex As Exception
            catch_cone(ex, "GridPrincipal_PageIndexChanged")
        Finally
        End Try
    End Sub
    Private Sub GridPrincipal_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GridPrincipal.SortCommand
        'Cambia el ordenamiento de los datos del grid
        Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)
        Try
            TxtOrder.Text = " ORDER BY " & e.SortExpression

            'Quita cualquier selección de registro
            GridPrincipal.SelectedIndex = -1

            'NHM ODT15-SC01-P24, se implementa el select case para la tabla [BDC_C_GR_SUBPROBLEMATICA]
            If Not Session("CatalogoTemp") Is Nothing Then
                RellenaGrid()
            End If

        Catch ex As Exception
            catch_cone(ex, "GridPrincipal_SortCommand")
        End Try
    End Sub
    Private Sub GridPrincipal_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GridPrincipal.DeleteCommand
        'Selecciona el registro
        Try
            'Cambio del color el registro seleccionado en el grid
            GridPrincipal.SelectedItemStyle.BackColor = System.Drawing.Color.FromArgb(208, 114, 95)
            '152, 76, 76
            GridPrincipal.SelectedItemStyle.Font.Bold = True
            GridPrincipal.SelectedItemStyle.ForeColor = System.Drawing.Color.White
            GridPrincipal.SelectedIndex = e.Item.ItemIndex
        Catch ex As Exception
            catch_cone(ex, "GridPrincipal_DeleteCommand")
        End Try
    End Sub
    Private Sub btnFiltrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFiltrar.Click
        Try

            If Not Session("CatalogoTemp") Is Nothing Then
                Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)
                Select Case ddlFiltro.SelectedValue
                    Case "T_DSC_DESCRIPCION_PROCESO"
                        TxtSQL.Text = " AND " & "BDS_C_GR_PROCESO.T_DSC_DESCRIPCION" & " =  '" & ddlOpciones.SelectedItem.Value & "'"
                    Case "T_DSC_DESCRIPCION_SUBPROCESO"
                        TxtSQL.Text = " AND " & "BDS_C_GR_SUBPROCESO.T_DSC_DESCRIPCION" & " =  '" & ddlOpciones.SelectedItem.Value & "'"
                    Case "T_DSC_DESCRIPCION"
                        TxtSQL.Text = " AND " & "A." & ddlFiltro.SelectedValue & " =  '" & ddlOpciones.SelectedItem.Value & "'"
                    Case "T_ID_USUARIO"
                        TxtSQL.Text = " AND " & "A." & ddlFiltro.SelectedValue & " =  '" & ddlOpciones.SelectedItem.Value & "'"
                    Case ""
                        'Elimina el filtrado
                        TxtSQL.Text = ""
                    Case Else
                        'Filtra por cualquier tipo de campo que sea de tipo carácter
                        TxtSQL.Text = " AND " & ddlFiltro.SelectedValue & " =  '" & ddlOpciones.SelectedItem.Value & "'"
                End Select
                'Pone al grid en la primera página
                GridPrincipal.CurrentPageIndex = 0

                'Quita cualquier selección de registro
                GridPrincipal.SelectedIndex = -1

                'Llena el grid con los datos
                RellenaGrid()
            End If

        Catch ex As Exception
            catch_cone(ex, "btnfiltrar")
        End Try
    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)
        Session("Registrar") = "Registrar"

        Session.Remove("CatalogoTemp")
        Session.Remove("ExportaGridCatalogo")
        Response.Redirect("~/Mantenimiento/ProcesoVigilanciaModif.aspx?catalogo=" & HttpUtility.UrlEncode(dv.Table.TableName))
    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click

        Session("Registrar") = "Modificar"

        Try
            'Verifica que haya un registro seleccionado
            If GridPrincipal.SelectedIndex <> -1 Then
                'Verifica que el tipo de registro seleccionado se pueda modificar
                If CBool(GridPrincipal.SelectedItem.Cells(1).Text) Then
                    Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)
                    Dim queryString As String = "catalogo=" & HttpUtility.UrlEncode(dv.Table.TableName)
                    For col As Integer = 2 To GridPrincipal.Columns.Count - 2 'Recorre las columnas creadas dinamicamente, se brinca el botón y B_FLAG_VIG y al final omite ESTATUS
                        Dim row As DataRow = Nothing
                        For Each row In dv.Table.Rows
                            'Se agrega al query string si son llave
                            If Convert.ToString(row("ColumnName")).Equals(CType(GridPrincipal.Columns(col), BoundColumn).DataField) Then
                                Exit For
                            End If
                        Next
                        If (Convert.ToBoolean(row("PrimaryKey"))) Then
                            queryString &= "&" & Convert.ToString(row("ColumnName")) & "="
                            Select Case Convert.ToString(row("Type"))
                                Case "varchar"
                                    queryString &= HttpUtility.UrlEncode("'" & GridPrincipal.SelectedItem.Cells(col).Text & "'")
                                Case "date" 'PENDIENTE: No existen PK de tipo date en el proyecto actual, por el momento se toma como varchar
                                    queryString &= HttpUtility.UrlEncode("'" & GridPrincipal.SelectedItem.Cells(col).Text & "'")
                                Case Else 'Incluye numeric
                                    queryString &= HttpUtility.UrlEncode(GridPrincipal.SelectedItem.Cells(col).Text)
                            End Select
                        End If
                    Next
                    Session.Remove("CatDataSet")
                    Session.Remove("ExportaGridCatalogo")
                    Response.Redirect("~/Mantenimiento/ProcesoVigilanciaModif.aspx?" & queryString)

                Else
                    'Quita cualquier selección de registro
                    GridPrincipal.SelectedIndex = -1
                End If
            Else
                Mensaje = "Seleccione un registro"
                btnAceptarM2B1A.CommandArgument = ""

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeError();", True)
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Try
            'Verifica que haya un registro seleccionado
            If GridPrincipal.SelectedIndex <> -1 Then
                'Verifica que el tipo de registro seleccionado se pueda eliminar
                If CBool(GridPrincipal.SelectedItem.Cells(1).Text) Then

                    Select Case Request.QueryString("catalogo")
                        Case "BDS_C_GR_PROCESO"
                            Mensaje = "Está seguro de eliminar el Proceso y todos los subprocesos, recuerda que al eliminar un proceso también eliminas supervisores e Inspectores asignados a este proceso"
                        Case "BDS_C_GR_SUBPROCESO"
                            Mensaje = "Esta seguro de eliminar el subproceso y todos los supervisores e inspectores asignado"
                        Case "BDS_C_GR_SUPERVISOR"
                            Mensaje = "Esta seguro de eliminar al supervisor asignado"
                        Case "BDS_C_GR_INSPECTOR"
                            Mensaje = "Esta seguro de eliminar el inspector asignado"
                    End Select


                    btnAceptarM2B1A.CommandArgument = "Eliminar"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionEliminar();", True)
                Else
                    Mensaje = "Registro de sólo lectura"
                    btnAceptarM2B1A.CommandArgument = ""

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeError();", True)

                    GridPrincipal.SelectedIndex = -1
                End If
            Else
                Mensaje = "Seleccione un registro"
                btnAceptarM2B1A.CommandArgument = ""

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeError();", True)
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Try
            Dim objUsuario As Entities.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
            Dim dv As DataView = CType(Session("ExportaGridCatalogo"), DataView)
            Dim utl As New Utilerias.ExportarExcel
            Dim referencias As New List(Of String)
            referencias.Add(objUsuario.IdentificadorUsuario.ToString)
            referencias.Add(Now.ToString)
            If Not IsNothing(dv) Then
                utl.ExportaGrid(dv.ToTable(), GridPrincipal, "CATALOGO " & psNombreCatalogo.ToUpper(), referencias)
            End If

        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Eventos Controles"
    Private Sub RbtnEstatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbtnEstatus.SelectedIndexChanged
        Try

            GridPrincipal.CurrentPageIndex = 0
            GridPrincipal.SelectedIndex = -1
            LimpiarControles()

            If Not Session("CatalogoTemp") Is Nothing Then
                RellenaGrid()
            End If

        Catch ex As Exception
            catch_cone(ex, "RbtnEstatus_SelectedIndexChanged")
        End Try
    End Sub
    Private Sub ddlFiltro_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFiltro.SelectedIndexChanged
        Dim Con As New Entities.ProcesosVigilancia

        Try
            If ddlFiltro.SelectedItem.Text.ToUpper().Equals("AGREGAR CRITERIO") Then
                ddlOpciones.Visible = False
            Else
                Con = New Entities.ProcesosVigilancia
                Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)
                'NHM INICIA ODT15-SC01-P24
                'Codigo nuevo:
                Dim oDs As DataView
                Select Case dv.Table.TableName
                    Case "BDS_C_GR_PROCESO"
                        Select Case ddlFiltro.SelectedValue
                            Case "T_DSC_AREA"
                                Dim valueSelected As String = "T_DSC_AREA"
                                oDs = Con.ObtenerDatos("SELECT " & valueSelected & " FROM [dbo].[BDS_C_GR_AREAS] " &
                                                       " WHERE NOT " & valueSelected & " IS NULL " &
                                                       " AND [B_FLAG_VIGENTE] = 1 " &
                                                       " ORDER BY " & valueSelected)

                        End Select
                    Case "BDS_C_GR_SUBPROCESO"
                        Select Case ddlFiltro.SelectedValue
                            Case "T_DSC_DESCRIPCION_PROCESO"
                                Dim valueSelected As String = "T_DSC_DESCRIPCION"

                                oDs = Con.ObtenerDatos("SELECT " & valueSelected & " AS T_DSC_DESCRIPCION_PROCESO FROM " & "dbo.BDS_C_GR_PROCESO" &
                                             " WHERE NOT " & valueSelected & " IS NULL " &
                                             " AND B_FLAG_VIGENTE = 1 " &
                                             " ORDER BY " & valueSelected)
                            Case "T_DSC_TIPO_DIA"
                                oDs = Con.ObtenerDatos("SELECT S.I_ID_TIPO_DIA, " & ddlFiltro.SelectedValue & " FROM [BDC_C_GR_SUBPROBLEMATICA] AS S " &
                                                " INNER JOIN [BDC_C_GR_TIPO_DIA] AS TD ON  S.[I_ID_TIPO_DIA]=TD.[I_ID_TIPO_DIA] " &
                                                " WHERE S.[B_FLAG_VIG] = 1 And TD.[B_FLAG_VIG] = 1 " &
                                                " AND NOT " & ddlFiltro.SelectedValue & " IS NULL " &
                                                " GROUP BY S.I_ID_TIPO_DIA, " & ddlFiltro.SelectedValue &
                                                " ORDER BY " & ddlFiltro.SelectedValue)
                            Case Else
                                oDs = Con.ObtenerDatos("SELECT DISTINCT(" & ddlFiltro.SelectedValue & ") FROM " & "dbo." & dv.Table.TableName &
                                              " WHERE NOT " & ddlFiltro.SelectedValue & " IS NULL ORDER BY " & ddlFiltro.SelectedValue)

                        End Select
                    Case "BDS_C_GR_SUPERVISOR"
                        Select Case ddlFiltro.SelectedValue
                            Case "T_DSC_DESCRIPCION_PROCESO"
                                Dim valueSelected As String = "T_DSC_DESCRIPCION"
                                oDs = Con.ObtenerDatos("SELECT " & valueSelected & " AS T_DSC_DESCRIPCION_PROCESO FROM " & "dbo.BDS_C_GR_PROCESO" &
                                             " WHERE NOT " & valueSelected & " IS NULL " &
                                             " AND B_FLAG_VIGENTE = 1 " &
                                             " ORDER BY " & valueSelected)
                            Case "T_DSC_DESCRIPCION_SUBPROCESO"
                                Dim valueSelected As String = "T_DSC_DESCRIPCION"
                                oDs = Con.ObtenerDatos("SELECT " & valueSelected & " AS T_DSC_DESCRIPCION_SUBPROCESO FROM " & "dbo.BDS_C_GR_SUBPROCESO" &
                                             " WHERE NOT " & valueSelected & " IS NULL " &
                                             " AND B_FLAG_VIGENTE = 1 " &
                                             " ORDER BY " & valueSelected)
                            Case Else
                                oDs = Con.ObtenerDatos("SELECT DISTINCT(" & ddlFiltro.SelectedValue & ") FROM " & "dbo." & dv.Table.TableName &
                                              " WHERE NOT " & ddlFiltro.SelectedValue & " IS NULL ORDER BY " & ddlFiltro.SelectedValue)
                        End Select
                    Case "BDS_C_GR_INSPECTOR"
                        Select Case ddlFiltro.SelectedValue
                            Case "T_DSC_DESCRIPCION_PROCESO"
                                Dim valueSelected As String = "T_DSC_DESCRIPCION"
                                oDs = Con.ObtenerDatos("SELECT " & valueSelected & " AS T_DSC_DESCRIPCION_PROCESO FROM " & "dbo.BDS_C_GR_PROCESO" &
                                             " WHERE NOT " & valueSelected & " IS NULL " &
                                             " AND B_FLAG_VIGENTE = 1 " &
                                             " ORDER BY " & valueSelected)
                            Case "T_DSC_DESCRIPCION_SUBPROCESO"
                                Dim valueSelected As String = "T_DSC_DESCRIPCION"
                                oDs = Con.ObtenerDatos("SELECT " & valueSelected & " AS T_DSC_DESCRIPCION_SUBPROCESO FROM " & "dbo.BDS_C_GR_SUBPROCESO" &
                                             " WHERE NOT " & valueSelected & " IS NULL " &
                                             " AND B_FLAG_VIGENTE = 1 " &
                                             " ORDER BY " & valueSelected)
                            Case Else
                                oDs = Con.ObtenerDatos("SELECT DISTINCT(" & ddlFiltro.SelectedValue & ") FROM " & "dbo." & dv.Table.TableName &
                                              " WHERE NOT " & ddlFiltro.SelectedValue & " IS NULL ORDER BY " & ddlFiltro.SelectedValue)
                        End Select
                    Case Else
                        oDs = Con.ObtenerDatos("SELECT DISTINCT(" & ddlFiltro.SelectedValue & ") FROM " & "dbo." & dv.Table.TableName &
                                              " WHERE NOT " & ddlFiltro.SelectedValue & " IS NULL ORDER BY " & ddlFiltro.SelectedValue)
                End Select
                'Codigo anterior:
                'Dim oDs As DataView = Con.ObtenerDatos("SELECT DISTINCT(" & ddlFiltro.SelectedValue & ") FROM " & "dbo." & dv.Table.TableName & _
                '" WHERE NOT " & ddlFiltro.SelectedValue & " IS NULL ORDER BY " & ddlFiltro.SelectedValue)
                'NHM FIN

                If oDs.Table.Rows.Count > 0 Then
                    ddlOpciones.DataSource = oDs
                    ddlOpciones.DataTextField = ddlFiltro.SelectedValue

                    If ddlFiltro.SelectedValue = "T_DSC_TIPO_DIA" Then
                        ddlOpciones.DataValueField = "I_ID_TIPO_DIA"
                    Else
                        ddlOpciones.DataValueField = ddlFiltro.SelectedValue
                    End If

                    ddlOpciones.DataBind()
                Else
                    ddlOpciones.Items.Clear()
                    ddlOpciones.Items.Add("No hay opciones disponibles")
                End If
                ddlOpciones.Visible = True
            End If
        Catch ex As Exception
            catch_cone(ex, "DrpFiltro_SelectedIndexChanged")
        Finally
            If Not Con Is Nothing Then
                Con = Nothing
            End If
        End Try
    End Sub

#End Region
    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click
        Dim Campos As String = ""
        Dim coma As String = ""
        Dim Valores As String = ""
        Dim Condicion As String = ""
        Dim union As String = ""

        Dim catTemp As DataView = CType(Session("CatalogoTemp"), DataView)
        'Dim catalogo As New Entities.Catalogo(catTemp.Table.TableName)
        Dim catalogo As New Entities.ProcesosVigilancia(catTemp.Table.TableName)
        catalogo.CampoEstatus = psCampoEstatus

        Dim primaryKey As String = String.Empty

        Select Case btnAceptarM2B1A.CommandArgument
            Case "Eliminar"

                Select Case Request.QueryString("catalogo")
                    Case "BDS_C_GR_SUPERVISOR"
                        Dim query As String = "UPDATE S
SET F_FECH_FIN_VIG = GETDATE(), [B_FLAG_VIGENTE] = 0
FROM [dbo].[BDS_C_GR_SUPERVISOR] S INNER JOIN [dbo].[BDS_C_GR_PROCESO] P ON S.I_ID_PROCESO = P.I_ID_PROCESO
INNER JOIN [dbo].[BDS_C_GR_SUBPROCESO] SP ON S.I_ID_SUBPROCESO = SP.I_ID_SUBPROCESO
INNER JOIN [dbo].[BDS_C_GR_USUARIO] U ON S.T_ID_USUARIO = U.T_ID_USUARIO
WHERE P.T_DSC_DESCRIPCION = '" & GridPrincipal.SelectedItem.Cells(3).Text & "'
AND SP.T_DSC_DESCRIPCION = '" & GridPrincipal.SelectedItem.Cells(4).Text & "'
AND U.T_ID_USUARIO = '" & GridPrincipal.SelectedItem.Cells(5).Text & "'"
                        catalogo.Ejecutar(query, "Borrar supervisor")
                        btnFiltrar_Click(sender, e)
                    Case "BDS_C_GR_INSPECTOR"
                        Dim query As String = "UPDATE I
SET F_FECH_FIN_VIG = GETDATE(), [B_FLAG_VIGENTE] = 0
FROM [dbo].[BDS_C_GR_INSPECTOR] I INNER JOIN [dbo].[BDS_C_GR_PROCESO] P ON I.I_ID_PROCESO = P.I_ID_PROCESO
INNER JOIN [dbo].[BDS_C_GR_SUBPROCESO] SP ON I.I_ID_SUBPROCESO = SP.I_ID_SUBPROCESO
INNER JOIN [dbo].[BDS_C_GR_USUARIO] U ON I.T_ID_USUARIO = U.T_ID_USUARIO
WHERE P.T_DSC_DESCRIPCION = '" & GridPrincipal.SelectedItem.Cells(3).Text & "'
AND SP.T_DSC_DESCRIPCION = '" & GridPrincipal.SelectedItem.Cells(4).Text & "'
AND U.T_ID_USUARIO = '" & GridPrincipal.SelectedItem.Cells(5).Text & "'"

                        catalogo.Ejecutar(query, "Borrar inspector")
                        btnFiltrar_Click(sender, e)
                    Case Else
                        For Each row As DataRow In catTemp.Table.Rows
                            If Convert.ToBoolean(row("PrimaryKey")) Then
                                primaryKey = Convert.ToString(row("ColumnName"))
                                Exit For
                            End If
                        Next

                        For index As Integer = 2 To GridPrincipal.Columns.Count - 2
                            If Convert.ToString(primaryKey).Equals(CType(GridPrincipal.Columns(index), BoundColumn).DataField) Then
                                Dim camposCondicion As New List(Of String)
                                Dim valoresCondicion As New List(Of Object)

                                camposCondicion.Add(primaryKey)
                                valoresCondicion.Add(GridPrincipal.SelectedItem.Cells(index).Text)

                                catalogo.Baja(camposCondicion, valoresCondicion, "Borrar")

                                btnFiltrar_Click(sender, e)
                                Exit For
                            End If
                        Next
                End Select

                ActualizarRegistros()
        End Select
    End Sub


    Protected Sub ActualizarRegistros()
        Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)
        Response.Redirect("~/Mantenimiento/ProcesoVigilancia.aspx?catalogo=" & HttpUtility.UrlEncode(dv.Table.TableName))
    End Sub

    Protected Sub BtnRegresar_Click(sender As Object, e As EventArgs) Handles BtnRegresar.Click
        Response.Redirect("~/Mantenimiento/ProcesosVigilancia.aspx")
    End Sub
End Class