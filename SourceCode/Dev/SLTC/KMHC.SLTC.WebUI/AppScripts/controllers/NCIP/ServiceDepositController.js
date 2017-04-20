angular.module("sltcApp")

 //护理险服务保证金列表
.controller("ServiceDepositListCtrl", ['$scope', '$compile', 'NCIPServiceDepositRes', 'NCIAAuditAppcertRes', 'utility', function ($scope, $compile, NCIPServiceDepositRes, NCIAAuditAppcertRes, utility) {
    $scope.Data = {};
    $scope.selectObj = "-1";

    var preMonthDate = new Date();
    var preYear = preMonthDate.getFullYear();

    $scope.YearList = [{ "value": preYear - 3, "text": preYear - 3 }, { "value": preYear - 2, "text": preYear - 2 }, { "value": preYear - 1, "text": preYear - 1 }, { "value": preYear, "text": preYear }, { "value": preYear + 1, "text": preYear + 1 }, { "value": preYear + 2, "text": preYear + 2 }, { "value": preYear + 3, "text": preYear + 3 }]


    $scope.init = function () {
        $scope.Year = preYear;
        $scope.loadOrg();
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: NCIPServiceDepositRes,//异步请求的res
            params: { yearMonth: $scope.Year, nsId: $scope.selectObj},
            success: function (data) {//请求成功时执行函数
                $scope.Data.ServiceDepositList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 12
            }
        }
    };
    $scope.Grant = function () {
        var serdeposit = 0;
        var grantStatue = false;
        for (var i = 0; i < $scope.Data.ServiceDepositList.length; i++) {
            if ($scope.Data.ServiceDepositList[i].Status == 0) {
                serdeposit += $scope.Data.ServiceDepositList[i].Amount;
                grantStatue = true;
            }
        }
        if (!grantStatue) {
            utility.message("该年份的服务保证金都已拨付！");
            return;
        }
        serdeposit = serdeposit.toFixed(2);
        var orgName = "";
        for (var i = 0; i < $scope.OrgData.length; i++) {
            if ($scope.OrgData[i].NsId == $scope.selectObj) {
                orgName = $scope.OrgData[i].NsName;
            }
        }
        var params = { name: orgName, nsid: $scope.selectObj, yearmonth: $scope.Year, serdeposit: serdeposit, status: 0 };
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

    };
    $scope.Search = function () {
        if (!angular.isDefined($scope.Year) || $scope.Year == "") {
            $scope.options.params.yearMonth = "-1";
        }
        else {
            $scope.options.params.yearMonth = $scope.Year;
        }
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

    $scope.init();
    $("#pageFooter").hide();

}])


