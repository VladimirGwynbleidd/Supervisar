<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Requerimientos.aspx.vb" Inherits="SEPRIS.Requerimientos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <div align="center" style="padding: 20px 20px 15px 20px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="TitulosWebProyectos" Text="Requerimientos Mínimos" EnableTheming="false" ></asp:Label>
    </div>

     <div align="center" style="padding: 20px 20px 15px 20px">
         <asp:GridView ID="gvRequerimientos" runat="server" ShowHeader="false" 
             AutoGenerateColumns="False" BorderStyle="None" BorderWidth="0px" 
             CellPadding="0" EnableTheming="false">
             <EditRowStyle BorderStyle="None" BorderColor="White" BorderWidth="0" /> 
         <Columns>
         
         <asp:TemplateField ShowHeader="false" ItemStyle-BorderStyle="None" ItemStyle-BorderColor="White">
             <ItemTemplate>
                 <asp:Image ID="img" runat="server" ImageUrl="~/Imagenes/arrow_next.gif"/>
             </ItemTemplate>
         </asp:TemplateField>
         <asp:BoundField DataField="T_DSC_VALOR" ShowHeader="false"  ItemStyle-CssClass="txt_gral" ItemStyle-HorizontalAlign="Left" ControlStyle-BorderStyle="None" ItemStyle-BorderColor="White" HtmlEncode="false"/>
         
         </Columns>
         </asp:GridView>
       </div>

    <div align="center" style="padding: 150px 20px 0px 20px;">

        <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CssClass="botones" />

        
    </div>


</asp:Content>
