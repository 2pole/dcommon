(function ($) {
    var count = 0;
    var EntityEditor = function (element, options) {
        count++;
        var $element = $(element);
        var $footer = $(" > div.modal-footer", $element);
        var self = this;
        var editElement = $("[data-action=edit]", $footer).click(function () {
            var args = [$(this).attr('data-id')];
            self.edit.apply(self, args);
        });
        var saveElement = $("[data-action=save]", $footer).click(function () {
            self.save.apply(self, []);
        });
        //set id
        var id = $element.attr("id");
        if (!id) {
            id = "entity-editor-" + count;
            $element.attr("id", id);
        }

        this.options = options;
        this.$editor = $element;
        this.$editBtn = editElement;
        this.$saveBtn = saveElement;
        this.$body = $(" > div.modal-body", $element);
        this.viewState = "";
    };

    EntityEditor.prototype.detail = function (id) {
        var option = this.options;
        this.viewState = "detail";
        this.$editBtn.attr("data-id", id).show();
        this.$saveBtn.hide();
        this.showModal();
        this.$body.load(option.detailUrl, { id: id }, function () {
            
        });
    };

    EntityEditor.prototype.add = function (data) {
        this.viewState = 'add';
        var option = this.options;
        this.$editBtn.hide();
        this.$saveBtn.show();
        this.showModal();
        var self = this;
        this.$body.load(option.addUrl, data, function () {
            self.loadEditCallback.apply(self);
        });
    };

    EntityEditor.prototype.edit = function (id) {
        this.viewState = 'edit';
        var option = this.options;
        this.$editBtn.hide();
        this.$saveBtn.show();
        this.showModal();
        var self = this;
        this.$body.load(option.editUrl, { id: id }, function () {
            self.loadEditCallback.apply(self);
        });
    };

    EntityEditor.prototype.save = function () {
        var $form = $("form", this.$body);
        $form.submit();
    };

    EntityEditor.prototype.saveSuccess = function (result) {
        var opt = this.options;
        if (result.Success) {
            this.clearValidationMessage();
            if (this.viewState == "add") {
                this.viewState = 'edit';
                this.setHeaderHtml();
                $("#Id").val(result.Data);
            }
            if (opt.saveCallback) {
                opt.saveCallback();
            }
        } else {
            this.setValidationMessage(result.Message);
        }
        this.showSaveMessage(result);
    };

    EntityEditor.prototype.showSaveMessage = function (result) {
        //var m = result.Success ? "成功" : "失败";
        //var message = result.Message ? result.Message : "保存" + this.options.headerText + m;
        //$.gritter.add({
        //    title: "保存" + m,
        //    text: message,
        //    sticky: false
        //});
        bootbox.alert(result.Message);
    };

    EntityEditor.prototype.clearValidationMessage = function () {
        this.setValidationMessage("<ul><li style='display:none'></li></ul>");
    };

    EntityEditor.prototype.setValidationMessage = function (message) {
        var $valSummary = $("div[data-valmsg-summary]", this.$body);
        var $msg = $(message);
        if ($msg.is("ul")) {
            $valSummary.html($msg);
        } else if ($msg.is("li")) {
            $("> ul", $valSummary).html($msg);
        } else {
            $("> ul", $valSummary).html("<li>" + message + "</li>");
        }
        $valSummary.addClass("validation-summary-errors");
    };

    EntityEditor.prototype.getSaveSuccessFunction = function () {
        var id = this.$editor.attr("id");
        var callback = "$('#" + id + "').entityEditor('saveSuccess', data);";
        return callback;
    };

    EntityEditor.prototype.loadEditCallback = function () {
        var callback = this.getSaveSuccessFunction();
        $("form", this.$body).attr("data-ajax-success", callback).attr("action", this.options.saveUrl);
        jQuery.validator.unobtrusive.parse(this.$body);
        var cb = this.options.loadEditCallback;
        if (cb != null) {
            cb.apply(this, [this.viewState]);
        }
    };

    EntityEditor.prototype.setHeaderHtml = function () {
        var opt = this.options;
        var operation;
        switch (this.viewState) {
            case "detail":
                operation = "查看";
                break;
            case "edit":
                operation = "修改";
                break;
            case "add":
                operation = "新增";
                break;
            default:
                operation = "";
                break;
        }

        this.setHeader(opt.headerIconCss, operation + opt.headerText);
    };

    EntityEditor.prototype.setHeader = function(css, header) {
        var html = '<i class="' + css + '" />&nbsp;' + header;
        $(" > .modal-header > h4", this.$editor).html(html);
    };

    EntityEditor.prototype.setModalCss = function () {
        var css = this.options.modalCss;
        if (css && !this.$editor.hasClass("css")) {
            this.$editor.addClass(css);
        }
    };

    EntityEditor.prototype.showModal = function () {
        this.$body.html("");
        this.setHeaderHtml();
        this.setModalCss();
        this.$editor.modal('show');
    };

    EntityEditor.prototype.hideModal = function () {
        this.$editor.modal('hide');
    };

    $.fn.entityEditor = function (option) {
        var method = typeof arguments[0] == "string" && arguments[0];
        var args = method && Array.prototype.slice.call(arguments, 1) || arguments;
        var self = (this.length == 0) ? null : $.data(this[0], "entityEditor");

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
                    var r = $.data(this, "entityEditor")[method].apply(self, args);
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

                return result || this;
            } else return this;
        } else {
            return this.each(function () {
                var $this = $(this),
                    data = $.data(this, "entityEditor"),
                    options = $.extend({}, $.fn.entityEditor.defaults, $this.data(), typeof option == 'object' && option);
                if (!data)
                    $.data(this, 'entityEditor', (data = new EntityEditor(this, options)));
            });
        };
    };

    $.fn.entityEditor.defaults = {
        headerIconCss: 'icon-edit',
        headerText: '',
        modalCss: null,
        addUrl: '',
        editUrl: '',
        detailUrl: '',
        saveUrl: '',
        saveCallback: null,
        loadEditCallback: null,
        loadViewCallback: null
    };
})(jQuery);