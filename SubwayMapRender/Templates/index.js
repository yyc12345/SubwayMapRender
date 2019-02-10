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
        $(".smr-display-station").show();
    } else {
        $(".smr-display-line").hide();
        $(".smr-display-station").hide();
    }
}

function setDisplay(name) {
    restoreDisplay(false);
    $(".smr-display--" + name).show();
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

//station function =============================

function showWindowStation(stationId) {
    closeAllWindow();
    clearWindowStationBuilder();
    clearWindowStationLayout();
    generatedCodeShowStation(stationId);
    openWindow('uiWindowStation');
}

function setWindowStation(name, subtitle, id, desc, x, y, z) {
    $("#uiWindowStation-name").text(name);
    $("#uiWindowStation-subtitle").text(subtitle);
    $("#uiWindowStation-id").text(id);
    $("#uiWindowStation-description").text(desc);
    $("#uiWindowStation-tp").text("/tp @p " + x + " " + y + " " + z);
}

function clearWindowStationBuilder() {
    $("#uiWindowStation-builder").html("");
}

function addWindowStationBuilder(segment, builder) {
    $("#uiWindowStation-builder").append("<tr><td>" + segment + "</td><td>" + builder + "</td></tr>");
}

function clearWindowStationLayout() {
    $("#uiWindowStation-layout").html("");
}

function addWindowStationLayout(floor, isHorizon, metadata) {
    //insert title
    $("#uiWindowStation-layout").append('<p style="text-align: center;">' + floor + '</p>');
    //insert svg
    var insertedSvgCode = '<div style="text-align: center;"><svg xmlns="http://www.w3.org/2000/svg" version="1.1" width="200" height="200" style="flex: 1;">';

    var sp = metadata.split(',');
    var index = 0;
    for (item in sp) {
        insertedSvgCode += '<g>';
        //0 is type, 1 is name
        var inner = item.split('#');
        var type = inner[0];
        var name = inner[1];
        var className = name.replace(' ', '-');

        switch (type) {
            case 'v':
                insertedSvgCode += '<title>Void</title>';
                if (isHorizon) insertedSvgCode += '<rect x="0" y="' + (index * 20) + '" width="200" height="20" class="smr-window-layout smr-window-layout-void"/>';
                else insertedSvgCode += '<rect y="0" x="' + (index * 20) + '" width="20" height="200" class="smr-window-layout smr-window-layout-void"/>';
                break;
            case 'p':
                insertedSvgCode += '<title>Platform</title>';
                if (isHorizon) insertedSvgCode += '<rect x="0" y="' + (index * 20) + '" width="200" height="20" class="smr-window-layout smr-window-layout-platform"/>';
                else insertedSvgCode += '<rect y="0" x="' + (index * 20) + '" width="20" height="200" class="smr-window-layout smr-window-layout-platform"/>';
                break;
            case 'l':
                insertedSvgCode += '<title>' + name + '</title>';
                insertedSvgCode += '<rect x="0" y="' + (index * 20) + '" width="200" height="20" class="smr-window-layout smr-window-layout--' + className + '"/>';

                insertedSvgCode += '<line x1="10" y1="' + (index * 20 + 9) + '" x2="190" y2="' + (index * 20 + 9) + '" class="smr-window-layout-arrow"/>';
                insertedSvgCode += '<line x1="10" y1="' + (index * 20 + 9) + '" x2="20" y2="' + (index * 20 + 5) + '" class="smr-window-layout-arrow"/>';
                insertedSvgCode += '<line x1="10" y1="' + (index * 20 + 9) + '" x2="20" y2="' + (index * 20 + 13) + '" class="smr-window-layout-arrow"/>';

                break;
            case 'r':
            insertedSvgCode += '<title>' + name + '</title>';
                insertedSvgCode += '<rect x="0" y="' + (index * 20) + '" width="200" height="20" class="smr-window-layout smr-window-layout--' + className + '"/>';

                insertedSvgCode += '<line x1="10" y1="' + (index * 20 + 9) + '" x2="190" y2="' + (index * 20 + 9) + '" class="smr-window-layout-arrow"/>';
                insertedSvgCode += '<line x1="190" y1="' + (index * 20 + 9) + '" x2="180" y2="' + (index * 20 + 5) + '" class="smr-window-layout-arrow"/>';
                insertedSvgCode += '<line x1="190" y1="' + (index * 20 + 9) + '" x2="180" y2="' + (index * 20 + 13) + '" class="smr-window-layout-arrow"/>';

                break;
            case 'u':
            insertedSvgCode += '<title>' + name + '</title>';
                insertedSvgCode += '<rect y="0" x="' + (index * 20) + '" width="20" height="200" class="smr-window-layout smr-window-layout--' + className + '"/>';

                insertedSvgCode += '<line y1="10" x1="' + (index * 20 + 9) + '" y2="190" x2="' + (index * 20 + 9) + '" class="smr-window-layout-arrow"/>';
                insertedSvgCode += '<line y1="10" x1="' + (index * 20 + 9) + '" y2="20" x2="' + (index * 20 + 5) + '" class="smr-window-layout-arrow"/>';
                insertedSvgCode += '<line y1="10" x1="' + (index * 20 + 9) + '" y2="20" x2="' + (index * 20 + 13) + '" class="smr-window-layout-arrow"/>';

                break;
            case 'd':
            insertedSvgCode += '<title>' + name + '</title>';
                insertedSvgCode += '<rect y="0" x="' + (index * 20) + '" width="20" height="200" class="smr-window-layout smr-window-layout--' + className + '"/>';

                insertedSvgCode += '<line y1="10" x1="' + (index * 20 + 9) + '" y2="190" x2="' + (index * 20 + 9) + '" class="smr-window-layout-arrow"/>';
                insertedSvgCode += '<line y1="190" x1="' + (index * 20 + 9) + '" y2="180" x2="' + (index * 20 + 5) + '" class="smr-window-layout-arrow"/>';
                insertedSvgCode += '<line y1="190" x1="' + (index * 20 + 9) + '" y2="180" x2="' + (index * 20 + 13) + '" class="smr-window-layout-arrow"/>';

                break;
        }

        insertedSvgCode += '</g>'
        index++;
    }

    insertedSvgCode += "</svg></div>";
    $("#uiWindowStation-layout").append(insertedSvgCode);
}
