<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Catalogo.aspx.vb"  Inherits="SEPRIS.Catalogo" %>

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
            <tr style="height:20px">
			    <td height="20px" valign="top" align="center">
				    <asp:radiobuttonlist id="RbtnEstatus" runat="server" AutoPostBack="True" CssClass="txt_gral" RepeatDirection="Horizontal">
					    <asp:ListItem Value="-1">Todo</asp:ListItem>
					    <asp:ListItem Value="1" Selected="True">Vigentes</asp:ListItem>
					    <asp:ListItem Value="0">Historial</asp:ListItem>
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
							<td align="right">
							    <asp:button id="btnFiltrar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
								runat="server" Height="22" Width="123px" Text="Filtrar" CssClass="botones"></asp:button>
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
								<asp:TemplateColumn HeaderText="ESTATUS" SortExpression="B_FLAG_VIG">
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
						<table style="height:22px" cellspacing="0" cellpadding="0" width="80%" border="0">
							<tr>
								<td width="25%" align="center">
                                <asp:Button ID="btnAgregar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                                runat="server" Height="22px" Text="Agregar" CssClass="botones" Width="123"></asp:Button>
									
								</td>
								<td width="25%" align="center">
									<asp:button id="btnModificar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
										runat="server" Height="22px" Width="123" Text="Modificar" CssClass="botones"></asp:button>
								</td>
								<td width="25%" align="center">
									<asp:button id="btnEliminar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
										runat="server" Height="22px" Width="123" Text="Eliminar" CssClass="botones"></asp:button>
								</td>
								<td align="center" width="25%">
									<asp:Button id="btnExcel" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
										runat="server" Height="22px" Width="123px" CssClass="botones" Text="Exportar a Excel"></asp:Button></td>
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


            <div id="divMensajeUnBotonNoAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align:top">
                            <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                            <%= Mensaje%>
                           </div>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="divMensajeDosBotonesUnaAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align:top">
                            <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

            <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />


            <script type="text/javascript">
                $(function () {

                    MensajeUnBotonNoAccionLoad();
                    MensajeDosBotonesDosAccionesLoad();
                    MensajeUnBotonUnaAccionLoad();
                    MensajeDosBotonesUnaAccionLoad();
                });


                function ConfirmacionError() {

                    MensajeUnBotonNoAccion();

                };


                function ConfirmacionEliminar() {

                    MensajeDosBotonesUnaAccion();

                };


                function MensajeFinalizar() {
                    MensajeUnBotonUnaAccion();
                }

                function MensajeConfirmacion() {
                    MensajeDosBotonesUnaAccion();
                }

            </script>
</asp:Content>
