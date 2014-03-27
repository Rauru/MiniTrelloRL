'use strict';

// Google Analytics Collection APIs Reference:
// https://developers.google.com/analytics/devguides/collection/analyticsjs/

angular.module('app.controllers', [])

    // Path: /
    .controller('HomeCtrl', ['$scope', '$location', '$window', function ($scope, $location, $window) {
        $scope.$root.title = 'AngularJS SPA Template for Visual Studio';
        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
        });
    }])

    // Path: /about
    .controller('AboutCtrl', ['$scope', '$location', '$window', function ($scope, $location, $window) {
        $scope.$root.title = 'AngularJS SPA | About';
        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
        });
    }])

    // Path: /login
    .controller('LoginCtrl', ['$scope', '$location', '$window', function ($scope, $location, $window) {
        $scope.$root.title = 'AngularJS SPA | Sign In';
        // TODO: Authorize a user
        $scope.submit = function () {
            $http
              .post('/authenticate', $scope.user)
              .success(function (data, status, headers, config) {
                  $window.sessionStorage.token = data.token;
                  $scope.message = 'Welcome';
              })
              .error(function (data, status, headers, config) {
                  // Erase the token if the user fails to log in
                  delete $window.sessionStorage.token;

                  // Handle login errors here
                  $scope.message = 'Error: Invalid user or password';
              });
        };
        $scope.login = function () {
            $location.path('http://minitrelloapis.apphb.com/login/login');
            $window.sessionStorage.token = data.token;
            $scope.message = 'Welcome';
            return false;
        };
        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
        });
    }])


    // Path: /Register
    .controller('RegisterCtrl', ['$scope', '$location', '$window', '$http', function ($scope, $location, $window, $http) {
        $scope.$root.title = ' SPA | Register';
        $scope.register = function () {
            var account = {
                FirstName: $scope.FirstName,
                LastName: $scope.LastName,
                Email: $scope.Email,
                Password: $scope.Password,
                ConfirmPassword: $scope.ConfirmPassword
            };
            //AccountServices.register(account);
            //$location.path('/register');
            return $http.post('http://localhost:1416/register', account);
        };
        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
        });
    }])

    // Path: /error/404
    .controller('Error404Ctrl', ['$scope', '$location', '$window', function ($scope, $location, $window) {
        $scope.$root.title = 'Error 404: Page Not Found';
        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
        });
    }]);