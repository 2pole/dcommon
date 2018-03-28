using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCommon.Environment.Configuration;
using DCommon.Security;
using DCommon.Security.Providers;
using Xunit;

namespace DCommon.Tests.Security
{
    public class DefaultEncryptionServiceTest : TestFixtureBase<EncryptionServiceContext>
    {
        [Fact]
        public void EncodeAndDecodeTest()
        {
            var service = base.FixtureContext.EncryptionService;
            var input = new byte[] {128, 64, 32, 16, 8, 4, 2, 1};
            var encoded = service.Encode(input);
            var decoded = service.Decode(encoded);

            Assert.Equal(input.AsEnumerable(), decoded.AsEnumerable());
        }

        [Fact]
        public void HashTest()
        {
            var service = base.FixtureContext.EncryptionService;
            var input = new byte[] { 128, 64, 32, 16, 8, 4, 2, 1 };
            var hashed1 = service.Hash(input);
            var hashed2 = service.Hash(input);

            Assert.Equal(hashed1.AsEnumerable(), hashed2.AsEnumerable());
        }
    }
}
