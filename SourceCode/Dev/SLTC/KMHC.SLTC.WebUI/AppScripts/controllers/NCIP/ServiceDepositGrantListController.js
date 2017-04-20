angular.module("sltcApp")

 //护理险服务保证金列表
.controller("ServiceDepositGrantListCtrl", ['$scope', '$compile', 'NCIPServiceDepositGrantListRes', 'NCIAAuditAppcertRes', 'utility', function ($scope, $compile, NCIPServiceDepositGrantListRes, NCIAAuditAppcertRes, utility) {
    $scope.Data = {};
    $scope.selectObj = "-1";
    $scope.init = function () {
        $scope.loadOrg();
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: NCIPServiceDepositGrantListRes,//异步请求的res
            params: { nsId: $scope.selectObj },
            success: function (data) {//请求成功时执行函数
                $scope.Data.ServiceDepositGrantList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    };
    $scope.Search = function () {
        if (!angular.isDefined($scope.selectObj) || $scope.selectObj == "" || $scope.selectObj == null) {
            $scope.options.params.nsId = "-1";
        }
        else {
            $scope.options.params.nsId = $scope.selectObj;
        }
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    };

    $scope.loadOrg = function () {
        $scope.OrgData = {};
        NCIAAuditAppcertRes.get({ org: 0 }, function (data) {
            $scope.OrgData = data.Data;
            //if ($scope.OrgData != undefined && $scope.OrgData != null) {
            //    if ($scope.OrgData.length > 0) {
            //        $scope.selectObj = data.Data[0].NsId;
            //    }
            //}
        });
    };

    $scope.edit = function (item) {
        var orgName = "";
        for (var i = 0; i < $scope.OrgData.length; i++) {
            if ($scope.OrgData[i].NsId == item.NsId) {
                orgName = $scope.OrgData[i].NsName;
            }
        }
        var params = { name: orgName, nsid: item.NsId, yearmonth: item.Year, dueOfPay: item.DueOfPay, decut: item.Decut, actrualPay: item.ActrualPay, decutReason: item.DecutReason,sdGrantid:item.SdGrantid, status: 1 };
        var html = '<div km-include km-template="Views/NCIP/ServiceDespositGrant.html" km-controller="ServiceDepositGrantCtrl"  km-include-params=\'' + JSON.stringify(params) + '\'}" ></div>';
        $scope.dialog = BootstrapDialog.show({
            title: '<label class=" control-label">服务保证金</label>',
            cssClass: 'pop-dialog',
            type: BootstrapDialog.TYPE_DEFAULT,
            message: html,
            size: BootstrapDialog.SIZE_WIDE,
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            }
        });
    }

    $scope.deleteItem = function (item) {
        if (confirm("确定删除该条服务保证金记录吗?")) {
            NCIPServiceDepositGrantListRes.save(item, function (data) {
                if (data.ResultCode == -1) {
                    utility.message("删除失败！" + data.ResultMessage);
                }
                else {
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                    utility.message("删除成功！");
                }
            });
        }
    };

    $scope.init();

}])


