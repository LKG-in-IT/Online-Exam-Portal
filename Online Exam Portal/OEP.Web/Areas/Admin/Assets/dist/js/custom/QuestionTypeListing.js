
var questionTypeGrid = $("#questionTypeGrid").DataTable({

    "processing": true, // for show progress bar
    "serverSide": true, // for process server side
    "filter": true, // this is for disable filter (search box)
    "orderMulti": false, // for disable multiple column at once
    "pageLength": 3,

    "ajax": {
        "url": "/Admin/QuestionType/LoadQuestionTypes",
        "type": "POST",
        "datatype": "json"
    },
    "columns": [
          {
              "data": "Name", "name": "Name", "autoWidth": true, "searchable": true,
              "orderable": true
          },
           {
               "data": "Status", "name": "Status", "autoWidth": false, "searchable": false,
               "orderable": false,
               render: function (data, type, row) {
                   if (type === 'display') {
                       var status = data ? 'checked' : '';
                       return "<input type='checkbox' "+ status+" disabled  class='editor-active'>";
                   }
                   return data;
               },
               className: "dt-body-center"
           },
           {
               data: null, render: function (data, type, row) {
                   return "<a href='/Admin/QuestionType/Edit/" + row.Id + "' class='btn btn-success editQuestionType' data-id='" + row.Id + "'  >Edit</a>";
               },
               "searchable": false,
               "orderable": false
           },
           {
               data: null, render: function (data, type, row) {
                   return "<a href='/Admin/QuestionType/Details/" + row.Id + "' class='btn btn-info detailsQuestionType' data-id='" + row.Id + "'  >Details</a>";
               },
               "searchable": false,
               "orderable": false
           },
           {
               data: null, render: function (data, type, row) {
                   var disableDeleteButton = $('#hfRole').val() === "true" ? "" : "disabled=disabled";
                   return "<a href='/Admin/QuestionType/Delete/" + row.Id + "' " + disableDeleteButton + " class='btn btn-danger deleteQuestionType' data-id='" + row.Id + "'  >Delete</a>";
               },
               "searchable": false,
               "orderable": false
           }

    ],
    "order": [[0, 'asc']] 

});