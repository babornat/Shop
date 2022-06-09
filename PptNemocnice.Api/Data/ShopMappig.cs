using AutoMapper;
using Shop.Shared;

namespace Shop.Api.Data;

public class ShopMappig:Profile
{

    public ShopMappig()
    {
        CreateMap<Vybaveni, VybaveniModel>().ReverseMap();
        CreateMap<Vybaveni, VybaveniSRevizemaModel>();
        CreateMap<Revize, RevizeModel>().ReverseMap();
    }

}
