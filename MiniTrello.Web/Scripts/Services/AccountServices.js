'use strict';

angular.module('app.services', []).factory('AccountServices', ['$http', function ($http) {

    var baseRemoteUrl = "http://minitrelloapis.apphb.com";
    var baseLocalUrl = "http://localhost:1416";
    var baseUrl = baseLocalUrl;

    var account = {};

    
    account.login = function (data) {
        return $http.post(baseUrl + '/login', data);
    };

    account.register = function (data) {
        return $http.post(baseUrl + '/register', data);
    };

    account.resetPassword = function(data) {
        return $http.put(baseUrl + '/resetPassword', data);
    };


    return account;

}]);