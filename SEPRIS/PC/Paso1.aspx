<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Paso1.aspx.vb" Inherits="SEPRIS.Paso1" EnableEventValidation="false" %> 

<%@ Register Src="UserControls/Actividades.ascx" TagName="Actividades" TagPrefix="uc1" %>
<%@ Register Src="UserControls/DetalleSICOD.ascx" TagName="DetalleSICOD" TagPrefix="uc2" %>

<%@ Register Src="UserControls/Supervisor.ascx" TagName="Supervisor" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Styles/TabsV1.css" rel="stylesheet" type="text/css" />
    <link href="../Site.css" rel="stylesheet" />

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


        function GuardarBitacora(folio, usuario, paso, accion,comentarios) {
            $.ajax({
                type: "POST",
                url: "DetallePC.aspx/GuardarBitacota",
                contentType: "application/json;charset=utf-8",
                data: '{Folio: ' + folio +
                ', Usuario: "' + usuario +
                '", Paso: "' + paso +
                    '", Accion: "' + accion +
                    '", Comentarios: "' + comentarios +'\"}',
                dataType: "json",
                success: function (data) {
                },
                error: function (result) {
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
                        $(location).attr('href', '../BandejaSICOD.aspx');
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
            if (Page_ClientValidate("ucSupervisor")) {

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
                            AsignarPC();
                        },
                        "Cancelar": function () {
                            $(this).dialog("close");
                        }
                    }
                });

            }
            else {

                $('#divTextoRespuesta').text('Es necesario completar la información necesaria para el PC. Por favor completa los siguientes datos:');

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
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div style="float: left; width: 92%; text-align: center; position: absolute">
        <asp:Label ID="lblFolio" runat="server" Style="font-weight: bold; font-size: 13px; color: black;"></asp:Label><br />
    </div>
    <div id="tabs_wrapper" style="margin-top: 20px; height: 47px;">
        <ul id="Ul2" class="tabs" runat="server" style="width: 100%;">
            <li id="li1"><a href="#tab1">Información PC</a></li>
            <li id="li2"><a href="#tab2">Bitácora de acciones</a></li>
        </ul>
    </div>

    <div style="width: 100%; float: left;">
        <div id="tab1" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">

            <asp:Panel runat="server" ID="pnlDetalleSicod" Enabled="false">
            <uc2:DetalleSICOD ID="DetalleSICOD1" runat="server" />
            </asp:Panel>

            <hr />

            <asp:Panel runat="server" ID="pnlSupervisor" >
            <uc3:Supervisor ID="Supervisor1" runat="server" />
            </asp:Panel>
            <br />
            <div>
                <asp:ImageButton ID="btnRechazar" runat="server" Width="40" ImageUrl="~/Imagenes/RechazarDocto.png" OnClientClick="Rechazar(); return false;" ToolTip="Rechazar folio y regresar control a SICOD" />
                <asp:ImageButton ID="btnRegresar" runat="server" Width="40" ImageUrl="~/Imagenes/inicio.png" OnClientClick="Regresar(); return false;" ToolTip="Regresar a Bandeja SICOD"/>
                <asp:ImageButton ID="btnAceptar" runat="server" Width="40" ImageUrl="~/Imagenes/AprobarDocto.png" OnClientClick="Aceptar(); return false;" ToolTip="Aceptar y asignar PC a Supervisor"/>
            </div>
            <br />
        </div>
        <div id="tab2" class="tab_content" style="horizontal-align: left; width: 100%; border: 1px solid #999; font-family: Verdana; font-size: 0.8em; padding: 0px; height: auto;">

            <br />
        </div>
    </div>


    <div id="divMensajeRegresar" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px"
                        ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        ¿Estás seguro que deseas cancelar la asignación del PC?  
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
                        <asp:ValidationSummary ID="ValidationSummarySupervisor" runat="server" ValidationGroup="ucSupervisor" CssClass="MensajeModal-UI" />
                    </div>
                </td>
            </tr>
        </table>
    </div>



    <asp:Button ID="btnEliminar" runat="server" Style="display: none" />
    <asp:Button ID="btnGuardar" runat="server" Style="display: none" />
    <asp:HiddenField ID="hfCurrentTab" runat="server" ClientIDMode="Static" />

</asp:Content>
