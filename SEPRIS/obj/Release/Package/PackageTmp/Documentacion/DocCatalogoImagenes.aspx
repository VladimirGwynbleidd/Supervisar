<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocCatalogoImagenes.aspx.vb" Inherits="SEPRIS.DocCatalogoImagenes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación de Catálogo de Imágenes</label>
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
                    <div style="text-align: justify;" class="txt_gral">
                        <p class="txt_gral Justificado">
                            Generar un Catálogo con las imágenes que usarán cada uno de los componentes de nuestro
                            sistema.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Bandeja de Catálogo de Imágenes
                        </p>
                        <p class="txt_gral Justificado">
                            Para poder ver la Bandeja de Catálogo de Imágenes es necesario:
                        </p>
                        <p class="txt_gral Justificado">
                            1. Ir al Menú de Mantenimiento
                        </p>
                        <p class="txt_gral Justificado">
                            2. Una vez en el menú de Mantenimiento hacer clic en el submenú de Catálogo de Imagen
                            como se muestra en la figura siguiente.
                        </p>
                        <p class="txt_gral Justificado">
                            3. La bandeja es como se muestra en la figura 2.
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_4ae740220b707df0_html_m55a5c7e1.png"
                                    width="567" />
                                <br />
                                Figura 1
                            </p>
                            <br />
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_4ae740220b707df0_html_m71084356.png"
                                    width="567" />
                                <br />
                                Figura 2
                            </p>
                            <br />
                            <p>
                            </p>
                            <p class="SubTitulosWebProyectos">
                                Alta de Imágenes
                            </p>
                            <p class="txt_gral Justificado">
                                Para dar de alta una nueva imagen es necesario seguir los siguientes pasos:</p>
                            <p class="txt_gral Justificado">
                                1. Dar Clic en el botón “Agregar”, esto nos enviará a la pantalla de Alta de Imagen,
                                como se muestra en la figura siguiente.
                            </p>
                            <p class="txt_gral Justificado">
                                2. Llenar los campos obligatorios marcados con (*).
                            </p>
                            <p class="txt_gral Justificado">
                                3. Dar Clic en el botón de “Aceptar”, esto nos desplegará un Mensaje de confirmación.
                            </p>
                            <p class="txt_gral Justificado">
                                4. Dar clic en el botón de “Aceptar” del Mensaje de Confirmación, esto guardará
                                la información y direccionará a la Bandeja de Catálogo de Imágenes.
                            </p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_4ae740220b707df0_html_422f2b93.png"
                                    width="567" />
                                <br />
                                Figura 3
                            </p>
                            <br />
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_4ae740220b707df0_html_35db0a48.png"
                                    width="567" />
                                <br />
                                Figura 4
                            </p>
                            <br />
                            <p class="txt_gral Justificado">
                                5. Si algún campo se encuentra vacio, la aplicación validará y enviará el siguiente
                                mensaje, figura siguiente, con los campos que faltan de capturar.</p>
                            <p class="txt_gral Justificado">
                                6. Fin.</p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_4ae740220b707df0_html_b09f2f6.png"
                                    width="567" />
                                <br />
                                Figura 5
                            </p>
                            <br />
                            <p class="txt_gral Justificado">
                                El botón de cancelar nos mostrará la siguiente advertencia si no queremos guardar
                                la información figura siguiente.
                            </p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_4ae740220b707df0_html_29818972.png"
                                    width="567" />
                                <br />
                                Figura 6
                            </p>
                            <br />
                            <p class="SubTitulosWebProyectos">
                                Modificación de Imagen
                            </p>
                            <p class="txt_gral Justificado">
                                Para modificar alguna Imagen, es necesario:</p>
                            <p class="txt_gral Justificado">
                                1. Seleccionar un registro de la Bandeja, en caso que no seleccionemos un registro
                                nos mostrará un mensaje de Error como se muestra en la figura siguiente.
                            </p>
                            <p class="txt_gral Justificado">
                                2. Dar clic en el botón de “Modificar”, lo cual nos enviará a la pantalla de “Modificar
                                Imagen”.
                            </p>
                            <p class="txt_gral Justificado">
                                3. Modificamos los campos deseados.
                            </p>
                            <p class="txt_gral Justificado">
                                4. Dar clic en el botón de “Aceptar” ”, esto nos desplegará un Mensaje de confirmación.</p>
                            <p class="txt_gral Justificado">
                                5. Dar clic en el botón de “Aceptar” del Mensaje de Confirmación, esto guardará
                                la información y direccionará a la Bandeja de Catálogo de Imágenes.</p>
                            <p class="txt_gral Justificado">
                                6. Si algún campo se encuentra vacio, la aplicación validará y enviará un mensaje
                                con los campos que faltan de capturar.
                            </p>
                            <p class="txt_gral Justificado">
                                7. Fin.
                            </p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_4ae740220b707df0_html_2d10620f.png"
                                    width="567" />
                                <br />
                                Figura 7
                            </p>
                            <br />
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_4ae740220b707df0_html_m5ad29d2f.png"
                                    width="567" />
                                <br />
                                Figura 8
                            </p>
                            <br />
                            <p class="txt_gral Justificado">
                                El botón cancelar nos mostrará una advertencia antes de direccionarnos a la Bandeja
                                de Catálogo de Imágenes.
                            </p>
                            <p class="SubTitulosWebProyectos">
                                Eliminación de Imagen
                            </p>
                            <p class="txt_gral Justificado">
                                Para eliminar alguna imagen debemos seguir los siguientes pasos:
                            </p>
                            <p class="txt_gral Justificado">
                                1. Seleccionar un registro de la Bandeja, en caso que no seleccionemos un registro
                                nos mostrará un mensaje de error.
                            </p>
                            <p class="txt_gral Justificado">
                                2. Dar clic en el botón de “Eliminar”, esto nos mostrará un mensaje de confirmación.
                            </p>
                            <p class="txt_gral Justificado">
                                3. Dar clic en el botón de “Aceptar” del Mensaje de Confirmación, esto eliminará
                                el registro seleccionado y mostrará la Bandeja de Catálogo de Imágenes.
                            </p>
                            <p class="txt_gral Justificado">
                                4. Fin.
                            </p>
                            <br />
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_4ae740220b707df0_html_51cda839.png"
                                    width="567" />
                                <br />
                                Figura 9
                            </p>
                            <br />
                            <p class="SubTitulosWebProyectos">
                                Consulta de Imagen
                            </p>
                            <p class="txt_gral Justificado">
                                Para consultar una imagen en específico debemos seguir los pasos:
                            </p>
                            <p class="txt_gral Justificado">
                                3. Dar doble clic sobre el registro deseado, esto nos direccionará a la pantalla
                                de “Consulta de Imagen” como se muestra en la siguiente figura.
                            </p>
                            <p class="txt_gral Justificado">
                                4. Una vez en la pantalla de consulta damos clic en el botón de “Regresar” y nos
                                direccionará a la Bandeja de Catálogo de Imágenes”
                            </p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_4ae740220b707df0_html_23ce2a29.png"
                                    width="567" />
                                <br />
                                Figura 10
                            </p>
                            <br />
                            <p class="SubTitulosWebProyectos">
                                Agregar Filtro
                            </p>
                            <p class="txt_gral Justificado">
                                Para agregar un filtro en la Bandeja de Catálogo de Imágenes, es necesario seleccionar
                                el campo deseado de la lista que se muestra en la figura siguiente. Por default
                                aparecerá el filtro de vigencia en forma de Lista, donde podemos seleccionar, aquellas
                                imágenes vigentes, no vigentes o ambos.
                            </p>
                            <p align="CENTER">
                                <img align="BOTTOM" border="0" height="241" name="Imagen 2" src="Imagenes_Manual/o_4ae740220b707df0_html_a5c5b34.png"
                                    width="567" />
                                <br />
                                Figura 11
                            </p>
                            <br />
                            <p class="txt_gral Justificado">
                                Una vez seleccionado un campo, nos aparece el filtro en la parte superior, dándonos
                                la oportunidad de poder eliminar dicho filtro. Cuando damos clic en el botón “Filtrar”
                                el GridView se actualiza y nos muestra el resultado de la consulta.
                            </p>
                            <p>
                            </p>
                            <p>
                            </p>
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
                    <div style="text-align: justify;" class="txt_gral">
                        <p class="SubTitulosWebProyectos">
                            Requerimientos:</p>
                        <ul>
                            <li>Implementar patrón funcional de GridView.</li>
                            <li>Implementa patrón funcional de Mensaje modal.</li>
                            <li>Filtros dinámicos.</li>
                            <li>Implementar Bitácora</li>
                            <li>Script de base de datos.- el patrón funcional requiere de la tabla BDS_C_GR_IMAGEN.
                                Esta tabla se puede obtener del script de creación de base de datos de la ODT-02.</li>
                            <li>Código fuente.- Este patrón funcional está compuesto por una página CatalogoImagen.aspx
                                que se encuentra en la carpeta Mantenimiento del proyecto WEBSITE, que hereda de
                                la clase Imagen.vb que se encuentra en el proyecto Entities.</li>
                        </ul>
                        <br />
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_aabf09as239bed_html_5d463618.png"
                                alt="" />
                            <br />
                            Figura 1
                        </p>
                        <br />
                        <br />
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
