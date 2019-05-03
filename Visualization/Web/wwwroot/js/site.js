(function () {
    String.prototype.format = function () {
        a = this;
        for (k in arguments) {
            a = a.replace("{" + k + "}", arguments[k]);
        }
        return a;
    };

    $("#menu-toggle").click(function (e) {
        console.log("asd");
        e.preventDefault();
        $(this).toggleClass('fa-toggle-on fa-chevron-right');
        $(".menu-toggle-section").toggleClass('wrapped');
        $("#wrapper").toggleClass("toggled");
    });
}());