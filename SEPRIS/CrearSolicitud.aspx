<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="CrearSolicitud.aspx.vb" Inherits="SEPRIS.CrearSolicitud" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controles/wucAyuda.ascx" TagName="wucAyuda" TagPrefix="wcu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <wcu:wucAyuda ID="Ayuda" runat="server" />
    <asp:UpdatePanel ID="udpSolicitudes" runat="server">
        <ContentTemplate>
        <asp:HiddenField runat="server" ID="hdfImagen" ClientIDMode="Static" value="0"/>
    <div style="visibility:hidden;">
        <asp:Button runat="server" ID="lnkImagen" ClientIDMode="Static"/>
    </div>
            
            <div align="center" style="padding: 20px 20px 15px 20px">
                <label class="TitulosWebProyectos">
                    Crear Solicitud</label>
            </div>
            <div>
            <br />
                <asp:Panel ID="pnlCarrusel" runat="server">
                </asp:Panel>
            <br />
           <%-- <asp:Button ID="btnCrear" runat="server" Text="Crear solicitud" />--%>
            </div>
            <div>
                <asp:GridView runat="server" ID="gvConsultaSolicitudes" DataKeyNames="N_ID_SOLICITUD_SERVICIO"
                    Width="800px">
                    <Columns>
                       <asp:TemplateField HeaderText="Solicitud Previa">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkView" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Historial" Text='<%#Eval("N_NUM_FOLIO_USUARIO") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Fecha de captura" DataField="F_FECH_REGISTRO_SOLICITUD"
                            SortExpression="F_FECH_REGISTRO_SOLICITUD">
                            <ItemStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Fecha de última modificación" DataField="F_FECH_ULTIMA_MODIFICACION"
                            SortExpression="F_FECH_ULTIMA_MODIFICACION">
                            <ItemStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="No. de servicios agregados" DataField="NO_SERVICIOS"
                            SortExpression="NO_SERVICIOS">
                            <ItemStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Funcionario que autoriza" DataField="T_ID_AUTORIZADOR"
                            SortExpression="T_ID_AUTORIZADOR">
                            <ItemStyle Width="40%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Extensión" DataField="N_NUM_EXTENSION" SortExpression="N_NUM_EXTENSION">
                            <ItemStyle Width="10%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Acción">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:ImageButton ID="imbEditar" runat="server" Height="20px" CausesValidation="False"
                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Modificar"
                                    ImageUrl="~/Imagenes/icono_lapiz.GIF" ToolTip="Editar" />
                                <asp:ImageButton ID="imbBorrar" runat="server" Height="20px" CausesValidation="False"
                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Borrar"
                                    ImageUrl="~/Imagenes/icono_corregir.jpg" ToolTip="Borrar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Image ID="imgNoDisp" runat="server" ImageUrl="~/Imagenes/No Existen.gif" Visible="false" />
            </div>
            <div id="divMensajeUnBotonNoAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="../Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                                <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="Forma" CssClass="txt_gral" />
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
        <div id="divMensaje800x400Historial" style="display: none" title="Comentarios">
        <table width="100%">
            <tr>
                <td style="text-align: center; vertical-align: top">
                    <div class="MensajeModal-UI">
                        <asp:Panel ID="pnlHistorial" runat="server">
                            <div id="Div1" style="text-align: center; vertical-align: top;">
                                <br />
                                <label class="TitulosWebProyectos">
                                    Comentarios asociados a la solicitud
                                </label>
                                <br />
                                <br />
                                <asp:GridView ID="gvHistCom" runat="server" Width="100%" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField HeaderText="Fecha" DataField="F_FECH_REGISTRO" ControlStyle-Width="10%" />
                                        <asp:BoundField HeaderText="Usuario" DataField="USUARIO" ControlStyle-Width="30%" />
                                        <asp:BoundField HeaderText="Acción" DataField="T_DSC_ACCION" ControlStyle-Width="30%" />
                                        <asp:BoundField HeaderText="Comentario" DataField="T_DSC_COMENTARIO" ControlStyle-Width="30%" />
                                    </Columns>
                                </asp:GridView>
                                <p style="text-align: center;">
                                    <asp:Image ID="imgHistComentarios" runat="server" ImageUrl="~/Imagenes/No Existen.gif"
                                        Visible="False" />
                                </p>
                            </div>
                        </asp:Panel>
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
                    Mensaje800x400HistorialLoad();
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
                };

                function MensajeConfirmacion() {
                    MensajeDosBotonesUnaAccion();
                };

                function MensajeHistorial() {
                    Mensaje800x400Historial();
                };

            </script>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSalirM2B2A" />
            <asp:PostBackTrigger ControlID="btnContinuarM2B2A" />
            <asp:PostBackTrigger ControlID="btnAceptarM1B1A" />
            <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
