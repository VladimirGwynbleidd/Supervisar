<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConsultaHistorial.aspx.vb"
    Inherits="SICOD.ConsultaHistorial" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="~/Styles.css" />
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <center>
        <asp:Label runat="server" ID="lblTitulo" Text="Historial Oficio Número " CssClass="TitulosWebProyectos"></asp:Label>
        <br />
    </center>
    <br />
    <center>
        <table width="95%">
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnlGrid" runat="server" Width="99%" Height="250px" 
                        ScrollBars="Auto">
                        <asp:GridView ID="grvHistorial" runat="server" ForeColor="#555555" Font-Size="7.5pt"
                            CellPadding="1" BackColor="#EEEEEE" AutoGenerateColumns="False" Font-Names="Arial"
                            HorizontalAlign="Center" Font-Name="Arial" PageSize="15" BorderColor="White"
                            Width="100%" AllowSorting="false">
                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="NotSet" CssClass="GridViewHeaderOficios"
                                ForeColor="White" Font-Size="7.5pt"></HeaderStyle>
                            <RowStyle Wrap="true" />
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField HeaderText="Usuario Registro" DataField="USUARIO_SISTEMA_NOM" SortExpression="USUARIO_SISTEMA_NOM"
                                    HeaderStyle-ForeColor="White"></asp:BoundField>
                                <asp:BoundField HeaderText="Fecha Alta" DataField="F_FECHA_ALTA" SortExpression="F_FECHA_ALTA"
                                    HeaderStyle-ForeColor="White"></asp:BoundField>
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
                                <asp:BoundField HeaderText="Fecha Vencimiento" DataField="FECHA_VENCIMIENTO" SortExpression="FECHA_VENCIMIENTO"
                                    HeaderStyle-ForeColor="White"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 410px">
                    <asp:Button ID="BtnExportarInferior" onmouseover="style.backgroundColor='#A9A9A9'"
                        onmouseout="style.backgroundColor='#696969'" runat="server" Style="height: 22px;
                        width: 130px" Text="Exportar Grid a Excel" CssClass="botones" />
                </td>
                <td align="right">
                    <asp:Button ID="btnRegresar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                        runat="server" Style="height: 22px; width: 130px" Text="Regresar" CssClass="botones" />
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
