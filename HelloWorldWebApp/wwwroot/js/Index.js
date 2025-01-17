﻿"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/messagehub").build();

connection.start()

connection.on("NewTeamMemberAdded", (name, id) => {
    createNewLine(name, id)
});

connection.on("TeamMemberDeleted", id => {
    removeTeamMemberFromList(id)
})

connection.on("UpdateTeamMember", (name, id) => {
    updateTeamMemberFromList(name, id)
})


$(document).ready(function () {


    $("#addMembersButton").click(function () {
        var newcomerName = $("#nameField").val();

        $.ajax({
            url: "/Home/AddTeamMember",
            method: "POST",
            data: {
                teamMember: newcomerName
            },
            success: function (result) {
                $("#nameField").val("");
                document.getElementById("addMembersButton").disabled = true;
            }
        })
    })

    $('#submit').click(function () {
        const id = $('#editClassmate').attr('member-id');
        const newName = $('#classmateName').val();

        $.ajax({
            url: "/Home/UpdateMemberName",
            method: "POST",
            data: {
                memberId: id,
                name: newName
            }
        })
    });

    $("#teamMembers").on("click", ".pencil", function () {
        var targetMemberTag = $(this).closest('li');
        var id = targetMemberTag.attr('member-id');
        var currentName = targetMemberTag.find(".name").text();
        $('#editClassmate').attr("member-id", id);
        $('#classmateName').val(currentName);
        $('#editClassmate').modal('show');
    })
});

function deleteMember(index) {

    $.ajax({
        url: "/Home/RemoveMember",
        method: "DELETE",
        data: {
            memberIndex: index
        }
    })
}

(function () {

    $('#nameField').on('change textInput input', function () {
        var inputVal = this.value;
        if (inputVal != "") {
            document.getElementById("addMembersButton").disabled = false;
        } else {
            document.getElementById("addMembersButton").disabled = true;
        }
    });
}());

(function () {
    $("#clearButton").click(function () {
        document.getElementById("nameField").value = "";
    });
}());

const createNewLine = (name, id) => {
    $("#teamMembers").append(
        `<li class="member" member-id=${id}>
            <span class="name" >${name}</span>
            <span class="delete fa fa-remove" onclick="deleteMember(${id})"></span>
            <span class="pencil fa fa-pencil"></span>
        </li>`);
}

const removeTeamMemberFromList = (teamMemberId) => $(`li[member-id=${teamMemberId}]`).remove()

const updateTeamMemberFromList = (teamMemberName, teamMemberId) => $(`li[member-id=${teamMemberId}]`).children(".name").text(teamMemberName)