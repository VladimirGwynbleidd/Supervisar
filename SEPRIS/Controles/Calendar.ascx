<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Calendar.ascx.vb" Inherits="SEPRIS.Calendar" %>
<asp:UpdatePanel ID="UPgeneral" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfIdRow" runat="server" />
        <asp:HiddenField ID="hfLinkId" runat="server" />
        <asp:DataList ID="dtlmes" runat="server" OnItemDataBound="DataList1_ItemDataBound"
            RepeatColumns="7" RepeatDirection="Horizontal" BackColor="#CCCCCC" Width="100%">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <table>
                    <tr>
                        <td style="text-align: center; color: #FFFFFF; background-color: DarkSeaGreen; width: 125px;
                            border-style: solid; border-width: thin; border-color: Gray;">
                            <asp:Label ID="lblDiaDes" runat="server" Text='<%# Eval("DiaDes") %>' Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 125px; border-style: solid; border-width: thin; border-color: Gray;
                            background: #999999;">
                            <asp:LinkButton ID="lblDia" runat="server" Text='<%# Eval("Dia") %>' Font-Bold="true"
                                ForeColor="Black"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkActividad1" runat="server" Text='<%# Eval("Actividad1") %>'
                                            ForeColor="Black" Font-Size="XX-Small" BackColor="#CCCCCC" 
                                            onclick="lnkActividad1_Click" ></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkActividad2" runat="server" Text='<%# Eval("Actividad2") %>'
                                            ForeColor="Black" Font-Size="XX-Small" BackColor="#CCCCCC" 
                                            onclick="lnkActividad2_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkActividad3" runat="server" Text='<%# Eval("Actividad3") %>'
                                            ForeColor="Black" Font-Size="XX-Small" BackColor="#CCCCCC" 
                                            onclick="lnkActividad3_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkActividad4" runat="server" Text='<%# Eval("Actividad4") %>'
                                            ForeColor="Black" Font-Size="XX-Small" BackColor="#CCCCCC" 
                                            onclick="lnkActividad4_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkActividad5" runat="server" Text='<%# Eval("Actividad5") %>'
                                            ForeColor="Black" Font-Size="XX-Small" BackColor="#CCCCCC" 
                                            onclick="lnkActividad5_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkActividad6" runat="server" Text='<%# Eval("Actividad6") %>'
                                            ForeColor="Black" Font-Size="XX-Small" BackColor="#CCCCCC" 
                                            onclick="lnkActividad6_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkActividad7" runat="server" Text='<%# Eval("Actividad7") %>'
                                            ForeColor="Black" Font-Size="XX-Small" BackColor="#CCCCCC" 
                                            onclick="lnkActividad7_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkActividad8" runat="server" Text='<%# Eval("Actividad8") %>'
                                            ForeColor="Black" Font-Size="XX-Small" BackColor="#CCCCCC" 
                                            onclick="lnkActividad8_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkActividad9" runat="server" Text='<%# Eval("Actividad9") %>'
                                            ForeColor="Black" Font-Size="XX-Small" BackColor="#CCCCCC" 
                                            onclick="lnkActividad9_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkActividad10" runat="server" Text='<%# Eval("Actividad10") %>'
                                            ForeColor="Black" Font-Size="XX-Small" BackColor="#CCCCCC" 
                                            onclick="LinkButton10_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkActividad11" runat="server" Text='<%# Eval("Actividad11") %>'
                                            ForeColor="Black" Font-Size="XX-Small" BackColor="#CCCCCC" 
                                            onclick="LinkButton11_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkActividad12" runat="server" Text='<%# Eval("Actividad12") %>'
                                            ForeColor="Black" Font-Size="XX-Small" BackColor="#CCCCCC" 
                                            onclick="LinkButton12_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" />
        </asp:DataList>
        <div id="divMensajeUnBotonNoAccion" style="display: none;" title="Detalle actividad">
            <table width="100%">
                <tr>
                    <td style="width: 50px; text-align: center; vertical-align: top">
                        <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png" />
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
    </ContentTemplate>
</asp:UpdatePanel>
<%--<asp:UpdateProgress runat="server" ID="PageUpdateProgress" DisplayAfter="0" AssociatedUpdatePanelID="UPgeneral"
    DynamicLayout="False">
    <ProgressTemplate>
        <div id="progressBackgroundFilter">
        </div>
        <div id="processMessage" style="text-align: center">
            Loading...<br />
            <br />
            <img alt="Loading" src="../Images/imageespera.gif" /></div>
    </ProgressTemplate>
</asp:UpdateProgress>--%>
