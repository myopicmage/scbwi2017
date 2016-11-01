﻿// Write your Javascript code.
var app = angular.module('scbwi', ['ngMaterial']);

app.config(function ($mdThemingProvider) {
    $mdThemingProvider.definePalette('amazingPaletteName', {
        '50': 'ffebee',
        '100': 'ffcdd2',
        '200': 'ef9a9a',
        '300': 'e57373',
        '400': 'ef5350',
        '500': 'f69331', //main
        '600': 'e53935',
        '700': 'd32f2f',
        '800': 'ffcb98', //md-hue-2
        '900': 'b71c1c',
        'A100': 'EF5B34',
        'A200': 'ff5252',
        'A400': 'ff1744',
        'A700': 'd50000',
        'contrastDefaultColor': 'light',    // whether, by default, text (contrast)
        // on this palette should be dark or light
        'contrastDarkColors': ['50', '100', //hues which contrast should be 'dark' by default
         '200', '300', '400', 'A100'],
        'contrastLightColors': undefined    // could also specify this if default was 'dark'
    });

    $mdThemingProvider
        .theme('default')
        .primaryPalette('amazingPaletteName', {
            'default': '500',
            'hue-2': '800',
            'hue-3': 'A100'
        });
});

function AppController(menu, $scope, error) {
    var self = this;

    self.step = menu.getStep();
    self.isMember = menu.getMember();
    self.allowSunday = menu.getSunday();
    self.friday = menu.getFriday();

    menu.subscribe($scope, function () {
        self.step = menu.getStep();
    });

    menu.subscribeMember($scope, function () {
        self.isMember = menu.getMember();
    });

    menu.subscribeSunday($scope, function () {
        self.allowSunday = menu.getSunday();
    });

    menu.subscribeFriday($scope, function () {
        self.friday = menu.getFriday();
    });

    error.subscribe($scope, function (type, text) {
    });
}

function RegController(menu, info) {
    var self = this;

    self.regTypes = [];
    self.reg = {};
    self.reg.user = {
        first: 'kevin',
        last: 'bernfeld',
        address1: '370 Ocean Ave',
        address2: 'apt 1007',
        city: 'revere',
        state: 'ma',
        zip: '02151',
        country: 'USA',
        email: 'kcbernfeld@gmail.com',
        phone: '786.566.2467'
    };
    self.selectedReg = {};
    self.regType = {};
    self.reg.manuscripts = 0;
    self.reg.portfolios = 0;
    self.reg.satdinner = 0;
    self.prices = {};
    self.disableCouponButton = false;

    //step 0
    self.member = function (isMember) {
        menu.setMember(isMember);
        menu.setFriday(isMember);

        info.getRegTypes(isMember, function (data) {
            self.regTypes = data;
        });

        info.getPrices(function (data) {
            self.prices = data;
        });

        info.getWorkshops(function (data) {
            self.earlyworkshops = data.early;
            self.lateworkshops = data.late;
        });

        menu.setStep(1);
    }

    //step 1
    self.userInfo = function () {
        if (menu.getMember()) {
            info.getComprehensives(function (data) {
                self.comprehensives = data;
            });
        }

        menu.setStep(2);
    }

    // step 2
    self.typeSelect = function () {
        info.getMeals(function (data) {
            self.meals = data;
        });

        var next = 3;

        if (menu.getFriday()) {
            next = 3;
        } else if (menu.getSunday()) {
            next = 4;
        } else {
            next = 5;
        }

        menu.setStep(next);
    }

    //step 3
    self.comprehensive = function (ignoreSelection) {
        if (ignoreSelection) {
            self.reg.comprehensive = 0;
        }
        
        var next = 4;

        if (!menu.getSunday()) {
            next = 5;
        }

        menu.setStep(next);
    }

    //step 4
    self.workshops = function () {
        menu.setStep(5);
    }

    //step 5
    self.extras = function () {
        var c = self.comprehensives.single(function (item) {
            return item.id === self.reg.comprehensive;
        });

        var w1 = self.earlyworkshops.single(function (item) {
            return item.id === self.reg.first;
        });

        var w2 = self.lateworkshops.single(function (item) {
            return item.id === self.reg.second;
        });

        var m1 = self.meals.single(function (item) {
            return item.id === self.reg.meal;
        });

        self.c_name = !c ? "None Chosen" : c.title;
        self.w_first = !w1 ? "None Chosen" : w1.title;
        self.w_second = !w2 ? "None Chosen" : w2.title;
        self.meal = !m1 ? "None Chosen" : m1.title;

        info.getTotal(self.reg, function (data) {
            self.subtotal = data.subtotal;
            self.total = data.total;
        });

        menu.setStep(6);
    }

    self.submitCoupon = function () {
        self.disableCouponButton = true;

        info.getTotal(self.reg, function (data) {
            self.subtotal = data.subtotal;
            self.total = data.total;
            self.couponsuccess = data.validcoupon;
            self.c_message = data.msg;
            self.c_msg_show = true;
            self.disableCouponButton = false;
        });
    }

    // on select of registration type
    self.select = function () {
        self.regType = self.regTypes.single(function (item) {
            return item.id === self.reg.type;
        });

        menu.setSunday(self.regType.sunday);
        menu.setFriday(self.regType.friday || (menu.getMember() && self.regType.sunday));
    }
}

