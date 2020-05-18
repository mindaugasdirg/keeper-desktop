using Keeper.Desktop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace Keeper.Desktop.Services
{
    public abstract class ModelsService<T> where T : class
    {
        protected DatabaseContext context;

        public ModelsService(DatabaseContext _context)
        {
            context = _context;
        }

        public abstract DbSet<T> GetDbSet(DatabaseContext context);

        public int Add(T obj)
        {
            if (obj is null)
                return -1;
            GetDbSet(context).Add(obj);
            return context.SaveChanges();
        }

        public int Remove(T obj)
        {
            if (obj is null)
                return -1;
            GetDbSet(context).Remove(obj);
            return context.SaveChanges();
        }

        public int Update(T obj)
        {
            if (obj is null)
                return -1;
            GetDbSet(context).Update(obj);
            return context.SaveChanges();
        }

        public virtual List<T> GetAll()
        {
            return GetDbSet(context).ToList();
        }

        public int ClearTable()
        {
            var rows = GetDbSet(context).ToList();
            GetDbSet(context).RemoveRange(rows);
            return context.SaveChanges();
        }

        public virtual int HandleTransaction(DataTransaction.ActionType type, T obj)
        {
            switch(type)
            {
                case DataTransaction.ActionType.Create:
                    return Add(obj);
                case DataTransaction.ActionType.Delete:
                    return Remove(obj);
                case DataTransaction.ActionType.Edit:
                    return Update(obj);
                case DataTransaction.ActionType.Reset:
                    return ClearTable();
                default:
                    throw new ArgumentException("Unkown transaction type: " + Enum.GetName(typeof(DataTransaction.ActionType), type));
            }
        }
    }
}
