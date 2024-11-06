// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using nanoFramework.Protobuf.Test.Domain;

namespace nanoFramework.Protobuf.Test
{
    public static class ProtobufTests
    {
        public static void EndToEndTest()
        {
            var obj = ObjectProvider.CreateObject();

            ValidateEndToEndObject(obj);

            var serializer = new Serializer();

            var result = serializer.Serialize(obj);

            var deserialized = serializer.Deserialize(typeof(SomeObject), result) as SomeObject;

            Debug.Assert(deserialized != null);

            ValidateEndToEndObject(deserialized);
        }

        public static void BigStringTest()
        {
            string str = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMN";

            for (int i = 0; i < 12; i++)
                str += str;

            var obj = new SomeObject { StringProperty = str };

            var serializer = new Serializer(StreamType.ProtobufStream);

            var result = serializer.Serialize(obj);

            var deserialized = serializer.Deserialize(typeof(SomeObject), result) as SomeObject;

            Debug.Assert(deserialized != null);

            Debug.Assert(obj.StringProperty == deserialized.StringProperty);
        }

        public static void ValidateEndToEndObject(SomeObject obj)
        {
            var derivedObject = obj as SomeObjectDerived;
            Debug.Assert(derivedObject != null, $"The object should be a {nameof(SomeObjectDerived)}");

            Debug.Assert(derivedObject.StringProperty == "abc");
            Debug.Assert(derivedObject.IntProperty == 123);
            Debug.Assert(derivedObject.DerivedProperty);

            Debug.Assert(derivedObject.ChildObject != null);
            Debug.Assert(derivedObject.ChildObject.StringProperty == "def");
            Debug.Assert(derivedObject.ChildObject.IntProperty == 456);

            Debug.Assert(derivedObject.DerivedChildObject != null);
            Debug.Assert(derivedObject.DerivedChildObject.StringProperty == "ghi");
            Debug.Assert(derivedObject.DerivedChildObject.IntProperty == 789);
            Debug.Assert(derivedObject.DerivedChildObject.DerivedProperty);
        }
    }
}
