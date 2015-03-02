/***************************************************************
	TRAFFIC LIGHT
****************************************************************/
$(".trafficSelect img").click(function(){
		$(".trafficSelectContent").slideDown();
	})

$(".trafficIcon").click(function(){
	$(".traffic").toggle('slow');
	});
	
$(".trafficSelect").click(function(){
	$(".trafficSelectContent").toggle('slow');
	});

$(".bookmark").click(function(){
	$(".bookmarkContent").toggle('slow');
	$(".showBookmark .show").click(function(){
		$(".showBookmarkItems").slideDown('slow');
		})
	})
/*
$(".showBookmarkItems .bookmarkFunc .bookmarkEdit").click(function(){
		var seParent = $(this).parent(".bookmarkFunc");
		seParent.nextAll(".bookmarkEditContent").toggle('slow');
	})	
*/	
/***************************************************************
	VIEW RAW DATA
****************************************************************/
$(".viewRawData img").click(function(){
	if($(this).next(".viewRawDataContent").length)
		$(this).next(".viewRawDataContent").toggle('slow');
	});
	
/***************************************************************
	TAB
****************************************************************/
if($(".tab").length){
	$(".tab_content .formBox").not(".tab_content .formBox:first").hide();
	$('ul [id^="tab_"]').each(function(index) {
		$(this).click(function(){	
			$(".tab li").removeClass("tab_current");
			$(this).addClass("tab_current");
			var id_name = $(this).attr('id');
			$(".tab_content .formBox").hide();
			$('div.'+id_name).fadeIn("slow");
		   });
		});
		$(".tab_current").trigger('click');
	}
	
// TAB 2
if($(".tab2").length){
	$(".tab2_content .formBox").not(".tab2_content .formBox:first").hide();
	$('ul [id^="tab2_"]').each(function(index) {
		$(this).click(function(){	
			$(".tab2 li").removeClass("tab2_current");
			$(this).addClass("tab2_current");
			var id_name = $(this).attr('id');
			$(".tab2_content .formBox").hide();
			$('div.'+id_name).fadeIn("slow");
			});
		});
		$(".tab2_current").trigger('click');
	}
/***************************************************************
	REVEAL DIALOG
****************************************************************/
$(".btnDetail").click(function(){
	$('.reveal-modal').reveal();
	})
/***************************************************************
	EQUAL HEIGHT
****************************************************************/
$(window).load(function(){
	if($("#leftSide").height() < $("#rightSide").height()){
		$("#leftSide").height($("#rightSide").height())	
		}
	else if($("#leftSide").height() > $("#rightSide").height()){
			$("#rightSide").height($("#leftSide").height())	
		}
	});
/***************************************************************
	SELECT BOX
****************************************************************/
$(function () {
        $("select").selectbox();
    });
//prettyForms();

/************************************************************************
	Z-INDEX DIV
*************************************************************************/	 
    $(function() {  
        var zIndexNumber = 5000;  
        $('div').each(function() {  
            $(this).css('zIndex', zIndexNumber);  
            zIndexNumber -= 10;  
        });  
    });  