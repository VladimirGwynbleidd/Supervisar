<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="BandejaSICOD.aspx.vb" Inherits="SEPRIS.BandejaSICOD" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controles/wucAyuda.ascx" TagName="wucAyuda" TagPrefix="wcu" %>
<%@ Register Src="~/Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/wucPersonalizarColumnas.ascx" TagName="wucPersonalizarColumnas" TagPrefix="wuc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <div align="center" style="padding: 20px 20px 15px 20px">
                <label class="TitulosWebProyectos">
                    Bandeja registros SICOD</label>
            </div>
            <div style="text-align: left; width: 100%; padding-bottom: 5px;">
                <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                &nbsp;
                <asp:Button ID="btnPersonalizarColumnas" runat="server" Text="Personalizar Columnas"
                    Style="width: 130px;" />
            </div>
            <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="100%" />
            <br />
            <cc1:CustomGridView ID="gvConsulta" runat="server" DataKeyNames="N_ID_FOLIO, N_ID_SICOD" AutoGenerateColumns="false" HabilitaScroll="true" HeigthScroll="400" UnicoEnPantalla="true" HeaderStyle-Font-Size="14" Width="100%"
                ColumnasCongeladas="1" HabilitaSeleccion="No">
                <Columns>
                   
                    <asp:BoundField HeaderText="Folio sicod" DataField="N_ID_SICOD" SortExpression="N_ID_SICOD" ItemStyle-CssClass="Wrap"></asp:BoundField>
                    <asp:BoundField HeaderText="Fecha de registro" DataField="F_FECH_REGISTRO" SortExpression="F_FECH_REGISTRO" ItemStyle-CssClass="Wrap"></asp:BoundField>
                    <asp:BoundField HeaderText="Número de Oficio" DataField="T_DSC_NUM_OFICIO" SortExpression="T_DSC_NUM_OFICIO" ItemStyle-CssClass="Wrap"></asp:BoundField>
                    <asp:BoundField HeaderText="Referencia" DataField="T_DSC_REFERENCIA" SortExpression="T_DSC_REFERENCIA" ItemStyle-CssClass="Wrap"></asp:BoundField>
                    <asp:BoundField HeaderText="Fecha Documento" DataField="F_FECH_DOC" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_DOC" ItemStyle-CssClass="Wrap"></asp:BoundField>
                    <asp:BoundField HeaderText="Fecha Recepción doc" DataField="F_FECH_RECEPCION" DataFormatString="{0:dd/MM/yyyy}" SortExpression="F_FECH_RECEPCION" ItemStyle-CssClass="Wrap"></asp:BoundField>
                    <asp:BoundField HeaderText="Tipo Documento" DataField="T_DSC_T_DOC" SortExpression="T_DSC_T_DOC" ItemStyle-CssClass="Wrap"></asp:BoundField>
                    <asp:BoundField HeaderText="Remitente" DataField="T_DSC_REMITENTE" SortExpression="T_DSC_REMITENTE" ItemStyle-CssClass="Wrap"></asp:BoundField>
                    <asp:BoundField HeaderText="Nombre firmante" DataField="T_DSC_NOMB_FIRMNT" SortExpression="T_DSC_NOMB_FIRMNT" ItemStyle-CssClass="Wrap"></asp:BoundField>
                    <asp:BoundField HeaderText="Apellido Paterno" DataField="T_DSC_AP_PAT_FIRMNT" SortExpression="T_DSC_AP_PAT_FIRMNT" ItemStyle-CssClass="Wrap"></asp:BoundField>
                    <asp:BoundField HeaderText="Apellido Materno" DataField="T_DSC_AP_MAT_FIRMNT" SortExpression="T_DSC_AP_MAT_FIRMNT" ItemStyle-CssClass="Wrap"></asp:BoundField>
                    <asp:BoundField HeaderText="ASUNTO" DataField="T_DSC_ASUNTO" SortExpression="T_DSC_ASUNTO" ItemStyle-CssClass="Wrap"></asp:BoundField>
                    <asp:BoundField HeaderText="ESTATUS" DataField="T_DSC_ESTATUS" SortExpression="T_DSC_ESTATUS" ItemStyle-CssClass="Wrap"></asp:BoundField>
                    <asp:BoundField DataField="T_DSC_UNIDAD_ADM" SortExpression="T_DSC_UNIDAD_ADM" Visible="false"></asp:BoundField>
                </Columns>
            </cc1:CustomGridView>
            <img id="Noexisten" runat="server" src="../Imagenes/No%20Existen.gif" visible="false"
                alt="No existen Registos para la Consulta" />
            <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
            <br />
            <br />
            <br />
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
    <wuc:wucPersonalizarColumnas ID="PersonalizaColumnas" runat="server" />
</asp:Content>
