<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="CatalogoNivelesServicio.aspx.vb" Inherits="SEPRIS.CatalogoServicios" %>

<%@ Register Src="../Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/jscript">
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Catálogo de Niveles de Servicio</label>
                </div>
                <div style="text-align: left; width: 120%; padding-bottom: 5px;">
                    <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                </div>
                <div style="text-align:left; background-color:#426939;width: 120%;">
                    <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="800px" />
                </div>
                <cc1:CustomGridView ID="gvConsulta" runat="server" DataKeyNames="N_ID_NIVELES_SERVICIO" Width="120%"
                    AllowSorting="true" AutoGenerateColumns="false" AllowPaging="true" PageSize='<%# ObtenerPaginacion() %>'> 
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                            <HeaderStyle Width="15px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ID" DataField="N_ID_NIVELES_SERVICIO" SortExpression="N_ID_NIVELES_SERVICIO" >
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Tipo de servicio" DataField="T_DSC_TIPO_SERVICIO" SortExpression="N_ID_TIPO_SERVICIO" >
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Servicio" DataField="T_DSC_SERVICIO" SortExpression="N_ID_SERVICIO" >
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Aplicativo" DataField="T_DSC_APLICATIVO" SortExpression="N_ID_APLICATIVO" />
                        <asp:BoundField HeaderText="Área" DataField="T_DSC_AREA" SortExpression="N_ID_AREA" />
                        <asp:BoundField HeaderText="Flujo" DataField="T_DSC_FLUJO" SortExpression="N_ID_FLUJO" />
                        <asp:BoundField HeaderText="Ingeniero Responsable" DataField="NOMBRE_RESPONSABLE" SortExpression="NOMBRE_RESPONSABLE" />
                        <asp:BoundField HeaderText="Ingeniero Backup" DataField="NOMBRE_BACKUP" SortExpression="NOMBRE_BACKUP" />
                        <asp:BoundField HeaderText="Descripción" DataField="T_DSC_NIVELES_SERVICIO" SortExpression="T_DSC_NIVELES_SERVICIO" ItemStyle-CssClass="Wrap" />
                        <asp:BoundField HeaderText="Tiempo de Ejecución" DataField="N_NUM_TIEMPO_EJECUCION" SortExpression="N_NUM_TIEMPO_EJECUCION" >
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
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
                <div id="pnlNoExiste" runat="server" align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Image ID="Image1" runat="server" AlternateText="No existen registros para la consulta"
                        ImageAlign="Middle" ImageUrl="../Imagenes/no EXISTEN.gif" />
                </div>
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
                    <asp:Label ID="lblTituloRegistro" runat="server" CssClass="TitulosWebProyectos" Text="Alta de Nivel de Servicio" EnableTheming="false"></asp:Label>
                </div>
                <asp:Panel ID="pnlControles" runat="server">
                    <table>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Clave:</label>
                            </td>
                            <td align="left" width="300px">
                                <asp:TextBox ID="txtClave" runat="server" CssClass="txt_solo_lectura" Enabled="false"
                                    Width="100px"></asp:TextBox>
                            </td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Tipo de Servicio*:</label>
                            </td>
                            <td >
                                <asp:DropDownList ID="ddlTipoServicio" runat="server" Width="100%" CssClass="txt_gral" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvTipoServicio" runat="server" 
                                    ControlToValidate="ddlTipoServicio" Display="Dynamic" 
                                    EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true" 
                                    ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Servicio*:</label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlServicio" runat="server" Width="100%" CssClass="txt_gral" ></asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvServicio" runat="server" 
                                    ControlToValidate="ddlServicio" Display="Dynamic" EnableClientScript="false" 
                                    ForeColor="Red" ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Aplicativo*:</label>
                            </td>
                            <td >
                                <asp:DropDownList ID="ddlAplicativo" runat="server" Width="100%" CssClass="txt_gral" AutoPostBack="true" 
                                onfocus="obtenerValor(this.value);">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hfValorSeleccionado" runat="server" Value="-1" ClientIDMode="Static"/> 
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvAplicativo" runat="server" 
                                    ControlToValidate="ddlAplicativo" Display="Dynamic" EnableClientScript="false" 
                                    ForeColor="Red" ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Área*:</label>
                            </td>
                            <td >
                                <asp:DropDownList ID="ddlArea" runat="server" Width="100%" CssClass="txt_gral"></asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvArea" runat="server" ControlToValidate="ddlArea" 
                                    Display="Dynamic" EnableClientScript="false" ForeColor="Red" 
                                    ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Flujo*:</label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFlujo" runat="server" Width="100%" CssClass="txt_gral" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvFlujo" runat="server" ControlToValidate="ddlFlujo" 
                                    Display="Dynamic" EnableClientScript="false" ForeColor="Red" 
                                    ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Ingeniero Responsable*:</label>
                            </td>
                            <td >
                                <asp:DropDownList ID="ddlResponsable" runat="server" Width="100%" CssClass="txt_gral"></asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvResponsable" runat="server" 
                                    ControlToValidate="ddlResponsable" Display="Dynamic" EnableClientScript="false" 
                                    ForeColor="Red" ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Ingeniero Backup*:</label>
                            </td>
                            <td >
                                <asp:DropDownList ID="ddlBackup" runat="server" Width="100%" CssClass="txt_gral"  ></asp:DropDownList>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvBackup" runat="server" ControlToValidate="ddlBackup" 
                                    Display="Dynamic" EnableClientScript="false" ForeColor="Red" 
                                    ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Descripción*:</label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="txt_gral" Width="300px"
                                    ValidationGroup="Forma" MaxLength="100"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvDescripcion" runat="server" 
                                    ControlToValidate="txtDescripcion" Display="Dynamic" EnableClientScript="false" 
                                    ForeColor="Red" ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Tiempo de Ejecución (Hrs):</label>
                            </td>
                            <td >
                                <asp:TextBox ID="txtTiempoEjecucion" runat="server" CssClass="txt_gral" Width="300px"
                                    ValidationGroup="Forma" MaxLength="100"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:CustomValidator ID="cvTiempoEjecucion" runat="server" 
                                    ControlToValidate="txtTiempoEjecucion" Display="Dynamic" 
                                    EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true" 
                                    ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
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
            <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />


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

                function obtenerValor(valor) {
                    document.getElementById("hfValorSeleccionado").value = valor;
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
            <asp:PostBackTrigger ControlID="btnModificar" />
            <asp:PostBackTrigger ControlID="btnAgregar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
