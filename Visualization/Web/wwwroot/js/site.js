(function () {
    String.prototype.format = function () {
        a = this;
        for (k in arguments) {
            a = a.replace("{" + k + "}", arguments[k]);
        }
        return a;
    };

    $("#menu-toggle").click(function (e) {
        e.preventDefault();
        $(this).find('i').toggleClass('fa-toggle-on fa-toggle-off');
        $("#wrapper").toggleClass("toggled");
    });
}());