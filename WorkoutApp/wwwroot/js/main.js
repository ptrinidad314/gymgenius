
$(document).ready(() => {

    
    var slideIndex = 0;
    //showSlides();

    function showSlides() {
        var i;
        var slides = $('.slides');

        for (i = 0; i < slides.length; i++) {
            slides[i].style.display = "none";
        }

        slideIndex++;

        if (slideIndex > slides.length) {
            slideIndex = 1;
        }

        slides[slideIndex - 1].style.display = "block";
        
        setTimeout(showSlides, 2500);

    }

    $('#btnAddExercise').on('click', () => {
        $('#exercisesToAddList').append('<li><input type="text" value="" list="exerciseList" name="exerciseNames" placeholder="Add an exercise..." /></li>');
    });

    $('#getMoving').on('click', () => {
        $('#getMoving').addClass('animated bounceOutRight');
        setTimeout(function () {
                window.location = '#section-a';
                showSlides();
            }, 1000);
    });
   
});