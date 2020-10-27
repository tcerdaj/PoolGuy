using System;
using static PoolGuy.Mobile.Data.Models.Enums;

namespace PoolGuy.Mobile.Data.Models
{
    public class ResultStatus<T>
    {
        public ResultStatus(eResultStatus status, Exception exception, T entity)
        {
            this.Status = status;
            this.Exception = exception;
            this.Entity = entity;
        }

        public eResultStatus Status { get; set; }
        public Exception Exception { get; set; }
        public T Entity { get; set; }
    }
}