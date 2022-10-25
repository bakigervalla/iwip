using AutoMapper;
using iwip.PO;

namespace iwip;

public class iwipApplicationAutoMapperProfile : Profile
{
    public iwipApplicationAutoMapperProfile()
    {
        CreateMap<PurchaseOrder, PurchaseOrderDto>();
    }
}
