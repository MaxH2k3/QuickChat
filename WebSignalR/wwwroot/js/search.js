const searchGroupUser = (name) => {
    /*$.ajax({
        type: "Get",
        url: "/Menu/?handler=SearchGroup",
        contentType: "application/json",
        dataType: "json",
        data: { name: name, userId: userId },
        success: function (response) {
            return response;
        },
        error: function (error) {
            console.error(error);
        }
    });*/

    let filter = listGroups.filter(g => g.isJoining);
    
    if(name !== "") {
        filter = listGroups.filter(g => g.isJoining && g.groupName.includes(name));
    }

    $("#groups").empty();
    filter.forEach((group) => {
        addGroupTag(group.groupId, group.groupName, group.numOfMember);
        // Add event click for new group
        let elementJoinGroup = document.getElementById("groups").lastElementChild;
        addEventClickForGroup(elementJoinGroup);
        // Add event load message for group click
        addEventLoadMessageForGroup(elementJoinGroup);
        // Add event leave group
        addEventLeaveGroup(elementJoinGroup);
    });
    
    return filter;
}

const searchNewGroup = (name) => {
    let filter = listGroups.filter(g => !g.isJoining);

    if(name !== "") {
        filter = listGroups.filter(g => !g.isJoining && g.groupName.includes(name));
    }
    
    $("#list-new-group").empty();
    filter.forEach((group) => {
        renderNewGroup(group.groupId, group.groupName); 
    });
    
}

const useSearchGroupUser = debounce(searchGroupUser, 500);
const useSearchNewGroup = debounce(searchNewGroup, 500);