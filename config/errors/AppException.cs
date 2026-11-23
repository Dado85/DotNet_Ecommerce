namespace Ecommerce.config.errors;

public abstract class AppException(string msg) : Exception(msg)
{
}