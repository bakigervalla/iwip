using AutoMapper;
using iwip.PO;

namespace iwip;

public class iwipApplicationAutoMapperProfile : Profile
{
    public iwipApplicationAutoMapperProfile()
    {
        CreateMap<PurchaseOrder, PurchaseOrderDto>();
        CreateMap<POLine, POLineDto>();
        CreateMap<Shipping, ShippingDto>();
        CreateMap<ShippingDocumentDto, ShippingDocumentDto>();

        CreateMap<PurchaseOrderDto, CreateUpdatePODto>();
    }
}
