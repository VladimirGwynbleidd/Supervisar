<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="BandejaRegistrarAgenda.aspx.vb" Inherits="SEPRIS.BandejaRegistrarAgenda" %>

<%@ Register Src="~/Controles/wucAyuda.ascx" TagName="wucAyuda" TagPrefix="wcu" %>
<%@ Register Src="~/Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/wucPersonalizarColumnas.ascx" TagName="wucPersonalizarColumnas" TagPrefix="wuc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <wcu:wucAyuda ID="Ayuda" runat="server" />
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>            
            <div align="center" style="padding: 20px 20px 15px 20px">
                <label class="TitulosWebProyectos">
                    Bandeja de Agenda</label>
            </div>
            <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                &nbsp;
                <asp:Button ID="btnPersonalizarColumnas" runat="server" Text="Personalizar Columnas"
                    Style="width: 120px;" />
            </div>
            <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
            <br />
            <cc1:CustomGridView ID="gvConsulta" AllowSorting="true" runat="server" DataKeyNames="N_ID_REGISTRO_AGENDA"
                Width="100%" ColumnasCongeladas="0" DataBindEnPostBack="True" HabilitaScroll="False"
                HeigthScroll="0" ToolTipHabilitado="True" UnicoEnPantalla="True" WidthScroll="0"
                AllowPaging="true" PageSize="15">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkElemento" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="15px" />
                        <HeaderStyle Width="15px" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="ID" DataField="N_ID_REGISTRO_AGENDA" SortExpression="N_ID_REGISTRO_AGENDA">
                        <ItemStyle Width="50px" />
                        <HeaderStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Registra" DataField="SOLICITANTE" SortExpression="SOLICITANTE">
                        <ItemStyle CssClass="Wrap" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Autoriza" DataField="AUTORIZADOR" SortExpression="AUTORIZADOR">
                        <ItemStyle CssClass="Wrap" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Tipo de Registro" DataField="TIPO_REGISTRO" SortExpression="TIPO_REGISTRO">
                        <ItemStyle CssClass="Wrap" />
                        <ItemStyle Width="110px" />
                        <HeaderStyle Width="110px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Fecha de Inicio" DataField="F_FECH_INICIO_REGISTRO" SortExpression="F_FECH_INICIO_REGISTRO">
                        <ItemStyle Width="100px" />
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Fecha de Fin" DataField="F_FECH_FIN_REGISTRO" SortExpression="F_FECH_FIN_REGISTRO">
                        <ItemStyle Width="100px" />
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Estatus" SortExpression="B_FLAG_APROBADO">
                        <ItemTemplate>
                            <asp:Image ID="imagenAutorizacion" runat="server" ImageUrl='<%# ObtenerImagenAutorizacion(DataBinder.Eval(Container.DataItem, "B_FLAG_APROBADO"))  %>'
                                Width="15px" Height="15px" />
                        </ItemTemplate>
                        <ItemStyle Width="60px" />
                        <HeaderStyle Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vigencia" SortExpression="B_FLAG_VIG">
                        <ItemTemplate>
                            <asp:Image ID="imagenEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "B_FLAG_VIG"))  %>' />
                        </ItemTemplate>
                        <ItemStyle Width="65px" />
                        <HeaderStyle Width="65px" />
                    </asp:TemplateField>
                </Columns>
            </cc1:CustomGridView>
            <img id="Noexisten" runat="server" src="../Imagenes/No%20Existen.gif" visible="false"
                alt="No existen Registos para la Consulta" />
            <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
            <br />
            <br />
            <div>
                <label class="txt_gral">
                    Estatus:</label>
                <asp:Image ID="imgAprobado" runat="server" />
                <label class="txt_gral">
                    Aprobado</label>
                <asp:Image ID="imgRechazado" runat="server" />
                <label class="txt_gral">
                    Rechazado</label>
                <asp:Image ID="imgPendiente" runat="server" Width="15px" Height="15px" />
                <label class="txt_gral">
                    Pendiente</label>
                <br />
                <label class="txt_gral">
                    Vigencia:
                </label>
                <asp:Image ID="imgOK" runat="server" />
                <label class="txt_gral">
                    Vigente</label>
                <asp:Image ID="imgERROR" runat="server" />
                <label class="txt_gral">
                    No vigente</label>
            </div>
            <br />
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar"  />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnModificar" runat="server" Text="Modificar" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar"/>
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" Visible="false" />
            <div id="divMensajeUnBotonNoAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                                <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divMensajeDosBotonesUnaAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px"
                                ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnConsulta" Style="display: none" ClientIDMode="Static" />
            <script type="text/javascript">
                $(function () {

                    MensajeUnBotonNoAccionLoad();
                    MensajeUnBotonUnaAccionLoad();

                });


                function AquiMuestroMensaje() {

                    MensajeUnBotonNoAccion();

                };


                function ConfirmacionEliminar() {

                    MensajeDosBotonesUnaAccion();

                };


                function MensajeConfirmacion() {
                    MensajeDosBotonesUnaAccion();
                }

            </script>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAceptarM1B1A" />
            <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
            <asp:PostBackTrigger ControlID="btnConsulta" />
            <asp:PostBackTrigger ControlID="btnExportaExcel" />
            <asp:PostBackTrigger ControlID="btnPersonalizarColumnas" />
        </Triggers>
    </asp:UpdatePanel>

    <wuc:wucPersonalizarColumnas ID="PersonalizaColumnas" runat="server"/>

</asp:Content>
