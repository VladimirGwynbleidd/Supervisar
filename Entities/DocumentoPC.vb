Public Class DocumentoPC



    Property Folio As Integer
    Property FolioSICOD As String
    Property IdDocumento As Integer
    Property NombreDocumento As String
    Property NombreDocumentoSh As String
    Property FechaEstimada As String
    Property Usuario As String


    Public Shared Function ObtenerTodos() As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim query As String


        query = "SELECT I_ID_DOCUMENTO, T_NOM_DOCUMENTO, I_PASO_INICIAL, I_PASO_FINAL, B_BUSCAR_SICOD, B_REG_SICOD, N_FLAG_SICOD, T_OFICIO_SICOD FROM BDS_C_PC_DOCUMENTOS WHERE I_FLAG_ACTIVO = 1"

        data = conexion.ConsultarDT(query)

        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function ObtenerUno(I_ID_DOCUMENTO As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim query As String


        query = "SELECT I_ID_DOCUMENTO, T_PREFIJO, T_NOM_DOCUMENTO, I_PASO_INICIAL, I_PASO_FINAL, B_BUSCAR_SICOD, B_REG_SICOD FROM BDS_C_PC_DOCUMENTOS WHERE I_ID_DOCUMENTO = " + I_ID_DOCUMENTO.ToString

        data = conexion.ConsultarDT(query)

        conexion.CerrarConexion()

        Return data

    End Function

    Public Shared Function PasoValido(Folio As Integer, Paso As Integer) As Boolean
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim query As String


        query = "SELECT * FROM dbo.BDS_C_PC_DOCUMENTOS C LEFT JOIN dbo.BDS_R_PC_DOCUMENTOS R ON C.I_ID_DOCUMENTO = R.I_ID_DOCUMENTO AND R.N_ID_FOLIO = " + Folio.ToString() + " WHERE R.I_ID IS NULL AND C.I_PASO_INICIAL = " + Paso.ToString()
        data = conexion.ConsultarDT(query)
        conexion.CerrarConexion()

        If data.Rows.Count = 0 Then
            Return True

        Else
            Return False
        End If
    End Function

    Public Shared Function OficioConcluido(Oficio As String) As Boolean
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim query As String


        query = ""
        data = conexion.ConsultarDT(query)
        conexion.CerrarConexion()

        If data.Rows.Count = 0 Then
            Return True

        Else
            Return False
        End If
    End Function


    Public Shared Function ObtenerArchivos(Folio As Integer, IdDocumento As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim data As DataTable
        Dim query As String


        query = "SELECT * FROM BDS_R_PC_DOCUMENTOS WHERE N_ID_FOLIO=" + Folio.ToString + " AND I_ID_DOCUMENTO=" + IdDocumento.ToString()

        data = conexion.ConsultarDT(query)

        conexion.CerrarConexion()

        Return data
    End Function
    Public Sub Agregar()
        Dim conexion As New Conexion.SQLServer()
        Dim campos As New List(Of String)
        Dim valores As New List(Of Object)
        campos.Add("N_ID_FOLIO")
        campos.Add("I_ID_DOCUMENTO")
        campos.Add("T_DSC_NOMBRE_DOCUMENTO")
        campos.Add("T_DSC_NOMBRE_DOCUMENTO_SH")
        campos.Add("F_FECH_REGISTRO")
        campos.Add("T_ID_USUARIO_REGISTRO")

        valores.Add(Folio)
        valores.Add(IdDocumento)
        valores.Add(NombreDocumento)
        valores.Add(NombreDocumentoSh)
        valores.Add(Date.Now)
        valores.Add(Usuario)

        conexion.Insertar("BDS_R_PC_DOCUMENTOS", campos, valores)

        conexion.CerrarConexion()

    End Sub
    Public Sub AgregarLigar()
        Dim conexion As New Conexion.SQLServer()
        Dim campos As New List(Of String)
        Dim valores As New List(Of Object)
        campos.Add("N_ID_FOLIO")
        campos.Add("I_ID_DOCUMENTO")
        campos.Add("T_FOLIO_SICOD")
        campos.Add("T_DSC_NOMBRE_DOCUMENTO")
        campos.Add("T_DSC_NOMBRE_DOCUMENTO_SH")
        campos.Add("F_FECH_REGISTRO")
        campos.Add("T_ID_USUARIO_REGISTRO")

        valores.Add(Folio)
        valores.Add(IdDocumento)
        valores.Add(FolioSICOD)
        valores.Add(NombreDocumento)
        valores.Add(NombreDocumentoSh)
        valores.Add(Date.Now)
        valores.Add(Usuario)

        conexion.Insertar("BDS_R_PC_DOCUMENTOS", campos, valores)

        conexion.CerrarConexion()

    End Sub
    Public Sub Actualizar()
        Dim conexion As New Conexion.SQLServer()
        Dim campos As New List(Of String)
        Dim valores As New List(Of Object)
        Dim camposCondicion As New List(Of String)
        Dim valoresCondicion As New List(Of Object)

        campos.Add("T_DSC_NOMBRE_DOCUMENTO")
        valores.Add(NombreDocumento)

        campos.Add("T_DSC_NOMBRE_DOCUMENTO_SH")
        valores.Add(NombreDocumentoSh)

        campos.Add("F_FECH_REGISTRO")
        valores.Add(Date.Now)

        campos.Add("T_ID_USUARIO_REGISTRO")
        valores.Add(Usuario)


        camposCondicion.Add("N_ID_FOLIO")
        camposCondicion.Add("I_ID")

        valoresCondicion.Add(Folio)
        valoresCondicion.Add(IdDocumento)

        conexion.Actualizar("BDS_R_PC_DOCUMENTOS", campos, valores, camposCondicion, valoresCondicion)

        conexion.CerrarConexion()

    End Sub

    Public Sub ActualizarFolioSICOD()
        Dim conexion As New Conexion.SQLServer()
        Dim campos As New List(Of String)
        Dim valores As New List(Of Object)
        Dim camposCondicion As New List(Of String)
        Dim valoresCondicion As New List(Of Object)

        campos.Add("T_FOLIO_SICOD")
        valores.Add(FolioSICOD)

        camposCondicion.Add("N_ID_FOLIO")
        camposCondicion.Add("I_ID")

        valoresCondicion.Add(Folio)
        valoresCondicion.Add(IdDocumento)

        conexion.Actualizar("BDS_R_PC_DOCUMENTOS", campos, valores, camposCondicion, valoresCondicion)

        conexion.CerrarConexion()

    End Sub

    Public Shared Function ObtenerClasificacion(I_ID_DOCUMENTO As Integer) As DataTable
        Dim conexion As New Conexion.SQLServer()
        Dim Clasificacion As DataTable
        Dim query As String


        query = "SELECT I_ID_DOCUMENTO, T_NOM_DOCUMENTO, I_PASO_INICIAL, I_PASO_FINAL, T_DSC_CLASIFICACION, I_ID_CLASIFICACION FROM BDS_C_PC_DOCUMENTOS WHERE I_ID_DOCUMENTO = " + I_ID_DOCUMENTO.ToString

        Clasificacion = conexion.ConsultarDT(query)

        conexion.CerrarConexion()

        Return Clasificacion

    End Function

End Class
