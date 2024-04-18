function renderNewGroup(groupId, groupName) {
    let listTag = document.getElementById("list-new-group");
    let trTag = document.createElement("tr");
    trTag.classList.add("detail-group");
    let element = `
            <td class="group-id-detail d-none">${groupId}</td>
            <td class="group-name-detail">${groupName}</td>
            <td class="group-numOfMember-detail">0</td>
            <td>${new Date().toLocaleDateString()}</td>
            <td class="join-group"><i class="fa-solid fa-plus"></i></td>
    `;

    trTag.innerHTML = element;
    listTag.appendChild(trTag);
    addEventClickJoinGroup(trTag);

}

function renderNewJoinGroup(groupId, groupName, numOfMember) {
    let element = `
                        <input type="hidden" name="group-id" class="group-id-sidebar" value="${groupId}" />
							<div class="d-flex bd-highlight w-85">
								<div class="img_cont">
									<img src="https://static.turbosquid.com/Preview/001292/481/WV/_D.jpg" class="rounded-circle user_img">
									<span class="online_icon"></span>
								</div>
								<div class="user_info overflow-hidden w-100">
									<span class="group-name-sidebar">${groupName}</span>
									<p>${numOfMember}</p>
								</div>
							</div>
                            <div class="mt-auto mb-auto">
								<a href="#" type="button" class="btn-leave-group"><i class="fa-solid fa-arrow-right-from-bracket"></i></a>
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
        <div class="msg_cotainer max-w-90">
            <span class="text-wrap">${message}</span>
            <span class="msg_time min-w-50px">${createdDate}</span>
        </div>
        </div >
    `;

    return element;

}

function renderMyMessage(message, createdDate) {
    let element = `
        <div class="d-flex justify-content-end mb-4">
        <div class="msg_cotainer_send max-w-90">
            <span class="text-wrap">${message}</span>
            <span class="msg_time_send min-w-50px">${createdDate}</span>
        </div>
        <div class="img_cont_msg">
            <img src="https://static.turbosquid.com/Preview/001292/481/WV/_D.jpg" class="rounded-circle user_img_msg" />
        </div>
    </div>
    `;

    return element;

}
