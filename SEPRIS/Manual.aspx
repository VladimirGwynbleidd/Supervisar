<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Manual.aspx.vb" Inherits="SEPRIS.Manual" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" style="padding: 20px 20px 15px 20px">
        <asp:Label ID="lblTitulo" runat="server" Text="Manual de Usuario" CssClass="TitulosWebProyectos" EnableTheming="false"></asp:Label>
    </div>

    <div align="center" style="padding: 20px 20px 15px 20px" >

        <table onmouseover="style.backgroundColor='#ADD8E6'" onmouseout="style.backgroundColor='#FFF'""
            cellspacing="0" width="400px" border="0">
            <tr>
                <td valign="middle" align="center" width="20px" height="30px">
                    <img src="Imagenes/down_arrow_grey.png" alt="arrow_next.gif" />
                </td>
                <td style="font-family: Arial, Helvetica, sans-serif; font-size: 17px; cursor: pointer"
                    width="380px" height="30px" onclick="parent.location.href='Manuales/Manual_Usuario_Inspeccion.pdf'"
                    align="left">
                    Manual de Usuario Inspección.
                </td>
              </tr>
            </table>
           <table onmouseover="style.backgroundColor='#ADD8E6'" onmouseout="style.backgroundColor='#FFF'""
            cellspacing="0" width="400px" border="0">
               <tr>
                <td valign="middle" align="center" width="20px" height="30px">
                    <img src="Imagenes/down_arrow_grey.png" alt="arrow_next.gif" />
                </td>
                <td style="font-family: Arial, Helvetica, sans-serif; font-size: 17px; cursor: pointer"
                    width="380px" height="30px" onclick="parent.location.href='Manuales/Manual_Usuario_Vigilancia.pdf'"
                    align="left">
                    Manual de Usuario Vigilancia.
                </td>
            </tr>
        </table>
    </div>

     <div align="center" style="padding: 90px 20px 0px 20px;">

        <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CssClass="botones" />

        
    </div>


</asp:Content>
