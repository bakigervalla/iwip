using AutoMapper;
using iwip.PO;

namespace iwip;

public class iwipApplicationAutoMapperProfile : Profile
{
    public iwipApplicationAutoMapperProfile()
    {
        CreateMap<PurchaseOrder, PurchaseOrderDto>();
        CreateMap<POLine, POLineDto>();
        CreateMap<Shipping, ShippingDto>().ReverseMap();
        CreateMap<ShippingDocument, ShippingDocumentDto>().ReverseMap();

        CreateMap<PurchaseOrderDto, CreateUpdatePODto>();
    }

}