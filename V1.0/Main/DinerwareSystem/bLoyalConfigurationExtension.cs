using DinerwareSystem;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;

public class ConfigurationExtension : ManagerExtension
{
    #region Properties

    public override string displayName { get; } = "bLoyal MASTER SETTINGS";

    #endregion

    #region Public Methods

    public override void ButtonPressed(object parentForm, Dinerware.User currentUser)
    {        
        frmConfiguration frmConfiguration = new frmConfiguration();
        frmConfiguration.ShowDialog();
    }

    #endregion

}
