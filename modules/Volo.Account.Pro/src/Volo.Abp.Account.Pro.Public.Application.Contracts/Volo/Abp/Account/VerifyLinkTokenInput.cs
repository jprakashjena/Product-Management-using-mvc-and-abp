﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Account;

public class VerifyLinkTokenInput
{
    [Required]
    public Guid UserId { get; set; }

    public Guid? TenantId { get; set; }

    [Required]
    public string Token { get; set; }
}
