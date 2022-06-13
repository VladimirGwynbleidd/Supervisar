<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Error.aspx.vb" Inherits="SEPRIS._Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content  ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div>
    	<asp:Label id="Label1" style="Z-INDEX: 102; left: 72px; POSITION: absolute; TOP: 56px" runat="server"
				Width="536px" Height="64px" Font-Names="Arial" Font-Size="Large" BackColor="White" BorderColor="Black"
				BorderStyle="None" BorderWidth="1px" ForeColor="#000040">Un error ha ocurrido... </asp:Label>
			<asp:Label id="Label2" style="Z-INDEX: 101; LEFT: 264px; POSITION: absolute; TOP: 136px" runat="server"
				Width="536px" Height="64px" Font-Names="Arial" Font-Size="Medium" BackColor="White" BorderColor="Black"
				BorderStyle="None" BorderWidth="1px" ForeColor="#000040">Por el momento no es imposible completar su petición. Por favor de enviar una imagen y descripción del error a nuestros administradores del sistema al correo <a href="mailto:AtencionSEPRIS@consar.gob.mx?subject=Error%20SuperviSAR">Atención SuperviSAR</a> , quienes revisarán y resolverán el problema.</asp:Label>
    </div>
    
    <div style="display:none">
        
        <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
    </div>

</asp:Content>
