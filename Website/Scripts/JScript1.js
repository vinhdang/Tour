//ABOUT PAGE FUNCTION// 
function AboutPage() { //CHANGE LINK// ResizeWindows();
$('#new-message').attr('href', '#message'); $('#message').attr('href', '#message');$('#new-vission').attr('href', '#vission'); $('#vission').attr('href', '#vission');$('#new-history').attr('href', '#history'); $('#history').attr('href', '#history');$('#new-scope').attr('href', '#scope'); $('#scope').attr('href', '#scope');$('#new-chart').attr('href', '#chart'); $('#chart').attr('href', '#chart');$('#new-operations').attr('href', '#operations'); $('#operations').attr('href', '#operations');$('#new-results').attr('href', '#results'); $('#results').attr('href', '#results');$('#new-partners').attr('href', '#partners'); $('#partners').attr('href', '#partners');$('#new-social').attr('href', '#social'); $('#social').attr('href', '#social');$('#new-awards').attr('href', '#awards'); $('#awards').attr('href', '#awards');$('#new-video').attr('href', '#video'); $('#video').attr('href', '#video');$('#new-trading').attr('href', '#trading'); $('#trading').attr('href', '#trading');$('#new-finish').attr('href', '#finish'); $('#finish').attr('href', '#finish');
if (window.location.hash == '' || window.location.hash == undefined) { window.location.hash=
'message'; }
 var ContentChange = {

        //MESSAGE//  
        message: function () {
            $('#container').append('<div class="loadx" style="display:block"></div>');
            setTimeout('api.goTo(1)', 100);
            var loadUrl = $('a.off-message').attr("href");
            window.location.hash = 'message';
            //AJAX OPEN// 
            $.ajax({ url: loadUrl, cache: false, success: function (data) {
                $('.item-page').append(data);
                ScrollNiceHide(); ResizeWindows(); $('.content-page').delay(1000).fadeIn(500, 'linear', function () {
                    $(".loadx").fadeOut(500, "linear", function () {
                        $('.loadx').remove();
                        setTimeout(ScrollNiceA, 300); $('.block-nav').css({ 'display': 'none', 'width': 0, 'z-index': -9999 });
                    });
                });
            }
            });
            //AJAX CLOSE// 
        },

        //VISSION//  
        vission: function () {
            $('#container').append('<div class="loadx" style="display:block"></div>');
            setTimeout('api.goTo(2)', 100);
            var loadUrl = $('a.off-vission').attr("href");
            window.location.hash = 'vission';
            //AJAX OPEN// 
            $.ajax({ url: loadUrl, cache: false, success: function (data) {
                $('.item-page').append(data);
                ScrollNiceHide(); ResizeWindows(); $('.content-page').delay(1000).fadeIn(500, 'linear', function () {
                    $(".loadx").fadeOut(500, "linear", function () {
                        $('.loadx').remove();
                        setTimeout(ScrollNiceB, 300); $('.block-nav').css({ 'display': 'none', 'width': 0, 'z-index': -9999 });
                    });
                });
            }
            });

            //AJAX CLOSE// 

        },

        //HISTORY//  
        history: function () {
            $('#container').append('<div class="loadx" style="display:block"></div>');
            setTimeout('api.goTo(3)', 100);
            var loadUrl = $('a.off-history').attr("href");
            window.location.hash = 'history';
            //AJAX OPEN// 
            $.ajax({ url: loadUrl, cache: false, success: function (data) {
                $('.item-page').append(data);
                ScrollNiceHide(); ResizeWindows();
                var allItem = $('.history-slide li').length;
                var widthItem = $('.history-slide li').width() + 50;
                $('.history-slide ul').width(allItem * widthItem); $('.content-page').delay(1000).fadeIn(500, 'linear', function () {
                    $(".loadx").fadeOut(500, "linear", function () {
                        $('.loadx').remove();
                        setTimeout(ScrollNiceG, 300);
                        setTimeout(ScrollText, 300);
                        $('.year-number li:first-child').addClass('current');
                        $('.scrollG .history-slide li:first-child').addClass('active'); $('.block-nav').css({ 'display': 'none', 'width': 0, 'z-index': -9999 });
                    });
                });
                //-----OPTION-----// 
                $('.year-number li a').click(function () {
                    $('.year-number li').removeClass('current');
                    $(this).parent().addClass('current');
                    $('.scroll-text').stop().animate({ scrollTop: 0 });
                    scrollTo($(this).attr("data-target"));
                    return false;
                });

                function scrollTo(target) {
                    var ContentScroll = $(".history-slide li[data-pos='" + target + "']");
                    var Current = $(".active").offset().left;
                    if (target == 1) {
                        $('.history-slide li').removeClass('active').addClass('hidden');
                        $(ContentScroll).removeClass('hidden').addClass('active');
                        $('.scrollG').scrollLeft(Current);
                        $('.scrollG').delay(1000).stop().animate({ scrollLeft: 0 }, 1000, 'linear');

                    } else {
                        $('.scrollG').scrollLeft(0);
                        $('.history-slide li').removeClass('active').addClass('hidden');
                        $(ContentScroll).removeClass('hidden').addClass('active');
                        $('.scrollG').delay(1000).stop().animate({ scrollLeft: ContentScroll.offset().left - 500 }, 1000, 'linear');
                        $('.scrollG').mouseenter(function () { $('.history-slide li').removeClass('hidden'); });
                        $('.scrollG').click(function () { $('.history-slide li').removeClass('hidden'); });
                        $('.scrollG').mouseover(function () { $('.history-slide li').removeClass('hidden'); });
                    }
                }
                //-----OPTION-----// 
            }
            });

            //AJAX CLOSE// 

        },
        //SCOPE OF WORK//  
        scope: function () {
            $('#container').append('<div class="loadx" style="display:block"></div>');
            setTimeout('api.goTo(4)', 100);
            var loadUrl = $('a.off-scope').attr("href");
            window.location.hash = 'scope';
            //AJAX OPEN// 
            $.ajax({ url: loadUrl, cache: false, success: function (data) {
                $('.item-page').append(data);
                ScrollNiceHide(); ResizeWindows(); $('.content-page').delay(1000).fadeIn(500, 'linear', function () {
                    $(".loadx").fadeOut(500, "linear", function () {
                        $('.loadx').remove(); $('.block-nav').css({ 'display': 'none', 'width': 0, 'z-index': -9999 });
                    });
                });
            }
            });

            //AJAX CLOSE// 

        },
        //CHART//  
        chart: function () {
            $('#container').append('<div class="loadx" style="display:block"></div>');
            setTimeout('api.goTo(5)', 100);
            var loadUrl = $('a.off-chart').attr("href");
            window.location.hash = 'chart';
            //AJAX OPEN// 
            $.ajax({ url: loadUrl, cache: false, success: function (data) {
                $('.item-page').append(data);
                ScrollNiceHide(); ResizeWindows(); $('.content-page').delay(1000).fadeIn(500, 'linear', function () {
                    $(".loadx").fadeOut(500, "linear", function () {
                        $('.loadx').remove();
                        setTimeout(ScrollNiceD, 300); $('.block-nav').css({ 'display': 'none', 'width': 0, 'z-index': -9999 });
                    });
                });
                //-----OPTION-----// 
                $('.zoom').click(function () {
                    $('.overlay-dark').fadeIn(300, 'linear');
                    $('.all-pics').css({ 'width': '100%', 'height': '100%' });
                    $('.all-pics').append('<div class="full-screen"></div>');
                    var activePicLarge = $('.content-text p').find('img').attr("srcl");
                    //   var newActive = activePicLarge.replace("_s", "_l");
                    var newActive = activePicLarge;
                    $('body').append('<div class="loadx" style="display:block"></div>');
                    $('body').append('<div class="close-pics"></div>');
                    $('.all-pics').children().append('<img src ="' + (newActive) + '" alt="pic" />');
                    $('.full-screen').delay(300).fadeIn(400, 'linear', function () {
                        $('.full-screen').getNiceScroll().show();
                        $('.full-screen').stop().animate({ scrollTop: 0 });
                        $('.full-screen').niceScroll({ touchbehavior: true, horizrailenabled: false });
                        $(".loadx").remove();
                    });
                    $('.close-pics').click(function () {
                        $('.full-screen').getNiceScroll().remove();
                        $(this).fadeOut(200, 'linear').remove();
                        $('.full-screen').fadeOut(300, 'linear');
                        $('.overlay-dark').delay(300).fadeOut(300, 'linear', function () {
                            $('.all-pics .full-screen').remove();
                            $('.all-pics').css({ 'width': 0, 'height': 0 });
                        });
                    });
                    return false;
                });
                //-----OPTION-----// 
            }
            });
            //AJAX CLOSE// 
        },
        //OPERATION//  
        operations: function () {
            $('#container').append('<div class="loadx" style="display:block"></div>');
            setTimeout('api.goTo(6)', 100);
            var loadUrl = $('a.off-operations').attr("href");
            window.location.hash = 'operations';
            //AJAX OPEN// 
            $.ajax({ url: loadUrl, cache: false, success: function (data) {
                $('.item-page').append(data);
                ScrollNiceHide(); ResizeWindows();
                $('.operation-number li:first-child').addClass('current');
                $('.scrollF .operation-tab').first().css({ 'display': 'block' }); $('.content-page').delay(1000).fadeIn(500, 'linear', function () {
                    $(".loadx").fadeOut(500, "linear", function () {
                        $('.loadx').remove();
                        setTimeout(ScrollNiceF, 300); $('.block-nav').css({ 'display': 'none', 'width': 0, 'z-index': -9999 });
                    });
                });

                //-----OPTION-----// 
                $('.operation-number li a').click(function () {
                    ScrollNiceHide();
                    $(".operation-tab").fadeOut(500, "linear");
                    $('.operation-number li').removeClass('current');
                    $(this).parent().addClass('current');
                    FadeTab($(this).attr("data-target"));
                    return false;
                });

                function FadeTab(target) {
                    var ContentTab = $(".operation-tab[data-pos='" + target + "']");
                    $(ContentTab).delay(500).fadeIn(500, 'linear', function () {
                        setTimeout(ScrollNiceF, 300);
                    });
                }
                //-----OPTION-----// 
            }
            });

            //AJAX CLOSE// 

        },


        //BUSINESS//  
        results: function () {
            $('#container').append('<div class="loadx" style="display:block"></div>');
            // setTimeout( 'api.goTo(7)', 100);
            var loadUrl = $('a.off-results').attr("href");
            window.location.hash = 'results';
            //AJAX OPEN// 
            $.ajax({ url: loadUrl, cache: false, success: function (data) {
                $('.item-page').append(data);
                ScrollNiceHide(); ResizeWindows(); $('.content-page').delay(1000).fadeIn(500, 'linear', function () {
                    $(".loadx").fadeOut(500, "linear", function () {
                        $('.loadx').remove();
                        setTimeout(ScrollNiceD, 300); $('.block-nav').css({ 'display': 'none', 'width': 0, 'z-index': -9999 });
                    });
                });
            }
            });

            //AJAX CLOSE// 
        },

        //PARTNERS//  
        partners: function () {
            $('#container').append('<div class="loadx" style="display:block"></div>');
            setTimeout('api.goTo(8)', 100);
            var loadUrl = $('a.off-partners').attr("href");
            window.location.hash = 'partners';
            //AJAX OPEN// 
            $.ajax({ url: loadUrl, cache: false, success: function (data) {
                $('.item-page').append(data);
                ScrollNiceHide(); ResizeWindows(); $('.content-page').delay(1000).fadeIn(500, 'linear', function () {
                    $(".loadx").fadeOut(500, "linear", function () {
                        $('.loadx').remove();
                        setTimeout(ScrollNiceD, 300); $('.block-nav').css({ 'display': 'none', 'width': 0, 'z-index': -9999 });
                    });
                });
            }
            });

            //AJAX CLOSE// 
        },

        //SOCIAL//  
        social: function () {
            $('#container').append('<div class="loadx" style="display:block"></div>');
            setTimeout('api.goTo(9)', 100);
            var loadUrl = $('a.off-social').attr("href");
            window.location.hash = 'social';
            //AJAX OPEN// 
            $.ajax({ url: loadUrl, cache: false, success: function (data) {
                $('.item-page').append(data);
                ScrollNiceHide(); ResizeWindows(); $('.content-page').delay(1000).fadeIn(500, 'linear', function () {
                    $(".loadx").fadeOut(500, "linear", function () {
                        $('.loadx').remove();
                        setTimeout(ScrollNiceA, 300); $('.block-nav').css({ 'display': 'none', 'width': 0, 'z-index': -9999 });
                    });
                });
            }
            });

            //AJAX CLOSE// 

        },

        //AWARDS//  
        awards: function () {
            $('#container').append('<div class="loadx" style="display:block"></div>');
            setTimeout('api.goTo(10)', 100);
            var loadUrl = $('a.off-awards').attr("href");
            window.location.hash = 'awards';
            //AJAX OPEN// 
            $.ajax({ url: loadUrl, cache: false, success: function (data) {
                $('.item-page').append(data);
                ScrollNiceHide(); ResizeWindows(); $('.content-page').delay(1000).fadeIn(500, 'linear', function () {
                    $(".loadx").fadeOut(500, "linear", function () {
                        $('.loadx').remove();
                        setTimeout(ScrollNiceC, 300); $('.block-nav').css({ 'display': 'none', 'width': 0, 'z-index': -9999 });
                    });
                });
                if ($(window).width() < 1150) {
                    $('.award-item').easySlider({ showSlides: 3, prevId: 'prevC', nextId: 'nextC', allx: true });
                } else {
                    $('.award-item').easySlider({ showSlides: 4, prevId: 'prevC', nextId: 'nextC', allx: true });
                }
                //-----OPTION-----// 
                $('span.zoom').click(function () {
                    $('.overlay-dark').fadeIn(300, 'linear');
                    $('.all-pics').css({ 'width': '100%', 'height': '100%' });
                    $('.all-pics').append('<div class="full"></div>');
                    ResizeWindows();
                    var activePicLarge = $(this).parent().find('img').attr("srcl");
                    //var newActive = activePicLarge.replace("_s", "_l");
                    var newActive = activePicLarge;
                    $('body').append('<div class="loadx" style="display:block"></div>');
                    $('body').append('<div class="close-pics"></div>');
                    $('.all-pics').children().append('<img src ="' + (newActive) + '" alt="pic" />');
                    $('.full').delay(300).fadeIn(400, 'linear', function () {
                        var ImgH = $('.full img').height();
                        $('.full img').css({ 'margin-top': $(window).height() / 2 - ImgH / 2 });
                        $('.full img').animate({ 'opacity': 1 }, 500, 'linear');
                        $('.full').getNiceScroll().show();
                        $('.full').stop().animate({ scrollTop: 0 });
                        $('.full').niceScroll({ touchbehavior: true, horizrailenabled: false });
                        $(".loadx").remove();
                    });
                    $('.close-pics').click(function () {
                        $('.full').getNiceScroll().remove();
                        $(this).fadeOut(200, 'linear').remove();
                        $('.full').fadeOut(300, 'linear');
                        $('.overlay-dark').delay(300).fadeOut(300, 'linear', function () {
                            $('.all-pics .full').remove();
                            $('.all-pics').css({ 'width': 0, 'height': 0 });
                        });
                    });
                    return false;
                });
                //-----OPTION-----// 
            }
            });

            //AJAX CLOSE// 

        },


        //VIDEO//  
        video: function () {
            $('#container').append('<div class="loadx" style="display:block"></div>');
            setTimeout('api.goTo(11)', 100);
            var loadUrl = $('a.off-video').attr("href");
            window.location.hash = 'video';
            //AJAX OPEN// 
            $.ajax({ url: loadUrl, cache: false, success: function (data) {
                $('.item-page').append(data);
                ScrollNiceHide(); ResizeWindows(); $('.content-page').delay(1000).fadeIn(500, 'linear', function () {
                    $('.video-item li:first-child').addClass('centerframe');
                    var allItem = $('.slidemenu-item').length;
                    var widthItem = $('.slidemenu-item').width();
                    $('.slidemenu-slide').width(allItem * widthItem);
                    $('.slidemenu-slide').css({ 'left': 0 });
                    $(".loadx").fadeOut(500, "linear", function () {
                        $('.loadx').remove(); $('.block-nav').css({ 'display': 'none', 'width': 0, 'z-index': -9999 });
                    });
                });

                //-----OPTION-----// 
                $('.video-item').MenuList();

                $('a.play').click(function (e) {
                    e.stopPropagation();
                    //loadVideo();
                    var idx = $(this).attr("href");
                    VideoLoad(idx);
                    return false;
                });
                //-----OPTION-----// 
            }
            });

            //AJAX CLOSE// 

        },

        //TRADING//  
        trading: function () {
            $('#container').append('<div class="loadx" style="display:block"></div>');
            setTimeout('api.goTo(12)', 100);
            var loadUrl = $('a.off-trading').attr("href");
            window.location.hash = 'trading';
            //AJAX OPEN// 
            $.ajax({ url: loadUrl, cache: false, success: function (data) {
                $('.item-page').append(data);
                ScrollNiceHide(); ResizeWindows(); $('.content-page').delay(1000).fadeIn(500, 'linear', function () {
                    $('.trading-item li:first-child').addClass('centerframe');
                    var allItem = $('.slidemenu-item').length;
                    var widthItem = $('.slidemenu-item').width();
                    $('.slidemenu-slide').width(allItem * widthItem);
                    $('.slidemenu-slide').css({ 'left': 0 });
                    $(".loadx").fadeOut(500, "linear", function () {
                        $('.loadx').remove(); $('.block-nav').css({ 'display': 'none', 'width': 0, 'z-index': -9999 });
                    });
                });

                //-----OPTION-----// 
                $('.trading-item').MenuList();
                //-----OPTION-----// 
            }
            });

            //AJAX CLOSE// 

        },


        //FINISH//  
        finish: function () {
            $('#container').append('<div class="loadx" style="display:block"></div>');
            setTimeout('api.goTo(13)', 100);
            var loadUrl = $('a.off-finish').attr("href");
            window.location.hash = 'finish';
            //AJAX OPEN// 
            $.ajax({ url: loadUrl, cache: false, success: function (data) {
                $('.item-page').append(data);
                ScrollNiceHide(); ResizeWindows(); $('.content-page').delay(1000).fadeIn(500, 'linear', function () {
                    $(".loadx").fadeOut(500, "linear", function () {
                        $('.loadx').remove();
                        setTimeout(ScrollNiceB, 300); $('.block-nav').css({ 'display': 'none', 'width': 0, 'z-index': -9999 });
                    });
                });
            }
            });
            //AJAX CLOSE// 

        }

    };



    $('.sub li a').bind('click', function (e) {
        e.preventDefault();
        ScrollNiceHide();
        ScrollTextHide();
        $('.block-nav').css({ 'display': 'block', 'width': 335, 'z-index': 9999 });
        $('.sub li').removeClass('current');
        $(this).parent().addClass('current');
        //HIDE CONTENT//
        $('.content-page').fadeOut(600, 'linear', function () {
            $(this).remove();
            $('.over').css({ 'width': 0 });
        });

        var idx = $(this).attr('id');
        switch (idx) {
            default:
                if (ContentChange[idx.substr(4)] != undefined) {
                    ContentChange[idx.substr(4)]();
                }
                break;
        }
        return false;
    });

    //REFRESH AND LOAD INNER PAGE//
    if (window.location.hash) {
        var PageActive = window.location.hash;
        PageActive = PageActive.slice(1);
        ScrollNiceHide();
        ResizeWindows();

        if (PageActive == 'message') { setTimeout(function () { $('#new-message').trigger('click'); }, 2000); }
        else if (PageActive == 'vission') { setTimeout(function () { $('#new-vission').trigger('click'); }, 2000); }
        else if (PageActive == 'history') { setTimeout(function () { $('#new-history').trigger('click'); }, 2000); }
        else if (PageActive == 'scope') { setTimeout(function () { $('#new-scope').trigger('click'); }, 2000); }
        else if (PageActive == 'organization') { setTimeout(function () { $('#new-chart').trigger('click'); }, 2000); }
        else if (PageActive == 'operations') { setTimeout(function () { $('#new-operations').trigger('click'); }, 2000); }
        else if (PageActive == 'operating') { setTimeout(function () { $('#new-results').trigger('click'); }, 2000); }
        else if (PageActive == 'partners') { setTimeout(function () { $('#new-partners').trigger('click'); }, 2000); }
        else if (PageActive == 'social') { setTimeout(function () { $('#new-social').trigger('click'); }, 2000); }
        else if (PageActive == 'awards') { setTimeout(function () { $('#new-awards').trigger('click'); }, 2000); }
        else if (PageActive == 'video') { setTimeout(function () { $('#new-video').trigger('click'); }, 2000); }
        else if (PageActive == 'trading') { setTimeout(function () { $('#new-trading').trigger('click'); }, 2000); }
        else if (PageActive == 'finish') { setTimeout(function () { $('#new-finish').trigger('click'); }, 2000); }
    }

}

