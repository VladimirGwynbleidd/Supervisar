<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="DetalleOPI.aspx.vb" EnableEventValidation="false" Inherits="SEPRIS.DetalleOPI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<%@ Register Src="UserControls/ComentariosOPI.ascx" TagName="Comentarios" TagPrefix="uc1" %>
<%@ Register Src="UserControls/ExpedienteSisan.ascx" TagName="Expedientes" TagPrefix="uc10" %>
<%@ Register Src="UserControls/Motivo.ascx" TagName="Motivo" TagPrefix="uc10" %>
<%@ Register Src ="~/OPI/UserControls/MotivoOpi1.ascx" TagName="Motivo1" TagPrefix ="uc11" %>
<%@ Register Src="UserControls/ClasificacionOPI.ascx" TagName="Clasificacion" TagPrefix="uc2" %>
<%@ Register Src="UserControls/SupuestoAvisoOPI.ascx" TagName="SupuestoAviso" TagPrefix="uc3" %>
<%@ Register Src="UserControls/RespuestaAforeOPI.ascx" TagName="RespAfore" TagPrefix="uc4" %>
<%@ Register Src="UserControls/IrregularidadOPI.ascx" TagName="Irregularidad" TagPrefix="uc5" %>
<%@ Register Src="UserControls/PosibleIncumplimiento.ascx" TagName="PosibIncumplim" TagPrefix="uc6" %>
<%@ Register Src="UserControls/AltaIrregularidadesOPI.ascx" TagName="AltaIrregularidad" TagPrefix="uc8" %>
<%@ Register Src="~/OPI/UserControls/DetalleOPIuc.ascx" TagPrefix="ucdopi" TagName="DetalleOPIuc" %>

