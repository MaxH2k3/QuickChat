// Create display message
connection.on("Send", function (result) {
    var message = JSON.parse(result);
    var groupId = document.getElementById("group-name").value;
    if (message.GroupId.toString() !== groupId.toString()) {
        return;
    }

    var display_message = document.getElementById("display-message");
    if (thisUserId == message.UserId) {
        display_message.innerHTML += renderMyMessage(message.Content, message.CreatedDate);
    } else {
        display_message.innerHTML += renderYourMessage(message.Content, message.CreatedDate);
    }

    scrollToEndOfPage();
});

// Create display group
connection.on("DisplayGroup", function (groupId, groupName) {
    renderNewGroup(groupId, groupName);
});

// Load message for group
connection.on("LoadMessageForGroup", function (listMessage) {
    var list = JSON.parse(listMessage);

    // Load message to display
    var display_message = document.getElementById("display-message");
    display_message.innerHTML = "";

    try {
        Array.from(list).forEach((element, index) => {
            if (thisUserId == element.UserId) {
                display_message.innerHTML += renderMyMessage(element.Content, element.CreatedDate);
            } else {
                display_message.innerHTML += renderYourMessage(element.Content, element.CreatedDate);
            }
        });

        scrollToEndOfPage();

    } catch (e) {
        console.error(e.toString());
    }
    

});

// Create new group
document.getElementById("create-group").addEventListener("click", async (event) => {
    var groupName = document.getElementById("new-group-name");
    try {
        await connection.invoke("CreateNewGroup", groupName.value);
        groupName.value = "";
    }
    catch (e) {
        console.error(e.toString());
    }
    event.preventDefault();
});

// Handle send message to group
document.getElementById("groupmsg").addEventListener("click", async (event) => {
    var groupName = document.getElementById("group-name");
    var groupMsg = document.getElementById("group-message-text");

    try {
        await connection.invoke("SendMessageToGroup", groupName.value, groupMsg.value);
        groupMsg.value = "";
    }
    catch (e) {
        console.error(e.toString());
    }
    event.preventDefault();
});

// Handle leave group
document.getElementById("leave-group").addEventListener("click", async (event) => {
    var groupName = document.getElementById("group-name").value;
    try {
        await connection.invoke("RemoveFromGroup", groupName);
    }
    catch (e) {
        console.error(e.toString());
    }
    event.preventDefault();
});

// Handle load message to group


// Handle add new group to display group component
function addGroupTag(groupId, groupName, numOfMember) {
    // Remove active
    const currentActiveLi = document.querySelector("li.active");
    if (currentActiveLi) {
        currentActiveLi.classList.remove("active");
    }

    let groups = document.getElementById("groups");
    let listLi = groups.getElementsByTagName("li");
    let liTag = document.createElement("li");
    if (listLi.length == 0) {
        liTag.classList.add("active");
        liTag.innerHTML = renderNewJoinGroup(groupId, groupName, numOfMember);
    } else {
        liTag.innerHTML = renderNewJoinGroup(groupId, groupName, numOfMember);
    }
    groups.appendChild(liTag);

    liTag.addEventListener("click", () => {
        addEventClickForGroup(liTag);
    });
}




