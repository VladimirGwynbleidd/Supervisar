<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocPersonalizarColumnas.aspx.vb" Inherits="SEPRIS.DocPersonalizarColumnas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación de Personalizar Columnas</label>
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
                    <div style="text-align:justify;" class="txt_gral">
                      
                        <p class="txt_gral Justificado">
                            Mostrar una pantalla de configuración para mostrar y ocultar columnas dentro de
                            un GridView por cada usuario.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Ocultar Columnas
                        </p>
                        <p class="txt_gral Justificado">
                            Para ocultar una o más columnas de nuestro GridView es necesario realizar los 
                            siguientes pasos.
                        </p>
                        <p class="txt_gral Justificado">
                            1. Dar Clic en el botón “Personalizar columnas”, esto nos abrirá un Mensaje con 
                            las columnas de nuestro GridView.
                        </p>
                        <p class="txt_gral Justificado">
                            2. En la lista de la izquierda debemos seleccionar los elementos deseados.
                        </p>
                        <p class="txt_gral Justificado">
                            3. Dar clic en el botón “>>”</p>
                        <p class="txt_gral Justificado">
                        4. Dar clic en el botón “Guardar”, de lo contrario no se guardará la 
                            información.
                        </p>
                        <p class="txt_gral Justificado">
                            5. Fin
                        </p>
                       <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_638445a2eb4e767f_html_63de45f5.png" width="567" />
                            <br />
                              Figura 1
                         </p>
                                             
                        <br />
                        <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_638445a2eb4e767f_html_m12065dd8.png" width="567" />
                            <br />
                              Figura 2
                         </p>
                        <p class="SubTitulosWebProyectos">
                            Mostrar Columnas
                        </p>
                        <p class="txt_gral Justificado">
                            Para mostrar una o más columnas que estén ocultas en nuestro GridView es 
                            necesario realizar los siguientes pasos.
                            <p class="txt_gral Justificado">
                                1. Dar Clic en el botón “Personalizar columnas”, esto nos abrirá un Mensaje con 
                                las columnas de nuestro GridView.
                            </p>
                            <p class="txt_gral Justificado">
                                2. En la lista de la derecha debemos seleccionar los elementos deseados.</p>
                            <p class="txt_gral Justificado">
                                3. Dar clic en el botón “<<” </p>
                        <p class="txt_gral Justificado">
                            4. Dar clic en el botón “Guardar”, de lo contrario no se guardará la información.
                            </p>
                        <p class="txt_gral Justificado">
                            5. Fin 
                        </p>
                       <p align="CENTER">
                            <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_638445a2eb4e767f_html_m4f771c1b.png" width="567" />
                            <br />
                              Figura 3
                         </p>
                        <p class="txt_gral Justificado">
                            Cuando se ingrese nuevamente a la Bandeja en donde se personalizaron
                            las columnas, automáticamente aparecerán las columnas que se hayan configurado por
                            usuario.
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
                    <div style="text-align:justify;" class="txt_gral">
                        <p class="SubTitulosWebProyectos">
                            Instalación
                        </p>
                        <p class="txt_gral Justificado">
                            La instalación del componente Personalizar Columnas consiste en que el control personalizado se encuentre dentro del proyecto que lo consumirá, como se muestra en la figura (Imagen 1).
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_61de4a98a1b586aa_html_m71c614d.png" name="Picture 1" width="255" height="237" border="0" alt="Instalación">
                            <br />
                            Imagen 1
                        </p>
                        <p class="txt_gral Justificado">
                            Finalmente se debe incluir el archivo “MensajeModal.js” en la pagina, por defecto está en la MasterPage.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Implementación
                        </p>
                        <p class="txt_gral Justificado">
                            Para utilizar el componente de Personalizar Columnas es necesario que se encuentre incluido en el proyecto.
                        </p>
                        <p class="txt_gral Justificado">
                            Antes de poder utilizarlo en una pantalla es necesario referenciarlo, para esto se escribe la línea que se muestra en la figura (Imagen 2) debajo del registro de la pantalla.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_61de4a98a1b586aa_html_143c1ceb.png" name="Picture 2" width="589" height="25" border="0" alt="Registro">
                            <br />
                            Imagen 2
                        </p>
                        <p class="txt_gral Justificado">
                            Posterior a esto se debe crear un botón para llamar al componente como se muestra en la figura (Imagen 3)
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_61de4a98a1b586aa_html_4e7b4a0c.png" name="Picture 3" width="589" height="19" border="0" alt="Boton">
                            <br />
                            Imagen 3
                        </p>
                        <p class="txt_gral Justificado">
                            Se debe crear un control personalizar columnas, con una línea como la que se muestra en la figura (Imagen 4)
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_61de4a98a1b586aa_html_1c1d68f1.png" name="Picture 4" width="509" height="20" border="0" alt="Control">
                            <br />
                            Imagen 4
                        </p>
                        <p class="txt_gral Justificado">
                            El control puede estar en cualquier lugar de la página pero debe estar fuera de un UpdatePanel
                        </p>
                        <p class="txt_gral Justificado">
                            Con esto se ha creado el control, lo siguiente es configurarlo, para esto se debe realizar la carga inicial del control.
                        </p>
                        <p class="txt_gral Justificado">
                            El primer bloque de instrucciones que se utiliza es como el que se muestra en la figura (Imagen 5), este se debe aplicar en el load de la pagina la primera vez que se carga
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_61de4a98a1b586aa_html_39c6b8a1.png" name="Picture 5" width="589" height="74" border="0" alt="Load">
                            <br />
                            Imagen 5
                        </p>
                        <p class="txt_gral Justificado">
                            Ahora se explicaran las propiedades y métodos empleados:
                            <ul>
                                <li>IdentificadorGridView: Identificador del GridView que se encuentra en la tabla BDS_C_GR_GRIDVIEW, en el campo N_ID_GRIDVIEW</li>
                                <li>GridViewPersonalizar: Gridview a personalizar</li>
                                <li>Usuario: Usuario actualmente firmado, se requiere para almacenar sus preferencias en la base de datos</li>
                                <li>Personalizar: Método que lee de la base de datos las preferencias del usuario y las aplica al GridView</li>
                            </ul>
                            <p>
                            </p>
                            <p class="txt_gral Justificado">
                                El siguiente bloque de instrucciones corresponde al manejador del evento clic 
                                del botón para personalizar columnas, su implementación es como se muestra en la 
                                figura (Imagen 6)
                            </p>
                            <p align="center">
                                <img alt="Clic" border="0" height="63" name="Picture 6" 
                                    src="Imagenes_Manual/o_61de4a98a1b586aa_html_m72cacc37.png" width="589">
                                <br />
                                Imagen 6 </img></p>
                            <p class="txt_gral Justificado">
                                Se utilizan las mismas propiedades, lo que cambia es el método “Mostrar” que se 
                                utiliza para mostrar el dialogo Personalizar Columnas.
                            </p>
                            <p class="txt_gral Justificado">
                                El último bloque corresponde al manejador del evento FinPersonalizacion del 
                                control personalizar columnas, este se activa al aceptar los cambios en el 
                                dialogo, su implementación es como se muestra en la figura (Imagen 7)
                            </p>
                            <p align="center">
                                <img alt="Clic" border="0" height="82" name="Picture 7" 
                                    src="Imagenes_Manual/o_61de4a98a1b586aa_html_4929e6f5.png" width="589">
                                <br />
                                Imagen 7 </img></p>
                            <p class="txt_gral Justificado">
                                Como se puede observar en la Imagen 7 se utiliza el método 
                                “GuardaPersonalizacion” para almacenar las preferencias del usuario y aplicarlas 
                                al Gridview.
                            </p>
                            <p class="txt_gral Justificado">
                                Adicional al proceso descrito anteriormente, es necesario agregar el GridView y 
                                sus columnas personalizables a la base de datos.
                            </p>
                            <p class="txt_gral Justificado">
                                La primera tabla a llenar es BDS_C_GR_GRIDVIEW, sus campos son:
                                <ul>
                                    <li>N_ID_GRIDVIEW: Id del GridView, es el que se le envía al control</li>
                                    <li>T_DSC_GRIDIVIEW: Descripción del GridView</li>
                                    <li>T_DSC_PAGINA: Descripción de la pagina en la que está el GridView</li>
                                    <li>T_DSC_VISTA: Vista de la pagina</li>
                                </ul>
                                <p>
                                </p>
                                <p class="txt_gral Justificado">
                                    La segunda tabla es de las columnas que se pueden ocultar y mostrar, el nombre 
                                    de la tabla es BDS_R_GR_GRIDVIEW_COLUMNAS, sus campos son:
                                    <ul>
                                        <li>N_ID_GRIDVIEW: Id de GridView, debe ser el de la tabla BDS_C_GR_GRIDVIEW</li>
                                        <li>N_ID_COLUMNA: Id de la columna</li>
                                        <li>T_DSC_COLUMNA: Descripción de la columna, debe ser igual al que tiene en el 
                                            Header del GridView</li>
                                        <li>T_DSC_COLUMNA_TEXTO: Texto de la columna, debe ser igual al que tiene en el 
                                            Header del GridView</li>
                                    </ul>
                                    <p>
                                    </p>
                                    <p>
                                    </p>
                                    <p>
                                    </p>
                                    <p>
                                    </p>
                                </p>
                            </p>
                        </p>
                    </div>

                    <asp:Button ID="btnRegresarTecnico" runat="server" Text="Regresar" />
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
