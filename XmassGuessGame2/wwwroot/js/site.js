// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#personPick").hide();

$("#showHideButton").click(function () {
    var pickLabel = $("#personPick");
    var showHideButton = $("#showHideButton");
    if (pickLabel.is(":visible")) {
        pickLabel.hide();
        showHideButton.html("Afficher");
    } else {
        pickLabel.show();
        showHideButton.html("Cacher");
    }
    // pickLabel.toggle();
    // $("#personPick").toggle();
});