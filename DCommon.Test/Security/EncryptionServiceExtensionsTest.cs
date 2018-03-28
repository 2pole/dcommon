using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using DCommon.Security;

namespace DCommon.Tests.Security
{
    public class EncryptionServiceExtensionsTest : TestFixtureBase<EncryptionServiceContext>
    {
        [Fact]
        public void EncodeAndDecode()
        {
            var service = base.FixtureContext.EncryptionService;
            var username = "2Pole";
            var encodedUsername = service.Encode(username);
            var decodedUsername = service.Decode(encodedUsername);

            Assert.Equal(username, decodedUsername);
        }

        [Fact]
        public void EncodeAndDecode_2()
        {
            var service = base.FixtureContext.EncryptionService;
            var username = "Z";
            var encodedUsername = service.Encode(username);
            var decodedUsername = service.Decode(encodedUsername);

            Assert.Equal(username, decodedUsername);
        }

        [Fact]
        public void HashTest()
        {
            var service = base.FixtureContext.EncryptionService;

            var username = "2Pole";
            var hashed1 = service.Hash(username);
            var hashed2 = service.Hash(username);

            Assert.Equal(hashed1.AsEnumerable(), hashed2.AsEnumerable());
        }

        [Fact]
        public void HashTest_2()
        {
            var service = base.FixtureContext.EncryptionService;

            var username = "Z";
            var hashed1 = service.Hash(username);
            var hashed2 = service.Hash(username);

            Assert.Equal(hashed1.AsEnumerable(), hashed2.AsEnumerable());
        }
    }
}
