/* This file is automatically generated by ABP framework to use MVC Controllers from javascript. */


// module identity

(function(){

  // controller volo.abp.identity.identityClaimType

  (function(){

    abp.utils.createNamespace(window, 'volo.abp.identity.identityClaimType');

    volo.abp.identity.identityClaimType.getList = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/claim-types' + abp.utils.buildQueryString([{ name: 'filter', value: input.filter }, { name: 'sorting', value: input.sorting }, { name: 'skipCount', value: input.skipCount }, { name: 'maxResultCount', value: input.maxResultCount }]) + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityClaimType.get = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/claim-types/' + id + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityClaimType.create = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/claim-types',
        type: 'POST',
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.identityClaimType.update = function(id, input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/claim-types/' + id + '',
        type: 'PUT',
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.identityClaimType['delete'] = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/claim-types/' + id + '',
        type: 'DELETE',
        dataType: null
      }, ajaxParams));
    };

  })();

  // controller volo.abp.identity.identityRole

  (function(){

    abp.utils.createNamespace(window, 'volo.abp.identity.identityRole');

    volo.abp.identity.identityRole.get = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/roles/' + id + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityRole.create = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/roles',
        type: 'POST',
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.identityRole.update = function(id, input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/roles/' + id + '',
        type: 'PUT',
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.identityRole['delete'] = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/roles/' + id + '',
        type: 'DELETE',
        dataType: null
      }, ajaxParams));
    };

    volo.abp.identity.identityRole.getAllList = function(ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/roles/all',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityRole.getList = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/roles' + abp.utils.buildQueryString([{ name: 'filter', value: input.filter }, { name: 'sorting', value: input.sorting }, { name: 'skipCount', value: input.skipCount }, { name: 'maxResultCount', value: input.maxResultCount }]) + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityRole.updateClaims = function(id, input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/roles/' + id + '/claims',
        type: 'PUT',
        dataType: null,
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.identityRole.getClaims = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/roles/' + id + '/claims',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityRole.getAllClaimTypes = function(ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/roles/all-claim-types',
        type: 'GET'
      }, ajaxParams));
    };

  })();

  // controller volo.abp.identity.identitySecurityLog

  (function(){

    abp.utils.createNamespace(window, 'volo.abp.identity.identitySecurityLog');

    volo.abp.identity.identitySecurityLog.getList = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/security-logs' + abp.utils.buildQueryString([{ name: 'startTime', value: input.startTime }, { name: 'endTime', value: input.endTime }, { name: 'applicationName', value: input.applicationName }, { name: 'identity', value: input.identity }, { name: 'action', value: input.action }, { name: 'userName', value: input.userName }, { name: 'clientId', value: input.clientId }, { name: 'correlationId', value: input.correlationId }, { name: 'sorting', value: input.sorting }, { name: 'skipCount', value: input.skipCount }, { name: 'maxResultCount', value: input.maxResultCount }]) + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identitySecurityLog.get = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/security-logs/' + id + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identitySecurityLog.getMyList = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/security-logs/my' + abp.utils.buildQueryString([{ name: 'startTime', value: input.startTime }, { name: 'endTime', value: input.endTime }, { name: 'applicationName', value: input.applicationName }, { name: 'identity', value: input.identity }, { name: 'action', value: input.action }, { name: 'userName', value: input.userName }, { name: 'clientId', value: input.clientId }, { name: 'correlationId', value: input.correlationId }, { name: 'sorting', value: input.sorting }, { name: 'skipCount', value: input.skipCount }, { name: 'maxResultCount', value: input.maxResultCount }]) + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identitySecurityLog.getMy = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/security-logs/my/' + id + '',
        type: 'GET'
      }, ajaxParams));
    };

  })();

  // controller volo.abp.identity.identitySettings

  (function(){

    abp.utils.createNamespace(window, 'volo.abp.identity.identitySettings');

    volo.abp.identity.identitySettings.get = function(ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/settings',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identitySettings.update = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/settings',
        type: 'PUT',
        dataType: null,
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.identitySettings.getLdap = function(ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/settings/ldap',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identitySettings.updateLdap = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/settings/ldap',
        type: 'PUT',
        dataType: null,
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.identitySettings.getOAuth = function(ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/settings/oauth',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identitySettings.updateOAuth = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/settings/oauth',
        type: 'PUT',
        dataType: null,
        data: JSON.stringify(input)
      }, ajaxParams));
    };

  })();

  // controller volo.abp.identity.identityUser

  (function(){

    abp.utils.createNamespace(window, 'volo.abp.identity.identityUser');

    volo.abp.identity.identityUser.get = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/' + id + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.getList = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users' + abp.utils.buildQueryString([{ name: 'filter', value: input.filter }, { name: 'roleId', value: input.roleId }, { name: 'organizationUnitId', value: input.organizationUnitId }, { name: 'userName', value: input.userName }, { name: 'phoneNumber', value: input.phoneNumber }, { name: 'emailAddress', value: input.emailAddress }, { name: 'isLockedOut', value: input.isLockedOut }, { name: 'notActive', value: input.notActive }, { name: 'sorting', value: input.sorting }, { name: 'skipCount', value: input.skipCount }, { name: 'maxResultCount', value: input.maxResultCount }]) + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.create = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users',
        type: 'POST',
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.update = function(id, input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/' + id + '',
        type: 'PUT',
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.identityUser['delete'] = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/' + id + '',
        type: 'DELETE',
        dataType: null
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.getRoles = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/' + id + '/roles',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.getAssignableRoles = function(ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/assignable-roles',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.getAvailableOrganizationUnits = function(ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/available-organization-units',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.getAllClaimTypes = function(ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/all-claim-types',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.getClaims = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/' + id + '/claims',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.getOrganizationUnits = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/' + id + '/organization-units',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.updateRoles = function(id, input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/' + id + '/roles',
        type: 'PUT',
        dataType: null,
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.updateClaims = function(id, input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/' + id + '/claims',
        type: 'PUT',
        dataType: null,
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.lock = function(id, lockoutEnd, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/' + id + '/lock/' + lockoutEnd + '',
        type: 'PUT',
        dataType: null
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.unlock = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/' + id + '/unlock',
        type: 'PUT',
        dataType: null
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.findByUsername = function(username, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/by-username/' + username + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.findByEmail = function(email, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/by-email/' + email + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.getTwoFactorEnabled = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/' + id + '/two-factor-enabled',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.setTwoFactorEnabled = function(id, enabled, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/' + id + '/two-factor/' + enabled + '',
        type: 'PUT',
        dataType: null
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.updatePassword = function(id, input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/' + id + '/change-password',
        type: 'PUT',
        dataType: null,
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.getRoleLookup = function(ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/lookup/roles',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.getOrganizationUnitLookup = function(ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/lookup/organization-units',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.getExternalLoginProviders = function(ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/external-login-Providers',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUser.importExternalUser = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'import-external-user',
        type: 'POST',
        data: JSON.stringify(input)
      }, ajaxParams));
    };

  })();

  // controller volo.abp.identity.identityUserLookup

  (function(){

    abp.utils.createNamespace(window, 'volo.abp.identity.identityUserLookup');

    volo.abp.identity.identityUserLookup.findById = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/lookup/' + id + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUserLookup.findByUserName = function(userName, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/lookup/by-username/' + userName + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUserLookup.search = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/lookup/search' + abp.utils.buildQueryString([{ name: 'sorting', value: input.sorting }, { name: 'filter', value: input.filter }, { name: 'skipCount', value: input.skipCount }, { name: 'maxResultCount', value: input.maxResultCount }]) + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.identityUserLookup.getCount = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/users/lookup/count' + abp.utils.buildQueryString([{ name: 'filter', value: input.filter }]) + '',
        type: 'GET'
      }, ajaxParams));
    };

  })();

  // controller volo.abp.identity.organizationUnit

  (function(){

    abp.utils.createNamespace(window, 'volo.abp.identity.organizationUnit');

    volo.abp.identity.organizationUnit.addRoles = function(id, input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units/' + id + '/roles',
        type: 'PUT',
        dataType: null,
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit.addMembers = function(id, input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units/' + id + '/members',
        type: 'PUT',
        dataType: null,
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit.create = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units',
        type: 'POST',
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit['delete'] = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units' + abp.utils.buildQueryString([{ name: 'id', value: id }]) + '',
        type: 'DELETE',
        dataType: null
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit.get = function(id, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units/' + id + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit.getList = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units' + abp.utils.buildQueryString([{ name: 'filter', value: input.filter }, { name: 'sorting', value: input.sorting }, { name: 'skipCount', value: input.skipCount }, { name: 'maxResultCount', value: input.maxResultCount }]) + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit.getListAll = function(ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units/all',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit.getRoles = function(id, input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units/' + id + '/roles' + abp.utils.buildQueryString([{ name: 'skipCount', value: input.skipCount }, { name: 'maxResultCount', value: input.maxResultCount }, { name: 'sorting', value: input.sorting }]) + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit.getMembers = function(id, input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units/' + id + '/members' + abp.utils.buildQueryString([{ name: 'filter', value: input.filter }, { name: 'roleId', value: input.roleId }, { name: 'organizationUnitId', value: input.organizationUnitId }, { name: 'userName', value: input.userName }, { name: 'phoneNumber', value: input.phoneNumber }, { name: 'emailAddress', value: input.emailAddress }, { name: 'isLockedOut', value: input.isLockedOut }, { name: 'notActive', value: input.notActive }, { name: 'sorting', value: input.sorting }, { name: 'skipCount', value: input.skipCount }, { name: 'maxResultCount', value: input.maxResultCount }]) + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit.move = function(id, input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units/' + id + '/move',
        type: 'PUT',
        dataType: null,
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit.getAvailableUsers = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units/available-users' + abp.utils.buildQueryString([{ name: 'filter', value: input.filter }, { name: 'id', value: input.id }, { name: 'sorting', value: input.sorting }, { name: 'skipCount', value: input.skipCount }, { name: 'maxResultCount', value: input.maxResultCount }]) + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit.getAvailableRoles = function(input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units/available-roles' + abp.utils.buildQueryString([{ name: 'filter', value: input.filter }, { name: 'id', value: input.id }, { name: 'sorting', value: input.sorting }, { name: 'skipCount', value: input.skipCount }, { name: 'maxResultCount', value: input.maxResultCount }]) + '',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit.update = function(id, input, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units/' + id + '',
        type: 'PUT',
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit.removeMember = function(id, memberId, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units/' + id + '/members/' + memberId + '',
        type: 'DELETE',
        dataType: null
      }, ajaxParams));
    };

    volo.abp.identity.organizationUnit.removeRole = function(id, roleId, ajaxParams) {
      return abp.ajax($.extend(true, {
        url: abp.appPath + 'api/identity/organization-units/' + id + '/roles/' + roleId + '',
        type: 'DELETE',
        dataType: null
      }, ajaxParams));
    };

  })();

})();

