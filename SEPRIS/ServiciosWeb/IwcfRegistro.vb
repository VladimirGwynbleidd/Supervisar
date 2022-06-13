Imports System.ServiceModel

' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de interfaz "IwcfRegistro" en el código y en el archivo de configuración a la vez.
<ServiceContract()>
Public Interface IwcfRegistro

    <OperationContract()>
    Function InsertaVisita(IdVisitaSisvig As Integer,
                            IdentificadorUsuario As String,
                            pdFechaInicio As DateTime,
                            piIdEntidad As Integer,
                            psNombreEntidad As String,
                            piTipoVisita As Integer,
                            psLstObjetoVisita As List(Of Integer),
                            psDscOtroObjVisita As String,
                            psSupervisor As String,
                            psLstInspector As List(Of String),
                            psDescripcionVisita As String,
                            psComentarios As String,
                            psOrdenVisita As String,
                            pbHayRevicioVul As Boolean,
                            Optional pdFechaVul As DateTime = Nothing,
                            Optional IdVisitaSepris As Integer = 0) As List(Of String)

    <OperationContract()>
    Function EditaVisita(IdVisitaSepris As Integer,
                            IdentificadorUsuario As String,
                            pdFechaInicio As DateTime,
                            piTipoVisita As Integer,
                            psLstObjetoVisita As List(Of Integer),
                            psSupervisor As String,
                            psLstInspector As List(Of String),
                            psDescripcionVisita As String,
                            psComentarios As String,
                            psOrdenVisita As String,
                            Optional pdFechaVul As DateTime = Nothing) As List(Of String)
    <OperationContract()>
    Function AvanzaPaso(IdVisita As Integer,
                        IdentificadorUsuario As String,
                        psComentarios As String,
                        Optional opcNotifica As Integer = 0,
                        Optional idDocumento As Integer = 0,
                        Optional versionDocto As Integer = 0,
                        Optional nombreDocto As String = "") As List(Of String)

    <OperationContract()>
    Function AvanzaPasoSiete(IdVisita As Integer,
                        IdentificadorUsuario As String,
                        psComentarios As String,
                        pbFlagReunHallazgos As Boolean,
                        pbFlagSancion As Boolean) As List(Of String)

    <OperationContract()>
    Function AvanzaPasoOcho(IdVisita As Integer,
                        IdentificadorUsuario As String,
                        psComentarios As String,
                        pbFlagSancion As Boolean,
                        Optional pbPrimeraNotificacion As Boolean = False) As List(Of String)


    <OperationContract()>
    Function RechazaPaso(IdVisita As Integer,
                        IdentificadorUsuario As String,
                        psComentarios As String) As List(Of String)

    <OperationContract()>
    Function ActualizaFecha(IdVisita As Integer,
                            IdentificadorUsuario As String,
                            psComentarios As String,
                            Fecha As DateTime,
                            TipoFecha As Constantes.TipoFecha,
                            Optional NotificarCambio As Boolean = True) As List(Of String)

    '<OperationContract()>
    'Function ActualizaFechaVulnera(IdVisitaSepris As Integer,
    '                        Fecha As DateTime) As List(Of String)

    '<OperationContract()>
    'Function ActualizaFechaSinAccion(IdVisita As Integer,
    '                        IdentificadorUsuario As String,
    '                        psComentarios As String,
    '                        Fecha As DateTime,
    '                        TipoFecha As Constantes.TipoFecha) As List(Of String)

    <OperationContract()>
    Function FinalizaCargaDocumentos(IdVisita As Integer,
                             IdentificadorUsuario As String,
                             psCadenaParametros As String,
                             psComentarios As String) As List(Of String)

    <OperationContract()>
    Function SolicitaProrroga(IdVisita As Integer,
                             IdentificadorUsuario As String,
                             motivoProrroga As String) As List(Of String)

    '<OperationContract()>
    'Function ObtenerDetalleVisita(IdVisita As Integer) As Visita

    <OperationContract()>
    Function getErrorValidacionSupervisar(ByVal id_error As String) As String

    <OperationContract()>
    Function CancelarVisita(IdVisita As Integer,
                            IdentificadorUsuario As String,
                            motivoCancela As String) As List(Of String)

    <OperationContract()>
    Function ConsultaDocumentosObligatoriosSinCargarSinVisita(IdVisitaSisvig As Integer) As Boolean
End Interface
