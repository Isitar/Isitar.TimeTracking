namespace Isitar.TimeTracking.Api.Attributes
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class ProducesBadRequestResponseAttribute : ProducesResponseTypeAttribute
    {
        public ProducesBadRequestResponseAttribute() : base(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest) { }
    }
}