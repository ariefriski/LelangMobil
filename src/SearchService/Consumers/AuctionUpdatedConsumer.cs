using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

public class AuctionUpdatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;
    public AuctionUpdatedConsumer(IMapper mapper){
      _mapper =mapper;
    }
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine("--> Consuming AuctionUpdated : " + context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);

        var result = await DB.Update<Item>()
        .Match(a => a.ID == context.Message.Id.ToString())
        .ModifyOnly(x=>new{
          x.Color,
          x.Make,
          x.Model,
          x.Year,
          x.Mileage
        },item).ExecuteAsync();  

        
        if(!result.IsAcknowledged)
          throw new MessageException(typeof(AuctionUpdated),"Masalah update mongodb");
    }
}