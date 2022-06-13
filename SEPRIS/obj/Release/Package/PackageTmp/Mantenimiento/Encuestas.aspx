<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="Encuestas.aspx.vb" Inherits="SEPRIS.Encuestas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" style="padding: 20px 20px 15px 20px">
        <label class="TitulosWebProyectos">
            Encuestas</label>
    </div>
    <br /><br />
    <div align="center" style="padding: 20px 20px 15px 20px">
        <asp:Button ID="btnAspectosEvaluar" runat="server" Width="200px" Text="Aspectos a Evaluar" />
        <br /><br />
        <asp:Button ID="btnOpcionesEvaluacion" runat="server" Width="200px" Text="Opciones de Evaluación" />
        <br /><br />
        <asp:Button ID="btnEncuestas" runat="server" Width="200px" Text="Catálogo de Encuestas" />
        <br /><br />
    </div>
</asp:Content>
