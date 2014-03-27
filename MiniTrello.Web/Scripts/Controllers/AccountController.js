'use strict';

// Google Analytics Collection APIs Reference:
// https://developers.google.com/analytics/devguides/collection/analyticsjs/

angular.module('app.controllers')



    // Path: /login
    .controller('AccountController', ['$scope', '$location', '$window', 'AccountServices', function ($scope, $location, $window, AccountServices) {

        $scope.getOrganizationsForLoggedUser = function () {

            AccountServices
                .getOrganizationsForLoggedUser()
              .success(function (data, status, headers, config) {
                  $scope.organizations = data;
              })
              .error(function (data, status, headers, config) {

              });
            //$location.path('/');
        };

        $scope.$root.title = 'AngularJS SPA | Sign In';

        $scope.loginModel = { Email: '', Password: '' };

        $scope.registerModel = { Email: '', Password: '', FirstName: '', LastName: '', ConfirmPassword: '' };

        $scope.isLogged = function() {
            return $window.sessionStorage.token != null;
        };

        // TODO: Authorize a user
        $scope.login = function () {
            var model = {
                Email: $scope.Email, Password: $scope.Password
            };
            AccountServices
                .login(model)
              .success(function (data, status, headers, config) {
                  
                  $window.sessionStorage.token = data.Token;
                  $location.path('/organizations');
              })
              .error(function (data, status, headers, config) {
                  // Erase the token if the user fails to log in
                  delete $window.sessionStorage.token;

                  // Handle login errors here
                  $scope.message = 'Error: Invalid user or password';
              });
        };
        
        $scope.register = function () {
            var model = {
                FirstName: $scope.FirstName, LastName: $scope.LastName,
                Email: $scope.Email, Password: $scope.Password, ConfirmPassword: $scope.ConfirmPassword
            };
            AccountServices.register(model).success(function (data, status, headers, config) {
                console.log(data);
                $location.path('/login');
            })
            .error(function (data, status, headers, config) {
                console.log(data);
            });
            
            return false;
        };
        $scope.resetPassword = function () {
            var model = {
                Email: $scope.Email
            };
            AccountServices.resetPassword(model).success(function (data, status, headers, config) {
                console.log(data);
                $location.path('/login');
            })
            .error(function (data, status, headers, config) {
                console.log(data);
            });

            return false;
        };
        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
        });
    }]);