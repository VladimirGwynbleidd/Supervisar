/* ********************************************************************************************************************************

Fecha Creación:       18 Julio 2013
Codificó:             Jorge Alberto Rangel Ruiz
Empresa:              Softtek
Descripción:          Código para implementar mensajes modales.


Consideraciones:      
1.- Debe existir la referencia a la librería jquery-ui-1.10.3.custom.js (http://jqueryui.com/download/)
y jquery-ui-1.10.3.custom.min.js (http://jqueryui.com/themeroller/) con el tema Smoothness, así mismo, a la libreria de jquery
jquery-1.9.1.js (versión actual al momento del desarrollo el presente componente)

2.- Cada Mensaje está compuesto de dos llamadas, una en la carga de la página:
método "$(document).ready(function() { XXXXXXX });" o "$(function() { XXXXXXX });"   en javascript
y la segunda donde se desea mostrar el mensaje

3.- Los botones de acción (que realicen postbacks deben incluir la etiqueta ClientIDMode="Static")

4.- Los elementos div deben tener la etiqueta style="display: none"

5.- LEA LA DESCRIPCIÓN Y OBSERVACIONES DE CADA MENSAJE QUE DESEE IMPLEMENTAR


MODIFICACIONES: 


******************************************************************************************************************************** */





/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */
/*  MensajeDosBotonesDosAcciones  */

/*  Descripción: Función para mostrar una ventana modal con un mensaje y dos botones de acción, Continuar y Cerrar  
ambos botones realizan un llamado a servidor
Caracteristicas:  Mensaje en ventana modal, NO CONTIENE MENSAJE DE TITULO
Observaciones IMPORTANTES:
1.- En la página donde se consuma, debe existir un div de nombre divConfirmacionM2B2A, mismo que contiene
el mensaje a mostrar
2.- Así mismo, deben existir dos botones de nombre btnContinuarM2B2A y btnSalirM2B2A, mismos que implementarán las
acciones deseadas por el programador, se sugiere esten ocultos (Style="display: none").  Deben tener la etiqueta ClientIDMode="Static"

Ejemplo implementacion:

<script type="text/javascript">
$(function () {

MensajeDosBotonesDosAccionesLoad();

});

function CreaConfirmacion() {

MensajeDosBotonesDosAcciones();

}
</script>

<body>
<br />
<asp:Button runat="server" ID="btnMostrarMensaje"  OnClientClick="CreaConfirmacion()" />  
<br />
<div id="divConfirmacionM2B2A" style="display: none">
<p>
Mensaje a mostrar. </p>
<p>
</div>
<asp:Button runat="server" ID="btnSalirM2B2A" Style="display: none" ClientIDMode="Static" />
<asp:Button runat="server" ID="btnContinuarM2B2A" Style="display: none" ClientIDMode="Static" />   
</body>

*/

function MantenSesion()
{                
    var CONTROLADOR = "../ReiniciaSesion.ashx";
    var head = document.getElementsByTagName("head").item(0);            
    script = document.createElement("script");            
    script.src = CONTROLADOR ;
    script.setAttribute("type", "text/javascript");
    script.defer = true;
    head.appendChild(script);
} 

// Funcion que debe ser invocada en el método "$(document).ready(function()" o "$(function() {" 
// de la página donde se deseee implementar 
function MensajeDosBotonesDosAccionesLoad() {

    $("#divConfirmacionM2B2A").dialog({
        resizable: false,
        autoOpen: false,
        height: 300,
        width: 500,
        modal: true,
        buttons: {
            "Continuar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cerrar", disabled: true }]);
                $('#btnContinuarM2B2A').trigger("click");
            },
            "Cerrar": function () {
                $("#btnSalirM2B2A").trigger("click");
            }
        }
    });

}

// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesDosAcciones() {

    $("#divConfirmacionM2B2A").dialog({
        resizable: false,
        autoOpen: true,
        height: 300,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Continuar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cerrar", disabled: true }]);
                $('#btnContinuarM2B2A').trigger("click");
            },
            "Cerrar": function () {
                $("#btnSalirM2B2A").trigger("click");
            }
        }
    });


    $("#divConfirmacionM2B2A").dialog("open");

}

/*  FIN MensajeDosBotonesDosAcciones  */
/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */




/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */
/*  MensajeUnBotonNoAccion  */

/*  Descripción: Función para mostrar una ventana modal con un mensaje y un boton, mismo que cierra el mensaje
Caracteristicas:  Mensaje en ventana modal
Observaciones IMPORTANTES:
1.- En la página donde se consuma, debe existir un div de nombre divMensajeUnBotonNoAccion, mismo que contiene
el mensaje a mostrar, y debe implementar la propiedad title para mostrar el titulo en la ventana

Ejemplo implementacion:

<script type="text/javascript">
$(function () {

MensajeUnBotonNoAccionLoad();

});

function MuestraMensajeUnBotonNoAccion() {

MensajeUnBotonNoAccion();
return false;

}
</script>

<body>
<br />
<asp:Button runat="server" ID="btnMostrarMensaje"  OnClientClick="return MuestraMensajeUnBotonNoAccion()" />  
<br />
<div id="divMensajeUnBotonNoAccion" title="Titulo del mensaje" style="display: none">
<p>
Mensaje a mostrar sin acciones a realizar </p>
<p>
</div>
</body>

*/

function MensajeUnBotonNoAccionLoad() {

    $("#divMensajeUnBotonNoAccion").dialog({
        resizable: true,
        height: 300,
        width: 500,
        modal: true,
        autoOpen: false,
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        }
    });

}

function MensajeUnBotonNoAccionRA14() {

    $("#divMensajeUnBotonNoAccion").dialog({
        resizable: true,
        height: 300,
        width: 500,
        modal: true,
        autoOpen: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $('#imgRevisarDocs').trigger("click");
                $(this).dialog("close");
            }
        }
    });

}

function MensajeUnBotonNoAccion() {

    $("#divMensajeUnBotonNoAccion").dialog({
        resizable: true,
        height: 300,
        width: 500,
        modal: true,
        autoOpen: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        }
    });

}

