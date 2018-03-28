(function ($) {
    var Operate = function (element, options) {
        this.options = options;
        this.$element = $(element).delegate('button[data-action]', 'click.operate', this.execute);
    };

    Operate.prototype.execute = function (e) {
        var operate = $(e.delegateTarget).data("operate");
        if (!operate || !operate.options)
            return;
        var $target = $(e.target);
        var options = operate.options;
        if (!$target.is("button")) {
            $target = $target.closest("button[data-action]", operate.$element);
        }
        var action = $target.attr('data-action');
        if (!action)
            return;

        if (!options[action]) {
            var customFn = options.custom;
            if (customFn) { customFn.apply($target, [action]); }
            return;
        }

        if (action == 'refresh') {
            var refreshFn = options.refresh;
            if (refreshFn) { refreshFn.apply($target); }
            return;
        }
        if (action == 'add') {
            var addFn = options.add;
            if (addFn) { addFn.apply($target); }
            return;
        }

        var ids = options.getSelectedIds();
        if (!ids.length) {
            bootbox.alert("请先选中您要操作的数据记录。");
            return;
        }

        if (ids.length > 1) {
            bootbox.alert("该操作只允许选中一条数据记录。");
            return;
        }

        if (action == 'delete') {
            var deleteFn = options["delete"];
            if (deleteFn) {
                bootbox.confirm("您确实需要删除选中的数据吗？", function (confirmed) {
                    if (confirmed) { deleteFn.apply($target, [ids[0]]); }
                });
            }
            return;
        }
        if (action == 'edit') {
            var editFn = options.edit;
            if (editFn) { editFn.apply($target, [ids[0]]); }
            return;
        }
        if (action == 'detail') {
            var detailFn = options.detail;
            if (detailFn) { detailFn.apply($target, [ids[0]]); }
            return;
        }
        
        var fn = options[action];
        if (fn) { fn.apply($target, [action, ids]); }
    };

    $.fn.operate = function (option) {
        return this.each(function () {
            var $this = $(this),
                data = $this.data('operate'),
                options = $.extend({}, $.fn.operate.defaults, $this.data(), typeof option == 'object' && option);

            if (!data)
                $this.data('operate', (data = new Operate(this, options)));
        });
    };

    $.fn.operate.defaults = {
        'getSelectedIds': function () { },
        'add': function () { },
        'delete': function () { },
        'detail': function () { },
        'edit': function () { },
        'custom': function () { }
    };

    $.fn.getSelectedIds = function (fn) {
        var ids = [];
        this.each(function () {
            var v = fn ? fn() : $(this).val();
            ids.push(v);
        });
        return ids;
    };
})(jQuery);