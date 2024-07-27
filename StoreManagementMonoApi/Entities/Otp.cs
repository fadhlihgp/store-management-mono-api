using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class Otp
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Value { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiredAt { get; set; }

    public bool IsVerified { get; set; }
}
