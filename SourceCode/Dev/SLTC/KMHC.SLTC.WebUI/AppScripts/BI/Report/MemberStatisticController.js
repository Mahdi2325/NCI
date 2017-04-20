angular.module("sltcApp")
.controller('memberStatisticCtrl', ['$scope', 'resourceBase', 'dc_memberStatisticRes', function ($scope, resourceBase, dc_memberStatisticRes) {

    var orgId = $scope.orgId;
    if (!angular.isDefined(orgId) || orgId == "") {
       // return;
    }


    dc_memberStatisticRes.get({chartType:0}, function (data) {
        var ageData = [];
        var sexData = [];
        $.each(data.Data.AgeDistrib, function (i, v) {         
            ageData.push({ name: v.AgeRange, value: v.Percent });
        });
        $.each(data.Data.SexDistrib, function (i, v) {
            sexData.push({ name: v.Sex, value: v.Percent });
        });

        AgeDistribution(ageData);
        SexDistribution(sexData);
    })

    var option1 = {
        url: "/DC_Charts/GetDiseaseDistribution",
        type: 'Post',
        cache: false,
        dataType: 'json',
        data: {}, //发送服务器数据
        success: function (res) {  //成功事件
            var charts_data = res.data;
            DiseaseDistribution(charts_data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) { //发送失败事件
            alert(textStatus);
        }
    };
    $.ajax(option1);

    var height = ($(document).height() - 80) / 2 - 40;
    $("#age").height(height);
    $("#sex").height(height);
    $("#symptom").height(height);
    $("#area").height(height);

    //$.get(resourceBase + 'condition', { orgId: orgId }, function (data) {

    //    var conditionData = [];
    //    var symptomData = [];
    //    var healthData = [];
    //    var rotate = 0;
    //    $.each(data, function (i, v) {
    //        conditionData.push(v.name);
    //        symptomData.push(v.value);
    //        healthData.push(v.value1);
    //    });

    //    if (data.length > 7) {
    //        rotate = 45;
    //    }

    //    //常见病症数量
    //    (function () {
    //        // 基于准备好的dom，初始化echarts实例
    //        var myChart = echarts.init(document.getElementById('symptom'));

    //        var option = {
    //            color: ['#428BCA', '#5CB85C', '#F0AD4E'],
    //            title: {
    //                text: '常见症状分析',
    //                x: 'center'
    //                //subtext: '数据来自国家统计局'
    //            },
    //            tooltip: {},
    //            calculable: true,
    //            grid: {
    //                left: '3%',
    //                right: '4%',
    //                bottom: 50,
    //                containLabel: true
    //            },
    //            xAxis: {
    //                'type': 'category',
    //                'axisLabel': { 'interval': 0 },
    //                'data': conditionData,
    //                splitLine: { show: false },
    //                axisLabel: {
    //                    interval: 0,
    //                    rotate: rotate,
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
    //                name: '人数',
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
    //                barWidth:50,
    //                label: {
    //                    normal: {
    //                        show: true,
    //                        position: 'top'
    //                    }
    //                },
    //                data: symptomData
    //            }
    //        }

    //        myChart.setOption(option, true);
    //    }());
    //    //健康标签数量
    //    /*
    //    (function () {
    //        // 基于准备好的dom，初始化echarts实例
    //        var myChart = echarts.init(document.getElementById('health'));

    //        var option = {
    //            color: ['#428BCA', '#5CB85C', '#F0AD4E'],
    //            title: {
    //                text: '健康标签数量',
    //                x: 'center'
    //                //subtext: '数据来自国家统计局'
    //            },
    //            tooltip: {},
    //            calculable: true,
    //            grid: {
    //                left: '3%',
    //                right: '4%',
    //                bottom: 50,
    //                containLabel: true
    //            },
    //            xAxis: {
    //                'type': 'category',
    //                'axisLabel': { 'interval': 0 },
    //                'data': conditionData,
    //                splitLine: { show: false },
    //                axisLabel: {
    //                    interval: 0,
    //                    rotate: rotate,
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
    //                name: '人数',
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
    //                barWidth: 16,
    //                label: {
    //                    normal: {
    //                        show: true,
    //                        position: 'top'
    //                    }
    //                },
    //                data: healthData
    //            }
    //        }

    //        myChart.setOption(option, true);
    //    }());
    //    */
    //});
}]);



//年龄
function AgeDistribution(data) {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById('age'));

    var option = {
        color: ["#53B2FB", "#7FEC4A", "#F0B629", "#252540", "#87CEEB", "#4682B4", "#708090", "#6A5ACD"],
        title: {
            text: '会员年龄分布图',
            //subtext: '纯属虚构',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        series: [
            {
                name: '年龄',
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
//性别
function SexDistribution(data) {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById('sex'));

    var option = {
        color: ["#53B2FB", "#252540"],
        title: {
            text: '会员性别分布图',
            //subtext: '纯属虚构',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        series: [
            {
                name: '性别',
                selectedMode: 'single',
                type: 'pie',
                radius: [0, '60%'],
                center: ['50%', '60%'],
                label: {
                    normal: {
                        position: 'inner',
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

function DiseaseDistribution(data) {

    var cityData = [];
    var areaData = [];

    $.each(data, function (i, v) {
        cityData.push(v.DiseaseName);
        areaData.push(v.UUCount);
    });

    //区域
    (function () {
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('area'));

        var option = {
            color: ['#428BCA', '#5CB85C', '#F0AD4E'],
            title: {
                text: '会员患病分布图',
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
                'data': cityData,
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
                decimal:false,
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
                data: areaData
            }
        }

        myChart.setOption(option, true);
    }());

};