window.onload = function () {
    userInformation();
    showSeachBar();
    showTable();
};

function userInformation() {

    $.ajax({
        url: "User/GetEmail",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data) {
                document.getElementById("email").textContent = data.email;
            } else {
                console.error("No movies found or unexpected response format.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error retrieving movies:", textStatus, errorThrown);
        }
    });
}
function showSeachBar() {

    $.ajax({
        url: "User/GetSearchValue",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data) {
                document.getElementById("movieSearch").value = data.value;
                document.getElementById("movieSearchBy").value = data.option;
            } else {
                console.error("No movies found or unexpected response format.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error retrieving movies:", textStatus, errorThrown);
        }
    });
}

function handleChoiceChange() {
    const searchValue = document.getElementById("movieSearch").value;
    const searchOption = document.getElementById("movieSearchBy").value;
    searchMovieList(searchOption, searchValue);
}

function handleSearch(event) {
    if (event.key === "Enter") {
        const searchValue = document.getElementById("movieSearch").value;
        const searchOption = document.getElementById("movieSearchBy").value;
        searchMovieList(searchOption, searchValue);
    }
}

function searchMovieList(option, value) {

    $.ajax({
        url: "User/PostSearchValue",
        dataType: "json",
        type: "POST",
        data: { option: option, value: value },
        success: function (data) {
            if (data) {
                showTable();
            } else {
                console.error("No movies found or unexpected response format.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error retrieving movies:", textStatus, errorThrown);
        }
    });
}

function showTable() {

    $.ajax({
        url: "User/GetMovie",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data) {
                const tableBody = document.getElementById("movieTableBody");
                tableBody.innerHTML = "";

                for (let i = 0; i < data.length; ++i) {
                    const tableRow = document.createElement('tr');
                    tableRow.innerHTML = `<td>${data[i].title}</td>
                                          <td>${data[i].cast}</td>
                                          <td>${data[i].category}</td>
                                          <td><button type="button" class="btn btn-primary" onclick="changeFavourite(${data[i].id})">Remove from Favourite</button>
                                          <button  class="btn btn-primary" onclick="details(${data[i].id})">Details</button> 
                                          </td>`;

                    tableBody.appendChild(tableRow);  //onlclick=changeFavourite(${data[i].id})
                }
            } else {
                console.error("Error retrieving Movie:", data.error);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error sending AJAX request:", textStatus, errorThrown);
        }
    });
}

function changeFavourite(id) {
    $.ajax({
        url: "User/ChangeFavourite",
        dataType: "json",
        type: "POST",
        data: { id: id },
        success: function (data) {
            if (data.success) {
                showTable();
            } else {
                window.location.href = `/register`;
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error sending AJAX request:", textStatus, errorThrown);
        }
    });
}

function details(id) {
    window.location.href = `/Home/Details/${id}`;
}