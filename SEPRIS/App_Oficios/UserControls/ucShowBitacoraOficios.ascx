<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucShowBitacoraOficios.ascx.vb"
    Inherits="SICOD.ucShowBitacoraOficios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<br />
<asp:LinkButton ID="lnkShowC" runat="server" Text="" Style="display: none"></asp:LinkButton>
<asp:Panel ID="pnlShowBitacora" runat="server" Style="background-color: White" Width="90%">
    <br />
    <center class="TitulosWebProyectos">
        Historial
        <asp:Label runat="server" ID="lblTitulo" Text="Oficio Número " CssClass="TitulosWebProyectos"></asp:Label>
        <br />
    </center>
    <br />
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width: 2px">
            </td>
            <td align="left" style="width: 420px">
                <asp:Button ID="BtnExportarInferior" onmouseover="style.backgroundColor='#A9A9A9'"
                    onmouseout="style.backgroundColor='#696969'" runat="server" Style="height: 22px;
                    width: 130px" Text="Exportar Grid a Excel" CssClass="botones" Visible="true" />
            </td>
            <td>
            </td>
            <td style="width: 2px">
            </td>
        </tr>
        <tr>
            <td style="width: 2px">
            </td>
            <td colspan="2">
                <asp:Panel ID="pnlGrid" runat="server" Width="100%" Height="250px" ScrollBars="Auto">
                    <asp:GridView ID="grvHistorial" runat="server" ForeColor="#555555" Font-Size="7.5pt"
                        CellPadding="1" BackColor="#EEEEEE" AutoGenerateColumns="False" Font-Names="Arial"
                        HorizontalAlign="Center" Font-Name="Arial" PageSize="15" BorderColor="White"
                        Width="95%" AllowSorting="false">
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="NotSet" CssClass="HeaderBandeja"
                            ForeColor="Black" Font-Size="7.5pt"></HeaderStyle>
                        <RowStyle Wrap="true" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField HeaderText="Usuario Registro" DataField="USUARIO_SISTEMA_NOM" SortExpression="USUARIO_SISTEMA_NOM"
                                HeaderStyle-ForeColor="White"></asp:BoundField>
                            <asp:BoundField HeaderText="Fecha Alta" DataField="" SortExpression="" HeaderStyle-ForeColor="White">
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Tipo de Movimiento" DataField="MOVIMIENTO" SortExpression="MOVIMIENTO"
                                HeaderStyle-ForeColor="White"></asp:BoundField>
                            <asp:BoundField HeaderText="Descripción Movimiento" DataField="DESCRIPCION" SortExpression="DESCRIPCION"
                                HeaderStyle-ForeColor="White"></asp:BoundField>
                            <asp:BoundField HeaderText="Usuario Origen" DataField="USUARIO_ORIGEN_NOM" SortExpression="USUARIO_ORIGEN_NOM"
                                HeaderStyle-ForeColor="White"></asp:BoundField>
                            <asp:BoundField HeaderText="Usuario Destino" DataField="USUARIO_DESTINO_NOM" SortExpression="USUARIO_DESTINO_NOM"
                                HeaderStyle-ForeColor="White"></asp:BoundField>
                            <asp:BoundField HeaderText="Fecha Movimiento" DataField="FECH_MOVIMIENTO" SortExpression="FECH_MOVIMIENTO"
                                HeaderStyle-ForeColor="White"></asp:BoundField>
                            <asp:BoundField HeaderText="Fecha Vencimiento" DataField="" SortExpression="" HeaderStyle-ForeColor="White">
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel Height="100px" ID="pnlImagenNoExisten" runat="server" Style="display: none;">
                    <br />
                    <br />
                    <asp:Image ID="Image18" runat="server" ImageUrl="../imagenes/No Existen.gif"></asp:Image>
                </asp:Panel>
                <asp:Panel Height="100px" ID="pnlErrores" runat="server" Style="display: none">
                    <center>
                        <asp:Label ID="lblErrores" runat="server" CssClass="txt_gral txt_gral_o_gris" ForeColor="Red"></asp:Label>
                    </center>
                </asp:Panel>
            </td>
            <td style="width: 2px">
            </td>
        </tr>
        <tr>
            <td style="width: 2px">
            </td>
            <td align="left" style="width: 423px">
            </td>
            <td align="right" style="width: 423px">
                <asp:Button ID="btnCerrar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                    runat="server" Style="height: 22px; width: 130px; display: block" Text="Cerrar"
                    CssClass="botones" />
                &nbsp;
                <asp:UpdatePanel ID="ElBoton" runat="server">
                    <ContentTemplate>
                        <button id="btnCerrarAfuera" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                            onmouseout="style.backgroundColor='#696969'" style="height: 22px; width: 130px;
                            display: none" type="button" class="botones" onclick="Cierrame();">
                            Cerrar
                        </button>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td style="width: 2px">
            </td>
        </tr>
    </table>
    <br />
    <asp:HiddenField ID="hdnSession" runat="server" Value="" />
</asp:Panel>
<asp:ModalPopupExtender ID="mpeShowComentario" runat="server" PopupControlID="pnlShowBitacora"
    TargetControlID="lnkShowC" BackgroundCssClass="FondoAplicacion" CancelControlID="btnCerrar">
</asp:ModalPopupExtender>
