"use strict";

const
    connection = new signalR.HubConnectionBuilder().withUrl("/consoleHub").build(),
    sysConsole = "NotifySystemConsole",
    simConsole = "NotifySimConsole",
    sysSelector = "#system-console",
    simSelector = "#sim-console",
    logType = {
        0: "debug",
        1: "info",
        2: "error",
        3: "warning"
    };

function getLogTemplate(value, type) {
    return "<div class='log-message {0}'>{1}</div>".format(logType[type], value);
}

function configureHubConnection() {
    connection.on(sysConsole, function (msg, type) {
        var template = getLogTemplate(msg, type);
        $(sysSelector).prepend(template);
    });

    connection.on(simConsole, function (msg, type) {
        var template = getLogTemplate(msg, type);
        $(simSelector).prepend(template);
    });

    connection.start().then(function () {
        connection.serverTimeoutInMilliseconds = 100000;
    }).catch(function (err) {
        return console.error(err.toString());
    });
}

(function () {
    configureHubConnection();
})();