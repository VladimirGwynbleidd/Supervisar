<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Login.aspx.vb" Inherits="SEPRIS.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="scripts/animateText.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pntLogin" runat="server" DefaultButton="btnAceptar">
        <div style="text-align: center; width: 350px; height: 100px; margin-left: auto; margin-right: auto;">
            <script src="scripts/animateText.js" type="text/javascript"></script>
            <ul id="titulo_animado" class="titulo_animado">
                <li class="titulo_animado">SUPERVISAR</li>
            </ul>
            <script type="text/javascript">
                $("#titulo_animado").animateText([
                    {
                        offset: 0,
                        duration: 3000,
                        animation: "leftToRight"
                    }]
                );
            </script>
        </div>
        <div>
            <asp:Label ID="lblMensaje" ForeColor="Red" runat="server" Text="" CssClass="mensaje_login"></asp:Label>
        </div>
        <div>
            <table>
                <tr>
                    <td style="vertical-align: top">
                        <table class="tbl_login" cellpadding="0" cellspacing="0" border="1">
                            <tr>
                                <td align="center" class="TemaFondoOscuro tbl_login_encabezado">
                                    <asp:Label ID="lblAcceso" ForeColor="White" runat="server" Text="Acceso de Usuarios"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr id="trLblDomimio" runat="server">
                                            <td style="height: 17px; text-align: center; border: 0;">
                                                <asp:Label ID="lblDominio" runat="server" Text="Dominio:" CssClass="txt_gral"></asp:Label></td>
                                        </tr>
                                        <tr id="trTxtDominio" runat="server">
                                            <td style="height: 17px; text-align: center; border: 0;">
                                                <asp:TextBox ID="txtDominio" runat="server" CssClass="txt_solo_lectura" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 17px; text-align: center; border: 0;">
                                                <asp:Label ID="lblUsuario" runat="server" Text="Usuario:" CssClass="txt_gral"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 17px; text-align: center; border: 0;"><%--Text="ntorres"--%>
                                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="txt_gral" onkeypress="ReemplazaCEspeciales(this.id)" onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 17px; text-align: center; border: 0;">
                                                <asp:Label ID="lblContrasena" runat="server" Text="Contraseña:" CssClass="txt_gral"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 23px; text-align: center; border: 0;" valign="top"><%--Text="+UzLrYC7hF8wguftxi28Mg=="--%>
                                                <asp:TextBox TextMode="Password" ID="txtContrasena" runat="server" CssClass="txt_gral"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trRecuperar" runat="server">
                                            <td style="height: 23px; text-align: right; border: 0;" valign="top">
                                                <asp:LinkButton ID="lnkRecuperar" runat="server" PostBackUrl="Recuperar.aspx" CssClass="liga_recupera_pass" Text="Olvidé mi contraseña"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td style="height: 37px; text-align: center; border: 0;" valign="middle">
                                                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="botones" OnClientClick="Deshabilita(this);" />
                                                &nbsp;<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="botones" OnClientClick="Deshabilita(this);" /></td>
                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

    <script language="javascript" type="text/javascript">
        document.getElementById('<%= txtUsuario.ClientID %>').focus();
    </script>


    <div id="divMensajeUnBotonNoAccion" style="display: none" title="Información">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
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

    <script type="text/javascript">
        function ConfirmacionLogin() {
            ConfirmacionNoClose("divMensajeUnBotonNoAccion", "btnConfirmarDocsSI", 700, 400);
        }
    </script>

    <asp:Button runat="server" ID="btnConfirmarDocsSI" Style="display: none" ClientIDMode="Static" OnClick="btnConfirmarDocsSI_Click" />
</asp:Content>
