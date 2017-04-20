angular.module("sltcApp")
.controller("PayGrantCtrl", ['$scope', '$location', 'NCIAAuditAppcertRes', 'NCIPPayGrantRes', 'utility', function ($scope, $location, NCIAAuditAppcertRes, NCIPPayGrantRes, utility) {
    $scope.Data = {};
    var preMonthDate = new Date();
    var preYear = preMonthDate.getFullYear();

    $scope.YearList = [{ "value": preYear - 3, "text": preYear - 3 }, { "value": preYear - 2, "text": preYear - 2 }, { "value": preYear - 1, "text": preYear - 1 }, { "value": preYear, "text": preYear }, { "value": preYear + 1, "text": preYear + 1 }, { "value": preYear + 2, "text": preYear + 2 }, { "value": preYear + 3, "text": preYear + 3 }]

    $scope.init = function () {
        $scope.Year = preYear;
        $scope.nsId = "-1";
        $scope.loadOrg();
        $scope.loadPayGrant($scope.Year, $scope.nsId);
    };

    $scope.loadOrg = function () {
        $scope.OrgData = {};
        NCIAAuditAppcertRes.get({ org: 0 }, function (data) {
            $scope.OrgData = data.Data;
            //$scope.nsId = data.Data[0].NsId;
        });
    };

    $scope.loadPayGrant = function (year, nsId) {
        $scope.payGrantList = {};
        NCIPPayGrantRes.get({ year: $scope.Year, nsid: nsId }, function (data) {
            $scope.payGrantList = data.Data;
        });
    };

    $scope.searchInfo = function () {
        if (!angular.isDefined($scope.nsId) || $scope.nsId == "" || $scope.nsId == null) {
            $scope.loadPayGrant($scope.Year, "-1");
        }
        else
        {
            $scope.loadPayGrant($scope.Year, $scope.nsId);
        }
    };

    //拨款
    $scope.payGrantInfo = function (payGrant) {
        var url = '/NCIP/payGrantEdit/' + payGrant.GrantID;
        $location.url(url);
    };

    $scope.changeAmount = function (num) {
        if (num == null || num == "" || num == 0 || Number(num) == NaN) {
            $scope.payGrantNum = $scope.currcurrentItem.TotalNCIpay;
        }
        var nummoney = Number(num) + $scope.currcurrentItem.TotalNCIpay;

        if (nummoney < 0) {
            utility.message("实际拨付金额为负数,请重新输入调整金额！");
            $scope.payGrantNum = $scope.currcurrentItem.TotalNCIpay;
            $scope.currcurrentItem.AdjustAmount = 0;
            return;
        }
        else {
            $scope.payGrantNum = nummoney;
        }
        $scope.ServiceMoney = ($scope.payGrantNum * 0.05).toFixed(2);
    }

    $scope.save = function (currcurrentItem) {
        NCIPPayGrantRes.save($scope.currcurrentItem, function (data) {
            $("#modalPayGrant").modal('toggle');
            utility.message("保存成功！");
            $scope.currcurrentItem = {};
            $scope.searchInfo();
        });
    };
    $scope.init();
}])
.controller("PayGrantEditCtrl", ['$scope', '$location', '$stateParams', 'NCIPPayGrantRes', 'NCIAAuditAppcertRes', 'utility', function ($scope, $location, $stateParams, NCIPPayGrantRes, NCIAAuditAppcertRes,utility) {
    $scope.Data = {};
    $scope.isShowPay = false;

    $scope.init = function ()
    {
        $scope.loadOrg();
        if ($stateParams.id) {
            NCIPPayGrantRes.get({ grantId: $stateParams.id, type: 0 }, function (data) {
                $scope.NsName = "";
                $scope.currcurrentItem = {};
                $scope.OrgData.forEach(function (item) {
                    if (item.NsId == data.Data.NSID) {
                        $scope.NsName = item.NsName;
                    }
                });
                $scope.currcurrentItem = data.Data;
                if (data.Data.Status == 20)
                {
                    $scope.currcurrentItem.AdjustAmount = 0;
                    $scope.changeAmount("0");
                    $scope.isShowPay = false;
                }
                else if (data.Data.Status == 30)
                {
                    $scope.changeAmount($scope.currcurrentItem.AdjustAmount);
                    $scope.isShowPay = true;
                }
            });
        }
    }

    $scope.loadOrg = function () {
        $scope.OrgData = {};
        NCIAAuditAppcertRes.get({ org: 0 }, function (data) {
            $scope.OrgData = data.Data;
            $scope.nsId = data.Data[0].NsId;
        });
    };

    $scope.changeAmount = function (num) {
        if (num == null || num == "" || num == 0 || Number(num) == NaN) {
            num = 0;
        }
        var nummoney = Number(num) + $scope.currcurrentItem.TotalNCIpay;

        if (nummoney < 0) {
            utility.message("实际拨付金额为负数,请重新输入调整金额！");
            $scope.ServiceMoney = ( $scope.currcurrentItem.TotalNCIpay * 0.05).toFixed(2);
            $scope.payGrantNum = $scope.currcurrentItem.TotalNCIpay - $scope.ServiceMoney;
            $scope.currcurrentItem.AdjustAmount = 0;
            return;
        }
        else {
            $scope.ServiceMoney = (nummoney * 0.05).toFixed(2);
            $scope.payGrantNum = nummoney - $scope.ServiceMoney;
        }
    }

    $scope.backPaygrantList = function ()
    {
       location.href = "/NCIP/payGrantList";
    }

    $scope.save = function (currcurrentItem) {
        NCIPPayGrantRes.save($scope.currcurrentItem, function (data) {
            location.href = "/NCIP/payGrantList";
            utility.message("保存成功！");
        });
    };

    $scope.lookFeeMonDetail = function (id) {
        var url = '/NCIP/payGrantDetail/' + id + '/' + $stateParams.id;
        $location.url(url);
    };


    $scope.init();
}])

