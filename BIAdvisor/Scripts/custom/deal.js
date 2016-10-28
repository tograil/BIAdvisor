(function () {

    function formatCriteriaResult(item) {
        if (item.loading) return item.text;
        if (item.disabled == true) {
            return '<div class="s2-result clearfix"><div class="s2-row s2-head"><span class="s2-item car">Criteria</span>' +
            '<span class="s2-item prod">Priority</span></div></div>';
        }

        return '<div class="s2-result clearfix"><div class="s2-row '+ (item.active == "N" ? 'grey' : '') + '"><span class="s2-item car">' + item.id + '</span>' +
            '<span class="s2-item prod">' + item.priority + '</span></div></div>';
    }

    function formatCriteriaSelection(item) {
        return item.id;
    }
    
    function formatCarrierResult(item) {
        if (item.loading) return item.text;
        if (item.disabled == true) {
            return '<div class="s2-result clearfix"><div class="s2-row s2-head"><span class="s2-item car">Carrier</span>' +
            '<span class="s2-item prod">Product Line</span></div></div>';
        }

        return '<div class="s2-result clearfix"><div class="s2-row"><span class="s2-item car">' + item.text + '</span>' +
            '<span class="s2-item prod">' + item.line + '</span></div></div>';
    }

    function formatCarrierSelection(item) {
        $('#ProductLine').val(item.line);
        return item.text;
    }

    function CarrierChanged() {
        var cid = $('#CarrierID').val();
        if (cid && cid != "") {
            //Rebuild Product dropdown
            $.getJSON('/Maintenance/GetProductItems', {
                carrierID: cid,
                productLine: $('#ProductLine').val()
            }, function (data) {
                var prod = $("#ProductID");
                var val = prod.val();
                prod.empty();
                prod.append($('<option></option>').val('').text(''));
                prod.select2({
                    minimumResultsForSearch: Infinity,
                    minimumInputLength: 0,
                    dropdownAutoWidth: true,
                    allowClear: true,
                    placeholder: " ",
                    data: data
                });
                prod.val(val).trigger("change");
            });

            //Rebuild Carrier Level & Virtual Carrier
            $.getJSON('/Maintenance/GetCarrierLevel', {
                carrierID: cid,
                productLine: $('#ProductLine').val()
            }, function (data) {
                var car = $("#CarrierLevel");
                car.empty();
                car.append($('<option></option>').val('').text(''));
                car.select2({
                    minimumInputLength: 0,
                    dropdownAutoWidth: true,
                    allowClear: true,
                    placeholder: " ",
                    tags: true,
                    data: data
                });

                var vcar = $("#VirtualCarrierLevel");
                vcar.empty();
                vcar.append($('<option></option>').val('').text(''));
                vcar.select2({
                    minimumInputLength: 0,
                    dropdownAutoWidth: true,
                    allowClear: true,
                    placeholder: " ",
                    tags: true,
                    data: data
                });
            });
        }
    }

    function CriteriaChanged() {
        ProcessCriteriaChange();

        //Rebuild CalculationCode dropdown
        $.getJSON('/Maintenance/GetCalCodeItems', {
            payeeType: ($('#PayeeType').val() || '').trim(),
            dealCriteria: ($('#DealCriteria').val() || '').trim()
        }, function (data) {
            var cal = $("#CalculationID");
            var val = cal.val();
            cal.empty();
            cal.append($('<option></option>').val('').text(''));
            initCalCode(data);
            if (data.map(function (a) { return a.id; }).indexOf(val) < 0) {
                $('#CalculationDescription').val('');
                ProcessCalculationChange();
            }
            else {
                cal.val(val).trigger("change");
            }
        });
    }

    function initCalCode(c) {
        $("#CalculationID").select2({
            minimumResultsForSearch: Infinity,
            minimumInputLength: 0,
            dropdownAutoWidth: true,
            allowClear: true,
            placeholder: " ", /*background text can be added here*/
            data: c
        }).on("select2:select", function (e) {
            var item = e.params.data;
            $('#CalculationDescription').val(item.desc);
            //enable/disabled controls
            if (item != undefined && item.caljson != undefined) {
                ProcessCalculationChange(JSON.parse(item.caljson));
            }
        }).on("select2:unselect", function (e) {
            $('#CalculationDescription').val('');
            ProcessCalculationChange();
        });
    }

    function ProcessCriteriaChange() {
        enableMe('#DealCriteria', true);
        var disableString = "#CarrierID,#ProductID,#Wholesaler,#WholesalerRole,#ProductAttribute,#CarrierAttribute," +
                "#BrokerDealer,#BrokerDealerInHierarchy,#BrokerDealerAttribute,#Channel,#Internal,#ProducerID,#MSA," +
                "#Recruiter,#MPPartner,#RecruiterAttribute,#PayeeGroup,#StatementDate,#SubmitDate";

        var controlsToEnable = [];
        var criteriaCombination = $("#DealCriteria").val();

        //Rebuild Wholesaler dropdown
        $.getJSON('/Maintenance/GetWholeSalerItems', {
            category: criteriaCombination.indexOf("AFMO") < 0 ? "Wholesaler" : "AFMO"
        }, function (data) {
            var wi = $("#Wholesaler");
            var val = wi.val();
            wi.empty();
            wi.append($('<option></option>').val('').text(''));
            wi.select2({
                minimumResultsForSearch: Infinity,
                minimumInputLength: 0,
                dropdownAutoWidth: true,
                allowClear: true,
                placeholder: " ",
                data: data
            });
            wi.val(val).trigger("change");
        });

        if (criteriaCombination) {
            var res = criteriaCombination.split("+");
            for (var i = 0; i < res.length; i++) {
                var focusControl = res[i];
                switch (focusControl) {
                    case "AFMO":
                        controlsToEnable.push('#Wholesaler');
                        break;
                    case "BDAttribute":
                        controlsToEnable.push('#BrokerDealerAttribute');
                        break;
                    case "BDInHierarchy":
                        controlsToEnable.push('#BrokerDealerInHierarchy');
                        break;
                    case "Product":
                    case "Carrier":
                    case "Producer":
                        controlsToEnable.push('#' + focusControl + 'ID');
                        break;
                    default:
                        controlsToEnable.push('#' + focusControl);
                        break;
                }
            }
        }

        //loop through each controlsToDisable
        //check if it exist in controlsToEnable, then keep it as is
        //if not, clear and disable it.
        disableString.split(',').forEach(function (val) {
            if (controlsToEnable.indexOf(val) < 0)
                disableMe(val);
            else
                enableMe(val, true);
        });
    }

    function ProcessCalculationChange(json) {
        var allControls = "#CarrierLevel,#VirtualCarrierLevel,#LOA,#Street,#PayIndexKey," +
            '#Amount1,#Amount2,#Amount3,#Rate1,#Rate2,#Rate3,#PctCarrierLevel,#PctWholesalerNet,#RecruiterDebit,#RecruiterCredit,' +
            '#ExpDelta,#ContingentBonus,#YearEndBonus,#DealCap,#DeductionModel,#Directive,#PctGross,#PctPremium,#PctOverride';
        var enabledControls = [];
        //console.log(json);
        json = json && json.validation ? json : { validation: [] };
        //For each value in json, enable controls and and add them to enabled array
        for (var i = 0; i < json.validation.length; i++) {
            if (json.validation[i].type == "required") {
                enabledControls.push(json.validation[i].control);
                enableMe(json.validation[i].control, true);
            } else if (json.validation[i].type == "optional") {
                enabledControls.push(json.validation[i].control);
                enableMe(json.validation[i].control, false);
            }
        }
        //loop through all controls, and disable the ones not in enabled array
        allControls.split(',').forEach(function (item) {
            if (enabledControls.indexOf(item) < 0)
                disableMe(item);
        });
    }

    //enable control and make it required if isReq = true
    function enableMe(c, isReq) {
        isReq = isReq == true ? true : false;
        $(c).removeAttr("disabled");
        if (isReq) {
            $(c).prev().addClass("required");
            $(c).next('.select2').addClass("required");
            $(c).prop("required", "required");
        }
        else {
            $(c).prev().addClass("optional");
        }
    }

    //disable and clear control
    function disableMe(c) {
        $(c).prop("disabled", "disabled");
        $(c).prev().removeClass("required optional");
        $(c).removeClass('error');
        $(c).next("span").removeClass('error');
        $(c).removeAttr("required");
        $(c).val('').trigger("change");
    }

    $(document).ready(function () {
        var criteriaByPriority = criteria.slice();
        criteriaByPriority.forEach(function (c) { c.text = c.priority });
        criteriaByPriority.sort(function (a, b) { return a.priority - b.priority });

        initDealCriteria(criteria);
        //Hack to set value for Select2 on load. It should have data-s2val though.
        //This is for table dropdowns, which can't be set in usual way.
        $("#DealCriteria").val($("#DealCriteria").data('s2val')).trigger("change");
        $("#CarrierID").val($("#CarrierID").data('s2val')).trigger("change");

        //sort by alpha
        $(".sortaz").on('click', function () {
            $(".sort12").show();
            $(".sortaz").hide();

            initDealCriteria(criteria);
        });

        //sort by num
        $(".sort12").on('click', function () {
            $(".sort12").hide();
            $(".sortaz").show();

            initDealCriteria(criteriaByPriority);
        });
        
        ProcessCriteriaChange();
        initCalCode(calCode);
        ProcessCalculationChange(JSON.parse($('#CalJson').val() || "[]"));

        $("#DealCriteria").on("change", CriteriaChanged);

        $('#CarrierID').on("change", CarrierChanged);

        $(".decimal").Numeric(true);

        $('.deleteDeal').on('click', function () {
            if ($("#PayeeDealKey").val() > 0) {
                if (confirm('Are you sure you want to delete this deal?')) {
                    //Ajax delete and parent reload
                    $.post("/Maintenance/DeleteDeal", $('#DealMaintenance').serialize(), function (res) {
                        if (res === true) {
                            parent.dealDeleted();
                        }
                        else {
                            parent.toastr.error('Deal could not be deleted!');
                        }
                    });
                };
            }
        });

        $('#DealMaintenance')
            .validate({
                rules: {
                    StatementDate: "date",
                    SubmitDate: "date",
                    DealCap: {
                        number: true,
                        max: 9999
                    },
                    PctGross: {
                        number: true,
                        max: 9999
                    },
                    PctPremium: {
                        number: true,
                        max: 9999
                    },
                    PctOverride: {
                        number: true,
                        max: 9999
                    },
                    PctCarrierLevel: {
                        number: true,
                        max: 9999
                    },
                    PctWholesalerNet: {
                        number: true,
                        max: 9999
                    },
                    RecruiterDebit: {
                        number: true,
                        max: 9999
                    },
                    RecruiterCredit: {
                        number: true,
                        max: 9999
                    },
                    ExpDelta: {
                        number: true,
                        max: 9999
                    },
                    ContingentBonus: {
                        number: true,
                        max: 9999
                    },
                    YearEndBonus: {
                        number: true,
                        max: 9999
                    },
                    Amount1: {
                        number: true,
                        max: 9999
                    },
                    Amount2: {
                        number: true,
                        max: 9999
                    },
                    Amount3: {
                        number: true,
                        max: 9999
                    },
                    Rate1: {
                        number: true,
                        max: 9999
                    },
                    Rate2: {
                        number: true,
                        max: 9999
                    },
                    Rate3: {
                        number: true,
                        max: 9999
                    }
                },
                errorPlacement: function () { },
                // set the errorClass as a random string to prevent label disappearing when valid
                errorClass: "error",
                // use highlight and unhighlight
                highlight: function (element, errorClass, validClass) {
                    $(element).addClass("error").next("span").addClass("error");
                },
                unhighlight: function (element, errorClass, validClass) {
                    $(element).removeClass("error").next("span").removeClass("error");
                }
            });

        $('#DealMaintenance').dirtyForms().on("dirty.dirtyforms", function () {
            $(this).find('#btnsave').removeAttr('disabled');
        }).on("clean.dirtyforms", function () {
            $(this).find('#btnsave').attr('disabled', 'disabled');
        });

        if ($("#PayeeDealKey").val() == "undefined") {
            $("#PayeeDealKey").val(0);
        }
    });

    function initDealCriteria(c) {
        var dc = $("#DealCriteria");
        var val = dc.val();
        dc.empty();
        dc.append($('<option></option>').val('').text(''));

        dc.select2({
            minimumResultsForSearch: Infinity,
            dropdownAutoWidth: true,
            placeholder: ' ',
            allowClear: true,
            data: c,
            escapeMarkup: function (markup) { return markup; },
            minimumInputLength: 0,
            templateResult: formatCriteriaResult,
            templateSelection: formatCriteriaSelection
        });
        dc.val(val).trigger("change");
    }

    $('#CarrierID').select2({
        minimumResultsForSearch: Infinity,
        dropdownAutoWidth: true,
        placeholder: ' ',
        allowClear: true,
        data: carriers,
        escapeMarkup: function (markup) { return markup; },
        minimumInputLength: 0,
        templateResult: formatCarrierResult,
        templateSelection: formatCarrierSelection
    });

    $(".s2").select2({
        minimumResultsForSearch: Infinity,
        minimumInputLength: 0,
        dropdownAutoWidth: true,
        allowClear: true,
        placeholder: " "
    });

    $(".s2tag").select2({
        minimumInputLength: 0,
        dropdownAutoWidth: true,
        allowClear: true,
        placeholder: " ",
        tags: true
    });
})();