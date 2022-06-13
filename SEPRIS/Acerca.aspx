<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Acerca.aspx.vb" Inherits="SEPRIS.Acerca" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
    <div align="center" style="padding: 20px 20px 15px 20px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="TitulosWebProyectos" EnableTheming="false"></asp:Label>
    </div>

    <div align="center" style="text-align:justify; padding: 20px 21px 20px 21px;">

            <asp:Label ID="lblDescripcion" runat="server" CssClass="txt_gral"> </asp:Label>

    </div>

    <div align="center" style="padding: 20px 20px 0px 20px;">

        <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CssClass="botones" />

        
    </div>

</asp:Content> 

