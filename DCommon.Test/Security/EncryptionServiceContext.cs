using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCommon.Environment.Configuration;
using DCommon.Security;
using DCommon.Security.Providers;

namespace DCommon.Tests.Security
{
    public class EncryptionServiceContext
    {
        public IEncryptionService EncryptionService { get; set; }

        public EncryptionServiceContext()
        {
            var setting = new ShellSetting()
            {
                EncryptionAlgorithm = "AES",
                HashAlgorithm = "HMACSHA1",
                EncryptionKey = "9E982892E595E49E520B6A3B2238D7840D78CEA993D51C30F2EBCB01E8C459D5",
                HashKey = "27E9C12EE415321A85F62631BCDC16D6",
            };
            var settingManagerMock = new Moq.Mock<ISettingsManager>();
            settingManagerMock.Setup(m => m.LoadSetting()).Returns(setting);
            var encryptionService = new DefaultEncryptionService(settingManagerMock.Object);
            EncryptionService = encryptionService;
        }
    }
}
