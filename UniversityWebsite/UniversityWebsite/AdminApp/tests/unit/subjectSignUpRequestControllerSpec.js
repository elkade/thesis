/// <reference path="../../subjects/controllers/subjectSignUpRequestController.js" />

describe("subjectSignUpRequestCtrl", function() {
    var $controllerConstructor, scope, mockSubjectsService, httpBackend;

    beforeEach(module("configApp.subjects"));

    beforeEach(inject(function ($controller, $rootScope, $httpBackend) {
        $controllerConstructor = $controller;
        scope = $rootScope.$new();
        mockSubjectsService = sinon.stub({ getSignUpRequests: function () { } });
        httpBackend = $httpBackend;
    }));

    it('sth', function() {
        var mockSignUpRequests = {};
        mockSubjectsService.getSignUpRequests.calledWith(2);
        scope.subject = { Id: 2 };

        httpBackend.whenGET("http://localhost:1625/api/signup?subjectId=2").respond({
            data: {
                children: [
                  {
                      data: {
                          StudentFirstName: "Jan",
                          StudentLastName: "Kowalski"
                      }
                  }
                ]
            }
        });

        //$controllerConstructor('subjectSignUpRequestCtrl',
        //{ '$scope': scope, subjectsService: mockSubjectsService });

        //expect(scope.signUpRequests).toBe(mockSignUpRequests);
    });

});