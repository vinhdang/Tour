$(document).ready(function () {
    var SalePrice = $("#SalePrice").text().replace('.', '').replace('.', '');
    var quantity = $("#Quantity :selected").text();
    if (quantity != null && quantity > 1) {
        $("#priceformat").text(SalePrice * quantity).formatCurrency();
        var priceformat = $("#priceformat").text();
        $("#totalPrice").text(priceformat);
    } else {
        $("#priceformat").text(SalePrice).formatCurrency();
        var priceformat = $("#priceformat").text();
        $("#totalPrice").text(priceformat);
    }
    $('#Quantity').change(function () {
        var quantity = $("#Quantity :selected").text();
        var SalePrice = $("#SalePrice").text().replace('.', '');
        if (SalePrice != null && SalePrice > 0) {
            $("#priceformat").text(SalePrice * quantity).formatCurrency();
            var priceformat = $("#priceformat").text();
            $("#totalPrice").text(priceformat);
        }

    });
    $(".choosePaymentType").bind("click", function () {
        DoChoosePaymentType($(this));
        return false;
    });
    function DoChoosePaymentType(divClick) {
        $(".choosePaymentType").removeClass("active");
        var currentId = divClick.attr('id').toString();
        divClick.addClass("active");
        switch (currentId) {
            case "PayByCash": //we don't have any Payment Type yet, 
                $("input[name=PaymentType]").val(1);
                break;
            case "PayByATM":
                $("input[name=PaymentType]").val(2);
                break;
            case "PayByLocalCard":
                $("input[name=PaymentType]").val(3);
                break;
        }
        //   alert($("input[name = PaymentType]").val());

    }
});
