using AutoMapper;
using AutoMapper.Internal.Mappers;
using iwip.PO;
using System.Collections.Generic;

namespace iwip;

public class iwipApplicationAutoMapperProfile : Profile
{
    public iwipApplicationAutoMapperProfile()
    {
        CreateMap<PurchaseOrder, PurchaseOrderDto>().ReverseMap();
        CreateMap<POLine, POLineDto>().ReverseMap();
        
        CreateMap<Shipping, ShippingDto>().ReverseMap();
        CreateMap<ShippingDocument, ShippingDocumentDto>().ReverseMap();

        /*CreateMap<PurchaseOrderDto, PurchaseOrder>();*/

        CreateMap<PurchaseOrderDto, CreateUpdatePODto>();
    }

}