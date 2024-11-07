// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Text;
using nanoFramework.Protobuf.Test.Domain;

#if NANOFRAMEWORK_1_0
using nanoFramework.TestFramework;
#endif

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
            //Purpose of this test is to serialize and deserialize a payload > 65,535 bytes since this is the
            //limit of MemoryStream. An alternative ProtobufStream is provided for this need and is indicated
            //through the Serializer constructed.
            //We try/catch OutOfMemoryExceptions and swallow them since boards with too few memory will always
            //fail this test which is to be expected and can be ignored.
            try
            {
                string stringChunk = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMN";
                StringBuilder strBuilder = new(stringChunk);

                while (strBuilder.Length < ushort.MaxValue)
                {
                    _ = strBuilder.Append(Guid.NewGuid().ToString());
                }

                string str = strBuilder.ToString();

                var obj = new SomeObject { StringProperty = str };

                var serializer = new Serializer(StreamType.ProtobufStream);

                byte[] result = serializer.Serialize(obj);

                var deserialized = serializer.Deserialize(typeof(SomeObject), result) as SomeObject;

                Debug.Assert(deserialized != null);

                Debug.Assert(obj.StringProperty == deserialized.StringProperty);
            }
            catch (OutOfMemoryException)
            {
                // swallow exception so the test doesn't fail when to be expected
                // won't happen on full .NET Framework, so no need to output anything

#if NANOFRAMEWORK_1_0
                OutputHelper.WriteLine($"*************************************");
                OutputHelper.WriteLine($"*** OutOfMemoryException occurred ***");
                OutputHelper.WriteLine($"*************************************");
#endif

            }
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
