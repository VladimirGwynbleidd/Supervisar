<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="CatalogoNivelServicioPaqueteServicio.aspx.vb" Inherits="SEPRIS.CatalogoNivelServicioPaqueteServicio" %>

<%@ Register Assembly="CustomGridView" Namespace="CustomGridView" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            MensajeDosBotonesUnaAccionLoad();
            MensajeUnBotonNoAccionLoad();
        });

        function LevantaVentanaConfirma() {
            MensajeDosBotonesUnaAccion();
        }

        function MuestraMensajeUnBotonNoAccion() {
            MensajeUnBotonNoAccion();
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Catálogo Niveles de Servicio de Paquete de Servicio</label>
                </div>
                <br />
                <table align="center">
                    <tr>
                        <td>
                            <asp:Label ID="lblPaqueteServicio" runat="server" Text="Paquete de Servicio:" CssClass="txt_gral"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPaqueteServicio" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="pnlSubMenus" runat="server" Style="display: none">
                    <table align="center">
                        <tr>
                            <td valign="top" style="width: 300px;">
                                <asp:Button ID="btnExportaExcelUsados" runat="server" Text="Exportar a Excel" />
                                <asp:Label ID="lblUsados" runat="server" Text="Niveles de Servicio Agregados" CssClass="txt_gral"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td valign="top" style="width: 300px;">
                                <asp:Button ID="btnExportaExcelDisponibles" runat="server" Text="Exportar a Excel" />
                                <asp:Label ID="lblDisponibles" runat="server" Text="Niveles de Servicio Disponibles"
                                    CssClass="txt_gral"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <div id="Div2" style="text-align: left; vertical-align: top;">
                                    <cc1:CustomGridView ID="gvUsados" runat="server" Width="100%" SkinID="SeleccionMultipleCliente"
                                        HabilitaScroll="true" HeigthScroll="400" WidthScroll="435" UnicoEnPantalla="false"
                                        ToolTipHabilitado="false">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkElemento" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="ID" DataField="Identificador" />
                                            <asp:BoundField HeaderText="Nivel de Servicio" DataField="Descripcion" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </cc1:CustomGridView>
                                    <asp:Image ID="imgUsados" runat="server" ImageUrl="~/Imagenes/No Existen.gif" Visible="false" />
                                </div>
                            </td>
                            <td valign="middle">
                                <asp:Button ID="btnRemueve" runat="server" Text=">>" />
                                <br />
                                <br />
                                <asp:Button ID="btnAgrega" runat="server" Text="<<" />
                            </td>
                            <td valign="top">
                                <div id="Div1" style="text-align: left; vertical-align: top;">
                                    <cc1:CustomGridView ID="gvDisponibles" runat="server" Width="100%" SkinID="SeleccionMultipleCliente"
                                        HabilitaScroll="true" HeigthScroll="400" WidthScroll="435" UnicoEnPantalla="false"
                                        ToolTipHabilitado="false">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkElemento" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="ID" DataField="Identificador" />
                                            <asp:BoundField HeaderText="Nivel de Servicio" DataField="Descripcion" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </cc1:CustomGridView>
                                    <asp:Image ID="imgDisponibles" runat="server" ImageUrl="~/Imagenes/No Existen.gif"
                                        Visible="false" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
                </asp:Panel>
                <div align="center">
                    <asp:Image ID="imgDatos" runat="server" ImageUrl="~/Imagenes/No Existen.gif" Visible="false" />
                </div>
                <table align="center" id="tblBotones" runat="server" visible="false">
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" OnClientClick="Deshabilita(this);" />
                            <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
                        </td>
                        <td>
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                        </td>
                    </tr>
                </table>
                <div id="divMensajeDosBotonesUnaAccion" style="display: none">
                    <table width="100%">
                        <tr>
                            <td style="width: 50px; text-align: center; vertical-align: top">
                                <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px"
                                    ImageUrl="~/Imagenes/Errores/Error1.png" />
                            </td>
                            <td style="text-align: left">
                                <div class="MensajeModal-UI">
                                    <%= Mensaje %>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divMensajeUnBotonNoAccion" style="display: none">
                    <table width="100%">
                        <tr>
                            <td style="width: 50px; text-align: center; vertical-align: top">
                                <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                            </td>
                            <td style="text-align: left">
                                <div class="MensajeModal-UI">
                                    <asp:Label ID="lblMensaje" runat="server" CssClass="MensajeModal-UI" EnableTheming="false"></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
            <asp:PostBackTrigger ControlID="btnExportaExcelDisponibles" />
            <asp:PostBackTrigger ControlID="btnExportaExcelUsados" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
