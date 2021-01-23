namespace Ianf.Fittrack.Services.Errors 
{
    public record SqlError(string ErrorMessage) : Error(ErrorMessage) { };
}
