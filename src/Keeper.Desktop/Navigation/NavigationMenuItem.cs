using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Keeper.Desktop.Navigation
{
    public class NavigationMenuItem : HamburgerMenuItem
    {
        public PackIconMaterialLightKind IconKey { get; set; }
        public Uri View { get; set; }
    }
}
