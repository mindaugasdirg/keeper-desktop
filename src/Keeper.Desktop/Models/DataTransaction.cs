using System;
using System.ComponentModel.DataAnnotations;

namespace Keeper.Desktop.Models
{
    [Serializable]
    public class DataTransaction
    {
        [Required]
        public ActionType Action { get; set; }
        [Required]
        public object Data { get; set; }
        [Required]
        public DataType Type { get; set; }

        public static DataType MatchType<T>(T item)
        {
            switch(item)
            {
                case Account a:
                    return DataType.Account;
                case Activity a:
                    return DataType.Activity;
                case Category c:
                    return DataType.Category;
                case TimeEntry t:
                    return DataType.TimeEntry;
                case Transaction t:
                    return DataType.Transaction;
                default:
                    throw new Exception("Provided type does not match DataType");
            }
        }

        public enum ActionType
        {
            Create,
            Delete,
            Edit,
            Reset
        }

        public enum DataType
        {
            Account,
            Activity,
            Category,
            TimeEntry,
            Transaction
        }
    }
}
