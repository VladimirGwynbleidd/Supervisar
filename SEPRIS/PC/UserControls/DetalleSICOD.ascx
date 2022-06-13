<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DetalleSICOD.ascx.vb" Inherits="SEPRIS.DetalleSICOD" %>

<asp:Panel ID="pnlRegistro" runat="server">
    <asp:Panel ID="pnlEnabled" runat="server">


        <table style="width: 840px"  >
            <tr>
                <td style="height: 30px" colspan="5"> 
                </td>
            </tr>
            <tr>
                <td style="width: 130px">
                    <asp:Label ID="lblDocumento" runat="server" Text="Archivo:" CssClass="txt_gral"></asp:Label>
                </td>
                <td class="style1" colspan="2">
                    <asp:LinkButton ID="lnkDocumento" runat="server" CssClass="txt_gral" Enabled="false"></asp:LinkButton>
                    <asp:Button ID="btnVerDoc" runat="server" Text="Ver Documento" CausesValidation="false" Enabled="True" Visible="false"/>
                </td>
                <td style="width: 130px;">
                    <asp:Label ID="lblFechaRegistro" runat="server" Text="Fecha Registro:" CssClass="txt_gral"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFechaRegistro" CssClass="txt_solo_lectura" Width="136px"
                        ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 130px">
                    <asp:Label ID="lblTipoDocto" runat="server" Text="Tipo de Documento:" CssClass="txt_gral"></asp:Label>
                </td>
                <td class="style1">
                    <asp:DropDownList ID="ddlTDocto" runat="server" Height="20px" Width="255px" 
                        CssClass="txt_gral">
                        <asp:ListItem Value="-1" Selected="True">-Seleccione uno-</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style12"></td>
                <td>
                    <asp:Label ID="lblFechaRecepcion" runat="server" Text="Fecha Recepci&oacute;n Doc.:"
                        CssClass="txt_gral"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFechaRecepcion" runat="server" Style="width: 114px" CssClass="txt_gral"></asp:TextBox>
                    <asp:Image ImageAlign="Bottom" ID="imgCalFechaRecepcion" runat="server" ImageUrl="~/imagenes/Calendar.gif" />
                </td>
            </tr>
            <tr>
                <td style="width: 130px">
                    <asp:Label ID="lblNumeroOficio" runat="server" Text="Número de Oficio:" CssClass="txt_gral"></asp:Label>
                </td>
                <td align="left" class="style1">
                    <asp:TextBox ID="txtNumeroOficio" runat="server" Width="250px" CssClass="txt_gral"></asp:TextBox>
                </td>
                <td class="style12"></td>
                <td style="width: 130px">
                    <asp:Label ID="lblFechaDocto" runat="server" Text="Fecha Documento:" CssClass="txt_gral"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFechaDocto" runat="server" Style="width: 115px" CssClass="txt_gral"></asp:TextBox>
                    <asp:Image ImageAlign="Bottom" ID="imgCalFechaDocto" runat="server" ImageUrl="~/imagenes/Calendar.gif" />
                </td>
            </tr>
            <tr>
                <td style="width: 130px">
                    <asp:Label ID="lblReferencia" runat="server" Text="Referencia:" CssClass="txt_gral"></asp:Label>
                </td>
                <td class="style1">
                    <asp:TextBox ID="txtReferencia" runat="server" Width="250px" CssClass="txt_gral"></asp:TextBox>
                </td>
                <td class="style12"></td>
            </tr>
        </table>
        <table style="width: 840px">
            <tr>
                <td style="width: 130px">
                    <asp:Label ID="lblRemitente" runat="server" Text="Remitente:" CssClass="txt_gral"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRemitente" runat="server" Width="250px" CssClass="txt_gral"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table style="width: 840px">
            <tr>
                <td style="width: 130px">
                    <asp:Label ID="lblFirmanteNombre" runat="server" Text="Firmante/Nombre(s):" CssClass="txt_gral"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFirmanteNombre" runat="server" Width="120px" CssClass="txt_gral"></asp:TextBox>
                </td>
                <td style="width: 90px" align="right">
                    <asp:Label ID="lblApPaterno" runat="server" Text="A Paterno:" CssClass="txt_gral"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtApPaterno" runat="server" Width="120px" CssClass="txt_gral"></asp:TextBox>
                </td>
                <td style="width: 85px" align="left">
                    <asp:Label ID="lblApMaterno" runat="server" Text="A Materno:" CssClass="txt_gral"></asp:Label>
                </td>
                <td style="width: 205px">
                    <asp:TextBox ID="txtApMaterno" runat="server" Width="120px" CssClass="txt_gral"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table style="width: 840px">
            <tr>
                <td style="width: 130px">
                    <asp:Label ID="lblCargoFirmante" runat="server" Text="Cargo Firmante:" CssClass="txt_gral"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtCargoFirmante" runat="server" Width="250px" CssClass="txt_gral">
                    </asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="PanelEnabDest" runat="server">
        <table width="840px">
            <tr runat="server" id="tr_EstructuraDestinatario" visible="false">
                <td style="width: 130px">&nbsp;
                </td>
                <td style="width: 400px">
                    <asp:RadioButtonList ID="rdbEstructura" runat="server" CssClass="txt_gral"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">Estructura Oficial</asp:ListItem>
                        <asp:ListItem Value="2" Selected="True">Estructura Funcional</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td style="width: 85px" align="left">&nbsp;
                </td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 130px">
                    <asp:Label ID="lblArea" runat="server" CssClass="txt_gral" Text="Área Destinatario:"></asp:Label>
                </td>
                <td style="width: 400px">
                    <asp:DropDownList ID="ddlArea" runat="server" CssClass="txt_gral"
                        Height="20px" Width="390px">
                    </asp:DropDownList>
                </td>
                <td align="left" style="width: 85px">
                    <asp:Label ID="lblDestinatario" runat="server" CssClass="txt_gral" Text="Destinatario:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDestinatario" runat="server" CssClass="txt_gral"
                        Height="20px" Width="195px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table style="width: 840px">
        <tr>
            <td style="width: 135px">&nbsp;
            </td>
            <td colspan="4" style="width: 400px">
                <asp:CheckBox ID="chkCopiaConocimiento" runat="server" CssClass="txt_gral" Text="Copia de conocimiento"
                    Width="207px" />
            </td>
        </tr>
        <tr runat="server" id="tr_EstructuraCC" visible="false">
            <td style="width: 135px">&nbsp;
            </td>
            <td colspan="4" style="width: 400px">
                <asp:RadioButtonList ID="rdbEstructuraCC" runat="server" CssClass="txt_gral"
                    RepeatDirection="Horizontal">
                    <asp:ListItem Value="1">Estructura Oficial</asp:ListItem>
                    <asp:ListItem Value="2" Selected="True">Estructura Funcional</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="width: 135px">
                <asp:Label ID="Label4" runat="server" CssClass="txt_gral" Text="Área C.C.:"></asp:Label>
            </td>
            <td colspan="4" style="width: 400px">
                <asp:DropDownList ID="ddlAreaCC" runat="server" CssClass="txt_gral"
                    Height="20px" Width="390px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
            <td width="130px">
                <asp:Label ID="lblFuncionario" runat="server" Text="Funcionario:" CssClass="txt_gral"></asp:Label>
            </td>
            <td width="130px">&nbsp;
            </td>
            <td width="30px">&nbsp;
            </td>
            <td width="260px">
                <asp:Label ID="lblCC" runat="server" Text="C.C.:" CssClass="txt_gral"></asp:Label>
            </td>
            <td width="140px">&nbsp;
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
            <td colspan="2" rowspan="2" valign="top">
                <asp:ListBox ID="ListFuncionario" runat="server" CssClass="txt_gral" Height="150px"
                    Width="255px"></asp:ListBox>
            </td>
            <td style="text-align: center">
                <asp:ImageButton ID="btnFuncionarioCC" runat="server" ImageUrl="~/imagenes/FlechaRojaDer.gif"
                    Width="30px" Height="30px"  Enabled="false"/>
            </td>
            <td rowspan="2" valign="top">
                <asp:ListBox ID="ListFuncionarioCC" runat="server" Height="150px" Width="255px" CssClass="txt_gral"></asp:ListBox>
            </td>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
            <td style="text-align: center">
                <asp:ImageButton ID="btnFuncionario" runat="server" ImageUrl="~/imagenes/FlechaRojaIzq.gif"
                    Width="30px" Height="30px" Enable="false"/>
            </td>
            <td>&nbsp;
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td style="width: 130px">
                <asp:Label ID="lblAsunto" runat="server" Text="Asunto:" CssClass="txt_gral"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
            <td>
                <asp:TextBox ID="TxtAsunto" runat="server" CssClass="txt_gral" Height="45px" Width="685px"
                    TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td style="width: 685px; text-align: right">
                <asp:Label runat="server" ID="lblCarAsunto" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                <asp:Label runat="server" ID="lblAsuntoCaracteres" CssClass="txt_gral" Text="600"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlEnableB" runat="server">
        <table style="width: 840px">
            <tr>
                <td style="width: 130px">
                    <asp:Label ID="lblAnexos" runat="server" Text="Anexos:" CssClass="txt_gral"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 130px">&nbsp;
                </td>
                <td align="left" style="width: 235px">
                    <asp:CheckBox ID="chkCarpeta" runat="server" Text="Carpeta" CssClass="txt_gral" Width="140px" />
                </td>
                <td align="left" style="width: 235px">
                    <asp:CheckBox ID="chkCDDVD" runat="server" Text="CD/DVD" CssClass="txt_gral" Width="140px" />
                </td>
                <td align="left" style="width: 20px">
                    <asp:CheckBox ID="chkSobreC" runat="server" CssClass="txt_gral" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblSobreCerrado" CssClass="txt_gral" Text="Sobre Cerrado"
                        Width="120px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 130px">&nbsp;
                </td>
                <td align="left" style="width: 235px">
                    <asp:CheckBox ID="chkPaquete" runat="server" Text="Paquete" CssClass="txt_gral" Width="140px" />
                </td>
                <td align="left" style="width: 235px">
                    <asp:CheckBox ID="chkRevistas" runat="server" Text="Revistas" CssClass="txt_gral"
                        Width="140px" />
                </td>
                <td align="left" style="width: 8px">
                    <asp:CheckBox ID="chkOtros" runat="server" CssClass="txt_gral"/>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblOtros" CssClass="txt_gral" Text="Otros:"></asp:Label>
                </td>
             </tr>
                <tr style="height: 30px">
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:TextBox ID="TxtOtros" runat="server" CssClass="txt_gral" Width="176px"></asp:TextBox>
                    </td>
                    
                </tr>
            
        </table>
    </asp:Panel>
</asp:Panel>

<script type="text/javascript">
   <%-- $(document).ready(function () {
        $("<%= btnVerDoc.ClientID%>").removeAttr('disabled');
        $("<%= btnVerDoc.ClientID%>").prop('disabled', false);
    });--%>
</script>