namespace MiniTrello.Api.Models
{
    public class ReturnModel
    {
        public virtual string Code { get; set; }
        public virtual string Message { get; set; }

        public virtual ReturnModel ConfigureModel(string code, string message,ReturnModel model)
        {
            model.Code = code;
            model.Message = message;
            return model;
        }
    }
}