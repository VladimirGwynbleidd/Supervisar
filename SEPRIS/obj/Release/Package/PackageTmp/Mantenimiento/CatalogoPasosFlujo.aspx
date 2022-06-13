<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="CatalogoPasosFlujo.aspx.vb" Inherits="SEPRIS.CatalogoPasosFlujo" %>
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
                        Catálogo de Pasos de Flujos</label>
                </div>
                <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                    <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                    &nbsp;
                    </div>
                <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
                <br />
                <cc1:CustomGridView ID="gvConsulta" AllowSorting="true" runat="server" DataKeyNames="N_ID_PASO"
                    Width="100%" ColumnasCongeladas="0" DataBindEnPostBack="True" HabilitaScroll="False" 
                    HeigthScroll="0" ToolTipHabilitado="True" UnicoEnPantalla="true" WidthScroll="0">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="20px" />
                            <HeaderStyle Width="20px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ID" DataField="N_ID_PASO" SortExpression="N_ID_PASO" >
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Descripción Paso (Tooltip)" DataField="T_DSC_PASO_TOOLTIP" SortExpression="T_DSC_PASO_TOOLTIP" >
                            <ItemStyle CssClass="Wrap" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Estatus asociado al paso" DataField="T_DSC_ESTATUS_SERVICIO" SortExpression = "T_DSC_ESTATUS_SERVICIO" >
                             <ItemStyle CssClass="Wrap" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Imagen activa durante el flujo" SortExpression="B_FLAG_VISIBLE">
                            <ItemTemplate>
                                <asp:Label ID="lblImagenVisible" runat="server" Text='<%# ObtenerImagenVisible(DataBinder.Eval(Container.DataItem, "B_FLAG_VISIBLE"))  %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="120px" />
                            <HeaderStyle Width="120px" />
                        </asp:TemplateField>                        
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
                <%--<asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />--%>
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
                    <asp:Label ID="lblTituloRegistro" runat="server" CssClass="TitulosWebProyectos" Text="Alta Paso de Flujo" EnableTheming="false"></asp:Label>
                </div>
                <asp:Panel ID="pnlControles" runat="server">
                    <table>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    ID:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtID" runat="server" CssClass="txt_solo_lectura" Enabled="false"
                                    Width="100px"></asp:TextBox>
                            </td>
                            <td align="left">
                                &nbsp;</td>
                            <td align="left">
                                &nbsp;</td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Descripción Paso (Tooltip)*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtPaso" runat="server" CssClass="txt_gral" Width="300px"
                                    ValidationGroup="Forma" MaxLength="50"></asp:TextBox>
                            </td>
                            <td align="left" >
                                <asp:CustomValidator ID="cvPaso" runat="server" ControlToValidate="txtPaso" 
                                    Display="Dynamic" EnableClientScript="false" ForeColor="Red" 
                                    ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                            <td align="left" >
                                &nbsp;</td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" >
                                <label class="txt_gral">
                                    Estatus asociado al paso*:</label>                            
                            </td>
                            <td align="left" >
                                <asp:DropDownList ID="ddlEstatusServicio" runat="server" CssClass="txt_gral" Width="100%"></asp:DropDownList>
                            </td> 
                            <td align="left">
                                <asp:CustomValidator ID="cvEstatusServicio" runat="server" 
                                    ControlToValidate="ddlEstatusServicio" Display="Dynamic" 
                                    EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true" 
                                    ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                            <td align="left">
                                &nbsp;</td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right"  >
                                <label class="txt_gral">
                                        Color barra de avance estatus anterior*:</label>
                            </td>
                            <td align="left" >
                                <asp:TextBox ID="txtColorAnterior" runat="server" CssClass="txt_gral" Width="100px"
                                    ValidationGroup="Forma" MaxLength="6"></asp:TextBox>
                                <asp:ColorPickerExtender ID="cpeColorAnterior" runat="server" TargetControlID="txtColorAnterior" 
                                    PopupButtonID="ibtnColorAnterior" samplecontrolid="txtSampleAnterior"/>
                                <asp:CustomValidator ID="cvColorAnterior" runat="server" 
                                    ControlToValidate="txtColorAnterior" Display="Dynamic" 
                                    EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true" 
                                    ValidationGroup="Forma">*</asp:CustomValidator>
                                <asp:ImageButton ID="ibtnColorAnterior" runat="server" Height="16px" 
                                    imageurl="~/Imagenes/cp_button.png" Width="16px" />
                                <asp:TextBox ID="txtSampleAnterior" runat="server" Enabled="false" 
                                    Height="12px" Width="12px" />
                            </td>
                            <td align="left" valign="bottom">
                                &nbsp;</td>
                            <td align="left" >
                                &nbsp;</td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" >
                                <label class="txt_gral">
                                        Color barra de avance estatus actual*:</label>
                            </td>
                            <td align="left" >
                                <asp:TextBox ID="txtColorActual" runat="server" CssClass="txt_gral" Width="100px" 
                                    ValidationGroup="Forma" MaxLength="6"></asp:TextBox>
                                <asp:ColorPickerExtender ID="cpeColorActual" runat="server" TargetControlID="txtColorActual" 
                                    PopupButtonID="ibtnColorActual" samplecontrolid="txtSampleActual"/>
                                <asp:CustomValidator ID="cvColorActual" runat="server" 
                                    ControlToValidate="txtColorActual" Display="Dynamic" EnableClientScript="false" 
                                    ForeColor="Red" ValidateEmptyText="true" ValidationGroup="Forma">*</asp:CustomValidator>
                                <asp:ImageButton ID="ibtnColorActual" runat="server" Height="16px" 
                                    imageurl="~/Imagenes/cp_button.png" Width="16px" />
                                <asp:TextBox ID="txtSampleActual" runat="server" Enabled="false" Height="12px" 
                                    Width="12px" />
                            </td>
                            <td align="left" valign="bottom">
                                &nbsp;</td>
                            <td align="left" >
                                &nbsp;</td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" >
                                <label class="txt_gral">
                                        Color barra de avance estatus posterior*:</label>
                            </td>
                            <td align="left" >
                                <asp:TextBox ID="txtColorPosterior" runat="server" CssClass="txt_gral" Width="100px" 
                                    ValidationGroup="Forma" MaxLength="6"></asp:TextBox>
                                <asp:ColorPickerExtender ID="cpeColorPosterior" runat="server" TargetControlID="txtColorPosterior" 
                                    PopupButtonID="ibtnColorPosterior" samplecontrolid="txtSamplePosterior"/>
                                <asp:CustomValidator ID="cvColorPosterior" runat="server" 
                                    ControlToValidate="txtColorPosterior" Display="Dynamic" 
                                    EnableClientScript="false" ForeColor="Red" ValidateEmptyText="true" 
                                    ValidationGroup="Forma">*</asp:CustomValidator>
                                <asp:ImageButton ID="ibtnColorPosterior" runat="server" Height="16px" 
                                    imageurl="~/Imagenes/cp_button.png" Width="16px" />
                                <asp:TextBox ID="txtSamplePosterior" runat="server" Enabled="false" 
                                    Height="12px" Width="12px" />
                            </td>
                            <td align="left" valign="bottom">
                                &nbsp;</td>
                            <td align="left" >
                                &nbsp;</td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        
                        <tr>
                            <td align="right" >
                                <label class="txt_gral">
                                    Imagen activa durante el flujo:</label>
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkImagenActiva" runat="server" class="txt_gral" />
                            </td>
                            <td align="left">
                                &nbsp;</td>
                            <td align="left">
                                &nbsp;</td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        
                        <tr id="trImagenActual" runat="server" visible="false">
                            <td align="right" >
                                <label class="txt_gral">
                                    Imagen:</label>
                            </td>
                            <td align="left" >
                                <asp:Image ID="imgActual" runat="server" Width="22px" Height="22px" />
                            </td>
                            <td align="left" >
                                &nbsp;</td>
                            <td align="left" >
                                &nbsp;</td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr id="trImagenCatalogo" runat="server">
                            <td align="right" valign="top" >
                                <label class="txt_gral">
                                    Imagen*:</label>
                            </td>
                            <td align="left">
                                <cc1:CustomGridView ID="gvImagen" runat="server" DataBindEnPostBack="true" SkinID="SeleccionSimpleCliente" DataKeyNames="N_ID_IMAGEN"
                                     ToolTipHabilitado="false" Width="300px" UnicoEnPantalla="false" HiddenFieldSeleccionSimple = "hfSelectedValueG1">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkElemento" runat="server" AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Clave" DataField="N_ID_IMAGEN" SortExpression="N_ID_IMAGEN" />
                                        <asp:BoundField HeaderText="Descripción" DataField="T_DSC_IMAGEN" SortExpression="T_DSC_IMAGEN" />
                                        <asp:TemplateField HeaderText="Imagen">
                                            <ItemTemplate>
                                                <asp:Image ID="imagen" runat="server" Width="22px" Height="22px" ImageUrl='<%# ObtenerImagen(DataBinder.Eval(Container.DataItem, "T_DSC_RUTA_IMAGEN"))  %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GridViewEncabezado" />
                                    <RowStyle CssClass="GridViewContenido" />
                                    <PagerStyle CssClass="GridviewScrollPager" />
                                    <AlternatingRowStyle CssClass="GridViewContenidoAlternate" />
                                </cc1:CustomGridView>                                                              
                            </td>
                            <td align="left" valign="top" >
                                <asp:CustomValidator ID="cvImagen" runat="server" EnableClientScript="false" Display="Dynamic" 
                                    ValidationGroup="Forma" ForeColor="Red">*</asp:CustomValidator>  
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr id="trImagenInactiva" runat="server" visible="false">
                            <td align="right" >
                                <label class="txt_gral">
                                    Imagen Inactiva:</label>
                            </td>
                            <td align="left" >
                                <asp:Image ID="imgInactiva" runat="server" Width="22px" Height="22px" />
                            </td>
                            <td align="left" >
                                &nbsp;</td>
                            <td align="left" >
                                &nbsp;</td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr id="trImagenInactivaCatalogo" runat="server">
                            <td align="right" valign="top" >
                                <label class="txt_gral">
                                    Imagen Inactiva*:</label>
                            </td>
                            <td align="left">
                                <cc1:CustomGridView ID="gvImagenInactiva" runat="server" SkinID="SeleccionSimpleCliente" DataBindEnPostBack="true" DataKeyNames="N_ID_IMAGEN"
                                     ToolTipHabilitado="false" Width="300px" UnicoEnPantalla="false" HiddenFieldSeleccionSimple="hfSelectedValueG2">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkElemento" runat="server" AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Clave" DataField="N_ID_IMAGEN" SortExpression="N_ID_IMAGEN" />
                                        <asp:BoundField HeaderText="Descripción" DataField="T_DSC_IMAGEN" SortExpression="T_DSC_IMAGEN" />
                                        <asp:TemplateField HeaderText="Imagen">
                                            <ItemTemplate>
                                                <asp:Image ID="imagen" runat="server" Width="22px" Height="22px" ImageUrl='<%# ObtenerImagen(DataBinder.Eval(Container.DataItem, "T_DSC_RUTA_IMAGEN"))  %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="GridViewEncabezado" />
                                    <RowStyle CssClass="GridViewContenido" />
                                    <PagerStyle CssClass="GridviewScrollPager" />
                                    <AlternatingRowStyle CssClass="GridViewContenidoAlternate" />
                                </cc1:CustomGridView>                                                              
                            </td>
                            <td align="left" valign="top" >
                                <asp:CustomValidator ID="cvImagenInactiva" runat="server" EnableClientScript="false" Display="Dynamic" 
                                    ValidationGroup="Forma" ForeColor="Red">*</asp:CustomValidator>  
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>

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
            
            <asp:HiddenField ID="hfSelectedValueG1" runat="server" ClientIDMode="Static" Value="-1" />
            <asp:HiddenField ID="hfSelectedValueG2" runat="server" ClientIDMode="Static" Value="-1" />
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
            <asp:PostBackTrigger ControlID="btnAgregar" />
            <asp:PostBackTrigger ControlID="btnExportaExcel" />
            <asp:PostBackTrigger ControlID="btnModificar" />
            <asp:PostBackTrigger ControlID="btnRegresar" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>
