<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Folio_s_Consecutivo.aspx.vb" Inherits="SICOD.Folio_s_Consecutivo" %>

<%@ Register assembly="MsgBox" namespace="MsgBox" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
	<head>
		<title></title>
		<script language="JavaScript" type="text/JavaScript">
			<!--
		    function MM_swapImgRestore() { //v3.0
		        var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
		    }

		    function MM_preloadImages() { //v3.0
		        var d = document; if (d.images) {
		            if (!d.MM_p) d.MM_p = new Array();
		            var i, j = d.MM_p.length, a = MM_preloadImages.arguments; for (i = 0; i < a.length; i++)
		                if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; } 
		        }
		    }

		    function MM_findObj(n, d) { //v4.01
		        var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
		            d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
		        }
		        if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
		        for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
		        if (!x && d.getElementById) x = d.getElementById(n); return x;
		    }

		    function MM_swapImage() { //v3.0
		        var i, j = 0, x, a = MM_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2); i += 3)
		            if ((x = MM_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
		    }
			//-->
		</script>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<link href="../Styles.css" type="text/css" rel="stylesheet"/>
		<link rel="stylesheet" href="../EstilosMenuLogin.css" type="text/css" />
	    <style type="text/css">
            .style1
            {
                height: 23px;
            }
            .style2
            {
                height: 23px;
                text-align: left;
            }
            .style3
            {
                text-align: left;
            }
        </style>
	</head>
	<body>
	<form id="Form1" runat="server">
		<table align="center" width="100%" border="0" cellpadding="0" cellspacing="0" 
            style="background-image:url(../imagenes/fondo_consar.jpeg); background-attachment:scroll; background-repeat:no-repeat">
    		<tr>
				<!--Esta Fila crecerá dependiendo de la cantidad de filas a colocar--->
				<td style="HEIGHT: 20px">
					<p class="TitulosWebProyectos" style="TEXT-ALIGN: center" align="center">
						Registro de Inicializador de Folios</p>
				</td>
			</tr>
			<!--Espacio para el contenido de la página-->
			<tr>
				<td style="HEIGHT: 10px"></td>
			</tr>
			<tr align="center" valign="top" style="height:410px;">
				<td>
					<table align="center" class="forma" width="100%" border="0">
						<tr><td colspan="2">&nbsp;</td></tr>
						<tr>
							<td align="right">
								<asp:Label ID="lblClave0" runat="server" Text="* Area:" 
                                    CssClass="txt_gral"></asp:Label>
							&nbsp;</td>
							<td width="500" class="style3">
	
							    <asp:DropDownList ID="DropDownList1" runat="server" Height="16px" Width="267px">
                                </asp:DropDownList>
	
							</td>
						</tr>
						<tr>
							<td align="right">
								<asp:Label ID="lblClave" runat="server" Text="* Tipo Doc:" CssClass="txt_gral"></asp:Label>
							</td>
							<td width="500" class="style3">
								<asp:DropDownList ID="DropDownList2" runat="server" Height="16px" Width="267px">
                                </asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td align="right" class="style1">
								<asp:Label ID="lblDescripcion" runat="server" Text="* Año:" CssClass="txt_gral"></asp:Label>
							</td>
							<td class="style2">
                                <asp:TextBox ID="TextBox1" runat="server" Width="269px"></asp:TextBox>
							</td>
						</tr>
						<tr>
                        <td style="text-align: right">
								<asp:Label ID="lblDescripcion0" runat="server" 
                                Text="* Inicializador de Folio:" CssClass="txt_gral"></asp:Label>
							</td>
							<td align="left">
								<asp:TextBox ID="TextBox2" runat="server" Width="269px"></asp:TextBox>
							</td>
						</tr>
						<tr>
                        <td style="WIDTH: 320px">&nbsp;</td>
							<td align="left">
								&nbsp;</td>
						</tr>
						<tr>
                        <td></td>
							<td align="left">
								<asp:Label id="LblPrincipal" runat="server" ForeColor="Navy" CssClass="txt_gral">* Datos Obligatorios</asp:Label>
							</td>
						</tr>
						<tr><td colspan="2">&nbsp;</td></tr>
						<tr>
                        <td></td>
							<td align="left">
								<asp:button id="cmdAceptar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'" runat="server" Width="123" Height="21" Text="Aceptar" CssClass="botones"></asp:button>
								&nbsp;&nbsp;
								<asp:button id="cmdCancelar" onmouseover="style.backgroundColor='#A9A9A9'" 
                                    onmouseout="style.backgroundColor='#696969'" runat="server" Width="123" 
                                    Height="21" Text="Cancelar" CssClass="botones" 
                                    PostBackUrl="~/App_Oficios/FolioConsecutivo.aspx"></asp:button>
							</td>
						</tr>
						<tr>
							<td align="right" colspan="2" style="WIDTH: 190px">
                                <asp:TextBox ID="TxtClave" runat="server" Height="16px" Visible="False" 
                                    Width="148px"></asp:TextBox>
                                <cc1:MsgBox id="MsgBox1" runat="server"></cc1:MsgBox></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td style="HEIGHT: 10px"></td>
			</tr>
			<!--Fin del Espacio para el contenido de la página-->
		</table>
		</form>
	</body>
</html>
