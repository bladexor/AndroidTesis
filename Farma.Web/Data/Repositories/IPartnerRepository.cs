using System.Threading.Tasks;
using Farma.Web.Data.Entities;

namespace Farma.Web.Data.Repositories
{
    public interface IPartnerRepository : IGenericRepository<Partner>
    {
        Partner GetPartnerByUserId(string uid);
        
        Task<Partner> GetPartnerPharmaciesAsync(int id);
    }
}