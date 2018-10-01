(function() {
    var app = angular.module('Transaction', ['angucomplete-alt', 'ng-currency']);

    app.config(['$qProvider', function ($qProvider) {
        $qProvider.errorOnUnhandledRejections(false);
    }]);

    app.controller('TransactionController', ['$scope', '$http', '$templateCache', function ($scope, $http, $templateCache) {
        $scope.amount = 0.01;
        $scope.min = 0.01;
        $scope.max = 0.01;

        $scope.Recipients = [];
        $scope.SelectedRecipient = null;
        $scope.SelectedRecipientId = null;
        $scope.SelectedRecipientName = null;
        $scope.InitialRecipient = null;

        $scope.data = null;
        $scope.status = null;
        $scope.headers = null
        $scope.config = null;
        $scope.statusText = null;

        $scope.SelectedRecipientFn = function (selected) {
            console.info('SelectedRecipientFn called')
            if (selected) {
                console.info('try to set SelectedRecipient: ', selected.originalObject);
                console.info('try to set SelectedRecipientId:', selected.originalObject.id);
                $scope.SelectedRecipient = selected.originalObject;
                $scope.SelectedRecipientId = selected.originalObject.id
                $scope.SelectedRecipientName = selected.originalObject.name;
                console.info('SelectedRecipient is: ', $scope.SelectedRecipient);
                console.info('SelectedRecipientId is:', $scope.SelectedRecipientId);
                console.info('SelectedRecipientName is:', $scope.SelectedRecipientName);
            } else {
                console.info('try to set SelectedRecipient: ', null);
                $scope.SelectedRecipient = null;
                $scope.SelectedRecipientId = null;
                $scope.SelectedRecipientName = null;
                console.info('SelectedRecipient is: ', $scope.SelectedRecipient);
                console.info('SelectedRecipientId is:', $scope.SelectedRecipientId);
                console.info('SelectedRecipientName is:', $scope.SelectedRecipientName);
            }
        }

        $scope.initRecipients = function (recipients) {
            console.info('initRecipients called', recipients)
            $scope.Recipients = recipients;
        }

        $scope.initSelectedRecipientId = function (selectedRecipientId) {
            console.info('initSelectedRecipientId called', selectedRecipientId)
            $scope.SelectedRecipientId = selectedRecipientId;
            console.info('SelectedRecipientId is:', $scope.SelectedRecipientId);
        }

        $scope.initTransactionAmount = function (transactionAmount) {
            console.info('initTransactionAmount called', transactionAmount)
            $scope.amount = transactionAmount;
        }

        $scope.initMaxTransaction = function (maxTransaction) {
            console.info('initMaxTransaction called', maxTransaction)
            $scope.max = maxTransaction;
        }

        $scope.initInitialRecipient = function (initialRecipient) {
            console.info('initialRecipient called', initialRecipient)
            $scope.InitialRecipient = initialRecipient;
            $scope.SelectedRecipientName = initialRecipient;
        }

        $scope.commitTransaction = function () {
            var req = {
                method: 'POST',
                url: '/Transaction/CommitTransaction',
                headers: {
                    'Content-Type': 'application/json'
                },
                data: {
                    amount: $scope.amount,
                    selectedRecipient: $scope.SelectedRecipientId
                }
            };

            $http(req).
                then(function (response) {
                    console.log('then invoked');
                    $scope.data = angular.fromJson(response.data);
                    $scope.status = response.status;
                    $scope.config = response.config;
                    $scope.statusText = response.statusText;
                    console.log('$scope.data: ', $scope.data);
                    console.log('$scope.data.success: ', $scope.data.success);
                    console.log('$scope.data.responseText: ', $scope.data.responseText);
                    console.log('$scope.status: ', $scope.status);
                    console.log('$scope.headers: ', $scope.headers);
                    console.log('$scope.config: ', $scope.config);
                    console.log('$scope.statusText: ', $scope.statusText);

                    if ($scope.data.success == false) {
                        console.warn('commitTransaction completed with error');
                        alert($scope.data.responseText);
                        console.warn($scope.data.responseText);
                    } else {
                        console.log('success');
                        var rediretTo = '/Transaction/TransactionComplete/' + $scope.data.responseText;
                        console.log('rediretTo: ', rediretTo);
                        window.location.href = rediretTo;
                    } 

                });
        }

/*
        $scope.commitTransaction = function () {
            console.info('commitTransaction called');
            $scope.method = 'JSONP';
            $scope.url = '/Transaction/CommitTransaction';
            $scope.code = null;
            $scope.response = null;

            console.info('amount: ', $scope.amount);
            console.info('SelectedRecipientId: ', $scope.SelectedRecipientId);
        
            {
                amount: $scope.amount,
                selectedRecipient: $scope.SelectedRecipientId
            };
            

            $http({ method: $scope.method, url: $scope.url, cache: $templateCache }).
                success(function (data, status, headers, config, statusText) {
                    $scope.status = status;
                    $scope.data = data;
                    $scope.statusText = statusText;

                    console.log('success');
                    console.log('scope.status: ', $scope.status);
                    console.log('scope.data: ', $scope.data);
                    console.log('scope.statusText: ', $scope.statusText);
                }).
                error(function (data, status, headers, config, statusText) {
                    $scope.data = data || "Request failed";
                    $scope.status = status;
                    $scope.statusText = statusText;
                }).
                then(function (response) {
                    $scope.data = response.data || "Request failed";
                    $scope.statusText = response.statusText;
                    $scope.status == response.status;
                    $scope.responseData = response;
                });
*/

/*
            $http.post('/Transaction/CommitTransaction', {
                amount: $scope.amount,
                selectedRecipient: $scope.SelectedRecipientId
            }).success(
                function (httpData) {
                    console.log('successResponse.success: ', httpData);

                    if (httpData.success === false) {
                        console.warn('commitTransaction completed with error');
                        alert(httpData.responseText);
                        console.warn(httpData.responseText);
                    } else {
                        console.log('success');
                        var rediretTo = '/Transaction/TransactionComplete/' + httpData.responseText;
                        console.log('rediretTo: ', rediretTo);
                        window.location.href = rediretTo;
                    } 
                }).error(
                function (errorResponse) {
                    // handle errors here
                });

        };
*/

        $scope.cantTransmit = function (errorMessage) {
            // handle errors here
            alert(errorMessage);
        };

    }]);
})();