function MensajeModalPendientes() {

    $("#dvModalPendientes").dialog({
        resizable: true,
        height: 500,
        width: 900,
        modal: true,
        autoOpen: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        }
    });

}

/*  FIN MensajeUnBotonNoAccion  */
/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */






/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */
/*  MensajeUnBotonUnaAccion  */

/*  Descripción: Función para mostrar una ventana modal con un mensaje y un boton, mismo que realiza una acción
Caracteristicas:  Mensaje en ventana modal
Observaciones IMPORTANTES:
1.- En la página donde se consuma, debe existir un div de nombre divMensajeUnBotonUnaAccion, mismo que contiene
el mensaje a mostrar, y debe implementar la propiedad title para mostrar el titulo en la ventana
2.- Así mismo, debe existir un boton de nombre btnAceptarM1B1A, mismo que implementará la
accion deseada por el programador, se sugiere este oculto (Style="display: none").  Debe tener la etiqueta ClientIDMode="Static"

Ejemplo implementacion:

<script type="text/javascript">
$(function () {

MensajeUnBotonUnaAccionLoad();

});

function MuestraMensajeAceptar() {

MensajeUnBotonUnaAccion();

}
</script>

<body>
<br />
<asp:Button runat="server" ID="btnMostrarMensaje"  OnClientClick="MuestraMensajeAceptar()" />  
<br />
<div id="divMensajeUnBotonUnaAccion" style="display: none">
<p>
Mensaje a mostrar, esto puede ser un label </p>
<p>
</div>
<asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
</body>

*/


function MensajeUnBotonUnaAccionLoad() {

    $("#divMensajeUnBotonUnaAccion").dialog({
        resizable: true,
        height: 300,
        width: 500,
        modal: true,
        autoOpen: false,
        buttons: {
            "Aceptar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }]);
                $('#btnAceptarM1B1A').trigger("click");
            }
        }
    });

}


function MensajeUnBotonUnaAccion() {

    $("#divMensajeUnBotonUnaAccion").dialog({
        resizable: true,
        height: 300,
        width: 500,
        modal: true,
        autoOpen: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }]);
                $('#btnAceptarM1B1A').trigger("click");
            }
        }
    });

}


/*  FIN MensajeUnBotonUnaAccion  */
/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */


/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */
/*  MensajeDosBotonesUnaAccion  */

/*  Descripción: Función para mostrar una ventana modal con un mensaje y dos botones con solo una acción, Aceptar y Cancelar  
el botón Aceptar realiza un llamado a servidor
Caracteristicas:  Mensaje en ventana modal
Observaciones IMPORTANTES:
1.- En la página donde se consuma, debe existir un div de nombre divMensajeDosBotonesUnaAccion, mismo que contiene
el mensaje a mostrar
2.- Así mismo, debe existir un boton de nombre btnAceptarM2B1A, mismo que implementará la
accion deseada por el programador, se sugiere este oculto (Style="display: none").  Debe tener la etiqueta ClientIDMode="Static"

Ejemplo implementacion:

<script type="text/javascript">
$(function () {

MensajeDosBotonesUnaAccionLoad();

});

function LevantaVentanaConfirma() {

MensajeDosBotonesUnaAccion();

}
</script>

<body>
<br />
<asp:Button runat="server" ID="btnMostrarMensaje"  OnClientClick="LevantaVentanaConfirma()" />  
<br />
<div id="divMensajeDosBotonesUnaAccion" style="display: none">
<p>
Mensaje a mostrar. </p>
<p>
</div>
<asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
</body>

*/


