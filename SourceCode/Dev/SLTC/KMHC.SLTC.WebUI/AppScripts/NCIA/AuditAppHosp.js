angular.module("sltcApp")
//入院申请审核
.controller("NCIAAuditAppHospCtrl", ['$scope', 'NCIAAuditAppHospRes', 'NCIAAuditAppcertRes', 'utility', function ($scope, NCIAAuditAppHospRes, NCIAAuditAppcertRes, utility) {
    $scope.Data = {};

    $scope.init = function () {
        $scope.loadOrg();
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: NCIAAuditAppHospRes,//异步请求的res
            params: { name: "", idno: "", nsId: "-1", status: -1 },
            success: function (data) {//请求成功时执行函数
                $scope.Data.AppHospList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    };

    $scope.AuditStatus = [{ "value": "3", "text": "待审核" }, { "value": "6", "text": "审核通过" }, { "value": "9", "text": "审核未通过" }]

    $scope.searchInfo = function () {
        if ($scope.options.params.nsId == null) {
            $scope.options.params.nsId = "-1";
        }

        if ($scope.options.params.status == null) {
            $scope.options.params.status = -1;
        }

        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();

    }

    $scope.loadOrg = function () {
        $scope.OrgData = {};
        NCIAAuditAppcertRes.get({ org: 0 }, function (data) {
            $scope.OrgData = data.Data;
        });
    };

    $scope.init();

}])

.controller("NCIAAuditAppHospEditCtrl", ['$scope', 'NCIAAuditAppHospRes', 'NCIAAppHospRes', '$http', '$stateParams', 'utility', 'webUploader', 'auditAgency', '$compile', function ($scope, NCIAAuditAppHospRes, NCIAAppHospRes, $http, $stateParams, utility, webUploader, auditAgency, $compile) {
    $scope.init = function () {
        if ($stateParams.id) {
            NCIAAppHospRes.get({ appHospid: $stateParams.id }, function (data) {
                $scope.currentItem = data.Data;
                if ($scope.currentItem.CareType == 1) {
                    $scope.currentItem.CareTypetxt = "一级专护";
                }
                else if ($scope.currentItem.CareType == 2) {
                    $scope.currentItem.CareTypetxt = "二级专护";
                }
                else if ($scope.currentItem.CareType == 3) {
                    $scope.currentItem.CareTypetxt = "机构护理";
                }
            });
        }
    }

    //
    webUploader.init('#ArchiveFilePicker', { category: 'ArchiveFile' }, '文件', 'doc,docx,pdf,xls,xlsx,gif,jpg,jpeg,bmp,png', 'doc/*,application/pdf,image/*,xls/*', function (data) {
        if (data.length > 0) {
            $scope.currentItem.SavedLocation = data[0].SavedLocation;
            $scope.currentItem.FileName = data[0].FileName;
            $scope.currentItem.UploadFiles = '{0}|$|{1}'.format($scope.currentItem.FileName, $scope.currentItem.SavedLocation);
            $scope.$apply();
        }
    });
    //
    $scope.clear = function () {
        $scope.currentItem.SavedLocation = "";
        $scope.currentItem.FileName = "";
        $scope.currentItem.UploadFiles = "";
    }

    $scope.AuditAppHosp = function (item, mark) {

        if (angular.isDefined($scope.appertForm.$error.required)) {
            for (var i = 0; i < $scope.appertForm.$error.required.length; i++) {
                utility.msgwarning($scope.appertForm.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.appertForm.$error.maxlength)) {
            for (var i = 0; i < $scope.appertForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.appertForm.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (mark == "success") {
            item.ActionStatus = 6;
        }
        else if (mark == "fail") {
            item.ActionStatus = 9;
        }
        NCIAAuditAppHospRes.save(item, function (data) {
            if (data.ResultCode == -1) {
                utility.message("保存失败！" + data.ResultMessage);
            }
            else {
                $scope.PutAppHospRate(item.NsID);
                utility.message("保存成功！");
                $scope.backList();
            }


        });
    };


    //调用API接口
    $scope.PutAppHospRate = function (nsId) {
        var year = new Date().getFullYear();
        $http({
            url: 'api/OrgStatistics/PutApphospRate?NSID=' + nsId + '&YEAR=' + year,
            method: 'GET'
        }).then(function (res) {

        })
    }


    $scope.backList = function () {
        location.href = "/NCIA/auditAppHosp";
    }
    $scope.init();

}])
//入院申请审核
.controller("NCIAAuditAppHospViewCtrl", ['$scope', '$stateParams', 'NCIAAppHospRes', 'utility', 'webUploader', function ($scope, $stateParams, NCIAAppHospRes, utility, webUploader) {
    $scope.currentItem = {};

    //
    webUploader.init('#ArchiveFilePicker', { category: 'ArchiveFile' }, '文件', 'doc,docx,pdf,xls,xlsx,gif,jpg,jpeg,bmp,png', 'doc/*,application/pdf,image/*,xls/*', function (data) {
        if (data.length > 0) {
            $scope.currentItem.SavedLocation = data[0].SavedLocation;
            $scope.currentItem.FileName = data[0].FileName;
            $scope.currentItem.UploadFiles = '{0}|$|{1}'.format($scope.currentItem.FileName, $scope.currentItem.SavedLocation);
            $scope.$apply();
        }
    });
    //
    $scope.clear = function () {
        $scope.currentItem.SavedLocation = "";
        $scope.currentItem.FileName = "";
        $scope.currentItem.UploadFiles = "";
    }
    $scope.Print = function (AppcertId ) {
        window.open("/NCI_Report/NCI_ReportPreview?templateName=AuditingBILL&id="+ $stateParams.id +"&feeNo=00");
    }
    $scope.init = function () {
        if ($stateParams.id) {
            NCIAAppHospRes.get({ appHospid: $stateParams.id }, function (data) {
                $scope.currentItem = data.Data;
            });
        }
    };
    $scope.init();

}])


