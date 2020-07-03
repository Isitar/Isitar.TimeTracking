namespace Isitar.TimeTracking.Application.Common.Exceptions
{
    using System;
    using global::Common.Resources;

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
            : base(Translation.UnauthorizedException) { }
    }
}