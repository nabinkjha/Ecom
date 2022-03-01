function createProduct() {
    var url = "/Product/Create";
    showDialogWindow("divProduct", "frmProduct", url, "Create Product");
}

function editProduct(id) {
    var url = "/Product/Edit?id=" + id;
    showDialogWindow("divProduct", "frmProduct", url, "Edit Product");
}
function deleteProduct(id) {
    var url = "/Product/Delete?id=" + id;
    $.ajax({
        url: url,
        headers: {
            'X-CSRF-Token': $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        type: "POST",
        success: function (data) {
            swal("Deleted!", data.message, "success");
            loadProduct();
        }
    });
    return false;
}
function warnDeleteProduct(id,name) {
    var message = `The product ${decodeURI(name)} will be deleted permanently.`;
    displayDeleteAlert(message, deleteProduct, id);
}
$(function () {
    loadProduct();
    $(document).keypress(function (e) {
        if (e.which == 13) {
            loadProduct();
        }
    });
})

function loadProduct() {
    $('#productTableId').dataTable().fnDestroy();
    $('#productTableId').DataTable({
        searchDelay: 500,
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
            "url": baseUrl + "Product/GetProductList",
            "datatype": "json",
            "contentType": "application/json; charset=utf-8",
            //"headers": { 'RequestVerificationToken': $('#__RequestVerificationToken').val() },
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
            "targets": 4,
            "data": "edit_link",
            "render": function (data, type, row, meta) {
                return "<button type='button' class='btn btn-primary mr-1' onclick=editProduct('" + row.id + "');  >Edit</button><button type='button' class='btn btn-danger' onclick=warnDeleteProduct('" + row.id + "','" + encodeURIComponent(row.name) + "'); >Delete</button>";
            }
        }
        ],
        "columns": [
            { "data": "id", "name": "Id", "autoWidth": true },
            { "data": "sku", "name": "SKU", "autoWidth": true },
            { "data": "slug", "name": "Slug", "autoWidth": true },
            { "data": "name", "name": "Name", "autoWidth": true },
        ],
        "dom": "<'row'<'col-sm-6'l><'col-sm-6'<'#buttonContainer.site-datatable-button-container'>f>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "order": [1, "desc"]
    });
    $("#buttonContainer").addClass("float-right").append("<button class='btn btn-sm bg-success' onclick='createProduct()'>Create</button>");
}