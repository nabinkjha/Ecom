
$(function () {
    loadProductCategory();
    $(document).keypress(function (e) {
        if (e.which == 13) {
            loadProductCategory();
        }
    });
});

function createProductCategory() {
    var url = "/ProductCategory/Create";
    showDialogWindow("divProductCategory", "frmProductCategory", url, "Create ProductCategory");
}

function editProductCategory(id) {
    var url = "/ProductCategory/Edit?id=" + id;
    showDialogWindow("divProductCategory", "frmProductCategory", url, "Edit ProductCategory");
}
function deleteProductCategory(id) {
    var url = "/ProductCategory/Delete?id=" + id;
    $.ajax({
        url: url,
        headers: {
            'X-CSRF-Token': $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        type: "POST",
        success: function (data) {
            swal("Deleted!", data.message, "success");
            loadProductCategory();
        }
    });
    return false;
}
function warnDeleteProductCategory(id, name) {
    var message = `The ProductCategory ${decodeURI(name)} will be deleted permanently.`;
    displayDeleteAlert(message, deleteProductCategory, id);
}


function loadProductCategory() {
    $('#productCategoryTableId').dataTable().fnDestroy();
    $('#productCategoryTableId').DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ordering": true,
        "deferRender": true,
        "drawCallback": function () {
            $("#dataTable_wrapper").children().eq(1).css("overflow", "auto");
        },
        "ajax": {
            "type": "POST",
            "url": baseUrl + "ProductCategory/GetSearchResult",
            "datatype": "json",
            "contentType": "application/json; charset=utf-8",
            "headers": { 'RequestVerificationToken': $('#__RequestVerificationToken').val() },
            "data": function (data) {
                return JSON.stringify(data);
            }
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        },
        {
            "targets": 2,
            "data": "edit_link",
            "render": function (data, type, row, meta) {
                return "<button type='button' class='btn btn-primary mr-1' onclick=editProductCategory('" + row.id + "'); >Edit</button><button type='button' class='btn btn-danger' onclick=warnDeleteProductCategory('" + row.id + "','" + encodeURIComponent(row.name) + "');>Delete</button>";
            }
        }
        ],
        "columns": [
            { "data": "id", "name": "Id", "autoWidth": true },
            { "data": "name", "name": "Name", "autoWidth": true },
        ],
        "dom": "<'row'<'col-sm-6'l><'col-sm-6'<'#buttonContainer.site-datatable-button-container'>f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "order": [0, "desc"]
    });
    $("#buttonContainer").addClass("float-right").append("<button class='btn btn-sm bg-success' onclick='createProductCategory()'>Create</button>");
}