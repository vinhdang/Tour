sys = {};

if (sys.data == undefined) {
    sys.data = {};
}

sys.search = {};
sys.search.index = {};

sys.search.index.filter = {
    initPage: function(){
        //sys.search.index.filter.filterByTheme($('input.filter_theme[type="checkbox"]'));
        //sys.search.index.filter.filterByMonth($('input.filter_month[type="checkbox"]'));
        //sys.search.index.filter.filterByComment($('input.filter_comment[type="checkbox"]'));
        //sys.search.index.filter.filterByDuration($('input.filter_comment[type="checkbox"]'));
        sys.search.index.filter.filterByNational($('input.filter_national[type="checkbox"]'));
       
    },
    filterByTheme: function (elem) {
        $(elem).click(function() {
            sys.search.index.filter.loadData();
        });
    },
    filterByMonth: function (elem) {
        $(elem).click(function () {
            sys.search.index.filter.loadData();
        });
    },
    filterByComment: function (elem) {
        $(elem).click(function () {
            sys.search.index.filter.loadData();
        });
    },
    filterByDuration: function (elem) {
        $(elem).click(function () {
            sys.search.index.filter.loadData();
        });
    },
    filterByNational: function (elem) {
        $(elem).click(function () {
            sys.search.index.filter.loadData();
        });
    },
    loadData: function (){
        var url = $('input[name="filter_action"]').val(),
            //duration = $('input#duration').val(),
            price = $('input#price-range').val(),
            //themes = sys.search.index.filter.collectThemeSelected(),
            //months = sys.search.index.filter.collectMonthSelected(),
            //comments = sys.search.index.filter.collectCommentSelected(),
            nationals = sys.search.index.filter.collectNationalSelected(),
            departprovince = $('input#departadministrative_area_level_1').val(),
            destProvince = $('input#destadministrative_area_level_1').val(),
            destNational = $('input#destcountry').val(),
            departNational = $('input#departcountry').val();
        helper.core.util.showLoading();
        $.ajax({
            type: "POST",
            url: url,
            data: {
                //duration: duration,
                price: price,
                //themes: themes.join(','),
                //months: months.join(','),
                //comments: comments.join(','),
                depart: departprovince,
                dest: destProvince,
                nationals: nationals.join(','),
                destnational: destNational,
                departnational: departNational
            },
            beforeSend: function () {
                return true;
            },
            success: function (data) {
                helper.core.util.closeLoading();
                if (!data.success) {
                    //sys.core.pageMsg.success("Reset password success!");
                    alert('Tìm kiếm lỗi!');

                } else {
                    $('div#search_tour_result').html(data.html);
                    var nationals = data.nationals.split(',');
                    if(nationals.length > 0) {

                        $('input.filter_national').each(function () {
                            var self = $(this);
                            if (nationals.indexOf(self.val()) != -1) {
                                self.prop('checked', true);
                            } else {
                                self.prop('checked', false);
                            }
                        });
                    }
                }
            }
        });
    },
    collectThemeSelected: function () {
        var themeIds = [];
        $('input.filter_theme[type="checkbox"]').each(function () {
            var me = $(this);
            
            if(me.is(':checked')) {
                themeIds.push(me.val());
            }
        });

        return themeIds;
    },
    collectCommentSelected: function () {
        var commentIds = [];
        $('input.filter_comment[type="checkbox"]').each(function () {
            var me = $(this);

            if (me.is(':checked')) {
                commentIds.push(me.val());
            }
        });

        return commentIds;
    },
    collectMonthSelected: function () {
        var monthIds = [];
        $('input.filter_month[type="checkbox"]').each(function () {
            var me = $(this);

            if (me.is(':checked')) {
                monthIds.push(me.val());
            }
        });

        return monthIds;
    },
    collectNationalSelected: function () {
        var nationalIds = [];
        $('input.filter_national[type="checkbox"]').each(function () {
            var me = $(this);

            if (me.is(':checked')) {
                nationalIds.push(me.val());
            }
        });

        return nationalIds;
    }
};

