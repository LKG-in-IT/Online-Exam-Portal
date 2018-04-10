$(document).ready(function () {

    /*-------------------------------------------
   * Remove image using ajax. It will remove the current user's profile picture
   -------------------------------------------*/
    $(document).on('click','#removeImage',function() {
        $.post("/Manage/DeleteUserProfilePicture", function (data) {
            var responseObj = JSON.parse(data);
            if (responseObj.Status === "Success") {
                $("#profil-picture").attr('src', "/Uploads/female-user-avatar.png");
                $("#removeImage").css("display", "none");
            } else if (responseObj.Status === "Error") {
                alert(responseObj.Message);
            }

        });
    });

    /**
      * ************************** Show image on file input change
      * @param {} input  -- file input
      * @returns {} 
      */
    function readUrl(input) {

        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#profil-picture').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }

    $(document).on('change','.file-upload-image',function() {
        readUrl(this);
        $("#removeImage").css("display", "none");
    });

    //******************************************************end input file change


    setTimeout(function() {
        $('.text-success').css('display', 'none');
    }, 3000);
});