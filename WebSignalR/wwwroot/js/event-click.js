
// Event scroll to end of page
function scrollToEndOfPage() {
    var element = document.getElementById("display-message");
    element.scrollTo(0, element.scrollHeight);
}

// Add event click for join group
function addEventClickJoinGroup(element) {
    var idDetail = element.querySelector(".group-id-detail");
    var nameDetail = element.querySelector(".group-name-detail");
    var numOfMember = element.querySelector(".group-numOfMember-detail");
    var btnJoinGroup = element.querySelector(".join-group");
    var displayMessage = document.getElementById("display-message");
    btnJoinGroup.addEventListener("click", async (event) => {
        try {
            await connection.invoke("AddToGroup", idDetail.textContent);
            // Delete tag on display new group component
            element.remove();
            // Add tag to display group component
            addGroupTag(idDetail.textContent, nameDetail.textContent, numOfMember.textContent);
            // Add event click for new group
            var elementJoinGroup = document.getElementById("groups").lastElementChild;
            addEventClickForGroup(elementJoinGroup);
            // Add event load message for group click
            addEventLoadMessageForGroup(elementJoinGroup);
            // Clear message
            displayMessage.innerHTML = "";
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
    var groupId = element.querySelector(".group-id-sidebar").value;
    element.addEventListener("click", async (event) => {

        try {
            await connection.invoke("LoadMessageOnGroup", groupId);
        }
        catch (e) {
            console.error(e.toString());
        }
        event.preventDefault();
    });


}