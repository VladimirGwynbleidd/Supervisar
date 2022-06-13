using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConsumoServicios.ServiceReference1;

namespace ConsumoServicios
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IwcfRegistroClient objCliente = new IwcfRegistroClient();
            String[] lstSupervisores = new String[1];
            lstSupervisores[0] = "jgarcia";

            String[] lstInspector = new String[1];
            lstInspector[0] = "jgarcia";

            Int32[] lstObjetoVis = new Int32[2];
            lstObjetoVis[0] = 29;
            lstObjetoVis[1] = 1;

            //string[] liRes = objCliente.InsertaVisita(177, "jgarcia", DateTime.Now, 1, "INBURSA", 2, lstObjetoVis, "Otro objeto de la visita MC", "jgarcia", lstSupervisores, "descripcion", "comentarios", "Orden de la visita", true, DateTime.Now, 177);

            //Int32 idVisita = Convert.ToInt32(liRes[1]);
            //string[] liRes = objCliente.ActualizaFecha(171, "jgarcia", "Fecha fin insitu", Convert.ToDateTime("07/07/2017"), ServiceReference1.ConstantesTipoFecha.FechaCampoFinal, false);
            
            //objCliente.EditaVisita(558, "jgarcia", DateTime.Now, 1, lstObjetoVis, "jgarcia", lstInspector, "descripción de la visita prueba", "comentarios de la visita prueba", "orden prueba ");

            //objCliente.ActualizaFecha(145, "jgarcia", "Se actualiza fecha de vulnerabilidades", DateTime.Now.AddDays(5), ConstantesTipoFecha.FechaReunionPresi, false);

            //objCliente.ActualizaFechaVulnera(134, DateTime.Now);

            //String[] liRes = objCliente.SolicitaProrroga(558, "jgarcia", "Motivo de la prorroga por que se acabaron los dias.");

            //objCliente.AvanzaPasoSiete(191, "jgarcia", "comentarios avanza paso 7 true - false", true, false); //Presentacion Paso 8
            //objCliente.AvanzaPasoSiete(558, "jgarcia", "comentarios avanza paso 7 false - true", false, true); //sancion Paso 9
            //objCliente.AvanzaPasoSiete(558, "jgarcia", "comentarios avanza paso 7 false - false", false, false); //sancion Paso 16

            //objCliente.AvanzaPasoOcho(151, "jgarcia", "comentarios avanza paso 8 false", false, true); //sancion Paso 8 e 36 bandera en 0
            //objCliente.AvanzaPasoOcho(66, "jgarcia", "comentarios avanza paso 8 true", true); //sancion Paso 8 e 36 bandera en 1

            objCliente.AvanzaPaso(156, "segomez", "avanzo paso", 0, 0, 0, "");
            //objCliente.RechazaPaso(558, "jgarcia", "rechazo paso");
            //objCliente.CancelarVisita(559, "jgarcia", "avanzo paso");
        }
    }
}