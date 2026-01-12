namespace Networking
{
    public interface ILanSession
    {
        void StartHost();
        void StopHost();
        void StartClient(string address);
        void StopClient();
    }
}
