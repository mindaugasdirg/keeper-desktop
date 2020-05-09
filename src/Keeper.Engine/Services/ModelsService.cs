using Keeper.Engine.Models;
using System;

namespace Keeper.Engine.Services
{
    public abstract class ModelsService<T> where T : Model
    {
        internal abstract int Add(T obj);
        internal abstract int Remove(T id);
        internal abstract int Update(T obj);

        internal int HandleTransaction(DataTransaction.Type type, T obj)
        {
            switch(type)
            {
                case DataTransaction.Type.Create:
                    return Add(obj);
                case DataTransaction.Type.Delete:
                    return Remove(obj);
                case DataTransaction.Type.Edit:
                    return Update(obj);
                default:
                    throw new ArgumentException("Unkown transaction type: " + Enum.GetName(typeof(DataTransaction.Type), type));
            }
        }
    }
}
