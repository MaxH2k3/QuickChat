

// Event scroll to end of page
function scrollToEndOfPage() {
    let element = document.getElementById("display-message");
    element.scrollTo(0, element.scrollHeight);
}

// Add event click for join group
function addEventClickJoinGroup(element) {
    let idDetail = element.querySelector(".group-id-detail");
    let nameDetail = element.querySelector(".group-name-detail");
    let numOfMember = element.querySelector(".group-numOfMember-detail");
    let btnJoinGroup = element.querySelector(".join-group");
    let displayMessage = document.getElementById("display-message");
    btnJoinGroup.addEventListener("click", async (event) => {
        try {
            await connection.invoke("AddToGroup", idDetail.textContent);
            // Delete tag on display new group component
            element.remove();
            // Add tag to display group component
            addGroupTag(idDetail.textContent, nameDetail.textContent, numOfMember.textContent);
            // Add event click for new group
            let elementJoinGroup = document.getElementById("groups").lastElementChild;
            addEventClickForGroup(elementJoinGroup);
            // Add event load message for group click
            addEventLoadMessageForGroup(elementJoinGroup);
            // Add event leave group
            addEventLeaveGroup(elementJoinGroup);
            // Clear message
            displayMessage.innerHTML = "";
            // Add message avaible
            getMessageOnGroup(elementJoinGroup);

            // update group user on list
            updateJoiningGroupUser(idDetail.textContent, true);
        }
        catch (e) {
            console.error(e);
        }
        event.preventDefault();
    });
}

// Add event click for group (active group chat)
function addEventClickForGroup(currentGroup) {
    document.getElementById("group-name").value = currentGroup.querySelector(".group-id-sidebar").value;

    // Active chat group
    const currentActiveLi = document.querySelector("li.active");
    if (currentActiveLi) {
        currentActiveLi.classList.remove("active");
    }

    currentGroup.classList.add("active");

}

function addEventLoadMessageForGroup(element) {
    $(element).on("click", async (event) => {
        getMessageOnGroup(element);
        event.preventDefault();
    });

}

function addEventLeaveGroup(element) {
    let groupId = element.querySelector(".group-id-sidebar").value;
    element.querySelector(".btn-leave-group").addEventListener("click", async (event) => {
        event.stopPropagation();
        try {
            await connection.invoke("RemoveFromGroup", groupId);

            // Check active is being
            if ($(element).hasClass("active")) {
                // Active next group
                const nextElement = $(element).next();
                nextElement.addClass("active");
                getMessageOnGroup(nextElement);
            }

            // Remove tag group
            element.remove();

            // update group user on list
            updateJoiningGroupUser(groupId, false);

        } catch (e) {
            console.error(e.toString());
        }
        event.preventDefault();
    });
}

const updateJoiningGroupUser = (groupId, status) => {
    let index = listGroups.findIndex(g => g.groupId === groupId);
    listGroups[index].isJoining = status;
    if(status) {
        listGroups[index].NumOfMember += 1;
    } else {
        listGroups[index].NumOfMember -= 1;
    }
    useSearchNewGroup("");
}