sys = {};
sys.ad = {};
sys.ad.config = {
    deleteTourType: function () { return '/Tourtype/Delete'; },
    loadProvince: function () { return '/TourProvider/LoadProvinceByNational'; }
};
sys.ad.tourprovider = {};

sys.ad.tourprovider.index = {
    initPage: function () {
        sys.ad.tourprovider.index.checkAll($('#checkAllRecords'));
        //sys.ad.tourprovider.index.deleteTourType($('input[name="Delete"]'));
    },
    checkAll: function (elem) {
        $(elem).click(function () {
            var me = $(this),
                ischecked = me.is(':checked'),
                status = '';

            $('input.checkedRecords').each(function () {
                var self = $(this);
                self.attr('checked', ischecked);
            });
        });
    },
    deleteTourType: function (elem) {
        $(elem).click(function () {
            var me = $(this),
                url = sys.ad.config.deleteTourType(),
                items = [];

            $('input.checkedRecords').each(function () {
                var self = $(this),
                    ischeck = self.is(':checked');
                if (ischeck) {
                    items.push(self.val());
                }
            });

            url += '?ids=' + items.join(',');

            $.ajax({
                type: 'GET',
                url: url,
                success: function (data) {
                    if (data) {
                        window.location.reload(true);
                    } else {
                        alert('Xóa loại tour thất bại. Vui lòng thử lại sau.');
                    }
                }
            });
        });
    }
};

sys.ad.tourprovider.edit = {
    initPage: function () {
        sys.ad.tourprovider.edit.loadProvince($('select[name="NationalID"]'));
    },
    loadProvince: function (elem) {
        $(elem).change(function () {
            var me = $(this),
                url = sys.ad.config.loadProvince(),
                value = me.val();

            if (value === '') {
                alert('Vui lòng chọn quốc gia.');
                return;
            }

            url += '?nationalId=' + value;

            $.ajax({
                type: "GET",
                url: url,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data) {
                        $('select[name="ProvinceID"]').find('option')
                                .remove()
                                .end();

                        $.each(data, function (i, item) {
                            $('select[name="ProvinceID"]').append($('<option>').text(item.Name).attr('value', item.ID));
                        });
                    } else {
                        alert('Load tỉnh/tp lỗi');
                    }
                }
            });
        });
    }
};
