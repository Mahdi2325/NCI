angular.module("sltcApp")

 //护理险服务保证金拨款
.controller("ServiceDepositGrantCtrl", ['$scope', 'NCIPServiceDepositGrantRes', '$http', '$stateParams', 'utility', 'webUploader', 'auditAgency', '$compile', function ($scope, NCIPServiceDepositGrantRes, $http, $stateParams, utility, webUploader, auditAgency, $compile) {
   
    $scope.currentItem = {};
    $scope.currentItem.Decut = 0;
    $scope.currentItem.DueofPay = 0;
    $scope.currentItem.Decut = 0;
    $scope.currentItem.Name = $scope.kmIncludeParams.name;
    $scope.currentItem.NSID = $scope.kmIncludeParams.nsid;
    $scope.currentItem.Year = $scope.kmIncludeParams.yearmonth;
    if ($scope.kmIncludeParams.status == 0) {
        $scope.currentItem.DueofPay = $scope.kmIncludeParams.serdeposit;
        $scope.currentItem.ActrualPay = $scope.currentItem.DueofPay - $scope.currentItem.Decut;
    }
    if ($scope.kmIncludeParams.status == 1) {
        $scope.currentItem.DueofPay = $scope.kmIncludeParams.dueOfPay;
        $scope.currentItem.Decut = $scope.kmIncludeParams.decut;
        $scope.currentItem.DecutReason = $scope.kmIncludeParams.decutReason;
        $scope.currentItem.ActrualPay = $scope.kmIncludeParams.actrualPay;
        $scope.currentItem.SdGrantid = $scope.kmIncludeParams.sdGrantid;
    }

    $scope.ActuralFee = function () {
        if (angular.isDefined($scope.currentItem.Decut)) {
            if ($scope.currentItem.DueofPay - $scope.currentItem.Decut >= 0) {
                $scope.currentItem.ActrualPay = $scope.currentItem.DueofPay - $scope.currentItem.Decut;
            }
            else {
                utility.message("扣款金额不能大于应拨总额！");
                $scope.currentItem.ActrualPay = 0;
            }
        }
        
    };


    $scope.ServiceDepositGrant = function (item) {

        if ($scope.currentItem.DueofPay <= 0) {
            utility.message("应拨总额应该大于0！");
            return;
        }

        if ($scope.currentItem.DueofPay - $scope.currentItem.Decut < 0) {
            utility.message("扣款金额不能大于应拨总额！");
            return;
        }
        if ($scope.currentItem.Decut > 0) {
            if ($scope.currentItem.DecutReason == null || $scope.currentItem.DecutReason=="")
            {
                utility.message("请输入扣款原因！");
                return;
            }
        }


        NCIPServiceDepositGrantRes.save(item, function (data) {
            if (data.ResultCode == -1) {
                utility.message("保存失败！" + data.ResultMessage);
            }
            else {
                utility.message("保存成功！");
                $scope.backList();
            }


        });
        }
    $scope.backList = function () {
        if ($scope.kmIncludeParams.status == 0) {
            location.href = "/NCIP/ServiceDepositList";
        }
        else {
            location.href = "/NCIP/ServiceDepositGrantList";
        }

    }
    //$scope.init();

}])


