using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OwnedPartner
{
    public PartnerData data;
    public int level;
    public bool bookMark;

    public OwnedPartner(PartnerData partnerData)
    {
        data = partnerData;
    }

    public void EnrollBookMark()
    {
        bookMark = !bookMark;
    }
}
