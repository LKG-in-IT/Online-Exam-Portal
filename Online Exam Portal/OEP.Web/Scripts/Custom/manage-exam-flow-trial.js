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
        examFinished = false,
        interval = false;

    startTimer();

    /**
     * Timer will be start on page load
     */
    function startTimer() {
        var duration = $('#duration').val();
        var timer = parseInt(duration * 60), minutes, seconds;
        interval = setInterval(function () {
            minutes = parseInt(timer / 60, 10)
            seconds = parseInt(timer % 60, 10);

            minutes = minutes < 10 ? "0" + minutes : minutes;
            seconds = seconds < 10 ? "0" + seconds : seconds;

            $("#hdntime").val(minutes);
            $("#timer").html(minutes + ":" + seconds);

            if (timer <= 10 && !$("#timer").hasClass('.timer-danger')) {
                $("#timer").addClass('timer-danger');
                $("#timer").removeClass('timer-start');
            }

            if (--timer < 0) {
                clearInterval(interval);
                $("#timer").html('');
                $("#timer").html("Time Expired");
                $finishButton.remove(); $nextButton.remove(); $prevButton.remove();
                sendDatatoServer();
            }
        }, 1000);
    }

    /*
     * Options -- multiple choices click event
     */
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
        sendDatatoServer();
    });

    /**
     * Send data to server if user click finish button of if the timout expired. It won't send data if user doesn't answer any questions.
     */
    function sendDatatoServer() {
        if (Answers.length > 0) {
            data = { ExamAnswersResourceList: Answers, QuestionType: $('#QuestionType').val() }
            $finishButton.hide(); $nextButton.hide(); $prevButton.hide();
            $('#img-load').show();
            $.ajax({
                type: "POST",
                url: "/Trial/SubmitAnswers",
                data: JSON.stringify(data),
                success: function (data) {
                    $('#img-load').hide();
                    if (data.success) {
                        examFinished = true;
                        $('#result').html(data.result);
                        clearInterval(interval);
                        $("#timer").html('');
                        $("#timer").removeClass('timer-danger');
                        $("#timer").addClass('timer-start');
                        $("#timer").html("You have successfully completed the session.");
                        setTimeout(function () { $("#timer").html(''); }, 2000);

                        showChart();
                    }
                },
                dataType: 'json',
                contentType: 'application/json'
            });
        }
    }

    /*The tab container will load initially and it will recreate after the exam. So, dynamic tab click won't work.
      * we need to handle both in a single click event. we are using a flag to handle this  */
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


    //Show result in chart after user completed the exam
    function showChart() {
        var canvas = document.getElementById("cnResultChart");
        var ctx = canvas.getContext('2d');       

        // Global Options:
        Chart.defaults.global.defaultFontColor = 'black';
        Chart.defaults.global.defaultFontSize = 16;

        var data = {
            labels: ["Correct Answer", "InCorrect Answer"],
            datasets: [
                {
                    fill: true,
                    backgroundColor: [
                        "#427b01",
                        "#ff1100"],
                    data: [$('#TotalCorrectAnswered').val(), $('#TotalInCorrectAnswers').val()],
                    // Notice the borderColor 
                    borderColor: ['black', 'black'],
                    borderWidth: [1,1 ]
                }
            ]
        };

        // Notice the rotation from the documentation.

        var options = {
            title: {
                display: true,
                text: 'Report',
                position: 'top'
            },
            rotation: -0.7 * Math.PI,
            responsive: false,
        };      

        // Chart declaration:
        var myBarChart = new Chart(ctx, {
            type: 'pie',
            data: data,
            options: options
        });
    }

});//end ready