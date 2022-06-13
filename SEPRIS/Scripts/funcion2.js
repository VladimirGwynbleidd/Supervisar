$(function () {
    $('#container').highcharts({
        data: {
            table: 'datatable'
        },
        
        legend:{
        	align: 'right',
        	horizontalalign:'left', x:-50, y:-5
        },
        
        chart: {
            type: 'column',
            margin: 70,
            options3d: {
                enabled: true,
                alpha: 10,
                beta: 15,
                depth: 70
            }
        },

        title: {
           text: 'Total de Visitas por \u00C1rea y Entidad'
        },
        
        yAxis: {
            allowDecimals: false,
            title: {
               text: 'N\u00FAmero de Visitas'
            }
        },
        
        tooltip: {
            formatter: function () {
                return '<b>' + this.series.name + '</b><br/>' + this.point.y;
            }
        }
    });
});