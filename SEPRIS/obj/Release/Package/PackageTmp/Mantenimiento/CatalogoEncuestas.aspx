<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="CatalogoEncuestas.aspx.vb" Inherits="SEPRIS.CatalogoEncuestas" %>

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
                        Catálogo de Encuestas</label>
                </div>
                <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                    <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                    &nbsp;
                </div>
                <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
                <br />
                <cc1:CustomGridView ID="gvConsulta" AllowSorting="True" runat="server" DataKeyNames="N_ID_ENCUESTA"
                    Width="100%" ColumnasCongeladas="0" DataBindEnPostBack="True" HabilitaScroll="False"
                    HabilitaSeleccion="No" HeigthScroll="0" Text="[gvConsulta]" ToolTip="" ToolTipHabilitado="True"
                    UnicoEnPantalla="True" WidthScroll="0" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                            <HeaderStyle Width="15px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ID" DataField="N_ID_ENCUESTA" SortExpression="N_ID_ENCUESTA">
                            <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Descripción" DataField="T_DSC_ENCUESTA" SortExpression="T_DSC_ENCUESTA" />
                        <asp:TemplateField HeaderText="Estatus" SortExpression="B_FLAG_VIG">
                            <ItemTemplate>
                                <asp:Image ID="imagenEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "B_FLAG_VIG"))  %>' />
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                        </asp:TemplateField>
                    </Columns>
                </cc1:CustomGridView>
                <img id="Noexisten" runat="server" src="../Imagenes/No%20Existen.gif" visible="false"
                    alt="No existen Registos para la Consulta" />
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
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnRegresarBandeja" runat="server" Text="Regresar" />
            </asp:Panel>
            <asp:Panel ID="pnlRegistro" runat="server" Visible="false">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Label ID="lblTituloRegistro" runat="server" CssClass="TitulosWebProyectos" Text="Alta de Encuesta" EnableTheming="false"></asp:Label>
                </div>
                <asp:Panel ID="pnlControles" runat="server">
                    <table>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    ID:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtId" runat="server" CssClass="txt_solo_lectura" Enabled="false"
                                    Width="100px">1</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label class="txt_gral">
                                    Descripción*:</label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtEncuesta" runat="server" CssClass="txt_gral" Width="300px" ValidationGroup="Forma"
                                    MaxLength="50"></asp:TextBox>
                                <asp:CustomValidator ID="cvEncuesta" ControlToValidate="txtEncuesta" ValidationGroup="Forma"
                                    ValidateEmptyText="true" runat="server" EnableClientScript="false" Display="Dynamic"
                                    ForeColor="Red">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            <br /><br />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblAspectosEvaluar" runat="server" Text="Aspectos a Evaluar" CssClass="txt_gral"></asp:Label>
                            </td>
                            <td>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblOpcionesEvaluacion" runat="server" Text="Opciones de Evaluación" CssClass="txt_gral"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top;">
                                <asp:GridView  ID="gvConsultaAspectos" runat="server" DataKeyNames="N_ID_ENCUESTA_ASPECTO_EVALUAR"
                                    Width="300px" ColumnasCongeladas="0" DataBindEnPostBack="True" HabilitaScroll="False"
                                    HabilitaSeleccion="No" HeigthScroll="0" Text="[gvConsultaAspectos]" ToolTip=""
                                    ToolTipHabilitado="True" UnicoEnPantalla="True" WidthScroll="0" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Orden">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txbOrden" Width="25px" runat="server" Text='<%# Eval("N_ID_ORDEN") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Descripción" DataField="T_DSC_ASPECTO_EVALUAR" SortExpression="T_DSC_ASPECTO_EVALUAR">
                                            <ItemStyle Width="70%" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <img id="NoExistenAspectos" runat="server" src="../Imagenes/No%20Existen.gif" visible="false"
                                alt="No existen Registos para la Consulta" />
                            </td>
                            <td style="width:100px;" align="left" valign="top" >
                                <asp:CustomValidator ID="cvAspectos" runat="server" EnableClientScript="false" Display="Dynamic" 
                                    ValidationGroup="Forma" ForeColor="Red">*</asp:CustomValidator>  
                            </td>
                             <td style="vertical-align:top;">
                                <asp:GridView ID="gvConsultaOpciones" runat="server" DataKeyNames="N_ID_ENCUESTA_OPCION"
                                    Width="300px" ColumnasCongeladas="0" DataBindEnPostBack="True" HabilitaScroll="False"
                                    HabilitaSeleccion="No" HeigthScroll="0" Text="[gvConsultaOpciones]" ToolTip=""
                                    ToolTipHabilitado="True" UnicoEnPantalla="True" WidthScroll="0" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Orden">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txbOrden" Width="25px" runat="server" Text='<%# Eval("N_ID_ORDEN") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Descripción" DataField="T_DSC_OPCION" SortExpression="T_DSC_OPCION">
                                            <ItemStyle Width="70%" />
                                        </asp:BoundField>
                                      </Columns>
                                </asp:GridView >
                                 <img id="NoExistenOpciones" runat="server" src="../Imagenes/No%20Existen.gif" visible="false"
                                alt="No existen Registos para la Consulta" />
                            </td>
                            <td align="left" valign="top" >
                                <asp:CustomValidator ID="cvOpciones" runat="server" EnableClientScript="false" Display="Dynamic" 
                                    ValidationGroup="Forma" ForeColor="Red">*</asp:CustomValidator>  
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
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"  />
                                 &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlRegresar" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td colspan="2">
                                 <asp:Button ID="btnRegresar2" runat="server" Text="Regresar" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <div id="divMensajeUnBotonNoAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="../Imagenes/Errores/Error1.png" />
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
                        <td style="width: 50px; text-align: center; vertical-align: top">
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
                        <td style="width: 50px; text-align: center; vertical-align: top">
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
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px"
                                ImageUrl="~/Imagenes/Errores/Error1.png" />
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
