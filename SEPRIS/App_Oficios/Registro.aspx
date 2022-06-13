<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Registro.aspx.vb" ValidateRequest="false"
    Inherits="SICOD.RegistroOficios" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="~/Styles.css" />
    <style type="text/css">
        TH
        {
            color: #fff;
        }
        .style11
        {
            width: 842px;
        }
        .style14
        {
            width: 150px;
        }
        .style19
        {
            width: 530px;
        }
        .style25
        {
            width: 77px;
        }
        .style26
        {
            width: 100%;
        }
        .style27
        {
            width: 183px;
        }
        .style28
        {
            width: 172px;
        }
        .style32
        {
            width: 22px;
        }
        .style33
        {
            width: 128px;
        }
        .style34
        {
            width: 127px;
        }
        .style35
        {
            width: 145px;
        }
        .style36
        {
            width: 33px;
        }
        .style40
        {
            width: 26px;
        }
        .style41
        {
            width: 133px;
        }
        .style42
        {
            width: 174px;
        }
        .BordeSuperior
        {
            border-top-style: ridge;
        }
        .BordeInferior
        {
            border-bottom-style: ridge;
        }
        .BordeDerecho
        {
            border-right-style: ridge;
        }
        .BordeIzquierdo
        {
            border-left-style: ridge;
        }
    </style>
    <script type="text/javascript" language="javascript" src="/Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/json2.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        //AGC Validar que al presionar la tecla "Backspace"
        document.onkeydown = function () {
            activeObj = document.activeElement;
            if (activeObj.tagName != "INPUT" && activeObj.tagName != "TEXTAREA") {
                if (event.keyCode === 8) {
                    return false;
                }
            }
        }

        function setACEValueKey(source, eventArgs) {
            document.getElementById("destinatarioKey").value = eventArgs.get_value();
            document.getElementById("destinatarioText").value = document.getElementById("txtDestinatario").value
        }

        function validaLimite(obj, maxchar) {

            if (this.id) obj = this;

            var remaningChar = maxchar - obj.value.length;
            document.getElementById('lblAsuntoCaracteres').innerHTML = remaningChar;

            if (remaningChar <= 0) {
                document.getElementById('lblAsuntoCaracteres').innerHTML = 0;
                obj.value = obj.value.substring(maxchar, 0);
                return false;
            }
            else
            { return true; }

        }

        function validaLimite2(obj, maxchar) {

            if (this.id) obj = this;

            var remaningChar = maxchar - obj.value.length;
            document.getElementById('lblAsuntoCaracteres2').innerHTML = remaningChar;

            if (remaningChar <= 0) {
                document.getElementById('lblAsuntoCaracteres2').innerHTML = 0;
                obj.value = obj.value.substring(maxchar, 0);
                //alert('Character Limit exceeds!');
                return false;
            }
            else
            { return true; }

        }

        function validaLimite3(obj, maxchar) {

            if (this.id) obj = this;

            var remaningChar = maxchar - obj.value.length;
            document.getElementById('lblCarRestantesCancelacion').innerHTML = remaningChar;

            if (remaningChar <= 0) {
                document.getElementById('lblCarRestantesCancelacion').innerHTML = 0;
                obj.value = obj.value.substring(maxchar, 0);
                //alert('Character Limit exceeds!');
                return false;
            }
            else
            { return true; }

        }

        function keepDisplayedValidators(event) {
            $('#displayedValidators')[0].value = '';
            var Inline = '';
            var separadorInline = '';
            var Visible = '';
            var separadorVisible = '';
            $('span[validationGroup][controltovalidate]').each(function (index, element) {
                if ($(element).attr("controltovalidate") != null) {
                    if ($(element).attr("controlToValidate").value != event.srcElement.id) {
                        if (element.style.display == 'inline') {
                            Inline += separadorInline + '#' + element.id;
                            separadorInline = ', ';
                        }
                    }
                    if (element.style.visibility == 'visible') {
                        Visible += separadorVisible + '#' + element.id;
                        separadorVisible = ', ';
                    }
                }
                else {
                    if (element.style.display == 'inline') {
                        Inline += separadorInline + '#' + element.id;
                        separadorInline = ', ';
                    }
                    if (element.style.visibility == 'visible') {
                        Visible += separadorVisible + '#' + element.id;
                        separadorVisible = ', ';
                    }
                }
            });

            $('#displayedValidators')[0].value = Inline + '>-<' + Visible;
        }

        function restoreDisplayedValidators() {
            $($('#displayedValidators')[0].value.split('>-<')[0]).css('display', 'inline');
            $($('#displayedValidators')[0].value.split('>-<')[1]).css('display', '');
            $($('#displayedValidators')[0].value.split('>-<')[1]).css('visibility', 'visible');
            inicializaEventos();
        }

        function setHourglass() {
            $('#Container').css('cursor', 'wait');
        }


        function ShowProcesa() {

            $find('mpeProcesa').show();
            return true;

        }

        function Cierrame() {

            window.close();

        }




       
    </script>
