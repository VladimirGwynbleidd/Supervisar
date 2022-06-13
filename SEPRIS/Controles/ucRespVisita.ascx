<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucRespVisita.ascx.vb" Inherits="SEPRIS.ucRespVisita" %>
    <style type="text/css">
        .AsteriscoHide {
            display: none;
        }

        .AsteriscoShow {
            display: inline;
            color: red;
            font-family: Verdana;
            font-size: 1.2em;
            font-weight: bold;
        }

        .Oculto {
            display: none;
        }
    </style>

        <asp:UpdatePanel runat="server" ID="udpAsignacionUsuarios" UpdateMode="Conditional">
            <ContentTemplate>
                <br />
                <table id="tbAsignacionInspectores" runat="server" style="width: 90%; margin-top: 20px; border-collapse: collapse; margin-left:5%;">
                    <tr>
                        <td colspan="3" style="text-align: center ;">Responsables de la inspección:</td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="text-align: center ; width: 40%;">Usuarios Disponibles</td>
                        <td style="width: 10%">&nbsp;</td>
                        <td style="text-align: left; width: 40%">Supervisor
                                        &nbsp;&nbsp;
                                        <div id="ast6" runat="server" class="AsteriscoHide">*</div>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="5" style="vertical-align: middle;">
                            <asp:ListBox ID="lsbUsuariosDisponibles" runat="server" Width="100%" Height="230px"></asp:ListBox>
                        </td>
                        <td style="vertical-align: bottom; text-align: center;">
                            <asp:ImageButton ID="imgAsignarSupervisor" runat="server" ImageUrl="~/Imagenes/FlechaRojaDer.gif" OnClientClick="Deshabilita(this);"/>
                            &nbsp;
                        </td>
                        <td rowspan="2" style="vertical-align: bottom;">
                            <asp:ListBox ID="lsbSupervisor" runat="server" Width="100%" Height="110px"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; text-align: center;">
                            <asp:ImageButton ID="imgDesasignarSupervisor" runat="server" ImageUrl="~/Imagenes/FlechaRojaIzq.gif" OnClientClick="Deshabilita(this);" />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td style="text-align: left; height: 10px;">Inspector (es)
                                        &nbsp;&nbsp;
                                        <div id="ast7" runat="server" class="AsteriscoHide">*</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: bottom; text-align: center">
                            <asp:ImageButton ID="imgAsignarInspector" runat="server" ImageUrl="~/Imagenes/FlechaRojaDer.gif" OnClientClick="Deshabilita(this);"/>
                            &nbsp;
                        </td>
                        <td rowspan="2" style="vertical-align: top;">
                            <asp:ListBox ID="lsbInspectores" runat="server" Width="100%" Height="110px"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; text-align: center;">
                            <asp:ImageButton ID="imgDesasignarInspector" runat="server" ImageUrl="~/Imagenes/FlechaRojaIzq.gif" OnClientClick="Deshabilita(this);"/>
                            &nbsp;
                        </td>
                    </tr>
                </table>

                <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>