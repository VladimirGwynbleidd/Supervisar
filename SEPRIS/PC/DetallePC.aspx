<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="DetallePC.aspx.vb" EnableEventValidation="false" Inherits="SEPRIS.DetallePC" %>

<%@ Register Src="UserControls/Actividades.ascx" TagName="Actividades" TagPrefix="uc1" %>
<%@ Register Src="UserControls/DetalleSICOD.ascx" TagName="DetalleSICOD" TagPrefix="uc2" %>
<%@ Register Src="UserControls/Irregularidad.ascx" TagName="Irregularidad" TagPrefix="uc3" %>
<%@ Register Src="UserControls/Irregularidades.ascx" TagName="Irregularidades" TagPrefix="uc4" %>
<%@ Register Src="UserControls/Checklist.ascx" TagName="Checklist" TagPrefix="uc5" %>
<%@ Register Src="UserControls/Bitacora.ascx" TagName="Bitacora" TagPrefix="uc6" %>
<%@ Register Src="UserControls/Supervisor.ascx" TagName="Supervisor" TagPrefix="uc7" %>
<%@ Register Src="UserControls/Documentos.ascx" TagName="Documentos" TagPrefix="uc8" %>
<%@ Register Src="~/PC/UserControls/Inspector.ascx" TagPrefix="uc1" TagName="Inspector" %>
<%@ Register Src="~/PC/UserControls/SancionPC.ascx" TagPrefix="uc1" TagName="SancionPC" %>
<%@ Register Src="~/OPI/UserControls/Motivo.ascx" TagName="Motivo" TagPrefix="uc10" %>
<%@ Register Src="~/OPI/UserControls/ExpedienteSisan.ascx" TagName="Expedientes" TagPrefix="uc10" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Styles/TabsV1.css" rel="stylesheet" type="text/css" />
    <link href="../Site.css" rel="stylesheet" />
    <script type="text/javascript">


        var winPop = false;
        SubProcesoValor = "";
        mainJquery();
        //para que funcione enpostBacks de updatepanels.
        function pageLoad(sender, args) {


            if (args.get_isPartialLoad()) {
                mainJquery();
            }
        }

        function GuardarBitacora(folio, usuario, paso, accion, comentarios) {

            $.ajax({
                type: "POST",
                async: false,
                url: "DetallePC.aspx/GuardarBitacota",
                contentType: "application/json;charset=utf-8",
                data: '{Folio: ' + folio +
                    ', Usuario: "' + usuario +
                    '", Paso: "' + paso +
                    '", Accion: "' + accion +
                    '", Comentarios: "' + comentarios + '\"}',
                async: false,
                dataType: "json",
                success: function (data) {
                    return true;
                },
                error: function (result) {
                }
            });
        }

        function mainJquery() {
            $(document).ready(function () {
                //$("MainContent_DetalleSICOD1_btnVerDoc").removeAttr('disabled');

                //Evento para el clic en los tabs. 
                $("ul.tabs li").click(function () {
                    setActiveTab($(this).attr("id"));
                });

                //Maneja que no sepierdan los postbacks de la pagina, se guarda el tab actual en el campo oculto con id: MainContent_hfCurrentTab, 
                // y muestra esa tab cuando se refresco la pantalla entera. 
                if ($("#hfCurrentTab").val() != "") {
                    setActiveTab($("#hfCurrentTab").val());
                } else {
                    setActiveTab("li1");
                }

                if (((<%=EstatusPC%>== 104 || <%=EstatusPC%>== 108) && <%=IdResolucion%>== "3") || <%=ShwAct%>== true) {
                    $(".tab3").show();

                }

                else {
                    $(".tab3").hide();
                }

<%--                if (<%=EstatusPC%>< 4) {
                    $(".tab4").hide();
                    $(".tab5").hide();--%>
                if (<%=EstatusPCant%>< 4) {
                    $(".tab4").hide();
                    $(".tab5").hide();
                    if (<%=EstatusPC%>> 8 && <%=EstatusPC%>!= 101) {
                        $(".tab5").hide();
                    }
                }
                else {
                    if (<%=EstatusPC%>> 9 && <%=EstatusPC%>!= 101) {
                        $(".tab5").show();
                        setActiveTab("li5");
                        setActiveTab("li4");
                    }
                }
            });
        }

        function ActulizarSupervisor_GuardarInspector() {
            ReAsignarSupervisor();
            GuardarInspectorInfo();
            GuardarBitacora(<%=Folio%>,"<%=Usuario%>", "1", "Se actualizaron supervisores y se guardaron inspectores.", " ");
            $(location).attr('href', './DetallePC.aspx');
        }

        function LlenarSupervisorInspector() {
            $(".lstinspe").empty();
            $(".lstinspe").empty();
            SubProcesoValor = $(".ddlSubprocesoP2 option:selected").val()
            ObtenerListaSupervisores()
            ObtenerListaInspectoresPaso2(SubProcesoValor)

        }

        function setActiveTab(tabId) {

            if (typeof tabId === "undefined") {
                tabId = "li1";
            }

            var str = "";
            var idPanel = "";

            str = $("#" + tabId).find("a").attr("href");

            if (str != null) {
                idPanel = str.substring(str.lastIndexOf("#"));

                $("#hfCurrentTab").val(tabId);

                $("ul.tabs li").removeClass("active");
                $("#" + tabId).addClass("active");
                $(".tab_content").hide();
                $(idPanel).fadeIn();
            }
            return false;
        }

        function Regresar() {
            $("#divMensajeRegresar").dialog({
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
                    "Aceptar": function () {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        if (winPop && !winPop.closed) {  //checks to see if window is open
                            winPop.close();
                        }
                        $(location).attr('href', './BandejaPC.aspx');
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function Rechazar() {
            var url = '';
            url = '../PC/ReasignaSupervisores.aspx';
            //dvReasigna
            //winprops = "dialogHeight: 650px; dialogWidth: 800px; edge: Raised; center: Yes; help: No;resizable: Yes; status: No; Location: No; Titlebar: No;"
            //winprops = 'width=650px,height=320px,resizable=0, toolbar=0, menubar=0'
            //winPop = window.open(url, "ReasignaSupervisores", winprops); 
            $("#dvReasigna").dialog({
                resizable: false,
                autoOpen: true,
                height: 400,
                width: 700,
                modal: true,
                open:
                    function (event, ui) {
                        $(this).parent().css('z-index', 3999);
                        $(this).parent().appendTo(jQuery("form:last"));
                        $('.ui-widget-overlay').css('position', 'fixed');
                        $('.ui-widget-overlay').css('z-index', 3998);
                        $('.ui-widget-overlay').appendTo($("form:last"));
                        $("#dvReasigna").append('<iframe src="' + url + '" style="width: 100%;height: 100%;" name="ifmReasigna" id="ifmReasigna"></iframe>')
                    }
            });
        }

        function Aceptar() {
            if (Page_ClientValidate("<%=ViewState("ValidationGroup")%>")) {

                $('#divTextoRespuesta').text('¿Estás seguro que deseas aprobar y guardar la asignación del PC?');


                $("#divMensajeRespuesta").dialog({
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
                        "Aceptar": function () {
                            $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                            $("#<%=btnGuardar.ClientID%>").trigger("click");
                        },
                        "Cancelar": function () {
                            $(this).dialog("close");
                        }
                    }
                });

            }
            else {

                $('#divTextoRespuesta').text('Es necesario completar la información necesaria para el  PC. Por favor completa los siguientes datos:');

                $("#divMensajeRespuesta").dialog({
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

                        "Aceptar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
            }
        }

        function AceptarChkLst() {
            $('#divTextoRespuesta').text('Estas por guardar la información ingresada, ¿Deseas continuar?');

            $("#divMensajeRespuesta").dialog({
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
                    "Aceptar": function () {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        $("#<%=btnGuardar.ClientID%>").trigger("click");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function _SolicitarCancelacion() {

            $("#divSolicitarCancelar").dialog({
                resizable: false,
                autoOpen: true,
                height: 300,
                width: 500,
                modal: true,
                title: 'Solicitud de cancelación',
                open:
                    function (event, ui) {
                        $(this).parent().css('z-index', 3999);
                        $(this).parent().appendTo(jQuery("form:last"));
                        $('.ui-widget-overlay').css('position', 'fixed');
                        $('.ui-widget-overlay').css('z-index', 3998);
                        $('.ui-widget-overlay').appendTo($("form:last"));
                    },
                buttons: {
                    "Enviar": function () {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        $("#<%=btnCancelarPC.ClientID%>").trigger("click");
                    },
                    "Cancelar": function () {
                        var txtName = document.getElementById('<%=cancelarMotivoCancelar.FindControl("txtMotivo").ClientID %>');
                        txtName.value="";
                        $(this).dialog("close");
                    }
                }
            });
        }

        function _SolicitarCancelacion_e() {

            //', IsSubEntidad: "' + control.prop('checked') + EstatusPC
            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/SolicitarCancelacion",
                //data: '{Folio: ' + '<%=Folio%> ' + "}",
                data: '{Folio: ' + '<%=Folio%> ' +
                    ', EstatusPC: ' + '<%=EstatusPC%> ' + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    GuardarBitacora(<%=Folio%>, "<%=Usuario%>", "1", "Se solicita la cancelación de PC." + $("#<%=cancelarMotivoCancelar.ClientID%>").ValMotivo);
                    $(location).attr('href', './DetallePC.aspx');
                },

                failure: function () {
                    $('#divTextoAlerta').html('Error al guardar');
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
            })
        }

        function _AprobarCancelacion_e() {

            //', IsSubEntidad: "' + control.prop('checked') +
            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/AprobarCancelacion",
                data: '{Folio: ' + '<%=Folio%> ' + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    GuardarBitacora(<%=Folio%>, "<%=Usuario%>", "1", "Se aprobó la cancelación de PC.");
                    /*$(location).attr('href', './DetallePC.aspx');*/
                    $('#divTextoAlerta').html('Se aprobó la cancelación');
                    $("#divAlertaGeneral").dialog({
                        resizable: false,
                        autoOpen: true,
                        height: 300,
                        width: 500,
                        modal: true,
                        title: 'Aprobación',
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
                            $(location).attr('href', './DetallePC.aspx');
                            $(this).dialog("close");
                            },
                        }
                    });

                },

                failure: function () {
                    $('#divTextoAlerta').html('Error al guardar');
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
            })
        }

        function _RechazarCancelacion_e() {

            //', IsSubEntidad: "' + control.prop('checked') +
            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/RechazarCancelacion",
                //data: '{Folio: ' + '<%=Folio%> ' + "}",
                data: '{Folio: ' + '<%=Folio%> ' +
                    ', EstatusAnterior: ' + '<%=EstatusPCant%> ' + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                   GuardarBitacora(<%=Folio%>, "<%=Usuario%>", "1", "Se rechazó la cancelación de PC.");
                  /*$(location).attr('href', './DetallePC.aspx');*/

                    $('#divTextoAlerta').html('Se rechazó la cancelación');
                    $("#divAlertaGeneral").dialog({
                        resizable: false,
                        autoOpen: true,
                        height: 300,
                        width: 500,
                        modal: true,
                        title: 'Rechazo',
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
                            $(location).attr('href', './DetallePC.aspx');
                            $(this).dialog("close");
                            },
                        }
                    });
                },

                failure: function () {
                    $('#divTextoAlerta').html('Error al guardar');
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
            })
        }


        function Requerimientos() {

            //Es necesario completar la  información necesaria para el  OPI.Por favor completa los siguientes datos:

            $('#divTextoRespuesta').html('Se iniciará el proceso para el envío de requerimientos de información a la AFORE, ¿Deseas continuar?');


            $("#divMensajeRespuesta").dialog({
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
                    "Aceptar": function () {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        GuardarBitacora(<%=Folio%>,"<%=Usuario%>", "2", "Se solicita documentación adicional.", " ");
                        CambiaEstatusFolio(2, 8);
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function AceptarPCMen() {
            if (winPop && !winPop.closed) {  //checks to see if window is open
                alert('Termine de reasignar supervisores o cierre la ventana.')
                winPop.focus();
            }
            else {
                $('#divTextoRespuesta').text('Estas por confirmar que estas de acuerdo en la asignación del Programa de correción, ¿Deseas continuar?');


                $("#divMensajeRespuesta").dialog({
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
                        "Aceptar": function () {
                            $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                            $("#<%=btnGuardar.ClientID%>").trigger("click");
                        },
                        "Cancelar": function () {
                            $(this).dialog("close");
                        }
                    }
                });
            }
        }


        function MensajeResSicod(Folio) {
            debugger

            if (Folio.indexOf("Error") == -1) {
                $('#divMensajeSICOD').html('Se ha registrado correctamente en SISAN con  el número de Folio: ' + Folio + '.');
            }
            else {
                $('#divMensajeSICOD').html(Folio);
            }







            $("#divMensajeConfirmaSICOD").dialog({
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
                    "Aceptar": function () {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        $(this).dialog("close");
                        $(location).attr('href', './BandejaPC.aspx');
                    }
                    //"Cancelar": function () {
                    //    $(this).dialog("close");
                    //}
                }
            });
        }

        function Mensajes(EnvioMensaje, Val) {
            switch (Val) {
                case 1:
                    $('#divTextoRespuesta').html(EnvioMensaje);
                    $("#divMensajeRespuesta").dialog({
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

                            "Aceptar": function () {
                                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                                $("#<%=btnGuardarIrregularidad.ClientID%>").trigger("click");
                            },
                            "Cancelar": function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                    break;
                case 2:
                    $('#divMensajeConfirmaSICOD').html(EnvioMensaje);
                    $('#divMensajeSICOD').dialog({
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

                            "Aceptar": function () {
                                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                                //Prueba();
                            },

                            "Cancelar": function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                    break;
                case 3:
                    $('#divTextoRespuesta').html(EnvioMensaje);
                    $("#divMensajeRespuesta").dialog({
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
                            "Aceptar": function () {
                                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                                $(this).dialog("close");
                                Mensajes('La resolución del Programa de Corrección es NO PRESENTADO, ¿Desea registrar un Oficio de Observaciones?', 4);
                            },
                            "Cancelar": function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                    break;
                case 4:
                    $('#divTextoRespuesta').html(EnvioMensaje);
                    $("#divMensajeRespuesta").dialog({
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
                            "Si": function () {
                                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "No", disabled: true }]);
                                $("#<%=btnGuardar.ClientID%>").trigger("click");
                            },
                            "No": function () {
                                GuardarBitacora(<%=Folio%>, "<%=Usuario%>", "3", "Resolución no presentada.", " ");
                                CambiaEstatusFolio(4, 105);
                                $(this).dialog("close");
                            }
                        }
                    });
                    break;
                default:
                    $('#divTextoRespuesta').html(EnvioMensaje);
                    $("#divMensajeRespuesta").dialog({
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
                            "Aceptar": function () {
                                $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                                $("#<%=btnGuardar.ClientID%>").trigger("click");

                            },
                            "Cancelar": function () {
                                $(this).dialog("close");
                            }
                        }
                    });

            }
        }


        function ValidaIrregularidad() {

            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/ValidarIrregularidades",
                contentType: "application/json;charset=utf-8",
                data: '{Folio: ' + '<%=Folio%>' + '}', // MMOB POSIBLEMENTE 
                dataType: "json",
                success: function (data) {
                    if (data.d == "Si") {

                        Mensajes("Estas por guardar la información ingresada ¿Deseas continuar? <br\> Al aceptar por el momento ya no podrás agregar irregularidades", 1);
                    }
                    else {

                        $('#divTextoRespuesta').text("Es necesario registrar al menos una irregularidad  por favor completar los  campos marcados con asterisco.");
                        $("#divMensajeRespuesta").dialog({
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
                                "Aceptar": function () {
                                    $(this).dialog("close");
                                },
                            }
                        });
                    }
                },
                error: function (result) {
                }

            });
        }

        function ValidaIrregularidadesCompRes() {

            var comp = false;
            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/ValidaIrregularidadesComp",
                data: '{Folio: ' + '<%=Folio%>' + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d == "Si") {
                        comp = true;
                    }
                },
                failure: function () {
                    $('#divTextoAlerta').html('Error al validar irregularidades.');
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
            });
            return comp;
        }

        function ValidaIrregularidadesComp() {
            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/ValidaIrregularidadesComp",
                data: '{Folio: ' + '<%=Folio%>' + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d == "Si") {
                        $.ajax({
                            type: "POST",
                            async: false,
                            url: "DetallePC.aspx/ValidaExpediente",
                            data: '{Folio: ' + '<%=Folio%>' + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data2) {
                                if (data2.d == false) {
                                    //var msj = "Es necesario completar información sobre la(s) respuesta(s) a (los) requerimiento(s) de información."
                                    var msj = "Existen documentos pendientes de adjuntar."
                                    $('#divTextoRespuesta').html(msj);
                                    $("#divMensajeRespuesta").dialog({
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
                                                setActiveTab('li6');
                                            },
                                        }
                                    });
                                }
                                else {
                                    $("#<%=btnGuardar.ClientID%>").removeAttr('onclick');
                                    $("#<%=btnGuardar.ClientID%>").attr('onclick', 'GuardarBitacora(<%=Folio%>,"<%=Usuario%>","2","Se validaron irregularidades.", " "); CambiaEstatusFolio(2,9);') // MMOB ESTA MAL CREO
                                    Mensajes("Estás por guardar la información ingresada, ¿Deseas Continuar?", 0);
                                }
                            },
                            failure: function () {
                                $('#divTextoAlerta').html('Error interno. Contacte al administrador');
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
                        });
                    }
                    else {
                        var msj = "<ul><li>Favor de completar información de la(s) irregularidad(es) identificada(s).</li>"
                        $.ajax({
                            type: "POST",
                            async: false,
                            url: "DetallePC.aspx/ValidaExpediente",
                            data: '{Folio: ' + '<%=Folio%>' + '}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data2) {

                                if (data2.d == false) {
                                    msj = msj + "<li>Es necesario completar información sobre la(s) respuesta(s) a (los) requerimiento(s) de información.</li>"
                                }
                            },
                            failure: function () {
                                $('#divTextoAlerta').html('Error interno. Contacte al administrador');
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
                        });
                        msj = msj + "</ul>"
                        $('#divTextoRespuesta').html(msj);
                        $("#divMensajeRespuesta").dialog({
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
                                    setActiveTab('li5');
                                },
                            }
                        });
                    }
                },
                failure: function () {
                    $('#divTextoAlerta').html('Error al cargar el checklist');
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
            });
        }


        function MensajeSubEntidad() {
            ObtenerSubEntidadesResolucion();
            $('#divSubEtidadesText').html('Seleccione una de las subvisitas para replicar la información</br>');

            $("#divSubEntidadesSeleccion").dialog({
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
                    "Aceptar": function () {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        $("#<%=btnGuardar.ClientID%>").trigger("click");

                    },

                    "Cerrar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function CallClick(objclick) {
            try {
                window.location.href = document.getElementById(objclick).href;
                return false;
            }
            catch (e) { }

        }

        function CambiaEstatusFolio(Paso, Estatus) {

            $.ajax({
                type: "POST",
                async: false,
                url: "DetallePC.aspx/Notifica",
                contentType: "application/json;charset=utf-8",
                data: '{Folio: ' + '<%=Folio%>' +
                    ', Paso: ' + Paso +
                    ',Estatus: ' + Estatus + ' }',
                dataType: "json",
                success: function (data) {
                    $(location).attr('href', './DetallePC.aspx');
                },
                error: function (result) {
                    return result
                }
            });
        }
        function RegistraOPI() {
            
            var comp = false;
            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/SendToOPI",
                data: '{Folio: ' + '<%=Folio%>' + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    $('#divTextoRespuesta').empty();
                    $("#divMensajeRespuesta img").attr('src', '../Imagenes/icono_paloma.png');
                    if (data.d.split('|')[0] == "True") {
                        GuardarBitacora(<%=Folio%>,"<%=Usuario%>", "3", "Se realizó registro de OPI.");
                        $('#divTextoRespuesta').html(data.d.split('|')[1]);
                        $("#divMensajeRespuesta").dialog({
                            resizable: false,
                            autoOpen: true,
                            height: 300,
                            width: 500,
                            title: 'Confirmación',
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
                                    window.top.location.href = './DetallePC.aspx';
                                }
                            }
                        });
                    }
                    else {
                        $('#divTextoRespuesta').html(data.d.split('|')[1]);
                        $("#divMensajeRespuesta").dialog({
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
                                "Aceptar": function () {
                                    $(this).dialog("close");
                                }
                            }
                        });
                    }
                },
                failure: function () {
                    $('#divTextoRespuesta').html("Error al registrar el OPI");
                    $("#divMensajeRespuesta").dialog({
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
                            "Aceptar": function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            });

        }



        function MensajeGuardaResolucion() {

            $('#divTextoRespuesta').empty();
            var comp = ValidaIrregularidadesCompRes();
            if (Page_ClientValidate("<%=ViewState("ValidationGroup")%>") && comp === true) {
                $('#divTextoRespuesta').html('Estas por  notificar  la resolución del programa de corrección, ¿Deseas continuar?');
                $("#divMensajeRespuesta").dialog({
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
                        "Aceptar": function () {
                            $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                            $("#<%=btnGuardar.ClientID%>").trigger("click");
                            $(location).attr('href', './DetallePC.aspx');
                        },
                        "Cancelar": function () {
                            $(this).dialog("close");
                        }
                    }
                });

            }
            else {
                $('#divTextoRespuesta').html('Es necesario completar la información necesaria para el  PC. Por favor completa los siguientes datos:');
                if (!comp) {
                    $("#divTextoRespuesta").append('<ul><li>Favor de completar información de la(s) irregularidad(es) identificada(s).</li></ul>');
                }
                $("#divMensajeRespuesta").dialog({
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
                        "Aceptar": function () {
                            $(this).dialog("close");
                            if (!comp) {
                                setActiveTab('li5');
                            }
                        }
                    }
                });
            }
        }

        function SeparaSubEnt() {
            $("#divMensajeRespuesta").attr("title", "Seleccione las subentidades que no compartirán la resolución de éste PC")
            $('#divTextoRespuesta').html('<table id="tblSubFolios" style="width: 100%"><td style="text-align: center;"><img id="NoexistenSub" runat="server" src="../Imagenes/No%20Existen.gif" visible="true" alt="No existen registos para la consulta" /></td></table>');

            var btns = 0;
            $("#divMensajeRespuesta").dialog({
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
                        $('.ui-dialog-title').css('font-size', 'x-small');

                        $.ajax({
                            type: "POST",
                            url: "DetallePC.aspx/ObtenerSubEntidadesMensaje",
                            contentType: "application/json;charset=utf-8",
                            data: '{Folio: ' + '<%=Folio%>' + ' }',
                            dataType: "json",
                            async: false,
                            success: function (data) {
                                btns = data.d.length;
                                if (data.d.length > 0) {
                                    $('#tblSubFolios').empty();
                                    $.each(data.d, function () {
                                        var nuevafila = "<tr><td><input type='checkbox' name='chkSubFolios' value='" + this['Value'] + "'>" + this['Text'] + "</td></tr>"
                                        $("#tblSubFolios").append(nuevafila)
                                        $("#<%=ValidationSummaryGeneral.ClientID%>").hide();

                                    });
                                }
                            },
                            error: function (result) {
                                return result
                            }
                        });

                    }
            });

            var myButtons1 = {
                "Aceptar": function () {
                    $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                    var SubFolios = new Array();
                    $("[name='chkSubFolios']").each(function (index, data) {
                        if (data.checked) {
                            SubFolios.push(data.value);
                        }
                    });
                    if (SubFolios.length > 0) {
                        $.ajax({
                            type: "POST",
                            url: "DetallePC.aspx/SepararSubfolios",
                            contentType: "application/json;charset=utf-8",
                            data: '{Folio: ' + '<%=Folio%>' +
                                ', Subfolios: "' + SubFolios + '\"}',
                            dataType: "json",
                            async: false,
                            success: function (data) {
                                $("#divMensajeRespuesta").attr("title", "")
                                $('#divTextoRespuesta').html('La separación de SIEFORES se realizó con éxito.');

                                $("#divMensajeRespuesta").dialog({
                                    resizable: false,
                                    autoOpen: true,
                                    height: 300,
                                    width: 500,
                                    modal: true,
                                    buttons: {
                                        "Aceptar": function () {
                                            $(location).attr('href', './DetallePC.aspx');
                                        }
                                    },
                                    open:
                                        function (event, ui) {
                                            $(this).parent().css('z-index', 3999);
                                            $(this).parent().appendTo(jQuery("form:last"));
                                            $('.ui-widget-overlay').css('position', 'fixed');
                                            $('.ui-widget-overlay').css('z-index', 3998);
                                            $('.ui-widget-overlay').appendTo($("form:last"));
                                            $('.ui-dialog-title').css('font-size', 'x-small');
                                        }
                                });
                            },
                            error: function (result) {
                                return result
                            }
                        });
                    }
                    else {
                        $('#divTextoAlerta').html('Seleccione al menos un folio');
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
                        $(this).dialog("close");
                        SeparaSubEnt();
                    }
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            };

            var myButtons2 = {
                "Cerrar": function () {
                    $(this).dialog("close");
                }
            };
            if (btns > 0) {
                $('#divMensajeRespuesta').dialog('option', 'buttons', myButtons1);
            }
            else {
                $('#divMensajeRespuesta').dialog('option', 'buttons', myButtons2);
            }
        }

        function ImagenMostrar() {
            if (<%=IdArea%> == 35) {
                ImgOperaciones();
            }
            else {
                ImgFinanciero();
            }
        }

        function ImgOperaciones() {
            $("#divImagenOp").dialog({
                resizable: true,
                autoOpen: true,
                height: 896,
                width: 1175,
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
            });

        }

        function ImgFinanciero() {
            $("#divImagenVF").dialog({
                resizable: true,
                autoOpen: true,
                height: 896,
                width: 1175,
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
            });

        }

        function MostrarModificaIrregularidad(idIrregularidad) {
            
            ObtenerProcesosDdl();
            ObtenerDatosIrregularidad(0, idIrregularidad);
            $("#modificarIrregularidad").dialog({
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
                    "Aceptar": function () {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        //EliminaIrreg(nIDIrreg);
                        //$(location).attr('href', './DetalleOPI.aspx');
                        ModificaIrregularidad(idIrregularidad);
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        function MostrarMensajeError(mensaje) {
            $('#textoMensaje').text(mensaje);
            $("#mensajeError").dialog({
                resizable: false,
                autoOpen: true,
                height: 200,
                width: 300,
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
                        $("#modificarIrregularidad").dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        $(this).dialog("close");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        function QuestionEliminar(nIDIrreg) {
            $("#eliminarIrregularidad").dialog({
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
                    "Aceptar": function () {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        EliminaIrreg(nIDIrreg);
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        function EliminaIrreg(nIDIrreg) {
            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/EliminarIrregularidad",
                data: '{idIrregularidad: ' + nIDIrreg + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    //recargar tabla de irregularidades
                    $(location).attr('href', './DetallePC.aspx');
                    return true;
                },
                failure: function () {
                    alert("Error al eliminar irregularidad");
                }
            });
        }

        function ModificaIrregularidad(idIrregularidad) {
            
            let ddlProcesoModifica = document.getElementById('ddlProcesoModifica');
            let ddlSubProceso = document.getElementById('ddlSubProceso');
            let ddlConducta = document.getElementById('ddlConducta');
            let ddlIrregularidad = document.getElementById('ddlIrregularidad');
            if ($("#<%=txtFecIrregularidad.ClientID%>").val() == '' ||
                ddlProcesoModifica.value <= 0 || ddlSubProceso.value <= 0
                || ddlConducta.value <= 0 || ddlIrregularidad.value <= 0
                || $("#<%=TxtComentarios.ClientID%>").val() == ''
                ) {
                MostrarMensajeError('Debes llenar la información obligatoria.');
                return;
            }

            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/ActualizarIrregularidad",
                async: false,
                data: '{Folio: ' + <%=Folio%> +
                ', IdIrregularidad: ' + idIrregularidad +
                ', Fecha: "' + $("#<%=txtFecIrregularidad.ClientID%>").val() +
                '", Proceso: "' + ddlProcesoModifica.value +
                '", SubProceso: "' + ddlSubProceso.value +
                '", Conducta: "' + ddlConducta.value +
                '", Irregularidad: "' + ddlIrregularidad.value +
                '", Comentarios: "' + $("#<%=TxtComentarios.ClientID%>").val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                
                $("#<%=TxtComentarios.ClientID%>").val("");
                $("#modificarIrregularidad").dialog("close");
                $(location).attr('href', './DetallePC.aspx');
                //ObtenerIrregularidadesTabla();
            },
            failure: function () {
                alert("Error al actualizar información de la irregularidad");
            }
        });
    }
    function ObtenerDatosIrregularidad(RowIndex, nIDIrreg) {
        
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerDatosModificarIrregularidad",
            data: '{Irregularidad: ' + nIDIrreg + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            async: false,
            success: function (data) {
                
                let jsonData = JSON.parse(data);
                $("#<%=txtFecIrregularidad.ClientID%>").val(jsonData.d[1]);
                document.getElementById('<%=txtFecIrregularidad.ClientID%>').value = jsonData.d[1];
                let ddlProcesoModifica = document.getElementById('ddlProcesoModifica');
                ddlProcesoModifica.value = jsonData.d[2];
                let event = new Event('change');
                ddlProcesoModifica.dispatchEvent(event);
                let ddlSubProceso = document.getElementById('ddlSubProceso');
                ddlSubProceso.value = jsonData.d[3];
                ddlSubProceso.dispatchEvent(event);
                let ddlConducta = document.getElementById('ddlConducta');
                ddlConducta.value = jsonData.d[4];
                ddlConducta.dispatchEvent(event);
                let ddlIrregularidad = document.getElementById('ddlIrregularidad');
                ddlIrregularidad.value = jsonData.d[5];
                $("#<%=TxtComentarios.ClientID%>").val(jsonData.d[9]);
                return true;
            },
            failure: function () {
                alert("Error al cargar información de las irregularidades");
            }
        });
    }
    
    function ActualizaSubProcesoAlta(RowIndex, nIDIrreg) {
        
        $.ajax({
            type: "POST",
            url: "Irregularidades.ascx/ActualizaSubProcesoAlta",
            data: '',
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            async: false,
            success: function (data) {
                
                return true;
            },
            failure: function () {
                alert("Error al cargar información de las irregularidades");
            }
        });
    }
    function ObtenerProcesosDdl() {
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerProcesos",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                var ddlProcesoModifica = $("[id*=ddlProcesoModifica]");
                ddlProcesoModifica.empty().append(
                    '<option selected="selected" value="0">--Selecciona--</option>'
                );
                $.each(data.d, function () {
                    ddlProcesoModifica.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
            },
            failure: function () {
                alert("Error al cargar información de procesos");
            }
        });
    }
    function ddlProcesoOnchange() {
        try {
            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/ObtenerSubProcesos",
                data: '{Proceso:' + parseInt(document.getElementById('ddlProcesoModifica').value) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    var ddlSubProceso = $("[id*=ddlSubProceso]");
                    ddlSubProceso.empty().append(
                        '<option selected="selected" value="0">--Selecciona--</option>'
                    );
                    $.each(data.d, function () {
                        ddlSubProceso.append($("<option></option>").val(this['Value']).html(this['Text']));
                    });
                },
                failure: function () {
                    alert("Error al cargar información de las irregularidades");
                }
            });
        } catch (error) {
            alert('Error:' + error)
        }
    }
    function ddlSubProcesoOnChange() {
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerConducta",
            data: '{Proceso:' + parseInt(document.getElementById('ddlProcesoModifica').value) +
                ',SubProceso:' + parseInt(document.getElementById('ddlSubProceso').value) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                var ddlConducta = $("[id*=ddlConducta]");
                ddlConducta.empty().append(
                    '<option selected="selected" value="0">--Selecciona--</option>'
                );
                $.each(data.d, function () {
                    ddlConducta.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
            },
            failure: function () {
                alert("Error al cargar información de las irregularidades");
            }
        });
    }
    function ddlConductaOnChange() {
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerIrregularidad",
            data: '{Proceso:' + parseInt(document.getElementById('ddlProcesoModifica').value) +
                ',SubProceso:' + parseInt(document.getElementById('ddlSubProceso').value) +
                ',Conducta:' + parseInt(document.getElementById('ddlConducta').value) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                var ddlIrregularidad = $("[id*=ddlIrregularidad]");
                ddlIrregularidad.empty().append(
                    '<option selected="selected" value="0">--Selecciona--</option>'
                );
                $.each(data.d, function () {
                    ddlIrregularidad.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
            },
            failure: function () {
                alert("Error al cargar información de las irregularidades");
            }
        });
    }
    function ObtenerIrregularidadesTabla() {
        
        $.ajax({
            type: "POST",
            url: "DetallePC.aspx/ObtenerIrregularidadesTabla",
            data: '{Folio:' + <%=Folio%> + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                
            },
            failure: function () {
                alert("Error al actualizar tabla de irregularidades");
            }
        });
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <asp:Button ID="btnActualizaOculto" runat="server" Text="Prueba" Style="display: none" />
    <div style="float: left; width: 92%; text-align: center; position: absolute">
        <asp:Label ID="lblFolio" runat="server" Style="font-weight: bold; font-size: 13px; color: black;"></asp:Label><br />
        <asp:Label ID="lblPaso" runat="server" Style="font-weight: bold; font-size: 9px; color: black;"></asp:Label>
        <div style="right: 7%; top: 70%; position: absolute; width: 26%;">
            <asp:ImageButton ID="imgProcesoVisita" OnClientClick="ImagenMostrar()" ToolTip="Ver diagrama del proceso" runat="server" ImageUrl="../Imagenes/ProcesoInspeccion.png" />
        </div>
        <div id="pnlSepara" title="Seleccione los folios que no compartirán la resolución" style="right: 2%; top: 70%; position: absolute; display: none; width: 26%;">
            <img id="btnSepara" width="40" onclick="SeparaSubEnt();" src="../Imagenes/SepararSubvisitas.png" alt="Reasignar" />
        </div>
    </div>
    <div id="tabs_wrapper" style="margin-top: 20px; height: 47px;">
        <ul id="Ul2" class="tabs" runat="server" style="width: 100%;">
            <li id="li1" ><a href="#tab1">Información PC</a></li>
            <li id="li2" ><a href="#tab2">Bitácora de acciones</a></li>
            <li id="li3" class="tab3"><a href="#tab3">Actividades del programa</a></li>
            <li id="li4" class="tab4"><a href="#tab4">Análisis PC</a></li>
            <li id="li5" class="tab5"><a href="#tab5">Irregularidades</a></li>
            <li id="li6" ><a href="#tab6">Expediente documentos</a></li>
            <li id="liExpedientes" runat="server"><a href="#tab7">Expediente SISAN</a></li> 

        </ul>
    </div>

    <div style="width: 100%; float: left;">
        <div id="tab1" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 20px 0px 15px 0px; height: auto;">


            <asp:Panel ID="pnlDetalleSICOD" runat="server" Enabled="false">
                <uc2:DetalleSICOD ID="DetalleSICOD1" runat="server" />
            </asp:Panel>
            <hr />
            <asp:Panel ID="pnlSupervisores" runat="server" Width="76%">
                <uc7:Supervisor ID="Supervisor1" runat="server" />
            </asp:Panel>
            <hr id="hrIns" runat="server" visible="false" />
            <asp:Panel ID="PnlInspector" runat="server" Width="76%" Enabled="false">
                <uc1:Inspector runat="server" ID="Inspector" />
            </asp:Panel>
            <hr id="hrSan" runat="server" visible="false" />
            <uc1:SancionPC runat="server" ID="SancionPC" Visible="false" />
            <hr id="hrIrr" runat="server" visible="false" />
            <asp:Panel ID="pnlIrregularidad" runat="server" Width="76%" Visible="false">
                <br />
                <br />
                Por favor de completar la información siguiente para la Identificación de  la(s) irregularidad(es):
                <br />
                <div id="pnlIrreg" runat="server">
                </div>
                <br />
                <table id="tblIrregularidades" width="100%" border="1" style="border-collapse: collapse; height: 100%; margin-bottom: 1rem;">
                    <thead class="GridViewEncabezadoTbl">
                        <tr>
                            <th>Irregularidades identificadas:</th>
                        </tr>
                    </thead>
                    <tr>
                        <td style="text-align: center;">
                            <img id="Noexisten" runat="server" src="../Imagenes/No%20Existen.gif" visible="true" alt="No existen Registos para la Consulta" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <div>
                <asp:Panel ID="pnlPaso1" runat="server" Visible="false">
                    <img id="btnAceptar1" width="40" onclick="AceptarPCMen();" src="../Imagenes/icono_paloma.png" alt="Aceptar" title="Aceptar asignación de PC" />
                    <img id="btnHome1" width="40" onclick="Regresar();" src="../Imagenes/inicio.png" alt="Regresar a Bandeja" title="Regresar a Bandeja PC" />
                    <img id="btnDetalle1" width="40" onclick="setActiveTab('li6');" src="../Imagenes/IrADetallevisita.png" alt="Ver Expediente" title="Ver Expediente" />
                    <img id="btnReasignar" width="40" onclick="Rechazar();" src="../Imagenes/icono_tache.png" alt="Regresar a SICOD" title="Rechazar asignación de PC" />
                </asp:Panel>
                <asp:Panel ID="pnlPaso1_1" runat="server" Visible="false">
                    <img id="btnAceptarP1_E1" width="40" onclick="Mensajes('Se avanzará al paso siguiente y se notificará  a todos los asignados.¿Deseas continuar?');" src="../Imagenes/notificar_3.png" alt="Reasignar" runat="server" visible="false" title="Notificar asignaciones" />
                    <img id="btnReasignarP1_E1" width="40" src="../Imagenes/AddDestinatario.jpg" runat="server" alt="Asignar Inspector" onclick="Aceptar();" title="Asignar Inspector(es)" />
                    <img id="btnHome2P1_E1" width="40" onclick="Regresar();" src="../Imagenes/inicio.png" alt="Regresar a Bandeja PC" title="Regresar a Bandeja PC" />
                    <img id="btnDetalleP1_E1" width="40" onclick="setActiveTab('li6');" src="../Imagenes/IrADetallevisita.png" alt="Ver Expediente" title="Ver Expediente" />
                </asp:Panel>
                <asp:Panel ID="pnlPaso2" runat="server" Visible="false">
                    <img id="btnCancelarPC_P2" runat="server" width="40" onclick="_SolicitarCancelacion()" src="../Imagenes/cancelarVisita.png" alt="Cancelar PC" title="Cancelar PC" visible="false" />

                    <img id="btnAceptarP2" width="40" onclick="ValidaIrregularidad();" src="../Imagenes/registrarVisita.png" alt="Reasignar" title="Guardar Irregularidades" />
                    <img id="btnHomeP2" width="40" onclick="Regresar();" src="../Imagenes/inicio.png" alt="Reasignar" title="Regresar a Bandeja PC" />
                     <img id="btnDetalleP1_E1" width="40" onclick="setActiveTab('li6');" src="../Imagenes/IrADetallevisita.png" alt="Ver Expediente" title="Ver Expediente" />
                </asp:Panel>
                <asp:Panel ID="PnlPaso2_1" runat="server" Visible="false">
                    <%--<img id="btnInfoP2_E1" width="40" onclick="" src="../Imagenes/reload.png" alt="Reasignar"/>--%>
                    <img id="btnAceptarP2_E1" width="40" onclick="" src="../Imagenes/registrarVisita.png" alt="Reasignar" title="Guardar Checklist" />
                    <img id="btnHomeP2_E1" width="40" onclick="Regresar();" src="../Imagenes/inicio.png" alt="Reasignar" title="Regresar a Bandeja PC" />
                    <img id="btnDetalleP2_E1" width="40" onclick="" src="../Imagenes/IrADetallevisita.png" alt="Reasignar" title="Ver Expediente" />

                </asp:Panel>
                <asp:Panel ID="pnlPaso2_2" runat="server" Visible="false">
                    <img id="btnInfoPCP2_E2" width="40" onclick="" src="../Imagenes/VerDetalle.png" alt="Reasignar" title="Ver información del PC" />
                    <img id="btnAceptarP2_E2" width="40" onclick="" src="../Imagenes/registrarVisita.png" alt="Reasignar" title="Revisar este botón" />
                    <img id="btnHomeP2_E2" width="40" onclick="Regresar();" src="../Imagenes/inicio.png" alt="Reasignar" title="Regresar a Bandeja PC" />
                    <img id="btnDetalleP2_E2" width="40" onclick="" src="../Imagenes/IrADetallevisita.png" alt="Reasignar" title="Ver Expediente" />
                </asp:Panel>
                <asp:Panel ID="pnlPaso3" runat="server" Visible="false">
                    <img id="InfoPCP3" width="40" onclick="" src="../Imagenes/registrarVisita.png" alt="Reasignar" />
                    <img id="btnHomeP3" width="40" onclick="Regresar();" src="../Imagenes/inicio.png" alt="Reasignar" />
                    <img id="btnDetalleP3" width="40" onclick="" src="../Imagenes/IrADetallevisita.png" alt="Reasignar" />
                    <img id="btnAceptarP3" width="40" onclick="" src="../Imagenes/notificar_3.png" alt="Reasignar" />
                </asp:Panel>
                <asp:Panel ID="pnlPaso4" runat="server" Visible="false">
                    <img id="btnAceptarP4" width="40" onclick="" src="../Imagenes/registrarVisita.png" alt="Reasignar" />
                    <img id="btnHomeP4" width="40" onclick="Regresar();" src="../Imagenes/inicio.png" alt="Reasignar" />
                    <img id="btnDetalleP4" width="40" onclick="" src="../Imagenes/IrADetallevisita.png" alt="Reasignar" />
                    <img id="btnEnvioP4" width="40" onclick="" src="../Imagenes/siguiente.png" alt="Reasignar" />
                </asp:Panel>
                <asp:Panel ID="PnlBtnInspector" runat="server" Visible="false">
                    <asp:ImageButton ID="btnRechazar" runat="server" Width="40" ImageUrl="~/Imagenes/RechazarDocto.png" OnClientClick="Rechazar(); return false;" ToolTip="Regresar folio a SICOD" />
                    <asp:ImageButton ID="btnRegresar_2" runat="server" Width="40" ImageUrl="~/Imagenes/inicio.png" OnClientClick="Regresar(); return false;" ToolTip="Regresar a Bandeja SICOD" />
                    <asp:ImageButton ID="btnAceptar" runat="server" Width="40" ImageUrl="~/Imagenes/AprobarDocto.png" OnClientClick="Aceptar(); return false;" ToolTip="Aceptar Folio y seguir proceso de PC" />
                </asp:Panel>
            </div>

        </div>
        <div id="tab2" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
            <uc6:Bitacora ID="Bitacora1" runat="server" />
        </div>
        <div id="tab3" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
            <asp:Panel ID="pnlActiv" runat="server" Visible="false">
            </asp:Panel>
            <div style="margin-top: 20px; margin-bottom: 20px;">
                <img id="btnAceptar_Actividad" width="40" onclick="setActiveTab('li1');" src="../Imagenes/VerDetalle.png" alt="Reasignar" />
                <img id="btnDetalle_Activadad" width="40" onclick="setActiveTab('li6');" src="../Imagenes/IrADetallevisita.png" alt="Reasignar" />
            </div>
        </div>
        <div id="tab4" class="tab_content tab4" style="horizontal-align: center; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
            <uc5:Checklist ID="Checklist1" runat="server" />
            <img id="btnCancelarPC_P2_A" runat="server" width="40" onclick="_SolicitarCancelacion()" src="../Imagenes/cancelarVisita.png" alt="Cancelar PC" title="Cancelar PC" visible="false" />
            <img id="btnAprobarCancelarPC_P2" runat="server" width="40" onclick="_AprobarCancelacion_e()" src="../Imagenes/AprobarDejarSEfecto.png" alt="Aprobar cancelar PC" title="Aprobar cancelar PC" visible="false" />
            <img id="btnRechazarPC_P2" runat="server" width="40" onclick="_RechazarCancelacion_e()" src="../Imagenes/RechazarDejarSEfecto.png" alt="Rechazar cancelar PC" title="Rechazar cancelar PC" visible="false" />
                    
            <img id="btnADetalle_tab4" runat="server" width="40" onclick="" src="../Imagenes/Requerimiento.png" alt="Reasignar" title="Solicitar información adicional" />
            <img id="btnNotifica_tab4" runat="server" width="40" onclick="Mensajes('Estas por  notificar  la resolución del Programa de corrección, ¿Deseas continuar?',3)" src="../Imagenes/notificar_3.png" alt="Reasignar" title="Notificar Resolución" />
            <samp runat="server" id="BtnAceptarTab4">
                <img id="btnAceptar_tab4" width="40" onclick="AceptarChkLst();" src="../Imagenes/registrarVisita.png" alt="Reasignar" title="Guardar Información de PC" />
            </samp>
            <img id="btnHome_tab4" width="40" onclick=" Regresar();" src="../Imagenes/inicio.png" alt="Reasignar" title="Regresar a Bandeja PC" />
            <img id="btnFolder_tab4" width="40" onclick="setActiveTab('li6');" src="../Imagenes/IrADetallevisita.png" alt="Reasignar" title="Ver Expediente" />
            <img id="btnEnvio_tab4" runat="server" width="40" onclick="Mensajes('Se enviará el asunto a sanciones SISAN, ¿Deseas continuar?')" src="../Imagenes/siguiente.png" alt="Reasignar" title="Registrar en SISAN" />
        </div>
        <div id="tab5" class="tab_content tab5" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
            <asp:Panel ID="pnlIrr" runat="server" Visible="false">
            </asp:Panel>
        </div>
        <div id="tab6" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
            <iframe src="ExpedientePC.aspx" width="100%" height="700px" style="border-width: 0px;"></iframe>
        </div>
        <asp:Panel ID="tabExpedienteSISAN" runat="server" visible="false">
            <div id="tab7" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
                <uc1:SancionPC runat="server" ID="SancionPC1" Visible="false" />
                <asp:Panel ID="Panel1" runat="server" Visible="true">                  
                    <img id="btnHomeP2_E1" width="40" onclick="Regresar();" src="../Imagenes/inicio.png" alt="Reasignar" title="Regresar a Bandeja PC" />
                    <img id="btnDetalleP2_E1" width="40" onclick="setActiveTab('li6')" src="../Imagenes/IrADetallevisita.png" alt="Reasignar" title="Ver Expediente" />
                </asp:Panel>
            </div>
            
        </asp:Panel>
    </div>

   <%-- HASTA AQUI TERMINA INFORMACIN GENERAL--%>

    <div id="divMensajeRegresar" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        ¿Estás seguro que deseas regresar a la página principal?
                        <br />
                        <br />
                        *Recuerda que al abandonar la pantalla perderás los datos capturados.
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divMensajeRechazar" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image1" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        ¿Estás seguro que deseas rechazar el registro de SICOD?                      
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divMensajeRespuesta" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image2" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <div id="divTextoRespuesta"></div>
                        <asp:ValidationSummary ID="ValidationSummaryGeneral" runat="server" ValidationGroup="" CssClass="MensajeModal-UI" />
                    </div>
                </td>
            </tr>
        </table>

    </div>

    <div id="divAlertaGeneral" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image3" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <div id="divTextoAlerta"></div>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="" CssClass="MensajeModal-UI" />
                    </div>
                </td>
            </tr>
        </table>

    </div>

    <div id="divMensajeConfirmaSICOD" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="MSJ_Sicod" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/icono_paloma.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <div id="divMensajeSICOD"></div>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div id="divImagenOp" class="txt_gral" style="display: none">
        <table width="100%">
            <tr>
                <td style="text-align: center; vertical-align: top">
                    <asp:Image ID="Image8" runat="server" Width="100%" Height="100%"
                        ImageUrl="~/Imagenes/PC_VO.JPG" />
                </td>
            </tr>
        </table>
    </div>

    <div id="divImagenVF" class="txt_gral" style="display: none">
        <table width="100%">
            <tr>
                <td style="text-align: center; vertical-align: top">
                    <asp:Image ID="Image9" runat="server" Width="100%" Height="100%"
                        ImageUrl="~/Imagenes/PC_VF.JPG" />
                </td>
            </tr>
        </table>
    </div>

    <div id="dvReasigna" title="Reasigna supervisores" style="display: none; overflow: hidden;">
    </div>

    <div id="divSolicitarCancelar" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image10" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%--<%= Mensaje%>&nbsp;--%>
                        <label id="Label1" runat="server" class="txt_gral">Se enviará solicitud de cancelación del PC al administrador del sistema, por favor ingresa el motivo de la cancelación</label>
                    </div>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <uc10:Motivo ID="cancelarMotivoCancelar" runat="server" />
                </td>
            </tr>
        </table>
         <table >
             <tr >
                <tb ></tb>
                <tb >
                  <asp:Label runat="server" ID="lblNota" Style="color: black; font-size: 9pt"  ></asp:Label>
                </tb>
                
            </tr>
        </table>

    </div>

    <div id="mensajeError">
        <table width="100%">
            <tr>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <label id="textoMensaje"></label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="modificarIrregularidad" style="display: none" title='Modificar Irregularidad'>
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="LblNumIrregularidad" runat="server" Text="Fecha de irregularidad" CssClass="txt_gral"></asp:Label>
                    <span style="color:red">&nbsp;*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtFecIrregularidad" runat="server" Width="200px" CssClass="txt_gral"></asp:TextBox>

                    <ajx:calendarextender ID="calExFechaIrregularidad" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCal"
                        TargetControlID="txtFecIrregularidad" CssClass="teamCalendar" />
                    <asp:Image ImageAlign="Bottom" ID="imgCal" runat="server" ImageUrl="~/imagenes/Calendar.gif" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblProceso" runat="server" Text="Proceso" CssClass="txt_gral"></asp:Label><span style="color:red">&nbsp;*</span>
                </td>
                <td>
                    
                    <select id="ddlProcesoModifica" onchange="ddlProcesoOnchange()" style="width:200px"></select>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblSubProceso" runat="server" Text="Subproceso" CssClass="txt_gral"></asp:Label><span style="color:red">&nbsp;*</span>
                </td>
                <td>
                    
                    <select id="ddlSubProceso" onchange="ddlSubProcesoOnChange()"  style="width:200px"></select>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblConducta" runat="server" Text="Conducta sancionable" CssClass="txt_gral"></asp:Label>
                    <span style="color:red">&nbsp;*</span>
                </td>
                <td>
                    <select id="ddlConducta" onchange="ddlConductaOnChange()" style="width:200px"></select>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Irregularidad" CssClass="txt_gral"></asp:Label>
                    <span style="color:red">&nbsp;*</span>
                </td>
                <td>
                    <select id="ddlIrregularidad" style="width:200px"></select>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblComentarios" runat="server" Text="Comentarios adicionales<br>de la irregularidad" CssClass="txt_gral"></asp:Label>
                    <span style="color:red">&nbsp;*</span>
                </td>
                <td>
                    <asp:TextBox ID="TxtComentarios" runat="server" CssClass="txt_gral" Height="45px" Width="200px"
                        TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>

    <div id="eliminarIrregularidad" style="display: none" title='Eliminar Irregularidad'>
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image4" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        ¿Esta seguro que desea eliminar la irregularidad?                      
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <asp:Button ID="btnEliminar" runat="server" Style="display: none" />
    <asp:Button ID="btnGuardar" runat="server" Style="display: none" />
    <asp:Button ID="btnCancelarPC" runat="server" Style="display: none" />
    <asp:Button ID="btnGeneraOpi" OnClick="btnGeneraOpi_Click" runat="server" Style="display: none" />
    <asp:Button ID="btnGuardarIrregularidad" runat="server" Style="display: none" />
    <asp:Button ID="btnSicodMensg" runat="server" Style="display: none" />
    <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfCurrentTab" runat="server" ClientIDMode="Static" />

</asp:Content>
