<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FolioConsecutivo.aspx.vb" Inherits="SICOD.FolioConsecutivo" %>

<%@ Register assembly="MsgBox" namespace="MsgBox" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">




<html xmlns="http://www.w3.org/1999/xhtml">
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
	    <style type="text/css">
            .style1
            {
                text-align: center;
            }
        </style>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table id="table3" style="background-attachment: scroll; background-image: url(../imagenes/fondo_consar.jpeg);  background-repeat: no-repeat;height:450px;width:100%"
		cellspacing="0" cellpadding="0"  align="center" border="0">
				   <tr>
			    <!--Esta Fila crecerá dependiendo de la cantidad de filas a colocar--->
				<td rowspan="12" width="20px">
                </td>
				<td style="height: 20px" colspan="4">
				    <p class="TitulosWebProyectos" style="TEXT-ALIGN: center" align="center">
							Catálogo Inicializador de Folios
					</p>
				</td>
			</tr>
            <tr>
			    <td style="height: 10px" colspan="4">
                </td>
		    </tr>
            <tr style="height:20px">
			    <td height="20px" valign="top" align="center" colspan="4">
				    <asp:radiobuttonlist id="RbtnEstatus" runat="server" AutoPostBack="True" CssClass="txt_gral" RepeatDirection="Horizontal">
					    <asp:ListItem Value="WHERE 1=1 ">Todo</asp:ListItem>
					    <asp:ListItem Value="WHERE VIG_FLAG=1 " Selected="True">Vigentes</asp:ListItem>
					    <asp:ListItem Value="WHERE VIG_FLAG=0 ">Vencidos</asp:ListItem>
					</asp:radiobuttonlist>
				</td>
			</tr>
            <tr>
			    <td style="height: 10px" colspan="4"></td>
			</tr>
            <tr>
			    <td width="100%" align="center" valign="top" colspan="4">
				    <table cellspacing="0" cellpadding="0" 
                        style="height:25px;width:100%; background-color:#5D7370" border="0">
					    <tr>
						    <td align="center">
								<asp:dropdownlist id="DrpFiltro" Width="180px" runat="server" AutoPostBack="True"></asp:dropdownlist>
							</td>
							<td  align="left">
							    <asp:textbox id="TxtBuscar" runat="server" Visible="False"></asp:textbox>
								<asp:dropdownlist id="ddlOpciones" runat="server" Width="462px" Visible="False"></asp:dropdownlist>
							</td>
							<td  align="right">
							    <asp:button id="cmdFiltrar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
								runat="server" Height="22" Width="123" Text="Filtrar" CssClass="botones"></asp:button>
							</td>
						</tr>
                    </table>
				</td>
			</tr>
           <tr>
			    <td  valign="top" align="center" style="height:300px" colspan="4">
						<asp:datagrid id="GridPrincipal" runat="server" AllowPaging="True" 
                            AllowSorting="True" ForeColor="#555555"   
							Font-Names="Arial" HorizontalAlign="Center" Font-Name="Arial" CellPadding="1" 
							AutoGenerateColumns="False" BackColor="#D6EBBD"  Font-Size="7.5pt" width="100%" 
                            BorderColor="White">
							<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
							<HeaderStyle  Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="HeaderSICOD"
                        ForeColor="#FFFFFF"></HeaderStyle>
							<Columns>
								<asp:ButtonColumn Text="O" CommandName="Delete">
                                <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
								</asp:ButtonColumn>
								<asp:BoundColumn visible="true" DataField="ID_INI_FOLIO" SortExpression="ID_INI_FOLIO" ReadOnly="True" HeaderText="CLAVE">
									<ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn visible="FALSE" DataField="ID_UNIDAD_ADM" SortExpression="ID_UNIDAD_ADM" ReadOnly="True" HeaderText="ID_UNIDAD_ADM">
									<ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn visible="FALSE" DataField="TIPO_DOC" SortExpression="TIPO_DOC" ReadOnly="True" HeaderText="TIPO_DOC">
									<ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
								</asp:BoundColumn>
                                 <asp:BoundColumn visible="TRUE" DataField="ANIO" SortExpression="ANIO" ReadOnly="True" HeaderText="AÑO">
									<ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn  DataField="DSC_UNIDAD_ADM" SortExpression="DSC_UNIDAD_ADM"  HeaderText="AREA">
									<ItemStyle HorizontalAlign="Left" Width="600px"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOCUMENTO" SortExpression="TIPO_DOC"  HeaderText="TIPO_DOC">
									<ItemStyle HorizontalAlign="Left" Width="620px"></ItemStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="INICIAL" SortExpression="INICIAL"  HeaderText="NUM_INICIAL">
									<ItemStyle HorizontalAlign="Left" Width="115px"></ItemStyle>
								</asp:BoundColumn>
                                 <asp:BoundColumn Visible="FALSE" DataField="VIG_FLAG" SortExpression="VIG_FLAG"  HeaderText="VIG_FLAG">
									<ItemStyle HorizontalAlign="Left" Width="620px"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="ESTATUS" SortExpression="VIG_FLAG">
									<ItemTemplate>
										<asp:Image id="xx" runat="server" ImageUrl='<%# ImagenVigencia(DataBinder.Eval(Container.DataItem,"VIG_FLAG"))%>'>
										</asp:Image>
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" Width="112px"></ItemStyle>
								</asp:TemplateColumn>
								
							</Columns>
							<PagerStyle HorizontalAlign="Right" Position="Bottom" Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
						<asp:Image id="LblPrincipal" runat="server" ImageUrl="../imagenes/no EXISTEN.gif" Visible="False"></asp:Image>
					</td>
				</tr>
				<tr>
					<td style="HEIGHT: 10px" colspan="4"></td>
				</tr>
				<tr>
					<td style="HEIGHT: 10px" class="style1">
									<asp:button id="cmdAgregar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
										runat="server" Height="22px" Width="123" Text="Agregar" CssClass="botones"></asp:button>
								</td>
					<td style="HEIGHT: 10px" class="style1">
									<asp:button id="cmdModificar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
										runat="server" Height="22px" Width="123" Text="Modificar" CssClass="botones"></asp:button>
								</td>
					<td style="HEIGHT: 10px" class="style1">
									<asp:button id="cmdEliminar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
										runat="server" Height="22px" Width="123" Text="Eliminar" CssClass="botones"></asp:button>
								</td>
					<td style="HEIGHT: 10px" class="style1">
									<asp:Button id="bExcel" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
										runat="server" Height="22px" Width="127px" CssClass="botones" Text="Exportar Grid a Excel"></asp:Button></td>
				</tr>
				<tr>
					<td width="812px" colspan="4">
						&nbsp;</td>
				</tr>
				<tr>
					<td style="HEIGHT: 10px" colspan="4">
						<asp:textbox id="Clave" runat="server" Visible="False"></asp:textbox>
						<asp:TextBox id="TxtVigencia" runat="server" Visible="False"></asp:TextBox>
						<asp:TextBox id="TxtSQL" runat="server" Visible="False"></asp:TextBox>
						<asp:TextBox id="TxtOrder" runat="server" Visible="False"></asp:TextBox>
						<cc1:msgbox id="MsgBox1" runat="server"></cc1:msgbox>
					</td>
				</tr>
				<!--Fin del Espacio para el contenido de la página-->
			</table>
		</form>
	</body>
</html>
