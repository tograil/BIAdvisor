function disableModel() {
    if ($("#boolModel").is(':checked')) {
        $("#PayeeModel").prop("disabled", "disabled");
    }
    else
        $("#PayeeModel").removeAttr("disabled");
}

// disable controls if published, readonly and not superuser.
function disCtrls(t, u) {
    if (t === 'True' && u !== 'True') {
        //disabled everything
        $("#boolDoNotPay").prop("disabled", "disabled");
        $("#EffectiveDate").prop("disabled", "disabled");
        $("#ExpirationDate").prop("disabled", "disabled");
        $("#boolPublished").prop("disabled", "disabled");
        $("#PayIndexKey").prop("disabled", "disabled");
        $("select#BlacksmithCode").prop("disabled", "disabled");
        $("#COCPCT").prop("readonly", "readonly");
        $("#PayeeNotes").prop("readonly", "readonly");
        $("#PayeeModel").prop("disabled", "disabled");
        $("#DeductionModel").prop("disabled", "disabled");
        $("#PayeeGroup").prop("disabled", "disabled");
        $("#btnSave").prop("disabled", "disabled");
    }
}

function enableChecks(t) {
    if (t === 'True') {
        $('#boolModel').removeAttr("disabled");
        $('#boolDoNotPay').removeAttr("disabled");
        $('#boolMethodBCheck').removeAttr("disabled");
    }
}

function dealDeleted() {
    $('#popup').bPopup().close();
    toastr.success("Deal has been deleted successfully.");
    //$('#' + k).closest('tr').remove();
    //replacePayeeDeals();
    //Temporarily disabled code above an doing a full refresh
    location.reload();
}

function replacePayeeDeals() {
    $.get("/Maintenance/_PayeeDeals?id=" + $('#PayeeKey').val(), null,
        function (res) {
            $('#deals').empty();
            $('#deals').replaceWith(res);
        });
}

function showModal(e) {
    e.preventDefault();
    $('#popup').bPopup({
        modalClose: false,
        content: 'iframe',
        iframeAttr: 'width="950" height="880" scrolling="no" frameborder="0"',
        scrollBar: true,
        contentContainer: ".pcontent",
        onClose: replacePayeeDeals,
        loadUrl: '/Maintenance/_DealMaintenance?payeeDealKey=' + $(this).attr('id') + "&payeeKey=" + $('#PayeeKey').val()
        //loadUrl: $(this).attr('rel') == "Add" ? 'ProductsAdd.aspx' : ($(this).attr('rel') == "Edit" ? ('ProductsAdd.aspx?ProductID=' + $(this).attr('id')) : ('LookProductDetails.aspx'))
    }).css({
        width: '980px',
        height: '900px'
    });
}

