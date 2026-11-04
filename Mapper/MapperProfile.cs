using AutoMapper;
using SystemBank.Models;
using SystemBank;
namespace SystemBank;
public class MapperFilter : Profile
{
    public MapperFilter()
    {
        CreateMap<Account,AccountDTO>();
    }
}