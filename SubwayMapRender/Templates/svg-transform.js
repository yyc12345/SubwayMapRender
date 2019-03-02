isMouseDown = false;
isFirstSet = true;

oldPosX = 0;
oldPosY = 0;
newPosX = 0;
newPosY = 0;

currentScale = 1;
currentTranslateX = 0;
currentTranslateY = 0;

$(document).ready(function () {
    $("#uiSvg").mousedown(mouseDownHandle);
    $("#uiSvg").mousemove(mouseMoveHandle);
    $("#uiSvg").mouseup(mouseUpHandle);
    $("#uiSvg").on('mousewheel', mouseWheelHandle)
});

function mouseDownHandle() {
    isMouseDown = true;
    isFirstSet = true;
}

function mouseMoveHandle(e) {
    if (isMouseDown) {
        if (isFirstSet) {
            oldPosX = e.clientX;
            oldPosY = e.clientY;
            isFirstSet = false;
            return;
        }

        newPosX = e.clientX;
        newPosY = e.clientY;
        var realScale = getRealScale(currentScale);
        currentTranslateX += (newPosX - oldPosX) / realScale;
        currentTranslateY += (newPosY - oldPosY) / realScale;
        oldPosX = newPosX;
        oldPosY = newPosY;
        setScaleAndTranslate();
    }
}

function mouseUpHandle() {
    isMouseDown = false;
}

function mouseWheelHandle(e) {
    var angle = 0;
    if (e.originalEvent.wheelDelta) {//IE/Opera/Chrome  
        angle = e.originalEvent.wheelDelta;
    } else if (e.originalEvent.detail) {//Firefox  
        angle = e.originalEvent.detail;
    } else angle = 0;

    //pre calc translate
    var sx = 0;
    var sy = 0;
    var cx = sx - currentTranslateX;
    var cy = sy - currentTranslateY;
    var oldScale = getRealScale(currentScale);
    var rx = cx / oldScale;
    var ry = cy / oldScale;

    if (angle > 0) setCurrentScale(true, false);
    else if (angle < 0) setCurrentScale(false, false);
    else return;

    //re-calc translate
    var newScale = getRealScale(currentScale);
    var newCx = rx * newScale;
    var newCy = ry * newScale;
    currentTranslateX = sx - newCx;
    currentTranslateY = sy - newCy;

    setScaleAndTranslate();
}

function getRealScale(curScale) {
    var applyScale = 1.0;
    if (curScale > 1) applyScale = curScale;
    else if (curScale < -1) applyScale = 1.0 / (-curScale);
    else applyScale = 1.0;

    return applyScale;
}

function setCurrentScale(isUp, isSmall) {
    if (isUp) {
        if (isSmall) currentScale += 0.1;
        else currentScale += 1.0;
    } else {
        if (isSmall) currentScale -= 0.1;
        else currentScale -= 1.0;
    }

    if (currentScale < 1 && currentScale > -1) {
        if (isUp) currentScale = 1.0;
        else currentScale = -1.0;
    }
}

function setScaleAndTranslate() {
    var applyScale = getRealScale(currentScale);
    $("#uiSvgRoot").attr('transform', 'scale(' + applyScale + ') translate(' + currentTranslateX + ',' + currentTranslateY + ')');
}
