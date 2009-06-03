$(function() {
    var colorManager = new ColorManager();
    var gChartManager = new GChartManagerPie();

    var colorMappings = {'MOD_BFG': 'bfg', 
                        'MOD_BFG_SPLASH': 'bfg',
                        'MOD_CRUSH': 'the_world',
                        'MOD_FALLING': 'the_world',
                        'MOD_GAUNTLET': 'gauntlet',
                        'MOD_GRENADE': 'grenade_launcher',
                        'MOD_GRENADE_SPLASH': 'grenade_launcher',
                        'MOD_JUICED': 'the_world',
                        'MOD_KAMIKAZE': 'the_bomb',
                        'MOD_LAVA': 'the_world',
                        'MOD_LIGHTNING': 'lightning_gun', 
                        'MOD_MACHINEGUN': 'machine_gun',
                        'MOD_NAIL': 'nailgun', 
                        'MOD_PLASMA': 'plasma_gun',
                        'MOD_PLASMA_SPLASH': 'plasma_gun',
                        'MOD_PROXIMITY_MINE': 'proximity_mine_launcher', 
                        'MOD_RAILGUN': 'railgun', 
                        'MOD_ROCKET': 'rocket_launcher',
                        'MOD_ROCKET_SPLASH': 'rocket_launcher', 
                        'MOD_SHOTGUN': 'shotgun',
                        'MOD_SUICIDE': 'the_bomb',
                        'MOD_TELEFRAG': 'the_world',
                        'MOD_TRIGGER_HURT': 'the_world',
                        'MOD_WATER': 'the_world'
                    };

    var nameMappings = {'MOD_BFG': 'BFG',
                        'MOD_BFG_SPLASH': 'BFG (Splash Damage)',
                        'MOD_CRUSH': 'The World (Crushing)',
                        'MOD_FALLING': 'The World (Falling)',
                        'MOD_GAUNTLET': 'Gauntlet',
                        'MOD_GRENADE': 'Grenades',
                        'MOD_GRENADE_SPLASH': 'Grenades (Splash Damage)',
                        'MOD_JUICED': 'The World (Juiced)',
                        'MOD_KAMIKAZE': 'The Bomb',
                        'MOD_LAVA': 'The World (Lava)',
                        'MOD_LIGHTNING': 'Lightning Gun',
                        'MOD_MACHINEGUN': 'Machine Gun/Chaingun',
                        'MOD_NAIL': 'Nailgun',
                        'MOD_PLASMA': 'Plasma Gun',
                        'MOD_PLASMA_SPLASH': 'Plasma Gun (Splash Damage)',
                        'MOD_PROXIMITY_MINE': 'Proximity Mines',
                        'MOD_RAILGUN': 'Railgun',
                        'MOD_ROCKET': 'Rockets',
                        'MOD_ROCKET_SPLASH': 'Rockets (Splash Damage)',
                        'MOD_SHOTGUN': 'Shotgun',
                        'MOD_SUICIDE': 'The Bomb (Self-Kill)',
                        'MOD_TELEFRAG': 'The World (Tele-Frag)',
                        'MOD_TRIGGER_HURT': 'The World (Other)',
                        'MOD_WATER': 'The World (Drowned)'
                    };
                         
    var makeColorMapper = function(mappings, manager) {       
        return function(value) {
            if (mappings.hasOwnProperty(value)) {
                return manager.getColor(mappings[value]);
            } else {
                throw new Error('Unexpected value in colorMapper(): ' + value);
            }
        };
    };

    var makeNameMapper = function(mappings) {
        return function(value) {
            if (mappings.hasOwnProperty(value)) {
                return mappings[value];
            } else {
                throw new Error('Unexpected value in nameMapper(): ' + value);
            }
        };
    };
                                
    gChartManager.createPieChart('#killsByWeaponPie',
                                'DataProcedures.spGetKillsByWeapon',
                                '../../WS/DataProcedureService.asmx/CallChartProcedure',
                                {
                                    title: 'Kills by Weapon',
                                    titleColor: 'blue',
                                    
                                    height: 410,    // TODO: jquery the dimensions of the parent div here?
                                    width: 700,
                                    backgroundColor: 'aaaaaa'
                                },
                                makeNameMapper(nameMappings),
                                makeColorMapper(colorMappings, colorManager));
});