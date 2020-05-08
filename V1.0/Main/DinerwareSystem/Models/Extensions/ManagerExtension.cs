using System;
using Dinerware;
using Dinerware.WorkstationInterfaces;

namespace DinerwareSystem.Models
{
    public abstract class ManagerExtension : IManagerExtension
    {
        public string Author { get; } = "I am the author";
        public string Copyright { get; } = "Copyright Me, Now";
        public Guid LicenseFeature { get; } = Guid.Empty;
        public string Name { get; } = "My Addin That I Made";
        public bool OnBrain { get; } = false;
        public bool OnWorkstation { get; } = true;
        public abstract string displayName { get; }

        public abstract void ButtonPressed(object parentForm, User currentUser);
    }
}
