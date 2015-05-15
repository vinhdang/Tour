var helper = {};
if (!helper.data) {
    helper.data = {};
}

/* end init mcom */
helper.core = {};


helper.core.util = {
    showLoading: function (el) {
        if(el == undefined) {
            el = $('body');
        }
        var replaceDiv = el;
        var position = replaceDiv.offset();
        var parent = replaceDiv.parent();
        
        var overlay = $('<div></div>')
				.attr('id', "loading_overlay")
				.addClass('loading-overlay')
				.css({
				    height: parent.outerHeight(),
				    width: parent.outerWidth(),
				    left: 0,
				    top: 0
				}).appendTo('body');
        var loading_image = $('<div id="loading_content">&nbsp;</div>').appendTo(overlay);
       
        overlay.show();
    },
    closeLoading: function () {
        var loadding_overlay = $("#loading_overlay");
        if (loadding_overlay.length > 0) {
            loadding_overlay.css('display', 'none');
            loadding_overlay.hide();
            loadding_overlay.remove();
        }
    },
    multiReplace: function (template, data) {
        for (var param in data) {
            template = template.replace(new RegExp("\\{" + param + "\\}", "g"), data[param]);
        }
        return template;
    }
};

helper.core.dialog = {
    //only for use in dialog
    convertFormToFormAjax: function (popDialog) {
        var form = $('#dialog_content .sys_form');
        if (form.length == 0) {
            return;
        }
        var newAction = helper.core.util.addParameter(form.attr('action'), 'formatlayout', 'plain');
        form.attr('action', newAction);
        form.ajaxForm({
            target: '#dialog_content',
            error: function (xhr) {
                helper.core.dialog.errorDialog(xhr.responseText);
                popDialog.remove();
            },
            success: function (responseText, statusText, xhr) {
                if (helper.IsRefresh) {
                    setTimeout(function () {
                        location.reload(true);
                    }, helper.WaitTimeToRefresh);
                }
                // click cancel link, close box
                $('button.cancel').click(function () { popDialog.dialog('close'); });
                $("#dialog_content h2").hide();
                helper.core.dialog.convertFormToFormAjax(popDialog);
            } //success
        });
    },
    openLinkAsDialog: function (elements, opts) {
        var options = {
            width: 'auto',
            height: 'auto'
        };
        $.extend(options, opts);
        /* user jquery ui*/
        elements.click(function () {

            var url = this.href;
            var title = this.title;
            //url = sys.core.util.refreshLink(url);
            var link = $(this);
            var width = link.attr('dwidth') || 'auto';
            options.width = width;
            var popDialog = $('<div/>', { id: 'dialog_content', style: "display:none", html: '<div id="cboxLoadingGraphic" style="width:' + '100%;' + ';">&nbsp;</div>' }).appendTo('body');
            /*
            var beFocuesEffectE = null;
            var hasFocusEffect = link.hasClass('focus_effect');
            if (hasFocusEffect) {
            beFocuesEffectE = link.parents('tr');
            }
            */
            if (options.loadInFrame === true) {
                popDialog = $('<div><iframe src="' + url + '" width="100%" height="100%"></iframe></div>');
            }
            var dialogClass = link.attr('dialogClass');
            // init dialog
            popDialog.dialog({
                modal: true,
                width: options.width,
                height: options.height,
                title: title,
                open: function (event, ui) {
                    var me = $(this);
                    if (options.loadInFrame === true) {
                        return false;
                    }
                    //load content
                    me.load(url, function (responseText, textStatus, XMlHttpRequest) {
                        var me = $(this);
                        me.dialog('close');
                        if (textStatus == 'error') {
                            helper.core.dialog.errorDialog('Connection Error');
                            popDialog.remove();
                            return;
                        }

                        // get default title
                        var titleE = $("#dialog_content h2");
                        titleE = titleE.length > 0 ? $(titleE[0]) : null;
                        var title = titleE != null ? titleE.html() : '';
                        if (titleE != null) {
                            titleE.hide();
                        }
                        // end get default title
                        me.dialog('option', {
                            width: options.width,
                            height: options.height,
                            title: title,
                            dialogClass: dialogClass,
                            close: function (event, ui) {
                                var me = $(this);
                                //sys.core.util.turnOffFocus(link);
                                me.remove();
                                if (helper.IsRefresh) {
                                    location.reload(true);
                                }

                            }, //close
                            open: function (event, ui) {
                                var me = $(this);
                                helper.core.dialog.convertFormToFormAjax(me);
                                $('button.cancel').click(function () { me.dialog('close'); });
                                if (helper.IsRefresh) {
                                    setTimeout(function () {
                                        location.reload(true);
                                    }, helper.WaitTimeToRefresh);
                                }
                            } //open
                        }); // popDialog.load

                        me.dialog('open');
                    }); // popDialog.load

                } //open
            }); // popDialog
            return false;
        }); //elements.click
    }, //open Link as Dialog
    openPageAsDialog: function (opts) {
        var options = {
            width: '650px',
            height: 'auto'
        };
        if (opts.dwidth != '' || opts.dwidth != undefined) {
            options = {
                width: opts.dwidth,
                height: 'auto'
            };
        }

        $.extend(options, opts);
        /* user jquery ui*/

        var url = options.href;
        var title = options.title;
        //url = sys.core.util.refreshLink(url);
        var width = options.dwidth || '400px';
        //        var popDialog = $('<div/>', { id: 'dialog_content', style: "display:none", html: '<div id="cboxLoadingGraphic" style="width:' + width + ';">&nbsp;</div>' }).appendTo('body');
        var popDialog = $('<div/>', { id: 'dialog_content', style: "background: url('/Content/css/themes/aredstore/images/login-loading.gif') no-repeat center;", html: '<div id="cboxLoadingGraphic" style="width:' + '100%;' + ';">&nbsp;</div>' }).appendTo('body');
        /*
        var beFocuesEffectE = null;
        var hasFocusEffect = link.hasClass('focus_effect');
        if (hasFocusEffect) {
        beFocuesEffectE = link.parents('tr');
        }
        */

        if (options.loadInFrame === true) {
            popDialog = $('<div><iframe src="' + url + '" width="100%" height="100%"></iframe></div>');
        }
        // init dialog
        popDialog.dialog({
            modal: true,
            width: options.width,
            height: options.height,
            title: title,
            open: function (event, ui) {
                var me = $(this);
                if (options.loadInFrame === true) {
                    return false;
                }
                //hide close button
                me.closest('.ui-dialog').find('.ui-dialog-titlebar').hide();
                // me.closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                //load content
                me.load(url, function (responseText, textStatus, XMlHttpRequest) {
                    var me = $(this);
                    me.dialog('close');
                    if (textStatus == 'error') {
                        helper.core.dialog.errorDialog('Connection Error');
                        popDialog.remove();
                        return;
                    }

                    // get default title
                    var titleE = $("#dialog_content h2");
                    titleE = titleE.length > 0 ? $(titleE[0]) : null;
                    var title = titleE != null ? titleE.html() : '';
                    if (titleE != null) {
                        titleE.hide();
                    }
                    // end get default title
                    me.dialog('option', {
                        width: options.width,
                        height: options.height,
                        title: title,
                        close: function (event, ui) {
                            var me = $(this);
                            //sys.core.util.turnOffFocus(link);
                            me.remove();
                            if (helper.IsRefresh) {
                                location.reload(true);
                            }

                        }, //close
                        open: function (event, ui) {
                            var me = $(this);
                            helper.core.dialog.convertFormToFormAjax(me);
                            $('button.cancel').click(function () { me.dialog('close'); });
                            if (helper.IsRefresh) {
                                setTimeout(function () {
                                    location.reload(true);
                                }, helper.WaitTimeToRefresh);
                            }
                        } //open
                    }); // popDialog.load
                    me.dialog('open');
                    $('#dialog_content').css('background', 'none');
                }); // popDialog.load
            } //open
        }); // popDialog
        return popDialog;
    }, //open Link as Dialog

    closeColorbox: function () {
        $.colorbox.close();
    }, //closeColorbox

    confirmDialog: function (config) {
        var options = {
            message: '',
            title: '',
            yesText: 'Yes',
            noText: 'No',
            beforeYes: function () { },
            afterYes: function () { },
            noCallBack: function () { },
            closeCallBack: function () { },
            type: 'GET',
            url: '',
            data: ''
        };
        $.extend(options, config);
        var confirmDialog = $('<div/>', {
            id: 'confirm_dialog',
            style: "display:none",
            html: options.message
        }).appendTo('body');

        confirmDialog.dialog({
            title: options.title,
            resizable: false,
            modal: true,
            close: function (event, ui) {
                options.closeCallBack();
            },
            buttons: [
             {
                 text: options.yesText,
                 click: function () {
                     //options.afterYes();
                     if (options.url != '') {
                         $.ajax({
                             type: options.type,
                             url: options.url,
                             data: options.data,
                             success: function (rdata, textStatus, jqXHR) {
                                 confirmDialog.html(rdata);
                                 var titleE = $("#confirm_dialog h2");
                                 titleE = titleE.length > 0 ? $(titleE[0]) : null;
                                 var title = titleE != null ? titleE.html() : '';
                                 if (titleE != null) {
                                     titleE.empty();
                                 }
                                 confirmDialog.dialog('option', { title: title });
                                 //remove buttons
                                 confirmDialog.next().remove();
                                 if (helper.IsRefresh) {
                                     setTimeout(function () {
                                         location.reload(true);
                                     }, helper.WaitTimeToRefresh);
                                 }
                                 options.afterYes(confirmDialog, rdata);

                             }

                         }); //$.ajax
                     } // if (options.url != '')
                     else {
                         options.afterYes(this);
                         $(this).dialog('close');
                     }
                 }
             },
             {
                 text: options.noText,
                 click: function () {
                     options.noCallBack(confirmDialog);
                     $(this).dialog('close');
                 }
             }
            ] //end buttons
        }); //confirmDialog.dialog
    }, //confirmDialog
    //start dialogInfoWithButton
    dialogInfoWithButton: function (config) {
        var options = {
            message: '',
            title: '',
            yesText: 'Yes',
            noText: 'Close',
            beforeYes: function () { },
            afterYes: function () { },
            noCallBack: function () { },
            closeCallBack: function () { },
            type: 'GET',
            url: '',
            data: ''
        };
        $.extend(options, config);
        var infoDialog = $('<div/>', {
            id: 'confirm_dialog',
            style: "display:none",
            html: options.message
        }).appendTo('body');

        infoDialog.dialog({
            title: options.title,
            resizable: false,
            modal: true,
            close: function (event, ui) {
                // $(this).dialog('close');
                options.closeCallBack();
            },
            buttons: [
            //             {
            //                 text: options.yesText,
            //                 click: function () {
            //                     //options.afterYes();
            //                     if (options.url != '') {
            //                         window.location = options.url;
            //                     } // if (options.url != '')
            //                     else {
            //                         options.afterYes(this);
            //                         $(this).dialog('close');
            //                     }
            //                 }
            //             },
             {
             text: options.noText,
             click: function () {
                 options.noCallBack(infoDialog);
                 $(this).dialog('close');
             }
         }
            ] //end buttons
        }); //confirmDialog.dialog
    }, //end dialogInfoWithButton

    errorDialog: function (errorMessage) {
        var errorDialog = $('<div/>', { id: 'dialog_error', style: "display:none", html: '<div id="sys_errordialog" style="width:200px;">' + errorMessage + '</div>' }).appendTo('body');
        errorDialog.dialog({
            title: 'Error',
            close: function (event, ui) {
                errorDialog.remove();
                sys.core.util.turnOffFocus();
            } //close
        }); //errorDialog.dialog
    }, //errorDialog
    warningDialog: function (warningMessage) {
        var warningDialog = $('<div/>', { id: 'dialog_warning', style: "display:none", html: '<div id="sys_warningdialog" style="width:200px;">' + warningMessage + '</div>' }).appendTo('body');
        warningDialog.dialog({
            title: 'Warning',
            close: function (event, ui) {
                warningDialog.remove();
                //sys.core.util.turnOffFocus();
            } //close
        }); //errorDialog.dialog
    },
    infoDialog: function (warningMessage) {
        var warningDialog = $('<div/>',
            { id: 'dialog_info', style: "display:none", html: '<div id="sys_infodialog" style="width:200px;">' + warningMessage + '</div>' }).appendTo('body');
        warningDialog.dialog({
            title: 'Information',
            close: function (event, ui) {
                warningDialog.remove();
                //sys.core.util.turnOffFocus();
            } //close
        }); //errorDialog.dialog
    }
};

helper.core.pageMsg = {
    alert: function (msg, opts) {
        var options = {
            cls: ''
        };
        $.extend(options, opts);
        var placeHolder = $('body');
        if ($('.message_place').length > 0) {
            placeHolder = $('.message_place');
        }
        var html = [
            '<div class="alert ' + options.cls + '">',
            '<button class="close" data-dismiss="alert" type="button">x</button>',
            msg,
            '</div>'
        ];
        var div = $(html.join(''));
        div.find('.close').click(function () {
            $(this).parent().hide();
            $(this).parent().remove();
        });
        placeHolder.append(div);

    },
    error: function (msg, opts) {
        sys.core.pageMsg.alert(msg, $.extend({ cls: 'alert-error' }, opts));
    },
    fail: function (msg) {
        var cmp = $('.alert_fail');
        cmp.html(msg);
        cmp.fadeIn(1500).delay(1500).fadeOut(1500);
    },
    success: function (msg) {
        var cmp = $('.alert_success');
        cmp.html(msg);
        cmp.fadeIn(1500).delay(1500).fadeOut(1500);
    }
};
helper.IsRefresh = false;
helper.WaitTimeToRefresh = 2000;
