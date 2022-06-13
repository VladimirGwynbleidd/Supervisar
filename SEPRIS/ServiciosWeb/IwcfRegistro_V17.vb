Imports System.ServiceModel

' NOTA: puede usar el comando "Cambiar nombre" del menú contextual para cambiar el nombre de interfaz "IwcfRegistro" en el código y en el archivo de configuración a la vez.
<ServiceContract()>
Public Interface IwcfRegistro_V17

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
                                   pdFechaVul As DateTime,
                                   pbHabraRevicionVul As Boolean) As List(Of String)

   <OperationContract()>
   Function Paso1(IdVisitaSEPRIS As Integer,
                                   IdentificadorUsuario As String,
                                   HaySesion As Boolean,
                                   FechaSesVulnerabilidad As DateTime,
                                   psComentarios As String,
                                   cambioNormativa As Boolean) As List(Of String)

   <OperationContract()>
   Function Paso3(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, _
                                   ByVal PrimeraVez As Boolean, _
                                   ByVal Aceptado As Boolean, _
                                   ByVal CambioNorma As Boolean, _
                                   ByVal Comentarios As String) As List(Of String)

   <OperationContract()>
   Function Paso4(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, _
                                   ByVal FechaInicioP4 As DateTime, _
                                   ByVal Comentarios As String) As List(Of String)

   <OperationContract()>
   Function Paso5(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, _
                                   ByVal idVisitaSISVIG As Integer, _
                                   ByVal HuboRespuesta As Boolean, _
                                   ByVal FechaInSitu As DateTime) As List(Of String)


   <OperationContract()>
   Function Paso6(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal FechaInSitu As DateTime) As List(Of String)

   <OperationContract()>
   Function Paso6y7(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal FechaInSitu As DateTime, _
                     ByVal HayPres As Boolean, ByVal HaySancion As Boolean, ByVal FechaP7 As DateTime) As List(Of String)

   <OperationContract()>
   Function Paso7(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, _
                     ByVal HayPres As Boolean, ByVal HaySancion As Boolean, ByVal Fecha As DateTime) As List(Of String)

   <OperationContract()>
   Function Paso8(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal Fecsion As DateTime, ByVal Comentarios As String) As List(Of String)

   <OperationContract()>
   Function Paso9(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal Aceptado As Boolean, ByVal FechaVOBO As DateTime, ByVal Comentarios As String) As List(Of String)

   <OperationContract()>
   Function Paso11(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal Nuevo As Boolean, ByVal FecPresVJ As DateTime, ByVal Notifica As Boolean) As List(Of String)

   <OperationContract()>
   Function Paso12(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal Nuevo As Boolean, ByVal FecPresAfore As DateTime) As List(Of String)

   <OperationContract()>
   Function Paso13(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal FecLevantamiento As DateTime) As List(Of String)

   <OperationContract()>
   Function Paso14(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal HayRespAfore As Boolean) As List(Of String)

   <OperationContract()>
   Function Paso15(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal FecC As DateTime, ByVal Comentarios As String) As List(Of String)

   <OperationContract()>
   Function Paso16(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal Aceptado As Boolean, ByVal FechaVobo As DateTime, _
                   ByVal Comentarios As String) As List(Of String)

   <OperationContract()>
   Function Paso18(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal Acepta As Boolean, ByVal FecVoBo As DateTime, ByVal Comentarios As String) As List(Of String)

   <OperationContract()>
   Function Paso19(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal FechaLev As DateTime) As List(Of String)

   <OperationContract()>
   Function Paso20(ByVal idVisitaSEPRIS As Integer, ByVal Usr As String, ByVal FechaLev As DateTime) As List(Of String)

   <OperationContract()>
   Function Notificar(ByVal idVisitaSEPRIS As Integer, ByVal idCorreo As Integer, _
                                ByVal idUsuario As String, _
                                Optional pbAreaVoVf As Boolean = False,
                                       Optional pbAreaVj As Boolean = False,
                                       Optional pbAreaPresidencia As Boolean = False,
                                       Optional lstPersonasDestinatarios As List(Of Persona) = Nothing,
                                       Optional pbSuperUsuarios As Boolean = False) As List(Of String)

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

   <OperationContract()>
   Function BloquearCargaDeArchivos(ByVal IdVisita As Integer, IdentificadorUsuario As String, ByVal Edo As Boolean) As List(Of String)

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
