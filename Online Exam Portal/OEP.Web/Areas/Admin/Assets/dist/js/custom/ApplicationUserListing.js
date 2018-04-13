
var userGrid = $("#userGrid").DataTable({

    "processing": true, // for show progress bar
    "serverSide": true, // for process server side
    "filter": true, // this is for disable filter (search box)
    "orderMulti": false, // for disable multiple column at once
    "pageLength": 3,

    "ajax": {
        "url": "/Admin/ApplicationUsers/LoadUsers",
        "type": "POST",
        "datatype": "json"
    },
    "columns": [
        {
            "data": "Name", "name": "Name", "autoWidth": true, "searchable": true,
            "orderable": true
        },
        {
            "data": "Email", "name": "Email", "autoWidth": true, "searchable": true,
            "orderable": false
        },
        {
            "data": "PhoneNumber", "name": "PhoneNumber", "autoWidth": true, "searchable": true,
            "orderable": false
        },
        {
            "data": "UserName", "name": "UserName", "autoWidth": true, "searchable": true,
            "orderable": true
        },
        {
            "data": "Role", "name": "Role", "autoWidth": true, "searchable": true,
            "orderable": false
        },
        {
            "data": "Status", "name": "Status", "autoWidth": false, "searchable": false,
            "orderable": false,
            render: function (data, type, row) {
                if (type === 'display') {
                    var status = data ? 'checked' : '';
                    return "<input type='checkbox' " + status + " disabled  class='editor-active'>";
                }
                return data;
            },
            className: "dt-body-center"
        },
        {
            data: null, render: function (data, type, row) {
                return "<a href='/Admin/ApplicationUsers/Edit?UserName=" + data["UserName"] + "' class='btn btn-success editCategory' data-id='" + row.Id + "'  >Edit</a>";
            },
            "searchable": false,
            "orderable": false
        },
        {
            data: null, render: function (data, type, row) {
                return "<a href='/Admin/ApplicationUsers/Details?UserName=" + data["UserName"]+ "' class='btn btn-info detailsCategory' data-id='" + row.Id + "'  >Details</a>";
            },
            "searchable": false,
            "orderable": false
        },
        {
            data: null, render: function (data, type, row) {

              
                if (data["Status"] === false) {
                    return "<a href='/Admin/ApplicationUsers/Disable?UserName=" + data["UserName"] + "' class='btn btn-danger deleteCategory' data-id='" + row.Id + "'  >Enable</a>";

                }
                else {
                    return "<a href='/Admin/ApplicationUsers/Disable?UserName=" + data["UserName"] + "' class='btn btn-danger deleteCategory' data-id='" + row.Id + "'  >Disable</a>";

                }
            },
            "searchable": false,
            "orderable": false
        }

    ],
    "order": [[0, 'asc']]

});