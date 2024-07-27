using System;
using System.Collections.Generic;

namespace store_management_mono_api.Entities;

public partial class ConfigValue
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;
}
