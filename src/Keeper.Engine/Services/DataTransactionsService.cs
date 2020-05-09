using Keeper.Engine.Models;
using System;

namespace Keeper.Engine.Services
{
    public class DataTransactionsService
    {
        private CategoriesService categoriesService;

        public DataTransactionsService(CategoriesService _categoriesService)
        {
            categoriesService = _categoriesService;
        }

        public int HandleDataTransaction(DataTransaction dataTransaction)
        {
            if (dataTransaction is null)
                return -1;

            switch(dataTransaction.Data)
            {
                case Category c:
                    return categoriesService.HandleTransaction(dataTransaction.Action, c);
                default:
                    throw new ArgumentException("Unkown data type " + dataTransaction.Data.GetType().Name);
            }
        }
    }
}
