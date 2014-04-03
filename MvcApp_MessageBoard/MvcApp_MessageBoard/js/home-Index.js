﻿
var module = angular.module("homeIndex", ['ngRoute']);

module.config(function ($routeProvider) {
    $routeProvider.when("/", { controller:"topicsController",templateUrl:"/templates/topicsView.html"});
    $routeProvider.when("/newmessage", { controller: "newTopicController", templateUrl: "/templates/newTopicView.html" });
    $routeProvider.when("/message/:id", { controller: "singleTopicController", templateUrl: "/templates/singleTopicView.html" });
    $routeProvider.otherwise({redirectTo:"/"});
});

module.factory("dataService", function ($http,$q) {
    var _topics = [];
    var _isInit = false;

    var _isReady = function () {
        return _isInit;
    }
    var _getTopics = function () {

        var deferred = $q.defer();
        $http.get("/api/topics?includeReplies=true")
        .then(function (result) {
            console.log('Success');
            angular.copy(result.data, _topics);
            _isInit = true;
            deferred.resolve();
            console.log(result.data);
        },
        function () {
            deferred.reject();
        });

        return deferred.promise;
    };

    var _addTopic = function (newtopic) {
        var deferred = $q.defer();

        $http.post("/api/topics", newtopic)
            .then(function (result) {
                //Success
                var newlyCreatedTopic = result.data;
                _topics.splice(0, 0, newlyCreatedTopic);
                deferred.resolve(newlyCreatedTopic);
            },
            function () {
                //Failure
                deferred.reject();
            });

        return deferred.promise;
    };

    function _findTopic(id) {
        var found = null;

        $.each(_topics, function (i, item) {
            if (item.id == id) {
                found = item;
                return false;
            }
        });
        return found;
    }


    var _getTopicById = function (id) {
        var deferred = $q.defer();

        if (_isReady()) {
            var topic = _findTopic(id);
            if (topic) {
                deferred.resolve(topic);
            } else {
                deferred.reject();
            }
        } else {
            _getTopics()
             .then(function () {
                 //success
                 var topic = _findTopic(id);
                 if (topic) {
                     deferred.resolve(topic);
                 } else {
                     deferred.reject();
                 }
             },
             function () {
                 //error
                 deferred.reject();
             });
        }


        return deferred.promise;
    }


    var _saveReply = function (topic,newReply) {
        var deferred = $q.defer();

        $http.post("/api/topics/" + topic.id + "/replies", newReply)
          .then(function (result) {
              //success
              if (topic.replies == null) topic.replies = [];
              topic.replies.push(result.data);
              deferred.resolve(result.data);
          },
          function () {
              //error
              deferred.reject();
          });

        return deferred.promise;
    };

    return {
        topics: _topics,
        getTopics: _getTopics,
        addTopic: _addTopic,
        isReady: _isReady,
        getTopicById: _getTopicById,
        saveReply: _saveReply
    };
});

function topicsController($scope, $http, dataService) {
    
    $scope.data =dataService;
    $scope.isBusy = false;

    if (dataService.isReady() == false) {
        $scope.isBusy = true;
        dataService.getTopics()
            .then(function () {
                //Success
            },
            function () {
                //Error
                alert('Could Not Load Topics');
            })
           .then(function () {
               $scope.isBusy = false;
           });
    }
}

function newTopicController($scope, $http, $window, dataService) {
    $scope.newTopic = {};

    $scope.save = function () {        
        dataService.addTopic($scope.newTopic)
          .then(function () {
              $window.location = "#/";
          },
          function () {
              alert("COuld Not save the new topic");
          });


    };
}

function singleTopicController($scope, dataService, $window, $routeParams) {
    $scope.topic = null;
    $scope.newReply = {};

    dataService.getTopicById($routeParams.id)
      .then(function (topic) {
          //success
          $scope.topic = topic;
      }, function () {
          //error
          $window.location = "#/";
      });

    $scope.addReply = function () {
        dataService.saveReply($scope.topic, $scope.newReply)
          .then(function () {
              //Success
              $scope.newReply.body = "";
          },
          function () {
              //error
              alert('Could Not Save New Reply');
          });
    };
}