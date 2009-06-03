/*
This class will handle getting data from the db for pie charts.
Works with the chart data web service.
*/
var GChartManagerPie = new JS.Class(GChartManagerBase, {
    /* 
    Constructor.
    */
    initialize: function() {
        this._rawData = null;
    },

    /*
    Public Functions
    */
    createPieChart: function(chartSelector, dataProcedure, url, chartArgs, nameMapper, colorMapper) {
        var webServiceArgStr = '{pstrProcedureName:\'' + dataProcedure + '\', plstArgs:[0]}';
        var manager = this;

        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: url,
            data: webServiceArgStr,
            dataType: 'json',
            success: function(response) {
                manager._rawData = (typeof response.d) == 'string' ? JSON.parse(response.d) : response.d;

                $(chartSelector).empty();

                manager._drawPieChart(chartSelector,
                                      manager._buildDataSeries(chartArgs, colorMapper),
                                      manager._buildDataLabels(nameMapper),
                                      chartArgs);

                $('.pieContainer').corner({ tl: { radius: 6 },
                    tr: { radius: 6 },
                    bl: { radius: 6 },
                    br: { radius: 6 }
                });
            },
            failure: function(msg) {
                alert('Failure: ' + response.d);
            }
        });
        
        $(chartSelector).append("<img src='../../static/img/redflaggif.gif' alt='ajax-loader'></img>");
    },

    /*
    Private Helpers
    */

    _buildDataSeries: function(chartArgs, colorMapper) {
        var series;
        var data = new Array();
        var colors = new Array();

        // Fill arrays needed for series creation,
        // populate colors array by calling color mapper function,
        // populate dataLabels with names,
        // populate data with values
        $.each(this._rawData, function(name, value) {
            colors.push(colorMapper(value.Label));
            data.push(value.Data);
        });

        // Create the data series for the gChart call
        series = [$.gchart.series((chartArgs.hasOwnProperty('label') ? chartArgs['label'] : 'default'), // label (unused)
                                    data, // data values array
                                    colors)]; // colors array

        return series;
    },

    _buildDataLabels: function(nameMapper) {
        var dataLabels = new Array();

        // Fill the array which will hold the dataLabels by extracting
        // all the name values from the rawData array
        $.each(this._rawData, function(name, value) {
            dataLabels.push(nameMapper(value.Label));
        });

        return dataLabels;
    },

    _drawPieChart: function(chartSelector, series, dataLabels, chartArgs) {
        $(chartSelector).gchart({
            type: 'pie',

            series: series,
            dataLabels: dataLabels,

            height: (chartArgs.hasOwnProperty('height') ? chartArgs['height'] : 400),
            width: (chartArgs.hasOwnProperty('width') ? chartArgs['width'] : 400),

            title: (chartArgs.hasOwnProperty('title') ? chartArgs['title'] : 'default'),
            titleColor: (chartArgs.hasOwnProperty('titleColor') ? chartArgs['titleColor'] : 'red'),
            backgroundColor: (chartArgs.hasOwnProperty('backgroundColor') ? chartArgs['backgroundColor'] : 'blue')
        });
    }
});