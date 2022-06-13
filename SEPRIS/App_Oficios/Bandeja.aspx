<%@ Page Language="vb" Culture="es-MX" UICulture="es" AutoEventWireup="false" CodeBehind="Bandeja.aspx.vb" Inherits="SICOD.BandejaOficios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="PersonalizarColumasOficios.ascx" TagName="PersonalizarColumas" TagPrefix="cc1" %>
<%@ Register src="~/App_Oficios/UserControls/ucFiltroOficios.ascx" tagname="ucFiltroOficios" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" src="/Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
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
        function GetScrollGrid() {

            document.getElementById('hdnScrollPosition').innerText = $get('GRID').scrollTop;
            return true;

        }


        function SetScrollGrid(position) {

            $('#GRID').scrollTop(position);

        }

        function BotonFiltrar() {
            $("#GRID").css("display", "none");
            $("#Imagen_procesando").css("display", "")
            $("#Imagen_procesando").css("height", "300px");
            $("#tbl_footer").css("display", "none");
            $("#pnlImagenNoExisten").css("display", "none");
        }

        function CallClickINICIO() {
            try {
                //WORK IN ALL BROWSERS
                try {

                    document.getElementById('hdn_width').value = document.body.clientWidth;
                    //document.getElementById('btnFiltrar').click();
                    document.getElementById('btnCargaGRID').click();
                    //btnCargaGRID

                } catch (e) { }

                return false;
            }
            catch (e) { }
        }

    </script>
