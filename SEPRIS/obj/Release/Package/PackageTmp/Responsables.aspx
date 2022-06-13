<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Responsables.aspx.vb" Inherits="SEPRIS.Responsables" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" style="width:100%; padding: 10px 21px 10px 21px;"> 
      <asp:Label ID="lblTitulo" runat="server" CssClass="TitulosWebProyectos" EnableTheming="false"></asp:Label>
      <br /><br /><br />
        
    <% Mesa_Ayuda_Tecnica()%>

        
    </div>

    <div align="center" style="width:100%; padding: 10px 21px 10px 21px;">    
       <% Mesa_Ayuda_Funcional()%>
       <br />
        <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CssClass="botones" />
    </div>
</asp:Content>
