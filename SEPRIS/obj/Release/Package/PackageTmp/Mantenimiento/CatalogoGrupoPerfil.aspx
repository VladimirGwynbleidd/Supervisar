<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="CatalogoGrupoPerfil.aspx.vb" Inherits="SEPRIS.CatalogoGrupoPerfil" %>
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
                    <label class="TitulosWebProyectos" enabletheming="false">Catálogo Grupos de Perfil</label>
                </div>
                <br />
                <table align="center">
                    <tr>
                        <td class="txt_gral">
                            Perfil:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPerfil" runat="server" AutoPostBack="true"  CssClass="txt_gral" Width="200px">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="cvPerfil" ControlToValidate="ddlPerfil" ValidationGroup="Forma" ValidateEmptyText="true"
                                runat="server" EnableClientScript="false" Display="Dynamic" ForeColor="Red">*</asp:CustomValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="pnlGrupos" runat="server" style="display: none">
                    <table align="center">
                        <tr>
                            <td valign="top" style="width: 300px;">
                                <asp:Button ID="btnExportaExcelAsigandos" runat="server" Text="Exportar a Excel"  />
                                <asp:Label ID="lblAsignados" runat="server" Text="Grupos actualmente usados por el perfil" CssClass="txt_gral" ></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td valign="top" style="width: 300px;">
                                <asp:Button ID="btnExportaExcelDisponibles" runat="server" Text="Exportar a Excel"  />
                                <asp:Label ID="lblDisponibles" runat="server" Text="Grupos disponibles para agregar" CssClass="txt_gral"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="width: 300px;">                                                                
                                <cc1:CustomGridView ID="gvAsignados" runat="server" Width="300px" SkinID="SeleccionMultipleCliente"
                                    HabilitaScroll="true" HeigthScroll="400" WidthScroll="320" UnicoEnPantalla="false" ToolTipHabilitado="false" >
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkElemento" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="ID" DataField="Identificador" /> 
                                        <asp:BoundField HeaderText="Grupo" DataField="Descripcion" /> 
                                    </Columns>
                                </cc1:CustomGridView>
                                <asp:Image ID="imgAsignados" runat="server" ImageUrl="~/Imagenes/No Existen.gif" Visible="false" />
                            </td>
                            <td valign="middle">
                                <asp:Button ID="btnRemueve" runat="server" Text=">>"  />
                                <br /><br />
                                <asp:Button ID="btnAgrega" runat="server" Text="<<"  />
                            </td>
                            <td valign="top" style="width: 300px;">
                                <cc1:CustomGridView ID="gvDisponibles" runat="server" Width="300px" SkinID="SeleccionMultipleCliente"
                                    HabilitaScroll="true" HeigthScroll="400" WidthScroll="320" UnicoEnPantalla="false" ToolTipHabilitado="false" >
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkElemento" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="ID" DataField="Identificador" /> 
                                        <asp:BoundField HeaderText="Grupo" DataField="Descripcion" /> 
                                    </Columns>
                                </cc1:CustomGridView>
                                <asp:Image ID="imgDisponibles" runat="server" ImageUrl="~/Imagenes/No Existen.gif" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div align="center">
                    <asp:Image ID="imgDatos" runat="server" ImageUrl="~/Imagenes/No Existen.gif" Visible="false" />
                </div>

                <table align="center">
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnMostrarMensaje"  Text="Aceptar" Visible="false" />  

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
                            <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" /> 
                        </td>
                        <td>
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Visible="false" />
                        </td>
                    </tr>
                </table>

                <div id="divMensajeUnBotonNoAccion" style="display: none">
                    <table width="100%">
                        <tr>
                            <td style="width: 50px; text-align: center; vertical-align:top">
                                <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                            </td>
                            <td style="text-align: left">
                                <div class="MensajeModal-UI">
                                     <asp:Label ID="lblMensaje" runat="server" CssClass="MensajeModal-UI" EnableTheming="false"></asp:Label>
                                    <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" EnableTheming="false" />
                               </div>
                            </td>
                        </tr>
                    </table>
                </div>

            </asp:Panel>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
            <asp:PostBackTrigger ControlID="btnExportaExcelAsigandos" />
            <asp:PostBackTrigger ControlID="btnExportaExcelDisponibles" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
