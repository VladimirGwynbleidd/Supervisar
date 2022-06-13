<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BitacoraVisita.ascx.vb" Inherits="SEPRIS.BitacoraVisita" %>
<%@ Register Src ="~/Controles/ucFiltro.ascx" TagName ="ucFiltro" TagPrefix ="uc1" %>

<asp:UpdatePanel ID="upnlConsulta" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
            <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                </div> 
        <div >
            <uc1:ucFiltro ID="ucFiltro1" runat ="server" Width="100%" />
        </div>
         
            <cc1:CustomGridView ID="gvConsulta" AllowSorting="true" runat="server" Width="100%" ColumnasCongeladas="1"  HabilitaScroll="False" HiddenFieldSeleccionSimple="hfSelectedValue"
            HeigthScroll="0" ToolTipHabilitado="True" UnicoEnPantalla="True" WidthScroll="0" 
            AllowPaging="false" PageSize="100">                
            <Columns>
                    <asp:BoundField HeaderText="Fecha movimiento" DataField="F_FECH_REGISTRO" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_REGISTRO"></asp:BoundField>
                    <asp:BoundField HeaderText="Usuario" DataField="T_ID_USUARIO" SortExpression="T_ID_USUARIO"></asp:BoundField>
                    <asp:BoundField HeaderText="Paso" DataField="I_ID_PASO" SortExpression="I_ID_PASO"></asp:BoundField>
                    <asp:BoundField HeaderText="Descripción de movimiento " DataField="T_DSC_MOVIMIENTO" SortExpression="T_DSC_MOVIMIENTO"></asp:BoundField>
                    <asp:BoundField HeaderText="Comentarios" DataField="T_DSC_COMENTARIO" SortExpression="COMENTARIOS"></asp:BoundField>
                </Columns>
            </cc1:CustomGridView>
            <img id="Noexisten" runat="server" src="~/Imagenes/No%20Existen.gif" visible="false"
                alt="No existen Registos para la Consulta" />
            <br />
            <br />
            <br />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportaExcel" />
        </Triggers>
</asp:UpdatePanel> 