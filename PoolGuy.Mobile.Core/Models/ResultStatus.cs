using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.Data.Models
{
    public class ResultStatus<T>
    {
        public ResultStatus(eResultStatus status, string message, T entity)
        {
            this.Status = status;
            this.Message = message;
            this.Entity = entity;
        }

        public eResultStatus Status { get; set; }
        public string Message { get; set; }
        public T Entity { get; set; }
    }
}