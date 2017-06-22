using PromisePayDotNet.Dto;
using System.Threading.Tasks;
using PromisePayDotNet.Internals;
namespace PromisePayDotNet.Abstractions
{
    /// <summary>
    /// User api 
    /// https://reference.assemblypayments.com/#addresses
    /// Addresses are associated with a User and Companies. The User Address is required for Users that are assigned as sellers, and is optional for buyers. If the User is a principal of a Company, then the Company Address is also required.
    ///    While an Address is optional for users set as buyers, it is recommended that you capture and create all Users with an Address, as they may at some stage be a seller on your marketplace or platform.
    /// </summary>
    public interface IAddressRepository
    {
        /// <summary>
        /// Show details of a specific Address using a given id.
        /// </summary>
        /// <param name="addressId">address ID</param>
        /// <returns></returns>
        Task<Address> GetAddressByIdAsync(string addressId);
    }
    public static class AddressRepositoryExtensions
    {
        public static Address GetAddressById(this IAddressRepository repo, string addressId)
        {
            return repo.GetAddressByIdAsync(addressId).WrapResult();
        }
    }
}
