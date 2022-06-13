<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="Bitacora.aspx.vb" Inherits="SEPRIS.Bitacora" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp1" %>
<%@ Register Src="../Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Bitácora de acciones</label>
                </div>
                <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                    <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                </div>
                <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
                <br />
                <cc1:CustomGridView ID="gvConsulta" AllowSorting="true" runat="server" DataKeyNames="N_ID_BITACORA"
                        Width="100%"  AllowPaging="true" PageSize='<%# ObtenerPaginacion() %>' SkinID="SeleccionSimpleCliente">    
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkElemento" runat="server"/>
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                            <HeaderStyle Width="15px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="FECHA" DataField="F_FECH_BITACORA" />
                        <asp:BoundField HeaderText="USUARIO" DataField="T_ID_USUARIO" >
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="PROCEDIMIENTO" DataField="PROCEDIMIENTO" ItemStyle-CssClass="Wrap" />
                        <asp:TemplateField HeaderText="RESULTADO">
                            <ItemTemplate>
                                <asp:Image ID="imagenEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "RESULTADO"))  %>' />
                            </ItemTemplate>
                            <ItemStyle Width="60px" />
                            <HeaderStyle Width="60px" />
                        </asp:TemplateField>
                    </Columns>
                </cc1:CustomGridView>
                <div id="pnlNoExiste" runat="server" align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Image ID="Image1" runat="server" AlternateText="No existen registros para la consulta"
                        ImageAlign="Middle" ImageUrl="../Imagenes/no EXISTEN.gif" />
                </div>
                <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
                <br />
                <br />
                <asp:Image ID="imgOK" runat="server" />
                <label class="txt_gral">
                    Exitoso</label>
                <asp:Image ID="imgERROR" runat="server" />
                <label class="txt_gral">
                    No Exitoso</label>
                <br />
                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
            </asp:Panel>
            <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static"
                OnClientClick="return false;" />
            <div id="divMensajeUnBotonNoAccion" class="MensajeModal-UI" style="display: none">
                <p>
                    <%= Mensaje%>
                </p>
                <table id="tblMensaje" runat="server" visible="false">
                    <tr>
                        <td>
                            <label class="txt_gral">
                                Procedimiento:</label>
                        </td>
                        <td>
                            <asp:Label ID="txtProcedimiento" CssClass="txt_gral" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="txt_gral">
                                Fecha y hora:</label>
                        </td>
                        <td>
                            <asp:Label ID="txtFecha" CssClass="txt_gral" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="txt_gral">
                                Línea:</label>
                        </td>
                        <td>
                            <asp:Label ID="txtLinea" CssClass="txt_gral" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="txt_gral">
                                Función:</label>
                        </td>
                        <td>
                            <asp:Label ID="txtFuncion" CssClass="txt_gral" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="txt_gral">
                                SQL:</label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="txt_gral">
                                Tabla:</label>
                        </td>
                        <td>
                            <asp:Label ID="txtTabla" CssClass="txt_gral" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
            <script type="text/javascript">
                $(function () {

                    MensajeUnBotonNoAccionLoad();

                });


                function MensajeBitacora() {

                    MensajeUnBotonNoAccion();

                };



 
            </script>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportaExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
