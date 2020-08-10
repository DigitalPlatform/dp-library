using Microsoft.VisualStudio.TestTools.UnitTesting;

using DigitalPlatform.SIP2;
using DigitalPlatform.SIP2.Response;

namespace UnitTestSIP
{
    /// <summary>
    /// 测试通讯包的打包、解包
    /// </summary>
    [TestClass]
    public class TestMessage
    {
        [TestMethod]
        public void Test_parse_patronInfoResponse()
        {
            string text = "64              01920200810    224914000000000002000000000000AOdp2Library|AAR0000001|AE张三|BZ0008|CB10|BLY|CQN|AF您在本馆最多可借【10】册，还可以再借【8】册。|AG您在本馆最多可借【10】册，还可以再借【8】册。|AU0000001|AUT0000131|AY4AZ2A96";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);

            Assert.AreEqual(0, nRet);

            var response64 = message as PatronInformationResponse_64;
            Assert.IsTrue(response64 != null);

            Assert.AreEqual(2, response64.AS_HoldItems_o.Count);
        }
    }
}
