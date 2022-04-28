using System.Threading.Tasks;
using AppTrackerMVC.Auth.Models;

namespace AppTrackerMVC.Auth
{
    public interface IFirebaseAuthService
    {
        Task<FirebaseUser> Login(Credentials credentials);
        Task<FirebaseUser> Register(Registration registration);
    }
}