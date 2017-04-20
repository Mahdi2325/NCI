angular.module("sltcApp")
 //护理险资格申请列表
.controller("AppcertListCtrl", ['$scope', 'NCIAAppcertRes', 'utility', 'cfpLoadingBar', function ($scope, NCIAAppcertRes, utility, cfpLoadingBar) {
    $scope.Data = {};
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: NCIAAppcertRes,//异步请求的res
            params: { name: "", idno: "", status: -1 },
            success: function (data) {//请求成功时执行函数
                cfpLoadingBar.complete();
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
        cfpLoadingBar.start();
        $scope.options.search();
    }

    //提交
    $scope.submitAppcert = function (item) {
        if (confirm("您确定提交吗?")) {
            item.ActionStatus = 3;
            NCIAAppcertRes.save(item, function (data) {
                if (data.ResultCode == -1) {
                    utility.msgwarning("提交失败！" + data.ResultMessage);
                    return; ss
                }
                item.Status = data.Data.Status;
                utility.message("提交成功")
            });
        }
    }

    //撤回
    $scope.cancelAppcert = function (item) {
        if (confirm("您确定撤回吗?")) {
            item.ActionStatus = 1;
            NCIAAppcertRes.save(item, function (data) {
                item.Status = data.Data.Status;
                utility.message("撤回成功");
            });
        }
    }

    //删除
    $scope.deleteAppcert = function (item) {
        if (confirm("您确定删除吗?")) {
            item.ActionStatus = -1;
            NCIAAppcertRes.save(item, function (data) {
                $scope.Data.AppcertList.splice($scope.Data.AppcertList.indexOf(item), 1);
                $scope.options.search()
                utility.message("删除成功");
            });
        }
    }

    $scope.init();

}])
//护理险资格申请编辑
.controller("AppcertEditCtrl", ['$scope', '$q', 'NCIAAppcertRes', '$stateParams', 'utility', 'webUploader1', '$compile', 'cfpLoadingBar', function ($scope, $q, NCIAAppcertRes, $stateParams, utility, webUploader1, $compile, cfpLoadingBar) {
    $scope.currentItem = {};
    $scope.NursingHomeAsstRecordDetail = {};
    $scope.NursingHomeAsstRecord = {};
    $scope.NursingHomeAsstRecordData = {};
    $scope.oldItem = {}
    $scope.showSubmit = false;
    //
    $scope.url = [];
    webUploader1.init('#filePicker', { category: 'ArchiveFile' }, '110', '110', function (file, res) {
        $scope.url.push(file.name + "||" + JSON.parse(res._raw)[0].SavedLocation);
    }, function (status) {
        if (status == 0) {
            $scope.currentItem.UploadFiles = $scope.url.join(",");
            if (angular.isDefined($scope.BaseAppcertIdOfReApp)) {

                $scope.oldItem.ActionStatus = 11;
                $.blockUI({ message: '', css: '' });
                NCIAAppcertRes.save($scope.oldItem, function (data) {
                    $.unblockUI();
                    $scope._currentItem.ActionStatus = 0;
                    $scope._currentItem.BaseAppcertIdOfReApp = $scope.oldItem.AppcertId;
                    NCIAAppcertRes.save($scope._currentItem, function (data) {

                        if (data.ResultCode == -1) {
                            utility.msgwarning("保存失败！" + data.ResultMessage);
                            return;
                        }
                        if ($scope.isAdd) {
                            $scope.AppcertId = data.Data.AppcertId;
                        }
                        $scope.currentItem = data.Data;
                        $scope.showADL = true;
                        $scope.NursingHomeAsstRecord.AppcertId = data.Data.AppcertId;

                        utility.message("保存成功");
                    });
                });
            }
            else {
                $scope._currentItem.ActionStatus = 0;
                $.blockUI({ message: '', css: '' });

                NCIAAppcertRes.save($scope._currentItem, function (data) {
                    $.unblockUI();

                    if (data.ResultCode == -1) {
                        utility.msgwarning("保存失败！" + data.ResultMessage);
                        return;
                    }
                    if ($scope.isAdd) {
                        $scope.AppcertId = data.Data.AppcertId;
                    }
                    $scope.currentItem = data.Data;
                    $scope.showADL = true;
                    $scope.NursingHomeAsstRecord.AppcertId = data.Data.AppcertId;
                    utility.message("保存成功");
                });
            }
        }

    });

    // 初始化Web Uploader
    // var uploader = WebUploader.create({
    //
    //     // 选完文件后，是否自动上传。
    //     auto: false,
    //     // swf:'https://cdn.staticfile.org/webuploader/0.1.5/Uploader.swf',
    //     // 文件接收服务端。
    //     server: '/api/Upload',
    //     thumb:{
    //         width: 110,
    //         height: 110,
    //
    //         // 图片质量，只有type为`image/jpeg`的时候才有效。
    //         quality: 70,
    //
    //         // 是否允许放大，如果想要生成小图的时候不失真，此选项应该设置为false.
    //         allowMagnify: false,
    //
    //         // 是否允许裁剪。
    //         crop: true,
    //
    //         // 为空的话则保留原有图片格式。
    //         // 否则强制转换成指定的类型。
    //         type: 'image/jpeg'
    //     },
    //     // 选择文件的按钮。可选。
    //     // 内部根据当前运行是创建，可能是input元素，也可能是flash.
    //     pick: '#filePicker',
    //     fileNumLimit:5,
    //     formData: {
    //         category: 'ArchiveFile'
    //     },
    //     // 只允许选择图片文件。
    //     accept: {
    //         title: 'Images',
    //         extensions: 'gif,jpg,jpeg,bmp,png',
    //         mimeTypes: 'image/*'
    //     }
    // });
    //
    // // 当有文件添加进来的时候
    // uploader.on( 'fileQueued', function( file ) {
    //     var $li = $(
    //             '<div id="' + file.id + '"  title="' + file.name + '" class="file-item thumbnail fl">' +
    //             '<img>' +
    //             '</div>'
    //         ),
    //         $img = $li.find('img');
    //
    //     var $list=$('#fileList');
    //     // $list为容器jQuery实例
    //     $list.append( $li );
    //
    //     // 创建缩略图
    //     // 如果为非图片文件，可以不用调用此方法。
    //     // thumbnailWidth x thumbnailHeight 为 100 x 100
    //     uploader.makeThumb( file, function( error, src ) {
    //         if ( error ) {
    //             $img.replaceWith('<span>不能预览</span>');
    //             return;
    //         }
    //         $img.attr( 'src', src );
    //     });
    //     $li.on('click', function() {
    //         uploader.removeFile( $(this).closest('.file-item').attr('id'),true );
    //         $(this).remove();
    //     })
    // });
    // // 文件上传过程中创建进度条实时显示。
    // uploader.on( 'uploadProgress', function( file, percentage ) {
    //     var $li = $( '#'+file.id ),
    //         $percent = $li.find('.progress span');
    //
    //     // 避免重复创建
    //     if ( !$percent.length ) {
    //         $percent = $('<p class="progress"><span></span></p>')
    //             .appendTo( $li )
    //             .find('span');
    //     }
    //
    //     $percent.css( 'width', percentage * 100 + '%' );
    // });

    //
    $scope.clear = function () {
        $scope.currentItem.SavedLocation = "";
        $scope.currentItem.FileName = "";
        $scope.currentItem.UploadFiles = "";
    }

    $scope.loadCareType = function () {
        NCIAAppcertRes.get(function (data) {
            $scope.careTypeList = data.Data;
            if ($scope.careTypeList != null) {
                if ($scope.careTypeList.length > 0) {
                    $scope.currentItem.NsappcareType = $scope.careTypeList[0].CareType;
                }
            }
        })
    }
    $scope.init = function () {
        $scope.loadCareType();
        if (angular.isDefined($stateParams.ssNo) && $stateParams.ssNo != "") {
            $scope.GetApplicantInfo($stateParams.ssNo, 'Ss');
        }
        if (angular.isDefined($stateParams.oldId) && $stateParams.oldId != "") {
            $scope.showADL = false;
            $scope.isAdd = true;
            $scope.BaseAppcertIdOfReApp = $stateParams.oldId;
            NCIAAppcertRes.get({ appcertId: $scope.BaseAppcertIdOfReApp }, function (data) {
                $scope.oldItem = data.Data;
                $scope.currentItem.Name = data.Data.Name;
                $scope.currentItem.Gender = data.Data.Gender;
                $scope.currentItem.Age = data.Data.Age;
                $scope.currentItem.SsNo = data.Data.SsNo;
                $scope.currentItem.IdNo = data.Data.IdNo;
                $scope.currentItem.McType = data.Data.McType;
                $scope.currentItem.Disease = data.Data.Disease;
                $scope.currentItem.Address = data.Data.Address;
                $scope.currentItem.Phone = data.Data.Phone;
                $scope.currentItem.ApplicantId = data.Data.ApplicantId;
                //$scope.currentItem.NsappcareType = 3;
            });
        }
        else {
            $scope.isAdd = true;
            if (angular.isDefined($stateParams.id) && $stateParams.id != "") {
                $scope.isAdd = false;
                $scope.AppcertId = $stateParams.id;
            }
            $scope.showADL = false;
            if (angular.isDefined($scope.AppcertId)) {
                $scope.showADL = true;
                NCIAAppcertRes.get({ appcertId: $scope.AppcertId }, function (data) {
                    $scope.currentItem = data.Data;
                    $scope.NursingHomeAsstRecord.AppcertId = data.Data.AppcertId;
                });
            }
            else {
                $scope.AppcertId = 0;
                //$scope.currentItem.NsappcareType = 3;
            }
        }

    }
    //
    $scope.EnterGetApplicantInfo = function (e, val, mark) {
        var keycode = window.event ? e.keyCode : e.which;
        if (keycode == 13) {
            $scope.GetApplicantInfo(val, mark);
        }
    }
    //
    $scope.GetApplicantInfo = function (val, mark) {
        if (!val) return;
        var tempKeyNo = val;
        var tempNsappcareType = $scope.currentItem.NsappcareType;
        NCIAAppcertRes.get({ keyNo: val }, function (data) {
            if (data.Data != null) {
                $scope.currentItem.Name = data.Data.Name;
                $scope.currentItem.Gender = data.Data.Gender;
                $scope.currentItem.Age = data.Data.Age;
                $scope.currentItem.SsNo = data.Data.Ssno;
                $scope.currentItem.IdNo = data.Data.Idno;
                $scope.currentItem.McType = data.Data.McType;
                $scope.currentItem.Disease = data.Data.Disease;
                $scope.currentItem.Address = data.Data.Address;
                $scope.currentItem.Phone = data.Data.Phone;
                $scope.currentItem.ApplicantId = data.Data.ApplicantId;
            }
            else {
                $scope.currentItem = {};
                $scope.currentItem.NsappcareType = tempNsappcareType;
                if (mark == 'Ss') {
                    $scope.currentItem.SsNo = tempKeyNo;
                }
                else if (mark == 'Id') {
                    $scope.currentItem.IdNo = tempKeyNo;
                }

            }

        });
    }
    //
    $scope.saveAppcert = function (item) {
        $scope._currentItem = item;
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
        //webupload
        if (webUploader1.getQueuedFiles()) {//判断是否添加文件
            webUploader1.uploadFile();
        } else {
            if (angular.isDefined($scope.BaseAppcertIdOfReApp)) {

                $scope.oldItem.ActionStatus = 11;
                $.blockUI({ message: '', css: '' });
                NCIAAppcertRes.save($scope.oldItem, function (data) {
                    $.unblockUI();
                    $scope._currentItem.ActionStatus = 0;
                    $scope._currentItem.BaseAppcertIdOfReApp = $scope.oldItem.AppcertId;
                    NCIAAppcertRes.save($scope._currentItem, function (data) {

                        if (data.ResultCode == -1) {
                            utility.msgwarning("保存失败！" + data.ResultMessage);
                            return;
                        }
                        if ($scope.isAdd) {
                            $scope.AppcertId = data.Data.AppcertId;
                        }
                        $scope.currentItem = data.Data;
                        $scope.showADL = true;
                        $scope.NursingHomeAsstRecord.AppcertId = data.Data.AppcertId;

                        utility.message("保存成功");
                    });
                });
            }
            else {
                $scope._currentItem.ActionStatus = 0;
                $.blockUI({ message: '', css: '' });

                NCIAAppcertRes.save($scope._currentItem, function (data) {
                    $.unblockUI();

                    if (data.ResultCode == -1) {
                        utility.msgwarning("保存失败！" + data.ResultMessage);
                        return;
                    }
                    if ($scope.isAdd) {
                        $scope.AppcertId = data.Data.AppcertId;
                    }
                    $scope.currentItem = data.Data;
                    $scope.showADL = true;
                    $scope.NursingHomeAsstRecord.AppcertId = data.Data.AppcertId;
                    utility.message("保存成功");
                });
            }
        }

    }
    //
    $scope.submitAppcert = function (item) {
        //提交
        item.TotalScore = $scope.NursingHomeAsstRecord.TotalScore;
        item.ActionStatus = 3;
        NCIAAppcertRes.save(item, function (data) {
            if (data.ResultCode == -1) {
                utility.msgwarning("提交失败！" + data.ResultMessage);
                return;
            }
            item.Status = data.Data.Status;
            utility.message("提交成功！");
            location.href = "/NCIA/appcertList";
        });
    }
    //
    $scope.GetEvalData = function (val) {
        if ($scope.showADL == false) {
            return;
        }
        $scope.NursingHomeAsstRecord.TotalScore = 0;
        $scope.RegQuestion = {};
        $scope.currentCode = val;
        NCIAAppcertRes.get({ code: val }, function (data) {
            $scope.MakerItem = data.Data.MakerItem;
            $scope.QuestionResultsList = data.Data.QuestionResults;
            $scope.QuestionName = data.Data.QuestionName;
            $scope.currentQuestionId = data.Data.QuestionId;
            //加载具体评估数据
            NCIAAppcertRes.get({ appcertId: $scope.AppcertId, mark: "AdlRec" }, function (data) {
                if (data.Data != null) {
                    if (data.Data.NursingHomeAsstRecord != null && data.Data.NursingHomeAsstRecordDetail != null) {
                        $scope.showSubmit = true;
                        $scope.NursingHomeAsstRecord.NsAsstRecordId = data.Data.NursingHomeAsstRecord.NsAsstRecordId;
                        $scope.NursingHomeAsstRecord.AppcertId = data.Data.NursingHomeAsstRecord.AppcertId;
                        $scope.NursingHomeAsstRecord.TotalScore = data.Data.NursingHomeAsstRecord.TotalScore;
                        $scope.NursingHomeAsstRecord.AsstDate = $scope.NursingHomeAsstRecord.AsstDate;
                        $scope.NursingHomeAsstRecord.IsExpired = data.Data.NursingHomeAsstRecord.IsExpired;
                        $scope.NursingHomeAsstRecord.QuestionId = data.Data.NursingHomeAsstRecord.QuestionId;
                        if (data.Data.NursingHomeAsstRecordDetail != null && data.Data.NursingHomeAsstRecordDetail.length > 0) {
                            angular.forEach($scope.MakerItem, function (item) {
                                angular.forEach(data.Data.NursingHomeAsstRecordDetail, function (subItem) {
                                    if (item.MakerId == subItem.MakerId) {
                                        item.LimitedValueId = subItem.LimitedValueId;
                                        item.LimitedValue = subItem.MakerValue;
                                        item.RegQuestionDataId = subItem.NsAsstRecordDetailId;
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
        $scope.NursingHomeAsstRecord.TotalScore = 0;
        angular.forEach($scope.MakerItem, function (item) {
            $scope.NursingHomeAsstRecord.TotalScore = $scope.NursingHomeAsstRecord.TotalScore + item.LimitedValue;
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
        $scope.NursingHomeAsstRecord.QuestionId = $scope.currentQuestionId;
        $scope.NursingHomeAsstRecordData.NursingHomeAsstRecord = $scope.NursingHomeAsstRecord;
        $scope.NursingHomeAsstRecordData.NursingHomeAsstRecordDetail = [];
        angular.forEach($scope.MakerItem, function (item) {
            $scope.NursingHomeAsstRecordDetail.NsAsstRecordDetailId = item.RegQuestionDataId;
            $scope.NursingHomeAsstRecordDetail.MakerId = item.MakerId;
            $scope.NursingHomeAsstRecordDetail.MakerValue = item.LimitedValue;
            $scope.NursingHomeAsstRecordDetail.LimitedValueId = item.LimitedValueId;
            $scope.NursingHomeAsstRecordDetail.QuestionId = $scope.currentQuestionId;
            $scope.NursingHomeAsstRecordData.NursingHomeAsstRecordDetail.push($scope.NursingHomeAsstRecordDetail);
            $scope.NursingHomeAsstRecordDetail = {};
        });
        NCIAAppcertRes.saveADL($scope.NursingHomeAsstRecordData, function (data) {
            if (data.Data != null) {
                if (data.Data.NursingHomeAsstRecord != null) {
                    $scope.NursingHomeAsstRecord.NsAsstRecordId = data.Data.NursingHomeAsstRecord.NsAsstRecordId;
                    $scope.currentItem.NsAsstRecordId = data.Data.NursingHomeAsstRecord.NsAsstRecordId;
                    $scope.GetEvalData('ADL');
                    NCIAAppcertRes.save($scope.currentItem, function (data) {
                        $scope.showSubmit = true;
                        utility.message("保存成功");
                    });
                }
            }
        });
    }



    $scope.closeAppcert = function () {
        location.href = "/NCIA/appcertList";
    }

    $scope.init();

    function jsGetAge(strBirthday) {
        var returnAge;
        var strBirthdayArr = strBirthday.split("-");
        var birthYear = strBirthdayArr[0];
        var birthMonth = strBirthdayArr[1];
        var birthDay = strBirthdayArr[2];

        d = new Date();
        var nowYear = d.getFullYear();
        var nowMonth = d.getMonth() + 1;
        var nowDay = d.getDate();

        if (nowYear == birthYear) {
            returnAge = 0;//同年 则为0岁  
        }
        else {
            var ageDiff = nowYear - birthYear; //年之差  
            if (ageDiff > 0) {
                if (nowMonth == birthMonth) {
                    var dayDiff = nowDay - birthDay;//日之差  
                    if (dayDiff < 0) {
                        returnAge = ageDiff - 1;
                    }
                    else {
                        returnAge = ageDiff;
                    }
                }
                else {
                    var monthDiff = nowMonth - birthMonth;//月之差  
                    if (monthDiff < 0) {
                        returnAge = ageDiff - 1;
                    }
                    else {
                        returnAge = ageDiff;
                    }
                }
            }
            else {
                returnAge = -1;//返回-1 表示出生日期输入错误 晚于今天  
            }
        }

        return returnAge;//返回周岁年龄  

    }

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

    $scope.getAgencyAsstRecordDetail = function () {
        var html = '<div km-include ' +
               'km-template="Views/NCIA/AgencyAdlForm.html" ' +
               'km-controller="agencyAdlCtrl" km-include-params="{id:\'' + $scope.AppcertId + '\'}"</div>';
        BootstrapDialog.show({
            title: '<label class="control-label">经办机构评分详情</label>',
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
.controller("agencyAdlCtrl", ['$scope', 'NCIAAuditAppcertRes', '$stateParams', function ($scope, NCIAAuditAppcertRes, $stateParams) {
    $scope.init = function () {
        if (angular.isDefined($stateParams.id) && $stateParams.id != "") {
            $scope.AppcertId = $stateParams.id;
        }
        if (angular.isDefined($scope.AppcertId)) {
            $scope.RegQuestion = {};
            NCIAAuditAppcertRes.get({ code: "ADL" }, function (data) {
                $scope.MakerItem = data.Data.MakerItem;
                $scope.QuestionResultsList = data.Data.QuestionResults;
                $scope.QuestionName = data.Data.QuestionName;
                $scope.currentQuestionId = data.Data.QuestionId;
                //加载具体评估数据
                NCIAAuditAppcertRes.get({ appcertId: $scope.AppcertId, mark: "AdlRec" }, function (data) {
                    if (data.Data != null) {
                        if (data.Data.AgencyAsstRecord != null && data.Data.AgencyAsstRecordDetail != null) {
                            $scope.TotalScore = data.Data.AgencyAsstRecord.TotalScore;
                            if (data.Data.AgencyAsstRecordDetail != null && data.Data.AgencyAsstRecordDetail.length > 0) {
                                angular.forEach($scope.MakerItem, function (item) {
                                    angular.forEach(data.Data.AgencyAsstRecordDetail, function (subItem) {
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
