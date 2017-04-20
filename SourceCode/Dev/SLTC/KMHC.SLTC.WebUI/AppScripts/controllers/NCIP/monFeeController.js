angular.module("sltcApp")
.controller("MonFeeCtrl", ['$scope', '$location', 'NCIAAuditAppcertRes', 'NCIPMonFeeRes', 'utility', function ($scope, $location, NCIAAuditAppcertRes, NCIPMonFeeRes, utility) {
    $scope.Data = {};
    //$scope.selectObj = "-1"; remove By Duke
    var preMonthDate = new Date();
    var preYear = preMonthDate.getFullYear();
    var preMonth = preMonthDate.getMonth();
    //$scope.startTime = new Date(preYear, preMonth, 1).format("yyyy-MM-dd");
    $scope.startTime = moment().subtract(1, 'month').set('date', 1).format("YYYY-MM-DD");
    $scope.endTime = new Date(preYear, preMonth, getMonthDays(preYear, preMonth)).format("yyyy-MM-dd");

    $scope.sTime = new Date(preYear, preMonth, 1).format("yyyy-MM");
    $scope.eTime = new Date(preYear, preMonth, getMonthDays(preYear, preMonth)).format("yyyy-MM");

    //获得某月的天数
    function getMonthDays(nowYear, myMonth) {
        var monthStartDate = new Date(nowYear, myMonth, 1);
        var monthEndDate = new Date(nowYear, myMonth + 1, 1);
        var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
        return days;
    }

    $scope.checkstartTime = function () {
        if (angular.isDefined($scope.startTime) && angular.isDefined($scope.endTime)) {
            if (!checkDate($scope.startTime, $scope.endTime)) {
                utility.msgwarning("查询开始日期必须在结束日期之前");
                $scope.endTime = "";
                return;
            }
        }
    }

    $scope.checkendTime = function () {
        if ($scope.startTime != "" && $scope.endTime != "") {
            if (angular.isDefined($scope.startTime) && angular.isDefined($scope.endTime)) {
                if (!checkDate($scope.startTime, $scope.endTime)) {
                    utility.msgwarning("查询结束日期必须在开始日期之后");
                    $scope.endTime = "";
                    return;
                }
            }
        }
        else {
            utility.msgwarning("账单开始日期或账单结束日期不能为空");
            return;
        }
    }

    $scope.init = function () {
        $scope.loadOrg();
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: NCIPMonFeeRes,//异步请求的res
            params: { startTime: $scope.sTime, endTime: $scope.eTime, orgID: "-1" },//mod By Duke
            success: function (data) {//请求成功时执行函数
                $scope.Data.MonFeeList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    };

    $scope.searchInfo = function () {
        $scope.options.params.startTime = $scope.startTime.substring(0, 7);
        $scope.options.params.endTime = $scope.endTime.substring(0, 7);

        if (!angular.isDefined($scope.selectObj) || $scope.selectObj == "") {
            $scope.options.params.orgID = "-1";
        }
        else {
            $scope.options.params.orgID = $scope.selectObj;
        }
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    };

    $scope.loadOrg = function () {
        $scope.OrgData = {};
        NCIAAuditAppcertRes.get({ org: 0 }, function (data) {
            $scope.OrgData = data.Data;
            //remove By Duke
            //if ($scope.OrgData != undefined && $scope.OrgData != null) {
            //    if ($scope.OrgData.length > 0) {
            //        $scope.selectObj = data.Data[0].NsId;
            //    }
            //}
        });
    };

    $scope.OpenMonFeeEdit = function () {
        var id = "";
        $scope.Data.MonFeeList.forEach(function (item) {
            if (item.IsCheck) {
                id += item.NSMonFeeID + ",";
            }
        });

        if (id == "") {
            utility.msgwarning("请选择有效数据！");
            return;
        }
        else {
            var url = '/NCIP/monFeeListEdit/' + id;
            $location.url(url);
        }
    };

    $scope.OpenOneMonFeeEdit = function (id) {
        id = id + ",";
        var url = '/NCIP/monFeeListEdit/' + id;
        $location.url(url);
    };
    
    //查询详情
    $scope.DetailsInfo = function (monFee) {
        $scope.NsName = "";
        $scope.OrgData.forEach(function (item) {
            if (item.NsId == monFee.NSID) {
                $scope.NsName = item.NsName;
            }
        });
        $scope.MonFeeInfo = monFee;
    };

    $scope.init();

}])
.controller("MonFeeEditCtrl", ['$scope', '$stateParams', '$location', 'NCIAAuditAppcertRes', 'NCIPMonFeeRes', 'utility', function ($scope, $stateParams, $location, NCIAAuditAppcertRes, NCIPMonFeeRes, utility) {
    $scope.Data = {};
    $scope.currentItem = {};
    $scope.IslookDetail = true;

    $scope.currentItem.TotalResident = 0;
    $scope.currentItem.TotalHospDay = 0;
    $scope.currentItem.TotalAmount = 0;
    $scope.currentItem.TotalNCIpay = 0;
    $scope.ServiceSecurity = 0;
    $scope.currentItem.AgencyResult = 20;

    $scope.init = function () {
        if ($stateParams.id) {
            NCIPMonFeeRes.get({ monFeeID: $stateParams.id }, function (data) {
                if (data.Data != null) {
                    $scope.Data.MonFeeList = data.Data;
                    $scope.Data.MonFeeList.forEach(function (item) {
                        $scope.currentItem.TotalResident += item.TotalResident;
                        $scope.currentItem.TotalHospDay += item.TotalHospday;
                        $scope.currentItem.TotalAmount += item.TotalAmount;
                        $scope.currentItem.TotalNCIpay += item.TotalNCIPay;
                    });
                    $scope.ServiceSecurity = ($scope.currentItem.TotalNCIpay * 0.05).toFixed(2);

                    if ($scope.Data.MonFeeList[0].Status != 10) {
                        $scope.IslookDetail = false;
                    }
                    else {
                        $scope.IslookDetail = true;
                    }
                }
                else {
                    utility.message(data.ResultMessage);
                }
            });
        }
    };

    $scope.lookFeeMonDetail = function (id) {
        var url = '/NCIP/monFeeDetail/' + id + '/' + $stateParams.id;
        $location.url(url);
    };

    $scope.save = function (currentItem) {
        $scope.MonFeeGrantRequestEntity = {};
        $scope.MonFeeGrantRequestEntity.monfeeList = [];
        $scope.MonFeeGrantRequestEntity.payGrantModel = {};
        $scope.MonFeeGrantRequestEntity.monfeeList = $scope.Data.MonFeeList;
        $scope.MonFeeGrantRequestEntity.payGrantModel = currentItem;

        NCIPMonFeeRes.save($scope.MonFeeGrantRequestEntity, function (data) {
            utility.message("保存成功！");
            location.href = "/NCIP/monFeeList";
        });
    };
    $scope.init();

}])
.controller("deducTionDialogCtrl", ['$scope', '$location', '$rootScope', '$compile', '$http', 'utility', 'DeductionRes', function ($scope, $location, $rootScope, $compile, $http, utility, DeductionRes) {
    var NSMonFeeID = $scope.kmIncludeParams.NSMonFeeID;
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: DeductionRes,//异步请求的res
            params: { NSMonFeeID: NSMonFeeID },
            success: function (data) {//请求成功时执行函数
                $scope.DeductionList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        };
    };
    $scope.init();
}])
.controller("MonFeeDetailCtrl", ['$scope', '$stateParams', '$location', '$compile', 'NCIAAuditAppcertRes', 'NCIPMonFeeRes', 'utility', function ($scope, $stateParams, $location,$compile, NCIAAuditAppcertRes, NCIPMonFeeRes, utility) {
    $scope.Data = {};
    $scope.Data.MonFeeEntity = {};
    $scope.Data.ResidentMonFeeEntityList = [];
    $scope.idlist = $stateParams.idlist;
    $scope.init = function () {
        if ($stateParams.id) {
            NCIPMonFeeRes.get({ monFeeid: $stateParams.id, type: 'detail' }, function (data) {
                $scope.Data.deAmount = data.Data.deAmount;
                $scope.Data.MonFeeEntity = data.Data.MonFeeEntity;
                $scope.Data.ResidentMonFeeEntityList = data.Data.ResidentMonFeeEntityList;
            });        
        }
    };

    $scope.backmonFeeEdit = function () {
        var url = '/NCIP/monFeeListEdit/' + $scope.idlist;
        $location.url(url);
    };

    $scope.rsMonFee = function (item) {
        var url = '/NCIP/rsMonFeeDtl/' +  $scope.idlist + '/' + $scope.Data.MonFeeEntity.NSMonFeeID + '/' + item.Rsmonfeeid + '/' + $scope.Data.MonFeeEntity.YearMonth;
        $location.url(url);
    };
    $scope.init();

    $scope.show = function () {
        var html = '<div km-include km-template="Views/NCIP/DeducTionList.html" km-controller="deducTionDialogCtrl"  km-include-params="{NSMonFeeID:\'' + $scope.Data.MonFeeEntity.NSMonFeeID + '\'}" ></div>';
        $scope.dialog = BootstrapDialog.show({
            title: '<label class=" control-label">巡检扣款记录</label>',
            type: BootstrapDialog.TYPE_DEFAULT,
            message: html,
            cssClass: 'pop-dialogInHos',
            size: BootstrapDialog.SIZE_WIDE,
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            }
        });
    }
}])