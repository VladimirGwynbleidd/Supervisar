<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Logout.aspx.vb" Inherits="SEPRIS.Logout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table style="height:200px">
        <tr>
            <td>
                <label style="color:#CA0016">Su sesión ha finalizado...</label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnCerrar" Text="Cerrar" runat="server" />            
            </td>
        </tr>
    </table>
    


</asp:Content>
