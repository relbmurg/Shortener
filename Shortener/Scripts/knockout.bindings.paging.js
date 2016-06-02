/// <reference path="~/Scripts/knockout-3.4.0.debug.js" />
/*jshint multistr: true */

(function (ko) {

    var templateEngine = new ko['nativeTemplateEngine']();

    templateEngine.addTemplate = function (templateName, templateMarkup) {
        document.write("<script type='text/html' id='" + templateName + "'>" + templateMarkup + "<" + "/script>");
    };

    templateEngine.addTemplate("ko_paging_pagination", "\
<nav>\
    <ul class='pagination'>\
        <li data-bind='css: { disabled: atStart }'>\
            <a href='#' data-bind='click: function () { moveToPage(0) }'><<</a>\
        </li>\
        <li data-bind='css: { disabled: atStart }'>\
            <a href='#' data-bind='click: previousPage'><</a>\
        </li>\
        <!-- ko foreach: allPages -->\
        <li data-bind='css: { active: $data.pageNumber === ($parent.pageIndex() + 1) }'>\
            <a href='#' data-bind='text: $data.pageNumber, click: function () { $parent.moveToPage($data.pageNumber - 1); }'></a>\
        </li>\
        <!-- /ko -->\
        <li data-bind='css: { disabled: atEnd }'>\
            <a href='#' data-bind='click: nextPage'>></a>\
        </li>\
        <li data-bind='css: { disabled: atEnd }'>\
            <a href='#' data-bind='click: function () { moveToPage(maxPageIndex()) }'>>></a>\
        </li>\
    </ul>\
</nav>\
    ");

    templateEngine.addTemplate("ko_paging_size_selector", "\
<select data-bind='value: pageSize' class='paging-size-selector'>\
    <option value='10'>10</option>\
    <option value='25'>25</option>\
    <option value='50'>50</option>\
    <option value='2147483647'>все</option>\
</select>\
    ");

    var localPager = function(list) {
        var self = this;
        //self.list = list;

        self.current = ko.observable(0);                // current page index
        self.total = ko.computed(function () {          // lenght of list
            return list().length;
        }, self);
        self.size = ko.observable("10");                // page size

        self.page = ko.computed(function () {           // current page elements
            var size = +self.size();
            var start = self.current() * size;
            return list().slice(start, start + size);
        }, self);

        return {
            current: self.current,
            total: self.total,
            size: self.size,
            page: self.page
        };
    };

    var model = function (pager) {
        var self = this;
        self.pageIndex = ko.computed(function () {
            return pager.current();
        }, self);
        self.pageSize = ko.computed({
            read: function() {
                return pager.size();
            },
            write: function(value) {
                pager.size(value);
            },
            owner: self
        });
        self.maxPageIndex = ko.computed(function () {
            return Math.ceil(pager.total() / pager.size()) - 1;
        }, self);
        self.previousPage = function () {
            if (pager.current() > 0) {
                pager.current(pager.current() - 1);
            }
        };
        self.nextPage = function () {
            if (pager.current() < self.maxPageIndex()) {
                pager.current(pager.current() + 1);
            }
        };
        self.allPages = ko.computed(function () {
            var pages = [];
            if (self.maxPageIndex() <= 10) {
                for (i = 0; i <= self.maxPageIndex() ; i++) {
                    pages.push({ pageNumber: (i + 1) });
                }
            }
            else {
                var start = pager.current() < 10 ? 0 : pager.current() - 5;
                var end = pager.current() < 10 ? 10 : pager.current() + 5;
                if (end > self.maxPageIndex())
                    end = self.maxPageIndex() + 1;
                for (var i = start ; i < end; i++) {
                    pages.push({ pageNumber: (i + 1) });
                }
            }
            return pages;
        });
        self.moveToPage = function (index) {
            pager.current(index);
        };
        self.atStart = ko.computed(function () {
            return pager.current() === 0;
        });
        self.atEnd = ko.computed(function () {
            return pager.current() == self.maxPageIndex();
        }, self);
        self.visiblePagination = ko.computed(function () {
            return self.maxPageIndex() > 0;
        }, self);

        self.pagedList = ko.computed(function () {
            return pager.page ? pager.page : ko.observableArray([]);
        }, self);

        pager.size.subscribe(function () {
            self.moveToPage(0);
        });

    };

    function createPagerIfEmpty(observable, pager) {
        if (!observable.pagerModel) {
            if (!pager)
                pager = new localPager(observable);
            observable.pagerModel = new model(pager);
        }
    }

    // 
    // usage: data-binding = "page: observableArray"
    //
    ko.bindingHandlers.page = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var value = valueAccessor();
            createPagerIfEmpty(value);
            return ko.bindingHandlers.foreach.init(element, value.pagerModel.pagedList, allBindingsAccessor, viewModel, bindingContext);
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var value = valueAccessor();
            return ko.bindingHandlers.foreach.update(element, value.pagerModel.pagedList, allBindingsAccessor, viewModel, bindingContext);
        }
    };

    //
    // usage: 
    //    data-binding = "pagination: observableArray"  
    //    data-binding = "pagination: observableArray, pager: { current: observable, total: observable, size: observable}"
    //
    ko.bindingHandlers.pagination = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var value = valueAccessor();
            var pager = allBindingsAccessor()['pager'];
            createPagerIfEmpty(value, pager);
            var context = bindingContext.createChildContext(value.pagerModel);

            ko.renderTemplate("ko_paging_pagination", context, { templateEngine: templateEngine }, element);

            return { 'controlsDescendantBindings': true };
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var value = valueAccessor();
            ko.bindingHandlers.visible.update(element, value.pagerModel.visiblePagination, allBindingsAccessor, viewModel, bindingContext);
        }
    };

    //
    // usage: 
    //    data-binding = "pageSize: observableArray"  
    //    data-binding = "pageSize: observableArray, pager: { current: observable, total: observable, size: observable}"
    //
    ko.bindingHandlers.pageSize = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var value = valueAccessor();
            var pager = allBindingsAccessor()['pager'];
            createPagerIfEmpty(value, pager);
            var context = bindingContext.createChildContext(value.pagerModel);

            ko.renderTemplate("ko_paging_size_selector", context, { templateEngine: templateEngine }, element);

            return { 'controlsDescendantBindings': true };
        }
    };

}(ko));