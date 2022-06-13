<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="InspectoresOPI.ascx.vb" Inherits="SEPRIS.InspectoresOPI" %>

<script type="text/javascript">
    <%--function AgregarInspector() {
        if ($("#<%=lstInspectorDisponible.ClientID%> option:selected").length > 0) {
            $("#<%=lstInspectorSeleccionado.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstInspectorDisponible.ClientID%> option:selected").val()).text($("#<%=lstInspectorDisponible.ClientID%> option:selected").text()));
            $("#<%=lstInspectorSeleccionado.ClientID%>").val($("#<%=lstInspectorDisponible.ClientID%>").val());
            $("#<%=lstInspectorDisponible.ClientID%> option:selected").remove();
        }
        
    }

    function QuitarInspector() {
        if ($("#<%=lstInspectorSeleccionado.ClientID%> option:selected").length > 0) {
            $("#<%=lstInspectorDisponible.ClientID%>").append($("<option></option>").attr("value", $("#<%=lstInspectorSeleccionado.ClientID%> option:selected").val()).text($("#<%=lstInspectorSeleccionado.ClientID%> option:selected").text()));
            $("#<%=lstInspectorSeleccionado.ClientID%> option:selected").remove();

        }
        
    }--%>
</script>

<asp:UpdatePanel ID="up1" runat="Server" UpdateMode="Conditional">
    <ContentTemplate>
        <table style="width:100%">
                    <tr>
                        <td colspan="4" style="text-align: left;" class="auto-style3">
                            <div id="ast6" runat="server" class="AsteriscoShow">*</div>
                            Inspector(es):</td>
                    </tr>
                    <tr>
                        <td style="width: 10%;">&nbsp;</td>
                        <td style="text-align: left; width: 40%;">Disponible(s)</td>
                        <td style="width: 10%">&nbsp;</td>
                        <td style="text-align: left; width: 40%">Seleccionado(s)</td>
                        
                    </tr>
                    <tr>
                        <td style="width: 10%;">&nbsp;</td>
                        <td rowspan="2" style="vertical-align: middle;">
                            <asp:ListBox ID="lstInspectorDisponible" runat="server" Width="95%" Height="110px"></asp:ListBox>
                        </td>
                        <%--<td style="vertical-align: bottom; text-align: center;">
                            <asp:ImageButton ID="imgAsignarInspector" runat="server" ImageUrl="~/Imagenes/FlechaRojaDer.gif" 
                                OnClientClick="AgregarInspector(); return false;" />
                        </td>--%>
                        <td style="vertical-align: bottom; text-align: center;">
                            <asp:ImageButton ID="imgAsignarInspector" runat="server" ImageUrl="~/Imagenes/FlechaRojaDer.gif" 
                                OnClick="imgAsignarInspector_Click" />
                        </td>
                        <td  rowspan="2" style="vertical-align: bottom;">
                            <asp:ListBox ID="lstInspectorSeleccionado" runat="server" Width="95%" Height="110px"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <%--<td style="vertical-align: top; text-align: center;">
                            <asp:ImageButton ID="imgDesasignarInspector" runat="server" ImageUrl="~/Imagenes/FlechaRojaIzq.gif" 
                                 OnClientClick="QuitarInspector(); return false;"/>
                        </td>--%>
                        <td style="vertical-align: top; text-align: center;">
                            <asp:ImageButton ID="imgDesasignarInspector" runat="server" ImageUrl="~/Imagenes/FlechaRojaIzq.gif" 
                                 OnClick="imgDesasignarInspector_Click"/>
                        </td>
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
            <%--<asp:ImageButton ID="btnDerecha" runat="server" ImageUrl="~/Images/FlechaRojaDer.gif"OnClick="imgAsignarInspector_Click" OnClick="imgDesasignarInspector_Click" OnClientClick="AgregarSupervisor(); return false;" />--%>
            <br />
            <%--<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/FlechaRojaIzq.gif" OnClientClick="QuitarSupervisor(); return false;"/>--%>


        <%--</td>--%>
        <%--<td>--%>
            <%--<asp:ListBox ID="lstSupervisorSeleccionado" runat="server" CssClass="txt_gral" onclick="GetSupervisores();"></asp:ListBox>--%>

        <%--</td>--%>

    <%--</tr>--%>
    
</table>
        
</ContentTemplate>
    <%--<Triggers>
                <asp:AsyncPostBackTrigger ControlID="imgDesasignarInspector" EventName="Click" />
            </Triggers>--%>
</asp:UpdatePanel>

