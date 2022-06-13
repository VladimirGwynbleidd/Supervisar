<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="CatalogoUsuarios.aspx.vb" Inherits="SEPRIS.CatalogoUsuarios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function CambiaMayuscula(campo) {
        campo.value = campo.value.toUpperCase();
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Catálogo de Usuarios</label>
                </div>
                <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                    <asp:Button ID="btnExportar" runat="server" Text="Exportar a Excel" />
                    &nbsp;
                </div>
                <br />
                <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
                <br />
                <cc1:CustomGridView ID="gvConsulta" runat="server" DataKeyNames="T_ID_USUARIO" Width="100%"
                    AllowSorting="true">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                            <HeaderStyle Width="15px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Usuario" DataField="T_ID_USUARIO" SortExpression="T_ID_USUARIO" >
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Nombre(s)" DataField="T_DSC_NOMBRE" SortExpression="T_DSC_NOMBRE" />
                        <asp:BoundField HeaderText="Apellido Paterno" DataField="T_DSC_APELLIDO" SortExpression="T_DSC_APELLIDO" />
                        <asp:BoundField HeaderText="Apellido Materno" DataField="T_DSC_APELLIDO_AUX" SortExpression="T_DSC_APELLIDO_AUX" />
                        <asp:BoundField HeaderText="Teléfono" DataField="T_DSC_TELEFONO" SortExpression="T_DSC_TELEFONO" />
                        <asp:BoundField HeaderText="E-mail" DataField="T_DSC_MAIL" SortExpression="T_DSC_MAIL" />
                        <asp:BoundField HeaderText="Área" DataField="T_DSC_AREA" SortExpression="T_DSC_AREA" />
                       <asp:BoundField HeaderText="Perfil" DataField="T_DSC_PERFIL" SortExpression="T_DSC_PERFIL" />
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
                    <asp:Label ID="lblTituloRegistro" runat="server" CssClass="TitulosWebProyectos" Text="Alta de Usuario" EnableTheming="false"></asp:Label>
                </div>
                <asp:Panel ID="pnlControles" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Nombre(s)*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="txt_gral" Width="300px"></asp:TextBox><asp:CustomValidator
                                    ID="csvNombre" ControlToValidate="txtNombre" ValidationGroup="Forma" ValidateEmptyText="true"
                                    runat="server" EnableClientScript="false" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Apellido Paterno*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtApellidoPaterno" runat="server" CssClass="txt_gral" Width="300px"
                                    ValidationGroup="Forma"></asp:TextBox><asp:CustomValidator ID="csvApellidoPaterno"
                                        ControlToValidate="txtApellidoPaterno" ValidationGroup="Forma" ValidateEmptyText="true"
                                        runat="server" EnableClientScript="false" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Apellido Materno:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtApellidoMaterno" runat="server" CssClass="txt_gral" Width="300px"
                                    ValidationGroup="Forma"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Teléfono*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="txt_gral" Width="300px" ValidationGroup="Forma"></asp:TextBox><asp:CustomValidator
                                    ID="csvTelefono" ControlToValidate="txtTelefono" ValidationGroup="Forma" ValidateEmptyText="true"
                                    runat="server" EnableClientScript="false" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    E-mail*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="txt_gral" Width="300px" ValidationGroup="Forma"></asp:TextBox><asp:CustomValidator
                                    ID="csvEmail" ControlToValidate="txtEmail" ValidationGroup="Forma" ValidateEmptyText="true"
                                    runat="server" EnableClientScript="false" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Perfil*:</label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="txt_gral" Width="100%"
                                    ValidationGroup="Forma">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CustomValidator ID="csvPefil" ControlToValidate="ddlPerfil" ValidationGroup="Forma"
                                    ValidateEmptyText="true" runat="server" EnableClientScript="false" Display="Dynamic"
                                    ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Usuario*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="txt_gral" Width="300px" ValidationGroup="Forma"></asp:TextBox><asp:CustomValidator
                                    ID="csvUsuario" ControlToValidate="txtUsuario" ValidationGroup="Forma" ValidateEmptyText="true"
                                    runat="server" EnableClientScript="false" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr id="trActualizarContrasena" runat="server" visible="false">
                            <td align="right">
                                <label class="txt_gral">
                                </label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkActualizarContrasena" runat="server" Text="Restablecer contraseña"
                                    CssClass="txt_gral" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlControlesUsuarioInterno" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Nombre(s)*:</label>
                            </td>
                            <td colspan="2" align="left">
                                <asp:DropDownList ID="ddlNombre" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                    Width="100%">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtNombreMod" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:CustomValidator ID="csvNombreInterno" ControlToValidate="ddlNombre" ValidationGroup="Forma"
                                    ValidateEmptyText="true" runat="server" EnableClientScript="false" Display="Dynamic"
                                    ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Usuario:</label>
                            </td>
                            <td colspan="2" align="left">
                                <asp:TextBox ID="txtUsuarioInterno" runat="server" CssClass="txt_solo_lectura" Width="300px"
                                    Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <!--
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                RFC:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtRFC" runat="server" Width="300px" MaxLength="13" onKeyup="javascript:CambiaMayuscula(this)"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:CustomValidator runat="server" ID="cvRFC" ControlToValidate="txtRFC" ValidationGroup="Forma"
                                ValidateEmptyText="true" EnableClientScript="false" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                        -->
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Perfil*:</label>
                            </td>
                            <td colspan="2" align="left">
                                <asp:DropDownList ID="ddlPerfilInterno" runat="server" CssClass="txt_gral" Width="100%">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CustomValidator ID="csvPerfilInterno" ControlToValidate="ddlPerfilInterno" ValidationGroup="Forma"
                                    ValidateEmptyText="true" runat="server" EnableClientScript="false" Display="Dynamic"
                                    ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                          <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Área*:</label>
                            </td>
                            <td colspan="2" align="left">
                                <asp:DropDownList ID="ddlArea" runat="server" CssClass="txt_gral" Width="100%">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CustomValidator ID="csvArea" ControlToValidate="ddlArea" ValidationGroup="Forma"
                                    ValidateEmptyText="true" runat="server" EnableClientScript="false" Display="Dynamic"
                                    ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Fecha de Alta*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtFechaIngreso" class="txt_gral" runat="server" Width="135px" MaxLength="10"></asp:TextBox>
                                <asp:CalendarExtender ID="txtFechaIngreso_CalendarExtender" runat="server" TargetControlID="txtFechaIngreso"
                                    Enabled="True" PopupButtonID="imgFec3" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>                                                            
                                
                                    <asp:CustomValidator ID="csvFechaIngreso" runat="server" 
                                        ControlToValidate="txtFechaIngreso" Display="Dynamic" 
                                        EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true" 
                                        ValidationGroup="Forma">*</asp:CustomValidator>
                                    <asp:Image ID="imgFec3" runat="server" src="/Controles/filtro/imagenes/Calendar.gif"
                                        Style="cursor:inherit" ImageUrl="~/Controles/filtro/imagenes/Calendar.GIF" 
                                        EnableTheming="True"/>                                
                            </td>
                            <tr>
                                <td align="right">
                                    &nbsp;</td>
                                <td align="left">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right">
                                    &nbsp;</td>
                                <td align="left">
                                    &nbsp;</td>
                            </tr>
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
                    MensajeDosBotonesDosAccionesLoad();
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
            <asp:PostBackTrigger ControlID="btnExportar" />
            <asp:PostBackTrigger ControlID="btnRegresar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
