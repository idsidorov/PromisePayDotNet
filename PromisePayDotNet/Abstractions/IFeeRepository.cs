using PromisePayDotNet.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using PromisePayDotNet.Internals;

namespace PromisePayDotNet.Abstractions
{
    public interface IFeeRepository
    {

        Task<IEnumerable<Fee>> ListFeesAsync();

        Task<Fee> GetFeeByIdAsync(string feeId);

        Task<Fee> CreateFeeAsync(Fee fee);
    }

    public static class IFeeRepositoryExtensions
    {
        public static IEnumerable<Fee> ListFees(this IFeeRepository repo) => repo.ListFeesAsync().WrapResult();
        public static Fee GetFeeById(this IFeeRepository repo, string feeId) => repo.GetFeeByIdAsync(feeId).WrapResult();
        public static Fee CreateFee(this IFeeRepository repo, Fee item) => repo.CreateFeeAsync(item).WrapResult();
    }
}
