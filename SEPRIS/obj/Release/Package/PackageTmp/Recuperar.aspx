<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Recuperar.aspx.vb" Inherits="SEPRIS.Recuperar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(function () {

            MensajeUnBotonNoAccionLoad();

        });

        function MostrarMensaje() {

            MensajeUnBotonNoAccion();

        };

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div>
    <p style="text-align: center; vertical-align: middle">
        <asp:Label ID="lblTitulo" runat="server" Text="Olvidé mi Contraseña" CssClass="TitulosWebProyectos" EnableTheming="false"></asp:Label>
    </p>
</div>
<div>
    <asp:Label ID="lblMensaje" runat="server" CssClass="txt_gral" Text="Ingrese su correo electrónico y en breve recibirá su contraseña:"></asp:Label>
    <br />
    <br />
    <br />
</div>
<div>

    <table>
        <tr>
            <td align="right"><label class="txt_gral">Usuario: </label></td>
            <td align="left"><asp:TextBox ID="txtUsuario" runat="server" CssClass="txt_gral" Width="150px" MaxLength="16"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right"><label class="txt_gral">Correo electrónico: </label></td>
            <td align="left"><asp:TextBox ID="txtCorreo" runat="server" CssClass="txt_gral" Width="250px"></asp:TextBox></td>
        </tr>

    </table>
    
    <br />
    <br />
    <br />
</div>
<div>
    <asp:Button ID="btnEnviar" runat="server" CssClass="botones" Text="ENVIAR" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnRegresar" runat="server" CssClass="botones" Text="REGRESAR" />
</div>
<br />
<br />
<br />
<br />
<span class="txt_gral">
<strong>Nota:</strong> Si usted ya solicitó su contraseña y no se ha resuelto su problema, por favor envíe su problemática a los <asp:LinkButton ID="lnkResponsables" runat="server" PostBackUrl="Responsables.aspx" Text="Responsables del Sistema"></asp:LinkButton>
</span>

    <div id="divMensajeUnBotonNoAccion">
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


</asp:Content>
