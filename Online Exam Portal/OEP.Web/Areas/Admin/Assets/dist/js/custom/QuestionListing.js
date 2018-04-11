

var QuestionsGrid = $("#questionGrid").DataTable({

    "processing": true, // for show progress bar
    "serverSide": true, // for process server side
    "filter": true, // this is for disable filter (search box)
    "orderMulti": false, // for disable multiple column at once
    "pageLength": 3,

    "ajax": {
        "url": "/Admin/Questions/LoadQuestions",
        "type": "POST",
        "datatype": "json"
    },
    "columns": [
        {
            "data": "QuestionType.Name", "name": "QuestionType", "autoWidth": true, "searchable": true,
            "orderable": true
        },
        {
            "data": "Question", "name": "Question", "autoWidth": true, "searchable": true,
            "orderable": true
        },
        {
            "data": "OptionA", "name": "OptionA", "autoWidth": true, "searchable": true,
            "orderable": false
        },
        {
            "data": "OptionB", "name": "OptionB", "autoWidth": true, "searchable": true,
            "orderable": false
        },
        {
            "data": "OptionC", "name": "OptionC", "autoWidth": true, "searchable": true,
            "orderable": false
        },
        {
            "data": "OptionD", "name": "OptionD", "autoWidth": false, "searchable": true,
            "orderable": false
        },
        {
            "data": "Answer", "name": "Answer", "autoWidth": false, "searchable": false,
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
                return "<a href='/Admin/Questions/Edit/" + row.Id + "' class='btn btn-success editCategory' data-id='" + row.Id + "'  >Edit</a>";
            },
            "searchable": false,
            "orderable": false
        },
        {
            data: null, render: function (data, type, row) {
                return "<a href='/Admin/Questions/Details/" + row.Id + "' class='btn btn-info detailsCategory' data-id='" + row.Id + "'  >Details</a>";
            },
            "searchable": false,
            "orderable": false
        },
        {
            data: null, render: function (data, type, row) {
                return "<a href='/Admin/Questions/Delete/" + row.Id + "' class='btn btn-danger deleteCategory' data-id='" + row.Id + "'  >Delete</a>";
            },
            "searchable": false,
            "orderable": false
        }

    ],
    "order": [[1, 'asc']]

});