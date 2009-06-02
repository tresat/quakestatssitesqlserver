/*
This class will handle getting data from the db for charts.
This is an abstract base class for a generic chart, and will be extended by 
specific child classes for each chart type (bar, pie, etc.);
*/
var GChartManagerBase = new JS.Class({
    /* 
    Constructor: abstract class cannot be instantiated, 
    so initialize throws error.
    */
    initialize: function() {
        throw new Error('Abstract GChartManagerBase class cannot be instantiated!');
    }
});