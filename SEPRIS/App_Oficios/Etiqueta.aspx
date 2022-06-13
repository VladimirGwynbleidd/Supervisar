<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Etiqueta.aspx.vb" Inherits="SICOD.Etiqueta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="~/Styles.css" />
    <style type="text/css">
       
        .style10
        {
            width: 80px;
        }
        .style17
        {
            width: 543px;
        }
        .style18
        {
            width: 168px;
        }
        .style21
        {
            width: 151px;
        }
        .style22
        {
            height: 17px;
        }
    </style>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
</head>
<body>
    <div id="Container">
        <form id="frmEtiqueta" runat="server">
        
        <script type="text/javascript" language="javascript" src="/Scripts/wait.js"></script>
        <asp:ToolkitScriptManager ID="Toolkitscriptmanager1" runat="server" EnableScriptGlobalization="true"
            AsyncPostBackTimeout="7200">
        </asp:ToolkitScriptManager>
        <br />
        <center>
            <asp:Label runat="server" ID="lblTitulo" Text="Etiquetas" CssClass="TitulosWebProyectos"></asp:Label>
        </center>
        <br />
        <asp:UpdatePanel ID="updPanelEtiqueta" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnOficialia" />
                <asp:PostBackTrigger ControlID="btnEtiquetas" />
            </Triggers>
            <ContentTemplate>
                <div>
                    <table width="90%">
                        <tr>
                            <td>
                                <asp:Panel runat="server" ID="pnlEtiqueta">
                                    <table align="center">
                                        <tr>
                                            <td class="style10">
                                                <asp:Label ID="lblArea" runat="server" Text="Área" CssClass="txt_gral"></asp:Label>
                                            </td>
                                            <td class="style21">
                                                <asp:DropDownList ID="ddlArea" runat="server" Width="439px" AutoPostBack="true" CssClass="txt_gral">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="style21">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAnio" runat="server" CssClass="txt_gral" Text="Año"></asp:Label>
                                            </td>
                                            <td class="style21">
                                                <asp:DropDownList ID="ddlAnio" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                                    Height="19px" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style10">
                                                &nbsp;
                                            </td>
                                            <td class="style21">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style10">
                                                <asp:Label ID="lblNumConsecutivo" runat="server" CssClass="txt_gral" Text="Número consecutivo"
                                                    Width="150px"></asp:Label>
                                            </td>
                                            <td class="style21">
                                                <asp:TextBox ID="txtOficioConsecutivo" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                                    Height="19px" Width="200px">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnAbrir" runat="server" CssClass="botones" Height="28" onmouseout="style.backgroundColor='#696969'"
                                                    onmouseover="style.backgroundColor='#A9A9A9'" Text="Aceptar" Width="93px" />
                                                &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnCancelar" runat="server" CssClass="botones" Height="28" onmouseout="style.backgroundColor='#696969'"
                                                    onmouseover="style.backgroundColor='#A9A9A9'" Text="Cancelar" Width="93px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnDestinatario" runat="server">
                                    <table align="center">
                                        <tr>
                                            <td width="150">
                                                <asp:Label ID="lblDiaEnvio" runat="server" CssClass="txt_gral" Text="Día de envió"></asp:Label>
                                            </td>
                                            <td class="style17">
                                                <asp:TextBox ID="txtFechaEnvio" runat="server" CssClass="txt_gral" Style="width: 115px"
                                                    MaxLength="10"></asp:TextBox>
                                                <asp:Image ID="imgCalFechaDocumento" runat="server" ImageAlign="Bottom" ImageUrl="~/imagenes/Calendar.gif"
                                                    Width="16px" />
                                                <asp:CalendarExtender ID="calFechaEnvio" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalFechaDocumento"
                                                    TargetControlID="txtFechaEnvio">
                                                </asp:CalendarExtender>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorFechaDocumento" runat="server"
                                                    ControlToValidate="txtFechaEnvio" Display="Dynamic" ErrorMessage="La Fecha Documento debe ser válida con el formato DD/MM/AAAA"
                                                    ForeColor="Red" Text="*" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                    ValidationGroup="Forma"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="style18">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOficio" runat="server" CssClass="txt_gral" Text="Oficio"></asp:Label>
                                            </td>
                                            <td class="style18">
                                                <asp:TextBox ID="textOficio" runat="server" CssClass="txt_gral" ReadOnly="True" Style="width: 120px"
                                                    Text="Número de oficio"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="style18">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trEntidad" runat="server">
                                            <td>
                                                <asp:Label ID="lblEntidad" runat="server" CssClass="txt_gral" Text="Entidad:"></asp:Label>
                                            </td>
                                            <td class="style18">
                                                <asp:DropDownList ID="ddlEntidad" runat="server" CssClass="txt_gral" Height="17px"
                                                    Width="350px" Enabled="False">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="150">
                                                &nbsp;
                                            </td>
                                            <td class="style17" valign="top">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="150">
                                                <asp:Label ID="lblTitular" runat="server" CssClass="txt_gral" Text="Destinatario"></asp:Label>
                                            </td>
                                            <td class="style17" valign="top" align="left">
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:CheckBoxList ID="chkDestinatarios" runat="server" CssClass="txt_gral">
                                                            </asp:CheckBoxList>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/imagenes/AddDestinatario.jpg" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="150">
                                                <asp:Label ID="lbldomicilio" runat="server" CssClass="txt_gral" Text="Domicilio"></asp:Label>
                                            </td>
                                            <td class="style17">
                                                <asp:TextBox ID="textDomicilio" runat="server" CssClass="txt_gral" Height="70px"
                                                    Width="350px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="150px">
                                                &nbsp;
                                            </td>
                                            <td class="style17">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="150px">
                                                <asp:Label ID="lblColonia" runat="server" CssClass="txt_gral" Text="Colonia"></asp:Label>
                                            </td>
                                            <td class="style17">
                                                <asp:TextBox ID="textColonia" runat="server" CssClass="txt_gral" Width="350px" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="150px">
                                                &nbsp;
                                            </td>
                                            <td class="style17">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="150px">
                                                <asp:Label ID="lblEstado" runat="server" CssClass="txt_gral" Text="Estado"></asp:Label>
                                            </td>
                                            <td class="style17">
                                                <asp:DropDownList ID="ddlEstado" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                                    Width="350px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="150px">
                                                &nbsp;
                                            </td>
                                            <td class="style17">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="150px">
                                                <asp:Label ID="lblPoblacion" runat="server" CssClass="txt_gral" Text="Población"></asp:Label>
                                            </td>
                                            <td class="style17">
                                                <asp:DropDownList ID="ddlPoblacion" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                                    Width="350px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style4">
                                                &nbsp;
                                            </td>
                                            <td class="style4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style4">
                                                <asp:Label ID="lblCP" runat="server" CssClass="txt_gral" Text="C.P."></asp:Label>
                                            </td>
                                            <td class="style4">
                                                <asp:TextBox ID="textCP" runat="server" CssClass="txt_gral" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style4">
                                            </td>
                                            <td class="style4">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style4">
                                                &nbsp;
                                            </td>
                                            <td class="style4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style4">
                                                &nbsp;
                                            </td>
                                            <td class="style4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style4">
                                                &nbsp;
                                            </td>
                                            <td class="style4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style4" align="center" colspan="2">
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnDatosFormatoEtiqueta" runat="server" CssClass="botones" Height="28"
                                                    onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                                    Text="Aceptar" Width="95px" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnCancelarDestinatario" runat="server" CssClass="botones" Height="28"
                                                    onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                                    Text="Cancelar" Width="95px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Panel ID="pnFormatosEtiquetas" runat="server">
                                    <table class="style3" width="100%">
                                        <tr>
                                            <td class="style22">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNombreEtiquetas" runat="server" Text=" " CssClass="txt_gral">
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:DataGrid ID="gwEtiquetasFormatos" runat="server" AutoGenerateColumns="False"
                                                    Style="margin-top: 0px" Width="100%" BackColor="#EEEEEE" Font-Size="7.5pt" CellPadding="1"
                                                    Font-Name="Arial" HorizontalAlign="Center" Font-Names="Arial" 
                                                    ForeColor="#555555">
                                                    <HeaderStyle ForeColor="White" BackColor="#236B7C" CssClass="GridViewHeaderOficios">
                                                    </HeaderStyle>
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSeleccion" runat="server" Enabled="True" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="T_ENTIDAD_CORTO" HeaderStyle-CssClass="BO_Column" HeaderText="Entidad" />
                                                        <asp:BoundColumn DataField="NOMBRE_PERSONA" HeaderStyle-CssClass="BO_Column" HeaderText="Nombre" />
                                                        <asp:BoundColumn DataField="T_DIRECCION" HeaderStyle-CssClass="BO_Column" HeaderText="Dirección" />
                                                        <asp:BoundColumn DataField="T_COLONIA" HeaderStyle-CssClass="BO_Column" HeaderText="Colonia" />
                                                        <asp:BoundColumn DataField="T_CP" HeaderStyle-CssClass="BO_Column" HeaderText="CP" />
                                                        <asp:BoundColumn DataField="T_ESTADO" HeaderStyle-CssClass="BO_Column" HeaderText="Estado" />
                                                        <asp:BoundColumn DataField="T_POBLACION" HeaderStyle-CssClass="BO_Column" HeaderText="Población" />
                                                    </Columns>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style4">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnEtiquetas" runat="server" CssClass="botones" Height="28" Width="93px"
                                                    onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                                    Text="Etiquetas" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnOficialia" runat="server" CssClass="botones" Height="28px" Width="93px"
                                                    onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                                    Text="Oficialía" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnCancelarEtiquetas" runat="server" CssClass="botones" Height="28"
                                                    Width="93px" onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                                    Text="Cancelar" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>
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
                                            Style="vertical-align: middle; margin-left: 5px; color: White" CssClass="titulo_seccioninterior"></asp:Label>
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
                        TargetControlID="LB1" BackgroundCssClass="FondoAplicacion" DropShadow="true"
                        PopupDragHandleControlID="PanelErroresHandle" OkControlID="BtnModalOk" OnCancelScript="getFlickerSolved()">
                    </asp:ModalPopupExtender>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <script type="text/javascript">
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
        </script>
        </form>
    </div>
</body>
</html>
