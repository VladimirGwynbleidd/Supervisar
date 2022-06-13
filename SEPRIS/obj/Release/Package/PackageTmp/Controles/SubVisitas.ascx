<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SubVisitas.ascx.vb" Inherits="SEPRIS.SubVisitas" %>
<script type="text/javascript">
    $(function () {
        MensajeDosBotonesUnaAccionAgcLoad("divGeneraSubvisita", "btnGeneraSubvisita", 700, 500);
    });

    function MuestraModal() {
        MensajeDosBotonesUnaAccionAgc("divGeneraSubvisita", "btnGeneraSubvisita", 700, 500);
    }

    function AvisoCCF() {
        Aviso("divAvisoRegistro", 500, 300);
    }

    function ConfirmacionSubvisitas() {
        Confirmacion("divAvisoRegistro", "btnConfirmaReg", 500, 300);
    }
    
</script>

<asp:CheckBox ID="chkSubVisitas" runat="server" AutoPostBack="true"  />


<%--modal que muestra las subentidades--%>
    <div id="divGeneraSubvisita" style="display:none">
        <table style="width:90%">
            <tr>
                <td style="width: 100%; text-align: center;">
                    <h3 style="color:green;">Selecciona alguna subentidad.</h3>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    <asp:Label ID="lblMsg" runat="server" Text="" Visible ="false" ForeColor="Red"></asp:Label>
                    <div>
                        <asp:CheckBoxList ID="chkSubEntidad" ClientIDMode="Static"  runat="server" ></asp:CheckBoxList>
                    </div>
                </td>
            </tr>
        </table>
    </div>


    <div id="divAvisoRegistro" style="display: none" title="Confirmación de Registro de subvisita">
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

    <asp:Button runat="server" ID="btnGeneraSubvisita" Style="display: none" ClientIDMode="Static" OnClick="btnGeneraSubvisita_Click" />
    <asp:Button runat="server" ID="btnConfirmaReg" Style="display: none" ClientIDMode="Static" OnClick="btnConfirmaReg_Click"/>