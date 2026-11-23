using Ecommerce.Data;
using Ecommerce.Repositories.Interfaces;
using Ecommerce.Schema;

namespace Ecommerce.Repositories.Implementations;

public class ReviewRepository(AppDbContext context) : Repository<Review>(context), IReviewRepository
{

}