

var exmaQuestionsGrid = $("#exmaQuestionsGrid").DataTable({

    "processing": true, // for show progress bar
    "serverSide": true, // for process server side
    "filter": true, // this is for disable filter (search box)
    "orderMulti": false, // for disable multiple column at once
    "pageLength": 3,

    "ajax": {
        "url": "/Admin/ExamQuestions/LoadQuestions?id=" + $('#exmaId').val(),
        "type": "POST",
        "datatype": "json"
    },
    "columns": [
           {
               "className": 'details-control',
               "data": null,
               "defaultContent": '', "searchable": false,
               "orderable": false
           },
          {
              "data": "Questions.Question", "name": "Question", "autoWidth": true, "searchable": true,
              "orderable": true
          },
           {
               data: null, render: function (data, type, row) {
                   return "<a href='#' class='btn btn-danger deleteQuestion' data-id='" + row.Id + "'  >Remove</a>";
               },
               "searchable": false,
               "orderable": false
           }

    ],
    "order": [[1, 'asc']] // to remove order icon from first coloumn(0th column)

});

/******************************************************************************************************************
  *  Nested Row with Question Details--Start
  *****************************************************************************************************************/
// Add event listener for opening and closing details
$('#exmaQuestionsGrid tbody').on('click', 'td.details-control', function () {
    var tr = $(this).closest('tr');
    var row = exmaQuestionsGrid.row(tr);

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
            '<td>' + data.Questions.OptionA + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="option-items">Option Two</td>' +
            '<td class="option-spacer">:</td>' +
            '<td>' + data.Questions.OptionB + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="option-items">Option Three</td>' +
            '<td class="option-spacer">:</td>' +
            '<td>' + data.Questions.OptionC + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="option-items">Option Four</td>' +
            '<td class="option-spacer">:</td>' +
            '<td>' + data.Questions.OptionD + '</td>' +
        '</tr>' +
        '<tr class="option-answer-tr">' +
            '<td class="option-items-answer">Answer</td>' +
            '<td class="option-spacer">:</td>' +
            '<td class="option-items-answer">' + data.Questions.Answer + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td style="padding-top: 10px;"><a class="btn btn-success" href="/Admin/Questions/Edit/' + data.Questions.Id + '">Edit Question</a></td>' +
            '<td></td>' +
        '</tr>' +
    '</table>';
}
/******************************************************************************************************************
  *  Nested Row with Question Details--End
  *****************************************************************************************************************/

$(document).ready(function () {

    var selectedItem = {};
    /**/
    var options = {
        url: function (phrase) {
            return "/Admin/ExamQuestions/GetQuestions?phrase=" + phrase + "";
        },

        getValue: "Question",
        placeholder: "Type here to search questions and Use Add button to add it to list",

        list: {
            match: {
                enabled: false
            },
            maxNumberOfElements: 10,
            showAnimation: {
                type: "fade", //normal|slide|fade
                time: 400,
                callback: function () { }
            },

            hideAnimation: {
                type: "slide", //normal|slide|fade
                time: 400,
                callback: function () { }
            },
            onClickEvent: function () {
                var index = $("#txtquestions").getSelectedItemIndex();
                selectedItem = $("#txtquestions").getItemData(index);
                if (selectedItem !== undefined && selectedItem.Id > 0) {
                    $('#add-item').css('display', 'inline-block');
                    $('#clear-input').css('display', 'inline-block');
                }
                selectedItem = undefined;
                $("#txtquestions").val('');
            }
        },

        theme: "square"
    };

    $("#txtquestions").easyAutocomplete(options);

    /**/

    $('#clear-input').click(function () {
        $("#txtquestions").val('');
    });

    $('#add-item').click(function () {
        var selectedText = $("#txtquestions").val();
        if (selectedText !== undefined && selectedText !== "") {
            var examQuestionResource = {};
            examQuestionResource.ExamId = $('#exmaId').val();
            examQuestionResource.QuestionId = selectedItem.Id;
            $.post("/Admin/ExamQuestions/AddQuestions", examQuestionResource, function (data) {
                var responseObj = JSON.parse(data);
                if (responseObj.Status === "Success") {
                    var oTable = $('#exmaQuestionsGrid').DataTable();
                    oTable.draw();
                } else if (responseObj.Status === "Exist") {
                    alert(responseObj.Message);
                }

            });
            console.log(selectedItem);
            $("#txtquestions").val('');
            $('#add-item').css('display', 'none');
            $('#clear-input').css('display', 'none');
        }
        //
    });


    $(document).on('click', '.deleteQuestion', function (e) {

        e.preventDefault();
        var questionId = $(this).attr("data-id");
        $.post("/Admin/ExamQuestions/DeleteQuestion/" + questionId, function (response) {
            var responseObj = JSON.parse(response);
            if (responseObj.Status === "Success") {
                var oTable = $('#exmaQuestionsGrid').DataTable();
                oTable.draw();
            } else if (responseObj.Status === "NotExist") {
                alert(responseObj.Message);
            }

        });
    });

});