// Funcion que debe ser invocada en el método "$(document).ready(function()" o "$(function() {" 
// de la página donde se deseee implementar 
function MensajeDosBotonesUnaAccionLoad() {

    $("#divMensajeDosBotonesUnaAccion").dialog({
        resizable: false,
        autoOpen: false,
        height: 300,
        width: 500,
        modal: true,
        buttons: {
            "Aceptar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#btnAceptarM2B1A').trigger("click");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

}

// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccion() {

    $("#divMensajeDosBotonesUnaAccion").dialog({
        resizable: false,
        autoOpen: true,
        height: 300,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#btnAceptarM2B1A').trigger("click");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}

/*  FIN MensajeDosBotonesUnaAccion  */
/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */


/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */
/*  Mensaje300x300 Mensaje sin contenido y tamaño 300 x 300 px  */

/*  Descripción: Función para mostrar una ventana modal con un mensaje 
Observaciones IMPORTANTES:
1.- En la página donde se consuma, debe existir un div de nombre divMensaje300x300, mismo que contiene
lo necesario

Ejemplo implementacion:

<script type="text/javascript">
$(function () {

Mensaje300x300Load();

});

function MuestraMensaje300() {

Mensaje300x300();
return false;

}
</script>

<body>
<br />
<asp:Button runat="server" ID="btnMostrarMensaje"  OnClientClick="return MuestraMensaje300()" />  
<br />
<div id="divMensaje300x300" title="Titulo del mensaje" style="display: none">
<p>
Aqui coloquen lo necesario para que funcione su control </p>
</div>
</body>

*/

function Mensaje300x300Load() {

    $("#divMensaje300x300").dialog({
        resizable: false,
        width: 300,
        height: 300,
        modal: true,
        autoOpen: false
    });

}


function Mensaje300x300() {

    $("#divMensaje300x300").dialog({
        resizable: false,
        width: 300,
        height: 300,
        modal: true,
        autoOpen: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            }
    });

}


/*  FIN Mensaje300x300  */
/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */


/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */
/*  Mensaje800x400 Mensaje sin contenido y tamaño 800 x 400 px  */

/*  Descripción: Función para mostrar una ventana modal con un mensaje 
Observaciones IMPORTANTES:
1.- En la página donde se consuma, debe existir un div de nombre divMensaje800x400, mismo que contiene
lo necesario

Ejemplo implementacion:

<script type="text/javascript">
$(function () {

Mensaje800x400Load();

});

function MuestraMensaje800x400() {

Mensaje800x400();
return false;

}
</script>

<body>
<br />
<asp:Button runat="server" ID="btnMostrarMensaje"  OnClientClick="return MuestraMensaje800x400()" />  
<br />
<div id="divMensaje800x400" title="Titulo del mensaje" style="display: none">
<p>
Aqui coloquen lo necesario para que funcione su control </p>
</div>
</body>

*/

function Mensaje800x400Load() {

    $("#divMensaje800x400").dialog({
        resizable: false,
        width: 800,
        height: 400,
        modal: true,
        autoOpen: false
    });

}


function Mensaje800x400() {

    $("#divMensaje800x400").dialog({
        resizable: false,
        width: 800,
        height: 400,
        modal: true,
        autoOpen: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            }
    });

}


/*  FIN Mensaje800x400  */
/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */



/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */
/*  Mensaje800x400 Mensaje sin contenido y tamaño 800 x 400 px  */

/*  Descripción: Función para mostrar una ventana modal con un mensaje 
Observaciones IMPORTANTES:
1.- En la página donde se consuma, debe existir un div de nombre divMensaje800x400Historial, mismo que contiene
lo necesario

Ejemplo implementacion:

<script type="text/javascript">
$(function () {

Mensaje800x400HistorialLoad();

});

function MuestraMensaje800x400() {

Mensaje800x400Historial();
return false;

}
</script>

<body>
<br />
<asp:Button runat="server" ID="btnMostrarMensaje"  OnClientClick="return MuestraMensaje800x400()" />  
<br />
<div id="divMensaje800x400" title="Titulo del mensaje" style="display: none">
<p>
Aqui coloquen lo necesario para que funcione su control </p>
</div>
</body>

*/

function Mensaje800x400HistorialLoad() {

    $("#divMensaje800x400Historial").dialog({
        resizable: false,
        width: 800,
        height: 400,
        modal: true,
        autoOpen: false,
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        }
    });

}


function Mensaje800x400Historial() {

    $("#divMensaje800x400Historial").dialog({
        resizable: false,
        width: 800,
        height: 400,
        modal: true,
        autoOpen: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        }
    });

}


/*  FIN Mensaje800x400  */
/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */



















/* ***************************************************************************************************************************** */
/*  SECCION PARA MENSAJES EXCLUSIVOS DE CONTROLES O PÁGINAS DE USO COMÚN, NO USAR EN LAS PÁGINAS DE NEGOCIO  */
/* ***************************************************************************************************************************** */



/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */
/*  Mensaje para ser usado por el control de Vencimiento de Sesiones NO USAR  */

function MensajeVencimientoSesionLoad() {

    $("#divConfirmacionVencimientoSesion").dialog({
        resizable: false,
        autoOpen: false,
        height: 300,
        width: 500,
        modal: true,
        buttons: {
            "Continuar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cerrar", disabled: true }]);
                $('#btnContinuarVencimientoSesion').trigger("click");
            },
            "Cerrar": function () {
                $(this).dialog("option", "buttons", [{ text: "Continuar", disabled: true }, { text: "Procesando...", disabled: true }]);
                $("#btnSalirVencimientoSesion").trigger("click");
            }
        }
    });

}

// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeVencimientoSesion() {

    $("#divConfirmacionVencimientoSesion").dialog({
        resizable: false,
        autoOpen: true,
        height: 300,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Continuar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cerrar", disabled: true }]);
                $('#btnContinuarVencimientoSesion').trigger("click");
            },
            "Cerrar": function () {
                $(this).dialog("option", "buttons", [{ text: "Continuar", disabled: true }, { text: "Procesando...", disabled: true }]);
                $("#btnSalirVencimientoSesion").trigger("click");
            }
        }
    });


    $("#divConfirmacionVencimientoSesion").dialog("open");

}

function divCargando() {

    $("#divCargandoProceso").dialog({
        resizable: false,
        autoOpen: true,
        height: 300,
        width: 500,
        modal: false,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            }
    });


    $("#divCargandoProceso").dialog("open");

}


/*  FIN Mensaje para ser usado por el control de Vencimiento de Sesiones NO USAR  */
/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */




/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */
/*  Mensaje para ser usado por el control de Filtros NO USAR  */

/*  
Ejemplo implementacion:

<script type="text/javascript">
$(function () {

MensajeVerificaFiltroLoad();

});

function MuestraVerificacionFiltro() {

MensajeVerificaFiltro();
return false;

}
</script>

<body>
<br />
<asp:Button runat="server" ID="btnMostrarMensaje"  OnClientClick="return MuestraVerificacionFiltro()" />  
<br />
<div id="divMensajeVerificaFiltro" title="Titulo del mensaje">
<p>
<asp:Label runat="server" ID="lblMensajeFiltro" Text="Aqui va el mensaje"></asp:Label> 
</p>
</div>
</body>

*/

function MensajeVerificaFiltroLoad() {

    $("#divMensajeVerificaFiltro").dialog({
        resizable: true,
        width: 500,
        height: 300,
        modal: true,
        autoOpen: false,
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        }
    });

}


function MensajeVerificaFiltro() {

    $("#divMensajeVerificaFiltro").dialog({
        resizable: true,
        width: 500,
        height: 300,
        modal: true,
        autoOpen: true,
        //closeText: 'Cerrar',
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        }
    });

}

//function MensajeVerificaFiltroLoadPen() {

//    $("#divMensajeVerificaFiltroPen").dialog({
//        resizable: true,
//        width: 500,
//        height: 300,
//        modal: true,
//        autoOpen: false,
//        buttons: {
//            "Aceptar": function () {
//                $(this).dialog("close");
//            }
//        }
//    });

//}


//function MensajeVerificaFiltroPen() {

//    $("#divMensajeVerificaFiltroPen").dialog({
//        resizable: true,
//        width: 500,
//        height: 300,
//        modal: true,
//        autoOpen: true,
//        closeText: 'Cerrar',
//        buttons: {
//            "Aceptar": function () {
//                $(this).dialog("close");
//            }
//        }
//    });

