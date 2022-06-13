<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PersonalizarColumasOfi.ascx.vb" Inherits="SEPRIS.PersonalizarColumasOfi" %>
<asp:UpdatePanel ID="updControles" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <table align="center" class="forma panelVentanaEmergenteBackground"
            style="width: 650px;padding:5px;" border="0">
            <tr>
                <td colspan="3" align="center">
                    <asp:Label ID="lblTitulo" runat="server" Text="Personalizar Columnas" CssClass="titulo_seccioninterior titulo_popup"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblTituloSinAsignar" runat="server" Text="Ocultar" CssClass="txt_gral txt_gral_o_gris"></asp:Label>
                </td>
                <td>
                </td>
                <td align="center">
                    <asp:Label ID="lblAsignados" runat="server" Text="Ver" CssClass="txt_gral txt_gral_o_gris"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:ListBox ID="lbxSinAsignar" runat="server" Height="150px" Width="249px"></asp:ListBox>
                </td>
                <td align="center">
                    <asp:Button ID="BtnPasarUnoVer" runat="server" Text=">" Width="80px" Height="20px"
                        class="botones" />
                    <br />
                    <br />
                    <asp:Button ID="BtnPasarTodosVer" runat="server" Text=">>" Width="80px" Height="20px"
                        class="botones" />
                    <br />
                    <br />
                    <asp:Button ID="BtnPasarUnoOcultar" runat="server" Text="<" Width="80px" Height="20px"
                        class="botones" />
                    <br />
                    <br />
                    <asp:Button ID="BtnPasarTodosOcultar" runat="server" Text="<<" Width="80px" Height="20px"
                        class="botones" />
                </td>
                <td align="center">
                    <asp:ListBox ID="lbxAsignado" runat="server" Height="150px" Width="249px"></asp:ListBox>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="right">
                    <br />
                    <asp:Button ID="btnGuardar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                        runat="server" Width="67px" CssClass="botones" Text="Guardar" Height="20px">
                    </asp:Button>
                    &nbsp;
                    <asp:Button ID="btnCancelar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                        runat="server" Width="67px" CssClass="botones" Text="Cancelar" Height="20px">
                    </asp:Button>
                    &nbsp; &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:Label ID="lblAccines" runat="server" ForeColor="White"></asp:Label>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