</head>
<body>
    <div id="Container">
        <form id="form1" runat="server">
        <asp:ScriptManager ID="scrip1" runat="server" EnableScriptGlobalization="true" AsyncPostBackTimeout="600">
        </asp:ScriptManager>
        <asp:HiddenField runat="server" ID="hdnScrollPosition" Value="0" ClientIDMode="Static" />
        <table width="100%" align="center">
            <tr>
                <td>
                    <p class="TitulosWebProyectos" style="text-align: center; width: 90%" align="center">
                        Bandeja de Oficios
                    </p>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upPersonaliza" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExportarExcelTop" />
                        </Triggers>
                        <ContentTemplate>
                            <div>
                                <asp:Panel ID="PanelPersonaliza" runat="server" Style="display: none">
                                    <table>
                                        <tr>
                                            <td id="td_modalPersonalizarColumnas">
                                                <cc1:PersonalizarColumas ID="ModalPersonalizarColumas" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--by guillermo.banda. Boton para cerrar la modal Personalizado--%>
                                                <button id="btnCerrarModal" runat="server" style="display: none">
                                                    Cerrar Modal Personalizar</button>
                                                <%--Boton para actualizar el Grid al cerrar la modal --%>
                                                <asp:Button ID="btnAcualizaGridPersonalizado" runat="server" Text="Actualiza Grid"
                                                    Style="display: none" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:ModalPopupExtender ID="ModalPersonaliza" runat="server" PopupControlID="PanelPersonaliza"
                                        TargetControlID="BtnPersonalizar" BackgroundCssClass="FondoAplicacion" PopupDragHandleControlID="PanelErroresHandle"
                                        CancelControlID="btnCerrarModal">
                                    </asp:ModalPopupExtender>
                                </asp:Panel>
                            </div>
                            <asp:Button ID="BtnPersonalizar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                                runat="server" Height="22px" ToolTip="Personalizar Columnas" Text="Personalizar Columnas"
                                CssClass="botones" Width="130" Style="float: left;"></asp:Button>
                            <asp:Button ID="btnExportarExcelTop" runat="server" CssClass="botones" Height="22px"
                                onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                Text="Exportar Grid a Excel" Width="130px" Style="float: right; visibility: hidden" />
                            <div style="clear: both">
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td id="filtroOficiosWrapper" runat="server">
                    <table width="100%" class="FiltroBandeja">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                    <ContentTemplate>
                                  
                                        <uc1:ucFiltroOficios ID="ucFiltro1" runat="server" />
                                  
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="818px">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ToolTip="Oficios elaborados, registrados, firmados o rubricados por mi"
                                                ID="chkSoloMios" CssClass="txt_gral txt_blanco" Text="Sólo míos" Checked="false"
                                                runat="server" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblEstatusOficio" runat="server" CssClass="txt_gral" Style="color: #FFF;"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Pendientes" Value="1" title="En elaboración, En Revisión, En Firma, Notificado - Falta Acuse, Se espera información, Se esp. inf. Falta Acuse, Por dictaminar, Dictaminado, Para ensobretar, Para notificar"
                                                    Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Terminados/Cancelados" Value="2" Selected="False"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <span class="txt_gral" style="color: #FFF">Ver</span>
                                            <asp:DropDownList runat="server" ID="ddlVerUltimos">
                                                <asp:ListItem Selected="True" Text="" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Todos" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnFiltrar" runat="server" CssClass="botones" Height="22px" onmouseout="style.backgroundColor='#696969'"
                                                OnClientClick="BotonFiltrar()" UseSubmitBehavior="false" onmouseover="style.backgroundColor='#A9A9A9'"
                                                Text="Filtrar" Width="130"></asp:Button>
                                            <asp:Button runat="server" ID="btnCargaGRID" Style="display: none;" UseSubmitBehavior="false"
                                                OnClientClick="BotonFiltrar();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="upGridView" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:AsyncPostBackTrigger ControlID="btnFiltrar" />
                <asp:AsyncPostBackTrigger ControlID="gvBandejaOficios" />
                <asp:AsyncPostBackTrigger ControlID="BtnModalOk" />
                <asp:AsyncPostBackTrigger ControlID="btnCargaGRID" />
            </Triggers>
            <ContentTemplate>
                <table align="center" border="0" width="100%">
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <div id="GRID" runat="server" style="overflow: auto; height: 300px; position: relative;
                                vertical-align: top; text-align: center;">
                                <asp:GridView ID="gvBandejaOficios" runat="server" ForeColor="#555555" Font-Size="7.5pt"
                                    CellPadding="1" BackColor="#EEEEEE" AutoGenerateColumns="False" Font-Names="Arial"
                                    HorizontalAlign="Center" Font-Name="Arial" PageSize="15" BorderColor="White"
                                    AllowSorting="True" DataKeyNames="ID_UNIDAD_ADM, ID_ANIO, I_OFICIO_CONSECUTIVO, ID_TIPO_DOCUMENTO, T_ENTIDAD_CORTO, DSC_UNIDAD_ADM"
                                    OnRowCommand="gvBandejaOficios_RowCommand" SelectedRowStyle-BackColor="#7FA6A1"
                                    SelectedRowStyle-Font-Bold="true">
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="NotSet" CssClass="GridViewHeaderOficios"
                                        ForeColor="White" Wrap="false"></HeaderStyle>
                                    <RowStyle Wrap="true" />
                                    <AlternatingRowStyle CssClass="tr_odd" />
                                    <Columns>
                                        <asp:ButtonField Text="O" CommandName="Edit" ItemStyle-HorizontalAlign="Center" ButtonType="Link"
                                            ControlStyle-ForeColor="DarkBlue"></asp:ButtonField>
                                        <asp:BoundField HeaderText="Area" DataField="DSC_UNIDAD_ADM" SortExpression="DSC_UNIDAD_ADM"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundField>
                                        <%--<asp:BoundField HeaderText="Oficio" DataField="T_OFICIO_NUMERO" SortExpression="T_OFICIO_NUMERO"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Oficio" SortExpression="T_OFICIO_NUMERO">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="BO_Column" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkHistorial" runat="server" Text='<%# Bind("T_OFICIO_NUMERO") %>'
                                                    OnClientClick="return ShowBitacora(this);" ToolTip="Ver Historial" ForeColor="#555555"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Asunto" DataField="T_ASUNTO" SortExpression="T_ASUNTO"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="BO_Column"></asp:BoundField>
                                        <asp:BoundField HeaderText="Elaboró" DataField="ELABORO" SortExpression="ELABORO"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="BO_Column"></asp:BoundField>
                                        <asp:BoundField HeaderText="Registró" DataField="REGISTRO" SortExpression="REGISTRO"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="BO_Column"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"
                                            HeaderStyle-ForeColor="White" HeaderText="Destinatario">
                                            <ItemTemplate>
                                                <asp:Image ID="logoImg" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Entidad" DataField="T_ENTIDAD_CORTO" SortExpression="T_ENTIDAD_CORTO"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundField>
                                        <asp:BoundField HeaderText="Año" DataField="ID_ANIO" HeaderStyle-CssClass="BO_Column"
                                            SortExpression="ID_ANIO" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                        <asp:BoundField HeaderText="Tipo de Documento" DataField="T_TIPO_DOCUMENTO" SortExpression="T_TIPO_DOCUMENTO"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" SortExpression="DESTINATARIO"
                                            HeaderStyle-CssClass="BO_Column" HeaderStyle-ForeColor="White" HeaderText="Destinatario">
                                            <ItemTemplate>
                                                <asp:Label ID="destinatario" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Fecha de Recepción" DataField="F_FECHA_RECEPCION" SortExpression="F_FECHA_RECEPCION"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundField>
                                        <asp:BoundField HeaderText="Fecha de Documento" DataField="F_FECHA_OFICIO" SortExpression="F_FECHA_OFICIO"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundField>
                                        <asp:BoundField HeaderText="Fecha de Acuse" DataField="F_FECHA_ACUSE" SortExpression="F_FECHA_ACUSE"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column"></asp:BoundField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/imagenes/pdficon_large.png" CausesValidation="false"
                                            CommandName="Cedula" HeaderText="Generar Cédula PDF" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="btn_notificar" HeaderStyle-ForeColor="White">
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/imagenes/notificar_pendiente.png"
                                            CausesValidation="false" CommandName="Notificar" HeaderText="Notificar" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="btn_notificar" HeaderStyle-ForeColor="White">
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/imagenes/fup2.png" CausesValidation="false"
                                            CommandName="T_HYP_ARCHIVOWORD" SortExpression="T_HYP_ARCHIVOWORD" HeaderText="Oficio WORD"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document">
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/imagenes/fup2.png" CausesValidation="false"
                                            CommandName="T_HYP_ARCHIVOSCAN" SortExpression="T_HYP_ARCHIVOSCAN" HeaderText="Oficio Firmado PDF"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document">
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/imagenes/fup2.png" CausesValidation="false"
                                            CommandName="T_HYP_CEDULAPDF" SortExpression="T_HYP_CEDULAPDF" HeaderText="Cédula PDF"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document">
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/imagenes/fup2.png" CausesValidation="false"
                                            CommandName="T_CEDULADIGITAL" SortExpression="T_CEDULADIGITAL" HeaderText="Cédula SBM"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document">
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/imagenes/fup2.png" CausesValidation="false"
                                            CommandName="T_HYP_FIRMADIGITAL" SortExpression="T_HYP_FIRMADIGITAL" HeaderText="Oficio Firmado SBM"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document">
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/imagenes/fup2.png" CausesValidation="false"
                                            CommandName="T_HYP_RESPUESTAOFICIO" SortExpression="T_HYP_RESPUESTAOFICIO" HeaderText="Respuesta_old"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document"
                                            Visible="false"></asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/imagenes/fup2.png" CausesValidation="false"
                                            CommandName="T_HYP_ACUSERESPUESTA" SortExpression="T_HYP_ACUSERESPUESTA" HeaderText="Acuse Notificación PDF"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document">
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/imagenes/fup2.png" CausesValidation="false"
                                            CommandName="T_ANEXO_UNO" SortExpression="T_ANEXO_UNO" HeaderText="Anexos" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document"></asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/imagenes/fup2.png" CausesValidation="false"
                                            CommandName="T_ANEXO_DOS" SortExpression="T_ANEXO_DOS" HeaderText="Anexos SBM"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document">
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/imagenes/fup2.png" CausesValidation="false"
                                            CommandName="T_HYP_EXPEDIENTE" SortExpression="T_HYP_EXPEDIENTE" HeaderText="Expediente"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-CssClass="BO_document">
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Link" CausesValidation="false" CommandName="DICTAMINADO_FLAG"
                                            SortExpression="DICTAMINADO_FLAG" HeaderText="Dictaminado" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-CssClass="BO_Column"></asp:ButtonField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Vencimiento_old"
                                            SortExpression="F_FECHA_VENCIMIENTO" HeaderStyle-CssClass="BO_Column" Visible="false">
                                            <ItemTemplate>
                                                <asp:Image Style="height: 20px; width: 20px" ID="imgVencimiento" runat="server" ImageUrl="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Notificación" SortExpression="NOTIF_ELECTRONICA_FLAG"
                                            HeaderStyle-CssClass="BO_Column">
                                            <ItemTemplate>
                                                <asp:Image ID="imgNotificacionElectronica" runat="server" ToolTip='<%# EstatusNotificacionTooltip(DataBinder.Eval(Container.DataItem,"NOTIF_ELECTRONICA_FLAG")) %>'
                                                    ImageUrl='<%# EstatusNotificacion(DataBinder.Eval(Container.DataItem,"NOTIF_ELECTRONICA_FLAG")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Clasificación" DataField="T_CLASIFICACION" SortExpression="T_CLASIFICACION"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="BO_Column"></asp:BoundField>
                                        <asp:ButtonField ButtonType="Link" CausesValidation="false" Text="DoubleClick" CommandName="DoubleClick"
                                            Visible="false" />
                                        <asp:BoundField HeaderText="Estatus" DataField="T_ESTATUS" SortExpression="T_ESTATUS"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="BO_Column" ItemStyle-Font-Bold="true">
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Panel Height="100" ID="pnlImagenNoExisten" runat="server" Style="display: none">
                                <br />
                                <br />
                                <asp:Image ID="Image18" runat="server" ImageUrl="../imagenes/No Existen.gif"></asp:Image>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="Imagen_procesando" runat="server" style="display: none; text-align: center">
                                <img src="/imagenes/carga.gif" alt="Imagen_de_carga" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table style="width: 100%;" id="tbl_footer" runat="server">
                                <tr>
                                    <td align="center" valign="middle" colspan="2">
                                        <asp:Button ID="btnExcel" runat="server" CssClass="botones" Height="22px" onmouseout="style.backgroundColor='#696969'"
                                            onmouseover="style.backgroundColor='#A9A9A9'" Style="display: none" Text="Exportar Grid a Excel"
                                            Width="130px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" style="display: none">
                                        <asp:Panel ID="pnlVencimiento" runat="server" Style="padding: 8px; display: none"
                                            CssClass="txt_gral" GroupingText="Vencimiento">
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
                                    <td align="left" valign="top">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" style="width: 160px">
                                        <asp:Panel ID="Panel2" runat="server" Style="display: none" CssClass="txt_gral" GroupingText="Notificación Electrónica">
                                            <div>
                                                <div style="padding: 5px; float: left; width: 115px">
                                                    <asp:Image ID="Image14" runat="server" ImageAlign="AbsBottom" ImageUrl="~/imagenes/OK.gif" />
                                                    <asp:Label ID="Label11" runat="server" CssClass="txt_gral">= Notificado</asp:Label>
                                                </div>
                                                <div style="padding: 5px; float: left; width: 115px">
                                                    <asp:Image ID="Image15" runat="server" ImageAlign="AbsBottom" ImageUrl="~/imagenes/ERROR.gif" />
                                                    <asp:Label ID="Label12" runat="server" CssClass="txt_gral">= Pendiente</asp:Label>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                    <td align="left" valign="top" style="width: 680px">
                                        <asp:Panel ID="Panel1" runat="server" Style="padding: 0px 8px 8px 8px; display: none"
                                            CssClass="txt_gral" GroupingText="Archivos Adjuntos">
                                            <div>
                                                <div style="padding: 5px; float: left; width: 115px">
                                                    <asp:Image ID="Image16" runat="server" ImageAlign="AbsBottom" ImageUrl="~/imagenes/fup4.png" />
                                                    <asp:Label ID="Label13" runat="server" CssClass="txt_gral">= Subir Archivo</asp:Label></div>
                                                <div style="padding: 5px; float: left; width: 115px">
                                                    <asp:Image ID="Image1" runat="server" ImageAlign="AbsBottom" ImageUrl="~/imagenes/archivo-word.gif" />
                                                    <asp:Label ID="Label1" runat="server" CssClass="txt_gral">= Archivo Word</asp:Label></div>
                                                <div style="padding: 5px; float: left; width: 115px">
                                                    <asp:Image ID="Image2" runat="server" ImageAlign="AbsBottom" ImageUrl="~/imagenes/archivo-pdf.gif" />
                                                    <asp:Label ID="Label2" runat="server" CssClass="txt_gral">= Archivo PDF</asp:Label></div>
                                                <div style="padding: 5px; float: left; width: 115px">
                                                    <asp:Image ID="Image7" runat="server" ImageAlign="AbsBottom" ImageUrl="~/imagenes/archivo-cne.gif" />
                                                    <asp:Label ID="Label4" runat="server" CssClass="txt_gral">= Cédula Digital</asp:Label></div>
                                                <div style="padding: 5px; float: left; width: 115px">
                                                    <asp:Image ID="Image6" runat="server" ImageAlign="AbsBottom" ImageUrl="~/imagenes/archivo-cedula.gif" />
                                                    <asp:Label ID="Label3" runat="server" CssClass="txt_gral">= Cédula PDF</asp:Label></div>
                                                <div style="padding: 5px; float: left; width: 115px">
                                                    <asp:Image ID="Image8" runat="server" ImageAlign="AbsBottom" ImageUrl="~/imagenes/archivo-firma.gif" />
                                                    <asp:Label ID="Label5" runat="server" CssClass="txt_gral">= Firma Digital</asp:Label></div>
                                                <div style="padding: 5px; float: left; width: 115px">
                                                    <asp:Image ID="Image9" runat="server" ImageAlign="AbsBottom" ImageUrl="~/imagenes/archivo-respuesta.gif" />
                                                    <asp:Label ID="Label6" runat="server" CssClass="txt_gral">= Respuesta</asp:Label></div>
                                                <div style="padding: 5px; float: left; width: 115px">
                                                    <asp:Image ID="Image10" runat="server" ImageAlign="AbsBottom" ImageUrl="~/imagenes/archivo-acuse.gif" />
                                                    <asp:Label ID="Label7" runat="server" CssClass="txt_gral">= Acuse</asp:Label></div>
                                                <div style="padding: 5px; float: left; width: 115px">
                                                    <asp:Image ID="Image11" runat="server" ImageAlign="AbsBottom" ImageUrl="~/imagenes/archivo-anexo1.gif" />
                                                    <asp:Label ID="Label8" runat="server" CssClass="txt_gral">= Anexos</asp:Label></div>
                                                <div style="padding: 5px; float: left; width: 115px">
                                                    <asp:Image ID="Image12" runat="server" ImageAlign="AbsBottom" ImageUrl="~/imagenes/archivo-anexo2.gif" />
                                                    <asp:Label ID="Label9" runat="server" CssClass="txt_gral">= Anexos SBM</asp:Label></div>
                                                <div style="padding: 5px; float: left; width: 125px">
                                                    <asp:Image ID="Image13" runat="server" ImageAlign="AbsBottom" ImageUrl="~/imagenes/archivo-expediente.gif" />
                                                    <asp:Label ID="Label10" runat="server" CssClass="txt_gral">= Expediente</asp:Label></div>
                                            </div>
                                        </asp:Panel>
                                        <div style="clear: both">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="botones" Visible="true" />
                            <asp:LinkButton ID="lnkProcesa" runat="server" Text="" Style="display: none"></asp:LinkButton>
                            <asp:Panel ID="pnlProcesando" runat="server" Width="400px" Height="200px" CssClass="FondoManualOut">
                                <asp:LinkButton ID="lnkCierra" runat="server" Text="" Style="display: none"></asp:LinkButton>
                                &nbsp;
                                <img src="../imagenes/carga.gif" alt="" />
                            </asp:Panel>
                            <asp:ModalPopupExtender ID="mpeProcesa" runat="server" PopupControlID="pnlProcesando"
                                BackgroundCssClass="FondoAplicacion" TargetControlID="lnkProcesa" CancelControlID="btnCerrarModalCedula">
                            </asp:ModalPopupExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
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
                                    <asp:Label runat="server" ID="lblErroresTitulo" Text="La información presenta errores"
                                        Style="vertical-align: middle; margin-left: 5px;" CssClass="titulo_seccioninterior titulo_popup"></asp:Label>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblErroresPopup" CssClass="txt_gral txt_gral_o_gris"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; height: 35px">
                                <center>
                                    <asp:Button ID="BtnModalOk" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                        onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Aceptar"
                                        CssClass="botones" OnClientClick="MuestraProcesa()" />
                                    <asp:Button ID="BtnCancelarModal" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                        onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Cancelar"
                                        CssClass="hide" />
                                </center>
                                <asp:Label ID="lblModalPostBack" runat="server" Style="display: none"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:ModalPopupExtender ID="ModalPopupExtenderErrores" runat="server" PopupControlID="PanelErrores"
                    TargetControlID="LB1" BackgroundCssClass="FondoAplicacion" PopupDragHandleControlID="PanelErroresHandle"
                    CancelControlID="BtnCancelarModal">
                </asp:ModalPopupExtender>
                <asp:Label runat="server" ID="lblErrores" CssClass="txt_gral_rojo" Visible="false"
                    Style="display: none"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upFechaAcuse" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Calendar1" />
                <asp:AsyncPostBackTrigger ControlID="BtnModalFechaAcuseOk" />
            </Triggers>
            <ContentTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" Text="Popup" Style="display: none">
                </asp:LinkButton>
                <asp:Panel ID="pnlFechaAcuse" runat="server" CssClass="panelVentanaEmergenteBackground"
                    Width="550px" Style="display: none">
                    <table style="width: 100%; border-style: solid; border-width: 2px; border-color: White"
                        cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <asp:Panel ID="Panel4" runat="server" CssClass="panelVentanaEmergenteTitulo" Width="100%"
                                    Height="20px">
                                    <asp:Label runat="server" ID="Label14" Text="Seleccione la fecha del Acuse" Style="vertical-align: middle;
                                        margin-left: 5px;" CssClass="titulo_seccioninterior titulo_popup"></asp:Label>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="padding: 10px 0">
                                <asp:Calendar ID="Calendar1" Width="200px" BackColor="White" DayHeaderStyle-Font-Bold="false"
                                    DayHeaderStyle-ForeColor="#4f4d4e" DayHeaderStyle-Font-Names="Arial, Helvetica, sans-serif"
                                    DayHeaderStyle-Font-Size="11px" DayStyle-Font-Size="11px" DayStyle-Font-Names="tahoma, verdana, helvetica"
                                    DayHeaderStyle-BackColor="White" DayNameFormat="FirstTwoLetters" DayStyle-CssClass="txt_gral  txt_gral_o_gris"
                                    TitleStyle-Font-Bold="true" OtherMonthDayStyle-ForeColor="#4f4d4e" TitleStyle-Font-Size="11px"
                                    TitleStyle-Font-Names="tahoma, verdana, helvetica" Format="dd/MM/yyyy" runat="server">
                                </asp:Calendar>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; height: 35px">
                                <center>
                                    <asp:Button ID="BtnModalFechaAcuseOk" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                        onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Aceptar"
                                        CssClass="botones" />
                                    <asp:Button ID="btnModalFechaAcuseCancel" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                        onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Cancelar"
                                        CssClass="botones" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:ModalPopupExtender ID="modalPopupFechaAcuse" runat="server" PopupControlID="pnlFechaAcuse"
                    TargetControlID="LinkButton1" BackgroundCssClass="FondoAplicacion" PopupDragHandleControlID="Panel4"
                    CancelControlID="btnModalFechaAcuseCancel">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upFileUpload" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnModalFileUploadOK" />
            </Triggers>
            <ContentTemplate>
                <asp:LinkButton ID="lbDummy" runat="server" Text="Popup" Style="display: none">
                </asp:LinkButton>
                <asp:Panel ID="pnlFileUpload" runat="server" CssClass="panelVentanaEmergenteBackground"
                    Width="550px" Style="display: none;">
                    <table style="width: 100%; border-style: solid; border-width: 2px; border-color: White"
                        cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlFileUploadHandle" runat="server" CssClass="panelVentanaEmergenteTitulo"
                                    Width="100%" Height="20px">
                                    <asp:Label ID="fileUploadTitulo" Text="Adjuntar Archivo" Style="padding: 3px; margin-left: 15px"
                                        CssClass="titulo_seccioninterior titulo_popup" runat="server"></asp:Label>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 50px">
                                <asp:Label ID="lblTipoArchivo" Style="margin-left: 15px; padding: 10px;" runat="server"
                                    Text="Adjuntar archivo" CssClass="txt_gral txt_gral_o_gris"></asp:Label>
                                <br />
                                <asp:Label ID="lblExtensionesPermitidas" CssClass="txt_gral txt_gral_o_gris" Style="margin-left: 15px;
                                    padding: 10px;" runat="server" Text="Tipo de archivo"></asp:Label>
                                <asp:Label ID="lblFileUploadError" CssClass="txt_gral txt_gral_o_gris" Style="margin-left: 15px;
                                    padding: 10px;" runat="server" Visible="false" Text="Tipo de archivo"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="width: 400px; text-align: center; height: 35px;">
                                <asp:Image runat="server" ID="imgStatusFileUpload" Visible="false"></asp:Image>
                                <asp:AsyncFileUpload ID="AsyncFileUpload2" runat="server" OnClientUploadComplete="enableModalFileUploadButtonOK"
                                    OnUploadedComplete="AsyncFileUpload2_UploadedComplete" OnClientUploadStarted="verImgCargando" OnClientUploadError="onUploadError"/>
                                <asp:FileUpload Width="400" ID="fuArchivosAdjuntos" ToolTip="Adjuntar Archivo al Oficio"
                                    runat="server" Style="display: none" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                <asp:Image ID="imgCargandoArchivo" runat="server" ImageUrl="./iconoCargando.gif" Width="143px"  Height="70px" Style="display: none" />
                                    </center>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; height: 35px">
                                <center>
                                    <asp:Button ID="btnModalFileUploadOK" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                        onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Aceptar"
                                        CssClass="botones" OnClientClick="getFlickerSolved()" />
                                    <asp:Button ID="btnModalFileUploadCancelar" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                        onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Cancelar"
                                        CssClass="botones" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:ModalPopupExtender ID="modalFileUpload" runat="server" PopupControlID="pnlFileUpload"
                    TargetControlID="lbDummy" BackgroundCssClass="FondoAplicacion" PopupDragHandleControlID="pnlFileUploadHandle"
                    CancelControlID="btnModalFileUploadCancelar">
                </asp:ModalPopupExtender>
                <asp:Label runat="server" ID="tmpArchivo" Style="display: none"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upCedula" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:LinkButton ID="LBCedula" runat="server" Text="Popup" Style="display: none">
                </asp:LinkButton>
                <asp:Panel ID="pnlCedula" runat="server" Width="550px" Style="" CssClass="panelVentanaEmergenteBackground">
                    <table style="width: 100%; border-style: solid; border-width: 2px; border-color: White"
                        cellspacing="0" cellpadding="0">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Panel ID="PanelTitulo" runat="server" CssClass="panelVentanaEmergenteTitulo"
                                    Width="100%" Height="20px">
                                    <asp:Label runat="server" ID="lblTituloModal" Text="Datos para la Cédula Electrónica"
                                        Style="vertical-align: middle; margin-left: 5px;" CssClass="titulo_seccioninterior titulo_popup"></asp:Label>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td style="text-indent: 25px;" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                            </td>
                            <tr>
                                <td style="text-indent: 25px;">
                                    <asp:Label ID="lblNotificador" runat="server" Text="Selecciona el nombre del notificador: "
                                        CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlNotificador" runat="server" Height="16px" Width="220px">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:RequiredFieldValidator ID="rfvNoti" runat="server" ControlToValidate="ddlNotificador"
                                        InitialValue="-1" Text="*" ErrorMessage="Se requiere seleccionar al Notificador"
                                        Display="Dynamic" ForeColor="Red" ValidationGroup="Forma"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <br />
                                </td>
                                <tr>
                                    <td style="text-indent: 25px;">
                                        <asp:Label ID="lblFecha" runat="server" Text="Selecciona la fecha: " CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFechaCedula" runat="server"></asp:TextBox>
                                        <asp:MaskedEditExtender ID="meeFechCedula" runat="server" TargetControlID="txtFechaCedula"
                                            MaskType="Date" Mask="99/99/9999" UserDateFormat="DayMonthYear">
                                        </asp:MaskedEditExtender>
                                        &nbsp;&nbsp;
                                        <asp:Image ImageAlign="Bottom" ID="imgCalendarioCedula" runat="server" ImageUrl="~/imagenes/Calendar.gif" />
                                        <asp:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="txtFechaCedula"
                                            Format="dd/MM/yyyy" PopupButtonID="imgCalendarioCedula">
                                        </asp:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorFechaCedula" runat="server"
                                            ErrorMessage="La Fecha de Cédula debe ser válida con el formato DD/MM/AAAA" ControlToValidate="txtFechaCedula"
                                            Text="*" ValidationGroup="Forma" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                            Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <br />
                                    </td>
                                    <tr>
                                        <td style="text-indent: 25px;">
                                            <asp:Label ID="lblHora" runat="server" Text="Selecciona la hora: " CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnAumentarHora" runat="server" BackColor="#696969" Font-Bold="True"
                                                Text="+" Width="30px" ForeColor="#FFFFFF" OnClientClick="return SetHourMinute('H', '+')" />
                                            <asp:Button ID="btnDisminuirHora" runat="server" BackColor="#696969" Font-Bold="True"
                                                Width="30px" Text="-" ForeColor="#FFFFFF" OnClientClick="return SetHourMinute('H', '-')" />
                                            <asp:TextBox ID="txtHora" runat="server" Width="25px"></asp:TextBox>
                                            <asp:Label ID="lblPuntos" Text=":" ForeColor="Black" Font-Bold="True" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtMin" runat="server" Width="25px"></asp:TextBox>
                                            <asp:Button ID="btnAumentarMinuto" runat="server" Font-Bold="True" Width="30px" Text="+"
                                                BackColor="#696969" ForeColor="#FFFFFF" OnClientClick="return SetHourMinute('M', '+')" />
                                            <asp:Button ID="btnDisminuirMinuto" runat="server" Font-Bold="True" Width="30px"
                                                Text="-" BackColor="#696969" ForeColor="#FFFFFF" OnClientClick="return SetHourMinute('M', '-')" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btnAceptarCedula" runat="server" CssClass="botones" Height="28" onmouseout="style.backgroundColor='#696969'"
                                                onmouseover="style.backgroundColor='#A9A9A9'" Text="Aceptar" ValidationGroup="Forma"
                                                Width="93px" OnClientClick="return setHourglass();" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnCerrarModalCedula" runat="server" CssClass="botones" Height="28"
                                                onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                                Text="Cancelar" Width="93px" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                </tr>
                            </tr>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:ModalPopupExtender ID="ModalCedula" runat="server" PopupControlID="pnlCedula"
                    BackgroundCssClass="FondoAplicacion" PopupDragHandleControlID="PanelTitulo" TargetControlID="LBCedula"
                    CancelControlID="btnCerrarModalCedula">
                </asp:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hdn_width" runat="server" />
        <script language="javascript" src="~/Scripts/wait.js" type="text/javascript"></script>
        <!-- Mostrar reloj de arena en la página cuando haya postbacks asíncronos -->
        <script language="javascript" type="text/javascript">

            function setHourglass() {

                if (document.getElementById('<%= txtFechaCedula.ClientID %>').value == '')
                    return false;


                if (document.getElementById('<%= ddlNotificador.ClientID %>').value == '-1')
                    return false;


                $('#Container').css('cursor', 'wait');
                return true;
            }

            function removeHourglass() {
                $('#Container').css('cursor', 'auto');
            }

            function getFlickerSolved() {
                jQuery('<%= PanelPersonaliza.ClientID %>').css("display", "none");
            }

            function closePopupCedula() {
                $find('ModalCedula').hide();
            }

            function CierraModalPersonzalizar(ActualzarGrid) {
                //Cierra Modal Columnas Personalizadas
                if (ActualzarGrid == 1) {
                    document.getElementById('btnAcualizaGridPersonalizado').click();
                }
                else {
                    $find('ModalPersonaliza').hide();
                    //upGridView
                    return false;
                }
                //document.getElementById('btnCerrarModal').click();
                //getFlickerSolved();


            }

            function BotonFiltrar() {
                jQuery("#GRID").css("display", "none");
                jQuery("#Imagen_procesando").css("display", "")
                jQuery("#Imagen_procesando").css("height", "300px");
                jQuery("#tbl_footer").css("display", "none");
                jQuery("#pnlImagenNoExisten").css("display", "none");
            }

            //NHM INI - Se comenta porque en SISAN manda excepcion de que algo llega nulo
           // $(document).ready(function () {
             //   $('#GRID').scroll(function () {
               //     var scroll = $("#GRID").scrollTop();
                 //   $("#GRID th").css("top", scroll);

             //   });
             //   //$("#GRID").css("width", document.body.clientWidth);
             //   //alert(document.body.clientWidth);
             //   document.getElementById('<%= hdn_width.ClientID %>').value = document.body.clientWidth;
             //   //                document.getElementById('<%= GRID.ClientID %>').style.width = document.body.clientWidth;

             //   //CallClickINICIO();


            //});
            //NHM FIN

            //            function CallClickINICIO() {
            //                try {
            //                    //WORK IN ALL BROWSERS
            //                    try {

            //                        //$get('<%= btnCargaGRID.ClientID %>').click();
            //                        alert('mi mensaje');

            //                    } catch (e) { }

            //                    return false;
            //                }
            //                catch (e) { }
            //            }

            function enableModalFileUploadButtonOK() {
                document.getElementById('<%= btnModalFileUploadOK.ClientID %>').disabled = false;
                //NHM INI
                var img = document.getElementById('<%= imgCargandoArchivo.ClientID%>');
                img.style.display = "none";
                //NHM FIN
            }

            //NHM INI
            function verImgCargando(sender, args) {
                //alert("inica carga");
                var img = document.getElementById('<%= imgCargandoArchivo.ClientID%>');
                img.style.display = "block";
            }

            function onUploadError(sender, args) {
                var img = document.getElementById('<%= imgCargandoArchivo.ClientID%>');
                img.style.display = "none";
                alert(args.get_errorMessage());

            }
            //NHM FIN

            function clickDownloadButton() {
                try {
                    //WORK IN ALL BROWSERS
                    try {
                        $get('<%= lnkdl.ClientID %>').click();
                    } catch (e) { }

                    return false;
                }
                catch (e) { }
            }

            //            var xPosGRID, yPosGRID; //Para mantener la posición del scroll en #GRID
            //            var prm2 = Sys.WebForms.PageRequestManager.getInstance();
            //            prm2.add_endRequest(EndRequestInline);
            //            prm2.add_beginRequest(beginRequestInline);

            //            function EndRequestInline(sender, args) {
            //                $('#GRID').scroll(function () {
            //                    var scroll = $("#GRID").scrollTop();
            //                    $("#GRID th").css("top", scroll);
            //                });

            //                $get('GRID').scrollLeft = xPosGRID;
            //                $get('GRID').scrollTop = yPosGRID;

            //            }

            //            function beginRequestInline(sender, args) {
            //                xPosGRID = $get('GRID').scrollLeft;
            //                yPosGRID = $get('GRID').scrollTop;

            //            }

            function copyToClipboard(s) {
                if (window.clipboardData && clipboardData.setData) {
                    clipboardData.setData("Text", s);
                }
                else {
                    // You have to sign the code to enable this or allow the action in about:config by changing user_pref("signed.applets.codebase_principal_support", true);
                    netscape.security.PrivilegeManager.enablePrivilege('UniversalXPConnect');

                    var clip = Components.classes['@mozilla.org/widget/clipboard;[[[[1]]]]'].createInstance(Components.interfaces.nsIClipboard);
                    if (!clip) return;

                    // create a transferable
                    var trans = Components.classes['@mozilla.org/widget/transferable;[[[[1]]]]'].createInstance(Components.interfaces.nsITransferable);
                    if (!trans) return;

                    // specify the data we wish to handle. Plaintext in this case.
                    trans.addDataFlavor('text/unicode');

                    // To get the data from the transferable we need two new objects
                    var str = new Object();
                    var len = new Object();

                    var str = Components.classes["@mozilla.org/supports-string;[[[[1]]]]"].createInstance(Components.interfaces.nsISupportsString);

                    var copytext = meintext;

                    str.data = copytext;

                    trans.setTransferData("text/unicode", str, copytext.length * [[[[2]]]]);

                    var clipid = Components.interfaces.nsIClipboard;

                    if (!clip) return false;

                    clip.setData(trans, null, clipid.kGlobalClipboard);
                }
            }


            function SetHourMinute(tipo, operacion) {

                var final = new String();

                var maximo = 23;
                var control = document.getElementById('<%= txtHora.ClientID %>');

                if (tipo == 'M') {
                    control = document.getElementById('<%= txtMin.ClientID %>');
                    maximo = 59;
                }

                var valor = new Number(control.value);


                if (valor == 0 && operacion == '+')
                    valor++;
                else if (valor == 0 && operacion == '-')
                    valor = maximo;
                else if (valor == maximo && operacion == '+')
                    valor = 0;
                else if (valor == maximo && operacion == '-')
                    valor--;
                else if (operacion == '+')
                    valor++;
                else
                    valor--;

                final = valor.toString(); ;

                if (final.length < 2)
                    final = '0' + final;

                control.value = final;

                return false;

            }


            function ShowProcesa() {

                $find('mpeProcesa').show();

            }


            function Cierrame() {

                window.close();

            }


            function ShowBitacora(link) {


                url = 'UserControls/ShowBitacoraOficios.aspx?doc=' + link.innerText + '&tipo=1';

                winprops = "dialogHeight: 400px; dialogWidth: " + parseInt(document.body.clientWidth * 0.9).toString() + "px; edge: Raised; center: Yes; help: No;resizable: No; status: No; Location: No; Titlebar: No;"

                window.showModalDialog(url, "Historial", winprops);

                return false;

            }

            function MuestraProcesa() {

                if (document.getElementById('<%= lblModalPostBack.ClientID %>').innerText == 'CorreoNotificacion') {

                    ShowProcesa();

                }

            }


            function GetScrollGrid() {

                document.getElementById('hdnScrollPosition').innerText = $get('GRID').scrollTop;
                return true;

            }


            function SetScrollGrid(position) {

                $('#GRID').scrollTop(position);

            }


        </script>
        <asp:UpdatePanel ID="upDld" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="lnkdl" />
            </Triggers>
            <ContentTemplate>
                <asp:LinkButton ID="lnkdl" runat="server" ClientIDMode="Static" Text="Click_me" OnClick="DownloadFile"
                    Style="display: none"></asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
        </form>
    </div>
    <script language="javascript" type="text/javascript">

        function disposeTree(sender, args) {
            var elements = args.get_panelsUpdating();
            for (var i = elements.length - 1; i >= 0; i--) {
                var element = elements[i];
                var allnodes = element.getElementsByTagName('*'),
                length = allnodes.length;
                var nodes = new Array(length)
                for (var k = 0; k < length; k++) {
                    nodes[k] = allnodes[k];
                }
                for (var j = 0, l = nodes.length; j < l; j++) {
                    var node = nodes[j];
                    if (node.nodeType === 1) {
                        if (node.dispose && typeof (node.dispose) === "function") {
                            node.dispose();
                        }
                        else if (node.control && typeof (node.control.dispose) === "function") {
                            node.control.dispose();
                        }

                        var behaviors = node._behaviors;
                        if (behaviors) {
                            behaviors = Array.apply(null, behaviors);
                            for (var k = behaviors.length - 1; k >= 0; k--) {
                                behaviors[k].dispose();
                            }
                        }
                    }
                }
                element.innerHTML = "";
            }
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(disposeTree);

    </script>
</body>
</html>
