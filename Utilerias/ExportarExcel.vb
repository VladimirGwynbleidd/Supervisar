'- Fecha de creación: 24/07/2013
'- Nombre del Responsable: Rafael Rodríguez Sánchez
'- Empresa: Softtek
'- Modulo para esportar a excel un GridView

Imports System.Web.UI.WebControls
Imports System.Web.Configuration

Public Class ExportarExcel

    ''' <summary>
    ''' Exporta el datasource de un GridView a excel
    ''' </summary>
    ''' <param name="dt">Datatable con el datasource del GridView</param>
    ''' <param name="gv">Gridview del cual se obtendran las columnas</param>
    ''' <param name="nombreBandeja">Nombre de la bandeja o pantalla, sera el nombre de la tabla de excel</param>
    ''' <param name="referencias">Lista de String de las referencias</param>
    ''' <remarks></remarks>
    Public Sub ExportaGrid(ByVal dt As DataTable, ByVal gv As GridView, ByVal nombreBandeja As String, Optional ByVal referencias As List(Of String) = Nothing)

        Try
            Dim headerColumns As List(Of String()) = New List(Of String())

            For Each gridcolumn As DataControlField In gv.Columns

                Select Case gridcolumn.GetType().Name
                    Case "CommandField"
                        'Select
                    Case "BoundField"
                        If gridcolumn.Visible And Not String.IsNullOrWhiteSpace(DirectCast(gridcolumn, BoundField).DataField) Then
                            headerColumns.Add({DirectCast(gridcolumn, BoundField).HeaderText, DirectCast(gridcolumn, BoundField).DataField})
                        End If
                    Case "TemplateField"
                        'Estatus
                        If gridcolumn.Visible And Not String.IsNullOrWhiteSpace(DirectCast(gridcolumn, TemplateField).HeaderText) Then
                            If DirectCast(gridcolumn, TemplateField).HeaderText <> "Estatus Paso Actual" And DirectCast(gridcolumn, TemplateField).HeaderText <> "Subfolios" And DirectCast(gridcolumn, TemplateField).HeaderText <> "DOCUMENTOS ADJUNTOS" Then
                                If DirectCast(gridcolumn, TemplateField).HeaderText = "Documento" Then
                                    headerColumns.Add({DirectCast(gridcolumn, TemplateField).HeaderText, "T_NOM_MACHOTE_ORI"})
                                ElseIf DirectCast(gridcolumn, TemplateField).HeaderText = "Fecha de Envío a Sanciones" Then
                                    headerColumns.Add({DirectCast(gridcolumn, TemplateField).HeaderText, DirectCast(gridcolumn, TemplateField).HeaderText})
                                End If
                            End If
                        End If
                End Select
            Next

            If IsNothing(referencias) Then
                referencias = New List(Of String)
            End If

            'Cuando las referencias son diferentes de 3 se desconfigura el encabezado
            'Por lo que si son menores a 3 se agregan
            For index = referencias.Count To 2
                referencias.Add("")
            Next

            Dim export As New OpenXML.ExportExcel()
            export.SheetName = nombreBandeja
            export.TableName = nombreBandeja
            export.HeaderColor = WebConfigurationManager.AppSettings.Item("HeaderColorExcel")
            export.HeaderForeColor = WebConfigurationManager.AppSettings.Item("HeaderForeColorExcel")
            export.CellForeColor = WebConfigurationManager.AppSettings.Item("CellForeColorExcel")
            export.ShowGridLines = True
            export.Reference = referencias
            export.HeaderColumns = headerColumns
            export.DataSource = dt
            export.CreatePackage(nombreBandeja)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Warning, "AccesoBD, ExportaGridDocs", "")
        End Try
        

    End Sub

    Public Sub ExportaGrid(ByVal dt As DataTable, ByVal gv As DataGrid, ByVal nombreBandeja As String, Optional ByVal referencias As List(Of String) = Nothing)
        Try
            Dim headerColumns As List(Of String()) = New List(Of String())

            For Each gridcolumn As Object In gv.Columns

                Select Case gridcolumn.GetType().Name
                    Case "ButtonColumn"
                        'nada
                    Case "CommandField"
                        'Select
                    Case "BoundField", "BoundColumn"
                        If gridcolumn.Visible And Not String.IsNullOrWhiteSpace(gridcolumn.HeaderText) Then
                            headerColumns.Add({gridcolumn.HeaderText, gridcolumn.DataField})
                        End If

                    Case "TemplateField"
                        'Estatus
                        If gridcolumn.Visible And Not String.IsNullOrWhiteSpace(gridcolumn.HeaderText) Then
                            If gridcolumn.HeaderText <> "ESTATUS" Then
                                headerColumns.Add({gridcolumn.HeaderText, gridcolumn.HeaderText})
                            End If
                        End If
                End Select
            Next

            If IsNothing(referencias) Then
                referencias = New List(Of String)
            End If

            'Cuando las referencias son diferentes de 3 se desconfigura el encabezado
            'Por lo que si son menores a 3 se agregan
            For index = referencias.Count To 2
                referencias.Add("")
            Next

            Dim export As New OpenXML.ExportExcel()
            export.SheetName = nombreBandeja
            export.TableName = nombreBandeja
            export.HeaderColor = WebConfigurationManager.AppSettings.Item("HeaderColorExcel")
            export.HeaderForeColor = WebConfigurationManager.AppSettings.Item("HeaderForeColorExcel")
            export.CellForeColor = WebConfigurationManager.AppSettings.Item("CellForeColorExcel")
            export.ShowGridLines = True
            export.Reference = referencias
            export.HeaderColumns = headerColumns
            export.DataSource = dt
            export.CreatePackage(nombreBandeja)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ExportaGrid", "")
        End Try

    End Sub

    Public Sub ExportaGridDocs(ByVal dt As DataTable, ByVal gv As GridView, ByVal nombreBandeja As String, Optional ByVal referencias As List(Of String) = Nothing)

        Try
            Dim headerColumns As List(Of String()) = New List(Of String())

            For Each gridcolumn As DataControlField In gv.Columns

                Select Case gridcolumn.GetType().Name
                    Case "CommandField"
                        'Select
                    Case "BoundField"
                        If gridcolumn.Visible And Not String.IsNullOrWhiteSpace(DirectCast(gridcolumn, BoundField).DataField) Then
                            headerColumns.Add({DirectCast(gridcolumn, BoundField).HeaderText, DirectCast(gridcolumn, BoundField).DataField})
                        End If
                End Select
            Next

            If IsNothing(referencias) Then
                referencias = New List(Of String)
            End If

            'Cuando las referencias son diferentes de 3 se desconfigura el encabezado
            'Por lo que si son menores a 3 se agregan
            For index = referencias.Count To 2
                referencias.Add("")
            Next

            Dim export As New OpenXML.ExportExcel()
            export.SheetName = nombreBandeja
            export.TableName = nombreBandeja
            export.HeaderColor = WebConfigurationManager.AppSettings.Item("HeaderColorExcel")
            export.HeaderForeColor = WebConfigurationManager.AppSettings.Item("HeaderForeColorExcel")
            export.CellForeColor = WebConfigurationManager.AppSettings.Item("CellForeColorExcel")
            export.ShowGridLines = True
            export.Reference = referencias
            export.HeaderColumns = headerColumns
            export.DataSource = dt
            export.CreatePackage(nombreBandeja)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "AccesoBD, ExportaGridDocs", "")
        End Try

    End Sub

    Public Sub ExportaGridBandejaPC(ByVal dt As DataTable, ByVal gv As GridView, ByVal nombreBandeja As String, Optional ByVal referencias As List(Of String) = Nothing)

        Try
            Dim headerColumns As List(Of String()) = New List(Of String())

            For Each gridcolumn As DataControlField In gv.Columns

                Select Case gridcolumn.GetType().Name
                    Case "CommandField"
                        'Select
                    Case "BoundField"
                        If gridcolumn.Visible And Not String.IsNullOrWhiteSpace(DirectCast(gridcolumn, BoundField).DataField) Then
                            headerColumns.Add({DirectCast(gridcolumn, BoundField).HeaderText, DirectCast(gridcolumn, BoundField).DataField})
                        End If
                    Case "TemplateField"
                        'Estatus
                        If gridcolumn.Visible And Not String.IsNullOrWhiteSpace(DirectCast(gridcolumn, TemplateField).HeaderText) Then
                            If DirectCast(gridcolumn, TemplateField).HeaderText = "Entidad" Then
                                headerColumns.Add({DirectCast(gridcolumn, TemplateField).HeaderText, "DESC_ENTIDAD"})

                            ElseIf DirectCast(gridcolumn, TemplateField).HeaderText = "Fecha de Envío a Sanciones" Then
                                headerColumns.Add({DirectCast(gridcolumn, TemplateField).HeaderText, DirectCast(gridcolumn, TemplateField).HeaderText})
                            End If
                            'If DirectCast(gridcolumn, TemplateField).HeaderText = "Fecha de Envío a Sanciones" Then
                            '    headerColumns.Add({DirectCast(gridcolumn, TemplateField).HeaderText, "T_ID_FOLIO_SISAN"})
                            'End If
                        End If
                End Select
            Next

            If IsNothing(referencias) Then
                referencias = New List(Of String)
            End If

            'Cuando las referencias son diferentes de 3 se desconfigura el encabezado
            'Por lo que si son menores a 3 se agregan
            For index = referencias.Count To 2
                referencias.Add("")
            Next

            Dim export As New OpenXML.ExportExcel()
            export.SheetName = nombreBandeja
            export.TableName = nombreBandeja
            export.HeaderColor = WebConfigurationManager.AppSettings.Item("HeaderColorExcel")
            export.HeaderForeColor = WebConfigurationManager.AppSettings.Item("HeaderForeColorExcel")
            export.CellForeColor = WebConfigurationManager.AppSettings.Item("CellForeColorExcel")
            export.ShowGridLines = True
            export.Reference = referencias
            export.HeaderColumns = headerColumns
            export.DataSource = dt
            export.CreatePackage(nombreBandeja)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Warning, "AccesoBD, ExportaGridBandejaPC", "")
        End Try


    End Sub

    Public Sub ExportaGridBandejaOPI(ByVal dt As DataTable, ByVal gv As GridView, ByVal nombreBandeja As String, Optional ByVal referencias As List(Of String) = Nothing)

        Try
            Dim headerColumns As List(Of String()) = New List(Of String())

            For Each gridcolumn As DataControlField In gv.Columns

                Select Case gridcolumn.GetType().Name
                    Case "CommandField"
                        'Select
                    Case "BoundField"
                        If gridcolumn.HeaderText = "Días transcurridos <br /> en el paso actual" Then
                            gridcolumn.HeaderText = "Días transcurridos en el paso actual"
                        ElseIf gridcolumn.HeaderText = "Días transcurridos <br /> desde el posible incumplimiento" Then
                            gridcolumn.HeaderText = "Días transcurridos desde el posible incumplimiento"
                        End If

                        If gridcolumn.Visible And Not String.IsNullOrWhiteSpace(DirectCast(gridcolumn, BoundField).DataField) Then
                            headerColumns.Add({DirectCast(gridcolumn, BoundField).HeaderText, DirectCast(gridcolumn, BoundField).DataField})
                        End If


                    Case "TemplateField"
                        'Estatus
                        If gridcolumn.Visible And Not String.IsNullOrWhiteSpace(DirectCast(gridcolumn, TemplateField).HeaderText) Then
                            If DirectCast(gridcolumn, TemplateField).HeaderText = "Entidad" Then
                                headerColumns.Add({DirectCast(gridcolumn, TemplateField).HeaderText, "DESC_ENTIDAD"})
                            End If
                            If DirectCast(gridcolumn, TemplateField).HeaderText = "Supervisor(es)" Then
                                headerColumns.Add({DirectCast(gridcolumn, TemplateField).HeaderText, "D_SUPERVISORES"})
                            End If
                            If DirectCast(gridcolumn, TemplateField).HeaderText = "Inspector(es)" Then
                                headerColumns.Add({DirectCast(gridcolumn, TemplateField).HeaderText, "D_INSPECTORES"})
                            End If
                            If DirectCast(gridcolumn, TemplateField).HeaderText = "Fecha de Envío a Sanciones" Then
                                headerColumns.Add({DirectCast(gridcolumn, TemplateField).HeaderText, "F_FECH_ENVIA_SANCIONES"})
                            End If
                        End If
                End Select
            Next

            If IsNothing(referencias) Then
                referencias = New List(Of String)
            End If

            'Cuando las referencias son diferentes de 3 se desconfigura el encabezado
            'Por lo que si son menores a 3 se agregan
            For index = referencias.Count To 2
                referencias.Add("")
            Next

            Dim export As New OpenXML.ExportExcel()
            export.SheetName = nombreBandeja
            export.TableName = nombreBandeja
            export.HeaderColor = WebConfigurationManager.AppSettings.Item("HeaderColorExcel")
            export.HeaderForeColor = WebConfigurationManager.AppSettings.Item("HeaderForeColorExcel")
            export.CellForeColor = WebConfigurationManager.AppSettings.Item("CellForeColorExcel")
            export.ShowGridLines = True
            export.Reference = referencias
            export.HeaderColumns = headerColumns
            export.DataSource = dt
            export.CreatePackage(nombreBandeja)
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Warning, "AccesoBD, ExportaGridBandejaOPI", "")
        End Try


    End Sub
End Class

