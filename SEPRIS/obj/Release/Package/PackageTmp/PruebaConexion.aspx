<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PruebaConexion.aspx.vb" Inherits="SEPRIS.PruebaConexion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="400px" border="1">
            <tr>
                <td>
                    <asp:Button ID="btnProbarConexion" runat="server" Text="Probar Conexión" 
                        Width="100px" />
                </td>
                <td>
                    <asp:Label ID="lblMensaje" runat="server"></asp:Label>
                </td>
                <td>
                
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnInicializar" runat="server" Text="Inicializar" 
                        Width="100px" />
                </td>
                <td>
                    <asp:Label ID="lblInicializar" runat="server"></asp:Label>
                </td>
                <td>&nbsp;</td>            
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnBulkCopy" runat="server" Text="Bulk Copy" Width="100px" />
                </td>
                <td>
                    <asp:Label ID="lblBulkCopy" runat="server"></asp:Label>
                </td>
                <td></td>            
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnVarbinary" runat="server" Text="Varbinary" Width="100px" />
                </td>
                <td>
                    <asp:Label ID="lblVarbinary" runat="server"></asp:Label>
                </td>
                <td>&nbsp;</td>            
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
