using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour
{
    public Faction faction = Faction.None;

    private void Start()
    {
        List<iFactionControlReciever> factionControlReceivers = new List<iFactionControlReciever>();

        GetComponents(factionControlReceivers);

        for(int i = 0; i < factionControlReceivers.Count; ++i)
        {
            factionControlReceivers[i].InjectFactionControl(this);
        }
    }
}
