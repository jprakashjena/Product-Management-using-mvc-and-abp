﻿using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity;

public class IdentityTestData : ISingletonDependency
{
    public Guid RoleModeratorId { get; } = Guid.NewGuid();

    public Guid UserJohnId { get; } = Guid.NewGuid();
    public Guid UserDavidId { get; } = Guid.NewGuid();
    public Guid UserNeoId { get; } = Guid.NewGuid();
    public Guid AgeClaimId { get; } = Guid.NewGuid();
    public Guid EducationClaimId { get; } = Guid.NewGuid();

    public Guid UserJohnSecurityLogId { get; set; }

    public Guid UserDavidSecurityLogId { get; set; }
}
