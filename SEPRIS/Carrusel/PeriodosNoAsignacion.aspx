<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="PeriodosNoAsignacion.aspx.vb" Inherits="SEPRIS.PeriodosNoAsignacion" %>

<%@ Register Assembly="CustomGridView" Namespace="CustomGridView" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>



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
<div>
        <asp:UpdatePanel ID="updPanel1" runat="server">
         <ContentTemplate>
       
            <div align="center" style="padding: 20px 20px 15px 20px">
				
                        <asp:Label ID="lblTitulo" runat="server" class="TitulosWebProyectos" Text="Periodos de No Asignación para " EnableTheming="false"></asp:Label>
                        <asp:Label ID="lblNombreUsuario" runat="server" class="TitulosWebProyectos" EnableTheming="false"></asp:Label>
                    
			</div>

             
			<div align="center" style="padding: 20px 0px 15px 0px; height: 40px;" class="FiltroDinamico" >  
                    <table border="0" cellpadding="0" cellspacing="0" 
                        style="height: 40px;" width="100%">
                        <tr valign="middle">
                            <td style="text-align: left" width="13%">
                                <asp:Label ID="Label1" runat="server" CssClass="txt_gral_blanco" 
                                    Text="PERIODO :"></asp:Label>
                            </td>
                            <td align="left" width="18%">
                                <asp:TextBox ID="TxtFechaIni" runat="server" Height="17px" Width="145px"></asp:TextBox>
                                <ajx:CalendarExtender ID="TxtFecIni_CalendarExtender" runat="server" 
                                    Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgFec3" 
                                    TargetControlID="TxtFechaIni">
                                </ajx:CalendarExtender>
                            </td>
                            <td align="left" width="5%">
                                <asp:Image ID="imgFec3" runat="server" src="../Imagenes/Calendar.gif" Style="cursor: hand" />
                            </td>
                            <td align="left" width="18%">
                                <asp:TextBox ID="TxtFechaFin" runat="server" Height="17px" 
                                    ViewStateMode="Enabled" Width="145px"></asp:TextBox>
                                <ajx:CalendarExtender ID="TxtFecFin_CalendarExtender" runat="server" 
                                    Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgFec2" 
                                    TargetControlID="TxtFechaFin">
                                </ajx:CalendarExtender>
                            </td>
                            <td align="left" width="5%">
                                <asp:Image ID="imgFec2" runat="server" src="../Imagenes/Calendar.gif" Style="cursor: hand" />
                            </td>
                            <td width="150">
                                <asp:Button ID="btnAgregar" runat="server" CssClass="botones" Height="22px"  Text="Agregar" 
                                    Width="123px" OnClientClick="Deshabilita(this);" />
                            </td>
                        </tr>
                    </table>
          </div> 
          
                 
     
          <div id="DataGridContainer" runat="server" style="height:250px">
                        
              <cc1:CustomGridView ID="gvGridAsignaciones" runat="server" width="100%" SkinID="SinSeleccion">
                   <Columns>
                         <asp:BoundField DataField="F_FECH_INI_VIG" HeaderText="FECHA INICIO PERIODO"  />
                          <asp:BoundField DataField="F_FECH_FIN_VIG" HeaderText="FECHA FIN PERIODO" />
                          <asp:TemplateField>
                              <ItemTemplate>
                                  <asp:LinkButton ID="LinkButton1" runat="server" Text="Eliminar" CommandName="deleterow"
                                      CommandArgument='<%# Container.DataItemIndex %>' />
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

           <div align="center" style="padding: 20px 20px 15px 20px">
						    
                <asp:button id="cmdRegresar" 
				runat="server" Height="22px" Width="123" CssClass="botones" Text="Regresar" >
                </asp:button>
								   
			</div>	 


            <div id="divMensajeUnBotonNoAccion" title="" style="display:none">
                <table width="100%">
                    <tr>
                        <td style="width: 50px; text-align: center">
                            <asp:Image ID="imgUnBotonUnaAccion" runat="server" Width="32px" Height="32px"/>
                        </td>
                        <td  style=" text-align: left">
                            <asp:Label ID="lblTextoMensaje" runat="server" CssClass="MensajeModal-UI" EnableTheming="false" ></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>

            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>

    

