using System.Text.Json;
using TalentSubmissionForm.Models;

namespace TalentSubmissionForm.Helper
{
    public static class SessionExtensions
    {
        public static void SetObject<UploadedFile>(this ISession session, string key, UploadedFile value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static UploadedFile? GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<UploadedFile>(value);
        }
    }
}
