<%@ Page Title="" Language="vb" AutoEventWireup="false" ValidateRequest="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="CatalogoAyuda.aspx.vb" Inherits="SEPRIS.CatalogoAyuda" %>

<%@ Register Src="../Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/jscript">   
        function validaLimite(obj, maxchar) {
            if (this.id) obj = this;
            var remaningChar = maxchar - obj.value.length;
            if (remaningChar <= 0) {
                obj.value = obj.value.substring(maxchar, 0);
                return false;
            }
            else
            { return true; }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Catálogo de Temas de&nbsp; Ayuda</label>
                </div>
                <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                    <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                </div>
                <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
                <br />
                <cc1:CustomGridView ID="gvConsulta" AllowSorting="true" runat="server" DataKeyNames="N_ID_MENU, N_ID_SUBMENU, N_ID_AYUDA"
                    Width="100%">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                            <HeaderStyle Width="15px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Clave" DataField="N_ID_AYUDA" SortExpression="N_ID_AYUDA" >
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Título" DataField="T_DSC_TITULO" SortExpression="T_DSC_TITULO" />
                        <asp:BoundField HeaderText="Contenido" DataField="T_DSC_CONTENIDO" SortExpression="T_DSC_CONTENIDO"
                            HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-CssClass="Wrap" />
                        <asp:TemplateField HeaderText="Estatus" SortExpression="N_FLAG_VIG">
                            <ItemTemplate>
                                <asp:Image ID="imagenEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "N_FLAG_VIG"))  %>' />
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:TemplateField>
                    </Columns>
                </cc1:CustomGridView>
                <img id="Noexisten" runat="server" src="../Imagenes/No%20Existen.gif" visible="false" />
                <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />
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
                    <asp:Label ID="lblTituloRegistro" runat="server" CssClass="TitulosWebProyectos" Text="Alta de Tema de Ayuda" EnableTheming="false"></asp:Label>
                </div>
                <asp:Panel ID="pnlControles" runat="server">
                    <table>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Menú*:</label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlMenu" runat="server" CssClass="txt_gral" Enabled="true"
                                    Width="50%" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvddlMenu" runat="server" ControlToValidate="ddlMenu" 
                                    Display="Dynamic" EnableClientScript="false" ForeColor="Red" 
                                    ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Submenú*:</label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlSubMenu" runat="server" CssClass="txt_gral" Enabled="false"
                                    Width="50%" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvddlSubMenu" runat="server" 
                                    ControlToValidate="ddlSubMenu" Display="Dynamic" EnableClientScript="false" 
                                    ForeColor="Red" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Ayuda*:</label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlAyuda" runat="server" CssClass="txt_gral" Enabled="true"
                                    Width="50%">
                                </asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvddlAyuda" runat="server" 
                                    ControlToValidate="ddlAyuda" Display="Dynamic" EnableClientScript="false" 
                                    ForeColor="Red" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Título*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtTitulo" runat="server" CssClass="txt_gral" Enabled="true" Width="700px"
                                    ValidationGroup="Forma"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvTitulo" runat="server" ControlToValidate="txtTitulo" 
                                    Display="Dynamic" EnableClientScript="false" ForeColor="Red" 
                                    ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Contenido*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtContenido" runat="server" CssClass="txt_gral" Width="700px" onkeyup="validaLimite(this,5000)"
                                    ValidationGroup="Forma" Rows="20" TextMode="MultiLine" onkeypress="ReemplazaCEspeciales(this.id)" 
                                   onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                            </td>
                            <td align="left" valign="bottom">
                                <asp:CustomValidator ID="cvContenido" runat="server" 
                                    ControlToValidate="txtContenido" Display="Dynamic" EnableClientScript="false" 
                                    ForeColor="Red" ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Orden:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtOrden" runat="server" CssClass="txt_gral" Width="350px" onkeypress="javascript:return validanumero(event);"></asp:TextBox>
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
                            <td colspan="2">
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
            </div>

            <div id="divConfirmacionM2B2A" style="display: none">
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
            <div id="divMensajeDosBotonesUnaAccion" style="display: none">
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

                function validanumero(e) {
                    tecla = (document.all) ? e.keyCode : e.which;
                    if (tecla == 8) return true;
                    patron = /\d/;
                    // patron = /[-][d]*.?[d]*/;
                    // patron = /[-][d]/;
                    return patron.test(String.fromCharCode(tecla));
                }


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
            <asp:PostBackTrigger ControlID="btnExportaExcel" />
            <asp:PostBackTrigger ControlID="btnRegresar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
