Imports System.Data.SqlClient

''' <summary>
''' Clase que sirve para manipular los documentos dinamicos, complementa a la clase Expediente.
''' </summary>
''' <remarks>AGC</remarks>
<Serializable>
Public Class Documento

#Region "Propiedades"
    Private Property _N_ID_DOCUMENTO As Integer
    Private Property _N_ID_TIPO_DOCUMENTO_ORI As Integer
    Private Property _T_NOM_DOCUMENTO_CAT As String
    Private Property _T_NOM_CORTO As String
    Private Property _T_NOM_MACHOTE_ORI As String
    Private Property _T_NOM_MACHOTE_ACTUAL As String
    Private Property _N_NUM_ORDEN As Integer
    Private Property _N_FLAG_OBLI As Integer
    Private Property _N_FLAG_VERSIONADO As Integer
    Private Property _N_NUM_VERSIONES As Integer
    Private Property _N_FLAG_PDF As Integer
    Private Property _N_FLAG_PDF_OBLI As Integer
    Private Property _N_FLAG_SICOD As Integer
    Private Property _N_FLAG_CONFIRMACION As Integer
    Private Property _N_FLAG_CONFIRM_PDF As Integer
    Private Property _N_FLAG_VIG As Integer
    Private Property _F_FECH_INI_VIG As DateTime
    Private Property _F_FECH_FI_N_VIG As DateTime
    Private Property _I_ID_PASO_INI As Integer
    Private Property _I_ID_PASO_FIN As Integer
    Private Property _I_ID_PASO_PDF_INI As Integer
    Private Property _I_ID_PASO_PDF_FIN As Integer
    Private Property _N_FLAG_TERMINA_CARGA_DOCS As Integer
    Private Property _N_FLAG_TERMINA_CARGA_PDF As Integer

    Private Property _T_NOM_DOCUMENTO As String

    Private Property _T_NOM_DOCUMENTO_ORI As String

    Private Property _N_ID_TIPO_DOCUMENTO As Integer

    Private Property _T_ID_USUARIO As String

    Private Property _N_ID_VERSION As Integer

    Private Property _NUM_VERSIONES As Integer

    Private Property _BANDERA_PASO_HABILITADO As Integer

    Private Property _T_NOM_DOCUMENTO_ADJ As String

    Private Property _N_FLAG_HEREDA As Integer

    Private Property _N_FLAG_HEREDA_ENTRE_SBVISITA As Integer

    Private Property _N_FLAG_APLICA_NOMENCLATURA As Integer

    '':( Esto era por si un documento se podia visializar en pasos no consecutivos
    Private Property _PASOS_HABILES As List(Of PasosHabilita)

    Public Property N_ID_DOCUMENTO As Integer
        Get
            Return IIf(IsNothing(_N_ID_DOCUMENTO), 0, _N_ID_DOCUMENTO)
        End Get
        Set(value As Integer)
            _N_ID_DOCUMENTO = value
        End Set
    End Property

    Public Property N_ID_TIPO_DOCUMENTO_ORI As Integer
        Get
            Return IIf(IsNothing(_N_ID_TIPO_DOCUMENTO_ORI), 0, _N_ID_TIPO_DOCUMENTO_ORI)
        End Get
        Set(value As Integer)
            _N_ID_TIPO_DOCUMENTO_ORI = value
        End Set
    End Property

    Public Property T_NOM_DOCUMENTO_CAT As String
        Get
            Return IIf(IsNothing(_T_NOM_DOCUMENTO_CAT), "", _T_NOM_DOCUMENTO_CAT)
        End Get
        Set(value As String)
            _T_NOM_DOCUMENTO_CAT = value
        End Set
    End Property

    Public Property T_NOM_CORTO As String
        Get
            Return IIf(IsNothing(_T_NOM_CORTO), "", _T_NOM_CORTO)
        End Get
        Set(value As String)
            _T_NOM_CORTO = value
        End Set
    End Property

    Public Property T_NOM_MACHOTE_ORI As String
        Get
            Return IIf(IsNothing(_T_NOM_MACHOTE_ORI), "", _T_NOM_MACHOTE_ORI)
        End Get
        Set(value As String)
            _T_NOM_MACHOTE_ORI = value
        End Set
    End Property

    Public Property T_NOM_MACHOTE_ACTUAL As String
        Get
            Return IIf(IsNothing(_T_NOM_MACHOTE_ACTUAL), "", _T_NOM_MACHOTE_ACTUAL)
        End Get
        Set(value As String)
            _T_NOM_MACHOTE_ACTUAL = value
        End Set
    End Property

    Public Property N_NUM_ORDEN As Integer
        Get
            Return IIf(IsNothing(_N_NUM_ORDEN), 0, _N_NUM_ORDEN)
        End Get
        Set(value As Integer)
            _N_NUM_ORDEN = value
        End Set
    End Property

    Public Property N_FLAG_OBLI As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_OBLI), 0, _N_FLAG_OBLI)
        End Get
        Set(value As Integer)
            _N_FLAG_OBLI = value
        End Set
    End Property

    Public Property N_FLAG_VERSIONADO As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_VERSIONADO), 0, _N_FLAG_VERSIONADO)
        End Get
        Set(value As Integer)
            _N_FLAG_VERSIONADO = value
        End Set
    End Property

    Public Property N_NUM_VERSIONES As Integer
        Get
            Return IIf(IsNothing(_N_NUM_VERSIONES), 0, _N_NUM_VERSIONES)
        End Get
        Set(value As Integer)
            _N_NUM_VERSIONES = value
        End Set
    End Property

    Public Property N_FLAG_PDF As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_PDF), 0, _N_FLAG_PDF)
        End Get
        Set(value As Integer)
            _N_FLAG_PDF = value
        End Set
    End Property
    Public Property N_FLAG_PDF_OBLI As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_PDF_OBLI), 0, _N_FLAG_PDF_OBLI)
        End Get
        Set(value As Integer)
            _N_FLAG_PDF_OBLI = value
        End Set
    End Property
    Public Property N_FLAG_SICOD As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_SICOD), 0, _N_FLAG_SICOD)
        End Get
        Set(value As Integer)
            _N_FLAG_SICOD = value
        End Set
    End Property
    Public Property FolioSICOD As String
    Public Property I_ID_PASO As Integer
    Public Property IdVisita As Integer
    Public Property T_DSC_COMENTARIO As String
    Public Property N_ID_DOCUMENTO_PASO As Integer
    Public Property TipoOficio As Integer

    Public Property N_FLAG_CONFIRMACION As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_CONFIRMACION), 0, _N_FLAG_CONFIRMACION)
        End Get
        Set(value As Integer)
            _N_FLAG_CONFIRMACION = value
        End Set
    End Property

    Public Property N_FLAG_CONFIRM_PDF As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_CONFIRM_PDF), 0, _N_FLAG_CONFIRM_PDF)
        End Get
        Set(value As Integer)
            _N_FLAG_CONFIRM_PDF = value
        End Set
    End Property

    Public Property N_FLAG_VIG As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_VIG), 0, _N_FLAG_VIG)
        End Get
        Set(value As Integer)
            _N_FLAG_VIG = value
        End Set
    End Property

    Public Property F_FECH_INI_VIG As DateTime
        Get
            Return IIf(IsNothing(_F_FECH_INI_VIG), DateTime.MinValue, _F_FECH_INI_VIG)
        End Get
        Set(value As DateTime)
            _F_FECH_INI_VIG = value
        End Set
    End Property

    Public Property F_FECH_FI_N_VIG As DateTime
        Get
            Return IIf(IsNothing(_F_FECH_INI_VIG), DateTime.MinValue, _F_FECH_INI_VIG)
        End Get
        Set(value As DateTime)
            _F_FECH_FI_N_VIG = value
        End Set
    End Property

    Public Property I_ID_PASO_INI As Integer
        Get
            Return IIf(IsNothing(_I_ID_PASO_INI), 0, _I_ID_PASO_INI)
        End Get
        Set(value As Integer)
            _I_ID_PASO_INI = value
        End Set
    End Property
    Public Property I_ID_PASO_FIN As Integer
        Get
            Return IIf(IsNothing(_I_ID_PASO_FIN), 0, _I_ID_PASO_FIN)
        End Get
        Set(value As Integer)
            _I_ID_PASO_FIN = value
        End Set
    End Property
    Public Property I_ID_PASO_PDF_INI As Integer
        Get
            Return IIf(IsNothing(_I_ID_PASO_PDF_INI), -1, _I_ID_PASO_PDF_INI)
        End Get
        Set(value As Integer)
            _I_ID_PASO_PDF_INI = value
        End Set
    End Property
    Public Property I_ID_PASO_PDF_FIN As Integer
        Get
            Return IIf(IsNothing(_I_ID_PASO_PDF_FIN), -1, _I_ID_PASO_PDF_FIN)
        End Get
        Set(value As Integer)
            _I_ID_PASO_PDF_FIN = value
        End Set
    End Property

    Public Property N_FLAG_TERMINA_CARGA_DOCS As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_TERMINA_CARGA_DOCS), 0, _N_FLAG_TERMINA_CARGA_DOCS)
        End Get
        Set(value As Integer)
            _N_FLAG_TERMINA_CARGA_DOCS = value
        End Set
    End Property

    Public Property N_FLAG_TERMINA_CARGA_PDF As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_TERMINA_CARGA_PDF), 0, _N_FLAG_TERMINA_CARGA_PDF)
        End Get
        Set(value As Integer)
            _N_FLAG_TERMINA_CARGA_PDF = value
        End Set
    End Property

    Public Property T_NOM_DOCUMENTO As String
        Get
            Return IIf(IsNothing(_T_NOM_DOCUMENTO), "", _T_NOM_DOCUMENTO)
        End Get
        Set(value As String)
            _T_NOM_DOCUMENTO = value
        End Set
    End Property

    Public Property T_NOM_DOCUMENTO_ORI As String
        Get
            Return IIf(IsNothing(_T_NOM_DOCUMENTO_ORI), "", _T_NOM_DOCUMENTO_ORI)
        End Get
        Set(value As String)
            _T_NOM_DOCUMENTO_ORI = value
        End Set
    End Property

    Public Property N_ID_TIPO_DOCUMENTO As Integer
        Get
            Return IIf(IsNothing(_N_ID_TIPO_DOCUMENTO), 0, _N_ID_TIPO_DOCUMENTO)
        End Get
        Set(value As Integer)
            _N_ID_TIPO_DOCUMENTO = value
        End Set
    End Property

    Public Property N_ID_VERSION As Integer
        Get
            Return IIf(IsNothing(_N_ID_VERSION), 0, _N_ID_VERSION)
        End Get
        Set(value As Integer)
            _N_ID_VERSION = value
        End Set
    End Property

    Public Property NUM_VERSIONES As Integer
        Get
            Return IIf(IsNothing(_NUM_VERSIONES), 0, _NUM_VERSIONES)
        End Get
        Set(value As Integer)
            _NUM_VERSIONES = value
        End Set
    End Property

    Public Property BANDERA_PASO_HABILITADO As Integer
        Get
            Return IIf(IsNothing(_BANDERA_PASO_HABILITADO), 0, _BANDERA_PASO_HABILITADO)
        End Get
        Set(value As Integer)
            _BANDERA_PASO_HABILITADO = value
        End Set
    End Property

    Public Property T_ID_USUARIO As String
        Get
            Return IIf(IsNothing(_T_ID_USUARIO), "", _T_ID_USUARIO)
        End Get
        Set(value As String)
            _T_ID_USUARIO = value
        End Set
    End Property

    Public Property T_NOM_DOCUMENTO_ADJ As String
        Get
            Return IIf(IsNothing(_T_NOM_DOCUMENTO_ADJ), "", _T_NOM_DOCUMENTO_ADJ)
        End Get
        Set(value As String)
            _T_NOM_DOCUMENTO_ADJ = value
        End Set
    End Property

    Public Property N_FLAG_HEREDA As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_HEREDA), 0, _N_FLAG_HEREDA)
        End Get
        Set(value As Integer)
            _N_FLAG_HEREDA = value
        End Set
    End Property

    Public Property N_FLAG_HEREDA_ENTRE_SBVISITA As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_HEREDA_ENTRE_SBVISITA), 0, _N_FLAG_HEREDA_ENTRE_SBVISITA)
        End Get
        Set(value As Integer)
            _N_FLAG_HEREDA_ENTRE_SBVISITA = value
        End Set
    End Property

    Public Property N_FLAG_APLICA_NOMENCLATURA As Integer
        Get
            Return IIf(IsNothing(_N_FLAG_APLICA_NOMENCLATURA), 0, _N_FLAG_APLICA_NOMENCLATURA)
        End Get
        Set(value As Integer)
            _N_FLAG_APLICA_NOMENCLATURA = value
        End Set
    End Property

    Public Property PASOS_HABILES As List(Of PasosHabilita)
        Get
            Return _PASOS_HABILES
        End Get
        Set(value As List(Of PasosHabilita))
            _PASOS_HABILES = value
        End Set
    End Property

    ''PROPIEDAD PARA LOS PASOS
    <Serializable>
    Public Class PasosHabilita
        Private Property I_ID_PASO_INI As Integer
        Private Property I_ID_PASO_FIN As Integer
        Private Property I_ID_PASO_PDF_INI As Integer
        Private Property I_ID_PASO_PDF_FIN As Integer
    End Class

    ''Clase documentos ligera
    <Serializable>
    Public Class DocumentoMini
        Private Property _N_ID_DOCUMENTO As Integer
        Private Property _T_NOM_DOCUMENTO_CAT As String
        Public Property I_ID_PASO As Integer

        Public Property N_ID_DOCUMENTO As Integer
            Get
                Return IIf(IsNothing(_N_ID_DOCUMENTO), 0, _N_ID_DOCUMENTO)
            End Get
            Set(value As Integer)
                _N_ID_DOCUMENTO = value
            End Set
        End Property


        Public Property T_NOM_DOCUMENTO_CAT As String
            Get
                Return IIf(IsNothing(_T_NOM_DOCUMENTO_CAT), "", _T_NOM_DOCUMENTO_CAT)
            End Get
            Set(value As String)
                _T_NOM_DOCUMENTO_CAT = value
            End Set
        End Property
    End Class
