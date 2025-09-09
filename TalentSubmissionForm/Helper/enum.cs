
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TalentSubmissionForm.Helper
{
    public enum ApplicationStatus
    {
        [Display(Name = "on-hold")]
        onhold = 1,

        [Display(Name = "interview")]
        interview = 2,

        [Display(Name = "reject")]
        reject = 3,
    }
    public static class EnumHelper
    {
        public static IEnumerable<SelectListItem> GetEnumSelectList<ApplicationStatusEnum>() where ApplicationStatusEnum : Enum
        {
            return Enum.GetValues(typeof(ApplicationStatusEnum))
                       .Cast<ApplicationStatusEnum>()
                       .Select(e => new SelectListItem
                       {
                           Text = e.GetDisplayName(),
                           Value = e.ToString()
                       });
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            var displayAttribute = memberInfo?.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.Name ?? enumValue.ToString();
        }
        public static ApplicationStatusEnum? FromDisplayName<ApplicationStatusEnum>(string displayName) where ApplicationStatusEnum : struct, Enum
        {
            foreach (var field in typeof(ApplicationStatusEnum).GetFields())
            {
                var attr = field.GetCustomAttribute<DisplayAttribute>();
                if (attr?.Name?.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    return (ApplicationStatusEnum)field.GetValue(null)!;
                }
            }

            return null;
        }
    }
}
