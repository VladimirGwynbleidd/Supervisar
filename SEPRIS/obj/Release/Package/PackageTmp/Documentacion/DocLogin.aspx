<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocLogin.aspx.vb" Inherits="SEPRIS.DocLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación de Login Interno y Login Externo</label>
                </div>
                <br />
                <br />
                <asp:Button ID="btnManualUsuario" runat="server" Text="Manual Usuario" />
                <br />
                <br />
                <asp:Button ID="btnManualTecnico" runat="server" Text="Manual Técnico" />
            </asp:Panel>
            <asp:Panel ID="pnlUsuario" runat="server" Visible="false">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Label ID="lblTituloManualUsuario" runat="server" CssClass="TitulosWebProyectos"
                        Text=""></asp:Label>
                </div>
                <asp:Panel ID="pnlPrincipalUsuario" runat="server">
                    <div style="text-align: left;" class="txt_gral">
                        <p class="SubTitulosWebProyectos">
                            Acceso al sistema.</p>
                        <p class="txt_gral justificado">
                            1. Ingresar los campos usuario y contraseña.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_19c42d78e19ae773_html_m6d3d0158.jpg" alt="" />
                            <br />
                            Figura 1
                        </p>
                        <p class="txt_gral justificado">
                            2. Hacer clic en el botón Aceptar.
                            </p>

                        <p class="txt_gral justificado">
                            Excepción: En caso de que los datos ingresados no sean correctos el sistema mostrara un texto con la leyenda “Compruebe su Usuario/Contraseña. No se puede iniciar sesión.”
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_19c42d78e19ae773_html_m5ee24b23sa.png" alt="" />
                            <br />
                            Figura 1

                        <p class="txt_gral justificado">
                            3. Fin.
                        </p>
                        <br />
                        <p class="SubTitulosWebProyectos">
                            OLVIDÉ MI CONTRASEÑA
                        </p>
                        <p class="txt_gral justificado">
                            Para el caso de un sistema externo en que un usuario no recuerde su contraseña podrá
                            recuperarla usando este método.
                        </p>
                        <p class="txt_gral justificado">
                            1. Dar clic sobre el texto “Olvide mi contraseña” que se muestra debajo del campo
                            contraseña.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_19c42d78e19ae773_html_m5ee24b2.png" alt="" />
                            <br />
                            Figura 2
                        </p>
                        <p class="txt_gral justificado">
                            2. En la siguiente pantalla a la cual nos dirigido el sistema capturar el usuario
                            y correo electrónico y dar clic en el botón enviar.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_19c42d78e19ae773_html_7e5e0268.jpg" alt="" />
                            <br />
                            Figura 3
                        </p>
                        <p class="txt_gral justificado">
                            Se muestra un mensaje indicando al usuario que se enviará un correo con la información
                            de la contraseña.
                        </p>
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_19c42d78e19ae773_html_33244718.jpg" alt="" />
                            <br />
                            Figura 4
                        </p>
                        <p class="txt_gral justificado">
                            3. Fin.
                        </p>
                    </div>
                    <asp:Button ID="btnRegresarUusario" runat="server" Text="Regresar" />
                    <asp:Button ID="btnDescargaManual" runat="server" Text="Descarga Manual" OnClientClick="parent.location.href='../Manuales/Manual Usuario.pdf'" Width="110px" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlTecnico" runat="server" Visible="false">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Label ID="lblTituloManualTecnico" runat="server" CssClass="TitulosWebProyectos"
                        Text=""></asp:Label>
                </div>
                <asp:Panel ID="pnlPrincipalTecnico" runat="server">
                    <table>
                        <tr>
                            <td align="right">
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnRegresarTecnico" runat="server" Text="Regresar" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnManualUsuario" />
            <asp:PostBackTrigger ControlID="btnManualTecnico" />
            <asp:PostBackTrigger ControlID="btnRegresarUusario" />
            <asp:PostBackTrigger ControlID="btnRegresarTecnico" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
