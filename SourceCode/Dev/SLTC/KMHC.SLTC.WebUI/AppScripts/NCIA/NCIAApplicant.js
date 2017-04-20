/*****************************************************************************
 * Filename: NCIAApplicant
 * Creator:  Amaya
 * Create Date: 2016-11-11
 * Description: 参保人基本信息
 * Modifier: 
 * Modify Date: 
 * Modify Description: 
 ******************************************************************************/
angular.module("sltcApp")
.controller("NCIAApplicantCtrl", ['$rootScope', '$scope', '$http', '$location', '$state', 'utility', 'cloudAdminUi', 'NCIAApplicantRes', 'NCIAAuditAppcertRes', function ($rootScope, $scope, $http, $location, $state, utility, cloudAdminUi, NCIAApplicantRes, NCIAAuditAppcertRes) {
    $scope.currentUser = currentUser;
    $scope.Data = {};

    //初始化加载数据
    $scope.init = function () {
        $scope.loadOrg();
        if (utility.getUserInfo().OrgType == 2) {
            $scope.IsShowNs = true;
        }
        else if (utility.getUserInfo().OrgType == 1) {
            $scope.IsShowNs = false;
        };

        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: NCIAApplicantRes,//异步请求的res
            params: { name: "", idno: "", nsId: "" },
            success: function (data) {//请求成功时执行函数
                $scope.Data.personList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        };
    }

    $scope.searchPersonList = function () {
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    };

    // 逻辑删除参保人
    $scope.deletePerson = function (item) {
        if (confirm("您确定删除吗?")) {
            item.Isdelete = true;  //
            NCIAApplicantRes.save(item, function (data) {
                if (data.ResultCode == 1001) {
                    utility.msgwarning(item.Name + "存在已经申请或者审核的记录！");
                } else if (data.ResultMessage != null) {
                    utility.message(data.ResultMessage);
                } else {
                    utility.message(item.Name + "删除成功！");
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                }
            });
        }
    };

    $scope.loadOrg = function () {
        $scope.OrgData = {};
        NCIAAuditAppcertRes.get({ org: 0 }, function (data) {
            $scope.OrgData = data.Data;
        });
    };

    $scope.init();
}])
.controller("NCIAApplicantEditCtrl", ['$rootScope', '$scope', '$stateParams', '$http', '$location', '$state', 'utility', 'cloudAdminUi', 'NCIAApplicantRes', function ($rootScope, $scope, $stateParams, $http, $location, $state, utility, cloudAdminUi, NCIAApplicantRes) {
    $scope.currentUser = currentUser;
    $scope.person = {};
    $scope.disHaBithos = true;
    $scope.Economystatus1 = false;
    $scope.Economystatus2 = false;
    $scope.Economystatus3 = false;
    $scope.Economystatus4 = false;
    $scope.MedicalWay1 = false;
    $scope.MedicalWay2 = false;
    $scope.MedicalWay3 = false;
    $scope.MedicalWay4 = false;
    $scope.IsPartInfoDisabled = false;
    $scope.changeHaBithos = function () {
        if ($('#MedicalWayOther').prop("checked")) {
            $scope.disHaBithos = false;
        }
        else {
            $scope.disHaBithos = true;
            $scope.person.MedicalWay5 = "";
        }
    }

    $scope.PostSelected = function (item) {
        $scope.person.Nativeplace = item.DQ;
    }


    //根据IdNo 获取性别
    $scope.Getsex = function (psidno) {
        if (psidno != null) {
            var sexno;
            if (psidno.length == 10) {
                sexno = psidno.substring(1, 2);
            } else if (psidno.length == 15) {
                sexno = psidno.substring(14, 15)
            } else if (psidno.length == 18) {
                sexno = psidno.substring(16, 17)
            }
            else {
                alert("错误的身份证号码，请核对！");
                return;
            }
            var tempid = sexno % 2;
            if (tempid == 0) {
                $scope.person.Gender = "男";
                $('#SexF').attr("checked", "checked");
                $('#SexM').removeAttr("checked");

            } else {
                $scope.person.Gender = "女";
                $('#SexM').attr("checked", "checked");
                $('#SexF').removeAttr("checked");
            }

            var areano;
            if (psidno.length == 15) {
                areano = psidno.substring(0, 6)
            } else if (psidno.length == 18) {
                areano = psidno.substring(0, 6)
            }
        }
    }

    $scope.init = function () {
    };

    if ($stateParams.id) {
        NCIAApplicantRes.get({ no: $stateParams.id }, function (data) {
            $scope.person = data.Data;
            if ($scope.person.Economystatus.indexOf("001") != -1) {
                $scope.EconomyStatus1 = true;
            }
            if ($scope.person.Economystatus.indexOf("002") != -1) {
                $scope.EconomyStatus2 = true;
            }
            if ($scope.person.Economystatus.indexOf("003") != -1) {
                $scope.EconomyStatus3 = true;
            }
            if ($scope.person.Economystatus.indexOf("004") != -1) {
                $scope.EconomyStatus4 = true;
            }

            if ($scope.person.Habithos.indexOf("01") != -1) {
                $scope.MedicalWay1 = true;
            }
            if ($scope.person.Habithos.indexOf("02") != -1) {
                $scope.MedicalWay2 = true;
            }
            if ($scope.person.Habithos.indexOf("03") != -1) {
                $scope.MedicalWay3 = true;
            }
            if ($scope.person.Habithos.indexOf("04") != -1) {
                $scope.MedicalWay4 = true;
                $scope.disHaBithos = false;
                $scope.person.MedicalWay5 = $scope.person.Habithos.split("04|")[1];
            }

            if (utility.getUserInfo().OrgType == 1) {
                if ($scope.person.IsApply) {
                    $scope.IsPartInfoDisabled = true;
                }
            }
            //拷贝第一次加载的人员资料
            $scope.CurrentPerson = angular.copy($scope.person);
        });
    }

    $scope.save = function () {
        if (angular.isDefined($scope.CurrentPerson)) {
            if (angular.equals($scope.CurrentPerson, $scope.person)) {
                utility.msgwarning("人员信息未更改，无需保存");
                return;
            }
            else {
                $scope.person.IsInsertHistory = true;
                if (!angular.equals($scope.CurrentPerson.Idno, $scope.person.Idno)) {
                    $scope.person.IsIdNoChaged = true;
                }
            }
        }
        $scope.person.Economystatus = '';
        if ($('#pension').prop("checked")) {
            $scope.person.Economystatus += '001|';
        }
        if ($('#childAllowance').prop("checked")) {
            $scope.person.Economystatus += '002|';
        }
        if ($('#Subsidize').prop("checked")) {
            $scope.person.Economystatus += '003|';
        }
        if ($('#OtherSubsidies').prop("checked")) {
            $scope.person.Economystatus += '004|';
        }

        $scope.person.Habithos = '';
        if ($('#Familyward').prop("checked")) {
            $scope.person.Habithos += '01|';
        }
        if ($('#communityHospital').prop("checked")) {
            $scope.person.Habithos += '02|';
        }
        if ($('#Goout').prop("checked")) {
            $scope.person.Habithos += '03|';
        }
        if ($('#MedicalWayOther').prop("checked")) {
            $scope.person.Habithos += '04|';
            $scope.person.Habithos += $scope.person.MedicalWay5;
        }

        NCIAApplicantRes.dtlSave($scope.person, function (data) {
            if (data.ResultCode == 1001) {
                utility.msgwarning(data.ResultMessage);
                return;
            }
            else {
                $location.url('/NCIA/applicant');
                utility.message($scope.person.Name + "的信息保存成功！");
            }
        });
    }

    $scope.init();
}]);