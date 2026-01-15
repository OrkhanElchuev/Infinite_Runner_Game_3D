using UnityEngine;

public class Coin : PickUp
{
    protected override void OnPickup()
    {
        print("Add 100 points");
    }
}
