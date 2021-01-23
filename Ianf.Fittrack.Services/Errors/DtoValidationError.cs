namespace Ianf.Fittrack.Services.Errors 
{
    public record DtoValidationError(string ErrorMessage, string DtoType, string DtoProperty) : Error(ErrorMessage) { };
}
