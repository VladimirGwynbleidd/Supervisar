<%@ Page Language="vb" MasterPageFile="~/Master/SiteInterno.Master" AutoEventWireup="false"
    CodeBehind="CarruselAsignacion.aspx.vb" Inherits="SEPRIS.CarruselAsignacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {

            MensajeUnBotonNoAccionLoad();

        });

        function MensajeModal() {

            MensajeUnBotonNoAccion();

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center" style="padding: 20px 20px 15px 20px">
        <asp:Label runat="server" ID="lblTitulo" Text="CARRUSEL DE ASIGNACIONES" CssClass="TitulosWebProyectos" EnableTheming="false"></asp:Label>
    </div>
    <div style="text-align: left; width: 100%; padding-bottom: 5px;">
        <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
    </div>
    <div align="center">
        <cc1:CustomGridView ID="gvCarrusel" runat="server" SkinID="SeleccionSimpleCliente"
            Width="100%" ToolTipHabilitado="false">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                    <ItemStyle Width="15px" />
                     <HeaderStyle Width="15px" />
                </asp:TemplateField>
                <asp:BoundField HeaderText="Usuario" DataField="T_ID_USUARIO" SortExpression="Usuario"
                    ReadOnly="true" >
                    <ItemStyle Width="50px" />
                    <HeaderStyle Width="50px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Nombre" DataField="NOMBRE" SortExpression="Usuario" ReadOnly="true" />
                <asp:BoundField HeaderText="Orden" DataField="N_NUM_ORDEN" />
                <asp:TemplateField HeaderText="Subir" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgbSubir" runat="server" CommandName="Subir" CommandArgument='<%# Container.DataItemIndex %>'
                            ImageUrl="../Imagenes/up_arrow_grey.png" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bajar" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgBajar" runat="server" CommandName="Bajar" CommandArgument='<%# Container.DataItemIndex %>'
                            ImageUrl="../Imagenes/down_arrow_grey.png" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Recibe" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbRecibe" runat="server" AutoPostBack="true" OnCheckedChanged="chkRecibe_CheckedChanged"
                            Checked='<%# verificaRecibe (DataBinder.Eval (Container.DataItem, "N_FLAG_VIG")) %>'>
                        </asp:CheckBox>
                    </ItemTemplate>
                    <ItemStyle Width="50px" />
                    <HeaderStyle Width="50px" />
                </asp:TemplateField>
            </Columns>
        </cc1:CustomGridView>
        <div style="padding: 30px" align="center">
            <asp:Button ID="cmdModificar" runat="server" Height="22px" Width="250px" Text="Modificar Periodos de No Asignación"
                OnClientClick="Deshabilita(this);"></asp:Button>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="BtnSiguienteAsig" runat="server" Height="22px" Width="250px" Text="Obtener Siguiente Asignación"
                OnClientClick="Deshabilita(this);"></asp:Button>
        </div>
    </div>

    <div id="divMensajeUnBotonNoAccion" title="">
        <table width="100%">
            <tr>
                <td style="width: 50px; text-align: center">
                    <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px" ImageUrl="~/Imagenes/Errores/Error1.png"/>
                </td>
                <td style="text-align: left">
                    <asp:Label ID="lblTextoMensaje" runat="server" CssClass="MensajeModal-UI" EnableTheming="false"></asp:Label>
                </td>
            </tr>
        </table>
    </div>



    <asp:HiddenField ID="hfGridView1SV" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfSelectedValue" runat="server" ClientIDMode="Static" Value="-1" />
</asp:Content>
