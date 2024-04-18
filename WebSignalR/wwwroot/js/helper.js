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