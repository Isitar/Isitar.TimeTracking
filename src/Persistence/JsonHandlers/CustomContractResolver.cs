namespace Isitar.TimeTracking.Persistence.JsonHandlers
{
    using System;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class CustomContractResolver : DefaultContractResolver
    {
        private readonly Func<bool> includeProperty;

        public CustomContractResolver(Func<bool> includeProperty)
        {
            this.includeProperty = includeProperty;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var shouldSerialize = property.ShouldSerialize;
            property.ShouldSerialize = obj => includeProperty() &&
                                              (null == shouldSerialize || shouldSerialize(obj));
            return property;
        }
    }
}