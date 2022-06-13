<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master" CodeBehind="DocComponenteAyuda.aspx.vb" Inherits="SEPRIS.DocComponenteAyuda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Componente de Ayuda</label>
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
                            Presentar al usuario un dialogo de ayuda especifico a la pantalla que esta visualizando.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Icono de Ayuda
                        </p>
                        <p class="txt_gral Justificado">
                            En todas las pantallas del sistema aparecerá un icono de ayuda, en la parte superior derecha con transparencia, como se muestra en la imagen.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_079937cc9f30acca_html_m23d8c46f.png" name="Picture 1" width="69" height="67" border="0" alt="Icono Ayuda">
                            <br />
                            Figura 1
                        </p>
                        <p class="txt_gral Justificado">
                            Al colocar el mouse sobre la imagen no tendrá transparencia, como se muestra en la imagen.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_079937cc9f30acca_html_m729f246e.png" name="Picture 2" width="69" height="67" border="0" alt="Icono Ayuda Solido">
                            <br />
                            Figura 2
                        </p>
                        <p class="txt_gral Justificado">
                            Al dar clic en la imagen se muestra el dialogo de ayuda.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Ingreso a la Ayuda
                        </p>
                        <p class="txt_gral Justificado">
                            Cuando se ingresa al dialogo de ayuda este muestra la información correspondiente al Menú y Submenú actual, como se muestra en la imagen.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_079937cc9f30acca_html_46ed8346.png" name="Picture 3" width="567" height="350" border="0" alt="Entrada Ayuda">
                            <br />
                            Figura 3
                        </p>
                        <p class="txt_gral Justificado">
                            Si el Submenú contiene más de un elemento de ayuda se muestra una lista del Menú -&gt; Submenú -&gt; Lista de ayuda, como se muestra en la imagen.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_079937cc9f30acca_html_m558dc042.png" name="Picture 4" width="567" height="350" border="0" alt="Temas Ayuda">
                            <br />
                            Figura 4
                        </p>
                        <p class="txt_gral Justificado">
                            Al dar clic a algún elemento de la lista de ayuda muestra su contenido, como se muestra en la imagen 1.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Inicio de la Ayuda
                        </p>
                        <p class="txt_gral Justificado">
                            Al dar clic en el icono Inicio se muestra la pantalla inicial de la ayuda, como se muestra en la imagen.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_079937cc9f30acca_html_3b104268.png" name="Picture 5" width="567" height="350" border="0" alt="Inicio Ayuda">
                            <br />
                            Figura 5
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Índice de la Ayuda
                        </p>
                        <p class="txt_gral Justificado">
                            Al dar clic en el icono Índice, se muestra un árbol de los temas de ayuda disponibles para el sistema, como se muestra en la imagen.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_079937cc9f30acca_html_4dfa7d71.png" name="Picture 6" width="567" height="350" border="0" alt="Índice Ayuda">
                            <br />
                            Figura 6
                        </p>
                        <p class="txt_gral Justificado">
                            Al dar clic en un elemento de ayuda se muestra el contenido de ayuda para dicho elemento, como se muestra en la figura 1.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Búsqueda
                        </p>
                        <p class="txt_gral Justificado">
                            Para realizar una búsqueda se debe escribir la palabra o frase en la caja de texto y presionar el botón Buscar, como se muestra en la imagen.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_079937cc9f30acca_html_m8j4m340d.png" name="Picture 7" width="567" height="350" border="0" alt="Búsqueda">
                            <br />
                            Figura 7
                        </p>
                        <p class="txt_gral Justificado">
                            La búsqueda se realiza tanto en el titulo de la ayuda como en su contenido, los resultados encontrados se muestran en forma de árbol jerárquico como se muestra en la imagen.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_079937cc9f30acca_html_m8j4m555d.png" name="Picture 8" width="567" height="350" border="0" alt="Resultados">
                            <br />
                            Figura 8
                        </p>
                        <p class="txt_gral Justificado">
                            Al dar clic en un elemento de ayuda se muestra el contenido de ayuda para dicho elemento, como se muestra en la figura 1.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Hilo de Ariadna
                        </p>
                        <p class="txt_gral Justificado">
                            En las pantallas de contenido de ayuda se muestra un Hilo de Ariadna, también conocido como Breadcrumb, este indica la posición actual del tema de ayuda, como se muestra en la imagen.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_079937cc9f30acca_html_2388d7dc.png" name="Picture 9" width="567" height="350" border="0" alt="Hilo de Ariadna">
                            <br />
                            Figura 9
                        </p>
                        <p class="txt_gral Justificado">
                            En el Hilo de Ariadna se muestra el Menú, Submenú y tema de Ayuda.
                        </p>
                        
                    </div>

                    <asp:Button ID="btnRegresarUsuario" runat="server" Text="Regresar" />
                    <asp:Button ID="btnDescargaManual" runat="server" Text="Descarga Manual" Width="110px" OnClientClick="parent.location.href='../Manuales/Manual Usuario.pdf'" />
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
                            La instalación del componente de ayuda consiste en que el control personalizado  se encuentre dentro del proyecto que lo consumirá, como se muestra en la figura (Imagen 1).
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_8b8e696cd8324ee8_html_1ae8aba0.png" name="Picture 1" width="208" height="203" border="0" alt="Componente">
                            <br />
                            Imagen 1
                        </p>
                        <p class="txt_gral Justificado">
                            También es necesario que las imágenes:
                            <ul>
                                <li>Question_mark.png</li>
                                <li>Home.png</li>
                                <li>book_search.png</li>
                            </ul>
                            Existan en la carpeta “Imágenes”.
                        </p>
                        <p class="txt_gral Justificado">
                            Finalmente se debe incluir el archivo “MensajeModal.js” en la pagina, por defecto está en la MasterPage.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Implementación
                        </p>
                        <p class="txt_gral Justificado">
                            Para utilizar el Componente de ayuda es necesario que se encuentre incluido en el proyecto.
                        </p>
                        <p class="txt_gral Justificado">
                            Antes de poder crear un componente en una pantalla es necesario referenciarlo, para esto se escribe la línea que se muestra en la figura (Imagen 2) debajo del registro de la pantalla.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_8b8e696cd8324ee8_html_m36f0de9f.png" name="Picture 2" width="587" height="21" border="0" alt="Registro">
                            <br />
                            Imagen 2
                        </p>
                        <p class="txt_gral Justificado">
                            Posterior a esto se escribe la línea de la figura (Imagen 3)
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_8b8e696cd8324ee8_html_2f3460c7.png" name="Picture 3" width="309" height="16" border="0" alt="Uso">
                            <br />
                            Imagen 3
                        </p>
                        <p class="txt_gral Justificado">
                            Como primer línea dentro del ContentPlaceholder del cuerpo de la pantalla, es importante colocarlo fuera de un UpdatePanel.
                        </p>
                    </div>

                    <asp:Button ID="btnRegresarTecnico" runat="server" Text="Regresar" />

                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnManualUsuario" />
            <asp:PostBackTrigger ControlID="btnManualTecnico" />
            <asp:PostBackTrigger ControlID="btnRegresarUsuario" />
            <asp:PostBackTrigger ControlID="btnRegresarTecnico" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
