angular.module('configApp.subjects')

.controller('subjectsEditCtrl', function ($scope, $stateParams, utils, subjectsService) {
    $scope.subject = $stateParams.subject;
    $scope.alerts = [];
    $scope.studentsSection = { name: 'students', url: 'adminapp/views/subjects/section.students.html' };
    $scope.teachersSection = { name: 'teachers', url: 'adminapp/views/subjects/section.teachers.html' };
    $scope.filesSection = { name: 'files', url: 'adminapp/views/subjects/section.files.html' };
    $scope.requestsSection = { name: 'requests', url: 'adminapp/views/subjects/section.requests.html' };
    $scope.newsSection = { name: 'news', url: 'adminapp/views/subjects/section.news.html' };
    $scope.syllabusSection = { name: 'syllabus', url: 'adminapp/views/subjects/section.syllabus.html' };
    $scope.scheduleSection = { name: 'schedule', url: 'adminapp/views/subjects/section.schedule.html' };

    $scope.editMode = false;
    if ($stateParams.subjectName == "newSubject") {
        $scope.editMode = true;
    }

    //$scope.oneAtATime = true;

    $scope.tinymceOptions = {
        lplugins: 'textcolor link code',
        toolbar: "undo redo styleselect bold italic forecolor backcolor code",
        menu: { // this is the complete default configuration
            edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall' },
            insert: { title: 'Insert', items: 'link media | template hr' },
            format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat' },
        },

        setup: function (ed) {
        },
    };

    $scope.edit = function() {
        $scope.editMode = true;
        //TODO:
    };

    $scope.update = function () {
        utils.showValidation($scope.subjectForm);
        if ($scope.subjectForm.$valid) {
            if ($scope.subject.Id == null) {
                $scope.subject.Syllabus = new Object();
                $scope.subject.Schedule = new Object();
                subjectsService.post($scope.subject, function(response) {
                    $scope.subject = response;
                    $scope.subjects.push($scope.subject);
                    $scope.editMode = false;
                }, errorHandler);
            } else {
                subjectsService.update({ id: $scope.subject.Id }, $scope.subject, function (response) {
                    $scope.subject = response;
                    $scope.editMode = false;
                }, errorHandler);
            }
        }
    };

    $scope.createNews = function () {
        var news = new Object();
        $scope.subject.News.push(news);
    };

    var errorHandler = function (response) {
        var errors = utils.parseErrors(response.data.ModelState);
        for (var i = 0; i < errors.length; i++) {
            var alert = { type: 'alert', msg: 'Error: ' + errors[i] };
            $scope.addAlert(alert);
        }
    };

    /**
     * Removes alert with given idex
     * @param {int} index
     */
    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    /**
     * Adds alert to set of allerts assosiated with edited page.
     * @param {type} alert
     */
    $scope.addAlert = function (alert) {
        $scope.alerts.push(alert);
    };
})