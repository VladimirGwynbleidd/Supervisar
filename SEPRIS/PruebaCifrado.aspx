<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PruebaCifrado.aspx.vb" Inherits="SEPRIS.PruebaCifrado" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table> 
            <tr>
                <td>
                    <asp:ImageButton ID="imgCandado" runat="server" 
                        ImageUrl="~/Imagenes/candado3.jpg" Height="47px" Width="59px" />
                </td>
                <td align="center">
                    <asp:RadioButtonList ID="rdOpciones" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                        <asp:ListItem Value="AES" Text="AES" Selected="True"></asp:ListItem> 
                        <asp:ListItem Value="SHA512" Text="SHA512"></asp:ListItem> 
                    </asp:RadioButtonList>
                </td>
           </tr>
            <tr>
                <td>
                    <label>Entrada:</label>
                </td>
                <td>
                    <asp:TextBox ID="txtEntrada" runat="server" Columns="3" Rows="3" 
                        TextMode="MultiLine" Width="600px">Data Source={0};Initial Catalog={1};User Id={2};Password={3};</asp:TextBox>
                </td>
           </tr>
            <tr>
                <td>
                    <label>Salida:</label>
                </td>
                <td>
                    <asp:TextBox ID="txtSalida" runat="server" Columns="3" ReadOnly="True" Rows="3" 
                        TextMode="MultiLine" Width="600px"></asp:TextBox>
                </td>
           </tr>
            <tr>
                <td>
                    </td>
                <td align="center">
                    <asp:Button ID="btnCifrar" runat="server" Text="Cifrar" />&nbsp;
                    <asp:Button ID="btnDescifrar" runat="server" Text="Descifrar" />
                </td>
           </tr>
        </table>
    </div>
    </form>
</body>
</html>
