using Energy.Repositories.Entities;

namespace Energy.Repositories.Interfaces;

public interface IEgonRepository
{
    Task AddReadingAsync(DataReading dataReading);
}