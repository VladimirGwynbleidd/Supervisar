Imports AjaxControlToolkit

Public Class CatalogoModif
    Inherits System.Web.UI.Page

    Public Property Mensaje As String
    Private Property CadCamposMostrarGrid As String = "B_FLAG_VIG, F_FECH_INI_VIG, F_FECH_FIN_VIG, B_FLAG_VIGENTE, N_FLAG_VIG, N_FLAG_EN_PRORROGA, N_NUM_ORDEN"

    Private Property psCampoEstatusModif As String
        Get
            If IsNothing(Session("psCampoEstatusModif")) Then
                Return ""
            Else
                Return Session("psCampoEstatusModif").ToString()
            End If
        End Get
        Set(value As String)
            Session("psCampoEstatusModif") = value
        End Set
    End Property

    Private Property pbPermisoEditarCatMod As Boolean
        Get
            If IsNothing(Session("pbPermisoEditarCatMod")) Then
                Return False
            Else
                Return CBool(Session("pbPermisoEditarCatMod"))
            End If
        End Get
        Set(value As Boolean)
            Session("pbPermisoEditarCatMod") = value
        End Set
    End Property

    Private Sub CatalogoModif_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim objCatalogo As New Entities.Catalogos

        If Not IsPostBack Then
            Dim dv As DataView = objCatalogo.ObtenerTodos()
            pbPermisoEditarCatMod = False

            ''Valida que la tabla que se envia por query string tenga permisos para ser editada
            If dv.Count > 0 Then
                For Each fila As DataRow In dv.Table.Rows
                    If fila("N_ID_CAT_ADMBLE").ToString().Trim().ToUpper() = Request.QueryString("catalogo").ToString().Trim().ToUpper() Then
                        pbPermisoEditarCatMod = True
                        Exit For
                    End If
                Next
            End If
        End If

        If Not pbPermisoEditarCatMod Then
            Response.Redirect("~/Mantenimiento/Catalogos.aspx")
        End If

        Dim catTable As String = ""

        If Not Request.QueryString("catalogo") Is Nothing Then
            catTable = Request.QueryString("catalogo")
        Else
            Response.Redirect("~/Mantenimiento/Catalogos.aspx")
        End If

        Select Case catTable
            Case "BDS_C_GR_PASOS", "BDS_C_GR_ESTATUS_CAT", "BDS_C_GR_TIPOS_VISITA", "BDS_C_GR_DIAS_FERIADOS"
                psCampoEstatusModif = "B_FLAG_VIGENTE"
            Case "BDS_C_GR_OBJETO_VISITA"
                psCampoEstatusModif = "N_FLAG_VIG"
            Case Else
                psCampoEstatusModif = "N_FLAG_VIG"
        End Select

        CargaDatosCatalogo(catTable)
        RellenaControles()
        CargaDatos()

    End Sub




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Dim catTable As String = ""
            If Not Request.QueryString("catalogo") Is Nothing Then
                catTable = Request.QueryString("catalogo")
            Else
                Response.Redirect("~/Mantenimiento/Catalogos.aspx")
            End If
            Session.Remove("CatalogoTemp")
            Session.Remove("ExportaGridCatalogo")
        End If
    End Sub
    Private Sub CargaDatosCatalogo(ByVal catTable As String)
        Dim catalogo As New Entities.Catalogo(catTable)
        Dim dv As DataView = catalogo.ObtenerCatalogo()
        Dim Ds As DataSet = Nothing

        lblRegistro.Text = "Registro de " & dv(0)("T_DSC_CAT_ADMBLE")

        dv = catalogo.ObtenerEstructura()
        dv.Table.TableName = catTable


        If dv.Count = 0 Then
            Throw New Exception("No existen datos de la tabla " & catTable)
        Else
            Session("CatalogoTemp") = dv
            BuildControls()
        End If


    End Sub
    Private Sub BuildControls()
        If Not Session("CatalogoTemp") Is Nothing Then
            Try
                Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)
                'Construye controles por cada columna editable
                Dim tabIndex As Int16 = 1
                Dim rowPos As Integer = 0
                For Each row As DataRow In dv.Table.Rows
                    Dim campo As String = Convert.ToString(row("ColumnName"))
                    If Not CadCamposMostrarGrid.Contains(campo) Then
                        Dim tr As New HtmlTableRow()
                        '********LABEL****
                        Dim td As New HtmlTableCell()
                        td.Width = "200px"
                        td.Align = "right"

                        Dim lbl As New Label()
                        lbl.Style.Add("text-align", "right")
                        lbl.CssClass = "txt_gral"
                        Dim obligatorio As String = ""
                        If Convert.ToBoolean(row("NotNull")) Or Convert.ToBoolean(row("ForeignKey")) Then
                            obligatorio = "*"
                        End If
                        If Convert.ToString(row("Description")).Trim.Length > 0 Then
                            lbl.Text = Convert.ToString(row("Description")) & obligatorio & ":"
                        Else
                            lbl.Text = campo & obligatorio & ":"
                        End If

                        td.Controls.Add(lbl)
                        tr.Cells.Add(td)

                        '********CONTROL DE EDICIÓN******
                        td = New HtmlTableCell()
                        td.Width = "400px"
                        td.Align = "left"

                        'Verifica si es llave foránea
                        If Convert.ToBoolean(row("ForeignKey")) Then
                            Dim ddl As New DropDownList()

                            ddl.ToolTip = Convert.ToString(row("Description"))
                            ddl.ID = campo
                            ddl.Width = New Unit(476, UnitType.Pixel)
                            ddl.TabIndex = tabIndex
                            td.Controls.Add(ddl)
                        Else
                            Dim tb As New TextBox()
                            If Convert.ToString(row("Type")).Equals("varchar") Then
                                tb.MaxLength = Convert.ToInt32(row("Length"))
                            End If

                            tb.ToolTip = Convert.ToString(row("Description"))
                            tb.ID = campo
                            tb.TabIndex = tabIndex
                            tb.Width = New Unit(470, UnitType.Pixel)
                            td.Controls.Add(tb)
                        End If

                        tabIndex += 1S

                        Dim tdCal = New HtmlTableCell()
                        tdCal.Width = "400px"
                        tdCal.Align = "left"
                        ''agregar calendario para fechas
                        If Convert.ToString(row("Type")) = "datetime" Or Convert.ToString(row("Type")) = "date" Then
                            Dim lcCalExt As New CalendarExtender
                            lcCalExt.ID = "cal_" & campo
                            lcCalExt.Format = "dd/MM/yyyy"
                            lcCalExt.PopupButtonID = "imgCal_" & campo
                            lcCalExt.TargetControlID = campo
                            tdCal.Controls.Add(lcCalExt)

                            Dim liImageCal As New Image
                            liImageCal.ID = "imgCal_" & campo
                            liImageCal.ImageUrl = "../Imagenes/Calendar.GIF"
                            liImageCal.Width = 16
                            liImageCal.ImageAlign = ImageAlign.Bottom
                            liImageCal.Height = 16
                            tdCal.Controls.Add(liImageCal)

                            tr.Cells.Add(td)
                            tr.Cells.Add(tdCal)
                        Else
                            td.ColSpan = 2
                            tr.Cells.Add(td)
                        End If
                        

                        tblContenido.Rows.Insert(rowPos, tr)
                        rowPos += 1
                    End If
                Next
            Catch ex As Exception
                ' catch_cone(ex, "BuildControls")
            End Try
        End If
    End Sub

    Private Sub RellenaControles()

        Dim catalogo As New Entities.Catalogo

        Dim dv As DataView = catalogo.ObtenerCat(Request.QueryString("catalogo"))

        For index As Integer = 0 To dv.Table.Rows.Count - 1
            Dim campo As String = Convert.ToString(dv.Table.Rows(index)("FKTableColumnName"))
            Dim tablaForanea As String = Convert.ToString(dv.Table.Rows(index)("PKTableName"))
            Dim campoTablaForanea As String = Convert.ToString(dv.Table.Rows(index)("PKTableColumnName"))
            Dim ddl As DropDownList = CType(tblContenido.FindControl(campo), DropDownList)
            RellenaDDL(ddl, tablaForanea, campoTablaForanea)
        Next



    End Sub

    Private Sub RellenaDDL(ByVal ddl As DropDownList, ByVal tablaForanea As String, ByVal columna As String)
        ddl.Items.Clear()


        Dim columnaDescripcion As String = columna

        Dim catalogo As New Entities.Catalogo(Request.QueryString("catalogo"))


        columnaDescripcion = catalogo.ObtenerColumnaForanea(columna)

        Dim dvConsulta As DataView

        If tablaForanea = "BDS_C_GR_USUARIO" Then
            dvConsulta = catalogo.ObtenerDatos("SELECT " & columna & ", " & columnaDescripcion & " FROM " & tablaForanea & " WHERE N_FLAG_VIG = 1")
        ElseIf tablaForanea = "BDS_C_GR_AREAS" Then
            ''VALIDA CATALOGOS POR AREA PARA LA TABLA DE OBJETO DE VISITA
            Dim objUsuario As Entities.Usuario = CType(Session(Entities.Usuario.SessionID), Entities.Usuario)
            If Not IsNothing(objUsuario) Then
                If Constantes.EsAreaSeprisSnPrec(objUsuario.IdArea) Then
                    dvConsulta = catalogo.ObtenerDatos("SELECT " & columna & ", " & columnaDescripcion & " FROM " & tablaForanea & " WHERE B_FLAG_VIGENTE = 1 AND I_ID_AREA=" & objUsuario.IdArea.ToString())
                Else
                    dvConsulta = catalogo.ObtenerDatos("SELECT " & columna & ", " & columnaDescripcion & " FROM " & tablaForanea & " WHERE B_FLAG_VIGENTE = 1")
                End If
            Else
                dvConsulta = catalogo.ObtenerDatos("SELECT " & columna & ", " & columnaDescripcion & " FROM " & tablaForanea & " WHERE B_FLAG_VIGENTE = 1")
            End If
        Else
            dvConsulta = catalogo.ObtenerDatos("SELECT " & columna & ", " & columnaDescripcion & " FROM " & tablaForanea & " WHERE " & psCampoEstatusModif & " = 1")
        End If

        ddl.DataValueField = columna
        ddl.DataTextField = columnaDescripcion
        ddl.DataSource = dvConsulta
        ddl.DataBind()
        If ddl.Items.Count > 0 Then
            ddl.Items.Insert(0, New ListItem("-Seleccione una-", "-1"))
        Else
            ddl.Items.Insert(0, New ListItem("No hay opciones", "-1"))
        End If

    End Sub


    Private Sub CargaDatos()

        Dim catalogo As New Entities.Catalogo(Request.QueryString("catalogo"))
        Dim dvEstructura As DataView = catalogo.ObtenerEstructura()

        If Request.QueryString.Count = 1 Then 'Nuevo
            Try

                For Each row As DataRow In dvEstructura.Table.Rows
                    Dim columna As String = Convert.ToString(row("ColumnName"))
                    If Not CadCamposMostrarGrid.Contains(columna) Then
                        If (Convert.ToBoolean(row("PrimaryKey")) And Not Convert.ToBoolean(row("ForeignKey")) And (Convert.ToString(row("Type")).Equals("numeric")) Or Convert.ToString(row("Type")).Equals("int")) Then

                            Dim tb As TextBox = CType(tblContenido.FindControl(columna), TextBox)
                            tb.Text = catalogo.ObtenerIdSiguiente(columna)
                            tb.Enabled = False
                        End If
                    End If
                Next

            Catch ex As Exception
                'catch_cone(ex, "CargaDatos_Nuevo")
            Finally
            End Try

        Else 'Modificar
            Dim faltanLlaves As Boolean = False
            Dim camposLlave As New List(Of String)
            Try
                Dim Condicion As String = ""
                Dim union As String = ""


                For Each row As DataRow In dvEstructura.Table.Rows
                    Dim columna As String = Convert.ToString(row("ColumnName"))
                    If Not CadCamposMostrarGrid.Contains(columna) Then
                        'Deben haberse enviado todas las llaves primarias
                        If Convert.ToBoolean(row("PrimaryKey")) Then
                            If Not Request.QueryString(columna) Is Nothing Then
                                Condicion &= union & columna & " = " & Request.QueryString(columna)
                                union = " AND "
                                camposLlave.Add(columna)
                            Else
                                faltanLlaves = True
                            End If
                        End If
                    End If
                Next


                Dim dvDatos As DataView = catalogo.ObtenerDatos("SELECT * FROM " & Request.QueryString("catalogo") & " WHERE " & Condicion)

                For index As Integer = 0 To dvDatos.Table.Columns.Count - 1


                    Dim c As Control = tblContenido.FindControl(dvDatos.Table.Columns(index).ColumnName)
                    If TypeOf c Is TextBox Then
                        Dim tb As TextBox = CType(c, TextBox)
                        Dim objGen As Object = dvDatos.Table.Rows(0)(index)

                        Select Case objGen.GetType().Name
                            Case "DateTime", "Date"
                                tb.Text = DirectCast(objGen, Date).ToString("dd/MM/yyyy")
                            Case Else
                                tb.Text = objGen.ToString()
                        End Select

                        If camposLlave.Contains(dvDatos.Table.Columns(index).ColumnName) Then
                            tb.Enabled = False
                        End If

                        'NHM INICIA ODT15-SC01-P23
                        If Request.QueryString("catalogo") = "BDC_C_GR_CLASIFICACION" Then
                            If dvDatos.Table.Columns(index).ColumnName = "T_DSC_CLASIFICACION" Then
                                Session("T_DSC_CLASIFICACION") = Convert.ToString(dvDatos.Table.Rows(0)(index))
                            End If
                        End If
                        'NHM FIN

                    ElseIf TypeOf c Is CheckBox Then

                        If Not IsDBNull(dvDatos.Table.Rows(0)(index)) Then
                            Dim chk As CheckBox = CType(c, CheckBox)
                            chk.Checked = Convert.ToBoolean(dvDatos.Table.Rows(0)(index))
                        End If

                    ElseIf TypeOf c Is DropDownList Then
                        Dim ddl As DropDownList = CType(c, DropDownList)
                        Try
                            ddl.SelectedValue = Convert.ToString(dvDatos.Table.Rows(0)(index))
                        Catch
                            ddl.SelectedIndex = 0
                        End Try
                        If camposLlave.Contains(dvDatos.Table.Columns(index).ColumnName) Then
                            ddl.Enabled = False
                        End If
                    End If
                Next


            Catch ex As Exception

            Finally

            End Try
        End If
    End Sub





    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        If validaForma() Then
            'Manda una alerta para confirmar la operación de insertar o actualizar
            If Request.QueryString().Count = 1 Then
                Mensaje = "¿Está seguro que desea guardar el registro?"
                btnAceptarM2B1A.CommandArgument = "Nuevo"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
            Else
                Mensaje = "¿Está seguro que desea actualizar el registro?"
                btnAceptarM2B1A.CommandArgument = "Actualizacion"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "MensajeConfirmacion();", True)
            End If
        End If
    End Sub

    Private Function validaForma() As Boolean
        Dim msjError As String = ""
        'Dim funciones As New Fun_Generales
        Try
            Dim catTemp As DataView = CType(Session("CatalogoTemp"), DataView)
            For Each row As DataRow In catTemp.Table.Rows
                Dim campo As String = Convert.ToString(row("ColumnName"))
                Dim descripcion As String = Convert.ToString(row("Description"))
                If descripcion.Length = 0 Then
                    descripcion = campo
                End If
                If Not CadCamposMostrarGrid.Contains(campo) Then
                    If Convert.ToBoolean(row("ForeignKey")) Then 'DDL
                        Dim ddl As DropDownList = CType(tblContenido.FindControl(campo), DropDownList)
                        If ddl.SelectedIndex = 0 Then
                            msjError &= "El campo " & descripcion & " es obligatorio." + vbCrLf
                        End If
                    Else 'TextBox
                        Dim tb As TextBox = CType(tblContenido.FindControl(campo), TextBox)
                        If Convert.ToBoolean(row("NotNull")) Then 'Valida que no esté vacío
                            If tb.Text.Length = 0 Then
                                msjError &= "El campo " & descripcion & " es obligatorio." + vbCrLf
                            End If
                        End If
                        If Convert.ToString(row("Type")) = "numeric" And tb.Text.Length > 0 Then
                            Dim scale As Integer = Convert.ToInt32(row("NumericScale"))
                            Dim enteros As Integer = Convert.ToInt32(row("NumericPrecision")) - scale
                            Dim regExp As String = "^(|\+|-)\d{1," & enteros.ToString() & "}"
                            If enteros = 0 Then
                                regExp = "^(|\+|-)0"
                            End If
                            If scale > 0 Then
                                regExp &= "(|\.\d{1," & scale.ToString() & "})"
                            End If
                            regExp &= "$"
                            If Not Regex.IsMatch(tb.Text, regExp) Then
                                msjError &= "Campo " & descripcion & " inválido." + vbCrLf
                            End If
                        End If
                        If Convert.ToString(row("Type")) = "varchar" And tb.Text.Length > 0 Then
                            'Quita los acentos y caractéres alfanuméricos no válidos
                            tb.Text = Utilerias.Generales.AcentosSepris(tb.Text)

                            'Verifica que el campo descripción no este vacío y sea válido
                            'If Not Utilerias.Generales.AlfaNumericoEspacios(tb.Text) Then
                            '    msjError &= "Campo " & descripcion & " inválido." + vbCrLf
                            'End If
                        End If
                        If (Convert.ToString(row("Type")) = "datetime" Or Convert.ToString(row("Type")) = "date") And tb.Text.Length > 0 Then
                            Dim ldFechaAux As Date
                            Dim vecFecha As String() = tb.Text.Split("/")

                            If vecFecha.Length = 3 Then
                                Try
                                    ldFechaAux = New Date(vecFecha(2), vecFecha(1), vecFecha(0))
                                    If IsNothing(ldFechaAux) Then
                                        msjError &= "Campo " & descripcion & " inválido." + vbCrLf
                                    End If
                                Catch : msjError &= "Campo " & descripcion & " inválido." + vbCrLf : End Try
                            Else
                                msjError &= "Campo " & descripcion & " inválido." + vbCrLf
                            End If
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            'catch_cone(ex, "validaForma")
        End Try


        If msjError.Length > 0 Then
            Mensaje = msjError
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionError();", True)
            Return False
        Else
            Return True
        End If
    End Function


    Protected Sub btnAceptarM2B1A_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptarM2B1A.Click

        Dim Campos As String = ""
        Dim coma As String = ""
        Dim Valores As String = ""
        Dim Condicion As String = ""
        Dim union As String = ""


        Dim catTemp As DataView = CType(Session("CatalogoTemp"), DataView)
        Dim cat As New Entities.Catalogo(catTemp.Table.TableName)


        Select Case btnAceptarM2B1A.CommandArgument

            Case "Nuevo"

                For Each row As DataRow In catTemp.Table.Rows

                    Dim campo As String = Convert.ToString(row("ColumnName"))
                    If Not CadCamposMostrarGrid.Contains(campo) Then
                        Campos &= coma & campo
                        Dim valor As String = ""
                        If Convert.ToBoolean(row("ForeignKey")) Then 'DDL
                            Dim ddl As DropDownList = CType(tblContenido.FindControl(campo), DropDownList)
                            If Convert.ToString(row("Type")) = "numeric" Or Convert.ToString(row("Type")) = "int" Then
                                valor = ddl.SelectedValue
                            ElseIf Convert.ToString(row("Type")) = "varchar" Then
                                valor = "'" & ddl.SelectedValue & "'"
                            End If
                        Else 'TextBox
                            Dim tb As TextBox = CType(tblContenido.FindControl(campo), TextBox)
                            If tb.Text.Length = 0 Then
                                valor = "NULL"
                            Else
                                If Convert.ToString(row("Type")) = "numeric" OrElse Convert.ToString(row("Type")) = "int" Then
                                    valor = tb.Text
                                ElseIf Convert.ToString(row("Type")) = "bit" Then
                                    If tb.Text.Trim() = "0" Or tb.Text.Trim() = "1" Then
                                        valor = tb.Text.Trim()
                                    ElseIf tb.Text.Trim().ToUpper() = "TRUE" Or tb.Text.Trim().ToUpper() = "FALSE" Then
                                        Dim valorBoolean As Boolean
                                        valorBoolean = Convert.ToBoolean(tb.Text)
                                        If valorBoolean = True Then
                                            valor = 1
                                        Else
                                            valor = 0
                                        End If
                                    Else
                                        valor = 0
                                    End If

                                ElseIf Convert.ToString(row("Type")) = "varchar" Then
                                    valor = "'" & tb.Text & "'"
                                ElseIf Convert.ToString(row("Type")) = "datetime" Or Convert.ToString(row("Type")) = "date" Then
                                    Dim lsVecFecha As String() = tb.Text.Split("/")
                                    If lsVecFecha.Length = 3 Then
                                        Dim lsFechaAux As DateTime = New DateTime(lsVecFecha(2), lsVecFecha(1), lsVecFecha(0))
                                        valor = "'" & lsFechaAux.ToString("yyyy/MM/dd") & "'"
                                    End If
                                End If
                            End If
                        End If
                        Valores &= coma & valor
                        If Convert.ToBoolean(row("PrimaryKey")) Then

                            Condicion &= union & campo & " = " & valor
                            union = " AND "
                        End If
                        coma = ", "
                    End If
                Next



                If cat.BusquedaReg(Condicion) Then
                    Mensaje = "El registro ya existe."
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionError();", True)
                Else

                    Campos &= coma & psCampoEstatusModif & ", F_FECH_INI_VIG"
                    Valores &= coma & "1, GETDATE()"

                    If cat.Agregar(Campos, Valores) Then
                        RedirigirCatalogo()
                        'Mensaje = "Exito."
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionError();", True)
                    Else
                        Mensaje = "Error al guardar los datos, intente nuevamente."
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionError();", True)
                    End If
                End If


            Case "Actualizacion"

                Dim camposActualizar As New List(Of String)
                Dim valoresActualizar As New List(Of Object)
                Dim camposCondicion As New List(Of String)
                Dim valoresCondicion As New List(Of Object)

                For Each row As DataRow In catTemp.Table.Rows

                    Dim columna As String = Convert.ToString(row("ColumnName"))


                    If Not CadCamposMostrarGrid.Contains(columna) Then


                        If Convert.ToBoolean(row("PrimaryKey")) Then

                            Dim tb As TextBox = CType(tblContenido.FindControl(columna), TextBox)

                            camposCondicion.Add(columna)
                            valoresCondicion.Add(tb.Text)


                        Else
                            camposActualizar.Add(columna)

                            If Convert.ToBoolean(row("ForeignKey")) Then 'DDL
                                Dim ddl As DropDownList = CType(tblContenido.FindControl(columna), DropDownList)

                                valoresActualizar.Add(ddl.SelectedValue)

                            Else 'TextBox
                                Dim tb As TextBox = CType(tblContenido.FindControl(columna), TextBox)

                                If Convert.ToString(row("Type")) = "datetime" Or Convert.ToString(row("Type")) = "date" Then
                                    Dim lsVecFecha As String() = tb.Text.Split("/")
                                    If lsVecFecha.Length = 3 Then
                                        Dim lsFechaAux As DateTime = New DateTime(lsVecFecha(2), lsVecFecha(1), lsVecFecha(0))
                                        valoresActualizar.Add(lsFechaAux.ToString("yyyy/MM/dd"))
                                    End If
                                Else
                                    If tb.Text.Length = 0 Then
                                        valoresActualizar.Add(Nothing)
                                    Else
                                        valoresActualizar.Add(tb.Text)
                                    End If
                                End If
                            End If
                        End If
                    End If

                Next


                If cat.Actualizar(camposActualizar, valoresActualizar, camposCondicion, valoresCondicion) Then
                    RedirigirCatalogo()
                Else
                    Mensaje = "Error al actualizar los datos, intente nuevamente."
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Confirmacion", "ConfirmacionError();", True)
                End If


                btnCancelar_Click(sender, e)
        End Select

    End Sub






    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim dv As DataView = CType(Session("CatalogoTemp"), DataView)

        Session.Remove("CatalogoTemp")
        Session.Remove("ExportaGridCatalogo")
        Response.Redirect("~/Mantenimiento/Catalogo.aspx?catalogo=" & HttpUtility.UrlEncode(dv.Table.TableName))

    End Sub

    Public Function RegistrarBitacora(ByVal descripcion As String, ByVal nombreCatalaogo As String, ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object)) As Boolean
        Dim conexion As New Conexion.SQLServer
        Dim bitac As New Conexion.Bitacora(descripcion, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
        Dim resultado As Boolean = True
        Try
            bitac.Actualizar(nombreCatalaogo, campos, valores, camposCondicion, valoresCondicion, resultado, "")
        Catch ex As Exception

        Finally
            Try : bitac.Finalizar(resultado) : Catch ex As Exception : End Try
        End Try
    End Function

    Private Sub RedirigirCatalogo()
        If Not IsNothing(Request.QueryString("catalogo")) Then
            Response.Redirect("~/Mantenimiento/Catalogo.aspx?catalogo=" & Request.QueryString("catalogo").ToString())
        Else
            Response.Redirect("~/Mantenimiento/Catalogos.aspx")
        End If
    End Sub

End Class