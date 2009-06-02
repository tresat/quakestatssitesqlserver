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
                        'MOD_TELEFRAG': 'teleporter',
                        'MOD_TRIGGER_HURT': 'the_world',
                        'MOD_WATER': 'the_world'
                    };

    var nameMappings = { 'MOD_BFG': 'BFG',
                        'MOD_BFG_SPLASH': 'BFG',
                        'MOD_CRUSH': 'The World (Crushing)',
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
                        'MOD_TELEFRAG': 'teleporter',
                        'MOD_TRIGGER_HURT': 'the_world',
                        'MOD_WATER': 'the_world'
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
                                    title: 'Test Graph',
                                    height: 400,
                                    width: 600,
                                    backgroundColor: 'aaaaaa'
                                },
                                makeNameMapper(nameMappings),
                                makeColorMapper(colorMappings, colorManager));
});