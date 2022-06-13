<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VincularOficios.aspx.vb"
    Inherits="SICOD.VincularOficios" %>

<%@ Register Src="~/UserControls/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" src="/Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
</head>
<body>
    <div id="Container">
        <form id="form1" runat="server">
        <asp:ScriptManager ID="scrip1" runat="server" EnableScriptGlobalization="true">
        </asp:ScriptManager>
        <table width="100%" align="center">
            <tr>
                <td style="height: 20px; padding: 15px;" align="center">
                    <asp:Label ID="lblTitulo" class="TitulosWebProyectos" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td id="filtroOficiosWrapper">
                    <table width="100%" style="background-color: #7FA6A1">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <uc1:ucFiltro ID="ucFiltro1" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" style="background-color: #7FA6A1">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ToolTip="Oficios elaborados, registrados, firmados o rubricados por mi"
                                                ID="chkSoloMios" CssClass="txt_gral txt_blanco" Checked="false" Text="Sólo míos"
                                                runat="server" />
                                        </td>
                                        <td align="left">
                                            <asp:RadioButtonList ID="rblEstatusOficio" runat="server" CssClass="txt_gral"
                                                Style="color: #FFF;" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Pendientes" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Terminados/Cancelados" Value="2" Selected="False"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnFiltrar" runat="server" CssClass="botones" Height="22px" onmouseout="style.backgroundColor='#696969'"
                                                OnClientClick="BotonFiltrar()" UseSubmitBehavior="false" onmouseover="style.backgroundColor='#A9A9A9'"
                                                Text="Filtrar" Width="130"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table align="center" width="100%">
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td align="center" valign="top">
                    <asp:UpdatePanel ID="upGridView" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSeleccionar" />
                            <asp:AsyncPostBackTrigger ControlID="btnFiltrar" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="Imagen_procesando" runat="server" style="display: none; text-align: center;
                                margin: 0 auto; width: 100%">
                                <img src="/imagenes/carga.gif" alt="Imagen_de_carga" />
                            </div>
                            <asp:Panel Height="100" ID="pnlImagenNoExisten" runat="server" style="display:none">
                                <br />
                                <br />
                                <asp:Image ID="Image18" runat="server" ImageUrl="../imagenes/No Existen.gif"></asp:Image>
                            </asp:Panel>
                            <div id="GRID" runat="server" style="overflow: auto; height: 300px; width: 100%;
                                position: relative;">
                                <asp:GridView ID="gvVincularOficios" runat="server" ForeColor="#555555" Font-Size="7.5pt"
                                    CellPadding="1" BackColor="#EEEEEE" AutoGenerateColumns="False" Font-Names="Arial"
                                    HorizontalAlign="Center" Font-Name="Arial" PageSize="15" BorderColor="White"
                                    AllowSorting="false" DataKeyNames="ID_UNIDAD_ADM,ID_ANIO,I_OFICIO_CONSECUTIVO,ID_TIPO_DOCUMENTO, T_HYP_ARCHIVOSCAN">
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="NotSet" CssClass="GridViewHeaderOficios"
                                        ForeColor="White" Wrap="false"></HeaderStyle>
                                    <RowStyle Wrap="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Seleccionar">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chSeleccionado" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="BO_Column" ForeColor="White" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Oficio" ItemStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="White"
                                            HeaderStyle-CssClass="BO_Column">
                                            <HeaderStyle CssClass="BO_Column" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblNumOficio" ForeColor="Blue" ToolTip="Abrir Archivo PDF" runat="server"
                                                    HeaderStyle-ForeColor="White" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                    CommandName="VerPDF" Text='<%# Bind("T_OFICIO_NUMERO") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Asunto" DataField="T_ASUNTO" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-CssClass="BO_Column txt_blanco"></asp:BoundField>
                                        <asp:BoundField HeaderText="Elaboró" DataField="ELABORO" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-CssClass="BO_Column txt_blanco"></asp:BoundField>
                                        <asp:BoundField HeaderText="Registró" DataField="REGISTRO" ItemStyle-HorizontalAlign="center"
                                            HeaderStyle-CssClass="BO_Column txt_blanco"></asp:BoundField>
                                        <asp:BoundField HeaderText="Entidad" DataField="T_ENTIDAD_CORTO" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-CssClass="BO_Column txt_blanco"></asp:BoundField>
                                        <asp:BoundField HeaderText="Tipo de Documento" DataField="T_TIPO_DOCUMENTO" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-CssClass="BO_Column txt_blanco"></asp:BoundField>
                                        <asp:BoundField HeaderText="Destinatario" DataField="DESTINATARIO" SortExpression="DESTINATARIO"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column txt_blanco">
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Estatus" DataField="T_ESTATUS" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-CssClass="BO_Column txt_blanco"></asp:BoundField>
                                        <asp:BoundField HeaderText="Fecha de Documento" DataField="F_FECHA_OFICIO" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-CssClass="BO_Column txt_blanco"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Vencimiento" HeaderStyle-CssClass="BO_Column txt_blanco" Visible="false">
                                            <ItemTemplate>
                                                <asp:Image Style="height: 16px; width: 16px" ID="imgVencimiento" runat="server" ImageUrl="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Clasificación" DataField="T_CLASIFICACION" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-CssClass="BO_Column txt_blanco"></asp:BoundField>
                                        <asp:TemplateField HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hf_ID_ANIO" Value='<%# Eval("ID_ANIO") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hf_ID_UNIDAD_ADM" Value='<%# Eval("ID_UNIDAD_ADM") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hf_ID_TIPO_DOCUMENTO" Value='<%# Eval("ID_TIPO_DOCUMENTO") %>'
                                                    runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hf_I_OFICIO_CONSECUTIVO" Value='<%# Eval("I_OFICIO_CONSECUTIVO") %>'
                                                    runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div>
                                <table style="width: 100%;">
                                    <tr>
                                        <td align="right" valign="middle">
                                            <asp:Button ID="btnSeleccionar" UseSubmitBehavior="false" runat="server" CssClass="botones"
                                                Height="22px" onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                                Text="Vincular Seleccionados" Width="130px" Style="float: right" />
                                            <asp:Button ID="btnRegresar" UseSubmitBehavior="false" runat="server" CssClass="botones"
                                                Height="22px" onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                                Text="Regresar" Width="130px" Style="float: left" />
                                            <div style="clear: both">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Panel ID="pnlFooter" runat="server" Style="padding: 8px; display:none" CssClass="txt_gral"
                                                GroupingText="Vencimiento">
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
                                                    &nbsp;<asp:Image ID="Image1" runat="server" Height="22px" ImageUrl="~/imagenes/ATENDIDO.png"
                                                        Width="22px" />
                                                    <asp:Label ID="Label1" runat="server" CssClass="txt_gral">= Terminado</asp:Label>
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="right">
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:LinkButton ID="LB1" runat="server" Text="Popup" Style="display: none">
                </asp:LinkButton>
                <asp:Panel ID="PanelErrores" runat="server" CssClass="PanelErrores" Width="550px"
                    Style="display: none">
                    <table style="width: 100%; border-style: solid; border-width: 2px; border-color: White"
                        cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <asp:Panel ID="PanelErroresHandle" runat="server" CssClass="PanelErroresHandle" Width="100%"
                                    Height="20px">
                                    <asp:Label runat="server" ID="lblErroresTitulo" Text="" Style="vertical-align: middle;
                                        margin-left: 5px;" CssClass="titulo_seccioninterior titulo_popup"></asp:Label>
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
                                        CssClass="botones" OnClientClick="getFlickerSolved()" />
                                    <asp:Button ID="BtnContinua" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                        onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Continuar"
                                        CssClass="botones" Visible="false" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:ModalPopupExtender ID="ModalPopupExtenderErrores" runat="server" PopupControlID="PanelErrores"
                    TargetControlID="LB1" BackgroundCssClass="FondoAplicacion" DropShadow="false"
                    PopupDragHandleControlID="PanelErroresHandle" OkControlID="BtnModalOk" OnCancelScript="getFlickerSolved()">
                </asp:ModalPopupExtender>
                <asp:Label runat="server" ID="lblErrores" CssClass="txt_gral_rojo" Visible="false"
                    Style="display: none"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <script language="javascript" src="~/Scripts/wait.js" type="text/javascript"></script>
        <!-- Mostrar reloj de arena en la página cuando haya postbacks asíncronos -->
        <script language="javascript" type="text/javascript">
            //AGC Validar que al presionar la tecla "Backspace"
            document.onkeydown = function () {
                activeObj = document.activeElement;
                if (activeObj.tagName != "INPUT" && activeObj.tagName != "TEXTAREA") {
                    if (event.keyCode === 8) {
                        return false;
                    }
                }
            }

            function getFlickerSolved() {
                jQuery('<%= PanelErrores.ClientID %>').css("display", "none");
            }


            $(document).ready(function () {
                $('#GRID').scroll(function () {
                    var scroll = $("#GRID").scrollTop();
                    $("#GRID th").css("top", scroll);
                });
                CallClickINICIO();

            });

            function CallClickINICIO() {
                try {
                    //WORK IN ALL BROWSERS
                    try {
                        //$get('<%= btnFiltrar.ClientID %>').click();
                    } catch (e) { }

                    return false;
                }
                catch (e) { }
            }

            function BotonFiltrar() {
                jQuery("#GRID").css("display", "none");
                jQuery("#Imagen_procesando").css("display", "")
                jQuery("#Imagen_procesando").css("height", "300px");
                jQuery("#pnlFooter").css("display", "none");
                jQuery("#pnlImagenNoExisten").css("display", "none");

            }

            var prm2 = Sys.WebForms.PageRequestManager.getInstance();
            prm2.add_endRequest(EndRequestInline);
            function EndRequestInline(sender, args) {
                $('#GRID').scroll(function () {
                    var scroll = $("#GRID").scrollTop();
                    $("#GRID th").css("top", scroll);
                });
            }

        </script>
        </form>
    </div>
</body>
</html>
