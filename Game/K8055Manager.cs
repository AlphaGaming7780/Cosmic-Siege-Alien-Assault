using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8055Velleman.Game;

internal class K8055Manager
{

    public K8055Manager() 
    {
        K8055.OnConnectionChanged += OnConnectionChanged;
    }

    private void OnConnectionChanged()
    {
        if (K8055.IsConnected) K8055.SetAnalogChannel(1);
    }

}
