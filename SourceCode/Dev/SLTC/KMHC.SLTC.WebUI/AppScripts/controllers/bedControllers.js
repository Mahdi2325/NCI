/*
创建人: 张正泉
创建日期:2016-02-20
说明:床位管理
*/
angular.module("sltcApp")
    .controller("bedListCtrl", ['$scope', '$http', '$location', '$state', 'bedRes', 'utility', function($scope, $http, $location, $state, bedRes, utility) {
        $scope.init = function() {
            $scope.Data = {};
            
            //$scope.$watch("keyword", function (newValue) {
            //    if (newValue) {
            //        bedRes.query({}, function (data) {
            //            $scope.Data.beds = data;
            //        });
            //    }
            //});

            $scope.options = {
                //buttons: [],//需要打印按钮时设置
                ajaxObject: bedRes,//异步请求的res,
                params: { keyWords: "" },
                success: function (data) {//请求成功时执行函数
                    $scope.Data.beds = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
            }
        };

        $scope.delete = function (item) {
            if (confirm("确定删除该床位信息吗?")) {
                bedRes.delete({ id: item.BedNo }, function (data) {
                    //$scope.options.search();
                    $scope.Data.beds.splice($scope.Data.beds.indexOf(item), 1);
                    utility.message("删除成功");
                });
            }
        };


        $scope.search = $scope.reload = function () {
            bedRes.get($scope.options.pageInfo, function (req) {
                $scope.Data.beds = req.Data;
                $scope.options.sumInfo = { RecordsCount: req.RecordsCount, PagesCount: req.PagesCount };
                $scope.options.renderPage = $scope.options.pageInfo.CurrentPage;
            });
        };

        $scope.goResident = function (id) {
            $state.go('Person.BasicInfo', { id: id });
            $state.stateName = "ServiceResidentList";
        }

        $scope.init();
    }])
    .controller("bedEditCtrl", ['$scope', '$http', '$location', '$stateParams', 'bedRes', 'floorRes', 'roomRes', 'deptRes',
        function ($scope, $http, $location, $stateParams, bedRes, floorRes, roomRes, deptRes) {

        $scope.init = function () {
            $scope.Data = {};
            $scope.BedNo = "null";
            $scope.Data.bed = {};
            $scope.Data.bed.BedKind = "001";
            $scope.Data.bed.BedType = "001";
            $scope.Data.bed.Prestatus = "N";
            $scope.Data.bed.InsbedFlag = "N";
            $scope.Data.bed.SexType = "001";
        
            deptRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {
                $scope.Data.depts = data.Data;
                $scope.Data.bed.DeptNo = $scope.Data.depts[0].DeptNo
            });
            $scope.$watch('Data.bed.Floor', function (newVal, oldVal, scope) {
                if (newVal === oldVal) {
                       // 只会在监控器初始化阶段运行
                 } else {
                    roomRes.get({ CurrentPage: 1, PageSize: 100, floorName: newVal }, function (data) {
                        $scope.Data.rooms = data.Data;
                        //$scope.Data.bed.RoomNo = $scope.Data.rooms[0].RoomNo
                    });
                 }
             });
            floorRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {
                $scope.Data.floors = data.Data;
                $scope.Data.bed.Floor = $scope.Data.floors[0].FloorId
            });

            /*roomRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {
                $scope.Data.rooms = data.Data;
                //$scope.Data.bed.RoomNo = $scope.Data.rooms[0].RoomNo
            });*/

            if ($stateParams.id) {
                bedRes.get({ id: $stateParams.id }, function (data) {
                    $scope.Data.bed = data;
                });
                $scope.isAdd = false;
            } else {
                $scope.isAdd = true;

            }
            
        };

        $scope.submit = function () {
            $scope.Data.bed.bedStatus = "E"
            bedRes.save($scope.Data.bed, function (data) {
                $location.url("/angular/bedList");
            });
        };

        $scope.init();

    }]);
