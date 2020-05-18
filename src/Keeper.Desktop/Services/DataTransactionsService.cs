using Keeper.Desktop.Models;
using System;
using System.Threading.Tasks;

namespace Keeper.Desktop.Services
{
    public class DataTransactionsService
    {
        private AccountsService accountsService;
        private ActivitiesService activitiesService;
        private CategoriesService categoriesService;
        private TimeEntriesService timeEntriesService;
        private TransactionsService transactionsService;
        private NetworkService networkService;

        public DataTransactionsService(AccountsService _accountsService, ActivitiesService _activitiesService, CategoriesService _categoriesService,
                                        TimeEntriesService _timeEntriesService, TransactionsService _transactionsService, NetworkService _networkService)
        {
            accountsService = _accountsService;
            activitiesService = _activitiesService;
            categoriesService = _categoriesService;
            timeEntriesService = _timeEntriesService;
            transactionsService = _transactionsService;
            networkService = _networkService;
        }

        public async Task<int> AddTransaction(DataTransaction dataTransaction)
        {
            var result = HandleDataTransaction(dataTransaction);
            if (!await networkService.Add(dataTransaction))
                return -1;
            return result;
        }

        public async Task Synchronize()
        {
            var transactions = await networkService.GetFromLastSynched();
            foreach(var transaction in transactions)
            {
                HandleDataTransaction(transaction);
            }
        }

        public int HandleDataTransaction(DataTransaction dataTransaction)
        {
            if (dataTransaction is null)
                return -1;

            switch(dataTransaction.Data)
            {
                case Account a:
                    return accountsService.HandleTransaction(dataTransaction.Action, a);
                case Activity a:
                    return activitiesService.HandleTransaction(dataTransaction.Action, a);
                case Category c:
                    return categoriesService.HandleTransaction(dataTransaction.Action, c);
                case TimeEntry t:
                    return timeEntriesService.HandleTransaction(dataTransaction.Action, t);
                case Transaction t:
                    return transactionsService.HandleTransaction(dataTransaction.Action, t);
                default:
                    throw new ArgumentException("Unkown data type " + dataTransaction.Data.GetType().Name);
            }
        }

        public async Task SendAll()
        {
            await SyncTable(accountsService);
            await SyncTable(activitiesService);
            await SyncTable(categoriesService);
            await SyncTable(timeEntriesService);
            await SyncTable(transactionsService);
        }

        private async Task SyncTable<T>(ModelsService<T> service) where T : class
        {
            foreach(var item in service.GetAll())
            {
                await networkService.Add(new DataTransaction()
                {
                    Action = DataTransaction.ActionType.Create,
                    Data = item,
                    Type = DataTransaction.MatchType(item)
                });
            }
        }


        public void ClearData()
        {
            foreach(var resetObj in new object[] { new Transaction(), new TimeEntry(), new Account(), new Account(), new Category() })
            {
                HandleDataTransaction(new DataTransaction()
                {
                    Action = DataTransaction.ActionType.Reset,
                    Data = resetObj
                });
            }
        }
    }
}
