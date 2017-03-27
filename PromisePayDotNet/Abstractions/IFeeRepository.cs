using PromisePayDotNet.DTO;
using System.Collections.Generic;

namespace PromisePayDotNet.Abstractions
{
    public interface IFeeRepository
    {

        IEnumerable<Fee> ListFees();

        Fee GetFeeById(string feeId);

        Fee CreateFee(Fee fee);

    }
}
