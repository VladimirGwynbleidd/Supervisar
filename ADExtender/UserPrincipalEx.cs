using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;

namespace ADExtender
{
    [DirectoryRdnPrefix("CN")]
    [DirectoryObjectClass("Person")]
    public class UserPrincipalEx : UserPrincipal
    {
        // Inplement the constructor using the base class constructor. 
        public UserPrincipalEx(PrincipalContext context)
            : base(context)
        { }

        // Implement the constructor with initialization parameters.    
        public UserPrincipalEx(PrincipalContext context,
                             string samAccountName,
                             string password,
                             bool enabled)
            : base(context, samAccountName, password, enabled)
        { }

        UserPrincipalExSearchFilter searchFilter;

        new public UserPrincipalExSearchFilter AdvancedSearchFilter
        {
            get
            {
                if (null == searchFilter)
                    searchFilter = new UserPrincipalExSearchFilter(this);

                return searchFilter;
            }
        }

        // Create the "Title" property.    
        [DirectoryProperty("title")]
        public string Title
        {
            get
            {
                if (ExtensionGet("title").Length != 1)
                    return string.Empty;

                return (string)ExtensionGet("title")[0];
            }
            set { ExtensionSet("title", value); }
        }

        // Create the "Department" property.
        [DirectoryProperty("department")]
        public string Department
        {
            get
            {
                if (ExtensionGet("department").Length != 1)
                    return string.Empty;

                return (string)ExtensionGet("department")[0];
            }
            set
            {
                ExtensionSet("department", value);
            }
        }

        // Implement the overloaded search method FindByIdentity.
        public static new UserPrincipalEx FindByIdentity(PrincipalContext context, string identityValue)
        {
            return (UserPrincipalEx)FindByIdentityWithType(context, typeof(UserPrincipalEx), identityValue);
        }

        // Implement the overloaded search method FindByIdentity. 
        public static new UserPrincipalEx FindByIdentity(PrincipalContext context, IdentityType identityType, string identityValue)
        {
            return (UserPrincipalEx)FindByIdentityWithType(context, typeof(UserPrincipalEx), identityType, identityValue);
        }
    }

    public class UserPrincipalExSearchFilter : AdvancedFilters
    {
        public UserPrincipalExSearchFilter(Principal p) : base(p) { }

        public void LogonCount(int value, MatchType mt)
        {
            this.AdvancedFilterSet("LogonCount", value, typeof(int), mt);
        }
    }
}
