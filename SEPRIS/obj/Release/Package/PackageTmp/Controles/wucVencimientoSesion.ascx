<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="wucVencimientoSesion.ascx.vb"
    Inherits="SEPRIS.wucVencimientoSesion" %>
<script type="text/javascript">

    /* Variables de trabajo */
    var ContadorSesion;
    var ContadorConfirmacion;


    /* Seccion para tiempo de sesion */

    /* Funcion para crear con timer */
    function ContadorDecrecienteSesion(options) {
        var timer,
            instance = this,
            seconds = options.seconds || 10,
            updateStatus = options.onUpdateStatus || function () { },
            counterEnd = options.onCounterEnd || function () { };

        function decrementCounter() {
            updateStatus(seconds);
            if (seconds === 0) {
                counterEnd();
                instance.stop();
            }


            seconds--;
        }

        this.start = function () {
            clearInterval(timer);
            timer = 0;
            seconds = options.seconds;
            timer = setInterval(decrementCounter, 1000);
        };

        this.stop = function () {
            clearInterval(timer);
        };


    }


    /* Permite reiniciar tiempo de sesion si es una llamada asincrona */
    function pageLoad(sender, args) {
        if (args.get_isPartialLoad()) {
            ReiniciaTiempo();
        }
    }

    /* invocamos creacion de mensaje desde MensajeModal.js */
    $(function () {


        MensajeVencimientoSesionLoad();


    });



    /* Funcion que permite reiniciar el tiempo de sesion */
    function ReiniciaTiempo() {

        var DivMensaje = $("#divTiempoSesion");

        if (ContadorSesion != null) {
            ContadorSesion.stop();
        }
        
        ContadorSesion = new ContadorDecrecienteSesion({
            seconds: $("#hdnTiempoSesionSegundos").val(),
            onUpdateStatus: function (sec) {

                var minutos, segundos;
                minutos = parseInt(sec / 60);
                segundos = sec % 60;
                DivMensaje.text("Su sesión finalizará en " + minutos + " Minutos y " + segundos + " Segundos");

                if (sec === (parseInt($("#hdnTiempoConfirmacion").val()) + 1)) {

                    CreaConfirmacionVencimientoSesion();

                }


            }, // callback for each second
            onCounterEnd: function () {

                $("#btnSalirVencimientoSesion").trigger("click");
            } // final action
        });

        ContadorSesion.start();

        

    }

    /* FIN Seccion para tiempo de sesion */


    /* Seccion para tiempo de confirmacion */

    /* recreamos ventana de confirmacio para poder mostrarla */
    function CreaConfirmacionVencimientoSesion() {

        var msgLabel = $("#divTextoConfirmacion");

        ContadorConfirmacion = new ContadorDecrecienteSesion({
            seconds: $("#hdnTiempoConfirmacion").val(),
            onUpdateStatus: function (sec) {

                msgLabel.text("La sesión se cerrará automáticamente en " + sec + " segundos ...")

            }, // callback for each second
            onCounterEnd: function () {

                $("#btnSalirVencimientoSesion").trigger("click");
            } // final action
        });

        ContadorConfirmacion.start();

        /* mostramos mensaje desde MensajeModal.js */
        MensajeVencimientoSesion();

    }
    /* Fin Seccion para tiempo de sesion */

</script>
<div id="divTiempoSesion" class="TemaFondoClaro" style="text-align: center; font-weight: bold;
    font-size: 12px; height: 15px" >
</div>
<asp:HiddenField ID="hdnTiempoSesion" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnTiempoSesionSegundos" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnTiempoConfirmacion" runat="server" ClientIDMode="Static" />
<div id="divConfirmacionVencimientoSesion" class="MensajeModal-UI">
    <p>
        Su sesión ha expirado por inactividad</p>
    <p>
        ¿Desea continuar con esta sesión?</p>
    <div id="divTextoConfirmacion" class="MensajeModal-UI">
    </div>
  
</div>
