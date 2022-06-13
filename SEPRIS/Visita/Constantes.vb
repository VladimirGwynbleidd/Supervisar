Public Class Constantes

   Public Const ETAPA_1_PASO_INI As Integer = 1
   Public Const ETAPA_1_PASO_FIN As Integer = 9
   Public Const ETAPA_2_PASO_INI As Integer = 10
   Public Const ETAPA_2_PASO_FIN As Integer = 18
   Public Const ETAPA_3_PASO_INI As Integer = 19
   Public Const ETAPA_3_PASO_FIN As Integer = 27
   Public Const ETAPA_4_PASO_INI As Integer = 28
   Public Const ETAPA_4_PASO_FIN As Integer = 37

   Public Const CODIGO_AREA_VO_INI As Integer = 200
   Public Const CODIGO_AREA_VO_FIN As Integer = 299

   Public Const CODIGO_AREA_VF_INI As Integer = 300
   Public Const CODIGO_AREA_VF_FIN As Integer = 399

   Public Const CODIGO_AREA_VJ_INI As Integer = 400
   Public Const CODIGO_AREA_VJ_FIN As Integer = 499

   Public Const AREA_CGIV As Integer = 1
   Public Const AREA_PR As Integer = 34
   Public Const AREA_VO As Integer = 35
   Public Const AREA_VF As Integer = 36
   Public Const AREA_VJ As Integer = 37
   Public Const AREA_SN As Integer = 38

   Public Const imgWarning As String = "1Error.png"
   Public Const imgError As String = "2Error.png"
   Public Const imgOk As String = "3Error.png"

   Public Const PERFIL_ADM As Integer = 1
   Public Const PERFIL_SUP As Integer = 2
   Public Const PERFIL_INS As Integer = 3
   Public Const PERFIL_ABO As Integer = 4
   Public Const PERFIL_SOLO_LEC As Integer = 8
    Public Const PERFIL_SOLO_CARGA As Integer = 9
    Public Const ESTATUS As Integer = 49
    Public Const CANCELAR As Integer = 48



    Enum ABOGADOS
      PERFIL_ABO_ASESOR = 5
      PERFIL_ABO_CONTENCIOSO = 7
      PERFIL_ABO_SANCIONES = 6
   End Enum

   Enum ResponsablesVisita
      Supervisor = 2
      Inspector = 3
      AbogadoSupAsesor = 25
      Asesor = 5
      AbogadoSupSancion = 26
      Sanciones = 6
      AbogadoSupContecioso = 27
      Contencioso = 7
   End Enum

   Enum OPERCION
      Insertar = 0
      Actualizar = 1
   End Enum

   Public Const CORREO_ID_NOTIFICA_VISITA_CREADA As Integer = 1
   Public Const CORREO_ID_NOTIFICA_REUNION_VULNERA_REALIZADA As Integer = 0
   Public Const CORREO_ID_NOTIFICA_REGISTRO As Integer = 1
   Public Const CORREO_ID_NOTIFICA_CANCELACION As Integer = 2
   Public Const CORREO_ID_NOTIFICA_VJ_REVISAR_OF_COM_ACT_INI As Integer = 3
   Public Const CORREO_NOTIFICA_ABOGADO_ASESOR As Integer = 4
   Public Const CORREO_ID_NOTIFICA_AREA_REGISTRO_VISITA_RECHAZADO As Integer = 5
   Public Const CORREO_ID_NOTIFICA_AREA_REGISTRO_VISITA_APROBADO As Integer = 6
   Public Const CORREO_ID_NOTIFICA_SUPERVISOR_DOCS_INICIO_MODIFICADOS As Integer = 7
   Public Const CORREO_ID_NOTIFICA_ABOGADO_Y_PRESIDENCIA_VISITA_INICIA As Integer = 8
   'Public Const CORREO_ID_NOTIFICA_FECHA_ACUSE_NOTIFICACION As Integer = 9
   Public Const CORREO_ID_NOTIFICA_NO_RESPUESTA_AFORE_OFICIOS_INICIO As Integer = 9
   'Public Const CORREO_ID_NOTIFICA_RESPUESTA_AFORE_OFICIOS_INICIO As Integer = 10
   Public Const CORREO_ID_NOTIFICA_SI_RESPUESTA_AFORE_OFICIOS_INICIO As Integer = 10
   Public Const CORREO_ID_NOTIFICA_INFORMACION_HALLAZGOS As Integer = 11
   Public Const CORREO_ID_NOTIFICA_ENVIO_ACTA_CIRCUNSTANCIADA As Integer = 12
   Public Const CORREO_ID_NOTIFICA_RECHAZA_ACTA_CIRCUNSTANCIADA As Integer = 13
   Public Const CORREO_ID_NOTIFICA_SUPERVISOR_ACTA_CIRCUNSTANCIADA_MODIFICADA As Integer = 14
   Public Const CORREO_ID_NOTIFICA_APRUEBA_ACTA_CIRCUNSTANCIADA As Integer = 15
   Public Const CORREO_ID_NOTIFICA_RANGO_SANCION As Integer = 16
   Public Const CORREO_ID_NOTIFICA_VJ_PR_HA_LEVANTADO_ACTA_CIR_IN_SITU As Integer = 17
   Public Const CORREO_ID_NOTIFICA_VJ_SI_EXISTE_ACUSE_RESPUESTA_AFORE As Integer = 18
   Public Const CORREO_ID_NOTIFICA_VJ_ADJUNTA_ACTA_CONCLUSION_OF_RECOMENDACIONES As Integer = 19
   Public Const CORREO_ID_NOTIFICA_RECHAZA_ACTA_CONCLUSION As Integer = 20
   Public Const CORREO_ID_NOTIFICA_VERSION_FINAL_ACTA_CONCLUSION_OFICIO_RECOMENDACIONES As Integer = 21
   Public Const CORREO_ID_NOTIFICA_APRUEBA_ACTA_CONCLUSION As Integer = 22
   Public Const CORREO_ID_NOTIFICA_VJ_PR_HA_LEVANTADO_ACTA_CON_IN_SITU As Integer = 23
   Public Const CORREO_ID_NOTIFICA_VJ_DICTAMEN As Integer = 24
   Public Const CORREO_ID_NOTIFICA_VO_VF_REVISAR_OF_EMPLAZAMIENTO As Integer = 25
   Public Const CORREO_ID_NOTIFICA_RECHAZA_OF_EMPLAZAMIENTO As Integer = 26
   Public Const CORREO_ID_NOTIFICA_APRUEBA_OF_EMPLAZAMIENTO As Integer = 27
   Public Const CORREO_ID_NOTIFICA_FECHA_POSIBLE_EMPLAZAMIENTO As Integer = 28
   Public Const CORREO_ID_NOTIFICA_FECHA_ACUSE_AFORE_RECIBE_OF_EMPL As Integer = 29
   Public Const CORREO_ID_NOTIFICA_FECHA_ACUSE_AFORE_CONTESTO_OF_EMPL As Integer = 30
   Public Const CORREO_ID_NOTIFICA_VO_VF_HUBO_RESPUESTA_AFORE As Integer = 31
   Public Const CORREO_ID_NOTIFICA_VO_VF_PR_HUBO_SANCION_FECHA_IMPOSICION As Integer = 32
   Public Const CORREO_ID_NOTIFICA_VO_VF_PR_HUBO_SANCION_FECHA_IMPOSICION_ESTIMADA As Integer = 33
   Public Const CORREO_ID_NOTIFICA_VO_VF_PR_FECHA_ACUSE_PAGO_AFORE As Integer = 34
   Public Const CORREO_ID_NOTIFICA_VO_VF_PR_AFORE_SOLICITA_RECURSO_REVOCACION As Integer = 35
   Public Const CORREO_ID_NOTIFICA_VO_VF_PR_AFORE_SOLICITA_JUICIO_NULIDAD As Integer = 36
   Public Const CORREO_ID_NOTIFICA_VO_VF_PR_AFORE_SOLICITA_NINGUNO_DE_LOS_DOS As Integer = 37
   Public Const CORREO_ID_NOTIFICA_VO_VF_REVISAR_DOCUMENTO_RESPUESTA_SOLICITUD_AFORE As Integer = 38
   Public Const CORREO_ID_NOTIFICA_VJ_RECHAZA_DOCUMENTO_RESPUESTA_SOLICITUD_AFORE As Integer = 39
   Public Const CORREO_ID_NOTIFICA_VJ_AJUSTA_DOCUMENTO_RESPUESTA_SOLICITUD_AFORE As Integer = 40
   Public Const CORREO_ID_NOTIFICA_VJ_APRUEBA_DOCUMENTO_RESPUESTA_SOLICITUD_AFORE As Integer = 41
   Public Const CORREO_ID_NOTIFICA_VO_VF_ADJUNTA_ACUSE_ENTREGA_RESPUESTA As Integer = 42
   Public Const CORREO_ID_NOTIFICA_VO_VF_PR_RESPUESTA_ENVIO_VJ As Integer = 43
   Public Const CORREO_ID_NOTIFICA_VJ_PR_VISITA_ENTRA_EN_PRORROGA As Integer = 44
   Public Const CORREO_ATIENDE_AJUSTES As Integer = 45
   Public Const CORREO_RECHAZA_AJUSTES_A_VJ As Integer = 46
   Public Const CORREO_DOCUMENTOS_ADJUNTADOS As Integer = 47
   Public Const CORREO_FINALIZA_DIAG_HALLAZGOS As Integer = 48
   Public Const CORREO_PRESENTA_DIAG_HALLAZGOS As Integer = 49
   Public Const CORREO_INSPECTOR_REVISA_DOCUMENTOS As Integer = 50
   Public Const CORREO_VER_FINAL_ACTA_CIRCUNSTANCIADA As Integer = 51
   Public Const CORREO_SANDRA_PACHECO As Integer = 52
   Public Const CORREO_PRESENTACION_DIAGNOSTICO_HALLAZGOS As Integer = 53
   Public Const CORREO_FECHA_INICIO_VISITA As Integer = 54
   Public Const CORREO_RECHAZO_DOCUMENTOS As Integer = 55
   Public Const CORREO_VERSION_FINAL_DOCUMENTOS As Integer = 56
   Public Const CORREO_RECHAZO_COMENTARIOS As Integer = 57
   Public Const CORREO_ID_NOTIFICA_A_SUPERVISOR_PRORROGA As Integer = 58
   Public Const CORREO_ID_NOTIFICA_RECHAZO_PRORROGA As Integer = 59
   Public Const CORREO_CAMBIO_FECHA_EXTERNA_INTERNA As Integer = 60
   Public Const CORREO_CAMBIO_RESPONSABLES_VISITA As Integer = 61
   Public Const CORREO_RECHAZO_PASO_DOS As Integer = 62
   Public Const CORREO_PASO_12_VERSION_FINAL_DOCTOS As Integer = 63
   Public Const CORREO_PASO_12_RECHAZA_VERSION_FIN_DOC As Integer = 64
   Public Const CORREO_NUEVO_RANGO_SANCION As Integer = 65
   Public Const CORREO_NOTIFICA_SUPERVISOR_PASO16 As Integer = 66
   Public Const CORREO_NOTIFICA_ABOGADO_SANCIONES As Integer = 67
   Public Const CORREO_ULTIMA_VERSION_OF_SANCION As Integer = 68
   Public Const CORREO_NOTIFICA_ABOGADO_CONTENCIOSO As Integer = 69
   Public Const CORREO_CAMBIO_ABOGADOS_VISITA As Integer = 70
    Public Const CORREO_SOLICITUD_REVISION_VULNERABILIDADES As Integer = 75
    Public Const CORREO_REVISION_DOCUMENTOS As Integer = 78
    Public Const CORREO_RECHAZO_PASO_17 As Integer = 79
    Public Const CORREO_EDICION_VISITA As Integer = 80
    Public Const CORREO_RECHAZO_PASO_36 As Integer = 81
    Public Const CORREO_CAMBIO_NORMATIVA_P3 As Integer = 93

