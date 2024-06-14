let current_theme = 'quantum-theme';

function convertToDateOnly(str) {
    var date = new Date(str);
    return date.toLocaleDateString("en-GB");
}

// Define a debounce function
function debounce(func, delay) {
    let timeoutId;
    return function () {
        const context = this;
        const args = arguments;
        clearTimeout(timeoutId);
        timeoutId = setTimeout(function () {
            func.apply(context, args);
        }, delay);
    };
}

function openThemeQuantum(theme) {
    // delete class of body
    $("body").removeClass(current_theme);
    $(".display-new-group").removeClass(current_theme);

    // update current theme
    current_theme = theme;

    // add new class
    $("body").addClass(current_theme);
    $(".display-new-group").addClass(current_theme);
}