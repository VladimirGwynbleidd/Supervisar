<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="ConsultaAgenda.aspx.vb" Inherits="SEPRIS.ConsultaAgenda" %>

<%@ Register Src="~/Controles/wucAyuda.ascx" TagName="wucAyuda" TagPrefix="wcu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controles/Calendar.ascx" TagName="Calendar" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <wcu:wucAyuda ID="Ayuda" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div align="center" style="padding: 20px 20px 15px 20px">
                <asp:HiddenField ID="hfIdIng" runat="server" />
                <label class="TitulosWebProyectos">
                    Consulta Agenda
                </label>
            </div>
            <div id="divSeleccion" runat="server" style="background: #426939;">
                <table width="100%">
                    <tr>
                        <td style="width:20%" align="left">
                            <asp:Label ID="Label1" runat="server" Text="Tipo:" ForeColor="White"></asp:Label>
                        </td>
                        <td align="left" >
                            <asp:RadioButton ID="rbActividad" runat="server" Text="Actividad" 
                                Checked="true" ForeColor="White" GroupName="Tipo" AutoPostBack="True" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="rbAusencia" runat="server" Text="Ausencia" 
                                ForeColor="White" GroupName="Tipo" AutoPostBack="True"/>
                            &nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="rbServicio" runat="server" Text="Atención de Servicios" 
                                ForeColor="White" GroupName="Tipo" AutoPostBack="True"/>
                            &nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="rbTodos" runat="server" Text="Todos" ForeColor="White" 
                                GroupName="Tipo" AutoPostBack="True"/>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divDropDownList" runat="server" visible="false" style="background: #426939;">
                <table width="100%">
                    <tr>
                        <td align="left" style="width: 20%" >
                            <asp:Label ID="lblIngeniros" runat="server" Text="Ingenieros:" ForeColor="White" ></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlIngenieros" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divCalendar" runat="server" visible="false">
                <table width="100%" style="background: #ffffff;">
                    <tr>
                        <td align="left" style="background: #C0C0C0; width: 30%;">
                            <asp:Button ID="btnMesAnt" runat="server" Text="Mes Anterior" BackColor="Silver"
                                Height="16px" Width="101px" Font-Size="Small" />
                            &nbsp;
                            <asp:Button ID="btnMesSig" runat="server" Text="Mes Siguiente" BackColor="Silver"
                                Height="16px" Width="101px" Font-Size="Small" />
                        </td>
                        <td style="background: #C0C0C0;">
                            <asp:Label ID="lblMesAnio" runat="server" Text="" ForeColor="White" Font-Size="Large"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <uc1:Calendar runat="server" ID="calendar1" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
