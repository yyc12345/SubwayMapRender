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

//display function =========================
function restoreDisplay(isDisplayed) {
    if (isDisplayed) {
        $(".smr-display-line").show();
    } else {
        $(".smr-display-line").hide();
    }
}

function setDisplay(name) {
    restoreDisplay(false);
    $(".smr-display-line--"+name).show();
}

//line function ============================

function showWindowLine(nodeName) {
    closeAllWindow();
    clearWindowLineBuilder();
    generatedCodeShowLine(nodeName);
    openWindow('uiWindowLine');
}

function setWindowLine(color, name) {
    $("#uiWindowLine-color").attr("color", color);
    $("#uiWindowLine-name").text(name);
}

function clearWindowLineBuilder() {
    $("#uiWindowLine-builder").html("");
}

function addWindowLineBuider(segment, builder) {
    $("#uiWindowLine-builder").append("<tr><td>" + segment + "</td><td>" + builder + "</td></tr>");
}
