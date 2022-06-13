<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="ProcesoVigilancia.aspx.vb" Inherits="SEPRIS.ProcesoVigilancia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table id="table3" style="height:400px; width:100%"
		cellspacing="0" cellpadding="0"  align="center" border="0">
		<tr>
			<!--Esta Fila crecerá dependiendo de la cantidad de filas a colocar--->
			<%--<td rowspan="11" width="20px">
            </td>--%>
			<td align="center" style="height: 50px">
				<asp:Label runat="server" ID="lblCatalogo" CssClass="TitulosWebProyectos" EnableTheming="false" Text="Catálogo">	
					</asp:Label>
			</td>
		</tr>
        <tr>
			<td style="height: 10px">
            </td>
		</tr>
		<tr>
			<td>
				<table style="height:22px" cellspacing="0" cellpadding="0" width="80%" border="0" align="center" >
					<tr>
						<td width="50%" align="left">
							

							<asp:Button id="btnExcel" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
								runat="server" Height="22px" Width="123px" CssClass="botones" Text="Exportar a Excel"></asp:Button>
						</td>
						<td width="25%" align="center">
							
						</td>
						<td width="25%" align="center"></td>
					</tr>
				</table>
			</td>
		</tr>
        <tr style="height:20px">
			<td height="20px" valign="top" align="center">
				<asp:radiobuttonlist id="RbtnEstatus" runat="server" AutoPostBack="True" CssClass="txt_gral" RepeatDirection="Horizontal">
					<asp:ListItem Value="1" Selected="True">Vigentes</asp:ListItem>
					<asp:ListItem Value="0">No vigentes</asp:ListItem>
					<asp:ListItem Value="-1">Todo</asp:ListItem>
				</asp:radiobuttonlist>
			</td>
		</tr>
        <tr>
			<td style="height: 10px"></td>
		</tr>
        <tr>
			<td align="center" valign="top">
				<table cellspacing="0" cellpadding="0" class="GridViewEncabezado" style="height:25px; width:80%;" border="0" >
					<tr>
						<td align="center">
							<asp:dropdownlist id="ddlFiltro" Width="180px" runat="server" AutoPostBack="True"></asp:dropdownlist>
						</td>
						<td align="left">
							<asp:dropdownlist id="ddlOpciones" runat="server" Width="462px" Visible="False"></asp:dropdownlist>
						</td>
						<td align="right" style="text-align:right;">
							<asp:button id="btnFiltrar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
							runat="server" Height="22" Width="123px" Text="Filtrar" CssClass="botones" ></asp:button>
						</td>
					</tr>
                </table>
			</td>
		</tr>
        <tr>
			<td  valign="top"  style="height:250px;">
				<asp:datagrid id="GridPrincipal" runat="server" class="GridViewContenido" AllowPaging="True" 
                    AllowSorting="True"    
					Font-Names="Arial" HorizontalAlign="center" Font-Name="Arial" CellPadding="1" 
					AutoGenerateColumns="False" Font-Size="7.5pt" width="80%" 
                    BorderColor="White"
                    HeaderStyle-CssClass="GridViewEncabezado" 
                    AlternatingRowStyle-CssClass="GridViewContenidoAlternate"  
                    RowStyle-CssClass="GridViewContenido" 
                    PagerStyle-CssClass="GridviewScrollPager">
					<Columns >
						<asp:ButtonColumn Text="O" CommandName="Delete">
							<ItemStyle HorizontalAlign="Center" Width="32px"></ItemStyle>
						</asp:ButtonColumn>
                        <asp:BoundColumn visible="FALSE" DataField="B_FLAG_VIG" ReadOnly="True" HeaderText="VIGENCIA">
							<ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
						</asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Estatus" SortExpression="B_FLAG_VIG">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
							<ItemTemplate>
									<asp:Image ID="imagenEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "B_FLAG_VIG"))  %>' />
									
							</ItemTemplate>
							<ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
						</asp:TemplateColumn>
					</Columns>
					<PagerStyle HorizontalAlign="Right" Position="Bottom" Mode="NumericPages"></PagerStyle>
				</asp:datagrid>


    			<asp:Image id="LblPrincipal" runat="server" ImageUrl="../imagenes/No Existen.gif" Visible="False"></asp:Image>
			</td>
		</tr>
        <tr>
			<td align="center" valign="top">
			</td>
		</tr>
		<tr>
			<td style="HEIGHT: 30px"></td>
		</tr>
		<tr>
			<td>
				<table style="height:22px" cellspacing="0" cellpadding="0" width="80%" border="0" align="center">
					<tr>
						<td align="left" width="20%">
							<asp:Button ID="BtnRegresar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
							runat="server" Height="22px" Text="Regresar" CssClass="botones" Width="123"></asp:Button>
						</td>
						<td width="20%" align="center" runat="server" id="tdBtnAgregar">
							<asp:Button ID="btnAgregar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
							runat="server" Height="22px" Text="Agregar" CssClass="botones" Width="123"></asp:Button>
						</td>
						<td width="20%" align="center" runat="server" id="tdBtnModificar">
							<asp:button id="btnModificar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
								runat="server" Height="22px" Width="123" Text="Modificar" CssClass="botones"></asp:button>
						</td>
						<td width="20%" align="center" runat="server" id="tdBtnEliminar">
							<asp:button id="btnEliminar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
								runat="server" Height="22px" Width="123" Text="Eliminar" CssClass="botones"></asp:button>
						</td>
						<td align="center" width="20%">
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td style="HEIGHT: 10px">
					
				<asp:TextBox id="TxtSQL" runat="server" Visible="False"></asp:TextBox>
				<asp:TextBox id="TxtOrder" runat="server" Visible="False"></asp:TextBox>
			</td>
		</tr>
		<!--Fin del Espacio para el contenido de la página-->
	</table>

	<asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />

	<div id="divConfirmacionEliminar" style="display: none">
        <table width="100%">
            <tr>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%=Mensaje%>
                    </div>
                </td>
            </tr>
        </table>
    </div>
	<div id="divMensajeError" style="display: none">
        <table width="100%">
            <tr>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%=Mensaje%>
                    </div>
                </td>
            </tr>
        </table>
    </div>
	<script type="text/javascript">
		function ConfirmacionEliminar() {
			$("#divConfirmacionEliminar").dialog({
                resizable: false,
                autoOpen: true,
                height: 300,
                width: 500,
				modal: true,
				open:
					function (event, ui) {
                        $(this).parent().css('z-index', 3999);
                        $(this).parent().appendTo(jQuery("form:last"));
                        $('.ui-widget-overlay').css('position', 'fixed');
                        $('.ui-widget-overlay').css('z-index', 3998);
                        $('.ui-widget-overlay').appendTo($("form:last"));
					},
				buttons: {
					"Aceptar": function () {
						$(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "Cancelar", disabled: true }]);
                        $('#btnAceptarM2B1A').trigger("click");
					},
					"Cancelar": function () {
                        $(this).dialog("close");
                        $('#btnCancelarM2B1A').trigger("click");
					}
				}
			});
		}
		function MensajeError() {
            $("#divMensajeError").dialog({
                resizable: false,
                autoOpen: true,
                height: 200,
                width: 400,
                modal: true,
                open:
                    function (event, ui) {
                        $(this).parent().css('z-index', 3999);
                        $(this).parent().appendTo(jQuery("form:last"));
                        $('.ui-widget-overlay').css('position', 'fixed');
                        $('.ui-widget-overlay').css('z-index', 3998);
                        $('.ui-widget-overlay').appendTo($("form:last"));
                    },
                buttons: {
                    "Aceptar": function () {
                        $(this).dialog("close");
                    }
                }
            });
		}
    </script>
</asp:Content>