#End Region

    Public Function getTipoDocumento() As DataTable
        Dim con As Conexion.SQLServer = Nothing
        Dim dt As DataTable

        Try

            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_TIPO_DOCUMENTO")

            dt = con.EjecutarSPConsultaDT(sp)

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Documento.vb, getTipoDocumento", "")
            dt = New DataTable
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return dt

    End Function

    ''' <summary>
    ''' Da de alta/actualiza un documento, debe de estar llena la entidad actual
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function InsertarActualizarDocumento() As Integer
        Dim con As Conexion.SQLServer = Nothing
        Dim liRes As Integer

        Try

            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spI_BDS_MERGE_DOCUMENTO")

            liRes = con.EjecutarSP(sp, Me)

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Documento.vb, getTipoDocumento", "")
            liRes = False
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return liRes
    End Function

    ''' <summary>
    ''' Consulta los documentos, todos, por visita o por usuario cuando se esta insertando una nueva visita.
    ''' </summary>
    ''' <param name="piIdVisita"></param>
    ''' <param name="piIdUsuario"></param>
    ''' <param name="piIdPaso"></param>
    ''' <param name="piIdDocumento"></param>
    ''' <param name="piIdVigencia"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
   Public Function getDocumentos(ByVal idVisitaMC As Integer,
                                 Optional piIdVigencia As Integer = Constantes.Vigencia.NoConsideraVigencia,
                                 Optional piIdPaso As Integer = Constantes.Todos,
                                 Optional piIdVisita As Integer = Constantes.Todos,
                                 Optional piIdUsuario As String = "",
                                 Optional piIdDocumento As Integer = Constantes.Todos,
                                 Optional piIdOpcion As Integer = Constantes.Todos) As DataTable
      Dim con As Conexion.SQLServer = Nothing
      Dim dt As DataTable

      Dim visita As New Visita()
      visita = AccesoBD.getDetalleVisita(idVisitaMC, -1)
      Dim fechaProcesoNvo = Conexion.SQLServer.Parametro.ObtenerValor(Constantes.Parametros.FechaInicialProceso2017)

      Try

         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim sp As String = Nothing

         If Not IsNothing(visita) Then
            If (Convert.ToDateTime(visita.FechaRegistro).ToString("yyyy/MM/dd") < Convert.ToDateTime(fechaProcesoNvo).ToString("yyyy/MM/dd")) Then
               sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DOCUMENTOS_V2")
            Else
               If piIdOpcion = 9 Then
                  sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DOCUMENTOS_V2_V17_PRORROGA")
               Else
                  sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DOCUMENTOS_V2_V17")
               End If
            End If
         End If

         Dim sqlParameter(4) As SqlParameter
         sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
         sqlParameter(1) = New SqlParameter("@I_ID_USUARIO", piIdUsuario)
         sqlParameter(2) = New SqlParameter("@I_ID_PASO", piIdPaso)
         sqlParameter(3) = New SqlParameter("@N_ID_DOCUMENTO", piIdDocumento)
         sqlParameter(4) = New SqlParameter("@ID_VIGENCIA", piIdVigencia)

         dt = con.EjecutarSPConsultaDT(sp, sqlParameter)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Documento.vb, getDocumentos", "")
         dt = New DataTable
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

      Return dt

   End Function

    ''' <summary>
    ''' Obtiene todos los documentos de una visita pero agrupando por documento
    ''' </summary>
    ''' <param name="piIdVigencia"></param>
    ''' <param name="piIdPaso"></param>
    ''' <param name="piIdVisita"></param>
    ''' <param name="piIdUsuario"></param>
    ''' <param name="piIdDocumento"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getDocumentosAgrupados(Optional piIdVigencia As Integer = Constantes.Vigencia.NoConsideraVigencia,
                                  Optional piIdPaso As Integer = Constantes.Todos,
                                  Optional piIdVisita As Integer = Constantes.Todos,
                                  Optional piIdUsuario As String = "",
                                  Optional piIdDocumento As Integer = Constantes.Todos) As DataTable
        Dim con As Conexion.SQLServer = Nothing
        Dim dt As DataTable

        Try

            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim sp = String.Format("{0}[{1}]", Owner, "spS_BDS_GRL_GET_DOCUMENTOS_AGRUPADOS")

            Dim sqlParameter(4) As SqlParameter
            sqlParameter(0) = New SqlParameter("@I_ID_VISITA", piIdVisita)
            sqlParameter(1) = New SqlParameter("@I_ID_USUARIO", piIdUsuario)
            sqlParameter(2) = New SqlParameter("@I_ID_PASO", piIdPaso)
            sqlParameter(3) = New SqlParameter("@N_ID_DOCUMENTO", piIdDocumento)
            sqlParameter(4) = New SqlParameter("@ID_VIGENCIA", piIdVigencia)

            dt = con.EjecutarSPConsultaDT(sp, sqlParameter)

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Documento.vb, getDocumentos", "")
            dt = New DataTable
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

        Return dt

    End Function

    'RRA METODOS PARA PASO 6
    Public Function ActualizaEstatusDocumento(ByVal idVisita As String, ByVal idDocumento As String, ByVal estado As Int32, ByVal Version As String) As Boolean
        Dim con As Conexion.SQLServer = Nothing
        Dim Where As String = ""
        Try
            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            Dim query As String = "update BDS_D_VS_DOCUMENTO_PASO SET T_ID_ESTATUS_DOC = " + estado.ToString + " WHERE I_ID_VISITA = " + idVisita + " AND N_ID_DOCUMENTO = " + idDocumento + " AND N_ID_VERSION = " + Version

            Return con.Ejecutar(query)

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Documento.vb, getDocumentos", "")
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try

    End Function

   Public Function ActualizaEstatusDocumentoComentarios(ByVal idVisita As String, ByVal idDocumento As String, ByVal estado As Int32,
                                                        ByVal Version As String, ByVal comentariosRechaza As String) As Boolean
      Dim con As Conexion.SQLServer = Nothing
      Dim Where As String = ""
      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim query As String = "update BDS_D_VS_DOCUMENTO_PASO SET T_ID_ESTATUS_DOC = " + estado.ToString + ", T_DSC_COMENTARIO = '" + comentariosRechaza + "' WHERE I_ID_VISITA = " + idVisita + " AND N_ID_DOCUMENTO = " + idDocumento + " AND N_ID_VERSION = " + Version

         Return con.Ejecutar(query)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Documento.vb, getDocumentos", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

   End Function

   Public Function ActualizaEstatusDoctoAprueba(ByVal idVisita As String, ByVal idDocumento As String, ByVal estado As Int32,
                                                ByVal Version As String, ByVal comentariosAprueba As String) As Boolean
      Dim con As Conexion.SQLServer = Nothing
      Dim Where As String = ""
      Try
         con = New Conexion.SQLServer
         Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
         Dim query As String = "update BDS_D_VS_DOCUMENTO_PASO SET T_ID_ESTATUS_DOC = " + estado.ToString + ", T_DSC_COMENTARIO = '" + comentariosAprueba + "' WHERE I_ID_VISITA = " + idVisita + " AND N_ID_DOCUMENTO = " + idDocumento + " AND N_ID_VERSION = " + Version

         Return con.Ejecutar(query)

      Catch ex As Exception
         Utilerias.ControlErrores.EscribirEvento(ex.ToString(), EventLogEntryType.Error, "Documento.vb, getDocumentos", "")
      Finally
         If Not IsNothing(con) Then
            con.CerrarConexion()
         End If
      End Try

   End Function

    Public Function ValidaEstadoDocumento(ByVal idVisita As String, ByVal idDocumento As String, ByVal Version As String) As String
        Dim con As Conexion.SQLServer = Nothing
        Dim tabla As New DataTable
        Dim valor As String = ""
        Dim lsQuery As String = ""
        Try
            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            lsQuery = "SELECT T_ID_ESTATUS_DOC FROM BDS_D_VS_DOCUMENTO_PASO " & _
                                  "WHERE I_ID_VISITA = " + idVisita + " AND N_ID_DOCUMENTO = " + idDocumento + " AND N_ID_VERSION = " + Version
            Dim ldDT As DataTable = con.ConsultarDT(lsQuery)

            If Not IsNothing(ldDT) Then
                If ldDT.Rows.Count > 0 Then
                    valor = (From tr In con.ConsultarDT(lsQuery) Select tr.Item("T_ID_ESTATUS_DOC")).FirstOrDefault.ToString
                End If
            End If

            If valor = "" Or valor = "Nothing" Then
                valor = "19"
            End If
        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString() & " - " & lsQuery, EventLogEntryType.Error, "Documento.vb, ValidaEstadoDocumento", "")
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try
        Return valor
    End Function

    Public Sub ActualizarFolioSICOD()
        Dim conexion As New Conexion.SQLServer()
        Dim campos As New List(Of String)
        Dim valores As New List(Of Object)
        Dim camposCondicion As New List(Of String)
        Dim valoresCondicion As New List(Of Object)

        campos.Add("T_FOLIO_SICOD")
        valores.Add(FolioSICOD)

        camposCondicion.Add("I_ID_VISITA")
        valoresCondicion.Add(IdVisita)

        conexion.Actualizar("BDS_D_VS_DOCUMENTO_PASO", campos, valores, camposCondicion, valoresCondicion)

        conexion.CerrarConexion()

    End Sub
    Public Sub AgregarLigar()
        Dim conexion As New Conexion.SQLServer()
        Dim campos As New List(Of String)
        Dim valores As New List(Of Object)
        campos.Add("I_ID_VISITA")
        campos.Add("N_ID_DOCUMENTO")
        campos.Add("T_FOLIO_SICOD")
        campos.Add("T_NOM_DOCUMENTO")
        campos.Add("T_NOM_DOCUMENTO_ORI")
        campos.Add("F_FECH_REGISTRO")
        campos.Add("T_ID_USUARIO")
        campos.Add("I_ID_PASO")
        campos.Add("N_ID_VERSION")

        valores.Add(IdVisita)
        valores.Add(N_ID_DOCUMENTO)
        valores.Add(FolioSICOD)
        valores.Add(T_NOM_DOCUMENTO)
        valores.Add(T_NOM_DOCUMENTO_ORI)
        valores.Add(Date.Now)
        valores.Add(T_ID_USUARIO)
        valores.Add(I_ID_PASO)
        valores.Add(N_ID_VERSION)

        conexion.Insertar("BDS_D_VS_DOCUMENTO_PASO", campos, valores)

        conexion.CerrarConexion()

    End Sub
    '----------------------
    Public Function RecuperaFolioSicod() As String
        Dim con As Conexion.SQLServer = Nothing
        Dim tabla As New DataTable
        Dim valor As String = ""
        Dim lsQuery As String = ""
        Try
            con = New Conexion.SQLServer
            Dim Owner As String = Web.Configuration.WebConfigurationManager.AppSettings("Owner") & "."
            lsQuery = "SELECT T_FOLIO_SICOD FROM BDS_D_VS_DOCUMENTO_PASO " &
                                  "WHERE I_ID_VISITA = " + IdVisita.ToString() + " AND N_ID_DOCUMENTO = " + Me.N_ID_DOCUMENTO.ToString() '+ " AND N_ID_VERSION = " + Version
            Dim ldDT As DataTable = con.ConsultarDT(lsQuery)

            If Not IsNothing(ldDT) Then
                If ldDT.Rows.Count > 0 Then
                    valor = (From tr In con.ConsultarDT(lsQuery) Select tr.Item("T_FOLIO_SICOD")).FirstOrDefault.ToString
                    Me.FolioSICOD = valor
                End If
            End If

        Catch ex As Exception
            Utilerias.ControlErrores.EscribirEvento(ex.ToString() & " - " & lsQuery, EventLogEntryType.Error, "Documento.vb, ValidaEstadoDocumento", "")
        Finally
            If Not IsNothing(con) Then
                con.CerrarConexion()
            End If
        End Try
        Return valor
    End Function

End Class
