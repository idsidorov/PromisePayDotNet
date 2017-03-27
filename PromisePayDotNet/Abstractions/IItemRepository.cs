using PromisePayDotNet.DTO;
using System.Collections.Generic;

namespace PromisePayDotNet.Abstractions
{
    public interface IItemRepository
    {

        IEnumerable<Item> ListItems(int limit = 10, int offset = 0);

        Item GetItemById(string itemId);

        Item CreateItem(Item item);

        bool DeleteItem(string itemId);

        Item UpdateItem(Item item);

        IEnumerable<Transaction> ListTransactionsForItem(string itemId);

        ItemStatus GetStatusForItem(string itemId);

        IEnumerable<Fee> ListFeesForItem(string itemId);

        User GetBuyerForItem(string itemId);

        User GetSellerForItem(string itemId);

        WireDetails GetWireDetailsForItem(string itemId);

        BPayDetails GetBPayDetailsForItem(string itemId);

        //actions methods start here

        Item MakePayment(string itemId, string accountId);

        Item RequestPayment(string itemId);

        Item ReleasePayment(string itemId, int releaseAmount);

        Item RequestRelease(string itemId, int releaseAmount);

        Item Cancel(string itemId);

        Item AcknowledgeWire(string itemId);

        Item AcknowledgePayPal(string itemId);

        Item RevertWire(string itemId);

        Item RequestRefund(string itemId, string refundAmount, string refundMessage);

        Item Refund(string itemId, string refundAmount, string refundMessage);

    }
}
