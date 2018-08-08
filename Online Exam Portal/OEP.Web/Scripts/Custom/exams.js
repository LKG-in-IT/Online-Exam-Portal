
$(document).ready(function () {

    var pageSize = 3;
    var skip = 0;
    var total = 0;
    var status = false;
    var $listItemsGlobal = '';


    $("#Category").change(function () {

        var categoryId = $("#Category").val();
        $.post("/Exam/SubCategory/", { categoryId: categoryId }, function (data) {

            $("#SubCategory").empty();
            $("#SubCategory").append("<option value='0'>Sub Category</option>");
            for (var i = 0; i < data.length; i++) {
                $("#SubCategory").append("<option value=" + data[i].Id + ">" + data[i].Name + "</option>");
            }
        });
    });



    $("#search").click(function () {
        skip = 0;

        $("#examlist").empty();
        $("#btnloadmore").remove();
        loadExam();
        $listItemsGlobal = '';
        return false;
    });

    $('#loadmoreDiv').on('click', '#btnloadmore', function () {

        skip += pageSize;

        $("#btnloadmore").remove();
        loadExam();
        return false;
    });
    $('#loading').show();
    setTimeout(function () {
        loadExam();
    }, 1000);



    function loadExam() {
        var subCatoryId = $("#SubCategory").val();
        var categoryId = $("#Category").val();
        var ExamTypeId = $("#ExamType").val();
        var KeyWord = $("#txtsearch").val();
        var ExamResource = {};
        ExamResource.ExamTypeId = ExamTypeId;
        ExamResource.SubcategoryId = subCatoryId;
        ExamResource.CategoryId = categoryId;
        ExamResource.KeyWord = KeyWord;
        ExamResource.skip = skip;
        ExamResource.pageSize = pageSize;
        $('#loading').show();
        $.post("/Exam/SearchExam/", ExamResource, function (data) {
            var $listItems = '';
            for (var i = 0; i < data.exam.length; i++) {

                if (i == 0 || i % 3 == 0) {
                    $listItems += "<div class='row mt-4'>";
                }

                $listItems = $listItems + "<div class='col-md-4 col-sm-4 col-lg-4'>" +
                    " <div class='background-white pb-4 h-100 radius-secondary' style='padding-top: 15px;'>   " +
                    "    <div class='px-4'>" +
                    "     <div class='overflow-hidden'>" +
                    "        <h5 >" + data.exam[i].Name + "</h5>" +
                    "    </div>" +
                    "    <div class='overflow-hidden'>" +
                    "       <h6 class='fw-400 color-7' style='text-align: left;'>" + (data.exam[i].Description != null ? data.exam[i].Description : '') + "</h6>" +
                    "   </div>" +
                    "    <div class='overflow-hidden'>" +
                    "       <h6 class=' mb-0'>" + data.exam[i].SubCategory.Name + "</h6>" +
                    "      <p class='py-3 mb-0'>" + data.exam[i].ExamType.Name + "</p>" +
                    "   </div>" +

                    "   <div class='overflow-hidden'>" +
                    "      <a  class='btn btn-outline-primary btn-xs' href='/Exam/ViewExam?ExamId=" + data.exam[i].Id + "' class='btn btn-info pt-1'>View Exam</a>" +
                    "  </div>" +
                    "  </div>" +
                    " </div>" +
                    "  </div>";

                if (i == data.exam.length - 1 || (i != 0 && i % 2 == 0)) {
                    $listItems = $listItems + "</div>"
                }

            }
            $('#loading').hide();

            if (data.exam.length > 0) {
                if (!status) {
                    $(".grid").html($listItems);
                    status = true;
                } else {
                    $(".grid").append($listItems);
                }
            } else {
                $(".grid").html('<p style="padding: 35px 0 0 0;">No Results Found</p>');
            }


            if (data.totalItem >= pageSize) {

                $("#loadmoreDiv").append(" <input type='button' id='btnloadmore' value='ShowMore' class='btn-primary btn  ml-0' />");
            }
            else {

                $("#btnloadmore").remove();
            }
        });



    }
});