//}

/*  FIN Mensaje para ser usado por el control de Filtros NO USAR  */
/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */




/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */
/*  Mensaje para notificaciones NO USAR  */

/*  Descripción: Función para mostrar una ventana modal para las notificaciones 

*/

function MensajeNotificacionUsuarioLoad() {

    $("#divMensajeNotificacionUsuario").dialog({
        resizable: false,
        width: 800,
        height: 500,
        modal: true,
        autoOpen: false
    });

}


function MensajeNotificacionUsuario() {

    $("#divMensajeNotificacionUsuario").dialog({
        resizable: false,
        width: 800,
        height: 500,
        modal: true,
        autoOpen: true,
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
                $(".ui-dialog-titlebar-close").hide();
            }
    });

}


/*  FIN Mensaje para notificaciones NO USAR  */
/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */





/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */
/*  Mensaje para Personalizacion de Columans NO USAR  */

/*  Descripción: Función para mostrar una ventana modal para las notificaciones 

*/

function MensajePersonalizaColumnasControlLoad() {

    $("#divPersonalizaColumnasControlPadre").dialog({
        resizable: false,
        width: 710,
        height: 420,
        modal: true,
        autoOpen: false,
        buttons: {
            "Guardar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#btnGuardarPersonalizacion').trigger("click");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

}


function MensajePersonalizaColumnasControl() {

    $("#divPersonalizaColumnasControlPadre").dialog({
        resizable: false,
        width: 710,
        height: 420,
        modal: true,
        autoOpen: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Guardar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#btnGuardarPersonalizacion').trigger("click");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

}


/*  FIN Mensaje para notificaciones NO USAR  */
/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */





/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */
/*  Mensaje para Componente de Ayuda NO USAR  */

/*  Descripción: Función para mostrar una ventana modal para el componente de ayuda 

*/

function MensajeComponenteAyudaControlLoad() {

    $("#divMensajeComponenteAyudaControlPadre").dialog({
        resizable: false,
        width: 800,
        height: 500,
        modal: true,
        autoOpen: false
    });

}


function MensajeComponenteAyudaControl() {

    $("#divMensajeComponenteAyudaControlPadre").dialog({
        resizable: false,
        width: 800,
        height: 500,
        modal: true,
        autoOpen: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            }
    });

}


/*  FIN Mensaje para notificaciones NO USAR  */
/* ***************************************************************************************************************************** */
/* ***************************************************************************************************************************** */

function Deshabilita(btn) {
    btn.disabled = true;
    btn.value = 'Procesando...';
    __doPostBack(btn.name, "");
}

function DeshabilitaSP(btn) {
    btn.disabled = true;
}

function DeshabilitaPen(btn) {
    btn.disabled = true;
    btn.value = 'Procesando...';
    __doPostBack(btn.name, "");
    return false;
}

function DeshabilitaSPPen(btn) {
    btn.disabled = true;
    return false;
}


function MensajeUnBotonAgendaLoad() {

    $("#divMensajeUnBotonNoAccion").dialog({
        resizable: true,
        height: 400,
        width: 500,
        modal: true,
        autoOpen: false,
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        }
    });

}


function MensajeUnBotonAgenda() {

    $("#divMensajeUnBotonNoAccion").dialog({
        resizable: true,
        height: 400,
        width: 500,
        modal: true,
        autoOpen: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        }
    });

}



// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccion_CargarDocumentosInicio() {

    $("#divMensajeDosBotonesUnaAccion_CargarDocumentosInicio").dialog({
        resizable: false,
        autoOpen: true,
        height: 600,
        width: 1100,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            //"Cargar Archivos": function () {
            //    $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
            //    $('#btnCargaArchivo').trigger("click");
            //},
            "Cerrar": function () {
                $(this).dialog("close");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}

//Función que muestra la pregunta sobre si hay cambios de normativa
function MensajeDosBotonesUnaAccion_PreguntaNormativa(){
    $("#dvPreguntaCambioNormativa").dialog({
        resizable: false,
        autoOpen: true,
        height: 250,
        width: 400,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "SI": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#btnCambioNormativa').trigger("click");                
            },
            "NO": function () {
                $(this).dialog("close");
            }
        }
    });
}

// Función que debe ser invocada en el instante que se desee mostrar la ventana para ingresar fecha a agendar convocatoria
function MensajeDosBotonesUnaAccion_EnviarConvocatoria(nombreDiv, nombreBoton) {

        $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: true,
        height: 400,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#'+ nombreBoton).trigger("click");
                //$('#btnAceptarM2B1A').trigger("click");
            },
            "Cancelar": function () {
                $(this).dialog("close");
                //$('#btnAgendarM2').trigger("click");
                $('#btnCancelarM2B1A').trigger("click");
            }
        }
    });
}

// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccion_CancelarVisita() {

    $("#divMensajeDosBotonesUnaAccion_CancelarVisita").dialog({
        resizable: false,
        autoOpen: true,
        height: 400,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#btnAceptarM2B1A').trigger("click");
            },
            "Cancelar": function () {
                $(this).dialog("close");
                $('#btnCancelarM2B1A').trigger("click");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}


function Informacion_SolicitarProrroga() {

   $("#divInfoProrroga").dialog({
      resizable: false,
      autoOpen: true,
      height: 280,
      width: 550,
      modal: true,
      //closeText: 'Cerrar',
      open:
          function (event, ui) {
             $(this).parent().css('z-index', 3999);
             $(this).parent().appendTo(jQuery("form:last"));
             $('.ui-widget-overlay').css('position', 'fixed');
             $('.ui-widget-overlay').css('z-index', 3998);
             $('.ui-widget-overlay').appendTo($("form:last"));
          },
      buttons: {
         "SI": function () {
            $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
            $('#btnSiProrroga').trigger("click");
         },
         "NO": function () {
            $(this).dialog("close");
         }
      }
   });


   //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}

// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccion_SolicitarProrroga() {

    $("#divMensajeDosBotonesUnaAccion_SolicitarProrroga").dialog({
        resizable: false,
        autoOpen: true,
        height: 500,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#btnAceptarM2B1A').trigger("click");
            },
            "Cancelar": function () {
                $(this).dialog("close");
                $('#btnCancelarM2B1A').trigger("click");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}

// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccion_Redirect() {

    $("#divMensajeDosBotonesUnaAccion_Redirect").dialog({
        resizable: false,
        autoOpen: true,
        height: 300,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "SI": function () {
                $(this).dialog("close");
                $('#btnAceptarM2B1A').trigger("click");
            },
            "NO": function () {
                $(this).dialog("close");
                $('#btnCancelarM2B1A').trigger("click");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}


// Funcion para invocar el evento que borra un archivo del expediente
function ConfirmaRegistroVisitaLoad() {

    $("#divMensajeConfirmaRegistroVisita").dialog({
        resizable: false,
        autoOpen: true,
        height: 250,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $('#BtnConfirmaRegistro').trigger("click");
                $(this).dialog("close");
            }
        }
    });

    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}



// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccion_ConfirmarFechaInicioVisita() {

    $("#divMensajeDosBotonesUnaAccion_ConfirmarFechaInicioVisita").dialog({
        resizable: false,
        autoOpen: true,
        height: 350,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "ACEPTAR": function () {
                //$(this).dialog("close");
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "CANCELAR", disabled: true }]);
                $('#btnAceptarM2B1A').trigger("click");
            },
            "CANCELAR": function () {
                $(this).dialog("close");
                //$('#btnCancelarM2B1A').trigger("click");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}

// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccion_FinalizarPaso7() {

    $("#divMensajeDosBotonesUnaAccion_FinalizaPaso7").dialog({
        resizable: false,
        autoOpen: true,
        height: 300,
        width: 550,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                //$(this).dialog("close");
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Aceptar", disabled: true }]);
                $('#btnDetieneAvanza').trigger("click");
            },
            "Cancelar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                //$('#btnNoCierraPaso7').trigger("click");
                $(this).dialog("close");
            }
            //"CANCELAR": function () {
            //    $(this).dialog("close");
            //}
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}


function MensajeDosBotonesUnaAccion_SolicitarFechaFinVisita() {

    $("#divMensajeDosBotonesUnaAccion_SolicitarFechaFinVisita").dialog({
        resizable: false,
        autoOpen: true,
        height: 420,
        width: 550,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "ACEPTAR": function () {
                //$(this).dialog("close");
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "CANCELAR", disabled: true }]);
                $('#btnAceptarM2B1A').trigger("click");
            },
            "CANCELAR": function () {
                $(this).dialog("close");
                //$('#btnCancelarM2B1A').trigger("click");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}


// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
//YA NO SE USA SOLO QUEDA DE REFFERENCIA
function MensajeDosBotonesUnaAccion_SolicitarFechaEnviaDictamen() {

    $("#divMensajeDosBotonesUnaAccion_SolicitarFechaEnviaDictamen").dialog({
        resizable: false,
        autoOpen: true,
        height: 370,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "ACEPTAR": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "CANCELAR", disabled: true }]);
                $('#btnAceptarM2B1A').trigger("click");
            },
            "CANCELAR": function () {
                $(this).dialog("close");
                //$('#btnCancelarM2B1A').trigger("click");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}


// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccion_SolicitarFechaPosibleEmplazamiento() {

    $("#divMensajeDosBotonesUnaAccion_SolicitarFechaPosibleEmplazamiento").dialog({
        resizable: false,
        autoOpen: true,
        height: 370,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "ACEPTAR": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "CANCELAR", disabled: true }]);
                $('#btnAceptarM2B1A').trigger("click");
            },
            "CANCELAR": function () {
                $(this).dialog("close");
                //$('#btnCancelarM2B1A').trigger("click");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}


// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccion_SolicitarFechaAcuseAforeRecibeOfEmpl() {

    $("#divMensajeDosBotonesUnaAccion_SolicitarFechaAcuseAforeRecibeOfEmpl").dialog({
        resizable: false,
        autoOpen: true,
        height: 400,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "ACEPTAR": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "CANCELAR", disabled: true }]);
                $('#btnAceptarM2B1A').trigger("click");
            },
            "CANCELAR": function () {
                $(this).dialog("close");
                //$('#btnCancelarM2B1A').trigger("click");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}



// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccion_SolicitarFechaAcuseAforeContestoOfEmpl() {

    $("#divMensajeDosBotonesUnaAccion_SolicitarFechaAcuseAforeContestoOfEmpl").dialog({
        resizable: false,
        autoOpen: true,
        height: 400,
        width: 550,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "ACEPTAR": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "CANCELAR", disabled: true }]);
                $('#btnAceptarM2B1A').trigger("click");
            },
            "CANCELAR": function () {
                $(this).dialog("close");
                //$('#btnCancelarM2B1A').trigger("click");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}


// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccion_SolicitarFechaImposicionSancion() {

    $("#divMensajeDosBotonesUnaAccion_SolicitarFechaImposicionSancion").dialog({
        resizable: false,
        autoOpen: true,
        height: 370,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "ACEPTAR": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "CANCELAR", disabled: true }]);
                $('#btnAceptarM2B1A').trigger("click");
            },
            "CANCELAR": function () {
                $(this).dialog("close");
                //$('#btnCancelarM2B1A').trigger("click");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}



// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccion_SolicitarFechaImposicionSancionEstimada() {

    $("#divMensajeDosBotonesUnaAccion_SolicitarFechaImposicionSancionEstimada").dialog({
        resizable: false,
        autoOpen: true,
        height: 370,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "ACEPTAR": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "CANCELAR", disabled: true }]);
                $('#btnAceptarM2B1A').trigger("click");
            },
            "CANCELAR": function () {
                $(this).dialog("close");
                //$('#btnCancelarM2B1A').trigger("click");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}


// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccion_SolicitarFechaAcusePagoAfore() {

    $("#divMensajeDosBotonesUnaAccion_SolicitarFechaAcusePagoAfore").dialog({
        resizable: false,
        autoOpen: true,
        height: 370,
        width: 500,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "ACEPTAR": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "CANCELAR", disabled: true }]);
                $('#btnAceptarM2B1A').trigger("click");
            },
            "CANCELAR": function () {
                $(this).dialog("close");
                //$('#btnCancelarM2B1A').trigger("click");
            }
        }
    });


    //    $("#divMensajeDosBotonesUnaAccion").dialog("open");

}


