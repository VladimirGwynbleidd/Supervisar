Public Class ProcesosVigilancia
    Dim Tabla As String = "BDS_C_GR_CAT_PROCESOS_VIGILANCIA"
    Public Property CampoEstatus As String
    Public Property Identificador As String
    Public Property Descripcion As String
    Public Property Estatus As Boolean
    Public Property InicioVigencia As Date
    Public Property FinVigencia As Date
#Region "Constructor"
    Public Sub New()

    End Sub
    Public Sub New(identificador As String)
        Me.Identificador = identificador
    End Sub
#End Region
#Region "Métodos"
    Public Function ObtenerTodos() As DataView
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataView
            dv = conexion.ConsultarDT("SELECT N_ID_CAT_PV, T_DSC_CAT_PV FROM BDS_C_GR_CAT_PROCESOS_VIGILANCIA WHERE N_FLAG_VIG = 1 ORDER BY T_DSC_CAT_PV").DefaultView
            dv.Sort = "N_ID_CAT_PV"
            Return dv
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try

    End Function
    Public Function ObtenerCatalogoVigilancia() As DataView
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataView
            dv = conexion.ConsultarDT("SELECT N_ID_CAT_PV,T_DSC_CAT_PV FROM BDS_C_GR_CAT_PROCESOS_VIGILANCIA WHERE N_FLAG_VIG = 1 AND N_ID_CAT_PV = '" & Me.Identificador & "'").DefaultView
            Return dv

        Catch ex As Exception
            Throw ex
        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try

    End Function
    Public Function ObtenerDatos(query As String) As DataView
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataView
            dv = conexion.ConsultarDT(query).DefaultView
            Return dv
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
    Public Function ObtenerEstructura() As DataView

        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataView
            dv = conexion.ConsultarDT("SELECT" & vbCrLf &
            "  clmns.name as [ColumnName]," & vbCrLf &
            "  clmns.column_id as [ColumnID]," & vbCrLf &
            "  scparam.name as [TypeSchema]," & vbCrLf &
            "  CASE" & vbCrLf &
            "    WHEN (usrt.system_type_id <> usrt.user_type_id) AND" & vbCrLf &
            "         (usrt.name = N'sysname')" & vbCrLf &
            "    THEN N'sysname'" & vbCrLf &
            "    ELSE ISNULL(baset.name, N'')" & vbCrLf &
            "  END as [Type]," & vbCrLf &
            "  CASE" & vbCrLf &
            "    WHEN (usrt.system_type_id <> usrt.user_type_id) AND" & vbCrLf &
            "         (usrt.name <> N'sysname')" & vbCrLf &
            "    THEN usrt.name" & vbCrLf &
            "    ELSE N''" & vbCrLf &
            "  END as [UserType]," & vbCrLf &
            "  CAST(CASE" & vbCrLf &
            "         WHEN (baset.name IN (N'nchar', N'nvarchar')) AND" & vbCrLf &
            "              (clmns.max_length <> -1)" & vbCrLf &
            "         THEN clmns.max_length / 2" & vbCrLf &
            "         ELSE clmns.max_length" & vbCrLf &
            "       END AS int) as [Length]," & vbCrLf &
            "  CAST(clmns.precision AS int) as [NumericPrecision]," & vbCrLf &
            "  CAST(clmns.scale AS int) as [NumericScale]," & vbCrLf &
            "  CAST(ISNULL(cik.index_column_id, 0) AS bit) as [PrimaryKey]," & vbCrLf &
            "  CAST(ISNULL(cik.key_ordinal, 0) AS int) as [PosInPKey]," & vbCrLf &
            "  CASE" & vbCrLf &
            "    WHEN cik.key_ordinal IS NULL" & vbCrLf &
            "    THEN N''" & vbCrLf &
            "    WHEN cik.is_descending_key = 0" & vbCrLf &
            "    THEN N'A'" & vbCrLf &
            "    ELSE N'D'" & vbCrLf &
            "  END as [OrderInPKey]," & vbCrLf &
            "  CAST(ISNULL((SELECT TOP 1 1" & vbCrLf &
            "               FROM sys.foreign_key_columns colfk" & vbCrLf &
            "               WHERE (colfk.parent_column_id = clmns.column_id) AND" & vbCrLf &
            "                     (colfk.parent_object_id = clmns.object_id)), 0) AS bit) as [ForeignKey]," & vbCrLf &
            "  ~clmns.is_nullable as [NotNull]," & vbCrLf &
            "  clmns.is_identity as [Identity]," & vbCrLf &
            "  CAST(ISNULL(ic.seed_value, 0) AS int) as [Seed]," & vbCrLf &
            "  CAST(ISNULL(ic.increment_value, 0) AS int) as [Increment]," & vbCrLf &
            "  clmns.is_rowguidcol as [RowGuidCol]," & vbCrLf &
            "  clmns.is_computed as [Computed]," & vbCrLf &
            "  ISNULL(cc.definition, N'') as [ComputedText]," & vbCrLf &
            "  CAST(ISNULL(cc.uses_database_collation, 0) AS bit) as [UsesDatabaseCollation]," & vbCrLf &
            "  CAST(ISNULL(cc.is_persisted, 0) AS bit) as [Persisted]," & vbCrLf &
            "  ISNULL(clmns.collation_name, N'') as [Collation]," & vbCrLf &
            "  CASE" & vbCrLf &
            "    WHEN o1.parent_object_id = 0" & vbCrLf &
            "    THEN SCHEMA_NAME(o1.schema_id)" & vbCrLf &
            "    ELSE N''" & vbCrLf &
            "  END as [DefaultOwner]," & vbCrLf &
            "  CASE" & vbCrLf &
            "    WHEN o1.parent_object_id = 0" & vbCrLf &
            "    THEN o1.name" & vbCrLf &
            "    ELSE N''" & vbCrLf &
            "  END as [DefaultName]," & vbCrLf &
            "  CASE" & vbCrLf &
            "    WHEN (o1.parent_object_id IS NULL) OR (o1.parent_object_id = 0)" & vbCrLf &
            "    THEN N''" & vbCrLf &
            "    ELSE (SELECT defs.definition" & vbCrLf &
            "          FROM sys.default_constraints defs" & vbCrLf &
            "          WHERE defs.object_id = o1.object_id)" & vbCrLf &
            "  END as [DefaultCnstr]," & vbCrLf &
            "  CASE" & vbCrLf &
            "    WHEN (clmns.rule_object_id = 0)" & vbCrLf &
            "    THEN N''" & vbCrLf &
            "    ELSE SCHEMA_NAME(o2.schema_id)" & vbCrLf &
            "  END as [RuleOwner]," & vbCrLf &
            "  CASE" & vbCrLf &
            "    WHEN (clmns.rule_object_id = 0)" & vbCrLf &
            "    THEN N''" & vbCrLf &
            "    ELSE o2.name" & vbCrLf &
            "  END as [RuleName]," & vbCrLf &
            "  ISNULL(ch.name, N'') as [CheckCnstrName]," & vbCrLf &
            "  ISNULL(ch.definition, N'') as [CheckCnstr]," & vbCrLf &
            "  ISNULL(CAST(xp.value AS nvarchar(4000)), N'') as [Description]," & vbCrLf &
            "  ISNULL(s2clmns.name, N'') as [XmlSchemaNamespaceSchema]," & vbCrLf &
            "  ISNULL(xscclmns.name, N'') as [XmlSchemaNamespace]," & vbCrLf &
            "  ISNULL((CASE clmns.is_xml_document" & vbCrLf &
            "            WHEN 1" & vbCrLf &
            "            THEN 2" & vbCrLf &
            "            ELSE 1" & vbCrLf &
            "          END), 0) as [XmlDocumentConstraint]," & vbCrLf &
            "  CAST(clmns.is_sparse AS bit) as [Sparse]," & vbCrLf &
            "  CAST(clmns.is_column_set AS bit) as [ColumnSet]" & vbCrLf &
            "FROM" & vbCrLf &
            "  sys.all_objects o INNER JOIN sys.schemas sc ON sc.schema_id = o.schema_id" & vbCrLf &
            "                    INNER JOIN sys.all_columns clmns ON clmns.object_id = o.object_id" & vbCrLf &
            "                    LEFT OUTER JOIN sys.identity_columns ic ON (ic.object_id = o.object_id) AND" & vbCrLf &
            "                                                               (ic.column_id = clmns.column_id)" & vbCrLf &
            "                    LEFT OUTER JOIN sys.computed_columns cc ON (cc.object_id = o.object_id) AND" & vbCrLf &
            "                                                               (cc.column_id = clmns.column_id)" & vbCrLf &
            "                    LEFT OUTER JOIN sys.all_objects o1 ON o1.object_id = clmns.default_object_id" & vbCrLf &
            "                    LEFT OUTER JOIN sys.all_objects o2 ON o2.object_id = clmns.rule_object_id" & vbCrLf &
            "                    LEFT OUTER JOIN sys.check_constraints ch ON (ch.parent_object_id = clmns.object_id) AND" & vbCrLf &
            "                                                                (ch.parent_column_id = clmns.column_id)" & vbCrLf &
            "                    LEFT OUTER JOIN sys.indexes ik ON (ik.object_id = clmns.object_id) AND" & vbCrLf &
            "                                                      (ik.is_primary_key = 1)" & vbCrLf &
            "                    LEFT OUTER JOIN sys.index_columns cik ON (cik.index_id = ik.index_id) AND" & vbCrLf &
            "                                                             (cik.column_id = clmns.column_id) AND" & vbCrLf &
            "                                                             (cik.object_id = clmns.object_id) AND" & vbCrLf &
            "                                                             (cik.is_included_column = 0)" & vbCrLf &
            "                    LEFT OUTER JOIN sys.types usrt ON usrt.user_type_id = clmns.user_type_id" & vbCrLf &
            "                    LEFT OUTER JOIN sys.schemas scparam ON scparam.schema_id = usrt.schema_id" & vbCrLf &
            "                    LEFT OUTER JOIN sys.types baset ON (baset.user_type_id = clmns.system_type_id) AND" & vbCrLf &
            "                                                       (baset.user_type_id = baset.system_type_id) OR" & vbCrLf &
            "                                                       (baset.system_type_id = clmns.system_type_id) AND" & vbCrLf &
            "                                                       (baset.user_type_id = clmns.user_type_id) AND" & vbCrLf &
            "                                                       (baset.is_user_defined = 0) AND" & vbCrLf &
            "                                                       (baset.is_assembly_type = 1)" & vbCrLf &
            "                    LEFT OUTER JOIN fn_listextendedproperty(N'MS_Description', N'schema', 'dbo', N'table', '" & Me.Identificador & "', N'column', default) xp ON ((xp.objname COLLATE database_default) = clmns.name)" & vbCrLf &
            "                    LEFT OUTER JOIN sys.xml_schema_collections xscclmns ON xscclmns.xml_collection_id = clmns.xml_collection_id" & vbCrLf &
            "                    LEFT OUTER JOIN sys.schemas s2clmns ON s2clmns.schema_id = xscclmns.schema_id" & vbCrLf &
            "WHERE" & vbCrLf &
            "  (o.name = '" & Me.Identificador & "') AND" & vbCrLf &
            "  (sc.name = 'dbo')" & vbCrLf &
            "ORDER BY" & vbCrLf &
            "  clmns.column_id ASC;").DefaultView
            Return dv



        Catch ex As Exception
            Throw ex
        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try


    End Function
    Public Function ObtenerTablaForanea(ByVal TableName As String, ByVal ColumnName As String)
        Dim conexion As New Conexion.SQLServer
        Dim Dr As IDataReader = Nothing
        Try
            Dim dv As New DataView
            Dim campo As String
            Dim tablaForanea As String = ""
            Dim campoTablaForanea As String = ""



            Dr = conexion.ConsultarDR("SELECT" & vbCrLf &
                            "  fo.name as [FKName]," & vbCrLf &
                            "  ~fo.is_disabled as [FKEnabled]," & vbCrLf &
                            "  CAST(db_name() AS sysname) as [PKTableQualifier]," & vbCrLf &
                            "  rs.name as [PKTableSchema]," & vbCrLf &
                            "  ro.name as [PKTableName]," & vbCrLf &
                            "  i.name as [PKName]," & vbCrLf &
                            "  rc.name as [PKTableColumnName]," & vbCrLf &
                            "  fkc.constraint_column_id as [KeySequence]," & vbCrLf &
                            "  CAST(db_name() AS sysname) as [FKTableQualifier]," & vbCrLf &
                            "  ps.name as [FKTableSchema]," & vbCrLf &
                            "  po.name as [FKTableName]," & vbCrLf &
                            "  pc.name as [FKTableColumnName]," & vbCrLf &
                            "  ~fo.is_not_for_replication as [FKForReplication]," & vbCrLf &
                            "  ~fo.is_not_trusted as [TrustedFK]," & vbCrLf &
                            "  CAST(fo.update_referential_action AS int) as [UpdateRule]," & vbCrLf &
                            "  CAST(fo.delete_referential_action AS int) as [DeleteRule]" & vbCrLf &
                            "FROM" & vbCrLf &
                            "  sys.foreign_key_columns fkc INNER JOIN sys.foreign_keys fo ON fkc.constraint_object_id = fo.object_id" & vbCrLf &
                            "                              INNER JOIN sys.indexes i ON (fo.referenced_object_id = i.object_id) AND" & vbCrLf &
                            "                                                          (fo.key_index_id = i.index_id)" & vbCrLf &
                            "                              INNER JOIN sys.objects ro ON fkc.referenced_object_id = ro.object_id" & vbCrLf &
                            "                              INNER JOIN sys.schemas rs ON rs.schema_id = ro.schema_id" & vbCrLf &
                            "                              INNER JOIN sys.columns rc ON (rc.object_id = fkc.referenced_object_id) AND" & vbCrLf &
                            "                                                           (fkc.referenced_column_id = rc.column_id)" & vbCrLf &
                            "                              INNER JOIN sys.objects po ON fkc.parent_object_id = po.object_id" & vbCrLf &
                            "                              INNER JOIN sys.schemas ps ON ps.schema_id = po.schema_id" & vbCrLf &
                            "                              INNER JOIN sys.columns pc ON (pc.object_id = fkc.parent_object_id) AND" & vbCrLf &
                            "                                                           (fkc.parent_column_id = pc.column_id)" & vbCrLf &
                            "WHERE" & vbCrLf &
                            "  (ps.name = 'dbo') AND" & vbCrLf &
                            "  (po.name = '" & TableName & "') AND" & vbCrLf &
                            "  (pc.name = '" & ColumnName & "')" & vbCrLf &
                            "ORDER BY" & vbCrLf &
                            "  [PKTableSchema], [PKTableName], fkc.referenced_column_id")

            While (Dr.Read())
                campo = Convert.ToString(Dr.Item("FKTableColumnName"))
                tablaForanea = Convert.ToString(Dr.Item("PKTableName"))
                campoTablaForanea = Convert.ToString(Dr.Item("PKTableColumnName"))
            End While

            Dr.Close()


            Return tablaForanea

        Catch ex As Exception
            Throw ex
        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If

        End Try

    End Function
    Public Function ObtenerColumnaForanea(ByVal ColumnName As String) As String

        Return ObtenerColumnaForanea(Identificador, ColumnName)

    End Function
    Public Function ObtenerColumnaForanea(ByVal TableName As String, ByVal ColumnName As String) As String

        Dim Con As New Conexion.SQLServer
        Dim Dr As IDataReader = Nothing
        Dim dv As New DataView
        Dim campo As String
        Dim tablaForanea As String = String.Empty
        Dim campoTablaForanea As String = String.Empty
        Dim columnaDescripcion As String = String.Empty

        Try

            Dr = Con.ConsultarDR("SELECT" & vbCrLf &
                            "  fo.name as [FKName]," & vbCrLf &
                            "  ~fo.is_disabled as [FKEnabled]," & vbCrLf &
                            "  CAST(db_name() AS sysname) as [PKTableQualifier]," & vbCrLf &
                            "  rs.name as [PKTableSchema]," & vbCrLf &
                            "  ro.name as [PKTableName]," & vbCrLf &
                            "  i.name as [PKName]," & vbCrLf &
                            "  rc.name as [PKTableColumnName]," & vbCrLf &
                            "  fkc.constraint_column_id as [KeySequence]," & vbCrLf &
                            "  CAST(db_name() AS sysname) as [FKTableQualifier]," & vbCrLf &
                            "  ps.name as [FKTableSchema]," & vbCrLf &
                            "  po.name as [FKTableName]," & vbCrLf &
                            "  pc.name as [FKTableColumnName]," & vbCrLf &
                            "  ~fo.is_not_for_replication as [FKForReplication]," & vbCrLf &
                            "  ~fo.is_not_trusted as [TrustedFK]," & vbCrLf &
                            "  CAST(fo.update_referential_action AS int) as [UpdateRule]," & vbCrLf &
                            "  CAST(fo.delete_referential_action AS int) as [DeleteRule]" & vbCrLf &
                            "FROM" & vbCrLf &
                            "  sys.foreign_key_columns fkc INNER JOIN sys.foreign_keys fo ON fkc.constraint_object_id = fo.object_id" & vbCrLf &
                            "                              INNER JOIN sys.indexes i ON (fo.referenced_object_id = i.object_id) AND" & vbCrLf &
                            "                                                          (fo.key_index_id = i.index_id)" & vbCrLf &
                            "                              INNER JOIN sys.objects ro ON fkc.referenced_object_id = ro.object_id" & vbCrLf &
                            "                              INNER JOIN sys.schemas rs ON rs.schema_id = ro.schema_id" & vbCrLf &
                            "                              INNER JOIN sys.columns rc ON (rc.object_id = fkc.referenced_object_id) AND" & vbCrLf &
                            "                                                           (fkc.referenced_column_id = rc.column_id)" & vbCrLf &
                            "                              INNER JOIN sys.objects po ON fkc.parent_object_id = po.object_id" & vbCrLf &
                            "                              INNER JOIN sys.schemas ps ON ps.schema_id = po.schema_id" & vbCrLf &
                            "                              INNER JOIN sys.columns pc ON (pc.object_id = fkc.parent_object_id) AND" & vbCrLf &
                            "                                                           (fkc.parent_column_id = pc.column_id)" & vbCrLf &
                            "WHERE" & vbCrLf &
                            "  (ps.name = 'dbo') AND" & vbCrLf &
                            "  (po.name = '" & TableName & "') AND" & vbCrLf &
                            "  (pc.name = '" & ColumnName & "')" & vbCrLf &
                            "ORDER BY" & vbCrLf &
                            "  [PKTableSchema], [PKTableName], fkc.referenced_column_id")
            While (Dr.Read())
                campo = Convert.ToString(Dr.Item("FKTableColumnName"))
                tablaForanea = Convert.ToString(Dr.Item("PKTableName"))
                campoTablaForanea = Convert.ToString(Dr.Item("PKTableColumnName"))
            End While

            Dr.Close()

            '''''''''''''''''''''''''''
            '''''''''''''''''''''''''''
            '''''''''''''''''''''''''''

            columnaDescripcion = campoTablaForanea

            Dr = Con.ConsultarDR("select clmns.name" & vbCrLf &
                        "  from sys.all_objects o, sys.schemas sc, sys.all_columns clmns" & vbCrLf &
                        "  where o.[schema_id] = sc.[schema_id]" & vbCrLf &
                        "    and clmns.[object_id] = o.[object_id]" & vbCrLf &
                        "    and o.name = '" & tablaForanea & "'" & vbCrLf &
                        "    and sc.name = 'dbo'" & vbCrLf &
                        "    and clmns.name in ('T_DSC_DESCRIPCION'," & vbCrLf &
                        "'DESCRIPCION_" & campoTablaForanea.Substring(5) & "'," & vbCrLf &
                        "'T_DSC_" & campoTablaForanea.Substring(5) & "')"
                        )
            If Not Dr Is Nothing Then
                If Dr.Read() Then
                    If tablaForanea.Equals("BDS_C_GR_PROCESO") Or tablaForanea.Equals("BDS_C_GR_SUBPROCESO") Then
                        columnaDescripcion = Convert.ToString(Dr.Item("name")) & "_" & tablaForanea.Substring(9)
                    Else
                        columnaDescripcion = Convert.ToString(Dr.Item("name"))
                    End If

                End If
            End If

            Dr.Close()

            Return columnaDescripcion


        Catch ex As Exception
            catch_cone(ex, "RellenaControles")
        Finally
            If Not Con Is Nothing Then
                Con.CerrarConexion()
                Con = Nothing
            End If
            If Not Dr Is Nothing Then
                If Not Dr.IsClosed Then
                    Dr.Close()
                End If
                Dr = Nothing
            End If

        End Try
        Return columnaDescripcion
    End Function
    Private Sub catch_cone(ByVal ex As Exception, ByVal p2 As String)
        Throw New NotImplementedException
    End Sub
    Public Function BusquedaReg(ByVal Query As String) As Boolean
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataView
            dv = conexion.ConsultarDT("SELECT 1 FROM " & Identificador.ToString() & " WHERE " & Query).DefaultView

            If dv.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
    Public Function Agregar(ByVal Campos As String, ByVal Valores As String) As Boolean
        Dim conexion As New Conexion.SQLServer
        Try
            Return conexion.Ejecutar("INSERT INTO " & Me.Identificador.ToString() & "(" & Campos & ") VALUES(" & Valores & ")")
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
    Public Function Actualizar(ByVal campos As List(Of String), ByVal valores As List(Of Object), ByVal camposCondicion As List(Of String), ByVal valoresCondicion As List(Of Object))
        Dim conexion As New Conexion.SQLServer
        Try
            Return conexion.Actualizar(Me.Identificador, campos, valores, camposCondicion, valoresCondicion)
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
    Public Function ObtenerIdSiguiente(columna As String) As Integer
        Dim resultado As Integer = 1
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dr As SqlClient.SqlDataReader = conexion.ConsultarDR("SELECT (MAX(" & columna & ") + 1) " & columna & " FROM " & Identificador)
            If dr.Read() Then
                If IsDBNull(dr(columna)) Then
                    resultado = 1
                Else
                    resultado = CInt(dr(columna))
                End If
            End If
            Return resultado
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
    End Function
    Public Function Baja(camposCondicion As List(Of String), valoresCondicion As List(Of Object), mensajeBitacora As String) As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer
        Try
            Dim bitacora As New Conexion.Bitacora(mensajeBitacora, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            listCampos.Add(CampoEstatus) : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)
            Select Case Identificador
                Case "BDS_C_GR_PROCESO"
                    resultado = conexion.Actualizar("BDS_C_GR_SUBPROCESO", listCampos, listValores, camposCondicion, valoresCondicion)
                    resultado = conexion.Actualizar("BDS_C_GR_SUPERVISOR", listCampos, listValores, camposCondicion, valoresCondicion)
                    resultado = conexion.Actualizar("BDS_C_GR_INSPECTOR", listCampos, listValores, camposCondicion, valoresCondicion)
                Case "BDS_C_GR_SUBPROCESO"
                    resultado = conexion.Actualizar("BDS_C_GR_SUPERVISOR", listCampos, listValores, camposCondicion, valoresCondicion)
                    resultado = conexion.Actualizar("BDS_C_GR_INSPECTOR", listCampos, listValores, camposCondicion, valoresCondicion)
                Case Else
            End Select

            resultado = conexion.Actualizar(Identificador, listCampos, listValores, camposCondicion, valoresCondicion)
            bitacora.Actualizar(Identificador, listCampos, listValores, camposCondicion, valoresCondicion, resultado, "Error al " + mensajeBitacora)

            bitacora.Finalizar(resultado)
        Catch ex As Exception
            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return resultado
    End Function
    Public Function Ejecutar(query As String, mensajeBitacora As String) As Boolean
        Dim resultado As Boolean = False
        Dim conexion As New Conexion.SQLServer
        Try
            Dim bitacora As New Conexion.Bitacora(mensajeBitacora, System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)

            resultado = conexion.Ejecutar(query)
            bitacora.Actualizar(Identificador, "", "", resultado, mensajeBitacora)
            bitacora.Finalizar(resultado)
        Catch ex As Exception
            resultado = False
            Utilerias.ControlErrores.EscribirEvento(ex.ToString, EventLogEntryType.Error)
        Finally
            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try
        Return resultado
    End Function
#End Region
End Class
