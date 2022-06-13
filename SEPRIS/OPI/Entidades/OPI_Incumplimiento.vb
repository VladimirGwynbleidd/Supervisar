<Serializable>
Public Class OPI_Incumplimiento
    Private _tabla As String = "BDS_D_OPI_INCUMPLIMIENTO"
    Public ReadOnly Property Tabla() As String
        Get
            Return _tabla
        End Get
    End Property

    Private _i_opiid As Integer
    Public Property I_ID_OPI() As Integer
        Get
            Return _i_opiid
        End Get
        Set(ByVal value As Integer)
            _i_opiid = value
        End Set
    End Property

    Private _t_id_Folio As String
    Public Property T_ID_FOLIO() As String
        Get
            Return _t_id_Folio
        End Get
        Set(ByVal value As String)
            _t_id_Folio = value
        End Set
    End Property

    Private _i_id_paso As Integer
    Public Property N_ID_PASO() As Integer
        Get
            Return _i_id_paso
        End Get
        Set(ByVal value As Integer)
            _i_id_paso = value
        End Set
    End Property

    Private _i_id_subpaso As Integer
    Public Property N_ID_SUBPASO() As Integer
        Get
            Return _i_id_subpaso
        End Get
        Set(ByVal value As Integer)
            _i_id_subpaso = value
        End Set
    End Property

    Private _i_tipoEnt_id As Integer 'Entities.EntidadSicod
    Public Property I_ID_TIPO_ENTIDAD() As Integer 'Entities.EntidadSicod
        Get
            Return _i_tipoEnt_id
        End Get
        Set(ByVal value As Integer)
            _i_tipoEnt_id = value
        End Set
    End Property

    Private _i_entidadId As Integer 'Entities.EntidadTipo
    Public Property I_ID_ENTIDAD() As Integer 'Entities.EntidadTipo
        Get
            Return _i_entidadId
        End Get
        Set(ByVal value As Integer)
            _i_entidadId = value
        End Set
    End Property

    Private _i_subEntidadId As List(Of Integer) 'Entities.SubEntidad)
    Public Property I_ID_SUBENTIDAD() As List(Of Integer) 'Entities.SubEntidad)
        Get
            Return _i_subEntidadId
        End Get
        Set(ByVal value As List(Of Integer)) 'Entities.SubEntidad))
            _i_subEntidadId = value
        End Set
    End Property

    Private _i_subEntidadSB_Id As Integer
    Public Property I_ID_SUBENTIDAD_SB As Integer
        Get
            Return _i_subEntidadSB_Id
        End Get
        Set(ByVal value As Integer)
            _i_subEntidadSB_Id = value
        End Set
    End Property

    Private _fechaPI As DateTime
    Public Property F_FECH_POSIBLE_INC() As DateTime
        Get
            Return _fechaPI
        End Get
        Set(ByVal value As DateTime)
            _fechaPI = value
        End Set
    End Property

    Private _procesoPI As Integer
    Public Property I_ID_PROCESO_POSIBLE_INC() As Integer
        Get
            Return _procesoPI
        End Get
        Set(ByVal value As Integer)
            _procesoPI = value
        End Set
    End Property

    Private _descripcionPI As String
    Public Property T_DSC_POSIBLE_INC() As String
        Get
            Return _descripcionPI
        End Get
        Set(ByVal value As String)
            _descripcionPI = value
        End Set
    End Property


    Private _id_subproceso As Integer
    Public Property I_ID_SUBPROCESO() As Integer
        Get
            Return _id_subproceso
        End Get
        Set(ByVal value As Integer)
            _id_subproceso = value
        End Set
    End Property


    Private _supervisores As Hashtable
    Public Property T_ID_SUPERVISORES() As Hashtable
        Get
            Return _supervisores
        End Get
        Set(ByVal value As Hashtable)
            _supervisores = value
        End Set
    End Property

    Private _inspectores As Hashtable
    Public Property T_ID_INSPECTORES() As Hashtable
        Get
            Return _inspectores
        End Get
        Set(ByVal value As Hashtable)
            _inspectores = value
        End Set
    End Property

    Private _observaciones As String
    Public Property T_OBSERVACIONES_OPI() As String
        Get
            Return _observaciones
        End Get
        Set(ByVal value As String)
            _observaciones = value
        End Set
    End Property

    Private _f_Fech_Registro As Date
    Public Property F_FECH_REGISTRO() As Date
        Get
            Return _f_Fech_Registro
        End Get
        Set(ByVal value As Date)
            _f_Fech_Registro = value
        End Set
    End Property

    Private _t_clasifiecacion As String
    Public Property T_DSC_CLASIFICACION() As String
        Get
            Return _t_clasifiecacion
        End Get
        Set(ByVal value As String)
            _t_clasifiecacion = value
        End Set
    End Property

    Private _i_id_estatus As Integer
    Public Property I_ID_ESTATUS() As Integer
        Get
            Return _i_id_estatus
        End Get
        Set(ByVal value As Integer)
            _i_id_estatus = value
        End Set
    End Property

    Private _status As String
    Public Property T_DSC_ESTATUS() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _id_supuesto As Integer
    Public Property I_ID_SUPUESTO() As Integer
        Get
            Return _id_supuesto
        End Get
        Set(ByVal value As Integer)
            _id_supuesto = value
        End Set
    End Property

    Private _respAfore As String
    Public Property T_DSC_RESP_AFORE() As String
        Get
            Return _respAfore
        End Get
        Set(ByVal value As String)
            _respAfore = value
        End Set
    End Property

    Private _existIrregularidad As Boolean
    Public Property B_EXISTE_IRREG() As Boolean
        Get
            Return _existIrregularidad
        End Get
        Set(ByVal value As Boolean)
            _existIrregularidad = value
        End Set
    End Property

    Private _justNOI As String
    Public Property T_DSC_JUST_NO_IRREG() As String
        Get
            Return _justNOI
        End Get
        Set(ByVal value As String)
            _justNOI = value
        End Set
    End Property

    Private _desIrreg As Boolean
    Public Property B_IRREG_STD() As Boolean
        Get
            Return _desIrreg
        End Get
        Set(ByVal value As Boolean)
            _desIrreg = value
        End Set
    End Property

    Private _fech As Date?
    Public Property F_FECH_ESTIM_ENTREGA() As Date?
        Get
            Return _fech
        End Get
        Set(ByVal value As Date?)
            _fech = value
        End Set
    End Property

    Private _descPos As String
    Public Property T_DSC_PROC_POSIB_INCUMP() As String
        Get
            Return _descPos
        End Get
        Set(ByVal value As String)
            _descPos = value
        End Set
    End Property

    Private _b_posible_incump As Boolean
    Public Property B_POSIBLE_INC() As Boolean
        Get
            Return _b_posible_incump
        End Get
        Set(ByVal value As Boolean)
            _b_posible_incump = value
        End Set
    End Property

    Private _NoProcede As String
    Public Property T_DSC_MOTIV_NO_PROC() As String
        Get
            Return _NoProcede
        End Get
        Set(ByVal value As String)
            _NoProcede = value
        End Set
    End Property

    Private _Comentarios_Pasos As String
    Public Property T_COMENTARIOS_PASOS() As String
        Get
            Return _Comentarios_Pasos
        End Get
        Set(ByVal value As String)
            _Comentarios_Pasos = value
        End Set
    End Property

    Private _i_idarea As Integer
    Public Property I_ID_AREA() As Integer
        Get
            Return _i_idarea
        End Get
        Set(ByVal value As Integer)
            _i_idarea = value
        End Set
    End Property

    Private _dsc_area As String
    Public Property T_DSC_AREA() As String
        Get
            Return _dsc_area
        End Get
        Set(ByVal value As String)
            _dsc_area = value
        End Set
    End Property

    Private _f_fech_paso_actual As Date?
    Public Property F_FECH_PASO_ACTUAL() As Date?
        Get
            Return _f_fech_paso_actual
        End Get
        Set(ByVal value As Date?)
            _f_fech_paso_actual = value
        End Set
    End Property

    Private _b_proced As Boolean
    Public Property B_PROCEDE() As Boolean
        Get
            Return _b_proced
        End Get
        Set(ByVal value As Boolean)
            _b_proced = value
        End Set
    End Property

    Private _i_id_paso_ant As Integer
    Public Property N_ID_PASO_ANT() As Integer
        Get
            Return _i_id_paso_ant
        End Get
        Set(ByVal value As Integer)
            _i_id_paso_ant = value
        End Set
    End Property

    Private _f_fech_acusedocto As Date?
    Public Property F_FECH_ACUSE_DOCTO() As Date?
        Get
            Return _f_fech_acusedocto
        End Get
        Set(ByVal value As Date?)
            _f_fech_acusedocto = value
        End Set
    End Property

    Private _f_fech_reunion As Date?
    Public Property F_FECH_REUNION() As Date?
        Get
            Return _f_fech_reunion
        End Get
        Set(value As Date?)
            _f_fech_reunion = value
        End Set
    End Property


    Private _f_fech_reunion_real As Date?
    Public Property F_FECH_REUNION_REAL() As Date?
        Get
            Return _f_fech_reunion_real
        End Get
        Set(value As Date?)
            _f_fech_reunion_real = value
        End Set
    End Property

    Private _t_id_folio_sisan As String
    Public Property T_ID_FOLIO_SISAN As String
        Get
            Return _t_id_folio_sisan
        End Get
        Set(value As String)
            _t_id_folio_sisan = value
        End Set
    End Property
    Private _i_id_estatus_ant As Integer
    Public Property I_ID_ESTATUS_ANT() As Integer
        Get
            Return _i_id_estatus_ant
        End Get
        Set(ByVal value As Integer)
            _i_id_estatus_ant = value
        End Set
    End Property
End Class


