using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

public interface ISharedResource
{
}
public class SharedResource : ISharedResource
{
    private readonly IStringLocalizer _localizer;

    public SharedResource(IStringLocalizer<SharedResource> localizer)
    {
        _localizer = localizer;
    }

    public string this[string index]
    {
        get
        {
            return _localizer[index];
        }
    }
}

