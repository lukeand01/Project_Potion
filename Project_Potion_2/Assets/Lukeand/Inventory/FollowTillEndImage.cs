using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowTillEndImage : FollowTillEnd
{
    Image baseImage;
    public void ChangeSprite(Sprite sprite)
    {
        if (baseImage == null) baseImage =  GetComponent<Image>();

        baseImage.sprite = sprite;
    }

}
