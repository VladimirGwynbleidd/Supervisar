<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="NotificacionesConfiguracionEstilos.aspx.vb" Inherits="SEPRIS.NotificacionesConfiguracionEstilos" %>

<%@ Register Src="/Controles/wucPersonalizarColumnas.ascx" TagName="ucPersonaliza"
    TagPrefix="ucPersonaliza" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(function () {

            $(".cbToggle  input:checkbox").button();
            $('.cbToggle  input:checkbox').click(function () {
                $(this).blur();
            });

            MensajeUnBotonNoAccionLoad();
            MensajeDosBotonesUnaAccionLoad();
            MensajeUnBotonUnaAccionLoad();

        });

        function MostrarError() {

            MensajeUnBotonNoAccion();

        }

        function MostrarConfirmacion() {

            MensajeUnBotonUnaAccion();

        }

        function PreguntaGuardar() {

            MensajeDosBotonesUnaAccion();
            return false;

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
                Configuraci&oacute;n de Estilos
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <cc1:CustomGridView ID="grvDatosConfiguracion" runat="server" Width="100%" HabilitaSeleccion="No"
                    DataKeyNames="Identificador" DataBindEnPostBack="false" SkinID="SinSeleccion">
                    <Columns>
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                        <asp:TemplateField HeaderText="Tipo Letra">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlTipoLetra" runat="server" CssClass="txt_gral">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tamaño Letra">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlTamanioLetra" runat="server" CssClass="txt_gral">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Negrita">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkNegrita" runat="server" class="cbToggle" Text="B" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Itálica">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkItalica" runat="server" class="cbToggle" Text="I" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alineación">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlAlineacion" runat="server" CssClass="txt_gral" Width="160px">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Color Letra / Fondo">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlColor" runat="server" CssClass="txt_gral">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </cc1:CustomGridView>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnAceptar" Text="Aceptar" />
                &nbsp;&nbsp;
                <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" />
            </td>
        </tr>
    </table>
    <table align="left" width="200px">
        <tr>
            <td class="txt_gral" style="width: 150px">
                Seleccionado
            </td>
            <td style="width: 50px">
                <asp:CheckBox ID="CheckBox2" runat="server" Text="B" Checked="true" Enabled="False"
                    CssClass="cbToggle" />
            </td>
        </tr>
        <tr>
            <td class="txt_gral">
                No Seleccionado
            </td>
            <td>
                <asp:CheckBox ID="CheckBox3" runat="server" class="cbToggle" Text="B" Checked="false"
                    Enabled="False" CssClass="cbToggle" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />
    <div id="divMensajeUnBotonNoAccion" title="Atenci&oacute;n" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align:top">
                    <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <asp:Label runat="server" ID="lblError" Text=""></asp:Label>
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
                        <%= Mensaje %>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divMensajeUnBotonUnaAccion" style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align:top">
                    <asp:Image ID="Image1" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                            <%= Mensaje %>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />
    <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />
</asp:Content>
