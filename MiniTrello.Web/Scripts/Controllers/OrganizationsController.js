'use strict';
// Google Analytics Collection APIs Reference:
// https://developers.google.com/analytics/devguides/collection/analyticsjs/

angular.module('app.controllers')



    // Path: /login
    .controller('OrganizationsController', ['$scope', '$location', '$window', 'BoardServices', 'OrganizationServices', '$stateParams', function ($scope, $location, $window, boardServices, organizationServices, $stateParams) {
    $scope.boardDetailId = $stateParams.boardId;

    $scope.organizations = [];
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


    


        // TODO: Authorize a user
        $scope.getOrganizationsForLoggedUser = function () {

            organizationServices
                .getOrganizationsForLoggedUser()
              .success(function (data, status, headers, config) {
                  $scope.organizations = data;
              })
              .error(function (data, status, headers, config) {

              });
            //$location.path('/');
        };

        if ($scope.boardDetailId > 0) {
            //get board details
        }
        else {
            $scope.getBoardsForLoggedUser();
        }

        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
        });
    }]);