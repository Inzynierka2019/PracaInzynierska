"use strict";

const
    debugConn = new signalR.HubConnectionBuilder().withUrl("/hubs/debug").build(),
    simConn = new signalR.HubConnectionBuilder().withUrl("/hubs/simulation").build(),

    debug = "DebugHub",
    sim = "SimHub",
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

function configureHubConnections() {
    debugConn.on(debug, function (msg, type) {
        var template = getLogTemplate(msg, type);
        $(sysSelector).prepend(template);
    });

    debugConn.start().then(function () {
        debugConn.serverTimeoutInMilliseconds = 100000;
    }).catch(function (err) {
        return console.error(err.toString());
    });

    simConn.on(sim, function (msg, type) {
        var template = getLogTemplate(msg, type);
        $(simSelector).prepend(template);
    });

    simConn.start().then(function () {
        simConn.serverTimeoutInMilliseconds = 100000;
    }).catch(function (err) {
        return console.error(err.toString());
    });
}

(function () {
    configureHubConnections();
})();