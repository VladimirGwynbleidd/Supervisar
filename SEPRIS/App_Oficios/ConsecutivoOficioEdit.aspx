<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConsecutivoOficioEdit.aspx.vb" Inherits="SICOD.ConsecutivoOficioEdit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ctk" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="~/Styles.css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <ctk:ToolkitScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="false">
        </ctk:ToolkitScriptManager>
        <br />
        <center>
        <asp:Label ID="lblTitulo" runat="server" Text="Consecutivo Oficios Modificar" CssClass="TitulosWebProyectos"></asp:Label>
    </center>
        <br />
        <asp:UpdatePanel ID="upPrincipal" runat="server" UpdateMode="Always">
            <ContentTemplate>

                <table width="100%">
                    <tr>
                        <td style="width: 25px">&nbsp;
                        </td>
                        <td style="width: 150px">
                            <asp:Label ID="lblarea" runat="server" Text="&Aacute;rea:" CssClass="txt_gral"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 650px">
                            <asp:DropDownList ID="ddlarea" runat="server" AutoPostBack="false" Width="100%" CssClass="txt_gral">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 25px">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Label ID="lblTipoDoc" runat="server" Text="Tipo Documento:" CssClass="txt_gral"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipoDoc" runat="server" AutoPostBack="false" Width="50%"
                                CssClass="txt_gral">
                            </asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Label ID="lblanio" runat="server" Text="Año" CssClass="txt_gral"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlanio" runat="server" CssClass="txt_gral" Width="15%">
                            </asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Label ID="lblNumConsecutivo" runat="server" Text="Número Consecutivo" CssClass="txt_gral"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txbNumConsecutivo" runat="server" CssClass="txt_gral" Style="width: 135px"></asp:TextBox>
                            <ctk:FilteredTextBoxExtender ID="ftbeNumConsecutivo" runat="server" FilterType="Numbers"
                                TargetControlID="txbNumConsecutivo">
                            </ctk:FilteredTextBoxExtender>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Label ID="lblAplicaNumCons" runat="server" Text="Aplica Número Consecutivo" CssClass="txt_gral"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAplicaNumCons" runat="server" CssClass="txt_gral" Width="15%"></asp:DropDownList>
                        </td>
                        <td></td>

                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4" style="width: 100%; text-align: center;">
                            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="botones" Height="22px" Width="130" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="botones" Height="22px" Width="130" />

                        </td>
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
                                    <asp:Label runat="server" ID="lblErroresTitulo" Text="Atenci&oacute;n" Style="vertical-align: middle; margin-left: 5px; color: White"
                                        CssClass="titulo_seccioninterior"></asp:Label>
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
                                    <asp:Label ID="lblModalPostBack" runat="server" Style="display: none"></asp:Label>
                            </center>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <ctk:ModalPopupExtender ID="ModalPopupExtenderErrores" runat="server" PopupControlID="PanelErrores"
                    TargetControlID="lnkError" BackgroundCssClass="FondoAplicacion" DropShadow="true"
                    PopupDragHandleControlID="PanelErroresHandle" CancelControlID="BtnModalOk">
                </ctk:ModalPopupExtender>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
