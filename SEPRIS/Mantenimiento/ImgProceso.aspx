<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="ImgProceso.aspx.vb" Inherits="SEPRIS.ImgProceso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pnlConsulta" runat="server" Visible="true">
        <div align="center" style="padding: 20px 20px 15px 20px">
            <label class="TitulosWebProyectos">Modificar Imagen del Proceso Inspección – Sanción</label>
        </div>
        <br /> <br />
        <div align="center" style="padding: 20px 20px 15px 20px">
            <table>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rdOpcImgProc" runat="server" RepeatDirection="Horizontal" CellSpacing="10" Width="400" AutoPostBack="false" >
                        <asp:ListItem Value="1" Text="Imagen para VF" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Imagen para VO, PLD y CGIV"></asp:ListItem> 
                    </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div align="center" id="fuCargaImgProceso" style="display:block; padding: 20px 20px 15px 20px;">
            <table>
                <tr>
                    <td>Selecciona la nueva imagen:</td>
                    <td><asp:FileUpload ID="fuImagenProc" runat="server" /></td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnCargarImg" runat="server" Text="Cargar Imagen"/>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

    <div id="divMensajeImgProceso" style="display: none" title="Información">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center; vertical-align: top">
                    <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/1Error.png" />
                </td>
                <td style="text-align: left">
                    <div class="MensajeModal-UI">
                        <%= Mensaje%>
                        <asp:ValidationSummary ID="vsErrores" runat="server" ValidationGroup="Forma" CssClass="MensajeModal-UI" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        function MostrarAviso() {
            Aviso("divMensajeImgProceso", 500, 300);
        };

        $(document).ready(function () {
            $("#<%=rdOpcImgProc.ClientID%>").change(function () {
                var rbvalue = $("input[name='<%=rdOpcImgProc.UniqueID%>']:radio:checked").val();

                //document.getElementById("fuCargaImgProceso").style.display = "block";
            });
        });

    </script>
</asp:Content>
