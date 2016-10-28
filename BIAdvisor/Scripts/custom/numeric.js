$.fn.Numeric = function (decimal, min, max) {
    var d = (decimal === true) ? decimal : false;
    $(this)
		.keypress(function (e) {
		    if (d) {
		        return (e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57) || (($(this).val().indexOf('.') > -1) && e.which == 46)) ? false : true;			// remove (e.which != 46) for non-decimal values
		    }
		    else {
		        return (e.which != 0 && (e.which < 48 || e.which > 57)) ? false : true;
		    }
		})
		.blur(function () {
		    var val = this.value;
		    if (val !== "") {
		        this.value = (max != null && val > max) ? max : this.value;
		        this.value = (min != null && val < min) ? min : this.value;
		    }
		})
};