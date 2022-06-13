<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="CatalogoPeriodosVacacionales.aspx.vb" Inherits="SEPRIS.CatalogoPeriodosVacacionales" %>
<%@ Register Src="../Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Catálogo de Periodos Vacacionales</label>
                </div>
                <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                    <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                    &nbsp;
                 </div>
                <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
                <br />
                <cc1:CustomGridView ID="gvConsulta" AllowSorting="true" runat="server" DataKeyNames="N_ID_VACACIONES"
                    Width="100%">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                            <HeaderStyle Width="15px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ID" DataField="N_ID_VACACIONES" SortExpression="N_ID_VACACIONES" >
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:boundfield>
                        <asp:BoundField HeaderText="Usuario" DataField="T_ID_USUARIO" SortExpression="T_ID_USUARIO" />
                        <asp:BoundField HeaderText="Fecha de inicio del periodo" DataField="F_FECH_INICIO_PERIODO" SortExpression="F_FECH_INICIO_PERIODO" />
                        <asp:BoundField HeaderText="Fecha de fin del periodo" DataField="F_FECH_FIN_PERIODO" SortExpression="F_FECH_FIN_PERIODO" />
                        <asp:BoundField HeaderText="Días asignados al usuario" DataField="N_NUM_DIAS_ASIGNADOS" SortExpression="N_NUM_DIAS_ASIGNADOS" />
                        <asp:BoundField HeaderText="Días consumidos por el usuario" DataField="N_NUM_DIAS_CONSUMIDOS" SortExpression="N_NUM_DIAS_CONSUMIDOS" />
                        <asp:TemplateField HeaderText="Estatus" SortExpression="N_FLAG_VIG">
                            <ItemTemplate>
                                <asp:Image ID="imagenEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "B_FLAG_VIG"))  %>' />
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
                    <asp:Label ID="lblTituloRegistro" runat="server" CssClass="TitulosWebProyectos" Text="Alta de Periodo Vacacional" EnableTheming="false"></asp:Label>
                </div>
                <asp:Panel ID="pnlControles" runat="server">
                    <table style="border-collapse:collapse">
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    ID:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtID" runat="server" CssClass="txt_solo_lectura" Enabled="false"
                                    Width="100px">1</asp:TextBox>
                            </td>
                            <td>
                            
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Usuario*:</label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUsuario" runat="server" CssClass="txt_gral" Enabled="true"
                                    Width="100%">
                                </asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvUsuario" runat="server" 
                                    ControlToValidate="ddlUsuario" Display="Dynamic" 
                                    EnableClientScript="false" ForeColor="Red" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Fecha de inicio del periodo*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtFechaIni" runat="server" CssClass="txt_gral" Width="200px"
                                    ValidationGroup="Forma" MaxLength="10"></asp:TextBox>
                                <asp:CalendarExtender ID="cldFechaIni" runat="server" TargetControlID="txtFechaIni"
                                    Enabled="True" PopupButtonID="imgFec3" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td align="left">
                                <asp:Image ID="imgFec3" runat="server" src="/Controles/filtro/imagenes/Calendar.gif"
                                        Style="cursor: hand" ImageUrl="~/Controles/filtro/imagenes/Calendar.GIF" 
                                        EnableTheming="True"/>
                                <asp:CustomValidator ID="cvFechaIni"
                                        ControlToValidate="txtFechaIni" ValidationGroup="Forma" ValidateEmptyText="true"
                                        runat="server" EnableClientScript="false" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Fecha de fin del periodo*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtFechaFin" runat="server" CssClass="txt_gral" Width="200px"
                                    ValidationGroup="Forma" MaxLength="10"></asp:TextBox>
                                <asp:CalendarExtender ID="cldFechaFin" runat="server" TargetControlID="txtFechaFin"
                                    Enabled="True" PopupButtonID="imgFech" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td align="left">
                                <asp:Image ID="imgFech" runat="server" src="/Controles/filtro/imagenes/Calendar.gif"
                                        Style="cursor: hand" ImageUrl="~/Controles/filtro/imagenes/Calendar.GIF" 
                                        EnableTheming="True"/>
                                <asp:CustomValidator ID="cvFechaFin"
                                        ControlToValidate="txtFechaFin" ValidationGroup="Forma" ValidateEmptyText="true"
                                        runat="server" EnableClientScript="false" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Días asignados al usuario*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtDiasAsignados" runat="server" CssClass="txt_gral" Width="100px"
                                    ValidationGroup="Forma" MaxLength="2"></asp:TextBox>
                                <asp:CustomValidator ID="cvDiasAsignados" runat="server" 
                                    ControlToValidate="txtDiasAsignados" Display="Dynamic" 
                                    EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true" 
                                    ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Días consumidos por el usuario*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtDiasConsumidos" runat="server" CssClass="txt_gral" Width="100px"
                                    ValidationGroup="Forma" MaxLength="2"></asp:TextBox>
                                <asp:CustomValidator ID="cvDiasCunsumidos" runat="server" 
                                    ControlToValidate="txtDiasConsumidos" Display="Dynamic" 
                                    EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true" 
                                    ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                            </td>
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
