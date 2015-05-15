sys = {};

if (sys.data == undefined) {
    sys.data = {};
}

sys.home = {};
sys.home.index = {};

sys.home.index.search = {
    initPage: function(){
        //var options = {
        //    //types: ['(cities)'],
        //    //componentRestrictions: { 'country': 'vn' }
        //};
        //var input = (document.getElementById('home_search_input'));
        //var autocomplete = new google.maps.places.Autocomplete(input, options);
        //google.maps.event.addListener(autocomplete, 'place_changed', function () {
        //    sys.home.index.search.fillInAddress(autocomplete);
        //});
        
        function split(val) {
            return val.split(/,\s*/);
        }

        function extractLast(term) {
            return split(term).pop();
        }

     
            $("#home_search_input").bind("keydown", function (event, ui) {
            
                //if (event.keyCode === ui.keyCode.TAB && $(this).data("autocomplete").menu.active) {
                //    event.preventDefault();
                //}
            });

            var isHoverSelect = false;
        $("#home_search_input").autocomplete({
            autoFocus: true,
            minLength: 1,
            source: function(request, response) {
                $.ajax({
                    url: $('input[name="search-province-url"]').val(),
                    dataType: "json",
                    data: {
                        query: request.term
                    },
                    term: extractLast(request.term),
                    success: function(data) {
                        //response(data);
                        response($.map(data, function(item) {
                            console.log(item);
                            return { label: item.label, value: item.value, id: item.proid };
                        }));
                    }
                });
            },
            focus: function(event, ui) {
                // prevent value inserted on focus
                return false;
            },
            search: function() {
                // custom minLength
                var term = extractLast(this.value);
                if (term.length < 1) {
                    return;
                }
            },
            select: function(event, ui) {
                var terms = split(this.value);
                // remove the current input
                terms.pop();
                // add the selected item
                terms.push(ui.item.value);
                // add placeholder to get the comma-and-space at the end
                //terms.push("");
                this.value = ui.item.label; //.join(", ");
                var parts = ui.item.value.split(',');
                var dist = parts.length > 0 ? parts[0] : "";
                var pro = parts.length > 1 ? parts[1] : "";
                var nat = parts.length > 2 ? parts[2] : "";
                $('#locality').val(dist);
                $('#administrative_area_level_1').val(pro);
                $('#country').val(nat);
                return false;
            }
        });

        sys.home.index.search.initBanner();
    },
    fillInAddress: function (autocomplete) {
        var componentForm = {
            // street_number: 'short_name',
            //route: 'long_name',
            locality: 'long_name',
            administrative_area_level_1: 'short_name',
            country: 'long_name'
            //postal_code: 'short_name'
        };
        // Get the place details from the autocomplete object.
          var place = autocomplete.getPlace();
    
          for (var index in componentForm) {
              if (componentForm.hasOwnProperty(index)) {
                  var component = componentForm[index];
                  var obj = document.getElementById(component);
                  $(obj).val('');
                  $(obj).disabled = false;
              }
        }

        // Get each component of the address from the place details
        // and fill the corresponding field on the form.
        for (var i = 0; i < place.address_components.length; i++) {
            var addressType = place.address_components[i].types[0];
            if (componentForm[addressType]) {
                var val = place.address_components[i][componentForm[addressType]];
                var obj = document.getElementById(addressType);
                $(obj).val(val);
            }
        }
    },
    initBanner: function () {
        var tpj = jQuery;
        tpj.noConflict();

        tpj(document).ready(function () {

            if (tpj.fn.cssOriginal != undefined)
                tpj.fn.css = tpj.fn.cssOriginal;

            tpj('.fullscreenbanner').revolution(
                {
                    delay: 9000,
                    startwidth: 1170,
                    startheight: 200,

                    onHoverStop: "off",						// Stop Banner Timet at Hover on Slide on/off

                    thumbWidth: 100,							// Thumb With and Height and Amount (only if navigation Tyope set to thumb !)
                    thumbHeight: 10,
                    thumbAmount: 3,

                    hideThumbs: 0,
                    navigationType: "bullet",				// bullet, thumb, none
                    navigationArrows: "solo",				// nexttobullets, solo (old name verticalcentered), none

                    navigationStyle: false,				// round,square,navbar,round-old,square-old,navbar-old, or any from the list in the docu (choose between 50+ different item), custom


                    navigationHAlign: "left",				// Vertical Align top,center,bottom
                    navigationVAlign: "bottom",					// Horizontal Align left,center,right
                    navigationHOffset: 30,
                    navigationVOffset: 30,

                    soloArrowLeftHalign: "left",
                    soloArrowLeftValign: "center",
                    soloArrowLeftHOffset: 20,
                    soloArrowLeftVOffset: 0,

                    soloArrowRightHalign: "right",
                    soloArrowRightValign: "center",
                    soloArrowRightHOffset: 20,
                    soloArrowRightVOffset: 0,

                    touchenabled: "on",						// Enable Swipe Function : on/off


                    stopAtSlide: -1,							// Stop Timer if Slide "x" has been Reached. If stopAfterLoops set to 0, then it stops already in the first Loop at slide X which defined. -1 means do not stop at any slide. stopAfterLoops has no sinn in this case.
                    stopAfterLoops: -1,						// Stop Timer if All slides has been played "x" times. IT will stop at THe slide which is defined via stopAtSlide:x, if set to -1 slide never stop automatic

                    hideCaptionAtLimit: 0,					// It Defines if a caption should be shown under a Screen Resolution ( Basod on The Width of Browser)
                    hideAllCaptionAtLilmit: 0,				// Hide all The Captions if Width of Browser is less then this value
                    hideSliderAtLimit: 0,					// Hide the whole slider, and stop also functions if Width of Browser is less than this value

                    fullWidth: "on",							// Same time only Enable FullScreen of FullWidth !!
                    fullScreen: "off",						// Same time only Enable FullScreen of FullWidth !!

                    shadow: 0								//0 = no Shadow, 1,2,3 = 3 Different Art of Shadows -  (No Shadow in Fullwidth Version !)
                });
        });
    }
};






