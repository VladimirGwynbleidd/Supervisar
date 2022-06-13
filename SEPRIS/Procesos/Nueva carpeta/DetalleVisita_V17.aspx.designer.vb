'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class DetalleVisita_V17

   '''<summary>
   '''Control lblFolioVisita.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblFolioVisita As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control lblPasoVisita.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblPasoVisita As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Ul2.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Ul2 As Global.System.Web.UI.HtmlControls.HtmlGenericControl

   '''<summary>
   '''Control imgSubvisitas.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgSubvisitas As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgCancelarVisita.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgCancelarVisita As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgSolicitarProrroga.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgSolicitarProrroga As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgProcesoVisita.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgProcesoVisita As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgProcesoVisitaAmbos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgProcesoVisitaAmbos As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control sbSubVisitas.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents sbSubVisitas As Global.SEPRIS.SubVisitas

   '''<summary>
   '''Control ccfCopiarFolios.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents ccfCopiarFolios As Global.SEPRIS.CopiarFolios

   '''<summary>
   '''Control idContPestanias.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents idContPestanias As Global.AjaxControlToolkit.TabContainer

   '''<summary>
   '''Control tpPestaniaP.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents tpPestaniaP As Global.AjaxControlToolkit.TabPanel

   '''<summary>
   '''Control cuDetallePrincipal.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents cuDetallePrincipal As Global.SEPRIS.cuDetalleVisita

   '''<summary>
   '''Control t_ddlAbogadoAsignado.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents t_ddlAbogadoAsignado As Global.System.Web.UI.HtmlControls.HtmlTable

   '''<summary>
   '''Control udpAsignacionUsuarios.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents udpAsignacionUsuarios As Global.System.Web.UI.UpdatePanel

   '''<summary>
   '''Control tbAsignacionInspectores.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents tbAsignacionInspectores As Global.System.Web.UI.HtmlControls.HtmlTable

   '''<summary>
   '''Control lblTituloAbogado.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblTituloAbogado As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control trRenSupTit.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents trRenSupTit As Global.System.Web.UI.HtmlControls.HtmlTableRow

   '''<summary>
   '''Control trRenSupUsu.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents trRenSupUsu As Global.System.Web.UI.HtmlControls.HtmlTableRow

   '''<summary>
   '''Control lsbUsuariosDisponibles.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lsbUsuariosDisponibles As Global.System.Web.UI.WebControls.ListBox

   '''<summary>
   '''Control imgAsignarSupervisor.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgAsignarSupervisor As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control lsbSupervisor.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lsbSupervisor As Global.System.Web.UI.WebControls.ListBox

   '''<summary>
   '''Control trRenSupFle.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents trRenSupFle As Global.System.Web.UI.HtmlControls.HtmlTableRow

   '''<summary>
   '''Control imgDesasignarSupervisor.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgDesasignarSupervisor As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control lblAsignaAbogado.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblAsignaAbogado As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control lsbAbogadoDisponibles.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lsbAbogadoDisponibles As Global.System.Web.UI.WebControls.ListBox

   '''<summary>
   '''Control imgAsignarAbogado.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgAsignarAbogado As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control lsbAbogado.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lsbAbogado As Global.System.Web.UI.WebControls.ListBox

   '''<summary>
   '''Control imgDesasignarAbogado.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgDesasignarAbogado As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control tbDiasTranscurridosVisitaInSitu.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents tbDiasTranscurridosVisitaInSitu As Global.System.Web.UI.HtmlControls.HtmlTable

   '''<summary>
   '''Control lblDiasTranscurridosVisitaInSitu.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblDiasTranscurridosVisitaInSitu As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control tbDiasTranscurridosDiagnosticoHallazgos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents tbDiasTranscurridosDiagnosticoHallazgos As Global.System.Web.UI.HtmlControls.HtmlTable

   '''<summary>
   '''Control lblDiasTranscurridosDiagnosticoHallazgos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblDiasTranscurridosDiagnosticoHallazgos As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control tbHallazgozInt.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents tbHallazgozInt As Global.System.Web.UI.HtmlControls.HtmlTable

   '''<summary>
   '''Control txtEditAllazgosInt.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txtEditAllazgosInt As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control btnEditAllazgosInt.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnEditAllazgosInt As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control tbHallazgozExt.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents tbHallazgozExt As Global.System.Web.UI.HtmlControls.HtmlTable

   '''<summary>
   '''Control txtEditAllazgosExt.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txtEditAllazgosExt As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control btnEditAllazgosExt.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnEditAllazgosExt As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control tbVulnerabilidad.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents tbVulnerabilidad As Global.System.Web.UI.HtmlControls.HtmlTable

   '''<summary>
   '''Control txtFechaVulnerabilidad.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txtFechaVulnerabilidad As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control ImgFecVulnerabilidad.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents ImgFecVulnerabilidad As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control tbDiasTranscurridosPosterioresPago.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents tbDiasTranscurridosPosterioresPago As Global.System.Web.UI.HtmlControls.HtmlTable

   '''<summary>
   '''Control lblDiasTranscurridosPosterioresPago.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblDiasTranscurridosPosterioresPago As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control tbAforeSolicita.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents tbAforeSolicita As Global.System.Web.UI.HtmlControls.HtmlTable

   '''<summary>
   '''Control rdbRecurosRevocacion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents rdbRecurosRevocacion As Global.System.Web.UI.WebControls.RadioButton

   '''<summary>
   '''Control rdbJuicioNulidad.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents rdbJuicioNulidad As Global.System.Web.UI.WebControls.RadioButton

   '''<summary>
   '''Control rdbNinguno.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents rdbNinguno As Global.System.Web.UI.WebControls.RadioButton

   '''<summary>
   '''Control lblMensajeSolicitaAforeObligatorio.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblMensajeSolicitaAforeObligatorio As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control divComentarios.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents divComentarios As Global.System.Web.UI.HtmlControls.HtmlGenericControl

   '''<summary>
   '''Control lblComentarios.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblComentarios As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control txbComentarios.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txbComentarios As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control lblMensajeComentarioObligatorio.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblMensajeComentarioObligatorio As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Div2.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Div2 As Global.System.Web.UI.HtmlControls.HtmlGenericControl

   '''<summary>
   '''Control lblCarComentarios.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblCarComentarios As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control lblComentariosCaracteres.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblComentariosCaracteres As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control imgDetener.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgDetener As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgNotificar3.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgNotificar3 As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgGuardarCambios.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgGuardarCambios As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgNotificar.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgNotificar As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgAnterior.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgAnterior As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgInicio.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgInicio As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgRevisarDocs.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgRevisarDocs As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgAgendar.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgAgendar As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgAgendarP11.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgAgendarP11 As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgAgendarP12.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgAgendarP12 As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgCambioNormatividad.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgCambioNormatividad As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgSiguiente.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgSiguiente As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgNotificar2.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgNotificar2 As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgIniciarVisita.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgIniciarVisita As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgSolicitaRespuestaP14.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgSolicitaRespuestaP14 As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgRechazarProrroga.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgRechazarProrroga As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgAprobarProrroga.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgAprobarProrroga As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control Label2.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Label2 As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control tcContPestaDocs.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents tcContPestaDocs As Global.AjaxControlToolkit.TabContainer

   '''<summary>
   '''Control tpPestaniasDocsP.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents tpPestaniasDocsP As Global.AjaxControlToolkit.TabPanel

   '''<summary>
   '''Control ucDocumentos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents ucDocumentos As Global.SEPRIS.ucDocumentos

   '''<summary>
   '''Control hfCurrentTab.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents hfCurrentTab As Global.System.Web.UI.WebControls.HiddenField

   '''<summary>
   '''Control hfFolioVisita.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents hfFolioVisita As Global.System.Web.UI.WebControls.HiddenField

   '''<summary>
   '''Control hfEstaEnProrroga.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents hfEstaEnProrroga As Global.System.Web.UI.WebControls.HiddenField

   '''<summary>
   '''Control txbMotivoCancelacion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txbMotivoCancelacion As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control ast1.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents ast1 As Global.System.Web.UI.HtmlControls.HtmlGenericControl

   '''<summary>
   '''Control Div1.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Div1 As Global.System.Web.UI.HtmlControls.HtmlGenericControl

   '''<summary>
   '''Control Label20.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Label20 As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control lblCancelacionCaracteres.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblCancelacionCaracteres As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control imgErrorInfoProrroga.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgErrorInfoProrroga As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblFecFinSinProrroga.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblFecFinSinProrroga As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control lblFecFinConProrroga.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblFecFinConProrroga As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control txbMotivoProrroga.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txbMotivoProrroga As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control Div3.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Div3 As Global.System.Web.UI.HtmlControls.HtmlGenericControl

   '''<summary>
   '''Control Label21.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Label21 As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control lblProrrogaCaracteres.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblProrrogaCaracteres As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control ucDoctosProrroga.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents ucDoctosProrroga As Global.SEPRIS.ucDoctosProrroga

   '''<summary>
   '''Control ast2.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents ast2 As Global.System.Web.UI.HtmlControls.HtmlGenericControl

   '''<summary>
   '''Control Image4.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image4 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txtFechaConvoca.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txtFechaConvoca As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control calExFecConvoca.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents calExFecConvoca As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgCalendarFecConvoca.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgCalendarFecConvoca As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblFechConvoca.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblFechConvoca As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image19.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image19 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txtFechaConvocaP11.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txtFechaConvocaP11 As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control calExFecConvocaP11.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents calExFecConvocaP11 As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control Image20.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image20 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblFechConvocaP11.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblFechConvocaP11 As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image13.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image13 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control imgUnBotonNoAccion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgUnBotonNoAccion As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblFechaActaCirc.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblFechaActaCirc As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control vsErrores.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents vsErrores As Global.System.Web.UI.WebControls.ValidationSummary

   '''<summary>
   '''Control imgDosBotonesUnaAccion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgDosBotonesUnaAccion As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control btnAceptarM2B1A.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnAceptarM2B1A As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnAgendarM2.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnAgendarM2 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnAgendarM2_P11.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnAgendarM2_P11 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnCancelarM2B1A.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnCancelarM2B1A As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnSiProrroga.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnSiProrroga As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnCambioNormativa.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnCambioNormativa As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control Image2.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image2 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txtFecIniVista.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txtFecIniVista As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control calExFecIni.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents calExFecIni As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgCalendarFecIni.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgCalendarFecIni As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblErrorFecIni.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblErrorFecIni As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image3.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image3 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txbFechFinVisita.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txbFechFinVisita As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control CalendarExtender1.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents CalendarExtender1 As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgCalendario2.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgCalendario2 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblMensajeFechaFinVisitaObligatorio.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblMensajeFechaFinVisitaObligatorio As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image16.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image16 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control Image5.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image5 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblFolioVisitaRango.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblFolioVisitaRango As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control txbRangoInicio.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txbRangoInicio As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control txbRangoFin.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txbRangoFin As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control Label22.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Label22 As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control txtRango.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txtRango As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control lblMensajeRangoSancionObligatorio.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblMensajeRangoSancionObligatorio As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image6.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image6 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txbFechaEnvioDictamen.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txbFechaEnvioDictamen As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control CalendarExtender3.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents CalendarExtender3 As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgCalendario4.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgCalendario4 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblMensajeFechaEnvioDictamenObligatorio.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblMensajeFechaEnvioDictamenObligatorio As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image7.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image7 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txbFechaPosibleEmplazamiento.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txbFechaPosibleEmplazamiento As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control CalendarExtender4.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents CalendarExtender4 As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgCalendario5.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgCalendario5 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblMensajeFechaPosibleEmplazamientoObligatorio.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblMensajeFechaPosibleEmplazamientoObligatorio As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image8.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image8 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txbFechaAcuseAforeRecibeOfEmpl.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txbFechaAcuseAforeRecibeOfEmpl As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control CalendarExtender5.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents CalendarExtender5 As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgCalendario6.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgCalendario6 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblMensajeFechaAcuseAforeRecibeOfEmplObligatorio.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblMensajeFechaAcuseAforeRecibeOfEmplObligatorio As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image9.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image9 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txbFechaAcuseAforeContestoOfEmpl.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txbFechaAcuseAforeContestoOfEmpl As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control CalendarExtender6.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents CalendarExtender6 As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgCalendario7.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgCalendario7 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblMensajeFechaAcuseAforeContestoOfEmplObligatorio.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblMensajeFechaAcuseAforeContestoOfEmplObligatorio As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image10.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image10 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txbFechaImposicionSancion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txbFechaImposicionSancion As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control CalendarExtender7.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents CalendarExtender7 As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgCalendario8.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgCalendario8 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txtMontoImpSan.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txtMontoImpSan As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control txtComentImpSan.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txtComentImpSan As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control lblMensajeFechaImposicionSancionObligatorio.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblMensajeFechaImposicionSancionObligatorio As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image11.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image11 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txbFechaImposicionSancionEstimada.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txbFechaImposicionSancionEstimada As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control CalendarExtender8.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents CalendarExtender8 As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgCalendario9.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgCalendario9 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblMensajeFechaImposicionSancionEstimadaObligatorio.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblMensajeFechaImposicionSancionEstimadaObligatorio As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image12.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image12 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txbFechaAcusePagoAfore.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txbFechaAcusePagoAfore As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control CalendarExtender9.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents CalendarExtender9 As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgCalendario10.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgCalendario10 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblMensajeFechaAcusePagoAforeObligatorio.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblMensajeFechaAcusePagoAforeObligatorio As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image17.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image17 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txtFechaCampo.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txtFechaCampo As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control ceFechaCampo.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents ceFechaCampo As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgFechaCampo.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgFechaCampo As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblFechaCampo.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblFechaCampo As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image18.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image18 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txtFechRevHallazgos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txtFechRevHallazgos As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control ceFechaCampoHallazgos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents ceFechaCampoHallazgos As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgFechaCampo2.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgFechaCampo2 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblInfoPregunta81.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblInfoPregunta81 As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control lblPresentaHallazgos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblPresentaHallazgos As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control Image15.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image15 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control txtFechaReunion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txtFechaReunion As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control ceFechaReunion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents ceFechaReunion As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgFechaReunion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgFechaReunion As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblFechaReunion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblFechaReunion As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control imgProcesoVF.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgProcesoVF As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgProcesoOtras.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgProcesoOtras As Global.System.Web.UI.WebControls.ImageButton

   '''<summary>
   '''Control imgProcesoInspSancion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgProcesoInspSancion As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control imgProcesoInspSancionVF.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgProcesoInspSancionVF As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control imgProcesoInspSancionOtras.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgProcesoInspSancionOtras As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control imgErrorGeneral.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgErrorGeneral As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control lblDscFechaPaso.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblDscFechaPaso As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control txtFechaGeneral.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents txtFechaGeneral As Global.System.Web.UI.WebControls.TextBox

   '''<summary>
   '''Control ceFechaGeneral.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents ceFechaGeneral As Global.AjaxControlToolkit.CalendarExtender

   '''<summary>
   '''Control imgFechaGeneral.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgFechaGeneral As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control dvRdo.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents dvRdo As Global.System.Web.UI.HtmlControls.HtmlGenericControl

   '''<summary>
   '''Control lblInfoValFechaIngresada.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblInfoValFechaIngresada As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control lblFechaGeneral.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents lblFechaGeneral As Global.System.Web.UI.WebControls.Label

   '''<summary>
   '''Control imgConfirmacion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents imgConfirmacion As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control ValidationSummary1.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents ValidationSummary1 As Global.System.Web.UI.WebControls.ValidationSummary

   '''<summary>
   '''Control Image1.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image1 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control ValidationSummary3.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents ValidationSummary3 As Global.System.Web.UI.WebControls.ValidationSummary

   '''<summary>
   '''Control divSubVisitas.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents divSubVisitas As Global.System.Web.UI.HtmlControls.HtmlGenericControl

   '''<summary>
   '''Control Image14.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents Image14 As Global.System.Web.UI.WebControls.Image

   '''<summary>
   '''Control chkSubVisitasMod.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents chkSubVisitasMod As Global.System.Web.UI.WebControls.CheckBoxList

   '''<summary>
   '''Control ValidationSummary2.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents ValidationSummary2 As Global.System.Web.UI.WebControls.ValidationSummary

   '''<summary>
   '''Control btnPasoDiezConfSancionNo.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnPasoDiezConfSancionNo As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfSanPasoSieteSi.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfSanPasoSieteSi As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfSanPasoSieteNo.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfSanPasoSieteNo As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfSanPasoSieteSi_V17.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfSanPasoSieteSi_V17 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfSanPasoSieteNo_V17.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfSanPasoSieteNo_V17 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnSancionPaso8Si.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnSancionPaso8Si As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnSancionPaso8No.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnSancionPaso8No As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnPasoSieteConfirmSi.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnPasoSieteConfirmSi As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnPasoSieteConfirmNo.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnPasoSieteConfirmNo As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control hdfPresentacionP6y7.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents hdfPresentacionP6y7 As Global.System.Web.UI.WebControls.HiddenField

   '''<summary>
   '''Control btnPasoSieteConfirmSi_V17.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnPasoSieteConfirmSi_V17 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnPasoSieteConfirmNo_V17.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnPasoSieteConfirmNo_V17 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnPasoCincoConfirmSi.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnPasoCincoConfirmSi As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnPasoCincoConfirmNo.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnPasoCincoConfirmNo As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaPaso25Ok.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaPaso25Ok As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaPaso25NoDos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaPaso25NoDos As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaPaso32Ok.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaPaso32Ok As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaPaso32NoDos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaPaso32NoDos As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfRetroActaConP18.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfRetroActaConP18 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfRetroActaConP18NoDos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfRetroActaConP18NoDos As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaPaso25.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaPaso25 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaPaso32.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaPaso32 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaPaso25Conf.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaPaso25Conf As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaPaso32Conf.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaPaso32Conf As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaPaso18Conf.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaPaso18Conf As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaReunionP8ok.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaReunionP8ok As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaReunionP8NoDos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaReunionP8NoDos As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaReunionP9ok.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaReunionP9ok As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaReunionP9NoDos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaReunionP9NoDos As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaRetroOk.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaRetroOk As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaRetroNo.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaRetroNo As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaRetroNoDos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaRetroNoDos As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnAlertaPaso18.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnAlertaPaso18 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaReunionVjP8.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaReunionVjP8 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnCancelaFechaReunionVjP8.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnCancelaFechaReunionVjP8 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaReunionVjp9.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaReunionVjp9 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnCancelaFechaReunionVjp9.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnCancelaFechaReunionVjp9 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnCancelaFechaReunionVjp16.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnCancelaFechaReunionVjp16 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnCancelaFechaPaso25.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnCancelaFechaPaso25 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnCancelaFechaPaso32.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnCancelaFechaPaso32 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaReunionP10.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaReunionP10 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnCancelaFechaReunionP10.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnCancelaFechaReunionP10 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfRespuestaAforeP14Ok.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfRespuestaAforeP14Ok As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfRespuestaAforeP14No.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfRespuestaAforeP14No As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaPresInternaHP10.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaPresInternaHP10 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnCancelaFechaPresInternaHP10.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnCancelaFechaPresInternaHP10 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaReunionPresInternaTres.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaReunionPresInternaTres As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaReunionPresInterna.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaReunionPresInterna As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaReunionVjp16.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaReunionVjp16 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaReunionVjpok.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaReunionVjpok As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfFechaReunionVjp16NoDos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfFechaReunionVjp16NoDos As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaInSituActaC.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaInSituActaC As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaLevantaActa.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaLevantaActa As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfirmaFechaRetroalimentacion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfirmaFechaRetroalimentacion As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfirmaFechaRetroalimentacionNo.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfirmaFechaRetroalimentacionNo As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaRetroalimentacion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaRetroalimentacion As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control hfBotonPresionado.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents hfBotonPresionado As Global.System.Web.UI.WebControls.HiddenField

   '''<summary>
   '''Control btnSubvisitasModal.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnSubvisitasModal As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaCampo.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaCampo As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaRevHallazgos_V17.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaRevHallazgos_V17 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaRevHallazgos_V17_SC.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaRevHallazgos_V17_SC As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaRevHallazgos.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaRevHallazgos As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaReunion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaReunion As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnRangoSancion.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnRangoSancion As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnRangoSancionPaso17.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnRangoSancionPaso17 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnRangoSancionPaso17Sub.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnRangoSancionPaso17Sub As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaAforePresi.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaAforePresi As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfirmarDv.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfirmarDv As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfirmarNoDv.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfirmarNoDv As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaReunionVj.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaReunionVj As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaReunionVjP16.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaReunionVjP16 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaReunionRetroComAtAC.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaReunionRetroComAtAC As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnFechaReunionRetroComAtACP17.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnFechaReunionRetroComAtACP17 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfirmacionVulnerabilidadSi.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfirmacionVulnerabilidadSi As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnConfirmacionVulnerabilidadNo.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnConfirmacionVulnerabilidadNo As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnGuardaFecVulnerabilidad.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnGuardaFecVulnerabilidad As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnGuardaFecVulnerabilidad4.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnGuardaFecVulnerabilidad4 As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnCancelaFecVulnerabilidad.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnCancelaFecVulnerabilidad As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnDetieneAvanza.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnDetieneAvanza As Global.System.Web.UI.WebControls.Button

   '''<summary>
   '''Control btnNoCierraPaso7.
   '''</summary>
   '''<remarks>
   '''Campo generado automáticamente.
   '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
   '''</remarks>
   Protected WithEvents btnNoCierraPaso7 As Global.System.Web.UI.WebControls.Button
End Class
