﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="CatalogoSubmenu.aspx.vb" Inherits="SEPRIS.CatalogoSubmenu" %>

<%@ Register Src="../Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportaExcel" />
            <asp:PostBackTrigger ControlID="btnRegresar" />
<asp:PostBackTrigger ControlID="btnSalirM2B2A"></asp:PostBackTrigger>
<asp:PostBackTrigger ControlID="btnContinuarM2B2A"></asp:PostBackTrigger>
<asp:PostBackTrigger ControlID="btnAceptarM1B1A"></asp:PostBackTrigger>
<asp:PostBackTrigger ControlID="btnAceptarM2B1A"></asp:PostBackTrigger>
<asp:PostBackTrigger ControlID="btnConsulta"></asp:PostBackTrigger>
<asp:PostBackTrigger ControlID="btnAceptar"></asp:PostBackTrigger>
<asp:PostBackTrigger ControlID="btnRegresar"></asp:PostBackTrigger>
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Catálogo de Submenús</label>
                </div>
                <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                    <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                </div>
                <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
                <br />
                <cc1:CustomGridView ID="gvConsulta" runat="server" DataKeyNames="N_ID_MENU,N_ID_SUBMENU"
                    Width="100%" AllowSorting="true" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                            <HeaderStyle Width="15px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Clave" DataField="N_ID_SUBMENU" SortExpression="N_ID_SUBMENU" >
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Descripción" DataField="T_DSC_SUBMENU" SortExpression="T_DSC_SUBMENU" />
                        <asp:BoundField HeaderText="Url Página" DataField="T_DSC_URL_PAGINA" SortExpression="T_DSC_URL_PAGINA" />
                        <asp:TemplateField HeaderText="Estatus" SortExpression="N_FLAG_VIG">
                            <ItemTemplate>
                                <asp:Image ID="imagenEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "N_FLAG_VIG"))  %>' />
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:TemplateField>
                    </Columns>
                </cc1:CustomGridView>
                <div id="pnlNoExiste" runat="server" align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Image ID="Image1" runat="server" AlternateText="No existen registros para la consulta"
                        ImageAlign="Middle" ImageUrl="../Imagenes/no EXISTEN.gif" />
                </div>
                <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
                <br />
                <br />
                <asp:Image ID="imgOK" runat="server" />
                <label class="txt_gral">
                    Vigente</label>
                <asp:Image ID="imgERROR" runat="server" />
                <label class="txt_gral">
                    No vigente</label>
                <br />
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnModificar" runat="server" Text="Modificar" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" />
            </asp:Panel>
            <asp:Panel ID="pnlRegistro" runat="server" Visible="false">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Label ID="lblTituloRegistro" runat="server" CssClass="TitulosWebProyectos" Text="Alta de Submenú" EnableTheming="false"></asp:Label>
                </div>
                <asp:Panel ID="pnlControles" runat="server">
                    <table>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Clave:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtClave" runat="server" CssClass="txt_solo_lectura" Enabled="false"
                                    Width="100px"></asp:TextBox>
<%--                                <asp:CustomValidator ID="cvClave" ControlToValidate="txtClave" ValidationGroup="Forma"
                                    ValidateEmptyText="true" runat="server" EnableClientScript="false" Display="Dynamic"
                                    ForeColor="Red">*</asp:CustomValidator>--%>
                            </td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Menú*:</label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlMenu" runat="server" AutoPostBack="true" Width="100%" CssClass="txt_gral">
                                </asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvMenu" ControlToValidate="ddlMenu" ValidationGroup="Forma"
                                    runat="server" EnableClientScript="false" Display="Dynamic"
                                    ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Descripción Submenú*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtDescripcionSub" runat="server" CssClass="txt_gral" Width="300px"
                                    ValidationGroup="Forma"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvDescripcionSub" runat="server" 
                                    ControlToValidate="txtDescripcionSub" Display="Dynamic" 
                                    EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true" 
                                    ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Url Página*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtUrlPagina" runat="server" CssClass="txt_gral" Width="300px" ValidationGroup="Forma"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvUrlPagina" runat="server" 
                                    ControlToValidate="txtUrlPagina" Display="Dynamic" EnableClientScript="false" 
                                    ForeColor="Red" ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Descripción de la página*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtDescripcionPagina" runat="server" CssClass="txt_gral" Width="300px"
                                    ValidationGroup="Forma"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvDescPagina" runat="server" 
                                    ControlToValidate="txtDescripcionPagina" Display="Dynamic" 
                                    EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true" 
                                    ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Número de Orden*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtOrden" runat="server" CssClass="txt_gral" Width="300px" ValidationGroup="Forma"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvOrden" runat="server" ControlToValidate="txtOrden" 
                                    Display="Dynamic" EnableClientScript="false" ForeColor="Red" 
                                    ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Visible:</label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="cbVisible" runat="server" />
                            </td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlBotones" runat="server">
                    <table>
                        <tr>
                            <td colspan="2" align="left">
                                <label class="txt_gral">
                                    *Datos Obligatorios</label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClientClick="Deshabilita(this);" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlRegresar" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnRegresar" runat="server" Text="Regresar" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <div id="divMensajeUnBotonNoAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align:top">
                            <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                            <%= Mensaje%>
                            <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div><div id="divConfirmacionM2B2A" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align:top">
                            <asp:Image ID="imgM2B2A" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divMensajeUnBotonUnaAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align:top">
                            <asp:Image ID="imgUnBotonUnaAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divMensajeDosBotonesUnaAccion" style="display: none" class="MensajeModal-UI">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align:top">
                            <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
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
            <script type="text/javascript">
                $(function () {

                    MensajeUnBotonNoAccionLoad();
                    MensajeUnBotonUnaAccionLoad();
                    MensajeDosBotonesUnaAccionLoad();

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

            </script>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSalirM2B2A" />
            <asp:PostBackTrigger ControlID="btnContinuarM2B2A" />
            <asp:PostBackTrigger ControlID="btnAceptarM1B1A" />
            <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
            <asp:PostBackTrigger ControlID="btnConsulta" />
            <asp:PostBackTrigger ControlID="btnAceptar" />
            <asp:PostBackTrigger ControlID="btnRegresar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
