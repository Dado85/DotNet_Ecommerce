namespace Ecommerce.config.errors;

public class BadRequestException(string msg) : AppException(msg)
{

}