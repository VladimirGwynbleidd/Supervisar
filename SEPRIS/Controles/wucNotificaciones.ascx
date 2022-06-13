<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="wucNotificaciones.ascx.vb"
    Inherits="SEPRIS.wucNotificaciones" %>
<script type="text/javascript">



    /* invocamos creacion de mensaje desde MensajeModal.js */
    $(function () {


        MensajeNotificacionUsuarioLoad();


    });


    function MostramosNotificacionUsuario() {

        MensajeNotificacionUsuario();

    }


    function CerramosNotificacion() {

        try{
            $("#divMensajeNotificacionUsuario").dialog("close");
        }
        catch(e){
            return true;
        }

        

    }



</script>
<div id="divMensajeNotificacionUsuario" title="" style="display: none" runat="server"
    clientidmode="Static">
    <table style="width: 100%; height: 390px">
        <tr>
            <td>
                &nbsp;
            </td>
            <td rowspan="2" style="vertical-align: top;">
                &nbsp;<div id="divmensaje" runat="server">
                    <br />
                    <asp:Label ID="lblMensajeNotificacion" runat="server" Text="Label"></asp:Label>
                    <br />
                    <br />
                    <br />
                    <br />
                </div>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="height: 69px;">
                &nbsp;
            </td>
            <td style="text-align: center; height: 69px;">
                &nbsp;&nbsp;&nbsp;
                <div style="display: none">
                    <asp:Button ID="btnArchivo" ClientIDMode="Static" runat="server" Text="Descargar Archivo Adjunto"
                        Width="147px" CssClass="botones" />
                </div>
                &nbsp;<asp:Button ID="btnEnterado" runat="server" Text="Enterado" Width="147px" CssClass="botones" />
                <div style="padding: 5px; margin: 5px">
                    <div style="">
                    </div>
                </div>
            </td>
            <td style="height: 5px;">
                &nbsp;
            </td>
        </tr>
    </table>
</div>
