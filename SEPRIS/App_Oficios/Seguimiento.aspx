<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Seguimiento.aspx.vb" ValidateRequest="false"
    Inherits="SICOD.Seguimiento" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="~/Styles.css" />
    <script language="javascript" type="text/javascript">

        function LevantaVentanaOficio() {

            var numExpediente = document.getElementById('hdnExpediente').value;

            var url = '';

            if (numExpediente > 0)
                url = 'Bandeja.aspx?vm=1&cl=1&le=1';
            else
                url = 'Bandeja.aspx?vm=1&cl=1';

            var ancho = document.body.clientHeight;

            if (ancho < 600)
                ancho = 600;

            if (ancho > 800)
                ancho = 800;

            winprops = "dialogHeight: " + parseInt(ancho * 0.80).toString() + "px; dialogWidth: " + parseInt(document.body.clientWidth * 0.90).toString() + "px; edge: Raised; center: Yes; help: No;resizable: No; status: No; Location: No; Titlebar: No;"

            window.showModalDialog(url, "", winprops);


            document.getElementById('lnkVentanaOficio').click();


            return false;

        }


        function LevantaVentanaFolio() {

            var numExpediente = document.getElementById('hdnExpediente').value;

            var url = '';

            if (numExpediente > 0)
                url = '/BandejaEntrada.aspx?ie=1';
            else
                url = '/BandejaEntrada.aspx?ie=1&se=1';


            var ancho = document.body.clientHeight;

            if (ancho < 600)
                ancho = 600;

            if (ancho > 800)
                ancho = 800;

            winprops = "dialogHeight: " + parseInt(ancho * 0.80).toString() + "px; dialogWidth: " + parseInt(document.body.clientWidth * 0.90).toString() + "px; edge: Raised; center: Yes; help: No;resizable: No; status: No; Location: No; Titlebar: No;"

            window.showModalDialog(url, "", winprops);

            document.getElementById('lnkVentanaFolio').click();

            return false;

        }


    </script>
    <style type="text/css">
        .botones
        {
            margin-bottom: 1px;
        }
        .txt_gral
        {
        }
        .style6
        {
            width: 144px;
            height: 31px;
        }
        .style8
        {
        }
        .style9
        {
            width: 74px;
            height: 31px;
        }
        .style11
        {
            width: 144px;
        }
        .style14
        {
            width: 74px;
        }
    </style>
