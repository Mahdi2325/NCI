angular.module("sltcApp")

 //住民在院状态信息
.controller("regInHosStatusDtlCtrl", ['$scope', '$http', '$stateParams', 'utility', 'webUploader', 'auditAgency', '$compile', function ($scope, $http, $stateParams, utility, webUploader, auditAgency, $compile) {
   
    $scope.currentItem = {};
    $scope.currentItem.Name = $scope.kmIncludeParams.name;
    $scope.currentItem.Gender = $scope.kmIncludeParams.gender;
    $scope.currentItem.Birthday = $scope.kmIncludeParams.birthday;
    $scope.currentItem.Age = $scope.kmIncludeParams.age;
    $scope.currentItem.IdNo = $scope.kmIncludeParams.idno;
    $scope.currentItem.SsNo = $scope.kmIncludeParams.ssno;
    $scope.currentItem.Phone = $scope.kmIncludeParams.phone;
    $scope.currentItem.McType = $scope.kmIncludeParams.mctype;
    $scope.currentItem.Disease = $scope.kmIncludeParams.disease;
    $scope.currentItem.MaritalStatus = $scope.kmIncludeParams.maritalstatus;
    $scope.currentItem.FamilyMemberName = $scope.kmIncludeParams.familymembername;
    $scope.currentItem.FamilyMemberRelationship = $scope.kmIncludeParams.familymemberrelationship;
    $scope.currentItem.FamilyMemberPhone = $scope.kmIncludeParams.familymemberphone;
    $scope.currentItem.InHosStatus = $scope.kmIncludeParams.inhosstatus;
    $scope.currentItem.NsId = $scope.kmIncludeParams.nsid;
    $scope.currentItem.NsAppCareType = $scope.kmIncludeParams.nsappcaretype;
    $scope.currentItem.IpdFlag = $scope.kmIncludeParams.ipdflag;
    $scope.currentItem.InDate = $scope.kmIncludeParams.indate;
    $scope.currentItem.OutDate = $scope.kmIncludeParams.outdate;
    $scope.currentItem.StartDate = $scope.kmIncludeParams.startdate;
    $scope.currentItem.ReturnDate = $scope.kmIncludeParams.returndate;
    $scope.currentItem.LeHour = $scope.kmIncludeParams.lehour;


    //$scope.Close = function () {
    //    location.href = "/NCIP/RegInHosStatusList";
    //    }

}])


