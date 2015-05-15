sys = {};
sys.ad = {};
sys.ad.config = {
    deleteTourType: function (){return '/Tourtype/Delete';}
};
sys.ad.tourtype = {};

sys.ad.tourtype.index = {
    initPage: function () {
        sys.ad.tourtype.index.checkAll($('#checkAllRecords'));
        sys.ad.tourtype.index.deleteTourType($('input[name="Delete"]'));
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

sys.ad.tourtype.edit = {
    initPage: function () {
    },
    updateOrder: function (elem) {
        $(elem).click(function () {
            var me = $(this),
                url = $('form#hotel_update_order').attr('action'),
                orderVal = $('input[name="display_order"]').val(),
                regex = /^\d+$/;

            if (orderVal === '' || !regex.test(orderVal)) {
                sys.core.dialog.warningDialog("Giá trị không hợp lệ!");
                return;
            }

            $.ajax({
                type: "POST",
                url: url,
                data: $('form#hotel_update_order').serialize(),
                beforeSend: function () {
                    sys.core.util.showLoading();
                    return true;
                },
                success: function (data) {
                    sys.core.util.closeLoading();
                    if (data.success != true) {
                        sys.core.dialog.warningDialog("Cập nhật không thành công! Vui lòng thử lại");
                        $('#dialog_content').dialog('close');
                    } else {
                        window.location = data.url;
                    }
                }
            });
        });
    },
    cancel: function (elem) {
        $(elem).click(function () {
            $('#dialog_content').dialog('close');
        });
    }
};
