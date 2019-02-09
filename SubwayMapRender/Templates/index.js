//window manager ==========================

function openWindow(name) {
    closeAllWindow();
    $('#' + name).show();
}

function closeWindow(name) {
    $("#" + name).hide();
}

function closeAllWindow() {
    closeWindow("uiWindowDisplay");
    closeWindow("uiWindowLine");
    closeWindow("uiWindowStation");
}

