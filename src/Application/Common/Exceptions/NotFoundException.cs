namespace Isitar.TimeTracking.Application.Common.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using global::Common.Resources;

    public class NotFoundException : Exception
    {
        public NotFoundException(string name, [NotNull] object key)
            : base(Translation.NotFoundException.Replace("{name}", name).Replace("{key}", key.ToString())) { }
    }
}