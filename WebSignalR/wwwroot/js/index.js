// Connection to hub server
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

// Get element storage userId
const thisUserId = document.getElementById("user-id").value;
const btnSendMessage = document.getElementById("groupmsg");

// Prevent key on textarea 
document.getElementById("group-message-text").addEventListener("keydown", (e) => {
    if (e.key == "Enter" && !e.shiftKey) {
        e.preventDefault();
        btnSendMessage.click();
    }
});

// Scroll to page at the begining
scrollToEndOfPage();

// We need an async function in order to use await, but we want this code to run immediately,
// so we use an "immediately-executed async function"
(async () => {
    try {
        await connection.start();
    }
    catch (e) {
        console.error(e.toString());
    }
})();


// Handle join group
Array.from(document.getElementsByClassName("detail-group")).forEach((element, index) => {
    addEventClickJoinGroup(element);
});

// Add event listener for the group exist
Array.from(document.getElementById("groups").getElementsByTagName("li")).forEach((element, index) => {

    // Add event click active group
    element.addEventListener("click", function () {
        addEventClickForGroup(element);
    });

    // Add event load message for group click
    addEventLoadMessageForGroup($(element));

    // Add event leave group exist
    addEventLeaveGroup(element);
});

