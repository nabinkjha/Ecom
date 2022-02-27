function createProduct() {
    var url = "/Product/Create";
    showDialogWindow("divProduct", "frmProduct", url, "Create Product");
}

function editProduct(id) {
    var url = "/Product/Edit?id=" + id;
    showDialogWindow("divProduct", "frmProduct", url, "Edit Product");
}
function deleteProduct(message) {
    var type = 'info';
    displayMessage(message, type, type);
}
