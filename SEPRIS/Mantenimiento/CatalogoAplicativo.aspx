<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="CatalogoAplicativo.aspx.vb" Inherits="SEPRIS.CatalogoAplicativo" %>

<%@ Register Src="../Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Controles/wucPersonalizarColumnas.ascx" TagName="wucPersonalizarColumnas" TagPrefix="wuc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server" >
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" Visible="true" BorderStyle="None" 
                HorizontalAlign="Center" >
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                    Catálogo de Aplicativo</label>
                </div>
                <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                    <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                    &nbsp;
                </div>
                <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
                <br />
                <cc1:CustomGridView ID="gvConsulta"  runat="server" DataKeyNames="N_ID_APLICATIVO" AllowSorting="True"
                    Width="100%" ColumnasCongeladas="0" DataBindEnPostBack="True" HabilitaScroll="false"
                    HeigthScroll="0" ToolTipHabilitado="True" UnicoEnPantalla="True" WidthScroll="0">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                            <HeaderStyle Width="15px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ID" DataField="N_ID_APLICATIVO" 
                            SortExpression="N_ID_APLICATIVO" >
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Clave" DataField="T_CVE_APLICATIVO" 
                            SortExpression="T_CVE_APLICATIVO" >
                            <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Acrónimo" DataField="T_DSC_ACRONIMO_APLICATIVO" 
                            SortExpression="T_DSC_ACRONIMO_APLICATIVO" >
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Descripción Aplicativo" DataField="T_DSC_APLICATIVO" SortExpression="T_DSC_APLICATIVO" ItemStyle-CssClass="Wrap" />
                        <asp:BoundField HeaderText="Ingeniero Responsable" DataField="NOMBRE_RESPONSABLE" 
                            SortExpression="NOMBRE_RESPONSABLE" >
                            <ItemStyle Width="175px" />
                            <HeaderStyle Width="175px" />
                            <ItemStyle CssClass="Wrap" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Ingeniero Backup" DataField="NOMBRE_BACKUP" 
                            SortExpression="NOMBRE_BACKUP" >
                            <ItemStyle Width="175px" />
                            <HeaderStyle Width="175px" />
                            <ItemStyle CssClass="Wrap" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Estatus" SortExpression="B_FLAG_VIG">
                            <ItemTemplate>
                                <asp:Image ID="imagenEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "B_FLAG_VIG"))  %>' />
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:TemplateField>
                    </Columns>
                </cc1:CustomGridView>
                <img id="Noexisten" runat="server" src="../Imagenes/No%20Existen.gif" visible="false" alt="No existen Registos para la Consulta" />
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
                    <asp:Label ID="lblTituloRegistro" runat="server" CssClass="TitulosWebProyectos" Text="Alta de Aplicativo" EnableTheming="false"></asp:Label>
                </div>
                <asp:Panel ID="pnlControles" runat="server">
                    <table>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    ID:</label>
                            </td>
                            <td align="left" width="300px">
                                <asp:TextBox ID="txtId" runat="server" CssClass="txt_solo_lectura" Enabled="false"
                                    Width="100px"></asp:TextBox>
                            </td>
                            <td align="left">
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Clave:</label>
                            </td>
                            <td align="left" width="300px">
                                <asp:TextBox ID="txtClave" runat="server" CssClass="txt_gral" MaxLength="5"
                                    Width="100px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftxtClave" runat="server"
                                    TargetControlID="txtClave" FilterType="Custom, Numbers">
                                </asp:FilteredTextBoxExtender>
                                <asp:CustomValidator ID="cvClave" runat="server" ControlToValidate="txtClave" 
                                    Display="Dynamic" EnableClientScript="false" ForeColor="Red" 
                                    ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Acrónimo*:</label>
                            </td>
                            <td align="left" width="300px">
                                <asp:TextBox ID="txtAcronimo" runat="server" CssClass="txt_gral" Width="300px"
                                    ValidationGroup="Forma" MaxLength="20">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:CustomValidator ID="cvAcronimo" runat="server" 
                                    ControlToValidate="txtAcronimo" Display="Dynamic" EnableClientScript="false" 
                                    ForeColor="Red" ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Descripción Aplicativo*:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAplicativo" runat="server" CssClass="txt_gral" Width="300px"
                                    ValidationGroup="Forma" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                <asp:CustomValidator ID="cvAplicativo" runat="server" 
                                    ControlToValidate="txtAplicativo" Display="Dynamic" EnableClientScript="false" 
                                    ForeColor="Red" ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Ingeniero Responsable*:</label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlIngResponsable" runat="server" CssClass="txt_gral" Enabled="true"
                                    Width="100%">
                                </asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvddlIngResponsable" runat="server" 
                                    ControlToValidate="ddlIngResponsable" Display="Dynamic" 
                                    EnableClientScript="false" ForeColor="Red" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Ingeniero Backup*:</label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlIngBackup" runat="server" CssClass="txt_gral" Enabled="true"
                                    Width="100%">
                                </asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvddlIngBackup" runat="server" 
                                    ControlToValidate="ddlIngBackup" Display="Dynamic" EnableClientScript="false" 
                                    ForeColor="Red" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>    
                        <tr>
                            <td>
                            </td>
                            <td width="300px">
                            </td>
                            <td>
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
