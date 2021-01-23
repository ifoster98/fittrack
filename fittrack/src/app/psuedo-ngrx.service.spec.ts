import { TestBed } from '@angular/core/testing';

import { PsuedoNgrxService, ViewPage } from './psuedo-ngrx.service';

describe('PsuedoNgrxService', () => {
  let service: PsuedoNgrxService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PsuedoNgrxService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should not have a user id on initialisation', () => {
    expect(service.getUser()).toBeUndefined();
    expect(service.isLoggedIn()).toBeFalse();
  })

  it('should be logged in after logging in', () => {
    service.login(42);
    expect(service.isLoggedIn()).toBeTrue();
  });

  it('should not have a user id after logging out', () => {
    service.logout();
    expect(service.getUser()).toBeUndefined();
    expect(service.isLoggedIn()).toBeFalse();
  });

  it('should not have a match id after logging out', () => {
    service.login(42);
    service.chooseMatch({id: 22, name: 'test'});
    expect(service.getMatch()).toEqual({id: 22, name: 'test'});
    service.logout();
    expect(service.getMatch()).toBeUndefined();
  });
  
  it('should show None as current page if not logged in', () => {
    expect(service.currentPage()).toEqual(ViewPage.None);
  });

  it('should show MatchSelection as current page if logged in and no match selected', () => {
    service.login(42);
    expect(service.currentPage()).toEqual(ViewPage.MatchSelection);
  });

  it('should not have a match id on initialisation', () => {
    expect(service.getMatch()).toBeUndefined();
  });

  it('should be at a match id after choosing a match', () => {
    service.chooseMatch({id: 22, name: 'test'});
    expect(service.isAtMatch()).toBeTrue();
  });

  it('should not have a match id after ending the match', () => {
    service.endMatch();
    expect(service.getMatch()).toBeUndefined();
  });

  it('should show EventEntry as current page if logged in and match is selected', () => {
    service.login(42);
    service.chooseMatch({id: 22, name: 'test'});
    expect(service.currentPage()).toEqual(ViewPage.EventEntry);
  });

  it('should not have matchEvents on initialisation', () => {
    expect(service.getMatchEvents()).toBeUndefined();
  });

  it('should have match events to edit after setting events', () => {
    service.setMatchEvents([{"userId": 42, "matchId": 22, eventTime: '2020-12-12', matchEventType: 0}]);
    expect(service.hasMatchEventsToEdit()).toBeTrue();
  });

  it('should show EventEdit as current page if logged in, match is selected and events are available to edit', () => {
    service.login(42);
    service.chooseMatch({id: 22, name: 'test'});
    service.setMatchEvents([{"userId": 42, "matchId": 22, eventTime: '2020-12-12', matchEventType: 0}]);
    expect(service.currentPage()).toEqual(ViewPage.EventEdit);
  });
});
