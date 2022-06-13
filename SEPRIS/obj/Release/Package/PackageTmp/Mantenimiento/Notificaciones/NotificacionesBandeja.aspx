<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="NotificacionesBandeja.aspx.vb" Inherits="SEPRIS.NotificacionesBandeja" %>

<%@ Register Src="/Controles/ucFiltro.ascx" TagName="ucFiltro" TagPrefix="ucFiltro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(function () {

            MensajeDosBotonesUnaAccionLoad();
            MensajeUnBotonUnaAccionLoad();
            MensajeUnBotonNoAccionLoad();

        });

        function MostrarConfirmacion() {

            MensajeDosBotonesUnaAccion();

        }

        function MostrarAceptar() {

            MensajeUnBotonUnaAccion();

        }

        function MostrarMensaje() {

            MensajeUnBotonNoAccion();

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitulosWebProyectos">
                Mensajes de Notificaci&oacute;n
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:Button runat="server" ID="btnExportar" Text="Exportar a Excel" Width="100px" />
            </td>
        </tr>
        <tr>
            <td>
                <ucFiltro:ucFiltro runat="server" ID="Filtros" Width="100%" />
            </td>
        </tr>
        <tr>
            <td>
                <cc1:CustomGridView ID="grvNotificaciones" runat="server" Width="100%" AllowSorting="true" ToolTipHabilitado="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkSeleccion" />
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                            <HeaderStyle Width="15px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Identificador" HeaderText="Clave" SortExpression="Identificador" >
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Titulo" HeaderText="Nombre" SortExpression="Titulo" />
                        <asp:TemplateField HeaderText="ESTATUS" SortExpression="Vigente">
                            <ItemTemplate>
                                <asp:Image ID="imgEstatus" runat="server" ImageUrl='<%# ImagenVigencia(DataBinder.Eval(Container.DataItem,"Vigente"))%>'>
                                </asp:Image>
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                            <HeaderStyle Width="50px" />
                        </asp:TemplateField>
                    </Columns>
                </cc1:CustomGridView>

                <div id="pnlNoExiste" runat="server" align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Image ID="Image1" runat="server" 
                            AlternateText="No existen registros para la consulta" ImageAlign="Middle" 
                            imageurl="../../Imagenes/no EXISTEN.gif" />
                </div>

            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnAgregar" Text="Agregar" />
                &nbsp;
                <asp:Button runat="server" ID="btnModificar" Text="Modificar" />
                &nbsp;
                <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" />
            </td>
        </tr>
    </table>
    <div id="divMensajeDosBotonesUnaAccion" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align:top">
                    <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%=Mensaje  %>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divMensajeUnBotonUnaAccion" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align:top">
                    <asp:Image ID="imgUnBotonUnaAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%=Mensaje %>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divMensajeUnBotonNoAccion" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align:top">
                    <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <asp:Label runat="server" ID="lblError" Text="" EnableTheming="false"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
    <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" /> 
    <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" /> 
    <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
</asp:Content>