<%@ Register Src="UserControls/Bitacora.ascx" TagName="Bitacora" TagPrefix="uc7" %>
<%@ Register Src="../PC/UserControls/SancionPC.ascx" TagName="SancionPC" TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Styles/TabsV1.css" rel="stylesheet" type="text/css" />
    <link href="../Site.css" rel="stylesheet" />
    <link href="../Styles/Site.css" rel="stylesheet" />

    <script type="text/javascript">

        mainJquery();
        //para que funcione enpostBacks de updatepanels.
        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
                mainJquery();
            }
        }

        function mainJquery() {
            $(document).ready(function () {
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
            });
        }

        $(function () {

            MensajeDosBotonesUnaAccionLoad();
            MensajeUnBotonUnaAccionLoad();
            MensajeUnBotonNoAccionLoad();

        });


        function MensajeValidacion() {
            MensajeUnBotonNoAccion();
        };

        function MensajeFinalizar() {
            MensajeUnBotonUnaAccion();
            //location.href = "../OPI/BandejaOpi.aspx";
        }

        function MensajeConfirmacion() {
            MensajeDosBotonesUnaAccion();
        }

        function MensajeSalir() {

            MensajeDosBotonesUnaAccion();

        }

        function Notifica(Paso, Subpaso) {

            $.ajax({
                type: "POST",
                url: "DetalleOPI.aspx/Notifica",
                contentType: "application/json;charset=utf-8",
                data: '{Folio: ' + '<%=Folio%>' +
                    ', Paso: ' + Paso +
                    ',Estatus: ' + Subpaso + ' }',
                dataType: "json",
                success: function (data) {
                    return true
                    //$(location).attr('href', './DetalleOPI.aspx');
                },
                error: function (result) {
                    return result
                }
            });
        }


        function MostrarAceptar() {

            MensajeUnBotonUnaAccion();
            location.href = "../OPI/BandejaOpi.aspx";
        }

        function MostrarMensaje() {

            MensajeUnBotonNoAccion();

        }

        function AceptarPCMen() {

            //Es necesario completar la  información necesaria para el  OPI.Por favor completa los siguientes datos:

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


        function Regresar2() {
            $(location).attr('href', './BandejaOPI.aspx');
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
                        $(location).attr('href', './BandejaOPI.aspx');
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function Rechazar() {
            $("#divMensajeRechazar").dialog({
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
                        $("#<%=btnEliminar.ClientID%>").trigger("click");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

        function Aceptar() {

            //Es necesario completar la  información necesaria para el  OPI.Por favor completa los siguientes datos:

            $('#divTextoRespuesta').text('¿Estás seguro que deseas completar el?');


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


        function AceptarPCMen() {

            //Es necesario completar la  información necesaria para el  OPI.Por favor completa los siguientes datos:

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

        function AccionGuardar() {
            $("#<%=btnGuardar.ClientID%>").trigger("click");
        }

       
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />

    <div style="float: left; width: 92%; text-align: center; position: absolute">
        <asp:Label ID="lblFolio" runat="server" Style="font-weight: bold; font-size: 13px; color: black;"></asp:Label>
        <br />
        <asp:Label ID="lblPaso" runat="server" Style="font-weight: bold; font-size: 9px; color: black;"></asp:Label>
    </div>
    <div id="tabs_wrapper" style="margin-top: 20px; height: 47px;">
        <ul id="Ul2" class="tabs" runat="server" style="width: 100%;">
            <li id="li1"><a href="#tab1">Información General</a></li>
            <li id="li2"><a href="#tab2">Expediente Documentos</a></li>
            <li id="li3"><a href="#tab3">Bitácora de acciones</a></li>
            <li id="liExpedientes" runat="server"><a href="#tab7">Expediente SISAN</a></li>
        </ul>
    </div>
    <div id="tab1" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
        <div style="float: left; width: 100%; text-align: right; margin: -50px 150px 0px 0px;">
            <asp:ImageButton ID="imgProcesoVisita" ToolTip="Ver diagrama del proceso" runat="server" ImageUrl="../Imagenes/ProcesoInspeccion.png" />
        </div>

        <asp:UpdatePanel runat="server" ID="upTabla">
            <Triggers>
                <%--<asp:PostBackTrigger ControlID="imgRegistrarVisita" />--%>
                <%--<asp:AsyncPostBackTrigger ControlID="GridPrincipal" />--%>
                <asp:PostBackTrigger ControlID="btnAceptarM1B1A"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btnAceptarM2B1A"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btnAceptarM3B1A"></asp:PostBackTrigger>
            </Triggers>

            <ContentTemplate>
                <table id="tbDatosCaptura" runat="server" style="width: 95%; margin-top: 20px;">
                    <tr>
                        <td>
                            <div style="width: 100%; float: left;">
                                <asp:Panel ID="pnlDetalleOPI" runat="server" Enabled="false">
                                    <ucdopi:DetalleOPIuc runat="server" ID="DetalleOPIuc" />
                                </asp:Panel>
                                <asp:Panel ID="pnlDetalleClasifOPI" runat="server" Enabled="true">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%">
                                                <uc2:Clasificacion ID="DetalleClasifOPI1" runat="server" />
                                            </td>
                                            <td>
                                                <div id="divFechaEstimadaEntrega" runat="server" visible="false">
                                                    <label class="txt_gral">Fecha estimada de entrega de información:</label>
                                                    <asp:TextBox ID="txtFecEstimadaEntrega" runat="server"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <asp:Panel ID="pnlSupuestoAviso" runat="server" Enabled="true">
                                    <uc3:SupuestoAviso ID="DetalleSupuestoAvisoOPI1" runat="server" />
                                </asp:Panel>

                                <asp:Panel ID="pnlIrregularidad" runat="server" Enabled="true">
                                    <uc5:Irregularidad ID="DetalleIrregularidad1" runat="server" />
                                </asp:Panel>

                                <asp:Panel ID="pnlRespAfore" runat="server" Enabled="true">
                                    <uc4:RespAfore ID="DetalleRespAforeOPI1" runat="server" />
                                </asp:Panel>

                                <asp:Panel ID="pnlDetallePosibIncumplim" runat="server" Enabled="true">
                                    <uc6:PosibIncumplim ID="DetallePosibIncumplim" runat="server" />
                                </asp:Panel>

                                <asp:Panel ID="pnlDetalleAltaIrregularidad" runat="server" Enabled="true">
                                    <uc8:AltaIrregularidad ID="DetalleAltaIrregularidad" runat="server" />
                                </asp:Panel>

                                <asp:Panel ID="pnlDetalleComentOPI" runat="server" Enabled="true">
                                    <uc1:Comentarios ID="DetalleComentOPI1" runat="server" />
                                </asp:Panel>

                                <hr />

                            </div>
                            <hr id="hrSan" runat="server" visible="false" />
                            <uc9:SancionPC runat="server" ID="SancionPC" Visible="false" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>

            <Triggers>
                <asp:PostBackTrigger ControlID="btnAceptarM1B1A" />
                <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
                <asp:PostBackTrigger ControlID="btnAceptarM3B1A" />
            </Triggers>
        </asp:UpdatePanel>

        <div>
            <asp:Panel ID="xpnlPaso1__1" runat="server" Visible="true">
                <asp:ImageButton ID="btnNotificar" runat="server" src="../Imagenes/notificar_3.png" Width="40px"
                    CommandName="Notificar_Paso1" CommandArgument="1,0" OnCommand="btnNotificar_Command" ToolTip="Notificar Registro de Oficio" />
                <asp:ImageButton ID="btnAceptar1" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" ToolTip="Guardar Registro" />
                <asp:ImageButton ID="btnEditar1" runat="server" src="../Imagenes/icono_lapiz.gif" Width="40px" OnCommand="HabilitarEdicion" Visible="false" ToolTip="Editar información" />
                <asp:ImageButton ID="btnActuOpi1" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" OnCommand="ActualizaOPI" Visible="false" ToolTip="Guardar información" />
                <asp:ImageButton ID="btnHome1" runat="server" src="../Imagenes/inicio.png" Width="40px" OnCommand="btnHome_Command" ToolTip="Regresar a Bandeja de Oficios" />
                <asp:ImageButton ID="btnDetalle1" runat="server" Width="40" OnClientClick="setActiveTab('li2'); return false;" src="../Imagenes/IrADetallevisita.png" alt="Detalle" ToolTip="Ver expediente" />
            </asp:Panel>
            <asp:Panel ID="xpnlPaso2_0" runat="server" Visible="true">
                <asp:ImageButton ID="btnNotificarP2" runat="server" src="../Imagenes/notificar_3.png" Width="40px"
                    CommandName="Notificar_Paso2" CommandArgument="2,1" OnCommand="btnNotificar_Command" ToolTip="Notificar Clasificación de Oficio" />
                <asp:ImageButton ID="btnAceptar2" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" ToolTip="Guardar Clasificación" />
                <asp:ImageButton ID="btnEditar2" runat="server" src="../Imagenes/icono_lapiz.gif" Width="40px" OnCommand="HabilitarEdicion" Visible="false" ToolTip="Editar información" />
                <asp:ImageButton ID="btnActuOpi2" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" OnCommand="ActualizaOPI" Visible="false" ToolTip="Guardar información" />
                <asp:ImageButton ID="btnHome20" runat="server" src="../Imagenes/inicio.png" Width="40px" OnCommand="btnHome_Command" ToolTip="Ir a bandeja de Oficios" />
                <asp:ImageButton ID="btnDetalle20" runat="server" Width="40" OnClientClick="setActiveTab('li2'); return false;" src="../Imagenes/IrADetallevisita.png" alt="Detalle" ToolTip="Ver expediente" />
            </asp:Panel>
            <asp:Panel ID="xpnlPaso2_1" runat="server" Visible="true">
                <asp:ImageButton ID="btnNotificarP21" runat="server" src="../Imagenes/notificar_3.png" Width="40px"
                    CommandName="Notificar_Paso2.4" CommandArgument="2,2" OnCommand="btnNotificar_Command" ToolTip="Notificar envío de Requerimiento" />
                <asp:ImageButton ID="btnAceptar21" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" ToolTip="Guardar información" />
                <asp:ImageButton ID="btnEditar21" runat="server" src="../Imagenes/icono_lapiz.gif" Width="40px" OnCommand="HabilitarEdicion" Visible="false" ToolTip="Editar información" />
                <asp:ImageButton ID="btnActuOpi21" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" OnCommand="ActualizaOPI" Visible="false" ToolTip="Guardar información" />
                <asp:ImageButton ID="btnHome21" runat="server" src="../Imagenes/inicio.png" Width="40px" OnCommand="btnHome_Command" ToolTip="Ir a Bnadeja de Oficios" />

                <asp:ImageButton ID="btnDetalle21" runat="server" OnClientClick="setActiveTab('li2'); return false;" Width="40" src="../Imagenes/IrADetallevisita.png" alt="Detalle" ToolTip="Ver Expediente" />
            </asp:Panel>

            <asp:Panel ID="xpnlPaso3" runat="server" Visible="true">
                <asp:ImageButton ID="btnNotificarP3" runat="server" src="../Imagenes/notificar_3.png" Width="40px"
                    CommandName="Notificar_Paso3" CommandArgument="3,1" OnCommand="btnNotificar_Command" ToolTip="Notificar Respuesta AFORE" />
                <asp:ImageButton ID="btnAceptar3" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" ToolTip="Guarda Respuesta AFORE" />
                <asp:ImageButton ID="btnEditar23" runat="server" src="../Imagenes/icono_lapiz.gif" Width="40px" OnCommand="HabilitarEdicion" Visible="false" ToolTip="Editar información" />
                <asp:ImageButton ID="btnActuOpi23" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" OnCommand="ActualizaOPI" Visible="false" ToolTip="Guardar información" />
                <asp:ImageButton ID="btnHome3" runat="server" src="../Imagenes/inicio.png" Width="40px" OnCommand="btnHome_Command" ToolTip="Ir a Bandeja de Oficios" />
                <asp:ImageButton ID="btnDetalle3" runat="server" OnClientClick="setActiveTab('li2'); return false;" Width="40" src="../Imagenes/IrADetallevisita.png" alt="Detalle" ToolTip="Ver expediente" />
            </asp:Panel>
            <asp:Panel ID="xpnlPaso4" runat="server" Visible="true">
                <asp:ImageButton ID="btnAceptar4" runat="server" src="../Imagenes/icono_paloma.png" Width="40px" />
                <asp:ImageButton ID="btnInfoSol4" runat="server" src="../Imagenes/Requerimiento.png" Width="40px" ToolTip="Solicitar información adicional" />
                <asp:ImageButton ID="btnAceptar4_1" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" ToolTip="Finaliza análisis de la información" />
                <asp:ImageButton ID="btnCorreo4" runat="server" src="../Imagenes/notificar_3.png" Width="40px" ToolTip="Notificación hay o no irregularidad" />
                <img id="btnHome4" runat="server" width="40" onclick="Regresar();" src="../Imagenes/inicio.png" alt="Home" />
                <asp:ImageButton ID="btnDetalle4" runat="server" OnClientClick="setActiveTab('li2'); return false;" Width="40" src="../Imagenes/IrADetallevisita.png" alt="Detalle" ToolTip="Ver Expediente" />
                <img id="btnSalir4" runat="server" width="40" onclick="Regresar();" src="../Imagenes/icono_tache.png" alt="Salir" />
            </asp:Panel>

            <asp:Panel ID="xpnlPaso4_1" runat="server" Visible="true">
                <asp:ImageButton ID="btnAceptar41" runat="server" src="../Imagenes/icono_paloma.png" Width="40px" />
                <img id="btnCorreo41" width="40" onclick="Envio();" src="../Imagenes/notificar_3.png" alt="Correo" />
                <img id="btnHome41" width="40" onclick="Regresar();" src="../Imagenes/inicio.png" alt="Home" />
                <asp:ImageButton ID="btnDetalle41" runat="server" OnClientClick="setActiveTab('li2'); return false;" Width="40" src="../Imagenes/IrADetallevisita.png" alt="Detalle" />
                <img id="btnSalir41" width="40" onclick="Regresar();" src="../Imagenes/icono_tache.png" alt="Salir" />
            </asp:Panel>
            <asp:Panel ID="xpnlPaso5" runat="server" Visible="true">
                <asp:ImageButton ID="btnAceptar5" runat="server" src="../Imagenes/icono_paloma.png" Width="40px" />
                <asp:ImageButton ID="btnCorreo5" runat="server" Width="40px" ImageUrl="~/Imagenes/notificar_3.png" />
                <img id="btnHome5" width="40" onclick="Regresar();" src="../Imagenes/inicio.png" alt="Home" />
                <asp:ImageButton ID="btnAceptar5x15" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" />
                <asp:ImageButton ID="btnDetalle5" runat="server" OnClientClick="setActiveTab('li2'); return false;" Width="40" src="../Imagenes/IrADetallevisita.png" alt="Detalle" />
                <img id="btnSalir5" width="40" runat="server" onclick="Regresar();" src="../Imagenes/icono_tache.png" alt="Salir" />
            </asp:Panel>
            <asp:Panel ID="xpnlPaso6" runat="server" Visible="true">

                <asp:ImageButton ID="btnAceptar6" runat="server" src="../Imagenes/icono_paloma.png" Width="40px" />
                <asp:ImageButton ID="btnSolInfo6" runat="server" src="../Imagenes/Requerimiento.png" Width="40px" ToolTip="Solicitar información adicional" />
                <asp:ImageButton ID="btnCorreo6" runat="server" src="../Imagenes/notificar_3.png" Width="40px" />
                <asp:ImageButton ID="btnHome6" runat="server" src="../Imagenes/inicio.png" Width="40px" OnCommand="btnHome_Command" />
                <asp:ImageButton ID="btnDetalle6" runat="server" OnClientClick="setActiveTab('li2'); return false;" src="../Imagenes/IrADetallevisita.png" alt="Detalle" Width="40px" />
                <asp:ImageButton ID="btnSalir6" runat="server" src="../Imagenes/icono_tache.png" Width="40px" />

            </asp:Panel>
            <asp:Panel ID="xpnlPaso7" runat="server" Visible="true">
                <%--Sye-alejandro.guevara--%>
                <asp:ImageButton ID="btnCancelarOpiP7" runat="server" src="../Imagenes/cancelarVisita.png" Width="40px" ComanName="Cancelar_OPI" CommandArgument="" OnCommand="btnConfirmaCancelacionOPI" ToolTip="Cancelar Oficio" />
                <asp:ImageButton ID="btnNotificarP7" runat="server" src="../Imagenes/notificar_3.png" Width="40px" CommandName="Notificar_Paso7" CommandArgument="7,0" OnCommand="btnNotificar_Command" ToolTip="Notificar envío de Oficio de Observaciones" />
                <%--<img runat="server" id="btnNotif1" width="40" onclick="Notifica(1,0);" src="../Imagenes/notificar_3.png" alt="Correo" />--%>
                <asp:ImageButton ID="btnAceptar7" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" ToolTip="Guardar información" />
                <%--<img runat="server" id="btnNotif1" width="40"  visible="false" onclick="Notifica(3,2);" src="../Imagenes/notificar_3.png" alt="Correo"/>--%>
                <asp:ImageButton ID="ImageButton3" runat="server" src="../Imagenes/inicio.png" Width="40px" OnCommand="btnHome_Command" ToolTip="Ir a bandeja de Oficios" />
                <asp:ImageButton ID="btnDetalle7" runat="server" OnClientClick="setActiveTab('li2'); return false;" Width="40" src="../Imagenes/IrADetallevisita.png" alt="Detalle" ToolTip="Ver expediente" />
                <%--<img id="btnSalir" width="40" onclick="Regresar();" src="../Imagenes/icono_tache.png" alt="Salir"/>--%>
            </asp:Panel>
            <asp:Panel ID="xpnlPaso8" runat="server" Visible="true">
                <asp:ImageButton ID="btnNotificarP8" runat="server" src="../Imagenes/notificar_3.png" Width="40px"
                    CommandName="Notificar_Paso8" CommandArgument="7,0" OnCommand="btnNotificar_Command" ToolTip="Notificar la Respuesta de la AFORE" />
                <asp:ImageButton ID="btnAceptar8" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" ToolTip="Guardar Respuesta AFORE" />
                <asp:ImageButton ID="btnHome8" runat="server" src="../Imagenes/inicio.png" Width="40px" OnCommand="btnHome_Command" />
                <asp:ImageButton ID="btnDetalle8" runat="server" Width="40" OnClientClick="setActiveTab('li2'); return false;" src="../Imagenes/IrADetallevisita.png" alt="Detalle" Style="margin-left: 0px" />
            </asp:Panel>
            <asp:Panel ID="xpnlPaso9" runat="server" Visible="true">
                <asp:ImageButton ID="btnSolicitarP9" runat="server" src="../Imagenes/Requerimiento.png" Width="40px"
                    CommandName="Solicitar_Paso9" CommandArgument="9,0" OnCommand="btnSolicitar_Command" ToolTip="Solicitar información adicional" />
                <asp:ImageButton ID="btnNotificarP9" runat="server" src="../Imagenes/notificar_3.png" Width="40px"
                    CommandName="Notificar_Paso9" CommandArgument="9,0" OnCommand="btnNotificar_Command" ToolTip="Notificar Resolución " />
                <%--<img runat="server" id="btnNotif1" width="40" onclick="Notifica(1,0);" src="../Imagenes/notificar_3.png" alt="Correo" />--%>
                <asp:ImageButton ID="btnAceptar9" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" />
                <%--<img runat="server" id="btnNotif1" width="40"  visible="false" onclick="Notifica(3,2);" src="../Imagenes/notificar_3.png" alt="Correo"/>--%>
                <asp:ImageButton ID="ImageButton4" runat="server" src="../Imagenes/inicio.png" Width="40px" OnCommand="btnHome_Command" />
                <asp:ImageButton ID="btnDetalle9" runat="server" OnClientClick="setActiveTab('li2'); return false;" Width="40" src="../Imagenes/IrADetallevisita.png" alt="Detalle" />
                <%--<img id="btnSalir" width="40" onclick="Regresar();" src="../Imagenes/icono_tache.png" alt="Salir"/>--%>
            </asp:Panel>
            <asp:Panel ID="xpnlPaso10" runat="server" Visible="true">
                <asp:ImageButton ID="btnDejarSEfecto" runat="server" src="../Imagenes/DejarSinEfecto.png" Width="40px" ComanName="DejarSEfecto" OnCommand="btnDejarSinEfecto" ToolTip="Dejar sin efectos el Oficio de Observaciones" />
                <asp:ImageButton ID="btnAprobarSEfecto" runat="server" src="../Imagenes/AprobarDejarSEfecto.png" Width="40px" ComanName="AprobarDejarSEfecto" OnCommand="AprobarSEfecto" ToolTip="Aprobar Dejar sin efectos el Oficio de Observaciones" />
                <asp:ImageButton ID="btnRechazarSEfecto" runat="server" src="../Imagenes/RechazarDejarSEfecto.png" Width="40px" ComanName="RechazarDejarSEfecto" OnCommand="RechazarSEfecto" ToolTip="Rechazar Dejar sin efectos el Oficio de Observaciones" />
                <%--<asp:ImageButton ID="btnDejarSEfecto" runat="server" src="../Imagenes/detener.png" Width="40px" ComanName="DejarSinEfecto_OPI" CommandArgument="" OnCommand="btnDejarSinEfectoOPI" ToolTip="Dejar sin efectos el Oficio de Observaciones" />--%>
                <asp:ImageButton ID="btnNotificarP10" runat="server" src="../Imagenes/notificar_3.png" Width="40px"
                    CommandName="Notificar_Paso10" CommandArgument="10,0" OnCommand="btnNotificar_Command" />
                <%--<img runat="server" id="btnNotif1" width="40" onclick="Notifica(1,0);" src="../Imagenes/notificar_3.png" alt="Correo" />--%>
                <asp:ImageButton ID="btnAceptar10a" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" ToolTip="Guardar información" />
                <asp:ImageButton ID="btnAceptar10b" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" ToolTip="Guardar Información" />
                <asp:ImageButton ID="btnAceptar10b2" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" ToolTip="Guardar Irregularidades" />
                <asp:ImageButton ID="btnAceptar10b3" runat="server" src="../Imagenes/registrarVisita.png" Width="40px" ToolTip="Guardar Dictamen" />
                <%--<img runat="server" id="btnNotif1" width="40"  visible="false" onclick="Notifica(3,2);" src="../Imagenes/notificar_3.png" alt="Correo"/>--%>
                <asp:ImageButton ID="ImageButton6" runat="server" src="../Imagenes/inicio.png" Width="40px" OnCommand="btnHome_Command" ToolTip="Ir a bandeja de Oficios" />
                <asp:ImageButton ID="btnDetalle10" runat="server" OnClientClick="setActiveTab('li2'); return false;" Width="40" src="../Imagenes/IrADetallevisita.png" alt="Detalle" ToolTip="Ver expediente" />
                <%--<img id="btnSalir" width="40" onclick="Regresar();" src="../Imagenes/icono_tache.png" alt="Salir"/>--%>
                <asp:ImageButton ID="btnSiSAN" runat="server" src="../Imagenes/siguiente.png" Width="40px" ToolTip="Enviar registro a SISAN" />
            </asp:Panel>
        </div>
    </div>
    <div id="tab2" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
        <iframe src="ExpedienteOPI.aspx" width="100%" height="700px" style="border-width: 0px;"></iframe>
          <asp:Panel ID="Panel3" runat="server" Visible="true">
            <asp:ImageButton ID="ImageButton8" runat="server" src="../Imagenes/inicio.png" Width="40px" OnCommand="btnHome_Command" ToolTip="Ir a Bandeja de Oficios" />
            <asp:ImageButton ID="ImageButton9" runat="server" OnClientClick="setActiveTab('li2'); return false;" Width="40" src="../Imagenes/IrADetallevisita.png" alt="Detalle" ToolTip="Ver expediente" />
        </asp:Panel>
    </div>
    <div id="tab3" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
        <uc7:Bitacora ID="Bitacora2" runat="server" />
          <asp:Panel ID="Panel2" runat="server" Visible="true">
            <asp:ImageButton ID="ImageButton1" runat="server" src="../Imagenes/inicio.png" Width="40px" OnCommand="btnHome_Command" ToolTip="Ir a Bandeja de Oficios" />
            <asp:ImageButton ID="ImageButton2" runat="server" OnClientClick="setActiveTab('li2'); return false;" Width="40" src="../Imagenes/IrADetallevisita.png" alt="Detalle" ToolTip="Ver expediente" />
        </asp:Panel>
        <br />
    </div>
    <asp:Panel ID="tabExpedienteSISAN" runat="server" Visible="false">
        <div id="tab7" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">
            <uc9:SancionPC runat="server" ID="SancionPC1" Visible="false" />
              <asp:Panel ID="Panel1" runat="server" Visible="true">
            <asp:ImageButton ID="ImageButton5" runat="server" src="../Imagenes/inicio.png" Width="40px" OnCommand="btnHome_Command" ToolTip="Ir a Bandeja de Oficios" />
            <asp:ImageButton ID="ImageButton7" runat="server" OnClientClick="setActiveTab('li2'); return false;" Width="40" src="../Imagenes/IrADetallevisita.png" alt="Detalle" ToolTip="Ver expediente" />
        </asp:Panel>
        </div>

      
    </asp:Panel>

    <asp:Button runat="server" ID="btnAceptarM3B1A" Style="display: none" ClientIDMode="Static" />
    <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
    <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
    <asp:Button runat="server" ID="btnCancelarM2B1A" Style="display: none" ClientIDMode="Static" />
    <asp:Label runat="server" ID="lblTitle" ClientIDMode="Static" Visible="false"></asp:Label>

    <div id="divMensajeRegresar" style="display: none" title='<%="" + lblTitle.Text + "" %>'>
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        ¿Estás seguro que deseas regresar a Bandeja de Oficios?  
                        <br />
                        <br />
                        *Recuerda que al abandonar la pantalla perderás la información que no ha sido guardada.
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divMensajeRechazar" style="display: none" title='<%="" + lblTitle.Text + "" %>'>
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image1" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        ¿Esta seguro que deseas rechazar el registro de SICOD?                      
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divMensajeRespuesta" style="display: none" title='<%="" + lblTitle.Text + "" %>'>
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image2" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <div id="divTextoRespuesta"></div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divMensajeDosBotonesUnaAccion" style="display: none" title='<%="" + lblTitle.Text + "" %>'>
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image3" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div id="divMensajeUnBotonNoAccion" style="display: none" title='<%="" + lblTitle.Text + "" %>'>
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                        <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div id="divMensajeUnBotonUnaAccion" style="display: none" title='<%="" + lblTitle.Text + "" %>'>
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgUnBotonUnaAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%=Mensaje %>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div id="divMensajeDosBotonesUnaAccion_SolicitarProrroga" style="display: none;" title="Solicitud de prórroga">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image4" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                    </div>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <div id="ast2" runat="server" class="AsteriscoHide" style="font-size: small; font-style: normal">* Días de prórroga</div>
                    <asp:TextBox ID="txtDiasProrroga" runat="server" Height="22px" MaxLength="2"
                        onkeypress="javascript:return validanumero(event);"
                        Width="100%" CssClass="txt_gral" Font-Size="Small"></asp:TextBox>
                    <ajx:NumericUpDownExtender ID="UaDExtender" Minimum="1" Maximum="100" Width="200" runat="server"
                        TargetControlID="txtDiasProrroga">
                    </ajx:NumericUpDownExtender>
                </td>
            </tr>

        </table>
    </div>

    <div id="divMensajePreguntaReunion" style="display: none" title='<%="" + lblTitle.Text + "" %>'>
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image5" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div id="divMsjFechaReunion" style="display: none" title='<%="" + lblTitle.Text + "" %>'>
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image6" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                    </div>

                </td>
            </tr>
            <tr>
                <td colspan="2" style="align-content: center; align-items: center">
                    <br />
                    <br />
                    <label id="lblFecha" runat="server" class="txt_gral">* Fecha estimada</label>

                    <asp:TextBox ID="txtFechaReunion4" Width="30%" runat="server"> </asp:TextBox>
                    <ajx:CalendarExtender ID="calEx" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalFechaReunion"
                        TargetControlID="txtFechaReunion4" CssClass="teamCalendar" />
                    <asp:Image ImageAlign="Bottom" ID="imgCalFechaReunion" runat="server" ImageUrl="~/imagenes/Calendar.gif" />
                    <br />
                    <br />
                    <br />
                    <asp:RangeValidator ID="valDateMustBeWithinMinMaxRange" runat="server" ControlToValidate="txtFechaReunion4"
                        ErrorMessage="La fecha seleccionada está fuera del rango permitido" Type="Date" MaximumValue="31/12/2100" MinimumValue="01/01/2019" CssClass="ajax__calendar_container enlaces_rojo " />
                </td>
            </tr>
        </table>
    </div>

    <div id="divMsjReunionP6" class="txt_gral" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image7" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>&nbsp;
                        <label id="lblP6Fecha" runat="server" class="txt_gral"></label>
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
                        ImageUrl="~/Imagenes/OPI_VO.JPG" />
                </td>
            </tr>
        </table>
    </div>

    <div id="divImagenVF" class="txt_gral" style="display: none">
        <table width="100%">
            <tr>
                <td style="text-align: center; vertical-align: top">
                    <asp:Image ID="Image9" runat="server" Width="100%" Height="100%"
                        ImageUrl="~/Imagenes/OPI_VF.JPG" />
                </td>
            </tr>
        </table>
    </div>

    <%--<%--    <%--Sye-alejandro.guevara--%>
    <div id="divConfirmaCancelMotivo" class="txt_gral" style="display: none" title='<%="" + lblTitle.Text + "" %>'>
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image10" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>&nbsp;
                        <%--<label id="Label1" runat="server" class="txt_gral">Ingresa el motivo de la cancelación del oficio de observaciones</label>--%>
                    </div>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <uc11:Motivo1 ID="CancelarOPI1" runat ="server" />

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

    <%--Sye/EC-juan.jose.velazquez--%>
 <%--   <div id="divDejarSinEfecto" class="txt_gral" style="display: none" title='<%="" + lblTitle.Text + "" %>'>
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image11" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%=Mensaje%>&nbsp;
                    </div>
                </td>
            </tr>
            <br />
            <tr>
                <td></td>
                <td>
                    <uc10:Motivo ID="MotivoSE" runat="server" />
                </td>
            </tr>
        </table>
      
    </div>--%>

    <%--Sye/Engine Core-juan.jose.velazquez--%>
    <div id="divConfirmaNoHayRespuesta" class="txt_gral" style="display: none" title='<%="" + lblTitle.Text + "" %>'>
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="Image12" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%=Mensaje%>&nbsp;
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">

        function ImagenMostrar(val) {
            if (val == 1) {
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



        function P6ConfirmaReunion() {
            MsjFechaReunionP6()
        }

        function MsjFechaReunionP6() {
            $("#divMsjReunionP6").dialog({
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
                    "Si": function () {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        $('#btnAceptarM2B1A').trigger("click");
                    },

                    "No": function () {
                        //$(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        $('#btnAceptarM3B1A').trigger("click");
                    },

                    "No se llevó a cabo": function () {
                        $('#btnAceptarM1B1A').trigger("click");

                    }
                }
            });

        }






        function FechaReunion() {
            MsjFechaReunion()
        }

        function MsjFechaReunion() {
            $("#divMsjFechaReunion").dialog({
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
                        $('#btnCancelarM2B1A').trigger("click");
                    }
                }
            });

        }

        function ConfitmaReunion() {
            MensajeConfrimaReunion();
        }

        function MensajeConfrimaReunion() {

            $("#divMensajePreguntaReunion").dialog({
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
                    "Sí": function () {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        $('#btnAceptarM2B1A').trigger("click");
                    },
                    "No": function () {
                        $('#btnAceptarM3B1A').trigger("click");
                    }
                }
            });
        }

        function MensajeConfirmacionProrroga() {
            MensajeDosBotonesUnaAccion_SolicitarProrroga_OPI();
        }

        function MensajeDosBotonesUnaAccion_SolicitarProrroga_OPI() {

            $("#divMensajeDosBotonesUnaAccion_SolicitarProrroga").dialog({
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
                        //var tex = $("#<%=txtDiasProrroga.ClientID%>");
                        //$("#<%=hddiasprorroga.ClientID%>").val(tex.val());
                        //var hf = $("#<%=hddiasprorroga.ClientID%>");
                        //alert(hf.val());
                        $('#btnAceptarM2B1A').trigger("click");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                        $('#btnCancelarM2B1A').trigger("click");
                    }
                }
            });

        }

        function validanumero(e) {
            tecla = (document.all) ? e.keyCode : e.which;
            if (tecla == 8) return true;
            patron = /\d/;
            // patron = /[-][d]*.?[d]*/;
            // patron = /[-][d]/;
            return patron.test(String.fromCharCode(tecla));
        }

        //Sye-alejandro.guevara
        function MensajeConfrimaCancelaOPI() {

            $("#divConfirmaCancelMotivo").dialog({
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
                        $('#btnAceptarM3B1A').trigger("click");
                    }
                }
            });
        }

        function MensajeDejarSinEfecto() {

            $("#divConfirmaCancelMotivo").dialog({
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
                    "Enviar": function () {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        $('#btnAceptarM2B1A').trigger("click");
                    },
                    "Cancelar": function () {
                          var txtName = document.getElementById('<%=CancelarOPI1.FindControl("txtMotivo1").ClientID %>');
                        txtName.value = "";
                        $('#btnAceptarM3B1A').trigger("click");
                    }
                }
            });
        }

        function MensajeConfirmaNoHayRespuesta() {

            $("#divConfirmaNoHayRespuesta").dialog({
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
                    "Sí": function () {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        $('#btnAceptarM2B1A').trigger("click");
                    },
                    "No": function () {
                        $('#btnAceptarM3B1A').trigger("click");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }

    </script>
    <asp:Button ID="btnEliminar" runat="server" Style="display: none" />
    <asp:Button ID="btnGuardar" runat="server" Style="display: none" />
    <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfCurrentTab" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hddiasprorroga" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HiddenField2" runat="server" ClientIDMode="Static" />
</asp:Content>

