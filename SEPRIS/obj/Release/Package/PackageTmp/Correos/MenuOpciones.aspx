<%@ Page Title="" Language="vb" AutoEventWireup="false" ValidateRequest="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="MenuOpciones.aspx.vb" Inherits="SEPRIS.MenuOpciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <div align="center" style="padding: 20px 20px 15px 20px">
      <asp:Label ID="lblTitulo" runat="server" Text="Opciones de Perfil" CssClass="TitulosWebProyectos"></asp:Label>
   </div>

    <div align="center" style="padding: 20px 20px 15px 20px">
        <asp:Button id="btnCorreos" runat="server" Width="296px" Height="42px" 
        Text="Correos" CssClass="botones"></asp:Button>  
   </div>

     <div align="center" style="padding: 20px 20px 15px 20px">
        <asp:Button id="bCorreoPerfil" runat="server"  Width="296px" Height="42px" 
        Text="Correo - Perfil" CssClass="botones"></asp:Button>
    </div>

     <div align="center" style="padding: 20px 20px 15px 20px">
         <asp:Button id="btnExpeciones" runat="server" Width="296px" Height="42px"
         Text="Usuario - Exclusión" CssClass="botones"></asp:Button>
    </div>

</asp:Content>
