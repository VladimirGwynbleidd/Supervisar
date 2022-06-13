<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="Bandeja.aspx.vb" Inherits="SEPRIS.Bandeja" %>

<%@ Register src="~/Controles/ucFiltro.ascx" tagname="ucFiltro" tagprefix="uc1" %>

<%@ Register Src="~/Controles/wucPersonalizarColumnas.ascx" TagName="wucPersonalizarColumnas" TagPrefix="wuc" %>

<%@ Register Src="~/Controles/wucAyuda.ascx" TagName="wucAyuda" TagPrefix="wcu" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.7.123, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <wcu:wucAyuda ID="Ayuda" runat="server"/>
    
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" Visible="true">
                
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">Ejemplo de Bandeja</label>
                </div>                

                <div style="text-align:left; width:800px; padding-bottom: 5px;">
                    <asp:Button ID="btnExportaExcel" runat="server" Text="Exportar a Excel" />
                    &nbsp;
                    <asp:Button ID="btnPersonalizaColumnas" runat="server" Text="Personalizar Columnas" Width="150px" />
                    
                </div>
                    
                <uc1:ucFiltro ID="ucFiltro1" runat="server" Width="800px" />

                <div id="pnlGrid" runat="server">

                    <div style="text-align:left; width:800px; padding: 5px;">
                        <asp:CheckBox ID="chkSeleccionaTodos" runat="server" AutoPostBack="true" />
                        <asp:Label ID="lblSeleccionaTodos" runat="server" Text="Seleccionar Todo" CssClass="txt_gral"></asp:Label>
                    </div>

                    <cc1:CustomGridView ID="gvConsulta" runat="server" DataKeyNames="N_ID_PARAMETRO" SkinID="SeleccionMultiple"                     
                        HabilitaScroll="true" WidthScroll="800" HeigthScroll="250" ColumnasCongeladas="2"
                        AllowSorting="true"  GridLines="None" AllowPaging="true" >
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkElemento" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelecteds2_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Clave" DataField="N_ID_PARAMETRO" HeaderStyle-Width="80px" ItemStyle-Width="80px" SortExpression="N_ID_PARAMETRO" />
                            <asp:BoundField HeaderText="Parámetro" DataField="T_DSC_PARAMETRO" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-CssClass="Wrap" SortExpression="T_DSC_PARAMETRO" />
                            <asp:BoundField HeaderText="Descripción" DataField="T_OBS_PARAMETRO" HeaderStyle-Width="300px" ItemStyle-Width="300px" ItemStyle-CssClass="Wrap" />
                            <asp:BoundField HeaderText="Valor" DataField="T_DSC_VALOR" HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-CssClass="Wrap" />
                            <asp:TemplateField HeaderText="Estatus" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                <ItemTemplate >
                                    <asp:Image ID="imagenEstatus" runat="server" ImageUrl='<%# ObtenerImagenEstatus(DataBinder.Eval(Container.DataItem, "N_FLAG_VIG"))  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </cc1:CustomGridView>
                </div>

                <div id="pnlNoExiste" runat="server" align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Image ID="Image1" runat="server" 
                         AlternateText="No existen registros para la consulta" ImageAlign="Middle" 
                         imageurl="../Imagenes/no EXISTEN.gif" />
                </div>

                <asp:HiddenField ID="hfGridView1SV_gvConsulta" runat="server" ClientIDMode="Static" /> 
                <asp:HiddenField ID="hfGridView1SH_gvConsulta" runat="server" ClientIDMode="Static" /> 
                <asp:HiddenField ID="hfSelectedValue_gvConsulta" runat="server" ClientIDMode="Static" Value="-1" />

                <br />

                <asp:Panel ID="pnlSignificadoImagen" runat="server">
                    <fieldset style="width: 800px">
                        <legend class="txt_gral">&nbsp;Estatus&nbsp;</legend>
                        <asp:Image ID="imgOK" runat="server"/>
                        <label class="txt_gral">Vigente</label>
                        <asp:Image ID="imgERROR" runat="server" />
                        <label class="txt_gral">No vigente</label>
                    </fieldset>
                </asp:Panel>    
                                           
                <br />

                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnModificar" runat="server" Text="Modificar" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar"  />
            </asp:Panel>

            <asp:Panel ID="pnlDetalle" runat="server" Visible="false">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <asp:label ID="lblDetalle" runat="server" class="TitulosWebProyectos">Inserta Registro</asp:label>
                </div>
                <br />
                <asp:Label ID="Label1" runat="server" CssClass="txt_gral">En esta pantalla se insertara, modificara o vera el detalle de un registro</asp:Label>
                <br /><br />
                <asp:Button ID="btnRegresa" runat="server" Text="Regresa" />
            </asp:Panel>
        
            <div id="divMensajeUnBotonNoAccion" style="display:none">
                <table width="100%">
                    <tr>
                        <td style="width: 25%; text-align: center">
                            <asp:Image ID="imgUnBotonNoAccion" runat="server" Width="32px" Height="32px"/>
                        </td>
                        <td  style="width: 75%; text-align: left">
                            <%= Mensaje %>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="divConfirmacionM2B2A" style="display:none">
                <table width="100%">
                    <tr>
                        <td style="width: 25%; text-align: center">
                            <asp:Image ID="imgM2B2A" runat="server" Width="32px" Height="32px"/>
                        </td>
                        <td  style="width: 75%; text-align: left">
                            <%= Mensaje %>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="divMensajeUnBotonUnaAccion" style="display:none">
                <table width="100%">
                    <tr>
                        <td style="width: 25%; text-align: center">
                            <asp:Image ID="imgUnBotonUnaAccion" runat="server" Width="32px" Height="32px"/>
                        </td>
                        <td  style="width: 75%; text-align: left">
                            <%= Mensaje %>
                        </td>
                    </tr>
                </table>
            </div>

    
            <div id="divMensajeDosBotonesUnaAccion" style="display:none">
                <table width="100%">
                    <tr>
                        <td style="width: 25%; text-align: center">
                            <asp:Image ID="imgDosBotonesUnaAccion" runat="server" Width="32px" Height="32px"/>
                        </td>
                        <td  style="width: 75%; text-align: left">
                            <%= Mensaje %>
                        </td>
                    </tr>
                </table>
            </div>


            <asp:Button runat="server" ID="btnSalirM2B2A" Style="display: none" ClientIDMode="Static" />
            <asp:Button runat="server" ID="btnContinuarM2B2A" Style="display: none" ClientIDMode="Static" />   
            <asp:Button runat="server" ID="btnAceptarM1B1A" Style="display: none" ClientIDMode="Static" />   
            <asp:Button runat="server" ID="btnAceptarM2B1A" Style="display: none" ClientIDMode="Static" />   
            <asp:Button runat="server" ID="btnConsulta" Style="display: none" />   
    
            <script type="text/javascript">
                $(function () {

                    MensajeUnBotonNoAccionLoad();
                    MensajeUnBotonUnaAccionLoad();
                    MensajeDosBotonesUnaAccionLoad();

                });


                function AquiMuestroMensaje() {

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

            </script>
  


        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnSalirM2B2A" />
            <asp:PostBackTrigger ControlID="btnContinuarM2B2A" />
            <asp:PostBackTrigger ControlID="btnAceptarM1B1A" />
            <asp:PostBackTrigger ControlID="btnAceptarM2B1A" />
            <asp:PostBackTrigger ControlID="btnConsulta" />
            <asp:PostBackTrigger ControlID="btnPersonalizaColumnas" />
            <asp:PostBackTrigger ControlID="btnExportaExcel" />
            <asp:PostBackTrigger ControlID="btnRegresa" />
        </Triggers>

    </asp:UpdatePanel>
    
    <wuc:wucPersonalizarColumnas ID="PersonalizaColumnas" runat="server"/>

</asp:Content>
