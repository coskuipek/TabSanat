function CalculatePrice(_pricebig, _pricesmall) {
    var big = 0;
    var small = 0.00;
    if (_pricebig !== "") {
        big = parseFloat(_pricebig, 2);
    }
    if (_pricesmall !== "") {
        if (_pricesmall.length == 1) {
            small = (parseFloat(_pricesmall, 2) / 10);
        }
        else {
            small = (parseFloat(_pricesmall, 2) / 100);
        }
    }
    var total = big + small;
    total = total.toString().replace(".", ",")
    return total;
}