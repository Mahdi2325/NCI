angular.module("sltcApp")
.controller("monthFeeStatisticsCtrl", ['$scope', '$rootScope', '$compile', '$http', 'utility', 'NCIAAuditAppcertRes', function ($scope, $rootScope, $compile, $http, utility, NCIAAuditAppcertRes) {
    $scope.loadOrg = function () {
        $scope.OrgData = {};
        NCIAAuditAppcertRes.get({ org: 0 }, function (data) {
            $scope.OrgData = data.Data;
        });
    };
    $scope.init = function () {
        if (!$scope.sDate || !$scope.eDate) {
            utility.msgwarning("开始月份或结束月份不能为空");
            return;
        }
        if ($scope.sDate > $scope.eDate) {
            utility.msgwarning("开始月份不能大于结束月份");
            return;
        }
        $http({
            url: 'api/OrgStatistics/GetMonthFeeStatistics?nsId=' + ((typeof ($scope.nsId) == "undefined" || $scope.nsId == null) ? "" : $scope.nsId) + '&sDate=' + $scope.sDate + '&eDate=' + $scope.eDate,
            method: 'GET'
        }).then(function (res) {
            var x = _.map(res.data, 'yearMonth');
            initChart("amount", x, ["总金额", "护理险报销金额"], "月申报费用趋势图", "元", res.data, fn1);
            initChart("res", x, ["总人数"], "月申报人数趋势图", "人", res.data, fn2);
        })
    };
    $scope.loadOrg();
    $scope.sDate = moment().subtract(1, 'year').format("YYYY-MM");
    $scope.eDate = moment().format("YYYY-MM");
    $scope.init();
    var fn1 = function (option, sData) {
        option.series.push({
            name: '总金额',
            type: 'line',
            symbol: 'circle',
            symbolSize: 8,
            lineStyle: { normal: { color: '#418CF0' } },
            itemStyle: { normal: { color: '#418CF0' } },
            data: _.map(sData, 'tAmount')
        });
        option.series.push({
            name: '护理险报销金额',
            type: 'line',
            symbol: 'circle',
            symbolSize: 8,
            lineStyle: { normal: { color: '#FCB441' } },
            itemStyle: { normal: { color: '#FCB441' } },
            data: _.map(sData, 'tNciPay')
        });
    };
    var fn2 = function (option, sData) {
        option.series.push({
            name: '总人数',
            type: 'line',
            symbol: 'circle',
            symbolSize: 8,
            lineStyle: { normal: { color: '#418CF0' } },
            itemStyle: { normal: { color: '#418CF0' } },
            data: _.map(sData, 'tRes')
        });
    }
    function initChart(id, xData, lData, title, unit, sData, fn) {
        var myChart = echarts.init(document.getElementById(id));
        var option = {
            title: { text: title, x: 'center' },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: lData,
                x: 'right'
            },
            xAxis: [
                {
                    type: 'category',
                    data: xData
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    splitLine: {
                        show: false
                    },
                    axisLabel: {
                        formatter: '{value}' + unit
                    }
                }
            ],
            series: []
        };
        fn(option, sData);
        myChart.setOption(option);
    }
}]);