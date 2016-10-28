(function () {
    $.validator.addMethod('gtDate', function (value, element, param) {
        return this.optional(element) || new Date(value) > new Date($(param).val());
    }, 'Invalid value');
    $.fn.clearValidation = function () { var v = $(this).validate(); $('[name]', this).each(function () { v.successList.push(this); v.showErrors(); }); v.resetForm(); v.reset(); };

    $("#Timeframe").select2({
        minimumResultsForSearch: Infinity
    });

    $("#Name").select2({
        dropdownAutoWidth: true,
        tags: true,
        allowClear: true,
        placeholder: " ", /*background text can be added here*/
        //createTag: function (params) {
        //    return {
        //        id: params.term,
        //        text: params.term,
        //        newOption: true
        //    }
        //},
        ajax: {
            url: '/Home/GetPayees',
            dataType: 'json',
            delay: 200,
            data: function (params) {
                return {
                    term: params.term,
                    type: ($('#Type').val() || '').trim().split(":")[0],
                    role: ($('#Role').val() || '').trim(),
                    timeframe: ($('#Timeframe').val() || '').trim()
                };
            },
            processResults: function (data) {
                return {
                    results: data
                    //results: $.map(data, function (obj) {
                    //    obj = obj || {};
                    //    obj.id = obj.id || obj.value;
                    //    obj.text = obj.value;
                    //    return obj;
                    //})
                };
            },
            cache: true
        }
    });

    //Search Type Dropdown
    $('#Type').select2({
        minimumResultsForSearch: Infinity,
        dropdownAutoWidth: true,
        placeholder: ' ',
        allowClear: true,
        //ajax: {
        //    url: '/Home/GetTypeRole',
        //    dataType: 'json',
        //    delay: 200,
        //    processResults: function (data, params) {
        //        return {
        //            results: data
        //        };
        //    },
        //    cache: true
        //},
        data: types,
        escapeMarkup: function (markup) { return markup; },
        minimumInputLength: 0,
        templateResult: formatResult,
        templateSelection: formatType
    });

    //Modal Type Dropdown
    $('#PayeeType').select2({
        minimumResultsForSearch: Infinity,
        dropdownParent: $("#popup1"),
        dropdownAutoWidth: true,
        placeholder: ' ',
        allowClear: true,
        data: types,
        escapeMarkup: function (markup) { return markup; },
        minimumInputLength: 0,
        templateResult: formatResult,
        templateSelection: formatPayeeType
    });

    $(document).ready(function () {
        $(".s2pop").val($(".s2pop").data('s2val')).trigger("change");
        $('#boolModel').val('false');

        //Toggle Delete icon on clicking header icon
        $('.del.tog').on('click', function () {
            $('.del').slice(1).toggle();
            setDeleteToggle();
        });

        $('.refresh').on('click', function (e) {
            $('#Name').val('');
            $('#Type').val('');
            $('#Role').val('');
            //$('#Timeframe').val('Current Deals Only');
            $('#boolModel').val('true');
            $('#formSearch').submit();
        });

        $('#prntResults').on('click', function () {
            printPanel($('#results')[0]);
        });

        //Delete Popup
        //Update name and dates on delete form
        $('.del').slice(1).on('click', function (e) {
            e.preventDefault();
            var tr = e.target.parentElement.parentElement;
            $('#NameDel').val($(tr).find('td').eq(3).text());
            $('#EfDel').val($(tr).find('td').eq(6).text());
            $('#ExDel').val($(tr).find('td').eq(7).text());
            $('#PayeeDel').val($(e.target).data('id'));
            $('#popup2').bPopup({
                modalClose: false,
                positionStyle: 'fixed'
            });
        });

        //Add Popup
        //Update name, type and role on add form
        $('.add').on('click', function (e) {
            e.preventDefault();
            var tr = $(e.target).closest('table').find('tbody tr').eq(0);
            var n = $(tr).find('td').eq(3).text();
            var t = $(tr).find('td').eq(4).text();
            var r = $(tr).find('td').eq(5).text();
            $('#PayeeName').val(n);
            $('#PayeeType').val(t + ':' + r).trigger("change");

            $('#PayeeRole').val(r);
            var $eff = $('#EffectiveDate');
            $eff.val($eff.data('default'));
            var $exp = $('#ExpirationDate');
            $exp.val($exp.data('default'));

            $('#popup1').bPopup({
                modalClose: false,
                positionStyle: 'fixed',
                onClose: function () {
                    $('#PayeeName').removeClass("error");
                    $('#PayeeType').next('.select2').removeClass("error");
                    $('#EffectiveDate').removeClass("error");
                    $('#ExpirationDate').removeClass("error");
                }
            });
        });

        getDeleteToggle();

        $("#formAdd").validate({
            submitHandler: function (form) {
                $.post("/Home/CheckOverlapDates",
                    {
                        PayeeName: $('#PayeeName').val(),
                        PayeeType: $('#PayeeType').val(),
                        EffectiveDate: $('#EffectiveDate').val(),
                        ExpirationDate: $('#ExpirationDate').val()
                    },
                    function (res) {
                        if (res === true) {
                            $('#popup1').bPopup().close();
                            form.submit();
                        }
                        else {
                            toastr.error("Payee already has a deal within these dates.", "Failed");
                        }
                    });
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass(errorClass).removeClass(validClass).next('.select2').addClass("error");
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass(errorClass).addClass(validClass).next('.select2').removeClass("error");
            },
            errorPlacement: function () { },
            rules: {
                PayeeName: "required",
                PayeeType: "required",
                EffectiveDate: "required date",
                ExpirationDate: {
                    date: true,
                    required: true,
                    gtDate: '#EffectiveDate'
                }
            },
            messages: {
                ExpirationDate: {
                    gtDate: "Expiration Date should be greater than Effective Date"
                }
            }
        });
    });

    function formatResult(item) {
        if (item.loading) return item.text;
        if (item.disabled == true) {
            return '<div class="s2-result clearfix"><div class="s2-row s2-head"><span class="s2-item type">Type</span>' +
                '<span class="s2-item role">Role</span></div></div>';
        }

        return '<div class="s2-result clearfix"><div class="s2-row"><span class="s2-item type">' + item.text + '</span>' +
                '<span class="s2-item role">' + item.role + '</span></div></div>';
    };

    function formatType(item) {
        if (item.role != undefined || item.id == "") {
            $('#Role').val(item.role);
        }
        return item.text;
    };

    function formatPayeeType(item) {
        if (item.role != undefined || item.id == "") {
            $('#PayeeRole').val(item.role);
        }
        return item.text;
    };

    function setDeleteToggle() {
        if (typeof (Storage) !== "undefined") {
            var d = JSON.parse(localStorage.getItem('showDel'));
            localStorage.setItem('showDel', d === null ? false : !d);
        }
    };

    function getDeleteToggle() {
        if (typeof (Storage) !== "undefined") {
            var tog = JSON.parse(localStorage.getItem('showDel'));
            if (tog === true) {
                $('.del').slice(1).show();
            }
        }
    };

})();