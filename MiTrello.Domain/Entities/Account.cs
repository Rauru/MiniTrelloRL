using System;
using System.Collections.Generic;

namespace MiniTrello.Domain.Entities
{
    public class Account : IEntity
    {
        private readonly IList<Organization> _organizations = new List<Organization>();
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string Token { get; set; }
        public virtual DateTime TokenTime { get; set; }
        private readonly IList<Activity> _activities =new List<Activity>();

        public virtual IEnumerable<Activity> Activities { get { return _activities; }}
        public virtual IEnumerable<Organization> Organizations{ get { return _organizations; } }

        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }

        
        public virtual void AddActivities(Activity activity)
        {
            _activities.Add(activity);
        }

        public virtual void AddOrganization(Organization organization)
        {
            if (!_organizations.Contains(organization))
            {
                _organizations.Add(organization);
            }
        }
        public virtual bool VerifyToken(Account account)
        {
            if (account.TokenTime<DateTime.Now.AddMinutes(-30))
            {
                return false;
            }
            return true;
        }

        public virtual Organization GetOrganizacionById(long id)
        {
            foreach (var org in _organizations)
            {
                if (org.Id == id)
                    return org;

            }
            return null;
        }

        public virtual Organization GetOrganizacionByTitle(string title)
        {
            foreach (var org in _organizations)
            {
                if (org.Title.Equals(title))
                    return org;
            }
            return null;
        }
    }
}