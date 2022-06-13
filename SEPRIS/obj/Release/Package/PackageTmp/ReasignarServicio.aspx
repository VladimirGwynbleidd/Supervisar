<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="ReasignarServicio.aspx.vb" Inherits="SEPRIS.ReasignarServicio" %>

<%@ Register Assembly="CustomGridView" Namespace="CustomGridView" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controles/wucAyuda.ascx" TagName="wucAyuda" TagPrefix="wcu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function Mensaje800x400Load() {

            $("#divMensaje800x400").dialog({
                resizable: false,
                width: 800,
                height: 400,
                modal: true,
                autoOpen: false,
                buttons: {
                    "Aceptar": function () {
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true}]);
                        $('#btnAceptarInges').trigger("click");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });

        }
        $(function () {
            MensajeUnBotonNoAccionLoad();
            MensajeUnBotonUnaAccionLoad();
            MensajeDosBotonesUnaAccionLoad();
            Mensaje800x400Load();
        });

        function AquiMuestroMensaje() {
            MensajeUnBotonNoAccion();
        };

        function ConfirmacionEliminar() {
            MensajeDosBotonesUnaAccion();
        };

        function MensajeFinalizar() {
            MensajeUnBotonUnaAccion();
        }

        function MensajeConfirmacion() {
            MensajeDosBotonesUnaAccion();
        }

        function MensajeCorreccion() {
            MensajeUnBotonNoAccion();
        };

        function Mensaje800x400() {

            $("#divMensaje800x400").dialog({
                resizable: false,
                width: 800,
                height: 400,
                modal: true,
                autoOpen: true,
                closeText: 'Cerrar',
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
                        $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true}]);
                        $('#btnAceptarInges').trigger("click");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }

            });

        }

        function MensajeAtencion() {
            Mensaje800x400();
        };

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <wcu:wucAyuda ID="Ayuda" runat="server" />
    <asp:UpdatePanel ID="udpReasignacion" runat="server">
        <ContentTemplate>
            <div align="center" style="padding: 20px 20px 15px 20px">
                <label class="TitulosWebProyectos">
                    Reasignar Servicio</label>
            </div>
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblFolioSolicitud" runat="server" Text="Folio Solicitud:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFolioSolicitud" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CausesValidation="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="regexFolio" ControlToValidate="txtFolioSolicitud"
                                ValidationExpression="^[0-9]+-[0-9]+$" ForeColor="Red" CssClass="txt_gral" ErrorMessage="El formato del folio no es correcto"
                                runat="server" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div style="text-align: left;">
                <cc1:CustomGridView ID="gvServicios" runat="server" Width="100%" SkinID="SeleccionSimpleCliente"
                    HabilitaScroll="false" UnicoEnPantalla="false" ToolTipHabilitado="false" AutoGenerateColumns="false"
                    DataKeyNames="N_ID_NIVELES_SERVICIO">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Servicio" DataField="SERVICIO" />
                        <asp:BoundField HeaderText="Ingeniero asignado" DataField="INGENIERO_ASIGNADO" />
                        <asp:BoundField HeaderText="Fecha estimada de termino" DataField="F_FECH_TENTATIVA_TERMINO_SOLICITUD" />
                        <asp:TemplateField HeaderText="Estatus" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Image ID="imagenEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "N_ID_ESTATUS_SERVICIO"))  %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </cc1:CustomGridView>
                <p style="text-align: center; width: 100%;">
                    <asp:Image ID="imgNoServicios" runat="server" ImageUrl="~/Imagenes/No Existen.gif"
                        Visible="false" />
                </p>
                <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
            </div>
            <br />
            <div>
                <asp:Image ID="imgERROR" runat="server" />
                &nbsp;
                <label class="txt_gral">
                    Asignado</label>
                &nbsp;&nbsp;
                <asp:Image ID="Image1" runat="server" Width="17px"  />
                &nbsp;
                <label class="txt_gral">
                    Vencimiento interno</label>
                &nbsp;&nbsp;
                <asp:Image ID="Image2" runat="server" Width="17px"  />
                &nbsp;
                <label class="txt_gral">
                    Vencido</label>
            </div>
            <br />
            <br />
            <div>
                <asp:Button ID="btnReasignar" runat="server" Text="Reasignar" OnClientClick="Deshabilita(this);"
                    CausesValidation="false" />
                &nbsp;&nbsp;
                <asp:Button ID="btnAgendarFinal" runat="server" Text="Agendar al final" OnClientClick="Deshabilita(this);"
                    CausesValidation="false" />
            </div>
            <div id="divMensajeUnBotonNoAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                                <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="valFirmaElectronica"
                                    CssClass="txt_gral" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Button runat="server" ID="btnSalirM2B2A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnContinuarM2B2A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnConsulta" Style="display: none" ClientIDMode="Static" />
            <div id="divMensajeUnBotonUnaAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgUnBotonUnaAccion" runat="server" Width="32px" Height="32px" ImageUrl="" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divMensajeDosBotonesUnaAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px"
                                ImageUrl="" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divMensaje800x400" style="display: none" title="Reasignar Servicio">
                <table width="100%">
                    <tr>
                        <td style="text-align: center; vertical-align: top">
                            <div class="MensajeModal-UI">
                                <cc1:CustomGridView ID="gvIngenieros" runat="server" Width="100%" SkinID="SeleccionSimpleCliente"
                                    HabilitaScroll="false" UnicoEnPantalla="false" ToolTipHabilitado="false" AutoGenerateColumns="false"
                                    DataKeyNames="T_ID_INGENIERO_RESPONSABLE" HiddenFieldSeleccionSimple="hfSelectedValueInge">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkElemento" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Ingeniero" DataField="INGENIERO_ASIGNADO" />
                                        <asp:BoundField HeaderText="Fecha de diponibilidad" DataField="FECHA_DISPONIBLE" />
                                    </Columns>
                                </cc1:CustomGridView>
                                <p style="text-align: center; width: 100%;">
                                    <asp:Image ID="imgNoInges" runat="server" ImageUrl="~/Imagenes/No Existen.gif" Visible="false" />
                                </p>
                                 <asp:HiddenField ID="hfSelectedValueInge" runat="server" ClientIDMode="Static" Value="-1" />
                                <asp:Button ID="btnAceptarInges" runat="server" Text="Aceptar" ClientIDMode="Static" Style="display: none"/>
                                &nbsp;&nbsp;
                                <asp:Button ID="btnCancelarInges" runat="server" Text="Cancelar" ClientIDMode="Static" Style="display: none"/>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAceptar" />
            <asp:PostBackTrigger ControlID="btnReasignar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
