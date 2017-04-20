var homeApp = angular.module('sltcApp', [
        'ui.router',
        'ngResource',
        'ngCookies',
        'Utility',
        'extentDirective',
        'extentFilter',
        'extentComponent',
        'extentService',
        'angular-loading-bar',
        'ngAnimate'
])
     .constant("resourceBase", "http://120.25.225.5:5501/")
     .factory("regEvalSheetRes", ['$resource', function ($resource) {
         return $resource("api/regEvalSheet/:id", { id: "@id" });
     }])
     .factory("evaluationResultsRes", ['$resource', function ($resource) {
         return $resource("api/EvaluationResults/:id", { id: "@id" }, {
             update: {
                 method: 'PUT' // this method issues a PUT request
             }
         });
     }])
     .factory("communityRes", ['$resource', function ($resource) {
         return $resource("api/Community/:id", { id: "@id" });
     }])
     .factory("EvaluationResultsStatisticsRes", ['$resource', function ($resource) {
         return $resource("api/EvaluationResultsStatistics/:id", { id: "@id" });
     }])
    //服务申请  addby Amaya 
    .factory("ECCaseStudyRes", ['$resource', function ($resource) {
        return $resource('api/Applicant/:id', { id: '@id' });
    }])
    .factory("adminHandoversRes", ['$resource', function ($resource) { // 交付照会
        return $resource('api/AssignTask/:id', { id: '@id' }, {//bobdu扩展了方法
            assSave: {     //自定义的方法  
                method: 'POST',
                url: 'api/AssignTask/assSave',
            }
        });
        //return $resource(resourceBase + 'affairshandover/:id', { id: '@id' });
    }])
    .factory("dc_AssignJobsRes", ['$resource', function ($resource) {
        return $resource('api/AssignJobs/:id', { id: '@id' });
    }])
    .factory("roleModuleRes", ['$resource', function ($resource) { //出院办理
        return $resource('api/module/:id', { id: '@id' });
    }])
        .factory("postAreaRes", ['$resource', function ($resource) {
            return $resource('api/postArea/:id', { id: '@id' });
        }])

    .factory("NCIAApplicantRes", ['$resource', function ($resource) { // 参保人基本信息录入
        return $resource('api/Applicant/:id', { id: '@id' }, {//bobdu扩展了方法
            dtlSave: {     //自定义的方法  
                method: 'POST',
                url: 'api/Applicant/dtlSave',
            }
        });
    }])

   .factory("NCIPMonFeeRes", ['$resource', function ($resource) {
       return $resource('api/monFee/:id', { id: '@id' });
   }])

       .factory("NCIPPayGrantRes", ['$resource', function ($resource) {
           return $resource('api/payGrant/:id', { id: '@id' });
       }])

    .factory("NCIAAppcertRes", ['$resource', function ($resource) {
        return $resource('api/Appcert/:id', { id: '@id' }, {
            saveADL: {     //自定义的方法  
                method: 'POST',
                url: 'api/Appcert/saveADL',
            }
        });
    }])

    .factory("NCIAAuditAppcertRes", ['$resource', function ($resource) {
        return $resource('api/AuditAppcert/:id', { id: '@id' }, {
            saveADL: {     //自定义的方法  
                method: 'POST',
                url: 'api/AuditAppcert/saveADL',
            }
        });
    }])

    .factory("NCIAAppHospRes", ['$resource', function ($resource) {
            return $resource('api/appHosp/:id', { id: '@id' });
        }])
    .factory("CaseMgrFeeRes", ['$resource', function ($resource) {
            return $resource('api/CaseMgr/GetFee/:id', { id: '@id' });
        }])
    .factory("CaseMgrCplRec", ['$resource', function ($resource) {
            return $resource('api/CaseMgr/GetCplRec/:id', { id: '@id' });
        }])
    .factory("CaseMgrNsRecRes", ['$resource', function ($resource) {
            return $resource('api/CaseMgr/GetNsRec/:id', { id: '@id' });
        }])
    .factory("CaseMgrMeasureRecRes", ['$resource', function ($resource) {
            return $resource('api/CaseMgr/GetMeasureRec/:id', { id: '@id' });
        }])
    .factory("CaseMgrEvlRecRes", ['$resource', function ($resource) {
            return $resource('api/CaseMgr/GetEvlRec/:id', { id: '@id' });
        }])

        .factory("NCIAAuditAppHospRes", ['$resource', function ($resource) {
            return $resource('api/auditAppHosp/:id', { id: '@id' });
        }])
    .factory('auditAgency', [function () {
        return {
            Comment: ""
        }
    }])
    //字典管理list zhongyh
    .factory("DCDataDicListRes", ['$resource', function ($resource) {
        return $resource('api/DCDataDicList/:id', { id: '@id' });
    }])
    .factory("userRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //用户管理
        return $resource("api/users/:id", { id: "@id" }, {//bobdu扩展了方法
            ChangePassWord: {     //自定义的方法  
                method: 'POST',
                url: 'api/users/ChangePassWord',
            }
        });
    }])
    .factory("roleRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //角色管理
        return $resource("api/roles/:id", { id: "@id" });
    }])
    .factory("roleNewRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //角色管理
        return $resource("api/rolesNew/:id", { id: "@id" });
    }])
    .factory("moduleRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //模块管理
        return $resource("api/module/:id", { id: "@id" });
    }])
    .factory("roleModuleRes", ['$resource', 'resourceBase', function ($resource, resourceBase) { //角色模块
        return $resource('api/module/:id', { id: '@id' });
    }])
    .factory("NCIPServiceDepositRes", ['$resource', function ($resource) {
        return $resource('api/ServiceDeposit/:id', { id: '@id' });
    }])

    .factory("NCIPServiceDepositGrantRes", ['$resource', function ($resource) {
        return $resource('api/ServiceDepositGrant/:id', { id: '@id' });
    }])

    .factory("NCIPServiceDepositGrantListRes", ['$resource', function ($resource) {
        return $resource('api/ServiceDepositGrantList/:id', { id: '@id' });
    }])

    .factory("RegInHosStatusRes", ['$resource', function ($resource) {
        return $resource('api/RegInHosStatusRes/:id', { id: '@id' });
    }])

    .factory("rsFeeMonDtlRes", ['$resource', function ($resource) { //申报费用明细
        return $resource('api/rsMonFeeDtl/:rsMonFeeId/:YearMon', { id: '@id' });
    }])

    .factory("AuditYearCertRes", ['$resource', function ($resource) {
        return $resource('api/AuditYearCert/:id', { id: '@id' });
    }])

    .factory("ReportTempManageRes", ['$resource', function ($resource) {
        return $resource('api/ReportTempManage/:id', { id: '@id' });
    }])

    .factory("PersonStatusReportRes", ['$resource', function ($resource) {
        return $resource('api/PersonStatusReport/:id', { id: '@id' });
    }])

    .factory('dictManageRes', ['$resource', 'resourceBase', function ($resource, resourceBase) {
        return $resource("api/DictManage/:id", { id: "@id" }, {
            GetFeeIntervalByMonth: {     //自定义的方法  
                method: 'GET',
                url: 'api/DictManage/GetFeeIntervalByMonth/:month',
            },
            GetFeeIntervalByDateStr: {     //自定义的方法  
                method: 'GET',
                url: 'api/DictManage/GetFeeIntervalByDateStr/:date'
            },
            GetFeeIntervalByDateTime: {     //自定义的方法  
                method: 'GET',
                url: 'api/DictManage/GetFeeIntervalByDateTime/:date',
            },
            GetFeeIntervalByYearMonth: {     //自定义的方法  
                method: 'GET',
                url: 'api/DictManage/GetFeeIntervalByYearMonth/:yearMonth',
            }
        });
    }])
    .factory("DeductionRes", ['$resource', function ($resource) {
        return $resource('api/Deduction/:id', { id: '@id' });
    }]);
homeApp.run(['$rootScope', 'utility', function ($rootScope, utility) {
    $rootScope.homeUrl = utility.getUserInfo().OrgType == 1 ? '/NCIA/HlxSbaoDesk' : '/NCIA/HlxDesk';
}]);