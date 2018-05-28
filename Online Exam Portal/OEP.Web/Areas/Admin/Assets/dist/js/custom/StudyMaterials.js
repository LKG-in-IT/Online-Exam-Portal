
var examGrid = $("#examGrid").DataTable({

    "processing": true, // for show progress bar
    "serverSide": true, // for process server side
    "filter": true, // this is for disable filter (search box)
    "orderMulti": false, // for disable multiple column at once
    "pageLength": 3,

    "ajax": {
        "url": "/Admin/StudyMaterials/LoadstudyMaterial",
        "type": "POST",
        "datatype": "json"
    },
    "columns": [
        {
            "data": "subCategory.Name", "name": "SubCategory", "autoWidth": true, "searchable": true,
            "orderable": true
        },
        {
            "data": "Name", "name": "File Name", "autoWidth": true, "searchable": true,
            "orderable": true
        },
       
        {
            "data": "Filepath", "name": "Filepath", "autoWidth": true, "searchable": true,
            "orderable": true, render: function (data, type, row) {

                return "<a href='"+data+"' download='' class='btn btn-success editCategory'  >Download</a>";
            }
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
                return "<a href='/Admin/StudyMaterials/Edit/" + row.Id + "' class='btn btn-success editCategory' data-id='" + row.Id + "'  >Edit</a>";
            },
            "searchable": false,
            "orderable": false
        },
        {
            data: null, render: function (data, type, row) {
                return "<a href='/Admin/StudyMaterials/Details/" + row.Id + "' class='btn btn-info detailsCategory' data-id='" + row.Id + "'  >Details</a>";
            },
            "searchable": false,
            "orderable": false
        },
        {
            data: null, render: function (data, type, row) {
                var disableDeleteButton = $('#hfRole').val() === "true" ? "" : "disabled=disabled";
                return "<a href='/Admin/StudyMaterials/Delete/" + row.Id + "' " + disableDeleteButton + "  class='btn btn-danger deleteCategory' data-id='" + row.Id + "'  >Delete</a>";
            },
            "searchable": false,
            "orderable": false
        },
       

    ],
    "order": [[0, 'asc']]

});