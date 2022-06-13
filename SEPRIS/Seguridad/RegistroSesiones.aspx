<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="RegistroSesiones.aspx.vb" Inherits="SEPRIS.RegistroSesiones" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp1" %>
<%@ Register Src="../Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .txt_gral_blanco
        {
            width: 37px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Registro de Sesiones</label>
                </div>
                <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                    <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                </div>
                <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
                <br />
                <cc1:CustomGridView  ID="gvConsulta" runat="server" DataKeyNames="T_ID_SESION"
                                    Width="100%" ColumnasCongeladas="0" DataBindEnPostBack="True" HabilitaScroll="False"
                                    HabilitaSeleccion="No" HeigthScroll="0" ToolTipHabilitado="False" UnicoEnPantalla="True">
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox Visible="false" runat="server"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="FECHA" DataField="FECHA" />
                        <asp:BoundField HeaderText="USUARIO" DataField="T_ID_USUARIO" />
                        <asp:BoundField HeaderText="DIRECCIÓN IP" DataField="T_DSC_DIRECCION_IP" />
                        <asp:BoundField HeaderText="HORA INICIO" DataField="HORAINICIO" />
                        <asp:BoundField HeaderText="HORA FIN" DataField="HORAFIN" />
                    </Columns>
                </cc1:CustomGridView>
                <div id="pnlNoExiste" runat="server" align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Image ID="Image1" runat="server" AlternateText="No existen registros para la consulta"
                        ImageAlign="Middle" ImageUrl="../Imagenes/no EXISTEN.gif" />
                </div>
                <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />
                <br />
                <br />
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportaExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
