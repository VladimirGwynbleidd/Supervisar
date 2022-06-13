<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VincularDocumentosSICOD.aspx.vb"
    Inherits="SICOD.VincularDocumentosSICOD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="SICOD" TagName="FiltroBusquedaAsociarOficios" Src="~/App_Oficios/UserControls/FiltroBusquedaAsociarOficios.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        //AGC Validar que al presionar la tecla "Backspace"
        document.onkeydown = function () {
            activeObj = document.activeElement;
            if (activeObj.tagName != "INPUT" && activeObj.tagName != "TEXTAREA") {
                if (event.keyCode === 8) {
                    return false;
                }
            }
        }
        function BotonFiltrar() {
            document.getElementById('Imagen_procesando').style.display = '';
            document.getElementById('Imagen_procesando').style.visibility = 'visible';
            document.getElementById('Imagen_procesando').style.height = '150px';
            document.getElementById('GRID').style.visibility = 'hidden';
            try {
                document.getElementById('Boton_guardar').style.visibility = 'hidden';
            }
            catch (e) { }
        }
      
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="Toolkitscriptmanager2" runat="server" EnableScriptGlobalization="true">
    </asp:ToolkitScriptManager>
    <asp:Panel ID="pnlBusqueda" runat="server" Width="100%" Style="margin: 0 auto">
        <table width="100%" align="center">
            <tr>
                <td style="height: 20px; padding: 15px;" align="center">
                    <asp:Label ID="lblTitulo" class="TitulosWebProyectos" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td id="filtroOficiosWrapper">
                    <table width="100%" style="background-color: #7FA6A1">
                        <tr>
                            <td>
                                <SICOD:FiltroBusquedaAsociarOficios ID="FiltrosAsociar" runat="server" DefaultButtonId="BtnFiltrar"></SICOD:FiltroBusquedaAsociarOficios>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="BtnFiltrar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                                    runat="server" Height="22px" ToolTip="FILTRO ESPECIFICO" Text="Filtrar" CssClass="botones"
                                    Width="130" OnClientClick="BotonFiltrar()"></asp:Button>
                                <br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%" align="center">
            <tr>
                <td>
                    <div id="Imagen_procesando" runat="server" style="display: none; text-align: center">
                        <img src="/imagenes/carga.gif" alt="Cargando..." />
                    </div>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <div id="GRID" runat="server" style="overflow: auto; width: 100%; height: 300px;
                        position: relative">
                        <asp:GridView ID="GridPrincipal" runat="server" Font-Size="7.5pt" AutoGenerateColumns="false"
                            BackColor="#EEEEEE" CellPadding="1" Font-Name="Arial" HorizontalAlign="Center"
                            Font-Names="Arial" ForeColor="#555555" DataKeyNames="ID_FOLIO, NOMBRE_ARCHIVO"
                            BorderColor="White">
                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="NotSet" CssClass="GridViewHeaderOficios"
                                ForeColor="#FFFFFF"></HeaderStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="T" ItemStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="White"
                                    ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSeleccionar" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Folio" HeaderStyle-ForeColor="White">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblFolio" runat="server" Text='<%# Bind("ID_FOLIO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FECH_RECEPCION" HeaderStyle-ForeColor="White" HeaderText="Fecha Rec. Doc.">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DSC_REFERENCIA" HeaderStyle-ForeColor="White" HeaderText="Referencia">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DSC_NUM_OFICIO" HeaderStyle-ForeColor="White" HeaderText="Oficio">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DSC_REMITENTE" HeaderStyle-ForeColor="White" HeaderText="Remitente"
                                    ItemStyle-Width="120px">
                                    <ItemStyle Width="140px" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DSC_ASUNTO" HeaderStyle-ForeColor="White" HeaderText="Asunto">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DSC_T_DOC" HeaderText="Tipo de Documento" HeaderStyle-ForeColor="White">
                                    <ItemStyle Width="75px" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Es Copia" HeaderStyle-ForeColor="White">
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    <ItemTemplate>
                                        <asp:Image ID="imgD" runat="server" ImageUrl='<%# ImagenEstatusCopia(DataBinder.Eval(Container.DataItem,"DESTINATARIO"))%>' />
                                        <asp:Label ID="lblEsCopia" runat="server" Text='<%# Bind("DESTINATARIO") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="NOMBRE" HeaderText="Responsable" HeaderStyle-ForeColor="White">
                                    <ItemStyle HorizontalAlign="Center" Width="300px"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Nombre Archivo" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-ForeColor="White" HeaderStyle-CssClass="BO_Column">
                                    <HeaderStyle CssClass="BO_Column" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblArchivoSICOD" ForeColor="Blue" ToolTip="Abrir Archivo PDF"
                                            runat="server" HeaderStyle-ForeColor="White" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                            CommandName="verPDFFolio" Text='<%# Bind("NOMBRE_ARCHIVO") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="USUARIO" Visible="false">
                                    <ItemStyle HorizontalAlign="Center" Width="300px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUsuario" runat="server" Text='<%# Bind("USUARIO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ARCHIVO_SBM" HeaderText="Archivo SBM" HeaderStyle-ForeColor="White">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CORREO_SIE" HeaderText="Correo SIE" HeaderStyle-ForeColor="White">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ORIGINAL_FLAG"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnAsociar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                        runat="server" Style="height: 22px; width: 130px; float: right" Text="Asociar"
                        CssClass="botones" />
                    <asp:Button ID="btnRegresar" runat="server" CssClass="botones" onmouseout="style.backgroundColor='#696969'"
                        onmouseover="style.backgroundColor='#A9A9A9'" Style="height: 22px; width: 130px;
                        float: left" Text="Regresar" />
                    <div style="clear: both">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlModal" runat="server" Width="830px" Style="margin: 0 auto">
        <table align="center" width="818px">
            <tr>
                <td>
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
                                            CssClass="botones" />
                                        <asp:Button ID="BtnCancelarModal" runat="server" onmouseover="style.backgroundColor='#A9A9A9'"
                                            onmouseout="style.backgroundColor='#696969'" Height="28" Width="93" Text="Cancelar"
                                            CssClass="botones" Visible="false" />
                                        <asp:Label ID="lblModalPostBack" runat="server" Style="display: none"></asp:Label>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:ModalPopupExtender ID="ModalPopupExtenderErrores" runat="server" PopupControlID="PanelErrores"
                        TargetControlID="LB1" BackgroundCssClass="FondoAplicacion" DropShadow="true"
                        PopupDragHandleControlID="PanelErroresHandle"></asp:ModalPopupExtender>
                    <asp:Label runat="server" ID="lblErrores" CssClass="txt_gral_rojo" Visible="false"
                        Style="display: none"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
