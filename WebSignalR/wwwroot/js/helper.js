function convertToDateOnly(str) {
    var date = new Date(str);
    return date.toLocaleDateString("en-GB");
}