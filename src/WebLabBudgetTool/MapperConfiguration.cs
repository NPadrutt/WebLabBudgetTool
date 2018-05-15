using AutoMapper;
using WebLabBudgetTool.DataTransferObjects;
using WebLabBudgetTool.Entities;

namespace WebLabBudgetTool
{
    public class MapperConfiguration
    {
        public static void Init()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Account, AccountDto>();
                cfg.CreateMap<AccountDto, Account>();
            });
        }
    }
}
