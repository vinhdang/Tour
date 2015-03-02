sys = {};

if (sys.data == undefined) {
    sys.data = {};
}

sys.detail = {};
sys.detail.index = {};



sys.detail.index.gallery = {
    initPage: function(){
        $('#order-request-modal').modal('hide');
        sys.detail.index.gallery.orderTour($('button[name="send_request_tour"]'));
    },
    orderTour: function(elem) {
        $(elem).click(function() {
            var me = $(this),
                form = $('form#form_order_tour'),
                email = $('input[name="order_email"]').val(),
                phone = $('input[name="order_phone"]').val(),
                note = $('textarea[name="order_note"]').val(),
                url = form.attr('action');
            $.validator.addMethod("validateEmail", function (value, element) {
                var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                return re.test(value);
            }, "Email không hợp lệ!");
            sys.detail.index.gallery.formValidation();

            if(!form.valid()) {
                return;
            }
            helper.core.util.showLoading();
            url += '&em=' + email + '&p=' + phone + '&note=' + note;

            $.ajax({
                type: "GET",
                url: url,
                beforeSend: function () {
                    return true;
                },
                success: function (data) {
                    helper.core.util.closeLoading();
                    if (data.success || data.success == "true") {
                        $('input[name="order_email"]').val('');
                        $('input[name="order_phone"]').val('');
                        //sys.core.pageMsg.success("Reset password success!");
                        $('button[name="close_modal_tour"]').trigger('click');
                        helper.core.pageMsg.success("Đặt tour thành công, xin cảm ơn!");
                        $('textarea[name="order_note"]').val('');
                        $('input[name="order_phone"]').val('');
                        $('input[name="order_email"]').val('');
                    }
                    else {
                        helper.core.pageMsg.fail("Đặt tour lỗi!");
                    }
                }
            });
        });
    },
    formValidation: function () {

        $('form#form_order_tour').validate({
            rules: {
                order_email: true,
                order_phone: true
            },
            messages: {
                order_email: {
                    required: "Thông tin không hợp lệ!"
                },
                order_phone: {
                    required: "Thông tin không hợp lệ!"
                }
            },
            errorPlacement: function (error, element) {
                error.appendTo(element.parent());
            }
        });
    },
    
};

sys.detail.index.info = {
    initPage: function () {
        sys.detail.index.info.submitComment($('button#comment_submit'));
        //$.validator.addMethod("validateEmail", function (value, element) {
        //    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        //    return re.test(value);
        //}, "Email không hợp lệ!");
        sys.detail.index.info.formValidation();
    },
    submitComment: function (elem) {
        $(elem).click(function() {
            var me = $(this),
                name = $('input#comment_customer_name').val(),
                email = $('input#comment_customer_email').val(),
                msg = $('textarea#comment_customer_message').val(),
                form = $('form#form_tour_comment'),
                tourid = $('input[name="comment_tour_id"]').val(),
                level = $('#price-range').val().split(';')[1];
                url = form.attr('action');
            
                if (!form.valid()) {
                return;
            }
            helper.core.util.showLoading();
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    un: name,
                    msg: msg,
                    tid: tourid,
                    level: level,
                    email: email
                },
                beforeSend: function () {
                    return true;
                },
                success: function (data) {
                    helper.core.util.closeLoading();
                    if (data.success == "false" || data.success == "False" || !data.success) {
                        //sys.core.pageMsg.success("Reset password success!");
                        if (data.exist = false) {
                            helper.core.pageMsg.fail('Tạo nhận xét lỗi!');
                        }else {
                            helper.core.pageMsg.fail('Nhận xét đã tồn tại!');
                        }
                        
                    } else {
                        $('div#comment').html(data.html);
                        helper.core.pageMsg.success('Thêm nhận xét thành công. Cảm ơn!');
                    }
                }
            });
        });
    },
    formValidation: function () {
        $('form#form_tour_comment').validate({
            rules: {
                comment_name: "required",
                comment_message: "required",
                comment_customer_email: "required"
            },
            messages: {
                comment_name: {
                    required: "Thông tin không hợp lệ!"
                },
                comment_message: {
                    required: "Thông tin không hợp lệ!"
                },
                comment_email: {
                    required: "Email không hợp lệ!"
                }
            },
            errorPlacement: function (error, element) {
                console.log((error[0]));
                error.appendTo(element.parent());
            }
        });
    }
};




