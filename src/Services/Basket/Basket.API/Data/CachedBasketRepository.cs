
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    //Questa classe funge da proxy (pattern) e inoltra le chiamate al repository sottostante. si è utilizzato anche il pattern decorator

    public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {        
        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);  // The IDistributedCache interface provides the following
                                                                                         // methods to manipulate items in the distributed cache implementation: 
                                                                                         // Get, GetAsync, Set, SetAsync, Refresh, RefreshAsync, Remove, RemoveAsync
            if (!string.IsNullOrEmpty(cachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

            var basket = await repository.GetBasket(userName, cancellationToken);
            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;              
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await repository.StoreBasket(basket, cancellationToken);
            await cache.SetStringAsync(basket.UserName!, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;

        }
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasket(userName, cancellationToken);
            await cache.RemoveAsync(userName, cancellationToken);

            return true;
        }
    }
}
