angular.module("sltcApp")
.controller("reportTempManageCtrl", ['$scope','$q', 'ReportTempManageRes', 'utility', 'NCIAAuditAppcertRes', function ($scope,$q, ReportTempManageRes, utility, NCIAAuditAppcertRes) {
    Handlebars.registerHelper("prettifyDate", function (timestamp) {
        return timestamp ? moment(timestamp).format('YYYY-MM-DD') : null;
    });
    $scope.loadOrg = function () {
        $scope.OrgData = {};
        NCIAAuditAppcertRes.get({ org: 0 }, function (data) {
            $scope.OrgData = data.Data;
            if ($scope.OrgData.length > 0) {
                $scope.options.params.nsno = $scope.OrgData[0].NSNo;
            }
        });
    };
    $scope.init = function () {
        $scope.loadOrg();
        $scope.data = {};

        $scope.eDate = moment().format('YYYY-MM');
        $scope.sDate = moment().subtract(1, "M").format('YYYY-MM');
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: ReportTempManageRes,//异步请求的res
            params: { startDate: $scope.sDate, endDate: $scope.eDate, mark: "" },
            success: function (data) {//请求成功时执行函数
                if (data.RecordsCount == 0) {
                    utility.showNoData();
                    return;
                }
                $scope.Data = data.Data;
                if (!$scope.reportType) return;
                var source = '';
                switch ($scope.reportType) {
                    case "feeDtl":
                        source = "#FeeDetailReport";
                        break;
                    case "disease":
                        source = "#FeeByDiseaseReport";
                        break;
                    case "feeTreat":
                        source = "#FeeBydaiyu";
                        break;
                    case "feeApproval":
                        source = "#FeeBydaiyu";
                        break;
                    case "MonthFee":
                        source = "#MonthFee";
                        break;
                    default:
                        source = "#commonReport";
                        break;
                }
                var code = $(source).html();
                var template = Handlebars.compile(code);
                //注册一个比较大小的Helper,判断v1是否大于v2
                Handlebars.registerHelper("compare", function (v1, v2, options) {
                    if (v1 == v2) {
                        //满足添加继续执行
                        return options.fn(this);
                    } else {
                        //不满足条件执行{{else}}部分
                        return options.inverse(this);
                    }
                });
                var dom = template($scope.Data);
                $("#reportListTab").html(dom);
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    };

    $scope.exportRecort = function () {
        if ($scope.reportType == "MonthFee" && !$scope.options.params.nsno) {
            utility.message("请选择机构！");
            return;
        }
        switch ($scope.reportType) {
            case "careType":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeByCareTypeReport", $scope.sDate, $scope.eDate), "_blank");
                break;
            case "disease":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeByDiseaseReport", $scope.sDate, $scope.eDate), "_blank");
                break;
            case "ns":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeByOrgReport", $scope.sDate, $scope.eDate), "_blank");
                break;
            case "govArea":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeByRegionReport", $scope.sDate, $scope.eDate), "_blank");
                //utility.message("此报表功能稍后添加");
                break;
            case "lvl":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeByLevelReport", $scope.sDate, $scope.eDate), "_blank");
                break;
            case "feeDtl":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeDetailReport", $scope.sDate, $scope.eDate), "_blank");
                //utility.message("此报表功能稍后添加");
                break;
            case "feeTreat":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeByCareTypeDetailReport", $scope.sDate, $scope.eDate), "_blank");
                break;
            case "feeApproval":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeApprovalReport", $scope.sDate, $scope.eDate), "_blank");
                break;
            case "MonthFee":
                window.open("/ExportExcelReport/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls&nsno={3}".format("MonthFeeReport", $scope.sDate, $scope.eDate, $scope.options.params.nsno), "_blank");
                break;
            default:
                utility.message("请选择报表类型！");
        }
    }

    $scope.Search = function () {
        if (!$scope.sDate) {
            utility.message("请选择开始月份！");
            return;
        }
        if (!$scope.eDate) {
            utility.message("请选择结束月份！");
            return;
        }
        if (!$scope.reportType) {
            utility.message("请选择报表名称！");
            return;
        }
        if ($scope.reportType == "MonthFee" && !$scope.options.params.nsno) {
            utility.message("请选择机构！");
            return;
        }
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.pageInfo.PageSize = 10;
        $scope.options.params.mark = $scope.reportType;
        $scope.options.params.startDate = $scope.sDate;
        $scope.options.params.endDate = $scope.eDate;
        $scope.options.search();
    }


    $scope.init();

}])
.controller("reportPersonStatus", ['$scope', '$http', 'PersonStatusReportRes', 'utility', function ($scope, $http, PersonStatusReportRes, utility) {

    $scope.init = function () {
        $scope.data = {};
        var d = new Date(),
            dd = d.getDay(),
            friday = new Date((5 - dd) * (3600 * 24 * 1000) + d.getTime());
        $scope.eDate = moment(friday).format('YYYY-MM-DD');
        $scope.sDate = moment($scope.eDate).subtract(6, "days").format('YYYY-MM-DD');
        $scope.loadPersonStatus($scope.sDate, $scope.eDate);
    };

    $scope.exportRecort = function () {
        window.open("/ExportExcelReport/TaskExport?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("PersonStatusReport", $scope.sDate, $scope.eDate), "_blank");
    }

    $scope.Search = function () {
        if (!$scope.sDate) {
            utility.message("请选择开始日期！");
            return;
        }
        if (!$scope.eDate) {
            utility.message("请选择结束日期！");
            return;
        }
        $scope.data = {};
        $scope.loadPersonStatus($scope.sDate, $scope.eDate);

    }

    $scope.loadPersonStatus = function (starttime, endtime) {
        PersonStatusReportRes.get({ startDate: starttime, endDate: endtime }, function (data) {
            if (data.ResultCode == -1) {
                utility.message(data.ResultMessage);
            }
            $scope.data = data.Data;
            var code = $("#FeeByZige").html();
            var template = Handlebars.compile(code);

            var dom = template($scope.data);
            $("#reportListTab").html(dom);
        });
    }


    $scope.init();

}])