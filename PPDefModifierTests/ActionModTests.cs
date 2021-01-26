using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PPDefModifier;
using System;
using System.Collections.Generic;

namespace PPDefModifierTests
{
    using ModnixAction = Dictionary<string, object>;
    using MockRepo  = ApplyModTests.MockRepo;
    using TestClass = ApplyModTests.TestClass;

    [TestClass]
    public class ActionModTests
    {
        public static class MockApi
        {
            public static object mockApiPath;

            public static object mockApiVersion;

            public static object Api(string api, object _)
            {
                switch (api?.ToLowerInvariant())
                {
                    case "path":
                        return mockApiPath;
                    case "version":
                        return mockApiVersion;
                    default:
                        return null;
                }
            }
        }

        [TestMethod]
        public void ConversionTest()
        {
            ModDefinition mod = new ModDefinition();
            ModnixAction action = JsonConvert.DeserializeObject<ModnixAction>(@"{
                guid: ""g"",
                cls: ""c"",
                field: ""f"",
                value: ""v"",
                comment: ""//"",
                flags: [""foo"",""bar""],
                modletlist : [
                    { ""field"": ""mf1"", ""value"": ""mv1"" },
                    { ""field"": ""mf2"", ""value"": 1234 },
                ],
            }");

            PPDefModifier.PPDefModifier.ConvertActionToMod(action, mod);
            Assert.AreEqual(mod.guid, "g");
            Assert.AreEqual(mod.cls , "c");
            Assert.AreEqual(mod.field, "f");
            Assert.AreEqual(mod.value, "v");
            Assert.AreEqual(mod.comment, "//");
            Assert.AreEqual(mod.flags, new string[]{ "foo", "bar" });
            Assert.AreEqual(mod.modletlist?.Count, 2);
            Assert.AreEqual(mod.modletlist[0], new ModletStep{ field = "mf1", value = "mv1" });
            Assert.AreEqual(mod.modletlist[1], new ModletStep{ field = "mf2", value = 1234 });
        }
    }
}
