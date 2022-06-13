<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SupervisorOPI.ascx.vb" Inherits="SEPRIS.SupervisorOPI" %>

<script type="text/javascript">
    <%--function AgregarSupervisor() {
        if ($("#<%=lstSupervisorDisponible.ClientID%> option:selected").length > 0) {
            $("#<%=lstSupervisorSeleccionado.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstSupervisorDisponible.ClientID%> option:selected").val()).text($("#<%=lstSupervisorDisponible.ClientID%> option:selected").text()));
            $("#<%=lstSupervisorSeleccionado.ClientID%>").val($("#<%=lstSupervisorDisponible.ClientID%>").val());
            $("#<%=lstSupervisorDisponible.ClientID%> option:selected").remove();
        }
        
    }

    function QuitarSupervisor() {
        if ($("#<%=lstSupervisorSeleccionado.ClientID%> option:selected").length > 0) {
            $("#<%=lstSupervisorDisponible.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstSupervisorSeleccionado.ClientID%> option:selected").val()).text($("#<%=lstSupervisorSeleccionado.ClientID%> option:selected").text()));
            $("#<%=lstSupervisorSeleccionado.ClientID%> option:selected").remove();

        }
        
    }--%>
</script>

<asp:UpdatePanel ID="up1" runat="Server">
    <ContentTemplate>
        <table style="width:100%">
    <tr>
                        <td colspan="4" style="text-align: left;" class="auto-style3"><div id="ast6" runat="server" class="AsteriscoShow">*</div>
                            Supervisor(es):</td>
                    </tr>
                    
                    <tr>
                        <td style="width: 10%;">&nbsp;</td>
                        <td style="text-align: left; width: 40%;">
                            Disponible(s)</td>
                        <td style="width: 10%">&nbsp;</td>
                        
                        <td style="text-align: left; width: 40%">Seleccionado(s)
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%;">&nbsp;</td>
                        <td rowspan="2" style="vertical-align: middle;">
                            <asp:ListBox ID="lstSupervisorDisponible" runat="server" Width="95%" Height="110px"></asp:ListBox>
                        </td>
                        <td style="vertical-align: bottom; text-align: center;">
                            <asp:ImageButton ID="imgAsignarSupervisor" runat="server" ImageUrl="~/Imagenes/FlechaRojaDer.gif" 
                                OnClick="imgAsignarSupervisor_Click" />
                        </td>
                        <%--<td style="vertical-align: bottom; text-align: center;">
                            <asp:ImageButton ID="imgAsignarSupervisor" runat="server" ImageUrl="~/Imagenes/FlechaRojaDer.gif" 
                                OnClientClick="AgregarSupervisor(); return false;" />
                        </td>--%>
                        <td  rowspan="2" style="vertical-align: bottom;">
                            <asp:ListBox ID="lstSupervisorSeleccionado" runat="server" Width="95%" Height="110px"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="vertical-align: top; text-align: center;">
                            <asp:ImageButton ID="imgDesasignarSupervisor" runat="server" ImageUrl="~/Imagenes/FlechaRojaIzq.gif" 
                                OnClick="imgDesasignarSupervisor_Click"  />
                        </td>
                        <%--<td style="vertical-align: top; text-align: center;">
                            <asp:ImageButton ID="imgDesasignarSupervisor" runat="server" ImageUrl="~/Imagenes/FlechaRojaIzq.gif" 
                                OnClientClick="QuitarSupervisor(); return false;" />
                        </td>--%>
                    </tr>
                    <tr>
                        <td colspan="4"></td>
                    </tr>
                    <tr>
                        <td colspan="4"></td>
                    </tr>
    
    <%--<tr>--%>
        <%--<td><asp:ListBox ID="lstSupervisorDisponible" runat="server" CssClass="txt_gral"></asp:ListBox> </td>--%>
        <%--<td>--%>
            <%--<asp:ImageButton ID="btnDerecha" runat="server" ImageUrl="~/Images/FlechaRojaDer.gif"OnClick="imgDesasignarSupervisor_Click" OnClientClick="AgregarSupervisor(); return false;" />--%>
            <br />
            <%--<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/FlechaRojaIzq.gif" OnClientClick="QuitarSupervisor(); return false;"/>--%>


        <%--</td>--%>
        <%--<td>--%>
            <%--<asp:ListBox ID="lstSupervisorSeleccionado" runat="server" CssClass="txt_gral" onclick="GetSupervisores();"></asp:ListBox>--%>

        <%--</td>--%>

    <%--</tr>--%>
    
</table>

        <%--<asp:ListBox ID="listBoxSubeKiyaslama1" runat="server">
        </asp:ListBox>
        <asp:ListBox ID="listBoxSubeKiyaslama2" runat="server">
        </asp:ListBox>--%>
    </ContentTemplate>
</asp:UpdatePanel>

