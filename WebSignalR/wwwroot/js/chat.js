// Create display message
connection.on("SendMessage", function (result) {
    let message = result;
    let groupId = document.getElementById("group-name").value;
    if (message.groupId.toString() !== groupId.toString()) {
        return;
    }

    let display_message = document.getElementById("display-message");
    if (thisUserId === message.userId) {
        display_message.innerHTML += renderMyMessage(message.content, message.createdDate);
    } else {
        display_message.innerHTML += renderYourMessage(message.content, message.createdDate);
    }

    scrollToEndOfPage();
});

// Create display group
connection.on("DisplayGroup", function (groupId, groupName) {
    renderNewGroup(groupId, groupName);
});

// Load message for group
connection.on("LoadMessageForGroup", function (listMessage) {

    // Load message to UI
    loadMessage(listMessage);

});

const loadMessage = (list) => {
    // Load message to display
    let display_message = document.getElementById("display-message");
    display_message.innerHTML = "";

    try {
        Array.from(list).forEach((element) => {
            if (thisUserId === element.userId) {
                display_message.innerHTML += renderMyMessage(element.content, element.createdDate);
            } else {
                display_message.innerHTML += renderYourMessage(element.content, element.createdDate);
            }
        });

        scrollToEndOfPage();

    } catch (e) {
        console.error(e.toString());
    }
}

const getMessageOnGroup = async (element) => {
    try {
        let groupId = $(element).children('.group-id-sidebar').attr("value");
        await connection.invoke("GetMessageOnGroup", groupId);
    }
    catch (e) {
        console.error(e.toString());
    }
}

// Create new group
document.getElementById("create-group").addEventListener("click", async (event) => {
    let groupName = document.getElementById("new-group-name");
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
    let groupName = document.getElementById("group-name");
    let groupMsg = document.getElementById("group-message-text");

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
    let groupName = document.getElementById("group-name").value;
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

    liTag.classList.add("d-flex");
    liTag.classList.add("justify-content-between");

    groups.appendChild(liTag);

    // Add active tag
    liTag.addEventListener("click", () => {
        addEventClickForGroup(liTag);
    });
}






