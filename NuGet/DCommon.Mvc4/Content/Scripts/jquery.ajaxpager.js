(function ($) {
    // count instances	
    var counter = 0;

    var ajaxPager = function (input, opt) {
        var self = this
            , $container = $(input)
            , id = ++counter
            // make a copy of the options and use the metadata if provided
            , options = $.extend({}, $.fn.ajaxPager.defaults, opt, (!!$.metadata ? $container.metadata() : {}));

        if ($.data($container[0], "ajaxPager")) return;

        self.options = options;
        self.container = $container[0];
        // store a reference to this marquee
        $.data($container[0], "ajaxPager", self);

        var registerCallback = function () {
            var $container = $(this.container);
            var opt = this.options;
            if (opt.allowSort) {
                $('thead th[data-ajaxpager=true][class^=sorting]', $container).bind('click.ajaxPager', sortMemberChanged);
            }
            $('.pagination a[data-ajaxpager=true]', $container).bind('click.ajaxPager', pageNumberChanged);
            if (opt.registerBehavior && $.isFunction(opt.registerBehavior)) {
                opt.registerBehavior($container);
            }
        };

        var sortMemberChanged = function (e) {
            var $this = $(this);
            self.options.sort = [{
                field: $this.attr('data-sortmember'),
                direction: $this.is('.sorting_asc') ? 'descending' : 'ascending'
            }];
            self.ajaxRequest();
        };

        var pageNumberChanged = function (e) {
            var $this = $(this);
            self.options.pageNumber = $this.attr('data-pagenumber');
            self.ajaxRequest();
            e.preventDefault();
        };

        this.ajaxRequest = function (d) {
            var opt = this.options;
            var defaults = {
                pageNumber: opt.pageNumber,
                pageSize: opt.pageSize,
                sort: opt.sort
            };
            if (!defaults.sort)
                defaults.sort = [{
                    field: $('thead th[class!=sorting]', $(this.container)).attr('data-sortmember') || 'Id',
                    direction: 'ascending'
                }];
            var requestData = $.extend({}, defaults, d);
            if (opt.beforeAjaxRequest && $.isFunction(opt.beforeAjaxRequest)) {
                opt.beforeAjaxRequest(requestData);
            }

            $.ajax({
                url: opt.dataSourceUrl,
                data: JSON.stringify(requestData),
                type: 'POST',
                contentType: 'application/json',
                success: function (data, textStatus, jqXHR) {
                    self.destroy();
                    $(self.container).html(data);
                    registerCallback.apply(self);
                    if (opt.afterAjaxRequest && $.isFunction(opt.afterAjaxRequest)) {
                        opt.afterAjaxRequest(data, textStatus, jqXHR);
                    }
                    //regularNotice("提示信息", "列表已刷新");
                }
            });
        };

        //this will destory the previous html
        this.destroy = function () {
            // remove behaviors
            var $container = $(self.container);
            $('thead th[data-ajaxpager=true][class^=sorting]', $container).unbind('.ajaxPager');
            $('.pagination a[data-ajaxpager=true]', $container).unbind('.ajaxPager');
            //custom destroy
            if (this.options.destroy && $.isFunction(opt.destroy)) {
                opt.destroy($container);
            }
            //remove children
            $container.children().remove();
        };

        registerCallback.apply(this);
    };

    $.fn.ajaxPager = function (options) {
        var method = typeof arguments[0] == "string" && arguments[0];
        var args = method && Array.prototype.slice.call(arguments, 1) || arguments;
        // get a reference to the first ajaxPager found
        var self = (this.length == 0) ? null : $.data(this[0], "ajaxPager");

        // if a method is supplied, execute it for non-empty results
        if (self && method && this.length) {

            // if request a copy of the object, return it			
            if (method.toLowerCase() == "object") return self;
                // if method is defined, run it and return either it's results or the chain
            else if (self[method]) {
                // define a result variable to return to the jQuery chain
                var result;
                this.each(function (i) {
                    // apply the method to the current element
                    var r = $.data(this, "ajaxPager")[method].apply(self, args);
                    // if first iteration we need to check if we're done processing or need to add it to the jquery chain
                    if (i == 0 && r) {
                        // if this is a jQuery item, we need to store them in a collection
                        if (!!r.jquery) {
                            result = $([]).add(r);
                            // otherwise, just store the result and stop executing
                        } else {
                            result = r;
                            // since we're a non-jQuery item, just cancel processing further items
                            return false;
                        }
                        // keep adding jQuery objects to the results
                    } else if (!!r && !!r.jquery) {
                        result = result.add(r);
                    }
                });

                // return either the results (which could be a jQuery object) or the original chain
                return result || this;
                // everything else, return the chain
            } else return this;
            // initializing request (only do if ajaxPager not already initialized)
        } else {
            // create a new ajaxPager for each object found
            return this.each(function () {
                new ajaxPager(this, options);
            });
        };
    };

    $.fn.ajaxPager.defaults = {
        pageNumber: 0,
        pageSize: 20,
        allowSort: true,
        sortMember: '',
        dataSourceUrl: '',
        sortDirection: 'descending', //'ascending',
        beforeAjaxRequest: null,
        afterAjaxRequest: null,
        registerBehavior: null,
        destroy: null
    };
})(jQuery);