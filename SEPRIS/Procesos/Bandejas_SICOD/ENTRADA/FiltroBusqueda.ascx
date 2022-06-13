<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="FiltroBusqueda.ascx.vb"
    Inherits="SEPRIS.FiltroBusqueda" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html>
<head>
    <link href="Scripts/dynCalendar.css" type="text/css" rel="stylesheet" />
    <script language="javascript" src="Scripts/browserSniffer.js" type="text/javascript"></script>
    <script language="javascript" src="Scripts/dynCalendar.js" type="text/javascript"></script>
    <script language="JavaScript" type="text/javascript">
        function clickButton(e, buttonid) {
            var evt = e ? e : window.event;
            var bt = document.getElementById(buttonid);

            if (bt) {
                if (evt.keyCode == 13) {
                    bt.click();
                    return false;
                }
            }
        }

        function SelectMeOnly(objRadioButton) {
            var i, obj;
            for (i = 0; i < document.all.length; i++) {
                obj = document.all(i);
                if (obj.type == "radio") {
                    if (objRadioButton == obj.id) {
                        obj.checked = true;
                    }
                    else {
                        obj.checked = false;
                    }
                }
            }
        }

        function SelectMeOnlyStatusRec(objRadioButtonCheck) {
            var rdbRec = document.getElementById('Filtros_rdbRec');
            var rdbNRec = document.getElementById('Filtros_rdbNRec');

            if (objRadioButtonCheck == "Filtros_rdbRec") {
                rdbRec.checked = true;
                rdbNRec.checked = false;
            }
            else {
                rdbRec.checked = false;
                rdbNRec.checked = true;
            }
        }


        function SelectMeOnlySIE(objRadioButtonCheck) {
            var rbtnSIE_NO = document.getElementById('Filtros_rbtnSIE_NO');
            var rbtnSIE_SI = document.getElementById('Filtros_rbtnSIE_SI');

            if (objRadioButtonCheck == "Filtros_rbtnSIE_SI") {
                rbtnSIE_SI.checked = true;
                rbtnSIE_NO.checked = false;
            }
            else {
                rbtnSIE_SI.checked = false;
                rbtnSIE_NO.checked = true;
            }
        }



    </script>
