function renderNewGroup(groupId, groupName) {
    let listTag = document.getElementById("list-new-group");
    let trTag = document.createElement("tr");
    trTag.classList.add("detail-group");
    let element = `
            <td class="group-id-detail">${groupId}</td>
            <td class="group-name-detail">${groupName}</td>
            <td class="group-numOfMember-detail">0</td>
            <td class="join-group"><i class="fa-solid fa-plus"></i></td>
    `;

    trTag.innerHTML = element;
    listTag.appendChild(trTag);
    addEventClickJoinGroup(trTag);

}

function renderNewJoinGroup(groupId, groupName, numOfMember) {
    let element = `
                        <input type="hidden" name="group-id" class="group-id-sidebar" value="${groupId}" />
							<div class="d-flex bd-highlight">
								<div class="img_cont">
									<img src="https://static.turbosquid.com/Preview/001292/481/WV/_D.jpg" class="rounded-circle user_img">
									<span class="online_icon"></span>
								</div>
								<div class="user_info">
									<span class="group-name-sidebar">${groupName}</span>
									<p>${numOfMember}</p>
								</div>
							</div>
    `
    return element;
}

function renderYourMessage(message, createdDate) {
    let element = `
        <div class="d-flex justify-content-start mb-4">
        <div class="img_cont_msg">
            <img src="https://static.turbosquid.com/Preview/001292/481/WV/_D.jpg" class="rounded-circle user_img_msg" />
        </div>
        <div class="msg_cotainer">
            ${message}
            <span class="msg_time">${convertToDateOnly(createdDate)}</span>
        </div>
        </div >
    `;

    return element;

}

function renderMyMessage(message, createdDate) {
    let element = `
        <div class="d-flex justify-content-end mb-4">
        <div class="msg_cotainer_send">
            ${message}
            <span class="msg_time_send">${convertToDateOnly(createdDate)}</span>
        </div>
        <div class="img_cont_msg">
            <img src="https://static.turbosquid.com/Preview/001292/481/WV/_D.jpg" class="rounded-circle user_img_msg" />
        </div>
    </div>
    `;

    return element;

}