function MensajeDosBotonesUnaAccionAgcLoad(nombreDiv, nombreBoton, x, y) {

    $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: false,
        height: y,
        width: x,
        modal: true,
        buttons: {
            "Aceptar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#' + nombreBoton).trigger("click");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

}

// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesUnaAccionAgc(nombreDiv, nombreBoton, x, y) {

    $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: true,
        height: y,
        width: x,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#' + nombreBoton).trigger("click");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });
}

function MensajeDosBotonesUnaAccionMCS(nombreDiv, nombreBoton, x, y) {

   $("#" + nombreDiv).dialog({
      resizable: false,
      autoOpen: true,
      height: y,
      width: x,
      modal: true,
      //closeText: 'Cerrar',
      open:
          function (event, ui) {
             $(this).parent().css('z-index', 3999);
             $(this).parent().appendTo(jQuery("form:last"));
             $('.ui-widget-overlay').css('position', 'fixed');
             $('.ui-widget-overlay').css('z-index', 3998);
             $('.ui-widget-overlay').appendTo($("form:last"));
          },
      buttons: {
         "Aceptar": function () {
            $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
            $('#' + nombreBoton).trigger("click");
            $(this).dialog("close");
         },
         "Cancelar": function () {
            $(this).dialog("close");
         }
      }
   });
}

// Funcion que debe ser invocada en el instante que se desee mostrar la ventana
function MensajeDosBotonesDosAccionRRA(nombreDiv, nombreBotonAceptar,nombreBontonCancelar, x, y) {

    $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: true,
        height: y,
        width: x,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#' + nombreBotonAceptar).trigger("click");
            },
            "Cancelar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#' + nombreBontonCancelar).trigger("click");
            }
        }
    });
}

function MensajeTresBotonesTresAccionRRA(nombreDiv, nombreBotonAceptar, nombreBotonNo, nombreBontonCancelar, x, y) {

    $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: true,
        height: y,
        width: x,
        modal: true,
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Si": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#' + nombreBotonAceptar).trigger("click");
            },
            "No": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#' + nombreBotonNo).trigger("click");
            },
            "Cancelar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                $('#' + nombreBontonCancelar).trigger("click");
            }
        }
    });
}

//Funcion general para confirmacion
function Confirmacion(nombreDiv, nombreBoton, x, y) {

    $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: true,
        height: y,
        width: x,
        modal: true,
        //closeText: 'Cerrar',
        closeOnEscape: false,
        dialogClass: "noclose",
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }]);

                $('#' + nombreBoton).trigger("click");

                if (nombreBoton != "btnConfirmarDocsSI")
                    $(this).dialog("close");
            }
        }
    });
}


function ConfirmacionNoClose(nombreDiv, nombreBoton, x, y) {

    $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: true,
        height: y,
        width: x,
        modal: true,
        //closeText: 'Cerrar',
        closeOnEscape: false,
        beforeClose: function (event, ui) { return false; },
        dialogClass: "noclose",
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }]);

                $('#' + nombreBoton).trigger("click");

                if (nombreBoton != "btnConfirmarDocsSI")
                    $(this).dialog("close");
            }
        }
    });
}


//Funcion generica para dar un aviso
function Aviso(nombreDiv, x, y) {
    $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: true,
        height: y,
        width: x,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
                return true;
            }
        }
    });
}

//Funcion general para confirmacion
//ejecutarTriggerBtnNo, sirve para llamar a un codigo de servidor en el boton no con el valor 1, si viene en 0 solo cierra la modal
function ConfirmacionSiNo(nombreDiv, nombreBotonSi, nombreBotonNo, ejecutarTriggerBtnNo, x, y) {

    $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: true,
        height: y,
        width: x,
        modal: true,
        //closeText: 'Cerrar',
        closeOnEscape: false,
        dialogClass: "noclose",
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "SI": function () {
                if (nombreBotonSi == "imgRevisarDocs" || nombreBotonSi == "btnPasoDiezConfSancionSi") {
                    $(this).dialog("close");
                    $('#' + nombreBotonSi).trigger("click");
                } else {
                    $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "NO", disabled: true }]);
                    $('#' + nombreBotonSi).trigger("click");
                }
            },
            "NO": function () {
                if (nombreBotonNo == 'btnPasoSieteConfirmNoSnPostback') { //TRUQUEARLO PARA LA FUNCIONALIDAD ESPECIFICA SE SANCION EN PASO 7
                    $(this).dialog("close");
                    $('#' + nombreBotonNo).trigger("click");
                } else {

                    if (ejecutarTriggerBtnNo == 1) {
                        $(this).dialog("option", "buttons", [{ text: "SI", disabled: true }, { text: "Procesando...", disabled: true }]);
                        $('#' + nombreBotonNo).trigger("click");
                    } else {
                        $(this).dialog("close");
                    }
                }
            }
        }
    });
}

