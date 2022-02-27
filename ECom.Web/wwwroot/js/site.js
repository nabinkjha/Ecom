/*\
|*|
|*|  :: cookies.js ::
|*|
|*|  A complete cookies reader/writer framework with full unicode support.
|*|
|*|  Revision #1 - September 4, 2014
|*|
|*|  https://developer.mozilla.org/en-US/docs/Web/API/document.cookie
|*|  https://developer.mozilla.org/User:fusionchess
|*|
|*|  This framework is released under the GNU Public License, version 3 or later.
|*|  http://www.gnu.org/licenses/gpl-3.0-standalone.html
|*|
|*|  Syntaxes:
|*|
|*|  * docCookies.setItem(name, value[, end[, path[, domain[, secure]]]])
|*|  * docCookies.getItem(name)
|*|  * docCookies.removeItem(name[, path[, domain]])
|*|  * docCookies.hasItem(name)
|*|  * docCookies.keys()
|*|
\*/

var docCookies = {
    getItem: function (sKey) {
        if (!sKey) { return null; }
        return decodeURIComponent(document.cookie.replace(new RegExp("(?:(?:^|.*;)\\s*" + encodeURIComponent(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=\\s*([^;]*).*$)|^.*$"), "$1")) || null;
    },
    setItem: function (sKey, sValue, vEnd, sPath, sDomain, bSecure) {
        if (!sKey || /^(?:expires|max\-age|path|domain|secure)$/i.test(sKey)) { return false; }
        var sExpires = "";
        if (vEnd) {
            switch (vEnd.constructor) {
                case Number:
                    sExpires = vEnd === Infinity ? "; expires=Fri, 31 Dec 9999 23:59:59 GMT" : "; max-age=" + vEnd;
                    break;
                case String:
                    sExpires = "; expires=" + vEnd;
                    break;
                case Date:
                    sExpires = "; expires=" + vEnd.toUTCString();
                    break;
            }
        }
        document.cookie = encodeURIComponent(sKey) + "=" + encodeURIComponent(sValue) + sExpires + (sDomain ? "; domain=" + sDomain : "") + (sPath ? "; path=" + sPath : "") + (bSecure ? "; secure" : "");
        return true;
    },
    removeItem: function (sKey, sPath, sDomain) {
        if (!this.hasItem(sKey)) { return false; }
        document.cookie = encodeURIComponent(sKey) + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT" + (sDomain ? "; domain=" + sDomain : "") + (sPath ? "; path=" + sPath : "");
        return true;
    },
    hasItem: function (sKey) {
        if (!sKey) { return false; }
        return (new RegExp("(?:^|;\\s*)" + encodeURIComponent(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=")).test(document.cookie);
    },
    keys: function () {
        var aKeys = document.cookie.replace(/((?:^|\s*;)[^\=]+)(?=;|$)|^\s*|\s*(?:\=[^;]*)?(?:\1|$)/g, "").split(/\s*(?:\=[^;]*)?;\s*/);
        for (var nLen = aKeys.length, nIdx = 0; nIdx < nLen; nIdx++) { aKeys[nIdx] = decodeURIComponent(aKeys[nIdx]); }
        return aKeys;
    }
};


/* collapse/expand */
if (docCookies.hasItem("sidebarstate")) {
    $("body").addClass(docCookies.getItem("sidebarstate"));
}

/*global datatable defaults*/
//$.extend(true, $.fn.dataTable.defaults, {
   
//});

/* automatically defaults all submit button to be disabled */
function useSubmitClass() {
    $('form').submit(function () {
        if ($(this).valid()) {
            $(this).find(':submit').attr('disabled', 'disabled');
        }
    });
}

function useDeleteConfirmation() {
    $('.btn-delete').on("click", function (e) {
        e.preventDefault();

        var choice = confirm("Are you sure you want to delete?");

        if (choice) {
            window.location.href = $(this).attr('href');
        }
    });
}
var reminder;
var redirect;
var refeshDiv;
var waitingTime = 1000;
//var msgSession = 'Warning: Within next 3 minutes, if you do not do anything, system will refresh the login credential. Please save changed data.';
var ajaxInProgress = $('#ajaxInProgress'), timer;


$(function () {
    $.ajaxSetup({
        cache: false,
        headers: {
            'X-CSRF-Token': $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        xhrFields: {
            withCredentials: true
        },
        complete: function () {
            clearTimeout(timer);
            $(".panel-overlay").remove();
        },
        error: function (x, e) {
            if (x.status == 0) {
                displayErrorMessage('You are offline!!\n Please Check Your Network.');
            } else if (x.status == 404) {
                displayErrorMessage('Requested URL not found.');
                /*------>*/
            } else if (x.status == 500) {
                displayErrorMessage('Internel Server Error.');
            } else if (e == 'parsererror') {
                displayErrorMessage('Error.\nParsing JSON Request failed.');
            } else if (e == 'timeout') {
                displayErrorMessage('Request Time out.');
            } else {
                displayErrorMessage('Unknown Error.\n' + x.responseText);
            }
            $(".panel-overlay").remove();
        }
    });
});

function displaySuccessMessage(message) {
    var type = 'success';
    displayMessage(message, type, type);
}

function displayErrorMessage(message) {
    var type = 'error';
    displayMessage(message, type, 'Error');
}

function displayWarningMessage(message) {
    var type = 'warning';
    displayMessage(message, type, type);
}
function displayInfoMessage(message) {
    var type = 'info';
    displayMessage(message, type, type);
}
function displayMessage(message, type, title) {
   
   
}

function showDialogWindow(parentDivId, formId, url, title) {
    $.ajax({
        url: url,
        headers: {
            'X-CSRF-Token': $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        type: "GET",
        success: function (data) {
            if (data.message) {
                displayErrorMessage(data.message);
            } else {
                $(".modal-title").text(title);
                $(".modal-body").html(data);
                $("#" + parentDivId).modal("show");
                //hack to get clientside validation working
                //if (formId !== '')
                //    $.validator.unobtrusive.parse("#" + formId);
            }
        }
    });
    return false;
};

function submitModalForm(form, event, refreshElementId) {
    console.log('Person save URL ' + $(form).attr('action'));
    event.preventDefault();
    event.stopImmediatePropagation();
    var model = objectifyForm(form.serializeArray());
    $.ajax({
        url: $(form).attr('action'),
        type: "POST",
        headers: {
            'X-CSRF-Token': $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        data: model,
        success: function (result) {
            if (result.message) {
                $(".close").click();
                $(".btn-filter").click();
                displaySuccessMessage(result.message);

            } else {
                $(".modal-body").html(result);
            }
            refeshDiv = setTimeout(function () {
                $("#" + refreshElementId).click();
            }, waitingTime);
        }
    });

}

function objectifyForm(formArray) {//serialize data function
    var propertyNames = [];
    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        if (propertyNames.indexOf(formArray[i]['name']) === -1) {
            returnArray[formArray[i]['name']] = formArray[i]['value'];
            propertyNames.push(formArray[i]['name']);
        }
    }
    return returnArray;
}

function displayDeleteAlert(message, callbackFunction, inputParam) {
    swal({
        title: "Are you sure?",
        text: message,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel!",
        closeOnConfirm: true,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                callbackFunction(inputParam);
            } else {
                swal("Cancelled", message, "error");
            }
        });
}
