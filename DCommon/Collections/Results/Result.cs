using System.Runtime.Serialization;

namespace DCommon
{
    [DataContract]
    public class Result : IResult
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string ErrorCode { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}
