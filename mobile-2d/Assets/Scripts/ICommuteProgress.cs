// Durable trip host. Vehicle presentation never owns a second wallet when a host is bound.
public interface ICommuteProgress
{
    int Money { get; }
    bool MissedBus { get; }
    bool CanBoard(bool returning,out string error);
    bool CommitArrival(bool returning,out string error);
}
