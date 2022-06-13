<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CompletarIrregularidad.aspx.vb" Inherits="SEPRIS.CompletarIrregularidad" %>

<!DOCTYPE html>

<script type="text/javascript">

    function CerrarCompletar() {
        window.close();
    }

    function AvisoGuardarDatos() {
//        window.opener.__doPostBack('upnlConsulta','');
////        window.opener.document.forms[0].submit();
        alert("Se almacenó correctamente la información");
        window.close();
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Completar irregularidad</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td><asp:Label ID="LblNumIrregularidad" runat="server" Text="Número de irregularidad" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label1" runat="server" Text="*" CssClass="txt_gral"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtNumIrregularidad" runat="server" Width="150px" disabled="true"  CssClass="txt_gral"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="LblFecIrregularidad" runat="server" Text="Fecha Irregularidad" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label3" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtFecIrregularidad" runat="server" Width="150px" disabled ="true" CssClass="txt_gral"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="LblProceso" runat="server" Text="Proceso" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label4" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
                    <td><asp:TextBox ID="TxtProceso" runat="server" Width="377px" disabled ="true" CssClass="txt_gral"></asp:TextBox>
                        </td>
                </tr>
                <tr>
                    <td><asp:Label ID="LblSubproceso" runat="server" Text="Subproceso" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label5" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
                    <td><asp:TextBox ID="TxtSubproceso" runat="server" Width="377px" disabled ="true" CssClass="txt_gral"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label ID="LblConducta" runat="server" Text="Conducta Sancionable" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label6" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
                    <td><asp:TextBox ID="TxtConducta" runat="server" Width="377px" disabled ="true" CssClass="txt_gral"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label2" runat="server" Text="Irregularidad" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label7" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtDescIrregularidad" runat="server" Width="377px" disabled="true" CssClass="txt_gral"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label8" runat="server" Text="La irregularidad se ha corregido" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label9" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
                    <td>
                        <asp:DropDownList ID="ddlCorreccion" runat="server" CssClass="txt_gral">
                            <asp:ListItem Selected="True" Value=""> --Selecciona una opción-- </asp:ListItem>
                            <asp:ListItem Value="Total"> Total </asp:ListItem>
                            <asp:ListItem Value="Parcial"> Parcial </asp:ListItem>
                            <asp:ListItem Value="Nula"> Nula </asp:ListItem>
                        </asp:DropDownList> 
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label10" runat="server" Text="¿Como corrige el PC la irregularidad identificada?" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label11" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="TxtComoCorrigeIrregularidad" runat="server" CssClass="txt_gral" Height="45px" Width="385px"
                            TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td><asp:Label ID="Label12" runat="server" Text="Fecha de corrección" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label13" runat="server" Text="*" CssClass="txt_gral"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="TxtFecCorreccion" runat="server" Width="150px" CssClass="txt_gral"></asp:TextBox>
                        <asp:Image ImageAlign="Bottom" ID="imgCalFechaRecepcion" runat="server" ImageUrl="~/imagenes/Calendar.gif" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnAceptar" runat="server" Text="Agregar"/>
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="CerrarCompletar(); return false;" />
        </div>
    </form>
</body>
</html>
