<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="CambioContrasenia.aspx.vb" Inherits="SEPRIS.CambioContrasenia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <br />
    <table class="tbl_login" cellpadding="0" cellspacing="0" border="1">
        <tr>
            <td class="TemaFondoMedio tbl_login_encabezado" align="center">
                <p>
                    <asp:Label ID="lblUsuario" runat="server" Text=""></asp:Label></p>
                <p>
                    Debe cambiar su contraseña</p>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="100%">
                    <tr>
                        <td class="txt_gral">
                            Contraseña:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPassword" TabIndex="1" runat="server" Width="168px" CssClass="txt_gral"
                                TextMode="Password" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="txt_gral">
                            Confirmar Contraseña:
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 23px" valign="top" align="center">
                                <asp:TextBox ID="txtConfirma" TabIndex="2" runat="server" Width="168px" CssClass="txt_gral"
                                    TextMode="Password" MaxLength="30"></asp:TextBox><br />
                                <asp:CompareValidator ID="CompareValidatorCambio" runat="server" ControlToCompare="txtConfirma"
                                    ControlToValidate="txtPassword" ErrorMessage="Las Contraseñas no coinciden" CssClass="txt_gral"
                                    Style="color: #FF0000"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 37px; text-align: center; border: 0;" valign="middle">
                            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="botones" />
                            &nbsp;<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="botones" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id="divMensajeUnBotonUnaAccion" style="display: none" clientidmode="Static" title="Contraseña No Válida">
        <table width="100%">
            <tr>
                <td style="width: 25%; text-align: center">
                    <asp:Image ID="imgUnBotonAccion" runat="server" Width="32px" Height="32px" />
                </td>
                <td style="width: 75%; text-align: left">
                    <asp:Label runat="server" ID="lblError" Text="" CssClass="txt_gral"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
    <script type="text/javascript" language="javascript" >

        document.getElementById('<%= txtPassword.ClientID %>').focus();

        $(function () {

            MensajeUnBotonUnaAccionLoad();


        });


        function MostrarMensajeAccion() {

            MensajeUnBotonUnaAccion();

        }


    </script>
</asp:Content>
