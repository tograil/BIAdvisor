(function () {
    setTimeout(function () {
        $('.alert-dismissible').slideUp(300);
    }, 3000);
})();

function printPanel(panel) {
    var printWindow = window.open('', '', 'height=400,width=800');
    printWindow.document.write('<html><head><title></title>');
    printWindow.document.write('<link href="/Content/bootstrap.css" rel="stylesheet" />');
    printWindow.document.write('<link href="/Content/Site.css" rel="stylesheet" />');
    printWindow.document.write('</head><body >');
    printWindow.document.write(panel.innerHTML);
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    setTimeout(function () {
        printWindow.print();
        printWindow.close();
    }, 500);
    return false;
}