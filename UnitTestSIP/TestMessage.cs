using Microsoft.VisualStudio.TestTools.UnitTesting;

using DigitalPlatform.SIP2;
using DigitalPlatform.SIP2.Response;
using DigitalPlatform.SIP2.Request;

namespace UnitTestSIP
{
    /// <summary>
    /// 测试通讯包的打包、解包
    /// </summary>
    [TestClass]
    public class TestMessage
    {

        #region Login 93,94

        //Login, Message 93 
        [TestMethod]
        public void Test_93_1()
        {
            string text = "9300CNzizhu_test@instance5|CO1234567|CPzizhu_test@instance5|AY1AZEAE5";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as Login_93;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        //Login, Message 93 
        [TestMethod]
        public void Test_93_2()
        {
            string text = "9300CNLoginUserID|COLoginPassword|CPLocationCode|AY5AZEC78";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as Login_93;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        //Login Response, Message 94 
        [TestMethod]
        public void Test_94()
        {
            string text = "941AY3AZFDFA";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as LoginResponse_94;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        #endregion

        #region SC Status 99,ACS Status 98

        //SC Status, Message 99 
        [TestMethod]
        public void Test_99()
        {
            string text = "9900401.00AY1AZFCA5";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as SCStatus_99;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        //ACS Status, Message 98 
        // 这个例子是从SIP2 Developers Guide.pdf拷下来，里面没有BX,
        // 但在SIP2.0.pdf里指明是必备字段，为了兼容以前的数据，所以在设置命令结构时把该字段改为可选字段了。
        [TestMethod]
        public void Test_98_1()
        {
            string text = "98YYYNYN01000319960214    1447001.00AOID_21|AMCentral Library|ANtty30|AFScreen Message|AGPrint Message|AY1AZDA74";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as ACSStatus_98;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        //ACS Status, Message 98 
        // 这个例子有BXYYYYYYYYYYYYYYYY
        [TestMethod]
        public void Test_98_2()
        {
            string text = "98YYYYYN01000320170623    1537532.00AOdp2Library|AMdp2Library|BXYYYYYYYYYYYYYYYY|AF连接成功!|AY4AZ3EFC";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as ACSStatus_98;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        #endregion

        #region ACS Resend 97,SC Resend 96

        //Request ACS Resend, Message 97 
        [TestMethod]
        public void Test_97()
        {
            string text = "97";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as RequestACSResend_97;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        //Request SC Resend, Message 96 
        [TestMethod]
        public void Test_96()
        {
            string text = "96";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as RequestSCResend_96;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        #endregion

        #region Patron Status 23,24

        //Patron Status Request, Message 23 
        [TestMethod]
        public void Test_23()
        {
            string text = "2300119960212    100239AOid_21|AA104000000105|AC|AD|AY2AZF400";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as PatronStatusRequest_23;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        //Patron Status Response, Message 24 
        [TestMethod]
        public void Test_24()
        {
            string text = "24              00119960212    100239AO|AA104000000105|AEJohn Doe|AFScreen Message|AGCheck Print message|AY2AZDFC4";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as PatronStatusResponse_24;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }
        #endregion

        #region Patron Information Message 63,64

        //Patron Information Message 63
        [TestMethod]
        public void Test_63()
        {
            string text = "6300119980723    091905Y         AOInstitutionID|AAPatronID|BP00001|BQ00005|AY1AZEA83";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as PatronInformation_63;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

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

            Assert.AreEqual(2, response64.AU_ChargedItems_o.Count);// 检查解析出来的AU数量是否正确
        }

        [TestMethod]
        // 重复的AU
        public void Test_64_1()
        {
            string text = "64              00119980723    104009000100000002000100020000AOInstitutionID for PatronID|AAPatronID|AEPatron Name|BZ0002|CA0003|CB0010|BLY|ASItemID1 for PatronID |AUChargeItem1|AUChargeItem2";//"64              00119980723    104009000100000002000100020000AOInstitutionID for PatronID|AAPatronID|AEPatron Name|BZ0002|CA0003|CB0010|BLY|ASItemID1 for PatronID |AUChargeItem1|AUChargeItem2|BDHome Address|BEE Mail Address|BFHome Phone for PatronID|AFScreenMessage 0 for PatronID, Language 1|AFScreen Message 1 for PatronID, Language 1|AFScreen Message 2 for PatronID, Language 1|AGPrint Line 0 for PatronID, Language 1|AGPrint Line 1 for PatronID, Language 1|AGPrint Line 2 for PatronID, language 1|AY4AZ608F";
                                                                                                                                                                                                                            // return:
                                                                                                                                                                                                                            //      -1  出错
                                                                                                                                                                                                                            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);

            Assert.AreEqual(0, nRet);

            var request = message as PatronInformationResponse_64;
            Assert.IsTrue(request != null);

            Assert.AreEqual(2, request.AU_ChargedItems_o.Count);// 检查解析出来的AU数量是否正确

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        [TestMethod]
        // 重复AU，AF，AG
        public void Test_64_2()
        {
            string text = "64              00119980723    104009000100000002000100020000AOInstitutionID for PatronID|AAPatronID|AEPatron Name|BZ0002|CA0003|CB0010|BLY|ASItemID1 for PatronID |AUChargeItem1|AUChargeItem2|BDHome Address|BEE Mail Address|BFHome Phone for PatronID|AFScreenMessage 0 for PatronID, Language 1|AFScreen Message 1 for PatronID, Language 1|AFScreen Message 2 for PatronID, Language 1|AGPrint Line 0 for PatronID, Language 1|AGPrint Line 1 for PatronID, Language 1|AGPrint Line 2 for PatronID, language 1|AY4AZ608F";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);

            Assert.AreEqual(0, nRet);

            var request = message as PatronInformationResponse_64;
            Assert.IsTrue(request != null);

            Assert.AreEqual(2, request.AU_ChargedItems_o.Count);// 检查解析出来的AU数量是否正确

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        #endregion

        #region Patron Enable, Message 25,26

        //Patron Enable, Message 25 
        [TestMethod]
        public void Test_25()
        {
            string text = "2519980723    094240AOCertification Institute ID|AAPatronID|AY4AZEBF1";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as PatronEnable_25;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        //Patron Enable Response, Message 26
        [TestMethod]
        public void Test_26()
        {
            string text = "26              00119980723    111413AOInstitutionID for PatronID|AAPatronID|AEPatron Name|BLY|AFScreenMessage 0 for PatronID, Language 1|AFScreen Message 1 for PatronID, Language 1 |AGPrint Line 0 for PatronID, Language 1|AY7AZ8EA6";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as PatronEnableResponse_26;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }
        #endregion

        #region Item Information, Message 17,18

        //Item Information, Message 17
        [TestMethod]
        public void Test_17()
        {
            string text = "1719980723    100000AOCertification Institute ID|ABItemBook|AY1AZEBEB";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as ItemInformation_17;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        //Item Information Response, Message 18 
        [TestMethod]
        public void Test_18()
        {
            string text = "1808000119980723    115615CF00000|ABItemBook|AJTitle For Item Book|CK003|AQPermanent location|APCurrent Location|CHFree-form text|AY0AZC05B";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as ItemInformationResponse_18;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }
        #endregion

        #region Checkout, Message 11,12

        //Checkout, Message 11 
        [TestMethod]
        public void Test_11()
        {
            string text = "11YN19960212   10051419960212   100514AO|AA104000000105|AB000000000005792|AC|AY3AZEDB7";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as Checkout_11;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }

        //Checkout Response，Message 12
        [TestMethod]
        public void Test_12()
        {
            string text = "121NNY20170630    141540AOdp2Library|AAL905071|AB549096|AJ川味家常菜|AH2017-10-28|AF图书【川味家常菜】借阅成功！应还日期：2017-10-28|AG读者【L905071】借书 川味家常菜【川味家常菜】成功，应还日期：2017-10-28|AY4AZF331";
            // return:
            //      -1  出错
            //      0   成功
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // 检查返回值

            var request = message as CheckoutResponse_12;
            Assert.IsTrue(request != null); // 检查解析对象

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // 检查原始字符串和转成对象后输出字符串是否一致
        }
        #endregion
    }
}
