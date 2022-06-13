<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="CatalogoSeguimientoFlujo.aspx.vb" Inherits="SEPRIS.CatalogoSeguimientoFlujo" %>
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
    <style type="text/css">
        .style2
        {
            width: 31px;
        }
        .style3
        {
            width: 8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">Catálogo Seguimiento de Flujo</label>
                </div>
                <br />
                <table align="center">                    
                    <tr>
                        <td>
                            <asp:Label ID="lblFlujo" runat="server" Text="Flujo:" CssClass="txt_gral"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFlujo" runat="server" AutoPostBack="true"  CssClass="txt_gral" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="pnlPasos" runat="server" style="display: none">
                    <table align="center">
                        <tr>
                            <td valign="top" style="width: 300px;">
                                <asp:Button ID="btnExportaExcelUsados" runat="server" Text="Exportar a Excel"  />
                                <asp:Label ID="lblUsados" runat="server" Text="Pasos agregados al flujo" CssClass="txt_gral"></asp:Label>
                            </td>
                            <td valign="top">
                                &nbsp;</td>
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td class="style3">
                                &nbsp;</td>
                            <td valign="top" style="width: 300px;">
                                <asp:Button ID="btnExportaExcelDisponibles" runat="server" Text="Exportar a Excel"  />
                                <asp:Label ID="lblDisponibles" runat="server" Text="Pasos disponibles para agregar" CssClass="txt_gral"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <cc1:CustomGridView ID="gvUsados" runat="server" Width="300px" SkinID="SeleccionMultipleCliente" 
                                    HabilitaScroll="true" HeigthScroll="400" WidthScroll="320" UnicoEnPantalla="false" ToolTipHabilitado="false" >
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkElemento" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="ID" DataField="Identificador" /> 
                                        <asp:BoundField HeaderText="Descripción paso" DataField="Descripcion" /> 
                                        <asp:TemplateField HeaderText="Orden">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOrden" Width="25px" runat="server" Text='<%# Eval("Orden") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>   
                                </cc1:CustomGridView>
                                <asp:Image ID="imgUsados" runat="server" ImageUrl="~/Imagenes/No Existen.gif" Visible="false" />
                            </td>
                            <td valign="top">
                                <asp:CustomValidator ID="cvOrden" runat="server" Display="Dynamic" 
                                    EnableClientScript="false" ForeColor="Red" ValidationGroup="Forma">*</asp:CustomValidator>
                            </td>
                            <td valign="middle" class="style2">
                                <asp:Button ID="btnRemueve" runat="server" Text=">>"  />
                                <br /><br />
                                <asp:Button ID="btnAgrega" runat="server" Text="<<"  />
                            </td>
                            <td class="style3" valign="middle">
                                &nbsp;</td>
                            <td valign="top">
                                <cc1:CustomGridView ID="gvDisponibles" runat="server" Width="300px"  SkinID="SeleccionMultipleCliente" 
                                    HabilitaScroll="true" HeigthScroll="400" WidthScroll="320" UnicoEnPantalla="false" ToolTipHabilitado="false" >
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkElemento" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="ID" DataField="Identificador" /> 
                                        <asp:BoundField HeaderText="Descripción paso" DataField="Descripcion" /> 
                                    </Columns>
                                </cc1:CustomGridView>
                                <asp:Image ID="imgDisponibles" runat="server" ImageUrl="~/Imagenes/No Existen.gif" Visible="false" />
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
                <table align="center" id="tblBotones" runat="server">
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" 
                                OnClientClick="Deshabilita(this);" Visible="False" />  


                            <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" /> 
                        </td>
                        <td>
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Visible="False"  />
                        </td>
                    </tr>
                </table>    
                
                <div id="divMensajeDosBotonesUnaAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align:top">
                            <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
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
                        <td style="width: 50px; text-align: center; vertical-align:top">
                            <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <asp:Label ID="lblMensaje" runat="server" CssClass="MensajeModal-UI" EnableTheming="false"></asp:Label>
                                <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
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
