<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PosibleIncumplimiento.ascx.vb" Inherits="SEPRIS.PosibleIncumplimiento" %>
<asp:UpdatePanel ID="upnlConsulta" runat="server">
    <ContentTemplate>

        <script lang="javascript" type="text/javascript">

                function validaLimiteNO(obj, maxchar) {
                    if (this.id) obj = this;
                    var remaningChar = maxchar - obj.value.length;
                    //document.getElementById('lblComentariosCaracteres').innerHTML = remaningChar;
                    $("#<%=lblCaracteresNO.ClientID%>").text("" + remaningChar);

                    if (remaningChar <= 0) {
                        //document.getElementById('lblComentariosCaracteres').innerHTML = 0;
                        $("#<%=lblCaracteresNO.ClientID%>").text("" + 0);
                        obj.value = obj.value.substring(maxchar, 0);
                        return false;
                    }
                    else { return true; }
                }

                function validaLimiteDescripcionNO(obj, maxchar) {
                    if (this.id) obj = this;
                    var remaningChar = maxchar - obj.value.length;
                    //document.getElementById('lblComentariosCaracteres').innerHTML = remaningChar;
                    $("#<%=lblDescripcionContadorNO.ClientID%>").text("" + remaningChar);

                    if (remaningChar <= 0) {
                        //document.getElementById('lblComentariosCaracteres').innerHTML = 0;
                        $("#<%=lblDescripcionContadorNO.ClientID%>").text("" + 0);
                        obj.value = obj.value.substring(maxchar, 0);
                        return false;
                    }
                    else { return true; }
                }

                function validaLimiteLongitudNO(obj, maxchar) {
                    if (this.id) obj = this;
                    var remaningChar = maxchar - obj.value.length;
                    //document.getElementById('lblComentariosCaracteres').innerHTML = remaningChar;
                    $("#<%=lblCaracteresNO.ClientID%>").text("" + remaningChar);

                    if (remaningChar <= 0) {
                        //document.getElementById('lblComentariosCaracteres').innerHTML = 0;
                        $("#<%=lblCaracteresNO.ClientID%>").text("" + 0);
                        obj.value = obj.value.substring(maxchar, 0);
                        return false;
                    }
                    else { return true; }
                }


                function ValidateTextNO(obj) {
                    if (obj.value.length > 0) {
                        obj.value = obj.value.replace(/[^\d]+/g, '');
                    }
                }

                function JsActivaCampos(blnActiva) {
                    if (blnActiva = "False") {
                        document.getElementById('<%= LblAsterixPosInc1.ClientID%>').style.display = 'none';
                        document.getElementById('<%= LblAsterixPosInc1.ClientID%>').style.Visible = false;
<%--                        document.getElementById('<%= LblAsterixPosInc2.ClientID%>').style.display = 'none';
                        document.getElementById('<%= LblAsterixPosInc2.ClientID%>').style.Visible = false;--%>
                    } else {
                        document.getElementById('<%= LblAsterixPosInc1.ClientID%>').style.display = 'block';
                        document.getElementById('<%= LblAsterixPosInc1.ClientID%>').style.Visible = true;
<%--                        document.getElementById('<%= LblAsterixPosInc2.ClientID%>').style.display = 'block';
                        document.getElementById('<%= LblAsterixPosInc2.ClientID%>').style.Visible = true;--%>
                    }
                }

        </script>

        <div id="IDPosibIncumplOPI">
            <table style="width: 95%; align-content:center">
                <tr>
                    <td style="height: 35px; width: 15%; text-align: left;">
                        <asp:Label ID="LblPosibleIncumpOPINO" runat="server" Text="¿Procede el Posible incumplimiento?" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label3" runat="server" Text=" " CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="LblAsterixPosInc1" runat="server" Text="*" style="color: red; font-size: 9pt"></asp:Label>
                    </td>
                    <td style="width=85%">
                        <asp:RadioButtonList ID="rdbPosibIncumpl" runat="server" AutoPostBack="True" CssClass="txt_gral"
                            RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">SI</asp:ListItem>
                            <asp:ListItem Value="0" Selected="True">NO</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </div>
        <div id="DivMotivNOProced" runat="server">
            <table style="width: 95%; align-content:center">
                <tr>
                    <td style="height: 35px; width: 15%; text-align: left;">
                        <asp:Label ID="LblMotivNOProced" runat="server" Text="Motivo de NO PROCEDENCIA" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label5" runat="server" Text=" " CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="LblAsterixPosInc2" runat="server" Text="*" style="color: red; font-size: 9pt" ></asp:Label>
                    </td>
                    <td style="height: 35px; width: 85%; text-align: left;">
                        <asp:TextBox ID="txtMotivNOProced" runat="server" onkeypress="ReemplazaCEspeciales(this.id)" 
                                onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)" 
                                onkeyup="validaLimiteLongitudNO(this,250)" 
                                TextMode="MultiLine" Width="100%" MaxLength="250" Height="70px"></asp:TextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4" colspan="2">
                    </td>
                </tr>
                <tr id="ObjCaracteresNO" runat="server" clientidmode="Static">
                    <td style="text-align: right;" colspan="2" >
                        <div id="divConttxtObjetoOPINO" runat="server" style="width: 100%; text-align: right; float:left;">
                            <asp:Label runat="server" ID="lblAstericoNO" style="color: red; font-size: 9pt" Text="*"></asp:Label>
                            <asp:Label runat="server" ID="lblDescripcionContadorNO" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                            <asp:Label runat="server" ID="lblCaracteresNO" CssClass="txt_gral" Text="250" ClientIDMode="Static"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:HiddenField ID="HddFldPosibIncump" runat="server" ClientIDMode="Static" Value="NO" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>