//Funcion general para confrimacion(Aceptar, Cancelar)
function ConfirmacionAceptarCancelar(nombreDiv, nombreBotonSi, nombreBotonNo, ejecutarTriggerBtnNo, x, y) {
    debugger;
    $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: true,
        height: y,
        width: x,
        modal: true,
        //closeText: 'Cerrar',
        closeOnEscape: false,
        dialogClass: "noclose",
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                debugger;
                if (nombreBotonSi == "imgRevisarDocs" || nombreBotonSi == "btnPasoDiezConfSancionSi") {
                    $(this).dialog("close");
                    $('#' + nombreBotonSi).trigger("click");
                } else {
                    $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "NO", disabled: true }]);
                    $('#' + nombreBotonSi).trigger("click");
                }
            },
            "Cancelar": function () {
                debugger;
                if (nombreBotonNo == 'btnPasoSieteConfirmNoSnPostback') { //TRUQUEARLO PARA LA FUNCIONALIDAD ESPECIFICA SE SANCION EN PASO 7
                    $(this).dialog("close");
                    $('#' + nombreBotonNo).trigger("click");
                } else {

                    if (ejecutarTriggerBtnNo == 1) {
                        $(this).dialog("option", "buttons", [{ text: "SI", disabled: true }, { text: "Procesando...", disabled: true }]);
                        $('#' + nombreBotonNo).trigger("click");
                    } else {
                        $(this).dialog("close");
                    }
                }
            }
        }
    });
}

function ConfirmacionSiNoLoad(nombreDiv, nombreBotonSi, nombreBotonNo, ejecutarTriggerBtnNo, x, y) {
    $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: false,
        height: y,
        width: x,
        modal: true,
        //closeText: 'Cerrar',
        closeOnEscape: false,
        dialogClass: "noclose",
        buttons: {
            "SI": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "NO", disabled: true }]);
                $('#' + nombreBotonSi).trigger("click");
                //$(this).dialog("close");
            },
            //"NO": function () {
            //    if (ejecutarTriggerBtnNo == 1) {
            //        $(this).dialog("option", "buttons", [{ text: "SI", disabled: true }, { text: "Procesando...", disabled: true }]);
            //        $('#' + nombreBotonNo).trigger("click");
            //    } else {
            //        $(this).dialog("close");
            //    }
            //}
        }
    });
}


function ConfirmacionvJ(nombreDiv, nombreBotonSi, nombreBotonNo, ejecutarTriggerBtnNo, nombreBotonNoDos, x, y) {

    $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: true,
        height: y,
        width: x,
        modal: true,
        //closeText: 'Cerrar',
        closeOnEscape: false,
        dialogClass: "noclose",
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "SI": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "NO", disabled: true }, { text: "NO SE LLEVÓ A CABO", disabled: true }]);
                $('#' + nombreBotonSi).trigger("click");
                //$(this).dialog("close");
            },
            "NO": function () {
                //$(this).dialog("close");
                if (ejecutarTriggerBtnNo == 1) {
                    //$(this).dialog("option", "buttons", [{ text: "SI", disabled: true }, { text: "Procesando...", disabled: true }, { text: "NO SE LLEVÓ A CABO", disabled: true }]);
                    $('#' + nombreBotonNo).click();
                    //$('#' + nombreBotonNo).trigger("click");
                }
				//$(this).dialog("close");
            },
            "NO SE LLEVÓ A CABO": function () {
                $(this).dialog("option", "buttons", [{ text: "SI", disabled: true }, { text: "NO", disabled: true }, { text: "Procesando...", disabled: true }]);
                $('#' + nombreBotonNoDos).trigger("click");
                //$(this).dialog("close");
            }
        }
    });
}


function ConfirmacionAlertaPaso18(nombreDiv, nombreBotonSi, nombreBotonNo, x, y) {

    $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: true,
        height: y,
        width: x,
        modal: true,
        //closeText: 'Cerrar',
        closeOnEscape: false,
        dialogClass: "noclose",
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "SI": function () {
                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "NO", disabled: true }]);
                $('#' + nombreBotonSi).trigger("click");
                //$(this).dialog("close");
            },
            "NO": function () {
                $(this).dialog("option", "buttons", [{ text: "SI", disabled: true }, { text: "Procesando...", disabled: true }]);
                $('#' + nombreBotonNo).trigger("click");
                //$(this).dialog("close");
            }
        }
    });
}


//Funcion agregada ammm mayo 2021 para confirmación reunión con presidencia que ya no se lleva 
function Continuar(nombreDiv,nombreBotonNo, x, y) {
    $("#" + nombreDiv).dialog({
        resizable: false,
        autoOpen: true,
        height: y,
        width: x,
        modal: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Continuar": function () {
                $(this).dialog("option", "buttons", [{ text: "Continuar", disabled: true }, { text: "Procesando...", disabled: true }]);
                $('#' + nombreBotonNo).trigger("click");
            }
        }
    });
}
//Bloqueo de pantalla con capa de humo
function jsShowWindowLoad(mensaje) {
    //alert(mensaje);
    //eliminamos si existe un div ya bloqueando
    jsRemoveWindowLoad();
    
    document.getElementsByTagName("html")[0].style.overflow = "hidden";

    //si no enviamos mensaje se pondra este por defecto
    //if (mensaje === undefined) mensaje = "Procesando la información<br>Espere por favor";
    if (mensaje === undefined) mensaje = "";
    //centrar imagen gif
    height = 200;//El div del titulo, para que se vea mas arriba (H)
    var ancho = 0;
    var alto = 0;

    //obtenemos el ancho y alto de la ventana de nuestro navegador, compatible con todos los navegadores
    if (window.innerWidth == undefined) ancho = window.screen.width;
    else ancho = window.innerWidth;
    if (window.innerHeight == undefined) alto = window.screen.height;
    else alto = window.innerHeight;

    //operación necesaria para centrar el div que muestra el mensaje
    var heightdivsito = alto / 2 - parseInt(height) / 2;//Se utiliza en el margen superior, para centrar
    //var heightdivsito = (alto / 2) - ;//Se utiliza en el margen superior, para centrar

    //imagen que aparece mientras nuestro div es mostrado y da apariencia de cargando
    imgCentro = "<div style='text-align:center;height:" + alto + "px;'><div  style='color:#000;margin-top:" + heightdivsito + "px; font-size:20px;font-weight:bold'><img  src='../Imagenes/loading.gif' width='130' height='111'><br/>" + mensaje + "</div></div>";
    //imgCentro = "<div style='text-align:center;height:" + alto + "px;'><div  style='color:#000;margin-top:" + heightdivsito + "px; font-size:20px;font-weight:bold'>" + mensaje + "</div></div>";

    //creamos el div que bloquea grande------------------------------------------
    div = document.createElement("div");
    div.id = "WindowLoad"
    div.style.backgroundColor = "gray";
    div.style.opacity = "0.5";
    div.style.filter = "alpha(opacity=50)";
    div.style.width = ancho + "px";
    div.style.height = alto + "px";
    $("body").append(div);
    
    //creamos un input text para que el foco se plasme en este y el usuario no pueda escribir en nada de atras
    input = document.createElement("input");
    input.id = "focusInput";
    input.type = "text"

    //asignamos el div que bloquea
    $("#WindowLoad").append(input);

    //asignamos el foco y ocultamos el input text
    $("#focusInput").focus();
    $("#focusInput").hide();

    //centramos el div del texto
    $("#WindowLoad").html(imgCentro);
}

