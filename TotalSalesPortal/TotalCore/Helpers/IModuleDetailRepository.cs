using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TotalModel.Models;

namespace TotalCore.Helpers
{
    public interface IModuleDetailRepository
    {
        IQueryable<ModuleDetail> GetAllModuleDetails();
        IQueryable<ModuleDetail> GetModuleDetailByModuleID(int moduleID);
        ModuleDetail GetModuleDetailByID(int moduleDetailID);

        void AddModuleDetail(ModuleDetail moduleDetail);

        void Add(ModuleDetail moduleDetail);

        void Remove(ModuleDetail recordToDelete);
        void SaveChanges();
    }
}
