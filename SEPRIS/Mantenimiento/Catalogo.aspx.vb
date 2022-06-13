Imports System.Web.Configuration
Public Class Catalogo
    Inherits System.Web.UI.Page

    Public Property Mensaje As String
    Private Property CadCamposMostrarGrid As String = "B_FLAG_VIG, F_FECH_INI_VIG, F_FECH_FIN_VIG, B_FLAG_VIGENTE, N_FLAG_VIG, N_FLAG_EN_PRORROGA, N_NUM_ORDEN, I_ID_AREA"

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
        Dim objCatalogo As New Entities.Catalogos

        If Not Request.QueryString("catalogo") Is Nothing Then
            If Not IsPostBack Then
                Dim dv As DataView = objCatalogo.ObtenerTodos()
                pbPermisoEditarCat = False

                ''Valida que la tabla que se envia por query string tenga permisos para ser editada
                If dv.Count > 0 Then
                    For Each fila As DataRow In dv.Table.Rows
                        If fila("N_ID_CAT_ADMBLE").ToString().Trim().ToUpper() = Request.QueryString("catalogo").ToString().Trim().ToUpper() Then
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

                    If catTable.Trim().ToUpper() = "BDS_C_GR_ESTATUS_CAT" Or catTable.Trim().ToUpper() = "BDS_C_GR_PASOS" Then
                        btnAgregar.Visible = False
                        btnEliminar.Visible = False
                    End If
                Else
                    Response.Redirect("Catalogos.aspx", False)
                End If
                Session.Remove("CatDataSet")
                ''Session.Remove("ExportaGridCatalogo")
            End If
        Catch ex As Exception
        End Try
    End Sub
