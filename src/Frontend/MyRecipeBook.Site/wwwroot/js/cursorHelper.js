window.CursorWait = function () {
    document.body.classList.add("wait-cursor");
};

window.CursorDefault = function () {
    document.body.classList.remove("wait-cursor");
};