// LOAD VIDEO HTML5//
function VideoLoad(idx) {
    $('#container').append('<div class="loadx" style="display:block"></div>');
    var loadUrl = idx;
    $.ajax({ url: loadUrl, cache: false, success: function (data) {
        $('.allvideo').append(data);
        $('video').VideoJS();
        $('.overlay-dark').delay(1000).fadeIn(500, 'linear', function () {
            $('.allvideo .video-list').css({ 'top': $(window).height() / 2 - 250, 'left': $(window).width() / 2 - 440 });
            $('.allvideo').css({ 'width': '100%' });
            $('.loadx').remove();
            $('.close-video').fadeIn(300, 'linear');
        });

        // CLOSE VIDEO//
        if (isIE && version < 9) {
            $('.close-video').click(function () {
                //jwplayer().stop();
                $('.overlay-dark').fadeOut(500, 'linear', function () {
                    $('.allvideo').css({ 'width': 0 });
                    //$('.allvideo .video-list .vjs-flash-fallback').remove();
                    $('.allvideo').children().remove();
                });
                return false;
            });
        } else {
            $('.close-video').click(function () {
                $('.video-wrap').find('video')[0].pause();
                $('.overlay-dark').fadeOut(500, 'linear', function () {
                    $('.allvideo').css({ 'width': 0 });
                    $('.allvideo .video-list').remove();
                });
                return false;
            });
        }
    }
    });
}