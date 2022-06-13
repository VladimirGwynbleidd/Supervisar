<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConsecutivoOficio.aspx.vb"
    Inherits="SICOD.ConsecutivoOficio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ctk" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="~/Styles.css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ValidaFiltros() {

            // Aqui validamos que por lo menos se haya seleccionado un filtro
            ValAr = document.getElementById('<%= ddlarea.ClientID %>').value;
            ValTd = document.getElementById('<%= ddlTipoDoc.ClientID %>').value;
            ValAn = document.getElementById('<%= ddlanio.ClientID %>').value;

            if (ValAr == '-1' && ValTd == '-1' && ValAn == '-1') {

                document.getElementById('<%= lblErroresPopup.ClientID %>').innerHtml = 'Se debe seleccionar al menos un filtro';
                $find('ModalPopupExtenderErrores').show();
                return false;

            }


            return true;

        }

        function Foco() {
            //$('#' + '<%=frmConsecutivoOficio.FindControl("txtConsecutivo").ClientID %>').focus();
            //$("#tt > input").focus();
            return;
        }

    </script>
</head>
<body>
    <form id="frmConsecutivoOficio" runat="server">
    <ctk:ToolkitScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="false">
    </ctk:ToolkitScriptManager>
    <br />
    <center>
        <asp:Label ID="lblTitulo" runat="server" Text="Consecutivo Oficios" CssClass="TitulosWebProyectos"></asp:Label>
    </center>
    <br />
    <asp:UpdatePanel ID="upPrincipal" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table width="100%" class="FiltroBandeja">
                <tr>
                    <td style="width: 25px">
                        &nbsp;
                    </td>
                    <td style="width: 150px">
                        <asp:Label ID="lblarea" runat="server" Text="&Aacute;rea:" CssClass="txt_gral" ForeColor="White"
                            Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 650px">
                        <asp:DropDownList ID="ddlarea" runat="server" AutoPostBack="false" Width="100%" CssClass="txt_gral">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 25px">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblTipoDoc" runat="server" Text="Tipo Documento:" CssClass="txt_gral"
                            ForeColor="White" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipoDoc" runat="server" AutoPostBack="false" Width="50%"
                            CssClass="txt_gral">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblanio" runat="server" Text="Año" CssClass="txt_gral" ForeColor="White"
                            Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlanio" runat="server" CssClass="txt_gral" Width="15%">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnfiltrar" runat="server" Text="Filtrar" CssClass="botones" Height="22px"
                            onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                            Width="130" OnClientClick="return ValidaFiltros()" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%">
                <tr>
                    <td style="width: 25px">
                        &nbsp;
                    </td>
                    <td style="width: 100%">
                        <asp:GridView ID="gvDatos" runat="server" AllowSorting="False" ForeColor="#555555"
                            Font-Size="7.5pt" CellPadding="1" AutoGenerateColumns="False" Font-Names="Arial"
                            HorizontalAlign="Center" Font-Name="Arial" PageSize="15" BackColor="#D6EBBD"
                            BorderColor="White" DataKeyNames="ID_UNIDAD_ADM, ID_T_UNIDAD_ADM, ID_ANIO, ID_TIPO_DOCUMENTO, NUM_CONSECUTIVO, CURRENT_CONSECUTIVO, B_APLICA_NUM_CONSEC"
                            Width="100%" AllowPaging="false">
                            <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="HeaderSICOD"
                                ForeColor="#FFFFFF"></HeaderStyle>
                            <RowStyle Wrap="true" />
                            <Columns>
                                <asp:ButtonField ButtonType="Link" CommandName="Selecciona" ControlStyle-ForeColor="DarkBlue"
                                    ItemStyle-HorizontalAlign="Center" Text="O" />
                                <asp:BoundField DataField="DSC_UNIDAD_ADM" HeaderStyle-CssClass="BO_Column" HeaderStyle-ForeColor="White"
                                    HeaderText="Área" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="395px" />
                                <asp:BoundField DataField="ID_ANIO" HeaderStyle-CssClass="BO_Column" HeaderStyle-ForeColor="White"
                                    HeaderText="Año" ItemStyle-Width="75px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="T_TIPO_DOCUMENTO" HeaderStyle-CssClass="BO_Column" HeaderText="Tipo Documento"
                                    ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="White" />
                                <asp:BoundField DataField="NUM_CONSECUTIVO" HeaderStyle-CssClass="BO_Column" HeaderText="Consecutivo Inicial"
                                    ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="White" />
                                <asp:BoundField DataField="CURRENT_CONSECUTIVO" HeaderStyle-CssClass="BO_Column"
                                    HeaderText="Consecutivo Actual" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-ForeColor="White" />
                                 <asp:BoundField DataField="B_APLICA_NUM_CONSEC" HeaderStyle-CssClass="BO_Column"
                                    HeaderText="Aplicar Número Consecutivo" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-ForeColor="White" />
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td style="width: 25px">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 25px">
                        &nbsp;</td>
                    <td style="width: 100%; text-align: center;">
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="botones"  Height="22px" Width="130"/>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="botones"  Height="22px" Width="130" />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnDefine" runat="server" CssClass="botones" Height="22px" 
                            Text="Define" Visible="false" Width="130" />
                    </td>
                    <td style="width: 25px">
                        &nbsp;</td>
                </tr>
            </table>
            <asp:LinkButton ID="lnkError" runat="server" Text="Popup" Style="display: none"></asp:LinkButton>
            <asp:Panel ID="PanelErrores" runat="server" CssClass="PanelErrores" Width="550px"
                Style="display: none">
                <table style="width: 100%; border-style: solid; border-width: 2px; border-color: White"
                    cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <asp:Panel ID="PanelErroresHandle" runat="server" CssClass="PanelErroresHandle" Width="100%"
                                Height="20px">
                                <asp:Label runat="server" ID="lblErroresTitulo" Text="Atenci&oacute;n" Style="vertical-align: middle;
                                    margin-left: 5px; color: White" CssClass="titulo_seccioninterior"></asp:Label>
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
                            </center>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ctk:ModalPopupExtender ID="ModalPopupExtenderErrores" runat="server" PopupControlID="PanelErrores"
                TargetControlID="lnkError" BackgroundCssClass="FondoAplicacion" DropShadow="true"
                PopupDragHandleControlID="PanelErroresHandle" CancelControlID="BtnModalOk">
            </ctk:ModalPopupExtender>
            <asp:LinkButton ID="lnkCaptura" runat="server" Text="PopUp" Style="display: none"></asp:LinkButton>
            <asp:Panel ID="PanelCaptura" runat="server" CssClass="PanelErrores" Width="450px"
                Style="display: none">
                <table style="width: 100%; border-style: solid; border-width: 2px; border-color: White"
                    cellspacing="0" cellpadding="0">
                    <tr>
                        <td colspan="2" style="text-align: center">
                            <asp:Panel ID="PanelCapturaHandel" runat="server" CssClass="PanelErroresHandle" Width="100%"
                                Height="40px">
                                <asp:Label runat="server" ID="lblCapturaTitulo" Text="Consecutivo Inicial" Style="vertical-align: middle;
                                    margin-left: 5px; color: White" CssClass="titulo_seccioninterior"></asp:Label>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 225px" class="txt_gral" align="right">
                            * Consecutivo&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td style="width: 225px" align="left">
                            <asp:TextBox ID="txtConsecutivo" runat="server" MaxLength="5" Width="60px" Style="text-align: right"
                                CssClass="txt_gral"></asp:TextBox>
                            <ctk:FilteredTextBoxExtender ID="fte" runat="server" TargetControlID="txtConsecutivo"
                                FilterMode="ValidChars" FilterType="Numbers">
                            </ctk:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="txtConsecutivo"
                                Text="*" Display="Dynamic" ForeColor="Red" ErrorMessage="*" ValidationGroup="Consecutivo"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" valign="middle" style="height: 40px">
                            <asp:Label ID="Label1" runat="server" Text="* = Captura Obligatoria" CssClass="txt_gral"></asp:Label>
                        </td>
                    </tr>
                    <tr align="center">
                        <td colspan="2">
                            <asp:Button ID="BtnCapturaOK" runat="server" Text="Aceptar" CssClass="botones" Width="130"
                                Height="22px" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'" ValidationGroup="Consecutivo" />
                            &nbsp;
                            <asp:Button ID="BtnCancelarCancel" runat="server" Text="Cancelar" CssClass="botones"
                                Width="130" Height="22px" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ctk:ModalPopupExtender ID="ModalPopUpCaptura" runat="server" PopupControlID="PanelCaptura"
                TargetControlID="lnkCaptura" BackgroundCssClass="FondoAplicacion" DropShadow="true"
                PopupDragHandleControlID="PanelCapturaHandel" CancelControlID="BtnCancelarCancel">
            </ctk:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
