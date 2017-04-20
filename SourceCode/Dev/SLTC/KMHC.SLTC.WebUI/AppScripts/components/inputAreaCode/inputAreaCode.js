﻿angular.module("extentComponent")
.directive("inputAreaCode", ['$q', 'postAreaRes', '$http', function ($q, postAreaRes, $http) {
    var cityURL, delay;
    delay = $q.defer();
    cityURL = '/AppScripts/components/inputAreaCode/inputArea.js';
    $http.get(cityURL).success(function (data) {
        return delay.resolve(data);
    }).error(function (data, status, headers, config) {
        return;
    });
    return {
        resctict: "E",
        templateUrl: "/AppScripts/components/inputAreaCode/inputAreaCode.html",
        scope: {
            value: "@value",
            callbackFn: "&callback"
        },
        link: function (scope, element, attrs) {
            scope.allItems = [];
            scope.items = [];
            scope.focus = function () {
                delay.promise.then(function (data) {
                    scope.items = data;
                    scope.allItems = data;
                    scope.showList = true;
                });
            }

            scope.keydown = function () {
                if (event.keyCode == 9) {
                    scope.showList = false;
                }
            }

            scope.mouseleave = function () {
                scope.showList = false;
            };

            scope.mouseenter = function () {
                scope.showList = true;
            };

            scope.change = function () {
                scope.showList = (angular.isDefined(scope.searchWords));
                if (scope.showList) {
                    var searchItems = $.grep(scope.allItems, function (item, i) {
                        return (item.BM.indexOf(scope.searchWords) == 0 || item.DQ.indexOf(scope.searchWords) > -1);
                    });
                    scope.items = searchItems
                }
            }

            scope.rowClick = function (item) {
                scope.searchWords = item.BM;
                scope.showList = false;//隐藏列表
                scope.callbackFn({ item: item });//回调函数

            };
            //根据关键字过滤结果
            scope.filterItems = function (item) {
                return ((angular.isDefined(item.BM) && item.BM.indexOf(scope.searchWords) >= 0) ||
                        (angular.isDefined(item.DQ) && item.DQ.indexOf(scope.searchWords) >= 0) ||
                        !angular.isDefined(scope.searchWords)
                );
            };

            //监控传入值的改变,同步关键字显示
            scope.$watch("value", function (newValue) {
                if (angular.isDefined(newValue) && newValue != "") {
                    scope.searchWords = newValue;
                    return false;
                }
            });
        }
    }
}]);

