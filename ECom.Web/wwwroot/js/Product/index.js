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
function warnDeleteProduct(id, name) {
    var message = `The product ${decodeURI(name)} will be deleted permanently.`;
    displayDeleteAlert(message, deleteProduct, id);
}
$(function () {
    loadProductTable();
})

function loadProductTable() {

    var tableOptions = {
        searchDelay: 500,

        "serverSide": true,
        "filter": true,
        "processing": true,
        "ordering": true,
        "deferRender": true,
        "drawCallback": function () {
            $("#dataTable_wrapper").children().eq(1).css("overflow", "auto");
        },
        language:
        {
            processing: "<div class=''><i class='fa fa-cog fa-spin site-loader-color'></i></div>",
            search: "filter",
            searchPlaceholder: "product name or desc"
        },
        "ajax": {
            "type": "POST",
            "url": baseUrl + "Product/GetProductList",
            "datatype": "json",
            "contentType": "application/json; charset=utf-8",
            "headers": { 'RequestVerificationToken': $('#__RequestVerificationToken').val() },
            "data": function (data) {
                data.FilterBy = [];
                data.FilterBy.push({ entityName: "ProductCategory", propertyName: "ProductCategoryId", propertyValue: $('#filterProductCategoryId').val() })
                return JSON.stringify(data);
            }
        },
        "columnDefs": [
            { "width": "5%", "targets": [5, 6] },
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            },
            {
                "targets": 5,
                "data": "edit_link",
                "searchable": false,
                "render": function (data, type, row, meta) {
                    return "<button type='button' class='btn btn-primary ' onclick=editProduct('" + row.id + "');><i class='fa fa-edit'></i><span class='ml-1'>Edit</span></button>";
                }
            },
            {
                "targets": 6,
                "data": "delete_link",
                "searchable": false,
                "render": function (data, type, row, meta) {
                    return "<button type='button' class='btn btn-danger' onclick=warnDeleteProduct('" + row.id + "','" + encodeURIComponent(row.name) + "'); ><i class='fa fa-trash'></i><span class='ml-1'>Delete</span></button>";
                }
            }
        ],
        "columns": [
            { "data": "id" },
            { "data": "sku" },
            { "data": "slug" },
            { "data": "name" },
            { "data": "category" },
        ],
        dom: "<'row'<'col-sm-2 col-md-1'<'#actionButtonContainer'>><'col-sm-4 col-md-5'Bl><'col-sm-3 col-md-4'<'#filterContainer'>><'col-sm-3 col-md-2'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        buttons: [
           'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "order": [2, "asc"]
    };

    $('#productTableId').dataTable().fnDestroy();
    var table = $('#productTableId').DataTable(tableOptions);
    $("#actionButtonContainer").append($("<button class='btn btn-md dt-button' onclick='createProduct()'><i class='fa fa-plus'></i><span>&nbsp;Add</span></button>"));

    //Take the category filter drop down and append it to the datatables_filter div. 
    //You can use this same idea to move the filter anywhere withing the datatable that you want.
    $("#filterContainer").append($("#divCategoryFilter"));

    //Set the change event for the Category Filter dropdown to redraw the datatable each time
    //a user selects a new filter.
    $("#filterProductCategoryId").change(function (e) {
        table.draw();
    });
}
