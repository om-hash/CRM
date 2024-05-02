jQuery(document).ready(function ($) {

    "use strict";

    /***************************************************************************/
    //MAIN MENU SUB MENU TOGGLE
    /***************************************************************************/
    $('.nav.navbar-nav > li.menu-item-has-children > a').on('click', function (event) {
        event.preventDefault();
        $(this).parent().find('.sub-menu').toggle();
        $(this).parent().find('.sub-menu li .sub-menu').hide();
    });

    $('.nav.navbar-nav li .sub-menu li.menu-item-has-children > a ').on('click', function (event) {
        event.preventDefault();
        $(this).parent().find('.sub-menu').toggle();
    });

    /***************************************************************************/
    //TABS
    /***************************************************************************/
    $(function () {
        $(".tabs").tabs({
            create: function (event, ui) {
                $(this).fadeIn();
            }
        });
    });

    /***************************************************************************/
    //ACTIVATE CHOSEN 
    /***************************************************************************/
    //$("select").chosen({disable_search_threshold: 11});

    /***************************************************************************/
    //ACCORDIONS
    /***************************************************************************/
    $(function () {
        $("#accordion").accordion({
            heightStyle: "content",
            closedSign: '<i class="fa fa-minus"></i>',
            openedSign: '<i class="fa fa-plus"></i>'
        });
    });

    /***************************************************************************/
    //SLICK SLIDER - SIMPLE SLIDER
    /***************************************************************************/
    $('.slider.slider-simple').slick({
        prevArrow: $('.slider-nav-simple-slider .slider-prev'),
        nextArrow: $('.slider-nav-simple-slider .slider-next'),
        adaptiveHeight: true,
        fade: true
    });


    /***************************************************************************/
    //SLICK SLIDER - FEATURED PROPERTIES
    /***************************************************************************/
    $('.slider.slider-featured').slick({
        prevArrow: $('.slider-nav-properties-featured .slider-prev'),
        nextArrow: $('.slider-nav-properties-featured .slider-next'),
        slidesToShow: 4,
        slidesToScroll: 1,
        responsive: [
            {
                breakpoint: 990,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 589,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });

    /***************************************************************************/
    //SLICK SLIDER - TESTIMONIALS 
    /***************************************************************************/
    $('.slider.slider-testimonials').slick({
        prevArrow: $('.slider-nav-testimonials .slider-prev'),
        nextArrow: $('.slider-nav-testimonials .slider-next'),
        adaptiveHeight: true
    });

    $('.slider.slider-advertisements').slick({
        prevArrow: $('.slider-nav-advertisements .slider-prev'),
        nextArrow: $('.slider-nav-advertisements .slider-next'),
        adaptiveHeight: true,
        autoplay: true,
        pauseOnHover:true,
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1
        
    });
  

    /***************************************************************************/
    //SLICK SLIDER - PROPERTY GALLERY 
    /***************************************************************************/
    $('.slider.slider-property-gallery.ltr').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        adaptiveHeight: true,
        arrows: false,
        fade: true,
        infinite: true,
        asNavFor: '.property-gallery-pager'
    });

    $('.slider.slider-property-gallery.rtl').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        adaptiveHeight: true,
        arrows: false,
        fade: true,
        rtl: true,
        infinite: true,
        asNavFor: '.property-gallery-pager'
    });

    $('.property-gallery-pager').on('init', function (event, slick) {
        $('.slide-counter').text('1 / ' + slick.slideCount);
    });

    $('.property-gallery-pager.ltr').slick({
        prevArrow: $('.slider-nav-property-gallery .slider-prev'),
        nextArrow: $('.slider-nav-property-gallery .slider-next'),
        slidesToShow: 5,
        autoplay:true,
        slidesToScroll: 1,
        asNavFor: '.slider.slider-property-gallery',
        dots: false,
        focusOnSelect: true,
        infinite: true,
    });

    $('.property-gallery-pager.rtl').slick({
        prevArrow: $('.slider-nav-property-gallery .slider-prev'),
        nextArrow: $('.slider-nav-property-gallery .slider-next'),
        slidesToShow: 5,
        autoplay: true,
        slidesToScroll: 1,
        asNavFor: '.slider.slider-property-gallery',
        dots: false,
        focusOnSelect: true,
        infinite: true,
        rtl:true,
    });

    $('.property-gallery-pager').on('afterChange', function (event, slick, currentSlide, nextSlide) {
        currentSlide = currentSlide + 1;
        var counter = currentSlide + ' / ' + slick.slideCount;
        $('.slide-counter').text(counter);
    });

    //INITIATE SLIDES
    $('.slide').addClass('initialized');

    /***************************************************************************/
    //FIXED HEADER
    /***************************************************************************/
    var navToggle = $('.header-default .navbar-toggle');
    var mainMenuWrap = $('.header-default .main-menu-wrap');

    if ($(window).scrollTop() > 140) {
        navToggle.addClass('fixed');
        mainMenuWrap.addClass('fixed');
    }


    $(window).bind('scroll', function () {
        if ($(window).scrollTop() > 140) {
            navToggle.addClass('fixed');
            mainMenuWrap.addClass('fixed');
        } else {
            navToggle.removeClass('fixed');
            mainMenuWrap.removeClass('fixed');
        }
    });


    /***************************************************************************/
    //INITIALIZE BLOG CREATIVE
    /***************************************************************************/
    $('.grid-blog').isotope({
        itemSelector: '.col-lg-4'
    });

    
    /***************************************************************************/
    //FILTER TOGGLE (ON GOOGLE MAPS)
    /***************************************************************************/
    $('.filter-toggle').on('click', function () {
        $(this).parent().find('form').stop(true, true).slideToggle();
    });

   

    /******************************************************************************/
    /** SUBMIT PROPERTY - ADDITIONAL IMAGES  **/
    /******************************************************************************/
    var files_count = $('.additional-img-container .additional-image').length + 1;
    $('.add-additional-img').on('click', function () {
        files_count++;
        $('.additional-img-container').append('<table><tr><td><div class="media-uploader-additional-img"><input type="file" class="additional_img" name="additional_img' + files_count + '" value="" /><span class="delete-additional-img appended right"><i class="fa fa-trash"></i> Delete</span></div></td></tr></table>');
    });

    $('.additional-img-container').on("click", ".delete-additional-img", function () {
        $(this).parent().parent().parent().parent().parent().remove();
    });

    /******************************************************************************/
    /** SUBMIT PROPERTY - OWNER INFO **/
    /******************************************************************************/
    $('#owner-info input[type="radio"]').on('click', function () {
        var input = $(this).val();
        $('#owner-info .form-block-agent-options').hide();
        if (input === 'agent') {
            $('#owner-info .form-block-select-agent').slideDown('fast');
        }
        if (input === 'custom') {
            $('#owner-info .form-block-custom-agent').slideDown('fast');
        }
    });

    /***************************************************************************/
    //AJAX CONTACT FORM
    /***************************************************************************/
    //--><![CDATA[//><!--
    $(document).on('submit', 'form#contact-us', function () {
        $('form#contact-us .error').remove();
        var hasError = false;
        $('.requiredField').each(function () {
            if ($.trim($(this).val()) === '') {
                $(this).parent().find('label').append('<span class="error">This field is required!</span>');
                $(this).addClass('inputError');
                hasError = true;
            } else if ($(this).hasClass('email')) {
                var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
                if (!emailReg.test($.trim($(this).val()))) {
                    $(this).parent().find('label').append('<span class="error">Sorry! You\'ve entered an invalid email.</span>');
                    $(this).addClass('inputError');
                    hasError = true;
                }
            }
        });
        if (!hasError) {
            var formInput = $(this).serialize();
            $.post($(this).attr('action'), formInput, function (data) {
                $('form#contact-us').slideUp("fast", function () {
                    $(this).before('<p class="alert-box success"><i class="fa fa-check icon"></i><strong>Thanks!</strong> Your email has been delivered!</p>');
                });
            });
        }

        return false;
    });
    //-->!]]>

});


// Added by Khal

//-------------------------------------------------------------
function ErrorDialog(title, msg) {
    Swal.fire({
        icon: 'error',
        title: title,
        text: msg,
        showConfirmButton: true,
        showCloseButton: true
    })
}

function SuccessDialog(title, msg) {
    Swal.fire({
        icon: 'success',
        title: title,
        text: msg,
        showConfirmButton: true,
        showCloseButton: true
    })
}