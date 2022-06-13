<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CopiarFolios.ascx.vb" Inherits="SEPRIS.CopiarFolios" %>
<script type="text/javascript">
    $(function () {
        MensajeDosBotonesUnaAccionAgcLoad("divMensajeDosBotonesUnaAccionDocsAcg", "btnAceptarCopiar", 900, 500);
    });

    function MuestraModal() {
        MensajeDosBotonesUnaAccionAgc("divMensajeDosBotonesUnaAccionDocsAcg", "btnAceptarCopiar", 900, 500);
    }

    function ConfirmacionCCF() {
        Confirmacion("divMensajeConfirmaRegistroCCF", "btnConfirmar", 500, 300);
    }

    function AvisoCCF() {
        Aviso("divAvisoRegistro", 500, 300);
    }

    function Mensaje() {
        alert("Selecciona alguna visita!");
    }

    function ConfirmacionCopia() {
        ConfirmacionSiNo("divMensajeConfirmaRegistroCCF", "btnConfirmar", "btnConfirmarNo", 1, 500, 300);
    }
</script>

<asp:CheckBox ID="chkCopiarFolio" runat="server" Text="Copiar Folio"  AutoPostBack="true"/>

<%--modal que muestra el grid--%>
    <div id="divMensajeDosBotonesUnaAccionDocsAcg" style="display:none">
        <table width="98%">
            <tr>
                <td style="width: 100%; text-align: center;">
                    <h3 style="color:green;">Selecciona alguna visita para replicar su información.</h3>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <cc1:CustomGridView ID="gvConsulta" runat="server" DataKeyNames="I_ID_VISITA" Width="100%" DataBindEnPostBack="true"
                        AllowSorting="false" SkinID="SeleccionSimpleCliente2" HiddenFieldSeleccionSimple="hfSelectedValue">
                            <Columns>
                               <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkElemento" runat="server"  AutoPostBack="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Folio" DataField="T_ID_FOLIO" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField HeaderText="Fecha inicio de visita" DataField="F_FECH_VISITA" HeaderStyle-Width="130px" ItemStyle-Width="130px" ItemStyle-CssClass="Wrap"/>
                                <asp:BoundField HeaderText="Entidad" DataField="T_DSC_ENTIDAD" HeaderStyle-Width="70px" ItemStyle-Width="100px" ItemStyle-CssClass="Wrap" />
                                <asp:BoundField HeaderText="Tipo de visita" DataField="T_DSC_TIPO_VISITA" HeaderStyle-Width="35px" ItemStyle-Width="35px" ItemStyle-CssClass="Wrap" />
                                <asp:BoundField HeaderText="# Paso actual" DataField="I_ID_PASO_ACTUAL" HeaderStyle-Width="35px" ItemStyle-Width="35px" ItemStyle-CssClass="Wrap" />
                                <asp:BoundField HeaderText="Responsable de inspección" DataField="T_DSC_NOMBRE_INSPECTOR_RESPONSABLE" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-CssClass="Wrap" />
                                <asp:BoundField HeaderText="Abogado sanciones" DataField="T_DSC_NOMBRE_ABOGADO_SANCION" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-CssClass="Wrap" />
                            </Columns>
                        </cc1:CustomGridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div id="divMensajeConfirmaRegistroCCF" style="display: none" title="Confirmación de Registro de visita">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgConfirmaRegistroVisitaCC" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div id="divAvisoRegistro" style="display: none" title="Confirmación de Registro de visita">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgAviso" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
    <asp:Button runat="server" ID="btnAceptarCopiar" Style="display: none" ClientIDMode="Static" />
    <asp:Button runat="server" ID="btnConfirmar" Style="display: none" ClientIDMode="Static"/>
    <asp:Button runat="server" ID="btnConfirmarNo" Style="display: none" ClientIDMode="Static" OnClick="btnConfirmarNo_Click"/>
    <asp:Button runat="server" ID="btnConsulta" Style="display: none" />