using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalPlatform
{
    [Serializable()]
    public class NormalResult
    {
        public int Value { get; set; }
        public string ErrorInfo { get; set; }
        public string ErrorCode { get; set; }

        public NormalResult(NormalResult result)
        {
            this.Value = result.Value;
            this.ErrorInfo = result.ErrorInfo;
            this.ErrorCode = result.ErrorCode;
        }

        public NormalResult(int value, string error)
        {
            this.Value = value;
            this.ErrorInfo = error;
        }

        public NormalResult()
        {

        }

        public override string ToString()
        {
            return $"Value={Value},ErrorInfo={ErrorInfo},ErrorCode={ErrorCode}";
        }
    }

    public class TextResult : NormalResult
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return $"Value={Value},Text={Text},ErrorInfo={ErrorInfo},ErrorCode={ErrorCode}";
        }

    }

}
