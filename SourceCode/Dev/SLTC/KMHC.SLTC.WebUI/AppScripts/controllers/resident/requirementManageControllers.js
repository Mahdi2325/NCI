/*
创建人:张凯
创建日期:2016-02-25
说明: 需求管理
*/
angular.module("sltcApp")
.controller("requirementManageCtrl", ['$rootScope', '$scope', '$state', 'dictionary', 'utility', 'requirementRes', function ($rootScope, $scope, $state, dictionary, utility, requirementRes) {
    //var id = $state.params.id;
    $scope.Data = {};
    $scope.currentItem = {};
    // 当前住民
    $scope.currentResident = {}
    $scope.buttonShow = false;

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        $scope.listItem($scope.currentResident.FeeNo);//加载当前住民的需求管理记录
        $scope.currentItem = {}//清空编辑项
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }
    //加载需求管理
    $scope.listItem = function (residentId) {
        $scope.Data.ReqList = {};
        requirementRes.get({ currentPage: 1, pageSize: 10, feeNo: residentId }, function (obj) {
            $scope.Data.ReqList = obj.Data;
        });
    }
    $scope.deleteItem = function (item) {
        if (confirm("确定删除该信息吗?")) {
            requirementRes.delete({ id: item.Id }, function (data) {
                $scope.Data.ReqList.splice($scope.Data.ReqList.indexOf(item), 1);
                utility.message("删除成功");
            });
        }
    };

    $scope.createItem = function (item) {
        //新增需求管理记录，得到住民ID
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;
        $scope.currentItem.RegName = $scope.currentResident.Name;
        $scope.currentItem.OrgId = $scope.currentResident.OrgId;
        requirementRes.save($scope.currentItem, function (data) {           
            $scope.Data.ReqList.push(data.Data);
        });
        $scope.currentItem = {};
    };

    $scope.updateItem = function (item) {
        requirementRes.save($scope.currentItem, function (data) {
            $scope.currentItem = {};
        });
        $scope.currentItem = {};
    };


    $scope.rowSelect = function (item) {
        $scope.currentItem = item;

    };

    $scope.saveEdit = function (item) {
        if (angular.isDefined(item.Id)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        utility.message($scope.currentResident.Name + "的需求信息保存成功！");
    };

    $scope.staffByInSelected = function (item) {
        $scope.currentItem.ExecuteBy = item.EmpNo;
        $scope.currentItem.ExecutorName = item.EmpName;
    }
}]);