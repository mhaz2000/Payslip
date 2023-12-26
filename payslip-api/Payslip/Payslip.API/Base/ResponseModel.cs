namespace Payslip.API.Base
{
    public class ResponseModel
    {
        public ResponseModel(int total, object data)
        {
            Total = total;
            Data = data;
        }

        public int Total { get; set; }
        public object Data { get; set; }
    }
}
