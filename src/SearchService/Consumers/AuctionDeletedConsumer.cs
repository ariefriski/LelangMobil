using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

public class AuctionDeletedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;
    public AuctionDeletedConsumer(IMapper mapper){
      _mapper =mapper;
    }
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine("--> Consuming AuctionDeleted : " + context.Message.Id);

        var result = await DB.DeleteAsync<Item>(context.Message.Id);

        if(!result.IsAcknowledged)
          throw new MessageException(typeof(AuctionDeleted),"Masalah delete mongodb");
    }
}