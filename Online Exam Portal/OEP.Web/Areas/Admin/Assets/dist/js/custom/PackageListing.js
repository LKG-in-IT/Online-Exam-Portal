
var packagesGrid = $("#packagesGrid").DataTable({

    "processing": true, // for show progress bar
    "serverSide": true, // for process server side
    "filter": true, // this is for disable filter (search box)
    "orderMulti": false, // for disable multiple column at once
    "pageLength": 3,

    "ajax": {
        "url": "/Admin/Packages/LoadPackages",
        "type": "POST",
        "datatype": "json"
    },
    "columns": [
        {
            "data": "Name", "name": "Name", "autoWidth": true, "searchable": true,
            "orderable": true
        },
        {
            "data": "Details", "name": "Details", "autoWidth": true, "searchable": true,
            "orderable": true
        },
        {
            "data": "Prize", "name": "Prize", "autoWidth": true, "searchable": true,
            "orderable": true
        },
        {
            "data": "Duration", "name": "Duration", "autoWidth": true, "searchable": true,
            "orderable": true
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
                return "<a href='/Admin/Packages/Edit/" + row.Id + "' class='btn btn-success editCategory' data-id='" + row.Id + "'  >Edit</a>";
            },
            "searchable": false,
            "orderable": false
        },
        {
            data: null, render: function (data, type, row) {
                return "<a href='/Admin/Packages/Details/" + row.Id + "' class='btn btn-info detailsCategory' data-id='" + row.Id + "'  >Details</a>";
            },
            "searchable": false,
            "orderable": false
        },
        {
            data: null, render: function (data, type, row) {
                return "<a href='/Admin/Packages/Delete/" + row.Id + "' class='btn btn-danger deleteCategory' data-id='" + row.Id + "'  >Delete</a>";
            },
            "searchable": false,
            "orderable": false
        }

    ],
    "order": [[0, 'asc']]

});