</head>
<body>
    <form id="frmSeguimiento" runat="server">
    <br />
    <center>
        <asp:Label runat="server" ID="lblTitulo" Text="Seguimiento y Vinculación de Documentos"
            CssClass="TitulosWebProyectos"></asp:Label>
        <br />
        <asp:Label runat="server" ID="lblDescripcion" Text="El Oficio no se encuentra actualmente ligado a ningún expediente"
            CssClass="titulo_notaprincipal"></asp:Label>
    </center>
    <br />
    <asp:UpdatePanel ID="updPanelSeguimiento" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGuardar" />
            <asp:PostBackTrigger ControlID="btnNuevoComentario" />
            <asp:PostBackTrigger ControlID="dgComentarioSeguimiento" />
            <asp:PostBackTrigger ControlID="gvVincularOficios" />
            <asp:PostBackTrigger ControlID="gvVincularDocsSICOD" />
            <asp:PostBackTrigger ControlID="lnkVentanaOficio" />
            <asp:PostBackTrigger ControlID="lnkVentanaFolio" />
        </Triggers>
        <ContentTemplate>
            <asp:ToolkitScriptManager ID="Toolkitscriptmanager1" runat="server" EnableScriptGlobalization="true"
                AsyncPostBackTimeout="7200">
            </asp:ToolkitScriptManager>
            <table>
            </table>
            <asp:Panel runat="server" ID="pnlVincularOficios" GroupingText="Vincular Oficios"
                Width="100%" Style="margin: 0 auto; padding: 10px 0 10px 0;">
                <table width="100%" align="center">
                    <tr>
                        <td>
                            <asp:Label ID="lblNoHayOficiosRelacionados" runat="server" CssClass="txt_gral" Style="display: none;
                                padding: 15px;">No hay oficios vinculados</asp:Label>
                            <asp:GridView ID="gvVincularOficios" runat="server" ForeColor="#555555" Font-Size="7.5pt"
                                CellPadding="1" BackColor="#EEEEEE" AutoGenerateColumns="False" Font-Names="Arial"
                                HorizontalAlign="Center" Font-Name="Arial" PageSize="15" BorderColor="White"
                                AllowSorting="false" DataKeyNames="ID_UNIDAD_ADM,ID_ANIO,I_OFICIO_CONSECUTIVO,ID_TIPO_DOCUMENTO, INICIAL_FLAG, T_HYP_ARCHIVOSCAN"
                                Width="98%">
                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="NotSet" CssClass="GridViewHeaderOficios"
                                    ForeColor="White" Font-Size="7.5pt"></HeaderStyle>
                                <RowStyle Wrap="true" />
                                <Columns>
                                    <asp:ButtonField Text="O" CommandName="Editar" ItemStyle-HorizontalAlign="Center"
                                        ButtonType="Link" ControlStyle-ForeColor="Blue"></asp:ButtonField>
                                    <asp:BoundField HeaderText="Area" DataField="DSC_UNIDAD_ADM" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-ForeColor="White" HeaderStyle-CssClass="BO_Column">
                                        <HeaderStyle CssClass="BO_Column" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Oficio" ItemStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="White"
                                        HeaderStyle-CssClass="BO_Column">
                                        <HeaderStyle CssClass="BO_Column" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblTipoDoc" ForeColor="Blue" ToolTip="Abrir Archivo PDF" runat="server"
                                                HeaderStyle-ForeColor="White" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                CommandName="VerPDF" Text='<%# Bind("T_OFICIO_NUMERO") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Asunto" DataField="T_ASUNTO" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-ForeColor="White" HeaderStyle-CssClass="BO_Column">
                                        <HeaderStyle CssClass="BO_Column" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Elaboró" DataField="ELABORO" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-ForeColor="White" HeaderStyle-CssClass="BO_Column">
                                        <HeaderStyle CssClass="BO_Column" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Entidad" DataField="T_ENTIDAD_CORTO" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-ForeColor="White" HeaderStyle-CssClass="BO_Column">
                                        <HeaderStyle CssClass="BO_Column" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Año" DataField="ID_ANIO" HeaderStyle-CssClass="BO_Column"
                                        HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle CssClass="BO_Column" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Tipo de Documento" DataField="T_TIPO_DOCUMENTO" HeaderStyle-ForeColor="White"
                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column">
                                        <HeaderStyle CssClass="BO_Column" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Destinatario" DataField="DESTINATARIO" HeaderStyle-ForeColor="White"
                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column">
                                        <HeaderStyle CssClass="BO_Column" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Fecha de Documento" DataField="F_FECHA_OFICIO" HeaderStyle-ForeColor="White"
                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column">
                                        <HeaderStyle CssClass="BO_Column" />
                                    </asp:BoundField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Vencimiento" SortExpression="F_FECHA_VENCIMIENTO"
                                        HeaderStyle-CssClass="BO_Column" HeaderStyle-ForeColor="White" Visible="false">
                                        <ItemTemplate>
                                            <asp:Image Style="height: 20px; width: 20px" ID="imgVencimiento" runat="server" ImageUrl="" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField Text="Eliminar" CommandName="Eliminar" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table style="height: 40px; width: 100%;">
                    <tr>
                        <td align="right" valign="middle">
                            <asp:Button ID="btnAgregarOficio" runat="server" CssClass="botones" Height="22px"
                                onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                Text="Agregar" Width="130px" OnClientClick="return LevantaVentanaOficio();" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table style="width: 818px; margin: 0 auto;" id="Table1" runat="server">
                <tr>
                    <td>
                        <asp:Panel ID="Panel2" runat="server" Style="padding: 8px;" CssClass="txt_gral" GroupingText="Vencimiento" Visible="false">
                            <div style="padding: 8px">
                                &nbsp;<asp:Image ID="Image4" runat="server" Height="22px" ImageAlign="AbsBottom"
                                    ImageUrl="~/imagenes/statusNormal.png" Width="22px" />
                                <asp:Label ID="lblNormal" runat="server" CssClass="txt_gral">= Normal</asp:Label>
                                &nbsp;<asp:Image ID="Image3" runat="server" Height="22px" ImageAlign="AbsBottom"
                                    ImageUrl="~/imagenes/PREVENTIVO.png" Width="22px" />
                                <asp:Label ID="lblPorVencer" runat="server" CssClass="txt_gral">= Por vencer</asp:Label>
                                &nbsp;<asp:Image ID="Image5" runat="server" Height="22px" ImageUrl="~/imagenes/VENCIDO.png"
                                    Width="22px" />
                                <asp:Label ID="lblVencido" runat="server" CssClass="txt_gral">= Vencido</asp:Label>
                                &nbsp;<asp:Image ID="Image17" runat="server" Height="22px" ImageUrl="~/imagenes/ATENDIDO.png"
                                    Width="22px" />
                                <asp:Label ID="Label15" runat="server" CssClass="txt_gral">= Concluido/Cancelado</asp:Label>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlNuevoComentario" runat="server">
                <table align="center" width="818px">
                    <tr>
                        <td class="style14">
                            &nbsp;
                        </td>
                        <td class="style11">
                            <asp:Label ID="lblFechaEnvio" runat="server" CssClass="txt_gral" Text="Fecha"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFechaEnvio" runat="server" Style="width: 115px" CssClass="txt_gral"></asp:TextBox>
                            <asp:CalendarExtender runat="server" ID="calFechaEnvio" TargetControlID="txtFechaEnvio"
                                Format="dd/MM/yyyy" PopupButtonID="imgCalFechaDocumento">
                            </asp:CalendarExtender>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblOficio" runat="server" CssClass="txt_gral" Text="Oficio"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="textOficio" runat="server" CssClass="txt_gral" ReadOnly="True" Style="width: 120px"
                                Text="Número de oficio"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style14">
                            &nbsp;
                        </td>
                        <td class="style11">
                        </td>
                    </tr>
                    <tr>
                        <td class="style14">
                            &nbsp;
                        </td>
                        <td class="style11">
                            <asp:Label ID="lbldocumentoRelacionado" runat="server" CssClass="txt_gral" Text="Documento Relacionado"></asp:Label>
                        </td>
                        <td>
                            <asp:FileUpload ID="fUpOficioProcedente" runat="server" Width="330px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style14">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style14">
                            &nbsp;
                        </td>
                        <td class="style11">
                            <asp:Label ID="lblComentario" runat="server" CssClass="txt_gral" Text="Comentarios"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="textComentario" runat="server" Height="70px" Width="493px" CssClass="txt_gral"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style9">
                            &nbsp;
                        </td>
                        <td class="style6">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="style8" align="center" colspan="3">
                            <asp:Button ID="btnGuardar" runat="server" CssClass="botones" Height="28" onmouseout="style.backgroundColor='#696969'"
                                onmouseover="style.backgroundColor='#A9A9A9'" Text="Guardar" Width="93px" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCancelarNuevo" runat="server" CssClass="botones" Height="28" onmouseout="style.backgroundColor='#696969'"
                                onmouseover="style.backgroundColor='#A9A9A9'" Text="Cancelar" Width="93px" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlVincularDocsSICOD" Width="100%" Style="margin: 0 auto"
                GroupingText="Vincular Documentos SICOD">
                <table align="center" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblNoHayDocsSICOD" runat="server" CssClass="txt_gral" Style="display: none;
                                padding: 15px;">No hay documentos SICOD vinculados</asp:Label>
                            <asp:GridView ID="gvVincularDocsSICOD" runat="server" ForeColor="#555555" Font-Size="7.5pt"
                                CellPadding="1" BackColor="#EEEEEE" AutoGenerateColumns="False" Font-Names="Arial"
                                HorizontalAlign="Center" DataKeyNames="ID_FOLIO, DESTINATARIO, ORIGINAL_FLAG, USUARIO, NOMBRE_ARCHIVO, ID_ARCHIVO, DSC_T_DOC"
                                Font-Name="Arial" PageSize="15" BorderColor="White" AllowSorting="false" Width="98%">
                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="NotSet" CssClass="GridViewHeaderOficios"
                                    ForeColor="White" Font-Size="7.5pt"></HeaderStyle>
                                <RowStyle Wrap="true" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Folio" ItemStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="White"
                                        HeaderStyle-CssClass="BO_Column">
                                        <HeaderStyle CssClass="BO_Column" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblFolio" ForeColor="Blue" ToolTip="Ver Folio" runat="server"
                                                HeaderStyle-ForeColor="White" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                CommandName="IraFolio" Text='<%# Bind("ID_FOLIO") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FECH_RECEPCION" ReadOnly="True" HeaderText="Fecha Rec. Doc."
                                        DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-ForeColor="White" HeaderStyle-CssClass="BO_Column">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DSC_REFERENCIA" HeaderStyle-ForeColor="White" HeaderText="Referencia"
                                        ReadOnly="true" HeaderStyle-CssClass="BO_Column">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DSC_NUM_OFICIO" HeaderStyle-ForeColor="White" HeaderText="Oficio"
                                        ReadOnly="true" HeaderStyle-CssClass="BO_Column">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Tipo de Documento" HeaderStyle-ForeColor="White" HeaderStyle-CssClass="BO_Column">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblTipoDoc" runat="server" HeaderStyle-ForeColor="White" Text='<%# Bind("DSC_T_DOC") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remitente" HeaderStyle-ForeColor="White" HeaderStyle-CssClass="BO_Column">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemitente" runat="server" HeaderStyle-ForeColor="White" Text='<%# Bind("DSC_REMITENTE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Es Copia" HeaderStyle-ForeColor="White" HeaderStyle-CssClass="BO_Column">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Image ID="imgD" runat="server" ImageUrl='<%# ImagenEstatusCopia(DataBinder.Eval(Container.DataItem,"DESTINATARIO"), DataBinder.Eval(Container.DataItem,"BLOQ_TURNADO"))%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asunto" HeaderStyle-ForeColor="White" HeaderStyle-CssClass="BO_Column"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblAsunto" runat="server" HeaderStyle-ForeColor="White" Text='<%# Bind("DSC_ASUNTO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nombre Archivo" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-ForeColor="White" HeaderStyle-CssClass="BO_Column">
                                        <HeaderStyle CssClass="BO_Column" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblArchivoSICOD" ForeColor="Blue" ToolTip="Abrir Archivo PDF"
                                                runat="server" HeaderStyle-ForeColor="White" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                CommandName="verPDFFolio" Text='<%# Bind("NOMBRE_ARCHIVO") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NOMBRE" ReadOnly="True" HeaderStyle-CssClass="BO_Column"
                                        HeaderStyle-ForeColor="White" HeaderText="Responsable">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Estatus/DxV" HeaderStyle-ForeColor="White" HeaderStyle-CssClass="BO_Column">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Image ID="imgEstatus" runat="server" Width="15px" Height="15px" ImageUrl='<%# ImagenFechaLimite(DataBinder.Eval(Container.DataItem, "ATENDIDA"))  %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ARCHIVO_SBM" HeaderStyle-ForeColor="White" HeaderText="Archivo SBM">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CORREO_SIE" HeaderStyle-ForeColor="White" HeaderText="Correo SIE">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:ButtonField Text="Eliminar" CommandName="Eliminar" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table style="height: 40px; width: 100%;">
                    <tr>
                        <td align="right" valign="middle">
                            <asp:Button ID="btnAgregarDocSICOD" runat="server" CssClass="botones" Height="22px"
                                onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                Text="Agregar" Width="130px" OnClientClick="return LevantaVentanaFolio()" />
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table style="width: 100%; margin: 0 auto;" id="tbl_footer" runat="server">
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" Style="padding: 8px;" CssClass="txt_gral" GroupingText="Es Copia">
                            <div style="padding: 8px">
                                <asp:Image ID="imgOriginal" ImageUrl="~/imagenes/original.gif" runat="server" /><asp:Label
                                    ID="lblOriginal" Text=" = Original" CssClass="txt_gral" runat="server"></asp:Label>
                                &nbsp;<asp:Image ID="imgCopia" ImageUrl="~/imagenes/copia.gif" runat="server" /><asp:Label
                                    ID="lblCopia" Text=" = Copia" CssClass="txt_gral" runat="server"></asp:Label>
                                &nbsp;<asp:Image ID="Image2" ImageUrl="~/imagenes/TemplateTab2.gif" runat="server" /><asp:Label
                                    ID="Label4" Text=" = Turnado Original" CssClass="txt_gral" runat="server"></asp:Label>
                                <asp:Image ID="Image8" ImageUrl="~/imagenes/TemplateTab1.gif" runat="server" /><asp:Label
                                    ID="Label5" Text=" = Turnado Copia" CssClass="txt_gral" runat="server"></asp:Label>
                            </div>
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="Panel3" runat="server" Style="padding: 8px;" CssClass="txt_gral" GroupingText="Estatus">
                            <div style="padding: 8px">
                                <asp:Image ID="Image1" runat="server" Height="16px" ImageAlign="AbsBottom" ImageUrl="~/imagenes/statusNormal.png"
                                    Width="16px" />
                                <asp:Label ID="Label1" runat="server" CssClass="txt_gral">= Normal</asp:Label>
                                &nbsp;<asp:Image ID="Image91" runat="server" Height="16px" ImageAlign="AbsBottom"
                                    ImageUrl="~/imagenes/tramite.png" Width="16px" />
                                <asp:Label ID="Label21" runat="server" CssClass="txt_gral">= En trámite</asp:Label>
                                &nbsp;<asp:Image ID="lblEsperaVobo" runat="server" Height="16px" ImageAlign="AbsBottom"
                                    ImageUrl="~/imagenes/question.png" Width="16px" />
                                <asp:Label ID="Label22" runat="server" CssClass="txt_gral">= Espera Vo. Bo.</asp:Label>
                                &nbsp;<asp:Image ID="lblComplemento" runat="server" Height="16px" ImageAlign="AbsBottom"
                                    ImageUrl="~/imagenes/COMPLEMENTO.png" Width="16px" />
                                <asp:Label ID="Label3" runat="server" CssClass="txt_gral">= Complemento</asp:Label>
                                &nbsp;<asp:Image ID="Image6" runat="server" Height="16px" ImageAlign="AbsBottom"
                                    ImageUrl="~/imagenes/PREVENTIVO.png" Width="16px" />
                                <asp:Label ID="Label6" runat="server" CssClass="txt_gral">= Por vencer</asp:Label>
                                &nbsp;<asp:Image ID="Image7" runat="server" Height="16px" ImageUrl="~/imagenes/VENCIDO.png"
                                    Width="16px" />
                                <asp:Label ID="Label7" runat="server" CssClass="txt_gral">= Vencido</asp:Label>
                                &nbsp;<asp:Image ID="Image9" runat="server" Height="16px" ImageAlign="AbsBottom"
                                    ImageUrl="~/imagenes/ATENDIDO.png" Width="16px" />
                                <asp:Label ID="lblAtendido" runat="server" CssClass="txt_gral">= Atendido</asp:Label>
                                &nbsp;<asp:Image ID="Image10" runat="server" Height="16px" ImageAlign="AbsBottom"
                                    ImageUrl="~/imagenes/PROCESO.png" Width="16px" />
                                <asp:Label ID="lblEnProceso" runat="server" CssClass="txt_gral">= En Proceso</asp:Label>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlSeguimiento" runat="server" GroupingText="Vincular archivos" Width="100%"
                Style="margin: 0 auto">
                <table width="100%" align="center">
                    <tr>
                        <td>
                            <asp:Label ID="lblNoHayComentarios" runat="server" CssClass="txt_gral" Style="display: none;
                                padding: 15px;">No hay documentos relacionados</asp:Label>
                            <asp:GridView ID="dgComentarioSeguimiento" runat="server" ForeColor="#555555" Font-Size="7.5pt"
                                CellPadding="1" BackColor="#EEEEEE" AutoGenerateColumns="False" Font-Names="Arial"
                                HorizontalAlign="Center" Style="margin-top: 0px" Width="98%" DataKeyNames="ID_SEGUIMIENTO_OFICIO">
                                <HeaderStyle CssClass="GridViewHeaderOficios" ForeColor="White" Font-Size="7.5pt">
                                </HeaderStyle>
                                <Columns>
                                    <asp:BoundField DataField="ID_SEGUIMIENTO_OFICIO" HeaderStyle-CssClass="BO_Column"
                                        HeaderText="ID" HeaderStyle-ForeColor="White" Visible="false"></asp:BoundField>
                                    <asp:BoundField DataField="F_SEGUIMIENTO" HeaderText="Fecha" HeaderStyle-ForeColor="White"
                                        HeaderStyle-CssClass="BO_Column" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:BoundField DataField="T_SEGUIMIENTO" HeaderText="Comentarios" HeaderStyle-ForeColor="White"
                                        HeaderStyle-CssClass="BO_Column" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:BoundField DataField="T_OFICIO_PROCEDENTE" HeaderText="Oficio Procedente" HeaderStyle-ForeColor="White"
                                        HeaderStyle-CssClass="BO_Column" ItemStyle-HorizontalAlign="Center" Visible="false">
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Oficio Procedente" HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-CssClass="BO_Column">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkOficioProcedente" runat="server" CommandName="OpenDoc" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"T_OFICIO_PROCEDENTE") %>'
                                                Text='<%#DataBinder.Eval(Container.DataItem,"T_OFICIO_PROCEDENTE") %>' OnClick="AbrirDocumento_Click"
                                                ForeColor="Blue"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField runat="server" CommandName="Eliminar" Text="Eliminar" ItemStyle-VerticalAlign="Middle"
                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnNuevoComentario" runat="server" CssClass="botones" Height="22"
                                Width="130px" onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                Text="Agregar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlBotones" Width="830px" Style="margin: 0 auto">
                <table width="818px" align="center">
                    <tr>
                        <td style="text-align: center">
                            <asp:Button ID="btnCancelar" runat="server" CssClass="botones" Height="22" Width="130px"
                                onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                Text="Regresar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlModal" runat="server" Width="830px" Style="margin: 0 auto">
                <table align="center" width="818px">
                    <tr>
                        <td>
                            <asp:LinkButton ID="LB1" runat="server" Text="Popup" Style="display: none"></asp:LinkButton>
                            <asp:Panel ID="PanelErrores" runat="server" CssClass="PanelErrores" Width="550px"
                                Style="display: none">
                                <table style="width: 100%; border-style: solid; border-width: 2px; border-color: White"
                                    cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PanelErroresHandle" runat="server" CssClass="PanelErroresHandle" Width="100%"
                                                Height="20px">
                                                <asp:Label runat="server" ID="lblErroresTitulo" Text="La información presenta errores"
                                                    Style="vertical-align: middle; margin-left: 5px;" CssClass="titulo_seccioninterior titulo_popup"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblErroresPopup" CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; height: 35px">
                                            <center>
                                                <asp:Button ID="BtnModalOk" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                                    onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Aceptar"
                                                    CssClass="botones" />
                                                <asp:Button ID="BtnCancelarModal" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                                    onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Cancelar"
                                                    CssClass="botones" Visible="false" />
                                                <asp:Label ID="lblModalPostBack" runat="server" Style="display: none"></asp:Label>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:ModalPopupExtender ID="ModalPopupExtenderErrores" runat="server" PopupControlID="PanelErrores"
                                TargetControlID="LB1" BackgroundCssClass="FondoAplicacion" PopupDragHandleControlID="PanelErroresHandle">
                            </asp:ModalPopupExtender>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            </div>
            <asp:HiddenField runat="server" ID="hdnExpediente" Value="0" />
            <asp:LinkButton ID="lnkVentanaOficio" runat="server" Text="" Style="display: none"></asp:LinkButton>
            <asp:LinkButton ID="lnkVentanaFolio" runat="server" Text="" Style="display: none"></asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
