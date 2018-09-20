$(document).ready(function () {

    var $wrapper = $('.tab-wrapper'),
        $allTabs = $wrapper.find('.tab-content'),
        $activeTabcontent = $wrapper.find('.tab-content.active'),
        $tabMenu = $wrapper.find('.tab-menu li.tab-header'),
        $line = $('<div class="line"></div>').appendTo($tabMenu),
        $nextButton = $('#next'),
        $prevButton = $('#previous'),
        currentQuestion = 0,
        totalQuestions = $('#questionCount').val(),
        $finishButton = $('#finishExam'),
        $options = $('.options'),
        Answers = [],
        examFinished = false;

    $options.on('click', function () {
        var $optionsContainer = $(this).closest($('.options-container'));
        var questionId = $optionsContainer.attr('data-question-id');
        var selectedOptionNumber = $(this).attr('data-option');
        $optionsContainer.find('.options').removeClass('active');
        $(this).addClass('active');
        var found = false;
        for (var i = 0; i < Answers.length; i++) {
            if (Answers[i].QuestionId == questionId) {
                Answers[i].Answer = selectedOptionNumber;
                found = true;
                break;
            }
        }
        if (!found) {
            Answers.push({ "QuestionId": questionId, "Answer": selectedOptionNumber });
        }

        if (currentQuestion === totalQuestions - 1) {
            if (Answers.length > 0) {
                $finishButton.show();
            }
            $nextButton.hide();
        }
    });

    $nextButton.on('click', function () {
        currentQuestion++;
        var $getWrapper = $(this).closest($wrapper);
        $activeTabcontent = $('.tab-content.active');
        var $activeTabDivs = $('.tab-content.active > div'),
            $dataTabContent = $getWrapper.find($allTabs).filter('[data-tab-content=' + currentQuestion + ']');

        $activeTabcontent.hide().removeClass('active');

        $dataTabContent.addClass('active').show();

        //hide / show next and prev buttons
        if (currentQuestion > 0) {
            $prevButton.show();
        }
        if (currentQuestion === totalQuestions - 1) {
            if (Answers.length > 0) {
                $finishButton.show();
            }
            $nextButton.hide();
        }

        //set tab to 0
        $tabMenu.removeClass('active');
        $getWrapper.find('.line').width(0);
        $getWrapper.find('[data-tab-head=0]').addClass('active').find($line).animate({ 'width': '100%' }, 'fast');
        $getWrapper.find($activeTabDivs).hide();
        $getWrapper.find($activeTabDivs).filter('[data-tab-sub=' + 0 + ']').show();
        return false;
    });
    $prevButton.on('click', function () {
        currentQuestion--;
        var $getWrapper = $(this).closest($wrapper);
        $activeTabcontent = $('.tab-content.active');
        var $activeTabDivs = $('.tab-content.active > div'),
            $dataTabContent = $getWrapper.find($allTabs).filter('[data-tab-content=' + currentQuestion + ']');

        $activeTabcontent.hide().removeClass('active');
        $dataTabContent.addClass('active');
        $dataTabContent.show();
        if (currentQuestion == 0) {
            $prevButton.hide();
        }
        if (currentQuestion < totalQuestions - 1) {
            $nextButton.show();
            $finishButton.hide();
        }
        //set tab to 0
        $tabMenu.removeClass('active');
        $getWrapper.find('.line').width(0);
        $getWrapper.find('[data-tab-head=0]').addClass('active').find($line).animate({ 'width': '100%' }, 'fast');

        $getWrapper.find($activeTabDivs).hide();
        $getWrapper.find($activeTabDivs).filter('[data-tab-sub=' + 0 + ']').show();
        return false;
    });
    $finishButton.on('click', function (e) {
        e.preventDefault();
        if (Answers.length > 0) {
            data = { ExamAnswersResourceList: Answers, ExamId: $('#ExamId').val() }
            $finishButton.hide(); $nextButton.hide(); $prevButton.hide();
            $('#img-load').show();
            $.ajax({
                type: "POST",
                url: "/Exam/StartExam",
                data: JSON.stringify(data),
                success: function (data) {
                    $('#img-load').hide();
                    if (data.success) {
                        examFinished = true;
                        $('#result').html(data.result);
                    }
                },
                dataType: 'json',
                contentType: 'application/json'
            });
        } 
    });

    
    $('#result').on('click', $tabMenu, function (event) {
        var _this = $(event.target);
        if (!_this.hasClass('tab-header')) {
            return false;
        }
        if (!examFinished) {            
            var dataTab = _this.data('tab-head'),
                $getWrapper = _this.closest($wrapper);

            $getWrapper.find($tabMenu).removeClass('active');
            _this.addClass('active');

            $getWrapper.find('.line').width(0);
            _this.find($line).animate({ 'width': '100%' }, 'fast');
            $getWrapper.find($('.tab-content.active > div')).hide();
            $getWrapper.find($('.tab-content.active > div')).filter('[data-tab-sub=' + dataTab + ']').show();
        } else {            
            var _this = $(event.target);
            var dataTab = _this.data('tab-head'),
                $getWrapper = $('.tab-wrapper');

            $getWrapper.find($tabMenu).removeClass('active');
            _this.addClass('active');

            
            $getWrapper.find('.line').width(0);
            $('<div class="line"></div>').appendTo(_this);
            _this.find('.line').animate({ 'width': '100%' }, 'fast');
            $getWrapper.find($('.tab-content > div')).hide();
            $getWrapper.find($('.tab-content > div')).filter('[data-tab-sub=' + dataTab + ']').show();
        }
    });

});//end ready