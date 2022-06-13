<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="IrregularidadOPI.ascx.vb" Inherits="SEPRIS.IrregularidadOPI" %>

<%@ Register Src="ComentariosOPI.ascx" TagName="Comentarios" TagPrefix="uc1" %>

<asp:UpdatePanel ID="upnlConsulta" runat="server">
    <ContentTemplate>

        <script language="javascript" type="text/javascript">

            function ActivaExisteIrreg(strValor) {
                Existe = $("#ExisteIrreg");
                NOIrreg = $("#IDJustNOIrreg");
                if (strValor == "S") {
                    Existe.show();
                    ActionJustNoIrreg();
                } else {
                    Existe.hide();
                    NOIrreg.hide();
                }
            }

            function ActionJustNoIrreg() {
                resultado = getCheckedRadio("rdbExisteIrregOPI");
                divJustNoIrreg = $("#IDJustNOIrreg");
                divIrregStd = $("#IDIrregStd");
                if (resultado == "SI") {
                    //divJustNoIrreg.hide();
                    $('#<%=txtJustificacionOPI.ClientID%>').text("");
                    divIrregStd.show();
                    $("#<%=txtExisteIrregOPI.ClientID%>").text("1");
                } else {
                    if ($("#ExisteIrreg").is(':visible')) {
                        //divJustNoIrreg.show();
                    } else {
                        //divJustNoIrreg.hide();
                    }
                    divIrregStd.hide();
                    $("#<%=txtExisteIrregOPI.ClientID%>").text("0");
                }
            }


            function getCheckedRadio(objRadio) {
                var radioButtons = document.getElementsByName(objRadio);
                for (var x = 0; x < radioButtons.length; x++) {
                    if (radioButtons[x].checked) {
                        return radioButtons[x].value;
                    }
                }
            }

            function validaLimiteJust(obj, maxchar) {
                if (this.id) obj = this;
                var remaningChar = maxchar - obj.value.length;
                //document.getElementById('lblComentariosCaracteres').innerHTML = remaningChar;
                $("#<%=lblJustificacionCaracteres.ClientID%>").text("" + remaningChar);

                        if (remaningChar <= 0) {
                            //document.getElementById('lblComentariosCaracteres').innerHTML = 0;
                            $("#<%=lblJustificacionCaracteres.ClientID%>").text("" + 0);
                            obj.value = obj.value.substring(maxchar, 0);
                            return false;
                        }
                        else { return true; }
                    }

                    function validaLimiteDescripcionJust(obj, maxchar) {
                        if (this.id) obj = this;
                        var remaningChar = maxchar - obj.value.length;
                        //document.getElementById('lblComentariosCaracteres').innerHTML = remaningChar;
                        $("#<%=lblDescripcionContadorJust.ClientID%>").text("" + remaningChar);

                        if (remaningChar <= 0) {
                            //document.getElementById('lblComentariosCaracteres').innerHTML = 0;
                            $("#<%=lblDescripcionContadorJust.ClientID%>").text("" + 0);
                            obj.value = obj.value.substring(maxchar, 0);
                            return false;
                        }
                        else { return true; }
                    }

                    function validaLimiteLongitudJust(obj, maxchar, labelContador) {
                        if (this.id) obj = this;
                        var remaningChar = maxchar - obj.value.length;
                        //document.getElementById('lblComentariosCaracteres').innerHTML = remaningChar;
                        $("#" + labelContador).text("" + remaningChar);

                        if (remaningChar <= 0) {
                            //document.getElementById('lblComentariosCaracteres').innerHTML = 0;
                            $("#" + labelContador).text("" + 0);
                            obj.value = obj.value.substring(maxchar, 0);
                            return false;
                        }
                        else { return true; }
                    }

