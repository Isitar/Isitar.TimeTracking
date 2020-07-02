namespace Isitar.TimeTracking.Application.Common.NotFoundException
{
    using System;
    using global::Common.Resources;

    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base(Translation.NotFoundException.Replace("{name}", name).Replace("{key}", key.ToString())) { }
    }
}