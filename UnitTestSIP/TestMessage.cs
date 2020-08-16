using Microsoft.VisualStudio.TestTools.UnitTesting;

using DigitalPlatform.SIP2;
using DigitalPlatform.SIP2.Response;
using DigitalPlatform.SIP2.Request;

namespace UnitTestSIP
{
    /// <summary>
    /// ����ͨѶ���Ĵ�������
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
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as Login_93;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //Login, Message 93 
        [TestMethod]
        public void Test_93_2()
        {
            string text = "9300CNLoginUserID|COLoginPassword|CPLocationCode|AY5AZEC78";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as Login_93;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //Login Response, Message 94 
        [TestMethod]
        public void Test_94()
        {
            string text = "941AY3AZFDFA";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as LoginResponse_94;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        #endregion

        #region SC Status 99,ACS Status 98

        //SC Status, Message 99 
        [TestMethod]
        public void Test_99()
        {
            string text = "9900401.00AY1AZFCA5";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as SCStatus_99;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //ACS Status, Message 98 
        // ��������Ǵ�SIP2 Developers Guide.pdf������������û��BX,
        // ����SIP2.0.pdf��ָ���Ǳر��ֶΣ�Ϊ�˼�����ǰ�����ݣ���������������ṹʱ�Ѹ��ֶθ�Ϊ��ѡ�ֶ��ˡ�
        [TestMethod]
        public void Test_98_1()
        {
            string text = "98YYYNYN01000319960214    1447001.00AOID_21|AMCentral Library|ANtty30|AFScreen Message|AGPrint Message|AY1AZDA74";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as ACSStatus_98;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //ACS Status, Message 98 
        // ���������BXYYYYYYYYYYYYYYYY
        [TestMethod]
        public void Test_98_2()
        {
            string text = "98YYYYYN01000320170623    1537532.00AOdp2Library|AMdp2Library|BXYYYYYYYYYYYYYYYY|AF���ӳɹ�!|AY4AZ3EFC";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as ACSStatus_98;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        #endregion

        #region ACS Resend 97,SC Resend 96

        //Request ACS Resend, Message 97 
        [TestMethod]
        public void Test_97()
        {
            string text = "97";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as RequestACSResend_97;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //Request SC Resend, Message 96 
        [TestMethod]
        public void Test_96()
        {
            string text = "96";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as RequestSCResend_96;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        #endregion

        #region Patron Status 23,24

        //Patron Status Request, Message 23 
        [TestMethod]
        public void Test_23()
        {
            string text = "2300119960212    100239AOid_21|AA104000000105|AC|AD|AY2AZF400";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as PatronStatusRequest_23;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //Patron Status Response, Message 24 
        [TestMethod]
        public void Test_24()
        {
            string text = "24              00119960212    100239AO|AA104000000105|AEJohn Doe|AFScreen Message|AGCheck Print message|AY2AZDFC4";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as PatronStatusResponse_24;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }
        #endregion

        #region Patron Information Message 63,64

        //Patron Information Message 63
        [TestMethod]
        public void Test_63()
        {
            string text = "6300119980723    091905Y         AOInstitutionID|AAPatronID|BP00001|BQ00005|AY1AZEA83";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as PatronInformation_63;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        [TestMethod]
        public void Test_parse_patronInfoResponse()
        {
            string text = "64              01920200810    224914000000000002000000000000AOdp2Library|AAR0000001|AE����|BZ0008|CB10|BLY|CQN|AF���ڱ������ɽ衾10���ᣬ�������ٽ衾8���ᡣ|AG���ڱ������ɽ衾10���ᣬ�������ٽ衾8���ᡣ|AU0000001|AUT0000131|AY4AZ2A96";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);

            Assert.AreEqual(0, nRet);

            var response64 = message as PatronInformationResponse_64;
            Assert.IsTrue(response64 != null);

            Assert.AreEqual(2, response64.AU_ChargedItems_o.Count);// ������������AU�����Ƿ���ȷ
        }

        [TestMethod]
        // �ظ���AU
        public void Test_64_1()
        {
            string text = "64              00119980723    104009000100000002000100020000AOInstitutionID for PatronID|AAPatronID|AEPatron Name|BZ0002|CA0003|CB0010|BLY|ASItemID1 for PatronID |AUChargeItem1|AUChargeItem2";//"64              00119980723    104009000100000002000100020000AOInstitutionID for PatronID|AAPatronID|AEPatron Name|BZ0002|CA0003|CB0010|BLY|ASItemID1 for PatronID |AUChargeItem1|AUChargeItem2|BDHome Address|BEE Mail Address|BFHome Phone for PatronID|AFScreenMessage 0 for PatronID, Language 1|AFScreen Message 1 for PatronID, Language 1|AFScreen Message 2 for PatronID, Language 1|AGPrint Line 0 for PatronID, Language 1|AGPrint Line 1 for PatronID, Language 1|AGPrint Line 2 for PatronID, language 1|AY4AZ608F";
                                                                                                                                                                                                                            // return:
                                                                                                                                                                                                                            //      -1  ����
                                                                                                                                                                                                                            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);

            Assert.AreEqual(0, nRet);

            var request = message as PatronInformationResponse_64;
            Assert.IsTrue(request != null);

            Assert.AreEqual(2, request.AU_ChargedItems_o.Count);// ������������AU�����Ƿ���ȷ

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        [TestMethod]
        // �ظ�AU��AF��AG
        public void Test_64_2()
        {
            string text = "64              00119980723    104009000100000002000100020000AOInstitutionID for PatronID|AAPatronID|AEPatron Name|BZ0002|CA0003|CB0010|BLY|ASItemID1 for PatronID |AUChargeItem1|AUChargeItem2|BDHome Address|BEE Mail Address|BFHome Phone for PatronID|AFScreenMessage 0 for PatronID, Language 1|AFScreen Message 1 for PatronID, Language 1|AFScreen Message 2 for PatronID, Language 1|AGPrint Line 0 for PatronID, Language 1|AGPrint Line 1 for PatronID, Language 1|AGPrint Line 2 for PatronID, language 1|AY4AZ608F";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);

            Assert.AreEqual(0, nRet);

            var request = message as PatronInformationResponse_64;
            Assert.IsTrue(request != null);

            Assert.AreEqual(2, request.AU_ChargedItems_o.Count);// ������������AU�����Ƿ���ȷ

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        #endregion

        #region Patron Enable, Message 25,26

        //Patron Enable, Message 25 
        [TestMethod]
        public void Test_25()
        {
            string text = "2519980723    094240AOCertification Institute ID|AAPatronID|AY4AZEBF1";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as PatronEnable_25;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //Patron Enable Response, Message 26
        [TestMethod]
        public void Test_26()
        {
            string text = "26              00119980723    111413AOInstitutionID for PatronID|AAPatronID|AEPatron Name|BLY|AFScreenMessage 0 for PatronID, Language 1|AFScreen Message 1 for PatronID, Language 1 |AGPrint Line 0 for PatronID, Language 1|AY7AZ8EA6";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as PatronEnableResponse_26;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }
        #endregion

        #region Item Information, Message 17,18

        //Item Information, Message 17
        [TestMethod]
        public void Test_17()
        {
            string text = "1719980723    100000AOCertification Institute ID|ABItemBook|AY1AZEBEB";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as ItemInformation_17;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //Item Information Response, Message 18 
        [TestMethod]
        public void Test_18()
        {
            string text = "1808000119980723    115615CF00000|ABItemBook|AJTitle For Item Book|CK003|AQPermanent location|APCurrent Location|CHFree-form text|AY0AZC05B";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as ItemInformationResponse_18;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }
        #endregion

        #region Checkout, Message 11,12

        //Checkout, Message 11 
        [TestMethod]
        public void Test_11()
        {
            string text = "11YN19960212    10051419960212    100514AO|AA104000000105|AB000000000005792|AC|AY3AZEDB7";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as Checkout_11;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //Checkout Response��Message 12
        [TestMethod]
        public void Test_12()
        {
            string text = "121NNY20170630    141540AOdp2Library|AAL905071|AB549096|AJ��ζ�ҳ���|AH2017-10-28|AFͼ�顾��ζ�ҳ��ˡ����ĳɹ���Ӧ�����ڣ�2017-10-28|AG���ߡ�L905071������ ��ζ�ҳ��ˡ���ζ�ҳ��ˡ��ɹ���Ӧ�����ڣ�2017-10-28|AY4AZF331";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as CheckoutResponse_12;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }
        #endregion

        #region Checkin, Message 09,10

        //Checkin, Message 09 
        [TestMethod]
        public void Test_09()
        {
            string text = "09N19980821    085721                  APCertification Terminal Location|AOCertification Institute ID|ABCheckInBook|ACTPWord|BIN|AY2AZD6A5";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as Checkin_09;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //Checkin Response,Message 10
        [TestMethod]
        public void Test_10_1()
        {
            string text = "101YNN20170630    141632AOdp2Library|AB510102|AQ�ۺ�ͼ���|AJ�������������͡�|AAL905071|CK001|CHProperties|CLsort bin A1|AFͼ�顶�����������͡����ش���ɹ���2017-06-30|AGͼ�顶�����������͡�-- 510102����2017-06-30�黹��|AY4AZ4E37";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as CheckinResponse_10;
            Assert.IsTrue(request != null); // ����������

            // 2020/8/16ע��������Ӱ�CL������AA�˺��棬��SIP2.0�У�CL�ֶ�����AA֮ǰ��
            // �����������������ַ�����ԭʼ�ַ������ֶ�λ���в�𣬵�Ҳ����ȷ�ģ����Ը�Ϊ�ж��ַ��������Ƿ�
            string retText = request.ToText();
            Assert.AreEqual(text.Length, retText.Length); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        [TestMethod]
        public void Test_10_2()
        {
            string text = "101YNN20170630    141632AOdp2Library|AB510102|AQ�ۺ�ͼ���|AJ�������������͡�|CLsort bin A1|AAL905071|CK001|CHProperties|AFͼ�顶�����������͡����ش���ɹ���2017-06-30|AGͼ�顶�����������͡�-- 510102����2017-06-30�黹��|AY4AZ4E37";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as CheckinResponse_10;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }
        #endregion

        #region Fee Paid Message 37,38

        //Fee Paid Message 37 
        [TestMethod]
        public void Test_37()
        {
            string text = "3719980723    0932110401USDBV111.11|AOCertification Institute ID|AAPatronID|BKTransactionID|AY2AZE1EF";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as FeePaid_37;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //Fee Paid Response,Message 38
        [TestMethod]
        public void Test_38()
        {
            string text = "38Y19980723    111035AOInstitutionID for PatronID|AAPatronID|AFScreenMessage 0 for PatronID, Language 1|AGPrint Line 0 for PatronID, Language 1|AY6AZ9716";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as FeePaidResponse_38;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }
        #endregion

        #region End Patron Session, Message 35,36

        //End Patron Session, Message 35 
        [TestMethod]
        public void Test_35()
        {
            string text = "3519980723    094014AOCertification Institute ID|AAPatronID|AY3AZEBF2";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as EndPatronSession_35;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //End Session Response, Message 36
        [TestMethod]
        public void Test_36()
        {
            string text = "36Y19980723    110658AOInstitutionID for PatronID|AAPatronID|AFScreenMessage 0 for PatronID, Language 1|AFScreen Message 1 for PatronID, Language 1|AFScreen Message 2 for PatronID, Language 1|AGPrint Line 0 for PatronID, Language 1|AGPrint Line 1 for PatronID, Language 1|AGPrint Line 2 for PatronID, language 1|AY5AZ970F";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as EndSessionResponse_36;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }
        #endregion

        #region Block Patron, Message 01 

        //Checkout, Message 11 
        [TestMethod]
        public void Test_01()
        {
            string text = "01N19960213    162352AO|ALCARD BLOCK TEST|AA104000000705|AC|AY2AZF02F";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as BlockPatron_01;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        #endregion

        #region Renew 29,30

        //Renew 29
        [TestMethod]
        public void Test_29()
        {
            string text = "29NN20170630    144419                  AOdp2Library|AAL905071|AB510105|BON";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as Renew_29;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //Renew Response 30
        [TestMethod]
        public void Test_30_1()
        {
            string text = "300YNN20170630    144410AOdp2Library|AA|AB510105|AJ|AH|AFͼ�顾510105������ʧ�ܣ�|AG���ߡ�������ͼ�顾510105��ʧ�ܣ�����������ܾ������߱� renew Ȩ�ޡ� �� zh-CN ��û���ҵ���Ӧ����Դ��|AY4AZ3CAD";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as RenewResponse_30;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //Renew Response 30
        [TestMethod]
        public void Test_30_2()
        {
            string text = "301YNN20170630    154932AOdp2Library|AAL905071|AB510104|AJ������������|AH2017-08-29|AFͼ�顾�����������͡����账��ɹ���Ӧ�����ڣ�2017-08-29|AG���ߡ�L905071�����账��ɹ���Ӧ�����ڣ�2017-08-29|AY4AZD173";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as RenewResponse_30;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }


        #endregion

        #region Renew All 65,66

        //Renew All 65
        [TestMethod]
        public void Test_65()
        {
            //65	18-char	AO	AA	AD	AC	BO
            string text = "6520170630    144419AOdp2Library|AAL905071|AD|AC|BON";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as RenewAll_65;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //Renew All Response 66
        [TestMethod]
        public void Test_66()
        {
            //66	1-char	4-char	4-char	18-char	AO	BM	BN	AF	AG
            string text = "6601234123420170630    144410AOdp2Library|BM001|BN002|BN003|AF001�й�|AF002����|AF003�¹�|AGtest|AY4AZ3CAD";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as RenewAllResponse_66;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        #endregion

        #region Hold 15,16

        //Hold 15
        [TestMethod]
        public void Test_15()
        {
            //15 1-char 18-char BW BS BY AO AA AD AB AJ AC BO
            string text = "15+20170630    144419BW20170630    144419|BS|BY|AOdp2Library|AAL905071|AD|AB|AJ|AC|BON";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as Hold_15;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        //Hold Response 16
        [TestMethod]
        public void Test_16()
        {
            //16  1-char  1-char  18-char  BW  BR  BS  AO  AA  AB AJ  AF  AG
            string text = "160120170630    144410BW20170630    144410|BR|BS|AOdp2Library|AA001|AB|AJ001�й�|AF002����|AF003�¹�|AGtest|AY4AZ3CAD";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as HoldResponse_16;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        #endregion

        #region Item Status Update 19,20

        // Item Status Update 19
        [TestMethod]
        public void Test_19()
        {
            //19	18-char	AO	AB	AC	CH
            string text = "1920170630    144419AOdp2Library|AB905071|CH";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as ItemStatusUpdate_19;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        // Item Status Update Response 20
        [TestMethod]
        public void Test_20()
        {
            //20	1-char	18-char	AB	AJ	CH	AF	AG
            string text = "20020170630    144410AB001|AJ001�й�|CH|AF002����|AGtest|AY4AZ3CAD";
            // return:
            //      -1  ����
            //      0   �ɹ�
            int nRet = SIPUtility.ParseMessage(text,
                out BaseMessage message,
                out string error);
            Assert.AreEqual(0, nRet); // ��鷵��ֵ

            var request = message as ItemStatusUpdateResponse_20;
            Assert.IsTrue(request != null); // ����������

            string retText = request.ToText();
            Assert.AreEqual(text, retText); // ���ԭʼ�ַ�����ת�ɶ��������ַ����Ƿ�һ��
        }

        #endregion
    }
}
