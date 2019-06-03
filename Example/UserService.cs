namespace RequestCaching.Example
{
    public class UserService
    {
        private readonly IRequestCache _cache;
        private readonly IUserRepository _userRepository;

        public UserService(IRequestCache cache, IUserRepository userRepository)
        {
            _cache = cache;
            _userRepository = userRepository;
        }

        public User RetrieveUserById(int userId)
        {
            var buildCacheKey = UserService.BuildCacheKey(userId);

            return _cache.RetrieveOrAdd(buildCacheKey, () => _userRepository.RetrieveUserBy(userId));
        }

        public void Delete(int userId)
        {
            var buildCacheKey = UserService.BuildCacheKey(userId);

            _userRepository.Delete(userId);
            _cache.Remove(BuildCacheKey(userId));
        }

        private static string BuildCacheKey(int userId)
        {
            return $"user_{userId}";
        }
    }

    public class User
    {
    }

    public interface IUserRepository
    {
        User RetrieveUserBy(int userId);
        void Delete(int userId);
    }
}