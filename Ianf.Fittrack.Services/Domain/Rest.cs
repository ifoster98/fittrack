namespace Ianf.Fittrack.Services.Domain
{
    public record Rest(PositiveInt Minutes, PositiveInt Seconds) 
    { 
        public Dto.Rest ToDto()  =>
            new Dto.Rest() {
                Minutes = this.Minutes.Value,
                Seconds = this.Seconds.Value
            };
    }
}