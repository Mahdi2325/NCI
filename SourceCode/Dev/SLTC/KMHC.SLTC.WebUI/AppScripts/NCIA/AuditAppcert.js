angular.module("sltcApp")

 //护理险资格审核列表
.controller("AuditAppcertListCtrl", ['$scope', 'NCIAAuditAppcertRes', 'utility', function ($scope, NCIAAuditAppcertRes, utility) {
    $scope.Data = {};
    $scope.init = function () {
        $scope.loadOrg();
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: NCIAAuditAppcertRes,//异步请求的res
            params: { name: "", idno: "", nsId: "-1", status: -1 },
            success: function (data) {//请求成功时执行函数
                $scope.Data.AppcertList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    };

    $scope.Search = function () {
        if (!angular.isDefined($scope.status) || $scope.status == "") {
            $scope.options.params.status = "-1";
        }
        else {
            $scope.options.params.status = $scope.status;
        }
        if (!angular.isDefined($scope.nsId) || $scope.nsId == "") {
            $scope.options.params.nsId = "-1";
        }
        else {
            $scope.options.params.nsId = $scope.nsId;
        }
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

.controller("AuditAppcertEditCtrl", ['$scope', 'NCIAAuditAppcertRes', '$stateParams', 'utility', 'webUploader', 'auditAgency', '$compile', '$http', function ($scope, NCIAAuditAppcertRes, $stateParams, utility, webUploader, auditAgency, $compile, $http) {
    $scope.currentItem = {};
    $scope.AgencyAsstRecordDetail = {};
    $scope.AgencyAsstRecord = {};
    $scope.AgencyAsstRecordData = {};
    $scope.showAudit = false;
    //
    webUploader.init('#ArchiveFilePicker', { category: 'ArchiveFile' }, '文件', 'doc,docx,pdf,xls,xlsx,gif,jpg,jpeg,bmp,png', 'doc/*,application/pdf,image/*,xls/*', function (data) {
        if (data.length > 0) {
            $scope.currentItem.SavedLocation = data[0].SavedLocation;
            $scope.currentItem.FileName = data[0].FileName;
            $scope.currentItem.UploadFiles = '{0}|$|{1}'.format($scope.currentItem.FileName, $scope.currentItem.SavedLocation);
            $scope.$apply();
        }
    });
    $scope.Print = function (AppcertId, ptype) {
        //debugger;
        if (ptype == "ApplyBILL") {
            window.open("/NCI_Report/NCI_ReportPreview?templateName=ApplyBILL&id=" + AppcertId + "&feeNo=22");
        }
        if (ptype == 'ADLBILL') {
            window.open("/NCI_Report/NCI_ReportPreview?templateName=ADLBILL&id=" + AppcertId + "&feeNo=AdlRec");
            //window.open("/NCI_Report/NCI_ReportPreview?templateName={0}".format("ADLBILL"), "_blank"); AdlRec

        }

    }

    //
    $scope.clear = function () {
        $scope.currentItem.SavedLocation = "";
        $scope.currentItem.FileName = "";
        $scope.currentItem.UploadFiles = "";
    }

    $scope.init = function () {
        $scope.isdisabledAdlSave = false;
        if (angular.isDefined($stateParams.id) && $stateParams.id != "") {
            $scope.AppcertId = $stateParams.id;
        }
        if (angular.isDefined($scope.AppcertId)) {
            NCIAAuditAppcertRes.get({ appcertId: $scope.AppcertId }, function (data) {
                $scope.currentItem = data.Data;
                if ($scope.currentItem.EvaluationTime == null) {
                    $scope.currentItem.EvaluationTime = moment().format('YYYY-MM-DD')
                }

                auditAgency.Comment = $scope.currentItem.AgencyComment;
                if ($scope.currentItem.AgencyapprovedcareType == null)
                {
                    $scope.currentItem.AgencyapprovedcareType = data.Data.NsappcareType;
                }
                NCIAAuditAppcertRes.get({ nsId: $scope.currentItem.NsId, mark: "" }, function (data) {
                    $scope.careTypeList = data.Data;
                })

                //$scope.isExitCareType = false;
                //angular.forEach($scope.careTypeList, function (item) {
                //    if (item.CareTypeID == data.Data.NsappcareType) {
                //        $scope.currentItem.AgencyapprovedcareType = data.Data.NsappcareType;
                //        $scope.isExitCareType = true;
                //    }
                //});
                //if (!$scope.isExitCareType) {
                //    if ($scope.careTypeList != null) {
                //        if ($scope.careTypeList.length > 0) {
                //            $scope.currentItem.AgencyapprovedcareType = $scope.careTypeList[0].CareTypeID;
                //        }
                //    }
                //}
                $scope.AgencyAsstRecord.AppcertId = data.Data.AppcertId;
            });
        }
        else {
            $scope.AppcertId = 0;
        }


    }

    //
    $scope.saveAppcert = function (item) {

        item.ActionStatus = 0;
        NCIAAuditAppcertRes.save(item, function (data) {
            if (data.ResultCode == -1) {
                utility.msgwarning("保存失败！" + data.ResultMessage);
                return;
            }
            $scope.currentItem = data.Data;
            $scope.AgencyAsstRecord.AppcertId = data.Data.AppcertId;
            $scope.GetEvalData('ADL');
            $scope.isAdlActive = true;
            utility.message("保存成功");

        });

    }
    //
    $scope.submitAuditAppcert = function (item) {
        if ($scope.AgencyAsstRecord.TotalScore >= 60) {
            auditAgency.Comment = "日常生活能力评定量表总分大于或等于60分";
        }
        else {
            auditAgency.Comment = "";
        }
        var html = '<div km-include ' +
               'km-template="Views/NCIA/AuditConment.html" ' +
               'km-controller="auditConment" " </div>';
        BootstrapDialog.show({
            title: '<label class=" control-label">审核结果</label>',
            closable: false,
            size: BootstrapDialog.SIZE_WIDE,
            //message: $('<div  ng-controller="auditConment"></div>').load('/Views/NCIA/AuditConment.html'),
            message: html,
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            },
            buttons: [{
                label: '通过',
                cssClass: 'btn-success',
                hotkey: 13, // Enter.
                action: function (dialogRef) {
                    $scope.currentItem.AgencyComment = auditAgency.Comment;
                    $scope.currentItem.ActionStatus = 6;
                    NCIAAuditAppcertRes.save($scope.currentItem, function (data) {
                        if (data.ResultCode == -1) {
                            if ($scope.AgencyAsstRecord.TotalScore < 40) {
                                dialogRef.close();
                                utility.msgwarning("日常生活能力评定量表未保存！");
                                return;
                            }
                            dialogRef.close();
                            utility.msgwarning("审核无法通过！" + data.ResultMessage);
                            return;
                        }
                        $scope.isdisabledAdlSave = true;
                        $scope.currentItem = data.Data;
                        $scope.AgencyAsstRecord.AppcertId = data.Data.AppcertId;
                        $scope.PutAppcertRate(data.Data.NsId);
                        dialogRef.close();
                        utility.message("提交审核结果成功");
                    });
                }
            }, {
                label: '不通过',
                cssClass: 'btn-danger',
                action: function (dialogRef) {
                    $scope.currentItem.AgencyComment = auditAgency.Comment;
                    $scope.currentItem.ActionStatus = 9;
                    NCIAAuditAppcertRes.save($scope.currentItem, function (data) {
                        if (data.ResultCode == -1) {
                            dialogRef.close();
                            utility.msgwarning("无法提交审核结果！" + data.ResultMessage);
                            return;
                        }
                        $scope.isdisabledAdlSave = true;
                        $scope.currentItem = data.Data;
                        $scope.AgencyAsstRecord.AppcertId = data.Data.AppcertId;
                        $scope.PutAppcertRate(data.Data.NsId);
                        dialogRef.close();
                        utility.message("提交审核结果成功");
                    });

                }
            }, {
                label: '取消',
                action: function (dialogRef) {
                    dialogRef.close();
                }
            }

            ]
        });
    }
    //调用API接口
    $scope.PutAppcertRate = function (nsId) {
        var year = new Date().getFullYear();
        $http({
            url: 'api/OrgStatistics/PutAppcertRate?NSID=' + nsId + '&YEAR=' + year,
            method: 'GET'
        }).then(function (res) {

        })
    }

    //
    $scope.GetEvalData = function (val) {
        if ($scope.showADL == false) {
            return;
        }
        $scope.AgencyAsstRecord.TotalScore = 0;
        $scope.isAdlActive = true;
        $scope.RegQuestion = {};
        $scope.currentCode = val;
        NCIAAuditAppcertRes.get({ code: val }, function (data) {
            $scope.MakerItem = data.Data.MakerItem;
            $scope.QuestionResultsList = data.Data.QuestionResults;
            $scope.QuestionName = data.Data.QuestionName;
            $scope.currentQuestionId = data.Data.QuestionId;
            //加载具体评估数据
            NCIAAuditAppcertRes.get({ appcertId: $scope.AppcertId, mark: "AdlRec" }, function (data) {
                if (data.Data != null) {
                    if (data.Data.AgencyAsstRecord != null && data.Data.AgencyAsstRecordDetail != null) {
                        $scope.showAudit = true;
                        $scope.AgencyAsstRecord.AgencyAsstRecordId = data.Data.AgencyAsstRecord.AgencyAsstRecordId;
                        $scope.AgencyAsstRecord.AppcertId = data.Data.AgencyAsstRecord.AppcertId;
                        $scope.AgencyAsstRecord.TotalScore = data.Data.AgencyAsstRecord.TotalScore;
                        $scope.AgencyAsstRecord.AsstDate = $scope.AgencyAsstRecord.AsstDate;
                        $scope.AgencyAsstRecord.IsExpired = data.Data.AgencyAsstRecord.IsExpired;
                        $scope.AgencyAsstRecord.QuestionId = data.Data.AgencyAsstRecord.QuestionId;
                        if (data.Data.AgencyAsstRecordDetail != null && data.Data.AgencyAsstRecordDetail.length > 0) {
                            angular.forEach($scope.MakerItem, function (item) {
                                angular.forEach(data.Data.AgencyAsstRecordDetail, function (subItem) {
                                    if (item.MakerId == subItem.MakerId) {
                                        item.LimitedValueId = subItem.LimitedValueId;
                                        item.LimitedValue = subItem.MakerValue;
                                        item.RegQuestionDataId = subItem.AgencyAsstRecordDetailId;
                                    }
                                });
                            });
                        }
                    }
                }
            });
        });
    }
    //计算评估总分和评估结果
    $scope.calcResult = function (Item, answer) {
        if (answer.LimitedValue < 99) {
            Item.LimitedValue = answer.LimitedValue;
        }
        $scope.AgencyAsstRecord.TotalScore = 0;
        angular.forEach($scope.MakerItem, function (item) {
            $scope.AgencyAsstRecord.TotalScore = $scope.AgencyAsstRecord.TotalScore + item.LimitedValue;
        });
    }
    //
    $scope.saveADL = function () {
        $scope.isSaved = false;
        angular.forEach($scope.MakerItem, function (item) {
            if (item.LimitedValueId == 0) {
                utility.msgwarning(item.MakeName + "没有选择！");
                $scope.isSaved = true;
            }
        });
        if ($scope.isSaved) {
            return;
        }

        $scope.AgencyAsstRecord.QuestionId = $scope.currentQuestionId;
        $scope.AgencyAsstRecordData.AgencyAsstRecord = $scope.AgencyAsstRecord;
        $scope.AgencyAsstRecordData.AgencyAsstRecordDetail = [];
        angular.forEach($scope.MakerItem, function (item) {
            $scope.AgencyAsstRecordDetail.AgencyAsstRecordDetailId = item.RegQuestionDataId;
            $scope.AgencyAsstRecordDetail.MakerId = item.MakerId;
            $scope.AgencyAsstRecordDetail.MakerValue = item.LimitedValue;
            $scope.AgencyAsstRecordDetail.LimitedValueId = item.LimitedValueId;
            $scope.AgencyAsstRecordDetail.QuestionId = $scope.currentQuestionId;
            $scope.AgencyAsstRecordData.AgencyAsstRecordDetail.push($scope.AgencyAsstRecordDetail);
            $scope.AgencyAsstRecordDetail = {};
        });
        NCIAAuditAppcertRes.saveADL($scope.AgencyAsstRecordData, function (data) {
            if (data.Data != null) {
                if (data.Data.AgencyAsstRecord != null) {
                    $scope.AgencyAsstRecord.AgencyAsstRecordId = data.Data.AgencyAsstRecord.AgencyAsstRecordId;
                    $scope.currentItem.AgencyAsstRecordId = data.Data.AgencyAsstRecord.AgencyAsstRecordId;
                    $scope.GetEvalData('ADL');
                    NCIAAuditAppcertRes.save($scope.currentItem, function (data) {
                        $scope.showAudit = true;
                        utility.message("保存成功");
                    });
                }
            }
        });
    }



    $scope.closeAppcert = function () {
        location.href = "/NCIA/auditAppcertList";
    }

    $scope.init();

    $scope.getnursingHomeAsstRecordDetail = function () {
        var html = '<div km-include ' +
               'km-template="Views/NCIA/NursingHomeAdlForm.html" ' +
               'km-controller="nursingHomeAdlCtrl" km-include-params="{id:\'' + $scope.AppcertId + '\'}"</div>';
        BootstrapDialog.show({
            title: '<label class="control-label">定点机构评分详情</label>',
            closable: true,
            size: BootstrapDialog.SIZE_WIDE,
            message: html,
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            },
            buttons: [{
                label: '关闭',
                action: function (dialogRef) {
                    dialogRef.close();
                }
            }

            ]
        });
    }

}])
.controller("auditConment", ['$scope', 'auditAgency', function ($scope, auditAgency) {
    $scope.AgencyComment = auditAgency.Comment;
    $scope.$watch("AgencyComment", function (newValue) {
        if (angular.isDefined(newValue) && newValue.length != null) {
            if (newValue.length > 500) {
                newValue = newValue.substring(0, 499);
            }
            auditAgency.Comment = newValue;
        }
    })
}])
.controller("nursingHomeAdlCtrl", ['$scope', 'NCIAAppcertRes', '$stateParams', function ($scope, NCIAAppcertRes, $stateParams) {
    $scope.init = function () {
        if (angular.isDefined($stateParams.id) && $stateParams.id != "") {
            $scope.AppcertId = $stateParams.id;
        }
        if (angular.isDefined($scope.AppcertId)) {
            $scope.RegQuestion = {};
            NCIAAppcertRes.get({ code: "ADL" }, function (data) {
                $scope.MakerItem = data.Data.MakerItem;
                $scope.QuestionResultsList = data.Data.QuestionResults;
                $scope.QuestionName = data.Data.QuestionName;
                $scope.currentQuestionId = data.Data.QuestionId;
                //加载具体评估数据
                NCIAAppcertRes.get({ appcertId: $scope.AppcertId, mark: "AdlRec" }, function (data) {
                    if (data.Data != null) {
                        if (data.Data.NursingHomeAsstRecord != null && data.Data.NursingHomeAsstRecordDetail != null) {
                            $scope.TotalScore = data.Data.NursingHomeAsstRecord.TotalScore;
                            if (data.Data.NursingHomeAsstRecordDetail != null && data.Data.NursingHomeAsstRecordDetail.length > 0) {
                                angular.forEach($scope.MakerItem, function (item) {
                                    angular.forEach(data.Data.NursingHomeAsstRecordDetail, function (subItem) {
                                        if (item.MakerId == subItem.MakerId) {
                                            item.LimitedValueId = subItem.LimitedValueId;
                                            item.LimitedValue = subItem.MakerValue;
                                            angular.forEach(item.MakerItemLimitedValue, function (answer) {
                                                if (answer.LimitedValueId == item.LimitedValueId) {
                                                    item.LimitedValueName = answer.LimitedValueName;
                                                }
                                            });
                                        }
                                    });
                                });
                            }
                        }
                    }
                });
            });
        }
    }

    $scope.init();
}])