function ErrorService($rootScope) {
    var errorService = {};

    errorService.subscribe = function (scope, callback) {
        var handler = $rootScope.$on('error-event', callback);
        scope.$on('$destroy', handler);
    };

    errorService.error = function (type, text) {
        $rootScope.$emit('error-event', type, text);
    }

    return errorService;
}

function MenuService($rootScope) {
    var menuService = {};

    menuService.step = 0;
    menuService.isMember = true;
    menuService.allowSunday = true;
    menuService.friday = true;

    menuService.getStep = function () {
        return menuService.step;
    };

    menuService.getMember = function () {
        return menuService.isMember;
    }

    menuService.getSunday = function () {
        return menuService.allowSunday;
    }

    menuService.getFriday = function () {
        return menuService.friday;
    }

    menuService.setStep = function (newstep) {
        menuService.step = newstep;
        $rootScope.$emit('menu-event');
    };

    menuService.setMember = function (isMember) {
        menuService.isMember = isMember;
        $rootScope.$emit('member-event');
    };

    menuService.setSunday = function (sunday) {
        menuService.allowSunday = sunday;
        $rootScope.$emit('sunday-event');
    }

    menuService.setFriday = function (friday) {
        menuService.friday = friday;
        $rootScope.$emit('friday-event');
    }

    menuService.subscribe = function (scope, callback) {
        var handler = $rootScope.$on('menu-event', callback);
        scope.$on('$destroy', handler);
    };

    menuService.subscribeMember = function (scope, callback) {
        var handler = $rootScope.$on('member-event', callback);
        scope.$on('$destroy', handler);
    };

    menuService.subscribeSunday = function (scope, callback) {
        var handler = $rootScope.$on('sunday-event', callback);
        scope.$on('$destroy', handler);
    };

    menuService.subscribeFriday = function (scope, callback) {
        var handler = $rootScope.$on('friday-event', callback);
        scope.$on('$destroy', handler);
    }

    return menuService;
}

function InfoService($http, error) {
    var infoService = {};

    infoService.regTypes = [];
    infoService.comprehensives = [];
    infoService.meals = [];
    infoService.workshops = [];
    infoService.manuscript = -1;
    infoService.portfolio = -1;
    infoService.satdinner = -1;

    function e() {
        error.error("error", "Uh oh, failed to download some data. Please refresh the page.");
    }

    infoService.getRegTypes = function (isMember, callback) {
        if (infoService.regTypes.length === 0) {
            function success(data, status, headers, config) {
                infoService.regTypes = data.data;

                callback(infoService.regTypes);
            }

            $http
                .post('/register/regtypes', isMember)
                .then(success, e);
        } else {
            callback(infoService.regTypes);
        }
    }

    infoService.getComprehensives = function (callback) {
        if (infoService.comprehensives.length === 0) {
            function success(data, status, headers, config) {
                infoService.comprehensives = data.data;

                callback(infoService.comprehensives);
            }

            $http
                .post('/register/comprehensives')
                .then(success, e);
        } else {
            callback(infoService.comprehensives);
        }
    }

    infoService.getMeals = function (callback) {
        if (infoService.meals.length === 0) {
            function success(data) {
                infoService.meals = data.data;

                callback(infoService.meals);
            }

            $http
                .get('/register/meals')
                .then(success, e);
        } else {
            callback(infoService.meals);
        }
    }

    infoService.getWorkshops = function (callback) {
        if (infoService.workshops.length === 0) {
            function success(data) {
                infoService.workshops = data.data;

                callback(infoService.workshops);
            }

            $http
                .get('/register/workshops')
                .then(success, e);
        } else {
            callback(infoService.workshops);
        }
    }

    infoService.getPrices = function (callback) {
        if (infoService.manuscripts === -1 || infoService.portfolios === -1 || infoService.satdinner === -1) {
            function success(data) {
                for (var i = 0; i < data.data.length; i++) {
                    infoService[data.data[i].type] = data.data[i].value;
                }

                var prices = {
                    manuscript: infoService.manuscript,
                    portfolio: infoService.portfolio,
                    satdinner: infoService.satdinner
                };

                callback(prices);
            }

            $http
                .get('/register/price?type=satdinner,portfolio,manuscript')
                .then(success, e);
        } else {
            callback({
                manuscript: infoService.manuscript,
                portfolio: infoService.portfolio,
                satdinner: infoService.satdinner
            });
        }
    }

    infoService.getTotal = function (reg, callback) {
        function success(response) {
            callback({
                validcoupon: response.data.validcoupon,
                subtotal: response.data.subtotal,
                total: response.data.total,
                msg: response.data.msg
            });
        }

        $http
            .post('/register/total', reg)
            .then(success, e);
    }

    return infoService;
}

app.factory('menu', MenuService);
app.factory('info', InfoService);
app.factory('error', ErrorService);
app.controller('AppCtrl', AppController);
app.controller('RegCtrl', RegController);

Array.prototype.single = function (func) {
    var temp = this.filter(func);

    if (temp.length === 1) {
        return temp[0];
    } else {
        return null;
    }
};