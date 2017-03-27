using PromisePayDotNet.DTO;

namespace PromisePayDotNet.Abstractions
{
    public interface IAddressRepository
    {
        Address GetAddressById(string addressId);
    }
}