#Region "Funciones"


    Private Sub CargaDatosCatalogo(ByVal catTable As String)
        Dim catalogo As New Entities.Catalogo(catTable)
        Dim dv As DataView = catalogo.ObtenerCatalogo()
        lblCatalogo.Text = "Catálogo " & dv(0)("T_DSC_CAT_ADMBLE")
        psNombreCatalogo = dv(0)("T_DSC_CAT_ADMBLE").ToString()

        dv = catalogo.ObtenerEstructura()
        dv.Table.TableName = catTable

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
                    For Each row As DataRow In dv.Table.Rows

                        Dim campo As String = Convert.ToString(row("ColumnName"))
                        If Convert.ToBoolean(row("ForeignKey")) Then
                            Dim c As New Entities.Catalogo()


                            campo = c.ObtenerColumnaForanea(catalogo, campo)

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
        Dim con As New Entities.Catalogo()
        Try

            Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)
            Dim Campos As String = ""
            Dim Joins As String = ""
            Dim coma As String = ""
            'Arma campos a seleccionar

            For Each row As DataRow In dv.Table.Rows

                Dim campo As String = Convert.ToString(row("ColumnName"))


                If Convert.ToBoolean(row("ForeignKey")) Then

                    Dim tablaForanea As String = con.ObtenerTablaForanea(dv.Table.TableName, campo)
                    Dim campoForaneo As String = con.ObtenerColumnaForanea(dv.Table.TableName, campo)

                    Campos &= coma & tablaForanea & "." & campoForaneo
                    coma = ", " & vbCrLf

                    Joins &= "LEFT JOIN " & tablaForanea & " ON A." & campo & " = " & tablaForanea & "." & campo & vbCrLf

                Else

                    If Not CadCamposMostrarGrid.Contains(campo) Then
                        Campos &= coma & "A." & campo
                        coma = ", " & vbCrLf
                    End If

                End If

            Next

            Dim lsCampoVigencia As String = "A.B_FLAG_VIG"
            Dim lsCondicionVigencia As String = " WHERE "

            Select Case dv.Table.TableName
                Case "BDS_C_GR_PASOS", "BDS_C_GR_ESTATUS_CAT", "BDS_C_GR_TIPOS_VISITA", "BDS_C_GR_DIAS_FERIADOS"
                    lsCampoVigencia = "A.B_FLAG_VIGENTE"
                    psCampoEstatus = "B_FLAG_VIGENTE"
                Case "BDS_C_GR_OBJETO_VISITA"
                    lsCampoVigencia = "A.N_FLAG_VIG"
                    psCampoEstatus = "N_FLAG_VIG"

                    ''VALIDA CATALOGOS POR AREA PARA LA TABLA DE OBJETO DE VISITA
                    Dim objUsuario As Entities.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
                    If Not IsNothing(objUsuario) Then
                        If ((objUsuario.IdentificadorPerfilActual = Constantes.PERFIL_ADM) And Constantes.EsAreaSeprisSnPrec(objUsuario.IdArea)) Then
                            lsCondicionVigencia &= " A.I_ID_AREA = " & objUsuario.IdArea.ToString() & " AND "
                        End If
                    End If
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
        Dim con As New Entities.Catalogo()
        Try
            ddlFiltro.Items.Clear()
            ddlFiltro.Items.Add(New ListItem("SIN FILTRO", ""))
            Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)
            For Each row As DataRow In dv.Table.Rows
                'Campo
                Dim campo As String = Convert.ToString(row("ColumnName"))

                If Convert.ToBoolean(row("ForeignKey")) Then
                    Dim tablaForanea As String = con.ObtenerTablaForanea(dv.Table.TableName, campo)
                    campo = con.ObtenerColumnaForanea(dv.Table.TableName, campo)
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
                Select Case ddlFiltro.SelectedValue
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

        Session.Remove("CatalogoTemp")
        Session.Remove("ExportaGridCatalogo")
        Response.Redirect("~/Mantenimiento/CatalogoModif.aspx?catalogo=" & HttpUtility.UrlEncode(dv.Table.TableName))
    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
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
                    Response.Redirect("~/Mantenimiento/CatalogoModif.aspx?" & queryString)

                Else
                    'MsgBox1.ShowMessage("Registro de sólo lectura.")
                    'Quita cualquier selección de registro
                    GridPrincipal.SelectedIndex = -1
                End If
            Else
                'MsgBox1.ShowMessage("Seleccione un registro.")
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

                    Mensaje = "¿Está seguro que desea eliminar el registro?"
                    btnAceptarM2B1A.CommandArgument = "Eliminar"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionEliminar();", True)

                Else

                    Mensaje = "Registro de sólo lectura"
                    btnAceptarM2B1A.CommandArgument = ""

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionError();", True)

                    GridPrincipal.SelectedIndex = -1
                End If
            Else


                Mensaje = "Seleccione un registro"
                btnAceptarM2B1A.CommandArgument = ""

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionError();", True)


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
        Dim Con As New Entities.Catalogo

        Try
            If ddlFiltro.SelectedItem.Text.ToUpper().Equals("SIN FILTRO") Then
                ddlOpciones.Visible = False
            Else
                Con = New Entities.Catalogo
                Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)
                'NHM INICIA ODT15-SC01-P24
                'Codigo nuevo:
                Dim oDs As DataView
                Select Case dv.Table.TableName
                    Case "BDC_C_GR_SUBPROBLEMATICA"

                        Select Case ddlFiltro.SelectedValue
                            Case "T_DSC_PROBLEMATICA"
                                oDs = Con.ObtenerDatos("SELECT " & ddlFiltro.SelectedValue & " FROM " & "dbo.BDC_C_GR_PROBLEMATICA" & _
                                             " WHERE NOT " & ddlFiltro.SelectedValue & " IS NULL " & _
                                             " AND B_FLAG_VIG = 1 " & _
                                             " ORDER BY " & ddlFiltro.SelectedValue)
                            Case "T_DSC_TIPO_DIA"
                                oDs = Con.ObtenerDatos("SELECT S.I_ID_TIPO_DIA, " & ddlFiltro.SelectedValue & " FROM [BDC_C_GR_SUBPROBLEMATICA] AS S " & _
                                                " INNER JOIN [BDC_C_GR_TIPO_DIA] AS TD ON  S.[I_ID_TIPO_DIA]=TD.[I_ID_TIPO_DIA] " & _
                                                " WHERE S.[B_FLAG_VIG] = 1 And TD.[B_FLAG_VIG] = 1 " &
                                                " AND NOT " & ddlFiltro.SelectedValue & " IS NULL " & _
                                                " GROUP BY S.I_ID_TIPO_DIA, " & ddlFiltro.SelectedValue & _
                                                " ORDER BY " & ddlFiltro.SelectedValue)
                            Case Else
                                oDs = Con.ObtenerDatos("SELECT DISTINCT(" & ddlFiltro.SelectedValue & ") FROM " & "dbo." & dv.Table.TableName & _
                                              " WHERE NOT " & ddlFiltro.SelectedValue & " IS NULL ORDER BY " & ddlFiltro.SelectedValue)

                        End Select

                    Case "BDS_C_GR_OBJETO_VISITA"
                        Select Case ddlFiltro.SelectedValue
                            Case "T_DSC_AREA"
                                oDs = Con.ObtenerDatos("SELECT DISTINCT A.T_DSC_AREA " & _
                                                      " FROM dbo.BDS_C_GR_AREAS A " & _
                                                      " JOIN BDS_C_GR_OBJETO_VISITA OV ON OV.I_ID_AREA = A.I_ID_AREA " & _
                                                      " WHERE Not A.T_DSC_AREA Is NULL " & _
                                                      " ORDER BY A.T_DSC_AREA ")
                        End Select
                    Case Else
                        oDs = Con.ObtenerDatos("SELECT DISTINCT(" & ddlFiltro.SelectedValue & ") FROM " & "dbo." & dv.Table.TableName & _
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
        Dim catalogo As New Entities.Catalogo(catTemp.Table.TableName)
        catalogo.CampoEstatus = psCampoEstatus

        Dim primaryKey As String = String.Empty

        Select Case btnAceptarM2B1A.CommandArgument

            Case "Eliminar"



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

                        catalogo.Baja(camposCondicion, valoresCondicion)

                        btnFiltrar_Click(sender, e)

                        Exit For

                    End If

                Next
        End Select
    End Sub
End Class