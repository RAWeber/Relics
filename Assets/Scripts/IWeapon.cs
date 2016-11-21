using UnityEngine;
using System.Collections;

public interface IWeapon {

    void MainTriggerHold();
    void MainTriggerRelease();
    void SecondaryTriggerHold();
    void SecondaryTriggerRelease();
}
