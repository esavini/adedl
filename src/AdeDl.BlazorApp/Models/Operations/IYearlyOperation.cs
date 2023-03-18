namespace AdeDl.BlazorApp.Models.Operations;

public interface IYearlyOperation : IOperation
{
    int Year { get; set; }
}