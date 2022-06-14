<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExpedientePC.aspx.vb" Inherits="SEPRIS.ExpedientePC" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<script src="/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
<script src="/Scripts/jquery-ui-1.10.3.custom.js" type="text/javascript"></script>
<script src="/Scripts/MensajeModal.js" type="text/javascript"></script>
<body>
    <link href="/Styles/jquery-ui-1.10.3.custom.css" rel="stylesheet" type="text/css" />
    <style>
        input[type="file"] {
            display: none;
        }

        #WindowLoad {
            position: fixed;
            top: 0px;
            left: 0px;
            z-index: 3200;
            filter: alpha(opacity=65);
            opacity: 0.65;
            background: #999;
        }

        #DropZone {
            margin-top: 40px;
            margin-bottom: 20px;
            padding: 100px 0 80px 0;
            text-align: center;
            border: dashed 3px gray;
            background-image: url(../imagenes/cloud-upload1.png);
            background-repeat: no-repeat;
            background-position: 50% 40px;
            width: 650px;
        }
    </style>

    <%@ Register Src="~/Controles/ucFiltro2.ascx" TagName="ucFiltro2" TagPrefix="uc1" %>
    <form id="form1" style="width: 100%;" runat="server">
        <div>

            <script type="text/javascript">

                function UploadProcess(EsNuevo) {
                    debugger
                    var data = new FormData();
                    data.append("Usuario", "<%=Usuario%>");
                    data.append("Folio", <%=Folio%>);
                    data.append("IdDocumento", $("#<%=hdIdDocumento.ClientID%>").val());
                    data.append("EsNuevo", EsNuevo);
                    data.append(selectedName, selectedFile);


                    jQuery.ajax({
                        type: "POST",
                        url: "UploadFilePC.ashx",
                        contentType: false,
                        processData: false,
                        async: false,
                        data: data,
                        success: function (data) {
                            
                            $("#<%=btnActulizarGrid.ClientID%>").click();
                            var pnlupd = window.parent.$("div[id*='upnlReqInfo']")
                            if (pnlupd.length > 0) {
                                pnlupd = pnlupd[0].id
                                window.parent.__doPostBack(pnlupd, '');
                            }
                        }
                    });

                    }

                    function AgregarFirma() {
                        if ($("#<%=lstUsuariosFirma.ClientID%> option:selected").length > 0) {
                            $("#<%=lstFirmas.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstUsuariosFirma.ClientID%> option:selected").val()).text($("#<%=lstUsuariosFirma.ClientID%> option[value='" + $("#<%=lstUsuariosFirma.ClientID%>").val() + "']").text()));
                            $("#<%=lstFirmas.ClientID%>").val($("#<%=lstUsuariosFirma.ClientID%>").val());
                            $("#<%=lstUsuariosFirma.ClientID%> option:selected").remove();
                        }
                        else {
                            $('#divTextoAlerta').html('Selecciona una firma');
                            $("#divAlertaGeneral").dialog({
                                resizable: false,
                                autoOpen: true,
                                height: 300,
                                width: 500,
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
                                    "Cerrar": function () {
                                        $(this).dialog("close");
                                    },
                                }
                            });
                        }

                    }

                    function QuitarFirma() {
                    
                        if ($("#<%=lstFirmas.ClientID%> option:selected").length > 0) {
                            $("#<%=lstUsuariosFirma.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstFirmas.ClientID%> option:selected").val()).text($("#<%=lstFirmas.ClientID%> option[value='" + $("#<%=lstFirmas.ClientID%>").val() + "']").text()));
                            $("#<%=lstFirmas.ClientID%> option:selected").remove();

                        }
                        else {
                            $('#divTextoAlerta').html('Selecciona una firma');
                            $("#divAlertaGeneral").dialog({
                                resizable: false,
                                autoOpen: true,
                                height: 300,
                                width: 500,
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
                                    "Cerrar": function () {
                                        $(this).dialog("close");
                                    },
                                }
                            });
                        }

                    }

                    function AgregarRubrica() {
                        if ($("#<%=lstUsuariosRubrica.ClientID%> option:selected").length > 0) {
                        $("#<%=lstRubricas.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstUsuariosRubrica.ClientID%>").val()).text($("#<%=lstUsuariosRubrica.ClientID%> option[value='" + $("#<%=lstUsuariosRubrica.ClientID%>").val() + "']").text()));
                        $("#<%=lstRubricas.ClientID%>").val($("#<%=lstUsuariosRubrica.ClientID%>").val());
                        $("#<%=lstUsuariosRubrica.ClientID%> option:selected").remove();
                    }
                    else {
                        $('#divTextoAlerta').html('Selecciona una rubrica');
                        $("#divAlertaGeneral").dialog({
                            resizable: false,
                            autoOpen: true,
                            height: 300,
                            width: 500,
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
                                "Cerrar": function () {
                                    $(this).dialog("close");
                                },
                            }
                        });
                    }

                    return false;
                }

                function QuitarRubrica() {
                    if ($("#<%=lstRubricas.ClientID%> option:selected").length > 0) {
                        $("#<%=lstUsuariosRubrica.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstRubricas.ClientID%>").val()).text($("#<%=lstRubricas.ClientID%> option[value='" + $("#<%=lstRubricas.ClientID%>").val() + "']").text()));
                        $("#<%=lstRubricas.ClientID%> option:selected").remove();

                    }
                    else {
                        $('#divTextoAlerta').html('Selecciona una rubrica');
                        $("#divAlertaGeneral").dialog({
                            resizable: false,
                            autoOpen: true,
                            height: 300,
                            width: 500,
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
                                "Cerrar": function () {
                                    $(this).dialog("close");
                                },
                            }
                        });
                    }

                    return false;
                }


                function SubirArchivo(IdDocumento) {
                    //alert(IdDocumento)
                    
                    $("#<%=hdIdDocumento.ClientID%>").val(IdDocumento);
                    $("#divCargaDocumento").dialog({
                        resizable: false,
                        autoOpen: true,
                        height: 500,
                        width: 700,
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
                            "Aceptar": function () {
                                //revisar si esto funciona AMMM 09/052019                              
                                UploadProcess(1); //este si es código original de ODT07
                                jsShowWindowLoad("Procesando....");
                                $(this).dialog("close");  //este si es código original de ODT07                               
                                // fin AMMM
                            },
                            "Cancelar": function () {
                                $(this).dialog("close");
                            }
                        }
                    });

                }

                //AMMM agregado para bloquera pantalla al subiry/o reemmplazar un doc 13/05/2019
                function jsShowWindowLoad(mensaje) {
                    //alert(mensaje);
                    //eliminamos si existe un div ya bloqueando
                    jsRemoveWindowLoad();

                    document.getElementsByTagName("html")[0].style.overflow = "hidden";

                    //si no enviamos mensaje se pondra este por defecto
                    if (mensaje === undefined) mensaje = "";
                    //centrar imagen gif
                    height = 500;//El div del titulo, para que se vea mas arriba (H)
                    var ancho = 0;
                    var alto = 0;

                    //obtenemos el ancho y alto de la ventana de nuestro navegador, compatible con todos los navegadores
                    if (window.innerWidth == undefined)
                        ancho = window.screen.width;
                    else ancho = window.innerWidth;
                    if (window.innerHeight == undefined)
                        alto = window.screen.height;
                    else alto = window.innerHeight;

                    //operación necesaria para centrar el div que muestra el mensaje
                    var heightdivsito = alto / 2 - parseInt(height) / 2;


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
                    //div.style.height = height + "px";
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
                //AMMM fin código

                function ReemplazarArchivo(IdDocumento) {
                    
                    $("#<%=hdIdDocumento.ClientID%>").val(IdDocumento);
                    $("#divCargaDocumento").dialog({
                        resizable: false,
                        autoOpen: true,
                        height: 500,
                        width: 700,
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
                            "Aceptar": function () {
                                UploadProcess(0);
                                jsShowWindowLoad("Procesando...."); //ammm 14/05/2019
                                $(this).dialog("close");
                            },
                            "Cancelar": function () {
                                $(this).dialog("close");
                            }
                        }
                    });

                }

                function RegistroSICOD(IdCatDocumento, IdDocumento, idTipoOficio) {
                    $("#<%=hdRegistrar.ClientID%>").val("Registrar")
                    //alert(IdCatDocumento)
                    //alert(IdDocumento)
                    //alert(idTipoOficio)
                    
                    $("#<%=hdCatDocumento.ClientID%>").val(IdCatDocumento);
                    $("#<%=hdIdDocumento.ClientID%>").val(IdDocumento);
                    $("#<%=hdIdTipoOficio.ClientID%>").val(idTipoOficio);

                    $("#txtAsuntoOficio").val("");
                    $("#txtComentariosOficio").val("");
                    $("#txtDestinatarioOficio").val("");
                    $("#ddlPuestoDestinatario").val("-1");
                    $("#ddlAreaFirma").val("-1");
                    $("#ddlAreaOficio").val("-1");
                    $('#lstUsuariosFirma').children().remove();
                    $('#lstFirmas').children().remove();
                    $("#ddlAreaRubrica").val("-1");
                    $('#lstUsuariosRubrica').children().remove();
                    $('#lstRubricas').children().remove();

                    $("#divRegistroSICOD").dialog({
                        resizable: false,
                        autoOpen: true,
                        height: 500,
                        width: 700,
                        modal: true,
                        title: "Registro SICOD",
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
                                var err = '<ul>';
                                var conterr = 0;
                                if ($("#txtAsuntoOficio").val() == "") {
                                    conterr++;
                                    err += "<li>El asunto es obligatorio.</li>";
                                }
                                if ($("#txtComentariosOficio").val() == "") {
                                    conterr++;
                                    err += "<li>Los comentarios son obligatorios.</li>";
                                }
                                if ($("#txtDestinatarioOficio").val() == "") {
                                    conterr++;
                                    err += "<li>El destinatario es obligatorio.</li>";
                                }
                                if ($("#ddlPuestoDestinatario").val() == "-1") {
                                    conterr++;
                                    err += "<li>El puesto es obligatorio.</li>";
                                }
                                if ($("#lstFirmas option").length == 0) {
                                    conterr++;
                                    err += "<li>Debe haber al menos una firma.</li>";
                                }
                                err += "</ul>";

                                if (conterr == 0) {
                                    ConfirmacionAltaSICOD(1, '');
                                }
                                else {
                                    ConfirmacionAltaSICOD(0, err);
                                }

                            },
                            "Cancelar": function () {
                                ConfirmacionAltaSICOD(0);
                            }
                        }
                    });

                }

                function ObtenerFirmas(Origen) {
                    var EsSIE = 0;
                    if ($("#<%=chkFiltroSIE.ClientID%>").prop('checked')) {
                        EsSIE = 1;
                    }

                    if (Origen == 1) {
                        param_data = '{Area: ' + $("#<%=ddlAreaFirma.ClientID%>").val() + ', EsSIE: ' + EsSIE + ' }'
                    }
                    else {
                        param_data = '{Area: ' + $("#<%=ddlAreaRubrica.ClientID%>").val() + ', EsSIE: ' + EsSIE + ' }'
                    }

                    $.ajax({
                        type: "POST",
                        url: "ServiciosSICOD.aspx/ObtenerUsuariosParaFirma",
                        contentType: "application/json;charset=utf-8",
                        data: param_data,
                        dataType: "json",
                        success: function (data) {

                            if (Origen == 1) {
                                $("#<%=lstUsuariosFirma.ClientID%>").html("")
                                $("#<%=lstFirmas.ClientID%>").html("")
                                $.each(data.d, function () {
                                    $("#<%=lstUsuariosFirma.ClientID%>").append($("<option></option>").attr("value", this['Value']).text(this['Text']));
                                });
                            }
                            else {
                                $("#<%=lstUsuariosRubrica.ClientID%>").html("")
                                $("#<%=lstRubricas.ClientID%>").html("")
                                $.each(data.d, function () {
                                    $("#<%=lstUsuariosRubrica.ClientID%>").append($("<option></option>").attr("value", this['Value']).text(this['Text']));
                                });
                            }


                        },
                        error: function (result) {
                            return result
                        }
                    });

                }



                function CrearFolio() {
                    var Registrar = $("#<%=hdRegistrar.ClientID%>").val()
                    //var Registrar = document.getElementById('btnRegistroSICOD').textContent
                    var Firmas = '';
                    var Rubricas = '';
                    var EsSIE = 0;
                    $("#<%=lstFirmas.ClientID%> option").each(function (i, selected) {
                        Firmas += $(selected).val() + ',';
                    });
                    $("#<%=lstRubricas.ClientID%> option").each(function (i, selected) {
                        Rubricas += $(selected).val() + ',';
                    });

                    if ($("#<%=chkFiltroSIE.ClientID%>").prop('checked')) {
                        EsSIE = 1;
                    }

                    //param_data = '{Folio:' + <%=Folio%> + ', IdCatDocumento:' + $("#<%=hdCatDocumento.ClientID%>").val() + ', IdDocumento: ' + $("#<%=hdIdDocumento.ClientID%>").val() + ', Firmas: "' + Firmas + '", Rubricas: "' + Rubricas + '", Destinatario: "' + $("#<%=txtDestinatarioOficio.ClientID%>").val() + '", Puesto: ' + $("#<%=ddlPuestoDestinatario.ClientID%>").val() + ', EsSIE: ' + EsSIE + ', Asunto: "' + $("#<%=txtAsuntoOficio.ClientID%>").val() + '", Comentarios: "' + $("#<%=txtComentariosOficio.ClientID%>").val() + '", UsuarioElaboro: "' + '<%=Usuario%>' + '", Area: "' + $("#<%=ddlAreaOficio.ClientID%>").val() + '", Proceso:1 }';

                 <%--   alert( <%=Folio%>)
                    alert($("#<%=hdCatDocumento.ClientID%>").val())
                    alert($("#<%=hdIdTipoOficio.ClientID%>").val())--%>


                    if (Registrar = "Registrar")
                        param_data = '{Folio:' + <%=Folio%> + ', IdCatDocumento:' + $("#<%=hdCatDocumento.ClientID%>").val() + ', IdDocumento: ' + $("#<%=hdIdTipoOficio.ClientID%>").val() + ', Firmas: "' + Firmas + '", Rubricas: "' + Rubricas + '", Destinatario: "' + $("#<%=txtDestinatarioOficio.ClientID%>").val() + '", Puesto: ' + $("#<%=ddlPuestoDestinatario.ClientID%>").val() + ', EsSIE: ' + EsSIE + ', Asunto: "' + $("#<%=txtAsuntoOficio.ClientID%>").val() + '", Comentarios: "' + $("#<%=txtComentariosOficio.ClientID%>").val() + '", UsuarioElaboro: "' + '<%=Usuario%>' + '", Area: "' + $("#<%=ddlAreaOficio.ClientID%>").val() + '", TipoDocumentoP: "' + $("#<%=hdIdTipoOficio.ClientID%>").val() + '", Clasificacion:0, Paso: 0, Proceso:3 }';
                    else
                        param_data = '{Folio:' + <%=Folio%> + ', IdCatDocumento:' + $("#<%=hdCatDocumento.ClientID%>").val() + ', IdDocumento: ' + $("#<%=hdIdTipoOficio.ClientID%>").val() + ', Firmas: "' + Firmas + '", Rubricas: "' + Rubricas + '", Destinatario: "' + $("#<%=txtDestinatarioOficio.ClientID%>").val() + '", Puesto: ' + $("#<%=ddlPuestoDestinatario.ClientID%>").val() + ', EsSIE: ' + EsSIE + ', Asunto: "' + $("#<%=txtAsuntoOficio.ClientID%>").val() + '", Comentarios: "' + $("#<%=txtComentariosOficio.ClientID%>").val() + '", UsuarioElaboro: "' + '<%=Usuario%>' + '", Area: "' + $("#<%=ddlAreaOficio.ClientID%>").val() + '", TipoDocumentoP: "' + $("#<%=hdIdTipoOficio.ClientID%>").val() + '", Clasificacion:0, Paso: 0, Proceso:1 }';




               <%--     if ($("#<%=hdIdDocumento.ClientID%>").val() > 0)
                        param_data = '{Folio:' + <%=Folio%> + ', IdCatDocumento:' + $("#<%=hdCatDocumento.ClientID%>").val() + ', IdDocumento: ' + $("#<%=hdIdDocumento.ClientID%>").val() + ', Firmas: "' + Firmas + '", Rubricas: "' + Rubricas + '", Destinatario: "' + $("#<%=txtDestinatarioOficio.ClientID%>").val() + '", Puesto: ' + $("#<%=ddlPuestoDestinatario.ClientID%>").val() + ', EsSIE: ' + EsSIE + ', Asunto: "' + $("#<%=txtAsuntoOficio.ClientID%>").val() + '", Comentarios: "' + $("#<%=txtComentariosOficio.ClientID%>").val() + '", UsuarioElaboro: "' + '<%=Usuario%>' + '", Area: "' + $("#<%=ddlAreaOficio.ClientID%>").val() + '", TipoDocumentoP: "' + $("#<%=hdIdTipoOficio.ClientID%>").val() + '", Clasificacion:0, Paso: 0, Proceso:1 }';
                        //param_data = '{Folio:' + <%=Folio%> + ', IdCatDocumento:' + $("#<%=hdCatDocumento.ClientID%>").val() + ', IdDocumento: ' + $("#<%=hdIdDocumento.ClientID%>").val() + ', Firmas: "' + Firmas + '", Rubricas: "' + Rubricas + '", Destinatario: "' + $("#<%=txtDestinatarioOficio.ClientID%>").val() + '", Puesto: ' + $("#<%=ddlPuestoDestinatario.ClientID%>").val() + ', EsSIE: ' + EsSIE + ', Asunto: "' + $("#<%=txtAsuntoOficio.ClientID%>").val() + '", Comentarios: "' + $("#<%=txtComentariosOficio.ClientID%>").val() + '", UsuarioElaboro: "' + '<%=Usuario%>' + '", Area: "' + $("#<%=ddlAreaOficio.ClientID%>").val() +  '" , Proceso:2 }';
                    else
                        param_data = '{Folio:' + <%=Folio%> + ', IdCatDocumento:' + $("#<%=hdCatDocumento.ClientID%>").val() + ', IdDocumento: ' + $("#<%=hdIdDocumento.ClientID%>").val() + ', Firmas: "' + Firmas + '", Rubricas: "' + Rubricas + '", Destinatario: "' + $("#<%=txtDestinatarioOficio.ClientID%>").val() + '", Puesto: ' + $("#<%=ddlPuestoDestinatario.ClientID%>").val() + ', EsSIE: ' + EsSIE + ', Asunto: "' + $("#<%=txtAsuntoOficio.ClientID%>").val() + '", Comentarios: "' + $("#<%=txtComentariosOficio.ClientID%>").val() + '", UsuarioElaboro: "' + '<%=Usuario%>' + '", Area: "' + $("#<%=ddlAreaOficio.ClientID%>").val() + '", TipoDocumentoP: "' + $("#<%=hdIdTipoOficio.ClientID%>").val() + '", Clasificacion:0, Paso: 0, Proceso:3 }';--%>
                    //alert("paso")
                    //alert(param_data)
                    $.ajax({
                        type: "POST",
                        url: "ServiciosSICOD.aspx/RegistrarOficioSD",
                        //url: "ServiciosSICOD.aspx/RegistrarOficio",
                        contentType: "application/json;charset=utf-8",
                        data: param_data,
                        async: false,
                        dataType: "json",
                        success: function (data) {
                            var folio = data.d;
                            url_sicod = 'VerOficios.aspx?folio=' + folio.replace("/", "-");
                            console.log("url_sicod success");
                            console.log(url_sicod);
                            $("#<%=hfFolioSICOD.ClientID%>").val(data.d);
                            
                            $("#<%=btnActulizarGrid.ClientID%>").click();
                            //var pnlupd = window.parent.$("div[id*='upnlReqInfo']")
                            //if (pnlupd.length > 0) {
                            //    pnlupd = pnlupd[0].id
                            //    window.parent.__doPostBack(pnlupd, '');
                            //}
                        },


                        error: function (result) {
                            return result
                        }
                    });

                }



                function ConsultarOficios(pFolioSICOD) {
                    try {
                        var url = 'VerOficios.aspx?folio=' + pFolioSICOD

                        var Prop = "channelmode=1, height=450px, titlebar=0, toolbar=0; location=0;"
                        new_window = window.open(url, "", Prop);

                        new_window.onbeforeunload = function () {
                            $("#<%=btnActulizarGrid.ClientID%>").click();
                            var pnlupd = window.parent.$("div[id*='upnlReqInfo']")
                            if (pnlupd.length > 0) {
                                pnlupd = pnlupd[0].id
                                window.parent.__doPostBack(pnlupd, '');
                            }
                        }
                    }
                    catch (err) { }
                }

                function ConfirmacionAltaSICOD(Respuesta, err) {
                    if (Respuesta == 0) {
                        if (err == '') {
                            $('#divTextoConfirmacion').text("Se cancelará el registro del documento en SICOD, ¿Deseas continuar?");
                        }
                        else {
                            $('#divTextoConfirmacion').html(err);
                        }
                    }
                    else {
                        $('#divTextoConfirmacion').text("Se realizará el registro del documento en SICOD, ¿Deseas continuar?");
                    }
                    $("#divConfirmacion").dialog({
                        resizable: false,
                        autoOpen: true,
                        height: 500,
                        width: 700,
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
                            "Aceptar": function () {
                                if (Respuesta == 0) {
                                    if (err == '') {
                                        $(this).dialog("close");
                                        $("#divRegistroSICOD").dialog("close");
                                    }
                                    else {
                                        $(this).dialog("close");
                                    }
                                }
                                else {
                                    CrearFolio();

                                    $(this).dialog("close");
                                    $("#divRegistroSICOD").dialog("close");
                                }
                            },
                            "Cancelar": function () {
                                $(this).dialog("close");
                            }
                        }
                    });

                }



                jQuery(function () {
                    $('#InsertButton').click(function () {
                        $('#fileUpload').click();
                        return false;
                    });
                });

                function OnDragEnter(e) {
                    e.stopPropagation();
                    e.preventDefault();
                }

                function OnDragOver(e) {
                    e.stopPropagation();
                    e.preventDefault();
                }

                function OnDrop(e) {
                    e.stopPropagation();
                    e.preventDefault();
                    selectedName = e.dataTransfer.files[0].name;
                    selectedFile = e.dataTransfer.files[0];
                    $("#documentoSubir").text(e.dataTransfer.files[0].name + " listo para subir");
                }

                $(document).ready(function () {
                    var box;
                    box = document.getElementById("DropZone");
                    box.addEventListener("dragenter", OnDragEnter, false);
                    box.addEventListener("dragover", OnDragOver, false);
                    box.addEventListener("drop", OnDrop, false);

                    $('input[type="file"]').change(function (e) {
                        selectedName = e.target.files[0].name;
                        selectedFile = e.target.files[0];
                        $("#documentoSubir").text(e.target.files[0].name + " listo para subir");
                    });

                });

                function LevantaVentanaOficio(idRen, idDocumento, idCatDocumento, idArea, tipo) {
                    //MuestraImgCarga(null);
                    
                    var url = '';
                    $("#hfIdRenglon").val(idRen);
                    $("#hfIdDocSicod").val(idDocumento);
                    $("#hfIdArea").val(idArea);

                    //Levanta la modal
                    url = '../Procesos/Bandejas_SICOD/OFICIOS/BandejaOficios.aspx';
                    idVersion = 1;

                    //Detectar navegador
                    var nav = navigator.userAgent.toLowerCase();

                    //buscamos dentro de la cadena mediante indexOf() el identificador del navegador
                    if (nav.indexOf("msie") != -1) {
                        winprops = "dialogHeight: 450px; dialogWidth: 850px; edge: Raised; center: Yes; help: No;resizable: Yes; status: No; Location: No; Titlebar: No;";
                        window.showModalDialog(url, "SICOD", winprops);
                        OcultaImagenCarga();
                        return true;
                    } else {
                        var liTop = (parseInt($(window).height()) / 2) - (450 / 2);
                        var liLeft = (parseInt($(window).width()) / 2) - (850 / 2);

                        winprops = "width=850,height=450,toolbar=no, menubar=no, scrollbars=no, resizable=Yes, location=no, status=no, left=" + liLeft + ", top=" + liTop;
                        var modal = window.open(url + "?r=" + idRen + "&ds=" + idDocumento + "&idCat=" + idCatDocumento + "&idArea=" + idArea + "&tipo=" + tipo + "&vds=" + idVersion + "&idFolio=" + <%=Folio%> + "&origen=PC", "SICOD", winprops);
                        //var modal = window.open(url + "?ds=" + idDocumento + "&idArea=" + idArea + "&tipo=" + tipo + "&vds=" + idVersion, "SICOD", winprops);

                        modal.focus();
                        OcultaImagenCarga();
                        return false;
                    }
                }

                function llamaPostback() {
                    $("#<%=btnActulizarGrid.ClientID%>").click();
                }

                var selectedFile;
                var selectedName;
                var url_sicod;
            </script>

            <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" Visible="false" />
            </div>

            <uc1:ucFiltro2 ID="ucFiltroExp" runat="server" Width="100%" Visible="false" />
            <br />
            <asp:GridView ID="gvExpedientePC" Style="width: 100%;" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField HeaderText="PASO" DataField="I_PASO_INICIAL" />
                    <asp:BoundField HeaderText="DOCUMENTO" DataField="T_NOM_DOCUMENTO" />
                    <asp:TemplateField HeaderText="DOCUMENTOS ADJUNTOS">
                        <ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnReemplazarDocumento" ImageUrl="~/Imagenes/Delete.png" Width="25px" runat="server" ToolTip="Reemplazar Documento" />
                            <asp:ImageButton ID="btnAgregarDocumento" ImageUrl="~/Imagenes/masDocumentos.png" Width="25px" runat="server" ToolTip="Agregar nuevo documento" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SICOD">
                        <ItemTemplate>
                            <asp:Button ID="btnRegistroSICOD" runat="server" Text="Registrar" ToolTip="Registrar en SICOD" Visible="false" />
                            <asp:Button ID="btnBuscarSICOD" runat="server" Text="Buscar" ToolTip="Buscar en SICOD" Visible="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Núm. Oficio SICOD">
                        <ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>

            <div id="divCargaDocumento" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <div id="DropZone">
                                    <br />
                                    <br />
                                    <div id="documentoSubir" style="font-weight: bold; font-size: 15px">Arrastre y coloque el documento a subir</div>
                                    <br />
                                    <input type="button" id="InsertButton" value="ó seleccione un documento" />
                                    <input type="file" id="fileUpload" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="divRegistroSICOD" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <div class="txt_gral">

                                    <asp:Panel ID="PanelDatosSicod" runat="server" CssClass="PanelErrores1" Width="550px">
                                        <table cellpadding="0" cellspacing="0" style="width: 100%; border-style: solid; border-width: 2px; border-color: White">
                                            <tr>
                                                <td>
                                                    <table class="style163">
                                                        <tr>
                                                            <td class="style165">
                                                                <asp:Label ID="Label1" runat="server" CssClass="txt_gral" Text="Área del Oficio"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlAreaOficio" runat="server" CssClass="txt_gral" Width="410px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style165">
                                                                <asp:Label ID="Label17" runat="server" CssClass="txt_gral" Text="Asunto"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAsuntoOficio" runat="server" CssClass="txt_gral" Width="410px" MaxLength="255"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblComentariosOficio" Text="Comentarios" CssClass="txt_gral"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtComentariosOficio" runat="server" CssClass="txt_gral" Width="410px" MaxLength="255"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style165">
                                                                <asp:Label ID="Label18" runat="server" CssClass="txt_gral" Text="Destinatario"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDestinatarioOficio" runat="server" CssClass="txt_gral" Width="410px" MaxLength="50"></asp:TextBox>
                                                                <input id="destinatarioKey" name="destinatarioKey" runat="server" type="hidden" value="0" />
                                                                <input id="destinatarioText" name="destinatarioText" runat="server" type="hidden"
                                                                    value="" />
                                                                <asp:HiddenField runat="server" ID="hdnIdEntidad" Value="0" />
                                                                <asp:HiddenField runat="server" ID="hdnIdTipoEntidad" Value="0" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style165">
                                                                <asp:Label ID="Label19" runat="server" CssClass="txt_gral" Text="Puesto"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlPuestoDestinatario" runat="server" CssClass="txt_gral" Width="410px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style165">
                                                                <asp:Label ID="Label20" runat="server" CssClass="txt_gral" Text="Firmas"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <asp:DropDownList ID="ddlAreaFirma" runat="server" CssClass="txt_gral" Width="410px"
                                                                                onchange="ObtenerFirmas(1);">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 45%">
                                                                            <asp:Label ID="lblFiltroSIE" runat="server" CssClass="txt_gral" Text="Con firma SIE:"></asp:Label>
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:CheckBox ID="chkFiltroSIE" runat="server" Checked="true" CssClass="txt_gral" CausesValidation="false" onclick="ObtenerFirmas(1);" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 45%">
                                                                            <asp:Label ID="lblUsuariosRub" runat="server" CssClass="txt_gral" Text="Disponibles:"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 10%">&nbsp;
                                                                        </td>
                                                                        <td style="width: 45%">&nbsp;
                                                            <asp:Label ID="lblRubrica" runat="server" CssClass="txt_gral" Text="Seleccionados:"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="1" rowspan="2" style="text-align: right" valign="top">
                                                                            <asp:ListBox ID="lstUsuariosFirma" runat="server" CssClass="txt_gral" Height="68px"
                                                                                TabIndex="26" Width="100%"></asp:ListBox>
                                                                        </td>
                                                                        <td style="text-align: center">

                                                                            <img id="imgAgregarFirma" src="../../Imagenes/FlechaRojaDer.gif" style="height: 30px" onclick="AgregarFirma();" />
                                                                            <br />
                                                                            <img id="imgQuitarFirma" src="../../Imagenes/FlechaRojaIzq.gif" style="height: 30px" onclick="QuitarFirma();" />
                                                                        </td>
                                                                        <td rowspan="2" valign="top">
                                                                            <asp:ListBox ID="lstFirmas" runat="server" CssClass="txt_gral" Height="68px" Width="100%"></asp:ListBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: center">
                                                                            <br />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style165">
                                                                <asp:Label ID="Label21" runat="server" CssClass="txt_gral" Text="Rubricas"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <asp:DropDownList ID="ddlAreaRubrica" runat="server" CssClass="txt_gral" Width="410px"
                                                                                onchange="ObtenerFirmas(2);">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 45%">
                                                                            <asp:Label ID="Label8" runat="server" CssClass="txt_gral" Text="Disponibles:"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 10%">&nbsp;
                                                                        </td>
                                                                        <td style="width: 45%">&nbsp;
                                                            <asp:Label ID="Label10" runat="server" CssClass="txt_gral" Text="Seleccionados:"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="1" rowspan="2" style="text-align: right" valign="top">
                                                                            <asp:ListBox ID="lstUsuariosRubrica" runat="server" CssClass="txt_gral" Height="68px" TabIndex="26" Width="100%"></asp:ListBox>
                                                                        </td>
                                                                        <td style="text-align: center">
                                                                            <img id="imgAgregarRubrica" src="../../Imagenes/FlechaRojaDer.gif" style="height: 30px" onclick="AgregarRubrica();" />
                                                                            <br />
                                                                            <img id="imgQuitarRubrica" src="../../Imagenes/FlechaRojaIzq.gif" style="height: 30px" onclick="QuitarRubrica();" />
                                                                        </td>
                                                                        <td rowspan="2" valign="top">
                                                                            <asp:ListBox ID="lstRubricas" runat="server" CssClass="txt_gral" Height="68px" Width="100%"></asp:ListBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: center">&nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr id="trTema" runat="server" visible="false">
                                                            <td class="style165">
                                                                <asp:Label ID="Label23" runat="server" CssClass="txt_gral" Text="Tipo de Oficio"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlTema" runat="server" CssClass="txt_gral" Width="410px" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>



                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>


            <div id="divConfirmacion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <div id="divTextoConfirmacion"></div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

            <asp:HiddenField ID="hfValorScrollDocs" runat="server" Value="0" ClientIDMode="Static" />

            <asp:HiddenField ID="hfIdDocSicod" runat="server" Value="0" ClientIDMode="Static" />
            <asp:HiddenField ID="hfIdVerDocSicod" runat="server" Value="0" ClientIDMode="Static" />
            <asp:HiddenField ID="hfIdRenglon" runat="server" Value="-1" ClientIDMode="Static" />
            <asp:HiddenField ID="hfNumVerDocSicod" runat="server" Value="0" ClientIDMode="Static" />
            <asp:HiddenField ID="hfVisita" runat="server" Value="0" ClientIDMode="Static" />

            <asp:HiddenField ID="hdIdDocumento" runat="server" Value="-1" />
            <asp:HiddenField ID="hdCatDocumento" runat="server" Value="-1" />
            <asp:HiddenField ID="hfFolioSICOD" runat="server" Value="" />
            <asp:HiddenField ID="hdIdTipoOficio" runat="server" Value="" />
            <asp:HiddenField ID="hdRegistrar" runat="server" Value="" />
            <asp:Button ID="btnActulizarGrid" runat="server" OnClick="btnActulizarGrid_Click" Style="display: none" />
            <asp:Button ID="Button1" runat="server" Style="display: none" />
            <asp:Button ID="Button2" runat="server" Style="display: none" />
        </div>

    </form>
</body>
</html>