sys.search.index.search = {
    initPage: function () {
        sys.search.index.search.search($('button#search_tour_submit'));
        sys.search.index.search.initSearchDepart();
        sys.search.index.search.initSearchDest();
    },
    search: function (elem) {
        $(elem).click(function () {
            if($('#search_dest_province').val() == "") {
                $('#destcountry').val('');
            }
            sys.search.index.filter.loadData();
            
        });
    },
    initSearchDepart: function () {
        function split(val) {
            return val.split(/,\s*/);
        }

        function extractLast(term) {
            return split(term).pop();
        }


        $("#search_depart_province").bind("keydown", function (event, ui) {

            //if (event.keyCode === ui.keyCode.TAB && $(this).data("autocomplete").menu.active) {
            //    event.preventDefault();
            //}
        });

        $("#search_depart_province").autocomplete({
            autoFocus: true,
            minLength: 1,
            source: function (request, response) {
                $.ajax({
                    url: $('input[name="search-province-url"]').val(),
                    dataType: "json",
                    data: {
                        query: request.term
                    },
                    term: extractLast(request.term),
                    success: function (data) {
                        //response(data);
                        response($.map(data, function (item) {
                            console.log(item);
                            return { label: item.label, value: item.value, id: item.proid };
                        }));
                    }
                });
            },
            focus: function (event, ui) {
                // prevent value inserted on focus
                return false;
            },
            search: function () {
                // custom minLength
                var term = extractLast(this.value);
                if (term.length < 1) {
                    return;
                }
            },
            select: function (event, ui) {
                var terms = split(this.value);
                // remove the current input
                terms.pop();
                // add the selected item
                terms.push(ui.item.value);
                // add placeholder to get the comma-and-space at the end
                //terms.push("");
                this.value = ui.item.label; //.join(", ");
                var parts = ui.item.value.split(',');
                var pro = parts.length > 1 ? parts[1] : "";
                var nat = parts.length > 2 ? parts[2] : "";
                $('#departadministrative_area_level_1').val(pro);
                $('#departcountry').val(nat);
                return false;
            }
        });
    },
    initSearchDest: function () {
        function split(val) {
            return val.split(/,\s*/);
        }

        function extractLast(term) {
            return split(term).pop();
        }


        $("#search_dest_province").bind("keydown", function (event, ui) {

            //if (event.keyCode === ui.keyCode.TAB && $(this).data("autocomplete").menu.active) {
            //    event.preventDefault();
            //}
        });

        $("#search_dest_province").autocomplete({
            autoFocus: true,
            minLength: 1,
            source: function (request, response) {
                $.ajax({
                    url: $('input[name="search-province-url"]').val(),
                    dataType: "json",
                    data: {
                        query: request.term
                    },
                    term: extractLast(request.term),
                    success: function (data) {
                        //response(data);
                        response($.map(data, function (item) {
                            console.log(item);
                            return { label: item.label, value: item.value, id: item.proid };
                        }));
                    }
                });
            },
            focus: function (event, ui) {
                // prevent value inserted on focus
                return false;
            },
            search: function () {
                // custom minLength
                var term = extractLast(this.value);
                if (term.length < 1) {
                    return;
                }
            },
            select: function (event, ui) {
                var terms = split(this.value);
                // remove the current input
                terms.pop();
                // add the selected item
                terms.push(ui.item.value);
                // add placeholder to get the comma-and-space at the end
                //terms.push("");
                this.value = ui.item.label; //.join(", ");
                var parts = ui.item.value.split(',');
                var pro = parts.length > 1 ? parts[1] : "";
                var nat = parts.length > 2 ? parts[2] : "";
                $('#destadministrative_area_level_1').val(pro);
                $('#departcountry').val(nat);
                return false;
            }
        });
    }
};

sys.search.index.tour = {
    initPage: function () {
        sys.search.index.tour.changePage($('a.navigate_page'));
        
        //var options = {
        //    //types: ['(cities)'],
        //    //componentRestrictions: { 'country': 'vn' }
        //};
        //var departinput = (document.getElementById('search_depart_province'));
        //var destinput = (document.getElementById('search_dest_province'));

        //var departcomponentForm = {
        //    // street_number: 'short_name',
        //    //route: 'long_name',
        //    //locality: 'long_name',
        //    administrative_area_level_1: 'short_name',
        //    //country: 'long_name'
        //    //postal_code: 'short_name'
        //};
        //var destcomponentForm = {
        //    // street_number: 'short_name',
        //    //route: 'long_name',
        //    //locality: 'long_name',
        //    administrative_area_level_1: 'short_name',
        //    country: 'long_name'
        //    //postal_code: 'short_name'
        //};

        //var autocomplete1 = new google.maps.places.Autocomplete(departinput, options);
        //google.maps.event.addListener(autocomplete1, 'place_changed', function () {
        //    sys.search.index.tour.fillInAddress(autocomplete1, departcomponentForm, 'depart');
        //});
        
        //var autocomplete2 = new google.maps.places.Autocomplete(destinput, options);
        //google.maps.event.addListener(autocomplete2, 'place_changed', function () {
        //    sys.search.index.tour.fillInAddress(autocomplete2, destcomponentForm, 'dest');
        //});
    },
    fillInAddress: function (autocomplete, componentForm, prefix) {
        
        // Get the place details from the autocomplete object.
        var place = autocomplete.getPlace();
    
        for (var index in componentForm) {
            if (componentForm.hasOwnProperty(index)) {
                var component = componentForm[index];
                var obj = document.getElementById(prefix + component);
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
                var obj = document.getElementById(prefix + addressType);
                console.log(val, obj);
                if(val) {
                    val = val.toLocaleLowerCase();
                    if(val.indexOf("province")) {
                        val = val.replace('province', '').trim();
                    }
                    
                    if (val.indexOf("city")) {
                        val = val.replace('city', '').trim();
                    }
                }             

                $(obj).val(val);
            }
        }
    },
    changePage: function (elem) {
        $(elem).click(function (evt) {
            var me = $(this),
                url = me.attr('navigate');
            helper.core.util.showLoading();
            $.ajax({
                type: "GET",
                url: url,
                beforeSend: function () {
                    return true;
                },
                success: function (data) {
                    helper.core.util.closeLoading();
                    if (!data.success) {
                        //sys.core.pageMsg.success("Reset password success!");
                        alert('Chuyển trang lỗi!');

                    } else {
                        $('div#search_tour_result').html(data.html);
                    }
                }
            });
        });
    },
    love: function (elem) {
        $(elem).click(function() {
            var me = $(this),
                url = me.attr('navigate');

            $.ajax({
                type: "GET",
                url: url,
                beforeSend: function () {
                    return true;
                },
                success: function (data) {
                    if (!data.success) {
                        //sys.core.pageMsg.success("Reset password success!");
                        alert('Chuyển trang lỗi!');
                    } else {
                        alert('Cám ơn sự bình chọn của bạn!');
                    }
                }
            });
        });
    }
};




