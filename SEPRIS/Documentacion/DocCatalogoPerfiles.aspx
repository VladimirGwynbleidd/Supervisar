<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/SiteInterno.Master"
    CodeBehind="DocCatalogoPerfiles.aspx.vb" Inherits="SEPRIS.docCatalogoPerfiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upnlConsulta" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <label class="TitulosWebProyectos">
                        Documentación Catálogo de Perfiles</label>
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
                            Generar un Catálogo con los Perfiles del sistema.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Bandeja de Catálogo de Perfiles
                        </p>
                        <p class="txt_gral Justificado">
                            Para poder ver la Bandeja de Catálogo de Perfiles es necesario:
                        </p>
                        <p class="txt_gral Justificado">
                            1.	Ir al Menú de Seguridad
                        </p>
                        <p class="txt_gral Justificado">
                            2.	Una vez en el menú de Seguridad hacer clic en el submenú de Catálogo de Perfil, como se muestra en la figura siguiente
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_2dcb2543.png" name="Picture 1" width="566" height="302" border="0" alt="Catálogo Perfil">
                            <br />
                            Figura 1
                        </p>
                        <p class="txt_gral Justificado">
                            3.	La bandeja es como se muestra en la figura 2.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_343f5dba.png" name="Picture 2" width="567" height="345" border="0" alt="Bandeja Catálogo Perfil">
                            <br />
                            Figura 2
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Alta de Perfil
                        </p>
                        <p class="txt_gral Justificado">
                            Para dar de alta un nuevo Perfil es necesario seguir los siguientes pasos:
                        </p>
                        <p class="txt_gral Justificado">
                            1.	Dar Clic en el botón “Agregar”, esto nos enviará a la pantalla de Alta de Perfil, como se muestra en la figura siguiente.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_53226337.png" name="Picture 3" width="567" height="235" border="0" alt="Alta Perfil">
                            <br />
                            Figura 3
                        </p>
                        <p class="txt_gral Justificado">
                            2.	Llenar los campos obligatorios marcados con (*).
                        </p>
                        <p class="txt_gral Justificado">
                            3.	Dar Clic en el botón de “Aceptar”, esto nos desplegará un Mensaje de confirmación como se muestra en la figura 4.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_1959387.png" name="Picture 4" width="567" height="235" border="0" alt="Alta Perfil">
                            <br />
                            Figura 4
                        </p>
                        <p class="txt_gral Justificado">
                            4.	Dar clic en el botón de “Aceptar” del Mensaje de Confirmación, esto guardará la información y direccionará a la Bandeja de Catálogo de Perfil.
                        </p>
                        <p class="txt_gral Justificado">
                            5.	Si algún campo se encuentra vacio, la aplicación validará y enviará el siguiente mensaje, figura siguiente, con los campos que faltan de capturar.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_1ff77d05.png" name="Picture 5" width="567" height="235" border="0" alt="Alta Perfil">
                            <br />
                            Figura 5
                        </p>
                        <p class="txt_gral Justificado">
                            6.	Fin.
                        </p>
                        <p class="txt_gral Justificado">
                            El botón de cancelar nos mostrará la siguiente advertencia.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_m567c32cb.png" name="Picture 5" width="567" height="235" border="0" alt="Alta Perfil">
                            <br />
                            Figura 5
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Modificación de Perfil
                        </p>
                        <p class="txt_gral Justificado">
                            Para modificar algún Perfil, es necesario:
                        </p>
                        <p class="txt_gral Justificado">
                            1.	Seleccionar un registro de la Bandeja, en caso que no seleccionemos un registro nos mostrará un mensaje de Error como se muestra en la figura siguiente.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_m2a0b2678.png" name="Picture 6" width="567" height="343" border="0" alt="Seleccione Perfil">
                            <br />
                            Figura 6
                        </p>
                        <p class="txt_gral Justificado">
                            2.	Dar clic en el botón de “Modificar”, lo cual nos enviará a la pantalla de “Modificación de Perfil” como se  muestra en la figura 7.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_6d87d94b.png" name="Picture 7" width="567" height="236" border="0" alt="Modificaión Perfil">
                            <br />
                            Figura 7
                        </p>
                        <p class="txt_gral Justificado">
                            3.	Modificamos los campos deseados.
                        </p>
                        <p class="txt_gral Justificado">
                            4.	Dar clic en el botón de “Aceptar” ”, esto nos desplegará un mensaje de confirmación como se muestra en la figura 8.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_m6eed943b.png" name="Picture 8" width="566" height="235" border="0" alt="Confirmacion">
                            <br />
                            Figura 8
                        </p>
                        <p class="txt_gral Justificado">
                            5.	Dar clic en el botón de “Aceptar” del Mensaje de Confirmación, esto guardará la información y direccionará a la Bandeja de Catálogo de Perfiles.
                        </p>
                        <p class="txt_gral Justificado">
                            6.	Si algún campo se encuentra vacio, la aplicación validará y enviará el siguiente mensaje figura 73, con los campos que faltan de capturar.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_m61eb50cf.png" name="Picture 9" width="567" height="236" border="0" alt="Llene obligatorios">
                            <br />
                            Figura 9
                        </p>
                        <p class="txt_gral Justificado">
                            7.	Fin.
                        </p>
                        <p class="txt_gral Justificado">
                            El botón cancelar nos mostrará una advertencia antes de direccionarnos a la Bandeja de Catálogo de Perfil, como se muestra en la figura 10.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_e863a4b.png" name="Picture 10" width="567" height="236" border="0" alt="Llene obligatorios">
                            <br />
                            Figura 10
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Eliminación de Perfil
                        </p>
                        <p class="txt_gral Justificado">
                            Para eliminar algún Perfil debemos seguir los siguientes pasos:
                        </p>
                        <p class="txt_gral Justificado">
                            1.	Seleccionar un registro de la Bandeja, en caso que no seleccionemos un registro nos mostrará un mensaje de Error como se muestra en la figura 
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_2a9319a5.png" name="Picture 11" width="567" height="345" border="0" alt="Seleccione Perfil">
                            <br />
                            Figura 11
                        </p>
                        <p class="txt_gral Justificado">
                            2.	Dar clic en el botón de “Eliminar”, esto nos mostrará un mensaje de confirmación como se muestra en la figura siguiente.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_m56d8e3c5.png" name="Picture 12" width="567" height="345" border="0" alt="Confirme eliminación">
                            <br />
                            Figura 12
                        </p>
                        <p class="txt_gral Justificado">
                            3.	Dar clic en el botón de “Aceptar” del Mensaje de Confirmación, esto eliminará el registro seleccionado y mostrará la Bandeja de Catálogo de Perfiles.
                        </p>
                        <p class="txt_gral Justificado">
                            4.	Fin.
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Consulta de Perfil
                        </p>
                        <p class="txt_gral Justificado">
                            Para consultar un Perfil en específico debemos seguir los pasos:
                        </p>
                        <p class="txt_gral Justificado">
                            1.	Dar doble clic sobre el registro deseado, esto nos direccionará a la pantalla de “Consulta de Perfiles” como se muestra en la siguiente figura.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_m41a56f9.png" name="Picture 13" width="567" height="217" border="0" alt="Consulta Perfil">
                            <br />
                            Figura 13
                        </p>
                        <p class="txt_gral Justificado">
                            2.	Una vez en la pantalla de consulta damos clic en el botón de “Regresar” y nos direccionará a la Bandeja de Catálogo de Perfiles”
                        </p>
                        <p class="SubTitulosWebProyectos">
                            Agregar Filtro
                        </p>
                        <p class="txt_gral Justificado">
                            Para agregar un filtro en la Bandeja de Catálogo de Perfiles, es necesario seleccionar el campo deseado de la lista que se muestra en la figura siguiente.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_330f852.png" name="Picture 14" width="566" height="347" border="0" alt="Consulta Perfil">
                            <br />
                            Figura 14
                        </p>
                        <p class="txt_gral Justificado">
                            Por default aparecerá el filtro de vigencia en forma de lista, donde podemos seleccionar, aquellos Perfiles vigentes, no vigentes o ambos.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_1e8330c9.png" name="Picture 15" width="566" height="65" border="0" alt="Filtros">
                            <br />
                            Figura 15
                        </p>
                        <p class="txt_gral Justificado">
                            Una vez seleccionado un campo nos aparece el filtro en la parte superior, dándonos la oportunidad de poder eliminar dicho filtro.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_4fc5710d.png" name="Picture 16" width="567" height="82" border="0" alt="Filtros">
                            <br />
                            Figura 16
                        </p>
                        <p class="txt_gral Justificado">
                            Cuando damos clic en el botón “Filtrar”  el GridView se actualiza y nos muestra el resultado de la consulta.
                        </p>
                        <p align="center">
                            <img src="Imagenes_Manual/o_727cabd9fc217b30_html_23fb029.png" name="Picture 17" width="567" height="280" border="0" alt="Filtros">
                            <br />
                            Figura 17
                        </p>
                    </div>
                    <asp:Button ID="btnRegresarUusario" runat="server" Text="Regresar" />
                    <asp:Button ID="btnDescargaManual" runat="server" Text="Descarga Manual" Width="110px" OnClientClick="parent.location.href='../Manuales/Manual Usuario.pdf'" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlTecnico" runat="server" Visible="false">
                <div align="center" style="padding: 20px 20px 15px 20px">
                    <asp:Label ID="lblTituloManualTecnico" runat="server" CssClass="TitulosWebProyectos"
                        Text=""></asp:Label>
                </div>
                <asp:Panel ID="pnlPrincipalTecnico" runat="server">
                    <div style="text-align: left;" class="txt_gral">
                        <p class="SubTitulosWebProyectos">
                            Requerimientos:</p>
                        <ul>
                            <li>Implementar patrón funcional de GridView.</li>
                            <li>Implementa patrón funcional de Mensaje modal.</li>
                            <li>Filtros dinámicos.</li>
                            <li>Implementar Bitácora</li>
                            <li>Implementar catálogo de errores.</li>
                            <li>Script de base de datos.- el patrón funcional requiere de la tabla BDS_C_GR_PERFIL.
                                Esta tabla se puede obtener del script de creación de base de datos de la ODT-02.</li>
                            <li>Código fuente.- Este patrón funcional está compuesto por una página CatalogoPerfiles.aspx
                                que se encuentra en la carpeta Mantenimiento del proyecto WEBSITE, que hereda de
                                la clase Perfil.vb que se encuentra en el proyecto Entities.</li>
                        </ul>
                        <br />
                        <p align="center">
                            <img align="bottom" border="0" name="Imagen 2" src="Imagenes_Manual/o_a34fdwcdea32da19bed_html_5d463s618.png"
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
