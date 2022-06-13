Public Class ServiciosSICOD
    Inherits System.Web.UI.Page



    <System.Web.Services.WebMethod()>
    Public Shared Function ObtenerUsuariosParaFirma(Area As Integer, EsSIE As Integer) As List(Of ListItem)
        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim Usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")
        Dim mycredentialCache As System.Net.CredentialCache = New System.Net.CredentialCache()
        Dim credentials As System.Net.NetworkCredential = New System.Net.NetworkCredential(Usuario, Password, Dominio)
        Dim proxySICOD As New WR_SICOD.ws_SICOD
        proxySICOD.Credentials = credentials
        proxySICOD.ConnectionGroupName = "SEPRIS"


        Dim usuarios As WR_SICOD.Diccionario() = proxySICOD.GetUsuariosFirmaOficios(Area, CBool(EsSIE))

        Dim firmas As New List(Of ListItem)()

        For Each firma As WR_SICOD.Diccionario In usuarios
            firmas.Add(New ListItem() With {
                          .Value = firma.Key,
                          .Text = firma.Value})

        Next

        Return firmas

    End Function


    '<System.Web.Services.WebMethod()>
    'Public Shared Function RegistarOficio(Folio As Integer, IdCatDocumento As Integer, IdDocumento As Integer, Firmas As String, Rubricas As String, Destinatario As String, Puesto As Integer, EsSIE As Integer, Asunto As String, Comentarios As String, UsuarioElaboro As String, Area As Integer, Proceso As Integer) As String


    '    Dim enc As New YourCompany.Utils.Encryption.Encryption64
    '    Dim Usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodUser")
    '    Dim Password As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
    '    Dim Dominio As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")
    '    Dim mycredentialCache As System.Net.CredentialCache = New System.Net.CredentialCache()
    '    Dim credentials As System.Net.NetworkCredential = New System.Net.NetworkCredential(Usuario, Password, Dominio)
    '    Dim proxySICOD As New WR_SICOD.ws_SICOD
    '    proxySICOD.Credentials = credentials
    '    proxySICOD.ConnectionGroupName = "SEPRIS"


    '    Dim TipoEntidad As Integer = -1
    '    Dim Entidad As Integer = -1
    '    Dim Subentidad As Integer = -1
    '    Dim TipoDocumento As Integer = 1
    '    'Dim dtCodigoArea As DataTable = proxySICOD.GetUnidadAdministrativaUsuario(UsuarioElaboro, WR_SICOD.UnidadAdministrativaTipo.Funcional)
    '    Dim dtCodigoArea As DataSet = proxySICOD.GetUnidadesAdministrativas(WR_SICOD.UnidadAdministrativaTipo.Funcional, WR_SICOD.UnidadAdministrativaEstatus.Activo)
    '    Dim carea As String
    '    Dim CodigoArea As Integer
    '    Dim cuenta As Integer = 0
    '    Dim DSCArea As String
    '    Dim IDclasificacion As Integer


    '    For Each row As Data.DataRow In dtCodigoArea.Tables(0).Rows
    '        carea = dtCodigoArea.Tables(0).Rows(cuenta)("ID_UNIDAD_ADM".ToString)
    '        If CInt(carea) = Area Then
    '            DSCArea = dtCodigoArea.Tables(0).Rows(cuenta)("DSC_CODIGO_UNIDAD_ADM".ToString())
    '            CodigoArea = CInt(DSCArea.Substring(0, 3))
    '        End If
    '        cuenta = cuenta + 1
    '    Next


    '    'If CodigoArea.ToString().Substring(0, 1) = 2 Then
    '    '    'DIRECCIÓN GENERAL DE SUPERVISIÓN OPERATIVA
    '    '    CodigoArea = 220
    '    'End If

    '    'If CodigoArea.ToString().Substring(0, 1) = 3 Then
    '    '    'DIRECCIÓN GENERAL DE SUPERVISIÓN FINANCIERA
    '    '    'CodigoArea = 320  se comenta ya que indican debe ser área del usuario que elabora
    '    '    'CodigoArea = CodigoArea
    '    '    CodigoArea = 300  'Prueba de asignación área provisional
    '    'End If


    '    If Proceso = 1 Then
    '        'PC
    '        Dim PC As New Entities.PC(Folio)
    '        Entidad = PC.IdEntidad
    '        TipoEntidad = 1

    '        'agrega clasificación de documento de acuerdo a SICOD
    '        Select Case IdCatDocumento
    '            Case 2
    '                IDclasificacion = 74 'Requerimiento de información
    '            Case 3
    '                IDclasificacion = 3 'Programa de Corrección -  No Procede
    '            Case 5
    '                IDclasificacion = 56 'Programas de Corrección- No existe irregularidad
    '            Case 7
    '                IDclasificacion = 112 'Programa de corrección - con condición suspensiva
    '            Case 10
    '                IDclasificacion = 2 'Programa de Corrección -  Procede
    '            Case 14
    '                IDclasificacion = 21 'Dictamen de Posible Incumplimiento
    '            Case Else
    '                IDclasificacion = 84 'otros
    '        End Select

    '        'Si se trata del dictamen
    '        If IdCatDocumento = 14 Then
    '            TipoDocumento = 2
    '        End If


    '    ElseIf Proceso = 2 Then
    '        'OPI
    '        Dim OPI As New Registro_OPI
    '        Dim OPIDetalle As OPI_Incumplimiento = OPI.GetOPIDetail(Folio)
    '        Entidad = OPIDetalle.I_ID_ENTIDAD
    '        Subentidad = OPIDetalle.I_ID_SUBENTIDAD_SB 'se asigna valor de la bd
    '        TipoEntidad = OPIDetalle.I_ID_TIPO_ENTIDAD 'se asigna valor de la bd

    '        'agrega clasificación de documento de acuerdo a SICOD
    '        Select Case IdCatDocumento
    '            Case 1
    '                IDclasificacion = 116 'Aviso de conocimiento
    '            Case 2, 4, 14
    '                IDclasificacion = 74 'Requerimiento de información
    '            Case 6, 110
    '                IDclasificacion = 110 'Autorización de Prórroga
    '            Case 7
    '                IDclasificacion = 1 'Oficio de Vigilancia - Posible Incumplimiento
    '            Case 10
    '                IDclasificacion = 4 'Requerimiento de información - Posible Incumplimiento
    '            Case 11
    '                IDclasificacion = 109 'OPI cierre - confirma y desvirtúa irregularidad
    '            Case 13
    '                IDclasificacion = 21 'Dictamen de Posible Incumplimiento
    '            Case Else
    '                IDclasificacion = 84 'otros
    '        End Select

    '        'Si se trata del dicatamen
    '        If IdCatDocumento = 13 Then
    '            TipoDocumento = 2
    '        End If

    '        If OPIDetalle.I_ID_ESTATUS = 42 Then
    '            TipoDocumento = 2
    '        End If

    '    End If

    '    Dim vecTipoEntidades As New List(Of Integer)
    '    vecTipoEntidades.Add(1)
    '    vecTipoEntidades.Add(7)
    '    vecTipoEntidades.Add(2)
    '    vecTipoEntidades.Add(3)
    '    vecTipoEntidades.Add(4)
    '    vecTipoEntidades.Add(17)


    '    '20012020 - Se comenta ya que el valor del tipo de entidad se toma de la tabla de registro del OPI
    '    ' al dejarlo así siempre asignaba un tipo entidad 2 uan cuando era 1
    '    'For Each ent As Integer In vecTipoEntidades
    '    '    Dim dv As DataView = proxySICOD.GetEntidadesComplete(ent).Tables(0).DefaultView
    '    '    dv.RowFilter = "CVE_ID_ENT=" + Entidad.ToString()
    '    '    If dv.Count > 0 Then
    '    '        TipoEntidad = dv(0)("ID_T_ENT")
    '    '    End If
    '    'Next

    '    If (Rubricas = "") Then
    '        Rubricas = Firmas
    '    End If

    '    Dim respuesta As DataSet = Nothing

    '    If TipoDocumento = 1 Then
    '        respuesta = proxySICOD.AltaOficioConIdSistema(Nothing,
    '                                                    CodigoArea,
    '                                                    TipoDocumento,
    '                                                    Now.Year,
    '                                                    Firmas.Split(",")(0),
    '                                                    UsuarioElaboro,
    '                                                    "SUPERVISAR PC",
    '                                                    TipoEntidad,
    '                                                    Entidad,
    '                                                    Subentidad,
    '                                                    0,
    '                                                    IDclasificacion,
    '                                                    1,
    '                                                    3,
    '                                                    Now.ToString("yyyyMMdd"),
    '                                                    UsuarioElaboro,
    '                                                    0,
    '                                                    Puesto,
    '                                                    Asunto,
    '                                                    Comentarios,
    '                                                    Nothing,
    '                                                    0,
    '                                                    Nothing,
    '                                                    Nothing,
    '                                                    Nothing,
    '                                                    Nothing,
    '                                                    Firmas.TrimEnd(","),
    '                                                    Rubricas.TrimEnd(","),
    '                                                    EsSIE,
    '                                                    Destinatario,
    '                                                    6)
    '    ElseIf TipoDocumento = 2 Then
    '        'Para registro de dictamen
    '        respuesta = proxySICOD.AltaOficioConIdSistemaDictamen(Nothing,
    '                                                    CodigoArea,
    '                                                    TipoDocumento,
    '                                                    Now.Year,
    '                                                    Firmas.Split(",")(0),
    '                                                    UsuarioElaboro,
    '                                                    "SUPERVISAR PC",
    '                                                    16,'TipoEntidad, 
    '                                                    4, 'Entidad,
    '                                                    0,'Subentidad,
    '                                                    0,
    '                                                    21,
    '                                                    1,
    '                                                    3,
    '                                                    Now.ToString("yyyyMMdd"),
    '                                                    UsuarioElaboro,
    '                                                    0,
    '                                                    Puesto,
    '                                                    Asunto,
    '                                                    Comentarios,
    '                                                    Nothing,
    '                                                    0,
    '                                                    Nothing,
    '                                                    Nothing,
    '                                                    Nothing,
    '                                                    Nothing,
    '                                                    Firmas.TrimEnd(","),
    '                                                    Rubricas.TrimEnd(","),
    '                                                    0,
    '                                                    Destinatario,
    '                                                    6)

    '    End If




    '    If Proceso = 1 Then
    '        'PC
    '        Dim documento As New Entities.DocumentoPC
    '        documento.Folio = Folio
    '        documento.IdDocumento = IdDocumento
    '        documento.FolioSICOD = respuesta.Tables(0).Rows(0)(0)
    '        'documento.FechaEstimada = "s"
    '        documento.ActualizarFolioSICOD()

    '        'Requerimiento de información adicional
    '        If IdCatDocumento = 2 Then
    '            Entities.RequerimientoPC.ActualizarFolioSICOD(New List(Of String)({"T_FOLIO_SICOD"}), New List(Of Object)({respuesta.Tables(0).Rows(0)(0)}), Folio)
    '        End If

    '    ElseIf Proceso = 2 Then
    '        'OPI
    '        Dim documento As New Entities.DocumentoOPI
    '        documento.Folio = Folio
    '        documento.IdDocumento = IdDocumento
    '        documento.FolioSICOD = respuesta.Tables(0).Rows(0)(0)
    '        documento.ActualizarFolioSICOD()

    '    End If

    '    Return respuesta.Tables(0).Rows(0)(0).ToString().Replace("/", "-")

    'End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function RegistarOficio(Folio As Integer, IdCatDocumento As Integer, IdDocumento As Integer, Firmas As String, Rubricas As String, Destinatario As String, Puesto As Integer, EsSIE As Integer, Asunto As String, Comentarios As String, UsuarioElaboro As String, Area As Integer, Proceso As Integer) As String


        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim Usuario As String = System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(System.Web.Configuration.WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")
        Dim mycredentialCache As System.Net.CredentialCache = New System.Net.CredentialCache()
        Dim credentials As System.Net.NetworkCredential = New System.Net.NetworkCredential(Usuario, Password, Dominio)
        Dim proxySICOD As New WR_SICOD.ws_SICOD
        proxySICOD.Credentials = credentials
        proxySICOD.ConnectionGroupName = "SEPRIS"


        Dim TipoEntidad As Integer = -1
        Dim Entidad As Integer = -1
        Dim Subentidad As Integer = -1
        Dim TipoDocumento As Integer = 1
        'Dim dtCodigoArea As DataTable = proxySICOD.GetUnidadAdministrativaUsuario(UsuarioElaboro, WR_SICOD.UnidadAdministrativaTipo.Funcional)
        Dim dtCodigoArea As DataSet = proxySICOD.GetUnidadesAdministrativas(WR_SICOD.UnidadAdministrativaTipo.Funcional, WR_SICOD.UnidadAdministrativaEstatus.Activo)
        Dim carea As String
        Dim CodigoArea As Integer
        Dim cuenta As Integer = 0
        Dim DSCArea As String
        Dim IDclasificacion As Integer


        For Each row As Data.DataRow In dtCodigoArea.Tables(0).Rows
            carea = dtCodigoArea.Tables(0).Rows(cuenta)("ID_UNIDAD_ADM".ToString)
            If CInt(carea) = Area Then
                DSCArea = dtCodigoArea.Tables(0).Rows(cuenta)("DSC_CODIGO_UNIDAD_ADM".ToString())
                CodigoArea = CInt(DSCArea.Substring(0, 3))
            End If
            cuenta = cuenta + 1
        Next


        'If CodigoArea.ToString().Substring(0, 1) = 2 Then
        '    'DIRECCIÓN GENERAL DE SUPERVISIÓN OPERATIVA
        '    CodigoArea = 220
        'End If

        'If CodigoArea.ToString().Substring(0, 1) = 3 Then
        '    'DIRECCIÓN GENERAL DE SUPERVISIÓN FINANCIERA
        '    'CodigoArea = 320  se comenta ya que indican debe ser área del usuario que elabora
        '    'CodigoArea = CodigoArea
        '    CodigoArea = 300  'Prueba de asignación área provisional
        'End If


        If Proceso = 1 Then
            'PC
            Dim PC As New Entities.PC(Folio)
            Entidad = PC.IdEntidad
            TipoEntidad = 1

            'agrega clasificación de documento de acuerdo a SICOD
            Select Case IdCatDocumento
                Case 2
                    IDclasificacion = 74 'Requerimiento de información
                Case 3
                    IDclasificacion = 3 'Programa de Corrección -  No Procede
                Case 5
                    IDclasificacion = 56 'Programas de Corrección- No existe irregularidad
                Case 7
                    IDclasificacion = 112 'Programa de corrección - con condición suspensiva
                Case 10
                    IDclasificacion = 2 'Programa de Corrección -  Procede
                Case 14
                    IDclasificacion = 21 'Dictamen de Posible Incumplimiento
                Case Else
                    IDclasificacion = 84 'otros
            End Select

            'Si se trata del dictamen
            If IdCatDocumento = 14 Then
                TipoDocumento = 2
            End If


        ElseIf Proceso = 2 Then
            'OPI
            Dim OPI As New Registro_OPI
            Dim OPIDetalle As OPI_Incumplimiento = OPI.GetOPIDetail(Folio)
            Entidad = OPIDetalle.I_ID_ENTIDAD
            Subentidad = OPIDetalle.I_ID_SUBENTIDAD_SB 'se asigna valor de la bd
            TipoEntidad = OPIDetalle.I_ID_TIPO_ENTIDAD 'se asigna valor de la bd

            'agrega clasificación de documento de acuerdo a SICOD
            Select Case IdCatDocumento
                Case 1
                    IDclasificacion = 116 'Aviso de conocimiento
                Case 2, 4, 14
                    IDclasificacion = 74 'Requerimiento de información
                Case 6, 110
                    IDclasificacion = 110 'Autorización de Prórroga
                Case 7
                    IDclasificacion = 1 'Oficio de Vigilancia - Posible Incumplimiento
                Case 10
                    IDclasificacion = 4 'Requerimiento de información - Posible Incumplimiento
                Case 11
                    IDclasificacion = 109 'OPI cierre - confirma y desvirtúa irregularidad
                Case 13
                    IDclasificacion = 21 'Dictamen de Posible Incumplimiento
                Case Else
                    IDclasificacion = 84 'otros
            End Select

            'Si se trata del dicatamen
            If IdCatDocumento = 13 Then
                TipoDocumento = 2
            End If

            If OPIDetalle.I_ID_ESTATUS = 42 Then
                TipoDocumento = 2
            End If

        End If

        Dim vecTipoEntidades As New List(Of Integer)
        vecTipoEntidades.Add(1)
        vecTipoEntidades.Add(7)
        vecTipoEntidades.Add(2)
        vecTipoEntidades.Add(3)
        vecTipoEntidades.Add(4)
        vecTipoEntidades.Add(17)


        '20012020 - Se comenta ya que el valor del tipo de entidad se toma de la tabla de registro del OPI
        ' al dejarlo así siempre asignaba un tipo entidad 2 uan cuando era 1
        'For Each ent As Integer In vecTipoEntidades
        '    Dim dv As DataView = proxySICOD.GetEntidadesComplete(ent).Tables(0).DefaultView
        '    dv.RowFilter = "CVE_ID_ENT=" + Entidad.ToString()
        '    If dv.Count > 0 Then
        '        TipoEntidad = dv(0)("ID_T_ENT")
        '    End If
        'Next

        If (Rubricas = "") Then
            Rubricas = Firmas
        End If

        Dim respuesta As DataSet = Nothing

        If TipoDocumento = 1 Then
            respuesta = proxySICOD.AltaOficioConIdSistema(Nothing,
                                                        CodigoArea,
                                                        TipoDocumento,
                                                        Now.Year,
                                                        Firmas.Split(",")(0),
                                                        UsuarioElaboro,
                                                        "SUPERVISAR PC",
                                                        TipoEntidad,
                                                        Entidad,
                                                        Subentidad,
                                                        0,
                                                        IDclasificacion,
                                                        1,
                                                        3,
                                                        Now.ToString("yyyyMMdd"),
                                                        UsuarioElaboro,
                                                        0,
                                                        Puesto,
                                                        Asunto,
                                                        Comentarios,
                                                        Nothing,
                                                        0,
                                                        Nothing,
                                                        Nothing,
                                                        Nothing,
                                                        Nothing,
                                                        Firmas.TrimEnd(","),
                                                        Rubricas.TrimEnd(","),
                                                        EsSIE,
                                                        Destinatario,
                                                        6)
        ElseIf TipoDocumento = 2 Then
            'Para registro de dictamen
            respuesta = proxySICOD.AltaOficioConIdSistemaDictamen(Nothing,
                                                        CodigoArea,
                                                        TipoDocumento,
                                                        Now.Year,
                                                        Firmas.Split(",")(0),
                                                        UsuarioElaboro,
                                                        "SUPERVISAR PC",
                                                        16,'TipoEntidad, 
                                                        4, 'Entidad,
                                                        0,'Subentidad,
                                                        0,
                                                        21,
                                                        1,
                                                        3,
                                                        Now.ToString("yyyyMMdd"),
                                                        UsuarioElaboro,
                                                        0,
                                                        Puesto,
                                                        Asunto,
                                                        Comentarios,
                                                        Nothing,
                                                        0,
                                                        Nothing,
                                                        Nothing,
                                                        Nothing,
                                                        Nothing,
                                                        Firmas.TrimEnd(","),
                                                        Rubricas.TrimEnd(","),
                                                        0,
                                                        Destinatario,
                                                        6)

        End If




        If Proceso = 1 Then
            'PC
            Dim documento As New Entities.DocumentoPC
            documento.Folio = Folio
            documento.IdDocumento = IdDocumento
            documento.FolioSICOD = respuesta.Tables(0).Rows(0)(0)
            'documento.FechaEstimada = "s"
            documento.ActualizarFolioSICOD()

            'Requerimiento de información adicional
            If IdCatDocumento = 2 Then
                Entities.RequerimientoPC.ActualizarFolioSICOD(New List(Of String)({"T_FOLIO_SICOD"}), New List(Of Object)({respuesta.Tables(0).Rows(0)(0)}), Folio)
            End If

        ElseIf Proceso = 2 Then
            'OPI
            Dim documento As New Entities.DocumentoOPI
            documento.Folio = Folio
            documento.IdDocumento = IdDocumento
            documento.FolioSICOD = respuesta.Tables(0).Rows(0)(0)
            documento.ActualizarFolioSICOD()

        End If

        Return respuesta.Tables(0).Rows(0)(0).ToString().Replace("/", "-")

    End Function

    <System.Web.Services.WebMethod()>
    Public Shared Function RegistrarOficioSD(Folio As Integer, IdCatDocumento As Integer, IdDocumento As Integer, Firmas As String, Rubricas As String, Destinatario As String, Puesto As Integer, EsSIE As Integer, Asunto As String, Comentarios As String, UsuarioElaboro As String, Area As Integer, Proceso As Integer, TipoDocumentoP As Integer, Clasificacion As Integer, Paso As Integer) As String

        Dim enc As New YourCompany.Utils.Encryption.Encryption64
        Dim Usuario As String = Web.Configuration.WebConfigurationManager.AppSettings("wsSicodUser")
        Dim Password As String = enc.DecryptFromBase64String(Web.Configuration.WebConfigurationManager.AppSettings("wsSicodPwd"), "webCONSAR")
        Dim Dominio As String = enc.DecryptFromBase64String(Web.Configuration.WebConfigurationManager.AppSettings("wsSicodDomain"), "webCONSAR")
        Dim mycredentialCache As Net.CredentialCache = New Net.CredentialCache()
        Dim credentials As Net.NetworkCredential = New Net.NetworkCredential(Usuario, Password, Dominio)
        Dim proxySICOD As New WR_SICOD.ws_SICOD
        proxySICOD.Credentials = credentials
        proxySICOD.ConnectionGroupName = "SEPRIS"

        Dim TipoEntidad As Integer = -1
        Dim Entidad As Integer = -1
        Dim Subentidad As Integer = -1
        Dim dtCodigoArea As DataSet = proxySICOD.GetUnidadesAdministrativas(WR_SICOD.UnidadAdministrativaTipo.Funcional, WR_SICOD.UnidadAdministrativaEstatus.Activo)
        Dim carea As String
        Dim CodigoArea As Integer
        Dim cuenta As Integer = 0
        Dim DSCArea As String
        Dim IDclasificacion As Integer

        For Each row As Data.DataRow In dtCodigoArea.Tables(0).Rows
            carea = dtCodigoArea.Tables(0).Rows(cuenta)("ID_UNIDAD_ADM".ToString)
            If CInt(carea) = Area Then
                DSCArea = dtCodigoArea.Tables(0).Rows(cuenta)("DSC_CODIGO_UNIDAD_ADM".ToString())
                CodigoArea = CInt(DSCArea.Substring(0, 3))
            End If
            cuenta = cuenta + 1
        Next

        If Proceso = 1 OrElse Proceso = 3 Then
            'PC
            Dim PC As New Entities.PC(Folio)
            Entidad = PC.IdEntidad
            TipoEntidad = 1

            'agrega clasificación de documento de acuerdo a SICOD
            Select Case IdCatDocumento
                Case 2
                    IDclasificacion = 74 'Requerimiento de información
                Case 3
                    IDclasificacion = 3 'Programa de Corrección -  No Procede
                Case 5
                    IDclasificacion = 56 'Programas de Corrección- No existe irregularidad
                Case 7
                    IDclasificacion = 112 'Programa de corrección - con condición suspensiva
                Case 10
                    IDclasificacion = 2 'Programa de Corrección -  Procede
                Case 14
                    IDclasificacion = 21 'Dictamen de Posible Incumplimiento
                Case Else
                    IDclasificacion = 84 'otros
            End Select

            'Si se trata del dictamen
            'Se agrega comentario porque el tipo de documento  se envía como parámetro, 2020/03/12 jjvs
            'If IdCatDocumento = 14 Then
            '    TipoDocumentoP = 2         ' se elimina esta condición porque el tipoDocumento viene como parámetro
            'End If

        ElseIf Proceso = 2 Or Proceso = 4 Then
            'OPI
            Dim OPI As New Registro_OPI
            Dim OPIDetalle As OPI_Incumplimiento = OPI.GetOPIDetail(Folio)
            Entidad = OPIDetalle.I_ID_ENTIDAD
            Subentidad = OPIDetalle.I_ID_SUBENTIDAD_SB 'se asigna valor de la bd
            TipoEntidad = OPIDetalle.I_ID_TIPO_ENTIDAD 'se asigna valor de la bd

            'agrega clasificación de documento de acuerdo a SICOD
            Select Case IdCatDocumento
                Case 1
                    IDclasificacion = 116 'Aviso de conocimiento
                Case 2, 4, 14
                    IDclasificacion = 74 'Requerimiento de información
                Case 6, 110
                    IDclasificacion = 110 'Autorización de Prórroga
                Case 7
                    IDclasificacion = 1 'Oficio de Vigilancia - Posible Incumplimiento
                Case 10
                    IDclasificacion = 4 'Requerimiento de información - Posible Incumplimiento
                Case 11
                    IDclasificacion = 109 'OPI cierre - confirma y desvirtúa irregularidad
                Case 13
                    IDclasificacion = 21 'Dictamen de Posible Incumplimiento
                Case Else
                    IDclasificacion = 84 'otros
            End Select

            'Si se trata del dicatamen
            'Se agrega comentario porque el tipo de documento  se envía como parámetro 2020/03/12
            'If IdCatDocumento = 13 Then
            '    TipoDocumentoP = 2         ' se elimina esta condición porque el tipoDocumento viene como parámetro
            'End If

            'If OPIDetalle.I_ID_ESTATUS = 42 Then
            '    TipoDocumentoP = 2
            'End If

        ElseIf Proceso = 5 Or Proceso = 6 Then
            'Visitas

            Dim oVisita As Visita = AccesoBD.getDetalleVisita(Folio, Area)
            IDclasificacion = Clasificacion
            TipoEntidad = oVisita.IdTipoEntidad
            Entidad = oVisita.IdEntidad
            Subentidad = oVisita.IdSubentidad

        End If

        Dim vecTipoEntidades As New List(Of Integer)
        vecTipoEntidades.Add(1)
        vecTipoEntidades.Add(7)
        vecTipoEntidades.Add(2)
        vecTipoEntidades.Add(3)
        vecTipoEntidades.Add(4)
        vecTipoEntidades.Add(17)

        '20012020 - Se comenta ya que el valor del tipo de entidad se toma de la tabla de registro del OPI
        ' al dejarlo así siempre asignaba un tipo entidad 2 uan cuando era 1
        'For Each ent As Integer In vecTipoEntidades
        '    Dim dv As DataView = proxySICOD.GetEntidadesComplete(ent).Tables(0).DefaultView
        '    dv.RowFilter = "CVE_ID_ENT=" + Entidad.ToString()
        '    If dv.Count > 0 Then
        '        TipoEntidad = dv(0)("ID_T_ENT")
        '    End If
        'Next

        If (Rubricas = "") Then
            Rubricas = Firmas
        End If

        Dim respuesta As DataSet = Nothing

        If TipoDocumentoP = 1 Then
            respuesta = proxySICOD.AltaOficioConIdSistema(Nothing,
                                                        CodigoArea,
                                                        TipoDocumentoP,
                                                        Now.Year,
                                                        Firmas.Split(",")(0),
                                                        UsuarioElaboro,
                                                        "SUPERVISAR PC",
                                                        TipoEntidad,
                                                        Entidad,
                                                        Subentidad,
                                                        0,
                                                        IDclasificacion,
                                                        1,
                                                        3,
                                                        Now.ToString("yyyyMMdd"),
                                                        UsuarioElaboro,
                                                        0,
                                                        Puesto,
                                                        Asunto,
                                                        Comentarios,
                                                        Nothing,
                                                        0,
                                                        Nothing,
                                                        Nothing,
                                                        Nothing,
                                                        Nothing,
                                                        Firmas.TrimEnd(","),
                                                        Rubricas.TrimEnd(","),
                                                        EsSIE,
                                                        Destinatario,
                                                        6)

        ElseIf TipoDocumentoP = 3 OrElse TipoDocumentoP = 4 Then
            respuesta = proxySICOD.AltaOficioConIdSistema(Nothing,
                                                        CodigoArea,
                                                        TipoDocumentoP,
                                                        Now.Year,
                                                        Firmas.Split(",")(0),
                                                        UsuarioElaboro,
                                                        "SUPERVISAR PC",
                                                        16,'TipoEntidad, 
                                                        4, 'Entidad,
                                                        0,'Subentidad,
                                                        0,
                                                        IDclasificacion,
                                                        1,
                                                        3,
                                                        Now.ToString("yyyyMMdd"),
                                                        UsuarioElaboro,
                                                        0,
                                                        Puesto,
                                                        Asunto,
                                                        Comentarios,
                                                        Nothing,
                                                        0,
                                                        Nothing,
                                                        Nothing,
                                                        Nothing,
                                                        Nothing,
                                                        Firmas.TrimEnd(","),
                                                        Rubricas.TrimEnd(","),
                                                        EsSIE,
                                                        Destinatario,
                                                        6)
        ElseIf TipoDocumentoP = 2 Then
            'Para registro de dictamen
            respuesta = proxySICOD.AltaOficioConIdSistemaDictamen(Nothing,
                                                        CodigoArea,
                                                        TipoDocumentoP,
                                                        Now.Year,
                                                        Firmas.Split(",")(0),
                                                        UsuarioElaboro,
                                                        "SUPERVISAR PC",
                                                        16,'TipoEntidad, 
                                                        4, 'Entidad,
                                                        0,'Subentidad,
                                                        0,
                                                        21,
                                                        1,
                                                        3,
                                                        Now.ToString("yyyyMMdd"),
                                                        UsuarioElaboro,
                                                        0,
                                                        Puesto,
                                                        Asunto,
                                                        Comentarios,
                                                        Nothing,
                                                        0,
                                                        Nothing,
                                                        Nothing,
                                                        Nothing,
                                                        Nothing,
                                                        Firmas.TrimEnd(","),
                                                        Rubricas.TrimEnd(","),
                                                        0,
                                                        Destinatario,
                                                        6)
        End If

        If Proceso = 1 Then
            'PC
            Dim documento As New Entities.DocumentoPC
            documento.Folio = Folio
            documento.IdDocumento = IdDocumento
            documento.FolioSICOD = respuesta.Tables(0).Rows(0)(0)
            documento.Usuario = UsuarioElaboro
            documento.ActualizarFolioSICOD()

            'Requerimiento de información adicional
            If IdCatDocumento = 2 Then
                Dim dtRequerimientoExistente As DataTable = Entities.RequerimientoPC.ObtenerRequerimientos(Folio)
                If dtRequerimientoExistente.Rows.Count > 0 Then
                    Entities.RequerimientoPC.ActualizarFolioSICOD(New List(Of String)({"T_FOLIO_SICOD"}), New List(Of Object)({respuesta.Tables(0).Rows(0)(0)}), Folio)
                Else
                    Dim req As New Entities.RequerimientoPC
                    req.Folio = Folio
                    req.Usuario = Usuario
                    req.Agregar()

                    Entities.RequerimientoPC.ActualizarFolioSICOD(New List(Of String)({"T_FOLIO_SICOD"}), New List(Of Object)({respuesta.Tables(0).Rows(0)(0)}), Folio)
                End If
            End If

        ElseIf Proceso = 2 Then
            'OPI
            Dim documento As New Entities.DocumentoOPI
            documento.Folio = Folio
            documento.IdDocumento = IdDocumento
            documento.FolioSICOD = respuesta.Tables(0).Rows(0)(0)
            documento.Usuario = UsuarioElaboro

            documento.ActualizarFolioSICOD()

        ElseIf Proceso = 3 Then
            Dim doc As New Entities.DocumentoPC
            doc.Folio = Folio
            doc.FolioSICOD = respuesta.Tables(0).Rows(0)(0)
            doc.IdDocumento = IdCatDocumento
            doc.NombreDocumento = ""
            doc.NombreDocumentoSh = ""
            doc.Usuario = UsuarioElaboro
            doc.AgregarLigar()

            Dim dtRequerimientoExistente As DataTable = Entities.RequerimientoPC.ObtenerRequerimientos(Folio)
            If dtRequerimientoExistente.Rows.Count = 0 Then
                Dim req As New Entities.RequerimientoPC
                req.Folio = Folio
                req.Usuario = Usuario
                req.Agregar()

                Entities.RequerimientoPC.ActualizarFolioSICOD(New List(Of String)({"T_FOLIO_SICOD"}), New List(Of Object)({respuesta.Tables(0).Rows(0)(0)}), Folio)
            End If

        ElseIf Proceso = 4 Then
            'OPI agregar
            Dim doc As New Entities.DocumentoOPI
            doc.Folio = Folio
            doc.FolioSICOD = respuesta.Tables(0).Rows(0)(0)
            doc.IdDocumento = IdCatDocumento
            doc.Prefijo = ""
            doc.NombreDocumento = ""
            doc.NombreDocumentoSh = ""
            doc.Usuario = UsuarioElaboro
            doc.AgregarLigar()

        ElseIf Proceso = 5 Then
            'Visita aactualizar
            Dim documento As New Documento
            documento.IdVisita = Folio
            documento.N_ID_DOCUMENTO = IdDocumento
            documento.FolioSICOD = respuesta.Tables(0).Rows(0)(0)
            documento.T_ID_USUARIO = UsuarioElaboro
            documento.ActualizarFolioSICOD()

        ElseIf Proceso = 6 Then
            'Visita agregar
            Dim documento As New Documento
            documento.IdVisita = Folio
            documento.N_ID_DOCUMENTO = IdCatDocumento
            documento.FolioSICOD = respuesta.Tables(0).Rows(0)(0)
            documento.T_NOM_DOCUMENTO = respuesta.Tables(0).Rows(0)(0) '""
            documento.T_NOM_DOCUMENTO_ORI = respuesta.Tables(0).Rows(0)(0)
            documento.T_ID_USUARIO = UsuarioElaboro
            documento.N_ID_VERSION = 1
            documento.I_ID_PASO = Paso
            documento.AgregarLigar()

        End If

        Return respuesta.Tables(0).Rows(0)(0).ToString().Replace("/", "-")

    End Function

End Class