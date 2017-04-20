angular.module("sltcApp")
 //护理险资格年审
.controller("AuditYearCertCtrl", ['$scope', 'NCIAAuditAppcertRes', 'utility', 'AuditYearCertRes', function ($scope, NCIAAuditAppcertRes, utility, AuditYearCertRes) {
    $scope.Data = {};
    $scope.currentItem = {};
    $scope.init = function () {
        $scope.loadOrg();
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: AuditYearCertRes,//异步请求的res
            params: { name: "", idno: "", nsId: "-1", status: "" },
            success: function (data) {//请求成功时执行函数
                $scope.Data.AuditYearCertLsit = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }
    };


    $scope.loadOrg = function () {
        $scope.OrgData = {};
        NCIAAuditAppcertRes.get({ org: 0 }, function (data) {
            $scope.OrgData = data.Data;
        });
    };

    $scope.Search = function ()
    {
        if ($scope.options.params.nsId == null)
        {
            $scope.options.params.nsId = "-1";
        }

        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.search();
    }

    $scope.updateYearCert = function (item)
    {
        $scope.currentItem = {};
        $scope.currentItem = item;
        $("#modalYearCert").modal("toggle");
    }

    $scope.save = function (yearinfo)
    {
        if (yearinfo.Reason == null)
        {  
            utility.message("请填写启用/停用原因！");
            return;
        }

        AuditYearCertRes.save(yearinfo, function (data) {

            if (JSON.stringify(data[0]) == '"0"')
            {
                var mess = yearinfo.Name + "的长期护理保险待遇资格已";
                mess += yearinfo.HospStatus == 9 ? "启用" : "停用";
                utility.message(mess);
            } else if (JSON.stringify(data[0]) == '"1"')
            {
                utility.msgwarning("长期护理保险待遇资格同步失败，请联系管理员！");
            }

            $scope.Search();
            $("#modalYearCert").modal("hide");
        });
    }

    $scope.init();

}])