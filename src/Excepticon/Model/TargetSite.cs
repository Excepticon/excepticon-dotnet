using System.Reflection;

namespace Excepticon.Model
{
    public class TargetSite
    {
        public TargetSite(MethodBase targetSite)
        {
            DeclaringTypeName = targetSite.DeclaringType?.Name;
            DeclaringTypeFullName = targetSite.DeclaringType?.FullName;
            MemberType = targetSite.MemberType;
            Name = targetSite.Name;
        }

        public string DeclaringTypeName { get; set; }

        public string DeclaringTypeFullName { get; set; }

        public MemberTypes MemberType { get; set; }

        public string Name { get; set; }
    }
}