#Region "PASO 6 DOCUMENTOS RRA y VULNERABILIDAD"
    Public Const CORREO_DOCUMENTOS_REVISION As Integer = 71
    Public Const CORREO_DOCUMENTOS_RECHAZO As Integer = 72
    Public Const CORREO_DOCUMENTOS_APROBADOS As Integer = 73
    Public Const CORREO_DOCUMENTOS_NOTIFICAR_CORRECTO As Integer = 74

    Public Const CORREO_VULNERABILIDAD_NOTIFICAR_SANDRA As Integer = 75
    Public Const CORREO_VULNERABILIDAD_NOTIFICAR_FECHA As Integer = 76
    Public Const CORREO_VULNERABILIDAD_NOTIFICAR_CAMBIO_FECHA As Integer = 77
#End Region

    Public Const EMAIL_DESTINATARIO_DESA_1 As String = "cortescorpss@gmail.com"
    Public Const EMAIL_DESTINATARIO_DESA_2 As String = "jm.abeja.cortes@hotmail.com"
    Public Const EMAIL_DESTINATARIO_DESA_3 As String = "jmcortes@indracompany.com"
    Public Const EMAIL_DESTINATARIO_DESA_4 As String = "cortescorpss@gmail.com"

    Public Const CORREO_ENVIADO_OK As String = "Correo enviado correctamente"
    Public Enum MovimientoPaso
        RegistroVisita = 0
        CargaDocumentacionInicial = 1
        EnviaDocumentacionInicial = 2
        AsignaAbogadoSupervisor = 3
        AsignaAbogadoAsesor = 4
        RevisaDocumentosInicio = 5
        AprobacionDocumentosInicio = 6
        RechazaDocumentosInicio = 7
        RevisaComentariosDocumentosInicio = 8
        RechazaComentariosDocumentosInicio = 9
        AprobacionDocumentosInicioVersionFinal = 10
        CargaPDFDocumentosInicio = 11
        NotificaInicioVisita = 12
        ValidaResultadoAFOREeIniciaINSITU = 13
        FinalizaVisitaINSITU = 14
        GeneraActaCircunstanciada = 15
        EnviaActaCircunstanciadaRevision = 16
        RevisaActaCircunstanciada = 17
        ApruebaActaCircunstanciada = 18
        RechazaActaCircunstanciada = 19
        RevisaComentariosActaCircunstanciada = 20
        RechazaComentariosActaCircunstanciada = 21
        ApruebaComentariosActaCircunstanciada = 22
        CargaPresentacionInterna = 23
        ConfirmaFechaPresentacion = 24
        CargaPDFActaCircunstanciada = 25
        LevantamientoINSITUActaCircunstanciada = 26
        ValidaRespuestaAFORE = 27
        GeneraActaConclusion = 28
        EnviaActaConclusionRevision = 29
        RevisaActaConclusion = 30
        ApruebaActaConclusion = 31
        RechazaActaConclusion = 32
        RevisaComentariosActaConclusion = 33
        RechazaComentariosActaConclusion = 34
        ApruebaComentariosActaConclusion = 35
        CargaPDFActaConclusión = 36
        LevantamientoINSITUActaConclusión = 37
        CompletarDatosSISAN = 38
        REgistraIrregularidadSISAN = 39
    End Enum
    Public Enum EstatusPaso
        Registrado = 1
        Enviado = 2
        Asignado = 3
        Inicia_revision = 28
        En_revision = 4
        Aprobado = 5
        Modificado = 6
        Notificado = 7
        Afore_Notificado = 8
        Respuesta_Afore = 9
        Visita_iniciada = 10
        Visita_Finalizada = 11
        En_diagnostico_de_hallazgos = 12
        Hallazgos_presentados = 13
        Elaborada = 14
        Respuesta_Afore_Notificada = 15
        Impuesto = 16
        Pagado = 17
        Revocacion = 18
        Nulidad = 19
        Sin_respuesta = 20
        Respuesta_Elaborada = 21
        Respuesta_en_revisión = 22
        Respuesta_Aprobada = 23
        Respuesta_Notificada = 24
        En_Prórroga = 25
        Cancelado = 26
        Hallazgos_presentados_notificado = 27
        AjustesEnviados = 29
        AsesorAsignado = 30
        Revisado = 31
        EnRevisionEspera = 32
        Rechazado = 33
        EnAjustes = 34
        AjustesRealizados = 35
        EnEsperaPresentarHallazgos = 36
        HallazgosGuardados = 37
        SinReunionPresidencia = 38
        SupervisorAsignado = 39
        Cerrada = 0
    End Enum

    Enum TipoReactivacion
        Reactivado = 1
        FinalizaReactivacion = 2
    End Enum

    Enum CopFoliosTConsul
        DelPasoUnoAl17 = 1
        TodosLosPasos = -1
    End Enum

    Enum TipoArchivo
        WORD = 1
        PDF = 2
        LIBRE = 3
        TODOS = -1
    End Enum


    Enum Obligatorio
        Obligatorios = 1
        NoObligatorios = 2
    End Enum

    Enum AsuntoCorreo
        Paso0 = 1
        Paso8 = 2
        Paso9 = 3
        Paso10 = 4
        Paso11 = 5
        Paso12 = 6
        Paso16 = 7
        Paso18 = 8
    End Enum

    Enum TipoFecha
        FechaInicio = 1
        FechaCampoInicial = 2
        FechaCampoFinal = 3
        FechaReunionPresi = 4
        FechaReunionAfore = 5
        FechaReunionVjp9 = 6
        FechaReunionVjp16 = 7
        FechaReunionVjp9Confirmacion = 8
        FechaReunionVjp16Confirmacion = 9
        FechaReunionVjp9NoSeRealizo = 10
        FechaReunionVjp16NoSeRealizo = 11
        FechaInSituActaCircunstanciada = 12
        FechaLevantamientoActaConclusion = 13
        FechaReunionVjp25 = 14
        FechaReunionVjp32 = 15
        FechaReunionVjp25Confirmacion = 16
        FechaReunionVjp32Confirmacion = 17
        FechaReunionVjp25NoSeRealizo = 18
        FechaReunionVjp32NoSeRealizo = 19
        BanderaDeApropiacion = 20
        LimpiaBanderaDeApropiacion = 21
        BanderaDeReunionPaso8 = 22
        FechaVulneravilidades = 23
        FechaCampoInicialP7 = 24
        FechaCampoFinalP7 = 25
        FechaInicioPaso13 = 26
        FechaInicioPaso14 = 27
        FechaVoBoP18 = 28
        Fecha81 = 29
        FechaReunionVjPaso10VOBO = 100
        FechaCampoFinalP19 = 30
    End Enum

    ''' <summary>
    ''' CLASESITA QUE DEFINE UN TIPO DE COMENTARIO PARA LA TABLA DE ESTATUS
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TipoComentario
        Public Const USUARIO As String = "USUARIO"
        Public Const SISTEMA As String = "SISTEMA"
    End Class

    Public Const Verdadero As Integer = 1
    Public Const Falso As Integer = 0
    Public Const Todos As Integer = -1
    Public Const DiasPaso18 As Integer = 80
    Public Const DiasPaso19 As Integer = 80

    Enum Vigencia
        Vigente = 1
        NoVigente = 0
        NoConsideraVigencia = 2
    End Enum

    Enum Semaforo
        Verde = 1
        Amarillo = 2
        Rojo = 3
        Gris = 4
    End Enum

    Enum TipoCopia
        SubVisita = 1
        CopiaFolio = 2
    End Enum

    Enum IncremeteDecrementa
        Incrementa = 1
        Decrementa = 0
    End Enum

    Enum TipoDias
        Habiles = 0
        Naturales = 1
    End Enum

    Enum Pantalla
        Bandeja = 0
        DocumentosControl = 1
        DocumentosCat = 2
        CatalogoPerfiles = 3
        Correos = 4
        Bitacora = 5
        CatalgoMenus = 6
        CatalgoSubMenus = 7
        CatalgogoUsuarios = 8
        Formato = 9
        PendientesVisita = 10
        PendientesPaso = 11
    End Enum

    ''' <summary>
    ''' Areas que pueden registrar visitas en SEPRIS
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAreasOperativas() As List(Of Entities.Area)
        Dim lstAreas As New List(Of Entities.Area)
        lstAreas.Add(New Entities.Area With {.Identificador = 1, .Descripcion = "COORDINACION GENERAL DE INFORMACION Y VINCULACION"})
        lstAreas.Add(New Entities.Area With {.Identificador = 35, .Descripcion = "VICEPRESIDENCIA DE OPERACIONES"})
        lstAreas.Add(New Entities.Area With {.Identificador = 36, .Descripcion = "VICEPRESIDENCIA FINANCIERA"})
        lstAreas.Add(New Entities.Area With {.Identificador = 34, .Descripcion = "PRESIDENCIA"})
        lstAreas.Add(New Entities.Area With {.Identificador = AREA_PLD, .Descripcion = "VICEPRESIDENCIA JURIDICA - PLD"})

        Return lstAreas
    End Function

    Public Shared Function GetAreasSnPrec() As List(Of Entities.Area)
        Dim lstAreas As New List(Of Entities.Area)
        lstAreas.Add(New Entities.Area With {.Identificador = 1, .Descripcion = "COORDINACION GENERAL DE INFORMACION Y VINCULACION"})
        lstAreas.Add(New Entities.Area With {.Identificador = 35, .Descripcion = "VICEPRESIDENCIA DE OPERACIONES"})
        lstAreas.Add(New Entities.Area With {.Identificador = 36, .Descripcion = "VICEPRESIDENCIA FINANCIERA"})
        lstAreas.Add(New Entities.Area With {.Identificador = 37, .Descripcion = "VICEPRESIDENCIA JURIDICA"})
        lstAreas.Add(New Entities.Area With {.Identificador = AREA_PLD, .Descripcion = "VICEPRESIDENCIA JURIDICA - PLD"})

        Return lstAreas
    End Function
    ''' <summary>
    ''' Se hace un case, ademas no creo que cambien los nombre de las areas jdejejejee...
    ''' </summary>
    ''' <param name="idArea"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function BuscarNombreArea(idArea As Integer) As String
        Dim lsNomArea As String = ""

        Select Case idArea
            Case Constantes.AREA_VF
                lsNomArea = "Vicepresidencia Financiera"
            Case Constantes.AREA_VJ
                lsNomArea = "Vicepresidencia Jurídica"
            Case Constantes.AREA_VO
                lsNomArea = "Vicepresidencia de Operaciones"
            Case Constantes.AREA_PR
                lsNomArea = "Presidencia"
            Case Constantes.AREA_CGIV
                lsNomArea = "Coordinación General de Información y Vinculación"
            Case Constantes.AREA_PLD
                lsNomArea = "Vicepresidencia Jurídica - PLD"
        End Select

        Return lsNomArea
    End Function

    ''' <summary>
    ''' Retorna los pasos y estatus en donde se debe de reemplazar la imagen de aprobar doctos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetListaPasosBtnSig() As List(Of Paso)
        Dim lstPasos As New List(Of Paso)
        lstPasos.Add(New Paso With {.IdPaso = 2, .EstatusPaso = 30})
        lstPasos.Add(New Paso With {.IdPaso = 2, .EstatusPaso = 31})
        lstPasos.Add(New Paso With {.IdPaso = 2, .EstatusPaso = 34})
        lstPasos.Add(New Paso With {.IdPaso = 2, .EstatusPaso = 35})
        lstPasos.Add(New Paso With {.IdPaso = 10, .EstatusPaso = 31})
        lstPasos.Add(New Paso With {.IdPaso = 10, .EstatusPaso = 35})
        'lstPasos.Add(New Paso With {.IdPaso = 17, .EstatusPaso = 31}) 'Cambio por nuevos pasos
        lstPasos.Add(New Paso With {.IdPaso = 17, .EstatusPaso = 35})
        lstPasos.Add(New Paso With {.IdPaso = 22, .EstatusPaso = 31}) 'AMMM 17/10/2018 se cambia num de paso 21 a 22
        lstPasos.Add(New Paso With {.IdPaso = 27, .EstatusPaso = 31}) 'AMMM 17/10/2018 se agrega paso 27
        lstPasos.Add(New Paso With {.IdPaso = 29, .EstatusPaso = 31}) 'AMMM 17/10/2018 se agrega paso 29
        lstPasos.Add(New Paso With {.IdPaso = 35, .EstatusPaso = 31})
        lstPasos.Add(New Paso With {.IdPaso = 35, .EstatusPaso = 32})
        lstPasos.Add(New Paso With {.IdPaso = 35, .EstatusPaso = 34})
        lstPasos.Add(New Paso With {.IdPaso = 35, .EstatusPaso = 35})

        Return lstPasos
    End Function

    ''' <summary>
    ''' Retorna los pasos y estatus en donde se debe de reemplazar la imagen de rechazo de doctos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetListaPasosBtnRechazar() As List(Of Paso)
        Dim lstPasos As New List(Of Paso)
        lstPasos.Add(New Paso With {.IdPaso = 2, .EstatusPaso = 30})

        Return lstPasos
    End Function

    Public Class MensajesModal
        Public Const FinalizaCargaDoctos As String = "¿Deseas terminar la carga de documentos en este paso?<br /><br /><ul><li>Si tu respuesta es Sí, ya no podrás adjuntar documentos para este paso.</li> <li>Si tu respuesta es No, podrás seguir adjuntando documentos de este paso hasta el paso 18.</li></ul>"
        Public Const FechaLevantamientoInSitu As String = "Favor de ingresar la fecha en que se realizó el levantamiento in situ de Acta de Conclusión:"
        Public Const EditarFecPresentacion As String = "Si deseas modificar la fecha de la reunión para la presentación de hallazgos, por favor ingresa la nueva fecha:"
        Public Const AlertaPaso18 As String = "El plazo para adjuntar la documentación completa finalizará al avanzar al siguiente paso, ¿deseas adjuntar la documentación correspondiente a esta visita en este momento?"
        Public Const FechaPaso25 As String = "¿En qué fecha se llevará a cabo la reunión de revisión con la [AREA]?"
        Public Const FechaPaso32 As String = "¿En qué fecha se llevará a cabo la reunión de revisión con la Vicepresidencia Juridica?"
        Public Const ActaCircunstanciada As String = "Favor de ingresar la fecha de levantamiento in situ de acta circunstanciada:"
        Public Const ConfirmacionFechaVj As String = "La fecha de la reunión con Vicepresidencia Jurídica fue: "
        Public Const ConfirmacionFechaVjPaso27 As String = "La fecha de la reunión con el área de inspección para revisar el oficio de sanción fue: "
        Public Const SolicitudSancionPasoDiez As String = "Es necesario que se estime el rango de la sanción antes del paso 17.<br/><br/>¿Deseas ingresarlo en este momento?"
        Public Const ConfirmacionFechaVjPaso8 As String = "¿Se llevó a cabo la reunión opcional  8.1 para revisar la presentación de hallazgos?"
        Public Const FechaReunionRevisioPresentacionHallazgos As String = "¿En qué fecha será la reunión con el área de inspección <br/><br/>para revisión de acta circunstanciada?"
        Public Const FechaReunionRevisioPresentacionHallazgosV17 As String = "¿En qué fecha será la reunión opcional 9.1 - Revisión de acta circunstanciada?"
        Public Const FechaRetroalimentacion As String = "¿En que fecha se llevará a cabo la reunión opcional 9.1 - Retroalimentación de Acta circunstanciada?:"
        Public Const FechaRetroalimentacionPasado As String = "¿En que fecha se llevó a cabo la reunión opcional 9.1 - Retroalimentación de Acta circunstanciada?:"
        Public Const FechaRetroalimentacionP13 As String = "¿En que fecha se llevará a cabo la reunión opcional 13.1 - Retroalimentación de Acta circunstanciada?:"
        Public Const FechaRetroalimentacionP13Pasado As String = "¿En que fecha se llevó a cabo la reunión opcional 13.1 - Retroalimentación de Acta circunstanciada?:"
        Public Const ConfirmaFechaRetroalimentacion As String = "La fecha de la reunión opcional  9.1 - Retroalimentación de Acta circunstanciada fue: " +
                                                          "<ul><li>Si: Estas de acuerdo que  la reunión se realizó en la fecha mostrada.</li>" +
                                                          "<li>No: Se te solicitará ingresar la fecha real en que se realizó la reunión.</li>" +
                                                          "<li>No se llevó a cabo: Confirmas que la reunión no se realizó.</li></ul>"

        Public Const ConfirmaFechaRetroActaC As String = "¿Se llevó a cabo la reunión 15.1 - Retroalimentación de acta de conclusión?"

        'RRA
        Public Const Vulnerabilidades As String = "Favor de ingresar la fecha de la revisión de vulnerabilidades"
        ''Implementacion de mensaje Reuniom 13.1
        Public Const FechaReunionResAfore As String = "¿La reunión 13.1 para revisar la respuesta de la AFORE con VJ se llevó a cabo en la fecha?"
    End Class

    Public Class Imagenes
        Public Const Exito = "~/Imagenes/Errores/3Error.png"
        Public Const Fallo = "~/Imagenes/Errores/2Error.png"
        Public Const Aviso = "~/Imagenes/Errores/1Error.png"
    End Class

    Public Class Parametros
        Public Const SandraPacheco As String = "Correo Secretaria Presidente"
        Public Const RangoFechaValidaPaso9_16 As String = "Días para validar las fecha del paso 9 y 16"
        Public Const MsgSinComentarios As String = "MensajeSinComentariosParaCorreos"
        Public Const MsgComentarios As String = "MensajeComentariosParaCorreos"
        Public Const DiasFechasAnteriores As String = "DiasPermitidosFechasAnteriores"
        Public Const NombreImagenProcesoInspeccionSanc As String = "NombreImagenProcesoInspeccionSanc"
        Public Const NombreImagenProcesoInspeccionSancVF As String = "NombreImagenProcesoInspeccionSancVF"
        Public Const NombreImagenProcesoInspeccionSancOtras As String = "NombreImagenProcesoInspeccionSancOtras"
        Public Const MinutosExtraFechaInicio As String = "MinutosExtraFechaInicio"
        Public Const ValorMaximoPaso13FlujoNoSancion As String = "ValorMaximoPaso13FlujoNoSancion"
        Public Const Activa_DesactivaCopiarFolio As String = "AcDesCopiarFolio"
        Public Const IdAreaPld As String = "ID_AREA_PLD"
        Public Const DiaHabilesSinProrrogaPlazosLegales As String = "DiaHabilesSinProrrogaPlazosLegales"
        Public Const DiaHabilesConProrrogaPlazosLegales As String = "DiaHabilesConProrrogaPlazosLegales"
        Public Const FechaInicialProceso2017 As String = "FechaInicialProceso2017"
        Public Const fechaMinima As DateTime = (#1/1/1900#)
    End Class

    Enum EstatusVisita
        Vigente = 1
        ConProrroga = 2
        ProcesoEmplazamiento = 3
        Emplazada = 4
        ProcesoSanción = 5
        Sancionada = 6
        EnesperadeacciónJurídica = 7
        Revocación = 8
        JuicioNulidad = 9
        Cancelada = 10
        Cerrada = 11
    End Enum

    Public Shared ReadOnly AREA_PLD As Integer = Conexion.SQLServer.Parametro.ObtnerIdAreaPld("ID_AREA_PLD")
    Public Shared ReadOnly AREAS_OPERATIVAS As String = ",34,35,36,1," & AREA_PLD.ToString() & ","
    Public Shared ReadOnly AREAS_OPERATIVAS_Y_VJ As String = ",34,35,36,1,37," & AREA_PLD.ToString() & ","
    Public Shared ReadOnly AREAS_OPERATIVAS_SN_PREC As String = ",35,36,1," & AREA_PLD.ToString() & ","


    Public Shared Function EsAreaOperativaConPrec(idAreaActual As Integer) As Boolean
        If (From objA As Entities.Area In Constantes.GetAreasOperativas() Where objA.Identificador = idAreaActual Select objA).Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function EsAreaSeprisSnPrec(idAreaActual As Integer) As Boolean
        If idAreaActual = AREA_PR Then Return False

        If (From objA As Entities.Area In Constantes.GetAreasSnPrec() Where objA.Identificador = idAreaActual Select objA).Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Enum TipoDocumento
        Ninguno = -1

        AtentaNota = 1
        Registro = 2
        PorElArea = 3

        BorrardorNoProdedencia = 4

        DefinitivoCancelacion = 5

        DefinitivoNoProcede = 6

        BorradorSancion = 7
        DefinitivoSansion = 8

        BorradorEmplazamiento = 9
        DefinitivoEmplazamiento = 10

        DefinitivoCierreFaltaInfo = 11
        DefinitivoProrroga = 12
        DefinitivoMulta = 13
        DefinitivoPago = 14
        DefinitivoImpugnacion = 15
        DefinitivoRespuestaAfore = 16
        DefinitivoRevocacion = 17
        DefinitivoNulidad = 18
        AtentaNotaWord = 19

        borradorSolicitaInformación = 20
        borradorProrrogaEmplazamiento = 21
        borradorRespuestaAfore = 22

        Prorroga = 23
        Respuesta = 24
        Pago_Multa = 25

        borradorNotificaSAT = 26

        OficioAlterno = 27
        DefinitivoSolicitud = 28
        DefinitivoEscProrroga = 29
        DefinitivoOficioSAT = 30
        DefinitivoRequerimientoOficioPago = 31
        'NHM INI
        DictamenWord = 31
        'NHM FIN

        DefinitivoOficioAlterno = 32

        DictamenPDF = 34

        BorradorCitatorio = 35
        DefinitivoCitatorio = 36
        BorradorCedula = 37
        DefinitivoCedula = 38
        BorradorInsDocumental = 39
        DefinitivoInsDocumental = 40
        BorradorSolInfoOfic = 41
        DefinitivoSolInfoOfic = 42
    End Enum

    Public Shared Function TipoDocumentoSicod(idTipo As Integer) As String
        Dim lsNomArea As String = ""

        Select Case idTipo
            Case 31
                lsNomArea = "Dictamen" '31	Dictámen Word
            Case 28
                lsNomArea = "Solicitud de Información"  '28	DefinitivoSolicitud
            Case 10
                lsNomArea = "Oficio de Emplazamiento"  '10	definitivoEmplazamiento-----
            Case 6
                lsNomArea = "Oficio/ Memo de No Procedencia" ' 6	No Procede Sicod ------
            Case 29
                lsNomArea = "Escrito de Prórroga" ' 29	DefinitivoEscProrroga
            Case 16
                lsNomArea = "Respuesta de Emplazamiento" ' 16	definitivoRespuestaAfore-----
            Case 8
                lsNomArea = "Oficio de Sanción"   '8	definitivoSansion------
            Case 30
                lsNomArea = "Oficio SAT"  '30	DefinitivoOficioSAT
            Case 14
                lsNomArea = "Oficio de Requerimiento de Pago" '14	definitivoPago-----
            Case Else
                lsNomArea = ""

        End Select

        Return lsNomArea
    End Function
    Public Shared Function TipoOficio(idTipo As Integer) As String
        Dim lsTipoOficio As String = ""
        Try
            Select Case idTipo
                Case 1
                    lsTipoOficio = "Externo"
                Case 2
                    lsTipoOficio = "Dictamen"
                Case 3
                    lsTipoOficio = "Atenta Nota"
                Case 4
                    lsTipoOficio = "Oficio Interno"
                Case Else
                    lsTipoOficio = "Indefinido"
            End Select

        Catch ex As Exception
            lsTipoOficio = "Indefinido"
        End Try

        Return lsTipoOficio
    End Function
End Class
