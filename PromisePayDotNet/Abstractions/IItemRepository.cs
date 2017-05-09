using PromisePayDotNet.DTO;
using PromisePayDotNet.Internals;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromisePayDotNet.Abstractions
{
    public interface IItemRepository
    {
        /// <summary>
        /// Retrieve an ordered and paginated list of existing Items.
        /// </summary>
        Task<IEnumerable<Item>> ListItemsAsync(int limit = 10, int offset = 0);
        /// <summary>
        /// Show details of a specific Item using a given :id.
        /// </summary>
        Task<Item> GetItemByIdAsync(string itemId);
        /// <summary>
        /// Create an Item. Items require two Users, a buyer and a seller.
        /// 
        /// The payment type can be one of Escrow, Express, Escrow Partial Release or Approve.
        /// 
        /// The buyer_id and seller_id are your unique user identifiers.
        /// </summary>
        Task<Item> CreateItemAsync(Item item);
        /// <summary>
        /// Delete an existing Item using a given :id.
        /// </summary>
        Task<bool> DeleteItemAsync(string itemId);
        /// <summary>
        /// Update an existing Items attributes using a given :id.
        /// </summary>
        Task<Item> UpdateItemAsync(Item item);
        /// <summary>
        /// Retrieve an ordered and paginated list of Transactions associated with the Item using a given :id.
        /// </summary>
        Task<IEnumerable<Transaction>> ListTransactionsForItemAsync(string itemId);
        /// <summary>
        /// Show the status of an Item using a given :id.
        /// </summary>
        Task<ItemStatus> GetStatusForItemAsync(string itemId);
        /// <summary>
        /// Show the Fees associated with the Item using a given :id.
        /// </summary>
        Task<IEnumerable<Fee>> ListFeesForItemAsync(string itemId);
        /// <summary>
        /// Show the buyer User associated with the Item using a given :id.
        /// </summary>
        Task<User> GetBuyerForItemAsync(string itemId);
        /// <summary>
        /// Show the seller User associated with the Item using a given :id.
        /// </summary>
        Task<User> GetSellerForItemAsync(string itemId);
        /// <summary>
        /// Show the Item wire payment details using a given :id.
        /// </summary>
        Task<WireDetails> GetWireDetailsForItemAsync(string itemId);
        /// <summary>
        /// Show the Item BPay payment details using a given :id.
        /// </summary>
        Task<BPayDetails> GetBPayDetailsForItemAsync(string itemId);

        //actions methods start here
        /// <summary>
        /// Make a payment for an Item. Pass the :account_id of a Bank Account or a Card Account associated with the Item’s buyer.
        /// 
        /// The Item state will transition to one of payment_held, payment_pending, payment_deposited for an Escrow or Escrow Partial Release payment type.
        /// 
        /// The Item state will transition to one of payment_held, payment_pending or completed for an Express or Approve payment type.
        /// </summary>
        Task<Item> MakePaymentAsync(string itemId, string accountId);
        /// <summary>
        /// Request payment for an Item. This can trigger an email or an SMS with instructions for payment. Contact support@assemblypayments.com if you require email or SMS notifications configured. This will transition the Item state to payment_required.
        /// </summary>
        Task<Item> RequestPaymentAsync(string itemId);
        /// <summary>
        /// Release funds held in escrow from an Item with an Escrow or Escrow Partial Release payment type. This will transition the Item state to completed.
        /// </summary>
        Task<Item> ReleasePaymentAsync(string itemId, int releaseAmount);
        /// <summary>
        /// Request release of funds held in escrow, from an Item with an Escrow or Escrow Partial Release payment type. This will transition the Item state to work_completed.
        /// </summary>
        Task<Item> RequestReleaseAsync(string itemId, int releaseAmount);
        /// <summary>
        /// Cancel an Item. This will transition the Item state to cancelled. Items can only be cancelled if they haven’t been actioned in any other way.
        /// </summary>
        Task<Item> CancelAsync(string itemId);
        /// <summary>
        /// Acknowledge that funds are being wired for payment. This will transition the Item state to wire_pending.
        /// </summary>
        Task<Item> AcknowledgeWireAsync(string itemId);
        /// <summary>
        /// 
        /// </summary>
        Task<Item> AcknowledgePayPalAsync(string itemId);
        /// <summary>
        /// Revert an acknowledge wire Item Action. This will transition the Item state to pending.
        /// </summary>
        Task<Item> RevertWireAsync(string itemId);
        /// <summary>
        /// Request a refund for an Item. This will transition the Item status to refund_flagged.
        /// </summary>
        Task<Item> RequestRefundAsync(string itemId, string refundAmount, string refundMessage);
        /// <summary>
        /// Refund an Item’s funds held in escrow. An partial amount can be specified otherwise the full amount will be refunded. This will transition the Item state to ‘refunded’ if the full amount is refunded, or to the previously held state if a partial amount is specified.
        /// </summary>
        Task<Item> RefundAsync(string itemId, string refundAmount, string refundMessage);
    }

    public static class ItemRepositoryExtensions
    {
        public static IEnumerable<Item> ListItems(this IItemRepository repo, int limit = 10, int offset = 0)=> repo.ListItemsAsync(offset: offset, limit:limit).WrapResult();
        public static Item GetItemById(this IItemRepository repo, string itemId) => repo.GetItemByIdAsync(itemId).WrapResult();
        public static Item CreateItem(this IItemRepository repo, Item item) => repo.CreateItemAsync(item).WrapResult();
        public static bool DeleteItem(this IItemRepository repo, string itemId) => repo.DeleteItemAsync(itemId).WrapResult();
        public static Item UpdateItem(this IItemRepository repo, Item item) => repo.UpdateItemAsync(item).WrapResult();
        public static IEnumerable<Transaction> ListTransactionsForItem(this IItemRepository repo, string itemId) => repo.ListTransactionsForItemAsync(itemId).WrapResult();
        public static ItemStatus GetStatusForItem(this IItemRepository repo, string itemId) => repo.GetStatusForItemAsync(itemId).WrapResult();
        public static IEnumerable<Fee> ListFeesForItem(this IItemRepository repo, string itemId) => repo.ListFeesForItemAsync(itemId).WrapResult();
        public static User GetBuyerForItem(this IItemRepository repo, string itemId) => repo.GetBuyerForItemAsync(itemId).WrapResult();
        public static User GetSellerForItem(this IItemRepository repo, string itemId) => repo.GetSellerForItemAsync(itemId).WrapResult();
        public static WireDetails GetWireDetailsForItem(this IItemRepository repo, string itemId) => repo.GetWireDetailsForItemAsync(itemId).WrapResult();
        public static BPayDetails GetBPayDetailsForItem(this IItemRepository repo, string itemId) => repo.GetBPayDetailsForItemAsync(itemId).WrapResult();
        public static Item MakePayment(this IItemRepository repo, string itemId, string accountId) => repo.MakePaymentAsync(itemId, accountId).WrapResult();
        public static Item RequestPayment(this IItemRepository repo, string itemId) => repo.RequestPaymentAsync(itemId).WrapResult();
        public static Item ReleasePayment(this IItemRepository repo, string itemId, int releaseAmount) => repo.ReleasePaymentAsync(itemId, releaseAmount).WrapResult();
        public static Item RequestRelease(this IItemRepository repo, string itemId, int releaseAmount) => repo.RequestReleaseAsync(itemId, releaseAmount).WrapResult();
        public static Item Cancel(this IItemRepository repo, string itemId) => repo.CancelAsync(itemId).WrapResult();
        public static Item AcknowledgeWire(this IItemRepository repo, string itemId) => repo.AcknowledgeWireAsync(itemId).WrapResult();
        public static Item AcknowledgePayPal(this IItemRepository repo, string itemId) => repo.AcknowledgePayPalAsync(itemId).WrapResult();
        public static Item RevertWire(this IItemRepository repo, string itemId) => repo.RevertWireAsync(itemId).WrapResult();
        public static Item RequestRefund(this IItemRepository repo, string itemId, string refundAmount, string refundMessage) => repo.RequestRefundAsync(itemId, refundAmount, refundMessage).WrapResult();
        public static Item Refund(this IItemRepository repo, string itemId, string refundAmount, string refundMessage) => repo.RefundAsync(itemId, refundAmount, refundMessage).WrapResult();
    }
}
