angular.module("sltcApp")
.controller("hlxDeskCtrl", ['$scope', '$rootScope', '$compile', '$http', 'utility', function ($scope, $rootScope, $compile, $http, utility) {
    $scope.data = {};
    var sDate = moment().subtract(1, 'year').format("YYYY-MM");
    var eDate = moment().format("YYYY-MM");
    $http({
        url: 'api/HlxDesk/GetHeadMsg',
        method: 'GET'
    }).then(function (res) {
        $scope.data.headMsg = res.data;
    })
    $http({
        url: 'api/HlxDesk/GetDeclareState',
        method: 'GET'
    }).then(function (res) {
        $scope.data.declareState = res.data;
    })
    $http({
        url: 'api/HlxDesk/GetRequireAppItem',
        method: 'GET'
    }).then(function (res) {
        $scope.data.requireAppItem = res.data;
    })
    $http({
        url: 'api/OrgStatistics/GetMonthFeeStatistics?nsId='+'&sDate=' +sDate + '&eDate=' + eDate,
        method: 'GET'
    }).then(function (res) {
        var x = _.map(res.data, 'yearMonth');
        initChart("mainfee", x, ["总金额", "护理险报销金额"], "月申报费用趋势图", "元", res.data, fn1);
    })
    $http({
        url: 'api/HlxDesk/GetAppcertStatistics',
        method: 'GET'
    }).then(function (res) {
        var appcertStatistics = res.data;
        _.forEach(appcertStatistics, function (item,index) {
            appcertStatistics[index].passRate =(parseInt(item.passResNum) * 100 / parseInt(item.applyResNum)).toFixed(2);
        });
        var myChart = echarts.init(document.getElementById('main'));
        var option = {
            tooltip: {
                trigger: 'axis'
            },
            toolbox: {
                show: true,
                feature: {
                    mark: { show: false },
                    dataView: { show: false, readOnly: false },
                    magicType: { show: false, type: ['line', 'bar'] },
                    restore: { show: false },
                    saveAsImage: { show: false }
                }
            },
            calculable: true,
            legend: {
                data: ['申请人数', '通过人数', '通过率']
            },
            xAxis: [
                {
                    type: 'category',
                    data: _.map(appcertStatistics, 'orgName')
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    splitLine: {
                        show: false
                    },
                    name: '人数',
                    axisLabel: {
                        formatter: '{value} 人'
                    }
                },
                {
                    type: 'value',
                    splitLine: {
                        show: false
                    },
                    name: '通过率',
                    axisLabel: {
                        formatter: '{value} %'
                    }
                }
            ],
            series: [

                {
                    name: '申请人数',
                    type: 'bar',
                    lineStyle: { normal: { color: '#418CF0' } },
                    itemStyle: { normal: { color: '#418CF0' } },
                    data: _.map(appcertStatistics, 'applyResNum')
                },
                {
                    name: '通过人数',
                    type: 'bar',
                    lineStyle: { normal: { color: '#FCB441' } },
                    itemStyle: { normal: { color: '#FCB441' } },
                    data: _.map(appcertStatistics, 'passResNum')
                },
                {
                    name: '通过率',
                    type: 'line',
                    yAxisIndex: 1,
                    lineStyle: { normal: { color: '#E0400A' } },
                    itemStyle: { normal: { color: '#E0400A' } },
                    data: _.map(appcertStatistics, 'passRate')
                }
            ]
        };
        myChart.setOption(option);
    })
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
    function initChart(id, xData, lData, title, unit, sData, fn) {
        var myChart = echarts.init(document.getElementById(id));
        var option = {
            //title: { text: title, x: 'center' },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: lData
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap:false,
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
}])
.controller("hlxSbaoDeskCtrl", ['$scope', '$rootScope', '$compile', '$http', 'utility', function ($scope, $rootScope, $compile, $http, utility) {
    $scope.data = {};
    var sDate = moment().subtract(1, 'year').format("YYYY-MM");
    var eDate = moment().format("YYYY-MM");
    $http({
        url: 'api/HlxDesk/GetHlxSbaoHeadMsg',
        method: 'GET'
    }).then(function (res) {
        $scope.data.headMsg = res.data;
    })
    $http({
        url: 'api/OrgStatistics/GetMonthFeeStatistics?nsId=' + utility.getUserInfo().OrgId + '&sDate=' + sDate + '&eDate=' + eDate,
        method: 'GET'
    }).then(function (res) {
        var x = _.map(res.data, 'yearMonth');
        initChart("mainfee", x, ["总金额", "护理险报销金额"], "月申报费用趋势图", "元", res.data, fn1);
    })
    $http({
        url: 'api/HlxDesk/GetHlxSbaoAppcertStatistics',
        method: 'GET'
    }).then(function (res) {
        var appcertStatistics = res.data;
        _.forEach(appcertStatistics, function (item, index) {
            appcertStatistics[index].passRate = (parseInt(item.passResNum) * 100 / parseInt(item.applyResNum)).toFixed(2);
        });
        var myChart = echarts.init(document.getElementById('main'));
        var option = {
            tooltip: {
                trigger: 'axis'
            },
            toolbox: {
                show: true,
                feature: {
                    mark: { show: false },
                    dataView: { show: false, readOnly: false },
                    magicType: { show: false, type: ['line', 'bar'] },
                    restore: { show: false },
                    saveAsImage: { show: false }
                }
            },
            calculable: true,
            legend: {
                data: ['申请人数', '通过人数', '通过率']
            },
            xAxis: [
                {
                    type: 'category',
                    data: _.map(appcertStatistics, 'year')
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    splitLine: {
                        show: false
                    },
                    name: '人数',
                    axisLabel: {
                        formatter: '{value} 人'
                    }
                },
                {
                    type: 'value',
                    splitLine: {
                        show: false
                    },
                    name: '通过率',
                    axisLabel: {
                        formatter: '{value} %'
                    }
                }
            ],
            series: [

                {
                    name: '申请人数',
                    type: 'bar',
                    lineStyle: { normal: { color: '#418CF0' } },
                    itemStyle: { normal: { color: '#418CF0' } },
                    data: _.map(appcertStatistics, 'applyResNum')
                },
                {
                    name: '通过人数',
                    type: 'bar',
                    lineStyle: { normal: { color: '#FCB441' } },
                    itemStyle: { normal: { color: '#FCB441' } },
                    data: _.map(appcertStatistics, 'passResNum')
                },
                {
                    name: '通过率',
                    type: 'line',
                    yAxisIndex: 1,
                    lineStyle: { normal: { color: '#E0400A' } },
                    itemStyle: { normal: { color: '#E0400A' } },
                    data: _.map(appcertStatistics, 'passRate')
                }
            ]
        };
        myChart.setOption(option);
    })
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
    function initChart(id, xData, lData, title, unit, sData, fn) {
        var myChart = echarts.init(document.getElementById(id));
        var option = {
            //title: { text: title, x: 'center' },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: lData
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
}])