</head>
<body>
    <!-- div necesaria para poner el cursor en 'wait', ver wait.js -->
    <div id="Container">
        <form id="frmRegistroOficio" runat="server" accept="tipos mime">
        <script type="text/javascript" language="javascript" src="/Scripts/wait.js"></script>
        <script type="text/javascript" language="javascript">


            var prm2 = Sys.WebForms.PageRequestManager.getInstance();
            prm2.add_endRequest(EndRequestInline);
            function EndRequestInline(sender, args) {
                $('#<%= txtPlazo.ClientID %>').blur(function () {
                    var plazoDiasVar = $('#<%= txtPlazo.ClientID %>').val();
                    var fechaAcuseVar = $('#<%= txtFechaAcuse.ClientID %>').val();
                    $.ajax({
                        type: "POST",
                        url: "Registro.aspx/GetPlazoDate",
                        data: JSON.stringify({ plazoDias: plazoDiasVar, fechaAcuse: fechaAcuseVar }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            if (msg.d[0] != null && msg.d[0] != undefined && msg.d[0] != '') {
                                $('#<%= txtFechaVencimiento.ClientID %>').val(msg.d[0]);
                            }

                            if (msg.d[1] != null && msg.d[1] != undefined && msg.d[1] != '') {
                                $('#<%= txtFechaAcuse.ClientID %>').val(msg.d[1]);
                            }
                        }
                    });
                });
            }

            function closePopupCedula() {
                $find('ModalCedula').hide();
            }


            function ValidaImgWord() {


                if (document.getElementById('<%= ddlArea.ClientID %>').value == '-1') {

                    document.getElementById('<%= lblErroresTitulo.ClientID %>').style.display = 'block';
                    document.getElementById('<%= lblErroresPopup.ClientID %>').style.display = 'block';
                    document.getElementById('<%= lblErroresPopup.ClientID %>').innerText = 'Debe seleccionar el Área';
                    document.getElementById('<%= lblErroresTitulo.ClientID %>').innerText = 'Atención';

                    document.getElementById('<%= BtnModalOk.ClientID %>').style.display = 'none';
                    document.getElementById('<%= BtnCancelarModal.ClientID %>').style.display = 'block';
                    document.getElementById('<%= BtnCancelarModal.ClientID %>').innerText = 'Cancelar';

                    $find('ModalPopupExtenderErrores').show();

                    return false;
                }


                return true;

            }


            function click_Archivo() {
                document.getElementById('linkbotonArchivo').click();
            }


            function UploadError(sender, args) {

                document.getElementById('<%= lblErroresTitulo.ClientID %>').style.display = 'block';
                document.getElementById('<%= lblErroresPopup.ClientID %>').style.display = 'block';
                document.getElementById('<%= lblErroresPopup.ClientID %>').innerHTML = '<ul><li>Verifique que el tamaño del archivo sea menor a 15MB</li></ul>';
                document.getElementById('<%= lblErroresTitulo.ClientID %>').innerText = 'Error';

                document.getElementById('<%= BtnModalOk.ClientID %>').style.display = 'none';
                document.getElementById('<%= BtnCancelarModal.ClientID %>').style.display = 'block';
                document.getElementById('<%= BtnCancelarModal.ClientID %>').innerText = 'Cancelar';

                $find('mpeProcesa').hide();
                $find('ModalPopupExtenderErrores').show();

            }

            function UploadStart(sender, args) {

                $find('mpeProcesa').show();

            }

            function UploadFinish(sender, args) {

                $find('mpeProcesa').hide();

            }



        </script>
        <div>
            <asp:UpdatePanel ID="updPanel" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarGenerarOficios" />
                    <asp:AsyncPostBackTrigger ControlID="chkMultiplesAfores" />
                    <asp:PostBackTrigger ControlID="gvNumerosOficios" />
                    <asp:PostBackTrigger ControlID="lnkDictamenRelacionado" />
                    <asp:AsyncPostBackTrigger ControlID="lnkOficioExternoRelacionado" />
                    <asp:PostBackTrigger ControlID="btnAceptarCedula" />
                    <asp:AsyncPostBackTrigger ControlID="dgMultiplesAfores" />
                </Triggers>
                <ContentTemplate>
                    <br />
                    <center>
                        <asp:Label runat="server" ID="lblTitulo" Text="Alta de Nuevo Documento" CssClass="TitulosWebProyectos"></asp:Label>
                    </center>
                    <br />
                    <asp:ToolkitScriptManager ID="Toolkitscriptmanager1" runat="server" EnableScriptGlobalization="true"
                        AsyncPostBackTimeout="7200">
                    </asp:ToolkitScriptManager>
                    <asp:Panel ID="pnlBotonesImagen" runat="server" Visible="false" Style="text-align: center">
                        <center>
                            <table width="450px" style="text-align: center">
                                <tr>
                                    <td style="text-align: center; width: 32px">
                                        <asp:ImageButton ID="iBtnNuevo" runat="server" ImageUrl="~/imagenes/nuevo02.jpg"
                                            Height="32px" Width="32px" ToolTip="Nuevo" TabIndex="40" />
                                    </td>
                                    <td style="width: 20px">
                                    </td>
                                    <td style="text-align: center; width: 32px">
                                        <asp:ImageButton ID="iBtnBitacora" runat="server" ImageUrl="~/imagenes/Actualizar.bmp"
                                            Height="32px" Width="32px" ToolTip="Historial" TabIndex="39" />
                                    </td>
                                    <td style="width: 20px">
                                    </td>
                                    <td style="text-align: center; width: 32px">
                                        <asp:ImageButton ID="iBtnAdjuntarDocumentos" runat="server" ImageUrl="~/imagenes/archivo_01.jpg"
                                            Height="32px" Width="32px" ToolTip="Adjuntar Documentos" TabIndex="41" />
                                    </td>
                                    <td style="width: 20px">
                                    </td>
                                    <td style="text-align: center; width: 32px">
                                        <asp:ImageButton ID="iBtnSeguimiento" runat="server" ImageUrl="~/imagenes/seguimiento.ico"
                                            Height="32px" Width="32px" ToolTip="Seguimiento no disponible, documento no concluído"
                                            TabIndex="42" Enabled="false" />
                                    </td>
                                    <td style="width: 20px">
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Button runat="server" ID="btnEnviar" CssClass="botones" Text="ENVIAR" Height="28px"
                                            Width="100px" onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                            ToolTip="Enviar por Oficialía" />
                                    </td>
                                    <td style="width: 20px; display: none">
                                    </td>
                                    <td style="text-align: center; width: 32px; display: none">
                                        <asp:ImageButton ID="iBtnCedula" runat="server" ImageUrl="~/imagenes/pdf01.jpg" Height="32px"
                                            Width="32px" ToolTip="Generar Cédula PDF" TabIndex="43" Visible="false" />
                                    </td>
                                    <td style="width: 20px; display: none">
                                    </td>
                                    <td style="text-align: center; width: 32px; display: none">
                                        <asp:ImageButton ID="iBtnEnviarNotificacion" runat="server" ImageUrl="~/imagenes/email_01.jpg"
                                            Height="32px" Width="32px" ToolTip="Enviar Notificación" TabIndex="44" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>
                    <br />
                    <asp:Panel runat="server" ID="pnlNumerosOficiosMultiplesAfores" Visible="false">
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gvNumerosOficios" runat="server" AutoGenerateColumns="False" BorderColor="White"
                                        CellPadding="1" Font-Name="Arial" Font-Names="Arial" Font-Size="9pt" ForeColor="#555555"
                                        Height="64px" HorizontalAlign="Center" Width="335px">
                                        <HeaderStyle CssClass="GridViewHeaderOficios" Font-Bold="True" ForeColor="#FFFFFF"
                                            HorizontalAlign="Center" VerticalAlign="NotSet" />
                                        <Columns>
                                            <asp:BoundField DataField="T_ENTIDAD_CORTO" HeaderStyle-CssClass="BO_Column" HeaderText="Afore"
                                                ItemStyle-HorizontalAlign="Center" SortExpression="T_ENTIDAD_CORTO" />
                                            <asp:BoundField DataField="T_OFICIO_NUMERO" HeaderStyle-CssClass="BO_Column" HeaderText="Número Oficio"
                                                ItemStyle-HorizontalAlign="Center" SortExpression="T_OFICIO_NUMERO" />
                                            <asp:ButtonField ButtonType="Link" CommandName="Visualizar" ControlStyle-ForeColor="Blue"
                                                DataTextField="T_HYP_ARCHIVOWORD" HeaderStyle-CssClass="BO_Column" HeaderText="Archivo Word"
                                                ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlArea">
                        <table width="100%" border="0">
                            <tr>
                                <td class="style14">
                                </td>
                                <td class="style11">
                                </td>
                                <td align="left" rowspan="3">
                                    <table width=" 100%" runat="server" id="TablaNumOficio" style="border-bottom-style: ridge;
                                        border-top-style: ridge; border-left-style: ridge; border-right-style: ridge">
                                        <tr>
                                            <td colspan="2" align="center" valign="top">
                                                <asp:Label ID="lblNumeroOficioTag" runat="server" Text="Número Oficio:" CssClass="txt_gral"></asp:Label>
                                                &nbsp;
                                                <asp:Label ID="lblNumeroOficio" runat="server" Style="width: 215px; font-size: 1.2em;
                                                    color: #000; font-weight: bold;" CssClass="txt_gral"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblEstatus" runat="server" CssClass="txt_gral" Text="* Estatus:&nbsp;"
                                                    Width="100px"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlEstatus" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                                    TabIndex="6" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trMultiplesAfores" runat="server">
                                <td class="style14">
                                    &nbsp;&nbsp;<asp:Label ID="lblMultiplesAfores" runat="server" CssClass="txt_gral"
                                        Text="Múltiples Afores:" Width="140px"></asp:Label>
                                </td>
                                <td class="style11">
                                    <asp:CheckBox ID="chkMultiplesAfores" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                        Text="" TabIndex="1" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style14">
                                    <asp:Label ID="lblTipoDocumento" runat="server" CssClass="txt_gral" Text="* Tipo de documento:"
                                        Width="140px"></asp:Label>
                                </td>
                                <td class="style11">
                                    <asp:DropDownList ID="ddlTipoDocumento" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                        TabIndex="2" Width="300px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="tr_EstructuraArea">
                                <td class="style14">
                                    &nbsp;
                                </td>
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rblEstructuraArea" runat="server" AutoPostBack="True" CssClass="txt_gral"
                                        RepeatDirection="Horizontal" TabIndex="3" Visible="false">
                                        <asp:ListItem Value="2" Selected="True">Estructura Funcional</asp:ListItem>
                                        <asp:ListItem Value="1">Estructura Oficial</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style14">
                                    <asp:Label ID="lblArea" runat="server" CssClass="txt_gral" Text="* Área:"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlArea" runat="server" Width="99%" AutoPostBack="true" CssClass="txt_gral"
                                        TabIndex="5">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style14">
                                    <asp:Label ID="lblPrioridad" runat="server" CssClass="txt_gral" Text="* Prioridad:"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlPrioridad" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                        TabIndex="8" Width="300px">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblAño" runat="server" CssClass="txt_gral" Text="* Año:&nbsp;"></asp:Label>
                                    <asp:DropDownList ID="ddlAño" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                        TabIndex="7" Width="105px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlDatosBasicos" runat="server">
                        <table width="100%" border="0">
                            <tr>
                                <td class="style14">
                                    <asp:Label ID="lblTipoEntidad" runat="server" CssClass="txt_gral" Text="* Tipo entidad destino:"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlTipoEntidad" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                        TabIndex="9" Width="300px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style14">
                                    <asp:Label ID="lblEntidad" runat="server" Text="* Entidad destino:" CssClass="txt_gral"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlEntidad" runat="server" Width="500px" AutoPostBack="true"
                                        CssClass="txt_gral" TabIndex="10">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="rowSubEntidad" runat="server" style="display: none">
                                <td class="style14">
                                    &nbsp;&nbsp;<asp:Label ID="lblSubentidad" runat="server" Text="Subentidad:" CssClass="txt_gral"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlSubentidad" runat="server" Width="500px" AutoPostBack="true"
                                        CssClass="txt_gral" TabIndex="11" Enabled="False">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style14">
                                    <asp:Label ID="lblCargoDestinatario" Width="150px" runat="server" Text="* Cargo del destinatario:"
                                        CssClass="txt_gral" Height="16px"></asp:Label>
                                </td>
                                <td style="width: 500px">
                                    <asp:DropDownList ID="ddlCargoDestinatario" runat="server" Width="300px" CssClass="txt_gral"
                                        TabIndex="12">
                                    </asp:DropDownList>
                                </td>
                                <td align="right" style="width: 120px">
                                    <asp:Label ID="lblDestinatario" runat="server" Text="* Destinatario:" CssClass="txt_gral"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDestinatario" runat="server" Width="95%" CssClass="txt_aceList"
                                        TabIndex="13" Style="text-align: left"></asp:TextBox>
                                    <input id="destinatarioKey" name="destinatarioKey" runat="server" type="hidden" />
                                    <input id="destinatarioText" name="destinatarioText" runat="server" type="hidden" />
                                    <asp:AutoCompleteExtender ID="aceDestinatario" TargetControlID="txtDestinatario"
                                        ServiceMethod="ObtenerDestinatarioOficios" MinimumPrefixLength="1" runat="server"
                                        CompletionInterval="100" CompletionListItemCssClass="AutocompleteListElement"
                                        CompletionListHighlightedItemCssClass="Element" UseContextKey="True" OnClientItemSelected="setACEValueKey">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlFechas" runat="server">
                        <table width="100%" border="0">
                            <tr>
                                <td class="style14">
                                    <asp:Label ID="lblFechaDocumento" runat="server" Text="* Fecha Documento:" CssClass="txt_gral"
                                        Width="140px"></asp:Label>
                                </td>
                                <td>
                                    <asp:CalendarExtender runat="server" ID="calFechaDocumento" TargetControlID="txtFechaDocumento"
                                        Format="dd/MM/yyyy" PopupButtonID="imgCalFechaDocumento">
                                    </asp:CalendarExtender>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:CalendarExtender ID="calFechaAcuse" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgCalFechaAcuse"
                                        TargetControlID="txtFechaAcuse">
                                    </asp:CalendarExtender>
                                    <table class="style26">
                                        <tr>
                                            <td class="style35">
                                                <asp:TextBox ID="txtFechaDocumento" runat="server" CssClass="txt_gral" Style="width: 135px"
                                                    TabIndex="14"></asp:TextBox>
                                            </td>
                                            <td class="style36">
                                                <asp:ImageButton ID="imgCalFechaDocumento" runat="server" CausesValidation="false"
                                                    ImageAlign="Bottom" ImageUrl="~/imagenes/Calendar.gif" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorFechaDocumento" runat="server"
                                                    ControlToValidate="txtFechaDocumento" Display="Dynamic" ErrorMessage="La Fecha Documento debe ser válida con el formato DD/MM/AAAA"
                                                    ForeColor="Red" Text="*" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                    ValidationGroup="Forma"></asp:RegularExpressionValidator>
                                            </td>
                                            <td class="style28" align="right">
                                                <asp:Label ID="lblFechaAcuse" runat="server" CssClass="txt_gral" Text="Fecha Acuse:&nbsp;&nbsp;"></asp:Label>
                                            </td>
                                            <td class="style41">
                                                <asp:TextBox ID="txtFechaAcuse" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                                    Style="width: 115px" TabIndex="15"></asp:TextBox>
                                            </td>
                                            <td class="style32">
                                                <asp:ImageButton ID="imgCalFechaAcuse" runat="server" CausesValidation="false" ImageAlign="Bottom"
                                                    ImageUrl="~/imagenes/Calendar.gif" />
                                            </td>
                                            <td class="style33" align="right">
                                                <asp:Label ID="lblSeDaPlazo" runat="server" CssClass="txt_gral" Text="¿Se da algún plazo?:&nbsp;"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkSeDaPlazo" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                                    TabIndex="16" Text="" Width="127px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlFechasPlazo" runat="server" Visible="false">
                        <table width="100%" border="0">
                            <tr>
                                <td class="style14">
                                    &nbsp;&nbsp;<asp:Label ID="lblPlazo" runat="server" Text="Plazo en días:" CssClass="txt_gral"></asp:Label>
                                </td>
                                <td class="style27">
                                    <asp:TextBox ID="txtPlazo" runat="server" Style="width: 80px" CssClass="txt_gral"
                                        TabIndex="17"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="fte" runat="server" TargetControlID="txtPlazo" FilterMode="ValidChars"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="style42">
                                    &nbsp;<asp:Label ID="lblFechaVencimiento" runat="server" CssClass="txt_gral" Text="Fecha de vencimiento:"></asp:Label>
                                </td>
                                <td class="style41">
                                    <asp:TextBox ID="txtFechaVencimiento" runat="server" CssClass="txt_gral" Style="width: 115px"
                                        TabIndex="18"></asp:TextBox>
                                </td>
                                <td class="style40">
                                    <asp:ImageButton CausesValidation="false" ImageAlign="Bottom" ID="imgCalFechaVencimiento"
                                        runat="server" ImageUrl="~/imagenes/Calendar.gif" />
                                    <asp:CalendarExtender runat="server" ID="calFechaVencimiento" TargetControlID="txtFechaVencimiento"
                                        Format="dd/MM/yyyy" PopupButtonID="imgCalFechaVencimiento">
                                    </asp:CalendarExtender>
                                </td>
                                <td class="style34">
                                    <asp:Label ID="lblFechaRecepcion" runat="server" Text="Fecha de respuesta:" CssClass="txt_gral"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaRecepcion" runat="server" Style="width: 115px" CssClass="txt_gral"
                                        TabIndex="19"></asp:TextBox>
                                    <asp:ImageButton CausesValidation="false" ImageAlign="Bottom" ID="imgCalFechaRecepcion"
                                        runat="server" ImageUrl="~/imagenes/Calendar.gif" />
                                    <asp:CalendarExtender runat="server" ID="calFechaRecepcion" TargetControlID="txtFechaRecepcion"
                                        Format="dd/MM/yyyy" PopupButtonID="imgCalFechaRecepcion">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlClasificacion" runat="server">
                        <table width="100%" border="0">
                            <tr>
                                <td class="style14">
                                    <asp:Label ID="lblClasificacion" runat="server" CssClass="txt_gral" Text="* Clasificación:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlClasificacion" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                        TabIndex="20" Width="99%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlAsunto" runat="server">
                        <table width="100%" border="0">
                            <tr>
                                <td class="style14">
                                    <asp:Label ID="lblAsunto" runat="server" Text="* Asunto:" CssClass="txt_gral"></asp:Label>
                                    &nbsp;&nbsp
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAsunto" runat="server" CssClass="txt_gral" Height="35px" TextMode="MultiLine"
                                        MaxLength="255" onkeyup="validaLimite(this,255)" TabIndex="21" Width="99%"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorAsunto" runat="server"
                                        ErrorMessage="El Asunto no debe exceder 255 caracteres" ControlToValidate="txtAsunto"
                                        Text="*" ValidationGroup="Forma" ValidationExpression="^[\s\S]{0,255}$" Display="Dynamic"
                                        ForeColor="Red"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style14">
                                </td>
                                <td style="text-align: right; padding-right: 2%;">
                                    <asp:Label runat="server" ID="lblCarAsunto" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                                    <asp:Label runat="server" ID="lblAsuntoCaracteres" CssClass="txt_gral" Text="255"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style14">
                                    &nbsp;&nbsp;<asp:Label ID="lblComentarios" runat="server" Text="Comentarios:" CssClass="txt_gral"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtComentarios" runat="server" Style="width: 99%" CssClass="txt_gral"
                                        Height="35px" TextMode="MultiLine" MaxLength="255" onkeyup="validaLimite2(this,255)"
                                        TabIndex="22"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorComentarios" runat="server"
                                        ErrorMessage="Los comentarios no deben exceder 255 caracteres" ControlToValidate="txtComentarios"
                                        Text="*" ValidationGroup="Forma" ValidationExpression="^[\s\S]{0,255}$" Display="Dynamic"
                                        ForeColor="Red"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style14">
                                </td>
                                <td style="text-align: right; padding-right: 2%;">
                                    <asp:Label runat="server" ID="lblCarAsunto2" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                                    <asp:Label runat="server" ID="lblAsuntoCaracteres2" CssClass="txt_gral" Text="255"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlFirmas" runat="server">
                        <table width="100%" cellspacing="0">
                            <tr>
                                <td colspan="5">
                                </td>
                            </tr>
                            <tr>
                                <td class="BordeIzquierdo BordeSuperior">
                                    <asp:Label ID="Label4" runat="server" Text="* Firmado por SIE" CssClass="txt_gral"
                                        Width="150px"></asp:Label>
                                </td>
                                <td colspan="3" class="BordeSuperior">
                                    <asp:RadioButtonList ID="rblFirmaSIE" runat="server" AutoPostBack="True" CssClass="txt_gral"
                                        RepeatDirection="Horizontal" TabIndex="24" Width="295px">
                                        <asp:ListItem Value="1" Selected="True">SI</asp:ListItem>
                                        <asp:ListItem Value="0">NO</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td class="BordeSuperior BordeDerecho">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr runat="server" id="tr_EstructuraFirmas" visible="false">
                                <td class="BordeIzquierdo">
                                    &nbsp;
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblEstructuraFirmas" runat="server" AutoPostBack="True"
                                        CssClass="txt_gral" RepeatDirection="Horizontal" TabIndex="24">
                                        <asp:ListItem Value="2" Selected="True">Estructura Funcional</asp:ListItem>
                                        <asp:ListItem Value="1">Estructura Oficial</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td class="BordeDerecho">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style14 BordeIzquierdo">
                                    <asp:Label ID="Label2" runat="server" Text="* Firmas" CssClass="txt_gral" Width="140px"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlAreaFirmas" runat="server" Width="100%" AutoPostBack="true"
                                        CssClass="txt_gral" TabIndex="25">
                                    </asp:DropDownList>
                                </td>
                                <td class="BordeDerecho">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="BordeIzquierdo BordeInferior">
                                    &nbsp;
                                </td>
                                <td colspan="3" class="BordeInferior">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 45%">
                                                <asp:Label ID="lblUsuarios" runat="server" Text="Disponibles:" CssClass="txt_gral"></asp:Label>
                                            </td>
                                            <td style="width: 10%">
                                                &nbsp;
                                            </td>
                                            <td style="width: 45%">
                                                &nbsp;
                                                <asp:Label ID="lblFirmas" runat="server" CssClass="txt_gral" Text="Seleccionados:"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td rowspan="2" valign="top" style="text-align: right">
                                                <asp:ListBox ID="lstUsuariosFirma" runat="server" Height="68px" Width="100%" CssClass="txt_gral"
                                                    TabIndex="23"></asp:ListBox>
                                            </td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="btnAgregarFirma" runat="server" ImageUrl="~/imagenes/FlechaRojaDer.gif"
                                                    Width="30px" Height="30px" TabIndex="22" />
                                            </td>
                                            <td rowspan="2" valign="top">
                                                <asp:ListBox ID="lstFirmas" runat="server" Height="68px" Width="100%" CssClass="txt_gral">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="btnQuitarFirma" runat="server" ImageUrl="~/imagenes/FlechaRojaIzq.gif"
                                                    Width="30px" Height="30px" TabIndex="23" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="BordeInferior BordeDerecho">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr runat="server" id="tr_EstructuraRubricas" visible="false">
                                <td class="style14 BordeIzquierdo BordeSuperior">
                                    &nbsp;
                                </td>
                                <td colspan="3" class="BordeSuperior">
                                    <asp:RadioButtonList ID="rblEstructuraRubricas" runat="server" AutoPostBack="True"
                                        CssClass="txt_gral" RepeatDirection="Horizontal" TabIndex="24">
                                        <asp:ListItem Value="2" Selected="True">Estructura Funcional</asp:ListItem>
                                        <asp:ListItem Value="1">Estructura Oficial</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td class="BordeSuperior BordeDerecho">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style14 BordeIzquierdo">
                                    <asp:Label ID="lblArea0" runat="server" CssClass="txt_gral" Text="* Rúbricas:" Width="140px"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlAreaRubrica" runat="server" Width="100%" AutoPostBack="true"
                                        CssClass="txt_gral" TabIndex="25">
                                    </asp:DropDownList>
                                </td>
                                <td class="BordeDerecho">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="BordeIzquierdo BordeInferior">
                                    &nbsp;
                                </td>
                                <td colspan="3" class="BordeInferior">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 45%">
                                                <asp:Label ID="lblUsuariosRub" runat="server" Text="Disponibles:" CssClass="txt_gral"></asp:Label>
                                            </td>
                                            <td style="width: 10%">
                                                &nbsp;
                                            </td>
                                            <td style="width: 45%">
                                                &nbsp;
                                                <asp:Label ID="lblRubrica" runat="server" CssClass="txt_gral" Text="Seleccionados:"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="1" rowspan="2" valign="top" style="text-align: right">
                                                <asp:ListBox ID="lstUsuariosRubrica" runat="server" Height="68px" Width="100%" CssClass="txt_gral"
                                                    TabIndex="26"></asp:ListBox>
                                            </td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="btnAgregarRubrica" runat="server" ImageUrl="~/imagenes/FlechaRojaDer.gif"
                                                    Width="30px" Height="30px" TabIndex="25" />
                                            </td>
                                            <td rowspan="2" valign="top">
                                                <asp:ListBox ID="lstRubricas" runat="server" Height="68px" Width="100%" CssClass="txt_gral">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="btnQuitarRubrica" runat="server" ImageUrl="~/imagenes/FlechaRojaIzq.gif"
                                                    Width="30px" Height="30px" TabIndex="26" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="BordeInferior BordeDerecho">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr runat="server" id="tr_EstructuraElaboro" visible="false">
                                <td class="style14 BordeIzquierdo BordeSuperior">
                                    &nbsp;
                                </td>
                                <td colspan="4" class="BordeSuperior BordeDerecho">
                                    <asp:RadioButtonList ID="rblEstructuraElaboro" runat="server" AutoPostBack="True"
                                        CssClass="txt_gral" RepeatDirection="Horizontal" TabIndex="27">
                                        <asp:ListItem Value="2" Selected="True">Estructura Funcional</asp:ListItem>
                                        <asp:ListItem Value="1">Estructura Oficial</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style14 BordeIzquierdo">
                                    <asp:Label ID="lblElaboro" runat="server" CssClass="txt_gral" Text="* Elaboró:" Width="140px"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlAreaElaboro" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                        TabIndex="28" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td class="BordeDerecho">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="BordeIzquierdo BordeInferior">
                                    &nbsp;
                                </td>
                                <td valign="middle" colspan="3" class="BordeInferior">
                                    <asp:DropDownList ID="ddlUsuarioElaboro" runat="server" CssClass="txt_gral" TabIndex="29"
                                        Width="45%">
                                    </asp:DropDownList>
                                </td>
                                <td class="BordeDerecho BordeInferior">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlDocRelacionado" runat="server">
                        <table width="100%">
                            <caption>
                                &nbsp;&nbsp;<tr>
                                    <td class="style14">
                                        &nbsp;&nbsp;<asp:Label ID="lblIncumplimiento" runat="server" CssClass="txt_gral"
                                            Text="Incumplimiento:" Visible="False"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlIncumplimiento" runat="server" CssClass="txt_gral" TabIndex="30"
                                            Visible="False" Width="98%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </caption>
                            </tr>
                            <tr>
                                <td class="style14">
                                </td>
                                <td>
                                    <asp:Panel ID="pnlLnkDictamen" runat="server" Visible="false">
                                        <asp:CheckBox ID="chkDictaminado" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                            TabIndex="31" Text="Dictaminado" TextAlign="Left" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnDeleteDictamen" runat="server" Height="16px" ImageUrl="~/imagenes/Delete.png"
                                            Visible="false" TabIndex="32" />
                                        <asp:LinkButton ID="lnkDictamenRelacionado" runat="server" CssClass="txt_gral" Text="lnkDictaminado"
                                            TabIndex="33"></asp:LinkButton>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="style14">
                                </td>
                                <td>
                                    <asp:Panel ID="pnlLnkOficioExterno" runat="server" Visible="false">
                                        <asp:Label ID="lblDocRelacionadoDictamen" runat="server" CssClass="txt_gral" Text="Oficio Externo Relacionado:"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnDeleteOficioExterno" runat="server" Height="16px" ImageUrl="~/imagenes/Delete.png"
                                            Visible="false" TabIndex="34" />
                                        <asp:LinkButton ID="lnkOficioExternoRelacionado" runat="server" CssClass="txt_gral"
                                            Text="Buscar" TabIndex="35"></asp:LinkButton>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlComentariosCancelacion" runat="server" Visible="false">
                        <table width="100%">
                            <tr>
                                <td class="style14">
                                    <asp:Label ID="lblComentariosCancelacion" runat="server" Text="Descripción Cancelación:"
                                        CssClass="txt_gral"></asp:Label>
                                    &nbsp;&nbsp
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCancelacion" runat="server" Style="width: 99%" CssClass="txt_gral"
                                        Height="35px" TextMode="MultiLine" MaxLength="250" onkeyup="validaLimite3(this,250)"
                                        TabIndex="36"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorCancelacion" runat="server"
                                        ErrorMessage="Los comentarios no deben exceder 255 caracteres" ControlToValidate="txtCancelacion"
                                        Text="*" ValidationGroup="Forma" ValidationExpression="^[\s\S]{0,250}$" Display="Dynamic"
                                        ForeColor="Red"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style14">
                                </td>
                                <td style="text-align: right; padding-right: 2%;">
                                    <asp:Label runat="server" ID="lblCaracteresRestantes" CssClass="txt_gral" Text="Caracteres restantes: "></asp:Label>
                                    <asp:Label runat="server" ID="lblCarRestantesCancelacion" CssClass="txt_gral" Text="250"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlMultipleAfore" runat="server" Visible="false">
                        <table width="100%" border="0">
                            <tr>
                                <td class="style14">
                                    <asp:Label ID="lblDirigido" runat="server" Text="* Dirigido: " CssClass="txt_gral"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlDirigido" runat="server" Width="45%" AutoPostBack="true"
                                        CssClass="txt_gral" TabIndex="37">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="3">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 45%">
                                                <asp:Label ID="lblPersoal" runat="server" Text="Personal:" CssClass="txt_gral"></asp:Label>
                                            </td>
                                            <td style="width: 10%">
                                            </td>
                                            <td style="width: 45%">
                                                <asp:Label ID="lblConCopia" runat="server" Text="Con copia:" CssClass="txt_gral"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td rowspan="2" valign="top" style="text-align: right">
                                                <asp:ListBox ID="lstPersonal" runat="server" Height="68px" Width="100%" CssClass="txt_gral"
                                                    TabIndex="38"></asp:ListBox>
                                            </td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="btnAgregarConCopia" runat="server" ImageUrl="~/imagenes/FlechaRojaDer.gif"
                                                    Width="30px" Height="30px" TabIndex="32" />
                                            </td>
                                            <td rowspan="2" valign="top">
                                                <asp:ListBox ID="lstConCopia" runat="server" Height="68px" Width="100%" CssClass="txt_gral">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="btnQuitarConCopia" runat="server" ImageUrl="~/imagenes/FlechaRojaIzq.gif"
                                                    Width="30px" Height="30px" TabIndex="33" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="style14">
                                </td>
                                <td colspan="3" align="left">
                                    <asp:DataGrid ID="dgMultiplesAfores" runat="server" AutoGenerateColumns="False" Height="64px"
                                        Width="100%" Font-Size="7.5pt" BackColor="#EEEEEE" CellPadding="1" Font-Name="Arial"
                                        Font-Names="Arial" ForeColor="#555555" BorderColor="White" TabIndex="39">
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" BackColor="#236B7C" VerticalAlign="NotSet"
                                            CssClass="GridViewHeaderOficios" ForeColor="#FFFFFF" Wrap="false"></HeaderStyle>
                                        <AlternatingItemStyle BackColor="White" />
                                        <Columns>
                                            <asp:BoundColumn DataField="ID_ENTIDAD" HeaderText="ID" SortExpression="ID_ENTIDAD"
                                                Visible="False" HeaderStyle-CssClass="BO_Column"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="T_ENTIDAD_CORTO" HeaderText="    Afores    " SortExpression="T_ENTIDAD_CORTO"
                                                HeaderStyle-CssClass="BO_Column" />
                                            <asp:TemplateColumn HeaderText="Selección" HeaderStyle-CssClass="BO_Column">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSeleccion" runat="server" Enabled="True" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAll" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged"
                                                        runat="server" Enabled="true" />
                                                </HeaderTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Oficio Referencia" HeaderStyle-CssClass="BO_Column">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOficioReferencia1" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Oficio Referencia" HeaderStyle-CssClass="BO_Column">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOficioReferencia2" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Fecha Referencia" HeaderStyle-CssClass="BO_Column">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFechaReferencia1" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Fecha Referencia" HeaderStyle-CssClass="BO_Column">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFechaReferencia2" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlGenerarDocumento" runat="server" Style="display: none">
                        <table border="0" width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblFormatoword" Width="285px" runat="server" Text="Abrir formato del oficio en blanco:"
                                        CssClass="txt_gral"></asp:Label>
                                </td>
                                <td>
                                    <asp:ImageButton ID="imgFormatoWord" runat="server" ImageUrl="~/imagenes/word.jpg"
                                        TabIndex="40" OnClientClick="return ValidaImgWord()" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblArchivoCombinar" Width="305px" runat="server" Text="Seleccióna la ruta del archivo a combinar:"
                                        CssClass="txt_gral"></asp:Label>
                                </td>
                                <td style="width: 75%">
                                    <asp:AsyncFileUpload ID="AsyncFileUp" runat="server" TabIndex="41" OnUploadedComplete="AsyncFileUpFinish"
                                        Width="400px" OnClientUploadError="UploadError" OnClientUploadStarted="UploadStart"
                                        OnClientUploadComplete="UploadFinish" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:UpdatePanel runat="server" ID="upDocRelacionadoDictamen">
                        <ContentTemplate>
                            <asp:LinkButton ID="LB2" runat="server" Text="Popup" Style="display: none"></asp:LinkButton>
                            <asp:Panel ID="pnlModalDocRelacionado" runat="server" CssClass="panelVentanaEmergenteBackground"
                                Width="800px">
                                <table style="width: 100%; border-style: solid; border-width: 2px; border-color: White"
                                    cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlModalDocRelacionadoHandle" runat="server" CssClass="panelVentanaEmergenteTitulo"
                                                Width="100%" Height="20px">
                                                <asp:Label runat="server" ID="pnlDocRelacionadoTitulo" Text="Vincular Documento"
                                                    Style="vertical-align: middle; margin-left: 5px;" CssClass="titulo_seccioninterior titulo_popup"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding: 5px; height: 20px;">
                                            <asp:Label runat="server" ID="lblErrorDocRelacionadoDictamen" CssClass="txt_gral txt_gral_o_gris"
                                                Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 5px;">
                                            <span class="txt_gral txt_gral_o_gris" style="text-indent: 10px;">Área</span>
                                        </td>
                                        <td style="padding: 5px;">
                                            <span class="txt_gral txt_gral_o_gris" style="text-indent: 10px;">Año</span>
                                        </td>
                                        <td style="padding: 5px;">
                                            <span class="txt_gral txt_gral_o_gris" style="text-indent: 10px;">Consecutivo</span>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:DropDownList runat="server" Width="480px" AutoPostBack="false" ID="ddlAreaDocRelacionado">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList runat="server" Width="80px" AutoPostBack="false" ID="ddlAnioDocRelacionado">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center">
                                            <asp:TextBox runat="server" ID="txtConsecutivoDocRelacionado" CssClass="txt_gral"
                                                Width="50px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" CssClass="botones" onmouseover="style.backgroundColor='#A9A9A9'"
                                                onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Buscar"
                                                ID="btnBuscarDocRelacionado" UseSubmitBehavior="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding: 5px; height: 20px;">
                                            <asp:Label runat="server" CssClass="txt_gral txt_gral_o_gris" ID="lnkTmpDocRelacionado"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="text-align: center; height: 35px">
                                            <center>
                                                <asp:Button ID="btnDocRelacionadoAceptar" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                                    onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Aceptar"
                                                    CssClass="botones" UseSubmitBehavior="false" Enabled="false" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnDocRelacionadoCancelar" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                                    onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Cancelar"
                                                    CssClass="botones" UseSubmitBehavior="false" />
                                            </center>
                                            <asp:Label ID="lblDocRelacionadoDictamenCallBack" runat="server" Style="display: none"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:ModalPopupExtender ID="modalDocRelacionado" runat="server" PopupControlID="pnlModalDocRelacionado"
                                TargetControlID="LB2" BackgroundCssClass="FondoAplicacion" PopupDragHandleControlID="pnlModalDocRelacionadoHandle">
                            </asp:ModalPopupExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:LinkButton ID="LB1" runat="server" Text="Popup" Style="display: none"></asp:LinkButton>
                    <asp:Panel ID="PanelErrores" runat="server" CssClass="PanelErrores" Width="550px">
                        <table style="width: 100%; border-style: solid; border-width: 2px; border-color: White"
                            cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                    <asp:Panel ID="PanelErroresHandle" runat="server" CssClass="PanelErroresHandle" Width="100%"
                                        Height="20px">
                                        <asp:Label runat="server" ID="lblErroresTitulo" Text="INFORMACIÓN" Style="vertical-align: middle;
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
                                            CssClass="botones" />
                                        <asp:Button ID="BtnCancelarModal" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                            onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Cancelar"
                                            CssClass="botones" UseSubmitBehavior="false" />
                                    </center>
                                    <asp:Label ID="lblModalPostBack" runat="server" Style="display: none"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:ModalPopupExtender ID="ModalPopupExtenderErrores" runat="server" PopupControlID="PanelErrores"
                        TargetControlID="LB1" BackgroundCssClass="FondoAplicacion" PopupDragHandleControlID="PanelErroresHandle">
                    </asp:ModalPopupExtender>
                    <asp:Label runat="server" ID="lblErrores" CssClass="txt_gral_rojo" Visible="false"
                        Style="display: none"></asp:Label>
                    <asp:LinkButton ID="LBCedula" runat="server" Text="Popup" Style="display: none"></asp:LinkButton>
                    <asp:Panel ID="pnlCedula" runat="server" Width="550px" CssClass="panelVentanaEmergenteBackground">
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
                                            <asp:TextBox ID="txtFechaCedula" runat="server" Enabled="false"></asp:TextBox>
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
                                                    Text="+" Width="30px" ForeColor="#FFFFFF" />
                                                <asp:Button ID="btnDisminuirHora" runat="server" BackColor="#696969" Font-Bold="True"
                                                    Width="30px" Text="-" ForeColor="#FFFFFF" />
                                                <asp:TextBox ID="txtHora" runat="server" Width="25px"></asp:TextBox>
                                                <asp:Label ID="lblPuntos" Text=":" ForeColor="Black" Font-Bold="True" runat="server"></asp:Label>
                                                <asp:TextBox ID="txtMin" runat="server" Width="25px"></asp:TextBox>
                                                <asp:Button ID="btnAumentarMinuto" runat="server" Font-Bold="True" Width="30px" Text="+"
                                                    BackColor="#696969" ForeColor="#FFFFFF" />
                                                <asp:Button ID="btnDisminuirMinuto" runat="server" Font-Bold="True" Width="30px"
                                                    Text="-" BackColor="#696969" ForeColor="#FFFFFF" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="btnAceptarCedula" runat="server" CssClass="botones" Height="28px"
                                                    onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                                    Text="Aceptar" ValidationGroup="Forma" Width="92px" OnClientClick="setHourGlass();" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnCerrarModalCedula" runat="server" CssClass="botones" Height="28"
                                                    onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                                    Text="Cancelar" Width="93px" />
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                        </table>
                    </asp:Panel>
                    <asp:ModalPopupExtender ID="ModalCedula" runat="server" PopupControlID="pnlCedula"
                        BackgroundCssClass="FondoAplicacion" PopupDragHandleControlID="PanelTitulo" TargetControlID="LBCedula"
                        CancelControlID="btnCerrarModalCedula">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnlBotones" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td colspan="3">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Label ID="lblRegistroTag" Text="Registró:" runat="server" CssClass="txt_gral"></asp:Label>
                                    <asp:Label ID="lblRegistro" Text="" runat="server" CssClass="txt_gral"></asp:Label>
                                    <asp:Button ID="btnBandeja" runat="server" CssClass="botones" Height="28" Visible="false"
                                        onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                        TabIndex="42" Text="Bandeja" Width="93px" />
                                </td>
                            </tr>
                            <tr align="center" style="height: 10px">
                                <td colspan="3">
                                    <asp:Label ID="Label1" runat="server" Text="* = Selección o Captura Obligatoria"
                                        CssClass="txt_gral"></asp:Label>
                                </td>
                            </tr>
                            <tr align="center">
                                <td style="text-align: center">
                                    <asp:Button ID="btnGuardar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                                        runat="server" Height="28" Width="93px" Text="Guardar" CssClass="botones" ValidationGroup="Forma"
                                        TabIndex="42" OnClientClick="return ShowProcesa()"></asp:Button>
                                </td>
                                <td style="text-align: center">
                                    <asp:Button ID="btnCancelar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                                        runat="server" Height="28" Width="93px" Text="Cancelar" CssClass="botones" ValidationGroup="Forma"
                                        TabIndex="45"></asp:Button>
                                </td>
                                <td style="text-align: center">
                                    <asp:Button ID="btnGuardarGenerarOficios" Visible="false" onmouseover="style.backgroundColor='#A9A9A9'"
                                        onmouseout="style.backgroundColor='#696969'" runat="server" Height="28px" Width="140px"
                                        Text="Guardar y generar número" OnClientClick="return ShowProcesa()" CssClass="botones"
                                        ValidationGroup="Forma" TabIndex="46"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:LinkButton ID="lnkProcesa" runat="server" Text="" Style="display: none"></asp:LinkButton>
                    <asp:Panel ID="pnlProcesando" runat="server" Width="400px" Height="200px" CssClass="FondoManualOut">
                        <asp:LinkButton ID="lnkCierra" runat="server" Text="" Style="display: none"></asp:LinkButton>
                        &nbsp;
                        <img src="../imagenes/carga.gif" alt="" />
                    </asp:Panel>
                    <asp:ModalPopupExtender ID="mpeProcesa" runat="server" PopupControlID="pnlProcesando"
                        BackgroundCssClass="FondoAplicacion" TargetControlID="lnkProcesa" CancelControlID="btnCerrarModalCedula">
                    </asp:ModalPopupExtender>
                    <asp:LinkButton ID="lnkWordFile" runat="server" Text="Popup" Style="display: none"></asp:LinkButton>
                    <asp:Panel ID="pnlWordFile" runat="server" Width="590px" CssClass="panelVentanaEmergenteBackground"
                        HorizontalAlign="Center">
                        <table style="width: 100%; border-style: solid; border-width: 2px; border-color: White"
                            cellspacing="0" cellpadding="0">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Panel ID="PanelTituloWordFile" runat="server" CssClass="panelVentanaEmergenteTitulo"
                                        Width="100%" Height="20px">
                                        <asp:Label runat="server" ID="lblTituloModalWordFile" Text="Seleccione Machote para usar"
                                            Style="vertical-align: middle; margin-left: 5px;" CssClass="titulo_seccioninterior titulo_popup"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="Label3" runat="server" Text="Selecciona el Tema: " CssClass="txt_gral  txt_gral_o_gris"></asp:Label>
                                </td>
                                <td style="width: 450px">
                                    <asp:DropDownList ID="ddlTema" runat="server" Height="16px" Width="450px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="grvMachotes" runat="server" Width="550px" ForeColor="#555555" Font-Size="7.5pt"
                                        CellPadding="1" BackColor="#EEEEEE" AutoGenerateColumns="False" Font-Names="Arial"
                                        HorizontalAlign="Left" Font-Name="Arial" BorderColor="White" SelectedRowStyle-BackColor="#cccccc">
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="NotSet" CssClass="GridViewHeaderOficios"
                                            ForeColor="White" Wrap="false"></HeaderStyle>
                                        <RowStyle Wrap="true" />
                                        <Columns>
                                            <asp:ButtonField Text="O" CommandName="Selecciona">
                                                <ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="NOMBRE_ARCHIVO" HeaderText="Archivo" ItemStyle-Width="260px"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="DSC_TEMPLATE" HeaderText="Descripci&oacute;n" ItemStyle-Width="260px"
                                                ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="btnAceptarWordFile" runat="server" CssClass="botones" Height="28px"
                                        onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                        Text="Aceptar" Width="92px" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCerrarWordFile" runat="server" CssClass="botones" Height="28"
                                        onmouseout="style.backgroundColor='#696969'" onmouseover="style.backgroundColor='#A9A9A9'"
                                        Text="Cancelar" Width="93px" />
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:ModalPopupExtender ID="mpeWordFile" runat="server" PopupControlID="pnlWordFile"
                        BackgroundCssClass="FondoAplicacion" PopupDragHandleControlID="PanelTituloWordFile"
                        TargetControlID="lnkWordFile" CancelControlID="btnCerrarWordFile">
                    </asp:ModalPopupExtender>
                    <asp:LinkButton ID="lnkEnvia" runat="server" Text="" Style="display: none"></asp:LinkButton>
                    <asp:Panel ID="pnlEnvia" runat="server" Width="500px" Height="400px" CssClass="FondoManualOut">
                        <table width="500px">
                            <tr style="height: 20px">
                                <td align="center">
                                    <asp:Panel ID="Panel1" runat="server" CssClass="panelVentanaEmergenteTitulo" Width="100%"
                                        Height="20px">
                                        <asp:Label runat="server" ID="Label5" Text="Confirmación Envío por Oficialía" Style="vertical-align: middle;
                                            margin-left: 5px;" CssClass="titulo_seccioninterior titulo_popup"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr style="height: 20px">
                                <td align="center" valign="bottom" class="txt_gral_error">
                                    Seleccione Funcionarios que recibirán copia en caso de ser necesario
                                </td>
                            </tr>
                            <tr style="height: 310px">
                                <td align="center">
                                    <asp:Panel ID="pnlFuncionario" runat="server" ScrollBars="Vertical" Width="100%"
                                        Height="305">
                                        <asp:GridView runat="server" ID="grvFuncionarios" AutoGenerateColumns="false" Font-Size="7.5pt"
                                            BackColor="#EEEEEE" CellPadding="1" Font-Name="Arial" Font-Names="Arial" ForeColor="#555555"
                                            BorderColor="White" TabIndex="39" Width="470px">
                                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" BackColor="#236B7C" VerticalAlign="NotSet"
                                                CssClass="GridViewHeaderOficios" ForeColor="#FFFFFF" Wrap="false"></HeaderStyle>
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkFuncionario" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="NOMBRECOMPLETO" HeaderText="Funcionario" ItemStyle-Width="410px"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="USUARIO" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide"
                                                    FooterStyle-CssClass="hide" />
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr style="height: 35px">
                                <td align="center" valign="middle">
                                    <asp:Button ID="btnAceptaEnvia" runat="server" CssClass="botones" Height="28px" onmouseout="style.backgroundColor='#696969'"
                                        onmouseover="style.backgroundColor='#A9A9A9'" Text="Aceptar" Width="92px" ToolTip="Realizar Envío" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancelaEnvia" runat="server" CssClass="botones" Height="28" onmouseout="style.backgroundColor='#696969'"
                                        onmouseover="style.backgroundColor='#A9A9A9'" Text="Cancelar" Width="93px" ToolTip="Cancela Envío" />
                                    <asp:HiddenField runat="server" ID="hdnDestinatario" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:ModalPopupExtender ID="mpeEnvia" runat="server" PopupControlID="pnlEnvia" BackgroundCssClass="FondoAplicacion"
                        TargetControlID="lnkEnvia" CancelControlID="btnCancelaEnvia">
                    </asp:ModalPopupExtender>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:LinkButton ID="linkbotonArchivo" runat="server" ClientIDMode="Static" Text="Click_me"
                CssClass="hide"></asp:LinkButton>
        </div>
        </form>
    </div>
</body>
</html>