function jsShowWindowLoadRegistro(mensaje) {
    //alert(mensaje);
    //eliminamos si existe un div ya bloqueando
    jsRemoveWindowLoadRegistro();

    document.getElementsByTagName("html")[0].style.overflow = "hidden";

    //si no enviamos mensaje se pondra este por defecto
    //if (mensaje === undefined) mensaje = "Procesando la información<br>Espere por favor";
    if (mensaje === undefined) mensaje = "";
    //centrar imagen gif
    height = 200;//El div del titulo, para que se vea mas arriba (H)
    var ancho = 0;
    var alto = 0;

    //obtenemos el ancho y alto de la ventana de nuestro navegador, compatible con todos los navegadores
    if (window.innerWidth == undefined) ancho = window.screen.width;
    else ancho = window.innerWidth;
    if (window.innerHeight == undefined) alto = window.screen.height;
    else alto = window.innerHeight;

    //operación necesaria para centrar el div que muestra el mensaje
    var heightdivsito = alto / 2 - parseInt(height) / 2;//Se utiliza en el margen superior, para centrar
    //var heightdivsito = (alto / 2) - ;//Se utiliza en el margen superior, para centrar

    //imagen que aparece mientras nuestro div es mostrado y da apariencia de cargando
    imgCentro = "<div style='text-align:center;height:" + alto + "px;'><div  style='color:#000;"+
                "font-size:20px;font-weight:bold'><div style='height: 250px!important;'></div>"+
                "<img  src='../Imagenes/loading.gif' width='130' height='111'><br/>" + mensaje + "</div></div>";

    //creamos el div que bloquea grande------------------------------------------
    div = document.createElement("div");
    div.id = "WindowLoadRegistro"
    div.style.backgroundColor = "gray";
    div.style.opacity = "0.5";
    div.style.filter = "alpha(opacity=50)";
    div.style.width = ancho + "px";
    div.style.height = alto + "px";
    $("body").append(div);

    //creamos un input text para que el foco se plasme en este y el usuario no pueda escribir en nada de atras
    input = document.createElement("input");
    input.id = "focusInput";
    input.type = "text"

    //asignamos el div que bloquea
    $("#WindowLoadRegistro").append(input);

    //asignamos el foco y ocultamos el input text
    $("#focusInput").focus();
    $("#focusInput").hide();

    //centramos el div del texto
    $("#WindowLoadRegistro").html(imgCentro);
}

//Desbloqueo de pantalla con capa de humo
function jsRemoveWindowLoad() {
    // eliminamos el div que bloquea pantalla
    $("#WindowLoad").remove();
    document.getElementsByTagName("html")[0].style.overflow = "auto";
}

function jsRemoveWindowLoadRegistro() {
    // eliminamos el div que bloquea pantalla
    $("#WindowLoadRegistro").remove();
    document.getElementsByTagName("html")[0].style.overflow = "auto";
}

//PARA LA IMAGEN DE CARGA
function MuestraImgCarga(btnPrecionado) {
    try {
        //if (btnPrecionado != null)
        //    $("#" + btnPrecionado.id).attr('disabled', 'disabled');
        jsShowWindowLoad("Procesando....");
        /*var divHtml = document.getElementById('divCargaAgc');
        if (divHtml != null)
            document.getElementById('divCargaAgc').style.display = 'block';*/
    }catch(e){
        alert('error:' + e);
    }

    return true;
}

function MuestraImgCargaRegistro(btnPrecionado) {
    try {
        jsShowWindowLoadRegistro("Procesando....");
    } catch (e) {
        alert('error:' + e);
    }

    return true;
}

function OcultaImagenCargaRegistro() {
    jsRemoveWindowLoadRegistro();
}

function OcultaImagenCarga() {
    jsRemoveWindowLoad();
    /*var divHtml = document.getElementById('divCargaAgcModal');
    if (divHtml != null)
        document.getElementById('divCargaAgcModal').style.display = 'none';
    var divHtml = document.getElementById('divCargaAgc');
    if (divHtml != null)
        document.getElementById('divCargaAgc').style.display = 'none';*/
}

function DivOpcionesImagenesProcesos() {

    $("#dvModalImagenesProcesos").dialog({
        resizable: true,
        height: 300,
        width: 500,
        modal: true,
        autoOpen: true,
        //closeText: 'Cerrar',
        open:
            function (event, ui) {
                $(this).parent().css('z-index', 3999);
                $(this).parent().appendTo(jQuery("form:last"));
                $('.ui-widget-overlay').css('position', 'fixed');
                $('.ui-widget-overlay').css('z-index', 3998);
                $('.ui-widget-overlay').appendTo($("form:last"));
            },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        }
    });

}

function ReemplazaCEspeciales(idCtrl) {

   $('#' + idCtrl + '').on('input', function (e) {
      if (!/^[ a-z0-9áéíóúüñ\/@,;+*%$-]*$/i.test(this.value)) {
         this.value = this.value.replace(/[^ a-z0-9áéíóúüñ\/@,;+*%$-]+/ig, "");
      }
   });
}