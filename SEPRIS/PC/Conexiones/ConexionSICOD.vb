Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports Utilerias
Imports System.Reflection
Imports System.ComponentModel
Imports Clases

Public Class ConexionSICOD

    Public Property ObtenerQuery As String

    Public Shared Function ObtenerEntidadesAFOREArea(Area As Integer) As DataTable


        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim dsEntidades As New DataSet
        dsEntidades.Tables.Add("Entidades")

        Dim Usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        Dim vecTipoEntidades As New List(Of Integer)

        Dim usuarioActual As New Entities.Usuario()

        If Area = 35 Or Area = 1 Then
            '<Value>AFORE</Value>
            '<Value>EMPRESA OPERADORA</Value> 
            vecTipoEntidades.Add(1)
            vecTipoEntidades.Add(7)
            vecTipoEntidades.Add(12)
        ElseIf Area = 36 Then
            '<Value>AFORE</Value>
            vecTipoEntidades.Add(1)
            vecTipoEntidades.Add(2)
            vecTipoEntidades.Add(3)
            vecTipoEntidades.Add(4)
            vecTipoEntidades.Add(17)
            vecTipoEntidades.Add(7)
            vecTipoEntidades.Add(12)
            'vecTipoEntidades.Add(12)
        Else
            'Administrador
            '<Value>AFORE</Value>
            '<Value>EMPRESA OPERADORA</Value> 
            vecTipoEntidades.Add(1)
            vecTipoEntidades.Add(7)
            vecTipoEntidades.Add(12)
            vecTipoEntidades.Add(2)
            vecTipoEntidades.Add(3)
            vecTipoEntidades.Add(4)
            vecTipoEntidades.Add(17)


        End If

        Dim mycredentialCache As System.Net.CredentialCache = New System.Net.CredentialCache()
        Dim credentials As System.Net.NetworkCredential = New System.Net.NetworkCredential(Usuario, Password, Dominio)
        Dim proxySICOD As New WR_SICOD.ws_SICOD
        proxySICOD.Credentials = credentials
        proxySICOD.ConnectionGroupName = "SEPRIS"
        Dim lstEntidadesSicod As New List(Of Entities.EntidadSicod)

        For Each ent As Integer In vecTipoEntidades
            dsEntidades.Tables(0).Merge(proxySICOD.GetEntidadesComplete(ent).Tables(0))
            dsEntidades.AcceptChanges()
        Next


        Return dsEntidades.Tables(0)

    End Function


    Public Shared Function ObtenerEntidadesAFORE() As DataTable


        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim dsEntidades As New DataSet
        dsEntidades.Tables.Add("Entidades")

        Dim Usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        Dim vecTipoEntidades As New List(Of Integer)

        Dim usuarioActual As New Entities.Usuario()

        vecTipoEntidades.Add(1)
        vecTipoEntidades.Add(2)
        vecTipoEntidades.Add(3)
        vecTipoEntidades.Add(4)
        vecTipoEntidades.Add(7)
        vecTipoEntidades.Add(17)

        Dim mycredentialCache As System.Net.CredentialCache = New System.Net.CredentialCache()
        Dim credentials As System.Net.NetworkCredential = New System.Net.NetworkCredential(Usuario, Password, Dominio)
        Dim proxySICOD As New WR_SICOD.ws_SICOD
        proxySICOD.Credentials = credentials
        proxySICOD.ConnectionGroupName = "SEPRIS"
        Dim lstEntidadesSicod As New List(Of Entities.EntidadSicod)

        Try
            For Each ent As Integer In vecTipoEntidades
                dsEntidades.Tables(0).Merge(proxySICOD.GetEntidadesComplete(ent).Tables(0))
                dsEntidades.AcceptChanges()
            Next
        Catch ex As Exception
            Dim txerror As String = ex.Message
        End Try



        Return dsEntidades.Tables(0)

    End Function

    Public Shared Function ObtenerNombreEntidad(Entidad As String) As String

        'SELECT T_ENTIDAD_CORTO, T_ENTIDAD_LARGO, ID_ENTIDAD, * From dbo.BDA_ENTIDAD Where VIG_FLAG = 1 And [ID_ADMINISTRADA POR] = 0

        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNencAux").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNencAux").ToString())
        End If
        If Entidad = 1 Then
            data = conexion.ConsultarDT("Select [T_ENTIDAD_CORTO] FROM [dbo].[BDA_ENTIDAD] Where VIG_FLAG = 1 And ID_TIPO_ENTIDAD = 14 And CVE_ID_ENT=" + Entidad)
        Else
            'data = conexion.ConsultarDT("SELECT [T_ENTIDAD_CORTO] FROM [dbo].[BDA_ENTIDAD] Where VIG_FLAG = 1 and ID_TIPO_ENTIDAD=3 and CVE_ID_ENT= " + Entidad)
            data = conexion.ConsultarDT("SELECT [T_ENTIDAD_CORTO] FROM [dbo].[BDA_ENTIDAD] Where VIG_FLAG = 1  and CVE_ID_ENT= " + Entidad)
        End If


        conexion.CerrarConexion()

        Dim rows As String = ""
        If data.Rows.Count > 0 Then
            rows = data.Rows(0).ItemArray(0).ToString()
        End If

        Return rows.ToString()
    End Function

    Public Shared Function ObtenerNombreEntidadporTipoEntidad(IDTipoEntidad As Integer, Entidad As String) As String

        'SELECT T_ENTIDAD_CORTO, T_ENTIDAD_LARGO, ID_ENTIDAD, * From dbo.BDA_ENTIDAD Where VIG_FLAG = 1 And [ID_ADMINISTRADA POR] = 0

        'Dim conexion As Conexion.SQLServer = Nothing
        'Dim data As DataTable
        'If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
        '    conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNencAux").ToString())
        'Else
        '    conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNencAux").ToString())
        'End If

        'data = conexion.ConsultarDT("SELECT [T_ENTIDAD_CORTO] FROM [dbo].[BDA_ENTIDAD] Where VIG_FLAG = 1 and ID_TIPO_ENTIDAD=" & IDTipoEntidad & " and CVE_ID_ENT= " + Entidad)

        'conexion.CerrarConexion()

        'Dim rows As String = ""
        'If data.Rows.Count > 0 Then
        '    rows = data.Rows(0).ItemArray(0).ToString()
        'End If

        'Return rows.ToString()



        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim Usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        Dim mycredentialCache As System.Net.CredentialCache = New System.Net.CredentialCache()
        Dim credentials As System.Net.NetworkCredential = New System.Net.NetworkCredential(Usuario, Password, Dominio)
        Dim proxySICOD As New WR_SICOD.ws_SICOD
        proxySICOD.Credentials = credentials
        proxySICOD.ConnectionGroupName = "SEPRIS"

        Dim data As DataSet = proxySICOD.GetDatosEntidad(Entidad, IDTipoEntidad)
        Return data.Tables(0).Rows(0)("SIGLAS_ENT")



    End Function

    Public Shared Function FolioFinalizado(Folio As String) As Boolean

        'ID_ESTATUS  T_ESTATUS
        '1   En Elaboración
        '2   En Revisión
        '3   En Firma
        '4   Notificado - Falta Acuse
        '5   Se Espera Información
        '6   Concluido
        '7   Cancelado
        '8   Se Esp. Inf. - Falta Acuse
        '9   Por Dictaminar
        '10  Dictaminado
        '11  Para Ensobretar
        '12  Para Notificar
        '13  Vista previa p/imprimir
        '14  Enviado
        '15  Apartado

        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNencAux").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNencAux").ToString())
        End If
        data = conexion.ConsultarDT("SELECT T_OFICIO_NUMERO, ID_ESTATUS FROM [dbo].[BDA_OFICIO] WHERE T_OFICIO_NUMERO = '" + Folio + "'")
        conexion.CerrarConexion()

        Dim dvOficio As DataView = data.DefaultView
        dvOficio.RowFilter = "ID_ESTATUS = 6"

        If dvOficio.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Shared Function ObtenerFechaAcuse(Folio As String) As String
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNencAux").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNencAux").ToString())
        End If
        'CAGC FSW 
        data = conexion.ConsultarDT("SELECT CONVERT(VARCHAR(8), F_FECHA_ACUSE, 3) FROM BDA_OFICIO where T_OFICIO_NUMERO = '" + Folio + "'")
        conexion.CerrarConexion()
        If (data.Rows.Count = 0) Then
            Return ""
        Else
            Return data.Rows(0).Item(0).ToString()
        End If

    End Function


    Public Shared Function ObtenerNombreEntidadN(Entidad As String) As String

        'SELECT T_ENTIDAD_CORTO, T_ENTIDAD_LARGO, ID_ENTIDAD, * From dbo.BDA_ENTIDAD Where VIG_FLAG = 1 And [ID_ADMINISTRADA POR] = 0

        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNencAux").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNencAux").ToString())
        End If

        data = conexion.ConsultarDT("SELECT [CVE_ID_ENT] FROM [dbo].[BDA_ENTIDAD] Where VIG_FLAG = 1 and ID_TIPO_ENTIDAD=3 and T_ENTIDAD_CORTO = '" + Entidad + "'")

        conexion.CerrarConexion()

        Dim rows As String = ""
        If data.Rows.Count > 0 Then
            rows = data.Rows(0).ItemArray(0).ToString()
        End If

        Return rows.ToString()
    End Function
    Public Shared Function ObtenerSubEntidadesAFORE(Entidad As Integer) As DataTable
        'AMMM 12/07/2019 metodo copia del obtenerSubentidadesAFORE  que trae solo las subentidades del tipo entidad
        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim dsSubEntidades As New DataSet
        dsSubEntidades.Tables.Add("subEntidades")

        Dim Usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")

        Dim mycredentialCache As System.Net.CredentialCache = New System.Net.CredentialCache()
        Dim credentials As System.Net.NetworkCredential = New System.Net.NetworkCredential(Usuario, Password, Dominio)
        Dim proxySICOD As New WR_SICOD.ws_SICOD
        proxySICOD.Credentials = credentials
        proxySICOD.ConnectionGroupName = "SEPRIS"

        dsSubEntidades.Tables(0).Merge(proxySICOD.GetSubEntidadesComplete(Entidad).Tables(0))
        dsSubEntidades.AcceptChanges()
        'Dim mycredentialCache As System.Net.CredentialCache = New System.Net.CredentialCache()
        'Dim credentials As System.Net.NetworkCredential = New System.Net.NetworkCredential(Usuario, Password, Dominio)
        'Dim proxyOsiris As New WR_Osiris.Osiris
        'proxyOsiris.Credentials = credentials

        'CAGC FSW SOFTTEK
        Try

            Return RemoveDuplicateRows(dsSubEntidades.Tables(0))
            'Return dsSubEntidades.Tables(0)
            'Return proxyOsiris.ObtenerSubEntidades(Entidad)
        Catch ex As Exception
            Dim tabla As DataTable = New DataTable()
            Return tabla
        End Try

    End Function
    Public Shared Function RemoveDuplicateRows(rDataTable As DataTable) As DataTable
        Dim pNewDataTable As DataTable
        Dim pCurrentRowCopy As DataRow
        Dim pColumnList As New List(Of String)
        Dim pColumn As DataColumn

        'Build column list
        For Each pColumn In rDataTable.Columns
            If pColumn.ColumnName <> "FECH_INI_VIG" Then
                pColumnList.Add(pColumn.ColumnName)
            End If
        Next

        'Filter by selected columns
        pNewDataTable = rDataTable.DefaultView.ToTable(True, pColumnList.ToArray)

        rDataTable = rDataTable.Clone

        'Import rows into original table structure
        For Each pCurrentRowCopy In pNewDataTable.Rows
            rDataTable.ImportRow(pCurrentRowCopy)
        Next

        Return rDataTable
    End Function
    Public Shared Function ObtenerSubEntidadesAFORECompletas(Entidad As Integer) As DataTable
        'AMMM 12/07/2019 metodo original que trae todas las subentidades de la AFORE
        Dim enc As New YourCompany.Utils.Encryption.Encryption64


        Dim Usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")


        Dim mycredentialCache As System.Net.CredentialCache = New System.Net.CredentialCache()
        Dim credentials As System.Net.NetworkCredential = New System.Net.NetworkCredential(Usuario, Password, Dominio)
        Dim proxyOsiris As New WR_Osiris.Osiris
        proxyOsiris.Credentials = credentials

        'CAGC FSW SOFTTEK
        Try
            Return proxyOsiris.ObtenerSubEntidades(Entidad)
        Catch ex As Exception
            Dim tabla As DataTable = New DataTable()
            Return tabla
        End Try


    End Function

    Public Shared Function EliminarPC(Folio As Integer) As Integer
        Dim conexion As Conexion.SQLServer = Nothing
        Dim result As Integer
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaDSNencAux").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdDSNencAux").ToString())
        End If

        result = conexion.Ejecutar("UPDATE BDA_INFO_DOC SET ID_SISTEMA = 1 WHERE ID_FOLIO = " + Folio.ToString())
        conexion.Ejecutar("UPDATE BDA_R_DOC_COPIAS SET ESTATUS_TRAMITE = 0 WHERE ID_FOLIO = " + Folio.ToString())
        conexion.Ejecutar("UPDATE BDA_TURNADO SET ESTATUS_TRAMITE = 0 WHERE ID_FOLIO = " + Folio.ToString())
        conexion.CerrarConexion()

        Return result

    End Function

    Public Shared Function IsFestivo(Fecha As String) As String
        Dim conexion As Conexion.SQLServer = Nothing
        Dim data As DataTable
        If WebConfigurationManager.AppSettings("Desarrollo").ToString() = "True" Then
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("DesaSeprisDSNenc").ToString())
        Else
            conexion = New Conexion.SQLServer(WebConfigurationManager.AppSettings("ProdSeprisDSNenc").ToString())
        End If

        data = conexion.ConsultarDT("IF EXISTS (SELECT 1 FROM [dbo].[BDS_C_GR_DIAS_FERIADOS] where [F_FECH_DIA_FERIADO] = '" + Fecha + "') BEGIN SELECT 'Si' END ELSE BEGIN SELECT 'No' END")

        conexion.CerrarConexion()

        Dim rows As String = ""
        If data.Rows.Count > 0 Then
            rows = data.Rows(0).ItemArray(0).ToString()
        End If

        Return rows.ToString()
    End Function

End Class

