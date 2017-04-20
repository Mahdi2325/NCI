angular.module("sltcApp")
.controller('careStatisticCtrl', ['$scope', 'resourceBase', 'dc_memberStatisticRes', function ($scope, resourceBase, dc_memberStatisticRes) {

    var orgId = $scope.orgId;
    if (!angular.isDefined(orgId) || orgId == "") {
       // return;
    }

    var height = ($(document).height() - 80) / 2 - 40;
    $("#swAchievingRate").height(height);
    $("#swplanDistribution").height(height);
    $("#centerActiveTotal").height(height);
    $("#evaluationAverage").height(height);
    //1 社工计划达成率与分布
    dc_memberStatisticRes.get({ chartType: 1 }, function (data) {
        var planAchiveData = [];
        var planDistribution = [];
        $.each(data.Data.PlanAchievingRate, function (i, v) {
            planAchiveData.push({ name: v.Title, value: v.Percent });
        });
 
        PlanAchiveingRate(planAchiveData, 'swAchievingRate','社工');
        PlanDistribution(data.Data.PlanDistribution, 'swplanDistribution', '社工');
    });

    dc_memberStatisticRes.get({ chartType: 2 }, function (data) {
        var planAchiveData = [];
        var planDistribution = [];
        $.each(data.Data.PlanAchievingRate, function (i, v) {
            planAchiveData.push({ name: v.Title, value: v.Percent });
        });

        PlanAchiveingRate(planAchiveData, 'nurAchievingRate', '护理');
        PlanDistribution(data.Data.PlanDistribution, 'nurplanDistribution', '护理');
    });


    //    //服务中心活动人次
    //    (function () {
    //        // 基于准备好的dom，初始化echarts实例
    //        var myChart = echarts.init(document.getElementById('centerActiveTotal'));

    //        var option = {
    //            color: ['#F0AD4E'],
    //            title: {
    //                text: '服务中心活动人次',
    //                x: 'center'
    //                //subtext: '数据来自国家统计局'
    //            },
    //            tooltip: {},
    //            calculable: true,
    //            grid: {
    //                left: '3%',
    //                right: '4%',
    //                bottom: 15,
    //                containLabel: true
    //            },
    //            xAxis: {
    //                'type': 'category',
    //                'data': serviceData.x,
    //                splitLine: { show: false },
    //                axisLabel: {
    //                    interval: 0,
    //                    textStyle: {
    //                        color: '#000000'
    //                    }
    //                },
    //                axisLine: {
    //                    lineStyle: {
    //                        color: '#cccccc'
    //                    }
    //                },
    //                axisTick: {
    //                    show: false
    //                }
    //            },
    //            yAxis: {
    //                type: 'value',
    //                name: '人次',
    //                //show: false,
    //                axisLine: {
    //                    show: false
    //                },
    //                axisTick: {
    //                    show: false
    //                }
    //            },
    //            series: {
    //                name: '人数',
    //                type: 'bar',
    //                label: {
    //                    normal: {
    //                        show: true,
    //                        position: 'top'
    //                    }
    //                },
    //                barWidth: $scope.barWidth,
    //                data: serviceData.y
    //            }
    //        }

    //        myChart.setOption(option, true);

    //        setInterval(function () {
    //            myChart.setOption({
    //                series: {
    //                    data: []
    //                }
    //            });
    //            myChart.setOption({
    //                series: {
    //                    data: serviceData.y
    //                }
    //            });
    //        }, 4000);
    //    }());
    //});
 
}]);


//达成率
function PlanAchiveingRate(data,id,title) {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById(id));

    var option = {
        color: ["#53B2FB", "#7FEC4A", "#F0B629", "#252540", "#87CEEB", "#4682B4", "#708090", "#6A5ACD"],
        title: {
            text: title+'计划达成率',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        series: [
            {
                name: '比例',
                type: 'pie',
                radius: '60%',
                center: ['50%', '60%'],
                label: {
                    normal: {
                        formatter: '{b}: {d}%'
                    }
                },
                data: data,
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

    myChart.setOption(option, true);
};

function PlanDistribution(data,id,title) {

    var xData = [];
    var yData = [];

    $.each(data, function (i, v) {
        xData.push(v.Title);
        yData.push(v.Count);
    });

    (function () {
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById(id));

        var option = {
            color: ['#428BCA', '#5CB85C', '#F0AD4E'],
            title: {
                text: title+'计划数量分布图',
                x: 'center'
                //subtext: '数据来自国家统计局'
            },
            tooltip: {},
            calculable: true,
            grid: {
                left: '3%',
                right: '4%',
                bottom: 50,
                containLabel: true
            },
            xAxis: {
                'type': 'category',
                'axisLabel': { 'interval': 0 },
                'data': xData,
                splitLine: { show: false },
                axisLabel: {
                    interval: 0,
                    rotate: 0,
                    textStyle: {
                        color: '#000000'
                    }
                },
                axisLine: {
                    lineStyle: {
                        color: '#cccccc'
                    }
                },
                axisTick: {
                    show: false
                }
            },
            yAxis: {
                type: 'value',
                name: '人数',
                //show: false,
                decimal: false,
                axisLine: {
                    show: false
                },
                axisTick: {
                    show: false
                }
            },
            series: {
                name: '人数',
                type: 'bar',
                barWidth: 50,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: yData
            }
        }

        myChart.setOption(option, true);
    }());

};