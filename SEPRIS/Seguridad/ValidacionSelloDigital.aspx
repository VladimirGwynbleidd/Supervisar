<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="ValidacionSelloDigital.aspx.vb" Inherits="SEPRIS.ValidacionSelloDigital" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            MensajeUnBotonNoAccionLoad();
        });

        function AquiMuestroMensaje() {
            MensajeUnBotonNoAccion();
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="udpSelloDigital" runat="server">
        <ContentTemplate>
            <div align="center" style="padding: 20px 20px 15px 20px">
                <label class="TitulosWebProyectos">
                    Validación Sello Digital</label>
            </div>
            <br />
            <div>
                <table style="width:100%; border-collapse:collapse;">
                    <tr>
                        <td style="width:20%; text-align:justify;" valign="top">
                            <asp:Label ID="lblSelloDigital" runat="server" Text="Sello digital:" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width:80%" align="right">
                            <asp:TextBox ID="txtSelloDigital" runat="server" Width="900px" TextMode="MultiLine" Columns="1000" Rows="8" 
                                  onkeyup="validaLimite(250, this, 'lblCharsRestantes')"></asp:TextBox>
                        </td>
                        <td valign="top">
                            <asp:CustomValidator runat="server" ControlToValidate="txtSelloDigital" ID="cvSelloDigital" SetFocusOnError="true"
                                ValidateEmptyText="true" ForeColor="Red" ValidationGroup="valSelloDigital">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Label ID="lblCharsRestantes" runat="server" ClientIDMode="Static" CssClass="txt_gral" Text="Caracteres restantes: 250"></asp:Label>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <br />
            <div>
                <asp:Button ID="btnValidar" runat="server" Text="Validar" OnClientClick="Deshabilita(this);"
                    CausesValidation="true" />
            </div>
            <div id="divMensajeUnBotonNoAccion" style="display: none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center; vertical-align: top">
                            <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="" />
                        </td>
                        <td style="text-align: left">
                            <div class="MensajeModal-UI">
                                <%= Mensaje %>
                                <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="valSelloDigital"
                                    CssClass="MensajeModal-UI" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <script type="text/javascript">
                
                function validaLimite(maxchar, obj, lblId) {
                    if (this.id) obj = this;
                    var remaningChar = maxchar - obj.value.length;
                    $("#" + lblId).html("Caracteres restantes: " + remaningChar);
                    if (remaningChar <= 0) {
                        obj.value = obj.value.substring(maxchar, 0);
                        return false;
                    }
                    else
                    { return true; }
                }
                
        </script>
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnValidar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
