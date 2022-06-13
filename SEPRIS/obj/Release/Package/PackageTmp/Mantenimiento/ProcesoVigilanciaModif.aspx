<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="ProcesoVigilanciaModif.aspx.vb" Inherits="SEPRIS.ProcesoVigilanciaModif" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <table id="table3" cellspacing="0" cellpadding="0" width="97%" align="center" border="0" style="height:450px;background-attachment: scroll;">
            <tr>
                <td colspan="4" align="center">
                    <br />
                    <div align="center" style="padding: 20px 20px 15px 20px">
                   <asp:Label class="TitulosWebProyectos" EnableTheming="false" runat="server"  ID="lblRegistro" >
                       Registro </asp:Label>
                </div>
                   
                </td>
            </tr>

            <!--Espacio para el contenido de la página-->
            <tr>
                <td style="height: 10px">
                </td>
            </tr>
            <tr align="center" valign="top" style="height:410px;width:97%">
                <td>
                    <table runat="server" id="tblContenido" class="forma" width="90%" border="0">
                        <tr>
                            <td align="right">
                                &nbsp;
                            </td>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblPrincipal" runat="server" ForeColor="Navy" Font-Size="8pt">* Datos Obligatorios</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">

                            </td>
                        </tr>

                        <tr>
                            <td style="width: 277px">
                                &nbsp;
                            </td>
                            <td align="left" style="width: 523px; height: 24px" colspan="2">
                                <asp:Button ID="btnAceptar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                                runat="server" Width="123px" Height="21px" Text="Guardar" CssClass="botones"></asp:Button>
                                &nbsp;
                                <asp:Button ID="btnCancelar" onmouseover="style.backgroundColor='#A9A9A9'" onmouseout="style.backgroundColor='#696969'"
                                runat="server" Width="123px" Height="21px" Text="Cancelar" CssClass="botones"></asp:Button>
                           </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">

                            </td>

                        </tr>

                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
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

        <div id="divMensajeConfirmacionDosBotonesUnaAccion" style="display: none">
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
            function MensajeConfirmacionDosBotones() {

                $("#divMensajeConfirmacionDosBotonesUnaAccion").dialog({
                    resizable: false,
                    autoOpen: true,
                    height: 300,
                    width: 500,
                    modal: true,
                    //closeText: 'Cerrar',
                    open:
                        function (event, ui) {
                            $(this).parent().css('z-index', 3999);
                            $(this).parent().appendTo(jQuery("form:last"));
                            $('.ui-widget-overlay').css('position', 'fixed');
                            $('.ui-widget-overlay').css('z-index', 3998);
                            $('.ui-widget-overlay').appendTo($("form:last"));
                        },
                    buttons: {
                        "Si": function () {
                            $(this).dialog("option", "buttons", [{ text: "Procesando...", disabled: true }, { text: "No", disabled: true }]);
                            $('#btnAceptarM2B1A').trigger("click");
                        },
                        "No": function () {
                            $(this).dialog("close");
                        }
                    }
                });
            }
        </script>
    </div>
</asp:Content>