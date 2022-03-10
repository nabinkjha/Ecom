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
  
    var tableOptions = {
        searchDelay: 500,

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
                data.FilterBy = [];
                data.FilterBy.push({ entityName: "ProductCategory", propertyName: "ProductCategoryId", propertyValue: $('#filterProductCategoryId').val() })
                return JSON.stringify(data);
            }
        },
        "columnDefs": [
            { "width": "5%", "targets": [5,6] },
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
                return "<button type='button' class='btn btn-primary mr-1' onclick=editProduct('" + row.id + "');><i class='fa fa-edit'></i> Edit</button>";
            }
            },
            {
                "targets": 6,
                "data": "delete_link",
                "searchable": false,
                "render": function (data, type, row, meta) {
                    return "<button type='button' class='btn btn-danger' onclick=warnDeleteProduct('" + row.id + "','" + encodeURIComponent(row.name) + "'); ><i class='fa fa-trash'></i>Delete</button>";
                }
            }
        ],
        "columns": [
            { "data": "id", "name": "Id", "autoWidth": true },
            { "data": "sku", "name": "SKU", "autoWidth": true },
            { "data": "slug", "name": "Slug", "autoWidth": true },
            { "data": "name", "name": "Name", "autoWidth": true },
            { "data": "category", "name": "Category", "autoWidth": true },
        ],
        "dom": "<'row'<'col-sm-2 col-md-1'<'#actionButtonContainer'>><'col-sm-4 col-md-5'l><'col-sm-3 col-md-4'<'#filterContainer'>><'col-sm-3 col-md-2'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        "order": [2, "asc"]
    };
  
    $('#productTableId').dataTable().fnDestroy();
    var table = $('#productTableId').DataTable(tableOptions);
    $("#actionButtonContainer").append($("<button class='btn btn-sm bg-success' onclick='createProduct()'><i class='fa fa-plus'></i>Add Product</button>"));

    //$("#productTableId_length").addClass("float-left").prepend("<button class='btn btn-sm bg-success' style='margin-right: 30px;' onclick='createProduct()'>Create</button>");
    //Take the category filter drop down and append it to the datatables_filter div. 
    //You can use this same idea to move the filter anywhere withing the datatable that you want.
    $("#filterContainer").append($("#divCategoryFilter"));
  
    //Set the change event for the Category Filter dropdown to redraw the datatable each time
    //a user selects a new filter.
    $("#filterProductCategoryId").change(function (e) {
        table.draw();
    });

   // table.draw();


}
