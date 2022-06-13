Public Class Catalogo
#Region "Constructores"
    Public Sub New()

    End Sub

    Public Sub New(ByVal N_ID_CAT_ADMBLE As String)
        Me.Identificador = N_ID_CAT_ADMBLE
        ObtenerCatalogo()
    End Sub
#End Region
#Region "Propiedades"
    Public Property CampoEstatus As String

    Public Property Identificador As String
    Public Property Descripcion As String
    Public Property Estatus As Boolean
    Public Property InicioVigencia As Date
    Public Property FinVigencia As Date
    Public Property Existe As Boolean = False

#End Region
    Public Function ObtenerCatalogo() As DataView
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataView
            dv = conexion.ConsultarDT("SELECT N_ID_CAT_ADMBLE,T_DSC_CAT_ADMBLE FROM BDS_C_GR_CAT_ADMBLE WHERE N_FLAG_VIG = 1 AND N_ID_CAT_ADMBLE = '" & Me.Identificador & "'").DefaultView
            Return dv

        Catch ex As Exception
            Throw ex
        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try

    End Function
    Public Function ObtenerDatos(ByVal query As String) As DataView
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
            dv = conexion.ConsultarDT("SELECT" & vbCrLf & _
            "  clmns.name as [ColumnName]," & vbCrLf & _
            "  clmns.column_id as [ColumnID]," & vbCrLf & _
            "  scparam.name as [TypeSchema]," & vbCrLf & _
            "  CASE" & vbCrLf & _
            "    WHEN (usrt.system_type_id <> usrt.user_type_id) AND" & vbCrLf & _
            "         (usrt.name = N'sysname')" & vbCrLf & _
            "    THEN N'sysname'" & vbCrLf & _
            "    ELSE ISNULL(baset.name, N'')" & vbCrLf & _
            "  END as [Type]," & vbCrLf & _
            "  CASE" & vbCrLf & _
            "    WHEN (usrt.system_type_id <> usrt.user_type_id) AND" & vbCrLf & _
            "         (usrt.name <> N'sysname')" & vbCrLf & _
            "    THEN usrt.name" & vbCrLf & _
            "    ELSE N''" & vbCrLf & _
            "  END as [UserType]," & vbCrLf & _
            "  CAST(CASE" & vbCrLf & _
            "         WHEN (baset.name IN (N'nchar', N'nvarchar')) AND" & vbCrLf & _
            "              (clmns.max_length <> -1)" & vbCrLf & _
            "         THEN clmns.max_length / 2" & vbCrLf & _
            "         ELSE clmns.max_length" & vbCrLf & _
            "       END AS int) as [Length]," & vbCrLf & _
            "  CAST(clmns.precision AS int) as [NumericPrecision]," & vbCrLf & _
            "  CAST(clmns.scale AS int) as [NumericScale]," & vbCrLf & _
            "  CAST(ISNULL(cik.index_column_id, 0) AS bit) as [PrimaryKey]," & vbCrLf & _
            "  CAST(ISNULL(cik.key_ordinal, 0) AS int) as [PosInPKey]," & vbCrLf & _
            "  CASE" & vbCrLf & _
            "    WHEN cik.key_ordinal IS NULL" & vbCrLf & _
            "    THEN N''" & vbCrLf & _
            "    WHEN cik.is_descending_key = 0" & vbCrLf & _
            "    THEN N'A'" & vbCrLf & _
            "    ELSE N'D'" & vbCrLf & _
            "  END as [OrderInPKey]," & vbCrLf & _
            "  CAST(ISNULL((SELECT TOP 1 1" & vbCrLf & _
            "               FROM sys.foreign_key_columns colfk" & vbCrLf & _
            "               WHERE (colfk.parent_column_id = clmns.column_id) AND" & vbCrLf & _
            "                     (colfk.parent_object_id = clmns.object_id)), 0) AS bit) as [ForeignKey]," & vbCrLf & _
            "  ~clmns.is_nullable as [NotNull]," & vbCrLf & _
            "  clmns.is_identity as [Identity]," & vbCrLf & _
            "  CAST(ISNULL(ic.seed_value, 0) AS int) as [Seed]," & vbCrLf & _
            "  CAST(ISNULL(ic.increment_value, 0) AS int) as [Increment]," & vbCrLf & _
            "  clmns.is_rowguidcol as [RowGuidCol]," & vbCrLf & _
            "  clmns.is_computed as [Computed]," & vbCrLf & _
            "  ISNULL(cc.definition, N'') as [ComputedText]," & vbCrLf & _
            "  CAST(ISNULL(cc.uses_database_collation, 0) AS bit) as [UsesDatabaseCollation]," & vbCrLf & _
            "  CAST(ISNULL(cc.is_persisted, 0) AS bit) as [Persisted]," & vbCrLf & _
            "  ISNULL(clmns.collation_name, N'') as [Collation]," & vbCrLf & _
            "  CASE" & vbCrLf & _
            "    WHEN o1.parent_object_id = 0" & vbCrLf & _
            "    THEN SCHEMA_NAME(o1.schema_id)" & vbCrLf & _
            "    ELSE N''" & vbCrLf & _
            "  END as [DefaultOwner]," & vbCrLf & _
            "  CASE" & vbCrLf & _
            "    WHEN o1.parent_object_id = 0" & vbCrLf & _
            "    THEN o1.name" & vbCrLf & _
            "    ELSE N''" & vbCrLf & _
            "  END as [DefaultName]," & vbCrLf & _
            "  CASE" & vbCrLf & _
            "    WHEN (o1.parent_object_id IS NULL) OR (o1.parent_object_id = 0)" & vbCrLf & _
            "    THEN N''" & vbCrLf & _
            "    ELSE (SELECT defs.definition" & vbCrLf & _
            "          FROM sys.default_constraints defs" & vbCrLf & _
            "          WHERE defs.object_id = o1.object_id)" & vbCrLf & _
            "  END as [DefaultCnstr]," & vbCrLf & _
            "  CASE" & vbCrLf & _
            "    WHEN (clmns.rule_object_id = 0)" & vbCrLf & _
            "    THEN N''" & vbCrLf & _
            "    ELSE SCHEMA_NAME(o2.schema_id)" & vbCrLf & _
            "  END as [RuleOwner]," & vbCrLf & _
            "  CASE" & vbCrLf & _
            "    WHEN (clmns.rule_object_id = 0)" & vbCrLf & _
            "    THEN N''" & vbCrLf & _
            "    ELSE o2.name" & vbCrLf & _
            "  END as [RuleName]," & vbCrLf & _
            "  ISNULL(ch.name, N'') as [CheckCnstrName]," & vbCrLf & _
            "  ISNULL(ch.definition, N'') as [CheckCnstr]," & vbCrLf & _
            "  ISNULL(CAST(xp.value AS nvarchar(4000)), N'') as [Description]," & vbCrLf & _
            "  ISNULL(s2clmns.name, N'') as [XmlSchemaNamespaceSchema]," & vbCrLf & _
            "  ISNULL(xscclmns.name, N'') as [XmlSchemaNamespace]," & vbCrLf & _
            "  ISNULL((CASE clmns.is_xml_document" & vbCrLf & _
            "            WHEN 1" & vbCrLf & _
            "            THEN 2" & vbCrLf & _
            "            ELSE 1" & vbCrLf & _
            "          END), 0) as [XmlDocumentConstraint]," & vbCrLf & _
            "  CAST(clmns.is_sparse AS bit) as [Sparse]," & vbCrLf & _
            "  CAST(clmns.is_column_set AS bit) as [ColumnSet]" & vbCrLf & _
            "FROM" & vbCrLf & _
            "  sys.all_objects o INNER JOIN sys.schemas sc ON sc.schema_id = o.schema_id" & vbCrLf & _
            "                    INNER JOIN sys.all_columns clmns ON clmns.object_id = o.object_id" & vbCrLf & _
            "                    LEFT OUTER JOIN sys.identity_columns ic ON (ic.object_id = o.object_id) AND" & vbCrLf & _
            "                                                               (ic.column_id = clmns.column_id)" & vbCrLf & _
            "                    LEFT OUTER JOIN sys.computed_columns cc ON (cc.object_id = o.object_id) AND" & vbCrLf & _
            "                                                               (cc.column_id = clmns.column_id)" & vbCrLf & _
            "                    LEFT OUTER JOIN sys.all_objects o1 ON o1.object_id = clmns.default_object_id" & vbCrLf & _
            "                    LEFT OUTER JOIN sys.all_objects o2 ON o2.object_id = clmns.rule_object_id" & vbCrLf & _
            "                    LEFT OUTER JOIN sys.check_constraints ch ON (ch.parent_object_id = clmns.object_id) AND" & vbCrLf & _
            "                                                                (ch.parent_column_id = clmns.column_id)" & vbCrLf & _
            "                    LEFT OUTER JOIN sys.indexes ik ON (ik.object_id = clmns.object_id) AND" & vbCrLf & _
            "                                                      (ik.is_primary_key = 1)" & vbCrLf & _
            "                    LEFT OUTER JOIN sys.index_columns cik ON (cik.index_id = ik.index_id) AND" & vbCrLf & _
            "                                                             (cik.column_id = clmns.column_id) AND" & vbCrLf & _
            "                                                             (cik.object_id = clmns.object_id) AND" & vbCrLf & _
            "                                                             (cik.is_included_column = 0)" & vbCrLf & _
            "                    LEFT OUTER JOIN sys.types usrt ON usrt.user_type_id = clmns.user_type_id" & vbCrLf & _
            "                    LEFT OUTER JOIN sys.schemas scparam ON scparam.schema_id = usrt.schema_id" & vbCrLf & _
            "                    LEFT OUTER JOIN sys.types baset ON (baset.user_type_id = clmns.system_type_id) AND" & vbCrLf & _
            "                                                       (baset.user_type_id = baset.system_type_id) OR" & vbCrLf & _
            "                                                       (baset.system_type_id = clmns.system_type_id) AND" & vbCrLf & _
            "                                                       (baset.user_type_id = clmns.user_type_id) AND" & vbCrLf & _
            "                                                       (baset.is_user_defined = 0) AND" & vbCrLf & _
            "                                                       (baset.is_assembly_type = 1)" & vbCrLf & _
            "                    LEFT OUTER JOIN fn_listextendedproperty(N'MS_Description', N'schema', 'dbo', N'table', '" & Me.Identificador & "', N'column', default) xp ON ((xp.objname COLLATE database_default) = clmns.name)" & vbCrLf & _
            "                    LEFT OUTER JOIN sys.xml_schema_collections xscclmns ON xscclmns.xml_collection_id = clmns.xml_collection_id" & vbCrLf & _
            "                    LEFT OUTER JOIN sys.schemas s2clmns ON s2clmns.schema_id = xscclmns.schema_id" & vbCrLf & _
            "WHERE" & vbCrLf & _
            "  (o.name = '" & Me.Identificador & "') AND" & vbCrLf & _
            "  (sc.name = 'dbo')" & vbCrLf & _
            "ORDER BY" & vbCrLf & _
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

    Public Function ObtenerColumFora(ByVal TableName As String, ByVal ColumnName As String)
        Dim conexion As New Conexion.SQLServer
        Try
            Dim dv As New DataView
            dv = conexion.ConsultarDT("SELECT" & vbCrLf & _
                            "  fo.name as [FKName]," & vbCrLf & _
                            "  ~fo.is_disabled as [FKEnabled]," & vbCrLf & _
                            "  CAST(db_name() AS sysname) as [PKTableQualifier]," & vbCrLf & _
                            "  rs.name as [PKTableSchema]," & vbCrLf & _
                            "  ro.name as [PKTableName]," & vbCrLf & _
                            "  i.name as [PKName]," & vbCrLf & _
                            "  rc.name as [PKTableColumnName]," & vbCrLf & _
                            "  fkc.constraint_column_id as [KeySequence]," & vbCrLf & _
                            "  CAST(db_name() AS sysname) as [FKTableQualifier]," & vbCrLf & _
                            "  ps.name as [FKTableSchema]," & vbCrLf & _
                            "  po.name as [FKTableName]," & vbCrLf & _
                            "  pc.name as [FKTableColumnName]," & vbCrLf & _
                            "  ~fo.is_not_for_replication as [FKForReplication]," & vbCrLf & _
                            "  ~fo.is_not_trusted as [TrustedFK]," & vbCrLf & _
                            "  CAST(fo.update_referential_action AS int) as [UpdateRule]," & vbCrLf & _
                            "  CAST(fo.delete_referential_action AS int) as [DeleteRule]" & vbCrLf & _
                            "FROM" & vbCrLf & _
                            "  sys.foreign_key_columns fkc INNER JOIN sys.foreign_keys fo ON fkc.constraint_object_id = fo.object_id" & vbCrLf & _
                            "                              INNER JOIN sys.indexes i ON (fo.referenced_object_id = i.object_id) AND" & vbCrLf & _
                            "                                                          (fo.key_index_id = i.index_id)" & vbCrLf & _
                            "                              INNER JOIN sys.objects ro ON fkc.referenced_object_id = ro.object_id" & vbCrLf & _
                            "                              INNER JOIN sys.schemas rs ON rs.schema_id = ro.schema_id" & vbCrLf & _
                            "                              INNER JOIN sys.columns rc ON (rc.object_id = fkc.referenced_object_id) AND" & vbCrLf & _
                            "                                                           (fkc.referenced_column_id = rc.column_id)" & vbCrLf & _
                            "                              INNER JOIN sys.objects po ON fkc.parent_object_id = po.object_id" & vbCrLf & _
                            "                              INNER JOIN sys.schemas ps ON ps.schema_id = po.schema_id" & vbCrLf & _
                            "                              INNER JOIN sys.columns pc ON (pc.object_id = fkc.parent_object_id) AND" & vbCrLf & _
                            "                                                           (fkc.parent_column_id = pc.column_id)" & vbCrLf & _
                            "WHERE" & vbCrLf & _
                            "  (ps.name = 'dbo') AND" & vbCrLf & _
                            "  (po.name = '" & TableName & "') AND" & vbCrLf & _
                            "  (pc.name = '" & ColumnName & "')" & vbCrLf & _
                            "ORDER BY" & vbCrLf & _
                            "  [PKTableSchema], [PKTableName], fkc.referenced_column_id").DefaultView



            Return dv

        Catch ex As Exception
            Throw ex
        Finally

            If Not IsNothing(conexion) Then
                conexion.CerrarConexion()
            End If
        End Try



    End Function

    Public Function consulta(ByVal TablaForanea As String)
        Dim conexion As New Conexion.SQLServer
        Try

            Dim dv As New DataView
            dv = conexion.ConsultarDT("select clmns.name" & vbCrLf & _
                        "  from sys.all_objects o, sys.schemas sc, sys.all_columns clmns" & vbCrLf & _
                        "  where o.[schema_id] = sc.[schema_id]" & vbCrLf & _
                        "    and clmns.[object_id] = o.[object_id]" & vbCrLf & _
                        "    and o.name = '" & TablaForanea & "'" & vbCrLf & _
                        "    and sc.name = 'dbo'" & vbCrLf & _
                        "    and clmns.name in ('DSC_" & TablaForanea.Substring(3) & "'," & vbCrLf & _
                        "'DESCRIPCION_" & TablaForanea & "')").DefaultView
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



            Dr = conexion.ConsultarDR("SELECT" & vbCrLf & _
                            "  fo.name as [FKName]," & vbCrLf & _
                            "  ~fo.is_disabled as [FKEnabled]," & vbCrLf & _
                            "  CAST(db_name() AS sysname) as [PKTableQualifier]," & vbCrLf & _
                            "  rs.name as [PKTableSchema]," & vbCrLf & _
                            "  ro.name as [PKTableName]," & vbCrLf & _
                            "  i.name as [PKName]," & vbCrLf & _
                            "  rc.name as [PKTableColumnName]," & vbCrLf & _
                            "  fkc.constraint_column_id as [KeySequence]," & vbCrLf & _
                            "  CAST(db_name() AS sysname) as [FKTableQualifier]," & vbCrLf & _
                            "  ps.name as [FKTableSchema]," & vbCrLf & _
                            "  po.name as [FKTableName]," & vbCrLf & _
                            "  pc.name as [FKTableColumnName]," & vbCrLf & _
                            "  ~fo.is_not_for_replication as [FKForReplication]," & vbCrLf & _
                            "  ~fo.is_not_trusted as [TrustedFK]," & vbCrLf & _
                            "  CAST(fo.update_referential_action AS int) as [UpdateRule]," & vbCrLf & _
                            "  CAST(fo.delete_referential_action AS int) as [DeleteRule]" & vbCrLf & _
                            "FROM" & vbCrLf & _
                            "  sys.foreign_key_columns fkc INNER JOIN sys.foreign_keys fo ON fkc.constraint_object_id = fo.object_id" & vbCrLf & _
                            "                              INNER JOIN sys.indexes i ON (fo.referenced_object_id = i.object_id) AND" & vbCrLf & _
                            "                                                          (fo.key_index_id = i.index_id)" & vbCrLf & _
                            "                              INNER JOIN sys.objects ro ON fkc.referenced_object_id = ro.object_id" & vbCrLf & _
                            "                              INNER JOIN sys.schemas rs ON rs.schema_id = ro.schema_id" & vbCrLf & _
                            "                              INNER JOIN sys.columns rc ON (rc.object_id = fkc.referenced_object_id) AND" & vbCrLf & _
                            "                                                           (fkc.referenced_column_id = rc.column_id)" & vbCrLf & _
                            "                              INNER JOIN sys.objects po ON fkc.parent_object_id = po.object_id" & vbCrLf & _
                            "                              INNER JOIN sys.schemas ps ON ps.schema_id = po.schema_id" & vbCrLf & _
                            "                              INNER JOIN sys.columns pc ON (pc.object_id = fkc.parent_object_id) AND" & vbCrLf & _
                            "                                                           (fkc.parent_column_id = pc.column_id)" & vbCrLf & _
                            "WHERE" & vbCrLf & _
                            "  (ps.name = 'dbo') AND" & vbCrLf & _
                            "  (po.name = '" & TableName & "') AND" & vbCrLf & _
                            "  (pc.name = '" & ColumnName & "')" & vbCrLf & _
                            "ORDER BY" & vbCrLf & _
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

            Dr = Con.ConsultarDR("SELECT" & vbCrLf & _
                            "  fo.name as [FKName]," & vbCrLf & _
                            "  ~fo.is_disabled as [FKEnabled]," & vbCrLf & _
                            "  CAST(db_name() AS sysname) as [PKTableQualifier]," & vbCrLf & _
                            "  rs.name as [PKTableSchema]," & vbCrLf & _
                            "  ro.name as [PKTableName]," & vbCrLf & _
                            "  i.name as [PKName]," & vbCrLf & _
                            "  rc.name as [PKTableColumnName]," & vbCrLf & _
                            "  fkc.constraint_column_id as [KeySequence]," & vbCrLf & _
                            "  CAST(db_name() AS sysname) as [FKTableQualifier]," & vbCrLf & _
                            "  ps.name as [FKTableSchema]," & vbCrLf & _
                            "  po.name as [FKTableName]," & vbCrLf & _
                            "  pc.name as [FKTableColumnName]," & vbCrLf & _
                            "  ~fo.is_not_for_replication as [FKForReplication]," & vbCrLf & _
                            "  ~fo.is_not_trusted as [TrustedFK]," & vbCrLf & _
                            "  CAST(fo.update_referential_action AS int) as [UpdateRule]," & vbCrLf & _
                            "  CAST(fo.delete_referential_action AS int) as [DeleteRule]" & vbCrLf & _
                            "FROM" & vbCrLf & _
                            "  sys.foreign_key_columns fkc INNER JOIN sys.foreign_keys fo ON fkc.constraint_object_id = fo.object_id" & vbCrLf & _
                            "                              INNER JOIN sys.indexes i ON (fo.referenced_object_id = i.object_id) AND" & vbCrLf & _
                            "                                                          (fo.key_index_id = i.index_id)" & vbCrLf & _
                            "                              INNER JOIN sys.objects ro ON fkc.referenced_object_id = ro.object_id" & vbCrLf & _
                            "                              INNER JOIN sys.schemas rs ON rs.schema_id = ro.schema_id" & vbCrLf & _
                            "                              INNER JOIN sys.columns rc ON (rc.object_id = fkc.referenced_object_id) AND" & vbCrLf & _
                            "                                                           (fkc.referenced_column_id = rc.column_id)" & vbCrLf & _
                            "                              INNER JOIN sys.objects po ON fkc.parent_object_id = po.object_id" & vbCrLf & _
                            "                              INNER JOIN sys.schemas ps ON ps.schema_id = po.schema_id" & vbCrLf & _
                            "                              INNER JOIN sys.columns pc ON (pc.object_id = fkc.parent_object_id) AND" & vbCrLf & _
                            "                                                           (fkc.parent_column_id = pc.column_id)" & vbCrLf & _
                            "WHERE" & vbCrLf & _
                            "  (ps.name = 'dbo') AND" & vbCrLf & _
                            "  (po.name = '" & TableName & "') AND" & vbCrLf & _
                            "  (pc.name = '" & ColumnName & "')" & vbCrLf & _
                            "ORDER BY" & vbCrLf & _
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

            Dr = Con.ConsultarDR("select clmns.name" & vbCrLf & _
                        "  from sys.all_objects o, sys.schemas sc, sys.all_columns clmns" & vbCrLf & _
                        "  where o.[schema_id] = sc.[schema_id]" & vbCrLf & _
                        "    and clmns.[object_id] = o.[object_id]" & vbCrLf & _
                        "    and o.name = '" & tablaForanea & "'" & vbCrLf & _
                        "    and sc.name = 'dbo'" & vbCrLf & _
                        "    and clmns.name in ('T_DSC_" & campoTablaForanea.Substring(5) & "'," & vbCrLf & _
                        "'DESCRIPCION_" & campoTablaForanea.Substring(5) & "')")
            If Not Dr Is Nothing Then
                If Dr.Read() Then
                    columnaDescripcion = Convert.ToString(Dr.Item("name"))
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

    Public Function ObtenerCat(ByVal TableName As String) As DataView
        Dim conexion As New Conexion.SQLServer
        Dim dv As DataView

        dv = conexion.ConsultarDT("SELECT" & vbCrLf & _
                            "  fo.name as [FKName]," & vbCrLf & _
                            "  ~fo.is_disabled as [FKEnabled]," & vbCrLf & _
                            "  CAST(db_name() AS sysname) as [PKTableQualifier]," & vbCrLf & _
                            "  rs.name as [PKTableSchema]," & vbCrLf & _
                            "  ro.name as [PKTableName]," & vbCrLf & _
                            "  i.name as [PKName]," & vbCrLf & _
                            "  rc.name as [PKTableColumnName]," & vbCrLf & _
                            "  fkc.constraint_column_id as [KeySequence]," & vbCrLf & _
                            "  CAST(db_name() AS sysname) as [FKTableQualifier]," & vbCrLf & _
                            "  ps.name as [FKTableSchema]," & vbCrLf & _
                            "  po.name as [FKTableName]," & vbCrLf & _
                            "  pc.name as [FKTableColumnName]," & vbCrLf & _
                            "  ~fo.is_not_for_replication as [FKForReplication]," & vbCrLf & _
                            "  ~fo.is_not_trusted as [TrustedFK]," & vbCrLf & _
                            "  CAST(fo.update_referential_action AS int) as [UpdateRule]," & vbCrLf & _
                            "  CAST(fo.delete_referential_action AS int) as [DeleteRule]" & vbCrLf & _
                            "FROM" & vbCrLf & _
                            "  sys.foreign_key_columns fkc INNER JOIN sys.foreign_keys fo ON fkc.constraint_object_id = fo.object_id" & vbCrLf & _
                            "                              INNER JOIN sys.indexes i ON (fo.referenced_object_id = i.object_id) AND" & vbCrLf & _
                            "                                                          (fo.key_index_id = i.index_id)" & vbCrLf & _
                            "                              INNER JOIN sys.objects ro ON fkc.referenced_object_id = ro.object_id" & vbCrLf & _
                            "                              INNER JOIN sys.schemas rs ON rs.schema_id = ro.schema_id" & vbCrLf & _
                            "                              INNER JOIN sys.columns rc ON (rc.object_id = fkc.referenced_object_id) AND" & vbCrLf & _
                            "                                                           (fkc.referenced_column_id = rc.column_id)" & vbCrLf & _
                            "                              INNER JOIN sys.objects po ON fkc.parent_object_id = po.object_id" & vbCrLf & _
                            "                              INNER JOIN sys.schemas ps ON ps.schema_id = po.schema_id" & vbCrLf & _
                            "                              INNER JOIN sys.columns pc ON (pc.object_id = fkc.parent_object_id) AND" & vbCrLf & _
                            "                                                           (fkc.parent_column_id = pc.column_id)" & vbCrLf & _
                            "WHERE" & vbCrLf & _
                            "  (ps.name = 'dbo') AND" & vbCrLf & _
                            "  (po.name = '" & TableName & "')" & vbCrLf & _
                            "ORDER BY" & vbCrLf & _
                            "  [PKTableSchema], [PKTableName], fkc.referenced_column_id").DefaultView

        Return dv

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

    Public Function Baja(camposCondicion As List(Of String), valoresCondicion As List(Of Object)) As Boolean
        Dim resultado As Boolean = False


        Dim conexion As New Conexion.SQLServer
        Try


            Dim bitacora As New Conexion.Bitacora("Borrar imagen", System.Web.HttpContext.Current.Session.SessionID, CType(System.Web.HttpContext.Current.Session(Entities.Usuario.SessionID), Entities.Usuario).IdentificadorUsuario)
            Dim listCampos As New List(Of String)
            Dim listValores As New List(Of Object)


            listCampos.Add(CampoEstatus) : listValores.Add(0)
            listCampos.Add("F_FECH_FIN_VIG") : listValores.Add(Date.Now)


            resultado = conexion.Actualizar(Identificador, listCampos, listValores, camposCondicion, valoresCondicion)
            bitacora.Actualizar(Identificador, listCampos, listValores, camposCondicion, valoresCondicion, resultado, "Error al eliminar imagen")

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
End Class
