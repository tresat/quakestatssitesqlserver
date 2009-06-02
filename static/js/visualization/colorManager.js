/*
This class will hold color definitions.
*/
var ColorManager = new JS.Class({
    /*
    Exceptions
    */
    ColorNotDefinedException: function(name) {
        return new Error('Color: ' + name + ' not known!');
    },

    /*
    Constructor: Will fill pre-populate object array with
    color values for weapons.
    */
    initialize: function() {
        this._colors = new Object();
        this._colors['gauntlet'] = '00ccff';
        this._colors['machine_gun'] = 'ff0000';
        this._colors['shotgun'] = 'ff6600';
        this._colors['grenade_launcher'] = '009900';
        this._colors['rocket_launcher'] = 'ff0000';
        this._colors['lightning_gun'] = 'ffffaf';
        this._colors['railgun'] = '00ff00';
        this._colors['plasma_gun'] = 'cc00ff';
        this._colors['bfg'] = '0066ff';
        this._colors['grapling_hook'] = '00cc99';
        this._colors['chaingun'] = 'cccccc';
        this._colors['nailgun'] = '00ffcc';
        this._colors['proximity_mine_launcher'] = 'ff2299';

        this._colors['the_bomb'] = 'ffcc00';
        this._colors['teleporter'] = '00ffff';
        this._colors['the_world'] = '111111';
    },

    /*
    Public Methods
    */

    addColor: function(name, value) {
        this._colors[name] = value;
    },

    getColor: function(name) {
        if (this._colors.hasOwnProperty(name)) {
            return this._colors[name];
        } else {
            throw this.ColorNotDefinedException('Color: ' + name + ' not known!');
        }
    }
});