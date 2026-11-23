namespace Ecommerce.config.errors;

public class NotFoundException(string msg) : AppException(msg)
{

}