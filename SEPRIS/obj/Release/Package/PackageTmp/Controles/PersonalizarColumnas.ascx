<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PersonalizarColumnas.ascx.vb"
    Inherits="SEPRIS.PersonalizarColumnas" %>

<script type="text/javascript">

    /* invocamos creacion de mensaje desde MensajeModal.js */
    $(function () {


        MensajePersonalizaColumnasControlLoad();


    });



    function MostramosPersonalizaColumnasControl() {

        MensajePersonalizaColumnasControl();

    }


</script>

<div id="divPersonalizaColumnasControlPadre" style="display: none;" title="Personalizar Columnas">
    <asp:UpdatePanel ID="upControlesPersonaliza" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table align="center" style="width: 650px;" border="0" class="PersonalizaColumnas">
                <tr>
                    <td colspan="3" align="center">
                        <asp:Label ID="lblTitulo" runat="server" Text="Personalizar Columnas" CssClass="txt_gral"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblTituloSinAsignar" runat="server" Text="Ocultar" CssClass="txt_gral"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td align="center">
                        <asp:Label ID="lblTituloAsignados" runat="server" Text="Ver" CssClass="txt_gral"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ListBox ID="lstSinAsignar" runat="server" Height="150px" Width="249px" CssClass="txt_gral" SelectionMode="Multiple">
                        </asp:ListBox>
                    </td>
                    <td align="center">
                        <asp:Button ID="btnPasarUnoVer" runat="server" Text="&gt;" Width="80px" Height="20px"
                            class="botones" />
                        <br />
                        <br />
                        <asp:Button ID="btnPasarTodosVer" runat="server" Text="&gt;&gt;" Width="80px" Height="20px"
                            class="botones" />
                        <br />
                        <br />
                        <asp:Button ID="btnPasarUnoOcultar" runat="server" Text="&lt;" Width="80px" Height="20px"
                            class="botones" />
                        <br />
                        <br />
                        <asp:Button ID="btnPasarTodosOcultar" runat="server" Text="&lt;&lt;" Width="80px"
                            Height="20px" class="botones" />
                    </td>
                    <td align="center">
                        <asp:ListBox ID="lstAsignado" runat="server" Height="150px" Width="249px" CssClass="txt_gral">
                        </asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center" >
                        <asp:Button ID="btnGuardarPersonalizacion" runat="server" Width="93px" CssClass="botones"
                            Text="Guardar" Height="22px" ClientIDMode="Static" Style="display:none;"></asp:Button>
                            &nbsp;
                        <asp:Button ID="btnCancelarPersonalizacion" runat="server" Width="93px" CssClass="botones"
                            Text="Cancelar" Height="22px" ClientIDMode="Static" Style="display:none;"></asp:Button>
                    </td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ID="hdnIdGridView" Value="0" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGuardarPersonalizacion" />
            <asp:PostBackTrigger ControlID="btnCancelarPersonalizacion" />
        </Triggers>
    </asp:UpdatePanel>
</div>
