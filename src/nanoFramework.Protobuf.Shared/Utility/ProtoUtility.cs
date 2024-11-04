using nanoFramework.Protobuf.Dto;
using System;
using System.Collections;
using System.Reflection;

namespace nanoFramework.Protobuf.Utility
{
    internal static class ProtoUtility
    {
        #region constants

        public const int ObjectStart = 0x80;
        public const int ObjectEnd = 0x81;
        public const int InheritedObjectStart = 0x82;

        #endregion

        #region static dictionaries

        private static readonly Hashtable MappingDictionary = new Hashtable();

        #endregion

        #region public

        public static MemberMapping[] GetMappings(Type targetType)
        {
            var cached = MappingDictionary[targetType];
            if (cached is MemberMapping[] cachedMappings) return cachedMappings;

            var mappings = new ArrayList();

            FieldUtility.Map(targetType, mappings);
            PropertyUtility.Map(targetType, mappings);

            var result = (MemberMapping[])mappings.ToArray(typeof(MemberMapping));

            //check for duplicates
            for (int i = 0; i < result.Length; i++)
                for (int j = i + 1; j < result.Length; j++)
                    if (result[i].Tag == result[j].Tag)
                        ThrowDuplicateTagException(targetType, result, i, j);
                    else if (result[i].Name == result[j].Name)
                        ThrowDuplicateMemberException(targetType, result, i, j);

            MappingDictionary.Add(targetType, result);

            return result;
        }


        public static ProtoMemberAttribute GetProtoMember(this MemberInfo member)
        {
            var attributes = member.GetCustomAttributes(true);

            foreach (var attribute in attributes)
                if (attribute is ProtoMemberAttribute protoAttribute)
                    return protoAttribute;

            return null;
        }

        #endregion

        #region private

        private static void ThrowDuplicateTagException(Type targetType, MemberMapping[] result, int first, int second)
        {
            var message = $"Duplicate Proto Tag {result[first].Tag} found on {targetType.FullName}." +
                $" for member '{result[first].Name}' with ProtoMember {result[first].ProtoMember}";

            if (result[first].ProtoInclude > 0)
                message += $" and ProtoInclude {result[first].ProtoInclude}";

            message += $" and member '{result[second].Name}' with ProtoMember {result[second].ProtoMember}";

            if (result[second].ProtoInclude > 0)
                message += $" and ProtoInclude {result[second].ProtoInclude}.";

            throw new Exception(message);
        }

        private static void ThrowDuplicateMemberException(Type targetType, MemberMapping[] result, int first, int second)
        {
            var message = $"Duplicate member '{result[first].Name}' found on {result[first].DeclaringType.FullName}." +
                $" with ProtoMember {result[first].ProtoMember}";

            if (result[first].ProtoInclude > 0)
                message += $" and ProtoInclude {result[first].ProtoInclude}";

            message += $" and on {result[second].DeclaringType.FullName} with ProtoMember {result[second].ProtoMember}";

            if (result[second].ProtoInclude > 0)
                message += $" and ProtoInclude {result[second].ProtoInclude}.";

            throw new Exception(message);
        }

        #endregion
    }
}
