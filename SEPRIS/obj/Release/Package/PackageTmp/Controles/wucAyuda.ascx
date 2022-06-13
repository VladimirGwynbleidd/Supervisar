<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="wucAyuda.ascx.vb" Inherits="SEPRIS.wucAyuda" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:ImageButton ID="imbAyuda" runat="server" ImageUrl="~/Imagenes/Question_mark.png" AlternateText="Ayuda" 
                 ToolTip="Clic para obtener ayuda" CssClass="transparencia" />

<div id="divMensajeComponenteAyudaControlPadre" title="Ayuda" style="display:none" >
    <p>
        <asp:UpdatePanel ID="upnlAyuda" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlEncabezadoAyuda" runat="server" Visible="true">
                    <table>
                        <tr>
                            <td>
                                <asp:ImageButton ID="imbInicioAyuda" runat="server" ImageUrl="~/Imagenes/Home.png" Height="32px" Width="32px" />
                            </td>
                            <td>
                                <asp:ImageButton ID="imbIndiceAyuda" runat="server" ImageUrl="~/Imagenes/book_search.png" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtBuscarAyuda" runat="server" CssClass="txt_gral" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnBuscarAyuda" runat="server" Text="Buscar" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblInicioAyuda" runat="server" Text="Inicio" CssClass="txt_gral"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblIndiceAyuda" runat="server" Text="Índice" CssClass="txt_gral"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblBuscarAyuda" runat="server" Text="Buscar en la Ayuda" CssClass="txt_gral" ></asp:Label>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </asp:Panel>
                <hr />
                <asp:Panel ID="pnlContenidoAyuda" runat="server" style="height:330px; overflow:auto;">
                    <asp:Panel ID="pnlInicioAyuda" runat="server">
                        <asp:Label ID="lblTituloInicioAyuda" runat="server" CssClass="TitulosWebProyectos" Text="Inicio de la ayuda"></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="lblContenidoInicioAyuda" runat="server" CssClass="txt_gral" Text="Cuerpo de ayuda"></asp:Label>
                    </asp:Panel>
                    <asp:Panel ID="pnlIndice" runat="server" Visible="false">
                        <asp:Label ID="lblIndice" runat="server" Text="Titulo Índice" CssClass="TitulosWebProyectos" ></asp:Label>
                        <br />
                        <asp:TreeView ID="trvIndice" runat="server" CssClass="txt_gral" >
                        </asp:TreeView>
                    </asp:Panel>
                    <asp:Panel ID="pnlInformacionAyuda" runat="server" Visible="false" >
                        <div align="right">
                            <asp:LinkButton ID="lnbMenu" runat="server" Text="Menu" CssClass="txt_gral" ></asp:LinkButton>
                            <asp:Label ID="lblMenu" runat="server" Text="&gt;" CssClass="txt_gral"></asp:Label>
                            <asp:LinkButton ID="lnbSubmenu" runat="server" Text="Submenu" CssClass="txt_gral" ></asp:LinkButton>
                            <asp:Label ID="lblSubmenu" runat="server" Text="&gt;" CssClass="txt_gral"></asp:Label>
                            <asp:LinkButton ID="lnbAyudaPadre" runat="server" Text="AyudaPadre" CssClass="txt_gral" ></asp:LinkButton>
                            <asp:Label ID="lblAyudaPadre" runat="server" Text="&gt;" CssClass="txt_gral"></asp:Label>
                            <asp:LinkButton ID="lnbAyuda" runat="server" Text="Ayuda" CssClass="txt_gral" ></asp:LinkButton>
                        </div>
                        <br />
                        <div>
                            <asp:Label ID="lblTituloAyuda" runat="server" Text="Titulo Contenido" CssClass="TitulosWebProyectos" ></asp:Label>
                            <br /><br />
                            <asp:Label ID="lblContenudoAyuda" runat="server" Text="Contenido" CssClass="txt_gral" style="text-align:justify"></asp:Label>
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="imbAyuda" />
            </Triggers>           
        </asp:UpdatePanel>
    
    </p>
    
</div>

<script type="text/javascript">
    $(function () {
        MensajeComponenteAyudaControlLoad();        
    });

    function DialogoAyuda() {
        MensajeComponenteAyudaControl();
    }
</script>

