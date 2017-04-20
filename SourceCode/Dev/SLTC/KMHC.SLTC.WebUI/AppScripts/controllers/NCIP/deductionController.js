angular.module("sltcApp")
.controller("deducTionCtrl", ['$scope', '$location', '$rootScope', '$compile', '$http', 'utility', 'NCIAAuditAppcertRes', 'DeductionRes', function ($scope, $location, $rootScope, $compile, $http, utility, NCIAAuditAppcertRes, DeductionRes) {
    $scope.sDate = moment().subtract(1, 'year').format("YYYY-MM");
    $scope.eDate = moment().format("YYYY-MM");
    $scope.currItem = {};
    $scope.DeductionList = {};
    //记载机构列表
    $scope.loadOrg = function () {
        $scope.OrgData = {};
        NCIAAuditAppcertRes.get({ org: 0 }, function (data) {
            $scope.OrgData = data.Data;
        });
    };

    //初始化
    $scope.init = function () {
        $scope.loadOrg();
        $scope.checkDate();
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: DeductionRes,//异步请求的res
            params: { nsId: "-1", sDate: $scope.sDate, eDate: $scope.eDate },
            success: function (data) {//请求成功时执行函数
                if (data.resultContent == "-1") {
                    utility.msgwarning("数据错误，请联系管理员！");
                }
                else {
                    $scope.DeductionList = data.Data;
                }
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        };
    };

    //查询
    $scope.search = function () {
        $scope.checkDate();
        if (!angular.isDefined($scope.nsId) || $scope.nsId == "" || $scope.nsId == null) {
            $scope.options.params.nsId = "-1";
        }
        else {
            $scope.options.params.nsId = $scope.nsId;
        }
        $scope.options.params.sDate = $scope.sDate;
        $scope.options.params.eDate = $scope.eDate;

        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    };

    //校验日期
    $scope.checkDate = function () {
        if (!$scope.sDate || !$scope.eDate) {
            utility.msgwarning("开始月份或结束月份不能为空");
            return;
        }
        if ($scope.sDate > $scope.eDate) {
            utility.msgwarning("开始月份不能大于结束月份");
            return;
        }
    };

    $scope.initmodal = function () {
        $scope.currItem = {};
        $scope.currItem.Debitmonth = moment().format("YYYY-MM");
    }

    $scope.save = function (item) {
        if (angular.isDefined($scope.ReqForm.$error.required)) {
            for (var i = 0; i < $scope.ReqForm.$error.required.length; i++) {
                utility.msgwarning($scope.ReqForm.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.ReqForm.$error.maxlength)) {
            for (var i = 0; i < $scope.ReqForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.ReqForm.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }
        item.IsDelete = false;

        if (item.DeductionStatus == 1) {
            utility.msgwarning("该条扣款记录信息处于已扣款状态！");
        }
        else {
            DeductionRes.save(item, function (data) {
                if (data.resultContent == "-1") {
                    utility.msgwarning("数据错误，请联系管理员！");
                }
                else {
                    utility.message("保存成功！");
                    $("#modalDetail").modal("toggle");
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                }
            });
        }
    };

    $scope.updatemodal = function (item) {
        $scope.currItem = {};
        $scope.currItem = item;
        $scope.currItem.ID = item.ID;
        $scope.currItem.Orgid = item.Orgid;
        $scope.currItem.Debitmonth = item.Debitmonth;
        $scope.currItem.Amount = item.Amount;
        $scope.currItem.Debitdays = item.Debitdays;
        $scope.currItem.DeductionReason = item.DeductionReason;
        $scope.currItem.OrgName = item.OrgName;
    };

    $scope.deletededu = function (item) {
        if (item.DeductionStatus == 1) {
            utility.msgwarning("该条扣款记录信息处于已扣款状态！");
        }
        else {
            if (confirm("确定删除该条扣款记录信息吗?")) {
                item.IsDelete = true;
                DeductionRes.save(item, function (data) {
                    if (data.resultContent == "-1") {
                        utility.msgwarning("数据错误，请联系管理员！");
                    }
                    else {
                        utility.message("删除成功！");
                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.search();
                    }
                });
            }
        }
    };

    $scope.init();
}]);