.controller("payGrantDetail", ['$scope', '$stateParams', '$location', 'NCIAAuditAppcertRes', 'NCIPMonFeeRes', 'utility', function ($scope, $stateParams, $location, NCIAAuditAppcertRes, NCIPMonFeeRes, utility) {
    $scope.Data = {};
    $scope.Data.MonFeeEntity = {};
    $scope.Data.ResidentMonFeeEntityList = [];
    $scope.payid = $stateParams.payid;
    $scope.init = function () {
        if ($stateParams.id) {
            NCIPMonFeeRes.get({ monFeeid: $stateParams.id, type: 'detail' }, function (data) {
                $scope.Data.MonFeeEntity = data.Data.MonFeeEntity;
                $scope.Data.ResidentMonFeeEntityList = data.Data.ResidentMonFeeEntityList;
            });
        }
    };

    $scope.backmonFeeEdit = function () {
        var url = '/NCIP/payGrantEdit/' + $scope.payid;
        $location.url(url);
    };

    $scope.rsMonFee = function (item) {
        var url = '/NCIP/rspayGrantDtl/' + $scope.payid + '/' + $scope.Data.MonFeeEntity.NSMonFeeID + '/' + item.Rsmonfeeid + '/' + $scope.Data.MonFeeEntity.YearMonth;
        $location.url(url);
    };

    $scope.init();

}])

.controller("rspayGrantCtrl", ['$scope', '$location', '$state', '$stateParams', 'rsFeeMonDtlRes', 'utility', function ($scope, $location, $state, $stateParams, rsFeeMonDtlRes, utility) {
    var NsMonFeeID = $state.params.id;
    var RsMonFeeID = $state.params.rsMonFeeId;
    var YearMon = $state.params.YearMon;
    var payid = $state.params.payid;
    $scope.NsMonFeeID = NsMonFeeID;
    $scope.RsMonFeeID = RsMonFeeID;
    $scope.YearMon = YearMon;
    $scope.payid = payid;
    $scope.Data = {};
    $scope.Data.RSMonFees = {};
    $scope.Data.RSNciMonFeeDtlList = [];
    $scope.Data.RSSelfMonFeeDtlList = [];

    $scope.init = function () {
        //获取住民费用汇总信息
        rsFeeMonDtlRes.get({ currentPage: 1, pageSize: 10, orgMonFeeId: $scope.RsMonFeeID }, function (data) {
            $scope.Data.RSMonFees = data.Data[0];
            if ($scope.Data.RSMonFees != null && $scope.Data.RSMonFees != undefined && $scope.Data.RSMonFees != '') {
                $scope.Data.RSMonFees.RA = $scope.Data.RSMonFees.Hospday * $scope.Data.RSMonFees.Ncipaylevel
            };
        });
    };

    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: rsFeeMonDtlRes,//异步请求的res
        params: { orgMonFeeId: $scope.RsMonFeeID, feeType: "NCI" },
        success: function (data) {//请求成功时执行函数
            $scope.Data.RSNciMonFeeDtlList = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    };

    $scope.optionsSELF = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: rsFeeMonDtlRes,//异步请求的res
        params: { orgMonFeeId: $scope.RsMonFeeID, feeType: "SELF" },
        success: function (data) {//请求成功时执行函数
            $scope.Data.RSSelfMonFeeDtlList = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    };

    $scope.closeRSMonFeeDtl = function () {
        $state.go('payGrantDetail', { id: $scope.NsMonFeeID, payid: $scope.payid });
        $state.stateName = "payGrantDetail";
    };

    $scope.init();
}])