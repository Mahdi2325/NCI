angular.module("sltcApp").controller('exportReportCtrl', ['$scope', 'utility', '$state', function ($scope, utility, $state) {
    //当前选中的报表项
    $scope.reportType = "careType";
    $scope.init = function () {
        var date = new Date();
        $scope.startDate = new Date(new Date().setDate(date.getDate() - 45)).format("yyyy-MM-dd");
        $scope.endDate = date.format("yyyy-MM-dd");
    }
    //导出
    $scope.exportRecort = function () {
        switch ($scope.reportType) {
            case "careType":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeByCareTypeReport", $scope.startDate, $scope.endDate), "_blank");
                break;
            case "disease":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeByDiseaseReport", $scope.startDate, $scope.endDate), "_blank");
                break;
            case "ns":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeByOrgReport", $scope.startDate, $scope.endDate), "_blank");
                break;
            case "govArea":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeByRegionReport", $scope.startDate, $scope.endDate), "_blank");
                //utility.message("此报表功能稍后添加");
                break;
            case "lvl":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeByLevelReport", $scope.startDate, $scope.endDate), "_blank");
                break;
            case "feeDtl":
                window.open("/Report/Export?templateName={0}&startTimeStr={1}&endTimeStr={2}&fileTypeStr=xls".format("FeeDetailReport", $scope.startDate, $scope.endDate), "_blank");
                //utility.message("此报表功能稍后添加");
                break;
            default:
                utility.message("请选择报表类型！");
        }

    }

    //选中某个报表
    $scope.clickRecort = function (val) {
        $scope.reportType = val;
    }
    $scope.init();
}]);
