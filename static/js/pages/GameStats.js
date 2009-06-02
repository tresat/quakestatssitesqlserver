/// <reference name="MicrosoftAjax.js"/>

var mintGameID;

$(function() {
    // Grab the game id from the hidden field
    mintGameID = $('.gameID').val();

    // Set up the jquery grid
    $("#gridGame").jqGrid({ url: '../CallDataProcedure.aspx',
        datatype: 'xml',
        mtype: 'POST',
        colNames: ['Client ID', 'Client Name', 'Team Log ID', 'Team', 'Score', 'Kills', 'Deaths', 'Team Kills', 'Self Kills', 'Carrier Kills', 'Recoveries', 'Steals', 'Pickups', 'Captures'],
        colModel: [{ name: 'ClientID', index: 1, width: 50, hidden: true },
                    { name: 'ClientName', index: 2, width: 155 },
                    { name: 'TeamLogID', index: 3, width: 50, hidden: true },
                    { name: 'Team', index: 4, width: 55 },
                    { name: 'Score', index: 5, width: 57, align: 'right' },
                    { name: 'Kills', index: 6, width: 50, align: 'right' },
                    { name: 'Deaths', index: 7, width: 63, align: 'right' },
                    { name: 'TeamKills', index: 8, width: 90, align: 'right' },
                    { name: 'SelfKills', index: 9, width: 75, align: 'right' },
                    { name: 'CarrierKills', index: 10, width: 95, align: 'right' },
                    { name: 'FlagRecoveries', index: 11, width: 90, align: 'right' },
                    { name: 'FlagSteals', index: 12, width: 62, align: 'right' },
                    { name: 'FlagPickups', index: 13, width: 70, align: 'right' },
                    { name: 'FlagCaptures', index: 14, width: 77, align: 'right'}],
        pager: $('#pagerGame'),
        rowNum: 10,
        rowList: [5, 10, 25],
        sortname: 14,
        sortorder: 'desc',
        viewrecords: true,
        imgpath: '../../static/gridThemes/redmond/images',
        caption: 'Player Statistics',
        height: 'auto',
        loadonce: true,
        postData: { idrow: 0, // unique row used as id
                    proc: 'DataProcedures.spGetGameResults', // stored procedure to call (must be in approved list for user)
                    arg0: mintGameID } // all other arguments starting with 'arg' are params to proc
    });
});