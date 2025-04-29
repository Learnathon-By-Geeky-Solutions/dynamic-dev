let sortColumn = "Name";
let sortOrder = "asc";

function loadAgencies(page = 1) {
    let search = $("#searchInput").val();

    $.get("/Admin/AdminAgency/LoadData", {
        searchTerm: search,
        sortColumn: sortColumn,
        sortOrder: sortOrder,
        page: page,
        pageSize: 10
    }, function (html) {
        $("#agencyTableContainer").html(html);
    });
}

$("#searchInput").on("input", function () {
    loadAgencies(1);
});


function sortTable(column) {
    if (sortColumn === column) {
        // If already sorting by this column, toggle order
        sortOrder = (sortOrder === 'asc') ? 'desc' : 'asc';
    } else {
        // Otherwise, new column, default to ascending
        sortColumn = column;
        sortOrder = 'asc';
    }

    // Reload page 1 with new sorting
    loadAgencies(1);
}


function openCreateModal() {
    $.get("/Admin/AdminAgency/Create", function (html) {
        $("#modalsContainer").html(html);
        $("#createEditModal").modal("show");

        $("#createEditForm").submit(function (e) {
            e.preventDefault();
            $.post("/Admin/AdminAgency/Create", $(this).serialize(), function () {
                $("#createEditModal").modal("hide");
                loadAgencies();
            });
        });
    });
}

function openEditModal(id) {
    $.get("/Admin/AdminAgency/Edit/" + id, function (html) {
        $("#modalsContainer").html(html);
        $("#createEditModal").modal("show");

        $("#createEditForm").submit(function (e) {
            e.preventDefault();
            $.post("/Admin/AdminAgency/Edit", $(this).serialize(), function () {
                $("#createEditModal").modal("hide");
                loadAgencies();
            });
        });
    });
}

function openDeleteModal(id) {
    $.get("/Admin/AdminAgency/Delete/" + id, function (html) {
        $("#modalsContainer").html(html);
        $("#deleteModal").modal("show");

        $("#deleteForm").submit(function (e) {
            e.preventDefault();
            $.post("/Admin/AdminAgency/Delete", $(this).serialize(), function () {
                $("#deleteModal").modal("hide");
                loadAgencies();
            });
        });
    });
}

$(document).ready(function () {
    loadAgencies();
});

$(document).on("click", ".pagination a", function (e) {
    e.preventDefault();
    let page = $(this).data("page");
    if (page) {
        loadAgencies(page);
    }
});

