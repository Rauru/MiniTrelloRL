'use strict';
// Google Analytics Collection APIs Reference:
// https://developers.google.com/analytics/devguides/collection/analyticsjs/

angular.module('app.controllers')



    // Path: /login
    .controller('BoardController', ['$scope', '$location', '$window', 'BoardServices','OrganizationServices', function ($scope, $location, $window, boardServices,organizationServices) {
    
       
        $scope.organizations =
        /*[
            {
                Title: 'My Boards',
                boards: [
                    { Title: 'Myboard1', Administrator: 'Siwady' },
                    { Title: 'Myboard2', Administrator: 'Edward' }
                ]
            },
            {
                Title: 'Unitec', 
                boards: [
                    { Title: 'Myboard3', Administrator: 'Edward3' },
                    { Title: 'Myboard4', Administrator: 'Edward4' },
                    { Title: 'Myboard5', Administrator: 'Edward5' }
                ]
            }
        ];*/


    $scope.getOrganizationsForLoggedUser = function() {
        organizationServices.getOrganizationsForLoggedUser().success(function(data, status, headers, config) {
            $scope.organizations = data;
        }).error(function(data, status, headers, config) {

        });
    };

       
    // TODO: Authorize a user
            $scope.getBoardsForLoggedUser = function () {
        
                    boardServices
                        .getBoardsForLoggedUser()
                      .success(function (data, status, headers, config) {
                                //$scope.boards = data;
                            })
                      .error(function (data, status, headers, config) {
            
                          });
                    //$location.path('/');
                };
    
           
    
            $scope.$on('$viewContentLoaded', function () {
                    $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
                });
        }]);