angular.module("sltcApp")

 //人员状态列表
.controller("regInHosStatusListCtrl", ['$scope', '$compile', '$http', 'RegInHosStatusRes', 'NCIAAuditAppcertRes', 'utility', function ($scope, $compile, $http,RegInHosStatusRes, NCIAAuditAppcertRes, utility) {
    $scope.Data = {};
    $scope.Data.RegInHosStatusDtl = {};

    $scope.init = function () {
        $http({
            url: 'api/RegInHosStatusRes?name=&idno=&nsId=-1&status=-1&CurrentPage=1&PageSize=10',
            method: 'GET'
        }).success(function (data, header, config, status) {
            $scope.Data = data.Data;

        }).error(function (data, header, config, status) {
            //处理响应失败
            //alert("护理险平台无法连接，请联系管理员！")
            utility.msgwarning("长照服务平台无法连接，请联系管理员！");
            return;
        })
        $scope.loadOrg();
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: RegInHosStatusRes,//异步请求的res
            params: { name: "", idno: "", nsId: "-1", status: -1 },
            success: function (data) {//请求成功时执行函数
                $scope.Data = data.Data;
                var myChart = echarts.init(document.getElementById('main'));
                var option = {
                    title: {
                        text: '人员状态',
                        x: 'center'
                    },
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b} : {c} ({d}%)"
                    },
                    legend: {
                        orient: 'vertical',
                        left: 'left',
                        data: ['在院', '不在院']
                    },
                    series: [
                        {
                            name: '人员状态',
                            type: 'pie',
                            radius: '55%',
                            center: ['50%', '60%'],
                            data: [
                                { value: $scope.Data.InHosCount, name: '在院' },
                                { value: $scope.Data.OutHosCount, name: '不在院' }
                            ],
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
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    };

    



    $scope.View = function (item) {
        var params = {
            name: item.Name, gender: item.Gender, birthday: item.Birthday, age: item.Age, idno: item.IdNo, ssno: item.SsNo, phone: item.Phone,
            mctype: item.McType, disease: item.Disease, maritalstatus: item.MaritalStatus, familymembername: item.FamilyMemberName, familymemberrelationship: item.FamilyMemberRelationship,
            familymemberphone: item.FamilyMemberPhone, inhosstatus: item.InHosStatus, nsid: item.NsId, nsappcaretype: item.NsAppCareType, ipdflag: item.IpdFlag, indate: item.InDate,
            outdate: item.OutDate, startdate: item.StartDate, returndate: item.ReturnDate, lehour: item.LeHour
        };
        var html = '<div km-include km-template="Views/NCIP/RegInHosStatusDtl.html" km-controller="regInHosStatusDtlCtrl"  km-include-params=\'' + JSON.stringify(params) + '\'}" ></div>';
        $scope.dialog = BootstrapDialog.show({
            title: '<label class=" control-label">人员状态信息</label>',
            cssClass: 'pop-dialogInHos',
            type: BootstrapDialog.TYPE_DEFAULT,
            message: html,
            size: BootstrapDialog.SIZE_WIDE,
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            }
        });

    };
    $scope.Search = function () {
        if (!angular.isDefined($scope.status) || $scope.status == "") {
            $scope.options.params.status = "-1";
        }
        else {
            $scope.options.params.status = $scope.status;
        }
        if (!angular.isDefined($scope.nsId) || $scope.nsId == "" || $scope.nsId == null) {
            $scope.options.params.nsId = "-1";
        }
        else {
            $scope.options.params.nsId = $scope.nsId;
        }
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    };

    $scope.exportRecort = function () {
        if (!angular.isDefined($scope.status) || $scope.status == "") {
            $scope.options.params.status = "-1";
        }
        else {
            $scope.options.params.status = $scope.status;
        }
        if (!angular.isDefined($scope.nsId) || $scope.nsId == "" || $scope.nsId == null) {
            $scope.options.params.nsId = "-1";
        }
        else {
            $scope.options.params.nsId = $scope.nsId;
        }


        window.open("/Report/ExportRegInHosStatus?templateName={0}&name={1}&idno={2}&status={3}&nsId={4}&fileTypeStr=xls".format("ResidentStatusReport", $scope.options.params.name, $scope.options.params.idno, $scope.options.params.status, $scope.options.params.nsId), "_blank");
    };
    $scope.loadOrg = function () {
        $scope.OrgData = {};
        NCIAAuditAppcertRes.get({ org: 0 }, function (data) {
            $scope.OrgData = data.Data;
        });
    };

    $scope.init();

}])