(function () {
    $.validator.addMethod('gtDate', function (value, element, param) {
        return this.optional(element) || new Date(value) > new Date($(param).val());
    }, 'Invalid value');

    $("#formCopy").validate({
        errorLabelContainer: $("#copyError"),
        rules: {
            CopyName: "required",
            CopyEffectiveDate: "required",
            CopyExpirationDate: {
                required: true,
                gtDate: '#CopyEffectiveDate'
            }
        },
        messages: {
            CopyName: "Payee Name is required.",
            CopyEffectiveDate: "Effective Date is required.",
            CopyExpirationDate: {
                required: "Expiration Date is required.",
                gtDate: "Expiration Date should be greater than Effective Date."
            }
        }
    });

    $(document).on('click', '#btnCopy', function (e) {
        e.preventDefault();
        $('#CopyPayeeId').val($('#PayeeKey').val());
        $('#CopyPayeeType').val($('#PayeeType').val());
        $('#CopyName').val($('#Payee').val());
        $('#popup2').bPopup({
            modalClose: false,
            positionStyle: 'fixed'
        });
    });

    $("#boolModel").on("change", function () {
        disableModel();
    });

    $(document).on('click', '.view, .edit, .add', showModal);

    $("#PayeeGroup").select2({
        tags: true,
        allowClear: true,
        placeholder: " ", /*background text can be added here*/
        createTag: function (params) {
            return {
                id: params.term,
                text: params.term,
                newOption: true
            }
        }
    });

    $('#PayeeModel').select2({
        minimumResultsForSearch: Infinity,
        placeholder: ' ',
        allowClear: true,
    });

    $('#DeductionModel').select2({
        minimumResultsForSearch: Infinity,
        placeholder: ' ',
        allowClear: true,
    });

    $('#PayIndexKey').select2({
        minimumResultsForSearch: Infinity,
        dropdownAutoWidth: true,
        placeholder: ' ',
        allowClear: true,
        data: payIndexs,
        escapeMarkup: function (markup) { return markup; },
        minimumInputLength: 0,
        templateResult: formatPIResult,
        templateSelection: formatPISelection
    });

    $('select#BlacksmithCode').select2({
        minimumResultsForSearch: Infinity,
        dropdownAutoWidth: true,
        placeholder: ' ',
        allowClear: true,
        data: blacksmiths,
        escapeMarkup: function (markup) { return markup; },
        minimumInputLength: 0,
        templateResult: formatBSResult,
        templateSelection: formatBSSelection
    });

    //$(".datepicker").datepicker({ dateFormat: 'mm/dd/yy' });
    //TODO: fix mask
    //$("#COCPCT").mask("999.9999");


    //Autocomplete for table search
    //$('#PayIndexKey').tableAutocomplete({
    //    source: payIndexs,
    //    columns: [{ field: 'value', title: 'PayIndex' },
    //        { field: 'blacksmith', title: 'Blacksmith' },
    //        { field: 'coc', title: 'COC'},
    //        { field: 'basis', title: 'Basis'}],
    //    delay: 0,
    //    minLength: 0,
    //    change: function (event, ui) {
    //        //check for empty field, if yes clear other related fields
    //        if (!ui.item) {
    //            $(event.target).val("");
    //            $('#BlacksmithCode').val("");
    //            $('#COCPCT').val("");
    //            $("#BlacksmithCode").removeAttr("readonly");
    //            $("#COCPCT").removeAttr("readonly");
    //        }
    //    },
    //    focus: function (event, ui) {
    //        return false;
    //    },
    //    select: function (event, ui) {
    //        if (ui.item != undefined) {
    //            //console.log(ui.item);
    //            $('#PayIndexKey').val(ui.item.value);
    //            $('#BlacksmithCode').val(ui.item.blacksmith);
    //            $('#COCPCT').val(ui.item.coc);
    //            $("#BlacksmithCode").prop("readonly", "readonly");
    //            $("#COCPCT").prop("readonly", "readonly");
    //        }
    //        return false;
    //    }
    //}).on('focus', function () { $(this).keydown(); });

    //Autocomplete for blacksmith
    //$('#BlacksmithCode').tableAutocomplete({
    //    source: blacksmiths,
    //    columns: [{ field: 'value', title: 'Blacksmith' }, {field: 'coc', title: 'COC pct'}],
    //    delay: 0,
    //    minLength: 0,
    //    change: function (event, ui) {
    //        if (!ui.item) {
    //            $(event.target).val("");
    //            $('#COCPCT').val("");
    //            $("#COCPCT").removeAttr("readonly");
    //        }
    //    },
    //    focus: function (event, ui) {
    //        return false;
    //    },
    //    select: function (event, ui) {
    //        if (ui.item != undefined) {
    //            console.log(ui.item);
    //            $('#BlacksmithCode').val(ui.item.value);
    //            $('#COCPCT').val(ui.item.coc);
    //            $("#COCPCT").prop("readonly", "readonly");
    //        }
    //        return false;
    //    }
    //}).on('focus', function () { $(this).keydown(); });


    function formatPIResult(item) {
        if (item.loading) return item.text;
        if (item.disabled == true) {
            return '<div class="s2-result clearfix"><div class="s2-row s2-head"><span class="s2-item pi-pi">PayIndex</span>' +
            '<span class="s2-item pi-bs">Blacksmith</span><span class="s2-item pi-coc">COC</span>' +
            '<span class="s2-item pi-basis">Basis</span></div></div>';
        }

        return '<div class="s2-result clearfix"><div class="s2-row"><span class="s2-item pi-pi">' + item.text + '</span>' +
            '<span class="s2-item pi-bs">' + item.blacksmith + '</span><span class="s2-item pi-coc">' + item.coc + '</span>' +
            '<span class="s2-item pi-basis">' + item.basis + '</span></div></div>';
    }

    function formatPISelection(item) {
        if (item.blacksmith != undefined) {
            $('input[type=hidden]#BlacksmithCode').val(item.blacksmith);
            $('select#BlacksmithCode').val(item.blacksmith).trigger("change");
            $('select#BlacksmithCode').prop('disabled', 'disabled');
        }
        if (item.coc != undefined) {
            $('#COCPCT').val(item.coc);
            $('#COCPCT').prop('readonly', 'readonly');
            $('#COCPCT').removeClass('input-validation-error');
        }
        if (item.id == "") {
            $('input[type=hidden]#BlacksmithCode').val(item.blacksmith);
            $('select#BlacksmithCode').val(item.blacksmith).trigger("change");
            $('select#BlacksmithCode').removeAttr('disabled');
            $('#COCPCT').val(item.coc);
            $('#COCPCT').removeAttr('readonly');
        }
        return item.text;
    }

    function formatBSResult(item) {
        if (item.loading) return item.text;
        if (item.disabled == true) {
            return '<div class="s2-result clearfix"><div class="s2-row s2-head"><span class="s2-item pi-bs">Blacksmith</span>' +
                '<span class="s2-item pi-coc">COC</span></div></div>';
        }

        return '<div class="s2-result clearfix"><div class="s2-row"><span class="s2-item pi-bs">' + item.text + '</span>' +
            '<span class="s2-item pi-coc">' + item.coc + '</span></div></div>';
    }

    function formatBSSelection(item) {
        if (item.coc != undefined) {
            $('input[type=hidden]#BlacksmithCode').val(item.id);
            $('#COCPCT').val(item.coc);
            $('#COCPCT').prop('readonly', 'readonly');
            $('#COCPCT').removeClass('input-validation-error');
        }
        if (item.id == "") {
            $('#COCPCT').val(item.coc);
            $('#COCPCT').removeAttr('readonly');
        }
        return item.text;
    }
})();