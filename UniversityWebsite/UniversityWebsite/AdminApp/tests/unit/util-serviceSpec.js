
describe('util-service', function() {

    beforeEach(module("configApp.utils.service"));

    it('findy by id', inject(function(utils) {
        var items = [{ Id: 1, Name: 'name1' }, { Id: 2, Name: 'name2' }];
        expect(utils.findById(items, 2)).toBe(items[1]);
    }));
});