</head>
<body>
    <table cellspacing="0">
        <tr>
            <td>
                <asp:Panel ID="pnlRangoDeFechas" Visible="False" runat="server" Width="669px">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblPeriodo" CssClass="txt_gral_blanco" Height="19px" runat="server">Fecha Recepción Doc.:</asp:Label>
                            </td>
                            <td width="140" align="left">
                                <asp:TextBox ID="TxtFecRecIni" runat="server" Width="135px" Height="15px" MaxLength="12"></asp:TextBox>
                                <cc1:CalendarExtender ID="TxtFecRecIni_CalendarExtender" runat="server" TargetControlID="TxtFecRecIni"
                                    Enabled="True" PopupButtonID="imgFec3" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td align="left">
                                <asp:Image ID="imgFec3" runat="server" src="../../../Imagenes/Calendar.GIF" 
                                    Style="cursor: hand" ImageUrl="../../../Imagenes/Calendar.GIF"  />
                            </td>
                            <td width="120" align="left">
                                <asp:TextBox ID="TxtFecRecFin" runat="server" Width="135px" Height="15px" MaxLength="12"></asp:TextBox>
                                <cc1:CalendarExtender ID="TxtFecRecFin_CalendarExtender" runat="server" TargetControlID="TxtFecRecFin"
                                    Enabled="True" PopupButtonID="imgFec2" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td width="40" align="left">
                                <asp:Image ID="imgFec2" runat="server" src="../../../Imagenes/Calendar.GIF"  Style="cursor: hand" />
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaRangoDeFechas" onmouseover="style.backgroundColor='#003F4B'"
                                    onmouseout="style.backgroundColor='#2E7752'" runat="server" CssClass="botones_x"
                                    Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlFechaDeDocto" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblFecDocto" CssClass="txt_gral_blanco" runat="server">Fecha Documento:</asp:Label>
                            </td>
                            <td width="140" align="left">
                                <asp:TextBox ID="TxtFecDocto" runat="server" Height="15px" Width="135px"></asp:TextBox>
                                <cc1:CalendarExtender ID="TxtFecDocto_CalendarExtender" runat="server" TargetControlID="TxtFecDocto"
                                    Enabled="True" PopupButtonID="imgFec1" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td align="left">
                                <asp:Image ID="imgFec1" runat="server" src="../../../Imagenes/Calendar.GIF" 
                                    Style="cursor: hand" ImageUrl="../../../Imagenes/Calendar.GIF"  />
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaFecDocto" onmouseover="style.backgroundColor='#003F4B'"
                                    onmouseout="style.backgroundColor='#2E7752'" runat="server" CssClass="botones_x"
                                    Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlFolio" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblFolio" CssClass="txt_gral_blanco" runat="server">Folio:</asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtFolio" runat="server" Height="15px" Width="135px" ToolTip="Numero de Folio"
                                    MaxLength="100"></asp:TextBox>
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaFolio" onmouseover="style.backgroundColor='#003F4B'" onmouseout="style.backgroundColor='#2E7752'"
                                    runat="server" CssClass="botones_x" Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlTipoDeDocto" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblTipoDocto" CssClass="txt_gral_blanco" runat="server">Tipo de Documento:</asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlTipoDocto" runat="server" Height="20px" Width="320px" AutoPostBack="true"
                                    CssClass="txt_gral">
                                </asp:DropDownList>
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaTipoDocto" onmouseover="style.backgroundColor='#003F4B'"
                                    onmouseout="style.backgroundColor='#2E7752'" runat="server" CssClass="botones_x"
                                    Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlArea" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblArea" CssClass="txt_gral_blanco" runat="server">&Aacute;rea:</asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlArea" runat="server" Height="20px" Width="320px" CssClass="txt_gral"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaArea" onmouseover="style.backgroundColor='#003F4B'" onmouseout="style.backgroundColor='#2E7752'"
                                    runat="server" CssClass="botones_x" Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlDestinatario" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblDestinatario" CssClass="txt_gral_blanco" runat="server">Destinatario:</asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlDestinatario" runat="server" Height="20px" Width="320px"
                                    AutoPostBack="true" CssClass="txt_gral">
                                </asp:DropDownList>
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaDestinatario" onmouseover="style.backgroundColor='#003F4B'"
                                    onmouseout="style.backgroundColor='#2E7752'" runat="server" CssClass="botones_x"
                                    Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlReferencia" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblRefere" CssClass="txt_gral_blanco" runat="server">Referencia:</asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="TxtRefere" runat="server" Height="15px" Width="320px" ToolTip="Descripcion Rerencia"
                                    MaxLength="50"></asp:TextBox>
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaRefere" onmouseover="style.backgroundColor='#003F4B'" onmouseout="style.backgroundColor='#003F4B'"
                                    runat="server" CssClass="botones_x" Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlOficio" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblOficio" CssClass="txt_gral_blanco" runat="server">Oficio:</asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtOficio" runat="server" Height="15px" Width="320px" ToolTip="Descripcion Oficio"
                                    MaxLength="100"></asp:TextBox>
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaOficio" onmouseover="style.backgroundColor='#003F4B'" onmouseout="style.backgroundColor='#2E7752'"
                                    runat="server" CssClass="botones_x" Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlRemitente" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblRemitente" CssClass="txt_gral_blanco" runat="server">Remitente:</asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtRemitente" runat="server" Height="15px" Width="320px" ToolTip="Descripcion Remitente"
                                    MaxLength="100"></asp:TextBox>
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaRemitente" onmouseover="style.backgroundColor='#003F4B'"
                                    onmouseout="style.backgroundColor='#2E7752'" runat="server" CssClass="botones_x"
                                    Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlAsunto" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblAsunto" CssClass="txt_gral_blanco" runat="server">Asunto:</asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtAsunto" runat="server" Height="15px" Width="320px" ToolTip="Descripcion de Asunto"
                                    MaxLength="100"></asp:TextBox>
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaAsunto" onmouseover="style.backgroundColor='#003F4B'" onmouseout="style.backgroundColor='#2E7752'"
                                    runat="server" CssClass="botones_x" Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlNombre" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblNombre" CssClass="txt_gral_blanco" runat="server">Responsable:</asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="txtNombre" runat="server" AutoPostBack="true" CssClass="txt_gral"
                                    Height="20px" Width="320px">
                                </asp:DropDownList>
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaNombre" onmouseover="style.backgroundColor='#003F4B'" onmouseout="style.backgroundColor='#2E7752'"
                                    runat="server" CssClass="botones_x" Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlStatusAtendida" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblAtendida" CssClass="txt_gral_blanco" runat="server">Estatus :</asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="320px">
                                    <asp:ListItem>Seleccionar</asp:ListItem>
                                    <asp:ListItem>Atendido</asp:ListItem>
                                    <asp:ListItem>Normal</asp:ListItem>
                                    <asp:ListItem>Por Vencer</asp:ListItem>
                                    <asp:ListItem>Vencido</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaStatus" onmouseover="style.backgroundColor='#003F4B'" onmouseout="style.backgroundColor='#2E7752'"
                                    runat="server" CssClass="botones_x" Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlFechaRegistro" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblFechaRegistro" CssClass="txt_gral_blanco" runat="server">Fecha Registro:</asp:Label>
                            </td>
                            <td width="140" align="left">
                                <asp:TextBox ID="txtFechaRegistro" runat="server" Height="15px" Width="135px"></asp:TextBox>
                                <cc1:CalendarExtender ID="cleFechaRegistro" runat="server" TargetControlID="txtFechaRegistro"
                                    Enabled="True" PopupButtonID="imgFecReg" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td align="left">
                                <asp:Image ID="imgFecReg" runat="server" src="../../../Imagenes/Calendar.GIF" 
                                    Style="cursor: hand" ImageUrl="../../../Imagenes/Calendar.GIF"  />
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaFechaRegistro" onmouseover="style.backgroundColor='#003F4B'"
                                    onmouseout="style.backgroundColor='#2E7752'" runat="server" CssClass="botones_x"
                                    Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlFechaLimite" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                <asp:Label ID="lblFechaLimite" CssClass="txt_gral_blanco" runat="server">Fecha Limite:</asp:Label>
                            </td>
                            <td width="140" align="left">
                                <asp:TextBox ID="txtFechaLimite" runat="server" Height="15px" Width="135px"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaLimite"
                                    Enabled="True" PopupButtonID="imgFecLim" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td align="left">
                                <asp:Image ID="imgFecLim" runat="server" src="../../../Images/Calendar.GIF" 
                                    Style="cursor: hand" ImageUrl="../../../Images/Calendar.GIF"  />
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaFechaLimite" onmouseover="style.backgroundColor='#003F4B'"
                                    onmouseout="style.backgroundColor='#2E7752'" runat="server" CssClass="botones_x"
                                    Text="x"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlEstatus" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                &nbsp;
                            </td>
                            <td width="120" align="left">
                                <asp:RadioButton ID="rdbRec" runat="server" Text="Recibido" CssClass="txt_gral_blanco" />
                            </td>
                            <td width="200" align="left">
                                <asp:RadioButton ID="rdbNRec" runat="server" CssClass="txt_gral_blanco" Text="No Recibido" />
                            </td>
                            <td width="150" align="left">
                                &nbsp;
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaEstatus" runat="server" CssClass="botones_x" onmouseout="style.backgroundColor='#2E7752'"
                                    onmouseover="style.backgroundColor='#003F4B'" Text="x" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlProvieneSIE" Visible="False" runat="server">
                    <table border="0" cellspacing="0" width="670">
                        <tr>
                            <td width="180" align="left">
                                &nbsp;
                            </td>
                            <td width="120" align="left">
                                <asp:RadioButton ID="rbtnSIE_SI" runat="server" Text="Proviene de SIE" CssClass="txt_gral_blanco" />
                            </td>
                            <td width="200" align="left">
                                <asp:RadioButton ID="rbtnSIE_NO" runat="server" CssClass="txt_gral_blanco" Text="No Proviene de SIE" />
                            </td>
                            <td width="150" align="left">
                                &nbsp;
                            </td>
                            <td width="20">
                                <asp:Button ID="BtnEliminaSIE" runat="server" CssClass="botones_x" onmouseout="style.backgroundColor='#2E7752'"
                                    onmouseover="style.backgroundColor='#003F4B'" Text="x" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table border="0" cellspacing="0" width="670">
                    <tr>
                        <td width="180" align="left">
                            <asp:DropDownList ID="ddlAgregar" Width="120px" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:ListBox ID="listFiltrosControl" Visible="False" runat="server" 
                                Height="26px">
                                <asp:ListItem Value="1">Fecha Recepción Doc.</asp:ListItem>
                                <asp:ListItem Value="0">Folio</asp:ListItem>
                                <asp:ListItem Value="0">Fecha Documento</asp:ListItem>
                                <asp:ListItem Value="0">Tipo de Documento</asp:ListItem>
                                <asp:ListItem Value="0">Area</asp:ListItem>
                                <asp:ListItem Value="0">Destinatario</asp:ListItem>
                                <asp:ListItem Value="0">Recibido</asp:ListItem>
                                <asp:ListItem Value="0">Referencia</asp:ListItem>
                                <asp:ListItem Value="0">Oficio</asp:ListItem>
                                <asp:ListItem Value="0">Asunto</asp:ListItem>
                                <asp:ListItem Value="0">Fecha de Registro</asp:ListItem>
                            </asp:ListBox>
                            <asp:CheckBox ID="CheckBox1" runat="server" Font-Bold="True" Font-Names="Verdana"
                                Font-Size="X-Small" ForeColor="White" Text="Solo Mios" TextAlign="Left" 
                                AutoPostBack="True" Visible="False" />
                        </td>
                        <td width="140" align="right">
                            <asp:Button ID="BtnFiltrar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                                runat="server" Height="22px" ToolTip="FILTRO ESPECIFICO" Text="Filtrar" CssClass="botones"
                                Width="130" OnClientClick="BotonFiltrar()"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
