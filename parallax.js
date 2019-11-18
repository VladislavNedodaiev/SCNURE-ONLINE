$(window).bind('scroll',function(e){
    parallaxScroll();
});
 
function parallaxScroll(){
    var scrolled = $(window).scrollTop();
    $('.yep_bg').css('top',(250+(scrolled*.3))+'px');
    $('.content').css('top',(0-(scrolled*.5))+'px');
}