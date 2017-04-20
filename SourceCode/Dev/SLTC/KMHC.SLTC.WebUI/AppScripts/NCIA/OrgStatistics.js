angular.module("sltcApp")
.controller("ComStatisticsCtrl", ['$scope', '$rootScope', '$compile', '$http', 'utility', function ($scope, $rootScope, $compile, $http, utility) {

    $scope.init = function () {
        $http({
            url: 'api/OrgStatistics/GetAppcertRate?starttime=' + ((typeof ($scope.starttime)) == "undefined" ? "" : $scope.starttime) + '&endtime=' + ((typeof ($scope.endtime)) == "undefined" ? "" : $scope.endtime),
            method: 'GET'
        }).then(function (res) {
            var lValue = _.map(res.data[0].DATA, 'YEAR')
            var yValue = _.map(res.data, 'NSNAME')
            initStackChart("certRate", "护理险资格申报年通过率", "百分比", lValue, yValue, res.data)
        })
        $http({
            url: 'api/OrgStatistics/GetApphospRate?starttime=' + ((typeof ($scope.starttime)) == "undefined" ? "" : $scope.starttime) + '&endtime=' + ((typeof ($scope.endtime)) == "undefined" ? "" : $scope.endtime),
            method: 'GET'
        }).then(function (res) {
            var lValue = _.map(res.data[0].DATA, 'YEAR')
            var yValue = _.map(res.data, 'NSNAME')
            initStackChart("hospRate", "护理险申请住院年通过率", "百分比", lValue, yValue, res.data)
        })
    };

    $scope.init();
    function initStackChart(id, title, name, lValue, yValue, data) {
        var myChart = echarts.init(document.getElementById(id));
        var option = {
            title: {
                text: title,
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: function (params) {
                    var res = params.name + '<br/>';
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].NSNAME == params.name) {
                            for (var j = 0; j < data[i].DATA.length; j++) {
                                if (data[i].DATA[j].YEAR == params.seriesName) {
                                    res += '年份 : ' + data[i].DATA[j].YEAR + '</br>' +
                                        '通过率 : ' + data[i].DATA[j].PASSRATE + '%</br>' +
                               '总人数 : ' + data[i].DATA[j].TOTALPEOPLE + '</br>' +
                               '通过人数 : ' + data[i].DATA[j].PASSPEOPLE + '</br>';
                                    return res;
                                }
                            }

                        }
                    }
                    
                }
            },
            toolbox: {
                feature: {
                    dataView: { show: true, readOnly: false },
                    magicType: { show: true, type: ['line', 'bar'] },
                    restore: { show: true },
                    saveAsImage: { show: true }
                }
            },
            legend: {
                data: lValue,
                top: 40
            },
            grid: {
                left: '3%',
                right: '4%',
                bottom: '3%',
                containLabel: true
            },
            xAxis: {
                type: 'value',
                axisLabel: {
                    show: true,
                    interval: 'auto',
                    formatter: '{value} %'
                },
                show: true
            },
            yAxis: {
                type: 'category',
                data: yValue,
                nameGap: 150
            },
            series: []
        };
        for (var i = 0; i < lValue.length; i++) {
            var values = [];
            for (var j = 0; j < data.length; j++) {
                values.push(_.find(data[j].DATA, { YEAR: lValue[i].toString() }).PASSRATE);
            }
            var item = {
                name: lValue[i],
                type: 'bar',
                stack: '通过率',
                label: {
                    normal: {
                        show: true,
                        position: 'top',
                        formatter: '{c} %'
                    }
                },
                data: values
            };
            option.series.push(item);
        }
        myChart.setOption(option);
    }
}])
.controller("OrgStatisticsCtrl", ['$scope', '$rootScope', '$compile', '$http', 'utility', 'NCIAAuditAppcertRes', function ($scope, $rootScope, $compile, $http, utility, NCIAAuditAppcertRes) {
    $scope.loadOrg = function () {
        $scope.OrgData = {};
        NCIAAuditAppcertRes.get({ org: 0 }, function (data) {
            $scope.OrgData = data.Data;
        });
    };
    $scope.init = function () {
        $http({
            url: 'api/OrgStatistics/GetcertRate?NSID=' + $scope.nsId,
            method: 'GET'
        }).then(function (res) {
            var xValue = _.map(res.data, 'YEAR')
            var yValue = _.map(res.data, 'PASSRATE')
            var orgName = _.find($scope.OrgData, { NsId: $scope.nsId }).NsName;
            initBarLineChart("certRate", orgName + "护理险资格申请年通过率", "百分比", xValue, yValue, res.data, "piecertRate", "护理险资格申请各状态人数占比", "GetAppcert")
        })
        $http({
            url: 'api/OrgStatistics/GethospRate?NSID=' + $scope.nsId,
            method: 'GET'
        }).then(function (res) {
            var xValue = _.map(res.data, 'YEAR')
            var yValue = _.map(res.data, 'PASSRATE')
            var orgName = _.find($scope.OrgData, { NsId: $scope.nsId }).NsName;
            initBarLineChart("hospRate", orgName + "护理险申请住院年通过率", "百分比", xValue, yValue, res.data, "piehospRate", "护理险申请住院各状态人数占比", "GetApphosp")
        })
    };

    $scope.loadOrg();
    function initPieChart(id, title, name, xValue, yValue) {
        var myChart = echarts.init(document.getElementById(id));
        var option = {
            title: {
                text: title,
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data: xValue,
                top: 30
            },
            series: [
                {
                    name: name,
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data: yValue,
                    itemStyle: {
                        emphasis: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    }
                }
            ]
        };
        myChart.setOption(option);
    }
    function initBarLineChart(id, title, name, xValue, yValue, data,pieId,pieTitle,pieMethod) {
        var myChart = echarts.init(document.getElementById(id));
        var option = {
            title: {
                text: title,
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: function (params) {
                    var res = params.name + '<br/>';
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].YEAR == params.name) {
                            res += '通过率 : ' + data[i].PASSRATE + '%</br>' +
                                '总人数 : ' + data[i].TOTALPEOPLE + '</br>' +
                                '通过人数 : ' + data[i].PASSPEOPLE + '</br>'
                            + '平均通过率 : ' + data[i].PASSRATEAVG + '%</br>'
                            + '所有机构总人数 : ' + data[i].TOTALPEOPLEAVG + '</br>'
                            + '所有机构通过人数 : ' + data[i].PASSPEOPLEAVG + '</br>';
                            return res;
                        }
                    }
                }
            },
            //toolbox: {
            //    feature: {
            //        dataView: { show: true, readOnly: false },
            //        magicType: { show: true, type: ['line', 'bar'] },
            //        restore: { show: true },
            //        saveAsImage: { show: true }
            //    }
            //},
            legend: {
                data: ['通过率', '平均通过率'],
                top: 30
            },
            xAxis: [
                {
                    type: 'category',
                    data: xValue
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    name: '通过率',
                    min: 0,
                    max: 100,
                    interval: 10,
                    axisLabel: {
                        formatter: '{value} %'
                    }
                },
                {
                    type: 'value',
                    name: '平均通过率',
                    min: 0,
                    max: 100,
                    interval: 10,
                    axisLabel: {
                        formatter: '{value} %'
                    }
                }
            ],
            series: []
        };
        option.series.push({
            name: '通过率',
            type: 'bar',
            data: _.map(data, 'PASSRATE')
        });
        option.series.push({
            name: '平均通过率',
            type: 'line',
            data: _.map(data, 'PASSRATEAVG')
        });
        myChart.setOption(option);
        myChart.on('click', function (param) {
            $http({
                url: 'api/OrgStatistics/'+pieMethod+'?NSID=' + $scope.nsId + '&YEAR='+param.name,
                method: 'GET'
            }).then(function (res) {
                var xValue = _.map(res.data, 'name')
                initPieChart(pieId, param.name + pieTitle, "申请状态", xValue, res.data)
            })
        });
    }

}])
.controller("caseManagementCtrl", ['$scope', '$rootScope', '$compile', '$http', 'utility','NCIAAuditAppcertRes','cfpLoadingBar','CaseMgrFeeRes','CaseMgrCplRec','CaseMgrNsRecRes','CaseMgrMeasureRecRes','CaseMgrEvlRecRes', function ($scope, $rootScope, $compile, $http, utility,NCIAAuditAppcertRes,cfpLoadingBar,CaseMgrFeeRes,CaseMgrCplRec,CaseMgrNsRecRes,CaseMgrMeasureRecRes,CaseMgrEvlRecRes) {
    Handlebars.registerHelper('getTime', function(time) {
        return moment(time).format('h:mm:ss');
    });
    Handlebars.registerHelper('getFullDate', function(time) {
        return moment(time).format('YYYY-MM-DD');
    });
    Handlebars.registerHelper('getIndex', function(idx) {
        return (idx+1)+".";
    });
    $scope.currentType = $('#case-tab li.active').attr('data-type');
    $('#case-tab li').click(function () {
        var _this= $(this),type=_this.attr('data-type');
        $scope.currentType=_this.attr('data-type');
        _this.addClass('active').siblings().removeClass('active');
        $scope.getCaseCon($scope.feeNo,$scope.currentType);

    }).css('cursor','pointer');

    $scope.Data = {};
    $scope.feeNo = -1;
    $scope.IpdFlag = "I";
    $scope.OrgData = {};
    $scope.eDate = moment().format('YYYY-MM-DD');
    $scope.sDate = moment().subtract(6, "d").format('YYYY-MM-DD');

    $scope.init=function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: CaseMgrFeeRes,//异步请求的res
            params: { st: $scope.sDate, et: $scope.eDate, feeNo: $scope.feeNo},
            success: function (data) {//请求成功时执行函数
                if($scope.feeNo==-1){
                    utility.msgwarning("请选择住民~");
                    return;
                }
                if (!data.RecordsCount) {
                    utility.showNoData('#showDatas');
                    return;
                }
                $scope.Data = data;
                var code = $('#'+$scope.currentType).html();
                var template = Handlebars.compile(code);
                var dom = template($scope.Data);
                $("#showDatas").html(dom);
                cfpLoadingBar.complete();

            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }

        $scope.loadOrg();

    }
    $scope.changeOrg = function (institutionName) {
        $scope.loadResident(institutionName);
    }
    $scope.loadOrg = function () {
        NCIAAuditAppcertRes.get({ org: 0 }, function (data) {
            if (data.Data && data.Data.length) {
                $scope.OrgData = data.Data;
                $scope.institutionName = data.Data[0].NsId;
                $scope.loadResident($scope.institutionName);
            }
        });
    };
    $scope.loadResident = function (institutionName) {
        $http({
            url:"/api/CaseMgr/GetResidents",
            params:{'nsId':institutionName}
        }).success(function (data) {
            $scope.residents = JSON.parse(data);
        })
    }
    $scope.afterSelected = function (item) {
        $scope.currentResident = item;//设置ResidentCard的currentResident
        $scope.feeNo =item.FeeNo;
        $scope.getCaseCon($scope.feeNo,$scope.currentType)
    }
    $scope.getCaseCon = function (feeNo,type) {
        cfpLoadingBar.start();

        var ajaxObj;
        switch (type){
            case "GetFee":
                ajaxObj = CaseMgrFeeRes;
                break;
            case "GetCplRec":
                ajaxObj = CaseMgrCplRec;
                break;
            case "GetNsRec":
                ajaxObj = CaseMgrNsRecRes;
                break;
            case "GetMeasureRec":
                ajaxObj = CaseMgrMeasureRecRes;
                break;
            case "GetEvlRec":
                ajaxObj = CaseMgrEvlRecRes;
                break;
            default:
                ajaxObj = null;
        }
        $scope.options.params.feeNo=feeNo;
        $scope.options.params.st = $scope.sDate;
        $scope.options.params.et = $scope.eDate;
        $scope.options.ajaxObject=ajaxObj;
        $scope.options.pageInfo.CurrentPage=1;
        $scope.options.search();
    }
    $scope.init();
}])