<%--                $(document).ready(function () {
                    $('#<%=rdbExisteIrregOPI.ClientID%> input[type=radio]:checked').change(function () {
                        alert("Hola");

                      alert(this.val())
                    });
                });--%>

                function MostrarOcultar(valida) {
                    if (valida == 1) {
                        $('#<%=IDJustNOIrreg.ClientID%>').hide();
                }
                else {
                    $('#<%=IDJustNOIrreg.ClientID%>').show();
                    }
                }

                function Valida() {
                    if ($('#<%=rdbExisteIrregOPI.ClientID%> input[type=radio]:checked').val() == "1") {
                    $('#<%=IDJustNOIrreg.ClientID%>').hide();
                }
                else {
                    $('#<%=IDJustNOIrreg.ClientID%>').show();
                }

            }
        </script>

        <div id="ExisteIrreg" runat="server">
            <table width="95%" style="align-content: center">
                <tr>
                    <td width="15%">
                        <asp:Label ID="LblExiste" runat="server" Text="¿Existe una posible Irregularidad?" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="LblEspacio1" runat="server" Text=" " CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="LblAsterisco1" runat="server" Style="color: red; font-size: 9pt" Text="*"></asp:Label>
                    </td>
                    <td width="85%">
                        <asp:RadioButtonList ID="rdbExisteIrregOPI" runat="server" RepeatDirection="Horizontal" OnChange="Valida()" CssClass="txt_gral">
                            <asp:ListItem Value="1">Si</asp:ListItem>
                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                        </asp:RadioButtonList>
                        <%--                        <input type="radio"  CssClass="txt_gral" name="rdbExisteIrregOPI" id="rdbExisteIrregOPI1" value="SI" onclick='ActionJustNoIrreg();'><span class="txt_gral">SI</span>
	                    <input type="radio"  CssClass="txt_gral" name="rdbExisteIrregOPI" id="rdbExisteIrregOPI2" value="NO" checked="checked"  onclick='ActionJustNoIrreg();'><span class="txt_gral">NO</span>--%>
                        <asp:TextBox ID="txtExisteIrregOPI" runat="server" Visible="false" Width="250px" CssClass="txt_gral"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>

        <div id="IDJustNOIrreg" runat="server">
            <table width="95%">
                <tr runat="server" clientidmode="Static">
                    <td style="text-align: left;" class="auto-style1" width="15%">
                        <asp:Label ID="LblJustificacion" runat="server" Text="Justificación de No irregularidad:" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label1" runat="server" Text=" " CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="LblAsterisco2" runat="server" Style="color: red; font-size: 9pt" Text="*"></asp:Label>
                    </td>
                    <td style="text-align: left;" class="auto-style2" width="85%" rowspan="2">
                        <asp:TextBox ID="txtJustificacionOPI" runat="server" onkeypress="ReemplazaCEspeciales(this.id)"
                            onblur="ReemplazaCEspeciales(this.id)" onfocus="ReemplazaCEspeciales(this.id)"
                            onkeyup="validaLimiteLongitudJust(this,250,'lblJustificacionCaracteres')"
                            TextMode="MultiLine" Width="100%" MaxLength="250" Height="70px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4" colspan="2"></td>
                </tr>
                <tr id="ObjCaracteres" runat="server" clientidmode="Static">
                    <td style="width: 15%; text-align: left;">&nbsp;</td>
                    <td style="width: 85%; text-align: right;">
                        <div id="divConttxtObjetoOPI" runat="server" style="width: 100%; text-align: right; float: left;">
                            <asp:Label runat="server" ID="lblAsterico" CssClass="AsteriscoHide" Style="color: red; font-size: 9pt" Text="*"></asp:Label>
                            <asp:Label runat="server" ID="lblDescripcionContadorJust" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                            <asp:Label runat="server" ID="lblJustificacionCaracteres" CssClass="txt_gral" Text="250" ClientIDMode="Static"></asp:Label>
                        </div>
                    </td>
                </tr>

            </table>
        </div>

        <div id="IDIrregStd" runat="server">
            <table style="align-content: center; width: 95%">
                <tr>
                    <td style="height: 35px; width: 15%; text-align: left;">
                        <asp:Label ID="Label2" runat="server" Text="¿La posible irregularidad es estándar?" CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label3" runat="server" Text=" " CssClass="txt_gral"></asp:Label>
                        <asp:Label ID="Label4" runat="server" Text="*" Style="color: red; font-size: 9pt"></asp:Label>
                    </td>
                    <td style="height: 35px; width: 85%; text-align: left;">
                        <div id="DivRadioButtonOPI2" runat="server">
                            <asp:RadioButtonList ID="rdbIrregStandardOPI" runat="server" CssClass="txt_gral"
                                RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">SI</asp:ListItem>
                                <asp:ListItem Value="0" Selected="True">NO</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
