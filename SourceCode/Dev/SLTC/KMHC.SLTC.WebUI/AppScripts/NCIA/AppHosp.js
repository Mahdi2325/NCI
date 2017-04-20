﻿angular.module("sltcApp")
 //护理险资格申请列表
.controller("NCIAAppHospCtrl", ['$scope', 'NCIAAppHospRes', 'utility', function ($scope, NCIAAppHospRes, utility) {
    $scope.Data = {};
    $scope.AuditStatus = [{ "value": "0", "text": "未提交" }, { "value": "1", "text": "已撤回" }, { "value": "3", "text": "待审核" }, { "value": "6", "text": "审核通过" }, { "value": "9", "text": "审核未通过" }]

    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: NCIAAppHospRes,//异步请求的res
            params: { name: "", idno: "", status: -1 },
            success: function (data) {//请求成功时执行函数
                $scope.Data.AppHospList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    };

    $scope.searchInfo = function () {
        if ($scope.options.params.status == null) {
            $scope.options.params.status = -1;
        }

        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    }

    //删除操作
    $scope.deleteAppcert = function (appHosp) {
        appHosp.ActionStatus = -1;
        appHosp.IsDelete = true;
        NCIAAppHospRes.save(appHosp, function (data) {
            if (data.ResultCode == -1) {
                utility.msgwarning("保存失败！" + data.ResultMessage);
                return;
            }
            else {
                utility.message("删除成功！");
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
            }
        });
    };

    //撤回操作
    $scope.cancelApphosp = function (appHosp) {
        appHosp.ActionStatus = 1;
        NCIAAppHospRes.save(appHosp, function (data) {
            if (data.ResultCode == -1) {
                utility.msgwarning("保存失败！" + data.ResultMessage);
                return;
            }
            else {
                utility.message("保存成功！");
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
            }
        });
    };

    //提交操作
    $scope.submitApphosp = function (appHosp) {
        appHosp.ActionStatus = 3;
        NCIAAppHospRes.save(appHosp, function (data) {
            if (data.ResultCode == -1) {
                utility.msgwarning("保存失败！" + data.ResultMessage);
                return;
            }
            else {
                utility.message("保存成功！");
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
            }
        });
    }

    $scope.init();

}])

.controller("NCIAAppHospEditCtrl", ['$scope', '$stateParams', 'NCIAAppHospRes', 'utility', 'webUploader1', function ($scope, $stateParams, NCIAAppHospRes, utility, webUploader1) {
    $scope.currentItem = {};
    $scope.Data = {};
    $scope.showPrint = false;
    //
    $scope.url=[];
    webUploader1.init('#filePicker', { category: 'ArchiveFile' },'110','110', function (file,res) {
        $scope.url.push(file.name+"||"+JSON.parse(res._raw)[0].SavedLocation);
    },function (status) {
        if(status == 0){
            $scope.currentItem.UploadFiles=$scope.url.join(",");
            $scope._currentItem.ActionStatus = 0; //保存
            NCIAAppHospRes.save($scope._currentItem, function (data) {
                if (data.ResultCode == -1) {
                    utility.msgwarning("保存失败！" + data.ResultMessage);
                    return;
                }
                else {
                    $scope.showPrint = true;
                    location.href = "/NCIA/appHosp";
                    utility.message("保存成功！");
                }

            });
        }

    });
    /*webUploader.init('#ArchiveFilePicker', { category: 'ArchiveFile' }, '文件', 'doc,docx,pdf,xls,xlsx,gif,jpg,jpeg,bmp,png', 'doc/!*,application/pdf,image/!*,xls/!*', function (data) {
        if (data.length > 0) {
            $scope.currentItem.SavedLocation = data[0].SavedLocation;
            $scope.currentItem.FileName = data[0].FileName;
            $scope.currentItem.UploadFiles = '{0}|$|{1}'.format($scope.currentItem.FileName, $scope.currentItem.SavedLocation);
            $scope.$apply();
        }
    });*/

    //
    $scope.clear = function () {
        $scope.currentItem.SavedLocation = "";
        $scope.currentItem.FileName = "";
        $scope.currentItem.UploadFiles = "";
    }

    $scope.init = function () {
        if ($stateParams.id) {
            //编辑
            $scope.ssnodisabled = true;
            $scope.idnodisabled = true;
            $scope.showPrint = true;
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
        else {
            //新增
            $scope.ssnodisabled = false;
            $scope.idnodisabled = false;
            $scope.showPrint = false;
        }
    };

    //返回入院申请列表页
    $scope.backList = function () {
        location.href = "/NCIA/appHosp";
    };

    $scope.EnterGetAppcertInfo = function (e, no, type) {
        var keycode = window.event ? e.keyCode : e.which;
        if (keycode == 13) {
            $scope.GetAppcertInfo(no, type);
        }
    }

    $scope.GetAppcertInfo = function (no, type) {
        if(!no) return;
        if (type == "Ss") {
            if(!no) return;
            NCIAAppHospRes.get({ keyNo: no }, function (data) {
                if (data.Data != null) {
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
                }
                else {
                    utility.msgwarning("当前社保卡号不存在有效的申请资格信息！");
                }
            });
        }
        else {
            NCIAAppHospRes.get({ keyNo: no }, function (data) {
                if (data.Data != null) {
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
                }
                else {
                    utility.msgwarning("当前身份证号码不存在有效的申请资格信息！");
                }
            });
        }
    };

    //保存入院申请信息
    $scope.saveAppHosp = function (item) {
        $scope._currentItem = item;
        if(webUploader1.getQueuedFiles()){//判断是否添加文件
            webUploader1.uploadFile();
        }else{
            $scope._currentItem.ActionStatus = 0; //保存
            NCIAAppHospRes.save($scope._currentItem, function (data) {
                if (data.ResultCode == -1) {
                    utility.msgwarning("保存失败！" + data.ResultMessage);
                    return;
                }
                else {
                    $scope.showPrint = true;
                    location.href = "/NCIA/appHosp";
                    utility.message("保存成功！");
                }

            });
        }

    };

    //提交操作
    $scope.submitAppHosp = function (item) {
        item.ActionStatus = 5; //提交
        NCIAAppHospRes.save(item, function (data) {
            if (data.ResultCode == -1) {
                utility.msgwarning("提交失败！" + data.ResultMessage);
                return;
            }
            else {
                location.href = "/NCIA/appHosp";
                utility.message("提交成功！");
            }

        });
    }
    $scope.Print = function (AppcertId) {
        window.open("/NCI_Report/NCI_ReportPreview?templateName=AuditingBILL&id=" + $stateParams.id + "&feeNo=00");
    }
    $scope.init();

}])

.controller("NCIAAppHospViewCtrl", ['$scope', '$stateParams', 'NCIAAppHospRes', 'utility', 'webUploader', function ($scope, $stateParams, NCIAAppHospRes, utility, webUploader) {
    $scope.currentItem = {};
    $scope.init = function () {
        if ($stateParams.id) {
            NCIAAppHospRes.get({ appHospid: $stateParams.id }, function (data) {
                $scope.currentItem = data.Data;
            });
        }
    };

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
    $scope.Print = function (AppcertId) {
        window.open("/NCI_Report/NCI_ReportPreview?templateName=AuditingBILL&id=" + $stateParams.id + "&feeNo=00");
    }
    $scope.init();

}])