<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Bitacora.ascx.vb" Inherits="SEPRIS.Bitacora2" %>

<%@ Register Src="~/Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>

    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
            </div>
            <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
            
            <br />
            <cc1:CustomGridView ID="gvConsulta" AllowSorting="true" runat="server"             Width="100%" ColumnasCongeladas="0" DataBindEnPostBack="True" HabilitaScroll="False" HiddenFieldSeleccionSimple="hfSelectedValue"
            HeigthScroll="0" ToolTipHabilitado="True" UnicoEnPantalla="True" WidthScroll="0" SelectedMultiRows="false" 
            AllowPaging="true" PageSize="15">                
            <Columns>
                    <asp:BoundField HeaderText="Fecha" DataField="F_FECH_REGISTRO" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_REGISTRO"></asp:BoundField>
                    <asp:BoundField HeaderText="Usuario" DataField="T_DSC_USUARIO" SortExpression="T_DSC_USUARIO"></asp:BoundField>
                    <asp:BoundField HeaderText="Paso" DataField="T_DSC_PASO" SortExpression="T_DSC_PASO"></asp:BoundField>
                    <asp:BoundField HeaderText="Acción" DataField="T_DSC_ACCION" SortExpression="T_DSC_ACCION"></asp:BoundField>
                    <asp:BoundField HeaderText="Comentarios" DataField="T_DSC_COMENTARIOS" SortExpression="T_DSC_COMENTARIOS"></asp:BoundField>
                </Columns>
            </cc1:CustomGridView>
            <img id="Noexisten" runat="server" src="../Imagenes/No%20Existen.gif" visible="false"
                alt="No existen Registos para la Consulta" />
            <br />
            <br />
            <br />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportaExcel" />
        </Triggers>
    </asp:UpdatePanel>

