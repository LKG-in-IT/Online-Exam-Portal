

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
            "className": 'details-control',
            "data": null,
            "defaultContent": '', "searchable": false, "autoWidth": true,
            "orderable": false
        },
        {
            "data": "Question", "name": "Question", "autoWidth": true, "searchable": true,
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
                var disableDeleteButton = $('#hfRole').val() === "true" ? "" : "disabled=disabled";
                return "<a href='/Admin/Questions/Delete/" + row.Id + "' "+disableDeleteButton+" class='btn btn-danger deleteCategory' data-id='" + row.Id + "'  >Delete</a>";
            },
            "searchable": false,
            "orderable": false
        }

    ],
    "order": [[2, 'asc']]

});

/******************************************************************************************************************
  *  Nested Row with Question Details--Start
  *****************************************************************************************************************/
// Add event listener for opening and closing details
$('#questionGrid tbody').on('click', 'td.details-control', function () {
    var tr = $(this).closest('tr');
    var row = QuestionsGrid.row(tr);

    if (row.child.isShown()) {
        // This row is already open - close it
        row.child.hide();
        tr.removeClass('shown');
    }
    else {
        // Open this row
        row.child(format(row.data())).show();
        tr.addClass('shown');
    }
});

/* Formatting function for row details - */
function format(data) {
    // `data` is the original data object for the row
    return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
        '<tr>' +
            '<td class="option-items">Option One</td>' +
            '<td class="option-spacer">:</td>' +
            '<td>' + data.OptionA + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="option-items">Option Two</td>' +
            '<td class="option-spacer">:</td>' +
            '<td>' + data.OptionB + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="option-items">Option Three</td>' +
            '<td class="option-spacer">:</td>' +
            '<td>' + data.OptionC + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="option-items">Option Four</td>' +
            '<td class="option-spacer">:</td>' +
            '<td>' + data.OptionD + '</td>' +
        '</tr>' +
        '<tr class="option-answer-tr">' +
            '<td class="option-items-answer">Answer</td>' +
            '<td class="option-spacer">:</td>' +
            '<td class="option-items-answer">' + data.Answer + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td style="padding-top: 10px;"><a class="btn btn-success" href="/Admin/Questions/Edit/' + data.Id + '">Edit Question</a></td>' +
            '<td></td>' +
        '</tr>' +
    '</table>';
}
/******************************************************************************************************************
  *  Nested Row with Question Details--End
  *****************************************************************************************************************/